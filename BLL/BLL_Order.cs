using EADS.Model;
using EADS.DAL;
using System.Data;
using System.Collections.Generic;

namespace EADS.BLL
{ 
	public class BLL_Order
    {
        private DAL.DAL_Order dal = new DAL_Order();

        /// <summary>
        /// 构造函数
        /// </summary>
        public BLL_Order()
        {
        }
        /// <summary>
        /// 添加记录
        /// </summary>
        /// <param name="model">EADS.Model.torder实体类</param>
        /// <returns>新增记录的ID</returns>
        public int Add(Model_Order model)
        {
            return dal.Add(model);
        }
        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="model">EADS.Model.torder实体类</param>
        public bool Update(Model_Order model)
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
  
        public long GetCount()
        {
            return dal.GetCount();
        }
        /// <summary>
        /// 根据主键查询一条记录
        /// </summary>
        public Model_Order GetModel(int id)
        {
            return dal.GetModel(id);
        }


        public DataSet GetListByPage(string where, int page, int pageSize, out int total) {
            dal.OpenConnect();
            DataSet ds = dal.GetListByPage(where, page, pageSize, out total);
            dal.CloseConnect();
            return ds;
        }

        public bool UpdateState(int id, int state)
        {
            dal.OpenConnect();
            bool result = dal.UpdateState(id, state);
            dal.CloseConnect();
            return result;
        }

        public bool UpdateAddressInfo(Model_Order model) {
            dal.OpenConnect();
            bool result = dal.UpdateAddressInfo(model);
            dal.CloseConnect();
            return result;
        }

        public List<Model_Order> GetListByOrderIds(string ids)
        { 
            dal.OpenConnect();
            List<Model_Order> list = dal.GetListByOrderIds(ids);
            dal.CloseConnect();
            return list;
        }

        /// <summary>
        /// 用户领取订单 
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="rows">领取条数</param>
        /// <returns>成功获取条数</returns>
        public int TakeOrder(int userId,int rows)
        {
            dal.OpenConnect();
            int got = dal.TakeOrder(userId, rows);
            dal.CloseConnect();
            return got;
        }


        public Model_Order getUserFirstUnHandlerOrder(int userId, out int count, out int unHandle)
        {
            dal.OpenConnect();
            Model_Order model = dal.getUserFirstUnHandlerOrder(userId, out count, out unHandle);
            dal.CloseConnect();
            return model;
        }

        public List<Model_Order> GetList(string strWhere)
        {
            dal.OpenConnect();
            List<Model_Order> list = dal.GetList(strWhere);
            dal.CloseConnect();
            return list;
        }


        /// <summary>
        /// 获取待回传的订单列表(按订单处理时间排升序)
        /// </summary>
        /// <returns></returns>
        public List<Model_Order> GetWaitForCallbackOrderList(bool isWithCallbacking=false)
        {
            dal.OpenConnect();
            string sqlWhere = "State=" + Model_Order.STATE_HANDLED_WAIT_FOR_CALLBACK;
            if (isWithCallbacking) {
                sqlWhere += " OR State=" + Model_Order.STATE_CALLBACKING;
            }
            List<Model_Order> list = dal.GetList(sqlWhere, "HandleTime ASC ");
            //将取得的待回传订单列表状态更改为回传中
            if (list != null && list.Count > 0) {
                string ids = "";
                foreach (Model_Order order in list) {
                    ids += order.ID +",";
                }
                if (!dal.UpdateStateAsCallbacking(ids.TrimEnd(','))) {
                    return null;
                }
            } 
            dal.CloseConnect();
            return list;
        }

        public bool UpdateStateAsWaitForCallback(string ids) {
            dal.OpenConnect();
            bool result = dal.UpdateStateAsWaitForCallback(ids);
            dal.CloseConnect();
            return result;
        }

        public bool UpdateCallbackResult(string ids, byte resultCode)
        {
            dal.OpenConnect();
            bool result = dal.UpdateCallbackResult(ids, resultCode);
            dal.CloseConnect();
            return result;
        }
    }
}
