using EADS.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using EADS.BLL;
using EADS.Model;
using System.Threading;

namespace EADS.Server.Handler
{
    /// <summary>
    /// 负责文件处理相关业务
    /// </summary>
    public class FileHandler
    {
        //private string unzipRootDir = "D:\\Web\\EADS\\Photos\\";//压缩文件解压到的文件夹
        private string _unzipRootDir = "";//压缩文件解压到的文件夹


        private bool _isRunning = false;
        private Thread _threadDirListenner;// 用于监听文件夹
        private int _dirListenerSleepTime = 10;//文件夹监听扫描间隔时间 单位为s
        private Thread[] _threadFileHandlers;//用于解压文件及生成订单
        private int _zipFileHandleThreadNum = 5;//解压的线程数量

        Dictionary<string, NewFile> _newFileList;//新收zip文件列表
        private Queue<NewFile> _queueFile;

        public class NewFile {
            public NewFile(string name, string full) {
                changedCount = 1;
                State = ZipFileState.New;
                fileName = name;
                fullPath = full;
            }
            public int changedCount = 0;
            public string fileName;
            public string fullPath;
            public ZipFileState State;
            public int FileID;
            public int UnzipFaildCount = 0;

            //0-尚未解压 1-入库成功 2-入库失败 3-解压成功 4-解压失败
            public enum ZipFileState { New, InsertDbSucceed, InsertDbFailed, UnzipSucceed, UnzipFailed };
        }

        public int ZipFileTotalCount;//接收zip文件总数量
        public int UnzipedSucceedCount;//解压成功数量
                                       // public int UnzipedFailedCount; //解压失败数量
        public int UnzipedPhotoCount;//解压出的图片数量
        public int CreatedOrderCount; //订单生成数量

        public NumChangedListener CountChangedListener;


        public FileHandler() {
            IniFile iniFiles = new IniFile(CommonConst.GetIniFilePath());
            _unzipRootDir = iniFiles.ReadString("SYSTEM", "UnzipPhotoToDir", "E:\\EADS\\Res\\Photo\\");
            _zipFileHandleThreadNum = iniFiles.ReadInteger("SYSTEM", "ZipFileHandleThreadNum", 5);
        }

        private void refreshUI()
        {
            if (CountChangedListener != null)
            {
                CountChangedListener.NumberChanged();
            } 
        }

        public void Start() {

            _newFileList = new Dictionary<string, NewFile>();
            _queueFile = new Queue<NewFile>();

            _isRunning = true;
            _threadDirListenner = new Thread(findNewReceivedZipFilesInFtpRootDir);
            _threadDirListenner.Start();


            _threadDirListenner = new Thread(findNewReceivedZipFilesInFtpRootDir);
            _threadDirListenner.Start();

            _threadFileHandlers = new Thread[_zipFileHandleThreadNum];
            for (int i = 0; i < _threadFileHandlers.Length; i++) {
                Thread t = new Thread(handZipFleFromQueue);
                t.Start();
            }
        }

        public void Stop() {
            _isRunning = false;
            _newFileList.Clear();
            _queueFile.Clear();
            if (_threadDirListenner != null && _threadDirListenner.IsAlive) {
                _threadDirListenner.Abort();
            }
            if (_threadFileHandlers != null )
            {
                foreach (Thread t in _threadFileHandlers) {
                    if (t != null && t.IsAlive) {
                        t.Abort();
                    }
                } 
            } 
        }


        //定时读取fpt根目录下的flag文件  将新文件加入到Dictionary
        private void findNewReceivedZipFilesInFtpRootDir() {
            IniFile iniFile = new IniFile(CommonConst.GetIniFilePath());
            string ftpDir = iniFile.ReadString("FILE", "ZipRootDir", "E:\\EADS\\Res\\Ftp\\");
            while (_isRunning) {
                DirectoryInfo ftpRootDir = new DirectoryInfo(ftpDir);
                if (Directory.Exists(ftpDir))
                {
                    FileInfo[] flagFiles = ftpRootDir.GetFiles("*.flag");
                    foreach (FileInfo file in flagFiles)
                    {
                        lock (_newFileList) {
                            if (!_newFileList.ContainsKey(file.Name.Replace("flag", "zip")))
                            {
                                NewFile zipFile = new NewFile(file.Name.Replace("flag","zip"), file.FullName.Replace("flag", "zip"));
                                _newFileList.Add(file.Name.Replace("flag", "zip"), zipFile);
                                lock (_queueFile) {
                                    _queueFile.Enqueue(zipFile);
                                }
                                ZipFileTotalCount++;
                                refreshUI();
                            }
                        }  
                    }
                }
                Thread.Sleep(_dirListenerSleepTime);
            }
          
        }

        private void handZipFleFromQueue() {
            Thread.Sleep(3000);//延迟3s 以确保扫描文件的线程已执行
            while (_isRunning) {
                NewFile zipFile = null;
                lock (_queueFile) {
                    if (_queueFile.Count > 0)
                    {
                        zipFile = _queueFile.Dequeue();
                    }
                }
                if (zipFile != null)
                {
                    handleFileChanged(zipFile);
                }
                else {
                    //队列中已无数据
                    Thread.Sleep(_dirListenerSleepTime);
                }
            } 
        }
        

        //接收到新的文件改变事件
        public void handleFileChanged(NewFile zipFile) {

 
            BLL_Zipfile bZipFile = new BLL.BLL_Zipfile();
            if (!File.Exists(zipFile.fullPath))
            {
                //文件已被删  则从列表中清除
                _newFileList.Remove(zipFile.fileName);
                refreshUI();
                return;
            }
            //待解压新包 或插入数据库失败的包 重新入库
            if (zipFile.State == NewFile.ZipFileState.New || zipFile.State == NewFile.ZipFileState.InsertDbFailed)
            {

                Model.Model_Zipfile model = new Model.Model_Zipfile();
                model.CreateTime = DateTime.Now;
                model.ZipName = zipFile.fileName;
                model.FilePath = zipFile.fullPath;
                int zipFileId = bZipFile.Add(model);
                if (zipFileId <= 0)
                {
                    //压缩包入库失败 重新加入队列 （一般应该为数据库连接有问题 ）
                    LogHelper.WriteLog("  压缩包【" + zipFile.fullPath + "】插入数据库库失败：");
                    zipFile.State = NewFile.ZipFileState.InsertDbFailed;
                    lock (_queueFile) {
                        _queueFile.Enqueue(zipFile);
                    }
                    return;
                }
                zipFile.State = NewFile.ZipFileState.InsertDbSucceed;
                zipFile.FileID = zipFileId;
            }
            //Thread.Sleep(300);
            //压缩包解压 解压目标文件夹与压缩包名称相同
            string dirName = zipFile.fileName.Remove(zipFile.fileName.LastIndexOf("."));
            string unzipDir = _unzipRootDir + (_unzipRootDir.EndsWith("\\")?"":"\\") + dirName;
            bool compress = CompressHelper.Decompress(zipFile.fullPath, unzipDir);
            if (compress)
            {
                UnzipedSucceedCount++;
                if (CountChangedListener != null)
                {
                    CountChangedListener.NumberChanged();
                }
                //将列表中的改文件对象State改为已解压 
                zipFile.State = NewFile.ZipFileState.UnzipSucceed;
                //解压成功后 将图片转为订单入库
                float fileTotalSize = 0;
                int fileCount = 0;
                int succeedCount = 0;
                DirectoryInfo dir = new DirectoryInfo(unzipDir);
                foreach (DirectoryInfo dirInfo in dir.GetDirectories())
                {
                    fileCount += dirInfo.GetFiles("*.jpg").Length;
                    succeedCount += readPhotoInDirToOrder(zipFile.FileID, dirInfo, ref fileTotalSize);
                }
                fileCount += dir.GetFiles("*.jpg").Length;
                succeedCount += readPhotoInDirToOrder(zipFile.FileID, dir, ref fileTotalSize);

                if (CountChangedListener != null)
                {
                    CountChangedListener.NumberChanged();
                }

                //订单入库完毕后更新zipfile
                Model.Model_Zipfile model = new Model.Model_Zipfile();
                model.ID = zipFile.FileID;
                model.Unziped = 1;
                model.UnzipTime = DateTime.Now;
                model.FileCount = (short)fileCount;
                model.FileSize = fileTotalSize;
                model.State = 1;
                bool unzipUpdate = bZipFile.UnzipedOver(model);
                if (!unzipUpdate)
                {
                    //=========压缩包解压状态更新失败========= 暂时不做操作 
                    LogHelper.WriteLog("  压缩包【" + zipFile.fullPath + "】解压状态更新失败：");
                }
                //删除压缩包
                try
                { 
                    if (CountChangedListener != null)
                    {
                        CountChangedListener.NumberChanged();
                    }
                    File.Delete(zipFile.fullPath);
                    File.Delete(zipFile.fullPath.Replace("zip", "flag"));
                    _newFileList.Remove(zipFile.fileName);
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog("  压缩包【" + zipFile.fullPath + "】删除失败:" + ex.Message);
                }
            }
            else
            {
                LogHelper.WriteLog("  压缩包【" + zipFile.fullPath + "】解压失败：");
                // 解压失败 尝试递归该方法
                zipFile.State = NewFile.ZipFileState.UnzipFailed;
                zipFile.UnzipFaildCount++;
                //解压失败重试 总次数不超过3次 超出则从列表中移除 不再处理 
                if (zipFile.UnzipFaildCount < 4)
                {
                    _queueFile.Enqueue(zipFile);
                }
                else
                {
                    //失败数量+1
                   // _newFileList.Remove(zipFile.fileName);
                } 
            }

            //NewFile existFile = _newFileList.Find(file => file.fileName == fileName);
            //if (existFile == null) {
            //    NewFile newFile = new NewFile(fileName,fullName);
            //    _newFileList.Add(newFile);
            //    ZipFileTotalCount++;
            //    if (CountChangedListener != null) {
            //        CountChangedListener.NumberChanged();
            //    }
            //} 
            //else {//已触发过一次changed事件的文件 进行入库操作
            //    existFile.changedCount++;
            //    if (existFile.changedCount == 2) {
            //        Thread thread = new Thread(HandleZipFile);
            //        thread.Start(existFile);
            //    } 
            //}
        }
 










        //
        private int readPhotoInDirToOrder(int zipFileId,DirectoryInfo dirInfo,ref float fileSize)
        {
            string dirName = dirInfo.FullName.Replace(_unzipRootDir, "").Replace("\\","/");

            int succeed = 0;
            List<Model_Photo> list = new List<Model_Photo>(); 
            foreach (FileInfo file in dirInfo.GetFiles("*.jpg"))
            {
                Model_Photo photo = new Model_Photo();
                photo.ZipFileID = zipFileId; 
                photo.FileName = file.Name; 
                photo.FilePath = file.FullName;
                photo.FileSize = file.Length/1024;
                photo.CreateTime = DateTime.Now;
                photo.WebUrl = dirName+"/"+file.Name;
                list.Add(photo);
                fileSize += photo.FileSize;
            }
            BLL_Photo bPhoto = new BLL_Photo();
            int orderCount = 0;
            succeed += bPhoto.AddList(list,out orderCount);
            //更新解压图片数量 及订单入库数量
            UnzipedPhotoCount += list.Count;
            CreatedOrderCount += orderCount; 

            return succeed;
        }
         
       // public enum ChangedType { ZipFileCount,UnzipedCount,WaitforUnzip,UnzipedPhoto,CreateOrderCount }
    } 
}