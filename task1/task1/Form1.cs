using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace task1
{
    public partial class Form1 : Form
    {
        const int width = 700, height = 500;
        Point[] trajectory1 = new Point[313];
        Point[] trajectory2 = new Point[313];
        int aX = 0, bX = 0; // а -- верхняя, б -- нижняя, х -- касающаяся
        int aY = 0, bY = 0; // а -- верхняя, б -- нижняя, х -- касающаяся
        int X = 10, Y = 10;
        double i = 0.01, step = 0.01;
        double k = 2.3;
        Pen triaj = new Pen(Color.DarkGoldenrod, 3);
        Pen trian = new Pen(Color.DarkBlue, 3);
        int index = 0;

        private void button1_Click(object sender, EventArgs e)
        {
            k = Convert.ToDouble(textBox1.Text);
            ColorDialog colorDialog1 = new ColorDialog();
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                triaj.Color = colorDialog1.Color;
            }
            ColorDialog colorDialog2 = new ColorDialog();
            if (colorDialog2.ShowDialog() == DialogResult.OK)
            {
                trian.Color = colorDialog2.Color;
            }
            triaj.Width = Convert.ToSingle(comboBox1.SelectedItem);
            trian.Width = Convert.ToSingle(comboBox2.SelectedItem);
            pictureBox1.Refresh();
        }

        void triangle(int hX, int hY) // координаты вершины треугольника
        {
            Graphics triangle = pictureBox1.CreateGraphics();
            aX = hX + Convert.ToInt32(textBox2.Text);
            aY = hY + Convert.ToInt32(textBox3.Text);
            bX = hX + Convert.ToInt32(textBox2.Text);
            bY = hY + Convert.ToInt32(textBox3.Text);
            triangle.DrawLine(trian, aX, aY, bX, bY);
            triangle.DrawLine(trian, aX, aY, hX, hY);
            triangle.DrawLine(trian, hX, hY, bX, bY);
        }

        void graph(Point[] p)
        {
            Graphics graph = pictureBox1.CreateGraphics();
            graph.Clear(BackColor);
            graph.DrawLines(triaj, p);
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "Cotangence";
            textBox1.Text = "2,3";
            textBox2.Text = "15";
            textBox3.Text = "5";
            button1.Text = "Рисовать";
            pictureBox1.Width = width;
            pictureBox1.Height = height;
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics graphics = pictureBox1.CreateGraphics();
            Pen pen = new Pen(Color.Black, 1f);
            double Step = 0.1, angle = 6.3;
            double x, y;
            int cX = 100, cY = 100;
            double R;
            Point[] points = new Point[500];
            for (int i = 0; i < points.Length; i++)
            {
                R = Math.Cos(angle) * Math.Cos(angle) / (Math.Sin(angle) * Math.Sin(angle) * Math.Sin(angle) - Math.Cos(angle) * Math.Cos(angle) * Math.Cos(angle));
                x = Convert.ToInt32(R * Math.Cos(angle) + cX);
                y = -Convert.ToInt32(R * Math.Sin(angle) - cY);
                points[i] = new Point((int)x + cX, (int)y + cY);
                angle -= Step;
            }
            graphics.DrawLines(pen, points);
            if (index == 0)
            {
                while (i <= 3.133)
                {
                    if (X > width / 2 - 1) break;
                    Y = 250 - Convert.ToInt32(Math.Cos(i) / Math.Sin(i) * k);
                    trajectory1[X - 10] = new Point(X, Y);
                    trajectory2[X - 10].X = X + width / 2;
                    trajectory2[X - 10].Y = Y;
                    graph(trajectory1);
                    Thread.Sleep(4);
                    X++;
                    i += step;
                }
            }

            e.Graphics.DrawLines(triaj, trajectory1);
            e.Graphics.DrawLines(triaj, trajectory2);
            for(int a = 0; a<313; a++)
            {
                e.Graphics.Clear(BackColor);
                e.Graphics.DrawLines(triaj, trajectory1);
                e.Graphics.DrawLines(triaj, trajectory2);
                aX = trajectory1[a].X + Convert.ToInt32(textBox2.Text);
                bX = trajectory1[a].X + Convert.ToInt32(textBox2.Text);
                aY = trajectory1[a].Y + Convert.ToInt32(textBox3.Text);
                bY = trajectory1[a].Y - Convert.ToInt32(textBox3.Text);
                e.Graphics.DrawLine(trian, trajectory1[a].X, trajectory1[a].Y, aX, aY);
                e.Graphics.DrawLine(trian, trajectory1[a].X, trajectory1[a].Y, bX, bY);
                e.Graphics.DrawLine(trian, aX, aY, bX, bY);
                Thread.Sleep(4);
            }
        }
    }
}
