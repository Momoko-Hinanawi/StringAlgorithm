using System;
using System.Collections.Generic;
using System.Text;

namespace StringAlgorithms
{
        class KMP //KMP substring search
        {
                public readonly string Pattern;
                private readonly int[,] DFA;
                public int DFAat(int i, int j)
                {
                        return DFA[i, j];
                }
                public readonly int R = 256;
                public KMP(string pat)
                {
                        Pattern = pat;
                        //build DFA
                        DFA = new int[R, Pattern.Length + 1];
                        //match building
                        for (int i = 0; i != Pattern.Length; ++i)
                        {
                                DFA[Pattern[i], i] = i + 1;
                        }
                        //mismatch building
                        if (Pattern.Length <= 1) return; //special case
                        int restartState = 0; //for mismatch of state i, the restart state is determined by transition [0..i-1]
                        for (int i = 1; i != Pattern.Length; ++i) //mismatch in state i
                        {
                                for (int j = 0; j != R; ++j)
                                        DFA[j, i] = DFA[j, restartState]; //restart state
                                DFA[Pattern[i], i] = i + 1; //maintain match transition
                                restartState = DFA[Pattern[i], restartState]; //update restart state
                        }
                }
                public int Search(string text)
                {
                        //return -1 if no match found
                        int state = 0; //initial state: 0; final state: Pattern.Length
                        for (int i = 0; i != text.Length; ++i)
                        {
                                char c = text[i];
                                state = DFA[c, state];
                                if (state == Pattern.Length)
                                {
                                        return i - Pattern.Length + 1;
                                }
                        }
                        return -1;
                }
                public static void Main(string[] args)
                {
                        KMP k = new KMP("ababac");
                        for (int i = 'a'; i <= 'c'; ++i)
                        {
                                for (int j = 0; j != 6; ++j)
                                {
                                        Console.Write(k.DFA[i, j] + " ");
                                }
                                Console.WriteLine();
                        }
                }
        }

}


