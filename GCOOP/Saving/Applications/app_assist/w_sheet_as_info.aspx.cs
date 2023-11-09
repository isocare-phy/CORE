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
using DataLibrary;
using Sybase.DataWindow; 


namespace Saving.Applications.app_assist
{
    public partial class w_sheet_as_info : System.Web.UI.Page
    {
        private DwTrans sqlca;
        private WebState state;

        protected void Page_Load(object sender, EventArgs e)
        {
            state = new WebState(Session, Request);

            if (state.IsReadable)
            {
                sqlca = new DwTrans();
                sqlca.Connect();
                DwMain_loan.SetTransaction(sqlca);
                if (IsPostBack)
                {
                    DwMain_loan.RestoreContext();
                    try
                    {
                        String eventArg = Request["__EVENTARGUMENT"];

                    }
                    catch { }
                }
                else
                {
                    // !IsPostback
                    //DwMain.Retrieve("");
                }

                if (DwMain_loan.RowCount < 1)
                {
                    DwMain.InsertRow(0);
                    DwMain_loan.InsertRow(0);
                    DwMain_dept.InsertRow(0);
                    Dwdetail.InsertRow(0);
                }
            }
        }// end PageLoad
        protected void Page_loadComplete(object sender, EventArgs e)
        {
            sqlca.Disconnect();

        }
    }
}
