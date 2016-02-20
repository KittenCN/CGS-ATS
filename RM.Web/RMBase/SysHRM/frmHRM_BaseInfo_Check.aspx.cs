using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using RM.Common.DotNetUI;
using RM.Busines;
using System.Text;
using RM.Common.DotNetData;
using System.Data;
using RM.Common.DotNetBean;
using RM.Common.DotNetCode;
using RM.Web.App_Code;

namespace RM.Web.RMBase.SysHRM
{
    public partial class frmHRM_BaseInfo_Check : System.Web.UI.Page
    {
        string _key, _ParentId;
        protected void Page_Load(object sender, EventArgs e)
        {
            _key = Request["key"];                  //主键
            _ParentId = Request["ParentId"];        //父节点
            DatabindtoGridview();

            if (!IsPostBack)
            {
                //InitParentId();
                //if (!string.IsNullOrEmpty(_ParentId))
                //{
                //    ParentId.Value = _ParentId;
                //}
                //if (!string.IsNullOrEmpty(_key))
                //{
                //    InitData();
                //}
            }
        }
        protected void DatabindtoGridview()
        {
            GridView1.DataSource = DataFactory.SqlDataBase().GetDataTable("Base_HRM_BaseInfo");
            GridView1.DataBind();
            int MaxR = GridView1.Rows.Count;
            for(int x=0; x<MaxR; x++)
            {
                if(GridView1.Rows[x].Cells[8].Text=="1")
                {
                    GridView1.Rows[x].Cells[8].Text = "在职";
                }
                if (GridView1.Rows[x].Cells[8].Text == "0")
                {
                    GridView1.Rows[x].Cells[8].Text = "离职";
                }
                if (GridView1.Rows[x].Cells[7].Text == "1900-1-1")
                {
                    GridView1.Rows[x].Cells[7].Text = "-";
                }
            }
        }
        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            //int SelectedNum = GridView1.SelectedIndex;
            //string CheckStr = ZWL.Common.PublicMethod.CheckCbx(this.GVData, "CheckSelect", "LabVisible");
            string CheckStr = GVdata.GVdata.CheckCbx(this.GridView1, "CheckSelect", "LabVisible");
            ShowMsgHelper.AlertMsg(CheckStr);
        }

    }
}