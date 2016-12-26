using System;
using System.Globalization;
using System.IO;
using System.Threading; 

namespace EADS.Common
{
    public class LogHelper
    {
        public LogHelper()
        {
            // 
            //   TODO:   在此处添加构造函数逻辑 
            // 
        }
          
        private static string GetLogFile()
        { 
            return System.Environment.CurrentDirectory + "\\log.txt";
        }
          



        private static readonly AutoResetEvent AutoResetEvent = new AutoResetEvent(true);
      
        public static void WriteLog(string log)
        {
            try
            {
                AutoResetEvent.WaitOne();
                using (StreamWriter writer1 = new StreamWriter(GetLogFile(), true))
                {
                    writer1.WriteLine(DateTime.Now.ToString(CultureInfo.InvariantCulture) + ":   " + log);
                    writer1.Close();
                }

                AutoResetEvent.Set();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }


        public static void WriteDataLog(string log)
        {
            try
            {
                AutoResetEvent.WaitOne();
                using (StreamWriter writer1 = new StreamWriter(Environment.CurrentDirectory+ "\\log_data_" 
                    +DateTime.Now.ToString("yyyy_MM_dd")+ ".txt", true))
                {
                    writer1.WriteLine(DateTime.Now.ToString(CultureInfo.InvariantCulture) + ":   " + log);
                    writer1.Close();
                }

                AutoResetEvent.Set();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public static void WriteLogInOneLine(string log)
        {
            try
            {
                AutoResetEvent.WaitOne();
                using (StreamWriter writer1 = new StreamWriter(GetLogFile(), true))
                {
                    writer1.Write(log);
                    writer1.Close();
                }

                AutoResetEvent.Set();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }


        private static string _recordRegisterCodeurl = System.Environment.CurrentDirectory + "\\RegisterCode.txt";
        public static void RecordRegisterCode(string code)
        {
            try
            {
                AutoResetEvent.WaitOne();
                using (StreamWriter writer1 = new StreamWriter(_recordRegisterCodeurl, true))
                {
                    writer1.WriteLine(code);
                    writer1.Close();
                }

                AutoResetEvent.Set();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static readonly AutoResetEvent AutoResetEvent2 = new AutoResetEvent(true);

        public static void WriteDebugLog(string log, DateTime dt, string phonenumber = "")
        {
            return;
            try
            {

                IniFile myini = new IniFile(CommonConst.GetIniFilePath());
                string strdebug = myini.ReadString("SYSTEM", "Debug", "on");
                if (strdebug.ToLower().Equals("on"))
                {
                    AutoResetEvent2.WaitOne();
                    using (StreamWriter writer1 = new StreamWriter(Environment.CurrentDirectory + "\\DebugLog_" + phonenumber + ".txt", true))
                    {
                        writer1.WriteLine(dt.ToString("yyyy-MM-dd HH:mm:ss:fff") + log);
                        writer1.Close();
                    }

                    AutoResetEvent2.Set();
                }

            }
            catch (Exception ex)
            {
                WriteLog(ex.Message);
            }

        }
         


        public static string GetRentCarList()
        {
            string ss = "";
            try
            {
                string path = Environment.CurrentDirectory + "\\RentCar.txt";
                if (!File.Exists(path))
                {
                    File.Create(path);
                }
                StreamReader streamReader = new StreamReader(path);
                ss  = streamReader.ReadToEnd();
                streamReader.Close();
            }
            catch (Exception ex)
            {
                return ss;
            }
            
            return ss;
        }
         
        #region   获得日志文件的地址
        //public   static   string   GetLogFile() 
        //{ 
        //return   HttpContext.Current.Server.MapPath(ConfigurationSettings.AppSettings[ "logfile "]); 
        //} 

        //#endregion 

        //#region   写日志文件 
        //public   static   void   Write(string   log) 
        //{ 
        //LogService.WriteLog(log); 
        //} 
        //  #endregion 

        //#region   
        //public   static   void   WriteAccessLog(string   log) 
        //{ 
        //try 
        //{ 
        //string   text1   =   ConfigurationSettings.AppSettings[ "dgtbawebdatadir "].ToString(); 
        //text1   =   text1   +   @ "log\ "; 
        //if   (!Directory.Exists(text1)) 
        //{ 
        //Directory.CreateDirectory(text1); 
        //} 
        //StreamWriter   writer1   =   new   StreamWriter(text1   +   "ACCESSLOG "   +   DateTime.Today.ToString( "yyyyMMdd ")   +   ".txt ",   true); 
        //writer1.WriteLine(DateTime.Now.ToString()   +   ":       "   +   log); 
        //writer1.Close(); 
        //} 
        //catch   (Exception   exception1) 
        //{ 
        //LogService.Write(exception1.Message); 
        //} 
        //} 


        //#endregion 

        //#region   写管理员日志 
        //public   static   void   WriteAdminLog(string   log) 
        //{ 
        //try 
        //{ 
        //string   text1   =   ConfigurationSettings.AppSettings[ "dgtbawebdatadir "].ToString(); 
        //text1   =   text1   +   @ "log\ "; 
        //if   (!Directory.Exists(text1)) 
        //{ 
        //Directory.CreateDirectory(text1); 
        //} 
        //StreamWriter   writer1   =   new   StreamWriter(text1   +   "ADMINLOG "   +   DateTime.Today.ToString( "yyyyMMdd ")   +   ".txt ",   true); 
        //writer1.WriteLine(DateTime.Now.ToString()   +   ":       "   +   log); 
        //writer1.Close(); 
        //} 
        //catch   (Exception   exception1) 
        //{ 
        //LogService.Write(exception1.Message); 
        //} 
        //} 

        //#endregion 

        //#region     写日志文件 
        //public   static   void   WriteLog(string   log) 
        //{ 
        //try 
        //{ 
        //StreamWriter   writer1   =   new   StreamWriter(LogService.GetLogFile(),   true); 
        //writer1.WriteLine(DateTime.Now.ToString()   +   ":         "   +   log); 
        //writer1.Close(); 
        //} 
        //catch(Exception   ex) 
        //{ 

        //} 

        //} 


        //#endregion 

        //#   region   创建日期文件夹 
        //public   static   string     CreateFile() 
        //{ 
        //                          string   text1= "images/ "+DateTime.Today.ToString( "yyyyMMdd "); 
        //if(!Directory.Exists(text1)) 
        //{ 
        //Directory.CreateDirectory(HttpContext.Current.Server.MapPath(text1)); 
        //} 
        //return   text1; 
        //} 

        #endregion
    }
} 