using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using System.Data;
using DataLibrary;

namespace Saving.Applications.hr.ws_hr_worktime_ctrl
{
    public partial class ws_hr_worktime : PageWebSheet, WebSheet
    {
        [JsPostBack]
        public string PostWorkDate { get; set; }
        [JsPostBack]
        public string PostInsertRow { get; set; }
        [JsPostBack]
        public string PostDeleteRow { get; set; }
        [JsPostBack]
        public string PostEmpNo { get; set; }

        public void InitJsPostBack()
        {
            dsList.InitDs(this);
            dsMain.InitDs(this);
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                dsMain.DATA[0].WORK_DATE = state.SsWorkDate; //กำหนดค่า วันให้เป็นวันปัจจุบัน ที่ดึงจาก State
                //dsList.RetrieveLogWorkDate(state.SsWorkDate);
                decimal c_emp_no = 0;
                string sql = @"select count(emp_no) as c_emp_no
                                from hrlogworktime 
                                where 
                                work_date ={0}
                                and coop_id ={1}";
                sql = WebUtil.SQLFormat(sql, state.SsWorkDate, state.SsCoopId);
                Sdt dt = WebUtil.QuerySdt(sql);
                if (dt.Next())
                {
                    c_emp_no = dt.GetDecimal("c_emp_no");


                }

                if (state.SsCoopControl == "031001")
                {

                    if (c_emp_no > 0)
                    {

                        dsList.RetrieveShowEmp(state.SsWorkDate);

                    }
                    else
                    {
                        dsList.RetrieveShowEmp2(state.SsWorkDate);

                    }

                }
                else
                {



                    dsList.RetrieveShowEmp2(state.SsWorkDate);

                }
            }
           // else {
          // }
            //dsList.RetrieveLogWorkDate(dsMain.DATA[0].WORK_DATE);//โชว์ ข้อมูล กรณีมีข้อมูลแล้วของวันนั้น
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == PostWorkDate)
            {
                DateTime WorkDate = dsMain.DATA[0].WORK_DATE;
                if (WorkDate != state.SsWorkDate)
                {
                    dsList.RetrieveLogWorkDate(WorkDate);

                    if (state.SsCoopControl == "031001")
                    {

                        if (dsList.RowCount == 0)
                        {
                            dsList.RetrieveShowEmp2(WorkDate);
                        }
                        else
                        {

                            dsList.RetrieveShowEmp(WorkDate);
                        }

                    }
                    else
                    {

                        dsList.RetrieveShowEmp2(WorkDate);

                    }
                }
               
                dsList.DATA[0].WORK_DATE = (WorkDate);
                //dsList.Ddhrucfworktimecode();//retrieve Ddlist

            }
            else if (eventArg == PostEmpNo)
            {
                int r = Convert.ToInt32(HdRows.Value);//dsList.GetRowFocus();// กำหนด r แถว โดยดึงจาก???????
                string EmpNo = dsList.DATA[r].EMP_NO;
                ///////////////////////////////////////////////////////////////////////
                string sql = @"select mp.prename_code,mp.prename_desc,he.emp_name,he.emp_surname 
                                from hremployee he, mbucfprename mp
                                where he.prename_code = mp.prename_code
                                and he.emp_no ={0}
                                and he.coop_id ={1}";
                sql = WebUtil.SQLFormat(sql, EmpNo, state.SsCoopId);
                Sdt dt = WebUtil.QuerySdt(sql);
                if (dt.Next()) {

                    //dsList.DATA[r].FULLNAME = dt.GetString("prename_desc") + dt.GetString("emp_name") + " " + dt.GetString("emp_surname");
                    dsList.DATA[r].PRENAME_DESC = dt.GetString("prename_desc");
                    dsList.DATA[r].EMP_NAME = dt.GetString("emp_name");
                    dsList.DATA[r].EMP_SURNAME = dt.GetString("emp_surname");
                }
                ///////////////////////////////////////////////////////////////////////

            }           
            /*else if (eventArg == PostInsertRow)
            {
                dsList.InsertLastRow();
                int r = dsList.RowCount;
                dsList.Ddhrucfworktimecode();
            }*/
            else if (eventArg == PostDeleteRow)
            {
                int rowDel = dsList.GetRowFocus();
                dsList.DeleteRow(rowDel);
                dsList.Ddhrucfworktimecode();
            }
        }

        public void SaveWebSheet()
        {

            

            try
            {
                /*///////////////////////////////////////////////////////////////
                int r = dsList.GetRowFocus();
                //string EmpNo = dsMain.DATA[r].EMP_NO;
                string CoopId = state.SsCoopId;
                string WorkTime = dsList.DATA[r].WORKTIME_CODE;
                string sql = @"select * from hrcfconstant where coop_id ={0} and work_time ={1}";
                sql = WebUtil.SQLFormat(sql, CoopId, WorkTime);
                Sdt dt = WebUtil.QuerySdt(sql);*/
                ///////////////////////////////////////////////////////////////

                ///////////////////////////////////////////////////////////////
               
                //dsList.Ddhrucfworktimecode();


                DateTime standard_time = new DateTime();
                DateTime lateleave_time = new DateTime();
                

                string sql = "select work_time from hrcfconstant";
                Sdt dt = WebUtil.QuerySdt(sql);
                if (dt.Next())
                {
                    standard_time = dt.GetDate("work_time");
                }

                DateTime WorkDate = dsMain.DATA[0].WORK_DATE;
                lateleave_time = new DateTime(WorkDate.Year, WorkDate.Month, WorkDate.Day, 10, 0, 0);

                string WorkDate_s = Convert.ToString(WorkDate);

                //string monthDate_1 = WorkDate_s.Substring(0, 1);

                string monthDate_1 = Convert.ToString(WorkDate.Month);


                if (monthDate_1 == "1" || monthDate_1 == "2" || monthDate_1 == "3" || monthDate_1 == "4" || monthDate_1 == "5" || monthDate_1 == "6" || monthDate_1 == "7" || monthDate_1 == "8" || monthDate_1 == "9")
                {

                    monthDate_1 = "0" + monthDate_1;
                }
                else
                {
                    monthDate_1 = monthDate_1;
                }


                //string dayDate_1 = WorkDate_s.Substring(2, 2);

                string dayDate_1 = Convert.ToString(WorkDate.Day);

                //string yearDate_1 = Convert.ToString(WorkDate.year);

                string yearDate_1 = WorkDate_s.Substring(6, 4);

                string WorkDate_s_all = dayDate_1 + "/" + monthDate_1 + "/" + yearDate_1;



                ExecuteDataSource exe = new ExecuteDataSource(this);
                string ls_sqldel = @"delete from hrlogworktime where TO_CHAR(work_date,'dd/mm/yyyy') = '" + WorkDate_s_all + "'";
                exe.SQL.Add(ls_sqldel);
                exe.Execute();
                exe.SQL.Clear();

                string ls_sqldel2 = @"delete from hrloglate where TO_CHAR(late_date,'dd/mm/yyyy') ='" + WorkDate_s_all + "'";
                exe.SQL.Add(ls_sqldel2);
                exe.Execute();
                exe.SQL.Clear();

                for (int i = 0; i < dsList.RowCount; i++) {

                    string ls_empno = dsList.DATA[i].EMP_NO;
                    string worktime = dsList.DATA[i].WORKTIME_CODE;
                    dsList.DATA[i].WORK_DATE = WorkDate;
                    dsList.DATA[i].COOP_ID = state.SsCoopId;
                    dsList.DATA[i].START_TIME = new DateTime(WorkDate.Year, WorkDate.Month, WorkDate.Day, dsList.DATA[i].START_HOUR, dsList.DATA[i].START_MINUTE, 0);
                    dsList.DATA[i].END_TIME = new DateTime(WorkDate.Year, WorkDate.Month, WorkDate.Day, dsList.DATA[i].END_HOUR, dsList.DATA[i].END_MINUTE, 0);
                    PutLogLeave(dsList.DATA[i].EMP_NO, WorkDate, dsList.DATA[i].START_TIME, standard_time, lateleave_time);


                    string sql2 = @"update hrlogworktime set worktime_code = {0} where emp_no = {1} and work_date = {2}";
                    sql2 = WebUtil.SQLFormat(sql2, worktime, ls_empno, WorkDate);
                    Sdt dt1 = WebUtil.QuerySdt(sql2);

                    if (dt1.Next())
                    {

                        worktime = dt1.GetString("worktime_code");
                        
                    }

                    dsList.DATA[i].WORKTIME_CODE = worktime;
                }

                //ExecuteDataSource exe = new ExecuteDataSource(this);
                exe.AddRepeater(dsList);  
                exe.Execute();
                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("บันทึกไม่สำเร็จ"+ex.Message);
            }
        }

        public void WebSheetLoadEnd()
        {

        }

        public void PutLogLeave(string emp_no, DateTime work_date, DateTime start_time, DateTime standard_time, DateTime lateleave_time)
        {

            decimal month_d = dsMain.DATA[0].WORK_DATE.Month;
            string month = "";

            if (Convert.ToString(month_d) == "1" || Convert.ToString(month_d) == "2" || Convert.ToString(month_d) == "3" || Convert.ToString(month_d) == "4" || Convert.ToString(month_d) == "5" || Convert.ToString(month_d) == "6" || Convert.ToString(month_d) == "7" || Convert.ToString(month_d) == "8" || Convert.ToString(month_d) == "9")
            {

                month = "0" + Convert.ToString(month_d);
            }
            else
            {
                month = Convert.ToString(month_d);
            }

            decimal year = dsMain.DATA[0].WORK_DATE.Year;
            string emp_no_check = "";
            string late_date = "";
            decimal late = 0;
            string late_date_sum = "";
            string late_date_sum2 = "";

            try { 
                
                if (start_time.TimeOfDay > standard_time.TimeOfDay) {
                    if (start_time.TimeOfDay > lateleave_time.TimeOfDay){
                        ExecuteDataSource exed = new ExecuteDataSource(this);
                        string coop_id = state.SsCoopControl;
                        decimal SeqNo = 1;
                        string sql = "select * from hrlogleave where coop_id = {0} and emp_no = {1}";
                        sql = WebUtil.SQLFormat(sql, state.SsCoopControl, emp_no);
                        Sdt dt = WebUtil.QuerySdt(sql);
                        string row = dt.ToString();
                        try
                        {
                            if (dt.Rows.Count <= 0)
                            {
                                string CoopId = state.SsCoopId;//unique
                                SeqNo = 1;//unique
                                string sql3 = @"insert into hrlogleave(coop_id, emp_no, seq_no, operate_date, leave_code, leave_from,
                                                leave_to, totalday)
                                                VALUES ({0},{1},{2},{3},{4},{5},{6},{7})";
                                sql3 = WebUtil.SQLFormat(sql3, state.SsCoopId, emp_no, SeqNo, work_date, "001", work_date, work_date, 0.5);
                                Sdt dt3 = WebUtil.QuerySdt(sql3);

                            }
                            else
                            {
                                string CoopId = state.SsCoopId;//unique
                                SeqNo = 1;//unique
                                string sql2 = @"select max(seq_no)+1 as seq_no from hrlogleave where emp_no ={0} and coop_id={1}";//นำค่าแมกของ seq_no บวก 1 เพื่อให้เป็นค่า seq_no ของลำดับต่อไป
                                sql2 = WebUtil.SQLFormat(sql2, emp_no, CoopId);//format ในรูปของ sql
                                Sdt dt2 = WebUtil.QuerySdt(sql2);
                                if (dt2.Next())
                                {
                                    SeqNo = dt2.GetDecimal("seq_no");
                                }
                                string sql4 = @"insert into hrlogleave(coop_id, emp_no, seq_no, operate_date, leave_code, leave_from,
                                                leave_to, totalday)
                                                VALUES ({0},{1},{2},{3},{4},{5},{6},{7})";
                                sql4 = WebUtil.SQLFormat(sql4, state.SsCoopId, emp_no, SeqNo, work_date, "001", work_date, work_date, 0.5);
                                Sdt dt4 = WebUtil.QuerySdt(sql4);
                            }
                        }catch (Exception ex){
                            LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                        }
                    }else{
                        

                        int diff_hour = start_time.Hour - standard_time.Hour;
                        int diff_minute = Math.Abs(start_time.Minute - standard_time.Minute);
                        string sql_check = "select * from hrloglate where emp_no={0} and coop_id={1} and late_date={2}";
                        sql_check = WebUtil.SQLFormat(sql_check, emp_no, state.SsCoopId, work_date);
                        Sdt dt_chk = WebUtil.QuerySdt(sql_check);
                        if (!dt_chk.Next()) {
                            decimal seq = 1;
                            string sql_get_seq = "select nvl(max(seq_no),0) + 1 as seq_no from hrloglate where emp_no={0} and coop_id={1}";
                            sql_get_seq = WebUtil.SQLFormat(sql_get_seq, emp_no, state.SsCoopId);
                            Sdt dt_seq = WebUtil.QuerySdt(sql_get_seq);
                            if (dt_seq.Next()) {
                                seq = dt_seq.GetDecimal("seq_no");
                            }

                            string sql_put_log = @"insert into hrloglate(coop_id, emp_no, seq_no, late_date, late_time, late_cause)
                            VALUES ({0},{1},{2},{3},{4},{5})";
                            sql_put_log = WebUtil.SQLFormat(sql_put_log, state.SsCoopId, emp_no, seq, work_date,
                                new DateTime(work_date.Year,work_date.Month,work_date.Day,diff_hour,diff_minute,0), "");
                            Sdt dt_insert = WebUtil.QuerySdt(sql_put_log);

                            string sql_hrchecklate = @"select emp_no,late_date from hrcheckleave where emp_no = {0} and year = {1} and month = {2}";
                            sql_hrchecklate = WebUtil.SQLFormat(sql_hrchecklate, emp_no, year, month);
                            // Sdt dt = WebUtil.QuerySdt(sql);
                            Sdt dt_checklate = WebUtil.QuerySdt(sql_hrchecklate);
                            if (dt_checklate.Next())
                            {
                                emp_no_check = dt_checklate.GetString("emp_no");
                                late_date = dt_checklate.GetString("late_date");
                            }

                            if (emp_no_check == "")
                            {
                                string ls_sql_checklate = @"insert into hrcheckleave(emp_no,year,month,leave_001_s,leave_001_c,leave_002_s,leave_002_c,leave_004_s,leave_004_c,leave_006_s,leave_006_c,late_date)values({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11})";
                                ls_sql_checklate = WebUtil.SQLFormat(ls_sql_checklate, emp_no, year, month, '0', '0', '0', '0', '0', '0', '0', '0', '0');
                                ExecuteDataSource exe = new ExecuteDataSource(this);
                                exe.SQL.Add(ls_sql_checklate);
                                exe.Execute();
                                exe.SQL.Clear();

                                string sql_hrchecklate1 = @"select count(late_date) as late_date from hrloglate where emp_no = {0} and TO_CHAR(late_date,'yyyy') = {1} and TO_CHAR(late_date,'mm') = {2}";
                                sql_hrchecklate1 = WebUtil.SQLFormat(sql_hrchecklate1, emp_no, year, month);
                                // Sdt dt = WebUtil.QuerySdt(sql);
                                Sdt dt_checklate1 = WebUtil.QuerySdt(sql_hrchecklate1);
                                if (dt_checklate1.Next())
                                {
                                    late_date_sum = dt_checklate1.GetString("late_date");
                                }

                                late = Convert.ToDecimal(late_date_sum);

                                string sqllate = @"update hrcheckleave set late_date = {0} where emp_no={1} and year = {2} and month = {3}";
                                sqllate = WebUtil.SQLFormat(sqllate, Convert.ToString(late), emp_no, year, month);
                                Sdt dt_late = WebUtil.QuerySdt(sqllate);
                            }
                            else
                            {
                                string sql_hrchecklate2 = @"select count(late_date) as late_date from hrloglate where emp_no = {0} and TO_CHAR(late_date,'yyyy') = {1} and TO_CHAR(late_date,'mm') = {2}";
                                sql_hrchecklate2 = WebUtil.SQLFormat(sql_hrchecklate2, emp_no, year, month);
                                // Sdt dt = WebUtil.QuerySdt(sql);
                                Sdt dt_checklate2 = WebUtil.QuerySdt(sql_hrchecklate2);
                                if (dt_checklate2.Next())
                                {
                                    late_date_sum2 = dt_checklate2.GetString("late_date");
                                }

                                late = Convert.ToDecimal(late_date_sum2);
                                string sqllate = @"update hrcheckleave set late_date = {0} where emp_no={1} and year = {2} and month = {3}";
                                sqllate = WebUtil.SQLFormat(sqllate, Convert.ToString(late), emp_no, year, month);
                                Sdt dt_late = WebUtil.QuerySdt(sqllate);
                            }
                        }
                    }
                }   
            
            }catch(Exception ex){
                throw new Exception("ไม่สามารถบันทึกการมาสายได้ กรุณาตรวจสอบ"+ex.Message);
            }
        }
    }
}