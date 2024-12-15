using System.Collections.Generic;

namespace AdventOfCode
{
    public class CellMap<T> where T : ICell
    {
        public Dictionary<int,T>.ValueCollection Values => _cells.Values;
        
        private readonly Dictionary<int,T> _cells = new Dictionary<int,T>();
        
        public bool TryGetCellAtCoordinate(Coordinate coordinate, out T cell)
        {
            return _cells.TryGetValue(coordinate.GetHashCode(), out cell);
        }

        public void AddCell(Coordinate coordinate, T cell)
        {
            _cells.Add(coordinate.GetHashCode(), cell);
        }

        public void ReplaceCell(Coordinate coordinate, T cell)
        {
            if (_cells.ContainsKey(coordinate.GetHashCode()))
            {
                _cells[coordinate.GetHashCode()] = cell;
            }
            else
            {
                AddCell(coordinate, cell);
            }
        }
    }
}