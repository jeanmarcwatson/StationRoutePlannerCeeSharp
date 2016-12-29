using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StationPlanner;

namespace StationRoutePlanner
{
	class Program
	{
		static void Main(string[] args)
		{
			List<StationNode> stationNodes = new List<StationNode>() {  new StationNode("A"),
																		new StationNode("B"),
																		new StationNode("C"),
																		new StationNode("D"),
																		new StationNode("E") };

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
				int distanceABC = stationGraph.DistanceForFixedRoute("ABC");
				Console.WriteLine($"Output #1: {distanceABC}");
			}
			catch (ApplicationException appEx)
			{
				Console.WriteLine($"Route A-B-C threw exception: {appEx.Message}");
			}

			int distanceAD = stationGraph.DistanceForFixedRoute("AD");
			Console.WriteLine($"Output #2: {distanceAD}");

			int distanceADC = stationGraph.DistanceForFixedRoute("ADC");
			Console.WriteLine($"Output #3: {distanceADC}");

			int distanceAEBCD = stationGraph.DistanceForFixedRoute("AEBCD");
			Console.WriteLine($"Output #4: {distanceAEBCD}");

			try
			{
				int distanceAED = stationGraph.DistanceForFixedRoute("AED");
			}
			catch (ApplicationException appEx)
			{
				Console.WriteLine($"Route A-E-D threw exception: {appEx.Message}");
			}

			List<List<StationNode>> paths = new List<List<StationNode>>();

			try
			{
				stationGraph.FindAllRoutes("C", "C");
				stationGraph.FindAllRoutes("A", "C");
				stationGraph.FindAllRoutes("B", "B");

				int ccMaxStops3 = stationGraph.DistinctRoutesWithMaxStops("CC", 3);
				Console.WriteLine($"Output #6: {ccMaxStops3}");

				int acExactStops4 = stationGraph.DistinctRoutesWithNStops("AC", 4);
				Console.WriteLine($"Output #7: {acExactStops4}");

				int acShortestDistance = stationGraph.ShortestRoute("AC");
				Console.WriteLine($"Output #8: {acShortestDistance}");

				int bbShortestDistance = stationGraph.ShortestRoute("BB");
				Console.WriteLine($"Output #9: {bbShortestDistance}");
			}
			catch (ApplicationException ex)
			{
				Console.WriteLine($"Exception {ex.Message} was thrown");
			}

			int ccDifferentRoutesLess30 = stationGraph.DifferentRoutesLessThanDistance("CC", 30);
			Console.WriteLine($"Output #10: {ccDifferentRoutesLess30}");

		}
	}
}
