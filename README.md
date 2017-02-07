# StationRoutePlannerCeeSharp
A simple C# application which can determine optimised paths between nodes in a logical network using recursive DFS in a directed graph

PROBLEM:

A train services a number of suburbs.  All of the tracks are ‘one-way’.  That is a connection from Stop A to Stop B does not imply a 
viable connection from Stop B to Stop A.  If both of these connections are available, they are distinct and not necessarily an 
equivalent distance.

The purpose of your solution is to provide information to customers about the available routes. 
Specifically computing distances along a route, the number of different routes between two stops, and
the shortest route between two stops.

INPUT:

A directed graph where a node represents a stop and the edge represents a connection between two stops.  
The weighting of the edge represents the distance along the connection.  A given connection will never appear more than 
once, and for a given connection the starting and ending stop will not be the same.

The stops are named using a single letter of the alphabet.  The format of the connection is the letter of the 
initial stop followed by the letter of the final stop and finally a number representing the distance of the 
connection (edge) between the two stops.  For example, a connection between Stop C and Stop D with a distance of
8 is represented as CD8.

Use the following graph as your test input:
 AB5, BC4, CD8, DC8, DE6, AD5, CE2, EB3, AE7
 
OUTPUT:

Your application should compute and output the following using the test input graph above.  If no such route exists 
output “NO SUCH ROUTE”.

The distance of the route A-B-C

The distance of the route A-D

The distance of the route A-D-C

The distance of the route A-E-B-C-D

The distance of the route A-E-D

The number of distinct routes starting at C and ending at C with a maximum of 3 stops.

The number of distinct routes starting at A and ending at C with exactly 4 stops.

The length of the shortest route (in terms of distance, not number of stops) from A to C.

The length of the shortest route (in terms of distance, not number of stops) from B to B.

The number of different routes from C to C with a distance of less than 30.

EXPECTED OUTPUT:
Output #1: 9
Output #2: 5
Output #3: 13 
Output #4: 22
Output #5: NO SUCH ROUTE
Output #6: 2
Output #7: 3
Output #8: 9
Output #9: 9
Output #10: 7 
