using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    public class SolutionStep
    {
        public StepType StepType;
        public BoardState BoardState;
        public string Current;
    }
}
