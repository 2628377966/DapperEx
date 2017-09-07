using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DapperEx
{
    /// <summary>
    /// 主键 默认不是自动主键
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class KeyAttribute : BaseAttribute
    {
        /// <summary>
        /// 是否为自动主键
        /// </summary>
        public bool CheckAutoKey { get; set; }

        /// <summary>
        /// 是否为自动主键
        /// </summary>
        public KeyAttribute()
        {
            this.CheckAutoKey = false;
        }
        /// <summary>
        /// 是否为自动主键
        /// </summary>
        /// <param name="checkAutoKey">是否为自动主键</param>
        public KeyAttribute(bool checkAutoKey)
        {
            this.CheckAutoKey = checkAutoKey;
        }
    }
}
