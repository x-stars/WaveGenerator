using System;
using System.Collections.Generic;
using XstarS.Collections.Generic;

namespace XstarS
{
    /// <summary>
    /// 提供数组的扩展方法。
    /// </summary>
    internal static class ArrayExtensions
    {
        /// <summary>
        /// 确定当前数组与指定数组的元素是否对应相等。
        /// </summary>
        /// <typeparam name="T">数组元素的类型。</typeparam>
        /// <param name="array">要进行相等比较的数组。</param>
        /// <param name="other">要与当前数组进行相等比较的数组。</param>
        /// <param name="comparer">用于比较数组元素的比较器。</param>
        /// <returns>若 <paramref name="array"/> 与
        /// <paramref name="other"/> 的每个元素都对应相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public static bool ArrayEquals<T>(this T[] array, T[] other,
            IEqualityComparer<T> comparer = null)
        {
            return new ArrayEqualityComparer<T>(comparer).Equals(array, other);
        }

        /// <summary>
        /// 返回一个新数组，此数组为当前数组和指定数组连接后的结果。
        /// </summary>
        /// <typeparam name="T"><paramref name="array"/> 中元素的类型。</typeparam>
        /// <param name="array">要进行连接的数组。</param>
        /// <param name="other">要于当前数组连接的数组。</param>
        /// <returns>一个新数组，此数组为当前数组和指定数组连接后的结果。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="array"/>
        /// 或 <paramref name="other"/> 为 <see langword="null"/>。</exception>
        public static T[] Concat<T>(this T[] array, T[] other)
        {
            if (array is null)
            {
                throw new ArgumentNullException(nameof(array));
            }
            if (other is null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            var result = new T[array.Length + other.Length];
            Array.Copy(array, 0, result, 0, array.Length);
            Array.Copy(other, 0, result, array.Length, other.Length);
            return result;
        }

        /// <summary>
        /// 返回一个新数组，此数组为当前交错数组中包含的数组顺序连接后的结果。
        /// </summary>
        /// <typeparam name="T"><paramref name="arrays"/> 包含的数组中元素的类型。</typeparam>
        /// <param name="arrays">要将内层数组顺序相连的数组。</param>
        /// <returns>一个新数组，此数组为指定交错数组中包含的数组顺序连接后的结果。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="arrays"/> 为 <see langword="null"/>。</exception>
        public static T[] Concat<T>(this T[][] arrays)
        {
            if (arrays is null)
            {
                throw new ArgumentNullException(nameof(arrays));
            }

            int length = 0;
            for (int i = 0; i < arrays.Length; i++)
            {
                var array = arrays[i] ?? Array.Empty<T>();
                length += array.Length;
            }

            var result = new T[length];
            int index = 0;
            for (int i = 0; i < arrays.Length; i++)
            {
                var array = arrays[i] ?? Array.Empty<T>();
                Array.Copy(array, 0, result, index, array.Length);
                index += array.Length;
            }
            return result;
        }

        /// <summary>
        /// 返回一个长度等于当前 32 位有符号整数的指定类型的数组，
        /// 数组的每个元素由 <see cref="Converter{TInput, TOutput}"/> 转换索引得到。
        /// </summary>
        /// <typeparam name="T">要创建的数组的元素的类型。</typeparam>
        /// <param name="length">要创建的数组的长度。</param>
        /// <param name="indexMap">用于将索引转换到数组元素的转换器。</param>
        /// <returns>长度为 <paramref name="length"/> 的数组，
        /// 其中的每个元素由 <paramref name="indexMap"/> 转换索引得到。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="indexMap"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="length"/> 小于 0。</exception>
        public static T[] InitializeArray<T>(this int length, Converter<int, T> indexMap)
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }
            if (indexMap is null)
            {
                throw new ArgumentNullException(nameof(indexMap));
            }

            var result = new T[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = indexMap(i);
            }
            return result;
        }
    }
}
