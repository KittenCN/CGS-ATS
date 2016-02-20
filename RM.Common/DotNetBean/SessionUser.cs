using System;
using System.Collections.Generic;
using System.Text;

namespace RM.Common.DotNetBean
{
    /// <summary>
    /// 存 Session对象
    /// </summary>
    public class SessionUser
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public object UserId { get; set; }
        /// <summary>
        /// 登陆账户
        /// </summary>
        public object UserAccount { get; set; }
        /// <summary>
        /// 登陆密码
        /// </summary>
        public object UserPwd { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public object UserName { get; set; }

        public SessionUser(object userId, object userAccount,object userPwd ,object userName)
        {
            this.UserId = userId;
            this.UserAccount = userAccount;
            this.UserName = userName;
            this.UserPwd = userPwd;
        }
        public SessionUser()
        {
        }
    }
}
