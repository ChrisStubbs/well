using System;
using System.Collections.Generic;
using System.Linq;

namespace PH.Well.Common
{
    public class Utilities
    {
        public static IEnumerable<IEnumerable<T>> Batch<T>(IEnumerable<T> source, int size)
        {
            double max = Math.Ceiling((double)source.Count() / (double)size);

            foreach (var p in Enumerable.Range(0, (int)max))
            {
                yield return source.Skip(p * size).Take(size);
            }
        }
    }
}
