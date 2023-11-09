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
using CoreSavingLibrary.WcfTrading;
using CoreSavingLibrary.WcfReport;
using Sybase.DataWindow;
using DataLibrary;

namespace Saving.Applications.trading
{
    public partial class w_sheet_td_info_produc : PageWebSheet, WebSheet
    {
        private string pbl = "sheet_td_info_product.pbl";
        private TradingClient tradingService;

        private DwThDate tDwTailer;

        protected String jsPostProductNo;
        protected String jsDwTailerInsertRow;
        protected String jsDwTailerInsertRow1;
        protected String jspostProductChange;

        public void InitJsPostBack()
        {
            tDwTailer = new DwThDate(DwTailer);
            tDwTailer.Add("effective_date", "effective_tdate");
            tDwTailer.Add("expire_date", "expire_tdate");

            jsPostProductNo = WebUtil.JsPostBack(this, "jsPostProductNo");
            jsDwTailerInsertRow = WebUtil.JsPostBack(this, "jsDwTailerInsertRow");
            jsDwTailerInsertRow1 = WebUtil.JsPostBack(this, "jsDwTailerInsertRow1");
            jspostProductChange = WebUtil.JsPostBack(this, "jspostProductChange");
        }

        public void WebSheetLoadBegin()
        {            
            try
            {
                tradingService = wcf.Trading;
            }
            catch
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถติดต่อ service ได้");
            }
            if(!IsPostBack){
                HiddenFieldTab.Value = "1";
                DwMain.InsertRow(0);
                DwMain.SetItemString(1, "coop_id", state.SsCoopId);
                DwMain.SetItemDecimal(1, "active_flag", 1);
                DwDetail.SetItemString(1, "coop_id", state.SsCoopId);
                DwTailer.SetItemString(1, "coop_id", state.SsCoopId);                
            }
            else
            {
                this.RestoreContextDw(DwMain);
                this.RestoreContextDw(DwDetail);
                this.RestoreContextDw(DwTailer, tDwTailer);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            switch (eventArg)
            {
                case "jsPostProductNo":
                    InitProduct();
                    break;
                case "jsDwTailerInsertRow":
                    DwTailerInsertRow();
                    break;
                case "jsDwTailerInsertRow1":
                    DwTailerInsertRow1();
                    break;
                case "jspostProductChange":
                    ProductChange();
                    break;
            }
        }

        public void SaveWebSheet()
        {
            try
            {
                str_tradesrv_oper astr_tradwsrv_opr = new str_tradesrv_oper();
                astr_tradwsrv_opr.xml_header = DwMain.Describe("DataWindow.Data.XML");
                astr_tradwsrv_opr.xml_detail = DwDetail.Describe("DataWindow.Data.XML");
                astr_tradwsrv_opr.xml_tailer = DwTailer.Describe("DataWindow.Data.XML");
                Int16 result = tradingService.of_save_info_product_master(state.SsWsPass, ref astr_tradwsrv_opr);
                if (result == 1)
                {
                    DwMain.Reset();
                    DwDetail.Reset();
                    DwTailer.Reset();
                    
                    try
                    {
                        DwUtil.ImportData(astr_tradwsrv_opr.xml_header, DwMain, null, FileSaveAsType.Xml);
                    }
                    catch {  }

                    try
                    {
                        DwUtil.ImportData(astr_tradwsrv_opr.xml_detail, DwDetail, null, FileSaveAsType.Xml);
                    }
                    catch {  }

                    try
                    {
                        DwUtil.ImportData(astr_tradwsrv_opr.xml_tailer, DwTailer, tDwTailer, FileSaveAsType.Xml);
                    }
                    catch {  }

                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");

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

        public void WebSheetLoadEnd()
        {
            try
            {
                DwUtil.RetrieveDDDW(DwMain, "producttype_code", pbl, null);
                DwUtil.RetrieveDDDW(DwMain, "productgroup_code", pbl, null);
                DwUtil.RetrieveDDDW(DwMain, "unit_code", pbl, null);
                //DwUtil.RetrieveDDDW(DwMain, "product_set_flag", pbl, null);
                DwUtil.RetrieveDDDW(DwDetail, "store_id", pbl, null);
                DwUtil.RetrieveDDDW(DwDetail, "taxtype_code", pbl, null);
                DwUtil.RetrieveDDDW(DwTailer, "price_lelvel", pbl, null);                
                
            }
            catch { }
            tDwTailer.Eng2ThaiAllRow();
            DwMain.SaveDataCache();
            DwDetail.SaveDataCache();
            DwTailer.SaveDataCache();
        }

        private void InitProduct()
        {
            try
            {                
                str_tradesrv_oper astr_tradwsrv_opr = new str_tradesrv_oper();
                astr_tradwsrv_opr.xml_header = DwMain.Describe("DataWindow.Data.XML");
                astr_tradwsrv_opr.xml_detail = DwDetail.Describe("DataWindow.Data.XML");
                astr_tradwsrv_opr.xml_tailer = DwTailer.Describe("DataWindow.Data.XML");
                Int16 result = tradingService.of_init_info_product_master_ed(state.SsWsPass, ref astr_tradwsrv_opr);
                if (result == 1)
                {
                    DwMain.Reset();
                    DwDetail.Reset();
                    DwTailer.Reset();

                    DwUtil.ImportData(astr_tradwsrv_opr.xml_header, DwMain, null, FileSaveAsType.Xml);
                    DwUtil.ImportData(astr_tradwsrv_opr.xml_detail, DwDetail, null, FileSaveAsType.Xml);
                    DwUtil.ImportData(astr_tradwsrv_opr.xml_tailer, DwTailer, tDwTailer, FileSaveAsType.Xml);
                }
                else
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("เกิดข้อผิดพลาด ไม่สามารถดึงข้อมูลได้");
                }

            }
            catch (Exception)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบข้อมูลสินค้า");
            }
        }

        private void DwTailerInsertRow()
        {
            try
            {
                int row = DwDetail.InsertRow(0);
                DwDetail.SetItemString(row, "coop_id", state.SsCoopId);
   
                string product_no = "";
                try { product_no = DwMain.GetItemString(1, "product_no"); }
                catch { }
                DwDetail.SetItemString(1, "product_no", product_no);



            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        private void DwTailerInsertRow1()
        {
            try
            {
                int row = DwTailer.InsertRow(0);
                DwTailer.SetItemDecimal(row, "active_flag", 1);
                DwTailer.SetItemString(row, "coop_id", state.SsCoopId);

                string product_no = "";
                try { product_no = DwMain.GetItemString(1, "product_no"); }
                catch { }
                DwTailer.SetItemString(1, "product_no", product_no);
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        private void ProductChange()
        {
            string product_no = "";
            try {product_no = DwMain.GetItemString(1, "product_no");}
            catch {}

            String Sql = @"select * from tdproductmaster 
                         where product_no = '" + product_no + "'" + " and coop_id = '" + state.SsCoopId + "'";
            Sdt dt = WebUtil.QuerySdt(Sql);
            if (dt.Next())
            {
                //DwMain.Reset();
                //DwMain.InsertRow(0);
                //DwDetail.Reset();
                //DwTailer.Reset();
                //HiddenFieldTab.Value = "1"; 
                //DwMain.SetItemString(1, "coop_id", state.SsCoopId);
               // DwMain.SetItemDecimal(1, "active_flag", 1);
               // DwDetail.SetItemString(1, "coop_id", state.SsCoopId);
              //  DwTailer.SetItemString(1, "coop_id", state.SsCoopId);    
                LtServerMessage.Text = WebUtil.ErrorMessage("รหัสสินค้า " + product_no + " ได้ถูกใช้งานแล้ว");
            }
        }
    }
}