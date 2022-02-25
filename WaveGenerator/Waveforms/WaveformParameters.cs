using System;
using XstarS.Runtime.CompilerServices;

namespace XstarS.WaveGenerator.Waveforms
{
    /// <summary>
    /// 表示波形的参数。
    /// </summary>
    [Serializable]
    public readonly struct WaveformParameters : IEquatable<WaveformParameters>
    {
        /// <summary>
        /// 以指定的波形初始化 <see cref="WaveformParameters"/> 结构的新实例。
        /// </summary>
        /// <param name="waveform">波形的枚举常数。</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="waveform"/> 不表示有效的波形。</exception>
        public WaveformParameters(Waveform waveform) : this(waveform, amplitude: 1.0)
        {
        }

        /// <summary>
        /// 以指定的波形、频率、幅度和相位初始化 <see cref="WaveformParameters"/> 结构的新实例。
        /// </summary>
        /// <param name="waveform">波形的枚举常数。</param>
        /// <param name="amplitude">波形的幅度，应在 0 到 1 之间。</param>
        /// <param name="frequency">波形的频率，单位 Hz。</param>
        /// <param name="phase">波形的相位，以弧度为单位，建议在 0 到 2π 之间。</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="waveform"/> 不表示有效的波形；或者输入的参数超出合理范围。</exception>
        public WaveformParameters(Waveform waveform,
            double amplitude = 1.0, double frequency = 1 / (2 * Math.PI), double phase = 0.0)
        {
            if (waveform < Waveform.Sine || waveform > Waveform.Sawtooth)
            {
                throw new ArgumentOutOfRangeException(nameof(waveform));
            }
            if (frequency < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(frequency));
            }
            if (amplitude < 0 || amplitude > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(amplitude));
            }

            this.Waveform = waveform;
            this.Frequency = frequency;
            this.Amplitude = amplitude;
            this.Phase = phase;
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
        /// 获取波形以弧度为单位的相位。
        /// </summary>
        public double Phase { get; }

        /// <summary>
        /// 指示当前 <see cref="WaveformParameters"/> 是否等于另一 <see cref="WaveformParameters"/>。
        /// </summary>
        /// <param name="other">要当前实例进行比较的另一 <see cref="WaveformParameters"/>。</param>
        /// <returns>若 <paramref name="other"/> 与当前实例相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public bool Equals(WaveformParameters other) => this.BinaryEquals(other);

        /// <summary>
        /// 指示当前 <see cref="WaveformParameters"/> 是否等于另一对象。
        /// </summary>
        /// <param name="obj">要当前实例进行比较的另一对象。</param>
        /// <returns>若 <paramref name="obj"/> 为 <see cref="WaveformParameters"/> 且与当前实例相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public override bool Equals(object? obj) =>
            (obj is WaveformParameters other) && this.Equals(other);

        /// <summary>
        /// 获取当前实例的哈希代码。
        /// </summary>
        /// <returns>当前实例的哈希代码。</returns>
        public override int GetHashCode() => this.GetBinaryHashCode();

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
        public static bool operator !=(WaveformParameters left, WaveformParameters right) => !left.Equals(right);
    }
}
