using ClosedXML.Excel;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using testDLLrecordsNatacion.Model;
using testDLLrecordsNatacion.Model.Entities;

namespace testDLLrecordsNatacion
{
    /// <summary>
    /// Processes Excel file of the club containing the current stablished records 
    /// and imports them into the Records table of the DB.
    /// </summary>
    internal class ExcelRecordsReader
    {
        private DbOperations dbQueries = new DbOperations();

        /// <summary>
        /// Reads and processes excel files containing the records from the club.
        /// Creates a list of records from each row of an excel file.
        /// </summary>
        /// <returns>List of all records imported from XML</returns>
        public List<Record> ImportDataFromExcel(string codeOfClub, string filePath)
        {
            List<Record> recordsToAdd = new List<Record>();
            List<Athlete> athletesInvolved = new List<Athlete>();
            int colAgeCategory = 1;
            int colAthleteLicense = 2;
            int colAthleteName = 3;
            int colRecordDate = 4;
            int colSwimStroke = 5;
            int colSwimDistance = 6;
            int colSwimCourse = 7; //aka, pool length
            int colRecordType = 8;
            int colSwimTime = 9;
            int colPointsFina = 10;

            //XLWorkbook workbook1 = new XLWorkbook(filePath);
            //IXLWorksheet ws2 = workbook1.Worksheet(1); //"p50m");
            using (var stream = new MemoryStream(File.ReadAllBytes(filePath).ToArray()))
            {
                XLWorkbook workbook = new XLWorkbook(stream);
                IXLWorksheet ws = workbook.Worksheet(1); //1st one

                foreach (var row in ws.RowsUsed())
                {
                    if (row.RowNumber() != 1) //skip headers
                    {
                        Record record = new Record();
                        record.Athlete = new Athlete();
                        //record.Result = new Result();

                        bool isRowEmpty = row.CellsUsed().Select(cell => cell.Value.ToString() != "").Count() > 0 ? false : true;
                        if (isRowEmpty) break;

                        string ageCategoryValue = row.Cell(colAgeCategory).Value.ToString().Trim().Split('.')[1];
                        string athleteLicenseValue = row.Cell(colAthleteLicense).Value.ToString().Trim() != ""
                                                        ? row.Cell(colAthleteLicense).Value.ToString().Trim() 
                                                        : null;
                        string athleteFullNameValue = Utils.CapitalizeString(row.Cell(colAthleteName).Value.ToString().Trim());
                        DateTime recordDateValue = DateTime.Parse(row.Cell(colRecordDate).Value.ToString().Trim());
                        string swimStrokeValue = Utils.SwimStrokeTranslatorEspToEng(row.Cell(colSwimStroke).Value.ToString().Trim());
                        string swimDistanceValueStr = row.Cell(colSwimDistance).Value.ToString().Trim().Split('.')[0];
                        int swimDistanceValue = Int32.Parse(swimDistanceValueStr.Substring(0, swimDistanceValueStr.Length - 1));
                        int poolLengthValue = Int32.Parse(row.Cell(colSwimCourse).Value.ToString().Trim());
                        string recordTypeValue = row.Cell(colRecordType).Value.ToString().Trim();
                        string swimTimeValue = row.Cell(colSwimTime).Value.ToString().Trim();
                        int pointsFinaValue = Int32.Parse(row.Cell(colPointsFina).Value.ToString().Trim());

                        record.AgeCategory = ageCategoryValue;
                        record.Athlete.License = athleteLicenseValue;
                        record.Athlete.FullName = Utils.RemoveSpanishAccentsString(athleteFullNameValue);
                        record.RecordDate = recordDateValue;
                        record.SwimStroke = swimStrokeValue;
                        record.SwimDistance = swimDistanceValue;
                        record.SwimCourse = poolLengthValue == 50 ? "LCM" : poolLengthValue == 25 ? "SCM" : "NO INFO";
                        record.RecordType = recordTypeValue == "Parcial" ? "Partial" : recordTypeValue;
                        record.SwimTime = swimTimeValue;
                        record.Points = pointsFinaValue;

                        record.MeetStatus = "OFFICIAL";
                        record.Position = -1;
                        record.AthleteId = -1;
                        record.ResultId = -1;
                        record.Result = null;

                        if(athletesInvolved.Where(a => a.FullName == record.Athlete.FullName).Count() == 0) athletesInvolved.Add(record.Athlete);
                        recordsToAdd.Add(record);
                    }
                }

                workbook.Dispose(); //stop reading Excel
            }
            //TODO: delete the Excel file from the Temp folder


            //TODO: order the records in the list and assign their positions and shit, use the same "updateRecords" function as when upload XML (function should be in DB communication)

            //consultar los atletas involucrados para obtener los ids si existen o insertarlos en DB y asociarlos
            recordsToAdd = AddInvolvedAthletesExcel(athletesInvolved,recordsToAdd);

            return recordsToAdd;
        }

        /// <summary>
        /// Checks if all of the involved athletes exist in the database and, if not, it creates them.
        /// Then, assigns the Id of the athlete to the record they belong to. 
        /// </summary>
        /// <param name="athletesInvolved">A list of all of the athletes that have at least one of the records</param>
        /// <param name="recordsToAdd">the imported records that we want to add into the DB</param>
        /// <returns>The records to add to the DB with their corresponding atheltes DB</returns>
        private List<Record> AddInvolvedAthletesExcel(List<Athlete> athletesInvolved, List<Record> recordsToAdd)
        {
            List<Record> updatedRecordsToAdd = new List<Record>();
            foreach (Athlete athlete in athletesInvolved)
            {
                int athleteId = dbQueries.SearchAndInsertAthlete(athlete, false);

                //Now insert the events and the results
                if (athleteId > 0)
                {
                    List<Record> athleteRecords = recordsToAdd.Where(r => r.Athlete.FullName == athlete.FullName).ToList();
                    athleteRecords.ForEach(rec => rec.AthleteId = athleteId);
                    updatedRecordsToAdd.AddRange(athleteRecords);
                }
                else
                {
                    Log.Instance.Fatal("AddInvolvedAthletesExcel failed", $"athleteId was {athleteId}");
                }
            }
            return updatedRecordsToAdd;
        }

    }
}