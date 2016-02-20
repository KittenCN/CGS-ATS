using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using RM.Busines;
using RM.Common.DotNetUI;
using System.Text;
using System.Data;
using RM.Common.DotNetData;
using RM.Common.DotNetBean;
using RM.Common.DotNetCode;
using RM.Busines.DAL;
using RM.Busines.IDAO;
using RM.Web.App_Code;

namespace RM.Web.RMBase.SysRole
{
    public partial class Role_Form : PageBase
    {
        public StringBuilder str_allUserInfo = new StringBuilder();
        public StringBuilder str_seleteUserInfo = new StringBuilder();
        RM_UserInfo_IDAO user_idao = new RM_UserInfo_Dal();
        RM_System_IDAO system_idao = new RM_System_Dal();
        string _key, _ParentId;
        protected void Page_Load(object sender, EventArgs e)
        {
            _key = Request["key"];                  //主键
            _ParentId = Request["ParentId"];        //父节点
            InitUserInfo();
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
            Hashtable ht = DataFactory.SqlDataBase().GetHashtableById("Base_Roles", "Roles_ID", _key);
            if (ht.Count > 0 && ht != null)
            {
                ControlBindHelper.SetWebControls(this.Page, ht);
            }
        }
        /// <summary>
        /// 节点位置下拉框绑定
        /// </summary>
        /// <param name="projectid"></param>
        /// <returns></returns>
        private void InitParentId()
        {
            DataTable dt = system_idao.InitRoleParentId();
            if (!string.IsNullOrEmpty(_key))
            {
                if (DataTableHelper.IsExistRows(dt))
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["Roles_ID"].ToString() == _key)
                            dt.Rows.RemoveAt(i);
                    }
                }
            }
            ControlBindHelper.BindHtmlSelect(dt, ParentId, "Roles_Name", "Roles_ID", "角色信息 - 父节");
        }

        #region 初始化角色成员
        /// <summary>
        /// 所属角色成员
        /// </summary>
        public void InitUserRole()
        {
            if (!string.IsNullOrEmpty(_key))
            {
                DataTable dt = system_idao.InitUserRole(_key);
                if (DataTableHelper.IsExistRows(dt))
                {
                    foreach (DataRow drv in dt.Rows)
                    {
                        str_seleteUserInfo.Append("<tr ondblclick='$(this).remove()'><td>" + drv["User_Name"] + "</td><td>" + drv["Organization_Name"] + "</td><td  style='display:none'>" + drv["User_ID"] + "</td></tr>");
                    }
                }
            }
        }
        /// <summary>
        /// 所有成员
        /// </summary>
        public void InitUserInfo()
        {
            InitUserRole();
            DataTable dt_Org = user_idao.Load_StaffOrganizeList();
            DataView dv = new DataView(dt_Org);
            dv.RowFilter = "ParentId = '0'";
            foreach (DataRowView drv in dv)
            {
                DataTable GetNewData = DataTableHelper.GetNewDataTable(dt_Org, "ParentId = '" + drv["Organization_ID"].ToString() + "'");
                if (DataTableHelper.IsExistRows(GetNewData))
                {
                    str_allUserInfo.Append("<li>");
                    str_allUserInfo.Append("<div>" + drv["Organization_Name"].ToString() + "</div>");
                    //创建子节点
                    str_allUserInfo.Append(GetTreeNode(drv["Organization_ID"].ToString(), drv["Organization_Name"].ToString(), dt_Org, "1"));
                    str_allUserInfo.Append("</li>");
                }
            }
        }
        int index_TreeNode = 0;
        /// <summary>
        /// 创建子节点
        /// </summary>
        /// <param name="parentID">父节点主键</param>
        /// <param name="dtMenu"></param>
        /// <returns></returns>
        public string GetTreeNode(string parentID, string parentName, DataTable dtNode, string status)
        {
            StringBuilder sb_TreeNode = new StringBuilder();
            DataTable GetNewData = new DataTable();
            DataView dv = new DataView(dtNode);
            dv.RowFilter = "ParentId = '" + parentID + "'";
            if (dv.Count > 0)
            {
                if (index_TreeNode == 0)
                {
                    sb_TreeNode.Append("<ul>");
                }
                else
                {
                    sb_TreeNode.Append("<ul style='display: none'>");
                }
                foreach (DataRowView drv in dv)
                {
                    GetNewData = DataTableHelper.GetNewDataTable(dtNode, "ParentId = '" + drv["Organization_ID"].ToString() + "'");
                    if (drv["isUser"].ToString() == "0")//判断是否是成员 1：是成员
                    {
                        if (DataTableHelper.IsExistRows(GetNewData))
                        {
                            sb_TreeNode.Append("<li>");
                            sb_TreeNode.Append("<div>" + drv["Organization_Name"] + "</div>");
                            //创建子节点
                            sb_TreeNode.Append(GetTreeNode(drv["Organization_ID"].ToString(), drv["Organization_Name"].ToString(), dtNode, "2"));
                            sb_TreeNode.Append("</li>");
                        }
                    }
                    else
                    {
                        if (status != "1")
                        {
                            sb_TreeNode.Append("<li>");
                            sb_TreeNode.Append("<div ondblclick=\"addUserInfo('" + drv["Organization_Name"] + "','" + drv["Organization_ID"] + "','" + parentName + "')\">");
                            sb_TreeNode.Append("<img src=\"/Themes/Images/user_mature.png\" width=\"16\" height=\"16\" />" + drv["Organization_Name"].ToString() + "</div>");
                            sb_TreeNode.Append("</li>");
                        }
                    }
                }
                sb_TreeNode.Append("</ul>");
            }
            index_TreeNode++;
            return sb_TreeNode.ToString();
        }
        #endregion

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Save_Click(object sender, EventArgs e)
        {
            string guid = CommonHelper.GetGuid;
            Hashtable ht = new Hashtable();
            ht = ControlBindHelper.GetWebControls(this.Page);
            ht.Remove("User_ID_Hidden");
            if (ParentId.Value == "")
            {
                ht["ParentId"] = "0";
            }
            if (!string.IsNullOrEmpty(_key))
            {
                guid = _key;
                ht["ModifyDate"] = DateTime.Now;
                ht["ModifyUserId"] = RequestSession.GetSessionUser().UserId;
                ht["ModifyUserName"] = RequestSession.GetSessionUser().UserName;
            }
            else
            {
                ht["Roles_ID"] = guid;
                ht["CreateUserId"] = RequestSession.GetSessionUser().UserId;
                ht["CreateUserName"] = RequestSession.GetSessionUser().UserName;
            }
            bool IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit("Base_Roles", "Roles_ID", _key, ht);
            if (IsOk)
            {
                string str = User_ID_Hidden.Value;
                if (!string.IsNullOrEmpty(str))
                {
                    str = User_ID_Hidden.Value.Substring(0, User_ID_Hidden.Value.Length - 1);
                }
                bool IsAllto = system_idao.Add_RoleAllotMember(str.Split(','), guid);
                if (IsAllto)
                {
                    ShowMsgHelper.AlertMsg("操作成功！");
                }
                else
                {
                    ShowMsgHelper.Alert_Error("操作失败！");
                }
            }
            else
            {
                ShowMsgHelper.Alert_Error("操作失败！");
            }
        }
    }
}