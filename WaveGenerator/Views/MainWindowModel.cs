using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Windows.Media;
using XstarS.ComponentModel;
using XstarS.WaveGenerator.Models;
using XstarS.WaveGenerator.WaveAudio;
using XstarS.WaveGenerator.Waveforms;
using XstarS.Windows.Input;

namespace XstarS.WaveGenerator.Views
{
    /// <summary>
    /// 表示主窗口 <see cref="MainWindow"/> 的数据逻辑模型。
    /// </summary>
    public class MainWindowModel : ObservableValidDataObject
    {
        /// <summary>
        /// 表示波形声音允许的最小频率。
        /// </summary>
        protected const double MinWaveFreqConst = 16.0;

        /// <summary>
        /// 表示波形声音允许的最大频率。
        /// </summary>
        protected const double MaxWaveFreqConst = 24000.0;

        /// <summary>
        /// 表示波形声音的默认频率。
        /// </summary>
        protected const double DefaultFreqConst = 440.0;

        /// <summary>
        /// 表示临时波形声音文件的路径。
        /// </summary>
        private readonly string TempWavePath = Path.Combine(
            Path.GetTempPath(), $"WaveGenerator.{Path.GetRandomFileName()}.wav");

        /// <summary>
        /// 表示用于播放波形声音的媒体播放器。
        /// </summary>
        private readonly MediaPlayer WavePlayer = new MediaPlayer();

        /// <summary>
        /// 初始化 <see cref="MainWindowModel"/> 类的新实例。
        /// </summary>
        public MainWindowModel()
        {
            this.WaveformView = new EnumVectorView<Waveform>();
            this.WaveformView[Waveform.Sine] = true;
            this.WaveFrequency = DefaultFreqConst;
            this.HasLeftChannel = true;
            this.HasRightChannel = true;
            this.WavePlayer.MediaEnded += this.OnWaveEnded;
            this.WavePlayer.MediaFailed += this.OnWaveEnded;
            this.GenerateWaveCommand = new DelegateCommand(
                _ => this.GenerateWave(), _ => this.CanGenerateWave)
                .ObserveCanExecute(this, nameof(this.CanGenerateWave));
            this.CanGenerateWave = true;
        }

        /// <summary>
        /// 获取当前波形声音的波形枚举的向量视图。
        /// </summary>
        public EnumVectorView<Waveform> WaveformView { get; }

        /// <summary>
        /// 获取波形声音允许的最小频率。
        /// </summary>
        public double MinWaveFrequency => MinWaveFreqConst;

        /// <summary>
        /// 获取波形声音允许的最大频率。
        /// </summary>
        public double MaxWaveFrequency => MaxWaveFreqConst;

        /// <summary>
        /// 获取或设置波形声音的频率。
        /// </summary>
        [Range(MinWaveFreqConst, MaxWaveFreqConst)]
        public double WaveFrequency
        {
            get => this.GetProperty<double>();
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
            if (this.HasErrors) { return; }
            if (!this.CanGenerateWave) { return; }
            this.CanGenerateWave = false;

            var wavePath = this.TempWavePath;
            using (var waveFile = File.OpenWrite(wavePath))
            {
                using (var waveWriter = new WaveStreamWriter(waveFile))
                {
                    WaveformGenerator.GenerateWave(
                        waveWriter, this.GetWaveParameters(),
                        this.GetChannelEnables(), durationSeconds: 1.0);
                }
            }

            var waveUri = new Uri(wavePath);
            this.WavePlayer.Open(waveUri);
            this.WavePlayer.Volume = 1.0;
            this.WavePlayer.Play();
        }

        /// <summary>
        /// 每当波形声音播放完成时调用。
        /// </summary>
        /// <param name="sender">事件源。</param>
        /// <param name="e">提供事件数据的对象。</param>
        private void OnWaveEnded(object? sender, EventArgs e)
        {
            this.WavePlayer.Close();
            File.Delete(this.TempWavePath);
            this.CanGenerateWave = true;
        }

        /// <summary>
        /// 获取当前模型表示的波形声音的波形参数。
        /// </summary>
        /// <returns>当前模型表示的波形声音的波形参数。</returns>
        public WaveformParameters GetWaveParameters() =>
            new WaveformParameters(this.WaveformView.Value, frequency: this.WaveFrequency);

        /// <summary>
        /// 获取当前模型表示的要输出声音的声道。
        /// </summary>
        /// <returns>当前模型表示的要输出声音的声道。</returns>
        public bool[] GetChannelEnables() =>
            new[] { this.HasLeftChannel, this.HasRightChannel };
    }
}
