using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace StringAlgorithms
{
        [Serializable]
        class LZW //LZW compression
        {
                public static readonly Int16 R = 256;
                public static readonly int L = 1023; //dictionary size
                [NonSerialized] TST<Nullable<Int16> > st = new TST<Nullable<Int16>>();
                List<Int16> list = new List<short>();
                string[] table = new string[L];
                public LZW(string input)
                {
                        for(Int16 i = 0; i!=R;++i)
                        {
                                st.Put("" + (char)i, i); //convert i to one-length string
                                table[i] = "" + (char)i;
                        }
                        Int16 code = Convert.ToInt16( R + 1);
                        while(input.Length>0)
                        {
                                string s = st.LongestPrefixOf(input);
                                list.Add((short)st.Get(s));
                                int t = s.Length;
                                if(t < input.Length && code < L)
                                {
                                        table[code] = input.Substring(0, t + 1);
                                        st.Put(input.Substring(0, t + 1), code); //add new substring to ST
                                        ++code;
                                }
                                input = input.Substring(t);
                                
                        }
                }
                public void Serialize(string fileName)
                {
                        IFormatter formatter = new BinaryFormatter();
                        Stream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None);
                        formatter.Serialize(stream, this);
                        stream.Close();
                }
                public string Decompress()
                {
                        StringBuilder sb = new StringBuilder();
                        foreach(var i in list)
                        {
                                sb.Append(table[i]);
                        }
                        return sb.ToString();
                }
        }
}
