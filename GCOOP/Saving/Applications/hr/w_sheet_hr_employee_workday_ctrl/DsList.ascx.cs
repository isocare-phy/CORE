using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace Saving.Applications.hr.w_sheet_hr_employee_workday_ctrl
{
    public partial class DsList : DataSourceRepeater
    {
        public DataSet1.HRNMLEMPLWORKDAYDEADataTable DATA { get; set; }

        public void InitDsList(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.HRNMLEMPLWORKDAYDEA;
           
            this.EventClicked = "OnDsListClicked";
            this.EventItemChanged = "OnDsListItemChanged";
            this.InitDataSource(pw, Repeater1, this.DATA, "dsList");
            this.Register();
        }

        public void retrieve(string work_dd,string work_mm,string work_yyyy)
        {
            string sql = @"  select hrnmlemplfilemas.emplid,HRNMLEMPLWORKDAYDEA.seq_no,emplcode,prename_desc,emplfirsname,empllastname,check_in from 
                            hrnmlemplfilemas left join HRNMLEMPLWORKDAYDEA on hrnmlemplfilemas.emplid =  HRNMLEMPLWORKDAYDEA.emplid left join  mbucfprename on
                                  hrnmlemplfilemas.prename_code = mbucfprename.prename_code where workdate = to_date('" + work_dd + "/" + work_mm + "/" + work_yyyy +@"' ,'dd/mm/yyyy')  order by emplcode
 ";
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }
    }
}