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
    public partial class w_sheet_td_fin_arcredit : PageWebSheet, WebSheet
    {
        string pbl = "sheet_td_fin_arcredit.pbl";
        private string sliptype_code = "ARC";
        private DwThDate tDwMain;
        private DwThDate tDwDetail;

        protected String jsDebtNo;
        protected String jsPostSlip;
        protected String jsDwDetailInsertRow;
        protected String jsChickChkAll;
        protected String jschkCalpay;

        private TradingClient tradingService;

        public void InitJsPostBack()
        {
            tDwMain = new DwThDate(DwMain);
            tDwMain.Add("creditdoc_date", "creditdoc_tdate");

            tDwDetail = new DwThDate(DwDetail);
            tDwDetail.Add("refdoc_date", "refdoc_tdate");
            tDwDetail.Add("due_date", "due_tdate");

            jsDebtNo = WebUtil.JsPostBack(this, "jsDebtNo");
            jsDwDetailInsertRow = WebUtil.JsPostBack(this, "jsDwDetailInsertRow");
            jsPostSlip = WebUtil.JsPostBack(this, "jsPostSlip");
            jsChickChkAll = WebUtil.JsPostBack(this, "jsChickChkAll");
            jschkCalpay = WebUtil.JsPostBack(this, "jschkCalpay");

            DwUtil.RetrieveDDDW(DwMain, "debt_type", pbl, null);
            DwUtil.RetrieveDDDW(DwMain, "doc_reason", pbl, null);
 
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
                    DwMain.SetItemString(1, "sliptype_code", sliptype_code);
                    DwMain.SetItemDateTime(1, "creditdoc_date", state.SsWorkDate);
                    //int row = DwDetail.InsertRow(0);
                    //DwDetail.SetItemString(row, "branch_id", state.SsCoopId);
                    //DwDetail.SetItemDecimal(row, "seq_no", row);
                    //DwDetail.SetItemString(row, "sliptype_code", sliptype_code);
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
                case "jsDebtNo":
                    initDebt();
                    break;
                case "jsDwDetailInsertRow":
                    DwDetailInsertRow();
                    break;
                case "jsPostSlip":
                    InitSlip();
                    break;
                case "jsChickChkAll":
                    ChickChkAll();
                    break;
                case "jschkCalpay":
                    chkCalpay();
                    break;
            }
        }
        public void SaveWebSheet()
        {
            try
            {
               //add//
                string posttovc = "";
                string creditdoc_no = DwMain.GetItemString(1, "creditdoc_no");
                try
                {
                    string Sql = "select posttovc from tddebtcreditdet where creditdoc_no = '" + creditdoc_no + "'";
                    Sdt dt = WebUtil.QuerySdt(Sql);

                    if (dt.Next())
                    {
                        posttovc = dt.GetString("posttovc");
                    }
                }
                catch { }

                if (posttovc != "1")
                {

                    str_tradesrv_oper astr_tradwsrv_req = new str_tradesrv_oper();
                    astr_tradwsrv_req.xml_header = DwMain.Describe("DataWindow.Data.XML");
                    astr_tradwsrv_req.xml_detail = DwDetail.Describe("DataWindow.Data.XML");
                    Int16 result = tradingService.of_save_op_slip(state.SsWsPass, ref astr_tradwsrv_req);
                    if (result == 1)
                    {
                        DwMain.Reset();
                        DwUtil.ImportData(astr_tradwsrv_req.xml_header, DwMain, tDwMain, FileSaveAsType.Xml);
                        DwDetail.Reset();
                        DwUtil.ImportData(astr_tradwsrv_req.xml_detail, DwDetail, tDwDetail, FileSaveAsType.Xml);
                        LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
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
                //
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        public void WebSheetLoadEnd()
        {
            tDwMain.Eng2ThaiAllRow();
            tDwDetail.Eng2ThaiAllRow();

            DwMain.SaveDataCache();
            DwDetail.SaveDataCache();
        }
        private void DwDetailInsertRow()
        {
            try
            {
                int row = DwDetail.InsertRow(0);
                DwDetail.SetItemString(row, "coop_id", state.SsCoopId);
                DwDetail.SetItemDecimal(row, "seq_no", row);
                //DwDetail.SetItemString(row, "sliptype_code", sliptype_code);
                DwDetail.SetItemString(DwDetail.RowCount, "chkflag", "Y");
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }
        private void ChickChkAll()
        {
            decimal debt_amt =0;
            try
            {
                int rowcount = DwDetail.RowCount;
                bool chk = chkAll.Checked;
                string checkflag;
                if (chk)
                {
                    checkflag = "Y";
                }
                else
                {
                    checkflag = "N";
                }

                for (int i = 1; i <= rowcount; i++)
                {
                    DwDetail.SetItemString(i, "chkflag", checkflag);
                    try
                    {
                        debt_amt = DwDetail.GetItemDecimal(i, "debt_amt");
                    }
                    catch
                    {
                        debt_amt = 0;
                    }
                     debt_amt =+ debt_amt;
                     DwDetail.SetItemString(i, "chkflag", checkflag);
                     DwMain.SetItemDecimal(1, "debt_amt", debt_amt);

                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        private void AddChkAll()
        {
            try
            {
                int rowcount = DwDetail.RowCount;  

                for (int i = 1; i <= rowcount; i++)
                {
                    DwDetail.SetItemString(i, "chkflag", "Y");
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        // ตั้งหนี้เดิม
        private void InitSlip()
        {
            try
            {
                str_tradesrv_oper astr_tradwsrv_req = new str_tradesrv_oper();
                astr_tradwsrv_req.xml_header = DwMain.Describe("DataWindow.Data.XML");
                astr_tradwsrv_req.xml_detail = DwDetail.Describe("DataWindow.Data.XML");
                Int16 result = tradingService.of_init_op_slip(state.SsWsPass, ref astr_tradwsrv_req);
                if (result == 1)
                {
                    DwMain.Reset();
                    DwUtil.ImportData(astr_tradwsrv_req.xml_header, DwMain, tDwMain, FileSaveAsType.Xml);
                    DwDetail.Reset();
                    DwUtil.ImportData(astr_tradwsrv_req.xml_detail, DwDetail, tDwDetail, FileSaveAsType.Xml);
                    AddChkAll();
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
        private void initDebt()
        {
            try
            {
                str_tradesrv_oper astr_tradwsrv_req = new str_tradesrv_oper();
                astr_tradwsrv_req.xml_header = DwMain.Describe("DataWindow.Data.XML");
                astr_tradwsrv_req.xml_detail = DwDetail.Describe("DataWindow.Data.XML");
                Int16 result = tradingService.of_init_info_debt(state.SsWsPass, ref astr_tradwsrv_req);
                if (result == 1)
                {
                    DwMain.Reset();
                    DwUtil.ImportData(astr_tradwsrv_req.xml_header, DwMain, tDwMain, FileSaveAsType.Xml);

                    DwDetail.Reset();
                    DwUtil.ImportData(astr_tradwsrv_req.xml_detail, DwDetail, tDwDetail, FileSaveAsType.Xml);
                    AddChkAll();
                }
                else
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถดึงข้อมูลได้");
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอกรหัสลูกหนี้");
            }
        }

        private void chkCalpay()
        {
            decimal debt_amt = 0;
            try
            {
                int rowcount = DwDetail.RowCount;
                int row = Convert.ToInt32(HdRow.Value);
                string checkflag = DwDetail.GetItemString(row, "chkflag");
                if (checkflag =="Y")
                {
                    for (int i = 1; i <= rowcount; i++)
                    {
                        try
                        {
                            debt_amt = DwDetail.GetItemDecimal(i, "debt_amt");
                        }
                        catch
                        {
                            debt_amt = 0;
                        }
                        debt_amt = +debt_amt;
                        DwMain.SetItemDecimal(1, "debt_amt", debt_amt);
                    }
                }
                else
                {
                    //for (int i = 1; i <= rowcount; i++)
                    //{

                    //    debt_amt = DwDetail.GetItemDecimal(i, "debt_amt");
                    //    debt_amt = -debt_amt;
                    DwMain.SetItemDecimal(1, "debt_amt", 0);

                    //}
                }


            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

    }
}