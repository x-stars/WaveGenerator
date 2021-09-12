using System;
using XstarS.WaveGenerator.Waveforms;

namespace XstarS.WaveGenerator.Models
{
    /// <summary>
    /// 表示波形的参数。
    /// </summary>
    public struct WaveformParameters : IEquatable<WaveformParameters>
    {
        /// <summary>
        /// 以波形、频率和幅度初始化 <see cref="WaveformParameters"/> 结构的新实例。
        /// </summary>
        /// <param name="waveform">波形的枚举常数。</param>
        /// <param name="amplitude">波形的幅度，应在 0 到 1 之间。</param>
        /// <param name="frequency">波形的频率，单位 Hz。</param>
        /// <exception cref="ArgumentOutOfRangeException">输入的参数超出合理范围。</exception>
        public WaveformParameters(Waveform waveform, double amplitude, double frequency)
        {
            if ((waveform < Waveform.Sine) || (waveform > Waveform.Sawtooth))
            {
                throw new ArgumentOutOfRangeException(nameof(waveform));
            }
            if (frequency < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(frequency));
            }
            if ((amplitude < 0) || (amplitude > 1))
            {
                throw new ArgumentOutOfRangeException(nameof(amplitude));
            }

            this.Waveform = waveform;
            this.Frequency = frequency;
            this.Amplitude = amplitude;
        }

        /// <summary>
        /// 获取波形的枚举常数。
        /// </summary>
        public Waveform Waveform { get; }

        /// <summary>
        /// 获取波形以 1 为峰值的幅度。
        /// </summary>
        public double Amplitude { get; }

        /// <summary>
        /// 获取波形以 Hz 为单位的频率。
        /// </summary>
        public double Frequency { get; }

        /// <summary>
        /// 指示当前 <see cref="WaveformParameters"/> 是否等于另一 <see cref="WaveformParameters"/>。
        /// </summary>
        /// <param name="other">要当前实例进行比较的另一 <see cref="WaveformParameters"/>。</param>
        /// <returns>若 <paramref name="other"/> 与当前实例相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public bool Equals(WaveformParameters other)
        {
            return (this.Waveform == other.Waveform) &&
                   (this.Amplitude == other.Amplitude) &&
                   (this.Frequency == other.Frequency);
        }

        /// <summary>
        /// 指示当前 <see cref="WaveformParameters"/> 是否等于另一对象。
        /// </summary>
        /// <param name="obj">要当前实例进行比较的另一对象。</param>
        /// <returns>若 <paramref name="obj"/> 为 <see cref="WaveformParameters"/> 且与当前实例相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public override bool Equals(object obj)
        {
            return (obj is WaveformParameters other) && this.Equals(other);
        }

        /// <summary>
        /// 获取当前实例的哈希代码。
        /// </summary>
        /// <returns>当前实例的哈希代码。</returns>
        public override int GetHashCode()
        {
            var hashCode = -482039158;
            hashCode = hashCode * -1521134295 + this.Waveform.GetHashCode();
            hashCode = hashCode * -1521134295 + this.Amplitude.GetHashCode();
            hashCode = hashCode * -1521134295 + this.Frequency.GetHashCode();
            return hashCode;
        }

        /// <summary>
        /// 比较两个 <see cref="WaveformParameters"/> 是否相等。
        /// </summary>
        /// <param name="left">要进行比较的第一个 <see cref="WaveformParameters"/>。</param>
        /// <param name="right">要进行比较的第二个 <see cref="WaveformParameters"/>。</param>
        /// <returns>若 <paramref name="left"/> 与 <paramref name="right"/> 相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public static bool operator ==(WaveformParameters left, WaveformParameters right) => left.Equals(right);

        /// <summary>
        /// 比较两个 <see cref="WaveformParameters"/> 是否不等。
        /// </summary>
        /// <param name="left">要进行比较的第一个 <see cref="WaveformParameters"/>。</param>
        /// <param name="right">要进行比较的第二个 <see cref="WaveformParameters"/>。</param>
        /// <returns>若 <paramref name="left"/> 与 <paramref name="right"/> 不等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public static bool operator !=(WaveformParameters left, WaveformParameters right) => !(left == right);
    }
}
