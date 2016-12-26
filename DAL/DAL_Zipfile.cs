using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using EADS.Model;

namespace EADS.DAL
{ 
	public class DAL_Zipfile : DAL_Bases
    { 
        /// <summary>
        /// 构造函数
        /// </summary>
        public DAL_Zipfile()
        {
           
        }
         
         
        /// <summary>
        /// 添加记录
        /// </summary>
        /// <param name="model">Model_Zipfile实体类</param>
        /// <returns>新增记录的ID</returns>
        public int Add(Model_Zipfile model) 
        { 
            string sql = @"INSERT INTO tzipfile
				(ZipName,FileSize,Unziped,FileCount,CreateTime,UnzipTime,DeleteTime,FilePath,State) 
				VALUES(@ZipName,@FileSize,@Unziped,@FileCount,@CreateTime,@UnzipTime,@DeleteTime,@FilePath,@State);
				SELECT LAST_INSERT_ID();";
            MySqlParameter[] parameters = new MySqlParameter[]{
                new MySqlParameter("@ZipName", MySqlDbType.VarChar, 40){ Value = model.ZipName },
                new MySqlParameter("@FileSize", MySqlDbType.Float, -1){ Value = model.FileSize },
                new MySqlParameter("@Unziped", MySqlDbType.Int16, 4){ Value = model.Unziped },
                new MySqlParameter("@FileCount", MySqlDbType.Int16, 6){ Value = model.FileCount },
                new MySqlParameter("@CreateTime", MySqlDbType.DateTime, -1){ Value = model.CreateTime },
                model.UnzipTime == null ? new MySqlParameter("@UnzipTime", MySqlDbType.DateTime, -1) { Value = DBNull.Value } : new MySqlParameter("@UnzipTime", MySqlDbType.DateTime, -1) { Value = model.UnzipTime },
                model.DeleteTime == null ? new MySqlParameter("@DeleteTime", MySqlDbType.VarChar, 45) { Value = DBNull.Value } : new MySqlParameter("@DeleteTime", MySqlDbType.VarChar, 45) { Value = model.DeleteTime },
                model.FilePath == null ? new MySqlParameter("@FilePath", MySqlDbType.VarChar, 45) { Value = DBNull.Value } : new MySqlParameter("@FilePath", MySqlDbType.VarChar, 45) { Value = model.FilePath },
                new MySqlParameter("@State", MySqlDbType.Int16, 4){ Value = model.State }
            };
            int maxID;
            dbHelper.ExecuteScalar(sql, parameters, out maxID);
            return maxID;
        }
        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="model">Model_Zipfile实体类</param>
        public bool Update(Model_Zipfile model)
        {
            string sql = @"UPDATE tzipfile SET 
				ZipName=@ZipName,FileSize=@FileSize,Unziped=@Unziped,FileCount=@FileCount,CreateTime=@CreateTime,UnzipTime=@UnzipTime,DeleteTime=@DeleteTime,FilePath=@FilePath,State=@State
				WHERE ID=@ID";
            MySqlParameter[] parameters = new MySqlParameter[]{
                new MySqlParameter("@ZipName", MySqlDbType.VarChar, 40){ Value = model.ZipName },
                new MySqlParameter("@FileSize", MySqlDbType.Float, -1){ Value = model.FileSize },
                new MySqlParameter("@Unziped", MySqlDbType.Int16, 4){ Value = model.Unziped },
                new MySqlParameter("@FileCount", MySqlDbType.Int16, 6){ Value = model.FileCount },
                new MySqlParameter("@CreateTime", MySqlDbType.DateTime, -1){ Value = model.CreateTime },
                model.UnzipTime == null ? new MySqlParameter("@UnzipTime", MySqlDbType.DateTime, -1) { Value = DBNull.Value } : new MySqlParameter("@UnzipTime", MySqlDbType.DateTime, -1) { Value = model.UnzipTime },
                model.DeleteTime == null ? new MySqlParameter("@DeleteTime", MySqlDbType.VarChar, 45) { Value = DBNull.Value } : new MySqlParameter("@DeleteTime", MySqlDbType.VarChar, 45) { Value = model.DeleteTime },
                model.FilePath == null ? new MySqlParameter("@FilePath", MySqlDbType.VarChar, 45) { Value = DBNull.Value } : new MySqlParameter("@FilePath", MySqlDbType.VarChar, 45) { Value = model.FilePath },
                new MySqlParameter("@State", MySqlDbType.Int16, 4){ Value = model.State },
                new MySqlParameter("@ID", MySqlDbType.Int32, 11){ Value = model.ID }
            };
            return dbHelper.ExecuteSql(sql, parameters) > 0;
        }
        public bool UnzipedOver(Model_Zipfile model)
        {
            string sql = @"UPDATE tzipfile SET 
				 FileSize=@FileSize,Unziped=@Unziped,FileCount=@FileCount,
                UnzipTime=@UnzipTime,State=@State
				WHERE ID=@ID";
            MySqlParameter[] parameters = new MySqlParameter[]{
                new MySqlParameter("@FileSize", MySqlDbType.Float, -1){ Value = model.FileSize },
                new MySqlParameter("@Unziped", MySqlDbType.Int16, 4){ Value = model.Unziped },
                new MySqlParameter("@FileCount", MySqlDbType.Int16, 6){ Value = model.FileCount },
                model.UnzipTime == null ? new MySqlParameter("@UnzipTime", MySqlDbType.DateTime, -1) { Value = DBNull.Value } : new MySqlParameter("@UnzipTime", MySqlDbType.DateTime, -1) { Value = model.UnzipTime },
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
            string sql = "DELETE FROM tzipfile WHERE ID=@ID";
            MySqlParameter[] parameters = new MySqlParameter[]{
                new MySqlParameter("@ID", MySqlDbType.Int32, 11){ Value = id }
            };
            return dbHelper.ExecuteSql(sql, parameters);
        }
        /// <summary>
        /// 将DataRedar转换为List
        /// </summary>
        private List<Model_Zipfile> DataReaderToList(MySqlDataReader dataReader)
        {
            List<Model_Zipfile> List = new List<Model_Zipfile>();
            Model_Zipfile model = null;
            while (dataReader.Read())
            {
                model = new Model_Zipfile();
                model.ID = dataReader.GetInt32(0);
                model.ZipName = dataReader.GetString(1);
                model.FileSize = dataReader.GetFloat(2);
                model.Unziped = (byte)dataReader.GetInt16(3);
                model.FileCount = dataReader.GetInt16(4);
                model.CreateTime = dataReader.GetDateTime(5);
                if (!dataReader.IsDBNull(6))
                    model.UnzipTime = dataReader.GetDateTime(6);
                if (!dataReader.IsDBNull(7))
                    model.DeleteTime = dataReader.GetString(7);
                if (!dataReader.IsDBNull(8))
                    model.FilePath = dataReader.GetString(8);
                model.State = (byte)dataReader.GetInt16(9);
                List.Add(model);
            }
            return List;
        }
        /// <summary>
        /// 查询所有记录
        /// </summary>
        public List<Model_Zipfile> GetAll()
        {
            string sql = "SELECT * FROM tzipfile";
            MySqlDataReader dataReader = dbHelper.GetDataReader(sql);
            List<Model_Zipfile> List = DataReaderToList(dataReader);
            dataReader.Close();
            return List;
        }
        /// <summary>
        /// 查询记录数
        /// </summary>
        public long GetCount()
        {
            string sql = "SELECT COUNT(*) FROM tzipfile";
            long count;
            return long.TryParse(dbHelper.GetSingle(sql).ToString(), out count) ? count : 0;
        }
        /// <summary>
        /// 根据主键查询一条记录
        /// </summary>
        public Model_Zipfile GetModel(int id)
        {
            string sql = "SELECT * FROM tzipfile WHERE ID=@ID";
            MySqlParameter[] parameters = new MySqlParameter[]{
                new MySqlParameter("@ID", MySqlDbType.Int32, 11){ Value = id }
            };
            MySqlDataReader dataReader = dbHelper.GetDataReader(sql, parameters);
            List<Model_Zipfile> List = DataReaderToList(dataReader);
            dataReader.Close();
            return List.Count > 0 ? List[0] : null;
        }

    }
}
