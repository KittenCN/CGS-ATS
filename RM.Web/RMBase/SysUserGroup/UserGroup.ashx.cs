using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.SessionState;
using RM.Common.DotNetCode;
using RM.Busines.DAL;
using RM.Busines.IDAO;
using RM.Common.DotNetJson;
using System.Data;
using RM.Common.DotNetData;

namespace RM.Web.RMBase.SysUserGroup
{
    /// <summary>
    /// UserGroup 的摘要说明
    /// </summary>
    public class UserGroup : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Buffer = true;
            context.Response.ExpiresAbsolute = DateTime.Now.AddDays(-1);
            context.Response.AddHeader("pragma", "no-cache");
            context.Response.AddHeader("cache-control", "");
            context.Response.CacheControl = "no-cache";
            string Action = context.Request["action"].Trim();               //提交动作
            string txt_Search = context.Request["txt_Search"];
            string Searchwhere = context.Request["Searchwhere"];
            string UserGroup_ID = context.Request["UserGroup_ID"];//用户组主键
            string User_ID = context.Request["User_ID"];//用户主键
            RM_UserInfo_IDAO user_idao = new RM_UserInfo_Dal();
            switch (Action)
            {
                case "UserList"://用户信息数据源 返回JSON
                    StringBuilder SqlWhere = new StringBuilder();
                    IList<SqlParam> IList_param = new List<SqlParam>();
                    if (!string.IsNullOrEmpty(txt_Search))
                    {
                        SqlWhere.Append(" AND " + Searchwhere.Trim() + " like @obj ");
                        IList_param.Add(new SqlParam("@obj", '%' + txt_Search.Trim() + '%'));
                    }
                    SqlWhere.Append(" AND USER_ID NOT IN(SELECT USER_ID FROM Base_UserInfoUserGroup WHERE UserGroup_ID = @UserGroup_ID)");
                    IList_param.Add(new SqlParam("@UserGroup_ID", UserGroup_ID));
                    context.Response.Write(JsonHelper.DataTableToJson(user_idao.GetUserInfoInfo(SqlWhere, IList_param), "UserGroupList"));
                    context.Response.End();
                    break;
                case "UserGroupInfo"://用户组  返回HTML
                    context.Response.Write(InitUserGroupInfo(user_idao.Load_UserInfoUserGroupList(UserGroup_ID)));
                    context.Response.End();
                    break;
                case "UserGroupaddMember"://用户组新增成员
                    bool IsOk = user_idao.AddUserGroupMenber(User_ID.Split(','), UserGroup_ID);
                    if (IsOk)
                    {
                        context.Response.Write(1);
                        context.Response.End();
                    }
                    else
                    {
                        context.Response.Write(-1);
                        context.Response.End();
                    }
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 用户组
        /// </summary>
        public string InitUserGroupInfo(DataTable dt)
        {
            StringBuilder str_allUserGroup = new StringBuilder();
            DataTable dtUserGroup = dt;
            if (DataTableHelper.IsExistRows(dtUserGroup))
            {
                DataView dv = new DataView(dtUserGroup);
                foreach (DataRowView drv in dv)
                {
                    str_allUserGroup.Append("<li>");
                    str_allUserGroup.Append("<div ondblclick=\"DeleteMember('" + drv["UserInfoUserGroup_ID"] + "')\" title='成员'><img src=\"/Themes/Images/user_mature.png\" width=\"16\" height=\"16\" />" + drv["User_Name"] + "</div>");
                    str_allUserGroup.Append("</li>");
                }
            }
            else
            {
                str_allUserGroup.Append("<li>");
                str_allUserGroup.Append("<div><span style='color:red;'>暂无数据</span></div>");
                str_allUserGroup.Append("</li>");
            }
            return str_allUserGroup.ToString();
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}