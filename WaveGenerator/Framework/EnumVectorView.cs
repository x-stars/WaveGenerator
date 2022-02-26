using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 提供枚举的向量视图的基类。
    /// </summary>
    /// <typeparam name="TEnum">枚举的类型。</typeparam>
    [Serializable]
    [DebuggerDisplay("Value = {" + nameof(Value) + "}")]
    public class EnumVectorView<TEnum> : ObservableDataObject
        where TEnum : struct, Enum
    {
        /// <summary>
        /// 表示在当前枚举类型中通过枚举名称查找枚举值的查找表。
        /// </summary>
        private static readonly ReadOnlyDictionary<string, TEnum> EnumLookupTable =
            EnumVectorView<TEnum>.CreateEnumLookupTable();

        /// <summary>
        /// 表示当前视图表示的枚举值。
        /// </summary>
        private volatile Enum EnumValue = default(TEnum);

        /// <summary>
        /// 初始化 <see cref="EnumVectorView{TEnum}"/> 类的新实例。
        /// </summary>
        public EnumVectorView() { }

        /// <summary>
        /// 获取或设置当前视图表示的枚举值。
        /// </summary>
        /// <returns>当前视图表示的枚举值。</returns>
        public TEnum Value
        {
            get => this.GetProperty<TEnum>();
            set => this.SetProperty(value);
        }

        /// <summary>
        /// 获取或设置当前视图是否选中了指定的枚举值。
        /// </summary>
        /// <param name="enumValue">要获取或设置是否选中的枚举值。</param>
        /// <returns>若当前视图选中了 <paramref name="enumValue"/>，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public bool this[TEnum enumValue]
        {
            get => this.IsSelected(enumValue);
            set => this.SelectEnum(enumValue, value);
        }

        /// <summary>
        /// 创建在当前枚举类型中通过枚举名称查找枚举值的查找表。
        /// </summary>
        /// <returns>在当前枚举类型中通过枚举名称查找枚举值的查找表。</returns>
        private static ReadOnlyDictionary<string, TEnum> CreateEnumLookupTable()
        {
            var enumNames = Enum.GetNames(typeof(TEnum));
            var enumValues = (TEnum[])Enum.GetValues(typeof(TEnum));
            var lookupTable = new Dictionary<string, TEnum>(enumNames.Length);
            for (int index = 0; index < enumNames.Length; index++)
            {
                lookupTable[enumNames[index]] = enumValues[index];
            }
            return new ReadOnlyDictionary<string, TEnum>(lookupTable);
        }

        /// <summary>
        /// 初始化 <see cref="EnumVectorView{TEnum}.Value"/> 属性的关联枚举属性的名称。
        /// </summary>
        protected override void InitializeRelatedProperties()
        {
            base.InitializeRelatedProperties();
            var enumNames = Enum.GetNames(typeof(TEnum));
            var valueRelated = new string[enumNames.Length + 1];
            Array.Copy(enumNames, valueRelated, enumNames.Length);
            valueRelated[enumNames.Length] = ObservableDataObject.IndexerName;
            this.SetRelatedProperties(nameof(this.Value), valueRelated);
        }

        /// <summary>
        /// 获取指定属性的值。
        /// </summary>
        /// <param name="propertyName">要获取值的属性的名称。</param>
        /// <returns>名为 <paramref name="propertyName"/> 的属性的值。</returns>
        protected override object? GetPropertyCore(string propertyName)
        {
            return (propertyName == nameof(this.Value)) ?
                this.EnumValue : base.GetPropertyCore(propertyName);
        }

        /// <summary>
        /// 设置指定属性的值。
        /// </summary>
        /// <param name="value">属性的新值。</param>
        /// <param name="propertyName">要设置值的属性的名称。</param>
        /// <exception cref="InvalidCastException">
        /// <paramref name="value"/> 无法转换为指定属性的类型。</exception>
        protected override void SetPropertyCore(string propertyName, object? value)
        {
            var isEnumValue = propertyName == nameof(this.Value);
            if (isEnumValue) { this.EnumValue = (TEnum)value!; }
            else { base.SetPropertyCore(propertyName, value); }
        }

        /// <summary>
        /// 设置指定属性的值，并返回其原值。
        /// 当属性为 <see cref="EnumVectorView{TEnum}.Value"/> 时为原子操作。
        /// </summary>
        /// <param name="value">属性的新值。</param>
        /// <param name="propertyName">要设置值的属性的名称。</param>
        /// <returns>名为 <paramref name="propertyName"/> 的属性的原值。</returns>
        /// <exception cref="InvalidCastException">
        /// <paramref name="value"/> 无法转换为指定属性的类型。</exception>
        protected override object? ExchangeProperty(string propertyName, object? value)
        {
            return (propertyName == nameof(this.Value)) ?
                Interlocked.Exchange(ref this.EnumValue, (TEnum)value!) :
                base.ExchangeProperty(propertyName, value);
        }

        /// <summary>
        /// 获取当前视图是否选中了指定的枚举值。
        /// </summary>
        /// <param name="enumValue">要确定是否选中的枚举值。</param>
        /// <returns>若当前视图选中了 <paramref name="enumValue"/>，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        protected virtual bool IsSelected(TEnum enumValue)
        {
            return this.Value.Equals(enumValue);
        }

        /// <summary>
        /// 根据指示在当前视图选中或取消选中指定的枚举值。
        /// </summary>
        /// <param name="enumValue">要选中或取消选中的枚举值。</param>
        /// <param name="select">若要在当前视图选中指定的枚举值，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</param>
        protected virtual void SelectEnum(TEnum enumValue, bool select)
        {
            if (select) { this.Value = enumValue; }
        }

        /// <summary>
        /// 在当前枚举类型中查找具有指定名称的枚举值。
        /// </summary>
        /// <param name="enumName">要查找的枚举值的名称。</param>
        /// <returns><typeparamref name="TEnum"/> 类型中名为
        /// <paramref name="enumName"/> 的枚举值。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="enumName"/> 不为有效的枚举值名称。</exception>
        protected TEnum FindEnumValue(string enumName)
        {
            var lookupTable = EnumVectorView<TEnum>.EnumLookupTable;
            var hasValue = lookupTable.TryGetValue(enumName, out var enumValue);
            return hasValue ? enumValue : (TEnum)Enum.Parse(typeof(TEnum), enumName);
        }

        /// <summary>
        /// 获取当前视图是否选中了具有指定名称的枚举值。
        /// </summary>
        /// <param name="enumName">要确定是否选中的枚举值的名称。</param>
        /// <returns>若当前视图选中了名为 <paramref name="enumName"/> 的枚举值，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="enumName"/> 不为有效的枚举值名称。</exception>
        protected bool IsSelected([CallerMemberName] string? enumName = null)
        {
            var enumValue = this.FindEnumValue(enumName!);
            return this.IsSelected(enumValue);
        }

        /// <summary>
        /// 根据指示在当前视图选中或取消选中具有指定名称的枚举值。
        /// </summary>
        /// <param name="select">若要在当前视图选中指定名称的枚举值，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</param>
        /// <param name="enumName">要选中或取消选中的枚举值的名称。</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="enumName"/> 不为有效的枚举值名称。</exception>
        protected void SelectEnum(bool select, [CallerMemberName] string? enumName = null)
        {
            var enumValue = this.FindEnumValue(enumName!);
            this.SelectEnum(enumValue, select);
        }
    }
}
