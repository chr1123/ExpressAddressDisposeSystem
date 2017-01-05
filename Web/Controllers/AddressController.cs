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
    public class AddressController : Controller
    {


        public ActionResult List() {
            return View();
        }
         
        public JsonResult GetList(int page, int rows,string address) {
            BLL_BaseAddress bll = new BLL_BaseAddress();
            int total = 0;
            DataTable dt =  bll.GetList(page, rows, address, out total).Tables[0]; 
            var query = from p in dt.AsEnumerable()
                        select new
                        {
                            Address = p["Address"].ToString()
                        };
            return Json(new { rows = query, total = total }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Add(string address) {

            BLL_BaseAddress bll = new BLL_BaseAddress();
            bool result = bll.Add(address); 
            return Json(new { result=result }, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public JsonResult GetMatchAddress(string Address)
        { 
            if(string.IsNullOrWhiteSpace(Address))
                return Json(new {}, JsonRequestBehavior.AllowGet);
            BLL_BaseAddress bll = new BLL_BaseAddress();
            DataTable dt = bll.GetMatchAddress(Address,30).Tables[0];
            var query = from p in dt.AsEnumerable()
                        select new
                        {
                            Address = p["Address"].ToString() 
                        };
            return Json(new { rows = query}, JsonRequestBehavior.AllowGet);
        }
 
    }
}