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
        private PackOfNums _instance;
        public MainWindow()
        {
            InitializeComponent();

        }

        private void ProcessBTN_Click(object sender, RoutedEventArgs e)
        {
            
            if (InputField.Text != null)
            {
                _instance = new PackOfNums(InputField.Text);
                //if (NumberOfFiles.Text != null)
                //    file.SetNumbersPerFile(int.Parse(NumberOfFiles.Text));
                Info.Text = _instance.ShowInfo();
            }
        }

        private void ShowDuplicatesBTN_Click(object sender, RoutedEventArgs e)
        {
            _instance.GenerateTxtWithDuplicates();
            System.Diagnostics.Process.Start("notepad", "duplicates.txt");
        }
    }
}
