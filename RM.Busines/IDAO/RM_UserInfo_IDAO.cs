using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using RM.Common.DotNetCode;

namespace RM.Busines.IDAO
{
    /// <summary>
    /// 用户设置
    /// </summary>
    public interface RM_UserInfo_IDAO
    {
        #region 用户管理
        /// <summary>
        /// 后台登陆验证
        /// </summary>
        /// <param name="name">账户</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        DataTable UserLogin(string name, string pwd);
        /// <summary>
        /// 用户列表，分页
        /// </summary>
        /// <param name="SqlWhere">SQL条件</param>
        /// <param name="IList_param">参数</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="count">总条数</param>
        /// <returns></returns>
        DataTable GetUserInfoPage(StringBuilder SqlWhere, IList<SqlParam> IList_param, int pageIndex, int pageSize, ref int count);
        /// <summary>
        /// 所有用户信息
        /// </summary>
        /// <param name="SqlWhere">SQL条件</param>
        /// <param name="IList_param">参数</param>
        /// <returns></returns>
        DataTable GetUserInfoInfo(StringBuilder SqlWhere, IList<SqlParam> IList_param);
        /// <summary>
        /// 加载所属用户权限
        /// </summary>
        /// <param name="User_ID">用户主键</param>
        DataTable InitUserRight(string User_ID);
        /// <summary>
        /// 加载所属用户组
        /// </summary>
        /// <param name="User_ID">用户主键</param>
        DataTable InitUserInfoUserGroup(string User_ID);
        /// <summary>
        /// 加载所属角色
        /// </summary>
        /// <param name="User_ID">用户主键</param>
        DataTable InitUserRole(string User_ID);
        /// <summary>
        /// 加载所属部门
        /// </summary>
        /// <param name="User_ID">用户主键</param>
        DataTable InitStaffOrganize(string User_ID);
        #endregion

        #region 系统日志
        /// <summary>
        /// 用户登录日志
        /// </summary>
        /// <param name="SYS_USER_ACCOUNT">登录账户</param>
        /// <param name="SYS_LOGINLOG_STATUS">登录状态</param>
        /// <param name="SYS_LOGINLOG_STATUS">ip所在地</param>
        /// <returns></returns>
        void SysLoginLog(string SYS_USER_ACCOUNT, string SYS_LOGINLOG_STATUS, string OWNER_address);
        /// <summary>
        /// 登录日志列表
        /// </summary>
        /// <param name="SqlWhere">SQL条件</param>
        /// <param name="IList_param">参数</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="count">总条数</param>
        /// <returns></returns>
        DataTable GetSysLoginLogPage(StringBuilder SqlWhere, IList<SqlParam> IList_param, int pageIndex, int pageSize, ref int count);
        /// <summary>
        /// 获取登陆状况
        /// </summary>
        /// <param name="count">本月登录总数</param>
        /// <returns></returns>
        DataTable GetLogin_Info(ref int count);
        #endregion

        #region 部门管理
        /// <summary>
        /// 加载部门所有员工
        /// </summary>
        /// <returns></returns>
        DataTable Load_StaffOrganizeList();
        /// <summary>
        /// 部门列表
        /// </summary>
        /// <returns></returns>
        DataTable GetOrganizeList();
        #endregion

        #region 用户组管理
        /// <summary>
        /// 用户组列表
        /// </summary>
        /// <returns></returns>
        DataTable InitUserGroupList();
        /// <summary>
        /// 节点位置下拉框绑定
        /// </summary>
        /// <returns></returns>
        DataTable InitUserGroupParentId();
        /// <summary>
        /// 加载用户组成员
        /// </summary>
        /// <param name="UserGroup_ID">用户组主键</param>
        /// <returns></returns>
        DataTable Load_UserInfoUserGroupList(string UserGroup_ID);
        /// <summary>
        /// 加载所属用户组权限
        /// </summary>
        /// <param name="UserGroup_ID">用户组主键</param>
        DataTable InitUserGroupRight(string UserGroup_ID);
        /// <summary>
        /// 新增用户组成员
        /// </summary>
        /// <param name="User_ID">员工主键</param>
        /// <param name="UserGroup_ID">用户组主键</param>
        /// <returns></returns>
        bool AddUserGroupMenber(string[] User_ID, string UserGroup_ID);
        /// <summary>
        /// 用户组配权限
        /// </summary>
        /// <param name="pkVal">选择权限主键</param>
        /// <param name="UserGroup_ID">用户组主键</param>
        /// <returns></returns>
        bool Add_UserGroupAllotAuthority(string[] pkVal, string UserGroup_ID);
        #endregion
    }
}
