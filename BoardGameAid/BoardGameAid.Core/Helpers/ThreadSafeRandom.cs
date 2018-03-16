using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameAid.Core.Helpers
{
    /// <summary>
    /// A thread safe, true random nubmer generator.
    /// Grabbed from: https://stackoverflow.com/a/1262619/3646421
    /// </summary>
    public class ThreadSafeRandom
    {
        [ThreadStatic] private static Random Local;

        public static Random ThisThreadsRandom
        {
            get { return Local ?? (Local = new Random(unchecked(Environment.TickCount * 31 + Environment.CurrentManagedThreadId))); }
        }
    }
}
