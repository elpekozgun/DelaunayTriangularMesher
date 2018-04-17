using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MathUtils;
using MathEntities;
using System.Collections.Generic;

namespace UnitTester
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void doesContain()
        {
            var poly = new List<XYPT>();

            poly.Add(new XYPT(0, 0));
            poly.Add(new XYPT(5, 0));
            poly.Add(new XYPT(5, 5));
            poly.Add(new XYPT(0, 5));

            var pt = new XYPT(2,2);

            Assert.AreEqual(true, Utilities2D.Contains(pt, poly));
        }
    }
}
