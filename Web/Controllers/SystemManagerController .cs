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
using System.Configuration;

namespace EADS.Web.Controllers
{
    public class SystemManagerController : Controller
    {
        public ActionResult Database()
        {
            return View();
        }

        public JsonResult Backup()
        {
            string backupPath = ConfigurationManager.AppSettings["dbBackupDir"].ToString();
            if (!backupPath.EndsWith("\\"))
            {
                backupPath += "\\";
            }
            bool result = Common.DatabaseBackupHelper.BackupDb(backupPath + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".sql");
            return Json(new  { result  = result }, JsonRequestBehavior.AllowGet);
        }
        

        public JsonResult getDbBackupList() {
            string backupPath = ConfigurationManager.AppSettings["dbBackupDir"].ToString();
            DirectoryInfo rootDir = new DirectoryInfo(backupPath);
            DataTable dt = new DataTable();
            dt.Columns.Add("FileName");
            dt.Columns.Add("FileSize");
            dt.Columns.Add("BackupTime");
            dt.Columns.Add("FileUrl");
            string backupRes= ConfigurationManager.AppSettings["dbBackupResUrl"].ToString();
            foreach (FileInfo dbFile in rootDir.GetFiles("*.sql")) {
                DataRow row = dt.NewRow();
                row["FileName"] = dbFile.Name;
                row["FileSize"] = dbFile.Length/1024/1024+"MB";
                row["BackupTime"] =  dbFile.CreationTime.ToString("yyyy年MM月dd号 HH点mm分ss秒");
                row["FileUrl"] = backupRes +"/"+ dbFile.Name;
                dt.Rows.Add(row);
            }
            var query = from p in dt.AsEnumerable()
                        select new
                        {
                            FileName = p["FileName"].ToString(),
                            FileSize = p["FileSize"].ToString(),
                            BackupTime = p["BackupTime"].ToString(), 
                            FileUrl = p["FileUrl"].ToString() 
                        };
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        public FileStreamResult DownloadDbBackup(string file)
        {
            string backupPath = ConfigurationManager.AppSettings["dbBackupDir"].ToString();
            if (!backupPath.EndsWith("\\"))
            {
                backupPath += "\\";
            }
            MemoryStream ms = new MemoryStream();
            FileStream fs = new FileStream(backupPath+file, FileMode.Open); 
            return File(fs, "application/ms-excel;charset=UTF-8", file);

        }

    }
}