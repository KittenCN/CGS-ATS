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
using RM.Busines.DAL;
using RM.Busines.IDAO;
using RM.Common.DotNetCode;
using RM.Common.DotNetBean;
using RM.Common.DotNetData;
using RM.Web.App_Code;
using RM.Common.DotNetEncrypt;

namespace RM.Web.RMBase.SysUser
{
    public partial class UserInfo_Form : PageBase
    {
        public StringBuilder str_OutputHtml = new StringBuilder();//附加信息
        public StringBuilder strOrgHtml = new StringBuilder();//所属部门
        public StringBuilder strRoleHtml = new StringBuilder();//所属角色
        public StringBuilder strUserGroupHtml = new StringBuilder();//用户工作组
        public StringBuilder strUserRightHtml = new StringBuilder();//用户权限
        RM_System_IDAO systemidao = new RM_System_Dal();
        RM_UserInfo_IDAO user_idao = new RM_UserInfo_Dal();
        string _key;
        string Property_Function = "用户附加信息";
        protected void Page_Load(object sender, EventArgs e)
        {
            _key = Request["key"];                  //主键
            str_OutputHtml.Append(systemidao.AppendProperty_Html(Property_Function));
            CreateUserName.Value = RequestSession.GetSessionUser().UserName.ToString();
            CreateDate.Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            InitInfoOrg();
            InitInfoRole();
            InitUserGroup();
            InitUserRight();
            if (!IsPostBack)
            {
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
            //初始化下拉框
            string sql = "select user_name from Base_UserInfo";
            StringBuilder sb_sql = new StringBuilder(sql);
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb_sql);
            var builder = new System.Text.StringBuilder();
            for (int i = 0; i < dt.Rows.Count; i++)
                builder.Append(String.Format("<option value='{0}'>", dt.Rows[i][0]));
            Boss_list.InnerHtml = builder.ToString();

            Hashtable ht = DataFactory.SqlDataBase().GetHashtableById("Base_UserInfo", "User_ID", _key);
            if (ht.Count > 0 && ht != null)
            {
                ControlBindHelper.SetWebControls(this.Page, ht);
                birthday.Text = Convert.ToDateTime(birthday.Text).ToString("yyyy-MM-dd");
                join_date.Text = Convert.ToDateTime(join_date.Text).ToString("yyyy-MM-dd");
                if(out_date.Text==null || out_date.Text=="")
                {
                    out_date.Text = "";
                }
                else
                {
                    out_date.Text = Convert.ToDateTime(out_date.Text).ToString("yyyy-MM-dd");
                }
                User_Pwd.Value = "*************";
                Boss_id.Value = GetNameFromID(Boss_id.Value);
                AppendProperty_value.Value = systemidao.GetPropertyInstancepk(Property_Function, _key);
            }
        }

        #region 所属部门
        /// <summary>
        /// 所属部门
        /// </summary>
        public void InitInfoOrg()
        {
            DataTable dtOrg = user_idao.GetOrganizeList();
            DataTable dtStaffOrganize = user_idao.InitStaffOrganize(_key);
            if (DataTableHelper.IsExistRows(dtOrg))
            {
                DataView dv = new DataView(dtOrg);
                dv.RowFilter = "ParentId = '0'";
                foreach (DataRowView drv in dv)
                {
                    strOrgHtml.Append("<li>");
                    strOrgHtml.Append("<div><input style='vertical-align: middle;margin-bottom:2px;' type=\"checkbox\" " + GetChecked("Organization_ID", drv["Organization_ID"].ToString(), dtStaffOrganize) + " value=\"" + drv["Organization_ID"] + "|所属部门" + "\" name=\"checkbox\" />");
                    strOrgHtml.Append(drv["Organization_Name"].ToString() + "</div>");
                    //创建子节点
                    strOrgHtml.Append(GetTreeNodeOrg(drv["Organization_ID"].ToString(), dtOrg, dtStaffOrganize));
                    strOrgHtml.Append("</li>");
                }
            }
            else
            {
                strOrgHtml.Append("<li>");
                strOrgHtml.Append("<div><span style='color:red;'>暂无数据</span></div>");
                strOrgHtml.Append("</li>");
            }
        }
        /// <summary>
        /// 创建子节点
        /// </summary>
        /// <param name="parentID">父节点主键</param>
        /// <param name="dtMenu"></param>
        /// <returns></returns>
        public string GetTreeNodeOrg(string parentID, DataTable dtNode, DataTable dtStaffOrganize)
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
                    sb_TreeNode.Append("<div class='treeview-file'>");
                    sb_TreeNode.Append("<input style='vertical-align: middle;margin-bottom:2px;' type=\"checkbox\" " + GetChecked("Organization_ID", drv["Organization_ID"].ToString(), dtStaffOrganize) + " value=\"" + drv["Organization_ID"] + "|所属部门" + "\" name=\"checkbox\" />");
                    sb_TreeNode.Append(drv["Organization_Name"].ToString() + "</div>");
                    //创建子节点
                    sb_TreeNode.Append(GetTreeNodeOrg(drv["Organization_ID"].ToString(), dtNode, dtStaffOrganize));
                    sb_TreeNode.Append("</li>");
                }
                sb_TreeNode.Append("</ul>");
            }
            return sb_TreeNode.ToString();
        }
        #endregion

        #region 所属角色
        /// <summary>
        /// 所属角色
        /// </summary>
        public void InitInfoRole()
        {
            DataTable dtRole = systemidao.InitRoleList();
            DataTable dtUserRole = user_idao.InitUserRole(_key);
            if (DataTableHelper.IsExistRows(dtRole))
            {
                DataView dv = new DataView(dtRole);
                dv.RowFilter = "ParentId = '0'";
                foreach (DataRowView drv in dv)
                {
                    strRoleHtml.Append("<li>");
                    strRoleHtml.Append("<div><input style='vertical-align: middle;margin-bottom:2px;' type=\"checkbox\" " + GetChecked("Roles_ID", drv["Roles_ID"].ToString(), dtUserRole) + " value=\"" + drv["Roles_ID"] + "|所属角色" + "\" name=\"checkbox\" />");
                    strRoleHtml.Append(drv["Roles_Name"].ToString() + "</div>");
                    //创建子节点
                    strRoleHtml.Append(GetTreeNodeRole(drv["Roles_ID"].ToString(), dtRole, dtUserRole));
                    strRoleHtml.Append("</li>");
                }
            }
            else
            {
                strRoleHtml.Append("<li>");
                strRoleHtml.Append("<div><span style='color:red;'>暂无数据</span></div>");
                strRoleHtml.Append("</li>");
            }
        }
        /// <summary>
        /// 创建子节点
        /// </summary>
        /// <param name="parentID">父节点主键</param>
        /// <param name="dtMenu"></param>
        /// <returns></returns>
        public string GetTreeNodeRole(string parentID, DataTable dtNode, DataTable dtUserRole)
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
                    sb_TreeNode.Append("<div class='treeview-file'>");
                    sb_TreeNode.Append("<input style='vertical-align: middle;margin-bottom:2px;' type=\"checkbox\" " + GetChecked("Roles_ID", drv["Roles_ID"].ToString(), dtUserRole) + " value=\"" + drv["Roles_ID"] + "|所属角色" + "\" name=\"checkbox\" />");
                    sb_TreeNode.Append(drv["Roles_Name"].ToString() + "</div>");
                    //创建子节点
                    sb_TreeNode.Append(GetTreeNodeRole(drv["Roles_ID"].ToString(), dtNode, dtUserRole));
                    sb_TreeNode.Append("</li>");
                }
                sb_TreeNode.Append("</ul>");
            }
            return sb_TreeNode.ToString();
        }
        #endregion

        #region 用户工作组
        /// <summary>
        /// 用户工作组
        /// </summary>
        public void InitUserGroup()
        {
            DataTable dtUserGroupList = user_idao.InitUserGroupList();
            DataTable dtUserInfoUserGroup = user_idao.InitUserInfoUserGroup(_key);
            if (DataTableHelper.IsExistRows(dtUserGroupList))
            {
                DataView dv = new DataView(dtUserGroupList);
                dv.RowFilter = "ParentId = '0'";
                foreach (DataRowView drv in dv)
                {
                    strUserGroupHtml.Append("<li>");
                    strUserGroupHtml.Append("<div><input style='vertical-align: middle;margin-bottom:2px;' type=\"checkbox\" " + GetChecked("UserGroup_ID", drv["UserGroup_ID"].ToString(), dtUserInfoUserGroup) + " value=\"" + drv["UserGroup_ID"] + "|用户工作组" + "\" name=\"checkbox\" />");
                    strUserGroupHtml.Append(drv["UserGroup_Name"].ToString() + "</div>");
                    //创建子节点
                    strUserGroupHtml.Append(GetTreeNodeUserGroup(drv["UserGroup_ID"].ToString(), dtUserGroupList,dtUserInfoUserGroup));
                    strUserGroupHtml.Append("</li>");
                }
            }
            else
            {
                strUserGroupHtml.Append("<li>");
                strUserGroupHtml.Append("<div><span style='color:red;'>暂无数据</span></div>");
                strUserGroupHtml.Append("</li>");
            }
        }
        /// <summary>
        /// 创建子节点
        /// </summary>
        /// <param name="parentID">父节点主键</param>
        /// <param name="dtMenu"></param>
        /// <returns></returns>
        public string GetTreeNodeUserGroup(string parentID, DataTable dtNode, DataTable dtUserInfoUserGroup)
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
                    sb_TreeNode.Append("<div class='treeview-file'>");
                    sb_TreeNode.Append("<input style='vertical-align: middle;margin-bottom:2px;' type=\"checkbox\" " + GetChecked("UserGroup_ID", drv["UserGroup_ID"].ToString(), dtUserInfoUserGroup) + " value=\"" + drv["UserGroup_ID"] + "|用户工作组" + "\" name=\"checkbox\" />");
                    sb_TreeNode.Append(drv["UserGroup_Name"].ToString() + "</div>");
                    //创建子节点
                    sb_TreeNode.Append(GetTreeNodeUserGroup(drv["UserGroup_ID"].ToString(), dtNode, dtUserInfoUserGroup));
                    sb_TreeNode.Append("</li>");
                }
                sb_TreeNode.Append("</ul>");
            }
            return sb_TreeNode.ToString();
        }
        #endregion

        #region 用户权限
        /// <summary>
        /// 菜单树列表
        /// </summary>
        public void InitUserRight()
        {
            DataTable dtList = systemidao.GetMenuBind();
            DataTable dtUserRight = user_idao.InitUserRight(_key);
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
                    strUserRightHtml.Append("<tr id='" + trID + "'>");
                    strUserRightHtml.Append("<td style='width: 200px;padding-left:20px;'><span class=\"folder\">" + drv["Menu_Name"] + "</span></td>");
                    if (!string.IsNullOrEmpty(drv["Menu_Img"].ToString()))
                        strUserRightHtml.Append("<td style='width: 30px;text-align:center;'><img src='/Themes/images/32/" + drv["Menu_Img"] + "' style='width:16px; height:16px;vertical-align: middle;' alt=''/></td>");
                    else
                        strUserRightHtml.Append("<td style='width: 30px;text-align:center;'><img src='/Themes/images/32/5005_flag.png' style='width:16px; height:16px;vertical-align: middle;' alt=''/></td>");
                    strUserRightHtml.Append("<td style=\"width: 23px; text-align: left;\"><input id='ckb" + trID + "' onclick=\"ckbValueObj(this.id)\" style='vertical-align: middle;margin-bottom:2px;' type=\"checkbox\" " + GetChecked("Menu_Id", drv["Menu_Id"].ToString(), dtUserRight) + "  value=\"" + drv["Menu_Id"] + "|用户权限" + "\" name=\"checkbox\" /></td>");
                    strUserRightHtml.Append("<td>" + GetButton(drv["Menu_Id"].ToString(), dtButoon, trID, dtUserRight) + "</td>");
                    strUserRightHtml.Append("</tr>");
                    //创建子节点
                    strUserRightHtml.Append(GetTreeNodeUserRight(drv["Menu_Id"].ToString(), dtMenu, trID, dtButoon, dtUserRight));
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
        public string GetTreeNodeUserRight(string parentID, DataTable dtMenu, string parentTRID, DataTable dtButoon, DataTable dtUserRight)
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
                sb_TreeNode.Append("<td style=\"width: 23px; text-align: left;\"><input id='ckb" + trID + "' onclick=\"ckbValueObj(this.id)\" " + GetChecked("Menu_Id", drv["Menu_Id"].ToString(), dtUserRight) + " style='vertical-align: middle;margin-bottom:2px;' type=\"checkbox\" value=\"" + drv["Menu_Id"] + "|用户权限" + "\" name=\"checkbox\" /></td>");
                sb_TreeNode.Append("<td>" + GetButton(drv["Menu_Id"].ToString(), dtButoon, trID, dtUserRight) + "</td>");
                sb_TreeNode.Append("</tr>");
                //创建子节点
                sb_TreeNode.Append(GetTreeNodeUserRight(drv["Menu_Id"].ToString(), dtMenu, trID, dtButoon, dtUserRight));
                i++;
            }
            return sb_TreeNode.ToString();
        }
        /// <summary>
        /// 获取导航菜单所属按钮
        /// </summary>
        /// <param name="Menu_Type">类型</param>
        /// <returns></returns>
        public string GetButton(string Menu_Id, DataTable dt, string parentTRID, DataTable dtUserRight)
        {
            StringBuilder ButtonHtml = new StringBuilder(); ;
            DataTable dt_Button = DataTableHelper.GetNewDataTable(dt, "ParentId = '" + Menu_Id + "'");
            if (DataTableHelper.IsExistRows(dt_Button))
            {
                int i = 1;
                foreach (DataRow drv in dt_Button.Rows)
                {
                    string trID = parentTRID + "--" + i.ToString();
                    ButtonHtml.Append("<lable><input id='ckb" + trID + "' onclick=\"ckbValueObj(this.id)\" " + GetChecked("Menu_Id", drv["Menu_Id"].ToString(), dtUserRight) + " style='vertical-align: middle;margin-bottom:2px;' type=\"checkbox\" value=\"" + drv["Menu_Id"] + "|用户权限" + "\" name=\"checkbox\" />");
                    ButtonHtml.Append("" + drv["Menu_Name"].ToString() + "</lable>&nbsp;&nbsp;&nbsp;&nbsp;");
                    i++;
                }
                return ButtonHtml.ToString();
            }
            return ButtonHtml.ToString();
        }
        #endregion

        /// <summary>
        /// 验证是否存在
        /// </summary>
        /// <param name="pkName">对象主键</param>
        /// <param name="Obj_id">对象主键值</param>
        /// <param name="dt">数据源</param>
        /// <returns></returns>
        public string GetChecked(string pkName, string Obj_Val, DataTable dt)
        {
            StringBuilder strSql = new StringBuilder();
            dt = DataTableHelper.GetNewDataTable(dt, "" + pkName + " = '" + Obj_Val + "'");
            if (DataTableHelper.IsExistRows(dt))
                return "checked=\"checked\"";
            else
                return "";
        }

        private string GetNameFromID(string EmpID)
        {
            string txt_Result = "";

            string sql = "select User_name from Base_UserInfo where User_ID='" + EmpID + "' ";
            StringBuilder sb_sql = new StringBuilder(sql);
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb_sql);
            if (dt.Rows.Count != 0 && dt.Rows[0].ItemArray[0].ToString() != "")
            {
                txt_Result = dt.Rows[0].ItemArray[0].ToString();
            }

            return txt_Result;
        }

        private string GetIDFromName(string EmpName)
        {
            string txt_Result = "";

            string sql = "select User_ID from Base_UserInfo where User_name='" + EmpName + "' ";
            StringBuilder sb_sql = new StringBuilder(sql);
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb_sql);
            if (dt.Rows.Count != 0 && dt.Rows[0].ItemArray[0].ToString() != "")
            {
                txt_Result = dt.Rows[0].ItemArray[0].ToString();
            }

            return txt_Result;
        }

        #region 保存事件
        /// <summary>
        /// 保存事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Save_Click(object sender, EventArgs e)
        {
            string guid = CommonHelper.GetGuid;
            Hashtable ht = new Hashtable();
            ht["User_Code"] = User_Code.Value;
            ht["User_Name"] = User_Name.Value;
            ht["User_Account"] = User_Account.Value;
            ht["User_Pwd"] = Md5Helper.MD5(User_Pwd.Value,32);
            ht["User_Sex"] = User_Sex.Value;
            ht["Email"] = Email.Value;
            //ht["Title"] = Title.Value;
            ht["User_Remark"] = User_Remark.Value;
            ht["birthday"] = birthday.Text;
            ht["Sage_b"] = Sage_b.Value;
            ht["join_date"] = join_date.Text;
            ht["out_date"] = out_date.Text;
            ht["work_flag"] = work_flag.Value;
            ht["Boss_id"] = GetIDFromName(Boss_id.Value);
            ht["Boss_Flag"] = Boss_Flag.Value;
            if (!string.IsNullOrEmpty(_key))
            {
                guid = _key;
                ht["ModifyDate"] = DateTime.Now;
                ht.Remove("User_Pwd");
                ht["ModifyUserId"] = RequestSession.GetSessionUser().UserId;
                ht["ModifyUserName"] = RequestSession.GetSessionUser().UserName;
            }
            else
            {
                ht["User_ID"] = guid;
                ht["CreateUserId"] = RequestSession.GetSessionUser().UserId;
                ht["CreateUserName"] = RequestSession.GetSessionUser().UserName;
            }
            bool IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit("Base_UserInfo", "User_ID", _key, ht);
            if (IsOk)
            {
                IsOk = systemidao.Add_AppendPropertyInstance(guid, AppendProperty_value.Value.Split(';'));
                if (IsOk)
                {
                    IsOk = this.add_ItemForm(checkbox_value.Value.Split(','), guid);
                }
            }
            if (IsOk)
            {
                ShowMsgHelper.ParmAlertMsg("操作成功！");
            }
            else
            {
                ShowMsgHelper.Alert_Error("操作失败！");
            }
        }
        /// <summary>
        /// 批量新增所属，部门，角色，工作组，用户权限
        /// </summary>
        /// <param name="item_value">对象主键</param>
        /// <param name="user_id">用户主键</param>
        /// <returns></returns>
        public bool add_ItemForm(string[] item_value, string user_id)
        {
            try
            {
                int index = 0;
                string key;
                string type;
                StringBuilder[] sqls = new StringBuilder[item_value.Length + 4];
                object[] objs = new object[item_value.Length + 4];

                StringBuilder sbDelete_org = new StringBuilder();
                sbDelete_org.Append("Delete From Base_StaffOrganize Where User_ID =@User_ID");
                SqlParam[] parm_org = new SqlParam[] { new SqlParam("@User_ID", user_id) };
                sqls[0] = sbDelete_org;
                objs[0] = parm_org;

                StringBuilder sbDelete_Role = new StringBuilder();
                sbDelete_Role.Append("Delete From Base_UserRole Where User_ID =@User_ID");
                SqlParam[] parm_Role = new SqlParam[] { new SqlParam("@User_ID", user_id) };
                sqls[1] = sbDelete_Role;
                objs[1] = parm_Role;

                StringBuilder sbDelete_UserGroup = new StringBuilder();
                sbDelete_UserGroup.Append("Delete From Base_UserInfoUserGroup Where User_ID =@User_ID");
                SqlParam[] parm_UserGroup = new SqlParam[] { new SqlParam("@User_ID", user_id) };
                sqls[2] = sbDelete_UserGroup;
                objs[2] = parm_UserGroup;

                StringBuilder sbDelete_Right = new StringBuilder();
                sbDelete_Right.Append("Delete From Base_UserRight Where User_ID =@User_ID");
                SqlParam[] parm_Right = new SqlParam[] { new SqlParam("@User_ID", user_id) };
                sqls[3] = sbDelete_Right;
                objs[3] = parm_Right;
                index = 4;
                foreach (var item in item_value)
                {
                    if (item.Length > 0)
                    {
                        string[] str_item = item.Split('|');
                        key = str_item[0];
                        type = str_item[1];
                        if (type == "所属部门")
                        {
                            StringBuilder sbadd = new StringBuilder();
                            sbadd.Append("Insert into Base_StaffOrganize(");
                            sbadd.Append("StaffOrganize_Id,Organization_ID,User_ID,CreateUserId,CreateUserName");
                            sbadd.Append(")Values(");
                            sbadd.Append("@StaffOrganize_Id,@Organization_ID,@User_ID,@CreateUserId,@CreateUserName)");
                            SqlParam[] parmAdd = new SqlParam[] { 
                                     new SqlParam("@StaffOrganize_Id", CommonHelper.GetGuid),
                                     new SqlParam("@Organization_ID", key),
                                     new SqlParam("@User_ID", user_id),
                                     new SqlParam("@CreateUserId", RequestSession.GetSessionUser().UserId),
                                     new SqlParam("@CreateUserName", RequestSession.GetSessionUser().UserName)};
                            sqls[index] = sbadd;
                            objs[index] = parmAdd;
                        }
                        else if (type == "所属角色")
                        {
                            StringBuilder sbadd = new StringBuilder();
                            sbadd.Append("Insert into Base_UserRole(");
                            sbadd.Append("UserRole_ID,User_ID,Roles_ID,CreateUserId,CreateUserName");
                            sbadd.Append(")Values(");
                            sbadd.Append("@UserRole_ID,@User_ID,@Roles_ID,@CreateUserId,@CreateUserName)");
                            SqlParam[] parmAdd = new SqlParam[] { 
                                     new SqlParam("@UserRole_ID", CommonHelper.GetGuid),
                                     new SqlParam("@User_ID",user_id),
                                     new SqlParam("@Roles_ID",  key),
                                     new SqlParam("@CreateUserId", RequestSession.GetSessionUser().UserId),
                                     new SqlParam("@CreateUserName", RequestSession.GetSessionUser().UserName)};
                            sqls[index] = sbadd;
                            objs[index] = parmAdd;
                        }
                        else if (type == "用户工作组")
                        {
                            StringBuilder sbadd = new StringBuilder();
                            sbadd.Append("Insert into Base_UserInfoUserGroup(");
                            sbadd.Append("UserInfoUserGroup_ID,User_ID,UserGroup_ID,CreateUserId,CreateUserName");
                            sbadd.Append(")Values(");
                            sbadd.Append("@UserInfoUserGroup_ID,@User_ID,@UserGroup_ID,@CreateUserId,@CreateUserName)");
                            SqlParam[] parmAdd = new SqlParam[] { 
                                     new SqlParam("@UserInfoUserGroup_ID", CommonHelper.GetGuid),
                                     new SqlParam("@User_ID",user_id),
                                     new SqlParam("@UserGroup_ID",  key),
                                     new SqlParam("@CreateUserId", RequestSession.GetSessionUser().UserId),
                                     new SqlParam("@CreateUserName", RequestSession.GetSessionUser().UserName)};
                            sqls[index] = sbadd;
                            objs[index] = parmAdd;
                        }
                        else if (type == "用户权限")
                        {
                            StringBuilder sbadd = new StringBuilder();
                            sbadd.Append("Insert into Base_UserRight(");
                            sbadd.Append("UserRight_ID,User_ID,Menu_Id,CreateUserId,CreateUserName");
                            sbadd.Append(")Values(");
                            sbadd.Append("@UserRight_ID,@User_ID,@Menu_Id,@CreateUserId,@CreateUserName)");
                            SqlParam[] parmAdd = new SqlParam[] { 
                                     new SqlParam("@UserRight_ID", CommonHelper.GetGuid),
                                     new SqlParam("@User_ID",user_id),
                                     new SqlParam("@Menu_Id",  key),
                                     new SqlParam("@CreateUserId", RequestSession.GetSessionUser().UserId),
                                     new SqlParam("@CreateUserName", RequestSession.GetSessionUser().UserName)};
                            sqls[index] = sbadd;
                            objs[index] = parmAdd;
                        }
                        index++;
                    }
                }
                return DataFactory.SqlDataBase().BatchExecuteBySql(sqls, objs) >= 0 ? true : false;
            }
            catch
            {
                return false;
            }
        }
        #endregion

    }
}