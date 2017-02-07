using System;
using System.Collections.Generic;
using System.Linq;

/* Extending the DirectedGraph class allows for a specialisation of the type to derive
 * a concrete purpose/application */

namespace StationPlanner
{
	public class StationDirectedGraph : DirectedGraph<StationNode>
	{
		// List of known paths in a handy encapsulated package with helpers
		List<StationRoute> discoveredRoutes = new List<StationRoute>();

        public List<StationRoute> DiscoveredRoutes
        {
            get
            {
                return discoveredRoutes;
            }
        }


		public StationDirectedGraph(List<StationNode> nodes) : base(nodes)
		{
		}

		override public void AddNode(StationNode node)
		{
			// If we add a node to the graph then we must invalidate the known routes
			base.AddNode(node);
			discoveredRoutes.Clear();
		}

		override public void RemoveNode(StationNode node)
		{
			// If we remove a node to the graph then we must invalidate the known routes
			base.RemoveNode(node);

			// Remove reference of deleted node (as neighbour) from all other nodes
			foreach (StationNode stationNode in nodes)
			{
				if (stationNode.HasNeighbour(node))
				{
					stationNode.RemoveNeighbour(node);
				}
			}

			discoveredRoutes.Clear();
		}

		// Publicly accessible function which simply takes a string sequence indicating a path
		public int DistanceForFixedRoute(string fixedRoute)
		{
			// Internally, we deal wth node objects to represent each node in the path
			var nodes = new List<StationNode>();

			foreach (char nodeName in fixedRoute)
			{
				nodes.Add(Node(nodeName.ToString()));
			}

			return DistanceForFixedRoute(nodes);
		}

		// Recursive function to determine distance for a sequence of nodes representing a station network
		private int DistanceForFixedRoute(List<StationNode> route)
		{
			var rootNode = route?[0];
			var neighbhours = route?.GetRange(1, route.Count - 1);
			var distance = 0;

			// If there is only one node in our list then we must have consumed the entire list
			if (route?.Count == 1)
			{
				return 0;
			}
			else if (rootNode != null && rootNode.HasNeighbour(route[1]))
			{
				// Aggregation of distance values from successive direct/indirect neighbours
				distance += rootNode.GetWeightForNeighbour(route[1]) + DistanceForFixedRoute(neighbhours);
			}
			else
			{
				/* There is no neighbour available and  we are not at the end of the path of nodes
				 * so we assume destination is not accessible */
				throw new ApplicationException("NO SUCH PATH");
			}

			return distance;
		}

		// Helper method to find total number of distinct routes along a path narrowed by a max-stops filter
		public int DistinctRoutesWithMaxStops(string routeID, int stops)
		{
			return (discoveredRoutes.FindAll(x => x.StationRouteId.Equals(routeID) && x.NumberOfStops <= stops)).Count;
		}

		// A method to find total number of distinct stops along a path with 'n' stops exactly
		public int DistinctRoutesWithNStops(string routeID, int stops)
		{
			return (discoveredRoutes.FindAll(x => x.StationRouteId.Equals(routeID) && x.NumberOfStops == stops)).Count;
		}

		public int ShortestRoute(string routeID)
		{
			// Protected against exceptions being thrown for empty container
			if (discoveredRoutes.Count > 0)
			{
				// Return the minimum known route-distance for the respective route
				return (discoveredRoutes.FindAll(x => x.StationRouteId.Equals(routeID)).Min(x => x.RouteDistance));
			}
			else
			{
				// Returning a negative value will indicate no shortest route found
				return -1;
			}

		}

		// Helper to ensure we do not put duplicate routes into the aggregate known paths container
		private bool DiscoveredRoutesAlreadyExists(StationRoute route)
		{
			return discoveredRoutes.Exists(x => x.StationRouteSequence == route.StationRouteSequence);
		}

		// Helper method to find total number of different routes between two points less than a specified distance
		public int DifferentRoutesLessThanDistance(string routeID, int distance)
		{
			// We use our 'known' routes for the start/destination as the source of subsequent combined routes
			var knownStationRoutes = discoveredRoutes.FindAll(x => x.StationRouteId.Equals(routeID) && x.RouteDistance < distance);

			// The current tally equates to the number of 'known' routes
			var differentRoutes = knownStationRoutes.Count;
			var calculatedDistance = 0;

			// Iterate through the known routes and compare them with each other and the oher 'known' routes
			foreach (StationRoute stationRoute in knownStationRoutes)
			{
				// If our current route distance is greater than the specified max distance then move to next
				if ((calculatedDistance = stationRoute.RouteDistance) >= distance)
				{
					continue;
				}

				// Iterate through the station nodes again whilst comparing them to the upper level nodes
				foreach (StationRoute innerStationRoute in knownStationRoutes)
				{
					// Does our outer-nodes distance plus the inner-nodes distance meet the criteria? If not, move to next
					if (calculatedDistance + innerStationRoute.RouteDistance >= distance)
					{
						continue;
					}

					/* This logic determines how many times the inner-path can be combined with the outer-path to
					   provide an aggregate route-count (for example: CEBC and then CEBCEBC and then CEBCEBCEBC - 3 routes) */
					var totalRouteExtensions = (((distance - 1) - stationRoute.RouteDistance) / innerStationRoute.RouteDistance);
					if (totalRouteExtensions >= 1)
					{
						// Tally the number of routes accordingly
						differentRoutes += totalRouteExtensions;
					}
				}
			}

			return differentRoutes;
		}

		// This method will take a start and end node and determine all known paths between the two, in the network
		public void FindAllRoutes(string start, string end)
		{
			// The parameters are simplified to strings for easy-use, but internally they use StationNode objects
			StationNode startStation = Node(start);
			StationNode endStation = Node(end);

			// Handle invalid start/destination
			if (startStation == null)
			{
				throw new ApplicationException($"Starting Station {start} was not found");
			}

			if (endStation == null)
			{
				throw new ApplicationException($"Ending Station {end} was not found");
			}

			// Call the recursive depth-first (DFS) implementation to determine paths in the graph (ultimately)
			var discoveredPaths = new List<List<StationNode>>();
			DepthFirst(ref discoveredPaths, startStation, endStation);

			// Add the routes returned to our container for subsequent reference
			AddRoutesToKnown(discoveredPaths);
		}

		private void AddRoutesToKnown(List<List<StationNode>> routes)
		{
			foreach (List<StationNode> route in routes)
			{
				StationRoute stationRoute = new StationRoute(route);

				// Don't allow insertion of (allbeit) unique StationRoute objects which duplicate path sequences
				if (!DiscoveredRoutesAlreadyExists(stationRoute))
				{
					discoveredRoutes.Add(stationRoute);
				}
			}
		}

		// This calls into the base-class and attempts to hide the architectural nature of the container from the caller
		private void DepthFirst(ref List<List<StationNode>> discoveredPaths, StationNode start, StationNode end)
		{
			LinkedList<StationNode> nodes = new LinkedList<StationNode>();
			DepthFirst(ref nodes, ref discoveredPaths, start, end);
		}
	}
}
