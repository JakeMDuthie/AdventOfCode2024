using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
    public class MazeNavigator
    {
        private class Route
        {
            public int Score;
            public NodeCell CurrentNode;
            public List<NodeCell> Path;
            public Coordinate ArrivalDirection;

            public void AddNodeToPath(NodeCell node)
            {
                Path.Add(node);
                CurrentNode = node;
            }

            public Route(List<NodeCell> path)
            {
                Path = new List<NodeCell>();
                Path.AddRange(path);
            }
        }
        
        private interface IMazeCell : ICell
        {
            Coordinate Coordinate { get; set; }
        }

        private class EmptyCell : IMazeCell
        {
            public Coordinate Coordinate { get; set; }

            public EmptyCell(Coordinate coordinate)
            {
                Coordinate = coordinate;
            }
        }

        private class WallCell : IMazeCell
        {
            public Coordinate Coordinate { get; set; }

            public WallCell(Coordinate coordinate)
            {
                Coordinate = coordinate;
            }
        }

        private class NodeCell : IMazeCell
        {
            internal struct ConnectingNode
            {
                public NodeCell Node { get; }
                public int Distance { get; }

                public ConnectingNode(NodeCell node, int distance)
                {
                    Node = node;
                    Distance = distance;
                }
            }
            public Coordinate Coordinate { get; set; }
            public List<ConnectingNode> ConnectingNodes { get; } = new List<ConnectingNode>();
            
            private readonly List<Coordinate> _neighbourDirections;
            

            public NodeCell(Coordinate coordinate, List<EmptyCell> neighbourCells)
            {
                Coordinate = coordinate;

                _neighbourDirections = new List<Coordinate>();
                foreach (var neighbourCell in neighbourCells)
                {
                    _neighbourDirections.Add(neighbourCell.Coordinate-coordinate);
                }
            }

            public void FindConnectingNodes(CellMap<IMazeCell> map)
            {
                ConnectingNodes.Clear();
                foreach (var direction in _neighbourDirections)
                {
                    var coordToCheck = Coordinate + direction;
                    var distance = 0;
                    while (map.TryGetCellAtCoordinate(coordToCheck, out var cell) && !(cell is WallCell))
                    {
                        distance++;
                        if (cell is NodeCell nodeCell)
                        {
                            ConnectingNodes.Add(new ConnectingNode(nodeCell, distance));
                            break;
                        }
                        coordToCheck += direction;
                    }
                }
            }
        }
        
        private readonly CellMap<IMazeCell> _mazeMap = new CellMap<IMazeCell>();
        private readonly List<NodeCell> _nodes = new List<NodeCell>();
        
        private Coordinate _startPoint;
        private Coordinate _endPoint;
        private List<Route> _bestRoutes;

        public MazeNavigator(string filename)
        {
            var sr = new StreamReader(
                Path.Combine(Environment.CurrentDirectory, filename));
            
            var line = sr.ReadLine();

            var y = 0;
            
            while (line != null)
            {
                AddLineToMap(line, y);
                y++;
                
                line = sr.ReadLine();
            }
        }

        private void AddLineToMap(string line, int yPos)
        {
            var cells = line.ToCharArray(); 
            for (var xPos = 0; xPos < cells.Length; xPos++)
            {
                var symbol = cells[xPos];

                if (char.IsControl(symbol))
                {
                    continue;
                }
                    
                var coordinate = new Coordinate(xPos, yPos);

                if (symbol == '#')
                {
                    _mazeMap.AddCell(coordinate, new WallCell(coordinate));
                    continue;
                }

                if (symbol == 'S')
                {
                    _startPoint = coordinate;
                }

                if (symbol == 'E')
                {
                    _endPoint = coordinate;
                }
                
                _mazeMap.AddCell(coordinate, new EmptyCell(coordinate));
            }
        }

        public int GetSmallestNavigationScore()
        {
            var visitedNodes = new HashSet<NodeCell>();
            var candidates = new List<Route>();
            
            var firstRoute = new Route(new List<NodeCell>());
            firstRoute.Score = 0;
            firstRoute.ArrivalDirection = CoordinateUtils.CardinalDirections[1]; // faces right to start, always
            _mazeMap.TryGetCellAtCoordinate(_startPoint, out var cell);
            firstRoute.AddNodeToPath((NodeCell)cell);
            candidates.Add(firstRoute);
            
            _bestRoutes = new List<Route>();

            var found = false;
            var result = -1;

            while (candidates.Count > 0)
            {
                candidates.Sort((x, y) => x.Score.CompareTo(y.Score));
                var currentCandidate = candidates[0];
                candidates.RemoveAt(0);
                
                foreach (var nextNode in currentCandidate.CurrentNode.ConnectingNodes)
                {
                    if (visitedNodes.Contains(nextNode.Node))
                    {
                        continue;
                    }

                    var scoreCostToConnect = GetScoreCostToConnect(currentCandidate.CurrentNode.Coordinate,
                        nextNode.Node.Coordinate,
                        currentCandidate.ArrivalDirection,
                        out var nextDirection);
                    var nextNodeScore = currentCandidate.Score + nextNode.Distance + scoreCostToConnect;
                    
                    if (nextNode.Node.Coordinate == _endPoint)
                    {
                        if (nextNodeScore > result && result > -1)
                        {
                            continue;
                        }
                        result = nextNodeScore;
                        
                        var bestRoute = new Route(currentCandidate.Path);
                        bestRoute.AddNodeToPath(nextNode.Node);
                        bestRoute.Score = result;
                        _bestRoutes.Add(bestRoute);
                        
                        continue;
                    }
                    
                    var newCandidate = new Route(currentCandidate.Path);
                    newCandidate.AddNodeToPath(nextNode.Node);
                    newCandidate.Score = nextNodeScore;
                    newCandidate.ArrivalDirection = nextDirection;
                    candidates.Add(newCandidate);
                }
                
                visitedNodes.Add(currentCandidate.CurrentNode);
            }
            
            Console.WriteLine($"Best Route Score: {result}, Route Cout {_bestRoutes.Count}");
            return result;
        }

        private int GetScoreCostToConnect(Coordinate source, Coordinate target, Coordinate currentDirection, out Coordinate nextDirection)
        {
            nextDirection = (target - source).Normalise();

            return nextDirection.Equals(currentDirection) 
                ? 0 // direction stays the same
                : 1000; // means left/right turn of 90'
        }

        public void BuildNodes()
        {
            _nodes.Clear();

            foreach (var mazeCell in _mazeMap.Values)
            {
                if (!(mazeCell is EmptyCell emptyCell))
                {
                    continue;
                }

                var neighbourCells = new List<EmptyCell>();
                foreach (var direction in CoordinateUtils.CardinalDirections)
                {
                    var coordToCheck = emptyCell.Coordinate + direction;
                    if (_mazeMap.TryGetCellAtCoordinate(coordToCheck, out var cell) &&
                        cell is EmptyCell emptyNeighbour)
                    {
                        neighbourCells.Add(emptyNeighbour);
                    }
                }

                if (neighbourCells.Count < 2 &&
                    !emptyCell.Coordinate.Equals(_startPoint)&&
                    !emptyCell.Coordinate.Equals(_endPoint))
                {
                    continue;
                }

                if (neighbourCells.Count == 2)
                {
                    var firstCoordinate = neighbourCells[0].Coordinate;
                    var secondCoordinate = neighbourCells[1].Coordinate;
                    if (firstCoordinate.X == secondCoordinate.X ||
                        firstCoordinate.Y == secondCoordinate.Y)
                    {
                        continue;
                    }
                }

                var nodeCell = new NodeCell(emptyCell.Coordinate, neighbourCells);
                
                _nodes.Add(nodeCell);
            }

            foreach (var node in _nodes)
            {
                _mazeMap.ReplaceCell(node.Coordinate, node);
            }

            foreach (var node in _nodes)
            {
                node.FindConnectingNodes(_mazeMap);
            }
            
            Console.WriteLine($"Map has {_nodes.Count} nodes");
        }

        public int GetBestSeatsCount()
        {
            var uniqueSpotsOnBestRoutes = new HashSet<Coordinate>();

            foreach (var route in _bestRoutes)
            {
                for (var i = 0; i < route.Path.Count-1; i++)
                {
                    var source = route.Path[i];
                    var target = route.Path[i + 1];
                    var direction = target.Coordinate - source.Coordinate;
                    direction.X = direction.X == 0 ? 0 : direction.X/Math.Abs(direction.X);
                    direction.Y = direction.Y == 0 ? 0 : direction.Y/Math.Abs(direction.Y);
                    var coordToCheck = source.Coordinate;
                    uniqueSpotsOnBestRoutes.Add(source.Coordinate);
                    do
                    {
                        coordToCheck += direction;
                        _mazeMap.TryGetCellAtCoordinate(coordToCheck, out var cell);
                        uniqueSpotsOnBestRoutes.Add(cell.Coordinate);
                    } while (!coordToCheck.Equals(target.Coordinate));
                }
            }
            
            return uniqueSpotsOnBestRoutes.Count;
        }
    }
}