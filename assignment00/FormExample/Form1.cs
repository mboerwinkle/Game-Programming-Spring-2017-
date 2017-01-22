using System;
using System.Drawing;
using System.Windows.Forms;

namespace FormExample
{
    public partial class Form1 : Form
    {
        static int X = 3;
        static int Y = 5;
        bool win = false;
        int x;
        int y;
        int moves = 0;
        int[,] data = new int[X, Y];
        int cellSize;
        Random rnd = new Random();
        public Form1()
        {
            generateBoard();
            InitializeComponent();
            DoubleBuffered = true;
            UpdateSize();
        }
        private void applyMove(int col, int row)
        {
            data[col, row] ^= 1;
            if (col + 1 < X) data[col + 1, row] ^= 1;
            if (col - 1 > -1) data[col - 1, row] ^= 1;
            if (row + 1 < Y) data[col, row + 1] ^= 1;
            if (row - 1 > -1) data[col, row - 1] ^= 1;
        }
        private void generateBoard()
        {
            for (int move = 0; move < X * Y * 50; move++)
            {
                applyMove(rnd.Next(0, X), rnd.Next(0, Y));
            }
        }
        private void UpdateSize()
        {
            cellSize = Math.Min(ClientSize.Width / X, ClientSize.Height / Y);
            if (ClientSize.Width > ClientSize.Height)
            {
                x = (ClientSize.Width - X * cellSize) / 2;
                y = 0;
            }
            else
            {
                x = 0;
                y = (ClientSize.Height - Y * cellSize) / 2;
            }
        }

        protected override void OnResize(EventArgs e)
        {

            base.OnResize(e);
            UpdateSize();
            Refresh();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (win)
            {
                generateBoard();
                Refresh();
                return;
            }
            int col, row;
            col = (int)Math.Floor((e.X - x) * 1.0 / cellSize);
            row = (int)Math.Floor((e.Y - y) * 1.0 / cellSize);
            if(col > -1 && row > -1 && col < X && row < Y)
            {
                applyMove(col, row);
                moves++;
            }
            Refresh();
            //base.OnMouseDown(e);
        }
        protected override void OnPaint(PaintEventArgs e)
        {

            //base.OnPaint(e);
            win = true;
            for (int i = 0; i < X; i++)
            {
                for (int j = 0; j < Y; j++)
                {
                    Rectangle rect = new Rectangle(x + i * cellSize, y + j * cellSize, cellSize, cellSize);
                    if (data[i, j] == 1)
                    {
                        win = false;
                        e.Graphics.FillRectangle(Brushes.Yellow, rect);
                    }else
                    {
                        e.Graphics.FillRectangle(Brushes.Black, rect);
                    }
                    e.Graphics.DrawRectangle(Pens.Black, rect);
                }
            }
            if (cellSize > 10)
            {
                System.Drawing.Font font = new System.Drawing.Font("Ubuntu", cellSize / 8);
                e.Graphics.DrawString("Moves:" + moves, font, Brushes.Red, x, y);
                if (win == true)
                {
                    moves = 0;
                    e.Graphics.DrawString("WIN!", font, Brushes.Red, x + cellSize * X / 2, y + cellSize * Y / 2);
                }
            }

        }
    }
}
