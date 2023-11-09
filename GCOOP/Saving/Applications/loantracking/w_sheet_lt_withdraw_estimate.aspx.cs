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
using CoreSavingLibrary.WcfShrlon;
using Sybase.DataWindow;


namespace Saving.Applications.loantracking
{
    public partial class w_sheet_lt_withdraw_estimate : PageWebSheet, WebSheet
    {

        private ShrlonClient srvShrlon;
        private DwThDate tDwMain;

        protected String saveWithdraw;
        protected String initDataWindow;
        protected String loanCalInt;
        protected String calculateAmt;
        protected String searchMemberNo;
        protected String setTrnColl;

        public void InitJsPostBack()
        {
            saveWithdraw = WebUtil.JsPostBack(this, "saveWithdraw");
            //initDataWindow = WebUtil.JsPostBack(this, "initDataWindow");
            loanCalInt = WebUtil.JsPostBack(this, "loanCalInt");
            calculateAmt = WebUtil.JsPostBack(this, "calculateAmt");
            searchMemberNo = WebUtil.JsPostBack(this, "searchMemberNo");

            setTrnColl = WebUtil.JsPostBack(this, "setTrnColl");

            tDwMain = new DwThDate(DwMain);
            tDwMain.Add("operate_date", "operate_tdate");
        }

        public void WebSheetLoadBegin()
        {
            srvShrlon = wcf.Shrlon;

            if (IsPostBack)
            {
                this.RestoreContextDw(DwMain);
                this.RestoreContextDw(DwOperateLoan);
                this.RestoreContextDw(DwOperateEtc);
                this.RestoreContextDw(DwTrnColl);
                this.RestoreContextDw(DwTrnCollCo);

            }
            else {
                DwMain.Reset();
                DwMain.InsertRow(0);

                DwOperateLoan.Reset();
                DwOperateLoan.InsertRow(0);

                DwOperateEtc.Reset();
                DwOperateEtc.InsertRow(0);

                DwTrnColl.Reset();
                DwTrnColl.InsertRow(0);

                DwTrnCollCo.Reset();
                DwTrnCollCo.InsertRow(0);

            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "loanCalInt")
            {
                this.LoanCalInt();
            }
            //else if (eventArg == "initDataWindow")
            //{
            //    this.InitDataWindow();
            //}
            else if (eventArg == "calculateAmt")
            {
                this.CalculateAmt();
            }
            else if (eventArg == "searchMemberNo") {
                SearchMemberNo();
            }
            else if (eventArg == "setTrnColl") {
                SetTrnColl();
            }
        }

        public void SaveWebSheet()
        {
            String memno = DwMain.GetItemString(1, "member_no");
            int index = Convert.ToInt32(HfIndex.Value);
            int allIndex = Convert.ToInt32(HfAllIndex.Value);

            str_slippayout strPayOut = new str_slippayout();
            strPayOut.coop_id = state.SsCoopId;
            strPayOut.entry_id = state.SsUsername;
            strPayOut.operate_date = DwMain.GetItemDateTime(1, "operate_date");
            strPayOut.member_no = memno;
            strPayOut.slip_date = state.SsWorkDate;
            strPayOut.initfrom_type = HfFormType.Value;

            String dwMainXML = "";
            String dwLoanXML = "";
            String dwEtcXML = "";

            dwMainXML = DwMain.Describe("DataWindow.Data.XML");
            try { dwLoanXML = DwOperateLoan.Describe("DataWindow.Data.XML"); }
            catch { dwLoanXML = ""; }
            try { dwEtcXML = DwOperateEtc.Describe("DataWindow.Data.XML"); }
            catch { dwEtcXML = ""; }

            strPayOut.xml_sliphead = dwMainXML;
            strPayOut.xml_slipcutlon = dwLoanXML;
            strPayOut.xml_slipcutetc = dwEtcXML;

            try
            {
                int result = srvShrlon.PostShareWithdraw(state.SsWsPass, ref strPayOut);

                int nextIndex = index + 1;
                if (nextIndex > allIndex)
                {
                    nextIndex = index - 1;
                }
                HfIndex.Value = nextIndex.ToString();

                //if (nextIndex != allIndex)
                //{
                //    this.InitDataWindow();
                //}
            }
            catch (Exception ex) { Response.Write("<script>alert('ไม่สามารถบันทึกเลขทะเบียน " + memno + " ได้');</script>"); }
        }

        public void WebSheetLoadEnd()
        {
            try { DwUtil.RetrieveDDDW(DwMain, "moneytype_code", "sl_share_withdraw.pbl", null); }
            catch { }
            DwMain.SaveDataCache();
            DwOperateLoan.SaveDataCache();
            DwOperateEtc.SaveDataCache();
            DwTrnColl.SaveDataCache();
            DwTrnCollCo.SaveDataCache();
        }

        private void LoanCalInt()
        {

            try
            {
                DateTime dt = new DateTime();
                dt = DwMain.GetItemDateTime(1, "operate_date");
                //DateTime operateDateTH = DwMain.GetItemDateTime(1, "operate_tdate");
                String as_xmlloan = DwOperateLoan.Describe("DataWindow.Data.XML");
                String as_sliptype = DwMain.GetItemString(1, "sliptype_code");
                String xmlloan = srvShrlon.InitSlipPayInCalInt(state.SsWsPass, as_xmlloan, as_sliptype, dt);
                DwOperateLoan.Reset();
                DwOperateLoan.ImportString(xmlloan, FileSaveAsType.Xml);
                CalculateAmt();
            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex.ToString()); }
        }

        private void CalculateAmt()
        {
            int loanAllRow = DwOperateLoan.RowCount;
            Decimal totalamt = 0;
            Decimal dwmain_bfshrcontbalamt = DwMain.GetItemDecimal(1, "bfshrcont_balamt");
            //ยอดโอนชำระ
            Decimal payoutclramt = 0;
            int protectcalculate = 1;
            for (int i = 1; i <= loanAllRow; i++)
            {
                Decimal itempayamt = DwOperateLoan.GetItemDecimal(i, "item_payamt");
                //ต้นเงิน bfshrcont_balamt
                Decimal dwloan_bfshrcontbalamt = DwOperateLoan.GetItemDecimal(i, "bfshrcont_balamt");
                //การชำระ ต้นเงิน
                Decimal principalpayamt = DwOperateLoan.GetItemDecimal(i, "principal_payamt");
                Decimal interest_payamt = DwOperateLoan.GetItemDecimal(i, "interest_payamt");
                //ดอกเบี้ยสะสม interest_period + bfintarr_amt 
                Decimal interestperiod = DwOperateLoan.GetItemDecimal(i, "interest_period");
                Decimal bfintarramt = DwOperateLoan.GetItemDecimal(i, "bfintarr_amt");
                Decimal operateflag = DwOperateLoan.GetItemDecimal(i, "operate_flag");
                if (itempayamt == 0)
                {
                    if (operateflag == 1)
                    {
                        if (dwmain_bfshrcontbalamt < dwloan_bfshrcontbalamt)
                        {
                            if (protectcalculate == 1)
                            {
                                DwOperateLoan.SetItemDecimal(i, "principal_payamt", dwmain_bfshrcontbalamt);
                                principalpayamt = DwOperateLoan.GetItemDecimal(i, "principal_payamt");
                                protectcalculate += 1;
                            }
                        }
                        DwOperateLoan.SetItemDecimal(i, "interest_payamt", interestperiod + bfintarramt);
                        Decimal interest_payamt_af = DwOperateLoan.GetItemDecimal(i, "interest_payamt");
                        DwOperateLoan.SetItemDecimal(i, "item_payamt", principalpayamt + interest_payamt_af);
                    }
                }
                else if (itempayamt != 0)
                {
                    DwOperateLoan.SetItemDecimal(i, "item_payamt", principalpayamt + interest_payamt);
                }
                //ใช้คำนวณ ยอด ต้นคงเหลือ
                principalpayamt = DwOperateLoan.GetItemDecimal(i, "principal_payamt");
                DwOperateLoan.SetItemDecimal(i, "item_balance", dwloan_bfshrcontbalamt - principalpayamt);

                //หายอดรวมที่ต้องชำระ
                itempayamt = DwOperateLoan.GetItemDecimal(i, "item_payamt");
                totalamt += itempayamt;
                payoutclramt += principalpayamt + interest_payamt;
            }
            //Set ค่า TotalAmt ไปที่ DwMain ช่อง ยอดโอนชำระ payoutnet_amt
            DwMain.SetItemDecimal(1, "payoutclr_amt", payoutclramt);
            //Set ค่า TotalAmt ไปที่ DwMain ช่อง ยอดหุ้นชำระ payoutnet_amt
            DwMain.SetItemDecimal(1, "payoutnet_amt", dwmain_bfshrcontbalamt - payoutclramt);
        }

        private void SearchMemberNo()
        {
            //int listIndex = 0;
            //ArrayList dwList = new ArrayList();
            //dwList = (ArrayList)Session["SSListWD"];

            //HfAllIndex.Value = dwList.Count.ToString(); //จำนวน Index ของ Array Data ที่ส่งมาจากหน้า Sheet


            //try { listIndex = Convert.ToInt32(HfIndex.Value); }
            //catch { HfIndex.Value = "0"; listIndex = 0; }

            //LbSaveStatus.Text = "(" + (listIndex + 1) + "/" + HfAllIndex.Value + ")";


            str_slippayout sSlipPayOut = new str_slippayout();
            //sSlipPayOut = (str_slippayout) dwList[listIndex];

            sSlipPayOut.coop_id = state.SsCoopId;
            sSlipPayOut.entry_id = state.SsUsername;
            sSlipPayOut.operate_date = state.SsWorkDate;
            sSlipPayOut.slip_date = state.SsWorkDate;
            sSlipPayOut.initfrom_type = "SWD";
            String memberNo = "";
            try {
                try { memberNo = DwMain.GetItemString(1, "member_no"); }
                catch { memberNo = HdMemberNo.Value; }
                sSlipPayOut.member_no = memberNo;
                //String xmlSlipHead = DwMain.Describe("DataWindow.Data.XML");
                //sSlipPayOut.xml_sliphead = xmlSlipHead;
                //String xmlSlipEtc = "";
                //xmlSlipEtc = DwOperateEtc.Describe("DataWindow.Data.XML");
                //sSlipPayOut.xml_slipcutetc = xmlSlipEtc;
                //String xmlSlipLoan = "";
                //xmlSlipLoan= DwOperateLoan.Describe("DataWindow.Data.XML");
                //sSlipPayOut.xml_slipcutlon = xmlSlipLoan;

            }
            catch(Exception ex) {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
            
            srvShrlon.InitShareWithdraw(state.SsWsPass, ref sSlipPayOut);

            try
            {
                DwMain.Reset();
                //DwUtil.RetrieveDDDW(DwMain, "moneytype_code", "sl_share_withdraw.pbl", null);
                DwMain.ImportString(sSlipPayOut.xml_sliphead, Sybase.DataWindow.FileSaveAsType.Xml);
                DwUtil.DeleteLastRow(DwMain);
                HfFormType.Value = sSlipPayOut.initfrom_type;
                tDwMain.Eng2ThaiAllRow();
                DwMain.SetItemDecimal(1, "payoutnet_amt", 0);

            }
            catch (Exception ex) { String ext = ex.ToString(); }
            try
            {
                DwOperateLoan.Reset();
                DwOperateLoan.ImportString(sSlipPayOut.xml_slipcutlon, Sybase.DataWindow.FileSaveAsType.Xml);
            }
            catch (Exception ex) { String ext = ex.ToString(); }
            try
            {
                DwOperateEtc.Reset();
                DwOperateEtc.ImportString(sSlipPayOut.xml_slipcutetc, Sybase.DataWindow.FileSaveAsType.Xml);
            }
            catch (Exception ex) { String ext = ex.ToString(); }
            Decimal dwmain_bfshrcontbalamt = DwMain.GetItemDecimal(1, "bfshrcont_balamt");
            Decimal payoutclr_amt = DwMain.GetItemDecimal(1, "payoutclr_amt");
            DwMain.SetItemDecimal(1, "payoutnet_amt", dwmain_bfshrcontbalamt - payoutclr_amt);
        }

        private void SetTrnColl()
        { 
            str_esttrncoll strEstTrnColl = new str_esttrncoll ();
            String xmlDwOperateLoan = "";
            try
            {
                xmlDwOperateLoan = DwOperateLoan.Describe("DataWindow.Data.XML");
            }
            catch
            {
                xmlDwOperateLoan = "";
            }
            strEstTrnColl.xml_prnpayin = xmlDwOperateLoan;

            srvShrlon.InitEstTrnColl(state.SsWsPass, ref strEstTrnColl);
            try
            {
                DwTrnColl.Reset();
                DwTrnColl.ImportString(strEstTrnColl.xml_trncoll, FileSaveAsType.Xml);
            }
            catch { DwTrnColl.Reset(); }
            try
            {
                DwTrnCollCo.Reset();
                DwTrnCollCo.ImportString(strEstTrnColl.xml_trncollco, FileSaveAsType.Xml);
            }
            catch
            {
                DwTrnCollCo.Reset();
            }

          
            
        }
    }
}
