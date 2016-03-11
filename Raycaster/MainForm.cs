using dx = SharpDX;
using dx2d = SharpDX.Direct2D1;
using dxgi = SharpDX.DXGI;
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

        List<QuadNode> Quads;

        Ray Player;

        public dx2d.WindowRenderTarget Target2D { get; private set; }        

        Bitmap BufferMap = new Bitmap(240, 240);

        dx2d.Brush solidBlackBrush;

        //System.Drawing.Image Bricks;

        private List<Wall> BuildWallsFromMap(byte[,] map)
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

        private bool QuadContainsPoint(Vector2D topLeft, Vector2D bottomRight, Vector2D point)
        {
            return
                (point.X >= topLeft.X) &&
                (point.X <= bottomRight.X) &&
                (point.Y >= topLeft.Y) &&
                (point.Y <= bottomRight.Y);
        }

        private bool QuadContainsWall(Vector2D topLeft, Vector2D bottomRight, Wall wall)
        {
            if (QuadContainsPoint(topLeft, bottomRight, wall.Point1) || QuadContainsPoint(topLeft, bottomRight, wall.Point2))
                return true;

            // might also overlap corner without either point being inside.
            // todo!
            return false;
        }

        private QuadNode BuildQuads(Vector2D topLeft, Vector2D bottomRight)
        {
            if ((bottomRight - topLeft).Length <= trkQuadSize.Value)
            {
                // leaf times
                var children = Walls.Where(t => QuadContainsWall(topLeft, bottomRight, t)).ToList();
                WallsPerQuad.Add(children.Count);
                return new QuadNode(topLeft, bottomRight, children);
            }
            else
            {
                // node times
                var centre = new Vector2D(
                    topLeft.X + ((bottomRight.X - topLeft.X) / 2f), 
                    topLeft.Y + ((bottomRight.Y - topLeft.Y) / 2f));

                var q1 = BuildQuads(topLeft, centre);                
                var q2 = BuildQuads(new Vector2D(centre.X, topLeft.Y), new Vector2D(bottomRight.X, centre.Y));                
                var q3 = BuildQuads(new Vector2D(topLeft.X, centre.Y), new Vector2D(centre.X, bottomRight.Y));
                var q4 = BuildQuads(centre, bottomRight);
                var children = new[] { q1, q2, q3, q4 }.Where(t => t.ChildNodes.Any() || t.ChildWalls.Any());
                return new QuadNode(topLeft, bottomRight, children);
            }
        }

        private readonly List<int> WallsPerQuad = new List<int>();


        private void trkQuadSize_Scroll(object sender, EventArgs e)
        {
            RebuildQuads();
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

            Walls = BuildWallsFromMap(map);
            RebuildQuads();



            Player = new Ray(new Vector2D(50, 50), new Vector2D(1, 0));



            //Bricks = ResourceLoader.GetTexture("brick1.png");
        }

        private void RebuildQuads()
        {
            WallsPerQuad.Clear();
            Quads = new List<QuadNode> {
                BuildQuads(new Vector2D(0, 0), new Vector2D(320, 240))
            };
            txtQuadInfo.Text = string.Format(
                "Average walls per quad: {0:0.00}\r\nNumber of leaf quads: {1}",
                WallsPerQuad.Average(),
                WallsPerQuad.Count);
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
            Stack<QuadNode> s = new Stack<QuadNode>();
            s.Push(Quads[0]);
            while (s.Any())
            {
                var q = s.Pop();
                g.DrawRectangle(Pens.Aqua, q.Bounds.X, q.Bounds.Y, q.Bounds.Width, q.Bounds.Height);
                foreach (var qq in q.ChildNodes)
                    s.Push(qq);
            }
        }

        private void PaintMapPlayer(Graphics g)
        {
            g.FillEllipse(Brushes.Blue, new Rectangle((int)Player.Location.X - 3, (int)Player.Location.Y - 3, 6, 6));
        }

        private void tmrTick_Tick(object sender, EventArgs e)
        {
            tmrTick.Enabled = false;
            Render();
            tmrTick.Enabled = true;
        }

        private IEnumerable<Wall> GetPossibleHitWalls(Ray ray, IEnumerable<QuadNode> quads)
        {
            foreach (var quad in quads)
            {
                if (quad.Intersects(ray))
                {
                    foreach (var wall in quad.ChildWalls)
                    {                        
                        yield return wall;
                    }
                    foreach (var wall in GetPossibleHitWalls(ray, quad.ChildNodes))
                        yield return wall;
                }
            }
        }

        private void Render()
        {
            var tStart = DateTime.Now;
            TimeSpan tsHitCheck = TimeSpan.Zero;
            TimeSpan tsPainting = TimeSpan.Zero;

            foreach (var shape in Walls)
                shape.Hit = false;

            Target2D.BeginDraw();

            Target2D.Clear(dx.Color.Cornsilk);
            Target2D.FillRectangle(
                new dx.RectangleF(0, 120, 320, 120),
                new dx2d.SolidColorBrush(Target2D, dx.Color.OliveDrab));
            

            using (var map = Graphics.FromImage(BufferMap))            
            {                
                map.Clear(Color.White);
                Wall hitWall = null;

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

                    var tStartHitCheck = DateTime.Now;

                    float closest = -1;                    
                    RayIntersection2D hit = new RayIntersection2D();
                    foreach (var shape in /*GetWalls()*/ GetPossibleHitWalls(ray, Quads))
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

                    tsHitCheck += (DateTime.Now - tStartHitCheck);

                    if (hitWall != null)
                    {
                        // compensate for fish-eye effect:
                        // project intersection point onto camera direction.
                        var point = ray.Direction * closest;          
                        var distance = Vector2D.Projection(point, Player.Direction).Length;

                        if (distance == 0) distance = float.Epsilon;

                        hitWall.Hit = true;
                        int sliceHeight = Math.Max(0, Math.Min(240, (int)(240F / distance * 10F)));
                        float lightness = ((sliceHeight / 240F * 200) + 20) / 255f;

                        if (chkTextures.Checked)
                        {

                            /*
                            var tx = (int)(Math.Abs(hit.SecondRayDistance) / hitWall.Length * Bricks.Width);
                            gfx.DrawImage(
                                Bricks,
                                new Rectangle(px, 120 - (sliceHeight / 2), 1, sliceHeight),
                                new Rectangle(tx, 0, 1, Bricks.Height),
                                GraphicsUnit.Pixel);

                            Pen pen = new Pen(Color.FromArgb((int)(240 * (1f - lightness)), Color.Black));
                            gfx.DrawLine(pen, px, 120 - (sliceHeight / 2), px, 120 + (sliceHeight / 2));
                            */
                        }
                        else
                        {
                            // tint looks weird with textures..
                            //Pen pen = new Pen(SetLightness(hitWall.Tint, lightness));
                            //gfx.DrawLine(pen, px, 120 - (sliceHeight / 2), px, 120 + (sliceHeight / 2));
                            Target2D.DrawLine(
                                new dx.Vector2(px, 120 - (sliceHeight / 2)),
                                new dx.Vector2(px, 120 + (sliceHeight / 2)),
                                solidBlackBrush);
                        }
                        
                        map.DrawLine(new Pen(hitWall.Tint), ray.Location, ray.Location + ray.Direction * closest);
                    }
                    else
                    {
                        map.DrawLine(Pens.LightGray, ray.Location, ray.Location + ray.Direction * 1000f);
                    }

                }

                var spf = DateTime.Now - tStart;
                txtInfo.Text = string.Format("ms to render: {0}\r\nms to hitcheck: {1}",
                    spf.TotalMilliseconds, tsHitCheck.TotalMilliseconds);

                PaintMapBlocks(map);
                PaintMapPlayer(map);
            }
            
            Target2D.EndDraw();
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
            //e.Graphics.DrawImageUnscaled(Buffer3D, 0, 0);            
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

        private void btnClearWalls_Click(object sender, EventArgs e)
        {
            Walls.Clear();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            var Factory2D = new SharpDX.Direct2D1.Factory();

            var properties = new dx2d.HwndRenderTargetProperties();
            properties.Hwnd = dxRender.Handle;
            properties.PixelSize = new dx.Size2(dxRender.ClientSize.Width, dxRender.ClientSize.Height);
            properties.PresentOptions = dx2d.PresentOptions.None;

            Target2D = new dx2d.WindowRenderTarget(
                Factory2D, 
                new dx2d.RenderTargetProperties(
                    new dx2d.PixelFormat(dxgi.Format.Unknown, dx2d.AlphaMode.Ignore)), properties);
            Target2D.AntialiasMode = dx2d.AntialiasMode.Aliased;            
            
            solidBlackBrush = new dx2d.SolidColorBrush(Target2D, dx.Color.Black);
        }

        private void dxRender_Paint(object sender, PaintEventArgs e)
        {
           
        }


    }
}