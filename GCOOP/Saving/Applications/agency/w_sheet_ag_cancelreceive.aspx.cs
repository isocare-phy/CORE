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
    public partial class w_sheet_ag_cancelreceive : PageWebSheet, WebSheet
    {
        protected String initCancelRecive;
        protected String calMemMain;
        private AgencyClient agenService;
        private DwThDate tDwDetail;
        protected str_agent strAgent;

        #region WebSheet Members

        public void InitJsPostBack()
        {
            initCancelRecive = WebUtil.JsPostBack(this, "initCancelRecive");
            calMemMain = WebUtil.JsPostBack(this, "calMemMain");
            tDwDetail = new DwThDate(DwDetail, this);
            tDwDetail.Add("cancel_day", "cancel_tday");
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
                DwDetail.InsertRow(0);
            }
            else
            {
                this.RestoreContextDw(DwMain);
                this.RestoreContextDw(DwDetail);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "initCancelRecive")
            {
                JsInitCancelRecive();
            }
            else if (eventArg == "calMemMain")
            {
                JsCalMemMain();
            }
        }

        public void SaveWebSheet()
        {
            DwDetail.SetItemString(1, "entry_id", state.SsUsername);
            DwDetail.SetItemString(1, "machine_id", state.SsClientIp);
            DwDetail.SetItemDateTime(1, "system_date", state.SsWorkDate);
            DwDetail.SetItemDateTime(1, "adj_time", DateTime.Now);
            
            strAgent.xml_head = DwMain.Describe("DataWindow.Data.XML");
            
            strAgent.xml_detail = DwDetail.Describe("DataWindow.Data.XML");
            try
            {
                agenService.SaveCancelReceive(state.SsWsPass, strAgent);
                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลเสร็จเรียบร้อยแล้ว...");
                DwMain.Reset();
                DwDetail.Reset();
                DwMain.InsertRow(0);
                DwDetail.InsertRow(0);
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        public void WebSheetLoadEnd()
        {
            WebUtil.RetrieveDDDW(DwMain, "recv_period", "ag_cancelreceive.pbl", null);
            WebUtil.RetrieveDDDW(DwDetail, "cause_code", "ag_cancelreceive.pbl", null);
            DwMain.SaveDataCache();
            DwDetail.SaveDataCache();
        }

        #endregion

        private void JsInitCancelRecive()
        {
            String xmlMain = "";
            String xmlDetail = "";
            String recvPeriod = DwMain.GetItemString(1, "recv_period");
            String memberNo = DwMain.GetItemString(1, "member_no");
            try
            {
                strAgent.xml_head = DwMain.Describe("DataWindow.Data.XML");
                agenService.InitCancelRecieve(state.SsWsPass, ref strAgent);
                xmlMain = strAgent.xml_head;
                xmlDetail = strAgent.xml_detail;
                DwMain.Reset();
                DwDetail.Reset();
                if (xmlMain != "" && xmlMain != null)
                {
                    DwUtil.ImportData(xmlMain, DwMain, null, FileSaveAsType.Xml);
                    DwUtil.ImportData(xmlDetail, DwDetail, null, FileSaveAsType.Xml);
                    DwDetail.SetItemDateTime(1, "cancel_day", state.SsWorkDate);
                    tDwDetail.Eng2ThaiAllRow();
                }
                else
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่มีข้อมูลงวดที่ : " + recvPeriod + " เลขทะเบียนที่ : " + memberNo);
                    DwMain.InsertRow(0);
                    DwDetail.InsertRow(0);
                }
                
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
            Decimal recvAmt = DwMain.GetItemDecimal(1, "recv_amt");
            Decimal retAllAmt = DwMain.GetItemDecimal(1, "ret_all_amt");
            Decimal adjAllAmt = DwMain.GetItemDecimal(1, "adj_all_amt");

            if (recvAmt > 0 || retAllAmt > 0 || adjAllAmt > 0)
            {
                DwMain.Reset();
                DwDetail.Reset();
                DwMain.InsertRow(0);
                DwDetail.InsertRow(0);
                LtServerMessage.Text = WebUtil.ErrorMessage("ข้อมูลงวดที่ : " + recvPeriod + " เลขทะเบียนที่ : " + memberNo + " ไม่สามารถทำการยกเลิกใบเสร็จได้");
            }
            else
            {
                DwDetail.SetItemDecimal(1, "cancel_amt", DwMain.GetItemDecimal(1, "receive_amt"));
                JsCalMemMain();
            }
        }

        private void JsCalMemMain()
        {
            try
            {
                strAgent.xml_head = DwMain.Describe("DataWindow.Data.XML");
                strAgent.amount = DwDetail.GetItemDecimal(1, "cancel_amt");
                strAgent.column_name = "cancel_all_amt";
                strAgent.recv_period = "-1";
                agenService.CalMemMain(state.SsWsPass, ref strAgent);
                if (strAgent.xml_head != "" && strAgent.xml_head != null)
                {
                    DwUtil.ImportData(strAgent.xml_head, DwMain, null, FileSaveAsType.Xml);
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
            
        }
    }
}
