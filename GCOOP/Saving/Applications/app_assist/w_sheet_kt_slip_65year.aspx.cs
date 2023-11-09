using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommonLibrary;
using DBAccess;
namespace Saving.Applications.app_assist
{
    public partial class w_sheet_kt_slip_65year : PageWebSheet, WebSheet
    {
        public String app = "";
        public String gid = "";
        public String rid = "";
        protected String pdf = "";
        protected String postslipClick;
        protected String postRetreiveDwMem;
        protected String postslipDetail;
        protected Sta ta;
        protected Sdt dt;

        public void InitJsPostBack()
        {
            postslipClick      = WebUtil.JsPostBack(this ,"postslipClick");
            postRetreiveDwMem  = WebUtil.JsPostBack(this, "postRetreiveDwMem");
            postslipDetail     = WebUtil.JsPostBack(this, "postslipDetail");     
        }
        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                DwMtran.Reset();
                DwMemb.Reset();
                DwMain.Reset();
                DwMtran.InsertRow(0);
                DwMemb.InsertRow(0);
                DwMain.InsertRow(0);
            }
            else
            {
                this.RestoreContextDw(DwMtran);
                this.RestoreContextDw(DwMemb);
                this.RestoreContextDw(DwMain);

            }
           
        }
        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postRetreiveDwMem")
            {
                RetreiveDwMem();
            }
            else  if (eventArg == "postslipClick")
            {
               slipClick();
            }
            else if (eventArg == "postslipDetail") 
            {
                DetailClick();
            }
        }
        private void RetreiveDwMem()
        {

             string  member_no = HdMemberNo.Value;
             DateTime calculate_date = state.SsWorkDate;
             DwUtil.RetrieveDataWindow(DwMtran, "Kt_65years.pbl", null, member_no);
             string sql = "select calculate_date from asnreqmaster where member_no='"+member_no+"' and assisttype_code='90'";
             dt = WebUtil.QuerySdt(sql);
             if (dt.Next())
             {
                 calculate_date = dt.GetDate("calculate_date");
             }
             else
             {
                 sql = "select calculate_date from asnreqmaster where member_no='" + member_no + "' and assisttype_code='80'";
                 dt = WebUtil.QuerySdt(sql);
                 if (dt.Next())
                 {
                     calculate_date = dt.GetDate("calculate_date");
                 }
             }
             string a = calculate_date.ToString("dd/MM/")+(Convert.ToInt32(calculate_date.ToString("yyyy"))+543).ToString();
             DwMtran.SetItemString(1,"slip_date",a);
             DwUtil.RetrieveDataWindow(DwMemb, "Kt_65years.pbl", null, member_no, calculate_date);
             DwUtil.RetrieveDataWindow(DwMain, "Kt_65years.pbl", null, member_no, calculate_date);
             DwMemb.SetItemDateTime(1, "calculate_date", calculate_date);
        }
        public void SaveWebSheet()
        {

        }

        public void WebSheetLoadEnd()
        {
            DwMtran.SaveDataCache();
            DwMemb.SaveDataCache();
            DwMain.SaveDataCache();

            dt.Clear();
            ta.Close();
            
        }
        private void RunProcess()
        {
            String print_id = "PS_ASS04";
            String as_member_no = HdMemberNo.Value;
            DateTime calculate_date = state.SsWorkDate;
            string sql = "select calculate_date from asnreqmaster where member_no='" + as_member_no + "' and assisttype_code='90'";
            dt = WebUtil.QuerySdt(sql);
            if (dt.Next())
            {
                calculate_date = dt.GetDate("calculate_date");
            }
            else
            {
                sql = "select calculate_date from asnreqmaster where member_no='" + as_member_no + "' and assisttype_code='80'";
                dt = WebUtil.QuerySdt(sql);
                if (dt.Next())
                {
                    calculate_date = dt.GetDate("calculate_date");
                }
            }
            string tdate = calculate_date.ToString() ;
            //DateTime date = new DateTime(DateTime.Now.Year - 1, 9, 30);
            //string tdate = date.ToString("dd/MM/yyyy");

            app = "app_assist";
            try
            {
                gid = "Pension";
            }
            catch { }
            try
            {
                rid = print_id;
            }
            catch { }

            ReportHelper lnv_helper = new ReportHelper();
            lnv_helper.AddArgument(as_member_no, ArgumentType.String);
            lnv_helper.AddArgument(tdate, ArgumentType.DateTime);

            //****************************************************************
            //ชื่อไฟล์ PDF = YYYYMMDDHHMMSS_<GID>_<RID>.PDF
            String pdfFileName = state.SsWorkDate.ToString("yyyyMMddHHmmss", WebUtil.EN);
            pdfFileName += "_" + gid + "_" + rid + ".pdf";
            pdfFileName = pdfFileName.Trim();
            //ส่งให้ ReportService สร้าง PDF ให้ {โดยปกติจะอยู่ใน C:\GCOOP\Saving\PDF\}.
            try
            {
                CommonLibrary.WsReport.Report lws_report = WsCalling.Report;
                String criteriaXML = lnv_helper.PopArgumentsXML();
                this.pdf = lws_report.GetPDFURL(state.SsWsPass) + pdfFileName;
                //String li_return = lws_report.RunWithID(state.SsWsPass, app, gid, rid, state.SsUsername, criteriaXML, pdfFileName);
                String li_return = lws_report.Run(state.SsWsPass, app, gid, rid, criteriaXML, pdfFileName);
                if (li_return == "true")
                {
                    HdOpenIFrame.Value = "True";
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                return;
            }
        }
        private void slipClick()
        {
            string print_period ="";
            string print_flag = "";
            
            try
            {
                print_flag = DwMtran.GetItemString(1, "print_flag");
                string member_no = DwMemb.GetItemString(1, "member_no");
                string sqlSelectPrint = "select print_period from asnreqmaster where member_no = '" + member_no + "'";
                Sdt dt = WebUtil.QuerySdt(sqlSelectPrint);
                if (dt.Next())
                {
                    print_period = dt.GetString("print_period");
                }
            }
            catch { }
            if (print_period != DwMtran.GetItemString(1, "slip_date"))
            {
                string sqlUpdate = "update asnreqmaster set print_period = '" + DwMtran.GetItemString(1, "slip_date") + "'";
                WebUtil.Query(sqlUpdate);
                RunProcess();
                String pop = "Gcoop.OpenPopup('" + pdf + "')";
                ClientScript.RegisterClientScriptBlock(this.GetType(), "DsReport", pop, true);
            }
            else
            {
                if (print_flag == "1")
                {
                    string sqlUpdate = "update asnreqmaster set print_period = '" + DwMtran.GetItemString(1, "slip_date") + "'";
                    WebUtil.Query(sqlUpdate);
                    RunProcess();
                    String pop = "Gcoop.OpenPopup('" + pdf + "')";
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "DsReport", pop, true);
                }
                else if (print_flag == "0")
                    LtServerMessage.Text = WebUtil.ErrorMessage("หมายเลขสมาชิกนี้ถูกปริ้นใบเสร็จไปแล้ว");
                
                //Label lbl = new Label();
                //lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('หมายเลขสมาชิกนี้ถูกปริ้นใบเสร็จไปแล้ว')</script>";
                //Page.Controls.Add(lbl);

            }


        }
        private void RunProcessDetail()
        {
            //อ่านค่าจากหน้าจอใส่ตัวแปรรอไว้ก่อน.
            String print_id = "RE_ASS002";
            String as_memberno = HdMemberNo.Value;
            Decimal sumpay = 0 ;
            string sql = "select sumpays from asnreqmaster where member_no = '" + as_memberno + "' and assisttype_code='90' order by member_no asc";
            Sdt rs = WebUtil.QuerySdt(sql);
            if (rs.Next())
            {
                sumpay = rs.GetDecimal("sumpays");

            }
            DateTime calculate_date = state.SsWorkDate;
             string sqlsum = "select calculate_date from asnreqmaster where member_no='" + as_memberno + "' and assisttype_code='90'";
            dt = WebUtil.QuerySdt(sqlsum);
            if (dt.Next())
            {
                calculate_date = dt.GetDate("calculate_date");
            }
            else
            {
                sql = "select calculate_date from asnreqmaster where member_no='" + as_memberno + "' and assisttype_code='80'";
                dt = WebUtil.QuerySdt(sql);
                if (dt.Next())
                {
                    calculate_date = dt.GetDate("calculate_date");
                }
            }
            String asncalto_tdate = calculate_date.ToString();
            String print_tdate = calculate_date.ToString();
            app = "app_assist";
            try
            {
                gid = "Reserve";
            }
            catch { }
            try
            {
                rid = print_id;
            }
            catch { }

            //แปลง Criteria ให้อยู่ในรูปแบบมาตรฐาน.
            //String as_memberno = dw_criteria.GetItemString(1, "as_memberno");
            ReportHelper lnv_helper = new ReportHelper();
            lnv_helper.AddArgument(as_memberno, ArgumentType.String);
            lnv_helper.AddArgument(asncalto_tdate, ArgumentType.DateTime);
            lnv_helper.AddArgument(print_tdate, ArgumentType.DateTime);
            lnv_helper.AddArgument(sumpay.ToString(), ArgumentType.Number);
            //ชื่อไฟล์ PDF = YYYYMMDDHHMMSS_<GID>_<RID>.PDF
            String pdfFileName = state.SsWorkDate.ToString("yyyyMMddHHmmss", WebUtil.EN);
            pdfFileName += "_" + gid + "_" + rid + ".pdf";
            pdfFileName = pdfFileName.Trim();

            //ส่งให้ ReportService สร้าง PDF ให้ {โดยปกติจะอยู่ใน C:\GCOOP\Saving\PDF\}.
            try
            {
                CommonLibrary.WsReport.Report lws_report = WsCalling.Report;
                String criteriaXML = lnv_helper.PopArgumentsXML();
                this.pdf = lws_report.GetPDFURL(state.SsWsPass) + pdfFileName;
                String li_return = lws_report.Run(state.SsWsPass, app, gid, rid, criteriaXML, pdfFileName);
                if (li_return == "true")
                {
                    HdOpenIFrame.Value = "True";
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                return;
            }
        }
        private void DetailClick()
        {
            //Mai
            RunProcessDetail();
            // Thread.Sleep(5000);
            //Thread.Sleep(4500);

            //เด้ง Popup ออกรายงานเป็น PDF.
            String pop = "Gcoop.OpenPopup('" + pdf + "')";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "DsReport", pop, true);
        }


    }
}