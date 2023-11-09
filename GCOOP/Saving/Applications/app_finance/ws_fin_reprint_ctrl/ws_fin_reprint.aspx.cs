using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using DataLibrary;
namespace Saving.Applications.app_finance.ws_fin_reprint_ctrl
{
    public partial class ws_fin_reprint : PageWebSheet, WebSheet
    {
        [JsPostBack]
        public string PostRetrieve { get; set; }
        [JsPostBack]
        public string PostPrint { get; set; }

        public void InitJsPostBack()
        {
            dsMain.InitDsMain(this);
            dsList.InitDsList(this);
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                dsMain.DDUsername();
                dsMain.DATA[0].ENTRY_ID = state.SsUsername.Trim();
                dsMain.DATA[0].SLIP_DATE_S = state.SsWorkDate;
                dsMain.DATA[0].SLIP_DATE_E = state.SsWorkDate;
                dsList.Retrieve(dsMain.DATA[0].MEMBER_NO, "", dsMain.DATA[0].PAY_RECV_STATUS, "", "", state.SsWorkDate, state.SsWorkDate);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == PostRetrieve)
            {
                try
                {
                    string member_no = "";
                    string entry_id = "";
                    string slip_no_s = "";
                    string slip_no_e = "";
                    decimal payrecv_status = 0;
                    DateTime slip_date_s = dsMain.DATA[0].SLIP_DATE_S;
                    DateTime slip_date_e = dsMain.DATA[0].SLIP_DATE_E;

                    member_no = dsMain.DATA[0].MEMBER_NO;
                    entry_id = dsMain.DATA[0].ENTRY_ID;
                    payrecv_status = dsMain.DATA[0].PAY_RECV_STATUS;
                    slip_no_s = dsMain.DATA[0].SLIP_NO_S;
                    slip_no_e = dsMain.DATA[0].SLIP_NO_E;
                    slip_date_s = dsMain.DATA[0].SLIP_DATE_S;
                    slip_date_e = dsMain.DATA[0].SLIP_DATE_E;
                    dsList.Retrieve(member_no, entry_id, payrecv_status, slip_no_s, slip_no_e, slip_date_s, slip_date_e);
                }
                catch { }
            }
            else if (eventArg == PostPrint)
            {
                bool chk = true;
                string slip_no = "";
                decimal payrecv_status = dsMain.DATA[0].PAY_RECV_STATUS;
                for (int j = 0; j < dsList.RowCount && chk; j++)
                {
                    if (dsList.DATA[j].checkselect == 1)
                    {
                        try
                        {
                            slip_no = dsList.DATA[j].SLIP_NO.Trim();
                            if (payrecv_status == 1)
                            {
                                if (state.SsCoopControl == "034001" || state.SsCoopControl == "040001" || state.SsCoopControl == "038001")
                                {
                                    String report_name = "";
                                    if (state.SsCoopControl == "034001") { report_name = "ir_printfin_receipt_clound"; }
                                    else if (state.SsCoopControl == "040001") { report_name = "ir_printfin_receipt_clound_a4"; }
                                    else if (state.SsCoopControl == "038001") { report_name = "ir_printfin_receipt_clound_tak_a4"; }
                                    string report_label = "ใบเสร็จรับเงิน";

                                    iReportArgument args = new iReportArgument();
                                    //args.Add("as_slip_no", iReportArgumentType.String, slip_no);
                                    args.Add("as_coopid", iReportArgumentType.String, state.SsCoopId);
                                    args.Add("as_slipno", iReportArgumentType.String, slip_no);
                                    iReportBuider report = new iReportBuider(this, "");
                                    report.AddCriteria(report_name, report_label, ReportType.pdf, args);
                                    report.AutoOpenPDF = true;
                                    report.Retrieve();
                                }
                                else
                                {
                                    Printing.PrintFinRecvSlipIreportExat(this, slip_no);
                                }
                            }
                            else if (payrecv_status == 0)
                            {
                                if (state.SsCoopControl == "034001" || state.SsCoopControl == "040001" || state.SsCoopControl == "038001")
                                {
                                    string report_name = "";
                                    if (state.SsCoopControl == "034001") { report_name = "ir_printfin_payslip_clound"; }
                                    else if (state.SsCoopControl == "040001") { report_name = "ir_printfin_payslip_clound_a4"; }
                                    else if (state.SsCoopControl == "038001") { report_name = "ir_printfin_payslip_clound_tak_a4"; }
                                    string report_label = "ใบสำคัญจ่าย";

                                    iReportArgument args = new iReportArgument();
                                    //args.Add("as_slip_no", iReportArgumentType.String, slip_no);
                                    args.Add("as_coopid", iReportArgumentType.String, state.SsCoopId);
                                    args.Add("as_slipno", iReportArgumentType.String, slip_no);
                                    iReportBuider report = new iReportBuider(this, "");
                                    report.AddCriteria(report_name, report_label, ReportType.pdf, args);
                                    report.AutoOpenPDF = true;
                                    report.Retrieve();
                                }
                                else
                                {
                                    Printing.PrintFinPaySlipIreportExat(this, slip_no);
                                }
                                chk = false;
                                LtServerMessage.Text = WebUtil.CompleteMessage("พิมพ์ใบเสร็จสำเร็จ");
                            }
                        }
                        catch (Exception ex)
                        {
                            LtServerMessage.Text = WebUtil.ErrorMessage("พิมพ์ใบเสร็จไม่สำเร็จ กรุณาตรวจสอบ" + ex.Message);
                        }
                    }
                }
            }

        }       
        

       public void SaveWebSheet()
        {
            
        }

        public void WebSheetLoadEnd()
        {
            
        }
    }
}