using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
            
            private bool _alreadySatisfies;

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
                if (_alreadySatisfies)
                {
                    return true;
                }
                
                for (var i = 0; i < PageOrder.Count-1; i++)
                {
                    var startPage = PageOrder[i];

                    for (var j = i; j < PageOrder.Count; j++)
                    {
                        var laterPage = PageOrder[j];

                        if (orderRules.Any(orderRule => orderRule.Later == startPage && orderRule.Earlier == laterPage))
                        {
                            return false;
                        }
                    }
                }

                _alreadySatisfies = true;
                return true;
            }
        }
        
        private readonly List<OrderRule> _orderRules = new List<OrderRule>();
        private readonly List<PrintCommand> _commands = new List<PrintCommand>();
        
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

        public int GetTotalValueOfMiddlePagesForPrintCommandsThatNeedToBeReorderedAfterReorderingThem()
        {
            var retVal = 0;

            foreach (var printCommand in _commands)
            {
                if (printCommand.SatisfiesOrderRules(_orderRules))
                {
                    continue;
                }
                
                // TODO: Reorder the print command so that it satisfies our rules
                
                retVal += printCommand.GetMiddlePage();
            }
            
            return retVal;
        }
    }
}