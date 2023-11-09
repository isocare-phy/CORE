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
using CoreSavingLibrary.WcfNCommon;
using CoreSavingLibrary.WcfNShrlon;
using Sybase.DataWindow;
using System.Globalization;
using System.Web.Services.Protocols;

namespace Saving.Applications.shrlon
{
    public partial class w_sheet_sl_approve_loanspc : PageWebSheet, WebSheet
    {
        private n_shrlonClient shrlonService;
        private DwThDate thDwMaster;


        protected String jsgenReqDocNo = "";
        #region WebSheet Members

        void WebSheet.InitJsPostBack()
        {


            jsgenReqDocNo = WebUtil.JsPostBack(this, "jsgenReqDocNo");
            //thDwMaster = new DwThDate(dw_master, this);
            //thDwMaster.Add("loanrequest_date", "loanrequest_tdate");
            //thDwMaster.Add("loanrcvfix_date", "loanrcvfix_tdate");
        }

        void WebSheet.WebSheetLoadBegin()
        {
            try
            {
                shrlonService = wcf.NShrlon;
            }
            catch
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ติดต่อ Web Service ไม่ได้");
                return;
            }
            this.ConnectSQLCA();

            if (IsPostBack)
            {
                dw_master.RestoreContext();
            }
            if (dw_master.RowCount < 1)
            {
                this.InitLnReqList();
            }

        }

        void WebSheet.CheckJsPostBack(string eventArg)
        {
            if (eventArg == "jsgenReqDocNo")
            {
                this.GenReqDocNo();
            }

        }

        void WebSheet.SaveWebSheet()
        {
            try
            {
                string as_apvid = state.SsUsername;
                String ls_xml_main = dw_master.Describe("DataWindow.Data.XML");
                Int32 ls_xml = shrlonService.of_saveapv_lnreq(state.SsWsPass, ls_xml_main, as_apvid, state.SsCoopControl, state.SsWorkDate);
                InitLnReqList();

            }
            catch (SoapException ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(WebUtil.SoapMessage(ex));
            }
            catch (Exception ex)
            {
                // LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
            }


        }

        void WebSheet.WebSheetLoadEnd()
        {
            DwUtil.RetrieveDDDW(dw_master, "loantype_code", "sl_approve_loan.pbl", null);
            dw_master.SaveDataCache();
        }

        #endregion

        private void InitLnReqList()
        {
            try
            {
                String reqListXML = shrlonService.of_initlist_lnreqapv(state.SsWsPass, state.SsCoopId, state.SsCoopId);

                dw_master.Reset();
                DwUtil.ImportData(reqListXML, dw_master, null, FileSaveAsType.Xml);
                //   dw_master.SetFilter("loantype_code not in ('23','26') ");
                dw_master.SetFilter("loantype_code >='30' and coop_id ='" + state.SsCoopId + "'");
                dw_master.Filter();
            }

            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่มีข้อมูลรอทำรายการ");
            }
        }

        private void GenReqDocNo()
        {
            int count = dw_master.RowCount;
            for (int i = 0; i < count; i++)
            {
                String lncont_no = "";
                try { lncont_no = dw_master.GetItemString(i + 1, "loancontract_no"); }
                catch { lncont_no = ""; }

                String lncont_status = dw_master.GetItemString(i + 1, "loanrequest_status");
                if (lncont_status == "1")
                {
                    if (lncont_no == "")
                    {
                        String loantype_code = dw_master.GetItemString(i + 1, "loantype_code").Trim();
                        String newReqDocNo = shrlonService.of_gennewcontractno(state.SsWsPass, state.SsCoopControl, loantype_code);

                        dw_master.SetItemString(i + 1, "loancontract_no", newReqDocNo);
                        //  dw_master.SetItemDateTime(i + 1, "approve_date", state.SsWorkDate);
                    }
                }
            }

        }
    }
}
