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
    public partial class w_sheet_kt_tran_pension : PageWebSheet, WebSheet
    {
        protected String postRetreiveDwMain;
        protected String postPay;
        protected String QueryData;
        protected String AccFromDiv;
        protected DwThDate tDwMain;
        protected String postDate;
        protected String AccFromMemb;
        protected String postNull;
        protected String postChangeMoneyType;
        protected String postChangeAccId;
        public void InitJsPostBack()
        {
            AccFromDiv = WebUtil.JsPostBack(this, "AccFromDiv");
            postChangeAccId = WebUtil.JsPostBack(this, "postChangeAccId");
            postChangeMoneyType = WebUtil.JsPostBack(this, "postChangeMoneyType");
            postNull = WebUtil.JsPostBack(this, "postNull");
            AccFromMemb = WebUtil.JsPostBack(this, "AccFromMemb");
            postRetreiveDwMain = WebUtil.JsPostBack(this, "postRetreiveDwMain");
            QueryData = WebUtil.JsPostBack(this, "QueryData");
            postPay = WebUtil.JsPostBack(this, "postPay");
            postDate = WebUtil.JsPostBack(this, "postDate");
            tDwMain = new DwThDate(DwMain, this);
            tDwMain.Add("pay_date", "slip_tdate");
        }
        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                DwMain.InsertRow(0);
                //UpDateData();
            }
            else
            {
                this.RestoreContextDw(DwMain);
                this.RestoreContextDw(DwDetail);
            }
            tDwMain.Eng2ThaiAllRow();
        }
        public void CheckJsPostBack(string eventArg)
        {

            if (eventArg == "AccFromDiv")
            {
                CheckBankAccId();
            }
            if (eventArg == "AccFromMemb")
            {
                UpDateData();
            }
            if (eventArg == "QueryData")
            {
                JsQueryData();
            }
            if (eventArg == "postRetreiveDwMain")
            {
                RetreiveDwMain();
            }
            if (eventArg == "postPay")
            {
                PensionPay();
            }
            if (eventArg == "postDate")
            {
                ChangeDate();
            }
            if (eventArg == "postNull")
            {
                JsPostNull();
            }
            if (eventArg == "postChangeMoneyType")
            {
                ChangeMoneyType();
            }
            if (eventArg == "postChangeAccId")
            {
                ChangeAccId();
            }

        }
        public void ChangeAccId()
        {
            //รีทีฟ สาขา

            int r = Convert.ToInt32(hdRow.Value);
            string acc_id = DwDetail.GetItemString(r, "expense_accid").Trim();
            acc_id = acc_id.Substring(0, 3);
            int acc = Convert.ToInt32(acc_id);
            acc_id = acc.ToString("0000");
            DwDetail.SetItemString(r, "expense_branch", acc_id);

            //ตัดเลข
        }
        public void ChangeMoneyType()
        {
            //postback เพื่อโพเทก ddw
            string selectaccid = "";
            int r = Convert.ToInt32(hdRow.Value);
            string bank_code = "";
            string bank_branch = "";
            string bank_accid = "";
            string member_no = DwDetail.GetItemString(r, "member_no");
            string moneytype = DwDetail.GetItemString(r, "expense_type");
            try
            {
                DwDetail.SetItemString(r, "expense_type", moneytype);
                DwDetail.SetItemString(r, "expense_bank", "");
                DwDetail.SetItemString(r, "expense_branch", "");
                DwDetail.SetItemString(r, "expense_accid", "");
            }
            catch { }

            if (moneytype.Trim() == "CBS" || moneytype.Trim() == "D00")
            {
                selectaccid = "select divpaytype_code,bank_code,bank_branch,bank_accid,deptaccount_no from mbdivmethodpayment where member_no='" + member_no + "' and divpaytype_code='" + moneytype.Trim() + "' order by div_year DESC";
                Sdt Qacc = WebUtil.QuerySdt(selectaccid);
                if (Qacc.Next())
                {
                    if (moneytype.Trim() == "CBS")
                    {
                        bank_code = Qacc.GetString("bank_code");
                        bank_branch = Qacc.GetString("bank_branch");
                        bank_accid = Qacc.GetString("bank_accid");
                        DwDetail.SetItemString(r, "expense_bank", bank_code);
                        DwDetail.SetItemString(r, "expense_branch", bank_branch);
                        DwDetail.SetItemString(r, "expense_accid", bank_accid);
                    }
                    else if (moneytype.Trim() == "D00")
                    {
                        bank_accid = Qacc.GetString("deptaccount_no");
                        DwDetail.SetItemString(r, "expense_accid", bank_accid);
                    }
                }

            }
            else//กรณี CBT TRN
            {
                DwDetail.SetItemString(r, "expense_bank", "");
                DwDetail.SetItemString(r, "expense_branch", "");
                DwDetail.SetItemString(r, "expense_accid", "");
            }
        }
        public void JsPostNull()
        {
            //postback เพื่อเซตค่าว่าง
            //int r = Convert.ToInt32(hdRow.Value);
            //DwDetail.SetItemString(r, "expense_accid", "");
            //DwDetail.SetItemString(r, "expense_bank", "");
            //DwDetail.SetItemString(r, "expense_branch", "");
            //DwDetail.SetItemString(r, "remark", "ไม่ได้แจ้ง");
        }
        public void ChangeDate()
        {
            DateTime entry_date = DateTime.ParseExact(hdate.Value, "ddMMyyyy", WebUtil.TH);
            DwMain.SetItemDateTime(1, "pay_date", entry_date);
        }
        public void RetreiveDwMain()
        {

        }
        public void PensionPay()
        {
            string member_no = "";
            string assisttype_code = "";
            string expense_type = "";
            string expense_bank = "";
            string expense_branch = "";
            string expense_accid = "";
            decimal perpay = 0;
            DateTime trandate = state.SsWorkDate;
            try
            {
                trandate = DwMain.GetItemDate(1, "pay_date");
            }
            catch { trandate = state.SsWorkDate; }
            int row = DwDetail.RowCount;
            for (int i = 1; i < row + 1; i++)
            {

                member_no = DwDetail.GetItemString(i, "member_no").Trim();
                assisttype_code = DwDetail.GetItemString(i, "assisttype_code").Trim();
                perpay = DwDetail.GetItemDecimal(i, "perpay");

                try
                {
                    expense_type = DwDetail.GetItemString(1, "expense_type").Trim();
                    expense_bank = DwDetail.GetItemString(i, "expense_bank").Trim();
                    expense_branch = DwDetail.GetItemString(i, "expense_branch").Trim();
                }
                catch
                {
                    expense_bank = "";
                    expense_branch = "";
                }
                try
                {
                    expense_accid = DwDetail.GetItemString(i, "expense_accid").Trim();
                }
                catch { expense_accid = ""; }
                string status = "0";
                if (expense_bank != "" && expense_branch != "" && expense_accid != "" && perpay != 0)
                {
                    string caplital_year = (Convert.ToInt32(state.SsWorkDate.ToString("yyyy")) + 543).ToString();
                    try
                    {
                        //insert asnslippayout เตรียมข้อมูล insert
                        string payoutslip_no = "";
                        string sqlmaxslip_no = "select max(payoutslip_no) from asnslippayout where payoutslip_no like '" + caplital_year.Substring(2, 2) + assisttype_code + "%'";
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
                        string slippayout_no = caplital_year.Substring(2, 2) + assisttype_code + ass.ToString("00000");

                        DateTime entry_date = DwMain.GetItemDateTime(1, "pay_date");//วันที่ต้องการจ่ายเงิน
                        //---------------------------------------------------------------------------
                        //ลงรายการใน asnslippayout
                        string sqlsavepay = "insert into asnslippayout (payoutslip_no,member_no,slip_date,operate_date,entry_date,payout_amt,slip_status," +
                        "assisttype_code,moneytype_code,post_fin,entry_id,bank_code,bankbranch_id,bank_accid) values ('" + slippayout_no + "','" + member_no + "',to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy')," +
                        "to_date('" + entry_date.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy'),to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy')," + perpay + ",1,'" + assisttype_code + "','CBT',8,'" +
                        state.SsUsername + "','" + expense_bank + "','" + expense_branch + "','" + expense_accid + "')";
                        DataTable slip_insert = WebUtil.Query(sqlsavepay);
                        //--------------------------------------------------------------------
                        decimal seqno = 0;
                        try
                        {
                            string maxseq_no = "select max(seq_no) from dpdepttran where system_code='ASS'";
                            Sdt seq_no = WebUtil.QuerySdt(maxseq_no);
                            if (seq_no.Next())
                            { seqno = seq_no.GetDecimal("max(seq_no)"); }
                        }
                        catch { seqno = 0; }
                        seqno++;
                        //--------------------------------------------------------------------

                        string sqldpdepttran = @"insert into dpdepttran (system_code,tran_year,tran_date,member_no,deptaccount_no,branch_id,seq_no, " +
                        " lastcalint_date,deptitem_amt,tran_status,ref_slipno,branch_operate,tranitem_type) " +
                        " values " +
                        " ('ASS'," + caplital_year + ",to_date('" + entry_date.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy'),'" + member_no + "','" + expense_accid + "','001'," + seqno + ", " +
                        " to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy')," + perpay + ",0,'" + slippayout_no + "','001','DAP' " +
                        ")";
                        Sdt insertdp = WebUtil.QuerySdt(sqldpdepttran);

                        status = "1";
                    }
                    catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
                }
                else
                {
                    //พวกนี้อีกแบบ
                    string caplital_year = (Convert.ToInt32(state.SsWorkDate.ToString("yyyy")) + 543).ToString();
                    try
                    {
                        //insert asnslippayout เตรียมข้อมูล insert
                        string payoutslip_no = "";
                        string sqlmaxslip_no = "select max(payoutslip_no) from asnslippayout where payoutslip_no like '" + caplital_year.Substring(2, 2) + assisttype_code + "%'";
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
                        string slippayout_no = caplital_year.Substring(2, 2) + assisttype_code + ass.ToString("00000");

                        DateTime entry_date = DwMain.GetItemDateTime(1, "pay_date");//วันที่ต้องการจ่ายเงิน
                        //---------------------------------------------------------------------------
                        //ลงรายการใน asnslippayout
                        string sqlsavepay = "insert into asnslippayout (payoutslip_no,member_no,slip_date,operate_date,entry_date,payout_amt,slip_status," +
                        "assisttype_code,moneytype_code,post_fin,entry_id,bank_code,bankbranch_id,bank_accid) values ('" + slippayout_no + "','" + member_no + "',to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy')," +
                        "to_date('" + entry_date.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy'),to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy')," + perpay + ",1,'" + assisttype_code + "','" + expense_type + "',8,'" +
                        state.SsUsername + "','000','0000','" + expense_accid + "')";
                        DataTable slip_insert = WebUtil.Query(sqlsavepay);
                        //--------------------------------------------------------------------
                        decimal seqno = 0;
                        try
                        {
                            string maxseq_no = "select max(seq_no) from dpdepttran where system_code='ASS'";
                            Sdt seq_no = WebUtil.QuerySdt(maxseq_no);
                            if (seq_no.Next())
                            { seqno = seq_no.GetDecimal("max(seq_no)"); }
                        }
                        catch { seqno = 0; }
                        seqno++;

                        //--------------------------------------------------------------------

                        sqlsavepay = @"insert into dpdepttran (system_code,tran_year,tran_date,member_no,deptaccount_no,branch_id,seq_no, " +
                        " lastcalint_date,deptitem_amt,tran_status,ref_slipno,branch_operate,tranitem_type) " +
                        " values " +
                        " ('ASS'," + caplital_year + ",to_date('" + entry_date.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy'),'" + member_no + "','" + expense_accid + "','001'," + seqno + ", " +
                        " to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy')," + perpay + ",0,'" + slippayout_no + "','001','DAP' " +
                        ")";
                        slip_insert = WebUtil.Query(sqlsavepay);
                        status = "1";
                    }//end else
                    catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
                }
                //อัพเดท
                if (status == "1")
                {
                    decimal sumpays = 0;
                    decimal balance = 0;
                    //คำนวณ
                    try
                    {
                        string selectsum = "select sumpays,balance from asnreqmaster where member_no='" + member_no + "' and assisttype_code ='" + assisttype_code + "'";
                        Sdt sump = WebUtil.QuerySdt(selectsum);
                        if (sump.Next())
                        {
                            sumpays = sump.GetDecimal("sumpays");
                            balance = sump.GetDecimal("balance");
                        }
                        sumpays = sumpays + perpay;
                        balance = balance - perpay;
                    }
                    catch { }
                    try
                    {
                        //อัพเดท asnreqmaster //อัพเดทสเตตัส pay_status = 1
                        string updatepay = "update asnreqmaster set sumpays=" + sumpays + ",balance=" + balance +
                            " , pay_date=to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy') , pay_status = 1  where member_no='"
                            + member_no + "' and assisttype_code ='" + assisttype_code + "'";
                        Sdt updatestatus = WebUtil.QuerySdt(updatepay);
                        //-------------------------------------------------
                        decimal withdraw_count = 0;
                        string sqlwithdraw = "select withdraw_count from asnpensionperpay where member_no='" + member_no + "' ";
                        updatestatus = WebUtil.QuerySdt(sqlwithdraw);
                        if (updatestatus.Next())
                        {
                            withdraw_count = updatestatus.GetDecimal("withdraw_count");
                        }
                        //-------------------------------------------------
                        withdraw_count = withdraw_count + 1;
                        string updateperpay = "update asnpensionperpay set withdraw_count =" + withdraw_count + " where member_no='" + member_no + "'";
                        updatestatus = WebUtil.QuerySdt(updateperpay);

                        //-------------------------------------------------
                        LtServerMessage.Text = WebUtil.CompleteMessage("ทำรายการสำเร็จ");
                    }
                    catch
                    {
                        //LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถอัพเดทเงินที่จ่ายไปแล้ว");
                    }
                }

            }//endfor
        }
        public void SaveWebSheet()
        {
            try
            {
                DwUtil.UpdateDateWindow(DwDetail, "kt_pension.pbl", "asnreqmaster");
                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }
        public void WebSheetLoadEnd()
        {
            try
            {
                DwUtil.RetrieveDDDW(DwDetail, "expense_bank", "kt_pension.pbl", null);
                DwUtil.RetrieveDDDW(DwDetail, "expense_branch", "kt_pension.pbl", null);
            }
            catch
            { }
            tDwMain.Eng2ThaiAllRow();
            DwDetail.SaveDataCache();
            DwMain.SaveDataCache();
        }
        public void CheckBankAccId()
        {
            int r = DwDetail.RowCount;
            string member_no = "";
            string expense_bank = "";
            string expense_branch="";
            string expense_accid = "";
            string sqlaccid = "";
            string moneytype = "";

            for (int i = 1; i < r + 1; i++)
            {
                member_no = DwDetail.GetItemString(i, "member_no");
                try
                {
                    expense_accid = DwDetail.GetItemString(i, "expense_accid");
                }
                catch
                {
                    expense_accid = "";
                }
                if (expense_accid == "")
                {
                    try
                    {
                        sqlaccid = "select divpaytype_code,bank_code,bank_branch,bank_accid,deptaccount_no from mbdivmethodpayment where member_no='" + member_no + "' and (divpaytype_code='CBT' or divpaytype_code='CBS' or divpaytype_code='D00') order by div_year ASC";
                        Sdt div = WebUtil.QuerySdt(sqlaccid);
                        if (div.Next())
                        {
                            moneytype = div.GetString("divpaytype_code");
                            if (moneytype == "CBT" || moneytype == "CBS")
                            {
                                expense_bank = div.GetString("bank_code");
                                expense_branch = div.GetString("bank_branch");
                                expense_accid = div.GetString("bank_accid");
                            }
                            else
                            {
                                expense_accid = div.GetString("deptaccount_no");
                                expense_bank = "";
                                expense_branch = "";
                            }
                            DwDetail.SetItemString(i, "expense_type", moneytype);
                            DwDetail.SetItemString(i, "expense_bank", expense_bank);
                            DwDetail.SetItemString(i, "expense_branch", expense_branch);
                            DwDetail.SetItemString(i, "expense_accid", expense_accid);
                            DwDetail.SetItemString(i, "remark", "วิธีปันผล");
                        }
                        else
                        {
                            DwDetail.SetItemString(i, "remark", "ไม่ได้แจ้ง");
                        }
                    }
                    catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
                }
            }
        }
        public void UpDateData()
        {
            //ดึงข้อมูลเลขบัญชีจาก moneytr
            //string sqlQmemb = "";
            string sqlQmoneytr = "";
            //string sqluptr = "";
            string member_no = "";
            string moneytype_code = "";
            string bank_code = "";
            string bank_branch = "";
            string bank_accid = "";
            int r = DwDetail.RowCount;

            try
            {
                for (int i = 1; i < r + 1; i++)
                {
                    member_no = DwDetail.GetItemString(i,"member_no");
                    try
                    {
                        sqlQmoneytr = "select moneytype_code,bank_code,bank_branch,bank_accid from mbmembmoneytr where trtype_code='RCV65' and  member_no='" + member_no + "'";
                        Sdt Qtr = WebUtil.QuerySdt(sqlQmoneytr);
                        if (Qtr.Next())
                        {
                            moneytype_code = Qtr.GetString("moneytype_code");
                            bank_code = Qtr.GetString("bank_code");
                            bank_branch = Qtr.GetString("bank_branch");
                            bank_accid = Qtr.GetString("bank_accid");

                            DwDetail.SetItemString(i, "expense_type", moneytype_code);
                            DwDetail.SetItemString(i, "expense_bank", bank_code);
                            DwDetail.SetItemString(i, "expense_branch", bank_branch);
                            DwDetail.SetItemString(i, "expense_accid", bank_accid);
                            DwDetail.SetItemString(i, "remark", "สมาชิกแจ้ง");
                        }
                        else
                        {
                            try
                            {
                                bank_accid = DwDetail.GetItemString(i, "expense_accid");
                            }
                            catch
                            {
                                bank_accid = "";
                            }
                            if (bank_accid == "")
                            {
                                DwDetail.SetItemString(i, "expense_type", "");
                                DwDetail.SetItemString(i, "expense_bank", "");
                                DwDetail.SetItemString(i, "expense_branch", "");
                                DwDetail.SetItemString(i, "expense_accid", "");
                                DwDetail.SetItemString(i, "remark", "ไม่ได้แจ้ง");
                            }
                        }
                    }
                    catch { }
                }
                //JsQueryData();
            }
            catch { }
        }
        public void JsQueryData()
        {
            try
            {
                DwUtil.RetrieveDataWindow(DwDetail, "kt_pension.pbl", null, null);
                string test = DwDetail.GetItemString(1, "member_no");
            }
            catch
            {
                DwDetail.Reset();
            }
            //CheckBankAccId();
        }
    }
}