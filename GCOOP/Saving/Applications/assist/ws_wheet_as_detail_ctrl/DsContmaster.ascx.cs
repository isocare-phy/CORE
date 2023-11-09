using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using System.Data;

namespace Saving.Applications.assist.ws_wheet_as_detail_ctrl
{
    public partial class DsContmaster : DataSourceFormView
    {
        public DataSet1.DataDsContDataTable DATA { get; set; }
        public void InitDsContmaster(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.DataDsCont;
            this.EventItemChanged = "OnDsContItemChanged";
            this.EventClicked = "OnDsContClicked";
            this.InitDataSource(pw, FormView2, this.DATA, "dsContmaster");
            this.Register();
        }

        public void RetrieveDetail(string ls_memno, string ls_assisttype)
        {
            string sql = @"
                         SELECT assisttype_code,asscontract_no,approve_amt,withdrawable_amt,pay_balance,payment_status from ASSCONTMASTER a
            where a.coop_id={0} and a.member_no={1} and a.assisttype_code={2}
            AND asscontract_no=(select max(asscontract_no) from ASSCONTMASTER where ASSCONTMASTER.member_no= a.member_no and ASSCONTMASTER.assisttype_code={2})
                         ";
            sql = WebUtil.SQLFormat(sql, state.SsCoopId, ls_memno,ls_assisttype);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }
        public void RetrieveDetailFromDDW(string ls_memno, string ls_assisttype,string ls_contractno)
        {
            string sql = @"
                         SELECT assisttype_code,asscontract_no,approve_amt,withdrawable_amt,pay_balance,payment_status from ASSCONTMASTER 
                        where coop_id={0} and member_no ={1} and assisttype_code={2} and asscontract_no={3}
                         ";
            sql = WebUtil.SQLFormat(sql, state.SsCoopId, ls_memno, ls_assisttype, ls_contractno);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }
        public void DDW_Contractno(string ls_memno, string ls_assisttype)
        {
            string sql = @" 
                    SELECT asscontract_no from ASSCONTMASTER 
                    where coop_id={0} and member_no ={1} and assisttype_code={2}  
                    order by asscontract_no desc
            ";
            sql = WebUtil.SQLFormat(sql, state.SsCoopId, ls_memno, ls_assisttype);
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "asscontract_no", "asscontract_no", "asscontract_no");
        }      
    }
}