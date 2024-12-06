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
            
            private bool _alreadyChecked;
            private bool _checkedResult;

            public PrintCommand(List<int> pageOrder)
            {
                PageOrder = pageOrder;
            }

            public int GetMiddlePage()
            {
                return PageOrder[PageOrder.Count/2];
            }

            public override string ToString()
            {
                var retVal = string.Empty;
                
                foreach (var i in PageOrder)
                {
                    retVal += i + ",";
                }
                
                return retVal;
            }

            public bool SatisfiesOrderRules(List<OrderRule> orderRules)
            {
                if (_alreadyChecked)
                {
                    return _checkedResult;
                }
                
                for (var i = 0; i < PageOrder.Count-1; i++)
                {
                    var startPage = PageOrder[i];

                    for (var j = i+1; j < PageOrder.Count; j++)
                    {
                        var laterPage = PageOrder[j];

                        if (orderRules.Any(orderRule => orderRule.Later == startPage && orderRule.Earlier == laterPage))
                        {
                            _alreadyChecked = true;
                            _checkedResult = false;
                            return false;
                        }
                    }
                }
                
                _alreadyChecked = true;
                _checkedResult = true;
                return true;
            }

            public void ReorderToSatisfyRules(List<OrderRule> orderRules)
            {
                //Console.WriteLine("Command before:" + ToString());
                
                _alreadyChecked = false;
                var swapped = false;
                do
                {
                    swapped = false;
                    for (var i = 0; i < PageOrder.Count-1; i++)
                    {
                        var startPage = PageOrder[i];

                        for (var j = i+1; j < PageOrder.Count; j++)
                        {
                            var laterPage = PageOrder[j];

                            if (orderRules.Any(orderRule => orderRule.Later == startPage && orderRule.Earlier == laterPage))
                            {
                                // rule broken - so swap the offenders
                                PageOrder[i] = laterPage;
                                PageOrder[j] = startPage;
                                swapped = true;
                                break;
                            }
                        }
                    }
                } while (swapped);
                
                //Console.WriteLine("Command after:" + ToString());
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
                
                printCommand.ReorderToSatisfyRules(_orderRules);
                
                retVal += printCommand.GetMiddlePage();
            }
            
            return retVal;
        }
    }
}