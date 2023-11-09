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
    public partial class DsTax : DataSourceRepeater
    {
        public DataSet2.HRNUCFTAX_RATEDataTable DATA { get; set; }

        public void InitDsList(PageWeb pw)
        {
            css1.Visible = false;
            DataSet2 ds = new DataSet2();
            this.DATA = ds.HRNUCFTAX_RATE;
            
            this.EventClicked = "OnDsTaxClicked";
            this.InitDataSource(pw, Repeater1, this.DATA, "dsTax");
            this.Register();
        }

        public void retrieve(String year)
        {
            int year_pay = Convert.ToInt16(year) - 543;
                string sql = @"     
                            SELECT HRNUCFTAX_RATE.MIN_RATE,   
                                             HRNUCFTAX_RATE.MAX_RATE,   
                                             HRNUCFTAX_RATE.TAXPER,   
                                             HRNUCFTAX_RATE.GOSS_RATE,
trim(to_char(HRNUCFTAX_RATE.MIN_RATE,'9,999,999,999,999,999,999,999,999,999'))||' - '||trim(to_char(HRNUCFTAX_RATE.MAX_RATE,'9,999,999,999,999,999,999,999,999,999')) as cp_minmaxrate  
                                        FROM HRNUCFTAX_RATE  
                                    where taxyear='" + year + @"'
                                    ORDER BY HRNUCFTAX_RATE.TAXPER ASC
    
 ";
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
            
        }
    }
}