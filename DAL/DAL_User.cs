using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using EADS.Model;
using System.Data;

namespace EADS.DAL
{
 
	public class DAL_User : DAL_Bases
    { 
        /// <summary>
        /// 构造函数
        /// </summary>
        public DAL_User()
        {
        }
        /// <summary>
        /// 添加记录
        /// </summary>
        /// <param name="model">DAL_User实体类</param>
        /// <returns>新增记录的ID</returns>
        public int Add(Model_User model)
        {
            string sql = @"INSERT INTO tuser
				(UserName,Password,GroupID,RealName,Phone,Sex,Age,Birthday,Address,CreateTime,Remark,State) 
				VALUES(@UserName,@Password,@GroupID,@RealName,@Phone,@Sex,@Age,@Birthday,@Address,@CreateTime,@Remark,@State);
				SELECT LAST_INSERT_ID();";
            MySqlParameter[] parameters = new MySqlParameter[]{
                model.UserName == null ? new MySqlParameter("@UserName", MySqlDbType.VarChar, 40) { Value = DBNull.Value } : new MySqlParameter("@UserName", MySqlDbType.VarChar, 40) { Value = model.UserName },
                model.Password == null ? new MySqlParameter("@Password", MySqlDbType.VarChar, 40) { Value = DBNull.Value } : new MySqlParameter("@Password", MySqlDbType.VarChar, 40) { Value = model.Password },
                new MySqlParameter("@GroupID", MySqlDbType.Int32, 11){ Value = model.GroupID },
                model.RealName == null ? new MySqlParameter("@RealName", MySqlDbType.VarChar, 20) { Value = DBNull.Value } : new MySqlParameter("@RealName", MySqlDbType.VarChar, 20) { Value = model.RealName },
                model.Phone == null ? new MySqlParameter("@Phone", MySqlDbType.VarChar, 20) { Value = DBNull.Value } : new MySqlParameter("@Phone", MySqlDbType.VarChar, 20) { Value = model.Phone },
                new MySqlParameter("@Sex", MySqlDbType.Int16, 4){ Value = model.Sex },
                new MySqlParameter("@Age", MySqlDbType.Int16, 4){ Value = model.Age },
                model.Birthday == null ? new MySqlParameter("@Birthday", MySqlDbType.DateTime, -1) { Value = DBNull.Value } : new MySqlParameter("@Birthday", MySqlDbType.DateTime, -1) { Value = model.Birthday },
                model.Address == null ? new MySqlParameter("@Address", MySqlDbType.VarChar, 40) { Value = DBNull.Value } : new MySqlParameter("@Address", MySqlDbType.VarChar, 40) { Value = model.Address },
                new MySqlParameter("@CreateTime", MySqlDbType.DateTime, -1){ Value = model.CreateTime },
                model.Remark == null ? new MySqlParameter("@Remark", MySqlDbType.VarChar, 100) { Value = DBNull.Value } : new MySqlParameter("@Remark", MySqlDbType.VarChar, 100) { Value = model.Remark },
                  new MySqlParameter("@State", MySqlDbType.Int32, 11){ Value = model.State }
            };
            int maxID = -1;
            dbHelper.ExecuteScalar(sql, parameters, out maxID);
            return maxID;
        }
        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="model">Model实体类</param>
        public bool Update(Model_User model)
        {
            string sql = @"UPDATE tuser SET 
				UserName=@UserName,Password=@Password,GroupID=@GroupID,RealName=@RealName,Phone=@Phone,Sex=@Sex,Age=@Age,Birthday=@Birthday,Address=@Address,CreateTime=@CreateTime,Remark=@Remark
				WHERE ID=@ID";
            MySqlParameter[] parameters = new MySqlParameter[]{
                model.UserName == null ? new MySqlParameter("@UserName", MySqlDbType.VarChar, 40) { Value = DBNull.Value } : new MySqlParameter("@UserName", MySqlDbType.VarChar, 40) { Value = model.UserName },
                model.Password == null ? new MySqlParameter("@Password", MySqlDbType.VarChar, 40) { Value = DBNull.Value } : new MySqlParameter("@Password", MySqlDbType.VarChar, 40) { Value = model.Password },
                new MySqlParameter("@GroupID", MySqlDbType.Int32, 11){ Value = model.GroupID },
                model.RealName == null ? new MySqlParameter("@RealName", MySqlDbType.VarChar, 20) { Value = DBNull.Value } : new MySqlParameter("@RealName", MySqlDbType.VarChar, 20) { Value = model.RealName },
                model.Phone == null ? new MySqlParameter("@Phone", MySqlDbType.VarChar, 20) { Value = DBNull.Value } : new MySqlParameter("@Phone", MySqlDbType.VarChar, 20) { Value = model.Phone },
                new MySqlParameter("@Sex", MySqlDbType.Int16, 4){ Value = model.Sex },
                new MySqlParameter("@Age", MySqlDbType.Int16, 4){ Value = model.Age },
                model.Birthday == null ? new MySqlParameter("@Birthday", MySqlDbType.DateTime, -1) { Value = DBNull.Value } : new MySqlParameter("@Birthday", MySqlDbType.DateTime, -1) { Value = model.Birthday },
                model.Address == null ? new MySqlParameter("@Address", MySqlDbType.VarChar, 40) { Value = DBNull.Value } : new MySqlParameter("@Address", MySqlDbType.VarChar, 40) { Value = model.Address },
                new MySqlParameter("@CreateTime", MySqlDbType.DateTime, -1){ Value = model.CreateTime },
                model.Remark == null ? new MySqlParameter("@Remark", MySqlDbType.VarChar, 100) { Value = DBNull.Value } : new MySqlParameter("@Remark", MySqlDbType.VarChar, 100) { Value = model.Remark },
                new MySqlParameter("@ID", MySqlDbType.Int32, 11){ Value = model.ID }
            };
            return dbHelper.ExecuteSql(sql, parameters) > 0;
        }
        /// <summary>
        /// 删除记录
        /// </summary>
        public int Delete(int id)
        {
            string sql = "DELETE FROM tuser WHERE ID=@ID";
            MySqlParameter[] parameters = new MySqlParameter[]{
                new MySqlParameter("@ID", MySqlDbType.Int32, 11){ Value = id }
            };
            return dbHelper.ExecuteSql(sql, parameters);
        }
        /// <summary>
        /// 将DataRedar转换为List
        /// </summary>
        private List<Model_User> DataReaderToList(MySqlDataReader dataReader)
        {
            List<Model_User> List = new List<Model_User>();
            Model_User model = null;
            while (dataReader.Read())
            {
                model = new Model_User();
                model.ID = dataReader.GetInt32(0);
                if (!dataReader.IsDBNull(1))
                    model.UserName = dataReader.GetString(1);
                if (!dataReader.IsDBNull(2))
                    model.Password = dataReader.GetString(2);
                model.GroupID = dataReader.GetInt32(3);
                if (!dataReader.IsDBNull(4))
                    model.RealName = dataReader.GetString(4);
                if (!dataReader.IsDBNull(5))
                    model.Phone = dataReader.GetString(5);
                model.Sex = (byte)dataReader.GetInt16(6);
                model.Age = (byte)dataReader.GetInt16(7);
                if (!dataReader.IsDBNull(8))
                    model.Birthday = dataReader.GetDateTime(8);
                if (!dataReader.IsDBNull(9))
                    model.Address = dataReader.GetString(9);
                model.CreateTime = dataReader.GetDateTime(10);
                if (!dataReader.IsDBNull(11))
                    model.Remark = dataReader.GetString(11);
                List.Add(model);
            }
            return List;
        }

        private Model_User DataRowToModel(DataRow row)
        {  
            Model_User model = new Model_User();
            model.ID = int.Parse(row["ID"].ToString());
            model.UserName = row["UserName"].ToString();
            model.Password = row["Password"].ToString();
            model.GroupID = int.Parse(row["GroupID"].ToString());
            model.RealName = row["RealName"].ToString();
            model.Phone = row["Phone"].ToString();
            model.Sex = byte.Parse(row["Sex"].ToString());
            model.Age = byte.Parse(row["Age"].ToString());
            if (!string.IsNullOrEmpty(row["Birthday"].ToString())) {
                model.Birthday = DateTime.Parse(row["Birthday"].ToString());
            } 
            model.Address = row["Address"].ToString();
            model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
            model.Remark = row["Remark"].ToString();
            model.State = int.Parse(row["State"].ToString());
            return model;
        }
        /// <summary>
        /// 查询所有记录
        /// </summary>
        public List<Model_User> GetAll()
        {
            string sql = "SELECT * FROM tuser";
            MySqlDataReader dataReader = dbHelper.GetDataReader(sql);
            List<Model_User> List = DataReaderToList(dataReader);
            dataReader.Close();
            return List;
        }
        /// <summary>
        /// 查询记录数
        /// </summary>
        public long GetCount()
        {
            string sql = "SELECT COUNT(*) FROM tuser";
            long count;
            return long.TryParse(dbHelper.GetSingle(sql).ToString(), out count) ? count : 0;
        }
        /// <summary>
        /// 根据主键查询一条记录
        /// </summary>
        public Model_User Get(int id)
        {
            string sql = "SELECT * FROM tuser WHERE ID=@ID";
            MySqlParameter[] parameters = new MySqlParameter[]{
                new MySqlParameter("@ID", MySqlDbType.Int32, 11){ Value = id }
            };
            MySqlDataReader dataReader = dbHelper.GetDataReader(sql, parameters);
            List<Model_User> List = DataReaderToList(dataReader);
            dataReader.Close();
            return List.Count > 0 ? List[0] : null;
        }

        public Model_User GetModelByUserNameAndPassword(string where)
        {
            string sql = "SELECT * FROM tuser ";
            if (!string.IsNullOrEmpty(where.Trim())) {
                sql += " where ";
                sql += where;
            } 
            DataSet ds = dbHelper.GetList(sql);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0) {
               return DataRowToModel(ds.Tables[0].Rows[0]);
            } 
            return null;
        }


        public DataSet GetListByPage(int page, int rows, string strWhere, out int total)
        {
            string sql = "SELECT * FROM tuser ";
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                sql += " where ";
                sql += strWhere;
            } 
            sql += " ORDER BY ID ASC ";
            sql += "limit ";
            sql += (page - 1) * rows;
            sql += ",";
            sql += rows;
            DataSet ds = dbHelper.GetList(sql);
            string sqlCount = "select count(ID) from tuser ";
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                sqlCount += " where ";
                sqlCount += strWhere;
            }
            total = int.Parse(dbHelper.GetSingle(sqlCount).ToString());
            return ds;
        }
        public bool UpdateState(int id, int state)
        {
            string sql = "update tuser set State = "+state+" where ID="+id;
            return dbHelper.ExecuteSql(sql) > 0;
        }

        public bool UpdatePassword(int id,string pwd)
        {
            string sql = "update tuser set Password = '" + pwd + "' where ID=" + id;
            return dbHelper.ExecuteSql(sql) > 0;
        }

        public bool UpdatePassword(int id, string oldPwd,string newPwd)
        {
            string sql = "update tuser set Password = '" + newPwd + "' where ID=" + id
                +" and Password='"+oldPwd+"'";
            return dbHelper.ExecuteSql(sql) > 0;
        }

    }
}
