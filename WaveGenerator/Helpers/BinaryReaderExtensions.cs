using System;
using System.IO;

namespace XstarS.IO
{
    /// <summary>
    /// 提供二进制读取器 <see cref="BinaryReader"/> 的扩展方法。
    /// </summary>
    public static class BinaryReaderExtensions
    {
        /// <summary>
        /// 从当前二进制读取器读取指定类型的非托管数据，并将当前位置前移对应的字节数。
        /// </summary>
        /// <typeparam name="T">非托管数据的类型。</typeparam>
        /// <param name="reader">要读取数据的二进制读取器。</param>
        /// <returns>从基础流中读取的非托管数据。如果到达了流的末尾，则该数据可能不完整。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="reader"/> 为 <see langword="null"/>。</exception>
        public static unsafe T Read<T>(this BinaryReader reader)
            where T : unmanaged
        {
            if (reader is null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            var size = sizeof(T);
            var bytes = reader.ReadBytes(size);
            fixed (byte* pBytes = bytes)
            {
                var value = *(T*)pBytes;
                return value;
            }
        }
    }
}
