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
using Sybase.DataWindow;
using DataLibrary;

namespace Saving.Applications.investment
{
    public partial class w_sheet_ln_req_contadj : PageWebSheet, WebSheet
    {
        String pbl = "loan.pbl";
        private DwThDate tDwSpc;
        private DwThDate tDwDetail;
        protected String jsPostMembNo;
        protected String jsPostLoancontractNoChange;
        protected String jsPostBack;
        protected String jsPostColl;
        protected String jsPostCollAmt;
        public void InitJsPostBack()
        {
            tDwDetail = new DwThDate(DwDetail, this);
            tDwDetail.Add("contadjust_date", "contadjust_tdate");

            tDwSpc = new DwThDate(DwSpc, this);
            tDwSpc.Add("effective_date", "effective_tdate");
            tDwSpc.Add("expire_date", "expire_tdate");

            jsPostMembNo = WebUtil.JsPostBack(this, "jsPostMembNo");
            jsPostLoancontractNoChange = WebUtil.JsPostBack(this, "jsPostLoancontractNoChange");
            jsPostBack = WebUtil.JsPostBack(this, "jsPostBack");
            jsPostColl = WebUtil.JsPostBack(this, "jsPostColl");
            jsPostCollAmt = WebUtil.JsPostBack(this, "jsPostCollAmt");
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                //DwMain.InsertRow(0);
                DwDetail.InsertRow(0);
                DwInt.InsertRow(0);
                DwPreriod.InsertRow(0);
            }
            else
            {
                //this.RestoreContextDw(DwMain);
                this.RestoreContextDw(DwDetail, tDwDetail);
                this.RestoreContextDw(DwInt);
                this.RestoreContextDw(DwPreriod);
                this.RestoreContextDw(DwSpc, tDwSpc);
                this.RestoreContextDw(DwColl);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            switch (eventArg)
            {
                case "jsPostMembNo":
                    //JsPostMembNo();
                    break;
                case "jsPostLoancontractNoChange":
                    JsPostLoancontractNoChange();
                    break;
                case "jsPostCollAmt":
                    JsPostCollAmt();
                    break;
            }
        }

        public void SaveWebSheet()
        {
            try
            {
                InvestmentClient InvService = wcf.Investment;
                str_lccontaj astr_lccontaj = new str_lccontaj();
                astr_lccontaj.xml_adjmast = DwDetail.Describe("DataWindow.Data.XML");
                astr_lccontaj.xml_adjpayment = DwPreriod.Describe("DataWindow.Data.XML");
                astr_lccontaj.xml_adjint = DwInt.Describe("DataWindow.Data.XML");
                astr_lccontaj.xml_adjintspc = DwSpc.Describe("DataWindow.Data.XML");
                astr_lccontaj.xml_adjcoll = DwColl.Describe("DataWindow.Data.XML");
                astr_lccontaj.entry_id = state.SsUsername;
                astr_lccontaj.entry_bycoopid = state.SsCoopId;
                astr_lccontaj.xml_oldadjcoll = astr_lccontaj.xml_adjintspc;
                astr_lccontaj.xml_oldadjintspc = astr_lccontaj.xml_adjintspc;
                short result = InvService.of_save_reqcontadjust(state.SsWsPass, ref astr_lccontaj);
                if (result == 1)
                {
                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
                }
                else
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("บันทึกไม่สำเร็จ");
                }
            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage("บันทึกไม่สำเร็จเนื่องจาก :" + ex); }
        }

        public void WebSheetLoadEnd()
        {
            try
            {
                DwUtil.RetrieveDDDW(DwInt, "int_continttabcode", pbl, null);
            }
            catch { }
            try
            {
                DwUtil.RetrieveDDDW(DwSpc, "int_continttabcode", pbl, null);
            }
            catch { }
            try
            {
                DwUtil.RetrieveDDDW(DwColl, "loancolltype_code", pbl, null);
            }
            catch { }
            tDwSpc.Eng2ThaiAllRow();
            tDwDetail.Eng2ThaiAllRow();
            //DwMain.SaveDataCache();
            DwDetail.SaveDataCache();
            DwInt.SaveDataCache();
            DwPreriod.SaveDataCache();
            DwColl.SaveDataCache();
            DwSpc.SaveDataCache();
        }
        public void JsPostCollAmt()
        {
            try
            {
                decimal percent = 0;
                Decimal sumall = 0;
                int r = Convert.ToInt32(HdCollRow.Value);
                Decimal Amt = Convert.ToDecimal(HdCollAmt.Value);
                String Desc = DwColl.GetItemString(r, "description");
                Desc = Desc + Amt.ToString("#,###") + " บาท";
                DwColl.SetItemString(r, "description", Desc);
                DwColl.SetItemDecimal(r, "coll_amt", Amt);
                for (int i = 1; i <= DwColl.RowCount; i++)
                {
                    sumall += DwColl.GetItemDecimal(i, "coll_amt");
                }
                for (int i = 1; i <= DwColl.RowCount; i++)
                {
                    Amt = DwColl.GetItemDecimal(i, "coll_amt");
                    percent = Amt / sumall;
                    DwColl.SetItemDecimal(i, "coll_percent", percent);
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }

        }
        //private void JsPostMembNo()
        //{
        //    string member_no = "";
        //    try
        //    {
        //        member_no = DwMain.GetItemString(1, "member_no");
        //        DwUtil.RetrieveDataWindow(DwMain, pbl, null, member_no);
        //        member_no = DwMain.GetItemString(1, "member_no");
        //    }
        //    catch
        //    {
        //        LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบข้อมูลสมาชิก");
        //        DwMain.InsertRow(0);
        //    }
        //}
        private void JsPostLoancontractNoChange()
        {
            string loancontract_no = "";
            try
            {
                InvestmentClient InvService = wcf.Investment;
                loancontract_no = DwDetail.GetItemString(1, "loancontract_no");

                str_lccontaj astr_lccontaj = new str_lccontaj();

                astr_lccontaj.contcoop_id = state.SsCoopId;
                astr_lccontaj.loancontract_no = loancontract_no;
                astr_lccontaj.contaj_date = state.SsWorkDate;

                short result = InvService.of_init_reqcontadjust(state.SsWsPass, ref astr_lccontaj);
                if (result == 1)
                {
                    if (astr_lccontaj.xml_adjmast != null && astr_lccontaj.xml_adjmast != "")
                    {
                        DwDetail.Reset();
                        DwDetail.ImportString(astr_lccontaj.xml_adjmast, FileSaveAsType.Xml);
                    }
                    if (astr_lccontaj.xml_adjpayment != null && astr_lccontaj.xml_adjpayment != "")
                    {
                        DwPreriod.Reset();
                        DwPreriod.ImportString(astr_lccontaj.xml_adjpayment, FileSaveAsType.Xml);
                    }
                    if (astr_lccontaj.xml_adjint != null && astr_lccontaj.xml_adjint != "")
                    {
                        DwInt.Reset();
                        DwInt.ImportString(astr_lccontaj.xml_adjint, FileSaveAsType.Xml);
                    }
                    if (astr_lccontaj.xml_adjintspc != null && astr_lccontaj.xml_adjintspc != "")
                    {
                        DwSpc.Reset();
                        DwSpc.ImportString(astr_lccontaj.xml_adjintspc, FileSaveAsType.Xml);
                    }
                    if (astr_lccontaj.xml_adjcoll != null && astr_lccontaj.xml_adjcoll != "")
                    {
                        DwColl.Reset();
                        DwColl.ImportString(astr_lccontaj.xml_adjcoll, FileSaveAsType.Xml);
                    }

                }
                else
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบข้อมูลสัญญา");
                    DwDetail.Reset();
                    DwInt.Reset();
                    DwColl.Reset();
                    DwPreriod.Reset();
                    DwSpc.Reset();
                    DwDetail.InsertRow(0);
                    DwInt.InsertRow(0);
                    DwPreriod.InsertRow(0);
                }
            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message); }
        }
    }
}