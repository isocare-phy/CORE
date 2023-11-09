using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Saving.WcfCommon;
using Saving.WcfShrlon;
using Sybase.DataWindow;
using System.Web.Services.Protocols;

namespace Saving.Applications.shrlon
{
    public partial class w_sheet_sl_loanreqeust_return : PageWebSheet, WebSheet
    {
        private ShrlonClient shrlonService;
        private CommonClient commonService;
        private DwThDate tDwMain;

        protected String cancelReturn;
        protected String jsPostReqReturn;
        protected String openReqReturn;
        protected String refresh;
        //*********************report****************************
        protected String app;
        protected String gid;
        protected String rid;
        protected String pdf;
        protected String runProcess;
        protected String popupReport;
        public void InitJsPostBack()
        {
            jsPostReqReturn = WebUtil.JsPostBack(this, "jsPostReqReturn");
            cancelReturn = WebUtil.JsPostBack(this, "cancelReturn");
            openReqReturn = WebUtil.JsPostBack(this, "openReqReturn");
            refresh = WebUtil.JsPostBack(this, "refresh");
            //**************report************************

            HdOpenIFrame.Value = "False";
            runProcess = WebUtil.JsPostBack(this, "runProcess");
            popupReport = WebUtil.JsPostBack(this, "popupReport");

            tDwMain = new DwThDate(dw_head, this);
            tDwMain.Add("operate_date", "operate_tdate");
        }
        public void WebSheetLoadBegin()
        {
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
                this.RestoreContextDw(dw_head);
                this.RestoreContextDw(dw_detail);
            }
            else
            {
                dw_head.Reset();
                dw_head.InsertRow(0);
            }
            dw_head.SetItemDateTime(1, "operate_date", state.SsWorkDate);
            DwUtil.RetrieveDDDW(dw_head, "loantype_code_1", "sl_loan_requestment.pbl", null);
            tDwMain.Eng2ThaiAllRow();
        }
        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "jsPostReqReturn")
            {
                JsPostReqReturn();
            }
            else if (eventArg == "cancelReturn")
            {
                CancelReturn();
            }
            else if (eventArg == "refresh")
            {
                Refresh();
            }
            else if (eventArg == "openReqReturn")
            {
                OpenReqReturn();
            }
            else if (eventArg == "popupReport")
            {
                PopupReport();
            }

        }
        public void SaveWebSheet()
        {
            try
            {
                String xmlDwHead = "";
                String xmlDwDetail = "";
                String entryID = "";
                String branchID = "";

                try
                {
                    xmlDwHead = dw_head.Describe("DataWindow.Data.XML");
                }
                catch
                {
                    xmlDwHead = "";
                }
                try
                {
                    xmlDwDetail = dw_detail.Describe("DataWindow.Data.XML");
                }
                catch
                {
                    xmlDwDetail = "";
                }

                entryID = state.SsUsername;
                branchID = state.SsCoopId;

                short result = shrlonService.SaveReqReturn(state.SsWsPass, xmlDwHead, xmlDwDetail, entryID, branchID);
                if (result == 1)
                {
                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลเรียบร้อยแล้ว");
                    HdReturn.Value = "11"; //  SheetLoadComplete()
                    RunProcess();
                }
                else
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถบันทึกข้อมูลได้");
                }

            }
            catch(Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }
        public void WebSheetLoadEnd()
        {

            //DwUtil.RetrieveDDDW(dw_main, "paytoorder_desc_1", "sl_loan_requestment.pbl", null);
            

            dw_head.SaveDataCache();
            dw_detail.SaveDataCache();
        }
        public void JsPostReqReturn()
        {
            String xmlDwHead = "";
            String xmlDwDetail = "";
            String xmlDwMsg = "";
            String memberNo = HdMemberNo.Value;
            HdMemberNo.Value = "";

            if ((memberNo != "") && (memberNo != null))
            {
                try
                {
                    int rowcount = dw_head.RowCount;
                    if (dw_head.RowCount != 1)
                    {
                        dw_head.Reset();
                        dw_head.InsertRow(0);
                        dw_head.SetItemString(1, "member_no", memberNo);
                    }
                    else
                    {dw_head.SetItemString(1, "member_no", memberNo);}
                }
                catch (Exception ex)
                {LtServerMessage.Text = WebUtil.ErrorMessage(ex);}
            }

            try
            {xmlDwHead = dw_head.Describe("DataWindow.Data.XML");}
            catch { xmlDwHead = ""; }
            try
            { xmlDwDetail = dw_detail.Describe("DataWindow.Data.XML");}
            catch
            {xmlDwDetail = "";}
            short result = shrlonService.InitReqReturn(state.SsWsPass, ref xmlDwHead, ref xmlDwDetail, ref xmlDwMsg);
            if (result == 1)
            {
                try
                {
                    dw_head.Reset();
                    dw_head.ImportString(xmlDwHead, FileSaveAsType.Xml);
                    //DwUtil.ImportData(xmlDwHead, dw_head, null, FileSaveAsType.Xml); 
                }
                catch { }
                try
                {DwUtil.ImportData(xmlDwDetail, dw_detail, null, FileSaveAsType.Xml); }
                catch { }
                if ((xmlDwMsg != null) && (xmlDwMsg != ""))
                {
                    dw_message.Reset();
                    dw_message.ImportString(xmlDwMsg, FileSaveAsType.Xml);
                    HdMsg.Value = dw_message.GetItemString(1, "msgtext");
                }
            }
        }
        public void CancelReturn()
        {
            try
            {
                String xmlDwHead = "";
                String xmlDwMessage = "";
                String cancelID = "";
                String branchID = "";

                try
                { xmlDwHead = dw_head.Describe("DataWindow.Data.XML"); }
                catch
                { xmlDwHead = ""; }
                //try
                //{xmlDwDetail = dw_detail.Describe("DataWindow.Data.XML");}
                //catch
                //{ xmlDwDetail = "";}

                cancelID = state.SsUsername;
                branchID = state.SsCoopId;

                short result = shrlonService.CancelReqRetrun(state.SsWsPass, ref xmlDwHead, ref xmlDwMessage, cancelID, branchID);

                if (result == 1)
                {
                    DwUtil.ImportData(xmlDwHead, dw_head, tDwMain, FileSaveAsType.Xml);
                    //DwUtil.ImportData(xmlDwDetail, dw_detail, null, FileSaveAsType.Xml);
                    LtServerMessage.Text = WebUtil.CompleteMessage("กรุณากดบันทึก เพื่อยืนยันการยกเลิก");
                    if ((xmlDwMessage != null) && (xmlDwMessage != ""))
                    {
                        dw_message.Reset();
                        dw_message.ImportString(xmlDwMessage, FileSaveAsType.Xml);
                        HdMsg.Value = dw_message.GetItemString(1, "msgtext");
                        //HdReturn.Value = "10"; //  SheetLoadComplete()
                    }
                }

            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);

            }
        }
        public void OpenReqReturn()
        {
            try
            {
                String requestDocNo = "";
                String xmlDwHead = "";
                String xmlDwDetail = "";
                String xmlDwMessage = "";
                try
                { requestDocNo = dw_head.GetItemString(1, "reqreturn_docno"); }
                catch { requestDocNo = ""; }
                try
                {
                    xmlDwDetail = dw_detail.Describe("DataWindow.Data.XML");
                }
                catch
                { xmlDwDetail = ""; }
                try
                { xmlDwHead = dw_head.Describe("DataWindow.Data.XML"); }
                catch
                { xmlDwHead = ""; }

                short result = shrlonService.OpenReqReturn(state.SsWsPass, requestDocNo, ref xmlDwHead, ref xmlDwDetail, ref xmlDwMessage);
                if (result == 1)
                {
                    dw_head.Reset();
                    dw_head.ImportString(xmlDwHead, FileSaveAsType.Xml);
                    //DwUtil.ImportData(xmlDwHead, dw_head, tDwMain, FileSaveAsType.Xml);
                    DwUtil.ImportData(xmlDwDetail, dw_detail, null, FileSaveAsType.Xml);
                    tDwMain.Eng2ThaiAllRow();

                    if ((xmlDwMessage != null) && (xmlDwMessage != ""))
                    {
                        dw_message.Reset();
                        dw_message.ImportString(xmlDwMessage, FileSaveAsType.Xml);
                        HdMsg.Value = dw_message.GetItemString(1, "msgtext");
                        HdReturn.Value = "1"; //  SheetLoadComplete()
                    }
                }
            }
            catch
            {

            }



        }
        public void Refresh() { }
        private void RunProcess()
        {

            // --- Page Arguments
            try
            {
                app = Request["app"].ToString();
            }
            catch { }
            if (app == null || app == "")
            {
                app = state.SsApplication;
            }
            try
            {
                decimal groupcode =Convert.ToInt32(dw_head.GetItemString(1, "loantype_code"));
                if (groupcode < 20)
                {
                    //gid = Request["gid"].ToString();
                    gid = "LN_EMER";
                }
                else
                { 
                    gid = "LN_NORM"; 
                }
            }
            catch { }
            try
            {
                decimal groupcode2 = Convert.ToInt32(dw_head.GetItemString(1, "loantype_code"));
                if (groupcode2 < 20)
                {
                    //gid = Request["gid"].ToString();
                    rid = "LN_EMER008";
                }
                else 
                {
                    rid = "LN_NORM007"; 
                }
              
            }
            catch { }
            String operate_date = WebUtil.ConvertDateThaiToEng(dw_head, "operate_tdate", null);
            String member_no = dw_head.GetItemString(1, "member_no");
            if (member_no == null || member_no == "")
            {
                return;
            }





            //แปลง Criteria ให้อยู่ในรูปแบบมาตรฐาน.
            ReportHelper lnv_helper = new ReportHelper();
            lnv_helper.AddArgument(operate_date, ArgumentType.DateTime);
            lnv_helper.AddArgument(member_no, ArgumentType.String);
            //ชื่อไฟล์ PDF = YYYYMMDDHHMMSS_<GID>_<RID>.PDF

            String pdfFileName = DateTime.Now.ToString("yyyyMMddHHmmss", WebUtil.EN);
            pdfFileName += "_" + gid + "_" + rid + ".pdf";
            pdfFileName = pdfFileName.Trim();

            //ส่งให้ ReportService สร้าง PDF ให้ {โดยปกติจะอยู่ใน C:\GCOOP\Saving\PDF\}.
            try
            {
                Saving.WcfReport.ReportClient lws_report = wcf.Report;
                String criteriaXML = lnv_helper.PopArgumentsXML();
                this.pdf = lws_report.GetPDFURL(state.SsWsPass) + pdfFileName;
                String li_return = lws_report.Run(state.SsWsPass, app, gid, rid, criteriaXML, pdfFileName);
                if (li_return == "true")
                {
                    HdOpenIFrame.Value = "True";
                    HdcheckPdf.Value = "True";

                }
                else if (li_return != "true")
                {
                    HdcheckPdf.Value = "False";
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                return;
            }
            Session["pdf"] = pdf;
            //PopupReport();


        }
        public void PopupReport()
        {
            //เด้ง Popup ออกรายงานเป็น PDF.
            //String pop = "Gcoop.OpenPopup('http://localhost/GCOOP/WebService/Report/PDF/20110223093839_shrlonchk_SHRLONCHK001.pdf')";

            String pop = "Gcoop.OpenPopup('" + Session["pdf"].ToString() + "')";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "DsReport", pop, true);

        }

    }
}
