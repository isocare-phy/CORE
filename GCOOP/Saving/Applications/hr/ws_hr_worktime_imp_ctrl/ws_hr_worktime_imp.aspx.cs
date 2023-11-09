using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using DataLibrary;
//////////////////////////////////////////
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.OracleClient;
using System.Globalization;

/////////////////////////////////////////
using System.IO;
using System.Text;
using OfficeOpenXml;
/////////////////////////////////////////


namespace Saving.Applications.hr.ws_hr_worktime_imp_ctrl
{
    public partial class ws_hr_worktime_imp : PageWebSheet, WebSheet
    {
        [JsPostBack]
        public String JsPostProcess { get; set; }
        [JsPostBack]
        public String JsPostDelete { get; set; }
        public void InitJsPostBack()
        {
            dsMain.InitDsMain(this);
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                dsMain.DATA[0].entry_date = state.SsWorkDate;
            }
        }
        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "JsPostProcess")
            {
                PostProcess();
            }
            else if (eventArg == "JsPostDelete")
            {
                // PostDelete();
            }
        }
        private void PostProcess()
        {
            try
            {
                string ls_coopid = state.SsCoopId;
                DateTime entry_date = dsMain.DATA[0].entry_date;
                string ls_typecode = dsMain.DATA[0].type_code;
                string error = "";

                if (txtInput.HasFile)
                {
                    if (System.IO.Path.GetExtension(txtInput.FileName) == ".xlsx" || System.IO.Path.GetExtension(txtInput.FileName) == ".xls")
                    {

                        ExecuteDataSource exe = new ExecuteDataSource(this);
                        string into = Server.MapPath("~/WSRPDF/") + DateTime.Now.ToString("ddMMyyyyHHmmss") + "_" + txtInput.FileName;
                        txtInput.PostedFile.SaveAs(into);
                        FileInfo excel = new FileInfo(into);
                        using (var package = new ExcelPackage(excel))
                        {
                            var workbook = package.Workbook;
                            var worksheet = workbook.Worksheets.First();
                            string emp_no = "", worktime_code = "", emptype_code = "", day_work = "", emp_name = "", work_in = "", work_out = "";
                            string work_date, empno = "", latetime = "", back_time = "" ;
                            DateTime workdate;
                            for (int i = 2; i <= worksheet.Dimension.End.Row; i++)
                            {
                                error = "";
                                try
                                {
                                    //ดึงข้อมูลจาก excell
                                    empno = worksheet.Cells[i, 1].Text.ToString().Trim();
                                    if (empno != "")
                                    {
                                        emp_name = worksheet.Cells[i, 2].Text.ToString().Trim();
                                        work_date = worksheet.Cells[i, 3].Text.ToString().Trim();


                                        string sql_work_date = @"select 
                                                                (case when to_char(to_date({0},'dd/mm/yyyy'),'mm') in ('01','02','03','04','05','06','07','08','09','10','11','12') and
                                                                to_char(to_date({0},'dd/mm/yyyy'),'dd') in ('01','02','03','04','05','06','07','08','09','10','11','12')
                                                                then to_char(to_number(to_char(to_date({0},'dd/mm/yyyy'),'mm'))) || '/' || to_char(to_number(to_char(to_date({0},'dd/mm/yyyy'),'dd'))) || '/' || to_char(to_date({0},'dd/mm/yyyy'),'yyyy')
                                                                else {0} end) as work_date from dual";
                                        sql_work_date = WebUtil.SQLFormat(sql_work_date, work_date);
                                        Sdt dt_work_date = WebUtil.QuerySdt(sql_work_date);
                                        if (dt_work_date.Next())
                                        {
                                            work_date = dt_work_date.GetString("work_date");
                                        }

                                        work_in = worksheet.Cells[i, 4].Text.ToString().Trim();
                                        work_out = worksheet.Cells[i, 5].Text.ToString().Trim();
                                        latetime = worksheet.Cells[i, 6].Text.ToString().Trim();
                                        back_time = worksheet.Cells[i, 7].Text.ToString().Trim();
                                        back_time = back_time.Replace(":", "."); 

                                        //ดึงข้อมูลพนักงาน
                                        string sql = @"select emp_no,
                                                worktime_code,
                                                emptype_code
                                                from hremployee hr
                                                where coop_id ={0} and	trim(scan_no) = {1}";
                                        sql = WebUtil.SQLFormat(sql, state.SsCoopId, empno);
                                        Sdt dt = WebUtil.QuerySdt(sql);
                                        if (dt.Next())
                                        {
                                            emp_no = dt.GetString("emp_no");
                                            worktime_code = dt.GetString("worktime_code");
                                            emptype_code = dt.GetString("emptype_code");
                                            day_work = "1";

                                        }
                                        else
                                        {
                                            emp_no = "NO";
                                        }

                                        IFormatProvider culture = new CultureInfo("en-US", true);
                                        string fmt = "00";
                                        //workdate = DateTime.ParseExact(work_date, "MM/dd/yyyy", culture);

                                        try
                                        {
                                            workdate = DateTime.ParseExact(work_date, "dd/MM/yyyy", culture);
                                        }
                                        catch
                                        {
                                            try
                                            {
                                                workdate = DateTime.ParseExact(work_date, "M/d/yyyy", culture);
                                            }
                                            catch
                                            {
                                                try
                                                {
                                                    workdate = DateTime.ParseExact(work_date, "MM/dd/yyyy", culture);
                                                }
                                                catch
                                                {
                                                    try 
                                                    { 
                                                        workdate = DateTime.ParseExact(work_date, "d/M/yyyy", culture);
                                                    }
                                                    catch 
                                                    {
                                                        try
                                                        {
                                                            workdate = DateTime.ParseExact(work_date, "M/dd/yyyy", culture);
                                                        }
                                                        catch
                                                        {
                                                            try
                                                            {
                                                                workdate = DateTime.ParseExact(work_date, "dd/M/yyyy", culture);
                                                            }
                                                            catch
                                                            {
                                                                workdate = DateTime.ParseExact(work_date, "MM/dd/yy", culture);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        string day = workdate.Day.ToString(fmt);
                                        string month = workdate.Month.ToString(fmt);
                                        string Year = workdate.Year.ToString();
                                        if (work_in.Length < 5) //เชค Length ว่าเวลาเข้างาน เช่น 8.40 ให้เติม 0 ข้างหน้าเเพื่อให้ครบ format 00.00
                                        {
                                            work_in = "0" + work_in;
                                        }
                                        if (work_out.Length < 5)
                                        {
                                            work_out = "0" + work_out;
                                        }
                                        if (work_in == "0")
                                        {
                                            work_in = "00:00";
                                        }
                                        if (work_out == "0")
                                        {
                                            work_out = "00:00";
                                        }

                                        string leave_code = "";
                                        string sqlleave = @"select leave_code from hrlogleave where {0} between trunc(leave_from) and trunc(leave_to) and apv_status =  1";
                                        sqlleave = WebUtil.SQLFormat(sqlleave, workdate);
                                        Sdt dt_leave = WebUtil.QuerySdt(sqlleave);
                                        if (dt_leave.Next())
                                        {
                                            leave_code = dt_leave.GetString("leave_code");
                                        }

                                        string workdays = Holiday(day, month, Year);

                                        if (work_in == "00:00" && work_out == "00:00" && workdays == "W" && leave_code =="")
                                        {
                                            worktime_code = "LW";
                                        }
  
                                        string times = HourTime(work_in, work_out);
                                        //เชต fomat วันที่
                                        string timeString = Convert.ToString(Convert.ToDecimal(Year)) + "/" + month + "/" + day + " " + work_in;
                                        string timeString1 = Convert.ToString(Convert.ToDecimal(Year)) + "/" + month + "/" + day + " " + work_out;

                                        DateTime dateVal = DateTime.ParseExact(timeString, "yyyy/MM/dd HH:mm", culture);
                                        DateTime dateVal2 = DateTime.ParseExact(timeString1, "yyyy/MM/dd HH:mm", culture);

                                        //เชคว่าเคย import ไปเเล้วหรือยัง
                                        string time = Convert.ToString(Convert.ToDecimal(Year)) + "/" + month + "/" + day;
                                        string date = Convert.ToString(DateTime.ParseExact(time, "yyyy/MM/dd", culture));

                                        if (emp_no != "NO")
                                        {
                                            string date1 = "", e_no = "";  //select เวลาเข้างานจากค่าคงที่เพื่อเชคว่าเขางานสายไหม
                                            string sql1 = @"select emp_no ,work_date from hrlogworktime where coop_id ={0} and  TRUNC(work_date) ={1} and emp_no ={2} ";
                                            sql1 = WebUtil.SQLFormat(sql1, state.SsCoopId, dateVal.Date, emp_no);
                                            Sdt dt1 = WebUtil.QuerySdt(sql1);
                                            if (dt1.Next())
                                            {
                                                date1 = dt1.GetString("work_date");
                                                e_no = dt1.GetString("emp_no");
                                            }

                                            //ลบเข้างานสายก่อนเเล้ว insert ใหม่
                                            string sql_de = "delete from hrloglate where coop_id ={0} and emp_no ={1} and TRUNC(late_date) ={2}";
                                            sql_de = WebUtil.SQLFormat(sql_de, state.SsCoopId, e_no, dateVal.Date);
                                            WebUtil.ExeSQL(sql_de);

                                            if (latetime != "")//วันไหนเข้างานสายให้เข้าเงื่อนไข insert 
                                            {
                                                Postlate(latetime, Year, month, day, e_no, dateVal, emp_no, workdate, emptype_code);
                                            }

                                            if (date == date1) //กรณีมีการ impport ไฟล์เดิมซ้ำ วันที่ซ้ำ ให้เข้าเงื่อนไข
                                            {
                                                //ลบก่อนเเล้ว insert ใหม่
                                                string sql2 = "delete from hrlogworktime where coop_id ={0} and emp_no ={1} and TRUNC(work_date) ={2}";
                                                sql2 = WebUtil.SQLFormat(sql2, state.SsCoopId, e_no, dateVal.Date);
                                                WebUtil.ExeSQL(sql2);

                                                string insert = @"insert into HRLOGWORKTIME 
                                                                            ( coop_id , work_date,emp_no,start_time ,end_time,worktime_code ,emptype_code , day_work ,work_time,back_time) 
                                                                            values ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9})";
                                                insert = WebUtil.SQLFormat(insert, ls_coopid, workdate, emp_no, dateVal, dateVal2, worktime_code, emptype_code, day_work, times, back_time);
                                                WebUtil.ExeSQL(insert);
                                            }
                                            else
                                            {   //กรณี import เขางานครั้งเเรกให่ insertเลย
                                                string insert = @"insert into HRLOGWORKTIME 
                                                                            ( coop_id , work_date,emp_no,start_time ,end_time,worktime_code ,emptype_code , day_work,work_time,back_time) 
                                                                            values ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9})";
                                                insert = WebUtil.SQLFormat(insert, ls_coopid, workdate, emp_no, dateVal, dateVal2, worktime_code, emptype_code, day_work, times, back_time);
                                                WebUtil.ExeSQL(insert);

                                            }
                                        }
                                        else 
                                        {
                                            LtServerMessage.Text = WebUtil.ErrorMessage("Import ไฟล์ไม่สำเร็จ");
                                            error = "catch";
                                        }
                                    }
                                }
                                catch
                                {
                                    error = "catch";
                                }
                            }
                            if (error == "")
                            {
                                decimal seq_no = 0;
                                string sql_get_seq = "select nvl(max(seq_no),0) + 1 as seq_no from hrlogimpworktime where  coop_id={0}";
                                sql_get_seq = WebUtil.SQLFormat(sql_get_seq, state.SsCoopId);
                                Sdt dt_seq = WebUtil.QuerySdt(sql_get_seq);
                                if (dt_seq.Next())
                                {
                                    seq_no = dt_seq.GetDecimal("seq_no");
                                }

                                string insert = @"insert into hrlogimpworktime ( coop_id , seq_no,import_date,entry_id) 
                                                 values ({0},{1},{2},{3})";
                                insert = WebUtil.SQLFormat(insert, ls_coopid, seq_no, state.SsWorkDate, state.SsUsername);
                                WebUtil.ExeSQL(insert);

                                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลเสร็จสิ้น");
                            }
                            else
                            {
                                LtServerMessage.Text = WebUtil.ErrorMessage("Import ไฟล์ไม่สำเร็จ");
                            }
                        }
                    }
                    else
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage("ต้องเป็น ไฟล์ .xlsx เท่านั้น");
                    }
                }
                else
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("เลือกข้อมูลที่จะนำเข้า");
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถ import ข้อมูลได้ " + ex);
            }
        }
        private void Postlate(string latetime, string Year, string month, string day, string e_no, DateTime dateVal, string emp_no, DateTime workdate, string emptype_code)
        {
            try
            {
                if (latetime.Length < 5)
                {
                    latetime = "0" + latetime;
                }
                IFormatProvider culture = new CultureInfo("en-US", true);
                string late_cause = "", max_late = "";
                string lalte = Convert.ToString(Convert.ToDecimal(Year)) + "/" + month + "/" + day + " " + latetime;
                DateTime latedate = DateTime.ParseExact(lalte, "yyyy/MM/dd HH:mm", culture);

                string sql_late = "select work_in , max_late from hrucfworktimecode where worktime_code = (select worktime_code from hremployee where emp_no = '" + emp_no + "')";
                Sdt dt_late = WebUtil.QuerySdt(sql_late);
                if (dt_late.Next())
                {
                    max_late = dt_late.GetString("max_late");
                }

                int diff_hour = latedate.Hour;
                int diff_minute = latedate.Minute;

                if (diff_hour == 0 && diff_minute < Convert.ToDecimal(max_late))
                {
                    late_cause = "2";
                }
                else if (diff_hour == 0 && diff_minute >= Convert.ToDecimal(max_late))
                {
                    late_cause = "2";
                }
                else if (diff_hour > 0)
                {
                    late_cause = "2";
                }

                decimal seq = 1;
                string sql_get_seq = "select nvl(max(seq_no),0) + 1 as seq_no from hrloglate where emp_no={0} and coop_id={1}";
                sql_get_seq = WebUtil.SQLFormat(sql_get_seq, emp_no, state.SsCoopId);
                Sdt dt_seq = WebUtil.QuerySdt(sql_get_seq);
                if (dt_seq.Next())
                {
                    seq = dt_seq.GetDecimal("seq_no");
                }

                if (latetime != "")
                {
                    if (emptype_code == "01" || emptype_code == "02")
                    { 
                        late_cause = "2"; 
                    }
                    string insert = @"insert into HRLOGLATE ( coop_id ,emp_no,seq_no,late_date ,late_time,late_cause) 
                                  values ({0},{1},{2},{3},{4},{5})";
                    insert = WebUtil.SQLFormat(insert, state.SsCoopId, emp_no, seq, workdate, latedate, late_cause);
                    WebUtil.ExeSQL(insert);
                }
            }
            catch
            {

            }
        }
        private string HourTime(string work_in, string work_out)
        {


            string stime = work_in.Replace(':', '.');
            string ltime = work_out.Replace(':', '.');
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
        private string Holiday(string day, string month, string year)
        {
            string workdays = "";
            string sql = "select  substr(workdays,{0},1) as  workdays  from amworkcalendar where year ={1} and month = {2}";
            sql = WebUtil.SQLFormat(sql, day, Convert.ToDecimal(year)+543, month);
            Sdt dt = WebUtil.QuerySdt(sql);
            if (dt.Next())
            {
                workdays = dt.GetString("workdays");
            }
            return Convert.ToString(workdays);
        }


        public void SaveWebSheet()
        {

        }

        public void WebSheetLoadEnd()
        {


        }
    }
}