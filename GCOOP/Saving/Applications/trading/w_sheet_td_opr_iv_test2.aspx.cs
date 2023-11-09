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
using Sybase.DataWindow;
using DataLibrary;
using CoreSavingLibrary.WcfTrading;
using CoreSavingLibrary.WcfReport;

namespace Saving.Applications.trading
{
    public partial class w_sheet_td_opr_iv_test2 : PageWebSheet, WebSheet
    {
        private string pbl = "sheet_td_opr_iv.pbl";
        private string sliptype_code = "IV";
        private TradingClient tradingService;
        private DwThDate tDwMain;

        protected String jsDebtNo;

        protected String jsProductNo;
        protected String jsPostSlipNo;

        protected String jsDiscPercentChange;
        protected String jsDiscAmtChange;
        protected String jsDiscountTailer;
        protected String jsRefresh;
        protected String jsDwDetailInsertRow;

        protected String jsItemQtyChange;
        protected String jsTaxCodeChange;
        protected String jsSlipNetAmtTailer;
        protected String jsTaxoptChange;
        protected String jsTaxCodeChangeTailer;

        protected String jsinitValues;
        protected String jsBSlipNo;
        protected String jsBSlipDate;
        protected String jsTransportfeeChange;
        //
        protected String jstaxratChange;
        protected String jsprint;
        protected String jsPostAccountNo;

        //------------Report------------------
        public String app = "";
        public String gid = "";
        public String rid = "";
        protected String pdf = "";


        public void InitJsPostBack()
        {

            tDwMain = new DwThDate(DwMain);
            tDwMain.Add("slip_date", "slip_tdate");
            tDwMain.Add("due_date", "due_tdate");


            jsDebtNo = WebUtil.JsPostBack(this, "jsDebtNo");

            jsProductNo = WebUtil.JsPostBack(this, "jsProductNo");


            jsPostSlipNo = WebUtil.JsPostBack(this, "jsPostSlipNo");
            jsBSlipDate = WebUtil.JsPostBack(this, "jsBSlipDate");

            jsDiscPercentChange = WebUtil.JsPostBack(this, "jsDiscPercentChange");
            jsDiscAmtChange = WebUtil.JsPostBack(this, "jsDiscAmtChange");
            jsDiscountTailer = WebUtil.JsPostBack(this, "jsDiscountTailer");
            jsRefresh = WebUtil.JsPostBack(this, "jsRefresh");
            jsItemQtyChange = WebUtil.JsPostBack(this, "jsItemQtyChange");
            jsTaxCodeChange = WebUtil.JsPostBack(this, "jsTaxCodeChange");

            jsDwDetailInsertRow = WebUtil.JsPostBack(this, "jsDwDetailInsertRow");
            jsSlipNetAmtTailer = WebUtil.JsPostBack(this, "jsSlipNetAmtTailer");
            jsTaxoptChange = WebUtil.JsPostBack(this, "jsTaxoptChange");
            jsTaxCodeChangeTailer = WebUtil.JsPostBack(this, "jsTaxCodeChangeTailer");

            jsinitValues = WebUtil.JsPostBack(this, "jsinitValues");
            jsBSlipNo = WebUtil.JsPostBack(this, "jsBSlipNo");
            jsTransportfeeChange = WebUtil.JsPostBack(this, "jsTransportfeeChange");
            //
            jstaxratChange = WebUtil.JsPostBack(this, "jstaxratChange");
            jsprint = WebUtil.JsPostBack(this, "jsprint");
            jsPostAccountNo = WebUtil.JsPostBack(this, "jsPostAccountNo");

        }

        public void WebSheetLoadBegin()
        {
            HdOpenIFrame.Value = "False";
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
                DwMain.SetItemString(1, "coop_id", state.SsCoopId);
                DwMain.SetItemString(1, "debtbranch_id", state.SsCoopId);
                DwMain.SetItemString(1, "entry_id", state.SsUsername);
                string Store_id = "";
                string taxtypecode = "";
                Decimal debttax_rate = 0;
                try
                {
                    string Sql = "select store_id from tdstore where default_flag = 1";
                    Sdt dt = WebUtil.QuerySdt(Sql);
                    if (dt.Next())
                    {
                        Store_id = dt.GetString("store_id");

                    }
                    string Sql2 = "select taxtype_code,debttax_rate from tdconstant where coop_id = '" + state.SsCoopId + "'";
                    Sdt dt2 = WebUtil.QuerySdt(Sql2);
                    if (dt2.Next())
                    {
                        taxtypecode = dt2.GetString("taxtype_code");
                        debttax_rate = dt2.GetDecimal("debttax_rate");
                    }
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                    throw ex;
                }
                DwMain.SetItemString(1, "store_id", Store_id);
                DwMain.SetItemString(1, "sliptype_code", sliptype_code);
                DwMain.SetItemDateTime(1, "slip_date", state.SsWorkDate);
                DwMain.SetItemDateTime(1, "entry_date", state.SsWorkDate);
                DwTailer.InsertRow(0);
                DwTailer.SetItemString(1, "taxtype_code", taxtypecode);
                DwTailer.SetItemDecimal(1, "tax_rate", debttax_rate);
            }
            else
            {
                this.RestoreContextDw(DwMain, tDwMain);
                this.RestoreContextDw(DwDetail);
                this.RestoreContextDw(DwTailer);



                string taxopt = "";
                try { taxopt = DwTailer.GetItemString(1, "taxopt"); }
                catch { }
                if (taxopt == "")
                {
                    DwTailer.SetItemString(1, "taxopt", "N");
                    try { taxopt = DwTailer.GetItemString(1, "taxopt"); }
                    catch { }
                }
                //initcolor_from_taxopt(taxopt);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {

            switch (eventArg)
            {
                case "jsDebtNo":
                    initMain();
                    break;

                case "jsProductNo":
                    initDetail();
                    break;
                case "jsPostSlipNo":
                    InitSlip();
                    break;
                case "jsDiscPercentChange":
                    DiscPercentChange();
                    TaxCodeChange();
                    TaxoptChange();
                    break;
                case "jsDiscAmtChange":
                    DiscAmtChange();
                    TaxCodeChange();
                    TaxoptChange();
                    break;
                case "jsDiscountTailer":
                    DiscountTailer();
                    TaxoptChange();
                    TaxCodeChange();
                    break;
                case "jsDwDetailInsertRow":
                    DwDetailInsertRow();
                    break;
                case "jsItemQtyChange":
                    ItemQtyChange();
                    TaxCodeChange();
                    TaxoptChange();
                    break;
                case "jsTaxCodeChange":
                    TaxCodeChange();
                    TaxoptChange();
                    break;
                case "jsSlipNetAmtTailer":
                    SlipNetAmtTailer();
                    break;
                case "jsTaxoptChange":
                    TaxoptChange();
                    break;
                case "jsTaxCodeChangeTailer":
                    TaxoptChange();
                    calTAX();
                    break;
                case "jsinitValues":
                    initValues();
                    break;
                case "jsBSlipNo":
                    initRef();
                    break;
                case "jsTransportfeeChange":
                    TransportfeeChange();
                    break;
                case "popupReport":
                    PopupReport();
                    break;
                case "jsBSlipDate":
                    SlipDateChange();
                    break;
                //
                case "jstaxratChange":
                    TaxoptChange();
                    break;
                case "jsprint":
                    RunReport();
                    break;
                case "jsPostAccountNo":
                    AccountNo();
                    break;

            }
        }

        #region printreport
        private void RunProcessDetail()
        {

            String print_id = "IV_FORM"; //รายงานตัวนี้ไม่มีใน webreportdetail นะรี่  by pat
            String branchId = state.SsCoopId;
            String slipno = DwMain.GetItemString(1, "Slip_no");
            //decimal seq_no = Convert.ToInt16(HdSeqNo.Value);
            //String seqNo = DwChq.GetItemString(1, "seq_no");//ตัวนี้ ใน DwChq ไม่มีนะ seq_no by pat

            app = state.SsApplication;
            gid = "TRADING_FORM";
            rid = print_id;
            ReportHelper lnv_helper = new ReportHelper();
            lnv_helper.AddArgument(slipno, ArgumentType.String);
            lnv_helper.AddArgument(slipno, ArgumentType.String);
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
            String employee_id = "";
            String store_id = "";
            String product_no = "";
            Decimal item_qty = 0;
            // Decimal product_price = 0;

            try
            {
                employee_id = DwMain.GetItemString(1, "employee_id");
                store_id = DwDetail.GetItemString(1, "store_id");
                product_no = DwDetail.GetItemString(1, "product_no");
                item_qty = DwDetail.GetItemDecimal(1, "item_qty");
                //product_price = DwDetail.GetItemDecimal(1, "product_price");

            }
            catch
            {
                employee_id = "";
                store_id = "";
                product_no = "";
                item_qty = 0;
                // product_price = 0;

            }
            //add16/01/2557
            string slip_no = DwMain.GetItemString(1, "slip_no");
            string posttovc_flag = "";
            string receipt_no = "";
            string slip_status = "";
            try
            {
                string Sql = "select  slip_status,receipt_no,posttovc_flag from tdstockslip where slip_no = '" + slip_no + "' ";//
                Sdt dt = WebUtil.QuerySdt(Sql);

                if (dt.Next())
                {
                    posttovc_flag = dt.GetString("posttovc_flag");
                    receipt_no = dt.GetString("receipt_no");
                    slip_status = dt.GetString("slip_status");
                }
            }
            catch { }

            if (posttovc_flag != "1" && receipt_no == "" && (slip_status == "8" || slip_status == ""))
            {
                if (employee_id == "" )//|| store_id == "" || product_no == "" || item_qty == 0)  //|| product_price == 0)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("กรุณาระบุข้อมูลให้ครบ");
                }
                else
                {
                    try
                    {
                        str_tradesrv_oper astr_tradwsrv_req = new str_tradesrv_oper();
                        astr_tradwsrv_req.xml_header = DwMain.Describe("DataWindow.Data.XML");
                        astr_tradwsrv_req.xml_detail = DwDetail.Describe("DataWindow.Data.XML");
                        astr_tradwsrv_req.xml_tailer = DwTailer.Describe("DataWindow.Data.XML");
                        Int16 result = tradingService.of_save_op_slip(state.SsWsPass, ref astr_tradwsrv_req);
                        if (result == 1)
                        {
                            DwMain.Reset();
                            DwDetail.Reset();
                            DwTailer.Reset();
                            DwUtil.ImportData(astr_tradwsrv_req.xml_header, DwMain, tDwMain, FileSaveAsType.Xml);
                            DwUtil.ImportData(astr_tradwsrv_req.xml_detail, DwDetail, null, FileSaveAsType.Xml);
                            DwUtil.ImportData(astr_tradwsrv_req.xml_tailer, DwTailer, null, FileSaveAsType.Xml);
                            LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
                            RunReport();
                            //RunProcessDetail();

                            //                        String sql = @" 
                            //                        SELECT ss.SLIP_NO,   
                            //	                        ss.SLIP_DATE,   
                            //	                        ss.DUE_DATE,   
                            //	                        ss.DEBT_NO,   
                            //	                        pn.PRENAME_DESC||dm.DEBT_NAME||' '||pn.SUFFNAME_DESC as debt_name,   
                            //	                        ss.DEBT_ADDR,   
                            //	                        dm.PHONE,   
                            //	                        dm.FAX,   
                            //	                        ssd.SEQ_NO,   
                            //	                        ssd.PRODUCT_DESC,   
                            //	                        ssd.ITEM_QTY,   
                            //	                        un.UNIT_DESC,   
                            //	                        ssd.PRODUCT_PRICE,   
                            //	                        ssd.AMTBEFORTAX AMTBEFORTAXDET,   
                            //	                        ss.SLIP_AMT,   
                            //	                        ss.DISC_AMT,   
                            //	                        ss.AMTBEFORTAX AMTBEFORTAX,   
                            //	                        ss.TAX_AMT,   
                            //	                        ss.SLIPNET_AMT,
                            //	                        ss.paymentby,   
                            //	                        ss.transportfee,
                            //                            pn2.prename_desc||''|| sm.name||' '||sm.surname sale_name,
                            //                            pn2.prename_desc||''|| sm.name||' '||sm.surname sale_name2,
                            //	                        dm.disccredits * ss.SLIPNET_AMT discamt,
                            //	                        dm.disccredits * ss.SLIPNET_AMT disccredits 
                            //                        FROM TDSTOCKSLIP ss,   
                            //	                        TDSTOCKSLIPDET ssd,   
                            //	                        TDUCFUNIT un,   
                            //	                        TDDEBTMASTER dm,   
                            //	                        TDSALESMAN sm,
                            //	                        MBUCFPRENAME pn,
                            //						 MBUCFPRENAME pn2
                            //                        WHERE ss.BRANCH_ID = ssd.BRANCH_ID and  
                            //	                        ss.SLIPTYPE_CODE = ssd.SLIPTYPE_CODE and  
                            //	                        ss.SLIP_NO = ssd.SLIP_NO and  
                            //	                        ss.BRANCH_ID = dm.BRANCH_ID and  
                            //	                        ss.DEBT_NO = dm.DEBT_NO and  
                            //	                        ssd.BRANCH_ID = un.BRANCH_ID(+) and  
                            //	                        ssd.UNIT_CODE = un.UNIT_CODE(+) and  
                            //	                        ss.BRANCH_ID = sm.BRANCH_ID(+) and 
                            //	                        ss.EMPLOYEE_ID = sm.SALENO(+) and  
                            //	                        dm.prename_code = pn.prename_code (+) and
                            //						 sm.prename_code =  pn2.prename_code(+) and  
                            //	                        ss.SLIPTYPE_CODE = 'IV' and 
                            //                            ss.BRANCH_ID = '" + state.SsCoopId + @"'";

                            //                        String slip_no = "";
                            //                        try
                            //                        {
                            //                            slip_no = DwMain.GetItemString(1, "slip_no");
                            //                        }
                            //                        catch
                            //                        {
                            //                            slip_no = "";
                            //                        }
                            //                        if (slip_no != "")
                            //                        {
                            //                            sql = sql + " and ss.slip_no = '" + slip_no + "'";
                            //                        }

                            //                        sql += " order by ssd.SEQ_NO";

                            //                        DataTable data = this.Query(sql);
                            //                        Printing.PrintAppletPB(this, "td_opr_iv", data);

                        }
                        else
                        {
                            LtServerMessage.Text = WebUtil.ErrorMessage("เกิดข้อผิดพลาด ไม่สามารถบันทึกได้");
                        }
                    }
                    catch (Exception ex)
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                    }
                }
            }
            else
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถแก้ไขได้ รายการนี้ตั้งหนี้แล้ว");
            }
        }

        public void WebSheetLoadEnd()
        {
            try
            {
                DwUtil.RetrieveDDDW(DwMain, "employee_id", pbl, null);
                DwUtil.RetrieveDDDW(DwDetail, "store_id", pbl, null);
                //     DwUtil.RetrieveDDDW(DwMain, "paymentby", pbl, null);
                DwUtil.RetrieveDDDW(DwTailer, "taxtype_code", pbl, null);
                DwUtil.RetrieveDDDW(DwDetail, "taxtype_code", pbl, null);
                DwUtil.RetrieveDDDW(DwDetail, "unit_code", pbl, null);
                DwUtil.RetrieveDDDW(DwMain, "tdstockslip_account_no", pbl, null);
            }
            catch { }

            //ADD//
            try
            {
                string slip_no = DwMain.GetItemString(1, "slip_no");
                if (slip_no == "Auto")
                {
                    DwMain.Modify("print_t.VIsible=0");

                }
                else
                {
                    DwMain.Modify("print_t.VIsible=1");

                }

            }
            catch
            {
                DwMain.Modify("print_t.VIsible=0");
            }
            //EndAdd

            tDwMain.Eng2ThaiAllRow();
            DwMain.SaveDataCache();
            DwDetail.SaveDataCache();
            DwTailer.SaveDataCache();
        }

        public void initMain()
        {
            try
            {
                //
                try
                {
                    DwMain.SetItemString(1, "coop_id", state.SsCoopId);
                    DwMain.SetItemString(1, "debtbranch_id", state.SsCoopId);
                    DwMain.SetItemString(1, "sliptype_code", sliptype_code);
                }
                catch { }
                str_tradesrv_oper astr_tradwsrv_req = new str_tradesrv_oper();
                astr_tradwsrv_req.xml_header = DwMain.Describe("DataWindow.Data.XML");
                astr_tradwsrv_req.xml_tailer = DwTailer.Describe("DataWindow.Data.XML");

                Int16 result = tradingService.of_init_info_debt(state.SsWsPass, ref astr_tradwsrv_req);
                if (result == 1)
                {
                    DwMain.Reset();
                    //DwTailer.Reset();
                    DwUtil.ImportData(astr_tradwsrv_req.xml_header, DwMain, tDwMain, FileSaveAsType.Xml);
                    //DwUtil.ImportData(astr_tradwsrv_req.xml_tailer, DwTailer, null, FileSaveAsType.Xml);
                }
                else
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("เกิดข้อผิดพลาด ไม่สามารถทำรายการได้");
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        public void initDetail()
        {
            try
            {

                str_tradesrv_oper astr_tradwsrv_req = new str_tradesrv_oper();
                astr_tradwsrv_req.xml_detail = DwDetail.Describe("DataWindow.Data.XML");
                Int16 result = tradingService.of_init_info_product(state.SsWsPass, ref astr_tradwsrv_req);
                if (result == 1)
                {
                    DwDetail.Reset();
                    DwUtil.ImportData(astr_tradwsrv_req.xml_detail, DwDetail, null, FileSaveAsType.Xml);

                }
                else
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("เกิดข้อผิดพลาด ไม่สามารถทำรายการได้");
                }
                /*  try // edit by ham (taxtype_code หาย)
                  {
                      DwTailer.SetItemString(1, "taxtype_code", DwDetail.GetItemString(1, "taxtype_code"));
                    //  DwTailer.SetItemDecimal(1, "tax_rate", DwDetail.GetItemDecimal(1, "tax_rate"));
                  }
                  catch
                  {
                      DwTailer.InsertRow(0);
                      DwTailer.SetItemString(1, "taxtype_code", DwDetail.GetItemString(1, "taxtype_code"));
                    //  DwTailer.SetItemDecimal(1, "tax_rate", DwDetail.GetItemDecimal(1, "tax_rate"));
                  } */
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        [AjaxPostBack]
        public string ajaxinitDetail()
        {
            string xml_string = "";
            try
            {
                string product_no = Request["product_no"];
                DwDetail.SetItemString(1, "product_no", product_no);

                str_tradesrv_oper astr_tradwsrv_req = new str_tradesrv_oper();
                astr_tradwsrv_req.xml_detail = DwDetail.Describe("DataWindow.Data.XML");
                Int16 result = tradingService.of_init_info_product(state.SsWsPass, ref astr_tradwsrv_req);
                if (result == 1)
                {
                    xml_string =  astr_tradwsrv_req.xml_detail;
                }
                else
                {
                    xml_string = "";
                }
           }
            catch (Exception ex)
            {
                xml_string = "";
                //LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
            return xml_string;
        }

        private void InitSlip()
        {
            try
            {
                DwMain.SetItemString(1, "coop_id", state.SsCoopId);
                DwMain.SetItemString(1, "sliptype_code", sliptype_code);
                str_tradesrv_oper astr_tradwsrv_req = new str_tradesrv_oper();
                astr_tradwsrv_req.xml_header = DwMain.Describe("DataWindow.Data.XML");
                astr_tradwsrv_req.xml_detail = DwDetail.Describe("DataWindow.Data.XML");
                astr_tradwsrv_req.xml_tailer = DwTailer.Describe("DataWindow.Data.XML");
                Int16 result = tradingService.of_init_op_slip(state.SsWsPass, ref astr_tradwsrv_req);
                if (result == 1)
                {
                    DwMain.Reset();
                    DwDetail.Reset();
                    DwTailer.Reset();
                    DwUtil.ImportData(astr_tradwsrv_req.xml_header, DwMain, tDwMain, FileSaveAsType.Xml);
                    DwUtil.ImportData(astr_tradwsrv_req.xml_detail, DwDetail, null, FileSaveAsType.Xml);
                    DwUtil.ImportData(astr_tradwsrv_req.xml_tailer, DwTailer, null, FileSaveAsType.Xml);

                    string taxopt = "";
                    try { taxopt = DwTailer.GetItemString(1, "taxopt"); }
                    catch { }
                    initcolor_from_taxopt(taxopt);
                }
                else
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("เกิดข้อผิดพลาด ไม่สามารถดึงข้อมูลได้");
                }

            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        public void DiscAmtChange()
        {
            try
            {
                int row = Convert.ToInt32(HdRowDetail.Value);
                decimal item_qty = 0, product_price = 0, disc_amt = 0;
                try
                {
                    item_qty = DwDetail.GetItemDecimal(row, "item_qty");
                    product_price = DwDetail.GetItemDecimal(row, "product_price");
                }
                catch
                {
                    throw new Exception("กรุณากรอกจำนวน และราคาต่อหน่วย");
                }
                try
                {
                    disc_amt = DwDetail.GetItemDecimal(row, "disc_amt");
                }
                catch { }
                CalAmt(item_qty, product_price, disc_amt, row);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DiscPercentChange()
        {
            try
            {
                int row = Convert.ToInt32(HdRowDetail.Value);
                decimal item_qty = 0, product_price = 0, disc_amt = 0;
                string disc_percent = "";
                try
                {
                    item_qty = DwDetail.GetItemDecimal(row, "item_qty");
                    product_price = DwDetail.GetItemDecimal(row, "product_price");
                }
                catch
                {
                    throw new Exception("กรุณากรอกจำนวน และราคาต่อหน่วย");
                }
                try
                {
                    disc_percent = DwDetail.GetItemString(row, "disc_percent");
                }
                catch { }
                try
                {
                    disc_amt = DwDetail.GetItemDecimal(row, "disc_amt");
                }
                catch { }
                CalAmt(item_qty, product_price, disc_percent, disc_amt, row);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ItemQtyChange()
        {
            try
            {
                int row = Convert.ToInt32(HdRowDetail.Value);
                string debt_no = "";
                try
                {
                    debt_no = DwMain.GetItemString(1, "debt_no");
                }
                catch { }
                string price_level = "";
                try
                {
                    price_level = DwMain.GetItemString(1, "price_level");
                }
                catch { }


                DateTime slip_date = DwMain.GetItemDateTime(1, "slip_date");


                string product_no = "";
                try
                {
                    product_no = DwDetail.GetItemString(row, "product_no");
                }
                catch { }
                decimal item_qty = 0;
                try
                {
                    item_qty = DwDetail.GetItemDecimal(row, "item_qty");
                }
                catch { }

                String Sql = @"select * from tdproductpricelist 
                where product_no = '" + product_no + "'" +
                " and price_lelvel = '" + price_level + "'" +
                " and (fromqty <= " + item_qty + " and toqty >= " + item_qty + ")" +
                " and trunc(effective_date) <= to_date('" + slip_date.ToString("ddMMyyyy") + "', 'ddmmyyyy')" +
                " and (trunc(expire_date) >= to_date('" + slip_date.ToString("ddMMyyyy") + "', 'ddmmyyyy') or expire_date is null)";
                Sdt dt = WebUtil.QuerySdt(Sql);

                if (dt.Next())
                {
                    decimal price = dt.GetDecimal("price");
                    string disc_percent = dt.GetString("disc_percent");
                    decimal disc_amt = dt.GetDecimal("disc_amt");

                    DwDetail.SetItemDecimal(row, "product_price", price);
                    if (disc_percent != "")
                    {
                        DwDetail.SetItemString(row, "disc_percent", disc_percent);
                    }
                    else
                    {
                        if (disc_amt != 0)
                        {
                            DwDetail.SetItemDecimal(row, "disc_amt", disc_amt * item_qty);
                        }
                    }
                    CalAmt(item_qty, price, disc_percent, disc_amt, row);

                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        [AjaxPostBack]
        private string ajaxinitItemQty()
        {
            string xml = "";
            try
            {
                int row = Convert.ToInt32(HdRowDwDetail.Value);
                string debt_no = Request["debt_no"];
                string price_level = Request["price_level"];
                string slip_tdate = Request["slip_tdate"];
                string[] arr = slip_tdate.Split('/');
                slip_tdate = arr[1] +"/"+arr[0]+"/" + Convert.ToString(Convert.ToDecimal(arr[2]) -543);
                DateTime slip_date = Convert.ToDateTime(slip_tdate);  //GetItemDateTime(1, "slip_date");
                string product_no = Request["product_no"];
                decimal item_qty = Convert.ToDecimal(Request["item_qty"]);

                String Sql = @"select * from tdproductpricelist 
                where product_no = '" + product_no + "'" +
                " and price_lelvel = '" + price_level + "'" +
                " and (fromqty <= " + item_qty + " and toqty >= " + item_qty + ")" +
                " and trunc(effective_date) <= to_date('" + slip_date.ToString("ddMMyyyy") + "', 'ddmmyyyy')" +
                " and (trunc(expire_date) >= to_date('" + slip_date.ToString("ddMMyyyy") + "', 'ddmmyyyy') or expire_date is null)";
                Sdt dt = WebUtil.QuerySdt(Sql);
                xml =  WebUtil.datatabletoXML(dt);
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
            return xml;
        }

        private void CalAmt(decimal item_qty, decimal product_price, decimal disc_amt, int row)
        {
            try
            {
                decimal amount = (item_qty * product_price);//, amtbefortax = 0;

                DwDetail.SetItemDecimal(row, "amtbefortax", (amount - disc_amt));
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        private void CalAmt(decimal item_qty, decimal product_price, string disc_percent, decimal disc_amt, int row)
        {
            try
            {
                decimal amount = (item_qty * product_price);//, amtbefortax = 0;
                disc_amt = tradingService.Discount(disc_percent, amount);

                DwDetail.SetItemDecimal(row, "disc_amt", disc_amt);

                DwDetail.SetItemDecimal(row, "amtbefortax", (amount - disc_amt));
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        public void DiscountTailer()
        {
            try
            {
                int row = 1;
                decimal slip_amt = 0;
                try
                {
                    slip_amt = DwTailer.GetItemDecimal(row, "slip_amt");
                }
                catch
                {
                    DwTailer.SetItemString(row, "disc_percent", "");
                    throw new Exception("กรุณากรอกจำนวนงิน");
                }
                string disc_percent = "";
                try
                {
                    disc_percent = DwTailer.GetItemString(row, "disc_percent");
                }
                catch { }
                decimal amount = slip_amt;
                decimal disc_amt = 0;
                if (disc_percent != "")
                {
                    disc_amt = tradingService.Discount(disc_percent, amount);
                    DwTailer.SetItemDecimal(row, "disc_amt", disc_amt);
                }
                else
                {
                    try { disc_amt = DwTailer.GetItemDecimal(row, "disc_amt"); }
                    catch { }
                }
                DwTailer.SetItemDecimal(row, "amtbefortax", (amount - disc_amt));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            calTAX();
        }

        private void DwDetailInsertRow()
        {
            try
            {
                int row = DwDetail.InsertRow(0);
                DwDetail.SetItemString(row, "coop_id", state.SsCoopId);
                DwDetail.SetItemDecimal(row, "seq_no", row);
                DwDetail.SetItemString(row, "sliptype_code", DwMain.GetItemString(1, "sliptype_code"));
                DwDetail.SetItemString(row, "taxtype_code", "E");

                String Sql = "select debttax_rate from tdconstant";
                Sdt dt = QuerySdt(Sql);
                if (dt.Next())
                {
                    decimal rate = 0;
                    rate = dt.GetDecimal("debttax_rate");
                    DwDetail.SetItemDecimal(row, "tax_rate", rate);
                }
                //decimal rate = 7;
                //DwDetail.SetItemDecimal(row, "tax_rate", rate);
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        private Sdt QuerySdt(string Sql)
        {
            throw new NotImplementedException();
        }

        private void TaxCodeChange()
        {

            try
            {
                int row = Convert.ToInt32(HdRowDetail.Value);
                decimal item_qty;
                try
                {
                    item_qty = DwDetail.GetItemDecimal(row, "item_qty");
                }
                catch
                {
                    throw new Exception("กรุณากรอก จำนวนสินค้า แถวที่ " + row);
                }
                decimal disc_amt = 0;
                try
                {
                    disc_amt = DwDetail.GetItemDecimal(row, "disc_amt");
                }
                catch { }
                decimal product_price = 0;
                try
                {
                    product_price = DwDetail.GetItemDecimal(row, "product_price");
                }
                catch { }

                decimal amtbefortax = (item_qty * product_price) - disc_amt;
                DwDetail.SetItemDecimal(row, "amtbefortax", amtbefortax);

                string taxtype_code = "";
                try
                {
                    taxtype_code = DwDetail.GetItemString(row, "taxtype_code");
                }
                catch { }

                decimal tax_rate = 0;
                try
                {
                    tax_rate = DwDetail.GetItemDecimal(row, "tax_rate");
                }
                catch { }

                switch (taxtype_code)
                {
                    case "I":

                        decimal sum = product_price - (product_price * tax_rate / (100 + tax_rate));
                        //sum = Convert.ToDecimal(string.Format("{0:0.00}", sum));
                        decimal sum_2 = sum * item_qty;
                        //sum_2 = Convert.ToDecimal(string.Format("{0:0.00}", sum_2));

                        DwDetail.SetItemDecimal(row, "amtbefortax", sum_2);

                       amtbefortax = amtbefortax - (amtbefortax * tax_rate / (100 + tax_rate));
                        //DwDetail.SetItemDecimal(row, "amtbefortax", amtbefortax - (amtbefortax * tax_rate / (100 + tax_rate)));
                        DwDetail.SetItemDecimal(row, "net_amt", amtbefortax);

                        break;
                    case "E":
                        DwDetail.SetItemDecimal(row, "net_amt", amtbefortax + (amtbefortax * (tax_rate / 100)));
                        break;
                    case "X":
                        DwDetail.SetItemDecimal(row, "net_amt", amtbefortax);
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void TaxoptChange()
        {
            try
            {
                string taxtype_code = "";
                string taxopt = "N";
                try
                {
                    taxopt = DwTailer.GetItemString(1, "taxopt");
                }
                catch { }

                decimal tax_amt = 0, amtbefortax = 0, slip_amt = 0, disc_amt = 0, amt = 0;
                decimal transportfee = 0;
                try
                {
                    transportfee = DwTailer.GetItemDecimal(1, "transportfee");
                }
                catch { }

                switch (taxopt)
                {
                    case "Y":
                        initcolor_from_taxopt(taxopt);
                        for (int i = 1; i <= DwDetail.RowCount; i++)
                        {
                            try
                            {
                                taxtype_code = DwDetail.GetItemString(i, "taxtype_code");
                            }
                            catch
                            {
                                taxtype_code = "";
                            }

                            switch (taxtype_code)
                            {
                                case "I":
                                    try { slip_amt += DwDetail.GetItemDecimal(i, "net_amt"); }
                                    catch { }
                                    try { amt += DwDetail.GetItemDecimal(i, "amtbefortax"); }
                                    catch { }
                                    try { tax_amt += (DwDetail.GetItemDecimal(i, "net_amt") - DwDetail.GetItemDecimal(i, "amtbefortax")); }
                                    catch { }
                                    try { slip_amt -= (DwDetail.GetItemDecimal(i, "net_amt") - DwDetail.GetItemDecimal(i, "amtbefortax")); }
                                    catch { }
                                    break;
                                case "E":
                                    try { slip_amt += DwDetail.GetItemDecimal(i, "amtbefortax"); }
                                    catch { }
                                    try { amt += DwDetail.GetItemDecimal(i, "amtbefortax"); }
                                    catch { }
                                    try { tax_amt += (DwDetail.GetItemDecimal(i, "net_amt") - DwDetail.GetItemDecimal(i, "amtbefortax")); }
                                    catch { }
                                    break;
                                case "X":
                                    try { slip_amt += DwDetail.GetItemDecimal(i, "amtbefortax"); }
                                    catch { }
                                    try { amt += DwDetail.GetItemDecimal(i, "amtbefortax"); }
                                    catch { }
                                    break;
                            }
                        }
                        try
                        {
                            disc_amt = DwTailer.GetItemDecimal(1, "disc_amt");
                        }
                        catch { }
                        amt = Convert.ToDecimal(string.Format("{0:0.00}", amt));
                        DwTailer.SetItemDecimal(1, "slip_amt", amt);
                        
                        decimal amt_am =  slip_amt - disc_amt;
                        amt_am = Convert.ToDecimal(string.Format("{0:0.00}", amt_am));
                        DwTailer.SetItemDecimal(1, "amtbefortax", amt_am);
                        amtbefortax = slip_amt - disc_amt;
                        break;
                    case "N":
                        initcolor_from_taxopt(taxopt);
                        for (int i = 1; i <= DwDetail.RowCount; i++)
                        {
                            try { DwDetail.SetItemString(i, "taxtype_code", DwTailer.GetItemString(1, "taxtype_code")); }
                            catch { }
                            HdRowDetail.Value = Convert.ToString(i);
                            TaxCodeChange();
                        }

                        decimal tax_rate = 0;
                        try
                        {
                            tax_rate = DwTailer.GetItemDecimal(1, "tax_rate");
                        }
                        catch { }

                        try
                        {
                            taxtype_code = DwTailer.GetItemString(1, "taxtype_code");
                        }
                        catch { }

                        switch (taxtype_code)
                        {
                            case "I":
                                for (int i = 1; i <= DwDetail.RowCount; i++)
                                {
                                    try { slip_amt += DwDetail.GetItemDecimal(i, "amtbefortax"); }
                                    catch { }
                                }
                                DwTailer.SetItemDecimal(1, "slip_amt", slip_amt);

                                try { disc_amt = DwTailer.GetItemDecimal(1, "disc_amt"); }
                                catch { }
                                DwTailer.SetItemDecimal(1, "amtbefortax", slip_amt - disc_amt);
                                amtbefortax = slip_amt - disc_amt;

                                tax_amt = ((amtbefortax * tax_rate) / 100);

                                break;

                            case "E":
                                for (int i = 1; i <= DwDetail.RowCount; i++)
                                {
                                    try { slip_amt += DwDetail.GetItemDecimal(i, "amtbefortax"); }
                                    catch { }
                                }
                                DwTailer.SetItemDecimal(1, "slip_amt", slip_amt);

                                try { disc_amt = DwTailer.GetItemDecimal(1, "disc_amt"); }
                                catch { }
                                DwTailer.SetItemDecimal(1, "amtbefortax", slip_amt - disc_amt);
                                amtbefortax = slip_amt - disc_amt;

                                tax_amt = ((amtbefortax * tax_rate) / 100);
                                break;
                            case "X":
                                for (int i = 1; i <= DwDetail.RowCount; i++)
                                {
                                    try { slip_amt += DwDetail.GetItemDecimal(i, "amtbefortax"); }
                                    catch { }
                                }
                                DwTailer.SetItemDecimal(1, "slip_amt", slip_amt);

                                try
                                {
                                    disc_amt = DwTailer.GetItemDecimal(1, "disc_amt");
                                }
                                catch { }
                                DwTailer.SetItemDecimal(1, "amtbefortax", slip_amt - disc_amt);
                                amtbefortax = slip_amt - disc_amt;
                                tax_amt = 0;
                                break;
                        }
                        break;
                }

                DwMain.SetItemDecimal(1, "tax_amt", tax_amt);
                tax_amt = Convert.ToDecimal(string.Format("{0:0.00}", tax_amt));
                DwTailer.SetItemDecimal(1, "tax_amt", tax_amt);
                decimal sum_ = 0;
                sum_ = amtbefortax + tax_amt + transportfee;
                sum_ = Convert.ToDecimal(string.Format("{0:0.00}", sum_));

                DwTailer.SetItemDecimal(1, "slipnet_amt", sum_);
                //add
                try
                {
                    decimal slipnet_amt = DwTailer.GetItemDecimal(1, "slipnet_amt");
                    DwMain.SetItemDecimal(1, "tdstockslip_pay_amt", slipnet_amt);
                }
                catch
                {
                }
                //end
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        private void TransportfeeChange()
        {
            try
            {
                //
                decimal slip_amt = 0, disc_amt = 0;

                slip_amt = DwTailer.GetItemDecimal(1, "slip_amt");

                try
                {
                    disc_amt = DwTailer.GetItemDecimal(1, "disc_amt");
                }
                catch { }

                DwTailer.SetItemDecimal(1, "amtbefortax", slip_amt - disc_amt);
                decimal tax_amt = 0;
                try
                {
                    tax_amt = DwTailer.GetItemDecimal(1, "tax_amt");
                }
                catch { }
                decimal transportfee = 0;
                try
                {
                    transportfee = DwTailer.GetItemDecimal(1, "transportfee");
                }
                catch { }
                //transportfee = 0;
                decimal sum = 0;
                sum = ((slip_amt - disc_amt) + tax_amt + transportfee);
                sum = Convert.ToDecimal(string.Format("{0:0.00}", sum));
                DwTailer.SetItemDecimal(1, "slipnet_amt", sum);

            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        private void SlipNetAmtTailer()
        {
            try
            {
                decimal slip_amt = 0, disc_amt = 0;

                slip_amt = DwTailer.GetItemDecimal(1, "slip_amt");

                try
                {
                    disc_amt = DwTailer.GetItemDecimal(1, "disc_amt");
                }
                catch { }

                DwTailer.SetItemDecimal(1, "amtbefortax", slip_amt - disc_amt);
                decimal tax_amt = 0;
                try
                {
                    tax_amt = DwTailer.GetItemDecimal(1, "tax_amt");
                }
                catch { }
                decimal transportfee = 0;
                try
                {
                    transportfee = DwTailer.GetItemDecimal(1, "transportfee");
                }
                catch { }
                decimal sum_ = 0;
                sum_ = ((slip_amt - disc_amt) + tax_amt + transportfee);
                sum_ = Convert.ToDecimal(string.Format("{0:0.00}", sum_));

                DwTailer.SetItemDecimal(1, "slipnet_amt", sum_);
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        private void initValues()
        {
            String values = HdResult.Value;
            String Products = HdProducts.Value;
            String value, product;
            int commaV, commaProduct;
            try
            {
                do
                {
                    commaV = values.IndexOf(",");
                    commaProduct = Products.IndexOf(",");
                    if (commaV == 0)
                    {
                        break;
                    }
                    else
                    {
                        value = values.Substring(0, commaV);
                        product = Products.Substring(0, commaProduct);

                        values = values.Substring(commaV + 1, (values.Length - (value.Length + 1)));
                        Products = Products.Substring(commaProduct + 1, (Products.Length - (product.Length + 1)));
                        if (values == "")
                        {
                            values = ",";
                        }
                        DwDetailInsertRow();
                        DwDetail.SetItemString(DwDetail.RowCount, "refdoc_no", value);
                        DwDetail.SetItemString(DwDetail.RowCount, "product_no", product);
                    }
                } while (true);

                str_tradesrv_oper astr_tradwsrv_req = new str_tradesrv_oper();
                astr_tradwsrv_req.xml_detail = DwDetail.Describe("DataWindow.Data.XML");
                Int16 result = tradingService.of_init_info_qt(state.SsWsPass, ref astr_tradwsrv_req);
                if (result == 1)
                {
                    DwDetail.Reset();
                    DwUtil.ImportData(astr_tradwsrv_req.xml_detail, DwDetail, null, FileSaveAsType.Xml);
                }
                else
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("เกิดข้อผิดพลาด ไม่สามารถดึงข้อมูลได้");
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        public void initRef()
        {
            try
            {
                str_tradesrv_oper astr_tradwsrv_req = new str_tradesrv_oper();
                astr_tradwsrv_req.xml_header = DwMain.Describe("DataWindow.Data.XML");
                astr_tradwsrv_req.xml_detail = DwDetail.Describe("DataWindow.Data.XML");
                astr_tradwsrv_req.xml_tailer = DwTailer.Describe("DataWindow.Data.XML");
                astr_tradwsrv_req.xml_list = "PO";
                Int16 result = tradingService.of_init_info_slipref(state.SsWsPass, ref astr_tradwsrv_req);
                if (result == 1)
                {
                    DwMain.Reset();
                    DwDetail.Reset();
                    DwTailer.Reset();
                    DwUtil.ImportData(astr_tradwsrv_req.xml_header, DwMain, tDwMain, FileSaveAsType.Xml);
                    DwUtil.ImportData(astr_tradwsrv_req.xml_detail, DwDetail, null, FileSaveAsType.Xml);
                    DwUtil.ImportData(astr_tradwsrv_req.xml_tailer, DwTailer, null, FileSaveAsType.Xml);

                    string taxopt = "";
                    try { taxopt = DwTailer.GetItemString(1, "taxopt"); }
                    catch { }
                    initcolor_from_taxopt(taxopt);
                }
                else
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("เกิดข้อผิดพลาด ไม่สามารถดึงข้อมูลได้");
                }

            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        public void initcolor_from_taxopt(string taxopt)
        {
            if (taxopt == "Y")
            {
                //DwDetail.Modify("taxtype_code.background.color=1073741824");
                //DwDetail.Modify("tax_rate.background.color=1073741824");
                //DwTailer.Modify("taxtype_code.background.color=33543143");
                //DwTailer.Modify("tax_rate.background.color=33543143");
                //DwDetail.Modify("taxtype_code.Protect=0");
                //DwDetail.Modify("tax_rate.Protect=0");
                //DwTailer.Modify("taxtype_code.Protect=1");
                //DwTailer.Modify("tax_rate.Protect=1");
            }
            else if (taxopt == "N")
            {
                //DwDetail.Modify("taxtype_code.background.color=33543143");
                //DwDetail.Modify("tax_rate.background.color=33543143");
                //DwTailer.Modify("taxtype_code.background.color=1073741824");
                //DwTailer.Modify("tax_rate.background.color=1073741824");
                //DwDetail.Modify("taxtype_code.Protect=1");
                //DwDetail.Modify("tax_rate.Protect=1");
                //DwTailer.Modify("taxtype_code.Protect=0");
                //DwTailer.Modify("tax_rate.Protect=0");
            }
        }

        public void SlipDateChange()
        {
            Double CreditTerm = Convert.ToDouble(DwMain.GetItemDecimal(1, "credit_term"));
            DateTime SlipDate = DwMain.GetItemDateTime(1, "slip_date");
            DateTime DueDate = SlipDate.AddDays(CreditTerm);
            DwMain.SetItemDateTime(1, "due_date", DueDate);

        }
        public void calTAX()
        {
            decimal transportfee = 0;
            string paymentby = "";
            decimal slip_amt = 0;
            decimal tax_amt = 0;
            decimal tax_rate = 0;
            decimal disc_amt = 0;
            decimal amtbefortax = 0;
            string taxtype_code = "";
            try
            {
                paymentby = DwMain.GetItemString(1, "paymentby");
            }
            catch
            {
                paymentby = "";
            }
            try
            {
                taxtype_code = DwTailer.GetItemString(1, "taxtype_code");
            }
            catch
            {
                taxtype_code = "";
            }
            try
            {
                slip_amt = DwTailer.GetItemDecimal(1, "slip_amt");
            }
            catch
            {
                slip_amt = 0;
            }
            try
            {
                tax_rate = DwTailer.GetItemDecimal(1, "tax_rate");
            }
            catch
            {
                tax_rate = 0;
            }
            try
            {
                disc_amt = DwTailer.GetItemDecimal(1, "disc_amt");
            }
            catch
            {
                disc_amt = 0;
            }
            try
            {
                amtbefortax = DwTailer.GetItemDecimal(1, "amtbefortax");
            }
            catch
            {
                amtbefortax = 0;
            }

            try
            {
                transportfee = DwTailer.GetItemDecimal(1, "transportfee");
            }
            catch
            {
                transportfee = 0;
            }


            switch (paymentby)
            {
                case "LON":
                    if (taxtype_code.Equals("E"))
                    {
                        tax_amt = (slip_amt * (tax_rate / 100));
                        tax_amt = Convert.ToDecimal(string.Format("{0:0.00}", tax_amt));
                        DwTailer.SetItemDecimal(1, "tax_amt", tax_amt);
                        decimal sum = 0;
                        sum = (amtbefortax + tax_amt + transportfee);
                        sum = Convert.ToDecimal(string.Format("{0:0.00}", sum));
                        DwTailer.SetItemDecimal(1, "slipnet_amt", sum);
                    }
                    //เพิ่ม
                    else if (taxtype_code.Equals("I"))
                    {
                        tax_amt = (slip_amt * (tax_rate / 100));
                        tax_amt = Convert.ToDecimal(string.Format("{0:0.00}", tax_amt));
                        DwTailer.SetItemDecimal(1, "tax_amt", tax_amt);
                        decimal sum = 0;
                        sum = (amtbefortax + tax_amt + transportfee);
                        sum = Convert.ToDecimal(string.Format("{0:0.00}", sum));
                        DwTailer.SetItemDecimal(1, "slipnet_amt", sum);
                    }//
                    else
                    {
                        tax_amt = (slip_amt * (tax_rate / 100));
                        tax_amt = Convert.ToDecimal(string.Format("{0:0.00}", tax_amt));
                        DwTailer.SetItemDecimal(1, "tax_amt", tax_amt);
                        decimal sum = 0;
                        sum = (amtbefortax + transportfee);
                        sum = Convert.ToDecimal(string.Format("{0:0.00}", sum));
                        DwTailer.SetItemDecimal(1, "slipnet_amt", sum);
                    }
                    break;
                case "CSH":
                    if (taxtype_code.Equals("E"))
                    {
                        tax_amt = (amtbefortax * (tax_rate / 100));
                        tax_amt = Convert.ToDecimal(string.Format("{0:0.00}", tax_amt));
                        DwTailer.SetItemDecimal(1, "tax_amt", tax_amt);
                        decimal sum = 0;
                        sum = (amtbefortax + tax_amt + transportfee);
                        sum = Convert.ToDecimal(string.Format("{0:0.00}", sum));
                        DwTailer.SetItemDecimal(1, "slipnet_amt", sum);
                    }
                    //เพิ่ม
                    else if (taxtype_code.Equals("I"))
                    {
                        tax_amt = (amtbefortax * (tax_rate / 100));
                        tax_amt = Convert.ToDecimal(string.Format("{0:0.00}", tax_amt));
                        DwTailer.SetItemDecimal(1, "tax_amt", tax_amt);
                        decimal sum = 0;
                        sum = (amtbefortax + tax_amt + transportfee);
                        sum = Convert.ToDecimal(string.Format("{0:0.00}", sum));
                        DwTailer.SetItemDecimal(1, "slipnet_amt", sum);
                    }//
                    else
                    {
                        tax_amt = (amtbefortax * (tax_rate / 100));
                        tax_amt = Convert.ToDecimal(string.Format("{0:0.00}", tax_amt));
                        DwTailer.SetItemDecimal(1, "tax_amt", tax_amt);
                        decimal sum = 0;
                        sum = (amtbefortax + transportfee);
                        sum = Convert.ToDecimal(string.Format("{0:0.00}", sum));
                        DwTailer.SetItemDecimal(1, "slipnet_amt", (amtbefortax + transportfee));
                    }
                    break;
            }
        }

        private void AccountNo()
        {
            try
            {

                string account_no = DwMain.GetItemString(1, "tdstockslip_account_no");
                DataWindowChild dc = DwMain.GetChild("tdstockslip_account_no");
                int rGroup = dc.FindRow("account_no ='" + account_no + "'", 1, dc.RowCount);
                string bank_code = DwUtil.GetString(dc, rGroup, "bank_code", "");
                string bankbranch_code = DwUtil.GetString(dc, rGroup, "bankbranch_code", "");
                string account_id = DwUtil.GetString(dc, rGroup, "account_id", "");

                ////////// add////////////
                string paymentby = DwMain.GetItemString(1, "paymentby");

                DwMain.SetItemString(1, "tdstockslip_bank_code", bank_code);
                DwMain.SetItemString(1, "tdstockslip_bank_branch", bankbranch_code);
                DwMain.SetItemString(1, "tdstockslip_account_no", account_no);




            }
            catch { }
        }
        ////report//
        public void RunReport()
        {
            try
            {
                string slip_no = DwMain.GetItemString(1, "slip_no");

                iReportArgument arg = new iReportArgument();
                arg.Add("as_slip_no", iReportArgumentType.String, slip_no);
                //arg.Add("as_member_no_end", iReportArgumentType.String, dsMain.DATA[0].endmembno);
                iReportBuider report = new iReportBuider(this, "ใบกำกับภาษี");
                report.AddCriteria("trading_report_iv", "ใบกำกับภาษี", ReportType.pdf, arg);
                report.Retrieve();
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }
        ///////////



    }


}


