using EADS.Model;
using EADS.BLL;
using EADS.Common; 
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Merchandiser.Controllers.SystemManager
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List() {
            return View();
        }

        public ActionResult Add()
        {
            return View();
        }
        public ActionResult ChangePassword()
        {
            return View();
        }
   
        public JsonResult ChangePwd(string oldPwd, string newPwd)
        {
            if(newPwd.Trim()=="") return Json(new { result = false, msg = "修改失败,新密码不合格" });
            BLL_User bll = new BLL_User();
            Model_User user = Session["USER"] == null ? null : (Model_User)Session["USER"];
            if (user == null) {
                return Json(new { result=false,msg="修改失败" });
            }
            bool result = bll.UpdatePassword(user.ID, oldPwd, newPwd);
            return Json(new { result = result, msg = result?"修改成功！":"修改失败,输入的当前密码有误!" });
        }

        [HttpPost]
        public JsonResult UserLogin(string username, string password, string idencode)
        {
            JsonResult jr = new JsonResult();
           string iCode = Session["IdentifyCode"].ToString();
            if (!iCode.Equals(idencode.ToUpper()))
            {
                return Json(new { result = false, msg="登陆失败，验证码错误！" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                BLL_User bUser = new BLL_User();
                Model_User user = bUser.Login(username, password);
                if (user != null)
                {
                    //     BLL.BPermission bll = new BLL.BPermission(); 
                    //     BRole bRole = new BRole();
                    //     Role role = bRole.GetModel(user.uRoleID);
                    //user.RoleName = role.rName;
                    //if (role.rPower != "")
                    //{
                    //    DataSet ds = bll.GetList(" ID in (" + role.rPower + ") ");
                    //    DataTable dt = ds.Tables[0];
                    //    user.Permissions = bll.DataTableToList(dt);
                    //}
                    //else {
                    //    user.Permissions = new List<Model.Permission>();
                    //}
                    Session["USER"] = user;
                    // Session.Add("USER", user);
                    //Session.Add("UserType", usermodel.UserType); 
                    if (user.State == Model_User.STATE_FORBIDDEN)
                    {
                        return Json(new { result = false,msg="登陆失败：用户被禁用，如有疑问请联系管理员！"},
                            JsonRequestBehavior.AllowGet);
                    } 
                    return Json(new { result = true, group =user.GroupID}, JsonRequestBehavior.AllowGet);
                }
                else {
                    return Json(new { result = false, msg = "登陆失败，用户名或密码错误！" }, JsonRequestBehavior.AllowGet);
                }

      
             }
        }

        public JsonResult GetList(int page, int rows, string likeValue, string createTimeStart, string createTimeEnd)
        {
            BLL_User bll = new BLL_User();
            string strWhere = " GroupID=3 ";
            if (!string.IsNullOrWhiteSpace(likeValue)) {
                strWhere += " and (UserName like '%";
                strWhere += likeValue;
                strWhere += "%' or RealName like '%";
                strWhere += likeValue;
                strWhere += "%' or Phone like '%";
                strWhere += likeValue;
                strWhere += "%') ";
            }
            if (!string.IsNullOrEmpty(createTimeStart)) {
                strWhere += " and CreateTime >= '";
                strWhere += createTimeStart;
                strWhere += "' "; 
            }
            if (!string.IsNullOrEmpty(createTimeEnd))
            {
                strWhere += " and CreateTime <= '";
                strWhere += createTimeEnd;
                strWhere += "' ";
            }
            int total = 0;
            DataSet ds = bll.GetTakeOrderUsers(page,rows,strWhere,out total);
            DataTable dt = ds.Tables[0];
            var query = from p in dt.AsEnumerable()
                        select new
                        {
                            ID = p["ID"].ToString(),
                            UserName = p["UserName"].ToString(),
                            RealName = p["RealName"].ToString(),
                            Phone = p["Phone"].ToString(),
                            Sex = p["Sex"].ToString()=="0"?"女":"男",
                            Age = p["Age"].ToString(),
                            Address = p["Address"].ToString(),
                            CreateTime = p["CreateTime"].ToString(),
                            Remark = p["Remark"].ToString(),
                            State = p["State"].ToString()
                        };
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddUser(string strModel)
        {
            Model_User model = SerializeHelper.JsonToObject<Model_User>(strModel);
            BLL_User bll = new BLL_User();
            model.CreateTime = DateTime.Now;
            model.GroupID = 3;
            model.State = Model_User.STATE_NORMAL;
            bool result = bll.Add(model);
            return Json(result, JsonRequestBehavior.AllowGet);
        }



        public JsonResult UpdateState(int userId, int state)
        { 
            BLL_User bll = new BLL_User(); 
            bool result = bll.UpdateState(userId,state);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ResetPassword(int userId, string pwd)
        {
            BLL_User bll = new BLL_User();
            bool result = bll.UpdatePassword(userId, pwd);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult OrderCount() {
            return View();
        }

        public JsonResult GetUserOrderCountList(int page, int rows,int timeType, string timeStart, string timeEnd,string likeValue)
        {
            BLL_Statistics bll = new BLL_Statistics(); 
            StringBuilder strWhere = new StringBuilder();
            strWhere.Append(" DisposeUserId!=0 ");//去掉未被领取的单 
            if (!string.IsNullOrWhiteSpace(likeValue))
            {
                strWhere.Append(" and (UserName like '%");
                strWhere.Append(likeValue);
                strWhere.Append("%' or RealName like '%");
                strWhere.Append(likeValue);
                strWhere.Append("%' or Phone like '%");
                strWhere.Append(likeValue);
                strWhere.Append("%') "); 
            }
            if (!string.IsNullOrEmpty(timeStart))
            {
                strWhere.Append(" and ");
                strWhere.Append(timeType==0? "TakeTime>='" : "HandleTime>='");//
                strWhere.Append(timeStart);
                strWhere.Append("' "); 
            }
            if (!string.IsNullOrEmpty(timeEnd))
            {
                strWhere.Append(" and ");
                strWhere.Append(timeType == 0 ? "TakeTime<='" : "HandleTime<='");//
                strWhere.Append(timeEnd);
                strWhere.Append("' ");
            }
            int total = 0;
            DataSet ds = bll.GetUserOrderCountList(page, rows, strWhere.ToString(), out total);
            DataTable dt = ds.Tables[0];
            var query = from p in dt.AsEnumerable()
                        select new
                        {
                            UserID = p["UserID"].ToString(),
                            RealName = p["RealName"].ToString(),
                            UserName = p["UserName"].ToString(),
                            OrderCount = p["OrderCount"].ToString(),
                            WaitForHandleCount = p["WaitForHandleCount"].ToString(),
                            HandledCount = p["HandledCount"].ToString() 
                        };
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult Permission()
        //{
        //    return View();
        //}

        //public JsonResult GetPermissionList()
        //{
        //    BLL.BPermission bll = new BLL.BPermission();
        //    DataSet ds = bll.GetList("");
        //    DataTable dt = ds.Tables[0];
        //    var query = from p in dt.AsEnumerable()
        //                select new
        //                {
        //                    ID = p["ID"].ToString(),
        //                    Description = p["Description"].ToString(),
        //                    pValue = p["pValue"].ToString(),
        //                    pType = p["pType"].ToString()
        //                };
        //    return Json(query, JsonRequestBehavior.AllowGet);
        //}
        //public JsonResult AddPermission(string modstring)
        //{
        //    Permission model = SerializeHelper.JsonToObject<Permission>(modstring);
        //    JsonResult jr = new JsonResult();
        //    BLL.BPermission bll = new BLL.BPermission();
        //    int result = bll.Add(model) ? 1 : 0;
        //    jr.Data = result;
        //    return jr;
        //}

        //public JsonResult UpdatePermission(string modstring)
        //{
        //    Permission model = SerializeHelper.JsonToObject<Permission>(modstring);
        //    JsonResult jr = new JsonResult();
        //    BLL.BPermission bll = new BLL.BPermission();
        //    int result = bll.Update(model) ? 1 : 0;
        //    jr.Data = result;
        //    return jr;
        //}

        //public JsonResult DeletePermission(int id)
        //{
        //    JsonResult jr = new JsonResult();
        //    BLL.BPermission bll = new BLL.BPermission();
        //    int result = bll.Delete(id) ? 1 : 0;
        //    jr.Data = result;
        //    return jr;
        //}

        //public ActionResult Role()
        //{
        //    return View();
        //}

        //public JsonResult GetRoleList()
        //{
        //    BLL.BRole bll = new BLL.BRole();
        //    DataSet ds = bll.GetList("");
        //    DataTable dt = ds.Tables[0];
        //    dt.Columns.Add("power");
        //    BLL.BPermission bPerm = new BPermission();

        //    foreach (DataRow row in dt.Rows) {
        //        if (!string.IsNullOrEmpty(row["rPower"].ToString()))
        //        { 
        //            DataTable dtPower = bPerm.GetList("ID in ("+ row["rPower"].ToString()+")").Tables[0];
        //            foreach(DataRow powerRow in dtPower.Rows) {
        //                row["power"] += powerRow["Description"].ToString();
        //                row["power"] += "  ";
        //            }
        //        } 
        //    }
        //    var query = from p in dt.AsEnumerable()
        //                select new
        //                {
        //                    ID = p["ID"].ToString(),
        //                    rName = p["rName"].ToString(),
        //                    rPower = p["rPower"].ToString(),
        //                     power = p["power"].ToString()
        //                };
        //    return Json(query, JsonRequestBehavior.AllowGet);
        //}
        //public JsonResult AddRole(string modstring)
        //{
        //    Role model = SerializeHelper.JsonToObject<Role>(modstring);
        //    model.rPower = model.rPower.TrimStart(',');
        //    model.rPower = model.rPower.TrimEnd(',');
        //    JsonResult jr = new JsonResult();
        //    BLL.BRole bll = new BLL.BRole();
        //    int result = bll.Add(model) ? 1 : 0;
        //    jr.Data = result;
        //    return jr;
        //}

        //public JsonResult UpdateRole(string modstring)
        //{
        //    Role model = SerializeHelper.JsonToObject<Role>(modstring);
        //   // model.rPower = model.rPower.TrimStart(',');
        //  //  model.rPower = model.rPower.TrimEnd(',');
        //    if (model.rPower.StartsWith(",")) {
        //        model.rPower = model.rPower.Remove(0,1);
        //    }
        //    if (model.rPower.EndsWith(","))
        //    {
        //        model.rPower = model.rPower.Remove(model.rPower.Length-1, 1);
        //    }
        //    JsonResult jr = new JsonResult();
        //    BLL.BRole bll = new BLL.BRole();
        //    int result = bll.Update(model) ? 1 : 0;
        //    jr.Data = result;
        //    return jr;
        //}
        //public JsonResult DeleteRole(int id)
        //{ 
        //    JsonResult jr = new JsonResult();
        //    BLL.BRole bll = new BLL.BRole();
        //    int result = bll.Delete(id) ? 1 : 0;
        //    jr.Data = result;
        //    return jr;
        //}

        //public ActionResult Users()
        //{
        //    return View();
        //}

        //public JsonResult GetUserList()
        //{
        //    BLL.BUser bll = new BLL.BUser();
        //    DataSet ds = bll.GetList("");
        //    DataTable dt = ds.Tables[0];
        //    var query = from p in dt.AsEnumerable()
        //                select new
        //                {
        //                    ID = p["ID"].ToString(),
        //                    uPhone = p["uPhone"].ToString(),
        //                    uPwd = p["uPwd"].ToString(),
        //                    uNickName = p["uNickName"].ToString(),
        //                    uRoleID = p["uRoleID"].ToString(),
        //                    roleName = p["rName"].ToString(),
        //                    uPhoneNumber = p["uPhoneNumber"].ToString(),
        //                    uRemark = p["uRemark"].ToString()
        //                };
        //    return Json(query, JsonRequestBehavior.AllowGet);
        //}
        //public JsonResult AddUser(string modstring)
        //{
        //    User model = SerializeHelper.JsonToObject<User>(modstring);
        //    JsonResult jr = new JsonResult();
        //    BLL.BUser bll = new BLL.BUser();
        //    if (bll.GetList("uPhone='" + model.uPhone + "'").Tables[0].Rows.Count > 0) {
        //        return Json(new{result=false,msg="添加用户失败：用户名已存在"});
        //    }
        //    bool result = bll.Add(model);
        //    return Json(new { result = result, msg = result?"":"添加用户失败" });
        //}

        //public JsonResult UpdateUser(string modstring)
        //{
        //    User model = SerializeHelper.JsonToObject<User>(modstring);
        //    JsonResult jr = new JsonResult();
        //    BLL.BUser bll = new BLL.BUser();
        //    bool result = bll.Update(model);
        //    return Json(new { result = result, msg = result ? "" : "修改失败" });
        //}

        //public JsonResult DeleteUser(int id)
        //{
        //    if (((User)Session["USER"]).ID == id) {
        //        return Json(new {result=false,msg="不能删除当前登录账号"}, JsonRequestBehavior.AllowGet);
        //    } 
        //    BLL.BUser bll = new BLL.BUser();
        //    bool result = bll.Delete(id);
        //    return Json(new { result = result,
        //        msg = result?"删除成功":"删除失败" }, JsonRequestBehavior.AllowGet);
        //}

        //public JsonResult GetMyPermissions()
        //{ 
        //    BLL.BPermission bll = new BLL.BPermission();
        //    User user = (User)Session["USER"]; 
        //    var query = from p in user.Permissions 
        //                select new
        //                {
        //                    pValue = p.pValue,
        //                    pType = p.pType
        //                };
        //    return Json(new {Data= query,isAdmin=user.RoleName=="超级管理员" }, JsonRequestBehavior.AllowGet); 
        //}


        //public ActionResult FileEdit()
        //{
        //    string path = Server.MapPath("~/Views");
        //    //F:\Code\DotNet\MyProgrames\Merchandiser\Merchandiser\Views
        //    string[] dirs = Directory.GetDirectories(path);
        //    List<TreeNode> list = new List<TreeNode>();
        //    foreach (string dir in dirs)
        //    {
        //        TreeNode dirNode = new TreeNode();
        //        dirNode.attributes = new TreeNodeAttribute();
        //        dirNode.attributes.path = dir;
        //        dirNode.text = dir.Substring(dir.LastIndexOf("\\") + 1);
        //        dirNode.state = "closed";
        //        string[] files = Directory.GetFiles(dir);
        //        dirNode.children = new List<TreeNode>();
        //        foreach (string file in files)
        //        {
        //            TreeNode fileNode = new TreeNode();
        //            fileNode.text = file.Substring(file.LastIndexOf("\\") + 1);
        //            fileNode.attributes = new TreeNodeAttribute();
        //            fileNode.attributes.path = file;
        //            dirNode.children.Add(fileNode);
        //        }
        //        list.Add(dirNode);
        //    }
        //    string result = SerializeHelper.ObjectToJson<List<TreeNode>>(list);
        //    ViewBag.TreeData = result;
        //    return View();
        //}

        //public JsonResult GetFileContent(string path)
        //{
        //    string data = System.IO.File.ReadAllText(path,Encoding.GetEncoding("GB2312"));

        //    return Json(data, JsonRequestBehavior.AllowGet);
        //}


        public JsonResult GetCurrentUser()
        {
            Model_User user = (Model_User)Session["USER"];
            return Json(user, JsonRequestBehavior.AllowGet);
        }

    }
}