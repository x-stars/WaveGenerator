using System;
using System.Collections.Generic;

namespace XstarS.Collections.Generic
{
    /// <summary>
    /// 提供数组的元素序列的相等比较的方法。
    /// </summary>
    /// <typeparam name="T">数组中的元素的类型。</typeparam>
    [Serializable]
    internal class ArrayEqualityComparer<T> : EqualityComparer<T[]>
    {
        /// <summary>
        /// 初始化 <see cref="ArrayEqualityComparer{T}"/> 类的新实例。
        /// </summary>
        public ArrayEqualityComparer() : this(null) { }

        /// <summary>
        /// 以比较元素时要使用的比较器初始化 <see cref="ArrayEqualityComparer{T}"/> 类的新实例。
        /// </summary>
        /// <param name="comparer">比较数组中的元素时要使用的比较器。</param>
        public ArrayEqualityComparer(IEqualityComparer<T> comparer = null)
        {
            this.ItemComparer = comparer ?? EqualityComparer<T>.Default;
        }

        /// <summary>
        /// 获取默认的 <see cref="ArrayEqualityComparer{T}"/> 实例。
        /// </summary>
        public static new ArrayEqualityComparer<T> Default { get; } = new ArrayEqualityComparer<T>();

        /// <summary>
        /// 获取比较数组中的元素时使用的比较器。
        /// </summary>
        protected IEqualityComparer<T> ItemComparer { get; }

        /// <summary>
        /// 确定两个指定的数组中所包含的元素是否对应相等。
        /// </summary>
        /// <param name="x">要比较的第一个数组。</param>
        /// <param name="y">要比较的第二个数组。</param>
        /// <returns>如果两个数组中的所有元素对应相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public override bool Equals(T[] x, T[] y)
        {
            if (object.ReferenceEquals(x, y)) { return true; }
            if ((x is null) ^ (y is null)) { return false; }

            var comparer = this.ItemComparer;

            if (x.GetType() != y.GetType()) { return false; }

            if (x.Length != y.Length) { return false; }

            var length = x.Length;
            for (int i = 0; i < length; i++)
            {
                if (!comparer.Equals(x[i], y[i]))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 获取指定的数组遍历元素得到的哈希代码。
        /// </summary>
        /// <param name="obj">要为其获取哈希代码的数组。</param>
        /// <returns>数组遍历元素得到的哈希代码。</returns>
        public override int GetHashCode(T[] obj)
        {
            if (obj is null) { return 0; }

            var comparer = this.ItemComparer;

            var length = obj.Length;
            var hashCode = obj.GetType().GetHashCode();
            for (int i = 0; i < length; i++)
            {
                var nextHashCode = comparer.GetHashCode(obj[i]);
                hashCode = hashCode * -1521134295 + nextHashCode;
            }
            return hashCode;
        }
    }
}
