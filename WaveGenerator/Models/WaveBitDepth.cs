namespace XstarS.WaveGenerator.Models
{
    /// <summary>
    /// 表示波形声音的采样位深度。
    /// </summary>
    public enum WaveBitDepth : short
    {
        /// <summary>
        /// 表示 8 位整数采样。
        /// </summary>
        Int8 = 8,
        /// <summary>
        /// 表示 16 位整数采样。
        /// </summary>
        Int16 = 16,
        /// <summary>
        /// 表示 24 位整数采样。
        /// </summary>
        Int24 = 24,
        /// <summary>
        /// 表示 32 位浮点数采样。
        /// </summary>
        Float32 = 32
    }
}
