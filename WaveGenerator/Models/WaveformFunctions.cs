using System;

namespace XstarS.WaveGenerator.Models
{
    /// <summary>
    /// 封装计算波形在指定时间的值的函数。
    /// </summary>
    /// <param name="time">当前时间，也即波形函数的自变量。</param>
    /// <returns>波形在 <paramref name="time"/> 处的函数值。</returns>
    public delegate double WaveformFunction(double time);

    /// <summary>
    /// 提供指定幅度、频率和相位的已知波形的函数。
    /// </summary>
    public static class WaveformFunctions
    {
        /// <summary>
        /// 表示标准参数的正弦波的函数。
        /// 此处所称的标准参数为：幅度 = 1，频率 = 1 / 2π，相位 0。
        /// </summary>
        /// <param name="time">当前时间，也即波形函数的自变量。</param>
        /// <returns>标准参数的正弦波在 <paramref name="time"/> 的函数值。</returns>
        public static double Sine(double time) => Math.Sin(time);

        /// <summary>
        /// 表示标准参数的方波的函数。
        /// 此处所称的标准参数为：幅度 = 1，频率 = 1 / 2π，相位 0。
        /// </summary>
        /// <param name="time">当前时间，也即波形函数的自变量。</param>
        /// <returns>标准参数的方波在 <paramref name="time"/> 的函数值。</returns>
        public static double Square(double time) =>
            Math.Sign(Math.Sign(time) * (Math.PI - Math.Abs(time % (2 * Math.PI))));

        /// <summary>
        /// 表示标准参数的三角波的函数。
        /// 此处所称的标准参数为：幅度 = 1，频率 = 1 / 2π，相位 0。
        /// </summary>
        /// <param name="time">当前时间，也即波形函数的自变量。</param>
        /// <returns>标准参数的三角波在 <paramref name="time"/> 的函数值。</returns>
        public static double Triangle(double time)
        {
            var shifted = Math.Abs((time - (Math.PI / 2)) % (2 * Math.PI)) - Math.PI;
            return (Math.Sign(shifted) * shifted / Math.PI * 2) - 1;
        }

        /// <summary>
        /// 表示标准参数的锯齿波的函数。
        /// 此处所称的标准参数为：幅度 = 1，频率 = 1 / 2π，相位 0。
        /// </summary>
        /// <param name="time">当前时间，也即波形函数的自变量。</param>
        /// <returns>标准参数的锯齿波在 <paramref name="time"/> 的函数值。</returns>
        public static double Sawtooth(double time) =>
            ((((time + Math.PI) % (2 * Math.PI)) + (2 * Math.PI)) % (2 * Math.PI) - Math.PI) / Math.PI;

        /// <summary>
        /// 创建指定幅度、频率和相位的特定波形的函数。
        /// </summary>
        /// <param name="waveform">波形的类型。</param>
        /// <param name="amplitude">波形的幅度。</param>
        /// <param name="frequency">波形的频率。</param>
        /// <param name="phase">波形的相位。</param>
        /// <returns>幅度为 <paramref name="amplitude"/>，频率为 <paramref name="frequency"/>，
        /// 相位为 <paramref name="phase"/> 的 <paramref name="waveform"/> 波形的函数。</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="waveform"/> 不表示有效的波形类型。</exception>
        public static WaveformFunction CreateWaveform(
            Waveform waveform, double amplitude, double frequency, double phase)
        {
            var function = waveform switch
            {
                Waveform.Sine => (WaveformFunction)WaveformFunctions.Sine,
                Waveform.Square => (WaveformFunction)WaveformFunctions.Square,
                Waveform.Triangle => (WaveformFunction)WaveformFunctions.Triangle,
                Waveform.Sawtooth => (WaveformFunction)WaveformFunctions.Sawtooth,
                _ => throw new ArgumentOutOfRangeException(nameof(waveform))
            };

            return time => amplitude * function((time * (2 * Math.PI) * frequency) + phase);
        }
    }
}
