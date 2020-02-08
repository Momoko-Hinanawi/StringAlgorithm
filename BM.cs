using System;
using System.Collections.Generic;
using System.Text;

namespace StringAlgorithms
{
        class BM //Boyer Moore substring search
        {
                
                static readonly int R = 256;
                private string Pattern;
                private int[] right = new int[R];
                public BM(string pat)
                {
                        Pattern = pat;
                        for (int i = 0; i != R; ++i)
                        {
                                right[i] = -1;
                        }
                        for (int i = 0; i != pat.Length; ++i)
                        {
                                right[Pattern[i]] = i;
                        }
                }
                public int Search(string text)
                {
                        int N = text.Length;
                        int M = Pattern.Length;
                        int skip = 0;
                        for (int i = 0; i <= N - M; i += skip) //align the pattern to the text from left to right
                        {
                                skip = 0;
                                for (int j = M - 1; j >= 0; j--) //scan the pattern from right to left
                                {
                                        if(Pattern[j] != text[i+j]) //first mismatch
                                        {
                                                skip = Math.Max(1, j - right[text[i + j]]); //calculate skip: align the last matchable char to the corresponding rightmost char of the pattern
                                                break;
                                        }
                                }
                                if (skip == 0) return i;//no mismatch
                        }
                        return -1;
                }
                public static void Main(string[] args)
                {

                }
        }
}
