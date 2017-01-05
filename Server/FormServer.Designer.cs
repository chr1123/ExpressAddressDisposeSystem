namespace EADS.Server
{
    partial class FormServer
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormServer));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.lb_callback_succeed = new System.Windows.Forms.Label();
            this.btn_test = new System.Windows.Forms.Button();
            this.btn_refresh = new System.Windows.Forms.Button();
            this.cb_autoRun = new System.Windows.Forms.CheckBox();
            this.lb_createOrderFailed = new System.Windows.Forms.Label();
            this.lb_notUnzipCount = new System.Windows.Forms.Label();
            this.lb_waitForCallback = new System.Windows.Forms.Label();
            this.btn_start_server = new System.Windows.Forms.Button();
            this.lb_creatOrder = new System.Windows.Forms.Label();
            this.lb_unZipedCount = new System.Windows.Forms.Label();
            this.lb_orderHandled = new System.Windows.Forms.Label();
            this.lb_photoCount = new System.Windows.Forms.Label();
            this.lb_zipFileCount = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tb_callbackUrl = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.tb_once_callback_num = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.tb_callback_thread_num = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tb_callback_noorder_sleeptime = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tb_select_db_timespan = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tb_password = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tb_user = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_db_name = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_save_setting = new System.Windows.Forms.Button();
            this.tb_port = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.tb_ip = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tb_unzip_dir = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tb_ftp_dir = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.fileWatcher = new System.IO.FileSystemWatcher();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fileWatcher)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(635, 419);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.lb_callback_succeed);
            this.tabPage1.Controls.Add(this.btn_test);
            this.tabPage1.Controls.Add(this.btn_refresh);
            this.tabPage1.Controls.Add(this.cb_autoRun);
            this.tabPage1.Controls.Add(this.lb_createOrderFailed);
            this.tabPage1.Controls.Add(this.lb_notUnzipCount);
            this.tabPage1.Controls.Add(this.lb_waitForCallback);
            this.tabPage1.Controls.Add(this.btn_start_server);
            this.tabPage1.Controls.Add(this.lb_creatOrder);
            this.tabPage1.Controls.Add(this.lb_unZipedCount);
            this.tabPage1.Controls.Add(this.lb_orderHandled);
            this.tabPage1.Controls.Add(this.lb_photoCount);
            this.tabPage1.Controls.Add(this.lb_zipFileCount);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(627, 390);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "服务器";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // lb_callback_succeed
            // 
            this.lb_callback_succeed.AutoSize = true;
            this.lb_callback_succeed.Location = new System.Drawing.Point(417, 152);
            this.lb_callback_succeed.Name = "lb_callback_succeed";
            this.lb_callback_succeed.Size = new System.Drawing.Size(120, 15);
            this.lb_callback_succeed.TabIndex = 12;
            this.lb_callback_succeed.Text = "回传成功数量：0";
            // 
            // btn_test
            // 
            this.btn_test.Location = new System.Drawing.Point(525, 272);
            this.btn_test.Name = "btn_test";
            this.btn_test.Size = new System.Drawing.Size(75, 30);
            this.btn_test.TabIndex = 11;
            this.btn_test.Text = "Test";
            this.btn_test.UseVisualStyleBackColor = true;
            this.btn_test.Visible = false;
            this.btn_test.Click += new System.EventHandler(this.btn_test_Click);
            // 
            // btn_refresh
            // 
            this.btn_refresh.Location = new System.Drawing.Point(322, 334);
            this.btn_refresh.Name = "btn_refresh";
            this.btn_refresh.Size = new System.Drawing.Size(75, 30);
            this.btn_refresh.TabIndex = 10;
            this.btn_refresh.Text = "刷新";
            this.btn_refresh.UseVisualStyleBackColor = true;
            this.btn_refresh.Click += new System.EventHandler(this.btn_refresh_Click);
            // 
            // cb_autoRun
            // 
            this.cb_autoRun.AutoSize = true;
            this.cb_autoRun.Location = new System.Drawing.Point(421, 341);
            this.cb_autoRun.Name = "cb_autoRun";
            this.cb_autoRun.Size = new System.Drawing.Size(89, 19);
            this.cb_autoRun.TabIndex = 9;
            this.cb_autoRun.Text = "自动开启";
            this.cb_autoRun.UseVisualStyleBackColor = true;
            // 
            // lb_createOrderFailed
            // 
            this.lb_createOrderFailed.AutoSize = true;
            this.lb_createOrderFailed.Location = new System.Drawing.Point(417, 101);
            this.lb_createOrderFailed.Name = "lb_createOrderFailed";
            this.lb_createOrderFailed.Size = new System.Drawing.Size(120, 15);
            this.lb_createOrderFailed.TabIndex = 8;
            this.lb_createOrderFailed.Text = "生成订单失败：0";
            this.lb_createOrderFailed.Visible = false;
            // 
            // lb_notUnzipCount
            // 
            this.lb_notUnzipCount.AutoSize = true;
            this.lb_notUnzipCount.Location = new System.Drawing.Point(417, 54);
            this.lb_notUnzipCount.Name = "lb_notUnzipCount";
            this.lb_notUnzipCount.Size = new System.Drawing.Size(75, 15);
            this.lb_notUnzipCount.TabIndex = 7;
            this.lb_notUnzipCount.Text = "待解压：0";
            // 
            // lb_waitForCallback
            // 
            this.lb_waitForCallback.AutoSize = true;
            this.lb_waitForCallback.Location = new System.Drawing.Point(236, 152);
            this.lb_waitForCallback.Name = "lb_waitForCallback";
            this.lb_waitForCallback.Size = new System.Drawing.Size(105, 15);
            this.lb_waitForCallback.TabIndex = 6;
            this.lb_waitForCallback.Text = "待回传数量：0";
            // 
            // btn_start_server
            // 
            this.btn_start_server.Location = new System.Drawing.Point(525, 334);
            this.btn_start_server.Name = "btn_start_server";
            this.btn_start_server.Size = new System.Drawing.Size(75, 30);
            this.btn_start_server.TabIndex = 5;
            this.btn_start_server.Text = "开启";
            this.btn_start_server.UseVisualStyleBackColor = true;
            this.btn_start_server.Click += new System.EventHandler(this.btn_start_server_Click);
            // 
            // lb_creatOrder
            // 
            this.lb_creatOrder.AutoSize = true;
            this.lb_creatOrder.Location = new System.Drawing.Point(236, 101);
            this.lb_creatOrder.Name = "lb_creatOrder";
            this.lb_creatOrder.Size = new System.Drawing.Size(120, 15);
            this.lb_creatOrder.TabIndex = 4;
            this.lb_creatOrder.Text = "生成订单数量：0";
            // 
            // lb_unZipedCount
            // 
            this.lb_unZipedCount.AutoSize = true;
            this.lb_unZipedCount.Location = new System.Drawing.Point(236, 54);
            this.lb_unZipedCount.Name = "lb_unZipedCount";
            this.lb_unZipedCount.Size = new System.Drawing.Size(90, 15);
            this.lb_unZipedCount.TabIndex = 3;
            this.lb_unZipedCount.Text = "解压完成：0";
            // 
            // lb_orderHandled
            // 
            this.lb_orderHandled.AutoSize = true;
            this.lb_orderHandled.Location = new System.Drawing.Point(51, 152);
            this.lb_orderHandled.Name = "lb_orderHandled";
            this.lb_orderHandled.Size = new System.Drawing.Size(150, 15);
            this.lb_orderHandled.TabIndex = 2;
            this.lb_orderHandled.Text = "订单识别完成数量：0";
            // 
            // lb_photoCount
            // 
            this.lb_photoCount.AutoSize = true;
            this.lb_photoCount.Location = new System.Drawing.Point(51, 101);
            this.lb_photoCount.Name = "lb_photoCount";
            this.lb_photoCount.Size = new System.Drawing.Size(120, 15);
            this.lb_photoCount.TabIndex = 1;
            this.lb_photoCount.Text = "解压图片数量：0";
            // 
            // lb_zipFileCount
            // 
            this.lb_zipFileCount.AutoSize = true;
            this.lb_zipFileCount.Location = new System.Drawing.Point(51, 54);
            this.lb_zipFileCount.Name = "lb_zipFileCount";
            this.lb_zipFileCount.Size = new System.Drawing.Size(105, 15);
            this.lb_zipFileCount.TabIndex = 0;
            this.lb_zipFileCount.Text = "接收压缩包：0";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.tb_callbackUrl);
            this.tabPage2.Controls.Add(this.label12);
            this.tabPage2.Controls.Add(this.tb_once_callback_num);
            this.tabPage2.Controls.Add(this.label11);
            this.tabPage2.Controls.Add(this.tb_callback_thread_num);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.tb_callback_noorder_sleeptime);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.tb_select_db_timespan);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.tb_password);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.tb_user);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.tb_db_name);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.btn_save_setting);
            this.tabPage2.Controls.Add(this.tb_port);
            this.tabPage2.Controls.Add(this.label10);
            this.tabPage2.Controls.Add(this.tb_ip);
            this.tabPage2.Controls.Add(this.label9);
            this.tabPage2.Controls.Add(this.tb_unzip_dir);
            this.tabPage2.Controls.Add(this.label8);
            this.tabPage2.Controls.Add(this.tb_ftp_dir);
            this.tabPage2.Controls.Add(this.label7);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(627, 390);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "配置";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tb_callbackUrl
            // 
            this.tb_callbackUrl.Location = new System.Drawing.Point(199, 204);
            this.tb_callbackUrl.Name = "tb_callbackUrl";
            this.tb_callbackUrl.Size = new System.Drawing.Size(338, 25);
            this.tb_callbackUrl.TabIndex = 24;
            this.tb_callbackUrl.Text = "http://116.228.72.129:6688/irds/rcv/reco_rep/addData.do";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(58, 208);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(106, 15);
            this.label12.TabIndex = 23;
            this.label12.Text = "订单回传Url：";
            // 
            // tb_once_callback_num
            // 
            this.tb_once_callback_num.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tb_once_callback_num.Location = new System.Drawing.Point(480, 312);
            this.tb_once_callback_num.Name = "tb_once_callback_num";
            this.tb_once_callback_num.Size = new System.Drawing.Size(46, 27);
            this.tb_once_callback_num.TabIndex = 22;
            this.tb_once_callback_num.Text = "3";
            this.tb_once_callback_num.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(339, 317);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(127, 15);
            this.label11.TabIndex = 21;
            this.label11.Text = "单次回传订单数：";
            // 
            // tb_callback_thread_num
            // 
            this.tb_callback_thread_num.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tb_callback_thread_num.Location = new System.Drawing.Point(199, 307);
            this.tb_callback_thread_num.Name = "tb_callback_thread_num";
            this.tb_callback_thread_num.Size = new System.Drawing.Size(46, 27);
            this.tb_callback_thread_num.TabIndex = 20;
            this.tb_callback_thread_num.Text = "3";
            this.tb_callback_thread_num.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(58, 312);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(127, 15);
            this.label6.TabIndex = 19;
            this.label6.Text = "订单回传线程数：";
            // 
            // tb_callback_noorder_sleeptime
            // 
            this.tb_callback_noorder_sleeptime.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tb_callback_noorder_sleeptime.Location = new System.Drawing.Point(349, 272);
            this.tb_callback_noorder_sleeptime.Name = "tb_callback_noorder_sleeptime";
            this.tb_callback_noorder_sleeptime.Size = new System.Drawing.Size(46, 27);
            this.tb_callback_noorder_sleeptime.TabIndex = 18;
            this.tb_callback_noorder_sleeptime.Text = "30";
            this.tb_callback_noorder_sleeptime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(58, 241);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(271, 15);
            this.label5.TabIndex = 17;
            this.label5.Text = "查询需回传订单的间隔时间(单位为s)：";
            // 
            // tb_select_db_timespan
            // 
            this.tb_select_db_timespan.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tb_select_db_timespan.Location = new System.Drawing.Point(349, 236);
            this.tb_select_db_timespan.Name = "tb_select_db_timespan";
            this.tb_select_db_timespan.Size = new System.Drawing.Size(46, 27);
            this.tb_select_db_timespan.TabIndex = 16;
            this.tb_select_db_timespan.Text = "30";
            this.tb_select_db_timespan.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(58, 277);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(277, 15);
            this.label4.TabIndex = 15;
            this.label4.Text = "当前无需回传订单时回传线程休眠时间：";
            // 
            // tb_password
            // 
            this.tb_password.Location = new System.Drawing.Point(422, 93);
            this.tb_password.Name = "tb_password";
            this.tb_password.PasswordChar = '*';
            this.tb_password.Size = new System.Drawing.Size(115, 25);
            this.tb_password.TabIndex = 14;
            this.tb_password.Text = "123456";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(364, 96);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 15);
            this.label3.TabIndex = 13;
            this.label3.Text = "密码：";
            // 
            // tb_user
            // 
            this.tb_user.Location = new System.Drawing.Point(199, 93);
            this.tb_user.Name = "tb_user";
            this.tb_user.Size = new System.Drawing.Size(115, 25);
            this.tb_user.TabIndex = 12;
            this.tb_user.Text = "root";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(58, 100);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 15);
            this.label2.TabIndex = 11;
            this.label2.Text = "用户名：";
            // 
            // tb_db_name
            // 
            this.tb_db_name.Location = new System.Drawing.Point(199, 22);
            this.tb_db_name.Name = "tb_db_name";
            this.tb_db_name.Size = new System.Drawing.Size(136, 25);
            this.tb_db_name.TabIndex = 10;
            this.tb_db_name.Text = "expressaddress";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(58, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 15);
            this.label1.TabIndex = 9;
            this.label1.Text = "数据库名称：";
            // 
            // btn_save_setting
            // 
            this.btn_save_setting.Location = new System.Drawing.Point(546, 354);
            this.btn_save_setting.Name = "btn_save_setting";
            this.btn_save_setting.Size = new System.Drawing.Size(75, 30);
            this.btn_save_setting.TabIndex = 8;
            this.btn_save_setting.Text = "保存";
            this.btn_save_setting.UseVisualStyleBackColor = true;
            this.btn_save_setting.Click += new System.EventHandler(this.btn_save_setting_Click);
            // 
            // tb_port
            // 
            this.tb_port.Location = new System.Drawing.Point(422, 54);
            this.tb_port.Name = "tb_port";
            this.tb_port.Size = new System.Drawing.Size(115, 25);
            this.tb_port.TabIndex = 7;
            this.tb_port.Text = "3306";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(364, 60);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(52, 15);
            this.label10.TabIndex = 6;
            this.label10.Text = "端口：";
            // 
            // tb_ip
            // 
            this.tb_ip.Location = new System.Drawing.Point(199, 57);
            this.tb_ip.Name = "tb_ip";
            this.tb_ip.Size = new System.Drawing.Size(115, 25);
            this.tb_ip.TabIndex = 5;
            this.tb_ip.Text = "127.0.0.1";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(58, 64);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(68, 15);
            this.label9.TabIndex = 4;
            this.label9.Text = "IP地址：";
            // 
            // tb_unzip_dir
            // 
            this.tb_unzip_dir.Location = new System.Drawing.Point(199, 164);
            this.tb_unzip_dir.Name = "tb_unzip_dir";
            this.tb_unzip_dir.Size = new System.Drawing.Size(338, 25);
            this.tb_unzip_dir.TabIndex = 3;
            this.tb_unzip_dir.Text = "E:\\EADS\\Res\\Photo";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(58, 168);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(112, 15);
            this.label8.TabIndex = 2;
            this.label8.Text = "解压到文件夹：";
            // 
            // tb_ftp_dir
            // 
            this.tb_ftp_dir.Location = new System.Drawing.Point(199, 129);
            this.tb_ftp_dir.Name = "tb_ftp_dir";
            this.tb_ftp_dir.Size = new System.Drawing.Size(338, 25);
            this.tb_ftp_dir.TabIndex = 1;
            this.tb_ftp_dir.Text = "E:\\EADS\\Res\\Ftp";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(58, 132);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(91, 15);
            this.label7.TabIndex = 0;
            this.label7.Text = "FTP文件夹：";
            // 
            // fileWatcher
            // 
            this.fileWatcher.EnableRaisingEvents = true;
            this.fileWatcher.Filter = "*.flag";
            this.fileWatcher.SynchronizingObject = this;
            // 
            // FormServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(659, 443);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormServer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "韵达面单识别处理系统服务端";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormServer_FormClosed);
            this.Load += new System.EventHandler(this.FormServer_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fileWatcher)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label lb_creatOrder;
        private System.Windows.Forms.Label lb_unZipedCount;
        private System.Windows.Forms.Label lb_orderHandled;
        private System.Windows.Forms.Label lb_photoCount;
        private System.Windows.Forms.Label lb_zipFileCount;
        private System.Windows.Forms.Label lb_waitForCallback;
        private System.Windows.Forms.Button btn_start_server;
        private System.Windows.Forms.TextBox tb_ftp_dir;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tb_unzip_dir;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tb_port;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox tb_ip;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btn_save_setting;
        private System.Windows.Forms.Label lb_notUnzipCount;
        private System.Windows.Forms.Label lb_createOrderFailed;
        private System.Windows.Forms.CheckBox cb_autoRun;
        private System.IO.FileSystemWatcher fileWatcher;
        private System.Windows.Forms.TextBox tb_db_name;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb_password;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tb_user;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_refresh;
        private System.Windows.Forms.Button btn_test;
        private System.Windows.Forms.TextBox tb_select_db_timespan;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tb_callback_thread_num;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tb_callback_noorder_sleeptime;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tb_once_callback_num;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lb_callback_succeed;
        private System.Windows.Forms.TextBox tb_callbackUrl;
        private System.Windows.Forms.Label label12;
    }
}

