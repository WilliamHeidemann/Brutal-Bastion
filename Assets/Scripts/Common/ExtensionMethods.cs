using System.Collections.Generic;

namespace Common
{
    public static class ExtensionMethods
    {
        public static T RandomElement<T>(this T[] array)
        {
            var length = array.Length;
            var index = UnityEngine.Random.Range(0, length);
            return array[index];
        }

        public static T RandomElement<T>(this List<T> list)
        {
            var count = list.Count;
            var index = UnityEngine.Random.Range(0, count);
            return list[index];
        }
    }
}