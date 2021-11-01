using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Convertible = System.Convert;

namespace XstarS.Windows.Data
{
    /// <summary>
    /// 表示 <see cref="IConvertible"/> 数字到其对应的对数的转换器。
    /// </summary>
    [ValueConversion(typeof(IConvertible), typeof(IConvertible),
                     ParameterType = typeof(IConvertible))]
    public sealed class LogarithmConverter : IValueConverter
    {
        /// <summary>
        /// 初始化 <see cref="LogarithmConverter"/> 类的新实例。
        /// </summary>
        public LogarithmConverter() { }

        /// <summary>
        /// 将指定的 <see cref="IConvertible"/> 数字转换为其对应的对数。
        /// </summary>
        /// <param name="value">要转换的 <see cref="IConvertible"/> 数字。</param>
        /// <param name="targetType">绑定目标属性的类型。</param>
        /// <param name="parameter">表示对数的底的转换器参数。默认为常数 <see langword="e"/>。</param>
        /// <param name="culture">要用在转换器中的区域性。</param>
        /// <returns><paramref name="value"/> 的底为 <paramref name="parameter"/> 的对数。</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var number = Convertible.ToDouble(value ?? 0.0, culture);
                var @base = Convertible.ToDouble(parameter ?? Math.E, culture);
                var result = Math.Log(number, @base);
                return Convertible.ChangeType(result, targetType, culture);
            }
            catch (Exception) { return DependencyProperty.UnsetValue; }
        }

        /// <summary>
        /// 将指定的 <see cref="IConvertible"/> 数字转换为其对应的指数。
        /// </summary>
        /// <param name="value">要转换的 <see cref="IConvertible"/> 数字。</param>
        /// <param name="targetType">绑定目标属性的类型。</param>
        /// <param name="parameter">表示指数的底的转换器参数。默认为常数 <see langword="e"/>。</param>
        /// <param name="culture">要用在转换器中的区域性。</param>
        /// <returns><paramref name="value"/> 的底为 <paramref name="parameter"/> 的指数。</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var number = Convertible.ToDouble(value ?? 0.0, culture);
                var @base = Convertible.ToDouble(parameter ?? Math.E, culture);
                var result = Math.Pow(@base, number);
                return Convertible.ChangeType(result, targetType, culture);
            }
            catch (Exception) { return DependencyProperty.UnsetValue; }
        }
    }
}
