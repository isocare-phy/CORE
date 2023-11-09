using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;


namespace Saving.Applications.shrlon.ws_sl_reprint_payout_ctrl
{
    public partial class DsList : DataSourceRepeater
    {
        public DataSet1.SLSLIPPAYOUTDataTable DATA { get; set; }


        public void InitDsList(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.SLSLIPPAYOUT;
            this.EventItemChanged = "OnDsListItemChanged";
            this.EventClicked = "OnDsListClicked";
            this.InitDataSource(pw, Repeater1, this.DATA, "dsList");
            this.Register();
        }
      

        public void Retrieve(string member_no, string entry_id, string sliptype_code, string document_no_s, string document_no_e, DateTime slip_date_s, DateTime slip_date_e)
        {
            string sqlext = "";
            if (member_no != "")
            {
                sqlext += " and slslippayout.member_no = '" + WebUtil.MemberNoFormat(member_no) + "'";
            }
            if (entry_id != "")
            {
                sqlext += " and slslippayout.entry_id = '" + entry_id + "'";
            }
            if (sliptype_code != "")
            {
                sqlext += " and slslippayout.sliptype_code = '" + sliptype_code + "'";
            }
            if (document_no_s != "" && document_no_e != "")
            {
                sqlext += " and (slslippayout.document_no between '" + document_no_s + "' and '" + document_no_e + "')";
            }
            if (slip_date_s.Year > 1900 && slip_date_e.Year > 1900)
            {
                sqlext += " and slslippayout.slip_date between {1} and {2} ";

            }
            string sql = @"  select MBUCFPRENAME.PRENAME_DESC,MBMEMBMASTER.MEMB_NAME,MBMEMBMASTER.MEMB_SURNAME,   
                            MBMEMBMASTER.MEMBGROUP_CODE, slslippayout.* 
                            from slslippayout,MBMEMBMASTER,MBUCFPRENAME
                            WHERE MBMEMBMASTER.PRENAME_CODE = MBUCFPRENAME.PRENAME_CODE and  
                            slslippayout.MEMBER_NO = MBMEMBMASTER.MEMBER_NO and  
                            MBMEMBMASTER.COOP_ID = slslippayout.MEMCOOP_ID  and   
                            rownum <= 100 and     
                            slslippayout.entry_bycoopid = {3} and  
                            slslippayout.COOP_ID = {0} " + sqlext + "order by " + "slslippayout.payoutslip_no desc ";
            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, slip_date_s, slip_date_e, state.SsCoopId);
            DataTable dt = WebUtil.Query(sql);
            ImportData(dt);
        }
    }
}