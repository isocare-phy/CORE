using System;
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
using Saving.CmConfig;
using Sybase.DataWindow;
using Saving.WsCommon;
using Saving.WsShrlon;
using DBAccess;
using System.Web.Services.Protocols;

namespace Saving.Applications.loantracking
{
    public partial class w_sheet_divavg_methodpayment : PageWebSheet, WebSheet
    {
        private Shrlon shrlonService;
        private Common commonService;
        protected String postInit;
        protected String postNewClear;
        protected String postInitMember;
        protected String postDeleteRow;
        protected String postAddRow;
        protected String postSumAll;
        protected String postBankbranch;
        protected String postCBT;
        protected String postTypeLON;
        protected String postTypeDEP;
        protected String postClearType;
        protected String postClearBranch;
        protected String postCheckBF;
        //======================
        private void JspostClearBranch()
        {
            int RowChange = int.Parse(HdRowCurrent.Value);
            Dw_detail.SetItemString(RowChange, "bank_branch", null);
            Hdbank_accid.Value = "";
        }

        private void JspostClearType()
        {
            int RowChange = 0;
            try
            {
                RowChange = int.Parse(HdRowCurrent.Value);
            }
            catch { RowChange = 0; }
            Dw_detail.SetItemString(RowChange, "description", null);
            Dw_detail.SetItemDecimal(RowChange, "item_amt", 0);
            Dw_detail.SetItemDecimal(RowChange, "cut_divamt", 0);
            Dw_detail.SetItemDecimal(RowChange, "cut_avgamt", 0);
            Dw_detail.SetItemDecimal(RowChange, "cut_giftamt", 0);
            Dw_detail.SetItemString(RowChange, "bank_code", "");
            Dw_detail.SetItemString(RowChange, "bank_branch", "");
            Dw_detail.SetItemString(RowChange, "bank_accid", "");
            Dw_detail.SetItemString(RowChange, "loancontract_no", "");
            Dw_detail.SetItemString(RowChange, "deptaccount_no", "");
        }

        private void JspostTypeDEP()
        {
            String deptaccount_no = "";
            String depttype_desc = "";
            int RowChange = 0;
            try
            {
                RowChange = int.Parse(HdRowCurrent.Value);
            }
            catch { RowChange = 0; }

            try
            {
                deptaccount_no = Hd_deptaccount_no.Value;
            }
            catch
            {
                deptaccount_no = "";
            }

            try
            {
                Dw_detail.SetItemString(RowChange, "deptaccount_no", deptaccount_no);
            }
            catch { Dw_detail.SetItemString(RowChange, "deptaccount_no", ""); }
            //==============
            try
            {
                depttype_desc = Hd_depttype_desc.Value;
            }
            catch
            {
                depttype_desc = "";
            }

            try
            {
                Dw_detail.SetItemString(RowChange, "description", depttype_desc);
            }
            catch { Dw_detail.SetItemString(RowChange, "description", ""); }
        }


        private void JspostTypeLON()
        {
            String loancontract_no = "";
            String loantype_desc = "";
            int RowChange = 0;
            try
            {
                RowChange = int.Parse(HdRowCurrent.Value);
            }
            catch { RowChange = 0; }

            try
            {
                loancontract_no = Hd_loancontract_no.Value;
            }
            catch
            {
                loancontract_no = "";
            }

            try
            {
                Dw_detail.SetItemString(RowChange, "loancontract_no", loancontract_no);
            }
            catch { Dw_detail.SetItemString(RowChange, "loancontract_no", ""); }
            //============
            try
            {
                loantype_desc = Hd_loantype_desc.Value;
            }
            catch
            {
                loantype_desc = "";
            }

            try
            {
                Dw_detail.SetItemString(RowChange, "description", loantype_desc);
            }
            catch { Dw_detail.SetItemString(RowChange, "description", ""); }
        }

        private void JspostCBT()
        {
            String divpaytype_code = "";
            int RowChange = 0;
            try
            {
                RowChange = int.Parse(HdRowCurrent.Value);
            }
            catch { RowChange = 0; }

            try
            {
                divpaytype_code = Hddivpaytype_code.Value;
            }
            catch
            {
                divpaytype_code = "";
            }

            try
            {
                Dw_detail.SetItemString(RowChange, "divpaytype_code", Hddivpaytype_code.Value);
            }
            catch { Dw_detail.SetItemString(RowChange, "divpaytype_code", ""); }


        }
        private void JspostSumPayAll()
        {
            int RowbranchId = int.Parse(HdRowCurrent.Value);
            Decimal cut_divamt, cut_avgamt, cut_giftamt, item_amt;
            String description = "";
            String BankACCID = "";

            try
            {
                if (Hddescription.Value != "")
                {
                    description = Hddescription.Value;
                    Dw_detail.SetItemString(RowbranchId, "description", description);
                }
            }
            catch
            {
                description = "";
            }
            //=========

            try
            {
                if (Hdbank_accid.Value  != "")
                {
                    BankACCID = Hdbank_accid.Value;
                    Dw_detail.SetItemString(RowbranchId, "description", BankACCID);
                }
            }
            catch
            {
                description = "";
            }
            //========
            
            try
            {
                Dw_detail.SetItemDecimal(RowbranchId, "cut_divamt", Convert.ToDecimal(Hdcut_divamt.Value));
                cut_divamt = Convert.ToDecimal(Hdcut_divamt.Value);
            }
            catch
            {
                Dw_detail.SetItemDecimal(RowbranchId, "cut_divamt", 0);
                cut_divamt = 0;
            }
            //=========
            try
            {
                Dw_detail.SetItemDecimal(RowbranchId, "cut_avgamt", Convert.ToDecimal(Hdcut_avgamt.Value));
                cut_avgamt = Convert.ToDecimal(Hdcut_avgamt.Value);
            }
            catch
            {
                Dw_detail.SetItemDecimal(RowbranchId, "cut_avgamt", 0);
                cut_avgamt = 0;
            }
            //=========
            try
            {
                Dw_detail.SetItemDecimal(RowbranchId, "cut_giftamt", Convert.ToDecimal(Hdcut_giftamt.Value));
                cut_giftamt = Convert.ToDecimal(Hdcut_giftamt.Value);
            }
            catch
            {
                Dw_detail.SetItemDecimal(RowbranchId, "cut_giftamt", 0);
                cut_giftamt = 0;
            }
            //=========
            item_amt = (cut_divamt + cut_avgamt + cut_giftamt);

            try
            {
                Dw_detail.SetItemDecimal(RowbranchId, "item_amt", item_amt);
            }
            catch
            {
                Dw_detail.SetItemDecimal(RowbranchId, "item_amt", 0);
            }

            //=========

        }
        private void JspostAddRow()
        {
            int RowAll = Dw_detail.RowCount;
            Dw_detail.InsertRow(0);
            Dw_detail.SetItemDecimal(RowAll + 1, "seq_no", RowAll + 1);
        }

        private void JspostBankbranch()
        {
            int RowbranchId = int.Parse(HdRowCurrent.Value);
            String BranchID = HdBranchId.Value.Trim();
            String BankCode = HdBankcode.Value.Trim();

            if (BranchID != null || BankCode != null)
            {
                try
                {
                    Dw_detail.SetItemString(RowbranchId, "divpaytype_code", Hddivpaytype_code.Value);
                }
                catch
                {
                    Dw_detail.SetItemString(RowbranchId, "divpaytype_code", "");
                }
                //=========

                try
                {
                    Dw_detail.SetItemString(RowbranchId, "bank_accid", Hdbank_accid.Value);
                }
                catch
                {
                    Dw_detail.SetItemString(RowbranchId, "bank_accid", "");
                }
                //=========
                try
                {
                    Dw_detail.SetItemString(RowbranchId, "bank_code", HdBankcode.Value);
                }
                catch
                {
                    Dw_detail.SetItemString(RowbranchId, "bank_code", "");
                }

                //========
                try
                {
                    Dw_detail.SetItemString(RowbranchId, "bank_branch", BranchID);
                }
                catch
                {
                    Dw_detail.SetItemString(RowbranchId, "bank_branch", "");
                }
            }

            Dw_detail.SaveDataCache();
        }

        private void JsRetrieveDDW()
        {
            DwUtil.RetrieveDDDW(Dw_detail, "bank_code", "div_avg.pbl", null);
            DwUtil.RetrieveDDDW(Dw_detail, "bank_branch", "div_avg.pbl", null);
            DwUtil.RetrieveDDDW(Dw_detail, "bank_accid", "div_avg.pbl", null);
        }

        private void JsClearBeginImp()
        {
            HdRowBankcode.Value = "";
            HdRowBankcode.Value = "";
            HdBranchId.Value = "";
            HdBankcode.Value = "";
            Hdcut_avgamt.Value = "";
            Hdcut_divamt.Value = "";
            Hdcut_giftamt.Value = "";
            Hddivpaytype_code.Value = "";
            Hddescription.Value = "";
            Hdbank_accid.Value = "";
            Hd_loancontract_no.Value = "";
            Hd_loantype_desc.Value = "";
            Hd_deptaccount_no.Value = "";
            Hd_depttype_desc.Value = "";
        }

        private void JspostDeleteRow()
        {
            int RowCurrent = int.Parse(HdRowCurrent.Value);
            Dw_detail.DeleteRow(RowCurrent);
        }

        private void JsGetYear()
        {
            Sta ta = new Sta(sqlca.ConnectionString);
            int account_year = 0;
            try
            {

                String sql = @"select max(DISTINCT DIV_YEAR) from MBDIVMASTER";
                Sdt dt = ta.Query(sql);
                if (dt.Next())
                {
                    account_year = int.Parse(dt.GetString("max(DISTINCTDIV_YEAR)"));
                    Dw_main.SetItemString(1, "div_year", account_year.ToString());
                }
                else
                {
                    sqlca.Rollback();
                }
            }
            catch (Exception ex)
            {
                account_year = DateTime.Now.Year;
                account_year = account_year + 543;
                Dw_main.SetItemString(1, "div_year", account_year.ToString());
            }
        }

        private void JspostInitMember()
        {
            Decimal  dividend_amt;
            Decimal  average_amt;
            try
            {
                Dw_main.SetItemString(1, "member_no", Hdmem_no.Value);
                String div_year = Dw_main.GetItemString(1, "div_year");
                String member_no = Dw_main.GetItemString(1, "member_no");
                String[] resultXML = shrlonService.InitDivMethodPM(state.SsWsPass, div_year, member_no);
                if (resultXML[0] != "")
                {
                    JsClearBeginImp();
                    Dw_main.Reset();
                    Dw_main.ImportString(resultXML[0], FileSaveAsType.Xml);

                    if (resultXML[1] != "")
                    {
                        Dw_detail.Reset();
                        DwUtil.ImportData(resultXML[1], Dw_detail, null, FileSaveAsType.Xml);
                        Dw_detail.SetItemDecimal(1, "seq_no", 1);
                    }


                    try 
                    {
                        dividend_amt = Dw_main.GetItemDecimal(1, "dividend_amt");
                    }
                    catch { dividend_amt = 0; }

                    try
                    {
                        average_amt = Dw_main.GetItemDecimal(1, "average_amt");
                    }
                    catch { average_amt = 0; }

                    
                    if (dividend_amt == 0 && average_amt == 0)
                    {
                        LtServerMessage.Text = WebUtil.CompleteMessage("ไม่มียอดเงินปันผลและยอดเงินเฉลี่ยคืน");
                    }
                    else if (dividend_amt == 0)
                    {
                        LtServerMessage.Text = WebUtil.CompleteMessage("ไม่มียอดเงินปันผล");
                    }
                    else if (average_amt == 0) 
                    {
                        LtServerMessage.Text = WebUtil.CompleteMessage("ไม่มียอดเงินเฉลี่ยคืน");
                    }
                }
                else
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบข้อมูล กรุณาตรวจสอบใหม่");
                    JspostNewClear();
                }
            }
            catch (SoapException ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("เกิดข้อผิดพลาด " + WebUtil.SoapMessage(ex));
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
            }
        }

        private void JspostNewClear()
        {
            Dw_main.Reset();
            Dw_main.InsertRow(0);
            Dw_detail.Reset();
            JsGetYear();
            HdRowBankcode.Value = "";
            HdRowBankcode.Value = "";
            HdBranchId.Value = "";
            HdBankcode.Value = "";
            Hdcut_avgamt.Value = "";
            Hdcut_divamt.Value = "";
            Hdcut_giftamt.Value = "";
            Hddivpaytype_code.Value = "";
            Hddescription.Value = "";
            Hdbank_accid.Value = "";
            Hd_loancontract_no.Value = "";
            Hd_loantype_desc.Value = "";
            Hd_deptaccount_no.Value = "";
            Hd_depttype_desc.Value = "";
        }


        private void JspostInit()
        {
            Decimal dividend_amt;
            Decimal average_amt;

            string d_year = "";
            string mber = "";
            try
            {
                d_year = Dw_main.GetItemString(1, "div_year");
            }
            catch { }

            try
            {
                mber = Dw_main.GetItemString(1, "member_no");
            }
            catch { }


            if (d_year == null || mber == null || d_year == "" || mber == "")
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอกข้อมูลให้ครบ");
            }
            else
            {
                try
                {
                    String div_year = Dw_main.GetItemString(1, "div_year");
                    String member_no = Dw_main.GetItemString(1, "member_no");
                    String[] resultXML = shrlonService.InitDivMethodPM(state.SsWsPass, div_year, member_no);
                    if (resultXML[0] != "")
                    {
                        JsClearBeginImp();
                        Dw_main.Reset();
                        Dw_main.ImportString(resultXML[0], FileSaveAsType.Xml);

                        if (resultXML[1] != "")
                        {
                            Dw_detail.Reset();
                            DwUtil.ImportData(resultXML[1], Dw_detail, null, FileSaveAsType.Xml);
                            Dw_detail.SetItemDecimal(1, "seq_no", 1);
                        }

                        try
                        {
                            dividend_amt = Dw_main.GetItemDecimal(1, "dividend_amt");
                        }
                        catch { dividend_amt = 0; }

                        try
                        {
                            average_amt = Dw_main.GetItemDecimal(1, "average_amt");
                        }
                        catch { average_amt = 0; }


                        if (dividend_amt == 0 && average_amt == 0)
                        {
                            LtServerMessage.Text = WebUtil.CompleteMessage("ไม่มียอดเงินปันผลและยอดเงินเฉลี่ยคืน");
                        }
                        else if (dividend_amt == 0)
                        {
                            LtServerMessage.Text = WebUtil.CompleteMessage("ไม่มียอดเงินปันผล");
                        }
                        else if (average_amt == 0)
                        {
                            LtServerMessage.Text = WebUtil.CompleteMessage("ไม่มียอดเงินเฉลี่ยคืน");
                        }
                    }
                    else
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบข้อมูล กรุณาตรวจสอบใหม่");
                        JspostNewClear();
                    }

                   

                }
                catch (SoapException ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("เกิดข้อผิดพลาด " + WebUtil.SoapMessage(ex));
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
                }
            }

        }


        #region WebSheet Members

        public void InitJsPostBack()
        {
            postInit = WebUtil.JsPostBack(this, "postInit");
            postNewClear = WebUtil.JsPostBack(this, "postNewClear");
            postInitMember = WebUtil.JsPostBack(this, "postInitMember");
            postDeleteRow = WebUtil.JsPostBack(this, "postDeleteRow");
            postAddRow = WebUtil.JsPostBack(this, "postAddRow");
            postSumAll = WebUtil.JsPostBack(this, "postSumAll");
            postBankbranch = WebUtil.JsPostBack(this, "postBankbranch");
            postCBT = WebUtil.JsPostBack(this, "postCBT");
            postTypeDEP = WebUtil.JsPostBack(this, "postTypeDEP");
            postTypeLON = WebUtil.JsPostBack(this, "postTypeLON");
            postClearType = WebUtil.JsPostBack(this, "postClearType");
            postClearBranch = WebUtil.JsPostBack(this, "postClearBranch");
            postCheckBF = WebUtil.JsPostBack(this, "postCheckBF");
        }


        public void WebSheetLoadBegin()
        {
            try
            {
                this.ConnectSQLCA();
            }
            catch { LtServerMessage.Text = WebUtil.CompleteMessage("ติดต่อ Database ไม่ได้"); }

            try
            {
                shrlonService = new Shrlon();
                commonService = new Common();

            }
            catch { LtServerMessage.Text = WebUtil.CompleteMessage("ติดต่อ Service ไม่ได้"); }

            Dw_main.SetTransaction(sqlca);
            Dw_detail.SetTransaction(sqlca);
            if (!IsPostBack)
            {
                JspostNewClear();
            }
            else
            {
                this.RestoreContextDw(Dw_main);
                this.RestoreContextDw(Dw_detail);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postInit")
            {
                JspostInit();
            }
            else if (eventArg == "postNewClear")
            {
                JspostNewClear();
            }
            else if (eventArg == "postInitMember")
            {
                JspostInitMember();
            }
            else if (eventArg == "postDeleteRow")
            {
                JspostDeleteRow();
            }
            else if (eventArg == "postAddRow")
            {
                JspostAddRow();
            }
            else if (eventArg == "postSumAll")
            {
                JspostSumPayAll();
            }
            else if (eventArg == "postBankbranch")
            {
                JspostBankbranch();
            }
            else if (eventArg == "postCBT")
            {
                JspostCBT();
            }
            else if (eventArg == "postTypeLON")
            {
                JspostTypeLON();
            }
            else if (eventArg == "postTypeDEP")
            {
                JspostTypeDEP();
            }
            else if (eventArg == "postClearType")
            {
                //JspostClearType();
            }
            else if (eventArg == "postClearBranch")
            {
                JspostClearBranch();
            }
            else if (eventArg == "postCheckBF")
            {
                //Refresh
            }
        }


        public void SaveWebSheet()
        {
            string d_year = "";
            string mber = "";
            try
            {
                d_year = Dw_main.GetItemString(1, "div_year");
            }
            catch { d_year = ""; }

            try
            {
                mber = Dw_main.GetItemString(1, "member_no");
            }
            catch { mber = ""; }


            if (d_year == null || mber == null)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบเลขที่สมาชิก ไม่สามารถบันทึกได้");
            }
            else 
            {
                int li_row = Dw_detail.FindRow("isnull( divpaytype_code )or divpaytype_code = '' ", 0, Dw_detail.RowCount);
                if (li_row > 0)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถบันทึกได้ กรุณาลบข้อมูลแถวว่าง");
                }
                else 
                {
                    try
                    {
                        String xml_main = Dw_main.Describe("DataWindow.Data.XML");
                        String xml_detail = Dw_detail.Describe("DataWindow.Data.XML");
                        String entry_id = state.SsUsername;

                        int SaveDivMethodPM = shrlonService.SaveDivMethodPM(state.SsWsPass, xml_main, xml_detail);
                        if (SaveDivMethodPM == 1)
                        {
                            LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลเรียบร้อยแล้ว");
                            JspostNewClear();
                        }
                    }
                    catch (SoapException ex)
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage("เกิดข้อผิดพลาด " + WebUtil.SoapMessage(ex));
                    }
                    catch (Exception ex)
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                    }
                }
            }
        }

        public void WebSheetLoadEnd()
        {
            if (Dw_main.RowCount > 1)
            {
                Dw_main.DeleteRow(Dw_main.RowCount);
            }

                Dw_main.SaveDataCache();
                Dw_detail.SaveDataCache();

                int RowbranchId = int.Parse(HdRowCurrent.Value);


                String divType_code = "";
                String description = "";
                String bankacc_id = "";
                String bank_code = "";
                String bank_branch = "";
                try
                {
                    divType_code = Dw_detail.GetItemString(RowbranchId, "divpaytype_code");
                    if (divType_code == "LON")
                    {
                        try
                        {
                            if (Hd_loantype_desc.Value != null || Hd_loantype_desc.Value != "")
                            {
                                description = Hd_loantype_desc.Value;
                                Dw_detail.SetItemString(RowbranchId, "description", description);
                            }
                            else
                            {
                                description = Hddescription.Value;
                                Dw_detail.SetItemString(RowbranchId, "description", description);
                            }

                            if (Hd_loancontract_no.Value != null || Hd_loancontract_no.Value != "")
                            {
                                bankacc_id = Hd_loancontract_no.Value;
                                Dw_detail.SetItemString(RowbranchId, "loancontract_no", bankacc_id);
                            }
                            else
                            {
                                bankacc_id = Hdbank_accid.Value;
                                Dw_detail.SetItemString(RowbranchId, "loancontract_no", bankacc_id);
                            }


                        }
                        catch
                        {
                            description = "";
                            bankacc_id = "";
                        }
                    }
                    else if (divType_code == "DEP")
                    {
                        try
                        {
                            if (Hd_depttype_desc.Value != null || Hd_depttype_desc.Value != "")
                            {
                                description = Hd_depttype_desc.Value;
                                Dw_detail.SetItemString(RowbranchId, "description", description);
                            }
                            else
                            {
                                description = Hddescription.Value;
                                Dw_detail.SetItemString(RowbranchId, "description", description);
                            }

                            if (Hd_deptaccount_no.Value != null || Hd_deptaccount_no.Value != "")
                            {
                                bankacc_id = Hd_deptaccount_no.Value;
                                Dw_detail.SetItemString(RowbranchId, "deptaccount_no", bankacc_id);
                            }
                            else
                            {
                                bankacc_id = Hdbank_accid.Value;
                                Dw_detail.SetItemString(RowbranchId, "deptaccount_no", bankacc_id);
                            }
                        }
                        catch
                        {
                            description = "";
                            bankacc_id = "";
                        }
                    }
                    else
                    {
                        if (Hddescription.Value != null || Hddescription.Value != "")
                        {
                            description = Hddescription.Value;
                            Dw_detail.SetItemString(RowbranchId, "description", description);
                        }
                        if (Hdbank_accid.Value != null || Hdbank_accid.Value != "")
                        {
                            bankacc_id = Hdbank_accid.Value;
                            Dw_detail.SetItemString(RowbranchId, "deptaccount_no", bankacc_id);
                        }
                        if (HdBankcode.Value != null || HdBankcode.Value != "")
                        {
                            bank_code = HdBankcode.Value;
                            Dw_detail.SetItemString(RowbranchId, "bank_code", bank_code);
                        }
                        if (HdBranchId.Value != null || HdBranchId.Value != "")
                        {
                            bank_branch = HdBranchId.Value;

                            Dw_detail.SetItemString(RowbranchId, "bank_branch", bank_branch);
                        }

                    }
                }
                catch
                {
                    divType_code = "";
                }
         

        #endregion

        }
    }
}
