using System;
using System.Text;
using System.Text.RegularExpressions;

namespace com.wjlc.util
{
    /// <summary>
    /// ҳ������У����
    /// </summary>
    public class PageValidate 
	{
		private static Regex RegNumber = new Regex("^[0-9]+$");
		private static Regex RegNumberSign = new Regex("^[+-]?[0-9]+$");
        private static Regex RegDecimal = new Regex("^\\d{1,}(\\.\\d{1,})?$");        //("^[0-9]+[.]?[0-9]+$");
		private static Regex RegDecimalSign = new Regex("^[+-]?[0-9]+[.]?[0-9]+$"); //�ȼ���^[+-]?\d+[.]?\d+$
		private static Regex RegEmail = new Regex(@"^[\w\.-]+@([\w-]+\.)+[a-zA-Z]{2,4}$");//w Ӣ����ĸ�����ֵ��ַ������� [a-zA-Z0-9] �﷨һ�� 
        private static Regex RegCHZN = new Regex("[\u4e00-\u9fa5]");//^[\u4E00-\u9FA5\uF900-\uFA2D]+$
        private static Regex RegPostCode = new Regex(@"^\d{6}$"); // ��������
        private static Regex RegMobile = new Regex(@"^1[3456789]\d{9}$"); // �ֻ�����
        private static Regex RegTelphone = new Regex(@"^\d{3,4}-\d{7,8}(-\d{1,6})?$"); //���ڴ��ֻ��ĵ绰����
        private static Regex RegIdentityCard = new Regex(@"^\d{6}(19|20)?\d{2}(0[1-9]|10|11|12)([012]\d|30|31)\d{3}[xX\d]?$");
        private static Regex RegDateAndTime = new Regex(@"^\d{4}-\d{1,2}-\d{1,2}\s\d{1,2}:\d{1,2}:\d{1,2}$");//����+ʱ��
        private static Regex RegDate = new Regex(@"^((\d{2}(([02468][048])|([13579][26]))[\-\/\s]?((((0?[13578])|(1[02]))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(3[01])))|(((0?[469])|(11))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(30)))|(0?2[\-\/\s]?((0?[1-9])|([1-2][0-9])))))|(\d{2}(([02468][1235679])|([13579][01345789]))[\-\/\s]?((((0?[13578])|(1[02]))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(3[01])))|(((0?[469])|(11))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(30)))|(0?2[\-\/\s]?((0?[1-9])|(1[0-9])|(2[0-8]))))))$"); //���ڲ���

		#region �����ַ������		
	
		/// <summary>
		/// �Ƿ������ַ���
		/// </summary>
		/// <param name="inputData">�����ַ���</param>
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
		/// �Ƿ������ַ��� �ɴ�������
		/// </summary>
		/// <param name="inputData">�����ַ���</param>
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
		/// �Ƿ��Ǹ�����
		/// </summary>
		/// <param name="inputData">�����ַ���</param>
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
		/// �Ƿ��Ǹ����� �ɴ�������
		/// </summary>
		/// <param name="inputData">�����ַ���</param>
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

		#region ���ļ��

		/// <summary>
		/// ����Ƿ��������ַ�
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

		#region �ʼ���ַ
		/// <summary>
		/// �Ƿ��Ǹ����� �ɴ�������
		/// </summary>
		/// <param name="inputData">�����ַ���</param>
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
        /// ��֤�Ƿ�Ϊ�ʱ�
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
        /// ��֤�ֻ���
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
        /// ��֤���ڹ̶��绰
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
        /// ��֤���֤����
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

		#region ����

		/// <summary>
		/// ����ַ�����󳤶ȣ�����ָ�����ȵĴ�
		/// </summary>
		/// <param name="sqlInput">�����ַ���</param>
		/// <param name="maxLength">��󳤶�</param>
		/// <returns></returns>			
		public static string SqlText(string sqlInput, int maxLength)
		{			
			if(sqlInput != null && sqlInput != string.Empty)
			{
				sqlInput = sqlInput.Trim();							
				if(sqlInput.Length > maxLength)//����󳤶Ƚ�ȡ�ַ���
					sqlInput = sqlInput.Substring(0, maxLength);
			}
			return sqlInput;
		}		

	
		/// <summary>
		/// �����ַ��������maxLength<=0�������ضϴ���
		/// </summary>
		/// <param name="inputString"></param>
		/// <param name="maxLength"></param>
		/// <returns></returns>
		public static string InputText(string inputString, int maxLength) 
		{			
			StringBuilder retVal = new StringBuilder();

			// ����Ƿ�Ϊ��
			if ((inputString != null) && (inputString != String.Empty)) 
			{
				inputString = inputString.Trim();
				
				//��鳤��
				if (maxLength>0 && inputString.Length > maxLength)
					inputString = inputString.Substring(0, maxLength);
				
				//�滻Σ���ַ�
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
				retVal.Replace("'", " ");// �滻������
			}
			return retVal.ToString();
			
		}
		/// <summary>
		/// ת���� HTML code
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
		///����html�� ��ͨ�ı�
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
        /// �������ַ��滻Ϊ�ո�
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
        
        #region ����+ʱ��
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
