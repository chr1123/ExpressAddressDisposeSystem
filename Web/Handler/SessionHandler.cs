using EADS.Common;
using System;
using EADS.BLL;
using EADS.Model;
using System.Xml;
using System.Collections.Generic;
using System.Web;

namespace EADS.Web.Handler
{
    /// <summary>
    /// 负责文件处理相关业务
    /// </summary>
    public class SessionHandler
    {
      
        public string getXmlStrngByList(List<Model_Order> orderList)
        {
            XmlDocument xmldoc = new XmlDocument();
            //加入XML的声明段落,<?xml version="1.0" encoding="gb2312"?>
            XmlDeclaration xmldecl;
            xmldecl = xmldoc.CreateXmlDeclaration("1.0", "UTF-8",null); 
            xmldoc.AppendChild(xmldecl); 
            //加入list根元素
            XmlElement xmlList = xmldoc.CreateElement("", "list", "http://www.yundaex.com/cn/index.php");
            xmlList.SetAttribute("auxids", "recvDiscMsg");
            xmldoc.AppendChild(xmlList);
            //company  timestamp  token
            XmlElement company = xmldoc.CreateElement("company");
            company.InnerText = "999999";
            xmlList.AppendChild(company);
            XmlElement timestamp = xmldoc.CreateElement("timestamp");
            timestamp.InnerText = "999999";
            xmlList.AppendChild(timestamp);
            XmlElement token = xmldoc.CreateElement("token");
            token.InnerText = "999999";
            xmlList.AppendChild(token);
             
            //加入order元素
            for (int i = 0; i < orderList.Count; i++)
            {
                Model_Order order = orderList[i];
               // XmlNode root = xmldoc.SelectSingleNode("list");//查找<Employees> 
                XmlElement xeItem = xmldoc.CreateElement("item");//创建一个<item>节点 

                XmlElement xeId = xmldoc.CreateElement("id");//创建一个<item>节点 
                xeId.InnerText = order.OrderNO;
                XmlElement xeFileType = xmldoc.CreateElement("fieldType");//创建一个<item>节点 
                xeFileType.InnerText = "1";
                XmlElement xeResultCode = xmldoc.CreateElement("resultCode");//创建一个<item>节点 
                xeResultCode.InnerText = "0";
                XmlElement xeFieldInfo0 = xmldoc.CreateElement("fieldInfo0");//创建一个<item>节点 
                xeFieldInfo0.InnerText = HttpUtility.UrlEncode(order.DestinationCity);
                XmlElement xeFieldInfo1 = xmldoc.CreateElement("fieldInfo1");//创建一个<item>节点 
                xeFieldInfo1.InnerText = HttpUtility.UrlEncode(order.RecipientAddress);
                xeItem.AppendChild(xeId);
                xeItem.AppendChild(xeFileType);
                xeItem.AppendChild(xeResultCode);
                xeItem.AppendChild(xeFieldInfo0);
                xeItem.AppendChild(xeFieldInfo1); 
                xmlList.AppendChild(xeItem); 
            } 
            string result = xmldoc.InnerXml;
            return result;
        }

    }
}