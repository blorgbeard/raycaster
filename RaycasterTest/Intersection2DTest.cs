using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raycaster;

namespace RaycasterTest
{
    [TestClass]
    public class Intersection2DTests
    {
        [TestMethod]
        public void SimpleIntersectionOfTwoRaysIsCorrect()
        {
            Ray r1 = new Ray(new Vector2D(0, 0), new Vector2D(1, 1));
            Ray r2 = new Ray(new Vector2D(1, 0), new Vector2D(0, 1));
            var i = new RayIntersection2D(r1, r2);
            Assert.IsTrue(i.Intersects);
            Assert.AreEqual(1f, i.SecondRayDistance, 0.01f);
            Assert.AreEqual((float)Math.Sqrt(2), i.FirstRayDistance, 0.01f);
        }

        [TestMethod]
        public void ParallelRaysDoNotIntersect()
        {
            Ray r1 = new Ray(new Vector2D(0, 0), new Vector2D(0, 1));
            Ray r2 = new Ray(new Vector2D(1, 0), new Vector2D(0, 1));
            var i = new RayIntersection2D(r1, r2);
            Assert.IsFalse(i.Intersects);
        }

        [TestMethod]
        public void RaysDoNotIntersectBehindTheirStartPoints()
        {
            Ray r1 = new Ray(new Vector2D(0, 0), new Vector2D(0, 1));
            Ray r2 = new Ray(new Vector2D(1, 0), new Vector2D(1, 1));
            var i = new RayIntersection2D(r1, r2);
            Assert.IsFalse(i.Intersects);            
        }
    }
}
