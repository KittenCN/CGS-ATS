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
using System.Data;
using RM.Common.DotNetData;
using System.Text;
using RM.Busines.DAL;
using RM.Busines.IDAO;
using RM.Web.App_Code;

namespace RM.Web.RMBase.SysMenu
{
    public partial class Menu_Form : PageBase
    {
        string  _key, _ParentId;
        protected void Page_Load(object sender, EventArgs e)
        {
            _key = Request["key"];                  //主键
            _ParentId = Request["ParentId"];        //父节点
            if (!IsPostBack)
            {
                InitParentId();
                if (!string.IsNullOrEmpty(_ParentId))
                {
                    ParentId.Value = _ParentId;
                }
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
            Hashtable ht = DataFactory.SqlDataBase().GetHashtableById("Base_SysMenu", "Menu_Id", _key);
            if (ht.Count > 0 && ht != null)
            {
                ControlBindHelper.SetWebControls(this.Page, ht);
                if (!string.IsNullOrEmpty(ht["MENU_IMG"].ToString()))
                {
                    Img_Menu_Img.Src = "/Themes/Images/32/" + ht["MENU_IMG"].ToString();
                }
            }
        }
        /// <summary>
        /// 节点位置下拉框绑定
        /// </summary>
        /// <param name="projectid"></param>
        /// <returns></returns>
        private void InitParentId()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT Menu_Id,
                            Menu_Name+' - '+CASE Menu_Type WHEN '1' THEN '父节' WHEN '2' THEN '子节' END AS Menu_Name
                            FROM Base_SysMenu WHERE DeleteMark = 1 AND Menu_Type != 3 ORDER BY SortCode ASC");
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(strSql);
            if (!string.IsNullOrEmpty(_key))
            {
                if (DataTableHelper.IsExistRows(dt))
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["Menu_Id"].ToString() == _key)
                            dt.Rows.RemoveAt(i);
                    }
                }
            }
            ControlBindHelper.BindHtmlSelect(dt, ParentId, "Menu_Name", "Menu_Id", "模块菜单 - 父节");
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
            if (ParentId.Value == "")
            {
                ht["ParentId"] = "0";
                ht["Menu_Type"] = 1;
            }
            else
            {
                ht["Menu_Type"] = 2;
            }
            if (!string.IsNullOrEmpty(_key))
            {
                ht["ModifyDate"] = DateTime.Now;
                ht["ModifyUserId"] = RequestSession.GetSessionUser().UserId;
                ht["ModifyUserName"] = RequestSession.GetSessionUser().UserName;
            }
            else
            {
                ht["Menu_Id"] = CommonHelper.GetGuid;
                ht["CreateUserId"] = RequestSession.GetSessionUser().UserId;
                ht["CreateUserName"] = RequestSession.GetSessionUser().UserName;
            }
            bool IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit("Base_SysMenu", "Menu_Id", _key, ht);
            if (IsOk)
            {
                CacheHelper.RemoveAllCache();
                ShowMsgHelper.AlertMsg("操作成功！");
            }
            else
            {
                ShowMsgHelper.Alert_Error("操作失败！");
            }
        }
    }
}