using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    public enum StepType
    {
        InitialBoard = 0,
        // Look for squares which only have one possible value
        SingletonSearch = 1,

        BacktraceSearch = 2,
        // Search for row/col/sub where value can only be placed in single square
        UnarySpotSearch = 3,
        // Look for subs with linear potential values. Exclude postential value from everything else in row/col
        AmbiguousLinearExclusion = 4,
        // Look for pairs of rows/subs or cols/subs that have the same coordinates for possible values 
        NAryAmbiguousLinearExclusion = 5,

        PointingTuples = 6,

        HiddenTuples = 7,
    }
}
