using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EADS.Common
{
    public class DatabaseBackupHelper
    {
        public static void StartCmd(String workingDirectory, String command)
        {
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.WorkingDirectory = workingDirectory;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.StandardInput.WriteLine(command);
            p.StandardInput.WriteLine("exit");
        }

        public static bool BackupDb(string savePath)
        {
            try {
                string command = "mysqldump --host=localhost --port=13306 -uroot -p206366   expressaddress    > " + savePath;
                string dirPath = savePath.Remove(savePath.LastIndexOf('\\'));
                StartCmd(dirPath, command);
               // Common.CompressHelper.CompressZipFile(dirPath,)
                return true;
            }
            catch (Exception ex) {
                return false;
            }
          
        }
         
    }
}
