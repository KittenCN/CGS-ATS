using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using RM.Common.DotNetUI;
using RM.Busines.IDAO;
using RM.Busines.DAL;
using RM.Common.DotNetCode;
using RM.Common.DotNetData;
using RM.Web.App_Code;

namespace RM.Web.RMBase.SysUserGroup
{
    public partial class UserGroupSet : PageBase
    {
        public string _UserGroup_ID, _UserGroup_Name;
        protected void Page_Load(object sender, EventArgs e)
        {
            _UserGroup_ID = Request["UserGroup_ID"];//主键
            _UserGroup_Name = Server.UrlDecode(Request["UserGroup_Name"]);//用户组名称
        }
    }
}