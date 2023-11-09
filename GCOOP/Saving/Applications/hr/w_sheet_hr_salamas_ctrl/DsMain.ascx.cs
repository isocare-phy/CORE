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
    public partial class DsMain : DataSourceFormView
    {
        public DataSet1.HRNMLEMPLFILEMASDataTable DATA { get; private set; }

        public void InitDs(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.HRNMLEMPLFILEMAS;
            this.InitDataSource(pw, FormView1, this.DATA, "dsMain");
            this.EventItemChanged = "OnDsMainItemChanged";
            this.EventClicked = "OnDsMainClicked";
            this.Button.Add("taxcal");
            this.Button.Add("tran_dp");

            this.Register();
        }

        public void DdApplication()
        {

            string sql = @"
            select  EMPLCODE , prename_desc || EMPLFIRSNAME ||' ' || EMPLLASTNAME as EMPLFIRSNAME , 1 as sorter  from  hrnmlemplfilemas  left join mbucfprename  on hrnmlemplfilemas.prename_code = mbucfprename.prename_code where hrnmlemplfilemas.emplstat = '1'
             union 
            select '','', 0 from dual
            order by sorter, EMPLCODE";
            DataTable dt = WebUtil.Query(sql);


            this.DropDownDataBind(dt, "EMPLCODE", "EMPLFIRSNAME", "EMPLCODE");

        }
    }
}