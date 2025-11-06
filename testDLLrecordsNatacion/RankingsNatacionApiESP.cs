using System.Collections.Generic;
using testDLLrecordsNatacion.Model;
using testDLLrecordsNatacion.Model.Entities;

namespace testDLLrecordsNatacion
{
    /// <summary>
    /// Donde todas las operaciones de procesamiento de datos se llevarán a cabo. 
    /// Funcionará como un puente entre la vista y la base de datos, 
    /// para mandar los datos limpios y organizados al frontend. 
    /// </summary>
    public class RankingsNatacionApiESP
    {
        private OperacionesBD consultasBD = new OperacionesBD();
        private ProcesadorXmlLenex procesadorXmlDLL = new ProcesadorXmlLenex();
        private LectorRegitrosExcel lectorExcelDLL = new LectorRegitrosExcel();

        /// <summary>
        /// Llama a la función que actualiza la base de datos con la información del XML
        /// </summary>
        /// <param name="codigoClub">Codigo del club que solicita la operación</param>
        public void ProcesarXml(string codigoClub) => procesadorXmlDLL.ProcesarAchivosXml(codigoClub);


        /// <summary>
        /// Llama a la función que actualiza la base de datos con la información del Excel.
        /// </summary>
        /// <param name="codigoClub">Codigo del club que solicita la operación</param>
        public List<Record> ImportDataFromExcel(string codigoClub, string filePath)
        {
            List<Record> recordsToInsert = lectorExcelDLL.ImportDataFromExcel(codeOfClub, filePath);

            //TODO: insertRecordsInDb --> compare with results to see if they need to be added??? idk
            foreach (Record record in recordsToInsert)
            {
                //TODO: make an InsertAllRecords query for better optimization
                consultasBD.InsertRecord(record);
            }

            return consultasBD.SelectAllRecords();
        }

        /// <summary>
        /// Fetches all data about Athletes, Events and results shown in the default view of the site.
        /// </summary>
        /// <returns>List with all the data</returns>
        public Dictionary<string, object> FetchAllData()
        {
            //TODO: open db connecton here?
            List<Athlete> updatedAthletes = consultasBD.SelectAllAthletes();
            List<Event> updatedEvents = consultasBD.SelectAllEvents();
            List<Result> updatedResults = consultasBD.SelectAllResults();
            List<Record> updatedRecords = consultasBD.SelectAllRecords();
            //TODO: close db connection here?

            //Group all of the data to send it to the frontend
            Dictionary<string, object> updatedObjects = 
                new Dictionary<string, object> {
                    {"Athletes", updatedAthletes }, 
                    {"Events", updatedEvents }, 
                    {"Results", updatedResults},
                    {"Records", updatedRecords} 
                };
            return updatedObjects;
        }

        public void InsertarRecordsEsp() => consultasBD.InsertarRecordsPersonalesMarcas();
    }
}
