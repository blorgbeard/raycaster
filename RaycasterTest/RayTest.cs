using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raycaster;
using System.Drawing;

namespace RaycasterTest
{
    [TestClass]
    public class RayTest
    {
        [TestMethod]
        public void RayIntersectingPlaneXWorksWhenRayStartsFromOriginAndGradientIsOne()
        {
            Ray ray = new Ray(new Vector2D(0, 0), new Vector2D(1,1));
            var d = ray.IntersectPlaneX(1);
            Assert.AreEqual(1F, d, 0.01F);
        }

        [TestMethod]
        public void RayIntersectingPlaneXWorksWhenRayStartsFromArbitraryPointAndGradientIsOne()
        {
            Ray ray = new Ray(new Vector2D(1, 3), new Vector2D(1, 1));
            var d = ray.IntersectPlaneX(4);
            Assert.AreEqual(2F, d, 0.01F);
        }

        [TestMethod]
        public void RayIntersectingPlaneXWorksWhenRayStartsFromArbitraryPointAndGradientIsNotOne()
        {
            Ray ray = new Ray(new Vector2D(3, 2), new Vector2D(3, 1));
            var d = ray.IntersectPlaneX(3);
            Assert.AreEqual(6F, d, 0.01F);
        }

        [TestMethod]
        public void RayIntersectingPlaneYWorksWhenRayStartsFromOriginAndGradientIsOne()
        {
            Ray ray = new Ray(new Vector2D(0, 0), new Vector2D(1, 1));
            var d = ray.IntersectPlaneY(1);
            Assert.AreEqual(1F, d, 0.01F);
        }

        [TestMethod]
        public void RayIntersectingPlaneYWorksWhenRayStartsFromArbitraryPointAndGradientIsOne()
        {
            Ray ray = new Ray(new Vector2D(1, 3), new Vector2D(1, 1));
            var d = ray.IntersectPlaneY(4);
            Assert.AreEqual(6F, d, 0.01F);
        }

        [TestMethod]
        public void RayIntersectingPlaneYWorksWhenRayStartsFromArbitraryPointAndGradientIsNotOne()
        {
            Ray ray = new Ray(new Vector2D(3, 2), new Vector2D(3, 1));
            var d = ray.IntersectPlaneY(6);
            Assert.AreEqual(3F, d, 0.01F);
        }
    }
}
