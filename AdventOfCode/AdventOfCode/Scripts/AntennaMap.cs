using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
    public class AntennaMap
    {
        public class AntennaMapCell : ICell
        {
            public Coordinate Coordinate { get; }
            public char Symbol { get; }
            public bool IsEmpty => Symbol == '.';
            public bool IsAntinode { get; private set; }

            public AntennaMapCell(Coordinate coordinate, char symbol)
            {
                Coordinate = coordinate;
                Symbol = symbol;
            }

            public bool TrySetIsAntinode()
            {
                IsAntinode = true;
                return true;
            }
        }
        
        private readonly CellMap<AntennaMapCell> _map = new CellMap<AntennaMapCell>();
        private readonly Dictionary<char, List<AntennaMapCell>> _sharedFrequencies = new Dictionary<char, List<AntennaMapCell>>();
        
        public AntennaMap(string filename)
        {
            var sr = new StreamReader(
                Path.Combine(Environment.CurrentDirectory, filename));
            
            var line = sr.ReadLine();

            var y = 0;
            
            while (line != null)
            {
                var cells = line.ToCharArray();
                
                for (var x = 0; x < cells.Length; x++)
                {
                    var symbol = cells[x];
                    if (char.IsControl(symbol))
                    {
                        continue;
                    }
                    
                    var coordinate = new Coordinate(x, y);
                    var cell = new AntennaMapCell(coordinate, symbol);
                    if (!cell.IsEmpty)
                    {
                        if (_sharedFrequencies.ContainsKey(cell.Symbol))
                        {
                            _sharedFrequencies[cell.Symbol].Add(cell);
                        }
                        else
                        {
                            _sharedFrequencies.Add(cell.Symbol, new List<AntennaMapCell> { cell });
                        }
                    }
                    _map.AddCell(coordinate, cell);
                }
                
                line = sr.ReadLine();
                y++;
            }
        }

        public void GenerateAntinodes()
        {
            Console.WriteLine("Generating Antinodes");
            var total = 0;
            foreach (var values in _sharedFrequencies.Values)
            {
                total += (values.Count-1)*values.Count;
            }
            
            var index = 0;
            foreach (var kvp in _sharedFrequencies)
            {
                foreach (var firstCell in kvp.Value)
                {
                    foreach (var otherCell in kvp.Value)
                    {
                        if (firstCell.Coordinate == otherCell.Coordinate)
                        {
                            continue;
                        }
                        index++;
                        Console.WriteLine($"{index} / {total}");
                        
                        var reflection = firstCell.Coordinate.GetReflection(otherCell.Coordinate);

                        if (!_map.TryGetCellAtCoordinate(reflection, out var cell))
                        {
                            continue;
                        }
                        
                        cell.TrySetIsAntinode();
                    }
                }
            }
        }
        
        public void GenerateAntinodesWithResonance()
        {
            Console.WriteLine("Generating Antinodes With Resonance");
            var total = _sharedFrequencies.Values.Sum(values => values.Count);
            var index = 0;
            
            foreach (var kvp in _sharedFrequencies)
            {
                foreach (var firstCell in kvp.Value)
                {
                    index++;
                    Console.WriteLine($"{index} / {total}");
                    firstCell.TrySetIsAntinode();
                    
                    foreach (var otherCell in kvp.Value)
                    {
                        if (firstCell.Coordinate == otherCell.Coordinate)
                        {
                            continue;
                        }

                        var reflection = firstCell.Coordinate.GetReflection(otherCell.Coordinate);
                        var nextOther = firstCell.Coordinate;

                        while (_map.TryGetCellAtCoordinate(reflection, out var cell))
                        {
                            cell.TrySetIsAntinode();
                            var temp = new Coordinate(reflection.X, reflection.Y);
                            reflection = reflection.GetReflection(nextOther);
                            nextOther = temp;
                        }
                    }
                }
            }
        }

        public int GetUniqueAntinodes()
        {
            return _map.Values.Count(v => v.IsAntinode);
        }
    }
}