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
    public partial class w_sheet_ent_coop_save : PageWebSheet, WebSheet
    {
        short result;
        protected string pbl = "int_lcmember.pbl";
        private InvestmentClient InvestmentService;
        protected string jsInsertRow;
        protected string jsInitRow;
        protected string jsOpenRow;
        protected string jsDwDetailInsertRow;
        static DateTime DateYear = DateTime.Now;
        short biz_year = Convert.ToInt16(DateYear.Year + 543);  
        public void InitJsPostBack()
        {
            jsInsertRow = WebUtil.JsPostBack(this, "jsInsertRow");
            jsInitRow = WebUtil.JsPostBack(this, "jsInitRow");
            jsOpenRow = WebUtil.JsPostBack(this, "jsOpenRow");
            jsDwDetailInsertRow = WebUtil.JsPostBack(this, "jsDwDetailInsertRow");
        }
        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                DwMain.InsertRow(0);
                DwMain.SetItemDecimal(1, "biz_year", biz_year); 
                DwDetail.InsertRow(0);
                DwUtil.RetrieveDDDW(DwDetail, "bdrank_code", pbl, "");   
            }
            else
            {
                this.RestoreContextDw(DwMain);
                this.RestoreContextDw(DwDetail);
            }
        
        }
        public void CheckJsPostBack(string eventArg) 
        {

            switch (eventArg) {
                case "jsInsertRow":
                    JsInsertRow();
                    break;
                case "jsInitRow":
                    JsInitRow();
                    break;
                case "jsDwDetailInsertRow":
                    DwDetailInsertRow();
                    DwDetail.ScrollLastPage();
                    break;
            }
        
        }
        public void SaveWebSheet() 
        {
            JsInsertRow();
        }
        public void WebSheetLoadEnd()
        {
            DwMain.SaveDataCache();
            DwDetail.SaveDataCache();
        }

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
               
                
                lcmember.coop_id = state.SsCoopId;
               // lcmember.member_no = DwMain.GetItemString(1, "member_no");
             //   lcmember.biz_year = Convert.ToInt16(DwMain.GetItemString(1, "biz_year"));
                lcmember.entry_id = "OH";
                lcmember.xml_lcyearbiz = DwMain.Describe("DataWindow.Data.XML");
                lcmember.xml_lcyearboard = DwDetail.Describe("DataWindow.Data.XML");
                result = InvestmentService.of_save_lcdetyear(state.SsWsPass, lcmember);
                if (result == 1)
                {
                    DwMain.Reset();
                    DwDetail.Reset();
                    if (DwMain.Describe("DataWindow.Data.XML") != null && DwDetail.Describe("DataWindow.Data.XML")!= null)
                    {
                        LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
                        DwMain.InsertRow(0);
                        DwMain.SetItemDecimal(1, "biz_year", biz_year);
                        DwDetail.InsertRow(0);
                        DwDetail.InsertRow(0);
                        DwDetail.InsertRow(0);
                        DwDetail.InsertRow(0);
                        DwDetail.InsertRow(0);
                        DwUtil.RetrieveDDDW(DwDetail, "bdrank_code", pbl, "");  
                    }
                    
                }
                else
                    LtServerMessage.Text = WebUtil.ErrorMessage("บันทึกไม่สำเร็จ");
            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
        }
        public void JsInitRow()
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
                lcmember.biz_year = Convert.ToInt16(DwMain.GetItemDecimal(1, "biz_year"));
                try {
                    result = InvestmentService.of_init_lcdetyear(state.SsWsPass, ref lcmember);
                }
                catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
                if (result == 1)
                {


                    if (lcmember.xml_lcyearbiz != "")
                    {
                        DwMain.Reset();
                        DwMain.ImportString(lcmember.xml_lcyearbiz, Sybase.DataWindow.FileSaveAsType.Xml);
                        
                    }
                    if (lcmember.xml_lcyearboard !="") 
                    {
                        DwDetail.Reset();
                        DwDetail.ImportString(lcmember.xml_lcyearboard, Sybase.DataWindow.FileSaveAsType.Xml);
                    }
                    //LtServerMessage.Text = WebUtil.CompleteMessage("ตั้งค่าสำเร็จ");
                }
                else
                    LtServerMessage.Text = WebUtil.ErrorMessage("ตั้งค่าไม่สำเร็จ");
            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
        }
        public void DwDetailInsertRow()
        {
            DwDetail.InsertRow(0);
        }
    }
}