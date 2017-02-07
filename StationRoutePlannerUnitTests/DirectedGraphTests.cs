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
    public class DirectedGraphTests
    {
        [TestMethod()]
        public void DirectedGraphTest()
        {
            List<Node> stationNodes = new List<Node>() {    new StationNode("A"),
                                                            new StationNode("B"),
                                                            new StationNode("C"),
                                                            new StationNode("D"),
                                                            new StationNode("E") };

            DirectedGraph<Node> directedGraph = new DirectedGraph<Node>(stationNodes);

            Assert.IsTrue(directedGraph.Count() == 5);
        }

        [TestMethod()]
        public void NodeTest()
        {
            List<Node> stationNodes = new List<Node>() {    new StationNode("A"),
                                                            new StationNode("B"),
                                                            new StationNode("C"),
                                                            new StationNode("D"),
                                                            new StationNode("E") };

            DirectedGraph<Node> directedGraph = new DirectedGraph<Node>(stationNodes);

            Assert.IsTrue(directedGraph.Node("E").Reference == "E");
        }

        [TestMethod()]
        public void NodeTest1()
        {
            List<Node> stationNodes = new List<Node>() {    new StationNode("A"),
                                                            new StationNode("B"),
                                                            new StationNode("C"),
                                                            new StationNode("D"),
                                                            new StationNode("E") };

            DirectedGraph<Node> directedGraph = new DirectedGraph<Node>(stationNodes);

            Assert.IsTrue(directedGraph.Node(stationNodes[0]).Reference == "A");
        }

        [TestMethod()]
        public void AddWeightedEdgeTest()
        {
            List<Node> stationNodes = new List<Node>() {    new StationNode("A"),
                                                            new StationNode("B"),
                                                            new StationNode("C"),
                                                            new StationNode("D"),
                                                            new StationNode("E") };

            DirectedGraph<Node> directedGraph = new DirectedGraph<Node>(stationNodes);

            directedGraph.AddWeightedEdge(directedGraph.Node("A"), directedGraph.Node("B"), 5);

            Assert.IsTrue(directedGraph.Node("A").HasNeighbour(directedGraph.Node("B")));
        }

        [TestMethod()]
        public void AddNodeTest()
        {
            List<Node> stationNodes = new List<Node>() {    new StationNode("A"),
                                                            new StationNode("B"),
                                                            new StationNode("C"),
                                                            new StationNode("D"),
                                                            new StationNode("E") };

            DirectedGraph<Node> directedGraph = new DirectedGraph<Node>(stationNodes);

            Assert.AreEqual(directedGraph.TotalNodes, 5);
            directedGraph.AddNode(new StationNode("F"));
            Assert.AreEqual(directedGraph.TotalNodes, 6);
        }

        [TestMethod()]
        public void RemoveNodeTest()
        {
            List<Node> stationNodes = new List<Node>() {    new StationNode("A"),
                                                            new StationNode("B"),
                                                            new StationNode("C"),
                                                            new StationNode("D"),
                                                            new StationNode("E") };

            DirectedGraph<Node> directedGraph = new DirectedGraph<Node>(stationNodes);

            Assert.AreEqual(directedGraph.TotalNodes, 5);
            directedGraph.AddNode(new StationNode("F"));
            Assert.AreEqual(directedGraph.TotalNodes, 6);
            directedGraph.RemoveNode((directedGraph.Node("F")));
            Assert.AreEqual(directedGraph.TotalNodes, 5);
        }
    }
}