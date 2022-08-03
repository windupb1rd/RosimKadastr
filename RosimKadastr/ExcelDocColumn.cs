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
        private readonly Columns _column;
        private readonly ExcelPackage _excelPackage;
        private Dictionary<string, List<int>> _theNumbers = new Dictionary<string, List<int>>();


        public ExcelDocColumn(string path, Columns col)
        {
            _excelPackage = new ExcelPackage(path);
            _column = col;
        }
        public string GetExcelColumnData()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelWorksheet sheet = _excelPackage.Workbook.Worksheets[0];

            var numberObjectsFromExcel = sheet.Columns[(int)_column].Range.ToList();
            var allEntries = new List<string>();

            foreach (var item in numberObjectsFromExcel)
            {
                string number = item.Value?.ToString();
                string address = item.Address?.ToString();

                if (number != null && Regex.IsMatch(number, @"\d+:\d+:\d+:\d+"))
                {
                    allEntries.Add(number);
                    if (!_theNumbers.ContainsKey(number))
                        _theNumbers[number] = new List<int>();

                    _theNumbers[number].Add(int.Parse(address.Substring(1)));
                }
            }
            return String.Join("\r\n", allEntries);
        }

        public Dictionary<string, List<int>> GetDuplicates() => _theNumbers.Where(x => x.Value.Count > 1).ToDictionary(x => x.Key, y => y.Value);

        public void CreateExcelFileWithoutDuplicates()
        {
            if (!System.IO.Directory.Exists("output"))
                System.IO.Directory.CreateDirectory("output");

            var itemsToDelete = new List<int>();
            var dups = GetDuplicates();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelWorkbook workbook = _excelPackage.Workbook;
            ExcelWorksheet sourceSheet = workbook.Worksheets[0];
            //workbook.Worksheets.Add("NEW");
            //ExcelWorksheet newSheet = workbook.Worksheets["NEW"];

            //int numberOfColumns = sourceSheet.Dimension.Columns;

            //List<ExcelRange> rowsData = new List<ExcelRange>();
            //int i = 1;
            //foreach (var item in _theNumbers)
            //{
            //    //sheet.Row(int.Parse(item.Value[0
            //    //rowsData.Add(sheet.Cells[3, 1, numberOfColumns, 1]);

            //    sourceSheet.Cells[item.Value[0], 1, item.Value[0], numberOfColumns].Copy(newSheet.Cells[i, 1, i, numberOfColumns]);
            //    //sourceSheet.Cells[item.Value[0], 1, item.Value[0], numberOfColumns].CopyStyles(newSheet.Cells[i, 1, i, numberOfColumns]);
            //    i++;
            //}

            //foreach (var item in _theNumbers)
            //{

            //}

            //foreach (var kvp in dups)
            //{
            //    kvp.Value.RemoveAt(0);
            //    foreach (var item in kvp.Value)
            //    {
            //        itemsToDelete.Add(item.Substring(1));

            //    }
            //}
            //itemsToDelete.Sort();

            //for (int i = 0; i < itemsToDelete.Count; i++)
            //{
            //    sheet.DeleteRow(int.Parse(itemsToDelete[i]) - i);
            //}

            foreach (var kvp in dups)
            {
                kvp.Value.RemoveAt(0);
                foreach (var item in kvp.Value)
                {
                    itemsToDelete.Add(item);
                }
            }

            itemsToDelete.Sort();
            int j = 0;
            foreach (var item in itemsToDelete)
            {
                sourceSheet.DeleteRow(item - j);
                j++;
            }

            _excelPackage.SaveAs(new FileInfo(@"output\new.xlsx"));
            _excelPackage.Dispose();

            //public void GetNewExcelFileWithoutDuplicates()
            //{
            //    if (!_theNumbers.ContainsKey(number))
            //        _theNumbers[number] = new List<string>();

            //    _theNumbers[number].Add(address);

        }
    }
}
