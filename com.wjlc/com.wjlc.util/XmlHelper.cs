using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Xsl;
using System.Web;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Data;

namespace com.wjlc.util
{
    public class XmlHelper
    {
        /// <summary>
        /// 创建一个xml
        /// </summary>
        /// <param name="rootNode"></param>
        /// <returns></returns>
        public static XmlDocument GetXmlDocument(string rootNode)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\" ?>" + rootNode);
            return xmlDoc;
        }

        /// <summary>
        /// 取得一个xml文档
        /// </summary>
        /// <param name="strXML"></param>
        /// <returns></returns>
        public static XmlDocument GetXmlDocumentFromString(string strXML)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(strXML);
            return xmlDoc;
        }

        /// <summary>
        /// 将xml转换成DataSet
        /// </summary>
        /// <param name="xmlStr"></param>
        /// <returns></returns>
        public static DataSet XmlToDataSet(string xmlStr)
        {
            if (!string.IsNullOrEmpty(xmlStr))
            {
                StringReader StrStream = null;
                XmlTextReader Xmlrdr = null;
                try
                {
                    DataSet ds = new DataSet();
                    StrStream = new StringReader(xmlStr);
                    Xmlrdr = new XmlTextReader(StrStream);
                    ds.ReadXml(Xmlrdr);
                    return ds;
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    //释放资源                     
                    if (Xmlrdr != null)
                    {
                        Xmlrdr.Close(); StrStream.Close(); StrStream.Dispose();
                    }
                }
            }
            else
            {
                return null;
            }
        }

        #region 读取指定节点的指定属性值

        /// <summary>
        /// 功能:读取指定节点的指定属性值
        /// 参数:
        /// 参数一:节点名称
        /// 参数二:此节点的属性
        /// </summary>
        /// <param name="strNode"></param>
        /// <param name="strAttribute"></param>
        /// <returns></returns>
        public static string GetXmlNodeValue(XmlDocument xmlDoc , string strNode, string strAttribute)
        {
            string strReturn = "";
            try
            {
                //根据指定路径获取节点
                XmlNode xmlNode = xmlDoc.SelectSingleNode(strNode);
                //获取节点的属性，并循环取出需要的属性值
                XmlAttributeCollection xmlAttr = xmlNode.Attributes;

                for (int i = 0; i < xmlAttr.Count; i++)
                {
                    if (xmlAttr.Item(i).Name == strAttribute)
                        strReturn = xmlAttr.Item(i).Value;
                }
            }
            catch (XmlException xmle)
            {
                throw xmle;
            }
            return strReturn;
        }
        #endregion

        #region 读取指定节点的值
        /// <summary>
        /// 功能:读取指定节点的值
        /// 参数:
        /// 参数:节点名称
        /// </summary>
        /// <param name="strNode"></param>
        /// <returns></returns>
        public static string GetXmlNodeValue(XmlDocument xmlDoc, string strNode)
        {
            string strReturn = String.Empty;
            try
            {
                //根据路径获取节点
                XmlNode xmlNode = xmlDoc.SelectSingleNode(strNode);
                if (xmlNode != null)
                    strReturn = xmlNode.InnerText;
            }
            catch (XmlException xmle)
            {
                System.Console.WriteLine(xmle.Message);
            }
            return strReturn;
        }
        #endregion

        /// <summary>
        /// 按限定属性将List转成XML节点
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">对象列</param>
        /// <param name="prop">限定的属性,</param>
        /// <param name="sbJsonName"></param>
        /// <returns></returns>
        public static void ListToXML<T>(XmlNode node,IList<T> list, IList<string> prop = null)
        {
            XmlNode dataListNode = AddNewNode(node,"datalist","");
            int k = 0;
            object objValue = null;
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    XmlNode dataNode = AddNewNode(dataListNode, list[i].GetType().Name, "");
      
                    PropertyInfo[] pi = Activator.CreateInstance<T>().GetType().GetProperties();
                    for (int j = 0; j < pi.Length; j++)
                    {
                        if (prop != null)
                        {
                            if (prop.Contains(pi[j].Name.ToString()))
                            {
                                objValue = pi[j].GetValue(list[i], null);
                                if ( objValue != null)
                                {
                                    SetAtrributeValue(dataNode, pi[j].Name.ToString(), objValue.ToString());
                                }
                                else
                                {
                                    SetAtrributeValue(dataNode, pi[j].Name.ToString(), "");
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
                            objValue = pi[j].GetValue(list[i], null);
                            if (objValue != null)
                            {
                                SetAtrributeValue(dataNode, pi[j].Name.ToString(), objValue.ToString());
                            }
                            else
                            {
                                SetAtrributeValue(dataNode, pi[j].Name.ToString(), "");
                            }
                        }
                    }
                    objValue = null;
                    k = 0;
                }
            }
        }

        /// <summary>   
        /// 对象转换为XML节点 
        /// </summary>   
        /// <param name="sbJsonObject">对象</param>   
        /// <returns>sbJson字符串</returns>   
        public static void ObjectToXML(XmlNode node, object paraOject, IList<string> prop = null)
        {
            int k = 0;
            XmlNode dataNode = AddNewNode(node, paraOject.GetType().Name, "");          
            PropertyInfo[] propertyInfo = paraOject.GetType().GetProperties();
            object objValue = null;
            for (int i = 0; i < propertyInfo.Length; i++)
            {
                if (prop != null)
                {
                    if (prop.Contains(propertyInfo[i].Name.ToString()))
                    {
                        objValue = propertyInfo[i].GetGetMethod().Invoke(paraOject, null);
                        if (objValue != null)
                        {
                            SetAtrributeValue(dataNode, propertyInfo[i].Name.ToString(), objValue.ToString());
                        }
                        else
                        {
                            SetAtrributeValue(dataNode, propertyInfo[i].Name.ToString(), "");
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
                        SetAtrributeValue(dataNode, propertyInfo[i].Name.ToString(), objValue.ToString());
                    }
                    else
                    {
                        SetAtrributeValue(dataNode, propertyInfo[i].Name.ToString(), "");
                    }
                    
                }
                objValue = null;
            }
        }


        /// <summary>  
        /// DataTable转成XML节点
        /// </summary>  
        /// <param name="sbJsonName"></param>  
        /// <param name="dt"></param>  
        /// <returns></returns>  
        public static void TableToXML(XmlNode node, DataTable dt , IList<string> prop = null)
        {
            int k = 0;
            XmlNode dataListNode = AddNewNode(node, "datalist", "");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        XmlNode dataNode = AddNewNode(dataListNode, dt.TableName, "");

                        if (prop != null)
                        {
                            if (prop.Contains(dt.Columns[j].ColumnName.ToString()))
                            {
                                SetAtrributeValue(dataNode, dt.Columns[j].ColumnName.ToString(), dt.Rows[i][j].ToString());
                                if (k == prop.Count - 1)
                                {
                                    break;
                                }
                                k++;
                            }
                        }
                        else
                        {
                            SetAtrributeValue(dataNode, dt.Columns[j].ColumnName.ToString(), dt.Rows[i][j].ToString());
                        }
                    }
                    k = 0;
                }
            }
        }

        /// <summary>
        /// 根据提供的xpath创建节点,并返回最终的页节点,
        /// 支持xpath的属性，支持是否创建属性节点
        /// </summary>
        /// <param name="xPath">相对于当前节点的xpath表达式，只支持简单的xpath，
        ///	如：a[@a='c']/b/c[@b='a' or @b='f']/d，
        ///		a[@a='c']/b/c[@b='a' and @a='f']/d,
        ///		a/b/c/d
        /// </param>
        /// <param name="rootNode">需要创建节点的根节点</param>
        /// <param name="IsCreateAttribute">是否创建属性节点</param>
        /// <returns>最后创建的页节点</returns>
        public static XmlNode GetOrCreateXmlTree(string xPath, XmlNode node)
        {
            return GetOrCreateXmlTree(xPath, node, true);
        }

        /// <summary>
        /// 根据提供的xpath创建节点,并返回最终的页节点,
        /// 支持xpath的属性，支持是否创建属性节点
        /// </summary>
        /// <param name="xPath">相对于当前节点的xpath表达式，只支持简单的xpath，
        ///	如：a[@a='c']/b/c[@b='a' or @b='f']/d，
        ///		a[@a='c']/b/c[@b='a' and @a='f']/d,
        ///		a/b/c/d
        /// </param>
        /// <param name="rootNode">需要创建节点的根节点</param>
        /// <param name="IsCreateAttribute">是否创建属性节点</param>
        /// <returns>最后创建的页节点</returns>
        public static XmlNode GetOrCreateXmlTree(string xPath, XmlNode node, bool IsCreateAttribute)
        {
            if (node.NodeType != XmlNodeType.Element)
            {
                throw new ArgumentException("传入的节点不是一个XmlNodeType.Element节点");
            }

            XmlDocument tempXdoc = node.OwnerDocument;
            XmlNode fatherNode = node;
            XmlNode sonNode = null;
            xPath = xPath.Trim('/');

            string regstr = @"(?<path>(?<name>\w+)[^/]*)";

            MatchCollection mc = Regex.Matches(xPath, regstr);

            foreach (Match match in mc)
            {
                string name = match.Groups["name"].Value;
                string path = match.Groups["path"].Value;
                sonNode = fatherNode.SelectSingleNode(path);
                if (sonNode == null)
                {
                    sonNode = tempXdoc.CreateElement(name);
                    fatherNode.AppendChild(sonNode);
                    if (IsCreateAttribute)
                    {
                        string attrRegx = "@(?<aName>\\w+)=['|\"]?(?<aValue>\\w+)['|\"]?";
                        MatchCollection amc = Regex.Matches(path, attrRegx);
                        foreach (Match amatch in amc)
                        {
                            string aName = amatch.Groups["aName"].Value;
                            string aValue = amatch.Groups["aValue"].Value;
                            sonNode.Attributes.Append(tempXdoc.CreateAttribute(aName));
                            sonNode.Attributes[aName].Value = aValue;
                        }
                    }
                }
                fatherNode = sonNode;
            }
            return sonNode;
        }

        /// <summary>
        /// 为xml节点的属性赋值，如果属性不存在则创建属性并赋值
        /// </summary>
        /// <param name="node"></param>
        /// <param name="attrName"></param>
        /// <param name="attrValue"></param>
        public static void SetAtrributeValue(XmlNode node, string attrName, string attrValue)
        {
            if (node.NodeType != XmlNodeType.Element)
            {
                throw new ArgumentException("传入的节点不是一个XmlNodeType.Element节点");
            }

            if (node.Attributes[attrName] == null)
            {
                XmlDocument tempXdoc = node.OwnerDocument;
                node.Attributes.Append(tempXdoc.CreateAttribute(attrName));
            }
            node.Attributes[attrName].Value = attrValue;
        }

        /// <summary>
        /// 添加一个新节点，如果有内容，同时为节点赋值，如果没有，则只创建
        /// </summary>
        /// <param name="fathernode"></param>
        /// <param name="name"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static XmlNode AddNewNode(XmlNode fathernode, string name, string content)
        {
            XmlDocument xdoc = fathernode.OwnerDocument;
            XmlNode snode = xdoc.CreateElement(name);
            if (!String.IsNullOrEmpty(content))
            {
                snode.InnerXml = ReplaceInvalidChar(content);
            }
            fathernode.AppendChild(snode);
            return snode;
        }

        public static string ReplaceInvalidChar(string instr)
        {
            string result = instr.Replace("&", "&amp;").Replace(">", "&gt;").Replace("<", "&lt;");
            return result;
        }

        public static XmlNode AddCDataNode(XmlNode fathernode, string name, string content)
        {
            XmlDocument xdoc = fathernode.OwnerDocument;
            XmlNode snode = xdoc.CreateElement(name);

            XmlCDataSection CData = xdoc.CreateCDataSection(content);

            snode.AppendChild((XmlNode)CData);

            fathernode.AppendChild(snode);

            return snode;
        }

        #region 通过xml和xslt数据得到html
        /// <summary>
        /// 通过xml和xslt数据得到html 代码
        /// </summary>
        /// <param name="xmlData">xml字符串</param>
        /// <param name="xsltData">xslt字符串</param>
        /// <returns></returns>
        public static string GetHTMLByXmlAndXsltData(string xmlData, string xsltData)
        {
            try
            {
                TextReader tr = new StringReader(xsltData);
                XmlTextReader xmlTextReader = new XmlTextReader(tr);
                StringReader strReader = new StringReader(xmlData);

                XslCompiledTransform tgXslt = new XslCompiledTransform();
                XmlDocument tgXml = new XmlDocument();

                tgXslt.Load(xmlTextReader);
                tgXml.Load(strReader);

                MemoryStream t = new MemoryStream();
                tgXslt.Transform(tgXml, null, t);
                string resultString = Encoding.Default.GetString(t.ToArray()).Trim();
                resultString = resultString.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "");
                return resultString;
            }
            catch
            {
                return "";
            }

        }

        /// <summary>
        /// 通过xml文件和xslt文件得到html 代码保存到指定文件夹
        /// </summary>
        /// <param name="xmlFileName"></param>
        /// <param name="xsltFileName"></param>
        /// <param name="htmlFilePath"></param>
        /// <returns></returns>
        public static bool WriteHTMLByXmlAndXsltData(string xmlFileName, string xsltFileName, string htmlFilePath)
        {
            try
            {
                XslCompiledTransform transform = new XslCompiledTransform();
                transform.Load(xsltFileName);
                transform.Transform(xmlFileName, htmlFilePath);
                return true;

            }
            catch (Exception exp)
            {
                throw exp;
                //rtnReason = exp.ToString();
                //return false;
            }
        }

        /// <summary>
        /// 通过xml数据和xslt文件得到html保存到指定地址
        /// </summary>
        /// <param name="xdoc"></param>
        /// <param name="xsltFileName"></param>
        /// <param name="htmlFilePath"></param>
        /// <returns></returns>
        public static bool GetHtmlFileByXmlStreamAndXsltFile(XmlDocument xdoc, string xsltFileName, string htmlFilePath)
        {
            try
            {
                /// Check Dir is exist ? if not create it .
                string fileDir = "";
                if (htmlFilePath.LastIndexOf("\\") != -1)
                    fileDir = htmlFilePath.Substring(0, htmlFilePath.LastIndexOf("\\"));
                if (!Directory.Exists(fileDir))
                    Directory.CreateDirectory(fileDir);
                FileInfo file = new FileInfo(htmlFilePath);
                XslCompiledTransform xslTrans = new XslCompiledTransform();
                xslTrans.Load(xsltFileName);
                //将转换的结果保存在内存流中，然后读出到字符串，返回
                StreamWriter writer = file.CreateText();
                xslTrans.Transform(xdoc, null, writer);
                writer.Close();
                return true;
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }
        #endregion 
    }
    public class XMLBuilder
    {

        private string xmlFilePath;
        private enumXmlPathType xmlFilePathType;
        private XmlDocument xmlDoc = new XmlDocument();

        public string XmlFilePath
        {
            set
            {
                xmlFilePath = value;

            }
        }

        public enumXmlPathType XmlFilePathTyp
        {
            set
            {
                xmlFilePathType = value;
            }
        }

        public XMLBuilder(string tempXmlFilePath)
        {
            this.xmlFilePathType = enumXmlPathType.VirtualPath;
            this.xmlFilePath = tempXmlFilePath;
            GetXmlDocument();
            //xmlDoc.Load( xmlFilePath ) ;
        }

        public XMLBuilder(string tempXmlFilePath, enumXmlPathType tempXmlFilePathType)
        {
            this.xmlFilePathType = tempXmlFilePathType;
            this.xmlFilePath = tempXmlFilePath;
            GetXmlDocument();
        }

        /// </summary>
        /// <param name="strEntityTypeName">实体类的名称</param>
        /// <returns>指定的XML描述文件的路径</returns>
        public XmlDocument GetXmlDocument()
        {
            XmlDocument doc = null;

            if (this.xmlFilePathType == enumXmlPathType.AbsolutePath)
            {
                doc = GetXmlDocumentFromFile(xmlFilePath);
            }
            else if (this.xmlFilePathType == enumXmlPathType.VirtualPath)
            {
                doc = GetXmlDocumentFromFile(HttpContext.Current.Server.MapPath(xmlFilePath));
            }
            return doc;
        }

        private XmlDocument GetXmlDocumentFromFile(string tempXmlFilePath)
        {
            string xmlFileFullPath = tempXmlFilePath;

            xmlDoc.Load(xmlFileFullPath);
            return xmlDoc;
        }

        #region 读取指定节点的指定属性值

        /// <summary>
        /// 功能:读取指定节点的指定属性值
        /// 参数:
        /// 参数一:节点名称
        /// 参数二:此节点的属性
        /// </summary>
        /// <param name="strNode"></param>
        /// <param name="strAttribute"></param>
        /// <returns></returns>
        public string GetXmlNodeValue(string strNode, string strAttribute)
        {
            string strReturn = "";
            try
            {
                //根据指定路径获取节点
                XmlNode xmlNode = xmlDoc.SelectSingleNode(strNode);
                //获取节点的属性，并循环取出需要的属性值
                XmlAttributeCollection xmlAttr = xmlNode.Attributes;

                for (int i = 0; i < xmlAttr.Count; i++)
                {
                    if (xmlAttr.Item(i).Name == strAttribute)
                        strReturn = xmlAttr.Item(i).Value;
                }
            }
            catch (XmlException xmle)
            {
                throw xmle;
            }
            return strReturn;
        }
        #endregion

        #region 读取指定节点的值
        /// <summary>
        /// 功能:读取指定节点的值
        /// 参数:
        /// 参数:节点名称
        /// </summary>
        /// <param name="strNode"></param>
        /// <returns></returns>
        public string GetXmlNodeValue(string strNode)
        {
            string strReturn = String.Empty;
            try
            {
                //根据路径获取节点
                XmlNode xmlNode = xmlDoc.SelectSingleNode(strNode);
                if (xmlNode != null)
                    strReturn = xmlNode.InnerText;
            }
            catch (XmlException xmle)
            {
                System.Console.WriteLine(xmle.Message);
            }
            return strReturn;
        }
        #endregion

        public string GetXmlNodeOuterXml(string strNode)
        {
            string strReturn = String.Empty;
            try
            {
                //根据路径获取节点
                XmlNode xmlNode = xmlDoc.SelectSingleNode(strNode);
                if (xmlNode != null)
                    strReturn = xmlNode.OuterXml;
            }
            catch (XmlException xmle)
            {
                System.Console.WriteLine(xmle.Message);
            }
            return strReturn;
        }

        #region 设置节点值
        /// <summary>
        /// 功能:设置节点值
        /// 参数:
        ///    参数一:节点的名称
        ///    参数二:节点值
        /// </summary>
        /// <param name="strNode"></param>
        /// <param name="newValue"></param>
        public void SetXmlNodeValue(string xmlNodePath, string xmlNodeValue)
        {
            try
            {
                //根据指定路径获取节点
                XmlNode xmlNode = xmlDoc.SelectSingleNode(xmlNodePath);
                //设置节点值
                xmlNode.InnerText = xmlNodeValue;
            }
            catch (XmlException xmle)
            {
                throw xmle;
            }
        }
        #endregion

        #region 设置节点的属性值
        /// <summary>
        /// 功能:
        /// 设置节点的属性值
        /// 参数:
        /// 参数一:节点名称
        /// 参数二:属性名称
        /// 参数三:属性值
        /// </summary>
        /// <param name="xmlNodePath"></param>
        /// <param name="xmlNodeAttribute"></param>
        /// <param name="xmlNodeAttributeValue"></param>
        public void SetXmlNodeValue(string xmlNodePath, string xmlNodeAttribute, string xmlNodeAttributeValue)
        {
            try
            {
                //根据指定路径获取节点
                XmlNode xmlNode = xmlDoc.SelectSingleNode(xmlNodePath);
                //获取节点的属性，并循环取出需要的属性值
                XmlAttributeCollection xmlAttr = xmlNode.Attributes;
                for (int i = 0; i < xmlAttr.Count; i++)
                {
                    if (xmlAttr.Item(i).Name == xmlNodeAttribute)
                    {
                        xmlAttr.Item(i).Value = xmlNodeAttributeValue;
                        break;
                    }
                }
            }
            catch (XmlException xmle)
            {
                throw xmle;
            }
        }
        #endregion

        /// <summary>
        /// 获取XML文件的根元素
        /// </summary>
        public XmlNode GetXmlRoot()
        {
            return xmlDoc.DocumentElement;
        }

        /// <summary>
        /// 在根节点下添加父节点
        /// </summary>
        public void AddParentNode(string parentNode)
        {
            XmlNode root = GetXmlRoot();
            XmlNode parentXmlNode = xmlDoc.CreateElement(parentNode);
            root.AppendChild(parentXmlNode);
        }

        /// <summary>
        /// 向一个已经存在的父节点中插入一个子节点
        /// </summary>
        public void AddChildNode(string parentNodePath, string childNodePath)
        {
            XmlNode parentXmlNode = xmlDoc.SelectSingleNode(parentNodePath);
            XmlNode childXmlNode = xmlDoc.CreateElement(childNodePath);
            parentXmlNode.AppendChild(childXmlNode);
        }

        public void AddChildNode(string parentNodePath, string childNodePath, string innerXmlText)
        {
            XmlNode parentXmlNode = xmlDoc.SelectSingleNode(parentNodePath);
            XmlNode childXmlNode = xmlDoc.CreateElement(childNodePath);
            parentXmlNode.AppendChild(childXmlNode);
            XmlNode theNode = xmlDoc.SelectSingleNode(parentNodePath + "/" + childNodePath);
            theNode.InnerXml = innerXmlText;
        }

        /// <summary>
        /// 向一个节点添加属性
        /// </summary>
        public void AddAttribute(string NodePath, string NodeAttribute)
        {
            XmlAttribute nodeAttribute = xmlDoc.CreateAttribute(NodeAttribute);
            XmlNode nodePath = xmlDoc.SelectSingleNode(NodePath);
            nodePath.Attributes.Append(nodeAttribute);
        }

        /// <summary>
        /// 删除一个节点的属性
        /// </summary>
        public void DeleteAttribute(string NodePath, string NodeAttribute, string NodeAttributeValue)
        {
            XmlNodeList nodePath = xmlDoc.SelectSingleNode(NodePath).ChildNodes;

            foreach (XmlNode xn in nodePath)
            {
                XmlElement xe = (XmlElement)xn;
                if (xe.GetAttribute(NodeAttribute) == NodeAttributeValue)
                {
                    xe.RemoveAttribute(NodeAttribute);//删除属性
                }
            }
        }

        /// <summary>
        /// 删除一个节点
        /// </summary>
        public void DeleteXmlNode(string tempXmlNode)
        {
            //  XmlNodeList     xmlNodePath = xmlDoc.SelectSingleNode(tempXmlNode).ChildNodes;
            XmlNode xmlNodePath = xmlDoc.SelectSingleNode(tempXmlNode);
            if (xmlNodePath != null)
            {
                xmlNodePath.ParentNode.RemoveChild(xmlNodePath);
                foreach (XmlNode xn in xmlNodePath)
                {
                    XmlElement xe = (XmlElement)xn;
                    xe.RemoveAll();
                    //xe.RemoveChild(xn);
                    xn.RemoveAll();
                    if (xe.HasChildNodes)
                    {
                        foreach (XmlNode xnode in xe)
                        {

                            xnode.RemoveAll();//删除所有子节点和属性
                        }
                    }
                }
            }
        }

        #region 保存XML文件
        /// <summary>
        /// 功能: 
        /// 保存XML文件
        /// 
        /// </summary>
        public void SaveXmlDocument()
        {
            try
            {
                //保存设置的结果

                if (this.xmlFilePathType == enumXmlPathType.AbsolutePath)
                {
                    xmlDoc.Save(xmlFilePath);
                }
                else if (this.xmlFilePathType == enumXmlPathType.VirtualPath)
                {
                    xmlDoc.Save(HttpContext.Current.Server.MapPath(xmlFilePath));
                }
               
                
            }
            catch (XmlException xmle)
            {
                throw xmle;
            }
        }
        #endregion
        
        #region 保存XML文件
        /// <summary>
        /// 功能: 
        /// 保存XML文件
        /// 
        /// </summary>
        public void SaveXmlDocument(string tempXMLFilePath)
        {
            try
            {
                //保存设置的结果
                xmlDoc.Save(tempXMLFilePath);
            }
            catch (XmlException xmle)
            {
                throw xmle;
            }
        }
        #endregion
    }

    public enum enumXmlPathType
    {
        AbsolutePath,
        VirtualPath
    }
}
