using ConsoleDapper.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDapper.Service
{
    public class sys_lottery
    {

        public Model.sys_lottery GetModel(Model.sys_lottery model)
        {
            return BMAData.Instance.Sys_lottery.GetModel(model);
        }

        public int AddModel(Model.sys_lottery model)
        {
            int no = BMAData.Instance.Sys_lottery.AddModel(model);
            return no;
        }

        public int DeleteModel(Model.sys_lottery model)
        {
            return BMAData.Instance.Sys_lottery.DeleteModel(model);
        }

        public IList<Model.sys_lottery> GetModelList(Model.sys_lottery model)
        {
            return BMAData.Instance.Sys_lottery.GetModelList(model);
        }

        public IList<Model.sys_lottery> GetModelListByPage(Model.sys_lottery model)
        {
            return BMAData.Instance.Sys_lottery.GetModelListByPage(model);
        }
    }
}
