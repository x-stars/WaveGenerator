using System.Windows;

namespace XstarS.WaveGenerator.Views
{
    /// <summary>
    /// 表示应用程序的主窗口。
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 初始化 <see cref="MainWindow"/> 类的新实例。
        /// </summary>
        public MainWindow()
        {
            this.DataContext = new MainWindowModel();
            this.InitializeComponent();
        }
    }
}
