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
        public static T[] BubbleSort<T>(T[] words) where T : IComparable // T_T
        {
            for (var i = 0; i < words.Length - 1; i++)
                for (var j = i + 1; j < words.Length; j++)
                    if (words[i].CompareTo(words[j]) > 0)
                    {
                        var temp = words[j];
                        words[j] = words[i];
                        words[i] = temp;
                    }
            return words;
        }

        public static T[] QuickSort<T>(T[] array) where T : IComparable // T_T
        {
            var stack = new Stack<int>();
            T pivot;
            var pivotIndex = 0;
            var start = pivotIndex + 1;
            var end = array.Length - 1;

            stack.Push(pivotIndex);
            stack.Push(end);

            int startOfSubSet, endOfSubset;

            while (stack.Count > 0)
            {
                endOfSubset = stack.Pop();
                startOfSubSet = stack.Pop();

                start = startOfSubSet + 1;
                pivotIndex = startOfSubSet;
                end = endOfSubset;

                pivot = array[pivotIndex];

                if (start > end)
                    continue;

                while (start < end)
                {
                    while (start <= end && array[start].CompareTo(pivot) <= 0)
                        start++;
                    while (start <= end && array[end].CompareTo(pivot) >= 0)
                        end--;
                    if (end >= start)
                        SwapElement(array, start, end);
                }

                if (pivotIndex <= end)
                    if (array[pivotIndex].CompareTo(array[end]) > 0)
                        SwapElement(array, pivotIndex, end);

                if (startOfSubSet < end)
                {
                    stack.Push(startOfSubSet);
                    stack.Push(end - 1);
                }

                if (endOfSubset > end)
                {
                    stack.Push(end + 1);
                    stack.Push(endOfSubset);
                }
            }
            return array;
        }

        private static void SwapElement<T>(T[] arr, int left, int right)
        {
            T temp = arr[left];
            arr[left] = arr[right];
            arr[right] = temp;
        }

        public static Dictionary<string, int> CountUniqueWords(string[] words)
        {
            var dic = new Dictionary<string, int>();
            foreach (var word in words)
                if (dic.ContainsKey(word))
                    dic[word]++;
                else
                    dic[word] = 1;
            return dic;
        }
    }
}
