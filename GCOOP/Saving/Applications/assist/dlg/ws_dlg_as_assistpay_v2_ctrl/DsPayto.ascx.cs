using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using System.Data;

namespace Saving.Applications.assist.dlg.ws_dlg_as_assistpay_v2_ctrl
{
    public partial class DsPayto : DataSourceRepeater
    {
        public DataSet1.DsPaytoDataTable DATA { get; set; }
        public void InitDsPayto(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.DsPayto;
            this.EventItemChanged = "OnDsPaytoItemChanged";
            this.EventClicked = "OnDsPaytoClicked";
            this.InitDataSource(pw, Repeater1, this.DATA, "dsPayto");
            this.Button.Add("b_searchbank");
            this.Button.Add("b_delpayto");
            this.Register();
        }
        //eq.expense_bank,req.expense_branch,trim(req.expense _accid),pre.prename_desc,gain.gain_name,gain.gain_surname
        public void RetrieveDetail(String ls_assistdocno)
        {
            string sql = @"
	                select 
		                distinct
		                req.moneytype_code,req.expense_bank,req.expense_branch,req.expense_accid,gain.gain_name,gain.gain_surname,pre.prename_desc,
                        
		                case when gain.gain_name is not null then 
			                case moneytype.moneytype_group
			                when 'CSH' then ' | | |' || pre.prename_desc || mb.memb_name || ' ' || mb.memb_surname || '| ' 
			                when 'CHQ' then req.expense_bank || '|' || req.expense_branch || '|' || trim(req.expense_accid) || '|' || pre.prename_desc || gain.gain_name || '  ' || gain.gain_surname || '| '
			                when 'BNK' then req.expense_bank || '|' || req.expense_branch || '|' || trim(req.expense_accid) || '|' || pre.prename_desc || gain.gain_name || '  ' || gain.gain_surname || '| '
			                when 'TRN' then ' | |' || trim(req.deptaccount_no) || '|' || pre.prename_desc || gain.gain_name || '  ' || gain.gain_surname || '| '
			                else '' end 
		                else
			                case moneytype.moneytype_group 
                            when 'CSH' then ' | | |' || pre.prename_desc || mb.memb_name || ' ' || mb.memb_surname || '| ' 
			                when 'CHQ' then req.expense_bank || '|' || req.expense_branch || '|' || trim(req.expense_accid)|| '| | '
			                when 'BNK' then req.expense_bank || '|' || req.expense_branch || '|' || trim(req.expense_accid) || '| | '
			                when 'TRN' then ' | |' || trim(req.deptaccount_no) || '| | '
			                else '' end 
		                end payto_detail,
		                case when gain.gain_name is null then req.assist_amt else 0 end assist_amt
	                from assreqmaster req
	                left join assreqgain gain on req.assist_docno = gain.assist_docno
	                inner join mbmembmaster mb on req.member_no = mb.member_no
	                inner join mbucfprename pre on mb.prename_code = pre.prename_code
	                left join mbucfprename pregain on gain.gainprename_code = pregain.prename_code
	                left join cmucfbank bank on req.expense_bank = bank.bank_code
	                left join cmucfbankbranch branch on req.expense_bank = branch.bank_code and req.expense_branch = branch.branch_id
				left join cmucfmoneytype moneytype on moneytype.moneytype_code=req.moneytype_code
	                where req.coop_control = {0} and req.assist_docno = {1}";
            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, ls_assistdocno);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }

        public void DdMoneyType()
        {
            string sql = @"select * from
                    (
	                    SELECT 
		                    MONEYTYPE_CODE,  
		                    MONEYTYPE_GROUP, 
		                    MONEYTYPE_DESC,   
		                    SORT_ORDER  ,
		                    MONEYTYPE_CODE || ' - '|| MONEYTYPE_DESC as MONEYTYPE_DISPLAY
	                    FROM CMUCFMONEYTYPE WHERE   MONEYTYPE_GROUP in ('CHQ','BNK','CSH','TRN') and moneytype_STATUS='DAY'
	              
                    )
                    order by sort_order
            ";
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "moneytype_code", "MONEYTYPE_DISPLAY", "MONEYTYPE_CODE");
        }


    }
}