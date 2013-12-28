using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raycaster
{
    struct RayIntersection2D
    {
        public float FirstRayDistance { get; private set; }
        public float SecondRayDistance { get; private set; }

        public bool Intersects
        {
            get { return FirstRayDistance >= 0 && SecondRayDistance >= 0; }
        }

        private void SetNoIntersect()
        {
            FirstRayDistance = float.MinValue;
            SecondRayDistance = float.MinValue;
        }

        public RayIntersection2D(Ray one, Ray two)
            : this()
        {

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


    struct Ray {
        public Vector2D Location { get; private set; }
        public Vector2D Direction { get; private set; }

        public Ray(Vector2D start, Vector2D direction)
            : this()
        {
            Location = start;
            Direction = direction.Normalised();
        }
    }

    abstract class Shape2D
    {

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
            
            // must not intersect at all!
            return -1;
        }
    }
}
