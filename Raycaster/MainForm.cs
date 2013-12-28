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
    public partial class MainForm : Form
    {

        List<Rectangle2D> Blocks;

        Ray Player;

        Bitmap Buffer3D = new Bitmap(320, 240);
        Bitmap BufferMap = new Bitmap(240, 240);

        private List<Rectangle2D> BuildBlocksFromMap(byte[,] map)
        {
            var result = new List<Rectangle2D>();
            for (int y = 0; y < map.GetLength(0); y++)
            {
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    if (map[y, x] != 0)
                    {
                        result.Add(new Rectangle2D() { 
                            ColorIndex = map[y,x],
                            Bounds = new RectangleF(x * 10, y * 10, 10, 10) 
                        });
                    }
                }
            }
            return result;
        }

        public MainForm()
        {
            InitializeComponent();

            byte[,] map = {
                {2,1,2,1,2,1,2,1,2,1,2,1,2,1,2,1,2,1,2,1},
                {1,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,2},
                {2,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,1},
                {1,0,0,2,2,2,2,2,0,0,0,0,1,1,1,1,1,0,0,2},
                {2,0,0,0,0,0,0,0,0,0,0,0,0,2,0,0,1,0,0,1},
                {1,0,0,0,0,0,0,0,0,0,0,0,2,2,0,0,1,0,0,2},
                {2,0,0,0,0,1,0,0,2,0,0,0,0,0,0,0,1,0,0,1},
                {1,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,1,0,0,2},
                {2,0,0,0,0,1,0,0,0,0,1,1,1,1,1,1,1,0,0,1},
                {1,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,2},
                {2,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2},
                {2,0,0,0,0,0,0,0,1,2,1,0,1,2,1,0,1,0,0,1},
                {1,0,0,0,0,0,0,0,2,0,2,1,2,0,2,1,2,0,0,2},
                {2,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,2,0,0,1},
                {1,0,0,2,1,2,1,0,2,0,0,0,0,0,0,0,2,0,0,2},
                {2,0,0,1,0,0,2,0,1,0,0,0,0,0,0,0,2,0,0,1},                
                {1,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,2,0,0,2},
                {2,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,2,0,0,1},
                {1,2,1,2,1,2,1,2,1,2,1,2,1,2,1,2,1,2,1,2}

                
            };

            Blocks = BuildBlocksFromMap(map);

            Player = new Ray(new Vector2D(50, 50), new Vector2D(1, 0));
        }

        private void PaintMapBlocks(Graphics g)
        {            
            foreach (var shape in Blocks.OfType<Rectangle2D>())
            {
                g.FillRectangle(shape.Hit ? Brushes.Red : Brushes.Black, shape.Bounds);
            }            
        }

        private void PaintMapPlayer(Graphics g)
        {
            g.FillEllipse(Brushes.Blue, new Rectangle((int)Player.Location.X - 3, (int)Player.Location.Y - 3, 6, 6));
        }

        private void tmrTick_Tick(object sender, EventArgs e)
        {
            Render();
        }

        private void Render()
        {
            foreach (var shape in Blocks)
                shape.Hit = false;

            using (var map = Graphics.FromImage(BufferMap))
            using (var gfx = Graphics.FromImage(Buffer3D))
            {
                gfx.Clear(Color.Cornsilk);
                map.Clear(Color.White);                

                float screenDistanceFromPlayer = 320F / (float)Math.Tan(Math.PI / 4);
                for (int px = 0; px < 320; px++)
                {

                    var perp = new Ray(
                        Player.Location + (Player.Direction * screenDistanceFromPlayer),
                        new Vector2D(-Player.Direction.Y, Player.Direction.X));

                    var ray = new Ray(Player.Location,
                        perp.Location +
                        (perp.Direction * (px - 160)) -
                        Player.Location);

                    float closest = -1;
                    Rectangle2D hit = null;
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
                    {
                        // compensate for fish-eye effect:
                        // project intersection point onto screen.
                        var point = ray.Location + ray.Direction * closest;
                        var pp = (perp.Location - point);
                        var ppdn = pp.Dot(perp.Direction);
                        var distance = Math.Abs((pp - (perp.Direction * ppdn)).Length);
                        //


                        hit.Hit = true;
                        int sliceHeight = (int)Math.Min(240, Math.Max(0, Math.Abs(4000F / closest)));
                        int color = (int)(sliceHeight / 240F * 200);
                        Pen pen = new Pen(Color.FromArgb(hit.ColorIndex == 1 ? color : 0, color, color));
                        gfx.DrawLine(pen, px, 120 - (sliceHeight / 2), px, 120 + (sliceHeight / 2));

                        map.DrawLine(pen, ray.Location, ray.Location + ray.Direction * closest);
                    }
                    else
                    {
                        map.DrawLine(Pens.LightGray, ray.Location, ray.Location + ray.Direction * 1000f);
                    }

                }
                PaintMapBlocks(map);
                PaintMapPlayer(map);
            }
            pbMain.Invalidate();
            pbMap.Invalidate();

        }

        private void pbMain_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImageUnscaled(Buffer3D, 0, 0);            
        }

        private void pbMap_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImageUnscaled(BufferMap, 0, 0);
        }

        private void pbMap_MouseDown(object sender, MouseEventArgs e)
        {
            MovePlayerByMouse(e);
        }

        private void pbMap_MouseMove(object sender, MouseEventArgs e)
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
