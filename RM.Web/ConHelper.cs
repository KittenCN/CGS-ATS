using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Xml;

namespace RM.Web
{
    public class ConHelper
    {
        public static string host = "mail.intercoop.com";
        public static string uname = "hrtest";
        public static string pwd = "qq@1234";
        public static string sender = "hrtest@coopgs.com";

        public ConHelper()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //           
        }

        private string strhost;
        public string strHost
        {
            get { return strhost; }
            set { strhost = value; }
        }

        private string struname;
        public string strUname
        {
            get { return struname; }
            set { struname = value; }
        }

        private string strpwd;
        public string strPWD
        {
            get { return strpwd; }
            set { strpwd = value; }
        }

        private string strsender;
        public string strSender
        {
            get { return strsender; }
            set { strsender = value; }
        }

        public void GetMailSetting()
        {
            //读取配置文件config.xml
            //string tt = HttpRuntime.AppDomainAppPath.ToString();
            if (File.Exists(HttpRuntime.AppDomainAppPath.ToString() + "MailSetting.config"))
            {
                try
                {
                    XmlDocument xmlCon = new XmlDocument();
                    xmlCon.Load(HttpRuntime.AppDomainAppPath.ToString() + "MailSetting.config");
                    XmlNode xnCon = xmlCon.SelectSingleNode("configuration");
                    strHost = xnCon.SelectSingleNode("host").InnerText;
                    strUname = xnCon.SelectSingleNode("uname").InnerText;
                    strPWD = xnCon.SelectSingleNode("pwd").InnerText;
                    strSender = xnCon.SelectSingleNode("sender").InnerText;
                }
                catch
                {
                    strHost = host;
                    strUname = uname;
                    strPWD = pwd;
                    strSender = sender;
                }
            }
            else
            {
                strHost = host;
                strUname = uname;
                strPWD = pwd;
                strSender = sender;
            }
        }

    }
}