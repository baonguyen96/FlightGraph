namespace FlightGraph.Model
{
    public partial class Graph
    {
        private class Edge
        {
            private Vertex FromVertex { get; set; }
            private Vertex ToVertex { get; set; }
            public int Cost { get; private set; }
            public int Time { get; private set; }


            public Edge(Vertex fromVertex, Vertex toVertex) : this(fromVertex, toVertex, 0, 0)
            {
            }


           
            public Edge(Vertex fromVertex, Vertex toVertex, int cost, int time)
            {
                this.FromVertex = fromVertex;
                this.ToVertex = toVertex;
                this.Cost = cost;
                this.Time = time;
            }

            
            public override bool Equals(object obj)
            {
                Edge edge = (Edge)obj;
                return FromVertex.Equals(edge.FromVertex) && ToVertex.Equals(edge.ToVertex);
            }


            public int GetComparator(string comparator)
            {
                return comparator.Equals(Graph.COST) ? Cost : Time;
            }
        }
    }
}