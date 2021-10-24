using System;
using XstarS.Runtime;
using int24 = XstarS.Int24;

namespace XstarS.WaveGenerator.WaveAudio
{
    /// <summary>
    /// 表示一个波形声音的采样点。
    /// </summary>
    [Serializable]
    public readonly struct WaveSample : IEquatable<WaveSample>
    {
        /// <summary>
        /// 使用采样点数据初始化 <see cref="WaveSample"/> 结构的新实例。
        /// </summary>
        /// <param name="info">采样点的结构参数。</param>
        /// <param name="data">采样点数据的字节数组。</param>
        private WaveSample(WaveSampleInfo info, byte[] data)
        {
            this.Info = info;
            this.Data = data;
        }

        /// <summary>
        /// 获取当前采样点的结构参数。
        /// </summary>
        public WaveSampleInfo Info { get; }

        /// <summary>
        /// 获取当前采样点的字节序列数据。
        /// </summary>
        public byte[] Data { get; }

        /// <summary>
        /// 获取当前采样点的字节序列的长度。
        /// </summary>
        public int Length => this.Data.Length;

        /// <summary>
        /// 以指定的 8 位整数序列为各声道的采样值创建 <see cref="WaveSample"/> 结构的实例。
        /// </summary>
        /// <param name="values">作为各声道的采样值的 8 位整数序列。</param>
        /// <returns>创建得到的 <see cref="WaveSample"/> 结构的实例。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="values"/> 为 <see langword="null"/>。</exception>
        public static WaveSample Int8(params byte[] values)
        {
            var data = WaveSample.ToByteArray(values);
            var channels = (WaveChannels)values.Length;
            var info = WaveSampleInfo.Int8(channels);
            return new WaveSample(info, data);
        }

        /// <summary>
        /// 以指定的 16 位整数序列为各声道的采样值创建 <see cref="WaveSample"/> 结构的实例。
        /// </summary>
        /// <param name="values">作为各声道的采样值的 16 位整数序列。</param>
        /// <returns>创建得到的 <see cref="WaveSample"/> 结构的实例。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="values"/> 为 <see langword="null"/>。</exception>
        public static WaveSample Int16(params short[] values)
        {
            var data = WaveSample.ToByteArray(values);
            var channels = (WaveChannels)values.Length;
            var info = WaveSampleInfo.Int16(channels);
            return new WaveSample(info, data);
        }

        /// <summary>
        /// 以指定的 24 位整数序列为各声道的采样值创建 <see cref="WaveSample"/> 结构的实例。
        /// </summary>
        /// <param name="values">作为各声道的采样值的 24 位整数序列。</param>
        /// <returns>创建得到的 <see cref="WaveSample"/> 结构的实例。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="values"/> 为 <see langword="null"/>。</exception>
        public static WaveSample Int24(params int24[] values)
        {
            var data = WaveSample.ToByteArray(values);
            var channels = (WaveChannels)values.Length;
            var info = WaveSampleInfo.Int24(channels);
            return new WaveSample(info, data);
        }

        /// <summary>
        /// 以指定的 32 位整数序列为各声道的采样值创建 <see cref="WaveSample"/> 结构的实例。
        /// </summary>
        /// <param name="values">作为各声道的采样值的 32 位整数序列。</param>
        /// <returns>创建得到的 <see cref="WaveSample"/> 结构的实例。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="values"/> 为 <see langword="null"/>。</exception>
        public static WaveSample Int32(params int[] values)
        {
            var data = WaveSample.ToByteArray(values);
            var channels = (WaveChannels)values.Length;
            var info = WaveSampleInfo.Int32(channels);
            return new WaveSample(info, data);
        }

        /// <summary>
        /// 以指定的 32 位浮点数序列为各声道的采样值创建 <see cref="WaveSample"/> 结构的实例。
        /// </summary>
        /// <param name="values">作为各声道的采样值的 32 位浮点数序列。</param>
        /// <returns>创建得到的 <see cref="WaveSample"/> 结构的实例。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="values"/> 为 <see langword="null"/>。</exception>
        public static WaveSample Float32(params float[] values)
        {
            var data = WaveSample.ToByteArray(values);
            var channels = (WaveChannels)values.Length;
            var info = WaveSampleInfo.Float32(channels);
            return new WaveSample(info, data);
        }

        /// <summary>
        /// 以指定的幅度为 1 的波形值序列为各声道的采样值创建 <see cref="WaveSample"/> 结构的实例。
        /// </summary>
        /// <param name="info">采样点的结构参数。</param>
        /// <param name="values">作为各声道的采样值的幅度为 1 的波形值序列。</param>
        /// <returns>创建得到的 <see cref="WaveSample"/> 结构的实例。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="values"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="info"/> 中的枚举值不为有效值；
        /// 或者 <see cref="WaveSampleInfo.Format"/> 为 <see cref="WaveFormat.IEEEFloat"/>，
        /// 但 <see cref="WaveSampleInfo.BitDepth"/> 不为 <see cref="WaveBitDepth.Bit32"/>。</exception>
        public static WaveSample FromWaveforms(WaveSampleInfo info, params double[] values)
        {
            return info.Format switch
            {
                WaveFormat.PCM => info.BitDepth switch
                {
                    WaveBitDepth.Bit8 => WaveSample.Int8(Array.ConvertAll(
                        values, value => (byte)info.GetSampleValue(value))),
                    WaveBitDepth.Bit16 => WaveSample.Int16(Array.ConvertAll(
                        values, value => (short)info.GetSampleValue(value))),
                    WaveBitDepth.Bit24 => WaveSample.Int24(Array.ConvertAll(
                        values, value => (int24)info.GetSampleValue(value))),
                    WaveBitDepth.Bit32 => WaveSample.Int32(Array.ConvertAll(
                        values, value => (int)info.GetSampleValue(value))),
                    _ => throw new ArgumentOutOfRangeException(nameof(info))
                },
                WaveFormat.IEEEFloat => info.BitDepth switch
                {
                    WaveBitDepth.Bit32 => WaveSample.Float32(Array.ConvertAll(
                        values, value => (float)info.GetSampleValue(value))),
                    _ => throw new ArgumentOutOfRangeException(nameof(info))
                },
                _ => throw new ArgumentOutOfRangeException(nameof(info))
            };
        }

        /// <summary>
        /// 将指定的各声道采样值的数组转换为等效的字节数据序列。
        /// </summary>
        /// <param name="values">各声道的采样值的数据序列。</param>
        /// <returns>转换得到的等效的字节数据序列。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="values"/> 为 <see langword="null"/>。</exception>
        private static unsafe byte[] ToByteArray<T>(params T[] values) where T : unmanaged
        {
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            var channels = values.Length;
            var data = new byte[channels * sizeof(T)];
            fixed (T* pValue = values)
            {
                fixed (byte* pData = data)
                {
                    var tpData = (T*)pData;
                    for (int i = 0; i < channels; i++)
                    {
                        tpData[i] = pValue[i];
                    }
                }
            }
            return data;
        }

        /// <summary>
        /// 指示当前 <see cref="WaveSample"/> 是否等于另一 <see cref="WaveSample"/>。
        /// </summary>
        /// <param name="other">要当前实例进行比较的另一 <see cref="WaveSample"/>。</param>
        /// <returns>若 <paramref name="other"/> 与当前实例相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public bool Equals(WaveSample other) =>
            this.Info.BinaryEquals(other.Info) && this.Data.BinaryEquals(other.Data);

        /// <summary>
        /// 指示当前 <see cref="WaveSample"/> 是否等于另一对象。
        /// </summary>
        /// <param name="obj">要当前实例进行比较的另一对象。</param>
        /// <returns>若 <paramref name="obj"/> 为 <see cref="WaveSample"/> 且与当前实例相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public override bool Equals(object obj) =>
            (obj is WaveSample sample) && this.Equals(sample);

        /// <summary>
        /// 获取当前实例的哈希代码。
        /// </summary>
        /// <returns>当前实例的哈希代码。</returns>
        public override int GetHashCode() =>
            this.Info.GetBinaryHashCode() * -1521134295 + this.Data.GetBinaryHashCode();

        /// <summary>
        /// 比较两个 <see cref="WaveSample"/> 是否相等。
        /// </summary>
        /// <param name="left">要进行比较的第一个 <see cref="WaveSample"/>。</param>
        /// <param name="right">要进行比较的第二个 <see cref="WaveSample"/>。</param>
        /// <returns>若 <paramref name="left"/> 与 <paramref name="right"/> 相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public static bool operator ==(WaveSample left, WaveSample right) => left.Equals(right);

        /// <summary>
        /// 比较两个 <see cref="WaveSample"/> 是否不等。
        /// </summary>
        /// <param name="left">要进行比较的第一个 <see cref="WaveSample"/>。</param>
        /// <param name="right">要进行比较的第二个 <see cref="WaveSample"/>。</param>
        /// <returns>若 <paramref name="left"/> 与 <paramref name="right"/> 不等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public static bool operator !=(WaveSample left, WaveSample right) => !left.Equals(right);
    }
}
