using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using RM.Busines;
using RM.Common.DotNetCode;
using RM.Busines.DAL;
using RM.Busines.IDAO;
using RM.Web.App_Code;

namespace RM.Web.RMBase.SysRole
{
    public partial class Role_List : PageBase
    {
        public StringBuilder str_tableTree = new StringBuilder();
        RM_System_IDAO systemidao = new RM_System_Dal();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetTreeTable();
            }
        }
        #region 树列表
        /// <summary>
        /// 树列表
        /// </summary>
        public void GetTreeTable()
        {

            DataTable dtRole = systemidao.InitRoleList();
            DataView dv = new DataView(dtRole);
            dv.RowFilter = "ParentId = '0'";
            int eRowIndex = 0;
            foreach (DataRowView drv in dv)
            {
                string trID = "node-" + eRowIndex.ToString();
                str_tableTree.Append("<tr id='" + trID + "'>");
                str_tableTree.Append("<td style='width: 180px;padding-left:20px;'><span class=\"folder\">" + drv["Roles_Name"].ToString() + "</span></td>");
                str_tableTree.Append("<td style='width: 60px;text-align:center;'>" + Get_Type(drv["DeleteMark"].ToString()) + "</td>");
                str_tableTree.Append("<td style='width: 60px;text-align:center;'>" + drv["SortCode"].ToString() + "</td>");
                str_tableTree.Append("<td style='width: 120px;text-align:center'>" + drv["CreateUserName"].ToString() + "</td>");
                str_tableTree.Append("<td style='width: 120px;text-align:center'>" + CommonHelper.GetFormatDateTime(drv["CreateDate"], "yyyy-MM-dd HH:mm") + "</td>");
                str_tableTree.Append("<td style='width: 120px;text-align:center'>" + drv["ModifyUserName"].ToString() + "</td>");
                str_tableTree.Append("<td style='width: 120px;text-align:center'>" + CommonHelper.GetFormatDateTime(drv["ModifyDate"].ToString(), "yyyy-MM-dd HH:mm") + "</td>");
                str_tableTree.Append("<td>" + drv["Roles_Remark"].ToString() + "</td>");
                str_tableTree.Append("<td style='display:none'>" + drv["Roles_ID"].ToString() + "</td>");
                str_tableTree.Append("</tr>");
                //创建子节点
                str_tableTree.Append(GetTableTreeNode(drv["Roles_ID"].ToString(), dtRole, trID));
                eRowIndex++;
            }
        }
        /// <summary>
        /// 创建子节点
        /// </summary>
        /// <param name="parentID">父节点主键</param>
        /// <param name="dtMenu"></param>
        /// <returns></returns>
        public string GetTableTreeNode(string parentID, DataTable dt, string parentTRID)
        {
            StringBuilder sb_TreeNode = new StringBuilder();
            DataView dv = new DataView(dt);
            dv.RowFilter = "ParentId = '" + parentID + "'";
            int i = 1;
            foreach (DataRowView drv in dv)
            {
                string trID = parentTRID + "-" + i.ToString();
                sb_TreeNode.Append("<tr id='" + trID + "' class='child-of-" + parentTRID + "'>");
                sb_TreeNode.Append("<td style='padding-left:20px;'><span class=\"folder\">" + drv["Roles_Name"].ToString() + "</span></td>");
                sb_TreeNode.Append("<td style='width: 60px;text-align:center;'>" + Get_Type(drv["DeleteMark"].ToString()) + "</td>");
                sb_TreeNode.Append("<td style='width: 60px;text-align:center;'>" + drv["SortCode"].ToString() + "</td>");
                sb_TreeNode.Append("<td style='width: 120px;text-align:center'>" + drv["CreateUserName"].ToString() + "</td>");
                sb_TreeNode.Append("<td style='width: 120px;text-align:center'>" + CommonHelper.GetFormatDateTime(drv["CreateDate"].ToString(), "yyyy-MM-dd HH:mm") + "</td>");
                sb_TreeNode.Append("<td style='width: 120px;text-align:center'>" + drv["ModifyUserName"].ToString() + "</td>");
                sb_TreeNode.Append("<td style='width: 120px;text-align:center'>" + CommonHelper.GetFormatDateTime(drv["ModifyDate"].ToString(), "yyyy-MM-dd HH:mm") + "</td>");
                sb_TreeNode.Append("<td>" + drv["Roles_Remark"].ToString() + "</td>");
                sb_TreeNode.Append("<td style='display:none'>" + drv["Roles_ID"].ToString() + "</td>");
                sb_TreeNode.Append("</tr>");
                //创建子节点
                sb_TreeNode.Append(GetTableTreeNode(drv["Roles_ID"].ToString(), dt, trID));
                i++;
            }
            return sb_TreeNode.ToString();
        }
        /// <summary>
        /// 角色状态
        /// </summary>
        /// <param name="Menu_Type">类型</param>
        /// <returns></returns>
        public string Get_Type(string Menu_Type)
        {
            if (Menu_Type == "1")
            {
                return "正常";
            }
            else if (Menu_Type == "2")
            {
                return "<span style='color:red'>停用</span>";
            }
            else
            {
                return "其他";
            }
        }
        #endregion
    }
}