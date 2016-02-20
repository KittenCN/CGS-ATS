using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RM.Common.DotNetBean;
using RM.Common.DotNetUI;
using RM.Web.App_Code;

namespace RM.Web.RMBase.SysPersonal
{
    public partial class Individuation_Set : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Language_Type.Value = CookieHelper.GetCookie("Language_Type");
                WebUI_Type.Value = CookieHelper.GetCookie("WebUI_Type");
                Menu_Type.Value = CookieHelper.GetCookie("Menu_Type");
                PageIndex.Value = CookieHelper.GetCookie("PageIndex");
            }
        }
        /// <summary>
        /// 保存设置，保存在浏览器Cookie
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Save_Click(object sender, EventArgs e)
        {
            try
            {
                if (WebHelper.SubmitCheckForm())
                {
                    CookieHelper.WriteCookie("Language_Type", Language_Type.Value, 30);
                    CookieHelper.WriteCookie("WebUI_Type", WebUI_Type.Value, 30);
                    CookieHelper.WriteCookie("Menu_Type", Menu_Type.Value, 30);
                    CookieHelper.WriteCookie("PageIndex", PageIndex.Value, 30);
                    ShowMsgHelper.ShowScript("MainSwitch()");
                }
            }
            catch
            {
                ShowMsgHelper.Alert_Error("设置失败！");
            }
        }
    }
}