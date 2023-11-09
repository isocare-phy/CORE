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
using CoreSavingLibrary.WcfCommon;
using CoreSavingLibrary.WcfReport;
using Sybase.DataWindow;
using DataLibrary;
using CoreSavingLibrary.WcfTrading;
using System.Globalization;

namespace Saving.Applications.trading
{
    public partial class w_sheet_td_fin_credrecieve : PageWebSheet, WebSheet
    {
        protected string app = "";
        protected string gid = "";
        protected string rid = "";
        protected string pdf = "";


        private string sliptype_code = "APRC";
        string pbl = "sheet_td_fin_credrecieve.pbl";
        string debtdecdoc_no = "";
        // รับวันที่
        private DwThDate tDwMain;
        private DwThDate tDwDetail;


        protected String jsSlipNo;
        protected String jsDwDetailInsertRow;
        protected String jsInitCred;
        protected String jsInitCreditSlip;
        protected String jspay_cal;
        protected String jsFilterBranch;
        protected String jsFilterAccNo;
        protected String jsChickChkAll;
        protected String jsChickChk;
        protected String jsCalTax;
        protected String jsCalTax2;

        //add//
        protected String jsbtn;
        protected String jsbtn_2;

        //ประกาศเรียก service
        private TradingClient tradingService;

        public void InitJsPostBack()
        {

            tDwMain = new DwThDate(DwMain);
            tDwMain.Add("debtintdoc_date", "debtintdoc_tdate");
            tDwMain.Add("slip_date", "slip_tdate");
            tDwMain.Add("debtintdoc_date", "debtintdoc_tdate");
            tDwMain.Add("operate_date", "operate_tdate");
            


            tDwDetail = new DwThDate(DwDetail);
            tDwDetail.Add("slip_date", "slip_tdate");
            tDwDetail.Add("due_date", "due_tdate");
            

            jsCalTax = WebUtil.JsPostBack(this, "jsCalTax");
            jsCalTax2 = WebUtil.JsPostBack(this, "jsCalTax2");
            jsInitCred = WebUtil.JsPostBack(this, "jsInitCred");
            jsSlipNo = WebUtil.JsPostBack(this, "jsSlipNo");
            jsDwDetailInsertRow = WebUtil.JsPostBack(this, "jsDwDetailInsertRow");
            jsChickChkAll = WebUtil.JsPostBack(this, "jsChickChkAll");
            jsChickChk = WebUtil.JsPostBack(this, "jsChickChk");
            jspay_cal = WebUtil.JsPostBack(this, "jspay_cal");
            jsFilterBranch = WebUtil.JsPostBack(this, "jsFilterBranch");
            jsFilterAccNo = WebUtil.JsPostBack(this, "jsFilterAccNo");

            //add
            jsbtn = WebUtil.JsPostBack(this, "jsbtn");
            jsbtn_2 = WebUtil.JsPostBack(this, "jsbtn_2");
            
            
         //   DwUtil.RetrieveDDDW(DwDetail, "paymentby", "sheet_td_fin_credrecieve.pbl", null);
        }

        public void WebSheetLoadBegin()
        {
            //DwUtil.RetrieveDDDW(DwDetail, "bank_branch_1", pbl, null);
            //this.c
            try
            {
                try
                {
                    tradingService = wcf.Trading;
                }
                catch
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถติดต่อ Service ได้");
                    return;
                }
                if (!IsPostBack)
                {
                    DwMain.InsertRow(0);
                    DwTailer.InsertRow(0);
                    DwPrepay.InsertRow(0);
                    DwMain.SetItemString(1, "coop_id", state.SsCoopId);
                    DwMain.SetItemString(1, "entry_id", state.SsUsername);
                    DwMain.SetItemDateTime(1, "slip_date", state.SsWorkDate);
                    DwMain.SetItemDateTime(1, "debtintdoc_date", state.SsWorkDate);
                    DwMain.SetItemDateTime(1, "creditdoc_tdate", state.SsWorkDate);
                    //DwMain.SetItemDateTime(1, "operate_tdate", state.SsWorkDate);
                    

                }
                else
                {
                    this.RestoreContextDw(DwMain, tDwMain);
                    this.RestoreContextDw(DwDetail, tDwDetail);
                    this.RestoreContextDw(DwTailer);
                    this.RestoreContextDw(DwPrepay);
                }
            }
            catch { } 
        }

        public void CheckJsPostBack(string eventArg)
        {
            switch (eventArg)
            {
                case "jsSlipNo":
                    initMain();
                    break;
                case "jsDwDetailInsertRow":
                    DwDetailInsertRow();
                    break;
                case "jsInitCred":
                    InitCred();
                    break;
                case "jspay_cal":
                    Paycal();
                    break;
                case "jsFilterBranch":
                    FilterBranch();
                    break;
                case "jsFilterAccNo":
                    FilterAccNo();
                    break;
                case "jsChickChkAll":
                    ChickChkAll();
                    break;
                case "jsCalTax":
                    calTax();
                    //calTax2();
                    break;
                case "jsCalTax2":
                    calTax();
                    //calTax2();
                    break;
                case "jsChickChk":
                    ChickChk();
                    break;
                case "popupReport":
                    PopupReport();
                    break;


                    //add
                case "jsbtn":
                    RunIReport();
                    break;
                case "jsbtn_2":
                    RunIIReport();
                    break;
                    

            }
        }

        #region printreport
        private void RunProcessDetail()
        {

            String print_id = "ST_FORM"; //รายงานตัวนี้ไม่มีใน webreportdetail นะรี่  by pat
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
            lnv_helper.AddArgument(debtdecdoc_no, ArgumentType.String);
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



        public void SaveWebSheet()
        {
            String operate_date = "",toapproval_desc = "", fromrequest_desc = "", payment_desc1 = "", payment_desc2 = "", payment_desc3 = "", debtdecdoc_no = "";
            String remark1 = "", remark2 = "", remark3 = "", remark4 = "", remark5 = "", remark6 = "", remark7 = "", sqlupdate = "";
            Decimal slipnet_amt = 0;
            setDataTailerToDwMain();
            try
            {
                //////add///
                string posttovc_flag = "";//
                string debtdecdocno = DwMain.GetItemString(1, "debtdecdoc_no");//
                string Sql = "select s.posttovc_flag from tdstockslip s, tddebtdecdet d where s.slip_no = d.refdoc_no and d.debtdecdoc_no ='" +debtdecdocno+ "' and s.posttovc_flag ='1'";//
                Sdt dt = WebUtil.QuerySdt(Sql);

                if (dt.Next())//
                {
                    posttovc_flag = dt.GetString("posttovc_flag");//
                }

                if (posttovc_flag != "1")//
                {



                    str_tradesrv_oper astr_tradwsrv_req = new str_tradesrv_oper();
                    DwMain.SetItemString(1, "sliptype_code", "APRC");
                    astr_tradwsrv_req.xml_header = DwMain.Describe("DataWindow.Data.XML");
                    astr_tradwsrv_req.xml_detail = DwDetail.Describe("DataWindow.Data.XML");
                    astr_tradwsrv_req.xml_tailer = DwTailer.Describe("DataWindow.Data.XML");


                    Int16 result = tradingService.of_save_op_slip(state.SsWsPass, ref astr_tradwsrv_req);

                    debtdecdoc_no = DwMain.GetItemString(1, "debtdecdoc_no");
                    try
                    {
                        try { operate_date = DwPrepay.GetItemString(1, "tddebtdec_remark1"); }
                        catch { toapproval_desc = ""; }
                        try { toapproval_desc = DwPrepay.GetItemString(1, "tddebtdec_toapproval_desc"); }
                        catch { toapproval_desc = ""; }
                        try { fromrequest_desc = DwPrepay.GetItemString(1, "tddebtdec_fromrequest_desc"); }
                        catch { }
                        try { payment_desc1 = DwPrepay.GetItemString(1, "tddebtdec_payment_desc1"); }
                        catch { payment_desc1 = ""; }
                        try { payment_desc2 = DwPrepay.GetItemString(1, "tddebtdec_payment_desc2"); }
                        catch { payment_desc2 = ""; }
                        try { payment_desc3 = DwPrepay.GetItemString(1, "tddebtdec_payment_desc3"); }
                        catch { payment_desc3 = ""; }
                        try { slipnet_amt = DwPrepay.GetItemDecimal(1, "tddebtdec_slipnet_amt"); }
                        catch { }
                        try { remark1 = DwPrepay.GetItemString(1, "tddebtdec_remark1"); }
                        catch { }
                        try { remark2 = DwPrepay.GetItemString(1, "tddebtdec_remark2"); }
                        catch { }
                        try { remark3 = DwPrepay.GetItemString(1, "tddebtdec_remark3"); }
                        catch { }
                        try { remark4 = DwPrepay.GetItemString(1, "tddebtdec_remark4"); }
                        catch { }
                        try { remark5 = DwPrepay.GetItemString(1, "tddebtdec_remark5"); }
                        catch { }
                        try { remark6 = DwPrepay.GetItemString(1, "tddebtdec_remark6"); }
                        catch { }
                        try { remark7 = DwPrepay.GetItemString(1, "tddebtdec_remark7"); }
                        catch { }
                        try
                        {
                            String[] tmpdate ;
                            String newstartdate ="";
                            try
                            {
                                tmpdate = operate_date.Split('/');
                                newstartdate = tmpdate[0] + "/" + tmpdate[1] + "/" + (Convert.ToDecimal(tmpdate[2]) - 543);
                            }
                            catch { }
                            //DateTime operate_date_1 = DateTime.ParseExact(operate_date, "dd/MM/yyyy", new CultureInfo("en-US"));
                            //string op = WebUtil.ConvertDateThaiToEng(null, null, operate_date);//("dd/mm/yyyy HH:mm:ss");
                            //string date = Convert.ToString(string.Format("dd/MM/yyyy",da));
                            //DateTime date = DateTime.ParseExact(da, "dd/MM/yyyy", null);
                            //DateTime dd = Convert.ToDateTime(operate_date);
                            //string dd2 = String.Format("{0:d/M/yyyy HH:mm:ss}", da);
                            //DateTime op = Convert.ToDateTime(operate_date);
                            sqlupdate = @"UPDATE TDDEBTDEC SET OPERATE_DATE=to_date('" + newstartdate + "','dd/MM/yyyy'),TOAPPROVAL_DESC='" + toapproval_desc + "',FROMREQUEST_DESC='" + fromrequest_desc + "', PAYMENT_DESC1='" + payment_desc1 +
                                "',PAYMENT_DESC2='" + payment_desc2 + "', PAYMENT_DESC3='" + payment_desc3 + "', REMARK1='" + remark1 + "', REMARK2='" + remark2 + "', REMARK3='" + remark3 +
                                "', REMARK4='" + remark4 + "', REMARK5='" + remark5 + "', REMARK6='" + remark6 + "', REMARK7='" + remark7 + "' WHERE debtdecdoc_no ='" + debtdecdoc_no + "' ";
                            //Sdt update = WebUtil.QuerySdt(sqlupdate);
                            Sdt update = WebUtil.QuerySdt(sqlupdate);

                        }
                        catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }

                    }
                    catch (Exception ex)
                    { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }



                    if (result == 1)
                    {
                        DwMain.Reset();
                        DwDetail.Reset();
                        DwTailer.Reset();
                        DwPrepay.Reset();
                        DwUtil.ImportData(astr_tradwsrv_req.xml_header, DwMain, tDwMain, FileSaveAsType.Xml);
                        DwUtil.ImportData(astr_tradwsrv_req.xml_detail, DwDetail, null, FileSaveAsType.Xml);
                        DwUtil.ImportData(astr_tradwsrv_req.xml_tailer, DwTailer, null, FileSaveAsType.Xml);
                        //
                        //DwUtil.ImportData(astr_tradwsrv_req.xml_prepay, DwPrepay, null, FileSaveAsType.Xml);
                        //


                        //add//
                        string debtdecdoc_no_2 = DwMain.GetItemString(1, "debtdecdoc_no");

                        if (debtdecdoc_no != debtdecdoc_no_2)
                        {
                            try
                            {
                                String[] tmpdate;
                                String newstartdate = "";
                                try
                                {
                                    tmpdate = operate_date.Split('/');
                                    newstartdate = tmpdate[0] + "/" + tmpdate[1] + "/" + (Convert.ToDecimal(tmpdate[2]) - 543);
                                }
                                catch { }
                                sqlupdate = @"UPDATE TDDEBTDEC SET OPERATE_DATE=to_date('" + newstartdate + "','dd/MM/yyyy'),TOAPPROVAL_DESC='" + toapproval_desc + "',FROMREQUEST_DESC='" + fromrequest_desc + "', PAYMENT_DESC1='" + payment_desc1 +
                                    "',PAYMENT_DESC2='" + payment_desc2 + "', PAYMENT_DESC3='" + payment_desc3 + "', REMARK1='" + remark1 + "', REMARK2='" + remark2 + "', REMARK3='" + remark3 +
                                    "', REMARK4='" + remark4 + "', REMARK5='" + remark5 + "', REMARK6='" + remark6 + "', REMARK7='" + remark7 + "' WHERE debtdecdoc_no ='" + debtdecdoc_no_2 + "' ";
                                Sdt update = WebUtil.QuerySdt(sqlupdate);

                            }
                            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
                        }
                        //
                        DwMain.ShareData(DwPrepay);
                        LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");

                        // RunProcessDetail();


                        // DwMain.Reset();
                        // DwDetail.Reset();
                        // DwTailer.Reset();
                        // DwUtil.ImportData(astr_tradwsrv_req.xml_header, DwMain, tDwMain, FileSaveAsType.Xml);
                        // DwUtil.ImportData(astr_tradwsrv_req.xml_detail, DwDetail, null, FileSaveAsType.Xml);
                        // DwUtil.ImportData(astr_tradwsrv_req.xml_tailer, DwTailer, null, FileSaveAsType.Xml);



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
                DwUtil.RetrieveDDDW(DwMain, "paymentby", pbl, null);
                DwUtil.RetrieveDDDW(DwTailer, "taxtype_code", pbl, null);
                DwMain.ShareData(DwPrepay);
               // DwMain.ShareData(DwTailer);

            }
            catch { }

            tDwMain.Eng2ThaiAllRow();
            tDwDetail.Eng2ThaiAllRow();

            DwMain.SaveDataCache();
            DwDetail.SaveDataCache();
            DwTailer.SaveDataCache();
            DwPrepay.SaveDataCache();


            try
            {
                string debtdecdoc_no = DwMain.GetItemString(1, "debtdecdoc_no");
                if (debtdecdoc_no == "Auto")
                {
                    DwMain.Modify("btn_01.VIsible=0");
                    DwMain.Modify("btn_02.VIsible=0");
                }
                else
                {
                    DwMain.Modify("btn_01.VIsible=1");
                    DwMain.Modify("btn_02.VIsible=1");
                }
                
            }
            catch 
            {
                DwMain.Modify("btn_01.VIsible=0");
                DwMain.Modify("btn_02.VIsible=0");
            }
           

        }
        // ใบตั้งหนี้ 3
        private void InitCreditSlip()
        {
            try
            {
                str_tradesrv_oper astr_tradwsrv_req = new str_tradesrv_oper();
                astr_tradwsrv_req.xml_header = DwMain.Describe("DataWindow.Data.XML");
                astr_tradwsrv_req.xml_detail = DwDetail.Describe("DataWindow.Data.XML");
                astr_tradwsrv_req.xml_tailer = DwTailer.Describe("DataWindow.Data.XML");
                try
                {
                    Int16 result = tradingService.of_init_fin_apcredit(state.SsWsPass, ref astr_tradwsrv_req);
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
                        if (astr_tradwsrv_req.xml_tailer != null && astr_tradwsrv_req.xml_tailer != "")
                        {
                            DwTailer.Reset();
                            DwUtil.ImportData(astr_tradwsrv_req.xml_tailer, DwDetail, tDwDetail, FileSaveAsType.Xml);
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

        // init เจ้าหนี้ 2
        private void InitCred()
        {
            try
            {
                str_tradesrv_oper astr_tradwsrv_req = new str_tradesrv_oper();
                astr_tradwsrv_req.xml_header = DwMain.Describe("DataWindow.Data.XML");
                astr_tradwsrv_req.xml_detail = DwDetail.Describe("DataWindow.Data.XML");
                astr_tradwsrv_req.xml_tailer = DwTailer.Describe("DataWindow.Data.XML");

                //                Int16 result = tradingService.of_init_info_cred(state.SsWsPass, ref astr_tradwsrv_req);
                Int16 result = tradingService.of_init_fin_apcredit(state.SsWsPass, ref astr_tradwsrv_req);
                if (result == 1)
                {
                    if (astr_tradwsrv_req.xml_header != null && astr_tradwsrv_req.xml_header != "")
                    {
                        if (astr_tradwsrv_req.xml_detail != null && astr_tradwsrv_req.xml_detail != "")
                        {

                            DwMain.Reset();
                            DwDetail.Reset();
                            DwTailer.Reset();
                            DwUtil.ImportData(astr_tradwsrv_req.xml_header, DwMain, tDwMain, FileSaveAsType.Xml);
                            DwUtil.ImportData(astr_tradwsrv_req.xml_detail, DwDetail, tDwDetail, FileSaveAsType.Xml);
                            DwUtil.ImportData(astr_tradwsrv_req.xml_tailer, DwTailer, null, FileSaveAsType.Xml);

                        }


                    }

                   

                }
                else
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("เกิดข้อผิดพลาด ไม่สามารถทำรายการได้");
                }
                
                ////////////////////////////////////
                //try
                //{

                //    decimal servicetax = 0;
                //    try
                //    {
                //        servicetax = DwTailer.GetItemDecimal(1, "servicetax");
                //    }
                //    catch { }
                //    decimal tax_rate = 0;
                //    try
                //    {
                //        tax_rate = DwTailer.GetItemDecimal(1, "tax_rate");
                //    }
                //    catch { }
                //    decimal debtdec_amt = 0;
                //    try
                //    {
                //        debtdec_amt = DwTailer.GetItemDecimal(1, "debtdec_amt");
                //    }
                //    catch { }
                //    decimal tax_amt = 0;
                //    try
                //    {
                //        tax_amt = DwTailer.GetItemDecimal(1, "tax_amt");
                //    }
                //    catch { }
                //    decimal servicetaxamt = 0;
                //    try
                //    {
                //        servicetaxamt = DwTailer.GetItemDecimal(1, "servicetaxamt");
                //    }
                //    catch { }
                //    decimal slipnet_amt = 0;
                //    try
                //    {
                //        slipnet_amt = DwTailer.GetItemDecimal(1, "slipnet_amt");
                //    }
                //    catch { }
                //    string operate_tdate = "";
                //    try
                //    {
                //        operate_tdate = DwMain.GetItemString(1, "operate_tdate");
                //    }
                //    catch { }
                //    DwPrepay.SetItemString(1, "tddebtdec_fromrequest_desc", "การขาย/ธุรกิจเสริม");
                //    //DwPrepay.SetItemDecimal(1, "tddebtdec_slipnet_amt", slipnet_amt);
                //    DwPrepay.SetItemString(1, "tddebtdec_remark1", "ขอรับเงินวันที่ " + operate_tdate);
                //    try
                //    {
                //        if (tax_rate != 0)
                //        {
                //            DwPrepay.SetItemString(1, "tddebtdec_payment_desc2", "จำนวนเงินทั้งสิ้น " + debtdec_amt.ToString("#,##0.00") + " บาท " + "VAT 7.00% เป็นเงิน " + tax_amt.ToString("#,##0.00") + " บาท");
                //        }
                //    }
                //    catch { }
                //    try
                //    {
                //        if (servicetax != 0)
                //        {
                //            DwPrepay.SetItemString(1, "tddebtdec_payment_desc3", "จำนวนเงินทั้งสิ้น " + slipnet_amt.ToString("#,##0.00") + " บาท " + "จำนวนเงิน " + debtdec_amt.ToString("#,##0.00") + " บาท" + "หักภาษี ณ ที่จ่าย 3.00% เป็นเงิน " + servicetaxamt.ToString("#,##0.00") + " บาท ");
                //        }
                //    }
                //    catch { }
                //}
                //catch (Exception ex)
                //{
                //    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                //}
                ///////////////////////////////////
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }

        }

        //init ชำระหนี้เดิม  1
        private void initMain()
        {
            try
            {
                str_tradesrv_oper astr_tradwsrv_req = new str_tradesrv_oper();
                astr_tradwsrv_req.xml_header = DwMain.Describe("DataWindow.Data.XML");
                astr_tradwsrv_req.xml_detail = DwDetail.Describe("DataWindow.Data.XML");
                astr_tradwsrv_req.xml_tailer = DwTailer.Describe("DataWindow.Data.XML");
                Int16 result = tradingService.of_init_op_slip(state.SsWsPass, ref astr_tradwsrv_req);

                if (result == 1)
                {
                    if (astr_tradwsrv_req.xml_header != null && astr_tradwsrv_req.xml_header != "")
                    {
                        DwMain.Reset();
                        DwUtil.ImportData(astr_tradwsrv_req.xml_header, DwMain, tDwMain, FileSaveAsType.Xml);
                    }
                    try
                    {
                        if (astr_tradwsrv_req.xml_detail != null && astr_tradwsrv_req.xml_detail != "")
                        {
                            DwDetail.Reset();
                            DwUtil.ImportData(astr_tradwsrv_req.xml_detail, DwDetail, tDwDetail, FileSaveAsType.Xml);
                        }
                    }
                    catch { }
                    try
                    {
                        if (astr_tradwsrv_req.xml_tailer != null && astr_tradwsrv_req.xml_tailer != "")
                        {
                            DwTailer.Reset();
                            DwUtil.ImportData(astr_tradwsrv_req.xml_tailer, DwTailer, null, FileSaveAsType.Xml);
                        }
                    }
                    catch { }
                    
                    
                    try
                    {
                        try
                        {
                            debtdecdoc_no = DwMain.GetItemString(1, "debtdecdoc_no");
                        }
                        catch { }
                        //DwUtil.RetrieveDataWindow(DwPrepay, pbl, null, state.SsCoopId, debtdecdoc_no);
                        //
                        decimal servicetax = 0;
                        try
                        {
                            servicetax = DwTailer.GetItemDecimal(1, "servicetax");
                        }
                        catch { } 
                        decimal tax_rate = 0;
                        try
                        {
                             tax_rate = DwTailer.GetItemDecimal(1, "tax_rate");
                        }
                        catch { }
                        decimal debtdec_amt = 0;
                        try
                        {
                            debtdec_amt = DwTailer.GetItemDecimal(1, "debtdec_amt");
                        }
                        catch { }
                        decimal tax_amt = 0;
                        try
                        {
                            tax_amt = DwTailer.GetItemDecimal(1, "tax_amt");
                        }
                        catch { }
                        decimal servicetaxamt = 0;
                        try
                        {
                            servicetaxamt = DwTailer.GetItemDecimal(1, "servicetaxamt");
                        }
                        catch { }
                         decimal slipnet_amt =0;
                        try
                        {
                            slipnet_amt = DwTailer.GetItemDecimal(1, "slipnet_amt");
                        }
                        catch { }
                        string operate_tdate = DwMain.GetItemString(1, "operate_tdate");
                        DwPrepay.SetItemString(1, "tddebtdec_fromrequest_desc", "การขาย/ธุรกิจเสริม");
                        DwPrepay.SetItemDecimal(1, "tddebtdec_slipnet_amt", slipnet_amt);
                        DwPrepay.SetItemString(1, "tddebtdec_remark1", "ขอรับเงินวันที่ " + operate_tdate);
                        try
                        {
                            if (tax_rate != 0)
                            {
                                DwPrepay.SetItemString(1, "tddebtdec_payment_desc2", "จำนวนเงินทั้งสิ้น " + debtdec_amt.ToString("#,##0.00") + " บาท " + "VAT 7.00% เป็นเงิน " + tax_amt.ToString("#,##0.00") + " บาท");
                            }
                        }
                        catch { }
                        try
                        {
                            if (servicetax != 0)
                            {
                                DwPrepay.SetItemString(1, "tddebtdec_payment_desc3", "จำนวนเงินทั้งสิ้น " + slipnet_amt.ToString("#,##0.00") + " บาท " + "จำนวนเงิน " + debtdec_amt.ToString("#,##0.00") + " บาท" + "หักภาษี ณ ที่จ่าย 3.00% เป็นเงิน " + servicetaxamt.ToString("#,##0.00") + " บาท ");
                            }
                        }
                        catch { }
                        
                        
                    }
                    catch (Exception ex) 
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                    }

                }
                else
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("เกิดข้อผิดพลาด ไม่สามารถทำรายการได้");
                }
                //DwUtil.RetrieveDDDW(DwMain, "paymentby", pbl, null);
                //DwUtil.RetrieveDDDW(DwTailer, "taxtype_code", pbl, null);
                //DwMain.ShareData(DwPrepay);
                DwMain.ShareData(DwTailer);
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("public void initMain()" + ex);
            }
        }

        // เพิ่มแถว
        private void DwDetailInsertRow()
        {
            try
            {
                int row = DwDetail.InsertRow(0);
                DwDetail.SetItemString(row, "coop_id", state.SsCoopId);
                DwDetail.SetItemDecimal(row, "seq_no", row);
                DwDetail.SetItemDateTime(row, "operate_date", state.SsWorkDate);
                //    DwDetail.SetItemString(row, "sliptype_code", sliptype_code);

            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }


        private void Paycal()
        {
            decimal sum_payAmt = 0;
            try
            {

                int row = Convert.ToInt32(HdRow.Value);
                HdRow.Value = "";

                decimal payAmt = DwDetail.GetItemDecimal(row, "pay_amt");
                decimal debtAmt = DwDetail.GetItemDecimal(row, "debt_amt");
                decimal sum = debtAmt - payAmt;
                DwDetail.SetItemDecimal(row, "debtbalance_amt", sum);
               
                int RowCount = DwDetail.RowCount;
                for (int i = 1; i <= RowCount; i++)
                {
                    decimal payAmt1 = DwDetail.GetItemDecimal(i, "debtbalance_amt");

                    sum_payAmt = sum_payAmt + payAmt1;
                }
            }
            catch
            { 
            }
            //เพิ่ม 561108
            //DwMain.SetItemDecimal(1, "debtdec_amt", sum_payAmt);
            DwTailer.SetItemDecimal(1, "debtdec_amt", sum_payAmt);
            calTax();
        }
        public void FilterBranch()
        {
            string BankCd = DwDetail.GetItemString(1, "bank_code");
            DwDetail.SetItemString(1, "bank_branch", "");
            DwUtil.RetrieveDDDW(DwDetail, "bank_branch", pbl, BankCd);
        }
        public void FilterAccNo()
        {
            try
            {
                string BankCd = DwDetail.GetItemString(1, "bank_code");
                string BranchCd = DwDetail.GetItemString(1, "bank_branch");
                DwDetail.SetItemString(1, "account_no", "");
                DwUtil.RetrieveDDDW(DwDetail, "account_no", pbl, BankCd, BranchCd);
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }
        private void ChickChkAll()
        {
            decimal debtbalance_amt = 0;
            try
            {
                int rowcount = DwDetail.RowCount;
                bool chk = chkAll.Checked;
                string checkflag;
                if (chk)
                {
                    checkflag = "Y";
                    for (int i = 1; i <= rowcount; i++)
                    {
                        DwDetail.SetItemString(i, "chkflag", checkflag);
                        try
                        {
                            debtbalance_amt += DwDetail.GetItemDecimal(i, "pay_amt");
                        }
                        catch
                        {
                            debtbalance_amt = 0;
                        }
                        debtbalance_amt = +debtbalance_amt;
                        DwDetail.SetItemString(i, "chkflag", checkflag);
                        DwTailer.SetItemDecimal(1, "debtdec_amt", debtbalance_amt);

                    }
                }
                else
                {
                    checkflag = "N";
                    for (int i = 1; i <= rowcount; i++)
                    {
                        DwDetail.SetItemString(i, "chkflag", checkflag);
                        try
                        {
                            debtbalance_amt -= DwDetail.GetItemDecimal(i, "pay_amt");
                        }
                        catch
                        {
                            debtbalance_amt = 0;
                        }
                        debtbalance_amt -= debtbalance_amt;
                        DwDetail.SetItemString(i, "chkflag", checkflag);
                        DwTailer.SetItemDecimal(1, "debtdec_amt", debtbalance_amt);

                    }
                    calTax();
                    calTax2();
                }


            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        private void ChickChk()
        {
            decimal debtbalance_amt = 0;
            try
            {
                int rowcount = DwDetail.RowCount;
                int rownow = Convert.ToInt32(HdRow_Detail.Value);

                DwDetail.SetItemString(rownow, "chkflag", DwDetail.GetItemString(rownow, "chkflag"));

                for (int i = 1; i <= rowcount; i++)
                {
                    string checkflag = DwDetail.GetItemString(i, "chkflag");

                    if (checkflag.Equals("Y"))
                    {
                        try
                        {
                            debtbalance_amt += DwDetail.GetItemDecimal(i, "pay_amt");
                        }
                        catch
                        {
                            debtbalance_amt += DwDetail.GetItemDecimal(i, "pay_amt");
                        }

                    }
                    else
                    {

                    }

                }
                DwTailer.SetItemDecimal(1, "debtdec_amt", debtbalance_amt);
                calTax();
                //calTax2();
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }

        }

        public void calTax()
        {

            try
            {
                //DwPrepay.SetItemString(1, "tddebtdec_toapproval_desc", "7654321");
                decimal debtbalance_amt = 0;
                int rowcount = DwDetail.RowCount;
                //int rownow = Convert.ToInt32(HdRow_Detail.Value);
                for (int i = 1; i <= rowcount; i++)
                {
                    string checkflag = DwDetail.GetItemString(i, "chkflag");

                    if (checkflag.Equals("Y"))
                    {
                        try
                        {
                            debtbalance_amt += DwDetail.GetItemDecimal(i, "pay_amt");
                        }
                        catch
                        {
                           // debtbalance_amt += DwDetail.GetItemDecimal(i, "pay_amt");
                        }

                    }
                    else
                    {

                    }

                }
                DwTailer.SetItemDecimal(1, "debtdec_amt", debtbalance_amt);
            }
            catch { }



            decimal debtdec_amt = DwTailer.GetItemDecimal(1, "debtdec_amt");
            decimal isservicetax = DwTailer.GetItemDecimal(1, "isservicetax");
            decimal servicetax = 0;
            decimal sum = 0;
            decimal amount = 0;

            try
            {
                String Sql = @"select credtax_rate from tdconstant";
                Sdt dt = WebUtil.QuerySdt(Sql);
                
                if (dt.Next())
                {
                    servicetax = dt.GetDecimal("credtax_rate");
                }
            }
            catch {  }
            
            if (isservicetax == 1)
            {
                
                DwTailer.SetItemDecimal(1, "servicetax", servicetax);
                //servicetax = DwTailer.GetItemDecimal(1, "servicetax");
                //sum = (debtdec_amt * (servicetax / 100));
                //DwTailer.SetItemDecimal(1, "servicetaxamt", sum);
                //amount = debtdec_amt - sum;          
               // DwTailer.SetItemDecimal(1, "amtbefortax", amount);


                string taxtype_code = DwTailer.GetItemString(1, "taxtype_code");
                //decimal debtdec_amt = DwTailer.GetItemDecimal(1, "debtdec_amt");
                decimal amtbefortax = DwTailer.GetItemDecimal(1, "amtbefortax");
                decimal slipnet_amt = 0;
                decimal tax_amt = 0;
                decimal tax_rate = 0;
                decimal sumtax = 0;
                try
                {
                    String Sql = @"select debttax_rate from tdconstant";
                    Sdt dt = WebUtil.QuerySdt(Sql);

                    if (dt.Next())
                    {
                        tax_rate = dt.GetDecimal("debttax_rate");
                    }
                }
                catch { }
                if (taxtype_code.Equals("E"))
                {
                    
                    DwTailer.SetItemDecimal(1, "tax_rate", tax_rate);
                    decimal debttt = DwTailer.GetItemDecimal(1, "debtdec_amt");
                    sum = 0;
                    sum = (debttt * servicetax) / 100;
                    DwTailer.SetItemDecimal(1, "servicetaxamt", sum);
                    amount = 0;
                    amount = debttt - sum;
                    DwTailer.SetItemDecimal(1, "amtbefortax", amount);
                    tax_amt = 0;
                    tax_amt = (debtdec_amt * tax_rate) / 100;
                    DwTailer.SetItemDecimal(1, "tax_amt", tax_amt);
                    slipnet_amt = 0;
                    slipnet_amt = amount + tax_amt;
                    DwTailer.SetItemDecimal(1, "slipnet_amt", slipnet_amt);
                   
                    //////////
                    
                    //DwTailer.SetItemDecimal(1, "slipnet_amt", 7654321);

                }
                else if (taxtype_code.Equals("I"))
                {

                    
                    DwTailer.SetItemDecimal(1, "tax_rate", tax_rate);

                    decimal amtbefor = DwTailer.GetItemDecimal(1, "debtdec_amt");
                   
                    sumtax = 0;
                    sumtax = amtbefor * tax_rate / (100 + tax_rate);
                    decimal debtt = 0;
                    debtt = amtbefor - sumtax;
                    DwTailer.SetItemDecimal(1, "debtdec_amt", debtt);
                    sum = 0;
                    sum = (debtt * (servicetax / 100));
                    DwTailer.SetItemDecimal(1, "servicetaxamt", sum);
                    amount = 0;
                    amount = debtt - sum;
                    DwTailer.SetItemDecimal(1, "amtbefortax", amount);
                    tax_amt = ((debtdec_amt * tax_rate) / 100);
                                                        
                    //DwTailer.SetItemDecimal(1, "debtdec_amt", amount);
                   // DwTailer.SetItemDecimal(1, "amtbefortax", amount);
                                        

                    DwTailer.SetItemDecimal(1, "tax_amt", tax_amt);//sumtax);
                    slipnet_amt = amount + tax_amt;

                    DwTailer.SetItemDecimal(1, "slipnet_amt", slipnet_amt);

                }

                else if (taxtype_code.Equals("X"))
                {

                    decimal amtbefor = DwTailer.GetItemDecimal(1, "debtdec_amt");                 
                    sum = 0;
                    sum = (amtbefor * (servicetax / 100));
                    DwTailer.SetItemDecimal(1, "servicetaxamt", sum);
                    amount = 0;
                    amount = amtbefor - sum;
                    DwTailer.SetItemDecimal(1, "amtbefortax", amount);
                    tax_amt = 0;              
                    DwTailer.SetItemDecimal(1, "slipnet_amt", amount);
                    DwTailer.SetItemDecimal(1, "tax_rate", 0);
                    DwTailer.SetItemDecimal(1, "tax_amt", 0);
                }         
            }
            else
            {
                

                DwTailer.SetItemDecimal(1, "servicetax", 0);              
                //amount = debtdec_amt;
                DwTailer.SetItemDecimal(1, "servicetaxamt", 0);
                //DwTailer.SetItemDecimal(1, "amtbefortax", debtdec_amt);

                string taxtype_code = DwTailer.GetItemString(1, "taxtype_code");
                //decimal debtdec_amt = DwTailer.GetItemDecimal(1, "debtdec_amt");
                //decimal amtbefortax = DwTailer.GetItemDecimal(1, "amtbefortax");
                amount = 0;
                decimal slipnet_amt = 0;
                decimal tax_amt = 0;
                decimal tax_rate = 0;
                decimal sumtax = 0;

                try
                {
                    String Sql = @"select debttax_rate from tdconstant";
                    Sdt dt = WebUtil.QuerySdt(Sql);

                    if (dt.Next())
                    {
                        tax_rate = dt.GetDecimal("debttax_rate");
                    }
                }
                catch { }

                if (taxtype_code.Equals("E"))
                {

                    DwTailer.SetItemDecimal(1, "tax_rate", tax_rate);

                    decimal amtbefor = DwTailer.GetItemDecimal(1, "debtdec_amt");
                    DwTailer.SetItemDecimal(1, "amtbefortax", amtbefor);
                    tax_amt = 0;
                    tax_amt = ((amtbefor * tax_rate) / 100);
                    DwTailer.SetItemDecimal(1, "tax_amt", tax_amt);
                    slipnet_amt = 0;
                    slipnet_amt = amtbefor + tax_amt;
                    DwTailer.SetItemDecimal(1, "slipnet_amt", slipnet_amt);
                }
                else if (taxtype_code.Equals("I"))
                {

                    decimal amtbefor = DwTailer.GetItemDecimal(1, "debtdec_amt");
                    DwMain.SetItemDecimal(1, "tax_rate", tax_rate);

                    sumtax = amtbefor * tax_rate / (100 + tax_rate);
                    amount = amtbefor - sumtax;

                    DwTailer.SetItemDecimal(1, "debtdec_amt", amount);
                    DwTailer.SetItemDecimal(1, "amtbefortax", amount);

                    tax_amt = ((amount * tax_rate) / 100);

                    DwTailer.SetItemDecimal(1, "tax_amt", tax_amt);//sumtax);
                    slipnet_amt = amount + tax_amt;

                    DwTailer.SetItemDecimal(1, "slipnet_amt", slipnet_amt);

                }

                else if (taxtype_code.Equals("X"))
                {
                    decimal amtbefor = DwTailer.GetItemDecimal(1, "debtdec_amt");
                    DwTailer.SetItemDecimal(1, "amtbefortax", amtbefor);
                    DwTailer.SetItemDecimal(1, "slipnet_amt", amtbefor);
                    DwTailer.SetItemDecimal(1, "tax_rate", 0);
                    DwTailer.SetItemDecimal(1, "tax_amt", 0);
                    DwTailer.SetItemDecimal(1, "servicetaxamt", 0);
                }

            }
            //calTax2();
        }



        public void calTax2()
        {
            string taxtype_code = DwTailer.GetItemString(1, "taxtype_code");
            decimal debtdec_amt = DwTailer.GetItemDecimal(1, "debtdec_amt");
            decimal amtbefortax = DwTailer.GetItemDecimal(1, "amtbefortax");
            decimal amount = 0;
            decimal slipnet_amt = 0;
            decimal tax_amt = 0;
            decimal tax_rate = 0;
            decimal sumtax = 0;
            try
            {
                String Sql = @"select debttax_rate from tdconstant";
                Sdt dt = WebUtil.QuerySdt(Sql);

                if (dt.Next())
                {
                    tax_rate = dt.GetDecimal("debttax_rate");
                }
            }
            catch { }

            if (taxtype_code.Equals("E"))
            {
                DwTailer.SetItemDecimal(1, "tax_rate", tax_rate);
                               
                sumtax = amtbefortax * (tax_rate / 100);
                amount = amtbefortax + sumtax;
                DwTailer.SetItemDecimal(1, "debtdec_amt", amount);
                DwTailer.SetItemDecimal(1, "amtbefortax", amount);
                tax_amt = ((amount * tax_rate) / 100);


                DwTailer.SetItemDecimal(1, "tax_amt", tax_amt);
                slipnet_amt = amount + tax_amt;

                DwTailer.SetItemDecimal(1, "slipnet_amt", slipnet_amt);
                
                //DwMain.SetItemDecimal(1, "tax_amt", sumtax);
                //DwMain.SetItemDecimal(1, "slipnet_amt", amount);
            }
            else if (taxtype_code.Equals("I"))
            {

                //DwDetail.SetItemDecimal(row, "amtbefortax", amtbefortax - (amtbefortax * tax_rate / (100 + tax_rate)));
                DwTailer.SetItemDecimal(1, "tax_rate", tax_rate);
                DwMain.SetItemDecimal(1, "tax_rate", tax_rate);
                //tax_rate = DwTailer.GetItemDecimal(1, "tax_rate");
                //sumtax = debtdec_amt * (tax_rate / 100);
                sumtax = amtbefortax * tax_rate / (100 + tax_rate);
                amount = amtbefortax - sumtax;//เปลี่ยนจาก + เป็น -

                DwTailer.SetItemDecimal(1, "debtdec_amt", amount);
                DwTailer.SetItemDecimal(1, "amtbefortax", amount);

                tax_amt = ((amount * tax_rate) / 100);

                DwTailer.SetItemDecimal(1, "tax_amt", tax_amt);//sumtax);
                slipnet_amt = amount + tax_amt;

                DwTailer.SetItemDecimal(1, "slipnet_amt", slipnet_amt);
                
                
                //DwMain.SetItemDecimal(1, "tax_amt", tax_amt);
                //DwMain.SetItemDecimal(1, "slipnet_amt", slipnet_amt);

            }
        }

        public void setDataTailerToDwMain()
        {
            DwMain.SetItemDecimal(1, "debtdec_amt", DwTailer.GetItemDecimal(1, "debtdec_amt"));
            DwMain.SetItemDecimal(1, "isservicetax", DwTailer.GetItemDecimal(1, "isservicetax"));
            DwMain.SetItemDecimal(1, "servicetax", DwTailer.GetItemDecimal(1, "servicetax"));
            DwMain.SetItemDecimal(1, "servicetaxamt", DwTailer.GetItemDecimal(1, "servicetaxamt"));
            DwMain.SetItemDecimal(1, "amtbefortax", DwTailer.GetItemDecimal(1, "amtbefortax"));
            DwMain.SetItemString(1, "taxtype_code", DwTailer.GetItemString(1, "taxtype_code"));
            DwMain.SetItemDecimal(1, "tax_rate", DwTailer.GetItemDecimal(1, "tax_rate"));
            DwMain.SetItemDecimal(1, "tax_amt", DwTailer.GetItemDecimal(1, "tax_amt"));
            DwMain.SetItemDecimal(1, "slipnet_amt", DwTailer.GetItemDecimal(1, "slipnet_amt"));


        }


        /////ADD/////
        protected void RunIReport()
        {
            try
            {
                string debtdecdoc_no = DwMain.GetItemString(1, "debtdecdoc_no");

                iReportArgument arg = new iReportArgument();
                arg.Add("DEBTDECDOC_NO1", iReportArgumentType.String, debtdecdoc_no);
                //arg.Add("as_member_no_end", iReportArgumentType.String, dsMain.DATA[0].endmembno);
                iReportBuider report = new iReportBuider(this, "รายละเอียด");
                report.AddCriteria("r_trading_settle_from", "รายละเอียด", ReportType.pdf, arg);
                report.Retrieve();
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }



        protected void RunIIReport()
        {
            try
            {
                string debtdecdoc_no = DwMain.GetItemString(1, "debtdecdoc_no");

                iReportArgument arg = new iReportArgument();
                arg.Add("DEBTDECDOC_NO1", iReportArgumentType.String, debtdecdoc_no);
                //arg.Add("as_member_no_end", iReportArgumentType.String, dsMain.DATA[0].endmembno);
                iReportBuider report = new iReportBuider(this, "ใบปะหน้า");
                report.AddCriteria("settle1", "ใบปะหน้า", ReportType.pdf, arg);
                report.Retrieve();
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }


        private void visible_()
        {

            DwMain.Modify("btn_01.VIsible=1");
            DwMain.Modify("btn_02.VIsible=1");
        }      




    }
}