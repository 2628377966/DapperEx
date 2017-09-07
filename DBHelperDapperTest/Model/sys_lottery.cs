using DBHelperDapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDapper.Model
{
    public class sys_lottery : PageBase
    {
        private int _id;
        private DateTime _lotteryData;
        private int _lotteryNo;
        private int _lotteryZi;
        private int _lotteryYang;
        private int _lotteryWa;
        private int _lotteryDie;


        [JsonProperty(PropertyName = "id")]
        [ModelAttribute(Name = ModelAttributeType.TableColumn)]
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        [JsonProperty(PropertyName = "lotteryData")]
        [ModelAttribute(Name = ModelAttributeType.TableColumn)]
        public DateTime LotteryData
        {
            get { return _lotteryData; }
            set { _lotteryData = value; }
        }

        [JsonProperty(PropertyName = "lotteryNo")]
        [ModelAttribute(Name = ModelAttributeType.TableColumn)]
        public int LotteryNo
        {
            get { return _lotteryNo; }
            set { _lotteryNo = value; }
        }

        [JsonProperty(PropertyName = "lotteryZi")]
        [ModelAttribute(Name = ModelAttributeType.TableColumn)]
        public int LotteryZi
        {
            get { return _lotteryZi; }
            set { _lotteryZi = value; }
        }

        [JsonProperty(PropertyName = "lotteryYang")]
        [ModelAttribute(Name = ModelAttributeType.TableColumn)]
        public int LotteryYang
        {
            get { return _lotteryYang; }
            set { _lotteryYang = value; }
        }

        [JsonProperty(PropertyName = "lotteryWa")]
        [ModelAttribute(Name = ModelAttributeType.TableColumn)]
        public int LotteryWa
        {
            get { return _lotteryWa; }
            set { _lotteryWa = value; }
        }

        [JsonProperty(PropertyName = "lotteryDie")]
        [ModelAttribute(Name = ModelAttributeType.TableColumn)]
        public int LotteryDie
        {
            get { return _lotteryDie; }
            set { _lotteryDie = value; }
        }

    }
}
