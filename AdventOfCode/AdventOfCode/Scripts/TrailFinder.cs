using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
    public class TrailFinder
    {
        public class TrailCell : ICell
        {
            public Coordinate Coordinate { get; }
            public int Height { get; }

            public TrailCell(Coordinate coordinate, int height)
            {
                Coordinate = coordinate;
                Height = height;
            }
        }

        public class Trail
        {
            private const int k_peakHeight = 9;

            private TrailCell StartCell { get; }
            public int Score { get; private set; }
            public int ScoreWithDupes { get; private set; }

            public Trail(TrailCell startCell)
            {
                StartCell = startCell;
            }

            public void FindPossibleRoutes(CellMap<TrailCell> map)
            {
                var cells = new HashSet<TrailCell> { StartCell };
                var nextCells = new HashSet<TrailCell>();

                while (cells.Count > 0)
                {
                    foreach (var trailCell in cells)
                    {
                        foreach (var direction in CoordinateUtils.Directions)
                        {
                            if (!map.TryGetCellAtCoordinate(trailCell.Coordinate + direction, out var nextCell) ||
                                nextCell.Height != trailCell.Height + 1)
                            {
                                continue;
                            }

                            if (nextCells.Add(nextCell) &&
                                nextCell.Height == k_peakHeight)
                            {
                                Score++;
                            }
                        }
                    }
                    
                    cells.Clear();
                    foreach (var trailCell in nextCells)
                    {
                        cells.Add(trailCell);
                    }
                    nextCells.Clear();
                }
                
                Console.WriteLine($"Score: {Score}");
            }

            public void FindPossibleRoutesWithDupes(CellMap<TrailCell> map)
            {
                var cells = new List<TrailCell> { StartCell };
                var nextCells = new List<TrailCell>();

                while (cells.Count > 0)
                {
                    foreach (var trailCell in cells)
                    {
                        foreach (var direction in CoordinateUtils.Directions)
                        {
                            if (!map.TryGetCellAtCoordinate(trailCell.Coordinate + direction, out var nextCell) ||
                                nextCell.Height != trailCell.Height + 1)
                            {
                                continue;
                            }

                            if (nextCell.Height == k_peakHeight)
                            {
                                ScoreWithDupes++;
                                continue;
                            }
                            
                            nextCells.Add(nextCell);
                        }
                    }
                    
                    cells.Clear();
                    cells.AddRange(nextCells);
                    nextCells.Clear();
                }
                
                Console.WriteLine($"Score with dupes: {ScoreWithDupes}");
            }
        }
        
        private readonly CellMap<TrailCell> _map = new CellMap<TrailCell>();
        private readonly List<Trail> _trails = new List<Trail>();
        
        public TrailFinder(string filename)
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
                    if (!char.IsDigit(symbol))
                    {
                        continue;
                    }
                    
                    var height = (int)char.GetNumericValue(symbol);
                    var coordinate = new Coordinate(x, y);
                    var cell = new TrailCell(coordinate, height);

                    if (height == 0)
                    {
                        _trails.Add(new Trail(cell));
                    }
                    _map.AddCell(coordinate, cell);
                }
                
                line = sr.ReadLine();
                y++;
            }
        }

        public int GetTotalTrailheadScores()
        {
            return _trails.Sum(trail => trail.Score);
        }

        public int GetTotalTrailheadScoresWithDupes()
        {
            return _trails.Sum(trail => trail.ScoreWithDupes);
        }

        public void MapTrailheads()
        {
            Console.WriteLine("Mapping trail heads");
            Console.WriteLine($"Total trail heads: {_trails.Count}");
            var index = 0;
            foreach (var trail in _trails)
            {
                Console.WriteLine($"{++index}/{_trails.Count}");
                trail.FindPossibleRoutes(_map);
                Console.WriteLine("");
            }
        }

        public void MapTrailheadsWithDupes()
        {
            Console.WriteLine("Mapping trail heads with dupes");
            Console.WriteLine($"Total trail heads: {_trails.Count}");
            var index = 0;
            foreach (var trail in _trails)
            {
                Console.WriteLine($"{++index}/{_trails.Count}");
                trail.FindPossibleRoutesWithDupes(_map);
                Console.WriteLine("");
            }
        }
    }
}