using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using EADS.Model;
using System.Text;
using System.Data;

namespace EADS.DAL
{
    public class DAL_BaseAddress : DAL_Bases
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        public DAL_BaseAddress()
        {
        }
         
        public DataSet GetMatchAddress(string address)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select Address from tbaseaddress where  Address like '%");
            sql.Append(address);
            sql.Append("%';"); 
            return dbHelper.GetDataBySql(sql.ToString()); 
        }
         
    }
}
