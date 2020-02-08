using System;
using System.Collections.Generic;
using System.Text;
using GraphDemo; //dependency: IGraph.cs; Directedgraph.cs
namespace StringAlgorithms
{
        //REGEX
        class NFA
        {
                private string re;
                private DirectedGraph G;
                private int M;
                public NFA(string regex)
                {
                        re = regex;
                        M = re.Length;
                        //construct NFA
                        G = new DirectedGraph(M + 1);
                        var stack = new Stack<int>();
                        for (int i = 0; i != re.Length; ++i)
                        {
                                if (re[i] == '(')
                                {
                                        G.AddEdge(i, i + 1);
                                        stack.Push(i);
                                }
                                else if (re[i] == '|')
                                {
                                        stack.Push(i);
                                }
                                else if (re[i] == ')') //pop the corresponding '(' and intervening '|'
                                {
                                        G.AddEdge(i, i + 1);
                                        var ors = new List<int>();
                                        int leftBracket = i;
                                        while (stack.Count > 0)
                                        {
                                                int j = stack.Pop();
                                                char c = re[j];
                                                if (c == '|')
                                                {
                                                        ors.Add(j);
                                                }
                                                else //'(' :
                                                {
                                                        foreach (var o in ors)
                                                        {
                                                                G.AddEdge(j, o + 1);
                                                                G.AddEdge(o, i); //processing '|'
                                                        }
                                                        leftBracket = j;
                                                        break;
                                                }
                                        }
                                        if (i + 1 != M && re[i + 1] == '*')
                                        {
                                                G.AddEdge(i + 1, leftBracket);
                                                G.AddEdge(leftBracket, i + 1);
                                                G.AddEdge(i + 1, i + 2);
                                        }
                                }
                                else if (re[i] == '*' && re[i - 1] != ')')
                                {
                                        G.AddEdge(i, i - 1);
                                        G.AddEdge(i - 1, i);
                                        G.AddEdge(i, i + 1);
                                }

                                //for alphabet: do nothing, the match transition is IMPLICIT

                        }
                }
                public bool Accept(string text)
                {
                        var states = new List<int>();
                        var marked = new bool[re.Length + 1];
                        DFS(0, marked);
                        for (int i = 0; i != marked.Length; ++i)
                        {
                                if (marked[i]) states.Add(i); //epsilon transitions : reachable states at the beginning
                        }
                        for (int i = 0; i != text.Length; ++i)
                        {
                                var matches = new List<int>(); //matched states
                                foreach (var s in states)
                                {
                                        if (s == M) continue; //accept state will be ignored until the last judgement
                                        if (re[s] == text[i] || re[i] == '.')  //match transition
                                        {
                                                matches.Add(s + 1);
                                        }
                                }
                                marked = new bool[re.Length + 1]; //discard the old marks, reset marks for DFS
                                foreach (var s in matches)
                                {
                                        DFS(s, marked); //epsilon transitions
                                }
                                states = new List<int>();
                                for (int j = 0; j != marked.Length; ++j)
                                {
                                        if (marked[j]) states.Add(j); //update states
                                }
                        }
                        if (marked[M]) return true; //the last judgement
                        else return false;
                }
                private void DFS(int V, bool[] marked)
                {
                        if (marked[V]) return;
                        marked[V] = true;
                        foreach (var next in G.AdjacentVertices(V))
                        {
                                DFS(next, marked);
                        }
                }
        }
}
