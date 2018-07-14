using System;
using System.Collections.Generic;

namespace FlightGraph.Model
{
    public partial class Graph
    {
        class Vertex : IComparable<Vertex>
        {
            public List<Vertex> AdjacentVertices { get; private set; }
            public int Cost { get; set; }
            public int Time { get; set; }
            public string Name { get; private set; }
            public Vertex PreviousVertex { get; set; }
            public int CompareValue { get; private set; }
            public static string Comparator { private get; set; }



            public Vertex(string name) : this(0, 0, name)
            {
            }


            public Vertex(int cost, int time, String name)
            {
                this.Cost = cost;
                this.Time = time;
                this.Name = name;
                AdjacentVertices = new List<Vertex>();
                PreviousVertex = null;
                CompareValue = 0;
            }



            public Vertex(Vertex vertex)
            {
                Cost = vertex.Cost;
                Time = vertex.Time;
                Name = vertex.Name;
                AdjacentVertices = vertex.AdjacentVertices;
                PreviousVertex = vertex.PreviousVertex;
                CompareValue = vertex.CompareValue;
            }

            public void AddAdjacentVertex(Vertex vertex)
            {
                AdjacentVertices.Add(vertex);
            }


            public override bool Equals(object o)
            {
                Vertex vertex = (Vertex)o;
                return Name.Equals(vertex.Name);
            }


            public int GetComparator(string comparator)
            {
                return comparator.Equals(Graph.COST) ? Cost : Time;
            }



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
    }
    
}