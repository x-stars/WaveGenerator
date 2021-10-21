namespace XstarS.WaveGenerator.Waveforms
{
    /// <summary>
    /// 封装计算波形在指定时间的值的函数。
    /// </summary>
    /// <param name="time">当前时间，也即波形函数的自变量。</param>
    /// <returns>波形在 <paramref name="time"/> 处的函数值。</returns>
    public delegate double WaveformFunction(double time);
}
