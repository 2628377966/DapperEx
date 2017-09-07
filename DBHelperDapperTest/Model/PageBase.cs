using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDapper.Model
{
    public class PageBase
    {
        /// <summary>
        /// page的编号从1开始
        /// </summary>
        [JsonProperty(PropertyName = "page")]
        public int Page { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "pageSize")]
        public int PageSize { get; set; }

        [JsonProperty(PropertyName = "total")]
        public int Total { get; set; }

        [JsonProperty(PropertyName = "order")]
        public string Order { get; set; }

        [JsonProperty(PropertyName = "isAsc")]
        public bool IsAsc { get; set; }
    }
}
