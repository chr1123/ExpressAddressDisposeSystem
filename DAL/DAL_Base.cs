using System; 
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using EADS.Model;

namespace EADS.DAL
{ 
	public class DAL_Bases
    {
        public  DBUtility.MySqlHelper dbHelper = new DBUtility.MySqlHelper();

        /// <summary>
        /// 构造函数
        /// </summary>
        public DAL_Bases()
        {
        }
        public bool OpenConnect() {
           return dbHelper.OpenConnect()>=0;
        }
        public void CloseConnect()
        {
            dbHelper.CloseConnect();
        }
    }
}
