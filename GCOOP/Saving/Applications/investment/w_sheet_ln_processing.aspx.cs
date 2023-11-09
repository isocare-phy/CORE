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
using CoreSavingLibrary.WcfInvestment;
using CoreSavingLibrary.WcfShrlon;
using Sybase.DataWindow;
using DataLibrary;

namespace Saving.Applications.investment
{
    public partial class w_sheet_ln_processing : PageWebSheet,WebSheet
    {
        String pbl = "loan.pbl";
        private DwThDate tDwMain;
        private InvestmentClient Invservice;
        protected String postRefresh;
        protected String postNewClear;

        //==================================
        public void InitJsPostBack()
        {
            tDwMain = new DwThDate(DwMain, this);
            tDwMain.Add("notice_date", "notice_tdate");

            //================================================
            postRefresh = WebUtil.JsPostBack(this, "postRefresh");
            postNewClear = WebUtil.JsPostBack(this, "postNewClear");
        }

        public void WebSheetLoadBegin()
        {
            Hd_process.Value = "false";
            if (!IsPostBack)
            {
                JspostNewClear();
            }
            else
            {
                this.RestoreContextDw(DwMain);
            }
            
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postRefresh")
            { 
                // Refresh หน้าจอ
            }
            else if (eventArg == "postNewClear")
            {
                JspostNewClear();
            }
        }

        public void SaveWebSheet()
        {
            try 

            {
                //service เรียก progress bar
                Invservice = wcf.Investment;
                str_lcprocloan astr_procloan = new str_lcprocloan();
                astr_procloan.xml_option = DwMain.Describe("DataWindow.Data.XML");
                Invservice.RunLnProcNoticemthrecv(state.SsWsPass, astr_procloan, state.SsApplication, state.CurrentPage);
                Hd_process.Value = "true";

                //Invservice = wcf.Investment;
                //str_lcprocloan astr_procloan = new str_lcprocloan();
                //astr_procloan.xml_option = DwMain.Describe("DataWindow.Data.XML");
                //short result =  LoanvService.of_saveproc_noticemthrecv(state.SsWsPass,astr_procloan);
                //if(result == 1)
                //{
                //    LtServerMessage.Text =WebUtil.CompleteMessage("ประมวลผลเสร็จเรียบร้อยแล้ว");
                //    JspostNewClear();
                //}
            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message); }
        }

        public void WebSheetLoadEnd()
        {
            DwMain.SaveDataCache();
        }

        //================================
        private void JspostNewClear()
        {
            DwMain.Reset();
            DwMain.InsertRow(0);
            DwMain.SetItemDecimal(1, "datarange_type", 1);
            DwMain.SetItemDateTime(1, "notice_date", state.SsWorkDate);
            tDwMain.Eng2ThaiAllRow();

        }

       
    }
}