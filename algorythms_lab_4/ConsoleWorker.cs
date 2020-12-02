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
        string _path;
        Dictionary<string, int> WordsCount = new Dictionary<string, int>();
        int _iterations = 1000;

        string GetInput(string message)
        {
            Clear();
            Write(message);
            var input = ReadLine();
            Clear();
            return input;
        }

        string[] Parse(string path)
        {
            if (path == "")
                path = "text/2.txt";


            if (!File.Exists(path))
                return Parse(GetInput("Incorrect path, try again: "));
            _path = path;
            var input = File.ReadAllText(_path);
            return Regex.Replace(input.ToLower(), @"[^a-zа-я_`\-]+", " ")
                .Split(' ')
                .ToArray();
        }

        public void StartProcessing()
        { 
            var unsortedWords = Parse(GetInput("Enter path: "));
            WriteLine($"Current text file: { _path }");
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

            for (var i = 0; i < _iterations; i++)
            {
                var timeA = new TimeAnalizer();
                string[] sortWords = new string[unsortedWords.Length];
                Array.Copy(unsortedWords, sortWords, unsortedWords.Length);
                timeA.Actions += () => sort(sortWords);
                timeA.Analyze();
            }
            var time = 0.0;
            for (var i = 0; i < _iterations; i++)
            {
                var timeAnalizer = new TimeAnalizer();
                string[] sortWords = new string[unsortedWords.Length];
                Array.Copy(unsortedWords, sortWords, unsortedWords.Length);
                timeAnalizer.Actions += () => sort(sortWords);
                time += timeAnalizer.Analyze();
            }
            ShowInConsoleTime(_path, Math.Round(time / _iterations, 10));
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

