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

namespace Saving.Applications.hr.w_hr_employee_to_acc_ctrl
{
    public partial class w_hr_employee_to_acc : PageWebSheet, WebSheet
    {
        [JsPostBack]
        public String Post_Salary { get; set; }

        [JsPostBack]
        public String Post_Update { get; set; }


        private CultureInfo th;

        public void InitJsPostBack()
        {

        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {

            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            ExecuteDataSource exe = new ExecuteDataSource(this);

            if (eventArg == "Post_Salary") // ยิงข้อมูลเงินเดือนเเยกปี ยิงไป talbe ของบัญชี
            {

                string ref_membno = ""; decimal salary_amt = 0; string year_insert = "";
                string coop_id = state.SsCoopId;

                year_insert = Convert.ToString(Convert.ToDecimal(year.Text) - 543);

                string sql = @"select trim(ref_membno) as ref_membno ,salary_amt from hremployee where ref_membno is not null";
                sql = WebUtil.SQLFormat(sql);//format ในรูปของ sql
                Sdt dt = WebUtil.QuerySdt(sql);
                dt = WebUtil.QuerySdt(sql);
                if (dt.Next())
                {
                    for (int r = 0; r < dt.GetRowCount(); r++)
                    {

                        ref_membno = dt.Rows[r]["ref_membno"].ToString();
                        salary_amt = int.Parse(dt.Rows[r]["salary_amt"].ToString());

                        string sql_check = @"select salary_amt from accempsalary where account_year = '" + year_insert + "' and member_no = '" + ref_membno + "'";
                        sql_check = WebUtil.SQLFormat(sql_check);
                        Sdt dt_check = WebUtil.QuerySdt(sql_check);
                        dt_check = WebUtil.QuerySdt(sql_check);
                        if (dt_check.Next())
                        {

                            string sql_salary = @"update accempsalary set salary_amt = {0} where trim(member_no)={1} and account_year = {2}";
                            sql_salary = WebUtil.SQLFormat(sql_salary, salary_amt, ref_membno, year_insert);
                            Sdt dt_salary = WebUtil.QuerySdt(sql_salary);

                        }

                        else
                        {


                            string ls_sql = @"insert into accempsalary(coop_id,member_no,account_year,salary_amt,salary_flag)
                                          values({0},{1},{2},{3},{4})";
                            ls_sql = WebUtil.SQLFormat(ls_sql, coop_id, ref_membno, year_insert, salary_amt, 0);
                            exe.SQL.Add(ls_sql);
                            exe.Execute();
                            exe.SQL.Clear();

                        }

                        LtServerMessage.Text = WebUtil.CompleteMessage("อัพเดทข้อมูลเงินเดือนสำเร็จ");

                    }

                }

            }
            else if (eventArg == "Post_Update")  // ยิงข้อมูล update ข้อมูลพนักงานเพิ่มเติมให้ table บัญชี
            
            {
                string ref_membno = ""; string prename_code = ""; string emp_name = ""; string emp_surname = "";
                DateTime work_date; string emp_status = ""; DateTime resign_date; string coop_id = state.SsCoopId;

                string sql = @"select trim(hr.ref_membno) as ref_membno ,
                                hr.prename_code,
                                hr.emp_name,
                                hr.emp_surname,
                                hr.work_date,
                                (case when hr.emp_status = '2' then '1' else '0' end) as emp_status, 
                                hr.resign_date 
                                from hremployee hr where ref_membno is not null";
                sql = WebUtil.SQLFormat(sql);//format ในรูปของ sql
                Sdt dt = WebUtil.QuerySdt(sql);
                dt = WebUtil.QuerySdt(sql);
                if (dt.Next())
               
                {
                    for (int r = 0; r < dt.GetRowCount(); r++)
                    {
                        ref_membno = dt.Rows[r]["ref_membno"].ToString();
                        prename_code = dt.Rows[r]["prename_code"].ToString();
                        emp_name = dt.Rows[r]["emp_name"].ToString();
                        emp_surname = dt.Rows[r]["emp_surname"].ToString();
                        work_date = Convert.ToDateTime(dt.Rows[r]["work_date"].ToString());
                        emp_status = dt.Rows[r]["emp_status"].ToString();

                        try
                        {

                            resign_date = Convert.ToDateTime(dt.Rows[r]["resign_date"].ToString());

                            string sql_check = @"select member_no from accemployee where member_no = '" + ref_membno + "'";
                            sql_check = WebUtil.SQLFormat(sql_check);
                            Sdt dt_check = WebUtil.QuerySdt(sql_check);
                            dt_check = WebUtil.QuerySdt(sql_check);
                            if (dt_check.Next())
                            {
                                string sql_emp = @"update accemployee set prename_code = {0},memb_name = {1},memb_surname = {2},work_date = {3},resign_status = {4},resign_date = {5} where trim(member_no)={6}";
                                sql_emp = WebUtil.SQLFormat(sql_emp, prename_code, emp_name, emp_surname, work_date, emp_status, resign_date, ref_membno);
                                Sdt dt_emp = WebUtil.QuerySdt(sql_emp);

                            }
                            else
                            {

                                string ls_sql = @"insert into accemployee(coop_id,member_no,prename_code,memb_name,memb_surname,work_date,resign_status,resign_date,used_flag)
                                          values({0},{1},{2},{3},{4},{5},{6},{7},{8})";
                                ls_sql = WebUtil.SQLFormat(ls_sql, coop_id, ref_membno, prename_code, emp_name, emp_surname, work_date, emp_status, resign_date, 0);
                                exe.SQL.Add(ls_sql);
                                exe.Execute();
                                exe.SQL.Clear();

                            }

                        }
                        catch
                        {
                            string sql_check = @"select member_no from accemployee where member_no = '" + ref_membno + "'";
                            sql_check = WebUtil.SQLFormat(sql_check);
                            Sdt dt_check = WebUtil.QuerySdt(sql_check);
                            dt_check = WebUtil.QuerySdt(sql_check);
                            if (dt_check.Next())
                            {
                                string sql_emp = @"update accemployee set prename_code = {0},memb_name = {1},memb_surname = {2},work_date = {3},resign_status = {4},resign_date = {5} where trim(member_no)={6}";
                                sql_emp = WebUtil.SQLFormat(sql_emp, prename_code, emp_name, emp_surname, work_date, emp_status, "", ref_membno);
                                Sdt dt_emp = WebUtil.QuerySdt(sql_emp);

                            }
                            else
                            {

                                string ls_sql = @"insert into accemployee(coop_id,member_no,prename_code,memb_name,memb_surname,work_date,resign_status,resign_date,used_flag)
                                          values({0},{1},{2},{3},{4},{5},{6},{7},{8})";
                                ls_sql = WebUtil.SQLFormat(ls_sql, coop_id, ref_membno, prename_code, emp_name, emp_surname, work_date, emp_status, "", 0);
                                exe.SQL.Add(ls_sql);
                                exe.Execute();
                                exe.SQL.Clear();

                            }

                    }

                }

                    LtServerMessage.Text = WebUtil.CompleteMessage("อัพเดทข้อมูลสำเร็จ");
                }

            }
        }

        public void SaveWebSheet()
        {

        }

        public void WebSheetLoadEnd()
        {

        }
    }
}