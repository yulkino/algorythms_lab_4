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
        string[] unsortedWords;
        string[] sortedWords;
        string _path;
        Dictionary<string, int> wordsCount = new Dictionary<string, int>();
        int _iterations = 1000;
        string[][] subArrayWors = new string[5][];

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
                path = "text/5k.txt";
            if (!File.Exists(path))
                return Parse(GetInput("Incorrect path, try again: "));
            _path = path;
            var input = File.ReadAllText(_path);
            return Regex.Replace(input.ToLower(), @"[\p{P}\d\s]+", " ")
                .Split(' ')
                .ToArray()[1..^1];
        }

        int ShowStartMenu()
        {
            Clear();
            WriteLine(
                "Choose action: \n" +
                "1. Show unsorted words \n" +
                "2. Show sorted words \n" +
                "3. Show unique words count \n" +
                "4. Show sorting time");
            SetCursorPosition("Choose action: ".Length, 0);
            if (!int.TryParse(ReadLine(), out var input))
                return ShowStartMenu();
            Clear();
            return input;
        }

        public void StartProcessing()
        {
            unsortedWords = Parse(GetInput("Enter path: "));
            WriteLine($"Current text file: { _path }");

            while (true)
            {
                var action = 5;
                while (action < 1 || action > 4)
                    action = ShowStartMenu();
                switch (action)
                {
                    case 1:
                        ShowWords(unsortedWords);
                        break;
                    case 2:
                        ShowWords(Sorter.QuickSort(unsortedWords));
                        break;
                    case 3:
                        ShowWordsCount(Sorter.CountUniqueWords(unsortedWords));
                        break;
                    case 4:
                        InitilizeSubArrays();
                        WriteLine("elements\tBubble Sort\tQuick Sort");
                        foreach (var subWords in subArrayWors)
                        {
                            var bs = ShowSortingTimeMeasurement(subWords, (x) => Sorter.BubbleSort(x));
                            var qs = ShowSortingTimeMeasurement(subWords, (x) => Sorter.QuickSort(x));
                            OutputTime(subWords.Length, bs, qs);
                        }
                        break;
                }
                ReadKey();
                Clear();
            }

        }

        void InitilizeSubArrays()
        {
            subArrayWors[0] = unsortedWords[0..100];
            subArrayWors[1] = unsortedWords[0..500];
            subArrayWors[2] = unsortedWords[0..1000];
            subArrayWors[3] = unsortedWords[0..2500];
            subArrayWors[4] = unsortedWords[0..5000];
        }

        double ShowSortingTimeMeasurement(string[] subWords, Action<string[]> sort)
        {
            for (var i = 0; i < _iterations; i++)
            {
                var timeA = new TimeAnalizer();
                string[] sortWords = new string[subWords.Length];
                Array.Copy(subWords, sortWords, subWords.Length);
                timeA.Actions += () => sort(sortWords);
                timeA.Analyze();
            }
            var time = 0.0;
            for (var i = 0; i < _iterations; i++)
            {
                var timeAnalizer = new TimeAnalizer();
                string[] sortWords = new string[subWords.Length];
                Array.Copy(subWords, sortWords, subWords.Length);
                timeAnalizer.Actions += () => sort(sortWords);
                time += timeAnalizer.Analyze();
            }
            return Math.Round(time / _iterations, 5);
        }

        void OutputTime(int wordsCount, double bsTime, double qsTime)
        {
            ForegroundColor = ConsoleColor.Blue;
            Write(wordsCount);
            ResetColor();
            ForegroundColor = ConsoleColor.Red;
            WriteLine($"\t\t{ bsTime }\t{ (bsTime.ToString().Length > 7 ? "" : "\t") }{ qsTime }");
            ResetColor();
        }

        void ShowWordsCount(Dictionary<string, int> wordsCount)
        {
            var hopper = true;
            string word, tab;
            ConsoleColor color;
            foreach (var kv in wordsCount.OrderByDescending(x => x.Value))
            {
                word = kv.Key;
                tab = "";
                while (word.Length + tab.Length < 30)
                    tab += "-";
                color = hopper ? ConsoleColor.Red : ConsoleColor.Blue;
                ForegroundColor = color;
                Write($" { word }");
                ResetColor();
                Write($" { tab } ");
                ForegroundColor = color;
                WriteLine(kv.Value);
                hopper = !hopper;
            }
                
        }

        void ShowWords(string[] words)
        {
            foreach (var word in words)
                WriteLine(word);
        }
    }
}

