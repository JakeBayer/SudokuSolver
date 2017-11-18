using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    public class BoardState
    {
        public BoardState(Board source)
        {
            foreach (var square in source.SquaresAsEnumerable)
            {
                Squares[square.x, square.y] = new SquareState(square);
            }
        }
        public SquareState[,] Squares = new SquareState[Global.BOARD_SIZE,Global.BOARD_SIZE];
    }
}
