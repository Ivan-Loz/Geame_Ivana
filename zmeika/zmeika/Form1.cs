using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace zmeika
{
    public partial class Form1 : Form
    {
        enum Direction { Up, Down, Left, Right }
        List<Circle> Snake = new List<Circle>();
        Circle food = new Circle();
        Direction direction = Direction.Right;
        int maxWidth, maxHeight;
        Random rand = new Random();
        public Form1()
        {
            InitializeComponent();
        }
        private void StartGame()
        {
            maxWidth = this.ClientSize.Width / 20;
            maxHeight = this.ClientSize.Height / 20;
            Snake.Clear();
            Circle head = new Circle { X = 10, Y = 5 };
            Snake.Add(head);
            for (int i = 1; i <= 3; i++)
            {
                Snake.Add(new Circle { X = head.X - i, Y = head.Y });
            }
            direction = Direction.Right;
            GenerateFood();
            gameTimer.Start();
        }
        private void GenerateFood()
        {
            int x = rand.Next(0, maxWidth);
            int y = rand.Next(0, maxHeight);
            food = new Circle { X = x, Y = y };
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;
            for (int i = 0; i < Snake.Count; i++)
            {
                Brush snakeBrush = (i == 0) ? Brushes.Green :
                Brushes.LightGreen;
                canvas.FillEllipse(snakeBrush, new Rectangle(Snake[i].X * 20,
                Snake[i].Y * 20, 20, 20));
            }
            canvas.FillEllipse(Brushes.Red, new Rectangle(food.X * 20, food.Y *
            20, 20, 20));
        }
        private void MovePlayer()
        {
            for (int i = Snake.Count - 1; i > 0; i--)
            {
                Snake[i].X = Snake[i - 1].X;
                Snake[i].Y = Snake[i - 1].Y;
            }
            switch (direction)
            {
                case Direction.Right: Snake[0].X++; break;
                case Direction.Left: Snake[0].X--; break;
                case Direction.Up: Snake[0].Y--; break;
                case Direction.Down: Snake[0].Y++; break;
            }
            if (Snake[0].X == food.X && Snake[0].Y == food.Y)
            {
                Snake.Add(new Circle
                {
                    X = Snake[Snake.Count - 1].X,
                    Y =
                Snake[Snake.Count - 1].Y
                });
                GenerateFood();
            }
            if (Snake[0].X < 0 || Snake[0].Y < 0 || Snake[0].X >= maxWidth ||
            Snake[0].Y >= maxHeight)
                GameOver();
            for (int i = 1; i < Snake.Count; i++)
                if (Snake[0].X == Snake[i].X && Snake[0].Y == Snake[i].Y)
                    GameOver();
            this.Invalidate();
        }
        private void GameOver()
        {
            gameTimer.Stop();
            MessageBox.Show("Игра окончена! Нажмите OK, чтобыначать заново.");
            StartGame();
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            MovePlayer();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            StartGame();
        }

        private void Form1_KeyDown_1(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    if (direction != Direction.Right) direction = Direction.Left;
                    break;
                case Keys.Right:
                    if (direction != Direction.Left) direction = Direction.Right;
                    break;
                case Keys.Up:
                    if (direction != Direction.Down) direction = Direction.Up;
                    break;
                case Keys.Down:
                    if (direction != Direction.Up) direction = Direction.Down;
                    break;
            }
        }
    }
}
