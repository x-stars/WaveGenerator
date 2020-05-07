using System;
using System.IO;
using System.Text;
using XstarS.IO;

namespace XstarS.WaveGenerator.Models
{
    /// <summary>
    /// 表示波形声音的写入流。
    /// </summary>
    public class WaveStreamWriter : IDisposable
    {
        /// <summary>
        /// 表示 PCM 音频格式的常数值。
        /// </summary>
        private const short AudioFormatPCM = 1;

        /// <summary>
        /// 表示波形声音的区块 ID 常数值。
        /// </summary>
        private static readonly byte[] ChunkID = Encoding.ASCII.GetBytes("RIFF");

        /// <summary>
        /// 表示波形声音的文件格式常数值。
        /// </summary>
        private static readonly byte[] Format = Encoding.ASCII.GetBytes("WAVE");

        /// <summary>
        /// 表示波形声音的格式区块 ID 常数值。
        /// </summary>
        private static readonly byte[] FmtChunkID = Encoding.ASCII.GetBytes("fmt ");

        /// <summary>
        /// 表示波形声音的数据区块 ID 常数值。
        /// </summary>
        private static readonly byte[] DataChunkID = Encoding.ASCII.GetBytes("data");

        /// <summary>
        /// 表示当前对象是否已经被释放。
        /// </summary>
        private bool IsDisposed = false;

        /// <summary>
        /// 以要写入的流初始化 <see cref="WaveStreamWriter"/> 类的新实例。
        /// </summary>
        /// <param name="stream">要写入波形声音的流。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="stream"/> 为 <see langword="null"/>。</exception>
        public WaveStreamWriter(Stream stream) : this(
            stream, WaveChannels.Stereo, WaveSampleRate.Hz44100, WaveBitDepth.Int16)
        {
        }

        /// <summary>
        /// 以要写入的流和波形声音参数初始化 <see cref="WaveStreamWriter"/> 类的新实例。
        /// </summary>
        /// <param name="stream">要写入波形声音的流。</param>
        /// <param name="channels">波形声音的声道数量。</param>
        /// <param name="sampleRate">波形声音的采样率。</param>
        /// <param name="bitDepth">波形声音的采样位深度。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="stream"/> 为 <see langword="null"/>。</exception>
        public WaveStreamWriter(Stream stream,
            WaveChannels channels = WaveChannels.Stereo,
            WaveSampleRate sampleRate = WaveSampleRate.Hz44100,
            WaveBitDepth bitDepth = WaveBitDepth.Int16)
        {
            if (stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            this.Stream = stream;
            this.Channels = channels;
            this.SampleRate = sampleRate;
            this.BitDepth = bitDepth;
            this.InitializeStream();
        }

        /// <summary>
        /// 获取当前正在写入的流。
        /// </summary>
        public Stream Stream { get; }

        /// <summary>
        /// 获取当前波形声音的声道数量。
        /// </summary>
        public WaveChannels Channels { get; }

        /// <summary>
        /// 获取当前波形声音的采样率。
        /// </summary>
        public WaveSampleRate SampleRate { get; }

        /// <summary>
        /// 获取当前波形声音的采样位深度。
        /// </summary>
        public WaveBitDepth BitDepth { get; }

        /// <summary>
        /// 获取当前波形声音的字节率。
        /// </summary>
        public int ByteRate => (int)this.Channels * (int)this.SampleRate * (int)this.BitDepth / 8;

        /// <summary>
        /// 获取当前波形声音的区块对齐。
        /// </summary>
        public short BlockAlign => (short)((int)this.Channels * (int)this.BitDepth / 8);

        /// <summary>
        /// 获取当前波形声音数据区块的大小。
        /// </summary>
        public int DataLength { get; private set; }

        /// <summary>
        /// 初始化当前正在写入的流。
        /// </summary>
        private void InitializeStream()
        {
            var stream = this.Stream;
            stream.Position = 0;

            stream.Write(WaveStreamWriter.ChunkID);
            stream.Write(WaveFileOffsets.DataBlocks - WaveFileOffsets.Format);
            stream.Write(WaveStreamWriter.Format);

            stream.Write(WaveStreamWriter.FmtChunkID);
            stream.Write(WaveFileOffsets.DataChunkID - WaveFileOffsets.AudioFormat);
            stream.Write(WaveStreamWriter.AudioFormatPCM);
            stream.Write(this.Channels);
            stream.Write(this.SampleRate);
            stream.Write(this.ByteRate);
            stream.Write(this.BlockAlign);
            stream.Write(this.BitDepth);

            stream.Write(WaveStreamWriter.DataChunkID);
            stream.Write(0);
        }

        /// <summary>
        /// 向当前波形声音写入一个采样点。
        /// </summary>
        /// <param name="sample">要写入的采样点。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="sample"/> 包含的数据为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="sample"/> 的声道数量或采样位深度与当前实例不匹配。</exception>
        public void WriteSample(WaveSample sample)
        {
            if (sample == default)
            {
                throw new ArgumentNullException(nameof(sample));
            }
            if ((sample.Channels != this.Channels) ||
                (sample.BitDepth != this.BitDepth))
            {
                throw new ArgumentException(
                    new ArgumentException().Message, nameof(sample));
            }

            this.Stream.Write(sample.Data, 0, sample.Length);
            this.DataLength += sample.Length;
        }

        /// <summary>
        /// 向当前波形声音写入一系列采样点。
        /// </summary>
        /// <param name="samples">要写入的一系列采样点。</param>
        /// <exception cref="ArgumentNullException"><paramref name="samples"/> 或
        /// <paramref name="samples"/> 包含的数据为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="samples"/> 的声道数量或采样位深度与当前实例不匹配。</exception>
        public void WriteSamples(WaveSample[] samples)
        {
            foreach (var sample in samples)
            {
                this.WriteSample(sample);
            }
        }

        /// <summary>
        /// 清除当前波形声音的所有采样点。
        /// </summary>
        public void ClearSamples()
        {
            this.Stream.SetLength(WaveFileOffsets.DataBlocks);
            this.DataLength = 0;
            this.UpdateChunkSize();
        }

        /// <summary>
        /// 更新流中区块大小的部分。
        /// </summary>
        public void UpdateChunkSize()
        {
            var stream = this.Stream;
            var position = stream.Position;

            stream.Position = WaveFileOffsets.ChunkSize;
            stream.Write(this.DataLength +
                WaveFileOffsets.DataBlocks - WaveFileOffsets.ChunkSize);

            stream.Position = WaveFileOffsets.DataChunkSize;
            stream.Write(this.DataLength);

            stream.Position = position;
        }

        /// <summary>
        /// 释放此实例占用的资源。
        /// </summary>
        public void Dispose() { this.Dispose(true); }

        /// <summary>
        /// 释放当前实例占用的非托管资源，并根据指示释放托管资源。
        /// </summary>
        /// <param name="disposing">指示是否释放托管资源。</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.IsDisposed)
            {
                if (disposing)
                {
                    this.Stream.Dispose();
                }

                this.IsDisposed = true;
            }
        }
    }
}
