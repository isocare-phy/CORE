using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DataLibrary;

namespace Saving.Applications.assist.ws_as_assisttranfer_ctrl
{
    public partial class DsList : DataSourceFormView
    {


        public DataSet1.ASSASSISTTRANFERDataTable DATA { get; set; }

        public void InitDsList(PageWeb pw)
        {
            css1.Visible = false;


            DataSet1 ds = new DataSet1();
            this.DATA = ds.ASSASSISTTRANFER;
            this.EventItemChanged = "OnDsListItemChanged";
            this.EventClicked = "OnDsListClicked";
            this.InitDataSource(pw, FormView1, this.DATA, "dsList");
            // this.Button.Add("Button3");
            this.Register();
        }


        public void DdBudgetTo(string coop_id, decimal assist_year)
        {
            String sql = @"select m.* from
                         ( select  assisttype_code,
                         assisttype_code||' - '|| assisttype_desc as assisttype_desc ,
                        1 as sorter
                        from asssumledgeryear
                        where  coop_id = {0}
                        and assist_year = {1}
                        union 
                        select   '99' as assisttype_code, 'รับจากภายนอก' as assisttype_desc,99 as sorter from dual
					    union 
                        select   '' as assisttype_code, 'กรุณาเลือกประเภทสวัสดิการ' as assisttype_desc,0 as sorter from dual) m order by m.sorter,m.assisttype_code";
            sql = WebUtil.SQLFormat(sql, state.SsCoopId, assist_year);
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "ASSISTTYPE_TO", "assisttype_desc", "assisttype_code");
            this.DropDownDataBind(dt, "ASSISTTYPE_FROM", "assisttype_desc", "assisttype_code");
        }


    }
}