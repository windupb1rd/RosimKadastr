using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace RosimKadastr
{
    internal class ExcelTable
    {
        private string pathToExcelDocument;
        private readonly Excel.Application app = new Excel.Application();

        public void OpenFile()
        {

        }
    }
}
