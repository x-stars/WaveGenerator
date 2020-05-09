using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        /// 表示当前接收属性更改通知的 <see cref="INotifyPropertyChanged"/> 对象的属性的名称。
        /// </summary>
        private readonly Dictionary<object, HashSet<string>> SourcePropertyNames;

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
            this.SourcePropertyNames = new Dictionary<object, HashSet<string>>();
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
            this.SourcePropertyNames = new Dictionary<object, HashSet<string>>();
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

        /// <summary>
        /// 设定在指定 <see cref="INotifyPropertyChanged"/> 对象的指定名称的属性发生更改时引发
        /// <see cref="CommandBase.CanExecuteChanged"/> 事件。
        /// </summary>
        /// <param name="source">发出通知的 <see cref="INotifyPropertyChanged"/> 对象。</param>
        /// <param name="propertyName">要接收更改通知的属性的名称。</param>
        /// <returns>当前 <see cref="DelegateCommand"/> 实例。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> 为 <see langword="null"/>。</exception>
        public DelegateCommand ObserveCanExecute(
            INotifyPropertyChanged source, string propertyName)
        {
            source = source ?? throw new ArgumentNullException(nameof(source));
            propertyName = propertyName ?? string.Empty;
            var sourcePropertyNames = this.SourcePropertyNames;
            if (!sourcePropertyNames.ContainsKey(source))
            {
                sourcePropertyNames[source] = new HashSet<string>();
                source.PropertyChanged += this.OnSourcePropertyChanged;
            }
            sourcePropertyNames[source].Add(propertyName);
            return this;
        }

        /// <summary>
        /// 每当接收属性更改通知的 <see cref="INotifyPropertyChanged"/> 对象的
        /// <see cref="INotifyPropertyChanged.PropertyChanged"/> 事件发生时调用。
        /// </summary>
        /// <param name="sender">作为事件源的 <see cref="INotifyPropertyChanged"/>。</param>
        /// <param name="e">提供事件数据的 <see cref="PropertyChangedEventArgs"/>。</param>
        private void OnSourcePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var propertyName = e.PropertyName ?? string.Empty;
            var sourcePropertyNames = this.SourcePropertyNames;
            if (sourcePropertyNames.ContainsKey(sender))
            {
                if (sourcePropertyNames[sender].Contains(propertyName))
                {
                    this.NotifyCanExecuteChanged();
                }
            }
        }
    }
}
