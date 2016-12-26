using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EADS.Model;
using EADS.BLL;

namespace EADS.Web.Controllers
{
    public class SessionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            object user = filterContext.HttpContext.Session["USER"];
            string controllerName = filterContext.RouteData.Values["controller"].ToString();
            string actionName = filterContext.RouteData.Values["action"].ToString();
            bool loginRequired = true;//是否需要登录
            switch (controllerName.ToLower()) {
                case "home":
                    loginRequired = false;
                    break;
                case "user":
                    switch (actionName) {
                        case "UserLogin":
                            loginRequired = false;
                            break;
                    }
                    break;
            }
            if (user == null && loginRequired)
            {
                filterContext.Result = new RedirectResult("~");
            }
            if (user != null && loginRequired)
            {
                //string viewPath = controllerName + "/" + actionName;
                //if (((Model_us)user).RoleName != "超级管理员")
                //{
                //    //获取权限表中的视图类型的权限  
                //    BLL.BPermission bPerms = new BPermission();
                //    DataSet ds = bPerms.GetList("pType='View' and pValue='" + viewPath + "'");
                //    if (ds.Tables[0].Rows.Count > 0)
                //    {
                //        //本次操作页面是需要权限的页面  查看当前用户是否有此限制
                //        bool hasPerm = ((User)user).Permissions.Exists(
                //            p => p.pType == "View" && p.pValue == viewPath);
                //        if (hasPerm)
                //        {
                //            filterContext.Result = new RedirectResult("~/Home/NoAuthoryty");
                //        }
                //    }
                // }
                //else {
                //    //管理员
                //    filterContext.Controller.ViewBag.IsAdmin = true;
                //} 
            }
            if (user != null) {
                filterContext.Controller.ViewBag.User = user;
            }
        }
    }
}