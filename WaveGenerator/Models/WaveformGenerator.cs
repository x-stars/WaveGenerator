﻿using System;
using XstarS.WaveGenerator.WaveAudio;
using XstarS.WaveGenerator.Waveforms;

namespace XstarS.WaveGenerator.Models
{
    /// <summary>
    /// 提供生成特定频率的波形声音的方法。
    /// </summary>
    public static class WaveformGenerator
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
            WaveformParameters parameters, double durationSeconds)
        {
            WaveformGenerator.GenerateWave(waveWriter, parameters,
                ((int)waveWriter.SampleInfo.Channels).InitializeArray(_ => true), durationSeconds);
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
            WaveformParameters parameters, bool[] channelEnables, double durationSeconds)
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
            if (channelEnables.Length != (int)waveWriter.SampleInfo.Channels)
            {
                throw new ArgumentException(
                    new ArgumentException().Message, nameof(channelEnables));
            }

            var waveFunc = WaveformFunctions.Create(parameters);
            var count = (int)(durationSeconds * (int)waveWriter.SampleRate);
            foreach (var index in 0..count)
            {
                var time = (double)index / (int)waveWriter.SampleRate;
                var waveformValue = waveFunc(time);
                var channels = (int)waveWriter.SampleInfo.Channels;
                var waveformValues = channels.InitializeArray(
                    channel => channelEnables[channel] ? waveformValue : 0.0);
                var sample = WaveSample.FromWaveforms(waveWriter.SampleInfo, waveformValues);
                waveWriter.WriteSample(sample);
            }
            waveWriter.UpdateChunkSize();
        }
    }
}
