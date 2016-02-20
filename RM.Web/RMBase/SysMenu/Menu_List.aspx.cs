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
using RM.Busines.IDAO;
using RM.Busines.DAL;
using RM.Web.App_Code;

namespace RM.Web.RMBase.SysMenu
{
    public partial class Menu_List1 : PageBase
    {
        public StringBuilder TableTree_Menu = new StringBuilder();
        RM_System_IDAO systemidao = new RM_System_Dal();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetMenuTreeTable();
            }
        }
        #region 菜单树列表
        /// <summary>
        /// 菜单树列表
        /// </summary>
        public void GetMenuTreeTable()
        {
            DataTable dtMenu = systemidao.GetMenuList();
            DataView dv = new DataView(dtMenu);
            dv.RowFilter = " ParentId = '0'";
            int eRowIndex = 0;
            foreach (DataRowView drv in dv)
            {
                string trID = "node-" + eRowIndex.ToString();
                TableTree_Menu.Append("<tr id='" + trID + "'>");
                TableTree_Menu.Append("<td style='width: 230px;padding-left:20px;'><span class=\"folder\">" + drv["Menu_Name"].ToString() + "</span></td>");
                if (!string.IsNullOrEmpty(drv["Menu_Img"].ToString()))
                    TableTree_Menu.Append("<td style='width: 30px;text-align:center;'><img src='/Themes/images/32/" + drv["Menu_Img"].ToString() + "' style='width:16px; height:16px;vertical-align: middle;' alt=''/></td>");
                else
                    TableTree_Menu.Append("<td style='width: 30px;text-align:center;'><img src='/Themes/images/32/5005_flag.png' style='width:16px; height:16px;vertical-align: middle;' alt=''/></td>");
                TableTree_Menu.Append("<td style='width: 60px;text-align:center;'>" + this.GetMenu_Type(drv["Menu_Type"].ToString()) + "</td>");
                TableTree_Menu.Append("<td style='width: 60px;text-align:center;'>" + drv["Target"].ToString() + "</td>");
                TableTree_Menu.Append("<td style='width: 60px;text-align:center;'>" + drv["Sort"].ToString() + "</td>");
                TableTree_Menu.Append("<td>" + drv["NavigateUrl"].ToString() + "</td>");
                TableTree_Menu.Append("<td style='display:none'>" + drv["Menu_Id"].ToString() + "</td>");
                TableTree_Menu.Append("</tr>");
                //创建子节点
                TableTree_Menu.Append(GetTableTreeNode(drv["Menu_Id"].ToString(), dtMenu, trID));
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
                sb_TreeNode.Append("<td style='padding-left:20px;'><span class=\"folder\">" + drv["Menu_Name"].ToString() + "</span></td>");
                if (!string.IsNullOrEmpty(drv["Menu_Img"].ToString()))
                    sb_TreeNode.Append("<td style='width: 30px;text-align:center;'><img src='/Themes/images/32/" + drv["Menu_Img"].ToString() + "' style='width:16px; height:16px;vertical-align: middle;' alt=''/></td>");
                else
                    sb_TreeNode.Append("<td style='width: 30px;text-align:center;'><img src='/Themes/images/32/5005_flag.png' style='width:16px; height:16px;vertical-align: middle;' alt=''/></td>");
                sb_TreeNode.Append("<td style='width: 60px;text-align:center;'>" + this.GetMenu_Type(drv["Menu_Type"].ToString()) + "</td>");
                sb_TreeNode.Append("<td style='width: 60px;text-align:center;'>" + drv["Target"].ToString() + "</td>");
                sb_TreeNode.Append("<td style='width: 60px;text-align:center;'>" + drv["Sort"].ToString() + "</td>");
                sb_TreeNode.Append("<td>" + drv["NavigateUrl"].ToString() + "</td>");
                sb_TreeNode.Append("<td style='display:none'>" + drv["Menu_Id"].ToString() + "</td>");
                sb_TreeNode.Append("</tr>");
                //创建子节点
                sb_TreeNode.Append(GetTableTreeNode(drv["Menu_Id"].ToString(), dtMenu, trID));
                i++;
            }
            return sb_TreeNode.ToString();
        }
        /// <summary>
        /// 菜单类型
        /// </summary>
        /// <param name="Menu_Type">类型</param>
        /// <returns></returns>
        public string GetMenu_Type(string Menu_Type)
        {
            if (Menu_Type == "1")
            {
                return "父节";
            }
            else if (Menu_Type == "2")
            {
                return "子节";
            }
            else if (Menu_Type == "3")
            {
                return "按钮";
            }
            else
            {
                return "其他";
            }
        }
        #endregion
    }
}