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
    [Route("api/HealthExam")]
    public class HealthExamController : Controller
    {
        private HealthExamBLL bll = new HealthExamBLL();
        private WorkIDInfosBLL workIDInfosBLL = new WorkIDInfosBLL();
        private HR_AgentInputsBLL agentInputsBLL = new HR_AgentInputsBLL();

        [Route("GetPage")]
        [HttpPost]
        public PageDateRes<HealthExam> GetPage([FromBody] PageDataReq pageReq)
        {
            var whereStr = GetWhereStr();
            if (whereStr == "-1")
            {
                return new PageDateRes<HealthExam>() { code = ResCode.Error, msg = "查询参数有误！", data = null };
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
        public PageDateRes<HealthExam> GetDataList([FromBody] SearchDataReq pageReq)
        {
            var whereStr = GetWhereStr();
            if (whereStr == "-1")
            {
                return new PageDateRes<HealthExam>() { code = ResCode.Error, msg = "查询参数有误！", data = null };
            }

            var list = bll.GetPage(whereStr, (pageReq.query["sort"] + " " + pageReq.query["order"]), Convert.ToInt32(pageReq.query["pageNumber"]), Convert.ToInt32(pageReq.query["pageSize"]));

            return list;
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <returns></returns>
        [Route("Add")]
        [HttpPost]
        public DataRes<bool> Add([FromBody] HealthExam model)
        {
            var user = new GMSUser(User);
            model.CreateTime = DateTime.Now;
            model.CreateUser = user.UserName;
            model.ModifyDate = DateTime.Now;
            model.ModifyUser = user.UserName;
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
        public DataRes<bool> Import([FromBody] List<HealthExam> modelList)
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
        public DataRes<bool> Edit([FromBody] HealthExam model)
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
        public DataRes<bool> UpdateBatch([FromBody] List<HealthExam> modelList)
        {
            DataRes<bool> res = new DataRes<bool>() { code = ResCode.Success, data = true };
            List<AgentInputs> agentInputs = new List<AgentInputs>();
            List<WorkIDInfos> workIDInfos = new List<WorkIDInfos>();

            var user = new GMSUser(User);
            modelList.ForEach(p =>
            {
                p.ModifyDate = DateTime.Now;
                p.ModifyUser = user.UserName;

                AgentInputs agentInput = new AgentInputs
                {
                    CardID = p.CardID,
                    Name = p.Name
                };
                agentInputs.Add(agentInput);

                WorkIDInfos workID = new WorkIDInfos
                {
                    Name = p.Name,
                    CardID = p.CardID,
                    Department = p.Department,
                    ModifyDate = p.ModifyDate,
                    ModifyUser = p.ModifyUser,
                    IsCard = false,
                    IsStop = false
                };
                if (p.ExamResult.Equals("合格"))
                {
                    workIDInfos.Add(workID);
                }

            });
            var r = bll.UpdateBatch(modelList);


            if (r)
            {
                //更新信息录入体检状态 ，0--未体检 1--已体检
                agentInputsBLL.ChangeStatus(agentInputs, "IsHealthExam");

                //面试合格的人员插入到体检记录表
                workIDInfosBLL.InsertBatch(workIDInfos);
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
        public DataRes<bool> DeleteBatch([FromBody] List<HealthExam> modelList)
        {
            DataRes<bool> res = new DataRes<bool>() { code = ResCode.Success, data = true };
            modelList.ForEach(p => p.ModifyDate = DateTime.Now);
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

