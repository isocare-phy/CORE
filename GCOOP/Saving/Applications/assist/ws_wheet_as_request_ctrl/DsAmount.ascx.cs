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
    public partial class DsAmount : DataSourceFormView
    {
        public DataSet1.ASSREQMASTERDataTable DATA { get; set; }
        public void InitDsAmount(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.ASSREQMASTER;
            this.EventItemChanged = "OnDsAmountItemChanged";
            this.EventClicked = "OnDsAmountClicked";
            this.InitDataSource(pw, FormView1, this.DATA, "dsAmount");
            this.Register();
        }

        public void Retrieve(String memno, String ls_assiscode)
        {
            string sql = @"
                    select * from
                    (
	                    select 
		                    moneytype_code, 
		                    expense_bank, 
		                    expense_branch, 
		                    expense_accid, 
		                    deptaccount_no, 
                            0,
		                    '',
                            (select sum(ln.principal_balance) from lncontmaster ln where ln.member_no =  {1} and payment_status=1) as principal_balance, 
		                    '0' sort 
	                    from assreqmaster where req_status = 8 and assisttype_code = {0} and member_no = {1}
	                    UNION
	                    select 
		                    expense_code moneytype_code, 
		                    expense_bank, 
		                    expense_branch, 
		                    case expense_code when 'TRN' then '' else expense_accid end expense_accid, 
		                    case expense_code when 'TRN' then expense_accid else '' end deptaccount_no, 
                            0,
		                    '',
                            (select sum(ln.principal_balance) from lncontmaster ln where ln.member_no =  {1} and payment_status=1) as principal_balance, 
		                    '1' sort 
	                    from mbmembmaster where member_no = {1}
                    )order by sort ";
            sql = WebUtil.SQLFormat(sql, ls_assiscode, memno);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }

        public void Retrieve2(String memno, String assist_docno)
        {
            string sql = @"
                    select * from
                    (
	                    select 
		                    moneytype_code, 
		                    expense_bank, 
		                    expense_branch, 
		                    expense_accid, 
		                    deptaccount_no, 
                            approve_amt,
		                    remark,
                            principal_balance, 
		                    '0' sort 
	                    from assreqmaster where req_status = 8 and assist_docno = {0} and member_no = {1}
	                    UNION
	                    select 
		                    expense_code moneytype_code, 
		                    expense_bank, 
		                    expense_branch, 
		                    case expense_code when 'TRN' then '' else expense_accid end expense_accid, 
		                    case expense_code when 'TRN' then expense_accid else '' end deptaccount_no, 
                            0,
		                    '',
                            (select sum(ln.principal_balance) from lncontmaster ln where ln.member_no =  {1} and payment_status=1) as principal_balance, 
		                    '1' sort 
	                    from mbmembmaster where member_no = {1}
                    )order by sort ";
            sql = WebUtil.SQLFormat(sql, assist_docno, memno);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }

        public void Retrieve3(String memno, String assist_docno, Decimal sys_amt)
        {
            string sql = @"
                    select * from
                    (
	                    select 
		                    moneytype_code, 
		                    expense_bank, 
		                    expense_branch, 
		                    expense_accid, 
		                    deptaccount_no, 
                            approve_amt,
		                    remark,
                            principal_balance,
                            {2} as system_amt, 
		                    '0' sort 
	                    from assreqmaster where req_status = 8 and assist_docno = {0} and member_no = {1}
	                    UNION
	                    select 
		                    expense_code moneytype_code, 
		                    expense_bank, 
		                    expense_branch, 
		                    case expense_code when 'TRN' then '' else expense_accid end expense_accid, 
		                    case expense_code when 'TRN' then expense_accid else '' end deptaccount_no, 
                            0,
		                    '',
                            (select sum(ln.principal_balance) from lncontmaster ln where ln.member_no =  {1} and payment_status=1) as principal_balance,
                            0, 
		                    '1' sort 
	                    from mbmembmaster where member_no = {1}
                    )order by sort ";
            sql = WebUtil.SQLFormat(sql, assist_docno, memno, sys_amt);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }

        public void RetrieveBank()
        {
            string sql = @"select bank_code,   
                bank_desc,   
                bank_code||'-'||bank_desc as display  ,1 as sorter
                from cmucfbank
            union
            select '','','เลือกธนาคาร',0 from dual order by sorter,bank_code asc ";
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "expense_bank", "display", "bank_code");
        }
        public void RetrieveBranch(String bank)
        {
            string sql = @"select branch_id,   
                        branch_name,   
                        branch_id||'-'||branch_name as display  
                        from cmucfbankbranch where bank_code={0}
                        order by branch_id asc ";
            sql = WebUtil.SQLFormat(sql, bank.Trim());
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "expense_branch", "display", "branch_id");
        }
       
        public void RetrieveMoneyType()
        {
            string sql = @" 
                   SELECT MONEYTYPE_CODE,   
                          MONEYTYPE_DESC,   
                          SORT_ORDER  ,
                          MONEYTYPE_CODE || ' - '|| MONEYTYPE_DESC as MONEYTYPE_DISPLAY
                     FROM CMUCFMONEYTYPE where MONEYTYPE_CODE in ('CSH','CHQ','CBT','TRN') order by sort_order
            ";
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "moneytype_code", "MONEYTYPE_DISPLAY", "MONEYTYPE_CODE");
        }

        public void RetrieveDeptaccount(string memno)
        {
            string sql = @" select deptaccount_no, deptaccount_desc from
                        (
	                        select deptaccount_no, deptaccount_no deptaccount_desc, depttype_code from dpdeptmaster where deptclose_status = 0 and coop_id = {0} and member_no = {1}
	                        union
	                        select '00', 'กรุณาเลือกบัญชี', '00' depttype_code from dpdeptmaster where rownum = 1
                        )
                        order by depttype_code, deptaccount_no ";
            sql = WebUtil.SQLFormat(sql, state.SsCoopId, memno);
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "deptaccount_no", "deptaccount_desc", "deptaccount_no");
        }
    }
}