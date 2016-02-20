using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RM.Busines.IDAO;
using System.Text;
using RM.Busines.DAL;
using System.Data;
using RM.Common.DotNetData;
using RM.Common.DotNetUI;
using RM.Web.App_Code;

namespace RM.Web.RMBase.SysMenu
{
    public partial class AllotButton_Form : PageBase
    {
        RM_System_IDAO systemidao = new RM_System_Dal();
        public StringBuilder ButtonList = new StringBuilder();
        public StringBuilder selectedButtonList = new StringBuilder();
        public string _ParentId;
        protected void Page_Load(object sender, EventArgs e)
        {
            _ParentId = Request["key"];                  //主键
            InitButtonList();
        }
        /// <summary>
        /// 初始化按钮
        /// </summary>
        public void InitButtonList()
        {
            //所有按钮
            DataTable dt = systemidao.GetButtonList();
            if (DataTableHelper.IsExistRows(dt))
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ButtonList.Append("<div id=" + dt.Rows[i]["Button_ID"].ToString() + " onclick='selectedButton(this)' title='" + dt.Rows[i]["Button_Name"].ToString() + "' class=\"shortcuticons\"><img src=\"/Themes/Images/16/" + dt.Rows[i]["Button_Img"].ToString() + "\" alt=\"\" /><br />" + dt.Rows[i]["Button_Name"].ToString() + "</div>");
                }
            }
            //已选按钮
            DataTable dt_selectedButton = systemidao.GetSysMenuByButton(_ParentId);
            if (DataTableHelper.IsExistRows(dt_selectedButton))
            {
                for (int i = 0; i < dt_selectedButton.Rows.Count; i++)
                {
                    selectedButtonList.Append("<div onclick='selectedButton(this)' ondblclick=\"removeButton('" + dt_selectedButton.Rows[i]["Menu_Id"] + "')\" title='" + dt_selectedButton.Rows[i]["Menu_Title"].ToString() + "' class=\"shortcuticons\"><img src=\"/Themes/Images/16/" + dt_selectedButton.Rows[i]["Menu_Img"].ToString() + "\" alt=\"\" /><br />" + dt_selectedButton.Rows[i]["Menu_Name"].ToString() + "</div>");
                }
            }
        }
    }
}