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
    public partial class w_sheet_ag_agentdetail : PageWebSheet, WebSheet
    {
        protected String initAgentList;
        protected String initAgentDetail;
        protected String PostsearchMembNo;

        private AgencyClient agenService;
        protected str_agent strAgent;
        private DwThDate tDwTabAdj;
        private DwThDate tDwTabCancel;
        private DwThDate tDwTabOut;
        private DwThDate tDwTabIn;
        private DwThDate tDwTabAdd;
        private DwThDate tDwTabGrp;
        #region WebSheet Members

        public void InitJsPostBack()
        {
            initAgentList = WebUtil.JsPostBack(this, "initAgentList");
            initAgentDetail = WebUtil.JsPostBack(this, "initAgentDetail");
            PostsearchMembNo = WebUtil.JsPostBack(this, "PostsearchMembNo");

            tDwTabAdj = new DwThDate(DwTabAdj, this);
            tDwTabAdj.Add("ret_day", "ret_tday");
            tDwTabAdj.Add("adj_day", "adj_tday");
            tDwTabCancel = new DwThDate(DwTabCancel, this);
            tDwTabCancel.Add("cancel_day", "cancel_tday");
            tDwTabOut = new DwThDate(DwTabOut, this);
            tDwTabOut.Add("move_day", "move_tday");
            tDwTabIn = new DwThDate(DwTabIn, this);
            tDwTabIn.Add("move_day", "move_tday");
            tDwTabAdd = new DwThDate(DwTabAdd, this);
            tDwTabAdd.Add("addrecv_day", "addrecv_tday");
            tDwTabGrp = new DwThDate(DwTabGrp, this);
            tDwTabGrp.Add("recv_day", "recv_tday");


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
                this.RestoreContextDw(DwList);
                this.RestoreContextDw(DwTabGrp);
                this.RestoreContextDw(DwTabMem);
                this.RestoreContextDw(DwTabAdd);
                this.RestoreContextDw(DwTabAdj);
                this.RestoreContextDw(DwTabCancel);
                this.RestoreContextDw(DwTabIn);
                this.RestoreContextDw(DwTabOut);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "initAgentList")
            {
                JsInitAgentList();
            }
            else if (eventArg == "initAgentDetail")
            {
                JsInitAgentDetail();
            }
            else if (eventArg == "PostsearchMembNo")
            {
                searchMembNo();
            }
        }

        public void SaveWebSheet()
        {

        }

        public void WebSheetLoadEnd()
        {
            WebUtil.RetrieveDDDW(DwMain, "recv_period", "ag_agentdetail.pbl", null);
            WebUtil.RetrieveDDDW(DwMain, "sagentgrp_code", "ag_agentdetail.pbl", null);
            WebUtil.RetrieveDDDW(DwMain, "eagentgrp_code", "ag_agentdetail.pbl", null);
            DwMain.SaveDataCache();
            DwList.SaveDataCache();
            DwTabGrp.SaveDataCache();
            DwTabMem.SaveDataCache();
            DwTabAdd.SaveDataCache();
            DwTabAdj.SaveDataCache();
            DwTabCancel.SaveDataCache();
            DwTabIn.SaveDataCache();
            DwTabOut.SaveDataCache();
        }

        #endregion

        private void JsInitAgentList()
        {
            String recvPeriod = DwMain.GetItemString(1, "recv_period");
            String sGrpCode = DwMain.GetItemString(1, "sagentgrp_code");
            String eGrpCode = DwMain.GetItemString(1, "eagentgrp_code");
            String xmlList = "";
            try
            {
                strAgent.xml_head = DwMain.Describe("DataWindow.Data.XML");
                agenService.InitClearAgent(state.SsWsPass, ref strAgent);
                xmlList = strAgent.xml_detail;
                DwList.Reset();
                if (xmlList != "" && xmlList != null)
                {
                    DwUtil.ImportData(xmlList, DwList, null, FileSaveAsType.Xml);
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

        private void JsInitAgentDetail()
        {
            int row = int.Parse(HdRowList.Value);
            String errText = "";
            strAgent.recv_period = DwMain.GetItemString(1, "recv_period");
            strAgent.agentgrp_code = DwList.GetItemString(row, "agentgrp_code").Trim();
            strAgent.member_type = short.Parse(DwList.GetItemDecimal(row, "member_type").ToString());

            try
            {
                agenService.InitAgentDetail(state.SsWsPass, ref strAgent);
                DwTabGrp.Reset();
                DwTabMem.Reset();
                DwTabAdd.Reset();
                DwTabAdj.Reset();
                DwTabCancel.Reset();
                DwTabIn.Reset();
                DwTabOut.Reset();

                if (strAgent.xml_detail_group != "" && strAgent.xml_detail_group != null)
                {
                    DwUtil.ImportData(strAgent.xml_detail_group, DwTabGrp, null, FileSaveAsType.Xml);
                    tDwTabGrp.Eng2ThaiAllRow();
                }
                else
                {
                    errText += "ไม่มีข้อมูลสังกัด ";
                }
                if (strAgent.xml_detail_mem != "" && strAgent.xml_detail_mem != null)
                {
                    DwUtil.ImportData(strAgent.xml_detail_mem, DwTabMem, null, FileSaveAsType.Xml);
                }
                else
                {
                    errText += "ไม่มีข้อมูลทะเบียน ";
                }
                if (strAgent.xml_detail_add != "" && strAgent.xml_detail_add != null)
                {
                    DwUtil.ImportData(strAgent.xml_detail_add, DwTabAdd, null, FileSaveAsType.Xml);
                    tDwTabAdd.Eng2ThaiAllRow();
                }
                else
                {
                    errText += "ไม่มีข้อมูลชำระเพิ่ม ";
                }
                if (strAgent.xml_detail_retadj != "" && strAgent.xml_detail_retadj != null)
                {
                    DwUtil.ImportData(strAgent.xml_detail_retadj, DwTabAdj, null, FileSaveAsType.Xml);
                    tDwTabAdj.Eng2ThaiAllRow();
                }
                else
                {
                    errText += "ไม่มีข้อมูลคืนเงิน ";
                }
                if (strAgent.xml_detail_cancel != "" && strAgent.xml_detail_cancel != null)
                {
                    DwUtil.ImportData(strAgent.xml_detail_cancel, DwTabCancel, null, FileSaveAsType.Xml);
                    tDwTabCancel.Eng2ThaiAllRow();
                }
                else
                {
                    errText += "ไม่มีข้อมูลยกเลิก ";
                }
                if (strAgent.xml_detail_movegrpin != "" && strAgent.xml_detail_movegrpin != null)
                {
                    DwUtil.ImportData(strAgent.xml_detail_movegrpin, DwTabIn, null, FileSaveAsType.Xml);
                    tDwTabIn.Eng2ThaiAllRow();
                }
                else
                {
                    errText += "ไม่มีข้อมูลย้ายเข้า ";
                }
                if (strAgent.xml_detail_movegrpout != "" && strAgent.xml_detail_movegrpout != null)
                {
                    DwUtil.ImportData(strAgent.xml_detail_movegrpout, DwTabOut, null, FileSaveAsType.Xml);
                    tDwTabOut.Eng2ThaiAllRow();
                }
                else
                {
                    errText += "ไม่มีข้อมูลย้ายออก ";
                }
                if (errText != "")
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(errText);
                }

            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        private void searchMembNo()
        {
            String membNo;

            try
            {
                membNo = TextBox1.Text;
            }
            catch
            {
                membNo = "";
            }
            if (membNo != "")
            {
                DwTabMem.SetFilter("member_no='" + membNo + "'");
                DwTabMem.Filter();
            }
            else
            {
                DwTabMem.SetFilter("");
                DwTabMem.Filter();
            }
        }
    }
}
