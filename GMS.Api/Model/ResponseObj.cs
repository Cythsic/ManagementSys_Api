﻿namespace GMS.Api
{
    public class ResponseObj<T>
    {
        /// <summary>
        /// 0请求数据错误 1 成功 -1失败
        /// </summary>
        public int Code { get; set; }

        public string Message { get; set; }

        public T Data { get; set; }
    }
}
