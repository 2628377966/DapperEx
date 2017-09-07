using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBHelperDapper
{
    public class ModelAttribute : Attribute
    {
        public ModelAttributeType Name { get; set; }
    }
    public enum ModelAttributeType : int
    {
        /// <summary>
        /// table列
        /// </summary>
        TableColumn = 1,
        /// <summary>
        /// 视图列
        /// </summary>
        ViewColumn = 2
    }
}
