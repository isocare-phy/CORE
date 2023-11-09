using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using System.Data;

namespace Saving.Applications.assist.dlg.wd_as_expense_detail_ctrl
{
    public partial class DsList : DataSourceFormView
    {
        public DataSet1.DsListDataTable DATA { get; set; }

        public void InitDsList(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.DsList;
            this.InitDataSource(pw, FormView1, this.DATA, "dsList");
            this.EventItemChanged = "OnDsListItemChanged";
            this.EventClicked = "OnDsListClicked";
            this.Register();
        }
        public void DdBankDesc()
        {
            string sql = @"
            select  bank_code,trim(bank_code)|| '-'||trim(bank_desc)  as bank_desc, 1 as sorter from cmucfbank 
            union 
            select '','---กรุณาเลือกธนาคาร---', 0 from dual order by sorter, bank_desc
            ";
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "bank_code", "bank_desc", "bank_code");
        }

        public void DdBranch(string bank_code)
        {
            string sql = @"
            select bank_code,branch_id, branch_name, 1 as sorter from cmucfbankbranch where bank_code = {0} 
            union
            select '', '---กรุณาเลือกสาขา---','', 0 from dual order by sorter, branch_id
            ";
            sql = WebUtil.SQLFormat(sql, bank_code);
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "branch_code", "branch_name", "branch_id");
        }

        public void DdFromAccId()
        {
            string sql = @"SELECT  
                            ACCMASTER.ACCOUNT_ID,    
                            ACCMASTER.ACCOUNT_ID ||'-'||ACCMASTER.ACCOUNT_NAME AS fromacc_display,
                            ACCMASTER.ACCOUNT_NAME  ,  
                            1 as sorter
                        FROM ACCMASTER  WHERE ACCOUNT_TYPE_ID = '3'
                        union
                        select '', '---กรุณาเลือกรหัสบัญชี---','', 0 from dual order by sorter, ACCOUNT_ID
                ";
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "tofrom_accid", "fromacc_display", "ACCOUNT_ID");
        }
    }
}