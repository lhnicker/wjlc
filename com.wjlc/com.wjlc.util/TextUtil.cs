using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace com.wjlc.util
{
    /// <summary>
    /// 字符串操作类
    /// </summary>
    public class TextUtil
    {
        /// <summary>
        /// Convert string to dictory 
        /// </summary>
        /// <param name="sourceString">eg. {"123":"abc","234":"cde"}</param>
        /// <returns></returns>
        public static Dictionary<string, string> ConvertString2Directory(string sourceString)
        {
            string directoryStr = sourceString.Trim(new char[] { '{', '}' });
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (string group in directoryStr.Split(','))
                if (!dic.ContainsKey(group.Split(':')[0].Trim('"')))
                    dic.Add(group.Split(':')[0].Trim('"'), group.Split(':')[1].Trim('"'));
            return dic;
        }

        public static string SubChineseString(string stringToSub, int length)
        {
            if (Encoding.Default.GetByteCount(stringToSub) > length)
                length = length - 2;
            Regex regex = new Regex("[\u4e00-\u9fa5]+", RegexOptions.Compiled);
            char[] stringChar = stringToSub.ToCharArray();
            StringBuilder sb = new StringBuilder();
            int nLength = 0;

            for (int i = 0; i < stringChar.Length; i++)
            {
                if (regex.IsMatch((stringChar[i]).ToString()))
                {
                    sb.Append(stringChar[i]);
                    nLength += 2;
                }
                else
                {
                    sb.Append(stringChar[i]);
                    nLength = nLength + 1;
                }

                if (nLength > length)
                    break;
            }

            return sb.ToString();
        }

        /// <summary>
        /// 截取汉字、超过指定长度的string追加suspoint
        /// </summary>
        /// <param name="stringToSub">被操作的字符串</param>
        /// <param name="length">长度</param>
        /// <param name="suspoint">省略号等。</param>
        /// <returns></returns>
        public static string SubChineseStringWithsuspoint(string stringToSub, int length, string suspoint)
        {
            if (Encoding.Default.GetByteCount(stringToSub) > length)
                length = length - 2;
            Regex regex = new Regex("[\u4e00-\u9fa5]+", RegexOptions.Compiled);
            char[] stringChar = stringToSub.ToCharArray();
            StringBuilder sb = new StringBuilder();
            int nLength = 0;

            for (int i = 0; i < stringChar.Length; i++)
            {
                if (regex.IsMatch((stringChar[i]).ToString()))
                {
                    sb.Append(stringChar[i]);
                    nLength += 2;
                }
                else
                {
                    sb.Append(stringChar[i]);
                    nLength = nLength + 1;
                }

                if (nLength > length)
                    break;
            }
            if (Encoding.Default.GetByteCount(stringToSub) > length)
                return sb.ToString() + suspoint;
            else
                return sb.ToString();
        }
    }

}
