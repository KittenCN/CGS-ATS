using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using RM.Common.DotNetData;
using System.Text;
using RM.Busines.DAL;
using RM.Busines.IDAO;
using RM.Common.DotNetBean;

namespace RM.Web.Frame
{
    public partial class MainTree : System.Web.UI.Page
    {
        public StringBuilder strHtml = new StringBuilder();
        RM_System_IDAO systemidao = new RM_System_Dal();
        protected void Page_Load(object sender, EventArgs e)
        {
            InitInfo();
        }
        /// <summary>
        /// 显示树导航菜单
        /// </summary>
        public void InitInfo()
        {
            string UserId = RequestSession.GetSessionUser().UserId.ToString();//用户ID
            DataTable dt = systemidao.GetMenuHtml(UserId);
            if (DataTableHelper.IsExistRows(dt))
            {
                DataView dv = new DataView(dt);
                dv.RowFilter = "ParentId = '0'";
                foreach (DataRowView drv in dv)
                {
                    strHtml.Append("<li>");
                    strHtml.Append("<div>" + drv["Menu_Name"] + "</div>");
                    //创建子节点
                    strHtml.Append(GetTreeNode(drv["Menu_Id"].ToString(), dt));
                    strHtml.Append("</li>");
                }
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
                    DataTable IsJudge = DataTableHelper.GetNewDataTable(dtNode, "ParentId = '" + drv["Menu_Id"].ToString() + "'");//判断是否有下级菜单
                    if (DataTableHelper.IsExistRows(IsJudge))
                    {
                        sb_TreeNode.Append("<div>" + drv["Menu_Name"] + "</div>");
                    }
                    else
                    {
                        sb_TreeNode.Append("<div title=\"" + drv["Menu_Title"] + "\" onclick=\"NavMenu('" + drv["NavigateUrl"] + "','" + drv["Menu_Name"] + "')\"><img src=\"/Themes/Images/32/" + drv["Menu_Img"] + "\" width=\"16\" height=\"16\" />" + drv["Menu_Name"] + "</div>");
                    }
                    //创建子节点
                    sb_TreeNode.Append(GetTreeNode(drv["Menu_Id"].ToString(), dtNode));
                    sb_TreeNode.Append("</li>");
                }
                sb_TreeNode.Append("</ul>");
            }
            return sb_TreeNode.ToString();
        }
    }
}