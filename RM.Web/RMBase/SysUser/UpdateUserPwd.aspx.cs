using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RM.Common.DotNetBean;
using System.Collections;
using RM.Common.DotNetEncrypt;
using RM.Busines;
using RM.Web.App_Code;

namespace RM.Web.RMBase.SysUser
{
    public partial class UpdateUserPwd : PageBase
    {
        public string _PasPwd;
        protected void Page_Load(object sender, EventArgs e)
        {
            txtUserName.Value = RequestSession.GetSessionUser().UserAccount.ToString();
            _PasPwd = RequestSession.GetSessionUser().UserPwd.ToString();
        }
        /// <summary>
        /// 保存事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Save_Click(object sender, EventArgs e)
        {
            string s = Session["dt_session_code"].ToString().ToLower();
            if (txtCode.Value.ToLower() != Session["dt_session_code"].ToString().ToLower())
            {
                txtCode.Focus();
                errorMsg.InnerHtml = "验证码输入不正确！";
                return;
            }
            int i = 0;
            if (txtUserName.Value != "Administrator")
            {
                Hashtable ht_User = new Hashtable();
                ht_User["User_Pwd"] = Md5Helper.MD5(txtUserPwd.Value, 32);
                i = DataFactory.SqlDataBase().UpdateByHashtable("Base_UserInfo", "User_ID", RequestSession.GetSessionUser().UserId.ToString(), ht_User);
            }
            if (i > 0)
            {
                Session.Abandon();  //取消当前会话
                Session.Clear();    //清除当前浏览器所以Session
                Response.Write("<script>alert('登陆修改成功,请重新登陆');top.location.href='/Index.htm'</script>");
            }
            else
            {
                errorMsg.InnerHtml = "修改登录密码失败";
            }
        }
    }
}