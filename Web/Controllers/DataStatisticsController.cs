using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Security.Cryptography;
using EADS.Model;
using System.Data;
using EADS.BLL;
using System.Text;
using EADS.Common;
using EADS.Web.Handler;
using System.Net.Http;
using System.Net;
using System.Net.Security;
using System.IO.Compression;

namespace EADS.Web.Controllers
{
    public class DataStatistics : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
           

        public JsonResult TakeOrder(int rows)
        {
            if(rows<=0)
                return Json(new { result = 0, msg = "接单失败,解答数量需大于0！" }, JsonRequestBehavior.AllowGet);
            Model_User user = (Model_User)Session["USER"];
            BLL_Order bll = new BLL_Order();
            int got = bll.TakeOrder(user.ID, rows);
            if(got==-1)
                return Json(new { result = 0, msg = "接单失败,当前在线接单人数较多，请重试！" }, JsonRequestBehavior.AllowGet);
            if (got == 0)
                return Json(new { result = 0, msg = "接单失败，当前系统已无未接订单！" }, JsonRequestBehavior.AllowGet); 
            return Json(new { result = got, msg =  "接单成功！" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Dispose()
        {
            //返回一条未处理的订单
            BLL_Order bll = new BLL_Order();
            Model_User user = (Model_User)Session["USER"];
            int count = 0;
            int unHandle = 0;
            Model_Order order = bll.getUserFirstUnHandlerOrder(user.ID,out count,out unHandle);
            if(order!=null) order.ImagePath = CommonConst.GetPhotoResUrlForWeb(order.ImagePath);
            ViewBag.ID = order == null ? 0 : order.ID;
            ViewBag.OrderCount = count;
            ViewBag.Unhandle = unHandle;
            return View(order);
        }

       
         
    }
}