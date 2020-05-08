using System;
using System.Windows.Input;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 表示由委托 <see cref="Delegate"/> 定义的命令 <see cref="ICommand"/>。
    /// </summary>
    public sealed class DelegateCommand : CommandBase
    {
        /// <summary>
        /// 表示 <see cref="DelegateCommand.Execute(object)"/> 方法的委托。
        /// </summary>
        private readonly Action<object> ExecuteDelegate;

        /// <summary>
        /// 表示 <see cref="DelegateCommand.CanExecute(object)"/> 方法的委托。
        /// </summary>
        private readonly Predicate<object> CanExecuteDelegate;

        /// <summary>
        /// 使用指定的委托初始化 <see cref="DelegateCommand"/> 类的新实例。
        /// </summary>
        /// <param name="executeDelegate">
        /// <see cref="DelegateCommand.Execute(object)"/> 方法的委托。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="executeDelegate"/> 为 <see langword="null"/>。</exception>
        public DelegateCommand(Action<object> executeDelegate)
        {
            this.ExecuteDelegate = executeDelegate ??
                throw new ArgumentNullException(nameof(executeDelegate));
            this.CanExecuteDelegate = base.CanExecute;
        }

        /// <summary>
        /// 使用指定的委托初始化 <see cref="DelegateCommand"/> 类的新实例。
        /// </summary>
        /// <param name="executeDelegate">
        /// <see cref="DelegateCommand.Execute(object)"/> 方法的委托。</param>
        /// <param name="canExecuteDelegate">
        /// <see cref="DelegateCommand.CanExecute(object)"/> 方法的委托。</param>
        /// <exception cref="ArgumentNullException"><paramref name="executeDelegate"/>
        /// 或 <paramref name="canExecuteDelegate"/> 为 <see langword="null"/>。</exception>
        public DelegateCommand(Action<object> executeDelegate,
            Predicate<object> canExecuteDelegate)
        {
            this.ExecuteDelegate = executeDelegate ??
                throw new ArgumentNullException(nameof(executeDelegate));
            this.CanExecuteDelegate = canExecuteDelegate ??
                throw new ArgumentNullException(nameof(canExecuteDelegate));
        }

        /// <summary>
        /// 在当前状态下执行此命令。
        /// </summary>
        /// <param name="parameter">此命令使用的数据。</param>
        public override void Execute(object parameter)
        {
            this.ExecuteDelegate.Invoke(parameter);
        }

        /// <summary>
        /// 确定此命令是否可在其当前状态下执行。
        /// </summary>
        /// <param name="parameter">此命令使用的数据。</param>
        /// <returns>如果可执行此命令，则为 <see langword="true"/>；
        /// 否则为 <see langword="false"/>。</returns>
        public override bool CanExecute(object parameter)
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
    }
}
