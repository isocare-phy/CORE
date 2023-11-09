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
using CommonLibrary.WsCommon;

namespace Saving.Applications.app_assist
{

    public partial class w_sheet_kt_pay_dead : PageWebSheet, WebSheet
    {
        TimeSpan tp;
        protected Sta ta;
        protected Sdt dt;
        protected DwThDate tDwDetail;
        protected String postDeadDate;
        protected String postRetreiveDwMem;
        protected String postRetrieveDwMain;
        protected String postGetMemberDetail;
        protected String postSelectReport;
        protected Common CmSrv;
        protected String pblPension = "kt_pension.pbl";

        protected String app;
        protected String gid;
        protected String rid;
        protected String pdf;
        protected String runProcess;
        protected String popupReport;

        public void InitJsPostBack()
        {
            postRetreiveDwMem = WebUtil.JsPostBack(this, "postRetreiveDwMem");
            postRetrieveDwMain = WebUtil.JsPostBack(this, "postRetrieveDwMain");
            postGetMemberDetail = WebUtil.JsPostBack(this, "postGetMemberDetail");
            postDeadDate = WebUtil.JsPostBack(this, "postDeadDate");
            postSelectReport = WebUtil.JsPostBack(this, "postSelectReport");

            HdOpenIFrame.Value = "False";
            runProcess = WebUtil.JsPostBack(this, "runProcess");

            tDwDetail = new DwThDate(DwDetailP, this);
            tDwDetail.Add("member_dead_date", "member_dead_tdate");
        }

        public void WebSheetLoadBegin()
        {
            tDwDetail.Eng2ThaiAllRow();
            try
            {
                CmSrv = WsCalling.Common;
            }
            catch
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ติดต่อ Web Service ไม่ได้");
                return;
            }
            this.ConnectSQLCA();
            if (!IsPostBack)
            {
                DwMemP.Reset();
                DwDetailP.Reset();
                DwMainP.Reset();
                DwMainP.InsertRow(0);
                DwMemP.InsertRow(0);
                DwDetailP.InsertRow(0);
                dw_65.Reset();
                dw_pension.Reset();
                dw_65.InsertRow(0);
                dw_pension.InsertRow(0);
            }
            else
            {
                this.RestoreContextDw(DwMemP);
                this.RestoreContextDw(DwMainP);
                this.RestoreContextDw(DwDetailP);
                this.RestoreContextDw(dw_pension);
                this.RestoreContextDw(dw_65);
            }

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
            else if (eventArg == "postGetMemberDetail")
            {
                GetMemberMain();
            }
            else if (eventArg == "postSelectReport")
            {
                RunProcess();
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
            decimal countAssit = 0;
            decimal assist_amt = 0, approve_amt = 0, sumpays = 0, balance = 0, perpay = 0;
            int capital_year = DateTime.Now.Year + 543;
            DateTime member_dead_date = DwDetailP.GetItemDate(1, "member_dead_date");
            String member_dead_tdate = member_dead_date.ToString("dd/MM/yyyy");
            try
            {
                string checkAssist = "select count(assist_docno) from asnreqmaster where member_no = '" + HdMemberNo.Value + "' and assisttype_code ='90'";
                Sdt ck = WebUtil.QuerySdt(checkAssist);
                if (ck.Next())
                {
                    countAssit = ck.GetDecimal("count(assist_docno)");
                    if (countAssit == 0)
                    {
                        CalculateNewmem(HdMemberNo.Value);
                    }
                }
            }
            catch
            {
            }

            try
            {
                string checkAssist = "select count(assist_docno) from asnreqmaster where member_no = '" + HdMemberNo.Value + "' and assisttype_code ='80'";
                Sdt ck = WebUtil.QuerySdt(checkAssist);
                if (ck.Next())
                {
                    countAssit = ck.GetDecimal("count(assist_docno)");
                    if (countAssit == 0)
                    {
                        savePS();
                    }
                }
            }
            catch
            {
            }

            try
            {
                assist_amt = dw_pension.GetItemDecimal(1, "assist_amt");
                approve_amt = dw_pension.GetItemDecimal(1, "approve_amt");
                sumpays = dw_pension.GetItemDecimal(1, "sumpays");
                balance = dw_pension.GetItemDecimal(1, "balance");
                perpay = dw_pension.GetItemDecimal(1, "perpay");
                string SqlUpdate80 = "update asnreqmaster set approve_amt = "+approve_amt+" , assist_amt = "+assist_amt+" , sumpays = "+sumpays +" , balance = "+balance+" , perpay = "+perpay +@"
                                      , dead_date = to_date('"+member_dead_date.ToString("dd/MM/yyyy")+"','dd/mm/yyyy') , calculate_date = to_date('"+member_dead_date.ToString("dd/MM/yyyy")+"','dd/mm/yyyy') , dead_status = 1 where member_no = '"+HdMemberNo.Value+"' and assisttype_code = '80'";
                WebUtil.Query(SqlUpdate80);

                assist_amt = dw_65.GetItemDecimal(1, "assist_amt");
                approve_amt = dw_65.GetItemDecimal(1, "approve_amt");
                sumpays = dw_65.GetItemDecimal(1, "sumpays");
                balance = dw_65.GetItemDecimal(1, "balance");
                perpay = dw_65.GetItemDecimal(1, "perpay");
                string SqlUpdate90 = "update asnreqmaster set approve_amt = " + approve_amt + " , assist_amt = " + assist_amt + " , sumpays = " + sumpays + " , balance = " + balance + " , perpay = " + perpay + @"
                                      , dead_date = to_date('" + member_dead_date.ToString("dd/MM/yyyy") + "','dd/mm/yyyy') , calculate_date = to_date('" + member_dead_date.ToString("dd/MM/yyyy") + "','dd/mm/yyyy') , dead_status = 1 where member_no = '" + HdMemberNo.Value + "' and assisttype_code = '90'";
                WebUtil.Query(SqlUpdate90);
                

                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("บันทึกไม่สำเร็จ" + ex);
            }

        }

        public void WebSheetLoadEnd()
        {
            DwMemP.SaveDataCache();
            DwMainP.SaveDataCache();
            DwDetailP.SaveDataCache();
            dw_65.SaveDataCache();
            dw_pension.SaveDataCache();

            dt.Clear();
            ta.Close();

        }

        private void RetreiveDwMem()
        {
            DwDetailP.Reset();
            DwDetailP.InsertRow(0);
            String member_no;
            Decimal deadpay_amt = 0, sumshare = 0;

            member_no = HdMemberNo.Value;


            DwUtil.RetrieveDataWindow(DwMemP, pblPension, null, member_no);
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
            decimal sharestk_amt = 0;
            DateTime member_date, work_date, birth_date, dead_date;
            try // Set วันที่เข้าเป็นสมาชิก
            {
                string SqlShareStk = "select sharestk_amt from shsharemaster where member_no = '"+member_no+"'";
                Sdt dtShare = WebUtil.QuerySdt(SqlShareStk);
                string SQLMemberDate = "select member_date,resign_date , birth_date from mbmembmaster where member_no = '" + member_no + "'";
                Sdt dt = WebUtil.QuerySdt(SQLMemberDate);
                string SQLGetAsnreqmaster = "select req_date,dead_date , dead_status from asnreqmaster where member_no = '" + HdMemberNo.Value + "' and assisttype_code = '90'";
                Sdt dtDate = WebUtil.QuerySdt(SQLGetAsnreqmaster);
                if (dtShare.Next())
                {
                    sharestk_amt = dtShare.GetDecimal("sharestk_amt");
                    DwMainP.SetItemDecimal(1, "sharestk_amt", sharestk_amt*10);
                }
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
                        {
                            LtServerMessage.Text = WebUtil.WarningMessage("หมายเลขสมาชิกนี้ เสียชีวิตเมื่อวันที่ " + dead_tdate);
                            birth_date = dt.GetDate("birth_date");
                            dead_date = dtDate.GetDate("dead_date");
                            decimal Age = (dead_date.Year - birth_date.Year);
                            DwDetailP.SetItemDecimal(1, "member_age", Age);
                        }
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
            GetMemberDetail();
        } //2

        private void GetMemberDetail()
        {
            decimal sumpays = 0, perpay = 0, approve_amt = 0, balance = 0, assist_amt = 0, share = 0, cal_balance = 0, cal_perpay = 0;
            try
            {
                DwUtil.RetrieveDataWindow(dw_pension, pblPension, null, HdMemberNo.Value);
                DwUtil.RetrieveDataWindow(dw_65, pblPension, null, HdMemberNo.Value);
                Set_65years_cal();
            }
            catch { }
        } //3

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
                DwMainP.SetItemString(1, "req_tdate", HdDeadDate.Value);
                birth_date = DateTime.ParseExact(birth_tdate, "dd/MM/yyyy", WebUtil.TH);
                dead_date = DateTime.ParseExact(HdDeadDate.Value, "dd/MM/yyyy", WebUtil.TH);

                dw_65.SetItemDate(1, "calculate_date", dead_date);
                dw_pension.SetItemDate(1, "calculate_date", dead_date);

                decimal Age = (dead_date.Year - birth_date.Year);

                DwDetailP.SetItemDecimal(1, "member_age", Age);

                //เช็คว่าอยู่ในกองทุน65ปีแล้วหรือยัง
                try
                {
                    string checkAssist = "select count(assist_docno) from asnreqmaster where member_no = '" + HdMemberNo.Value + "' and assisttype_code ='90'";
                    Sdt ck = WebUtil.QuerySdt(checkAssist);
                    if (ck.Next())
                    {
                        decimal countAssit = ck.GetDecimal("count(assist_docno)");
                        if (countAssit == 0)
                        {
                            string sqlDeleteAsnshareperiod = "delete from asnshareperiod where member_no = '"+HdMemberNo.Value+"'";
                            WebUtil.Query(sqlDeleteAsnshareperiod);
                            string sqlDeleteAsnsharecal = "delete from asnsharecal where member_no = '" + HdMemberNo.Value + "'";
                            WebUtil.Query(sqlDeleteAsnsharecal);
                            AddToAssist();
                        }
                        else
                        {
                            ReCalculate();
                            UpdateOldMember();
                        }
                    }
                }
                catch
                {
                }

                GetMemberDetail();
                CalculatePS();
                //

            }
            catch
            {
            }
        } //4

        private void SaveSF() //Insert ข้อมูลกองทุนสวัสดิการบำนาญ
        {
            string memNo = DwUtil.GetString(DwMemP, 1, "member_no");
            String yyyynow = (state.SsWorkDate.Year + 543).ToString();
            string ass_docno90 = CmSrv.GetNewDocNo(state.SsWsPass, "ASSISTDOCNO90");
            try
            {
                string savesql = "insert into asnreqmaster (assist_docno,capital_year,member_no,assisttype_code,req_date,approve_date) values ('" + ass_docno90 + "'," + yyyynow + ",'" + memNo + "','90',to_date('" + DateTime.Now.ToString("ddMMyyyy", WebUtil.EN) + "','ddmmyyyy'),to_date('" + DateTime.Now.ToString("ddMMyyyy", WebUtil.EN) + "','ddmmyyyy'))";
                WebUtil.Query(savesql);
                dw_65.SetItemString(1, "assist_docno", ass_docno90);
                dw_65.SetItemDecimal(1, "capital_year", Convert.ToDecimal(yyyynow));
                dw_65.SetItemString(1, "member_no", memNo);
                dw_65.SetItemString(1, "assisttype_code", "90");
                //String SQLUpdateASN = "update asnreqmaster set perpay = '" + perpay + "' , sumpays ='" + sumpays + "' , balance ='" + balance + "' , assist_amt ='" + assist_amt + "',pay_date = to_date('" + DateTime.Now.ToString("ddMMyyyy", WebUtil.EN) + "','ddmmyyyy'),pay_status= " + pay_status + ",dead_date = to_date('" + member_dead_date.ToString("ddMMyyyy", WebUtil.EN) + "','ddmmyyyy'), dead_status='1' , calculate_date = to_date('" + member_dead_date.ToString("ddMMyyyy", WebUtil.EN) + "','ddmmyyyy') , approve_amt = " + approve_amt + "  where member_no = '" + HdMemberNo.Value + "' and assisttype_code ='90'";
                //WebUtil.Query(SQLUpdateASN);

            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถบันทึกข้อมูล ในกองทุนได้" + ex);
            }

        }

        private void savePS() //Insert ข้อมูลกองทุนสวัสดิการ 65 ปีมีสุข
        {
            string memNo = DwUtil.GetString(DwMemP, 1, "member_no");
            DateTime calculate_date = DwDetailP.GetItemDate(1, "member_dead_date");
            string yyyynow = (state.SsWorkDate.Year + 543).ToString();
            Decimal sharestk_amt = 0;
            string ass_docno80 = CmSrv.GetNewDocNo(state.SsWsPass, "ASSISTDOCNO80");
            try
            {
                string savesql = "insert into asnreqmaster (assist_docno,capital_year,member_no,assisttype_code,req_date,approve_date) values ('" + ass_docno80 + "'," + yyyynow + ",'" + memNo + "','80',to_date('" + calculate_date.ToString("ddMMyyyy") + "','ddmmyyyy'),to_date('" + state.SsWorkDate.ToString("ddMMyyyy") + "','ddmmyyyy'))";
                WebUtil.Query(savesql);
                dw_pension.SetItemString(1, "assist_docno", ass_docno80);
                dw_pension.SetItemDecimal(1, "capital_year", Convert.ToDecimal(yyyynow));
                dw_pension.SetItemString(1, "assisttype_code", "80");
                dw_pension.SetItemString(1, "member_no", memNo);
                sharestk_amt = DwMainP.GetItemDecimal(1, "sharestk_amt");
                string QueryInsPensionperpay = "insert into asnpensionperpay (member_no, sharestk_amt,pension_amt1,pension_amt2,pension_amt3,pension_amt4,pension_amt5,pension_amt6,pension_amt7,pension_amt8,pension_amt9,pension_amt10,withdraw_count) values ('" + memNo + "','" + sharestk_amt + "',0,0,0,0,0,0,0,0,0,0,0)";
                WebUtil.Query(QueryInsPensionperpay);
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถบันทึกข้อมูล ในกองทุนได้ : " + ex);
            }
        }

        public void CalSumShare()
        {
            string member_no = HdMemberNo.Value;
            string yeartemp = "0000", monthtemp = "01", capital_year = "0000", month = "01", day = "01", rule_seq = "1", rule_seq_temp = "0";
            decimal count = 0, share_temp = 0;
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

            string SQLStatement = "select to_char(operate_date , 'mm') as shrmonth,share_amount , operate_date from shsharestatement where member_no = '" + member_no + "' and shritemtype_code = 'SPM' and item_status <> -9 order by seq_no ASC";
            Sdt dtStatement = WebUtil.QuerySdt(SQLStatement);

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
                    monthtemp = dtStatement.GetString("shrmonth");
                    share_temp = dtStatement.GetDecimal("share_amount");
                    share_temp = share_temp * 10;
                    capital_year = dtStatement.GetDateTh("operate_date");
                    capital_year = capital_year.Substring(capital_year.Length - 4);
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
                    //j++;

                    if (monthtemp == "12")
                    {
                        InsertSumShare(share1, share2, share3, share4, share5, share6, share7, share8, share9, share10, share11, share12, capital_year, flag);
                        j = 1;
                        monthtemp = "00";
                        flag = 0;
                        share_temp = 0;
                        share1 = share2 = share3 = share4 = share5 = share6 = share7 = share8 = share9 = share10 = share11 = share12 = 0;
                    }
                }

                if (monthtemp != "00" && share_temp != 0) // กรณี มีหุ้นไม่ครบปี
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
            }

        } //5

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
                sumcal = Math.Floor(sumcal);
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

        public void cal_last()
        {
            decimal month_1 = 0, month_2 = 0, month_3 = 0, rule_seq = 0, sumyear = 0, cal_amt = 0;
            decimal month_12 = 0, month_22 = 0, month_32 = 0, rule_seq2 = 0, sumyear2 = 0, cal_amt2 = 0;//แถวบน
            decimal m1 = 0, m2 = 0, m3 = 0, m4 = 0, m5 = 0, m6 = 0, m7 = 0, m8 = 0, m9 = 0, m10 = 0, m11 = 0, m12 = 0;
            string share_year = "";
            string shortmonth = "";
            Sdt dtAsnshare;

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

                try
                {
                    if (Convert.ToInt32(share_year) <= 2546)
                    {
                        String SqlAsnshare = "select * from asnshare where member_no = '" + HdMemberNo.Value + "' and capital_year = '" + share_year + "'";
                        dtAsnshare = WebUtil.QuerySdt(SqlAsnshare);
                        if (dtAsnshare.Next())
                        {
                            m1 = dtAsnshare.GetDecimal("share_amt_1");
                            m2 = dtAsnshare.GetDecimal("share_amt_2");
                            m3 = dtAsnshare.GetDecimal("share_amt_3");
                            m4 = dtAsnshare.GetDecimal("share_amt_4");
                            m5 = dtAsnshare.GetDecimal("share_amt_5");
                            m6 = dtAsnshare.GetDecimal("share_amt_6");
                            m7 = dtAsnshare.GetDecimal("share_amt_7");
                            m8 = dtAsnshare.GetDecimal("share_amt_8");
                            m9 = dtAsnshare.GetDecimal("share_amt_9");
                            m10 = dtAsnshare.GetDecimal("share_amt_10");
                            m11 = dtAsnshare.GetDecimal("share_amt_11");
                            m12 = dtAsnshare.GetDecimal("share_amt_12");
                        }
                    }
                    else if (Convert.ToInt32(share_year) > 2546)
                    {
                        String SqlSelectShare = "select member_no , operate_date , share_amount*10 as sharemultiple , to_char(operate_date,'mm') as shortmonth from shsharestatement where to_char(operate_date,'yyyy')='" + (Convert.ToInt32(share_year) - 543) + "' and member_no = '" + HdMemberNo.Value + "' and shritemtype_code ='SPM' order by shortmonth";
                        Sdt dtStmShare = WebUtil.QuerySdt(SqlSelectShare);
                        while (dtStmShare.Next())
                        {
                            shortmonth = dtStmShare.GetString("shortmonth");
                            switch (shortmonth)
                            {
                                case "01": m1 = dtStmShare.GetDecimal("sharemultiple"); break;
                                case "02": m2 = dtStmShare.GetDecimal("sharemultiple"); break;
                                case "03": m3 = dtStmShare.GetDecimal("sharemultiple"); break;
                                case "04": m4 = dtStmShare.GetDecimal("sharemultiple"); break;
                                case "05": m5 = dtStmShare.GetDecimal("sharemultiple"); break;
                                case "06": m6 = dtStmShare.GetDecimal("sharemultiple"); break;
                                case "07": m7 = dtStmShare.GetDecimal("sharemultiple"); break;
                                case "08": m8 = dtStmShare.GetDecimal("sharemultiple"); break;
                                case "09": m9 = dtStmShare.GetDecimal("sharemultiple"); break;
                                case "10": m10 = dtStmShare.GetDecimal("sharemultiple"); break;
                                case "11": m11 = dtStmShare.GetDecimal("sharemultiple"); break;
                                case "12": m12 = dtStmShare.GetDecimal("sharemultiple"); break;
                            }
                        }
                    }
                }
                catch { }

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
                                //m9 = m10 = m11 = m12 = 0;
                                //String tmpshare_year = (Convert.ToInt32(share_year) - 543).ToString();
                                //String SqlSelectShare = "select member_no , operate_date , share_amount*10 as sharemultiple , to_char(operate_date,'mm') as shortmonth from shsharestatement where to_char(operate_date,'yyyy')='" + tmpshare_year + "' and to_char(operate_date,'mm') in ('09','10','11','12') and member_no = '" + HdMemberNo.Value + "' and shritemtype_code ='SPM' order by shortmonth";
                                //Sdt dtShare = WebUtil.QuerySdt(SqlSelectShare);
                                //while (dtShare.Next())
                                //{
                                //    shortmonth = dtShare.GetString("shortmonth");
                                //    switch (shortmonth)
                                //    {
                                //        case "09": m9 = dtShare.GetDecimal("sharemultiple"); break;
                                //        case "10": m10 = dtShare.GetDecimal("sharemultiple"); break;
                                //        case "11": m11 = dtShare.GetDecimal("sharemultiple"); break;
                                //        case "12": m12 = dtShare.GetDecimal("sharemultiple"); break;
                                //    }
                                //}
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
                                //m9 = m10 = m11 = m12 = 0;
                                //String tmpshare_year = (Convert.ToInt32(share_year) - 543).ToString();
                                //String SqlSelectShare = "select member_no , operate_date , share_amount*10 as sharemultiple , to_char(operate_date,'mm') as shortmonth from shsharestatement where to_char(operate_date,'yyyy')='" + tmpshare_year + "' and to_char(operate_date,'mm') in ('09','10','11','12') and member_no = '" + HdMemberNo.Value + "' and shritemtype_code ='SPM' order by shortmonth";
                                //Sdt dtShare = WebUtil.QuerySdt(SqlSelectShare);
                                //while (dtShare.Next())
                                //{
                                //    shortmonth = dtShare.GetString("shortmonth");
                                //    switch (shortmonth)
                                //    {
                                //        case "09": m9 = dtShare.GetDecimal("sharemultiple"); break;
                                //        case "10": m10 = dtShare.GetDecimal("sharemultiple"); break;
                                //        case "11": m11 = dtShare.GetDecimal("sharemultiple"); break;
                                //        case "12": m12 = dtShare.GetDecimal("sharemultiple"); break;
                                //    }
                                //}
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
                                //m9 = m10 = m11 = m12 = 0;
                                //String tmpshare_year = (Convert.ToInt32(share_year) - 543).ToString();
                                //String SqlSelectShare = "select member_no , operate_date , share_amount*10 as sharemultiple , to_char(operate_date,'mm') as shortmonth from shsharestatement where to_char(operate_date,'yyyy')='" + tmpshare_year + "' and to_char(operate_date,'mm') in ('09','10','11','12') and member_no = '" + HdMemberNo.Value + "' and shritemtype_code ='SPM' order by shortmonth";
                                //Sdt dtShare = WebUtil.QuerySdt(SqlSelectShare);
                                //while (dtShare.Next())
                                //{
                                //    shortmonth = dtShare.GetString("shortmonth");
                                //    switch (shortmonth)
                                //    {
                                //        case "09": m9 = dtShare.GetDecimal("sharemultiple"); break;
                                //        case "10": m10 = dtShare.GetDecimal("sharemultiple"); break;
                                //        case "11": m11 = dtShare.GetDecimal("sharemultiple"); break;
                                //        case "12": m12 = dtShare.GetDecimal("sharemultiple"); break;
                                //    }
                                //}
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
                    cal_amt = Math.Floor(cal_amt);
                    cal_amt2 = Math.Floor(cal_amt2);

                }
                else if (Convert.ToInt32(share_year) + 10 == thisyear)
                {
                    cal_amt2 = Convert.ToDecimal(Convert.ToDouble(sumyear2) * 0.2);
                    cal_amt = Convert.ToDecimal(Convert.ToDouble(sumyear) * 0.3);
                    cal_amt = Math.Floor(cal_amt);
                    cal_amt2 = Math.Floor(cal_amt2);
                }
                else if (Convert.ToInt32(share_year) + 15 == thisyear)
                {
                    cal_amt2 = Convert.ToDecimal(Convert.ToDouble(sumyear2) * 0.3);
                    cal_amt = Convert.ToDecimal(Convert.ToDouble(sumyear) * 0.35);
                    cal_amt = Math.Floor(cal_amt);
                    cal_amt2 = Math.Floor(cal_amt2);
                }
                else if (Convert.ToInt32(share_year) + 20 == thisyear)
                {
                    cal_amt2 = Convert.ToDecimal(Convert.ToDouble(sumyear2) * 0.35);
                    cal_amt = Convert.ToDecimal(Convert.ToDouble(sumyear) * 0.4);
                    cal_amt = Math.Floor(cal_amt);
                    cal_amt2 = Math.Floor(cal_amt2);
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
                    //capital_year = dtyear.GetString("min(year)");
                    //String SQLDeleteLast = "delete from asnsumshare where year > '" + capital_year + "' and member_no = '"+HdMemberNo.Value+"'";
                    //WebUtil.Query(SQLDeleteLast);
                    //string SqlupdateMasterDate = "update asnreqmaster set calculate_date = to_date('" + calculate_date.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/MM/yyyy') where member_no = '" + HdMemberNo.Value + "' and assisttype_code = '90'";
                    //WebUtil.Query(SqlupdateMasterDate);
                    capital_year = dtyear.GetString("min(year)");
                    String SQLSelectCount = "select count(year) from asnsumshare where member_no = '" + HdMemberNo.Value + "' and year = '" + capital_year + "'";
                    Sdt dtC = WebUtil.QuerySdt(SQLSelectCount);
                    if (dtC.Next())
                    {
                        if (dtC.GetDecimal("count(year)") != 2)
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
        } //9

        public void Set_65years_cal()
        {
            decimal max_pay = 0 , assist_amt = 0, share = 0, cal_balance = 0, cal_perpay = 0;
            decimal sumpays = 0, perpay = 0, approve_amt = 0, balance = 0, pay_percent = 0, maxpay_peryear = 0;

            //ดึงค่าคงที่การจ่ายต่อปี
            String SqlSelectPerpay = "select percent , maxpay_peryear from asnucfperpay where assist_code = 'SF001'";
            Sdt dtPerpay = WebUtil.QuerySdt(SqlSelectPerpay);
            if (dtPerpay.Next())
            {
                pay_percent = dtPerpay.GetDecimal("percent");
                maxpay_peryear = dtPerpay.GetDecimal("maxpay_peryear");
            }

            //ดึงกองทุนที่คำนวนได้
            assist_amt = Convert.ToDecimal(shareQuery(HdMemberNo.Value));
            dw_65.SetItemDecimal(1, "assist_amt", assist_amt);

            //ดึงจำนวนเงินที่จ่ายไปแล้ว
            try
            {
                sumpays = dw_65.GetItemDecimal(1, "sumpays");
            }
            catch
            {
                dw_65.SetItemDecimal(1, "sumpays", 0);
            }

            //หายอดคงเหลือ
            cal_balance = assist_amt - sumpays;
            if (cal_balance < 0)
                cal_balance = 0;
            dw_65.SetItemDecimal(1, "balance", cal_balance);

            //คำนวนยอดจ่ายต่อปี
            cal_perpay = assist_amt * pay_percent ;
            if (cal_perpay > maxpay_peryear)
                cal_perpay = maxpay_peryear;
            if (cal_perpay % 10 != 0)
            {
                cal_perpay = cal_perpay + (10 - (cal_perpay % 10));//ปัดเศษ
            }
            dw_65.SetItemDecimal(1, "perpay", cal_perpay);


            //เช็คว่าถ้า approve_amy = 0 ให้ approve_amt = assist_amt
            try
            {
                approve_amt = dw_65.GetItemDecimal(1, "approve_amt");
            }
            catch
            {
                approve_amt = 0;
            }
            if (approve_amt == 0)
            {
                dw_65.SetItemDecimal(1, "approve_amt", assist_amt);
            }

        }

        //Calculate บำนาญ
        private void CalculatePS()
        {
            try
            {
                //ดึงข้อมูล ค่าเงินสูงสุด และ %การจ่ายแต่ละปี
                double shareAmt = 0, year_per = 0, totalMem_days = 0.00, setDeadpay_amt = 0, percent = 0;
                int total_year = 0, maxpay_peryear = 0, year_pay = 0,max_pay = 0,memb_age=0;
                decimal approve_amt = 0, balance = 0 , sumpays = 0;
                string Tage = "";
                DateTime member_date = state.SsWorkDate, dead_date = state.SsWorkDate, birth_date = state.SsWorkDate;
                try
                {
                    sumpays = dw_pension.GetItemDecimal(1, "sumpays");
                }
                catch
                {
                    sumpays = 0;
                }
                try
                {
                    member_date = DateTime.ParseExact(DwMainP.GetItemString(1, "member_tdate"), "dd/MM/yyyy", WebUtil.TH);
                    dead_date = DateTime.ParseExact(HdDeadDate.Value, "dd/MM/yyyy", WebUtil.TH); //DateTime.ParseExact("30/09/" + DateTime.Now.Year + 543, "dd/MM/yyyy", WebUtil.TH);
                    totalMem_days = (dead_date - member_date).TotalDays;
                    total_year = Convert.ToInt32(totalMem_days / 365);
                }
                catch { }
                try
                {
                    double totalBirth_days = 0.00;
                    birth_date = DateTime.ParseExact(DwMainP.GetItemString(1, "birth_tdate"), "dd/MM/yyyy", WebUtil.TH);
                    //dead_date = DateTime.ParseExact(HdDeadDate.Value, "dd/MM/yyyy", WebUtil.TH);
                    if (dead_date.Month != 9)
                    {
                        totalBirth_days = (dead_date - birth_date).TotalDays;
                    }
                }
                catch { }

                
                try
                {
                    Tage = DwMainP.GetItemString(1, "member_years_disp");
                }
                catch { }

                //try 
                //{
                //    String SqlCheckExist = "select 1 from asnreqmaster where member_no = '"+HdMemberNo.Value+"' and assisttype_code = '80'";
                //    Sdt dtCheck = WebUtil.QuerySdt(SqlCheckExist);
                //    if (dtCheck.Next())
                //    {
                //        String SqlMembAge = @"select trunc( ftcm_calagemth( mb.member_date , am.req_date ) , 0 )   as membage " +
                //                        @"from  mbmembmaster mb , asnreqmaster am " +
                //                        "where mb.member_no = am.member_no and assisttype_code = '80' and am.member_no = '" + HdMemberNo.Value + "'";
                //        Sdt dtMembage = WebUtil.QuerySdt(SqlMembAge);
                //        if (dtMembage.Next())
                //        {
                //            memb_age = Convert.ToInt32(dtMembage.GetDecimal("membage"));
                //        }
                //    }
                //    else
                //    {
                //        if (Tage == "")
                //        {
                //            memb_age = dead_date.Year - member_date.Year;
                //        }
                //        else
                //        {
                //            memb_age = Convert.ToInt32(Tage.Replace("ปี", ""));
                //        }
                //    }
                        
                //}
                //catch 
                //{
                    if (Tage == "")
                    {
                        memb_age = dead_date.Year - member_date.Year;
                    }
                    else
                    {
                        memb_age = Convert.ToInt32(Tage.Replace("ปี", ""));
                    }
                //}

                shareAmt = Convert.ToDouble(DwMainP.GetItemDecimal(1, "sharestk_amt"));

                string sqlmaxandper = @"select max_pay,percent from asnucfpayps where minmemb_year <= " +
                      memb_age + " and maxmemb_year >" + memb_age + "";
                Sdt dt1 = WebUtil.QuerySdt(sqlmaxandper);
                if (dt1.Next())
                {
                    max_pay = Convert.ToInt32(dt1.GetDecimal("max_pay"));
                    year_per = dt1.GetDouble("percent");

                }
                setDeadpay_amt = year_per * shareAmt;

                if (setDeadpay_amt > max_pay)
                {
                    setDeadpay_amt = max_pay;
                }
                string sqlyear_per = @"select percent,year,maxpay_peryear  from asnucfperpay 
                                        where assist_code =( select max(assist_code) 
                                        from asnucfperpay where assist_code like 'PS%' )";
                Sdt dt2 = WebUtil.QuerySdt(sqlyear_per);
                if (dt2.Next())
                {
                    percent = dt2.GetDouble("percent");
                    year_pay = Convert.ToInt32(dt2.GetDecimal("year"));
                    maxpay_peryear = Convert.ToInt32(dt2.GetDecimal("maxpay_peryear"));
                }
                double perpay = setDeadpay_amt * percent;
                if (perpay >= maxpay_peryear)
                {
                    perpay = maxpay_peryear;
                }

                if (setDeadpay_amt <= 0)
                    setDeadpay_amt = 0;
                dw_pension.SetItemDecimal(1, "assist_amt", Convert.ToDecimal(setDeadpay_amt));
                dw_pension.SetItemDecimal(1, "perpay", Convert.ToDecimal(perpay));
                try
                {
                    approve_amt = dw_pension.GetItemDecimal(1, "approve_amt");
                }
                catch
                {
                    approve_amt = 0;
                }
                if (approve_amt == 0)
                {
                    dw_pension.SetItemDecimal(1, "approve_amt", Convert.ToDecimal(setDeadpay_amt));
                }
                balance = Convert.ToDecimal(setDeadpay_amt) - sumpays;
                if (balance < 0)
                    balance = 0;
                dw_pension.SetItemDecimal(1, "sumpays", sumpays);
                dw_pension.SetItemDecimal(1, "balance", balance);

            }
            catch { }

        }

        //Calculate บำนาญ

        //Calculate 65 ปีมีสุข

        public void ReCalculate()
        {
            Decimal Cal_flag = 0;
            DateTime calculate_date = state.SsWorkDate;
            String member_no = DwMemP.GetItemString(1, "member_no");
            try
            {
                calculate_date = DwDetailP.GetItemDate(1, "member_dead_date");
                Cal_flag = 1;
            }
            catch { }
            if (Cal_flag == 1)
            {
                String SqlDeleteAsnsharecal = "delete from asnsharecal where member_no = '" + member_no + "'";
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
                                where member_no = '" + member_no + @"'
	                        )
	                group by member_no , shr_year , percent 
                     ) where (quarter01 + quarter02 + quarter03) > 0 and member_no = '" + member_no + "'";
                WebUtil.Query(InsertToAsnsharecal);

            }
        } //คำนวนใหม่ ทั้งคนที่ไม่มีกองทุน และมีกองทุนอยู่แล้ว (ตามMember_no)

        public void UpdateOldMember()
        {
            String member_no = DwMemP.GetItemString(1, "member_no");
            Double assist_amt = 0, perpay = 0, balance = 0, sumpays = 0;
            Decimal pay_percent = 0, maxpay_peryear = 0;
            String SqlMembNo = "select member_no , sumpays from asnreqmaster where assisttype_code = '90' and dead_status = 0 and member_no = '" + member_no + "'";
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
                if (perpay > Convert.ToDouble(maxpay_peryear)) perpay = Convert.ToDouble(maxpay_peryear);
                balance = assist_amt - sumpays;

                String SqlUpdateAsnreqmaster = @"update asnreqmaster 
                                             set assist_amt = " + assist_amt + " , perpay = " + perpay + @"
                                             where member_no = '" + member_no + "' and assisttype_code = '90' ";
                WebUtil.Query(SqlUpdateAsnreqmaster);
            }
        } //อัพเดตข้อมูลลง Asnreqmaster ทำทุกคน

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

        } //ดึงจำนวนเงินที่คำนวนได้ ตอบกลับมาเป็น assist_amt

        public void AddToAssist()
        {
            try
            {

                DateTime slipDate = state.SsWorkDate;
                DateTime calculate_date = state.SsWorkDate;
                String member_no = DwMemP.GetItemString(1, "member_no");
                try
                {
                    calculate_date = DwDetailP.GetItemDate(1, "member_dead_date");
                }
                catch { }


                String SqlInsertToAsnPeriod = @"
                    insert into asnshareperiod
                    ( member_no , shr_year , shr_mth , shr_value )
                    select memno , shryear , shrmth , shr_amt 
            	        from (
            		        select member_No as memno , capital_year as shryear , '01' as shrmth , share_amt_1 as shr_amt 
            		        from asnshare 
            		        union
            		        select member_No as memno , capital_year as shryear , '02' as shrmth , share_amt_2 as shr_amt 
            		        from asnshare 
            		        union
            		        select member_No as memno , capital_year as shryear , '03' as shrmth , share_amt_3 as shr_amt 
            		        from asnshare 
            		        union
            		        select member_No as memno , capital_year as shryear , '04' as shrmth , share_amt_4 as shr_amt 
            		        from asnshare 
            		        union
            		        select member_No as memno , capital_year as shryear , '05' as shrmth , share_amt_5 as shr_amt 
            		        from asnshare 
            		        union
            		        select member_No as memno , capital_year as shryear , '06' as shrmth , share_amt_6 as shr_amt 
            		        from asnshare 
            		        union
            		        select member_No as memno , capital_year as shryear , '07' as shrmth , share_amt_7 as shr_amt 
            		        from asnshare 
            		        union
            		        select member_No as memno , capital_year as shryear , '08' as shrmth , share_amt_8 as shr_amt 
            		        from asnshare 
            		        union
            		        select member_No as memno , capital_year as shryear , '09' as shrmth , share_amt_9 as shr_amt 
            		        from asnshare 
            		        union
            		        select member_No as memno , capital_year as shryear , '10' as shrmth , share_amt_10 as shr_amt 
            		        from asnshare 
            		        union
            		        select member_No as memno , capital_year as shryear , '11' as shrmth , share_amt_11 as shr_amt 
            		        from asnshare 
            		        union
            		        select member_No as memno , capital_year as shryear , '12' as shrmth , share_amt_12 as shr_amt 
            		        from asnshare 
            	        ) ah where shryear || shrmth < '255001' and memno = '" + member_no + @"'
       
            	        union

            	        select member_no , to_char ( to_number( to_char( slip_date , 'yyyy' ) ) + 543 ) as shryear , to_char( slip_date , 'mm' ) as shrmth , sum( share_amount * 10 ) as shr_amt
            	        from shsharestatement 
            	        where shritemtype_code = 'SPM'
            	        and slip_date >= to_date('01/01/2007' , 'dd/mm/yyyy') and slip_date <= to_date('" + calculate_date.ToString("dd/MM") + "/" + (calculate_date.Year - 2).ToString() + @"' , 'dd/mm/yyyy')
                        and member_no = '" + member_no + @"'
            	        and item_status = 1
            	        group by member_no , to_char ( to_number( to_char( slip_date , 'yyyy' ) ) + 543 ) , to_char( slip_date , 'mm' ) ";
                WebUtil.Query(SqlInsertToAsnPeriod);

                ReCalculate();
            }

                //คำนวนกองทุนใหม่

            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        } //ดึงหุ้นมาตั้งต้น สำหรับคนที่ยังไม่เคยมีกองทุนมาก่อน

        public void CalculateNewmem(string member_no)
        {
            //ต้องดึงหุ้นทั้งหมดมาลงใน Asnsumshare และคำนวณเงินสูงสุดออกมา
            DateTime calculate_date = DwDetailP.GetItemDate(1, "member_dead_date");
            Decimal pay_percent = 0, maxpay_peryear = 0;

            String SqlSelectPerpay = "select percent , maxpay_peryear from asnucfperpay where assist_code = 'SF001'";
            Sdt dtPerpay = WebUtil.QuerySdt(SqlSelectPerpay);
            if (dtPerpay.Next())
            {
                pay_percent = dtPerpay.GetDecimal("percent");
                maxpay_peryear = dtPerpay.GetDecimal("maxpay_peryear");
            }

            Double MaxPay_amt = shareQuery(member_no);
            double perpay = MaxPay_amt * Convert.ToDouble(pay_percent);//คำนวนจ่ายต่อปี
            //เช็คจ่ายสูงสุดต่อปี
            if (perpay >= Convert.ToDouble(maxpay_peryear))
            {
                perpay = Convert.ToDouble(maxpay_peryear);
            }
            if (perpay % 10 != 0)
            {
                perpay = perpay + (10 - (perpay % 10));//ปัดเศษ
            }
            perpay = Math.Floor(perpay);
            decimal assist_year = state.SsWorkDate.Year + 543; 
            string ass_docno90 = CmSrv.GetNewDocNo(state.SsWsPass, "ASSISTDOCNO90");
            //-------------------------------------------------------------------------------------------------
            //ลงทะเบียนข้อมูล
            //-------------------------------------------------------------------------------------------------
            try
            {
                string sqlintsert = "insert into asnreqmaster (assist_docno,capital_year,member_no,assisttype_code,pay_status,assist_amt,approve_amt,balance,calculate_date,perpay,sumpays,req_date,remark,approve_date) " +
                    "values ('" + ass_docno90 + "'," + assist_year + ",'" + member_no + "','90',8," + MaxPay_amt + "," + MaxPay_amt + "," + MaxPay_amt +
                    ",to_date('" + calculate_date.ToString("dd/MM/yyyy") + "','dd/mm/yyyy')," + perpay + ",0,to_date('" + calculate_date.ToString("dd/MM/yyyy") + "','dd/mm/yyyy'),'ASS90',to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy") + "','dd/mm/yyyy'))";
                WebUtil.Query(sqlintsert);
            }
            catch
            {

            }
        } //insert ข้อมูลลง Asnreqmaster
        //Calculate 65 ปีมีสุข

        #region Report Process

        private void RunProcess()
        {
            String print_tdate = state.SsWorkDate.ToString("dd/MM/yyyy");
            app = state.SsApplication;

            if (HdReport.Value == "1")
            {
                gid = "Pension";
                rid = "PS_ASS02";
            }
            else if (HdReport.Value == "2")
            {
                gid = "Reserve";
                rid = "RE_ASS002";

                print_tdate = WebUtil.ConvertDateThaiToEng(DwMainP, "entry_tdate", null);
            }
            String member_no = HdMemberNo.Value;
            String report_date = WebUtil.ConvertDateThaiToEng(DwDetailP, "member_dead_tdate", null);

            //แปลง Criteria ให้อยู่ในรูปแบบมาตรฐาน.
            ReportHelper lnv_helper = new ReportHelper();
            lnv_helper.AddArgument(member_no, ArgumentType.String);
            lnv_helper.AddArgument(report_date, ArgumentType.DateTime);
            if (HdReport.Value == "2")
            {
                lnv_helper.AddArgument(print_tdate, ArgumentType.DateTime);
            }

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

        //step ลดหุ้น
        //

    }
}