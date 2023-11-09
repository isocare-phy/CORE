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
using CommonLibrary.WsAssist;
using CommonLibrary.WsFinance;

namespace Saving.Applications.app_assist
{
    public partial class w_sheet_kt_helpcoll : PageWebSheet, WebSheet
    {
        string pbl = "kt_50bath.pbl";
        protected DwThDate tDwMain;
        protected String postInitMemb;
        protected String postmoneytype_code;
        protected String postbank_code;

        private Assist AsnService;
        private Finance Fin;

        public void InitJsPostBack()
        {
            postInitMemb = WebUtil.JsPostBack(this, "postInitMemb");
            postmoneytype_code = WebUtil.JsPostBack(this, "postmoneytype_code");
            postbank_code = WebUtil.JsPostBack(this, "postbank_code");
            //------------------------------------------
            tDwMain = new DwThDate(DwMain, this);
            tDwMain.Add("member_date", "member_tdate");
            tDwMain.Add("operate_date", "operate_tdate");
        }

        public void WebSheetLoadBegin()
        {
            AsnService = WsCalling.Assist;
            Fin =  WsCalling.Finance;
            
            if (!IsPostBack)
            {
                DwMem.InsertRow(0);
                DwMain.InsertRow(0);
                DwDetail.InsertRow(0);
                string tofrom_accid = Fin.DefaultAccId(state.SsWsPass, DwMain.GetItemString(1, "moneytype_code"));
                DwMain.SetItemString(1, "tofrom_accid", tofrom_accid);
                DwMain.SetItemDateTime(1, "operate_date", DateTime.Today);
            }
            else
            {
                this.RestoreContextDw(DwMem);
                this.RestoreContextDw(DwMain);
                this.RestoreContextDw(DwDetail);
                this.RestoreContextDw(DwColl);
            }
            tDwMain.Eng2ThaiAllRow();
        }

        public void CheckJsPostBack(string eventArg)
        {
            switch (eventArg)
            {
                case "postInitMemb":
                    InintMemb();
                    break;
                case "postmoneytype_code":
                    DwMain.SetItemString(1, "asnslippayout_bank_code", "");
                    DwMain.SetItemString(1, "asnslippayout_bankbranch_id", "");
                    string moneytype = DwMain.GetItemString(1, "moneytype_code");
                    if (moneytype != "TRN")
                    {
                        string tofrom_accid = Fin.DefaultAccId(state.SsWsPass, moneytype);
                        DwMain.SetItemString(1, "tofrom_accid", tofrom_accid);
                        DwMain.SetItemString(1, "to_system", "");
                    }
                    else
                    {
                        DwMain.SetItemString(1, "to_system", "DEP");
                        DwMain.SetItemString(1, "tofrom_accid", "");
                    }
                    break;
                case "postbank_code":
                    DwMain.SetItemString(1, "asnslippayout_bankbranch_id", "");
                    break;
            }
        }

        public void SaveWebSheet()
        {
            try
            {
                string toperate_date = DwMain.GetItemString(1, "operate_tdate");
                if (toperate_date.Length == 8)
                {
                    DateTime dt = DateTime.ParseExact(toperate_date, "ddMMyyyy", WebUtil.TH);
                    DwMain.SetItemDateTime(1, "operate_date", dt);
                }
            }
            catch { }
            try
            {
                string member_no = DwMem.GetItemString(1, "member_no");

                try
                {
                    string sql = "select pay_status, deptaccount_no from asnreqmaster where member_no = '" + member_no + "' and assisttype_code = '70'";
                    Sdt dt = WebUtil.QuerySdt(sql);
                    if (dt.Next())
                    {
                        decimal pay_status = dt.GetDecimal("pay_status");
                        DwMain.SetItemString(1, "deptaccount_no", dt.GetString("deptaccount_no"));
                        if (pay_status == 1)
                        {
                            LtServerMessage.Text = WebUtil.ErrorMessage("เลขทะเบียน " + member_no + "ได้ทำการจ่ายเงินช่วยเหลือไปแล้ว");
                            return;
                        }
                    }
                }
                catch
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถเช็คค่าการจ่ายเงินได้");
                    return;
                }
                string moneyType = "", tofrom_accid = "";
                DateTime operate_date = DateTime.MinValue;
                try
                {
                    moneyType = DwMain.GetItemString(1, "moneytype_code");
                    tofrom_accid = DwMain.GetItemString(1, "tofrom_accid");
                }
                catch
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("กรุณาระบุประการจ่ายเงิน และคู่บัญชี");
                    return;
                }
                try
                {
                    operate_date = DwMain.GetItemDateTime(1, "operate_date");
                }
                catch 
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("กรุณาระบุวันทำรายการ");
                    return;
                }
                string deptaccount_no = "";
                try
                {
                    deptaccount_no = DwMain.GetItemString(1, "deptaccount_no");
                }
                catch
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("เกิดข้อผิดพลาด account_no");
                    return;
                }
                DateTime stamentdate = DateTime.Today;
                double payout = DwDetail.GetItemDouble(1, "assist_amt");
                string sliptype_code = "AIZ";
                if (payout != 0)
                {
                    payout = DwDetail.GetItemDouble(1, "assist_amt");
                }

                string bankCode = "", bankBranch = "", bankAcc = "", to_system = "";
                if (moneyType == "CBT")
                {
                    try
                    {
                        bankCode = DwMain.GetItemString(1, "asnslippayout_bank_code");
                        bankBranch = DwMain.GetItemString(1, "asnslippayout_bankbranch_id");
                        bankAcc = DwMain.GetItemString(1, "asnslippayout_bank_accid");
                    }
                    catch
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage("กรูณากรอกข้อมูลการโอนเงินธนาคารให้ครบ");
                        return;
                    }
                }

                if (moneyType == "TRN")
                {
                    try { to_system = DwMain.GetItemString(1, "to_system"); }
                    catch
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage("กรูณากรอกข้อมูลการโอนภายในให้ครบ");
                        return;
                    }
                }

                /*
                string slippayout_no = NewDocumentNo("ASSISTDOCNO70", DateTime.Today.Year + 543, ta, true);

                if (moneyType == "TRN")
                {
                    string sqlupdateasn = @"update asnreqmaster set moneytype_code = '" + moneyType + "' , to_system = 'DIV', assist_amt = " + payout + ", pay_status = 1 where member_no = '" + member_no + "' and assisttype_code = '70'";
                    ta.Exe(sqlupdateasn);

                    string sqlsavestate = @"insert into asnslippayout (payoutslip_no,member_no,slip_date,operate_date,payout_amt,slip_status,assisttype_code,entry_id,entry_date,moneytype_code, sliptype_code, tofrom_accid)" +
                        "values('" + slippayout_no + "','" + member_no + "',to_date('" + DateTime.Now.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy'),to_date('" + DateTime.Now.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy')," +
                     +payout + ",1,'70','" + state.SsUsername + "',to_date('" + DateTime.Now.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy'),'" + moneyType + "', '" + sliptype_code + "', '" + tofrom_accid + "')";
                    ta.Exe(sqlsavestate);
                }
                else if (moneyType == "CBT")
                {
                    string bankCode = DwMain.GetItemString(1, "asnslippayout_bank_code");
                    string bankBranch = DwMain.GetItemString(1, "asnslippayout_bankbranch_id");
                    string bankAcc = DwMain.GetItemString(1, "asnslippayout_bank_accid");

                    string sqlupdateasn = @"update asnreqmaster set moneytype_code = '" + moneyType + "', assist_amt = " + payout + ", pay_status = 1 where member_no = '" + member_no + "' and assisttype_code = '70'";
                    ta.Exe(sqlupdateasn);

                    string sqlsavestate = @"insert into asnslippayout (payoutslip_no,member_no,slip_date,operate_date,payout_amt,slip_status,assisttype_code,entry_id,entry_date,moneytype_code,bank_code,bankbranch_id,bank_accid, sliptype_code, tofrom_accid)" +
                        "values('" + slippayout_no + "','" + member_no + "',to_date('" + DateTime.Now.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy'),to_date('" + DateTime.Now.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy')," +
                     +payout + ",1,'70','" + state.SsUsername + "',to_date('" + DateTime.Now.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy'),'" + moneyType + "','" + bankCode + "','" + bankBranch + "','" + bankAcc + "', '" + sliptype_code + "', '" + tofrom_accid + "')";
                    ta.Exe(sqlsavestate);
                }
                else
                {
                    string sqlupdateasn = @"update asnreqmaster set moneytype_code = '" + moneyType + "', assist_amt = " + payout + ", pay_status = 1 where member_no = '" + member_no + "' and assisttype_code = '70'";
                    ta.Exe(sqlupdateasn);

                    string sqlsavestate = @"insert into asnslippayout (payoutslip_no,member_no,slip_date,operate_date,payout_amt,slip_status,assisttype_code,entry_id,entry_date,moneytype_code,sliptype_code, tofrom_accid)" +
                        "values('" + slippayout_no + "','" + member_no + "',to_date('" + DateTime.Now.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy'),to_date('" + DateTime.Now.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy')," +
                     +payout + ",1,'70','" + state.SsUsername + "',to_date('" + DateTime.Now.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy'),'" + moneyType + "',  '" + sliptype_code + "', '" + tofrom_accid + "')";
                    ta.Exe(sqlsavestate);
                }
                 */
                bool result = AsnService.SaveMoneyHelp(state.SsWsPass, moneyType, payout, member_no, tofrom_accid, state.SsUsername, sliptype_code, bankCode, bankBranch, bankAcc, state.SsWorkDate,operate_date, deptaccount_no, to_system);
                if (result)
                {
                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
                }
                else
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("เกิดข้อผิดพลาด บันทึกไม่สำเร็จ");
                }
            }
            catch (SoapException ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(WebUtil.SoapMessage(ex));
            }
        }

        public void WebSheetLoadEnd()
        {
            try
            {
                DwUtil.RetrieveDDDW(DwMain, "moneytype_code", pbl, null);
                string moneytype_code = DwUtil.GetString(DwMain, 1, "moneytype_code", "");
                if (moneytype_code != "")
                {
                    DwUtil.RetrieveDDDW(DwMain, "asnslippayout_bank_code", pbl, moneytype_code);
                    string bank_code = DwUtil.GetString(DwMain, 1, "asnslippayout_bank_code", "");
                    if (bank_code != "")
                    {
                        DwUtil.RetrieveDDDW(DwMain, "asnslippayout_bankbranch_id", pbl, bank_code);
                        DataWindowChild dc = DwMain.GetChild("asnslippayout_bankbranch_id");
                        dc.SetFilter("bank_code ='" + bank_code + "'");
                        dc.Filter();
                    }
                    DwUtil.RetrieveDDDW(DwMain, "tofrom_accid", pbl, moneytype_code);
                }
            }
            catch { }
            DwMem.SaveDataCache();
            DwMain.SaveDataCache();
            DwDetail.SaveDataCache();
            DwColl.SaveDataCache();
            tDwMain.Eng2ThaiAllRow();
        }

        private void InintMemb()
        {
            try
            {
                string member_no = "";
                try
                {
                    member_no = WebUtil.MemberNoFormat(DwMem.GetItemString(1, "member_no"));
                }
                catch
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("สมาชิกคนนี้ไม่อยู่ในกองทุน");
                }
                DwUtil.RetrieveDataWindow(DwMem, pbl, null, member_no);

                DateTime member_date = DwMem.GetItemDateTime(1, "member_date");
                DwUtil.RetrieveDataWindow(DwColl, pbl, null, member_no);
                try
                {
                    DwColl.GetItemString(1, "ref_collno");
                }
                catch { DwColl.DeleteRow(0); }
                DwMain.SetItemDate(1, "member_date", member_date);
                double loan_bal = 0;
                try
                {
                    loan_bal = DwColl.GetItemDouble(1, "principal_balance");
                    DwDetail.SetItemDouble(1, "loan_amt", loan_bal);
                }
                catch
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ผู้กู้เลขสมาชิกสหกรณ์ " + member_no + " ได้ชำระหนี้หมดแล้ว");
                    DwMem.Reset();
                    DwMem.InsertRow(0);
                    DwMain.Reset();
                    DwMain.InsertRow(0);
                    DwDetail.Reset();
                    DwDetail.InsertRow(0);
                    return;
                }

                string memb_date = DwMain.GetItemDateTime(1, "member_date").ToString("yyyy");
                int memb_age = DateTime.Today.Year - (Convert.ToInt32(memb_date));
                //--------------------------------------------------------------------------------------
                int Dmemb_date = Convert.ToInt32(DwMem.GetItemDateTime(1, "member_date").ToString("dd"));
                int Mmemb_date = Convert.ToInt32(DwMem.GetItemDateTime(1, "member_date").ToString("MM"));
                int Ymemb_date = Convert.ToInt32(DwMem.GetItemDateTime(1, "member_date").ToString("yyyy"));
                if (DateTime.Today.Month < Mmemb_date)
                {
                    memb_age = memb_age - 1;
                }
                else if (DateTime.Today.Month == Mmemb_date)
                {
                    if (DateTime.Today.Day < Dmemb_date)
                    {
                        memb_age = memb_age - 1;
                    }
                }
                DwMain.SetItemString(1, "memb_age", (memb_age + " ปี"));
                int agememb = 0;
                try
                {
                    string birth_date = DwMem.GetItemDateTime(1, "birth_date").ToString("yyyy");
                    agememb = DateTime.Today.Year - (Convert.ToInt32(birth_date));
                    //---------------------------------------------------------------------------
                    int Dbirth_date = Convert.ToInt32(DwMem.GetItemDateTime(1, "birth_date").ToString("dd"));
                    int Mbirth_date = Convert.ToInt32(DwMem.GetItemDateTime(1, "birth_date").ToString("MM"));
                    int Ybirth_date = Convert.ToInt32(DwMem.GetItemDateTime(1, "birth_date").ToString("yyyy"));                   
                    if (DateTime.Today.Month < Mbirth_date)
                    {
                        agememb = agememb - 1;
                    }
                    else if (DateTime.Today.Month == Mbirth_date)
                    {
                        if (DateTime.Today.Day < Dbirth_date)
                        {
                            agememb = agememb - 1;
                        }
                    }
                }
                catch { }
                string sqlStr = @"select helpcoll_amt, helpcoll_persent_pay
                           from asnucfpayktdead
                           WHERE minmemb_year <= " + memb_age + " AND maxmemb_year >= " + memb_age;
                Sdt dt = WebUtil.QuerySdt(sqlStr);
                if (dt.Next())
                {
                    double amtPersent = dt.GetDouble("helpcoll_persent_pay");
                    double helpcoll_amt = dt.GetDouble("helpcoll_amt");
                    double assist_amt = loan_bal * amtPersent;
                    DwDetail.SetItemDouble(1, "help_amt", assist_amt);
                    if (assist_amt > helpcoll_amt)
                    {
                        assist_amt = helpcoll_amt;                        
                    }
                    DwDetail.SetItemDouble(1, "maxhelp_amt", helpcoll_amt);
                    DwDetail.SetItemDouble(1, "assist_amt", assist_amt);
                    DwMain.SetItemDouble(1, "assist_amt", assist_amt);
                    DwDetail.SetItemDouble(1, "persent_amt", amtPersent);

                    //double amt = DwDetail.GetItemDouble(1, "assist_amt");
                    //double prncbal = (amt * amtPersent);
                    //DwDetail.SetItemDouble(1, "assist_amt", prncbal);
                    
                }
            }
            catch (SoapException ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถทำรายการได้" + WebUtil.SoapMessage(ex));
                DwMem.Reset();
                DwMem.InsertRow(0);
                DwMain.Reset();
                DwMain.InsertRow(0);
                DwDetail.Reset();
                DwDetail.InsertRow(0);
            }

        }
    }
}