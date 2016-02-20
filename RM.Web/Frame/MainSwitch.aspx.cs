using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RM.Common.DotNetBean;

namespace RM.Web.Frame
{
    public partial class MainSwitch : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string Menu_Type = CookieHelper.GetCookie("Menu_Type");
            if (Menu_Type == "0")
            {
                Response.Redirect("~/Frame/MainDefault.aspx");
            }
            else if (Menu_Type == "1")
            {
                Response.Redirect("~/Frame/MainDefault.aspx");
            }
            else if (Menu_Type == "2")
            {
                Response.Redirect("~/Frame/MainIndex.aspx");
            }
            else if (Menu_Type == "3")
            {
                Response.Redirect("~/Frame/MainTree.aspx");
            }
            else
            {
                Response.Redirect("~/Frame/MainDefault.aspx");
            }
        }
    }
}