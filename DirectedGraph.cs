using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace GraphDemo
{
        class DirectedGraph : IGraph //Used for NFA
        {
                //Implemented by adjacent list
                int vertices = 0;
                private List<int>[] adj;
                int IGraph.V => vertices;
                public DirectedGraph(int v) //vertices
                {
                        vertices = v;
                        adj = new List<int>[vertices];
                        for (int i = 0; i != adj.Length; ++i)
                        {
                                adj[i] = new List<int>();
                        }
                }
                public DirectedGraph(string fileName)
                {
                        string fileContent = File.ReadAllText(fileName);
                        string[] integerStrings = fileContent.Split(new char[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                        int[] integers = new int[integerStrings.Length];
                        for (int n = 0; n < integerStrings.Length; n++)
                                integers[n] = int.Parse(integerStrings[n]);
                        vertices = integers[0];
                        adj = new List<int>[vertices]; //initialize adjacent list
                        for (int i = 0; i != adj.Length; ++i)
                        {
                                adj[i] = new List<int>(); //initialize adjacent lists
                        }
                        int edges = integers[1];
                        for (int i = 0; i != edges; ++i)
                        {
                                AddEdge(integers[2 * i + 2], integers[2 * i + 3]);
                        }
                }
                public void AddEdge(int v, int w) //TODO:debug
                {
                        Debug.Assert(v != w, "Self loop is not allowed");// No self-loop
                        Debug.Assert(!IsEdge(v, w), "Parallel edges are not allowed");//No parallel edges
                        if (w >= vertices) throw new ArgumentException();
                        adj[v].Add(w);
                }
                public IEnumerable<int> AdjacentVertices(int v) //adjacent vertices to v
                {
                        return adj[v];
                }
                public void ShowAllEdges() //TODO:debug
                {
                        for (int i = 0; i != adj.Length; ++i)
                        {
                                foreach (var j in adj[i])
                                {
                                        Console.WriteLine(i + " " + j);
                                }
                        }
                }
                public bool IsEdge(int v, int w)
                {
                        foreach (var i in adj[v])
                        {
                                if (i == w) return true;
                        }
                        return false;
                }
        }
}
