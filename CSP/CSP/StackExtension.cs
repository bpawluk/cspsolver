using System;
using System.Collections.Generic;
using System.Text;

namespace CSP
{
    public static class StackExtension
    {
        public static Stack<T> Clone<T>(this Stack<T> original)
        {
            T[] array = new T[original.Count];
            original.CopyTo(array, 0);
            Array.Reverse(array);
            return new Stack<T>(array);
        }
    }
}
