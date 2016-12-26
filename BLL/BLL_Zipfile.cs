using EADS.Model;
using EADS.DAL;
using System.Collections.Generic;

namespace EADS.BLL
{ 
	public class BLL_Zipfile
    {
        private DAL_Zipfile dal = new DAL_Zipfile(); 
        /// <summary>
        /// 构造函数
        /// </summary>
        public BLL_Zipfile()
        {
           
        }
 

        /// <summary>
        /// 添加记录
        /// </summary>
        /// <param name="model">Model_Zipfile实体类</param>
        /// <returns>新增记录的ID</returns>
        public int Add(Model_Zipfile model)
        {
            dal.OpenConnect();
            int result = dal.Add(model);
            dal.CloseConnect();
            return result;
        }
        /// <summary>
        /// 文件解压完成 更新内部文件数量 及本压缩包的大小 状态等 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UnzipedOver(Model_Zipfile model)
        { 
            dal.OpenConnect();
            bool result = dal.UnzipedOver(model);
            dal.CloseConnect();
            return result;
        }


        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="model">Model_Zipfile实体类</param>
        public bool Update(Model_Zipfile model)
        { 
            dal.OpenConnect();
            bool result = dal.Update(model);
            dal.CloseConnect();
            return result;
        }
        /// <summary>
        /// 删除记录
        /// </summary>
        public int Delete(int id)
        {

            dal.OpenConnect();
            int result = dal.Delete(id);
            dal.CloseConnect();
            return result; 
        }
  
        /// <summary>
        /// 查询所有记录
        /// </summary>
        public List<Model_Zipfile> GetAll()
        {
            dal.OpenConnect();
            List<Model_Zipfile> result = dal.GetAll();
            dal.CloseConnect();
            return result; 
        }
        /// <summary>
        /// 查询记录数
        /// </summary>
        public long GetCount()
        { 
            dal.OpenConnect();
            long result = dal.GetCount();
            dal.CloseConnect();
            return result; 
        }
        /// <summary>
        /// 根据主键查询一条记录
        /// </summary>
        public Model_Zipfile GetModel(int id)
        { 
            dal.OpenConnect();
            Model_Zipfile result = dal.GetModel(id);
            dal.CloseConnect();
            return result; 
        }

    }
}
