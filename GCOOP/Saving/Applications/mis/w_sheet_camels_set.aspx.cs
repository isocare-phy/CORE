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
using DataLibrary;
using System.Globalization;
using CoreSavingLibrary.WcfNMis;

namespace Saving.Applications.mis
{
    public partial class w_sheet_camels_set : System.Web.UI.Page
    {
        //private DwTrans SQLCA;
        //private WebState state;
        //private MisClient MisService;

        ////postback
        //protected String postGroupOut;
        //protected String postGroupBy;
        //protected String postGroupId;
        //protected String postRatioId;
        //protected String postMoneySheet;
        //protected String saveData;

        ////*********************report****************************
        //protected String app;
        //protected String gid;
        //protected String rid;
        //protected String pdf;
        //protected String runProcess;
        //protected String popupReport;
        //protected String downloadPDF;

        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    //state = new WebState(Session, Request);
        //    //SQLCA = new DwTrans();
        //    //SQLCA.Connect();

        //    //if (!state.IsReadable)
        //    //{
        //    //    LtServerMessage.Text = WebUtil.PermissionDeny(PermissType.ReadDeny);
        //    //    return;
        //    //}

        //    //try
        //    //{
        //    //    MisService = wcf.NMis;
        //    //}
        //    //catch
        //    //{
        //    //    LtServerMessage.Text = WebUtil.ErrorMessage("ติดต่อ Web Service ไม่ได้");
        //    //    return;
        //    //}

        //    //postGroupOut = WebUtil.JsPostBack(this, "postGroupOut");
        //    //postGroupBy = WebUtil.JsPostBack(this, "postGroupBy");
        //    //postGroupId = WebUtil.JsPostBack(this, "postGroupId");
        //    //postRatioId = WebUtil.JsPostBack(this, "postRatioId");
        //    //postMoneySheet = WebUtil.JsPostBack(this, "postMoneySheet");
        //    //saveData = WebUtil.JsPostBack(this, "saveData");
        //    //popupReport = WebUtil.JsPostBack(this, "popupReport");
        //    //LtChangeTap.Text = "";

        //    ////dw_tap1.SetTransaction(SQLCA);
        //    ////dw_tap21.SetTransaction(SQLCA);
        //    ////dw_tap22.SetTransaction(SQLCA);
        //    //dw_tap23.SetTransaction(SQLCA);
        //    ////dw_tap3.SetTransaction(SQLCA);
        //    ////dw_tap31.SetTransaction(SQLCA);

        //    //if (!IsPostBack)
        //    //{
        //    //    //dw_tap1.Retrieve();         
        //    //    String GetVar = MisService.RatioGetVariables(state.SsWsPass);
        //    //    dw_tap1.Reset();
        //    //    dw_tap1.ImportString(GetVar, Sybase.DataWindow.FileSaveAsType.Xml);
        //    //    dw_tap1.Sort();

        //    //    //dw_tap21.Retrieve();
        //    //    String GetRatio = MisService.RatioGetRatios(state.SsWsPass);
        //    //    dw_tap21.Reset();
        //    //    dw_tap21.ImportString(GetRatio, Sybase.DataWindow.FileSaveAsType.Xml);
        //    //    dw_tap21.Sort();

        //    //    //dw_tap22.Retrieve(1);
        //    //    String GetOperand = MisService.RatioGetOperands(state.SsWsPass, 1);
        //    //    dw_tap22.Reset();
        //    //    dw_tap22.ImportString(GetOperand, Sybase.DataWindow.FileSaveAsType.Xml);
        //    //    dw_tap22.Sort();

        //    //    //dw_tap3.Retrieve();
        //    //    String GetGroups = MisService.RatioGetGroups(state.SsWsPass);
        //    //    dw_tap3.Reset();
        //    //    dw_tap3.ImportString(GetGroups, Sybase.DataWindow.FileSaveAsType.Xml);
        //    //    dw_tap3.Sort();

        //    //    //dw_tap31.Retrieve(1);
        //    //    String GetRatioGroups = MisService.RatioGetRatiosGroups(state.SsWsPass, 1);
        //    //    dw_tap31.Reset();
        //    //    dw_tap31.ImportString(GetRatioGroups, Sybase.DataWindow.FileSaveAsType.Xml);
        //    //    dw_tap31.Sort();
        //    //}
        //    //else
        //    //{
        //    //    dw_tap1.RestoreContext();
        //    //    dw_tap21.RestoreContext();
        //    //    dw_tap22.RestoreContext();
        //    //    dw_tap3.RestoreContext();
        //    //    dw_tap31.RestoreContext();
        //    //}

        //    //dw_tap23.Retrieve(1);

        //    ////int req = 0;

        //    ////try
        //    ////{
        //    ////    req = Convert.ToInt32(Request["RatioId"]);
        //    ////}
        //    ////catch
        //    ////{
        //    ////    req = 1;
        //    ////}
        //    ////if (!IsPostBack)
        //    ////{
        //    ////    try
        //    ////    {
        //    ////        if (req != 0)
        //    ////        {
        //    ////            //dw_tap22.Retrieve(req);
        //    ////            String GetOperand = MisService.RatioGetOperands(state.SsWsPass, req);
        //    ////            dw_tap22.Reset();
        //    ////            dw_tap22.ImportString(GetOperand, Sybase.DataWindow.FileSaveAsType.Xml);
        //    ////        }
        //    ////        else
        //    ////        {
        //    ////dw_tap22.Retrieve(1);
        //    ////String GetOperand = MisService.RatioGetOperands(state.SsWsPass, 1);
        //    ////dw_tap22.Reset();
        //    ////dw_tap22.ImportString(GetOperand, Sybase.DataWindow.FileSaveAsType.Xml);                       
        //    ////        }
        //    ////    }
        //    ////    catch { }

        //    ////}

        //    ////if (req != 0)
        //    ////{
        //    ////    dw_tap23.Retrieve(req);
        //    ////}
        //    ////else
        //    ////{
        //    ////dw_tap23.Retrieve(1);
        //    ////}

        //    ////if (!IsPostBack)
        //    ////{
        //    //////dw_tap3.Retrieve();
        //    ////String GetGroups = MisService.RatioGetGroups(state.SsWsPass);
        //    ////dw_tap3.Reset();
        //    ////dw_tap3.ImportString(GetGroups, Sybase.DataWindow.FileSaveAsType.Xml);
        //    ////dw_tap3.Sort();
        //    ////}

        //    ////int reqGroupId;
        //    ////try
        //    ////{
        //    ////    reqGroupId = Convert.ToInt32(Request["GroupId"]);
        //    ////}
        //    ////catch
        //    ////{
        //    ////    reqGroupId = 1;
        //    ////}

        //    ////if (!IsPostBack)
        //    ////{
        //    ////    try
        //    ////    {
        //    ////        if (reqGroupId != 0)
        //    ////        {
        //    ////            //dw_tap31.Retrieve(reqGroupId);
        //    ////            String GetGroups = MisService.RatioGetRatiosGroups(state.SsWsPass, reqGroupId);
        //    ////            dw_tap31.Reset();
        //    ////            dw_tap31.ImportString(GetGroups, Sybase.DataWindow.FileSaveAsType.Xml);
        //    ////        }
        //    ////        else
        //    ////        {
        //    ////dw_tap31.Retrieve(1);
        //    ////String GetRatioGroups = MisService.RatioGetRatiosGroups(state.SsWsPass, 1);
        //    ////dw_tap31.Reset();
        //    ////dw_tap31.ImportString(GetRatioGroups, Sybase.DataWindow.FileSaveAsType.Xml);
        //    ////        }
        //    ////    }
        //    ////    catch { }
        //    ////}

        //    //// EVENT Postback
        //    //if (IsPostBack)
        //    //{
        //    //    try
        //    //    {
        //    //        String eventArg = Request["__EVENTARGUMENT"];
        //    //        if (eventArg == "saveData")
        //    //        {
        //    //            //บันทึกตารางตัวแปร
        //    //            String ls_xml_var = dw_tap1.Describe("DataWindow.Data.XML");
        //    //            String reVar = MisService.RatioSaveVariables(state.SsWsPass, ls_xml_var);

        //    //            //บันทึกตารางอัตราส่วน
        //    //            String ls_xml_ratios = dw_tap21.Describe("DataWindow.Data.XML");
        //    //            String reRatios = MisService.RatioSaveRatios(state.SsWsPass, ls_xml_ratios);

        //    //            //บันทึกตารางกำหนดตัวแปร
        //    //            String ls_xml_operand = dw_tap22.Describe("DataWindow.Data.XML");
        //    //            String reOperands = MisService.RatioSaveOperands(state.SsWsPass, ls_xml_operand);

        //    //            //บันทึกตารางกลุ่ม
        //    //            String ls_xml_groups = dw_tap3.Describe("DataWindow.Data.XML");
        //    //            String reGroups = MisService.RatioSaveGroups(state.SsWsPass, ls_xml_groups);
        //    //        }
        //    //        else if (eventArg == "postRatioId")
        //    //        {
        //    //            JsPostRatioId();
        //    //        }
        //    //        else if (eventArg == "postGroupId")
        //    //        {
        //    //            JsPostGroupId();
        //    //        }
        //    //        else if (eventArg == "postGroupBy")
        //    //        {
        //    //            JsPostGroupBy();
        //    //        }
        //    //        else if (eventArg == "postGroupOut")
        //    //        {
        //    //            JsPostGroupOut();
        //    //        }
        //    //        else if (eventArg == "InsertRow")
        //    //        {
        //    //            if (!IsPostBack)
        //    //            {
        //    //                dw_tap22.RestoreContext();
        //    //            }

        //    //            //dw_tap31.RestoreContext();
        //    //        }
        //    //        else if (eventArg == "popupReport")
        //    //        {
        //    //            RunProcess();
        //    //            PopupReport();
        //    //        }
        //    //    }
        //    //    catch (Exception ex)
        //    //    {
        //    //        ex.ToString();
        //    //    }
        //    //}

        //}

        //protected void Page_LoadComplete()
        //{
        //    try
        //    {
        //        SQLCA.Disconnect();
        //    }
        //    catch { }
        //}


        //private void JsPostGroupOut()
        //{
        //    int reqGroupBy = 1;
        //    int reqGroupRatioId = 1;

        //    try
        //    {
        //        reqGroupBy = Convert.ToInt32(HdGroupBy.Value);
        //        reqGroupRatioId = Convert.ToInt32(HdGroupRatioId.Value);
        //    }
        //    catch { }

        //    try
        //    {
        //        String re = MisService.RatioChangeGroups(state.SsWsPass, reqGroupRatioId, reqGroupBy);
        //    }
        //    catch { }

        //    LtChangeTap.Text = "<script>showTabPage('" + HdCurrentTap.Value + "')</script>";
        //}

        //private void JsPostGroupBy()
        //{
        //    int reqGroupBy = 1;
        //    int reqGroupRatioId = 1;

        //    try
        //    {
        //        reqGroupBy = Convert.ToInt32(HdGroupBy.Value);
        //        reqGroupRatioId = Convert.ToInt32(HdGroupRatioId.Value);
        //    }
        //    catch { }

        //    try
        //    {
        //        if (reqGroupBy != 0)
        //        {
        //            String re = MisService.RatioChangeGroups(state.SsWsPass, reqGroupRatioId, reqGroupBy);
        //        }
        //        else
        //        {

        //        }
        //    }
        //    catch { }

        //    LtChangeTap.Text = "<script>showTabPage('" + HdCurrentTap.Value + "')</script>";
        //}

        //private void JsPostGroupId()
        //{
        //    int reqGroupId = 1;

        //    try
        //    {
        //        reqGroupId = Convert.ToInt32(HdGroupId.Value);
        //    }
        //    catch { }

        //    try
        //    {
        //        if (reqGroupId != 0)
        //        {
        //            //dw_tap31.Retrieve(reqGroupId);
        //            String GetGroups = MisService.RatioGetRatiosGroups(state.SsWsPass, reqGroupId);
        //            dw_tap31.Reset();
        //            dw_tap31.ImportString(GetGroups, Sybase.DataWindow.FileSaveAsType.Xml);
        //            dw_tap31.Sort();
        //        }
        //        else
        //        {
        //            //dw_tap31.Retrieve(1);
        //            String GetGroups = MisService.RatioGetRatiosGroups(state.SsWsPass, 1);
        //            dw_tap31.Reset();
        //            dw_tap31.ImportString(GetGroups, Sybase.DataWindow.FileSaveAsType.Xml);
        //        }
        //    }
        //    catch { }

        //    LtChangeTap.Text = "<script>showTabPage('" + HdCurrentTap.Value + "')</script>";
        //}

        //private void JsPostRatioId()
        //{
        //    int reqRatioId = 1;

        //    try
        //    {
        //        reqRatioId = Convert.ToInt32(HdRatioId.Value);
        //    }
        //    catch { }

        //    try
        //    {
        //        if (reqRatioId != 0)
        //        {
        //            //dw_tap22.Retrieve(req);
        //            String GetOperand = MisService.RatioGetOperands(state.SsWsPass, reqRatioId);
        //            dw_tap22.Reset();
        //            dw_tap22.ImportString(GetOperand, Sybase.DataWindow.FileSaveAsType.Xml);
        //        }
        //        else
        //        {
        //            //dw_tap22.Retrieve(1);
        //            String GetOperand = MisService.RatioGetOperands(state.SsWsPass, 1);
        //            dw_tap22.Reset();
        //            dw_tap22.ImportString(GetOperand, Sybase.DataWindow.FileSaveAsType.Xml);
        //        }
        //    }
        //    catch { }

        //    if (reqRatioId != 0)
        //    {
        //        dw_tap23.Retrieve(reqRatioId);
        //    }
        //    else
        //    {
        //        dw_tap23.Retrieve(1);
        //    }

        //    LtChangeTap.Text = "<script>showTabPage('" + HdCurrentTap.Value + "')</script>";
        //}


        //protected void dw_tap21_AfterPerformAction(object sender, Sybase.DataWindow.Web.AfterPerformActionEventArgs e)
        //{
        //    if (e.Action == Sybase.DataWindow.Web.PostBackAction.InsertRow)
        //    {
        //        LtChangeTap.Text = "<script>showTabPage('" + HdCurrentTap.Value + "')</script>";
        //    }

        //}

        //protected void dw_tap22_AfterPerformAction(object sender, Sybase.DataWindow.Web.AfterPerformActionEventArgs e)
        //{
        //    if (e.Action == Sybase.DataWindow.Web.PostBackAction.InsertRow)
        //    {
        //        LtChangeTap.Text = "<script>showTabPage('" + HdCurrentTap.Value + "')</script>";
        //    }
        //}

        //protected void dw_tap3_AfterPerformAction(object sender, Sybase.DataWindow.Web.AfterPerformActionEventArgs e)
        //{
        //    if (e.Action == Sybase.DataWindow.Web.PostBackAction.InsertRow)
        //    {
        //        LtChangeTap.Text = "<script>showTabPage('" + HdCurrentTap.Value + "')</script>";
        //    }
        //}

        //protected void dw_tap31_AfterPerformAction(object sender, Sybase.DataWindow.Web.AfterPerformActionEventArgs e)
        //{
        //    if (e.Action == Sybase.DataWindow.Web.PostBackAction.InsertRow)
        //    {
        //        LtChangeTap.Text = "<script>showTabPage('" + HdCurrentTap.Value + "')</script>";
        //    }
        //}

        //private void RunProcess()
        //{

        //    // --- Page Arguments
        //    try
        //    {
        //        app = Request["app"].ToString();
        //    }
        //    catch { }
        //    if (app == null || app == "")
        //    {
        //        app = state.SsApplication;
        //    }
        //    try
        //    {
        //        //gid = Request["gid"].ToString();
        //        gid = "Mis_spec";
        //    }
        //    catch { }
        //    try
        //    {
        //        //rid = Request["rid"].ToString();
        //        rid = "MISSPEC006";
        //    }
        //    catch { }

        //    //String doc_no = dw_main.GetItemString(1, "loanrequest_docno");
        //    //if (doc_no == null || doc_no == "")
        //    //{
        //    //    return;
        //    //}

        //    //String ls_xmlmain = dw_main.Describe("DataWindow.Data.XML");
        //    //String ls_format = "CAT";
        //    //short li_membtime = 0; Decimal ldc_right25 = 0; Decimal ldc_right33 = 0; Decimal ldc_right35 = 0;
        //    //int re = shrlonService.Ofprintcallpermiss(state.SsWsPass, ls_xmlmain, ls_format, ref li_membtime, ref ldc_right25, ref ldc_right33, ref ldc_right35);
        //    //Decimal li_membtime_ = li_membtime;
        //    //Decimal ldc_right25_ = ldc_right25;
        //    //Decimal ldc_right33_ = ldc_right33;
        //    //Decimal ldc_right35_ = ldc_right35;

        //    Int16 startMonth = 0;
        //    String startYear = "";
        //    Int16 endMouth = 0;
        //    String endYear = "";

        //    //try
        //    //{ startMonth = Convert.ToInt16(dw_cri.GetItemDecimal(1, "start_month")); }
        //    //catch { startMonth = 0; }
        //    //try { startYear = Convert.ToString(dw_cri.GetItemString(1, "start_year")); }
        //    //catch { startYear = ""; }
        //    //try { endMouth = Convert.ToInt16(dw_cri.GetItemDecimal(1, "end_month")); }
        //    //catch { endMouth = 0; }
        //    //try { endYear = Convert.ToString(dw_cri.GetItemString(1, "end_year")); }
        //    //catch { endYear = ""; }


        //    //แปลง Criteria ให้อยู่ในรูปแบบมาตรฐาน.
        //    ReportHelper lnv_helper = new ReportHelper();
        //    //lnv_helper.AddArgument("55555", ArgumentType.String);
        //    //lnv_helper.AddArgument(startMonth.ToString(), ArgumentType.Number);
        //    //lnv_helper.AddArgument(startYear.ToString(), ArgumentType.Number);
        //    //lnv_helper.AddArgument(endMouth.ToString(), ArgumentType.Number);
        //    //lnv_helper.AddArgument(endYear.ToString(), ArgumentType.Number);

        //    //ชื่อไฟล์ PDF = YYYYMMDDHHMMSS_<GID>_<RID>.PDF

        //    String pdfFileName = DateTime.Now.ToString("yyyyMMddHHmmss", WebUtil.EN);
        //    pdfFileName += "_" + gid + "_" + rid + ".pdf";
        //    pdfFileName = pdfFileName.Trim();

        //    //ส่งให้ ReportService สร้าง PDF ให้ {โดยปกติจะอยู่ใน C:\GCOOP\Saving\PDF\}.
        //    try
        //    {
        //        //CoreSavingLibrary.WcfReport.ReportClient lws_report = wcf.Report;
        //        //String criteriaXML = lnv_helper.PopArgumentsXML();
        //        //this.pdf = lws_report.GetPDFURL(state.SsWsPass) + pdfFileName;
        //        String li_return = lws_report.Run(state.SsWsPass, app, gid, rid, criteriaXML, pdfFileName);
        //        if (li_return == "true")
        //        {
        //            HdOpenIFrame.Value = "True";
        //            HdcheckPdf.Value = "True";

        //        }
        //        else if (li_return != "true")
        //        {
        //            HdcheckPdf.Value = "False";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LtServerMessage.Text = WebUtil.ErrorMessage(ex);
        //        return;
        //    }
        //    Session["pdf"] = pdf;
        //    //PopupReport();


        //}

        //public void PopupReport()
        //{
        //    //เด้ง Popup ออกรายงานเป็น PDF.
        //    //String pop = "Gcoop.OpenPopup('http://localhost/GCOOP/WebService/Report/PDF/20110223093839_shrlonchk_SHRLONCHK001.pdf')";

        //    String pop = "Gcoop.OpenPopup('" + Session["pdf"].ToString() + "')";
        //    ClientScript.RegisterClientScriptBlock(this.GetType(), "DsReport", pop, true);

        //}
    }
}
