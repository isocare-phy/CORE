using System;
using CoreSavingLibrary;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using DataLibrary;
using Sybase.DataWindow;
using System.Web.Services.Protocols;

namespace Saving.Applications.app_assist
{
    public partial class w_sheet_as_edit_send_invite_jobs : PageWebSheet, WebSheet
    {
        protected String postChangeType;
        protected String postInsertRow;
        protected String postDeleteRow;

        #region WebSheet Members

        public void InitJsPostBack()
        {
            postInsertRow = WebUtil.JsPostBack(this, "postInsertRow");
            postDeleteRow = WebUtil.JsPostBack(this, "postDeleteRow");
            postChangeType = WebUtil.JsPostBack(this, "postChangeType");
        }

        public void WebSheetLoadBegin()
        {
            this.ConnectSQLCA();

            if (!IsPostBack)
            {
                DwMain.InsertRow(0);
                DwList.SetTransaction(sqlca);
                DwList.Retrieve();
            }
            else
            {
                this.RestoreContextDw(DwMain);
                this.RestoreContextDw(DwList);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postChangeType")
            {
                ChangeType();
            }
            else if (eventArg == "postInsertRow")
            {
                JsPostInsertRow();
            }
            else if (eventArg == "postDeleteRow")
            {
                JsPostDeleteRow();
            }
        }

        public void SaveWebSheet()
        {
            if (DwList.Visible == true)
            {
                DwList.SetTransaction(sqlca);
                DwList.UpdateData();
            }
            else if (DwListMem.Visible == true)
            {
                DwListMem.SetTransaction(sqlca);
                DwListMem.UpdateData();
            }
        }

        public void WebSheetLoadEnd()
        {
            DwMain.SaveDataCache();
            DwList.SaveDataCache();
        }

        #endregion


        private void ChangeType()
        {
            Decimal mem_type;

            mem_type = DwMain.GetItemDecimal(1, "mem_type");

            if (mem_type == 1)
            {
                DwList.Reset();
                DwList.SetTransaction(sqlca);
                DwList.Retrieve();
                DwListMem.Reset();
                DwList.Visible = true;
                DwListMem.Visible = false;
            }
            else if (mem_type == 2)
            {
                DwListMem.Reset();
                DwListMem.SetTransaction(sqlca);
                DwListMem.Retrieve();
                DwList.Reset();
                DwList.Visible = false;
                DwListMem.Visible = true;
            }
        }

        private void JsPostDeleteRow()
        {
            int row = int.Parse(HdListRow.Value);

            if (DwList.Visible == true)
            {
                DwList.DeleteRow(row);
            }
            else if (DwListMem.Visible == true)
            {
                DwListMem.DeleteRow(row);
            }
        }

        private void JsPostInsertRow()
        {
            if (DwList.Visible == true)
            {
                DwList.InsertRow(0);
            }
            else if (DwListMem.Visible == true)
            {
                DwListMem.InsertRow(0);
            }
        }
    }
}
