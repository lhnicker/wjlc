using System;
using System.IO;
using System.Collections.Generic;

namespace com.wjlc.util
{
    public class DirectoryFileHelper
    {
        /// <summary>
        /// 删除目录
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool DeleteFolder(string path)
        {
            try
            {
                Directory.Delete(path, true);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool DeleteFile(string path)
        {
            try
            {
                File.Delete(path);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 新建目录
        /// </summary>
        /// <param name="path"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool NewFolder(string path, out string msg)
        {
            msg = "";
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                    return true;
                }
                else
                {
                    msg = "文件夹已存在";
                    return false;
                }
            }
            catch(System.Exception e)
            {
                msg = e.Message.ToString();
                return false;
            }
        }

        public static List<FileSystemObject> GetList( string rootPath, string url , string directName)
        {
            List<FileSystemObject> list = new List<FileSystemObject>();

            if (directName != rootPath)
            {
                DirectoryInfo pd = new DirectoryInfo(directName).Parent;
                //list.Add(new FileSystemObject()
                //{
                //    FileName = "上级目录",
                //    FullName = pd.FullName,
                //    LastWriteTime = pd.LastWriteTime,
                //    Size = -1,
                //    Suffix = "none",
                //    Type = 3,
                //    URL = "--"
                //});
            }

            list.AddRange(GetDirectoryList(rootPath, url, directName));
            list.AddRange(GetFileList(rootPath, url, directName));
            return list;
        }

        /// <summary>
        /// 取得文件夹列表
        /// </summary>
        /// <param name="rootPath"></param>
        /// <param name="url"></param>
        /// <param name="directoryName"></param>
        /// <returns></returns>
        public static List<FileSystemObject> GetDirectoryList(string rootPath, string url, string directoryName)
        {
            List<FileSystemObject> list = new List<FileSystemObject>();

            foreach (string d in Directory.GetDirectories(rootPath + directoryName))
            {
                DirectoryInfo di = new DirectoryInfo(d);

                FileSystemObject fso = new FileSystemObject();

                fso.FileName = di.Name;
                fso.FullName = di.FullName;
                fso.Suffix = "folder";
                fso.URL = "--";
                fso.Size = -1;
                fso.LastWriteTime = di.LastWriteTime;
                fso.Type = 1;

                list.Add(fso);
            }

            list.Sort(delegate(FileSystemObject f1, FileSystemObject f2) { return Comparer<DateTime>.Default.Compare(f2.LastWriteTime, f1.LastWriteTime); });

            return list;

        }

        /// <summary>
        /// 取得文件列表
        /// </summary>
        /// <param name="rootPath"></param>
        /// <param name="url"></param>
        /// <param name="DirectoryName"></param>
        /// <returns></returns>
        public static List<FileSystemObject> GetFileList(string rootPath ,string url , string directoryName)
        {
            List<FileSystemObject> list = new List<FileSystemObject>();

            foreach (string f in Directory.GetFiles(rootPath + directoryName))
            {
                FileInfo fi = new FileInfo(f);

                FileSystemObject fso = new FileSystemObject();

                fso.FileName = fi.Name;
                fso.FullName = fi.FullName;
                fso.Suffix = Path.GetExtension(fi.FullName).Substring(1);
                fso.URL = url + fi.FullName.Replace(rootPath, String.Empty).Replace("\\", "/");
                fso.Size = Convert.ToDecimal((fi.Length / 1024.00).ToString("0.00"));
                fso.LastWriteTime = fi.LastWriteTime;
                fso.Type = 2;
                list.Add(fso);
            }

            list.Sort(delegate(FileSystemObject f1, FileSystemObject f2) { return Comparer<DateTime>.Default.Compare(f2.LastWriteTime, f1.LastWriteTime); });
            return list;
        }
    }

    /// <summary>
    /// 文件信息
    /// </summary>
    public class FileSystemObject
    {
        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName{   get;  set;  }

        /// <summary>
        /// 文件扩展名
        /// </summary>
        public string Suffix{   get;  set;  }
        
        /// <summary>
        /// 文件全名
        /// </summary>
        public string FullName{   get;  set;  }
        
        /// <summary>
        /// url路径
        /// </summary>
        public string URL{   get;  set;  }

        /// <summary>
        /// 文件大小
        /// </summary>
        public decimal Size{   get;  set;  }        

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime LastWriteTime{   get;  set;  }        

        /// <summary>
        /// 文件类型 file=1, folder=2
        /// </summary>
        public int Type{   get;  set;  }
       
    }
}
