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
}
