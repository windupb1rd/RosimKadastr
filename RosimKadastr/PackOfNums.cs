using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RosimKadastr
{
    internal class PackOfNums
    {
        private readonly List<string> _userInput;
        private int _numbersPerFile = 100;

        public PackOfNums(string userInput)
        {
            _userInput = userInput.Split("\r\n").ToList<string>();
            _userInput.RemoveAll(x => x == "");
        }

        public string ShowInfo()
        {
            HashSet<string> uniqueItems = _userInput.ToHashSet<string>();

            return $"Всего кадастровых номеров в списке: {_userInput.Count}.\r\nИз них уникальных: {uniqueItems.Count}";
        }

        public void CreateCSV(int numbersPerFile)
        {
            
        }

        public void SetNumbersPerFile(int num)
        {
            _numbersPerFile = num;
        }

        public int GetNumbersPerFiles()
        {
            return _numbersPerFile;
        }

        public Dictionary<string, List<int>> FindDuplicates()
        {
            var _duplicates = new Dictionary<string, List<int>>();
            int count = 0;
            foreach (string line in _userInput)
            {
                count++;
                if (!_duplicates.ContainsKey(line))
                    _duplicates.Add(line, new List<int>() { count });
                else
                    _duplicates[line].Add(count);
            }

            return _duplicates
                .OrderByDescending(x => x.Value.Count)
                .Where(x => x.Value.Count > 1)
                .ToDictionary(x => x.Key, y => y.Value);
        }

        public void GenerateTxtWithDuplicates()
        {
            var output = new StreamWriter("duplicates.txt");
            var valueString = new StringBuilder();
            foreach (KeyValuePair<string, List<int>> kvp in FindDuplicates())
            {
                foreach (int x in kvp.Value)
                {
                    valueString.Append(x + ", ");
                }
                output.WriteLine($"{kvp.Key}   |   встречается {kvp.Value.Count} раз в строках {valueString.ToString()}");
                valueString.Clear();
            }
            output.Close();

        }
    }
}
