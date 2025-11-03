
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using testDLLrecordsNatacion.Model;
using testDLLrecordsNatacion.Model.Entities;

namespace testDLLrecordsNatacion
{
    /// <summary>
    /// Functions neccessary to read the xml with the swimming results and save them to the database so that they can be displayed
    /// </summary>
    internal class LenexXmlProcesser
    {
        private DbCommunication dbCon = new DbCommunication();
        private readonly string ResourcesFolderPath = "C:\\Users\\crist\\source\\repos\\testDLLrecordsNatacion\\testDLLrecordsNatacion\\Resources\\XmlResultsMeets\\";

        /// <summary>
        /// Reads the XML files in the resources folders and updates the database with the info 
        /// relative to the atletes that belong to the club requesting the info
        /// </summary>
        /// <param name="codeOfClub">Code of the club of interest. 
        /// Only the information that is related to it will be extracted from the XML.</param>
        /// <returns>An updated list of all the Athletes existing in the DB</returns>
        public void ProcessXmlFiles(string codeOfClub)
        {
            List<Athlete> athletesFromClub = new List<Athlete>();
            List<Result> resultsToAdd = new List<Result>();
            List<Event> eventsToAdd = new List<Event>();
            List<string> eventNodeIdsOfEventObjsToAdd = new List<string>();

            //TODO: might have to change the way to obtain the files, depending on project requirements
            string[] xmlFilePaths = Directory.GetFiles(ResourcesFolderPath); //Directory.GetFiles("./Resources/", ".xml");

            //Process all of the XML files at our disposal
            foreach (var filePath in xmlFilePaths)
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(filePath); //xmlFilePaths[4]);

                XmlNodeList clubNodes = doc.DocumentElement.SelectNodes("/LENEX/MEETS/MEET/CLUBS/CLUB");
                foreach (XmlNode clubNode in clubNodes)
                {
                    string clubNodeCode = clubNode.Attributes["code"] != null
                                            ? clubNode.Attributes["code"].InnerText
                                            : clubNode.Attributes["clubid"] != null
                                                ? clubNode.Attributes["clubid"].InnerText
                                                : null;
                    if (clubNodeCode != null && clubNodeCode == codeOfClub)
                    {
                        XmlNodeList athleteNodes = clubNode.ChildNodes[0].ChildNodes;
                        foreach (XmlNode athleteNode in athleteNodes)
                        {
                            //Get the information of the athlete
                            Athlete athlete = new Athlete();
                            string firstName = athleteNode.Attributes["firstname"].InnerText;
                            string lastName = athleteNode.Attributes["lastname"].InnerText;
                            athlete.FullName = lastName + ", " + firstName;
                            athlete.Birthdate = DateTime.Parse(athleteNode.Attributes["birthdate"].InnerText);
                            athlete.Gender = athleteNode.Attributes["gender"].InnerText;
                            athlete.Nation = athleteNode.Attributes["nation"].InnerText;
                            athlete.License = athleteNode.Attributes["license"].InnerText;
                            athlete.ClubCode = Int32.Parse(clubNode.Attributes["code"].InnerText ?? "0");
                            athlete.ClubName = clubNode.Attributes["name"].InnerText;
                            athlete.ClubShortName = clubNode.Attributes["shortname"].InnerText;
                            athletesFromClub.Add(athlete);

                            //Get all the results of that athlete in that competition
                            XmlNodeList resultNodes = athleteNode.ChildNodes[0].ChildNodes;
                            foreach (XmlNode resultNode in resultNodes)
                            {
                                //Create the Event object belonging to that Result
                                string eventId = resultNode.Attributes["eventid"].InnerText;
                                XmlNode eventNode = doc.DocumentElement.SelectSingleNode($"/LENEX/MEETS/MEET/SESSIONS/SESSION/EVENTS/EVENT[@eventid={eventId}]");
                                XmlNode swimStyleNode = eventNode.ChildNodes[0];
                                XmlNode sessionNode = eventNode.ParentNode.ParentNode;
                                XmlNode meetNode = sessionNode.ParentNode.ParentNode;
                                XmlNode pointTableNode = meetNode.SelectSingleNode("POINTTABLE");
                                int isWaScoring = pointTableNode.Attributes["name"].InnerText == "AQUA Point Scoring" ? 1 : 0;
                                string eventCourse = resultNode.Attributes["entrycourse"] != null
                                                        ? resultNode.Attributes["entrycourse"].InnerText
                                                        : meetNode.Attributes["course"] != null
                                                            ? meetNode.Attributes["course"].InnerText
                                                            : "LCM"; //LCM is the most standard so we set that as default

                                Event eventObj = new Event();
                                eventObj.MeetName = meetNode.Attributes["name"].InnerText;
                                eventObj.MeetDate = DateTime.Parse(meetNode.SelectSingleNode("AGEDATE").Attributes["value"].InnerText);
                                eventObj.Nation = meetNode.Attributes["nation"].InnerText;
                                eventObj.City = meetNode.Attributes["city"].InnerText;
                                eventObj.Status = eventNode.Attributes["status"].InnerText;
                                eventObj.PoolLength = eventCourse == "LCM" ? 50 : 25;
                                eventObj.SessionNum = Int32.Parse(sessionNode.Attributes["number"].InnerText ?? "0");
                                eventObj.SessionName = sessionNode.Attributes["name"].InnerText;
                                eventObj.GenderCategory = eventNode.Attributes["gender"].InnerText;
                                eventObj.EventRound = eventNode.Attributes["round"].InnerText;
                                eventObj.EventCourse = eventCourse;
                                eventObj.SwimDistance = Int32.Parse(swimStyleNode.Attributes["distance"].InnerText ?? "0");
                                eventObj.SwimStroke = swimStyleNode.Attributes["stroke"].InnerText;
                                eventObj.SwimRelayCount = Int32.Parse(swimStyleNode.Attributes["relaycount"].InnerText ?? "0");
                                //check that the event has not already been added to the eventsToAdd list to avoid inserting duplicates in db
                                if (!eventNodeIdsOfEventObjsToAdd.Contains(eventId)) eventsToAdd.Add(eventObj);

                                //Get the Age Group of the result
                                string resultId = resultNode.Attributes["resultid"].InnerText;
                                XmlNode resultAgeGroupNode = doc.DocumentElement
                                    .SelectSingleNode($"/LENEX/MEETS/MEET/SESSIONS/SESSION/EVENTS/EVENT[@eventid={eventId}]/AGEGROUPS/AGEGROUP/RANKINGS/RANKING[@resultid={resultId}]")
                                    .ParentNode.ParentNode;
                                int resultAgeGroupMax = Int32.Parse(resultAgeGroupNode.Attributes["agemax"].InnerText ?? "-1");
                                int resultAgeGroupMin = Int32.Parse(resultAgeGroupNode.Attributes["agemin"].InnerText ?? "-1");

                                //Create the Result object/s (splits are turned into result objects too)
                                Result result = new Result();
                                result.SplitDistance = Int32.Parse(resultNode.Attributes["distance"] != null
                                                                    ? resultNode.Attributes["distance"].InnerText
                                                                    : "0");
                                result.SwimTime = resultNode.Attributes["swimtime"].InnerText;
                                result.EntryTime = resultNode.Attributes["entrytime"].InnerText;
                                result.Points = Int32.Parse(resultNode.Attributes["points"].InnerText ?? "0");
                                result.IsWaScoring = isWaScoring; //World Aquatics scoring (aka, according to FINA)
                                result.Comment = resultNode.Attributes["comment"] != null
                                                    ? resultNode.Attributes["comment"].InnerText
                                                    : null;
                                result.AgeGroupMaxAge = resultAgeGroupMax;
                                result.AgeGroupMinAge = resultAgeGroupMin;
                                result.EventId = eventObj.Id; //set the id after inserting the event in the DB
                                result.Event = eventObj;
                                result.AthleteId = athlete.Id; //set the id after inserting the event in the DB
                                result.Athlete = athlete;

                                //añadir las splits a los results
                                if (resultNode.HasChildNodes)
                                {
                                    int splitDistance = 0;

                                    //Create more Result objects for each split
                                    XmlNodeList splitsNodes = resultNode.ChildNodes[0].ChildNodes;
                                    foreach (XmlNode splitNode in splitsNodes)
                                    {
                                        Result splitResult = new Result();
                                        splitResult.SplitDistance = Int32.Parse(splitNode.Attributes["distance"].InnerText ?? "0");
                                        splitResult.SwimTime = splitNode.Attributes["swimtime"].InnerText;
                                        splitResult.EntryTime = null;
                                        splitResult.Points = -1;
                                        splitResult.IsWaScoring = isWaScoring;
                                        splitResult.Comment = splitNode.Attributes["comment"] != null
                                                                ? splitNode.Attributes["comment"].InnerText
                                                                : null;
                                        splitResult.AgeGroupMaxAge = resultAgeGroupMax;
                                        splitResult.AgeGroupMinAge = resultAgeGroupMin;
                                        splitResult.EventId = eventObj.Id;
                                        splitResult.Event = eventObj;
                                        splitResult.AthleteId = athlete.Id;
                                        splitResult.Athlete = athlete;

                                        resultsToAdd.Add(splitResult);

                                        splitDistance = splitResult.SplitDistance;
                                    }
                                    //change split distance of original result to save it as the last split
                                    result.SplitDistance = splitDistance + eventObj.PoolLength; //calc distance of last split
                                }
                                resultsToAdd.Add(result);
                            }

                        }
                    }
                }
            }

            //Now we update the DB accordingly
            UpdateDbWithXmlInfo(athletesFromClub, eventsToAdd, resultsToAdd);
        }

        /// <summary>
        /// Takes the objects built from the info on the XML (relative to the club of interest)
        /// and updates the DB with it, making all insertions and updates necessary.
        /// </summary>
        /// <param name="athletesFromClub">List of Athlete objects found in the XML that belong to the club of interest</param>
        /// <param name="eventsToInsert">List of Events that the athletesFound have participated in</param>
        /// <param name="resultsToInsert">List of Results that the athletesFound have gotten in the events</param>
        /// <returns>An updated list of all the Athletes existing in the DB</returns>
        private void UpdateDbWithXmlInfo(List<Athlete> athletesFromClub, List<Event> eventsToInsert, List<Result> resultsToInsert)
        {
            //TODO: open DB connection here --> once the usings have been removed

            //insert new athletes into the database
            foreach (Athlete athlete in athletesFromClub)
            {
                Athlete athleteExists = dbCon.SearchAthleteByName(athlete.FullName);
                int athleteId = 0;

                //Insert the athlete if needed
                if (athleteExists == null)
                {
                    athleteId = dbCon.InsertAthlete(athlete);
                }
                else
                {
                    athleteId = athleteExists.Id;
                    //TODO: if it already exists, check if it has empty fields that can be updated
                }

                //Now insert the events and the results
                if (athleteId != 0)
                {
                    //filter the results and events not related to the athlete
                    List<Result> athleteResults = resultsToInsert.Where(x => x.Athlete.Equals(athlete)).ToList();
                    List<Event> athleteEvents = athleteResults.Select(x => x.Event).Distinct().ToList();

                    foreach (Event evento in athleteEvents)
                    {
                        int eventId = dbCon.InsertEvent(evento);

                        //select only the filtered results after inserting the athlete and the event so we can assign the FKs 
                        List<Result> eventResultsOfTheAthlete = athleteResults.Where(x => x.Event.Equals(evento)).ToList();
                        foreach (Result result in eventResultsOfTheAthlete)
                        {
                            result.AthleteId = athleteId;
                            result.EventId = eventId;
                            dbCon.InsertResult(result);
                        }
                    }
                }

                //TODO: now check if any of those results surpass any records

            }
            //TODO: close DB connection here --> once the usings have been removed
        }

    }
}
