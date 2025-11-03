using ClosedXML.Excel;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using testDLLrecordsNatacion.Model;
using testDLLrecordsNatacion.Model.Entities;

namespace testDLLrecordsNatacion
{
    /// <summary>
    /// Processes Excel file of the club containing the current stablished records 
    /// and imports them into the Records table of the DB.
    /// </summary>
    internal class ExcelRecordsImporter
    {
        private DbCommunication dbCon = new DbCommunication();
        private readonly string ResourcesFolderPath = "C:\\Users\\crist\\source\\repos\\testDLLrecordsNatacion\\testDLLrecordsNatacion\\Resources\\ExcelsRecordsTenis\\";

        /// <summary>
        /// Reads and processes excel files containing the records from the club.
        /// Creates a list of records from each row of an excel file.
        /// </summary>
        /// <returns>List of all records imported from XML</returns>
        public List<Record> ReadExcelAndCreateRecordObjects(IFormFile excelFile)
        {
            List<Record> recordsToAdd = new List<Record>();

            //TODO: might have to change the way to obtain the files, depending on project requirements
            //string[] ExcelFiles = System.IO.Directory.GetFiles(ResourcesFolderPath);
            //foreach (var ExcelFile in ExcelFiles)
            //{
            //var dataTable = new DataTable();

            using (var stream = excelFile.OpenReadStream())
            {
                using (var workbook = new XLWorkbook(stream))
                {
                    IXLWorksheet worksheet = workbook.Worksheet(0); //"p50m");

                   
                }
            }

            //}
            return recordsToAdd;

        }



    }
}
