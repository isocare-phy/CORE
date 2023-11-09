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
    public partial class w_sheet_td_info_debt : PageWebSheet, WebSheet
    {
        private string pbl = "sheet_td_info_debt.pbl";
        private TradingClient tradingService;
        private DwThDate tDwStatement;
        protected String jsPostMembNo;
        protected String jsPostDebtNo;
        protected string postGetDistrict;//

        public void InitJsPostBack()
        {
            tDwStatement = new DwThDate(DwStatement);
            tDwStatement.Add("debtdet_date", "debtdet_tdate");
            jsPostDebtNo = WebUtil.JsPostBack(this, "jsPostDebtNo");
            jsPostMembNo = WebUtil.JsPostBack(this, "jsPostMembNo");
            postGetDistrict = WebUtil.JsPostBack(this, "postGetDistrict");//

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
            }
            else
            {
                this.RestoreContextDw(DwMain);
                this.RestoreContextDw(DwStatement, tDwStatement);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            switch (eventArg)
            {
                case "jsPostDebtNo":
                    InitDebt();
                    break;
                case "jsPostMembNo":
                    InitMemberNo();
                    break;
                case "postGetDistrict"://
                    GetDistrict();
                    break;
            }
        }

        public void SaveWebSheet()
        {
            try
            {
                str_tradesrv_oper astr_tradwsrv_opr = new str_tradesrv_oper();
                astr_tradwsrv_opr.xml_header = DwMain.Describe("DataWindow.Data.XML");
                astr_tradwsrv_opr.xml_detail = DwStatement.Describe("DataWindow.Data.XML");
                Int16 result = tradingService.of_save_info_debt_master(state.SsWsPass, ref astr_tradwsrv_opr);
                if (result == 1)
                {
                    DwMain.Reset();
                    DwStatement.Reset();
                    DwUtil.ImportData(astr_tradwsrv_opr.xml_header, DwMain, null, FileSaveAsType.Xml);
                    //DwUtil.ImportData(astr_tradwsrv_opr.xml_detail, DwStatement, tDwStatement, FileSaveAsType.Xml);
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
            tDwStatement.Eng2ThaiAllRow();
            DwMain.SaveDataCache();
            DwStatement.SaveDataCache();
          //  DwUtil.RetrieveDDDW(DwMain, "cred_group", pbl, null);
            DwUtil.RetrieveDDDW(DwMain, "debtfromtype_code", pbl, null);
            DwUtil.RetrieveDDDW(DwMain, "prename_code", pbl, null);
            DwUtil.RetrieveDDDW(DwMain, "price_level", pbl, null);
          //  DwUtil.RetrieveDDDW(DwMain, "payment_type", pbl, null);
            DwUtil.RetrieveDDDW(DwMain, "debt_district", pbl, null);
            DwUtil.RetrieveDDDW(DwMain, "debt_province", pbl, null);
            try
            {     //
                string debt_province = DwMain.GetItemString(1, "debt_province");
                DwUtil.RetrieveDDDW(DwMain, "debt_district", pbl, debt_province);
                //
            }
            catch { }
        }

        private void InitDebt()
        {
            try
            {
                String debt_no = DwMain.GetItemString(1, "debt_no");
                String sql = "select debt_no from tddebtmaster where debt_no = '" + debt_no + "' and coop_id = '" + state.SsCoopId + "'";
                Sta ta = new Sta(new DwTrans().ConnectionString);
                Sdt dt = ta.Query(sql);

                if (dt.Next())
                {
                    try
                    {
                        str_tradesrv_oper astr_tradwsrv_opr = new str_tradesrv_oper();
                        astr_tradwsrv_opr.xml_header = DwMain.Describe("DataWindow.Data.XML");
                        astr_tradwsrv_opr.xml_detail = DwStatement.Describe("DataWindow.Data.XML");
                        Int16 result = tradingService.of_init_info_debt_master(state.SsWsPass, ref astr_tradwsrv_opr);
                        if (result == 1)
                        {
                            DwMain.Reset();
                            DwStatement.Reset();
                            DwUtil.ImportData(astr_tradwsrv_opr.xml_header, DwMain, null, FileSaveAsType.Xml);
                            DwUtil.ImportData(astr_tradwsrv_opr.xml_detail, DwStatement, tDwStatement, FileSaveAsType.Xml);
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
                }
            }
            catch
            {
            }     
        }


        private void GetDistrict()
        {
            try
            {
                String ls_province;
                //ls_province = HfProvince.Value;
                // DwUtil.RetrieveDDDW(DwMain, "cred_district", pbl, ls_province);
                
            }
            catch
            {
            }

        }


        private void InitMemberNo()
        {
            try
            {
                str_tradesrv_oper astr_tradwsrv_opr = new str_tradesrv_oper();
                astr_tradwsrv_opr.xml_header = DwMain.Describe("DataWindow.Data.XML");
                Int16 result = tradingService.of_init_info_member(state.SsWsPass, ref astr_tradwsrv_opr);
                if (result == 1)
                {
                    DwMain.Reset();
                    DwUtil.ImportData(astr_tradwsrv_opr.xml_header, DwMain, null, FileSaveAsType.Xml);
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
    }
}