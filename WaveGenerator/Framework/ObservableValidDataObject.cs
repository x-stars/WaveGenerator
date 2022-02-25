using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 提供属性更改通知类型 <see cref="INotifyPropertyChanged"/>
    /// 和数据实体验证类型 <see cref="INotifyDataErrorInfo"/> 的抽象基类。
    /// </summary>
    [Serializable]
    public abstract class ObservableValidDataObject : ObservableDataObject, INotifyDataErrorInfo
    {
        /// <summary>
        /// 表示指定实体类型的所有公共实例属性的名称。
        /// </summary>
        private static readonly ConcurrentDictionary<Type, string[]> PropertyNames =
            new ConcurrentDictionary<Type, string[]>();

        /// <summary>
        /// 表示所有属性的验证错误。
        /// </summary>
        private readonly ConcurrentDictionary<string, IEnumerable> ValidationErrors;

        /// <summary>
        /// 初始化 <see cref="ObservableValidDataObject"/> 类的新实例。
        /// </summary>
        protected ObservableValidDataObject()
        {
            this.ValidationErrors = new ConcurrentDictionary<string, IEnumerable>();
        }

        /// <summary>
        /// 获取当前实体是否包含验证错误。
        /// </summary>
        /// <returns>如果实体当前具有验证错误，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public bool HasErrors => !this.ValidationErrors.IsEmpty;

        /// <summary>
        /// 当验证错误更改时发生。
        /// </summary>
        [field: NonSerialized]
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        /// <summary>
        /// 获取指定实体类型的所有公共实例属性的名称。
        /// </summary>
        /// <returns>指定实体类型的所有公共实例属性的名称。</returns>
        private static string[] GetAllPropertyNames(Type entityType)
        {
            var binding = BindingFlags.Instance | BindingFlags.Public;
            var properties = entityType.GetProperties(binding);
            var propertyNames = new List<string>(properties.Length);
            foreach (var property in properties) { propertyNames.Add(property.Name); }
            return propertyNames.ToArray();
        }

        /// <summary>
        /// 获取指定属性或整个实体是否包含验证错误。
        /// </summary>
        /// <param name="propertyName">要获取是否包含验证错误的属性的名称；
        /// 如果要获取实体级别错误，则为 <see langword="null"/> 或空字符串。</param>
        /// <returns>如果名为 <paramref name="propertyName"/> 的属性或当前实体具有验证错误，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public bool ContainsErrors([CallerMemberName] string? propertyName = null)
        {
            if (this.IsEntityName(propertyName)) { return this.HasErrors; }
            return this.ValidationErrors.ContainsKey(propertyName);
        }

        /// <summary>
        /// 获取当前数据实体的所有验证错误。
        /// </summary>
        /// <returns>当前数据实体的所有验证错误。</returns>
        public IEnumerable GetAllErrors()
        {
            var allErrors = this.ValidationErrors.Values;
            return allErrors.SelectMany(Enumerable.Cast<object>).ToList();
        }

        /// <summary>
        /// 获取指定属性或整个实体的验证错误。
        /// </summary>
        /// <param name="propertyName">要获取验证错误的属性的名称；
        /// 如果要获取实体级别错误，则为 <see langword="null"/> 或空字符串。</param>
        /// <returns>名为 <paramref name="propertyName"/> 的属性或当前实体的验证错误。</returns>
        public IEnumerable GetErrors([CallerMemberName] string? propertyName = null)
        {
            if (this.IsEntityName(propertyName)) { return this.GetAllErrors(); }
            this.ValidationErrors.TryGetValue(propertyName, out var errors);
            return errors ?? Array.Empty<object>();
        }

        /// <summary>
        /// 设置指定属性的验证错误。
        /// </summary>
        /// <param name="errors">属性的验证错误。</param>
        /// <param name="propertyName">要设置验证错误的属性的名称。</param>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="propertyName"/> 为 <see langword="null"/> 或空字符串。</exception>
        protected virtual void SetErrors(
            IEnumerable? errors, [CallerMemberName] string? propertyName = null)
        {
            if (this.IsEntityName(propertyName)) { throw new InvalidOperationException(); }
            var entityHadErrors = !this.ValidationErrors.IsEmpty;
            var hadErrors = this.ValidationErrors.ContainsKey(propertyName);
            var hasErrors = !(errors is null) && errors.Cast<object>().Any();
            if (hasErrors) { this.ValidationErrors[propertyName] = errors!; }
            else { this.ValidationErrors.TryRemove(propertyName, out errors!); }
            var errorsChanged = hadErrors || hasErrors;
            if (errorsChanged) { this.NotifyErrorsChanged(propertyName); }
            var hasErrorsChanged = entityHadErrors ^ this.HasErrors;
            if (hasErrorsChanged) { this.RelatedNotifyPropertyChanged(nameof(this.HasErrors)); }
        }

        /// <summary>
        /// 设置指定属性的值，并验证属性的错误。
        /// </summary>
        /// <typeparam name="T">属性的类型。</typeparam>
        /// <param name="value">属性的新值。</param>
        /// <param name="propertyName">要设置值的属性的名称。</param>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="propertyName"/> 为 <see langword="null"/> 或空字符串。</exception>
        protected override void SetProperty<T>(
            [AllowNull] T value, [CallerMemberName] string? propertyName = null)
        {
            base.SetProperty(value, propertyName);
            this.RelatedValidateProperty(propertyName);
        }

        /// <summary>
        /// 验证当前数据实体的错误。
        /// </summary>
        protected void ValidateAllProperties()
        {
            var propertyNames = ObservableValidDataObject.PropertyNames.GetOrAdd(
                this.GetType(), ObservableValidDataObject.GetAllPropertyNames);
            Array.ForEach(propertyNames, this.ValidateProperty);
        }

        /// <summary>
        /// 验证指定属性或整个实体的错误。
        /// </summary>
        /// <param name="propertyName">要验证错误的属性的名称；
        /// 如果要验证当前实体的错误，则为 <see langword="null"/> 或空字符串。</param>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        protected virtual void ValidateProperty([CallerMemberName] string? propertyName = null)
        {
            if (this.IsEntityName(propertyName)) { this.ValidateAllProperties(); return; }
            var value = this.GetProperty<object?>(propertyName);
            var context = new ValidationContext(this) { MemberName = propertyName };
            var results = new List<ValidationResult>();
            try { Validator.TryValidateProperty(value, context, results); } catch { }
            var errors = new List<string?>(results.Count);
            foreach (var result in results) { errors.Add(result.ErrorMessage); }
            this.SetErrors(errors, propertyName);
        }

        /// <summary>
        /// 验证指定属性的错误，并验证其关联属性的错误。
        /// </summary>
        /// <param name="propertyName">要验证错误的属性的名称。</param>
        protected void RelatedValidateProperty([CallerMemberName] string? propertyName = null)
        {
            this.ValidateProperty(propertyName);
            if (this.IsEntityName(propertyName)) { return; }
            var relatedProperties = this.GetRelatedProperties(propertyName);
            Array.ForEach(relatedProperties, this.ValidateProperty);
        }

        /// <summary>
        /// 通知指定属性的验证错误已更改。
        /// </summary>
        /// <param name="propertyName">验证错误已更改的属性的名称。</param>
        protected void NotifyErrorsChanged([CallerMemberName] string? propertyName = null)
        {
            this.OnErrorsChanged(new DataErrorsChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 使用指定的事件数据引发 <see cref="ObservableValidDataObject.ErrorsChanged"/> 事件。
        /// </summary>
        /// <param name="e">包含事件数据的 <see cref="DataErrorsChangedEventArgs"/>。</param>
        protected virtual void OnErrorsChanged(DataErrorsChangedEventArgs e)
        {
            this.ErrorsChanged?.Invoke(this, e);
        }
    }
}
