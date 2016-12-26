using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
namespace EADS.Common
{
    public class CompressHelper
    {

        public static bool Decompress(string file,string toDir)
        {
            if (string.IsNullOrEmpty(file)) return false;
            if (!File.Exists(file)) return false;
            try
            {
                if (file.EndsWith(".zip"))
                {
                    DecompressZipFile(file, toDir);
                }
                else if (file.EndsWith(".rar"))
                {
                   // DecompressRarFile(file, toDir);
                }
                else
                {
                    //暂不支持其他格式 
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("解压文件【"+ file+"】失败："+ex.Message);
                return false;
            }
           
        }
         

       

        /// <summary>
        /// 解压ZIP文件到目标目录
        /// </summary>
        /// <param name="zipFilePath"></param>
        /// <param name="toDir"></param>
        public static void DecompressZipFile(string zipFilePath, string toDir)
        {
            ZipFile.ExtractToDirectory(zipFilePath, toDir);
        }

        /// <summary>
        /// 压缩目录为ZIP文件
        /// </summary>
        /// <param name="compressDir"></param>
        /// <param name="zipFilePath"></param>
        public static void CompressZipFile(string compressDir,string zipFilePath) {
            ZipFile.CreateFromDirectory(compressDir, zipFilePath);
        }
         
    }
}
