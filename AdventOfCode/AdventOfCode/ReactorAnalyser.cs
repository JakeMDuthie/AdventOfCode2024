using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
    public class ReactorAnalyser
    {
        private List<ReactorReport> _reports = new List<ReactorReport>();
        
        public ReactorAnalyser(string filename)
        {
            InitialiseReactorReportFromFile(filename);
        }

        private void InitialiseReactorReportFromFile(string filename)
        {
            var sr = new StreamReader(
                Path.Combine(Environment.CurrentDirectory, filename));
            
            var line = sr.ReadLine();
            _reports.Clear();
            
            while (line != null)
            {
                _reports.Add(new ReactorReport(line));
                
                line = sr.ReadLine();
            }
        }

        public int GetSafeReports()
        {
            return _reports.Count(r => r.IsSafe());
        }
    }
}