using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raycaster
{
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

        public static Vector2D Rejection(Vector2D a, Vector2D b)
        {
            return a - ((a.Dot(b) / b.Dot(b)) * b);
        }

        public static Vector2D Projection(Vector2D a, Vector2D b)
        {
            return (a.Dot(b) / b.Dot(b)) * b;
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

        public static Vector2D operator *(float magnitude, Vector2D vector)
        {
            return vector * magnitude;
        }

        public static Vector2D operator /(Vector2D vector, float magnitude)
        {
            return new Vector2D(vector.X / magnitude, vector.Y / magnitude);
        }

        public static float Dot(Vector2D first, Vector2D second)
        {
            return first.X * second.X + first.Y * second.Y;
        }

        public float Dot(Vector2D other)
        {
            return Dot(this, other);
        }
    }
}
