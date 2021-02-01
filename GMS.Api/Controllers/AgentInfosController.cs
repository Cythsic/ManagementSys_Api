using GMS.Api.Model.Request;
using GMS.BLL;
using GMS.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text;

namespace GMS.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/AgentInfos")]
    public class AgentInfosController : Controller
    {
        private AgentInfosBLL bll = new AgentInfosBLL();

        [Route("GetPage")]
        [HttpPost]

        public PageDateRes<AgentInfos> GetPage([FromBody] PageDataReq pageReq)
        {
            var whereStr = GetWhereStr();
            if (whereStr == "-1")
            {
                return new PageDateRes<AgentInfos>() { code = ResCode.Error, msg = "查询参数有误！", data = null };
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

        /// <summary>
        /// 查询所有字典类型
        /// </summary>
        /// <returns></returns>
        [Route("GetAgentInfosByDeptCode/{key}")]
        [HttpPost]
        public DataRes<AgentInfos> GetAgentInfosByDeptCode(string key)
        {
            DataRes<AgentInfos> res = new DataRes<AgentInfos>() { code = ResCode.Success, msg = "success" };

            try
            {
                var r = bll.Get(key, "DeptCode");
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
        public DataRes<bool> Add([FromBody] AgentInfos model)
        {
            DataRes<bool> res = new DataRes<bool>() { code = ResCode.Success, data = true };
            //var rr = bll.Insert(model);
            var r = bll.Insert(model) == 0;

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
        public DataRes<bool> Import([FromBody] List<AgentInfos> modelList)
        {
            DataRes<bool> res = new DataRes<bool>() { code = ResCode.Success, data = true };
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
        public DataRes<bool> Edit([FromBody] AgentInfos model)
        {
            DataRes<bool> res = new DataRes<bool>() { code = ResCode.Success, data = true };
            //
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
        /// 批量删除
        /// </summary>
        /// <returns></returns>
        [Route("DeleteBatch/{ids}")]
        [HttpPost]
        public DataRes<bool> DeleteBatch(string ids)
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
        /// 删除用户
        /// </summary>
        /// <returns></returns>
        [Route("Delete/{id}")]
        [HttpPost]
        public DataRes<bool> Delete(string id)
        {
            DataRes<bool> res = new DataRes<bool>() { code = ResCode.Success, data = true };

            var r = bll.DeleteByWhere($"DeptCode={id}", null);
            if (!r)
            {
                res.code = ResCode.Error;
                res.data = false;
                res.msg = "删除失败";
            }

            return res;
        }
    }
}