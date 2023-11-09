using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using System.Data;
namespace Saving.Applications.assist.ws_wheet_as_request_ctrl
{
    public partial class DsMain : DataSourceFormView
    {
        public DataSet1.DataMainDataTable DATA { get; set; }
        public void InitDsMain(PageWeb pw)
        {
            css1.Visible = false;           
            DataSet1 ds = new DataSet1();
            this.DATA = ds.DataMain;
            this.EventItemChanged = "OnDsMainItemChanged";
            this.EventClicked = "OnDsMainClicked";
            this.Button.Add("b_search");
            this.InitDataSource(pw, FormView1, this.DATA, "dsMain");
            this.Register();
        }

        public void Retrieve(String memno)
        {
            string sql = @"select 
		                        m.member_no,
		                        FT_GETMEMNAME(m.coop_id,m.member_no) as namesurname,
		                        m.membgroup_code || ' : ' || mbgroup.membgroup_desc as membgroug,
		                        t.membtype_code || ' ' || t.membtype_desc membtype_desc,
		                        FT_CALAGE(m.birth_date,sysdate,4) age,
		                        FT_CALAGE(m.member_date,sysdate,4) as age_member,
                                m.salary_amount,
		                        m.card_person,
		                        m.member_date date_mamber,
		                        m.birth_date as birth_date,
		                        m.retry_date as retry_date
                         from mbmembmaster  m
                         inner join mbucfmembtype t on t.membtype_code = m.membtype_code
                        inner join mbucfmembgroup mbgroup on m.membgroup_code = mbgroup.membgroup_code
		                where m.member_no={0} ";
            sql = WebUtil.SQLFormat(sql, memno,state.SsWorkDate);
            DataTable dt = WebUtil.Query(sql );           
            this.ImportData(dt);
        }
    }
}