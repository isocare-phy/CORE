using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DataLibrary;


namespace Saving.Applications.mis.w_sheet_mssysbal_msv_ctrl
{
    public partial class DsList : DataSourceRepeater
    {
        public DataSet1.MSSYSBALDataTable DATA { get; set; }

        public void InitDsList(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.MSSYSBAL;

            this.EventClicked = "OnDsListClicked";
            this.InitDataSource(pw, Repeater1, this.DATA, "dsList");
            this.Register();

        }

        public void retrieve(DateTime _date)
        {
            //string b = "27/12/2013";
            //            string sql = @"  select decode(captype_code,'01','จัดหาทุน','02','ลงทุน') as bizz_system_det,
            //                b.description as bizz_system_desc,
            //                (CASE bizz_system
            //			         WHEN 'DEP' THEN (select d.depttype_desc 
            //										      from dpdepttype d 
            //										      where d.depttype_code=trim(a.bizztype_code))
            //  				     WHEN 'SHR' THEN 'หุ้นสามัญ'
            //				     WHEN 'LON' THEN (select l.loantype_desc 
            //										      from lnloantype l 
            //										      where l.loantype_code= trim(a.bizztype_code))
            //				      WHEN 'PMI' THEN (select investtype_desc 
            //										      from pmucfinvest_type p 
            //										      where p.investtype_code= trim(a.bizztype_code))
            // 				      ELSE bizz_system
            //				      END
            //                  ) as BIZZTYPE_CODE_DESC,
            //		        a.balance_value, round(a.bizztype_rate *100 ,2) as bizztype_rate
            //                from mssysbal a,MISCAPITALTYPE  b
            //                where a.bizz_system=b.system_code
            //		        and a.bal_date = to_date('" + _date.ToString("dd/MM/yyyy") + @"','dd/MM/yyyy' ) 
            //		        and a.coop_id = '" + state.SsCoopId + @"'  
            //                order by b.seq_no,a.seq_no ";

            string sql = @"select decode(captype_code,'01','จัดหาทุน','02','ลงทุน') as bizz_system_det,
                b.description as bizz_system_desc,
                (CASE bizz_system
			         WHEN 'DEP' THEN (select d.depttype_desc 
										      from dpdeptmidgroup d 
										      where d.depttype_group=trim(a.bizztype_code))
  				     WHEN 'SHR' THEN 'หุ้นสามัญ'
				     WHEN 'LON' THEN (select l.loantype_desc 
										      from lnloantype l 
										      where l.loantype_code= trim(a.bizztype_code))
				      WHEN 'PMI' THEN (select depttype_desc 
										      from pmdepttype p 
										      where p.depttype_code= trim(a.bizztype_code))
 				      ELSE bizz_system
				      END
                  ) as BIZZTYPE_CODE_DESC,
		        a.balance_value, round(a.bizztype_rate *100 ,2) as bizztype_rate
                from mssysbal a,MISCAPITALTYPE  b
                where a.bizz_system=b.system_code(+)
                and a.coop_id like '%'
		        and a.bal_date = {1}		          
                order by b.seq_no,a.seq_no";
            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, _date);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }

        public void sql_mssysbal(DateTime _date)
        {
            delete_MsSysBal_(_date);
            insert_shsharemaster(_date);
            insert_lncontmaster(_date);
            insert_dpdeptmaster(_date);
            insert_pminvestmaster(_date);
            updateAll_Rate(_date);
        }

        public void delete_MsSysBal_(DateTime _date)
        {
            try
            {
                string sql = "delete from mssysbal where bal_date = to_date('" + _date.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/MM/yyyy') ";
                Sdt res = WebUtil.QuerySdt(sql);
            }
            catch { }
        }
        public void insert_shsharemaster(DateTime _date)
        {
            try
            {
                string sql = @"insert into mssysbal
( coop_id , bal_date , seq_no , bizz_system , bizztype_code , balance_value , bizztype_rate )
select '000',  to_date('" + _date.ToString("dd/MM/yyyy", WebUtil.EN) + @"','dd/MM/yyyy') as bal_date ,
( nvl( ( select max( seq_no ) from mssysbal where bal_date =  to_date('" + _date.ToString("dd/MM/yyyy", WebUtil.EN) + @"','dd/MM/yyyy') ) , 0 ) + ( ROW_NUMBER()OVER(ORDER BY sm.sharetype_code  ) ) ) as seq_no ,
'SHR' as bizz_system , sm.sharetype_code ,
sum( sm.sharestk_amt * st.share_value ) as balance_value , 0.00 as bizztype_rate
from shsharemaster sm , shsharetype st
where sm.sharetype_code = st.sharetype_code
and sm.branch_id in ('000','001','002','003','004')
and sm.sharestk_amt > 0
group by sm.sharetype_code ";

                //Sdt res = WebUtil.QuerySdt(sql);
                Sdt res_count = WebUtil.QuerySdt(sql);
            }
            catch { }
        }
        public void insert_lncontmaster(DateTime _date)
        {
            try
            {
                string sql = @"insert into mssysbal
( coop_id , bal_date , seq_no , bizz_system , bizztype_code , balance_value , bizztype_rate )
select '000' ,  to_date('" + _date.ToString("dd/MM/yyyy", WebUtil.EN) + @"','dd/MM/yyyy')
 as bal_date , 
( nvl( ( select max( seq_no ) from mssysbal where bal_date =  to_date('" + _date.ToString("dd/MM/yyyy", WebUtil.EN) + @"','dd/MM/yyyy') ) , 0 ) + ( ROW_NUMBER()OVER(ORDER BY lm.loantype_code) ) ) as seq_no ,
'LON' as bizz_system , lm.loantype_code ,
sum( lm.principal_balance ) as balance_value , 0.00 as bizztype_rate
from lncontmaster lm
where lm.branch_id in ('000','001','002','003','004') 
and principal_balance > 0
group by lm.loantype_code";

                Sdt res = WebUtil.QuerySdt(sql);
            }
            catch { }
        }
        public void insert_dpdeptmaster(DateTime _date)
        {
            try
            {
                string sql = @"insert into mssysbal
( coop_id , bal_date , seq_no , bizz_system , bizztype_code , balance_value , bizztype_rate )
select '000',  to_date('" + _date.ToString("dd/MM/yyyy", WebUtil.EN) + @"','dd/MM/yyyy') as bal_date , 
( nvl( ( select max( seq_no ) from mssysbal where bal_date =  to_date('" + _date.ToString("dd/MM/yyyy", WebUtil.EN) + @"','dd/MM/yyyy') ) , 0 ) + (ROW_NUMBER()OVER(ORDER BY  dpdeptmidgroup.depttype_group)  ) ) as seq_no ,
'DEP' as bizz_system ,
dpdeptmidgroup.depttype_group,
sum(balance_value) as balance_value,
avg(bizztype_rate) as bizztype_rate
from dpdeptmidgroup,
(
select  
dm.depttype_code ,
(select depttype_group from dpdepttype where dpdepttype.depttype_code  = dm.depttype_code) as depttype_group,
sum( dm.prncbal ) as balance_value , 
(select round(avg( dir.interest_rate ),4)
	from dpdepttype dt , dpdeptintrate dir , 
	( 	select depttype_code , max( effective_date ) as effective_date from dpdeptintrate where effective_date <=  to_date('" + _date.ToString("dd/MM/yyyy", WebUtil.EN) + @"','dd/MM/yyyy') group by depttype_code ) tpdir
	where dt.depttype_code = dir.depttype_code
	and dir.depttype_code = tpdir.depttype_code
	and dir.effective_date = tpdir.effective_date
	and dt.depttype_code = dm.depttype_code
	group by dt.depttype_code) as bizztype_rate
from dpdeptmaster dm
where dm.branch_id in ('000','001','002','003','004')
and prncbal > 0
group by  dm.depttype_code
) s
where s.depttype_group = dpdeptmidgroup.depttype_group
group by dpdeptmidgroup.depttype_group";

                Sdt res = WebUtil.QuerySdt(sql);
            }
            catch { }
        }
        public void insert_pminvestmaster(DateTime _date)
        {
            try
            {
                string sql = @"insert into mssysbal
( coop_id , bal_date , seq_no , bizz_system , bizztype_code , balance_value , bizztype_rate )
select '000' ,  to_date('" + _date.ToString("dd/MM/yyyy", WebUtil.EN) + @"','dd/MM/yyyy') as bal_date ,
( nvl( ( select max( seq_no ) from mssysbal where bal_date =  to_date('" + _date.ToString("dd/MM/yyyy", WebUtil.EN) + @"','dd/MM/yyyy') ) , 0 ) + ( ROW_NUMBER()OVER(ORDER BY   depttype_code ) ) ) as seq_no ,
'PMI' as bizz_system ,  depttype_code  as bizztype_code , sum( prncbal ) as balance_value , sum( weigth ) as bizztype_rate
from(
	select  pi.member_branchid , pi.deptaccount_no , pi.depttype_code , sum( pi.prncbal ) as prncbal , 
	round(( ( sum( pi.prncbal ) * (pi.spcint_rate/100) ) / sum.prncbal ),4) as weigth
from pmdeptmaster pi, (select member_branchid , depttype_code , sum( prncbal ) as prncbal from pmdeptmaster where prncbal > 0 group by member_branchid , depttype_code) sum
where pi.member_branchid = sum.member_branchid
	and pi.depttype_code = sum.depttype_code
	and pi.prncbal > 0
group by pi.member_branchid , pi.deptaccount_no , pi.depttype_code , pi.spcint_rate , sum.prncbal
)
group by  depttype_code ";

                Sdt res = WebUtil.QuerySdt(sql);
            }
            catch { }
        }

        public void updateAll_Rate(DateTime _date)
        {
            //Rate SHR
            try
            {
                string sql = @"update mssysbal ms
set ms.bizztype_rate = 
(
	select max( ycr.divpercent_rate/100 )
	from yrcfrate ycr , cmaccountyear cay
	where cay.coop_id = ycr.coop_id
	and to_char( cay.account_year ) = substr( ycr.div_year , 1 , 4 )
	and  to_date('" + _date.ToString("dd/MM/yyyy", WebUtil.EN) + @"','dd/MM/yyyy') between cay.accstart_date and cay.accend_date
	and ms.coop_id = ycr.coop_id
)
where ms.coop_id = '" + state.SsCoopId + @"' 
and ms.bal_date =  to_date('" + _date.ToString("dd/MM/yyyy", WebUtil.EN) + @"','dd/MM/yyyy')
and ms.bizz_system = 'SHR'";

                Sdt res = WebUtil.QuerySdt(sql);
            }
            catch { }


            //Rate LON
            try
            {
                string sql = @"update mssysbal ms
set ms.bizztype_rate = 
(
	select round(avg( lci.interest_rate),4) as interest_rate
	from lnloanintrate lci
	where  to_date('" + _date.ToString("dd/MM/yyyy", WebUtil.EN) + @"','dd/MM/yyyy') >= lci.effective_date
	and ms.bizztype_code = lci.loantype_code
	group by lci.loantype_code
)
where ms.bal_date =  to_date('" + _date.ToString("dd/MM/yyyy", WebUtil.EN) + @"','dd/MM/yyyy')
and ms.bizz_system = 'LON'";

                Sdt res = WebUtil.QuerySdt(sql);
            }
            catch { }


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
where ms.bal_date =  to_date('" + _date.ToString("dd/MM/yyyy", WebUtil.EN) + @"','dd/MM/yyyy')
and ms.bizz_system = 'DEP'";

                //Sdt res = WebUtil.QuerySdt(sql);
            }
            catch { }


            /*กรณีไม่พบ Rate*/
            try
            {
                string sql = @"update mssysbal
set bizztype_rate = 0.05
where bal_date =  to_date('" + _date.ToString("dd/MM/yyyy", WebUtil.EN) + @"','dd/MM/yyyy')
and bizztype_rate is null";

                Sdt res = WebUtil.QuerySdt(sql);
            }
            catch { }
        }
    }
}