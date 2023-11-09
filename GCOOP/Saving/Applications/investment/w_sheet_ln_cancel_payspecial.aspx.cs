using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary.WcfNCommon;
using DataLibrary;
using Sybase.DataWindow;
using CoreSavingLibrary.WcfNInvestment;

namespace Saving.Applications.loan
{
    public partial class w_sheet_ln_cancel_payspecial : PageWebSheet,WebSheet
    {

        protected String jsPostMemberNo;
        public void InitJsPostBack()
        {
            jsPostMemberNo = WebUtil.JsPostBack(this, "jsPostMemberNo");
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                DwMain.InsertRow(0);
            }
            else
            {
                this.RestoreContextDw(DwMain);
                this.RestoreContextDw(DwDetail);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "jsPostMemberNo")
            {
                JsPostMemberNo();
            }
        }

        public void SaveWebSheet()
        {
            try
            {
                InvestmentClient LoanService = wcf.Investment;
                str_lcslipcancel astr_lcslipcancel = new str_lcslipcancel();
                astr_lcslipcancel.xml_sliplist = DwDetail.Describe("DataWindow.Data.XML");
                astr_lcslipcancel.cancel_id = state.SsUsername;
                astr_lcslipcancel.cancel_date = state.SsWorkDate;
                short result = LoanService.of_saveccl_payin(state.SsWsPass, astr_lcslipcancel);
                if (result == 1)
                {
                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
                }
                else
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("บันทึกไม่สำเร็จ");
                }
            }
            catch { }
        }

        public void WebSheetLoadEnd()
        {
            DwMain.SaveDataCache();
            DwDetail.SaveDataCache();
        }
        private void JsPostMemberNo()
        {
            string member_no = "";
            try
            {
                member_no = DwMain.GetItemString(1, "member_no");
                InvestmentClient LoanService = wcf.Investment;
                str_lcslipcancel astr_lcslipcancel = new str_lcslipcancel();
                astr_lcslipcancel.memcoop_id = state.SsCoopId;
                astr_lcslipcancel.member_no = member_no;
                astr_lcslipcancel.cancel_date = state.SsWorkDate;
                short result = LoanService.of_initccl_payinloan(state.SsWsPass, ref astr_lcslipcancel);
                if (result == 1)
                {
                    if (astr_lcslipcancel.xml_memdet != null && astr_lcslipcancel.xml_memdet != "")
                    {
                        DwMain.Reset();
                        DwMain.ImportString(astr_lcslipcancel.xml_memdet, FileSaveAsType.Xml);
                    }
                    if (astr_lcslipcancel.xml_sliplist != null && astr_lcslipcancel.xml_sliplist != "")
                    {
                        DwDetail.Reset();
                        DwDetail.ImportString(astr_lcslipcancel.xml_sliplist, FileSaveAsType.Xml);
                    }
                }
            }
            catch { }
        }
    }
}