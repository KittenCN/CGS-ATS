using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RM.Common.DotNetUI;
using System.IO;
using RM.Common.DotNetConfig;
using System.Text;
using RM.Web.App_Code;

namespace RM.Web.RMBase.SysLog
{
    public partial class SysLog_Content : PageBase
    {
        public string _FileName;
        protected void Page_Load(object sender, EventArgs e)
        {
            _FileName = Request["FileName"];
            if (!IsPostBack)
            {
                if (_FileName != null)
                {
                    GetTxtValue();
                }
                else
                {
                    _FileName = "未选择文件目录";
                }
            }
        }
        public void GetTxtValue()
        {
            string filepath = ConfigHelper.GetAppSettings("LogFilePath") + "\\" + _FileName;
            if (System.IO.File.Exists(filepath))
            {
                StreamReader sr = new StreamReader(filepath, Encoding.GetEncoding("UTF-8"));//取得这txt文件的编码
                string txtvalue = sr.ReadToEnd().ToString();
                sr.Close();
                txtLog.InnerText = txtvalue;
            }
        }
        /// <summary>
        /// 清空txt数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void hlkempty_Click1(object sender, EventArgs e)
        {
            string filepath = ConfigHelper.GetAppSettings("LogFilePath") + "\\" + _FileName;
            FileStream fs = new FileStream(filepath, FileMode.Create, FileAccess.Write); fs.Close();
            ShowMsgHelper.ShowScript("Alert_Ok()");
        }
    }
}