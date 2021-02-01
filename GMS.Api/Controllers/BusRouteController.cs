using GMS.Api.Model.Request;
using GMS.BLL;
using GMS.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GMS.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/BusRoute")]
    public class BusRouteController : Controller
    {
        private BusRouteBLL bll = new BusRouteBLL();
        private BusDivisionsBLL bll1 = new BusDivisionsBLL();
        [Route("GetPage")]
        [HttpPost]
        public PageDateRes<BusRoute> GetPage([FromBody] PageDataReq pageReq)
        {
            var whereStr = GetWhereStr();
            if (whereStr == "-1")
            {
                return new PageDateRes<BusRoute>() { code = ResCode.Error, msg = "查询参数有误！", data = null };
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
        public PageDateRes<BusRoute> GetDataList([FromBody] SearchDataReq pageReq)
        {
            var whereStr = GetWhereStr();
            if (whereStr == "-1")
            {
                return new PageDateRes<BusRoute>() { code = ResCode.Error, msg = "查询参数有误！", data = null };
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
        public DataRes<bool> Add([FromBody] BusRoute model)
        {
            var user = new GMSUser(User);
            model.CreateTime = DateTime.Now;
            model.CreateUser = user.UserName;
            model.ModifyDate = DateTime.Now;
            model.ModifyUser = user.UserName;
            model.IsStop = false;
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
        public DataRes<bool> Import([FromBody] List<BusRoute> modelList)
        {
            DataRes<bool> res = new DataRes<bool>() { code = ResCode.Success, data = true };

            var user = new GMSUser(User);
            modelList.ForEach(p =>
            {
                p.CreateTime = DateTime.Now;
                p.CreateUser = user.UserName;
                p.ModifyDate = DateTime.Now;
                p.ModifyUser = user.UserName;
                p.IsStop = false;
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
        public DataRes<bool> Edit([FromBody] BusRoute model)
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
        public DataRes<bool> UpdateBatch([FromBody] List<BusRoute> modelList)
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
        public DataRes<bool> DeleteBatch([FromBody] List<BusRoute> modelList)
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
        //上传图片
        [Route("AddImage")]
        [HttpPost]
        public DataRes<string> AddImage([FromServices] IHostingEnvironment environment)
        {
            DataRes<string> res = new DataRes<string>() { code = ResCode.Success, msg = "上传成功" };
            string path = string.Empty;

            try
            {
                var files = Request.Form.Files;
                string strPath = string.Empty;
                foreach (var file in files)
                {
                    string newFileName = DateTime.Now.ToString("MMddHHmmss_") + file.FileName;
                    strPath = Path.Combine("BusRoutePic", newFileName);
                    path = Path.Combine(environment.WebRootPath, strPath);
                    using (var stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                    {
                        file.CopyTo(stream);
                    }

                    res.success = true;              
                    res.data = "http://www.suitdo.cn/HR/BusRouteImg/" + newFileName;         
                }
            }
            catch
            {
                res.code = ResCode.Error;
                res.msg = "上传失败！";
            }

            return res;
        }


        [Route("AddImgInfo")]
        [HttpPost]
        public DataRes<bool> AddImgInfo([FromBody] BusDivisions model)
        {
            var user = new GMSUser(User);
            model.CreateTime = DateTime.Now;
            model.Statu = " 0";
            DataRes<bool> res = new DataRes<bool>() { code = ResCode.Success, data = true };

            var r = bll1.Insert(model) > 0;

            if (!r)
            {
                res.code = ResCode.Error;
                res.msg = "保存失败！";
            }

            return res;
        }

    }
}