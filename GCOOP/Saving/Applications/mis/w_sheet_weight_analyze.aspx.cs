using System;
using CoreSavingLibrary;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;

namespace Saving.Applications.mis
{
    public partial class w_sheet_weight_analyze : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                dw_pastyear.InsertRow(0);
                dw_weight.InsertRow(0);
                MultiView1.SetActiveView(View1);
                Label1.Text = "ต้นทุนจ่าย";
                
               
                
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            MultiView1.SetActiveView(View1);
            Label1.Text = "ต้นทุนจ่าย";
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            MultiView1.SetActiveView(View2);
            Label1.Text = "ต้นทุนรับ";
        }
    }
}
