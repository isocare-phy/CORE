using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DataLibrary;


namespace Saving.Applications.mis.w_sheet_mssysbal_ctrl
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
										      from dpdepttype d 
										      where d.depttype_code=trim(a.bizztype_code))
  				     WHEN 'SHR' THEN 'หุ้นสามัญ'
				     WHEN 'LON' THEN (select l.loantype_desc 
										      from lnloantype l 
										      where l.loantype_code= trim(a.bizztype_code))
				      WHEN 'PMI' THEN (select investtype_desc 
										      from pmucfinvest_type p 
										      where p.investtype_code= trim(a.bizztype_code))
 				      ELSE bizz_system
				      END
                  ) as BIZZTYPE_CODE_DESC,
		        a.balance_value, round(a.bizztype_rate *100 ,2) as bizztype_rate
                from mssysbal a,MISCAPITALTYPE  b
                where a.bizz_system=b.system_code(+)
                and a.coop_id = {0}
		        and a.bal_date = {1}		          
                order by b.seq_no,a.seq_no";
            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, _date);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }

        public void sql_mssysbal()
        {
            delete_MsSysBal_();
            insert_shsharemaster();
            insert_lncontmaster();
            insert_dpdeptmaster();
            insert_pminvestmaster();
            updateAll_Rate();
        }

        public void delete_MsSysBal_()
        {
            try
            {
                string sql = "delete from mssysbal where coop_id = '" + state.SsCoopId + "' and bal_date = to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/MM/yyyy') ";
                Sdt res = WebUtil.QuerySdt(sql);
            }
            catch { }
        }
        public void insert_shsharemaster()
        {
            try
            {
                string sql = @"insert into mssysbal
( coop_id , bal_date , seq_no , bizz_system , bizztype_code , balance_value , bizztype_rate )
select sm.coop_id ,  to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy", WebUtil.EN) + @"','dd/MM/yyyy') as bal_date ,
( nvl( ( select max( seq_no ) from mssysbal where coop_id = '" + state.SsCoopId + @"' and bal_date =  to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy", WebUtil.EN) + @"','dd/MM/yyyy') ) , 0 ) + ( rank() over ( partition by sm.coop_id order by sm.sharetype_code ) ) ) as seq_no ,
'SHR' as bizz_system , sm.sharetype_code ,
sum( sm.sharestk_amt * st.unitshare_value ) as balance_value , 
0.00 as bizztype_rate
from shsharemaster sm , shsharetype st
where sm.coop_id = st.coop_id
and sm.sharetype_code = st.sharetype_code
and sm.coop_id = '" + state.SsCoopId + @"' 
and sm.sharestk_amt > 0
group by sm.coop_id , sm.sharetype_code ";

                Sdt res = WebUtil.QuerySdt(sql);
            }
            catch { }
        }
        public void insert_lncontmaster()
        {
            try
            {
                string sql = @"insert into mssysbal
( coop_id , bal_date , seq_no , bizz_system , bizztype_code , balance_value , bizztype_rate )
select lm.coop_id ,  to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy", WebUtil.EN) + @"','dd/MM/yyyy')
 as bal_date , 
( nvl( ( select max( seq_no ) from mssysbal where coop_id = '" + state.SsCoopId + @"' and bal_date =  to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy", WebUtil.EN) + @"','dd/MM/yyyy') ) , 0 ) + ( rank() over ( partition by lm.coop_id order by lm.loantype_code ) ) ) as seq_no ,
'LON' as bizz_system , lm.loantype_code ,
sum( lm.principal_balance ) as balance_value , 0.00 as bizztype_rate
from lncontmaster lm
where lm.coop_id = '" + state.SsCoopId + @"' 
and principal_balance > 0
group by lm.coop_id , lm.loantype_code";

                Sdt res = WebUtil.QuerySdt(sql);
            }
            catch { }
        }
        public void insert_dpdeptmaster()
        {
            try
            {
                string sql = @"insert into mssysbal
( coop_id , bal_date , seq_no , bizz_system , bizztype_code , balance_value , bizztype_rate )
select dm.coop_id ,  to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy", WebUtil.EN) + @"','dd/MM/yyyy') as bal_date , 
( nvl( ( select max( seq_no ) from mssysbal where coop_id = '" + state.SsCoopId + @"' and bal_date =  to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy", WebUtil.EN) + @"','dd/MM/yyyy') ) , 0 ) + ( rank() over ( partition by dm.coop_id order by dm.depttype_code ) ) ) as seq_no ,
'DEP' as bizz_system , dm.depttype_code ,
sum( dm.prncbal ) as balance_value , 0.00 as bizztype_rate
from dpdeptmaster dm
where dm.coop_id = '" + state.SsCoopId + @"' 
and prncbal > 0
group by dm.coop_id , dm.depttype_code";

                Sdt res = WebUtil.QuerySdt(sql);
            }
            catch { }
        }
        public void insert_pminvestmaster()
        {
            try
            {
                string sql = @"insert into mssysbal
( coop_id , bal_date , seq_no , bizz_system , bizztype_code , balance_value , bizztype_rate )
select coop_id ,  to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy", WebUtil.EN) + @"','dd/MM/yyyy') as bal_date ,
( nvl( ( select max( seq_no ) from mssysbal where coop_id = '" + state.SsCoopId + @"' and bal_date =  to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy", WebUtil.EN) + @"','dd/MM/yyyy') ) , 0 ) + ( rank() over ( partition by coop_id order by  investtype_code ) ) ) as seq_no ,
'PMI' as bizz_system ,  investtype_code  as bizztype_code , sum( prncbal ) as balance_value , sum( weigth ) as bizztype_rate
from(
	select pi.coop_id , pi.account_no , pi.investtype_code , sum( pi.prncbal ) as prncbal , 
	( ( sum( pi.prncbal ) * (pi.duration_due/100) ) / sum.prncbal ) as weigth
	from pminvestmaster pi , ( select coop_id , investtype_code , sum( prncbal ) as prncbal from pminvestmaster where coop_id = '" + state.SsCoopId + @"' and prncbal > 0 group by coop_id , investtype_code ) sum
	where pi.coop_id = sum.coop_id
	and pi.investtype_code = sum.investtype_code
	and pi.coop_id = '" + state.SsCoopId + @"' 
	and pi.prncbal > 0
	group by pi.coop_id , pi.account_no , pi.investtype_code , pi.duration_due , sum.prncbal
)
group by coop_id ,  investtype_code ";

                Sdt res = WebUtil.QuerySdt(sql);
            }
            catch { }
        }

        public void updateAll_Rate()
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
	and  to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy", WebUtil.EN) + @"','dd/MM/yyyy') between cay.accstart_date and cay.accend_date
	and ms.coop_id = ycr.coop_id
)
where ms.coop_id = '" + state.SsCoopId + @"' 
and ms.bal_date =  to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy", WebUtil.EN) + @"','dd/MM/yyyy')
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
	select avg( lci.interest_rate/100 ) as interest_rate
	from lnloantype lt ,lncfloanintratedet lci
	where lt.coop_id = lci.coop_id(+)
	and lt.inttabrate_code = lci.loanintrate_code(+)
	and lt.coop_id = '" + state.SsCoopId + @"'
	and  to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy", WebUtil.EN) + @"','dd/MM/yyyy') between lci.effective_date and lci.expire_date
	and ms.coop_id = lt.coop_id
	and ms.bizztype_code = lt.loantype_code
	group by lt.loantype_code
)
where ms.coop_id = '" + state.SsCoopId + @"' 
and ms.bal_date =  to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy", WebUtil.EN) + @"','dd/MM/yyyy')
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
	select avg( dir.interest_rate )
	from dpdepttype dt , dpdeptintrate dir , 
	( 	select coop_id , depttype_code , max( effective_date ) as effective_date from dpdeptintrate where coop_id = '" + state.SsCoopId + @"' 
		and effective_date <=  to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy", WebUtil.EN) + @"','dd/MM/yyyy') group by coop_id , depttype_code ) tpdir
	where dt.coop_id = dir.coop_id
	and dt.depttype_code = dir.depttype_code
	and dir.coop_id = tpdir.coop_id
	and dir.depttype_code = tpdir.depttype_code
	and dir.effective_date = tpdir.effective_date
	and dt.coop_id = '" + state.SsCoopId + @"' 
	and ms.coop_id = dt.coop_id
	and ms.bizztype_code = dt.depttype_code
	group by dt.depttype_code
)
where ms.coop_id = '" + state.SsCoopId + @"' 
and ms.bal_date =  to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy", WebUtil.EN) + @"','dd/MM/yyyy')
and ms.bizz_system = 'DEP'";

                Sdt res = WebUtil.QuerySdt(sql);
            }
            catch { }


            /*กรณีไม่พบ Rate*/
            try
            {
                string sql = @"update mssysbal
set bizztype_rate = 0.00
where coop_id = '" + state.SsCoopId + @"' 
and bal_date =  to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy", WebUtil.EN) + @"','dd/MM/yyyy')
and bizztype_rate is null";

                Sdt res = WebUtil.QuerySdt(sql);
            }
            catch { }
        }
    }
}