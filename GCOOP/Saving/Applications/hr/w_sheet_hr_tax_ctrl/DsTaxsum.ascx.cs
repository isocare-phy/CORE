using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace Saving.Applications.hr.w_sheet_hr_tax_ctrl
{
    public partial class DsTaxsum : DataSourceFormView
    {
        public DataSet2.HRNPAYROLL_TAXSETDataTable DATA { get; private set; }

        public void InitDs(PageWeb pw)
        {
            css1.Visible = false;
            DataSet2 ds = new DataSet2();
            this.DATA = ds.HRNPAYROLL_TAXSET;
            this.InitDataSource(pw, FormView1, this.DATA, "dsTaxsum");
            this.EventItemChanged = "OnDsTaxsumItemChanged";
            this.Register();

        }

        public void RetreiveEmplid(String emplid, String year, int month)
        {
            int year_pay = Convert.ToInt16(year) - 543;
            int month_pay = month + 1;
            string sql = @"
           
           select sum(case when hrnpayroll_taxset.seq_tax = 'A01' AND   hrnpayroll_taxset.taxlist1_amt is not null  then hrnpayroll_taxset.taxlist1_amt  else 0 end) as A01
				,sum(case when hrnpayroll_taxset.seq_tax = 'A02' AND   hrnpayroll_taxset.taxlist1_amt is not null  then hrnpayroll_taxset.taxlist1_amt  else 0 end) as A02
				,sum(case when hrnpayroll_taxset.seq_tax = 'A03' AND   hrnpayroll_taxset.taxlist1_amt is not null  then hrnpayroll_taxset.taxlist1_amt  else 0 end) as A03
				,sum(case when hrnpayroll_taxset.seq_tax = 'A04' AND   hrnpayroll_taxset.taxlist1_amt is not null  then hrnpayroll_taxset.taxlist1_amt  else 0 end) as A04
				,sum(case when hrnpayroll_taxset.seq_tax = 'A05' AND   hrnpayroll_taxset.taxlist1_amt is not null  then hrnpayroll_taxset.taxlist1_amt  else 0 end) as A05
				,sum(case when hrnpayroll_taxset.seq_tax = 'A06' AND   hrnpayroll_taxset.taxlist1_amt is not null  then hrnpayroll_taxset.taxlist1_amt  else 0 end) as A06
				,sum(case when hrnpayroll_taxset.seq_tax = 'A07' AND   hrnpayroll_taxset.taxlist1_amt is not null  then hrnpayroll_taxset.taxlist1_amt  else 0 end) as A07
				,sum(case when hrnpayroll_taxset.seq_tax = 'A08' AND   hrnpayroll_taxset.taxlist1_amt is not null  then hrnpayroll_taxset.taxlist1_amt  else 0 end) as A08
				,sum(case when hrnpayroll_taxset.seq_tax = 'A09' AND   hrnpayroll_taxset.taxlist1_amt is not null  then hrnpayroll_taxset.taxlist1_amt  else 0 end) as A09
				,sum(case when hrnpayroll_taxset.seq_tax = 'A10' AND   hrnpayroll_taxset.taxlist1_amt is not null  then hrnpayroll_taxset.taxlist1_amt  else 0 end) as A10
				,sum(case when hrnpayroll_taxset.seq_tax = 'A11' AND   hrnpayroll_taxset.taxlist1_amt is not null  then hrnpayroll_taxset.taxlist1_amt  else 0 end) as A11
				,sum(case when hrnpayroll_taxset.seq_tax = 'A12' AND   hrnpayroll_taxset.taxlist1_amt is not null  then hrnpayroll_taxset.taxlist1_amt  else 0 end) as A12
				,sum(case when hrnpayroll_taxset.seq_tax = 'A13' AND   hrnpayroll_taxset.taxlist1_amt is not null  then hrnpayroll_taxset.taxlist1_amt  else 0 end) as A13
				,sum(case when hrnpayroll_taxset.seq_tax = 'A14' AND   hrnpayroll_taxset.taxlist1_amt is not null  then hrnpayroll_taxset.taxlist1_amt  else 0 end) as A14

from
hrnpayroll_taxcfg, HRNMLEMPLFILEMAS ,hrnpayroll_taxset 
where hrnpayroll_taxset.emplid = HRNMLEMPLFILEMAS.emplid and hrnpayroll_taxset.seq_tax = hrnpayroll_taxcfg.seq_tax
and hrnpayroll_taxset.pay_year = '" + year_pay + "' and hrnpayroll_taxset.pay_month = '" + month_pay + "' and HRNMLEMPLFILEMAS.emplcode = '" + emplid + @"'
group by hrnpayroll_taxset.emplid";
            DataTable dt = WebUtil.Query(sql);


            this.ImportData(dt);

        }

    }

}