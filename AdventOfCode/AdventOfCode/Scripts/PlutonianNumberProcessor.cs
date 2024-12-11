using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode
{
    public interface INumberRule
    {
        bool SatisfiesRule(ulong input);
        void Modify(ulong input, List<ulong> outputs);
    }

    public class ZeroRule : INumberRule
    {
        public bool SatisfiesRule(ulong input)
        {
            return input == 0;
        }

        public void Modify(ulong input, List<ulong> outputs)
        {
            outputs.Add(1);
        }
    }

    public class EvenRule : INumberRule
    {
        public bool SatisfiesRule(ulong input)
        {
            var digitCount = 
                input == 0 ? 1 : 1 + (int)Math.Log10(Math.Abs((double)input));
            return digitCount % 2 == 0;
        }

        public void Modify(ulong input, List<ulong> outputs)
        {
            var inputAsString = input.ToString();
            var halfLength = inputAsString.Length/2;
            
            var substring = inputAsString.Substring(0, halfLength);
            outputs.Add(ulong.Parse(substring));
            
            substring = inputAsString.Substring(halfLength, halfLength);
            outputs.Add(ulong.Parse(substring));
        }
    }

    public class DefaultRule : INumberRule
    {
        public bool SatisfiesRule(ulong input)
        {
            return true;
        }

        public void Modify(ulong input, List<ulong> outputs)
        {
            outputs.Add(input * 2024);
        }
    }
    
    public class PlutonianNumberProcessor
    {
        private readonly List<ulong> _startingNumbers = new List<ulong>();
        
        private List<ulong> _lastBlinkedStones = new List<ulong>();
        private List<INumberRule> _rules = new List<INumberRule>();
        
        public PlutonianNumberProcessor(string filename)
        {
            var sr = new StreamReader(
                Path.Combine(Environment.CurrentDirectory, filename));
            
            var data = sr.ReadToEnd();
            
            var numberStrings = data.Split(' ');
            
            foreach (var numberString in numberStrings)
            {
                if (ulong.TryParse(numberString, out var number))
                {
                    _startingNumbers.Add(number);    
                }
            }
            
            CreateRules();
        }

        public int StoneCount => _lastBlinkedStones.Count;

        public void HandleBlinks(int blinkCount)
        {
            _lastBlinkedStones.AddRange(_startingNumbers);
            
            Console.WriteLine("Initial Arrangement:");
            //Console.WriteLine(NumbersListToString() + "\n");
            Console.WriteLine($"Stones = {_lastBlinkedStones.Count}");
            
            for (var i = 0; i < blinkCount; i++)
            {
                _lastBlinkedStones = ProcessRulesForStones(_lastBlinkedStones);
                Console.WriteLine($"After {i+1} blink(s):");
                //Console.WriteLine(NumbersListToString() + "\n");
                Console.WriteLine($"Stones = {_lastBlinkedStones.Count}");
            }
        }

        private string NumbersListToString()
        {
            var result = string.Empty;

            foreach (var lastBlinkedStone in _lastBlinkedStones)
            {
                result += lastBlinkedStone + " ";
            }
            
            return result;
        }

        private List<ulong> ProcessRulesForStones(List<ulong> lastBlinkedStones)
        {
            var result = new List<ulong>();

            foreach (var number in lastBlinkedStones)
            {
                foreach (var numberRule in _rules)
                {
                    if (!numberRule.SatisfiesRule(number))
                    {
                        continue;
                    }
                    
                    numberRule.Modify(number, result);
                    break;
                }
            }
            
            return result;
        }

        private void CreateRules()
        {
            _rules.Add(new ZeroRule());
            _rules.Add(new EvenRule());
            _rules.Add(new DefaultRule());
        }
    }
}