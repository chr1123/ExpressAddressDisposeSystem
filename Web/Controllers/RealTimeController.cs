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
    public class RealTimeController : Controller  //实时数据
    {


        public ActionResult Index() {
            return View();
        }

        [HttpPost]
        public JsonResult GetOrderCountInfo() {
             
            BLL_Statistics bll = new BLL_Statistics();
            DataRow row1 = null;
            DataRow row2 = null;
            DataSet ds1 = bll.GetOrderCountInfo("");
            if (ds1 != null && ds1.Tables[0].Rows.Count > 0) {
                 row1 = ds1.Tables[0].Rows[0];
            }
            DataSet ds2 = bll.GetOrderCountInfo(" CreateTime between '"+DateTime.Now.ToString("yyyy-MM-dd 00:00:00")
                +"' and '"+ DateTime.Now.ToString("yyyy-MM-dd 23:59:59")+"' ");
            if (ds2 != null && ds2.Tables[0].Rows.Count > 0)
            { 
                row2 = ds2.Tables[0].Rows[0];
            }
           
            string errormsg = "";
            try
            {
                JsonResult jsonR = Json(new
                {
                    Total = row1 == null || string.IsNullOrEmpty(row1["Total"].ToString()) 
                        ? 0 : int.Parse(row1["Total"].ToString()),
                    NewOrderCount = row1 == null || string.IsNullOrEmpty(row1["NewOrderCount"].ToString()) 
                        ? 0 : int.Parse(row1["NewOrderCount"].ToString()),
                    WaitForHandleCount = row1 == null|| string.IsNullOrEmpty(row1["WaitForHandleCount"].ToString()) 
                        ? 0 : int.Parse(row1["WaitForHandleCount"].ToString()),
                    WaitForCallbackCount = row1 == null || string.IsNullOrEmpty(row1["WaitForCallbackCount"].ToString()) 
                        ? 0 : int.Parse(row1["WaitForCallbackCount"].ToString()),
                    CallbackingCount = row1 == null || string.IsNullOrEmpty(row1["CallbackingCount"].ToString()) 
                        ? 0 : int.Parse(row1["CallbackingCount"].ToString()),
                    FinishedCount = row1 == null || string.IsNullOrEmpty(row1["FinishedCount"].ToString()) 
                        ? 0 : int.Parse(row1["FinishedCount"].ToString()),

                    TotalToday = row2 == null || string.IsNullOrEmpty(row2["Total"].ToString()) 
                        ? 0 : int.Parse(row2["Total"].ToString()),
                    NewOrderCountToday = row2 == null || string.IsNullOrEmpty(row2["NewOrderCount"].ToString()) 
                        ? 0 : int.Parse(row2["NewOrderCount"].ToString()),
                    WaitForHandleCountToday = row2 == null || string.IsNullOrEmpty(row2["WaitForHandleCount"].ToString()) 
                        ? 0 : int.Parse(row2["WaitForHandleCount"].ToString()),
                    WaitForCallbackCountToday = row2 == null || string.IsNullOrEmpty(row2["WaitForCallbackCount"].ToString()) 
                        ? 0 : int.Parse(row2["WaitForCallbackCount"].ToString()),
                    CallbackingCountToday = row2 == null || string.IsNullOrEmpty(row2["CallbackingCount"].ToString()) 
                        ? 0 : int.Parse(row2["CallbackingCount"].ToString()),
                    FinishedCountToday = row2 == null || string.IsNullOrEmpty(row2["FinishedCount"].ToString()) 
                        ? 0 : int.Parse(row2["FinishedCount"].ToString()),

                }, JsonRequestBehavior.AllowGet);
                return jsonR;
            } 
            catch (Exception ex) {
                errormsg = ex.Message;
              
            }
            return Json(new
            {
                msg = errormsg
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Add(string address) {

            BLL_BaseAddress bll = new BLL_BaseAddress();
            bool result = bll.Add(address); 
            return Json(new { result=result }, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public JsonResult GetRealTimeData()
        {
            BLL_Statistics bll = new BLL_Statistics();
            DataTable dt = bll.GetOrderCountOfTodayEachHour().Tables[0];
            var query = from p in dt.AsEnumerable()
                        select new
                        {
                            Hour = p["Hour"].ToString() ,
                            OrderCount = p["OrderCount"].ToString(),
                            WaitForTakeCount = p["WaitForTakeCount"].ToString(),
                            WaitForHandleCount = p["WaitForHandleCount"].ToString(),
                            HandledCount = p["HandledCount"].ToString(),
                            FinishedCount = p["FinishedCount"].ToString()
                        };
            return Json(new { rows = query}, JsonRequestBehavior.AllowGet);
        }
 


    }
}