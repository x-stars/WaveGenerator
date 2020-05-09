using System;
using System.Runtime.CompilerServices;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 提供位域枚举的向量视图的基类。
    /// </summary>
    /// <typeparam name="TEnum">位域枚举的类型。</typeparam>
    [Serializable]
    public class EnumFlagsVectorView<TEnum> : ObservableDataObject
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
            set
            {
                var iValue = ((IConvertible)this.Value).ToUInt64(null);
                var iFlag = ((IConvertible)flagValue).ToUInt64(null);
                if (value) { iValue |= iFlag; } else { iValue &= ~iFlag; }
                this.Value = (TEnum)Enum.ToObject(typeof(TEnum), iValue);
            }
        }

        /// <summary>
        /// 获取或设置当前视图表示的枚举值。
        /// </summary>
        public TEnum Value
        {
            get => this.GetProperty<TEnum>();
            set => this.SetProperty(value);
        }

        /// <summary>
        /// 获取当前视图表示的枚举值是否包含指定的枚举位域。
        /// </summary>
        /// <param name="flagName">要确定的枚举位域的名称。</param>
        /// <returns>若当前视图表示的枚举值包含指定的枚举位域值，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="flagName"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="flagName"/> 不为有效的枚举值名称。</exception>
        protected bool HasFlag(
            [CallerMemberName] string flagName = null)
        {
            return this[(TEnum)Enum.Parse(typeof(TEnum), flagName)];
        }

        /// <summary>
        /// 根据指示设置当前视图表示的枚举值的指定的枚举位域。
        /// </summary>
        /// <param name="flagName">要设置的枚举位域的名称。</param>
        /// <param name="value">指示是添加位域还是移除位域。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="flagName"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="flagName"/> 不为有效的枚举值名称。</exception>
        protected virtual void SetFlag(bool value,
            [CallerMemberName] string flagName = null)
        {
            this[(TEnum)Enum.Parse(typeof(TEnum), flagName)] = value;
        }

        /// <summary>
        /// 设置指定属性的值。
        /// </summary>
        /// <typeparam name="T">属性的类型。</typeparam>
        /// <param name="value">属性的新值。</param>
        /// <param name="propertyName">要设置值的属性的名称。</param>
        protected override void SetProperty<T>(T value,
            [CallerMemberName] string propertyName = null)
        {
            base.SetProperty(value, propertyName);
            if (propertyName == nameof(this.Value))
            {
                var flagNames = Enum.GetNames(typeof(TEnum));
                Array.ForEach(flagNames, this.NotifyPropertyChanged);
                this.NotifyPropertyChanged(ObservableDataObject.IndexerName);
            }
        }
    }
}
