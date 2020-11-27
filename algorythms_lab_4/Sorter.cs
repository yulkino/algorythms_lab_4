using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using static System.Console;

namespace algorythms_lab_4
{
    class Sorter
    {
        string[] sortedText;
        string[] unsortedText;
        Dictionary<string, int> wordsCount = new Dictionary<string, int>();

        public void StartProcessing()
        {
            Write("Enter path: ");
            unsortedText = Parse(ReadLine());
            string[] bUnsortedText = new string[unsortedText.Length];
            Array.Copy(unsortedText, bUnsortedText, unsortedText.Length);
            sortedText = BubbleSort(bUnsortedText);
            //sortedText.Select(x => { WriteLine(x); return x; }).ToArray();
            wordsCount = CountUniqueWords(sortedText);
            OutputWordsCount();
        }

        string[] Parse(string path)
        {
            var input = File.ReadAllText(path);
            return Regex.Replace(input, @"[\W\d]", " ").Split(' ').Where(x => x != "").Select(x => x.ToLower()).ToArray();
        }

        string[] BubbleSort(string[] words)
        {
            for(var i = 0; i < words.Length - 1; i++)
                for(var j = i+1; j < words.Length; j++)
                    if(words[i].CompareTo(words[j]) > 0)
                    {
                        var temp = words[j];
                        words[j] = words[i];
                        words[i] = temp;
                    }
            return words;
        }

        Dictionary<string, int> CountUniqueWords(string[] words)
        {
            var dic = new Dictionary<string, int>();
            foreach (var word in words)
                if (dic.ContainsKey(word))
                    dic[word]++;
                else
                    dic[word] = 1;
            return dic;
        }

        void OutputWordsCount()
        {
            foreach (var kv in wordsCount)
                WriteLine($"{kv.Key}{(kv.Key.Length > 7 ? "\t" : "\t\t")}{kv.Value}");
        }
    }
}
