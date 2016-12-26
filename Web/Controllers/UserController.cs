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

        [HttpPost]
        public JsonResult UserLogin(string username, string password, string idencode)
        {
            JsonResult jr = new JsonResult();
           // string iCode = Session["IdentifyCode"].ToString();
            //if (!iCode.Equals(idencode.ToUpper()))
            //{
            //    jr.Data = -1;
            //    return jr;
            //}
            //else
            //{
                BLL_User bUser = new BLL_User();
                Model_User user = bUser.Login(username, password); 
                jr.Data = user == null ? 0 : 1;
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
                }
                return jr;
           // }
        }

        //public JsonResult GetList()
        //{
        //BLL.BUser bll = new BLL.BUser();
        //DataSet ds = bll.GetAllList();
        //DataTable dt = ds.Tables[0];
        //var query = from p in dt.AsEnumerable()
        //            select new
        //            {
        //                ID = p["ID"].ToString(),
        //                Phone = p["uPhone"].ToString(),
        //                NickName = p["uNickName"].ToString(),
        //                Email = p["uEmail"].ToString(),
        //                RoleID = p["uRoleID"].ToString(),
        //                DepartmentID = p["uDepartmentId"].ToString(),
        //                MasterUID = p["uMasterUserID"].ToString(),
        //                LastLoginTime = p["uLastLoginTime"].ToString()
        //            };
        //return Json(query, JsonRequestBehavior.AllowGet);
        //}


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