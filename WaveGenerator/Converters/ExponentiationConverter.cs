using System;
using System.Globalization;
using System.Windows.Data;

namespace XstarS.Windows.Data
{
    /// <summary>
    /// 表示 <see cref="IConvertible"/> 数字到其对应的指数的转换器。
    /// </summary>
    [ValueConversion(typeof(IConvertible), typeof(IConvertible),
        ParameterType = typeof(IConvertible))]
    public sealed class ExponentiationConverter : IValueConverter
    {
        /// <summary>
        /// 初始化 <see cref="ExponentiationConverter"/> 类的新实例。
        /// </summary>
        public ExponentiationConverter() { }

        /// <summary>
        /// 将指定的 <see cref="IConvertible"/> 数字转换为其对应的指数。
        /// </summary>
        /// <param name="value">要转换的 <see cref="IConvertible"/> 数字。</param>
        /// <param name="targetType">绑定目标属性的类型。</param>
        /// <param name="parameter">表示指数的底的转换器参数。默认为常数 <see langword="e"/>。</param>
        /// <param name="culture">要用在转换器中的区域性。</param>
        /// <returns><paramref name="value"/> 的底为 <paramref name="parameter"/> 的指数。</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var number = ((value as IConvertible) ?? 0.0).ToDouble(culture);
            var @base = ((parameter as IConvertible) ?? Math.E).ToDouble(culture);
            var result = Math.Pow(@base, number);
            return ((IConvertible)result).ToType(targetType, culture);
        }

        /// <summary>
        /// 将指定的 <see cref="IConvertible"/> 数字转换为其对应的对数。
        /// </summary>
        /// <param name="value">要转换的 <see cref="IConvertible"/> 数字。</param>
        /// <param name="targetType">绑定目标属性的类型。</param>
        /// <param name="parameter">表示对数的底的转换器参数。默认为常数 <see langword="e"/>。</param>
        /// <param name="culture">要用在转换器中的区域性。</param>
        /// <returns><paramref name="value"/> 的底为 <paramref name="parameter"/> 的对数。</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var number = ((value as IConvertible) ?? 0.0).ToDouble(culture);
            var @base = ((parameter as IConvertible) ?? Math.E).ToDouble(culture);
            var result = Math.Log(number, @base);
            return ((IConvertible)result).ToType(targetType, culture);
        }
    }
}
