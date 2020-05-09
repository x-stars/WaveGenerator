using System;
using System.Runtime.CompilerServices;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 提供枚举的向量视图的基类。
    /// </summary>
    /// <typeparam name="TEnum">枚举的类型。</typeparam>
    [Serializable]
    public class EnumVectorView<TEnum> : ObservableDataObject
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
            get => object.Equals(this.Value, enumValue);
            set { if (value) { this.Value = enumValue; } }
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
        /// 获取当前视图表示的枚举值是否为指定的枚举值。
        /// </summary>
        /// <param name="enumName">要确定的枚举值的名称。</param>
        /// <returns>若当前视图表示的枚举值为指定的枚举值，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="enumName"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="enumName"/> 不为有效的枚举值名称。</exception>
        protected bool IsEnum(
            [CallerMemberName] string enumName = null)
        {
            return this[(TEnum)Enum.Parse(typeof(TEnum), enumName)];
        }

        /// <summary>
        /// 根据指示设置当前视图表示的枚举值。
        /// </summary>
        /// <param name="value">指示是否设置枚举值。</param>
        /// <param name="enumName">要设置的枚举值的名称。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="enumName"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="enumName"/> 不为有效的枚举值名称。</exception>
        protected virtual void SetEnum(bool value,
            [CallerMemberName] string enumName = null)
        {
            this[(TEnum)Enum.Parse(typeof(TEnum), enumName)] = value;
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
                var enumNames = Enum.GetNames(typeof(TEnum));
                Array.ForEach(enumNames, this.NotifyPropertyChanged);
                this.NotifyPropertyChanged(ObservableDataObject.IndexerName);
            }
        }
    }
}
