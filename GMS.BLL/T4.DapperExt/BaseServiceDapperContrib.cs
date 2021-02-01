using GMS.DAL;
using GMS.Model;
using System.Collections.Generic;

namespace GMS.BLL
{
    public partial class BaseServiceDapperContrib<T> where T : class, new()
    {
        BaseDataDapperContrib<T> dal = new BaseDataDapperContrib<T>();
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public long Insert(T model)
        {
            return dal.Insert(model);
        }

        public long Insert(Dictionary<string, object> keys)
        {
            return dal.Insert(keys);
        }

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public bool InsertBatch(List<T> models)
        {
            return dal.InsertBatch(models);
        }

        public bool InsertBatch(Dictionary<string, object> keys)
        {
            return dal.InsertBatch(keys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Update(T model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public bool UpdateBatch(List<T> models)
        {
            return dal.UpdateBatch(models);
        }

        /// <summary> 
        ///根据实体删除 id必须是int 或 guid
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public dynamic Delete(T model)
        {
            return dal.Delete(model);
        }


        /// <summary>
        /// 根据条件删除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public dynamic Delete(object predicate)
        {
            return dal.Delete(predicate);
        }

        /// <summary>
        /// 根据条件删除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool DeleteByWhere(string where, object param = null)
        {
            return dal.DeleteByWhere(where, param);
        }

        public bool DeleteByFlag(string where, object param = null)
        {
            return dal.DeleteByFlag(where, param);
        }

        public bool UpdateByFlag(string where, string keyFiled, int status, object param = null)
        {
            return dal.UpdateByFlag(where, keyFiled, status, param);
        }

        /// <summary>
        /// 根据实体删除 id必须是int 或 guid
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public bool DeleteBatch(List<T> models)
        {
            return dal.DeleteBatch(models);
        }

        /// <summary>
        /// 获取一个实体对象
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public T Get(object id)
        {
            return dal.Get(id);
        }

        /// <summary>
        /// 获取一个实体对象
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public T Get(object id, string keyName)
        {
            return dal.Get(id, keyName);
        }

        /// <summary>
        /// 根据条件查询实体列表
        /// </summary>
        /// <param name="where">条件</param>
        /// <param name="sort">排序</param>
        /// <param name="limits">前几条</param>
        /// <returns></returns>
        public List<T> GetList(string where, string sort = null, int limits = -1, string fields = " * ", string orderby = "")
        {
            return dal.GetList(where, sort, limits, fields, orderby);

        }

        /// <summary>
        /// 存储过程分页查询
        /// </summary>
        /// <param name="where"></param>
        /// <param name="sort"></param>
        /// <param name="page"></param>
        /// <param name="resultsPerPage"></param>
        /// <returns></returns>

        public PageDateRes<T> GetPage(string where, string sort, int page, int resultsPerPage, string fields = "*")
        {
            return dal.GetPage(where, sort, page, resultsPerPage, fields);
        }

        public bool ChangeSotpStatus(string where)
        {
            return dal.ChangeSotpStatus(where);
        }
    }
}