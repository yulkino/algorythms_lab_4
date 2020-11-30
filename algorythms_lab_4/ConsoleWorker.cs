using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using static System.Console;

namespace algorythms_lab_4
{
    class ConsoleWorker
    {
        string[] BubbleSortedWords;
        string[] QuickSortedWords;
        Dictionary<string, int> WordsCount = new Dictionary<string, int>();
        int _iterations = 1000;
        string[] _paths = new string[]
        {
            "text/1.txt",
            "text/2.txt"
        };

        string[] Parse()
        {
            Write("Enter path: ");
            var path = ReadLine();
            if (path == "")
                path = "text/2.txt";
            if (!File.Exists(path))
            {
                Clear();
                WriteLine("Path is not found.");
                ReadKey(true);
                Clear();
                return Parse();
            }
            var input = File.ReadAllText(path);
            return Regex.Replace(input, @"[!@#$%^&*()+№;:?{[}\]|\\/<,>\.~\d]", " ")
                .Split(' ')
                .Where(x => x != "")
                .Select(x => x.ToLower())
                .ToArray();
        }

        public void StartProcessing()
        {
            var unsortedWords = Parse();

            WordsCount = Sorter.CountUniqueWords(unsortedWords);
            OutputWordsCount();

            SortingTimeMeasurement(unsortedWords, (x) => Sorter.BubbleSort(x));
            SortingTimeMeasurement(unsortedWords, (x) => Sorter.QuickSort(x));
        }

        void SortingTimeMeasurement(string[] unsortedWords, Action<string[]> sort)
        {
            //string[] bubbleSortWords = new string[unsortedWords.Length];
            //Array.Copy(unsortedWords, bubbleSortWords, unsortedWords.Length);
            //BubbleSortedWords = Sorter.BubbleSort(bubbleSortWords);

            var isFirst = true;
            foreach (var p in _paths)
            {
                if (isFirst)
                    for (var i = 0; i < _iterations; i++)
                    {
                        var timeA = new TimeAnalizer();
                        string[] sortWords = new string[unsortedWords.Length];
                        Array.Copy(unsortedWords, sortWords, unsortedWords.Length);
                        timeA.Actions += () => sort(sortWords);
                        timeA.Analyze();
                    }
                isFirst = false;
                double time = 0;
                for (var i = 0; i < _iterations; i++)
                {
                    var timeAnalizer = new TimeAnalizer();
                    string[] sortWords = new string[unsortedWords.Length];
                    Array.Copy(unsortedWords, sortWords, unsortedWords.Length);
                    timeAnalizer.Actions += () => sort(sortWords);
                    time += timeAnalizer.Analyze();
                }
                ShowInConsoleTime(p, Math.Round(time / _iterations, 10));
            }
        }

        void ShowInConsoleTime(string path, double time)
        {
            ForegroundColor = ConsoleColor.Blue;
            Write($"{path} \t:");
            ResetColor();
            ForegroundColor = ConsoleColor.Red;
            WriteLine($"\t{time}");
            ResetColor();
        }

        void OutputWordsCount()
        {
            foreach (var kv in WordsCount)
                WriteLine($" {kv.Key}{(kv.Key.Length > 7 ? "\t" : "\t\t")}{kv.Value}");
        }

        void OutputSortedWords(string[] words)
        {
            foreach (var word in words)
                WriteLine($"{word}");
        }
    }
}

