using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
    public class WordSearcher
    {
        private char[] _characters;
        private int _width;
        private int _height;

        private List<char[]> _xMasPatterns = new List<char[]>()
        {
            new char[] { 'M', 'M', 'A', 'S', 'S' },
            new char[] { 'S', 'S', 'A', 'M', 'M' },
            new char[] { 'M', 'S', 'A', 'M', 'S' },
            new char[] { 'S', 'M', 'A', 'S', 'M' },
        };
        
        public WordSearcher(string filename)
        {
            InitialiseWordGridFromFile(filename);
        }

        private void InitialiseWordGridFromFile(string filename)
        {
            var sr = new StreamReader(
                Path.Combine(Environment.CurrentDirectory, filename));
            
            var data = sr.ReadToEnd();
            _width = data.IndexOf((char)13);
            _characters = data.Where(char.IsLetter).ToArray();
            _height = _characters.Length/_width;
        }

        public int XmasOccurrences()
        {
            var occurrences = 0;
            
            for (var x = 0; x < _width - 2; x++)
            {
                for (var y = 0; y < _height - 2; y++)
                {
                    var toCheck = new char[5];
                    toCheck[0] = _characters[x + (y)*_width];
                    toCheck[1] = _characters[x+2 + (y)*_width];
                    toCheck[2] = _characters[x+1 + (y+1)*_width];
                    toCheck[3] = _characters[x + (y+2)*_width];
                    toCheck[4] = _characters[x+2 + (y+2)*_width];
                    //Console.WriteLine(toCheck);

                    foreach (var pattern in _xMasPatterns)
                    {
                        if (CharArraysEqual(toCheck, pattern))
                        {
                            occurrences++;
                            break;
                        }
                    }
                }
            }
            
            return occurrences;
        }

        public int GetOccurrencesOfWord(string wordToFind)
        {
            var occurrences = 0;
            var wordAsCharArray = wordToFind.ToCharArray();
            var reverseWordAsCharArray = wordToFind.Reverse().ToArray();

            // forwards and backwards
            for (var x = 0; x <= _width - wordAsCharArray.Length; x++)
            {
                for (var y = 0; y < _height; y++)
                {
                    var toCheck = new char[wordAsCharArray.Length];
                    Array.Copy(_characters, x + (y*_width), toCheck, 0, wordAsCharArray.Length);
                    //Console.WriteLine(toCheck);
                    
                    if (CharArraysEqual(toCheck, wordAsCharArray) ||
                        CharArraysEqual(toCheck, reverseWordAsCharArray))
                    {
                        occurrences++;
                    }
                }
            }
            
            //Console.WriteLine("Time for UP AND DOWN");
            
            // up and down
            for (var x = 0; x < _width; x++)
            {
                for (var y = 0; y <= _height - wordAsCharArray.Length; y++)
                {
                    var toCheck = new char[wordAsCharArray.Length];
                    for (var i = 0; i < wordAsCharArray.Length; i++)
                    {
                        toCheck[i] = _characters[x + (y+i)*_width];
                    }
                    //Console.WriteLine(toCheck);
                    
                    if (CharArraysEqual(toCheck, wordAsCharArray) ||
                        CharArraysEqual(toCheck, reverseWordAsCharArray))
                    {
                        occurrences++;
                    }
                }
            }
            
            //Console.WriteLine("Time for diagonal TL to BR");
            
            // diagonal TL to BR
            for (var x = 0; x <= _width - wordAsCharArray.Length; x++)
            {
                for (var y = 0; y <= _height - wordAsCharArray.Length; y++)
                {
                    var toCheck = new char[wordAsCharArray.Length];
                    for (var i = 0; i < wordAsCharArray.Length; i++)
                    {
                        toCheck[i] = _characters[(x+i) + (y+i)*_width];
                    }
                    //Console.WriteLine(toCheck);
                    
                    if (CharArraysEqual(toCheck, wordAsCharArray) ||
                        CharArraysEqual(toCheck, reverseWordAsCharArray))
                    {
                        occurrences++;
                    }
                }
            }
            
            //Console.WriteLine("Time for diagonal TR to BL");
            
            // diagonal TR to BL
            for (var x = wordAsCharArray.Length-1; x < _width; x++)
            {
                for (var y = 0; y <= _height - wordAsCharArray.Length; y++)
                {
                    var toCheck = new char[wordAsCharArray.Length];
                    for (var i = 0; i < wordAsCharArray.Length; i++)
                    {
                        toCheck[i] = _characters[(x-i) + (y+i)*_width];
                    }
                    //Console.WriteLine(toCheck);
                    
                    if (CharArraysEqual(toCheck, wordAsCharArray) ||
                        CharArraysEqual(toCheck, reverseWordAsCharArray))
                    {
                        occurrences++;
                    }
                }
            }
            
            return occurrences;
        }

        private bool CharArraysEqual(char[] first, char[] second)
        {
            if (first.Length != second.Length)
            {
                return false;
            }

            for (var i = 0; i < first.Length; i++)
            {
                if(first[i] != second[i])
                {
                    return false;
                }
            }
            
            return true;
        }
    }
}