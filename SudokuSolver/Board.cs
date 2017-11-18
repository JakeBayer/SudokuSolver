using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    public class Board : Grid
    {
        private readonly SubGrid[,] _subGrids;
        public Square[,] Squares { get {return _squares;} }

        public Square this[int x, int y]
        {
            get { return _squares[x, y]; }
        }

        public SubGrid[,] SubGrids
        {
            get { return _subGrids; }
        }

        public Board(int?[,] initialValues)
        {
            _squares = new Square[Global.BOARD_SIZE, Global.BOARD_SIZE];
            _rows = new LinearElement[Global.BOARD_SIZE];
            _columns = new LinearElement[Global.BOARD_SIZE];
            _subGrids = new SubGrid[Global.SIZE, Global.SIZE];
            for (int i = 0; i < Global.BOARD_SIZE; i++)
            {
                _rows[i] = new LinearElement();
                _columns[i] = new LinearElement();
                _subGrids[i/Global.SIZE, i%Global.SIZE] = new SubGrid();
            }

            Initialize(initialValues);
        }

        private void Initialize(int?[,] initialValues)
        {
            for (int i = 0; i < Global.BOARD_SIZE; i++)
            {
                for (int j = 0; j < Global.BOARD_SIZE; j++)
                {
                    var square = new Square(i, j)
                    {
                        Value = initialValues[i, j],
                        PossibleValues =
                            initialValues[i, j].HasValue
                                ? new HashSet<int>(new[] {initialValues[i, j].Value})
                                : new HashSet<int>(Enumerable.Range(1, Global.BOARD_SIZE)),
                    };
                    _squares[i, j] = square;
                    _rows[j][i] = square;
                    _columns[i][j] = square;
                    _subGrids[i/Global.SIZE, j/Global.SIZE][i%Global.SIZE, j%Global.SIZE] = square;
                }
            }
            foreach (var square in _squares)
            {
                if (square.HasValue)
                {
                    UpdatePossibleValues(square);
                }
            }
        }

        public IEnumerable<Square> SquaresAsEnumerable
        {
            get {
                foreach (var square in _squares)
                {
                    yield return square;
                }
            }
        }

        public SubGrid SubGridForSquare(Square square)
        {
            return _subGrids[square.x/Global.SIZE, square.y/Global.SIZE];
        }

        public SubGrid SubGrid(int index)
        {
            return _subGrids[index%Global.SIZE, index/Global.SIZE];
        }
        public SubGrid SubGrid(int x, int y)
        {
            return _subGrids[x, y];
        }

        public void SetValue(int x, int y, int value)
        {
            SetValue(_squares[x, y], value);
        }

        public void SetValue(Square square, int value)
        {
            if (square.PossibleValues.Contains(value))
            {
                square.SetValue(value);
                foreach (var s in Rows[square.y])
                {
                    s.SetDirty(StepType.PointingTuples);
                    s.SetDirty(StepType.HiddenTuples);
                }
                foreach (var s in Cols[square.x])
                {
                    s.SetDirty(StepType.PointingTuples);
                    s.SetDirty(StepType.HiddenTuples);
                }
            }
            else
            {
                throw new InvalidOperationException(string.Format("God damnit, something went wrong. Square at ({0}, {1}) can't have value {2}. Possible Values are: {3}",
                    square.x, square.y, value, string.Join(", ", square.PossibleValues.Select(v => v.ToString()))));
            }
        }

        public void SetDirty(int x, int y)
        {
            if (!_squares[x, y].Value.HasValue)
            {
                _squares[x, y].IsDirty();
            }
        }

        public void UpdatePossibleValues(Square square)
        {
            if (!square.HasValue) { throw new InvalidOperationException("Can't update for a square that doesn't have a value, fam.");}

            _rows[square.y].UpdatePossibleValues(square);
            _columns[square.x].UpdatePossibleValues(square);
            _subGrids[square.x/Global.SIZE, square.y/Global.SIZE].UpdatePossibleValues(square);
        }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            // Top Line
            for (var i = 0; i < Global.SIZE; i++)
            {
                sb.Append("+ ");
                for (var j = 0; j < Global.SIZE; j++)
                {
                    sb.Append("- ");
                }
            }
            sb.Append("+ ");
            sb.AppendLine();

            for (var i = 0; i < Global.SIZE; i++)
            {
                for (var j = 0; j < Global.SIZE; j++)
                {
                    sb.Append("| ");
                    for (var k = 0; k < Global.SIZE; k++)
                    {
                        for (var n = 0; n < Global.SIZE; n++)
                        {
                            sb.Append(_squares[n + Global.SIZE * k, j + Global.SIZE * i].Value.HasValue
                                ? _squares[n + Global.SIZE * k, j + Global.SIZE * i].Value.Value.ToString() + " "
                                : ". ");
                        }
                        sb.Append("| ");
                    }
                    sb.AppendLine();
                }
                for (var k = 0; k < Global.SIZE; k++)
                {
                    sb.Append("+ ");
                    for (var n = 0; n < Global.SIZE; n++)
                    {
                        sb.Append("- ");
                    }
                }
                sb.Append("+ ");
                sb.AppendLine();
            }

            return sb.ToString();
        }


        public void UpdateBoardComponents(Action<IEnumerable<Square>> UpdateAction)
        {
            
        }
    }
}
