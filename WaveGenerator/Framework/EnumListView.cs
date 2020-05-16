using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 表示枚举的列表视图。
    /// </summary>
    /// <typeparam name="TEnum">枚举的类型。</typeparam>
    [Serializable]
    public class EnumListView<TEnum> : ReadOnlyObservableCollection<TEnum>
        where TEnum : struct, Enum
    {
        /// <summary>
        /// 表示当前视图表示的枚举值的索引。
        /// </summary>
        private int IndexValue;

        /// <summary>
        /// 初始化 <see cref="EnumListView{TEnum}"/> 类的新实例。
        /// </summary>
        public EnumListView() : base(new ObservableCollection<TEnum>())
        {
            this.IndexValue = -1;
            var values = (TEnum[])Enum.GetValues(typeof(TEnum));
            foreach (var value in values) { this.Items.Add(value); }
        }

        /// <summary>
        /// 获取或设置当前视图表示的枚举值的索引。
        /// </summary>
        public int Index
        {
            get => this.IndexValue;
            set => this.SetIndex(value);
        }

        /// <summary>
        /// 获取或设置当前视图表示的枚举值。
        /// </summary>
        public TEnum Value
        {
            get => this.Index < 0 ? default(TEnum) : this[this.Index];
            set => this.Index = this.IndexOf(value);
        }

        /// <summary>
        /// 设置当前视图表示的枚举值的索引。
        /// </summary>
        /// <param name="index">要设置的枚举值的索引。</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/>
        /// 不在 -1 和 <see cref="Collection{T}.Count"/> - 1 之间。</exception>
        protected virtual void SetIndex(int index)
        {
            if ((index < -1) || (index >= this.Count))
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
            this.IndexValue = index;
            this.OnPropertyChanged(
                new PropertyChangedEventArgs(nameof(this.Index)));
            this.OnPropertyChanged(
                new PropertyChangedEventArgs(nameof(this.Value)));
        }
    }
}
