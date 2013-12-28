using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raycaster;
using System.Drawing;

namespace RaycasterTest
{
    [TestClass]
    public class Rectangle2DTest
    {
        [TestMethod]
        public void RectangleIntersectionWithRay()
        {
            Rectangle2D rect = new Rectangle2D() { Bounds = new RectangleF(2, 3, 3, 2) };
            Ray ray = new Ray(new Vector2D(2, 1), new Vector2D(1, 2));
            float d = rect.IntersectRay(ray);
            Assert.AreEqual((float)Math.Sqrt(1 * 1 + 2 * 2), d, 0.01F);
        }
    }
}
