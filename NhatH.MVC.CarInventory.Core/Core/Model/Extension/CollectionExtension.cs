using System;
using System.Collections.Generic;

namespace NhatH.MVC.CarInventory.Core.Core.Model.Extension
{
    public static class CollectionExtension
    {
        public static void RemoveIf<T>(this ICollection<T> collection, Predicate<T> match)
        {
            List<T> removed = new List<T>();
            foreach (T item in collection)
            {
                if (match(item))
                {
                    removed.Add(item);
                }
            }

            foreach (T item in removed)
            {
                collection.Remove(item);
            }

            removed.Clear();
        }

    }
}
