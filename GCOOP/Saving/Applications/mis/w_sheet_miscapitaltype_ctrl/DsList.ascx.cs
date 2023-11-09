using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataLibrary;
using System.Data;

namespace Saving.Applications.mis.w_sheet_miscapitaltype_ctrl
{
    public partial class DsList : DataSourceRepeater
    {
        public DataSet1.MISCAPITALTYPEDataTable DATA { get; set; }

        public void InitDsList(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.MISCAPITALTYPE;
            this.EventItemChanged = "OnDsListItemChange";
            this.EventClicked = "OnDsListClicked";
            this.Button.Add("b_del");
            this.InitDataSource(pw, Repeater1, this.DATA, "dsList");
            this.Register();

        }
        public void retrieve()
        {
            string sql = @" select b.CAPTYPE_CODE,b.SYSTEM_CODE, b.coop_id ,decode(b.captype_code,'01','จัดหาทุน','02','ลงทุน') as CAPTYPE_CODE_DESC,
                    b.system_code,
                    b.description ,
                    b.seq_no
                from MISCAPITALTYPE  b
                  where  b.coop_id like '%'  
                order by b.seq_no";
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }

        public void retrieveddp()
        {
            string sql = @"select b.captype_code , decode(b.captype_code,'01','จัดหาทุน','02','ลงทุน') as CAPTYPE_CODE_DESC, 1 as sorter
                    from MISCAPITALTYPE  b
                  where  b.coop_id like '%'  
 union
            select '','', 0 from dual order by sorter, captype_code

";
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "CAPTYPE_CODE", "CAPTYPE_CODE_DESC", "CAPTYPE_CODE");
        }
    }
}