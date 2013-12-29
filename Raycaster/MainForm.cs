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

        List<Wall> Walls;

        Ray Player;

        Bitmap Buffer3D = new Bitmap(320, 240);
        Bitmap BufferMap = new Bitmap(240, 240);

        Image Bricks;

        private List<Wall> BuildBlocksFromMap(byte[,] map)
        {
            var colors = new[] { Color.Green, Color.Blue};
            var result = new List<Wall>();
            for (int y = 0; y < map.GetLength(0); y++)
            {
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    if (map[y, x] != 0)
                    {
                        result.Add(new Wall(new Vector2D(x*10f, y*10f), new Vector2D(x*10f + 10f, y*10f), colors[map[y,x]-1]));
                        result.Add(new Wall(new Vector2D(x*10f + 10f, y*10f), new Vector2D(x*10f + 10f, y*10f + 10f), colors[map[y,x]-1]));
                        result.Add(new Wall(new Vector2D(x*10f + 10f, y*10f + 10f), new Vector2D(x*10f, y*10f + 10f), colors[map[y,x]-1]));
                        result.Add(new Wall(new Vector2D(x*10f, y*10f + 10f), new Vector2D(x*10f, y*10f), colors[map[y,x]-1]));
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

            Walls = BuildBlocksFromMap(map);

            Player = new Ray(new Vector2D(50, 50), new Vector2D(1, 0));

            Bricks = ResourceLoader.GetTexture("brick1.png");
        }

        private void PaintMapBlocks(Graphics g)
        {            
            foreach (var shape in GetWalls())
            {
                g.DrawLine(shape.Hit ? Pens.Red: Pens.Black, shape.Point1, shape.Point2);
            }
            if (_ErasingWall != null)
            {
                g.DrawLine(Pens.Red, _ErasingWall.Point1, _ErasingWall.Point2);
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
            foreach (var shape in Walls)
                shape.Hit = false;

            using (var map = Graphics.FromImage(BufferMap))
            using (var gfx = Graphics.FromImage(Buffer3D))
            {
                gfx.Clear(Color.Cornsilk);
                gfx.FillRectangle(Brushes.OliveDrab, 0, 120, 320, 120);

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
                    Wall hitWall = null;
                    RayIntersection2D hit = new RayIntersection2D();
                    foreach (var shape in GetWalls())
                    {
                        var i = shape.IntersectRay(ray);
                        if (!i.Intersects) continue;
                        var d = i.FirstRayDistance;
                        if (d < closest || closest < 0)
                        {
                            closest = d;
                            hitWall = shape;
                            hit = i;
                        }
                    }

                    if (hitWall != null)
                    {
                        // compensate for fish-eye effect:
                        // project intersection point onto camera direction.
                        var point = ray.Direction * closest;          
                        var distance = Vector2D.Projection(point, Player.Direction).Length;

                        if (distance == 0) distance = float.Epsilon;

                        hitWall.Hit = true;
                        int sliceHeight = Math.Max(0, Math.Min(240, (int)(240F / distance * 10F)));
                        
                        var tx = (int)(Math.Abs(hit.SecondRayDistance) / hitWall.Length * Bricks.Width);
                        gfx.DrawImage(
                            Bricks,
                            new Rectangle(px, 120 - (sliceHeight / 2), 1, sliceHeight),
                            new Rectangle(tx, 0, 1, Bricks.Height),
                            GraphicsUnit.Pixel);


                        float lightness = ((sliceHeight / 240F * 200) + 20) / 255f;
                        
                        // tint looks weird
                        //Pen pen = new Pen(Color.FromArgb(128, hitWall.Tint));
                        //gfx.DrawLine(pen, px, 120 - (sliceHeight / 2), px, 120 + (sliceHeight / 2));
                        
                        Pen pen = new Pen(Color.FromArgb((int)(240 * (1f - lightness)), Color.Black));
                        gfx.DrawLine(pen, px, 120 - (sliceHeight / 2), px, 120 + (sliceHeight / 2));
                        
                        map.DrawLine(new Pen(hitWall.Tint), ray.Location, ray.Location + ray.Direction * closest);
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

        private IEnumerable<Wall> GetWalls()
        {
            return Walls.Union(new[] { _DrawingWall }.Except(new Wall[] { null }));
        }

        private Color SetLightness(Color color, float lightness)
        {
            return Color.FromArgb((int)(color.R * lightness), (int)(color.G * lightness), (int)(color.B * lightness));
        }

        private void pbMain_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImageUnscaled(Buffer3D, 0, 0);            
        }

        private void pbMap_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImageUnscaled(BufferMap, 0, 0);
        }

        private Wall _DrawingWall = null;
        private Wall _ErasingWall = null;

        private void pbMap_MouseDown(object sender, MouseEventArgs e)
        {
            if (radMovePlayer.Checked)
            {
                MovePlayerByMouse(e);
            }
            else if (radDrawWalls.Checked)
            {
                if (e.Button == MouseButtons.Left)
                {
                    _DrawingWall = new Wall(e.Location, e.Location, Color.Orange);
                }
                else if (e.Button == MouseButtons.Right)
                {
                    _ErasingWall = new Wall(e.Location, e.Location, Color.Empty);
                }
            }
        }

        private void pbMap_MouseMove(object sender, MouseEventArgs e)
        {
            if (radMovePlayer.Checked)
            {
                MovePlayerByMouse(e);
            }
            else if (radDrawWalls.Checked)
            {
                if (e.Button == MouseButtons.Left && _DrawingWall != null)
                {
                    _DrawingWall.Point2 = e.Location;
                }
                else if (e.Button == MouseButtons.Right && _ErasingWall != null)
                {
                    _ErasingWall.Point1 = _ErasingWall.Point2;
                    _ErasingWall.Point2 = e.Location;
                    foreach (var wall in Walls.ToArray())
                    {
                        if (wall.IntersectWall(_ErasingWall).Intersects)
                        {
                            Walls.Remove(wall);
                        }
                    }                    
                }
            }
        }

        private void pbMap_MouseUp(object sender, MouseEventArgs e)
        {
            if (radDrawWalls.Checked && e.Button == MouseButtons.Left && _DrawingWall != null)
            {
                _DrawingWall.Point2 = e.Location;
                _DrawingWall.Tint = Color.DarkOrange;
                Walls.Add(_DrawingWall);             
            }
            _DrawingWall = null;
            _ErasingWall = null;
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
