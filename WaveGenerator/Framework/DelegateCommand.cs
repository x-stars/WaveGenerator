using System;
using System.ComponentModel;
using System.Windows.Input;
using XstarS.ComponentModel;

namespace XstarS.Windows.Input
{
    /// <summary>
    /// 表示由委托定义的命令 <see cref="ICommand"/>。
    /// </summary>
    public class DelegateCommand : CommandBase
    {
        /// <summary>
        /// 表示 <see cref="DelegateCommand.Execute(object)"/> 方法的委托。
        /// </summary>
        private readonly Action<object?> ExecuteDelegate;

        /// <summary>
        /// 表示 <see cref="DelegateCommand.CanExecute(object)"/> 方法的委托。
        /// </summary>
        private readonly Predicate<object?> CanExecuteDelegate;

        /// <summary>
        /// 使用指定的委托初始化 <see cref="DelegateCommand"/> 类的新实例。
        /// </summary>
        /// <param name="executeDelegate">
        /// <see cref="DelegateCommand.Execute(object)"/> 方法的委托。</param>
        /// <param name="canExecuteDelegate">
        /// <see cref="DelegateCommand.CanExecute(object)"/> 方法的委托。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="executeDelegate"/> 为 <see langword="null"/>。</exception>
        public DelegateCommand(Action<object?> executeDelegate,
            Predicate<object?>? canExecuteDelegate = null)
        {
            this.ExecuteDelegate = executeDelegate ??
                throw new ArgumentNullException(nameof(executeDelegate));
            this.CanExecuteDelegate = canExecuteDelegate ?? base.CanExecute;
        }

        /// <summary>
        /// 在当前状态下执行此命令。
        /// </summary>
        /// <param name="parameter">此命令使用的数据。</param>
        public override void Execute(object? parameter)
        {
            this.ExecuteDelegate.Invoke(parameter);
        }

        /// <summary>
        /// 确定此命令是否可在其当前状态下执行。
        /// </summary>
        /// <param name="parameter">此命令使用的数据。</param>
        /// <returns>如果可执行此命令，则为 <see langword="true"/>；
        /// 否则为 <see langword="false"/>。</returns>
        public override bool CanExecute(object? parameter)
        {
            return this.CanExecuteDelegate.Invoke(parameter);
        }

        /// <summary>
        /// 通知当前命令的可执行状态已更改。
        /// </summary>
        public new void NotifyCanExecuteChanged()
        {
            base.NotifyCanExecuteChanged();
        }

        /// <summary>
        /// 设定在指定属性发生更改时，通知当前命令的可执行状态已更改。
        /// </summary>
        /// <param name="source">发出属性更改通知的事件源对象。</param>
        /// <param name="propertyName">要接收更改通知的属性的名称。</param>
        /// <returns>当前 <see cref="DelegateCommand"/> 实例。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> 为 <see langword="null"/>。</exception>
        public DelegateCommand ObserveCanExecute(
            INotifyPropertyChanged source, string? propertyName)
        {
            if (source is null) { throw new ArgumentNullException(nameof(source)); }
            var observer = new SimplePropertyObserver(source, propertyName);
            observer.ObservingPropertyChanged += this.OnObservingCanExecuteChanged;
            return this;
        }

        /// <summary>
        /// 当指定属性发生更改时，通知命令的可执行状态发生更改。
        /// </summary>
        /// <param name="sender">属性更改通知的事件源。</param>
        /// <param name="e">提供属性更改通知的事件数据。</param>
        private void OnObservingCanExecuteChanged(object? sender, EventArgs e)
        {
            this.NotifyCanExecuteChanged();
        }
    }
}
