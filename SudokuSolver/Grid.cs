using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    public class Grid : IEnumerable<Square>
    {
        protected Square[,] _squares;
        protected LinearElement[] _rows;
        protected LinearElement[] _columns;

        public LinearElement[] Rows
        {
            get { return _rows; }
        }

        public LinearElement[] Cols
        {
            get { return _columns; }
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
