using System;
using System.Collections.Generic;
using System.Linq;

namespace Raycaster
{
    struct Ray
    {
        public Vector2D Location { get; private set; }
        public Vector2D Direction { get; private set; }

        public Ray(Vector2D start, Vector2D direction)
            : this()
        {
            Location = start;
            Direction = direction.Normalised();
        }
    }
}
