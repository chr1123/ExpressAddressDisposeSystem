using EADS.Model;
using EADS.DAL;
using System.Collections.Generic;

namespace EADS.BLL
{
 
	public class BLL_User
    {
        private DAL_User dal = new DAL_User();
        /// <summary>
        /// 构造函数
        /// </summary>
        public BLL_User()
        {
        }

        public Model_User Login(string username, string password) {
            dal.OpenConnect();
            Model_User model = dal.GetModelByUserNameAndPassword("UserName='" + username + "' and Password='" + password + "'");
            dal.CloseConnect();
            return model;
        }

        /// <summary>
        /// 添加记录
        /// </summary>
        /// <param name="model">DAL_User实体类</param>
        /// <returns>新增记录的ID</returns>
        public int Add(Model_User model)
        {
            return dal.Add(model);
        }
        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="model">Model实体类</param>
        public bool Update(Model_User model)
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
        /// 查询记录数
        /// </summary>
        public long GetCount()
        {
            return dal.GetCount();
        }
        /// <summary>
        /// 根据主键查询一条记录
        /// </summary>
        public Model_User Get(int id)
        {
            return dal.Get(id);
        }

    }
}
