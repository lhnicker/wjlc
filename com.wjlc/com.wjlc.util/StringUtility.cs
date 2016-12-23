using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Web;

namespace com.wjlc.util
{
    public class StringUtility
    {
        /// <summary>
        /// Lucene.Net 关键词替换
        /// </summary>
        /// <param name="instr"></param>
        /// <returns></returns>
        public static string LuceneReplace(string instr)
        {
            StringBuilder tempStr = new StringBuilder(instr);
            tempStr.Replace("+", "");
            tempStr.Replace("-", "");
            tempStr.Replace("(", "");
            tempStr.Replace(")", "");
            tempStr.Replace("*", "");
            tempStr.Replace("&", "");
            tempStr.Replace("@", "");
            tempStr.Replace("!", "");
            tempStr.Replace("~", "");
            tempStr.Replace("[", "");
            tempStr.Replace("]", "");
            tempStr.Replace("{", "");
            tempStr.Replace("}", "");
            tempStr.Replace("“", "");
            tempStr.Replace("”", "");
            tempStr.Replace("QUOTED", "");
            tempStr.Replace("TERM", "");
            tempStr.Replace("NOT", "");
            tempStr.Replace("PREFIXTERM", "");
            tempStr.Replace("WILDTERM", "");
            tempStr.Replace("NUMBER", "");
            return tempStr.ToString();
        }

        /// <summary>
        /// 替换掉字符串中的 ',",\,<,>
        /// </summary>
        /// <param name="instr">输入的字符串</param>
        /// <returns></returns>
        public static string HtmlReplaceLimitedChar(string instr)
        {
            StringBuilder tempStr = new StringBuilder(instr);
            tempStr.Replace("'", "&#39;");
            tempStr.Replace("\"", "&#34;");
            tempStr.Replace("\\", "&#92;");
            tempStr.Replace("<", "&lt;");
            tempStr.Replace(">", "&gt;");
            tempStr.Replace("&", "&amp;");
            return tempStr.ToString();
        }
        public static string Substring(string str, int num)
        {
            string result = "";
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            if (str.Length > num)
            {
                result = str.Substring(0, num) + "...";

            }
            else
            {
                result = str;
            }
            return result;
        }

        /// <summary>
        /// 格式化数据为JSON
        /// </summary>
        /// <param name="sIn"></param>
        /// <returns></returns>
        public static string SafeJSON(string sIn)
        {
            StringBuilder sbOut = new StringBuilder(sIn.Length);
            foreach (char ch in sIn)
            {
                if (Char.IsControl(ch) || ch == '\'')
                {
                    int ich = (int)ch;
                    sbOut.Append(@"\u" + ich.ToString("x4"));
                    continue;
                }
                else if (ch == '\"' || ch == '\\' || ch == '/')
                {
                    sbOut.Append('\\');
                }
                sbOut.Append(ch);
            }
            return sbOut.ToString();
        }

        /// <summary>
        /// 剔除输入
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string FilterScript(string content)
        {
            string regexstr = @"<script[^>]*>([\s\S](?!<script))*?</script>";
            return Regex.Replace(content, regexstr, string.Empty, RegexOptions.IgnoreCase);
        }
        public static string FilterStyle(string content)
        {
            string regexstr = @"<style[^>]*>([\s\S](?!<style))*?</style>";
            return Regex.Replace(content, regexstr, string.Empty, RegexOptions.IgnoreCase);
        }
        public static string FilterLink(string content)
        {
            string regexstr = @"<link[^>]*>([\s\S](?!<link))*?</link>";
            return Regex.Replace(content, regexstr, string.Empty, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 验证输入的字符串是否为邮件格式
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool ValidEmailFormat(string instr)
        {
            string regstr = @"^[\w\.-]+@([\w-]+\.)+[a-zA-Z]{2,4}$";
            return Regex.IsMatch(instr, regstr, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 验证输入的字符串是否为手机号
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool ValidMobileFormat(string instr)
        {
            string regstr = @"^1\d{10}$";
            return Regex.IsMatch(instr, regstr, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 验证输入的字符串是否为手机号
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool ValidPostalCodeFormat(string instr)
        {
            string regstr = @"^\d{6}$";
            return Regex.IsMatch(instr, regstr, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 验证输入的字符串是否为电话格式，区号-号码-分机
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool ValidPhoneFormat(string instr)
        {
            string regstr = @"^\d{3,5}-\d{7,8}(-\d{1,5})?$";
            return Regex.IsMatch(instr, regstr, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 检查是不是中文
        /// </summary>
        /// <param name="instr">中文字符串</param>
        /// <returns></returns>
        public static bool ValidChinaese(string instr)
        {
            string regstr = @"^[\u4E00-\u9FA5\uF900-\uFA2D]+$";
            return Regex.IsMatch(instr, regstr, RegexOptions.None);
        }

        /// <summary>
        /// 检查有没有 数字字母下划线,别的返回false
        /// </summary>
        /// <param name="instr">验证字符串</param>
        /// <returns></returns>
        public static bool ValidNumber(string instr)
        {
            string regstr = @"^[0-9a-zA-Z_]+$";
            return Regex.IsMatch(instr, regstr, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 比较md5签名
        /// </summary>
        /// <param name="inputstr"></param>
        /// <param name="signstr"></param>
        /// <returns></returns>
        public static bool ValidateMd5Sign(string inputstr, string signstr)
        {
            byte[] data = System.Text.Encoding.Default.GetBytes(inputstr);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(data);
            string resultCode = System.BitConverter.ToString(result).Replace("-", "").ToLower();
            return (signstr.ToLower() == resultCode);
        }

        /// <summary>
        /// 获得输入字符串的md5签名，去除“-”，并转为小写格式
        /// </summary>
        /// <param name="inputstr"></param>
        /// <returns></returns>
        public static string GetMd5Sign(string inputstr)
        {
            byte[] data = System.Text.Encoding.Default.GetBytes(inputstr);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(data);
            string resultCode = System.BitConverter.ToString(result).Replace("-", "").ToLower();
            return resultCode;
        }

        /// <summary>
        /// 比较md5签名
        /// </summary>
        /// <param name="inputstr"></param>
        /// <param name="signstr"></param>
        /// <returns></returns>
        public static bool ValidateMd5Sign(string inputstr, string signstr, Encoding encode)
        {
            byte[] data = encode.GetBytes(inputstr);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(data);
            string resultCode = System.BitConverter.ToString(result).Replace("-", "").ToLower();
            return (signstr.ToLower() == resultCode);
        }

        /// <summary>
        /// 获得输入字符串的md5签名，去除“-”，并转为小写格式
        /// </summary>
        /// <param name="inputstr"></param>
        /// <returns></returns>
        public static string GetMd5Sign(string inputstr, Encoding encode)
        {
            byte[] data = encode.GetBytes(inputstr);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(data);
            string resultCode = System.BitConverter.ToString(result).Replace("-", "").ToLower();
            return resultCode;
        }

        public static string RemoveHtmlTags(string HTMLStr)
        {
            return System.Text.RegularExpressions.Regex.Replace(HTMLStr, "<[^>]*>", "");
        }
        /// <summary>
        /// 字符串转换为 Guid数组
        /// </summary>
        /// <param name="guidStrs">字符串</param>
        /// <param name="separator">分隔符</param>
        /// <returns></returns>
        public static Guid[] ConvertStringToGuids(string guidStrs, char separator)
        {
            if (guidStrs == null || guidStrs == "") throw new Exception();
            string[] strs = guidStrs.Split(separator);
            Guid[] ids = new Guid[strs.Length];
            for (int i = 0; i < strs.Length; i++)
            {
                ids[i] = new Guid(strs[i]);
            }
            return ids;
        }

        public static string ConstructFileName(string fileName)
        {
            string timeSpan = "" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + DateTime.Now.Millisecond;
            timeSpan = timeSpan + "_" + Guid.NewGuid().ToString().Replace("-", "");
            int index = fileName.LastIndexOf('.');
            return fileName.Substring(0, index) + "_" + timeSpan + fileName.Substring(index);
        }

        public static string ConstructFileName()
        {
            string timeSpan = "" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + DateTime.Now.Millisecond;
            timeSpan = timeSpan + "_" + Guid.NewGuid().ToString().Replace("-", "");
            return timeSpan;
        }
        public static string ConstructNO()
        {
            string timeSpan = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            return timeSpan;
        }
        public static string GetDeffNo()
        {
            string GuidStr = Guid.NewGuid().ToString();
            string resultStr = GuidStr.Split('-')[0];
            return resultStr.ToUpper();

        }
        public static string GetDefaultPad()
        {
            return Guid.NewGuid().ToString().Split('-')[4].ToUpper();
        }
        /// <summary>
        /// 得到特殊值
        /// </summary>
        /// <param name="keyName"></param>
        /// <returns></returns>
        public static string CparaValue(string OrgStr, string keyName)
        {
            string res = "";
            if (OrgStr != "")
            {
                string[] strList = OrgStr.Split('|');
                for (int i = 0; i < strList.Length; i++)
                {
                    string tempStr = strList[i];
                    string tempKey = tempStr.Substring(tempStr.IndexOf('~') + 1, (tempStr.LastIndexOf('~') - tempStr.IndexOf('~')) - 1);
                    if (tempKey.Equals(keyName))
                    {
                        res = tempStr.Substring(tempStr.LastIndexOf('~') + 1, (tempStr.Length - 1) - tempStr.LastIndexOf('~'));
                        break;
                    }
                }
                return res;
            }
            else
            {
                return "";
            }
        }
        /// <summary>
        /// 判断某个字符串是不是数字型的
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static bool isNumeric(string num)
        {
            if (string.IsNullOrEmpty(num))
                return false;
            System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex("^[0-9]+$");
            return r.IsMatch(num);
        }
        public static bool isNumeric(object isNum)
        {
            if (isNum == null)
                return false;
            return isNumeric(isNum.ToString());
        }
        /// <summary>
        /// 去掉小数点的位数0
        /// </summary>
        /// <param name="moneyStr"></param>
        /// <returns></returns>
        public static string decimalOut0(string moneyStr)
        {
            return moneyStr.TrimEnd('0').TrimEnd('.');
        }
        public static string ConvertToZheKou(decimal MarketPrice, decimal TradePrice)
        {
            if (MarketPrice - TradePrice == 0) //不打折
            {
                return "0";
            }
            if (TradePrice == 0)
            {
                return "0";
            }
            return StringUtility.decimalOut0((Math.Round(TradePrice / MarketPrice, 2) * 10).ToString());


        }

        /// <summary>
        /// 检测含有中文字符串的实际长度
        /// </summary>
        /// <param name=”str”>字符串</param>
        public static int len(string str)
        {
            ASCIIEncoding n = new System.Text.ASCIIEncoding();
            byte[] b = n.GetBytes(str);
            int l = 0; // l 为字符串之实际长度
            for (int i = 0; i <= b.Length - 1; i++)
            {
                if (b[i] == 63) //判断是否为汉字或全脚符号
                {
                    l++;
                }
                l++;
            }
            return l;
        }

        /// <summary>
        /// 去除HTML标记
        /// </summary>
        /// <param name="htmlString">包括HTML的源码 </param>
        /// <returns>已经去除后的文字</returns>
        public static string NoHTML(string htmlString)
        {
            //删除脚本
            htmlString = Regex.Replace(htmlString, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML
            htmlString = Regex.Replace(htmlString, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            htmlString = Regex.Replace(htmlString, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            htmlString = Regex.Replace(htmlString, @"-->", "", RegexOptions.IgnoreCase);
            htmlString = Regex.Replace(htmlString, @"<!--.*", "", RegexOptions.IgnoreCase);
            htmlString = Regex.Replace(htmlString, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            htmlString = Regex.Replace(htmlString, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            htmlString = Regex.Replace(htmlString, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            htmlString = Regex.Replace(htmlString, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            htmlString = Regex.Replace(htmlString, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            htmlString = Regex.Replace(htmlString, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            htmlString = Regex.Replace(htmlString, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            htmlString = Regex.Replace(htmlString, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            htmlString = Regex.Replace(htmlString, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            htmlString = Regex.Replace(htmlString, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            htmlString.Replace("<", "");
            htmlString.Replace(">", "");
            htmlString.Replace("\r\n", "");
            htmlString = HttpContext.Current.Server.HtmlEncode(htmlString).Trim();
            return htmlString;
        }

        /// <summary>
        /// 根据正则表达式来获取字符串
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetPattern(string name, string str)
        {
            string regex = string.Format("【{0}】([^【]+)", name);
            string result = Regex.Match(str, regex).Groups[1].Value;
            return StringUtility.NoHTML(result);
        }
        /// <summary>
        /// 显示拼接后的商品名
        /// </summary>
        /// <param name="name">正则中需要</param>
        /// <param name="description">商品描述Description</param>
        /// <param name="chineseName">商品名称</param>
        /// <returns></returns>
        public static string GetProductName(string name, string description, string chineseName, string brandname)
        {
            try
            {
                string commonName = string.Empty;
                if (!string.IsNullOrEmpty(description))
                {
                    commonName = GetPattern(name, description);
                }
                string sname = string.Empty;
                if (!chineseName.Contains(commonName) && !commonName.Contains(chineseName))
                {
                    sname = "(" + commonName + ")";
                }
                string bname = string.Empty;
                if (!string.IsNullOrEmpty(brandname))
                {
                    if (!chineseName.Contains(brandname))
                    {
                        bname = brandname + " ";
                    }
                }
                string productName = string.Format("{0}{1}{2}", bname, chineseName, sname);
                return productName;
            }
            catch
            {
                return chineseName;
            }
        }

        /// <summary>
        /// 显示MetaKeywords
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="chineseName"></param>
        /// <param name="brandname"></param>
        /// <returns></returns>
        public static string GetMetaKeywords(string name, string description, string chineseName, string brandname)
        {
            try
            {
                string commonName = GetPattern(name, description);
                string strname = string.Empty;
                if (!string.IsNullOrEmpty(commonName))
                {
                    if (!commonName.Contains(chineseName) && !chineseName.Contains(commonName))
                    {
                        strname = string.Format(",{0}价格,{0}说明书,{0}作用,{0}不良反应", commonName);
                    }
                }
                string metaKeywords = string.Format("{2}{0}价格,{0}说明书,{0}作用,{0}不良反应{1}", commonName, strname, string.IsNullOrEmpty(brandname) ? string.Empty : brandname + ",");
                return metaKeywords;
            }
            catch
            {
                return "";
            }
        }
    }

}
