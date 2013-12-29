using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raycaster;
using System.Drawing;

namespace RaycasterTest
{
    [TestClass]
    public class Vector2DTest
    {
        [TestMethod]
        public void SimpleVectorRejectionWorksAsItShould()
        {
            Vector2D a = new Vector2D(4, 3);
            Vector2D b = new Vector2D(4, 0);
            var r = Vector2D.Rejection(a, b);
            Assert.AreEqual(9f, r.LengthSquared, 0.01f);
        }

        [TestMethod]
        public void SimpleVectorProjectionWorksAsItShould()
        {
            Vector2D a = new Vector2D(4, 3);
            Vector2D b = new Vector2D(1, 0);
            var r = Vector2D.Projection(a, b);
            Assert.AreEqual(16f, r.LengthSquared, 0.01f);
        }
       
    }
}
