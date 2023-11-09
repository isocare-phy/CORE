using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommonLibrary;
using CommonLibrary.WsKeeping;
using Sybase.DataWindow;

namespace Saving.Applications.app_assist
{
    public partial class w_sheet_kt_kp_process : PageWebSheet, WebSheet
    {
        String pbl = "kt_50bath.pbl";
        protected String jsKpPorcess;
        private Keeping KpService;
        protected String ClearProcess;
        protected String jsProc_status;

        public void InitJsPostBack()
        {
            jsKpPorcess = WebUtil.JsPostBack(this, "jsKpPorcess");
            ClearProcess = WebUtil.JsPostBack(this, "ClearProcess");
            jsProc_status = WebUtil.JsPostBack(this, "jsProc_status");
        }

        public void WebSheetLoadBegin()
        {
            HdRunProcess.Value = "false";
            KpService = WsCalling.Keeping;

            if (!IsPostBack)
            {
                JsNewClear();
            }
            else
            {
                this.RestoreContextDw(dw_main);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            switch (eventArg)
            {
                case "jsKpPorcess":
                    JsKpPorcess();
                    break;
                case "ClearProcess":
                    JsNewClear();
                    break;
                case "jsProc_status":
                    break;
            }
        }

        public void SaveWebSheet()
        {
        }

        public void WebSheetLoadEnd()
        {
            try
            {
                DwUtil.RetrieveDDDW(dw_main, "emp_type", pbl, null);
                // DwUtil.RetrieveDDDW(dw_main, "instype_code", pbl, null);
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
            ///fix กู้สามัญ


            dw_main.SaveDataCache();

        }

        private void JsNewClear()
        {
            dw_main.Reset();
            dw_main.InsertRow(0);
            dw_main.SetItemDecimal(1, "receive_year", (state.SsWorkDate.Year + 543));
            dw_main.SetItemDecimal(1, "receive_month", state.SsWorkDate.Month);
        }

        private void JsKpPorcess()
        {
            try
            {
                str_keep astr_keep = new str_keep();
                astr_keep.xml_option = dw_main.Describe("DataWindow.Data.XML");

                int result = KpService.RunkpKt50kProcess(state.SsWsPass, astr_keep, state.SsApplication, state.CurrentPage);

                HdRunProcess.Value = "true";
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }
    }
}