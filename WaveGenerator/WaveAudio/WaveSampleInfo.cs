using System;
using XstarS.Runtime;
using int24 = XstarS.Int24;

namespace XstarS.WaveGenerator.WaveAudio
{
    /// <summary>
    /// 表示波形声音的采样点的结构参数。
    /// </summary>
    [Serializable]
    public readonly struct WaveSampleInfo : IEquatable<WaveSampleInfo>
    {
        /// <summary>
        /// 使用采样点结构参数初始化 <see cref="WaveSampleInfo"/> 结构的新实例。
        /// </summary>
        /// <param name="format">采样点的波形声音格式。</param>
        /// <param name="bitDepth">采样点的采样位深度。</param>
        /// <param name="channels">采样点的声道数量。</param>
        private WaveSampleInfo(
            WaveFormat format, WaveBitDepth bitDepth, WaveChannels channels)
        {
            this.Format = format;
            this.BitDepth = bitDepth;
            this.Channels = channels;
        }

        /// <summary>
        /// 获取当前采样点的波形声音格式。
        /// </summary>
        public WaveFormat Format { get; }

        /// <summary>
        /// 获取当前采样点的采样位深度。
        /// </summary>
        public WaveBitDepth BitDepth { get; }

        /// <summary>
        /// 获取当前采样点的声道数量。
        /// </summary>
        public WaveChannels Channels { get; }

        /// <summary>
        /// 获取当前采样点的以字节为单位的大小。
        /// </summary>
        public int SampleSize => (int)this.Channels * ((int)this.BitDepth / 8);

        /// <summary>
        /// 以指定的声道数量创建 8 位整数采样的 <see cref="WaveSampleInfo"/> 结构的实例。
        /// </summary>
        /// <param name="channels">采样点的声道数量。</param>
        /// <returns>创建得到的 <see cref="WaveSampleInfo"/> 结构的实例。</returns>
        public static WaveSampleInfo Int8(WaveChannels channels) =>
            new WaveSampleInfo(WaveFormat.PCM, WaveBitDepth.Bit8, channels);

        /// <summary>
        /// 以指定的声道数量创建 16 位整数采样的 <see cref="WaveSampleInfo"/> 结构的实例。
        /// </summary>
        /// <param name="channels">采样点的声道数量。</param>
        /// <returns>创建得到的 <see cref="WaveSampleInfo"/> 结构的实例。</returns>
        public static WaveSampleInfo Int16(WaveChannels channels) =>
            new WaveSampleInfo(WaveFormat.PCM, WaveBitDepth.Bit16, channels);

        /// <summary>
        /// 以指定的声道数量创建 24 位整数采样的 <see cref="WaveSampleInfo"/> 结构的实例。
        /// </summary>
        /// <param name="channels">采样点的声道数量。</param>
        /// <returns>创建得到的 <see cref="WaveSampleInfo"/> 结构的实例。</returns>
        public static WaveSampleInfo Int24(WaveChannels channels) =>
            new WaveSampleInfo(WaveFormat.PCM, WaveBitDepth.Bit24, channels);

        /// <summary>
        /// 以指定的声道数量创建 32 位整数采样的 <see cref="WaveSampleInfo"/> 结构的实例。
        /// </summary>
        /// <param name="channels">采样点的声道数量。</param>
        /// <returns>创建得到的 <see cref="WaveSampleInfo"/> 结构的实例。</returns>
        public static WaveSampleInfo Int32(WaveChannels channels) =>
            new WaveSampleInfo(WaveFormat.PCM, WaveBitDepth.Bit32, channels);

        /// <summary>
        /// 以指定的声道数量创建 32 位浮点数采样的 <see cref="WaveSampleInfo"/> 结构的实例。
        /// </summary>
        /// <param name="channels">采样点的声道数量。</param>
        /// <returns>创建得到的 <see cref="WaveSampleInfo"/> 结构的实例。</returns>
        public static WaveSampleInfo Float32(WaveChannels channels) =>
            new WaveSampleInfo(WaveFormat.IEEEFloat, WaveBitDepth.Bit32, channels);

        /// <summary>
        /// 获取当前采样点对应幅度为 1 的波形的函数值的采样值。
        /// </summary>
        /// <param name="waveformValue">幅度为 1 的波形的函数值。</param>
        /// <returns>当前采样点对应 <paramref name="waveformValue"/> 的采样值。</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="waveformValue"/> 的绝对值大于 1；或者当前实例中的枚举值不为有效值；
        /// 或者 <see cref="WaveSampleInfo.Format"/> 为 <see cref="WaveFormat.IEEEFloat"/>，
        /// 但 <see cref="WaveSampleInfo.BitDepth"/> 不为 <see cref="WaveBitDepth.Bit32"/>。</exception>
        public double GetSampleValue(double waveformValue)
        {
            if ((waveformValue < -1.0) || (waveformValue > 1.0))
            {
                throw new ArgumentOutOfRangeException(nameof(waveformValue));
            }

            return this.Format switch
            {
                WaveFormat.PCM => this.BitDepth switch
                {
                    WaveBitDepth.Bit8 => (waveformValue * sbyte.MaxValue) - sbyte.MinValue,
                    WaveBitDepth.Bit16 => waveformValue * short.MaxValue,
                    WaveBitDepth.Bit24 => waveformValue * int24.MaxValue,
                    WaveBitDepth.Bit32 => waveformValue * int.MaxValue,
                    _ => throw new ArgumentOutOfRangeException(nameof(this.BitDepth))
                },
                WaveFormat.IEEEFloat => this.BitDepth switch
                {
                    WaveBitDepth.Bit32 => (float)waveformValue,
                    _ => throw new ArgumentOutOfRangeException(nameof(this.BitDepth))
                },
                _ => throw new ArgumentOutOfRangeException(nameof(this.Format))
            };
        }

        /// <summary>
        /// 指示当前 <see cref="WaveSampleInfo"/> 是否等于另一 <see cref="WaveSampleInfo"/>。
        /// </summary>
        /// <param name="other">要当前实例进行比较的另一 <see cref="WaveSampleInfo"/>。</param>
        /// <returns>若 <paramref name="other"/> 与当前实例相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public bool Equals(WaveSampleInfo other) => this.BinaryEquals(other);

        /// <summary>
        /// 指示当前 <see cref="WaveSampleInfo"/> 是否等于另一对象。
        /// </summary>
        /// <param name="obj">要当前实例进行比较的另一对象。</param>
        /// <returns>若 <paramref name="obj"/> 为 <see cref="WaveSampleInfo"/> 且与当前实例相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public override bool Equals(object obj) =>
            (obj is WaveSampleInfo other) && this.Equals(other);

        /// <summary>
        /// 获取当前实例的哈希代码。
        /// </summary>
        /// <returns>当前实例的哈希代码。</returns>
        public override int GetHashCode() => this.GetBinaryHashCode();

        /// <summary>
        /// 比较两个 <see cref="WaveSampleInfo"/> 是否相等。
        /// </summary>
        /// <param name="left">要进行比较的第一个 <see cref="WaveSampleInfo"/>。</param>
        /// <param name="right">要进行比较的第二个 <see cref="WaveSampleInfo"/>。</param>
        /// <returns>若 <paramref name="left"/> 与 <paramref name="right"/> 相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public static bool operator ==(WaveSampleInfo left, WaveSampleInfo right) => left.Equals(right);

        /// <summary>
        /// 比较两个 <see cref="WaveSampleInfo"/> 是否不等。
        /// </summary>
        /// <param name="left">要进行比较的第一个 <see cref="WaveSampleInfo"/>。</param>
        /// <param name="right">要进行比较的第二个 <see cref="WaveSampleInfo"/>。</param>
        /// <returns>若 <paramref name="left"/> 与 <paramref name="right"/> 不等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public static bool operator !=(WaveSampleInfo left, WaveSampleInfo right) => !left.Equals(right);
    }
}
