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

namespace Saving.Applications.hr
{
    public partial class w_sheet_hrtax_table : PageWebSheet, WebSheet
    {
        protected String newRecord;

        #region WebSheet Members

        public void InitJsPostBack()
        {
            newRecord = WebUtil.JsPostBack(this, "newRecord");
        }

        public void WebSheetLoadBegin()
        {
            try {
                this.ConnectSQLCA();
                DwMain.SetTransaction(sqlca);
                DwMain.Retrieve();
                if (!IsPostBack)
                {
                    if (DwMain.RowCount < 1)
                    {
                        DwMain.InsertRow(0);
                    }
                }
                else
                {
                    try
                    {
                        DwMain.RestoreContext();

                    }
                    catch { }
                }
            }
            catch (Exception e){
                LtServerMessage.Text = WebUtil.ErrorMessage(e.ToString());
            }
            

        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "newRecord")
            {
                //comment because of new record , it's user already
                DwMain.InsertRow(0);
            }
        }

        public void SaveWebSheet()
        {
            //Check values before save record
            //check new record or edit row

            try
            {
                DwMain.UpdateData();
                DwMain.Reset();
                DwMain.Retrieve();
            }
            catch (Exception e)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถเพิ่มรายการได้");
            }

        }

        public void WebSheetLoadEnd()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
