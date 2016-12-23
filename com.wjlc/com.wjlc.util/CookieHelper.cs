using System;
using System.Web;

namespace com.wjlc.util
{
    public class CookieHelper
    {
        const string COOKIE_PATH = "/";
        const string COOKIE_DOMAIN = "wjlc.com";
        const string COOKIE_VERSION = "1";
        public static int COOKIE_EXPIRE = 86400;//默认过期时间24小时

        /// <summary>
        /// 取得cookie
        /// </summary>
        /// <param name="cookieName"></param>
        /// <returns></returns>
        public static string GetCookie(string cookieName)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
            if (cookie == null)
            {
                return "";
            }
            else
            {
                return cookie.Value.ToString();
            }
        }

        /// <summary>
        /// 按默认时间设置cookie
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="cookieValue"></param>
        public static void SetCookie(string cookieName, string cookieValue)
        {
            HttpCookie cookie = new HttpCookie(cookieName);
            cookie.Domain = COOKIE_DOMAIN;
            cookie.Path = COOKIE_PATH;
            cookie.Expires = DateTime.Now.AddSeconds(COOKIE_EXPIRE);
            cookie.Value = HttpUtility.UrlEncode(cookieValue);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// 按参数设置cookie
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="cookieValue"></param>
        /// <param name="saveStatus">是否永久保存</param>
        /// <param name="httpOnly">是否是只能通过http访问</param>
        public static void SetCookie(string cookieName, string cookieValue, bool saveStatus, bool httpOnly)
        {
            HttpCookie cookie = new HttpCookie(cookieName);
            cookie.Domain = COOKIE_DOMAIN;
            cookie.Path = COOKIE_PATH;
            if (saveStatus)
            {
                cookie.Expires = DateTime.MaxValue;
            }
            else
            {
                cookie.Expires = DateTime.Now.AddSeconds(COOKIE_EXPIRE);
            }

            if (httpOnly)
            {
                cookie.HttpOnly = true;
            }
            cookie.Value = HttpUtility.UrlEncode(cookieValue);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// 清空cookie
        /// </summary>
        /// <param name="cookieName"></param>c
        /// <returns></returns>
        public static bool DeleteCookie(string cookieName)
        {
            bool ReturnBoolValue = false;
            HttpCookie httpCookie = new HttpCookie(cookieName);
            if (httpCookie != null)
            {
                httpCookie.Domain = COOKIE_DOMAIN;
                httpCookie.Path = "/";
                httpCookie.Expires = DateTime.Now.AddDays(-30);
                HttpContext.Current.Response.Cookies.Add(httpCookie);
                ReturnBoolValue = true;
            }
            return ReturnBoolValue;
        }

        /// <summary>
        /// 清空cookie
        /// </summary>
        /// <param name="cookieName"></param>c
        /// <returns></returns>
        public static bool DeleteCookie(string cookieName, string domain, string path)
        {
            bool ReturnBoolValue = false;
            HttpCookie httpCookie = new HttpCookie(cookieName);
            if (httpCookie != null)
            {
                httpCookie.Domain = domain;
                httpCookie.Path = path;
                httpCookie.Expires = DateTime.Now.AddDays(-30);
                HttpContext.Current.Response.Cookies.Add(httpCookie);
                ReturnBoolValue = true;
            }
            return ReturnBoolValue;
        }
    }

}
