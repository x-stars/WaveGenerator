using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 表示枚举的列表视图。
    /// </summary>
    /// <typeparam name="TEnum">枚举的类型。</typeparam>
    [Serializable]
    [DebuggerDisplay("Selected = {" + nameof(SelectedItem) + "}")]
    public class EnumListView<TEnum> : ReadOnlyObservableCollection<TEnum>
        where TEnum : struct, Enum
    {
        /// <summary>
        /// 表示当前视图选中的枚举值的索引。
        /// </summary>
        private int ItemIndex;

        /// <summary>
        /// 初始化 <see cref="EnumListView{TEnum}"/> 类的新实例。
        /// </summary>
        public EnumListView() : base(EnumListView<TEnum>.CreateItems()) { }

        /// <summary>
        /// 创建当前枚举类型中所有枚举值的集合。
        /// </summary>
        /// <returns>当前枚举类型中所有枚举值的集合。</returns>
        private static ObservableCollection<TEnum> CreateItems()
        {
            var values = (TEnum[])Enum.GetValues(typeof(TEnum));
            return new ObservableCollection<TEnum>(values);
        }

        /// <summary>
        /// 获取或设置当前视图选中的枚举值的索引。
        /// </summary>
        /// <returns>当前视图选中的枚举值的索引。</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="value"/> 不为小于当前枚举值数量的非负整数。</exception>
        public int SelectedIndex
        {
            get => this.ItemIndex;
            set => this.SelectIndex(value);
        }

        /// <summary>
        /// 获取或设置当前视图选中的枚举值。
        /// </summary>
        /// <returns>当前视图选中的枚举值。</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="value"/> 不为有效的枚举值。</exception>
        public TEnum SelectedItem
        {
            get => this[this.SelectedIndex];
            set => this.SelectedIndex = this.IndexOf(value);
        }

        /// <summary>
        /// 设置当前视图选中的枚举值的索引。
        /// </summary>
        /// <param name="index">要设置的枚举值的索引。</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="index"/> 不为小于当前枚举值数量的非负整数。</exception>
        protected virtual void SelectIndex(int index)
        {
            if ((index < 0) || (index >= this.Count))
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
            if (this.ItemIndex != index)
            {
                this.ItemIndex = index;
                this.NotifyPropertyChanged(nameof(this.SelectedIndex));
                this.NotifyPropertyChanged(nameof(this.SelectedItem));
            }
        }

        /// <summary>
        /// 通知指定属性的值已更改。
        /// </summary>
        /// <param name="propertyName">已更改属性的名称。</param>
        protected void NotifyPropertyChanged(string propertyName)
        {
            this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }
    }
}
