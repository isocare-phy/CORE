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

    public partial class w_sheet_kt_65years_dead : PageWebSheet, WebSheet
    {
        TimeSpan tp;
        protected Sta ta;
        protected Sdt dt;
        protected DwThDate tDwDetail;
        protected String postDeadDate;
        protected String postPayClick;
        protected String postInsertRow;
        protected String postBankAccid;
        protected String postDeleteRow;
        protected String postMoneyType;
        protected String postFilterBranch;
        protected String postRetreiveDwMem;
        protected String postRetrieveDwMain;
        protected String postGetMemberDetail;
        protected String postRetrieveBankBranch;

        public void InitJsPostBack()
        {
            postRetreiveDwMem = WebUtil.JsPostBack(this, "postRetreiveDwMem");
            postRetrieveDwMain = WebUtil.JsPostBack(this, "postRetrieveDwMain");
            postGetMemberDetail = WebUtil.JsPostBack(this, "postGetMemberDetail");
            postRetrieveBankBranch = WebUtil.JsPostBack(this, "postRetrieveBankBranch");
            postMoneyType = WebUtil.JsPostBack(this, "postMoneyType");
            postFilterBranch = WebUtil.JsPostBack(this, "postFilterBranch");
            postDeadDate = WebUtil.JsPostBack(this, "postDeadDate");
            postInsertRow = WebUtil.JsPostBack(this, "postInsertRow");
            postDeleteRow = WebUtil.JsPostBack(this, "postDeleteRow");
            postPayClick = WebUtil.JsPostBack(this, "postPayClick");
            postBankAccid = WebUtil.JsPostBack(this, "postBankAccid");

            tDwDetail = new DwThDate(DwDetailP, this);
            tDwDetail.Add("member_dead_date", "member_dead_tdate");
        }

        public void WebSheetLoadBegin()
        {
            tDwDetail.Eng2ThaiAllRow();
            this.ConnectSQLCA();
            ta = new Sta(sqlca.ConnectionString);
            dt = new Sdt();
            if (!IsPostBack)
            {
                DwMemP.Reset();
                DwDetailP.Reset();
                DwMainP.Reset();
                DwMainP.InsertRow(0);
                DwMemP.InsertRow(0);
                DwDetailP.InsertRow(0);
            }
            else
            {
                this.RestoreContextDw(DwMemP);
                this.RestoreContextDw(DwMainP);
                this.RestoreContextDw(DwDetailP);
                this.RestoreContextDw(DwReceive);
            }
            DwUtil.RetrieveDDDW(DwMainP, "paytype_code", "kt_65years.pbl", null);
            DwUtil.RetrieveDDDW(DwMainP, "moneytype_code", "kt_65years.pbl", null);
            DwUtil.RetrieveDDDW(DwMainP, "asnslippayout_bank_code", "kt_65years.pbl", null);
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postRetreiveDwMem")
            {
                RetreiveDwMem();
            }
            else if (eventArg == "postDeadDate")
            {
                ChangDeadDate();
            }
            else if (eventArg == "postRetrieveDwMain")
            {
                //RetrieveDwMain();
            }
            else if (eventArg == "postRetrieveBankBranch")
            {
                //RetrieveBankBranch();
            }
            else if (eventArg == "postGetMemberDetail")
            {
                GetMemberMain();
            }
            else if (eventArg == "postMoneyType")
            {
                MoneyType();
            }
            else if (eventArg == "postFilterBranch")
            {
                JspostFilterBranch();
            }
            else if (eventArg == "postInsertRow")
            {
                JsPostInsertRow();
            }
            else if (eventArg == "postDeleteRow")
            {
                JsPostDeleteRow();
            }
            else if (eventArg == "postPayClick")
            {
                JspostPayClick();
            }
            else if (eventArg == "postBankAccid")
            {
                JspostBankAccid();
            }

        }

        public void SaveWebSheet()
        {
            string Assist_docno = "", remark = "", deadcase = "", member_dead_tdate = "";
            string moneytype_code, bank_code, bankbranch_id, bank_accid, pay_status = "";
            int capital_year = DateTime.Now.Year + 543;
            DateTime member_dead_date = DateTime.Now;
            decimal perpay = 0, assist_amt = 0, sumpays = 0, balance = 0, member_age = 0, approve_amt = 0;

            string checkAssist = "select assist_docno from asnreqmaster where member_no = '" + HdMemberNo.Value + "' and assist_docno like 'SF%'";
            DataTable ck = WebUtil.Query(checkAssist);
            try
            {
                Assist_docno = ck.Rows[0][0].ToString().Trim();
            }
            catch
            {
                SaveSF();
                checkAssist = "select assist_docno from asnreqmaster where member_no = '" + HdMemberNo.Value + "' and assist_docno like 'SF%'";
                ck = WebUtil.Query(checkAssist);
                Assist_docno = ck.Rows[0][0].ToString().Trim();
            }
            SaveReceive();
            try
            {
                moneytype_code = DwMainP.GetItemString(1, "moneytype_code");
            }
            catch
            { moneytype_code = "CSH"; }
            if (moneytype_code == "CBT")
            {
                bank_code = DwMainP.GetItemString(1, "asnslippayout_bank_code");
                bankbranch_id = DwMainP.GetItemString(1, "asnslippayout_bankbranch_id");
                bank_accid = DwMainP.GetItemString(1, "asnslippayout_bank_accid");
            }
            try
            {
                perpay = DwDetailP.GetItemDecimal(1, "perpay");
            }
            catch { }
            try
            {
                assist_amt = DwDetailP.GetItemDecimal(1, "assist_amt");
            }
            catch { }
            try
            {
                approve_amt = DwDetailP.GetItemDecimal(1, "approve_amt");
            }
            catch { }
            try
            {
                balance = DwDetailP.GetItemDecimal(1, "balance");
                //balance = Convert.ToDecimal(HdPayout.Value);
            }
            catch { }
            try
            {
                sumpays = assist_amt - balance;//DwDetailP.GetItemDecimal(1, "sumpays");
            }
            catch { }
            try
            {
                member_age = DwDetailP.GetItemDecimal(1, "member_age");
            }
            catch { }
            try
            {
                member_dead_tdate = DwDetailP.GetItemString(1, "member_dead_tdate");
                member_dead_date = DateTime.ParseExact(member_dead_tdate, "ddMMyyyy", WebUtil.TH);
            }
            catch { }

            try
            {
                deadcase = DwDetailP.GetItemString(1, "member_dead_case");
            }
            catch { deadcase = ""; }
            try
            {
                remark = DwDetailP.GetItemString(1, "remark");
            }
            catch { remark = ""; }
            //if (deadcase != "" || balance == 0)
            //{
            pay_status = "-9";
            //}
            try
            {
                //sumpays = sumpays + balance;
                String SQLUpdateASN = "update asnreqmaster set perpay = '" + perpay + "' , sumpays ='" + sumpays + "' , balance ='" + balance + "' , assist_amt ='" + assist_amt + "',pay_date = to_date('" + DateTime.Now.ToString("ddMMyyyy", WebUtil.EN) + "','ddmmyyyy'),pay_status= " + pay_status + ",dead_date = to_date('" + member_dead_date.ToString("ddMMyyyy", WebUtil.EN) + "','ddmmyyyy'), dead_status='1' , calculate_date = to_date('" + member_dead_date.ToString("ddMMyyyy", WebUtil.EN) + "','ddmmyyyy') , approve_amt = " + approve_amt + "  where assist_docno = '" + Assist_docno + "'";
                WebUtil.Query(SQLUpdateASN);
                //SaveState(sumpays);
                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("บันทึกไม่สำเร็จ" + ex);
            }
            //if (balance == 0)
            //{
            //    try
            //    {
            //        //String SQLInsertReqCap = "insert into asnreqcapitaldet(capital_year,assist_docno,member_dead_date,member_age,member_dead_case,remark,assist_amt) values ('" + capital_year + "','" + Assist_docno + "',to_date('" + member_dead_date.ToString("ddMMyyyy", WebUtil.EN) + "','ddmmyyyy'),'" + member_age + "','" + deadcase + "','" + remark + "'," + assist_amt + ")";
            //        //WebUtil.Query(SQLInsertReqCap);

            //}       //String SQLInsertSlip = "insert into asnslippayout(payoutslip_no,member_no,slip_date,operate_date,payout_amt,slip_status,assisttype_code,moneytype_code,bank_code,bankbranch_id,bank_accid,entry_id,entry_date) "+
            //        //                                            "values ('','" + HdMemberNo.Value + "',to_date('" + DateTime.Now.ToString("ddMMyyyy", WebUtil.EN) + "','ddmmyyyy'),"+HdPayout.Value+",'1','90','','','','','','','')";
            //        //SaveState();
            //    }
            //    catch
            //    {
            //        LtServerMessage.Text = WebUtil.ErrorMessage("หมายเลขสมาชิกนี้มีการกดจ่าย กรณีเสียชีวิตแล้ว");
            //    }
            //else
            //{
            //    LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากดจ่ายเงิน");
            //}

        }

        private void SaveState(decimal sumpays)
        {
            string entry_id = state.SsUsername;
            decimal payout = 0;
            string payout_no = "";
            int yearnow = DateTime.Now.Year + 543;

            string moneyType = HdMoneyType.Value;//DwMainP.GetItemString(1, "moneytype_code");
            string member_no = HdMemberNo.Value;
            //DateTime stamentdate = DwMainP.GetItemDateTime(1, "entry_date");
            try
            {
                payout = sumpays;//Convert.ToDecimal(HdPayout.Value);//DwDetailP.GetItemDecimal(1, "");
            }
            catch
            {

            }

            string sqlQu = "select max(payoutslip_no) from asnslippayout where payoutslip_no like '" + yearnow.ToString().Substring(2, 2) + "90" + "%'";
            Sdt dtSlip = WebUtil.QuerySdt(sqlQu);
            if (dtSlip.Next())
            {
                payout_no = dtSlip.GetString("max(payoutslip_no)");//slip_no.Rows[0][0].ToString();
                if (payout_no != "")
                {
                    payout_no = payout_no.Substring(4, 6);
                }
                else { payout_no = "00000"; }
            }
            Decimal ass = Convert.ToDecimal(payout_no);
            ass++;
            string slippayout_no = yearnow.ToString().Substring(2, 2) + "90" + ass.ToString("00000");

            if (moneyType == "TRN")
            {
                string bankAcc = DwMainP.GetItemString(1, "asnslippayout_bank_accid");
                string sqlupdateasn = @"update asnreqmaster set moneytype_code = '" + moneyType + "' , to_system = 'DIV' where member_no = '" + member_no + "' and assisttype_code = '90'";
                ta.Exe(sqlupdateasn);

                string sqlsavestate = @"insert into asnslippayout (payoutslip_no,member_no,slip_date,operate_date,payout_amt,slip_status,assisttype_code,entry_id,entry_date,moneytype_code ,post_fin,bank_accid)" +
                    "values('" + slippayout_no + "','" + member_no + "',to_date('" + DateTime.Now.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy'),to_date('" + DateTime.Now.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy')," +
                 +payout + ",1,'" + "90" + "','" + entry_id + "',to_date('" + DateTime.Now.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy'),'" + moneyType + "','8','" + bankAcc + "')";
                ta.Exe(sqlsavestate);
            }
            else if (moneyType == "CBT")
            {
                string bankCode = DwMainP.GetItemString(1, "asnslippayout_bank_code");
                string bankBranch = DwMainP.GetItemString(1, "asnslippayout_bankbranch_id");
                string bankAcc = DwMainP.GetItemString(1, "asnslippayout_bank_accid");

                string sqlupdateasn = @"update asnreqmaster set moneytype_code = '" + moneyType + "' where member_no = '" + member_no + "' and assisttype_code = '90'";
                ta.Exe(sqlupdateasn);

                string sqlsavestate = @"insert into asnslippayout (payoutslip_no,member_no,slip_date,operate_date,payout_amt,slip_status,assisttype_code,entry_id,entry_date,moneytype_code,bank_code,bankbranch_id,bank_accid,post_fin)" +
                    "values('" + slippayout_no + "','" + member_no + "',to_date('" + DateTime.Now.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy'),to_date('" + DateTime.Now.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy')," +
                 +payout + ",1,'" + "90" + "','" + entry_id + "',to_date('" + DateTime.Now.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy'),'" + moneyType + "','" + bankCode + "','" + bankBranch + "','" + bankAcc + "','8')";
                ta.Exe(sqlsavestate);
            }
            else
            {
                string sqlupdateasn = @"update asnreqmaster set moneytype_code = '" + moneyType + "' where member_no = '" + member_no + "' and assisttype_code = '90'";
                ta.Exe(sqlupdateasn);

                string sqlsavestate = @"insert into asnslippayout (payoutslip_no,member_no,slip_date,operate_date,payout_amt,slip_status,assisttype_code,entry_id,entry_date,moneytype_code,post_fin)" +
                    "values('" + slippayout_no + "','" + member_no + "',to_date('" + DateTime.Now.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy'),to_date('" + DateTime.Now.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy')," +
                 +payout + ",1,'" + "90" + "','" + entry_id + "',to_date('" + DateTime.Now.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy'),'" + moneyType + "','8')";
                ta.Exe(sqlsavestate);
            }
            LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลสำเร็จ");

        }

        public void WebSheetLoadEnd()
        {
            DwMemP.SaveDataCache();
            DwMainP.SaveDataCache();
            DwDetailP.SaveDataCache();
            DwReceive.SaveDataCache();

            dt.Clear();
            ta.Close();

        }

        private void RetreiveDwMem()
        {
            DwDetailP.Reset();
            DwDetailP.InsertRow(0);
            DwReceive.Reset();
            String member_no;
            Decimal deadpay_amt = 0, sumshare = 0;

            member_no = HdMemberNo.Value;

            try
            {
                // เพิ่มค่าในช่องหุ้น
                //string sqlStr = "select sharestk_amt from shsharemaster where member_no = '" + member_no + "'";
                string sqlStr = @"select max(operate_date) ,min(operate_date)  
                        from shsharestatement
                        where shritemtype_code = 'SPM' and member_no = '" + member_no + "'";
                dt = ta.Query(sqlStr);
                if (dt.Next())
                {
                    //deadpay_amt = dt.GetDecimal("sumshare");
                    //DwMainP.SetItemDecimal(1, "sharestk_amt", deadpay_amt);
                    HdMaxOperatedate.Value = dt.GetDateTh("max(operate_date)");
                    HdMinOperatedate.Value = dt.GetDateTh("min(operate_date)");
                }
                else { DwMainP.SetItemDecimal(1, "sharestk_amt", 0); }
            }
            catch
            {

            }

            //DwMemP.Reset();
            DwUtil.RetrieveDataWindow(DwMemP, "kt_65years.pbl", null, member_no);
            if (DwMemP.RowCount < 1)
            {
                LtAlert.Text = "<script>Alert()</script>";
                return;
            }
            GetMemberMain();


        } //1

        private void GetMemberMain()
        {
            string member_no = HdMemberNo.Value;
            string member_tdate, work_tdate, req_tdate, dead_tdate, dead_status;
            DateTime member_date, work_date;
            try // Set วันที่เข้าเป็นสมาชิก
            {
                string SQLMemberDate = "select member_date,resign_date , birth_date from mbmembmaster where member_no = '" + member_no + "'";
                Sdt dt = WebUtil.QuerySdt(SQLMemberDate);
                string SQLGetAsnreqmaster = "select req_date,dead_date , dead_status from asnreqmaster where member_no = '" + HdMemberNo.Value + "' and assisttype_code = '90'";
                Sdt dtDate = WebUtil.QuerySdt(SQLGetAsnreqmaster);
                if (dt.Next())
                {
                    try
                    {
                        dtDate.Next();
                        try
                        {
                            req_tdate = dtDate.GetDateTh("req_date");
                            if (req_tdate == "01/01/1913")
                                req_tdate = state.SsWorkDate.ToString("dd/MM/yyyy", WebUtil.TH);//DateTime.Now.Date.ToString("dd/MM/yyyy", WebUtil.TH).Substring(0, 10).Trim();
                            DwMainP.SetItemString(1, "req_tdate", req_tdate);
                        }
                        catch
                        {

                        }
                        dead_status = dtDate.GetString("dead_status");
                        dead_tdate = dtDate.GetDateTh("dead_date");
                        if (dead_status == "1")
                            LtServerMessage.Text = WebUtil.WarningMessage("หมายเลขสมาชิกนี้ ตายเมื่อวันที่ " + dead_tdate);
                        HdDeadDate.Value = dead_tdate.Replace("/", "");
                        if (dead_tdate.Replace("/", "").ToString() == "01011913")
                            DwDetailP.SetItemString(1, "member_dead_tdate", "00000000");
                        else
                            DwDetailP.SetItemString(1, "member_dead_tdate", dead_tdate.Replace("/", ""));
                        ChangDeadDate();
                    }
                    catch { }
                    member_tdate = dt.GetDateTh("member_date");
                    HdBirthDate.Value = dt.GetDateTh("birth_date");
                    HdDeadDate.Value = dt.GetDateTh("resign_date");
                    DwMainP.SetItemString(1, "member_tdate", member_tdate);
                    DwMainP.SetItemString(1, "birth_tdate", HdBirthDate.Value);
                }
            }
            catch { }
            try // Set วันที่ทำการ
            {
                work_tdate = state.SsWorkDate.ToString("dd/MM/yyyy", WebUtil.TH); ;//DateTime.Now.Date.ToString("dd/MM/yyyy", WebUtil.TH).Substring(0, 10).Trim();

                DwMainP.SetItemString(1, "entry_tdate", work_tdate);
                DwDetailP.SetItemString(1, "calculate_tdate", work_tdate.Replace("/", ""));
            }
            catch { }

            // Set ประเภทกองทุน
            DwMainP.SetItemString(1, "assisttype_code", "กองทุนสวัสดิการ 65 ปีมีสุข");
            GetMemberDetail();
        } //2

        private void GetMemberDetail()
        {
            decimal sumpays = 0, perpay = 0, approve_amt = 0, balance = 0, assist_amt = 0, share = 0, cal_balance = 0, cal_perpay = 0;
            try
            {
                string SQLGetAsnreqmaster = "select * from asnreqmaster where member_no = '" + HdMemberNo.Value + "' and assisttype_code = '90'";
                Sdt dtAsn = WebUtil.QuerySdt(SQLGetAsnreqmaster);

                if (dtAsn.Next())
                {
                    sumpays = dtAsn.GetDecimal("sumpays");
                    perpay = dtAsn.GetDecimal("perpay");
                    approve_amt = dtAsn.GetDecimal("approve_amt");
                    balance = dtAsn.GetDecimal("balance");
                    assist_amt = dtAsn.GetDecimal("assist_amt");

                    DwDetailP.SetItemDecimal(1, "sumpays", sumpays);
                    DwDetailP.SetItemDecimal(1, "perpay", perpay);
                    DwDetailP.SetItemDecimal(1, "approve_amt", approve_amt);
                    DwDetailP.SetItemDecimal(1, "balance", balance);
                    DwDetailP.SetItemDecimal(1, "assist_amt", assist_amt);
                    HdPayout.Value = balance.ToString();
                }
                else
                {
                    DwDetailP.SetItemDecimal(1, "sumpays", 0);
                    DwDetailP.SetItemDecimal(1, "perpay", 0);
                    DwDetailP.SetItemDecimal(1, "approve_amt", 0);
                    DwDetailP.SetItemDecimal(1, "balance", 0);
                    DwDetailP.SetItemDecimal(1, "assist_amt", 0);
                }
                string SQLSelectShare = "select sum(sumyear)as sumyear ,sum(cal_amt)as calamt from asnsumshare where member_no = '" + HdMemberNo.Value + "' ";
                Sdt dtShare = WebUtil.QuerySdt(SQLSelectShare);
                if (dtShare.Next())
                {

                    assist_amt = dtShare.GetDecimal("calamt");
                    if (assist_amt > 500000)
                        assist_amt = 500000;
                    assist_amt = Math.Floor(assist_amt); //ปัดเศษที่คำนวนได้ทิ้ง
                    share = dtShare.GetDecimal("sumyear");
                    DwMainP.SetItemDecimal(1, "sharestk_amt", share);
                    DwDetailP.SetItemDecimal(1, "assist_amt", assist_amt);
                    cal_balance = assist_amt - sumpays;
                    if (cal_balance < 0)
                        cal_balance = 0;
                    DwDetailP.SetItemDecimal(1, "balance", cal_balance);
                    cal_perpay = assist_amt / 10;
                    if (cal_perpay > 10000)
                        cal_perpay = 10000;
                    if (cal_perpay % 10 != 0)
                    {
                        cal_perpay = cal_perpay + (10 - (cal_perpay % 10));//ปัดเศษ
                    }
                    DwDetailP.SetItemDecimal(1, "perpay", cal_perpay);

                    if (DwDetailP.GetItemDecimal(1, "approve_amt") == 0)
                    {
                        DwDetailP.SetItemDecimal(1, "approve_amt", assist_amt);
                    }
                }
            }
            catch { }
        } //3

        private void MoneyType()
        {
            HdMoneyType.Value = DwMainP.GetItemString(1, "moneytype_code");
        }

        private void JspostFilterBranch()
        {
            try
            {
                String bankcode = DwMainP.GetItemString(1, "asnslippayout_bank_code");
                DwUtil.RetrieveDDDW(DwMainP, "asnslippayout_bankbranch_id", "kt_65years.pbl", null);
                DataWindowChild dc = DwMainP.GetChild("asnslippayout_bankbranch_id");
                dc.SetFilter("bank_code ='" + bankcode + "'");
                dc.Filter();
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        private void ChangDeadDate()
        {
            try
            {
                DateTime dead_date, birth_date;
                string birth_tdate = HdBirthDate.Value;
                string temp;
                //string member_dead_tdate = DwDetailP.GetItemString(1, "member_dead_tdate");
                HdDeadDate.Value = DwDetailP.GetItemString(1, "member_dead_tdate");
                DwDetailP.SetItemString(1, "calculate_tdate", HdDeadDate.Value);
                temp = HdDeadDate.Value;
                temp = temp.Replace("/", "");
                temp = temp.Insert(2, "/");
                temp = temp.Insert(5, "/");
                HdDeadDate.Value = temp;

                birth_date = DateTime.ParseExact(birth_tdate, "dd/MM/yyyy", WebUtil.TH);
                dead_date = DateTime.ParseExact(HdDeadDate.Value, "dd/MM/yyyy", WebUtil.TH);

                decimal Age = (dead_date.Year - birth_date.Year);

                DwDetailP.SetItemDecimal(1, "member_age", Age);

                CalSumShare();
                cal_last();
                cut_reduce();
                GetMemberDetail();

            }
            catch
            {
            }
        } //4

        private void JsPostInsertRow()
        {
            DwReceive.InsertRow(0);
        }

        private void JsPostDeleteRow()
        {
            try
            {
                string assist_no = DwReceive.GetItemString(Convert.ToInt32(HdDwRow.Value), "assist_docno");
                string rece_no = DwReceive.GetItemString(Convert.ToInt32(HdDwRow.Value), "receive_no");
                string sql = "delete asnmembreceive where assist_docno = '" + assist_no + "' and receive_no ='" + rece_no + "'";
                ta.Exe(sql);
                DwReceive.DeleteRow(int.Parse(HdDwRow.Value));

            }
            catch
            {
                DwReceive.DeleteRow(int.Parse(HdDwRow.Value));
            }
        }

        private void JspostPayClick()
        {
            decimal sumpays = DwDetailP.GetItemDecimal(1, "sumpays");
            decimal balance = DwDetailP.GetItemDecimal(1, "balance");
            HdPayout.Value = balance.ToString();
            sumpays = sumpays + balance;

            DwDetailP.SetItemDecimal(1, "sumpays", sumpays);
            DwDetailP.SetItemDecimal(1, "balance", 0);
        }

        private void SaveSF()
        {
            string ass_docno = "";
            //string ck = "";
            string memNo = DwUtil.GetString(DwMemP, 1, "member_no");
            try
            {
                string qurryass = "select max(assist_docno) from asnreqmaster where assisttype_code = '90' and assist_docno like 'SF%' and capital_year = '" + (state.SsWorkDate.Year + 543) + "'";
                DataTable qass = WebUtil.Query(qurryass);
                if (qass.Rows.Count > 0)
                {
                    ass_docno = qass.Rows[0][0].ToString();
                    if (ass_docno != "")
                    {
                        ass_docno = ass_docno.Substring(4, 5);
                    }
                    else { ass_docno = "00000"; }
                }
            }
            catch
            {
                ass_docno = "00000";
            }

            String yyyynow = (state.SsWorkDate.Year + 543).ToString();
            Decimal ass = Convert.ToDecimal(ass_docno);
            ass++;
            string AssAmt = "SF" + yyyynow.Substring(2, 2) + ass.ToString("00000");
            try
            {
                string savesql = "insert into asnreqmaster (assist_docno,capital_year,member_no,assisttype_code,req_date,approve_date) values ('" + AssAmt + "'," + yyyynow + ",'" + memNo + "','90',to_date('" + DateTime.Now.ToString("ddMMyyyy", WebUtil.EN) + "','ddmmyyyy'),to_date('" + DateTime.Now.ToString("ddMMyyyy", WebUtil.EN) + "','ddmmyyyy'))";
                DataTable savekt = WebUtil.Query(savesql);

            }

            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถบันทึกข้อมูล ในกองทุนได้" + ex);
            }

        }

        private void SaveReceive()
        {
            if (HdRowReceive.Value == "เพิ่ม")
            {

                int rowww = DwReceive.RowCount + 1;
                string sqlcount = "select count(receive_no) from asnmembreceive where assist_docno = (select assist_docno from asnreqmaster  where member_no ='" + HdMemberNo.Value + "' and assist_docno like 'SF%') ";
                dt = ta.Query(sqlcount);
                dt.Next();
                int count = Convert.ToInt32(dt.GetDecimal("count(receive_no)"));
                int row2 = rowww - 1 - count;
                ////เพิ่ม 
                int rowcount = 1;
                string assist_docnoo = "";
                if (count != 0)
                {
                    string sqlrow = "select receive_no,assist_docno from asnmembreceive where receive_no =" +
                                    " (select max(receive_no) from asnmembreceive where " +
                                    " assist_docno = (select assist_docno from asnreqmaster  where member_no ='" + HdMemberNo.Value + "' and assist_docno like 'SF%')) and" +
                                    " assist_docno = (select assist_docno from asnreqmaster  where member_no ='" + HdMemberNo.Value + "' and assist_docno like 'SF%')";

                    dt = ta.Query(sqlrow);
                    dt.Next();
                    rowcount = Convert.ToInt32(dt.GetString("receive_no"));
                    rowcount++;
                    assist_docnoo = dt.GetString("assist_docno");
                }
                else if (count == 0)
                {
                    rowcount = 1;
                    string sqlassistno = "select assist_docno from asnreqmaster where member_no ='" + HdMemberNo.Value + "' and assist_docno like 'SF%'";
                    dt = ta.Query(sqlassistno);
                    dt.Next();
                    assist_docnoo = dt.GetString("assist_docno").Trim();
                }
                for (int k = count + 1; k < rowww; k++)
                {
                    string name = DwReceive.GetItemString(k, "member_receive");
                    string per = DwReceive.GetItemString(k, "percent_receive");
                    string pay = DwReceive.GetItemString(k, "pay_receive");
                    string rownext = rowcount.ToString("000");

                    string sqlinsertrow = "insert into asnmembreceive (assist_docno,receive_no,member_receive,percent_receive,pay_receive)" +
                        "values ('" + assist_docnoo + "','" + rownext + "','" + name + "','" + per + "'," + pay + ")";
                    ta.Exe(sqlinsertrow);
                    //เพิ่ม
                    rowcount++;
                }
                DwUtil.UpdateDateWindow(DwReceive, "kt_65years.pbl", "asnmembreceive");
                HdRowReceive.Value = "";
            }
            else if (HdRowReceive.Value == "")
            {

                DwUtil.UpdateDateWindow(DwReceive, "kt_65years.pbl", "asnmembreceive");
            }
        }

        //private void Calculate65(decimal sumpays)
        //{
        //    // Calculate 65
        //    string Tage = DwMainP.GetItemString(1, "member_years_disp");
        //    int memb_age = Convert.ToInt32(Tage.Replace("ปี", ""));
        //    string TMaxYear, TMinYear;
        //    decimal sumShare10 = 0, sumShare20 = 0, sumShare30 = 0, sumShare35 = 0, sumShare40 = 0;
        //    double PayShare;
        //    DateTime DateTemp2, DateTemp5, DateTemp10, DateTemp15, DateTemp20;
        //    String todayDate = DateTime.Now.ToString("dd/MM/yyyy", WebUtil.EN);
        //    DateTemp2 = DateTime.ParseExact(todayDate, "dd/MM/yyyy", WebUtil.EN);
        //    DateTemp2 = DateTemp2.AddYears(-2);
        //    DateTemp5 = DateTime.ParseExact(todayDate, "dd/MM/yyyy", WebUtil.EN);
        //    DateTemp5 = DateTemp5.AddYears(-5);
        //    DateTemp10 = DateTime.ParseExact(todayDate, "dd/MM/yyyy", WebUtil.EN);
        //    DateTemp10 = DateTemp10.AddYears(-10);
        //    DateTemp15 = DateTime.ParseExact(todayDate, "dd/MM/yyyy", WebUtil.EN);
        //    DateTemp15 = DateTemp15.AddYears(-15);
        //    DateTemp20 = DateTime.ParseExact(todayDate, "dd/MM/yyyy", WebUtil.EN);
        //    DateTemp20 = DateTemp20.AddYears(-20);

        //    //DateTemp = new DateTime(2012, 5, 30);
        //    //HdMinOperatedate.Value = DateTempMax.Date.ToShortDateString();
        //    if (memb_age > 2)
        //    {
        //        TMaxYear = HdMaxOperatedate.Value;
        //        TMinYear = HdMinOperatedate.Value;

        //        string SQLSelectShare10 = "select sum(share_amount) from shsharestatement where shritemtype_code = 'SPM' and member_no ='" + HdMemberNo.Value + "' and operate_date between to_date('" + DateTemp5.ToString("dd/MM/yyyy") + "','dd/mm/yyyy') and to_date('" + DateTemp2.ToString("dd/MM/yyyy") + "','dd/mm/yyyy')";
        //        Sdt dtShare10 = WebUtil.QuerySdt(SQLSelectShare10);
        //        if (dtShare10.Next())
        //        {
        //            sumShare10 = dtShare10.GetDecimal("sum(share_amount)");
        //        }

        //        string SQLSelectShare20 = "select sum(share_amount) from shsharestatement where shritemtype_code = 'SPM' and member_no ='" + HdMemberNo.Value + "' and operate_date between to_date('" + DateTemp10.ToString("dd/MM/yyyy") + "','dd/mm/yyyy') and to_date('" + DateTemp5.ToString("dd/MM/yyyy") + "','dd/mm/yyyy')";
        //        Sdt dtShare20 = WebUtil.QuerySdt(SQLSelectShare20);
        //        if (dtShare20.Next())
        //        {
        //            sumShare20 = dtShare20.GetDecimal("sum(share_amount)");
        //        }

        //        string SQLSelectShare30 = "select sum(share_amount) from shsharestatement where shritemtype_code = 'SPM' and member_no ='" + HdMemberNo.Value + "' and operate_date between to_date('" + DateTemp15.ToString("dd/MM/yyyy") + "','dd/mm/yyyy') and to_date('" + DateTemp10.ToString("dd/MM/yyyy") + "','dd/mm/yyyy')";
        //        Sdt dtShare30 = WebUtil.QuerySdt(SQLSelectShare30);
        //        if (dtShare30.Next())
        //        {
        //            sumShare30 = dtShare30.GetDecimal("sum(share_amount)");
        //        }

        //        string SQLSelectShare35 = "select sum(share_amount) from shsharestatement where shritemtype_code = 'SPM' and member_no ='" + HdMemberNo.Value + "' and operate_date between to_date('" + DateTemp20.ToString("dd/MM/yyyy") + "','dd/mm/yyyy') and to_date('" + DateTemp15.ToString("dd/MM/yyyy") + "','dd/mm/yyyy')";
        //        Sdt dtShare35 = WebUtil.QuerySdt(SQLSelectShare35);
        //        if (dtShare35.Next())
        //        {
        //            sumShare35 = dtShare35.GetDecimal("sum(share_amount)");
        //        }

        //        string SQLSelectShare40 = "select sum(share_amount) from shsharestatement where shritemtype_code = 'SPM' and member_no ='" + HdMemberNo.Value + "' and operate_date > to_date('" + DateTemp20.ToString("dd/MM/yyyy") + "','dd/mm/yyyy')";
        //        Sdt dtShare40 = WebUtil.QuerySdt(SQLSelectShare40);
        //        if (dtShare40.Next())
        //        {
        //            sumShare40 = dtShare35.GetDecimal("sum(share_amount)");
        //        }

        //        PayShare = (Convert.ToDouble(sumShare10) * 0.1) + (Convert.ToDouble(sumShare20) * 0.2) + (Convert.ToDouble(sumShare30) * 0.3) + (Convert.ToDouble(sumShare35) * 0.35) + (Convert.ToDouble(sumShare40) * 0.4);

        //        Calculate(sumpays, PayShare);
        //    }
        //}

        //        private void Calculate(decimal sumpays, double setPay)
        //        {
        //            try
        //            {
        //                //---------------------------------------------
        //                //ดึงข้อมูล ค่าเงินสูงสุด และ %การจ่ายแต่ละปี
        //                int memb_age = 0;
        //                double shareAmt = 0;
        //                double setDeadpay_amt = 0;
        //                string Tage = DwMainP.GetItemString(1, "member_years_disp");
        //                memb_age = Convert.ToInt32(Tage.Replace("ปี", ""));
        //                shareAmt = Convert.ToDouble(DwMainP.GetItemDecimal(1, "sharestk_amt"));
        //                //////
        //                double getAssist = Cal(HdMemberNo.Value);
        //                DwDetailP.SetItemDecimal(1, "assist_amt", Convert.ToDecimal(getAssist));
        //                //////
        //                string sqlmaxandper = @"select max_pay,percent from asnucfpay65 where minmemb_year <= " +
        //                      memb_age + " and " + memb_age + " < maxmemb_year";
        //                dt = ta.Query(sqlmaxandper);
        //                dt.Next();
        //                int max_pay = Convert.ToInt32(dt.GetDecimal("max_pay"));
        //                double year_per = dt.GetDouble("percent");

        //                setDeadpay_amt = setPay;//year_per * shareAmt; ******************** ส่งมาจาก Calculate

        //                if (setDeadpay_amt > max_pay)
        //                {
        //                    setDeadpay_amt = max_pay;
        //                }
        //                string sqlyear_per = @"select percent,year,maxpay_peryear  from asnucfperpay 
        //                                        where assist_code =( select max(assist_code) 
        //                                        from asnucfperpay where assist_code like 'SF%' )";
        //                dt = ta.Query(sqlyear_per);
        //                dt.Next();
        //                double percent = dt.GetDouble("percent");
        //                int year_pay = Convert.ToInt32(dt.GetDecimal("year"));
        //                int maxpay_peryear = Convert.ToInt32(dt.GetDecimal("maxpay_peryear"));
        //                double perpay = getAssist * percent;
        //                if (perpay >= maxpay_peryear)
        //                {
        //                    perpay = maxpay_peryear;
        //                }
        //                //setDeadpay_amt = setDeadpay_amt - Convert.ToDouble(sumpays);

        //                if (setDeadpay_amt <= 0)
        //                    setDeadpay_amt = 0;
        //                if (perpay % 10 > 1)
        //                {
        //                    perpay = perpay + (10 - (perpay % 10));
        //                }
        //                DwDetailP.SetItemDecimal(1, "assist_amt", Convert.ToDecimal(getAssist));
        //                DwDetailP.SetItemDecimal(1, "perpay", Convert.ToDecimal(perpay));

        //                if (DwDetailP.GetItemDecimal(1, "approve_amt") == 0)
        //                {
        //                    DwDetailP.SetItemDecimal(1, "approve_amt", Convert.ToDecimal(getAssist));
        //                }
        //                //if (DwDetailP.GetItemDecimal(1, "balance") == 0)
        //                //{
        //                //    decimal balance = Convert.ToDecimal(setDeadpay_amt) - sumpays;
        //                //    DwDetailP.SetItemDecimal(1, "balance", balance);
        //                //}
        //                decimal balance = Convert.ToDecimal(getAssist) - sumpays;
        //                if (balance < 0)
        //                    balance = 0;
        //                DwDetailP.SetItemDecimal(1, "balance", balance);

        //            }
        //            catch { }

        //        }

        private void JspostBankAccid()
        {
            string BankAccidStr = DwMainP.GetItemString(1, "asnslippayout_bank_accid");
            string branchId = BankAccidStr.Substring(0, 3);
            while (branchId.Count() != 4)
            {
                branchId = "0" + branchId;
            }

            DwMainP.SetItemString(1, "asnslippayout_bankbranch_id", branchId);
            //int BankAccidInt = Convert.ToInt32(BankAccidStr);
        }

        //public Double Cal(string member_no)
        //{
        //    DateTime calculate_date = DateTime.Now;
        //    DateTime entry_date = DateTime.Now;
        //    Double sum = 0;
        //    Double calpay = 0;
        //    Double share_amount = 0;

        //    string sqlshare = "select share_amount,entry_date from shsharestatement where shritemtype_code ='SPM' and member_no='" + member_no + "'";
        //    Sdt share = WebUtil.QuerySdt(sqlshare);
        //    while (share.Next())
        //    {
        //        share_amount = Convert.ToDouble(share.GetDecimal("share_amount"));
        //        entry_date = share.GetDate("entry_date");

        //        tp = calculate_date - entry_date;
        //        Double share_year = (tp.TotalDays / 365);

        //        if (share_year > 2 && share_year < 5)
        //        {
        //            calpay = share_amount * 0.1;
        //        }
        //        else if (share_year > 5 && share_year < 10)
        //        {
        //            calpay = share_amount * 0.2;
        //        }
        //        else if (share_year > 10 && share_year < 15)
        //        {
        //            calpay = share_amount * 0.3;
        //        }
        //        else if (share_year > 15 && share_year < 20)
        //        {
        //            calpay = share_amount * 0.35;
        //        }
        //        else if (share_year > 20)
        //        {
        //            calpay = share_amount * 0.4;
        //        }
        //        sum = sum + calpay;
        //    }//end while

        //    return sum;

        //}

        public void CalSumShare()
        {
            string member_no = HdMemberNo.Value;
            string yeartemp = "0000", monthtemp = "01", capital_year = "0000", month = "01", day = "01", rule_seq = "1", rule_seq_temp = "0";
            decimal count = 0, share_temp =0;
            int i = 1, j = 1, flag = 0;
            double sumcal = 0;
            string startdate = "01/01/2000", enddate = "01/01/2000", deaddate;
            //string startdate1, startdate2, startdate3, startdate4, startdate5;
            //string enddate1, enddate2, enddate3, enddate4, enddate5;
            string[] seq_no = new string[6];
            double[] percent = new double[6];
            decimal thismonth = DateTime.Now.Month;
            decimal thisyear = DateTime.Now.Year + 543;
            decimal diff_year;
            decimal share1 = 0, share2 = 0, share3 = 0, share4 = 0, share5 = 0, share6 = 0, share7 = 0, share8 = 0, share9 = 0, share10 = 0, share11 = 0, share12 = 0;
            string check65 = "0";

            string SQLSumshare = "Select * from asnsumshare where member_no = '" + member_no + "'";
            Sdt dtsumshare = WebUtil.QuerySdt(SQLSumshare);

            string SQLShare = "select * from asnshare where member_no = '" + member_no + "' and capital_year <= '2546' order by capital_year DESC";
            Sdt dtshare = WebUtil.QuerySdt(SQLShare);

            //string SQLStatement = "select * from shsharestatement where member_no = '"+member_no+"' and shritemtype_code = 'SPM' and item_status <> -9 order by seq_no ASC";
            string SQLStatementImp = "select member_no , share_amount , operate_date , to_char(operate_date,'mm') as sharemonth from shsharestatement where member_no = '" + member_no + "' and shritemtype_code = 'SPM' and item_status <> -9 order by seq_no ASC";
            Sdt dtStatement = WebUtil.QuerySdt(SQLStatementImp);

            string SQLDeadDate = "select dead_date from asnreqmaster where member_no = '" + member_no + "'";
            Sdt dtDeadDate = WebUtil.QuerySdt(SQLDeadDate);

            deaddate = DwDetailP.GetItemString(1, "member_dead_tdate"); //******************************************

            thismonth = Convert.ToInt32(deaddate.Substring(2, 2));
            thisyear = Convert.ToInt32(deaddate.Substring(4, 4));

            //ดูว่าใน ASNSUMSHARE มีข้อมูลหรือไม่
            if (dtsumshare.Next())
            {
                //check65 = "0";
                string SqlDelete = "delete from asnsumshare where member_no = '" + member_no + "'";
                WebUtil.Query(SqlDelete);
            }

            if (check65 == "0")
            {
                HdCheckOldMem.Value = "0";
                while (dtStatement.Next())
                {
                    monthtemp = dtStatement.GetString("sharemonth");
                    share_temp = dtStatement.GetDecimal("share_amount");
                    share_temp = share_temp * 10;
                    capital_year = dtStatement.GetDateTh("operate_date");
                    capital_year = capital_year.Substring(capital_year.Length - 4);
                    if (yeartemp == "0000")
                        yeartemp = capital_year;
                    if (yeartemp != capital_year)
                    {
                        InsertSumShare(share1, share2, share3, share4, share5, share6, share7, share8, share9, share10, share11, share12, yeartemp, flag);
                        //share_temp = 0;
                        j = 1;
                        flag = 0;
                        yeartemp = "0000";
                        share1 = share2 = share3 = share4 = share5 = share6 = share7 = share8 = share9 = share10 = share11 = share12 = 0;
                    }



                    diff_year = thisyear - Convert.ToInt32(capital_year);
                    if (diff_year >= 2 && count == 0)
                    {
                        flag = 1;
                        count = 1;
                    }

                    switch (monthtemp)
                    {
                        case "01": share1 = share_temp; break;
                        case "02": share2 = share_temp; break;
                        case "03": share3 = share_temp; break;
                        case "04": share4 = share_temp; break;
                        case "05": share5 = share_temp; break;
                        case "06": share6 = share_temp; break;
                        case "07": share7 = share_temp; break;
                        case "08": share8 = share_temp; break;
                        case "09": share9 = share_temp; break;
                        case "10": share10 = share_temp; break;
                        case "11": share11 = share_temp; break;
                        case "12": share12 = share_temp; break;
                    }
                    j++;

                    //if (yeartemp != capital_year)
                    //{
                    //    InsertSumShare(share1, share2, share3, share4, share5, share6, share7, share8, share9, share10, share11, share12, capital_year, flag);
                    //    j = 1;
                    //    flag = 0;
                    //    yeartemp = "0000";
                    //    share1 = share2 = share3 = share4 = share5 = share6 = share7 = share8 = share9 = share10 = share11 = share12 = 0;
                    //}

                }
                if (j >= 1 && share_temp != 0) // กรณี มีหุ้นไม่ครบปี ในปีสุดท้าย
                    InsertSumShare(share1, share2, share3, share4, share5, share6, share7, share8, share9, share10, share11, share12, capital_year, flag);


                while (dtshare.Next())
                {
                    share1 = dtshare.GetDecimal("share_amt_1");
                    share2 = dtshare.GetDecimal("share_amt_2");
                    share3 = dtshare.GetDecimal("share_amt_3");
                    share4 = dtshare.GetDecimal("share_amt_4");
                    share5 = dtshare.GetDecimal("share_amt_5");
                    share6 = dtshare.GetDecimal("share_amt_6");
                    share7 = dtshare.GetDecimal("share_amt_7");
                    share8 = dtshare.GetDecimal("share_amt_8");
                    share9 = dtshare.GetDecimal("share_amt_9");
                    share10 = dtshare.GetDecimal("share_amt_10");
                    share11 = dtshare.GetDecimal("share_amt_11");
                    share12 = dtshare.GetDecimal("share_amt_12");

                    capital_year = dtshare.GetString("capital_year");

                    InsertSumShare(share1, share2, share3, share4, share5, share6, share7, share8, share9, share10, share11, share12, capital_year, flag);

                }
                //CalSumShareChange();

            }
            //else if (check65 == "1") 
            //{
            //    HdCheckOldMem.Value = "1";
            //    string SqlDelete = "delete from asnsumshare where member_no = '"+member_no+"'";
            //    WebUtil.Query(SqlDelete);
            //    check65 = "0";
            //    CalSumShare();
            //    cal_last();
            //    GetMemberDetail();
            //    CalOldMember();
            //}
        } //5

        public void CalSumShareChange()
        {
            string member_no = HdMemberNo.Value;
            int i = 0;
            string year;
            string sql, sql2;
            string startdate = DateTime.Now.ToString("dd/MM/yyyy"), enddate = DateTime.Now.ToString("dd/MM/yyyy");
            decimal month_1 = 0, month_2 = 0, month_3 = 0, rule_seq = 0, sumyear = 0, cal_amt = 0;
            decimal month_12 = 0, month_22 = 0, month_32 = 0, rule_seq2 = 0, sumyear2 = 0, cal_amt2 = 0;//แถวบน
            decimal m1, m2, m3, m4, m5, m6, m7, m8, m9, m10, m11, m12;

            string dead_date = DwDetailP.GetItemString(1, "member_dead_tdate");//ทดสอบวันตาย*******************************************************
            DateTime dead_dateR = DateTime.ParseExact(HdDeadDate.Value, "dd/MM/yyyy", WebUtil.TH);
            decimal thisyear = dead_dateR.Year;
            int month_dead = Convert.ToInt32(dead_date.Substring(2, 2));

            int share_age = 0;

            sql = "select year from asnsumshare where member_no='" + member_no + "' order by rule_seq asc,year desc";
            Sdt dt = WebUtil.QuerySdt(sql);
            Sdt dt2;
            while (dt.Next())
            {
                year = dt.GetString("year");
                share_age = Convert.ToInt32(thisyear) - Convert.ToInt32(year);

                sql2 = "select month_1,month_2,month_3,rule_seq from asnsumshare where member_no='" + member_no + "' and year ='" + year + "' order by rule_seq asc";
                dt2 = WebUtil.QuerySdt(sql2);
                month_1 = 0;
                month_2 = 0;
                month_3 = 0;
                i = 0;
                while (dt2.Next())
                {
                    month_1 = month_1 + dt2.GetDecimal("month_1");
                    month_2 = month_2 + dt2.GetDecimal("month_2");
                    month_3 = month_3 + dt2.GetDecimal("month_3");
                    rule_seq2 = dt2.GetDecimal("rule_seq");
                    i++;
                }//while
                m1 = m2 = m3 = m4 = month_1 / 4;
                m5 = m6 = m7 = m8 = month_2 / 4;
                m9 = m10 = m11 = m12 = month_3 / 4;

                //----------------------------------------------------------------

                if (share_age == 5 || share_age == 10 || share_age == 15 || share_age == 20)
                {
                    if (month_dead >= 1 && month_dead < 5)
                    {
                        rule_seq = rule_seq2 + 1;
                        if (month_dead == 1)
                        {
                            month_1 = m1;
                            month_2 = 0;
                            month_3 = 0;
                            month_12 = m2 + m3 + m4;
                            month_22 = m5 + m6 + m7 + m8;
                            month_32 = m9 + m10 + m11 + m12;
                        }
                        else if (month_dead == 2)
                        {
                            month_1 = m1 + m2;
                            month_2 = 0;
                            month_3 = 0;
                            month_12 = m3 + m4;
                            month_22 = m5 + m6 + m7 + m8;
                            month_32 = m9 + m10 + m11 + m12;
                        }
                        else if (month_dead == 3)
                        {
                            month_1 = m1 + m2 + m3;
                            month_2 = 0;
                            month_3 = 0;
                            month_12 = m4;
                            month_22 = m5 + m6 + m7 + m8;
                            month_32 = m9 + m10 + m11 + m12;
                        }
                        else if (month_dead == 4)
                        {
                            month_1 = m1 + m2 + m3 + m4;
                            month_2 = 0;
                            month_3 = 0;
                            month_12 = 0;
                            month_22 = m5 + m6 + m7 + m8;
                            month_32 = m9 + m10 + m11 + m12;
                        }
                    }
                    else if (month_dead >= 5 && month_dead < 9)
                    {
                        rule_seq = rule_seq2 + 1;
                        if (month_dead == 5)
                        {
                            month_1 = m1 + m2 + m3 + m4;
                            month_2 = m5;
                            month_3 = 0;
                            month_12 = 0;
                            month_22 = m6 + m7 + m8;
                            month_32 = m9 + m10 + m11 + m12;
                        }
                        else if (month_dead == 6)
                        {
                            month_1 = m1 + m2 + m3 + m4;
                            month_2 = m5 + m6;
                            month_3 = 0;
                            month_12 = 0;
                            month_22 = m7 + m8;
                            month_32 = m9 + m10 + m11 + m12;
                        }
                        else if (month_dead == 7)
                        {
                            month_1 = m1 + m2 + m3 + m4;
                            month_2 = m5 + m6 + m7;
                            month_3 = 0;
                            month_12 = 0;
                            month_22 = m8;
                            month_32 = m9 + m10 + m11 + m12;
                        }
                        else if (month_dead == 8)
                        {
                            month_1 = m1 + m2 + m3 + m4;
                            month_2 = m5 + m6 + m7 + m8;
                            month_3 = 0;
                            month_12 = 0;
                            month_22 = 0;
                            month_32 = m9 + m10 + m11 + m12;
                        }
                    }
                    else if (month_dead >= 9 && month_dead < 12)
                    {
                        rule_seq = rule_seq2 + 1;
                        if (month_dead == 9)
                        {
                            month_1 = m1 + m2 + m3 + m4;
                            month_2 = m5 + m6 + m7 + m8;
                            month_3 = m9;
                            month_12 = 0;
                            month_22 = 0;
                            month_32 = m10 + m11 + m12;
                        }
                        else if (month_dead == 10)
                        {
                            month_1 = m1 + m2 + m3 + m4;
                            month_2 = m5 + m6 + m7 + m8;
                            month_3 = m9 + m10;
                            month_12 = 0;
                            month_22 = 0;
                            month_32 = m11 + m12;
                        }
                        else if (month_dead == 11)
                        {
                            month_1 = m1 + m2 + m3 + m4;
                            month_2 = m5 + m6 + m7 + m8;
                            month_3 = m9 + m10 + m11;
                            month_12 = 0;
                            month_22 = 0;
                            month_32 = m12;
                        }
                        else if (month_dead == 12)
                        {
                            month_1 = m1 + m2 + m3 + m4;
                            month_2 = m5 + m6 + m7 + m8;
                            month_3 = m9 + m10 + m11 + m12;
                            month_12 = 0;
                            month_22 = 0;
                            month_32 = 0;
                        }
                    }
                    //update ตรงนี้
                    sumyear2 = month_12 + month_22 + month_32;
                    sumyear = month_1 + month_2 + month_3;

                    if (share_age == 5)
                    {
                        cal_amt2 = Convert.ToDecimal(Convert.ToDouble(sumyear2) * 0.1);
                        cal_amt = Convert.ToDecimal(Convert.ToDouble(sumyear) * 0.2);

                    }
                    else if (share_age == 10)
                    {
                        cal_amt2 = Convert.ToDecimal(Convert.ToDouble(sumyear2) * 0.2);
                        cal_amt = Convert.ToDecimal(Convert.ToDouble(sumyear) * 0.3);
                    }
                    else if (share_age == 15)
                    {
                        cal_amt2 = Convert.ToDecimal(Convert.ToDouble(sumyear2) * 0.3);
                        cal_amt = Convert.ToDecimal(Convert.ToDouble(sumyear) * 0.35);
                    }
                    else if (share_age == 20)
                    {
                        cal_amt2 = Convert.ToDecimal(Convert.ToDouble(sumyear2) * 0.35);
                        cal_amt = Convert.ToDecimal(Convert.ToDouble(sumyear) * 0.4);
                    }

                    sql2 = "update asnsumshare set month_1=" + month_12 + ",month_2=" + month_22 + ",month_3=" + month_32 +
                        ",sumyear=" + sumyear2 + ",cal_amt=" + cal_amt2 + " where member_no='" + member_no + "' and year='" + year + "' and rule_seq=" + rule_seq2 + "";
                    //Sdt up = WebUtil.QuerySdt(sql2);
                    string SQLDate = "select startdate,enddate from asnsumshare where member_no = '" + member_no + "' and rule_seq ='" + rule_seq + "'";
                    Sdt dtDate = WebUtil.QuerySdt(SQLDate);
                    if (dtDate.Next())
                    {
                        startdate = dtDate.GetDateEn("startdate");
                        enddate = dtDate.GetDateEn("enddate");
                        startdate = startdate.Replace("-", "/");
                        enddate = enddate.Replace("-", "/");
                    }
                    if (HdCheckOldMem.Value == "1")
                        rule_seq = rule_seq - 2;
                    sql = "insert into asnsumshare (member_no,rule_seq,year,month_1,month_2,month_3,sumyear,cal_amt,startdate , enddate) values ('" +
                        member_no + "'," + rule_seq + ",'" + year + "'," + month_1 + "," + month_2 + "," + month_3 + "," + sumyear + "," + cal_amt + ",to_date('" + startdate + "','yyyy/mm/dd'),to_date('" + enddate + "','yyyy/mm/dd'))";
                    //Sdt insert = WebUtil.QuerySdt(sql);

                }
                else if (i == 2)
                {
                    rule_seq = rule_seq2 + 1;
                    try
                    {
                        sql2 = "delete from asnsumshare where member_no='" + member_no + "' and rule_seq =" + rule_seq2 + "  and year ='" + year + "' ";
                        Sdt de = WebUtil.QuerySdt(sql2);
                    }
                    catch { }

                    sql = "update asnsumshare set month_1=" + month_1 + ",month_2=" + month_2 + ",month_3=" + month_3 + ",sumyear=" + sumyear + ",cal_amt=" + cal_amt +
                        " where member_no='" + member_no + "' and rule_seq=" + rule_seq + " and year='" + year + "'";
                    Sdt up2 = WebUtil.QuerySdt(sql);
                }
                else if (i == 1)
                {
                    rule_seq = rule_seq2 + 1;

                    sql = "update asnsumshare set month_1=" + month_1 + ",month_2=" + month_2 + ",month_3=" + month_3 + ",sumyear=" + sumyear + ",cal_amt=" + cal_amt +
                        " where member_no='" + member_no + "' and rule_seq=" + rule_seq + " and year='" + year + "'";
                    //Sdt up2 = WebUtil.QuerySdt(sql);
                }

            }//while


        } //6

        public void InsertSumShare(decimal share1, decimal share2, decimal share3, decimal share4, decimal share5, decimal share6, decimal share7, decimal share8, decimal share9, decimal share10, decimal share11, decimal share12, string capital_year, int flag)
        {
            string member_no = HdMemberNo.Value;
            string yeartemp = "0000", monthtemp = "01", month = "01", day = "01", rule_seq = "1", rule_seq_temp = "0";
            decimal count = 0, share_temp;
            int i = 1, j = 1;
            double sumcal = 0;
            string startdate = "01/01/2000", enddate = "01/01/2000";
            string[] seq_no = new string[6];
            double[] percent = new double[6];
            DateTime dead_date = DateTime.ParseExact(HdDeadDate.Value, "dd/MM/yyyy", WebUtil.TH);
            decimal thisyear = dead_date.Year + 543;
            decimal thismonth = dead_date.Month;//DateTime.Now.Month;
            //decimal thisyear = DateTime.Now.Year + 543;
            decimal month1, month2, month3, sumyear, diff_year;
            //decimal share1, share2, share3, share4, share5, share6, share7, share8, share9, share10, share11, share12;
            string check65 = "0";
            if (flag == 1) // ดึงปีซื้อหุ้นปีแรก
            {
                HdMaxYear.Value = capital_year;
                monthtemp = thismonth.ToString("00");
                count = 1;
            }
            yeartemp = HdMaxYear.Value;
            monthtemp = thismonth.ToString("00");

            string SQLUcf = "select percent from asnucfpay65";
            Sdt dtUcf = WebUtil.QuerySdt(SQLUcf);
            while (dtUcf.Next())
            {
                percent[i] = dtUcf.GetDouble("percent");
                percent[i] = Convert.ToDouble(percent[i].ToString("0.00"));
                i++;
            }

            diff_year = thisyear - Convert.ToInt32(capital_year);

            if (diff_year == 2)
            {
                switch (Convert.ToInt32(thismonth))
                {
                    case 12: break;
                    case 11: share12 = 0; break;
                    case 10: share12 = share11 = 0; break;
                    case 9: share12 = share11 = share10 = 0; break;
                    case 8: share12 = share11 = share10 = share9 = 0; break;
                    case 7: share12 = share11 = share10 = share9 = share8 = 0; break;
                    case 6: share12 = share11 = share10 = share9 = share8 = share7 = 0; break;
                    case 5: share12 = share11 = share10 = share9 = share8 = share7 = share6 = 0; break;
                    case 4: share12 = share11 = share10 = share9 = share8 = share7 = share6 = share5 = 0; break;
                    case 3: share12 = share11 = share10 = share9 = share8 = share7 = share6 = share5 = share4 = 0; break;
                    case 2: share12 = share11 = share10 = share9 = share8 = share7 = share6 = share5 = share4 = share3 = 0; break;
                    case 1: share12 = share11 = share10 = share9 = share8 = share7 = share6 = share5 = share4 = share3 = share2 = 0; break;
                }
            }
            month1 = share1 + share2 + share3 + share4;
            month2 = share5 + share6 + share7 + share8;
            month3 = share9 + share10 + share11 + share12;
            sumyear = month1 + month2 + month3;

            if (share1 != 0)
                month = "31/01/";
            else if (share2 != 0)
                month = "28/02/";
            else if (share3 != 0)
                month = "31/03/";
            else if (share4 != 0)
                month = "30/04/";
            else if (share5 != 0)
                month = "31/05/";
            else if (share6 != 0)
                month = "30/06/";
            else if (share7 != 0)
                month = "31/07/";
            else if (share8 != 0)
                month = "31/08/";
            else if (share9 != 0)
                month = "30/09/";
            else if (share10 != 0)
                month = "31/10/";
            else if (share11 != 0)
                month = "30/11/";
            else if (share12 != 0)
                month = "31/12/";
            else
                month = "31/01/";

            if (diff_year >= 2 && diff_year <= 5)
            {
                rule_seq = "1";
                if (diff_year == 2)
                {
                    switch (Convert.ToInt32(thismonth))
                    {
                        case 12: sumcal = Convert.ToDouble(month1 + month2 + month3) * percent[1]; break;
                        case 11: sumcal = Convert.ToDouble(share9 + share10 + share11 + month1 + month2) * percent[1]; break;
                        case 10: sumcal = Convert.ToDouble(share9 + share10 + month1 + month2) * percent[1]; break;
                        case 9: sumcal = Convert.ToDouble(share9 + month1 + month2) * percent[1]; break;
                        case 8: sumcal = Convert.ToDouble(month1 + month2) * percent[1]; break;
                        case 7: sumcal = Convert.ToDouble(share5 + share6 + share7 + month1) * percent[1]; break;
                        case 6: sumcal = Convert.ToDouble(share5 + share6 + month1) * percent[1]; break;
                        case 5: sumcal = Convert.ToDouble(share5 + month1) * percent[1]; break;
                        case 4: sumcal = Convert.ToDouble(month1) * percent[1]; break;
                        case 3: sumcal = Convert.ToDouble(share1 + share2 + share3) * percent[1]; break;
                        case 2: sumcal = Convert.ToDouble(share1 + share2) * percent[1]; break;
                        case 1: sumcal = Convert.ToDouble(share1) * percent[1]; break;
                    }
                }
                else
                {
                    sumcal = Convert.ToDouble(sumyear) * percent[1];
                }
                i = 1;
            }
            else if (diff_year > 5 && diff_year <= 10)
            {
                rule_seq = "2";
                sumcal = Convert.ToDouble(sumyear) * percent[2];
                i = 2;
            }
            else if (diff_year > 10 && diff_year <= 15)
            {
                rule_seq = "3";
                sumcal = Convert.ToDouble(sumyear) * percent[3];
                i = 3;
            }
            else if (diff_year > 15 && diff_year <= 20)
            {
                rule_seq = "4";
                sumcal = Convert.ToDouble(sumyear) * percent[4];
                i = 4;
            }
            else if (diff_year > 20)
            {
                rule_seq = "5";
                sumcal = Convert.ToDouble(sumyear) * percent[5];
                i = 5;
            }
            //ดึง

            try
            {
                if (rule_seq == "1")
                {
                    startdate = month + (Convert.ToInt32(yeartemp) - 543);
                    enddate = "01" + month.Substring(2, 4) + (Convert.ToInt32(yeartemp) - 543 - 3);
                }
                else if (rule_seq == "2")
                {
                    startdate = month + (Convert.ToInt32(yeartemp) - 543 - 3);
                    enddate = "01" + month.Substring(2, 4) + (Convert.ToInt32(yeartemp) - 543 - 8);
                }
                else if (rule_seq == "3")
                {
                    startdate = month + (Convert.ToInt32(yeartemp) - 543 - 8);
                    enddate = "01" + month.Substring(2, 4) + (Convert.ToInt32(yeartemp) - 543 - 13);
                }
                else if (rule_seq == "4")
                {
                    startdate = month + (Convert.ToInt32(yeartemp) - 543 - 13);
                    enddate = "01" + month.Substring(2, 4) + (Convert.ToInt32(yeartemp) - 543 - 18);
                }
                else if (rule_seq == "5")
                {
                    startdate = month + (Convert.ToInt32(yeartemp) - 543 - 18);
                    enddate = "01" + month.Substring(2, 4) + (Convert.ToInt32(yeartemp) - 543 - 23);
                }
            }
            catch { }
            try
            {
                if (diff_year >= 2)
                {
                    string SQLInsSumShare = "insert into asnsumshare(member_no , rule_seq , year , month_1 , month_2 , month_3 , sumyear , cal_amt , startdate , enddate) values ('" + member_no + "','" + rule_seq + "','" + capital_year + "'," + month1 + "," + month2 + "," + month3 + "," + sumyear + "," + sumcal + ",to_date('" + startdate + "','dd/mm/yyyy'),to_date('" + enddate + "','dd/mm/yyyy'))";
                    WebUtil.Query(SQLInsSumShare);
                }
            }
            catch
            {
            }
        } //7

        public void CalOldMember()
        {
            decimal month1, month2, month3, month1_tmp, month2_tmp, month3_tmp, sumyear, sumyear_tmp, cal_amt, cal_amt_tmp, rule_seq;
            decimal share1 = 0, share2 = 0, share3 = 0, share4 = 0, share5 = 0, share6 = 0, share7 = 0, share8 = 0, share9 = 0, share10 = 0, share11 = 0, share12 = 0;
            decimal share_temp;
            DateTime dead_date = DateTime.ParseExact(HdDeadDate.Value, "dd/MM/yyyy", WebUtil.EN);
            decimal thisyear = dead_date.Year;
            string year, capital_year;
            DateTime startdate, enddate;
            int year_int, diff_year = 0, flag = 0, count = 0, j = 1;

            string SQLSelectSumShare = "select year from asnsumshare where member_no = '" + HdMemberNo.Value + "' group by year having count(year) >1";
            //string SQLSelectSumShare = "select * from asnsumshare where member_no = '"+HdMemberNo.Value+"'";
            Sdt dtSumShare = WebUtil.QuerySdt(SQLSelectSumShare);
            while (dtSumShare.Next())
            {
                year = dtSumShare.GetString("year");
                month1 = month2 = month3 = month1_tmp = month2_tmp = month3_tmp = sumyear_tmp = cal_amt_tmp = rule_seq = 0;
                string SQLSumShare = "select * from asnsumshare where member_no = '" + HdMemberNo.Value + "' and year = '" + year + "' order by year";
                Sdt dtTmp = WebUtil.QuerySdt(SQLSumShare);
                while (dtTmp.Next())
                {
                    month1 = dtTmp.GetDecimal("month_1");
                    month2 = dtTmp.GetDecimal("month_2");
                    month3 = dtTmp.GetDecimal("month_3");
                    sumyear = dtTmp.GetDecimal("sumyear");
                    cal_amt = dtTmp.GetDecimal("cal_amt");
                    startdate = dtTmp.GetDate("startdate");
                    enddate = dtTmp.GetDate("enddate");
                    rule_seq = dtTmp.GetDecimal("rule_seq");

                    month1_tmp = month1 + month1_tmp;
                    month2_tmp = month2 + month2_tmp;
                    month3_tmp = month3 + month3_tmp;
                    sumyear_tmp = sumyear + sumyear_tmp;
                    cal_amt_tmp = cal_amt + cal_amt_tmp;
                }

                string SQLDel = "delete from asnsumshare where member_no = '" + HdMemberNo.Value + "' and rule_seq = " + (rule_seq) + " and year = '" + year + "'";
                WebUtil.Query(SQLDel);

                string SQLUpdate = "update asnsumshare set month_1 = " + month1_tmp + " , month_2 =" + month2_tmp + " , month_3=" + month3_tmp + " , sumyear=" + sumyear_tmp + " , cal_amt=" + cal_amt_tmp + " where member_no = '" + HdMemberNo.Value + "' and year = " + year + "";
                WebUtil.Query(SQLUpdate);

                string SQLSelectMaxYear = "select max(year) from asnsumshare where member_no = '" + HdMemberNo.Value + "'";
                Sdt dtMaxYear = WebUtil.QuerySdt(SQLSelectMaxYear);

                //string SQLSelectMaxRec = "";
                string SQLDelMaxYear = "delete from asnsumshare where year in (select max(year) from asnsumshare where member_no = '" + HdMemberNo.Value + "') and member_no = '" + HdMemberNo.Value + "'";
                WebUtil.Query(SQLDelMaxYear);

                if (dtMaxYear.Next())
                {
                    year = dtMaxYear.GetString("max(year)");
                    year_int = Convert.ToInt32(year) - 543;
                    string SQLGetState = "select * from shsharestatement where member_no='" + HdMemberNo.Value + "'and shritemtype_code = 'SPM' and operate_date between to_date('01/01/" + year_int + "','dd/mm/yyyy') and to_date('31/12/" + year_int + "','dd/mm/yyyy')order by seq_no ";
                    Sdt dtGetState = WebUtil.QuerySdt(SQLGetState);
                    while (dtGetState.Next())
                    {
                        share_temp = dtGetState.GetDecimal("share_amount");
                        share_temp = share_temp * 10;
                        capital_year = dtGetState.GetDateTh("operate_date");
                        capital_year = capital_year.Substring(capital_year.Length - 4);
                        diff_year = Convert.ToInt32(thisyear) - Convert.ToInt32(capital_year);
                        if (diff_year >= 2 && count == 0)
                        {
                            flag = 1;
                            count = 1;
                        }
                        switch (j)
                        {
                            case 1: share1 = share_temp; break;
                            case 2: share2 = share_temp; break;
                            case 3: share3 = share_temp; break;
                            case 4: share4 = share_temp; break;
                            case 5: share5 = share_temp; break;
                            case 6: share6 = share_temp; break;
                            case 7: share7 = share_temp; break;
                            case 8: share8 = share_temp; break;
                            case 9: share9 = share_temp; break;
                            case 10: share10 = share_temp; break;
                            case 11: share11 = share_temp; break;
                            case 12: share12 = share_temp; break;
                        }
                        j++;

                        if (j > 12)
                        {
                            InsertSumShare(share1, share2, share3, share4, share5, share6, share7, share8, share9, share10, share11, share12, capital_year, flag);
                            j = 1;
                            flag = 0;
                        }
                    }
                }


            }
            CalSumShareChange();
        }

        public void cal_last()
        {
            decimal month_1 = 0, month_2 = 0, month_3 = 0, rule_seq = 0, sumyear = 0, cal_amt = 0;
            decimal month_12 = 0, month_22 = 0, month_32 = 0, rule_seq2 = 0, sumyear2 = 0, cal_amt2 = 0;//แถวบน
            decimal m1 = 0, m2 = 0, m3 = 0, m4 = 0, m5 = 0, m6 = 0, m7 = 0, m8 = 0, m9 = 0, m10 = 0, m11 = 0, m12 = 0;
            string share_year = "";
            string shortmonth = "";


            DateTime dead_date = DateTime.ParseExact(HdDeadDate.Value, "dd/MM/yyyy", WebUtil.TH);
            decimal thisday = dead_date.Day;
            decimal thisyear = dead_date.Year + 543;
            decimal thismonth = dead_date.Month;
            string start_tdate = "", end_tdate = "";

            string query = "select * from asnsumshare where member_no = '" + HdMemberNo.Value + "' and year in (" + (thisyear - 5) + " , " + (thisyear - 10) + " , " + (thisyear - 15) + " , " + (thisyear - 20) + ")";
            Sdt dt1 = WebUtil.QuerySdt(query);

            while (dt1.Next())
            {
                rule_seq2 = dt1.GetDecimal("rule_seq");
                share_year = dt1.GetString("year");
                month_1 = dt1.GetDecimal("month_1");
                month_2 = dt1.GetDecimal("month_2");
                month_3 = dt1.GetDecimal("month_3");
                start_tdate = thisday.ToString("00") + thismonth.ToString("00") + share_year;
                end_tdate = thisday.ToString("00") + thismonth.ToString("00") + (Convert.ToInt32(share_year) - 5).ToString();
                DateTime start_date = DateTime.ParseExact(start_tdate, "ddMMyyyy", WebUtil.TH);
                DateTime end_date = DateTime.ParseExact(end_tdate, "ddMMyyyy", WebUtil.TH);

                m1 = m2 = m3 = m4 = (month_1 / 4);
                m5 = m6 = m7 = m8 = (month_2 / 4);
                m9 = m10 = m11 = m12 = (month_3 / 4);

                if (thismonth >= 1 && thismonth < 5)
                {
                    rule_seq = rule_seq2 + 1;
                    if (thismonth == 1)
                    {
                        month_1 = m1;
                        month_2 = 0;
                        month_3 = 0;
                        month_12 = m2 + m3 + m4;
                        month_22 = m5 + m6 + m7 + m8;
                        month_32 = m9 + m10 + m11 + m12;
                    }
                    else if (thismonth == 2)
                    {
                        month_1 = m1 + m2;
                        month_2 = 0;
                        month_3 = 0;
                        month_12 = m3 + m4;
                        month_22 = m5 + m6 + m7 + m8;
                        month_32 = m9 + m10 + m11 + m12;
                    }
                    else if (thismonth == 3)
                    {
                        month_1 = m1 + m2 + m3;
                        month_2 = 0;
                        month_3 = 0;
                        month_12 = m4;
                        month_22 = m5 + m6 + m7 + m8;
                        month_32 = m9 + m10 + m11 + m12;
                    }
                    else if (thismonth == 4)
                    {
                        month_1 = m1 + m2 + m3 + m4;
                        month_2 = 0;
                        month_3 = 0;
                        month_12 = 0;
                        month_22 = m5 + m6 + m7 + m8;
                        month_32 = m9 + m10 + m11 + m12;
                    }
                }
                else if (thismonth >= 5 && thismonth < 9)
                {
                    rule_seq = rule_seq2 + 1;
                    if (thismonth == 5)
                    {
                        month_1 = m1 + m2 + m3 + m4;
                        month_2 = m5;
                        month_3 = 0;
                        month_12 = 0;
                        month_22 = m6 + m7 + m8;
                        month_32 = m9 + m10 + m11 + m12;
                    }
                    else if (thismonth == 6)
                    {
                        month_1 = m1 + m2 + m3 + m4;
                        month_2 = m5 + m6;
                        month_3 = 0;
                        month_12 = 0;
                        month_22 = m7 + m8;
                        month_32 = m9 + m10 + m11 + m12;
                    }
                    else if (thismonth == 7)
                    {
                        month_1 = m1 + m2 + m3 + m4;
                        month_2 = m5 + m6 + m7;
                        month_3 = 0;
                        month_12 = 0;
                        month_22 = m8;
                        month_32 = m9 + m10 + m11 + m12;
                    }
                    else if (thismonth == 8)
                    {
                        month_1 = m1 + m2 + m3 + m4;
                        month_2 = m5 + m6 + m7 + m8;
                        month_3 = 0;
                        month_12 = 0;
                        month_22 = 0;
                        month_32 = m9 + m10 + m11 + m12;
                    }
                }
                else if (thismonth >= 9 && thismonth <= 12)
                {
                    rule_seq = rule_seq2 + 1;
                    if (thismonth == 9)
                    {
                        if (Convert.ToInt32(share_year) > 2546)
                        {
                            try
                            {
                                month_1 = m1 + m2 + m3 + m4;
                                month_2 = m5 + m6 + m7 + m8;
                                m9 = m10 = m11 = m12 = 0;
                                String tmpshare_year = (Convert.ToInt32(share_year) - 543).ToString();
                                String SqlSelectShare = "select member_no , operate_date , share_amount*10 as sharemultiple , to_char(operate_date,'mm') as shortmonth from shsharestatement where to_char(operate_date,'yyyy')='" + tmpshare_year + "' and to_char(operate_date,'mm') in ('09','10','11','12') and member_no = '" + HdMemberNo.Value + "' and shritemtype_code ='SPM' order by shortmonth";
                                Sdt dtShare = WebUtil.QuerySdt(SqlSelectShare);
                                while (dtShare.Next())
                                {
                                    shortmonth = dtShare.GetString("shortmonth");
                                    switch (shortmonth)
                                    {
                                        case "09": m9 = dtShare.GetDecimal("sharemultiple"); break;
                                        case "10": m10 = dtShare.GetDecimal("sharemultiple"); break;
                                        case "11": m11 = dtShare.GetDecimal("sharemultiple"); break;
                                        case "12": m12 = dtShare.GetDecimal("sharemultiple"); break;
                                    }
                                }
                                month_3 = m9;
                                month_12 = 0;
                                month_22 = 0;
                                month_32 = m10 + m11 + m12;
                            }
                            catch { }
                            //ยังติดที่ว่ามันตัดส่วนหลัง 0 ทิ้ง
                        }
                        else
                        {
                            month_1 = m1 + m2 + m3 + m4;
                            month_2 = m5 + m6 + m7 + m8;
                            month_3 = m9;
                            month_12 = 0;
                            month_22 = 0;
                            month_32 = m10 + m11 + m12;
                        }
                    }
                    else if (thismonth == 10)
                    {
                        if (Convert.ToInt32(share_year) > 2546)
                        {
                            try
                            {
                                month_1 = m1 + m2 + m3 + m4;
                                month_2 = m5 + m6 + m7 + m8;
                                m9 = m10 = m11 = m12 = 0;
                                String tmpshare_year = (Convert.ToInt32(share_year) - 543).ToString();
                                String SqlSelectShare = "select member_no , operate_date , share_amount*10 as sharemultiple , to_char(operate_date,'mm') as shortmonth from shsharestatement where to_char(operate_date,'yyyy')='" + tmpshare_year + "' and to_char(operate_date,'mm') in ('09','10','11','12') and member_no = '" + HdMemberNo.Value + "' and shritemtype_code ='SPM' order by shortmonth";
                                Sdt dtShare = WebUtil.QuerySdt(SqlSelectShare);
                                while (dtShare.Next())
                                {
                                    shortmonth = dtShare.GetString("shortmonth");
                                    switch (shortmonth)
                                    {
                                        case "09": m9 = dtShare.GetDecimal("sharemultiple"); break;
                                        case "10": m10 = dtShare.GetDecimal("sharemultiple"); break;
                                        case "11": m11 = dtShare.GetDecimal("sharemultiple"); break;
                                        case "12": m12 = dtShare.GetDecimal("sharemultiple"); break;
                                    }
                                }
                                month_3 = m9 + m10;
                                month_12 = 0;
                                month_22 = 0;
                                month_32 = m11 + m12;
                            }
                            catch { }
                            //ยังติดที่ว่ามันตัดส่วนหลัง 0 ทิ้ง
                        }
                        else
                        {
                            month_1 = m1 + m2 + m3 + m4;
                            month_2 = m5 + m6 + m7 + m8;
                            month_3 = m9 + m10;
                            month_12 = 0;
                            month_22 = 0;
                            month_32 = m11 + m12;
                        }
                    }
                    else if (thismonth == 11)
                    {
                        if (Convert.ToInt32(share_year) > 2546)
                        {
                            try
                            {
                                month_1 = m1 + m2 + m3 + m4;
                                month_2 = m5 + m6 + m7 + m8;
                                m9 = m10 = m11 = m12 = 0;
                                String tmpshare_year = (Convert.ToInt32(share_year) - 543).ToString();
                                String SqlSelectShare = "select member_no , operate_date , share_amount*10 as sharemultiple , to_char(operate_date,'mm') as shortmonth from shsharestatement where to_char(operate_date,'yyyy')='" + tmpshare_year + "' and to_char(operate_date,'mm') in ('09','10','11','12') and member_no = '" + HdMemberNo.Value + "' and shritemtype_code ='SPM' order by shortmonth";
                                Sdt dtShare = WebUtil.QuerySdt(SqlSelectShare);
                                while (dtShare.Next())
                                {
                                    shortmonth = dtShare.GetString("shortmonth");
                                    switch (shortmonth)
                                    {
                                        case "09": m9 = dtShare.GetDecimal("sharemultiple"); break;
                                        case "10": m10 = dtShare.GetDecimal("sharemultiple"); break;
                                        case "11": m11 = dtShare.GetDecimal("sharemultiple"); break;
                                        case "12": m12 = dtShare.GetDecimal("sharemultiple"); break;
                                    }
                                }
                                month_3 = m9 + m10 + m11;
                                month_12 = 0;
                                month_22 = 0;
                                month_32 = m12;
                            }
                            catch { }
                            //ยังติดที่ว่ามันตัดส่วนหลัง 0 ทิ้ง
                        }
                        else
                        {
                            month_1 = m1 + m2 + m3 + m4;
                            month_2 = m5 + m6 + m7 + m8;
                            month_3 = m9 + m10 + m11;
                            month_12 = 0;
                            month_22 = 0;
                            month_32 = m12;
                        }
                    }
                    else if (thismonth == 12)
                    {
                        month_1 = m1 + m2 + m3 + m4;
                        month_2 = m5 + m6 + m7 + m8;
                        month_3 = m9 + m10 + m11 + m12;
                        month_12 = 0;
                        month_22 = 0;
                        month_32 = 0;
                    }
                }

                sumyear2 = month_12 + month_22 + month_32;
                sumyear = month_1 + month_2 + month_3;

                if (Convert.ToInt32(share_year) + 5 == thisyear)
                {
                    cal_amt2 = Convert.ToDecimal(Convert.ToDouble(sumyear2) * 0.1);
                    cal_amt = Convert.ToDecimal(Convert.ToDouble(sumyear) * 0.2);
                }
                else if (Convert.ToInt32(share_year) + 10 == thisyear)
                {
                    cal_amt2 = Convert.ToDecimal(Convert.ToDouble(sumyear2) * 0.2);
                    cal_amt = Convert.ToDecimal(Convert.ToDouble(sumyear) * 0.3);
                }
                else if (Convert.ToInt32(share_year) + 15 == thisyear)
                {
                    cal_amt2 = Convert.ToDecimal(Convert.ToDouble(sumyear2) * 0.3);
                    cal_amt = Convert.ToDecimal(Convert.ToDouble(sumyear) * 0.35);
                }
                else if (Convert.ToInt32(share_year) + 20 == thisyear)
                {
                    cal_amt2 = Convert.ToDecimal(Convert.ToDouble(sumyear2) * 0.35);
                    cal_amt = Convert.ToDecimal(Convert.ToDouble(sumyear) * 0.4);
                }

                string SQLUPDATE = "update asnsumshare set month_1=" + month_12 + ",month_2=" + month_22 + ",month_3=" + month_32 +
                        ",sumyear=" + sumyear2 + ",cal_amt=" + cal_amt2 + " where member_no='" + HdMemberNo.Value + "' and year='" + share_year + "' and rule_seq=" + rule_seq2 + "";
                WebUtil.QuerySdt(SQLUPDATE);

                String SQLINSERT = "insert into asnsumshare (member_no,rule_seq,year,month_1,month_2,month_3,sumyear,cal_amt,startdate , enddate) values ('" +
                        HdMemberNo.Value + "'," + rule_seq + ",'" + share_year + "'," + month_1 + "," + month_2 + "," + month_3 + "," + sumyear + "," + cal_amt + ",to_date('" + start_date.ToString("yyyy/MM/dd") + "','yyyy/mm/dd'),to_date('" + end_date.ToString("yyyy/MM/dd") + "','yyyy/mm/dd'))";
                WebUtil.QuerySdt(SQLINSERT);

                try
                {
                    String SqlUpdateDate = "update asnsumshare set startdate = to_date('" + start_date.ToString("yyyy/MM/dd") + "','yyyy/mm/dd') , enddate = to_date('" + end_date.ToString("yyyy/MM/dd") + "','yyyy/mm/dd') where member_no = '" + HdMemberNo.Value + "' and rule_seq = " + rule_seq;
                    WebUtil.Query(SqlUpdateDate);
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                }
            }



        }  //8

        public void cut_reduce()
        {
            try
            {
                string capital_year = (state.SsWorkDate.Year + 543).ToString();
                DateTime calculate_date = DateTime.ParseExact(HdDeadDate.Value, "dd/MM/yyyy", WebUtil.TH);
                string SqlSelect = "select min(year) from asnsumshare where member_no = '" + HdMemberNo.Value + "' and sumyear = 0";
                Sdt dtyear = WebUtil.QuerySdt(SqlSelect);
                if (dtyear.Next())
                {
                    capital_year = dtyear.GetString("min(year)");
                    String SQLSelectCount = "select count(year) from asnsumshare where member_no = '"+HdMemberNo.Value +"' and year = '"+capital_year+"'";
                    Sdt dtC = WebUtil.QuerySdt(SQLSelectCount);
                    if (dtC.Next())
                    {
                        if(dtC.GetDecimal("count(year)") != 2)
                        {
                        String SQLDeleteLast = "delete from asnsumshare where year > '" + capital_year + "' and member_no = '" + HdMemberNo.Value + "'";
                        WebUtil.Query(SQLDeleteLast);
                        string SqlupdateMasterDate = "update asnreqmaster set calculate_date = to_date('" + calculate_date.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/MM/yyyy') where member_no = '" + HdMemberNo.Value + "' and assisttype_code = '90'";
                        WebUtil.Query(SqlupdateMasterDate);
                    
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }


        //step ลดหุ้น
        //

    }
}