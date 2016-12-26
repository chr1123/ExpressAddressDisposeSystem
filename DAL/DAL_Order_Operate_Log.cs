using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic; 

namespace EADS.DAL
{ 
	public class DAL_Order_Operate_Log:DAL_Bases
    {
       
        /// <summary>
        /// 构造函数
        /// </summary>
        public DAL_Order_Operate_Log()
        {
        }
        /// <summary>
        /// 添加记录
        /// </summary>
        /// <param name="model">EADS.Model.torder_operate_log实体类</param>
        /// <returns>新增记录的ID</returns>
        public int Add(Model.Model_Order_Operate_Log model)
        {
            string sql = @"INSERT INTO torder_operate_log
				(OrderID,UserID,OperateType,Description,OperateTime) 
				VALUES(@OrderID,@UserID,@OperateType,@Description,@OperateTime);
				SELECT LAST_INSERT_ID();";
            MySqlParameter[] parameters = new MySqlParameter[]{
                new MySqlParameter("@OrderID", MySqlDbType.Int32, 11){ Value = model.OrderID },
                new MySqlParameter("@UserID", MySqlDbType.Int32, 11){ Value = model.UserID },
                new MySqlParameter("@OperateType", MySqlDbType.Int16, 4){ Value = model.OperateType },
                model.Description == null ? new MySqlParameter("@Description", MySqlDbType.VarChar, 30) { Value = DBNull.Value } : new MySqlParameter("@Description", MySqlDbType.VarChar, 30) { Value = model.Description },
                new MySqlParameter("@OperateTime", MySqlDbType.DateTime, -1){ Value = model.OperateTime }
            };
            int maxID = -1;
            dbHelper.ExecuteScalar(sql, parameters, out maxID);
            return maxID;
        }
        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="model">EADS.Model.torder_operate_log实体类</param>
        public bool Update(EADS.Model.Model_Order_Operate_Log model)
        {
            string sql = @"UPDATE torder_operate_log SET 
				OrderID=@OrderID,UserID=@UserID,OperateType=@OperateType,Description=@Description,OperateTime=@OperateTime
				WHERE ID=@ID";
            MySqlParameter[] parameters = new MySqlParameter[]{
                new MySqlParameter("@OrderID", MySqlDbType.Int32, 11){ Value = model.OrderID },
                new MySqlParameter("@UserID", MySqlDbType.Int32, 11){ Value = model.UserID },
                new MySqlParameter("@OperateType", MySqlDbType.Int16, 4){ Value = model.OperateType },
                model.Description == null ? new MySqlParameter("@Description", MySqlDbType.VarChar, 30) { Value = DBNull.Value } : new MySqlParameter("@Description", MySqlDbType.VarChar, 30) { Value = model.Description },
                new MySqlParameter("@OperateTime", MySqlDbType.DateTime, -1){ Value = model.OperateTime },
                new MySqlParameter("@ID", MySqlDbType.Int32, 11){ Value = model.ID }
            };
            return dbHelper.ExecuteSql(sql, parameters) > 0;
        }
        /// <summary>
        /// 删除记录
        /// </summary>
        public int Delete(int id)
        {
            string sql = "DELETE FROM torder_operate_log WHERE ID=@ID";
            MySqlParameter[] parameters = new MySqlParameter[]{
                new MySqlParameter("@ID", MySqlDbType.Int32, 11){ Value = id }
            };
            return dbHelper.ExecuteSql(sql, parameters);
        }
        /// <summary>
        /// 将DataRedar转换为List
        /// </summary>
        private List<EADS.Model.Model_Order_Operate_Log> DataReaderToList(MySqlDataReader dataReader)
        {
            List<EADS.Model.Model_Order_Operate_Log> List = new List<EADS.Model.Model_Order_Operate_Log>();
            EADS.Model.Model_Order_Operate_Log model = null;
            while (dataReader.Read())
            {
                model = new EADS.Model.Model_Order_Operate_Log();
                model.ID = dataReader.GetInt32(0);
                model.OrderID = dataReader.GetInt32(1);
                model.UserID = dataReader.GetInt32(2);
                model.OperateType = (byte)dataReader.GetInt16(3);
                if (!dataReader.IsDBNull(4))
                    model.Description = dataReader.GetString(4);
                model.OperateTime = dataReader.GetDateTime(5);
                List.Add(model);
            }
            return List;
        }
        /// <summary>
        /// 查询所有记录
        /// </summary>
        public List<EADS.Model.Model_Order_Operate_Log> GetAll()
        {
            string sql = "SELECT * FROM torder_operate_log";
            MySqlDataReader dataReader = dbHelper.GetDataReader(sql,null);
            List<EADS.Model.Model_Order_Operate_Log> List = DataReaderToList(dataReader);
            dataReader.Close();
            return List;
        }
        /// <summary>
        /// 查询记录数
        /// </summary>
        public long GetCount()
        {
            string sql = "SELECT COUNT(*) FROM torder_operate_log";
            long count;
            return long.TryParse(dbHelper.GetSingle(sql).ToString(), out count) ? count : 0;
        }
        /// <summary>
        /// 根据主键查询一条记录
        /// </summary>
        public EADS.Model.Model_Order_Operate_Log Get(int id)
        {
            string sql = "SELECT * FROM torder_operate_log WHERE ID=@ID";
            MySqlParameter[] parameters = new MySqlParameter[]{
                new MySqlParameter("@ID", MySqlDbType.Int32, 11){ Value = id }
            };
            MySqlDataReader dataReader = dbHelper.GetDataReader(sql, parameters);
            List<EADS.Model.Model_Order_Operate_Log> List = DataReaderToList(dataReader);
            dataReader.Close();
            return List.Count > 0 ? List[0] : null;
        }

    }
}
