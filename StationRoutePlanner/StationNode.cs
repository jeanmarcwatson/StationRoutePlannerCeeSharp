using System;

namespace StationPlanner
{
	/* This serves to demonstrate that Nodea can be extended to take on adapted semantics
	 * and functinality whilst retaining their Node characteristics */
	public class StationNode : Node
	{
		public StationNode(string station) : base(station)
		{
		}

		/* Just demonstrating that an extension of node is possible
		 * with only the underlying Node class being used for graph traversal.
		 * The extended class functions independently of its underlying Node parent. */
		public int StubMethod1()
		{
			return 1;
		}

		// Same again. Simply a method implementation seperate of the base-class Node implementation
		public void StubMethod2<T>(T obj)
		{
			var a = obj;
		}
	}
}
