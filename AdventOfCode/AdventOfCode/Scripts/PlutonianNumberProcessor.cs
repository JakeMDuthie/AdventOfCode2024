using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode
{
    public interface INumberRule
    {
        bool SatisfiesRule(ulong input);
        void Modify(KeyValuePair<ulong, ulong> input, Dictionary<ulong, ulong> outputs);
    }

    public class ZeroRule : INumberRule
    {
        public bool SatisfiesRule(ulong input)
        {
            return input == 0;
        }

        public void Modify(KeyValuePair<ulong, ulong> input, Dictionary<ulong, ulong> outputs)
        {
            if (outputs.ContainsKey(1))
            {
                outputs[1] += input.Value;
            }
            else
            {
                outputs.Add(1, input.Value);
            }
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

        public void Modify(KeyValuePair<ulong, ulong> input, Dictionary<ulong, ulong> outputs)
        {
            var inputAsString = input.Key.ToString();
            var halfLength = inputAsString.Length/2;
            
            var substring = inputAsString.Substring(0, halfLength);
            var key = ulong.Parse(substring);
            if (outputs.ContainsKey(key))
            {
                outputs[key] += input.Value;
            }
            else
            {
                outputs.Add(key, input.Value);
            }
            
            substring = inputAsString.Substring(halfLength, halfLength);
            key = ulong.Parse(substring);
            if (outputs.ContainsKey(key))
            {
                outputs[key] += input.Value;
            }
            else
            {
                outputs.Add(key, input.Value);
            }
        }
    }

    public class DefaultRule : INumberRule
    {
        public bool SatisfiesRule(ulong input)
        {
            return true;
        }

        public void Modify(KeyValuePair<ulong, ulong> input, Dictionary<ulong, ulong> outputs)
        {
            var newKey = input.Key * 2024;
            
            if (outputs.ContainsKey(newKey))
            {
                outputs[newKey] += input.Value;
            }
            else
            {
                outputs.Add(newKey, input.Value);
            }
        }
    }
    
    public class PlutonianNumberProcessor
    {
        private readonly List<ulong> _startingNumbers = new List<ulong>();
        private readonly List<INumberRule> _rules = new List<INumberRule>();
        
        private Dictionary<ulong,ulong> _lastBlinkedStones = new Dictionary<ulong,ulong>();
        
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

        public ulong StoneCount => GetLastBlinkedStonesCount();

        private ulong GetLastBlinkedStonesCount()
        {
            ulong result = 0;

            foreach (var number in _lastBlinkedStones)
            {
                result += (ulong)number.Value;
            }
            
            return result;
        }

        public void HandleBlinks(int blinkCount)
        {
            _lastBlinkedStones.Clear();
            foreach (var startingNumber in _startingNumbers)
            {
                _lastBlinkedStones.Add(startingNumber, 1);
            }
            
            Console.WriteLine("Initial Arrangement:");
            //Console.WriteLine(NumbersListToString() + "\n");
            Console.WriteLine($"Stones = {StoneCount}");
            
            for (var i = 0; i < blinkCount; i++)
            {
                _lastBlinkedStones = ProcessRulesForStones(_lastBlinkedStones);
                Console.WriteLine($"After {i+1} blink(s):");
                //Console.WriteLine(NumbersListToString() + "\n");
                Console.WriteLine($"Stones = {StoneCount}");
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

        private Dictionary<ulong,ulong> ProcessRulesForStones(Dictionary<ulong,ulong> lastBlinkedStones)
        {
            var result = new Dictionary<ulong,ulong>();

            foreach (var number in lastBlinkedStones)
            {
                foreach (var numberRule in _rules)
                {
                    if (!numberRule.SatisfiesRule(number.Key))
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