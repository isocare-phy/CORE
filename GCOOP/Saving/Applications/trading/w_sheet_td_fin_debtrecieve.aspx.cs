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
using CoreSavingLibrary.WcfReport;
using Sybase.DataWindow;
using DataLibrary;
using CoreSavingLibrary.WcfNTrading;


namespace Saving.Applications.trading
{
    public partial class w_sheet_td_fin_debtrecieve : PageWebSheet, WebSheet
    {
        //
        protected string app = "";
        protected string gid = "";
        protected string rid = "";
        protected string pdf = "";
        //


        string pbl = "sheet_td_fin_debtrecieve.pbl";
        private string sliptype_code = "ARRC";
        private DwThDate tDwMain;
        private DwThDate tDwDetail;


        protected String jsSlipNo;
        protected String jsDwDetailInsertRow;
        protected String jsInitDebt;
        protected String jsInitCreditSlip;
        protected String jspay_cal;
        protected String jsPostAccountNo;//เพิ่มใหม่
        protected String jsdiscount_amt;
        protected String jstax_amt;
        protected String jstransportfee;
        protected String jspaymentby;

        private n_tradingClient tradingService;

        public void InitJsPostBack()
        {
            tDwMain = new DwThDate(DwMain);
            tDwMain.Add("slip_date", "slip_tdate");            
            tDwMain.Add("debtintdoc_date", "debtintdoc_tdate");
            tDwMain.Add("creditdoc_date", "creditdoc_tdate");

            tDwDetail = new DwThDate(DwDetail);
            tDwDetail.Add("slip_date", "slip_tdate");
            tDwDetail.Add("due_date", "due_tdate");
            tDwDetail.Add("operate_date", "operate_tdate");

            jsSlipNo = WebUtil.JsPostBack(this, "jsSlipNo");
            jsDwDetailInsertRow = WebUtil.JsPostBack(this, "jsDwDetailInsertRow");
            jsInitDebt = WebUtil.JsPostBack(this, "jsInitDebt");
            jsInitCreditSlip = WebUtil.JsPostBack(this, "jsInitCreditSlip");
            jspay_cal = WebUtil.JsPostBack(this, "jspay_cal");
            //jsFilterBranch = WebUtil.JsPostBack(this, "jsFilterBranch");
            //jsFilterAccNo = WebUtil.JsPostBack(this, "jsFilterAccNo");
            jsPostAccountNo = WebUtil.JsPostBack(this, "jsPostAccountNo");//เพิ่มใหม่
            jsdiscount_amt = WebUtil.JsPostBack(this, "jsdiscount_amt");
            jstax_amt = WebUtil.JsPostBack(this, "jstax_amt");
            jstransportfee = WebUtil.JsPostBack(this, "jstransportfee");
            jspaymentby = WebUtil.JsPostBack(this, "jspaymentby");
            

            DwUtil.RetrieveDDDW(DwDetail, "paymentby", pbl, null);
        }

        public void WebSheetLoadBegin()
        {
            HdOpenIFrame.Value = "False";
            try
            {
                tradingService = wcf.NTrading;
            }
            catch
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถติดต่อ Service ได้");
                return;
            }
            if (!IsPostBack)
            {
                DwMain.InsertRow(0);
                DwMain.SetItemString(1, "coop_id", state.SsCoopId);
                DwMain.SetItemString(1, "entry_id", state.SsUsername);
                DwMain.SetItemString(1, "sliptype_code", sliptype_code);
                DwMain.SetItemDateTime(1, "slip_date", state.SsWorkDate);
                DwMain.SetItemDateTime(1, "debtintdoc_date", state.SsWorkDate);
                DwMain.SetItemString(1, "debtdectype_code", sliptype_code);
                               
            }
            else
            {
                this.RestoreContextDw(DwMain, tDwMain);
                this.RestoreContextDw(DwDetail, tDwDetail);
              
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            switch (eventArg)
            {
                case "jsSlipNo":
                    initMain();
                    //payment();
                    break;
                case "jsDwDetailInsertRow":
                    DwDetailInsertRow();
                    break;
                case "jsInitDebt":
                    InitDebt();
                    break;
                case "jsInitCreditSlip":
                    InitCreditSlip();
                    break;
                case "jspay_cal":
                    Paycal();
                    break;
                case "jsPostAccountNo"://เพิ่มใหม่
                    JsPostAccountNo();
                    break;
                    //
                case "popupReport":
                    PopupReport();
                    break;
                    //

                case "jsdiscount_amt":
                    Paycal();
                    break;
                case "jstax_amt":
                    Paycal();
                    break;
                case "jstransportfee":
                    Paycal();
                    break;
                case "jspaymentby":
                    payment();
                    break;
                    
                    

            }
        }

        public void SaveWebSheet()
        {
            try
            {
                string posttovc_flag = "";//
                string debtdecdocno = DwMain.GetItemString(1, "debtdecdoc_no");//
                string Sql = "select s.posttovc_flag from tdstockslip s, tddebtdecdet d where s.slip_no = d.refdoc_no and d.debtdecdoc_no ='" +debtdecdocno+ "' and s.posttovc_flag ='1'";//
                Sdt dt = WebUtil.QuerySdt(Sql);//

                if (dt.Next())//
                {
                    posttovc_flag = dt.GetString("posttovc_flag");//
                }

                if (posttovc_flag != "1")//
                {


                    str_tradesrv_oper astr_tradwsrv_req = new str_tradesrv_oper();
                    astr_tradwsrv_req.xml_header = DwMain.Describe("DataWindow.Data.XML");
                    astr_tradwsrv_req.xml_detail = DwDetail.Describe("DataWindow.Data.XML");

                    Int16 result = tradingService.of_save_op_slip(state.SsWsPass, ref astr_tradwsrv_req);
                    if (result == 1)
                    {
                        if (astr_tradwsrv_req.xml_header != null && astr_tradwsrv_req.xml_header != "")
                        {
                            DwMain.Reset();
                            DwUtil.ImportData(astr_tradwsrv_req.xml_header, DwMain, tDwMain, FileSaveAsType.Xml);
                        }
                      
                            if (astr_tradwsrv_req.xml_detail != null && astr_tradwsrv_req.xml_detail != "")
                            {
                                DwDetail.Reset();
                                DwUtil.ImportData(astr_tradwsrv_req.xml_detail, DwDetail, null, FileSaveAsType.Xml);
                            }
                        
                        LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
                        RunProcessDetail();
                    }
                    else
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage("เกิดข้อผิดพลาด ไม่สามารถบันทึกได้");
                    }
                }
                else
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถบันทึกได้ รายการนี้ไปยังฝ่ายบัญชีแล้ว");
                }
            }

            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        public void WebSheetLoadEnd()
        {

            try
            {
                DwUtil.RetrieveDDDW(DwDetail, "paymentby", pbl, null);
//                DwUtil.RetrieveDDDW(DwDetail, "bank_code", pbl, null);
                DwUtil.RetrieveDDDW(DwDetail, "account_no", pbl, null);
                //////
                try
                {
                    
                    string paymentby = DwDetail.GetItemString(1, "paymentby");
                    if (paymentby == "CHQ" || paymentby == "TNA")
                    {
                        DwDetail.Modify("account_no.Visible = 0");
                        DwDetail.Modify("account_no_1.Visible = 1");

                        //DwDetail.GetItemString(row, "account_no");
                    }
                }
                catch { }

            }
            catch { }
            tDwMain.Eng2ThaiAllRow();
            tDwDetail.Eng2ThaiAllRow();
            DwMain.SaveDataCache();
            DwDetail.SaveDataCache();
        }
        public void initMain()
        {
            try
            {
                str_tradesrv_oper astr_tradwsrv_req = new str_tradesrv_oper();
                astr_tradwsrv_req.xml_header = DwMain.Describe("DataWindow.Data.XML");
                astr_tradwsrv_req.xml_detail = DwDetail.Describe("DataWindow.Data.XML");

                Int16 result = tradingService.of_init_op_slip(state.SsWsPass, ref astr_tradwsrv_req);
                if (result == 1)
                {
                    if (astr_tradwsrv_req.xml_header != null && astr_tradwsrv_req.xml_header != "")
                    {
                        DwMain.Reset();
                        DwUtil.ImportData(astr_tradwsrv_req.xml_header, DwMain, tDwMain, FileSaveAsType.Xml);
                    }
                    if (astr_tradwsrv_req.xml_detail != null && astr_tradwsrv_req.xml_detail != "")
                    {
                        DwDetail.Reset();
                        DwUtil.ImportData(astr_tradwsrv_req.xml_detail, DwDetail, tDwDetail, FileSaveAsType.Xml);
                    }


                }
                else
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("เกิดข้อผิดพลาด ไม่สามารถทำรายการได้");
                }

                
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("public void initMain()" + ex);
            }
        }

        public void InitDebt()
        {
            try
            {
                str_tradesrv_oper astr_tradwsrv_req = new str_tradesrv_oper();
                astr_tradwsrv_req.xml_header = DwMain.Describe("DataWindow.Data.XML");
                Int16 result = tradingService.of_init_info_debt(state.SsWsPass, ref astr_tradwsrv_req);
                if (result == 1)
                {
                    DwMain.Reset();
                    DwUtil.ImportData(astr_tradwsrv_req.xml_header, DwMain, tDwMain, FileSaveAsType.Xml);
                }
                else
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถดึงข้อมูลได้");
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        private void DwDetailInsertRow()
        {
            try
            {
                int row = DwDetail.InsertRow(0);
                DwDetail.SetItemString(row, "coop_id", state.SsCoopId);
                DwDetail.SetItemDecimal(row, "seq_no", row);
                DwDetail.SetItemDateTime(row, "operate_date", state.SsWorkDate);
                //  DwDetail.SetItemString(row, "store_id", DwMain.GetItemString(1, "store_id"));
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        private void InitCreditSlip()
        {
            try
            {
                str_tradesrv_oper astr_tradwsrv_req = new str_tradesrv_oper();
                astr_tradwsrv_req.xml_header = DwMain.Describe("DataWindow.Data.XML");
                astr_tradwsrv_req.xml_detail = DwDetail.Describe("DataWindow.Data.XML");
                //astr_tradwsrv_req.xml_list = "APC";                   
                try
                {
                    Int16 result = tradingService.of_init_fin_arcredit(state.SsWsPass, ref astr_tradwsrv_req);
                    if (result == 1)
                    {
                        if (astr_tradwsrv_req.xml_header != null && astr_tradwsrv_req.xml_header != "")
                        {
                            DwMain.Reset();
                            DwUtil.ImportData(astr_tradwsrv_req.xml_header, DwMain, tDwMain, FileSaveAsType.Xml);
                        }
                        if (astr_tradwsrv_req.xml_detail != null && astr_tradwsrv_req.xml_detail != "")
                        {
                            DwDetail.Reset();
                            DwUtil.ImportData(astr_tradwsrv_req.xml_detail, DwDetail, tDwDetail, FileSaveAsType.Xml);
                        }
                    }
                    else
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage("เกิดข้อผิดพลาด ไม่สามารถทำรายการได้");
                    }
                }
                catch
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("เกิดข้อผิดพลาด ไม่สามารถทำรายการได้");
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("private void InitCreditSlip()" + ex);
            }
        }
        private void Paycal()
        {

            int row = Convert.ToInt32(HdRow.Value);
            HdRow.Value = "";

            //decimal payAmt = DwDetail.GetItemDecimal(row, "pay_amt");
            //decimal debtAmt = DwDetail.GetItemDecimal(row, "debt_amt");
            //decimal sum = debtAmt - payAmt;
            //DwDetail.SetItemDecimal(row, "debtbalance_amt", sum);
            
            //เพิ่ม    

            decimal pay_amt = 0;
            decimal discount_amt = 0;
            decimal tax_amt = 0;
            decimal transportfee = 0;
            decimal tot_amt = 0;
            decimal sum = 0;
            decimal total = 0;
                        
            String Sql = @"select amount from tdconstant" ; 
            Sdt dt = WebUtil.QuerySdt(Sql);
            if (dt.Next())
            {
                sum = dt.GetDecimal("amount");
            }
            
          try
            {
                try
                {
                    pay_amt = DwDetail.GetItemDecimal(row, "pay_amt");
                }
                catch { pay_amt = 0; }
                try
                {
                    discount_amt = DwDetail.GetItemDecimal(row, "discount_amt");
                }
                catch { discount_amt = 0; }
                try
                {
                    tax_amt = DwDetail.GetItemDecimal(row, "tax_amt");
                }
                catch { tax_amt = 0; }
                try
                {
                    transportfee = DwDetail.GetItemDecimal(row, "transportfee");
                }
                catch { transportfee = 0; }
                try
                {
                    tot_amt = DwDetail.GetItemDecimal(row, "tot_amt");
                }
                catch { tot_amt = 0; }
                //sum = ((((pay_amt + discount_amt) - tax_amt) - transportfee) - tot_amt);

                total = ((((pay_amt + discount_amt) - tax_amt) - transportfee) - tot_amt);

                if (total < 0 && total > (sum*-1))        //sum < 10 && sum > 0  //0,-10
                {
                    //DwDetail.SetItemDecimal(row, "lesspay_amt", sum);
                    DwDetail.SetItemDecimal(row, "lesspay_amt", total * -1);
                    DwDetail.SetItemDecimal(row, "overpay_amt", 0);
                    DwDetail.SetItemDecimal(row, "debtforward_amt", 0);
                    DwDetail.SetItemDecimal(row, "avpay_amt", 0);
                }
                else if (total <= (sum * -1))         //sum <=  - 10   //-10
                {
                    //DwDetail.SetItemDecimal(row, "overpay_amt", sum * -1);
                    DwDetail.SetItemDecimal(row, "debtforward_amt", total * -1); 
                    DwDetail.SetItemDecimal(row, "avpay_amt", 0);
                    DwDetail.SetItemDecimal(row, "overpay_amt", 0);
                    DwDetail.SetItemDecimal(row, "lesspay_amt", 0);
                }
                else if (total < sum && total > 0)          //sum < 0 && sum > -10  //10 , 0
                {
                    DwDetail.SetItemDecimal(row, "overpay_amt", total);
                    DwDetail.SetItemDecimal(row, "debtforward_amt", 0);
                    DwDetail.SetItemDecimal(row, "avpay_amt", 0);
                    DwDetail.SetItemDecimal(row, "lesspay_amt", 0);
                }

                else if (total >= sum)          
                 {
                     DwDetail.SetItemDecimal(row, "overpay_amt", 0);
                     DwDetail.SetItemDecimal(row, "debtforward_amt", 0);
                     DwDetail.SetItemDecimal(row, "avpay_amt", total);
                     DwDetail.SetItemDecimal(row, "lesspay_amt", 0);
                 }
                              
                else
                {

                    DwDetail.SetItemDecimal(row, "lesspay_amt", 0);
                    DwDetail.SetItemDecimal(row, "overpay_amt", 0);
                    DwDetail.SetItemDecimal(row, "avpay_amt", 0);
                    DwDetail.SetItemDecimal(row, "debtforward_amt", 0);
                }                
            }
            catch
            { }
            /////////////////
                                    
            decimal sum_payAmt = 0;
            int RowCount = DwDetail.RowCount;
            for (int i = 1; i <= RowCount; i++)
            {
                //ของเดิม//decimal payAmt1 = DwDetail.GetItemDecimal(i, "debtbalance_amt");
                //sum_payAmt = sum_payAmt + payAmt1; //ของเดิม

                decimal payAmt1 = DwDetail.GetItemDecimal(i, "pay_amt");
                sum_payAmt = sum_payAmt + payAmt1;

            }

            DwMain.SetItemDecimal(1, "debtdec_amt", sum_payAmt);
        }

     
        private void JsPostAccountNo()
        {
            try
            {
                int row = Convert.ToInt32(HdRow.Value);
                string account_no = DwDetail.GetItemString(row, "account_no");  
                DataWindowChild dc = DwDetail.GetChild("account_no");
                int rGroup = dc.FindRow("account_no='" + account_no + "'", row , dc.RowCount);
                string bank_code = DwUtil.GetString(dc, rGroup, "bank_code", "");
                string bankbranch_code = DwUtil.GetString(dc, rGroup, "bankbranch_code", "");
                string account_id = DwUtil.GetString(dc, rGroup, "account_id", "");

                //DwDetail.SetItemString(row, "bank_code", bank_code);
                //DwDetail.SetItemString(row, "bank_branch", bankbranch_code);
                //DwDetail.SetItemString(row, "account_no", account_no);

                ////////// add////////////
                string paymentby = DwDetail.GetItemString(row, "paymentby");

                if (paymentby == "CHQ" || paymentby == "TNA")
                {
                    string account_no_1 = DwDetail.GetItemString(row, "account_no_1");
                    DwDetail.SetItemString(row, "account_no", account_no_1);
                    DwDetail.SetItemString(row, "bank_code", "");
                    DwDetail.SetItemString(row, "bank_branch", "");

                    DwDetail.Modify("account_no.Visible = 0");
                    DwDetail.Modify("account_no_1.Visible = 1");

                }
                else
                {
                    DwDetail.SetItemString(row, "bank_code", bank_code);
                    DwDetail.SetItemString(row, "bank_branch", bankbranch_code);
                    DwDetail.SetItemString(row, "account_no", account_no);

                }
                

                
            }
            catch { }
        }

       
        protected void payment()
        {
            try
            {
             
                int row = Convert.ToInt32(HdRow.Value);
            
                string paymentby = DwDetail.GetItemString(row, "paymentby");
               
                if (paymentby == "CHQ" || paymentby == "TNA")
                {
                                        
                    DwDetail.Modify("account_no.Visible = 0");
                    DwDetail.Modify("account_no_1.Visible = 1");

                    //DwDetail.GetItemString(row, "account");
                    string account_no = "";
                    DwDetail.SetItemString(row, "account_no", account_no);
                    DwDetail.SetItemString(row, "account_no_1", account_no);


                    
                }
                              
              
            }
            catch
            {
            
            }

        }


        #region printreport
        private void RunProcessDetail()
        {

            String print_id = "DT_FORM"; //รายงานตัวนี้ไม่มีใน webreportdetail นะรี่  by pat
            String branchId = state.SsCoopId;
            String debtdecdoc_no = DwMain.GetItemString(1, "debtdecdoc_no");
            //decimal seq_no = Convert.ToInt16(HdSeqNo.Value);
            //String seqNo = DwChq.GetItemString(1, "seq_no");//ตัวนี้ ใน DwChq ไม่มีนะ seq_no by pat

            app = state.SsApplication;
            gid = "TRADING_FORM";
            rid = print_id;
            ReportHelper lnv_helper = new ReportHelper();
            // lnv_helper.AddArgument(branchId, ArgumentType.String);
            lnv_helper.AddArgument(debtdecdoc_no, ArgumentType.String);
           // lnv_helper.AddArgument(debtdecdoc_no, ArgumentType.String);
            //lnv_helper.AddArgument(seqNo, ArgumentType.String);//ไม่รู้ว่า ตัวนี้เอาไปใช้ด้วยแน่หรือป่าว ถ้าใช้มันคือเลขอะไร บอกพี่อีกทีละกัน by pat


            //****************************************************************
            //ชื่อไฟล์ PDF = YYYYMMDDHHMMSS_<GID>_<RID>.PDF
            String pdfFileName = DateTime.Now.ToString("yyyyMMddHHmmss", WebUtil.EN);
            pdfFileName += "_" + gid + "_" + rid + ".pdf";
            pdfFileName = pdfFileName.Trim();
            //ส่งให้ ReportService สร้าง PDF ให้ {โดยปกติจะอยู่ใน C:\GCOOP\Saving\PDF\}.
            try
            {
                ReportClient lws_report = wcf.Report;
                String criteriaXML = lnv_helper.PopArgumentsXML();
                this.pdf = lws_report.GetPDFURL(state.SsWsPass) + pdfFileName;
                // String li_return = lws_report.RunWithID(state.SsWsPass, app, gid, rid, state.SsUsername, criteriaXML, pdfFileName);
                String li_return = lws_report.RunWithID(state.SsWsPass, app, gid, rid, state.SsUsername, criteriaXML, pdfFileName);
                if (li_return == "true")
                {
                    HdOpenIFrame.Value = "True";
                }

            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                return;
            }
        }



        public void PopupReport()
        {
            //เด้ง Popup ออกรายงานเป็น PDF.
            String pop = "Gcoop.OpenPopup('" + pdf + "')";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "DsReport", pop, true);
        }

        #endregion

    }
}