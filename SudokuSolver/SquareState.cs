using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    public class SquareState
    {
        public SquareState(Square source)
        {
            Value = source.Value;
            PossibleValues = new HashSet<int>(source.PossibleValues);
        }

        public int? Value = null;
        public HashSet<int> PossibleValues;
    }
}
