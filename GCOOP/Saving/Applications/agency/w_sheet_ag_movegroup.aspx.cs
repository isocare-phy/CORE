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
    public partial class w_sheet_ag_movegroup : PageWebSheet, WebSheet
    {
        private n_agencyClient AgencyService;
        protected str_agent strAgent;
        private DwThDate tDwDetail;
        protected String postInitMoveGroup;
        protected String postAgentGrp;

        #region WebSheet Members

        public void InitJsPostBack()
        {
            tDwDetail = new DwThDate(DwDetail, this);
            tDwDetail.Add("move_day", "move_tday");

            postInitMoveGroup = WebUtil.JsPostBack(this, "postInitMoveGroup");
            postAgentGrp = WebUtil.JsPostBack(this, "postAgentGrp");
        }

        public void WebSheetLoadBegin()
        {
            strAgent = new str_agent();
            AgencyService = wcf.Agency;

            if (!IsPostBack)
            {
                DwMain.InsertRow(0);
                DwDetail.InsertRow(0);

                DwUtil.RetrieveDDDW(DwMain, "recv_period", "movegroup.pbl", null);
                DwUtil.RetrieveDDDW(DwDetail, "membgrp_code", "movegroup.pbl", null);
                DwUtil.RetrieveDDDW(DwDetail, "agentmov_code", "movegroup.pbl", null);
                DwUtil.RetrieveDDDW(DwDetail, "agentgrp_code", "movegroup.pbl", null);
            }
            else
            {
                this.RestoreContextDw(DwMain);
                this.RestoreContextDw(DwDetail);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postInitMoveGroup")
            {
                InitMoveGroup();
            }
            else if (eventArg == "postAgentGrp")
            {
                JsPostAgentGrp();
            }
        }

        public void SaveWebSheet()
        {
            strAgent.xml_head = DwMain.Describe("DataWindow.Data.XML");
            strAgent.xml_detail = DwDetail.Describe("DataWindow.Data.XML");

            int re = 0;

            try
            {
                re = AgencyService.SaveMoveGroup(state.SsWsPass, strAgent);
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
            }
        }

        public void WebSheetLoadEnd()
        {
            DwMain.SaveDataCache();
            DwDetail.SaveDataCache();
        }

        #endregion

        private void InitMoveGroup()
        {
            strAgent.xml_head = DwMain.Describe("DataWindow.Data.XML");
            strAgent.xml_detail = DwDetail.Describe("DataWindow.Data.XML");

            try
            {
                int re = AgencyService.InitMoveGroup(state.SsWsPass, ref strAgent);
                DwMain.Reset();
                DwMain.ImportString(strAgent.xml_head, FileSaveAsType.Xml);
                DwDetail.Reset();
                DwDetail.ImportString(strAgent.xml_detail, FileSaveAsType.Xml);

                DwDetail.SetItemDateTime(1, "move_day", state.SsWorkDate);
                tDwDetail.Eng2ThaiAllRow();
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                DwMain.Reset();
                DwDetail.Reset();
                DwMain.InsertRow(0);
                DwDetail.InsertRow(0);
            }
            Decimal receiveAmt = DwMain.GetItemDecimal(1, "receive_amt");
            Decimal outStanding = DwMain.GetItemDecimal(1, "outstandingbal_begin");
            Decimal recvAmt = DwMain.GetItemDecimal(1, "recv_amt");
            Decimal addRecvAmt = DwMain.GetItemDecimal(1, "addrecv_amt");
            Decimal retAmt = DwMain.GetItemDecimal(1, "ret_all_amt");
            Decimal adjAmt = DwMain.GetItemDecimal(1, "adj_all_amt");
            Decimal cancelAmt = DwMain.GetItemDecimal(1, "cancel_all_amt");
            Decimal totalAmt = receiveAmt + outStanding - recvAmt - addRecvAmt + retAmt - adjAmt + cancelAmt;
            DwDetail.SetItemDecimal(1, "mb_stmt_amt", totalAmt);
        }

        private void JsPostAgentGrp()
        {
            String membGrp = DwDetail.GetItemString(1, "membgrp_code");   
            String sql = "select AGENTGRP_CODE from MBUCFMEMBGROUP where MEMBGROUP_CODE='" + membGrp + "'";
            DataTable dt = WebUtil.Query(sql);
            if (dt.Rows.Count > 0)
            {
                String agentGrp = dt.Rows[0][0].ToString().Trim();
                DwDetail.SetItemString(1, "agentmov_code", agentGrp);
            }
        }

    }
}
