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
        private string _unzipRootDir="";//压缩文件解压到的文件夹
         
        public class NewFile {
            public NewFile(string name,string full) {
                changedCount = 1;
                State = ZipFileState.New;
                fileName = name;
                fullPath = full;
            }
            public int changedCount = 0;
            public string fileName;
            public string fullPath;  
            public ZipFileState State ;
            public int FileID;
            public int UnzipFaildCount = 0;

            //0-尚未解压 1-入库成功 2-入库失败 3-解压成功 4-解压失败
            public enum ZipFileState { New,InsertDbSucceed,InsertDbFailed,UnzipSucceed,UnzipFailed};
        }

        private List<NewFile> _newFileList;//新收zip文件列表
        public int ZipFileTotalCount;//接收zip文件总数量
        public int UnzipedSucceedCount;//解压成功数量
       // public int UnzipedFailedCount; //解压失败数量
        public int UnzipedPhotoCount;//解压出的图片数量
        public int CreatedOrderCount; //订单生成数量

        public NumChangedListener CountChangedListener;


        public FileHandler() {
            _newFileList = new List<NewFile>();
            IniFile iniFiles = new IniFile(CommonConst.GetIniFilePath());
            _unzipRootDir = iniFiles.ReadString("FILE", "UnzipPhotoToDir", "E:\\EADS\\Res\\Photo\\"); 
        }
        
        

        //接收到新的文件改变事件
        public void handleFileChanged(string fileName, string fullName) {

            fileName = fileName.Replace("flag", "zip");
            fullName = fullName.Replace("flag", "zip");
            NewFile existFile = _newFileList.Find(file => file.fileName == fileName);
            if (existFile == null)
            {
                //第一次触发flag文件的changed事件就直接解压
                NewFile newFile = new NewFile(fileName, fullName);
                _newFileList.Add(newFile);
                ZipFileTotalCount++;
                if (CountChangedListener != null)
                {
                    CountChangedListener.NumberChanged();
                }
                Thread thread = new Thread(HandleZipFile);
                thread.Start(newFile);
            }
            else
            {//已触发过一次changed事件的文件 
                existFile.changedCount++; 
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

        private void HandleZipFile(object obj) {
            NewFile newFile = (NewFile)obj;
            BLL_Zipfile bZipFile = new BLL.BLL_Zipfile();
            if (!File.Exists(newFile.fullPath)) {
                //文件已被删  则从列表中清除
                _newFileList.Remove(newFile);
                if (CountChangedListener != null)
                {
                    CountChangedListener.NumberChanged();
                }
                return;
            }
            //待解压新包 或插入数据库失败的包 重新入库
            if (newFile.State==NewFile.ZipFileState.New || newFile.State==NewFile.ZipFileState.InsertDbFailed) {

                Model.Model_Zipfile model = new Model.Model_Zipfile();
                model.CreateTime = DateTime.Now;
                model.ZipName = newFile.fileName;
                model.FilePath = newFile.fullPath;
                int zipFileId = bZipFile.Add(model);
                if (zipFileId <= 0)
                {
                    //压缩包入库失败 退出不再尝试 （数据库有问题）
                    LogHelper.WriteLog("  压缩包【" + newFile.fullPath + "】插入数据库库失败：");
                    newFile.State = NewFile.ZipFileState.InsertDbFailed;
                    return;
                }
                newFile.State = NewFile.ZipFileState.InsertDbSucceed; 
                newFile.FileID = zipFileId;
            } 
            Thread.Sleep(3000);
            //压缩包解压 解压目标文件夹与压缩包名称相同
            string dirName = newFile.fileName.Remove(newFile.fileName.LastIndexOf("."));
            string unzipDir = _unzipRootDir + "\\" + dirName; 
            bool compress = CompressHelper.Decompress(newFile.fullPath, unzipDir); 
            if (compress)
            {
                UnzipedSucceedCount++;
                if (CountChangedListener != null)
                {
                    CountChangedListener.NumberChanged();
                }
                //将列表中的改文件对象State改为已解压 
                newFile.State = NewFile.ZipFileState.UnzipSucceed;
                //解压成功后 将图片转为订单入库
                float fileTotalSize = 0;
                int fileCount = 0;
                int succeedCount = 0;
                DirectoryInfo dir = new DirectoryInfo(unzipDir);
                foreach (DirectoryInfo dirInfo in dir.GetDirectories())
                {
                    fileCount += dirInfo.GetFiles("*.jpg").Length;
                    succeedCount += readPhotoInDirToOrder(newFile.FileID, dirInfo, ref fileTotalSize); 
                }
                fileCount += dir.GetFiles("*.jpg").Length;
                succeedCount += readPhotoInDirToOrder(newFile.FileID, dir, ref fileTotalSize);
                 
                if (CountChangedListener != null)
                {
                    CountChangedListener.NumberChanged();
                }

                //订单入库完毕后更新zipfile
                Model.Model_Zipfile model = new Model.Model_Zipfile();
                model.ID = newFile.FileID;
                model.Unziped = 1;
                model.UnzipTime = DateTime.Now;
                model.FileCount = (short)fileCount;
                model.FileSize = fileTotalSize;
                model.State = 1;
                bool unzipUpdate = bZipFile.UnzipedOver(model);
                if (!unzipUpdate) {
                    //=========压缩包解压状态更新失败========= 暂时不做操作 
                    LogHelper.WriteLog("  压缩包【" + newFile.fullPath + "】解压状态更新失败："); 
                }
                //删除压缩包
                try
                {
                    _newFileList.Remove(newFile);
                    if (CountChangedListener != null)
                    {
                        CountChangedListener.NumberChanged();
                    }
                    File.Delete(newFile.fullPath);
                    File.Delete(newFile.fullPath.Replace("zip","flag"));
                }
                catch(Exception ex) {
                    LogHelper.WriteLog("  压缩包【" + newFile.fullPath + "】删除失败:"+ex.Message);
                } 
            }
            else
            { 
                LogHelper.WriteLog("  压缩包【" + newFile.fullPath + "】解压失败：");
                // 解压失败 尝试递归该方法
                newFile.State =  NewFile.ZipFileState.UnzipFailed;
                newFile.UnzipFaildCount++;
                //解压失败重试 总次数不超过3次 超出则从列表中移除 不再处理 
                if (newFile.UnzipFaildCount < 4)
                {
                    HandleZipFile(obj);
                }
                else {
                    _newFileList.Remove(newFile);
                }
              
            } 
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