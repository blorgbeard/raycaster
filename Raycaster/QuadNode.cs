using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Raycaster
{
    class QuadNode
    {
        private Ray[] Sides;
        private float[] Lengths;

        public RectangleF Bounds { get; private set; }

        public IEnumerable<QuadNode> ChildNodes { get; private set; }

        public IEnumerable<Wall> ChildWalls { get; private set; }

        protected QuadNode(Vector2D topLeft, Vector2D bottomRight)
        {
            var topRight = new Vector2D(bottomRight.X, topLeft.Y);
            var bottomLeft = new Vector2D(topLeft.X, bottomRight.Y);
            Bounds = new RectangleF(topLeft.X, topLeft.Y, bottomRight.X - topLeft.X, bottomRight.Y - topLeft.Y);
            Sides = new[] {
                        new Ray(topLeft, Vector2D.East),
                        new Ray(topRight, Vector2D.South),
                        new Ray(bottomRight, Vector2D.West),
                        new Ray(bottomLeft, Vector2D.North)
                    };
            Lengths = new[] {
                        (topRight - topLeft).Length,
                        (bottomRight - topRight).Length,
                        (bottomLeft - bottomRight).Length,
                        (topLeft - bottomLeft).Length
                    };
        }

        public QuadNode(Vector2D topLeft, Vector2D bottomRight, IEnumerable<QuadNode> children) :
            this(topLeft, bottomRight)
        {
            ChildNodes = children.ToArray();
            ChildWalls = new Wall[0];
        }

        public QuadNode(Vector2D topLeft, Vector2D bottomRight, IEnumerable<Wall> children) :
            this(topLeft, bottomRight)
        {
            ChildNodes = new QuadNode[0];
            ChildWalls = children.ToArray();
        }

        public bool Intersects(Ray ray)
        {
            return
                RayIntersectsSide(ray, 0) ||
                RayIntersectsSide(ray, 1) ||
                RayIntersectsSide(ray, 2) ||
                RayIntersectsSide(ray, 3);
        }

        private bool RayIntersectsSide(Ray ray, int x)
        {
            var i = new RayIntersection2D(ray, Sides[x]);
            return i.Intersects && i.SecondRayDistance <= Lengths[x];
        }
    }
}
