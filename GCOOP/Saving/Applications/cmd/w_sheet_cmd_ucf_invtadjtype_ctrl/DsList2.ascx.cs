using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace Saving.Applications.cmd.w_sheet_cmd_ucf_invtadjtype_ctrl
{
    public partial class DsList2 : DataSourceRepeater
    {
        public DataSet1.ptucfinvtadjtypeDataTable DATA { get; set; }

        public void InitDsList2(PageWeb pw)
        {
            css1.Visible = false;

            DataSet1 ds = new DataSet1();
            this.DATA = ds.ptucfinvtadjtype;
            this.InitDataSource(pw, Repeater1, this.DATA, "dsList2");
            this.EventClicked = "OnDsList2Clicked";
            this.EventItemChanged = "OnItemDsList2Changed";
            this.Button.Add("b_del");
            this.Button.Add("b_edit");
            this.Register();
        }

        public void RetrieveList2()
        {
            String sql = @"select 
adjtype_code,
adjtype_desc,
case  when  sign_flag=-1 then 'ออก'  else 'ไม่ระบุ'end as sign_flag 
from
(
select adjtype_code,
adjtype_desc,
sign_flag
from ptucfinvtadjtype
where sign_flag='-1'
order by adjtype_code asc
)";
            sql = WebUtil.SQLFormat(sql);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }

    }
}