using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Saving.WcfCommon;
using Saving.WcfShrlon;
using Saving.WcfBusscom;
using Sybase.DataWindow;
using System.Web.Services.Protocols;
using DataLibrary;
using System.Data;
namespace Saving.Applications.shrlon
{
    public partial class w_sheet_sl_contract_adjcoll : PageWebSheet, WebSheet
    {
        private DwThDate tDwMain;
        protected String jscontract_no;
        protected String newClear;
        protected string jsSetmembinfo;
        protected string jsSetCollContno;
        public void InitJsPostBack()
        {
            jscontract_no = WebUtil.JsPostBack(this, "jscontract_no");
            jsSetmembinfo = WebUtil.JsPostBack(this, "jsSetmembinfo");
            jsSetCollContno = WebUtil.JsPostBack(this, "jsSetCollContno");
            tDwMain = new DwThDate(dw_main, this);

        }

        public void WebSheetLoadBegin()
        {
            
            if (IsPostBack)
            {
                this.RestoreContextDw(dw_main);

            }
            else
            {
                if (dw_main.RowCount < 1)
                {
                    dw_main.InsertRow(0);
                    dw_main.SetItemString(1, "cancel_id", state.SsUsername);
                    dw_main.SetItemDate(1, "cancel_date", state.SsWorkDate);
                    tDwMain.Eng2ThaiAllRow();
                }
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "jsSetCollContno")
            {

                JsSetCollContno();

            }
            else if (eventArg == "newClear")
            {
                JsNewClear();
            }
            else if (eventArg == "jsSetmembinfo") 
            {
                JsSetmembinfo();
            }
        }

        public void SaveWebSheet()
        {
            Sta ta = new Sta(sqlca.ConnectionString);
            try
            {
                String loancontract_no = "", member_no = "";
                Decimal loanapprove_amt = 0, period_payamt = 0, period_payment = 0;
                member_no = dw_main.GetItemString(1, "member_no");
                loancontract_no = dw_main.GetItemString(1, "loancontract_no");
                loanapprove_amt = dw_main.GetItemDecimal(1, "loanapprove_amt");
                period_payamt = dw_main.GetItemDecimal(1, "period_payamt");
                period_payment = dw_main.GetItemDecimal(1, "period_payment");
                String sqlupdate = @"  UPDATE LNCONTMASTER  
                         SET LOANAPPROVE_AMT = " + loanapprove_amt + @",   
                             LOANREQUEST_AMT = " + loanapprove_amt + @",    
                             PERIOD_PAYMENT = " + period_payment + @",   
                             PERIOD_PAYAMT = " + period_payamt + @"  
                       WHERE ( LNCONTMASTER.LOANCONTRACT_NO ='" + loancontract_no + @"'  ) AND  
                             ( LNCONTMASTER.MEMBER_NO = '" + member_no + @"'  ) ";

                ta.Exe(sqlupdate);

                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
            ta.Close();
        }

        public void WebSheetLoadEnd()
        {
            this.dw_main.SaveDataCache();
        }

       
        public void JsSetmembinfo() 
        {
            String member_no = WebUtil.MemberNoFormat(HdMember_no.Value);
            dw_main.SetItemString(1, "member_no", member_no);
            string ls_sql = "select a.memb_name, a.memb_surname, b.prename_desc from mbmembmaster a , mbucfprename b where a.prename_code = b.prename_code and a.member_no = '" + member_no + "'";
            Sdt dt = WebUtil.QuerySdt(ls_sql);
            if (dt.Next()) {

                string prename_desc = dt.GetString("prename_desc");
                string memb_name = dt.GetString("memb_name");
                string memb_surname = dt.GetString("memb_surname");

                dw_main.SetItemString(1, "prename_desc", prename_desc);
                dw_main.SetItemString(1, "memb_name", memb_name);
                dw_main.SetItemString(1, "memb_surname", memb_surname);

            }
            
        }
        private int JsCheckMangrtColl(int row, string ref_collno)
        {
           
            string mangrtpermgrp_code = "01";
            string loantype = dw_main.GetItemString(1, "loantype_code");
            string membno = dw_main.GetItemString(1, "member_no");
            string memcoop_id = dw_main.GetItemString(1, "coop_id");
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
            string  contclr_all = "";
            
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
                LtServerMessage.Text = WebUtil.WarningMessage("ผู้ค้ำสมาชิกเลขที่ # " + ref_collno + "  ได้ค้ำประกันเกินกว่าที่กำหนดไว้ค้ำประกันได้สูงสุด  " + grtcountcont.ToString() + " สัญญา ค้ำประกันไปแล้ัว " + dt_coll.GetRowCount().ToString());
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
                LtServerMessage.Text = WebUtil.ErrorMessage("ผู้ค้ำสมาชิกเลขที่ # " + ref_collno + "  ได้ค้ำประกันสมาชิกเกินกว่าที่กำหนดไว้ค้ำประกันได้สูงสุด  " + "\n" + grtcountmem.ToString() + " ค้ำประกันได้ไม่ไม่เกิน " + collmem.ToString() + "\n " + ls_msg);
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
                    DateTime ldtm_reqloan = dw_main.GetItemDateTime(1, "loanrequest_date");
                    Decimal member_age = 0;// = BusscomService.of_cal_yearmonth(state.SsWsPass, ldtm_member, DateTime.Now);

                    member_age = (ldtm_reqloan.Year - ldtm_member.Year) * 12;
                    member_age += (ldtm_reqloan.Month - ldtm_member.Month);
                    if (ldtm_member.Day > ldtm_reqloan.Day) { member_age--; }
                    //decimal age_year = Math.Truncate(member_age / 12);
                    //decimal age_month = (member_age % 12) / 100;

                    //member_age = age_year + age_month;

                    //คำนวณสิทธิค้ำประกัน
                    Decimal coll_amt = wcf.Shrlon.CalloanpermissColl(state.SsWsPass, member_age, ldc_salary, mangrtpermgrp_code);
                    ldc_collbalance = coll_amt - ldc_collamt;
                    if (ldc_collbalance < 0) { ldc_collbalance = 0; }
                    dw_coll.SetItemDecimal(row, "coll_amt", coll_amt);
                    dw_coll.SetItemDecimal(row, "coll_balance", ldc_collbalance);
                    dw_coll.SetItemDecimal(row, "coll_useamt", ldc_collamt);
                    return 1; //-1

                }



            }
            dt_coll.Dispose();
            return 1;
        }

        private void JsCollCondition()
        {
            string ls_loantype = dw_main.GetItemString(1, "loantype_code");

            Decimal coll_balance = 0;
            string loancolltype_code = "";
            Decimal coll_use = 0;
            double collpercent_use = 0.00;
            Decimal per90 = new Decimal(0.9);
            Decimal sharestk_value = dw_main.GetItemDecimal(1, "sharestk_value");
            Decimal loanrequest, loanrequestbal = 0, colluseest_amt = 0;
            loanrequest = dw_main.GetItemDecimal(1, "loanrequest_amt");
            // loanrequest_amt = loanrequest - sharestk_value;

            decimal coll_newbalance = 0;
            int i = 0;
            loanrequestbal = loanrequest;

            //สิทธิยอดหลักทรัพย์ให้เต็มก่อน
            // string[] coll_mast = new string[] coll_mast;
            string[] coll_mast = new string[3] { "02", "03", "04" };
            int li_find = 0;
            int rowc = 1;
            coll_newbalance = loanrequest;

            for (int mrow = 0; mrow <= Convert.ToInt16(2); mrow++) //coll_mast.GetUpperBound()
            {
                string colltype = coll_mast[mrow];
                li_find = dw_coll.FindRow("loancolltype_code = '" + colltype + "'", rowc, dw_coll.RowCount);

                while (li_find > 0 && coll_newbalance > 0)
                {
                    if (coll_newbalance > 0)
                    {

                        coll_balance = dw_coll.GetItemDecimal(li_find, "coll_balance");
                        if (coll_balance >= coll_newbalance)
                        {
                            coll_use = coll_newbalance;
                            coll_newbalance = 0;
                            dw_coll.SetItemDecimal(li_find, "use_amt", coll_use);

                        }
                        else
                        {

                            coll_use = coll_balance;
                            coll_newbalance -= coll_use;
                            dw_coll.SetItemDecimal(li_find, "use_amt", coll_use);
                        }
                    }
                    collpercent_use = Convert.ToDouble(TruncateDecimal(Convert.ToDecimal(coll_use / loanrequest), 4));
                    dw_coll.SetItemDouble(li_find, "coll_percent", collpercent_use);

                    li_find++;
                    if (li_find > dw_coll.RowCount) { break; }
                    li_find = dw_coll.FindRow("loancolltype_code = '" + colltype + "'", li_find, dw_coll.RowCount);

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
                        if (coll_balance > 500000) { coll_balance = 500000; }

                        if (colluseest_amt >= coll_balance)
                        {
                            if (coll_balance > 500000) { coll_balance = 500000; }
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
                            if (coll_use > 500000)
                            {
                                coll_use = 500000;
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

        private decimal TruncateDecimal(decimal value, int precision)
        {
            decimal step = (decimal)Math.Pow(10, precision);
            int tmp = (int)Math.Truncate(step * value);
            return tmp / step;
        }
        private decimal JsRoundMoney(decimal adc_money, int ai_roundtype)
        {
            try
            {
                decimal tmp = TruncateDecimal(adc_money, 2);
                decimal tmp1 = TruncateDecimal(tmp, 0);
                decimal factorvalue = tmp - tmp1;
                double facvalue = Convert.ToDouble(factorvalue);
                decimal ipoint1 = TruncateDecimal(factorvalue, 1);
                decimal ipoint2 = factorvalue - ipoint1;
                double ipoint22 = Convert.ToDouble(ipoint2);
                double tmpround = 0;
                switch (ai_roundtype)
                {
                    case 1:
                        //ปัดที่ละสลึง
                        tmpround = 0.00;
                        if (facvalue >= 0.01 && facvalue <= 0.25) { tmpround = 0.25; }
                        if (facvalue >= 0.26 && facvalue <= 0.50) { tmpround = 0.50; }
                        if (facvalue >= 0.51 && facvalue <= 0.75) { tmpround = 0.75; }
                        if (facvalue >= 0.76 && facvalue <= 0.99) { tmpround = 1.00; }

                        break;
                    case 2:
                        //ปัดที่ละ 5 สตางค์
                        tmpround = 0.00;
                        if (ipoint22 == 0.00) { return adc_money; }
                        if (ipoint22 == 0.05) { return adc_money; }
                        if (ipoint22 >= 0.01 && ipoint22 <= 0.04) { ipoint22 = 0.05; }
                        if (ipoint22 >= 0.06 && ipoint22 <= 0.09) { ipoint22 = 0.10; }
                        tmpround = Convert.ToDouble(ipoint1) + ipoint22;

                        break;
                    case 3:
                        //ปัดที่ละ 10 สตางค์

                        if (ipoint22 == 0.00)
                        {
                            return adc_money;
                        }
                        else
                        {
                            ipoint22 = 0.10;
                        }
                        tmpround = Convert.ToDouble(ipoint1) + ipoint22;

                        break;

                    case 4:
                        //ปัดเต็มบาท

                        if (facvalue > 0.49)
                        {
                            tmpround = 1.00;
                        }
                        else
                        {
                            tmpround = 0.00;
                        }

                        break;

                    case 99:
                        //ปัดตามตาราง
                        //li_find	= ids_roundfactor.find( "factor_code = '"+is_rdsatangtab+"' and factor_step >= "+string( ldc_facvalue, "0.00" ), 1, ids_roundfactor.rowcount() )
                        //if li_find <= 0 then
                        //    return adc_money
                        //end if

                        //ldc_rdamt	= ids_roundfactor.getitemdecimal( li_find, "round_amt" )
                        tmpround = facvalue;
                        break;

                    default:
                        tmpround = facvalue;
                        break;

                }
                tmp = tmp1 + Convert.ToDecimal(tmpround);
                return tmp;

            }
            catch
            {
                return adc_money;
            }


        }
        private void JsCheckCollmastrightBalance()
        {
            try
            {
                int row = Convert.ToInt16(HdRowNumber.Value);
                string ref_collno = dw_coll.GetItemString(row, "ref_collno");
                string loantype = dw_main.GetItemString(1, "loantype_code");
                string membno = dw_main.GetItemString(1, "member_no");
                string memcoop_id = dw_main.GetItemString(1, "coop_id");
                string loancolltype_code = dw_coll.GetItemString(row, "loancolltype_code");
                string loanrequst_docno = dw_main.GetItemString(1, "loanrequest_docno");
                string contno_clr = "", contclr_all = "";
                decimal clear_flag = 0;
                
                if ((contclr_all.Length <= 7) || (contclr_all.Trim() == "")) { contclr_all = "'00000'"; }

                string sql_chkcoll = @" SELECT LNCONTMASTER.MEMBER_NO as member_no,   
             LNCONTMASTER.LOANCONTRACT_NO as loancontract_no,   
             LNCONTMASTER.PRINCIPAL_BALANCE as principal_balance ,
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
                 LNREQLOANCOLL.BASE_PERCENT as BASE_PERCENT  , 'Lnreqloan'
            FROM LNREQLOAN,   
                 LNREQLOANCOLL  
           WHERE ( LNREQLOAN.COOP_ID = LNREQLOANCOLL.COOP_ID ) and  
                 ( LNREQLOAN.LOANREQUEST_DOCNO = LNREQLOANCOLL.LOANREQUEST_DOCNO ) and  ( LNREQLOAN.LOANREQUEST_DOCNO <> '" + loanrequst_docno + @"' ) and  
                 ( ( LNREQLOANCOLL.LOANCOLLTYPE_CODE = '" + loancolltype_code + @"' ) AND  
                 ( LNREQLOAN.LOANREQUEST_STATUS = 8 ) AND  
                 ( LNREQLOANCOLL.REF_COLLNO =  '" + ref_collno + @"'  ) )  ";
                Sdt dt_coll = WebUtil.QuerySdt(sql_chkcoll);


                decimal principal_balance = 0, ldc_colluse = 0, ldc_collpercent = 0, ldc_collamt = 0;
                while (dt_coll.Next())
                {
                    principal_balance = dt_coll.GetDecimal("principal_balance");
                    ldc_collpercent = dt_coll.GetDecimal("COLL_PERCENT");
                    ldc_colluse = principal_balance * ldc_collpercent;
                    ldc_collamt += ldc_colluse;

                }

                if (ldc_collamt >= 0)
                {

                    decimal coll_balance = dw_coll.GetItemDecimal(row, "coll_balance");

                    coll_balance = coll_balance - ldc_collamt;

                    dw_coll.SetItemDecimal(row, "coll_balance", coll_balance);
                    dw_coll.SetItemDecimal(row, "use_amt", coll_balance);

                }



            }
            catch
            {

            }
            try
            {
                JsCollCondition();
            }
            catch
            {

            }
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
            }
            catch
            {
                percent_collmast = 0;

            }
            return percent_collmast;
        }
        //JS-EVENT
        private void JsNewClear()
        {
            dw_main.Reset();
            dw_main.InsertRow(0);
            dw_main.SetItemString(1, "cancel_id", state.SsUsername);
            dw_main.SetItemDate(1, "cancel_date", state.SsWorkDate);
            tDwMain.Eng2ThaiAllRow();
        }
        private void JsSetCollContno()
        {
            try
            {
                //กรณีเลือกกู้แบบกู้เพิ่ม
                string ls_memno = dw_main.GetItemString(1, "member_no");
               // string ls_loantype = dw_main.GetItemString(1, "loantype_code");
                string contno_pri = dw_main.GetItemString(1, "loancontract_no");

                string ls_sqlcon = "select a.loantype_code, b.loantype_desc, a.principal_balance, a.loanapprove_amt,a.withdrawable_amt, a.contlaw_status from lncontmaster a, lnloantype b where a.loantype_code = b.loantype_code and a.loancontract_no = '" + contno_pri + "'";
                Sdt dtcont = WebUtil.QuerySdt(ls_sqlcon);
                if (dtcont.Next() ) {
                    string ls_loantype = dtcont.GetString("loantype_code");
                    string ls_loantypedesc = dtcont.GetString("loantype_desc");
                    decimal principal_balance = dtcont.GetDecimal("principal_balance");
                    decimal loanapprove_amt = dtcont.GetDecimal("loanapprove_amt");
                    decimal withdrawable_amt = dtcont.GetDecimal("withdrawable_amt");
                    decimal contlaw_status = dtcont.GetDecimal("contlaw_status");

                    dw_main.SetItemString(1, "loantype_code", ls_loantype);
                    dw_main.SetItemString(1, "loantype_desc", ls_loantypedesc);
                    dw_main.SetItemDecimal(1, "principal_balance", principal_balance);
                    dw_main.SetItemDecimal(1, "loanapprove_amt", loanapprove_amt);
                    dw_main.SetItemDecimal(1, "withdrawable_amt", withdrawable_amt);

                }


                string sql_memcoll = @" SELECT LNCONTMASTER.MEMBER_NO as member_no,   
             LNCONTMASTER.LOANCONTRACT_NO as loancontract_no,   
             LNCONTCOLL.REF_COLLNO as  ref_collno,   
             LNCONTCOLL.DESCRIPTION as DESCRIPTION ,   
             LNCONTCOLL.COLL_AMT as COLL_AMT ,   
             LNCONTCOLL.USE_AMT as USE_AMT , 
            LNCONTCOLL.COLL_BALANCE as COLL_BALANCE,
             LNCONTCOLL.COLL_PERCENT as COLL_PERCENT ,   
             LNCONTCOLL.BASE_PERCENT as BASE_PERCENT , 
             LNCONTMASTER.COOP_ID as coop_id,
             LNCONTMASTER.loancredit_amt  ,
             LNCONTMASTER.last_periodrcv             
            FROM LNCONTCOLL,   
                 LNCONTMASTER  
           WHERE ( LNCONTCOLL.COOP_ID = LNCONTMASTER.COOP_ID ) and  
                 ( LNCONTCOLL.LOANCONTRACT_NO = LNCONTMASTER.LOANCONTRACT_NO ) and  
                 ( ( LNCONTCOLL.LOANCOLLTYPE_CODE in ('01') ) AND  
                 ( LNCONTMASTER.PRINCIPAL_BALANCE > 0 ) AND LNCONTMASTER.LOANCONTRACT_NO = '" + contno_pri + "')";

                Sdt dtcoll = WebUtil.QuerySdt(sql_memcoll);
                string collref_no = "", colldesc = "", coop_id = "";
                int coll_row = 0;
                decimal coll_balance = 0;
                decimal coll_use = 0;
                decimal coll_amt = 0;
                double base_percent = 0;
                double coll_percent = 0;
                dw_coll.Reset();
                if (dtcoll.GetRowCount() < 1)
                {
                    LtServerMessage.Text = WebUtil.WarningMessage("ไม่พบรายการสมาชิกค้ำประกันสัญญา " + contno_pri);
                }
                while (dtcoll.Next())
                {
                    collref_no = dtcoll.GetString("ref_collno");
                    colldesc = dtcoll.GetString("DESCRIPTION");
                    coop_id = dtcoll.GetString("coop_id");
                    coll_balance = dtcoll.GetDecimal("coll_balance");
                    coll_amt = dtcoll.GetDecimal("coll_amt");
                    coll_use = dtcoll.GetDecimal("use_amt");
                    base_percent = dtcoll.GetDouble("base_percent");
                    coll_percent = dtcoll.GetDouble("coll_percent");

                    coll_row = dw_coll.InsertRow(0);

                    dw_coll.SetItemString(coll_row, "coop_id", coop_id);
                    dw_coll.SetItemString(coll_row, "ref_collno", collref_no);
                    dw_coll.SetItemString(coll_row, "DESCRIPTION", colldesc);
                    dw_coll.SetItemDecimal(coll_row, "use_amt", coll_use);
                    dw_coll.SetItemDecimal(coll_row, "coll_balance", coll_balance);
                    dw_coll.SetItemDouble(coll_row, "base_percent", base_percent);
                    dw_coll.SetItemDouble(coll_row, "coll_percent", coll_percent);

                }

            }
            catch
            {

            }
        }
    }
}
