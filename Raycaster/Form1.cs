using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Raycaster
{    

    public partial class Form1 : Form
    {

        List<Shape2D> Blocks;

        Ray Player;

        Bitmap bmp = new Bitmap(320, 240);


        private List<Shape2D> BuildBlocksFromMap(byte[,] map)
        {
            var result = new List<Shape2D>();
            for (int y = 0; y < map.GetLength(0); y++)
            {
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    if (map[y, x] != 0)
                    {
                        result.Add(new Rectangle2D() { Bounds = new RectangleF(x * 10, y * 10, 10, 10) });
                    }
                }
            }
            return result;
        }

        public Form1()
        {
            InitializeComponent();

            byte[,] map = {
                {1,1,1,1,1,1,1,1,1,1},
                {1,0,0,0,0,0,0,0,0,1},
                {1,0,0,0,1,1,0,0,0,1},
                {1,0,1,0,0,0,0,0,0,1},
                {1,0,1,0,0,0,0,1,0,1},
                {1,1,1,0,0,0,0,1,0,1},
                {1,0,0,0,1,0,0,1,1,1},
                {1,0,0,0,1,0,0,0,0,1},
                {1,0,0,0,1,0,0,0,0,1},
                {1,1,1,1,1,1,1,1,1,1},    
            };

            /*
            map = new byte [,] {
                {0,0,1,1},
                {0,0,0,1},
                {0,0,0,1},
                {0,0,1,1},                
            };
            //*/
            Blocks = BuildBlocksFromMap(map);

            /*Blocks = new List<Shape2D>()
            {
                new Rectangle2D() { Bounds = new RectangleF(10000,0,1000,10000)}
            };*/

            Player = new Ray(new Vector2D(0, 25), new Vector2D(1, 0));
        }

        private void tmrTick_Tick(object sender, EventArgs e)
        {

            foreach (var shape in Blocks)
                shape.Hit = false;

            using (var g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Cornsilk);

                float screenDistanceFromPlayer = 320F / (float)Math.Tan(Math.PI / 4);
                for (int px = 0; px < 320; px++)
                {

                    var perp = new Ray(
                        Player.Location + (Player.Direction * screenDistanceFromPlayer), 
                        new Vector2D(-Player.Direction.Y, -Player.Direction.X));

                    var ray = new Ray(Player.Location,
                        perp.Location +
                        (perp.Direction * (px - 160)) -
                        Player.Location);

                    float closest = -1;
                    Shape2D hit = null;
                    foreach (var shape in Blocks)
                    {
                        var d = shape.IntersectRay(ray);
                        if (d < 0) continue;
                        if (d < closest || closest < 0)
                        {
                            closest = d;
                            hit = shape;
                        }
                    }

                    if (hit != null)
                        hit.Hit = true;
                    
                    if (closest >= 0)
                    {
                        int sliceHeight = (int)Math.Min(240, Math.Max(0, Math.Abs(2400F / closest)));
                        int color = (int)(sliceHeight / 240F * 200);
                        Pen pen = new Pen(Color.FromArgb(color, color, color));
                        g.DrawLine(pen, px, 120 - (sliceHeight / 2), px, 120 + (sliceHeight / 2));
                    }

                }
            }
            pictureBox1.Invalidate();
            pictureBox2.Invalidate();
            
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImageUnscaled(bmp, 0, 0);            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Player = new Ray(Player.Location + new Vector2D(-5, 0), Player.Direction);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Player = new Ray(Player.Location + new Vector2D(5, 0), Player.Direction);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Player = new Ray(Player.Location + new Vector2D(0, -5), Player.Direction);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Player = new Ray(Player.Location + new Vector2D(0, 5), Player.Direction);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Player = new Ray(Player.Location, new Vector2D(Player.Direction.Angle - 0.314f));
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Player = new Ray(Player.Location, new Vector2D(Player.Direction.Angle + 0.314f));
        }

        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.Cornsilk);
            foreach (var shape in Blocks.OfType<Rectangle2D>())
            {
                e.Graphics.FillRectangle(shape.Hit ? Brushes.Red : Brushes.Black, shape.Bounds);
            }

            e.Graphics.FillEllipse(Brushes.Blue, new Rectangle((int)Player.Location.X - 4, (int)Player.Location.Y - 4, 8, 8));
            e.Graphics.DrawLine(Pens.Blue, 
                Player.Location.X, Player.Location.Y, 
                Player.Location.X + Player.Direction.X * 10, 
                Player.Location.Y + Player.Direction.Y * 10);
        }



        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            MovePlayerByMouse(e);
        }

        private void pictureBox2_MouseMove(object sender, MouseEventArgs e)
        {
            MovePlayerByMouse(e);
        }

        private void MovePlayerByMouse(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Player = new Ray(e.Location, Player.Direction);
            }
            else if (e.Button == MouseButtons.Left)
            {
                Player = new Ray(Player.Location, e.Location - Player.Location);
            }
        }
    }
}
