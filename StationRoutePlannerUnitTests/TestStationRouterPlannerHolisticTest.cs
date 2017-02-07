using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StationPlanner;
using System.Collections.Generic;
using System.Diagnostics;

namespace StationRoutePlannerUnitTests
{
    [TestClass]
    public class TestStationRouterPlannerHolisticTest
    {
        [TestMethod]
        public void TestStationPlannerHolisticFunctionality()
        {

            /* This is not so much a unit-test but rather a functional test for the application as a whole */

            Debug.WriteLine("Debug version of TestStationRouterPlanner is executing..");

            List<StationNode> stationNodes = new List<StationNode>() {  new StationNode("A"),
                                                                        new StationNode("B"),
                                                                        new StationNode("C"),
                                                                        new StationNode("D"),
                                                                        new StationNode("E") };

            Assert.AreEqual(stationNodes.Count, 5);

            StationDirectedGraph stationGraph = new StationDirectedGraph(stationNodes);

            stationGraph.AddWeightedEdge(stationGraph.Node("A"), stationGraph.Node("B"), 5);
            stationGraph.AddWeightedEdge(stationGraph.Node("A"), stationGraph.Node("E"), 7);
            stationGraph.AddWeightedEdge(stationGraph.Node("A"), stationGraph.Node("D"), 5);

            stationGraph.AddWeightedEdge(stationGraph.Node("B"), stationGraph.Node("C"), 4);

            stationGraph.AddWeightedEdge(stationGraph.Node("C"), stationGraph.Node("D"), 8);
            stationGraph.AddWeightedEdge(stationGraph.Node("C"), stationGraph.Node("E"), 2);

            stationGraph.AddWeightedEdge(stationGraph.Node("D"), stationGraph.Node("C"), 8);
            stationGraph.AddWeightedEdge(stationGraph.Node("D"), stationGraph.Node("E"), 6);

            stationGraph.AddWeightedEdge(stationGraph.Node("E"), stationGraph.Node("B"), 3);

            try
            {
                Assert.AreEqual(stationGraph.DistanceForFixedRoute("ABC"), 9);
            }
            catch (ApplicationException appEx)
            {
                Console.WriteLine($"Route A-B-C threw exception: {appEx.Message}");
            }

            Assert.AreEqual(stationGraph.DistanceForFixedRoute("AD"), 5);
            Assert.AreEqual(stationGraph.DistanceForFixedRoute("ADC"), 13);
            Assert.AreEqual(stationGraph.DistanceForFixedRoute("AEBCD"), 22);

            /* Test for no such path */
            try
            {
                int distanceAED = stationGraph.DistanceForFixedRoute("AED");
            }
            catch (ApplicationException appEx)
            {
                Console.WriteLine($"Route A-E-D threw exception: {appEx.Message}");

                Assert.AreEqual(appEx.Message, "NO SUCH PATH");
            }

            try
            {
                stationGraph.FindAllRoutes("C", "C");
                stationGraph.FindAllRoutes("A", "C");
                stationGraph.FindAllRoutes("B", "B");

                Assert.AreEqual(stationGraph.DistinctRoutesWithMaxStops("CC", 3), 2);
                Assert.AreEqual(stationGraph.DistinctRoutesWithNStops("AC", 4), 1);
                Assert.AreEqual(stationGraph.ShortestRoute("AC"), 9);
                Assert.AreEqual(stationGraph.ShortestRoute("BB"), 9);
            }
            catch (ApplicationException ex)
            {
                Console.WriteLine($"Exception {ex.Message} was thrown");
            }

            try
            {
                Assert.AreEqual(stationGraph.DifferentRoutesLessThanDistance("CC", 30), 7);

                /* Test neighbour disassociation */
                StationNode aNode = stationGraph.Node("A");
                Assert.AreEqual(aNode.Neighbours.Count, 3);
                stationGraph.Node("A").RemoveNeighbour(stationGraph.Node("D"));

                /* We should have one less neighbour for this node */
                Assert.AreEqual(aNode.Neighbours.Count, 2);

                /* Test addin a node to the graph */
                Assert.AreEqual(stationGraph.TotalNodes, 5);
                stationGraph.AddNode(new StationNode("F"));

                /* The following tests that removal of the node has invalidated known routes */
                Assert.AreEqual(stationGraph.ShortestRoute("AC"), -1);

                /* We should have one more node in the graph */
                Assert.AreEqual(stationGraph.TotalNodes, 6);

                /* Associate an existing node with the new node as a neighbour */
                Assert.AreEqual(aNode.Neighbours.Count, 2);
                stationGraph.AddWeightedEdge(stationGraph.Node("A"), stationGraph.Node("F"), 2);

                /* Check our neighbour count has incremented */
                Assert.AreEqual(aNode.Neighbours.Count, 3);

                /* Associate the new node with another node as its neighbour */
                StationNode fNode = stationGraph.Node("F");
                Assert.AreEqual(fNode.Neighbours.Count, 0);
                stationGraph.AddWeightedEdge(stationGraph.Node("F"), stationGraph.Node("B"), 2);

                /* Test to see if we have a new route from A to B that uses the new node */
                /* Ultimately, we would need to re-build all routes after adding a new node */
                stationGraph.FindAllRoutes("A", "B");
                Assert.AreEqual(stationGraph.ShortestRoute("AB"), 4);

                /* Test removal of the new node */
                stationGraph.RemoveNode(stationGraph.Node("F"));
                Assert.AreEqual(stationGraph.TotalNodes, 5);

                /* The following tests that removal of the node has invalidated known routes */
                Assert.AreEqual(stationGraph.ShortestRoute("AB"), -1);

                /* We need to re-build our routes as adding a new node invalidated the known routes */
                stationGraph.FindAllRoutes("C", "C");
                stationGraph.FindAllRoutes("A", "C");
                stationGraph.FindAllRoutes("B", "B");
                stationGraph.FindAllRoutes("A", "C");
                stationGraph.FindAllRoutes("A", "B");

                /* We should have our original path from A to B now the F node has been removed */
                Assert.AreEqual(stationGraph.ShortestRoute("AB"), 5);

                /* Re-test some tests for consistency and to ensure graph integrity preserved */
                Assert.AreEqual(stationGraph.DistanceForFixedRoute("AD"), 5);
                Assert.AreEqual(stationGraph.DistanceForFixedRoute("ADC"), 13);
                Assert.AreEqual(stationGraph.DistanceForFixedRoute("AEBCD"), 22);

                Assert.AreEqual(stationGraph.DifferentRoutesLessThanDistance("CC", 30), 7);

                try
                {
                    int distanceAED = stationGraph.DistanceForFixedRoute("AED");
                }
                catch (ApplicationException appEx)
                {
                    Console.WriteLine($"Route A-E-D threw exception: {appEx.Message}");

                    Assert.AreEqual(appEx.Message, "NO SUCH PATH");
                }

            }
            catch (ApplicationException ex)
            {
                Console.WriteLine($"Exception {ex.Message} was thrown");
            }
        }
    }
}
