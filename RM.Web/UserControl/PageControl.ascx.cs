using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RM.Common.DotNetUI;
using RM.Common.DotNetCode;
using RM.Common.DotNetBean;

namespace RM.Web.UserControl
{
    public partial class PageControl : System.Web.UI.UserControl
    {
        #region 参数
        /// <summary>
        /// 判断下拉框值是否存在
        /// </summary>
        /// <param name="obj_value">值</param>
        /// <returns></returns>
        public bool IsListItem(string obj_value)
        {
            bool isok = false;
            foreach (ListItem item in ddlpageList.Items)
            {
                if (item.Value.Trim() == obj_value.ToString())
                {
                    isok = true;
                    break;
                }
            }
            return isok;
        }
        /// <summary>
        /// 页大小
        /// </summary>
        public int PageSize
        {
            get
            {
                if (CookieHelper.GetCookie("PageIndex") == "")
                {
                    return int.Parse(ddlpageList.Text);
                }
                else
                {
                    ddlpageList.Text = CookieHelper.GetCookie("PageIndex");
                }
                return int.Parse(ddlpageList.Text);
            }
            set
            {
                if (!IsListItem(value.ToString()))
                {
                    ddlpageList.Items.Add(value.ToString());
                }
                ddlpageList.Text = value.ToString();
            }
        }
        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex
        {
            get
            {
                return int.Parse(lblCurrentPageIndex.Text);
            }
            set
            {
                lblCurrentPageIndex.Text = value.ToString();
            }
        }
        /// <summary>
        /// 开始显示数量
        /// </summary>
        public int PagestartNumber
        {
            get
            {
                return ((PageIndex - 1) * PageSize) + 1;
            }
        }
        /// <summary>
        /// 结束显示数量
        /// </summary>
        public int PageendNumber
        {
            get
            {
                return (PageIndex) * PageSize;
            }
        }
        /// <summary>
        /// 总数量
        /// </summary>
        public int RecordCount { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public int TotaPage
        {
            get
            {
                return RecordCount % PageSize == 0 ? RecordCount / PageSize : RecordCount / PageSize + 1;
            }
        }
        #endregion

        public event EventHandler pageHandler;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                pager_PageChanged(sender, e);
            }
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void pager_PageChanged(object sender, EventArgs e)
        {
            pageHandler(sender, e);
            PageChecking();
        }
        /// <summary>
        /// 首页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void hlkFirst_Click(object sender, EventArgs e)
        {
            this.PageIndex = 1;
            pager_PageChanged(sender, e);
            PageChecking();
        }
        /// <summary>
        /// 上页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void hlkPrev_Click(object sender, EventArgs e)
        {
            this.PageIndex = PageIndex - 1;
            pager_PageChanged(sender, e);
            PageChecking();
        }
        /// <summary>
        /// 下页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void hlkNext_Click(object sender, EventArgs e)
        {
            this.PageIndex = PageIndex + 1;
            pager_PageChanged(sender, e);
            PageChecking();
        }
        /// <summary>
        /// 尾页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void hlkLast_Click(object sender, EventArgs e)
        {
            this.PageIndex = int.Parse(lblPageCount.Text);
            pager_PageChanged(sender, e);
            PageChecking();
        }
        /// <summary>
        /// 验证
        /// </summary>
        public void PageChecking()
        {
            lblRecordCount.Text = RecordCount.ToString();
            lblCurrentPageIndex.Text = PageIndex.ToString();
            lblPageCount.Text = TotaPage.ToString();
            default_pgStartRecord.Text = PagestartNumber.ToString();
            default_pgEndRecord.Text = PageendNumber.ToString();
            if (TotaPage == 0 || TotaPage == 1)
            {
                hlkFirst.Enabled = false;
                hlkPrev.Enabled = false;
                hlkNext.Enabled = false;
                hlkLast.Enabled = false;
                ShowMsgHelper.ShowScript("Script(1)");
            }
            else if (PageIndex == 1)
            {
                hlkFirst.Enabled = false;
                hlkPrev.Enabled = false;
                hlkNext.Enabled = true;
                hlkLast.Enabled = true;
                ShowMsgHelper.ShowScript("Script(2)");
            }
            else if (PageIndex == TotaPage)
            {
                hlkFirst.Enabled = true;
                hlkPrev.Enabled = true;
                hlkNext.Enabled = false;
                hlkLast.Enabled = false;
                ShowMsgHelper.ShowScript("Script(3)");
            }
            else
            {
                hlkFirst.Enabled = true;
                hlkPrev.Enabled = true;
                hlkNext.Enabled = true;
                hlkLast.Enabled = true;
                ShowMsgHelper.ShowScript("Script(4)");
            }
        }
        /// <summary>
        /// 跳转条数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlpageList_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.PageSize = int.Parse(ddlpageList.Text);
            pager_PageChanged(sender, e);
        }
    }
}