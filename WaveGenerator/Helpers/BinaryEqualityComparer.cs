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
                case 1:
                    return *(byte*)value == *(byte*)other;
                case 2:
                    return *(ushort*)value == *(ushort*)other;
                case 3:
                    var sValue = (ushort*)value;
                    var sOther = (ushort*)other;
                    return (*sValue++ == *sOther++) &&
                        BinaryEqualityComparer.Equals(sValue, sOther, size - 2);
                case 4:
                    return *(uint*)value == *(uint*)other;
                case 5:
                case 6:
                case 7:
                    var iValue = (uint*)value;
                    var iOther = (uint*)other;
                    return (*iValue++ == *iOther++) &&
                        BinaryEqualityComparer.Equals(iValue, iOther, size - 4);
                case 8:
                    return *(ulong*)value == *(ulong*)other;
                default:
                    var lValue = (ulong*)value;
                    var lOther = (ulong*)other;
                    var pEnd = lValue + (size / 8);
                    while (lValue < pEnd)
                    {
                        if (*lValue++ != *lOther++)
                        {
                            return false;
                        }
                    }
                    return (size % 8 == 0) ||
                        BinaryEqualityComparer.Equals(lValue, lOther, size % 8);
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
                case 1:
                    return *(byte*)value;
                case 2:
                    return *(ushort*)value;
                case 3:
                    var sValue = (ushort*)value;
                    return *sValue++ * -1521134295 +
                        BinaryEqualityComparer.GetHashCode(sValue, size - 2);
                case 4:
                    return *(int*)value;
                default:
                    var hashCode = 0;
                    var iValue = (int*)value;
                    var pEnd = iValue + (size / 4);
                    while (iValue < pEnd)
                    {
                        hashCode = hashCode * -1521134295 + *iValue++;
                    }
                    return (size % 4 == 0) ? hashCode : (hashCode * -1521134295 +
                        BinaryEqualityComparer.GetHashCode(iValue, size % 4));
            }
        }
    }
}
