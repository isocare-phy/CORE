using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using System.Data;
using DataLibrary;

namespace Saving.Applications.hr.ws_hr_cure_newfamily_ctrl
{
    public partial class ws_hr_cure_newfamily : PageWebSheet, WebSheet
    {
        [JsPostBack]
        public string PostEmpNo { get; set; }

        [JsPostBack]
        public String PostEmpLeave { get; set; }

        [JsPostBack]
        public String PostDelete { get; set; }

        decimal assist_halfamt = 0;
        string Seqno = "";
        decimal seq_no_check = 0;

        public void InitJsPostBack()
        {
            dsCure.InitDs(this);
            dsMain.InitDs(this);
            dsDetail.InitDsDetail(this);
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                dsCure.DATA[0].COOP_ID = state.SsCoopId;//Data[0] คือ กำหนดข้อมูลที่ฟิวไหน กรณีรูปแบบของ FormView
                dsCure.Visible = false;
                dsDetail.Visible = false;
            }

        }

        public void CheckJsPostBack(string eventArg)
        {
            //string stime = dsCure.DATA[0].START_TIME;
            //string ltime = dsCure.DATA[0].LAST_TIME;
            decimal hours = 0, min = 0;

            if (eventArg == PostEmpNo)
            {
                dsCure.Visible = true;
                string EmpNo = dsMain.DATA[0].EMP_NO;
                dsMain.RetrieveEmp(EmpNo);
                dsCure.DATA[0].EMP_NO = EmpNo;//นำข้อมูล emp_no จากที่ RetrieveEmp
                dsDetail.Visible = true;
                dsDetail.RetrieveDetail(EmpNo);


                string sql = "select sum(assist_minamt) as assist_minamt from hremployeeassist where coop_id = {0} and emp_no = {1} and assist_code = '01'";
                //string assist_detail = "";
                //string row = Convert.ToString(dsDetail.DATA[0].SEQ_NO);
                sql = WebUtil.SQLFormat(sql, state.SsCoopControl, dsMain.DATA[0].EMP_NO.Trim());
                Sdt dt = WebUtil.QuerySdt(sql);

                if (dt.Next())
                {
                    assist_halfamt = dt.GetDecimal("assist_minamt");
                }

                dsCure.DATA[0].ASSIST_HALFAMT = assist_halfamt;

            }
            else if (eventArg == PostEmpLeave)
            {
               /* dsCure.Visible = true;
                int row = dsDetail.GetRowFocus();
                string Seqno = Convert.ToString(dsDetail.DATA[row].SEQ_NO);
                string EmpNo = dsMain.DATA[0].EMP_NO;
                dsCure.Retrieveleave(Seqno, EmpNo);
                dsDetail.Visible = true;
                dsDetail.RetrieveDetail(EmpNo);*/

                string emp_no = dsMain.DATA[0].EMP_NO;
                dsMain.RetrieveEmp(emp_no);
                dsDetail.RetrieveDetail(emp_no);
                dsMain.DATA[0].EMP_NO = emp_no;
                int row = dsDetail.GetRowFocus();
                Seqno = Convert.ToString(dsDetail.DATA[row].SEQ_NO);
                //decimal SEQ_NO = dsDetail.DATA[0].SEQ_NO;
                dsCure.Retrieveleave(Seqno, emp_no);
               // dsDetail.DdEducation();

                string sql = "select sum(assist_minamt) as assist_minamt from hremployeeassist where coop_id = {0} and emp_no = {1} and assist_code = '01'";
                //string assist_detail = "";
                //string row = Convert.ToString(dsDetail.DATA[0].SEQ_NO);
                sql = WebUtil.SQLFormat(sql, state.SsCoopControl, dsMain.DATA[0].EMP_NO.Trim());
                Sdt dt = WebUtil.QuerySdt(sql);

                if (dt.Next())
                {
                    assist_halfamt = dt.GetDecimal("assist_minamt"); ;
                }

                dsCure.DATA[0].ASSIST_HALFAMT = assist_halfamt;
            }
            else if (eventArg == PostDelete)
            {

                int row = dsDetail.GetRowFocus();
                Seqno = Convert.ToString(dsDetail.DATA[row].SEQ_NO);

                try
                    {


                ExecuteDataSource exe = new ExecuteDataSource(this);
                string ls_sqldel = @"delete from hremployeeassist where coop_id = '" + state.SsCoopControl + "' and emp_no = '" + dsMain.DATA[0].EMP_NO.Trim() + "' and assist_code = '01' and seq_no = '" + Seqno + "'";
                exe.SQL.Add(ls_sqldel);
                exe.Execute();
                exe.SQL.Clear();
                LtServerMessage.Text = WebUtil.CompleteMessage("ลบข้อมูลสำเร็จ");
                setClear();
                dsDetail.Visible = false;
                dsCure.Visible = false;

                     }
                    catch (Exception ex)
                    {

                        LtServerMessage.Text = WebUtil.ErrorMessage(ex);

                    }
                
                

            }
           
        }

        public void SaveWebSheet()
        {
           ExecuteDataSource exe = new ExecuteDataSource(this);

           /* string coop_id = state.SsCoopControl;
            string EmpNo = dsMain.DATA[0].EMP_NO;
            string sql = "select * from hrlogleave where coop_id = {0} and emp_no = {1}";
            string row = dsCure.DATA[0].SEQ_NO.ToString();
            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, dsMain.DATA[0].EMP_NO.Trim());
            Sdt dt = WebUtil.QuerySdt(sql);
            string fullname = dsMain.DATA[0].FULLNAME;
            double min = 2.0;
            //decimal status = 0;
            //double totaltime = Convert.ToDouble(dsCure.DATA[0].TOTALTIME);
            //status = apv_status();//apv_status= 1 ,-9
            bool check = false;
            //check = checkTime(EmpNo);

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
                            dsLeaveout.DATA[0].LEAVE_CODE = "004";
                            //dsLeaveout.DATA[0].APV_STATUS = status;
                            //SavePass ตัวดัก Eror
                            exe.AddFormView(dsLeaveout, ExecuteType.Insert);
                            exe.Execute();
                            setClear();
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
                            dsLeaveout.DATA[0].LEAVE_CODE = "001";
                            //dsLeaveout.DATA[0].APV_STATUS = status;
                            dsLeaveout.DATA[0].TOTALDAY = Convert.ToDecimal(0.5);
                            dsLeaveout.DATA[0].TOTALTIME = 0;

                            exe.AddFormView(dsLeaveout, ExecuteType.Insert);
                            exe.Execute();
                            setClear();
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

                                string sql2 = "select * from hrlogleave where coop_id = {0} and emp_no = {1}";
                                sql2 = WebUtil.SQLFormat(sql2, state.SsCoopControl, dsLeaveout.DATA[0].EMP_NO.Trim());
                                Sdt dt2 = WebUtil.QuerySdt(sql);
                                dsLeaveout.DATA[0].COOP_ID = coop_id;
                                dsLeaveout.DATA[0].LEAVE_CODE = "004";
                                exe.AddFormView(dsLeaveout, ExecuteType.Update);
                                exe.Execute();
                                exe.SQL.Clear();
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

                                string sql2 = "select * from hrlogleave where coop_id = {0} and emp_no = {1}";
                                sql2 = WebUtil.SQLFormat(sql2, state.SsCoopControl, dsLeaveout.DATA[0].EMP_NO.Trim());
                                Sdt dt2 = WebUtil.QuerySdt(sql);
                                dsLeaveout.DATA[0].COOP_ID = coop_id;
                                dsLeaveout.DATA[0].LEAVE_CODE = "001";
                                dsLeaveout.DATA[0].TOTALDAY = Convert.ToDecimal(0.5);
                                dsLeaveout.DATA[0].TOTALTIME = 0;
                                exe.AddFormView(dsLeaveout, ExecuteType.Update);
                                exe.Execute();
                                exe.SQL.Clear();
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
                                dsLeaveout.DATA[0].LEAVE_CODE = "004";
                                //dsLeaveout.DATA[0].APV_STATUS = status;
                                //SavePass ตัวดัก Eror
                                exe.AddFormView(dsLeaveout, ExecuteType.Insert);
                                exe.Execute();
                                setClear();
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
                                dsLeaveout.DATA[0].LEAVE_CODE = "002";
                                //dsLeaveout.DATA[0].APV_STATUS = status;
                                dsLeaveout.DATA[0].TOTALDAY = 1;
                                dsLeaveout.DATA[0].TOTALTIME = 0;
                                exe.AddFormView(dsLeaveout, ExecuteType.Insert);
                                exe.Execute();
                                setClear();
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
            }*/

            decimal seq_no = 0;
            decimal max_seq_no = 0;
            decimal assist_use = 0;
            decimal assist_minamt_s = 0;
            decimal avg = 0;
            decimal maxseqno1 = 0;
            decimal maxseqno2 = 0;
            string ls_coopid = state.SsCoopControl;
            string assist_name = dsCure.DATA[0].ASSIST_NAME;
            string assist_state = dsCure.DATA[0].ASSIST_STATE;
            string assist_hosname = dsCure.DATA[0].ASSIST_HOSNAME;
            string assist_posit = dsCure.DATA[0].ASSIST_POSIT;
            string assist_amp = dsCure.DATA[0].ASSIST_AMP;
            DateTime assist_sdate = dsCure.DATA[0].ASSIST_SDATE;
            decimal assist_amt = dsCure.DATA[0].ASSIST_AMT;
            decimal assist_minamt = dsCure.DATA[0].ASSIST_MINAMT;
            decimal year_d = assist_sdate.Year;
            //string str_assist_sdate = Convert.ToString(year_d);
            string year = Convert.ToString(year_d);



            decimal sum_check = 0;
            decimal moneycure_money = 0;

            string sql = "select * from hremployeeassist where coop_id = {0} and emp_no = {1} and assist_code = '01'";
            //string assist_detail = "";
            string row = Convert.ToString(dsCure.DATA[0].SEQ_NO);
            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, dsMain.DATA[0].EMP_NO.Trim());
            Sdt dt_seq = WebUtil.QuerySdt(sql);
            

            string sql_seqno = @"select 
                                max(seq_no) as seq_no
                            from
                                hremployeeassist
                            where
                                emp_no={0} and assist_code = '01'
                                ";
            sql_seqno = WebUtil.SQLFormat(sql_seqno, dsMain.DATA[0].EMP_NO);
            Sdt dt = WebUtil.QuerySdt(sql_seqno);

            if (dt.Next())
            {
                seq_no = dt.GetDecimal("seq_no");
                max_seq_no = seq_no + 1;
            }
            else
            {
                max_seq_no = 1;
            }

            if (assist_state == "01")
            {

                string sql_hrucfmoneycure01 = @"select 
                                moneycure_money
                            from
                                hrucfmoneycure
                            where
                                moneycure_code = '01'
                                ";
                sql_hrucfmoneycure01 = WebUtil.SQLFormat(sql_hrucfmoneycure01);
                Sdt dt_hrucfmoneycure01 = WebUtil.QuerySdt(sql_hrucfmoneycure01);

                if (dt_hrucfmoneycure01.Next())
                {
                    moneycure_money = dt_hrucfmoneycure01.GetDecimal("moneycure_money");
                }

                string sql_maxseqno_01 = @"select 
                                max(seq_no) as maxseqno_01
                            from 
                                hremployeeassist
                            where
                                emp_no={0} and assist_code = '01' and TO_CHAR(assist_sdate,'yyyy') = {1} and assist_state = '01'
                                ";
                sql_maxseqno_01 = WebUtil.SQLFormat(sql_maxseqno_01, dsMain.DATA[0].EMP_NO, year);
                Sdt dt8 = WebUtil.QuerySdt(sql_maxseqno_01);

                if (dt8.Next())
                {
                    maxseqno1 = dt8.GetDecimal("maxseqno_01");
                }


                string sql1 = @"select 
                                assist_use,assist_minamt
                            from
                                hremployeeassist
                            where
                                emp_no={0} and assist_code = '01' and seq_no = {1} and TO_CHAR(assist_sdate,'yyyy') = {2} and assist_state = '01'
                                ";
                sql1 = WebUtil.SQLFormat(sql1, dsMain.DATA[0].EMP_NO, maxseqno1, year);
                Sdt dt1 = WebUtil.QuerySdt(sql1);

                if (dt1.Next())
                {
                    assist_use = dt1.GetDecimal("assist_use");
                    assist_minamt_s = dt1.GetDecimal("assist_minamt");
                    avg = assist_use - assist_minamt_s;
                    sum_check = avg;
                }
                else
                {
                    sum_check = moneycure_money;
                }
            }
            else
            {

                string sql_hrucfmoneycure02 = @"select 
                                moneycure_money
                            from
                                hrucfmoneycure
                            where
                                moneycure_code = '02'
                                ";
                sql_hrucfmoneycure02 = WebUtil.SQLFormat(sql_hrucfmoneycure02);
                Sdt dt_hrucfmoneycure02 = WebUtil.QuerySdt(sql_hrucfmoneycure02);

                if (dt_hrucfmoneycure02.Next())
                {
                    moneycure_money = dt_hrucfmoneycure02.GetDecimal("moneycure_money");
                }

                string sql_maxseqno_01 = @"select 
                                max(seq_no) as maxseqno_02
                            from 
                                hremployeeassist
                            where
                                emp_no={0} and assist_code = '01' and TO_CHAR(assist_sdate,'yyyy') = {1} and assist_state <> '01'
                                ";
                sql_maxseqno_01 = WebUtil.SQLFormat(sql_maxseqno_01, dsMain.DATA[0].EMP_NO, year);
                Sdt dt_max01 = WebUtil.QuerySdt(sql_maxseqno_01);

                if (dt_max01.Next())
                {
                    maxseqno2 = dt_max01.GetDecimal("maxseqno_02");
                }

                string sql1 = @"select 
                                assist_use,assist_minamt
                            from
                                hremployeeassist
                            where
                                emp_no={0} and assist_code = '01' and seq_no = {1} and TO_CHAR(assist_sdate,'yyyy') = {2} and assist_state <> '01'
                                ";
                sql1 = WebUtil.SQLFormat(sql1, dsMain.DATA[0].EMP_NO, maxseqno2, year);
                Sdt dt1 = WebUtil.QuerySdt(sql1);

                if (dt1.Next())
                {
                    assist_use = dt1.GetDecimal("assist_use");
                    assist_minamt_s = dt1.GetDecimal("assist_minamt");
                    avg = assist_use - assist_minamt_s;
                    sum_check = avg;
                }
                else
                {
                    sum_check = moneycure_money;
                }
            }


           /* string sql_seqno = @"select 
                                seq_no as seq_no_check
                            from
                                hremployeeassist
                            where
                                emp_no={0} and assist_code = '01' and seq_no = {1} and TO_CHAR(assist_sdate,'yyyy') = {2}
                                ";
            sql_seqno = WebUtil.SQLFormat(sql_seqno, dsMain.DATA[0].EMP_NO, Seqno, year);
            Sdt dt_seqno = WebUtil.QuerySdt(sql_seqno);

            if (dt_seqno.Next())
            {
                seq_no_check = dt_seqno.GetDecimal("seq_no");  
            }*/


            if (dsCure.DATA[0].SEQ_NO.ToString() == "0")
            {



                if (sum_check > 0)
                {

                    ExecuteDataSource exes = new ExecuteDataSource(this);
                    string update_ot_log = @"insert into hremployeeassist(coop_id,
                    emp_no,   
                    seq_no,   
                    assist_name,
                    assist_state,assist_hosname,assist_posit,assist_amp,assist_sdate,assist_amt,assist_minamt,assist_code,assist_use)values({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},'01',{11})";

                    try
                    {

                        object[] argInsert = new object[] { ls_coopid, dsMain.DATA[0].EMP_NO, max_seq_no, assist_name, assist_state, assist_hosname, assist_posit, assist_amp, assist_sdate, assist_amt, assist_minamt, sum_check };
                        update_ot_log = WebUtil.SQLFormat(update_ot_log, argInsert);
                        exes.SQL.Add(update_ot_log);
                        exes.Execute();
                        LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกการเบิกค่ารักษาพยาบาลเรียบร้อยเเล้ว");

                    }
                    catch (Exception ex)
                    {

                        LtServerMessage.Text = WebUtil.ErrorMessage(ex);

                    }


                    finally
                    {
                        exes.SQL.Clear();

                    }
                }
                else
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถทำรายการได้ ยอดเบิกค่ารักษาพยาบาลต่อปีของท่านไม่เพียงพอ ");
                }
            }
            else
            {
                try
                    {
               /* String sql_update_seqno = @"update hremployeeassist set    
                    assist_name = {0},
                    assist_state = {1},assist_hosname = {2},assist_posit = {3},assist_amp = {4},assist_sdate = {5},assist_amt = {6},assist_minamt = {7},assist_code = '01' ,assist_use = {8}  where emp_no = {9} and seq_no = {10} assist_code = '01' and seq_no = {11} and TO_CHAR(assist_sdate,'yyyy') = {12} ";
                sql_update_seqno = WebUtil.SQLFormat(sql_update_seqno, assist_name, assist_state, assist_hosname, assist_posit, assist_amp, assist_sdate, assist_amt, assist_minamt, sum_check, dsMain.DATA[0].EMP_NO, Seqno ,year);
                Sdt dt_update_seqno = WebUtil.QuerySdt(sql_update_seqno);*/

                        string sql2 = "select * from hremployeeassist where coop_id = {0} and emp_no = {1} and assist_code = '01'";
                        sql2 = WebUtil.SQLFormat(sql2, state.SsCoopControl, dsDetail.DATA[0].EMP_NO.Trim());
                        Sdt dt2 = WebUtil.QuerySdt(sql);
                        dsCure.DATA[0].COOP_ID = state.SsCoopControl;
                        dsCure.DATA[0].ASSIST_CODE = "01";
                        //dsCure.DATA[0].ASSIST_FCAREER = dsMain.DATA[0].occupation;
                        exe.AddFormView(dsCure, ExecuteType.Update);

                exe.Execute();
                exe.SQL.Clear();
                LtServerMessage.Text = WebUtil.CompleteMessage("แก้ไขเบิกค่ารักษาพยาบาลเรียบร้อยเเล้ว");
                setClear();
                dsDetail.Visible = false;
                dsCure.Visible = false;

                    }
                catch (Exception ex)
                {

                    LtServerMessage.Text = WebUtil.ErrorMessage(ex);

                }

            }

        }

        public void setClear()
        {
            dsMain.ResetRow();
            dsCure.ResetRow();
            dsDetail.ResetRow();
        }
        public void WebSheetLoadEnd()
        {

        }
    }
}