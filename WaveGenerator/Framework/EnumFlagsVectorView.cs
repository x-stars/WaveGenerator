using System;
using System.Runtime.CompilerServices;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 提供位域枚举的向量视图的基类。
    /// </summary>
    /// <typeparam name="TEnum">位域枚举的类型。</typeparam>
    [Serializable]
    public class EnumFlagsVectorView<TEnum> : EnumViewBase<TEnum>
        where TEnum : struct, Enum
    {
        /// <summary>
        /// 初始化 <see cref="EnumFlagsVectorView{TEnum}"/> 类的新实例。
        /// </summary>
        public EnumFlagsVectorView() { }

        /// <summary>
        /// 获取或设置当前视图表示的枚举值是否包含指定的枚举位域。
        /// </summary>
        /// <param name="flagValue">要获取或设置的枚举位域。</param>
        /// <returns>若当前视图表示的枚举值包含指定的枚举位域，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public bool this[TEnum flagValue]
        {
            get => this.Value.HasFlag(flagValue);
            set => this.SetFlag(flagValue, value);
        }

        /// <summary>
        /// 获取当前视图表示的枚举值是否包含指定的枚举位域。
        /// </summary>
        /// <param name="flagName">要确定的枚举位域的名称。</param>
        /// <returns>若当前视图表示的枚举值包含指定的枚举位域值，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="flagName"/> 不为有效的枚举值名称。</exception>
        protected bool HasFlag(
            [CallerMemberName] string flagName = null)
        {
            flagName = flagName ?? string.Empty;
            var flagValue = this.ParseEnum(flagName);
            return this[flagValue];
        }

        /// <summary>
        /// 根据指示设置当前视图表示的枚举值的指定的枚举位域。
        /// </summary>
        /// <param name="value">指示是添加位域还是移除位域。</param>
        /// <param name="flagName">要设置的枚举位域的名称。</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="flagName"/> 不为有效的枚举值名称。</exception>
        protected void SetFlag(bool value,
            [CallerMemberName] string flagName = null)
        {
            flagName = flagName ?? string.Empty;
            var flagValue = this.ParseEnum(flagName);
            this[flagValue] = value;
        }

        /// <summary>
        /// 根据指示设置当前视图表示的枚举值的指定的枚举位域。
        /// </summary>
        /// <param name="flagValue">要设置的枚举位域。</param>
        /// <param name="value">指示是否设置枚举值。</param>
        protected virtual void SetFlag(TEnum flagValue, bool value)
        {
            var iValue = ((IConvertible)this.Value).ToUInt64(null);
            var iFlag = ((IConvertible)flagValue).ToUInt64(null);
            if (value) { iValue |= iFlag; } else { iValue &= ~iFlag; }
            this.Value = (TEnum)Enum.ToObject(typeof(TEnum), iValue);
        }
    }
}
