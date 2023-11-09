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
using CoreSavingLibrary.WcfAgency;
using Sybase.DataWindow;

namespace Saving.Applications.agency
{
    public partial class w_sheet_ag_clearagent : PageWebSheet, WebSheet
    {
        protected String checkAll;
        protected String initClearAgent;

        private AgencyClient agenService;
        protected str_agent strAgent;

        #region WebSheet Members

        public void InitJsPostBack()
        {
            checkAll = WebUtil.JsPostBack(this, "checkAll");
            initClearAgent = WebUtil.JsPostBack(this, "initClearAgent");
        }

        public void WebSheetLoadBegin()
        {
            strAgent = new str_agent();
            try
            {
                agenService = wcf.Agency;
            }
            catch
            { }
            if (!IsPostBack)
            {
                DwMain.InsertRow(0);
            }
            else
            {
                this.RestoreContextDw(DwMain);
                this.RestoreContextDw(DwDetail);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "checkAll")
            {
                JsCheckAll();
            }
            else if (eventArg == "initClearAgent")
            {
                JsInitClearAgent();
            }
        }

        public void SaveWebSheet()
        {
            strAgent.xml_head = DwMain.Describe("DataWindow.Data.XML");
            strAgent.xml_detail = DwDetail.Describe("DataWindow.Data.XML");
            try
            {
                agenService.SaveClearAgent(state.SsWsPass, strAgent);
                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลเสร็จเรียบร้อยแล้ว...");
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        public void WebSheetLoadEnd()
        {
            WebUtil.RetrieveDDDW(DwMain, "recv_period", "ag_clearagent.pbl", null);
            WebUtil.RetrieveDDDW(DwMain, "sagentgrp_code", "ag_clearagent.pbl", null);
            WebUtil.RetrieveDDDW(DwMain, "eagentgrp_code", "ag_clearagent.pbl", null);
            DwMain.SaveDataCache();
            DwDetail.SaveDataCache();
        }

        #endregion

        private void JsCheckAll()
        {
            Decimal Set = 1;
            Boolean Select = chkAll.Checked;

            if (Select == true)
            {
                Set = 1;
            }
            else if (Select == false)
            {
                Set = 0;
            }
            for (int i = 1; i <= DwDetail.RowCount; i++)
            {
                DwDetail.SetItemDecimal(i, "operate_flag", Set);
            }
        }

        private void JsInitClearAgent()
        {
            String recvPeriod = DwMain.GetItemString(1, "recv_period");
            String sGrpCode = DwMain.GetItemString(1, "sagentgrp_code");
            String eGrpCode = DwMain.GetItemString(1, "eagentgrp_code");
            String xmlDetail = "";
            try
            {
                strAgent.xml_head = DwMain.Describe("DataWindow.Data.XML");
                agenService.InitClearAgent(state.SsWsPass, ref strAgent);
                xmlDetail = strAgent.xml_detail;
                DwDetail.Reset();
                if (xmlDetail != "" && xmlDetail != null)
                {
                    DwUtil.ImportData(xmlDetail, DwDetail, null, FileSaveAsType.Xml);
                }
                else
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่มีข้อมูลรายการงวดที่ : " + recvPeriod + "สังกัด :" + sGrpCode + " - " + eGrpCode);
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }
    }
}
