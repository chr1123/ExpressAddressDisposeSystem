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



        public DataSet GetMatchAddress(string address)
        { 

            dal.OpenConnect();
            DataSet result = dal.GetMatchAddress(address);
            dal.CloseConnect();
            return result; 
        }

    }
}
