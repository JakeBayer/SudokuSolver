using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    public static class BoardExtensions
    {
        public static void UpdatePossibleValues(this ISquareCollection squares, Square square)
        {
            if (!square.HasValue) { throw new InvalidOperationException("Can't update for a square that doesn't have a value, fam."); }
            foreach (var other in squares)
            {
                if (!Equals(other, square))
                {
                    other.RemoveValue(square.Value.Value);
                }
            }
        }
    }
}
