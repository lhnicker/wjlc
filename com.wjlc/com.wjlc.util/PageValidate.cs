using System;
using System.Text;
using System.Text.RegularExpressions;

namespace com.wjlc.util
{
    /// <summary>
    /// 页面数据校验类
    /// </summary>
    public class PageValidate 
	{
		private static Regex RegNumber = new Regex("^[0-9]+$");
		private static Regex RegNumberSign = new Regex("^[+-]?[0-9]+$");
        private static Regex RegDecimal = new Regex("^\\d{1,}(\\.\\d{1,})?$");        //("^[0-9]+[.]?[0-9]+$");
		private static Regex RegDecimalSign = new Regex("^[+-]?[0-9]+[.]?[0-9]+$"); //等价于^[+-]?\d+[.]?\d+$
		private static Regex RegEmail = new Regex(@"^[\w\.-]+@([\w-]+\.)+[a-zA-Z]{2,4}$");//w 英文字母或数字的字符串，和 [a-zA-Z0-9] 语法一样 
        private static Regex RegCHZN = new Regex("[\u4e00-\u9fa5]");//^[\u4E00-\u9FA5\uF900-\uFA2D]+$
        private static Regex RegPostCode = new Regex(@"^\d{6}$"); // 邮政编码
        private static Regex RegMobile = new Regex(@"^1[3456789]\d{9}$"); // 手机号码
        private static Regex RegTelphone = new Regex(@"^\d{3,4}-\d{7,8}(-\d{1,6})?$"); //国内带分机的电话号码
        private static Regex RegIdentityCard = new Regex(@"^\d{6}(19|20)?\d{2}(0[1-9]|10|11|12)([012]\d|30|31)\d{3}[xX\d]?$");
        private static Regex RegDateAndTime = new Regex(@"^\d{4}-\d{1,2}-\d{1,2}\s\d{1,2}:\d{1,2}:\d{1,2}$");//日期+时间
        private static Regex RegDate = new Regex(@"^((\d{2}(([02468][048])|([13579][26]))[\-\/\s]?((((0?[13578])|(1[02]))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(3[01])))|(((0?[469])|(11))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(30)))|(0?2[\-\/\s]?((0?[1-9])|([1-2][0-9])))))|(\d{2}(([02468][1235679])|([13579][01345789]))[\-\/\s]?((((0?[13578])|(1[02]))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(3[01])))|(((0?[469])|(11))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(30)))|(0?2[\-\/\s]?((0?[1-9])|(1[0-9])|(2[0-8]))))))$"); //日期部分

		#region 数字字符串检查		
	
		/// <summary>
		/// 是否数字字符串
		/// </summary>
		/// <param name="inputData">输入字符串</param>
		/// <returns></returns>
		public static bool IsNumber(string inputData,bool isAllowNull)
		{
            if (isAllowNull && String.IsNullOrEmpty(inputData))
            {
                return true;
            }
			return RegNumber.IsMatch(inputData);
		}		
		/// <summary>
		/// 是否数字字符串 可带正负号
		/// </summary>
		/// <param name="inputData">输入字符串</param>
		/// <returns></returns>
        public static bool IsNumberSign(string inputData, bool isAllowNull)
		{
            if (isAllowNull && String.IsNullOrEmpty(inputData))
            {
                return true;
            } 
            return RegNumberSign.IsMatch(inputData);
		}		
		/// <summary>
		/// 是否是浮点数
		/// </summary>
		/// <param name="inputData">输入字符串</param>
		/// <returns></returns>
        public static bool IsDecimal(string inputData, bool isAllowNull)
		{
            if (isAllowNull && String.IsNullOrEmpty(inputData))
            {
                return true;
            }
            return RegDecimal.IsMatch(inputData);
		}		
		/// <summary>
		/// 是否是浮点数 可带正负号
		/// </summary>
		/// <param name="inputData">输入字符串</param>
		/// <returns></returns>
        public static bool IsDecimalSign(string inputData, bool isAllowNull)
		{
            if (isAllowNull && String.IsNullOrEmpty(inputData))
            {
                return true;
            }
            return RegDecimalSign.IsMatch(inputData);
		}		

		#endregion

		#region 中文检测

		/// <summary>
		/// 检测是否有中文字符
		/// </summary>
		/// <param name="inputData"></param>
		/// <returns></returns>
        public static bool IsHasCHZN(string inputData, bool isAllowNull)
		{
            if (isAllowNull && String.IsNullOrEmpty(inputData))
            {
                return true;
            }
            return RegCHZN.IsMatch(inputData);
		}	

		#endregion

		#region 邮件地址
		/// <summary>
		/// 是否是浮点数 可带正负号
		/// </summary>
		/// <param name="inputData">输入字符串</param>
		/// <returns></returns>
        public static bool IsEmail(string inputData, bool isAllowNull)
		{
            if (isAllowNull && String.IsNullOrEmpty(inputData))
            {
                return true;
            }
            return RegEmail.IsMatch(inputData);
		}

        /// <summary>
        /// 验证是否为邮编
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public static bool IsPostCode(string inputData, bool isAllowNull)
        {
            if (isAllowNull && String.IsNullOrEmpty(inputData))
            {
                return true;
            }
            return RegPostCode.IsMatch(inputData);
        }

        /// <summary>
        /// 验证手机号
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public static bool IsMobile(string inputData, bool isAllowNull)
        {
            if (isAllowNull && String.IsNullOrEmpty(inputData))
            {
                return true;
            }
            return RegMobile.IsMatch(inputData);
        }

        /// <summary>
        /// 验证国内固定电话
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public static bool IsTelphone(string inputData, bool isAllowNull)
        {
            if (isAllowNull && String.IsNullOrEmpty(inputData))
            {
                return true;
            }
            return RegTelphone.IsMatch(inputData);
        }

        /// <summary>
        /// 验证身份证号码
        /// </summary>
        /// <param name="inputData"></param>
        /// <param name="isAllowNull"></param>
        /// <returns></returns>
        public static bool IsIdentityCard(string inputData, bool isAllowNull)
        {
            if (isAllowNull && String.IsNullOrEmpty(inputData))
            {
                return true;
            }
            return RegIdentityCard.IsMatch(inputData);
        }



		#endregion

		#region 其他

		/// <summary>
		/// 检查字符串最大长度，返回指定长度的串
		/// </summary>
		/// <param name="sqlInput">输入字符串</param>
		/// <param name="maxLength">最大长度</param>
		/// <returns></returns>			
		public static string SqlText(string sqlInput, int maxLength)
		{			
			if(sqlInput != null && sqlInput != string.Empty)
			{
				sqlInput = sqlInput.Trim();							
				if(sqlInput.Length > maxLength)//按最大长度截取字符串
					sqlInput = sqlInput.Substring(0, maxLength);
			}
			return sqlInput;
		}		

	
		/// <summary>
		/// 清理字符串，如果maxLength<=0，则不做截断处理
		/// </summary>
		/// <param name="inputString"></param>
		/// <param name="maxLength"></param>
		/// <returns></returns>
		public static string InputText(string inputString, int maxLength) 
		{			
			StringBuilder retVal = new StringBuilder();

			// 检查是否为空
			if ((inputString != null) && (inputString != String.Empty)) 
			{
				inputString = inputString.Trim();
				
				//检查长度
				if (maxLength>0 && inputString.Length > maxLength)
					inputString = inputString.Substring(0, maxLength);
				
				//替换危险字符
				for (int i = 0; i < inputString.Length; i++) 
				{
					switch (inputString[i]) 
					{
						case '"':
							retVal.Append("&quot;");
							break;
						case '<':
							retVal.Append("&lt;");
							break;
						case '>':
							retVal.Append("&gt;");
							break;
						default:
							retVal.Append(inputString[i]);
							break;
					}
				}				
				retVal.Replace("'", " ");// 替换单引号
			}
			return retVal.ToString();
			
		}
		/// <summary>
		/// 转换成 HTML code
		/// </summary>
		/// <param name="str">string</param>
		/// <returns>string</returns>
		public static string Encode(string str)
		{			
			str = str.Replace("&","&amp;");
			str = str.Replace("'","''");
			str = str.Replace("\"","&quot;");
			str = str.Replace(" ","&nbsp;");
			str = str.Replace("<","&lt;");
			str = str.Replace(">","&gt;");
			str = str.Replace("\n","<br>");
			return str;
		}
		/// <summary>
		///解析html成 普通文本
		/// </summary>
		/// <param name="str">string</param>
		/// <returns>string</returns>
		public static string Decode(string str)
		{			
			str = str.Replace("<br>","\n");
			str = str.Replace("&gt;",">");
			str = str.Replace("&lt;","<");
			str = str.Replace("&nbsp;"," ");
			str = str.Replace("&quot;","\"");
			return str;
		}

        /// <summary>
        /// 将特殊字符替换为空格
        /// </summary>
        /// <param name="strHtml"></param>
        /// <returns></returns>
        public static string StripHTML(string strHtml)
        {
            string strOutput = strHtml;
            Regex regex = new Regex(@"<[^>]+>|]+>");
            strOutput = regex.Replace(strOutput, "");
            return strOutput;
        }

        public static string StriptJson(string str)
        {
            str = str.Replace("\"", "\\\"");
            str = str.Replace("\r\n", "\\r\\n");
            str = str.Replace("\t", "\\t");
            str = str.Replace("\\", "\\");
            str = str.Replace("\b", "\\b");
            return str;
        }

		#endregion
        
        #region 日期+时间
        public static bool IsDateAndTime(string inputData)
        {
            Match m = RegDateAndTime.Match(inputData);
            return m.Success;
        }
        public static bool IsDate(string inputData)     
        {
            Match m = RegDate.Match(inputData);
            return m.Success;
        }
        #endregion
	}
}
