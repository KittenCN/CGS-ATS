using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RM.Common.DotNetBean;
using System.Text;
using RM.Busines.DAL;
using RM.Busines.IDAO;
using System.Collections;
using RM.Common.DotNetUI;
using RM.Busines;
using System.Data;
using RM.Common.DotNetData;
using RM.Web.App_Code;

namespace RM.Web.RMBase.SysPersonal
{
    public partial class CurrentUserManager : PageBase
    {
        public StringBuilder BasicUserInfoHtml = new StringBuilder();//基本信息
        public StringBuilder AppendHtml = new StringBuilder();//附加信息
        public StringBuilder StrTree_Menu = new StringBuilder();//拥有权限
        RM_UserInfo_IDAO user_idao = new RM_UserInfo_Dal();
        RM_System_IDAO systemidao = new RM_System_Dal();
        string Property_Function = "用户附加信息";
        public string _UserName, _key;
        protected void Page_Load(object sender, EventArgs e)
        {
            _key = RequestSession.GetSessionUser().UserId.ToString();
            _UserName = RequestSession.GetSessionUser().UserName.ToString();
            InitBasicUserInfo();
            AppendHtml.Append(systemidao.AppendProperty_HtmlLabel(Property_Function));
            ControlBindHelper.BindRepeaterList(systemidao.GetRoleByMember(_key), rp_Item);//拥有角色
            GetMenuTreeTable();
        }
        /// <summary>
        /// 基本信息
        /// </summary>
        public void InitBasicUserInfo()
        {
            Hashtable ht = DataFactory.SqlDataBase().GetHashtableById("Base_UserInfo", "User_ID", _key);
            if (ht.Count > 0 && ht != null)
            {
                ControlBindHelper.SetWebControls(this.Page, ht);
                if (ht["USER_SEX"].ToString() == "1")
                {
                    User_Sex.Text = "男士";
                }
                else
                {
                    User_Sex.Text = "女士";
                }
                if (ht["DELETEMARK"].ToString() == "2")
                {
                    DeleteMark.Text = "正常";
                }
                else if (ht["DELETEMARK"].ToString() == "3")
                {
                    DeleteMark.Text = "停用";
                }
                else if (ht["DELETEMARK"].ToString() == "1")
                {
                    DeleteMark.Text = "未授权";
                }
                AppendProperty_value.Value = systemidao.GetPropertyInstancepk(Property_Function, _key);
            }
        }
        #region 初始化权限树列表
        /// <summary>
        /// 菜单树列表
        /// </summary>
        public void GetMenuTreeTable()
        {
            DataTable dtList = systemidao.GetMenuBind();
            DataTable dtRight = systemidao.GetHaveRightUserInfo(_key);
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
                    StrTree_Menu.Append("<td style=\"width: 23px; text-align: left;\"><input id='ckb" + trID + "' onclick=\"ckbValueObj(this.id)\" style='vertical-align: middle;margin-bottom:2px;' type=\"checkbox\" " + GetChecked(drv["Menu_Id"].ToString(), dtRight) + "  value=\"" + drv["Menu_Id"] + "\" name=\"checkbox\" /></td>");
                    StrTree_Menu.Append("<td>" + GetButton(drv["Menu_Id"].ToString(), dtButoon, trID, dtRight) + "</td>");
                    StrTree_Menu.Append("</tr>");
                    //创建子节点
                    StrTree_Menu.Append(GetTableTreeNode(drv["Menu_Id"].ToString(), dtMenu, trID, dtButoon, dtRight));
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
        public string GetTableTreeNode(string parentID, DataTable dtMenu, string parentTRID, DataTable dtButoon, DataTable dtRight)
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
                sb_TreeNode.Append("<td style=\"width: 23px; text-align: left;\"><input id='ckb" + trID + "' onclick=\"ckbValueObj(this.id)\" " + GetChecked(drv["Menu_Id"].ToString(), dtRight) + " style='vertical-align: middle;margin-bottom:2px;' type=\"checkbox\" value=\"" + drv["Menu_Id"] + "\" name=\"checkbox\" /></td>");
                sb_TreeNode.Append("<td>" + GetButton(drv["Menu_Id"].ToString(), dtButoon, trID, dtRight) + "</td>");
                sb_TreeNode.Append("</tr>");
                //创建子节点
                sb_TreeNode.Append(GetTableTreeNode(drv["Menu_Id"].ToString(), dtMenu, trID, dtButoon, dtRight));
                i++;
            }
            return sb_TreeNode.ToString();
        }
        /// <summary>
        /// 获取导航菜单所属按钮
        /// </summary>
        /// <param name="Menu_Type">类型</param>
        /// <returns></returns>
        public string GetButton(string Menu_Id, DataTable dt, string parentTRID, DataTable dtRight)
        {
            StringBuilder ButtonHtml = new StringBuilder(); ;
            DataTable dt_Button = DataTableHelper.GetNewDataTable(dt, "ParentId = '" + Menu_Id + "'");
            if (DataTableHelper.IsExistRows(dt_Button))
            {
                int i = 1;
                foreach (DataRow drv in dt_Button.Rows)
                {
                    string trID = parentTRID + "--" + i.ToString();
                    ButtonHtml.Append("<lable><input id='ckb" + trID + "' onclick=\"ckbValueObj(this.id)\" " + GetChecked(drv["Menu_Id"].ToString(), dtRight) + " style='vertical-align: middle;margin-bottom:2px;' type=\"checkbox\" value=\"" + drv["Menu_Id"] + "\" name=\"checkbox\" />");
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
        /// <param name="dt">加载所属角色权限</param>
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
    }
}