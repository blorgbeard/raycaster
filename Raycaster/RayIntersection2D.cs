using System;
using System.Collections.Generic;
using System.Linq;

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
}
