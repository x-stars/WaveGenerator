namespace XstarS.WaveGenerator.WaveAudio
{
    /// <summary>
    /// 表示波形声音文件各区块的偏移量。
    /// </summary>
    internal static class WaveFileOffsets
    {
        /// <summary>
        /// 表示区块 ID 的偏移量。
        /// </summary>
        internal const int ChunkID = 0;
        /// <summary>
        /// 表示总区块大小的偏移量。
        /// </summary>
        internal const int ChunkSize = 4;
        /// <summary>
        /// 表示文件格式 ID 的偏移量。
        /// </summary>
        internal const int FormatID = 8;
        /// <summary>
        /// 表示格式区块 ID 的偏移量。
        /// </summary>
        internal const int FmtChunkID = 12;
        /// <summary>
        /// 表示格式区块大小的偏移量。
        /// </summary>
        internal const int FmtChunkSize = 16;
        /// <summary>
        /// 表示音频格式的偏移量。
        /// </summary>
        internal const int AudioFormat = 20;
        /// <summary>
        /// 表示声道数量偏移量。
        /// </summary>
        internal const int Channels = 22;
        /// <summary>
        /// 表示采样率的偏移量。
        /// </summary>
        internal const int SampleRate = 24;
        /// <summary>
        /// 表示字节率的偏移量。
        /// </summary>
        internal const int ByteRate = 28;
        /// <summary>
        /// 表示区块对齐的偏移量。
        /// </summary>
        internal const int BlockAlign = 32;
        /// <summary>
        /// 表示采样位深度的偏移量。
        /// </summary>
        internal const int BitDepth = 34;
        /// <summary>
        /// 表示数据区块 ID 的偏移量。
        /// </summary>
        internal const int DataChunkID = 36;
        /// <summary>
        /// 表示数据区块大小的偏移量。
        /// </summary>
        internal const int DataChunkSize = 40;
        /// <summary>
        /// 表示数据区块的偏移量。
        /// </summary>
        internal const int DataBlocks = 44;
    }
}
