using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using RM.Busines;
using RM.Common.DotNetUI;
using RM.Common.DotNetCode;
using RM.Common.DotNetBean;
using RM.Web.App_Code;

namespace RM.Web.RMBase.SysPersonal
{
    public partial class HomeShortcut_Form : PageBase
    {
        string _key;
        protected void Page_Load(object sender, EventArgs e)
        {
            _key = Request["key"];                  //主键
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(_key))
                {
                    InitData();
                }
            }
        }
        /// <summary>
        /// 初始化
        /// </summary>
        private void InitData()
        {
            Hashtable ht = DataFactory.SqlDataBase().GetHashtableById("Base_O_A_Setup", "Setup_ID", _key);
            if (ht.Count > 0 && ht != null)
            {
                ControlBindHelper.SetWebControls(this.Page, ht);
                if (!string.IsNullOrEmpty(ht["SETUP_IMG"].ToString()))
                {
                    Img_Setup_Img.Src = "/Themes/Images/32/" + ht["SETUP_IMG"].ToString();
                }
            }
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Save_Click(object sender, EventArgs e)
        {
            Hashtable ht = new Hashtable();
            ht = ControlBindHelper.GetWebControls(this.Page);
            if (string.IsNullOrEmpty(_key))
            {
                ht["Setup_ID"] = CommonHelper.GetGuid;
                ht["User_ID"] = RequestSession.GetSessionUser().UserId;
            }
            bool IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit("Base_O_A_Setup", "Setup_ID", _key, ht);
            if (IsOk)
                ShowMsgHelper.AlertMsg("操作成功！");
            else
                ShowMsgHelper.Alert_Error("操作失败！");
        }
    }
}