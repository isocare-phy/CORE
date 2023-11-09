using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OracleClient;
using CoreSavingLibrary;
using System.Globalization;
using DataLibrary;

namespace Saving.Applications.hr.ws_hr_inout_logtime_ctrl
{
    public partial class ws_hr_inout_logtime : PageWebSheet, WebSheet
    {
        [JsPostBack]
        public string PostWorkDate { get; set; }

        public void InitJsPostBack()
        {
            dsMain.InitMain(this);
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {

            }
            else
            {

            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "PostWorkDate")
            {
                string _emp_id = dsMain.DATA[0].EMP_NO;
                bool chkMemID = false;
                chkMemID = ValidateID(_emp_id);
                if (chkMemID != false)
                {
                    SaveWebSheet();
                }
                else
                {
                    cls();
                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่มีพนักงาน " + _emp_id + " ดังกล่าวกรุณาค้นหาอีกครั้ง");
                }
            }
        }

        #region ValidateID
        private bool ValidateID(string _emp_id)
        {
            string sqlValiID = @"select  * from hremployee where coop_id ={0} and emp_no={1}";
            bool _rechkID = false;
            object[] argVali = new object[] { state.SsCoopId, _emp_id };
            sqlValiID = WebUtil.SQLFormat(sqlValiID, argVali);
            Sdt sdt = WebUtil.QuerySdt(sqlValiID);
            if (sdt.Next())
            {
                _rechkID = true;
            }
            else
            {
                _rechkID = false;
            }
            return _rechkID;
        }
        #endregion
        #region Validate_TimeIn
        private bool Validate_TimeIn(string _emp_id)
        {
            string chkTimeIn = @"select * from hrlogworktime where emp_no={0} and work_date={1}";
            bool valiChkIn = false;
            try
            {
                object[] argChkTimeIn = new object[] { _emp_id, DateTime.Today };
                chkTimeIn = WebUtil.SQLFormat(chkTimeIn, argChkTimeIn);
                Sdt sdtChk = WebUtil.QuerySdt(chkTimeIn);
                if (sdtChk.Next())
                {
                    valiChkIn = false;
                }
                else
                {
                    valiChkIn = true;
                }
            }
            catch (OracleException orEx)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(orEx.ToString());
            }
            return valiChkIn;
        }
        #endregion
        #region list_detail
        private string list_detail(string _emp_id)
        {
            string _fullname = "", hr_detail = "";
            hr_detail = @"  SELECT 
                                    HREMPLOYEE.EMP_NO,
                                    MBUCFPRENAME.PRENAME_DESC,   
                                    HREMPLOYEE.EMP_NAME,   
                                    HREMPLOYEE.EMP_SURNAME  
                                        FROM MBUCFPRENAME,   
                                             HREMPLOYEE  
                                        WHERE ( MBUCFPRENAME.PRENAME_CODE = HREMPLOYEE.PRENAME_CODE )    
                                            AND HREMPLOYEE.EMP_NO = {0} AND HREMPLOYEE.COOP_ID= {1}";
            object[] arg_detail = new object[] { _emp_id, state.SsCoopId };
            hr_detail = WebUtil.SQLFormat(hr_detail, arg_detail);
            Sdt sdt = WebUtil.QuerySdt(hr_detail);
            if (sdt.Next())
            {
                _fullname = sdt.GetString("EMP_NO") + " " + sdt.GetString("PRENAME_DESC")
                    + sdt.GetString("EMP_NAME") + " " + sdt.GetString("EMP_SURNAME");
            }
            return _fullname;

        }
        #endregion
        #region updatelatelog
        private void updatelatelog(bool chkTimeLate, DateTime time_late, string coopid, decimal SeqNo, string _emp_id, DateTime time_now)
        {
            ExecuteDataSource exes = new ExecuteDataSource(this);
            string update_late_log = @"insert into hrloglate(coop_id, emp_no, seq_no, late_date, late_time, late_cause)
                                    VALUES ({0},{1},{2},{3},{4},{5})";
            try
            {
                object[] argInsert = new object[] { coopid, _emp_id, SeqNo, time_now, time_late, "" };
                update_late_log = WebUtil.SQLFormat(update_late_log, argInsert);
                exes.SQL.Add(update_late_log);
                exes.Execute();
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
        #endregion
        #region getSeqNo
        private decimal getSeqNo(string EmpNo, string CoopId)
        {
            decimal SeqNo = 0;
            string sql = @"select max(seq_no)+1 as seq_no from hrloglate where emp_no ={0} and coop_id={1}";
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
        #region getchkTimeLate & gettimelate
        private bool getchkTimeLate()
        {
            string _emp_id = dsMain.DATA[0].EMP_NO;
            string CoopId = state.SsCoopId;
            bool validate_timelate = false;
            DateTime standard_time = new DateTime();
            DateTime time_now = DateTime.Now;
            string sql = "select work_time from hrcfconstant";
            Sdt dt = WebUtil.QuerySdt(sql);
            if (dt.Next())
            {
                standard_time = dt.GetDate("work_time");
            }
            try
            {
                if (time_now.TimeOfDay > standard_time.TimeOfDay)
                {
                    validate_timelate = true;
                }
                else
                {
                    validate_timelate = false;
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.ToString());
            }
            return validate_timelate;
        }
        private DateTime gettimelate()
        {
            string _emp_id = dsMain.DATA[0].EMP_NO;
            string CoopId = state.SsCoopId;
            DateTime standard_time = new DateTime();
            DateTime time_now = DateTime.Now;
            DateTime time_late = new DateTime();
            string sql = "select work_time from hrcfconstant";
            Sdt dt = WebUtil.QuerySdt(sql);
            if (dt.Next())
            {
                standard_time = dt.GetDate("work_time");
            }
            int diff_hour = time_now.Hour - standard_time.Hour;
            int diff_minute = time_now.Minute - standard_time.Minute;
            time_late = new DateTime(time_now.Year, time_now.Month, time_now.Day, diff_hour, diff_minute, 0);
            return time_late;
        }
        #endregion
        public void SaveWebSheet()
        {
            ExecuteDataSource exes = new ExecuteDataSource(this);
            string _emp_id = dsMain.DATA[0].EMP_NO;
            string coopid = state.SsCoopId;
            DateTime time_now = DateTime.Now;
            DateTime time_late;
            string insert_time = "", wcode = "", wcode2 = "", fullname = "", update_time = "";
            bool chkTimeIn = false, chkTimeLate = false;
            decimal SeqNo = 0;
            fullname = list_detail(_emp_id);
            chkTimeIn = Validate_TimeIn(_emp_id);
            chkTimeLate = getchkTimeLate();
            wcode = "NML";
            wcode2 = "LA";
            if (chkTimeIn != false)
            {
                try
                {
                    if (chkTimeLate != false)
                    {
                        time_late = gettimelate();
                        SeqNo = getSeqNo(_emp_id, coopid);
                        updatelatelog(chkTimeLate, time_late, coopid, SeqNo, _emp_id, time_now);
                        #region insert_late_time
                        insert_time = @"insert into hrlogworktime (coop_id,emp_no,work_date,start_time,end_time,worktime_code)
                            values ({0},{1},{2},{3},{4},{5})";
                        try
                        {
                            object[] argInsert = new object[] { state.SsCoopId, _emp_id, DateTime.Today, time_now, time_now, wcode2 };
                            insert_time = WebUtil.SQLFormat(insert_time, argInsert);
                            exes.SQL.Add(insert_time);
                            exes.Execute();
                        }
                        catch (OracleException orEX)
                        {
                            LtServerMessage.Text = WebUtil.ErrorMessage(orEX.ToString());
                        }
                        finally
                        {
                            exes.SQL.Clear();
                            cls();
                            LtServerMessage.Text = WebUtil.CompleteMessage(fullname + " เข้างานสาย "
                                + time_now.ToString("dd/MM/yyyy HH:mm", new CultureInfo("th-TH")));
                        }
                        #endregion
                    }
                    else
                    {
                        #region insert_normal_time
                        insert_time = @"insert into hrlogworktime (coop_id,work_date,emp_no,start_time,end_time,worktime_code)
                            values ({0},{1},{2},{3},{4},{5})";
                        try
                        {
                            object[] argInsert = new object[] { state.SsCoopId, DateTime.Today, _emp_id, time_now, time_now, wcode };
                            insert_time = WebUtil.SQLFormat(insert_time, argInsert);
                            exes.SQL.Add(insert_time);
                            exes.Execute();
                        }
                        catch (OracleException orEX)
                        {
                            LtServerMessage.Text = WebUtil.ErrorMessage(orEX.ToString());
                        }
                        finally
                        {
                            exes.SQL.Clear();
                            cls();
                            LtServerMessage.Text = WebUtil.CompleteMessage(fullname + " เข้างาน "
                                + time_now.ToString("dd/MM/yyyy HH:mm", new CultureInfo("th-TH")));
                        }
                        #endregion
                    }
                }
                catch (OracleException or)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(or.ToString());
                }
            }
            else
            {
                #region update_time
                update_time = @"update hrlogworktime set end_time={0}
                                where emp_no={1} and work_date={2}";
                try
                {
                    object[] argUpdate = new object[] { time_now, _emp_id, DateTime.Today };
                    update_time = WebUtil.SQLFormat(update_time, argUpdate);
                    exes.SQL.Add(update_time);
                    exes.Execute();
                }
                catch (OracleException or)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(or.ToString());
                }
                finally
                {
                    exes.SQL.Clear();
                    cls();
                    LtServerMessage.Text = WebUtil.CompleteMessage(fullname + " ออกงาน "
                        + time_now.ToString("dd/MM/yyyy HH:mm", new CultureInfo("th-TH")));
                }
                #endregion
            }
        }

        public void WebSheetLoadEnd()
        {

        }

        public void cls()
        {
            dsMain.ResetRow();
        }

    }
}