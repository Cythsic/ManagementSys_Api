using GMS.Api.Model.Request;
using GMS.BLL;
using GMS.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace GMS.Api.Controllers
{
    [Produces("application/json")]
    [Authorize]
    [Route("api/DictManage")]
    public class DictManageController : Controller
    {
        private DictManageBLL bll = new DictManageBLL();

        [Route("GetPage")]
        [HttpPost]
        public PageDateRes<DictManage> GetPage([FromBody] PageDataReq pageReq)
        {
            var whereStr = GetWhereStr();
            if (whereStr == "-1")
            {
                return new PageDateRes<DictManage>() { code = ResCode.Error, msg = "查询参数有误！", data = null };
            }
            var list = bll.GetPage(whereStr, (pageReq.field + " " + pageReq.order), pageReq.pageNum, pageReq.pageSize);

            return list;
        }

        private string GetWhereStr()
        {
            StringBuilder sb = new StringBuilder(" 1=1 ");
            var query = this.HttpContext.GetWhereStr();
            if (query == "-1")
            {
                return query;
            }
            sb.AppendFormat(" and {0} ", query);

            return sb.ToString();
        }

        [Route("GetDataList")]
        [HttpPost]
        public PageDateRes<DictManage> GetDataList([FromBody] SearchDataReq pageReq)
        {
            var whereStr = GetWhereStr();
            if (whereStr == "-1")
            {
                return new PageDateRes<DictManage>() { code = ResCode.Error, msg = "查询参数有误！", data = null };
            }

            var list = bll.GetPage(whereStr, (pageReq.query["sort"] + " " + pageReq.query["order"]), Convert.ToInt32(pageReq.query["pageNumber"]), Convert.ToInt32(pageReq.query["pageSize"]));

            return list;
        }

        /// <summary>
        /// 查询字典数据
        /// </summary>
        /// <returns></returns>
        [Route("SearchDictData/{key}")]
        [HttpPost]
        public DataRes<List<DictManage>> SearchDictData(string key)
        {
            DataRes<List<DictManage>> res = new DataRes<List<DictManage>>() { code = ResCode.Success, msg = "success" };

            try
            {
                var r = bll.GetList("dictId ='" + key + "'");
                res.data = r;
            }
            catch
            {
                res.code = ResCode.Error;
                res.msg = "获取失败！";
            }

            return res;
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <returns></returns>
        [Route("Add")]
        [HttpPost]
        public DataRes<bool> Add([FromBody] DictManage model)
        {
            var user = new GMSUser(User);
            model.CreateTime = DateTime.Now;
            model.CreateUser = user.UserName;
            model.ModifyDate = DateTime.Now;
            model.ModifyUser = user.UserName;
            model.DelFlag = false;
            DataRes<bool> res = new DataRes<bool>() { code = ResCode.Success, data = true };

            var r = bll.Insert(model) > 0;

            if (!r)
            {
                res.code = ResCode.Error;
                res.msg = "保存失败！";
            }

            return res;
        }

        /// <summary>
        /// 导入用户
        /// </summary>
        /// <returns></returns>
        [Route("Import")]
        [HttpPost]
        public DataRes<bool> Import([FromBody] List<DictManage> modelList)
        {
            DataRes<bool> res = new DataRes<bool>() { code = ResCode.Success, data = true };

            var user = new GMSUser(User);
            modelList.ForEach(p =>
            {
                p.CreateTime = DateTime.Now;
                p.CreateUser = user.UserName;
                p.ModifyDate = DateTime.Now;
                p.ModifyUser = user.UserName;
            });
            var r = bll.InsertBatch(modelList);

            if (!r)
            {
                res.code = ResCode.Error;
                res.msg = "保存失败！";
            }

            return res;
        }

        /// <summary>
        /// 编辑用户
        /// </summary>
        /// <returns></returns>
        [Route("Edit")]
        [HttpPost]
        public DataRes<bool> Edit([FromBody] DictManage model)
        {
            DataRes<bool> res = new DataRes<bool>() { code = ResCode.Success, data = true };

            var user = new GMSUser(User);
            model.ModifyDate = DateTime.Now;
            model.ModifyUser = user.UserName;
            var r = bll.Update(model);
            if (!r)
            {
                res.code = ResCode.Error;
                res.data = false;
                res.msg = "保存失败";
            }

            return res;
        }


        /// <summary>
        /// 批量更新员工信息
        /// </summary>
        /// <returns></returns>
        [Route("UpdateBatch")]
        [HttpPost]
        public DataRes<bool> UpdateBatch([FromBody] List<DictManage> modelList)
        {
            DataRes<bool> res = new DataRes<bool>() { code = ResCode.Success, data = true };

            var user = new GMSUser(User);
            modelList.ForEach(p =>
            {
                p.ModifyDate = DateTime.Now;
                p.ModifyUser = user.UserName;

            });
            var r = bll.UpdateBatch(modelList);

            if (!r)
            {
                res.code = ResCode.Error;
                res.msg = "批量更新失败！";
            }

            return res;
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <returns></returns>
        [Route("Delete/{ids}")]
        [HttpPost]
        public DataRes<bool> Delete(string ids)
        {
            DataRes<bool> res = new DataRes<bool>() { code = ResCode.Success, data = true };

            var r = bll.DeleteByWhere($"Id in({ids})", null);
            if (!r)
            {
                res.code = ResCode.Error;
                res.data = false;
                res.msg = "删除失败";
            }

            return res;
        }

        /// <summary>
        /// 导入用户
        /// </summary>
        /// <returns></returns>
        [Route("DeleteBatch")]
        [HttpPost]
        public DataRes<bool> DeleteBatch([FromBody] List<DictManage> modelList)
        {
            DataRes<bool> res = new DataRes<bool>() { code = ResCode.Success, data = true };
            var r = bll.DeleteBatch(modelList);

            if (!r)
            {
                res.code = ResCode.Error;
                res.msg = "批量删除失败！";
            }

            return res;
        }

        /// <summary>
        /// 根据Type查询字典数据
        /// </summary>
        /// <returns></returns>
        [Route("GetDictDataByType/{type}")]
        [HttpPost]
        public DataRes<List<DictManage>> GetDictDataByType(string type)
        {
            DataRes<List<DictManage>> res = new DataRes<List<DictManage>>() { code = ResCode.Success, success = true };

            try
            {
                var r = bll.GetList("dictid in(select id from DictType(nolock) where type='" + type + "')");
                res.data = r;
            }
            catch
            {
                res.code = ResCode.Error;
                res.msg = "批量删除失败！";
            }

            return res;
        }
    }
}

