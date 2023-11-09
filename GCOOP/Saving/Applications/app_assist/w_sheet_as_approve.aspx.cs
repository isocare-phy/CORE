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
//  เทส Control version
namespace Saving.Applications.app_assist
{
    public partial class w_sheet_as_approve_scholarship : System.Web.UI.Page
    {
        private DwTrans sqlca;
        private WebState state;
        // test ด้วย
        protected void Page_Load(object sender, EventArgs e)
        {
            state = new WebState(Session, Request);

            if (state.IsReadable)
            {
                sqlca = new DwTrans();
                sqlca.Connect();
                DwMain.SetTransaction(sqlca);
                if (IsPostBack)
                {
                    DwMain.RestoreContext();
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

                if (DwMain.RowCount < 1)
                {
                    DwMain.InsertRow(10);
                    DwDetail.InsertRow(0);
                }
            }
        }// end PageLoad
        protected void Page_loadComplete(object sender, EventArgs e)
        {
            sqlca.Disconnect();

        }
    }
}
