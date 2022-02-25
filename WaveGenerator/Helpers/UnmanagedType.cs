using System.Runtime.CompilerServices;

namespace XstarS.Runtime.CompilerServices
{
    /// <summary>
    /// 提供非托管类型的值相关的帮助方法。
    /// </summary>
    internal static unsafe class UnmanagedType
    {
        /// <summary>
        /// 获取当前非托管类型以字节为单位的大小。
        /// </summary>
        /// <typeparam name="T">要获取大小的非托管类型。</typeparam>
        /// <returns>当前非托管类型以字节为单位的大小。</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SizeOf<T>() where T : unmanaged => sizeof(T);

        /// <summary>
        /// 确定当前非托管类型的值与指定非托管类型的值是否二进制相等。
        /// </summary>
        /// <typeparam name="T">非托管值的类型。</typeparam>
        /// <param name="value">要进行二进制相等比较的非托管类型的值。</param>
        /// <param name="other">要与当前值进行比较的非托管类型的值。</param>
        /// <returns>若 <paramref name="value"/> 与 <paramref name="other"/> 二进制相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool BinaryEquals<T>(this T value, T other) where T : unmanaged
        {
            return BinaryEqualityComparer.Equals(&value, &other, sizeof(T));
        }

        /// <summary>
        /// 获取当前非托管类型的值基于二进制值的哈希代码。
        /// </summary>
        /// <typeparam name="T">非托管值的类型。</typeparam>
        /// <param name="value">要获取基于二进制值的哈希代码的非托管类型的值。</param>
        /// <returns><paramref name="value"/> 基于二进制值的哈希代码。</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetBinaryHashCode<T>(this T value) where T : unmanaged
        {
            return BinaryEqualityComparer.GetHashCode(&value, sizeof(T));
        }

        /// <summary>
        /// 将当前非托管类型的二进制值填充到对应长度的字节数组并返回。
        /// </summary>
        /// <typeparam name="T">非托管值的类型。</typeparam>
        /// <param name="value">要填充到字节数组的非托管类型的值。</param>
        /// <returns>以 <paramref name="value"/> 的二进制值填充的字节数组。</returns>
        public static byte[] ToByteArray<T>(this T value) where T : unmanaged
        {
            var size = sizeof(T);
            var bytes = new byte[size];
            fixed (byte* pBytes = bytes)
            {
                *(T*)pBytes = value;
            }
            return bytes;
        }
    }
}
