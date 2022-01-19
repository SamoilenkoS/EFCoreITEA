using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        Random random = new Random();
        object locker = new object();
        int count = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {


        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread t1 = new Thread(DrawSomething);
            t1.Start(cancellationTokenSource.Token);
        }

        private void DrawSomething(object cancellation)
        {
            var cancellationToken = (CancellationToken)cancellation;
            Graphics graphics = pictureBox1
                .CreateGraphics();
            Point startPoint =
                new Point(
                    pictureBox1.Width / 2,
                    pictureBox1.Height / 2);
            for (int i = 0; i < 100_000; i++)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }
                Point destPoint = UpdatePoint(
                    startPoint,
                    pictureBox1.Width,
                    pictureBox1.Height);

                lock (locker)
                {
                    graphics.DrawLine(
                        Pens.Black,
                        startPoint,
                        destPoint);
                }

                startPoint = destPoint;

                Thread.Sleep(1);
            }
        }

        private Point UpdatePoint(Point point, int width, int height)
        {
            var isX = random.Next(0, 2) == 1;
            var isIncrement = random.Next(0, 2) == 1;
            int step = 5;
            if (isX)
            {
                int increment =  isIncrement ? step : -step;
                if((point.X + increment) > width ||
                    (point.X - increment) < 0)
                {
                    increment *= -1;
                }

                point.X += increment;
                lock (locker)
                {
                    count = 0;
                }
            }
            else
            {
                var increment = isIncrement ? step : -step;
                if ((point.Y + increment) > height ||
                    (point.Y - increment) < 0)
                {
                    increment *= -1;
                }

                point.Y += increment;
                Interlocked.Increment(ref count);
            }

            return point;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            cancellationTokenSource.Cancel();
        }
    }
}
