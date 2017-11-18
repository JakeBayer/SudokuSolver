using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SudokuSolver
{
    public partial class Form1 : Form
    {
        private SudokuSolution _solution;
        private int _page = 0;
        public Form1()
        {
            InitializeComponent();
            DrawGrid();
            DrawSolutionGrid();
        }

        private int?[,] Parse(string s)
        {
            var board = new int?[Global.BOARD_SIZE, Global.BOARD_SIZE];
            if (s.Length != Global.BOARD_SIZE * Global.BOARD_SIZE)
                throw new InvalidOperationException("Sample Sudoku must be of proper length!!");

            for (int i = 0; i < s.Length; i++)
            {
                board[i % (Global.BOARD_SIZE), i / (Global.BOARD_SIZE)] = s[i] == '.' ? (int?) null : int.Parse(s[i].ToString());
            }
            return board;
        }

        private void DrawGrid()
        {
            this.txtGrid = new TextBox[Global.BOARD_SIZE, Global.BOARD_SIZE];
            for (var i = 0; i < Global.BOARD_SIZE; i++)
            {
                for (var j = 0; j < Global.BOARD_SIZE; j++)
                {
                    var txtBx = new TextBox();
                    txtBx.SuspendLayout();
                    txtBx.Location = new Point(12 + 22 * j, 40 + 22 * i);
                    txtBx.Size = new Size(20, 20 );
                    txtGrid[i, j] = txtBx;
                    this.Controls.Add(txtBx);
                }
            }
            
        }

        private void DrawSolutionGrid()
        {
            this.lblGrid = new Label[Global.BOARD_SIZE, Global.BOARD_SIZE];
            for (var i = 0; i < Global.BOARD_SIZE; i++)
            {
                for (var j = 0; j < Global.BOARD_SIZE; j++)
                {
                    var lbl = new Label
                    {
                        Location = new Point(12 + (Global.SquareSize + 2) * i, 80 + (Global.SquareSize + 2) * j),
                        Size = new Size(Global.SquareSize, Global.SquareSize),
                        Font = new Font("Arial", 20, FontStyle.Bold),
                        TextAlign = ContentAlignment.MiddleCenter,
                };
                    lbl.SuspendLayout();
                    lblGrid[i, j] = lbl;
                    this.pnlSolution.Controls.Add(lbl);
                    for (var k = 0; k < Global.BOARD_SIZE; k++)
                    {
                        var lblPossibleValueFlag = new Label
                        {
                            //BackColor = Color.Black,
                            Size = new Size(6,6),
                            Location = new Point(2 + 10 * (k%Global.SIZE), 2 + 10* (k/Global.SIZE)),
                            Visible = false,
                            Font = new Font("Arial", 5),
                            Text = (k + 1).ToString(),
                            TextAlign = ContentAlignment.MiddleCenter,
                        };
                        lbl.Controls.Add(lblPossibleValueFlag);
                    }
                }
            }
        }

        private void DrawSolution(int step)
        {
            var grid = _solution.Steps[step];
            lblStepType.Text = grid.StepType.ToString();
            for (int i = 0; i < Global.BOARD_SIZE; i++)
            {
                for (int j = 0; j < Global.BOARD_SIZE; j++)
                {
                    lblGrid[i, j].Text = "";
                    foreach (var control in lblGrid[i, j].Controls)
                    {
                        ((Label) control).Visible = false;
                    }
                    var square = grid.BoardState.Squares[i, j];
                    if (square.Value.HasValue)
                    {
                        lblGrid[i, j].Text = square.Value.Value.ToString();
                    }
                    else
                    {
                        foreach (int possibleValue in square.PossibleValues)
                        {
                            lblGrid[i, j].Controls[possibleValue-1].Visible = true;
                        }
                    }
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //var sudoku = new Sudoku(Parse(".....7.95.....1...86..2.....2..73..85......6...3..49..3.5...41724................"));
            var sudoku = new Sudoku(Parse("............942.8.16.....29........89.6.....14..25......4.......2...8.9..5....7.."));
            _solution = sudoku.Solve();

            trkStepSlider.Minimum = 0;
            trkStepSlider.Maximum = _solution.Steps.Count - 1;

            txtStep.Text = _page.ToString();

            DrawSolution(0);
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            var pen = new Pen(Color.Black);
            for (var i = 1; i < Global.SIZE; i++)
            {
                e.Graphics.DrawLine(pen, 11 + i * 22 * Global.SIZE, 40, 11 + i * 22 * Global.SIZE, 40 + 22 * Global.BOARD_SIZE);
                e.Graphics.DrawLine(pen, 12, 39 + i * 22 * Global.SIZE, 12 + 22 * Global.BOARD_SIZE, 39 + i * 22 * Global.SIZE);
            }
        }

        private void pnlSolution_Paint(object sender, PaintEventArgs e)
        {
            var pen = new Pen(Color.DarkGray);
            var thickPen = new Pen(Color.Black, 8);
            for (var i = 1; i < Global.BOARD_SIZE; i++)
            {
                if (i%3 == 0)
                {
                    e.Graphics.DrawLine(thickPen, 11 + i* (Global.SquareSize + 2), 80, 11 + i* (Global.SquareSize + 2), 80 + (Global.SquareSize + 2) * Global.BOARD_SIZE);
                    e.Graphics.DrawLine(thickPen, 12, 79 + i* (Global.SquareSize + 2), 12 + (Global.SquareSize + 2) * Global.BOARD_SIZE, 79 + i* (Global.SquareSize + 2));
                }
                else
                {
                    e.Graphics.DrawLine(pen, 11 + i* (Global.SquareSize + 2), 80, 11 + i* (Global.SquareSize + 2), 80 + (Global.SquareSize + 2) * Global.BOARD_SIZE);
                    e.Graphics.DrawLine(pen, 12, 79 + i* (Global.SquareSize + 2), 12 + (Global.SquareSize + 2) * Global.BOARD_SIZE, 79 + i* (Global.SquareSize + 2));
                }
            }
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (_page > 0)
            {
                _page--;
                trkStepSlider.Value = _page;
                txtStep.Text = _page.ToString();
                DrawSolution(_page);
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (_page < _solution.Steps.Count-1)
            {
                _page++;
                trkStepSlider.Value = _page;
                txtStep.Text = _page.ToString();
                DrawSolution(_page);
            }
        }

        private void trkStepSlider_Scroll(object sender, EventArgs e)
        {
            var tb = (TrackBar) sender;
            _page = tb.Value;
            txtStep.Text = _page.ToString();
            DrawSolution(_page);
        }

        private void txtStep_TextChanged(object sender, EventArgs e)
        {
            int val;
            if (int.TryParse(((TextBox) sender).Text, out val))
            {
                _page = Math.Max(Math.Min(_solution.Steps.Count, val), 0);
                trkStepSlider.Value = _page;
                DrawSolution(_page);
            }
        }
    }
}
