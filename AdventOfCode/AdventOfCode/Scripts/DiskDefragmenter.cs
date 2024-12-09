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

    public abstract class Section
    {
        protected Section(int startIndex, int length)
        {
            Length = length;
            StartIndex = startIndex;
        }

        public int StartIndex { get; protected set; }
        public int Length { get; protected set; }
    }

    public class EmptySection : Section
    {
        public EmptySection(int startIndex, int length) : base(startIndex, length)
        {
        }

        public void ReduceLength(int toReduceBy)
        {
            Length -= toReduceBy;
            StartIndex += toReduceBy;
        }
    }

    public class IdSection : Section
    {
        public IdSection(int startIndex, int length, int id) : base(startIndex, length)
        {
            Id = id;
        }
        
        public int Id { get; }
    }
    
    public class DiskDefragmenter
    {
        private readonly List<IDiskEntry> _disk = new List<IDiskEntry>();
        private int _topId = 0;
        
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
                        _topId = id;
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

        public void DefragmentBySection()
        {
            var idSections = new List<IdSection>();
            var emptySections = new List<EmptySection>();
            
            PopulateSections(emptySections, idSections);
            Console.WriteLine($"Defragment {emptySections.Count} empty sections\n{idSections.Count} id sections");

            for (var i = idSections.Count - 1; i >= 0; i--)
            {
                var idSection = idSections[i];
                Console.WriteLine($"Defragment {idSection.Id} at index {idSection.StartIndex} with length {idSection.Length}");

                if (TryGetEmptySectionOfSize(emptySections, idSection.Length, out var emptySection))
                {
                    Console.WriteLine($"Empty Section found at index {emptySection.StartIndex} with length {emptySection.Length}");
                    
                    MoveSectionToEmptySection(idSection, emptySection);
                    emptySection.ReduceLength(idSection.Length);
                }
                else
                {
                    Console.WriteLine($"No empty section found with length at least {idSection.Length}");
                }
            }
        }

        private void MoveSectionToEmptySection(IdSection idSection, EmptySection emptySection)
        {
            for (var i = 0; i < idSection.Length; i++)
            {
                var entryIndex = idSection.StartIndex + i;
                var emptyIndex = emptySection.StartIndex + i;
                
                var temp = _disk[entryIndex];
                _disk[entryIndex] = _disk[emptyIndex];
                _disk[emptyIndex] = temp;
            }
        }

        private bool TryGetEmptySectionOfSize(List<EmptySection> emptySections, int length, out EmptySection emptySection)
        {
            emptySection = null;
            foreach (var section in emptySections)
            {
                if (section.Length < length)
                {
                    continue;
                }
                emptySection = section;
                return true;
            }

            return false;
        }

        private void PopulateSections(List<EmptySection> emptySections, List<IdSection> idSections)
        {
            var startEntry = _disk[0];
            var startIndex = 0;
            for (var i = 1; i <= _disk.Count; i++)
            {
                if (i < _disk.Count &&
                    DoSpacesMatch(startEntry, _disk[i]))
                {
                    continue;
                }

                if (startEntry is DiskEmptySpace)
                {
                    emptySections.Add(new EmptySection(startIndex, i - startIndex));
                }
                else if (startEntry is DiskIdSpace idSpace)
                {
                    idSections.Add(new IdSection(startIndex, i - startIndex, idSpace.Id));
                }

                if (i < _disk.Count)
                {
                    startEntry = _disk[i];
                    startIndex = i;
                }
            }
        }

        private bool DoSpacesMatch(IDiskEntry entryA, IDiskEntry entryB)
        {
            if (entryA.GetType() != entryB.GetType())
            {
                return false;
            }

            if (entryA is DiskIdSpace idSpaceA && 
                entryB is DiskIdSpace idSpaceB && 
                idSpaceA.Id != idSpaceB.Id)
            {
                return false;
            }

            return true;
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
                if (_disk[i] is DiskIdSpace idSpace)
                {
                    retVal += (ulong)(idSpace.Id * i);
                }
            }
            
            return retVal;
        }
    }
}