﻿using System;
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
    public class OrderController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        // "state": '-1',//-1表示不限
        //"takeTimeStart": '',//
        //"takeTimeEnd": '',
        //"handTimeStart": '',
        //"handTimeEnd": '',
        //"orderNO": '',

        [HttpPost]
        public JsonResult GetList(int page, int rows, string resultCode,string takeTimeStart, string takeTimeEnd,
                string handTimeStart, string handTimeEnd, string orderNO)
        {
            int total = 0;
            StringBuilder strWhere = new StringBuilder();
            Model_User user = (Model_User)Session["USER"];
            strWhere.Append("DisposeUserId=");
            strWhere.Append(user.ID); 
            if (!string.IsNullOrEmpty(resultCode))
            {
                strWhere.Append(" and ResultCode=");
                strWhere.Append(resultCode);
            }
            if (!string.IsNullOrEmpty(takeTimeStart))
            {
                strWhere.Append(" and TakeTime>='");
                strWhere.Append(takeTimeStart);
                strWhere.Append("' ");
            }
            if (!string.IsNullOrEmpty(takeTimeEnd))
            {
                strWhere.Append(" and TakeTime <= '");
                strWhere.Append(takeTimeEnd);
                strWhere.Append("' ");
            }
            if (!string.IsNullOrEmpty(handTimeStart))
            {
                strWhere.Append(" and HandleTime>='");
                strWhere.Append(handTimeStart);
                strWhere.Append("' ");
            }
            if (!string.IsNullOrEmpty(handTimeEnd))
            {
                strWhere.Append(" and HandleTime <= '");
                strWhere.Append(handTimeEnd);
                strWhere.Append("' ");
            }
            if (!string.IsNullOrEmpty(orderNO))
            {
                strWhere.Append(" and OrderNO like '%");
                strWhere.Append(orderNO);
                strWhere.Append("%' ");
            }
            BLL_Order bll = new BLL_Order();
            DataTable dt = bll.GetListByPage(strWhere.ToString(), page - 1, rows, out total).Tables[0];
            var query = from p in dt.AsEnumerable()
                        select new
                        {
                            ID = p["ID"].ToString(),
                            OrderNO = p["OrderNO"].ToString(),
                            DestinationCity = p["DestinationCity"].ToString(),
                            //SenderName = p["SenderName"].ToString(),
                            //SenderPhone = p["SenderPhone"].ToString(),
                            //SenderAddress = p["SenderAddress"].ToString(),
                            //RecipientName = p["RecipientName"].ToString(),
                            //RecipientPhone = p["RecipientPhone"].ToString(),
                            RecipientAddress = p["RecipientAddress"].ToString(),
                            //Goods = p["Goods"].ToString(),
                            State = p["State"].ToString(),
                            ResultCode = p["ResultCode"].ToString(),
                            DisposeUserId = p["DisposeUserId"].ToString(),
                            CreateTime = p["CreateTime"].ToString(),
                            TakeTime = p["TakeTime"].ToString(),
                            HandleTime = p["HandleTime"].ToString(),
                            FinishTime = p["FinishTime"].ToString(),
                            Remark = p["Remark"].ToString(),
                            ImagePath = CommonConst.GetPhotoResUrlForWeb(p["ImagePath"].ToString()),
                        };
            return Json(new { rows = query, total = total }, JsonRequestBehavior.AllowGet);
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

       


        public JsonResult SubmitOrder(string strModel)
        {
            Model_Order model = SerializeHelper.JsonToObject<Model_Order>(strModel);
            //此处应该先判断订单状态
            BLL_Order bll = new BLL_Order(); 
            model.State = Model_Order.STATE_HANDLED_WAIT_FOR_CALLBACK;
            model.HandleTime = DateTime.Now;
            bool subSucceed = bll.UpdateAddressInfo(model);
            if (!subSucceed)
            {
                return Json(new { result = false, msg =  "保存失败!" }, JsonRequestBehavior.AllowGet);
            }
            Model_User user = (Model_User)Session["USER"];
            int count = 0;
            int unHandle = 0;
            Model_Order order = bll.getUserFirstUnHandlerOrder(user.ID, out count, out unHandle);
           // string orderJson = order==null?null: SerializeHelper.ObjectToJson<Model_Order>(order);
            return Json(new { result = true, msg ="保存成功",
                orderCount=count,waitForHandle=unHandle,orderID=order==null?0:order.ID,
                imgSrc=order==null?null:CommonConst.GetPhotoResUrlForWeb(order.ImagePath) }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Callback(string ids)
        {
            ids = ids.TrimEnd(',');
            //此处应该先判断订单状态
            BLL_Order bll = new BLL_Order();
            List<Model_Order> list = bll.GetListByOrderIds(ids);
            XmlHandler xmlHandler = new XmlHandler();
            string xml = xmlHandler.getXmlStrngByList(list); 
            string result = DoPost("url", xml);
            return Json(new { result = result, msg = "保存成功"}, JsonRequestBehavior.AllowGet);

        }

        private const string DefaultUserAgent = "Mozilla/5.0 (Windows NT 5.1) AppleWebKit/537.1 (KHTML, like Gecko) Chrome/21.0.1180.89 Safari/537.1";
        private CookieContainer CC = new CookieContainer();
        private String DoPost(String url, String content)
        {
            string html = "";
            StreamReader reader = null;
            HttpWebRequest webReqst = null;
            //如果是发送HTTPS请求    
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                //ServicePointManager.ServerCertificateValidationCallback = 
                //    new RemoteCertificateValidationCallback(CheckValidationResult);
                //webReqst = WebRequest.Create(url) as HttpWebRequest;
                //webReqst.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                webReqst = WebRequest.Create(url) as HttpWebRequest;
            }
            webReqst.Method = "POST";
            //webReqst.UserAgent = DefaultUserAgent;
            webReqst.ContentType = "text/xml";
            webReqst.ContentLength = content.Length;
            webReqst.CookieContainer = CC;
            webReqst.Timeout = 30000;
            webReqst.ReadWriteTimeout = 30000;
            try
            {
                byte[] data = Encoding.Default.GetBytes(content);
                Stream stream = webReqst.GetRequestStream();
                stream.Write(data, 0, data.Length);


                HttpWebResponse webResponse = (HttpWebResponse)webReqst.GetResponse();
                //  BugFix_CookieDomain(CC);
                if (webResponse.StatusCode == HttpStatusCode.OK && webResponse.ContentLength < 1024 * 1024)
                {
                    stream = webResponse.GetResponseStream();
                    stream.ReadTimeout = 30000;
                    if (webResponse.ContentEncoding == "gzip")
                    {
                        reader = new StreamReader(new GZipStream(stream, CompressionMode.Decompress), Encoding.Default);
                    }
                    else
                    {
                        reader = new StreamReader(stream, Encoding.Default);
                    }
                    html = reader.ReadToEnd();
                }
            }
            catch
            {

            }

            return html;
        }
    }
}