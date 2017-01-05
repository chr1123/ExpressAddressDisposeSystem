using EADS.Model;
using EADS.DAL;
using System.Collections.Generic;
using System.Data;

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

        public DataSet GetTakeOrderUsers(int page, int rows, string strWhere, out int total)
        { 
            dal.OpenConnect();
            DataSet result = dal.GetListByPage(page,rows,strWhere,out total);
            dal.CloseConnect();
            return result;
        }

        public bool Add(Model_User model)
        {
            dal.OpenConnect();
            bool result = dal.Add(model)>0;
            dal.CloseConnect();
            return result;
        }


        /// <summary>
        /// 修改指定用户状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public bool UpdateState(int id, int state)
        {
            dal.OpenConnect();
            bool result = dal.UpdateState(id,state);
            dal.CloseConnect();
            return result;
        }

        public bool UpdatePassword(int id, string pwd)
        {
            dal.OpenConnect();
            bool result = dal.UpdatePassword(id, pwd);
            dal.CloseConnect();
            return result;
        }

        public bool UpdatePassword(int id, string oldPwd, string newPwd)
        {
            dal.OpenConnect();
            bool result = dal.UpdatePassword(id, oldPwd,newPwd);
            dal.CloseConnect();
            return result;
        }
    }
}
