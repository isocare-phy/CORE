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
    public partial class DsSalary : DataSourceFormView
    {
        public DataSet2.HRNPAYROLL_SETDataTable DATA { get; private set; }

        public void InitDs(PageWeb pw)
        {
            css1.Visible = false;
            DataSet2 ds = new DataSet2();
            this.DATA = ds.HRNPAYROLL_SET;
            this.InitDataSource(pw, FormView1, this.DATA, "dsTaxcal");
            this.EventItemChanged = "OnDsTaxcalItemChanged";
            this.Register();
         
        }

        public void RetreiveEmplid(String emplid, String year, int month)
        {
            int year_pay = Convert.ToInt16(year)-543;
            string sql = @"
            select 
sum(case when HRNPAYROLL_SET.SEQ_PAY='R01' AND  HRNPAYROLL_SET.SETPAYROLL_AMT is not null  then HRNPAYROLL_SET.SETPAYROLL_AMT  else 0 end) as salary,
sum(case when HRNPAYROLL_SET.SEQ_PAY='R99' AND  HRNPAYROLL_SET.SETPAYROLL_AMT is not null  then HRNPAYROLL_SET.SETPAYROLL_AMT  else 0 end) as bonus
from HRNPAYROLL_SET,HRNMLEMPLFILEMAS where HRNPAYROLL_SET.EMPLID= HRNMLEMPLFILEMAS.EMPLID and  HRNMLEMPLFILEMAS.emplcode = '" + emplid + "'";
            DataTable dt = WebUtil.Query(sql);


            this.ImportData(dt);

        }
   
    }

}