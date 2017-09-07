using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBHelperDapper
{
    public static class SqlMapperEx
    {

        /// <summary>
        /// Obtains the data as a list; if it is *already* a list, the original object is returned without
        /// any duplication; otherwise, ToList() is invoked.
        /// </summary>
        public static List<T> AsList<T>(this IEnumerable<T> source)
        {
            return (source == null || source is List<T>) ? (List<T>)source : source.ToList();
        }

    }
}
