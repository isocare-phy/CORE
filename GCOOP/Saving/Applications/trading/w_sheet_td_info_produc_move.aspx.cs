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
    public partial class w_sheet_td_info_produc_move : PageWebSheet, WebSheet
    {
        private string pbl = "sheet_td_info_product_move.pbl";
        private TradingClient tradingService;
        private DwThDate tDwDetail;
        private DwThDate tDwTailer;

        protected String jsPostProductNo;
        protected String jsstore;

        public void InitJsPostBack()
        {
            tDwTailer = new DwThDate(DwTailer);
            tDwTailer.Add("lot_date", "lot_tdate");

            tDwDetail = new DwThDate(DwDetail);
            tDwDetail.Add("slip_date", "slip_tdate");

            jsPostProductNo = WebUtil.JsPostBack(this, "jsPostProductNo");
            jsstore = WebUtil.JsPostBack(this, "jsstore");
            
        }

        public void WebSheetLoadBegin()
        {
            HiddenFieldTab.Value = "1";
            try
            {
                tradingService = wcf.Trading;
            }
            catch
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถติดต่อ service ได้");
            }
            if(!IsPostBack){
                DwMain.InsertRow(0);
                DwMain.SetItemString(1, "coop_id", state.SsCoopId);
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
                    //InitProduct();
                    stro_();
                    break;
                case "jsstore":
                    stro_();
                    break;
            }
        }

        public void SaveWebSheet()
        {
            //try
            //{
            //    str_tradesrv_oper astr_tradwsrv_opr = new str_tradesrv_oper();
            //    astr_tradwsrv_opr.xml_header = DwMain.Describe("DataWindow.Data.XML");
            //    astr_tradwsrv_opr.xml_detail = DwDetail.Describe("DataWindow.Data.XML");
            //    astr_tradwsrv_opr.xml_tailer = DwTailer.Describe("DataWindow.Data.XML");
            //    Int16 result = tradingService.of_save_info_product_master(state.SsWsPass, ref astr_tradwsrv_opr);
            //    if (result == 1)
            //    {
            //        DwMain.Reset();
            //        DwDetail.Reset();
            //        DwTailer.Reset();
                    
            //        try
            //        {
            //            DwUtil.ImportData(astr_tradwsrv_opr.xml_header, DwMain, null, FileSaveAsType.Xml);
            //        }
            //        catch {  }

            //        try
            //        {
            //            DwUtil.ImportData(astr_tradwsrv_opr.xml_detail, DwDetail, null, FileSaveAsType.Xml);
            //        }
            //        catch {  }

            //        try
            //        {
            //            DwUtil.ImportData(astr_tradwsrv_opr.xml_tailer, DwTailer, tDwTailer, FileSaveAsType.Xml);
            //        }
            //        catch {  }

            //        LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");

            //    }
            //    else
            //    {
            //        LtServerMessage.Text = WebUtil.ErrorMessage("เกิดข้อผิดพลาด ไม่สามารถบันทึกได้");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            //}
        }

        public void WebSheetLoadEnd()
        {
            try
            {
                DwUtil.RetrieveDDDW(DwMain, "producttype_code", pbl, null);
                DwUtil.RetrieveDDDW(DwMain, "productgroup_code", pbl, null);
                DwUtil.RetrieveDDDW(DwMain, "unit_code", pbl, null);
              
                //
                DwUtil.RetrieveDDDW(DwMain, "store_id", pbl, null);

                //string store_id = DwMain.GetItemString(1, "store_id");
                
                //DwUtil.RetrieveDDDW(DwMain, "product_set_flag", pbl, null);
                //DwUtil.RetrieveDDDW(DwDetail, "store_id", pbl, null);
                //DwUtil.RetrieveDDDW(DwDetail, "taxtype_code", pbl, null);
                //DwUtil.RetrieveDDDW(DwTailer, "price_lelvel", pbl, null);                
                
            }
            catch { }
            tDwTailer.Eng2ThaiAllRow();
            tDwDetail.Eng2ThaiAllRow();
            DwMain.SaveDataCache();
            DwDetail.SaveDataCache();
            DwTailer.SaveDataCache();
        }

        private void InitProduct()
        {
            try
            {
                
                String product_no = DwMain.GetItemString(1, "product_no");
                String sql = "select product_no from tdproductmaster where product_no = '" + product_no + "' and coop_id = '" + state.SsCoopId + "'";
                Sta ta = new Sta(new DwTrans().ConnectionString);
                Sdt dt = ta.Query(sql);

                if (dt.Next())
                {                    
                    try
                    {                
                        str_tradesrv_oper astr_tradwsrv_opr = new str_tradesrv_oper();
                        astr_tradwsrv_opr.xml_header = DwMain.Describe("DataWindow.Data.XML");
                        astr_tradwsrv_opr.xml_detail = DwDetail.Describe("DataWindow.Data.XML");
                        astr_tradwsrv_opr.xml_tailer = DwTailer.Describe("DataWindow.Data.XML");

                        
                            Int16 resultStock = tradingService.of_init_stockcard(state.SsWsPass, ref astr_tradwsrv_opr);
                            if (resultStock == 1)
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
                    catch (Exception ex)
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                    }
                }
                else
                {
                    DwMain.Reset();
                    DwMain.InsertRow(0);
                    DwMain.SetItemString(1, "coop_id", state.SsCoopId);
                    DwDetail.Reset();
                    DwTailer.Reset();
                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่มีรหัสสินค้า " + product_no);
                }
            }
            catch
            {

            }
        }

        protected void stro_()
        {
            try
            {
                String store = "";
                try
                {
                    store = DwMain.GetItemString(1, "store_id");
                }
                catch {}
                if (store == "")
                {
                    store = "01";
                    DwMain.SetItemSqlString(1, "store_id", store);
                }

                String product_no = DwMain.GetItemString(1, "product_no");
                    
                
                String sql = "select  sc.product_no  from 	tdstockcard sc, tdproductmaster sm  where sm.product_no ='" + product_no + "' and sc.product_no = '" + product_no + "' and	store_id ='" + store + "' ";
                //Sta ta = new Sta(new DwTrans().ConnectionString);
                Sdt dt = WebUtil.QuerySdt(sql);

                if (dt.Next())
                {
                    try
                    {
                        str_tradesrv_oper astr_tradwsrv_opr = new str_tradesrv_oper();
                        astr_tradwsrv_opr.xml_header = DwMain.Describe("DataWindow.Data.XML");
                        astr_tradwsrv_opr.xml_detail = DwDetail.Describe("DataWindow.Data.XML");
                        astr_tradwsrv_opr.xml_tailer = DwTailer.Describe("DataWindow.Data.XML");


                        Int16 resultStock = tradingService.of_init_stockcard(state.SsWsPass, ref astr_tradwsrv_opr);
                        if (resultStock == 1)
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
                    catch (Exception ex)
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                    }
                }
                    
                else
                {
                    //String product_no = DwMain.GetItemString(1, "product_no");
                    String sql1 = "select product_no from tdproductmaster where product_no = '" + product_no + "' and coop_id = '" + state.SsCoopId + "'";
                    Sta ta = new Sta(new DwTrans().ConnectionString);
                    Sdt dt1 = ta.Query(sql1);
                    if (dt1.Next())
                    {
                        str_tradesrv_oper astr_tradwsrv_opr = new str_tradesrv_oper();
                        astr_tradwsrv_opr.xml_header = DwMain.Describe("DataWindow.Data.XML");
                        astr_tradwsrv_opr.xml_detail = DwDetail.Describe("DataWindow.Data.XML");
                        astr_tradwsrv_opr.xml_tailer = DwTailer.Describe("DataWindow.Data.XML");
                        Int16 resultStock = tradingService.of_init_stockcard(state.SsWsPass, ref astr_tradwsrv_opr);
                        if (resultStock == 1)
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
                    //////////////////
                    else
                    {
                        DwMain.Reset();
                        DwMain.InsertRow(0);
                        DwMain.SetItemString(1, "coop_id", state.SsCoopId);
                        DwDetail.Reset();
                        DwTailer.Reset();
                        LtServerMessage.Text = WebUtil.ErrorMessage("ไม่มีรหัสสินค้า " + product_no);
                    }
                    
                }

                }
            

            catch
            { } 
        
        }


      
    }
}