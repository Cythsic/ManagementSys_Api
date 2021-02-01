using GMS.Api.Model.Request;
using GMS.BLL;
using GMS.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace GMS.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/MaintainRecord")]
    public class MaintainRecordController : Controller
    {
        private MaintainRecordBLL bll = new MaintainRecordBLL();

        [Route("GetPage")]
        [HttpPost]
        public PageDateRes<MaintainRecord> GetPage([FromBody] PageDataReq pageReq)
        {
            var whereStr = GetWhereStr();
            if (whereStr == "-1")
            {
                return new PageDateRes<MaintainRecord>() { code = ResCode.Error, msg = "查询参数有误！", data = null };
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
        public PageDateRes<MaintainRecord> GetDataList([FromBody] SearchDataReq pageReq)
        {
            var whereStr = GetWhereStr();
            if (whereStr == "-1")
            {
                return new PageDateRes<MaintainRecord>() { code = ResCode.Error, msg = "查询参数有误！", data = null };
            }

            var list = bll.GetPage(whereStr, (pageReq.query["sort"] + " " + pageReq.query["order"]), Convert.ToInt32(pageReq.query["pageNumber"]), Convert.ToInt32(pageReq.query["pageSize"]));

            return list;
        }



        /// <summary>
        /// 获取维修单
        /// </summary>
        /// <returns></returns>
        [Route("GetMaintainData/{id}")]
        [HttpPost]
        public DataRes<MaintainRecord> GetMaintainData(string id)
        {
            DataRes<MaintainRecord> res = new DataRes<MaintainRecord>() { code = ResCode.Success, success = true };

            var r = bll.Get(id);

            if (r != null)
            {
                res.code = ResCode.Success;
                res.data = r;
            }

            return res;
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <returns></returns>
        [Route("Add")]
        [HttpPost]
        public DataRes<bool> Add([FromBody] MaintainRecord model)
        {
            var user = new GMSUser(User);
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
        /// 编辑用户
        /// </summary>
        /// <returns></returns>
        [Route("Edit")]
        [HttpPost]
        public DataRes<bool> Edit([FromBody] MaintainRecord model)
        {
            DataRes<bool> res = new DataRes<bool>() { code = ResCode.Success, data = true };

            var user = new GMSUser(User);
            model.EndTime = DateTime.Now;
            model.IsStop = 1;
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
        public DataRes<bool> UpdateBatch([FromBody] List<MaintainRecord> modelList)
        {
            DataRes<bool> res = new DataRes<bool>() { code = ResCode.Success, data = true };

            var user = new GMSUser(User);
            modelList.ForEach(p =>
            {


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
        /// 接单
        /// </summary>
        /// <returns></returns>
        [Route("Apply/{id}")]
        [HttpPost]
        public DataRes<bool> Apply(string id)
        {
            DataRes<bool> res = new DataRes<bool>() { code = ResCode.Success, data = true };

            var r = bll.UpdateByFlag($"Id={id}", "eventStatus", 1);
            if (!r)
            {
                res.code = ResCode.Error;
                res.data = false;
                res.msg = "接单失败";
            }

            return res;
        }

        /// <summary>
        /// 撤单
        /// </summary>
        /// <returns></returns>
        [Route("Reject/{id}")]
        [HttpPost]
        public DataRes<bool> Reject(string id)
        {
            DataRes<bool> res = new DataRes<bool>() { code = ResCode.Success, data = true };

            var r = bll.UpdateByFlag($"Id={id}", "eventStatus", 2);
            if (!r)
            {
                res.code = ResCode.Error;
                res.data = false;
                res.msg = "撤单失败";
            }

            return res;
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <returns></returns>
        [Route("Delete/{id}")]
        [HttpPost]
        public DataRes<bool> Delete(string id)
        {
            DataRes<bool> res = new DataRes<bool>() { code = ResCode.Success, data = true };

            var r = bll.DeleteByFlag($"Id='{id}'", null);
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
        public DataRes<bool> DeleteBatch([FromBody] List<MaintainRecord> modelList)
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
    }
}