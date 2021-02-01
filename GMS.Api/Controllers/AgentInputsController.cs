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
    [Route("api/AgentInputs")]
    public class AgentInputsController : Controller
    {
        private AgentInputsBLL bll = new AgentInputsBLL();
        private HRInputsBLL hRInputsBLL = new HRInputsBLL();
        private InterviewBLL interviewBLL = new InterviewBLL();
        private HealthExamBLL healthExamBLL = new HealthExamBLL();

        [Route("GetPage")]
        [HttpPost]
        public PageDateRes<AgentInputs> GetPage([FromBody] PageDataReq pageReq)
        {
            var whereStr = GetWhereStr();
            if (whereStr == "-1")
            {
                return new PageDateRes<AgentInputs>() { code = ResCode.Error, msg = "查询参数有误！", data = null };
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
        public PageDateRes<AgentInputs> GetDataList([FromBody] SearchDataReq pageReq)
        {
            var whereStr = GetWhereStr();
            if (whereStr == "-1")
            {
                return new PageDateRes<AgentInputs>() { code = ResCode.Error, msg = "查询参数有误！", data = null };
            }

            var list = bll.GetPage(whereStr, (pageReq.query["sort"] + " " + pageReq.query["order"]), Convert.ToInt32(pageReq.query["pageNumber"]), Convert.ToInt32(pageReq.query["pageSize"]));

            return list;
        }

        /// <summary>
        /// 查询用户是否存在
        /// </summary>
        /// <returns></returns>
        [Route("SearchCardId/{key}")]
        [HttpPost]
        public DataRes<bool> SearchDict(string key)
        {
            DataRes<bool> res = new DataRes<bool>() { code = ResCode.Success, msg = "success" };

            try
            {
                var r = bll.GetList("cardId ='" + key + "' and isCard = 0").Count > 0;

                if (r)
                {
                    res.code = ResCode.Error;
                    res.msg = "身份证号码已存在！";
                }

            }
            catch
            {
                res.code = ResCode.Error;
                res.msg = "获取失败！";
            }

            return res;
        }

        /// <summary>
        /// 添加用户,数据同步到面试表
        /// </summary>
        /// <returns></returns>
        [Route("Add")]
        [HttpPost]
        public DataRes<bool> Add([FromBody] AgentInputs model)
        {
            DataRes<bool> res = new DataRes<bool>() { code = ResCode.Success, data = true };

            var user = new GMSUser(User);
            model.CreateDate = DateTime.Now;
            model.CreateUser = user.UserName;
            model.ModifyDate = DateTime.Now;
            model.ModifyUser = user.UserName;
            model.IsStop = false;
            model.IsInterview = false;
            model.IsHealthExam = false;
            model.IsCard = false;
            model.EmergentContact = model.HomeContact;
            model.EmergentPhone = model.HomeTel;

            Interview interview = new Interview
            {
                CardID = model.CardID,
                Name = model.Name,
                InterviewResult = "待面试",
                CreateTime = model.CreateDate,
                CreateUser = model.CreateUser,
                ModifyDate = model.ModifyDate,
                ModifyUser = model.ModifyUser,
                IsStop = false
            };

            Dictionary<string, object> keys = new Dictionary<string, object>();
            keys.Add("AgentInputs", model);
            keys.Add("Interview", interview);

            var r = bll.Insert(keys) > 0;

            if (!r)
            {
                res.code = ResCode.Error;
                res.msg = "保存失败！";
            }

            return res;
        }

        /// <summary>
        /// 导入用户，数据同步到面试表
        /// </summary>
        /// <returns></returns>
        [Route("Import")]
        [HttpPost]
        public DataRes<bool> Import([FromBody] List<AgentInputs> modelList)
        {
            DataRes<bool> res = new DataRes<bool>() { code = ResCode.Success, data = true };
            List<Interview> interviewList = new List<Interview>();

            var user = new GMSUser(User);
            modelList.ForEach(p =>
            {
                p.ModifyDate = DateTime.Now;
                p.ModifyUser = user.UserName;
                p.CreateDate = DateTime.Now;
                p.CreateUser = user.UserName;
                p.IsStop = false;
                p.IsInterview = false;
                p.IsHealthExam = false;
                p.IsCard = false;
                p.EmergentContact = p.HomeContact;
                p.EmergentPhone = p.HomeTel;

                Interview interview = new Interview
                {
                    CardID = p.CardID,
                    Name = p.Name,
                    InterviewResult = "待面试",
                    Department = p.Department,
                    CreateTime = p.CreateDate,
                    CreateUser = p.CreateUser,
                    ModifyDate = p.ModifyDate,
                    ModifyUser = p.ModifyUser
                };
                interviewList.Add(interview);

            });

            Dictionary<string, object> keys = new Dictionary<string, object>();
            keys.Add("AgentInputs", modelList);
            keys.Add("Interview", interviewList);

            var r = bll.InsertBatch(keys);

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
        public DataRes<bool> Edit([FromBody] AgentInputs model)
        {
            DataRes<bool> res = new DataRes<bool>() { code = ResCode.Success, data = true };

            var user = new GMSUser(User);
            model.ModifyDate = DateTime.Now;
            model.ModifyUser = user.UserName;
            model.EmergentContact = model.HomeContact;
            model.EmergentPhone = model.HomeTel;
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
        public DataRes<bool> Delete(int id)
        {
            DataRes<bool> res = new DataRes<bool>() { code = ResCode.Success, data = true };

            var r = bll.DeleteByWhere($"id='{id}'", null);
            if (!r)
            {
                res.code = ResCode.Error;
                res.data = false;
                res.msg = "删除失败";
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
    }
}
