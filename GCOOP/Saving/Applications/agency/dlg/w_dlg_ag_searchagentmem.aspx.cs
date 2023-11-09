using System;
using CoreSavingLibrary;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using CoreSavingLibrary.WcfAccount;
using CoreSavingLibrary.WcfCommon;
using Sybase.DataWindow;
using CoreSavingLibrary.WcfAgency;
using DataLibrary;

namespace Saving.Applications.agency.dlg
{
    public partial class w_dlg_ag_searchagentmem : PageWebDialog, WebDialog
    {
        private AgencyClient AgencyService;
        protected String postSearch;
        protected String postSelectRow;
        protected String postFormatMember;
        //========================
        private void JspostYearPeriod()
        {
            String YearPeriod = "";
            String T_year, T_period;
            int year, period;

            Sta ta = new Sta(sqlca.ConnectionString);
            try
            {

                String sql = @"select max(DISTINCT recv_period) from agagentmaster";
                Sdt dt = ta.Query(sql);

                if (dt.Next())
                {
                    YearPeriod = dt.GetString("max(DISTINCTRECV_PERIOD)");
                    if (YearPeriod == null || YearPeriod == "")
                    {
                        year = DateTime.Now.Year;
                        period = DateTime.Now.Month;
                        year = year + 543;
                        T_year = Convert.ToString(year);
                        T_period = Convert.ToString(period);
                        YearPeriod = T_year + T_period;
                        Dw_main.SetItemString(1, "recv_period", YearPeriod);
                    }
                    else
                    {
                        Dw_main.SetItemString(1, "recv_period", YearPeriod);
                    }
                }
                else
                {
                    sqlca.Rollback();
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
            }
        }
        private void JsRetrieveDDW()
        {
            DwUtil.RetrieveDDDW(Dw_main, "recv_period", "agent.pbl", null);
            DwUtil.RetrieveDDDW(Dw_main, "agentgrp_code", "agent.pbl", null);
            DwUtil.RetrieveDDDW(Dw_main, "membgroup_code", "agent.pbl", null);
        }

        private void JsNewClear()
        {
            Dw_main.Reset();
            Dw_main.InsertRow(0);
            Dw_detail.Reset();
            JspostYearPeriod();
            JsRetrieveDDW();
        }
               
        private void JspostSelectRow()
        {
            int RowClickList = int.Parse(HdRowClick.Value);
            Dw_detail.SelectRow(0, false);
            Dw_detail.SelectRow(RowClickList, true);
            Dw_detail.SetRow(RowClickList);
        }

        private void JspostSearch()
        {            
                try
                {
                    str_agent astr_agent = new str_agent();
                    astr_agent.xml_head = Dw_main.Describe("DataWindow.Data.XML");
                    astr_agent.xml_list = Dw_detail.Describe("DataWindow.Data.XML");
                    int result = AgencyService.of_searchagentmem(state.SsWsPass, ref astr_agent);
                    if (result ==1 )
                    {

                        String list = astr_agent.xml_list;
                        Dw_detail.Reset();
                        Dw_detail.ImportString(astr_agent.xml_list, FileSaveAsType.Xml);
                    }
                    else
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบข้อมูล กรุณาตรวจสอบใหม่");
                        JsNewClear();
                    }
                }
                catch (Exception ex)
                {
                   // LtServerMessage.Text = WebUtil.ErrorMessage("เกิดข้อผิดพลาด " + ex);
                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบข้อมูล กรุณาทำรายการค้นหาใหม่");
                    JsNewClear();
                }
        }
        //========================
   

        public void InitJsPostBack()
        {
            postSearch = WebUtil.JsPostBack(this, "postSearch");
            postSelectRow = WebUtil.JsPostBack(this, "postSelectRow");
            postFormatMember = WebUtil.JsPostBack(this, "postFormatMember");
        }

        public void WebDialogLoadBegin()
        {
            try
            {
                AgencyService = wcf.Agency;
               
            }
            catch
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ติดต่อ Web Service ไม่ได้");
               
            }
            this.ConnectSQLCA();
           
          
            if (!IsPostBack)
            {
                JsNewClear();
            }
            else
            {
                this.RestoreContextDw(Dw_main);
                this.RestoreContextDw(Dw_detail);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postSearch")
            {
                JspostSearch();
            }
            else if (eventArg == "postSelectRow")
            {
                JspostSelectRow();
            }
            else if (eventArg == "postFormatMember")
            { 
            
            }
        }

        public void WebDialogLoadEnd()
        {
            if (Dw_main.RowCount > 1)
            {
                Dw_main.DeleteRow(Dw_main.RowCount);
            }
            Dw_main.SaveDataCache();
            Dw_detail.SaveDataCache();
        }

    }
}
