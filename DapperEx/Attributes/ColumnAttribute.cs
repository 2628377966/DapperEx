using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DapperEx
{
    /// <summary>
    /// 列属性 默认不是自增长
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ColumnAttribute : BaseAttribute
    {
        /// <summary>
        /// 自增长
        /// </summary>
        public bool AutoIncrement { get; set; }

        /// <summary>
        /// 默认不是自增长
        /// </summary>
        public ColumnAttribute()
        {
            AutoIncrement = false;
        }
        
        /// <summary>
        /// 是否是自增长，默认false
        /// </summary>
        /// <param name="autoIncrement"></param>
        public ColumnAttribute(bool autoIncrement)
        {
            AutoIncrement = autoIncrement;
        }

    }
}
