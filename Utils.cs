using System;
using System.Collections.Generic;
using System.Linq;
using SharpDX;
using System.Text;
using System.Threading.Tasks;

namespace PGZ_Desert_Battle
{
    class Utils
    {
        public static void DisposeDictionaryElements<T>(Dictionary<string, T> dictionary) where T: class, IDisposable
        {
            for (int i = dictionary.Count - 1; i >= 0; i--)
            {
                KeyValuePair<string, T> nameValuePair = dictionary.ElementAt(i);
                dictionary.Remove(nameValuePair.Key);
                T value = nameValuePair.Value;
                Utilities.Dispose(ref value);
            }
        }

        public static void DisposeListElements<T>(List<T> list) where T : class, IDisposable
        {
            for (int i = list.Count - 1; i >= 0; i--)
            {
                T value = list.ElementAt(i);
                list.Remove(value);
                Utilities.Dispose(ref value);
            }
        }
    }
}
