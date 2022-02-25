using System.Runtime.CompilerServices;
#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
using System;
#endif

namespace XstarS.Runtime.CompilerServices
{
    /// <summary>
    /// 提供非托管类型的数组相关的帮助方法。
    /// </summary>
    internal static unsafe class UnmanagedTypeArray
    {
        /// <summary>
        /// 确定当前非托管类型的数组与指定非托管类型的数组的值是否二进制相等。
        /// </summary>
        /// <typeparam name="T">非托管类型的数组中的元素的类型。</typeparam>
        /// <param name="array">要进行二进制相等比较的非托管类型的数组。</param>
        /// <param name="other">要与当前值进行比较的非托管类型的数组。</param>
        /// <returns>若 <paramref name="array"/> 与 <paramref name="other"/> 的值二进制相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool BinaryEquals<T>(this T[]? array, T[]? other) where T : unmanaged
        {
            if (object.ReferenceEquals(array, other)) { return true; }
            if ((array is null) || (other is null)) { return false; }
            if (array.Length != other.Length) { return false; }

            fixed (T* pArray = array, pOther = other)
            {
                var size = sizeof(T) * array.Length;
                return BinaryEqualityComparer.Equals(pArray, pOther, size);
            }
        }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        /// <summary>
        /// 确定当前非托管类型的跨度与指定非托管类型的只读跨度的值是否二进制相等。
        /// </summary>
        /// <typeparam name="T">非托管类型的跨度中的元素的类型。</typeparam>
        /// <param name="span">要进行二进制相等比较的非托管类型的跨度。</param>
        /// <param name="other">要与当前跨度进行比较的非托管类型的只读跨度。</param>
        /// <returns>若 <paramref name="span"/> 与 <paramref name="other"/> 的值二进制相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool BinaryEquals<T>(this Span<T> span, ReadOnlySpan<T> other)
            where T : unmanaged
        {
            if (span == other) { return true; }
            if (span.Length != other.Length) { return false; }

            fixed (T* pSpan = span, pOther = other)
            {
                var size = sizeof(T) * span.Length;
                return BinaryEqualityComparer.Equals(pSpan, pOther, size);
            }
        }

        /// <summary>
        /// 确定当前非托管类型的只读跨度与指定非托管类型的只读跨度的值是否二进制相等。
        /// </summary>
        /// <typeparam name="T">非托管类型的跨度中的元素的类型。</typeparam>
        /// <param name="span">要进行二进制相等比较的非托管类型的只读跨度。</param>
        /// <param name="other">要与当前跨度进行比较的非托管类型的只读跨度。</param>
        /// <returns>若 <paramref name="span"/> 与 <paramref name="other"/> 的值二进制相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool BinaryEquals<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> other)
            where T : unmanaged
        {
            if (span == other) { return true; }
            if (span.Length != other.Length) { return false; }

            fixed (T* pSpan = span, pOther = other)
            {
                var size = sizeof(T) * span.Length;
                return BinaryEqualityComparer.Equals(pSpan, pOther, size);
            }
        }
#endif

        /// <summary>
        /// 获取当前非托管类型的数组基于二进制值的哈希代码。
        /// </summary>
        /// <typeparam name="T">非托管类型的数组中的元素的类型。</typeparam>
        /// <param name="array">要获取基于二进制值的哈希代码的非托管类型的数组。</param>
        /// <returns><paramref name="array"/> 基于二进制值的哈希代码。</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetBinaryHashCode<T>(this T[]? array) where T : unmanaged
        {
            if (array is null) { return 0; }

            fixed (T* pArray = array)
            {
                var size = sizeof(T) * array.Length;
                return BinaryEqualityComparer.GetHashCode(pArray, size);
            }
        }
    }
}
