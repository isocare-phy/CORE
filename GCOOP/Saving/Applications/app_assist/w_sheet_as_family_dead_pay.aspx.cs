using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommonLibrary;
using CommonLibrary.WsMis;
using System.Web.Services.Protocols;
using Sybase.DataWindow;
using DBAccess;
using CommonLibrary.WsAssist;

namespace Saving.Applications.app_assist
{
    public partial class w_sheet_as_family_dead_pay : PageWebSheet, WebSheet 
    {
        private string pbl = "kt_paydead.pbl";
        private Assist assistService;
        protected string postPay;
        public void InitJsPostBack()
        {
            postPay = WebUtil.JsPostBack(this, "postPay");
        }

        public void WebSheetLoadBegin()
        {
            try
            {
                assistService = WsCalling.Assist;

            }
            catch
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ติดต่อ Web Service ไม่ได้");
                return;
            }
            if (!IsPostBack)
            {
                dw_main.Reset();
                DwUtil.RetrieveDataWindow(dw_main, pbl, null);
                
            }
            else
            {
                this.RestoreContextDw(dw_main);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postPay")
            {
                JspostPay();
            }
        }

        public void SaveWebSheet()
        {
            string entry_id = state.SsUsername;
            DateTime entry_date = DateTime.Now;
            DateTime operate_date = state.SsWorkDate;
            string member_no = "", payoutslip_no = "", moneytype_code = "",bank_code = "" , bankbranch_id = "", bankacc_id = "";
            decimal payout_amt = 0 , pay_status = 0;
            int year = Convert.ToInt32(operate_date.ToString("yyyy")) + 543;
            
            int rowCount = dw_main.RowCount;
            for (int i = 1; i <= rowCount; i++)
            {
                pay_status = dw_main.GetItemDecimal(i, "pay_status");
                if (pay_status == 1)
                {
                    payoutslip_no = assistService.GetNewDocNo(state.SsWsPass, "ASNPAYDEADSLIP", year);

                    member_no = dw_main.GetItemString(i, "member_no");
                    moneytype_code = dw_main.GetItemString(i, "mbgaindetail_moneytype_code");
                    if (moneytype_code == "CBT")
                    {
                        bank_code = dw_main.GetItemString(i, "mbgaindetail_bank_code");
                        bankbranch_id = dw_main.GetItemString(i, "mbgaindetail_branch_id");
                        bankacc_id = dw_main.GetItemString(i, "mbgaindetail_account_id");
                    }
                    payout_amt = dw_main.GetItemDecimal(i, "mbgaindetail_pay_amt");

                    try
                    {

                        string InsertSQL = "insert into asnslippayout(payoutslip_no , member_no , slip_date , operate_date , payout_amt , slip_status , assisttype_code , moneytype_code , bank_code , bankbranch_id , bank_accid , entry_id , entry_date , post_fin)" + @" 
                                                       values('" + payoutslip_no + "','" + member_no + "',to_date('" + operate_date.ToString("dd/MM/yyyy") + "','dd/mm/yyyy'),to_date('" + operate_date.ToString("dd/MM/yyyy") + "','dd/mm/yyyy')," + payout_amt + ",1,40,'" + moneytype_code + "','" + bank_code + "','" + bankbranch_id + "','" + bankacc_id + "','" + entry_id + "',to_date('" + entry_date.ToString("dd/MM/yyyy") + "','dd/mm/yyyy'),8)";
                        WebUtil.Query(InsertSQL);

                        string updateSQL = "update asnfamilydead set pay_status = 1 where member_no = '" + member_no + "'";
                        WebUtil.Query(updateSQL);

                        LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จแล้ว");
                    }
                    catch (Exception ex)
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage("บันทึกข้อมูลไม่สำเร็จ - " + ex);
                    }
                }
            }
            dw_main.Reset();
            DwUtil.RetrieveDataWindow(dw_main, pbl, null);
        }

        public void WebSheetLoadEnd()
        {
            DwUtil.RetrieveDDDW(dw_main, "mbgaindetail_concern_code", pbl);
            dw_main.SaveDataCache();
        }

        public void JspostPay()
        {
            try
            {
                for (int i = 1; i <= dw_main.RowCount; i++)
                {
                    dw_main.SetItemDecimal(i, "pay_status", 1);
                }
            }
            catch { }
        }
    }
}