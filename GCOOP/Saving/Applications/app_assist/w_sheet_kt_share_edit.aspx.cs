using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using CommonLibrary;
using DBAccess;
using Sybase.DataWindow;
using System.Web.Services.Protocols;

namespace Saving.Applications.app_assist
{
    public partial class w_sheet_kt_share_edit : PageWebSheet, WebSheet
    {
        protected String jsPostMemberNo;
        protected String jsPostYear;
        private String pbl = "kt_65years.pbl";
        protected String app;
        protected String gid;
        protected String rid;
        protected String pdf;
        protected String runProcess;
        protected String popupReport;


        public void InitJsPostBack()
        {
            jsPostMemberNo = WebUtil.JsPostBack(this, "jsPostMemberNo");
            jsPostYear = WebUtil.JsPostBack(this, "jsPostYear");
            HdOpenIFrame.Value = "False";
            runProcess = WebUtil.JsPostBack(this, "runProcess");
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                DwMain.InsertRow(0);
                DwDetail.InsertRow(0);
                HdFocus.Value = "";
            }
            else
            {
                this.RestoreContextDw(DwMain);
                this.RestoreContextDw(DwDetail);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "jsPostMemberNo")
            {
                JsMemberNo();
            }
            else if (eventArg == "jsPostYear")
            {
                JsSeachYear();
            }
            else if (eventArg == "runProcess")
            {
                RunProcess();
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
                DwUtil.UpdateDateWindow(DwDetail, pbl, "asnshareperiod");
                ReCalculate();
                UpdateOldMember();

                DwDetail.Reset();
                DwDetail.InsertRow(0);

                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลสำเร็จ");

            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        public void WebSheetLoadEnd()
        {
            DwMain.SaveDataCache();
            DwDetail.SaveDataCache();
        }

        public void JsMemberNo()
        {
            HdFocus.Value = "1";
        }

        public void JsSeachYear()
        {
            string member_no = "", shr_year = "";
            try
            {
                member_no = DwMain.GetItemString(1, "member_no");
            }
            catch { }
            try
            {
                shr_year = DwMain.GetItemString(1, "shr_year");
            }
            catch { }

            DwUtil.RetrieveDataWindow(DwDetail, pbl, null, member_no, shr_year);
            HdFocus.Value = "3";

        }//

        public void ReCalculate()
        {
            DateTime calculate_date = state.SsWorkDate;
            String member_no = DwMain.GetItemString(1, "member_no");
            String shr_year = DwMain.GetItemString(1, "shr_year");
            String SqlGetCaldate = "select calculate_date from asnreqmaster where member_no = '" + member_no + "' and assisttype_code ='90'";
            Sdt dtCal = WebUtil.QuerySdt(SqlGetCaldate);
            if (dtCal.Next())
            {
                calculate_date = dtCal.GetDate("calculate_date");
            }

            String SqlDeleteAsnsharecal = "delete from asnsharecal where member_no = '" + member_no + "' and shr_year = '" + shr_year + "'";
            WebUtil.Query(SqlDeleteAsnsharecal);
            String InsertToAsnsharecal = @"
                    insert into asnsharecal
                    (member_no , shr_year , seq_no , shrqt01_value , shrqt02_value , shrqt03_value , shr_value , shr_percent , shrcal_value )
                    select member_no , shr_year , seq_no , quarter01 , quarter02 , quarter03 , ( quarter01 + quarter02 + quarter03 ) , percent , ( quarter01 + quarter02 + quarter03 ) * percent
                    from
                    (
	                    select member_No , shr_year , 
	                    rank() over ( partition by member_no , shr_year order by percent ) as seq_no ,
	                    sum( case when shr_mth = '01' then shr_value when shr_mth = '02' then shr_value when shr_mth = '03' then shr_value when shr_mth = '04' then shr_value  else 0 end ) as quarter01 ,
	                    sum( case when shr_mth = '05' then shr_value when shr_mth = '06' then shr_value when shr_mth = '07' then shr_value when shr_mth = '08' then shr_value  else 0 end ) as quarter02 ,
	                    sum( case when shr_mth = '09' then shr_value when shr_mth = '10' then shr_value when shr_mth = '11' then shr_value when shr_mth = '12' then shr_value  else 0 end ) as quarter03 ,
	                    percent
	                    from(
		                        select member_no , shr_year , shr_mth , shr_value ,  
		                        ( select percent  
		                            from asnucfpay65 where 
		                            trim(asnshareperiod.shr_year || asnshareperiod.shr_mth ) > trim( to_char ( to_number( to_char( to_date('" + calculate_date.ToString("dd/MM/yyyy") + @"' , 'dd/mm/yyyy') , 'yyyy' ) ) + 542 - maxmemb_year ) || to_char( to_date('" + calculate_date.ToString("dd/MM/yyyy") + @"' , 'dd/mm/yyyy') , 'mm' ) ) and
		                            trim(asnshareperiod.shr_year || asnshareperiod.shr_mth ) <= trim( to_char ( to_number( to_char( to_date('" + calculate_date.ToString("dd/MM/yyyy") + @"' , 'dd/mm/yyyy') , 'yyyy' ) ) + 543 - minmemb_year ) || to_char( to_date('" + calculate_date.ToString("dd/MM/yyyy") + @"' , 'dd/mm/yyyy') , 'mm' ) ) 
		                        ) as percent
		                        from asnshareperiod 
                                where member_no = '" + member_no + "' and shr_year ='" + shr_year + @"'
	                        )
	                group by member_no , shr_year , percent 
                     ) where (quarter01 + quarter02 + quarter03) > 0 and member_no = '" + member_no + "' and shr_year ='" + shr_year + "'";
            WebUtil.Query(InsertToAsnsharecal);
        }

        public void UpdateOldMember()
        {
            String member_no = DwMain.GetItemString(1, "member_no");
            Double assist_amt = 0, perpay = 0, balance = 0, sumpays = 0;
            Decimal pay_percent = 0, maxpay_peryear = 0;

            String SqlMembNo = "select member_no , sumpays from asnreqmaster where assisttype_code = '90' and member_no = '" + member_no + "'";
            Sdt dtMembNo = WebUtil.QuerySdt(SqlMembNo);
            String SqlSelectPerpay = "select percent , maxpay_peryear from asnucfperpay where assist_code = 'SF001'";
            Sdt dtPerpay = WebUtil.QuerySdt(SqlSelectPerpay);
            if (dtPerpay.Next())
            {
                pay_percent = dtPerpay.GetDecimal("percent");
                maxpay_peryear = dtPerpay.GetDecimal("maxpay_peryear");
            }

            if (dtMembNo.Next())
            {

                sumpays = dtMembNo.GetDouble("sumpays");
                assist_amt = shareQuery(member_no);
                perpay = Math.Floor(assist_amt * Convert.ToDouble(pay_percent));
                if (perpay % 10 != 0)
                {
                    perpay = perpay + (10 - (perpay % 10));//ปัดเศษ
                }
                if (perpay > Convert.ToDouble(maxpay_peryear)) perpay = Convert.ToDouble(maxpay_peryear);
                balance = assist_amt - sumpays;

                String SqlUpdateAsnreqmaster = @"update asnreqmaster 
                                             set assist_amt = " + assist_amt + " , perpay = " + perpay + " , balance = "+balance+@" 
                                             where member_no = '" + member_no + "' and assisttype_code = '90' ";
                WebUtil.Query(SqlUpdateAsnreqmaster);
            }
        }

        public Double shareQuery(string member_no)
        {
            decimal max_pay = 0;
            decimal sumcal_amt = 0;
            string sqlcal65 = "";
            Sdt selectsum;
            try
            {
                string sqlGetMaxPay = "select max(max_pay) from asnucfpay65";
                selectsum = WebUtil.QuerySdt(sqlGetMaxPay);
                if (selectsum.Next())
                {
                    max_pay = selectsum.GetDecimal("max(max_pay)");
                    selectsum.Reset();
                }

                sqlcal65 = "select sum(shrcal_value) from asnsharecal where member_no = '" + member_no + "'";
                selectsum = WebUtil.QuerySdt(sqlcal65);
                if (selectsum.Next())
                {
                    sumcal_amt = selectsum.GetDecimal("sum(shrcal_value)");
                    if (sumcal_amt > max_pay)
                        sumcal_amt = max_pay;
                }
            }
            catch
            {
                sumcal_amt = 0;
            }
            sumcal_amt = Math.Floor(sumcal_amt);
            return Convert.ToDouble(sumcal_amt);

        }//end shareQuery


        #region Report Process

        private void RunProcess()
        {

            DateTime calculate_date = state.SsWorkDate;
            String member_no = DwMain.GetItemString(1, "member_no");
            String print_tdate = state.SsWorkDate.ToString("MM/dd/yyyy");
            app = state.SsApplication;


            gid = "Reserve";
            rid = "RE_ASS002";

            String SqlGetCaldate = "select calculate_date from asnreqmaster where member_no = '" + member_no + "' and assisttype_code ='90'";
            Sdt dtCal = WebUtil.QuerySdt(SqlGetCaldate);
            if (dtCal.Next())
            {
                calculate_date = dtCal.GetDate("calculate_date");
            }

            //แปลง Criteria ให้อยู่ในรูปแบบมาตรฐาน.
            ReportHelper lnv_helper = new ReportHelper();
            lnv_helper.AddArgument(member_no, ArgumentType.String);
            lnv_helper.AddArgument(calculate_date.ToString("MM/dd/yyyy"), ArgumentType.DateTime);
            lnv_helper.AddArgument(print_tdate, ArgumentType.DateTime);


            //ชื่อไฟล์ PDF = YYYYMMDDHHMMSS_<GID>_<RID>.PDF
            String pdfFileName = DateTime.Now.ToString("yyyyMMddHHmmss", WebUtil.EN);
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
        public void PopupReport()
        {
            //เด้ง Popup ออกรายงานเป็น PDF.
            String pop = "Gcoop.OpenPopup('" + pdf + "')";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "DsReport", pop, true);
        }
        #endregion
    }
}