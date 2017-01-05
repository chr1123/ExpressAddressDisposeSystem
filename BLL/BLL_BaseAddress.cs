using EADS.Model;
using EADS.DAL;
using System.Collections.Generic;
using System.Data;

namespace EADS.BLL
{ 
	public class BLL_BaseAddress
    {
        private DAL_BaseAddress dal = new DAL_BaseAddress(); 
        /// <summary>
        /// 构造函数
        /// </summary>
        public BLL_BaseAddress()
        {
           
        }

        public int Insert(string sql)
        {
            dal.OpenConnect();
            int result =  dal.Insert(sql);
            dal.CloseConnect();
            return result;
        }

        //public DataSet GetListOfLv1(int page, int pageSize)
        //{
        //    dal.OpenConnect();
        //    DataSet result = dal.GetListOfLv1(page,pageSize);
        //    dal.CloseConnect();
        //    return result;
        //}
        //public DataSet GetListOfLv2(int page, int pageSize)
        //{
        //    dal.OpenConnect();
        //    DataSet result = dal.GetListOfLv2(page, pageSize);
        //    dal.CloseConnect();
        //    return result;
        //}
        //public DataSet GetListOfLv3(int page, int pageSize)
        //{
        //    dal.OpenConnect();
        //    DataSet result = dal.GetListOfLv3(page, pageSize);
        //    dal.CloseConnect();
        //    return result;
        //}
        //public DataSet GetListOfLv4(int page, int pageSize)
        //{
        //    dal.OpenConnect();
        //    DataSet result = dal.GetListOfLv4(page, pageSize);
        //    dal.CloseConnect();
        //    return result;
        //}


        public DataSet GetMatchAddress(string address,int rows)
        {  
            dal.OpenConnect();
            DataSet result = dal.GetMatchAddress(address,rows);
            dal.CloseConnect();
            return result; 
        }

        public DataSet GetList(int page, int rows, string address, out int total)
        {
            dal.OpenConnect();
            DataSet result = dal.GetList(page, rows, address,out total);
            dal.CloseConnect();
            return result;
        }


        public bool Add(string address)
        {
            dal.OpenConnect();
            bool result = dal.Add(address);
            dal.CloseConnect();
            return result;
        }
    }
}
