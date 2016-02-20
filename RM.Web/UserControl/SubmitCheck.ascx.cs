using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RM.Common.DotNetUI;

namespace RM.Web.UserControl
{
    public partial class SubmitCheck : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 调用验证防止刷新重复提交方法
        /// </summary>
        /// <returns></returns>
        public string GetToken()
        {
            return WebHelper.GetToken();
        }
    }
}