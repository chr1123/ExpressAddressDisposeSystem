using EADS.Model;
using EADS.DAL;
using System.Collections.Generic;

namespace EADS.BLL
{
	 
	public class BLL_User_Group
    {
        private DAL_User_Group dal = new DAL_User_Group();
        /// <summary>
        /// 构造函数
        /// </summary>
        public BLL_User_Group()
        {
        }
        /// <summary>
        /// 添加记录
        /// </summary>
        /// <param name="model">Model_User_Group实体类</param>
        /// <returns>新增记录的ID</returns>
        public int Add(Model_User_Group model)
        { 
            return dal.Add(model);
        }
   
        public bool Update(Model_User_Group model)
        {
            return dal.Update(model);
        }
        /// <summary>
        /// 删除记录
        /// </summary>
        public int Delete(int id)
        {
            return dal.Delete(id);
        }
       
        /// <summary>
        /// 查询所有记录
        /// </summary>
        public List<Model_User_Group> GetAll()
        {
            return dal.GetAll();
        }
        /// <summary>
        /// 查询记录数
        /// </summary>
        public long GetCount()
        {
            return dal.GetCount();
        }
        /// <summary>
        /// 根据主键查询一条记录
        /// </summary>
        public Model_User_Group Get(int id)
        {
            return dal.Get(id);
        }
    }
}
