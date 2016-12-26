using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using EADS.Model;

namespace EADS.DAL
{ 
	public class DAL_Photo : DAL_Bases
    {
        public DAL_Photo()
        {
        }
 

        /// <summary>
        /// 添加记录
        /// </summary>
        /// <param name="model">EADS.Model.tphoto实体类</param>
        /// <returns>新增记录的ID</returns>
        public int Add(Model_Photo model)
        {
            string sql = @"INSERT INTO tphoto
				(FileName,FileSize,CreateTime,DeleteTime,FilePath,State) 
				VALUES(@FileName,@FileSize,@CreateTime,@DeleteTime,@FilePath,@State);
				SELECT LAST_INSERT_ID();";
            MySqlParameter[] parameters = new MySqlParameter[]{
                new MySqlParameter("@FileName", MySqlDbType.VarChar, 50){ Value = model.FileName },
                new MySqlParameter("@FileSize", MySqlDbType.Float, -1){ Value = model.FileSize },
                new MySqlParameter("@CreateTime", MySqlDbType.DateTime, -1){ Value = model.CreateTime },
                model.DeleteTime == null ? new MySqlParameter("@DeleteTime", MySqlDbType.DateTime, -1) { Value = DBNull.Value } : new MySqlParameter("@DeleteTime", MySqlDbType.DateTime, -1) { Value = model.DeleteTime },
                model.FilePath == null ? new MySqlParameter("@FilePath", MySqlDbType.VarChar, 100) { Value = DBNull.Value } : new MySqlParameter("@FilePath", MySqlDbType.VarChar, 100) { Value = model.FilePath },
                new MySqlParameter("@State", MySqlDbType.Int16, 4){ Value = model.State }
            };
            int maxID = -1;
            dbHelper.ExecuteScalar(sql, parameters, out maxID);
            return maxID;
        }
        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="model">EADS.Model.tphoto实体类</param>
        public bool Update(Model_Photo model)
        {
            string sql = @"UPDATE tphoto SET 
				FileName=@FileName,FileSize=@FileSize,CreateTime=@CreateTime,DeleteTime=@DeleteTime,FilePath=@FilePath,State=@State
				WHERE ID=@ID";
            MySqlParameter[] parameters = new MySqlParameter[]{
                new MySqlParameter("@FileName", MySqlDbType.VarChar, 50){ Value = model.FileName },
                new MySqlParameter("@FileSize", MySqlDbType.Float, -1){ Value = model.FileSize },
                new MySqlParameter("@CreateTime", MySqlDbType.DateTime, -1){ Value = model.CreateTime },
                model.DeleteTime == null ? new MySqlParameter("@DeleteTime", MySqlDbType.DateTime, -1) { Value = DBNull.Value } : new MySqlParameter("@DeleteTime", MySqlDbType.DateTime, -1) { Value = model.DeleteTime },
                model.FilePath == null ? new MySqlParameter("@FilePath", MySqlDbType.VarChar, 100) { Value = DBNull.Value } : new MySqlParameter("@FilePath", MySqlDbType.VarChar, 100) { Value = model.FilePath },
                new MySqlParameter("@State", MySqlDbType.Int16, 4){ Value = model.State },
                new MySqlParameter("@ID", MySqlDbType.Int32, 11){ Value = model.ID }
            };
            return dbHelper.ExecuteSql(sql, parameters) > 0;
        }
        /// <summary>
        /// 删除记录
        /// </summary>
        public int Delete(int id)
        {
            string sql = "DELETE FROM tphoto WHERE ID=@ID";
            MySqlParameter[] parameters = new MySqlParameter[]{
                new MySqlParameter("@ID", MySqlDbType.Int32, 11){ Value = id }
            };
            return dbHelper.ExecuteSql(sql, parameters);
        }
        /// <summary>
        /// 将DataRedar转换为List
        /// </summary>
        private List<Model_Photo> DataReaderToList(MySqlDataReader dataReader)
        {
            List<Model_Photo> List = new List<Model_Photo>();
            Model_Photo model = null;
            while (dataReader.Read())
            {
                model = new Model_Photo();
                model.ID = dataReader.GetInt32(0);
                model.FileName = dataReader.GetString(1);
                model.FileSize = dataReader.GetFloat(2);
                model.CreateTime = dataReader.GetDateTime(3);
                if (!dataReader.IsDBNull(4))
                    model.DeleteTime = dataReader.GetDateTime(4);
                if (!dataReader.IsDBNull(5))
                    model.FilePath = dataReader.GetString(5);
                model.State = (byte)dataReader.GetInt16(6);
                List.Add(model);
            }
            return List;
        }
        /// <summary>
        /// 查询所有记录
        /// </summary>
        public List<Model_Photo> GetAll()
        {
            string sql = "SELECT * FROM tphoto";
            MySqlDataReader dataReader = dbHelper.GetDataReader(sql);
            List<Model_Photo> List = DataReaderToList(dataReader);
            dataReader.Close();
            return List;
        }
        /// <summary>
        /// 查询记录数
        /// </summary>
        public long GetCount()
        {
            string sql = "SELECT COUNT(*) FROM tphoto";
            long count;
            return long.TryParse(dbHelper.GetSingle(sql).ToString(), out count) ? count : 0;
        }
        /// <summary>
        /// 根据主键查询一条记录
        /// </summary>
        public Model_Photo Get(int id)
        {
            string sql = "SELECT * FROM tphoto WHERE ID=@ID";
            MySqlParameter[] parameters = new MySqlParameter[]{
                new MySqlParameter("@ID", MySqlDbType.Int32, 11){ Value = id }
            };
            MySqlDataReader dataReader = dbHelper.GetDataReader(sql, parameters);
            List<Model_Photo> List = DataReaderToList(dataReader);
            dataReader.Close();
            return List.Count > 0 ? List[0] : null;
        }
    }
}
