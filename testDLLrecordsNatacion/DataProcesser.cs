using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using testDLLrecordsNatacion.Model;
using testDLLrecordsNatacion.Model.Entities;

namespace testDLLrecordsNatacion
{
    /// <summary>
    /// Class where all operations regarding data processing will be done. 
    /// It will serve as a bridge between the view and the DB 
    /// to send the data clean and organized to the frontend.
    /// </summary>
    public class DataProcesser
    {
        private DbCommunication dbCon = new DbCommunication();
        private LenexXmlProcesser dllXmlProcesser = new LenexXmlProcesser();
        private ExcelRecordsImporter excelImporter = new ExcelRecordsImporter();

        /// <summary>
        /// Calls function that updates DB with XML file information.
        /// </summary>
        /// <param name="codeOfClub">Code of the club requesting the operation</param>
        public void ProcessXml(string codeOfClub) => dllXmlProcesser.ProcessXmlFiles(codeOfClub);


        /// <summary>
        /// Calls function that updates DB with Excel file information.
        /// </summary>
        /// <param name="codeOfClub">Code of the club requesting the operation</param>
        public void ReadExcel(string codeOfClub) => .ProcessXmlFiles(codeOfClub);

        /// <summary>
        /// Fetches all data about Athletes, Events and results shown in the default view of the site.
        /// </summary>
        /// <returns>List with all the data</returns>
        public Dictionary<string, object> FetchAllData()
        {
            //TODO: open db connecton here?
            List<Athlete> updatedAthletes = dbCon.SelectAllAthletes();
            List<Event> updatedEvents = dbCon.SelectAllEvents();
            List<Result> updatedResults = dbCon.SelectAllResults();
            //TODO: close db connection here?

            //Group all of the data to send it to the frontend
            Dictionary<string, object> updatedObjects = 
                new Dictionary<string, object> {
                    {"Athletes", updatedAthletes }, 
                    {"Events", updatedEvents }, 
                    {"Results", updatedResults} 
                };
            return updatedObjects;
        }


    }
}
