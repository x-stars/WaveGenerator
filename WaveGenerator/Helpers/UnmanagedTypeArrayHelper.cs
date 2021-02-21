namespace XstarS.Runtime
{
    /// <summary>
    /// 提供非托管类型的数组相关的帮助方法。
    /// </summary>
    internal static unsafe class UnmanagedTypeArrayHelper
    {
        /// <summary>
        /// 确定当前非托管类型的数组与指定非托管类型的数组是否二进制相等。
        /// </summary>
        /// <typeparam name="T">非托管类型的数组中的元素的类型。</typeparam>
        /// <param name="array">要进行二进制相等比较的非托管类型的数组。</param>
        /// <param name="other">要与当前值进行比较的非托管类型的数组。</param>
        /// <returns>若 <paramref name="array"/> 与 <paramref name="other"/> 二进制相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public static bool BinaryEquals<T>(this T[] array, T[] other) where T : unmanaged
        {
            if (object.ReferenceEquals(array, other)) { return true; }
            if ((array is null) ^ (other is null)) { return false; }
            if (array.Length != other.Length) { return false; }

            fixed (T* pArray = array, pOther = other)
            {
                var size = sizeof(T) * array.Length;
                return BinaryEqualityHelper.BinaryEquals(pArray, pOther, size);
            }
        }

        /// <summary>
        /// 获取当前非托管类型的数组基于二进制值的哈希代码。
        /// </summary>
        /// <typeparam name="T">非托管类型的数组中的元素的类型。</typeparam>
        /// <param name="array">要获取基于二进制值的哈希代码的非托管类型的数组。</param>
        /// <returns><paramref name="array"/> 基于二进制值的哈希代码。</returns>
        public static int GetBinaryHashCode<T>(this T[] array) where T : unmanaged
        {
            if (array is null) { return 0; }

            fixed (T* pArray = array)
            {
                var size = sizeof(T) * array.Length;
                return BinaryEqualityHelper.GetBinaryHashCode(pArray, size);
            }
        }
    }
}
