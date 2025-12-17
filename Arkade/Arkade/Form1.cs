using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Arkade
{
    public partial class Form1 : Form
    {
        List<Rectangle> blocks = new List<Rectangle>();
        Rectangle paddle;
        Rectangle ball;
        Point ballVelocity;
        int score = 0;
        bool leftArrowDown, rightArrowDown;
        public Form1()
        {
            InitializeComponent();
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            if (leftArrowDown && paddle.Left > 0) paddle.X -= 5;
            if (rightArrowDown && paddle.Right < ClientSize.Width) paddle.X
        += 5;

            // Движение мяча 
            ball.X += ballVelocity.X;
            ball.Y += ballVelocity.Y;

            // Отскоки от стен 
            if (ball.Left <= 0 || ball.Right >= ClientSize.Width)
                ballVelocity.X *= -1;
            if (ball.Top <= 0)
                ballVelocity.Y *= -1;

            // Отскок от платформы 
            if (ball.IntersectsWith(paddle))
                ballVelocity.Y *= -1;

            // Столкновения с блоками 
            for (int i = blocks.Count - 1; i >= 0; i--)
            {
                if (ball.IntersectsWith(blocks[i]))
                {
                    blocks.RemoveAt(i);
                    ballVelocity.Y *= -1;
                    score += 10;
                    break;
                }
            }
            if (blocks.Count == 0)
            {
                score += 10;
                gameTimer.Stop();
                MessageBox.Show("Игра окончена ты победил! Нажмите OK для новой игры.");

                StartGame();
            }

            // Проверка на проигрыш 
            if (ball.Bottom > ClientSize.Height)
            {
                gameTimer.Stop();
                MessageBox.Show("Игра окончена! Нажмите OK для новой игры."); 
        
                StartGame();
            }
           
            

            Invalidate(); // перерисовка
        }
        private void StartGame()
        {
            paddle = new Rectangle(350, 550, 100, 20);
            ball = new Rectangle(390, 530, 20, 20);
            ballVelocity = new Point(4, -4);
            score = 0;

            blocks.Clear();
            for (int y = 0; y < 5; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    blocks.Add(new Rectangle(60 + x * 70, 40 + y * 30, 60, 20));
                }
            }

            gameTimer.Start();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.FillRectangle(Brushes.White, paddle);
            g.FillEllipse(Brushes.Red, ball);

            foreach (var block in blocks)
            {
                g.FillRectangle(Brushes.Blue, block);
            }

            g.DrawString("Score: " + score, this.Font, Brushes.White, 10, 10);
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left) leftArrowDown = true;
            if (e.KeyCode == Keys.Right) rightArrowDown = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.DoubleBuffered = true;
            StartGame();
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left) leftArrowDown = false;
            if (e.KeyCode == Keys.Right) rightArrowDown = false;
        }
    }
}
