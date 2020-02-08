using System;
using System.Collections.Generic;
using System.Diagnostics;
namespace Algorithms
{
        class PriorityQueue<T> where T : IComparable
        {
                private StackByResizeableArray<T> stack;// where the data is stored
                                                        //Left child of stack[i]: stack[2*i]
                                                        //Right child of stack[i]: stack[2*i+1]
                private Comparison<T> comparison;
                public PriorityQueue(T root, Comparison<T> comparison)
                {
                        stack = new StackByResizeableArray<T>();
                        stack.Push(default);
                        stack.Push(root);
                        this.comparison = comparison;
                }
                public PriorityQueue(Comparison<T> comparison)
                {
                        stack = new StackByResizeableArray<T>();
                        stack.Push(default);
                        this.comparison = comparison;
                }
                void Exch(int a, int b)// exchange the stack[a] and stack[b]
                {
                        T i = stack[a];
                        stack[a] = stack[b];
                        stack[b] = i;
                }
                public T Top()// return the smallest key
                {
                        return stack[1];//The root must be the smallest key
                }
                public void Add(T newKey)//Add newKey to the heap
                {
                        int i = stack.Length();// index of the new key
                        stack.Push(newKey);//insert the key into the last place 
                        while (i >= 2)
                        {
                                if (comparison(stack[i], stack[i / 2]) < 0)//If the new key is greater than its father, exchange them
                                {
                                        Exch(i, i / 2);
                                        i /= 2;
                                }
                                else
                                {
                                        break;
                                }
                        }
                        Debug.Assert(IsConsistent());

                }
                public int Size => stack.Length() - 1;
                
                public T Remove()
                {
                        T ret = stack[1];//[1] is the root node
                        Exch(1, Size);
                        stack.Pop();
                        int i = 1;//index of the item which will be sunk
                        while ((2 * i) <= Size)//validate left child
                        {
                                int child = 2 * i;//default: left child
                                if (child + 1 <= Size)//Right child validation
                                {
                                        if (comparison(stack[child], stack[child + 1]) > 0) // Left > right
                                        {
                                                ++child;// if right child is smaller than left, set child to the right
                                        }
                                }
                                if (comparison(stack[child], stack[i]) < 0)
                                {
                                        Exch(child, i);
                                        i = child;
                                }
                                else
                                {
                                        break;
                                }
                        }
                        Debug.Assert(IsConsistent());
                        return ret;
                }
                public void Show()
                {
                        if(Size == 0)
                        {
                                Console.WriteLine("empty");
                                return;
                        }
                        for (int i = 1; i != stack.Length(); ++i)
                        {
                                Console.Write(stack[i] + " ");
                        }
                        Console.WriteLine();
                }
                public T[] ToArray()
                {
                        var array = new T[stack.Length()];
                        for (int i = 0; i != stack.Length(); ++i)
                        {
                                array[i] = stack[i];
                        }
                        array[0] = default;
                        return array;
                }
                private bool IsConsistent()
                {
                        //Check the invariant of heap: any node except the root must smaller than its parent node
                        bool isVerified = true;
                        if (Size == 1) return true;
                        for (int i = 2; i <= Size; ++i)
                        {
                                if (comparison( stack[i] , stack[i / 2]) < 0)
                                {
                                        isVerified = false;
                                        break;
                                }
                        }
                        return isVerified;
                }
                //Demo: add and remove
                public static void Test()
                {
                        var heap = new PriorityQueue<int>((int x,int y) => x.CompareTo(y));
                        int[] a = { 83, 25, 4, 92, 80, 70, 35, 29, 16, 35 };
                        foreach (var i in a)
                        {
                                heap.Add(i);
                        }
                        heap.Show();
                        Console.WriteLine(heap.IsConsistent());
                        for (int i = 0; i != 10; ++i)
                        {
                                heap.Remove();
                                heap.Show();
                                Console.WriteLine(heap.IsConsistent());
                        }
                }

        }
}
