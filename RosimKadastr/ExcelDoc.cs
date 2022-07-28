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

    internal class ExcelDoc
    {
        private readonly string _pathToFile;
        private readonly Columns _column;

        public ExcelDoc(string path, Columns col)
        {
            _pathToFile = path;
            _column = col;
        }
        public string GetExcelColumn()
        {
            FileInfo fileInfo = new FileInfo(_pathToFile);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage excelPackage = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet sheet = excelPackage.Workbook.Worksheets[0];

                var numberObjectsFromExcel = sheet.Columns[(int)_column].Range.ToList();
                List<string> theNumbers = new List<string>();
                var textBoxOutput = new StringBuilder();
                foreach (var item in numberObjectsFromExcel)
                {
                    string number = item.Value?.ToString();
                    if (number != null && Regex.IsMatch(number, @"\d+:\d+:\d+:\d+"))
                    {
                        theNumbers.Add(number);
                    }
                }
                return String.Join("\r\n", theNumbers);
            }
        }
    }
}
