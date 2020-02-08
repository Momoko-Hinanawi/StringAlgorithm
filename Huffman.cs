using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Collections;
using System.IO;
using Algorithms;
namespace StringAlgorithms
{
        class Huffman
        {
                class Node : IComparable
                {
                        public Node left;
                        public Node right;
                        public int frequency;
                        public char value;
                        public bool IsLeaf()
                        {
                                return left == null && right == null;
                        }
                        public int CompareTo(object other)
                        {
                                Node node = (Node)other;
                                return this.frequency - node.frequency;
                        }
                        public Node(Node left, Node right, int frequency, char value)
                        {
                                this.left = left;
                                this.right = right;
                                this.frequency = frequency;
                                this.value = value;
                        }
                }
                private static Node readTrie(BinaryReader reader) // read the trie from the compressed stream
                {
                        if (reader.ReadBoolean()) //leafnode
                        {
                                return new Node(null, null, 0, reader.ReadChar());
                        }
                        Node l = readTrie(reader);
                        Node r = readTrie(reader);
                        return new Node(l, r, 0, '\0');
                }
                public static string Expand(BinaryReader reader) //decompress
                {
                        StringBuilder sb = new StringBuilder();
                        Node root = readTrie(reader);
                        int M = reader.ReadInt32();
                        for (int i = 0; i != M; ++i)
                        {
                                var x = root;
                                while (!x.IsLeaf())
                                {
                                        if (reader.ReadBoolean())
                                        {
                                                x = x.right;
                                        }
                                        else
                                        {
                                                x = x.left;
                                        }
                                }
                                sb.Append(x.value);
                        }
                        return sb.ToString();
                }
                private static void writeTrie(BinaryWriter writer, Node node) //write the trie to the compressed stream
                {
                        if (node.IsLeaf())
                        {
                                writer.Write(true);//Console.Write("1");
                                writer.Write(node.value);
                                // Console.Write(node.value);
                                return;
                        }
                        else
                        {
                                writer.Write(false);
                                //Console.Write("0");
                                writeTrie(writer, node.left);
                                writeTrie(writer, node.right);
                        }
                }
                public static void Compress(string text, BinaryWriter writer)
                {
                        Dictionary<char, int> freqs = new Dictionary<char, int>();
                        foreach (char c in text)
                        {
                                if (freqs.ContainsKey(c))
                                {
                                        freqs[c]++;
                                }
                                else
                                {
                                        freqs.Add(c, 1);
                                }
                        }
                        var pq = new PriorityQueue<Node>((Node x, Node y) => x.CompareTo(y));
                        foreach (var c in freqs.Keys)
                        {
                                pq.Add(new Node(null, null, freqs[c], c));
                        }
                        while (pq.Size > 1)
                        {
                                Node l = pq.Remove();
                                Node r = pq.Remove();
                                Node newnode = new Node(l, r, l.frequency + r.frequency, '\0');
                                pq.Add(newnode);
                        }
                        Node root = pq.Remove();
                        writeTrie(writer, root); //write trie to the stream
                                                 //write the amount of chars
                        writer.Write(text.Length);
                        // Console.Write(text.Length);
                        Dictionary<char, bool[]> dict = new Dictionary<char, bool[]>();
                        Stack<bool> path = new Stack<bool>();
                        DFS(root, dict, path);
                        foreach (char c in text)
                        {
                                var seq = dict[c];
                                for (int i = seq.Length - 1; i >= 0; --i) //Stack.ToArray returns inversed array
                                {
                                        writer.Write(seq[i]);
                                        //  Console.Write(seq[i]);
                                }
                                // Console.WriteLine();
                        }
                        return;
                }
                private static void DFS(Node node, Dictionary<char, bool[]> dict, Stack<bool> path)
                {
                        if (node.IsLeaf())
                        {
                                dict.Add(node.value, path.ToArray());
                                return;
                        }
                        path.Push(true);
                        DFS(node.right, dict, path);
                        path.Pop();
                        path.Push(false);
                        DFS(node.left, dict, path);
                        path.Pop();
                }

                public static void Main(string[] args)
                {
                        string s = File.ReadAllText("gallic.mb.txt");
                        using (BinaryWriter writer = new BinaryWriter(File.OpenWrite("huff.txt")))
                        {
                                Huffman.Compress(s, writer);
                        }
                        using (BinaryReader reader = new BinaryReader(File.OpenRead("huff.txt")))
                        {
                                StreamWriter writer = new StreamWriter(File.OpenWrite("decompressed.txt"));
                                writer.Write(Huffman.Expand(reader));
                                writer.Flush();
                        }
                }
        }
}

