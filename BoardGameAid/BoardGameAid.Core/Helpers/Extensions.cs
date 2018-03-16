using System.Collections.Generic;

namespace BoardGameAid.Core.Helpers
{
    public static class Extensions
    {
        /// <summary>
        /// Shuffle extension for IList lists.
        /// Grabbed from https://stackoverflow.com/a/1262619/3646421
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = ThreadSafeRandom.ThisThreadsRandom.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}