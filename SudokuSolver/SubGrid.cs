using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    public class SubGrid : Grid, ISquareCollection
    {
        public SubGrid()
        {
            _squares = new Square[Global.SIZE, Global.SIZE];
            _rows = new LinearElement[Global.SIZE];
            _columns = new LinearElement[Global.SIZE];
            for (int i = 0; i < Global.SIZE; i++)
            {
                _rows[i] = new LinearElement();
                _columns[i] = new LinearElement();
            }
        }
        public bool IsDirty(StepType stepType) { return this.Any(s => s.IsDirty(stepType)); } 

        public Square this[int i, int j]
        {
            get { return _squares[i, j]; }
            set
            {
                _squares[i, j] = value;
                _rows[j][i] = value;
                _columns[i][j] = value;
            }
        }
    }
}
