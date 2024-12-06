using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode
{
    public class PageOrderer
    {
        private struct OrderRule
        {
            public int Earlier { get; }
            public int Later { get; }

            public OrderRule(int earlier, int later)
            {
                Earlier = earlier;
                Later = later;
            }
        }

        private class PrintCommand
        {
            private List<int> PageOrder { get; }

            public PrintCommand(List<int> pageOrder)
            {
                PageOrder = pageOrder;
            }

            public int GetMiddlePage()
            {
                return PageOrder[PageOrder.Count/2];
            }

            public bool SatisfiesOrderRules(List<OrderRule> orderRules)
            {
                return true;
            }
        }
        
        private List<OrderRule> _orderRules = new List<OrderRule>();
        private List<PrintCommand> _commands = new List<PrintCommand>();
        
        public PageOrderer(string filename)
        {
            InitialiseRulesAndInputs(filename);
        }

        private void InitialiseRulesAndInputs(string filename)
        {
            var sr = new StreamReader(
                Path.Combine(Environment.CurrentDirectory, filename));
            
            var line = sr.ReadLine();
            
            while (line != null)
            {
                if (line.Contains("|"))
                {
                    AddOrderRuleFromLine(line);
                }
                else if (line.Contains(","))
                {
                    AddPrintCommandFromLine(line);
                }
                
                line = sr.ReadLine();
            }
        }

        private void AddPrintCommandFromLine(string line)
        {
            var parts = line.Split(',');
            var pages = new List<int>();

            foreach (var part in parts)
            {
                if (int.TryParse(part, out var page))
                {
                    pages.Add(page);
                }
            }
            _commands.Add(new PrintCommand(pages));
        }

        private void AddOrderRuleFromLine(string line)
        {
            var parts = line.Split('|');
            var orderRule = new OrderRule(int.Parse(parts[0]), int.Parse(parts[1]));
            _orderRules.Add(orderRule);
        }

        public int GetTotalValueOfMiddlePagesForPrintCommandsThatSatisfyRules()
        {
            var retVal = 0;

            foreach (var printCommand in _commands)
            {
                if (!printCommand.SatisfiesOrderRules(_orderRules))
                {
                    continue;
                }
                
                retVal += printCommand.GetMiddlePage();
            }
            
            return retVal;
        }
    }
}