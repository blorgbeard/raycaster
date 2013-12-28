using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raycaster
{
    class World
    {
    }

    struct RayIntersection2D
    {
        public float FirstRayDistance { get; private set; }
        public float SecondRayDistance { get; private set; }

        public bool Intersects
        {
            get
            {
                return FirstRayDistance >= 0 && SecondRayDistance >= 0;
            }
        }

        private void SetNoIntersect()
        {
            FirstRayDistance = float.MinValue;
            SecondRayDistance = float.MinValue;
        }

        public RayIntersection2D(Ray one, Ray two)
            : this()
        {
            /*
            dx = bs.x - as.x
            dy = bs.y - as.y
            det = bd.x * ad.y - bd.y * ad.x
            u = (dy * bd.x - dx * bd.y) / det
            v = (dy * ad.x - dx * ad.y) / det
            */

            var dx = two.Location.X - one.Location.X;
            var dy = two.Location.Y - one.Location.Y;
            var det = two.Direction.X * one.Direction.Y - two.Direction.Y * one.Direction.X;
            if (det == 0)
            {
                SetNoIntersect();
                return;
            }
            FirstRayDistance = (dy * two.Direction.X - dx * two.Direction.Y) / det;
            SecondRayDistance = (dy * one.Direction.X - dx * one.Direction.Y) / det;
        }
    }

    struct Vector2D
    {
        public float X { get; private set; }
        public float Y { get; private set; }

        public static readonly Vector2D North = new Vector2D(0, -1);
        public static readonly Vector2D South = new Vector2D(0, 1);
        public static readonly Vector2D East = new Vector2D(1, 0);
        public static readonly Vector2D West = new Vector2D(-1, 0);

        public Vector2D(float x, float y)
            : this()
        {
            X = x;
            Y = y;
        }

        public Vector2D(float angle)
            : this()
        {
            X = (float)Math.Cos(angle);
            Y = (float)Math.Sin(angle);
        }

        public float LengthSquared
        {
            get { return X * X + Y * Y; }
        }

        public float Length
        {
            get { return (float)Math.Sqrt(LengthSquared); }
        }

        [Obsolete("not a good idea, remove.", false)]
        public float Gradient
        {
            get { return Y / X; }
        }

        public float Angle
        {
            get
            {
                return (float)Math.Atan2(Y, X);
            }
        }

        public Vector2D Normalised()
        {
            float ls = LengthSquared;
            if (ls == 1f) return this;
            float l = (float)Math.Sqrt(ls);
            return new Vector2D(X / l, Y / l);
        }

        public static implicit operator Vector2D(PointF point)
        {
            return new Vector2D(point.X, point.Y);
        }

        public static implicit operator Vector2D(Point point)
        {
            return new Vector2D(point.X, point.Y);
        }

        public static implicit operator PointF(Vector2D vector)
        {
            return new PointF(vector.X, vector.Y);
        }

        public static Vector2D operator +(Vector2D first, Vector2D second)
        {
            return new Vector2D(first.X + second.X, first.Y + second.Y);
        }

        public static Vector2D operator -(Vector2D first, Vector2D second)
        {
            return new Vector2D(first.X - second.X, first.Y - second.Y);
        }

        public static Vector2D operator *(Vector2D vector, float magnitude)
        {
            return new Vector2D(vector.X * magnitude, vector.Y * magnitude);
        }

        public static Vector2D operator /(Vector2D vector, float magnitude)
        {
            return new Vector2D(vector.X / magnitude, vector.Y / magnitude);
        }
    }

    struct Ray {
        public Vector2D Location { get; private set; }
        public Vector2D Direction { get; private set; }

        public Ray(Vector2D start, Vector2D direction)
            : this()
        {
            Location = start;
            Direction = direction.Normalised();
        }

        public float IntersectPlaneX(float y)
        {
            // y = mx + c
            // y = y1 + m(x - x1)
            // x = ((y - y1) / m) + x1
            /*                       _...
             *                   _ /
             * _______________ /_____________ y
             *            _ /  |
             *        _ /      |
             *      /          | x
             *    *
             *  x1,y1
             */

            var i = new RayIntersection2D(this, new Ray(new Vector2D(0, y), new Vector2D(1, 0)));
            if (i.Intersects)
            {
                return i.FirstRayDistance;
            }
            return -1;
            //return ((y - Start.Y) / Direction.Gradient) + Start.X;

        }

        public float IntersectPlaneY(float x)
        {
            // y = mx + c
            // y = y1 + m(x - x1)
            
            /*                 |     _...
             *                 | _ /
             * _______________ /_____________ y
             *            _ /  |
             *        _ /      |
             *      /          | x
             *    *
             *  x1,y1
             */
            var i = new RayIntersection2D(this, new Ray(new Vector2D(x, 0), new Vector2D(0, 1)));
            if (i.Intersects)
            {
                return i.FirstRayDistance;
            }
            return -1;
            //return Start.Y + (Direction.Gradient * (x - Start.X));
        }
    }

    abstract class Shape2D
    {

        protected float DistanceRayStartPointF(Ray ray, float x, float y)
        {
            float xd = x - ray.Location.X;
            float yd = y - ray.Location.Y;
            //return xd; //temp!
            if (xd < 0) return -1;
            if (yd < 0) return -1;
            return (float)Math.Sqrt(xd * xd + yd * yd);
        }

        //public abstract RectangleF BoundingBox { get; }

        public abstract float IntersectRay(Ray ray);

        public bool Hit { get; set; }
    }

    class Rectangle2D : Shape2D
    {
        public RectangleF Bounds { get; set; }

        public int ColorIndex { get; set; }

        public override float IntersectRay(Ray ray)
        {
            // ray can possibly intersect 1 or 2 sides.
            // which side(s) depends on where the ray start point is.

            // can intersect left side if start.x < left
            if (ray.Location.X < Bounds.Left)
            {
                var i = new RayIntersection2D(ray, new Ray(Bounds.Location, Vector2D.South));
                if (i.Intersects && i.SecondRayDistance <= Bounds.Size.Height)
                {
                    return i.FirstRayDistance;
                }
            }
            
            // can intersect with bottom side if start.y > bottom
            if (ray.Location.Y > Bounds.Bottom)
            {
                var i = new RayIntersection2D(ray, new Ray(new Vector2D(Bounds.Left, Bounds.Bottom), Vector2D.East));
                if (i.Intersects && i.SecondRayDistance <= Bounds.Size.Width)
                {
                    return i.FirstRayDistance;
                }
            }

            // can intersect right side if start.x > right
            if (ray.Location.X > Bounds.Right)
            {
                var i = new RayIntersection2D(ray, new Ray(new Vector2D(Bounds.Right, Bounds.Bottom), Vector2D.North));
                if (i.Intersects && i.SecondRayDistance <= Bounds.Size.Width)
                {
                    return i.FirstRayDistance;
                }
            }

            // can intersect with top side if start.y < top
            if (ray.Location.Y < Bounds.Top)
            {
                var i = new RayIntersection2D(ray, new Ray(Bounds.Location, Vector2D.East));
                if (i.Intersects && i.SecondRayDistance <= Bounds.Size.Height)
                {
                    return i.FirstRayDistance;
                }
            }
            /*

            // can intersect right side if start.x > right
            if (ray.Start.X > Bounds.Right)
            {
                var y = ray.IntersectPlaneY(Bounds.Right);
                if (y >= Bounds.Top && y <= Bounds.Bottom)
                {
                    // intersects with right edge!
                    return DistanceRayStartPointF(ray, Bounds.Right, y);
                }
            }

            // can intersect with top side if start.y < top
            if (ray.Start.Y < Bounds.Top)
            {
                var x = ray.IntersectPlaneX(Bounds.Top);
                if (x >= Bounds.Left && x <= Bounds.Right)
                {
                    // intersects with bottom edge!
                    return DistanceRayStartPointF(ray, x, Bounds.Top);
                }
            }

             */
            // must not intersect at all!
            return -1;
        }
    }
}
