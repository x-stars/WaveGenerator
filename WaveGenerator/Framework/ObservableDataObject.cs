using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 提供属性更改通知类型 <see cref="INotifyPropertyChanged"/> 的抽象基类。
    /// </summary>
    [Serializable]
    public abstract class ObservableDataObject : INotifyPropertyChanged
    {
        /// <summary>
        /// 表示索引属性的默认名称。
        /// </summary>
        protected const string IndexerName = "Item[]";

        /// <summary>
        /// 表示不存在的值，用于区别 <see langword="null"/> 值。
        /// </summary>
        private static readonly object? Missing = new object();

        /// <summary>
        /// 表示所有属性的值。
        /// </summary>
        private readonly ConcurrentDictionary<string, object?> Properties;

        /// <summary>
        /// 表示所有属性的关联属性的名称。
        /// </summary>
        private readonly ConcurrentDictionary<string, string[]> RelatedProperties;

        /// <summary>
        /// 初始化 <see cref="ObservableDataObject"/> 类的新实例。
        /// </summary>
        protected ObservableDataObject()
        {
            this.Properties = new ConcurrentDictionary<string, object?>();
            this.RelatedProperties = new ConcurrentDictionary<string, string[]>();
            this.InitializeRelatedProperties();
        }

        /// <summary>
        /// 在属性值更改时发生。
        /// </summary>
        [field: NonSerialized]
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// 在派生类中重写，初始化所有属性的关联属性的名称。
        /// </summary>
        protected virtual void InitializeRelatedProperties() { }

        /// <summary>
        /// 确定指定的属性名称是否表示当前数据实体。
        /// </summary>
        /// <param name="propertyName">要确认是否表示数据实体的属性名称。</param>
        /// <returns>若 <paramref name="propertyName"/> 为 <see langword="null"/> 或空字符串，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        protected bool IsEntityName([NotNullWhen(false)] string? propertyName)
        {
            return string.IsNullOrEmpty(propertyName);
        }

        /// <summary>
        /// 获取指定属性或当前实体的值。
        /// </summary>
        /// <typeparam name="T">属性的类型。</typeparam>
        /// <param name="propertyName">要获取值的属性的名称；
        /// 如果要获取当前实体的值，则为 <see langword="null"/> 或空字符串。</param>
        /// <returns>名为 <paramref name="propertyName"/> 的属性或当前实体的值。</returns>
        /// <exception cref="InvalidCastException">
        /// 指定属性或实体的值无法转换为 <typeparamref name="T"/> 类型。</exception>
        [return: MaybeNull]
        protected T GetProperty<T>([CallerMemberName] string? propertyName = null)
        {
            if (this.IsEntityName(propertyName)) { return (T)(object)this; }
            var value = this.GetPropertyCore(propertyName);
            var hasValue = value != ObservableDataObject.Missing;
            return hasValue ? (T?)value : default(T);
        }

        /// <summary>
        /// 设置指定属性的值。
        /// </summary>
        /// <typeparam name="T">属性的类型。</typeparam>
        /// <param name="value">属性的新值。</param>
        /// <param name="propertyName">要设置值的属性的名称。</param>
        /// <exception cref="InvalidCastException">
        /// <paramref name="value"/> 无法转换为指定属性的类型。</exception>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="propertyName"/> 为 <see langword="null"/> 或空字符串。</exception>
        protected virtual void SetProperty<T>(
            [AllowNull] T value, [CallerMemberName] string? propertyName = null)
        {
            if (this.IsEntityName(propertyName)) { throw new InvalidOperationException(); }
            var property = this.ExchangeProperty(propertyName, value);
            var propertyChanged = !RuntimeHelpers.Equals(property, value);
            if (propertyChanged) { this.RelatedNotifyPropertyChanged(propertyName); }
        }

        /// <summary>
        /// 获取指定属性的值。
        /// </summary>
        /// <param name="propertyName">要获取值的属性的名称。</param>
        /// <returns>名为 <paramref name="propertyName"/> 的属性的值。</returns>
        protected virtual object? GetPropertyCore(string propertyName)
        {
            var hasValue = this.Properties.TryGetValue(propertyName, out var value);
            return hasValue ? value : ObservableDataObject.Missing;
        }

        /// <summary>
        /// 设置指定属性的值。
        /// </summary>
        /// <param name="value">属性的新值。</param>
        /// <param name="propertyName">要设置值的属性的名称。</param>
        /// <exception cref="InvalidCastException">
        /// <paramref name="value"/> 无法转换为指定属性的类型。</exception>
        protected virtual void SetPropertyCore(string propertyName, object? value)
        {
            this.Properties[propertyName] = value;
        }

        /// <summary>
        /// 设置指定属性的值，并返回其原值。
        /// </summary>
        /// <param name="value">属性的新值。</param>
        /// <param name="propertyName">要设置值的属性的名称。</param>
        /// <returns>名为 <paramref name="propertyName"/> 的属性的原值。</returns>
        protected virtual object? ExchangeProperty(string propertyName, object? value)
        {
            var property = this.GetPropertyCore(propertyName);
            this.SetPropertyCore(propertyName, value);
            return property;
        }

        /// <summary>
        /// 获取指定属性的关联属性的名称。
        /// </summary>
        /// <param name="propertyName">要获取关联属性的属性的名称。</param>
        /// <returns>名为 <paramref name="propertyName"/> 的属性的关联属性的名称。</returns>
        protected string[] GetRelatedProperties([CallerMemberName] string? propertyName = null)
        {
            if (this.IsEntityName(propertyName)) { return Array.Empty<string>(); }
            this.RelatedProperties.TryGetValue(propertyName, out var relatedProperties);
            return relatedProperties ?? Array.Empty<string>();
        }

        /// <summary>
        /// 设置指定属性的关联属性的名称。
        /// </summary>
        /// <param name="propertyName">要设置关联属性的属性的名称。</param>
        /// <param name="relatedProperties">指定属性的关联属性的名称的数组。</param>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="propertyName"/> 为 <see langword="null"/> 或空字符串。</exception>
        protected void SetRelatedProperties(string propertyName, params string[] relatedProperties)
        {
            if (this.IsEntityName(propertyName)) { throw new InvalidOperationException(); }
            var hasRelated = !(relatedProperties is null) && (relatedProperties.Length != 0);
            if (hasRelated) { this.RelatedProperties[propertyName] = relatedProperties!; }
            else { this.RelatedProperties.TryRemove(propertyName, out relatedProperties!); }
        }

        /// <summary>
        /// 通知指定属性的值已更改。
        /// </summary>
        /// <param name="propertyName">已更改属性的名称。</param>
        protected void NotifyPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 通知指定属性的值已更改，并通知其关联属性的值已更改。
        /// </summary>
        /// <param name="propertyName">已更改属性的名称。</param>
        protected void RelatedNotifyPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            this.NotifyPropertyChanged(propertyName);
            if (this.IsEntityName(propertyName)) { return; }
            var relatedProperties = this.GetRelatedProperties(propertyName);
            Array.ForEach(relatedProperties, this.NotifyPropertyChanged);
        }

        /// <summary>
        /// 使用指定的事件数据引发 <see cref="ObservableDataObject.PropertyChanged"/> 事件。
        /// </summary>
        /// <param name="e">包含事件数据的 <see cref="PropertyChangedEventArgs"/>。</param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            this.PropertyChanged?.Invoke(this, e);
        }
    }
}
