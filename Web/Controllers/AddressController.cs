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
       
        [HttpPost]
        public JsonResult GetMatchAddress(string Address)
        { 
            BLL_BaseAddress bll = new BLL_BaseAddress();
            DataTable dt = bll.GetMatchAddress(Address).Tables[0];
            var query = from p in dt.AsEnumerable()
                        select new
                        {
                            Address = p["Address"].ToString() 
                        };
            return Json(new { rows = query}, JsonRequestBehavior.AllowGet);
        }
 
    }
}