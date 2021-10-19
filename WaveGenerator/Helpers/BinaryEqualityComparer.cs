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
                case sizeof(sbyte):
                    return *(sbyte*)value == *(sbyte*)other;
                case sizeof(short):
                    return *(short*)value == *(short*)other;
                case sizeof(short) + sizeof(sbyte):
                    return (*(short*)value == *(short*)other) &&
                        (*(sbyte*)((short*)value + 1) == *(sbyte*)((short*)other + 1));
                case sizeof(int):
                    return *(int*)value == *(int*)other;
                case sizeof(int) + sizeof(sbyte):
                    return (*(int*)value == *(int*)other) &&
                        (*(sbyte*)((int*)value + 1) == *(sbyte*)((int*)other + 1));
                case sizeof(int) + sizeof(short):
                    return (*(int*)value == *(int*)other) &&
                        (*(short*)((int*)value + 1) == *(short*)((int*)other + 1));
                case sizeof(int) * 2 - sizeof(byte):
                    return (*(int*)value == *(int*)other) &&
                        (*((int*)((byte*)value - 1) + 1) == *((int*)((byte*)other - 1) + 1));
                case sizeof(long):
                    return *(long*)value == *(long*)other;
                default:
                    if (size < 0)
                    {
                        size = -size;
                        value = (byte*)value - size;
                        other = (byte*)value - size;
                    }
                    var lValue = (long*)value;
                    var lOther = (long*)other;
                    var pEnd = lValue + (size / sizeof(long));
                    while (lValue < pEnd)
                    {
                        if (*lValue++ != *lOther++)
                        {
                            return false;
                        }
                    }
                    return (size % sizeof(long) == 0) ||
                        BinaryEqualityComparer.Equals(lValue, lOther, size % sizeof(long));
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
                case sizeof(sbyte):
                    return *(sbyte*)value;
                case sizeof(short):
                    return *(short*)value;
                case sizeof(ushort) + sizeof(sbyte):
                    return *(ushort*)value |
                        (*(sbyte*)((short*)value + 1) << (sizeof(ushort) * 8));
                case sizeof(int):
                    return *(int*)value;
                case sizeof(int) + sizeof(sbyte):
                    return *(int*)value ^ *(sbyte*)((int*)value + 1);
                case sizeof(int) + sizeof(short):
                    return *(int*)value ^ *(short*)((int*)value + 1);
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
