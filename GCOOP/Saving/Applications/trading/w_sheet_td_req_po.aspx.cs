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
//
using System.Globalization;

namespace Saving.Applications.trading
{
    public partial class w_sheet_td_req_po : PageWebSheet, WebSheet
    {
        private string pbl = "sheet_td_req_po.pbl";
        private string sliptype_code = "PO";

        private TradingClient tradingService;
        private DwThDate tDwMain;

        protected String jsDebtNo;
        protected String jsCredNo;

        protected String jsProductNo;
        protected String jsPostSlipNo;

        protected String jsDiscPercentChange;
        protected String jsDiscountTailer;
        protected String jsRefresh;
        protected String jsDwDetailInsertRow;

        protected String jsItemQtyChange;
        protected String jsTaxCodeChange;
        protected String jsSlipNetAmtTailer;
        protected String jsTaxoptChange;
        protected String jsTaxCodeChangeTailer;
        protected String jsdeliverydateChange;
        protected String jscredittermChange;
        //
        protected String jsTaxrat;
        

        //------------Report------------------
        public String app = "";
        public String gid = "";
        public String rid = "";
        protected String pdf = "";

        public void InitJsPostBack()
        {
            tDwMain = new DwThDate(DwMain);
            tDwMain.Add("slip_date", "slip_tdate");
            tDwMain.Add("delivery_date", "delivery_tdate");
            tDwMain.Add("due_date", "due_tdate");

            jsCredNo = WebUtil.JsPostBack(this, "jsCredNo");
            jsDebtNo = WebUtil.JsPostBack(this, "jsDebtNo");

            jsProductNo = WebUtil.JsPostBack(this, "jsProductNo");
            jsPostSlipNo = WebUtil.JsPostBack(this, "jsPostSlipNo");

            jsDiscPercentChange = WebUtil.JsPostBack(this, "jsDiscPercentChange");
            jsDiscountTailer = WebUtil.JsPostBack(this, "jsDiscountTailer");
            jsRefresh = WebUtil.JsPostBack(this, "jsRefresh");

            jsItemQtyChange = WebUtil.JsPostBack(this, "jsItemQtyChange");
            jsTaxCodeChange = WebUtil.JsPostBack(this, "jsTaxCodeChange");

            jsDwDetailInsertRow = WebUtil.JsPostBack(this, "jsDwDetailInsertRow");
            jsSlipNetAmtTailer = WebUtil.JsPostBack(this, "jsSlipNetAmtTailer");
            jsTaxoptChange = WebUtil.JsPostBack(this, "jsTaxoptChange");
            jsTaxCodeChangeTailer = WebUtil.JsPostBack(this, "jsTaxCodeChangeTailer");
            jsdeliverydateChange = WebUtil.JsPostBack(this, "jsdeliverydateChange");
            jscredittermChange = WebUtil.JsPostBack(this, "jscredittermChange");
            //
            jsTaxrat = WebUtil.JsPostBack(this, "jsTaxrat");
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
                for (int i = 0; i < 5; i++)
                {
                    DwDetailInsertRow();
                }
            }
            else
            {
                this.RestoreContextDw(DwMain, tDwMain);
                this.RestoreContextDw(DwDetail);
                this.RestoreContextDw(DwTailer);
                string taxopt = "";
                try { taxopt = DwTailer.GetItemString(1, "taxopt"); }
                catch { }
                initcolor_from_taxopt(taxopt);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {

            switch (eventArg)
            {
                case "jsCredNo":
                    initCred();
                    break;
                case "jsDebtNo":
                    initDebt();
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
                case "jsDiscountTailer":
                    DiscountTailer();
                    TaxoptChange();
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
                    break;
                case "jsdeliverydateChange":
                    CredittermChange();
                    break;
                case "jscredittermChange":
                    CredittermChange();
                    break;
                case "popupReport":
                    PopupReport();
                    break;
                    //
                case "jsTaxrat":
                    TaxoptChange();
                    break;
                    
            }
        }

        public void SaveWebSheet()
        {
            
            String store_id = "";
            String product_no = "";
           // Decimal item_qty = 0;
            String paymentby = "";
                        
            try
            {
               
                store_id = DwDetail.GetItemString(1, "store_id");
                product_no = DwDetail.GetItemString(1, "product_no");
                //item_qty = DwDetail.GetItemDecimal(1, "item_qty");
                
                paymentby = DwMain.GetItemString(1, "paymentby");

            }
            catch
            {
                
                store_id = "";
                product_no = "";
                //item_qty = 0;
                // product_price = 0;
                paymentby = "";

            }
            if (store_id == "" || product_no == "" || paymentby == "") 
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
                       // RunProcessDetail();
                                                  
                        

                        //                    String sql = @" 
                        //                    SELECT TDSTOCKSLIP.SLIP_NO,   
                        //                        TDSTOCKSLIP.SLIP_DATE,   
                        //                        TDSTOCKSLIP.DUE_DATE,   
                        //                        TDSTOCKSLIP.DEBT_NO,   
                        //                        TDDEBTMASTER.DEBT_CONTACT,   
                        //                        TDDEBTMASTER.DEBT_NAME,   
                        //                        TDSTOCKSLIP.DEBT_ADDR,   
                        //                        TDSTOCKSLIPDET.SEQ_NO,   
                        //                        TDSTOCKSLIPDET.PRODUCT_DESC,   
                        //                        TDSTOCKSLIPDET.ITEM_QTY,   
                        //                        TDUCFUNIT.UNIT_DESC,   
                        //                        TDSTOCKSLIPDET.PRODUCT_PRICE,   
                        //                        TDSTOCKSLIPDET.AMTBEFORTAX,   
                        //                        TDSTOCKSLIP.SLIP_AMT,   
                        //                        TDSTOCKSLIP.TAX_AMT,   
                        //                        TDSTOCKSLIP.SLIPNET_AMT  
                        //                    FROM TDSTOCKSLIP,   
                        //                        TDSTOCKSLIPDET,   
                        //                        TDUCFUNIT,   
                        //                        TDDEBTMASTER  
                        //                    WHERE TDSTOCKSLIP.BRANCH_ID = TDSTOCKSLIPDET.BRANCH_ID and  
                        //                        TDSTOCKSLIP.SLIP_NO = TDSTOCKSLIPDET.SLIP_NO and  
                        //                        TDSTOCKSLIP.SLIPTYPE_CODE = TDSTOCKSLIPDET.SLIPTYPE_CODE and  
                        //                        TDSTOCKSLIP.BRANCH_ID = TDUCFUNIT.BRANCH_ID and 
                        //                        TDSTOCKSLIPDET.UNIT_CODE = TDUCFUNIT.UNIT_CODE and      
                        //                        TDSTOCKSLIP.BRANCH_ID = TDDEBTMASTER.BRANCH_ID and  
                        //                        TDSTOCKSLIP.SLIPTYPE_CODE = 'PO' and  
                        //                        TDSTOCKSLIP.DEBT_NO = TDDEBTMASTER.DEBT_NO and 
                        //                        TDSTOCKSLIP.BRANCH_ID = '" + state.SsCoopId + @"'";

                        //                    String slip_no = "";
                        //                    try
                        //                    {
                        //                        slip_no = DwMain.GetItemString(1, "slip_no");
                        //                    }
                        //                    catch
                        //                    {
                        //                        slip_no = "";
                        //                    }
                        //                    if (slip_no != "")
                        //                    {
                        //                        sql = sql + " and tdstockslip.slip_no = '" + slip_no + "'";
                        //                    }

                        //                    DataTable data = this.Query(sql);
                        //                    Printing.PrintAppletPB(this, "td_req_po", data); 

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
        public void WebSheetLoadEnd()
        {

           
            try
            {
               // DwUtil.RetrieveDDDW(DwMain, "paymentby", pbl, null);
                DwUtil.RetrieveDDDW(DwDetail, "store_id", pbl, null);
                DwUtil.RetrieveDDDW(DwTailer, "taxtype_code", pbl, null);
                DwUtil.RetrieveDDDW(DwDetail, "taxtype_code", pbl, null);
                DwUtil.RetrieveDDDW(DwDetail, "unit_code", pbl, null);
                DwUtil.RetrieveDDDW(DwMain, "store_id", pbl, null);
            }
            catch { }
            tDwMain.Eng2ThaiAllRow();
            DwMain.SaveDataCache();
            DwDetail.SaveDataCache();
            DwTailer.SaveDataCache();
        }

        public void initCred()
        {
            try 
            {
                str_tradesrv_oper astr_tradwsrv_req = new str_tradesrv_oper();
                astr_tradwsrv_req.xml_header = DwMain.Describe("DataWindow.Data.XML");
                astr_tradwsrv_req.xml_tailer = DwTailer.Describe("DataWindow.Data.XML");
                Int16 result = tradingService.of_init_info_cred(state.SsWsPass, ref astr_tradwsrv_req);
                if (result == 1)
                {
                    DwMain.Reset();
                    DwUtil.ImportData(astr_tradwsrv_req.xml_header, DwMain, tDwMain, FileSaveAsType.Xml);
                    DwTailer.Reset();
                    DwUtil.ImportData(astr_tradwsrv_req.xml_tailer, DwTailer, null, FileSaveAsType.Xml);
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

        public void initDebt()
        {
            try
            {
                str_tradesrv_oper astr_tradwsrv_req = new str_tradesrv_oper();
                astr_tradwsrv_req.xml_header = DwMain.Describe("DataWindow.Data.XML");
                astr_tradwsrv_req.xml_tailer = DwTailer.Describe("DataWindow.Data.XML");
                Int16 result = tradingService.of_init_info_debt(state.SsWsPass, ref astr_tradwsrv_req);
                if (result == 1)
                {
                    DwMain.Reset();
                    DwUtil.ImportData(astr_tradwsrv_req.xml_header, DwMain, tDwMain, FileSaveAsType.Xml);
                    //DwTailer.Reset();
                    //DwUtil.ImportData(astr_tradwsrv_req.xml_tailer, DwTailer, null, FileSaveAsType.Xml);
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
                try // edit by ham (taxtype_code หาย)
                {
                    DwTailer.SetItemString(1, "taxtype_code", DwDetail.GetItemString(1, "taxtype_code"));
                }
                catch
                {
                    DwTailer.InsertRow(0);
                    DwTailer.SetItemString(1, "taxtype_code", DwDetail.GetItemString(1, "taxtype_code"));
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        private void InitSlip()
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
                    try { disc_amt = DwTailer.GetItemDecimal(row, "disc_amt");}
                    catch { }
                }
                DwTailer.SetItemDecimal(row, "amtbefortax", (amount - disc_amt));
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
                Sdt dt = WebUtil.QuerySdt(Sql);
                if (dt.Next())
                {
                    decimal rate = 0;
                    rate = dt.GetDecimal("debttax_rate");
                    DwDetail.SetItemDecimal(row, "tax_rate", rate);
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
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

                //decimal tax_rate = 7;
                //try
                //{
                //    DwDetail.SetItemDecimal(row, "tax_rate", tax_rate);
                //}
                decimal tax_rate = 0;
                try
                {
                    tax_rate = DwDetail.GetItemDecimal(row, "tax_rate");
                }
                catch { }
                switch (taxtype_code)
                {
                    case "I":
                        DwDetail.SetItemDecimal(row, "amtbefortax", amtbefortax - (amtbefortax * tax_rate / (100 + tax_rate)));
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
                                    try { amt += DwDetail.GetItemDecimal(i, "net_amt"); }
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
                        DwTailer.SetItemDecimal(1, "slip_amt", amt);
                        DwTailer.SetItemDecimal(1, "amtbefortax", slip_amt - disc_amt);
                        amtbefortax = slip_amt - disc_amt;
                        break;
                    case "N":
                        initcolor_from_taxopt(taxopt);
                        for (int i = 1; i <= DwDetail.RowCount; i++)
                        {
                            DwDetail.SetItemString(i, "taxtype_code", DwTailer.GetItemString(1, "taxtype_code"));
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
                                    try { slip_amt += DwDetail.GetItemDecimal(i, "amtbefortax"); }//
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

                               // tax_amt = (amtbefortax * tax_rate) / (100 + tax_rate);
                               // DwTailer.SetItemDecimal(1, "amtbefortax", amtbefortax - tax_amt);
                                tax_amt = ((amtbefortax * tax_rate) / 100);
                                //amtbefortax -= tax_amt;
                                break;
                            case "E":
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
                DwTailer.SetItemDecimal(1, "tax_amt", tax_amt);
                DwTailer.SetItemDecimal(1, "slipnet_amt", (amtbefortax + tax_amt));
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

                try { slip_amt = DwTailer.GetItemDecimal(1, "slip_amt"); }
                catch { }

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

                DwTailer.SetItemDecimal(1, "slipnet_amt", ((slip_amt - disc_amt) + tax_amt));
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

       private void CredittermChange()
       {
           Double Credit_term = 0;
           try { Credit_term = DwMain.GetItemDouble(1, "credit_term"); }
           catch { Credit_term = 0; }
           try
           {
               DateTime delivery_date = DwMain.GetItemDateTime(1, "delivery_date");
               DateTime due_tdate = delivery_date.AddDays(Credit_term);
               DwMain.SetItemDate(1, "due_date", due_tdate);
               tDwMain.Add("due_date", "due_tdate");           
           
           }
           catch { }
       }

       public void initcolor_from_taxopt(string taxopt)
       {
           if (taxopt == "Y")
           {
               DwDetail.Modify("taxtype_code.background.color=1073741824");
               DwDetail.Modify("tax_rate.background.color=1073741824");
               DwTailer.Modify("taxtype_code.background.color=33543143");
               DwTailer.Modify("tax_rate.background.color=33543143");
               DwDetail.Modify("taxtype_code.Protect=0");
               DwDetail.Modify("tax_rate.Protect=0");
               DwTailer.Modify("taxtype_code.Protect=1");
               DwTailer.Modify("tax_rate.Protect=1");
           }
           else if (taxopt == "N")
           {
               DwDetail.Modify("taxtype_code.background.color=33543143");
               DwDetail.Modify("tax_rate.background.color=33543143");
               DwTailer.Modify("taxtype_code.background.color=1073741824");
               DwTailer.Modify("tax_rate.background.color=1073741824");
               DwDetail.Modify("taxtype_code.Protect=1");
               DwDetail.Modify("tax_rate.Protect=1");
               DwTailer.Modify("taxtype_code.Protect=0");
               DwTailer.Modify("tax_rate.Protect=2");

              
                   //("delivery_date", "delivery_tdate");
           }
       }

       #region printreport
       private void RunProcessDetail()
       {

           String print_id = "PO_FORM"; //รายงานตัวนี้ไม่มีใน webreportdetail นะรี่  by pat
           String branchId = state.SsCoopId;
           String slipno = DwMain.GetItemString(1, "Slip_no");
           //decimal seq_no = Convert.ToInt16(HdSeqNo.Value);
           //String seqNo = DwChq.GetItemString(1, "seq_no");//ตัวนี้ ใน DwChq ไม่มีนะ seq_no by pat

           app = state.SsApplication;
           gid = "TRADING_FORM";
           rid = print_id;
           ReportHelper lnv_helper = new ReportHelper();
//           lnv_helper.AddArgument(branchId, ArgumentType.String);
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

       ////report//
       public void RunReport()
       {
           try
           {
               string slip_no = DwMain.GetItemString(1, "slip_no");

               iReportArgument arg = new iReportArgument();
               arg.Add("as_slip_no_start", iReportArgumentType.String, slip_no);
               arg.Add("as_slip_no_end", iReportArgumentType.String, slip_no);
               //arg.Add("as_member_no_end", iReportArgumentType.String, dsMain.DATA[0].endmembno);
               iReportBuider report = new iReportBuider(this, "ใบสั่งซื้อ");
               report.AddCriteria("trading_report_po_2", "ใบสั่งซื้อ", ReportType.pdf, arg);
               report.Retrieve();
           }
           catch (Exception ex)
           {
               LtServerMessage.Text = WebUtil.ErrorMessage(ex);
           }
       }
    }
}