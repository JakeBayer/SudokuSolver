using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    public class Coord : IEquatable<Coord>
    {
        public int x;
        public int y;

        public Coord(int _x, int _y)
        {
            x = _x;
            y = _y;
        }

        public int CompareTo(Coord other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(Coord other)
        {
            return this.x == other.x && this.y == other.y;
        }

        public override bool Equals(object obj)
        {
            var other = obj as Coord;
            if (other == null) return false;
            return this.x == other.x && this.y == other.y;
        }

        public override int GetHashCode()
        {
            return x ^ y;
        }
    }

    public class CoordComparer : IEqualityComparer<Coord>
    {

        public bool Equals(Coord x, Coord y)
        {
            return x.x == y.x && x.y == y.y;
        }

        public int GetHashCode(Coord obj)
        {
            return obj.x ^ obj.y;
        }
    }
}
