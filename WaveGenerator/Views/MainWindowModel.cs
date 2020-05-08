using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Media;
using XstarS.ComponentModel;
using XstarS.WaveGenerator.Models;

namespace XstarS.WaveGenerator.Views
{
    /// <summary>
    /// 表示主窗口 <see cref="MainWindow"/> 的数据逻辑模型。
    /// </summary>
    public class MainWindowModel : ObservableValidDataObject
    {
        /// <summary>
        /// 表示临时波形声音文件的路径。
        /// </summary>
        private readonly string TempWavePath = Path.Combine(
            Path.GetTempPath(), $"WaveGenerator.{Path.GetRandomFileName()}.wav");

        /// <summary>
        /// 表示用于播放波形声音的媒体播放器。
        /// </summary>
        private readonly MediaPlayer WavePlayer;

        /// <summary>
        /// 初始化 <see cref="MainWindowModel"/> 类的新实例。
        /// </summary>
        public MainWindowModel()
        {
            this.WaveformView = new EnumVectorView<Waveform>();
            this.WaveformView.Value = Waveform.Sine;
            this.WaveFrequency = 440;
            this.HasLeftChannel = true;
            this.HasRightChannel = true;
            this.WavePlayer = new MediaPlayer();
            this.WavePlayer.MediaEnded += this.WavePlayer_MediaEnded;
            this.WavePlayer.MediaFailed += this.WavePlayer_MediaEnded;
            this.GenerateWaveCommand = new DelegateCommand(
                _ => this.GenerateWave(), _ => this.CanGenerateWave);
            this.CanGenerateWave = true;
        }

        /// <summary>
        /// 获取当前波形声音的波形枚举的向量视图。
        /// </summary>
        public EnumVectorView<Waveform> WaveformView { get; }

        /// <summary>
        /// 表示波形声音允许的最小频率。
        /// </summary>
        public int MinWaveFrequency => 10;

        /// <summary>
        /// 表示波形声音允许的最大频率。
        /// </summary>
        public int MaxWaveFrequency => 22000;

        /// <summary>
        /// 获取或设置波形声音的频率。
        /// </summary>
        public int WaveFrequency
        {
            get => this.GetProperty<int>();
            set => this.SetProperty(value);
        }

        /// <summary>
        /// 获取或设置是否在左声道输出声音。
        /// </summary>
        public bool HasLeftChannel
        {
            get => this.GetProperty<bool>();
            set => this.SetProperty(value);
        }

        /// <summary>
        /// 获取或设置是否在右声道输出声音。
        /// </summary>
        public bool HasRightChannel
        {
            get => this.GetProperty<bool>();
            set => this.SetProperty(value);
        }

        /// <summary>
        /// 获取或设置是否可以生成声音。
        /// </summary>
        public bool CanGenerateWave
        {
            get => this.GetProperty<bool>();
            set => this.SetProperty(value);
        }

        /// <summary>
        /// 获取生成并播放波形声音的命令。
        /// </summary>
        public DelegateCommand GenerateWaveCommand { get; }

        /// <summary>
        /// 生成并播放当前模型定义的波形声音。
        /// </summary>
        public void GenerateWave()
        {
            if (!this.CanGenerateWave) { return; }
            this.CanGenerateWave = false;

            var path = this.TempWavePath;
            using (var file = File.OpenWrite(path))
            {
                using (var waveWriter = new WaveStreamWriter(file))
                {
                    var durationSeconds = 1.0;
                    WaveGenerators.GenerateWave(
                        waveWriter, this.GetWaveParameters(),
                        this.GetChannelEnables(), durationSeconds);
                }
            }

            var uri = new Uri(path);
            this.WavePlayer.Open(uri);
            this.WavePlayer.Volume = 1.0;
            this.WavePlayer.Play();
        }

        /// <summary>
        /// <see cref="MainWindowModel.WavePlayer"/> 媒体播放完成的事件处理。
        /// </summary>
        /// <param name="sender">事件源。</param>
        /// <param name="e">提供事件数据的对象。</param>
        private void WavePlayer_MediaEnded(object sender, EventArgs e)
        {
            this.WavePlayer.Close();
            File.Delete(this.TempWavePath);
            this.CanGenerateWave = true;
        }

        /// <summary>
        /// 获取当前模型表示的波形声音的波形参数。
        /// </summary>
        /// <returns>当前模型表示的波形声音的波形参数。</returns>
        public WaveParameters GetWaveParameters()
        {
            return new WaveParameters(this.WaveformView.Value, 1.0, this.WaveFrequency);
        }

        /// <summary>
        /// 获取当前模型表示的要输出声音的声道。
        /// </summary>
        /// <returns>当前模型表示的要输出声音的声道。</returns>
        public bool[] GetChannelEnables()
        {
            return new[] { this.HasLeftChannel, this.HasRightChannel };
        }

        /// <inheritdoc/>
        protected override void ValidateProperty(string propertyName)
        {
            base.ValidateProperty(propertyName);
            if (propertyName == nameof(this.WaveFrequency))
            {
                if ((this.WaveFrequency < this.MinWaveFrequency) ||
                    (this.WaveFrequency > this.MaxWaveFrequency))
                {
                    var errors = new[] { new ArgumentOutOfRangeException().Message };
                    this.SetErrors(errors, propertyName);
                }
                else
                {
                    this.SetErrors(null, propertyName);
                }
            }
        }

        /// <inheritdoc/>
        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.PropertyName == nameof(this.CanGenerateWave))
            {
                this.GenerateWaveCommand.NotifyCanExecuteChanged();
            }
        }
    }
}
