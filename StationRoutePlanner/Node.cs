using System;
using System.Collections.Generic;
using System.Collections;

namespace StationPlanner
{
	// The fundamental Node type, used by base graph functionality
	public class Node
	{
		// Position aligned neighbour nodes and their respective weighting
		private List<Node> neighbours;
		private List<int> weighting;

		// Literal identifier for the node
		string nodeReference;

		// Constructor which allows node to be named
		public Node(string reference)
		{
			nodeReference = reference;
		}

		public List<Node> Neighbours
		{
			get
			{
				if (neighbours == null)
					return neighbours = new List<Node>();
				else
					return neighbours;
			}
		}

		public List<int> Weighting
		{
			get
			{
				if (weighting == null)
					return weighting = new List<int>();
				else
					return weighting;
			}

		}

		// We can add a neighbour and its weighting to their respective containers
		public void AddNeighbour(Node neighbour, int weight)
		{
			Neighbours.Add(neighbour);
			Weighting.Add(weight);
		}

		public void RemoveNeighbour(Node neighbour)
		{
			// This method will remove both the node and it's respective weighting
			if (HasNeighbour(neighbour))
			{
				Weighting.RemoveAt(Neighbours.FindIndex(a => a == neighbour));
				Neighbours.Remove(neighbour);
			}
		}

		// Property for node reference/identification
		public string Reference
		{
			get
			{
				return nodeReference;
			}
		}

		// Helper method to allow an associated neighbours weighting to be returned
		public int GetWeightForNeighbour(Node neighbour)
		{
            // Tests
			if (HasNeighbour(neighbour))
			{
				var index = Neighbours.FindIndex(a => a == neighbour);
				return Weighting[index];
			}
			return 0;
		}

		// Helper method to determine if the current node actually has the specified neighbour 
		public bool HasNeighbour(Node neighbour)
		{
			return Neighbours.Exists(x => neighbour == x);
		}
	}
}