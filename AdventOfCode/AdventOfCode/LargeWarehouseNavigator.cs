using System.Collections.Generic;

namespace AdventOfCode
{
    public class LargeWarehouseNavigator
    {
        private interface IWarehouseCell : ICell
        {
            List<Coordinate> Coordinate { get; set; }
            string ToString();
        }
    }
}