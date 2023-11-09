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
using CoreSavingLibrary.WcfNCommon;
using CoreSavingLibrary.WcfNShrlon;
using Sybase.DataWindow;
using System.Globalization;
using System.Web.Services.Protocols;


namespace Saving.Applications.shrlon
{
    public partial class w_sheet_sl_approve_loancoll : PageWebSheet, WebSheet
    {
        private n_shrlonClient shrlonService;
        private DwThDate thDwMaster;
        protected String jsPostcoopId;

        private DwThDate tDwMain;
        protected String jsfillter;
        protected String jsgenReqDocNo = "";
        #region WebSheet Members

        void WebSheet.InitJsPostBack()
        {


            jsgenReqDocNo = WebUtil.JsPostBack(this, "jsgenReqDocNo");
            jsPostcoopId = WebUtil.JsPostBack(this, "jsPostcoopId");
            jsfillter = WebUtil.JsPostBack(this, "jsfillter");
            //thDwMaster = new DwThDate(dw_master, this);
            //thDwMaster.Add("loanrequest_date", "loanrequest_tdate");
            //thDwMaster.Add("loanrcvfix_date", "loanrcvfix_tdate");

            tDwMain = new DwThDate(dw_main, this);
            tDwMain.Add("loanrequest_date", "loanrequest_tdate");
        }

        void WebSheet.WebSheetLoadBegin()
        {
            try
            {
                shrlonService = wcf.NShrlon;
            }
            catch
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ติดต่อ Web Service ไม่ได้");
                return;
            }
            this.ConnectSQLCA();
            dw_master.SetTransaction(sqlca);
            if (IsPostBack)
            {
                this.RestoreContextDw(dw_master, null);
                this.RestoreContextDw(dw_main, tDwMain);
            }
            if (dw_master.RowCount < 1)
            {
                dw_main.InsertRow(0);
                DwUtil.RetrieveDDDW(dw_main, "coop_id", "sl_approve_loan.pbl", null);
                DwUtil.RetrieveDDDW(dw_main, "end_coopid", "sl_approve_loan.pbl", null);
                dw_main.SetItemString(1, "coop_id", state.SsCoopId);
                if (state.SsCoopControl == "001001")
                {
                    dw_main.SetItemString(1, "end_coopid", "001004");
                }
                else { dw_main.SetItemString(1, "end_coopid", state.SsCoopId); }
                dw_main.SetItemDateTime(1, "loanrequest_date", state.SsWorkDate);
                tDwMain.Eng2ThaiAllRow();
                this.InitLnReqList();
            }

        }
        private void JsPostcoopId()
        {
            try
            {
                DateTime loanrequest_date = dw_main.GetItemDateTime(1, "loanrequest_date");
                string coop_id = dw_main.GetItemString(1, "coop_id");
                string end_coopid = dw_main.GetItemString(1, "end_coopid");
                String reqListXML = shrlonService.of_initlist_lnreqapv(state.SsWsPass,state.SsCoopId,state.SsCoopId);
                dw_master.Reset();
                DwUtil.ImportData(reqListXML, dw_master, null, FileSaveAsType.Xml);
                string ls_sql = @"SELECT MBUCFPRENAME.PRENAME_DESC,   
         MBMEMBMASTER.MEMB_NAME,   
         MBMEMBMASTER.MEMB_SURNAME,   
         MBMEMBMASTER.MEMBGROUP_CODE,   
         LNREQLOAN.LOANREQUEST_DOCNO,   
         LNREQLOAN.MEMBER_NO,   
         LNREQLOAN.LOANTYPE_CODE,   
         LNREQLOAN.LOANREQUEST_DATE,   
         LNREQLOAN.LOANREQUEST_AMT,   
         LNREQLOAN.LOANPAYMENT_TYPE,   
         LNREQLOAN.PERIOD_PAYAMT,   
         LNREQLOAN.PERIOD_PAYMENT,   
         LNREQLOAN.BUYSHARE_FLAG,   
         LNREQLOAN.BUYSHARE_AMT,   
         LNREQLOAN.SUM_CLEAR,   
         LNREQLOAN.LOANOBJECTIVE_CODE,   
         LNREQLOAN.APPROVE_DATE,   
         LNREQLOAN.APPROVE_ID,   
         LNREQLOAN.LOANAPPROVE_AMT,   
         LNREQLOAN.LOANCONTRACT_NO,   
         LNREQLOAN.LOANREQUEST_STATUS,   
         LNREQLOAN.REF_REGISTERNO,   
         LNREQLOAN.COOP_ID,   
         LNREQLOAN.OD_FLAG,   
         LNREQLOAN.ENTRY_ID,   
         LNREQLOAN.LOANRCVFIX_DATE,   
         LNREQLOAN.INTCERTIFICATE_STATUS,   
         LNREQLOAN.INTENDORSE_AMT  
    FROM MBMEMBMASTER,   
         MBUCFPRENAME,   
         LNREQLOAN  
   WHERE ( MBMEMBMASTER.PRENAME_CODE = MBUCFPRENAME.PRENAME_CODE ) and  
         ( LNREQLOAN.MEMBER_NO = MBMEMBMASTER.MEMBER_NO ) and  
         ( MBMEMBMASTER.COOP_ID = LNREQLOAN.MEMCOOP_ID ) and  
         ( ( lnreqloan.loanrequest_status = 8 ) )   ";
  string ls_sqlext = " and  LNREQLOAN.COOP_ID between '" + coop_id + "' and '" + end_coopid + @"'  
and   LNREQLOAN.LOANRCVFIX_DATE =to_date('" + loanrequest_date.ToString("ddMMyyyy") + @"','ddmmyyyy') 
 ORDER BY LNREQLOAN.LOANREQUEST_DATE ASC,   
         MBMEMBMASTER.MEMBGROUP_CODE ASC,   
         MBMEMBMASTER.MEMBER_NO ASC  ";
                //dw_master.SetFilter("loantype_code in ('23','26') ");
                string sss = ls_sql + ls_sqlext;
                dw_master.SetSqlSelect(sss);
                dw_master.Retrieve();

                dw_master.SetFilter("loantype_code in ('23','26','24') ");
                dw_master.Filter();

            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
        }
        void WebSheet.CheckJsPostBack(string eventArg)
        {
            if (eventArg == "jsgenReqDocNo")
            {
                this.GenReqDocNo();
            }
            else if (eventArg == "jsPostcoopId")
            {
                JsPostcoopId();
            }
            else if (eventArg == "jsfillter")
            {
                try
                {
                    //DateTime loanrequest_date = dw_main.GetItemDateTime(1, "loanrequest_date");
                    //string coop_id = dw_main.GetItemString(1, "coop_id");
                    //string end_coopid = dw_main.GetItemString(1, "end_coopid");
                    //String reqListXML = shrlonService.Initlist_lnreqapv(state.SsWsPass, coop_id, end_coopid);
                    //dw_master.Reset();
                    //DwUtil.ImportData(reqListXML, dw_master, null, FileSaveAsType.Xml);
                    //object[] args = new object[] { coop_id, end_coopid };
                    //DwUtil.RetrieveDataWindow(dw_master, "sl_approve_loan.pbl", null, args);

                    JsPostcoopId();
                }
                catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
            }


        }

        void WebSheet.SaveWebSheet()
        {
            try
            {
                String ls_xml_main = dw_master.Describe("DataWindow.Data.XML");

                Int32 ls_xml = shrlonService.of_saveapv_lnreq(state.SsWsPass, ls_xml_main, "", state.SsCoopControl, state.SsWorkDate);
                InitLnReqList();

            }
            catch (SoapException ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(WebUtil.SoapMessage(ex));
            }
            catch (Exception ex)
            {
                // LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
            }


        }

        void WebSheet.WebSheetLoadEnd()
        {
            DwUtil.RetrieveDDDW(dw_master, "loantype_code", "sl_approve_loan.pbl", null);
            DwUtil.RetrieveDDDW(dw_main, "coop_id", "sl_approve_loan.pbl", null);
            DwUtil.RetrieveDDDW(dw_main, "end_coopid", "sl_approve_loan.pbl", null);
            if (dw_main.RowCount > 1)
            {

                DwUtil.DeleteLastRow(dw_main);
            }
            sqlca.Disconnect();
            dw_master.SaveDataCache();
            dw_main.SaveDataCache();
        }

        #endregion

        private void InitLnReqList()
        {
            try
            {

                string coop_id = dw_main.GetItemString(1, "coop_id");
                string end_coopid = dw_main.GetItemString(1, "end_coopid");
                String reqListXML = shrlonService.of_initlist_lnreqapv(state.SsWsPass,state.SsCoopId,state.SsCoopId);
                DateTime loanrequest_date = dw_main.GetItemDateTime(1, "loanrequest_date");
                dw_master.Reset();
                DwUtil.ImportData(reqListXML, dw_master, null, FileSaveAsType.Xml);
                dw_master.SetFilter("loantype_code in ('23','24','26') ");
                dw_master.Filter();
            }

            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่มีข้อมูลรอทำรายการ");
            }
        }

        private void GenReqDocNo()
        {
            int count = dw_master.RowCount;
            for (int i = 0; i < count; i++)
            {
                String lncont_no = "";
                try { lncont_no = dw_master.GetItemString(i + 1, "loancontract_no"); }
                catch { lncont_no = ""; }

                String lncont_status = dw_master.GetItemString(i + 1, "loanrequest_status");
                if (lncont_status == "1")
                {
                    if (lncont_no == "")
                    {
                        String loantype_code = dw_master.GetItemString(i + 1, "loantype_code").Trim();
                        String newReqDocNo = shrlonService.of_gennewcontractno(state.SsWsPass, state.SsCoopControl, loantype_code);

                        dw_master.SetItemString(i + 1, "loancontract_no", newReqDocNo);
                        //  dw_master.SetItemDateTime(i + 1, "approve_date", state.SsWorkDate);
                    }
                }
            }

        }
    }
}
