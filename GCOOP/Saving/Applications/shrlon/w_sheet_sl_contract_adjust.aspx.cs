using System;
using CoreSavingLibrary;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using CoreSavingLibrary.WcfNCommon;
using CoreSavingLibrary.WcfNShrlon;
using Sybase.DataWindow;
using DataLibrary;
using CoreSavingLibrary.WcfNBusscom;


namespace Saving.Applications.shrlon
{
    public partial class w_sheet_sl_contract_adjust : PageWebSheet, WebSheet
    {
        Sta ta;
        private DwThDate tdw_main;
        private DwThDate tdw_intspc;
        private n_shrlonClient ShrlonSv;
        protected String callContractAdjust;
        protected String getMemberNo;
        protected String itemChangedReload;
        protected String postNew;
        protected String checkRightColl;
        protected String postMemberFromDlg;
        protected String getBank;
        protected String changeColl;
        protected String jsGetMemberCollno;
        protected String jsCollCondition;
        protected String jsCollInitP;
        protected string jsCheckCollmastrightBalance;

        private n_shrlonClient shrlonService;
        private n_busscomClient BusscomService;
        private n_commonClient commonService;

        private String[] mem_coll;

        #region WebSheet Members

        public void InitJsPostBack()
        {
            jsCollCondition = WebUtil.JsPostBack(this, "jsCollCondition");
            jsCollInitP = WebUtil.JsPostBack(this, "jsCollInitP");
            jsCheckCollmastrightBalance = WebUtil.JsPostBack(this, "jsCheckCollmastrightBalance");
            jsGetMemberCollno = WebUtil.JsPostBack(this, "jsGetMemberCollno");
            postMemberFromDlg = WebUtil.JsPostBack(this, "postMemberFromDlg");
            callContractAdjust = WebUtil.JsPostBack(this, "callContractAdjust");
            getMemberNo = WebUtil.JsPostBack(this, "getMemberNo");
            itemChangedReload = WebUtil.JsPostBack(this, "itemChangedReload");
            postNew = WebUtil.JsPostBack(this, "postNew");
            checkRightColl = WebUtil.JsPostBack(this, "checkRightColl");
            getBank = WebUtil.JsPostBack(this, "getBank");
            changeColl = WebUtil.JsPostBack(this, "changeColl");
            tdw_main = new DwThDate(dw_main, this);
            tdw_main.Add("contadjust_date", "contadjust_tdate");
            tdw_intspc = new DwThDate(dw_detcontspc, this);
            tdw_intspc.Add("effective_date", "effective_date");
        }

        public void WebSheetLoadBegin()
        {
            this.ConnectSQLCA();
            dw_main.SetTransaction(sqlca);
            dw_detpay.SetTransaction(sqlca);
            dw_coll.SetTransaction(sqlca);
            dw_detcont.SetTransaction(sqlca);
            dw_detcontspc.SetTransaction(sqlca);
            dw_con.SetTransaction(sqlca);
            HdMemcoopId.Value = state.SsCoopId;

            try
            {
                shrlonService = wcf.NShrlon;
                commonService = wcf.NCommon;
                BusscomService = wcf.NBusscom;
            }
            catch
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ติดต่อ Web Service ไม่ได้");
                return;
            }

            if (!IsPostBack)
            {
                dw_main.InsertRow(1);
                dw_detpay.InsertRow(1);

                dw_coll.InsertRow(1);
                dw_detcont.InsertRow(1);
                dw_detcontspc.InsertRow(1);
                dw_main.SetItemDateTime(1, "contadjust_date", state.SsWorkDate);
                tdw_main.Eng2ThaiAllRow();
                dw_con.InsertRow(1);

                dw_main.SetItemString(1, "concoop_id", state.SsCoopId);
                dw_main.SetItemString(1, "coop_id", state.SsCoopId);

                dw_coll.SetItemString(1, "coop_id", state.SsCoopId);
                dw_detcont.SetItemString(1, "coop_id", state.SsCoopId);
                dw_detcontspc.SetItemString(1, "coop_id", state.SsCoopId);
                dw_detpay.SetItemString(1, "coop_id", state.SsCoopId);

                WebUtil.RetrieveDDDW(dw_con, "loanpay_bank", "sl_contract_adjust.pbl", null);
                WebUtil.RetrieveDDDW(dw_con, "loanpay_branch", "sl_contract_adjust.pbl", null);
            }
            else
            {
                dw_main.RestoreContext();
                dw_detpay.RestoreContext();
                dw_coll.RestoreContext();
                dw_detcont.RestoreContext();
                dw_detcontspc.RestoreContext();
                //dw_con.RestoreContext();
                this.RestoreContextDw(dw_con);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "callContractAdjust")
            {
                GetContractAdjust();
            }
            else if (eventArg == "jsCollCondition")
            {
                JsCollCondition();
            }
            else if (eventArg == "jsCheckCollmastrightBalance")
            {
                JsCheckCollmastrightBalance();
            }
            else if (eventArg == "jsGetMemberCollno")
            {
                JsGetMemberCollno();
            }
            else if (eventArg == "jsCollInitP")
            {
                JsCollInitP();
            }
            else if (eventArg == "getMemberNo")
            {
                GetMemberNo();
            }
            else if (eventArg == "itemChangedReload") { }
            else if (eventArg == "postNew")
            {
                JSNew();
            }
            else if (eventArg == "checkRightColl")
            {
                //เรียกเว็บเซอร์วิส เพื่อตรวจสอบรายละเอียดค้ำประกัน
                CheckCollRight();
            }
            else if (eventArg == "postMemberFromDlg")
            {
                JspostMemberFromDlg();
            }
            else if (eventArg == "changeColl")
            {
                String ref_collno;
                String loancolltype_code;
                int row = Convert.ToInt32(HfMembDetRow.Value);
                String sql = "";
                Sdt dt;
                try { ref_collno = dw_coll.GetItemString(row, "ref_collno"); }
                catch { ref_collno = ""; }
                try { loancolltype_code = dw_coll.GetItemString(row, "loancolltype_code"); }
                catch { loancolltype_code = ""; }

                if (loancolltype_code == "03")
                {
                    sql = @"  
                SELECT      DPDEPTMASTER.DEPTACCOUNT_NO,            DPDEPTMASTER.DEPTACCOUNT_NAME,            DPDEPTMASTER.PRNCBAL  
                FROM        MBMEMBMASTER,            DPDEPTMASTER  
                WHERE       MBMEMBMASTER.MEMBER_NO = DPDEPTMASTER.MEMBER_NO     and
                            DPDEPTMASTER.DEPTACCOUNT_NO = '" + ref_collno + "' ";
                    dt = WebUtil.QuerySdt(sql);
                    if (dt.Next())
                    {
                        dw_coll.SetItemString(row, "ref_collno", dt.GetString("DEPTACCOUNT_NO"));
                        dw_coll.SetItemString(row, "description", dt.GetString("DEPTACCOUNT_NAME"));
                        dw_coll.SetItemDecimal(row, "coll_balance", dt.GetDecimal("PRNCBAL"));
                    }
                }
                else if (loancolltype_code == "04")
                {
                    sql = @"  
                SELECT      LNCOLLMASTER.COLLMAST_NO,  LNCOLLMASTER.COLLMAST_DESC,     LNCOLLMASTER.MORTGAGE_PRICE  
                FROM        LNCOLLMASTMEMCO,           LNCOLLMASTER  
                WHERE       LNCOLLMASTER.COLLMAST_NO = LNCOLLMASTMEMCO.COLLMAST_NO     and 
                            LNCOLLMASTER.COLLMAST_NO = '" + ref_collno + "' ";
                    dt = WebUtil.QuerySdt(sql);
                    if (dt.Next())
                    {
                        dw_coll.SetItemString(row, "ref_collno", dt.GetString("COLLMAST_NO"));
                        dw_coll.SetItemString(row, "description", dt.GetString("COLLMAST_DESC"));
                        dw_coll.SetItemDecimal(row, "coll_balance", dt.GetDecimal("MORTGAGE_PRICE"));
                    }
                }
            }
            else if (eventArg == "getBank")
            {
                //if (dw_con.RowCount > 1) { 
                //for(int i = 1; i<=dw_con.RowCount;i)
                //}
                String loanpay_bank = dw_con.GetItemString(1, "loanpay_bank");
                //dw_con.SetItemString(1, "loanpay_bank", loanpay_bank);
                WebUtil.RetrieveDDDW(dw_con, "loanpay_branch", "sl_contract_adjust.pbl", loanpay_bank);
                //dw_con.Reset();
            }

        }

        private decimal of_getpercentcollmast(string as_coopid, string as_loantype, string as_colltype, string as_collmasttype)
        {
            decimal percent_collmast = 0;
            try
            {
                string sql_collperc = "select coll_percent from lnloantypecolluse where coop_id = '" + as_coopid + "' and loantype_code = '" + as_loantype + "' and  loancolltype_code  =  '" + as_colltype + "' and  collmasttype_code  = '" + as_collmasttype + "'";

                Sdt dt = WebUtil.QuerySdt(sql_collperc);
                if (dt.Next())
                {
                    percent_collmast = dt.GetDecimal("coll_percent");

                }
                else
                {
                    percent_collmast = 1;
                }
            }
            catch
            {
                percent_collmast = 0;

            }
            return percent_collmast;
        }

        private int JsCheckMangrtColl(int row, string ref_collno)
        {
            LtServerMessagecoll.Text = "";
            string mangrtpermgrp_code = "01";
            string loantype = dw_main.GetItemString(1, "loantype_code");
            string membno = dw_main.GetItemString(1, "member_no");
            //string memcoop_id = dw_main.GetItemString(1, "coop_id");
            string memcoop_id = state.SsCoopId;
            string sql_lngrtlntype = " select cntmangrtnum_flag,cntmangrtval_flag,grtman_type, grtman_amt, grtman_allmax, mangrtpermgrp_code,countcontgrt_code,cntmangrtnum_type from lnloantype where loantype_code = '" + loantype + "'";
            Sdt dtlnt = WebUtil.QuerySdt(sql_lngrtlntype);
            decimal lntgrtcountmemglag = 0, lntgrtvalmemglag = 0, grtman_type = 0, grtman_allmax = 0;
            if (dtlnt.Next())
            {
                lntgrtcountmemglag = dtlnt.GetDecimal("cntmangrtnum_flag");
                lntgrtvalmemglag = dtlnt.GetDecimal("cntmangrtval_flag");
                grtman_type = dtlnt.GetDecimal("grtman_type");
                grtman_allmax = dtlnt.GetDecimal("grtman_amt");
                mangrtpermgrp_code = dtlnt.GetString("mangrtpermgrp_code");
            }


            string sql_lnconst = "select grtright_contflag, grtright_memflag, grtright_contract, grtright_member from lnloanconstant ";
            Sdt dt = WebUtil.QuerySdt(sql_lnconst);
            decimal grtchkcont_flag = 0, grtchkmem_flag = 0, grtcountcont = 0, grtcountmem = 0;
            if (dt.Next())
            {
                grtchkcont_flag = dt.GetDecimal("grtright_contflag");
                grtchkmem_flag = dt.GetDecimal("grtright_memflag");
                grtcountcont = dt.GetDecimal("grtright_contract");
                grtcountmem = dt.GetDecimal("grtright_member");

            }
            string contno_clr = "", contclr_all = "";
            decimal clear_flag = 0;
            int k = 0;

            dt.Dispose();
            //            string sql_memcoll = @" select  b.member_no , a.ref_collno , a.coll_status, a.loancontract_no ,
            //                                   b.principal_balance from  lncontcoll a, lncontmaster b 
            //                                    where a.loancontract_no = b.loancontract_no and b.principal_balance > 0 
            //                                    and a.coll_status = 1  and a.ref_collno = '" + ref_collno + @"' and 
            //                                   a.loancontract_no not in ( " + contclr_all + @")  order by b.member_no ";

            if ((contclr_all.Length <= 7) || (contclr_all.Trim() == "")) { contclr_all = "'00000'"; }

            string sql_memcoll = @" SELECT LNCONTMASTER.MEMBER_NO as member_no,   
             LNCONTMASTER.LOANCONTRACT_NO as loancontract_no,   
             LNCONTMASTER.PRINCIPAL_BALANCE as principal_balance ,
             LNCONTCOLL.REF_COLLNO as  ref_collno,   
             LNCONTCOLL.DESCRIPTION as DESCRIPTION ,   
             LNCONTCOLL.COLL_AMT as COLL_AMT ,   
             LNCONTCOLL.COLL_PERCENT as COLL_PERCENT ,   
            MBMEMBMASTER.MEMB_NAME,
			MBMEMBMASTER.MEMB_SURNAME,
             LNCONTCOLL.BASE_PERCENT as BASE_PERCENT ,'Contno'
            FROM LNCONTCOLL,   
                 LNCONTMASTER  ,MBMEMBMASTER
           WHERE ( LNCONTCOLL.COOP_ID = LNCONTMASTER.COOP_ID ) and  
                    ( MBMEMBMASTER.MEMBER_NO = LNCONTMASTER.MEMBER_NO) AND
                 ( LNCONTCOLL.LOANCONTRACT_NO = LNCONTMASTER.LOANCONTRACT_NO ) and  
                 ( ( LNCONTCOLL.LOANCOLLTYPE_CODE in ( '01') ) AND  
                 ( LNCONTMASTER.PRINCIPAL_BALANCE > 0 ) AND LNCONTMASTER.LOANCONTRACT_NO not in (" + contclr_all + @") and 
                 ( LNCONTCOLL.REF_COLLNO = '" + ref_collno + @"'  ) )      
        Union
         SELECT LNREQLOAN.MEMBER_NO  as member_no ,   
                 LNREQLOAN.LOANREQUEST_DOCNO as loancontract_no ,   
                 LNREQLOAN.LOANREQUEST_AMT as  principal_balance,
                 LNREQLOANCOLL.REF_COLLNO as  ref_collno ,   
                 LNREQLOANCOLL.DESCRIPTION  as DESCRIPTION ,   
                 LNREQLOANCOLL.COLL_AMT as COLL_AMT ,   
                 LNREQLOANCOLL.COLL_PERCENT as COLL_PERCENT,   
                MBMEMBMASTER.MEMB_NAME,
			        MBMEMBMASTER.MEMB_SURNAME,
                 LNREQLOANCOLL.BASE_PERCENT as BASE_PERCENT  , 'Lnreqloan'
            FROM LNREQLOAN,   
                 LNREQLOANCOLL  ,MBMEMBMASTER
           WHERE ( LNREQLOAN.COOP_ID = LNREQLOANCOLL.COOP_ID ) and  
                    ( MBMEMBMASTER.MEMBER_NO = LNREQLOAN.MEMBER_NO) AND
                 ( LNREQLOAN.LOANREQUEST_DOCNO = LNREQLOANCOLL.LOANREQUEST_DOCNO ) and  
                 ( ( LNREQLOANCOLL.LOANCOLLTYPE_CODE = '01' ) AND  
                 ( LNREQLOAN.LOANREQUEST_STATUS = 8 ) AND  
                 ( LNREQLOANCOLL.REF_COLLNO =  '" + ref_collno + @"'  ) ) order by member_no ";
            Sdt dt_coll = WebUtil.QuerySdt(sql_memcoll);

            if (grtchkcont_flag == 1 && dt_coll.GetRowCount() >= grtcountcont)
            {
                //ตรวจสอบการค้ำประกันเกินสัญญาที่ระบุ
                LtServerMessagecoll.Text = WebUtil.WarningMessage("ผู้ค้ำสมาชิกเลขที่ # " + ref_collno + "  ได้ค้ำประกันเกินกว่าที่กำหนดไว้ค้ำประกันได้สูงสุด  " + grtcountcont.ToString() + " สัญญา ค้ำประกันไปแล้ัว " + dt_coll.GetRowCount().ToString());
                dw_coll.SetItemString(row, "ref_collno", "");
                dw_coll.SetItemString(row, "description", "");
                dw_coll.SetItemDecimal(row, "coll_balance", 0);
                return 1;
            }
            string member_nochk, prememno = "";
            int collmem = 0;
            decimal principal_balance = 0, ldc_collbalance = 0, ldc_colluse = 0, ldc_collpercent = 0, ldc_collamt = 0;
            string ls_msg = "", memb_name = "", memb_surname = "";
            while (dt_coll.Next())
            {
                member_nochk = dt_coll.GetString("member_no");
                memb_name = dt_coll.GetString("member_no");
                memb_surname = dt_coll.GetString("member_no");
                principal_balance = dt_coll.GetDecimal("principal_balande");
                ldc_collpercent = dt_coll.GetDecimal("COLL_PERCENT");
                ldc_colluse = principal_balance * ldc_collpercent;
                ldc_collamt += ldc_colluse;
                if (prememno != member_nochk)
                {
                    collmem++;
                    ls_msg += " , ค้ำประกันสมาชิกเลขที่  " + member_nochk + " " + memb_name.Trim() + " " + memb_surname.Trim() + "/n";
                }
                if (membno == member_nochk) { collmem--; }
                prememno = member_nochk;

            }
            if (grtchkmem_flag == 1 && collmem >= grtcountmem && lntgrtcountmemglag == 1)
            {

                //ตรวจสอบการค้ำประกันเกินจำนวนคนที่ระบุ
                LtServerMessagecoll.Text = WebUtil.ErrorMessage("ผู้ค้ำสมาชิกเลขที่ # " + ref_collno + "  ได้ค้ำประกันสมาชิกเกินกว่าที่กำหนดไว้ค้ำประกันได้สูงสุด  " + "\n" + grtcountmem.ToString() + " ค้ำประกันได้ไม่ไม่เกิน " + collmem.ToString() + "\n " + ls_msg);
                // Ltdividen.Text = WebUtil.ErrorMessage(ls_msg);

                dw_coll.SetItemString(row, "ref_collno", "");
                dw_coll.SetItemString(row, "description", "");
                dw_coll.SetItemDecimal(row, "coll_balance", 0);
                return 1; //-1
            }

            //เช็คสิทธิค้ำยามยอดเงิน
            if (lntgrtvalmemglag == 1)
            {
                String sqlstr = @"  SELECT       MBMEMBMASTER.BIRTH_DATE,
                                             MBMEMBMASTER.dropgurantee_flag,
                                             MBMEMBMASTER.REMARK,
                                             MBMEMBMASTER.MEMBER_DATE,   
                                             MBMEMBMASTER.WORK_DATE,   
                                             MBMEMBMASTER.RETRY_DATE,   
                                             MBMEMBMASTER.SALARY_AMOUNT,   
                                             MBMEMBMASTER.MEMBTYPE_CODE,   
                                             MBUCFMEMBTYPE.MEMBTYPE_DESC,                                             
                                             mbucfprename.prename_desc||mbmembmaster.memb_name||'   '||mbmembmaster.memb_surname as member_name
                               FROM MBMEMBMASTER,   
                                     MBUCFMEMBGROUP,   
                                     MBUCFPRENAME,
                                     MBUCFMEMBTYPE
                                  
                               WHERE ( MBUCFMEMBGROUP.MEMBGROUP_CODE = MBMEMBMASTER.MEMBGROUP_CODE ) and                                     
                                     ( MBMEMBMASTER.PRENAME_CODE = MBUCFPRENAME.PRENAME_CODE ) and
                                     ( MBMEMBMASTER.MEMBTYPE_CODE = MBUCFMEMBTYPE.MEMBTYPE_CODE ) and 
                                     ( MBMEMBMASTER.COOP_ID = MBUCFMEMBGROUP.COOP_ID ) and  
                                      ( mbmembmaster.member_no = '" + ref_collno + @"' ) AND                                 
                                     MBMEMBMASTER.COOP_ID   ='" + memcoop_id + @"'    ";
                Sdt dtgrt = WebUtil.QuerySdt(sqlstr);
                DateTime ldtm_member = DateTime.Now;
                while (dtgrt.Next())
                {
                    string membtype_code = dtgrt.GetString("membtype_code");
                    decimal ldc_salary = dtgrt.GetDecimal("salary_amount");
                    try
                    {
                        ldtm_member = dtgrt.GetDate("MEMBER_DATE");
                    }
                    catch { }
                    DateTime ldtm_reqloan = dw_main.GetItemDateTime(1, "contadjust_date");
                    Decimal member_age = 0;// = BusscomService.of_cal_yearmonth(state.SsWsPass, ldtm_member, DateTime.Now);

                    member_age = (ldtm_reqloan.Year - ldtm_member.Year) * 12;
                    member_age += (ldtm_reqloan.Month - ldtm_member.Month);
                    if (ldtm_member.Day > ldtm_reqloan.Day) { member_age--; }
                    //decimal age_year = Math.Truncate(member_age / 12);
                    //decimal age_month = (member_age % 12) / 100;

                    //member_age = age_year + age_month;

                    //คำนวณสิทธิค้ำประกัน
                    Decimal coll_amt = CalloanpermissColl(state.SsWsPass, member_age, ldc_salary, mangrtpermgrp_code);
                    ldc_collbalance = coll_amt - ldc_collamt;
                    if (ldc_collbalance < 0) { ldc_collbalance = 0; }
                    // dw_coll.SetItemDecimal(row, "coll_amt", coll_amt);
                    dw_coll.SetItemDecimal(row, "coll_balance", ldc_collbalance);
                    dw_coll.SetItemDecimal(row, "coll_useamt", ldc_collamt);
                    return 1; //-1

                }



            }
            dt_coll.Dispose();
            return 1;
        }
        public Decimal CalloanpermissColl(String wsPass, Decimal memtime, Decimal ldc_salary, String mangrtypemrp_code)
        {

            Decimal ldc_maxcredit = 1500000, ldc_maxcreditcoll = 0;
            String sqlStrcredit = @"  SELECT MANGRTPERMGRP_CODE,   
                                             SEQ_NO,   
                                             STARTSHARE_AMT,   
                                             ENDSHARE_AMT,   
                                             STARTMEMBER_TIME,   
                                             ENDMEMBER_TIME,   
                                             PERCENTSHARE,   
                                             PERCENTSALARY,   
                                             MAXGRT_AMT,   
                                             START_SALARY,   
                                             END_SALARY  
                                        FROM LNGRPMANGRTPERMDET   
                                        WHERE  MANGRTPERMGRP_CODE ='" + mangrtypemrp_code + @"'  
                                             and STARTMEMBER_TIME <=" + memtime + " and ENDMEMBER_TIME >" + memtime + @" 
                                    ORDER BY  MANGRTPERMGRP_CODE,   
                                             SEQ_NO ";
            Sdt dt = ta.Query(sqlStrcredit);
            while (dt.Next())
            {
                ldc_maxcredit = ldc_salary * dt.GetDecimal("PERCENTSALARY");
                ldc_maxcreditcoll = dt.GetDecimal("MAXGRT_AMT");
                if (ldc_maxcredit > ldc_maxcreditcoll) { ldc_maxcredit = ldc_maxcreditcoll; }
            }


            return ldc_maxcredit;

        }
        private string of_getrerydate(DateTime birthDate)
        {
            string retry_date = "";
            string coop_id = state.SsCoopControl;
            try
            {
                Sdt dt = WebUtil.QuerySdt("select	retry_age,retry_month,retry_day from	cmcoopmaster where coop_id ='" + coop_id + "' ");
                // dt.next คือเลื่อนเคอร์เซอร์เพื่อไปหาค่าแถวถัดไป
                if (dt.Next())
                {   //เอาค่า +ปีที่เกษียณ  + วันเกิด
                    int retry_age = dt.GetInt32("retry_age");
                    int retry_month = dt.GetInt32("retry_month");
                    int retry_day = dt.GetInt32("retry_day");
                    //int retry_age = Convert.ToInt16(dt.Rows[0]["retry_age"]);
                    //int retry_month = Convert.ToInt16(dt.Rows[0]["retry_month"]);
                    //int retry_day = Convert.ToInt16(dt.Rows[0]["retry_day"]);
                    int year = birthDate.Year + retry_age;
                    int month = birthDate.Month;
                    int day = birthDate.Day;
                    int loop_day = 0;
                    //ตั้งค่าวันที่สิ้นสุดของแต่ล่ะเดือน

                    if (retry_day == 0)
                    {
                        int[] daysinmonth = new int[12];
                        for (int i = 0; i < 12; i++)
                        {
                            if (i == 1)
                            {
                                daysinmonth[i] = 28;
                            }
                            else
                            {

                                if (i == 0 || i == 2 || i == 4 || i == 6 || i == 7 || i == 9 || i == 11)
                                {
                                    daysinmonth[i] = 31;
                                }
                                else
                                { daysinmonth[i] = 30; }
                            }

                        }
                        for (int i = 0; i < 12; i++)
                        {
                            if (day > daysinmonth[i])
                            {   //เช็ควันที่สิ้นสุดของเดือน กุมภาพันธ์
                                if (i == 1)
                                {

                                    if ((year % 4 == 0 && year % 100 != 0) || year % 400 == 0)
                                    {
                                        day = 29;
                                    }
                                }
                            }
                            else
                            {
                                if (month == i + 1)
                                {
                                    day = daysinmonth[i];
                                }
                            }

                        }
                        if (retry_month != 0)
                        {
                            loop_day = daysinmonth[retry_month - 1];
                            day = loop_day;
                        }
                    }
                    else
                    {
                        day = retry_day;
                    }

                    if (retry_month != 0)
                    {
                        //เช็คเกษียณครบรอบ
                        if (month > retry_month)
                        {
                            year = year + 1;
                        }
                        month = retry_month;
                    }
                    return day.ToString("00") + '-' + month.ToString("00") + '-' + year.ToString("0000");
                }
                else
                {
                    return retry_date;
                }
            }
            catch
            {
                return retry_date;
            }
        }


        private void JsGetMemberCollno()
        {
            LtServerMessagecoll.Text = "";
            try
            {
                int row = Convert.ToInt16(HdRefcollrow.Value);
                String ls_memcoopid;
                String ref_collno = WebUtil.MemberNoFormat(HdRefcoll.Value);
                Decimal period_payamt = dw_detpay.GetItemDecimal(1, "period_payamt");
                String description = "";
                String membtype_code = "";
                Decimal ldc_salary = 0, retry_age = 0;
                DateTime ldtm_member = DateTime.Now;
                DateTime ldtm_birth = DateTime.Now;
                DateTime ldtm_retry = DateTime.Now;
                Decimal lndropgrantee_flag = 0;
                decimal maxcollval_mbtype1 = 0;
                decimal maxcollval_mbtype2 = 0;
                String remark = "";
                String loancolltype_code = dw_coll.GetItemString(row, "loancolltype_code");
                string loantype_code = dw_main.GetItemString(1, "loantype_code");
                decimal loanmembtype = dw_main.GetItemDecimal(1, "member_type");
                string mangrtpermgrp_code = "";
                DateTime ldtm_reqloan = dw_main.GetItemDateTime(1, "contadjust_date");

                string sqllntype = @"select  maxcollval_mbtype1, maxcollval_mbtype2,mangrtpermgrp_code  from lnloantype  where loantype_code ='" + loantype_code + "' ";
                Sdt dtlntype = WebUtil.QuerySdt(sqllntype);
                if (dtlntype.Next())
                {
                    maxcollval_mbtype1 = dtlntype.GetDecimal("maxcollval_mbtype1");
                    maxcollval_mbtype2 = dtlntype.GetDecimal("maxcollval_mbtype2");
                    mangrtpermgrp_code = dtlntype.GetString("mangrtpermgrp_code");
                }

                HdCollmaxval1.Value = maxcollval_mbtype1.ToString();
                HdCollmaxval2.Value = maxcollval_mbtype2.ToString();

                decimal max_collamt = 0;
                //JsSetDeptnodefault(1); wa
                if (loancolltype_code == "01")
                {
                    dw_coll.SetItemString(row, "ref_collno", ref_collno);

                    decimal coll_memperc = of_getpercentcollmast(state.SsCoopControl, loantype_code, loancolltype_code, "00");
                    ls_memcoopid = state.SsCoopId;
                    dw_coll.SetItemString(row, "coop_id", state.SsCoopControl);

                    //mong ตรวจสอบการค้ำประกัน จำนวนที่ค้ำประกันได้

                    if (JsCheckMangrtColl(row, ref_collno) == 1)
                    {

                        String sqlstr = @"  SELECT       MBMEMBMASTER.BIRTH_DATE, MBMEMBMASTER.MEMBER_TYPE,
                                             MBMEMBMASTER.dropgurantee_flag,
                                             MBMEMBMASTER.REMARK,
                                             MBMEMBMASTER.MEMBER_DATE,   
                                             MBMEMBMASTER.WORK_DATE,   
                                             MBMEMBMASTER.RETRY_DATE,   
                                             MBMEMBMASTER.SALARY_AMOUNT,   MBMEMBMASTER.MEMBER_TYPE,
                                             MBMEMBMASTER.MEMBTYPE_CODE,   
                                             MBUCFMEMBTYPE.MEMBTYPE_DESC,                                             
                                             mbucfprename.prename_desc||mbmembmaster.memb_name||'   '||mbmembmaster.memb_surname as member_name
                               FROM MBMEMBMASTER,   
                                     MBUCFMEMBGROUP,   
                                     MBUCFPRENAME,
                                     MBUCFMEMBTYPE
                                  
                               WHERE ( MBUCFMEMBGROUP.MEMBGROUP_CODE = MBMEMBMASTER.MEMBGROUP_CODE ) and                                     
                                     ( MBMEMBMASTER.PRENAME_CODE = MBUCFPRENAME.PRENAME_CODE ) and
                                     ( MBMEMBMASTER.MEMBTYPE_CODE = MBUCFMEMBTYPE.MEMBTYPE_CODE ) and 
                                     ( MBMEMBMASTER.COOP_ID = MBUCFMEMBGROUP.COOP_ID ) and  
                                      ( mbmembmaster.member_no = '" + ref_collno + @"' ) AND                                 
                                     MBMEMBMASTER.COOP_ID   ='" + state.SsCoopControl + @"'    ";
                        Sdt dt = WebUtil.QuerySdt(sqlstr);

                        decimal collmembertype = 1;
                        decimal member_type = 1;
                        while (dt.Next())
                        {
                            lndropgrantee_flag = dt.GetDecimal("dropgurantee_flag");
                            collmembertype = dt.GetDecimal("member_type");
                            description = dt.GetString("member_name");
                            try
                            {
                                remark = dt.GetString("remark");
                            }
                            catch { remark = ""; }
                            membtype_code = dt.GetString("membtype_code");
                            ldc_salary = dt.GetDecimal("salary_amount");
                            try
                            {
                                ldtm_birth = dt.GetDate("BIRTH_DATE");
                            }
                            catch { }
                            try
                            {
                                ldtm_retry = dt.GetDate("RETRY_DATE");
                            }
                            catch
                            {
                                ///<หาวันที่เกษียณ>
                                ldtm_retry = Convert.ToDateTime(of_getrerydate(ldtm_birth));
                            }

                            member_type = dt.GetDecimal("member_type");
                        }

                        if (loanmembtype == 1 && collmembertype == 2)
                        {
                            LtServerMessagecoll.Text = WebUtil.ErrorMessage("ใบคำขอกู้นี้สมาชิกสามัญกู้ ไม่สามารถนำสมาชิกสมทบ มาค้ำประกันได้");
                        }
                        DateTime loanrequest_date = dw_main.GetItemDateTime(1, "contadjust_date");

                        mem_coll = GetMembercollwa(state.SsWsPass, ls_memcoopid, ref_collno, loanrequest_date, mangrtpermgrp_code, loantype_code); //, 

                        // หา max value ก่อน
                        if (member_type == 1)
                        {
                            max_collamt = Convert.ToDecimal(HdCollmaxval1.Value);
                            if (max_collamt == 0) { max_collamt = Convert.ToDecimal(mem_coll[2]); };
                        }
                        else
                        {
                            max_collamt = Convert.ToDecimal(HdCollmaxval2.Value);
                            if (max_collamt == 0) { max_collamt = Convert.ToDecimal(mem_coll[2]); };
                        }

                        // mong
                        retry_age = (ldtm_retry.Year - ldtm_reqloan.Year) * 12;
                        retry_age += (ldtm_retry.Month - ldtm_reqloan.Month);

                        //retry_age = age_year + age_month;
                        if (mem_coll[3].Length > 9)
                        {
                            LtServerMessagecoll.Text = WebUtil.WarningMessage(mem_coll[3]);
                        }
                        if (mem_coll[6].Length > 9)
                        {
                            LtServerMessagecoll.Text += WebUtil.WarningMessage(mem_coll[6]);
                        }

                        if (retry_age < 0)
                        {
                            LtServerMessagecoll.Text += WebUtil.ErrorMessage("ผู้ค้ำ  " + ref_collno + "  เป็นสมาชิกที่เกษียณแล้ว กรณาตรวจสอบด้วย !!!! ");
                        }
                        if (retry_age < period_payamt)
                        {
                            LtServerMessagecoll.Text += WebUtil.WarningMessage("งวดเกษียณของผู้ค้ำ ท. " + ref_collno + "  น้อยกว่างวดการส่งผ้งวดการส่งชำระ  " + retry_age.ToString() + "  <  " + period_payamt.ToString() + " !!!");
                            dw_coll.SetItemString(row, "ref_collno", ref_collno);
                            dw_coll.SetItemString(row, "description", description);
                            dw_coll.SetItemDecimal(row, "coll_balance", Convert.ToDecimal(mem_coll[2]));
                            dw_coll.SetItemDecimal(row, "coll_amt", Convert.ToDecimal(mem_coll[2]));
                            dw_coll.SetItemDecimal(row, "base_percent", coll_memperc);
                            // dw_coll.SetItemDecimal(row, "MAXCOLL_PERIOD", max_collamt);
                        }
                        else
                        {
                            try
                            {
                                if (mem_coll[0] != "")
                                {
                                    dw_coll.SetItemString(row, "ref_collno", ref_collno);
                                    dw_coll.SetItemString(row, "description", description);
                                    dw_coll.SetItemDecimal(row, "coll_balance", Convert.ToDecimal(mem_coll[2]));
                                    dw_coll.SetItemDecimal(row, "coll_amt", Convert.ToDecimal(mem_coll[2]));
                                    dw_coll.SetItemDecimal(row, "base_percent", coll_memperc);
                                    // dw_coll.SetItemDecimal(row, "MAXCOLL_PERIOD", max_collamt);
                                    HUseamt.Value = mem_coll[2];
                                }
                                else
                                {
                                    dw_coll.SetItemString(row, "ref_collno", ref_collno);
                                    dw_coll.SetItemString(row, "description", description);
                                    dw_coll.SetItemDecimal(row, "coll_balance", Convert.ToDecimal(mem_coll[2]));
                                    dw_coll.SetItemDecimal(row, "coll_amt", Convert.ToDecimal(mem_coll[2]));
                                    dw_coll.SetItemDecimal(row, "base_percent", coll_memperc);
                                    //  dw_coll.SetItemDecimal(row, "MAXCOLL_PERIOD", max_collamt);
                                    HUseamt.Value = mem_coll[2];
                                }
                            }
                            catch
                            {

                            }
                        }
                        JsCheckCollmastrightBalance();
                    }
                }
                else if (loancolltype_code == "02")
                {
                    decimal coll_shrperc = of_getpercentcollmast(state.SsCoopControl, loantype_code, loancolltype_code, "00");
                    if (coll_shrperc == 0) { coll_shrperc = Convert.ToDecimal(0.90); }
                    decimal sharestk_value = 0;
                    String sqlSharestk = "select sharestk_amt from shsharemaster where member_no = '" + dw_main.GetItemString(1, "member_no") + "'";
                    Sdt dt = WebUtil.QuerySdt(sqlSharestk);
                    if (dt.Next()) { sharestk_value = dt.GetDecimal("sharestk_amt"); }
                    decimal coll_amt = sharestk_value * coll_shrperc;
                    dw_coll.SetItemString(row, "ref_collno", dw_main.GetItemString(1, "member_no"));
                    dw_coll.SetItemString(row, "description", "ทุนเรือนหุ้น" + dw_main.GetItemString(1, "compute_1"));
                    dw_coll.SetItemDecimal(row, "coll_amt", sharestk_value);
                    dw_coll.SetItemDecimal(row, "coll_balance", coll_amt);
                    dw_coll.SetItemDecimal(row, "use_amt", 0);
                    dw_coll.SetItemDecimal(row, "base_percent", coll_shrperc);

                    JsCheckCollmastrightBalance();
                }
                else if (loancolltype_code == "03")
                {
                    decimal coll_dpperc = of_getpercentcollmast(state.SsCoopControl, loantype_code, loancolltype_code, "00");
                    if (coll_dpperc == 0) { coll_dpperc = Convert.ToDecimal(0.90); }
                    decimal coll_amtmast = dw_coll.GetItemDecimal(row, "coll_amt");
                    decimal coll_amt = coll_amtmast * coll_dpperc;
                    dw_coll.SetItemDecimal(row, "coll_amt", coll_amtmast);
                    dw_coll.SetItemDecimal(row, "coll_balance", coll_amt);
                    dw_coll.SetItemDecimal(row, "use_amt", coll_amt);
                    dw_coll.SetItemDecimal(row, "base_percent", coll_dpperc);
                    JsCheckCollmastrightBalance();
                }
                else if (loancolltype_code == "04")
                {
                    //dw_coll.SetItemDecimal(row, "coll_amt", coll_amt);
                    //dw_coll.SetItemDecimal(row, "coll_balance", coll_amt);
                    //dw_coll.SetItemDecimal(row, "use_amt", coll_amt);
                    //dw_coll.SetItemDecimal(row, "base_percent", coll_mastperc);
                }
                JsCollCondition();
            }
            catch (Exception ex)
            {
                //LtServerMessage.Text = WebUtil.ErrorMessage("JsGetMemberCollno===>" + ex); 
            }
        }
        public String[] GetMembercollwa(string wsPass, String memcoop_id, String ref_collno, DateTime dltm_workdate, string collmbgrp_code, string loantype_code)
        {
            String description = "";
            String membtype_code = "";
            Decimal ldc_salary = 0, member_age = 0, ldc_incomeetc = 0;
            DateTime ldtm_member = DateTime.Now;
            DateTime ldtm_birth = DateTime.Now;
            DateTime ldtm_retry = DateTime.Now;
            Decimal lndropgrantee_flag = 0;
            String remark = "";
            String[] mem_coll = new String[7];
            String sqlstr = @"  SELECT       MBMEMBMASTER.BIRTH_DATE,
                                             MBMEMBMASTER.dropgurantee_flag,
                                             MBMEMBMASTER.REMARK,
                                             MBMEMBMASTER.MEMBER_DATE,   
                                             MBMEMBMASTER.WORK_DATE,   
                                             MBMEMBMASTER.RETRY_DATE,   
                                             MBMEMBMASTER.SALARY_AMOUNT,
                                             MBMEMBMASTER.INCOMEETC_AMT,
                                             SHSHAREMASTER.LAST_PERIOD,  
                                             MBMEMBMASTER.MEMBTYPE_CODE,   
                                             MBUCFMEMBTYPE.MEMBTYPE_DESC,
                                             MBMEMBMASTER.MEMBER_TYPE,                                             
                                             mbucfprename.prename_desc||mbmembmaster.memb_name||'   '||mbmembmaster.memb_surname as member_name
                               FROM MBMEMBMASTER,   
                                     MBUCFMEMBGROUP,  SHSHAREMASTER , 
                                     MBUCFPRENAME,
                                     MBUCFMEMBTYPE
                                  
                               WHERE ( MBUCFMEMBGROUP.MEMBGROUP_CODE = MBMEMBMASTER.MEMBGROUP_CODE ) and                                     
                                     ( MBMEMBMASTER.PRENAME_CODE = MBUCFPRENAME.PRENAME_CODE ) and
                                     ( MBMEMBMASTER.MEMBTYPE_CODE = MBUCFMEMBTYPE.MEMBTYPE_CODE ) and 
                                     (  mbmembmaster.member_no = SHSHAREMASTER.MEMBER_NO ) AND ( MBMEMBMASTER.COOP_ID = SHSHAREMASTER.COOP_ID ) and 
                                     ( MBMEMBMASTER.COOP_ID = MBUCFMEMBGROUP.COOP_ID ) and  
                                      ( mbmembmaster.member_no = '" + ref_collno + @"' ) AND                                 
                                     MBMEMBMASTER.COOP_ID   ='" + memcoop_id + @"'    ";
            Sdt dt = ta.Query(sqlstr);
            while (dt.Next())
            {
                lndropgrantee_flag = dt.GetDecimal("dropgurantee_flag");
                description = dt.GetString("member_name");
                try
                {
                    remark = dt.GetString("remark");
                }
                catch { remark = ""; }
                membtype_code = dt.GetString("membtype_code");
                ldc_salary = dt.GetDecimal("salary_amount");
                ldc_incomeetc = dt.GetDecimal("INCOMEETC_AMT");
                ldc_salary += ldc_incomeetc;
                try
                {
                    ldtm_member = dt.GetDate("MEMBER_DATE");
                }
                catch { }
                decimal lastshare_period = 0;
                try
                {
                    lastshare_period = dt.GetDecimal("last_period");
                }
                catch { lastshare_period = shrlonService.of_cal_yearmonth(wsPass, ldtm_member, dltm_workdate) * 12; }

                member_age = lastshare_period;// bus.of_cal_yearmonth(wsPass, ldtm_member, dltm_workdate) * 12;
            }

            String mangrtypemrp_code = "";

            mangrtypemrp_code = collmbgrp_code;
            Decimal coll_balance = CalloanpermissColl(wsPass, member_age, ldc_salary, mangrtypemrp_code);
            if (coll_balance > 0)
            {
                mem_coll[0] = ref_collno;
                mem_coll[1] = description;
                mem_coll[2] = coll_balance.ToString();
                mem_coll[3] = "";
                mem_coll[4] = member_age.ToString();
                mem_coll[5] = lndropgrantee_flag.ToString();
                mem_coll[6] = remark;

            }
            else
            {
                mem_coll[0] = "";
                mem_coll[1] = "";
                mem_coll[2] = "";
                mem_coll[3] = "สิทธิ์ค้ำไม่ถึงเกณฑ์ที่กำหนด";
                mem_coll[4] = member_age.ToString();
                mem_coll[5] = lndropgrantee_flag.ToString();
                mem_coll[6] = remark;

            }

            //มง เพิ่ม กรณีสมาชิกสมทบ ให้เปลี่ยนรหัสสิทธิค้ำประกัน
            //decimal member_type = dt.GetDecimal("member_type");
            //if (member_type == 2 && (loantype_code == "21" || loantype_code == "22" || loantype_code == "28" || loantype_code == "29" || loantype_code == "30" || loantype_code == "C1" || loantype_code == "C2" || loantype_code == "C3")) { mangrtypemrp_code = "22"; }

            ta.Close();
            return mem_coll;
        }
        private void JsCheckCollmastrightBalance()
        {
            try
            {
                int row = Convert.ToInt16(HdRowNumber.Value);
                string ref_collno = dw_coll.GetItemString(row, "ref_collno");
                string loantype = dw_main.GetItemString(1, "loantype_code");
                string membno = dw_main.GetItemString(1, "member_no");
                string memcoop_id = state.SsCoopId;
                string loancolltype_code = dw_coll.GetItemString(row, "loancolltype_code");
                string contno_clr = "", contclr_all = "";
                decimal clear_flag = 0;
                int k = 0;

                if ((contclr_all.Length <= 7) || (contclr_all.Trim() == "")) { contclr_all = "'00000'"; }

                string sql_chkcoll = "";
                if (loancolltype_code == "01")
                {
                    sql_chkcoll = @" SELECT LNCONTMASTER.MEMBER_NO as member_no,   
            LNCONTMASTER.LOANTYPE_CODE as loantype_code, 
             LNCONTMASTER.LOANCONTRACT_NO as loancontract_no,   
             LNCONTMASTER.PRINCIPAL_BALANCE +  LNCONTMASTER.withdrawable_amt as principal_balance ,
             LNCONTCOLL.REF_COLLNO as  ref_collno,   
             LNCONTCOLL.DESCRIPTION as DESCRIPTION ,   
             LNCONTCOLL.COLL_AMT as COLL_AMT ,   
             LNCONTCOLL.COLL_PERCENT as COLL_PERCENT , 
             LNCONTCOLL.BASE_PERCENT as BASE_PERCENT ,'Contno'
            FROM LNCONTCOLL,   
                  LNCONTMASTER  ,lngrpmangrtperm, lnloantype
           WHERE ( LNCONTCOLL.COOP_ID = LNCONTMASTER.COOP_ID ) and lnloantype.loantpe_code = lncontmaster.loantype_code and   lnloantype.mangrtpermgrp_code = lngrpmangrtperm.mangrtpermgrp_code and 
                 ( LNCONTCOLL.LOANCONTRACT_NO = LNCONTMASTER.LOANCONTRACT_NO ) and  
                 ( ( LNCONTCOLL.LOANCOLLTYPE_CODE = '" + loancolltype_code + @"' ) AND  
                 ( LNCONTMASTER.PRINCIPAL_BALANCE +  LNCONTMASTER.withdrawable_amt > 0 ) AND LNCONTMASTER.LOANCONTRACT_NO not in (" + contclr_all + @") and 
                 ( LNCONTCOLL.REF_COLLNO = '" + ref_collno + @"'  ) )      
        Union
         SELECT LNREQLOAN.MEMBER_NO  as member_no ,   
                LNREQLOAN.LOANTYPE_CODE as loantype_code,
                 LNREQLOAN.LOANREQUEST_DOCNO as loancontract_no ,   
                 LNREQLOAN.LOANREQUEST_AMT as  principal_balance,
                 LNREQLOANCOLL.REF_COLLNO as  ref_collno ,   
                 LNREQLOANCOLL.DESCRIPTION  as DESCRIPTION ,   
                 LNREQLOANCOLL.COLL_AMT as COLL_AMT ,   
                 LNREQLOANCOLL.COLL_PERCENT as COLL_PERCENT,   
                 LNREQLOANCOLL.BASE_PERCENT as BASE_PERCENT  , 'Lnreqloan'
            FROM LNREQLOAN,   
                 LNREQLOANCOLL , lngrpmangrtperm , lnloantype
           WHERE ( LNREQLOAN.COOP_ID = LNREQLOANCOLL.COOP_ID ) and lnloantype.loantpe_code = lncontmaster.loantype_code and   lnloantype.mangrtpermgrp_code = lngrpmangrtperm.mangrtpermgrp_code and 
                 ( LNREQLOAN.LOANREQUEST_DOCNO = LNREQLOANCOLL.LOANREQUEST_DOCNO )  and  
                 ( ( LNREQLOANCOLL.LOANCOLLTYPE_CODE = '" + loancolltype_code + @"' ) AND  
                 ( LNREQLOAN.LOANREQUEST_STATUS = 8 ) AND  
                 ( LNREQLOANCOLL.REF_COLLNO =  '" + ref_collno + @"'  ) )  ";

                }
                else
                {

                    sql_chkcoll = @" SELECT LNCONTMASTER.MEMBER_NO as member_no,   
                            LNCONTMASTER.LOANTYPE_CODE as loantype_code, 
                             LNCONTMASTER.LOANCONTRACT_NO as loancontract_no,   
                             LNCONTMASTER.PRINCIPAL_BALANCE +  LNCONTMASTER.withdrawable_amt as principal_balance ,
                             LNCONTCOLL.REF_COLLNO as  ref_collno,   
                             LNCONTCOLL.DESCRIPTION as DESCRIPTION ,   
                             LNCONTCOLL.COLL_AMT as COLL_AMT ,   
                             LNCONTCOLL.COLL_PERCENT as COLL_PERCENT , 
                             LNCONTCOLL.BASE_PERCENT as BASE_PERCENT ,'Contno'
                            FROM LNCONTCOLL,   
                                 LNCONTMASTER  
                           WHERE ( LNCONTCOLL.COOP_ID = LNCONTMASTER.COOP_ID ) and   
                                 ( LNCONTCOLL.LOANCONTRACT_NO = LNCONTMASTER.LOANCONTRACT_NO ) and  
                                 ( ( LNCONTCOLL.LOANCOLLTYPE_CODE = '" + loancolltype_code + @"' ) AND  
                                 ( LNCONTMASTER.PRINCIPAL_BALANCE +  LNCONTMASTER.withdrawable_amt > 0 ) AND LNCONTMASTER.LOANCONTRACT_NO not in (" + contclr_all + @") and 
                                 ( LNCONTCOLL.REF_COLLNO = '" + ref_collno + @"'  ) )      
                        Union
                         SELECT LNREQLOAN.MEMBER_NO  as member_no ,   
                                LNREQLOAN.LOANTYPE_CODE as loantype_code,
                                 LNREQLOAN.LOANREQUEST_DOCNO as loancontract_no ,   
                                 LNREQLOAN.LOANREQUEST_AMT as  principal_balance,
                                 LNREQLOANCOLL.REF_COLLNO as  ref_collno ,   
                                 LNREQLOANCOLL.DESCRIPTION  as DESCRIPTION ,   
                                 LNREQLOANCOLL.COLL_AMT as COLL_AMT ,   
                                 LNREQLOANCOLL.COLL_PERCENT as COLL_PERCENT,   
                                 LNREQLOANCOLL.BASE_PERCENT as BASE_PERCENT  , 'Lnreqloan'
                            FROM LNREQLOAN,   
                                 LNREQLOANCOLL 
                           WHERE ( LNREQLOAN.COOP_ID = LNREQLOANCOLL.COOP_ID ) and  
                                 ( LNREQLOAN.LOANREQUEST_DOCNO = LNREQLOANCOLL.LOANREQUEST_DOCNO ) and  
                                 ( ( LNREQLOANCOLL.LOANCOLLTYPE_CODE = '" + loancolltype_code + @"' ) AND  
                                 ( LNREQLOAN.LOANREQUEST_STATUS = 8 ) AND  
                                 ( LNREQLOANCOLL.REF_COLLNO =  '" + ref_collno + @"'  ) )  ";
                }
                Sdt dt_coll = WebUtil.QuerySdt(sql_chkcoll);


                decimal principal_balance = 0, ldc_basepercent = 0, ldc_colluse = 0, ldc_collpercent = 0, ldc_collamt = 0, ldc_collamtold = 0;
                decimal ldc_sumusecoll = 0, ldc_collbalance1 = 0;
                decimal coll_balance = 0, ldc_basepercenloan = 0;
                ldc_basepercenloan = dw_coll.GetItemDecimal(row, "base_percent");

                coll_balance = dw_coll.GetItemDecimal(row, "coll_amt");

                while (dt_coll.Next())
                {
                    string loantype_code = dt_coll.GetString("loantype_code");
                    principal_balance = dt_coll.GetDecimal("principal_balance");
                    ldc_collamtold = dt_coll.GetDecimal("coll_amt");
                    ldc_collpercent = dt_coll.GetDecimal("COLL_PERCENT");

                    ldc_basepercent = dt_coll.GetDecimal("BASE_PERCENT");

                    ldc_colluse = Math.Round(principal_balance * ldc_collpercent / ldc_basepercent, 2);

                    ldc_sumusecoll += ldc_colluse;
                }
                coll_balance = coll_balance - ldc_sumusecoll;
                decimal percen_coll = dw_coll.GetItemDecimal(row, "base_percent");
                coll_balance = Math.Round(coll_balance * percen_coll, 2);
                //  ldc_collbalance1 = coll_balance * ldc_basepercenloan;
                if (coll_balance > 0)
                {
                    dw_coll.SetItemDecimal(row, "coll_balance", coll_balance);
                    dw_coll.SetItemDecimal(row, "use_amt", coll_balance);
                    dw_coll.SetItemDecimal(row, "coll_useamt", ldc_sumusecoll);
                }
            }
            catch { }

            try
            {
                JsCollCondition();
            }
            catch { }
        }

        private decimal TruncateDecimal(decimal value, int precision)
        {
            decimal step = (decimal)Math.Pow(10, precision);
            int tmp = (int)Math.Truncate(step * value);
            return tmp / step;
        }

        private void JsCollCondition()
        {
            try
            {
                string ls_loantype = dw_main.GetItemString(1, "loantype_code");
                Decimal coll_balance = 0;
                string loancolltype_code = "";
                Decimal coll_use = 0;
                double collpercent_use = 0.00;
                Decimal per90 = new Decimal(0.9);
                decimal sharestk_value = 0;
                String refcollno = "";
                decimal maxcollval_mbtype1 = 0;
                decimal maxcollval_mbtype2 = 0;
                String sqlSharestk = "select sharestk_amt from shsharemaster where member_no = '" + dw_main.GetItemString(1, "member_no") + "'";
                Sdt dt = WebUtil.QuerySdt(sqlSharestk);
                if (dt.Next()) { sharestk_value = dt.GetDecimal("sharestk_amt"); }
                Decimal loanrequest, loanrequestbal = 0, colluseest_amt = 0;
                loanrequest = dw_main.GetItemDecimal(1, "principal_balance");
                if (loanrequest == 0) { loanrequest = dw_main.GetItemDecimal(1, "loanapprove_amt"); }

                string sqllntype = @"select  maxcollval_mbtype1, maxcollval_mbtype2  from lnloantype  where loantype_code ='" + ls_loantype + "' ";
                Sdt dtlntype = WebUtil.QuerySdt(sqllntype);
                if (dtlntype.Next())
                {
                    maxcollval_mbtype1 = dtlntype.GetDecimal("maxcollval_mbtype1");
                    maxcollval_mbtype2 = dtlntype.GetDecimal("maxcollval_mbtype2");
                }

                decimal coll_newbalance = 0;
                int i = 0;
                loanrequestbal = loanrequest;

                //สิทธิยอดหลักทรัพย์ให้เต็มก่อน
                string[] coll_mast = new string[3] { "04", "03", "02" };

                int li_find = 0;
                int rowc = 1;
                coll_newbalance = loanrequest;
                decimal max_collamt = 0, member_type = 0;

                //เตลียร์ ข้อมูลเป็นศูนย์ก่อน
                for (int kc = 1; kc <= dw_coll.RowCount; kc++)
                {
                    dw_coll.SetItemDecimal(kc, "use_amt", 0);
                }

                for (int mrow = 0; mrow <= Convert.ToInt16(2); mrow++) //coll_mast.GetUpperBound()
                {
                    string colltype = coll_mast[mrow];
                    li_find = dw_coll.FindRow("loancolltype_code = '" + colltype + "' ", rowc, dw_coll.RowCount);
                    //แทรก ค่าแถวของหลักทรัพย์นั้น ที่มีค่าน้อยที่สุดมาก่อน
                    //and coll_balance - use_amt > 0
                    decimal mincollbal = 0, colltemp = 0, colluse = 0;
                    int krow = 0;
                    string colltype_temp = "";

                    if (colltype == "04")
                    {
                        try { refcollno = dw_coll.GetItemString(li_find, "ref_collno"); }
                        catch { }

                        String sqlstr = @"  SELECT    MBMEMBMASTER.MEMBER_TYPE  FROM MBMEMBMASTER
                               WHERE ( mbmembmaster.member_no = '" + refcollno + @"' ) AND                                 
                                     MBMEMBMASTER.COOP_ID   ='" + state.SsCoopControl + @"'    ";
                        Sdt dtMemType = WebUtil.QuerySdt(sqlstr);

                        if (dtMemType.Next())
                        {
                            member_type = dtMemType.GetDecimal("member_type");
                        }

                        for (int kk = 1; kk <= dw_coll.RowCount; kk++)
                        {
                            colltemp = dw_coll.GetItemDecimal(kk, "coll_balance");
                            colluse = dw_coll.GetItemDecimal(kk, "use_amt");
                            colltype_temp = dw_coll.GetItemString(kk, "loancolltype_code");

                            if (colltemp - colluse <= 0) { continue; }
                            if (colltype == colltype_temp)
                            {
                                if (kk == 1 || mincollbal == 0)
                                {
                                    mincollbal = colltemp;
                                    krow = kk;
                                }
                                else if (mincollbal > 0 && colltemp > 0)
                                {
                                    mincollbal = colltemp;
                                    krow = kk;
                                }
                                li_find = krow;
                            }

                            ///
                            //มง เพิ่ม กรณีสมาชิกสมทบ ให้เปลี่ยนรหัสสิทธิค้ำประกัน
                            if (member_type == 1)
                            {
                                max_collamt = maxcollval_mbtype1;
                                if (max_collamt == 0) { max_collamt = Convert.ToDecimal(mem_coll[2]); };
                            }
                            else
                            {
                                max_collamt = maxcollval_mbtype2;
                                if (max_collamt == 0) { max_collamt = Convert.ToDecimal(mem_coll[2]); };
                            }
                            if (coll_newbalance > 0)
                            {

                                try { coll_balance = dw_coll.GetItemDecimal(li_find, "coll_balance"); }
                                catch { coll_balance = 0; }
                                if (coll_balance >= coll_newbalance)
                                {
                                    coll_use = coll_newbalance;
                                    coll_newbalance = 0;
                                    try { dw_coll.SetItemDecimal(li_find, "use_amt", coll_use); }
                                    catch { }
                                }
                                else
                                {
                                    coll_use = coll_balance;
                                    coll_newbalance -= coll_use;
                                    try { dw_coll.SetItemDecimal(li_find, "use_amt", coll_use); }
                                    catch { }
                                }
                            }
                            collpercent_use = Convert.ToDouble(TruncateDecimal(Convert.ToDecimal(coll_use / loanrequest), 4));
                            try { dw_coll.SetItemDouble(li_find, "coll_percent", collpercent_use); }
                            catch { }
                            li_find++;
                            if (li_find > dw_coll.RowCount && coll_newbalance <= 0) { break; }
                            li_find = dw_coll.FindRow("loancolltype_code = '" + colltype + "' and use_amt = 0 ", 1, dw_coll.RowCount);
                        }
                    }
                    else
                    {
                        while (li_find > 0 && coll_newbalance > 0)
                        {
                            try { refcollno = dw_coll.GetItemString(li_find, "ref_collno"); }
                            catch { }

                            String sqlstr = @"  SELECT    MBMEMBMASTER.MEMBER_TYPE  FROM MBMEMBMASTER
                               WHERE ( mbmembmaster.member_no = '" + refcollno + @"' ) AND                                 
                                     MBMEMBMASTER.COOP_ID   ='" + state.SsCoopControl + @"'    ";
                            Sdt dtMemType = WebUtil.QuerySdt(sqlstr);
                            if (dtMemType.Next())
                            {
                                member_type = dtMemType.GetDecimal("member_type");
                            }


                            for (int kk = 1; kk <= dw_coll.RowCount; kk++)
                            {
                                colltemp = dw_coll.GetItemDecimal(kk, "coll_balance");
                                colluse = dw_coll.GetItemDecimal(kk, "use_amt");
                                colltype_temp = dw_coll.GetItemString(kk, "loancolltype_code");

                                if (colltemp - colluse <= 0) { continue; }
                                if (colltype == colltype_temp)
                                {
                                    if (kk == 1 || mincollbal == 0)
                                    {
                                        mincollbal = colltemp;
                                        krow = kk;
                                    }
                                    else if (mincollbal > 0 && colltemp > 0)
                                    {
                                        mincollbal = colltemp;
                                        krow = kk;
                                    }
                                    li_find = krow;
                                }
                            }
                            ///
                            //มง เพิ่ม กรณีสมาชิกสมทบ ให้เปลี่ยนรหัสสิทธิค้ำประกัน
                            if (member_type == 1)
                            {
                                max_collamt = maxcollval_mbtype1;
                                if (max_collamt == 0) { max_collamt = Convert.ToDecimal(mem_coll[2]); };
                            }
                            else
                            {
                                max_collamt = maxcollval_mbtype2;
                                if (max_collamt == 0) { max_collamt = Convert.ToDecimal(mem_coll[2]); };
                            }
                            // max_collamt = dw_coll.GetItemDecimal(li_find, "maxcoll_period");

                            if (coll_newbalance > 0)
                            {

                                try { coll_balance = dw_coll.GetItemDecimal(li_find, "coll_balance"); }
                                catch { coll_balance = 0; }
                                if (coll_balance >= coll_newbalance)
                                {
                                    coll_use = coll_newbalance;
                                    coll_newbalance = 0;
                                    try { dw_coll.SetItemDecimal(li_find, "use_amt", coll_use); }
                                    catch { }
                                }
                                else
                                {
                                    coll_use = coll_balance;
                                    coll_newbalance -= coll_use;
                                    try { dw_coll.SetItemDecimal(li_find, "use_amt", coll_use); }
                                    catch { }
                                }
                            }
                            collpercent_use = Convert.ToDouble(TruncateDecimal(Convert.ToDecimal(coll_use / loanrequest), 4));
                            try { dw_coll.SetItemDouble(li_find, "coll_percent", collpercent_use); }
                            catch { }

                            li_find++;
                            if (li_find > dw_coll.RowCount && coll_newbalance <= 0) { break; }
                            li_find = dw_coll.FindRow("loancolltype_code = '" + colltype + "' and use_amt = 0 ", 1, dw_coll.RowCount);

                        }
                    }
                }
                //หาว่ามีแถวใช้คนค้ำเหลือกี่แภว
                int row = dw_coll.RowCount;
                decimal coll_01 = 0;
                string ref_collno = "";
                for (i = 0; i < row; i++)
                {

                    loancolltype_code = dw_coll.GetItemString(i + 1, "loancolltype_code");
                    try
                    {

                        ref_collno = dw_coll.GetItemString(i + 1, "ref_collno");
                    }
                    catch { ref_collno = " "; }

                    if (loancolltype_code == "01" && ref_collno.Length > 3) { coll_01++; }

                }
                if (coll_01 > 0 && coll_newbalance > 0)
                {
                    colluseest_amt = coll_newbalance / coll_01;

                    li_find = dw_coll.FindRow("loancolltype_code = '01'", 1, dw_coll.RowCount);

                    while (li_find > 0 && coll_newbalance > 0)
                    {
                        if (coll_newbalance > 0)
                        {

                            coll_balance = dw_coll.GetItemDecimal(li_find, "coll_balance");
                            if (member_type == 1)
                            {
                                max_collamt = Convert.ToDecimal(HdCollmaxval1.Value);
                                if (max_collamt == 0) { max_collamt = Convert.ToDecimal(mem_coll[2]); };
                            }
                            else
                            {
                                max_collamt = Convert.ToDecimal(HdCollmaxval2.Value);
                                if (max_collamt == 0) { max_collamt = Convert.ToDecimal(mem_coll[2]); };
                            }
                            //max_collamt = dw_coll.GetItemDecimal(li_find, "MAXCOLL_PERIOD");
                            if (coll_balance > max_collamt) { coll_balance = max_collamt; }

                            if (colluseest_amt >= coll_balance)
                            {
                                if (coll_balance > max_collamt) { coll_balance = max_collamt; }
                                coll_use = coll_balance;

                            }
                            else
                            {
                                coll_use = colluseest_amt;

                            }
                            dw_coll.SetItemDecimal(li_find, "use_amt", coll_use);
                            coll_newbalance -= coll_use;
                            collpercent_use = Convert.ToDouble(TruncateDecimal(Convert.ToDecimal(coll_use / loanrequest), 4));
                            dw_coll.SetItemDouble(li_find, "coll_percent", collpercent_use);
                        }
                        li_find++;
                        if (li_find > row) { break; }
                        li_find = dw_coll.FindRow("loancolltype_code = '01'", li_find, row);

                    }

                }
                if (coll_newbalance > 0)
                {
                    for (i = 0; i < row; i++)
                    {

                        if (coll_newbalance <= 0) { continue; }
                        loancolltype_code = dw_coll.GetItemString(i + 1, "loancolltype_code");
                        if (member_type == 1)
                        {
                            max_collamt = Convert.ToDecimal(HdCollmaxval1.Value);
                            if (max_collamt == 0) { max_collamt = Convert.ToDecimal(mem_coll[2]); };
                        }
                        else
                        {
                            max_collamt = Convert.ToDecimal(HdCollmaxval2.Value);
                            if (max_collamt == 0) { max_collamt = Convert.ToDecimal(mem_coll[2]); };
                        }
                        //  max_collamt = dw_coll.GetItemDecimal(li_find, "MAXCOLL_PERIOD");
                        try
                        {
                            coll_balance = dw_coll.GetItemDecimal(i + 1, "coll_balance");
                        }
                        catch { coll_balance = 0; }
                        try
                        {
                            coll_use = dw_coll.GetItemDecimal(i + 1, "use_amt");
                        }
                        catch { coll_use = 0; }

                        if (coll_balance - coll_use > 0)
                        {
                            if (coll_newbalance > coll_balance - coll_use)
                            {
                                coll_use += coll_balance - coll_use;
                                coll_newbalance -= coll_balance - coll_use;
                            }
                            else
                            {
                                coll_use += coll_newbalance;
                                if (coll_use > max_collamt)
                                {
                                    coll_use = max_collamt;
                                    coll_newbalance -= coll_use;
                                }
                                coll_newbalance = 0;

                            }
                            dw_coll.SetItemDecimal(i + 1, "use_amt", coll_use);
                            collpercent_use = Convert.ToDouble(TruncateDecimal(Convert.ToDecimal(coll_use / loanrequest), 4));
                            dw_coll.SetItemDouble(i + 1, "coll_percent", collpercent_use);

                        }
                    }
                }
            }
            catch
            {

            }

        }

        private void JsCollInitP()
        {

            string ls_loantype = dw_main.GetItemString(1, "loantype_code");

            Decimal coll_balance = 0;
            string loancolltype_code = "";
            Decimal per90 = new Decimal(0.9);
            Decimal sharestk_value = 0;
            String sqlSharestk = "select sharestk_amt from shsharemaster where member_no = '" + dw_main.GetItemString(1, "member_no") + "'";
            Sdt dt = WebUtil.QuerySdt(sqlSharestk);
            if (dt.Next()) { sharestk_value = dt.GetDecimal("sharestk_amt"); }
            Decimal loanrequest = 0;
            decimal sumcoll_amt = 0;
            decimal coll_use = 0;
            double collpercent_use = 0;
            double sumcoll_percent = 0;
            loanrequest = dw_main.GetItemDecimal(1, "principal_balance");
            if (loanrequest == 0) { loanrequest = dw_main.GetItemDecimal(1, "loanapprove_amt"); }
            // loanrequest_amt = loanrequest - sharestk_value;
            int row = dw_coll.RowCount;
            int i = 0;

            for (i = 0; i < row; i++)
            {

                loancolltype_code = dw_coll.GetItemString(i + 1, "loancolltype_code");
                try
                {
                    coll_balance = dw_coll.GetItemDecimal(i + 1, "coll_balance");
                }
                catch { coll_balance = 0; }
                coll_use = dw_coll.GetItemDecimal(i + 1, "use_amt");
                // balance = loanrequest_amt * (collbalance / total);
                //  dw_coll.SetItemDecimal(i + 1, "use_amt", balance);
                if (loancolltype_code == "01")
                {
                    dw_coll.SetItemDecimal(i + 1, "base_percent", 1);
                }
                else
                {
                    dw_coll.SetItemDouble(i + 1, "base_percent", 0.9);
                }
                collpercent_use = Convert.ToDouble(coll_use / loanrequest);
                collpercent_use = Convert.ToDouble(TruncateDecimal(Convert.ToDecimal(collpercent_use), 4));
                dw_coll.SetItemDouble(i + 1, "coll_percent", collpercent_use);
                sumcoll_percent += collpercent_use;
                sumcoll_amt += coll_use;
            }
            sumcoll_percent = Convert.ToDouble(TruncateDecimal(Convert.ToDecimal(sumcoll_percent), 4));
            //หาส่วนต่างของ เปอร์เซ็นที่เหลือ
            double diff = 1.00 - sumcoll_percent;
            diff = Convert.ToDouble(Math.Round(Convert.ToDecimal(diff), 5));
            diff = Convert.ToDouble(TruncateDecimal(Convert.ToDecimal(diff), 4));
            if (diff <= 0.0002 && diff > 0.0000)
            {
                //กรณีส่วนต่าง เหลือแค่ 0.01
                for (i = 0; i < row; i++)
                {
                    if (sumcoll_percent >= 1.00) { continue; }
                    loancolltype_code = dw_coll.GetItemString(i + 1, "loancolltype_code");
                    coll_balance = dw_coll.GetItemDecimal(i + 1, "coll_balance");
                    coll_use = dw_coll.GetItemDecimal(i + 1, "use_amt");
                    if (coll_balance > coll_use)
                    {
                        collpercent_use = dw_coll.GetItemDouble(i + 1, "coll_percent");
                        collpercent_use += diff;
                        coll_use = loanrequest + coll_use - sumcoll_amt;// Convert.ToDecimal(collpercent_use);
                        dw_coll.SetItemDouble(i + 1, "coll_percent", collpercent_use);
                        dw_coll.SetItemDecimal(i + 1, "use_amt", coll_use);
                    }
                    sumcoll_percent += diff;

                }
            }



        }

        private void JSNew()
        {
            dw_main.Reset();
            dw_detpay.Reset();
            dw_coll.Reset();
            dw_con.Reset();
            dw_detcont.Reset();
            dw_detcontspc.Reset();
            dw_main.InsertRow(1);
            dw_detpay.InsertRow(1);
            dw_coll.InsertRow(1);
            dw_detcont.InsertRow(1);
            dw_detcontspc.InsertRow(1);
            dw_main.SetItemDateTime(1, "contadjust_date", state.SsWorkDate);
            tdw_main.Eng2ThaiAllRow();
        }

        public void SaveWebSheet()
        {
            //เช็คค่าก่อนบันทึกด้วย ที่ coll

            try
            {
                string loancontract_no = dw_main.GetItemString(1, "loancontract_no").Trim();
                DateTime contaj_date = dw_main.GetItemDateTime(1, "contadjust_date");

                str_lncontaj contadj = new str_lncontaj();
                contadj.xml_contdetail = dw_main.Describe("DataWindow.Data.XML");
                contadj.xml_contpayment = dw_detpay.Describe("DataWindow.Data.XML");
                contadj.xml_contloanpay = dw_con.Describe("DataWindow.Data.XML");//contadj.xml_contloanpay
                contadj.xml_contcoll = dw_coll.Describe("DataWindow.Data.XML");
                contadj.xml_contint = dw_detcont.Describe("DataWindow.Data.XML");
                contadj.xml_contintspc = dw_detcontspc.Describe("DataWindow.Data.XML");
                contadj.entry_id = state.SsUsername;

                //เงื่อนไข contintspc สามารุเพิ่มได้เมื่อ contint ฟิวด์ int_continttype = 3 (อัตราพิเศษเป็นช่วง)
                String int_continttype = dw_detcont.GetItemString(1, "int_continttype").Trim();
                if (int_continttype != "3")
                {
                    contadj.xml_contintspc = "";
                    if (dw_detcontspc.RowCount > 0)
                    {
                        Response.Write("<script> alert('ไม่ได้เลือกการคิดอัตราพิเศษช่วง จึงไม่สามารถป้อนอัตราดอกเบี้ยพิเศษเป็นช่วงได้'); </script>");
                    }
                }

                ShrlonSv = wcf.NShrlon;              

                try
                {
                    int result = ShrlonSv.of_savereq_contadjust(state.SsWsPass, contadj);
                    if (result == 1)
                    {
                        LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลสำเร็จ");
                    }
                    else { LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถบันทึกข้อมูลได้ ตรวจสอบอีกครั้ง"); }
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                }
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
                dw_con.SaveDataCache();
                dw_main.SetItemDateTime(1, "contadjust_date", state.SsWorkDate);
                tdw_main.Eng2ThaiAllRow();
            }
            catch (Exception ee) { }
        }

        #endregion


        //เรียกเว็บเซอร์วิส เปลี่ยนแปลงเลขที่สัญญา
        private void GetContractAdjust()
        {
            try
            {
                string loancontract_no = dw_main.GetItemString(1, "loancontract_no").Trim();
                DateTime contaj_date = dw_main.GetItemDateTime(1, "contadjust_date");
                string xml_contdetail = dw_main.Describe("DataWindow.Data.XML"); ;
                string xml_contpayment = dw_detpay.Describe("DataWindow.Data.XML"); ;
                string xml_contcoll = dw_coll.Describe("DataWindow.Data.XML"); ;
                string xml_contint = dw_detcont.Describe("DataWindow.Data.XML"); ;
                string xml_contintspc = dw_detcontspc.Describe("DataWindow.Data.XML"); ;
                string entry_id = state.SsUsername;
                string branch_id = state.SsCoopId;

                ShrlonSv = wcf.NShrlon;
                str_lncontaj contadj = new str_lncontaj();
                ShrlonSv.of_initreq_contadjust(state.SsWsPass, ref contadj);


                dw_main.Reset();
                dw_detpay.Reset();
                dw_coll.Reset();
                dw_detcont.Reset();
                dw_detcontspc.Reset();
                dw_con.Reset();
                // return xml = null กรณีที่ไม่มีรายการ (จะทำให้ import ไม่ได้)
                try
                {
                    dw_main.ImportString(contadj.xml_contdetail, FileSaveAsType.Xml);
                    dw_main.SetItemString(1, "concoop_id", state.SsCoopId);
                    dw_main.SetItemString(1, "loancontract_no", loancontract_no);
                }
                catch (Exception ex)
                {
                    try
                    {
                        DwUtil.ImportData(contadj.xml_contdetail, dw_main, tdw_main, FileSaveAsType.Xml);
                    }
                    catch (Exception) { }
                }

                try
                {
                    DwUtil.ImportData(contadj.xml_contloanpay, dw_con, tdw_main, FileSaveAsType.Xml);
                    //dw_con.ImportString(contadj.xml_contloanpay, FileSaveAsType.Xml); }
                }
                catch { }
                //dw_con.DeleteRow(2);
                try { dw_detpay.ImportString(contadj.xml_contpayment, FileSaveAsType.Xml); }
                catch { }

                try { dw_coll.ImportString(contadj.xml_contcoll, FileSaveAsType.Xml); }
                catch { }   

                try { dw_detcont.ImportString(contadj.xml_contint, FileSaveAsType.Xml); }
                catch { }

                try { dw_detcontspc.ImportString(contadj.xml_contintspc, FileSaveAsType.Xml); }
                catch { }

                tdw_main.Eng2ThaiAllRow();
                tdw_intspc.Eng2ThaiAllRow();
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
            //str_lncontaj lstr_lncontaj lstr_lncontaj.loancontract_no = dw_contdet.getitemstring( 1, "loancontract_no" ) lstr_lncontaj.contaj_date = dw_contdet.getitemdatetime( 1, "contadjust_date" ) lnv_lnoperate.of_initreq_contadjust( lstr_lncontaj ) dw_contdet.reset() dw_contdet.importstring( XML!, lstr_lnpause.xml_contdetail ) dw_contpay.reset() dw_contpay.importstring( XML!, lstr_lnpause.xml_contpayment ) dw_contint.reset() dw_contint.importstring( XML!, lstr_lnpause.xml_contint ) dw_contintspc.reset() dw_contintspc.importstring( XML!, lstr_lnpause.xml_contintspc ) dw_contcoll.reset() dw_contcoll.importstring( XML!, lstr_lnpause.xml_contcoll ) ---------------------------------------------------------------------- Revision History: Version 1.0 (Initial version) Modified Date 20/9/2010 by Ohh
        }

        private void GetMemberNo()
        {
            string membno = dw_main.GetItemString(1, "member_no").Trim();
            membno = WebUtil.MemberNoFormat(membno);
            String prename = "";
            String firstname = "";
            String lastname = "";
            //------------------get memb no

            Sta ta = new Sta(sqlca.ConnectionString);
            try
            {
                String sql = @"select  mbmembmaster.member_no,
                                mbucfprename.prename_desc , mbmembmaster.memb_name,
                                 mbmembmaster.memb_surname
                                from  mbmembmaster, mbucfprename

                                where mbucfprename.prename_code = mbmembmaster.prename_code
                                and mbmembmaster.member_no = '" + membno + "' and coop_Id = '" + state.SsCoopId + "'";

                Sdt dt = ta.Query(sql);
                //dt.Rows[0]["workdate"]
                prename = Convert.ToString(dt.Rows[0]["prename_desc"]);
                firstname = Convert.ToString(dt.Rows[0]["memb_name"]);
                lastname = Convert.ToString(dt.Rows[0]["memb_surname"]);

            }
            catch (Exception ex)
            {
            }
            ta.Close();
            //------------end get Memno det

            dw_main.SetItemString(1, "member_no", membno);
            dw_main.SetItemString(1, "prename_desc", prename);
            dw_main.SetItemString(1, "memb_name", firstname);
            dw_main.SetItemString(1, "memb_surname", lastname);
        }

        /// <summary>
        /// เรียกใช้เว็บเซอรวิส CheckRightColl เพื่อตรวจสอบยอดค้ำคงเหลือ รายละเอียดการค้ำประกัน
        /// </summary>
        private void CheckCollRight()
        {
            //พารามิเตอร์ทั้งหมด string as_memno, string as_membtype, string as_loantype, DateTime adtm_operate, string as_colltype, string as_refcollno, string as_contclear, short ai_period, ref str_checkrightcoll astr_checkrightcoll
            //1memno @main
            //2memtype @main, 
            //3loantype @main
            //4date_opr @main
            //5coll_type@coll
            //6refcollno @ coll
            //7contclear = เลขที่สัญญา @main
            //8period@pay
            //9str
            try
            {
                int collRow = Convert.ToInt32(HdRefcollrow.Value);
                String memno = dw_main.GetItemString(1, "member_no").Trim();
                String memtype = dw_main.GetItemString(1, "member_type").Trim();
                String loantype = dw_main.GetItemString(1, "loantype_code").Trim();
                DateTime date_op = state.SsWorkDate;
                String coll_type = dw_coll.GetItemString(collRow, "loancolltype_code");
                String refcollno = dw_coll.GetItemString(collRow, "ref_collno").Trim();
                String contclear = dw_main.GetItemString(1, "loancontract_no").Trim();
                object ff = dw_detpay.GetItemDecimal(1, "period_payamt");
                short period_pay = Convert.ToInt16(ff);
                Boolean ab_change = true;

                ShrlonSv = wcf.NShrlon;
                str_checkrightcoll chkrightcoll = new str_checkrightcoll();
                try
                {
                    ShrlonSv.of_checkrightcoll(state.SsWsPass, memno, loantype, date_op, coll_type, refcollno, contclear, period_pay, ab_change, chkrightcoll);
                    //ยัดค่า 1 percent,2 description, 3 coll_amt, 4 redcollno
                    //dw_coll.SetItemDecimal(collRow, "coll_percent", chkrightcoll.base_percent );
                    dw_coll.SetItemString(collRow, "description", chkrightcoll.description);
                    dw_coll.SetItemDecimal(collRow, "coll_balance", chkrightcoll.coll_balance);
                    dw_coll.SetItemString(collRow, "ref_collno", chkrightcoll.ref_collno);

                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                }

            }
            catch (Exception er) { }

        }

        private void JspostMemberFromDlg()
        {
            try
            {
                string membno = hdmember_no.Value.Trim();
                membno = WebUtil.MemberNoFormat(membno);
                String prename = "";
                String firstname = "";
                String lastname = "";
                //------------------get memb no

                Sta ta = new Sta(sqlca.ConnectionString);
                try
                {
                    String sql = @"select  mbmembmaster.member_no,
                                mbucfprename.prename_desc , mbmembmaster.memb_name,
                                 mbmembmaster.memb_surname
                                from  mbmembmaster, mbucfprename

                                where mbucfprename.prename_code = mbmembmaster.prename_code
                                and mbmembmaster.member_no = '" + membno + "' and coop_Id = '" + state.SsCoopId + "'";

                    Sdt dt = ta.Query(sql);
                    //dt.Rows[0]["workdate"]
                    prename = Convert.ToString(dt.Rows[0]["prename_desc"]);
                    firstname = Convert.ToString(dt.Rows[0]["memb_name"]);
                    lastname = Convert.ToString(dt.Rows[0]["memb_surname"]);
                    Hdbutton.Value = "";
                }
                catch (Exception ex)
                {
                }
                ta.Close();
                //------------end get Memno det

                dw_main.SetItemString(1, "member_no", membno);
                dw_main.SetItemString(1, "prename_desc", prename);
                dw_main.SetItemString(1, "memb_name", firstname);
                dw_main.SetItemString(1, "memb_surname", lastname);
            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message); }
        }
    }
}