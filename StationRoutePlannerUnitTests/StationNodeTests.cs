using Microsoft.VisualStudio.TestTools.UnitTesting;
using StationPlanner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StationPlanner.Tests
{
    [TestClass()]
    public class StationNodeTests
    {
        [TestMethod()]
        public void StationNodeTest()
        {
            StationNode stationNodea = new StationNode("A");
            StationNode stationNodeb = new StationNode("B");

            Assert.IsTrue(stationNodea.Reference == "A");
            Assert.IsTrue(stationNodeb.Reference == "B");
        }
    }
}