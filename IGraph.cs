using System;
using System.Collections.Generic;
using System.Text;

namespace GraphDemo
{
        public interface IGraph
        {
                int V { get; }
                bool IsEdge(int v, int w);
                //void AddEdge(int v, int w);
                IEnumerable<int> AdjacentVertices(int v); //adjacent vertices to v
        }
}
