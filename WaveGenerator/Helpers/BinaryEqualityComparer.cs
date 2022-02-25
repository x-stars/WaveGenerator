using System.Runtime.CompilerServices;

namespace XstarS.Runtime.CompilerServices
{
    /// <summary>
    /// 提供二进制相等比较相关的帮助方法。
    /// </summary>
    internal static unsafe class BinaryEqualityComparer
    {
        /// <summary>
        /// 确定指定的两个指针指向的值是否二进制相等。
        /// </summary>
        /// <param name="value">指向要进行二进制相等比较的值的指针。</param>
        /// <param name="other">指向要与当前值进行比较的值的指针。</param>
        /// <param name="size">指针指向的值以字节为单位的大小。</param>
        /// <returns>若 <paramref name="value"/> 与 <paramref name="other"/> 指向的值二进制相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool Equals(void* value, void* other, int size)
        {
        Begin:
            switch (size)
            {
                case 0:
                    return true;
                case sizeof(byte):
                    return ((byte*)value)[0] == ((byte*)other)[0];
                case sizeof(ushort):
                    return ((ushort*)value)[0] == ((ushort*)other)[0];
                case sizeof(ushort) + sizeof(byte):
                    return ((ushort*)value)[0] == ((ushort*)other)[0] &&
                           ((byte*)value)[2] == ((byte*)other)[2];
                case sizeof(uint):
                    return ((uint*)value)[0] == ((uint*)other)[0];
                case sizeof(uint) + sizeof(byte):
                    return ((uint*)value)[0] == ((uint*)other)[0] &&
                           ((byte*)value)[4] == ((byte*)other)[4];
                case sizeof(uint) + sizeof(ushort):
                    return ((uint*)value)[0] == ((uint*)other)[0] &&
                           ((ushort*)value)[2] == ((ushort*)other)[2];
                case sizeof(uint) + sizeof(ushort) + sizeof(byte):
                    return ((uint*)value)[0] == ((uint*)other)[0] &&
                           ((ushort*)value)[2] == ((ushort*)other)[2] &&
                           ((byte*)value)[6] == ((byte*)other)[6];
                case sizeof(ulong):
                    return ((ulong*)value)[0] == ((ulong*)other)[0];
                default:
                    var lValue = (ulong*)value;
                    var lOther = (ulong*)other;
                    var pEnd = lValue + (size / sizeof(ulong));
                    while (lValue < pEnd)
                    {
                        if (*lValue++ != *lOther++)
                        {
                            return false;
                        }
                    }
                    var rest = size % sizeof(ulong);
                    if (rest == 0) { return true; }
                    value = lValue; other = lOther; size = rest;
                    goto Begin;
            }
        }

        /// <summary>
        /// 获取指定的指针指向的值基于二进制的哈希代码。
        /// </summary>
        /// <param name="value">指向要获取基于二进制的哈希代码的值的指针。</param>
        /// <param name="size">指针指向的值以字节为单位的大小。</param>
        /// <returns><paramref name="value"/> 指向的值基于二进制的哈希代码。</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int GetHashCode(void* value, int size)
        {
            var hashCode = 0;
        Begin:
            switch (size)
            {
                case 0:
                    return 0;
                case sizeof(byte):
                    return ((byte*)value)[0];
                case sizeof(ushort):
                    return ((ushort*)value)[0];
                case sizeof(ushort) + sizeof(byte):
                    return ((ushort*)value)[0] | (((byte*)value)[2] << 16);
                case sizeof(int):
                    return ((int*)value)[0];
                case sizeof(int) + sizeof(byte):
                    return ((int*)value)[0] ^ ((byte*)value)[4];
                case sizeof(int) + sizeof(ushort):
                    return ((int*)value)[0] ^ ((ushort*)value)[2];
                case sizeof(int) + sizeof(ushort) + sizeof(byte):
                    return ((int*)value)[0] ^ (((ushort*)value)[2] |
                                               (((byte*)value)[6] << 16));
                case sizeof(int) + sizeof(int):
                    return ((int*)value)[0] ^ ((int*)value)[1];
                default:
                    var iValue = (int*)value;
                    var pEnd = iValue + (size / sizeof(int));
                    while (iValue < pEnd)
                    {
                        hashCode ^= *iValue++;
                    }
                    var rest = size % sizeof(int);
                    if (rest == 0) { return hashCode; }
                    value = iValue; size = -rest;
                    goto Begin;
                case -(sizeof(byte)):
                    return hashCode ^ ((byte*)value)[0];
                case -(sizeof(ushort)):
                    return hashCode ^ ((ushort*)value)[0];
                case -(sizeof(ushort) + sizeof(byte)):
                    return hashCode ^ (((ushort*)value)[0] |
                                       (((byte*)value)[2] << 16));
                case -(sizeof(int)):
                    return hashCode ^ ((int*)value)[0];
            }
        }
    }
}
