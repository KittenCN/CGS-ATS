using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RM.Common.DotNetCode;
using RM.Busines.IDAO;
using System.Data;
using RM.Common.DotNetEncrypt;
using System.Collections;
using RM.Common.DotNetBean;
using RM.Common.DotNetData;
using RM.Common.DotNetUI;
using RM.Common.DotNetConfig;
using RM.DataBase.DataBase.Common;
using System.IO;
using RM.Common.DotNetFile;

namespace RM.Busines.DAL
{
    /// <summary>
    /// 系统业务设置
    /// </summary>
    public class RM_System_Dal : RM_System_IDAO
    {
        #region 公用接口
        /// <summary>
        /// 删除信息
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="pkName">字段主键</param>
        /// <param name="pkVal">字段值</param>
        /// <returns></returns>
        public int DeleteData_Base(string tableName, string pkName, string[] pkVal)
        {
            return DataFactory.SqlDataBase().BatchDeleteData(tableName, pkName, pkVal);
        }
        #endregion

        #region 菜单导航
        /// <summary>
        /// 菜单所有列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetMenuBind()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT * FROM Base_SysMenu WHERE DeleteMark = 1 ORDER BY SortCode ASC");
            return DataFactory.SqlDataBase().GetDataTableBySQL(strSql);
        }
        /// <summary>
        /// 菜单树列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetMenuList()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT Menu_Id,Menu_Name,Menu_Img,Menu_Type,TARGET,ParentId,
                            CAST(Menu_Type AS VARCHAR(10)) +'-'+CAST(SortCode AS VARCHAR(10)) AS Sort,
                            NavigateUrl,CreateUserName,CreateDate,ModifyUserName,ModifyDate
                            FROM Base_SysMenu WHERE DeleteMark = 1 and Menu_Type !=3 ORDER BY SortCode ASC");
            return DataFactory.SqlDataBase().GetDataTableBySQL(strSql);
        }
        /// <summary>
        /// 所有按钮信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetButtonList()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM Base_Button WHERE DELETEMARK = 1 ORDER BY SORTCODE ASC");
            return DataFactory.SqlDataBase().GetDataTableBySQL(strSql);
        }
        /// <summary>
        /// 根据菜单主键获取已有按钮
        /// </summary>
        /// <returns></returns>
        public DataTable GetSysMenuByButton(string Menu_Id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM Base_SysMenu ");
            strSql.Append("WHERE ParentId =@Menu_Id ");
            strSql.Append("AND DELETEMARK = 1 AND Menu_Type = 3 ORDER BY SORTCODE ASC");
            SqlParam[] parm = new SqlParam[] { new SqlParam("@Menu_Id", Menu_Id) };
            return DataFactory.SqlDataBase().GetDataTableBySQL(strSql, parm);
        }
        #region 分配按钮
        /// <summary>
        /// 分配按钮
        /// </summary>
        /// <param name="pkVal">全中按钮ID</param>
        /// <param name="ParentId"></param>
        /// <returns></returns>
        public int AllotButton(string pkVal, string ParentId)
        {
            try
            {
                DataTable dt_Button = this.GetButtonList();
                DataTable Newdt_Button = DataTableHelper.GetNewDataTable(dt_Button, "Button_ID = '" + pkVal + "'");
                string Button_Name = Newdt_Button.Rows[0]["Button_Name"].ToString();            //按钮名称
                string Button_Title = Newdt_Button.Rows[0]["Button_Title"].ToString();          //按钮标记
                string Button_Img = Newdt_Button.Rows[0]["Button_Img"].ToString();              //按钮图标
                string Button_Code = Newdt_Button.Rows[0]["Button_Code"].ToString();            //按钮代码
                Hashtable ht = new Hashtable();
                ht["Menu_Id"] = CommonHelper.GetGuid;
                ht["ParentId"] = ParentId;
                ht["Menu_Name"] = Button_Name;
                ht["Menu_Title"] = Button_Title;
                ht["Menu_Img"] = Button_Img;
                ht["Menu_Type"] = 3;
                ht["NavigateUrl"] = Button_Code;
                ht["SortCode"] = CommonHelper.GetInt(DataFactory.SqlDataBase().GetObjectValue(new StringBuilder("SELECT MAX(SortCode) FROM Base_SysMenu WHERE ParentId = '" + ParentId + "' AND DELETEMARK = 1 AND Menu_Type = 3"))) + 1;
                ht["Target"] = "Onclick";
                ht["CreateUserId"] = RequestSession.GetSessionUser().UserId;
                ht["CreateUserName"] = RequestSession.GetSessionUser().UserName;
                return DataFactory.SqlDataBase().InsertByHashtable("Base_SysMenu", ht);
            }
            catch (Exception)
            {
                return -1;
            }
        }
        #endregion

        #region 权限显示按钮
        /// <summary>
        /// 动态显示按钮
        /// </summary>
        /// <param name="UserId">用户主键</param>
        /// <returns></returns>
        public DataTable GetButtonHtml(string UserId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  M.Menu_Id,M.Menu_Name ,M.Menu_Title ,M.Menu_Img ,M.TARGET ,M.ParentId ,M.NavigateUrl ,M.SortCode,M.Menu_Type
                            FROM    ( SELECT    M.Menu_Name ,M.Menu_Title ,M.Menu_Img ,M.TARGET ,M.ParentId ,M.Menu_Id ,M.NavigateUrl ,M.SortCode,M.Menu_Type ,'角色权限' AS TheirTYPE
                                      FROM      Base_SysMenu M
                                                LEFT JOIN Base_RoleRight R_R ON R_R.Menu_Id = M.Menu_Id
                                                LEFT JOIN Base_UserRole U_R ON U_R.Roles_ID = R_R.Roles_ID
                                      WHERE     U_R.User_ID = @User_ID
                                                AND M.DeleteMark = 1 
                                      UNION ALL
                                      SELECT    M.Menu_Name ,M.Menu_Title ,M.Menu_Img ,M.TARGET ,M.ParentId ,M.Menu_Id ,M.NavigateUrl ,M.SortCode,M.Menu_Type ,'用户组权限' AS TheirTYPE
                                      FROM      Base_SysMenu M
                                                LEFT JOIN Base_UserGroupRight U_R ON U_R.Menu_Id = M.Menu_Id
                                                LEFT JOIN Base_UserInfoUserGroup U_G ON U_G.UserGroup_ID = U_R.UserGroup_ID
                                      WHERE     U_G.User_ID = @User_ID
                                                AND M.DeleteMark = 1 
                                      UNION ALL
                                      SELECT    M.Menu_Name ,M.Menu_Title ,M.Menu_Img ,M.TARGET ,M.ParentId ,M.Menu_Id ,M.NavigateUrl ,M.SortCode,M.Menu_Type,'用户权限' AS TheirTYPE
                                      FROM      Base_SysMenu M
                                                LEFT JOIN Base_UserRight U_R ON U_R.Menu_Id = M.Menu_Id
                                      WHERE     U_R.User_ID = @User_ID
                                                AND M.DeleteMark = 1
                                    ) M
                            GROUP BY M.Menu_Id ,M.Menu_Name ,M.Menu_Title ,M.Menu_Img,M.TARGET ,M.ParentId ,M.NavigateUrl ,M.SortCode,M.Menu_Type ORDER BY M.SortCode");
            SqlParam[] para = {
                                         new SqlParam("@User_ID",UserId)};
            StringBuilder sb_html = new StringBuilder();
            string URL = RequestHelper.GetScriptName;
            DataTable dt_Menu = DataFactory.SqlDataBase().GetDataTableBySQL(strSql, para);//获取所有菜单
            return DataTableHelper.GetNewDataTable(dt_Menu, "ParentId='" + GetMenuByNavigateUrl(URL, dt_Menu) + "' AND Menu_Type = 3");
        }
        /// <summary>
        /// 根据菜单URL获取主键
        /// </summary>
        /// <param name="NavigateUrl">菜单路径</param>
        /// <returns>返回主键</returns>
        public string GetMenuByNavigateUrl(string NavigateUrl, DataTable dt_Menu)
        {
            try
            {
                DataTable dt = DataTableHelper.GetNewDataTable(dt_Menu, "NavigateUrl='" + NavigateUrl + "'");
                return dt.Rows[0]["Menu_Id"].ToString();
            }
            catch
            {
                return "";
            }
        }
        #endregion
        #endregion

        #region 系统权限
        /// <summary>
        /// 根据用户主键，显示拥有所有权限
        /// </summary>
        /// <param name="User_ID">用户主键</param>
        /// <returns></returns>
        public DataTable GetHaveRightUserInfo(string User_ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT DISTINCT(M.Menu_Id),M.TheirTYPE FROM(
                            SELECT M.Menu_Id,'角色权限' AS TheirTYPE FROM Base_SysMenu M
                            LEFT JOIN  Base_RoleRight R_R ON R_R.Menu_Id = M.Menu_Id
                            LEFT JOIN Base_UserRole U_R ON U_R.Roles_ID = R_R.Roles_ID
                            WHERE U_R.User_ID = @User_ID UNION ALL
                            SELECT M.Menu_Id,'用户组权限' AS TheirTYPE FROM Base_SysMenu M
                            LEFT JOIN  Base_UserGroupRight U_R ON U_R.Menu_Id = M.Menu_Id
                            LEFT JOIN Base_UserInfoUserGroup U_G ON U_G.UserGroup_ID = U_R.UserGroup_ID
                            WHERE U_G.User_ID = @User_ID UNION ALL
                            SELECT M.Menu_Id,'用户权限' AS TheirTYPE FROM Base_SysMenu M
                            LEFT JOIN  Base_UserRight U_R ON U_R.Menu_Id = M.Menu_Id
                            WHERE U_R.User_ID = @User_ID
                            ) M");
            SqlParam[] para = {
                                         new SqlParam("@User_ID",User_ID)};
            return DataFactory.SqlDataBase().GetDataTableBySQL(strSql, para);
        }
        /// <summary>
        /// 权限菜单导航
        /// </summary>
        /// <param name="UserId">用户主键</param>
        /// <returns></returns>
        public DataTable GetMenuHtml(string UserId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  M.Menu_Id,M.Menu_Name ,M.Menu_Title ,M.Menu_Img ,M.TARGET ,M.ParentId ,M.NavigateUrl ,M.SortCode
                            FROM    ( SELECT    M.Menu_Name ,M.Menu_Title ,M.Menu_Img ,M.TARGET ,M.ParentId ,M.Menu_Id ,M.NavigateUrl ,M.SortCode ,'角色权限' AS TheirTYPE
                                      FROM      Base_SysMenu M
                                                LEFT JOIN Base_RoleRight R_R ON R_R.Menu_Id = M.Menu_Id
                                                LEFT JOIN Base_UserRole U_R ON U_R.Roles_ID = R_R.Roles_ID
                                      WHERE     M.TARGET = 'Iframe' AND U_R.User_ID = @User_ID
                                                AND M.DeleteMark = 1 
                                      UNION ALL
                                      SELECT    M.Menu_Name ,M.Menu_Title ,M.Menu_Img ,M.TARGET ,M.ParentId ,M.Menu_Id ,M.NavigateUrl ,M.SortCode ,'用户组权限' AS TheirTYPE
                                      FROM      Base_SysMenu M
                                                LEFT JOIN Base_UserGroupRight U_R ON U_R.Menu_Id = M.Menu_Id
                                                LEFT JOIN Base_UserInfoUserGroup U_G ON U_G.UserGroup_ID = U_R.UserGroup_ID
                                      WHERE     M.TARGET = 'Iframe' AND U_G.User_ID = @User_ID
                                                AND M.DeleteMark = 1 
                                      UNION ALL
                                      SELECT    M.Menu_Name ,M.Menu_Title ,M.Menu_Img ,M.TARGET ,M.ParentId ,M.Menu_Id ,M.NavigateUrl ,M.SortCode ,'用户权限' AS TheirTYPE
                                      FROM      Base_SysMenu M
                                                LEFT JOIN Base_UserRight U_R ON U_R.Menu_Id = M.Menu_Id
                                      WHERE     M.TARGET = 'Iframe' AND U_R.User_ID = @User_ID
                                                AND M.DeleteMark = 1
                                    ) M
                            GROUP BY M.Menu_Id ,M.Menu_Name ,M.Menu_Title ,M.Menu_Img ,M.TARGET ,M.ParentId ,M.NavigateUrl ,M.SortCode ORDER BY M.SortCode");
            SqlParam[] para = {
                                         new SqlParam("@User_ID",UserId)};
            return DataFactory.SqlDataBase().GetDataTableBySQL(strSql, para);
        }
        /// <summary>
        /// URL权限验证
        /// </summary>
        /// <param name="UserId">用户主键</param>
        /// <returns></returns>
        public DataTable GetPermission_URL(string UserId)
        {
            DataTable dt = new DataTable();
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  M.NavigateUrl ,
                                    M.TheirTYPE ,
                                    M.User_ID
                            FROM    ( SELECT    M.NavigateUrl ,
                                                '角色权限' AS TheirTYPE ,
                                                U_R.User_ID AS User_ID
                                      FROM      Base_SysMenu M
                                                LEFT JOIN Base_RoleRight R_R ON R_R.Menu_Id = M.Menu_Id
                                                LEFT JOIN Base_UserRole U_R ON U_R.Roles_ID = R_R.Roles_ID
                                      WHERE     M.Menu_Type != 3
                                      UNION ALL
                                      SELECT    M.NavigateUrl ,
                                                '用户组权限' AS TheirTYPE ,
                                                U_G.User_ID AS User_ID
                                      FROM      Base_SysMenu M
                                                LEFT JOIN Base_UserGroupRight U_R ON U_R.Menu_Id = M.Menu_Id
                                                LEFT JOIN Base_UserInfoUserGroup U_G ON U_G.UserGroup_ID = U_R.UserGroup_ID
                                      WHERE     M.Menu_Type != 3
                                      UNION ALL
                                      SELECT    M.NavigateUrl ,
                                                '用户权限' AS TheirTYPE ,
                                                U_R.User_ID AS User_ID
                                      FROM      Base_SysMenu M
                                                LEFT JOIN Base_UserRight U_R ON U_R.Menu_Id = M.Menu_Id
                                      WHERE     M.Menu_Type != 3
                                    ) M
                            WHERE   M.NavigateUrl != ''
                            AND M.User_ID IS NOT NULL");
            #region 数据缓存
            if (CacheHelper.GetCache("KeyPermission_URL") == null)//判断缓存是否过期
            {
                dt = DataFactory.SqlDataBase().GetDataTableBySQL(strSql);
                CacheHelper.Insert("KeyPermission_URL", dt);//数据源存入缓存
            }
            else
            {
                dt = (DataTable)CacheHelper.GetCache("KeyPermission_URL");//获取数据源
            }
            #endregion
            dt = DataTableHelper.GetNewDataTable(dt, "User_ID= '" + UserId + "'");//根据用户ID查询所属权限页面
            return dt;
        }
        #endregion

        #region 角色管理
        /// <summary>
        /// 角色列表
        /// </summary>
        /// <returns></returns>
        public DataTable InitRoleList()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT Roles_ID, ParentId, Roles_Name, Roles_Remark, SortCode, DeleteMark, CreateDate,CreateUserName, ModifyDate, ModifyUserName
                            FROM Base_Roles WHERE DeleteMark != 0 ORDER BY SortCode ASC");
            return DataFactory.SqlDataBase().GetDataTableBySQL(strSql);
        }
        /// <summary>
        /// 节点位置下拉框绑定
        /// </summary>
        /// <returns></returns>
        public DataTable InitRoleParentId()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT Roles_ID,
                            Roles_Name+' - '+CASE ParentId WHEN '0' THEN '父节' ELSE  '子节' END AS Roles_Name
                            FROM Base_Roles WHERE DeleteMark = 1 ORDER BY SortCode ASC");
            return DataFactory.SqlDataBase().GetDataTableBySQL(strSql);
        }
        /// <summary>
        /// 加载所属角色成员
        /// </summary>
        /// <param name="Roles_ID">角色主键</param>
        public DataTable InitUserRole(string Roles_ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT R.User_ID,U.User_Code+'|'+U.User_Name AS User_Name,O.Organization_Name FROM Base_UserRole R
                            LEFT JOIN Base_UserInfo U ON U.User_ID=R.User_ID
                            LEFT JOIN Base_StaffOrganize S ON S.User_ID = U.User_ID
                            LEFT JOIN Base_Organization O ON O.Organization_ID = S.Organization_ID WHERE Roles_ID = @Roles_ID");
            SqlParam[] para = {
                                         new SqlParam("@Roles_ID",Roles_ID)};
            return DataFactory.SqlDataBase().GetDataTableBySQL(strSql, para);
        }
        /// <summary>
        /// 加载所属角色权限
        /// </summary>
        /// <param name="Roles_ID">角色主键</param>
        public DataTable InitRoleRight(string Roles_ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT Menu_Id FROM Base_RoleRight WHERE Roles_ID = @Roles_ID");
            SqlParam[] para = {
                                         new SqlParam("@Roles_ID",Roles_ID)};
            return DataFactory.SqlDataBase().GetDataTableBySQL(strSql, para);
        }
        /// <summary>
        /// 添加角色成员
        /// </summary>
        /// <param name="pkVal">选择成员主键</param>
        /// <param name="Roles_ID">角色主键</param>
        /// <returns></returns>
        public bool Add_RoleAllotMember(string[] pkVal, string Roles_ID)
        {
            try
            {
                StringBuilder[] sqls = new StringBuilder[pkVal.Length + 1];
                object[] objs = new object[pkVal.Length + 1];
                int index = 0;
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("Delete From Base_UserRole Where Roles_ID =@Roles_ID");
                SqlParam[] parm = new SqlParam[] { new SqlParam("@Roles_ID", Roles_ID) };
                sqls[0] = sbDelete;
                objs[0] = parm;
                index = 1;
                foreach (string item in pkVal)
                {
                    if (item.Length > 0)
                    {
                        StringBuilder sbadd = new StringBuilder();
                        sbadd.Append("Insert into Base_UserRole(");
                        sbadd.Append("UserRole_ID,User_ID,Roles_ID,CreateUserId,CreateUserName");
                        sbadd.Append(")Values(");
                        sbadd.Append("@UserRole_ID,@User_ID,@Roles_ID,@CreateUserId,@CreateUserName)");
                        SqlParam[] parmAdd = new SqlParam[] { 
                                     new SqlParam("@UserRole_ID", CommonHelper.GetGuid),
                                     new SqlParam("@User_ID", item),
                                     new SqlParam("@Roles_ID", Roles_ID),
                                     new SqlParam("@CreateUserId", RequestSession.GetSessionUser().UserId),
                                     new SqlParam("@CreateUserName", RequestSession.GetSessionUser().UserName)};
                        sqls[index] = sbadd;
                        objs[index] = parmAdd;
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
        /// <summary>
        /// 角色分配权限
        /// </summary>
        /// <param name="pkVal">选择权限主键</param>
        /// <param name="Roles_ID">角色主键</param>
        /// <returns></returns>
        public bool Add_RoleAllotAuthority(string[] pkVal, string Roles_ID)
        {
            try
            {
                StringBuilder[] sqls = new StringBuilder[pkVal.Length + 1];
                object[] objs = new object[pkVal.Length + 1];
                int index = 0;
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("Delete From Base_RoleRight Where Roles_ID =@Roles_ID");
                SqlParam[] parm = new SqlParam[] { new SqlParam("@Roles_ID", Roles_ID) };
                sqls[0] = sbDelete;
                objs[0] = parm;
                index = 1;
                foreach (string item in pkVal)
                {
                    if (item.Length > 0)
                    {
                        StringBuilder sbadd = new StringBuilder();
                        sbadd.Append("Insert into Base_RoleRight(");
                        sbadd.Append("RoleRight_ID,Roles_ID,Menu_Id,CreateUserId,CreateUserName");
                        sbadd.Append(")Values(");
                        sbadd.Append("@RoleRight_ID,@Roles_ID,@Menu_Id,@CreateUserId,@CreateUserName)");
                        SqlParam[] parmAdd = new SqlParam[] { 
                                     new SqlParam("@RoleRight_ID", CommonHelper.GetGuid),
                                     new SqlParam("@Roles_ID", Roles_ID),
                                     new SqlParam("@Menu_Id", item),
                                     new SqlParam("@CreateUserId", RequestSession.GetSessionUser().UserId),
                                     new SqlParam("@CreateUserName", RequestSession.GetSessionUser().UserName)};
                        sqls[index] = sbadd;
                        objs[index] = parmAdd;
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
        /// <summary>
        /// 根据成员ID获取所属角色
        /// </summary>
        /// <param name="user_id">用户主键</param>
        /// <returns></returns>
        public DataTable GetRoleByMember(string user_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT Roles_Name,Roles_Remark FROM Base_Roles
                            WHERE Roles_ID IN(SELECT Roles_ID FROM Base_UserRole WHERE User_ID = @User_ID)");
            SqlParam[] para = {
                                         new SqlParam("@User_ID",user_id)};
            return DataFactory.SqlDataBase().GetDataTableBySQL(strSql, para);
        }
        #endregion

        #region 附加属性信息
        /// <summary>
        /// 业务附加属性列表
        /// </summary>
        /// <param name="Function">业务功能</param>
        /// <returns></returns>
        public DataTable AppendProperty_List(string Function)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM Base_AppendProperty WHERE DeleteMark = 1 AND Property_Control_ID !='0' AND Property_Function=@Property_Function ORDER BY SortCode ASC");
            SqlParam[] para = {
                                         new SqlParam("@Property_Function",Function)};
            return DataFactory.SqlDataBase().GetDataTableBySQL(strSql, para);
        }
        /// <summary>
        /// 业务功能列表
        /// </summary>
        /// <returns></returns>
        public DataTable AppendProperty_Function()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM Base_AppendProperty WHERE DeleteMark = 1 AND Property_Control_ID = '0'");
            return DataFactory.SqlDataBase().GetDataTableBySQL(strSql);
        }
        /// <summary>
        /// 根据业务名称获取附加属性值
        /// </summary>
        /// <param name="Property_Function">所属功能</param>
        /// <param name="Obj_ID">业务主键</param>
        /// <returns></returns>
        public string GetPropertyInstancepk(string Property_Function, string Obj_ID)
        {
            StringBuilder item_value = new StringBuilder();
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT I.Property_Control_ID,I.PropertyInstance_Value FROM Base_AppendPropertyInstance I 
                            LEFT JOIN Base_AppendProperty A ON I.Property_Control_ID = A.Property_Control_ID");
            strSql.Append(" where I.PropertyInstance_Key = @PropertyInstance_Key AND A.Property_Function = @Property_Function");
            SqlParam[] param = {
                                        new SqlParam("@PropertyInstance_Key",Obj_ID),
                                         new SqlParam("@Property_Function",Property_Function)};
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(strSql, param);
            if (dt != null && dt.Rows.Count != 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    item_value.Append(dt.Rows[i]["Property_Control_ID"].ToString() + "|" + dt.Rows[i]["PropertyInstance_Value"].ToString() + ";");
                }
            }
            return item_value.ToString();
        }
        /// <summary>
        /// 加载动态显示附加属性(返回Input)
        /// </summary>
        /// <param name="Function"></param>
        /// <returns>返回html格式</returns>
        public string AppendProperty_Html(string Function)
        {
            StringBuilder str_Output = new StringBuilder();
            DataTable dt = this.AppendProperty_List(Function);
            if (DataTableHelper.IsExistRows(dt))
            {
                int j = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    string Property_Name = dr["Property_Name"].ToString();                     //属性名称
                    string Control_ID = dr["Property_Control_ID"].ToString();                  //控件ID
                    string Control_Type = dr["Property_Control_Type"].ToString();              //控件类型
                    string Control_Length = dr["Property_Control_Length"].ToString();          //控件长度
                    string Control_Style = dr["Property_Control_Style"].ToString();            //控件样式
                    string Control_Validator = dr["Property_Control_Validator"].ToString();    //验证控件
                    string Maxlength = dr["Property_Control_Maxlength"].ToString();//最大长度
                    string Colspan = dr["Property_Colspan"].ToString();                        //合并列
                    string DataSource = dr["Property_Control_DataSource"].ToString();          //数据源 
                    string Event = dr["Property_Event"].ToString();                            //事件
                    if (Control_Validator != "")
                    {
                        Control_Validator = "datacol=\"yes\" err=\"" + Property_Name + "\" checkexpession=\"" + Control_Validator + "\"";
                    }
                    if (Colspan == "0")
                        Colspan = "";
                    else
                        Colspan = "colspan=" + Colspan + "";
                    if (Colspan == "")
                    {
                        if (j == 0)
                        {
                            str_Output.Append(ControlBindHelper.GetControlProperty(Control_Type, Property_Name, Control_ID, Control_Style, Control_Length, Control_Validator, j, Colspan, DataSource, Event, Maxlength));
                            j = 1;
                        }
                        else if (j == 1)
                        {
                            str_Output.Append(ControlBindHelper.GetControlProperty(Control_Type, Property_Name, Control_ID, Control_Style, Control_Length, Control_Validator, j, Colspan, DataSource, Event, Maxlength));
                            j = 0;
                        }
                    }
                    else
                    {
                        str_Output.Append(ControlBindHelper.GetControlProperty(Control_Type, Property_Name, Control_ID, Control_Style, Control_Length, Control_Validator, j, Colspan, DataSource, Event, Maxlength));
                    }
                }
            }
            return str_Output.ToString();
        }
        /// <summary>
        /// 加载动态显示附加属性(返回Label)
        /// </summary>
        /// <param name="Function"></param>
        /// <returns>返回html格式</returns>
        public string AppendProperty_HtmlLabel(string Function)
        {
            StringBuilder str_Output = new StringBuilder();
            DataTable dt = this.AppendProperty_List(Function);
            if (DataTableHelper.IsExistRows(dt))
            {
                int j = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    string Property_Name = dr["Property_Name"].ToString();                     //属性名称
                    string Control_ID = dr["Property_Control_ID"].ToString();                  //控件ID
                    string Control_Type = "4";                                                 //控件类型
                    string Control_Length = dr["Property_Control_Length"].ToString();          //控件长度
                    string Control_Style = dr["Property_Control_Style"].ToString();            //控件样式
                    string Control_Validator = dr["Property_Control_Validator"].ToString();    //验证控件
                    string Maxlength = dr["Property_Control_Maxlength"].ToString();//最大长度
                    string Colspan = dr["Property_Colspan"].ToString();                        //合并列
                    string DataSource = dr["Property_Control_DataSource"].ToString();          //数据源 
                    string Event = dr["Property_Event"].ToString();                            //事件
                    str_Output.Append(ControlBindHelper.GetControlProperty(Control_Type, Property_Name, Control_ID, Control_Style, Control_Length, Control_Validator, j, Colspan, DataSource, Event, Maxlength));
                }
            }
            return str_Output.ToString();
        }
        /// <summary>
        /// 新增附加属性实例
        /// </summary>
        /// <param name="guid">对象业务属性主键</param>
        /// <param name="arrayitem">附加属性实例值</param>
        /// <returns></returns>
        public bool Add_AppendPropertyInstance(string guid, string[] arrayitem)
        {
            int index = 0;
            string[] pkVal = arrayitem;
            StringBuilder[] sqls = new StringBuilder[pkVal.Length + 1];
            object[] objs = new object[pkVal.Length + 1];
            StringBuilder sbDelete = new StringBuilder();
            sbDelete.Append("Delete From Base_AppendPropertyInstance Where PropertyInstance_Key =@PropertyInstance_Key");
            SqlParam[] parm = new SqlParam[] { new SqlParam("@PropertyInstance_Key", guid) };
            sqls[0] = sbDelete;
            objs[0] = parm;
            index = 1;
            foreach (var item in pkVal)
            {
                if (item.Length > 0)
                {
                    string[] str_item = item.Split('|');
                    StringBuilder sbadd = new StringBuilder();
                    sbadd.Append("Insert into Base_AppendPropertyInstance(");
                    sbadd.Append("PropertyInstance_ID,Property_Control_ID,PropertyInstance_Value,PropertyInstance_Key");
                    sbadd.Append(")Values(");
                    sbadd.Append("@PropertyInstance_ID,@Property_Control_ID,@PropertyInstance_Value,@PropertyInstance_Key)");
                    SqlParam[] parmAdd = new SqlParam[] { 
                                     new SqlParam("@PropertyInstance_ID", CommonHelper.GetGuid),
                                     new SqlParam("@Property_Control_ID", str_item[0]),
                                     new SqlParam("@PropertyInstance_Value", str_item[1]),
                                     new SqlParam("@PropertyInstance_Key", guid)};
                    sqls[index] = sbadd;
                    objs[index] = parmAdd;
                    index++;
                }
            }
            return DataFactory.SqlDataBase().BatchExecuteBySql(sqls, objs) >= 0 ? true : false;
        }
        #endregion

        #region 首页快捷功能
        /// <summary>
        /// 首页快捷功能
        /// </summary>
        /// <param name="User_ID">用户主键</param>
        /// <returns></returns>
        public DataTable GetHomeShortcut_List(string User_ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM Base_O_A_Setup WHERE User_ID = @User_ID");
            SqlParam[] para = {
                                         new SqlParam("@User_ID",User_ID)};
            return DataFactory.SqlDataBase().GetDataTableBySQL(strSql, para);
        }
        #endregion

        #region 回收站
        /// <summary>
        /// 获取某一个表的字段，说明
        /// </summary>
        /// <param name="object_TableName"></param>
        /// <returns></returns>
        public DataTable GetRecyclebin_ObjField(string object_TableName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT Field_Key=a.name,Field_Name=isnull(g.[value],'未填说明')
                            FROM syscolumns a
                            left join systypes b on a.xusertype=b.xusertype
                            inner join sysobjects d on a.id=d.id  and d.xtype='U' and  d.name<>'dtproperties'
                            left join syscomments e on a.cdefault=e.id
                            left join sys.extended_properties g on a.id=g.major_id and a.colid=g.minor_id 
                            left join sys.extended_properties f on d.id=f.major_id and f.minor_id=0");
            strSql.Append("where d.name='" + object_TableName + "' order by a.id,a.colorder");
            return DataFactory.SqlDataBase().GetDataTableBySQL(strSql);
        }
        /// <summary>
        /// 数据放入回收站 
        /// </summary>
        /// <param name="module">业务模块</param>
        /// <param name="tableName">表名</param>
        /// <param name="pkName">字段主键</param>
        /// <param name="pkVal">字段值</param>
        /// <returns></returns>
        public int Virtualdelete(string module, string tableName, string pkName, string[] pkVal)
        {
            int num = 0;
            try
            {
                StringBuilder[] sqls = new StringBuilder[pkVal.Length * 2];
                object[] objs = new object[pkVal.Length * 2];
                int index = 0;
                foreach (string item in pkVal)
                {
                    StringBuilder sbEdit = new StringBuilder();
                    sbEdit.Append(" Update ");
                    sbEdit.Append(tableName);
                    sbEdit.Append(" Set DeleteMark = 0");
                    sbEdit.Append(" Where ").Append(pkName).Append("=").Append("@ID");
                    SqlParam[] parmEdit = new SqlParam[] { new SqlParam("@ID", item) };
                    sqls[index] = sbEdit;
                    objs[index] = parmEdit;
                    index++;
                    StringBuilder sbadd = new StringBuilder();
                    sbadd.Append("Insert into Base_Recyclebin(");
                    sbadd.Append("Recyclebin_ID,");
                    sbadd.Append("Recyclebin_Name,");
                    sbadd.Append("Recyclebin_Database,");
                    sbadd.Append("Recyclebin_Table,CreateUserId,CreateUserName,");
                    sbadd.Append("Recyclebin_FieldKey,Recyclebin_EventField)Values(");
                    sbadd.Append("@Recyclebin_ID,");
                    sbadd.Append("@Recyclebin_Name,");
                    sbadd.Append("@Recyclebin_Database,");
                    sbadd.Append("@Recyclebin_Table,@CreateUserId,@CreateUserName,");
                    sbadd.Append("@Recyclebin_FieldKey,@Recyclebin_EventField)");
                    SqlParam[] parmAdd = new SqlParam[] { 
                                     new SqlParam("@Recyclebin_ID", CommonHelper.GetGuid),
                                     new SqlParam("@Recyclebin_Name", module),
                                     new SqlParam("@Recyclebin_Database", "RM_DB"),
                                     new SqlParam("@Recyclebin_Table", tableName),
                                     new SqlParam("@Recyclebin_FieldKey", pkName),
                                     new SqlParam("@CreateUserId", RequestSession.GetSessionUser().UserId),
                                     new SqlParam("@CreateUserName", RequestSession.GetSessionUser().UserName),
                                     new SqlParam("@Recyclebin_EventField", item)};
                    sqls[index] = sbadd;
                    objs[index] = parmAdd;
                    index++;
                }
                num = DataFactory.SqlDataBase().BatchExecuteBySql(sqls, objs);
            }
            catch
            {
                return num;
            }
            return num;
        }
        /// <summary>
        /// 还原回收站数据 
        /// </summary>
        /// <param name="pkVal">字段值</param>
        /// <returns></returns>
        public int Recyclebin_Restore(string[] pkVal)
        {
            int num = 0;
            try
            {
                string tableName;
                string pkName;
                StringBuilder[] sqls = new StringBuilder[pkVal.Length * 2];
                object[] objs = new object[pkVal.Length * 2];
                int index = 0;
                foreach (string item in pkVal)
                {
                    Hashtable ht = DataFactory.SqlDataBase().GetHashtableById("Base_Recyclebin", "Recyclebin_ID", item);
                    if (ht.Count > 0 && ht != null)
                    {
                        tableName = ht["RECYCLEBIN_TABLE"].ToString();
                        pkName = ht["RECYCLEBIN_FIELDKEY"].ToString();
                        StringBuilder sbEdit = new StringBuilder();
                        sbEdit.Append(" Update ");
                        sbEdit.Append(tableName);
                        sbEdit.Append(" Set DeleteMark = 1");
                        sbEdit.Append(" Where ").Append(pkName).Append("=").Append("@ID");
                        SqlParam[] parmEdit = new SqlParam[] { new SqlParam("@ID", ht["RECYCLEBIN_EVENTFIELD"].ToString()) };
                        sqls[index] = sbEdit;
                        objs[index] = parmEdit;
                        index++;
                        StringBuilder sbDelete = new StringBuilder();
                        sbDelete.Append("Delete From Base_Recyclebin Where Recyclebin_ID =@Recyclebin_ID");
                        SqlParam[] parmDelete = new SqlParam[] { new SqlParam("@Recyclebin_ID", item) };
                        sqls[index] = sbDelete;
                        objs[index] = parmDelete;
                    }
                    index++;
                }
                num = DataFactory.SqlDataBase().BatchExecuteBySql(sqls, objs);
            }
            catch
            {
                return num;
            }
            return num;
        }
        /// <summary>
        /// 清空回收站数据 
        /// </summary>
        /// <param name="pkVal">字段值</param>
        /// <returns></returns>
        public int Recyclebin_Empty(string[] pkVal)
        {
            int num = 0;
            try
            {
                string tableName;
                string pkName;
                StringBuilder[] sqls = new StringBuilder[pkVal.Length * 2];
                object[] objs = new object[pkVal.Length * 2];
                int index = 0;
                foreach (string item in pkVal)
                {
                    Hashtable ht = DataFactory.SqlDataBase().GetHashtableById("Base_Recyclebin", "Recyclebin_ID", item);
                    if (ht.Count > 0 && ht != null)
                    {
                        tableName = ht["RECYCLEBIN_TABLE"].ToString();
                        pkName = ht["RECYCLEBIN_FIELDKEY"].ToString();
                        StringBuilder sb = new StringBuilder();
                        sb.Append(" Delete From ");
                        sb.Append(tableName);
                        sb.Append(" Where ").Append(pkName).Append("=").Append("@ID");
                        SqlParam[] parm = new SqlParam[] { new SqlParam("@ID", ht["RECYCLEBIN_EVENTFIELD"].ToString()) };
                        sqls[index] = sb;
                        objs[index] = parm;
                        index++;
                        StringBuilder sbDelete = new StringBuilder();
                        sbDelete.Append("Delete From Base_Recyclebin Where Recyclebin_ID =@Recyclebin_ID");
                        SqlParam[] parmDelete = new SqlParam[] { new SqlParam("@Recyclebin_ID", item) };
                        sqls[index] = sbDelete;
                        objs[index] = parmDelete;
                    }
                    index++;
                }
                num = DataFactory.SqlDataBase().BatchExecuteBySql(sqls, objs);
            }
            catch
            {
                return num;
            }
            return num;
        }
        #endregion

        #region 数据库管理
        /// <summary>
        /// 还原恢复数据库
        /// </summary>
        /// <param name="FilePath">保存路径</param>
        /// <returns></returns>
        public bool DataRestore(string FilePath)
        {
            string[] Connection = ConfigHelper.GetAppSettings("SqlServer_RM_DB").Split(';');
            SqlServerBackup sqlserverbackup = new SqlServerBackup();
            sqlserverbackup.Server = Connection[0].Substring(7);
            sqlserverbackup.Database = Connection[1].Substring(9);
            sqlserverbackup.Uid = Connection[2].Substring(4);
            sqlserverbackup.Pwd = Connection[3].Substring(4);
            if (sqlserverbackup.DbRestore(FilePath))
                return true;
            else
                return false;
        }
        /// <summary>
        /// 备份数据库
        /// </summary>
        /// <param name="FilePath">保存路径</param>
        /// <returns></returns>
        public bool DataBackups(string FilePath)
        {
            try
            {
                string[] Connection = ConfigHelper.GetAppSettings("SqlServer_RM_DB").Split(';');
                SqlServerBackup sqlserverbackup = new SqlServerBackup();
                sqlserverbackup.Server = Connection[0].Substring(7);
                sqlserverbackup.Database = Connection[1].Substring(9);
                sqlserverbackup.Uid = Connection[2].Substring(4);
                sqlserverbackup.Pwd = Connection[3].Substring(4);
                if (sqlserverbackup.DbBackup(FilePath))
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 添加数据库备份恢复记录
        /// </summary>
        /// <param name="Type">类型：备份、恢复</param>
        /// <param name="File">文件名</param>
        /// <param name="Size">文件大小</param>
        /// <param name="CreateUserName">创建用户</param>
        /// <param name="DB">所属数据库</param>
        /// <param name="Memo">备注</param>
        /// <returns></returns>
        public void Add_Backup_Restore_Log(string Type, string File, string Size, string CreateUserName, string DB, string Memo)
        {
            LogHelper Logger = new LogHelper("Backup_Restore_Log");
            Hashtable ht = new Hashtable();
            StringBuilder sb = new StringBuilder();
            sb.Append(Type + "∫");
            sb.Append(File + "∫");
            sb.Append(Size + "∫");
            sb.Append(CreateUserName + "∫");
            sb.Append(DB + "∫");
            sb.Append(Memo + "∫");
            sb.Append(DateTime.Now + "∫");
            sb.Append("∮");
            Logger.WriteLog(sb.ToString());
        }
        /// <summary>
        /// 数据库备份还原记录
        /// </summary>
        /// <returns></returns>
        public DataTable GetBackup_Restore_Log_List()
        {
            LogHelper Logger = new LogHelper("Backup_Restore_Log");
            string filepath = ConfigHelper.GetAppSettings("LogFilePath") + "/Backup_Restore_Log.log";
            StreamReader sr = new StreamReader(filepath, Encoding.GetEncoding("UTF-8"));//取得这txt文件的编码
            string[] strvalue = sr.ReadToEnd().ToString().Split('∮');
            sr.Close();
            DataTable dt = new DataTable();
            dt.Columns.Add("Backup_Restore_Type", Type.GetType("System.String"));
            dt.Columns.Add("Backup_Restore_File", Type.GetType("System.String"));
            dt.Columns.Add("Backup_Restore_Size", Type.GetType("System.String"));
            dt.Columns.Add("CreateUserName", Type.GetType("System.String"));
            dt.Columns.Add("Backup_Restore_DB", Type.GetType("System.String"));
            dt.Columns.Add("Backup_Restore_Memo", Type.GetType("System.String"));
            dt.Columns.Add("CreateDate", Type.GetType("System.String"));
            foreach (string item in strvalue)
            {
                if (item.Length > 6)
                {
                    string[] str_item = item.Split('∫');
                    DataRow row = dt.NewRow();
                    string[] Typeitem = str_item[0].Split(']');
                    row["Backup_Restore_Type"] = Typeitem[1].Trim();
                    row["Backup_Restore_File"] = str_item[1];
                    row["Backup_Restore_Size"] = str_item[2];
                    row["CreateUserName"] = str_item[3];
                    row["Backup_Restore_DB"] = str_item[4];
                    row["Backup_Restore_Memo"] = str_item[5];
                    row["CreateDate"] = str_item[6];
                    dt.Rows.Add(row);
                }
            }
            dt.DefaultView.Sort = "CreateDate DESC";//DataTable排序
            DataTable dtTemp = dt.DefaultView.ToTable();
            return dtTemp;
        }
        /// <summary>
        /// 获取数据库所有表名
        /// </summary>
        /// <returns></returns>
        public DataTable GetSysobjects()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Name as TABLE_NAME from sysobjects where xtype='u' and status >=0 and Name !='sysdiagrams' ");
            return DataFactory.SqlDataBase().GetDataTableBySQL(strSql);
        }
        /// <summary>
        /// 获取某一个表的所有字段
        /// </summary>
        /// <param name="object_id">表名</param>
        /// <returns></returns>
        public DataTable GetSyscolumns(string object_id)
        {
            DataTable dt = new DataTable();
            StringBuilder strSql = new StringBuilder();
            if (!string.IsNullOrEmpty(object_id) && object_id != "未选择")
            {
                strSql.Append(@"SELECT
                                     [列名]=a.name,
                                     [数据类型]=b.name,
                                     [长度]=COLUMNPROPERTY(a.id,a.name,'PRECISION'),
                                     [是否为空]=case when a.isnullable=1 then '√'else '' end,
                                     [默认值]=isnull(e.text,''),
                                     [说明]=isnull(g.[value],'未填说明')
                                     FROM syscolumns a
                                     left join systypes b on a.xusertype=b.xusertype
                                     inner join sysobjects d on a.id=d.id  and d.xtype='U' and  d.name<>'dtproperties'
                                     left join syscomments e on a.cdefault=e.id
                                     left join sys.extended_properties g on a.id=g.major_id and a.colid=g.minor_id 
                                     left join sys.extended_properties f on d.id=f.major_id and f.minor_id=0");
                strSql.Append("where d.name='" + object_id + "' order by a.id,a.colorder");
                return DataFactory.SqlDataBase().GetDataTableBySQL(strSql);
            }
            return dt;
        }
        #endregion
    }
}
