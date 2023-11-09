using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataLibrary;
using System.Globalization;


namespace Saving.Applications.mis.w_sheet_mssysbal_msv_ctrl
{
    public partial class w_sheet_mssysbal_msv: PageWebSheet, WebSheet
    {
        [JsPostBack]
        public string PostInsertRow { get; set; } //สำสั่ง JS postback
        [JsPostBack]
        public string PostDeleteRow { get; set; } //สำสั่ง JS postback

        [JsPostBack]
        public string postsearch { get; set; } //สำสั่ง JS postback
        [JsPostBack]
        public string postprocess { get; set; } //สำสั่ง JS postback


        public void InitJsPostBack()
        {
            dsMain.InitDsMain(this);
            dsList.InitDsList(this);
        }

        public void WebSheetLoadBegin()
        {

            if (!IsPostBack)
            {
                dsMain.DATA[0].work_date = state.SsWorkDate;
                //dsList.retrieve(state.SsWorkDate);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == PostInsertRow)
            {
                dsList.InsertAtRow(0);
            }
            else if (eventArg == PostDeleteRow)
            {
                int Rowdelete = dsList.GetRowFocus();
                dsList.DeleteRow(Rowdelete);
            }
            else if (eventArg == postsearch)
            {
                //String a = state.SsWorkDate.ToString("ddMMyyyy", new CultureInfo("th-TH"));
                //String b = dsMain.DATA[0].work_date.Year.ToString();
                //int y = Convert.ToInt16(b) - 543;
                //String datework = dsMain.DATA[0].work_date.Day.ToString("00/") + dsMain.DATA[0].work_date.Month.ToString("00/") + dsMain.DATA[0].work_date.Year.ToString();
                //DateTime workwork = Convert.ToDateTime(datework);
                try
                {
                    dsList.ResetRow();
                    dsList.retrieve(dsMain.DATA[0].work_date);
                    if (dsList.RowCount <= 0) throw new Exception("ไม่พบข้อมูล");
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                }
            }
            else if (eventArg == postprocess)
            {
                dsList.sql_mssysbal(dsMain.DATA[0].work_date);
                dsList.retrieve(dsMain.DATA[0].work_date);
            }
        }

        public void SaveWebSheet()
        {
            try
            {
                ExecuteDataSource exed = new ExecuteDataSource(this);
                exed.AddRepeater(dsList);

                int result = exed.Execute();

                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ"); //หากบันทึกให้ขึ้น บันทึกแล้ว
                dsList.retrieve(dsMain.DATA[0].work_date);
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message); // กรณีไม่สำเร็จ
            }

        }

        public void WebSheetLoadEnd()
        {
        }

        public void sql_process()
        {
            delete_MsSysBal_s();
            insert_shsharemaster_s();
            insert_lncontmaster_s();
            insert_dpdeptmaster_s();
            insert_pminvestmaster_s();
            updateAll_Rate_s();
        }
        public void delete_MsSysBal_s()
        {
            try
            {
                string sql = "delete from mssysbal where bal_date = to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/MM/yyyy') ";
                Sdt res = WebUtil.QuerySdt(sql);
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message); // กรณีไม่สำเร็จ
            }
        }
        public void insert_shsharemaster_s()
        {
            try
            {
                string sql = @"insert into mssysbal
( coop_id , bal_date , seq_no , bizz_system , bizztype_code , balance_value , bizztype_rate )
select sm.branch_id ,  to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy", WebUtil.EN) + @"','dd/MM/yyyy') as bal_date ,
( nvl( ( select max( seq_no ) from mssysbal where bal_date =  to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy", WebUtil.EN) + @"','dd/MM/yyyy') ) , 0 ) + ( ROW_NUMBER()OVER(ORDER BY sm.branch_id , sm.sharetype_code  ) ) ) as seq_no ,
'SHR' as bizz_system , sm.sharetype_code ,
sum( sm.sharestk_amt * st.share_value ) as balance_value , 0.00 as bizztype_rate
from shsharemaster sm , shsharetype st
where sm.sharetype_code = st.sharetype_code
and sm.branch_id in ('000','001','002','003','004')
and sm.sharestk_amt > 0
group by sm.branch_id , sm.sharetype_code 
order by sm.branch_id , sm.sharetype_code ";

                //Sdt res = WebUtil.QuerySdt(sql);
                Sdt res_count = WebUtil.QuerySdt(sql);
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message); // กรณีไม่สำเร็จ
            }
        }
        public void insert_lncontmaster_s()
        {
            try
            {
                string sql = @"insert into mssysbal
( coop_id , bal_date , seq_no , bizz_system , bizztype_code , balance_value , bizztype_rate )
select lm.branch_id ,  to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy", WebUtil.EN) + @"','dd/MM/yyyy')
 as bal_date , 
( nvl( ( select max( seq_no ) from mssysbal where bal_date =  to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy", WebUtil.EN) + @"','dd/MM/yyyy') ) , 0 ) + ( ROW_NUMBER()OVER(ORDER BY lm.branch_id , lm.loantype_code) ) ) as seq_no ,
'LON' as bizz_system , lm.loantype_code ,
sum( lm.principal_balance ) as balance_value , 0.00 as bizztype_rate
from lncontmaster lm
where lm.branch_id in ('000','001','002','003','004') 
and principal_balance > 0
group by lm.branch_id , lm.loantype_code
order by lm.branch_id , lm.loantype_code";

                Sdt res = WebUtil.QuerySdt(sql);
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message); // กรณีไม่สำเร็จ
            }
        }
        public void insert_dpdeptmaster_s()
        {
            try
            {
                string sql = @"insert into mssysbal
( coop_id , bal_date , seq_no , bizz_system , bizztype_code , balance_value , bizztype_rate )
select dm.branch_id,  to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy", WebUtil.EN) + @"','dd/MM/yyyy') as bal_date , 
( nvl( ( select max( seq_no ) from mssysbal where bal_date =  to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy", WebUtil.EN) + @"','dd/MM/yyyy') ) , 0 ) + (ROW_NUMBER()OVER(ORDER BY dm.branch_id , dm.depttype_code)  ) ) as seq_no ,
'DEP' as bizz_system , dm.depttype_code ,
sum( dm.prncbal ) as balance_value , 0.00 as bizztype_rate
from dpdeptmaster dm
where dm.branch_id in ('000','001','002','003','004')
and prncbal > 0
group by dm.branch_id , dm.depttype_code
order by dm.branch_id , dm.depttype_code";

                Sdt res = WebUtil.QuerySdt(sql);
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message); // กรณีไม่สำเร็จ
            }
        }
        public void insert_pminvestmaster_s()
        {
            try
            {
                string sql = @"insert into mssysbal
( coop_id , bal_date , seq_no , bizz_system , bizztype_code , balance_value , bizztype_rate )
select member_branchid ,  to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy", WebUtil.EN) + @"','dd/MM/yyyy') as bal_date ,
( nvl( ( select max( seq_no ) from mssysbal where bal_date =  to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy", WebUtil.EN) + @"','dd/MM/yyyy') ) , 0 ) + ( ROW_NUMBER()OVER(ORDER BY member_branchid ,  depttype_code ) ) ) as seq_no ,
'PMI' as bizz_system ,  depttype_code  as bizztype_code , sum( prncbal ) as balance_value , sum( weigth ) as bizztype_rate
from(
	select  pi.member_branchid , pi.deptaccount_no , pi.depttype_code , sum( pi.prncbal ) as prncbal , 
	round(( ( sum( pi.prncbal ) * (pi.spcint_rate) ) / sum.prncbal ),4) as weigth
from pmdeptmaster pi, (select member_branchid , depttype_code , sum( prncbal ) as prncbal from pmdeptmaster where prncbal > 0 group by member_branchid , depttype_code) sum
where pi.member_branchid = sum.member_branchid
	and pi.depttype_code = sum.depttype_code
	and pi.prncbal > 0
group by pi.member_branchid , pi.deptaccount_no , pi.depttype_code , pi.spcint_rate , sum.prncbal
)
group by member_branchid ,  depttype_code 
order by member_branchid ,  depttype_code";

                Sdt res = WebUtil.QuerySdt(sql);
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message); // กรณีไม่สำเร็จ
            }
        }

        public void updateAll_Rate_s()
        {
            //Rate SHR
            try
            {
                string sql = @"update mssysbal ms
set ms.bizztype_rate = 
(
	select max( ycr.divpercent_rate )
	from yrcfrate ycr , cmaccountyear cay
	where cay.coop_id = ycr.coop_id
	and to_char( cay.account_year ) = substr( ycr.div_year , 1 , 4 )
	and  to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy", WebUtil.EN) + @"','dd/MM/yyyy') between cay.accstart_date and cay.accend_date
	and ms.coop_id = ycr.coop_id
)
where ms.coop_id = '" + state.SsCoopId + @"' 
and ms.bal_date =  to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy", WebUtil.EN) + @"','dd/MM/yyyy')
and ms.bizz_system = 'SHR'";

                //Sdt res = WebUtil.QuerySdt(sql);
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message); // กรณีไม่สำเร็จ
            }


            //Rate LON
            try
            {
                string sql = @"update mssysbal ms
set ms.bizztype_rate = 
(
	select avg( max(lci.interest_rate)) as interest_rate
	from lnloanintrate lci
	where  to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy", WebUtil.EN) + @"','dd/MM/yyyy') >= lci.effective_date
	and ms.bizztype_code = lci.loantype_code
	group by lci.loantype_code
)
where ms.bal_date =  to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy", WebUtil.EN) + @"','dd/MM/yyyy')
and ms.bizz_system = 'LON'";

                Sdt res = WebUtil.QuerySdt(sql);
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message); // กรณีไม่สำเร็จ
            }


            //Rate DEP
            try
            {
                string sql = @"update mssysbal ms
set ms.bizztype_rate = 
(
	select round(avg( dir.interest_rate ),4)
	from dpdepttype dt , dpdeptintrate dir , 
	( 	select depttype_code , max( effective_date ) as effective_date from dpdeptintrate where effective_date <=  to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy", WebUtil.EN) + @"','dd/MM/yyyy') group by depttype_code ) tpdir
	where dt.depttype_code = dir.depttype_code
	and dir.depttype_code = tpdir.depttype_code
	and dir.effective_date = tpdir.effective_date
	and dt.depttype_code = ms.bizztype_code
	group by dt.depttype_code
)
where ms.bal_date =  to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy", WebUtil.EN) + @"','dd/MM/yyyy')
and ms.bizz_system = 'DEP'";

                Sdt res = WebUtil.QuerySdt(sql);
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message); // กรณีไม่สำเร็จ
            }


            /*กรณีไม่พบ Rate*/
            try
            {
                string sql = @"update mssysbal
set bizztype_rate = 0.05
where bal_date =  to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy", WebUtil.EN) + @"','dd/MM/yyyy')
and bizztype_rate is null";

                Sdt res = WebUtil.QuerySdt(sql);
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message); // กรณีไม่สำเร็จ
            }
        }
    }
}