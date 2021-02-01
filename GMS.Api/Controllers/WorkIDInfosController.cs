using GMS.Api.Model.Request;
using GMS.BLL;
using GMS.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GMS.Api.Controllers
{
    [Produces("application/json")]
    [Authorize]
    [Route("api/WorkIDInfos")]
    public class WorkIDInfosController : Controller
    {
        private WorkIDInfosBLL bll = new WorkIDInfosBLL();
        private HRInputsBLL hRInputsBLL = new HRInputsBLL();
        private HR_AgentInputsBLL agentInputsBLL = new HR_AgentInputsBLL();

        [Route("GetPage")]
        [HttpPost]
        public PageDateRes<WorkIDInfos> GetPage([FromBody] PageDataReq pageReq)
        {
            var whereStr = GetWhereStr();
            if (whereStr == "-1")
            {
                return new PageDateRes<WorkIDInfos>() { code = ResCode.Error, msg = "查询参数有误！", data = null };
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
        public PageDateRes<WorkIDInfos> GetDataList([FromBody] SearchDataReq pageReq)
        {
            var whereStr = GetWhereStr();
            if (whereStr == "-1")
            {
                return new PageDateRes<WorkIDInfos>() { code = ResCode.Error, msg = "查询参数有误！", data = null };
            }

            var list = bll.GetPage(whereStr, (pageReq.query["sort"] + " " + pageReq.query["order"]), Convert.ToInt32(pageReq.query["pageNumber"]), Convert.ToInt32(pageReq.query["pageSize"]));

            return list;
        }


        /// <summary>
        /// 查询当前最大工号
        /// </summary>
        /// <returns></returns>
        [Route("SearchWorkId")]
        [HttpGet]
        public DataRes<WorkIDInfos> SearchWorkId()
        {
            DataRes<WorkIDInfos> res = new DataRes<WorkIDInfos>() { code = ResCode.Success, msg = "success" };

            try
            {
                var r = bll.GetList("isCard = 1").OrderByDescending(p => p.ModifyDate);

                if (r.Count() == 0)
                {
                    res.code = ResCode.Error;
                    return res;
                }
                res.data = r.FirstOrDefault();
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
        public DataRes<bool> Add([FromBody] WorkIDInfos model)
        {

            DataRes<bool> res = new DataRes<bool>() { code = ResCode.Success, data = true };
            //
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
        public DataRes<bool> Import([FromBody] List<WorkIDInfos> modelList)
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
        public DataRes<bool> Edit([FromBody] WorkIDInfos model)
        {
            DataRes<bool> res = new DataRes<bool>() { code = ResCode.Success, data = true };
            List<AgentInputs> agentInputs = new List<AgentInputs>();

            AgentInputs agentInput = new AgentInputs
            {
                CardID = model.CardID,
                Name = model.Name
            };

            model.IsCard = true;
            var r = bll.Update(model);

            if (r)
            {
                //更新信息录入体检状态 ，0--未制卡 1--已制卡
                agentInputsBLL.ChangeStatus(agentInputs, "IsCard");
            }
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
        public DataRes<bool> UpdateBatch([FromBody] List<WorkIDInfos> modelList)
        {
            DataRes<bool> res = new DataRes<bool>() { code = ResCode.Success, data = true };
            List<AgentInputs> agentInputs = new List<AgentInputs>();
            List<HRInputs> hRInputs = new List<HRInputs>();

            modelList.ForEach(p =>
            {
                p.IsStop = true;
                p.IsCard = true;

                AgentInputs agentInput = new AgentInputs
                {
                    CardID = p.CardID,
                    Name = p.Name
                };
                agentInputs.Add(agentInput);

                HRInputs hRInput = new HRInputs
                {
                    CardID = p.CardID,
                    Name = p.Name,
                    ModifyDate = p.ModifyDate,
                    ModifyUser = p.ModifyUser,
                };
                hRInputs.Add(hRInput);
            });
            var r = bll.UpdateBatch(modelList);

            if (r)
            {
                //更新信息录入体检状态 ，0--未制卡 1--已制卡
                agentInputsBLL.ChangeStatus(agentInputs, "IsCard");

                //已制卡人员插入到信息管理表
                hRInputsBLL.InsertBatch(hRInputs);
            }

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
        [Route("Delete/{id}")]
        [HttpPost]
        public DataRes<bool> Delete(string id)
        {
            DataRes<bool> res = new DataRes<bool>() { code = ResCode.Success, data = true };

            var r = bll.DeleteByWhere($"CardID='{id}'", null);
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
        public DataRes<bool> DeleteBatch([FromBody] List<WorkIDInfos> modelList)
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

