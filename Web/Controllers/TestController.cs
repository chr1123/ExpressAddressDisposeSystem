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

namespace EADS.Web.Controllers
{
    public class TestController : Controller
    {
        public ActionResult Index()
        { 
            return View();
        }

        public ActionResult BigImage(string imgName)
        {
            ViewBag.ImgName = imgName;
            return View();
        }
        public ActionResult NoAuthoryty()
        { 
            return View();
        }

        

        public ActionResult Login()
        {
            Session["USER"] = null;
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        //获取省列表
        public JsonResult GetProList() {
            BLL.BProvince bll = new BLL.BProvince();
            DataTable dt = bll.GetAllList().Tables[0];
            var query = from p in dt.AsEnumerable()
                        select new
                        {
                            ID = p["ID"].ToString(),
                            NAME = p["pName"].ToString()
                        };
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        
        //获取城市列表
        public JsonResult GetCityListByProId(int proId)
        {
            BLL.BCity bll = new BLL.BCity();
            DataTable dt = bll.GetList(" cProID="+proId).Tables[0];
            var query = from p in dt.AsEnumerable()
                        select new
                        {
                            ID = p["ID"].ToString(),
                            NAME = p["cName"].ToString()
                        };
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        //获取区列表
        public JsonResult GetAreaListByCityId(int cityId)
        {
            BLL.BArea bll = new BLL.BArea();
            DataTable dt = bll.GetList(" aCityID=" + cityId).Tables[0];
            var query = from p in dt.AsEnumerable()
                        select new
                        {
                            ID = p["ID"].ToString(),
                            NAME = p["aName"].ToString()
                        };
            return Json(query, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public ActionResult UploadFile(System.Web.HttpPostedFileBase Filedata, string filePath)
        {
            JsonResult jr = new JsonResult();
            jr.ContentType = "application/json";
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            // 如果没有上传文件
            if (Filedata == null || string.IsNullOrEmpty(Filedata.FileName) || Filedata.ContentLength == 0)
            {
                return this.HttpNotFound();
            }
            try
            {

                string rootPath = Server.MapPath("~/Upload")+"\\"+filePath;
                   
                // 保存到服务器
                string sourceFileName = System.IO.Path.GetFileName(Filedata.FileName);
                string strExtention = sourceFileName.Substring(sourceFileName.LastIndexOf('.')).ToLower();
                Random random = new Random();
                string newFileName = DateTime.Now.ToString("yyyyMMddHHmmss") 
                    + random.Next(0, 9999).ToString("0000")
                     + strExtention;
                //string directorypath = ConfigurationManager.AppSettings[filePath];
                string path = rootPath + "\\" + newFileName ;
                bool hasdirectory = System.IO.Directory.Exists(rootPath);
                if (!hasdirectory)
                {
                    System.IO.Directory.CreateDirectory(rootPath);
                }
                Filedata.SaveAs(path);
                Common.ImageHelper imgHelper = new Common.ImageHelper();
                imgHelper.SaveSmallImage(path, 120);
                jr.Data = newFileName;
                return this.Json(jr);
            }
            catch (Exception ex)
            {
                return this.HttpNotFound();
            }
        }


        //生成验证码
        public FileContentResult Identifying()
        {
            //string path = Server.MapPath("~/App_Data/Area.xls");//HttpContext.Current.Server.MapPath();
            //FileStream fff = new FileStream(path, FileMode.Open);
            //string sss = "";
            //DataTable  dt = Common.ExcelHelper.ConvertExcelToDataTable(fff, ref sss); 
            //BLL.BArea bll = new BLL.BArea();
            //foreach (DataRow row in dt.Rows) {
            //    Area model = new Area();
            //    model.ID = int.Parse(row["ID"].ToString());
            //    model.aName = row["NAME"].ToString();  
            //    model.aCityID = int.Parse(row["CITYID"].ToString());
            //   // bool result =  bll.Add(model);
            //}
            

            string code = CreateVerifyCode(4);
            Session["IdentifyCode"] = code.ToUpper();
            int fSize = FontSize;
            int fWidth = fSize + Padding;
            int imageWidth = (int)(code.Length * fWidth) + 4 + Padding * 2;
            int imageHeight = fSize * 2 + Padding;
            

            System.Drawing.Bitmap image = new System.Drawing.Bitmap(imageWidth, imageHeight);
            Graphics g = Graphics.FromImage(image);
            g.Clear(BackgroundColor);
            Random rand = new Random();

            //给背景添加随机生成的燥点
            if (this.Chaos)
            {
                Pen pen = new Pen(ChaosColor, 0);
                int c = Length * 10;

                for (int i = 0; i < c; i++)
                {
                    int x = rand.Next(image.Width);
                    int y = rand.Next(image.Height);

                    g.DrawRectangle(pen, x, y, 1, 1);
                }
            }

            int left = 0, top = 0, top1 = 1, top2 = 1;

            int n1 = (imageHeight - FontSize - Padding * 2);
            int n2 = n1 / 4;
            top1 = n2;
            top2 = n2 * 2;

            Font f;
            Brush b;
            int cindex, findex;

            //随机字体和颜色的验证码字符
            for (int i = 0; i < code.Length; i++)
            {
                cindex = rand.Next(Colors.Length - 1);
                findex = rand.Next(Fonts.Length - 1);

                f = new System.Drawing.Font(Fonts[findex], fSize, System.Drawing.FontStyle.Bold);
                b = new System.Drawing.SolidBrush(Colors[cindex]);

                if (i % 2 == 1)
                {
                    top = top2;
                }
                else
                {
                    top = top1;
                }

                left = i * fWidth;

                g.DrawString(code.Substring(i, 1), f, b, left, top);
            }

            //画一个边框 边框颜色为Color.Gainsboro
            g.DrawRectangle(new Pen(Color.Gainsboro, 0), 0, 0, image.Width - 1, image.Height - 1);
            g.Dispose();

            //产生波形
            image = TwistImage(image, true, 0, 4);
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            //将图像保存到指定的流
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return File(ms.GetBuffer(), "image/JPEG");
        }
         

        #region 验证码长度

        private int length = 4;

        private int Length
        {
            get { return length; }
            set { length = value; }
        }

        #endregion 验证码长度

        #region 验证码字体大小(为了显示扭曲效果，默认40像素，可以自行修改)

        private int fontSize = 13;

        private int FontSize
        {
            get { return fontSize; }
            set { fontSize = value; }
        }

        #endregion 验证码字体大小(为了显示扭曲效果，默认40像素，可以自行修改)

        #region 边框补(默认1像素)

        private int padding = 2;

        private int Padding
        {
            get { return padding; }
            set { padding = value; }
        }

        #endregion 边框补(默认1像素)

        #region 是否输出燥点(默认不输出)

        private bool chaos = false;

        private bool Chaos
        {
            get { return chaos; }
            set { chaos = value; }
        }

        #endregion 是否输出燥点(默认不输出)

        #region 输出燥点的颜色(默认灰色)

        private Color chaosColor = Color.LightGray;

        private Color ChaosColor
        {
            get { return chaosColor; }
            set { chaosColor = value; }
        }

        #endregion 输出燥点的颜色(默认灰色)

        #region 自定义背景色(默认白色)

        private Color backgroundColor = Color.White;

        private Color BackgroundColor
        {
            get { return backgroundColor; }
            set { backgroundColor = value; }
        }

        #endregion 自定义背景色(默认白色)

        #region 自定义随机颜色数组

        private Color[] colors = { Color.Black, Color.Red, Color.DarkBlue, Color.Green, Color.Orange, Color.Brown, Color.DarkCyan, Color.Purple };

        private Color[] Colors
        {
            get { return colors; }
            set { colors = value; }
        }

        #endregion 自定义随机颜色数组

        #region 自定义字体数组

        private string[] fonts = { "Arial", "Georgia" };

        private string[] Fonts
        {
            get { return fonts; }
            set { fonts = value; }
        }

        #endregion 自定义字体数组

        #region 自定义随机码字符串序列(使用逗号分隔)

        private string codeSerial = "0,1,2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,i,j,k,m,n,o,p,q,r,s,t,u,v,w,x,y,z,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";

        private string CodeSerial
        {
            get { return codeSerial; }
            set { codeSerial = value; }
        }

        #endregion 自定义随机码字符串序列(使用逗号分隔)
         
        #region 产生波形滤镜效果

        private const double PI = 3.1415926535897932384626433832795;
        private const double PI2 = 6.283185307179586476925286766559;

        /// <summary>
        /// 正弦曲线Wave扭曲图片（Edit By 51aspx.com）
        /// </summary>
        /// <param name="srcBmp">图片路径</param>
        /// <param name="bXDir">如果扭曲则选择为True</param>
        /// <param name="nMultValue">波形的幅度倍数，越大扭曲的程度越高，一般为3</param>
        /// <param name="dPhase">波形的起始相位，取值区间[0-2*PI)</param>
        /// <returns></returns>
        private System.Drawing.Bitmap TwistImage(Bitmap srcBmp, bool bXDir, double dMultValue, double dPhase)
        {
             
            System.Drawing.Bitmap destBmp = new Bitmap(srcBmp.Width, srcBmp.Height);

            // 将位图背景填充为白色
            System.Drawing.Graphics graph = System.Drawing.Graphics.FromImage(destBmp);
            graph.FillRectangle(new SolidBrush(System.Drawing.Color.White), 0, 0, destBmp.Width, destBmp.Height);
            graph.Dispose();

            double dBaseAxisLen = bXDir ? (double)destBmp.Height : (double)destBmp.Width;

            for (int i = 0; i < destBmp.Width; i++)
            {
                for (int j = 0; j < destBmp.Height; j++)
                {
                    double dx = 0;
                    dx = bXDir ? (PI2 * (double)j) / dBaseAxisLen : (PI2 * (double)i) / dBaseAxisLen;
                    dx += dPhase;
                    double dy = Math.Sin(dx);

                    // 取得当前点的颜色
                    int nOldX = 0, nOldY = 0;
                    nOldX = bXDir ? i + (int)(dy * dMultValue) : i;
                    nOldY = bXDir ? j : j + (int)(dy * dMultValue);

                    System.Drawing.Color color = srcBmp.GetPixel(i, j);
                    if (nOldX >= 0 && nOldX < destBmp.Width
                     && nOldY >= 0 && nOldY < destBmp.Height)
                    {
                        destBmp.SetPixel(nOldX, nOldY, color);
                    }
                }
            }

            return destBmp;
        }

        #endregion 产生波形滤镜效果

        #region 生成随机字符码

        private string CreateVerifyCode(int codeLen)
        {
            if (codeLen == 0)
            {
                codeLen = Length;
            }

            string[] arr = CodeSerial.Split(',');

            string code = "";

            int randValue = -1;

            Random rand = new Random(unchecked((int)DateTime.Now.Ticks));

            for (int i = 0; i < codeLen; i++)
            {
                randValue = rand.Next(0, arr.Length - 1);

                code += arr[randValue];
            }

            return code;
        }

        #endregion 生成随机字符码
    }
}