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
        private ExcelDocColumn _excelDocInstance;
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
                // обработать попытку открыть файл с пустым значением колонки
            }
        }

        private void ProcessBTN_Click(object sender, RoutedEventArgs e)
        {
            if (InputField.Text != null)
            {
                if (_inputFieldInstance == null)
                    _inputFieldInstance = new InputFieldData(InputField.Text);
                else
                    _inputFieldInstance.setUserInput(InputField.Text);

                Info.Text = _inputFieldInstance.ShowInfo();
            }
        }

        private void ShowDuplicatesBTN_Click(object sender, RoutedEventArgs e)
        {
            _inputFieldInstance.GenerateTxtWithDuplicates();
            System.Diagnostics.Process.Start("notepad", "duplicates.txt");
            _excelDocInstance.CopyUniqueRows();
        }

        private void DownloadBTN_Click(object sender, RoutedEventArgs e)
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
    }
}
