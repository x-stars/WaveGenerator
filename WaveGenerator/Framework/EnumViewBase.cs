using System;
using System.Diagnostics;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 提供枚举的视图的抽象基类。
    /// </summary>
    /// <typeparam name="TEnum">枚举的类型。</typeparam>
    [Serializable]
    [DebuggerDisplay("Value = {" + nameof(Value) + "}")]
    public abstract class EnumViewBase<TEnum> : ObservableDataObject
        where TEnum : struct, Enum
    {
        /// <summary>
        /// 初始化 <see cref="EnumVectorView{TEnum}"/> 类的新实例。
        /// </summary>
        protected EnumViewBase() { }

        /// <summary>
        /// 获取或设置当前视图表示的枚举值。
        /// </summary>
        /// <returns>当前视图表示的枚举值。</returns>
        public TEnum Value
        {
            get => this.GetProperty<TEnum>();
            set => this.SetEnumValue(value);
        }

        /// <summary>
        /// 将当前枚举类型的枚举名称转换为等效的枚举值。
        /// </summary>
        /// <param name="name">要转换的枚举值的名称。</param>
        /// <returns>名为 <paramref name="name"/> 的枚举值。</returns>
        protected TEnum ParseEnum(string name)
        {
            return (TEnum)Enum.Parse(typeof(TEnum), name);
        }

        /// <summary>
        /// 设置当前视图表示的枚举值。
        /// </summary>
        /// <param name="value">要设置的枚举值。</param>
        protected virtual void SetEnumValue(TEnum value)
        {
            var enumValue = this.Value;
            this.SetProperty(value, nameof(this.Value));
            var valueChanged = !object.Equals(enumValue, value);
            if (valueChanged) { this.NotifyEnumPropertiesChanged(); }
        }

        /// <summary>
        /// 通知所有枚举值对应的属性的值已更改。
        /// </summary>
        protected void NotifyEnumPropertiesChanged()
        {
            var enumNames = Enum.GetNames(typeof(TEnum));
            Array.ForEach(enumNames, this.NotifyPropertyChanged);
            this.NotifyPropertyChanged(ObservableDataObject.IndexerName);
        }
    }
}
