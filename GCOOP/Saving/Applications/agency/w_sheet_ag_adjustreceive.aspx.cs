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
using System.Web.Services.Protocols;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using CoreSavingLibrary.WcfNCommon;
using CoreSavingLibrary.WcfNAgency;
using Sybase.DataWindow;

namespace Saving.Applications.agency
{
    public partial class w_sheet_ag_adjustreceive : PageWebSheet, WebSheet
    {
        private DwThDate tDwDetail;
        protected str_agent strAgent;
        private n_agencyClient AgencyService;
        protected String postCalMemMain;
        protected String postInitAdjustReceive;

        #region WebSheet Members

        public void InitJsPostBack()
        {
            tDwDetail = new DwThDate(DwDetail, this);
            tDwDetail.Add("adj_day", "adj_tday");

            postCalMemMain = WebUtil.JsPostBack(this, "postCalMemMain");
            postInitAdjustReceive = WebUtil.JsPostBack(this, "postInitAdjustReceive");
        }

        public void WebSheetLoadBegin()
        {
            AgencyService = wcf.Agency;
            strAgent = new str_agent();

            if (!IsPostBack)
            {
                DwMain.InsertRow(0);
                DwDetail.InsertRow(0);

                try
                {
                    DwUtil.RetrieveDDDW(DwMain, "recv_period", "adjustreceive.pbl", null);
                    DwUtil.RetrieveDDDW(DwDetail, "cause_code", "adjustreceive.pbl", null);
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                }
            }
            else
            {
                this.RestoreContextDw(DwMain);
                this.RestoreContextDw(DwDetail);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postInitAdjustReceive")
            {
                InitAdjustReceive();
            }
            else if (eventArg == "postCalMemMain")
            {
                CalMemMain();
            }
        }

        public void SaveWebSheet()
        {
            strAgent.xml_head = DwMain.Describe("DataWindow.Data.XML");
            DwDetail.SetItemString(1, "entry_id", state.SsUsername);
            DwDetail.SetItemString(1, "machine_id", state.SsClientIp);
            DwDetail.SetItemDateTime(1, "system_date", state.SsWorkDate);
            DwDetail.SetItemDateTime(1, "adj_time", DateTime.Now);
            strAgent.xml_detail = DwDetail.Describe("DataWindow.Data.XML");
            int re = 0;
            try
            {
                re = AgencyService.SaveAdjustReceive(state.SsWsPass, strAgent);
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }

            if (re == 1)
            {
                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกเรียบร้อย");

                DwMain.Reset();
                DwMain.InsertRow(0);
                DwDetail.Reset();
                DwDetail.InsertRow(0);

                DwDetail.SetItemDateTime(1, "adj_day", state.SsWorkDate);
                tDwDetail.Eng2ThaiAllRow();
            }
        }

        public void WebSheetLoadEnd()
        {
            DwMain.SaveDataCache();
            DwDetail.SaveDataCache();
        }

        #endregion

        private void CalMemMain()
        {
            strAgent.xml_head = DwMain.Describe("DataWindow.Data.XML");
  
            strAgent.amount = DwDetail.GetItemDecimal(1, "adj_amt");
            String itemCode = DwDetail.GetItemString(1, "itempaytype_code");
            if (itemCode == "IAG" || itemCode == "DAG")
            {
                strAgent.column_name = "addrecv_amt";
            }
            else if (itemCode == "DTR" || itemCode == "ITR")
            {
                strAgent.column_name = "ADJ_ALL_AMT";
            }


            if (itemCode == "IAG" || itemCode == "DTR")
            {
                strAgent.recv_period = "-1";
            }
            else if (itemCode == "DAG" || itemCode == "ITR")
            {
                strAgent.recv_period = "1";
            }
            else
            {
                strAgent.recv_period = "1";
            }
            try
            {
                int re = AgencyService.CalMemMain(state.SsWsPass, ref strAgent);

                DwMain.Reset();
                DwMain.ImportString(strAgent.xml_head, FileSaveAsType.Xml);
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        private void InitAdjustReceive()
        {
            strAgent.xml_head = DwMain.Describe("DataWindow.Data.XML");
            strAgent.xml_detail = DwDetail.Describe("DataWindow.Data.XML");
            String recvPeriod = DwMain.GetItemString(1, "recv_period");
            String memberNo = DwMain.GetItemString(1, "member_no");
            try
            {
                AgencyService.Initadjustreceive(state.SsWsPass, ref strAgent);
                DwMain.Reset();
                DwDetail.Reset();
                if (strAgent.xml_head != "" && strAgent.xml_head != null)
                {
                    DwMain.ImportString(strAgent.xml_head, FileSaveAsType.Xml);
                    DwDetail.ImportString(strAgent.xml_detail, FileSaveAsType.Xml);

                    DwDetail.SetItemDateTime(1, "adj_day", state.SsWorkDate);
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
            Decimal cancelAmt = DwMain.GetItemDecimal(1, "cancel_all_amt");
            if (cancelAmt > 0)
            {
                DwMain.Reset();
                DwDetail.Reset();
                DwMain.InsertRow(0);
                DwDetail.InsertRow(0);
                LtServerMessage.Text = WebUtil.ErrorMessage("ข้อมูลงวดที่ : " + recvPeriod + " เลขทะเบียนที่ : " + memberNo + " ไม่สามารถทำการปรับปรุงลูกหนี้ตัวแทนได้");
            }
        }
    }
}
