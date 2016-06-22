using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Xml;

namespace GVdata
{
    public class GVdata
    {
        public static string CheckCbx(GridView GVData, string CheckBoxName, string LabID)
        {
            string str = "";
            for (int i = 0; i < GVData.Rows.Count; i++)
            {
                GridViewRow row = GVData.Rows[i];
                System.Web.UI.WebControls.CheckBox Chk = (System.Web.UI.WebControls.CheckBox)row.FindControl(CheckBoxName);
                System.Web.UI.WebControls.Label LabVis = (System.Web.UI.WebControls.Label)row.FindControl(LabID);
                if (Chk.Checked == true)
                {
                    if (str == "")
                    {
                        str = LabVis.Text.ToString();
                    }
                    else
                    {
                        str = str + "," + LabVis.Text.ToString();
                    }
                }
            }
            return str;
        }
    }
}
