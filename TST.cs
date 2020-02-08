using System;
using System.Collections.Generic;
using System.Text;

namespace StringAlgorithms
{
        class TST<V>
        {
                Node root = null;
                class Node
                {
                        public char key;
                        public V value;
                        public Node lChild;
                        public Node mChild;
                        public Node rChild;
                }
                public void Put(string s, V val)
                {
                        putNode(s, val,ref root, 0);
                }
                private void putNode(string s, V val,ref Node node, int pos)
                {
                        if (node == null) //create node
                        {
                                node = new Node();
                                node.key = s[pos];
                        }
                        if (s[pos] < node.key) //go left
                        {
                                putNode(s, val,ref node.lChild, pos);
                                return;
                        }
                        if(s[pos] > node.key) //go right
                        {
                                putNode(s, val,ref node.rChild, pos);
                                return;
                        }
                        //s[pos] == node.key
                        if (pos == s.Length - 1) //last char of the string
                                node.value = val;
                        else putNode(s, val, ref node.mChild, pos + 1);

       
                }
                public V Get(string key)
                {
                        return GetV(key, 0, root);
                }
                private V GetV(string key,int pos,Node node)
                {
                        if (node == null) return default(V);
                       
                        if(key[pos] < node.key) return GetV(key, pos, node.lChild);
                        if(key[pos] > node.key) return GetV(key, pos, node.rChild);
                        //if(key[pos] == node.key)
                        if (pos < key.Length-1) return GetV(key, pos + 1, node.mChild); 
                        return node.value; //last judgement: return default if no value assigned to the node;return its value if assigned
                }
                public void Delete(string key)
                {
                        throw new NotImplementedException();
                }
                public string LongestPrefixOf(string str)
                {
                        return str.Substring(0,longestPrefixOf(str, 0, root));
                }
                private int longestPrefixOf(string str,int position, Node node)
                {
                        if (node == null) return position;
                        if (position == str.Length) return position;
                        if( str[position] == node.key) //match
                        {
                                return longestPrefixOf(str, position + 1, node.mChild);
                        }
                        if(str[position] < node.key)
                        {
                                return longestPrefixOf(str, position, node.lChild);
                        }
                        else
                        {
                                return longestPrefixOf(str, position, node.rChild);
                        }
                }
        }
}
