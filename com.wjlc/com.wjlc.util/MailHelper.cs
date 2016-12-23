using System;
using System.Text;
using System.Collections;
using System.Net.Sockets;

namespace com.wjlc.util
{

    public class MailHelper
    {
        //System.Web.Mail.MailMessage
        /// <summary>
        /// 设定语言代码，默认设定为GB2312，如不需要可设置为""
        /// </summary>
        public string Charset = "GB2312";

        /// <summary>
        /// 发件人地址
        /// </summary>
        public string From = "";

        /// <summary>
        /// 发件人姓名
        /// </summary>
        public string FromName = "";

        /// <summary>
        /// 回复邮件地址
        /// </summary>
        public string ReplyTo = "";

        /// <summary>
        /// 邮件服务器域名
        /// </summary>	
        private string mailserver = "";

        /// <summary>
        /// 邮件服务器域名和验证信息
        /// 形如："user:pass@www.server.com:25"，也可省略次要信息。如"user:pass@www.server.com"或"www.server.com"
        /// </summary>	
        public string MailDomain
        {
            set
            {
                string maidomain = value.Trim();
                int tempint;

                if (maidomain != "")
                {
                    tempint = maidomain.IndexOf("@");
                    if (tempint != -1)
                    {
                        string up = maidomain.Substring(0, tempint);
                        MailServerUserName = up.Substring(0, up.IndexOf(":"));
                        MailServerPassWord = up.Substring(up.IndexOf(":") + 1, up.Length - up.IndexOf(":") - 1);
                        maidomain = maidomain.Substring(tempint + 1, maidomain.Length - tempint - 1);
                    }

                    tempint = maidomain.IndexOf(":");
                    if (tempint != -1)
                    {
                        mailserver = maidomain.Substring(0, tempint);
                        mailserverport = System.Convert.ToInt32(maidomain.Substring(tempint + 1, maidomain.Length - tempint - 1));
                    }
                    else
                    {
                        mailserver = maidomain;
                    }
                }
            }
        }

        /// <summary>
        /// 邮件服务器端口号
        /// </summary>	
        private int mailserverport = 25;

        /// <summary>
        /// 邮件服务器端口号
        /// </summary>	
        public int MailDomainPort
        {
            set
            {
                mailserverport = value;
            }
        }

        /// <summary>
        /// 是否需要SMTP验证
        /// </summary>		
        private bool ESmtp = false;

        /// <summary>
        /// SMTP认证时使用的用户名
        /// </summary>
        private string username = "";

        /// <summary>
        /// SMTP认证时使用的用户名
        /// </summary>
        public string MailServerUserName
        {
            set
            {
                if (value.Trim() != "")
                {
                    username = value.Trim();
                    ESmtp = true;
                }
                else
                {
                    username = "";
                    ESmtp = false;
                }
            }
        }

        /// <summary>
        /// SMTP认证时使用的密码
        /// </summary>
        private string password = "";

        /// <summary>
        /// SMTP认证时使用的密码
        /// </summary>
        public string MailServerPassWord
        {
            set
            {
                password = value;
            }
        }

        /// <summary>
        /// 邮件主题
        /// </summary>		
        public string Subject = "";

        /// <summary>
        /// 是否Html邮件
        /// </summary>		
        public bool Html = false;

        /// <summary>
        /// 邮件正文
        /// </summary>		
        public string Body = "";

        /// <summary>
        /// 收件人列表
        /// </summary>
        private Hashtable Recipient = new Hashtable();

        /// <summary>
        /// 密送收件人列表
        /// </summary>
        private Hashtable RecipientBCC = new Hashtable();

        /// <summary>
        /// 邮件发送优先级，可设置为"High","Normal","Low"或"1","3","5"
        /// </summary>
        private string priority = "Normal";

        /// <summary>
        /// 邮件发送优先级，可设置为"High","Normal","Low"或"1","3","5"
        /// </summary>
        public string Priority
        {
            set
            {
                switch (value.ToLower())
                {
                    case "high":
                        priority = "High";
                        break;

                    case "1":
                        priority = "High";
                        break;

                    case "normal":
                        priority = "Normal";
                        break;

                    case "3":
                        priority = "Normal";
                        break;

                    case "low":
                        priority = "Low";
                        break;

                    case "5":
                        priority = "Low";
                        break;

                    default:
                        priority = "Normal";
                        break;
                }
            }
        }

        /// <summary>
        /// 错误消息反馈
        /// </summary>
        private string errmsg;

        /// <summary>
        /// 错误消息反馈
        /// </summary>		
        public string ErrorMessage
        {
            get
            {
                return errmsg;
            }
        }

        /// <summary>
        /// 服务器交互记录
        /// </summary>
        private string logs = "";

        /// <summary>
        /// 服务器交互记录。
        /// </summary>
        public string Logs
        {
            get
            {
                return logs;
            }
        }

        private string enter = "\r\n";
        /// <summary>
        /// TcpClient对象，用于连接服务器
        /// </summary>	
        private TcpClient tc;

        /// <summary>
        /// NetworkStream对象
        /// </summary>	
        private NetworkStream ns;

        /// <summary>
        /// SMTP错误代码哈希表
        /// </summary>
        private Hashtable ErrCodeHT = new Hashtable();

        /// <summary>
        /// SMTP正确代码哈希表
        /// </summary>
        private Hashtable RightCodeHT = new Hashtable();

        /// <summary>
        /// SMTP回应代码哈希表
        /// </summary>
        private void SMTPCodeAdd()
        {
            ErrCodeHT.Add("500", "邮箱地址错误");
            ErrCodeHT.Add("501", "参数格式错误");
            ErrCodeHT.Add("502", "命令不可实现");
            ErrCodeHT.Add("503", "服务器需要SMTP验证");
            ErrCodeHT.Add("504", "命令参数不可实现");
            ErrCodeHT.Add("421", "服务未就绪，关闭传输信道");
            ErrCodeHT.Add("450", "要求的邮件操作未完成，邮箱不可用（例如，邮箱忙）");
            ErrCodeHT.Add("550", "要求的邮件操作未完成，邮箱不可用（例如，邮箱未找到，或不可访问）");
            ErrCodeHT.Add("451", "放弃要求的操作；处理过程中出错");
            ErrCodeHT.Add("551", "用户非本地，请尝试<forward-path>");
            ErrCodeHT.Add("452", "系统存储不足，要求的操作未执行");
            ErrCodeHT.Add("552", "过量的存储分配，要求的操作未执行");
            ErrCodeHT.Add("553", "邮箱名不可用，要求的操作未执行（例如邮箱格式错误）");
            ErrCodeHT.Add("432", "需要一个密码转换");
            ErrCodeHT.Add("534", "认证机制过于简单");
            ErrCodeHT.Add("538", "当前请求的认证机制需要加密");
            ErrCodeHT.Add("454", "临时认证失败");
            ErrCodeHT.Add("530", "需要认证");

            RightCodeHT.Add("220", "服务就绪");
            RightCodeHT.Add("250", "要求的邮件操作完成");
            RightCodeHT.Add("251", "用户非本地，将转发向<forward-path>");
            RightCodeHT.Add("354", "开始邮件输入，以<CRLF>.<CRLF>结束");
            RightCodeHT.Add("221", "服务关闭传输信道");
            RightCodeHT.Add("334", "服务器响应验证Base64字符串");
            RightCodeHT.Add("235", "验证成功");
        }

        /// <summary>
        /// 将字符串编码为Base64字符串
        /// </summary>
        /// <param name="estr">要编码的字符串</param>
        private string Base64Encode(string estr)
        {
            byte[] barray;
            barray = Encoding.Default.GetBytes(estr);
            return Convert.ToBase64String(barray);
        }

        /// <summary>
        /// 将Base64字符串解码为普通字符串
        /// </summary>
        /// <param name="dstr">要解码的字符串</param>
        private string Base64Decode(string dstr)
        {
            byte[] barray;
            barray = Convert.FromBase64String(dstr);
            return Encoding.Default.GetString(barray);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public MailHelper()
        {
            
        }

        ///// <summary>
        ///// 析构函数
        ///// </summary>
        //~MailHelper()
        //{
        //    ns.Close();
        //    tc.Close();
        //}

        /// <summary>
        /// 收件人姓名
        /// </summary>	
        public string RecipientName = "";

        private int RecipientNum = 0;//收件人数量
        private int RecipientBCCNum = 0;//密件收件人数量

        /// <summary>
        /// 添加一个收件人
        /// </summary>	
        /// <param name="str">收件人地址</param>
        public bool AddRecipient(string str)
        {
            str = str.Trim();
            if (str == null || str == "" || str.IndexOf("@") == -1)
                return true;
            //			if(RecipientNum<10)
            //			{
            Recipient.Add(RecipientNum, str);
            RecipientNum++;
            return true;
            //			}
            //			else
            //			{
            //				errmsg+="收件人过多";
            //				return false;
            //			}
        }

        /// <summary>
        /// 添加一组收件人（不超过10个），参数为字符串数组
        /// </summary>
        /// <param name="str">保存有收件人地址的字符串数组（不超过10个）</param>	
        public bool AddRecipient(string[] str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                if (!AddRecipient(str[i]))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 添加一个密件收件人
        /// </summary>
        /// <param name="str">收件人地址</param>
        public bool AddRecipientBCC(string str)
        {
            if (str == null || str.Trim() == "")
                return true;
            //			if(RecipientBCCNum<10)
            //			{
            RecipientBCC.Add(RecipientBCCNum, str);
            RecipientBCCNum++;
            return true;
            //			}
            //			else
            //			{
            //				errmsg+="收件人过多";
            //				return false;
            //			}
        }

        /// <summary>
        /// 添加一组密件收件人（不超过10个），参数为字符串数组
        /// </summary>	
        /// <param name="str">保存有收件人地址的字符串数组（不超过10个）</param>
        public bool AddRecipientBCC(string[] str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                if (!AddRecipientBCC(str[i]))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 发送SMTP命令
        /// </summary>	
        private bool SendCommand(string Command)
        {
            byte[] WriteBuffer;
            if (Command == null || Command.Trim() == "")
            {
                return true;
            }
            logs += Command;
            WriteBuffer = Encoding.Default.GetBytes(Command);
            try
            {
                ns.Write(WriteBuffer, 0, WriteBuffer.Length);
            }
            catch
            {
                errmsg = "网络连接错误";
                return false;
            }
            return true;
        }

        /// <summary>
        /// 接收SMTP服务器回应
        /// </summary>
        private string RecvResponse()
        {
            int StreamSize;
            string ReturnValue = "";
            byte[] ReadBuffer = new byte[1024];
            try
            {
                StreamSize = ns.Read(ReadBuffer, 0, ReadBuffer.Length);
            }
            catch
            {
                errmsg = "网络连接错误";
                return "false";
            }

            if (StreamSize == 0)
            {
                return ReturnValue;
            }
            else
            {
                ReturnValue = Encoding.Default.GetString(ReadBuffer).Substring(0, StreamSize);
                logs += ReturnValue;
                return ReturnValue;
            }
        }

        /// <summary>
        /// 与服务器交互，发送一条命令并接收回应。
        /// </summary>
        /// <param name="Command">一个要发送的命令</param>
        /// <param name="errstr">如果错误，要反馈的信息</param>
        private bool Dialog(string Command, string errstr)
        {
            if (Command == null || Command.Trim() == "")
            {
                return true;
            }
            if (SendCommand(Command))
            {
                string RR = RecvResponse();
                if (RR == "false")
                {
                    return false;
                }
                string RRCode = RR.Substring(0, 3);
                if (RightCodeHT[RRCode] != null)
                {
                    return true;
                }
                else
                {
                    if (ErrCodeHT[RRCode] != null)
                    {
                        errmsg += (RRCode + ErrCodeHT[RRCode].ToString());
                        errmsg += enter;
                    }
                    else
                    {
                        errmsg += RR;
                    }
                    errmsg += errstr;
                    return false;
                }
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// 与服务器交互，发送一组命令并接收回应。
        /// </summary>
        private bool Dialog(string[] Command, string errstr)
        {
            for (int i = 0; i < Command.Length; i++)
            {
                if (!Dialog(Command[i], ""))
                {
                    errmsg += enter;
                    errmsg += errstr;
                    return false;
                }
            }

            return true;
        }

        private bool SendEmail()
        {
            //连接网络
            try
            {
                tc = new TcpClient(mailserver, mailserverport);
            }
            catch (Exception e)
            {
                errmsg = e.ToString();
                return false;
            }

            ns = tc.GetStream();
            SMTPCodeAdd();

            //验证网络连接是否正确
            if (RightCodeHT[RecvResponse().Substring(0, 3)] == null)
            {
                errmsg = "网络连接失败";
                return false;
            }


            string[] SendBuffer;
            string SendBufferstr;

            //进行SMTP验证
            if (ESmtp)
            {
                SendBuffer = new String[4];
                SendBuffer[0] = "EHLO " + mailserver + enter;
                SendBuffer[1] = "AUTH LOGIN" + enter;
                SendBuffer[2] = Base64Encode(username) + enter;
                SendBuffer[3] = Base64Encode(password) + enter;
                if (!Dialog(SendBuffer, "SMTP服务器验证失败，请核对用户名和密码。"))
                    return false;
            }
            else
            {
                SendBufferstr = "HELO " + mailserver + enter;
                if (!Dialog(SendBufferstr, ""))
                    return false;
            }

            //
            SendBufferstr = "MAIL FROM:<" + From + ">" + enter;
            if (!Dialog(SendBufferstr, "发件人地址错误，或不能为空"))
                return false;

            //
            SendBuffer = new string[10];
            for (int i = 0; i < Recipient.Count; i++)
            {
                SendBuffer[i] = "RCPT TO:<" + Recipient[i].ToString() + ">" + enter;
            }
            if (!Dialog(SendBuffer, "收件人地址有误"))
                return false;

            SendBuffer = new string[10];
            for (int i = 0; i < RecipientBCC.Count; i++)
            {

                SendBuffer[i] = "RCPT TO:<" + RecipientBCC[i].ToString() + ">" + enter;

            }
            if (!Dialog(SendBuffer, "密件收件人地址有误"))
                return false;

            SendBufferstr = "DATA" + enter;
            if (!Dialog(SendBufferstr, ""))
                return false;

            SendBufferstr = "From:" + ConvertHeaderToQP(FromName, "GB2312") + "<" + From + ">" + enter;

            if (ReplyTo.Trim() != "")
            {
                SendBufferstr += "Reply-To: " + ConvertHeaderToQP(ReplyTo, "GB2312") + enter;
            }

            SendBufferstr += "To:" + ConvertHeaderToQP(RecipientName, "GB2312") + "<" + Recipient[0] + ">" + enter;
            SendBufferstr += "CC:";
            for (int i = 1; i < Recipient.Count; i++)
            {
                SendBufferstr += Recipient[i].ToString() + "<" + Recipient[i].ToString() + ">,";
            }
            SendBufferstr += enter;
            //			if(Charset=="")
            //			{
            //				SendBufferstr+="Subject:" + Subject + enter;
            //			}
            //			else
            //			{
            //				SendBufferstr+="Subject:" + "=?" + Charset.ToUpper() + "?B?" + Base64Encode(Subject) +"?=" +enter;
            //			}
            StringBuilder cleanSubject = new StringBuilder(Subject);
            cleanSubject.Replace("\r\n", null);
            cleanSubject.Replace("\n", null);
            SendBufferstr += "Subject:" + ConvertHeaderToQP(cleanSubject.ToString(), "GB2312") + enter;
            SendBufferstr += "X-Priority:" + priority + enter;
            SendBufferstr += "X-MSMail-Priority:" + priority + enter;
            SendBufferstr += "Importance:" + priority + enter;
            SendBufferstr += "X-Mailer: Huolx.Pubclass" + enter;
            SendBufferstr += "MIME-Version: 1.0" + enter;

            if (Html)
            {
                SendBufferstr += "Content-Type: text/html;" + enter;
            }
            else
            {
                SendBufferstr += "Content-Type: text/plain;" + enter;
            }

            if (Charset == "")
            {
                SendBufferstr += "	charset=\"iso-8859-1\"" + enter;
            }
            else
            {
                SendBufferstr += "	charset=\"" + Charset.ToLower() + "\"" + enter;
            }

            //SendBufferstr+="Content-Transfer-Encoding: base64" + enter;
            SendBufferstr += "Content-Transfer-Encoding: quoted-printable";

            SendBufferstr += enter + enter;
            string body = ConvertToQP(Body, "GB2312");
            SendBufferstr += body + enter;
            //			SendBufferstr+= Base64Encode(Body) + enter;
            SendBufferstr += enter + "." + enter;

            if (!Dialog(SendBufferstr, "错误信件信息"))
                return false;


            SendBufferstr = "QUIT" + enter;
            if (!Dialog(SendBufferstr, "断开连接时错误"))
                return false;


            ns.Close();
            tc.Close();
            return true;
        }
        /// <summary>
        /// Convert to Quoted Printable if necessary
        /// </summary>
        /// <param name="s"></param>
        /// <param name="charset"></param>
        /// <param name="forceconversion">force a conversion</param>
        /// <returns></returns>
        internal static string ConvertHeaderToQP(string s, string charset, bool forceconversion)
        {
            if (!forceconversion)
            {
                bool needsconversion = false;
                for (int i = 0; i < s.Length; i++)
                {
                    if (s[i] > 126 || s[i] < 32)
                    {
                        needsconversion = true;
                        break;
                    }
                }
                if (!needsconversion)
                {
                    return s;
                }
            }
            return "=?" + charset + "?Q?" + ConvertToQP(s, charset) + "?=";
        }

        /// <summary>
        /// Convert to Quoted printable if necessary.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="charset"></param>
        /// <returns></returns>
        internal static string ConvertHeaderToQP(string s, string charset)
        {
            return ConvertHeaderToQP(s, charset, false);
        }
        /// <summary> Encodes a string using Quoted-Printable encoding (see RFC 1521)</summary>
        /// <param name="s">string that needs to be encoded</param>
        /// <param name="charset">charset of string that needs to be encoded</param>
        /// <example>
        /// <code>
        /// string qpEncodedText = MailEncoder.ConvertQP("饲琶");
        /// </code>
        /// </example>
        /// <returns>Quoted-Printable encoded string</returns>
        internal static string ConvertToQP(string s, string charset)
        {
            // TURNER.BSD MODIFIED 9/10/2004 GKW to fix NUMEROUS bugs
            // Conforms to rules described in http://www.fourmilab.ch/webtools/qprint/rfc1521.html

            if (s == null) { return null; }
            if (charset == null) charset = "ISO-8859-1"; // default charset

            // environment newline char
            char[] nl = Environment.NewLine.ToCharArray();

            // source char array
            char[] source = s.ToCharArray();
            char ch;

            StringBuilder sb = new StringBuilder();
            StringBuilder currLine = new StringBuilder();
            bool bNewline = false;

            Encoding oEncoding = Encoding.GetEncoding(charset);

            for (int sidx = 0; sidx < s.Length; sidx++)
            {
                ch = source[sidx];

                // RULE # 4: LINE BREAKS
                if (ch == nl[0] && sidx <= (s.Length - nl.Length))
                {
                    // peek ahead make sure the "whole" newline is present
                    if (s.Substring(sidx, nl.Length) == Environment.NewLine)
                    {
                        // RULE #3: ENCODE WHITESPACE PRECEEDING A HARD BREAK
                        if (currLine.Length > 0)
                        {
                            if (source[sidx - 1] == ' ')
                            {   // if char is preceded by space char add =20
                                currLine.Remove(currLine.Length - 1, 1);
                                currLine.Append("=20");
                            }
                            else if (source[sidx - 1] == '\t')
                            {   // if char is preceded by tab char add =09
                                currLine.Remove(currLine.Length - 1, 1);
                                currLine.Append("=09");
                            }
                        }

                        // flag for new line
                        bNewline = true;
                        sidx += nl.Length - 1;  // jump ahead 

                    }
                    else
                    {	// not actually a newline.  Encode per Rule #1
                        currLine.Append("=0" + Convert.ToString((byte)ch, 16).ToUpper());
                    }
                }
                // RULE #1 and #2
                // Optional characters are: !"#$@[\]^`{|}~
                else if (ch > 126 || (ch < 32 && ch != '\t') || ch == '=')
                {
                    byte[] outByte = new byte[10];
                    int iCount = oEncoding.GetBytes("" + ch, 0, 1, outByte, 0);

                    for (int i = 0; i < iCount; i++)
                    {
                        if (outByte[i] < 16)
                            currLine.Append("=0" + Convert.ToString(outByte[i], 16).ToUpper());
                        else
                            currLine.Append("=" + Convert.ToString(outByte[i], 16).ToUpper());
                    }
                }
                else
                {
                    currLine.Append(ch);
                }

                // Rule #5: MAXIMUM length 76 characters per line
                if (currLine.Length >= 76)
                {
                    // just make sure not to split an encoded char
                    string cLine = currLine.ToString();
                    int breakAt = cLine.LastIndexOf("=");
                    if (breakAt < 70) breakAt = 74;
                    sb.Append(cLine.Substring(0, breakAt) + "");
                    //sb.Append(cLine.Substring(0, breakAt) + "=\r\n");
                    currLine = new StringBuilder(cLine.Substring(breakAt));
                }

                if (bNewline)
                {
                    // RFC 822 linebreak = CRLF
                    sb.Append(currLine.ToString() + "\r\n");
                    currLine = new StringBuilder();
                    bNewline = false;
                }
            }

            // add last line
            sb.Append(currLine.ToString());

            return sb.ToString();
        }

        /// <summary>
        /// 发送邮件方法，所有参数均通过属性设置。
        /// </summary>
        public bool Send()
        {
            if (Recipient.Count == 0)
            {
                errmsg = "收件人列表不能为空";
                return false;
            }


            if (RecipientName == "")
                RecipientName = Recipient[0].ToString();

            if (mailserver.Trim() == "")
            {
                errmsg = "必须指定SMTP服务器";
                return false;
            }

            return SendEmail();

        }

        /// <summary>
        /// 发送邮件方法
        /// </summary>
        /// <param name="smtpserver">smtp服务器信息，如"username:password@www.smtpserver.com:25"，也可去掉部分次要信息，如"www.smtpserver.com"</param>
        public bool Send(string smtpserver)
        {

            MailDomain = smtpserver;
            return Send();
        }

        /// <summary>
        /// 发送邮件方法
        /// </summary>
        /// <param name="smtpserver">smtp服务器信息，如"username:password@www.smtpserver.com:25"，也可去掉部分次要信息，如"www.smtpserver.com"</param>
        /// <param name="from">发件人mail地址</param>
        /// <param name="fromname">发件人姓名</param>
        /// <param name="replyto">回复邮件地址</param>
        /// <param name="to">收件人地址</param>
        /// <param name="toname">收件人姓名</param>
        /// <param name="html">是否HTML邮件</param>
        /// <param name="subject">邮件主题</param>
        /// <param name="body">邮件正文</param>
        public bool Send(string smtpserver, string from, string fromname, string replyto, string to, string toname, bool html, string subject, string body)
        {
            MailDomain = smtpserver;
            From = from;
            FromName = fromname;
            ReplyTo = replyto;
            AddRecipient(to);
            RecipientName = toname;
            Html = html;
            Subject = subject;
            Body = body;
            return Send();
        }
    }
}
