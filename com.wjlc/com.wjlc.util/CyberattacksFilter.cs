namespace com.wjlc.util
{
    public  class CyberattacksFilter
    {
       /// <summary>
       /// 过滤可能产生的攻击字符
       /// </summary>
       /// <param name="str"></param>
       /// <returns></returns>
       public static string Filter(string str)
       {
           return StringUtility.FilterScript(StringUtility.FilterLink(StringUtility.FilterStyle(SqlUtil.ReplaceInjection2(str))));
       }
    }
}
