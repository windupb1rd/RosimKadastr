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
        private Dictionary<string, string> theNumbers = new Dictionary<string, string>();
        private Dictionary<string, string> doubles = new Dictionary<string, string>();

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
                
                foreach (var item in numberObjectsFromExcel)
                {
                    string number = item.Value?.ToString();
                    string address = item.Address?.ToString();
                    if (number != null && Regex.IsMatch(number, @"\d+:\d+:\d+:\d+"))
                    {
                        theNumbers[address] = number;
                    }
                }
                GetDuplicatesAdresses();
                return String.Join("\r\n", theNumbers.Values);
            }
        }

        public void GetDuplicatesAdresses()
        {
            //var distinctNumbers = theNumbers.Values.Distinct().ToList();
            var doubles = theNumbers.GroupBy(x => x.Value.Count() > 1);
        }
        public void GetNewExcelFileWithoutDuplicates()
        {

        }
    }
}
