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
                public static readonly int L = 32767; //dictionary size
                [NonSerialized] TST<Nullable<Int16>> st = new TST<Nullable<Int16>>();
                [NonSerialized] List<Int16> list = new List<short>();
                //[NonSerialized] string[] table = new string[L];
                Int16[] compressedData;
                public LZW(string input)
                {
                        for (Int16 i = 0; i != R; ++i)
                        {
                                st.Put("" + (char)i, i); //convert i to one-length string
                                //table[i] = "" + (char)i;
                        }
                        Int16 code = Convert.ToInt16(R + 1);
                        int offset = 0;
                        while (offset != input.Length)
                        {
                                string s = st.LongestPrefixOf(input, offset);
                                list.Add((short)st.Get(s));
                                int t = s.Length;
                                if (t < input.Length && code < L)
                                {
                                       // table[code] = input.Substring(offset, t + 1);
                                        st.Put(input.Substring(offset, t + 1), code); //add new substring to ST
                                        ++code;
                                }
                                offset += t;
                        }
                        compressedData = new short[list.Count];
                        for (int i = 0; i != list.Count; ++i)
                        {
                                compressedData[i] = list[i];
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
                        if (compressedData.Length == 0) return "";
                        string[] table = new string[L];
                        int offset = 0;
                       
                        for (Int16 i = 0; i != R; ++i)
                        {
                                table[i] = "" + (char)i;
                        }
                        StringBuilder sb = new StringBuilder();
                        int position = 0;
                        Int16 code = Convert.ToInt16(R + 1); //current code that will be assigned
                        int codeword = compressedData[position];
                        string val = table[codeword];
                        while(true)
                        {
                                sb.Append(val);
                                ++position;
                                if (position >= compressedData.Length) break;
                                codeword = compressedData[position];
                                string s = table[codeword];
                                if(code == codeword) //special case
                                {
                                        s = val + val[0];
                                }
                                if(code < L)
                                {
                                        table[code++] = val + s[0];
                                }
                                val = s;
                        }
                        return sb.ToString();
                }
        }
}
