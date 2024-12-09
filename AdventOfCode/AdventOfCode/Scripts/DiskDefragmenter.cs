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
            throw new NotImplementedException();
        }

        public int GetChecksum()
        {
            var retVal = 0;
            return retVal;
        }
    }
}