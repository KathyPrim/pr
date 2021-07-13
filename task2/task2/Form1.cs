using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace task2
{
    public partial class Form1 : Form
    {
        // Дерево:
        public const int max_iterations_amount = 7; // число уровней дерева/итераций фрактала
        public int iterations_amount = max_iterations_amount;
        public Point[][] tree_dots = new Point[max_iterations_amount][]; // координаты дерева
        Pen tree_pen = new Pen(Color.DarkBlue);

        // Фрактал:
        int crunch = 0;
        public enum Side { inside, outside, }
        public int angle;
        public Point[][] frac_dots = new Point[max_iterations_amount][];
        int smal_step = 5, big_step = 10;
        public int len = 0;
        Pen frac_pen1 = new Pen(Color.DarkGoldenrod, 3);
        Pen frac_pen2 = new Pen(Color.DarkGreen, 3);
        Pen frac_pen3 = new Pen(Color.DarkMagenta, 3);
        Pen frac_pen4 = new Pen(Color.DarkBlue, 3);
        Pen frac_pen5 = new Pen(Color.DarkKhaki, 3);
        Pen frac_pen6 = new Pen(Color.DarkCyan, 3);
        Pen frac_pen7 = new Pen(Color.DarkOrange, 3);


        static Point RotatePoint(Point pointToRotate, Point centerPoint, double angleInDegrees)
        { // По часовой +, против часовой -
            double angleInRadians = angleInDegrees * (Math.PI / 180);
            double cosTheta = Math.Cos(angleInRadians);
            double sinTheta = Math.Sin(angleInRadians);
            return new Point
            {
                X =
                    (int)
                    (cosTheta * (pointToRotate.X - centerPoint.X) -
                    sinTheta * (pointToRotate.Y - centerPoint.Y) + centerPoint.X),
                Y =
                    (int)
                    (sinTheta * (pointToRotate.X - centerPoint.X) +
                    cosTheta * (pointToRotate.Y - centerPoint.Y) + centerPoint.Y)
            };
        }

        static void FindOutDotLeft(Point left, Point right, ref Point newLeft, ref Point newRight, int len)
        {
            Point rotation = new Point(left.X + len, left.Y);
            newLeft = RotatePoint(rotation, left, -60);
            rotation.X = right.X - len;
            rotation.Y = right.Y;
            newRight = RotatePoint(rotation, right, 60);
        }

        static void FindInDotLeft(Point left, Point right, ref Point newLeft, ref Point newRight, int len)
        {
            Point rotation = new Point(left.X + len, left.Y);
            newLeft =  RotatePoint(rotation, left, 0);
            rotation.X = right.X - len;
            rotation.Y = right.Y;
            newRight = RotatePoint(rotation, right, -120);
        }

        static void FindOutDotRight(Point left, Point right, ref Point newLeft, ref Point newRight, int len)
        {
            Point rotation = new Point(left.X + len, left.Y);
            newLeft = RotatePoint(rotation, left, 180);
            rotation.X = right.X - len;
            rotation.Y = right.Y;
            newRight = RotatePoint(rotation, right, -60);
        }

        static void FindInDotRight(Point left, Point right, ref Point newLeft, ref Point newRight, int len)
        {
            Point rotation = new Point(left.X + len, left.Y);
            newLeft = RotatePoint(rotation, left, 120);
            rotation.X = right.X - len;
            rotation.Y = right.Y;
            newRight = RotatePoint(rotation, right, 0);
        }

        static void BildTrapezoid(Point left, Point right, ref Point newLeft, ref Point newRight, int len, Side side)
        {
            if (left.X < right.X) // правая сторона 
            {
                if (left.Y < right.Y)
                {
                    if(side == Side.inside)
                    {
                        FindInDotRight(left, right, ref newLeft, ref newRight, len);
                    }
                    else if (side == Side.outside)
                    {
                        Point rotation = new Point(left.X + len, left.Y);
                        newLeft = RotatePoint(rotation, left, 0);
                        rotation.X = right.X - len;
                        rotation.Y = right.Y;
                        newRight = RotatePoint(rotation, right, 120);
                    }
                }
                else if (left.Y > right.Y)
                {
                    if(side == Side.inside)
                    {
                        Point rotation = new Point(left.X + len, left.Y);
                        newLeft = RotatePoint(rotation, left, 0);
                        rotation.X = right.X - len;
                        rotation.Y = right.Y;
                        newRight = RotatePoint(rotation, right, -120);
                    }
                    else if (side == Side.outside)
                    {
                        Point rotation = new Point(left.X + len, left.Y);
                        newLeft = RotatePoint(rotation, left, -0);
                        rotation.X = right.X - len;
                        rotation.Y = right.Y;
                        newRight = RotatePoint(rotation, right, -120);
                    }
                }
                else if(left.Y == right.Y)
                {
                    if(side == Side.outside)
                    {
                        Point rotation = new Point(left.X + len, left.Y);
                        newLeft = RotatePoint(rotation, left, -60);
                        rotation.X = right.X - len;
                        rotation.Y = right.Y;
                        newRight = RotatePoint(rotation, right, 60);
                    }
                    if (side == Side.inside)
                    {
                        Point rotation = new Point(left.X + len, left.Y);
                        newLeft = RotatePoint(rotation, left, 60);
                        rotation.X = right.X - len;
                        rotation.Y = right.Y;
                        newRight = RotatePoint(rotation, right, -60);
                    }
                }
            }
            else if (left.X > right.X)
            {
                if (left.Y < right.Y)
                {
                    if (side == Side.inside)
                    {
                        Point rotation = new Point(left.X + len, left.Y);
                        newLeft = RotatePoint(rotation, left, -240);
                        rotation.X = right.X - len;
                        rotation.Y = right.Y;
                        newRight = RotatePoint(rotation, right, 180);
                    }
                    else if (side == Side.outside)
                    {
                        Point rotation = new Point(left.X + len, left.Y);
                        newLeft = RotatePoint(rotation, left, 60);
                        rotation.X = right.X - len;
                        rotation.Y = right.Y;
                        newRight = RotatePoint(rotation, right, -180);
                    }
                }
                else if (left.Y > right.Y)
                {
                    if (side == Side.inside)
                    {
                        Point rotation = new Point(left.X + len, left.Y);
                        newLeft = RotatePoint(rotation, left, -60);
                        rotation.X = right.X - len;
                        rotation.Y = right.Y;
                        newRight = RotatePoint(rotation, right, 180);
                    }
                    else if (side == Side.outside)
                    {
                        Point rotation = new Point(left.X + len, left.Y);
                        newLeft = RotatePoint(rotation, left, -180);
                        rotation.X = right.X - len;
                        rotation.Y = right.Y;
                        newRight = RotatePoint(rotation, right, -60);
                    }
                }
                else if (left.Y == right.Y)
                {
                    if (side == Side.outside)
                    {
                        Point rotation = new Point(left.X + len, left.Y);
                        newLeft = RotatePoint(rotation, left, -60);
                        rotation.X = right.X - len;
                        rotation.Y = right.Y;
                        newRight = RotatePoint(rotation, right, 60);
                    }
                    if (side == Side.inside)
                    {
                        Point rotation = new Point(left.X + len, left.Y);
                        newLeft = RotatePoint(rotation, left, 240);
                        rotation.X = right.X - len;
                        rotation.Y = right.Y;
                        newRight = RotatePoint(rotation, right, -240);
                    }
                }
            }
        }

        public Form1()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Graphics g = pictureBoxTree.CreateGraphics();
            g.Clear(BackColor);
            g = pictureBoxFrac.CreateGraphics();
            g.Clear(BackColor);
            iterations_amount = Convert.ToInt32(comboBox1.SelectedIndex);
            crunch = 1;
            pictureBoxTree.Refresh();
            pictureBoxFrac.Refresh();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Graphics g = pictureBoxTree.CreateGraphics();
            g.Clear(BackColor);
            g = pictureBoxFrac.CreateGraphics();
            g.Clear(BackColor);
            iterations_amount = Convert.ToInt32(comboBox1.SelectedIndex);
            crunch = 2;
            pictureBoxTree.Refresh();
            pictureBoxFrac.Refresh();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "Фракталы и деревья";
            button1.Text = "рисовать 1-n фракталов";
            button2.Text = "рисовать n фрактал";
        }

        private void pictureBoxTree_Paint(object sender, PaintEventArgs e)
        {
            if (iterations_amount > 0)
            {
                tree_dots[0] = new Point[1];
                int ystep = 50;
                Size size = new Size(2, 2);
                tree_dots[0][0] = new Point(150, ystep);
                Rectangle rectangle = new Rectangle(tree_dots[0][0], size);
                e.Graphics.DrawEllipse(tree_pen, rectangle);
                for (int i = 1; i < iterations_amount; i++)
                {
                    int k = Convert.ToInt32(Math.Pow(3, i));
                    int l = 0;
                    tree_dots[i] = new Point[k];
                    int len = pictureBoxTree.Width / (k + 1) + 1;
                    tree_dots[i][0] = new Point(len, tree_dots[i - 1][0].Y + ystep);
                    rectangle = new Rectangle(tree_dots[i][0], size);
                    e.Graphics.DrawEllipse(tree_pen, rectangle);
                    e.Graphics.DrawLine(tree_pen, tree_dots[i - 1][0], tree_dots[i][0]);
                    for (int j = 1; j < k; j++)
                    {
                        if (j % 3 == 0) l++;
                        tree_dots[i][j] = new Point(tree_dots[i][j - 1].X + len, tree_dots[i][j - 1].Y);
                        rectangle = new Rectangle(tree_dots[i][j], size);
                        e.Graphics.DrawEllipse(tree_pen, rectangle);
                        e.Graphics.DrawLine(tree_pen, tree_dots[i - 1][l], tree_dots[i][j]);
                    }
                }
            }
  
        }

        private void pictureBoxFrac_Paint(object sender, PaintEventArgs e)
        {
            if (crunch == 0)
            {
                // инициализация массива
                for (int i = 0; i < iterations_amount; i++)
                {
                    frac_dots[i] = new Point[Convert.ToInt32(Math.Pow(3, i)) + 1];
                }

                // 0 итерация
                frac_dots[0][0] = new Point(smal_step, pictureBoxFrac.Height - big_step);
                frac_dots[0][1] = new Point(pictureBoxFrac.Width - smal_step, pictureBoxFrac.Height - big_step);
                e.Graphics.DrawLine(frac_pen1, frac_dots[0][0], frac_dots[0][1]);

                // 1 итерация
                len = Convert.ToInt32((frac_dots[0][1].X - frac_dots[0][0].X) * 0.5);
                frac_dots[1][0] = frac_dots[0][0];
                frac_dots[1][3] = frac_dots[0][1];
                FindOutDotLeft(frac_dots[1][0], frac_dots[1][3], ref frac_dots[1][1], ref frac_dots[1][2], len);
                e.Graphics.DrawLine(frac_pen2, frac_dots[1][0], frac_dots[1][1]);
                e.Graphics.DrawLine(frac_pen2, frac_dots[1][2], frac_dots[1][1]);
                e.Graphics.DrawLine(frac_pen2, frac_dots[1][3], frac_dots[1][2]);

                // 2 итерация
                len = len / 2;
                frac_dots[2][0] = frac_dots[1][0];
                frac_dots[2][3] = frac_dots[1][1];
                FindInDotLeft(frac_dots[2][0], frac_dots[2][3], ref frac_dots[2][1], ref frac_dots[2][2], len);
                e.Graphics.DrawLine(frac_pen3, frac_dots[2][0], frac_dots[2][1]);
                e.Graphics.DrawLine(frac_pen3, frac_dots[2][2], frac_dots[2][1]);
                e.Graphics.DrawLine(frac_pen3, frac_dots[2][3], frac_dots[2][2]);

                frac_dots[2][6] = frac_dots[1][2];
                FindOutDotLeft(frac_dots[2][3], frac_dots[2][6], ref frac_dots[2][4], ref frac_dots[2][5], len);
                e.Graphics.DrawLine(frac_pen3, frac_dots[2][3], frac_dots[2][4]);
                e.Graphics.DrawLine(frac_pen3, frac_dots[2][4], frac_dots[2][5]);
                e.Graphics.DrawLine(frac_pen3, frac_dots[2][5], frac_dots[2][6]);

                frac_dots[2][9] = frac_dots[1][3];
                FindInDotRight(frac_dots[2][6], frac_dots[2][9], ref frac_dots[2][7], ref frac_dots[2][8], len);
                e.Graphics.DrawLine(frac_pen3, frac_dots[2][6], frac_dots[2][7]);
                e.Graphics.DrawLine(frac_pen3, frac_dots[2][7], frac_dots[2][8]);
                e.Graphics.DrawLine(frac_pen3, frac_dots[2][8], frac_dots[2][9]);

                // 3 итерация
                int j = 3;
                frac_dots[j][0] = frac_dots[0][0];
                len = len / 2;
                FindOutDotLeft(frac_dots[0][0], frac_dots[2][1], ref frac_dots[3][1], ref frac_dots[3][2], len);
                frac_dots[3][3] = frac_dots[2][1];
                frac_dots[3][6] = frac_dots[2][2];
                FindInDotLeft(frac_dots[2][1], frac_dots[2][2], ref frac_dots[3][4], ref frac_dots[3][5], len);
                FindOutDotRight(frac_dots[2][2], frac_dots[2][3], ref frac_dots[3][7], ref frac_dots[3][8], len);
                frac_dots[3][9] = frac_dots[2][3];

                FindInDotLeft(frac_dots[2][3], frac_dots[2][4], ref frac_dots[3][10], ref frac_dots[3][11], len);
                frac_dots[3][12] = frac_dots[2][4];
                frac_dots[3][15] = frac_dots[2][5];
                FindOutDotLeft(frac_dots[2][4], frac_dots[2][5], ref frac_dots[3][13], ref frac_dots[3][14], len);
                FindInDotRight(frac_dots[2][5], frac_dots[2][6], ref frac_dots[3][16], ref frac_dots[3][17], len);
                frac_dots[3][18] = frac_dots[2][6];

                FindInDotLeft(frac_dots[2][7], frac_dots[2][6], ref frac_dots[3][20], ref frac_dots[3][19], len);
                frac_dots[3][21] = frac_dots[2][7];
                frac_dots[3][24] = frac_dots[2][8];
                FindInDotRight(frac_dots[2][7], frac_dots[2][8], ref frac_dots[3][22], ref frac_dots[3][23], len);
                FindOutDotLeft(frac_dots[2][8], frac_dots[2][9], ref frac_dots[3][25], ref frac_dots[3][26], len);
                frac_dots[3][27] = frac_dots[2][9];

                for (int i = 0; i < 27; i++)
                {
                    e.Graphics.DrawLine(frac_pen4, frac_dots[j][i], frac_dots[j][i + 1]);
                }

                // 4 итерация
                j = 4;
                frac_dots[j][0] = frac_dots[0][0];
                len = len / 2;
                int k = 0;
                for (int i = 0; i < Math.Pow(3, j) + 1; i = i + 3)
                {
                    frac_dots[j][i] = frac_dots[j - 1][k];
                    k++;
                }
                Side side;
                k = 0;
                for (int i = 0; i < Math.Pow(3, j); i = i + 3)
                {
                    int d = k % 2;
                    if (d == 0) side = Side.inside;
                    else side = Side.outside;
                    k++;
                    BildTrapezoid(frac_dots[j][i], frac_dots[j][i + 3],
                        ref frac_dots[j][i + 1], ref frac_dots[j][i + 2], len, side);
                }
                for (int i = 0; i < 81; i++)
                {
                    e.Graphics.DrawLine(frac_pen5, frac_dots[4][i], frac_dots[4][i + 1]);
                }

                // 5 Иитерация
                j = 5;
                frac_dots[j][0] = frac_dots[0][0];
                len = len / 2;
                k = 0;
                for (int i = 0; i < Math.Pow(3, j) + 1; i = i + 3)
                {
                    frac_dots[j][i] = frac_dots[j - 1][k];
                    k++;
                }
                k = 1;
                for (int i = 0; i < Math.Pow(3, j); i = i + 3)
                {
                    int d = k % 2;
                    if (d == 0) side = Side.inside;
                    else side = Side.outside;
                    k++;
                    BildTrapezoid(frac_dots[j][i], frac_dots[j][i + 3],
                        ref frac_dots[j][i + 1], ref frac_dots[j][i + 2], len, side);
                }
                for (int i = 0; i < Math.Pow(3, j) - 1; i++)
                {
                    e.Graphics.DrawLine(frac_pen6, frac_dots[5][i], frac_dots[5][i + 1]);
                }

                // 6 Иитерация
                j = 6;
                frac_dots[j][0] = frac_dots[0][0];
                len = len / 2;
                k = 0;
                for (int i = 0; i < Math.Pow(3, j) + 1; i = i + 3)
                {
                    frac_dots[j][i] = frac_dots[j - 1][k];
                    k++;
                }
                k = 0;
                for (int i = 0; i < Math.Pow(3, j); i = i + 3)
                {
                    int d = k % 2;
                    if (d == 0) side = Side.inside;
                    else side = Side.outside;
                    k++;
                    BildTrapezoid(frac_dots[j][i], frac_dots[j][i + 3],
                        ref frac_dots[j][i + 1], ref frac_dots[j][i + 2], len, side);
                }
                for (int i = 0; i < Math.Pow(3, j) - 1; i++)
                {
                    e.Graphics.DrawLine(frac_pen7, frac_dots[j][i], frac_dots[j][i + 1]);
                }
            }
            else if (crunch == 1)
            {
                switch (iterations_amount)
                {
                    case 0:
                        {
                            e.Graphics.DrawLine(frac_pen1, frac_dots[0][0], frac_dots[0][1]);
                            break;
                        }
                    case 1:
                        {
                            e.Graphics.DrawLine(frac_pen1, frac_dots[0][0], frac_dots[0][1]);
                            e.Graphics.DrawLine(frac_pen2, frac_dots[1][0], frac_dots[1][1]);
                            e.Graphics.DrawLine(frac_pen2, frac_dots[1][2], frac_dots[1][1]);
                            e.Graphics.DrawLine(frac_pen2, frac_dots[1][3], frac_dots[1][2]);
                            break;
                        }
                    case 2:
                        {
                            e.Graphics.DrawLine(frac_pen1, frac_dots[0][0], frac_dots[0][1]);
                            e.Graphics.DrawLine(frac_pen2, frac_dots[1][0], frac_dots[1][1]);
                            e.Graphics.DrawLine(frac_pen2, frac_dots[1][2], frac_dots[1][1]);
                            e.Graphics.DrawLine(frac_pen2, frac_dots[1][3], frac_dots[1][2]);
                            for (int i = 0; i < 9; i++)
                            {
                                e.Graphics.DrawLine(frac_pen4, frac_dots[2][i], frac_dots[2][i + 1]);
                            }
                            break;
                        }
                    case 3:
                        {
                            e.Graphics.DrawLine(frac_pen1, frac_dots[0][0], frac_dots[0][1]);
                            e.Graphics.DrawLine(frac_pen2, frac_dots[1][0], frac_dots[1][1]);
                            e.Graphics.DrawLine(frac_pen2, frac_dots[1][2], frac_dots[1][1]);
                            e.Graphics.DrawLine(frac_pen2, frac_dots[1][3], frac_dots[1][2]);
                            for (int i = 0; i < 9; i++)
                            {
                                e.Graphics.DrawLine(frac_pen3, frac_dots[2][i], frac_dots[2][i + 1]);
                            }
                            for (int i = 0; i < 27; i++)
                            {
                                e.Graphics.DrawLine(frac_pen4, frac_dots[3][i], frac_dots[3][i + 1]);
                            }
                            break;
                        }
                    case 4:
                        {
                            e.Graphics.DrawLine(frac_pen1, frac_dots[0][0], frac_dots[0][1]);
                            e.Graphics.DrawLine(frac_pen2, frac_dots[1][0], frac_dots[1][1]);
                            e.Graphics.DrawLine(frac_pen2, frac_dots[1][2], frac_dots[1][1]);
                            e.Graphics.DrawLine(frac_pen2, frac_dots[1][3], frac_dots[1][2]);
                            for (int i = 0; i < 9; i++)
                            {
                                e.Graphics.DrawLine(frac_pen3, frac_dots[2][i], frac_dots[2][i + 1]);
                            }
                            for (int i = 0; i < 27; i++)
                            {
                                e.Graphics.DrawLine(frac_pen4, frac_dots[3][i], frac_dots[3][i + 1]);
                            }
                            for (int i = 0; i < 81; i++)
                            {
                                e.Graphics.DrawLine(frac_pen5, frac_dots[4][i], frac_dots[4][i + 1]);
                            }
                            break;
                        }
                    case 5:
                        {
                            e.Graphics.DrawLine(frac_pen1, frac_dots[0][0], frac_dots[0][1]);
                            e.Graphics.DrawLine(frac_pen2, frac_dots[1][0], frac_dots[1][1]);
                            e.Graphics.DrawLine(frac_pen2, frac_dots[1][2], frac_dots[1][1]);
                            e.Graphics.DrawLine(frac_pen2, frac_dots[1][3], frac_dots[1][2]);
                            for (int i = 0; i < 9; i++)
                            {
                                e.Graphics.DrawLine(frac_pen3, frac_dots[2][i], frac_dots[2][i + 1]);
                            }
                            for (int i = 0; i < 27; i++)
                            {
                                e.Graphics.DrawLine(frac_pen4, frac_dots[3][i], frac_dots[3][i + 1]);
                            }
                            for (int i = 0; i < 81; i++)
                            {
                                e.Graphics.DrawLine(frac_pen5, frac_dots[4][i], frac_dots[4][i + 1]);
                            }
                            for (int i = 0; i < 243; i++)
                            {
                                e.Graphics.DrawLine(frac_pen6, frac_dots[5][i], frac_dots[5][i + 1]);
                            }
                            break;
                        }
                    case 6:
                        {
                            e.Graphics.DrawLine(frac_pen1, frac_dots[0][0], frac_dots[0][1]);
                            e.Graphics.DrawLine(frac_pen2, frac_dots[1][0], frac_dots[1][1]);
                            e.Graphics.DrawLine(frac_pen2, frac_dots[1][2], frac_dots[1][1]);
                            e.Graphics.DrawLine(frac_pen2, frac_dots[1][3], frac_dots[1][2]);
                            for (int i = 0; i < 9; i++)
                            {
                                e.Graphics.DrawLine(frac_pen3, frac_dots[2][i], frac_dots[2][i + 1]);
                            }
                            for (int i = 0; i < 27; i++)
                            {
                                e.Graphics.DrawLine(frac_pen4, frac_dots[3][i], frac_dots[3][i + 1]);
                            }
                            for (int i = 0; i < 81; i++)
                            {
                                e.Graphics.DrawLine(frac_pen5, frac_dots[4][i], frac_dots[4][i + 1]);
                            }
                            for (int i = 0; i < 243; i++)
                            {
                                e.Graphics.DrawLine(frac_pen6, frac_dots[5][i], frac_dots[5][i + 1]);
                            }
                            for (int i = 0; i < 729; i++)
                            {
                                e.Graphics.DrawLine(frac_pen7, frac_dots[6][i], frac_dots[6][i + 1]);
                            }
                            break;
                        }
                }
            }
            else if (crunch == 2)
            {
                switch (iterations_amount)
                {
                    case 0:
                        {
                            e.Graphics.DrawLine(frac_pen1, frac_dots[0][0], frac_dots[0][1]);
                            break;
                        }
                    case 1:
                        {
                            e.Graphics.DrawLine(frac_pen2, frac_dots[1][0], frac_dots[1][1]);
                            e.Graphics.DrawLine(frac_pen2, frac_dots[1][2], frac_dots[1][1]);
                            e.Graphics.DrawLine(frac_pen2, frac_dots[1][3], frac_dots[1][2]);
                            break;
                        }
                    case 2:
                        {
                            for (int i = 0; i < 9; i++)
                            {
                                e.Graphics.DrawLine(frac_pen4, frac_dots[2][i], frac_dots[2][i + 1]);
                            }
                            break;
                        }
                    case 3:
                        {
                            for (int i = 0; i < 27; i++)
                            {
                                e.Graphics.DrawLine(frac_pen4, frac_dots[3][i], frac_dots[3][i + 1]);
                            }
                            break;
                        }
                    case 4:
                        {
                           for (int i = 0; i < 81; i++)
                            {
                                e.Graphics.DrawLine(frac_pen5, frac_dots[4][i], frac_dots[4][i + 1]);
                            }
                            break;
                        }
                    case 5:
                        {
                            for (int i = 0; i < 243; i++)
                            {
                                e.Graphics.DrawLine(frac_pen6, frac_dots[5][i], frac_dots[5][i + 1]);
                            }
                            break;
                        }
                    case 6:
                        {
                            for (int i = 0; i < 729; i++)
                            {
                                e.Graphics.DrawLine(frac_pen7, frac_dots[6][i], frac_dots[6][i + 1]);
                            }
                            break;
                        }
                }
            }
        }
    }
}
