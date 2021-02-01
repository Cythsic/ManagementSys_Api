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
    [Route("api/CheckIn")]
    public class CheckInController : Controller
    {
        private CheckInBLL bll = new CheckInBLL();


        [Route("GetPage")]
        [HttpPost]
        public PageDateRes<CheckIn> GetPage([FromBody] PageDataReq pageReq)
        {
            var whereStr = GetWhereStr();
            if (whereStr == "-1")
            {
                return new PageDateRes<CheckIn>() { code = ResCode.Error, msg = "查询参数有误！", data = null };
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
        public PageDateRes<CheckIn> GetDataList([FromBody] SearchDataReq pageReq)
        {
            var whereStr = GetWhereStr();
            if (whereStr == "-1")
            {
                return new PageDateRes<CheckIn>() { code = ResCode.Error, msg = "查询参数有误！", data = null };
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
        public DataRes<bool> Add([FromBody] CheckIn model)
        {
            DataRes<bool> res = new DataRes<bool>() { code = ResCode.Success, data = true };

            var user = new GMSUser(User);
            model.CreateTime = DateTime.Now;
            model.CreateUser = user.UserName;

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
        public DataRes<bool> Import([FromBody] List<CheckIn> modelList)
        {
            DataRes<bool> res = new DataRes<bool>() { code = ResCode.Success, data = true };

            var checkInsEdit = new List<CheckIn>();
            var checkInsAdd = new List<CheckIn>();

            var user = new GMSUser(User);
            modelList.ForEach(p =>
            {
                p.CreateTime = DateTime.Now;
                p.CreateUser = user.UserName;

                var v = bll.Get(p.RoomId);
                if (v != null)
                {
                    checkInsEdit.Add(v);
                }
                else
                {
                    checkInsAdd.Add(p);
                }
            });

            //如果已存在则更新
            bll.UpdateBatch(checkInsEdit);
            var r = bll.InsertBatch(checkInsAdd);

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
        public DataRes<bool> Edit([FromBody] CheckIn model)
        {
            DataRes<bool> res = new DataRes<bool>() { code = ResCode.Success, data = true };

            var user = new GMSUser(User);
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

            var r = bll.DeleteByWhere($"roomid='{id}'", null);
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