using EADS.Model;
using EADS.DAL;
using System.Collections.Generic;
using System;

namespace EADS.BLL
{ 
	public class BLL_Photo
	{
        private DAL_Photo dal = new DAL_Photo();
        public BLL_Photo()
        {
           
        }

        public int AddList(List<Model_Photo> list,out int orderCount)
        {
            int succeed = 0; //图片入库成功条数
            orderCount = 0;  //订单入库条数
            dal.OpenConnect();
            DAL_Order dalOrder = new DAL_Order();
            dalOrder.OpenConnect();
            foreach (Model_Photo photo in list)
            {
                //int photoId = dal.Add(photo);暂时不再记录图片
                int photoId = 1;
                if (photoId > 0)
                {
                    succeed++;
                    //加入订单
                    Model_Order modelOrder = new Model_Order();
                    modelOrder.CreateTime = DateTime.Now;
                    modelOrder.ImagePath = photo.WebUrl;
                    modelOrder.OrderNO = photo.FileName.Remove(photo.FileName.LastIndexOf("."));
                    if (modelOrder.OrderNO.Contains("_"))
                    {
                        modelOrder.OrderNO = modelOrder.OrderNO.Remove(modelOrder.OrderNO.LastIndexOf("_"));
                    } 
                    modelOrder.ResultCode = Model_Order.RESULT_CODE_NONE; 
                    int orderId = dalOrder.Add(modelOrder);
                    if (orderId > 0)
                    {
                        orderCount++; 
                    }
                    else {
                        //订单生成插入失败  
                    }
                }
                else
                {
                    //图片记录插入失败  
                }
            }
            dal.CloseConnect();
            dalOrder.CloseConnect();
            return succeed;
        }
        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="model">EADS.Model.tphoto实体类</param>
        public bool Update(Model_Photo model)
        { 
            return dal.Update(model);
        }
       



        public void OpenConnect()
        {
            dal.OpenConnect();
        }
        public void CloseConnect()
        {
            dal.CloseConnect();
        }


    }
}
