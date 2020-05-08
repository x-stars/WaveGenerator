using System;

namespace XstarS.WaveGenerator.Models
{
    /// <summary>
    /// 封装计算指定幅度和频率的波形在指定时间的函数值的函数。
    /// </summary>
    /// <param name="amplitude">波形的幅度。</param>
    /// <param name="frequency">波形的频率。</param>
    /// <param name="time">当前时间。</param>
    /// <returns>指定幅度和频率的波形在当前时间的函数值。</returns>
    internal delegate double WaveFunction(double amplitude, double frequency, double time);

    /// <summary>
    /// 提供已知波形的函数。
    /// </summary>
    internal static class WaveFunctions
    {
        /// <summary>
        /// 表示正弦波的函数。
        /// </summary>
        /// <param name="amplitude">波形的幅度。</param>
        /// <param name="frequency">波形的频率。</param>
        /// <param name="time">当前时间。</param>
        /// <returns>在指定频率与幅度的正弦波中，当前时间对应的函数值。</returns>
        internal static double Sine(double amplitude, double frequency, double time) =>
            amplitude * Math.Sin(2.0 * Math.PI * frequency * time);

        /// <summary>
        /// 表示方波的函数。
        /// </summary>
        /// <param name="amplitude">波形的幅度。</param>
        /// <param name="frequency">波形的频率。</param>
        /// <param name="time">当前时间。</param>
        /// <returns>在指定频率与幅度的方波中，当前时间对应的函数值。</returns>
        internal static double Square(double amplitude, double frequency, double time) =>
            amplitude * (time < 0 ? -1.0 : 1.0) * ((Math.Abs(time) / (1.0 / frequency) % 1.0 < 0.5) ? 1.0 : -1.0);

        /// <summary>
        /// 表示三角波的函数。
        /// </summary>
        /// <param name="amplitude">波形的幅度。</param>
        /// <param name="frequency">波形的频率。</param>
        /// <param name="time">当前时间。</param>
        /// <returns>在指定频率与幅度的三角波中，当前时间对应的函数值。</returns>
        internal static double Triangle(double amplitude, double frequency, double time) =>
            amplitude * WaveFunctions.Square(1.0, frequency, time + 0.25 / frequency) *
            WaveFunctions.Sawtooth(1.0, 2.0 * frequency, time + 0.5 / (2.0 * frequency));

        /// <summary>
        /// 表示锯齿波的函数。
        /// </summary>
        /// <param name="amplitude">波形的幅度。</param>
        /// <param name="frequency">波形的频率。</param>
        /// <param name="time">当前时间。</param>
        /// <returns>在指定频率与幅度的锯齿波中，当前时间对应的函数值。</returns>
        internal static double Sawtooth(double amplitude, double frequency, double time) =>
            amplitude * (time < 0 ? -1.0 : 1.0) * (Math.Abs(time) / (1.0 / frequency) % 1.0 * 2.0 - 1.0);
    }
}
