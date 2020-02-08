using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace StringAlgorithms
{
        class TestClient
        {
                public static void TestTST()
                {
                        TST<int> tree = new TST<int>();
                        tree.Put("s", 1);
                        tree.Put("sak", 2);
                        tree.Put("hongshan", 3);
                        tree.Put("ser", 4);
                        tree.Put("sert", 5);
                        tree.Put("szmithrandir", 6);
                        tree.Put("toronto", 7);
                        Console.WriteLine(tree.Get("ss"));
                        Console.WriteLine(tree.Get("s"));
                        Console.WriteLine(tree.LongestPrefixOf("serbia"));
                        Console.WriteLine(tree.LongestPrefixOf("sz"));
                        Console.WriteLine(tree.LongestPrefixOf("hongshanGuo"));
                        
                }
                public void TestRWay()
                {
                        RWayTrie<int> rWayTrie = new RWayTrie<int>();
                        rWayTrie.Put("SzmithRandir", 1);
                        rWayTrie.Put("Toronto", 2);
                        rWayTrie.Put("Mongol", 3);
                        rWayTrie.Put("Canada", 4);
                        rWayTrie.Put("Canaan", 5);
                        rWayTrie.Put("Montreal", 6);
                        rWayTrie.Put("Mongolian", 7);
                        var ss = rWayTrie.Prefix("");
                        foreach(var s in ss)
                        {
                                Console.WriteLine(s);
                        }
                }
                public void TestNFA()
                {
                        NFA n = new NFA("((A*B|AC|A*C|C)D)");
                        Console.WriteLine(n.Accept("AAABD"));
                        Console.WriteLine(n.Accept("AAACD"));
                        Console.WriteLine(n.Accept("AAAB"));
                        Console.WriteLine(n.Accept("ACD"));
                }
                public static void Main(string[] args)
                {
                        string s = File.ReadAllText("test.txt");
                        LZW lzw = new LZW(s);
                        Console.WriteLine("Compress finished");
                        lzw.Serialize("Compressed.bin");
                        var stream = new FileStream("Compressed.bin",FileMode.Open,FileAccess.Read);
                        IFormatter formatter = new BinaryFormatter();
                        var obj = (LZW)formatter.Deserialize(stream);
                        StreamWriter writer = new StreamWriter(File.OpenWrite("DecbyLZW.txt"));
                        writer.Write(obj.Decompress());
                        writer.Flush();
                }
        }
}
