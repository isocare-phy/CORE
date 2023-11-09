using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using System.Data;
using DataLibrary;

namespace Saving.Applications.hr.ws_hr_leave_ctrl
{
    public partial class ws_hr_leave : PageWebSheet, WebSheet
    {
        [JsPostBack]
        public string PostEmpNo { get; set; }
        [JsPostBack]
        public String PostEmpLeave { get; set; }

        [JsPostBack]
        public String PostEmpLeaveDelete { get; set; }

        public void InitJsPostBack()
        {
            dsLeave.InitDs(this);
            dsMain.InitDs(this);
            dsDetail.InitDsDetail(this);
            //dsLasrights.InitDs(this);
            //dsCallas.InitDs(this);
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                dsLeave.DdHrucfleavecode();
                dsLeave.DdHrucfleaveapvby();
                dsLeave.DATA[0].COOP_ID = state.SsCoopId;//Data[0] คือ กำหนดข้อมูลที่ฟิวไหน กรณีรูปแบบของ FormView
                dsLeave.Visible = false;
                dsDetail.Visible = false;
                dsLasrights.Visible = false;
                dsCallas.Visible = false;
            }
            else
            {
                
                dsLeave.Visible = false;
                dsDetail.Visible = false;
                dsLasrights.Visible = false;
                dsCallas.Visible = false;
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == PostEmpNo)
            {
                dsLeave.Visible = true;
                dsDetail.Visible = true;
                dsLasrights.Visible = true;
                dsCallas.Visible = true;
                string EmpNo = dsMain.DATA[0].EMP_NO;
                dsLeave.DATA[0].OPERATE_DATE = state.SsWorkDate;
                dsMain.RetrieveEmp(EmpNo);
                dsLeave.DATA[0].EMP_NO = EmpNo;//นำข้อมูล emp_no จากที่ RetrieveEmp
                displeyAddress(EmpNo);
                dsDetail.RetrieveDetail(EmpNo);
                dsLasrights.Retrieveleavemax(EmpNo);
                dsCallas.RetrieveWorkage(EmpNo);
                calleave(EmpNo);
                
            }
            else if (eventArg == PostEmpLeave)
            {
                dsLeave.Visible = true;
                dsDetail.Visible = true;
                dsLasrights.Visible = true;
                dsCallas.Visible = true;
                int row = dsDetail.GetRowFocus();
                string Seqno = Convert.ToString(dsDetail.DATA[row].SEQ_NO);
                string EmpNo = dsMain.DATA[0].EMP_NO;
                dsLeave.Retrieveleave(Seqno,EmpNo);
                displeyAddress(EmpNo);
                dsDetail.RetrieveDetail(EmpNo);
                dsLeave.DdHrucfleaveapvby();
                dsLeave.DdHrucfleavecode();
                dsLasrights.Retrieveleavemax(EmpNo);
                dsCallas.RetrieveWorkage(EmpNo);
                calleave(EmpNo);
            }
            else if (eventArg == PostEmpLeaveDelete)
            {
                dsLeave.Visible = true;
                dsDetail.Visible = true;
                dsLasrights.Visible = true;
                dsCallas.Visible = true;
                int row = dsDetail.GetRowFocus();
                string Seqno = Convert.ToString(dsDetail.DATA[row].SEQ_NO);
                string EmpNo = dsMain.DATA[0].EMP_NO;

                ExecuteDataSource exe = new ExecuteDataSource(this);
                string ls_sqldel = @"delete from hrlogleave where seq_no = " + Seqno + " and emp_no = '" + EmpNo + "'";
                exe.SQL.Add(ls_sqldel);
                exe.Execute();
                exe.SQL.Clear();

                dsLeave.Retrieveleave(Seqno, EmpNo);
                displeyAddress(EmpNo);
                dsDetail.RetrieveDetail(EmpNo);
                dsLeave.DdHrucfleaveapvby();
                dsLeave.DdHrucfleavecode();
                dsLasrights.Retrieveleavemax(EmpNo);
                dsCallas.RetrieveWorkage(EmpNo);
                calleave(EmpNo);
            } 

            else if (eventArg == "Postday")
            {
                dsLeave.Visible = true;
                dsDetail.Visible = true;
                dsLasrights.Visible = true;
                dsCallas.Visible = true;
                DateTime dt1 = Convert.ToDateTime(dsLeave.DATA[0].LEAVE_FROM);
                DateTime dt2 = Convert.ToDateTime(dsLeave.DATA[0].LEAVE_TO);
                TimeSpan ts = dt2 - dt1;
                dsLeave.DATA[0].TOTALDAY = ts.Days;
            }
        }

        private void displeyAddress(string EmpNo)
        {
            string sql, address = "", addr_no = "", addr_moo = "", addr_village = ""
                , addr_road = "", addr_tambol = "", addr_province = "", addr_postcode = "", addr_mobilephone = "";
            try
            {
                sql = @"select 
                            hr.emp_no,
                            hr.ref_membno,
                            ma.member_no,
                            ma.addr_no as addr_no,
                            ma.addr_moo as addr_moo,
                            ma.addr_village as addr_village,
                            ma.addr_road as addr_road,
                            ma.addr_mobilephone as  addr_mobilephone,
						    ucftam.tambol_desc as ta,
						    ucfpro.province_desc as pr,
                            ma.addr_postcode as addr_postcode
                         from
                            hremployee hr,mbmembmaster ma,mbucftambol ucftam
						    ,mbucfprovince ucfpro
                         where
                            hr.emp_no = {0} and
                            hr.ref_membno=ma.member_no and
						    ma.tambol_code=ucftam.tambol_code and
						    ma.province_code=ucfpro.province_code";
                sql = WebUtil.SQLFormat(sql, EmpNo);
                Sdt dt = WebUtil.QuerySdt(sql);
                if (dt.Next())
                {
                    addr_no = dt.GetString("addr_no");
                    addr_moo = dt.GetString("addr_moo");
                    addr_village = dt.GetString("addr_village");
                    addr_road = dt.GetString("addr_road");
                    addr_tambol = dt.GetString("ta");
                    addr_province = dt.GetString("pr");
                    addr_postcode = dt.GetString("addr_postcode");
                    addr_mobilephone = dt.GetString("addr_mobilephone");
                }
                if (addr_no != "")
                {
                    address = "เลขที่ " + addr_no;
                }
                else
                {
                    address += "";
                }
                if (addr_moo != "")
                {
                    address += " หมู่ที่ " + addr_moo;
                }
                else
                {
                    address += "";
                }
                if (addr_village != "")
                {
                    address += " หมู่บ้าน" + addr_village;
                }
                else
                {
                    address += "";
                }
                if (addr_road != "")
                {
                    address += " ถ." + addr_road;
                }
                else
                {
                    address += "";
                }
                if (addr_tambol != "")
                {
                    address += " ตำบล" + addr_tambol;
                }
                else
                {
                    address += "";
                }
                if (addr_province != "")
                {
                    address += " จ." + addr_province;
                }
                else
                {
                    address += "";
                }
                if (addr_postcode != "")
                {
                    address += " " + addr_postcode;
                }
                else
                {
                    address += "";
                }
                
                dsLeave.DATA[0].LEAVE_PLACE = address;
                dsLeave.DATA[0].LEAVE_TEL = addr_mobilephone;
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.WarningMessage(ex.ToString());
            }
        }

        public void calleave(string EmpNo)
        {
            string year = Convert.ToString(WebUtil.GetAccyear(state.SsCoopControl, state.SsWorkDate));
            string year_eng = Convert.ToString(Convert.ToInt32(year) - 543);
            string year_eng2 = Convert.ToString(Convert.ToInt32(year_eng) - 1);
            string date_leave = "";
            string sql = @"select TO_CHAR(WORK_DATE, 'YYYY') as WORK_DATE from HREMPLOYEE where EMP_NO = {0}";
            sql = WebUtil.SQLFormat(sql, EmpNo);
            Sdt dt = WebUtil.QuerySdt(sql);
            if(dt.Next()){
                date_leave = dt.GetString("WORK_DATE");
            }
            //เช็คปีที่ทำงาน
            if (date_leave == year_eng)
            {
                //นับวันลาพักผ่อนของปีปัจจุบัน
                sql = @"select sum(hl.TOTALDAY) as TODAY
              FROM hrlogleave hl,
                   hrucfleavecode hlc,
                   hremployee he 
              where hl.leave_code = hlc.leave_code 
                                       and hl.emp_no = he.emp_no
                                       and TO_CHAR(hl.operate_date, 'YYYY') = {0}
                                       and he.emp_no = {1}
                                       and hl.leave_code = '006'";
                sql = WebUtil.SQLFormat(sql, year_eng, EmpNo);
                dt = WebUtil.QuerySdt(sql);
                string total = dt.GetString("TODAY");
                if (total == "")
                {
                    total = "0";
                }
                else
                {
                    total = dt.GetString("TODAY");
                }
                //เก็บจำนวนสิทธิวันลาปีนี้
                string today = dsCallas.DATA[0].TO_YEAR;
                dsCallas.DATA[0].FROM_YEAR = "0";
                //นำค่าวันลาปีนี้ลบออกจาสิทธิที่ลาได้
                int sumtoday = Convert.ToInt32(dsCallas.DATA[0].TO_YEAR) - Convert.ToInt32(total);
                //นำค่าวันลาปีนี้บวกกับสิทธิลาของปีก่อน
                int sumday = Convert.ToInt32(sumtoday) + Convert.ToInt32(dsCallas.DATA[0].FROM_YEAR);
                //ตรวจสอบการลาทั้ง 2 ปีรวมกันไม่เกิน 30 วัน
                if (sumday > 30)
                {
                    dsCallas.DATA[0].TOTAL = "30";
                }
                else
                {
                    dsCallas.DATA[0].TOTAL = Convert.ToString(sumday);
                }
            }
            else
            {
                //เก็บจำนวนสิทธิวันลาปีนี้
                string today = dsCallas.DATA[0].TO_YEAR;
                //นับวันลาพักผ่อนของปีก่อน
                sql = @"select sum(hl.TOTALDAY) as TODAY
              FROM hrlogleave hl,
                   hrucfleavecode hlc,
                   hremployee he 
              where hl.leave_code = hlc.leave_code 
                                       and hl.emp_no = he.emp_no
                                       and TO_CHAR(hl.operate_date, 'YYYY') = {0}
                                       and he.emp_no = {1}
                                       and hl.leave_code = '006'";
                sql = WebUtil.SQLFormat(sql, year_eng2, EmpNo);
                dt = WebUtil.QuerySdt(sql);
                string total = dt.GetString("TODAY");
                if (total == "")
                {
                    total = "0";
                }
                else
                {
                    total = dt.GetString("TODAY");
                }
                
                //คำนวนวันลาที่เหลือของปีก่อน
                string day = Convert.ToString(Convert.ToInt32(today) - Convert.ToInt32(total));
                dsCallas.DATA[0].FROM_YEAR = day;
                int sumday = Convert.ToInt32(dsCallas.DATA[0].TO_YEAR) + Convert.ToInt32(dsCallas.DATA[0].FROM_YEAR);
                if (sumday > 30)
                {
                    dsCallas.DATA[0].TOTAL = "30";
                }
                else
                {
                    dsCallas.DATA[0].TOTAL = Convert.ToString(sumday);
                }
                string to_sumtal = dsCallas.DATA[0].TOTAL;
                //นับวันลาพักผ่อนของปีปัจจุบัน
                string to_tal = "",leave = "";
                string sql2 = @"select sum(hl.TOTALDAY) as TO_DAY
              FROM hrlogleave hl,
                   hrucfleavecode hlc,
                   hremployee he 
              where hl.leave_code = hlc.leave_code 
                                       and hl.emp_no = he.emp_no
                                       and TO_CHAR(hl.operate_date, 'YYYY') = {0}
                                       and he.emp_no = {1}
                                       and hl.leave_code = '006'";
                sql2 = WebUtil.SQLFormat(sql2, year_eng, EmpNo);
                Sdt dt2 = WebUtil.QuerySdt(sql2);
                if (dt2.Next())
                {
                    to_tal = dt2.GetString("TO_DAY");
                }
                if (to_tal == "")
                {
                    to_tal = "0";
                }
                else
                {
                    to_tal = dt2.GetString("TO_DAY");
                }
                leave = Convert.ToString(Convert.ToInt32(to_sumtal) - Convert.ToInt32(to_tal));
                dsCallas.DATA[0].DAYTAL = leave;
            }
        }

        public void SaveWebSheet()
        {
            ExecuteDataSource exed = new ExecuteDataSource(this);
            string coop_id = state.SsCoopControl;
            string EmpNo = dsMain.DATA[0].EMP_NO;
            string leave_code = dsLeave.DATA[0].LEAVE_CODE;
            string sql = "select * from hrlogleave where coop_id = {0} and emp_no = {1}";
            string row = dsLeave.DATA[0].SEQ_NO.ToString();
            decimal todtalday = 0;
            //decimal month = dsLeave.DATA[0].LEAVE_FROM.Month;
            decimal year = dsLeave.DATA[0].LEAVE_FROM.Year;
            decimal month_d = dsLeave.DATA[0].LEAVE_FROM.Month;
            string month = "";

            if (Convert.ToString(month_d) == "1" || Convert.ToString(month_d) == "2" || Convert.ToString(month_d) == "3" || Convert.ToString(month_d) == "4" || Convert.ToString(month_d) == "5" || Convert.ToString(month_d) == "6" || Convert.ToString(month_d) == "7" || Convert.ToString(month_d) == "8" || Convert.ToString(month_d) == "9")
            {

                month = "0" + Convert.ToString(month_d);
            }
            else
            {
                month =  Convert.ToString(month_d);
            }

            string emp_no_check = "";
            string leave_001_s = "";
            string leave_001_c = "";
            decimal sumday_leave001 = 0;
            decimal count_leave001 = 0;
            string emp_no_check2 = "";
            string leave_002_s = "";
            string leave_002_c = "";
            decimal sumday_leave002 = 0;
            decimal count_leave002 = 0;
            string emp_no_check6 = "";
            string leave_006_s = "";
            string leave_006_c = "";
            decimal sumday_leave006 = 0;
            decimal count_leave006 = 0;
            decimal sum_todtalday = 0;
            decimal month_edit_d = 0;
            string month_edit = "";
            decimal year_edit = 0;
            string leave_001_edit_s = ""; string leave_001_edit_c = "";
            decimal original_leave_001_s = 0; decimal original_leave_001_c = 0;
            string leave_002_edit_s = ""; string leave_002_edit_c = ""; decimal original_leave_002_s = 0; decimal original_leave_002_c = 0;
            string leave_006_edit_s = ""; string leave_006_edit_c = ""; decimal original_leave_006_s = 0; decimal original_leave_006_c = 0;
            DateTime leave_from;
            todtalday = dsLeave.DATA[0].TOTALDAY;
            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, dsMain.DATA[0].EMP_NO.Trim());
            Sdt dt = WebUtil.QuerySdt(sql);
            try
            {
                if (dt.Rows.Count <= 0  )
                {
                    decimal SeqNo = 1;//unique
                    string fullname = dsMain.DATA[0].FULLNAME;
                    dsLeave.DATA[0].SEQ_NO = SeqNo;//กำหนดค่าให้ ds.Leave >> SEQ_NO ใหม่ ให้เป็น ค่าใหม่ที่กำหนด จาก string sql select max(seq_no)+1
                    //SavePass ตัวดัก Eror
                    dsLeave.DATA[0].EMP_NO = EmpNo;
                    dsLeave.DATA[0].COOP_ID = coop_id;
                    ExecuteDataSource exe = new ExecuteDataSource(this);
                    exe.AddFormView(dsLeave, ExecuteType.Insert);
                    exe.Execute();

                    if (leave_code == "001")
                    {

                        string ls_sql001 = @"insert into hrcheckleave(emp_no,year,month,leave_001_s,leave_001_c,leave_002_s,leave_002_c,leave_004_s,leave_004_c,leave_006_s,leave_006_c,late_date)values({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11})";
                        ls_sql001 = WebUtil.SQLFormat(ls_sql001, EmpNo, year, month, '0', '0', '0', '0', '0', '0', '0', '0', '0');
                        exed.SQL.Add(ls_sql001);
                        exed.Execute();
                        exed.SQL.Clear();

                        string sqlleave_001 = @"update hrcheckleave set leave_001_s = {0} , leave_001_c = {1} where emp_no={2} and year = {3} and month = {4}";
                        sqlleave_001 = WebUtil.SQLFormat(sqlleave_001, Convert.ToString(todtalday), '1', EmpNo, year, month);
                        Sdt dt_leave_001 = WebUtil.QuerySdt(sqlleave_001);

                    }
                    else if (leave_code == "002")
                    {

                        string ls_sql002 = @"insert into hrcheckleave(emp_no,year,month,leave_001_s,leave_001_c,leave_002_s,leave_002_c,leave_004_s,leave_004_c,leave_006_s,leave_006_c,late_date)values({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11})";
                        ls_sql002 = WebUtil.SQLFormat(ls_sql002, EmpNo, year, month, '0', '0', '0', '0', '0', '0', '0', '0', '0');
                        exed.SQL.Add(ls_sql002);
                        exed.Execute();
                        exed.SQL.Clear();

                        string sqlleave_002 = @"update hrcheckleave set leave_002_s = {0} , leave_002_c = {1} where emp_no={2} and year = {3} and month = {4}";
                        sqlleave_002 = WebUtil.SQLFormat(sqlleave_002, Convert.ToString(todtalday), '1', EmpNo, year, month);
                        Sdt dt_leave_002 = WebUtil.QuerySdt(sqlleave_002);

                    }
                    else if (leave_code == "006")
                    {

                        string ls_sql006 = @"insert into hrcheckleave(emp_no,year,month,leave_001_s,leave_001_c,leave_002_s,leave_002_c,leave_004_s,leave_004_c,leave_006_s,leave_006_c,late_date)values({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11})";
                        ls_sql006 = WebUtil.SQLFormat(ls_sql006, EmpNo, year, month, '0', '0', '0', '0', '0', '0', '0', '0', '0');
                        exed.SQL.Add(ls_sql006);
                        exed.Execute();
                        exed.SQL.Clear();

                        string sqlleave_006 = @"update hrcheckleave set leave_006_s = {0} , leave_006_c = {1} where emp_no={2} and year = {3} and month = {4}";
                        sqlleave_006 = WebUtil.SQLFormat(sqlleave_006, Convert.ToString(todtalday), '1', EmpNo, year, month);
                        Sdt dt_leave_006 = WebUtil.QuerySdt(sqlleave_006);

                    }

                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกการลาสำเร็จ " + " " + EmpNo + " " + fullname);
                    reset();
                    dsDetail.RetrieveDetail(EmpNo);
                }
                else 
                {
                
                string fullname = dsMain.DATA[0].FULLNAME;
                //string seqno = Convert.ToString(dsDetail.DATA[0].SEQ_NO);
                if (dsLeave.DATA[0].SEQ_NO.ToString() != "0")
                    {

                        if (leave_code == "001")
                        {
                            string sql_leave = @"select totalday as sum_todtalday , leave_from from hrlogleave where emp_no = {0} and seq_no = {1}";
                            sql_leave = WebUtil.SQLFormat(sql_leave, EmpNo, dsLeave.DATA[0].SEQ_NO.ToString());
                            // Sdt dt = WebUtil.QuerySdt(sql);
                            Sdt dt_leave = WebUtil.QuerySdt(sql_leave);
                            if (dt_leave.Next())
                            {
                                sum_todtalday = dt_leave.GetDecimal("sum_todtalday");
                                leave_from = dt_leave.GetDate("leave_from");
                                month_edit_d = leave_from.Month;
                                
                                if (Convert.ToString(month_edit_d) == "1" || Convert.ToString(month_edit_d) == "2" || Convert.ToString(month_edit_d) == "3" || Convert.ToString(month_edit_d) == "4" || Convert.ToString(month_edit_d) == "5" || Convert.ToString(month_edit_d) == "6" || Convert.ToString(month_edit_d) == "7" || Convert.ToString(month_edit_d) == "8" || Convert.ToString(month_edit_d) == "9")
                                {

                                    month_edit = "0" + Convert.ToString(month_d);
                                }
                                else
                                {
                                    month_edit = Convert.ToString(month_d);
                                }

                                year_edit = leave_from.Year;
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

                            original_leave_001_s = Convert.ToDecimal(leave_001_edit_s) - sum_todtalday;
                            original_leave_001_c = Convert.ToDecimal(leave_001_edit_c) - 1;

                            string sqledit_001 = @"update hrcheckleave set leave_001_s = {0} , leave_001_c = {1}  where emp_no={2} and year = {3} and month = {4}";
                            sqledit_001 = WebUtil.SQLFormat(sqledit_001, Convert.ToString(original_leave_001_s), Convert.ToString(original_leave_001_c), EmpNo, year, month);
                            Sdt dt_edit_001 = WebUtil.QuerySdt(sqledit_001);

                        }
                        else if (leave_code == "002")
                        {
                            string sql_leave = @"select totalday as sum_todtalday , leave_from from hrlogleave where emp_no = {0} and seq_no = {1}";
                            sql_leave = WebUtil.SQLFormat(sql_leave, EmpNo, dsLeave.DATA[0].SEQ_NO.ToString());
                            // Sdt dt = WebUtil.QuerySdt(sql);
                            Sdt dt_leave = WebUtil.QuerySdt(sql_leave);
                            if (dt_leave.Next())
                            {
                                sum_todtalday = dt_leave.GetDecimal("sum_todtalday");
                                leave_from = dt_leave.GetDate("leave_from");
                                month_edit_d = leave_from.Month;

                                if (Convert.ToString(month_edit_d) == "1" || Convert.ToString(month_edit_d) == "2" || Convert.ToString(month_edit_d) == "3" || Convert.ToString(month_edit_d) == "4" || Convert.ToString(month_edit_d) == "5" || Convert.ToString(month_edit_d) == "6" || Convert.ToString(month_edit_d) == "7" || Convert.ToString(month_edit_d) == "8" || Convert.ToString(month_edit_d) == "9")
                                {

                                    month_edit = "0" + Convert.ToString(month_d);
                                }
                                else
                                {
                                    month_edit = Convert.ToString(month_d);
                                }
                                year_edit = leave_from.Year;
                            }

                            string sql_hrcheckedit = @"select leave_002_s,leave_002_c from hrcheckleave where emp_no = {0} and year = {1} and month = {2}";
                            sql_hrcheckedit = WebUtil.SQLFormat(sql_hrcheckedit, EmpNo, year_edit, month_edit);
                            // Sdt dt = WebUtil.QuerySdt(sql);
                            Sdt dt_edit = WebUtil.QuerySdt(sql_hrcheckedit);
                            if (dt_edit.Next())
                            {
                                leave_002_edit_s = dt_edit.GetString("leave_002_s");
                                leave_002_edit_c = dt_edit.GetString("leave_002_c");
                            }

                            original_leave_002_s = Convert.ToDecimal(leave_002_edit_s) - sum_todtalday;
                            original_leave_002_c = Convert.ToDecimal(leave_002_edit_c) - 1;

                            string sqledit_002 = @"update hrcheckleave set leave_002_s = {0} , leave_002_c = {1}  where emp_no={2} and year = {3} and month = {4}";
                            sqledit_002 = WebUtil.SQLFormat(sqledit_002, Convert.ToString(original_leave_002_s), Convert.ToString(original_leave_002_c), EmpNo, year, month);
                            Sdt dt_edit_002 = WebUtil.QuerySdt(sqledit_002);
                        }
                        else if (leave_code == "006")
                        {

                            string sql_leave = @"select totalday as sum_todtalday , leave_from from hrlogleave where emp_no = {0} and seq_no = {1}";
                            sql_leave = WebUtil.SQLFormat(sql_leave, EmpNo, dsLeave.DATA[0].SEQ_NO.ToString());
                            // Sdt dt = WebUtil.QuerySdt(sql);
                            Sdt dt_leave = WebUtil.QuerySdt(sql_leave);
                            if (dt_leave.Next())
                            {
                                sum_todtalday = dt_leave.GetDecimal("sum_todtalday");
                                leave_from = dt_leave.GetDate("leave_from");
                                month_edit_d = leave_from.Month;

                                if (Convert.ToString(month_edit_d) == "1" || Convert.ToString(month_edit_d) == "2" || Convert.ToString(month_edit_d) == "3" || Convert.ToString(month_edit_d) == "4" || Convert.ToString(month_edit_d) == "5" || Convert.ToString(month_edit_d) == "6" || Convert.ToString(month_edit_d) == "7" || Convert.ToString(month_edit_d) == "8" || Convert.ToString(month_edit_d) == "9")
                                {

                                    month_edit = "0" + Convert.ToString(month_d);
                                }
                                else
                                {
                                    month_edit = Convert.ToString(month_d);
                                }
                                year_edit = leave_from.Year;
                            }

                            string sql_hrcheckedit = @"select leave_006_s,leave_006_c from hrcheckleave where emp_no = {0} and year = {1} and month = {2}";
                            sql_hrcheckedit = WebUtil.SQLFormat(sql_hrcheckedit, EmpNo, year_edit, month_edit);
                            // Sdt dt = WebUtil.QuerySdt(sql);
                            Sdt dt_edit = WebUtil.QuerySdt(sql_hrcheckedit);
                            if (dt_edit.Next())
                            {
                                leave_006_edit_s = dt_edit.GetString("leave_006_s");
                                leave_006_edit_c = dt_edit.GetString("leave_006_c");
                            }

                            original_leave_006_s = Convert.ToDecimal(leave_006_edit_s) - sum_todtalday;
                            original_leave_006_c = Convert.ToDecimal(leave_006_edit_c) - 1;

                            string sqledit_006 = @"update hrcheckleave set leave_006_s = {0} , leave_006_c = {1}  where emp_no={2} and year = {3} and month = {4}";
                            sqledit_006 = WebUtil.SQLFormat(sqledit_006, Convert.ToString(original_leave_006_s), Convert.ToString(original_leave_006_c), EmpNo, year, month);
                            Sdt dt_edit_006 = WebUtil.QuerySdt(sqledit_006);
                        }


                        string sql2 = "select * from hrlogleave where coop_id = {0} and emp_no = {1}";
                        sql2 = WebUtil.SQLFormat(sql2, state.SsCoopControl, dsLeave.DATA[0].EMP_NO.Trim());
                        Sdt dt2 = WebUtil.QuerySdt(sql);
                        dsLeave.DATA[0].COOP_ID = coop_id;
                        exed.AddFormView(dsLeave, ExecuteType.Update);
                        exed.Execute();
                        exed.SQL.Clear();

                    if(leave_code == "001"){

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
                            ExecuteDataSource exe = new ExecuteDataSource(this);
                            exe.SQL.Add(ls_sql_check);
                            exe.Execute();
                            exe.SQL.Clear();

                            sumday_leave001 = Convert.ToDecimal(todtalday) + 0;
                            count_leave001 =  1;

                            string sqlleave_001 = @"update hrcheckleave set leave_001_s = {0} , leave_001_c = {1} where emp_no={2} and year = {3} and month = {4}";
                            sqlleave_001 = WebUtil.SQLFormat(sqlleave_001, Convert.ToString(sumday_leave001), Convert.ToString(count_leave001), EmpNo, year, month);
                            Sdt dt_leave_001 = WebUtil.QuerySdt(sqlleave_001);

                        }
                        else
                        {
                            sumday_leave001 = Convert.ToDecimal(todtalday) + Convert.ToDecimal(leave_001_s);
                            count_leave001 = Convert.ToDecimal(leave_001_c) + 1;

                            string sqlleave_001 = @"update hrcheckleave set leave_001_s = {0} , leave_001_c = {1} where emp_no={2} and year = {3} and month = {4}";
                            sqlleave_001 = WebUtil.SQLFormat(sqlleave_001, Convert.ToString(sumday_leave001), Convert.ToString(count_leave001), EmpNo, year, month);
                            Sdt dt_leave_001 = WebUtil.QuerySdt(sqlleave_001);
                        }
                    }
                        else if(leave_code == "002"){

                            string sql_hrcheckleave = @"select emp_no,leave_002_s,leave_002_c from hrcheckleave where emp_no = {0} and year = {1} and month = {2}";
                            sql_hrcheckleave = WebUtil.SQLFormat(sql_hrcheckleave, EmpNo, year, month);
                            // Sdt dt = WebUtil.QuerySdt(sql);
                            Sdt dt_check002 = WebUtil.QuerySdt(sql_hrcheckleave);
                            if (dt_check002.Next())
                            {
                                emp_no_check = dt_check002.GetString("emp_no");
                                leave_002_s = dt_check002.GetString("leave_002_s");
                                leave_002_c = dt_check002.GetString("leave_002_c");

                            }

                            if (emp_no_check == "")
                            {

                                string ls_sql_check = @"insert into hrcheckleave(emp_no,year,month,leave_001_s,leave_001_c,leave_002_s,leave_002_c,leave_004_s,leave_004_c,leave_006_s,leave_006_c,late_date)values({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11})";
                                ls_sql_check = WebUtil.SQLFormat(ls_sql_check, EmpNo, year, month, '0', '0', '0', '0', '0', '0', '0', '0', '0');
                                ExecuteDataSource exe = new ExecuteDataSource(this);
                                exe.SQL.Add(ls_sql_check);
                                exe.Execute();
                                exe.SQL.Clear();

                                sumday_leave002 = Convert.ToDecimal(todtalday) + 0;
                                count_leave002 =  1;

                                string sqlleave_002 = @"update hrcheckleave set leave_002_s = {0} , leave_002_c = {1} where emp_no={2} and year = {3} and month = {4}";
                                sqlleave_002 = WebUtil.SQLFormat(sqlleave_002, Convert.ToString(sumday_leave002), Convert.ToString(count_leave002), EmpNo, year, month);
                                Sdt dt_leave_002 = WebUtil.QuerySdt(sqlleave_002);

                            }
                            else
                            {
                                sumday_leave002 = Convert.ToDecimal(todtalday) + Convert.ToDecimal(leave_002_s);
                                count_leave002 = Convert.ToDecimal(leave_002_c) + 1;

                                string sqlleave_002 = @"update hrcheckleave set leave_002_s = {0} , leave_002_c = {1} where emp_no={2} and year = {3} and month = {4}";
                                sqlleave_002 = WebUtil.SQLFormat(sqlleave_002, Convert.ToString(sumday_leave002), Convert.ToString(count_leave002), EmpNo, year, month);
                                Sdt dt_leave_002 = WebUtil.QuerySdt(sqlleave_002);
                            }

                        }else if(leave_code == "006"){

                            string sql_hrcheckleave006 = @"select emp_no,leave_006_s,leave_006_c from hrcheckleave where emp_no = {0} and year = {1} and month = {2}";
                            sql_hrcheckleave006 = WebUtil.SQLFormat(sql_hrcheckleave006, EmpNo, year, month);
                            // Sdt dt = WebUtil.QuerySdt(sql);
                            Sdt dt_check006 = WebUtil.QuerySdt(sql_hrcheckleave006);
                            if (dt_check006.Next())
                            {
                                emp_no_check6 = dt_check006.GetString("emp_no");
                                leave_006_s = dt_check006.GetString("leave_006_s");
                                leave_006_c = dt_check006.GetString("leave_006_c");

                            }

                            if (emp_no_check6 == "")
                            {

                                string ls_sql_check6 = @"insert into hrcheckleave(emp_no,year,month,leave_001_s,leave_001_c,leave_002_s,leave_002_c,leave_004_s,leave_004_c,leave_006_s,leave_006_c,late_date)values({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11})";
                                ls_sql_check6 = WebUtil.SQLFormat(ls_sql_check6, EmpNo, year, month, '0', '0', '0', '0', '0', '0', '0', '0', '0');
                                ExecuteDataSource exe = new ExecuteDataSource(this);
                                exed.SQL.Add(ls_sql_check6);
                                exed.Execute();
                                exed.SQL.Clear();

                                sumday_leave006 = Convert.ToDecimal(todtalday) + 0;
                                count_leave006 =  1;

                                string sqlleave_006 = @"update hrcheckleave set leave_006_s = {0} , leave_006_c = {1} where emp_no={2} and year = {3} and month = {4}";
                                sqlleave_006 = WebUtil.SQLFormat(sqlleave_006, Convert.ToString(sumday_leave006), Convert.ToString(count_leave006), EmpNo, year, month);
                                Sdt dt_leave_006 = WebUtil.QuerySdt(sqlleave_006);

                            }
                            else
                            {
                                sumday_leave006 = Convert.ToDecimal(todtalday) + Convert.ToDecimal(leave_006_s);
                                count_leave006 = Convert.ToDecimal(leave_006_c) + 1;

                                string sqlleave_006 = @"update hrcheckleave set leave_006_s = {0} , leave_006_c = {1} where emp_no={2} and year = {3} and month = {4}";
                                sqlleave_006 = WebUtil.SQLFormat(sqlleave_006, Convert.ToString(sumday_leave006), Convert.ToString(count_leave006), EmpNo, year, month);
                                Sdt dt_leave_006 = WebUtil.QuerySdt(sqlleave_006);
                            }

                        }

                        LtServerMessage.Text = WebUtil.CompleteMessage("แก้ไขข้อมูลสำเร็จ" + " " + EmpNo + " " + fullname);
                        reset();
                        dsDetail.RetrieveDetail(EmpNo);
                    }
                    else 
                    //if (dsLeave.DATA[0].SEQ_NO.ToString() == SeqNo.ToString())
                    {
                        string CoopId = state.SsCoopId;//unique
                        decimal SeqNo = 1;//unique
                        string sql2 = @"select max(seq_no)+1 as seq_no from hrlogleave where emp_no ={0} and coop_id={1}";//นำค่าแมกของ seq_no บวก 1 เพื่อให้เป็นค่า seq_no ของลำดับต่อไป
                        sql2 = WebUtil.SQLFormat(sql2, EmpNo, CoopId);//format ในรูปของ sql
                        Sdt dt2 = WebUtil.QuerySdt(sql2);
                        if (dt2.Next())
                        {
                            SeqNo = dt2.GetDecimal("seq_no");
                        }
                        dsLeave.DATA[0].SEQ_NO = SeqNo;//กำหนดค่าให้ ds.Leave >> SEQ_NO ใหม่ ให้เป็น ค่าใหม่ที่กำหนด จาก string sql select max(seq_no)+1
                        //SavePass ตัวดัก Eror
                        dsLeave.DATA[0].COOP_ID = coop_id;
                        ExecuteDataSource exe = new ExecuteDataSource(this);
                        exe.AddFormView(dsLeave, ExecuteType.Insert);
                        exe.Execute();

                        if (leave_code == "001")
                        {
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

                            if (leave_001_c == "")
                            {
                                leave_001_c = "0";
                            }
                            else
                            {

                                leave_001_c = leave_001_c;
                            }

                            if (emp_no_check == "")
                            {

                                string ls_sql_check = @"insert into hrcheckleave(emp_no,year,month,leave_001_s,leave_001_c,leave_002_s,leave_002_c,leave_004_s,leave_004_c,leave_006_s,leave_006_c,late_date)values({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11})";
                                ls_sql_check = WebUtil.SQLFormat(ls_sql_check, EmpNo, year, month, '0', '0', '0', '0', '0', '0', '0', '0', '0');
                                exed.SQL.Add(ls_sql_check);
                                exed.Execute();
                                exed.SQL.Clear();

                                sumday_leave001 = Convert.ToDecimal(todtalday) + 0;
                                count_leave001 = 1;

                                string sqlleave_001 = @"update hrcheckleave set leave_001_s = {0} , leave_001_c = {1} where emp_no={2} and year = {3} and month = {4}";
                                sqlleave_001 = WebUtil.SQLFormat(sqlleave_001, Convert.ToString(sumday_leave001), Convert.ToString(count_leave001), EmpNo, year, month);
                                Sdt dt_leave_001 = WebUtil.QuerySdt(sqlleave_001);

                            }
                            else
                            {
                                sumday_leave001 = Convert.ToDecimal(todtalday) + Convert.ToDecimal(leave_001_s);
                                count_leave001 = Convert.ToDecimal(leave_001_c) + 1;

                                string sqlleave_001 = @"update hrcheckleave set leave_001_s = {0} , leave_001_c = {1} where emp_no={2} and year = {3} and month = {4}";
                                sqlleave_001 = WebUtil.SQLFormat(sqlleave_001, Convert.ToString(sumday_leave001), Convert.ToString(count_leave001), EmpNo, year, month);
                                Sdt dt_leave_001 = WebUtil.QuerySdt(sqlleave_001);
                            }
                        }
                        else if (leave_code == "002")
                        {
                            string sql_hrcheckleave002 = @"select emp_no,leave_002_s,leave_002_c from hrcheckleave where emp_no = {0} and year = {1} and month = {2}";
                            sql_hrcheckleave002 = WebUtil.SQLFormat(sql_hrcheckleave002, EmpNo, year, month);
                            // Sdt dt = WebUtil.QuerySdt(sql);
                            Sdt dt_check002 = WebUtil.QuerySdt(sql_hrcheckleave002);
                            if (dt_check002.Next())
                            {
                                emp_no_check2 = dt_check002.GetString("emp_no");
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

                            if (emp_no_check2 == "")
                            {

                                string ls_sql_check2 = @"insert into hrcheckleave(emp_no,year,month,leave_001_s,leave_001_c,leave_002_s,leave_002_c,leave_004_s,leave_004_c,leave_006_s,leave_006_c,late_date)values({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11})";
                                ls_sql_check2 = WebUtil.SQLFormat(ls_sql_check2, EmpNo, year, month, '0', '0', '0', '0', '0', '0', '0', '0', '0');
                                exed.SQL.Add(ls_sql_check2);
                                exed.Execute();
                                exed.SQL.Clear();

                                sumday_leave002 = Convert.ToDecimal(todtalday) + 0;
                                count_leave002 = 1;

                                string sqlleave_002 = @"update hrcheckleave set leave_002_s = {0} , leave_002_c = {1} where emp_no={2} and year = {3} and month = {4}";
                                sqlleave_002 = WebUtil.SQLFormat(sqlleave_002, Convert.ToString(sumday_leave002), Convert.ToString(count_leave002), EmpNo, year, month);
                                Sdt dt_leave_002 = WebUtil.QuerySdt(sqlleave_002);

                            }
                            else
                            {
                                sumday_leave002 = Convert.ToDecimal(todtalday) + Convert.ToDecimal(leave_002_s);
                                count_leave002 = Convert.ToDecimal(leave_002_c) + 1;

                                string sqlleave_002 = @"update hrcheckleave set leave_002_s = {0} , leave_002_c = {1} where emp_no={2} and year = {3} and month = {4}";
                                sqlleave_002 = WebUtil.SQLFormat(sqlleave_002, Convert.ToString(sumday_leave002), Convert.ToString(count_leave002), EmpNo, year, month);
                                Sdt dt_leave_002 = WebUtil.QuerySdt(sqlleave_002);
                            }
                        }
                        else if (leave_code == "006")
                        {

                            string sql_hrcheckleave006 = @"select emp_no,leave_006_s,leave_006_c from hrcheckleave where emp_no = {0} and year = {1} and month = {2}";
                            sql_hrcheckleave006 = WebUtil.SQLFormat(sql_hrcheckleave006, EmpNo, year, month);
                            // Sdt dt = WebUtil.QuerySdt(sql);
                            Sdt dt_check006 = WebUtil.QuerySdt(sql_hrcheckleave006);
                            if (dt_check006.Next())
                            {
                                emp_no_check6 = dt_check006.GetString("emp_no");
                                leave_006_s = dt_check006.GetString("leave_006_s");
                                leave_006_c = dt_check006.GetString("leave_006_c");


                            }

                            if (leave_006_c == "")
                            {
                                leave_006_c = "0";
                            }
                            else
                            {

                                leave_006_c = leave_006_c;
                            }

                            if (emp_no_check6 == "")
                            {

                                string ls_sql_check6 = @"insert into hrcheckleave(emp_no,year,month,leave_001_s,leave_001_c,leave_002_s,leave_002_c,leave_004_s,leave_004_c,leave_006_s,leave_006_c,late_date)values({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11})";
                                ls_sql_check6 = WebUtil.SQLFormat(ls_sql_check6, EmpNo, year, month, '0', '0', '0', '0', '0', '0', '0', '0', '0');
                                exed.SQL.Add(ls_sql_check6);
                                exed.Execute();
                                exed.SQL.Clear();

                                sumday_leave006 = Convert.ToDecimal(todtalday) + 0;
                                count_leave006 =  1;

                                string sqlleave_006 = @"update hrcheckleave set leave_006_s = {0} , leave_006_c = {1} where emp_no={2} and year = {3} and month = {4}";
                                sqlleave_006 = WebUtil.SQLFormat(sqlleave_006, Convert.ToString(sumday_leave006), Convert.ToString(count_leave006), EmpNo, year, month);
                                Sdt dt_leave_006 = WebUtil.QuerySdt(sqlleave_006);

                            }
                            else
                            {
                                sumday_leave006 = Convert.ToDecimal(todtalday) + Convert.ToDecimal(leave_006_s);
                                count_leave006 = Convert.ToDecimal(leave_006_c) + 1;

                                string sqlleave_006 = @"update hrcheckleave set leave_006_s = {0} , leave_006_c = {1} where emp_no={2} and year = {3} and month = {4}";
                                sqlleave_006 = WebUtil.SQLFormat(sqlleave_006, Convert.ToString(sumday_leave006), Convert.ToString(count_leave006), EmpNo, year, month);
                                Sdt dt_leave_006 = WebUtil.QuerySdt(sqlleave_006);
                            }

                        }

                        LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกการลาสำเร็จ " + " " + EmpNo + " " + fullname);
                        reset();
                        dsDetail.RetrieveDetail(EmpNo);
                    }
                }

                dsDetail.RetrieveDetail(EmpNo);
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        private void reset()
        {
            dsMain.ResetRow();
            dsLeave.ResetRow();
            dsLeave.DdHrucfleavecode();
            dsLeave.DdHrucfleaveapvby();
        }

        public void WebSheetLoadEnd()
        {

        }

        public object LAS_EMWORK { get; set; }
    }
}