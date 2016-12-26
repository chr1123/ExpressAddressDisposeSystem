using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;
using EADS.Common;
using System.Diagnostics;

namespace EADS.DBUtility
{
    /// <summary>
    /// MySql操作类
    /// </summary>
    public class MySqlHelper
    {

        private MySqlConnection pMySqlConnection;
        private MySqlTransaction pMySqlTransaction;
        private string strConnectionString;
        private Dictionary<string, object> pMySqlParameters;
        public enum DBTYPE
        {
            DBpublic,
            DBbusness
        }

        public MySqlHelper()
        {
            pMySqlConnection = new MySqlConnection();
            pMySqlParameters = new Dictionary<string, object>(); 
            try
            {
                strConnectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
                pMySqlConnection.ConnectionString = strConnectionString;
            }
            catch (Exception ex) {

            }
            if (strConnectionString==null) {
                bool exist = false;
                //连接串获取失败 则认为是winform程序 从ini中读取
                IniFile iniFile = new IniFile(CommonConst.GetIniFilePath(), out exist); 
                if (!exist) {
                    //新生成的ini文件  写默认值
                    iniFile.WriteString("DATABASE", "DBNAME", "expressaddress");
                    iniFile.WriteString("DATABASE", "IP", "127.0.0.1");
                    iniFile.WriteString("DATABASE", "PORT", "3306");
                    iniFile.WriteString("DATABASE", "USER", "root");
                    iniFile.WriteString("DATABASE", "PWD", "206366"); 
                }
                string database = iniFile.ReadString("DATABASE", "DBNAME", "expressaddress");
                string ip = iniFile.ReadString("DATABASE", "IP", "127.0.0.1");
                string port = iniFile.ReadString("DATABASE", "PORT", "3306");
                string user = iniFile.ReadString("DATABASE", "USER", "root");
                string password = iniFile.ReadString("DATABASE", "PWD", "206366");
                //server = localhost;port=13306;user id = root; password = 206366; 
                //database = expressaddress; Charset = utf8
                StringBuilder sb = new StringBuilder();
                sb.Append("server=");
                sb.Append(ip);
                sb.Append(";port=");
                sb.Append(port);
                sb.Append(";user id=");
                sb.Append(user);
                sb.Append(";password=");
                sb.Append(password);
                sb.Append(";database=");
                sb.Append(database);
                sb.Append(";Charset = utf8");
                strConnectionString = sb.ToString();
                pMySqlConnection.ConnectionString = strConnectionString;
            } 
        }


        public MySqlHelper(DBTYPE dty)
        {
            pMySqlConnection = new MySqlConnection();
            pMySqlParameters = new Dictionary<string, object>();
            if (dty == DBTYPE.DBpublic)
            {
                strConnectionString = ConfigurationManager.ConnectionStrings["ConnectionStringP"].ConnectionString;//ConfigurationManager.AppSettings["ConnectionString"].ToString();
            }
            else
            {
                strConnectionString = ConfigurationManager.ConnectionStrings["ConnectionStringB"].ConnectionString;
            }
            pMySqlConnection.ConnectionString = strConnectionString;
        }

        

        /// <summary>
        /// 打开连接
        /// </summary>
        /// <returns>0:成功;-1:失败</returns>
        public int OpenConnect()
        {
            try
            {
                pMySqlConnection.Open();
                return 0;
            }
            catch(Exception ex)
            {
                LogHelper.WriteLog(ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        public void CloseConnect()
        {
            pMySqlConnection.Close();
            pMySqlConnection.Dispose();
        }

        /// <summary>
        /// 开始事务
        /// </summary>
        public void BeginTransaction()
        {
            //pOracleTransaction = pOracleConnection.BeginTransaction();
            pMySqlTransaction = this.pMySqlConnection.BeginTransaction();
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        public void Commit()
        {
            pMySqlTransaction.Commit();
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        public void Rollback()
        {
            pMySqlTransaction.Rollback();
        }

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        /// <returns>影响行数</returns>
        public int ExecuteSql(string strSql,MySqlParameter[] parameters =null)
        {
            //OracleCommand pOracleCommand = new OracleCommand();
            MySqlCommand pMySqlCommand = new MySqlCommand();
            pMySqlCommand.Connection = pMySqlConnection;
            pMySqlCommand.CommandText = strSql;
            pMySqlCommand.CommandType = System.Data.CommandType.Text;

            if (parameters != null) {
                foreach (MySqlParameter item in parameters)
                {
                    pMySqlCommand.Parameters.Add(item);
                }
            } 
            try
            {
                int Result = pMySqlCommand.ExecuteNonQuery();
                return Result;
            }
            catch (Exception e)
            {
                return -1;
            }
            //finally
            //{
            //    pMySqlCommand.Parameters.Clear();
            //    pMySqlParameters.Clear();
            //}
        }


        public object ExecuteScalar(string strSql)
        {
            MySqlCommand pMySqlCommand = new MySqlCommand();
            pMySqlCommand.Connection = pMySqlConnection;
            pMySqlCommand.CommandText = strSql;
            pMySqlCommand.CommandType = System.Data.CommandType.Text;

            foreach (KeyValuePair<string, object> item in pMySqlParameters)
            {
                pMySqlCommand.Parameters.AddWithValue(item.Key, item.Value);
            }
            try
            {
                object Result = pMySqlCommand.ExecuteScalar();
                return Result;
            }
            catch (Exception e)
            {
                return -1;
            }
            finally
            {
                pMySqlCommand.Parameters.Clear();
                pMySqlParameters.Clear();
            }
        }

        /// <summary>
        /// 根据SQL语句获取数据
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        /// <returns>DataSet</returns>
        public DataSet GetDataBySql(string strSql)
        {
            MySqlDataAdapter pMySqlDataAdapter = new MySqlDataAdapter();

            MySqlCommand pMySqlCommand = new MySqlCommand(strSql, pMySqlConnection);

            foreach (KeyValuePair<string, object> item in pMySqlParameters)
            {
                pMySqlCommand.Parameters.AddWithValue(item.Key, item.Value);
            }

            pMySqlDataAdapter.SelectCommand = pMySqlCommand;

            DataSet pDataSet = new DataSet();
            try
            {
                pMySqlDataAdapter.Fill(pDataSet);
                return pDataSet;
            }
            catch(Exception ex)
            {
                LogHelper.WriteLog("MYSQLHELPER GetDataBySql 异常:" + ex.Message);
                LogHelper.WriteLog("sql:"+strSql);
                return null;
            }
            finally
            {
                pMySqlCommand.Parameters.Clear();
                pMySqlParameters.Clear();
            }
        }

        /// <summary>
        /// 根据存储过程获取记录集
        /// </summary>
        /// <param name="strProcedureName">存储过程名</param>
        /// <param name="OutParaName">返回记录集的参数名</param>
        /// <returns>记录集</returns>
        public DataSet GetDataByProcedure(string strProcedureName, string OutParaName)
        {
            MySqlCommand pMySqlCommand = new MySqlCommand(strProcedureName, pMySqlConnection);
            foreach (KeyValuePair<string, object> item in pMySqlParameters)
            {
                pMySqlCommand.Parameters.AddWithValue(item.Key, item.Value);
            }
            pMySqlCommand.Parameters.AddWithValue(OutParaName, OutParaName);
            pMySqlCommand.CommandType = CommandType.StoredProcedure;
            MySqlDataAdapter pOracleDataAdapter = new MySqlDataAdapter(pMySqlCommand);
            MySqlDataReader read = pMySqlCommand.ExecuteReader(CommandBehavior.SchemaOnly);
            DataSet pDataSet = new DataSet();
            try
            {
                pOracleDataAdapter.Fill(pDataSet);
            }
            catch
            {
                return null;
            }
            finally
            {
                pMySqlCommand.Parameters.Clear();
                pMySqlParameters.Clear();
            }
            return pDataSet;
        }

        ///// <summary>
        ///// 根据存储过程分页获取记录集
        ///// </summary>
        ///// <param name="strProcedureName">存储过程名</param>
        ///// <param name="OutParaName">返回记录集的参数名</param>
        ///// <returns>记录集</returns>
        //public DataSet GetDataByProcedure(string strProcedureName, string OutParaName,string OutTotalCount,out int outvalue)
        //{
        //    OracleCommand pOracleCommand = new OracleCommand(strProcedureName, pOracleConnection);
        //    foreach (KeyValuePair<string, object> item in pOracleParameters)
        //    {
        //        pOracleCommand.Parameters.Add(item.Key, item.Value);
        //    }

        //    pOracleCommand.Parameters.Add(OutTotalCount, OracleDbType.Int32, ParameterDirection.Output);
        //    pOracleCommand.Parameters.Add(OutParaName, OracleDbType.RefCursor, ParameterDirection.Output);
            
        //    pOracleCommand.CommandType = CommandType.StoredProcedure;
        //    OracleDataAdapter pOracleDataAdapter = new OracleDataAdapter(pOracleCommand);
        //    OracleDataReader read = pOracleCommand.ExecuteReader(CommandBehavior.SchemaOnly);
        //    outvalue = 0;
        //    DataSet pDataSet = new DataSet();
        //    try
        //    {
        //        pOracleDataAdapter.Fill(pDataSet);
        //        outvalue = Convert.ToInt32(pOracleCommand.Parameters[OutTotalCount].Value.ToString());
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //    finally
        //    {
        //        pOracleCommand.Parameters.Clear();
        //        pOracleParameters.Clear();
        //    }
        //    return pDataSet;
        //}

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="strProcedureName">存储过程名</param>
        public int ExecProcedure(string strProcedureName)
        {
            MySqlCommand pMySqlCommand = new MySqlCommand(strProcedureName, pMySqlConnection);
            foreach (KeyValuePair<string, object> item in pMySqlParameters)
            {
                pMySqlCommand.Parameters.AddWithValue(item.Key, item.Value);
            }
            pMySqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                pMySqlCommand.ExecuteNonQuery();
            }
            catch
            {
                return -1;
            }
            finally
            {
                pMySqlCommand.Parameters.Clear();
                pMySqlParameters.Clear();
            }
            return 0;
        }
        /// <summary>
        /// 执行存储过程,得到一个输出值，输出值的类型是整形
        /// </summary>
        /// <param name="strProcedureName">存储过程名</param>
        public string ExecProcedureForSingleOutIntValue(string strProcedureName, string OutParaName)
        {
            MySqlCommand pMySqlCommand = new MySqlCommand(strProcedureName, pMySqlConnection);
            foreach (KeyValuePair<string, object> item in pMySqlParameters)
            {
                pMySqlCommand.Parameters.AddWithValue(item.Key, item.Value);
            }
            MySqlParameter outPara = new MySqlParameter(OutParaName, MySqlDbType.Int32);
            outPara.Direction = ParameterDirection.Output;
            pMySqlCommand.Parameters.Add(outPara);
            pMySqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                pMySqlCommand.ExecuteNonQuery();
                return outPara.Value.ToString();
            }
            catch (Exception ex)
            {
                return "-1";
            }
            finally
            {
                pMySqlCommand.Parameters.Clear();
                pMySqlParameters.Clear();
            }
        }

        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="strParameterName">参数名</param>
        /// <param name="objValue">值</param>
        public void AddParameters(string strParameterName, object objValue)
        {
            pMySqlParameters.Add(strParameterName, objValue);
        }

        public int GetMaxId(string columnName,string tableName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select IFNULL(Max(");
            strSql.Append(columnName);
            strSql.Append("),0)+1 from ");
            strSql.Append(tableName);
            int maxId = int.Parse(ExecuteScalar(strSql.ToString()).ToString());
            return maxId;
        }

        public object GetSingle(string strSql, MySqlParameter[] parameters =  null) {
            MySqlCommand pMySqlCommand = new MySqlCommand();
            pMySqlCommand.Connection = pMySqlConnection;
            pMySqlCommand.CommandText = strSql;
            pMySqlCommand.CommandType = System.Data.CommandType.Text;
            if (parameters != null) {
                foreach (MySqlParameter param in parameters)
                {
                    pMySqlCommand.Parameters.Add(param);
                }
            } 
            try
            {
                object obj = pMySqlCommand.ExecuteScalar();
                return obj;
            }
            catch (Exception e)
            {
                LogHelper.WriteLog("MySqlHelper—GetSingle 错误：" + e.Message);
                return null;
            }
        }
         
        public void ExecuteScalar(string strSql, MySqlParameter[] parameters,out int id) {
             
            MySqlCommand pMySqlCommand = new MySqlCommand();
            pMySqlCommand.Connection = pMySqlConnection;
            pMySqlCommand.CommandText = strSql;
            pMySqlCommand.CommandType = CommandType.Text;
            if (parameters != null)
            {
                foreach (MySqlParameter item in parameters)
                {
                    pMySqlCommand.Parameters.Add(item);
                }
            }
            try
            { 
                id = int.Parse(pMySqlCommand.ExecuteScalar().ToString()); 
            }
            catch (Exception e)
            {
                LogHelper.WriteLog("MySqlHelper—ExecuteScalar 错误："+e.Message);
                id = - 1;
            }  
        }


        public MySqlDataReader GetDataReader(string strSql, MySqlParameter[] parameters =null)
        {
            MySqlCommand pMySqlCommand = new MySqlCommand(strSql, pMySqlConnection);
            if (parameters != null)
            {
                foreach (MySqlParameter item in parameters)
                {
                    pMySqlCommand.Parameters.Add(item);
                }
            }
            pMySqlCommand.CommandType = CommandType.Text;
            MySqlDataAdapter pOracleDataAdapter = new MySqlDataAdapter(pMySqlCommand);
            MySqlDataReader reader = pMySqlCommand.ExecuteReader(CommandBehavior.SchemaOnly);
            return reader;
        }

        public DataSet GetList(string strSql, MySqlParameter[] parameters = null)
        {
            MySqlCommand pMySqlCommand = new MySqlCommand(strSql, pMySqlConnection);
            if (parameters != null)
            {
                foreach (MySqlParameter item in parameters)
                {
                    pMySqlCommand.Parameters.Add(item);
                }
            }
            pMySqlCommand.CommandType = CommandType.Text;
            MySqlDataAdapter dataAdapter = new MySqlDataAdapter(pMySqlCommand);
            DataSet ds = new DataSet();
            dataAdapter.Fill(ds);
            return ds;
        }
    }
}
