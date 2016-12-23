using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Reflection;
using System.Data.Common;
using System.Data;
using System.Collections;

namespace com.wjlc.util
{
    public class JsonHelper
    {

        /// <summary>
        /// 按限定属性将List转成Json 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">对象列</param>
        /// <param name="prop">限定的属性,</param>
        /// <param name="sbJsonName"></param>
        /// <returns></returns>
        public static string ListToJson<T>(IList<T> list, IList<string> prop = null)
        {
            int k = 0 ;
            object objValue = null;
            StringBuilder sbJson = new StringBuilder();
            //if (string.IsNullOrEmpty(sbJsonName))
            //{
            //    sbJsonName = list[0].GetType().Name;
            //}
            //sbJson.Append("{\"" + sbJsonName + "\":[");
            sbJson.Append("[");
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    T obj = Activator.CreateInstance<T>();
                    PropertyInfo[] pi = obj.GetType().GetProperties();
                    sbJson.Append("{");                    
                    for (int j = 0; j < pi.Length; j++)
                    {
                        if (prop != null)
                        {
                            if (prop.Contains(pi[j].Name.ToString()))
                            {
                                objValue =pi[j].GetValue(list[i], null);
                                if (objValue != null)
                                {
                                    sbJson.Append("\"" + pi[j].Name.ToString() + "\":" + StringFormat(objValue.ToString(), pi[j].GetValue(list[i], null).GetType()));
                                }
                                else
                                {
                                    sbJson.Append("\"" + pi[j].Name.ToString() + "\":\"\"");
                                }                                
                                if (k < prop.Count - 1)
                                {
                                    sbJson.Append(",");                                    
                                }
                                else if (k == prop.Count - 1)
                                {
                                    break;
                                }
                                k++;                                
                            }
                        }
                        else
                        {
                            objValue = pi[j].GetValue(list[i], null);
                            if (objValue != null)
                            {
                                sbJson.Append("\"" + pi[j].Name.ToString() + "\":" + StringFormat(objValue.ToString(), pi[j].GetValue(list[i], null).GetType()));
                            }
                            else
                            {
                                sbJson.Append("\"" + pi[j].Name.ToString() + "\":\"\"");
                            }
                            if (j < pi.Length - 1)
                            {
                                sbJson.Append(",");
                            }
                        }
                       
                    }
                    objValue = null;
                    k = 0;
                    sbJson.Append("}");
                    if (i < list.Count - 1)
                    {
                        sbJson.Append(",");
                    }
                }
            }
            sbJson.Append("]");            
            return sbJson.ToString();
        }


        /// <summary>   
        /// 对象转换为Json字符串   
        /// </summary>   
        /// <param name="sbJsonObject">对象</param>   
        /// <returns>sbJson字符串</returns>   
        public static string ObjectToJson(object paraOject, IList<string> prop = null)
        {
            int k = 0;
            object objValue = null;
            string sbJsonString = "{";
            PropertyInfo[] propertyInfo = paraOject.GetType().GetProperties();
            for (int i = 0; i < propertyInfo.Length; i++)
            {
                if (prop != null)
                {
                    if (prop.Contains(propertyInfo[i].Name.ToString()))
                    {
                        objValue  = propertyInfo[i].GetGetMethod().Invoke(paraOject, null);
                        if (objValue != null)
                        {
                            sbJsonString += "\"" + propertyInfo[i].Name.ToString() + "\":" + StringFormat(objValue.ToString(), propertyInfo[i].GetType()) + ",";
                        }
                        else
                        {
                            sbJsonString += "\"" + propertyInfo[i].Name.ToString() + "\":\"\",";
                        }
                        if (k == prop.Count - 1)
                        {
                            break;
                        }
                        k++;
                    }
                }
                else
                {
                    objValue = propertyInfo[i].GetGetMethod().Invoke(paraOject, null);
                    if (objValue != null)
                    {
                        sbJsonString += "\"" + propertyInfo[i].Name.ToString() + "\":" + StringFormat(objValue.ToString(), propertyInfo[i].GetType()) + ",";
                    }
                    else
                    {
                        sbJsonString += "\"" + propertyInfo[i].Name.ToString() + "\":\"\",";
                    }
                }
            }
            return sbJsonString.Remove(sbJsonString.Length - 1, 1) + "}";
        }

        /// <summary>   
        /// 对象集合转换sbJson   
        /// </summary>   
        /// <param name="array">集合对象</param>   
        /// <returns>sbJson字符串</returns>   
        public static string ObjectsToJson(IEnumerable array, IList<string> prop = null)
        {
            string strJson = "[";
            foreach (object item in array)
            {
                strJson += ObjectToJson(item, prop) + ",";
            }
            return strJson.Remove(strJson.Length - 1, 1) + "]";
        }

        /// <summary>  
        /// DataTable转成Json   
        /// </summary>  
        /// <param name="sbJsonName"></param>  
        /// <param name="dt"></param>  
        /// <returns></returns>  
        public static string TableToJson(DataTable dt, IList<string> prop = null)
        {
            int k = 0;
            StringBuilder sbJson = new StringBuilder();
            //if (string.IsNullOrEmpty(sbJsonName))
            //    sbJsonName = dt.TableName;
            //sbJson.Append("{\"" + sbJsonName + "\":[");
            sbJson.Append("[");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sbJson.Append("{");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (prop != null)
                        {
                            if (prop.Contains(dt.Columns[j].ColumnName.ToString()))
                            {
                                Type type = dt.Rows[i][j].GetType();
                                sbJson.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + StringFormat(dt.Rows[i][j].ToString(), type));
                              
                                if (k < prop.Count - 1)
                                {
                                    sbJson.Append(",");
                                }
                                else if (k == prop.Count - 1)
                                {
                                    break;
                                }
                                k++;       
                            }
                        }
                        else
                        {
                            Type type = dt.Rows[i][j].GetType();
                            sbJson.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + StringFormat(dt.Rows[i][j].ToString(), type));
                            if (j < dt.Columns.Count - 1)
                            {
                                sbJson.Append(",");
                            }
                        }
                    }
                    k = 0;
                    sbJson.Append("}");
                    if (i < dt.Rows.Count - 1)
                    {
                        sbJson.Append(",");
                    }
                }
            }
            sbJson.Append("]}");
            return sbJson.ToString();
        }

        /// <summary>   
        /// DataReader转换为Json   
        /// </summary>   
        /// <param name="dataReader">DataReader对象</param>   
        /// <returns>sbJson字符串</returns>   
        public static string DataReaderToJson(DbDataReader dataReader)
        {
            StringBuilder sbJsonString = new StringBuilder();
            sbJsonString.Append("[");
            while (dataReader.Read())
            {
                sbJsonString.Append("{");
                for (int i = 0; i < dataReader.FieldCount; i++)
                {
                    Type type = dataReader.GetFieldType(i);
                    string strKey = dataReader.GetName(i);
                    string strValue = dataReader[i].ToString();
                    sbJsonString.Append("\"" + strKey + "\":");
                    strValue = StringFormat(strValue, type);
                    if (i < dataReader.FieldCount - 1)
                    {
                        sbJsonString.Append(strValue + ",");
                    }
                    else
                    {
                        sbJsonString.Append(strValue);
                    }
                }
                sbJsonString.Append("},");
            }
            dataReader.Close();            
            sbJsonString.Append(sbJsonString.Remove(sbJsonString.Length - 1, 1) + "]");
            return sbJsonString.ToString();
        }

        /// <summary>   
        /// DataSet转换为Json   
        /// </summary>   
        /// <param name="dataSet">DataSet对象</param>   
        /// <returns>sbJson字符串</returns>   
        public static string DataSetToJson(DataSet dataSet)
        {
            string strJson = "{";
            foreach (DataTable table in dataSet.Tables)
            {
                strJson += "\"" + table.TableName + "\":" + TableToJson(table) + ",";
            }
            strJson = strJson.TrimEnd(',');
            return strJson + "}";
        }

        /// <summary>  
        /// 过滤特殊字符  
        /// </summary>  
        /// <param name="s"></param>  
        /// <returns></returns>  
        private static string StringFilter(String s)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                char c = s.ToCharArray()[i];
                switch (c)
                {
                    case '\"':
                        sb.Append("\\\""); break;
                    case '\\':
                        sb.Append("\\\\"); break;
                    case '/':
                        sb.Append("\\/"); break;
                    case '\b':
                        sb.Append("\\b"); break;
                    case '\f':
                        sb.Append("\\f"); break;
                    case '\n':
                        sb.Append("\\n"); break;
                    case '\r':
                        sb.Append("\\r"); break;
                    case '\t':
                        sb.Append("\\t"); break;
                    default:
                        sb.Append(c); break;
                }
            }
            return sb.ToString();
        }

        /// <summary>  
        /// 格式化字符型、日期型、布尔型  
        /// </summary>  
        /// <param name="str"></param>  
        /// <param name="type"></param>  
        /// <returns></returns>  
        private static string StringFormat(string str, Type type)
        {
            if (type == typeof(string))
            {
                str = StringFilter(str);
                str = "\"" + str + "\"";
            }
            else if (type == typeof(bool))
            {
                str = str.ToLower();
            }
            else
            {
                str = "\"" + str + "\"";
            }
            return str;
        }


        #region 生成json格式数据
        public static string GetJson<T>(T obj)
        {
            string szJson = string.Empty;
            DataContractJsonSerializer json = new DataContractJsonSerializer(obj.GetType());
            using (MemoryStream stream = new MemoryStream())
            {
                json.WriteObject(stream, obj);
                szJson = Encoding.UTF8.GetString(stream.ToArray());
            }
            return szJson;
        }
        #endregion

        #region 解析json格式数据
        public static T ParseFromJson<T>(string szJson)
        {
            T obj = Activator.CreateInstance<T>();
            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(szJson)))
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
                return (T)serializer.ReadObject(stream);
            }
        }
        #endregion



    }

    public class JsonResultObject
    {
        private string _status;
        private string _msg;
        private object _data;

        public string status
        {
            get { return _status; }
            set { _status = value; }
        }
        public string msg
        {
            get { return _msg; }
            set { _msg = value; }
        }
        public object data
        {
            get { return _data; }
            set { _data = value; }
        }

        public JsonResultObject(bool statusValue, string msgValue, object dataValue)
        {
            this._status = statusValue ? "succ" : "fail";
            this._msg = msgValue;
            this._data = dataValue;
        }
    }

}
