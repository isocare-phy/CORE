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
using Sybase.DataWindow;
using DataLibrary;


namespace Saving.Applications
{
    public partial class w_sheet_td_fin_apcredit : PageWebSheet, WebSheet
    {
        string pbl = "sheet_td_fin_apcredit.pbl";
        private string sliptype_code = "APC";
        private DwThDate tDwMain;
        private DwThDate tDwDetail;

        protected String jsCredNo;
        protected String jsPostSlip;
        protected String jsDwDetailInsertRow;
        protected String jsChickChkAll;

        private TradingClient tradingService;

        public void InitJsPostBack()
        {
            tDwMain = new DwThDate(DwMain);
            tDwMain.Add("creditdoc_date", "creditdoc_tdate");

            tDwDetail = new DwThDate(DwDetail);
            tDwDetail.Add("refdoc_date", "refdoc_tdate");
            tDwDetail.Add("due_date", "due_tdate");

            jsCredNo = WebUtil.JsPostBack(this, "jsCredNo");
            jsDwDetailInsertRow = WebUtil.JsPostBack(this, "jsDwDetailInsertRow");
            jsPostSlip = WebUtil.JsPostBack(this, "jsPostSlip");
            jsChickChkAll = WebUtil.JsPostBack(this, "jsChickChkAll");
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
                case "jsCredNo":
                    initCred();
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
            }
        }

        public void SaveWebSheet()
        {
            try
            {
                //add//
                    string posttovc = "";
                    string creditdoc_no = DwMain.GetItemString(1,"creditdoc_no");
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
                        //LtServerMessage.Text = WebUtil.ErrorMessage("กรุณาระบุข้อมูลให้ครบ");
                        //ของเดิม
                        str_tradesrv_oper astr_tradwsrv_req = new str_tradesrv_oper();
                        astr_tradwsrv_req.xml_header = DwMain.Describe("DataWindow.Data.XML");
                        astr_tradwsrv_req.xml_detail = DwDetail.Describe("DataWindow.Data.XML");
                        Int16 result = tradingService.of_save_op_slip(state.SsWsPass, ref astr_tradwsrv_req);
                        if (result == 1)
                        {
                            DwMain.Reset();
                            DwDetail.Reset();
                            DwUtil.ImportData(astr_tradwsrv_req.xml_header, DwMain, tDwMain, FileSaveAsType.Xml);
                            DwUtil.ImportData(astr_tradwsrv_req.xml_detail, DwDetail, tDwDetail, FileSaveAsType.Xml);

                            try////add////
                            {
                                string voucher = "";

                                string sql1 = "select voucher_no from tddebtcreditdet where creditdoc_no = '" + creditdoc_no + "'";
                                Sdt td = WebUtil.QuerySdt(sql1);

                                if (td.Next())
                                {
                                    //posttovc = dt.GetString("posttovc");
                                    voucher = td.GetString("voucher_no");
                                }
                                
                                if (voucher != "")
                                {
                                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ'" + voucher + "'");
                                }
                                else
                                {
                                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
                                }
                                

                            }
                            catch { }
                        }
                        else
                        {
                            LtServerMessage.Text = WebUtil.ErrorMessage("เกิดข้อผิดพลาด ไม่สามารถบันทึกได้");
                        }//ของเดิม//เเค่นี้

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
            try
            {
                DwUtil.RetrieveDDDW(DwMain, "cred_type", pbl, null);
               // DwUtil.RetrieveDDDW(DwMain, "cred_group", pbl, null);
            }
            catch { }
            tDwMain.Eng2ThaiAllRow();
            tDwDetail.Eng2ThaiAllRow();

            DwMain.SaveDataCache();
            DwDetail.SaveDataCache();
        }
        public void initCred()
        {
            try
            {
                str_tradesrv_oper astr_tradwsrv_req = new str_tradesrv_oper();
                astr_tradwsrv_req.xml_header = DwMain.Describe("DataWindow.Data.XML");
                astr_tradwsrv_req.xml_detail = DwDetail.Describe("DataWindow.Data.XML");
                Int16 result = tradingService.of_init_info_cred(state.SsWsPass, ref astr_tradwsrv_req);
                if (result == 1)
                {
                    DwMain.Reset();
                    DwDetail.Reset();
                    DwUtil.ImportData(astr_tradwsrv_req.xml_header, DwMain, tDwMain, FileSaveAsType.Xml);                    
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
                LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอกรหัสเจ้าหนี้");
            }
        }
        private void DwDetailInsertRow()
        {
            try
            {
                int row = DwDetail.InsertRow(0);
                DwDetail.SetItemString(row, "coop_id", state.SsCoopId);
                DwDetail.SetItemDecimal(row, "seq_no", row);
 //               DwDetail.SetItemString(row, "sliptype_code", sliptype_code);
                DwDetail.SetItemString(DwDetail.RowCount, "chkflag", "Y");
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

        private void ChickChkAll()
        {
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
    }
}