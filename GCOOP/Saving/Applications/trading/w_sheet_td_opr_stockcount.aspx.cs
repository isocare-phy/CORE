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

namespace Saving.Applications.trading
{
    public partial class w_sheet_td_opr_stockcount : PageWebSheet, WebSheet
    {
        string pbl = "sheet_td_opr_stockcount.pbl";
        private string sliptype_code = "VCS";
        private TradingClient tradingService;
        protected String jsProductNo;
        protected String jsPostSlipNo;
        protected String jsDwDetailInsertRow;
        private DwThDate tDwMain;
        protected String jsStoreChange;

        public void InitJsPostBack()
        {
            tDwMain = new DwThDate(DwMain);
            tDwMain.Add("slip_date", "slip_tdate");
            jsProductNo = WebUtil.JsPostBack(this, "jsProductNo");
            jsPostSlipNo = WebUtil.JsPostBack(this, "jsPostSlipNo");
            jsStoreChange = WebUtil.JsPostBack(this, "jsStoreChange");     
            jsDwDetailInsertRow = WebUtil.JsPostBack(this, "jsDwDetailInsertRow");
        }

        public void WebSheetLoadBegin()
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
                DwMain.SetItemString(1, "coop_id", state.SsCoopId);
                DwMain.SetItemString(1, "entry_id", state.SsUsername);
                //DwMain.SetItemString(1, "store_id", "001");
                DwMain.SetItemString(1, "sliptype_code", sliptype_code);
                string Store_id = "";
                try
                {
                    string Sql = "select store_id from tdstore where default_flag = 1";
                    Sdt dt = WebUtil.QuerySdt(Sql);
                    if (dt.Next())
                    {
                        Store_id = dt.GetString("store_id");
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
            }
            else
            {
                this.RestoreContextDw(DwMain, tDwMain);
                this.RestoreContextDw(DwDetail);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            switch (eventArg)
            {
                case "jsProductNo":
                    initDetail();
                    break;
                case "jsPostSlipNo":
                    InitSlip();
                    break;
                case "jsDwDetailInsertRow":
                    DwDetailInsertRow();
                    break;
                case "jsStoreChange":
                    InitStockCount();
                    break;
            }
        }

        public void SaveWebSheet()
        {
            try
            {
                str_tradesrv_oper astr_tradwsrv_req = new str_tradesrv_oper();
                astr_tradwsrv_req.xml_header = DwMain.Describe("DataWindow.Data.XML");
                astr_tradwsrv_req.xml_detail = DwDetail.Describe("DataWindow.Data.XML");
                Int16 result = tradingService.of_save_verifycountstk(state.SsWsPass, ref astr_tradwsrv_req);
                if (result == 1)
                {
                    DwMain.Reset();
                    DwDetail.Reset();
                    DwUtil.ImportData(astr_tradwsrv_req.xml_header, DwMain, tDwMain, FileSaveAsType.Xml);
                    DwUtil.ImportData(astr_tradwsrv_req.xml_detail, DwDetail, null, FileSaveAsType.Xml);
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
                DwUtil.RetrieveDDDW(DwMain, "store_id", pbl, null);
                DwUtil.RetrieveDDDW(DwDetail, "unit_code", pbl, null);
            }
            catch { }
            tDwMain.Eng2ThaiAllRow();

            DwMain.SaveDataCache();
            DwDetail.SaveDataCache();
        }

        private void InitStockCount()
        {
            try
            { 
                DwDetail.Reset();
                string store_id = DwMain.GetItemString(1, "store_id");
                DwMain.SetItemString(1, "store_id", store_id);
                //DwMain.SetItemString(1, "coop_id", state.SsCoopId);
                //DwMain.SetItemString(1, "sliptype_code", sliptype_code);
                str_tradesrv_oper astr_tradwsrv_req = new str_tradesrv_oper();
                astr_tradwsrv_req.xml_header = DwMain.Describe("DataWindow.Data.XML");
                astr_tradwsrv_req.xml_detail = DwDetail.Describe("DataWindow.Data.XML");
                
                try
                {
                    Int16 result = tradingService.of_init_info_store(state.SsWsPass, ref astr_tradwsrv_req);
                    if (result == 1)
                    {
                        DwMain.Reset();
                        DwDetail.Reset();
                        DwUtil.ImportData(astr_tradwsrv_req.xml_header, DwMain, tDwMain, FileSaveAsType.Xml);
                        DwUtil.ImportData(astr_tradwsrv_req.xml_detail, DwDetail, null, FileSaveAsType.Xml);
                    }
                    else
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage("เกิดข้อผิดพลาด ไม่สามารถดึงข้อมูลได้");
                    }
                }
                catch
                {
                    DwMain.Reset();
                    DwDetail.Reset();
                    DwMain.InsertRow(0);
                    DwMain.SetItemString(1, "coop_id", state.SsCoopId);
                    DwMain.SetItemString(1, "entry_id", state.SsUsername);
                    DwMain.SetItemString(1, "sliptype_code", sliptype_code);
                    DwMain.SetItemDateTime(1, "slip_date", state.SsWorkDate);
                    DwMain.SetItemDateTime(1, "entry_date", state.SsWorkDate);
                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่มีรายการสินค้า");
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
                DwMain.SetItemString(1, "coop_id", state.SsCoopId);
                DwMain.SetItemString(1, "sliptype_code", sliptype_code);
                str_tradesrv_oper astr_tradwsrv_req = new str_tradesrv_oper();
                astr_tradwsrv_req.xml_header = DwMain.Describe("DataWindow.Data.XML");
                astr_tradwsrv_req.xml_detail = DwDetail.Describe("DataWindow.Data.XML");
                Int16 result = tradingService.of_init_verifycountstk(state.SsWsPass, ref astr_tradwsrv_req);
                if (result == 1)
                {
                    DwMain.Reset();
                    DwDetail.Reset();
                    DwUtil.ImportData(astr_tradwsrv_req.xml_header, DwMain, tDwMain, FileSaveAsType.Xml);
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

        public void initDetail()
        {
            try
            {
                str_tradesrv_oper astr_tradwsrv_req = new str_tradesrv_oper();
                astr_tradwsrv_req.xml_header = DwMain.Describe("DataWindow.Data.XML");
                astr_tradwsrv_req.xml_detail = DwDetail.Describe("DataWindow.Data.XML");
                Int16 result = tradingService.of_init_info_product(state.SsWsPass, ref astr_tradwsrv_req);
                if (result == 1)
                {
                    DwMain.Reset();
                    DwDetail.Reset();
                    DwUtil.ImportData(astr_tradwsrv_req.xml_header, DwMain, tDwMain, FileSaveAsType.Xml);
                    DwUtil.ImportData(astr_tradwsrv_req.xml_detail, DwDetail, null, FileSaveAsType.Xml);
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

        private void DwDetailInsertRow()
        {
            try
            {
                int row = DwDetail.InsertRow(0);
                DwDetail.SetItemString(row, "coop_id", state.SsCoopId);
                DwDetail.SetItemDecimal(row, "seq_no", row);
                DwDetail.SetItemString(row, "sliptype_code", sliptype_code);
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        //private void StoreChange()
        //{
        //    try
        //    {
        //        string store_id = DwMain.GetItemString(1, "store_id");
        //        DwUtil.RetrieveDataWindow(DwDetail, pbl, null, store_id);
        //    }
        //    catch (Exception ex)
        //    {
        //        LtServerMessage.Text = WebUtil.ErrorMessage(ex);
        //    }
        //}

    }
}