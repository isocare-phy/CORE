using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using DataLibrary;

namespace Saving.Applications.shrlon
{
    public partial class ws_sl_reprint : PageWebSheet, WebSheet
    {
        [JsPostBack]
        public string PostRetrieve { get; set; }
        [JsPostBack]
        public string PostPrint { get; set; }
        [JsPostBack]
        public string PostPrint2 { get; set; }

        public void InitJsPostBack()
        {
            dsMain.InitDsMain(this);
            dsList.InitDsList(this);
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                dsMain.DdCode();
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
                    string sliptype_code = "";
                    string document_no_s = "";
                    string document_no_e = "";
                    DateTime slip_date_s = dsMain.DATA[0].SLIP_DATE_S;
                    DateTime slip_date_e = dsMain.DATA[0].SLIP_DATE_E;

                    member_no = dsMain.DATA[0].MEMBER_NO;
                    entry_id = dsMain.DATA[0].ENTRY_ID;
                    sliptype_code = dsMain.DATA[0].SLIPTYPE_CODE;
                    document_no_s = dsMain.DATA[0].document_no_e;
                    document_no_e = dsMain.DATA[0].document_no_e;

                    dsList.Retrieve(member_no, entry_id, sliptype_code, document_no_s, document_no_e, slip_date_s, slip_date_e);
                    int row = dsList.RowCount;
                    for (int i = 0; i < row; i++)
                    {
                        decimal slip_status = dsList.DATA[i].SLIP_STATUS;
                        if (slip_status < 0)
                        {
                            dsList.FindTextBox(i, "COMPUTE_1").BackColor = System.Drawing.Color.DarkGray;
                            dsList.FindTextBox(i, "document_no").BackColor = System.Drawing.Color.DarkGray;
                            dsList.FindTextBox(i, "slip_date").BackColor = System.Drawing.Color.DarkGray;
                            dsList.FindTextBox(i, "entry_date").BackColor = System.Drawing.Color.DarkGray;
                            dsList.FindTextBox(i, "member_no").BackColor = System.Drawing.Color.DarkGray;
                            dsList.FindTextBox(i, "COMPUTE_2").BackColor = System.Drawing.Color.DarkGray;
                            dsList.FindTextBox(i, "entry_id").BackColor = System.Drawing.Color.DarkGray;
                        }
                    }
                }
                catch { }
            }
            else if (eventArg == PostPrint)
            {
                // ตรวจสอบก่อนว่าพิมพ์แบบไหน
                string sqlprint = "select printslip_type, ireport_obj from lnloanconstant ";
                Sdt dtp = WebUtil.QuerySdt(sqlprint);
                string printtype = "PB", ireportobj = "r_sl_slip_in_exat_a4_table";

                if (dtp.Next())
                {
                    printtype = dtp.GetString("printslip_type");
                    ireportobj = dtp.GetString("ireport_obj");
                }
                else
                {
                    printtype = "PB";
                    ireportobj = "r_sl_slip_in_exat_a4_table";
                }

                string rslip = "";
                string ls_payinno = "";
                int[] prt_arr = new int[dsList.RowCount];

                for (int i = 0; i < dsList.RowCount; i++)
                {
                    if (dsList.DATA[i].checkselect != 1)
                    {
                        continue;
                    }

                    ls_payinno = dsList.DATA[i].PAYINSLIP_NO;

                    // พิมพ์เป็นแบบการพิมพ์เลย
                    if (printtype == "PBA")
                    {
                        Printing.PrintSlipSlpayin(this, ls_payinno, state.SsCoopControl);
                        continue;
                    }

                    rslip += "," + ls_payinno + "";
                }

                // ถ้าเป็นการพิมพ์เลย ไม่ต้องทำข้างล่างต่อเพราะพิมพ์หมดแล้ว
                if (printtype == "PBA")
                {
                    return;
                }

                // ถ้ามีข้อมูลตัด comma ตัวแรกทิ้ง
                if (rslip.Trim() != "")
                {
                    rslip = rslip.Substring(1);
                }

                if (printtype == "IR")
                {
                    //พิมพ์แบบireport
                    Printing.RePrintSlipSlInIreportExat(this, rslip, state.SsCoopControl, ireportobj);
                }
                else if (printtype == "JS" || printtype == "PB")
                {
                    // ถ้าเป็นการพิมพ์ผ่าน PB ให้ตัด quote หน้าหลังทิ้ง
                    rslip = rslip.Substring(1, rslip.Length - 2);
                    Printing.PrintSlipSlpayin(this, rslip, state.SsCoopControl);
                }

                // หาไม่เจอว่าใครใช้ ใครเดือนร้อนก็เอากลับแล้วกัน
                //Printing.ShrlonPrintSlipPayIn(this, state.SsCoopControl, PayinslipNo, 2);
            }
            else if (eventArg == PostPrint2) //ใบเสร็จรับเงิน-จ่ายเงินกู้ = sl_slip_payout สอ.มสธ.
            {
                // ตรวจสอบก่อนว่าพิมพ์แบบไหน
                string sqlprint = "select printslip_type, ireport_obj from lnloanconstant ";
                Sdt dtp = WebUtil.QuerySdt(sqlprint);
                string printtype = "PB", ireportobj = "r_sl_slip_in_exat_a4_table";

                if (dtp.Next())
                {
                    printtype = dtp.GetString("printslip_type");
                    ireportobj = dtp.GetString("ireport_obj");
                }
                else
                {
                    printtype = "PB";
                    ireportobj = "r_sl_slip_in_exat_a4_table";
                }

                string rslip = "";
                string ls_payinno = "";
                int[] prt_arr = new int[dsList.RowCount];

                for (int i = 0; i < dsList.RowCount; i++)
                {
                    if (dsList.DATA[i].checkselect != 1)
                    {
                        continue;
                    }

                    ls_payinno = dsList.DATA[i].PAYINSLIP_NO;

                    // พิมพ์เป็นแบบการพิมพ์เลย
                    if (printtype == "PBA")
                    {
                        //Printing.PrintSlipSlpayin(this, ls_payinno, state.SsCoopControl);
                        if (state.SsCoopControl == "030001") //ใบเสร็จรับเงิน-จ่ายเงินกู้ = sl_slip_payout 
                        {
                            Printing.PrintSlipSlpayOut(this, ls_payinno, state.SsCoopControl);
                        }
                        continue;
                    }

                    rslip += "','" + ls_payinno + "'";
                }

                // ถ้าเป็นการพิมพ์เลย ไม่ต้องทำข้างล่างต่อเพราะพิมพ์หมดแล้ว
                if (printtype == "PBA")
                {
                    return;
                }

                // ถ้ามีข้อมูลตัด comma ตัวแรกทิ้ง
                if (rslip.Trim() != "")
                {
                    rslip = rslip.Substring(1);
                }

                if (printtype == "IR")
                {
                    //พิมพ์แบบireport
                    Printing.RePrintSlipSlInIreportExat(this, rslip, state.SsCoopControl, ireportobj);
                }
                else if (printtype == "JS" || printtype == "PB")
                {
                    // ถ้าเป็นการพิมพ์ผ่าน PB ให้ตัด quote หน้าหลังทิ้ง
                    rslip = rslip.Substring(1, rslip.Length - 2);
                    Printing.PrintSlipSlpayin(this, rslip, state.SsCoopControl);
                }

                // หาไม่เจอว่าใครใช้ ใครเดือนร้อนก็เอากลับแล้วกัน
                //Printing.ShrlonPrintSlipPayIn(this, state.SsCoopControl, PayinslipNo, 2);
            }
        }

        private static string XmlReadVar(string responseData, string szVar)
        {
            int i1stLoc = responseData.IndexOf("<" + szVar + ">");
            if (i1stLoc < 0)
                return string.Empty;
            int ilstLoc = responseData.IndexOf("</" + szVar + ">");
            if (ilstLoc < 0)
                return string.Empty;
            int len = szVar.Length;
            return responseData.Substring(i1stLoc + len + 2, ilstLoc - i1stLoc - len - 2);
        }

        public void SaveWebSheet()
        {

        }

        public void WebSheetLoadEnd()
        {

        }
    }
}
