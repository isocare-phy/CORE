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
    public partial class w_sheet_as_distance_province : PageWebSheet, WebSheet
    {
        protected String postRetrieve;
        protected String postRefresh;
        protected String postSave;
        protected String postInsertRow;
        protected String postDeleteRow;



        public void InitJsPostBack()
        {
            postInsertRow = WebUtil.JsPostBack(this, "postInsertRow");
            postDeleteRow = WebUtil.JsPostBack(this, "postDeleteRow");
            postSave = WebUtil.JsPostBack(this, "postSave");
            postRetrieve = WebUtil.JsPostBack(this, "postRetrieve");
            postRefresh = WebUtil.JsPostBack(this, "postRefresh");
        }

        public void WebSheetLoadBegin()
        {
            this.ConnectSQLCA();

            if (!IsPostBack)
            {
                DwHead.InsertRow(0);
            }
            else
            {
                this.RestoreContextDw(DwHead);
                this.RestoreContextDw(DwMain);
            }

            DwUtil.RetrieveDDDW(DwHead, "start_province_name", "as_seminar.pbl", null);
            DwUtil.RetrieveDDDW(DwHead, "end_province_name", "as_seminar.pbl", null);
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postRetrieve")
            {
                Retrieve();
            }
            else if (eventArg == "postRefresh")
            {
                Refresh();
            }
            else if (eventArg == "postSave")
            {
                Save();
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

        }

        public void WebSheetLoadEnd()
        {
            DwHead.SaveDataCache();
            DwMain.SaveDataCache();
        }



        private void Save()
        {
            try
            {
                DwMain.SetTransaction(sqlca);
                DwMain.UpdateData();
                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกเรียบร้อย");
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        private void Refresh()
        {

        }

        private void Retrieve()
        {
            String post_start_code, post_end_code;

            post_start_code = DwHead.GetItemString(1, "start_province_name");
            post_end_code = DwHead.GetItemString(1, "end_province_name");

            DwMain.SetTransaction(sqlca);
            DwMain.Retrieve(post_start_code, post_end_code);
        }

        private void JsPostDeleteRow()
        {
            int row = int.Parse(HdMainRow.Value);
            DwMain.DeleteRow(row);
        }

        private void JsPostInsertRow()
        {
            DwMain.InsertRow(0);
        }
    }
}
