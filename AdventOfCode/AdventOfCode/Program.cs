using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var sr = new StreamReader(
                Path.Combine(Environment.CurrentDirectory, "..\\..\\Data\\historianHysteria.txt"));

            var line = sr.ReadLine();

            var firstList = new List<int>();
            var secondList = new List<int>();
            
            while (line != null)
            {
                var numbers = line.Split(' ');
                firstList.Add(int.Parse(numbers[0]));
                secondList.Add(int.Parse(numbers[numbers.Length - 1]));
                
                line = sr.ReadLine();
            }
            
            firstList.Sort();
            secondList.Sort();
            var totalDiff = 0;

            for (int i = 0; i < firstList.Count; i++)
            {
                var diff = Math.Abs(firstList[i] - secondList[i]);
                Console.WriteLine($"DIFF = {diff}");
                totalDiff += diff;
            }
            
            Console.WriteLine($"Total difference = {totalDiff}");
        }
    }
}