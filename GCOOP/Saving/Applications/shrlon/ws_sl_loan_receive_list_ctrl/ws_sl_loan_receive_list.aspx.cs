using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataLibrary;
using System.Drawing;
using CoreSavingLibrary.WcfNShrlon;
using System.Data;
using System.IO;

namespace Saving.Applications.shrlon.ws_sl_loan_receive_list_ctrl
{
    public partial class ws_sl_loan_receive_list : PageWebSheet, WebSheet
    {
        [JsPostBack]
        public string PostShowData { get; set; }
        [JsPostBack]
        public string PostMemberNo { get; set; }
        [JsPostBack]
        public string PostPrintSlip { get; set; }
        [JsPostBack]
        public string PostPrintRcvSlip { get; set; }

        public void InitJsPostBack()
        {
            dsMain.InitDsMain(this);
            dsList.InitDsList(this);
        }

        public void WebSheetLoadBegin()
        {
            dsMain.RetriveEntryid();

            if (!IsPostBack)
            {
                dsMain.DATA[0].GROUP = "01";
                dsList.RetrieveList("01", "%", "");
                dsMain.DATA[0].LIST_QUANTITY = "0";
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == PostShowData)
            {
                this.ShowData();

            }
            else if (eventArg == PostMemberNo)
            {
                string ls_membno = WebUtil.MemberNoFormat(dsMain.DATA[0].MEMBER_NO);

                dsMain.DATA[0].MEMBER_NO = ls_membno;
                setcolordefault();
                for (int i = 0; i < dsList.RowCount; i++)
                {
                    if (dsList.DATA[i].MEMBER_NO == ls_membno)
                    {
                        setcolor_row(i);
                        dsList.FindTextBox(i, "member_no").Focus();
                    }
                }
            }
            else if (eventArg == PostPrintSlip)
            {
                string payoutslip_no = HdPayoutNo.Value.Trim();
                string payinslip_no = HdPayinNo.Value.Trim();

                LtServerMessage.Text = payoutslip_no + " : " + payinslip_no;
                try
                {
                    string[] reportobj = WebUtil.GetIreportObjPrintLoan();
                    string printslip_type = reportobj[0];
                    string ireport_obj = reportobj[1];
                    string ireportpayout_obj = reportobj[2];
                    if (state.SsCoopControl == "035001")
                    {
                        payoutslip_no = payoutslip_no.Replace("'", "").Trim();
                        payinslip_no = payinslip_no.Replace("'", "").Trim();
                    }
                    if (state.SsCoopControl == "013001") // ของออมสิน
                    {
                        Printing.PrintSlipSlOutIreportGsb(this, payoutslip_no, state.SsCoopId);
                    }
                    else if (state.SsCoopControl == "020001") // ของเพชรบูรณ์
                    {
                        Printing.PrintIRSlippayOutPBN(this, state.SsCoopControl, payoutslip_no, ireportpayout_obj);
                    }
                    else // ของพวกที่ใช้ส่วนกลาง
                    {
                        if (printslip_type == "IR")
                        {
                            Printing.PrintIRSlip(this, state.SsCoopControl, payoutslip_no, ireportpayout_obj);
                        }
                        else if (printslip_type == "PB" || printslip_type == "JS")
                        {
                            // ถ้าไม่มีใบเสร็จส่งเข้ามาให้ออก
                            if (payinslip_no == null || payinslip_no.Trim() == "")
                            {
                                return;
                            }

                            // ถ้าเป็นการพิมพ์ผ่าน PB ให้ตัด quote หน้าหลังทิ้ง
                            payinslip_no = payinslip_no.Substring(1, payinslip_no.Length - 2);
                            Printing.PrintSlipSlpayin(this, payinslip_no, state.SsCoopControl);
                        }
                        else if (printslip_type == "PBA")
                        {
                            Printing.PrintIRSlippayout(this, state.SsCoopControl, payoutslip_no, ireportpayout_obj);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
                }

                if (state.SsCoopControl == "028001" || state.SsCoopControl == "035001")
                {
                    this.ShowData();
                }
            }
            else if (eventArg == "PostPrintRcvSlip")
            {
                string[] reportobj = WebUtil.GetIreportObjPrintLoan();
                string printslip_type = reportobj[0];
                string ireport_obj = reportobj[1];
                string ireportpayout_obj = reportobj[2];
                string payinslip_no = HdPayinNo.Value.Trim();
                payinslip_no = payinslip_no.Replace("'", "").Trim();
                if (state.SsCoopControl == "035001")
                {
                    Printing.PrintIRSlip(this, state.SsCoopControl, payinslip_no, ireport_obj);
                }
            }
        }

        private void setcolordefault()
        {
            Color myRgbColor = new Color();
            myRgbColor = Color.FromArgb(255, 255, 255);
            for (int index_row = 0; index_row < dsList.RowCount; index_row++)
            {
                dsList.FindTextBox(index_row, "lnrcvfrom_code").BackColor = myRgbColor;
                dsList.FindTextBox(index_row, "loancontract_no").BackColor = myRgbColor;
                dsList.FindTextBox(index_row, "prefix").BackColor = myRgbColor;
                dsList.FindTextBox(index_row, "member_no").BackColor = myRgbColor;
                dsList.FindTextBox(index_row, "name").BackColor = myRgbColor;
                dsList.FindTextBox(index_row, "membgroup_code").BackColor = myRgbColor;
                dsList.FindTextBox(index_row, "withdrawable_amt").BackColor = myRgbColor;
            }
        }

        private void setcolor_row(int index_row)
        {
            Color myRgbColor = new Color();
            myRgbColor = Color.FromArgb(92, 172, 238);

            dsList.FindTextBox(index_row, "lnrcvfrom_code").BackColor = myRgbColor;
            dsList.FindTextBox(index_row, "loancontract_no").BackColor = myRgbColor;
            dsList.FindTextBox(index_row, "prefix").BackColor = myRgbColor;
            dsList.FindTextBox(index_row, "member_no").BackColor = myRgbColor;
            dsList.FindTextBox(index_row, "name").BackColor = myRgbColor;
            dsList.FindTextBox(index_row, "membgroup_code").BackColor = myRgbColor;
            dsList.FindTextBox(index_row, "withdrawable_amt").BackColor = myRgbColor;
        }

        private void ShowData()
        {
            try
            {
                string group = "", entry = "", str_query = "";
                decimal list_quantity = Convert.ToDecimal(dsMain.DATA[0].LIST_QUANTITY);
                if (list_quantity > 0)
                {
                    str_query = " where rownum <= " + list_quantity;
                }

                if (dsMain.DATA[0].GROUP == "0")
                {
                    group = "%";
                }
                else
                {
                    group = dsMain.DATA[0].GROUP + "%";
                }

                entry = "%" + dsMain.DATA[0].ENTRY_ID + "%";

                dsList.RetrieveList(group, entry, str_query);
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
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