using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode
{
    public interface INumberRule
    {
        bool SatisfiesRule(uint input);
        void Modify(uint input, List<uint> outputs);
    }

    public class ZeroRule : INumberRule
    {
        public bool SatisfiesRule(uint input)
        {
            return input == 0;
        }

        public void Modify(uint input, List<uint> outputs)
        {
            outputs.Add(1);
        }
    }

    public class EvenRule : INumberRule
    {
        public bool SatisfiesRule(uint input)
        {
            var digitCount = 
                input == 0 ? 1 : 1 + (int)Math.Log10(Math.Abs((double)input));
            return digitCount % 2 == 0;
        }

        public void Modify(uint input, List<uint> outputs)
        {
            var inputAsString = input.ToString();
            var halfLength = inputAsString.Length/2;
            
            var substring = inputAsString.Substring(0, halfLength);
            outputs.Add(uint.Parse(substring));
            
            substring = inputAsString.Substring(halfLength, halfLength);
            outputs.Add(uint.Parse(substring));
        }
    }

    public class DefaultRule : INumberRule
    {
        public bool SatisfiesRule(uint input)
        {
            return true;
        }

        public void Modify(uint input, List<uint> outputs)
        {
            outputs.Add(input * 2024);
        }
    }
    
    public class PlutonianNumberProcessor
    {
        private readonly List<uint> _startingNumbers = new List<uint>();
        
        private List<uint> _lastBlinkedStones = new List<uint>();
        private List<INumberRule> _rules = new List<INumberRule>();
        
        public PlutonianNumberProcessor(string filename)
        {
            var sr = new StreamReader(
                Path.Combine(Environment.CurrentDirectory, filename));
            
            var data = sr.ReadToEnd();
            
            var numberStrings = data.Split(' ');
            
            foreach (var numberString in numberStrings)
            {
                if (uint.TryParse(numberString, out var number))
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

        private List<uint> ProcessRulesForStones(List<uint> lastBlinkedStones)
        {
            var result = new List<uint>();

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