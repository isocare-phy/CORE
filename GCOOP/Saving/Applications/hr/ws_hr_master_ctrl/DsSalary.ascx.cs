using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using DataLibrary;
using System.Data;

namespace Saving.Applications.hr.ws_hr_master_ctrl
{
    public partial class DsSalary : DataSourceFormView
    {
        public DataSet1.DT_SalaryDataTable DATA { get; set; }

        public void InitDsSalary(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.DT_Salary;
            this.EventItemChanged = "OnDsSalaryItemChanged";
            this.EventClicked = "OnDsSalaryClicked";
            this.InitDataSource(pw, FormView1, this.DATA, "dsSalary");
            this.TableName = "HREMPLOYEE";
            this.Register();
        }

        public void Retrieve(string as_empno)
        {
            string sql = @"select hremployee.coop_id,   
                 hremployee.emp_no,         
                 hremployee.salary_amt,   
                 hremployee.salexp_code,   
                 hremployee.salexp_bank,   
                 hremployee.salexp_branch,   
                 hremployee.salexp_accid,   
                 hremployee.tax_calcode,   
                 hremployee.tax_bfamt,   
                 hremployee.ss_status,   
                 hremployee.ss_appfirststs,   
                 hremployee.ss_bfamt,   
                 hremployee.ss_appdate,   
                 hremployee.ss_rate,   
                 hremployee.ss_hospital,   
                 hremployee.provf_status,   
                 hremployee.provf_corprate,   
                 hremployee.provf_emprate,   
                 hremployee.provf_appdate,   
                 hremployee.provf_resigndate,   
                 hremployee.provf_bfamt 
            from hremployee
            where hremployee.coop_id = {0}
            and hremployee.emp_no = {1}";

            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, as_empno);
            Sdt dt = WebUtil.QuerySdt(sql);
            this.ImportData(dt);
        }

        public void DdBank()
        {
            string sql = @"
              SELECT BANK_CODE,   
                     BANK_DESC,   
                     EDIT_FORMAT ,1 as sorter 
                FROM CMUCFBANK 
            union
            select '','','',0 from dual order by sorter,BANK_DESC ASC";
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "salexp_bank", "BANK_DESC", "BANK_CODE");
        }

        public void DdBranch(string bank_code)
        {
            string sql = @"
                  SELECT BANK_CODE,   
                         BRANCH_ID,   
                         BRANCH_NAME,   
                         1 as sorter
                    FROM CMUCFBANKBRANCH
                   where bank_code={0}
                    union 
                    select '','','',0 from dual order by sorter,  BRANCH_NAME ASC";
            sql = WebUtil.SQLFormat(sql, bank_code);
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "salexp_branch", "BRANCH_NAME", "BRANCH_ID");
        }
    }
}