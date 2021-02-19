using System;
using System.Runtime.CompilerServices;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 提供枚举的向量视图的基类。
    /// </summary>
    /// <typeparam name="TEnum">枚举的类型。</typeparam>
    [Serializable]
    public class EnumVectorView<TEnum> : EnumViewBase<TEnum>
        where TEnum : struct, Enum
    {
        /// <summary>
        /// 初始化 <see cref="EnumVectorView{TEnum}"/> 类的新实例。
        /// </summary>
        public EnumVectorView() { }

        /// <summary>
        /// 获取或设置当前视图表示的枚举值是否为指定的枚举值。
        /// </summary>
        /// <param name="enumValue">要获取或设置的枚举值。</param>
        /// <returns>若当前视图表示的枚举值为指定的枚举值，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public bool this[TEnum enumValue]
        {
            get => this.Value.Equals(enumValue);
            set => this.SetEnum(enumValue, value);
        }

        /// <summary>
        /// 获取当前视图表示的枚举值是否为指定的枚举值。
        /// </summary>
        /// <param name="enumName">要确定的枚举值的名称。</param>
        /// <returns>若当前视图表示的枚举值为指定的枚举值，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="enumName"/> 不为有效的枚举值名称。</exception>
        protected bool IsEnum(
            [CallerMemberName] string enumName = null)
        {
            enumName = enumName ?? string.Empty;
            var enumValue = this.ParseEnum(enumName);
            return this[enumValue];
        }

        /// <summary>
        /// 根据指示设置当前视图表示的枚举值。
        /// </summary>
        /// <param name="value">指示是否设置枚举值。</param>
        /// <param name="enumName">要设置的枚举值的名称。</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="enumName"/> 不为有效的枚举值名称。</exception>
        protected void SetEnum(bool value,
            [CallerMemberName] string enumName = null)
        {
            enumName = enumName ?? string.Empty;
            var enumValue = this.ParseEnum(enumName);
            this[enumValue] = value;
        }

        /// <summary>
        /// 根据指示设置当前视图表示的枚举值。
        /// </summary>
        /// <param name="enumValue">要设置的枚举值。</param>
        /// <param name="value">指示是否设置枚举值。</param>
        protected virtual void SetEnum(TEnum enumValue, bool value)
        {
            if (value) { this.Value = enumValue; }
        }
    }
}
