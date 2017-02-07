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
    public class StationDirectedGraphTests
    {
        [TestMethod()]
        public void StationDirectedGraphTest()
        {
            List<StationNode> stationNodes = new List<StationNode>() {  new StationNode("A"),
                                                                        new StationNode("B"),
                                                                        new StationNode("C"),
                                                                        new StationNode("D"),
                                                                        new StationNode("E") };

            StationDirectedGraph stationDirectedGraph = new StationDirectedGraph(stationNodes);

            Assert.IsTrue(stationDirectedGraph.Count() == 5);
        }

        [TestMethod()]
        public void AddNodeTest()
        {
            List<StationNode> stationNodes = new List<StationNode>() {  new StationNode("A"),
                                                                        new StationNode("B"),
                                                                        new StationNode("C"),
                                                                        new StationNode("D"),
                                                                        new StationNode("E") };

            StationDirectedGraph stationDirectedGraph = new StationDirectedGraph(stationNodes);

            Assert.IsTrue(stationDirectedGraph.Count() == 5);

            stationDirectedGraph.AddNode(new StationNode("F"));

            Assert.IsTrue(stationDirectedGraph.Count() == 6);
            Assert.IsTrue(stationDirectedGraph.Node(stationDirectedGraph.Node("F")).Reference == "F");
        }

        [TestMethod()]
        public void RemoveNodeTest()
        {
            List<StationNode> stationNodes = new List<StationNode>() {  new StationNode("A"),
                                                                        new StationNode("B"),
                                                                        new StationNode("C"),
                                                                        new StationNode("D"),
                                                                        new StationNode("E") };

            StationDirectedGraph stationDirectedGraph = new StationDirectedGraph(stationNodes);

            Assert.IsTrue(stationDirectedGraph.Count() == 5);

            stationDirectedGraph.AddNode(new StationNode("F"));        
            Assert.IsTrue(stationDirectedGraph.Count() == 6);

            stationDirectedGraph.RemoveNode(stationDirectedGraph.Node("F"));
            Assert.IsTrue(stationDirectedGraph.Count() == 5);
        }

        [TestMethod()]
        public void DistanceForFixedRouteTest()
        {
            List<StationNode> stationNodes = new List<StationNode>() {  new StationNode("A"),
                                                                        new StationNode("B"),
                                                                        new StationNode("C"),
                                                                        new StationNode("D"),
                                                                        new StationNode("E") };

            StationDirectedGraph stationDirectedGraph = new StationDirectedGraph(stationNodes);

            stationDirectedGraph.AddWeightedEdge(stationDirectedGraph.Node("A"), stationDirectedGraph.Node("B"), 5);
            stationDirectedGraph.AddWeightedEdge(stationDirectedGraph.Node("A"), stationDirectedGraph.Node("E"), 7);
            stationDirectedGraph.AddWeightedEdge(stationDirectedGraph.Node("A"), stationDirectedGraph.Node("D"), 5);

            stationDirectedGraph.AddWeightedEdge(stationDirectedGraph.Node("B"), stationDirectedGraph.Node("C"), 4);

            stationDirectedGraph.AddWeightedEdge(stationDirectedGraph.Node("C"), stationDirectedGraph.Node("D"), 8);
            stationDirectedGraph.AddWeightedEdge(stationDirectedGraph.Node("C"), stationDirectedGraph.Node("E"), 2);

            stationDirectedGraph.AddWeightedEdge(stationDirectedGraph.Node("D"), stationDirectedGraph.Node("C"), 8);
            stationDirectedGraph.AddWeightedEdge(stationDirectedGraph.Node("D"), stationDirectedGraph.Node("E"), 6);

            stationDirectedGraph.AddWeightedEdge(stationDirectedGraph.Node("E"), stationDirectedGraph.Node("B"), 3);

            stationDirectedGraph.FindAllRoutes("C", "C");
            stationDirectedGraph.FindAllRoutes("A", "C");
            stationDirectedGraph.FindAllRoutes("B", "B");

            try
            {
                int distanceABC = stationDirectedGraph.DistanceForFixedRoute("ABC");
                Console.WriteLine($"Output #1: {distanceABC}");
            }
            catch (ApplicationException appEx)
            {
                Console.WriteLine($"Route A-B-C threw exception: {appEx.Message}");
            }

            Assert.AreEqual(stationDirectedGraph.DistanceForFixedRoute("AD"), 5);
            Assert.AreEqual(stationDirectedGraph.DistanceForFixedRoute("ADC"), 13);
            Assert.AreEqual(stationDirectedGraph.DistanceForFixedRoute("AEBCD"), 22);

            /* Test for no such path */
            try
            {
                int distanceAED = stationDirectedGraph.DistanceForFixedRoute("AED");
            }
            catch (ApplicationException appEx)
            {
                Console.WriteLine($"Route A-E-D threw exception: {appEx.Message}");

                Assert.AreEqual(appEx.Message, "NO SUCH PATH");
            }
        }

        [TestMethod()]
        public void DistinctRoutesWithMaxStopsTest()
        {
            List<StationNode> stationNodes = new List<StationNode>() {  new StationNode("A"),
                                                                        new StationNode("B"),
                                                                        new StationNode("C"),
                                                                        new StationNode("D"),
                                                                        new StationNode("E") };

            StationDirectedGraph stationDirectedGraph = new StationDirectedGraph(stationNodes);

            stationDirectedGraph.AddWeightedEdge(stationDirectedGraph.Node("A"), stationDirectedGraph.Node("B"), 5);
            stationDirectedGraph.AddWeightedEdge(stationDirectedGraph.Node("A"), stationDirectedGraph.Node("E"), 7);
            stationDirectedGraph.AddWeightedEdge(stationDirectedGraph.Node("A"), stationDirectedGraph.Node("D"), 5);

            stationDirectedGraph.AddWeightedEdge(stationDirectedGraph.Node("B"), stationDirectedGraph.Node("C"), 4);

            stationDirectedGraph.AddWeightedEdge(stationDirectedGraph.Node("C"), stationDirectedGraph.Node("D"), 8);
            stationDirectedGraph.AddWeightedEdge(stationDirectedGraph.Node("C"), stationDirectedGraph.Node("E"), 2);

            stationDirectedGraph.AddWeightedEdge(stationDirectedGraph.Node("D"), stationDirectedGraph.Node("C"), 8);
            stationDirectedGraph.AddWeightedEdge(stationDirectedGraph.Node("D"), stationDirectedGraph.Node("E"), 6);

            stationDirectedGraph.AddWeightedEdge(stationDirectedGraph.Node("E"), stationDirectedGraph.Node("B"), 3);

            stationDirectedGraph.FindAllRoutes("C", "C");
            stationDirectedGraph.FindAllRoutes("A", "C");
            stationDirectedGraph.FindAllRoutes("B", "B");

            Assert.AreEqual(stationDirectedGraph.DistinctRoutesWithMaxStops("CC", 3), 2);
        }

        [TestMethod()]
        public void DistinctRoutesWithNStopsTest()
        {
            List<StationNode> stationNodes = new List<StationNode>() {  new StationNode("A"),
                                                                        new StationNode("B"),
                                                                        new StationNode("C"),
                                                                        new StationNode("D"),
                                                                        new StationNode("E") };

            StationDirectedGraph stationDirectedGraph = new StationDirectedGraph(stationNodes);

            stationDirectedGraph.AddWeightedEdge(stationDirectedGraph.Node("A"), stationDirectedGraph.Node("B"), 5);
            stationDirectedGraph.AddWeightedEdge(stationDirectedGraph.Node("A"), stationDirectedGraph.Node("E"), 7);
            stationDirectedGraph.AddWeightedEdge(stationDirectedGraph.Node("A"), stationDirectedGraph.Node("D"), 5);

            stationDirectedGraph.AddWeightedEdge(stationDirectedGraph.Node("B"), stationDirectedGraph.Node("C"), 4);

            stationDirectedGraph.AddWeightedEdge(stationDirectedGraph.Node("C"), stationDirectedGraph.Node("D"), 8);
            stationDirectedGraph.AddWeightedEdge(stationDirectedGraph.Node("C"), stationDirectedGraph.Node("E"), 2);

            stationDirectedGraph.AddWeightedEdge(stationDirectedGraph.Node("D"), stationDirectedGraph.Node("C"), 8);
            stationDirectedGraph.AddWeightedEdge(stationDirectedGraph.Node("D"), stationDirectedGraph.Node("E"), 6);

            stationDirectedGraph.AddWeightedEdge(stationDirectedGraph.Node("E"), stationDirectedGraph.Node("B"), 3);

            stationDirectedGraph.FindAllRoutes("C", "C");
            stationDirectedGraph.FindAllRoutes("A", "C");
            stationDirectedGraph.FindAllRoutes("B", "B");

            Assert.AreEqual(stationDirectedGraph.DistinctRoutesWithNStops("AC", 4), 1);
        }

        [TestMethod()]
        public void ShortestRouteTest()
        {
            List<StationNode> stationNodes = new List<StationNode>() {  new StationNode("A"),
                                                                        new StationNode("B"),
                                                                        new StationNode("C"),
                                                                        new StationNode("D"),
                                                                        new StationNode("E") };

            StationDirectedGraph stationDirectedGraph = new StationDirectedGraph(stationNodes);

            stationDirectedGraph.AddWeightedEdge(stationDirectedGraph.Node("A"), stationDirectedGraph.Node("B"), 5);
            stationDirectedGraph.AddWeightedEdge(stationDirectedGraph.Node("A"), stationDirectedGraph.Node("E"), 7);
            stationDirectedGraph.AddWeightedEdge(stationDirectedGraph.Node("A"), stationDirectedGraph.Node("D"), 5);

            stationDirectedGraph.AddWeightedEdge(stationDirectedGraph.Node("B"), stationDirectedGraph.Node("C"), 4);

            stationDirectedGraph.AddWeightedEdge(stationDirectedGraph.Node("C"), stationDirectedGraph.Node("D"), 8);
            stationDirectedGraph.AddWeightedEdge(stationDirectedGraph.Node("C"), stationDirectedGraph.Node("E"), 2);

            stationDirectedGraph.AddWeightedEdge(stationDirectedGraph.Node("D"), stationDirectedGraph.Node("C"), 8);
            stationDirectedGraph.AddWeightedEdge(stationDirectedGraph.Node("D"), stationDirectedGraph.Node("E"), 6);

            stationDirectedGraph.AddWeightedEdge(stationDirectedGraph.Node("E"), stationDirectedGraph.Node("B"), 3);

            stationDirectedGraph.FindAllRoutes("C", "C");
            stationDirectedGraph.FindAllRoutes("A", "C");
            stationDirectedGraph.FindAllRoutes("B", "B");

            Assert.AreEqual(stationDirectedGraph.ShortestRoute("AC"), 9);
            Assert.AreEqual(stationDirectedGraph.ShortestRoute("BB"), 9);

            stationDirectedGraph.RemoveNode(stationDirectedGraph.Node("D"));

            /* The following tests that removal of the node has invalidated known routes */
            Assert.AreEqual(stationDirectedGraph.ShortestRoute("AC"), -1);
        }

        [TestMethod()]
        public void DifferentRoutesLessThanDistanceTest()
        {
            List<StationNode> stationNodes = new List<StationNode>() {  new StationNode("A"),
                                                                        new StationNode("B"),
                                                                        new StationNode("C"),
                                                                        new StationNode("D"),
                                                                        new StationNode("E") };

            StationDirectedGraph stationDirectedGraph = new StationDirectedGraph(stationNodes);

            stationDirectedGraph.AddWeightedEdge(stationDirectedGraph.Node("A"), stationDirectedGraph.Node("B"), 5);
            stationDirectedGraph.AddWeightedEdge(stationDirectedGraph.Node("A"), stationDirectedGraph.Node("E"), 7);
            stationDirectedGraph.AddWeightedEdge(stationDirectedGraph.Node("A"), stationDirectedGraph.Node("D"), 5);

            stationDirectedGraph.AddWeightedEdge(stationDirectedGraph.Node("B"), stationDirectedGraph.Node("C"), 4);

            stationDirectedGraph.AddWeightedEdge(stationDirectedGraph.Node("C"), stationDirectedGraph.Node("D"), 8);
            stationDirectedGraph.AddWeightedEdge(stationDirectedGraph.Node("C"), stationDirectedGraph.Node("E"), 2);

            stationDirectedGraph.AddWeightedEdge(stationDirectedGraph.Node("D"), stationDirectedGraph.Node("C"), 8);
            stationDirectedGraph.AddWeightedEdge(stationDirectedGraph.Node("D"), stationDirectedGraph.Node("E"), 6);

            stationDirectedGraph.AddWeightedEdge(stationDirectedGraph.Node("E"), stationDirectedGraph.Node("B"), 3);

            stationDirectedGraph.FindAllRoutes("C", "C");
            stationDirectedGraph.FindAllRoutes("A", "C");
            stationDirectedGraph.FindAllRoutes("B", "B");

            Assert.AreEqual(stationDirectedGraph.DifferentRoutesLessThanDistance("CC", 30), 7);
        }

        [TestMethod()]
        public void FindAllRoutesTest()
        {
            List<StationNode> stationNodes = new List<StationNode>() {  new StationNode("A"),
                                                                        new StationNode("B"),
                                                                        new StationNode("C"),
                                                                        new StationNode("D"),
                                                                        new StationNode("E") };

            StationDirectedGraph stationDirectedGraph = new StationDirectedGraph(stationNodes);

            stationDirectedGraph.AddWeightedEdge(stationDirectedGraph.Node("A"), stationDirectedGraph.Node("B"), 5);
            stationDirectedGraph.AddWeightedEdge(stationDirectedGraph.Node("A"), stationDirectedGraph.Node("E"), 7);
            stationDirectedGraph.AddWeightedEdge(stationDirectedGraph.Node("A"), stationDirectedGraph.Node("D"), 5);

            stationDirectedGraph.AddWeightedEdge(stationDirectedGraph.Node("B"), stationDirectedGraph.Node("C"), 4);

            stationDirectedGraph.AddWeightedEdge(stationDirectedGraph.Node("C"), stationDirectedGraph.Node("D"), 8);
            stationDirectedGraph.AddWeightedEdge(stationDirectedGraph.Node("C"), stationDirectedGraph.Node("E"), 2);

            stationDirectedGraph.AddWeightedEdge(stationDirectedGraph.Node("D"), stationDirectedGraph.Node("C"), 8);
            stationDirectedGraph.AddWeightedEdge(stationDirectedGraph.Node("D"), stationDirectedGraph.Node("E"), 6);

            stationDirectedGraph.AddWeightedEdge(stationDirectedGraph.Node("E"), stationDirectedGraph.Node("B"), 3);

            /* Routes not yet caclulated */
            Assert.AreEqual(stationDirectedGraph.ShortestRoute("AC"), -1);
            Assert.AreEqual(stationDirectedGraph.ShortestRoute("BB"), -1);

            stationDirectedGraph.FindAllRoutes("C", "C");
            stationDirectedGraph.FindAllRoutes("A", "C");
            stationDirectedGraph.FindAllRoutes("B", "B");

            Assert.AreEqual(stationDirectedGraph.ShortestRoute("AC"), 9);
            Assert.AreEqual(stationDirectedGraph.ShortestRoute("BB"), 9);
        }
    }
}