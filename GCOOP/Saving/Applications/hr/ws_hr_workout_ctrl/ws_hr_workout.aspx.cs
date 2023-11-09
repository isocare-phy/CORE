using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OracleClient;
using CoreSavingLibrary;
using DataLibrary;

namespace Saving.Applications.hr.ws_hr_workout_ctrl
{
    public partial class ws_hr_workout : PageWebSheet, WebSheet
    {
        [JsPostBack]
        public string PostEmpNo { get; set; }

        [JsPostBack]
        public string PostRemark { get; set; }

        [JsPostBack]
        public string PostLast { get; set; }

        [JsPostBack]
        public string PostEmpOT { get; set; }

        public void InitJsPostBack()
        {
            dsMain.InitMain(this);
            dsList.InitDsList(this);
            dsLog.InitDsList(this);
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                dsList.Visible = false;
                dsLog.Visible = false;
            }
        }
        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "PostEmpNo")
            {
                DateTime standard_time = new DateTime();
                DateTime time_now = DateTime.Now;
                string EmpNo = dsMain.DATA[0].EMP_NO;
                dsMain.RetrieveEmp(EmpNo);
                dsList.DATA[0].DATE_WORK = time_now;
                dsList.RetrieveHr_Ot();
                //dsList.DATA[0].REMARK = dsList.RetrieveHr_Ot();


                string sql = @"select end_time from hrlogworktime where coop_id = {0} and emp_no = {1} and work_date = to_date({2},'dd/mm/yyyy')";
                sql = WebUtil.SQLFormat(sql, state.SsCoopId, EmpNo, time_now);
                Sdt dt = WebUtil.QuerySdt(sql);
                if (dt.Next())
                {

                    DateTime end_time  = dt.GetDate("end_time");
                    string end_time_last = Convert.ToDateTime(end_time).ToString("HH.mm");
                    //decimal start_time_ot = 0;
                   // start_time_ot = Convert.ToDecimal(end_time);

                    dsList.DATA[0].WORK_OUT = end_time_last;


                }

                dsList.Visible = true;
                dsLog.Visible = true;
                dsLog.RetrieveHrLog(EmpNo);

            }
            else if (eventArg == PostRemark)
            {

                if (dsList.DATA[0].REMARK == "002")
                {

                    dsList.DATA[0].WORK_IN = "17.00";

                }
                else if (dsList.DATA[0].REMARK == "003")
                {

                    dsList.DATA[0].WORK_IN = "08.20";

                }
                else if (dsList.DATA[0].REMARK == "004")
                {
                    dsList.DATA[0].WORK_IN = "17.00";
                }
            }

            else if (eventArg == PostLast)
            {
                string stime = dsList.DATA[0].WORK_IN;
                string ltime = dsList.DATA[0].WORK_OUT;
                decimal hours = 0, min = 0;

                    try
                {
                      
                    decimal statime = Convert.ToDecimal(stime);
                    decimal entime = Convert.ToDecimal(ltime);
                  
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
                        dsList.DATA[0].OT_P = all;

                       
                        }
                    catch (Exception ex)
                        {
                    LtServerMessage.Text = WebUtil.WarningMessage(ex.ToString());
                        }
            }
            else if (eventArg == PostEmpOT)
            {
                dsList.Visible = true;
                int row = dsLog.GetRowFocus();
                string Seqno = Convert.ToString(dsLog.DATA[row].SEQ_NO);
                string EmpNo = dsMain.DATA[0].EMP_NO;
                decimal apv_ot_status = 8;
                DateTime date_work = new DateTime();

                string sql = @"select date_work
                               from hrw_ot 
                               where apv_ot_status = {0} 
                               and emp_no = {1} 
                               and seq_no = {2}
                               "
                    ;

                sql = WebUtil.SQLFormat(sql, apv_ot_status, EmpNo, Seqno);//format ในรูปของ sql
                Sdt dt = WebUtil.QuerySdt(sql);
                dt = WebUtil.QuerySdt(sql);
                if (dt.Next())
                {
                    date_work = dt.GetDate("date_work");

                }

                dsList.Retrieveot(Seqno, EmpNo);
                dsLog.Visible = true;
                dsList.DATA[0].DATE_WORK = date_work;
                dsList.RetrieveHr_Ot();
            }
           
        }
        

        public void NewClear()
        {
            dsMain.ResetRow();
            dsList.ResetRow();

            //dsList.RetrieveHrResign();

        }

        public void SaveWebSheet()
        {

            decimal seq_no = 0;
            decimal max_seq_no = 0;
            decimal apv_ot_status = 8;
            string ls_coopid = state.SsCoopControl;
            //int row_check = dsLog.GetRowFocus();
            //string Seqno_check = Convert.ToString(dsLog.DATA[row_check].SEQ_NO);
            
            string sql = @"select 
                                max(seq_no) as seq_no
                            from
                                hrw_ot
                            where
                                emp_no={0}
                                ";
            sql = WebUtil.SQLFormat(sql, dsMain.DATA[0].EMP_NO);
            Sdt dt = WebUtil.QuerySdt(sql);

            if (dt.Next())
            {

                seq_no = dt.GetDecimal("seq_no");
                max_seq_no = seq_no + 1;

            }
            else
            {
                max_seq_no = 1;
            }


            string emp_no_check = "";

            string sql2 = @"select 
                                emp_no
                            from
                                hrw_ot
                            where
                                emp_no={0} and to_date(date_work,'dd/mm/yyyy') = to_date({1},'dd/mm/yyyy')
                                ";

            sql2 = WebUtil.SQLFormat(sql2, dsMain.DATA[0].EMP_NO, dsList.DATA[0].DATE_WORK);
            Sdt dt2 = WebUtil.QuerySdt(sql2);

            if (dt2.Next())
            {
                emp_no_check = dt2.GetString("emp_no");
            }


            ExecuteDataSource exes = new ExecuteDataSource(this);
            string insert_ot_log = @"insert into hrw_ot(coop_id,
                    emp_no,   
                    seq_no,   
                    remark,   
                    date_work,work_in,work_out,ot_p,apv_ot_status,description)values({0},{1},{2},{3},{4},{5},{6},{7},{8},{9})";

            try
            {

                if (dsList.DATA[0].SEQ_NO == 0)
                {
			if (dsList.DATA[0].OT_P >= 2)
                    {
						if(emp_no_check == "" || emp_no_check == null){
						
                        object[] argInsert = new object[] { ls_coopid, dsMain.DATA[0].EMP_NO, max_seq_no, dsList.DATA[0].REMARK, dsList.DATA[0].DATE_WORK, dsList.DATA[0].WORK_IN, dsList.DATA[0].WORK_OUT, dsList.DATA[0].OT_P, apv_ot_status, dsList.DATA[0].DESCRIPTION };
                        insert_ot_log = WebUtil.SQLFormat(insert_ot_log, argInsert);
                        exes.SQL.Add(insert_ot_log);
                        exes.Execute();
                        LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกการทำงานล่วงเวลาสำเร็จ");
						}else{
							
							LtServerMessage.Text = WebUtil.ErrorMessage("ท่านได้มีการขอทำงานล่วงเวลาของวันนี้ไปเเล้ว");
						}
                    }
                    else
                    {
						
							
							LtServerMessage.Text = WebUtil.ErrorMessage("จำนวนชั่วโมงที่ทำไม่เพียงพอในการทำงานล่วงเวลา");
						
						
                        
                    }
                }
                else
                {
                    //LtServerMessage.Text = WebUtil.ErrorMessage("มีการบันทึกการทำงานล่วงเวลาของวันนี้ไปเเล้ว");
                     string emp_no_check_update = "";
                   

                    string sql4 = @"select 
                                emp_no
                            from
                                hrw_ot
                            where
                                emp_no={0} and to_date(date_work,'dd/mm/yyyy') = to_date({1},'dd/mm/yyyy') and seq_no != {2}
                                ";

                    sql4 = WebUtil.SQLFormat(sql4, dsMain.DATA[0].EMP_NO, dsList.DATA[0].DATE_WORK, dsList.DATA[0].SEQ_NO
                        );
                    Sdt dt4 = WebUtil.QuerySdt(sql4);

                    if (dt4.Next())
                    {
                        emp_no_check_update = dt4.GetString("emp_no");
                    }

                    if (emp_no_check_update == "" || emp_no_check_update == null)
                    {

                    string update_ot_log = @"update hrw_ot set remark={0} , date_work={1} , work_in={2} , work_out={3} , ot_p={4} , description={5} where emp_no={6} and seq_no={7}";
                    update_ot_log = WebUtil.SQLFormat(update_ot_log, dsList.DATA[0].REMARK, dsList.DATA[0].DATE_WORK, dsList.DATA[0].WORK_IN, dsList.DATA[0].WORK_OUT, dsList.DATA[0].OT_P, dsList.DATA[0].DESCRIPTION, dsMain.DATA[0].EMP_NO, dsList.DATA[0].SEQ_NO);
                    dt = WebUtil.QuerySdt(update_ot_log);

                    LtServerMessage.Text = WebUtil.CompleteMessage("แก้ไขการทำงานล่วงเวลาสำเร็จ");
					}
					else{
						
						LtServerMessage.Text = WebUtil.ErrorMessage("ท่านได้มีการขอทำงานล่วงเวลาของวันนี้ไปเเล้ว");
					}

                }
            }
                 
            catch (OracleException or)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(or.ToString());
            }
            finally
            {
                exes.SQL.Clear();
                
            }

        }

        public void WebSheetLoadEnd()
        {

        }
    }
}