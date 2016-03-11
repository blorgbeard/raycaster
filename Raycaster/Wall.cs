using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raycaster
{
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

        public RayIntersection2D IntersectWall(Wall other)
        {
            var wallVector = Point2 - Point1;
            var otherVector = other.Point2 - other.Point1;            
            var i = new RayIntersection2D(new Ray(other.Point1, otherVector), new Ray(Point1, wallVector));
            if (i.Intersects && i.FirstRayDistance <= otherVector.Length && i.SecondRayDistance <= wallVector.Length)
            {
                return i;
            }
            else return RayIntersection2D.NoIntersection;
        }

        public float Length
        {
            get { return (Point2 - Point1).Length; }
        }
    }

}
