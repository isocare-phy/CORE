using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using CoreSavingLibrary.WcfShrlon;
using CoreSavingLibrary.WcfNShrlon;
using System.Drawing;
using CoreSavingLibrary;


namespace Saving.Applications.mbshr.ws_sl_share_withdraw_ctrl
{
    public partial class ws_sl_share_withdraw : PageWebSheet, WebSheet
    {
        [JsPostBack]
        public string PostCheckSelect { get; set; }
        [JsPostBack]
        public string PostShowData { get; set; }
        [JsPostBack]
        public string PostListMemberNo { get; set; }
        [JsPostBack]
        public string Postmember_serach { get; set; }
        [JsPostBack]
        public string PostPrintSlippayin { get; set; }

        public void InitJsPostBack()
        {
            dsMain.InitDsMain(this);
            dsList.InitDsList(this);
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                dsMain.RetrieveCoop();
                dsMain.DATA[0].coop_id = state.SsCoopId;
                dsMain.DATA[0].SHAREMASTER_STATUS = 8;
                dsList.RetrieveList(dsMain.DATA[0].coop_id, dsMain.DATA[0].SHAREMASTER_STATUS);
                Hdrow_member.Value = "-1";
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == PostShowData)
            {
                if (HdSlipno.Value.Trim() != "" || HdSlipno.Value != "")
                {
                    //PostPrintSlip(state.SsCoopControl); 
                }

                dsList.RetrieveList(dsMain.DATA[0].coop_id, dsMain.DATA[0].SHAREMASTER_STATUS);
            }
            else if (eventArg == PostCheckSelect)
            {
                if (dsMain.DATA[0].checkselect == 0)
                {
                    for (int i = 0; i < dsList.RowCount; i++)
                    {
                        dsList.SetItem(i, dsList.DATA.OPERATE_FLAGColumn, 0);
                    }
                }
                else if (dsMain.DATA[0].checkselect == 1)
                {
                    for (int i = 0; i < dsList.RowCount; i++)
                    {
                        dsList.SetItem(i, dsList.DATA.OPERATE_FLAGColumn, 1);
                    }
                }
            }
            else if (eventArg == Postmember_serach)
            {
                int index_row = Convert.ToInt16(Hdrow_member.Value);
                setcolordefault();
                setcolor_row(index_row);
                Hdrow_member.Value = Convert.ToString(index_row);
                dsMain.DATA[0].MEMBER_NO = Hdmember_no.Value;
                dsList.RetrieveMemberNo(dsMain.DATA[0].MEMBER_NO);
            }
            else if (eventArg == PostPrintSlippayin)
            {
                string payinslip_no = HdSlipno.Value;
                Printing.PrintSlipSlInIreportTnt(this, payinslip_no, state.SsCoopId);
            }
        }

        public void SaveWebSheet()
        {

        }

        public void WebSheetLoadEnd()
        {

            if (state.SsCoopControl == "013001") //GSB
            {
                if (HdSlipno.Value == "")
                {
                    dsMain.FindButton(0, "b_print").Style.Add(HtmlTextWriterStyle.Visibility, "hidden");
                }
                else
                {
                    dsMain.FindButton(0, "b_print").Style.Add(HtmlTextWriterStyle.Visibility, "visible");
                }
            }
            else
            {
                dsMain.FindButton(0, "b_print").Style.Add(HtmlTextWriterStyle.Visibility, "hidden");
            }
        }

        private void setcolor_row(int index_row)
        {
            Color myRgbColor = new Color();
            myRgbColor = Color.FromArgb(92, 172, 238);

            dsList.FindTextBox(index_row, "sharetype_code").BackColor = myRgbColor;
            dsList.FindTextBox(index_row, "member_no").BackColor = myRgbColor;
            dsList.FindTextBox(index_row, "cp_mbname").BackColor = myRgbColor;
            dsList.FindTextBox(index_row, "membgroup_code").BackColor = myRgbColor;
            dsList.FindTextBox(index_row, "resign_date").BackColor = myRgbColor;
            dsList.FindTextBox(index_row, "cp_shareamt").BackColor = myRgbColor;
            dsList.FindTextBox(index_row, "cp_sharemasterstatus").BackColor = myRgbColor;
            dsList.FindCheckBox(index_row, "operate_flag").BackColor = myRgbColor;

        }

        private void setcolordefault()
        {
            Color myRgbColor = new Color();
            myRgbColor = Color.FromArgb(255, 255, 255);
            for (int i = 0; i < dsList.RowCount; i++)
            {
                dsList.FindTextBox(i, "sharetype_code").BackColor = myRgbColor;
                dsList.FindTextBox(i, "member_no").BackColor = myRgbColor;
                dsList.FindTextBox(i, "cp_mbname").BackColor = myRgbColor;
                dsList.FindTextBox(i, "membgroup_code").BackColor = myRgbColor;
                dsList.FindTextBox(i, "resign_date").BackColor = myRgbColor;
                dsList.FindTextBox(i, "cp_shareamt").BackColor = myRgbColor;
                dsList.FindTextBox(i, "cp_sharemasterstatus").BackColor = myRgbColor;
                dsList.FindCheckBox(i, "operate_flag").BackColor = myRgbColor;
            }
        }

        private void PostPrintSlip(string coopId)
        {
            string[] reportobj = WebUtil.GetIreportObjPrintLoan();
            string printslip_type = reportobj[0];
            string ireport_obj = reportobj[1].Trim();
            string ireportpayout_obj = reportobj[2].Trim();

            if (coopId == "022001")
            {
                Printing.PrintSlipSlInOutIrExat(this, HdSlipno.Value, state.SsCoopControl);
            }
            else if (coopId == "020001")
            {
                Printing.PrintIRSlippayOutPBN(this, coopId, HdSlipno.Value, ireportpayout_obj);
            }
            else if (printslip_type == "IR")
            {
                Printing.PrintIRSlip(this, coopId, HdSlipno.Value, ireportpayout_obj);
            }
        }

    }
}