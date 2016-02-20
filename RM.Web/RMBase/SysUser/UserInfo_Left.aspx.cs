using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using RM.Common.DotNetData;
using RM.Busines;
using RM.Busines.DAL;
using RM.Busines.IDAO;
using RM.Web.App_Code;

namespace RM.Web.RMBase.SysUser
{
    public partial class UserInfo_Left : PageBase
    {
        public StringBuilder strHtml = new StringBuilder();
        RM_UserInfo_IDAO user_idao = new RM_UserInfo_Dal();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitInfo();
            }
        }
        /// <summary>
        /// 所属部门
        /// </summary>
        public void InitInfo()
        {
            DataTable dtOrg = user_idao.GetOrganizeList();
            if (DataTableHelper.IsExistRows(dtOrg))
            {
                DataView dv = new DataView(dtOrg);
                dv.RowFilter = "ParentId = '0'";
                foreach (DataRowView drv in dv)
                {
                    strHtml.Append("<li>");
                    strHtml.Append("<div>" + drv["Organization_Name"].ToString() + "");
                    strHtml.Append("<span style='display:none'>" + drv["Organization_ID"].ToString() + "</span></div>");
                    //创建子节点
                    strHtml.Append(GetTreeNode(drv["Organization_ID"].ToString(),dtOrg));
                    strHtml.Append("</li>");
                }
            }
            else
            {
                strHtml.Append("<li>");
                strHtml.Append("<div><span style='color:red;'>暂无数据</span></div>");
                strHtml.Append("</li>");
            }
        }
        /// <summary>
        /// 创建子节点
        /// </summary>
        /// <param name="parentID">父节点主键</param>
        /// <param name="dtMenu"></param>
        /// <returns></returns>
        public string GetTreeNode(string parentID, DataTable dtNode)
        {
            StringBuilder sb_TreeNode = new StringBuilder();
            DataView dv = new DataView(dtNode);
            dv.RowFilter = "ParentId = '" + parentID + "'";
            if (dv.Count > 0)
            {
                sb_TreeNode.Append("<ul>");
                foreach (DataRowView drv in dv)
                {
                    sb_TreeNode.Append("<li>");
                    sb_TreeNode.Append("<div>" + drv["Organization_Name"] + "");
                    sb_TreeNode.Append("<span style='display:none'>" + drv["Organization_ID"].ToString() + "</span></div>");
                    //创建子节点
                    sb_TreeNode.Append(GetTreeNode(drv["Organization_ID"].ToString(), dtNode));
                    sb_TreeNode.Append("</li>");
                }
                sb_TreeNode.Append("</ul>");
            }
            return sb_TreeNode.ToString();
        }
    }
}