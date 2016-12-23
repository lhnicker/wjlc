using System;
using System.Text;
using System.Net;
using System.IO;
using System.Web;
using System.Collections;

namespace com.wjlc.util
{
    public class FileHelper
    {

        private const int _bufferSize = 1024;

        public static void GetWebPageAndWriteToFile(string sourceWebPath, string destFilePath)
        {
            Uri uri = new Uri(sourceWebPath);
            WebRequest wr = WebRequest.Create(uri);
            using (Stream s = wr.GetResponse().GetResponseStream())
            {
                using (StreamReader sreader = new StreamReader(s, Encoding.Default))
                {
                    using (StreamWriter writer = new StreamWriter(destFilePath, false, Encoding.Default))
                    {
                        string endHtml = FilterSpecialContent(sreader.ReadToEnd());
                        writer.Write(endHtml);
                        writer.Close();
                    }
                    sreader.Close();
                }
                s.Close();
            }

        }

        public static string GetContentFromUrl(string _requestUrl)
        {
            string _StrResponse = "";

            HttpWebRequest _WebRequest = (HttpWebRequest)WebRequest.Create(_requestUrl);
            _WebRequest.ContentType = "utf-8";
            _WebRequest.Method = "GET";
            WebResponse _WebResponse = _WebRequest.GetResponse();
            StreamReader _ResponseStream = new StreamReader(_WebResponse.GetResponseStream(), System.Text.Encoding.GetEncoding("utf-8"));
            _StrResponse = _ResponseStream.ReadToEnd();
            _StrResponse = _StrResponse.Replace("\n", "").Replace("\r", "").Trim();

            _WebResponse.Close();
            _ResponseStream.Close();
            return _StrResponse;
        }



        public static string GetContentFromUrlWithCookie(string _requestUrl, HttpContext context, string cookieAllName)
        {
            string _StrResponse = "";

            HttpWebRequest _WebRequest = (HttpWebRequest)WebRequest.Create(_requestUrl);
            _WebRequest.ContentType = "utf-8";
            _WebRequest.Method = "GET";

            CookieContainer cookieContainer = new CookieContainer();

            foreach (string cookieName in context.Request.Cookies)
            {
                if (cookieAllName.IndexOf(cookieName + ",") > 0)
                {
                    HttpCookie cookie = context.Request.Cookies[cookieName];
                    cookieContainer.Add(new Cookie(cookie.Name, cookie.Value.Replace(",", " "), cookie.Path, "jxdyf.com"));
                }
            }

            _WebRequest.CookieContainer = cookieContainer;

            WebResponse _WebResponse = _WebRequest.GetResponse();
            StreamReader _ResponseStream = new StreamReader(_WebResponse.GetResponseStream(), System.Text.Encoding.GetEncoding("utf-8"));
            _StrResponse = _ResponseStream.ReadToEnd();
            _StrResponse = _StrResponse.Replace("\n", "").Replace("\r", "").Trim();

            _WebResponse.Close();
            _ResponseStream.Close();
            return _StrResponse;
        }

        private static string FilterSpecialContent(string originContent)
        {
            return originContent;
        }

        /// <summary>
        /// 从图片地址下载图片到本地磁盘
        /// </summary>
        /// <param name="ToLocalPath">图片本地磁盘地址</param>
        /// <param name="Url">图片网址</param>
        /// <returns></returns>
        public static bool GetWebFileAndWriteToLocal(string Url, string FileName)
        {
            bool Value = false;
            WebResponse response = null;
            Stream stream = null;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);

                response = request.GetResponse();
                stream = response.GetResponseStream();

                if (!response.ContentType.ToLower().StartsWith("text/"))
                {
                    Value = SaveBinaryFile(response, FileName);
                }

            }
            catch (Exception err)
            {
                string aa = err.ToString();
            }
            return Value;
        }
        /// <summary>
        /// Save a binary file to disk.
        /// </summary>
        /// <param name="response">The response used to save the file</param>
        // 将二进制文件保存到磁盘
        private static bool SaveBinaryFile(WebResponse response, string FileName)
        {
            bool Value = true;
            byte[] buffer = new byte[1024];

            try
            {
                if (File.Exists(FileName))
                    File.Delete(FileName);
                Stream outStream = System.IO.File.Create(FileName);
                Stream inStream = response.GetResponseStream();

                int l;
                do
                {
                    l = inStream.Read(buffer, 0, buffer.Length);
                    if (l > 0)
                        outStream.Write(buffer, 0, l);
                }
                while (l > 0);

                outStream.Close();
                inStream.Close();
            }
            catch
            {
                Value = false;
            }
            return Value;
        }

        public static string GetContentFromUrlGB(string _requestUrl)
        {
            string _StrResponse = "";
            HttpWebRequest _WebRequest = (HttpWebRequest)WebRequest.Create(_requestUrl);
            _WebRequest.Method = "GET";
            WebResponse _WebResponse = _WebRequest.GetResponse();
            StreamReader _ResponseStream = new StreamReader(_WebResponse.GetResponseStream(), System.Text.Encoding.GetEncoding("gb2312"));
            _StrResponse = _ResponseStream.ReadToEnd();
            _WebResponse.Close();
            _ResponseStream.Close();
            return _StrResponse;
        }

        public static string GetContentFromUrlUTF(string _requestUrl)
        {
            string _StrResponse = "";
            HttpWebRequest _WebRequest = (HttpWebRequest)WebRequest.Create(_requestUrl);
            _WebRequest.Method = "GET";
            WebResponse _WebResponse = _WebRequest.GetResponse();
            StreamReader _ResponseStream = new StreamReader(_WebResponse.GetResponseStream(), System.Text.Encoding.GetEncoding("utf-8"));
            _StrResponse = _ResponseStream.ReadToEnd();
            _WebResponse.Close();
            _ResponseStream.Close();
            return _StrResponse;
        }

        /// <summary>
        /// 获取绝对路径文件的内容。
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetFileContent(string filePath)
        {
            string fileContent = "";
            using (StreamReader reader = new StreamReader(filePath, Encoding.Default))
            {
                fileContent = reader.ReadToEnd();
                reader.Close();
            }
            return fileContent;
        }
        public static void SaveContentToFile(string filePath, string fileContent)
        {
            string directoryName = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }
            using (StreamWriter sw = new StreamWriter(filePath, false, Encoding.GetEncoding("utf-8")))
            {
                sw.Write(fileContent);
                sw.Close();
            }
        }

        /// <summary>
        /// 将流写入到文件中。
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="inputStream"></param>
        public static void CreateFileFromStream(string filePath, Stream inputStream)
        {
            if (File.Exists(filePath))
            {
                throw new ArgumentException("The file already exists", filePath);
            }
            string directoryName = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryName))
            {
                throw new DirectoryNotFoundException(String.Format("The physical upload directory {0} for the file does not exist", directoryName));
            }
            // string tempFilePath = Path.GetFileName(filePath);

            FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            Copy(inputStream, fs);
            fs.Flush();
            fs.Close();

            //this._createdFiles.Add(new string[2] { filePath, tempFilePath });
        }

        public static Stream ReadStreamFromFile(string filePath)
        {
            try
            {
                FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                return fs;
            }
            catch (FileNotFoundException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void Copy(Stream fromStream, Stream toStream)
        {
            fromStream.Position = 0;
            toStream.Position = 0;

            byte[] buffer = new byte[_bufferSize];
            int len;
            while ((len = fromStream.Read(buffer, 0, _bufferSize)) > 0)
            {
                toStream.Write(buffer, 0, len);
            }
        }
        public static void DeleteFile(string fileName)
        {
            try
            {
                File.Delete(fileName);
            }
            catch
            {
            }
        }


        public static bool EnsureFilePathCreated(string fileFullPath)
        {
            try
            {
                string directoryName = Path.GetDirectoryName(fileFullPath);
                if (!Directory.Exists(directoryName))
                    Directory.CreateDirectory(directoryName);

                return true;
            }
            catch { return false; }
        }

        ///for download 
        ///
        private static Hashtable fileTypesMap;
        public static string GetIconFilename(string extension)
        {
            if (fileTypesMap == null)
            {
                InitMap();
            }
            string iconFilename = "file.gif"; // default
            if (fileTypesMap[extension] != null)
            {
                iconFilename = fileTypesMap[extension].ToString();
            }
            return iconFilename;
        }

        private static void InitMap()
        {
            fileTypesMap = new Hashtable();
            fileTypesMap.Add(".asf", "mpg.gif");
            fileTypesMap.Add(".avi", "mpg.gif");
            fileTypesMap.Add(".bmp", "bmp.gif");
            fileTypesMap.Add(".chm", "chm.gif");
            fileTypesMap.Add(".cs", "cs.gif");
            fileTypesMap.Add(".doc", "doc.gif");
            fileTypesMap.Add(".dot", "doc.gif");
            fileTypesMap.Add(".exe", "exe.gif");
            fileTypesMap.Add(".gif", "gif.gif");
            fileTypesMap.Add(".gz", "zip.gif");
            fileTypesMap.Add(".gzip", "zip.gif");
            fileTypesMap.Add(".htm", "htm.gif");
            fileTypesMap.Add(".html", "html.gif");
            fileTypesMap.Add(".jpg", "jpg.gif");
            fileTypesMap.Add(".jpeg", "jpg.gif");
            fileTypesMap.Add(".mdb", "mdb.gif");
            fileTypesMap.Add(".mov", "mpg.gif");
            fileTypesMap.Add(".mp3", "wav.gif");
            fileTypesMap.Add(".mpg", "mpg.gif");
            fileTypesMap.Add(".mpeg", "mpg.gif");
            fileTypesMap.Add(".pdf", "pdf.gif");
            fileTypesMap.Add(".ppt", "ppt.gif");
            fileTypesMap.Add(".rar", "zip.gif");
            fileTypesMap.Add(".rtf", "doc.gif");
            fileTypesMap.Add(".tgz", "zip.gif");
            fileTypesMap.Add(".txt", "txt.gif");
            fileTypesMap.Add(".wav", "wav.gif");
            fileTypesMap.Add(".wma", "wav.gif");
            fileTypesMap.Add(".xls", "xls.gif");
            fileTypesMap.Add(".xml", "xml.gif");
            fileTypesMap.Add(".zip", "zip.gif");
        }
    }
}
