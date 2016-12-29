using System;
using System.Collections;
using System.Collections.Generic;


namespace StationPlanner
{

	// This is the base class for directed graphs (It is iterate-able)
	public class DirectedGraph<T> : IEnumerable<T> where T : Node
	{
		protected List<T> nodes;
		protected bool routesInvalidated = false;

		// Provides a simple tally of nodes in the graph
		public int TotalNodes
		{
			get
			{
				return nodes.Count;
			}

		}

		public DirectedGraph(List<T> nodes)
		{
			this.nodes = nodes;
		}

		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return nodes.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return nodes.GetEnumerator();
		}

		public T Node(string value)
		{
			// Find a node based upon its literal name, inside our container of nodes
			return nodes?.Find(x => value == x.Reference);
		}

		public T Node(T node)
		{
			// Find a node based by object reference, inside our container of nodes
			return nodes?.Find(x => node == x);
		}

		public void AddWeightedEdge(T from, T to, int weighting)
		{
			// Adding a neighbour and it's associated weight/cost
			from.AddNeighbour(to, weighting);
		}

		virtual public void AddNode(T node)
		{
			// Simply add a type T node to the graph
			nodes.Add(node);
		}

		virtual public void RemoveNode(T node)
		{
			// Remove a type T node from the graph
			nodes.Remove(node);
		}

		protected void DepthFirst(ref LinkedList<T> inspected, ref List<List<T>> discoveredPaths, T start, T end)
		{
			// Flag to determine if not yet recursed
			bool startOfRecursion = (inspected.Count == 0);

			LinkedListNode<T> inspected_node = null;
			List<T> neighbours = null;

			if (startOfRecursion)
			{
				// Do this at start of recursion so we can obtain the neighbour nodes of the
				// start node.
				neighbours = (start.Neighbours.ConvertAll(x => (T)x));
			}
			else
			{
				// All subordinate nodes on the searched path are sourced from the list of "discovered" nodes
				// being inspected (subordinate to the start node)
				inspected_node = inspected.Last;
				neighbours = (inspected_node.Value.Neighbours.ConvertAll(x => (T)x));
			}

			// Check each neighbour of this current node
			foreach (T neighbour_node in neighbours)
			{
				// If we've seen it before then move on
				if (inspected.Contains(neighbour_node))
				{
					continue;
				}

				// Handle the case where neighbour node being inspected is actually our target/destination node
				if (neighbour_node == end)
				{
					// Put it at the back of linked-list for future inspection
					inspected.AddLast(neighbour_node);

					var path = new List<T>();

					// We removed the start node after obtaining it's neighbours
					// but we need to insert it at start of discovered path again once
					// the inspected nodes for this path discover the end-node..
					path.Add(start);

					// Build up our known path from source to target node
					foreach (Node nde in inspected)
					{
						path.Add((nde as T));
					}

					inspected.RemoveLast();

					// Add discovered path to list of known paths
					discoveredPaths.Add(path);
					break;
				}
			}

			// Don't stop if we've reached the end
			foreach (T neighbour_node in neighbours)
			{
				if (inspected.Contains(neighbour_node) || neighbour_node == end)
				{
					continue;
				}

				// Add the last discovered neighbour node to the list of candidates and recurse
				// into this function using the currently maintained list of nodes yet to inspect.
				inspected.AddLast(neighbour_node);
				DepthFirst(ref inspected, ref discoveredPaths, start, end);
				inspected.RemoveLast();
			}
		}
	}
}