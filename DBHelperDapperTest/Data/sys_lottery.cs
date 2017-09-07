using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDapper.Data
{
    public class sys_lottery
    {
        public ConsoleDapper.Model.sys_lottery GetModel(ConsoleDapper.Model.sys_lottery model)
        {
            return DBHelperDapper.DBHelper.GetModel<ConsoleDapper.Model.sys_lottery>(new { id = model.Id });
        }

        public int AddModel(ConsoleDapper.Model.sys_lottery model)
        {
            return DBHelperDapper.DBHelper.InsertModelUnAutoUpdate<ConsoleDapper.Model.sys_lottery>(ref model);
        }

        public int DeleteModel(ConsoleDapper.Model.sys_lottery model)
        {
            return DBHelperDapper.DBHelper.DeleteModel(ref model);
        }

        public IList<ConsoleDapper.Model.sys_lottery> GetModelList(ConsoleDapper.Model.sys_lottery model)
        {
            return DBHelperDapper.DBHelper.GetModelListByModelTable<ConsoleDapper.Model.sys_lottery>(GetWhere(model), model);
        }

        public IList<Model.sys_lottery> GetModelListByPage(Model.sys_lottery model)
        {
            string where = GetWhere(model);
            model.Total = DBHelperDapper.DBHelper.GetRecordCountByTBName<Model.sys_lottery>(where, model);

            return DBHelperDapper.DBHelper.GetModelListPageByTBName<ConsoleDapper.Model.sys_lottery>
                (where, model.Order + (model.IsAsc ? " ASC" : " DESC"),
                (model.Page - 1) * model.PageSize, model.Page * model.PageSize, model);
        }

        public string GetWhere(ConsoleDapper.Model.sys_lottery model)
        {
            StringBuilder SB = new StringBuilder(" 1=1");
            if (model.Id > 0)
            {
                SB.Append(" and Id = ?Id");
            }
            return SB.ToString();
        
        }
    }
}
