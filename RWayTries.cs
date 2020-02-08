using System;
using System.Collections.Generic;
using System.Text;

namespace StringAlgorithms
{
        class RWayTrie<V>
        {
                private static readonly int R = 256;//extended ascii
                private Node root = new Node(); //root is an empty node
                public void Put(string key, V val)
                {
                        if (val.Equals(default(V)))
                        {
                                throw new ArgumentNullException("The assigned value cannot be equal to the default value of V");
                        }
                        root = PutAt(key, val, root, 0);
                }
                private Node PutAt(string key, V val, Node node, int position) //node:current node; position:next position in the string 
                {
                        if (node == null) node = new Node();
                        if (position == key.Length)
                        {
                                node.Value = val;
                                return node;
                        }
                        char c = key[position];
                        node.next[c] = PutAt(key, val, node.next[key[position]], position + 1);
                        return node;
                }
                private class Node
                {
                        public Node[] next = new Node[R];
                        public V Value;
                }
                public V Get(string key) //return the default value when search miss
                                         //e.g. if V<int>, return 0
                {
                        return Getv(key, root, 0);
                }
                private V Getv(string key, Node node, int position)//node:current node; position:next position in the string 
                {
                        if (node == null) return default(V); //search miss
                        if (position == key.Length) return node.Value;
                        return Getv(key, node.next[key[position]], position + 1);
                }
                private void Collect(string prefix, Queue<string> queue, Node node)
                {
                        if (node == null) return;
                        if (!node.Value.Equals(default(V))) queue.Enqueue(prefix);
                        for (char c = Convert.ToChar(0); c != R; ++c)
                        {
                                Collect(prefix + c, queue, node.next[c]);
                        }
                }
                public IEnumerable<string> Prefix(string prefix)
                {
                        var s = new Queue<string>();
                        Node start =root;
                        for(int i = 0;i!=prefix.Length;++i)
                        {
                                start = start.next[prefix[i]];
                        }
                        Collect(prefix, s, start);
                        return s;
                }
                public void Delete(string key)
                {
                        var stack = new Stack<Node>();
                        stack.Push(root);
                        //find the node corresponding to key and set its value to default
                        for (int position = 0; position != key.Length; ++position)
                        {
                                var n = stack.Peek().next[key[position]];
                                if (n == null)
                                {
                                        throw new ArgumentException("Key does not exist");
                                }
                                stack.Push(n);
                        }
                        int pos = key.Length - 1;
                        stack.Peek().Value = default(V);
                        //if the node has all null links and default value, delete it and recur
                        while (stack.Count > 0)
                        {
                                var top = stack.Pop();
                                if (!top.Value.Equals(default(V)))
                                {
                                        return; //if another key exists
                                }
                                bool deathFlag = true;
                                foreach (var n in top.next)
                                {
                                        if (n != null)
                                        {
                                                deathFlag = false;
                                                break;
                                        }
                                }
                                if (deathFlag)
                                {
                                        stack.Peek().next[key[pos]] = null;
                                        --pos;
                                }
                                else
                                {
                                        return;
                                }
                        }
                }

        }
}
