using EADS.Server.Handler;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text; 
using System.Windows.Forms;
using EADS.Common;

namespace EADS.Server
{
    public partial class FormServer : Form , NumChangedListener 
    {
        public FormServer()  
        { 
            InitializeComponent();      
        }
        private bool _serverRuning = false; //服务端是否在运行中
        private FileHandler _fileHandler; //文件处理类
        IniFile _iniFile = null;
        private OrderHandler _orderHandler;//订单回调处理

        private void FormServer_Load(object sender, EventArgs e)
        {

            initConfigByIni(); //初始化系统配置文件
            _fileHandler = new FileHandler(); 
            _fileHandler.CountChangedListener = this;
            _orderHandler = new OrderHandler();
            _orderHandler.setNumChangedListener(this);
            bool autoRun = _iniFile.ReadBool("SYSTEM", "AutoStart", false);
            if (autoRun)
            {
                startServer();
            }
        }

        public void NumberChanged() {
            Invoke(new MethodInvoker(delegate{

                lb_zipFileCount.Text = "接收压缩包：" + _fileHandler.ZipFileTotalCount;
                lb_unZipedCount.Text = "解压完成：" + _fileHandler.UnzipedSucceedCount;
                lb_notUnzipCount.Text = "待解压：" + (_fileHandler.ZipFileTotalCount - _fileHandler.UnzipedSucceedCount);
                lb_photoCount.Text = "解压图片数量：" + _fileHandler.UnzipedPhotoCount;
                lb_creatOrder.Text = "生成订单数量：" + _fileHandler.CreatedOrderCount;
                lb_createOrderFailed.Text = "生成订单失败：";

                lb_orderHandled.Text = "订单识别完成数量：" +_orderHandler.OrderHandleNum;
                lb_waitForCallback.Text = "待回传数量：" + _orderHandler.WaitForCallbackOrderNum;
                lb_callback_succeed.Text = "回传成功数量：" + _orderHandler.CallbcakSucceedNum;
            }));
         
        }

        private void btn_start_server_Click(object sender, EventArgs e)
        { 
            if (!_serverRuning) //开启监听服务
            {
                startServer();
            }
            else
            { //关闭监听
                stopServer();
            }
        }
         
        private void fileSystemWatcher_EventHandle(object sender, FileSystemEventArgs e)  //文件增删改时被调用的处理方法  
        {
            //LogHelper.WriteLog("压缩包changed事件...");
            //.WriteLine(" <<====== 压缩包 ：" + e.Name + " Type:" + (int)e.ChangeType);
            if (_serverRuning)
            {
                _fileHandler.handleFileChanged(e.Name, e.FullPath);
            }
            else
            {
                //未开启的时候如何处理
            }
        }

        private void btn_save_setting_Click(object sender, EventArgs e)
        {
            if (saveSettings()) {
                System.Windows.Forms.MessageBox.Show("保存成功！");
            }
        }

        /// <summary>
        /// 初始化系统设置ini文件
        /// </summary>
        private void initConfigByIni()
        {
            if (_iniFile == null)
            {
                bool iniExist = false;
                _iniFile = new IniFile(CommonConst.GetIniFilePath(), out iniExist);
                if (!iniExist)
                {
                    //ini文件新创建时内部配置均为空 初始化相关配置名称  方便手动修改文件
                    //数据库相关
                    _iniFile.WriteString("DATABASE", "DBNAME", "expressaddress");
                    _iniFile.WriteString("DATABASE", "IP", "127.0.0.1");
                    _iniFile.WriteString("DATABASE", "PORT", "3306");
                    _iniFile.WriteString("DATABASE", "USER", "root");
                    _iniFile.WriteString("DATABASE", "PWD", "206366");
                    //文件相关
                    _iniFile.WriteString("FILE", "ZipRootDir", "E:\\EADS\\Res\\Ftp\\");
                    _iniFile.WriteString("FILE", "UnzipPhotoToDir", "E:\\EADS\\Res\\Photo\\");
                    //系统相关
                    _iniFile.WriteBool("SYSTEM", "AutoStart", false);
                    _iniFile.WriteString("SYSTEM", "CallbackUrl", "http://116.228.72.129:6688/irds/rcv/reco_rep/addData.do");
                    _iniFile.WriteString("SYSTEM", "CompanyCode", "666666");
                    _iniFile.WriteString("SYSTEM", "SecureCode", "b2QwgcG54jsmAdVGaH6nuRTSaJHCcD5j");
           
                    _iniFile.WriteInteger("SYSTEM", "FindWaitForCallbackTimespan",60);
                    _iniFile.WriteInteger("SYSTEM", "CallbackThreadSleepTimeIfNoOrder",60);
                    _iniFile.WriteInteger("SYSTEM", "CallbackThreadCount", 1);
                    _iniFile.WriteInteger("SYSTEM", "OnceCallbackOrderCount", 3);

                    

                } 
            } 
            cb_autoRun.Checked = _iniFile.ReadBool("SYSTEM", "AutoStart", false);
            tb_db_name.Text = _iniFile.ReadString("DATABASE", "DBNAME", "expressaddress");
            tb_ip.Text = _iniFile.ReadString("DATABASE", "IP", "127.0.0.1");
            tb_port.Text = _iniFile.ReadString("DATABASE", "PORT", "3306");
            tb_user.Text = _iniFile.ReadString("DATABASE", "USER", "root");
            tb_ftp_dir.Text = _iniFile.ReadString("FILE", "ZipRootDir", "E:\\EADS\\Res\\Ftp\\");
            tb_unzip_dir.Text = _iniFile.ReadString("FILE", "UnzipPhotoToDir", "E:\\EADS\\Res\\Photo\\");

            tb_callbackUrl.Text = _iniFile.ReadString("SYSTEM", "CallbackUrl",
                "http://116.228.72.129:6688/irds/rcv/reco_rep/addData.do");
            tb_select_db_timespan.Text = _iniFile.ReadString("SYSTEM", "FindWaitForCallbackTimespan", "20");
            tb_callback_noorder_sleeptime.Text = _iniFile.ReadString("SYSTEM", "CallbackThreadSleepTimeIfNoOrder", "20");
            tb_callback_thread_num.Text = _iniFile.ReadString("SYSTEM", "CallbackThreadCount", "3");
            tb_once_callback_num.Text = _iniFile.ReadString("SYSTEM", "OnceCallbackOrderCount", "3");
        }

        

        /// <summary>
        /// 开启服务
        ///// </summary>
        private void startServer()
        {  
            if (!Directory.Exists(tb_ftp_dir.Text.Trim()))
            {
                System.Windows.Forms.MessageBox.Show("请在配置中填写正确的FTP文件夹！");
                return;
            }
            if (!new BLL.BLL_Test().IsDabaseConnectedOk()) {
                System.Windows.Forms.MessageBox.Show("数据库连接失败，请检查配置！");
                return;
            }
            btn_start_server.Text = "暂停";
            fileWatcher.Path = tb_ftp_dir.Text.Trim();
            fileWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.CreationTime;
            this.fileWatcher.Changed += new FileSystemEventHandler(fileSystemWatcher_EventHandle);
            _serverRuning = true;
            cb_autoRun.Enabled = false;
            btn_save_setting.Enabled = false;
            setInputEnable(false);
            _iniFile.WriteBool("SYSTEM", "AutoStart", cb_autoRun.CheckState == CheckState.Checked);

            _orderHandler.StartHandle();
            LogHelper.WriteLog("开启服务...");
        }

        /// <summary>
        /// 关闭服务
        /// </summary>
        private void stopServer()
        {
            btn_start_server.Text = "开启";
            _serverRuning = false;
            btn_save_setting.Enabled = true;
            cb_autoRun.Enabled = true;
            setInputEnable(true);
            _orderHandler.Stop();
            LogHelper.WriteLog("关闭服务...");
        }

        private void setInputEnable(bool enabled) {
            tb_db_name.Enabled = enabled;
            tb_user.Enabled = enabled;
            tb_password.Enabled = enabled;
            tb_ip.Enabled = enabled;
            tb_port.Enabled = enabled;
            tb_ftp_dir.Enabled = enabled;
            tb_unzip_dir.Enabled = enabled;
            tb_select_db_timespan.Enabled = enabled;
            tb_callback_noorder_sleeptime.Enabled = enabled;
            tb_callback_thread_num.Enabled = enabled;
            tb_once_callback_num.Enabled = enabled;
            tb_callbackUrl.Enabled = enabled;
        }

        /// <summary>
        /// 保存系统设置
        /// </summary>
        /// <returns></returns>
        private bool saveSettings()
        {
            string dbName = tb_db_name.Text.Trim();
            string ip = tb_ip.Text.Trim();
            string port = tb_port.Text.Trim();
            string user = tb_user.Text.Trim();
            string password = tb_password.Text.Trim();
            string ftpDir = tb_ftp_dir.Text.Trim();
            string unzipDir = tb_unzip_dir.Text.Trim();
              
            if (string.IsNullOrEmpty(dbName))
            {
                System.Windows.Forms.MessageBox.Show("请在配置中填写数据库名称！");
                return false;
            }
            if (string.IsNullOrEmpty(ip))
            {
                System.Windows.Forms.MessageBox.Show("请在配置中填写数据库IP！");
                return false;
            }
            if (string.IsNullOrEmpty(port))
            {
                System.Windows.Forms.MessageBox.Show("请在配置中填写数据库端口！");
                return false;
            }
            if (string.IsNullOrEmpty(user))
            {
                System.Windows.Forms.MessageBox.Show("请在配置中填写数据库用户名！");
                return false;
            }
            if (string.IsNullOrEmpty(password))
            {
                System.Windows.Forms.MessageBox.Show("请在配置中填写数据库密码！");
                return false;
            }
            if (string.IsNullOrEmpty(ftpDir))
            {
                System.Windows.Forms.MessageBox.Show("请在配置中填写监控的FTP目录！");
                return false;
            }
            if (string.IsNullOrEmpty(unzipDir))
            {
                System.Windows.Forms.MessageBox.Show("请在配置中填写图片解压到的目录！");
                return false;
            }
            if (!Directory.Exists(ftpDir))
            {
                System.Windows.Forms.MessageBox.Show("当前配置的FTP文件夹不存在！");
                return false;
            }
            if (!Directory.Exists(unzipDir))
            {
                System.Windows.Forms.MessageBox.Show("当前配置的解压目标文件夹不存在！");
                return false;
            }
            //先保存数据库相关配置 然后尝试连接
            _iniFile.WriteString("DATABASE", "DBNAME", dbName);
            _iniFile.WriteString("DATABASE", "IP", ip);
            _iniFile.WriteString("DATABASE", "PORT", port);
            _iniFile.WriteString("DATABASE", "USER", user);
            _iniFile.WriteString("DATABASE", "PWD", password);
            if (!new BLL.BLL_Test().IsDabaseConnectedOk())
            {
                System.Windows.Forms.MessageBox.Show("数据库连接失败，请检查配置！");
                return false;
            }
            int selectDbSpan = 20;
            int noorderCallbackSleep = 20;
            int callbackThreadNum = 3;
            int onceCallbackNum = 3;
            if (string.IsNullOrEmpty(tb_callbackUrl.Text.Trim()))
            {
                System.Windows.Forms.MessageBox.Show("请在配置中填写查询订单回传Url！");
                return false;
            }
            if (!int.TryParse(tb_select_db_timespan.Text.Trim(),out selectDbSpan))
            {
                System.Windows.Forms.MessageBox.Show("请在配置中填写查询需回传订单的间隔时间！");
                return false;
            }
            if (!int.TryParse(tb_callback_noorder_sleeptime.Text.Trim(), out noorderCallbackSleep))
            {
                System.Windows.Forms.MessageBox.Show("请在配置中填写无需回传订单时回传线程休眠时间！");
                return false;
            }
            if (!int.TryParse(tb_callback_thread_num.Text.Trim(), out callbackThreadNum))
            {
                System.Windows.Forms.MessageBox.Show("请在配置中填写回传线程数！");
                return false;
            }
            if (  callbackThreadNum <=0 )
            {
                System.Windows.Forms.MessageBox.Show("回传线程数需大于0，至少为1！");
                return false;
            }
            if (!int.TryParse(tb_once_callback_num.Text.Trim(), out onceCallbackNum))
            {
                System.Windows.Forms.MessageBox.Show("请在配置中填写单次回传订单数量！");
                return false;
            }
            if (onceCallbackNum <= 0)
            {
                System.Windows.Forms.MessageBox.Show("单次回传订单数需大于0，至少为1！");
                return false;
            }
            //string noorderCallbackSleep = tb_callback_noorder_sleeptime.Text.Trim();
            //string callbackThreadNum = tb_callback_thread_num.Text.Trim();
            //string onceCallbackNum = tb_once_callback_num.Text.Trim();
         
            //文件相关
            _iniFile.WriteString("FILE", "ZipRootDir", ftpDir);
            _iniFile.WriteString("FILE", "UnzipPhotoToDir", unzipDir);
            //订单回传设置

            _iniFile.WriteString("SYSTEM", "CallbackUrl", tb_callbackUrl.Text.Trim());
            _iniFile.WriteInteger("SYSTEM", "FindWaitForCallbackTimespan", selectDbSpan);
            _iniFile.WriteInteger("SYSTEM", "CallbackThreadSleepTimeIfNoOrder", noorderCallbackSleep);
            _iniFile.WriteInteger("SYSTEM", "CallbackThreadCount", callbackThreadNum);
            _iniFile.WriteInteger("SYSTEM", "OnceCallbackOrderCount", onceCallbackNum);
            return true;
        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            NumberChanged();
        }

        private void btn_test_Click(object sender, EventArgs e)
        {
            //======================测试使用======================

         
           // string result = orderHandler.callbackOrder();

        }

        private void FormServer_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
