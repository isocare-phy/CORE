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
    public partial class w_sheet_sl_loanrequest_master_emer : PageWebSheet, WebSheet
    {
        private DwThDate tDwMain;
        private ShrlonClient shrlonService;
        private CommonClient commonService;
        protected String cancelRequest;
        protected String cancelRequestL;
        protected String collCondition;
        protected String collInitP;
        protected String jsPostMember;
        protected String jsPostColl;
        protected String jsPostClear;
        protected String jsPostReqLoop;
        //protected String loadDwCollR;
        protected String openNew;
        protected String openOldDocNo;
        protected String genJs = "";
        protected String visibleChange;
        protected String resetColl;
        protected String resendStr;
        protected String refreshDW;
        protected String regenLoanClear;
        protected String resumLoanClear;
        protected String setData;
        protected String setDWOthClr;
        protected String setLoanType;
        protected String setOldData;
        protected String setOthClr;
        protected String showBranch;

        public void InitJsPostBack()
        {
            cancelRequest = WebUtil.JsPostBack(this, "cancelRequest");
            cancelRequestL = WebUtil.JsPostBack(this, "cancelRequestL");
            collCondition = WebUtil.JsPostBack(this, "collCondition");
            collInitP = WebUtil.JsPostBack(this, "collInitP");
            jsPostMember = WebUtil.JsPostBack(this, "jsPostMember");
            jsPostColl = WebUtil.JsPostBack(this, "jsPostColl");
            jsPostClear = WebUtil.JsPostBack(this, "jsPostClear");
            jsPostReqLoop = WebUtil.JsPostBack(this, "jsPostReqLoop");
            //loadDwCollR = WebUtil.JsPostBack(this, "loadDwCollR");
            openNew = WebUtil.JsPostBack(this, "openNew");
            openOldDocNo = WebUtil.JsPostBack(this, "openOldDocNo");
            resendStr = WebUtil.JsPostBack(this, "resendStr");
            refreshDW = WebUtil.JsPostBack(this, "refreshDW");
            resetColl = WebUtil.JsPostBack(this, "resetColl");
            regenLoanClear = WebUtil.JsPostBack(this, "regenLoanClear");
            resumLoanClear = WebUtil.JsPostBack(this, "resumLoanClear");
            setData = WebUtil.JsPostBack(this, "setData");
            setDWOthClr = WebUtil.JsPostBack(this, "setDWOthClr");
            setLoanType = WebUtil.JsPostBack(this, "setLoanType");
            setOldData = WebUtil.JsPostBack(this, "setOldData");
            setOthClr = WebUtil.JsPostBack(this, "setOthClr");
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
                    loantype = Session["loantype"].ToString();
                    Session.Remove("loantype");
                }
                catch
                {
                    loantype = "10";
                    Session.Remove("loantype");
                }
                //ประกาศ dddw
                try
                {

                    DwUtil.RetrieveDDDW(dw_main, "loantype_code_1", "sl_loan_requestment.pbl", null);
                    DwUtil.RetrieveDDDW(dw_main, "expense_code", "sl_loan_requestment.pbl", null);

                    if (loantype == "")
                    {
                        dw_main.SetItemString(1, "loantype_code", "10");    // กำหนดค่าเริ่มต้น เป็น ฉุกเฉิน
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
                    DwUtil.RetrieveDDDW(dw_main, "paytoorder_desc_1", "sl_loan_requestment.pbl", null);
                    DwUtil.RetrieveDDDW(dw_main, "loanobjective_code", "sl_loan_requestment.pbl", null);
                    DwUtil.RetrieveDDDW(dw_otherclr, "clrothertype_code", "sl_loan_requestment.pbl", null);
                    //DwUtil.RetrieveDDDW(dw_main, "expense_branch_1", "sl_loan_requestment.pbl", null);
                    //ShowBranch();
                }
                catch (Exception ex)
                { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
            }
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
            //else if (eventArg == "loadDwCollR")
            //{
            //    LoadDwCollR();
            //}
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
        }
        public void SaveWebSheet()
        {
            try
            {
                ////ตรวจสอบกรณีเลือกเป็น ธนาณัติ แล้วไม่กรอก ปณ. ให้แจ้งเตือนให้กรอกข้อมูลปณ. ก่อนบันทึก
                //String payToOrder = "";
                //try { payToOrder = dw_main.GetItemString(1, "paytoorder_desc"); }
                //if (dw_main.GetItemString(1, "paytoorder_desc") == "")
                //{
                //    HdReturn.Value = "30";
                //}

                String xmlDwReqLoop = dw_reqloop .Describe("DataWindow.Data.XML");
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
                strSave.xml_reqperiod = xmlDwReqLoop;
                strSave.format_type = "CAT";
                //strSave.branch_id = "001";
                strSave.entry_id = state.SsUsername;
                strSave.coop_id = state.SsCoopId;

                // เรียก service สำหรับบันทึกรายการ
                String runningNo = shrlonService.LoanRightSaveReqloan(state.SsWsPass,ref strSave);
                if ((runningNo != "") && (runningNo != null))
                {
                    // ตรวจสอบค่า และรับค่า running No ตัวถัดไปมาแสดง
                    //LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลเรียบร้อยแล้ว");
                    LtRunningNo.Text = runningNo;
                    //OpenNew();
                    //HdMsg.Value = "บันทึกข้อมูลเรียบร้อยแล้ว";
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
                //เรียก service สำหรับตรวจสอบรายการฉ โอน
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

            //เพิ่มแถว dw_coll ให้ 1 แถวโดยอัตโนมัติ (ปาล์ม แก้ไข)
            String columnName = HdColumnName.Value;
            if (dw_coll.RowCount < 1) //&& (columnName != "")) //&& 
            {
                //HdColumnName.Value = "";
                dw_coll.InsertRow(0);
            }

            //กรณีเลือก ธนาณัติแบบต่างๆ ให้ เอา สังกัดมาใส่ให้อัตโนมัติ
            if (columnName == "expense_code")
            {
                dw_main.SetItemString(1, "paytoorder_desc", dw_main.GetItemString(1, "membgroup_code"));

            }
            //ตั้งค่าให้ dropdown เป็น 006
            //string expendCode = "";
            //try
            //{
            //    expendCode = dw_main.GetItemString(1, "expense_code");

            //    if ((expendCode == "CHQ") || (expendCode == "TRN") || (expendCode == "CBT") || (expendCode == "DRF") || (expendCode == "TBK"))
            //    {
            //        dw_main.SetItemString(1, "expense_bank", "006");

            //    }
            //}
            //catch { }
            // CollCondition เป็น method สำหรับ เฉลี่ย % ของผู้ค้ำประกันให้เท่ากับ 100 %
           
            VisibleChange();

            dw_main.SaveDataCache();
            dw_coll.SaveDataCache();
            dw_clear.SaveDataCache();
            dw_keeping.SaveDataCache();
            dw_reqloop.SaveDataCache();
            dw_otherclr.SaveDataCache();
        }
        private void CancelRequest()
        {
            // CancelRequest เป็น Method สำหรับยกเลิกใบคำขอกู้ (แต่ไม่ได้บันทึกลงในฐานข้อมูล เปลี่ยนสถานะเป็นยกเลิกเท่านั้น)
            String dwXmlMain = dw_main.Describe("DataWindow.Data.XML");
            String dwXmlMessage = "";
            String cancelID = "isocare";
            String branch = state.SsCoopId;
            try
            {
                // เรียก Service ส่วนแสดงยกเลิกใบคำขอกู้
                int result = shrlonService.CancelRequest(state.SsWsPass, ref dwXmlMain, ref dwXmlMessage, cancelID, branch);
                if (result <= 1)
                {
                    //นำเข้าข้อมูลหลัก
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
            //ตรวจสอบ Error Message ว่ามีหรือไม่
            if ((dwXmlMessage != "") && (dwXmlMessage != null))
            {
                dw_message.Reset();
                dw_message.ImportString(dwXmlMessage, FileSaveAsType.Xml);
                HdMsg.Value = dw_message.GetItemString(1, "msgtext");
            }
        }
        private void CancelRequestL()
        {
            // CancelRequestL เป็น Method สำหรับยกเลิกสัญญาฉ. โอน 
            String dwXmlMain = dw_main.Describe("DataWindow.Data.XML");
            String cancelID = "isocare";
            String branch = state.SsCoopId;
            String conLoopNo = "";
            try
            { conLoopNo = dw_reqloop.GetItemString(1, "contractloop_no");}
            catch { conLoopNo = null; }
            //ตรวจสอบเลขที่สัญญา ฉ. โอนว่ามีหรือไม่ 
            if ((conLoopNo != null) && (conLoopNo != ""))
            {
                //เรียก Service สำหรับยกเลิกรายละเอียดฉ. โอน
                int result = shrlonService.CancelReqLoop(state.SsWsPass, ref dwXmlMain, cancelID, branch);
                if (result == 1)
                {
                    LtServerMessage.Text = WebUtil.CompleteMessage("ยกเลิกสัญญา ฉ. โอนเรียบร้อยแล้ว");
                    try
                    {
                        dw_main.Reset();
                        dw_main.ImportString(dwXmlMain, FileSaveAsType.Xml);
                        if (dw_main.RowCount > 1) dw_main.DeleteRow(dw_main.RowCount);

                        dw_reqloop.Reset();
                        dw_reqloop.InsertRow(0);
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
            // CollCondition เป็น method สำหรับ เฉลี่ย % ของผู้ค้ำประกันให้เท่ากับ 100 %
            String dwXmlMain = dw_main.Describe("DataWindow.Data.XML");
            String dwXmlColl = dw_coll.Describe("DataWindow.Data.XML");
            
            String dwXmlMessage = "";
            // เรียก Service สำหรับเฉลี่ยค่า % ให้มีค่าเท่ากับ 100 %
            dwXmlColl = shrlonService.CollPercCondition(state.SsWsPass, dwXmlMain, dwXmlColl, ref dwXmlMessage);
            //Label20.Text = dwXmlColl;
            try
            {
                if ((dwXmlColl != "") && (dwXmlColl != null))
                {
                    dw_coll.Reset();
                    dw_coll.ImportString(dwXmlColl, FileSaveAsType.Xml);
                }
            }
            catch { dw_coll.Reset(); }
            //ตรวจสอบ Error Message ว่ามีหรือไม่
            if ((dwXmlMessage != "") && (dwXmlMessage != null))
            {
                dw_message.Reset();
                dw_message.ImportString(dwXmlMessage, FileSaveAsType.Xml);
                HdMsg.Value = dw_message.GetItemString(1, "msgtext");
            }
        }
        private void CollInitP()
        {
            // CollInitP เป็น Method สำหรับ กำหนดค่า % ให้กับผู้ค้ำประกัน
            String dwXmlMain = dw_main.Describe("DataWindow.Data.XML");
            String dwXmlColl = dw_coll.Describe("DataWindow.Data.XML");
            // เรียก Service สำหรับกำหนดค่า % ให้
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
            { dw_coll.Reset();   }
        }
        //private void JsPostMember()
        //{
        //    //JsPostMember เป็นMethod ที่ใช้สำหรับกรณีมีการเปลี่ยนแปลงค่าใน DW_Main
        //    try
        //    {
        //        //กำหนดค่าเริ่มต้น
        //        String columnName = HdColumnName.Value;
        //        String dwMainXML = dw_main.Describe("DataWindow.Data.XML");
        //        String dwCollXML = dw_coll.Describe("DataWindow.Data.XML");
        //        String dwClearXML = dw_clear.Describe("DataWindow.Data.XML");
        //        String t = "20";
        //        t = dw_main.GetItemString(1, "loantype_code");
        //        String dwkeepingXML = null;
        //        String dwreqloopXML = null;
        //        if (dw_coll.RowCount < 1) dwCollXML = null;

        //        str_itemchange strList = new str_itemchange();
        //        strList.column_name = columnName;
        //        strList.xml_main = dwMainXML;
        //        strList.xml_guarantee = dwCollXML;
        //        strList.xml_clear = dwClearXML;
        //        strList.import_flag = true;
        //        strList.format_type = "CAT";
        //        //เรียก Service  สำหรับ itemChange DW_Main 
        //        int result = shrlonService.LoanRightItemChangeMain(state.SsWsPass, ref strList);
        //        if ((columnName == "loantype_code") || (columnName == "loantype_code_1"))
        //        {
        //            //กรณีเปลี่ยนแปลงประเภทเงินกู้
        //            try
        //            {
        //                //เรียก service สำหรับดึงข้อมูลรายการเรียกเก็บล่าสุดมาแสดง
        //                dwkeepingXML = shrlonService.GetLastKeeping(state.SsWsPass, strList.xml_main);
        //                if (dw_reqloop.RowCount > 0) dwreqloopXML = dw_reqloop.Describe("DataWindow.Data.XML");
        //                else dwreqloopXML = "";
        //                //เรียก service สำหรับรายการเปลี่ยนแปลง DW_ReqLoop
        //                dwreqloopXML = shrlonService.ItemChangeReqLoop(state.SsWsPass, strList.xml_main, dwreqloopXML);
        //                Session["xml_reqloop"] = dwreqloopXML;
        //            }
        //            catch (Exception ex)
        //            { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
        //        }
        //        else if (columnName == "member_no")
        //        {
        //            // กรณีเปลี่ยนแปลง เลขทะเบียน
        //            try
        //            {
        //                //    dw_reqloop.Reset();
        //                //เรียก service สำหรับดึงข้อมูลรายการเรียกเก็บล่าสุดมาแสดง
        //                dwkeepingXML = shrlonService.GetLastKeeping(state.SsWsPass, strList.xml_main);
        //                // เรียก service สำหรับดึงข้อมูลรายการฉ.โอน มาแสดง
        //                dwreqloopXML = shrlonService.ReqLoopOpen(state.SsWsPass, strList.xml_main);
        //                dw_reqloop.Reset();
        //                if (dwreqloopXML.Trim() != "") dw_reqloop.ImportString(dwreqloopXML, FileSaveAsType.Xml);
        //                //เรียก service สำหรับรายการเปลี่ยนแปลง DW_ReqLoop
        //                dwreqloopXML = shrlonService.ItemChangeReqLoop(state.SsWsPass, strList.xml_main, dwreqloopXML);

        //                if ((dwkeepingXML != "") && (dwkeepingXML != null))
        //                {
        //                    dw_keeping.Reset();
        //                    dw_keeping.ImportString(dwkeepingXML, FileSaveAsType.Xml);
        //                }
        //                if ((dwreqloopXML != "") && (dwreqloopXML != null))
        //                {
        //                    dw_reqloop.Reset();
        //                    dw_reqloop.ImportString(dwreqloopXML, FileSaveAsType.Xml);
        //                }
        //                else
        //                { dw_reqloop.InsertRow(0); }

        //            }
        //            catch (Exception ex)
        //            { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
        //        }
        //        else if (columnName == "loanrequest_amt")
        //        {
        //            // กรณีเปลี่ยนแปลงค่า จำนวนเงินขอกู้
        //            if (dw_reqloop.RowCount > 0) dwreqloopXML = dw_reqloop.Describe("DataWindow.Data.XML");
        //            else dwreqloopXML = "";
        //            // เรียก service สำหรับคำนวณค่าธรรมเนียมให้ใหม่
        //            strList.xml_main = shrlonService.ReCalFee(state.SsWsPass, strList.xml_main);
        //            //เรียก service สำหรับรายการเปลี่ยนแปลง DW_ReqLoop
        //            dwreqloopXML = shrlonService.ItemChangeReqLoop(state.SsWsPass, strList.xml_main, dwreqloopXML);
        //            dw_reqloop.Reset();

        //            if ((dwreqloopXML != "") && (dwreqloopXML != null))
        //            {
        //                dw_reqloop.Reset();
        //                dw_reqloop.ImportString(dwreqloopXML, FileSaveAsType.Xml);
        //            }
        //            DwUtil.ImportData(strList.xml_main, dw_main, tDwMain, FileSaveAsType.Xml);
        //            if (dw_main.RowCount > 1) dw_main.DeleteRow(dw_main.RowCount);
        //        }

        //        LtXmlKeeping.Text = dwkeepingXML;
        //        LtXmlReqloop.Text = dwreqloopXML;
        //        Session["strItemchange"] = strList;
        //        //ตรวจสอบว่ามี Error Message หรือไม่
        //        if ((strList.xml_message != null) && (strList.xml_message != ""))
        //        {
        //            dw_message.Reset();
        //            dw_message.ImportString(strList.xml_message, FileSaveAsType.Xml);
        //            HdMsg.Value = dw_message.GetItemString(1, "msgtext");
        //        }
        //        //กรณีค่า 8 ต้องตรวจสอบอีกครั้งใน SheetLoadComplete
        //        if (result == 8)
        //        {
        //            HdReturn.Value = result.ToString();
        //            HdColumnName.Value = strList.column_name;
        //            DwUtil.ImportData(strList.xml_main, dw_main, tDwMain, FileSaveAsType.Xml);
        //            if (dw_main.RowCount > 1) dw_main.DeleteRow(dw_main.RowCount);
        //            HdMemberNo.Value = dw_main.GetItemString(1, "member_no");
        //        }
        //        else
        //        {
        //            try
        //            {
        //                dw_main.Reset();
        //                dw_main.ImportString(strList.xml_main, FileSaveAsType.Xml);
        //                if (dw_main.RowCount > 1) dw_main.DeleteRow(dw_main.RowCount);
        //            }
        //            catch (Exception ex) {
        //                LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
        //            }
        //        }
        //        tDwMain.Eng2ThaiAllRow();
        //        // ตรวจสอบกรณี เปลี่ยนแปลงค่า ประเภทจ่ายเงินกู้เป็น
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
        //                //โอนภายในระบบ เรียกเลขที่บัญชีของสมาชิก
        //                HdMemberNo.Value = dw_main.GetItemString(1, "member_no");
        //                HdReturn.Value = "15"; // กำหนดค่าเพื่อให้เรียก DLG เลือกบัญชีเงินฝาก ใน SheetLoadComplete()
        //            }
        //        }
        //      //นำค่า XML import เข้าใน DW
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
        //        catch { dw_coll.Reset(); }
        //        try
        //        {
        //            dw_clear.Reset();
        //            dw_clear.ImportString(strList.xml_clear, FileSaveAsType.Xml);
        //        }
        //        catch
        //        { dw_clear.Reset();}
        //        if (dw_reqloop.RowCount < 0)
        //        {
        //            dw_reqloop.Reset();
        //            dw_reqloop.InsertRow(0);
        //        }
        //        if ((LtRunningNo.Text != null) && (LtRunningNo.Text != ""))
        //        {
        //            dw_main.SetItemString(1, "runing_no", LtRunningNo.Text);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LtServerMessage.Text = WebUtil.ErrorMessage(ex);
        //        //HdMsg.Value = "ไม่สามารถเข้าใช้งานได้ กรุณาติดต่อผู้ดูแลระบบ";
        //        //OpenNew(); 
        //        HdReturn.Value = "12"; // กำหนดค่าเพื่อให้เรียก OpenNew() ใหม่อีกครั้ง หลังจากแสดง DLG ใน SheetLoadComplete()
        //    }
        //}
        private void JsPostMember()
        {
            try
            {
                String columnName = HdColumnName.Value;
                String dwMainXML = dw_main.Describe("DataWindow.Data.XML");
                String dwCollXML = dw_coll.Describe("DataWindow.Data.XML");
                String dwClearXML = dw_clear.Describe("DataWindow.Data.XML");
                String t = "10";
                t = dw_main.GetItemString(1, "loantype_code");
                String dwkeepingXML = null;
                String dwreqloopXML = null;


                str_itemchange strList = new str_itemchange();
                strList.column_name = columnName;
                strList.xml_main = dwMainXML;
                strList.xml_guarantee = dwCollXML;
                strList.xml_clear = dwClearXML;
                strList.import_flag = true;
                strList.format_type = "CAT";


                int result = shrlonService.LoanRightItemChangeMain(state.SsWsPass, ref strList);
                if ((columnName == "loantype_code") || (columnName == "loantype_code_1"))
                {
                   
                }
                else if (columnName == "member_no")
                {
                    
                }
                else if (columnName == "loanrequest_amt")
                {
                    strList.xml_main = shrlonService.ReCalFee(state.SsWsPass, strList.xml_main);
                }
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
                //Boom เอาข้อมูลส่วนหลัก แสดงทางหน้าจอ
                dw_main.Reset();
                dw_main.ImportString(strList.xml_main, FileSaveAsType.Xml);
                //DwUtil.ImportData(strList.xml_main, dw_main, tDwMain, FileSaveAsType.Xml);
              //  ShowBranch();
                if (dw_main.RowCount > 1) dw_main.DeleteRow(dw_main.RowCount);

                //Boom ตรวจสอบขั้นตอนว่าจะต้องดำเนินการต่อไปหรือไม่
                if (result == 8)
                {
                    HdReturn.Value = result.ToString();
                    HdColumnName.Value = strList.column_name;
                    HdMemberNo.Value = dw_main.GetItemString(1, "member_no");
                }

                if (columnName == "expense_code")
                {
                    string expendCode = "";
                    try
                    {
                        expendCode = dw_main.GetItemString(1, "expense_code");
                    }
                    catch { }
                    if (expendCode == "TRN")
                    {
                        //โอนภายใน เรียกเลขที่บัญชีของสมาชิก
                        HdMemberNo.Value = dw_main.GetItemString(1, "member_no");
                        HdReturn.Value = "15"; // กำหนดค่าเพื่อให้เรียก DLG เลือกบัญชีเงินฝาก ใน SheetLoadComplete()
                    }
                    else if (expendCode == "CBT")
                    {
                        ShowBranch();
                    }
                }
                tDwMain.Eng2ThaiAllRow();
                //if (dw_main.RowCount > 1) dw_main.DeleteRow(dw_main.RowCount);
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
                catch
                {
                    dw_coll.Reset();
                }
                try
                {
                    dw_clear.Reset();
                    dw_clear.ImportString(strList.xml_clear, FileSaveAsType.Xml);
                }
                catch { dw_clear.Reset(); }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                HdReturn.Value = "12"; // กำหนดค่าเพื่อให้เรียก OpenNew() ใหม่อีกครั้ง หลังจากแสดง DLG ใน SheetLoadComplete()
            }
        }
        private void JsPostColl()
        {
            //JSPostColl เป็น Method สำหรับ มีการเปลี่ยนแปลงค่า ใน Dw_Coll
            try
            {
                // กำหนดค่าเริ่มต้น
                String strXmlKeep = LtXmlKeeping.Text;
                String strXmlReqloop = LtXmlReqloop.Text;
                String columnName = HdColumnName.Value;
                String dwMainXML = dw_main.Describe("DataWindow.Data.XML");
                String dwCollXML = dw_coll.Describe("DataWindow.Data.XML");
                String dwClearXML = dw_clear.Describe("DataWindow.Data.XML");
                String t = dw_main.GetItemString(1, "loantype_code");
                str_itemchange strList = new str_itemchange();
                // ตรวจสอบว่า HdRowNumber มีค่าหรือไม่ ถ้าไม่มี ไม่ต้องทำอะไร
                if (HdRowNumber.Value.ToString() != "") {
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
                    {refCollNo = "";}
                    strList.column_name = columnName;
                    strList.xml_main = dwMainXML;
                    strList.xml_guarantee = dwCollXML;
                    strList.xml_clear = dwClearXML;
                    strList.import_flag = true;
                    strList.format_type = "CAT";
                    strList.column_data = dw_coll.GetItemString(rowNumber, "loancolltype_code") + refCollNo;
                    // เรียก service สำหรับการเปลี่ยนแปลงค่า ของ DW_coll
                    int result = shrlonService.LoanRightItemChangeColl(state.SsWsPass, ref strList);
                    Session["strItemchange"] = strList;
                    // ตรวจสอบว่ามี Error Message หรือไม่
                    if ((strList.xml_message != null) && (strList.xml_message != ""))
                    {
                        dw_message.Reset();
                        dw_message.ImportString(strList.xml_message, FileSaveAsType.Xml);
                        HdMsg.Value = dw_message.GetItemString(1, "msgtext");
                        HdMsgWarning.Value = dw_message.GetItemString(1, "msgicon");
                    }
                    //กรณี มีค่าเป็น 8 ให้ไปตรวจสอบ ใน SheetLoadComplete() อีกครั้ง
                    if (result == 8)
                    {
                        HdReturn.Value = result.ToString();
                        HdColumnName.Value = strList.column_name;
                    }
                    // ImportString XML ลงใน DW
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
            // JsPostClear เป็น Method ที่ใช้สำหรับ มีการเปลี่ยนแปลงค่าใน DW_Clear (กรณีเงินกู้ฉุกเฉินไม่ได้ใช้งาน)
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
                //เรียก service สำหรับการเปลี่ยนแปลงค่าของ Dw_Clear
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
                    //dw_keeping.InsertRow(0); 
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
            // JsPostReqLoop เป็น Method สำหรับกรณีมีการเปลี่ยนแปลงค่าใน Dw_Reqloop (เงินกู้ฉุกเฉินไม่ได้ใช้งาน)
            String xmlMain = dw_main.Describe("DataWindow.Data.XML");
            String xmlReqLoop = dw_reqloop.Describe("DataWindow.Data.XML");
            try
            {
                // เรียก service สำหรับการเปลี่ยนแปลงค่า Dw_reqloop
                xmlReqLoop = shrlonService.ItemChangeReqLoop(state.SsWsPass, xmlMain, xmlReqLoop);
                DwUtil.ImportData(xmlReqLoop, dw_reqloop, null, FileSaveAsType.Xml);
            }
            catch
            {
                dw_reqloop.Reset();
                dw_reqloop.InsertRow(0);
            }


        }
        //private void LoadDwCollR()
        //{
        //    try
        //    {
        //        str_itemchange strList = new str_itemchange();
        //        strList = WebUtil.str_itemchange_session(this);
        //        dw_coll.Reset();
        //        dw_coll.ImportString(strList.xml_guarantee, FileSaveAsType.Xml);

        //    }
        //    catch (Exception ex)
        //    {
        //        LtServerMessage.Text = WebUtil.ErrorMessage(ex);
        //    }
        //}
        private void OpenNew()
        {
            //OpenNew เป็น Method สำหรับ กรณีเคลียร์ค่า
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
                dw_main.SetItemString(1, "loantype_code", "10");    // กำหนดค่าเริ่มต้น เป็น ฉุกเฉิน
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
            dw_message.Visible = false;
        }
        private void OpenOldDocNo()
        {
            //OpenOldDocNo เป็น Method สำหรับเปิดข้อมูลสัญญาเก่ามาแสดง โดยรับค่าจากหน้าจอค้นหาใบคำขอกู้ฉุกเฉิน
            //กำหนดค่าเริ่มต้น
            str_itemchange strList = new str_itemchange();
            str_requestopen strRequestOpen = new str_requestopen();
            String docno = dw_main.GetItemString(1, "loanrequest_docno");
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
                // เรียก service สำหรับเรียข้อมูลใบคำขอกู้อันเก่า
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
            {expendBank = dw_main.GetItemString(1, "expense_bank");}
            catch { expendBank = ""; }
            //ตรวจสอบ ธนาคารว่ามีหรือไม่ 
            if (expendBank != "")
            {
                // เรียก Method  ShowBranch() สำหรับแสดงสาขาธนาคาร
                ShowBranch();
            }
            tDwMain.Eng2ThaiAllRow();
            // ImportSting ค่า XML ลงใน DW
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
            {   // เรียก servier สำหรับดึงรายการเรียกเก็บล่าสุดมาแสดง
                dwkeepingXML = shrlonService.GetLastKeeping(state.SsWsPass, strRequestOpen.xml_main);
                // เรียก service สำหรับดึงข้อมูลรายการฉ.โอน
                dwreqloopXML = shrlonService.ReqLoopOpen(state.SsWsPass, strRequestOpen.xml_main);
                if (dwreqloopXML.Trim() != "") dw_reqloop.ImportString(dwreqloopXML, FileSaveAsType.Xml);
                // เรียก service สำหรับการเปลี่ยนแปลงรายการ DW_ReqLoop
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

            if ((strRequestOpen.xml_message != null) && (strRequestOpen.xml_message != ""))
            {
                dw_message.Reset();
                dw_message.ImportString(strRequestOpen.xml_message, FileSaveAsType.Xml);
                HdMsg.Value = dw_message.GetItemString(1, "msgtext");
            }
            Decimal loanrequestStatus = dw_main.GetItemDecimal(1, "loanrequest_status");
            // กำหนดค่า กรณีประเภทใบคำขอกู้เป็น ประเภท 8, 81 และ 11 สามารถแก้ไขได้
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
        private void ResendStr()
        {
            // ResendStr เป็น Method สำหรับกรณีมีใบคำขอกู้เก่าอยู่แล้ว แล้วอยากกรอกข้อมูลเข้าไปใหม่
            try
            {
                genJs = "";
                str_itemchange strList = new str_itemchange();
                strList = WebUtil.str_itemchange_session(this);
                String strXmlKeep = LtXmlKeeping.Text;
                String strXmlReqloop = LtXmlReqloop.Text;
                strList.xml_message = null;
                //เรียก service สำหรับการเปลี่ยนแปลงค่าของ Dw_main
                int result = shrlonService.LoanRightItemChangeMain(state.SsWsPass, ref strList);
                Session["strItemchange"] = strList;
                // ตรวจสอบ Error Message ว่ามีหรือไม่
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
                // Import String XML เข้าไปใน DW
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
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
        }
        private void RefreshDW()
        {
            // RefreshDW เป็น Method สำหรับ Reset Dw_main
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
             // ResetColl เป็น Method สำหรับ Reset Dw_coll
            try
            {
                dw_coll.Reset();
            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
        }
        private void RegenLoanClear()
        {
            // RegenLoanClear เป็น Method สำหรับ สร้างรายการหักกลบใหม่ (เงินกู้ฉุกเฉินไม่ได้ใช้งาน)
            String xmlMain = "";
            String xmlColl = "";
            String xmlClear = "";
            String xmlMessage = "";

            xmlMain = dw_main.Describe("DataWindow.Data.XML");
            xmlColl = dw_coll.Describe("DataWindow.Data.XML");
            xmlClear = dw_clear.Describe("DataWindow.Data.XML");
            // เรียก Service สำหรับสร้างรายการหักกลบ ใหม่
            int result = shrlonService.RegenLoanClear(state.SsWsPass, ref xmlMain, ref xmlClear, ref xmlColl, ref xmlMessage);

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
            // ตรวจสอบ Error Message ว่ามีหรือไม่
            if ((xmlMessage != null) && (xmlMessage != ""))
            {
                dw_message.Reset();
                dw_message.ImportString(xmlMessage, FileSaveAsType.Xml);
                HdMsg.Value = dw_message.GetItemString(1, "msgtext");
            }
        }
        private void ResumLoanClear()
        {
            //ResumLoanClear เป็น Method สำหรับ คำนวณค่าหักกลบใหม่ ( เงินกู้ฉุกเฉินไม่ได้ใช้งาน)
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
            // เรียก service สำหรับคำนวณค่าหักกลบใหม่
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
            // ตรวจสอบว่า มี Error Message หรือไม่
            if ((xmlMessage != null) && (xmlMessage != ""))
            {
                dw_message.Reset();
                dw_message.ImportString(xmlMessage, FileSaveAsType.Xml);
                HdMsg.Value = dw_message.GetItemString(1, "msgtext");
            }


        }
        private void SetData()
        {
            // SetData เป็น Method สำหรับ กำหนดข้อมูลลงใน Session ก่อนแสดงค่าในรายละเอียดข้อมูล
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
            //SetLoanType เป็น Method สำหรับการส่งค่า loantype_code กรณีเปลี่ยนค่าประเภทเงินกู้
            Session["loantype"] = dw_main.GetItemString(1, "loantype_code");
            HdReturn.Value = "9";
        }
        private void SetOldData()
        {
            // SetOldData เป็น Method สำหรับเปิดข้อมูลสัญญาเก่ามาแสดง โดยรับค่าจากหน้าจอเลขที่ใบสัญญาเดิม w_dlg_sl_loanrequest_duplicate
            str_itemchange strList = new str_itemchange();
            str_requestopen strRequestOpen = new str_requestopen();
            strList = WebUtil.str_itemchange_session(this);
            string docno = dw_main.GetItemString(1, "loanrequest_docno");
            String strXmlKeep = null;
            String strXmlReqloop = null;

            strRequestOpen.request_no = docno;
            strRequestOpen.format_type = "CAT";
            strRequestOpen.xml_main = strList.xml_main;
            strRequestOpen.xml_clear = strList.xml_clear;
            strRequestOpen.xml_guarantee = strList.xml_guarantee;
            strRequestOpen.xml_insurance = strList.xml_insurance;
            strRequestOpen.xml_otherclr = strList.xml_otherclr;
            strRequestOpen.xml_intspc = "";                     // เพิ่มเติมเฉพาะ ฉุกเฉิน
            
          
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
            //ShowBranch();
            //Boom ส่วน ฉ.โอน Reqloop
            try
            {
                strXmlReqloop = string.IsNullOrEmpty(strRequestOpen.xml_reqloop) ? "" : strRequestOpen.xml_reqloop;
                //Session["xml_reqloop"] = dwreqloopXML;
                dw_reqloop.Reset();
                if (strXmlReqloop.Trim() != "")
                {
                    dw_reqloop.ImportString(strXmlReqloop, FileSaveAsType.Xml);
                }
                else { dw_reqloop.InsertRow(0); }
            }
            catch { dw_reqloop.Reset(); dw_reqloop.InsertRow(0); }
            

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
            
        //Boom
            //strXmlReqloop = shrlonService.ReqLoopOpen(state.SsWsPass, strRequestOpen.xml_main);
            //try
            //{
            //    dw_reqloop.Reset();
            //    dw_reqloop.ImportString(strXmlReqloop, FileSaveAsType.Xml);
            //}
            //catch { dw_reqloop.Reset(); dw_reqloop.InsertRow(0); }
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
            // เรียก service สำหรับกำหนดค่าผลรงมรายการหักอื่นๆ
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
            // เรียก service สำหรับกำหนดค่าผลรงมรายการหักอื่นๆ
            strList.xml_main = shrlonService.SetSumOtherClr(state.SsWsPass, strList.xml_main, strList.xml_otherclr);
            //นำเข้าข้อมูลหลัก
            dw_main.Reset();
            dw_main.ImportString(strList.xml_main, FileSaveAsType.Xml);
            //DwUtil.ImportData(strList.xml_main, dw_main, tDwMain, FileSaveAsType.Xml);
            if (dw_main.RowCount > 1) dw_main.DeleteRow(dw_main.RowCount);
            LtXmlOtherlr.Text = strList.xml_otherclr;
            Session["strItemchange"] = strList;
        }
        private void ShowBranch()
        {
            try
            {
                DwUtil.RetrieveDDDW(dw_main, "expense_branch_1", "sl_loan_requestment.pbl", null);
                String bankCode;
                try { bankCode = dw_main.GetItemString(1, "expense_bank").Trim(); }
                catch { bankCode = ""; }
              
                String formateAccID = "";
                //Boolean uiFormat = true;
                if (bankCode != "")
                {
                    //formateAccID = shrlonService.GetSavingFormat(state.SsWsPass, bankCode, uiFormat);
                    formateAccID = "@@@-@-@@@@@-@";
                }
                HdAccID.Value = formateAccID;

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
            // VisibleChange เป็น Method สำหรับ กำหนดการแสดงค่าตาม ประเภทการจ่ายเงินกู้
            str_itemchange strList = new str_itemchange();
            strList = WebUtil.str_itemchange_session(this);
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

                //ซ่อนช่องยอดรวม
                dw_main.Modify("t_51.visible =1"); //ปาล์ม edit
                dw_main.Modify("compute_15.visible =1"); //ปาล์ม edit 


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

                dw_main.Modify("t_51.visible =0");
                dw_main.Modify("compute_15.visible =0");
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
                    if ((strList.xml_main == null) || (strList.xml_main == ""))
                    {
                        strList.xml_main = dw_main.Describe("DataWindow.Data.XML");
                        //เรียก service สำหรับคำนวณค่าธรรมเนียมใหม่
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
}

