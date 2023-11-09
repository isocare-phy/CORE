using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using System.Globalization;
using DataLibrary;

namespace Saving.Applications.hr.ws_hr_payroll_process_ctrl
{
    public partial class ws_hr_payroll_process : PageWebSheet, WebSheet
    {
        [JsPostBack]
        public String Post { get; set; }
        private CultureInfo th;

        public void InitJsPostBack()
        {
            dsMain.InitDsMain(this);
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                dsMain.DdEmptype();
                dsMain.DATA[0].tran_date = state.SsWorkDate;

                //set งวด
                this.th = new CultureInfo("th-TH");
                DateTime dt_contadjust_date = state.SsWorkDate;
                year.Text = Convert.ToString(WebUtil.GetAccyear(state.SsCoopControl, state.SsWorkDate));
                month.Text = DateTime.Now.Month.ToString();

                //set rate ประกันสังคม
                string ls_sql = @"select ss_emprate, salary_day from hrcfconstant";
                Sdt dt = WebUtil.QuerySdt(ls_sql);
                if (dt.Next())
                {
                    ss_emprate.Text = Convert.ToString(dt.GetDecimal("ss_emprate"));
                    salary_day.Text = Convert.ToString(dt.GetDecimal("salary_day"));
                }
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "Post")
            {
                string ls_sql = "", ls_sqlinsert = "", ls_sqlupdate = "", ls_sqldel = "";
                string ls_prefix = "", ls_year = "", ls_documentno = "", ls_emptype = "";
                string ls_period = "", ls_empno = "";
                decimal ldc_salary = 0, ldc_ssemprate = 0, ldc_days = 0, ldc_salaryday = 0;
                decimal ldc_lastdocno = 0;
                DateTime ldtm_workdate, ldtm_trandate;//, ldtm_lastworkdate;
                int seq_no = 0;
                Sdt dt;
                ExecuteDataSource exe = new ExecuteDataSource(this);

                ldtm_trandate = dsMain.DATA[0].tran_date;
                ls_emptype = dsMain.DATA[0].emptype_code;

                ls_period = year.Text + Convert.ToDecimal(month.Text).ToString("00");
                var now = DateTime.Now;
                string mount = Convert.ToDecimal(month.Text).ToString("00");
                string ls_sdate = @"select firstworkdate as firstwork,
                                    lastworkdate as lastwork
                                from amworkcalendar 
                                where coop_id = '" + state.SsCoopControl + @"'
                                    and year = '" + ls_period.Substring(0,4) + @"'                                    
                                    and month = '" + mount + "'";
                dt = WebUtil.QuerySdt(ls_sdate);
                if (dt.Next())
                {
                    var firstwork = dt.GetInt32("firstwork");
                   // var lastwork = dt.GetInt32("lastwork");
                
                var first = new DateTime(now.Year, now.Month, firstwork);
                //var last = new DateTime(now.Year, now.Month, lastwork);
                ldc_salaryday = Convert.ToDecimal(salary_day.Text);

                if (ldc_salaryday == 0)
                {
                    ldc_salaryday = System.DateTime.DaysInMonth(now.Year, now.Month);
                }

                ls_sql = @"select last_documentno, document_prefix, document_year
                    from cmdocumentcontrol 
                    where coop_id = '" + state.SsCoopControl + @"'
                    and document_code = 'HRPAYROLLSLIPNO'";
                dt = WebUtil.QuerySdt(ls_sql);
                if (dt.Next())
                {
                    ldc_lastdocno = dt.GetDecimal("last_documentno");
                    ls_prefix = dt.GetString("document_prefix");
                    ls_year = Convert.ToString(dt.GetDecimal("document_year"));
                    ls_year = ls_year.Substring(2, 2);
                }

                //ลบข้อมูลก่อน
                ls_sqldel = @"delete from hrpayroll 
                    where coop_id = '" + state.SsCoopControl + @"'
                    and payroll_period = '" + ls_period + @"'
                    and emp_no in ( select hem.emp_no from hremployee hem where hem.emptype_code = '" + ls_emptype + @"' )
                    and post_status = 0";
                exe.SQL.Add(ls_sqldel);

                //insert เงินเดือน hrpayroll
                ls_sql = @"select hrm.emp_no, nvl(hrm.salary_amt, 0) as salary_amt, 
                            hrm.work_date, last_day(trunc(sysdate)) - hrm.work_date as days, hrm.salexp_code,
                            hrm.salexp_bank, hrm.salexp_branch, hrm.salexp_accid, hrm.resign_date,
                            TO_CHAR(hrm.resign_date, 'dd') as days2,hrm.ref_membno as ref_membno
                    from hremployee hrm
                    where coop_id = '" + state.SsCoopControl + @"'
                    and emp_status = 1
                    and emptype_code = '" + ls_emptype + @"'
                    and hrm.emp_no not in ( select hpr.emp_no from hrpayroll hpr where hpr.post_status = 1 and hpr.payroll_period = '" + ls_period + "')";
                dt = WebUtil.QuerySdt(ls_sql);
                try
                {

                    if (dt.Next())
                    {
                        for (int i = 0; i < dt.GetRowCount(); i++)
                        {
                            ldc_lastdocno += 1;
                            ls_documentno = ls_prefix + ls_year + ldc_lastdocno.ToString("00000");
                            ls_empno = Convert.ToString(dt.Rows[i]["emp_no"]);
                            ldc_salary = Convert.ToDecimal(dt.Rows[i]["salary_amt"]);
                            ldtm_workdate = Convert.ToDateTime(dt.Rows[i]["work_date"]);
                            //ldtm_lastworkdate = Convert.ToDateTime(dt.Rows[i]["resign_date"]);

                            //กรณีเริ่มทำงานระหว่างเดือน
                            if (ldtm_workdate > first)
                            {
                                ldc_days = Convert.ToInt32(dt.Rows[i]["days"]);
                                ldc_salary = decimal.Truncate(ldc_salary / ldc_salaryday * ldc_days);
                            }

                            //กรณีออกจากงานระหว่างเดือน
                            //if (ldtm_lastworkdate < last)
                            //{
                            //    ldc_days = Convert.ToInt32(dt.Rows[i]["days2"]);
                            //    ldc_salary = decimal.Truncate(ldc_salary / ldc_salaryday * ldc_days);
                            //}

                            ls_sqlinsert = @"insert into hrpayroll ( coop_id, 
                            payrollslip_no, 
                            emp_no, 
                            payroll_date, 
                            payroll_period, 
                            salarybase_amt, 
                            payroll_status, 
                            expense_code, 
                            expense_bank, 
                            expense_branch, 
                            expense_accid, 
                            post_status,
                            member_no)
                        values( {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11},{12})";
                            object[] argsInsert = new object[] { state.SsCoopControl,
                        ls_documentno,
                        ls_empno,
                        ldtm_trandate,
                        ls_period,
                        ldc_salary,
                        1,
                        Convert.ToString(dt.Rows[i]["salexp_code"]),
                        Convert.ToString(dt.Rows[i]["salexp_bank"]),
                        Convert.ToString(dt.Rows[i]["salexp_branch"]),
                        Convert.ToString(dt.Rows[i]["salexp_accid"]),
                        0,Convert.ToString(dt.Rows[i]["ref_membno"])};
                            ls_sqlinsert = WebUtil.SQLFormat(ls_sqlinsert, argsInsert);

                            exe.SQL.Add(ls_sqlinsert);

                            //เงินเดือน hrpayrolldet
                            ls_sqlinsert = @"insert into hrpayrolldet ( coop_id, 
                            payrollslip_no, 
                            seq_no, 
                            salitem_code, 
                            description, 
                            item_amt )
                        values( {0}, {1}, {2}, {3}, {4}, {5} )";

                            object[] argsInsertDet = new object[] { state.SsCoopControl,
                            ls_documentno,
                            seq_no+1,
                            "R01",
                            "เงินเดือน",
                            ldc_salary};
                            ls_sqlinsert = WebUtil.SQLFormat(ls_sqlinsert, argsInsertDet);

                            exe.SQL.Add(ls_sqlinsert);
                        }
                    }
                }
                catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); return; }
                //update เลข slip payroll ล่าสุด
                ls_sqlupdate = @"update cmdocumentcontrol set last_documentno = " + ldc_lastdocno + @"                    
                    where coop_id = '" + state.SsCoopControl + @"'
                    and document_code = 'HRPAYROLLSLIPNO'";
                exe.SQL.Add(ls_sqlupdate);

                //เงินประจำตำแหน่ง hrpayrolldet
                ls_sqlinsert = @"insert into hrpayrolldet
                    (
                    select hep.coop_id, 
                    hpr.payrollslip_no, 
                    ( select nvl( max( hpd.seq_no) , 0 ) + 1 from hrpayrolldet hpd where hpd.coop_id = hpr.coop_id and hpd.payrollslip_no = hpr.payrollslip_no ) as seq_no , 
                    'R03', 
                    'เงินประจำตำแหน่ง', 
                    hup.pos_money
                    from hremployee hep,
                    hrpayroll hpr,
                    hrucfposition hup
                    where hep.coop_id = hpr.coop_id
                    and hep.emp_no = hpr.emp_no
                    and hep.pos_code = hup.pos_code
                    and hep.emp_status = 1
                    and hup.pos_money > 0
                    and hpr.post_status = 0
                    and hep.emptype_code = '" + ls_emptype + @"'
                    and hpr.payroll_period = '" + ls_period + "')";
                exe.SQL.Add(ls_sqlinsert);                

                //เงินกองทุน hrpayrolldet
                ls_sqlinsert = @"insert into hrpayrolldet
                    (
                    select hep.coop_id,
                     hpr.payrollslip_no,
                    ( select nvl( max( hpd.seq_no) , 0 ) + 1 from hrpayrolldet hpd where hpd.coop_id = hpr.coop_id and hpd.payrollslip_no = hpr.payrollslip_no ) as seq_no , 
                    'P15',
                    'หักกองทุนสำรองเลี้ยงชีพ',
                    hep.salary_amt * hep.provf_emprate/100 as provf_amt
                    from hremployee hep,
                    hrpayroll hpr
                    where  hep.coop_id = hpr.coop_id
                    and hep.emp_no = hpr.emp_no
                    and hep.emp_status = 1
                    and hep.provf_status = 1
                    and hpr.post_status = 0
                    and hep.emptype_code = '" + ls_emptype + @"'
                    and hpr.payroll_period = '" + ls_period + "')";
                exe.SQL.Add(ls_sqlinsert);

                if (state.SsCoopControl == "0310001")
                {
                    //ชำระหนี้ กรมชล
                    ls_sqlinsert = @"insert into hrpayrolldet
                    (
                    select kpt.coop_id,
                    hpr.payrollslip_no,
                    ( nvl( hprd.seq_no , 0 ) + 1 ) as seq_no ,
                    'P18',
                    'หักชำระหนี้',
                    sum( nvl( kpt.item_payment , 0 ) ) as item_payment
                    from kptempreceivedet kpt,
                    hremployee hep,
                    hrpayroll hpr,
                    ( select hpd.coop_id , hpd.payrollslip_no , max( hpd.seq_no ) as seq_no
                    from hrpayrolldet hpd
                    group by hpd.coop_id , hpd.payrollslip_no 
                    ) hprd
                    where kpt.coop_id = hep.coop_id
                    and kpt.ref_membno = hep.ref_membno
                    and kpt.coop_id = hpr.coop_id (+)
                    and kpt.recv_period = hpr.payroll_period (+)
                    and hpr.coop_id = hprd.coop_id(+)
                    and hpr.payrollslip_no = hprd.payrollslip_no(+)
                    and hep.coop_id = hpr.coop_id
                    and hep.emp_no = hpr.emp_no
                    and hep.emp_status = 1   
                    and hpr.post_status = 0                 
                    and kpt.recv_period = '" + ls_period + @"'
                    and hep.emptype_code = '" + ls_emptype + @"'
                    group by kpt.coop_id, hpr.payrollslip_no , ( nvl( hprd.seq_no , 0 ) + 1 )
                    )";

                }
                else
                {
                    //ชำระหนี้
                    ls_sqlinsert = @"insert into hrpayrolldet
                    (
                    select kpt.coop_id,
                    hpr.payrollslip_no,
                    ( nvl( hprd.seq_no , 0 ) + 1 ) as seq_no ,
                    'P18',
                    'หักชำระหนี้',
                    sum( nvl( kpt.item_payment , 0 ) ) as item_payment
                    from kptempreceivedet kpt,
                    hremployee hep,
                    hrpayroll hpr,
                    ( select hpd.coop_id , hpd.payrollslip_no , max( hpd.seq_no ) as seq_no
                    from hrpayrolldet hpd
                    group by hpd.coop_id , hpd.payrollslip_no 
                    ) hprd
                    where kpt.coop_id = hep.coop_id
                    and kpt.member_no = hep.ref_membno
                    and kpt.coop_id = hpr.coop_id (+)
                    and kpt.recv_period = hpr.payroll_period (+)
                    and hpr.coop_id = hprd.coop_id(+)
                    and hpr.payrollslip_no = hprd.payrollslip_no(+)
                    and hep.coop_id = hpr.coop_id
                    and hep.emp_no = hpr.emp_no
                    and hep.emp_status = 1   
                    and hpr.post_status = 0                 
                    and kpt.recv_period = '" + ls_period + @"'
                    and hep.emptype_code = '" + ls_emptype + @"'
                    group by kpt.coop_id, hpr.payrollslip_no , ( nvl( hprd.seq_no , 0 ) + 1 )
                    )";

                }
               
                exe.SQL.Add(ls_sqlinsert);

                //hrbasepayrollfixed
                ls_sqlinsert = @"insert into hrpayrolldet
                    (
                    select hep.coop_id,
                        hpr.payrollslip_no,
                        ( ( rank() over( partition by hpf.coop_id , hpf.emp_no order by hpf.seq_no ) ) + nvl( hprd.seq_no , 0 ) ) as seq_no ,
                        hpf.salitem_code,
                        hus.salitem_desc,
                        hpf.item_amt
                    from hremployee hep,
                        hrpayroll hpr,
                        hrbasepayrollfixed hpf,
                        hrucfsalaryitem hus ,
                        (
                        select hpd.coop_id , hpd.payrollslip_no , max( hpd.seq_no) as seq_no 
                        from hrpayrolldet hpd 
                        group by hpd.coop_id , hpd.payrollslip_no 
                        ) hprd
                    where  hep.coop_id = hpr.coop_id
                    and hep.emp_no = hpr.emp_no
                    and hep.coop_id = hpf.coop_id
                    and hep.emp_no = hpf.emp_no
                    and hpf.salitem_code = hus.salitem_code
                    and hpr.coop_id = hprd.coop_id(+)
                    and hpr.payrollslip_no = hprd.payrollslip_no(+)
                    and hpr.post_status = 0
                    and hep.emp_status = 1
                    and hep.emptype_code = '" + ls_emptype + @"'
                    )";
                exe.SQL.Add(ls_sqlinsert);

                //hrbasepayrollother
//                ls_sqlinsert = @"insert into hrpayrolldet
//                    (
//                    select hep.coop_id,
//                        hpr.payrollslip_no,
//                        ( ( rank() over( partition by hpo.coop_id , hpo.emp_no order by hpo.seq_no ) ) + nvl( hprd.seq_no , 0 ) ) as seq_no ,
//                        hpo.salitem_code,
//                        hus.salitem_desc,
//                        hpo.item_amt
//                    from hremployee hep,
//                        hrpayroll hpr,
//                        hrbasepayrollother hpo,
//                        hrucfsalaryitem hus ,
//                        (
//                        select hpd.coop_id , hpd.payrollslip_no , max( hpd.seq_no) as seq_no 
//                        from hrpayrolldet hpd 
//                        group by hpd.coop_id , hpd.payrollslip_no 
//                        ) hprd
//                    where  hep.coop_id = hpr.coop_id
//                    and hep.emp_no = hpr.emp_no
//                    and hep.coop_id = hpo.coop_id
//                    and hep.emp_no = hpo.emp_no
//                    and hpo.salitem_code = hus.salitem_code
//                    and hpr.coop_id = hprd.coop_id(+)
//                    and hpr.payrollslip_no = hprd.payrollslip_no(+)
//                    and hpr.post_status = 0
//                    and hep.emp_status = 1
//                    and hep.emptype_code = '" + ls_emptype + @"'
//                    and hpo.payroll_period = '" + ls_period + @"'
//                    )";
//                exe.SQL.Add(ls_sqlinsert);                

                //update เงินเดือน
                ls_sqlupdate = @"update hrpayroll hrp 
                    set hrp.salaryoth_amt =
                    (
	                    select  sum( hrpd.item_amt ) as salaryoth_amt	                    
	                    from hrpayrolldet hrpd,
	                    hrucfsalaryitem hus
	                    where hrp.coop_id = hrpd.coop_id
	                    and hrp.payrollslip_no = hrpd.payrollslip_no
	                    and hrpd.salitem_code = hus.salitem_code
                        and hrp.payroll_period = '" + ls_period + @"'
                        and hus.sign_flag = 1
                        and hrpd.salitem_code <> 'R01'              
                    )";
                exe.SQL.Add(ls_sqlupdate);

                //เงินประกันสังคม hrpayrolldet
                ldc_ssemprate = Convert.ToDecimal(ss_emprate.Text);
                ls_sqlinsert = @"insert into hrpayrolldet
                    (
                    select hep.coop_id,
                     hpr.payrollslip_no,
                    ( select nvl( max( hpd.seq_no) , 0 ) + 1 from hrpayrolldet hpd where hpd.coop_id = hpr.coop_id and hpd.payrollslip_no = hpr.payrollslip_no ) as seq_no , 
                    'P12',
                    'หักเบี้ยประกันสังคม',
                    ( case when (nvl( hpr.salarybase_amt, 0 ) + nvl( hpr.salaryoth_amt, 0) ) > hcf.ss_maxsalary then hcf.ss_maxsalary else (nvl( hpr.salarybase_amt, 0 ) + nvl( hpr.salaryoth_amt, 0) ) end * ( " + ldc_ssemprate + @"/100 ) ) as ss_amt
                    from hremployee hep,
                    hrcfconstant hcf,
                    hrpayroll hpr
                    where hep.coop_id = hpr.coop_id
                    and hep.emp_no = hpr.emp_no
                    and hep.emp_status = 1
                    and hep.ss_status = 1
                    and hpr.post_status = 0
                    and hep.emptype_code = '" + ls_emptype + @"'
                    and hpr.payroll_period = '" + ls_period + "')";
                exe.SQL.Add(ls_sqlinsert);

                //update เงินหัก เงินเดือนสุทธิ
                ls_sqlupdate = @"update hrpayroll hrp 
                    set ( hrp.salarysubt_amt , hrp.salarynet_amt ) =
                    (
	                    select sum( case when hus.sign_flag = -1 then hrpd.item_amt end ) as salarysubt_amt ,
	                    sum( case when hus.sign_flag = 1 then hrpd.item_amt end ) - sum( case when hus.sign_flag = -1 then hrpd.item_amt end ) as salarynet_amt
	                    from hrpayrolldet hrpd,
	                    hrucfsalaryitem hus
	                    where hrp.coop_id = hrpd.coop_id
	                    and hrp.payrollslip_no = hrpd.payrollslip_no
	                    and hrpd.salitem_code = hus.salitem_code
                        and hrp.payroll_period = '" + ls_period + @"'
	                    group by hrpd.payrollslip_no
                    )";
                exe.SQL.Add(ls_sqlupdate);
                }

                try
                {
                    int result = exe.Execute();
                    exe.SQL.Clear();
                    LtServerMessage.Text = WebUtil.CompleteMessage("ประมวลผลสำเร็จ");
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                }
            }
        }

        public void of_salaryprc()
        {

        }

        public void SaveWebSheet()
        {

        }

        public void WebSheetLoadEnd()
        {

        }
    }
}