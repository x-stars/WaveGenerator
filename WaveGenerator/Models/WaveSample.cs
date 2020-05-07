using System;
using XstarS.Collections.Generic;

namespace XstarS.WaveGenerator.Models
{
    using BytesComparer = ArrayEqualityComparer<byte>;

    /// <summary>
    /// 表示一个波形声音的采样点。
    /// </summary>
    public struct WaveSample : IEquatable<WaveSample>
    {
        /// <summary>
        /// 表示用于 <see cref="WaveSample.Data"/> 的相等比较器。
        /// </summary>
        private static readonly BytesComparer DataComparer = BytesComparer.Default;

        /// <summary>
        /// 使用采样点数据初始化 <see cref="WaveSample"/> 结构的新实例。
        /// </summary>
        /// <param name="channels">采样点的声道数量。</param>
        /// <param name="bitDepth">采样点的采样位深度。</param>
        /// <param name="data">采样点数据的字节数组。</param>
        private WaveSample(WaveChannels channels, WaveBitDepth bitDepth, byte[] data)
        {
            this.Channels = channels;
            this.BitDepth = bitDepth;
            this.Data = data;
        }

        /// <summary>
        /// 获取当前采样点的声道数量。
        /// </summary>
        public WaveChannels Channels { get; }

        /// <summary>
        /// 获取当前采样点的采样位深度。
        /// </summary>
        public WaveBitDepth BitDepth { get; }

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
        [CLSCompliant(false)]
        public static unsafe WaveSample Int8(params sbyte[] values)
        {
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            var channels = values.Length;
            var data = new byte[channels * sizeof(sbyte)];
            fixed (sbyte* pValue = values)
            {
                fixed (byte* pData = data)
                {
                    var tpData = (sbyte*)pData;
                    for (int i = 0; i < channels; i++)
                    {
                        tpData[i] = pValue[i];
                    }
                }
            }
            return new WaveSample((WaveChannels)channels, WaveBitDepth.Int8, data);
        }

        /// <summary>
        /// 以指定的 16 位整数序列为各声道的采样值创建 <see cref="WaveSample"/> 结构的实例。
        /// </summary>
        /// <param name="values">作为各声道的采样值的 16 位整数序列。</param>
        /// <returns>创建得到的 <see cref="WaveSample"/> 结构的实例。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="values"/> 为 <see langword="null"/>。</exception>
        public static unsafe WaveSample Int16(params short[] values)
        {
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            var channels = values.Length;
            var data = new byte[channels * sizeof(short)];
            fixed (short* pValue = values)
            {
                fixed (byte* pData = data)
                {
                    var tpData = (short*)pData;
                    for (int i = 0; i < channels; i++)
                    {
                        tpData[i] = pValue[i];
                    }
                }
            }
            return new WaveSample((WaveChannels)channels, WaveBitDepth.Int16, data);
        }

        /// <summary>
        /// 以指定的 24 位整数序列为各声道的采样值创建 <see cref="WaveSample"/> 结构的实例。
        /// </summary>
        /// <param name="values">作为各声道的采样值的 24 位整数序列。</param>
        /// <returns>创建得到的 <see cref="WaveSample"/> 结构的实例。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="values"/> 为 <see langword="null"/>。</exception>
        public static unsafe WaveSample Int24(params Int24[] values)
        {
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            var channels = values.Length;
            var data = new byte[channels * sizeof(Int24)];
            fixed (Int24* pValue = values)
            {
                fixed (byte* pData = data)
                {
                    var tpData = (Int24*)pData;
                    for (int i = 0; i < channels; i++)
                    {
                        tpData[i] = pValue[i];
                    }
                }
            }
            return new WaveSample((WaveChannels)channels, WaveBitDepth.Int24, data);
        }

        /// <summary>
        /// 以指定的 32 位浮点数序列为各声道的采样值创建 <see cref="WaveSample"/> 结构的实例。
        /// </summary>
        /// <param name="values">作为各声道的采样值的 32 位浮点数序列。</param>
        /// <returns>创建得到的 <see cref="WaveSample"/> 结构的实例。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="values"/> 为 <see langword="null"/>。</exception>
        public static unsafe WaveSample Float32(params float[] values)
        {
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            var channels = values.Length;
            var data = new byte[channels * sizeof(float)];
            fixed (float* pValue = values)
            {
                fixed (byte* pData = data)
                {
                    var tpData = (float*)pData;
                    for (int i = 0; i < channels; i++)
                    {
                        tpData[i] = pValue[i];
                    }
                }
            }
            return new WaveSample((WaveChannels)channels, WaveBitDepth.Float32, data);
        }

        /// <summary>
        /// 指示当前 <see cref="WaveSample"/> 是否等于另一 <see cref="WaveSample"/>。
        /// </summary>
        /// <param name="other">要当前实例进行比较的另一 <see cref="WaveSample"/>。</param>
        /// <returns>若 <paramref name="other"/> 与当前实例相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public bool Equals(WaveSample other)
        {
            return (this.Channels == other.Channels) &&
                   (this.BitDepth == other.BitDepth) &&
                   WaveSample.DataComparer.Equals(this.Data, other.Data);
        }

        /// <summary>
        /// 指示当前 <see cref="WaveSample"/> 是否等于另一对象。
        /// </summary>
        /// <param name="obj">要当前实例进行比较的另一对象。</param>
        /// <returns>若 <paramref name="obj"/> 为 <see cref="WaveSample"/> 且与当前实例相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public override bool Equals(object obj)
        {
            return obj is WaveSample sample && this.Equals(sample);
        }

        /// <summary>
        /// 获取当前实例的哈希代码。
        /// </summary>
        /// <returns>当前实例的哈希代码。</returns>
        public override int GetHashCode()
        {
            var hashCode = -223416172;
            hashCode = hashCode * -1521134295 + this.Channels.GetHashCode();
            hashCode = hashCode * -1521134295 + this.BitDepth.GetHashCode();
            hashCode = hashCode * -1521134295 + WaveSample.DataComparer.GetHashCode(this.Data);
            return hashCode;
        }

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
        public static bool operator !=(WaveSample left, WaveSample right) => !(left == right);
    }
}
