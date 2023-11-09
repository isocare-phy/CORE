using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace Saving.Applications.hr.w_sheet_hr_salamas_ctrl
{
    public partial class DsTran : DataSourceFormView
    {
        public DataSet1.HRNPAYROLLDataTable DATA { get; private set; }

        public void InitDs(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.HRNPAYROLL;
            this.InitDataSource(pw, FormView1, this.DATA, "dsTran");
            this.EventItemChanged = "OnDsTranItemChanged";
            this.EventClicked = "OnDsTranClicked";
           

            this.Register();
        }

        public void Retrivetran(String year,String emplcode,int month)
        {
            int year_pay = Convert.ToInt16(year) - 543;
            int month_pay = month + 1;

            string sql = @"select distinct(select tran_acc from hrnpayroll,hrnmlemplfilemas where seq_pay = 'N03' and hrnpayroll.emplid
= hrnmlemplfilemas.emplid and pay_year = '" + year + "' and pay_month='" + month_pay + "' and hrnmlemplfilemas.emplid = '" + emplcode + @"' ) as tran_acc,
(select tran_date from hrnpayroll,hrnmlemplfilemas  where seq_pay = 'N03' and hrnpayroll.emplid
= hrnmlemplfilemas.emplid and pay_year = '" + year + "' and pay_month='" + month_pay + "' and hrnmlemplfilemas.emplid = '" + emplcode + @"') as tran_date from hrnpayroll,hrnmlemplfilemas  where pay_year = '" + year + "' and pay_month='" + month_pay + "' and hrnmlemplfilemas.emplid = '" + emplcode + "'";

            DataTable dt = WebUtil.Query(sql);


            this.ImportData(dt);

        }
    }
}