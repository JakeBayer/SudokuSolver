using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    public class Sudoku
    {
        private static int _size = 3;

        private List<SolutionStep> _solutionSteps = new List<SolutionStep>();
        public Sudoku(int?[,] initialValues)
        {
            board = new Board(initialValues);
        }
        //public Square[,] Board = new Square[_size * _size,_size * _size];

        private Board board;

        public int?[,] ToIntBoard()
        {
            var intBoard = new int?[Global.BOARD_SIZE, Global.BOARD_SIZE];
            for (var i = 0; i < intBoard.GetLength(0); i++)
            {
                for (var j = 0; j < intBoard.GetLength(1); j++)
                {
                    intBoard[i, j] = board[i, j].Value;
                }
            }
            return intBoard;
        }

        public SudokuSolution Solve()
        {
            _solutionSteps.Clear();
            _solutionSteps.Add(solutionStep(StepType.InitialBoard));


            while (board.Any(s => s.IsAnyDirty))
            {
                while (board.Any(s => s.IsDirty(StepType.SingletonSearch)) ||
                       board.Any(s => s.IsDirty(StepType.UnarySpotSearch)) ||
                       board.Any(s => s.IsDirty(StepType.AmbiguousLinearExclusion)) ||
                       board.Any(s => s.IsDirty(StepType.PointingTuples)) ||
                       board.Any(s => s.IsDirty(StepType.HiddenTuples)) )
                {
                    while (board.Any(s => s.IsDirty(StepType.SingletonSearch)) ||
                           board.Any(s => s.IsDirty(StepType.UnarySpotSearch)))
                    {
                        while (board.Any(s => s.IsDirty(StepType.SingletonSearch)))
                        {
                            RunSingletonSearch();
                        }
                        while (board.Any(s => s.IsDirty(StepType.UnarySpotSearch)))
                        {
                            RunUnarySearch();
                        }
                    }
                    while (board.Any(s => s.IsDirty(StepType.AmbiguousLinearExclusion)))
                    {
                        RunAmbiguousAlignmentExclusion();
                    }
                    //foreach (var square in board)
                    //{
                    //    square.SetDirty(StepType.PointingTuples);
                    //}
                    while (board.Any(s => s.IsDirty(StepType.PointingTuples)))
                    {
                        RunPointingTuples();
                    }
                    while (board.Any(s => s.IsDirty(StepType.HiddenTuples)))
                    {
                        RunHiddenTuples();
                    }
                }
                while (board.Any(s => s.IsDirty(StepType.NAryAmbiguousLinearExclusion)))
                {
                    RunNAryAbmiguousLinearExclusion();
                }
            }


            return new SudokuSolution
            {
                Steps = _solutionSteps
            };
        }

        public void RunSingletonSearch()
        {
            foreach (var square in board.Where(s => s.IsDirty(StepType.SingletonSearch)))
            {
                if (square.PossibleValues.Count == 1)
                {
                    board.SetValue(square, square.PossibleValues.Single());
                    board.UpdatePossibleValues(square);
                    _solutionSteps.Add(solutionStep(StepType.SingletonSearch));
                }
                square.Clean(StepType.SingletonSearch);
            }
        }

        public void RunUnarySearch()
        {
            var square = board.FirstOrDefault(s => s.IsDirty(StepType.UnarySpotSearch));
            if (square != null)
            {
                var groups = new IEnumerable<Square>[] {board.Rows[square.y], board.Cols[square.x], board.SubGridForSquare(square)};
                for (int i = 1; i < Global.BOARD_SIZE + 1; i++)
                {
                    foreach (var group in groups)
                    {
                        if (group.Count(s => !s.HasValue && s.PossibleValues.Contains(i)) == 1)
                        {
                            var unary = group.Single(s => !s.HasValue && s.PossibleValues.Contains(i));
                            board.SetValue(unary, i);
                            board.UpdatePossibleValues(unary);
                            _solutionSteps.Add(solutionStep(StepType.UnarySpotSearch));
                        }
                    }
                    square.Clean(StepType.UnarySpotSearch);
                }
            }
        }

        public void RunNAryAbmiguousLinearExclusion()
        {
            var square = board.FirstOrDefault(s => s.IsDirty(StepType.NAryAmbiguousLinearExclusion));
            if (square != null)
            {
                var row = board.Rows[square.y];
                var col = board.Cols[square.x];
                var sub = board.SubGridForSquare(square);

                var potentialRowVals = Enumerable.Range(1, Global.BOARD_SIZE).Except(row.Where(r => r.HasValue).Select(r => r.Value.Value));
                foreach (int potentialVal in potentialRowVals)
                {
                    if (row.Count(r => r.PossibleValues.Contains(potentialVal)) == 2)
                    {
                        CheckNAryRowAgainstRowsAndSubs(row, square.y, potentialVal);
                    }
                }

                var potentialColVals = Enumerable.Range(1, Global.BOARD_SIZE).Except(col.Where(c => c.HasValue).Select(c => c.Value.Value));
                foreach (int potentialVal in potentialColVals)
                {
                    if (col.Count(c => c.PossibleValues.Contains(potentialVal)) == 2)
                    {
                        CheckNAryColAgainstColsAndSubs(col, square.x, potentialVal);
                    }
                }
                square.Clean(StepType.NAryAmbiguousLinearExclusion);
            }
        }

        private void CheckNAryRowAgainstRowsAndSubs(LinearElement row, int rowIdx, int val)
        {
            var valIdxs = new HashSet<int>(row.Where(r => r.PossibleValues.Contains(val)).Select(r => r.x));
            var complimentaryRows = board.Rows.Where((element, i) => i/Global.SIZE != rowIdx/Global.SIZE);

            var buddyRows = complimentaryRows.Where(c => new HashSet<int>(c.Where(s => s.PossibleValues.Contains(val)).Select(s => s.x)).SetEquals(valIdxs));
            if (buddyRows.Any())
            {
                if (buddyRows.Count() > 1)
                { throw new InvalidOperationException("More than one buddy row. Your logic is flawed. Great job, fuck wad");}

                var buddyRow = buddyRows.Single();

                var affectedRows = board.Rows.Where((element, i) => i / Global.SIZE != rowIdx / Global.SIZE && i / Global.SIZE != buddyRow.First().y / Global.SIZE);
                var changesMade = false;
                foreach (var affectedRow in affectedRows)
                {
                    foreach (int i in valIdxs)
                    {
                        changesMade = affectedRow[i].RemoveValue(val) || changesMade;
                    }
                }

                if (changesMade)
                {
                    _solutionSteps.Add(solutionStep(StepType.NAryAmbiguousLinearExclusion));
                }
            }
        }

        private void CheckNAryColAgainstColsAndSubs(LinearElement col, int colIdx, int val)
        {
            var valIdxs = new HashSet<int>(col.Where(r => r.PossibleValues.Contains(val)).Select(r => r.y));
            var complimentaryCols = board.Cols.Where((element, i) => i / Global.SIZE != colIdx / Global.SIZE);

            var buddyCols = complimentaryCols.Where(c => new HashSet<int>(c.Where(s => s.PossibleValues.Contains(val)).Select(s => s.y)).SetEquals(valIdxs));
            if (buddyCols.Any())
            {
                if (buddyCols.Count() > 1)
                { throw new InvalidOperationException("More than one buddy row. Your logic is flawed. Great job, fuck wad"); }

                var buddyCol = buddyCols.Single();

                var affectedRows = board.Cols.Where((element, i) => i / Global.SIZE != colIdx / Global.SIZE && i / Global.SIZE != buddyCol.First().x / Global.SIZE);
                var changesMade = false;
                foreach (var affectedRow in affectedRows)
                {
                    foreach (int i in valIdxs)
                    {
                        changesMade = affectedRow[i].RemoveValue(val) || changesMade;
                    }
                }

                if (changesMade)
                {
                    _solutionSteps.Add(solutionStep(StepType.NAryAmbiguousLinearExclusion));
                }
            }
        }

        public void RunAmbiguousAlignmentExclusion()
        {
            var square = board.FirstOrDefault(s => s.IsDirty(StepType.AmbiguousLinearExclusion));
            if (square != null)
            {
                var subGrid = board.SubGridForSquare(square);
                for (int i = 1; i < Global.BOARD_SIZE + 1; i++)
                {
                    var potentialSquares = subGrid.Where(s => s.PossibleValues.Contains(i));
                    if (potentialSquares.Count() > 1 && potentialSquares.Select(s => s.x).Distinct().Count() == 1)
                    {
                        var colIdx = potentialSquares.Select(s => s.x).Distinct().Single();
                        var changesMade = false;
                        foreach (var colSquare in board.Cols[colIdx].Where(sq => !subGrid.Contains(sq)))
                        {
                            changesMade = colSquare.RemoveValue(i) || changesMade;
                        }
                        if (changesMade)
                            _solutionSteps.Add(solutionStep(StepType.AmbiguousLinearExclusion));
                    }
                    if (potentialSquares.Count() > 1 && potentialSquares.Select(s => s.y).Distinct().Count() == 1)
                    {
                        var rowIdx = potentialSquares.Select(s => s.y).Distinct().Single();
                        var changesMade = false;
                        foreach (var rowSquare in board.Rows[rowIdx].Where(sq => !subGrid.Contains(sq)))
                        {
                            changesMade = rowSquare.RemoveValue(i) || changesMade;
                        }
                        if (changesMade)
                            _solutionSteps.Add(solutionStep(StepType.AmbiguousLinearExclusion));
                    }
                }
                foreach (var subSquare in subGrid)
                {
                    subSquare.Clean(StepType.AmbiguousLinearExclusion);
                }
            }
        }

        public void RunPointingTuples()
        {
            var square = board.FirstOrDefault(s => s.IsDirty(StepType.PointingTuples));
            if (square != null)
            {
                var row = board.Rows[square.y];
                var col = board.Cols[square.x];
                var potentialRowVals = Enumerable.Range(1, Global.BOARD_SIZE).Except(row.Where(r => r.HasValue).Select(r => r.Value.Value));
                foreach (var val in potentialRowVals)
                {
                    var valIdxs = row.Where(r => r.PossibleValues.Contains(val)).Select(r => r.x);
                    if (valIdxs.Select(v => v/Global.SIZE).Distinct().Count() == 1)
                    {
                        var sub = board.SubGrid(valIdxs.First()/Global.SIZE, square.y / Global.SIZE);
                        var changesMade = false;
                        foreach (var sq in sub.Where(s => s.y != square.y))
                        {
                            changesMade = sq.RemoveValue(val) || changesMade;
                        }
                        if (changesMade)
                        {
                            _solutionSteps.Add(solutionStep(StepType.PointingTuples));
                        }
                    }
                }
                var potentialColVals = Enumerable.Range(1, Global.BOARD_SIZE).Except(col.Where(r => r.HasValue).Select(r => r.Value.Value));
                foreach (var val in potentialColVals)
                {
                    var valIdxs = col.Where(r => r.PossibleValues.Contains(val)).Select(r => r.y);
                    if (valIdxs.Select(v => v / Global.SIZE).Distinct().Count() == 1)
                    {
                        var sub = board.SubGrid(square.x / Global.SIZE, valIdxs.First() / Global.SIZE);
                        var changesMade = false;
                        foreach (var sq in sub.Where(s => s.x != square.x))
                        {
                            changesMade = sq.RemoveValue(val) || changesMade;
                        }
                        if (changesMade)
                        {
                            _solutionSteps.Add(solutionStep(StepType.PointingTuples));
                        }
                    }
                }
                square.Clean(StepType.PointingTuples);
            }
        }

        public void RunHiddenTuples()
        {
            var square = board.FirstOrDefault(s => s.IsDirty(StepType.HiddenTuples));
            if (square != null)
            {
                var groups = new IEnumerable<Square>[] { board.Rows[square.y], board.Cols[square.x], board.SubGridForSquare(square) };
                foreach (var group in groups)
                {
                    var potentialVals = Enumerable.Range(1, Global.BOARD_SIZE).Except(group.Where(r => r.HasValue).Select(r => r.Value.Value));
                    var valIdxsByCount = potentialVals.Select(v => new Tuple<int, HashSet<Coord>>(v, new HashSet<Coord>(group.Where(g => g.PossibleValues.Contains(v)).Select(s => new Coord(s.x, s.y)), new CoordComparer()))).ToLookup(h => h.Item2.Count);
                    var changesMade = false;
                    foreach (IGrouping<int, Tuple<int, HashSet<Coord>>> grouping in valIdxsByCount.Where(v => v.Key > 1))
                    {
                        foreach (var indexSet in grouping)
                        {
                            //var notIncludedVals = new HashSet<int>(potentialVals.Except(indexSet.Item1));
                            //foreach (var index in indexSet.Item2) {
                            //    foreach (var notIncludedVal in notIncludedVals) {
                            //        changesMade = board[index.x, index.y].RemoveValue(notIncludedVal) || changesMade;
                            //    }
                            //}
                            if (grouping.Count(g => g.Item2.SetEquals(indexSet.Item2)) == grouping.Key)
                            {
                                var tuple = grouping.Where(g => g.Item2.SetEquals(indexSet.Item2));
                                foreach (var square1 in group.Where(g => !tuple.First().Item2.Contains(new Coord(g.x, g.y))))
                                {
                                    foreach (Tuple<int, HashSet<Coord>> tuple1 in tuple)
                                    {
                                        changesMade = square1.RemoveValue(tuple1.Item1) || changesMade;
                                    }
                                }
                            }
                        }
                    }
                    if (changesMade)
                    {
                        _solutionSteps.Add(solutionStep(StepType.HiddenTuples));
                    }
                }
                square.Clean(StepType.HiddenTuples);
            }
        }

        private SolutionStep solutionStep(StepType stepType)
        {
            return new SolutionStep
            {
                StepType = stepType,
                BoardState = new BoardState(board),
                Current = board.ToString(),
            };
        }

    }
}
