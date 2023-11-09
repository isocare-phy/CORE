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
using CoreSavingLibrary.WcfNAccount;
using Sybase.DataWindow;
using System.Web.Services.Protocols;
using System.Globalization;
using Sybase.DataWindow.Web;
using System.IO;

namespace Saving.Applications.account
{
    public partial class w_acc_edit_detail_finslip : PageWebSheet, WebSheet
    {
        DataStore DStore;
        private n_accountClient acc;
        protected String postNewClear;
        protected String jsButtonShowData;
        protected String expExcel;
        private DwThDate tDwMain;

        private void JspostNewClear()
        {
            Dw_main.Reset();
            Dw_main.InsertRow(0);
            Dw_main1.Reset();
            Dw_main1.InsertRow(0);
        }

        public void InitJsPostBack()
        {
            postNewClear = WebUtil.JsPostBack(this, "postNewClear");
            jsButtonShowData = WebUtil.JsPostBack(this, "jsButtonShowData");
            expExcel = WebUtil.JsPostBack(this, "expExcel");
            //=========================
            tDwMain = new DwThDate(Dw_main, this);
            tDwMain.Add("start_date", "start_tdate");
            //tDwMain.Add("end_date", "end_tdate");
        }

        public void WebSheetLoadBegin()
        {
            DwUtil.RetrieveDDDW(Dw_main, "acc_id", "vc_voucher_edit.pbl", null);
            try
            {
                this.ConnectSQLCA();
            }
            catch { LtServerMessage.Text = WebUtil.CompleteMessage("ติดต่อ Database ไม่ได้"); }

            try
            {
                acc = wcf.NAccount;
            }
            catch { LtServerMessage.Text = WebUtil.CompleteMessage("ติดต่อ Service ไม่ได้"); }

            Dw_main.SetTransaction(sqlca);
            Dw_main1.SetTransaction(sqlca);

            if (!IsPostBack)
            {
                JspostNewClear();
                detail.Visible = false;
            }
            else
            {
                detail.Visible = true;
                this.RestoreContextDw(Dw_main);
                this.RestoreContextDw(Dw_main1);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postNewClear")
            {
                JspostNewClear();
            }
            else if (eventArg == "jsButtonShowData")
            {
                JsButtonShowData();
            }
            else if (eventArg == "expExcel")
            {
                SaveFile();
            }
        }

        public void SaveWebSheet()
        {

        }

        public void WebSheetLoadEnd()
        {
            Dw_main.SaveDataCache();
            Dw_main1.SaveDataCache();
        }

        private void JsButtonShowData()
        {
            try
            {
                DateTime startDate = Dw_main.GetItemDateTime(1, "start_date");
                String acc_id = Dw_main.GetItemString(1, "acc_id");
                //DateTime endDate = Dw_main.GetItemDateTime(1, "end_date");
                Dw_main1.Retrieve(startDate, acc_id, state.SsCoopControl);
            }
            catch (Exception ex)
            {

            }
        }

        private void SaveFile()
        {
            try
            {
                Dw_main1.UpdateData();
                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลเรียบร้อยแล้ว");
            }
            catch (SoapException ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("เกิดข้อผิดพลาด " + WebUtil.SoapMessage(ex));
            }
        }
    }
}
