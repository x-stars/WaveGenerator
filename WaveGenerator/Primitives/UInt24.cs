using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace XstarS
{
    /// <summary>
    /// 表示 24 位无符号整数。
    /// </summary>
    [Serializable]
    [CLSCompliant(false)]
    [StructLayout(LayoutKind.Sequential, Size = 3)]
    public readonly struct UInt24 : IComparable, IComparable<UInt24>, IEquatable<UInt24>, IFormattable, IConvertible
    {
        /// <summary>
        /// 表示 <see cref="UInt24"/> 的可能最大值。
        /// </summary>
        public static readonly UInt24 MaxValue = UInt24.FromBytes(0x00FFFFFF);

        /// <summary>
        /// 表示 <see cref="UInt24"/> 的可能最小值。
        /// </summary>
        public static readonly UInt24 MinValue = UInt24.FromBytes(0x00000000);

        /// <summary>
        /// 表示当前 <see cref="UInt24"/> 低 16 位的字节值。
        /// </summary>
        private readonly ushort LowWord;

        /// <summary>
        /// 表示当前 <see cref="UInt24"/> 高 8 位的字节值。
        /// </summary>
        private readonly byte HighByte;

        /// <summary>
        /// 将 <see cref="UInt24"/> 结构的新实例初始化为指定 32 位无符号整数表示的值。
        /// </summary>
        /// <param name="value">作为值的 32 位无符号整数。</param>
        /// <exception cref="OverflowException">
        /// <paramref name="value"/> 的值超出 <see cref="UInt24"/> 的表示范围。</exception>
        [CLSCompliant(false)]
        public UInt24(uint value)
        {
            if (value > UInt24.MaxValue)
            {
                throw new OverflowException();
            }
            this.LowWord = (ushort)value;
            this.HighByte = (byte)(value >> 16);
        }

        /// <summary>
        /// 将当前 <see cref="UInt24"/> 转换为等效的 32 位无符号整数。
        /// </summary>
        /// <returns>转换得到的 32 位无符号整数。</returns>
        [CLSCompliant(false)]
        public uint ToUInt32()
        {
            var result = 0U;
            result |= (uint)this.LowWord;
            result |= (uint)this.HighByte << 16;
            return result;
        }

        /// <summary>
        /// 由指定的 32 位字节创建 <see cref="UInt24"/> 结构的实例。
        /// </summary>
        /// <param name="dword">作为字节的 32 位无符号整数。</param>
        /// <returns>由 <paramref name="dword"/> 创建的 <see cref="UInt24"/> 结构的实例。</returns>
        [CLSCompliant(false)]
        public static unsafe UInt24 FromBytes(uint dword) => *(UInt24*)&dword;

        /// <summary>
        /// 将数字的字符串表示形式转换为它的等效 24 位无符号整数。
        /// </summary>
        /// <param name="s">包含要转换的数字的字符串。</param>
        /// <returns>与 <paramref name="s"/> 中包含的数字等效的 24 位无符号整数。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="s"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="FormatException"><paramref name="s"/> 的格式不正确。</exception>
        /// <exception cref="OverflowException"><paramref name="s"/> 表示一个小于 <see cref="UInt24.MinValue"/>
        /// 或大于 <see cref="UInt24.MaxValue"/> 的数字，或 <paramref name="s"/> 包含非零的小数位。</exception>
        public static UInt24 Parse(string s) => UInt24.Parse(s, NumberStyles.Integer, null);

        /// <summary>
        /// 将指定样式的数字的字符串表示形式转换为它的等效 24 位无符号整数。
        /// </summary>
        /// <param name="s">包含要转换的数字的字符串。</param>
        /// <param name="style">指示数字字符串参数中允许的样式。</param>
        /// <returns>与 <paramref name="s"/> 中包含的数字等效的 24 位无符号整数。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="s"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentException"><paramref name="style"/> 不是 <see cref="NumberStyles"/> 值。</exception>
        /// <exception cref="FormatException"><paramref name="s"/> 的格式不符合 <paramref name="style"/>。</exception>
        /// <exception cref="OverflowException"><paramref name="s"/> 表示一个小于 <see cref="UInt24.MinValue"/>
        /// 或大于 <see cref="UInt24.MaxValue"/> 的数字，或 <paramref name="s"/> 包含非零的小数位。</exception>
        public static UInt24 Parse(string s, NumberStyles style) => UInt24.Parse(s, style, null);

        /// <summary>
        /// 将指定的区域性特定格式的数字的字符串表示形式转换为它的等效 24 位无符号整数。
        /// </summary>
        /// <param name="s">包含要转换的数字的字符串。</param>
        /// <param name="provider">一个提供区域性特定的格式设置信息的对象。</param>
        /// <returns>与 <paramref name="s"/> 中指定的数字等效的 24 位带符号整数。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="s"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="FormatException"><paramref name="s"/> 的格式不正确。</exception>
        /// <exception cref="OverflowException"><paramref name="s"/> 表示一个小于 <see cref="UInt24.MinValue"/>
        /// 或大于 <see cref="UInt24.MaxValue"/> 的数字，或 <paramref name="s"/> 包含非零的小数位。</exception>
        public static UInt24 Parse(string s, IFormatProvider provider) => UInt24.Parse(s, NumberStyles.Integer, provider);

        /// <summary>
        /// 将指定样式和区域性特定格式的数字的字符串表示形式转换为它的等效 24 位无符号整数。
        /// </summary>
        /// <param name="s">包含要转换的数字的字符串。</param>
        /// <param name="style">指示数字字符串参数中允许的样式。</param>
        /// <param name="provider">一个提供区域性特定的格式设置信息的对象。</param>
        /// <returns>与 <paramref name="s"/> 中指定的数字等效的 24 位带符号整数。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="s"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentException"><paramref name="style"/> 不是 <see cref="NumberStyles"/> 值。</exception>
        /// <exception cref="FormatException"><paramref name="s"/> 的格式不符合 <paramref name="style"/>。</exception>
        /// <exception cref="OverflowException"><paramref name="s"/> 表示一个小于 <see cref="UInt24.MinValue"/>
        /// 或大于 <see cref="UInt24.MaxValue"/> 的数字，或 <paramref name="s"/> 包含非零的小数位。</exception>
        public static UInt24 Parse(string s, NumberStyles style, IFormatProvider provider)
        {
            var value = uint.Parse(s, style, provider);
            return new UInt24(value);
        }

        /// <summary>
        /// 将数字的字符串表示形式转换为它的等效 24 位无符号整数。并返回转换是否成功。
        /// </summary>
        /// <param name="s">包含要转换的数字的字符串。</param>
        /// <param name="result">如果转换成功，则包含与 <paramref name="s"/>
        /// 中所包含的数字等效的 24 位无符号整数值；如果转换失败，则包含零。</param>
        /// <returns>如果 <paramref name="s"/> 成功转换，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public static bool TryParse(string s, out UInt24 result) =>
            UInt24.TryParse(s, NumberStyles.Any, null, out result);

        /// <summary>
        /// 将指定样式和区域性特定格式的数字的字符串表示形式转换为它的等效 24 位无符号整数。并返回转换是否成功。
        /// </summary>
        /// <param name="s">包含要转换的数字的字符串。</param>
        /// <param name="style">指示数字字符串参数中允许的样式。</param>
        /// <param name="provider">一个提供区域性特定的格式设置信息的对象。</param>
        /// <param name="result">如果转换成功，则包含与 <paramref name="s"/>
        /// 中所包含的数字等效的 24 位无符号整数值；如果转换失败，则包含零。</param>
        /// <returns>如果 <paramref name="s"/> 成功转换，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out UInt24 result)
        {
            try { result = UInt24.Parse(s, style, provider); return true; }
            catch (Exception) { result = default(UInt24); return false; }
        }

        /// <summary>
        /// 将此实例与指定对象进行比较并返回一个对二者的相对值的指示。
        /// </summary>
        /// <param name="obj">要比较的对象，或为 <see langword="null"/>。</param>
        /// <returns>若此实例小于 <paramref name="obj"/>，则小于零；若此实例大于
        /// <paramref name="obj"/>，则大于零；若此实例等于 <paramref name="obj"/>，则为零。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="obj"/> 不为 <see cref="UInt24"/>。</exception>
        public int CompareTo(object obj) => (obj is null) ? this.CompareTo(default(UInt24)) :
            (obj is UInt24 other) ? this.CompareTo(other) : throw new ArgumentException();

        /// <summary>
        /// 将此实例与指定的 <see cref="UInt24"/> 值进行比较并返回对其相对值的指示。
        /// </summary>
        /// <param name="other">要进行比较的 <see cref="UInt24"/> 值。</param>
        /// <returns>若此实例小于 <paramref name="other"/>，则小于零；若此实例大于
        /// <paramref name="other"/>，则大于零；若此实例等于 <paramref name="other"/>，则为零。</returns>
        public int CompareTo(UInt24 other) => this.ToUInt32().CompareTo(other.ToUInt32());

        /// <summary>
        /// 确定此实例是否等于指定的 <see cref="UInt24"/> 值。
        /// </summary>
        /// <param name="other">要进行比较的 <see cref="UInt24"/> 值。</param>
        /// <returns>若 <paramref name="other"/> 与此实例相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public bool Equals(UInt24 other) => this.ToUInt32() == other.ToUInt32();

        /// <summary>
        /// 确定此实例是否等于指定的对象。
        /// </summary>
        /// <param name="obj">要进行比较的对象。</param>
        /// <returns>若 <paramref name="obj"/> 为 <see cref="UInt24"/> 且与此实例相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public override bool Equals(object obj) => (obj is UInt24 other) && this.Equals(other);

        /// <summary>
        /// 返回此实例的哈希代码。
        /// </summary>
        /// <returns>此实例的 32 位有符号整数哈希代码。</returns>
        public override int GetHashCode() => this.ToUInt32().GetHashCode();

        /// <summary>
        /// 获取 <see cref="UInt24"/> 类型的 <see cref="TypeCode"/>。
        /// </summary>
        /// <returns>枚举常数 <see cref="TypeCode.Object"/>。</returns>
        public TypeCode GetTypeCode() => TypeCode.Object;

        /// <summary>
        /// 将此实例的值转换为其等效的字符串表示形式。
        /// </summary>
        /// <returns>此实例的值的字符串表示形式。</returns>
        public override string ToString() => this.ToString(null, null);

        /// <summary>
        /// 使用指定的格式，将此实例的值转换为它的等效字符串表示形式。
        /// </summary>
        /// <param name="format">标准或自定义的数值格式字符串。</param>
        /// <returns>此实例的值的字符串表示形式，由 <paramref name="format"/> 指定。</returns>
        /// <exception cref="FormatException"><paramref name="format"/> 无效或不受支持。</exception>
        public string ToString(string format) => this.ToString(format, null);

        /// <summary>
        /// 使用指定的区域性特定格式信息，将此实例的数值转换为它的等效字符串表示形式。
        /// </summary>
        /// <param name="provider">一个提供区域性特定的格式设置信息的对象。</param>
        /// <returns>此实例的值的字符串表示形式，由 <paramref name="provider"/> 指定。</returns>
        public string ToString(IFormatProvider provider) => this.ToString(null, provider);

        /// <summary>
        /// 使用指定的格式和区域性特定格式信息，将此实例的值转换为它的等效字符串表示形式。
        /// </summary>
        /// <param name="format">标准或自定义的数值格式字符串。</param>
        /// <param name="provider">一个提供区域性特定的格式设置信息的对象。</param>
        /// <returns>此实例的值的字符串表示形式，由 <paramref name="format"/>
        /// 和 <paramref name="provider"/> 指定。</returns>
        /// <exception cref="FormatException"><paramref name="format"/> 无效或不受支持。</exception>
        public string ToString(string format, IFormatProvider provider) => this.ToUInt32().ToString(format, provider);

        /// <summary>
        /// 使用指定的区域性特定格式设置信息将此实例的值转换为等效的 <see cref="bool"/> 值。
        /// </summary>
        /// <param name="provider">一个提供区域性特定的格式设置信息的对象。</param>
        /// <returns>与此实例的值等效的 <see cref="bool"/> 值。</returns>
        bool IConvertible.ToBoolean(IFormatProvider provider) => ((IConvertible)(uint)this).ToBoolean(provider);

        /// <summary>
        /// 使用指定的区域性特定格式设置信息将此实例的值转换为等效的 <see cref="char"/> 值。
        /// </summary>
        /// <param name="provider">一个提供区域性特定的格式设置信息的对象。</param>
        /// <returns>与此实例的值等效的 <see cref="char"/> 值。</returns>
        char IConvertible.ToChar(IFormatProvider provider) => ((IConvertible)(uint)this).ToChar(provider);

        /// <summary>
        /// 使用指定的区域性特定格式设置信息将此实例的值转换为等效的 <see cref="sbyte"/> 值。
        /// </summary>
        /// <param name="provider">一个提供区域性特定的格式设置信息的对象。</param>
        /// <returns>与此实例的值等效的 <see cref="sbyte"/> 值。</returns>
        sbyte IConvertible.ToSByte(IFormatProvider provider) => ((IConvertible)(uint)this).ToSByte(provider);

        /// <summary>
        /// 使用指定的区域性特定格式设置信息将此实例的值转换为等效的 <see cref="byte"/> 值。
        /// </summary>
        /// <param name="provider">一个提供区域性特定的格式设置信息的对象。</param>
        /// <returns>与此实例的值等效的 <see cref="byte"/> 值。</returns>
        byte IConvertible.ToByte(IFormatProvider provider) => ((IConvertible)(uint)this).ToByte(provider);

        /// <summary>
        /// 使用指定的区域性特定格式设置信息将此实例的值转换为等效的 <see cref="short"/> 值。
        /// </summary>
        /// <param name="provider">一个提供区域性特定的格式设置信息的对象。</param>
        /// <returns>与此实例的值等效的 <see cref="short"/> 值。</returns>
        short IConvertible.ToInt16(IFormatProvider provider) => ((IConvertible)(uint)this).ToInt16(provider);

        /// <summary>
        /// 使用指定的区域性特定格式设置信息将此实例的值转换为等效的 <see cref="ushort"/> 值。
        /// </summary>
        /// <param name="provider">一个提供区域性特定的格式设置信息的对象。</param>
        /// <returns>与此实例的值等效的 <see cref="ushort"/> 值。</returns>
        ushort IConvertible.ToUInt16(IFormatProvider provider) => ((IConvertible)(uint)this).ToUInt16(provider);

        /// <summary>
        /// 使用指定的区域性特定格式设置信息将此实例的值转换为等效的 <see cref="int"/> 值。
        /// </summary>
        /// <param name="provider">一个提供区域性特定的格式设置信息的对象。</param>
        /// <returns>与此实例的值等效的 <see cref="int"/> 值。</returns>
        int IConvertible.ToInt32(IFormatProvider provider) => ((IConvertible)(uint)this).ToInt32(provider);

        /// <summary>
        /// 使用指定的区域性特定格式设置信息将此实例的值转换为等效的 <see cref="uint"/> 值。
        /// </summary>
        /// <param name="provider">一个提供区域性特定的格式设置信息的对象。</param>
        /// <returns>与此实例的值等效的 <see cref="uint"/> 值。</returns>
        uint IConvertible.ToUInt32(IFormatProvider provider) => ((IConvertible)(uint)this).ToUInt32(provider);

        /// <summary>
        /// 使用指定的区域性特定格式设置信息将此实例的值转换为等效的 <see cref="long"/> 值。
        /// </summary>
        /// <param name="provider">一个提供区域性特定的格式设置信息的对象。</param>
        /// <returns>与此实例的值等效的 <see cref="long"/> 值。</returns>
        long IConvertible.ToInt64(IFormatProvider provider) => ((IConvertible)(uint)this).ToInt64(provider);

        /// <summary>
        /// 使用指定的区域性特定格式设置信息将此实例的值转换为等效的 <see cref="ulong"/> 值。
        /// </summary>
        /// <param name="provider">一个提供区域性特定的格式设置信息的对象。</param>
        /// <returns>与此实例的值等效的 <see cref="ulong"/> 值。</returns>
        ulong IConvertible.ToUInt64(IFormatProvider provider) => ((IConvertible)(uint)this).ToUInt64(provider);

        /// <summary>
        /// 使用指定的区域性特定格式设置信息将此实例的值转换为等效的 <see cref="float"/> 值。
        /// </summary>
        /// <param name="provider">一个提供区域性特定的格式设置信息的对象。</param>
        /// <returns>与此实例的值等效的 <see cref="float"/> 值。</returns>
        float IConvertible.ToSingle(IFormatProvider provider) => ((IConvertible)(uint)this).ToSingle(provider);

        /// <summary>
        /// 使用指定的区域性特定格式设置信息将此实例的值转换为等效的 <see cref="double"/> 值。
        /// </summary>
        /// <param name="provider">一个提供区域性特定的格式设置信息的对象。</param>
        /// <returns>与此实例的值等效的 <see cref="double"/> 值。</returns>
        double IConvertible.ToDouble(IFormatProvider provider) => ((IConvertible)(uint)this).ToDouble(provider);

        /// <summary>
        /// 使用指定的区域性特定格式设置信息将此实例的值转换为等效的 <see cref="decimal"/> 值。
        /// </summary>
        /// <param name="provider">一个提供区域性特定的格式设置信息的对象。</param>
        /// <returns>与此实例的值等效的 <see cref="decimal"/> 值。</returns>
        decimal IConvertible.ToDecimal(IFormatProvider provider) => ((IConvertible)(uint)this).ToDecimal(provider);

        /// <summary>
        /// 使用指定的区域性特定格式设置信息将此实例的值转换为等效的 <see cref="DateTime"/> 值。
        /// </summary>
        /// <param name="provider">一个提供区域性特定的格式设置信息的对象。</param>
        /// <returns>与此实例的值等效的 <see cref="DateTime"/> 值。</returns>
        DateTime IConvertible.ToDateTime(IFormatProvider provider) => ((IConvertible)(uint)this).ToDateTime(provider);

        /// <summary>
        /// 使用指定的区域性特定格式设置信息将此实例的值转换为具有等效值的指定类型的对象。
        /// </summary>
        /// <param name="conversionType">要将此实例的值转换为的类型。</param>
        /// <param name="provider">一个提供区域性特定的格式设置信息的对象。</param>
        /// <returns>其值与此实例值等效的 <paramref name="conversionType"/> 类型的对象。</returns>
        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
        {
            return ((IConvertible)(uint)this).ToType(conversionType, provider);
        }

        /// <summary>
        /// 将指定的 <see cref="UInt24"/> 值取正。
        /// </summary>
        /// <param name="value">要取正的 <see cref="UInt24"/> 值。</param>
        /// <returns><paramref name="value"/> 取正后的值。</returns>
        public static UInt24 operator +(UInt24 value) => value;

        /// <summary>
        /// 将指定的 <see cref="UInt24"/> 值取反。
        /// </summary>
        /// <param name="value">要取反的 <see cref="UInt24"/> 值。</param>
        /// <returns><paramref name="value"/> 取反后的值。</returns>
        /// <exception cref="OverflowException">返回值超出 <see cref="UInt24"/> 的表示范围。</exception>
        public static UInt24 operator -(UInt24 value) => (UInt24)(-(uint)value);

        /// <summary>
        /// 将指定的 <see cref="UInt24"/> 值加一。
        /// </summary>
        /// <param name="value">要加一的 <see cref="UInt24"/> 值。</param>
        /// <returns><paramref name="value"/> 加一后的值。</returns>
        /// <exception cref="OverflowException">返回值超出 <see cref="UInt24"/> 的表示范围。</exception>
        public static UInt24 operator ++(UInt24 value) => (UInt24)((uint)value + 1);

        /// <summary>
        /// 将指定的 <see cref="UInt24"/> 值减一。
        /// </summary>
        /// <param name="value">要减一的 <see cref="UInt24"/> 值。</param>
        /// <returns><paramref name="value"/> 减一后的值。</returns>
        /// <exception cref="OverflowException">返回值超出 <see cref="UInt24"/> 的表示范围。</exception>
        public static UInt24 operator --(UInt24 value) => (UInt24)((uint)value - 1);

        /// <summary>
        /// 将两个指定的 <see cref="UInt24"/> 值相加。
        /// </summary>
        /// <param name="left">作为被加数的 <see cref="UInt24"/> 值。</param>
        /// <param name="right">作为加数的 <see cref="UInt24"/> 值。</param>
        /// <returns><paramref name="left"/> 加上 <paramref name="right"/> 后的值。</returns>
        /// <exception cref="OverflowException">返回值超出 <see cref="UInt24"/> 的表示范围。</exception>
        public static UInt24 operator +(UInt24 left, UInt24 right) => (UInt24)((uint)left + (uint)right);

        /// <summary>
        /// 将两个指定的 <see cref="UInt24"/> 值相减。
        /// </summary>
        /// <param name="left">作为被减数的 <see cref="UInt24"/> 值。</param>
        /// <param name="right">作为减数的 <see cref="UInt24"/> 值。</param>
        /// <returns><paramref name="left"/> 减去 <paramref name="right"/> 后的值。</returns>
        /// <exception cref="OverflowException">返回值超出 <see cref="UInt24"/> 的表示范围。</exception>
        public static UInt24 operator -(UInt24 left, UInt24 right) => (UInt24)((uint)left - (uint)right);

        /// <summary>
        /// 将两个指定的 <see cref="UInt24"/> 值相乘。
        /// </summary>
        /// <param name="left">作为被乘数的 <see cref="UInt24"/> 值。</param>
        /// <param name="right">作为乘数的 <see cref="UInt24"/> 值。</param>
        /// <returns><paramref name="left"/> 乘上 <paramref name="right"/> 后的值。</returns>
        /// <exception cref="OverflowException">返回值超出 <see cref="UInt24"/> 的表示范围。</exception>
        public static UInt24 operator *(UInt24 left, UInt24 right) => (UInt24)((uint)left * (uint)right);

        /// <summary>
        /// 将两个指定的 <see cref="UInt24"/> 值相除。
        /// </summary>
        /// <param name="left">作为被除数的 <see cref="UInt24"/> 值。</param>
        /// <param name="right">作为除数的 <see cref="UInt24"/> 值。</param>
        /// <returns><paramref name="left"/> 除以 <paramref name="right"/> 后的值。</returns>
        /// <exception cref="DivideByZeroException"><paramref name="right"/> 为零。</exception>
        /// <exception cref="OverflowException">返回值超出 <see cref="UInt24"/> 的表示范围。</exception>
        public static UInt24 operator /(UInt24 left, UInt24 right) => (UInt24)((uint)left / (uint)right);

        /// <summary>
        /// 将两个指定的 <see cref="UInt24"/> 值相除，并取得余数。
        /// </summary>
        /// <param name="left">作为被除数的 <see cref="UInt24"/> 值。</param>
        /// <param name="right">作为除数的 <see cref="UInt24"/> 值。</param>
        /// <returns><paramref name="left"/> 除以 <paramref name="right"/> 后的余数。</returns>
        /// <exception cref="DivideByZeroException"><paramref name="right"/> 为零。</exception>
        /// <exception cref="OverflowException">返回值超出 <see cref="UInt24"/> 的表示范围。</exception>
        public static UInt24 operator %(UInt24 left, UInt24 right) => (UInt24)((uint)left % (uint)right);

        /// <summary>
        /// 比较两个指定的 <see cref="UInt24"/> 值是否相等。
        /// </summary>
        /// <param name="left">要比较的第一个 <see cref="UInt24"/> 值。</param>
        /// <param name="right">要比较的第二个 <see cref="UInt24"/> 值。</param>
        /// <returns>若 <paramref name="left"/> 等于 <paramref name="right"/>，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public static bool operator ==(UInt24 left, UInt24 right) => (uint)left == (uint)right;

        /// <summary>
        /// 比较两个指定的 <see cref="UInt24"/> 值是否不等。
        /// </summary>
        /// <param name="left">要比较的第一个 <see cref="UInt24"/> 值。</param>
        /// <param name="right">要比较的第二个 <see cref="UInt24"/> 值。</param>
        /// <returns>若 <paramref name="left"/> 不等于 <paramref name="right"/>，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public static bool operator !=(UInt24 left, UInt24 right) => (uint)left != (uint)right;

        /// <summary>
        /// 比较指定的 <see cref="UInt24"/> 值是否小于另一个 <see cref="UInt24"/> 值。
        /// </summary>
        /// <param name="left">要比较的第一个 <see cref="UInt24"/> 值。</param>
        /// <param name="right">要比较的第二个 <see cref="UInt24"/> 值。</param>
        /// <returns>若 <paramref name="left"/> 小于 <paramref name="right"/>，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public static bool operator <(UInt24 left, UInt24 right) => (uint)left < (uint)right;

        /// <summary>
        /// 比较指定的 <see cref="UInt24"/> 值是否大于另一个 <see cref="UInt24"/> 值。
        /// </summary>
        /// <param name="left">要比较的第一个 <see cref="UInt24"/> 值。</param>
        /// <param name="right">要比较的第二个 <see cref="UInt24"/> 值。</param>
        /// <returns>若 <paramref name="left"/> 大于 <paramref name="right"/>，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public static bool operator >(UInt24 left, UInt24 right) => (uint)left > (uint)right;

        /// <summary>
        /// 比较指定的 <see cref="UInt24"/> 值是否小于等于另一个 <see cref="UInt24"/> 值。
        /// </summary>
        /// <param name="left">要比较的第一个 <see cref="UInt24"/> 值。</param>
        /// <param name="right">要比较的第二个 <see cref="UInt24"/> 值。</param>
        /// <returns>若 <paramref name="left"/> 小于等于 <paramref name="right"/>，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public static bool operator <=(UInt24 left, UInt24 right) => (uint)left <= (uint)right;

        /// <summary>
        /// 比较指定的 <see cref="UInt24"/> 值是否大于等于另一个 <see cref="UInt24"/> 值。
        /// </summary>
        /// <param name="left">要比较的第一个 <see cref="UInt24"/> 值。</param>
        /// <param name="right">要比较的第二个 <see cref="UInt24"/> 值。</param>
        /// <returns>若 <paramref name="left"/> 大于等于 <paramref name="right"/>，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public static bool operator >=(UInt24 left, UInt24 right) => (uint)left >= (uint)right;

        /// <summary>
        /// 将指定的 8 位无符号整数隐式转换为 <see cref="UInt24"/> 值。
        /// </summary>
        /// <param name="value">要转换的 8 位无符号整数。</param>
        /// <returns>转换后的 <see cref="UInt24"/> 值。</returns>
        public static implicit operator UInt24(byte value) => (UInt24)(uint)value;

        /// <summary>
        /// 将指定的 8 位有符号整数隐式转换为 <see cref="UInt24"/> 值。
        /// </summary>
        /// <param name="value">要转换的 8 位有符号整数。</param>
        /// <returns>转换后的 <see cref="UInt24"/> 值。</returns>
        [CLSCompliant(false)]
        public static implicit operator UInt24(sbyte value) => (UInt24)(uint)value;

        /// <summary>
        /// 将指定的 UTF-16 字符隐式转换为 <see cref="UInt24"/> 值。
        /// </summary>
        /// <param name="value">要转换的 UTF-16 字符。</param>
        /// <returns>转换后的 <see cref="UInt24"/> 值。</returns>
        public static implicit operator UInt24(char value) => (UInt24)(uint)value;

        /// <summary>
        /// 将指定的 16 位有符号整数隐式转换为 <see cref="UInt24"/> 值。
        /// </summary>
        /// <param name="value">要转换的 16 位有符号整数。</param>
        /// <returns>转换后的 <see cref="UInt24"/> 值。</returns>
        public static implicit operator UInt24(short value) => (UInt24)(uint)value;

        /// <summary>
        /// 将指定的 16 位无符号整数隐式转换为 <see cref="UInt24"/> 值。
        /// </summary>
        /// <param name="value">要转换的 16 位无符号整数。</param>
        /// <returns>转换后的 <see cref="UInt24"/> 值。</returns>
        [CLSCompliant(false)]
        public static implicit operator UInt24(ushort value) => (UInt24)(uint)value;

        /// <summary>
        /// 将指定的 32 位有符号整数显式转换为 <see cref="UInt24"/> 值。
        /// </summary>
        /// <param name="value">要转换的 32 位有符号整数。</param>
        /// <returns>转换后的 <see cref="UInt24"/> 值。</returns>
        /// <exception cref="OverflowException">
        /// <paramref name="value"/> 的值超出 <see cref="UInt24"/> 的表示范围。</exception>
        public static explicit operator UInt24(int value) => (UInt24)(uint)value;

        /// <summary>
        /// 将指定的 32 位无符号整数显式转换为 <see cref="UInt24"/> 值。
        /// </summary>
        /// <param name="value">要转换的 32 位无符号整数。</param>
        /// <returns>转换后的 <see cref="UInt24"/> 值。</returns>
        /// <exception cref="OverflowException">
        /// <paramref name="value"/> 的值超出 <see cref="UInt24"/> 的表示范围。</exception>
        [CLSCompliant(false)]
        public static explicit operator UInt24(uint value) => new UInt24(value);

        /// <summary>
        /// 将指定的 64 位有符号整数显式转换为 <see cref="UInt24"/> 值。
        /// </summary>
        /// <param name="value">要转换的 64 位有符号整数。</param>
        /// <returns>转换后的 <see cref="UInt24"/> 值。</returns>
        /// <exception cref="OverflowException">
        /// <paramref name="value"/> 的值超出 <see cref="UInt24"/> 的表示范围。</exception>
        public static explicit operator UInt24(long value) => (UInt24)(uint)value;

        /// <summary>
        /// 将指定的 64 位无符号整数显式转换为 <see cref="UInt24"/> 值。
        /// </summary>
        /// <param name="value">要转换的 64 位无符号整数。</param>
        /// <returns>转换后的 <see cref="UInt24"/> 值。</returns>
        /// <exception cref="OverflowException">
        /// <paramref name="value"/> 的值超出 <see cref="UInt24"/> 的表示范围。</exception>
        [CLSCompliant(false)]
        public static explicit operator UInt24(ulong value) => (UInt24)(uint)value;

        /// <summary>
        /// 将指定的单精度浮点数显式转换为 <see cref="UInt24"/> 值。
        /// </summary>
        /// <param name="value">要转换的单精度浮点数。</param>
        /// <returns>转换后的 <see cref="UInt24"/> 值。</returns>
        /// <exception cref="OverflowException">
        /// <paramref name="value"/> 的值超出 <see cref="UInt24"/> 的表示范围。</exception>
        public static explicit operator UInt24(float value) => (UInt24)(uint)value;

        /// <summary>
        /// 将指定的双精度浮点数显式转换为 <see cref="UInt24"/> 值。
        /// </summary>
        /// <param name="value">要转换的双精度浮点数。</param>
        /// <returns>转换后的 <see cref="UInt24"/> 值。</returns>
        /// <exception cref="OverflowException">
        /// <paramref name="value"/> 的值超出 <see cref="UInt24"/> 的表示范围。</exception>
        public static explicit operator UInt24(double value) => (UInt24)(uint)value;

        /// <summary>
        /// 将指定的十进制浮点数显式转换为 <see cref="UInt24"/> 值。
        /// </summary>
        /// <param name="value">要转换的十进制浮点数。</param>
        /// <returns>转换后的 <see cref="UInt24"/> 值。</returns>
        /// <exception cref="OverflowException">
        /// <paramref name="value"/> 的值超出 <see cref="UInt24"/> 的表示范围。</exception>
        public static explicit operator UInt24(decimal value) => (UInt24)(uint)value;

        /// <summary>
        /// 将指定的 <see cref="UInt24"/> 值显式转换为 8 位无符号整数。
        /// </summary>
        /// <param name="value">要转换的 <see cref="UInt24"/> 值。</param>
        /// <returns>转换后的 8 位无符号整数。</returns>
        /// <exception cref="OverflowException">
        /// <paramref name="value"/> 的值超出 <see cref="byte"/> 的表示范围。</exception>
        public static explicit operator byte(UInt24 value) => (byte)(uint)value;

        /// <summary>
        /// 将指定的 <see cref="UInt24"/> 值显式转换为 8 位有符号整数。
        /// </summary>
        /// <param name="value">要转换的 <see cref="UInt24"/> 值。</param>
        /// <returns>转换后的 8 位有符号整数。</returns>
        /// <exception cref="OverflowException">
        /// <paramref name="value"/> 的值超出 <see cref="sbyte"/> 的表示范围。</exception>
        [CLSCompliant(false)]
        public static explicit operator sbyte(UInt24 value) => (sbyte)(uint)value;

        /// <summary>
        /// 将指定的 <see cref="UInt24"/> 值显式转换为 UTF-16 字符。
        /// </summary>
        /// <param name="value">要转换的 <see cref="UInt24"/> 值。</param>
        /// <returns>转换后的 UTF-16 字符。</returns>
        /// <exception cref="OverflowException">
        /// <paramref name="value"/> 的值超出 <see cref="char"/> 的表示范围。</exception>
        public static explicit operator char(UInt24 value) => (char)(uint)value;

        /// <summary>
        /// 将指定的 <see cref="UInt24"/> 值显式转换为 16 位有符号整数。
        /// </summary>
        /// <param name="value">要转换的 <see cref="UInt24"/> 值。</param>
        /// <returns>转换后的 16 位有符号整数。</returns>
        /// <exception cref="OverflowException">
        /// <paramref name="value"/> 的值超出 <see cref="short"/> 的表示范围。</exception>
        public static explicit operator short(UInt24 value) => (short)(uint)value;

        /// <summary>
        /// 将指定的 <see cref="UInt24"/> 值显式转换为 16 位无符号整数。
        /// </summary>
        /// <param name="value">要转换的 <see cref="UInt24"/> 值。</param>
        /// <returns>转换后的 16 位无符号整数。</returns>
        /// <exception cref="OverflowException">
        /// <paramref name="value"/> 的值超出 <see cref="ushort"/> 的表示范围。</exception>
        [CLSCompliant(false)]
        public static explicit operator ushort(UInt24 value) => (ushort)(uint)value;

        /// <summary>
        /// 将指定的 <see cref="UInt24"/> 值隐式转换为 32 位有符号整数。
        /// </summary>
        /// <param name="value">要转换的 <see cref="UInt24"/> 值。</param>
        /// <returns>转换后的 32 位有符号整数。</returns>
        public static implicit operator int(UInt24 value) => (int)(uint)value;

        /// <summary>
        /// 将指定的 <see cref="UInt24"/> 值隐式转换为 32 位无符号整数。
        /// </summary>
        /// <param name="value">要转换的 <see cref="UInt24"/> 值。</param>
        /// <returns>转换后的 32 位无符号整数。</returns>
        [CLSCompliant(false)]
        public static implicit operator uint(UInt24 value) => value.ToUInt32();

        /// <summary>
        /// 将指定的 <see cref="UInt24"/> 值隐式转换为 64 位有符号整数。
        /// </summary>
        /// <param name="value">要转换的 <see cref="UInt24"/> 值。</param>
        /// <returns>转换后的 64 位有符号整数。</returns>
        public static implicit operator long(UInt24 value) => (long)(uint)value;

        /// <summary>
        /// 将指定的 <see cref="UInt24"/> 值隐式转换为 64 位无符号整数。
        /// </summary>
        /// <param name="value">要转换的 <see cref="UInt24"/> 值。</param>
        /// <returns>转换后的 64 位无符号整数。</returns>
        [CLSCompliant(false)]
        public static implicit operator ulong(UInt24 value) => (ulong)(uint)value;

        /// <summary>
        /// 将指定的 <see cref="UInt24"/> 值隐式转换为单精度浮点数。
        /// </summary>
        /// <param name="value">要转换的 <see cref="UInt24"/> 值。</param>
        /// <returns>转换后的单精度浮点数。</returns>
        public static implicit operator float(UInt24 value) => (float)(uint)value;

        /// <summary>
        /// 将指定的 <see cref="UInt24"/> 值隐式转换为双精度浮点数。
        /// </summary>
        /// <param name="value">要转换的 <see cref="UInt24"/> 值。</param>
        /// <returns>转换后的双精度浮点数。</returns>
        public static implicit operator double(UInt24 value) => (double)(uint)value;

        /// <summary>
        /// 将指定的 <see cref="UInt24"/> 值隐式转换为十进制浮点数。
        /// </summary>
        /// <param name="value">要转换的 <see cref="UInt24"/> 值。</param>
        /// <returns>转换后的十进制浮点数。</returns>
        public static implicit operator decimal(UInt24 value) => (decimal)(uint)value;
    }
}
