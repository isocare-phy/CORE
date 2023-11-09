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
    public partial class w_sheet_kt_65pay : PageWebSheet, WebSheet
    {
        protected String postRetreiveDwMain;
        protected String postPay;
        public void InitJsPostBack()
        {
            postRetreiveDwMain = WebUtil.JsPostBack(this, "postRetreiveDwMain");
            postPay = WebUtil.JsPostBack(this, "postPay");
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                DwMainP.InsertRow(0);
            }
            else
            {
                this.RestoreContextDw(DwMainP);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postRetreiveDwMain")
            {
                RetreiveDwMain();
            }
            if (eventArg == "postPay")
            {
                PensionPay();
            }

        }

        public void SaveWebSheet()
        {


        }
        public void RetreiveDwMain()
        {
            string member_no = DwMainP.GetItemString(1, "member_no");
            DwUtil.RetrieveDataWindow(DwMainP, "kt_65years.pbl", null, member_no);
            try
            {
                string check = DwMainP.GetItemString(1, "member_no");
            }
            catch { LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถทำรายการซ้ำ"); }
            string pay_tdate = DateTime.Now.ToString("ddMMyyyy", WebUtil.TH);
            DwMainP.SetItemString(1, "pay_tdate", pay_tdate);
        }
        public void PensionPay()
        {
            //จ่ายลง asnslippayout
            //ดึงข้อมูลมาจ่าย

            string caplital_year = (Convert.ToInt32(DateTime.Now.ToString("yyyy")) + 543).ToString();
            string member_no = DwMainP.GetItemString(1, "member_no");
            decimal perpay = 0;
            decimal sumpays = 0;
            decimal balance = 0;
            DateTime pay_date = DateTime.Now;

            perpay = DwMainP.GetItemDecimal(1, "perpay");
            sumpays = DwMainP.GetItemDecimal(1, "sumpays");
            balance = DwMainP.GetItemDecimal(1, "balance");

            sumpays = sumpays + perpay;
            balance = balance - perpay;

            DwMainP.SetItemDecimal(1, "sumpays", sumpays);
            DwMainP.SetItemDecimal(1, "balance", balance);
            //-------------------------------------------------
            string status = "0";
            if (perpay != 0)//
            {
                try
                {

                    //insert asnslippayout เตรียมข้อมูล insert
                    string payoutslip_no = "";
                    string sqlmaxslip_no = "select max(payoutslip_no) from asnslippayout where payoutslip_no like '" + caplital_year.Substring(2, 2) + "90" + "%'";
                    DataTable slip_no = WebUtil.Query(sqlmaxslip_no);
                    if (slip_no.Rows.Count > 0)
                    {
                        payoutslip_no = slip_no.Rows[0][0].ToString();
                        if (payoutslip_no != "")
                        {
                            payoutslip_no = payoutslip_no.Substring(4, 6);
                        }
                        else { payoutslip_no = "00000"; }
                    }
                    Decimal ass = Convert.ToDecimal(payoutslip_no);
                    ass++;
                    //เลขสลิปตามรูปแบบ
                    string slippayout_no = caplital_year.Substring(2, 2) + "90" + ass.ToString("00000");

                    //---------------------------------------------------------------------------
                    //ลงรายการใน asnslippayout
                    string sqlsavepay = "insert into asnslippayout (payoutslip_no,member_no,slip_date,operate_date,entry_date,payout_amt,slip_status," +
                    "assisttype_code,moneytype_code,post_fin,entry_id) values ('" + slippayout_no + "','" + member_no + "',to_date('" + DateTime.Now.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy')," +
                    "to_date('" + DateTime.Now.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy'),to_date('" + DateTime.Now.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy')," + perpay + ",1,'90','CSH',8,'" + state.SsUsername + "')";
                    DataTable slip_insert = WebUtil.Query(sqlsavepay);
                    status = "1";
                    //------------------------------------------------------------------------------
                }
                catch
                { LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถทำการจ่ายได้"); }

                if (status == "1")
                {
                    try
                    {
                        //อัพเดท asnreqmaster //อัพเดทสเตตัส pay_status = 1
                        string updatepay = "update asnreqmaster set sumpays=" + sumpays + ",balance=" + balance +
                            " , pay_date=to_date('" + DateTime.Now.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy') , pay_status = 1  where member_no='"
                            + member_no + "' and assist_docno like 'SF%'";
                        Sdt updatestatus = WebUtil.QuerySdt(updatepay);
                        LtServerMessage.Text = WebUtil.CompleteMessage("ทำรายการสำเร็จ");
                        //-------------------------------------------------
                    }
                    catch
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถอัพเดทเงินที่จ่ายไปแล้ว");
                    }
                }
            }//endif



        }
        public void WebSheetLoadEnd()
        {

            DwMainP.SaveDataCache();

        }
    }
}