using System;
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
using Saving.WcfCommon;
using Saving.WcfShrlon;
using Sybase.DataWindow;
using System.Web.Services.Protocols;

namespace Saving.Applications.shrlon
{
    public partial class w_sheet_sl_loan_requestment_loop : PageWebSheet, WebSheet
    {
        private DwThDate tDwMain;
        private ShrlonClient shrlonService;
        private CommonClient commonService;
        protected String cancelRequest;
        protected String cancelRequestL;
        protected String collCondition;
        protected String collInitP;
        protected String checkRefReqLoop;
        protected String jsPostMember;
        protected String jsPostColl;
        protected String jsPostClear;
        protected String jsPostReqLoop;
        protected String loadDwCollR;
        protected String openNew;
        protected String openOldDocNo;
        protected String genJs = "";
        protected String visibleChange;
        protected String resetLoop;
        protected String resetColl;
        protected String resendStr;
        protected String refresh;
        protected String refreshDW;
        protected String regenLoanClear;
        protected String resumLoanClear;
        protected String setData;
        protected String setDWOthClr;
        protected String setLoanType;
        protected String setOldData;
        protected String setOthClr;
        protected String setRunningNo;
        protected String showBranch;
        String loanrcvperiod_month;
        public void InitJsPostBack()
        {
            cancelRequest = WebUtil.JsPostBack(this, "cancelRequest");
            cancelRequestL = WebUtil.JsPostBack(this, "cancelRequestL");
            collCondition = WebUtil.JsPostBack(this, "collCondition");
            collInitP = WebUtil.JsPostBack(this, "collInitP");
            checkRefReqLoop = WebUtil.JsPostBack(this, "checkRefReqLoop");
            jsPostMember = WebUtil.JsPostBack(this, "jsPostMember");
            jsPostColl = WebUtil.JsPostBack(this, "jsPostColl");
            jsPostClear = WebUtil.JsPostBack(this, "jsPostClear");
            jsPostReqLoop = WebUtil.JsPostBack(this, "jsPostReqLoop");
            loadDwCollR = WebUtil.JsPostBack(this, "loadDwCollR");
            openNew = WebUtil.JsPostBack(this, "openNew");
            openOldDocNo = WebUtil.JsPostBack(this, "openOldDocNo");
            resetLoop = WebUtil.JsPostBack(this, "resetLoop");
            resendStr = WebUtil.JsPostBack(this, "resendStr");
            refresh = WebUtil.JsPostBack(this, "refresh");
            refreshDW = WebUtil.JsPostBack(this, "refreshDW");
            resetColl = WebUtil.JsPostBack(this, "resetColl");
            regenLoanClear = WebUtil.JsPostBack(this, "regenLoanClear");
            resumLoanClear = WebUtil.JsPostBack(this, "resumLoanClear");
            setData = WebUtil.JsPostBack(this, "setData");
            setDWOthClr = WebUtil.JsPostBack(this, "setDWOthClr");
            setLoanType = WebUtil.JsPostBack(this, "setLoanType");
            setOldData = WebUtil.JsPostBack(this, "setOldData");
            setOthClr = WebUtil.JsPostBack(this, "setOthClr");
            setRunningNo = WebUtil.JsPostBack(this, "setRunningNo");
            visibleChange = WebUtil.JsPostBack(this, "visibleChange");
            showBranch = WebUtil.JsPostBack(this, "showBranch");


            tDwMain = new DwThDate(dw_main, this);
            tDwMain.Add("loanrequest_date", "loanrequest_tdate");
            tDwMain.Add("loanrcvfix_date", "loanrcvfix_tdate");
            tDwMain.Add("startkeep_date", "startkeep_tdate");
        }
        public void WebSheetLoadBegin()
        {
            String loantype = "";
            try
            {
                shrlonService = wcf.Shrlon;
                commonService = wcf.Common;
            }
            catch
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ติดต่อ Web Service ไม่ได้");
                return;
            }
            String memberNo = "";
            try
            {
                memberNo = Session["memberNo"].ToString();
                Session["memberNo"] = "";
            }
            catch
            {
                memberNo = null;
            }

            if (IsPostBack)
            {
                this.RestoreContextDw(dw_main);
                this.RestoreContextDw(dw_coll);
                this.RestoreContextDw(dw_clear);
                this.RestoreContextDw(dw_keeping);
                this.RestoreContextDw(dw_reqloop);
                this.RestoreContextDw(dw_otherclr);
            }
            if (dw_main.RowCount < 1)
            {
                dw_main.DisplayOnly = false;
                dw_clear.DisplayOnly = false;
                dw_coll.DisplayOnly = false;
                dw_keeping.DisplayOnly = false;
                dw_reqloop.DisplayOnly = false;
                dw_message.DisplayOnly = false;
                dw_otherclr.DisplayOnly = false;

                dw_main.InsertRow(0);
                LtXmlOtherlr.Text = "";
                LtXmlKeeping.Text = "";
                LtXmlReqloop.Text = "";
                Session["requestopen"] = "";
                Session["strItemchange"] = "";
                try
                {
                    LtRunningNo.Text = Session["runningNo"].ToString();
                }
                catch { LtRunningNo.Text = ""; }

                try { dw_main.SetItemString(1, "loanrcvperiod_month", Session["loanrcvperiod_month"].ToString()); }
                catch { }
                try
                {
                    loantype = Session["loantype"].ToString();
                    Session.Remove("loantype");
                }
                catch
                {
                    loantype = "11";
                    Session.Remove("loantype");
                }
                //ประกาศ dddw
                try
                {

                    DwUtil.RetrieveDDDW(dw_main, "loantype_code_1", "sl_loan_requestment.pbl", null);
                    DwUtil.RetrieveDDDW(dw_main, "expense_code", "sl_loan_requestment.pbl", null);

                    if (loantype == "")
                    {
                        dw_main.SetItemString(1, "loantype_code", "11");    // กำหนดค่าเริ่มต้น เป็น ฉุกเฉินโอน
                    }
                    else
                    {
                        dw_main.SetItemString(1, "loantype_code", loantype);
                    }
                    dw_reqloop.InsertRow(0);

                    dw_main.SetItemDateTime(1, "startkeep_date", state.SsWorkDate);
                    dw_main.SetItemDateTime(1, "loanrcvfix_date", state.SsWorkDate);
                    dw_main.SetItemDateTime(1, "loanrequest_date", state.SsWorkDate);
                    tDwMain.Eng2ThaiAllRow();

                    HdColumnName.Value = "loantype_code";
                    JsPostMember();

                    if ((memberNo != null) && (memberNo != ""))
                    {
                        dw_main.SetItemString(1, "member_no", memberNo);
                        HdColumnName.Value = "member_no";
                        JsPostMember();
                        DwUtil.RetrieveDDDW(dw_main, "loanobjective_code", "sl_loan_requestment.pbl", null);
                        DwUtil.RetrieveDDDW(dw_otherclr, "clrothertype_code", "sl_loan_requestment.pbl", null);
                        //DwUtil.RetrieveDDDW(dw_main, "expense_branch_1", "sl_loan_requestment.pbl", null);
                        //ShowBranch();
                    }

                }
                catch (Exception ex)
                { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
            }
            //SetMonthYears();
            if (dw_main.RowCount > 1)
            {
                dw_main.DeleteRow(dw_main.RowCount);
            }
        }
        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "jsPostMember")
            {
                JsPostMember();
            }
            else if (eventArg == "jsPostColl")
            {
                JsPostColl();
            }
            else if (eventArg == "resendStr")
            {
                ResendStr();
            }
            else if (eventArg == "refreshDW")
            {
                RefreshDW();
            }
            else if (eventArg == "setOldData")
            {
                SetOldData();
            }
            else if (eventArg == "resetColl")
            {
                ResetColl();
            }
            else if (eventArg == "jsPostClear")
            {
                JsPostClear();
            }
            else if (eventArg == "showBranch")
            {
                ShowBranch();
            }
            else if (eventArg == "loadDwCollR")
            {
                LoadDwCollR();
            }
            else if (eventArg == "openOldDocNo")
            {
                OpenOldDocNo();
            }
            else if (eventArg == "openNew")
            {
                OpenNew();
            }
            else if (eventArg == "setOthClr")
            {
                SetOthClr();
            }
            else if (eventArg == "collInitP")
            {
                CollInitP();
            }
            else if (eventArg == "collCondition")
            {
                CollCondition();
            }
            else if (eventArg == "cancelRequest")
            {
                CancelRequest();
            }
            else if (eventArg == "cancelRequestL")
            {
                CancelRequestL();
            }
            else if (eventArg == "setLoanType")
            {
                SetLoanType();
            }
            else if (eventArg == "setData")
            {
                SetData();
            }
            else if (eventArg == "setDWOthClr")
            {
                SetDWOthClr();
            }
            else if (eventArg == "regenLoanClear")
            {
                RegenLoanClear();
            }
            else if (eventArg == "resumLoanClear")
            {
                ResumLoanClear();
            }
            else if (eventArg == "jsPostReqLoop")
            {
                JsPostReqLoop();
            }
            else if (eventArg == "checkRefReqLoop")
            {
                CheckRefReqLoop();
            }
            else if (eventArg == "setRunningNo")
            {
                SetRunningNo();
            }
            else if (eventArg == "resetLoop")
            {
                ResetLoop();
            }
            else if (eventArg == "refresh")
            {
                Refresh();
            }
        }
        public void SaveWebSheet()
        {
            try
            {
                //กำหนดค่าเริ่มต้น
                loanrcvperiod_month = dw_main.GetItemString(1, "loanrcvperiod_month");
                Session["loanrcvperiod_month"] = loanrcvperiod_month;
               
                String dwMain_XML = dw_main.Describe("DataWindow.Data.XML");
                String loancolltype_code = "";
                if (dw_coll.RowCount > 0)
                {
                    loancolltype_code = dw_coll.GetItemString(1, "loancolltype_code");
                }
                String dwColl_XML = "";
                if ((loancolltype_code != null) && (loancolltype_code != ""))
                {
                    dwColl_XML = dw_coll.Describe("DataWindow.Data.XML");
                }
                String loancontract_no = "";
                String dwClear_XML = "";
                try
                {
                    loancontract_no = dw_clear.GetItemString(dw_clear.RowCount, "loancontract_no");
                }
                catch { }
                if (loancontract_no != "")
                {
                    dwClear_XML = dw_clear.Describe("DataWindow.Data.XML");
                }
                String dwReqLoop_XML = "";
                try
                {
                    dwReqLoop_XML = dw_reqloop.Describe("DataWindow.Data.XML");
                }
                catch { dwReqLoop_XML = ""; }

                str_itemchange strList = new str_itemchange();
                str_savereqloan strSave = new str_savereqloan();
                strList = WebUtil.str_itemchange_session(this);
                String xmlOtherClr = "";
                String xmlIntspc = "";
                try
                {
                    xmlOtherClr = strList.xml_otherclr;
                }
                catch
                {
                    xmlOtherClr = null;
                }
                try
                {
                    xmlIntspc = strList.xml_intspc;
                }
                catch
                {
                    xmlIntspc = null;
                }
                strSave.xml_clear = dwClear_XML;
                strSave.xml_guarantee = dwColl_XML;
                strSave.xml_main = dwMain_XML;
                strSave.xml_otherclr = xmlOtherClr;
                strSave.xml_intspc = xmlIntspc;  // ตารางดอกเบี้ย
                strSave.xml_reqperiod = dwReqLoop_XML;
                strSave.format_type = "CAT";
                strSave.entry_id = state.SsUsername;
                strSave.coop_id = state.SsCoopId;

                // Service สำหรับบันทึก
                String runningNo = shrlonService.LoanRightSaveReqloan(state.SsWsPass,ref strSave);
                if ((runningNo != "") && (runningNo != null))
                {
                    // ตรวจสอบค่า และรับค่า running No ตัวถัดไปมาแสดง
                    //Session["month"] = dw_main.GetItemString(1, "loanrcvperiod_month");
                    //Session["year"] = dw_main.GetItemString(1, "loanrcvperiod_year");
                    Session["runningNo"] = runningNo;
                    LtRunningNo.Text = runningNo;
                    HdReturn.Value = "11"; // กำหนดค่าเพื่อให้เรียก OpenNew() ใหม่อีกครั้ง หลังจากแสดง DLG ใน SheetLoadComplete()
                }
            }
            catch (Exception ex)
            { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
        }
        public void WebSheetLoadEnd()
        {


            if (dw_coll.RowCount > 0)
            {
                DwUtil.RetrieveDDDW(dw_coll, "loancolltype_code", "sl_loan_requestment.pbl", null);
            }
           
            String loanTypeCode = dw_main.GetItemString(1, "loantype_code");
            try
            {
                int result = shrlonService.CheckReqLoop(state.SsWsPass, loanTypeCode);
                String refReqLoopDocNo = null;
                try { refReqLoopDocNo = dw_main.GetItemString(1, "refreqloop_docno"); }
                catch { refReqLoopDocNo = null; }
                if ((result == 1) && ((refReqLoopDocNo == null) || (refReqLoopDocNo == "")))
                {
                    dw_reqloop.DisplayOnly = false;
                }
                else
                {
                    dw_reqloop.DisplayOnly = true;
                }
            }
            catch { }

            VisibleChange();
            //กำหนดให้ DW ไม่ต้องแสดงค่า
            //dw_message.Visible = false;
            //dw_keeping.Visible = false;
            //dw_otherclr.Visible = false;
            //dw_clear.Visible = false;
            //trOtherClr.Visible = false;
            //trDetail.Visible = false;
            //trDetail.Visible = false;


            dw_main.SaveDataCache();
            dw_coll.SaveDataCache();
            dw_clear.SaveDataCache();
            dw_keeping.SaveDataCache();
            dw_reqloop.SaveDataCache();
            dw_otherclr.SaveDataCache();
        }
        private void CancelRequest()
        {
            String dwXmlMain = dw_main.Describe("DataWindow.Data.XML");
            String dwXmlMessage = "";
            String cancelID = "isocare";
            String branch = state.SsCoopId;
            try
            {
                int result = shrlonService.CancelRequest(state.SsWsPass, ref dwXmlMain, ref dwXmlMessage, cancelID, branch);
                if (result <= 1)
                {
                    dw_main.Reset();
                    dw_main.ImportString(dwXmlMain, FileSaveAsType.Xml);
                    //DwUtil.ImportData(dwXmlMain, dw_main, tDwMain, FileSaveAsType.Xml);
                    if (dw_main.RowCount > 1) dw_main.DeleteRow(dw_main.RowCount);
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
            if ((dwXmlMessage != "") && (dwXmlMessage != null))
            {
                dw_message.Reset();
                dw_message.ImportString(dwXmlMessage, FileSaveAsType.Xml);
                HdMsg.Value = dw_message.GetItemString(1, "msgtext");
            }
        }
        private void CancelRequestL()
        {
            String dwXmlMain = dw_main.Describe("DataWindow.Data.XML");
            String cancelID = "isocare";
            String branch = state.SsCoopId;
            String conLoopNo = "";
            try
            {
                conLoopNo = dw_reqloop.GetItemString(1, "contractloop_no");
            }
            catch { conLoopNo = null; }
            if ((conLoopNo != null) && (conLoopNo != ""))
            {
                //เรียก Service สำหรับยกเลิกรายละเอียดฉ. โอน
                int result = shrlonService.CancelReqLoop(state.SsWsPass, ref dwXmlMain, cancelID, branch);
                if (result == 1)
                {
                    LtServerMessage.Text = WebUtil.CompleteMessage("ยกเลิกสัญญา ฉ. โอนเรียบร้อยแล้ว");
                    try
                    {
                        String memberNO = dw_main.GetItemString(1, "member_no");
                        //    dw_main.SetItemString(1, "member_no", memberNO);
                        //    OpenNew();

                        //dw_main.SetItemString(1, "member_no", memberNO);
                        //dw_reqloop.Reset();
                        //dw_reqloop.InsertRow(0);
                        //dw_reqloop.DisplayOnly = false;
                        //HdColumnName.Value = "loanrcvperiod_month";
                        //JsPostMember();
                        Session["memberNo"] = memberNO;
                        HdReturn.Value = "13"; // กำหนดค่าเพื่อให้เรียก OpenNew() ใหม่อีกครั้ง หลังจากแสดง DLG ใน SheetLoadComplete()

                    }
                    catch { }
                }
            }
            else
            {
                //ส่งค่าไปยัง SheetLoadComplete
                HdReturn.Value = "20";
            }
        }
        private void CollCondition()
        {
            String dwXmlMain = dw_main.Describe("DataWindow.Data.XML");
            String dwXmlColl = dw_coll.Describe("DataWindow.Data.XML");
            String dwXmlMessage = "";

            dwXmlColl = shrlonService.CollPercCondition(state.SsWsPass, dwXmlMain, dwXmlColl, ref dwXmlMessage);

            try
            {
                if ((dwXmlColl != "") && (dwXmlColl != null))
                {
                    dw_coll.Reset();
                    dw_coll.ImportString(dwXmlColl, FileSaveAsType.Xml);
                }

            }
            catch
            {
                dw_coll.Reset();
            }
            if ((dwXmlMessage != "") && (dwXmlMessage != null))
            {
                dw_message.Reset();
                dw_message.ImportString(dwXmlMessage, FileSaveAsType.Xml);
                HdMsg.Value = dw_message.GetItemString(1, "msgtext");
            }

        }
        private void CollInitP()
        {
            String dwXmlMain = dw_main.Describe("DataWindow.Data.XML");
            String dwXmlColl = dw_coll.Describe("DataWindow.Data.XML");

            dwXmlColl = shrlonService.CollInitPrecent(state.SsWsPass, dwXmlMain, dwXmlColl);

            try
            {
                if ((dwXmlColl != "") && (dwXmlColl != null))
                {
                    dw_coll.Reset();
                    dw_coll.ImportString(dwXmlColl, FileSaveAsType.Xml);
                }
            }
            catch
            {
                dw_coll.Reset();
            }
        }
        private void CheckRefReqLoop()
        {
            try
            {
                String columnName = HdColumnName.Value;
                str_itemchange strList = new str_itemchange();
                strList = WebUtil.str_itemchange_session(this);
                String strXmlKeep = "";
                String strXmlReqloop = "";
                strList.xml_message = null;

                int result = shrlonService.LoanRightItemChangeMain(state.SsWsPass, ref strList);
                Session["strItemchange"] = strList;
                if ((strList.xml_message != null) && (strList.xml_message != ""))
                {
                    dw_message.Reset();
                    dw_message.ImportString(strList.xml_message, FileSaveAsType.Xml);
                    HdMsg.Value = dw_message.GetItemString(1, "msgtext");
                }
                HdReturn.Value = result.ToString();
                HdColumnName.Value = strList.column_name;
                //นำเข้าข้อมูลหลัก
                dw_main.Reset();
                dw_main.ImportString(strList.xml_main, FileSaveAsType.Xml);
                //DwUtil.ImportData(strList.xml_main, dw_main, tDwMain, FileSaveAsType.Xml);
                if (dw_main.RowCount > 1) dw_main.DeleteRow(dw_main.RowCount);
                tDwMain.Eng2ThaiAllRow();
                String refReqLoopNo = "";

                refReqLoopNo = dw_main.GetItemString(1, "refreqloop_docno");
                if ((refReqLoopNo != null) && (refReqLoopNo != ""))
                {
                    strXmlReqloop = shrlonService.ReqLoopOpen(state.SsWsPass, strList.xml_main);
                }
                else
                {
                    dw_reqloop.Reset();
                    dw_reqloop.InsertRow(0);
                    if (dw_reqloop.RowCount > 0) strXmlReqloop = dw_reqloop.Describe("DataWindow.Data.XML");
                    else strXmlReqloop = "";
                }

                try
                {

                    strXmlReqloop = shrlonService.ItemChangeReqLoop(state.SsWsPass, strList.xml_main, strXmlReqloop);
                    Session["xml_reqloop"] = strXmlReqloop;
                    LtXmlReqloop.Text = strXmlReqloop;
                }
                catch (Exception ex)
                { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }

                HdReturn.Value = result.ToString();
                HdColumnName.Value = strList.column_name;
                //นำเข้าข้อมูลหลัก
                dw_main.Reset();
                dw_main.ImportString(strList.xml_main, FileSaveAsType.Xml);
                //DwUtil.ImportData(strList.xml_main, dw_main, tDwMain, FileSaveAsType.Xml);
                if (dw_main.RowCount > 1) dw_main.DeleteRow(dw_main.RowCount);
                tDwMain.Eng2ThaiAllRow();
                try
                {
                    dw_otherclr.Reset();
                    dw_otherclr.ImportString(LtXmlOtherlr.Text, FileSaveAsType.Xml);
                }
                catch
                { dw_otherclr.Reset(); }
                try
                {
                    dw_coll.Reset();
                    dw_coll.ImportString(strList.xml_guarantee, FileSaveAsType.Xml);
                }
                catch { dw_coll.Reset(); }
                try
                {
                    dw_clear.Reset();
                    dw_clear.ImportString(strList.xml_clear, FileSaveAsType.Xml);
                }
                catch { dw_clear.Reset(); }
                try
                {
                    dw_keeping.Reset();
                    dw_keeping.ImportString(strXmlKeep, FileSaveAsType.Xml);
                }
                catch { dw_keeping.Reset(); }
                try
                {
                    dw_reqloop.Reset();
                    dw_reqloop.ImportString(strXmlReqloop, FileSaveAsType.Xml);
                }
                catch { dw_reqloop.Reset(); dw_reqloop.InsertRow(0); }

            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message); }
        }
        private void JsPostMember()
        {
            //JsPostMember เป็นMethod ที่ใช้สำหรับกรณีมีการเปลี่ยนแปลงค่าใน DW_Main
            try
            {
                //กำหนดค่าเริ่มต้น
                String columnName = HdColumnName.Value;
                String dwMainXML = dw_main.Describe("DataWindow.Data.XML");
                String dwCollXML = dw_coll.Describe("DataWindow.Data.XML");
                String dwClearXML = dw_clear.Describe("DataWindow.Data.XML");
                String dwreqloopXML = dw_reqloop.Describe("DataWindow.Data.XML");
                //String t = "20";
                //t = dw_main.GetItemString(1, "loantype_code");
                String dwkeepingXML = null;
                //String dwreqloopXML = null;
                String runningNo = LtRunningNo.Text;

                str_itemchange strList = new str_itemchange();
                strList.column_name = columnName;
                strList.xml_main = dwMainXML;
                strList.xml_guarantee = dwCollXML;
                strList.xml_clear = dwClearXML;
                strList.import_flag = true;
                strList.format_type = "CAT";
                strList.xml_reqloop = dwreqloopXML;

                //เรียก Service  สำหรับ itemChange DW_Main 
                int result = shrlonService.LoanRightItemChangeMain(state.SsWsPass, ref strList);
                columnName = strList.column_name;
                //กรณีเปลี่ยนค่าขอกู้ ให้กำหนด loanmaxreq_amt ให้กับ dw_reqloop เพื่อตรวจสอบเตือนค่าที่เกินขอกู้
                if(columnName == "loanrequest_amt")
                {
                    try{
                     dw_reqloop.SetItemDecimal(1, "loanmaxreq_amt", dw_main.GetItemDecimal(1, "loanmaxreq_amt"));
                    }catch{}
                }
                //else if (columnName == "expense_code")
                //{
                //    string expendCode = "";
                //    try
                //    {
                //        expendCode = dw_main.GetItemString(1, "expense_code");
                //    }
                //    catch { }
                //     if (expendCode == "CBT")
                //    {
                //        ShowBranch();
                //    }
                //}
               
                //Boom ส่วนเรียกเก็บ Keeping 
                try
                {
                    dwkeepingXML = string.IsNullOrEmpty(strList.xml_keep) ? "" : strList.xml_keep;
                    if ((dwkeepingXML != "") && (dwkeepingXML != null))
                    {
                        dw_keeping.Reset();
                        dw_keeping.ImportString(dwkeepingXML, FileSaveAsType.Xml);
                    }
                }
                catch { dw_keeping.Reset(); }
                //Boom ส่วน ฉ.โอน Reqloop
                try
                {
                    dwreqloopXML = string.IsNullOrEmpty(strList.xml_reqloop) ? "" : strList.xml_reqloop;
                    Session["xml_reqloop"] = dwreqloopXML;
                    dw_reqloop.Reset();
                    if (dwreqloopXML.Trim() != "")
                    {
                        dw_reqloop.ImportString(dwreqloopXML, FileSaveAsType.Xml);
                    }
                    else { dw_reqloop.InsertRow(0); }
                }
                catch { dw_reqloop.Reset(); dw_reqloop.InsertRow(0); }

                LtXmlKeeping.Text = dwkeepingXML;
                LtXmlReqloop.Text = dwreqloopXML;
                Session["strItemchange"] = strList;
                if ((strList.xml_message != null) && (strList.xml_message != ""))
                {
                    dw_message.Reset();
                    dw_message.ImportString(strList.xml_message, FileSaveAsType.Xml);
                    HdMsg.Value = dw_message.GetItemString(1, "msgtext");
                }
                if ((strList.column_name == "expense_code"))
                {
                    LtServerMessage.Text = strList.column_data;
                }
                //นำเข้าข้อมูลหลัก
                dw_main.Reset();
                dw_main.ImportString(strList.xml_main, FileSaveAsType.Xml);
                //DwUtil.ImportData(strList.xml_main, dw_main, tDwMain, FileSaveAsType.Xml);
               // ShowBranch();
                if (dw_main.RowCount > 1) dw_main.DeleteRow(dw_main.RowCount);
                //ตรวจสอบ ถ้าเป็น 8 ต้องทำต่อ ผ่าน SheetLoadComplete
                if (result == 8)
                {
                    HdReturn.Value = result.ToString();
                    HdColumnName.Value = strList.column_name;
                    //DwUtil.ImportData(strList.xml_main, dw_main, tDwMain, FileSaveAsType.Xml);
                    //if (dw_main.RowCount > 1) dw_main.DeleteRow(dw_main.RowCount);
                    HdMemberNo.Value = dw_main.GetItemString(1, "member_no");
                }
                //else
                //{
                //    DwUtil.ImportData(strList.xml_main, dw_main, tDwMain, FileSaveAsType.Xml);
                //    if (dw_main.RowCount > 1) dw_main.DeleteRow(dw_main.RowCount);
                //}
                tDwMain.Eng2ThaiAllRow();
                //Import String เข้าไปใน DW
                try
                {
                    dw_otherclr.Reset();
                    dw_otherclr.ImportString(LtXmlOtherlr.Text, FileSaveAsType.Xml);
                }
                catch { dw_otherclr.Reset(); }
                try
                {
                    dw_coll.Reset();
                    dw_coll.ImportString(strList.xml_guarantee, FileSaveAsType.Xml);
                }
                catch { dw_coll.Reset(); }
                try
                {
                    dw_clear.Reset();
                    dw_clear.ImportString(strList.xml_clear, FileSaveAsType.Xml);
                }
                catch
                {
                    dw_clear.Reset();
                }
                if (dw_reqloop.RowCount < 0)
                {
                    dw_reqloop.Reset();
                    dw_reqloop.InsertRow(0);
                }
                if ((LtRunningNo.Text != null) && (LtRunningNo.Text != ""))
                {
                    dw_main.SetItemString(1, "runing_no", LtRunningNo.Text);
                }

            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                HdReturn.Value = "12"; // กำหนดค่าเพื่อให้เรียก OpenNew() ใหม่อีกครั้ง หลังจากแสดง DLG ใน SheetLoadComplete()
            }


        }
        //private void JsPostMember()
        //{
        //    try
        //    {
        //        String columnName = HdColumnName.Value;
        //        String dwMainXML = dw_main.Describe("DataWindow.Data.XML");
        //        String dwCollXML = dw_coll.Describe("DataWindow.Data.XML");
        //        String dwClearXML = dw_clear.Describe("DataWindow.Data.XML");
        //        String t = "10";
        //        t = dw_main.GetItemString(1, "loantype_code");
        //        String dwkeepingXML = null;
        //        String dwreqloopXML = null;


        //        str_itemchange strList = new str_itemchange();
        //        strList.column_name = columnName;
        //        strList.xml_main = dwMainXML;
        //        strList.xml_guarantee = dwCollXML;
        //        strList.xml_clear = dwClearXML;
        //        strList.import_flag = true;
        //        strList.format_type = "CAT";


        //        int result = shrlonService.LoanRightItemChangeMain(state.SsWsPass, ref strList);
        //        if ((columnName == "loantype_code") || (columnName == "loantype_code_1"))
        //        {

        //        }
        //        else if (columnName == "member_no")
        //        {

        //        }
        //        else if (columnName == "loanrequest_amt")
        //        {
        //            strList.xml_main = shrlonService.ReCalFee(state.SsWsPass, strList.xml_main);
        //        }
        //        //Boom ส่วนเรียกเก็บ Keeping 
        //        try
        //        {
        //            dwkeepingXML = string.IsNullOrEmpty(strList.xml_keep) ? "" : strList.xml_keep;
        //            if ((dwkeepingXML != "") && (dwkeepingXML != null))
        //            {
        //                dw_keeping.Reset();
        //                dw_keeping.ImportString(dwkeepingXML, FileSaveAsType.Xml);
        //            }
        //        }
        //        catch { dw_keeping.Reset(); }
        //        //Boom ส่วน ฉ.โอน Reqloop
        //        try
        //        {
        //            dwreqloopXML = string.IsNullOrEmpty(strList.xml_reqloop) ? "" : strList.xml_reqloop;
        //            Session["xml_reqloop"] = dwreqloopXML;
        //            dw_reqloop.Reset();
        //            if (dwreqloopXML.Trim() != "")
        //            {
        //                dw_reqloop.ImportString(dwreqloopXML, FileSaveAsType.Xml);
        //            }
        //            else { dw_reqloop.InsertRow(0); }
        //        }
        //        catch { dw_reqloop.Reset(); dw_reqloop.InsertRow(0); }

        //        LtXmlKeeping.Text = dwkeepingXML;
        //        LtXmlReqloop.Text = dwreqloopXML;
        //        Session["strItemchange"] = strList;
        //        if ((strList.xml_message != null) && (strList.xml_message != ""))
        //        {
        //            dw_message.Reset();
        //            dw_message.ImportString(strList.xml_message, FileSaveAsType.Xml);
        //            HdMsg.Value = dw_message.GetItemString(1, "msgtext");
        //        }
        //        //Boom เอาข้อมูลส่วนหลัก แสดงทางหน้าจอ
        //        DwUtil.ImportData(strList.xml_main, dw_main, tDwMain, FileSaveAsType.Xml);
        //        if (dw_main.RowCount > 1) dw_main.DeleteRow(dw_main.RowCount);

        //        //Boom ตรวจสอบขั้นตอนว่าจะต้องดำเนินการต่อไปหรือไม่
        //        if (result == 8)
        //        {
        //            HdReturn.Value = result.ToString();
        //            HdColumnName.Value = strList.column_name;
        //            HdMemberNo.Value = dw_main.GetItemString(1, "member_no");
        //        }

        //        if (columnName == "expense_code")
        //        {
        //            string expendCode = "";
        //            try
        //            {
        //                expendCode = dw_main.GetItemString(1, "expense_code");
        //            }
        //            catch { }
        //            if (expendCode == "TRN")
        //            {
        //                //โอนภายใน เรียกเลขที่บัญชีของสมาชิก
        //                HdMemberNo.Value = dw_main.GetItemString(1, "member_no");
        //                HdReturn.Value = "15"; // กำหนดค่าเพื่อให้เรียก DLG เลือกบัญชีเงินฝาก ใน SheetLoadComplete()
        //            }
        //        }
        //        tDwMain.Eng2ThaiAllRow();
        //        //if (dw_main.RowCount > 1) dw_main.DeleteRow(dw_main.RowCount);
        //        try
        //        {
        //            dw_otherclr.Reset();
        //            dw_otherclr.ImportString(LtXmlOtherlr.Text, FileSaveAsType.Xml);
        //        }
        //        catch { dw_otherclr.Reset(); }
        //        try
        //        {
        //            dw_coll.Reset();
        //            dw_coll.ImportString(strList.xml_guarantee, FileSaveAsType.Xml);
        //        }
        //        catch
        //        {
        //            dw_coll.Reset();
        //        }
        //        try
        //        {
        //            dw_clear.Reset();
        //            dw_clear.ImportString(strList.xml_clear, FileSaveAsType.Xml);
        //        }
        //        catch { dw_clear.Reset(); }
        //    }
        //    catch (Exception ex)
        //    {
        //        LtServerMessage.Text = WebUtil.ErrorMessage(ex);
        //        HdReturn.Value = "12"; // กำหนดค่าเพื่อให้เรียก OpenNew() ใหม่อีกครั้ง หลังจากแสดง DLG ใน SheetLoadComplete()
        //    }
        //}
        private void JsPostColl()
        {
            //JSPostColl เป็น Method สำหรับ มีการเปลี่ยนแปลงค่า ใน Dw_Coll
            try
            {
                String strXmlKeep = LtXmlKeeping.Text;
                String strXmlReqloop = dw_reqloop.Describe("DataWindow.Data.XML");
                String columnName = HdColumnName.Value;
                String dwMainXML = dw_main.Describe("DataWindow.Data.XML");
                String dwCollXML = dw_coll.Describe("DataWindow.Data.XML");
                String dwClearXML = dw_clear.Describe("DataWindow.Data.XML");
                String t = dw_main.GetItemString(1, "loantype_code");
                str_itemchange strList = new str_itemchange();
                // ตรวจสอบว่า HdRowNumber มีค่าหรือไม่ ถ้าไม่มี ไม่ต้องทำอะไร
                if (HdRowNumber.Value.ToString() != "")
                {
                    int rowNumber = Convert.ToInt32(HdRowNumber.Value.ToString());
                    if ((columnName == "checkmancollperiod") || (columnName == "setcolldetail"))
                    {
                        dw_coll.SetItemString(rowNumber, "ref_collno", HdMemberNo.Value);
                        dwCollXML = dw_coll.Describe("DataWindow.Data.XML");
                    }
                    String refCollNo = "";
                    try
                    {
                        refCollNo = dw_coll.GetItemString(rowNumber, "ref_collno");
                        HdMemberNo.Value = refCollNo;
                    }
                    catch
                    {
                        refCollNo = "";
                    }
                    strList.column_name = columnName;
                    strList.xml_main = dwMainXML;
                    strList.xml_guarantee = dwCollXML;
                    strList.xml_clear = dwClearXML;
                    strList.import_flag = true;
                    strList.format_type = "CAT";
                    strList.column_data = dw_coll.GetItemString(rowNumber, "loancolltype_code") + refCollNo;

                    int result = shrlonService.LoanRightItemChangeColl(state.SsWsPass, ref strList);
                    Session["strItemchange"] = strList;
                    if ((strList.xml_message != null) && (strList.xml_message != ""))
                    {
                        dw_message.Reset();
                        dw_message.ImportString(strList.xml_message, FileSaveAsType.Xml);
                        HdMsg.Value = dw_message.GetItemString(1, "msgtext");
                        HdMsgWarning.Value = dw_message.GetItemString(1, "msgicon");
                    }
                    if (result == 8)
                    {
                        HdReturn.Value = result.ToString();
                        HdColumnName.Value = strList.column_name;
                    }
                    try
                    {

                        dw_otherclr.Reset();
                        dw_otherclr.ImportString(LtXmlOtherlr.Text, FileSaveAsType.Xml);
                    }
                    catch { dw_otherclr.Reset(); }
                    try
                    {
                        dw_coll.Reset();
                        dw_coll.ImportString(strList.xml_guarantee, FileSaveAsType.Xml);
                    }
                    catch { dw_coll.Reset(); }
                    try
                    {
                        dw_keeping.Reset();
                        dw_keeping.ImportString(strXmlKeep, FileSaveAsType.Xml);
                    }
                    catch { dw_keeping.Reset(); }
                    try
                    {
                        dw_reqloop.Reset();
                        dw_reqloop.ImportString(strXmlReqloop, FileSaveAsType.Xml);
                    }
                    catch { dw_reqloop.Reset(); dw_reqloop.InsertRow(0); }
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }
        private void JsPostClear()
        {
            try
            {
                String strXmlKeep = LtXmlKeeping.Text;
                String strXmlReqloop = LtXmlReqloop.Text;
                String columnName = HdColumnName.Value;
                String dwMainXML = dw_main.Describe("DataWindow.Data.XML");
                String dwCollXML = dw_coll.Describe("DataWindow.Data.XML");
                String dwClearXML = dw_clear.Describe("DataWindow.Data.XML");
                String t = dw_main.GetItemString(1, "loantype_code");

                str_itemchange strList = new str_itemchange();
                strList.column_name = columnName;
                strList.xml_main = dwMainXML;
                strList.xml_guarantee = dwCollXML;
                strList.xml_clear = dwClearXML;
                strList.import_flag = true;
                strList.format_type = "CAT";

                int result = shrlonService.LoanRightItemChangeClear(state.SsWsPass, ref strList);
                Session["strItemchange"] = strList;
                if ((strList.xml_message != null) && (strList.xml_message != ""))
                {
                    dw_message.Reset();
                    dw_message.ImportString(strList.xml_message, FileSaveAsType.Xml);
                    HdMsg.Value = dw_message.GetItemString(1, "msgtext");
                }
                if (result == 8)
                {
                    HdReturn.Value = result.ToString();
                    HdColumnName.Value = strList.column_name;
                }
                //นำเข้าข้อมูลหลัก
                dw_main.Reset();
                dw_main.ImportString(strList.xml_main, FileSaveAsType.Xml);
                //DwUtil.ImportData(strList.xml_main, dw_main, tDwMain, FileSaveAsType.Xml);
                if (dw_main.RowCount > 1) dw_main.DeleteRow(dw_main.RowCount);
                try
                {
                    dw_coll.Reset();
                    dw_coll.ImportString(strList.xml_guarantee, FileSaveAsType.Xml);
                }
                catch { dw_coll.Reset(); }
                try
                {
                    dw_keeping.Reset();
                    dw_keeping.ImportString(strXmlKeep, FileSaveAsType.Xml);
                }
                catch
                {
                    dw_keeping.Reset();
                }
                try
                {
                    dw_reqloop.Reset();
                    dw_reqloop.ImportString(strXmlReqloop, FileSaveAsType.Xml);
                }
                catch { dw_reqloop.Reset(); dw_reqloop.InsertRow(0); }
            }
            catch (Exception ex)
            { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
        }
        private void JsPostReqLoop()
        {
            String xmlMain = dw_main.Describe("DataWindow.Data.XML");
            String xmlReqLoop = dw_reqloop.Describe("DataWindow.Data.XML");
            try
            {
                if ((xmlReqLoop == null) || (xmlReqLoop == ""))
                {
                    dw_reqloop.Reset();
                    dw_reqloop.InsertRow(0);
                    if (dw_reqloop.RowCount > 0) xmlReqLoop = dw_reqloop.Describe("DataWindow.Data.XML");
                    else xmlReqLoop = "";
                }
                xmlReqLoop = shrlonService.ItemChangeReqLoop(state.SsWsPass, xmlMain, xmlReqLoop);
                DwUtil.ImportData(xmlReqLoop, dw_reqloop, null, FileSaveAsType.Xml);
            }
            catch
            {
                dw_reqloop.Reset();
                dw_reqloop.InsertRow(0);
            }


        }
        private void LoadDwCollR()
        {
            try
            {
                str_itemchange strList = new str_itemchange();
                strList = WebUtil.str_itemchange_session(this);
                dw_coll.Reset();
                dw_coll.ImportString(strList.xml_guarantee, FileSaveAsType.Xml);

            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }
        private void OpenNew()
        {
            dw_clear.Reset();
            dw_coll.Reset();
            dw_keeping.Reset();
            dw_main.Reset();
            dw_message.Reset();
            dw_reqloop.Reset();
            dw_otherclr.Reset();

            dw_main.InsertRow(0);
            dw_message.InsertRow(0);
            dw_reqloop.InsertRow(0);
            Session["requestopen"] = "";
            Session["strItemchange"] = "";


            dw_main.DisplayOnly = false;
            dw_clear.DisplayOnly = false;
            dw_coll.DisplayOnly = false;
            dw_keeping.DisplayOnly = false;
            dw_reqloop.DisplayOnly = false;
            dw_message.DisplayOnly = false;
            dw_otherclr.DisplayOnly = false;
            //ประกาศ dddw
            try
            {
                DwUtil.RetrieveDDDW(dw_main, "loantype_code_1", "sl_loan_requestment.pbl", null);
                dw_main.SetItemString(1, "loantype_code", "11");    // กำหนดค่าเริ่มต้น เป็น ฉุกเฉินโอน
                String runningNo = LtRunningNo.Text;
                if ((runningNo != null) && (runningNo != ""))
                {
                    dw_main.SetItemString(1, "runing_no", runningNo);
                }
                DwUtil.RetrieveDDDW(dw_main, "expense_code", "sl_loan_requestment.pbl", null);

                dw_main.SetItemDateTime(1, "startkeep_date", state.SsWorkDate);
                dw_main.SetItemDateTime(1, "loanrequest_date", state.SsWorkDate);
                dw_main.SetItemDateTime(1, "loanrequest_date", state.SsWorkDate);
                tDwMain.Eng2ThaiAllRow();

                HdColumnName.Value = "loantype_code";
                JsPostMember();
            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }

            if (dw_main.RowCount > 1)
            {
                dw_main.DeleteRow(dw_main.RowCount);
            }
            //dw_message.Visible = true;
            //dw_keeping.Visible = false;
            //dw_otherclr.Visible = false;
            //dw_clear.Visible = false;
        }
        private void OpenOldDocNo()
        {
            str_itemchange strList = new str_itemchange();
            str_requestopen strRequestOpen = new str_requestopen();
            string docno = dw_main.GetItemString(1, "loanrequest_docno");
            String strXmlKeep = LtXmlKeeping.Text;
            String strXmlReqloop = LtXmlReqloop.Text;
            String dwkeepingXML = null;
            String dwreqloopXML = null;

            strRequestOpen.request_no = docno;
            strRequestOpen.format_type = "CAT";
            strRequestOpen.xml_main = dw_main.Describe("DataWindow.Data.XML");
            strRequestOpen.xml_clear = "";
            strRequestOpen.xml_guarantee = "";
            strRequestOpen.xml_insurance = "";
            strRequestOpen.xml_intspc = "";
            try
            {
                strRequestOpen = shrlonService.LoanRequestOpen(state.SsWsPass, strRequestOpen);
            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }

            //นำข้อมูลเก็บไว้ใน DataWindow
            dw_main.Reset();
            dw_main.ImportString(strRequestOpen.xml_main, FileSaveAsType.Xml);
            //DwUtil.ImportData(strRequestOpen.xml_main, dw_main, tDwMain, FileSaveAsType.Xml);
            if (dw_main.RowCount > 1) dw_main.DeleteRow(dw_main.RowCount);
            string expendBank = "";
            try
            { expendBank = dw_main.GetItemString(1, "expense_bank"); }
            catch { expendBank = ""; }
            //ตรวจสอบ ธนาคารว่ามีหรือไม่ 
            if (expendBank != "")
            {
                // เรียก Method  ShowBranch() สำหรับแสดงสาขาธนาคาร
                ShowBranch();
            }
            tDwMain.Eng2ThaiAllRow();
            try
            {
                dw_clear.Reset();
                dw_clear.ImportString(strRequestOpen.xml_clear, FileSaveAsType.Xml);
            }
            catch
            {
                dw_clear.Reset();
            }
            try
            {
                dw_coll.Reset();
                dw_coll.ImportString(strRequestOpen.xml_guarantee, FileSaveAsType.Xml);
            }
            catch
            {
                dw_coll.Reset();
            }
            try
            {
                dw_otherclr.Reset();
                dw_otherclr.ImportString(strRequestOpen.xml_otherclr, FileSaveAsType.Xml);
            }
            catch
            {
                dw_otherclr.Reset();
            }
            dw_reqloop.Reset();
            try
            {
                dwkeepingXML = shrlonService.GetLastKeeping(state.SsWsPass, strRequestOpen.xml_main);

                dwreqloopXML = shrlonService.ReqLoopOpen(state.SsWsPass, strRequestOpen.xml_main);
                if ((dwreqloopXML == "") || (dwreqloopXML == null))
                {
                    dw_reqloop.Reset();
                    dw_reqloop.InsertRow(0);
                    if (dw_reqloop.RowCount > 0) dwreqloopXML = dw_reqloop.Describe("DataWindow.Data.XML");
                    else dwreqloopXML = "";
                }
                //if (dwreqloopXML.Trim() != "") dw_reqloop.ImportString(dwreqloopXML, FileSaveAsType.Xml);
                dwreqloopXML = shrlonService.ItemChangeReqLoop(state.SsWsPass, strRequestOpen.xml_main, dwreqloopXML);

                if ((dwkeepingXML != "") && (dwkeepingXML != null))
                {
                    dw_keeping.Reset();
                    dw_keeping.ImportString(dwkeepingXML, FileSaveAsType.Xml);
                }
                if ((dwreqloopXML != "") && (dwreqloopXML != null))
                {
                    dw_reqloop.Reset();
                    dw_reqloop.ImportString(dwreqloopXML, FileSaveAsType.Xml);
                }
                else
                {
                    dw_reqloop.Reset();
                    dw_reqloop.InsertRow(0);
                }
            }
            catch (Exception ex)
            { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }

            strList.xml_otherclr = strRequestOpen.xml_otherclr;
            strList.xml_main = strRequestOpen.xml_main;
            strList.xml_guarantee = strRequestOpen.xml_guarantee;
            strList.xml_clear = strRequestOpen.xml_clear;
            strList.xml_insurance = strRequestOpen.xml_insurance;
            strList.xml_message = strRequestOpen.xml_message;

            Session["strItemchange"] = strList;
            LtXmlOtherlr.Text = strRequestOpen.xml_otherclr;

            Decimal loanrequestStatus = dw_main.GetItemDecimal(1, "loanrequest_status");
            if ((loanrequestStatus != 8) && (loanrequestStatus != 81) && (loanrequestStatus != 11))
            {
                dw_main.DisplayOnly = true;
                dw_clear.DisplayOnly = true;
                dw_coll.DisplayOnly = true;
                dw_keeping.DisplayOnly = true;
                dw_reqloop.DisplayOnly = true;
                dw_message.DisplayOnly = true;
                dw_otherclr.DisplayOnly = true;
            }
            else
            {
                dw_main.DisplayOnly = false;
                dw_clear.DisplayOnly = false;
                dw_coll.DisplayOnly = false;
                dw_keeping.DisplayOnly = false;
                dw_reqloop.DisplayOnly = false;
                dw_message.DisplayOnly = false;
                dw_otherclr.DisplayOnly = false;
            }
        }
        private void ResetLoop()
        {
            try
            {
                dw_reqloop.SetItemDecimal(1, "month1_amt", 0);
                dw_reqloop.SetItemDecimal(1, "month2_amt", 0);
                dw_reqloop.SetItemDecimal(1, "month3_amt", 0);
                dw_reqloop.SetItemDecimal(1, "month4_amt", 0);
                dw_reqloop.SetItemDecimal(1, "month5_amt", 0);
                dw_reqloop.SetItemDecimal(1, "month6_amt", 0);
                dw_reqloop.SetItemDecimal(1, "month7_amt", 0);
                dw_reqloop.SetItemDecimal(1, "month8_amt", 0);
                dw_reqloop.SetItemDecimal(1, "month9_amt", 0);
                dw_reqloop.SetItemDecimal(1, "month10_amt", 0);
                dw_reqloop.SetItemDecimal(1, "month11_amt", 0);
                dw_reqloop.SetItemDecimal(1, "month12_amt", 0);
                dw_reqloop.SetItemDecimal(1, "difrequestloop_amt", 0);
            }
            catch
            {

            }
        }
        private void ResendStr()
        {
            try
            {
                String columnName = HdColumnName.Value;
                genJs = "";
                str_itemchange strList = new str_itemchange();
                strList = WebUtil.str_itemchange_session(this);
                String strXmlKeep = "";
                String strXmlReqloop = "";

                strList.xml_message = null;

                int result = shrlonService.LoanRightItemChangeMain(state.SsWsPass, ref strList);
                Session["strItemchange"] = strList;
                if ((strList.xml_message != null) && (strList.xml_message != ""))
                {
                    dw_message.Reset();
                    dw_message.ImportString(strList.xml_message, FileSaveAsType.Xml);
                    HdMsg.Value = dw_message.GetItemString(1, "msgtext");
                }
                try
                {
                    dw_reqloop.Reset();
                    dw_reqloop.InsertRow(0);
                    if (dw_reqloop.RowCount > 0) strXmlReqloop = dw_reqloop.Describe("DataWindow.Data.XML");
                    else strXmlReqloop = "";
                    strXmlReqloop = shrlonService.ItemChangeReqLoop(state.SsWsPass, strList.xml_main, strXmlReqloop);
                    Session["xml_reqloop"] = strXmlReqloop;
                    LtXmlReqloop.Text = strXmlReqloop;
                }
                catch (Exception ex)
                { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }

                HdReturn.Value = result.ToString();
                HdColumnName.Value = strList.column_name;
                //นำเข้าข้อมูลหลัก
                dw_main.Reset();
                dw_main.ImportString(strList.xml_main, FileSaveAsType.Xml);
                //DwUtil.ImportData(strList.xml_main, dw_main, tDwMain, FileSaveAsType.Xml);
                if (dw_main.RowCount > 1) dw_main.DeleteRow(dw_main.RowCount);
                tDwMain.Eng2ThaiAllRow();
                try
                {
                    dw_otherclr.Reset();
                    dw_otherclr.ImportString(LtXmlOtherlr.Text, FileSaveAsType.Xml);
                }
                catch
                { dw_otherclr.Reset(); }
                try
                {
                    dw_coll.Reset();
                    dw_coll.ImportString(strList.xml_guarantee, FileSaveAsType.Xml);
                }
                catch { dw_coll.Reset(); }
                try
                {
                    dw_clear.Reset();
                    dw_clear.ImportString(strList.xml_clear, FileSaveAsType.Xml);
                }
                catch { dw_clear.Reset(); }
                try
                {
                    dw_keeping.Reset();
                    dw_keeping.ImportString(strXmlKeep, FileSaveAsType.Xml);
                }
                catch { dw_keeping.Reset(); }
                try
                {
                    dw_reqloop.Reset();
                    dw_reqloop.ImportString(strXmlReqloop, FileSaveAsType.Xml);
                }
                catch { dw_reqloop.Reset(); dw_reqloop.InsertRow(0); }

            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message); }
        }
        private void Refresh()
        { }
        private void RefreshDW()
        {
            try
            {
                //ต้องปรับให้ Reset เฉพาะแถวที่เปลี่ยนแปลงเท่านั้น
                dw_main.Reset();
                dw_main.InsertRow(0);
            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }

        }
        private void ResetColl()
        {
            try
            {
                dw_coll.Reset();
            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
        }
        private void RegenLoanClear()
        {
            String xmlMain = "";
            String xmlColl = "";
            String xmlClear = "";
            String xmlMessage = "";

            xmlMain = dw_main.Describe("DataWindow.Data.XML");
            xmlColl = dw_coll.Describe("DataWindow.Data.XML");
            xmlClear = dw_clear.Describe("DataWindow.Data.XML");

            int result = shrlonService.RegenLoanClear(state.SsWsPass, ref xmlMain, ref xmlClear, ref xmlColl, ref xmlMessage);

            if (result == 1)
            {
                try
                {
                    dw_main.Reset();
                    dw_main.ImportString(xmlMain, FileSaveAsType.Xml);
                    //DwUtil.ImportData(xmlMain, dw_main, tDwMain, FileSaveAsType.Xml);
                    if (dw_main.RowCount > 1) dw_main.DeleteRow(dw_main.RowCount);
                }
                catch { dw_main.Reset(); }
                try
                {
                    dw_clear.Reset();
                    dw_clear.ImportString(xmlClear, FileSaveAsType.Xml);
                }
                catch { dw_clear.Reset(); }
                try
                {
                    dw_coll.Reset();
                    dw_coll.ImportString(xmlColl, FileSaveAsType.Xml);
                }
                catch { dw_coll.Reset(); }
            }
            if ((xmlMessage != null) && (xmlMessage != ""))
            {
                dw_message.Reset();
                dw_message.ImportString(xmlMessage, FileSaveAsType.Xml);
                HdMsg.Value = dw_message.GetItemString(1, "msgtext");
            }
        }
        private void ResumLoanClear()
        {
            //Session["xmlloandetail"]   
            String xmlMain = "";
            String xmlColl = "";
            String xmlClear = "";
            String xmlLoanDetail = "";
            String xmlMessage = "";

            xmlMain = dw_main.Describe("DataWindow.Data.XML");

            xmlLoanDetail = Session["xmlloandetail"].ToString();
            if (dw_coll.RowCount == 0)
            { xmlColl = null; }
            else { xmlColl = dw_coll.Describe("DataWindow.Data.XML"); }

            if (dw_clear.RowCount == 0)
            { xmlClear = null; }
            else { xmlClear = dw_clear.Describe("DataWindow.Data.XML"); }

            int result = shrlonService.ResumLoanClear(state.SsWsPass, ref xmlMain, ref xmlClear, ref xmlColl, xmlLoanDetail, ref xmlMessage);

            if (result == 1)
            {
                try
                {
                    //นำเข้าข้อมูลหลัก
                    dw_main.Reset();
                    dw_main.ImportString(xmlMain, FileSaveAsType.Xml);
                    //DwUtil.ImportData(xmlMain, dw_main, tDwMain, FileSaveAsType.Xml);
                    if (dw_main.RowCount > 1) dw_main.DeleteRow(dw_main.RowCount);
                }
                catch { dw_main.Reset(); dw_main.InsertRow(0); }
                try
                {
                    dw_clear.Reset();
                    dw_clear.ImportString(xmlClear, FileSaveAsType.Xml);
                }
                catch { dw_clear.Reset(); }
                try
                {
                    dw_coll.Reset();
                    dw_coll.ImportString(xmlColl, FileSaveAsType.Xml);
                }
                catch { dw_coll.Reset(); }
                LtXmlLoanDetail.Text = xmlLoanDetail;
            }
            if ((xmlMessage != null) && (xmlMessage != ""))
            {
                dw_message.Reset();
                dw_message.ImportString(xmlMessage, FileSaveAsType.Xml);
                HdMsg.Value = dw_message.GetItemString(1, "msgtext");
            }


        }
        private void SetData()
        {
            str_itemchange strList = new str_itemchange();
            strList = WebUtil.str_itemchange_session(this);

            if (dw_clear.RowCount == 0) strList.xml_clear = null;
            else strList.xml_clear = dw_clear.Describe("DataWindow.Data.XML");
            if (dw_coll.RowCount == 0) strList.xml_guarantee = null;
            else strList.xml_guarantee = dw_coll.Describe("DataWindow.Data.XML");

            Session["strItemchange"] = strList;
        }
        private void SetLoanType()
        {
            Session["loantype"] = dw_main.GetItemString(1, "loantype_code");
            HdReturn.Value = "9";
        }
        private void SetOldData()
        {
            //Method แสดงค่าใบสัญญาเก่า แสดงทางหน้าจอ
            str_itemchange strList = new str_itemchange();
            str_requestopen strRequestOpen = new str_requestopen();
            strList = WebUtil.str_itemchange_session(this);
            string docno = dw_main.GetItemString(1, "loanrequest_docno");
            String strXmlKeep = LtXmlKeeping.Text;
            String strXmlReqloop = LtXmlReqloop.Text;

            strRequestOpen.request_no = docno;
            strRequestOpen.format_type = "CAT";
            strRequestOpen.xml_main = strList.xml_main;
            strRequestOpen.xml_clear = strList.xml_clear;
            strRequestOpen.xml_guarantee = strList.xml_guarantee;
            strRequestOpen.xml_insurance = strList.xml_insurance;
            strRequestOpen.xml_otherclr = strList.xml_otherclr;
            //strRequestOpen.xml_reqperiod = dw_reqloop.Describe("Datawindow.data.XML");                // เพิ่มเติมเฉพาะ ฉุกเฉิน
            strRequestOpen.xml_intspc = "";                     // เพิ่มเติมเฉพาะ ฉุกเฉิน
            try
            {
                strRequestOpen = shrlonService.LoanRequestOpen(state.SsWsPass, strRequestOpen);
            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }

            try
            {
                strXmlReqloop = shrlonService.ReqLoopOpen(state.SsWsPass, strRequestOpen.xml_main);
                if ((strXmlReqloop == "") || (strXmlReqloop == null))
                {
                    dw_reqloop.Reset();
                    dw_reqloop.InsertRow(0);
                    if (dw_reqloop.RowCount > 0) strXmlReqloop = dw_reqloop.Describe("DataWindow.Data.XML");
                    else strXmlReqloop = "";
                }
                //if (strXmlReqloop.Trim() != "") dw_reqloop.ImportString(strXmlReqloop, FileSaveAsType.Xml);
                strXmlReqloop = shrlonService.ItemChangeReqLoop(state.SsWsPass, strRequestOpen.xml_main, strXmlReqloop);
                if ((strXmlReqloop != "") && (strXmlReqloop != null))
                {
                    dw_reqloop.Reset();
                    dw_reqloop.ImportString(strXmlReqloop, FileSaveAsType.Xml);
                }
                else
                {
                    dw_reqloop.Reset();
                    dw_reqloop.InsertRow(0);
                }
            }
            catch (Exception ex)
            { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
            //นำข้อมูลเก็บไว้ใน DataWindow
            dw_main.Reset();
            dw_main.ImportString(strRequestOpen.xml_main, FileSaveAsType.Xml);
            //DwUtil.ImportData(strRequestOpen.xml_main, dw_main, tDwMain, FileSaveAsType.Xml);
            if (dw_main.RowCount > 1) dw_main.DeleteRow(dw_main.RowCount);
            string expendBank = "";
            try
            { expendBank = dw_main.GetItemString(1, "expense_bank"); }
            catch { expendBank = ""; }
            //ตรวจสอบ ธนาคารว่ามีหรือไม่ 
            if (expendBank != "")
            {
                // เรียก Method  ShowBranch() สำหรับแสดงสาขาธนาคาร
                ShowBranch();
            }
            tDwMain.Eng2ThaiAllRow();

            try
            {
                dw_clear.Reset();
                dw_clear.ImportString(strRequestOpen.xml_clear, FileSaveAsType.Xml);
            }
            catch { dw_clear.Reset(); }
            try
            {
                dw_otherclr.Reset();
                dw_otherclr.ImportString(strRequestOpen.xml_otherclr, FileSaveAsType.Xml);
            }
            catch { dw_otherclr.Reset(); }
            try
            {
                dw_coll.Reset();
                dw_coll.ImportString(strRequestOpen.xml_guarantee, FileSaveAsType.Xml);
            }
            catch { dw_coll.Reset(); }

            try
            {
                dw_keeping.Reset();
                dw_keeping.ImportString(strXmlKeep, FileSaveAsType.Xml);
            }
            catch
            {
                dw_keeping.Reset();
            }
            try
            {
                dw_reqloop.Reset();
                dw_reqloop.ImportString(strXmlReqloop, FileSaveAsType.Xml);
            }
            catch { dw_reqloop.Reset(); dw_reqloop.InsertRow(0); }
            Decimal loanrequestStatus = dw_main.GetItemDecimal(1, "loanrequest_status");
            if ((loanrequestStatus != 8) && (loanrequestStatus != 81) && (loanrequestStatus != 11))
            {
                dw_main.DisplayOnly = true;
                dw_clear.DisplayOnly = true;
                dw_coll.DisplayOnly = true;
                dw_keeping.DisplayOnly = true;
                dw_reqloop.DisplayOnly = true;
                dw_message.DisplayOnly = true;
                dw_otherclr.DisplayOnly = true;
            }
            else
            {
                dw_main.DisplayOnly = false;
                dw_clear.DisplayOnly = false;
                dw_coll.DisplayOnly = false;
                dw_keeping.DisplayOnly = false;
                dw_reqloop.DisplayOnly = false;
                dw_message.DisplayOnly = false;
                dw_otherclr.DisplayOnly = false;
            }
            if ((strRequestOpen.xml_message != null) && (strRequestOpen.xml_message != ""))
            {
                dw_message.Reset();
                dw_message.ImportString(strRequestOpen.xml_message, FileSaveAsType.Xml);
                HdMsg.Value = dw_message.GetItemString(1, "msgtext");
                HdReturn.Value = "10"; // กำหนดค่าเพื่อให้เรียก OpenNew() ใหม่อีกครั้ง หลังจากแสดง DLG ใน SheetLoadComplete()
            }
        }
        private void SetOthClr()
        {
            //SetOthClr จะถูกเรียกใช้เมื่อปิดหน้าจอ pop up w_dlg_sl_loanrequest_otherclr.aspx 
            str_itemchange strList = new str_itemchange();

            strList = WebUtil.str_itemchange_session(this);
            strList.xml_main = dw_main.Describe("DataWindow.Data.XML");
            strList.xml_main = shrlonService.SetSumOtherClr(state.SsWsPass, strList.xml_main, strList.xml_otherclr);
            if ((strList.xml_otherclr != null) && (strList.xml_otherclr != ""))
            {
                dw_otherclr.Reset();
                dw_otherclr.ImportString(strList.xml_otherclr, FileSaveAsType.Xml);
            }
            else { dw_otherclr.Reset(); }
            //นำเข้าข้อมูลหลัก
            dw_main.Reset();
            dw_main.ImportString(strList.xml_main, FileSaveAsType.Xml);
            //DwUtil.ImportData(strList.xml_main, dw_main, tDwMain, FileSaveAsType.Xml);
            if (dw_main.RowCount > 1) dw_main.DeleteRow(dw_main.RowCount);
            LtXmlOtherlr.Text = strList.xml_otherclr;
            Session["strItemchange"] = strList;
        }
        private void SetDWOthClr()
        {
            //SetDWOthClr ใช้เมื่อมีการเปลี่ยนแปลงค่าต่างๆ ใน dw_otherclr รายการหักอื่นๆ
            str_itemchange strList = new str_itemchange();

            strList = WebUtil.str_itemchange_session(this);
            strList.xml_main = dw_main.Describe("DataWindow.Data.XML");
            if (dw_otherclr.RowCount == 0)
            {
                strList.xml_otherclr = null;
            }
            else
            {
                strList.xml_otherclr = dw_otherclr.Describe("DataWindow.Data.XML");
            }
            strList.xml_main = shrlonService.SetSumOtherClr(state.SsWsPass, strList.xml_main, strList.xml_otherclr);
            //นำเข้าข้อมูลหลัก
            dw_main.Reset();
            dw_main.ImportString(strList.xml_main, FileSaveAsType.Xml);
            //DwUtil.ImportData(strList.xml_main, dw_main, tDwMain, FileSaveAsType.Xml);
            if (dw_main.RowCount > 1) dw_main.DeleteRow(dw_main.RowCount);
            LtXmlOtherlr.Text = strList.xml_otherclr;
            Session["strItemchange"] = strList;
        }
        private void SetMonthYears()
        {
            Decimal month = 0;
            Decimal year = 0;
            try
            {
                month = Convert.ToDecimal(Session["month"].ToString());
                year = Convert.ToDecimal(Session["year"].ToString());
            }
            catch
            {
                month = 0;
                year = 0;
            }

            if ((month != 0) && (year != 0))
            {
                dw_main.SetItemDecimal(1, "loanrcvperiod_year", year);
                dw_main.SetItemDecimal(1, "loanrcvperiod_month", month);
                //HdColumnName.Value = "loanrcvperiod_month";
                //JsPostMember();
            }
        }
        private void SetRunningNo()
        {
            String runningNo = "";
            runningNo = dw_main.GetItemString(1, "running_no");
            Session["runningNo"] = runningNo;
            LtRunningNo.Text = runningNo;

        }
        private void ShowBranch()
        {
            try
            {
                DwUtil.RetrieveDDDW(dw_main, "expense_branch_1", "sl_loan_requestment.pbl", null);
                String bankCode;
                try { bankCode = dw_main.GetItemString(1, "expense_bank").Trim(); }
                catch { bankCode = ""; }
               
                //if (bankCode == "02")
                //{
                //    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถแสดงสาขาของธนาคารกรุงเทพได้ กรุณาเลือกธนาคารอื่นแทน");
                //    return;
                //}
//                String sql = @"
//                    SELECT  
//                         CMUCFBANKBRANCH.BRANCH_ID,   
//                         CMUCFBANKBRANCH.BRANCH_NAME
//                    FROM CMUCFBANKBRANCH where CMUCFBANKBRANCH.BANK_CODE = '" + bankCode + "'";
//                DataWindowChild dwExpenseBranch = dw_main.GetChild("expense_branch_1");
//                DwUtil.ImportData(sql, dwExpenseBranch, null);
                DataWindowChild dwExpenseBranch = dw_main.GetChild("expense_branch_1");
                //                DwUtil.ImportData(sql, dwExpenseBranch, null);
                dwExpenseBranch.SetFilter("CMUCFBANKBRANCH.BANK_CODE='" + bankCode + "'");
                dwExpenseBranch.Filter();

            }
            catch (Exception ex)
            { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
        }
        private void VisibleChange()
        {
            str_itemchange strList = new str_itemchange();
            string expendCode = "";
            try
            {
                expendCode = dw_main.GetItemString(1, "expense_code");
            }
            catch { }
            if (expendCode == null || expendCode == "") return;

            if ((expendCode == "CHQ") || (expendCode == "TRN") || (expendCode == "CBT") || (expendCode == "DRF") || (expendCode == "TBK"))
            {
                dw_main.Modify("t_32.visible =0");
                dw_main.Modify("postagefee_amt.visible =0");
                dw_main.Modify("t_50.visible =0");
                dw_main.Modify("tax_amt.visible =0");

                dw_main.Modify("t_31.visible =0");
                dw_main.Modify("emsfee_amt.visible =0");
                dw_main.Modify("t_52.visible =0");
                dw_main.Modify("onlinefee_amt.visible =0");

                dw_main.Modify("expense_accid_t.visible =0");
                dw_main.Modify("compute_12.visible =0");

                dw_main.Modify("t_44.visible =0");
                dw_main.Modify("compute_11.visible =0");
                dw_main.Modify("t_47.visible =0");
                dw_main.Modify("postagesrv_amt.visible =0");

                dw_main.Modify("t_53.visible =0");
                dw_main.Modify("paytoorder_desc.visible =0");
                dw_main.Modify("paytoorder_desc_1.visible =0");

                //ฝั่งธนาคาร
                dw_main.Modify("t_20.visible =1");
                dw_main.Modify("expense_bank.visible =1");
                dw_main.Modify("t_30.visible =1");
                dw_main.Modify("expense_bank_1.visible =1");

                dw_main.Modify("t_39.visible =1");
                dw_main.Modify("expense_branch.visible =1");
                dw_main.Modify("t_27.visible =1");
                dw_main.Modify("expense_branch_1.visible =1");

                dw_main.Modify("t_38.visible =1");
                dw_main.Modify("expense_accid.visible =1");

                dw_main.Modify("t_40.visible =1");
                dw_main.Modify("bankfee_amt.visible =1");
                dw_main.Modify("t_45.visible =1");
                dw_main.Modify("banksrv_amt.visible =1");

                dw_main.Modify("t_41.visible =1");
                dw_main.Modify("compute_10.visible =1");
                try
                {
                    DwUtil.RetrieveDDDW(dw_main, "expense_bank_1", "sl_loan_requestment.pbl", null);
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                }
            }
            else if ((expendCode == "CSH") || (expendCode == "BEX") || (expendCode == "MON") || (expendCode == "MOS") || (expendCode == "MOO"))
            {
                dw_main.Modify("t_32.visible =1");
                dw_main.Modify("postagefee_amt.visible =1");
                dw_main.Modify("t_50.visible =1");
                dw_main.Modify("tax_amt.visible =1");

                dw_main.Modify("t_31.visible =1");
                dw_main.Modify("emsfee_amt.visible =1");
                dw_main.Modify("t_52.visible =1");
                dw_main.Modify("onlinefee_amt.visible =1");

                dw_main.Modify("expense_accid_t.visible =1");
                dw_main.Modify("compute_12.visible =1");

                dw_main.Modify("t_44.visible =1");
                dw_main.Modify("compute_11.visible =1");
                dw_main.Modify("t_47.visible =1");
                dw_main.Modify("postagesrv_amt.visible =1");

                if (expendCode == "CSH")
                {
                    dw_main.Modify("t_53.visible =0");
                    dw_main.Modify("paytoorder_desc.visible =0");
                    dw_main.Modify("paytoorder_desc_1.visible =0");
                }
                else
                {
                    dw_main.Modify("t_53.visible =1");
                    dw_main.Modify("paytoorder_desc.visible =1");
                    dw_main.Modify("paytoorder_desc_1.visible =1");
                }

                //ฝั่งธนาคาร
                dw_main.Modify("t_20.visible =0");
                dw_main.Modify("expense_bank.visible =0");
                dw_main.Modify("t_30.visible =0");
                dw_main.Modify("expense_bank_1.visible =0");

                dw_main.Modify("t_39.visible =0");
                dw_main.Modify("expense_branch.visible =0");
                dw_main.Modify("t_27.visible =0");
                dw_main.Modify("expense_branch_1.visible =0");

                dw_main.Modify("t_38.visible =0");
                dw_main.Modify("expense_accid.visible =0");

                dw_main.Modify("t_40.visible =0");
                dw_main.Modify("bankfee_amt.visible =0");
                dw_main.Modify("t_45.visible =0");
                dw_main.Modify("banksrv_amt.visible =0");

                dw_main.Modify("t_41.visible =0");
                dw_main.Modify("compute_10.visible =0");
                if (expendCode == "CSH")
                {
                    try
                    {
                        dw_main.Modify("postagefee_amt.Edit.DisplayOnly=Yes");
                        dw_main.Modify("tax_amt.Edit.DisplayOnly=Yes");
                        dw_main.Modify("emsfee_amt.Edit.DisplayOnly=Yes");
                        dw_main.Modify("onlinefee_amt.Edit.DisplayOnly=Yes");
                        dw_main.Modify("postagesrv_amt.Edit.DisplayOnly=Yes");
                    }
                    catch (Exception ex)
                    { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
                }
                else
                {
                    try
                    {
                        dw_main.Modify("postagefee_amt.Edit.DisplayOnly=No");
                        dw_main.Modify("tax_amt.Edit.DisplayOnly=No");
                        dw_main.Modify("emsfee_amt.Edit.DisplayOnly=No");
                        dw_main.Modify("onlinefee_amt.Edit.DisplayOnly=No");
                        dw_main.Modify("postagesrv_amt.Edit.DisplayOnly=No");
                    }
                    catch (Exception ex)
                    { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
                }
                if ((expendCode == "MON") || (expendCode == "MOS") || (expendCode == "MOO"))
                {
                    strList.xml_main = dw_main.Describe("DataWindow.Data.XML");
                    strList.xml_main = shrlonService.ReCalFee(state.SsWsPass, strList.xml_main);
                    //นำเข้าข้อมูลหลัก
                    dw_main.Reset();
                    dw_main.ImportString(strList.xml_main, FileSaveAsType.Xml);
                    //DwUtil.ImportData(strList.xml_main, dw_main, tDwMain, FileSaveAsType.Xml);
                    if (dw_main.RowCount > 1) dw_main.DeleteRow(dw_main.RowCount);
                }
            }
        }
    }
}
