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
    public partial class w_sheet_kt_reduce_share65 : PageWebSheet, WebSheet
    {
        protected String postPayClick;
        protected String postRetreiveDwMem;
        protected Sta ta;
        protected Sdt dt;
        TimeSpan tp;
        public void InitJsPostBack()
        {
            postRetreiveDwMem = WebUtil.JsPostBack(this, "postRetreiveDwMem");
            postPayClick = WebUtil.JsPostBack(this, "postPayClick");

        }

        public void WebSheetLoadBegin()
        {
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

            }

        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postRetreiveDwMem")
            {
                RetreiveDwMem();
            }
            else if (eventArg == "postPayClick")
            {
                Calculate();
            }
        }

        public void SaveWebSheet()
        {
            ////เช็คจำนวนการจ่าย
            string member_no = HdMemberNo.Value;

                decimal perpay = DwDetailP.GetItemDecimal(1, "perpay");
                decimal balance = DwDetailP.GetItemDecimal(1, "balance");

                //อัพเดท asnreqmaster
                try
                {
                    string upmaster = "update asnreqmaster set perpay =" + perpay + ",balance=" + balance +
                        ",calculate_date=to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy')" +
                        " where assisttype_code ='90' and member_no ='" + member_no + "'";
                    dt = WebUtil.QuerySdt(upmaster);
                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
                }
                catch
                {

                }
        }

        public void WebSheetLoadEnd()
        {
            DwMemP.SaveDataCache();
            DwMainP.SaveDataCache();
            DwDetailP.SaveDataCache();

            dt.Clear();
            ta.Close();
        }
        private void RetreiveDwMem()
        {
            DwDetailP.Reset();
            DwDetailP.InsertRow(0);

            String member_no;
            Decimal Share_amt = 0;
            Decimal Share_all=0;
            member_no = HdMemberNo.Value;
            HdShareCk.Value = "Yes";
            try
            {
                // เพิ่มค่าในช่องหุ้น
                string sqlStr = @"select sum(sumyear) as sumshare 
                        from asnsumshare
                        where member_no = '" + member_no + "'";
                dt = ta.Query(sqlStr);
                if (dt.Next())
                {
                    Share_amt = dt.GetDecimal("sumshare");
                    DwMainP.SetItemDecimal(1, "sharestk_amt", Share_amt);
                }
                else { DwMainP.SetItemDecimal(1, "sharestk_amt", 0); }

                string sqlck = "select sharestk_amt from shsharemaster where member_no='"+member_no+"'";
                Sdt ck = WebUtil.QuerySdt(sqlck);
                if(ck.Next()){
                    Share_all = ck.GetDecimal("sharestk_amt");
                    Share_all = Share_all * 10;
                }
                if((Share_all) < Share_amt){
                    HdShareCk.Value = "No";
                    LtServerMessage.Text = WebUtil.ErrorMessage("หุ้นรายเดือนมากกว่าหุ้นทั้งหมด " + Share_amt.ToString("#,##0") + " : " + Share_all.ToString("#,##0"));
                }
            }
            catch
            {

            }

            //DwMemP.Reset();
            DwUtil.RetrieveDataWindow(DwMemP, "kt_65years.pbl", null, member_no);

            try
            {
                string test = DwMemP.GetItemString(1, "member_no");
            }
            catch
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่มีเลขที่สมาชิกนี้");
                //LtAlert.Text = "<script>Alert()</script>";
                //return;
            }

            GetMemberMain();
        }//end RetreiveDwMem()

        private void GetMemberMain()
        {
            string member_no = HdMemberNo.Value;
            string member_tdate, work_tdate;
            DateTime member_date, work_date;
            try // Set วันที่เข้าเป็นสมาชิก
            {
                string SQLMemberDate = "select member_date, birth_date from mbmembmaster where member_no = '" + member_no + "'";
                Sdt dt = WebUtil.QuerySdt(SQLMemberDate);
                if (dt.Next())
                {
                    member_tdate = dt.GetDateTh("member_date");
                    HdBirthDate.Value = dt.GetDateTh("birth_date");

                    DwMainP.SetItemString(1, "member_tdate", member_tdate);
                    DwMainP.SetItemString(1, "birth_tdate", HdBirthDate.Value);
                }
            }
            catch { }
            try // Set วันที่ทำการ
            {
                work_tdate = state.SsWorkDate.Date.ToString("dd/MM/yyyy", WebUtil.TH).Substring(0, 10).Trim();

                DwMainP.SetItemString(1, "entry_tdate", work_tdate);
                DwDetailP.SetItemString(1, "calculate_tdate", work_tdate.Replace("/", ""));
            }
            catch { }

            // Set ประเภทกองทุน
            DwMainP.SetItemString(1, "assisttype_code", "กองทุนสวัสดิการบำนาญสมาชิก");
            GetMemberDetail();
        }//end GetMemberMain()

        private void GetMemberDetail()
        {
            decimal sumpays = 0, perpay, approve_amt, balance, assist_amt;
            DateTime req_date = state.SsWorkDate;
            try
            {
                string SQLGetAsnreqmaster = "select * from asnreqmaster where member_no = '" + HdMemberNo.Value + "' and assist_docno like 'SF%'";
                Sdt dtAsn = WebUtil.QuerySdt(SQLGetAsnreqmaster);
                if (dtAsn.Next())
                {
                    sumpays = dtAsn.GetDecimal("sumpays");
                    perpay = dtAsn.GetDecimal("perpay");
                    approve_amt = dtAsn.GetDecimal("approve_amt");
                    balance = dtAsn.GetDecimal("balance");
                    assist_amt = dtAsn.GetDecimal("assist_amt");
                    req_date = dtAsn.GetDate("req_date");

                    DwDetailP.SetItemDecimal(1, "sumpays", sumpays);
                    DwDetailP.SetItemDecimal(1, "perpay", perpay);
                    DwDetailP.SetItemDecimal(1, "approve_amt", approve_amt);
                    DwDetailP.SetItemDecimal(1, "balance", balance);
                    DwDetailP.SetItemDecimal(1, "assist_amt", assist_amt);
                    string SF_date = req_date.ToString("dd/MM/yyyy", WebUtil.TH).Substring(0, 10).Trim();
                    DwMainP.SetItemString(1, "req_tdate", SF_date);
                }
                else
                {
                    DwDetailP.SetItemDecimal(1, "sumpays", 0);
                    DwDetailP.SetItemDecimal(1, "perpay", 0);
                    DwDetailP.SetItemDecimal(1, "approve_amt", 0);
                    DwDetailP.SetItemDecimal(1, "balance", 0);
                    DwDetailP.SetItemDecimal(1, "assist_amt", 0);
                }
            }
            catch { }

        }//end GetMemberDetail()

        private void Calculate()
        {
            if (HdShareCk.Value == "Yes")
            {
                //ดึงค่าของเงินที่ได้รับไปแล้ว
                double sumpays_amt = 0;
                double approve_amt = 0; //เงินที่ได้รับครั้งแรกเต็มจำนวน
                DateTime req_date = state.SsWorkDate;
                DateTime calculate_date = state.SsWorkDate;
                string member_no = HdMemberNo.Value;
                try
                {
                    string sqlsumpays = @"select sumpays, approve_amt,req_date,calculate_date
                           from asnreqmaster 
                           where member_no = '" + member_no + "' and assist_docno like 'SF%'";
                    dt = WebUtil.QuerySdt(sqlsumpays);
                    if (dt.Next())
                    {
                        sumpays_amt = Convert.ToDouble(dt.GetDecimal("sumpays"));
                        approve_amt = Convert.ToDouble(dt.GetDecimal("approve_amt"));
                        req_date = dt.GetDate("req_date");
                        calculate_date = dt.GetDate("calculate_date");
                    }
                }
                catch { sumpays_amt = 0; }
                //คำนวณตามเงื่อนไข
                Double MaxPay_amt = Cal(member_no);
                if (MaxPay_amt > approve_amt)
                {
                    MaxPay_amt = approve_amt;
                }
                double perpay = MaxPay_amt * 0.1;//คำนวนจ่ายต่อปี
                //เช็คจ่ายสูงสุดต่อปี
                if (perpay >= 10000)
                {
                    perpay = 10000;
                }
                if (perpay % 10 > 1)
                {
                    perpay = perpay + (10 - (perpay % 10));
                }
                //------------------------------------------------------------------------------------------
                //Set ค่าแสดง
                Double balance = MaxPay_amt - sumpays_amt;
                DwDetailP.SetItemDecimal(1, "assist_amt", Convert.ToDecimal(MaxPay_amt));
                DwDetailP.SetItemDecimal(1, "perpay", Convert.ToDecimal(perpay));
                DwDetailP.SetItemDecimal(1, "balance", Convert.ToDecimal(balance));
            }
            else
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถคำนวณได้เนื่องจากจำนวนหุ้นรวมน้อยกว่าจำนวนหุ้นที่ใช้คำนวณ");
            }
            
        }//end Calculate()
        public Double Cal(string member_no)
        {
                decimal sumcal_amt = 0;
                string sqlcal65 = "";
                Sdt selectsum;
                try
                {
                    sqlcal65 = "select sum(cal_amt) from asnsumshare where member_no='" + member_no + "'";
                    selectsum = WebUtil.QuerySdt(sqlcal65);
                    if (selectsum.Next())
                    {
                        sumcal_amt = selectsum.GetDecimal("sum(cal_amt)");
                    }
                    
                }
                catch { }
                return Convert.ToDouble(sumcal_amt);
        }
    //public Double Cal(string member_no)
    //    {
    //        DateTime calculate_date = DateTime.Now;
    //        DateTime entry_date = DateTime.Now;
    //        Double sum = 0;
    //        Double calpay = 0;
    //        Double share_amount = 0;

    //        string sqlshare = "select share_amount,entry_date from shsharestatement where shritemtype_code ='SPM' and member_no='"+member_no+"'";
    //        Sdt share = WebUtil.QuerySdt(sqlshare);
    //        while (share.Next())
    //        {
    //            share_amount = Convert.ToDouble(share.GetDecimal("share_amount"));
    //            entry_date = share.GetDate("entry_date");

    //            tp = calculate_date - entry_date;
    //            Double share_year = (tp.TotalDays / 365);

    //            if (share_year > 2 && share_year < 5)
    //            {
    //                calpay = share_amount * 0.1;
    //            }
    //            else if (share_year > 5 && share_year < 10)
    //            {
    //                calpay = share_amount * 0.2;
    //            }
    //            else if (share_year > 10 && share_year < 15)
    //            {
    //                calpay = share_amount * 0.3;
    //            }
    //            else if (share_year > 15 && share_year < 20)
    //            {
    //                calpay = share_amount * 0.35;
    //            }
    //            else if (share_year > 20)
    //            {
    //                calpay = share_amount * 0.4;
    //            }
    //            sum = sum + calpay;
    //        }//end while

    //        return sum;

    //    }
    }
}