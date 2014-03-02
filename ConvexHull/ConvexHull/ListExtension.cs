using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvexHull
{
    public static class ListExtension
    {
        public static IList<T> ForEach<T>(this IList<T> list, Action<T> action)
        {
            foreach (T item in list)
            {
                action(item);
            }

            return list;
        }
    }
}
