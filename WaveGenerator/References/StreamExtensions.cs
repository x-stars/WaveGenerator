using System;
using System.IO;

namespace XstarS.IO
{
    /// <summary>
    /// 提供字节流 <see cref="Stream"/> 的扩展方法。
    /// </summary>
    internal static class StreamExtensions
    {
        /// <summary>
        /// 从当前流读取指定长度的字节序列，并提升流的位置。
        /// </summary>
        /// <param name="stream">要读取的流。</param>
        /// <param name="length">要读取的字节序列的长度。</param>
        /// <returns>读取当前流得到的字节序列。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="stream"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="length"/> 为负数。</exception>
        public static byte[] Read(this Stream stream, int length)
        {
            if (stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            var bytes = new byte[length];
            stream.Read(bytes, 0, length);
            return bytes;
        }

        /// <summary>
        /// 从当前流读取指定类型的非托管数据，并提升流的位置。
        /// </summary>
        /// <typeparam name="T">非托管数据的类型。</typeparam>
        /// <param name="stream">要读取的流。</param>
        /// <returns>读取当前流得到的非托管数据。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="stream"/> 为 <see langword="null"/>。</exception>
        public static unsafe T Read<T>(this Stream stream)
            where T : unmanaged
        {
            if (stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            var bytes = stream.Read(sizeof(T));
            fixed (byte* pBytes = bytes)
            {
                return *(T*)pBytes;
            }
        }

        /// <summary>
        /// 向当前流写入指定的字节序列，并提升流的位置。
        /// </summary>
        /// <param name="stream">要写入的流。</param>
        /// <param name="bytes">要写入的字节序列。</param>
        /// <exception cref="ArgumentNullException"><paramref name="stream"/>
        /// 或 <paramref name="bytes"/> 为 <see langword="null"/>。</exception>
        public static void Write(this Stream stream, params byte[] bytes)
        {
            if (stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }
            if (bytes is null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            stream.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// 向当前流写入指定类型的非托管数据，并提升流的位置。
        /// </summary>
        /// <typeparam name="T">非托管数据的类型。</typeparam>
        /// <param name="stream">要写入的流。</param>
        /// <param name="value">要写入的非托管数据。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="stream"/> 为 <see langword="null"/>。</exception>
        public static unsafe void Write<T>(this Stream stream, T value)
            where T : unmanaged
        {
            if (stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            var bytes = new byte[sizeof(T)];
            fixed (byte* pBytes = bytes)
            {
                *(T*)pBytes = value;
            }
            stream.Write(bytes);
        }
    }
}
