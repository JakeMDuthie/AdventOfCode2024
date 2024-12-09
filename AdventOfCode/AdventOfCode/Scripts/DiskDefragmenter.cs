using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static System.Char;

namespace AdventOfCode
{
    public interface IDiskEntry
    {
    }

    public class DiskEmptySpace: IDiskEntry
    {
        public override string ToString()
        {
            return ".";
        }
    }

    public class DiskIdSpace : IDiskEntry
    {
        public int Id { get; }
        
        public DiskIdSpace(int id)
        {
            Id = id;
        }
        
        public override string ToString()
        {
            return Id.ToString();
        }
    }
    
    public class DiskDefragmenter
    {
        private readonly List<IDiskEntry> _disk = new List<IDiskEntry>();
        
        public DiskDefragmenter(string filename)
        {
            var sr = new StreamReader(
                Path.Combine(Environment.CurrentDirectory, filename));
            
            var line = sr.ReadToEnd();
            var addingNumber = true;
            var id = 0;
            
            foreach (var c in line)
            {
                var digit = (int)GetNumericValue(c);
                for (var i = 0; i < digit; i++)
                {
                    if (addingNumber)
                    {
                        _disk.Add(new DiskIdSpace(id));
                    }
                    else
                    {
                        _disk.Add(new DiskEmptySpace());
                    }
                }
                
                if(addingNumber) id++;
                
                addingNumber = !addingNumber;
            }
        }

        public override string ToString()
        {
            return _disk.Aggregate(string.Empty, (current, entry) => current + entry);
        }

        public void Defragment()
        {
            var emptyIndex = 0;
            var entryIndex = _disk.Count - 1;

            while (emptyIndex < entryIndex)
            {
                emptyIndex = GetNextEmptyIndex(emptyIndex);
                entryIndex = GetNextIdIndex(entryIndex);
                
                if(emptyIndex >= entryIndex) break;
                
                var temp = _disk[entryIndex];
                _disk[entryIndex] = _disk[emptyIndex];
                _disk[emptyIndex] = temp;
                Console.WriteLine($"Swap {emptyIndex} with {entryIndex}");
            }
        }

        private int GetNextEmptyIndex(int startingPoint)
        {
            for (var i = startingPoint; i < _disk.Count; i++)
            {
                if (_disk[i] is DiskEmptySpace)
                {
                    return i;
                }
            }
            
            return -1;
        }

        private int GetNextIdIndex(int startingPoint)
        {
            for (var i = startingPoint; i >= 0; i--)
            {
                if (_disk[i] is DiskIdSpace)
                {
                    return i;
                }
            }
            
            return -1;
        }

        public ulong GetChecksum()
        {
            ulong retVal = 0;

            for (var i = 0; i < _disk.Count; i++)
            {
                if (_disk[i] is DiskEmptySpace)
                {
                    break;
                }

                if (_disk[i] is DiskIdSpace idSpace)
                {
                    retVal += (ulong)(idSpace.Id * i);
                }
            }
            
            return retVal;
        }
    }
}