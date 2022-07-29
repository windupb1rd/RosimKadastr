using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;
using System.Text.RegularExpressions;
using System.IO;

namespace RosimKadastr
{
    internal enum Columns
    {
        A = 1,
        B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z
    }

    internal class ExcelDocColumn
    {
        private readonly string _pathToFile;
        private readonly Columns _column;
        private Dictionary<string, List<string>> theNumbers = new Dictionary<string, List<string>>();


        public ExcelDocColumn(string path, Columns col)
        {
            _pathToFile = path;
            _column = col;
        }
        public string GetExcelColumnData()
        {
            FileInfo fileInfo = new FileInfo(_pathToFile);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage excelPackage = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet sheet = excelPackage.Workbook.Worksheets[0];

                var numberObjectsFromExcel = sheet.Columns[(int)_column].Range.ToList();
                var allEntries = new List<string>();

                foreach (var item in numberObjectsFromExcel)
                {
                    string number = item.Value?.ToString();
                    string address = item.Address?.ToString();
                    
                    if (number != null && Regex.IsMatch(number, @"\d+:\d+:\d+:\d+"))
                    {
                        allEntries.Add(number);
                        if (!theNumbers.ContainsKey(number))
                            theNumbers[number] = new List<string>();
                        
                        theNumbers[number].Add(address); 
                    }
                }
                return String.Join("\r\n", allEntries);
            }
        }      

        public Dictionary<string, List<string>> GetDuplicates() => theNumbers.Where(x => x.Value.Count > 1).ToDictionary(x => x.Key, y => y.Value);

        public void DeleteDuplicates()
        {
            var itemsToDelete = new List<string>();
            var dups = GetDuplicates();
            foreach (var kvp in dups)
            {
                kvp.Value.RemoveAt(0);
                foreach (var item in kvp.Value)
                {
                    itemsToDelete.Add(item.Substring(1));

                }

            }


            //public void GetNewExcelFileWithoutDuplicates()
            //{
            //    if (!theNumbers.ContainsKey(number))
            //        theNumbers[number] = new List<string>();

            //    theNumbers[number].Add(address);

        }
}
