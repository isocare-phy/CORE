using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;

namespace Saving.Applications.cmd
{
    public partial class w_sheet_cmd_outproducts : PageWebSheet,WebSheet
    {

        public void InitJsPostBack()
        {
            
        }

        public void WebSheetLoadBegin()
        {
            this.ConnectSQLCA();

            DwMain.SetTransaction(sqlca);

            if (!IsPostBack)
            {
                DwMain.InsertRow(1);

            }
            else
            {
                try
                {
                    DwMain.Retrieve();
                }
                catch (Exception ex)
                {
                    String err = ex.ToString();
                }
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
           
        }

        public void SaveWebSheet()
        {
            
        }

        public void WebSheetLoadEnd()
        {
            DwMain.Retrieve();
            DwMain.SaveDataCache();
        }
    }
}