using System;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 提供位域枚举的向量视图的基类。
    /// </summary>
    /// <typeparam name="TEnum">位域枚举的类型。</typeparam>
    [Serializable]
    public class EnumFlagsVectorView<TEnum> : EnumVectorView<TEnum>
        where TEnum : struct, Enum
    {
        /// <summary>
        /// 初始化 <see cref="EnumFlagsVectorView{TEnum}"/> 类的新实例。
        /// </summary>
        public EnumFlagsVectorView() { }

        /// <summary>
        /// 获取当前视图是否选中了指定的枚举位域。
        /// </summary>
        /// <param name="enumValue">要确定是否选中的枚举位域。</param>
        /// <returns>若当前视图选中了 <paramref name="enumValue"/>，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        protected override bool IsSelected(TEnum enumValue)
        {
            return this.Value.HasFlag(enumValue);
        }

        /// <summary>
        /// 根据指示在当前视图选中或取消选中指定的枚举位域。
        /// </summary>
        /// <param name="enumValue">要选中或取消选中的枚举位域。</param>
        /// <param name="select">若要在当前视图选中指定的枚举位域，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</param>
        protected override void SelectEnum(TEnum enumValue, bool select)
        {
            var iValue = Convert.ToUInt64(this.Value);
            var iFlags = Convert.ToUInt64(enumValue);
            if (select) { iValue |= iFlags; } else { iValue &= ~iFlags; }
            this.Value = (TEnum)Enum.ToObject(typeof(TEnum), iValue);
        }
    }
}
