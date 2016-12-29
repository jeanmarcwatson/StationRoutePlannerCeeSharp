using System;
using System.Collections.Generic;

namespace StationPlanner
{

	// Class to encapsulate routes with associated helpers
	public class StationRoute
	{
		List<StationNode> route;

		string stationRouteId;
		string stationRouteSequence;
		int routeDistance;
		int numberOfStops;

		public StationRoute(List<StationNode> knownRoute)
		{
			route = knownRoute;

			stationRouteId = "";
			stationRouteSequence = "";
			routeDistance = 0;
			numberOfStops = 0;
		}

		public string StationRouteSequence
		{
			get
			{
				if (stationRouteSequence.Length == 0)
				{
					foreach (Node node in route)
					{
						stationRouteSequence += node.Reference;
					}
				}

				return stationRouteSequence;
			}
		}

		public string StationRouteId
		{
			get
			{
				if (stationRouteId.Length == 0)
				{
					stationRouteId = StationRouteSequence.Substring(0, 1) +
														StationRouteSequence.Substring(StationRouteSequence.Length - 1, 1);
				}

				return stationRouteId;
			}
		}

		public List<StationNode> Route
		{
			get
			{
				return route;
			}
		}

		public int RouteDistance
		{
			get
			{
				if (routeDistance == 0)
				{
					var index = 1;

					foreach (StationNode node in Route)
					{
						if (index < (Route.Count))
						{
							routeDistance += node.GetWeightForNeighbour(route[index++]);
						}
					}
				}

				return routeDistance;
			}
		}

		public int NumberOfStops
		{
			get
			{
				if (numberOfStops == 0)
				{
					numberOfStops = Route.Count - 1;
				}

				return numberOfStops;
			}
		}
	}
}

