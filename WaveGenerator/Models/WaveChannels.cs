namespace XstarS.WaveGenerator.Models
{
    /// <summary>
    /// 表示波形声音的声道数量。
    /// </summary>
    public enum WaveChannels : short
    {
        /// <summary>
        /// 表示单声道。
        /// </summary>
        Mono = 1,
        /// <summary>
        /// 表示立体声。
        /// </summary>
        Stereo = 2,
        /// <summary>
        /// 表示 5.1 环绕声。
        /// </summary>
        Surround51 = 6,
        /// <summary>
        /// 表示 7.1 环绕声。
        /// </summary>
        Surround71 = 8
    }
}
