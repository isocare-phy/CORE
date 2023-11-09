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
    public partial class w_dlg_ag_searchagmemb : PageWebDialog, WebDialog 
    {

        private AgencyClient AgencyService;
        protected String postSearch;

        //========================
      

        private void JsNewClear()
        {
            Dw_main.Reset();
            Dw_main.InsertRow(0);
            Dw_detail.Reset();
        }

        private void JspostSearch()
        {
            try
            {
                str_agent astr_agent = new str_agent();
                astr_agent.xml_head = Dw_main.Describe("DataWindow.Data.XML");
                astr_agent.xml_list = Dw_detail.Describe("DataWindow.Data.XML");
                int result = AgencyService.SearchChangeMemb(state.SsWsPass, ref astr_agent);
                if (result == 1)
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
             //   LtServerMessage.Text = WebUtil.ErrorMessage("เกิดข้อผิดพลาด " + ex);
                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบข้อมูล กรุณาทำรายการค้นหาใหม่");
                JsNewClear();
            }
        }

        #region WebDialog Members

        public void InitJsPostBack()
        {
            postSearch = WebUtil.JsPostBack(this, "postSearch");
           
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
                DwUtil.RetrieveDDDW(Dw_main, "agentgrp_code", "egat_ag_searchagmemb.pbl", null);
                DwUtil.RetrieveDDDW(Dw_main, "membgroup_code", "egat_ag_searchagmemb.pbl", null);
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

        #endregion
    }
}
