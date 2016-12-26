using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using EADS.Model;


namespace EADS.DAL
{
	 
	public class DAL_User_Group : DAL_Bases
    { 
        /// <summary>
        /// 构造函数
        /// </summary>
        public DAL_User_Group()
        {
        }
        /// <summary>
        /// 添加记录
        /// </summary>
        /// <param name="model">Model_User_Group实体类</param>
        /// <returns>新增记录的ID</returns>
        public int Add(Model_User_Group model)
        {
            string sql = @"INSERT INTO tuser_group
				(GroupName,TheHead,Phone,ProID,CityID,AreaID,Address,CreateTime,Remark) 
				VALUES(@GroupName,@TheHead,@Phone,@ProID,@CityID,@AreaID,@Address,@CreateTime,@Remark);
				SELECT LAST_INSERT_ID();";
            MySqlParameter[] parameters = new MySqlParameter[]{
                new MySqlParameter("@GroupName", MySqlDbType.VarChar, 40){ Value = model.GroupName },
                model.TheHead == null ? new MySqlParameter("@TheHead", MySqlDbType.VarChar, 20) { Value = DBNull.Value } : new MySqlParameter("@TheHead", MySqlDbType.VarChar, 20) { Value = model.TheHead },
                model.Phone == null ? new MySqlParameter("@Phone", MySqlDbType.VarChar, 20) { Value = DBNull.Value } : new MySqlParameter("@Phone", MySqlDbType.VarChar, 20) { Value = model.Phone },
                model.ProID == null ? new MySqlParameter("@ProID", MySqlDbType.Int32, 11) { Value = DBNull.Value } : new MySqlParameter("@ProID", MySqlDbType.Int32, 11) { Value = model.ProID },
                model.CityID == null ? new MySqlParameter("@CityID", MySqlDbType.Int32, 11) { Value = DBNull.Value } : new MySqlParameter("@CityID", MySqlDbType.Int32, 11) { Value = model.CityID },
                model.AreaID == null ? new MySqlParameter("@AreaID", MySqlDbType.Int32, 11) { Value = DBNull.Value } : new MySqlParameter("@AreaID", MySqlDbType.Int32, 11) { Value = model.AreaID },
                model.Address == null ? new MySqlParameter("@Address", MySqlDbType.VarChar, 40) { Value = DBNull.Value } : new MySqlParameter("@Address", MySqlDbType.VarChar, 40) { Value = model.Address },
                new MySqlParameter("@CreateTime", MySqlDbType.DateTime, -1){ Value = model.CreateTime },
                model.Remark == null ? new MySqlParameter("@Remark", MySqlDbType.VarChar, 100) { Value = DBNull.Value } : new MySqlParameter("@Remark", MySqlDbType.VarChar, 100) { Value = model.Remark }
            };
            int maxID = -1;
            dbHelper.ExecuteScalar(sql, parameters, out maxID);
            return maxID;
        }
        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="model">Model_User_Group实体类</param>
        public bool Update(Model_User_Group model)
        {
            string sql = @"UPDATE tuser_group SET 
				GroupName=@GroupName,TheHead=@TheHead,Phone=@Phone,ProID=@ProID,CityID=@CityID,AreaID=@AreaID,Address=@Address,CreateTime=@CreateTime,Remark=@Remark
				WHERE ID=@ID";
            MySqlParameter[] parameters = new MySqlParameter[]{
                new MySqlParameter("@GroupName", MySqlDbType.VarChar, 40){ Value = model.GroupName },
                model.TheHead == null ? new MySqlParameter("@TheHead", MySqlDbType.VarChar, 20) { Value = DBNull.Value } : new MySqlParameter("@TheHead", MySqlDbType.VarChar, 20) { Value = model.TheHead },
                model.Phone == null ? new MySqlParameter("@Phone", MySqlDbType.VarChar, 20) { Value = DBNull.Value } : new MySqlParameter("@Phone", MySqlDbType.VarChar, 20) { Value = model.Phone },
                model.ProID == null ? new MySqlParameter("@ProID", MySqlDbType.Int32, 11) { Value = DBNull.Value } : new MySqlParameter("@ProID", MySqlDbType.Int32, 11) { Value = model.ProID },
                model.CityID == null ? new MySqlParameter("@CityID", MySqlDbType.Int32, 11) { Value = DBNull.Value } : new MySqlParameter("@CityID", MySqlDbType.Int32, 11) { Value = model.CityID },
                model.AreaID == null ? new MySqlParameter("@AreaID", MySqlDbType.Int32, 11) { Value = DBNull.Value } : new MySqlParameter("@AreaID", MySqlDbType.Int32, 11) { Value = model.AreaID },
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
            string sql = "DELETE FROM tuser_group WHERE ID=@ID";
            MySqlParameter[] parameters = new MySqlParameter[]{
                new MySqlParameter("@ID", MySqlDbType.Int32, 11){ Value = id }
            };
            return dbHelper.ExecuteSql(sql, parameters);
        }
        /// <summary>
        /// 将DataRedar转换为List
        /// </summary>
        private List<Model_User_Group> DataReaderToList(MySqlDataReader dataReader)
        {
            List<Model_User_Group> List = new List<Model_User_Group>();
            Model_User_Group model = null;
            while (dataReader.Read())
            {
                model = new Model_User_Group();
                model.ID = dataReader.GetInt32(0);
                model.GroupName = dataReader.GetString(1);
                if (!dataReader.IsDBNull(2))
                    model.TheHead = dataReader.GetString(2);
                if (!dataReader.IsDBNull(3))
                    model.Phone = dataReader.GetString(3);
                if (!dataReader.IsDBNull(4))
                    model.ProID = dataReader.GetInt32(4);
                if (!dataReader.IsDBNull(5))
                    model.CityID = dataReader.GetInt32(5);
                if (!dataReader.IsDBNull(6))
                    model.AreaID = dataReader.GetInt32(6);
                if (!dataReader.IsDBNull(7))
                    model.Address = dataReader.GetString(7);
                model.CreateTime = dataReader.GetDateTime(8);
                if (!dataReader.IsDBNull(9))
                    model.Remark = dataReader.GetString(9);
                List.Add(model);
            }
            return List;
        }
        /// <summary>
        /// 查询所有记录
        /// </summary>
        public List<Model_User_Group> GetAll()
        {
            string sql = "SELECT * FROM tuser_group";
            MySqlDataReader dataReader = dbHelper.GetDataReader(sql);
            List<Model_User_Group> List = DataReaderToList(dataReader);
            dataReader.Close();
            return List;
        }
        /// <summary>
        /// 查询记录数
        /// </summary>
        public long GetCount()
        {
            string sql = "SELECT COUNT(*) FROM tuser_group";
            long count;
            return long.TryParse(dbHelper.GetSingle(sql).ToString(), out count) ? count : 0;
        }
        /// <summary>
        /// 根据主键查询一条记录
        /// </summary>
        public Model_User_Group Get(int id)
        {
            string sql = "SELECT * FROM tuser_group WHERE ID=@ID";
            MySqlParameter[] parameters = new MySqlParameter[]{
                new MySqlParameter("@ID", MySqlDbType.Int32, 11){ Value = id }
            };
            MySqlDataReader dataReader = dbHelper.GetDataReader(sql, parameters);
            List<Model_User_Group> List = DataReaderToList(dataReader);
            dataReader.Close();
            return List.Count > 0 ? List[0] : null;
        }
    }
}
