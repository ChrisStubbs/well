using System.Collections.Generic;

namespace PH.Well.Common
{
    public class Utilities
    {
        public static IEnumerable<IEnumerable<T>> Batch<T>(IEnumerable<T> source, int size)
        {
            var list = new List<T>(size);

            foreach (T item in source)
            {
                list.Add(item);
                if (list.Count == size)
                {
                    List<T> chunk = list;
                    list = new List<T>(size);
                    yield return chunk;
                }
            }

            if (list.Count > 0)
            {
                yield return list;
            }
        }
    }
}
