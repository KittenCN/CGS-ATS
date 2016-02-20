using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using RM.Busines.IDAO;
using RM.Busines.DAL;
using RM.Common.DotNetJson;

namespace RM.Web.WebService
{
    /// <summary>
    /// 权限系统开放给外部系统公共接口
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class RM_Interface : System.Web.Services.WebService
    {
        RM_System_IDAO systemidao = new RM_System_Dal();
        /// <summary>
        /// 权限菜单导航
        /// </summary>
        /// <param name="UserId">用户主键</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true, Description = "权限菜单导航（返回JSON格式），参数 UserId:用户主键")]
        public string GetMenuInfo(string UserId)
        {
            return JsonHelper.DataTableToJson(systemidao.GetMenuHtml(UserId), "RM");
        }
        /// <summary>
        /// URL权限验证,拒绝，不合法的请求
        /// </summary>
        /// <param name="UserId">用户主键</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true, Description = "URL权限验证,拒绝，不合法的请求（返回JSON格式），参数 UserId:用户主键")]
        public string GetURLPermission(string UserId)
        {
            return JsonHelper.DataTableToJson(systemidao.GetPermission_URL(UserId), "RM");
        }
        /// <summary>
        /// 权限操作按钮
        /// </summary>
        /// <param name="UserId">用户主键</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true, Description = "权限操作按钮（返回JSON格式），参数 UserId:用户主键")]
        public string GetButtonHtml(string UserId)
        {
            return JsonHelper.DataTableToJson(systemidao.GetButtonHtml(UserId), "RM");
        }
        /// <summary>
        /// 显示拥有所有权限
        /// </summary>
        /// <param name="UserId">用户主键</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true, Description = "显示拥有所有权限（返回JSON格式），参数 UserId:用户主键")]
        public string GetHaveRightUserInfo(string UserId)
        {
            return JsonHelper.DataTableToJson(systemidao.GetHaveRightUserInfo(UserId), "RM");
        }
    }
}
