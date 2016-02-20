using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using RM.Busines;
using RM.Busines.IDAO;
using RM.Busines.DAL;
using RM.Web.App_Code;

namespace RM.Web.RMBase.SysOrg
{
    public partial class Organization_List : PageBase
    {
        public StringBuilder str_tableTree = new StringBuilder();
        RM_UserInfo_IDAO user_idao = new RM_UserInfo_Dal();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetTreeTable();
            }
        }
        #region 部门树列表
        /// <summary>
        /// 部门树列表
        /// </summary>
        public void GetTreeTable()
        {
            DataTable dtOrg = user_idao.GetOrganizeList();
            DataView dv = new DataView(dtOrg);
            dv.RowFilter = " ParentId = '0'";
            int eRowIndex = 0;
            foreach (DataRowView drv in dv)
            {
                string trID = "node-" + eRowIndex.ToString();
                str_tableTree.Append("<tr id='" + trID + "'>");
                str_tableTree.Append("<td style='width: 120px;padding-left:20px;'><span class=\"folder\">" + drv["Organization_Name"].ToString() + "</span></td>");
                str_tableTree.Append("<td style='width: 50px;text-align: center;'>" + drv["Organization_Code"].ToString() + "</td>");
                str_tableTree.Append("<td style='width: 100px;text-align: center;'>" + drv["Organization_Manager"].ToString() + "</td>");
                str_tableTree.Append("<td style='width: 100px;text-align: center;'>" + drv["Organization_InnerPhone"].ToString() + "</td>");
                str_tableTree.Append("<td style='width: 100px;text-align: center;'>" + drv["Organization_OuterPhone"].ToString() + "</td>");
                str_tableTree.Append("<td style='width: 100px;text-align: center;'>" + drv["Organization_Fax"].ToString() + "</td>");
                str_tableTree.Append("<td style='width: 50px;text-align: center;'>" + drv["Organization_Zipcode"].ToString() + "</td>");
                str_tableTree.Append("<td style='width: 50px;text-align: center;'>" + drv["SortCode"].ToString() + "</td>");
                str_tableTree.Append("<td>" + drv["Organization_Address"].ToString() + "</td>");
                str_tableTree.Append("<td style='display:none'>" + drv["Organization_ID"].ToString() + "</td>");
                str_tableTree.Append("</tr>");
                //创建子节点
                str_tableTree.Append(GetTableTreeNode(drv["Organization_ID"].ToString(), dtOrg, trID));
                eRowIndex++;
            }
        }
        /// <summary>
        /// 创建子节点
        /// </summary>
        /// <param name="parentID">父节点主键</param>
        /// <param name="dtMenu"></param>
        /// <returns></returns>
        public string GetTableTreeNode(string parentID, DataTable dtMenu, string parentTRID)
        {
            StringBuilder sb_TreeNode = new StringBuilder();
            DataView dv = new DataView(dtMenu);
            dv.RowFilter = "ParentId = '" + parentID + "'";
            int i = 1;
            foreach (DataRowView drv in dv)
            {
                string trID = parentTRID + "-" + i.ToString();
                sb_TreeNode.Append("<tr id='" + trID + "' class='child-of-" + parentTRID + "'>");
                sb_TreeNode.Append("<td style='padding-left:20px;'><span class=\"folder\">" + drv["Organization_Name"].ToString() + "</span></td>");
                sb_TreeNode.Append("<td style='width: 50px;text-align: center;'>" + drv["Organization_Code"].ToString() + "</td>");
                sb_TreeNode.Append("<td style='width: 100px;text-align: center;'>" + drv["Organization_Manager"].ToString() + "</td>");
                sb_TreeNode.Append("<td style='width: 100px;text-align: center;'>" + drv["Organization_InnerPhone"].ToString() + "</td>");
                sb_TreeNode.Append("<td style='width: 100px;text-align: center;'>" + drv["Organization_OuterPhone"].ToString() + "</td>");
                sb_TreeNode.Append("<td style='width: 100px;text-align: center;'>" + drv["Organization_Fax"].ToString() + "</td>");
                sb_TreeNode.Append("<td style='width: 50px;text-align: center;'>" + drv["Organization_Zipcode"].ToString() + "</td>");
                sb_TreeNode.Append("<td style='width: 50px;text-align: center;'>" + drv["SortCode"].ToString() + "</td>");
                sb_TreeNode.Append("<td>" + drv["Organization_Address"].ToString() + "</td>");
                sb_TreeNode.Append("<td style='display:none'>" + drv["Organization_ID"].ToString() + "</td>");
                sb_TreeNode.Append("</tr>");
                //创建子节点
                sb_TreeNode.Append(GetTableTreeNode(drv["Organization_ID"].ToString(), dtMenu, trID));
                i++;
            }
            return sb_TreeNode.ToString();
        }
        #endregion
    }
}