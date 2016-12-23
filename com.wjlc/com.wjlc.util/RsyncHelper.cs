using System;
using System.Diagnostics;

namespace com.wjlc.util
{
    /// <summary>
    /// ͬ���ļ�����
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
        /// �ַ���Ŀ¼
        /// </summary>
        public string RootPath
        {
            get { return rootPath; }
            set { rootPath = value; }
        }

        /// <summary>
        /// �����ַ��;(�ֺ�)�ֿ���
        /// </summary>
        public string Dest
        {
            get { return dest; }
            set { dest = value; }
        }

        /// <summary>
        /// �ַ������ļ�
        /// </summary>
        /// <param name="fileName"></param>
        public void RsyncSingleFile(string fileName)
        {
            string[] sDesc = Dest.Split(new char[] { ';', '��' });
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
        /// �ַ��ļ��м���������������ļ��е��ļ���
        /// </summary>
        /// <param name="folderName">�ļ�������</param>
        public void RsyncFolderFiles(string folderName)
        {
            string[] sDesc = Dest.Split(new char[] { ';', '��' });
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
        /// ����ַ�����
        /// </summary>
        /// <param name="src">�ַ�ԴĿ¼�����ļ��������硰test����1.txt��</param>
        /// <param name="desc">�ַ�Ŀ�����硰127.0.0.1::test��</param>
        private void RsyncFolder(string src, string desc)
        {
            System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo();
            psi.WorkingDirectory = RootPath;    // ����Ϊ��Ҫcd�����·��
            psi.FileName = "cmd.exe";
            psi.Arguments = "/C \"" + RsyncExe + "\" -art -R --delete --force --ignore-errors " + src + " " + desc;
            System.Diagnostics.Process procss = System.Diagnostics.Process.Start(psi);
            procss.Close();
        }

        /// <summary>
        /// �ַ��ļ�
        /// </summary>
        /// <param name="src"></param>
        /// <param name="desc"></param>
        private void RsyncFile(string src, string desc)
        {
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.WorkingDirectory = RootPath;    // ����Ϊ��Ҫcd�����·��
            psi.FileName = "cmd.exe";
            psi.Arguments = "/C \"" + RsyncExe + "\" -art " + src + " " + desc;
            Process procss = Process.Start(psi);
            procss.Close();
        }
    }
}
