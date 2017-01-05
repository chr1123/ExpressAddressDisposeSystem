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

        public DataSet GetListOfLv1(int page, int pageSize)
        {
            StringBuilder sql = new StringBuilder(); 
            sql.Append(" SELECT * FROM expressaddress.tb_prov_city_area_street where level=1  "); 
            sql.Append(" limit ");
            sql.Append(page * pageSize);
            sql.Append(",");
            sql.Append(pageSize);
            return dbHelper.GetDataBySql(sql.ToString());
        }
        public DataSet GetListOfLv2(int page, int pageSize)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("Select ta.id as id2,ta.code as code2,ta.parentId as code2,ta.name as name2,");
            sql.Append("tb.code as id1,tb.name as name1 from (");
            sql.Append("SELECT * FROM expressaddress.tb_prov_city_area_street where level=2 ) ta  ");
            sql.Append("left join tb_prov_city_area_street tb on ta.parentId = tb.code ");
            sql.Append(" limit ");
            sql.Append(page * pageSize);
            sql.Append(",");
            sql.Append(pageSize);
            return dbHelper.GetDataBySql(sql.ToString());
        }
        public DataSet GetListOfLv3(int page, int pageSize)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("Select ta.id as id3,ta.code as code3,ta.parentId as code3,ta.name as name3,");
            sql.Append("tb.code as id32,tb.name as name2,tc.code as id1,tc.name as name1 ");
            sql.Append("from (");
            sql.Append("SELECT * FROM expressaddress.tb_prov_city_area_street where level=3 ) ta  ");
            sql.Append("left join tb_prov_city_area_street tb on ta.parentId = tb.code ");
            sql.Append(" left join tb_prov_city_area_street tc on tb.parentId = tc.code "); 
            sql.Append(" limit ");
            sql.Append(page * pageSize);
            sql.Append(",");
            sql.Append(pageSize);
            return dbHelper.GetDataBySql(sql.ToString());
        }

        public DataSet GetListOfLv4(int page, int pageSize) {
            StringBuilder sql = new StringBuilder();
            sql.Append("Select ta.id as id4,ta.code as code4,ta.parentId as code3,ta.name as name4,");
            sql.Append("tb.code as id3,tb.name as name3,tc.code as id2,tc.name as name2,");
            sql.Append("td.code as id1,td.name as name1 from (");
            sql.Append("SELECT * FROM expressaddress.tb_prov_city_area_street where level=4 ) ta  ");
            sql.Append("left join tb_prov_city_area_street tb on ta.parentId = tb.code ");
            sql.Append(" left join tb_prov_city_area_street tc on tb.parentId = tc.code ");
            sql.Append(" left join tb_prov_city_area_street td on tc.parentId = td.code ");
            sql.Append(" limit "); 
            sql.Append(page*pageSize);
            sql.Append(",");
            sql.Append(pageSize); 
            return dbHelper.GetDataBySql(sql.ToString());
        }


        public int Insert(string sql) {
            return dbHelper.ExecuteSql(sql);
        }
         
        public DataSet GetMatchAddress(string address,int maxRows)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select Address from tbaseaddress where  Address like '%");
            sql.Append(address);
            sql.Append("%' order by ID asc limit 0,");
            sql.Append(maxRows); 
            return dbHelper.GetDataBySql(sql.ToString()); 
        }


        public DataSet GetList(int page, int rows,string address,out int total)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select Address from tbaseaddress ");
            if (!string.IsNullOrEmpty(address)) {
                sql.Append(" where  Address like '%");
                sql.Append(address);
                sql.Append("%' ");
            } 
            sql.Append(" order by ID asc limit ");
            sql.Append((page -1)*rows);
            sql.Append(",");
            sql.Append(rows);
            string countSql = "select count(ID) from tbaseaddress";
            if (!string.IsNullOrEmpty(address)) {
                countSql += " where  Address like '%";
                countSql += address;
                countSql += "%' ";
            }
            total = int.Parse(dbHelper.GetSingle(countSql).ToString());
            return dbHelper.GetDataBySql(sql.ToString());
        }


        public bool Add(string address) {
            string sql = "INSERT INTO tbaseaddress (Address) VALUES ('"+address+"');";
            int result = dbHelper.ExecuteSql(sql);
            return result > 0;
        }

    }
}
