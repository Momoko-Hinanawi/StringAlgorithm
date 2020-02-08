using System;

namespace StringAlgorithms
{
        static class RadixSort
        {
                public static void LSD(string[] a) //ASSUME ALL STRINGS HAVE THE SAME LENGTH
                {
                        int R = 256; //radix
                        int W = a[0].Length; //length of string
                        int N = a.Length;//number of strings
                        var aux = new string[N];
                        for (int d = W - 1; d != -1; --d)
                        {
                                var count = new int[R + 1];
                                for (int i = 0; i != N; ++i) //count frequency 
                                        count[Convert.ToInt16(a[i][d]) + 1]++;
                                for (int i = 0; i != R; ++i) //accumulate frequency
                                        count[i + 1] += count[i];
                                for (int i = 0; i != N; ++i) //access the position in  the sorted array by the original one
                                {
                                        int position = Convert.ToInt16(a[i][d]);
                                        aux[count[position]++] = a[i];
                                }
                                for (int i = 0; i != aux.Length; ++i)
                                {
                                        a[i] = aux[i]; //deep copy; DO NOT SET REFERENCE SINCE aux[] will be different from a[]
                                }
                        }

                }
                public static void MSD(string[] a) //ASSUME ALL STRINGS HAVE THE SAME LENGTH
                {
                        var aux = new string[a.Length];
                        msd(a, aux, 0, a.Length - 1, 0);
                }
                private static void msd(string[] a, string[] aux, int lo, int hi, int d)
                {
                        int R = 256;
                        if (d >= a[0].Length) return;
                        if (hi <= lo) return;
                        var count = new int[R + 2];
                        for (int i = lo; i <= hi; ++i) //count frequency 
                        {
                                count[Convert.ToInt16(a[i][d]) + 2]++; //WARNING:here is +2
                        }
                        for (int i = 0; i != R + 1; ++i)
                        {
                                count[i + 1] += count[i]; //acumulate offsets
                        }
                        for (int i = lo; i <= hi; ++i)
                        {
                                int position = Convert.ToInt16(a[i][d]);
                                aux[count[position + 1]++] = a[i]; // the aux[0] corresponds to a[lo]
                        }
                        for (int i = lo; i <= hi; ++i)
                        {
                                a[i] = aux[i - lo]; //deep copy;be aware of [i - lo] since the aux[] starts from 0
                        }
                        for (int i = 0; i != R; ++i) //sort R subarrays recursively
                        {
                                msd(a, aux, lo + count[i], lo + count[i + 1] - 1, d + 1); //be aware of the offset
                        }
                }
                public static void CountingSort(ref int[] a) //ONLY can be used on integers 0~9
                {
                        var count = new int[11];
                        var aux = new int[a.Length];
                        for (int i = 0; i != a.Length; ++i) //count frequency 
                                count[a[i] + 1]++;
                        for (int i = 0; i != count.Length - 1; ++i) //accumulate frequency to set the starting position of each number
                                count[i + 1] += count[i];
                        for (int i = 0; i != a.Length; ++i)  //access the position in  the sorted array by the original one
                        {
                                aux[count[a[i]]] = a[i];
                                count[a[i]]++;
                        }
                        a = aux; //set the reference to the sorted array
                }
                private static void exch(string[] a, int i, int j)
                {
                        string s = a[i];
                        a[i] = a[j];
                        a[j] = s;
                }
                private static void partition(string[] a, int lo, int hi, int d)
                {
                        if (d >= a[0].Length) return; //out of bound
                        if (hi <= lo) return; //recurse ends
                        int lt = lo, gt = hi;
                        int v; //pivot
                        if(d >= a[lo].Length) v = -1; //out of bounds
                        else v = a[lo][d]; 
                        int i = lo + 1;
                        while (i <= gt) //three way partitioning
                        {
                                int t ;
                                if (d >= a[i].Length) t = -1; //out of bound
                                else t = a[i][d];
                                if (t < v) exch(a, lt++, i++);
                                else if (t == v) ++i;
                                else exch(a, i, gt--);
                        }
                        partition(a, lo, lt - 1, d);
                        if(v>=0)  partition(a, lt, gt, d + 1);
                        partition(a, gt + 1, hi, d);

                }
                public static void Qsort(string[] a) //Can be used on strings with different length
                {
                        partition(a, 0, a.Length - 1, 0);
                }
                public static void Main(string[] args)
                {
                        RWayTrie<int> tries = new RWayTrie<int>();
                        tries.Put("a", 7);
                        tries.Put("abc", 1);
                        tries.Put("abcd", 2);
                        tries.Put("acck", 3);
                        int i = tries.Get("ip");
                        int j = tries.Get("acck");
                        tries.Delete("abcd");
                        i = tries.Get("abc");
                        j = tries.Get("abcd");
                        Console.WriteLine(i);
                        Console.WriteLine(j);
                }
        }
}
