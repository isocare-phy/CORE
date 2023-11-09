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

namespace Saving.Applications.hr.ws_hr_edit_leave_ctrl
{
    public partial class ws_hr_edit_leave : PageWebSheet, WebSheet
    {
        [JsPostBack]
        public string PostEmpNo { get; set; }
        [JsPostBack]
        public string PostEmpLeave { get; set; }

        public void InitJsPostBack()
        {
            dsMain.InitMain(this);
            dsLeave.InitDsLeave(this);
            dsList.InitDsList(this);
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                dsLeave.Visible = false;
                dsList.Visible = false;
            }
            else
            {
                DateTime date_today;
                date_today = CURRENT_TIMESTAMP();
                dsLeave.DATA[0].LEAVE_DATE = date_today;
                dsLeave.Visible = false;
                dsList.Visible = false;
            }
        }

        #region CURRENT_TIMESTAMP
        private DateTime CURRENT_TIMESTAMP()
        {
            DateTime date_today = new DateTime();
            String current_date_sql = @"select  CURRENT_TIMESTAMP from dual";
            string sysdate_sql = @"select to_date(to_char(sysdate,'mm/dd/yyyy'),'mm/dd/yyyy') as sys_today from dual";
            try
            {
                Sdt dt = WebUtil.QuerySdt(sysdate_sql);
                if (dt.Next())
                {
                    date_today = dt.GetDate("sys_today");
                }
            }
            catch (OracleException or)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(or.ToString());
            }

            return date_today;
        }
        #endregion

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "PostEmpNo")
            {
                string EmpNo = dsMain.DATA[0].EMP_NO;
                dsMain.RetrieveEmp(EmpNo);
                dsLeave.Visible = true;
                dsList.Visible = true;
                dsLeave.DdHrucfleavecode();
                dsList.RetrieveList(EmpNo);
            }
            else if (eventArg == "PostEmpLeave")
            {
                string EmpNo = dsMain.DATA[0].EMP_NO;
                dsMain.RetrieveEmp(EmpNo);
                dsLeave.Visible = true;
                dsList.Visible = true;
                int row = dsList.GetRowFocus();
                string Seqno = Convert.ToString(dsList.DATA[row].SEQ_NO);
                dsLeave.Retrieveleave(Seqno,EmpNo);
                dsLeave.DdHrucfleavecode();
                dsList.RetrieveList(EmpNo);
            }
        }

        public void SaveWebSheet()
        {
            ExecuteDataSource exed = new ExecuteDataSource(this);
            string coop_id = state.SsCoopControl;
            string EmpNo = dsMain.DATA[0].EMP_NO;
            string fullname = fullname = dsMain.DATA[0].FULLNAME;
            string sql = "select * from hrlogleave where coop_id = {0} and emp_no = {1}";
            string row = dsLeave.DATA[0].SEQ_NO.ToString();
            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, dsMain.DATA[0].EMP_NO.Trim());
            Sdt dt = WebUtil.QuerySdt(sql);
            try
            {
                    if (dsLeave.DATA[0].SEQ_NO.ToString() != "0")
                    {
                        string sql2 = "select * from hrlogleave where coop_id = {0} and emp_no = {1}";
                        sql2 = WebUtil.SQLFormat(sql2, state.SsCoopControl, dsLeave.DATA[0].EMP_NO.Trim());
                        Sdt dt2 = WebUtil.QuerySdt(sql);
                        dsLeave.DATA[0].COOP_ID = coop_id;
                        dsLeave.DATA[0].APV_STATUS = 8;
                        dsLeave.DATA[0].LEAVE_DATE = dsList.DATA[0].LEAVE_DATE;
                        exed.AddFormView(dsLeave, ExecuteType.Update);
                        exed.Execute();
                        exed.SQL.Clear();
                        LtServerMessage.Text = WebUtil.CompleteMessage("แก้ไขข้อมูลสำเร็จ" + " " + EmpNo + " " + fullname);
                        reset();
                        dsList.RetrieveList(EmpNo);
                    }
                dsList.RetrieveList(EmpNo);
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
//            decimal SeqNo = 0;
//            string CoopId, EmpNo, fullname, leave_cause, leave_code;
//            decimal todtalday, apv_status;
//            DateTime leave_date, leave_from, leave_to;
//            fullname = dsMain.DATA[0].FULLNAME;
//            CoopId = state.SsCoopId;
//            EmpNo = dsMain.DATA[0].EMP_NO;
//            SeqNo = getSeqNo(EmpNo, CoopId);
//            leave_date = dsLeave.DATA[0].LEAVE_DATE;
//            leave_from = dsLeave.DATA[0].LEAVE_FROM;
//            leave_to = dsLeave.DATA[0].LEAVE_TO;
//            todtalday = dsLeave.DATA[0].TOTALDAY;
//            apv_status = 8;
//            leave_code = dsLeave.DATA[0].LEAVE_CODE;
//            leave_cause = dsLeave.DATA[0].LEAVE_CAUSE;
//            ExecuteDataSource exed = new ExecuteDataSource(this);
//            string sql = "select * from hrlogleave where coop_id = {0} and emp_no = {1}";
//            //string row = dsLeave.DATA[0].SEQ_NO.ToString();
//            string insert_leave = @"insert into hrlogleave 
//                                (coop_id,seq_no,emp_no,leave_date,leave_code,leave_from,leave_to,totalday,apv_status,leave_cause) 
//                                values({0},{1},{2},{3},{4},{5},{6},{7},{8},{9})";
//            try
//            {
//                sql = WebUtil.SQLFormat(sql, CoopId, EmpNo.Trim());
//                Sdt dt = WebUtil.QuerySdt(sql);
//                if (dt.Rows.Count <= 0)
//                {
//                    try
//                    {
//                        object[] argUpdate = new object[] { CoopId, SeqNo, EmpNo, leave_date, leave_code, leave_from, leave_to, todtalday, apv_status, leave_cause };
//                        insert_leave = WebUtil.SQLFormat(insert_leave, argUpdate);
//                        exed.SQL.Add(insert_leave);
//                        exed.Execute();
//                    }
//                    catch (OracleException or)
//                    {
//                        LtServerMessage.Text = WebUtil.ErrorMessage(or.ToString());
//                    }
//                    finally
//                    {
//                        exed.SQL.Clear();
//                        reset();
//                        LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกการลาสำเร็จ " + EmpNo + " " + fullname);
//                    }

//                    /*exed.AddFormView(dsLeave, ExecuteType.Insert);
//                    exed.Execute();*/

//                }
//                else
//                {
//                    try
//                    {
//                        object[] argUpdate = new object[] { CoopId, SeqNo, EmpNo, leave_date, leave_code, leave_from, leave_to, todtalday, apv_status, leave_cause };
//                        insert_leave = WebUtil.SQLFormat(insert_leave, argUpdate);
//                        exed.SQL.Add(insert_leave);
//                        exed.Execute();
//                    }
//                    catch (OracleException or)
//                    {
//                        LtServerMessage.Text = WebUtil.ErrorMessage(or.ToString());
//                    }
//                    finally
//                    {
//                        exed.SQL.Clear();
//                        reset();
//                        LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกการลาสำเร็จ " + EmpNo + " " + fullname);
//                    }
//                    /*exed.AddFormView(dsLeave, ExecuteType.Insert);
//                    exed.Execute();*/
//                }
//            }
//            catch (OracleException or)
//            {
//                LtServerMessage.Text = WebUtil.ErrorMessage(or.ToString());
//            }
        }

        #region reset
        private void reset()
        {
            dsMain.ResetRow();
            dsLeave.ResetRow();
            dsLeave.DdHrucfleavecode();
        }
        #endregion

        #region getSeqNo
        private decimal getSeqNo(string EmpNo, string CoopId)
        {
            decimal SeqNo = 0;
            string sql = @"select max(seq_no)+1 as seq_no from hrlogleave where emp_no ={0} and coop_id={1}";
            sql = WebUtil.SQLFormat(sql, EmpNo, CoopId);
            Sdt dt = WebUtil.QuerySdt(sql);

            if (dt.Next())
            {
                SeqNo = dt.GetDecimal("seq_no");
            }
            if (SeqNo == 0)
            {
                SeqNo = 1;
            }
            return SeqNo;
        }
        #endregion

        public void WebSheetLoadEnd()
        {

        }
    }
}