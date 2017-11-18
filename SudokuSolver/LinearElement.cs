using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    public class LinearElement : ISquareCollection
    {
        private readonly Square[] _squares = new Square[Global.BOARD_SIZE];
        public bool IsDirty(StepType stepType){ return _squares.Any(s => s.IsDirty(stepType)); } 

        public Square this[int i]
        {
            get { return _squares[i]; }
            set { _squares[i] = value; }
        }

        public IEnumerator<Square> GetEnumerator()
        {
            foreach (var square in _squares)
            {
                yield return square;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
