using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using RM.Common.DotNetUI;
using RM.Busines.IDAO;
using RM.Busines.DAL;
using RM.Common.DotNetCode;
using System.Text;
using RM.Common.DotNetData;
using RM.Web.App_Code;

namespace RM.Web.RMBase.SysUser
{
    public partial class UserInfo_List : PageBase
    {
        RM_UserInfo_IDAO user_idao = new RM_UserInfo_Dal();
        string _Organization_ID;
        protected void Page_Load(object sender, EventArgs e)
        {
            _Organization_ID = Request["Organization_ID"];//部门主键
            this.PageControl1.pageHandler += new EventHandler(pager_PageChanged);
            if (!IsPostBack)
            {
                
            }
        }
        /// <summary>
        /// 绑定数据，分页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void pager_PageChanged(object sender, EventArgs e)
        {
            DataBindGrid();
        }
        /// <summary>
        /// 绑定数据源
        /// </summary>
        private void DataBindGrid()
        {
            int count = 0;
            StringBuilder SqlWhere = new StringBuilder();
            IList<SqlParam> IList_param = new List<SqlParam>();
            if (!string.IsNullOrEmpty(txt_Search.Value))
            {
                SqlWhere.Append(" and U." + Searchwhere.Value + " like @obj ");
                IList_param.Add(new SqlParam("@obj", '%' + txt_Search.Value.Trim() + '%'));
            }
            if(_Organization_ID==null || _Organization_ID.Length>40)
            {
                _Organization_ID = null;
            }
            if (!string.IsNullOrEmpty(_Organization_ID))
            {
                SqlWhere.Append(" AND S.Organization_ID IN(" + _Organization_ID + ")");
            }
            DataTable dt = user_idao.GetUserInfoPage(SqlWhere, IList_param, PageControl1.PageIndex, PageControl1.PageSize, ref count);
            ControlBindHelper.BindRepeaterList(dt, rp_Item);
            this.PageControl1.RecordCount = Convert.ToInt32(count);
        }
        /// <summary>
        /// 绑定后激发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rp_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label lblUser_Sex = e.Item.FindControl("lblUser_Sex") as Label;
                Label lblDeleteMark = e.Item.FindControl("lblDeleteMark") as Label;
                if (lblUser_Sex != null)
                {
                    string text = lblUser_Sex.Text;
                    text = text.Replace("1", "男士");
                    text = text.Replace("0", "女士");
                    lblUser_Sex.Text = text;

                    string textDeleteMark = lblDeleteMark.Text;
                    textDeleteMark = textDeleteMark.Replace("1", "<span style='color:Blue'>启用</span>");
                    textDeleteMark = textDeleteMark.Replace("2", "<span style='color:red'>停用</span>");
                    lblDeleteMark.Text = textDeleteMark;
                }
            }
        }
        /// <summary>
        /// 筛选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtSearch_Click(object sender, EventArgs e)
        {
            DataBindGrid();
        }
    }
}