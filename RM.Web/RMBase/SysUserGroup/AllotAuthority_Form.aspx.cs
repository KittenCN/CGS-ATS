using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RM.Common.DotNetUI;
using System.Text;
using RM.Common.DotNetData;
using System.Data;
using RM.Busines;
using RM.Busines.IDAO;
using RM.Busines.DAL;
using RM.Web.App_Code;

namespace RM.Web.RMBase.SysUserGroup
{
    public partial class AllotAuthority_Form : PageBase
    {
        public StringBuilder StrTree_Menu = new StringBuilder();
        RM_System_IDAO system_idao = new RM_System_Dal();
        RM_UserInfo_IDAO user_idao = new RM_UserInfo_Dal();
        public string _UserGroup_ID, _UserGroup_Name;
        protected void Page_Load(object sender, EventArgs e)
        {
            _UserGroup_ID = Request["UserGroup_ID"];//主键
            _UserGroup_Name = Server.UrlDecode(Request["UserGroup_Name"]);//用户组名称
            GetMenuTreeTable();
        }
        #region 初始化权限树列表
        /// <summary>
        /// 菜单树列表
        /// </summary>
        public void GetMenuTreeTable()
        {
            DataTable dtList = system_idao.GetMenuBind();
            DataTable dtUserGroupRight = user_idao.InitUserGroupRight(_UserGroup_ID);
            if (DataTableHelper.IsExistRows(dtList))
            {
                DataTable dtButoon = DataTableHelper.GetNewDataTable(dtList, "Menu_Type = '3'");
                DataTable dtMenu = DataTableHelper.GetNewDataTable(dtList, "Menu_Type < '3'");
                DataView dv = new DataView(dtMenu);
                dv.RowFilter = " ParentId = '0'";
                int eRowIndex = 0;
                foreach (DataRowView drv in dv)
                {
                    string trID = "node-" + eRowIndex.ToString();
                    StrTree_Menu.Append("<tr id='" + trID + "'>");
                    StrTree_Menu.Append("<td style='width: 200px;padding-left:20px;'><span class=\"folder\">" + drv["Menu_Name"] + "</span></td>");
                    if (!string.IsNullOrEmpty(drv["Menu_Img"].ToString()))
                        StrTree_Menu.Append("<td style='width: 30px;text-align:center;'><img src='/Themes/images/32/" + drv["Menu_Img"] + "' style='width:16px; height:16px;vertical-align: middle;' alt=''/></td>");
                    else
                        StrTree_Menu.Append("<td style='width: 30px;text-align:center;'><img src='/Themes/images/32/5005_flag.png' style='width:16px; height:16px;vertical-align: middle;' alt=''/></td>");
                    StrTree_Menu.Append("<td style=\"width: 23px; text-align: left;\"><input id='ckb" + trID + "' onclick=\"ckbValueObj(this.id)\" style='vertical-align: middle;margin-bottom:2px;' type=\"checkbox\" " + GetChecked(drv["Menu_Id"].ToString(), dtUserGroupRight) + "  value=\"" + drv["Menu_Id"] + "\" name=\"checkbox\" /></td>");
                    StrTree_Menu.Append("<td>" + GetButton(drv["Menu_Id"].ToString(), dtButoon, trID, dtUserGroupRight) + "</td>");
                    StrTree_Menu.Append("</tr>");
                    //创建子节点
                    StrTree_Menu.Append(GetTableTreeNode(drv["Menu_Id"].ToString(), dtMenu, trID, dtButoon, dtUserGroupRight));
                    eRowIndex++;
                }
            }
        }
        /// <summary>
        /// 创建子节点
        /// </summary>
        /// <param name="parentID">父节点主键</param>
        /// <param name="dtMenu"></param>
        /// <returns></returns>
        public string GetTableTreeNode(string parentID, DataTable dtMenu, string parentTRID, DataTable dtButoon, DataTable dtUserGroupRight)
        {
            StringBuilder sb_TreeNode = new StringBuilder();
            DataView dv = new DataView(dtMenu);
            dv.RowFilter = "ParentId = '" + parentID + "'";
            int i = 1;
            foreach (DataRowView drv in dv)
            {
                string trID = parentTRID + "-" + i.ToString();
                sb_TreeNode.Append("<tr id='" + trID + "' class='child-of-" + parentTRID + "'>");
                sb_TreeNode.Append("<td style='padding-left:20px;'><span class=\"folder\">" + drv["Menu_Name"].ToString() + "</span></td>");
                if (!string.IsNullOrEmpty(drv["Menu_Img"].ToString()))
                    sb_TreeNode.Append("<td style='width: 30px;text-align:center;'><img src='/Themes/images/32/" + drv["Menu_Img"].ToString() + "' style='width:16px; height:16px;vertical-align: middle;' alt=''/></td>");
                else
                    sb_TreeNode.Append("<td style='width: 30px;text-align:center;'><img src='/Themes/images/32/5005_flag.png' style='width:16px; height:16px;vertical-align: middle;' alt=''/></td>");
                sb_TreeNode.Append("<td style=\"width: 23px; text-align: left;\"><input id='ckb" + trID + "' onclick=\"ckbValueObj(this.id)\" " + GetChecked(drv["Menu_Id"].ToString(), dtUserGroupRight) + " style='vertical-align: middle;margin-bottom:2px;' type=\"checkbox\" value=\"" + drv["Menu_Id"] + "\" name=\"checkbox\" /></td>");
                sb_TreeNode.Append("<td>" + GetButton(drv["Menu_Id"].ToString(), dtButoon, trID, dtUserGroupRight) + "</td>");
                sb_TreeNode.Append("</tr>");
                //创建子节点
                sb_TreeNode.Append(GetTableTreeNode(drv["Menu_Id"].ToString(), dtMenu, trID, dtButoon, dtUserGroupRight));
                i++;
            }
            return sb_TreeNode.ToString();
        }
        /// <summary>
        /// 获取导航菜单所属按钮
        /// </summary>
        /// <param name="Menu_Type">类型</param>
        /// <returns></returns>
        public string GetButton(string Menu_Id, DataTable dt, string parentTRID, DataTable dtRoleRight)
        {
            StringBuilder ButtonHtml = new StringBuilder(); ;
            DataTable dt_Button = DataTableHelper.GetNewDataTable(dt, "ParentId = '" + Menu_Id + "'");
            if (DataTableHelper.IsExistRows(dt_Button))
            {
                int i = 1;
                foreach (DataRow drv in dt_Button.Rows)
                {
                    string trID = parentTRID + "--" + i.ToString();
                    ButtonHtml.Append("<lable><input id='ckb" + trID + "' onclick=\"ckbValueObj(this.id)\" " + GetChecked(drv["Menu_Id"].ToString(), dtRoleRight) + " style='vertical-align: middle;margin-bottom:2px;' type=\"checkbox\" value=\"" + drv["Menu_Id"] + "\" name=\"checkbox\" />");
                    ButtonHtml.Append("" + drv["Menu_Name"].ToString() + "</lable>&nbsp;&nbsp;&nbsp;&nbsp;");
                    i++;
                }
                return ButtonHtml.ToString();
            }
            return ButtonHtml.ToString();
        }
        /// <summary>
        /// 验证权限是否存在
        /// </summary>
        /// <param name="Menu_Id">权限主键</param>
        /// <param name="dt">加载所属用户权限</param>
        /// <returns></returns>
        public string GetChecked(string Menu_Id, DataTable dt)
        {
            StringBuilder strSql = new StringBuilder();
            dt = DataTableHelper.GetNewDataTable(dt, "Menu_Id = '" + Menu_Id + "'");
            if (DataTableHelper.IsExistRows(dt))
                return "checked=\"checked\"";
            else
                return "";
        }
        #endregion
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Save_Click(object sender, EventArgs e)
        {
            string str = item_hidden.Value;
            bool IsAllto = user_idao.Add_UserGroupAllotAuthority(str.Split(','), _UserGroup_ID);
            if (IsAllto)
                ShowMsgHelper.ShowScript("back();showTipsMsg('操作成功！','2500','4');");
            else
                ShowMsgHelper.Alert_Error("操作失败！");
        }
    }
}