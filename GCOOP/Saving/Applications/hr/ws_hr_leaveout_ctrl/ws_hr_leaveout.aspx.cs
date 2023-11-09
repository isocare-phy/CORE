using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using System.Data;
using DataLibrary;


namespace Saving.Applications.hr.ws_hr_leaveout_ctrl
{
    public partial class ws_hr_leaveout : PageWebSheet, WebSheet
    {
        [JsPostBack]
        public string PostEmpNo { get; set; }

        [JsPostBack]
        public string Post { get; set; }

        [JsPostBack]
        public string PostLast { get; set; }

        [JsPostBack]
        public String PostEmpLeave { get; set; }

        [JsPostBack]
        public String PostEmpLeaveDelete { get; set; }

        public void InitJsPostBack()
        {
            dsLeaveout.InitDs(this);
            dsMain.InitDs(this);
            dsDetail.InitDsDetail(this);
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                dsLeaveout.DdHrucfleaveapvby();
                dsLeaveout.DATA[0].COOP_ID = state.SsCoopId;//Data[0] คือ กำหนดข้อมูลที่ฟิวไหน กรณีรูปแบบของ FormView
                dsLeaveout.DATA[0].OPERATE_DATE = state.SsWorkDate;
                dsLeaveout.DATA[0].LEAVE_FROM = state.SsWorkDate;
                dsLeaveout.DATA[0].LEAVE_TO = state.SsWorkDate;
                dsLeaveout.DATA[0].APV_DATE = state.SsWorkDate;
                dsLeaveout.Visible = false;
                dsDetail.Visible = false;
            }
            
        }

        public void CheckJsPostBack(string eventArg)
        {
            string stime = dsLeaveout.DATA[0].START_TIME;
            string ltime = dsLeaveout.DATA[0].LAST_TIME;
            decimal hours = 0, min = 0;

            if (eventArg == PostEmpNo)
            {
                dsLeaveout.Visible = true;
                string EmpNo = dsMain.DATA[0].EMP_NO;
                dsMain.RetrieveEmp(EmpNo);
                dsLeaveout.DATA[0].EMP_NO = EmpNo;//นำข้อมูล emp_no จากที่ RetrieveEmp
                dsDetail.Visible = true;
                dsDetail.RetrieveDetail(EmpNo);
                dsLeaveout.DdHrucfleaveapvby();
                dsLeaveout.DdHrucfleavecode();
            }
            else if (eventArg == PostEmpLeave)
            {
                dsLeaveout.Visible = true;
                int row = dsDetail.GetRowFocus();
                string Seqno = Convert.ToString(dsDetail.DATA[row].SEQ_NO);
                string EmpNo = dsMain.DATA[0].EMP_NO;
                dsLeaveout.Retrieveleave(Seqno, EmpNo);
                dsDetail.Visible = true;
                dsDetail.RetrieveDetail(EmpNo);
                dsLeaveout.DdHrucfleaveapvby();
                dsLeaveout.DdHrucfleavecode();
            }
            else if (eventArg == PostEmpLeaveDelete)
            {
                dsLeaveout.Visible = true;
                int row = dsDetail.GetRowFocus();
                string Seqno = Convert.ToString(dsDetail.DATA[row].SEQ_NO);
                string EmpNo = dsMain.DATA[0].EMP_NO;

                ExecuteDataSource exe = new ExecuteDataSource(this);
                string ls_sqldel = @"delete from hrlogleave where seq_no = " + Seqno + " and emp_no = '" + EmpNo + "'";
                exe.SQL.Add(ls_sqldel);
                exe.Execute();
                exe.SQL.Clear();

                dsDetail.Visible = true;
                dsDetail.RetrieveDetail(EmpNo);
                dsLeaveout.DdHrucfleaveapvby();
                dsLeaveout.DdHrucfleavecode();
            } 
            else if (eventArg == Post)
            {

            }
            else if (eventArg == PostLast)
            {
                try
                {
                    //check condition if(totalhours>3.5) then calculate last_time-start-time=3.5
                    //else totaltime   
                    decimal statime = Convert.ToDecimal(stime);
                    decimal entime = Convert.ToDecimal(ltime);
                    if (statime <= 12 && entime >= 13)
                    {
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
                        all = all - 1;
                        dsLeaveout.DATA[0].TOTALTIME = all;
                    }
                    else 
                    {
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
                        dsLeaveout.DATA[0].TOTALTIME = all;
                    }

                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.WarningMessage(ex.ToString());
                }
                
            }
        }

        public bool checkTime(string EmpNo)
        {
            decimal tbox = 0;
            string sql = @"select 
                                seq_no,emp_no,totaltime
                            from
                                hrlogleave
                            where
                                emp_no={0}
                                order by seq_no";
            decimal totalhour = 0;
            sql = WebUtil.SQLFormat(sql, EmpNo);
            Sdt dt = WebUtil.QuerySdt(sql);
            if (dt.Next())
            {
                //totalhour = dt.GetDecimal("totaltime");
                for (int i = 0; i < dt.GetRowCount(); i++)
                {
                    totalhour += Convert.ToDecimal(dt.Rows[i]["totaltime"]);
                }
            }
            tbox = dsLeaveout.DATA[0].TOTALTIME;
            totalhour += tbox;
            if (Convert.ToDouble(totalhour) > 3.5)
                return true;
            else
                return false;
        }

        public void SaveWebSheet()
        {
            
            ExecuteDataSource exed = new ExecuteDataSource(this);
            string emp_no_check = "";
            string emp_no_check_002 = "";
            string coop_id = state.SsCoopControl;
            string EmpNo = dsMain.DATA[0].EMP_NO;
            string leave_004_s = "";
            string leave_004_c = "";
            decimal sumday_leave004 = 0;
            decimal count_leave004 = 0;
            string leave_002_s = "";
            string leave_002_c = "";
            string leave_code = dsLeaveout.DATA[0].LEAVE_CODE ;
            decimal sumday_leave002 = 0;
            decimal count_leave002 = 0;
            string sql = "select * from hrlogleave where coop_id = {0} and emp_no = {1}";
            string row = dsLeaveout.DATA[0].SEQ_NO.ToString();
            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, dsMain.DATA[0].EMP_NO.Trim());
            Sdt dt = WebUtil.QuerySdt(sql);
            string fullname = dsMain.DATA[0].FULLNAME;
            decimal month_d = dsLeaveout.DATA[0].OPERATE_DATE.Month;
            string month = "";
            decimal sum_totaltime = 0; DateTime operate_date; decimal month_edit_d = 0; decimal year_edit = 0; string month_edit = "";
            string leave_004_edit_s = "";
            string leave_004_edit_c = "";
            decimal original_leave_004_s = 0;
            decimal original_leave_004_c = 0;
            string leave_001_edit_s = "";
            string leave_001_edit_c = "";
            decimal original_leave_001_s = 0;
            decimal original_leave_001_c = 0;
            string leave_001_s = ""; string leave_001_c = ""; decimal sumday_leave001 = 0; decimal count_leave001 = 0;

            if (Convert.ToString(month_d) == "1" || Convert.ToString(month_d) == "2" || Convert.ToString(month_d) == "3" || Convert.ToString(month_d) == "4" || Convert.ToString(month_d) == "5" || Convert.ToString(month_d) == "6" || Convert.ToString(month_d) == "7" || Convert.ToString(month_d) == "8" || Convert.ToString(month_d) == "9")
            {

                month = "0" + Convert.ToString(month_d);
            }
            else
            {
                month = Convert.ToString(month_d);
            }
            decimal year = dsLeaveout.DATA[0].OPERATE_DATE.Year;
            //decimal year = dsLeaveout.DATA[0].TOTALTIME;
            double min = 2.0;
            //decimal status = 0;
            double totaltime = Convert.ToDouble(dsLeaveout.DATA[0].TOTALTIME);
            //status = apv_status();//apv_status= 1 ,-9
            bool check = false;
            check = checkTime(EmpNo);

            try
            {
                if (dt.Rows.Count <= 0)
                {
                    if (totaltime <= min)
                    {
                        try
                        {
                            decimal SeqNo = 1;//unique
                            decimal SeqNo_max = 0;
                            sql = @"select max(seq_no) as seq_no from hrlogleave where emp_no ={0} and coop_id={1}";//นำค่าแมกของ seq_no บวก 1 เพื่อให้เป็นค่า seq_no ของลำดับต่อไป
                            sql = WebUtil.SQLFormat(sql, EmpNo, coop_id);//format ในรูปของ sql
                            dt = WebUtil.QuerySdt(sql);
                            if (dt.Next())
                            {
                                SeqNo = dt.GetDecimal("seq_no");
                                SeqNo_max = SeqNo + 1;
                                
                            }
                            dsLeaveout.DATA[0].SEQ_NO = SeqNo_max;//กำหนดค่าให้ ds.Leave >> SEQ_NO ใหม่ ให้เป็น ค่าใหม่ที่กำหนด จาก string sql select max(seq_no)+1
                            if (state.SsCoopControl == "031001")
                            {
                                dsLeaveout.DATA[0].LEAVE_CODE = "I";
                            }
                            else
                            {
                                dsLeaveout.DATA[0].LEAVE_CODE = "004";
                            }
                            //dsLeaveout.DATA[0].APV_STATUS = status;
                            //SavePass ตัวดัก Eror
                            ExecuteDataSource exe = new ExecuteDataSource(this);
                            exe.AddFormView(dsLeaveout, ExecuteType.Insert);
                            exe.Execute();
                            setClear();

                            string ls_sql1 = @"insert into hrcheckleave(emp_no,year,month,leave_001_s,leave_001_c,leave_002_s,leave_002_c,leave_004_s,leave_004_c,leave_006_s,leave_006_c,late_date)values({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11})";
                            ls_sql1 = WebUtil.SQLFormat(ls_sql1, EmpNo, year, month,'0','0','0','0','0','0','0','0','0');
                            exed.SQL.Add(ls_sql1);
                            exed.Execute();
                            exed.SQL.Clear();

                            string sqlleave_004 = @"update hrcheckleave set leave_004_s = {0} , leave_004_c = {1} where emp_no={2} and year = {3} and month = {4}";
                            sqlleave_004 = WebUtil.SQLFormat(sqlleave_004, Convert.ToString(totaltime), '1', EmpNo, year, month);
                            Sdt dt_leave_004 = WebUtil.QuerySdt(sqlleave_004);

                            LtServerMessage.Text = WebUtil.CompleteMessage(EmpNo + " " + fullname + " บันทึกสำเร็จ");
                            dsLeaveout.Visible = false;
                            dsDetail.Visible = false;
                        }
                        catch (Exception ex)
                        {
                            LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถบันทึกได้" + ex.Message);
                        }
                    }
                    else
                    {
                        //LtServerMessage.Text = WebUtil.WarningMessage("ลาเกิน 3.5 ชม");
                        try
                        {
                            decimal SeqNo = 1;
                            decimal SeqNo_max = 0;
                            sql = @"select max(seq_no) as seq_no from hrlogleave where emp_no ={0} and coop_id={1}";
                            sql = WebUtil.SQLFormat(sql, EmpNo, coop_id);
                            dt = WebUtil.QuerySdt(sql);
                            if (dt.Next())
                            {
                                SeqNo = dt.GetDecimal("seq_no");
                                SeqNo_max = SeqNo + 1;
                            }
                            dsLeaveout.DATA[0].SEQ_NO = SeqNo_max;
                            if (state.SsCoopControl == "031001")
                            {
                                dsLeaveout.DATA[0].LEAVE_CODE = "I";
                            }
                            else
                            {
                                dsLeaveout.DATA[0].LEAVE_CODE = "001";
                            }
                            //dsLeaveout.DATA[0].APV_STATUS = status;
                            dsLeaveout.DATA[0].TOTALDAY = Convert.ToDecimal(0.5);
                            dsLeaveout.DATA[0].TOTALTIME = 0;
                            ExecuteDataSource exe = new ExecuteDataSource(this);
                            exe.AddFormView(dsLeaveout, ExecuteType.Insert);
                            exe.Execute();
                            setClear();


                            string ls_sql001 = @"insert into hrcheckleave(emp_no,year,month,leave_001_s,leave_001_c,leave_002_s,leave_002_c,leave_004_s,leave_004_c,leave_006_s,leave_006_c,late_date)values({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11})";
                            ls_sql001 = WebUtil.SQLFormat(ls_sql001, EmpNo, year, month, '0', '0', '0', '0', '0', '0', '0', '0', '0');
                            exed.SQL.Add(ls_sql001);
                            exed.Execute();
                            exed.SQL.Clear();

                            string sqlleave_004 = @"update hrcheckleave set leave_001_s = {0} , leave_001_c = {1} where emp_no={2} and year = {3} and month = {4}";
                            sqlleave_004 = WebUtil.SQLFormat(sqlleave_004, Convert.ToString(0.5), '1', EmpNo, year, month);
                            Sdt dt_leave_004 = WebUtil.QuerySdt(sqlleave_004);


                            LtServerMessage.Text = WebUtil.CompleteMessage(EmpNo + " " + fullname + " บันทึกป่วยครึ่งวันสำเร็จ");
                            dsLeaveout.Visible = false;
                            dsDetail.Visible = false;
                        }
                        catch (Exception ex)
                        {
                            LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถบันทึกได้" + ex.Message);
                        }
                    }

                }
                else
                {

                    fullname = dsMain.DATA[0].FULLNAME;
                    //string seqno = Convert.ToString(dsDetail.DATA[0].SEQ_NO);
                    if (dsLeaveout.DATA[0].SEQ_NO.ToString() != "0")
                    {
                        if (totaltime <= min)
                        {
                            try
                            {


                                string sql_leave = @"select totaltime as totaltime , operate_date from hrlogleave where emp_no = {0} and seq_no = {1}";
                                    sql_leave = WebUtil.SQLFormat(sql_leave, EmpNo, dsLeaveout.DATA[0].SEQ_NO.ToString());
                                    // Sdt dt = WebUtil.QuerySdt(sql);
                                    Sdt dt_leave = WebUtil.QuerySdt(sql_leave);
                                    if (dt_leave.Next())
                                    {
                                        sum_totaltime = dt_leave.GetDecimal("totaltime");
                                        operate_date = dt_leave.GetDate("operate_date");
                                        month_edit_d = operate_date.Month;

                                        if (Convert.ToString(month_edit_d) == "1" || Convert.ToString(month_edit_d) == "2" || Convert.ToString(month_edit_d) == "3" || Convert.ToString(month_edit_d) == "4" || Convert.ToString(month_edit_d) == "5" || Convert.ToString(month_edit_d) == "6" || Convert.ToString(month_edit_d) == "7" || Convert.ToString(month_edit_d) == "8" || Convert.ToString(month_edit_d) == "9")
                                        {

                                            month_edit = "0" + Convert.ToString(month_edit_d);
                                        }
                                        else
                                        {
                                            month_edit = Convert.ToString(month_edit_d);
                                        }

                                        year_edit = operate_date.Year;
                                    }

                                    string sql_hrcheckedit = @"select leave_004_s,leave_004_c from hrcheckleave where emp_no = {0} and year = {1} and month = {2}";
                                    sql_hrcheckedit = WebUtil.SQLFormat(sql_hrcheckedit, EmpNo, year_edit, month_edit);
                                    // Sdt dt = WebUtil.QuerySdt(sql);
                                    Sdt dt_edit = WebUtil.QuerySdt(sql_hrcheckedit);
                                    if (dt_edit.Next())
                                    {
                                        leave_004_edit_s = dt_edit.GetString("leave_004_s");
                                        leave_004_edit_c = dt_edit.GetString("leave_004_c");
                                    }

                                    original_leave_004_s = Convert.ToDecimal(leave_004_edit_s) - sum_totaltime;
                                    original_leave_004_c = Convert.ToDecimal(leave_004_edit_c) - 1;

                                    string sqledit_004 = @"update hrcheckleave set leave_004_s = {0} , leave_004_c = {1}  where emp_no={2} and year = {3} and month = {4}";
                                    sqledit_004 = WebUtil.SQLFormat(sqledit_004, Convert.ToString(original_leave_004_s), Convert.ToString(original_leave_004_c), EmpNo, year, month);
                                    Sdt dt_edit_004 = WebUtil.QuerySdt(sqledit_004);

                                string sql2 = "select * from hrlogleave where coop_id = {0} and emp_no = {1}";
                                sql2 = WebUtil.SQLFormat(sql2, state.SsCoopControl, dsLeaveout.DATA[0].EMP_NO.Trim());
                                Sdt dt2 = WebUtil.QuerySdt(sql);
                                dsLeaveout.DATA[0].COOP_ID = coop_id;
                                if (state.SsCoopControl == "031001")
                                {
                                    dsLeaveout.DATA[0].LEAVE_CODE = "I";
                                }
                                else
                                {
                                    dsLeaveout.DATA[0].LEAVE_CODE = "004";
                                }
                                ExecuteDataSource exe = new ExecuteDataSource(this);
                                exe.AddFormView(dsLeaveout, ExecuteType.Update);
                                exe.Execute();
                                exe.SQL.Clear();

                                    string sql_hrcheckleave = @"select emp_no,leave_004_s,leave_004_c from hrcheckleave where emp_no = {0} and year = {1} and month = {2}";
                                    sql_hrcheckleave = WebUtil.SQLFormat(sql_hrcheckleave, EmpNo, year, month);
                                    // Sdt dt = WebUtil.QuerySdt(sql);
                                    Sdt dt_check = WebUtil.QuerySdt(sql_hrcheckleave);
                                    if (dt_check.Next())
                                    {
                                        emp_no_check = dt_check.GetString("emp_no");
                                        leave_004_s = dt_check.GetString("leave_004_s");
                                        leave_004_c = dt_check.GetString("leave_004_c");

                                    }

                                    if (emp_no_check == "")
                                    {

                                        string ls_sql_check = @"insert into hrcheckleave(emp_no,year,month,leave_001_s,leave_001_c,leave_002_s,leave_002_c,leave_004_s,leave_004_c,leave_006_s,leave_006_c,late_date)values({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11})";
                                        ls_sql_check = WebUtil.SQLFormat(ls_sql_check, EmpNo, year, month, '0', '0', '0', '0', '0', '0', '0', '0', '0');
                                        //ExecuteDataSource exe = new ExecuteDataSource(this);
                                        exed.SQL.Add(ls_sql_check);
                                        exed.Execute();
                                        exed.SQL.Clear();

                                        sumday_leave004 = Convert.ToDecimal(totaltime) + 0;
                                        count_leave004 = 1;

                                        string sqlleave_004 = @"update hrcheckleave set leave_004_s = {0} , leave_004_c = {1} where emp_no={2} and year = {3} and month = {4}";
                                        sqlleave_004 = WebUtil.SQLFormat(sqlleave_004, Convert.ToString(sumday_leave004), Convert.ToString(count_leave004), EmpNo, year, month);
                                        Sdt dt_leave_004 = WebUtil.QuerySdt(sqlleave_004);

                                    }
                                    else
                                    {
                                        sumday_leave004 = Convert.ToDecimal(totaltime) + Convert.ToDecimal(leave_004_s);
                                        count_leave004 = Convert.ToDecimal(leave_004_c) + 1;

                                        string sqlleave_004 = @"update hrcheckleave set leave_004_s = {0} , leave_004_c = {1} where emp_no={2} and year = {3} and month = {4}";
                                        sqlleave_004 = WebUtil.SQLFormat(sqlleave_004, Convert.ToString(sumday_leave004), Convert.ToString(count_leave004), EmpNo, year, month);
                                        Sdt dt_leave_004 = WebUtil.QuerySdt(sqlleave_004);
                                    }

                                LtServerMessage.Text = WebUtil.CompleteMessage("แก้ไขข้อมูลสำเร็จ");
                                setClear();
                                dsLeaveout.Visible = false;
                                dsDetail.Visible = false;
                            }
                            catch (Exception ex)
                            {
                                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถบันทึกได้" + ex.Message);
                            }
                        }
                        else
                        {
                            //LtServerMessage.Text = WebUtil.WarningMessage("ลาเกิน 3.5 ชม");
                            try
                            {



                                string sql_leave = @"select totaltime as totaltime , operate_date from hrlogleave where emp_no = {0} and seq_no = {1}";
                                    sql_leave = WebUtil.SQLFormat(sql_leave, EmpNo, dsLeaveout.DATA[0].SEQ_NO.ToString());
                                    // Sdt dt = WebUtil.QuerySdt(sql);
                                    Sdt dt_leave = WebUtil.QuerySdt(sql_leave);
                                    if (dt_leave.Next())
                                    {
                                        sum_totaltime = dt_leave.GetDecimal("totaltime");
                                        operate_date = dt_leave.GetDate("operate_date");
                                        month_edit_d = operate_date.Month;

                                        if (Convert.ToString(month_edit_d) == "1" || Convert.ToString(month_edit_d) == "2" || Convert.ToString(month_edit_d) == "3" || Convert.ToString(month_edit_d) == "4" || Convert.ToString(month_edit_d) == "5" || Convert.ToString(month_edit_d) == "6" || Convert.ToString(month_edit_d) == "7" || Convert.ToString(month_edit_d) == "8" || Convert.ToString(month_edit_d) == "9")
                                        {

                                            month_edit = "0" + Convert.ToString(month_edit_d);
                                        }
                                        else
                                        {
                                            month_edit = Convert.ToString(month_edit_d);
                                        }

                                        year_edit = operate_date.Year;
                                    }

                                    string sql_hrcheckedit = @"select leave_001_s,leave_001_c from hrcheckleave where emp_no = {0} and year = {1} and month = {2}";
                                    sql_hrcheckedit = WebUtil.SQLFormat(sql_hrcheckedit, EmpNo, year_edit, month_edit);
                                    // Sdt dt = WebUtil.QuerySdt(sql);
                                    Sdt dt_edit = WebUtil.QuerySdt(sql_hrcheckedit);
                                    if (dt_edit.Next())
                                    {
                                        leave_001_edit_s = dt_edit.GetString("leave_001_s");
                                        leave_001_edit_c = dt_edit.GetString("leave_001_c");
                                    }

                                    original_leave_001_s = Convert.ToDecimal(leave_001_edit_s) - sum_totaltime;
                                    original_leave_001_c = Convert.ToDecimal(leave_001_edit_c) - 1;

                                    string sqledit_001 = @"update hrcheckleave set leave_001_s = {0} , leave_001_c = {1}  where emp_no={2} and year = {3} and month = {4}";
                                    sqledit_001 = WebUtil.SQLFormat(sqledit_001, Convert.ToString(original_leave_001_s), Convert.ToString(original_leave_001_c), EmpNo, year, month);
                                    Sdt dt_edit_001 = WebUtil.QuerySdt(sqledit_001);

                                
                                string sql2 = "select * from hrlogleave where coop_id = {0} and emp_no = {1}";
                                sql2 = WebUtil.SQLFormat(sql2, state.SsCoopControl, dsLeaveout.DATA[0].EMP_NO.Trim());
                                Sdt dt2 = WebUtil.QuerySdt(sql);
                                dsLeaveout.DATA[0].COOP_ID = coop_id;
                                if (state.SsCoopControl == "031001")
                                {
                                    dsLeaveout.DATA[0].LEAVE_CODE = "I";
                                }
                                else
                                {
                                    dsLeaveout.DATA[0].LEAVE_CODE = "001";
                                }
                                dsLeaveout.DATA[0].TOTALDAY = Convert.ToDecimal(0.5);
                                dsLeaveout.DATA[0].TOTALTIME = 0;
                                ExecuteDataSource exe = new ExecuteDataSource(this);
                                exe.AddFormView(dsLeaveout, ExecuteType.Update);
                                exe.Execute();
                                exe.SQL.Clear();

                                    string sql_hrcheckleave = @"select emp_no,leave_001_s,leave_001_c from hrcheckleave where emp_no = {0} and year = {1} and month = {2}";
                                    sql_hrcheckleave = WebUtil.SQLFormat(sql_hrcheckleave, EmpNo, year, month);
                                    // Sdt dt = WebUtil.QuerySdt(sql);
                                    Sdt dt_check = WebUtil.QuerySdt(sql_hrcheckleave);
                                    if (dt_check.Next())
                                    {
                                        emp_no_check = dt_check.GetString("emp_no");
                                        leave_001_s = dt_check.GetString("leave_001_s");
                                        leave_001_c = dt_check.GetString("leave_001_c");

                                    }

                                    if (emp_no_check == "")
                                    {

                                        string ls_sql_check = @"insert into hrcheckleave(emp_no,year,month,leave_001_s,leave_001_c,leave_002_s,leave_002_c,leave_004_s,leave_004_c,leave_006_s,leave_006_c,late_date)values({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11})";
                                        ls_sql_check = WebUtil.SQLFormat(ls_sql_check, EmpNo, year, month, '0', '0', '0', '0', '0', '0', '0', '0', '0');
                                        //ExecuteDataSource exe = new ExecuteDataSource(this);
                                        exed.SQL.Add(ls_sql_check);
                                        exed.Execute();
                                        exed.SQL.Clear();

                                        sumday_leave001 = Convert.ToDecimal(totaltime) + 0;
                                        count_leave001 = 1;

                                        string sqlleave_001 = @"update hrcheckleave set leave_001_s = {0} , leave_001_c = {1} where emp_no={2} and year = {3} and month = {4}";
                                        sqlleave_001 = WebUtil.SQLFormat(sqlleave_001, Convert.ToString(sumday_leave001), Convert.ToString(count_leave001), EmpNo, year, month);
                                        Sdt dt_leave_001 = WebUtil.QuerySdt(sqlleave_001);

                                    }
                                    else
                                    {
                                        sumday_leave001 = Convert.ToDecimal(totaltime) + Convert.ToDecimal(leave_001_s);
                                        count_leave001 = Convert.ToDecimal(leave_001_c) + 1;

                                        string sqlleave_001 = @"update hrcheckleave set leave_004_s = {0} , leave_004_c = {1} where emp_no={2} and year = {3} and month = {4}";
                                        sqlleave_001 = WebUtil.SQLFormat(sqlleave_001, Convert.ToString(sumday_leave001), Convert.ToString(count_leave001), EmpNo, year, month);
                                        Sdt dt_leave_001 = WebUtil.QuerySdt(sqlleave_001);
                                    }
                                LtServerMessage.Text = WebUtil.CompleteMessage("แก้ไขข้อมูลสำเร็จ");
                                setClear();
                                dsLeaveout.Visible = false;
                                dsDetail.Visible = false;
                            }
                            catch (Exception ex)
                            {
                                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถบันทึกได้" + ex.Message);
                            }
                        }
                    }
                    else //if (dsLeave.DATA[0].SEQ_NO.ToString() == SeqNo.ToString())
                    {
                        string CoopId = state.SsCoopId;//unique
                        decimal SeqNo = 1;//unique
                        if (totaltime <= min)
                        {
                        try
                        {
                            SeqNo = 1;//unique
                            decimal SeqNo_max = 0;
                            sql = @"select max(seq_no) as seq_no from hrlogleave where emp_no ={0} and coop_id={1}";//นำค่าแมกของ seq_no บวก 1 เพื่อให้เป็นค่า seq_no ของลำดับต่อไป
                            sql = WebUtil.SQLFormat(sql, EmpNo, coop_id);//format ในรูปของ sql
                            dt = WebUtil.QuerySdt(sql);
                            if (dt.Next())
                            {
                                SeqNo = dt.GetDecimal("seq_no");
                                SeqNo_max = SeqNo + 1;
                            }
                            dsLeaveout.DATA[0].SEQ_NO = SeqNo_max;//กำหนดค่าให้ ds.Leave >> SEQ_NO ใหม่ ให้เป็น ค่าใหม่ที่กำหนด จาก string sql select max(seq_no)+1
                            if (state.SsCoopControl == "031001")
                            {
                                dsLeaveout.DATA[0].LEAVE_CODE = "I";
                            }
                            else
                            {
                                dsLeaveout.DATA[0].LEAVE_CODE = "004";
                            }
                            //dsLeaveout.DATA[0].APV_STATUS = status;
                            //SavePass ตัวดัก Eror
                            ExecuteDataSource exe = new ExecuteDataSource(this);
                            exe.AddFormView(dsLeaveout, ExecuteType.Insert);
                            exe.Execute();
                            setClear();

                            string sql_hrcheckleave = @"select emp_no,leave_004_s,leave_004_c from hrcheckleave where emp_no = {0} and year = {1} and month = {2}";
                            sql_hrcheckleave = WebUtil.SQLFormat(sql_hrcheckleave, EmpNo, year , month);
                           // Sdt dt = WebUtil.QuerySdt(sql);
                            Sdt dt_check = WebUtil.QuerySdt(sql_hrcheckleave);
                            if (dt_check.Next())
                            {
                                emp_no_check = dt_check.GetString("emp_no");
                                leave_004_s = dt_check.GetString("leave_004_s");
                                leave_004_c = dt_check.GetString("leave_004_c");
 
                            }

                            if (leave_004_c == "")
                            {
                                leave_004_c = "0";
                            }
                            else
                            {

                                leave_004_c = leave_004_c;
                            }

                            if (emp_no_check == "")
                            {

                                string ls_sql_check = @"insert into hrcheckleave(emp_no,year,month,leave_001_s,leave_001_c,leave_002_s,leave_002_c,leave_004_s,leave_004_c,leave_006_s,leave_006_c,late_date)values({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11})";
                                ls_sql_check = WebUtil.SQLFormat(ls_sql_check, EmpNo, year, month, '0', '0', '0', '0', '0', '0', '0', '0', '0');
                                exed.SQL.Add(ls_sql_check);
                                exed.Execute();
                                exed.SQL.Clear();

                                sumday_leave004 = Convert.ToDecimal(totaltime) + 0;
                                count_leave004 =  1;

                                string sqlleave_004 = @"update hrcheckleave set leave_004_s = {0} , leave_004_c = {1} where emp_no={2} and year = {3} and month = {4}";
                                sqlleave_004 = WebUtil.SQLFormat(sqlleave_004, Convert.ToString(sumday_leave004), Convert.ToString(count_leave004), EmpNo, year, month);
                                Sdt dt_leave_004 = WebUtil.QuerySdt(sqlleave_004);

                            }
                            else
                            {
                                sumday_leave004 = Convert.ToDecimal(totaltime) + Convert.ToDecimal(leave_004_s);
                                count_leave004 = Convert.ToDecimal(leave_004_c) + 1;

                                string sqlleave_004 = @"update hrcheckleave set leave_004_s = {0} , leave_004_c = {1} where emp_no={2} and year = {3} and month = {4}";
                                sqlleave_004 = WebUtil.SQLFormat(sqlleave_004, Convert.ToString(sumday_leave004), Convert.ToString(count_leave004), EmpNo, year, month);
                                Sdt dt_leave_004 = WebUtil.QuerySdt(sqlleave_004);
                            }

                            LtServerMessage.Text = WebUtil.CompleteMessage(EmpNo + " " + fullname + " บันทึกสำเร็จ");
                            dsLeaveout.Visible = false;
                            dsDetail.Visible = false;
                        }
                        catch (Exception ex)
                        {
                            LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถบันทึกได้" + ex.Message);
                        }
                    }
                    else
                    {
                        //LtServerMessage.Text = WebUtil.WarningMessage("ลาเกิน 3.5 ชม");
                        try
                        {
                            SeqNo = 1;
                            decimal SeqNo_max = 0;
                            sql = @"select max(seq_no)+1 as seq_no from hrlogleave where emp_no ={0} and coop_id={1}";
                            sql = WebUtil.SQLFormat(sql, EmpNo, coop_id);
                            dt = WebUtil.QuerySdt(sql);
                            if (dt.Next())
                            {
                                SeqNo = dt.GetDecimal("seq_no");
                                SeqNo_max = SeqNo + 1;
                            }
                            dsLeaveout.DATA[0].SEQ_NO = SeqNo_max;
                            if (state.SsCoopControl == "031001")
                            {
                                dsLeaveout.DATA[0].LEAVE_CODE = "I";
                            }
                            else
                            {
                                dsLeaveout.DATA[0].LEAVE_CODE = "002";
                            }
                            //dsLeaveout.DATA[0].APV_STATUS = status;
                            dsLeaveout.DATA[0].TOTALDAY = 1;
                            dsLeaveout.DATA[0].TOTALTIME = 0;
                            ExecuteDataSource exe = new ExecuteDataSource(this);
                            exe.AddFormView(dsLeaveout, ExecuteType.Insert);
                            exe.Execute();
                            setClear();


                            string sql_hrcheckleave002 = @"select emp_no,leave_002_s,leave_002_c from hrcheckleave where emp_no = {0} and year = {1} and month = {2}";
                            sql_hrcheckleave002 = WebUtil.SQLFormat(sql_hrcheckleave002, EmpNo, year, month);
                            // Sdt dt = WebUtil.QuerySdt(sql);
                            Sdt dt_check002 = WebUtil.QuerySdt(sql_hrcheckleave002);
                            if (dt_check002.Next())
                            {
                                emp_no_check_002 = dt_check002.GetString("emp_no");
                                leave_002_s = dt_check002.GetString("leave_002_s");
                                leave_002_c = dt_check002.GetString("leave_002_c");

                            }

                            if (leave_002_c == "")
                            {
                                leave_002_c = "0";
                            }
                            else
                            {

                                leave_002_c = leave_002_c;
                            }

                            if (emp_no_check_002 == "")
                            {

                                string ls_sql_check = @"insert into hrcheckleave(emp_no,year,month,leave_001_s,leave_001_c,leave_002_s,leave_002_c,leave_004_s,leave_004_c,leave_006_s,leave_006_c,late_date)values({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11})";
                                ls_sql_check = WebUtil.SQLFormat(ls_sql_check, EmpNo, year, month, '0', '0', '0', '0', '0', '0', '0', '0', '0');
                                exed.SQL.Add(ls_sql_check);
                                exed.Execute();
                                exed.SQL.Clear();

                                sumday_leave002 = 1 + 0;
                                count_leave002 = Convert.ToDecimal(leave_002_c) + 1;

                                string sqlleave_004 = @"update hrcheckleave set leave_002_s = {0} , leave_002_c = {1} where emp_no={2} and year = {3} and month = {4}";
                                sqlleave_004 = WebUtil.SQLFormat(sqlleave_004, Convert.ToString(sumday_leave002), Convert.ToString(count_leave002), EmpNo, year, month);
                                Sdt dt_leave_004 = WebUtil.QuerySdt(sqlleave_004);

                            }
                            else
                            {
                                sumday_leave002 = 1 + Convert.ToDecimal(leave_002_s);
                                count_leave002 = Convert.ToDecimal(leave_002_c) + 1;

                                string sqlleave_004 = @"update hrcheckleave set leave_002_s = {0} , leave_002_c = {1} where emp_no={2} and year = {3} and month = {4}";
                                sqlleave_004 = WebUtil.SQLFormat(sqlleave_004, Convert.ToString(sumday_leave002), Convert.ToString(count_leave002), EmpNo, year, month);
                                Sdt dt_leave_004 = WebUtil.QuerySdt(sqlleave_004);
                            }

                            LtServerMessage.Text = WebUtil.CompleteMessage(EmpNo + " " + fullname + " บันทึกลากิจสำเร็จ");
                            dsLeaveout.Visible = false;
                            dsDetail.Visible = false;
                        }
                        catch (Exception ex)
                        {
                            LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถบันทึกได้" + ex.Message);
                        }
                    }
                    }
                }
                dsDetail.RetrieveDetail(EmpNo);
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }

        }

        //public decimal apv_status()
        //{
        //    decimal apv = 0;
        //    apv = dsLeaveout.DATA[0].APV_STATUS;
        //    if (apv == 0)
        //        apv = 1;
        //    else if (apv == 1)
        //        apv = -9;
        //    return apv;
        //}

        public void setClear()
        {
            dsMain.ResetRow();
            dsLeaveout.ResetRow();
            dsDetail.ResetRow();
        }
        public void WebSheetLoadEnd()
        {

        }
    }
}