/***
 * Author: Bao Nguyen
 * 
 * Find 3 shortest paths using Dijstrak's algorithm.
 *
 * class: Graph
 * defines a graph with vertices and weighted, directional edges
 */


using System;
using System.Collections.Generic;
using System.IO;

namespace FlightGraph
{
    class Graph
    {
        /***
         * class: Vertex
         * represent each vertex of the graph
         */
        private class Vertex : IComparable<Vertex>
        {
            public List<Vertex> AdjacentVertices { get; set; }        // list of all adjacent vertices
            public int Cost { get; set; }                              // current least cost (distance)
            public int Time { get; set; }                            // current least time
            public String Name { get; set; }                         // name (location) of the vertex
            public Vertex PreviousVertex { get; set; }               // previous vertex in the shortest path
            public int CompareValue { get; set; }                    // cost or time
            public static String Comparator { get; set; }            // check the thing to compare


            /***
             * overloaded constructor
             * set up new vertex
             * @param name: location of the vertex
             */
            public Vertex(String name) : this(0, 0, name)
            {
            }


            /***
             * overloaded constructor
             * set up new vertex
             * @param cost: current distance of the vertex
             * @param time: current time of the vertex
             * @param name: location of the vertex
             */
            public Vertex(int cost, int time, String name)
            {
                this.Cost = cost;
                this.Time = time;
                this.Name = name;
                AdjacentVertices = new List<Vertex>();
                PreviousVertex = null;
                CompareValue = 0;
            }


            /***
             * copy constructor
             * copy value of the param vertex to this vertex
             * @param vertex: vertex to copy
             */
            public Vertex(Vertex vertex)
            {
                Cost = vertex.Cost;
                Time = vertex.Time;
                Name = vertex.Name;
                AdjacentVertices = vertex.AdjacentVertices;
                PreviousVertex = vertex.PreviousVertex;
                CompareValue = vertex.CompareValue;
            }

            /***
             * method: AddAdjacentVertex
             * add new vertex to its adjacent list
             * @param vertex: adjacent vertex
             */
            public void AddAdjacentVertex(Vertex vertex)
            {
                AdjacentVertices.Add(vertex);
            }


            /***
             * method: Equals (overridden)
             * defines how 2 vertices are equalled:
             * vertices are equalled if they have the same name
             * @param object: another vertex to compare
             * @return true if 2 vertices are equalled, false if not
             */
            public override bool Equals(Object o)
            {
                Vertex vertex = (Vertex)o;
                return Name.Equals(vertex.Name);
            }


            /***
             * method: GetComparator
             * return the value of the comparator
             * @param comparator: what thing to compare
             * @return comparator value
             */
            public int GetComparator(String comparator)
            {
                return comparator.Equals(Graph.COST) ? Cost : Time;
            }


            /***
             * method: compareTo (overridden)
             * compare the compareValue of 2 vertices
             * @param o: new vertex to compare
             * @return indication (int) of larger vertex
             */
            public int CompareTo(Vertex o)
            {
                // set the compare value according to the mode of the comparator
                CompareValue = Comparator.Equals(Graph.COST) ? Cost : Time;
                o.CompareValue = Comparator.Equals(Graph.COST) ? o.Cost : o.Time;

                // compare
                if (CompareValue < o.CompareValue)
                    return -1;
                else if (CompareValue == o.CompareValue)
                    return 0;
                else
                    return 1;
            }
        }


        /***
         * class: Edge
         * represents the edge between 2 vertices:
         * fromVertex --- (time|cost) ---> toVertex
         */
        private class Edge
        {
            public Vertex FromVertex { get; set; }
            public Vertex ToVertex { get; set; }
            public int Cost { get; set; }
            public int Time { get; set; }


            /***
             * overloaded constructor
             * setup an edge
             * @param fromVertex: vertex that the edge is leaving from
             * @param toVertex: vertex that the edge is going to
             */
            public Edge(Vertex fromVertex, Vertex toVertex) : this(fromVertex, toVertex, 0, 0)
            {
            }


            /***
             * overloaded constructor
             * setup an edge
             * @param fromVertex: vertex that the edge is leaving from
             * @param toVertex: vertex that the edge is going to
             * @param cost: the cost (distance) of the edge
             * @param time: the time of the edge
             */
            public Edge(Vertex fromVertex, Vertex toVertex, int cost, int time)
            {
                this.FromVertex = fromVertex;
                this.ToVertex = toVertex;
                this.Cost = cost;
                this.Time = time;
            }


            /***
             * method: Equals
             * compare 2 edges
             * they are equalled if their start vertices and end vertices are the same
             * @param obj: another edge to compare
             * @return true if they Equals, false if not
             */
            public override bool Equals(Object obj)
            {
                Edge edge = (Edge)obj;
                return FromVertex.Equals(edge.FromVertex) && ToVertex.Equals(edge.ToVertex);
            }


            /***
             * method: GetComparator
             * return the value of the comparator
             * @param comparator: what thing to compare
             * @return comparator value
             */
            public int GetComparator(String comparator)
            {
                return comparator.Equals(Graph.COST) ? Cost : Time;
            }
        }


        private List<Vertex> Vertices { set; get; }          // list of all vertices in the graph
        private List<Edge> Edges { set; get; }               // list of all edges
        public const String COST = "C";                      // cost as comparator
        public const String TIME = "T";                      // time as comparator


        /***
         * constructor
         * setup new Graph object
         * @param fileName: file of flights
         */
        public Graph(String fileName)
        {
            this.Vertices = new List<Vertex>();
            this.Edges = new List<Edge>();
            try
            {
                CreateGraph(fileName);
            }
            catch (FileNotFoundException)
            {
                throw;
            }
        }


        /***
         * method: createGraph
         * read data from the file and setup vertices & edges of the graph
         * @param fileName: input file
         */
        private void CreateGraph(String fileName)
        {
            try
            {
                StreamReader iFile = new StreamReader(fileName);

                using (iFile)
                {
                    int count = int.Parse(iFile.ReadLine());

                    for (int i = 0; i < count; i++)
                    {
                        string input = iFile.ReadLine();
                        string[] data = input.Split('|');

                        if (data.Length == 4)
                        {
                            // setup new vertices
                            Vertex fromVertex = new Vertex(data[0]);
                            Vertex toVertex = new Vertex(data[1]);

                            // add to the list of vertices if it has not been defined
                            if (!Vertices.Contains(fromVertex))
                                Vertices.Add(fromVertex);
                            if (!Vertices.Contains(toVertex))
                                Vertices.Add(toVertex);

                            // setup edge
                            Edges.Add(new Edge(fromVertex, toVertex, int.Parse(data[2]), int.Parse(data[3])));

                            // setup adjacent Vertices
                            Vertices[Vertices.IndexOf(fromVertex)].AddAdjacentVertex(
                                    Vertices[Vertices.IndexOf(toVertex)]);
                        }
                    }

                }
            }
            catch (FileNotFoundException)
            {
                //throw new FileNotFoundException(string.Format("Error: Cannot open the file {0}", fileName));
                throw;
            }

        }


        /***
         * method: SetupDijstraksSearch
         * initial starting vertex to 0 and all others to "infinity"
         * @param fromVertex: starting vertex
         */
        private void SetupDijstraksSearch(String fromVertex)
        {
            foreach (Vertex vertex in Vertices)
            {
                if (vertex.Name.Equals(fromVertex))
                {
                    vertex.Cost = 0;
                    vertex.Time = 0;
                }
                else
                {
                    vertex.Cost = int.MaxValue;
                    vertex.Time = int.MaxValue;
                }
            }
        }


        /***
         * method: findShortestPath
         * find shortest path from the starting vertex to the ending vertex
         * using Dijkstra’s algorithm
         * @param fromVertex: starting vertex
         * @param toVertex: ending vertex
         * @param comparator: compare mode
         * @return a string array of 3 shortest paths
         */
        public String[] FindShortestPath(String fromVertex, String toVertex, String comparator)
        {
            String[] shortestPaths = new String[3];                 // results
            List<Vertex> queue = new List<Vertex>(Vertices);        // hold all current vertices
            Vertex.Comparator = comparator;                         // compare mode
            int shortestPathValue = int.MaxValue,                   // shortest path value
                shortestPathValue2 = int.MaxValue,                  // second shortest path value
                shortestPathValue3 = int.MaxValue;                  // third shortest path value
            Vertex[] shortestVertices = new Vertex[3];              // path sources

            // initial graph to search
            SetupDijstraksSearch(fromVertex);
            ClearPreviousVertices();

            while (queue.Count != 0)
            {
                queue.Sort();
                Vertex currentVertex = queue[0];
                queue.RemoveAt(0);

                /*
                 * if the vertex on the top of the queue is the destination and the queue is not empty,
                 * (there is a shortcut to go to destination without going through every vertex),
                 * remove another vertex and add the current vertex back into the queue. This is not
                 * necessary to find the shortest path, but since we are interested in finding 3
                 * shortest paths, this step is crucial.
                 */
                if (currentVertex.Equals(new Vertex(toVertex)) && queue.Count != 0)
                {
                    queue.Sort();
                    Vertex newVertex = queue[0];
                    queue.RemoveAt(0);
                    queue.Add(currentVertex);
                    currentVertex = newVertex;
                }

                foreach (Vertex adjacentVertex in currentVertex.AdjacentVertices)
                {
                    if (queue.Contains(adjacentVertex))
                    {
                        // get values
                        int currentVertexValue = currentVertex.GetComparator(comparator);
                        int edgeValue = Edges[Edges.IndexOf(new Edge(
                                currentVertex, adjacentVertex))].GetComparator(comparator);
                        int adjacentVertexComparatorValue = adjacentVertex.GetComparator(comparator);
                        int currentPathValue = currentVertexValue + edgeValue;

                        // if find new shortest path from starting vertex to the current.adj one
                        if (currentPathValue < adjacentVertexComparatorValue)
                        {
                            // update adjacent vertex
                            UpdateVertex(adjacentVertex, currentVertex);

                            // update new shortest value
                            if (adjacentVertex.Equals(new Vertex(toVertex)))
                            {
                                shortestVertices[0] = new Vertex(adjacentVertex);
                                shortestPathValue = currentVertexValue + edgeValue;
                            }
                        }
                        // second shortest path
                        else if (currentPathValue > shortestPathValue &&
                                currentPathValue < shortestPathValue2)
                        {

                            /*
                             * if already found the second shortest path (the current
                             * is the new second shortest <=> old second become third),
                             * copy old second to third and create new second shortest
                             */
                            if (ContainsVertex(shortestVertices[1], adjacentVertex))
                            {
                                shortestVertices[2] = new Vertex(shortestVertices[1]);
                                shortestPathValue3 = shortestVertices[2].CompareValue;
                            }

                            shortestVertices[1] = new Vertex(shortestVertices[0]);
                            UpdateVertex(shortestVertices[1], currentVertex);
                            shortestPathValue2 = currentPathValue;
                        }
                        // third shortest path
                        else if (currentPathValue > shortestPathValue2 &&
                                currentPathValue < shortestPathValue3)
                        {

                            // apply the same logic as second shortest path
                            if (ContainsVertex(shortestVertices[1], adjacentVertex))
                            {
                                shortestVertices[2] = new Vertex(shortestVertices[1]);
                            }

                            shortestVertices[2] = new Vertex(shortestVertices[1]);
                            UpdateVertex(shortestVertices[2], currentVertex);
                            shortestPathValue3 = currentPathValue;
                        }
                    }
                }

                // update priority queue
                if (queue.Count != 0)
                {
                    Vertex vertex = queue[0];
                    queue.RemoveAt(0);
                    queue.Add(vertex);
                }
            }

            // print paths
            for (int i = 0; i < 3; i++)
            {
                shortestPaths[i] = PrintShortestPath(shortestVertices[i]);
            }

            return shortestPaths;
        }


        /***
         * method: ContainsVertex
         * check to see if a path already contains a given vertex
         * to ensure there is no cycle in the path
         * @param currentVertex: current (end) vertex of the path
         * @param checkContainment: vertex to check if it is in the path
         * @return true if the checkContainment vertex is already in the path,
         *          false otherwise
         */
        private bool ContainsVertex(Vertex currentVertex, Vertex checkContainment)
        {
            if (currentVertex == null)
                return false;

            Vertex current = new Vertex(currentVertex);

            // keep checking till the first vertex of the path
            while (current != null)
            {
                if (current.Equals(checkContainment))
                    return true;
                current = current.PreviousVertex;
            }
            return false;
        }


        /***
         * method: ClearPreviousVertices
         * for each vertex in the graph, reset its previousVertex to null
         */
        private void ClearPreviousVertices()
        {
            foreach (Vertex vertex in Vertices)
                vertex.PreviousVertex = null;
        }


        /***
         * method: UpdateVertex
         * update new least time and cost of the adjacent vertex
         * @param adjacentVertex: vertex adjacent to the current one
         * @param currentVertex: current vertex
         */
        private void UpdateVertex(Vertex adjacentVertex, Vertex currentVertex)
        {
            adjacentVertex.Cost = currentVertex.Cost + Edges[Edges.IndexOf(
                    new Edge(currentVertex, adjacentVertex))].Cost;
            adjacentVertex.Time = currentVertex.Time + Edges[Edges.IndexOf(
                    new Edge(currentVertex, adjacentVertex))].Time;
            adjacentVertex.PreviousVertex = currentVertex;
        }


        /***
         * method: PrintShortestPath
         * print out the shortest path from starting vertex to ending vertex
         * @param toVertex: ending vertex
         */
        private String PrintShortestPath(Vertex toVertex)
        {
            if (toVertex == null || toVertex.PreviousVertex == null)
                return "No available path.\r\n";

            String path = PrintShortestPath(toVertex, true);
            return path + string.Format(". Time: {0}, Cost: {1}\r\n", toVertex.Time, toVertex.Cost);
        }


        /***
         * method: PrintShortestPath (overloaded)
         * helper method to print out the shortest path
         * from starting vertex to ending vertex
         * @param toVertex: ending vertex
         */
        private String PrintShortestPath(Vertex toVertex, bool b)
        {
            if (toVertex.PreviousVertex != null)
                return PrintShortestPath(toVertex.PreviousVertex, b) + " -> " + toVertex.Name;
            return toVertex.Name;
        }
    }
}
