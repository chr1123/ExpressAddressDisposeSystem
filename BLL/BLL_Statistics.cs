using EADS.Model;
using EADS.DAL;
using System.Collections.Generic;
using System.Data;

namespace EADS.BLL
{ 
	public class BLL_Statistics
    {
        private DAL_Statistics dal = new DAL_Statistics(); 
        /// <summary>
        /// 构造函数
        /// </summary>
        public BLL_Statistics()
        {
           
        }

       

        public DataSet GetOrderCountInfo(string where)
        {
            dal.OpenConnect();
            DataSet result = dal.GetOrderCountInfo(where);
            dal.CloseConnect();
            return result;
        }

        public DataSet GetUserOrderCountList(int page, int rows, string strWhere, out int total) { 
            dal.OpenConnect();
            DataSet result = dal.GetUserOrderCountList(page, rows, strWhere, out total);
            dal.CloseConnect();
            return result;
        }

        public DataSet GetOrderCountOfTodayEachHour()
        {
            dal.OpenConnect();
            DataSet result = dal.GetOrderCountOfTodayEachHour();
            dal.CloseConnect();
            return result;
        }

    }
}
