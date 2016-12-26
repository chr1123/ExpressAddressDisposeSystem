using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using EADS.Model;
using System.Text;
using System.Data;

namespace EADS.DAL
{
    public class DAL_Order : DAL_Bases
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        public DAL_Order()
        {
        }
        /// <summary>
        /// 添加记录
        /// </summary>
        /// <param name="model">EADS.Model.torder实体类</param>
        /// <returns>新增记录的ID</returns>
        public int Add(Model_Order model)
        {
            string sql = @"INSERT INTO torder
				(OrderNO,DestinationCity,SenderName,SenderPhone,SenderAddress,RecipientName,RecipientPhone,RecipientAddress,Goods,State,ResultCode,DisposeUserId,CreateTime,FinishTime,Remark,ImagePath) 
				VALUES(@OrderNO,@DestinationCity,@SenderName,@SenderPhone,@SenderAddress,@RecipientName,@RecipientPhone,@RecipientAddress,@Goods,@State,@ResultCode,@DisposeUserId,@CreateTime,@FinishTime,@Remark,@ImagePath);
				SELECT LAST_INSERT_ID();";
            MySqlParameter[] parameters = new MySqlParameter[]{
                new MySqlParameter("@OrderNO", MySqlDbType.VarChar, 40){ Value = model.OrderNO },
                model.DestinationCity == null ? new MySqlParameter("@DestinationCity", MySqlDbType.VarChar, 40) { Value = DBNull.Value } : new MySqlParameter("@DestinationCity", MySqlDbType.VarChar, 40) { Value = model.DestinationCity },
                model.SenderName == null ? new MySqlParameter("@SenderName", MySqlDbType.VarChar, 20) { Value = DBNull.Value } : new MySqlParameter("@SenderName", MySqlDbType.VarChar, 20) { Value = model.SenderName },
                model.SenderPhone == null ? new MySqlParameter("@SenderPhone", MySqlDbType.VarChar, 20) { Value = DBNull.Value } : new MySqlParameter("@SenderPhone", MySqlDbType.VarChar, 20) { Value = model.SenderPhone },
                model.SenderAddress == null ? new MySqlParameter("@SenderAddress", MySqlDbType.VarChar, 40) { Value = DBNull.Value } : new MySqlParameter("@SenderAddress", MySqlDbType.VarChar, 40) { Value = model.SenderAddress },
                model.RecipientName == null ? new MySqlParameter("@RecipientName", MySqlDbType.VarChar, 20) { Value = DBNull.Value } : new MySqlParameter("@RecipientName", MySqlDbType.VarChar, 20) { Value = model.RecipientName },
                model.RecipientPhone == null ? new MySqlParameter("@RecipientPhone", MySqlDbType.VarChar, 30) { Value = DBNull.Value } : new MySqlParameter("@RecipientPhone", MySqlDbType.VarChar, 30) { Value = model.RecipientPhone },
                model.RecipientAddress == null ? new MySqlParameter("@RecipientAddress", MySqlDbType.VarChar, 40) { Value = DBNull.Value } : new MySqlParameter("@RecipientAddress", MySqlDbType.VarChar, 40) { Value = model.RecipientAddress },
                model.Goods == null ? new MySqlParameter("@Goods", MySqlDbType.VarChar, 20) { Value = DBNull.Value } : new MySqlParameter("@Goods", MySqlDbType.VarChar, 20) { Value = model.Goods },
                new MySqlParameter("@State", MySqlDbType.Int16, 4){ Value = model.State },
                new MySqlParameter("@ResultCode", MySqlDbType.Int16, 4){ Value = model.ResultCode },
                new MySqlParameter("@DisposeUserId", MySqlDbType.Int32, 11){ Value = model.DisposeUserId },
                new MySqlParameter("@CreateTime", MySqlDbType.DateTime, -1){ Value = model.CreateTime },
                model.FinishTime == null ? new MySqlParameter("@FinishTime", MySqlDbType.DateTime, -1) { Value = DBNull.Value } : new MySqlParameter("@FinishTime", MySqlDbType.DateTime, -1) { Value = model.FinishTime },
                model.Remark == null ? new MySqlParameter("@Remark", MySqlDbType.VarChar, 100) { Value = DBNull.Value } : new MySqlParameter("@Remark", MySqlDbType.VarChar, 100) { Value = model.Remark },
                model.ImagePath == null ? new MySqlParameter("@ImagePath", MySqlDbType.VarChar, 100) { Value = DBNull.Value } : new MySqlParameter("@ImagePath", MySqlDbType.VarChar, 100) { Value = model.ImagePath }
            };
            int maxID = -1;
            dbHelper.ExecuteScalar(sql, parameters, out maxID);
            return maxID;
        }
        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="model">EADS.Model.torder实体类</param>
        public bool Update(Model_Order model)
        {
            string sql = @"UPDATE torder SET 
				OrderNO=@OrderNO,DestinationCity=@DestinationCity,SenderName=@SenderName,SenderPhone=@SenderPhone,SenderAddress=@SenderAddress,RecipientName=@RecipientName,RecipientPhone=@RecipientPhone,RecipientAddress=@RecipientAddress,Goods=@Goods,State=@State,ResultCode=@ResultCode,DisposeUserId=@DisposeUserId,CreateTime=@CreateTime,FinishTime=@FinishTime,Remark=@Remark,ImagePath=@ImagePath
				WHERE ID=@ID";
            MySqlParameter[] parameters = new MySqlParameter[]{
                new MySqlParameter("@OrderNO", MySqlDbType.VarChar, 40){ Value = model.OrderNO },
                model.DestinationCity == null ? new MySqlParameter("@DestinationCity", MySqlDbType.VarChar, 40) { Value = DBNull.Value } : new MySqlParameter("@DestinationCity", MySqlDbType.VarChar, 40) { Value = model.DestinationCity },
                model.SenderName == null ? new MySqlParameter("@SenderName", MySqlDbType.VarChar, 20) { Value = DBNull.Value } : new MySqlParameter("@SenderName", MySqlDbType.VarChar, 20) { Value = model.SenderName },
                model.SenderPhone == null ? new MySqlParameter("@SenderPhone", MySqlDbType.VarChar, 20) { Value = DBNull.Value } : new MySqlParameter("@SenderPhone", MySqlDbType.VarChar, 20) { Value = model.SenderPhone },
                model.SenderAddress == null ? new MySqlParameter("@SenderAddress", MySqlDbType.VarChar, 40) { Value = DBNull.Value } : new MySqlParameter("@SenderAddress", MySqlDbType.VarChar, 40) { Value = model.SenderAddress },
                model.RecipientName == null ? new MySqlParameter("@RecipientName", MySqlDbType.VarChar, 20) { Value = DBNull.Value } : new MySqlParameter("@RecipientName", MySqlDbType.VarChar, 20) { Value = model.RecipientName },
                model.RecipientPhone == null ? new MySqlParameter("@RecipientPhone", MySqlDbType.VarChar, 30) { Value = DBNull.Value } : new MySqlParameter("@RecipientPhone", MySqlDbType.VarChar, 30) { Value = model.RecipientPhone },
                model.RecipientAddress == null ? new MySqlParameter("@RecipientAddress", MySqlDbType.VarChar, 40) { Value = DBNull.Value } : new MySqlParameter("@RecipientAddress", MySqlDbType.VarChar, 40) { Value = model.RecipientAddress },
                model.Goods == null ? new MySqlParameter("@Goods", MySqlDbType.VarChar, 20) { Value = DBNull.Value } : new MySqlParameter("@Goods", MySqlDbType.VarChar, 20) { Value = model.Goods },
                new MySqlParameter("@State", MySqlDbType.Int16, 4){ Value = model.State },
                new MySqlParameter("@ResultCode", MySqlDbType.Int16, 4){ Value = model.ResultCode },
                new MySqlParameter("@DisposeUserId", MySqlDbType.Int32, 11){ Value = model.DisposeUserId },
                new MySqlParameter("@CreateTime", MySqlDbType.DateTime, -1){ Value = model.CreateTime },
                model.FinishTime == null ? new MySqlParameter("@FinishTime", MySqlDbType.DateTime, -1) { Value = DBNull.Value } : new MySqlParameter("@FinishTime", MySqlDbType.DateTime, -1) { Value = model.FinishTime },
                model.Remark == null ? new MySqlParameter("@Remark", MySqlDbType.VarChar, 100) { Value = DBNull.Value } : new MySqlParameter("@Remark", MySqlDbType.VarChar, 100) { Value = model.Remark },
                model.ImagePath == null ? new MySqlParameter("@ImagePath", MySqlDbType.VarChar, 100) { Value = DBNull.Value } : new MySqlParameter("@ImagePath", MySqlDbType.VarChar, 100) { Value = model.ImagePath },
                new MySqlParameter("@ID", MySqlDbType.Int32, 11){ Value = model.ID }
            };
            return dbHelper.ExecuteSql(sql, parameters) > 0;
        }

        /// <summary>
        ///将指定的订单状态更改为正在回传中
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool UpdateStateAsCallbacking(string ids)
        {
            string sql = "UPDATE torder SET  State=";
            sql += Model_Order.STATE_CALLBACKING;
            sql += " WHERE ID in (";
            sql += ids;
            sql += ")";
            return dbHelper.ExecuteSql(sql,null) > 0;
        }

        public bool UpdateStateAsWaitForCallback(string ids)
        {
            string sql = "UPDATE torder SET  State=";
            sql += Model_Order.STATE_HANDLED_WAIT_FOR_CALLBACK;
            sql += " WHERE ID in (";
            sql += ids;
            sql += ")";
            return dbHelper.ExecuteSql(sql, null) > 0;
        }

        /// <summary>
        /// 删除记录
        /// </summary>
        public int Delete(int id)
        {
            string sql = "DELETE FROM torder WHERE ID=@ID";
            MySqlParameter[] parameters = new MySqlParameter[]{
                new MySqlParameter("@ID", MySqlDbType.Int32, 11){ Value = id }
            };
            return dbHelper.ExecuteSql(sql, parameters);
        }
        /// <summary>
        /// 将DataRedar转换为List
        /// </summary>
        private List<Model_Order> DataReaderToList(MySqlDataReader dataReader)
        {
            List<Model_Order> List = new List<Model_Order>();
            Model_Order model = null;
            while (dataReader.Read())
            {
                model = new Model_Order();
                model.ID = dataReader.GetInt32(0);
                model.OrderNO = dataReader.GetString(1);
                if (!dataReader.IsDBNull(2))
                    model.DestinationCity = dataReader.GetString(2);
                if (!dataReader.IsDBNull(3))
                    model.SenderName = dataReader.GetString(3);
                if (!dataReader.IsDBNull(4))
                    model.SenderPhone = dataReader.GetString(4);
                if (!dataReader.IsDBNull(5))
                    model.SenderAddress = dataReader.GetString(5);
                if (!dataReader.IsDBNull(6))
                    model.RecipientName = dataReader.GetString(6);
                if (!dataReader.IsDBNull(7))
                    model.RecipientPhone = dataReader.GetString(7);
                if (!dataReader.IsDBNull(8))
                    model.RecipientAddress = dataReader.GetString(8);
                if (!dataReader.IsDBNull(9))
                    model.Goods = dataReader.GetString(9);
                model.State = (byte)dataReader.GetInt16(10);
                model.ResultCode = (byte)dataReader.GetInt16(11);
                model.DisposeUserId = dataReader.GetInt32(12);
                model.CreateTime = dataReader.GetDateTime(13);
                if (!dataReader.IsDBNull(14))
                    model.FinishTime = dataReader.GetDateTime(14);
                if (!dataReader.IsDBNull(15))
                    model.Remark = dataReader.GetString(15);
                if (!dataReader.IsDBNull(16))
                    model.ImagePath = dataReader.GetString(16);
                List.Add(model);
            }
            return List;
        }


        private Model_Order DataRowToModel(DataRow row)
        {
            Model_Order model = new Model_Order();
            model.ID = int.Parse(row["ID"].ToString());
            model.OrderNO = row["OrderNO"].ToString();
            model.DestinationCity = row["DestinationCity"].ToString();  
            model.RecipientAddress = row["RecipientAddress"].ToString();
            model.State = byte.Parse(row["State"].ToString());
            model.ResultCode = byte.Parse(row["ResultCode"].ToString());
            model.DisposeUserId = int.Parse(row["DisposeUserId"].ToString());
            model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
            model.CallbackResult = byte.Parse(row["CallbackResult"].ToString());
            if (!string.IsNullOrEmpty(row["TakeTime"].ToString()))
            {
                model.TakeTime = DateTime.Parse(row["TakeTime"].ToString());
            }
            if (!string.IsNullOrEmpty(row["HandleTime"].ToString()))
            {
                model.HandleTime = DateTime.Parse(row["HandleTime"].ToString());
            }
            if (!string.IsNullOrEmpty(row["FinishTime"].ToString()))
            {
                model.FinishTime = DateTime.Parse(row["FinishTime"].ToString());
            }
           // model.Remark = row["Remark"].ToString();
            model.ImagePath = row["ImagePath"].ToString();


            return model;
        }

        public List<Model_Order> GetListByOrderIds(string ids)
        {
            string sql = "SELECT * FROM torder where ID in (";
            sql += ids;
            sql += ")";
            MySqlDataReader dataReader = dbHelper.GetDataReader(sql);
            List<Model_Order> List = DataReaderToList(dataReader);
            dataReader.Close();
            return List;
        }

        /// <summary>
        /// 查询记录数
        /// </summary>
        public long GetCount()
        {
            string sql = "SELECT COUNT(*) FROM torder";
            long count;
            return long.TryParse(dbHelper.GetSingle(sql).ToString(), out count) ? count : 0;
        }
        /// <summary>
        /// 根据主键查询一条记录
        /// </summary>
        public Model_Order GetModel(int id)
        {
            string sql = "SELECT * FROM torder WHERE ID=@ID";
            MySqlParameter[] parameters = new MySqlParameter[]{
                new MySqlParameter("@ID", MySqlDbType.Int32, 11){ Value = id }
            };
            MySqlDataReader dataReader = dbHelper.GetDataReader(sql, parameters);
            List<Model_Order> List = DataReaderToList(dataReader);
            dataReader.Close();
            return List.Count > 0 ? List[0] : null;
        }


        public DataSet GetListByPage(string where, int page, int pageSize, out int total)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM torder ");
            if (!string.IsNullOrEmpty(where))
            {
                sb.Append(" WHERE ");
                sb.Append(where);
            }
            sb.Append(" order by  ID ");
            sb.Append(" limit ");
            sb.Append(page * pageSize);
            sb.Append(",");
            sb.Append(pageSize);
            DataSet ds = dbHelper.GetList(sb.ToString(), null);

            StringBuilder sbCount = new StringBuilder();
            sbCount.Append("SELECT COUNT(ID) FROM torder ");
            if (!string.IsNullOrEmpty(where))
            {
                sbCount.Append(" WHERE ");
                sbCount.Append(where);
            }
            total = int.Parse(dbHelper.GetSingle(sbCount.ToString(), null).ToString());
            return ds;
        }

        public bool UpdateState(int id, int state)
        {
            string sql = @"UPDATE torder SET  State=@State WHERE ID=@ID";
            MySqlParameter[] parameters = new MySqlParameter[]{
               new MySqlParameter("@State", MySqlDbType.Int16, 4){ Value = state },
                new MySqlParameter("@ID", MySqlDbType.Int32, 11){ Value=id }
            };
            return dbHelper.ExecuteSql(sql, parameters) > 0;
        }

        public bool UpdateAddressInfo(Model_Order model)
        {
            string sql = @"UPDATE torder SET 
			    DestinationCity=@DestinationCity,SenderName=@SenderName,SenderPhone=@SenderPhone,
                SenderAddress=@SenderAddress,RecipientName=@RecipientName,RecipientPhone=@RecipientPhone,
                RecipientAddress=@RecipientAddress,Goods=@Goods,State=@State,
                ResultCode=@ResultCode, 
                HandleTime=@HandleTime
				WHERE ID=@ID";
              MySqlParameter[] parameters = new MySqlParameter[]{
                model.DestinationCity == null ? new MySqlParameter("@DestinationCity", MySqlDbType.VarChar, 40) { Value = DBNull.Value } : new MySqlParameter("@DestinationCity", MySqlDbType.VarChar, 40) { Value = model.DestinationCity },
                model.SenderName == null ? new MySqlParameter("@SenderName", MySqlDbType.VarChar, 20) { Value = DBNull.Value } : new MySqlParameter("@SenderName", MySqlDbType.VarChar, 20) { Value = model.SenderName },
                model.SenderPhone == null ? new MySqlParameter("@SenderPhone", MySqlDbType.VarChar, 20) { Value = DBNull.Value } : new MySqlParameter("@SenderPhone", MySqlDbType.VarChar, 20) { Value = model.SenderPhone },
                model.SenderAddress == null ? new MySqlParameter("@SenderAddress", MySqlDbType.VarChar, 40) { Value = DBNull.Value } : new MySqlParameter("@SenderAddress", MySqlDbType.VarChar, 40) { Value = model.SenderAddress },
                model.RecipientName == null ? new MySqlParameter("@RecipientName", MySqlDbType.VarChar, 20) { Value = DBNull.Value } : new MySqlParameter("@RecipientName", MySqlDbType.VarChar, 20) { Value = model.RecipientName },
                model.RecipientPhone == null ? new MySqlParameter("@RecipientPhone", MySqlDbType.VarChar, 30) { Value = DBNull.Value } : new MySqlParameter("@RecipientPhone", MySqlDbType.VarChar, 30) { Value = model.RecipientPhone },
                model.RecipientAddress == null ? new MySqlParameter("@RecipientAddress", MySqlDbType.VarChar, 40) { Value = DBNull.Value } : new MySqlParameter("@RecipientAddress", MySqlDbType.VarChar, 40) { Value = model.RecipientAddress },
                model.Goods == null ? new MySqlParameter("@Goods", MySqlDbType.VarChar, 20) { Value = DBNull.Value } : new MySqlParameter("@Goods", MySqlDbType.VarChar, 20) { Value = model.Goods },
                new MySqlParameter("@State", MySqlDbType.Int16, 4){ Value = model.State },
                new MySqlParameter("@ResultCode", MySqlDbType.Int16, 4){ Value = model.ResultCode }, 
                model.HandleTime == null ? new MySqlParameter("@HandleTime", MySqlDbType.DateTime, -1) { Value = DBNull.Value } : new MySqlParameter("@HandleTime", MySqlDbType.DateTime, -1) { Value = model.HandleTime },
                new MySqlParameter("@ID", MySqlDbType.Int32, 11){ Value = model.ID }
            };
            return dbHelper.ExecuteSql(sql, parameters) > 0;
        }

        public int TakeOrder(int userId, int count = 100)
        {//此处userid应判断是否存在
            //获取指定条数的未被领取的订单的ID集合
            string strGetIds = "SELECT group_concat(ID) from (select ID from torder where State = 0 and DisposeUserId=0 Order by ID asc limit 0,";
            strGetIds += count;
            strGetIds += ") ta ;";
            string ids = dbHelper.GetSingle(strGetIds).ToString();
            if (string.IsNullOrEmpty(ids)) return 0;//当前已无没被领取的订单
            //得到的未被订单的数量
            int getCount = ids.Length - ids.Replace(",", "").Length + 1;
            dbHelper.BeginTransaction();
            string strUpdate = "UPDATE torder set DisposeUserId=" + userId;
            strUpdate += ",TakeTime='";
            strUpdate += DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            strUpdate += "',State=1 WHERE ID in(";
            strUpdate += ids;
            strUpdate += ") and  State = 0 and DisposeUserId=0 ;";
            int updateRows = dbHelper.ExecuteSql(strUpdate);
            if (getCount != updateRows)
            {
                dbHelper.Rollback();
                return -1;//存在异常 回滚
            }
            else
            {
                dbHelper.Commit();
                return updateRows;
            }
        }

        /// <summary>
        /// 获取用户第一条应处理的订单 同时返回订单总数及待完成订单数量
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="count"></param>
        /// <param name="unHandle"></param>
        /// <returns></returns>
        public Model_Order getUserFirstUnHandlerOrder(int userId, out int count, out int unHandle)
        {
            StringBuilder sbCount = new StringBuilder();
            sbCount.Append("SELECT count(ID) as OrderCount, sum(case State when 1 then 1 else 0 end) as WaitHandle ");
            sbCount.Append("FROM expressaddress.torder where DisposeUserId = ");
            sbCount.Append(userId);
            DataSet ds = dbHelper.GetDataBySql(sbCount.ToString());
            count = 0;
            unHandle = 0;
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                count = string.IsNullOrEmpty(ds.Tables[0].Rows[0][0].ToString()) ? 0 :
                    int.Parse(ds.Tables[0].Rows[0][0].ToString());
                unHandle = string.IsNullOrEmpty(ds.Tables[0].Rows[0][1].ToString()) ? 0 :
                    int.Parse(ds.Tables[0].Rows[0][1].ToString());
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("select * from torder where DisposeUserId=");
            sb.Append(userId);
            sb.Append(" and State=1 order by TakeTime ASC limit 0,1");
            DataSet dsOrder = dbHelper.GetDataBySql(sb.ToString());
            if (dsOrder != null && dsOrder.Tables.Count > 0 && dsOrder.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(dsOrder.Tables[0].Rows[0]);
            }
            else {
                return null;
            }
        }


        public List<Model_Order> GetList(string strWhere,string orderBy="")
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT ID,OrderNO,DestinationCity,RecipientAddress,");
            sql.Append("State,ResultCode,DisposeUserId,CreateTime,TakeTime,HandleTime,CallbackResult,FinishTime,ImagePath FROM torder "); 
            if (!string.IsNullOrEmpty(strWhere.Trim())) {
                sql.Append(" where ");
                sql.Append(strWhere);
            }
            if (!string.IsNullOrEmpty(orderBy.Trim()))
            {
                sql.Append(" ORDER BY ");
                sql.Append(orderBy);
            }
            List<Model_Order> list = new List<Model_Order>(); 
            DataSet ds = dbHelper.GetDataBySql(sql.ToString());
            if (ds != null)
            {
                foreach (DataRow row in ds.Tables[0].Rows) {
                    list.Add(DataRowToModel(row));
                }
            } 
            return list;
        }


        public bool UpdateCallbackResult(string ids,byte resultCode)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("UPDATE torder SET  State=");
            sql.Append(Model_Order.STATE_FINISHED);
            sql.Append(",CallbackResult=");
            sql.Append(resultCode+ ",FinishTime='");
            sql.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            sql.Append("' WHERE ID IN (");
            sql.Append(ids);
            sql.Append(");"); 
            return dbHelper.ExecuteSql(sql.ToString()) > 0;
        }


    }
}
