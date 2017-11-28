using System;
using System.Collections.Generic;

namespace PH.Well.Common
{
    public class EqualityComparer<T> : IEqualityComparer<T>
    {
        private Func<T, T, bool> equalsFunction;
        private Func<T, int> getHashCodeFunction;

        public EqualityComparer(Func<T, T, bool> equalsFunction)
        {
            this.equalsFunction = equalsFunction;
        }

        public EqualityComparer(Func<T, T, bool> equalsFunction,
            Func<T, int> getHashCodeFunction) : this(equalsFunction)
        {
            this.getHashCodeFunction = getHashCodeFunction;
        }

        public bool Equals(T a, T b)
        {
            return equalsFunction(a, b);
        }

        public int GetHashCode(T obj)
        {
            if (getHashCodeFunction == null)
            {
                return obj.GetHashCode();
            }

            return getHashCodeFunction(obj);
        }
    }
}
