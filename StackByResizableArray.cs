using System;
namespace Algorithms
{
        public class StackByResizeableArray<T>
        {

                int size = 0;
                int capacity = 2;
                T[] array = new T[2];
                public T this[int i]
                {
                        get
                        {
                                return array[i];
                        }
                        set
                        {
                                array[i] = value;
                        }
                }
                public int Length()
                {
                        return size;
                }
                void transfer(T[] source, T[] destination, int n)
                {
                        for (int i = 0; i != n; ++i)
                        {
                                destination[i] = source[i];
                        }
                }
                public void Push(T newItem)
                {
                        if (size == capacity)
                        {
                                capacity *= 2; //double size if outnumbered
                                var newArray = new T[capacity];
                                transfer(array, newArray, size); //reconstruct an array with double size; 
                                array = newArray; //the former one will be garbage collected
                        }
                        array[size++] = newItem;
                }
                public T Pop()//remove the item which is latest added and return the deleted item
                {
                        if (size == 0) return default;
                        if (size <= capacity / 4) // shrink to half size if the actual size less than 1/4 of capacity
                                                  //If shrink at half size, may cause many useless transfers by adding and removing repeatly
                        {
                                capacity /= 2;
                                var newArray = new T[capacity];
                                transfer(array, newArray, size);
                                array = newArray;
                        }
                        T ret = Top();
                        array[--size] = default; //Set the reference to null to GC the deleted item 
                        return ret;
                }
                public T Top()
                {
                        return array[size - 1];
                }
                public static void Demo()        //Demo of "Stack By Resizable Array"
                {
                        var s = new StackByResizeableArray<int>();
                        for (int i = 0; i != 8; ++i)
                        {
                                s.Push(i + 1);
                        }
                        for (int i = 0; i != 8; ++i)
                        {
                                Console.WriteLine(Convert.ToString(s.Top()));
                                s.Pop();
                        }
                }
        }
}