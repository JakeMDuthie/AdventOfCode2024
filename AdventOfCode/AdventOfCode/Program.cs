using System;
using System.IO;

namespace AdventOfCode
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var sr = new StreamReader(
                Path.Combine(Environment.CurrentDirectory, "..\\..\\Data\\historianHysteriaSmall.txt"));

            Console.WriteLine("Hello World!");
            var line = sr.ReadLine();
            
            while (line != null)
            {
                Console.WriteLine(line);
                
                line = sr.ReadLine();
            }
        }
    }
}