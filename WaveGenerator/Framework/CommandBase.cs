using System;
using System.Threading;
using System.Windows.Input;

namespace XstarS.Windows.Input
{
    /// <summary>
    /// 提供命令 <see cref="ICommand"/> 的抽象基类。
    /// </summary>
    public abstract class CommandBase : ICommand
    {
        /// <summary>
        /// 表示创建当前命令的线程的同步上下文。
        /// </summary>
        private readonly SynchronizationContext? InitialSyncContext;

        /// <summary>
        /// 初始化 <see cref="CommandBase"/> 类的新实例。
        /// </summary>
        protected CommandBase()
        {
            this.InitialSyncContext = SynchronizationContext.Current;
        }

        /// <summary>
        /// 当出现影响是否应执行该命令的更改时发生。
        /// </summary>
        public event EventHandler? CanExecuteChanged;

        /// <summary>
        /// 在当前状态下执行此命令。
        /// </summary>
        /// <param name="parameter">此命令使用的数据。</param>
        public abstract void Execute(object? parameter);

        /// <summary>
        /// 确定此命令是否可在其当前状态下执行。
        /// </summary>
        /// <param name="parameter">此命令使用的数据。</param>
        /// <returns>如果可执行此命令，则为 <see langword="true"/>；
        /// 否则为 <see langword="false"/>。</returns>
        public virtual bool CanExecute(object? parameter) => true;

        /// <summary>
        /// 通知当前命令的可执行状态已更改。
        /// </summary>
        protected void NotifyCanExecuteChanged()
        {
            this.OnCanExecuteChanged(EventArgs.Empty);
        }

        /// <summary>
        /// 使用指定的事件数据引发 <see cref="CommandBase.CanExecuteChanged"/> 事件。
        /// </summary>
        /// <param name="e">包含事件数据的 <see cref="EventArgs"/>。</param>
        protected virtual void OnCanExecuteChanged(EventArgs e)
        {
            if (this.InitialSyncContext is null) { this.OnCanExecuteChanged((object)e); }
            else { this.InitialSyncContext.Post(this.OnCanExecuteChanged, (object)e); }
        }

        /// <summary>
        /// 使用指定的事件数据引发 <see cref="CommandBase.CanExecuteChanged"/> 事件。
        /// </summary>
        /// <param name="e">包含事件数据的 <see cref="EventArgs"/>。</param>
        private void OnCanExecuteChanged(object? e)
        {
            this.CanExecuteChanged?.Invoke(this, (EventArgs)e!);
        }
    }
}
