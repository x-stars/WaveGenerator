namespace XstarS.Runtime
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
        internal static bool Equals(void* value, void* other, int size)
        {
            switch (size)
            {
                case 0:
                    return true;
                case sizeof(byte):
                    return *(byte*)value == *(byte*)other;
                case sizeof(ushort):
                    return *(ushort*)value == *(ushort*)other;
                case sizeof(ushort) + sizeof(byte):
                    return (*(ushort*)value == *(ushort*)other) &&
                        (*(byte*)((ushort*)value + 1) == *(byte*)((ushort*)other + 1));
                case sizeof(uint):
                    return *(uint*)value == *(uint*)other;
                case sizeof(uint) + sizeof(byte):
                    return (*(uint*)value == *(uint*)other) &&
                        (*(byte*)((uint*)value + 1) == *(byte*)((uint*)other + 1));
                case sizeof(uint) + sizeof(ushort):
                    return (*(uint*)value == *(uint*)other) &&
                        (*(ushort*)((uint*)value + 1) == *(ushort*)((uint*)other + 1));
                case sizeof(uint) * 2 - sizeof(byte):
                    return (*(uint*)value == *(uint*)other) &&
                        (*((uint*)((byte*)value - 1) + 1) == *((uint*)((byte*)other - 1) + 1));
                case sizeof(ulong):
                    return *(ulong*)value == *(ulong*)other;
                default:
                    if (size < 0)
                    {
                        size = -size;
                        value = (byte*)value - size;
                        other = (byte*)value - size;
                    }
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
                    return (size % sizeof(ulong) == 0) ||
                        BinaryEqualityComparer.Equals(lValue, lOther, size % sizeof(ulong));
            }
        }

        /// <summary>
        /// 获取指定的指针指向的值基于二进制的哈希代码。
        /// </summary>
        /// <param name="value">指向要获取基于二进制的哈希代码的值的指针。</param>
        /// <param name="size">指针指向的值以字节为单位的大小。</param>
        /// <returns><paramref name="value"/> 指向的值基于二进制的哈希代码。</returns>
        internal static int GetHashCode(void* value, int size)
        {
            switch (size)
            {
                case 0:
                    return 0;
                case sizeof(byte):
                    return *(byte*)value;
                case sizeof(ushort):
                    return *(ushort*)value;
                case sizeof(ushort) + sizeof(byte):
                    return *(ushort*)value |
                        (*(byte*)((ushort*)value + 1) << (sizeof(ushort) * 8));
                case sizeof(int):
                    return *(int*)value;
                case sizeof(int) + sizeof(byte):
                    return *(int*)value ^ *(byte*)((int*)value + 1);
                case sizeof(int) + sizeof(ushort):
                    return *(int*)value ^ *(ushort*)((int*)value + 1);
                case sizeof(int) * 2 - sizeof(byte):
                    return *(int*)value ^ *((int*)((byte*)value - 1) + 1);
                case sizeof(int) * 2:
                    return *(int*)value ^ *((int*)value + 1);
                default:
                    if (size < 0)
                    {
                        size = -size;
                        value = (byte*)value - size;
                    }
                    var hashCode = 0;
                    var iValue = (int*)value;
                    var pEnd = iValue + (size / sizeof(int));
                    while (iValue < pEnd)
                    {
                        hashCode ^= *iValue++;
                    }
                    return (size % sizeof(int) == 0) ? hashCode : (hashCode ^
                        BinaryEqualityComparer.GetHashCode(iValue, size % sizeof(int)));
            }
        }
    }
}
