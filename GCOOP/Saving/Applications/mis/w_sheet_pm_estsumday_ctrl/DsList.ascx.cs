using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace Saving.Applications.mis.w_sheet_pm_estsumday_ctrl
{
    public partial class DsList : DataSourceRepeater 
    {
        public DataSet1.PMINVESTMASTERDataTable DATA { get; set; }  //**เปลี่ยนเป็นตารางของเรา อย่าเอาที่ก็อปมานะครับ **
        public void InitDsList(PageWeb pw)
        {
            css1.Visible = false;
            css2.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.PMINVESTMASTER; //**เปลี่ยนเป็นตารางของเรา อย่าเอาที่ก็อปมานะครับ **
            this.Button.Add("b_del");
            this.EventClicked = "OnDsListClicked";
            this.InitDataSource(pw, Repeater1, this.DATA, "dsList");
            this.Register();
        }
        public void retrieve(string val)
        {
            string sql = @"select  p2.account_no,
                     p1.investtype_desc , 
                    p2.symbol_code,
                     p2.prncbal ,  
                    p2.lastcalint_date 

                    from pmucfinvest_type p1 , pminvestmaster p2, pmucfinvsource p4, pminvestintrate p3
                    where p1.investtype_code = p2.investtype_code
                    and p2.account_no = p3.account_no
                    and p4.invsource_code = p2.invsource_code
                    and p2.start_intdate between p3.int_start_date and p3.int_end_date
                    and p2.close_status  = 0 "; //**เปลี่ยนเป็นตารางของเรา อย่าเอาที่ก็อปมานะครับ **
            if (val != "")
            {
                sql += "and p1.investtype_code = '" + val +"' ";
            }

            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }
    }
}