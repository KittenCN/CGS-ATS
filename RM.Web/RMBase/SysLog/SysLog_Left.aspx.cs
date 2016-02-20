using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using RM.Common.DotNetConfig;
using System.IO;
using RM.Web.App_Code;

namespace RM.Web.RMBase.SysLog
{
    public partial class SysLog_Left : PageBase
    {
        public StringBuilder strHtml = new StringBuilder();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitInfo();
            }
        }
        /// <summary>
        /// 所属功能
        /// </summary>
        public void InitInfo()
        {
            string LogFilePath = ConfigHelper.GetAppSettings("LogFilePath");
            DirectoryInfo dir = new DirectoryInfo(LogFilePath);
            foreach (FileInfo fsi in dir.GetFileSystemInfos())
            {
                if (fsi.Name != "Backup_Restore_Log.log")
                {
                    strHtml.Append("<li>");
                    strHtml.Append("<div title='" + fsi.Name + "' onclick=\"FileName('" + fsi.Name + "')\">" + fsi.Name + "</div>");
                    strHtml.Append("</li>");
                }
            }
        }
    }
}