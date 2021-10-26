using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
        /// 表示所有属性的验证错误。
        /// </summary>
        private readonly ConcurrentDictionary<string, IEnumerable> PropertiesErrors;

        /// <summary>
        /// 初始化 <see cref="ObservableValidDataObject"/> 类的新实例。
        /// </summary>
        protected ObservableValidDataObject()
        {
            this.PropertiesErrors = new ConcurrentDictionary<string, IEnumerable>();
        }

        /// <summary>
        /// 获取当前实体是否包含验证错误。
        /// </summary>
        /// <returns>如果实体当前具有验证错误，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public bool HasErrors => !this.PropertiesErrors.IsEmpty;

        /// <summary>
        /// 当验证错误更改时发生。
        /// </summary>
        [field: NonSerialized]
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        /// <summary>
        /// 获取指定属性是否包含验证错误。
        /// </summary>
        /// <param name="propertyName">要获取是否包含验证错误的属性的名称。</param>
        /// <returns>如果指定属性当前具有验证错误，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public bool ContainsErrors(
            [CallerMemberName] string propertyName = null)
        {
            propertyName = propertyName ?? string.Empty;
            return this.PropertiesErrors.ContainsKey(propertyName);
        }

        /// <summary>
        /// 获取指定属性的验证错误。
        /// </summary>
        /// <param name="propertyName">要获取验证错误的属性的名称。</param>
        /// <returns>指定属性的验证错误。</returns>
        public IEnumerable GetErrors(
            [CallerMemberName] string propertyName = null)
        {
            propertyName = propertyName ?? string.Empty;
            this.PropertiesErrors.TryGetValue(propertyName, out var errors);
            return errors ?? Array.Empty<object>();
        }

        /// <summary>
        /// 设置指定属性的验证错误。
        /// </summary>
        /// <param name="errors">属性的验证错误。</param>
        /// <param name="propertyName">要设置验证错误的属性的名称。</param>
        protected virtual void SetErrors(IEnumerable errors,
            [CallerMemberName] string propertyName = null)
        {
            propertyName = propertyName ?? string.Empty;
            var thisHadErrors = !this.PropertiesErrors.IsEmpty;
            var hadErrors = this.PropertiesErrors.ContainsKey(propertyName);
            var hasErrors = !(errors is null) && errors.GetEnumerator().MoveNext();
            if (hasErrors) { this.PropertiesErrors[propertyName] = errors; }
            else { this.PropertiesErrors.TryRemove(propertyName, out errors); }
            var errorsChanged = hadErrors || hasErrors;
            if (errorsChanged) { this.NotifyErrorsChanged(propertyName); }
            var hasErrorsChanged = thisHadErrors ^ this.HasErrors;
            if (hasErrorsChanged) { this.NotifyPropertyChanged(nameof(this.HasErrors)); }
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
            propertyName = propertyName ?? string.Empty;
            base.SetProperty(value, propertyName);
            this.ValidateProperty<T>(propertyName);
        }

        /// <summary>
        /// 验证指定属性的错误。
        /// </summary>
        /// <typeparam name="T">属性的类型。</typeparam>
        /// <param name="propertyName">要验证错误的属性的名称。</param>
        protected virtual void ValidateProperty<T>(
            [CallerMemberName] string propertyName = null)
        {
            propertyName = propertyName ?? string.Empty;
            var value = this.GetProperty<T>(propertyName);
            var context = new ValidationContext(this) { MemberName = propertyName };
            var results = new List<ValidationResult>();
            try { Validator.TryValidateProperty(value, context, results); }
            catch (ArgumentException) { }
            var errors = new List<string>(results.Count);
            foreach (var result in results) { errors.Add(result.ErrorMessage); }
            this.SetErrors(errors, propertyName);
        }

        /// <summary>
        /// 通知指定属性的验证错误已更改。
        /// </summary>
        /// <param name="propertyName">验证错误已更改的属性的名称。</param>
        protected void NotifyErrorsChanged(
            [CallerMemberName] string propertyName = null)
        {
            propertyName = propertyName ?? string.Empty;
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
