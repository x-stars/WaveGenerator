namespace XstarS.WaveGenerator.Models
{
    /// <summary>
    /// 表示波形声音的采样位深度。
    /// </summary>
    public enum WaveBitDepth : short
    {
        /// <summary>
        /// 表示 8 位采样。
        /// </summary>
        Bit8 = 8,
        /// <summary>
        /// 表示 16 位采样。
        /// </summary>
        Bit16 = 16,
        /// <summary>
        /// 表示 24 位采样。
        /// </summary>
        Bit24 = 24,
        /// <summary>
        /// 表示 32 位采样。
        /// </summary>
        Bit32 = 32
    }
}
