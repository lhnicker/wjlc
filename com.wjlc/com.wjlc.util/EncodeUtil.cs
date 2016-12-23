using System;
using System.Text;

namespace com.wjlc.util
{
    public class EncodeUtil
    {
        #region 字符串转换
        /// <summary>
        /// 将字节数组转化为数值 
        /// </summary>
        /// <param name="arrByte"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static int ConvertBytesToInt(byte[] arrByte, int offset)
        {
            return BitConverter.ToInt32(arrByte, offset);
        }

        /// <summary>
        /// 将数值转化为字节数组
        /// </summary>
        /// <param name="value"></param>
        /// <param name="reverse"></param>
        /// <returns></returns>
        public static byte[] ConvertIntToBytes(int value, bool reverse)
        {
            byte[] ret = BitConverter.GetBytes(value);
            if (reverse)
                Array.Reverse(ret);
            return ret;
        }

        /// <summary>
        /// 将字节数组转化为16进制字符串
        /// </summary>
        /// <param name="arrByte"></param>
        /// <param name="reverse"></param>
        /// <returns></returns>
        public static string ConvertBytesToHex(byte[] arrByte, bool reverse)
        {
            StringBuilder sb = new StringBuilder();
            if (reverse)
                Array.Reverse(arrByte);
            foreach (byte b in arrByte)
                sb.AppendFormat("{0:x2}", b);
            return sb.ToString();
        }

        /// <summary>
        /// 将16进制字符串转化为字节数组 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] ConvertHexToBytes(string value)
        {
            int len = value.Length / 2;
            byte[] ret = new byte[len];
            for (int i = 0; i < len; i++)
                ret[i] = (byte)(Convert.ToInt32(value.Substring(i * 2, 2), 16));
            return ret;
        }

        /// <summary>
        /// 将字符串编码为Base64字符串
        /// </summary>
        /// <param name="estr">要编码的字符串</param>
        public static string Base64Encode(string estr)
        {
            byte[] barray;
            barray = Encoding.Default.GetBytes(estr);
            return Convert.ToBase64String(barray);
        }

        /// <summary>
        /// 将Base64字符串解码为普通字符串
        /// </summary>
        /// <param name="dstr">要解码的字符串</param>
        public static string Base64Decode(string dstr)
        {
            byte[] barray;
            barray = Convert.FromBase64String(dstr);
            return Encoding.Default.GetString(barray);
        }
        #endregion
    }
}
