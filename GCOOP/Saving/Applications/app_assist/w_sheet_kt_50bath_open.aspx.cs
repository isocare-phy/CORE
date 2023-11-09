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
using DataLibrary;
using Sybase.DataWindow;
using CoreSavingLibrary.WcfNDeposit;
using System.Web.Services.Protocols;
using Saving.ConstantConfig;

namespace Saving.Applications.app_assist
{
    public partial class w_sheet_kt_50bath_open : PageWebSheet, WebSheet
    {
        private DwThDate tDwMain;
        private DwThDate tDwCheque;
        private DwThDate tDwItem;
        private String deptAccountNo = null;
        private n_depositClient depService;
        private bool isException = false;
        private DepositConfig depConfig;
        protected Sta ta;
        private bool IsAutoDeptWith
        {
            get
            {
                try
                {
                    return Session["is_auto_deptwith"].ToString().ToLower() == "true";
                }
                catch { return false; }
            }
            set
            {
                Session["is_auto_deptwith"] = value;
            }
        }
        private bool checkBox1Before;
        private bool completeCheque;

        //POSTBACK
        protected String postPost;
        protected String postNewAccount;
        protected String postDeptWith;
        protected String postRecpPayTypeCode;
        protected String newClear;
        protected String postTotalWidthFixed;
        protected String postItemSelect;
        protected String postSaveNoCheckApv;
        protected String postInsertRowCheque;
        protected String postDeleteRowCheque;
        protected String postBankCode;
        protected String postBankBranchCode;

        private String TryDwMainGetString(String column)
        {


            try
            {
                return DwMain.GetItemString(1, column).Trim();
            }
            catch
            {
                return "";
            }
        }

        private void CallPrintBook(String slipNo, String message)
        {
            //HdPrintFlag.Value = "true";
            ////gs_command[0] = account_no
            ////gs_command[1] = branch_id
            ////gs_command[2] = entry_id
            ////gs_command[3] = entry_date
            ////gs_command[4] = DBMS
            ////gs_command[5] = LogPass
            ////gs_command[6] = ServerName
            ////gs_command[7] = LogId
            ////gs_command[8] = E หรือ T
            ////gs_command[9] = B หรือ S
            //String[] gs_command = new String[10];
            //gs_command[0] = deptAccountNo;
            //gs_command[1] = state.SsCoopId;
            //gs_command[2] = state.SsUsername;
            //gs_command[3] = state.SsWorkDate.ToString("yyyyMMdd", WebUtil.EN);
            //gs_command[4] = "O10 Oracle10g (10.1.0)";
            //gs_command[5] = WebUtil.GetConnectionElement("Password");
            //gs_command[6] = WebUtil.GetConnectionElement("Data Source");
            //gs_command[7] = WebUtil.GetConnectionElement("User ID");
            //gs_command[8] = "T";
            //gs_command[9] = "B";
            //HdPrintBook.Value = String.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}", gs_command);
            //if (depConfig.PrintSlipStatus == 1)
            //{
            //    gs_command[9] = "S";
            //    HdPrintSlip.Value = String.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}", gs_command);
            //}
            //else
            //{
            //    HdPrintSlip.Value = "";
            //}
            HdPrintFlag.Value = "true";
            HdPrintBook.Value = deptAccountNo;
            try
            {
                if (depConfig.PrintSlipStatus == 1)
                {
                    //depService.PrintSlip(state.SsWsPass, slipNo, state.SsCoopId, state.SsPrinterSet);
                    //XmlConfigService xml = new XmlConfigService();
                    int printStatus = 1;// xml.DepositPrintMode;
                    string xml_return = "", xml_return_bf = "";
                    depService.of_print_slip(state.SsWsPass, slipNo, state.SsCoopId, state.SsPrinterSet, printStatus, ref xml_return);
                    if (xml_return != "" && printStatus == 1)
                    {
                        //if (printStatus == 1)
                        //{
                        //    Printing.Print(this, "Slip/ap_deposit/PrintSlip_MHD.aspx", xml_return, 1);
                        //}
                        //else
                        //{
                        Printing.PrintApplet(this, "dept_slip", xml_return);
                        //}
                    }
                }
                LtServerMessage.Text = WebUtil.CompleteMessage(message);
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.WarningMessage(message + " , ไม่สามารถเชื่อมต่อเครื่องพิมพ์ slip");
            }
            JsNewClear();
        }

        private String GetDwItemXml(bool forceItem)
        {
            String xml = ""; ;
            if (!forceItem)
            {
                if (DwItem.RowCount > 0)
                {
                    if (DwItem.GetItemDecimal(1, "prnc_no") > 0)
                    {
                        tDwItem.Eng2ThaiAllRow();
                        xml = DwItem.Describe("DataWindow.Data.XML");
                    }
                }
            }
            else
            {
                tDwItem.Eng2ThaiAllRow();
                xml = DwItem.Describe("DataWindow.Data.XML");
            }
            return xml;
        }

        //JS-EVENT
        private void JsNewAccountNo()
        {
            String accNo = null;
            //DwMain.Reset();
            DwCheque.Reset();
            DwItem.Reset();
            try
            {
                accNo = DwMain.GetItemString(1, "deptformat");
                accNo = depService.BaseFormatAccountNo(state.SsWsPass, accNo);
            }
            catch
            {
                this.deptAccountNo = null;
                return;
            }
            try
            {
                //isException = false;
                //String ls_xml = depService.InitDepSlip(state.SsWsPass, accNo, state.SsCoopId, state.SsUsername, state.SsWorkDate, state.SsClientIp);
                //DwUtil.ImportData(ls_xml, DwMain, tDwMain, FileSaveAsType.Xml);
                //String depformat = depService.ViewAccountNoFormat(state.SsWsPass, accNo);
                //DwMain.SetItemString(1, "deptformat", depformat);
                if (DwMain.RowCount != 1)
                {
                    throw new Exception("Import ไม่สำเร็จ ไม่ทราบสาเหตุ");
                }
                HdNewAccountNo.Value = "true";
            }
            catch (Exception ex)
            {
                isException = true;
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
            finally
            {
                if (isException)
                {
                    accNo = null;
                    JsNewClear();
                }
            }
            this.deptAccountNo = accNo;
        }

        //JS-EVENT
        private void JsPostDeptWith()
        {
            String accNo = DwMain.GetItemString(1, "deptaccount_no");
            try
            {
                isException = false;
                try
                {
                    DwMain.SetItemString(1, "deptitemtype_code", DwMain.GetItemString(1, "recppaytype_code"));
                }
                catch { }
                DwMain.SetItemString(1, "tofrom_accid", "");

                String deptWithFlag = TryDwMainGetString("deptwith_flag");
                String recpPayTypeCode = TryDwMainGetString("recppaytype_code");
                String deptTypeCode = TryDwMainGetString("DEPTTYPE_CODE");
                String cashType = depService.of_get_cashtype(state.SsWsPass, recpPayTypeCode);// TryDwMainGetString("cash_type");
                Decimal adc_intsum = 0;
                if (cashType == "CHQ" && DwCheque.RowCount < 1)
                {
                    DwMain.SetItemDecimal(1, "deptslip_amt", 0);
                    DwMain.SetItemDecimal(1, "deptslip_netamt", 0);
                    JsPostInsertRowCheque();
                }
                if (deptWithFlag == "/")
                {
                    DwMain.SetItemDecimal(1, "deptslip_amt", DwMain.GetItemDecimal(1, "prncbal"));
                }
                //------------------ เช็คว่าประจำรึเปล่า ------------------
                try
                {
                    //String accountNo = TryDwMainGetString("deptaccount_no");
                    //String xmlSlipDetail = depService.InitDeptSlipDetail(state.SsWsPass, deptTypeCode, accountNo, state.SsCoopId, state.SsWorkDate, recpPayTypeCode);
                    //DwUtil.ImportData(xmlSlipDetail, DwItem, tDwItem, FileSaveAsType.Xml);
                }
                catch (Exception ex)
                {
                    DwItem.Reset();
                    ex.ToString();
                }
                //-----------------------------------------------
                if (deptWithFlag == "/")
                {
                    String ls_xml = DwMain.Describe("DataWindow.Data.XML");
                    String ls_xml_det = "";
                    if (DwItem.RowCount > 0)
                    {
                        ls_xml_det = DwItem.Describe("DataWindow.Data.XML");
                    }
                    //String[] result = depService.InitDeptSlipCalInt(state.SsWsPass, accNo, state.SsCoopId, state.SsUsername, state.SsWorkDate, state.SsClientIp, ls_xml, ls_xml_det, adc_intsum);
                    //DwUtil.ImportData(result[0], DwMain, tDwMain, FileSaveAsType.Xml);
                    //DwItem.Reset();
                    //if (WebUtil.IsXML(result[1]))
                    //{
                    //    DwUtil.ImportData(result[1], DwItem, tDwItem, FileSaveAsType.Xml);
                    //}
                }
                else if (deptWithFlag == "+")
                {
                    //decimal balance = DwMain.GetItemDecimal(1, "prncbal");
                    //Decimal isEqualDept = depService.IsEqualDept(state.SsWsPass, accNo, state.SsCoopId, balance, deptTypeCode, recpPayTypeCode);
                    //if (isEqualDept > 0)
                    //{
                    //    DwMain.SetItemDecimal(1, "deptslip_amt", isEqualDept);
                    //    DwMain.SetItemDecimal(1, "deptslip_netamt", isEqualDept);
                    //    JsPostTotalWidthFixed();
                    //    DwMain.Modify("deptslip_amt.Protect=1");
                    //}
                }
                DefaultSendGov(cashType);
            }
            catch (Exception ex)
            {
                isException = true;
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
            finally
            {
                if (isException)
                {
                    accNo = null;
                    JsNewClear();
                }
            }
            this.deptAccountNo = accNo;
        }

        //JS-EVENT
        private void JsPostItemSelect()
        {
            //try
            //{
            //    String accNo = DwMain.GetItemString(1, "deptaccount_no");
            //    Decimal princBal = DwItem.GetItemDecimal(1, "prnc_bal");

            //    String xmlDwMain = DwMain.Describe("DataWindow.Data.XML");
            //    String xmlDwItem = DwItem.Describe("DataWindow.Data.XML");
            //  Decimal  adc_intsum = 0;
            //    String[] result = depService.InitDeptSlipCalInt(state.SsWsPass, accNo, state.SsCoopId, state.SsUsername, state.SsWorkDate, state.SsClientIp, xmlDwMain, xmlDwItem, adc_intsum);

            //    DwUtil.ImportData(result[0], DwMain, tDwMain, FileSaveAsType.Xml);
            //    DwUtil.ImportData(result[1], DwItem, tDwItem, FileSaveAsType.Xml);

            //}
            //catch (Exception ex)
            //{
            //    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            //}
        }

        //JS-EVENT
        private void JsNewClear()
        {
            DwMain.Reset();
            DwCheque.Reset();
            DwCheque.SaveDataCache();
            DwItem.Reset();
            DwItem.SaveDataCache();
            DwMain.InsertRow(0);
            DwMain.SaveDataCache();
            HdRequireCalInt.Value = "false";
        }

        //JS-EVENT
        private void JsPostTotalWidthFixed()
        {
            try
            {
                String accNo = DwMain.GetItemString(1, "deptaccount_no");
                String xmlDwMain = DwMain.Describe("DataWindow.Data.XML");
                String xmlDwItem = DwItem.Describe("DataWindow.Data.XML");
                String deptGroupCode = TryDwMainGetString("deptgroup_code");//deptgroup_code 01 = ประจำ
                String deptWithFlag = TryDwMainGetString("deptwith_flag");
                try
                {
                    DwUtil.ImportData(xmlDwItem, DwItem, tDwItem, FileSaveAsType.Xml);
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
                if (deptGroupCode == "01" && deptWithFlag == "+")
                {
                    DwItem.SetItemDecimal(1, "prnc_bal", DwMain.GetItemDecimal(1, "deptslip_amt"));
                    DwItem.SetItemDecimal(1, "prncslip_amt", DwMain.GetItemDecimal(1, "deptslip_netamt"));
                    tDwItem.Eng2ThaiAllRow();
                }
                else if (DwItem.RowCount > 0 && DwItem.GetItemDecimal(1, "prnc_no") > 0)
                {
                    decimal itemAmt;
                    decimal slipAmt = DwMain.GetItemDecimal(1, "deptslip_amt");//GET จาก SLIP
                    for (int i = 1; i <= DwItem.RowCount; i++)
                    {
                        itemAmt = DwItem.GetItemDecimal(i, "prnc_bal");
                        if (slipAmt > itemAmt && itemAmt > 0)
                        {
                            DwItem.SetItemDecimal(i, "select_flag", 1);
                            DwItem.SetItemDecimal(i, "prncslip_amt", itemAmt);
                        }
                        else
                        {

                            if (slipAmt == 0 || itemAmt == 0)
                            {
                                DwItem.SetItemDecimal(i, "select_flag", 0);
                                DwItem.SetItemDecimal(i, "prncslip_amt", 0);
                                DwItem.SetItemDecimal(i, "intslip_amt", 0);
                                DwItem.SetItemDecimal(i, "taxslip_amt", 0);
                                DwItem.SetItemDecimal(i, "fee_amt", 0);
                                DwItem.SetItemDecimal(i, "other_amt", 0);
                                DwItem.SetItemDecimal(i, "int_return", 0);
                                DwItem.SetItemDecimal(i, "intcur_accyear", 0);
                                DwItem.SetItemDecimal(i, "intarr_amt", 0);
                            }
                            else
                            {
                                DwItem.SetItemDecimal(i, "select_flag", 1);
                                DwItem.SetItemDecimal(i, "prncslip_amt", slipAmt);
                            }
                        }
                        slipAmt -= itemAmt;
                        if (slipAmt < 0) slipAmt = 0;
                    }
                }
                if (DwItem.RowCount > 0)
                {
                    xmlDwItem = DwItem.Describe("DataWindow.Data.XML");
                }
                else
                {
                    xmlDwItem = "";
                }
                //Decimal adc_intsum = 0;
                //String[] result = depService.InitDeptSlipCalInt(state.SsWsPass, accNo, state.SsCoopId, state.SsUsername, state.SsWorkDate, state.SsClientIp, xmlDwMain, xmlDwItem, adc_intsum);
                //DwUtil.ImportData(result[0], DwMain, tDwMain, FileSaveAsType.Xml);
                //String cashType = DwMain.GetItemString(1, "cash_type");
                //DefaultSendGov(cashType);
                //DwUtil.ImportData(result[1], DwItem, tDwItem, FileSaveAsType.Xml);

            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        //JS-EVENT
        private void JspostSaveNoCheckApv()
        {
            SaveSheet();
        }

        //JS-EVENT
        private void JsPostInsertRowCheque()
        {
            DwCheque.InsertRow(0);
            DwCheque.SetItemDateTime(DwCheque.RowCount, "cheque_date", state.SsWorkDate);
            DwCheque.SetItemDecimal(DwCheque.RowCount, "day_float", int.Parse(HdDayPassCheq.Value));
            tDwCheque.Eng2ThaiAllRow();
            HdIsInsertCheque.Value = "true";
        }

        //JS-EVENT
        private void JsPostBankCode()
        {
            try
            {
                Int32 row = Convert.ToInt32(HdDwChequeRow.Value);
                String bankCode = DwCheque.GetItemString(row, "bank_code");
                String sql = "select bank_desc from cmucfbank where bank_code='" + bankCode + "'";
                DataTable dt = WebUtil.Query(sql);
                if (dt.Rows.Count > 0)
                {
                    String bankName = dt.Rows[0][0].ToString().Trim();
                    DwCheque.SetItemString(row, "bank_name", bankName);
                }
                else
                {
                    throw new Exception("ไม่พบรหัสธนาคาร " + bankCode);
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        //JS-EVENT
        private void JsPostBankBranchCode()
        {
            try
            {
                Int32 row = Convert.ToInt32(HdDwChequeRow.Value);
                String bankCode = DwCheque.GetItemString(row, "bank_code");
                String branchCode = DwCheque.GetItemString(row, "branch_code");
                String sql = "select branch_name from cmucfbankbranch where bank_code='" + bankCode + "' and branch_id='" + branchCode + "'";
                DataTable dt = WebUtil.Query(sql);
                if (dt.Rows.Count > 0)
                {
                    String branchName = dt.Rows[0][0].ToString().Trim();
                    DwCheque.SetItemString(row, "branch_name", branchName);
                }
                else
                {
                    throw new Exception("ไม่พบรหัสสาขาธนาคารเลขที่ " + branchCode);
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        //JS-EVENT
        private void JsPostDeleteRowCheque()
        {
            try
            {
                DwCheque.DeleteRow(int.Parse(HdDwChequeRow.Value));
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        //INNER FUNCTION
        private void LoopCheque()
        {
            try
            {
                for (int i = 1; i <= DwCheque.RowCount; i++)
                {
                    try
                    {
                        String chequeNo = DwUtil.GetString(DwCheque, i, "cheque_no", "");
                        completeCheque = chequeNo == "" ? false : completeCheque;
                        int ii = chequeNo == "" ? 0 : int.Parse(chequeNo);

                        if (ii > 0)
                        {
                            DwCheque.SetItemString(i, "cheque_no", ii.ToString("0000000"));
                        }
                        else
                        {
                            completeCheque = false;
                        }
                    }
                    catch { completeCheque = false; }
                }
            }
            catch { }
        }

        private void DefaultSendGov(String cashType)
        {
            Decimal deptSlipNetAmt = DwMain.GetItemDecimal(1, "deptslip_netamt");
            DwMain.SetItemDecimal(1, "send_gov", 0);
            if (cashType == "CSH" && deptSlipNetAmt >= 2000000)
            {
                DwMain.SetItemDecimal(1, "send_gov", 1);
            }
            else
            {
                DwMain.SetItemDecimal(1, "send_gov", 0);
            }
        }

        //Using Deposit Service
        private void DepositPost()
        {
            try
            {
                String ls_xml_main = DwMain.Describe("DataWindow.Data.XML");
                String ls_xml_cheque = "";
                String ls_xml_item = "";
                String cashType = DwMain.GetItemString(1, "cash_type");
                String as_apvdoc = "";
                if (cashType == "CHQ" && DwCheque.RowCount > 0)
                {
                    for (int i = 1; i <= DwCheque.RowCount; i++)
                    {
                        int ii = int.Parse(DwCheque.GetItemString(i, "cheque_no"));
                        DwCheque.SetItemString(i, "cheque_no", ii.ToString("0000000"));
                    }
                    ls_xml_cheque = DwCheque.Describe("DataWindow.Data.XML");
                }
                if (DwItem.RowCount > 0 && DwItem.GetItemDecimal(1, "prnc_no") > 0)
                {
                    ls_xml_item = DwItem.Describe("DataWindow.Data.XML");
                }
                String result = depService.of_deposit(state.SsWsPass, ls_xml_main, ls_xml_cheque, ls_xml_item, as_apvdoc);
                String endMessage = "บันทึกทำรายการปิดบัญชี " + depConfig.CnvDeptAccountFormat(deptAccountNo) + " เรียบร้อยแล้ว";
                //CallPrintBook(result, endMessage);
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        //Using Withdraw Service
        private void Withdraw()
        {
            try
            {
                String ls_xml = DwMain.Describe("DataWindow.Data.XML");
                String ls_xml_det = "";
                if (DwItem.RowCount > 0 && DwItem.GetItemDecimal(1, "prnc_no") > 0)
                {
                    ls_xml_det = DwItem.Describe("DataWindow.Data.XML");
                }
                //   String result = depService.WithdrawClose(state.SsWsPass, ls_xml, ls_xml_det);   dotisocare
                String endMessage = "บันทึกทำรายการถอนเงินบัญชี " + depConfig.CnvDeptAccountFormat(deptAccountNo) + " เรียบร้อยแล้ว";
                //CallPrintBook(result, endMessage);
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        //Using Close Account Service
        private void CloseAccount()
        {
            try
            {
                String ls_xml = DwMain.Describe("DataWindow.Data.XML");
                String ls_xml_det = "";
                try
                {
                    if (DwItem.RowCount <= 0)
                    {
                        throw new Exception();
                    }
                    ls_xml_det = DwItem.Describe("DataWindow.Data.XML");
                }
                catch
                {
                    ls_xml_det = "";
                }
                //String result = depService.WithdrawClose(state.SsWsPass, ls_xml, ls_xml_det); dotisocare
                String endMessage = "บันทึกทำรายการฝากบัญชี " + depConfig.CnvDeptAccountFormat(deptAccountNo) + " เรียบร้อยแล้ว";
                double tempIntAmt = DwMain.GetItemDouble(1, "int_amt1");
                double tempPrncbal = DwMain.GetItemDouble(1, "deptslip_amt");
                string accno = DwMain.GetItemString(1, "deptaccount_no");
                accno = depService.BaseFormatAccountNo(state.SsWsPass, accno);

                string strsql = "UPDATE dpdeptmaster SET accuint_amt = " + tempIntAmt + ", prncbal = " + tempPrncbal + " WHERE deptaccount_no = '" + accno + "'";
                ta.Exe(strsql);

                string sqlStr = @"UPDATE  asnreqmaster SET assistdept_amt = '" + (tempIntAmt + tempPrncbal) + "', pay_status ='0' WHERE deptaccount_no ='" + accno + "'";
                ta.Exe(sqlStr);

                JsNewClear();
                LtServerMessage.Text = WebUtil.CompleteMessage(endMessage);
                //CallPrintBook(result, endMessage);
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        private void SaveSheet()
        {
            String control = DwMain.GetItemString(1, "deptwith_flag");
            if (control == "+")
            {
                DepositPost();
            }
            else if (control == "-")
            {
                Withdraw();
            }
            else if (control == "/")
            {
                CloseAccount();
            }
        }

        #region WebSheet Members

        public void InitJsPostBack()
        {
            //----------------------------------------------------------------------
            //----------------------------------------------------------------------
            postNewAccount = WebUtil.JsPostBack(this, "postNewAccount");
            postDeptWith = WebUtil.JsPostBack(this, "postDeptWith");
            postRecpPayTypeCode = WebUtil.JsPostBack(this, "postRecpPayTypeCode");
            newClear = WebUtil.JsPostBack(this, "newClear");
            postBankCode = WebUtil.JsPostBack(this, "postBankCode");
            postBankBranchCode = WebUtil.JsPostBack(this, "postBankBranchCode");
            postTotalWidthFixed = WebUtil.JsPostBack(this, "postTotalWidthFixed");
            postItemSelect = WebUtil.JsPostBack(this, "postItemSelect");
            postSaveNoCheckApv = WebUtil.JsPostBack(this, "postSaveNoCheckApv");
            postInsertRowCheque = WebUtil.JsPostBack(this, "postInsertRowCheque");
            postDeleteRowCheque = WebUtil.JsPostBack(this, "postDeleteRowCheque");
            postPost = WebUtil.JsPostBack(this, "postPost");
            //----------------------------------------------------------------------
            //----------------------------------------------------------------------
            tDwMain = new DwThDate(DwMain, this);
            tDwMain.Add("deptslip_date", "deptslip_date_tdate");
            //----------------------------------------------------------------------
            tDwCheque = new DwThDate(DwCheque, this);
            tDwCheque.Add("cheque_date", "cheque_tdate");
            //----------------------------------------------------------------------
            tDwItem = new DwThDate(DwItem, this);
            tDwItem.Add("calint_from", "calint_from_tdate");
            tDwItem.Add("calint_to", "calint_to_tdate");
            tDwItem.Add("prncdue_date", "prncdue_tdate");//prnc_tdate
            tDwItem.Add("prnc_date", "prnc_tdate");//prnc_tdate
            //----------------------------------------------------------------------
        }

        public void WebSheetLoadBegin()
        {
            this.ConnectSQLCA();
            ta = new Sta(sqlca.ConnectionString);

            HdPrintFlag.Value = "false";
            HdPrintBook.Value = "";
            HdPrintSlip.Value = "";
            HdNewAccountNo.Value = "";
            HdCheckApvAlert.Value = "";
            HdIsInsertCheque.Value = "";
            completeCheque = true;
            try
            {
                depService = wcf.Deposit;
            }
            catch
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ติดต่อ Web Service ไม่ได้");
                return;
            }
            //depConfig = new DepositConfig(depService);
            if (Session["is_auto_deptwith"] != null)
            {
                checkBox1Before = CheckBox1.Checked;
                CheckBox1.Checked = IsAutoDeptWith;
            }
            //---------------------------------------------------------------------
            if (IsPostBack)
            {
                try
                {
                    this.RestoreContextDw(DwMain);
                    this.RestoreContextDw(DwCheque);
                    this.RestoreContextDw(DwItem);
                }
                catch { }
                HdIsPostBack.Value = "true";
            }
            else
            {
                try
                {
                    HdDayPassCheq.Value = depService.GetDpDeptConstant(state.SsWsPass, "daypasschq");
                }
                catch
                {
                    HdDayPassCheq.Value = "1";
                }
                HdIsPostBack.Value = "false";
            }
            if (DwMain.RowCount < 1)
            {
                DwMain.InsertRow(0);
                DwMain.SetItemDate(1, "deptslip_date", state.SsWorkDate);
                tDwMain.Eng2ThaiAllRow();
            }
            LoopCheque();
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postBankCode")
            {
                JsPostBankCode();
            }
            else if (eventArg == "postBankBranchCode")
            {
                JsPostBankBranchCode();
            }
            else if (eventArg == "postNewAccount")
            {
                JsNewAccountNo();
            }
            else if (eventArg == "postDeptWith")
            {
                JsPostDeptWith();
            }
            else if (eventArg == "newClear")
            {
                JsNewClear();
            }
            else if (eventArg == "postTotalWidthFixed")
            {
                JsPostTotalWidthFixed();
            }
            else if (eventArg == "postItemSelect")
            {
                JsPostItemSelect();
            }
            else if (eventArg == "postSaveNoCheckApv")
            {
                deptAccountNo = DwMain.GetItemString(1, "deptaccount_no");
                SaveSheet();
            }
            else if (eventArg == "postInsertRowCheque")
            {
                JsPostInsertRowCheque();
            }
            else if (eventArg == "postDeleteRowCheque")
            {
                JsPostDeleteRowCheque();
            }
        }

        //public void SaveWebSheet()
        //{
        //    if (!completeCheque)
        //    {
        //        LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอกเลขที่เช็คให้ครบถ้วน!");
        //        return;
        //    }
        //    try
        //    {
        //        DwMain.SetItemString(1, "deptitemtype_code", DwMain.GetItemString(1, "recppaytype_code"));
        //        //String control = DwMain.GetItemString(1, "deptwith_flag");
        //        deptAccountNo = DwMain.GetItemString(1, "deptaccount_no");
        //        String slipXml = DwMain.Describe("datawindow.data.xml");
        //        String chqXml = DwCheque.Describe("datawindow.data.xml");
        //        //String[] apv = depService.CheckRightPermissionDep(state.SsWsPass, state.SsUsername, state.SsCoopId, slipXml, chqXml, 0);
        //        String[] apv = depService.CheckRightPermissionDep(state.SsWsPass, state.SsUsername, state.SsCoopId, slipXml, 0);
        //        if (apv[0] == "true")
        //        {
        //            SaveSheet();
        //            //if (control == "+")
        //            //{
        //            //    DepositPost();
        //            //}
        //            //else if (control == "-")
        //            //{
        //            //    Withdraw();
        //            //}
        //            //else if (control == "/")
        //            //{
        //            //    CloseAccount();
        //            //}
        //        }
        //        else
        //        {
        //            DataTable dt = WebUtil.Query("select member_no from dpdeptmaster where deptaccount_no='" + deptAccountNo + "'");
        //            String memberNo = dt.Rows.Count > 0 ? dt.Rows[0][0].ToString() : "";
        //            Decimal netAmt = DwUtil.GetDec(DwMain, 1, "deptslip_netamt", 0);
        //            String accName = DwUtil.GetString(DwMain, 1, "deptaccount_name", "");
        //            String itemType = DwUtil.GetString(DwMain, 1, "recppaytype_code", "");
        //            String itemTypeDesc = depService.GetRecpPayTypeDesc(state.SsWsPass, TryDwMainGetString("recppaytype_code"));

        //            String reportNo = depService.AddApvTask(state.SsWsPass, state.SsUsername, state.SsApplication, state.SsClientIp, itemType, apv[1], deptAccountNo, memberNo, state.SsWorkDate, netAmt, deptAccountNo, accName, apv[0], "DEP", 1, state.SsCoopId);
        //            HdProcessId.Value = reportNo;
        //            HdAvpCode.Value = apv[0];
        //            HdItemType.Value = itemType;
        //            HdAvpAmt.Value = netAmt.ToString();
        //            HdCheckApvAlert.Value = "true";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LtServerMessage.Text = WebUtil.ErrorMessage(ex);
        //    }
        //}
        public void SaveWebSheet()
        {
            if (!completeCheque)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอกเลขที่เช็คให้ครบถ้วน!");
                return;
            }
            try
            {
                DwMain.SetItemString(1, "deptitemtype_code", DwMain.GetItemString(1, "recppaytype_code"));
                //String control = DwMain.GetItemString(1, "deptwith_flag");
                deptAccountNo = DwMain.GetItemString(1, "deptaccount_no");
                String slipXml = DwMain.Describe("datawindow.data.xml");
                String as_apvdoc ="";
                String[] apv = depService.CheckRightPermissionDep(state.SsWsPass, state.SsUsername, state.SsCoopId, slipXml, slipXml, 0,ref as_apvdoc);
                if (apv[0] == "true")
                {
                    SaveSheet();
                    //if (control == "+")
                    //{
                    //    DepositPost();
                    //}
                    //else if (control == "-")
                    //{
                    //    Withdraw();
                    //}
                    //else if (control == "/")
                    //{
                    //    CloseAccount();
                    //}
                }
                else
                {
                    DataTable dt = WebUtil.Query("select member_no from dpdeptmaster where deptaccount_no='" + deptAccountNo + "'");
                    String memberNo = dt.Rows.Count > 0 ? dt.Rows[0][0].ToString() : "";
                    Decimal netAmt = DwUtil.GetDec(DwMain, 1, "deptslip_netamt", 0);
                    String accName = DwUtil.GetString(DwMain, 1, "deptaccount_name", "");
                    String itemType = DwUtil.GetString(DwMain, 1, "recppaytype_code", "");
                    String itemTypeDesc = depService.GetRecpPayTypeDesc(state.SsWsPass, TryDwMainGetString("recppaytype_code"));

                   // String reportNo = depService.AddApvTask(state.SsWsPass, state.SsUsername, state.SsApplication, state.SsClientIp, itemType, apv[1], deptAccountNo, memberNo, state.SsWorkDate, netAmt, deptAccountNo, accName, apv[0], "DEP", 1, state.SsCoopId);
                    //HdProcessId.Value = reportNo;
                    HdAvpCode.Value = apv[0];
                    HdItemType.Value = itemType;
                    HdAvpAmt.Value = netAmt.ToString();
                    HdCheckApvAlert.Value = "true";
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        public void WebSheetLoadEnd()
        {
            //เช็ค deptAccountNo
            try
            {
                deptAccountNo = DwMain.GetItemString(1, "deptaccount_no");
                deptAccountNo = string.IsNullOrEmpty(deptAccountNo) ? null : deptAccountNo;
            }
            catch { deptAccountNo = null; }
            //ใส่ตัวเงินคงเหลือลง text label
            if (!string.IsNullOrEmpty(deptAccountNo))
            {
                DwMain.Modify("prncbal_t.text='" + DwMain.GetItemDecimal(1, "prncbal").ToString("#,##0.00") + "'");
                DwMain.Modify("withdrawable_amt_t.text='" + DwMain.GetItemDecimal(1, "withdrawable_amt").ToString("#,##0.00") + "'");
                //ใส่ชื่อ นามสกุล ทะเบียนลง  text label
                try
                {
                    DataTable dt = WebUtil.Query("select member_no from dpdeptmaster where deptaccount_no='" + deptAccountNo + "'");
                    if (dt.Rows.Count > 0)
                    {
                        DwMain.Modify("t_member_no.text='" + dt.Rows[0]["member_no"].ToString() + "'");
                        HdMemberNo.Value = dt.Rows[0][0].ToString();
                    }
                }
                catch (Exception ex) { ex.ToString(); }
            }
            //ขั้นตอนเกี่ยวกับรายละเอียดประเภทเงิน
            String cashType = "";
            if (!String.IsNullOrEmpty(deptAccountNo))
            {
                try
                {
                    String sql = "select condforwithdraw from dpdeptmaster where deptaccount_no = '" + deptAccountNo + "'";
                    DataTable dt = WebUtil.Query(sql);
                    String condition = dt.Rows.Count > 0 ? dt.Rows[0][0].ToString() : "";
                    DwMain.Modify("t_33.text='" + condition + "'");
                }
                catch { }
                String recPayTypeCode = DwMain.GetItemString(1, "recppaytype_code");
                if (!String.IsNullOrEmpty(recPayTypeCode))
                {
                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    DwMain.Modify("recppaytype_desc_t.text='ออกจากกองทุน'");
                    //DwMain.Modify("recppaytype_desc_t.text='" + depService.GetRecpPayTypeDesc(state.SsWsPass, TryDwMainGetString("recppaytype_code")) + "'");
                    try
                    {
                        //ใส่ dddw สำหรับคู่บัญชี
                        cashType = depService.GetCashType(state.SsWsPass, recPayTypeCode);
                        DwMain.SetItemString(1, "cash_type", cashType);
                        WebUtil.RetrieveDDDW(DwMain, "tofrom_accid", "kt_50bath.pbl", null);
                        DataWindowChild dcToFromAccId = DwMain.GetChild("tofrom_accid");
                        dcToFromAccId.SetFilter("cash_type = '" + cashType + "'");
                        dcToFromAccId.Filter();
                        if (cashType == "CSH")
                        {
                            String accToFromNo1 = dcToFromAccId.GetItemString(1, "account_id");
                            DwMain.SetItemString(1, "tofrom_accid", accToFromNo1);
                        }
                    }
                    catch (Exception ex)
                    {
                        LtServerMessage.Text += (":::" + WebUtil.ErrorMessage(ex)).ToString();
                    }
                }
            }
            //ให้ลบแถวถ้าประเภทเงินไม่ใช่เช็ค
            if (cashType != "CHQ")
            {
                int i = 0;
                while (DwCheque.RowCount > 0)
                {
                    DwCheque.DeleteRow(0);
                    if (i++ > 500) break;
                }
            }

            //เงื่อนไขการ protected การคีย์เงิน
            if (cashType == "CHQ")
            {
                DwMain.Modify("deptslip_amt.Protect=1");
            }

            //เครียค่า WebService และเซฟ DataCache
            DwMain.SaveDataCache();
            DwItem.SaveDataCache();
            DwCheque.SaveDataCache();
            ta.Close();
        }

        #endregion

        protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            IsAutoDeptWith = checkBox1Before;
            CheckBox1.Checked = checkBox1Before;
        }
    }
}