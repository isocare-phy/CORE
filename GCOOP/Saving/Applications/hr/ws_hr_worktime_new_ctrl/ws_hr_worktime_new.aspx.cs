using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using System.Data;
using DataLibrary;
using System.Globalization;
using System.Drawing;
using System.Data;

namespace Saving.Applications.hr.ws_hr_worktime_new_ctrl
{
    public partial class ws_hr_worktime_new : PageWebSheet, WebSheet
    {
        [JsPostBack]
        public string PostWorkDate { get; set; }
        [JsPostBack]
        public string PostEmpno { get; set; }
        [JsPostBack]
        public string PostEmptype { get; set; }

        public void InitJsPostBack()
        {
            dsList.InitDs(this);
            dsMain.InitDs(this);
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                string fmt = "00";
                dsMain.DATA[0].year = Convert.ToString(state.SsWorkDate.Year + 543);
                dsMain.DATA[0].month = state.SsWorkDate.Month.ToString(fmt);
                dsMain.DdEmpno();
                dsMain.DdEmptype();
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            DateTime WorkDate = state.SsWorkDate;
            string emp_no = dsMain.DATA[0].EMP_NO;
            string year = dsMain.DATA[0].year;
            year = Convert.ToString(Convert.ToDecimal(dsMain.DATA[0].year) - 543);
            string month = dsMain.DATA[0].month;
            if (eventArg == PostEmpno)
            {
                dsList.ResetRow();
                dsMain.DATA[0].emptype_code = "";
                ExecuteDataSource exe = new ExecuteDataSource(this);
                dsList.RetrieveShowEmp(year, month, emp_no);
                Looptime();
            }
        }

        public void SaveWebSheet()
        {
            string year = Convert.ToString(Convert.ToDecimal(dsMain.DATA[0].year) - 543);
            string emp_no = dsList.DATA[0].EMP_NO;
            string month = dsMain.DATA[0].month;
            string emptype_code = dsMain.DATA[0].emptype_code;
            string day_work = "", worktime_code = "";
            ExecuteDataSource exe = new ExecuteDataSource(this);

            for (int i = 0; i < dsList.RowCount; i++)
            {

                // เช็ควันหยุดยิง day_work
                decimal day = i + 1;

                string sql_workday = @"select 
                                                (case when substr(workdays,{2},1) = 'H' then  '0'
                                                 when substr(workdays,{2},1) = 'W' then '1'
                                                 else '0' end) as day_work
                                                 from amworkcalendar where year = {0} and month = {1}";
                sql_workday = WebUtil.SQLFormat(sql_workday, Convert.ToDecimal(year) + 543, month, day);
                Sdt dt_workday = WebUtil.QuerySdt(sql_workday);
                if (dt_workday.Next())
                {
                    day_work = dt_workday.GetString("day_work");
                }

                string START_HOUR = "", START_MINUTE = "", END_HOUR = "", END_MINUTE = "";
                DateTime work_date = dsList.DATA[i].WORK_DATE;
                dsList.DATA[i].WORK_DATE = work_date;
                START_HOUR = dsList.DATA[i].work_in.Substring(0, 2);
                dsList.DATA[i].START_HOUR = Convert.ToInt16(START_HOUR);
                START_MINUTE = dsList.DATA[i].work_in.Substring(3, 2);
                dsList.DATA[i].START_MINUTE = Convert.ToInt16(START_MINUTE);
                END_HOUR = dsList.DATA[i].work_out.Substring(0, 2);
                dsList.DATA[i].END_HOUR = Convert.ToInt16(END_HOUR);
                END_MINUTE = dsList.DATA[i].work_out.Substring(3, 2);
                dsList.DATA[i].END_MINUTE = Convert.ToInt16(END_MINUTE);


                string work_in = START_HOUR + "." + START_MINUTE;
                string work_out = END_HOUR + "." + END_MINUTE;
                string times = HourTime(work_in, work_out);
                dsList.DATA[i].WORK_TIME = times;

                DateTime WorkDate;
                DateTime lateleave_time = new DateTime();
                WorkDate = dsList.DATA[i].WORK_DATE;
                string WorkDate_s = Convert.ToString(WorkDate);
                string monthDate_1 = Convert.ToString(WorkDate.Month);

                int Length_month = monthDate_1.Length;

                if (Length_month < 2)
                {
                    monthDate_1 = "0" + monthDate_1;
                }

                string dayDate_1 = Convert.ToString(WorkDate.Day);
                int Length_day = dayDate_1.Length;

                if (Length_day < 2)
                {
                    dayDate_1 = "0" + dayDate_1;
                }

                string yearDate_1 = Convert.ToString(WorkDate.Year);
                string WorkDate_s_all = dayDate_1 + "/" + monthDate_1 + "/" + yearDate_1;

                string ls_sqldel2 = @"delete from hrloglate where TO_CHAR(late_date,'dd/mm/yyyy') ='" + WorkDate_s_all + "' and emp_no ='" + dsList.DATA[i].EMP_NO + "' ";
                exe.SQL.Add(ls_sqldel2);
                exe.Execute();
                exe.SQL.Clear();


                string ls_empno = dsList.DATA[i].EMP_NO;
                string standard_time_s = ""; string max_late = "", standard_time_e = ""; //string worktime_code = "";
                string back_time = "";
                string sql = "select work_in , max_late ,worktime_code from hrucfworktimecode where worktime_code = (select worktime_code from hremployee where emp_no = '" + ls_empno + "')";
                Sdt dt = WebUtil.QuerySdt(sql);
                if (dt.Next())
                {
                    standard_time_s = dt.GetString("work_in");   //เวลาเข้างาน ปกติ
                    max_late = dt.GetString("max_late");         //เวลาเข้างาน สายสูฃสุด
                }

                standard_time_s = max_late.Replace(".", ":") + ":00";  //เอา สายสูงสุดมาคิด
                string max_late_time = "00:" + max_late + ":00";

                DateTime standard_time = DateTime.ParseExact(standard_time_s, "HH:mm:ss", CultureInfo.InvariantCulture);
                //DateTime standard_time_max = DateTime.ParseExact(max_late_time, "HH:mm:ss", CultureInfo.InvariantCulture);

                //standard_time_max = standard_time.AddMinutes(Convert.ToDouble(max_late));
                //standard_time_max = Convert.ToDateTime(standard_time_max);

                standard_time = Convert.ToDateTime(standard_time);

                dsList.DATA[i].COOP_ID = state.SsCoopId;
                dsList.DATA[i].START_TIME = new DateTime(WorkDate.Year, WorkDate.Month, WorkDate.Day, dsList.DATA[i].START_HOUR, dsList.DATA[i].START_MINUTE, 0);
                dsList.DATA[i].END_TIME = new DateTime(WorkDate.Year, WorkDate.Month, WorkDate.Day, dsList.DATA[i].END_HOUR, dsList.DATA[i].END_MINUTE, 0);

                string sql_worktime_code = "select work_in , work_out , max_late from hrucfworktimecode where worktime_code = (select worktime_code from hremployee where emp_no = '" + ls_empno + "')";
                Sdt dt_worktime_code = WebUtil.QuerySdt(sql_worktime_code);
                if (dt_worktime_code.Next())
                {
                    standard_time_s = dt_worktime_code.GetString("work_in");
                    standard_time_e = dt_worktime_code.GetString("work_out");
                }
                if (Convert.ToDecimal(standard_time_e) > Convert.ToDecimal(work_out) && Convert.ToDecimal(work_out) != 0)
                {
                    back_time = HourTime(work_out, standard_time_e);
                }

                PutLogLeave(dsList.DATA[i].EMP_NO, dsList.DATA[i].WORK_DATE, dsList.DATA[i].START_TIME, standard_time, lateleave_time, max_late, emptype_code);
                try
                {
                    string workdays = "";
                    int days = dsList.DATA[i].WORK_DATE.Day;
                    workdays = Holiday(days);

                    if (work_in == "00.00" && work_out == "00.00" && dsList.DATA[i].leave_code == "" && workdays =="W")
                    {
                        worktime_code = "LW";

                        String update = @"update HRLOGWORKTIME set START_TIME = {0} ,END_TIME = {1}, WORK_TIME = {2} , worktime_code = {3} , emptype_code = {4} , day_work = {5} , back_time = {6} where COOP_ID ={7} and WORK_DATE = {8} and EMP_NO =  {9}";
                        update = WebUtil.SQLFormat(update, dsList.DATA[i].START_TIME, dsList.DATA[i].END_TIME, dsList.DATA[i].WORK_TIME, worktime_code, dsList.DATA[i].emptype_code, day_work, back_time, dsList.DATA[i].COOP_ID, dsList.DATA[i].WORK_DATE, dsList.DATA[i].EMP_NO);
                        Sdt dt_update = WebUtil.QuerySdt(update);
                        LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
                    }
                    else
                    {

                        String update = @"update HRLOGWORKTIME set START_TIME = {0} ,END_TIME = {1}, WORK_TIME = {2}  , emptype_code = {3} , day_work = {4} , back_time = {5} where COOP_ID ={6} and WORK_DATE = {7} and EMP_NO =  {8}";
                        update = WebUtil.SQLFormat(update, dsList.DATA[i].START_TIME, dsList.DATA[i].END_TIME, dsList.DATA[i].WORK_TIME, dsList.DATA[i].emptype_code, day_work, back_time, dsList.DATA[i].COOP_ID, dsList.DATA[i].WORK_DATE, dsList.DATA[i].EMP_NO);
                        Sdt dt_update = WebUtil.QuerySdt(update);
                        LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
                    }
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("บันทึกไม่สำเร็จ" + ex.Message);
                }

            }
            dsList.RetrieveShowEmp(year, month, emp_no);
            Looptime();
        }

        public void WebSheetLoadEnd()
        {

        }

        public void PutLogLeave(string emp_no, DateTime work_date, DateTime start_time, DateTime standard_time, DateTime standard_time_max, string max_late, string emptype_code)
        {

            ExecuteDataSource exe = new ExecuteDataSource(this);
            decimal month_d = dsList.DATA[0].WORK_DATE.Month;
            decimal year = work_date.Year;
            try
            {
                if (start_time.TimeOfDay > standard_time.TimeOfDay)
                {
                    int diff_hour = start_time.Hour - standard_time.Hour;

                    if (start_time.Hour > 8 && start_time.Minute < 30) // แก้กรณสาย 9 โมงขึ้นไป ชั่วโมงเกิน 1 ชั่วโมง
                    {
                        diff_hour = diff_hour - 1;
                    }

                    int diff_minute = 0; //Math.Abs(start_time.Minute - standard_time.Minute);
                    string late_cause = "";
                    if (start_time.Hour == standard_time.Hour)
                    {
                        diff_hour = 0;
                    }
                    if (start_time.Minute == 30)
                    {
                        diff_minute = 0;
                    }
                    else if (start_time.Minute < 30)
                    {
                        diff_minute = standard_time.Minute + start_time.Minute;
                    }
                    else if (start_time.Minute > 30)
                    {
                        diff_minute = start_time.Minute - standard_time.Minute;
                    }

                    if (diff_hour == 0 && diff_minute < Convert.ToDecimal(max_late))
                    {
                        late_cause = "0";
                    }
                    else if (diff_hour == 0 && diff_minute >= Convert.ToDecimal(max_late))
                    {
                        late_cause = "1";
                    }
                    else if (diff_hour > 0)
                    {
                        late_cause = "1";
                    }

                    string sql_check = "select * from hrloglate where emp_no={0} and coop_id={1} and late_date={2}";
                    sql_check = WebUtil.SQLFormat(sql_check, emp_no, state.SsCoopId, work_date);
                    Sdt dt_chk = WebUtil.QuerySdt(sql_check);
                    if (!dt_chk.Next())
                    {
                        decimal seq = 1;
                        string sql_get_seq = "select nvl(max(seq_no),0) + 1 as seq_no from hrloglate where emp_no={0} and coop_id={1}";
                        sql_get_seq = WebUtil.SQLFormat(sql_get_seq, emp_no, state.SsCoopId);
                        Sdt dt_seq = WebUtil.QuerySdt(sql_get_seq);
                        if (dt_seq.Next())
                        {
                            seq = dt_seq.GetDecimal("seq_no");
                        }

                        string sql_put_log = @"insert into hrloglate(coop_id, emp_no, seq_no, late_date, late_time, late_cause)
                            VALUES ({0},{1},{2},{3},{4},{5})";
                        sql_put_log = WebUtil.SQLFormat(sql_put_log, state.SsCoopId, emp_no, seq, work_date,
                            new DateTime(work_date.Year, work_date.Month, work_date.Day, diff_hour, diff_minute, 0), "2");
                        Sdt dt_insert = WebUtil.QuerySdt(sql_put_log);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("ไม่สามารถบันทึกการมาสายได้ กรุณาตรวจสอบ" + ex.Message);
            }
        }
        private string Holiday(int day)
        {
            string workdays = "";
            string sql = "select  substr(workdays,{0},1) as  workdays  from amworkcalendar where year ={1} and month = {2}";
            sql = WebUtil.SQLFormat(sql, day, dsMain.DATA[0].year, dsMain.DATA[0].month);
            Sdt dt = WebUtil.QuerySdt(sql);
            if (dt.Next())
            {
                workdays = dt.GetString("workdays");
            }
            return Convert.ToString(workdays);
        }

        private string HourTime(string work_in, string work_out)
        {
            string stime = work_in;
            string ltime = work_out;
            decimal statime = Convert.ToDecimal(stime);
            decimal entime = Convert.ToDecimal(ltime);
            decimal hours = 0;


            string[] args = Convert.ToString(stime).Split('.');
            string[] args2 = Convert.ToString(ltime).Split('.');
            string s1, s2, l1, l2;
            decimal all = 0;
            s1 = args[0];
            s2 = args[1];
            l1 = args2[0];
            l2 = args2[1];

            stime = s1 + ':' + s2;
            ltime = l1 + ':' + l2;
            string startTime = stime;
            string endTime = ltime;
            TimeSpan duration = DateTime.Parse(endTime).Subtract(DateTime.Parse(startTime));
            string sumtime = Convert.ToString(duration);
            string[] args3 = Convert.ToString(sumtime).Split(':');
            string th, tm, ts;
            th = args3[0];
            tm = args3[1];
            ts = args3[2];
            hours = Convert.ToDecimal(th + "." + tm);
            all = hours;
            all = all;
            if (all < 0)
            {
                all = 0;
            }
            return Convert.ToString(all);

        }
        public void Looptime()
        {
            for (int r = 0; r < dsList.RowCount; r++)
            {
                string emp = "", workdays = "";
                string ls_emp_no = dsList.DATA[r].EMP_NO;
                string sql_workin = "select emp_no  from hrloglate where late_date = {0} and emp_no ={1}";
                sql_workin = WebUtil.SQLFormat(sql_workin, dsList.DATA[r].WORK_DATE, ls_emp_no);
                Sdt dt_workin = WebUtil.QuerySdt(sql_workin);
                if (dt_workin.Next())
                {
                    emp = dt_workin.GetString("emp_no");
                    dsList.FindTextBox(r, "work_in").ForeColor = Color.Red;
                    dsList.FindTextBox(r, "work_out").ForeColor = Color.Red;
                }

                if (dsList.DATA[r].leave_code != "")
                {
                    dsList.FindTextBox(r, "work_in").ForeColor = Color.Red;
                    dsList.FindTextBox(r, "work_out").ForeColor = Color.Red;
                }
                if (dsList.DATA[r].work_in == "00.00" && dsList.DATA[r].work_out == "00.00")
                {
                    dsList.FindTextBox(r, "work_in").ForeColor = Color.Red;
                    dsList.FindTextBox(r, "work_out").ForeColor = Color.Red;
                }
                if (dsList.DATA[r].work_in == "00.00" || dsList.DATA[r].work_out == "00.00")
                {
                    dsList.FindTextBox(r, "work_in").ForeColor = Color.Red;
                    dsList.FindTextBox(r, "work_out").ForeColor = Color.Red;
                }
                if (dsList.DATA[r].worktime.Trim() == "HO")
                {
                    dsList.FindTextBox(r, "work_in").ForeColor = Color.Red;
                    dsList.FindTextBox(r, "work_out").ForeColor = Color.Red;
                }
                if (dsList.DATA[r].back_time != "")
                {
                    dsList.FindTextBox(r, "work_in").ForeColor = Color.Red;
                    dsList.FindTextBox(r, "work_out").ForeColor = Color.Red;
                }

                int day = dsList.DATA[r].WORK_DATE.Day;
                workdays = Holiday(day);

                if (workdays == "H" && dsList.DATA[r].WORKTIME_CODE != "HO") //วันหยุด
                {
                    if (dsList.DATA[r].work_in == "00.00" && dsList.DATA[r].work_out == "00.00")
                    {
                        dsList.DATA[r].WORKTIME_CODE = "วันหยุด";
                        dsList.FindTextBox(r, "work_in").ForeColor = Color.Green;
                        dsList.FindTextBox(r, "work_out").ForeColor = Color.Green;
                    }
                    else if (dsList.DATA[r].work_in == "00.00" || dsList.DATA[r].work_out == "00.00")
                    {
                        dsList.DATA[r].WORKTIME_CODE = "วันหยุด";
                        dsList.FindTextBox(r, "work_in").ForeColor = Color.Red;
                        dsList.FindTextBox(r, "work_out").ForeColor = Color.Red;
                    }
                    //update tpye วันหยุด เพื่ออกรายงาน
                    String update = @"update hrlogworktime set worktime_code ='HO' where coop_id =  {0} and TRUNC(work_date) = {1} and emp_no ={2}";
                    update = WebUtil.SQLFormat(update, state.SsCoopId, dsList.DATA[r].WORK_DATE, dsList.DATA[r].EMP_NO);
                    Sdt dt_update = WebUtil.QuerySdt(update);
                }

            }
        }
    }
}