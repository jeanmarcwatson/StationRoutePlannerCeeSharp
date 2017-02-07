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
    public class NodeTests
    {
        [TestMethod()]
        public void NodeTest()
        {
            Node nodea = new Node("A");
            Node nodeb = new Node("B");

            Assert.IsTrue(nodea.Reference == "A");
            Assert.IsTrue(nodea.HasNeighbour(nodeb) == false);
        }

        [TestMethod()]
        public void AddNeighbourTest()
        {
            Node nodea = new Node("A");
            Node nodeb = new Node("B");

            Assert.IsTrue(nodea.Reference == "A");
            Assert.IsTrue(nodea.HasNeighbour(nodeb) == false);

            nodea.AddNeighbour(nodeb, 5);

            Assert.IsTrue(nodea.Reference == "A");
            Assert.IsTrue(nodea.HasNeighbour(nodeb) == true);
        }

        [TestMethod()]
        public void RemoveNeighbourTest()
        {
            Node nodea = new Node("A");
            Node nodeb = new Node("B");

            Assert.IsTrue(nodea.Reference == "A");
            Assert.IsTrue(nodea.HasNeighbour(nodeb) == false);

            nodea.AddNeighbour(nodeb, 5);

            Assert.IsTrue(nodea.Reference == "A");
            Assert.IsTrue(nodea.HasNeighbour(nodeb) == true);

            nodea.RemoveNeighbour(nodeb);

            Assert.IsTrue(nodea.HasNeighbour(nodeb) == false);
        }

        [TestMethod()]
        public void GetWeightForNeighbourTest()
        {
            Node nodea = new Node("A");
            Node nodeb = new Node("B");

            Assert.IsTrue(nodea.Reference == "A");
            Assert.IsTrue(nodea.HasNeighbour(nodeb) == false);

            nodea.AddNeighbour(nodeb, 5);

            Assert.IsTrue(nodea.Reference == "A");
            Assert.IsTrue(nodea.HasNeighbour(nodeb) == true);

            Assert.IsTrue(nodea.GetWeightForNeighbour(nodeb) == 5);
        }

        [TestMethod()]
        public void HasNeighbourTest()
        {
            Node nodea = new Node("A");
            Node nodeb = new Node("B");

            Assert.IsTrue(nodea.Reference == "A");
            Assert.IsTrue(nodea.HasNeighbour(nodeb) == false);

            nodea.AddNeighbour(nodeb, 5);

            Assert.IsTrue(nodea.Reference == "A");
            Assert.IsTrue(nodea.HasNeighbour(nodeb) == true);
        }
    }
}