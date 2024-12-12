using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode
{
    public class GardenGroups
    {
        public class PlantCell : ICell
        {
            public Coordinate Coordinate { get; }
            public char Label { get; }
            public int ExposedEdges { get; set; }
            public PlantRegion Region { get; set; }

            public PlantCell(Coordinate coordinate, char label)
            {
                Coordinate = coordinate;
                Label = label;
            }
        }

        public class PlantRegion
        {
            public char Label { get; }
            private CellMap<PlantCell> RegionMap { get; } = new CellMap<PlantCell>();

            public PlantRegion(char label, PlantCell firstCell)
            {
                Label = label;
                RegionMap.AddCell(firstCell.Coordinate, firstCell);
            }
            
            public uint GetFenceCostForRegion()
            {
                var exposedEdges = 0;
                foreach (var plantCell in RegionMap.Values)
                {
                    exposedEdges += plantCell.ExposedEdges;
                }

                return (uint)RegionMap.Values.Count * (uint)exposedEdges;
            }

            public void AddToRegion(PlantCell plantCell)
            {
                RegionMap.AddCell(plantCell.Coordinate, plantCell);
            }
        }
        
        private readonly CellMap<PlantCell> _map = new CellMap<PlantCell>();
        private readonly List<PlantRegion> _regions = new List<PlantRegion>();
        
        public GardenGroups(string filename)
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

                    if (!char.IsLetter(symbol))
                    {
                        continue;
                    }
                    
                    var coordinate = new Coordinate(x, y);
                    var cell = new PlantCell(coordinate, symbol);
                    _map.AddCell(coordinate, cell);
                }
                
                line = sr.ReadLine();
                y++;
            }
        }

        public uint GetFenceCost()
        {
            uint result = 0;

            foreach (var plantRegion in _regions)
            {
                var fenceCostForRegion = plantRegion.GetFenceCostForRegion();
                result += fenceCostForRegion;
                Console.WriteLine($"Region(s) of {plantRegion.Label} with price = {fenceCostForRegion}");
            }

            return result;
        }

        public uint GetFenceCostBasedOnSides()
        {
            throw new NotImplementedException();
        }

        public void CalculateRegionsAndExposedEdges()
        {
            CalculateExposedEdges();
            CalculateRegions();
        }

        private void CalculateRegions()
        {
            foreach (var plantCell in _map.Values)
            {
                if (plantCell.Region != null)
                {
                    continue;
                }
                
                var newRegion = new PlantRegion(plantCell.Label, plantCell);
                _regions.Add(newRegion);
                plantCell.Region = newRegion;
                
                AddNeighboursToRegion(plantCell, newRegion);
            }
        }

        private void AddNeighboursToRegion(PlantCell plantCell, PlantRegion newRegion)
        {
            foreach (var direction in CoordinateUtils.Directions)
            {
                var coordinate = plantCell.Coordinate + direction;

                if (!_map.TryGetCellAtCoordinate(coordinate, out var other) ||
                    other.Label != plantCell.Label ||
                    other.Region == newRegion)
                {
                    continue;
                }
                
                newRegion.AddToRegion(other);
                other.Region = newRegion;
                AddNeighboursToRegion(other, newRegion);
            }
        }

        private void CalculateExposedEdges()
        {
            foreach (var plantCell in _map.Values)
            {
                foreach (var direction in CoordinateUtils.Directions)
                {
                    var coordinate = plantCell.Coordinate + direction;

                    if (!_map.TryGetCellAtCoordinate(coordinate, out var other) ||
                        other.Label != plantCell.Label)
                    {
                        plantCell.ExposedEdges++;
                    }
                }
            }
        }
    }
}