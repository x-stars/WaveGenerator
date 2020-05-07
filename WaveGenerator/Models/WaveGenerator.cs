using System;

namespace XstarS.WaveGenerator.Models
{
    /// <summary>
    /// 提供生成特定频率的波形声音的方法。
    /// </summary>
    public static class WaveGenerator
    {
        /// <summary>
        /// 生成指定持续时间的指定波形参数的波形声音，并输出到指定的波形声音流。
        /// </summary>
        /// <param name="waveWriter">要输出波形的流。</param>
        /// <param name="parameters">波形声音的参数。</param>
        /// <param name="durationSeconds">波形声音的持续时间。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="waveWriter"/> 为 <see langword="null"/>，
        /// 或 <paramref name="parameters"/> 为默认值。</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="durationSeconds"/> 为负数。</exception>
        public static void GenerateWave(WaveStreamWriter waveWriter,
            WaveParameters parameters, double durationSeconds)
        {
            if (waveWriter is null)
            {
                throw new ArgumentNullException(nameof(waveWriter));
            }
            if (parameters == default)
            {
                throw new ArgumentNullException(nameof(parameters));
            }
            if (durationSeconds < 0.0)
            {
                throw new ArgumentOutOfRangeException(nameof(durationSeconds));
            }

            var peek = parameters.Amplitude;
            switch (waveWriter.BitDepth)
            {
                case WaveBitDepth.Int8: peek *= sbyte.MaxValue; break;
                case WaveBitDepth.Int16: peek *= short.MaxValue; break;
                case WaveBitDepth.Int24: peek *= Int24.MaxValue; break;
                case WaveBitDepth.Float32: peek *= float.MaxValue; break;
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
                    case WaveBitDepth.Int8:
                        sample = WaveSample.Int8(channels.InitializeArray(_ => (sbyte)value));
                        break;
                    case WaveBitDepth.Int16:
                        sample = WaveSample.Int16(channels.InitializeArray(_ => (short)value));
                        break;
                    case WaveBitDepth.Int24:
                        sample = WaveSample.Int24(channels.InitializeArray(_ => (Int24)value));
                        break;
                    case WaveBitDepth.Float32:
                        sample = WaveSample.Float32(channels.InitializeArray(_ => (float)value));
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
