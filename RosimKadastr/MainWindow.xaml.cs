using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using OfficeOpenXml;
using System.Text.RegularExpressions;

namespace RosimKadastr
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private PackOfNums? _instance = null;
        public MainWindow()
        {
            InitializeComponent();

        }

        private void ProcessBTN_Click(object sender, RoutedEventArgs e)
        {
            if (InputField.Text != null)
            {
                if (_instance == null)
                    _instance = new PackOfNums(InputField.Text);
                else
                    _instance.setUserInput(InputField.Text);

                Info.Text = _instance.ShowInfo();
            }
        }

        private void ShowDuplicatesBTN_Click(object sender, RoutedEventArgs e)
        {
            _instance.GenerateTxtWithDuplicates();
            System.Diagnostics.Process.Start("notepad", "duplicates.txt");
        }

        private void DownloadBTN_Click(object sender, RoutedEventArgs e)
        {
            int numberOfFiles;
            if (NumberOfFiles.Text != null)
            {
                if (int.TryParse(NumberOfFiles.Text, out numberOfFiles))
                    _instance.SetNumbersPerFile(numberOfFiles);
            }

            _instance.CreateCSV(_instance.GetNumbersPerFiles());
        }

        private void OpenFileBTN_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(ColumnLetter.Text))
            {
                Columns col;
                Enum.TryParse(ColumnLetter.Text.ToUpper(), out col);

                Microsoft.Win32.OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();
                Nullable<bool> dialogResult = openFileDlg.ShowDialog();

                if (dialogResult == true)
                {
                    string path = openFileDlg.FileName;

                    FileInfo fileInfo = new FileInfo(openFileDlg.FileName);
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (ExcelPackage excelPackage = new ExcelPackage(fileInfo))
                    {
                        ExcelWorksheet sheet = excelPackage.Workbook.Worksheets[0];

                        var numberObjectsFromExcel = sheet.Columns[(int)col].Range.ToList();
                        List<string> theNumbers = new List<string>();
                        var textBoxOutput = new StringBuilder();
                        foreach (var item in numberObjectsFromExcel)
                        {
                            string number = item.Value?.ToString();
                            if (number != null && Regex.IsMatch(number, @"\d+:\d+:\d+:\d+"))
                                {
                                    theNumbers.Add(number);
                                    //textBoxOutput.AppendLine(item.Value?.ToString());
                                }
                        }
                        InputField.Text = String.Join("\r\n", theNumbers);
                    }
                }
            }
            else
            {
                // обработать попытку открыть файл с пустым значением колонки
            }
        }
    }
}
