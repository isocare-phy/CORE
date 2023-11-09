using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using System.Globalization;
using DataLibrary;
using System.Data;

namespace Saving.Applications.hr.ws_hr_payroll_depttran_ctrl
{
    public partial class ws_hr_payroll_depttran : PageWebSheet, WebSheet
    {
        [JsPostBack]
        public String Post { get; set; }
        [JsPostBack]
        public String PostCheck { get; set; }

        private CultureInfo th;

        public void InitJsPostBack()
        {
            
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                //set งวด
                this.th = new CultureInfo("th-TH");
                DateTime dt_contadjust_date = state.SsWorkDate;
                year.Text = Convert.ToString(WebUtil.GetAccyear(state.SsCoopControl, state.SsWorkDate));
                month.Text = DateTime.Now.Month.ToString();
                DdEmptype();
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "PostCheck")
            {
                try
                {
                    string ls_emptype = emptype_code.SelectedValue;
                    string ls_period = year.Text + Convert.ToDecimal(month.Text).ToString("00");
                    string ls_sql = @"select sum(pr.salarynet_amt) as tran_amt 
                        from hrpayroll pr, hremployee em
                        where pr.coop_id = em.coop_id
                        and pr.emp_no = em.emp_no
                        and pr.coop_id = {0}
                        and pr.payroll_period = {1}
                        and em.emptype_code = {2}
                        and pr.expense_code = 'TRN'
                        and pr.expense_accid is not null
                        and pr.payroll_status = 1
                        and pr.post_status = 0
                        and pr.salarynet_amt > 0";
                    ls_sql = WebUtil.SQLFormat(ls_sql, state.SsCoopControl, ls_period, ls_emptype);
                    Sdt dt = WebUtil.QuerySdt(ls_sql);

                    if (dt.Next())
                    {
                        tran_amt.Text = dt.GetDecimal("tran_amt").ToString("#,##0.00");
                    }
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex); return;
                }
            }
            else if (eventArg == "Post")
            {
                ExecuteDataSource exe = new ExecuteDataSource(this);
                string ls_period = "", ls_sql = "", ls_sqlinsert = "", ls_sqlupdate = "";
                string ls_expenseaccid = "", ls_refmembno = "", ls_payrollslip = "";
                DateTime ldtm_payrolldate;
                Decimal ldc_salnet = 0;
                string ls_emptype = emptype_code.SelectedValue;
                try
                {
                    ls_period = year.Text + Convert.ToDecimal(month.Text).ToString("00");

                    ls_sql = @"select trim(hpr.expense_accid) as expense_accid,
                               hrm.ref_membno as member_no, hpr.payroll_date,
                               hpr.salarynet_amt,
                               hpr.payrollslip_no
                       from hrpayroll hpr,
                       hremployee hrm, dpdeptmaster dp
                       where hpr.coop_id = hrm.coop_id
                       and hpr.emp_no = hrm.emp_no
                       and hpr.expense_accid = dp.deptaccount_no(+)
                       and hpr.coop_id = '" + state.SsCoopControl + @"' 
                       and hpr.payroll_period = '" + ls_period + @"' 
                       and hrm.emptype_code = '" + ls_emptype + @"'
                       and hpr.expense_code = 'TRN'
                       and hpr.expense_accid is not null
                       and hpr.post_status = 0
                       and hpr.salarynet_amt > 0";
                    Sdt dt = WebUtil.QuerySdt(ls_sql);

                    if (dt.Next())
                    {
                        for (int i = 0; i < dt.GetRowCount(); i++)
                        {
                            ls_expenseaccid = Convert.ToString(dt.Rows[i]["expense_accid"]);
                            ls_refmembno = Convert.ToString(dt.Rows[i]["member_no"]);
                            ls_payrollslip = Convert.ToString(dt.Rows[i]["payrollslip_no"]);
                            ldtm_payrolldate = Convert.ToDateTime(dt.Rows[i]["payroll_date"]);
                            ldc_salnet = Convert.ToDecimal(dt.Rows[i]["salarynet_amt"]);

                            //โอนเข้า depttran
                            ls_sqlinsert = @"insert into dpdepttran
                            (   coop_id , 
                                deptaccount_no , 
                                memcoop_id , 
                                member_no , 
                                system_code , 
                                tran_year , 
                                tran_date , 
                                seq_no , 
                                deptitem_amt , 
                                tran_status , 
                                branch_operate , 
                                ref_slipno , 
                                ref_coopid )
                            values( {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11},{12})";
                            object[] argsInsert = new object[] { 
                                state.SsCoopControl,
                                ls_expenseaccid,
                                state.SsCoopId,
                                ls_refmembno,
                                "DHR",
                                Convert.ToInt32(year.Text),
                                ldtm_payrolldate,
                                1,
                                ldc_salnet,
                                0,
                                "001",
                                ls_payrollslip,
                                state.SsCoopControl
                            };
                            ls_sqlinsert = WebUtil.SQLFormat(ls_sqlinsert, argsInsert);
                            exe.SQL.Add(ls_sqlinsert);

                            //update post_status
                            ls_sqlupdate = @"update hrpayroll set post_status = 1 
                                where payrollslip_no = {0}";
                            ls_sqlupdate = WebUtil.SQLFormat(ls_sqlupdate, ls_payrollslip);
                            exe.SQL.Add(ls_sqlupdate);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex); return;
                }

                try
                {
                    exe.Execute();
                    exe.SQL.Clear();
                    LtServerMessage.Text = WebUtil.CompleteMessage("ผ่านรายการสำเร็จ");
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                }
            }
        }

        public void DdEmptype()
        {
            string ls_sql = @"select emptype_code,
                emptype_desc
                from hrucfemptype
                order by emptype_code";
            DataTable dt = new DataTable();
            dt = WebUtil.Query(ls_sql);
            emptype_code.DataTextField = "emptype_desc";
            emptype_code.DataValueField = "emptype_code";
            emptype_code.DataSource = dt;
            emptype_code.DataBind();
            emptype_code.SelectedIndex = 0;
        }

        public void SaveWebSheet()
        {

        }

        public void WebSheetLoadEnd()
        {

        }
    }
}