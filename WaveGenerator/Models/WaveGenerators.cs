using System;

namespace XstarS.WaveGenerator.Models
{
    /// <summary>
    /// 提供生成特定频率的波形声音的方法。
    /// </summary>
    public static class WaveGenerators
    {
        /// <summary>
        /// 生成指定持续时间的具有指定波形参数的波形声音，并输出到指定的波形声音流。
        /// </summary>
        /// <param name="waveWriter">要输出波形的流。</param>
        /// <param name="parameters">波形声音的参数。</param>
        /// <param name="durationSeconds">波形声音的持续时间。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="waveWriter"/> 为 <see langword="null"/>，
        /// 或 <paramref name="parameters"/> 为默认值。</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="durationSeconds"/> 为负数，或枚举值不为定义的值。</exception>
        public static void GenerateWave(WaveStreamWriter waveWriter,
            WaveParameters parameters, double durationSeconds)
        {
            WaveGenerators.GenerateWave(waveWriter, parameters,
                ((int)waveWriter.Channels).InitializeArray(_ => true), durationSeconds);
        }

        /// <summary>
        /// 在指定的声道生成指定持续时间的具有指定波形参数的波形声音，并输出到指定的波形声音流。
        /// </summary>
        /// <param name="waveWriter">要输出波形的流。</param>
        /// <param name="parameters">波形声音的参数。</param>
        /// <param name="channelEnables">指定输出声音的声道。</param>
        /// <param name="durationSeconds">波形声音的持续时间。</param>
        /// <exception cref="ArgumentNullException"><paramref name="waveWriter"/>
        /// 或 <paramref name="channelEnables"/> 为 <see langword="null"/>，
        /// 或 <paramref name="parameters"/> 为默认值。</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="durationSeconds"/> 为负数，或枚举值不为定义的值。</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="channelEnables"/> 的数量与声道数量不匹配。</exception>
        public static void GenerateWave(WaveStreamWriter waveWriter,
            WaveParameters parameters, bool[] channelEnables, double durationSeconds)
        {
            if (waveWriter is null)
            {
                throw new ArgumentNullException(nameof(waveWriter));
            }
            if (channelEnables is null)
            {
                throw new ArgumentNullException(nameof(channelEnables));
            }
            if (parameters == default)
            {
                throw new ArgumentNullException(nameof(parameters));
            }
            if (durationSeconds < 0.0)
            {
                throw new ArgumentOutOfRangeException(nameof(durationSeconds));
            }
            if (channelEnables.Length != (int)waveWriter.Channels)
            {
                throw new ArgumentException(
                    new ArgumentException().Message, nameof(channelEnables));
            }

            var peek = parameters.Amplitude;
            switch (waveWriter.BitDepth)
            {
                case WaveBitDepth.Bit8: peek *= sbyte.MaxValue; break;
                case WaveBitDepth.Bit16: peek *= short.MaxValue; break;
                case WaveBitDepth.Bit24: peek *= Int24.MaxValue; break;
                case WaveBitDepth.Bit32:
                    switch (waveWriter.Format)
                    {
                        case WaveFormat.PCM: peek *= int.MaxValue; break;
                        case WaveFormat.IEEEFloat: peek *= 1.0; break;
                        default: throw new ArgumentOutOfRangeException();
                    }
                    break;
                default: throw new ArgumentOutOfRangeException();
            }

            var count = (int)(durationSeconds * (int)waveWriter.SampleRate);

            var waveFunc = default(WaveFunction);
            switch (parameters.Waveform)
            {
                case Waveform.Sine: waveFunc = WaveFunctions.Sine; break;
                case Waveform.Square: waveFunc = WaveFunctions.Square; break;
                case Waveform.Triangle: waveFunc = WaveFunctions.Triangle; break;
                case Waveform.Sawtooth: waveFunc = WaveFunctions.Sawtooth; break;
                default: throw new ArgumentOutOfRangeException();
            }

            for (int i = 0; i < count; i++)
            {
                var channels = (int)waveWriter.Channels;
                var amplitude = parameters.Amplitude * peek;
                var frequency = parameters.Frequency;
                var time = (double)i / (int)waveWriter.SampleRate;
                var value = waveFunc(amplitude, frequency, time);

                var sample = default(WaveSample);
                switch (waveWriter.BitDepth)
                {
                    case WaveBitDepth.Bit8:
                        sample = WaveSample.Int8(channels.InitializeArray(
                            channel => (byte)((channelEnables[channel] ? value : 0.0) - sbyte.MinValue)));
                        break;
                    case WaveBitDepth.Bit16:
                        sample = WaveSample.Int16(channels.InitializeArray(
                            channel => (short)(channelEnables[channel] ? value : 0.0)));
                        break;
                    case WaveBitDepth.Bit24:
                        sample = WaveSample.Int24(channels.InitializeArray(
                            channel => (Int24)(channelEnables[channel] ? value : 0.0)));
                        break;
                    case WaveBitDepth.Bit32:
                        switch (waveWriter.Format)
                        {
                            case WaveFormat.PCM:
                                sample = WaveSample.Int32(channels.InitializeArray(
                                    channel => (int)(channelEnables[channel] ? value : 0.0)));
                                break;
                            case WaveFormat.IEEEFloat:
                                sample = WaveSample.Float32(channels.InitializeArray(
                                    channel => (float)(channelEnables[channel] ? value : 0.0)));
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                waveWriter.WriteSample(sample);
            }
            waveWriter.UpdateChunkSize();
        }
    }
}
