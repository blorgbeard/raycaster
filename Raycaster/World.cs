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
        public static readonly RayIntersection2D NoIntersection = new RayIntersection2D()
        {
            FirstRayDistance = float.MinValue,
            SecondRayDistance = float.MinValue
        };

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

    class Wall
    {
        public Vector2D Point1 { get; set; }
        public Vector2D Point2 { get; set; }

        public Color Tint { get; set; }

        public bool Hit { get; set; }

        public Wall(Vector2D start, Vector2D end, Color tint)
        {
            Point1 = start;
            Point2 = end;
            Tint = tint;
        }

        public RayIntersection2D IntersectRay(Ray ray)
        {
            var wallVector = Point2 - Point1;
            var i = new RayIntersection2D(ray, new Ray(Point1, wallVector));
            if (i.Intersects && i.SecondRayDistance <= wallVector.Length)
            {
                return i;
            }
            else return RayIntersection2D.NoIntersection;
        }
    }

}
