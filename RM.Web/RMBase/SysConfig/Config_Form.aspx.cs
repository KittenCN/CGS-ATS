using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using RM.Busines.IDAO;
using RM.Busines.DAL;
using System.Xml;
using RM.Common.DotNetUI;
using RM.Common.DotNetConfig;
using RM.Web.App_Code;

namespace RM.Web.RMBase.SysConfig
{
    public partial class Config_Form : PageBase
    {
        public StringBuilder str_OutputHtml = new StringBuilder();//附加信息
        string Property_Function = "系统配置信息";
        RM_System_IDAO systemidao = new RM_System_Dal();
        protected void Page_Load(object sender, EventArgs e)
        {
            str_OutputHtml.Append(systemidao.AppendProperty_Html(Property_Function));
            GetValue();
        }
        public void GetValue()
        {
            string returnValue = "";
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(Server.MapPath("/Config.xml"));
            XmlNodeList xmlNodeList = xmlDocument.SelectNodes("//appSettings/add");
            foreach (XmlNode xmlNode in xmlNodeList)
            {
                returnValue += xmlNode.Attributes["key"].Value + "∫" + xmlNode.Attributes["value"].Value + "∮";
            }
            AppendProperty_value.Value = returnValue;
            //XmlDocument webconfigDoc = new XmlDocument();
            //string filePath = Server.MapPath("/Config.xml");
            //string xPath = "/appSettings/add[@key='?']";
            //webconfigDoc.Load(filePath);
            //XmlNode passkey = webconfigDoc.SelectSingleNode(xPath.Replace("?", "Version"));
            //passkey.Attributes["value"].InnerText = "5555555555";
            //webconfigDoc.Save(filePath);
        }
    }
}