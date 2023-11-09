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
using CoreSavingLibrary.WcfTrading;
using Sybase.DataWindow;
using DataLibrary;

namespace Saving.Applications.trading
{
    public partial class w_sheet_td_opr_is : PageWebSheet, WebSheet
    {
        string pbl = "sheet_td_opr_is.pbl";
        private DwThDate tDwMain;

        private TradingClient tradingService;
        private string sliptype_code = "IS";
        protected String jsProductNo;
        protected String jsPostSlipNo;
        protected String jsBSlipNo;
        protected String jsDwDetailInsertRow;

        public void InitJsPostBack()
        {
            tDwMain = new DwThDate(DwMain);
            tDwMain.Add("slip_date", "slip_tdate");
            jsProductNo = WebUtil.JsPostBack(this, "jsProductNo");
            jsPostSlipNo = WebUtil.JsPostBack(this, "jsPostSlipNo");            
            jsDwDetailInsertRow = WebUtil.JsPostBack(this, "jsDwDetailInsertRow");
            jsBSlipNo = WebUtil.JsPostBack(this, "jsBSlipNo");
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
                return;
            }
            if (!IsPostBack)
            {
                DwMain.InsertRow(0);
                DwMain.SetItemString(1, "coop_id", state.SsCoopId);
                DwMain.SetItemString(1, "entry_id", state.SsUsername);
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
                case "jsBSlipNo":
                    initRef();
                    break;
                case "jsDwDetailInsertRow":
                    DwDetailInsertRow();
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
                Int16 result = tradingService.of_save_op_slip(state.SsWsPass, ref astr_tradwsrv_opr);
                if (result == 1)
                {
                    DwMain.Reset();
                    DwDetail.Reset();
                    DwUtil.ImportData(astr_tradwsrv_opr.xml_header, DwMain, null, FileSaveAsType.Xml);
                    DwUtil.ImportData(astr_tradwsrv_opr.xml_detail, DwDetail, null, FileSaveAsType.Xml);
                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
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

        private void InitSlip()
        {
            try
            {
                //DwMain.SetItemString(1, "branch_id", state.SsCoopId);
                //DwMain.SetItemString(1, "sliptype_code", sliptype_code);
                str_tradesrv_oper astr_tradwsrv_req = new str_tradesrv_oper();
                astr_tradwsrv_req.xml_header = DwMain.Describe("DataWindow.Data.XML");
                astr_tradwsrv_req.xml_detail = DwDetail.Describe("DataWindow.Data.XML");
                Int16 result = tradingService.of_init_op_slip(state.SsWsPass, ref astr_tradwsrv_req);
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
                try // edit by ham (ยิง store_id)
                {
                    DwDetail.SetItemString(1, "store_id", DwMain.GetItemString(1, "store_id"));

                }
                catch
                {
                    DwDetail.InsertRow(0);
                    DwDetail.SetItemString(1, "store_id", DwMain.GetItemString(1, "store_id"));

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
                astr_tradwsrv_req.xml_list = "IS";
                Int16 result = tradingService.of_init_info_slipref(state.SsWsPass, ref astr_tradwsrv_req);
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

        private void DwDetailInsertRow()
        {
            try
            {
                int row = DwDetail.InsertRow(0);
                DwDetail.SetItemString(row, "coop_id", state.SsCoopId);
                DwDetail.SetItemDecimal(row, "seq_no", row);
           //     DwDetail.SetItemString(row, "store_id", DwMain.GetItemString(1, "store_id"));
                DwDetail.SetItemString(row, "sliptype_code", sliptype_code);
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }
    }
}