using System;
using System.Diagnostics;

namespace com.wjlc.util
{
    /// <summary>
    /// 同步文件助手
    /// </summary>
    public class RsyncHelper
    {
        private string rsyncExe;
        private string rootPath;
        private string dest;
        
        public string RsyncExe
        {
            get { return rsyncExe; }
            set { rsyncExe = value; }
        }

        /// <summary>
        /// 分发根目录
        /// </summary>
        public string RootPath
        {
            get { return rootPath; }
            set { rootPath = value; }
        }

        /// <summary>
        /// 多个地址用;(分号)分开。
        /// </summary>
        public string Dest
        {
            get { return dest; }
            set { dest = value; }
        }

        /// <summary>
        /// 分发单个文件
        /// </summary>
        /// <param name="fileName"></param>
        public void RsyncSingleFile(string fileName)
        {
            string[] sDesc = Dest.Split(new char[] { ';', '；' });
            if (fileName != String.Empty)
            {
                for (int i = 0; i < sDesc.Length; i++)
                {
                    if (sDesc[i].Trim() != String.Empty)
                    {
                        RsyncFile(fileName, sDesc[i]);
                    }
                }
            }
        }
      
        /// <summary>
        /// 分发文件夹及其包含的所有子文件夹的文件。
        /// </summary>
        /// <param name="folderName">文件夹名字</param>
        public void RsyncFolderFiles(string folderName)
        {
            string[] sDesc = Dest.Split(new char[] { ';', '；' });
            for (int i = 0; i < sDesc.Length; i++)
            {
                if (sDesc[i].Trim() != String.Empty)
                {
                    if (folderName != String.Empty)
                    {
                        RsyncFolder(folderName, sDesc[i]);
                    }
                    else
                    {
                        RsyncFolder(RootPath, sDesc[i]);
                    }
                }
            }
        }
       
        /// <summary>
        /// 具体分发程序
        /// </summary>
        /// <param name="src">分发源目录或者文件名，例如“test”或“1.txt”</param>
        /// <param name="desc">分发目标例如“127.0.0.1::test”</param>
        private void RsyncFolder(string src, string desc)
        {
            System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo();
            psi.WorkingDirectory = RootPath;    // 设置为你要cd进入的路径
            psi.FileName = "cmd.exe";
            psi.Arguments = "/C \"" + RsyncExe + "\" -art -R --delete --force --ignore-errors " + src + " " + desc;
            System.Diagnostics.Process procss = System.Diagnostics.Process.Start(psi);
            procss.Close();
        }

        /// <summary>
        /// 分发文件
        /// </summary>
        /// <param name="src"></param>
        /// <param name="desc"></param>
        private void RsyncFile(string src, string desc)
        {
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.WorkingDirectory = RootPath;    // 设置为你要cd进入的路径
            psi.FileName = "cmd.exe";
            psi.Arguments = "/C \"" + RsyncExe + "\" -art " + src + " " + desc;
            Process procss = Process.Start(psi);
            procss.Close();
        }
    }
}
