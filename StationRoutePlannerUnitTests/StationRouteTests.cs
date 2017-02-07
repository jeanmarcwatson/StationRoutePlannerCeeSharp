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
    public class StationRouteTests
    {
        [TestMethod()]
        public void StationRouteTest()
        {
            List<StationNode> stationNodes = new List<StationNode>() {  new StationNode("A"),
                                                                        new StationNode("B"),
                                                                        new StationNode("C") };

            StationDirectedGraph stationDirectedGraph = new StationDirectedGraph(stationNodes);

            stationDirectedGraph.AddWeightedEdge(stationDirectedGraph.Node("A"), stationDirectedGraph.Node("B"), 5);          
            stationDirectedGraph.AddWeightedEdge(stationDirectedGraph.Node("B"), stationDirectedGraph.Node("C"), 4);
            stationDirectedGraph.AddWeightedEdge(stationDirectedGraph.Node("C"), stationDirectedGraph.Node("A"), 8);                        

            stationDirectedGraph.FindAllRoutes("A","A");            

            Assert.IsTrue(stationDirectedGraph.DiscoveredRoutes[0].StationRouteSequence == "ABCA");
            Assert.IsTrue(stationDirectedGraph.DiscoveredRoutes[0].StationRouteId == "AA");            
            Assert.IsTrue(stationDirectedGraph.DiscoveredRoutes[0].RouteDistance == 17);
            Assert.IsTrue(stationDirectedGraph.DiscoveredRoutes[0].NumberOfStops == 3);
        }
    }
}