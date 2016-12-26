using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Interface.Common
{
    public class JsonResult<T> where T : class
    {
        private T _data = null; //数据
        private int _code = 0;//结果编码
        private string _msg = "ok";//结果
        private int _totalcount = 0;//总条数
        public T data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
            }
        }
        public int ret
        {
            get
            {
                return _code;
            }
            set
            {
                _code = value;
            }
        }
        public string msg
        {
            get
            {
                return _msg;
            }
            set
            {
                _msg = value;
            }
        }
        public int totalcount
        {
            get { return _totalcount; }
            set { _totalcount = value; }
        }
    }
}