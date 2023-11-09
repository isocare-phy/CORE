using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.Security;
using CoreSavingLibrary.WcfInvestment;
using Sybase.DataWindow;
using DataLibrary;
using System.Configuration;
using System.Data;
using System.Xml.Linq;
using CoreSavingLibrary.WcfCommon;

namespace Saving.Applications.investment
{
    public partial class w_sheet_reg_coop : PageWebSheet, WebSheet
    {
        protected string jsInsertRow;
        protected string jsOpenRow;
        protected string jsPostProvince;
        protected string jsPostDistrict;
        protected DwThDate tDwMain;
        short result;
        protected string pbl = "int_lcmember.pbl";
        private InvestmentClient InvestmentService;

        #region Websheet menber
        public void InitJsPostBack()
        {
            jsPostProvince = WebUtil.JsPostBack(this, "jsPostProvince");
            jsPostDistrict = WebUtil.JsPostBack(this, "jsPostDistrict");
            jsInsertRow = WebUtil.JsPostBack(this, "jsInsertRow");
            jsOpenRow = WebUtil.JsPostBack(this, "jsOpenRow");
            tDwMain = new DwThDate(DwMain, this);
            tDwMain.Add("coopregis_date", "coopregis_tdate");


        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                DwMain.InsertRow(0);
                DwMain.SetItemDate(1,"coopregis_date", state.SsWorkDate);
                DwMain.SetItemString(1, "coop_id", state.SsCoopId);      
            }
            else
            {
                this.RestoreContextDw(DwMain, tDwMain);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            switch (eventArg)
            {
                case "jsInsertRow":
                    JsInsertRow();
                    break;
                case "jsOpenRow":
                    JsOpenRow();
                    break;
                case "jsPostDistrict":
                    DwMain.SetItemString(1, "tambol_code", "");
                    DwMain.SetItemString(1, "addr_postcode", "");
                    break;
                case "jsPostProvince":
                    DwMain.SetItemString(1, "district_code", "");
                    DwMain.SetItemString(1, "tambol_code", "");
                    DwMain.SetItemString(1, "addr_postcode", "");
                    break;
            }


        }

        public void SaveWebSheet()
        {
            JsInsertRow();
        }

        public void WebSheetLoadEnd()
        {
            try
            {
                DwUtil.RetrieveDDDW(DwMain, "prename_code", pbl, null);
                DwUtil.RetrieveDDDW(DwMain, "province_code", pbl, null);
                DwUtil.RetrieveDDDW(DwMain, "lntrnfinbankacc_code", pbl, null);
                try
                {
                    DwMain.SetItemString(1, "addr_postcode", "");
                    String pvCode = DwUtil.GetString(DwMain, 1, "province_code", "");

                    if (pvCode != "")
                    {
                        DwUtil.RetrieveDDDW(DwMain, "district_code", pbl, pvCode);
                        String dtCode = DwUtil.GetString(DwMain, 1, "district_code", "");

                        if (dtCode != "")
                        {
                            try
                            {
                                DataWindowChild dc = DwMain.GetChild("district_code");
                                int rPostCode = dc.FindRow("DISTRICT_CODE='" + dtCode + "'", 1, dc.RowCount);
                                String postCode = DwUtil.GetString(dc, rPostCode, "postcode", "");
                                DwMain.SetItemString(1, "addr_postcode", postCode);
                                DwMain.SetItemString(1, "province_code", pvCode);
                                DwMain.SetItemString(1, "district_code", dtCode);
                            }
                            catch (Exception ex)
                            {
                                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                            }

                            try
                            {
                                DwUtil.RetrieveDDDW(DwMain, "tambol_code", pbl, dtCode);
                                String tbCode = DwUtil.GetString(DwMain, 1, "tambol_code", "");
                            }
                            catch (Exception ex)
                            {
                                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                            }
                        }
                    }
                }
                catch { }
            }
            catch { }
            tDwMain.Eng2ThaiAllRow();
            DwMain.SaveDataCache();
        }
        #endregion

        public void JsInsertRow()
        {
            try
            {
                InvestmentService = wcf.Investment;
            }
            catch
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ติดต่อ Web Service ไม่ได้");
            }
            try
            {
                
                str_lcmember lcmember = new str_lcmember();
                lcmember.member_no = DwMain.GetItemString(1,"member_no");
                lcmember.coop_id = state.SsCoopId;
                lcmember.xml_lcmember = DwMain.Describe("DataWindow.Data.XML");
                if (!DwMain.GetItemString(1,"memb_name").Equals(""))
                {
                    result = InvestmentService.of_save_lcmember(state.SsWsPass, ref lcmember);
                }
                if (result == 1)
                {
                    DwMain.Reset();
                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
                    DwMain.InsertRow(0);
                    DwMain.SetItemDate(1, "coopregis_date", state.SsWorkDate);
                }
                else
                    LtServerMessage.Text = WebUtil.ErrorMessage("บันทึกไม่สำเร็จ");
            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
        }

        public void JsOpenRow()
        {
            try
            {
                InvestmentService = wcf.Investment;
            }
            catch
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ติดต่อ Web Service ไม่ได้");
            }
            try
            {

                str_lcmember lcmember = new str_lcmember();
                lcmember.member_no = DwMain.GetItemString(1, "member_no");
                lcmember.coop_id = state.SsCoopId;
                result = InvestmentService.of_open_lcmember(state.SsWsPass, ref lcmember);
                if (result == 1)
                {
                    if (lcmember.xml_lcmember != null)
                    {
                        DwMain.Reset();
                        DwMain.ImportString(lcmember.xml_lcmember, Sybase.DataWindow.FileSaveAsType.Xml);
                       // DwMain.InsertRow(0);
                    }
                    //LtServerMessage.Text = WebUtil.CompleteMessage("เปิดไฟล์สำเร็จ");
                }
                //else
                    //LtServerMessage.Text = WebUtil.ErrorMessage("เปิดไฟล์สำเร็จ");
            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }

        }

    }
}

