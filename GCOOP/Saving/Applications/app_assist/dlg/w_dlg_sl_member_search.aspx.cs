using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using CoreSavingLibrary.WcfCommon;

namespace Saving.Applications.app_assist
{
    public partial class w_dlg_sl_member_search : PageWebDialog, WebDialog
    {
        WebState state;
        protected String LoanContractSearch;
        DwTrans SQLCA;
        String ls_sql = "", ls_sqlext = "", ls_temp = "";

        protected void cb_find_Click(object sender, EventArgs e)
        {

            String ls_memno = "", ls_memname = "", ls_memsurname = "";
            String ls_memgroup = "", ls_contno = "", ls_loancontract = "";
            String ls_cardperson = "", ls_subgroup = "";

            string coop_id = "";
            try
            {
                coop_id = dw_data.GetItemString(1, "coop_select");
            }
            catch
            { Exception ex; }
            if (coop_id == "")
            {
                coop_id = state.SsCoopControl;
            }

            string ls_sql1 = "and mbmembmaster.coop_id ='" + coop_id + "'";


            try
            {
                ls_memno = dw_data.GetItemString(1, "member_no").Trim();

            }
            catch { ls_memno = ""; }

            try
            {
                ls_memname = dw_data.GetItemString(1, "memb_name").Trim();

            }
            catch { ls_memname = ""; }
            try
            {
                ls_memsurname = dw_data.GetItemString(1, "memb_surname").Trim();

            }
            catch { ls_memsurname = ""; }
            try
            {
                ls_memgroup = dw_data.GetItemString(1, "membgroup_no").Trim();

            }
            catch { ls_memgroup = ""; }

            try
            {
                ls_contno = dw_data.GetItemString(1, "loancontract_no").Trim();

            }
            catch { ls_contno = ""; }

            if (ls_memno.Length > 0)
            {
                ls_sqlext = " and (  mbmembmaster.member_no like '%" + ls_memno + "%') ";
            }

            if (ls_memname.Length > 0)
            {
                ls_sqlext += " and ( mbmembmaster.memb_name like '%" + ls_memname + "%') ";
            }
            if (ls_memsurname.Length > 0)
            {
                ls_sqlext += " and ( mbmembmaster.memb_surname like '%" + ls_memsurname + "%') ";
            }
            if (ls_memgroup.Length > 0)
            {
                ls_sqlext += " and ( mbmembmaster.membgroup_code = '%" + ls_memgroup + "') ";
            }

            if (ls_contno.Length > 0)
            {
                //ls_sqlext += " and ( lncontmaster.loancontract_no like '" + ls_contno + "%') ";
                ls_sqlext += " and ( mbmembmaster.member_no in( select lncontmaster.member_no from lncontmaster where lncontmaster.loancontract_no like '%" + ls_contno + "%')) ";
            }

            ls_temp = ls_sql + ls_sql1 + ls_sqlext;
            hidden_search.Value = ls_temp;
            dw_detail.SetSqlSelect(hidden_search.Value);
            dw_detail.Retrieve();
            DwUtil.RetrieveDDDW(dw_data, "coop_select", "kp_recieve_return.pbl", state.SsCoopControl);
        }

        public void InitJsPostBack()
        {

            LoanContractSearch = WebUtil.JsPostBack(this, "LoanContractSearch");
        }

        public void WebDialogLoadBegin()
        {
            state = new WebState(Session, Request);
            SQLCA = new DwTrans();
            SQLCA.Connect();
            dw_data.SetTransaction(SQLCA);
            dw_detail.SetTransaction(SQLCA);
            ls_sql = dw_detail.GetSqlSelect();

            if (!IsPostBack)
            {
                dw_data.InsertRow(0);

                DwUtil.RetrieveDDDW(dw_data, "coop_select", "as_public_funds.pbl", state.SsCoopControl);
                string coop_id = state.SsCoopControl;
                try
                {
                    coop_id = dw_data.GetItemString(1, "coop_select");
                }
                catch (Exception ex)
                { }
                DwUtil.RetrieveDDDW(dw_data, "membgroup_no_1", "as_public_funds.pbl", coop_id);
            }
            else
            {
                dw_data.RestoreContext();
                dw_detail.RestoreContext();
            }
            if (!hidden_search.Value.Equals(""))
            {
                dw_detail.SetSqlSelect(hidden_search.Value);
                dw_detail.Retrieve();
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "LoanContractSearch")
            {
                JsLoanContractSearch();
            }

        }

        private void JsLoanContractSearch()
        {
            String ls_memno = "", ls_memname = "", ls_memsurname = "";
            String ls_memgroup = "", ls_contno = "", ls_loancontract = "";
            String ls_cardperson = "", ls_subgroup = "";

            ls_sql = dw_detail.GetSqlSelect();
            string coop_id = "";
            try { coop_id = dw_data.GetItemString(1, "coop_select"); }
            catch (Exception ex) { ex.ToString(); }
            if (coop_id == "") { coop_id = state.SsCoopControl; }

            string ls_sql1 = "and mbmembmaster.coop_id ='" + coop_id + "'";

            try { ls_memno = hmember_no.Value; }
            catch { ls_memno = ""; }

            try { ls_memname = dw_data.GetItemString(1, "memb_name").Trim(); }
            catch { ls_memname = ""; }

            try { ls_memsurname = dw_data.GetItemString(1, "memb_surname").Trim(); }
            catch { ls_memsurname = ""; }

            try { ls_memgroup = dw_data.GetItemString(1, "membgroup_no").Trim(); }
            catch { ls_memgroup = ""; }

            try { ls_contno = dw_data.GetItemString(1, "loancontract_no").Trim(); }
            catch { ls_contno = ""; }

            if (ls_memno.Length > 0)
            {
                ls_sqlext = " and (  mbmembmaster.member_no like '" + ls_memno + "%') ";
            }

            if (ls_memname.Length > 0)
            {
                ls_sqlext += " and ( mbmembmaster.memb_name like '" + ls_memname + "%') ";
            }

            if (ls_memsurname.Length > 0)
            {
                ls_sqlext += " and ( mbmembmaster.memb_surname like '" + ls_memsurname + "%') ";
            }

            if (ls_memgroup.Length > 0)
            {
                ls_sqlext += " and ( mbmembmaster.membgroup_code = '" + ls_memgroup + "') ";
            }
            
            if (ls_contno.Length > 0)
            {
                ls_sqlext += " and ( mbmembmaster.member_no in( select lncontmaster.member_no from lncontmaster where lncontmaster.loancontract_no like '" + ls_contno + "%')) ";
            }

            ls_temp = ls_sql + ls_sql1 + ls_sqlext;
            hidden_search.Value = ls_temp;
            dw_detail.SetSqlSelect(hidden_search.Value);
            
            dw_detail.Retrieve();
            dw_data.SetItemString(1, "member_no", "");
            DwUtil.RetrieveDDDW(dw_data, "coop_select", "kp_recieve_return.pbl", state.SsCoopControl);
        }

        public void WebDialogLoadEnd()
        {
            SQLCA.Disconnect();
        }
    }
}
