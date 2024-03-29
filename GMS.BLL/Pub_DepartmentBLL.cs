﻿using GMS.DAL;
using GMS.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GMS.BLL
{

    public partial class Pub_DepartmentBLL
    {

        Pub_DepartmentDAL dal = new Pub_DepartmentDAL();
        /// <summary>
        /// 获取部门编号
        /// </summary>
        /// <param name="pCode"></param>
        /// <returns></returns>
        public string GetCode()
        {
            var code = "D000001";
            List<Pub_Department> depts = GetList("", " DeptCode Desc ", 1);
            if (depts.Count > 0)
            {
                var dept = depts.First();
                code = "D" + (Convert.ToInt32(dept.DeptCode.Remove(0, 1)) + 1).ToString().PadLeft(6, '0');
            }

            return code;
        }
    }
}
