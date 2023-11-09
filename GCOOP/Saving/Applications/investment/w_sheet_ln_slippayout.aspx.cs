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
using Sybase.DataWindow;
using DataLibrary;
using CoreSavingLibrary.WcfInvestment;

namespace Saving.Applications.investment
{
    public partial class w_sheet_ln_slippayout : PageWebSheet, WebSheet
    {
        #region Variable
        protected String jsPostMemberNo;
        protected String jsGetGroupMoneyType;
        protected String jsSetBankBranch;
        protected String jsPostEtcDel;
        protected String jsPostEtcChange;
        protected String jsPostContno;
        protected String jsPostFlag;
        protected String jsPostPayAmt;
        protected String jsChangeSlipdate;
        protected String jsChangeRcvFlag;
        protected String jsChangePayoutnet;
        protected String jsPostInsertPayOutExp;
        private DwThDate tOperateDate;
        private InvestmentClient InvestmentService;
        String pbl = "loan_slipayout.pbl";
        #endregion

        #region Websheet Members
        public void InitJsPostBack()
        {
            tOperateDate = new DwThDate(DwPayOut, this);
            tOperateDate.Add("slip_date", "slip_tdate");
            jsPostMemberNo = WebUtil.JsPostBack(this, "jsPostMemberNo");
            jsGetGroupMoneyType = WebUtil.JsPostBack(this, "jsGetGroupMoneyType");
            jsSetBankBranch = WebUtil.JsPostBack(this, "jsSetBankBranch");
            jsPostEtcDel = WebUtil.JsPostBack(this, "jsPostEtcDel");
            jsPostEtcChange = WebUtil.JsPostBack(this, "jsPostEtcChange");
            jsPostContno = WebUtil.JsPostBack(this, "jsPostContno");
            jsPostFlag = WebUtil.JsPostBack(this, "jsPostFlag");
            jsPostPayAmt = WebUtil.JsPostBack(this, "jsPostPayAmt");
            jsChangeSlipdate = WebUtil.JsPostBack(this, "jsChangeSlipdate");
            jsChangeRcvFlag = WebUtil.JsPostBack(this, "jsChangeRcvFlag");
            jsChangePayoutnet = WebUtil.JsPostBack(this, "jsChangePayoutnet");
            jsPostInsertPayOutExp = WebUtil.JsPostBack(this, "jsPostInsertPayOutExp");
        }

        public void WebSheetLoadBegin()
        {
            InvestmentService = wcf.Investment;
            if (!IsPostBack)
            {
                DwMain.InsertRow(0);
                DwPayOut.InsertRow(0);
            }
            else
            {
                this.RestoreContextDw(DwMain);
                this.RestoreContextDw(DwPayOut);
                this.RestoreContextDw(DwPayOutExp);
                this.RestoreContextDw(DwPayOutLon);
                this.RestoreContextDw(DwPayOutEtc);  
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            switch (eventArg)
            {
                case "jsPostMemberNo":
                    InitListMemCont();
                    break;
                case "jsGetGroupMoneyType":
                    MoneyTypeChange();
                    break;
                case "jsSetBankBranch":
                    SetBankBranch();
                    break;
                case "jsPostEtcDel":
                    EtcDelete();
                    break;
                case "jsPostEtcChange":
                    EtcChange();
                    break;
                case "jsPostContno":
                    InitData(Hdcontcoopid.Value, Hdcontno.Value);
                    break;
                case "jsPostFlag":
                    ChangeLoanFlag();
                    break;
                case "jsPostPayAmt":
                    JsPostPayAmt();
                    break;
                case "jsChangeSlipdate":
                    ChangeSlipdate();
                    break;
                case "jsChangeRcvFlag":
                    break;
                case "jsChangePayoutnet":
                    PayoutnetChange();
                    break;
                case "jsPostInsertPayOutExp":
                    DwPayOutExp.InsertRow(0);
                    DwPayOutExp.SetItemDecimal(DwPayOutExp.RowCount, "expense_amt", DwPayOut.GetItemDecimal(1, "payoutnet_amt"));
                    break;
            }
        }

        public void SaveWebSheet()
        {
            if (DwPayOut.GetItemDecimal(1, "payoutnet_amt") >= 0)
            {
                str_lcslippayout slippayout = new str_lcslippayout();

                try
                {
                    slippayout.xml_sliphead = DwPayOut.Describe("Datawindow.Data.XML");
                    slippayout.xml_slipcutlon = DwPayOutLon.Describe("Datawindow.Data.XML");
                    slippayout.xml_slipcutetc = DwPayOutEtc.Describe("Datawindow.Data.XML");
                    slippayout.xml_expense = DwPayOutExp.Describe("Datawindow.Data.XML");
                    slippayout.entry_id = state.SsUsername;
                    slippayout.entry_bycoopid = state.SsCoopId;
                }
                catch { }

                try
                {
                    short result = InvestmentService.of_saveslip_lnrcv(state.SsWsPass, ref slippayout);
                    if (result == 1)
                    {
                        LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลสำเร็จ");

                        string ls_receiptno = slippayout.payinslip_no;

                        if ( !(ls_receiptno == "" ||ls_receiptno == null)) {
                            Printing.LnCoopSlip(this, state.SsCoopControl, ls_receiptno, xmlconfig.LnReceivePrintMode);
                        }
                    }
                }
                catch(Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                }
            }
            else
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ยอดจ่ายสุทธิติดลบ กรุณาตรวจสอบ");
            }

         
        }

        public void WebSheetLoadEnd()
        {
            try
            {
                DwUtil.RetrieveDDDW(DwPayOutExp, "moneytype_code", pbl, null);
            }
            catch { }
            tOperateDate.Eng2ThaiAllRow();
            DwMain.SaveDataCache();
            DwPayOut.SaveDataCache();
            DwPayOutExp.SaveDataCache();
            DwPayOutLon.SaveDataCache();
            DwPayOutEtc.SaveDataCache();
        }
        #endregion

        #region Function
        private void InitListMemCont()
        {
            try
            {
                str_lclnrcvlist lclnrcvlist = new str_lclnrcvlist();
                String MemberNo = DwMain.GetItemString(1, "member_no");
                int MemberNo_Length = MemberNo.Length;
                switch (MemberNo_Length)
                {
                    case 1:
                        MemberNo = "00000" + MemberNo;
                        break;
                    case 2:
                        MemberNo = "0000" + MemberNo;
                        break;
                    case 3:
                        MemberNo = "000" + MemberNo;
                        break;
                    case 4:
                        MemberNo = "00" + MemberNo;
                        break;
                    case 5:
                        MemberNo = "0" + MemberNo;
                        break;
                }
                DwMain.SetItemString(1, "member_no", MemberNo);

                String sql = "select member_status from lcmembmaster where member_no = '" + MemberNo + "'";
                Sta ta = new Sta(new DwTrans().ConnectionString);
                Sdt dt = ta.Query(sql);

                if (dt.Next())
                {
                    if (dt.Rows[0][0].ToString() != "-1" && dt.Rows[0][0].ToString() != "-9")
                    {
                        try
                        {
                            lclnrcvlist.memcoop_id = state.SsCoopId;
                            lclnrcvlist.member_no = MemberNo;
                        }
                        catch { }
                        try
                        {
                            short result = InvestmentService.of_getmemb_lnrcv(state.SsWsPass, ref lclnrcvlist);
                            if (result == 1)
                            {
                                InitData(lclnrcvlist.concoop_id, lclnrcvlist.loancontract_no);
                            }
                            else if (result > 1)
                            {
                                HdOpenIFrame.Value = "True";
                                Hdmemno.Value = lclnrcvlist.member_no;
                            }
                        }
                        catch (Exception ex)
                        {
                            LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                        }
                    }
                    else
                    {
                        DwMain.Reset();
                        DwMain.InsertRow(0);
                        DwPayOut.Reset();
                        DwPayOut.InsertRow(0);
                        LtServerMessage.Text = WebUtil.ErrorMessage("สมาชิกเลขที่ " + MemberNo + " ได้ลาออกไปแล้ว");
                    }
                }
                else
                {
                    DwMain.Reset();
                    DwMain.InsertRow(0);
                    DwPayOut.Reset();
                    DwPayOut.InsertRow(0);
                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่มีเลขสมาชิก " + MemberNo);
                }
            }
            catch
            {

            }
        }

        private void InitData(string contcoop, string lncontno)
        {
            str_lcslippayout slippayout = new str_lcslippayout();

            try
            {
                slippayout.initfrom_type = "LWD";
                slippayout.concoop_id = contcoop;
                slippayout.loancontract_no = lncontno;
                slippayout.slip_date = state.SsWorkDate;
                slippayout.operate_date = state.SsWorkDate;
            }
            catch { }

            try
            {
                short result = InvestmentService.of_initlnrcv(state.SsWsPass, ref slippayout);
                if (result == 1)
                {
                    DwMain.Reset();
                    DwPayOut.Reset();
                    DwPayOutExp.Reset();
                    DwPayOutLon.Reset();
                    DwPayOutEtc.Reset();
                    if (slippayout.xml_slipmemdet != "" && slippayout.xml_slipmemdet != null)
                    {
                        DwMain.ImportString(slippayout.xml_slipmemdet, Sybase.DataWindow.FileSaveAsType.Xml);

                        if (slippayout.xml_sliphead != "" && slippayout.xml_sliphead != null)
                        {
                            DwPayOut.ImportString(slippayout.xml_sliphead, Sybase.DataWindow.FileSaveAsType.Xml);
                        }
                        if (slippayout.xml_slipcutlon != "" && slippayout.xml_slipcutlon != null)
                        {
                            DwPayOutLon.ImportString(slippayout.xml_slipcutlon, Sybase.DataWindow.FileSaveAsType.Xml);
                        }
                        if (slippayout.xml_slipcutetc != "" && slippayout.xml_slipcutetc != null)
                        {
                            DwPayOutEtc.ImportString(slippayout.xml_slipcutetc, Sybase.DataWindow.FileSaveAsType.Xml);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        private void MoneyTypeChange()
        {
            try
            {
                DwPayOutExp.SetItemNull(Convert.ToInt32(HdRow_Type.Value), "expense_accid");
                string money_type = DwPayOutExp.GetItemString(Convert.ToInt32(HdRow_Type.Value), "moneytype_code");
                DataWindowChild dc = DwPayOutExp.GetChild("moneytype_code");
                int rGroup = dc.FindRow("moneytype_code='" + money_type + "'", 1, dc.RowCount);
                string moneytype_group = DwUtil.GetString(dc, rGroup, "moneytype_group", "");
                DwPayOutExp.SetItemString(Convert.ToInt32(HdRow_Type.Value), "moneytype_group", moneytype_group);
                HdRow_Type.Value = "";
            }
            catch
            {
                HdRow_Type.Value = "";
            }
        }

        private void SetBankBranch()
        {
            try
            {
                int sheetRow = Convert.ToInt32(HdSheetRow.Value);
                DwPayOutExp.SetItemString(sheetRow, "expense_bank", HdBank_id.Value);
                DwPayOutExp.SetItemString(sheetRow, "expense_branch", HdBranch_id.Value);

                DwPayOutExp.SetItemString(sheetRow, "bank_desc", HdBank_desc.Value.Trim() + " " + HdBranch_desc.Value.Trim());
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                return;
            }
            HdSheetRow.Value = "";
            HdBank_id.Value = "";
            HdBank_desc.Value = "";
            HdBranch_id.Value = "";
            HdBranch_desc.Value = "";
        }

        private void EtcDelete()
        {
            int row = Convert.ToInt32(HdEtcRow.Value);
            Decimal item_payamt = 0, payoutnet_amt = 0, payoutclr_amt = 0, payout_amt = 0;
            try
            {
                item_payamt = DwPayOutEtc.GetItemDecimal(row, "item_payamt");
                payout_amt = DwPayOut.GetItemDecimal(1, "payout_amt");
                payoutclr_amt = DwPayOut.GetItemDecimal(1, "payoutclr_amt");
                payoutnet_amt = DwPayOut.GetItemDecimal(1, "payoutnet_amt");

                payoutclr_amt = payoutclr_amt - item_payamt;
                payoutnet_amt = payout_amt - payoutclr_amt;

                DwPayOut.SetItemDecimal(1, "payoutnet_amt", payoutnet_amt);
                DwPayOut.SetItemDecimal(1, "payoutclr_amt", payoutclr_amt);
                DwPayOutEtc.DeleteRow(row);
            }
            catch
            {
                DwPayOutEtc.DeleteRow(row);
            }
        }

        private void EtcChange()
        {
            Decimal item_payamt = 0, payoutnet_amt = 0, payoutclr_amt = 0, payout_amt = 0;
            int row = Convert.ToInt32(HdEtcRow.Value);
            try
            {
                item_payamt = DwPayOutEtc.GetItemDecimal(row, "item_payamt");
                payout_amt = DwPayOut.GetItemDecimal(1, "payout_amt");
                payoutclr_amt = DwPayOut.GetItemDecimal(1, "payoutclr_amt");
                payoutnet_amt = DwPayOut.GetItemDecimal(1, "payoutnet_amt");

                payoutclr_amt = payoutclr_amt + item_payamt;
                payoutnet_amt = payout_amt - payoutclr_amt;

                DwPayOut.SetItemDecimal(1, "payoutnet_amt", payoutnet_amt);
                DwPayOut.SetItemDecimal(1, "payoutclr_amt", payoutclr_amt);
            }
            catch
            {

            }
        }

        private void ChangeLoanFlag()
        {
            decimal flag = 0;
            Decimal payoutclr_amt = 0, item_balance = 0, payout_amt = 0, payoutnet_amt = 0, bfshrcont_balamt = 0, item_payamt = 0;//ยอกหักชำระ,คงเหลือ,หุ้นที่ถอน,รับสุทธิ
            Decimal interest_period = 0, bfintarr_amt = 0, interest_payamt = 0;
            int r = Convert.ToInt32(HdRow.Value);
            try
            {
                payout_amt = DwPayOut.GetItemDecimal(1, "payout_amt");//หุ้นที่ถอน
                payoutclr_amt = DwPayOut.GetItemDecimal(1, "payoutclr_amt");//ยอกหักชำระ
                payoutnet_amt = DwPayOut.GetItemDecimal(1, "payoutnet_amt");//รับสุทธิ
                flag = DwPayOutLon.GetItemDecimal(r, "operate_flag");
                item_balance = DwPayOutLon.GetItemDecimal(r, "item_balance");//คงเหลือของแถวนั้นๆ
                bfshrcont_balamt = DwPayOutLon.GetItemDecimal(r, "bfshrcont_balamt");
                item_payamt = DwPayOutLon.GetItemDecimal(r, "item_payamt");
                interest_period = DwPayOutLon.GetItemDecimal(r, "interest_period");
                bfintarr_amt = DwPayOutLon.GetItemDecimal(r, "bfintarr_amt");
                interest_payamt = interest_period + bfintarr_amt;

                if (flag == 1)
                {
                    payoutclr_amt = payoutclr_amt + item_balance + interest_payamt;
                    DwPayOutLon.SetItemDecimal(r, "item_balance", 0);
                    DwPayOutLon.SetItemDecimal(r, "principal_payamt", item_balance);
                    DwPayOutLon.SetItemDecimal(r, "interest_payamt", interest_payamt);
                    DwPayOutLon.SetItemDecimal(r, "item_payamt", item_balance + interest_payamt);
                }
                else
                {
                    payoutclr_amt = payoutclr_amt - item_payamt;
                    DwPayOutLon.SetItemDecimal(r, "item_balance", bfshrcont_balamt);
                    DwPayOutLon.SetItemDecimal(r, "item_payamt", 0);
                    DwPayOutLon.SetItemDecimal(r, "principal_payamt", 0);
                    DwPayOutLon.SetItemDecimal(r, "interest_payamt", 0);
                }
                payoutnet_amt = payout_amt - payoutclr_amt;
                DwPayOut.SetItemDecimal(1, "payoutclr_amt", payoutclr_amt);//ยอกหักชำระ
                DwPayOut.SetItemDecimal(1, "payoutnet_amt", payoutnet_amt);//รับสุทธิ

            }
            catch (Exception ex)
            { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
        }

        private void JsPostPayAmt()
        {
            Decimal payoutclr_amt = 0, item_balance = 0, payout_amt = 0, payoutnet_amt = 0;//ยอกหักชำระ,คงเหลือ,หุ้นที่ถอน,รับสุทธิ
            Decimal principal_payamt = 0, interest_payamt = 0, item_payamt = 0, bfshrcont_balamt = 0, item_payamt_;
            Decimal interest_period = 0, bfintarr_amt = 0, interest_payamt_all = 0;
            int r = Convert.ToInt32(HdRow.Value);
            try
            {
                payout_amt = DwPayOut.GetItemDecimal(1, "payout_amt");//หุ้นที่ถอน
                payoutclr_amt = DwPayOut.GetItemDecimal(1, "payoutclr_amt");//ยอกหักชำระ
                payoutnet_amt = DwPayOut.GetItemDecimal(1, "payoutnet_amt");//รับสุทธิ

                principal_payamt = DwPayOutLon.GetItemDecimal(r, "principal_payamt");// ต้นเงินแถวนั้นๆ
                interest_payamt = DwPayOutLon.GetItemDecimal(r, "interest_payamt"); //ดอกเบี้ยแถวนั้นๆ
                item_payamt = DwPayOutLon.GetItemDecimal(r, "item_payamt"); //ยอดชำระแถวนั้นๆ
                item_payamt_ = item_payamt;
                item_balance = DwPayOutLon.GetItemDecimal(r, "item_balance");//คงเหลือของแถวนั้นๆ
                bfshrcont_balamt = DwPayOutLon.GetItemDecimal(r, "bfshrcont_balamt");

                interest_period = DwPayOutLon.GetItemDecimal(r, "interest_period");
                bfintarr_amt = DwPayOutLon.GetItemDecimal(r, "bfintarr_amt");
                interest_payamt_all = interest_period + bfintarr_amt;
                if (interest_payamt > interest_payamt_all)
                {
                    DwPayOutLon.SetItemDecimal(r, "interest_payamt", interest_payamt_all);
                    interest_payamt = interest_payamt_all;
                }

                payoutclr_amt = (payoutclr_amt - item_payamt) + principal_payamt + interest_payamt;
                item_payamt = principal_payamt + interest_payamt;

                item_balance = bfshrcont_balamt - principal_payamt;
                if (item_balance >= 0)
                {
                    DwPayOutLon.SetItemDecimal(r, "item_balance", item_balance);
                    DwPayOutLon.SetItemDecimal(r, "item_payamt", item_payamt);

                    payoutnet_amt = payout_amt - payoutclr_amt;
                    DwPayOut.SetItemDecimal(1, "payoutclr_amt", payoutclr_amt);//ยอกหักชำระ
                    DwPayOut.SetItemDecimal(1, "payoutnet_amt", payoutnet_amt);//รับสุทธิ
                }
                else
                {
                    DwPayOutLon.SetItemDecimal(r, "principal_payamt", principal_payamt - (item_payamt - item_payamt_));
                }
            }
            catch (Exception ex)
            { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
        }

        private void ChangeSlipdate()
        {
            String tDate2 = DwPayOut.GetItemString(1, "slip_tdate");
            DateTime dt2 = DateTime.ParseExact(tDate2, "ddMMyyyy", WebUtil.TH);
            DwPayOut.SetItemDateTime(1, "slip_date", dt2);
            
            str_lcslippayout lcslippayout = new str_lcslippayout();
            try
            {
                lcslippayout.xml_sliphead = DwPayOut.Describe("Datawindow.Data.XML");
                lcslippayout.xml_slipcutlon = DwPayOutLon.Describe("Datawindow.Data.XML");
                lcslippayout.xml_slipcutetc = DwPayOutEtc.Describe("Datawindow.Data.XML");
            }
            catch { }

            try
            {
                short result = InvestmentService.of_initlnrcv_recalint(state.SsWsPass, ref lcslippayout);
                if (result == 1)
                {
                    DwPayOut.Reset();
                    DwPayOutLon.Reset();
                    DwPayOutEtc.Reset();

                    if (lcslippayout.xml_sliphead != "" && lcslippayout.xml_sliphead != null)
                    {
                        DwPayOut.ImportString(lcslippayout.xml_sliphead, Sybase.DataWindow.FileSaveAsType.Xml);
                    }
                    if (lcslippayout.xml_slipcutlon != "" && lcslippayout.xml_slipcutlon != null)
                    {
                        DwPayOutLon.ImportString(lcslippayout.xml_slipcutlon, Sybase.DataWindow.FileSaveAsType.Xml);
                    }
                    if (lcslippayout.xml_slipcutetc != "" && lcslippayout.xml_slipcutetc != null)
                    {
                        DwPayOutEtc.ImportString(lcslippayout.xml_slipcutetc, Sybase.DataWindow.FileSaveAsType.Xml);
                    }
                }
            }
            catch { }
        }

        private void PayoutnetChange()
        {
            Decimal payoutnet_amt = DwPayOut.GetItemDecimal(1, "payoutnet_amt");
            Decimal payout_amt = DwPayOut.GetItemDecimal(1, "payout_amt");
            Decimal payoutclr_amt = DwPayOut.GetItemDecimal(1, "payoutclr_amt");
            Decimal withdrawable_amt = DwMain.GetItemDecimal(1, "withdrawable_amt");
            if (withdrawable_amt >= payout_amt)
            {
                DwPayOut.SetItemDecimal(1, "payoutnet_amt", payout_amt - payoutclr_amt);
            }
            else
            {
                DwPayOut.SetItemDecimal(1, "payout_amt", withdrawable_amt);
                DwPayOut.SetItemDecimal(1, "payoutnet_amt", withdrawable_amt - payoutclr_amt);
            }
        }
        #endregion
    }

}