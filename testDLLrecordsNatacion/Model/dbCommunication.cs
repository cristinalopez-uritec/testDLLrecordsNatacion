using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using testDLLrecordsNatacion.Model.Entities;
using testDLLrecordsNatacion.Model;
using NLog;
using System.Configuration;

namespace testDLLrecordsNatacion.Model
{
    /// <summary>
    /// Where all Database operations are going to be performed. 
    /// </summary>
    internal class DbCommunication
    {
        //Access connection string stored in WebConfig file of the project containing the DLL
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        private readonly Parser parser = new Parser();

        #region Athlete 
        /// <summary>
        /// Searches if an athlete exists in the DB by its full name.
        /// </summary>
        /// <param name="athleteFullName">Full name of the athlete</param>
        /// <returns>The athelete or null if no matches were found</returns>
        public Athlete SearchAthleteByName(string athleteFullName)
        {
            string query = "SELECT * FROM Athlete WHERE FullName = @FullName";
            Athlete athlete = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@FullName", athleteFullName);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        athlete = parser.DbReaderToAthlete(reader);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + "\n\n" + ex.StackTrace);
                }
                finally
                {
                    reader.Close();
                }
            }
            return athlete;
        }

        /// <summary>
        /// Gets athlete info by its ID if it exists in the DB
        /// </summary>
        /// <param name="athleteId">Id of the athlete</param>
        /// <returns>The athelete or null if no matches were found</returns>
        public Athlete SelectAthleteById(int athleteId)
        {
            string query = "SELECT * FROM Athlete WHERE id = @id";
            Athlete athlete = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", athleteId);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        athlete = parser.DbReaderToAthlete(reader);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + "\n\n" + ex.StackTrace);
                }
                finally
                {
                    reader.Close();
                }
            }
            return athlete; 
        }

        /// <summary>
        /// Gets all the existing athletes in the DB
        /// </summary>
        /// <returns>List of existing athletes</returns>
        public List<Athlete> SelectAllAthletes()
        {
            string query = "SELECT * FROM Athlete";
            List<Athlete> atletas = new List<Athlete>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        atletas.Add(parser.DbReaderToAthlete(reader));
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message + "\n\n" + ex.StackTrace);
                    Log.Instance.Fatal("Failed to fetch all athletes", ex, ex.StackTrace);
                }
                finally
                {
                    reader.Close();
                }
            };
            return atletas;
        }

        /// <summary>
        /// Adds a new athlete into the DB.
        /// NOTE: this function does not check if the athlete already exists before adding it.
        /// </summary>
        /// <param name="athlete">the Athlete to insert</param>
        /// <returns>Id of the athlete inserted, -1 if insert failed</returns>
        public int InsertAthlete(Athlete athlete)
        {
            int newAthleteId = -1;
            string query = "INSERT INTO Athlete (FullName,Birthdate,Gender,Nation,License,ClubCode,ClubName,ClubShortName) " +
                            "VALUES (@fullName,@birthdate,@gender,@nation,@license,@clubCode,@clubName,@clubShortName); " +
                            "SELECT SCOPE_IDENTITY();";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    parser.AthleteToSqlCommandParams(athlete,command);

                    connection.Open();
                    newAthleteId = Convert.ToInt32(command.ExecuteScalar()); //return the Id of the record inserted

                    //Check Error
                    if (newAthleteId < 0)
                    {
                        Console.WriteLine("Error inserting data into Database!");
                        return -1;
                    }   
                }
            }

            return newAthleteId;
        }

        //TODO: update athlete (when already existed on BD due to Excel import but is lacking data, update it with the new data provided by XML)
        #endregion

        #region Event
        /// <summary>
        /// Adds a new Event into the DB.
        /// NOTE: this function does not check if the event already exists before adding it.
        /// </summary>
        /// <param name="evento">the Event to insert</param>
        /// <returns>Id of the event inserted, -1 if insert failed</returns>
        public int InsertEvent(Event evento)
        {
            int newEventId = -1;
            string query = "INSERT INTO Event (MeetName,MeetDate,Nation,City,Status,PoolLength,SessionNum,SessionName,GenderCategory,EventRound,EventCourse,SwimDistance,SwimStroke,SwimRelayCount) " +
                            "VALUES (@MeetName,@MeetDate,@Nation,@City,@Status,@PoolLength,@SessionNum,@SessionName,@GenderCategory,@EventRound,@EventCourse,@SwimDistance,@SwimStroke,@SwimRelayCount); " +
                            "SELECT SCOPE_IDENTITY();";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    parser.EventToSqlCommandParams(evento,command);

                    connection.Open();
                    newEventId = Convert.ToInt32(command.ExecuteScalar()); //return the Id of the record inserted

                    // Check Error
                    if (newEventId < 0)
                    {
                        Console.WriteLine("Error inserting data into Database!");
                        return -1;
                    }
                }
            }

            return newEventId;
        }

        /// <summary>
        /// Gets all the existing events in the DB
        /// </summary>
        /// <returns>List of all existing events</returns>
        public List<Event> SelectAllEvents()
        {
            string query = "SELECT * FROM recordsNatacion.dbo.[Event]";
            List<Event> events = new List<Event>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        events.Add(parser.DbReaderToEvent(reader));
                    }
                }
                catch (Exception ex)
                {
                    Log.Instance.Fatal("Failed to fetch all Events", ex, ex.StackTrace);
                    Console.WriteLine(ex.Message + "\n\n" + ex.StackTrace);
                }
                finally
                {
                    reader.Close();
                }
            }

            return events;
        }
        #endregion


        #region Results
        /// <summary>
        /// Adds a new Result into the DB.
        /// NOTE: this function does not check if the Result already exists before adding it.
        /// </summary>
        /// <param name="result">the Result to insert</param>
        /// <returns>Id of the result inserted, -1 if insert failed</returns>
        public int InsertResult(Result result)
        {
            int newResultId = -1;
            string query = "INSERT INTO Result (SplitDistance,SwimTime,Points,IsWaScoring,EntryTime,Comment,AgeGroupMaxAge,AgeGroupMinAge,EventId,AthleteId) " +
                            $"VALUES (@SplitDistance,@SwimTime,@Points,@IsWaScoring,@EntryTime,@Comment,@AgeGroupMaxAge,@AgeGroupMinAge,@EventId,@AthleteId); " +
                            "SELECT SCOPE_IDENTITY();";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    parser.ResultToSqlCommandParams(result, command);

                    connection.Open();
                    newResultId = Convert.ToInt32(command.ExecuteScalar()); //return the Id of the record inserted

                    // Check Error
                    if (newResultId < 0)
                    {
                        Console.WriteLine("Error inserting data into Database!");
                        return -1;
                    }
                }
            }

            return newResultId;
        }

        /// <summary>
        /// Gets all the existing Results in the DB
        /// </summary>
        /// <returns>List of all existing Results</returns>
        public List<Result> SelectAllResults()
        {
            string query = "SELECT * FROM Result";
            List<Result> results = new List<Result>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        results.Add(parser.DbReaderToResult(reader));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + "\n\n" + ex.StackTrace);
                }
                finally
                {
                    reader.Close();
                }
            }

            return results;
        }
        #endregion

        #region Records
        public int InsertRecord(Record record)
        {
            int newRecordId = -1;
            string query = "INSERT INTO Record (Position,MeetStatus,RecordType,AgeCategory,SwimTime,SwimCourse,SwimDistance,SwimStroke,Points,ResultId,AthleteId) " +
                            $"VALUES (@Position,@MeetStatus,@RecordType,@AgeCategory,@SwimTime,@SwimCourse,@SwimDistance,@SwimStroke,@Points,@ResultId,@AthleteId); " +
                            "SELECT SCOPE_IDENTITY();";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    parser.RecordToSqlCommandParams(record, command);

                    connection.Open();
                    newRecordId = Convert.ToInt32(command.ExecuteScalar()); //return the Id of the record inserted

                    // Check Error
                    if (newRecordId < 0)
                    {
                        Console.WriteLine("Error inserting data into Database!");
                        return -1;
                    }
                }
            }

            return newRecordId;
        }
        #endregion
    }
}
