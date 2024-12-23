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

            public uint GetFenceCostForRegionBasedOnSides()
            {
                // for a 2d shape, corners = sides - let's take that fact to simplify the solution
                double corners = 0;

                if (RegionMap.Values.Count == 1 ||
                    RegionMap.Values.Count == 2)
                {
                    corners = 4;
                }
                else
                {
                    // figure out sides from corners
                    foreach (var cellBeingChecked in RegionMap.Values)
                    {
                        var cornersForThisCell = 0d;
                        // check top right corner
                        cornersForThisCell += GetCornerValue(cellBeingChecked, 0, 2);
                        
                        // check bottom right corner
                        cornersForThisCell +=  GetCornerValue(cellBeingChecked, 2, 4);
                        
                        // check bottom left corner
                        cornersForThisCell +=  GetCornerValue(cellBeingChecked, 4, 6);
                        
                        // check top left corner
                        cornersForThisCell +=  GetCornerValue(cellBeingChecked, 6, 8);
                        
                        Console.WriteLine($"Checking coord = {cellBeingChecked.Coordinate} and added {cornersForThisCell} corners");
                        
                        corners += cornersForThisCell;
                        Console.WriteLine($"{corners} => {(uint)Math.Round(corners)}");
                    }
                }
                
                var cornersUint = (uint)Math.Round(corners);
                Console.WriteLine($"Region {Label} has {cornersUint} corners and {RegionMap.Values.Count} cells. => {RegionMap.Values.Count} * {cornersUint}.");
                return (uint)RegionMap.Values.Count * cornersUint;
            }

            private double GetCornerValue(PlantCell cellBeingChecked, int startIndex, int endIndex)
            {
                var neighbours = 0;
                var diagonalNeighbour = false;
                for (var i = startIndex; i <= endIndex; i++)
                {
                    var direction = CoordinateUtils.Directions[i % CoordinateUtils.Directions.Count];
                    var coordinate = cellBeingChecked.Coordinate + direction;

                    if (RegionMap.TryGetCellAtCoordinate(coordinate, out _))
                    {
                        neighbours++;

                        if (i == startIndex + 1)
                        {
                            diagonalNeighbour = true;
                        }
                    }
                }

                if (neighbours == 0)
                {
                    // convex corner
                    return 1;
                }

                if (neighbours == 2)
                {
                    // concave corner
                    // it will be counted three times, so add it as three third corners
                    return 1d/3d;
                }

                if (neighbours == 1 && diagonalNeighbour)
                {
                    // edge case for weird shapes where a diagonal isn't technically a neighbour
                    return 1;
                }

                return 0d;
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
                Console.WriteLine($"Region of {plantRegion.Label} with price = {fenceCostForRegion}");
            }

            return result;
        }

        public uint GetFenceCostBasedOnSides()
        {
            uint result = 0;

            foreach (var plantRegion in _regions)
            {
                var fenceCostForRegion = plantRegion.GetFenceCostForRegionBasedOnSides();
                result += fenceCostForRegion;
                Console.WriteLine($"Region of {plantRegion.Label} with price = {fenceCostForRegion}");
            }

            return result;
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
            foreach (var direction in CoordinateUtils.CardinalDirections)
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
                foreach (var direction in CoordinateUtils.CardinalDirections)
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