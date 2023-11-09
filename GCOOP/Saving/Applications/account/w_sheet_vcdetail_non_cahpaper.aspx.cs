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

namespace Saving.Applications.account
{
    public partial class w_sheet_vcdetail_non_cahpaper : PageWebSheet,WebSheet
    {
        private DwThDate tDwMain;
        private n_accountClient accService;
        protected String GetVcdetailForsetNoncash;
        protected String refresh;
        #region WebSheet Members

        public void InitJsPostBack()
        {
            GetVcdetailForsetNoncash = WebUtil.JsPostBack(this, "GetVcdetailForsetNoncash");
            refresh = WebUtil.JsPostBack(this, "refresh");
            tDwMain = new DwThDate(DwMain, this);
            tDwMain.Add("start_date", "start_tdate");
            tDwMain.Add("end_date", "end_tdate");
        }

        public void WebSheetLoadBegin()
        {
            accService = wcf.NAccount;
            if (!IsPostBack)
            {
                DwMain.InsertRow(0);
                DwMain.SetItemDateTime(1, "start_date", state.SsWorkDate);
                DwMain.SetItemDateTime(1, "end_date", state.SsWorkDate);
                tDwMain.Eng2ThaiAllRow();
            }
            else
            {
                this.RestoreContextDw(DwMain);
                this.RestoreContextDw(DwDetail);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "GetVcdetailForsetNoncash")
            {
                JsGetVcdetailForsetNoncash();
            }
        }

        public void SaveWebSheet()
        {
            String xmlDetail = "";
            xmlDetail = DwDetail.Describe("DataWindow.Data.XML");
            DateTime startDate = DwMain.GetItemDateTime(1, "start_date");
            DateTime endDate = DwMain.GetItemDateTime(1, "end_date");
            String accountId = DwMain.GetItemString(1, "account_id");
            try
            {
                accService.of_save_vcset_noncash(state.SsWsPass, xmlDetail, startDate, endDate, accountId);
                 
                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลเรียบร้อยแล้ว...");
                DwMain.Reset();
                DwDetail.Reset();
                DwMain.InsertRow(0);
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        public void WebSheetLoadEnd()
        {
            DwUtil.RetrieveDDDW(DwMain, "account_id_1", "vc_vcedit_vcdetail_non_cahpaper.pbl", state.SsCoopId);
            DwMain.SaveDataCache();
            DwDetail.SaveDataCache();
        }

        #endregion

        private void JsGetVcdetailForsetNoncash()
        {
            DateTime startDate = DwMain.GetItemDateTime(1, "start_date");
            DateTime endDate = DwMain.GetItemDateTime(1, "end_date");
            String accountId = DwMain.GetItemString(1, "account_id");
            String xmlDetail = "";
            try
            {
                xmlDetail = accService.of_init_vcset_noncash(state.SsWsPass, startDate, endDate, accountId);
                DwDetail.Reset();
                if (xmlDetail != "" && xmlDetail != null)
                {
                    DwUtil.ImportData(xmlDetail, DwDetail, null, FileSaveAsType.Xml);
                }
                else
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่มีข้อมูลประเภทบัญชี : " + accountId + " ช่วงวันที่ : " + DwMain.GetItemString(1, "start_tdate") + " - " + DwMain.GetItemString(1, "end_tdate"));
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }
    }
}
