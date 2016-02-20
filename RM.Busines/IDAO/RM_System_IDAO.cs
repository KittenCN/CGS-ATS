using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using RM.Common.DotNetCode;

namespace RM.Busines.IDAO
{
    /// <summary>
    /// 系统业务设置接口
    /// </summary>
    public interface RM_System_IDAO
    {
        #region 公用接口
        /// <summary>
        /// 删除信息
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="pkName">字段主键</param>
        /// <param name="pkVal">字段值</param>
        /// <returns></returns>
        int DeleteData_Base(string tableName, string pkName, string[] pkVal);
        #endregion

        #region 菜单导航
        /// <summary>
        /// 菜单所有列表
        /// </summary>
        /// <returns></returns>
        DataTable GetMenuBind();
        /// <summary>
        /// 菜单树列表
        /// </summary>
        /// <returns></returns>
        DataTable GetMenuList();
        /// <summary>
        /// 权限菜单导航
        /// </summary>
        /// <param name="UserId">用户主键</param>
        /// <returns></returns>
        DataTable GetMenuHtml(string UserId);
        /// <summary>
        /// 根据菜单主键获取已有按钮
        /// </summary>
        /// <returns></returns>
        DataTable GetSysMenuByButton(string Menu_Id);
        /// <summary>
        /// 所有按钮信息
        /// </summary>
        /// <returns></returns>
        DataTable GetButtonList();
        /// <summary>
        /// 分配按钮
        /// </summary>
        /// <param name="pkVal">全中按钮ID</param>
        /// <param name="ParentId">菜单ID</param>
        /// <returns></returns>
        int AllotButton(string pkVal, string ParentId);
        /// <summary>
        /// 动态显示按钮
        /// <param name="User_ID">用户主键</param>
        /// </summary>
        /// <returns></returns>
        DataTable GetButtonHtml(string User_ID);
        #endregion

        #region 系统权限
        /// <summary>
        /// 根据用户主键，显示拥有所有权限
        /// </summary>
        /// <param name="User_ID">用户主键</param>
        /// <returns></returns>
        DataTable GetHaveRightUserInfo(string User_ID);
        /// <summary>
        /// URL权限验证
        /// </summary>
        /// <param name="UserId">用户主键</param>
        /// <returns></returns>
        DataTable GetPermission_URL(string UserId);
        #endregion  /// <param name="User_ID">用户主键</param>

        #region 角色管理
        /// <summary>
        /// 角色列表
        /// </summary>
        /// <returns></returns>
        DataTable InitRoleList();
        /// <summary>
        /// 节点位置下拉框绑定
        /// </summary>
        /// <returns></returns>
        DataTable InitRoleParentId();
        /// <summary>
        /// 角色分配成员
        /// </summary>
        /// <param name="pkVal">选择成员主键</param>
        /// <param name="Roles_ID">角色主键</param>
        /// <returns></returns>
        bool Add_RoleAllotMember(string[] pkVal, string Roles_ID);
        /// <summary>
        /// 加载所属角色权限
        /// </summary>
        /// <param name="Roles_ID">角色主键</param>
        DataTable InitRoleRight(string Roles_ID);
        /// <summary>
        /// 加载所属角色成员
        /// </summary>
        /// <param name="Roles_ID">角色主键</param>
        DataTable InitUserRole(string Roles_ID);
        /// <summary>
        /// 角色分配权限
        /// </summary>
        /// <param name="pkVal">选择权限主键</param>
        /// <param name="Roles_ID">角色主键</param>
        /// <returns></returns>
        bool Add_RoleAllotAuthority(string[] pkVal, string Roles_ID);
        /// <summary>
        /// 根据成员ID获取所属角色
        /// </summary>
        /// <param name="user_id">用户主键</param>
        /// <returns></returns>
        DataTable GetRoleByMember(string user_id);
        #endregion

        #region 附加属性信息
        /// <summary>
        /// 业务附加属性列表
        /// </summary>
        /// <param name="Function">业务功能</param>
        /// <returns></returns>
        DataTable AppendProperty_List(string Function);
        /// <summary>
        /// 业务功能列表
        /// </summary>
        /// <returns></returns>
        DataTable AppendProperty_Function();
        /// <summary>
        /// 根据业务名称获取附加属性值
        /// </summary>
        /// <param name="Property_Function">所属功能</param>
        /// <param name="Obj_ID">业务主键</param>
        /// <returns></returns>
        string GetPropertyInstancepk(string Property_Function, string Obj_ID);
        /// <summary>
        /// 加载动态显示附加属性(返回Input)
        /// </summary>
        /// <param name="Function"></param>
        /// <returns>返回html格式</returns>
        string AppendProperty_Html(string Function);
        /// <summary>
        /// 加载动态显示附加属性(返回Label)
        /// </summary>
        /// <param name="Function"></param>
        /// <returns>返回html格式</returns>
        string AppendProperty_HtmlLabel(string Function);
        /// <summary>
        /// 新增附加属性实例
        /// </summary>
        /// <param name="guid">对象业务属性主键</param>
        /// <param name="arrayitem">附加属性实例值</param>
        /// <returns></returns>
        bool Add_AppendPropertyInstance(string guid, string[] arrayitem);
        #endregion

        #region 首页快捷功能
        /// <summary>
        /// 首页快捷功能
        /// </summary>
        /// <param name="User_ID">用户主键</param>
        /// <returns></returns>
        DataTable GetHomeShortcut_List(string User_ID);
        #endregion

        #region 回收站
        /// <summary>
        /// 获取某一个表的字段，说明
        /// </summary>
        /// <param name="object_TableName"></param>
        /// <returns></returns>
        DataTable GetRecyclebin_ObjField(string object_TableName);
        /// <summary>
        /// 数据放入回收站 
        /// </summary>
        /// <param name="module">业务模块</param>
        /// <param name="tableName">表名</param>
        /// <param name="pkName">字段主键</param>
        /// <param name="pkVal">字段值</param>
        /// <returns></returns>
        int Virtualdelete(string module, string tableName, string pkName, string[] pkVal);
        /// <summary>
        /// 还原回收站数据 
        /// </summary>
        /// <param name="pkVal">字段值</param>
        /// <returns></returns>
        int Recyclebin_Restore(string[] pkVal);
        /// <summary>
        /// 清空回收站数据 
        /// </summary>
        /// <param name="pkVal">字段值</param>
        /// <returns></returns>
        int Recyclebin_Empty(string[] pkVal);
        #endregion

        #region 数据库管理
        /// <summary>
        /// 还原恢复数据库
        /// </summary>
        /// <param name="FilePath">保存路径</param>
        /// <returns></returns>
        bool DataRestore(string FilePath);
        /// <summary>
        /// 备份数据库
        /// </summary>
        /// <param name="FilePath">保存路径</param>
        /// <returns></returns>
        bool DataBackups(string FilePath);
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
        void Add_Backup_Restore_Log(string Type, string File, string Size, string CreateUserName, string DB, string Memo);
        /// <summary>
        /// 数据库备份还原记录
        /// </summary>
        /// <returns></returns>
        DataTable GetBackup_Restore_Log_List();
        /// <summary>
        /// 获取数据库所有表名
        /// </summary>
        /// <returns></returns>
        DataTable GetSysobjects();
        /// <summary>
        /// 获取某一个表的所有字段
        /// </summary>
        /// <param name="object_id">表名</param>
        /// <returns></returns>
        DataTable GetSyscolumns(string object_id);
        #endregion
    }
}
