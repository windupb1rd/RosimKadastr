﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RosimKadastr
{
    internal class InputFieldData
    {
        private List<string> _userInput;
        private int _numbersPerFile;
        private List<string> _uniqueItems;

        public InputFieldData(string userInput)
        {
            setUserInput(userInput);
        }

        public void setUserInput(string userInput)
        {
            _userInput = userInput.Split("\r\n").ToList<string>();
            _userInput.RemoveAll(x => x == "");
        }

        public string ShowInfo()
        {
            _uniqueItems = _userInput.ToHashSet<string>().ToList<string>();
            

            return $"Всего кадастровых номеров в списке: {_userInput.Count}\r\nИз них уникальных: {_uniqueItems.Count}";
        }

        public void CreateCSV(int numbersPerFile)
        {
            if (!System.IO.Directory.Exists("output"))
                System.IO.Directory.CreateDirectory("output");
            string folderName = DateTime.Now.ToString().Replace('.', '-').Replace(' ', '-').Replace(':', '-');
            System.IO.Directory.CreateDirectory($"output\\{folderName}");

            int uniqueItemsCount = _uniqueItems.Count;
            int outputFilesQuantity;
            if (numbersPerFile > uniqueItemsCount)
            {
                outputFilesQuantity = 1;
                numbersPerFile = uniqueItemsCount;
            } 
            else
            {
                int temp = uniqueItemsCount % numbersPerFile;
                if (temp != 0)
                    outputFilesQuantity = uniqueItemsCount / numbersPerFile + 1;
                else
                    outputFilesQuantity = uniqueItemsCount / numbersPerFile;
            }

            int startPosition = 0;
            int endPosition = numbersPerFile;
            for (int i = 0; i < outputFilesQuantity; i++)
            {
                var f = System.IO.File.Create($"output\\{folderName}\\out_{i + 1}.csv");
                using (StreamWriter sw = new StreamWriter(f))
                    {
                        for (int j = startPosition; j < endPosition; j++)
                        {
                            sw.WriteLine(_uniqueItems[j] + ',');
                        }
                    }

                startPosition += numbersPerFile;
                if (endPosition + numbersPerFile <= uniqueItemsCount)
                    endPosition += numbersPerFile;
                else
                    endPosition = startPosition + (uniqueItemsCount - endPosition);
            }

            System.Diagnostics.Process.Start("explorer", $"output\\{folderName}");
        }

        public void SetNumbersPerFile(int num)
        {
            _numbersPerFile = num;
        }

        public int GetNumbersPerFile()
        {
            return _numbersPerFile;
        }
    }
}
