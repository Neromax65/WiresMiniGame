using System.Collections.Generic;

namespace Helpers
{
    public static class Extensions
    {
        /// <summary>
        /// Перемешать коллекцию IList
        /// </summary>
        /// <param name="ts">Коллекция IList</param>
        /// <typeparam name="T">Тип объектов</typeparam>
        public static void Shuffle<T>(this IList<T> ts)
        {
            var count = ts.Count;
            var last = count - 1;
            for (var i = 0; i < last; ++i)
            {
                var r = UnityEngine.Random.Range(i, count);
                var tmp = ts[i];
                ts[i] = ts[r];
                ts[r] = tmp;
            }
        }
    }
}