using GMS.Api.Model.Request;
using GMS.BLL;
using GMS.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text;

namespace GMS.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Batches")]
    public class BatchesController : Controller
    {
        private BatchesBLL bll = new BatchesBLL();

        [Route("GetPage")]
        [HttpPost]

        public PageDateRes<Batches> GetPage([FromBody] PageDataReq pageReq)
        {
            var whereStr = GetWhereStr();
            if (whereStr == "-1")
            {
                return new PageDateRes<Batches>() { code = ResCode.Error, msg = "查询参数有误！", data = null };
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
        /// 添加用户
        /// </summary>
        /// <returns></returns>
        [Route("Add")]
        [HttpPost]
        public DataRes<bool> Add([FromBody] AgentInfos agent)
        {
            Batches model = new Batches();
            DataRes<bool> res = new DataRes<bool>() { code = ResCode.Success, data = true };
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
        public DataRes<bool> Import([FromBody] List<Batches> modelList)
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
        public DataRes<bool> Edit([FromBody] Batches model)
        {
            DataRes<bool> res = new DataRes<bool>() { code = ResCode.Success, data = true };
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
        /// 删除用户
        /// </summary>
        /// <returns></returns>
        [Route("Delete/{id}")]
        [HttpPost]
        public DataRes<bool> Delete(string id)
        {
            DataRes<bool> res = new DataRes<bool>() { code = ResCode.Success, data = true };

            var r = bll.DeleteByWhere($"BatchCode={id}", null);
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