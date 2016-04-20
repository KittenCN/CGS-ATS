using RM.Web.App_Code;
using System;
using RM.Busines;
using RM.Common.DotNetCode;
using RM.Common.DotNetUI;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace RM.Web.RMBase.SysATS
{
    public partial class frmATS_OriDataIn : PageBase
    {
        protected System.Web.UI.HtmlControls.HtmlInputFile OriDataFile;
        DataTable dt = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            this.PageControl1.pageHandler += new EventHandler(pager_PageChanged);
            DataBindGrid();
            if (!IsPostBack)
            {

            }
        }

        private void DataBindGrid()
        {
            int count = 0;
            StringBuilder SqlWhere = new StringBuilder();
            IList<SqlParam> IList_param = new List<SqlParam>();
            //DataTable dt = DataFactory.SqlDataBase().GetDataTable("Base_ATS_OriDataIn");
            string sql = "select * from Base_ATS_OriDataIn";            
            StringBuilder sb_sql = new StringBuilder(sql);
           // DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb_sql);
            DataTable dt = DataFactory.SqlDataBase().GetPageList(sql, null, "OriData", "desc", PageControl1.PageIndex, PageControl1.PageSize, ref count);
            ControlBindHelper.BindRepeaterList(dt, rp_Item);
            this.PageControl1.RecordCount = Convert.ToInt32(count);
        }

        protected void pager_PageChanged(object sender, EventArgs e)
        {
            DataBindGrid();
        }

        protected void rp_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //Label lab_BeginFlag = e.Item.FindControl("lab_BeginFlag") as Label;
                //Label lab_EndFlag = e.Item.FindControl("lab_EndFlag") as Label;
                //if (lab_BeginFlag != null)
                //{
                //    string text = lab_BeginFlag.Text;
                //    text = text.Replace("1", "半天");
                //    text = text.Replace("0", "全天");
                //    lab_BeginFlag.Text = text;
                //}
                //if (lab_EndFlag != null)
                //{
                //    string text = lab_EndFlag.Text;
                //    text = text.Replace("1", "半天");
                //    text = text.Replace("0", "全天");
                //    lab_EndFlag.Text = text;
                //}
            }
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            if (OriDataFile.PostedFile != null && OriDataFile.PostedFile.ContentLength > 0)
            {
                string fn = System.IO.Path.GetFileName(OriDataFile.PostedFile.FileName);
                string SaveLocation = Server.MapPath("OriATSFiles") + "\\" + fn;
                try
                {
                    OriDataFile.PostedFile.SaveAs(SaveLocation);
                    //Response.Write("The file has been uploaded.");
                    ShowMsgHelper.AlertMsg("The file has been uploaded.");
                    bind(SaveLocation);
                }
                catch (Exception ex)
                {
                    //Response.Write("Error: " + ex.Message);
                    ShowMsgHelper.Alert_Error("Error: " + ex.Message);
                }
            }
            else
            {
                //Response.Write("Please select a file to upload.");
                ShowMsgHelper.Alert_Error("Please select a file to upload.");
            }
        }

        private void bind(string fileName)
        {
            string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" +
                 "Data Source=" + fileName + ";" +
                 "Extended Properties='Excel 8.0; HDR=Yes; IMEX=1'";
            OleDbDataAdapter da = new OleDbDataAdapter("SELECT *  FROM [Sheet1$]", strConn);
            DataSet ds = new DataSet();
            try
            {
                da.Fill(ds);
                dt = ds.Tables[0];
                this.GV_OriData.DataSource = dt;
                this.GV_OriData.DataBind();
                InsertToDB();
            }
            catch (Exception err)
            {
                ShowMsgHelper.Alert_Wern("Error！" + err.ToString());
            }
        }

        private void InsertToDB()
        {
            if (GV_OriData.Rows.Count > 0)
            {
                DataRow dr = null;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dr = dt.Rows[i];
                    insertToSql(dr);
                }
                ShowMsgHelper.AlertMsg("导入成功！");
            }
            else
            {
                ShowMsgHelper.Alert_Error("None Data！");
            }
        }

        private void insertToSql(DataRow dr)
        {
            //excel表中的列名和数据库中的列名一定要对应
            string Dept = dr[0].ToString();
            string OriID = dr[1].ToString();
            string Name = dr[2].ToString();
            string RegID = dr[3].ToString();
            string DeviceID = dr[4].ToString();
            string Posion = dr[5].ToString();
            string OriData = dr[6].ToString();
            string OriTime = dr[7].ToString();
            string ATSstatus = dr[8].ToString();
            string Remark = dr[9].ToString();
            string sql = "insert into Base_ATS_OriDataIn values('" + Dept + "','" + OriID + "','" + Name + "','" + RegID + "','" + DeviceID  + "','" + Posion + "','" + OriData + "','" + OriTime + "','" + ATSstatus + "','" + Remark + "')";
            StringBuilder sb_sql = new StringBuilder(sql);

            int int_Result = DataFactory.SqlDataBase().ExecuteBySql(sb_sql);

        }
    }
}