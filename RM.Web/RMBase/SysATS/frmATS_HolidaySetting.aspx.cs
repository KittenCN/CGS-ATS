using RM.Busines;
using RM.Common.DotNetCode;
using RM.Common.DotNetUI;
using RM.Web.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;

namespace RM.Web.RMBase.SysATS
{
    public partial class frmATS_HolidaySetting : PageBase
    {
        string _key, _ParentId;
        protected void Page_Load(object sender, EventArgs e)
        {
            _key = Request["key"];                  //主键
            _ParentId = Request["ParentId"];        //父节点
            DataBindGrid();
            if (!IsPostBack)
            {
                
            }
        }

        private void DataBindGrid()
        {
            StringBuilder SqlWhere = new StringBuilder();
            IList<SqlParam> IList_param = new List<SqlParam>();
            DataTable dt = DataFactory.SqlDataBase().GetDataTable("Base_ATS_HolidaySetting");
            ControlBindHelper.BindRepeaterList(dt, rp_Item);
            
        }

        protected void rp_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label lab_BeginFlag = e.Item.FindControl("lab_BeginFlag") as Label;
                Label lab_EndFlag = e.Item.FindControl("lab_EndFlag") as Label;
                if (lab_BeginFlag != null)
                {
                    string text = lab_BeginFlag.Text;
                    text = text.Replace("0", "Afternoon");
                    text = text.Replace("1", "Morning");
                    lab_BeginFlag.Text = text;
                }
                if (lab_EndFlag != null)
                {
                    string text = lab_EndFlag.Text;
                    text = text.Replace("0", "Morning");
                    text = text.Replace("1", "Aftermoon");
                    lab_EndFlag.Text = text;
                }
            }
        }

        protected void btn_CreateHoliday_Click(object sender, EventArgs e)
        {

        }

        protected void btn_EditHoliday_Click(object sender, EventArgs e)
        {

        }

        protected void btn_DelHoliday_Click(object sender, EventArgs e)
        {

        }
    }
}