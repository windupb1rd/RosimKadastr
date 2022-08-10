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

namespace RosimKadastr
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private InputFieldData? _inputFieldInstance = null;
        private ExcelDocColumn? _excelDocInstance = null;
        public MainWindow()
        {
            InitializeComponent();

        }

        private void OpenFileBTN_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(ColumnLetter.Text))
            {
                Columns column;
                Enum.TryParse(ColumnLetter.Text.ToUpper(), out column);

                Microsoft.Win32.OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();
                Nullable<bool> dialogResult = openFileDlg.ShowDialog();

                if (dialogResult == true)
                {
                    _excelDocInstance = new ExcelDocColumn(openFileDlg.FileName, column);
                    InputField.Text = _excelDocInstance.GetExcelColumnData();
                }
                
            }
            else
            {
                MessageBox.Show("Необходимо заполнить столбец, в котором находятся кадастровые номера.");
            }
        }

        private void ProcessBTN_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(InputField.Text))
            {
                if (_inputFieldInstance == null)
                    _inputFieldInstance = new InputFieldData(InputField.Text);
                else
                    _inputFieldInstance.setUserInput(InputField.Text);

                Info.Text = _inputFieldInstance.ShowInfo();
            }
            else
            {
                MessageBox.Show("Поле ввода не содержит записей.");
            }
        }

        private void DownloadXlsxBTN_Click(object sender, RoutedEventArgs e)
        {
            //_inputFieldInstance.GenerateTxtWithDuplicates();
            //System.Diagnostics.Process.Start("notepad", "duplicates.txt");
            if (_excelDocInstance != null & DownloadBTN.IsEnabled)
            {
                _excelDocInstance.CreateExcelFileWithoutDuplicates();
                System.Diagnostics.Process.Start("explorer", $"output");

                DownloadBTN.IsEnabled = false;
                
            }
        }

        private void DownloadBTN_Click(object sender, RoutedEventArgs e)
        {
            if (_inputFieldInstance != null)
            {
                int numberOfFiles;
                if (!String.IsNullOrEmpty(NumberOfFiles.Text))
                {
                    if (int.TryParse(NumberOfFiles.Text, out numberOfFiles))
                        _inputFieldInstance.SetNumbersPerFile(numberOfFiles);
                }
                else
                    _inputFieldInstance.SetNumbersPerFile(100);

                _inputFieldInstance.CreateCSV(_inputFieldInstance.GetNumbersPerFile());
            }
            else
            {
                MessageBox.Show("Сначала нажмите кнопку \"Обработать\".");
            }
        }
    }
}
