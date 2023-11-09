using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sybase.DataWindow;
using CoreSavingLibrary.WcfNInvestment;
using DataLibrary;
using System.Web.Services.Protocols;

namespace Saving.Applications.investment
{
    public partial class w_sheet_ln_intfixchg : PageWebSheet,WebSheet
    {
        public String pbl = "int_intfixchg.pbl";
        private n_investmentClient InvService;
        private DwThDate tDw_main;
        protected String postInit;
        protected String postNewClear;
        //========================

        public void InitJsPostBack()
        {
            postNewClear = WebUtil.JsPostBack(this, "postNewClear");
            postInit = WebUtil.JsPostBack(this, "postInit");
            //======================
            DwUtil.RetrieveDDDW(Dw_main, "loantype_code", pbl, null);
            //======================
            tDw_main = new DwThDate(Dw_main, this);
            tDw_main.Add("contbefore_date", "contbefore_tdate");
            tDw_main.Add("intfixchg_date", "intfixchg_tdate");
        }

        public void WebSheetLoadBegin()
        {
            try
            {
                InvService = wcf.NInvestment;
            }
            catch { LtServerMessage.Text = WebUtil.CompleteMessage("ติดต่อ Service ไม่ได้"); }

            if (!IsPostBack)
            {
                JspostNewClear();
            }
            else
            {
                this.RestoreContextDw(Dw_main, tDw_main);
                this.RestoreContextDw(Dw_detail);
            }

        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postNewClear")
            {
                JspostNewClear();
            }
            else if (eventArg == "postInit")
            {
                JspostInit();
            }
        }

        public void SaveWebSheet()
        {
            try
            {
                String as_xmldata = Dw_detail.Describe("Datawindow.Data.Xml");
                int result = InvService.of_saveintfixchg(state.SsWsPass, ref as_xmldata);
                if (result == 1)
                {
                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลเสร็จเรียบร้อยแล้ว");
                    JspostNewClear();
                }
                
            }
            catch (SoapException ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("เกิดข้อผิดพลาด " + WebUtil.SoapMessage(ex));
            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message); }
        }

        public void WebSheetLoadEnd()
        {
            Dw_main.SaveDataCache();
            Dw_detail.SaveDataCache();
        }

        private void JspostInit()
        {
            try
            {
                String as_xmloption = Dw_main.Describe("Datawindow.Data.Xml");
                String as_xmldata = Dw_detail.Describe("Datawindow.Data.Xml");
                int result = InvService.of_initintfixchg(state.SsWsPass, as_xmloption, ref as_xmldata);
                if (result == 1)
                {
                    Dw_detail.Reset();
                    DwUtil.ImportData(as_xmldata, Dw_detail, null, FileSaveAsType.Xml);
                }
            }
            catch (SoapException ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("เกิดข้อผิดพลาด " + WebUtil.SoapMessage(ex));
            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message); }
        }

        private void JspostNewClear()
        {
            Dw_main.Reset();
            Dw_main.InsertRow(0);
            Dw_main.SetItemDate(1, "contbefore_date", state.SsWorkDate);
            Dw_main.SetItemDate(1, "intfixchg_date", state.SsWorkDate);
            tDw_main.Eng2ThaiAllRow();
            Dw_detail.Reset();
        }
    }
}