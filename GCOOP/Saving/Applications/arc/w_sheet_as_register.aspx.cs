using System;
using CoreSavingLibrary;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using DataLibrary;
using Sybase.DataWindow;
using System.Web.Services.Protocols;

namespace Saving.Applications.arc
{
    public partial class w_sheet_as_register : PageWebSheet, WebSheet
    {
        protected DwThDate tDwMain;
        protected String postProtect;
        protected String postCourseId;
        protected String postTraveling;
        protected String postFilterProjectId;
        protected String postHotel;
        protected String postGetDistance;
        protected String postRetrieveDwMain;
        protected String postGetMemberDetail;
        protected String postRoomRate;
        protected DataStore DStore;
        protected Decimal member_time, check_came, pro_type, dif_year, total_seat = 0, remain_seat = 0, retry_status;
        protected Double post_distance;

        private Sta ta;
        private Sdt dt;

        DataStore Ds;


        public void InitJsPostBack()
        {
            tDwMain = new DwThDate(DwMain, this);
            tDwMain.Add("slip_date", "slip_tdate");
            tDwMain.Add("birth_date", "birth_tdate");
            tDwMain.Add("member_date", "member_tdate");

            postProtect = WebUtil.JsPostBack(this, "postProtect");
            postCourseId = WebUtil.JsPostBack(this, "postCourseId");
            postFilterProjectId = WebUtil.JsPostBack(this, "postFilterProjectId");
            postTraveling = WebUtil.JsPostBack(this, "postTraveling");
            postHotel = WebUtil.JsPostBack(this, "postHotel");
            postGetDistance = WebUtil.JsPostBack(this, "postGetDistance");
            postRoomRate = WebUtil.JsPostBack(this, "postRoomRate");
            postRetrieveDwMain = WebUtil.JsPostBack(this, "postRetrieveDwMain");
            postGetMemberDetail = WebUtil.JsPostBack(this, "postGetMemberDetail");
        }

        public void WebSheetLoadBegin()
        {
            this.ConnectSQLCA();

            ta = new Sta(sqlca.ConnectionString);
            dt = new Sdt();

            Ds = new DataStore();

            if (!IsPostBack)
            {
                DwMain.InsertRow(0);
                DwUtil.RetrieveDDDW(DwMain, "project_id", "as_seminar.pbl", null);//เพิ่ม
                DwMain.SetItemDateTime(1, "slip_date", state.SsWorkDate);
                DwUtil.RetrieveDDDW(DwMain, "project_year", "as_seminar.pbl", null);

            }
            else
            {
                this.RestoreContextDw(DwMain);
            }
            DwUtil.RetrieveDDDW(DwMain, "membnew_grp", "as_seminar.pbl", null);
            DwUtil.RetrieveDDDW(DwMain, "project_id", "as_seminar.pbl", null);
            DwUtil.RetrieveDDDW(DwMain, "course_id", "as_seminar.pbl", null);
            tDwMain.Eng2ThaiAllRow();
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postCourseId")
            {
                CourseId();
            }
            else if (eventArg == "postRetrieveDwMain")
            {
                RetrieveDwMain();
            }
            else if (eventArg == "postFilterProjectId")
            {
                FilterProjectId();
            }
            else if (eventArg == "postProtect")
            {
                Protect();
            }
            else if (eventArg == "postHotel")
            {
                Hotel();
            }
            else if (eventArg == "postRoomRate")
            {
                RoomRate();
            }
            else if (eventArg == "postGetDistance")
            {
                GetDistance();
            }
            else if (eventArg == "postTraveling")
            {
                Traveling();
            }
            else if (eventArg == "postGetMemberDetail")
            {
                GetMemberDetail();
            }
        }

        public void SaveWebSheet()
        {
            Sta ta2 = new Sta(state.SsConnectionString);
            Sdt dt2 = new Sdt();
                   ta2.Transection();
                   dt2.Clear();

            String sqlStr, mem_no = "";
            String thaifull_name, thaisure_name, mem_tel, document_no, province_code;
            String member_no, slip_no, enrollment_docno, membgroup_code, project_id;
            Decimal seat_no = 0, root_flag, lanunch_flag, dine_flag, not_root_flag, not_lanunch_flag, course_id, not_dine_flag = 0, magnitude1_flag, magnitude2_flag, magnitude3_flag, magnitude4_flag;
            Decimal traveling_amt, traveling_km, room_amt, roomoutciti_amt, follower_flag, employee_flag, lunch_amt, roomoutciti_flag, traveling_flag, other_amt;
            DateTime slip_date, enroll_date;

            try
            {
                enrollment_docno = DwMain.GetItemString(1, "document_no");
            }
            catch { enrollment_docno = ""; }

            member_no = DwMain.GetItemString(1, "member_no");
            membgroup_code = DwMain.GetItemString(1, "membgroup_code");
            project_id = DwMain.GetItemString(1, "project_id");
            try
            {
                seat_no = DwMain.GetItemDecimal(1, "projectenrollment_seat_no");
            }
            catch { seat_no = 0; }
            course_id = DwMain.GetItemDecimal(1, "course_id");
            root_flag = DwMain.GetItemDecimal(1, "root_flag");
            lanunch_flag = DwMain.GetItemDecimal(1, "lanunch_flag");
            dine_flag = DwMain.GetItemDecimal(1, "dine_flag");
            follower_flag = DwMain.GetItemDecimal(1, "follower_flag");
            employee_flag = DwMain.GetItemDecimal(1, "employee_flag");
            traveling_amt = DwMain.GetItemDecimal(1, "traveling_amt");
            traveling_km = DwMain.GetItemDecimal(1, "traveling_km");
            room_amt = DwMain.GetItemDecimal(1, "room_amt");
            roomoutciti_amt = DwMain.GetItemDecimal(1, "roomoutciti_amt");
            roomoutciti_flag = DwMain.GetItemDecimal(1, "roomoutciti_flag");
            traveling_flag = DwMain.GetItemDecimal(1, "traveling_flag");
            lunch_amt = DwMain.GetItemDecimal(1, "lunch_amt");
            try
            {
                other_amt = DwMain.GetItemDecimal(1, "other_amt");
            }
            catch { other_amt = 0; }

            magnitude1_flag = DwMain.GetItemDecimal(1, "magnitude1_flag");
            magnitude2_flag = DwMain.GetItemDecimal(1, "magnitude2_flag");
            magnitude3_flag = DwMain.GetItemDecimal(1, "magnitude3_flag");
            magnitude4_flag = DwMain.GetItemDecimal(1, "magnitude4_flag");
            not_lanunch_flag = DwMain.GetItemDecimal(1, "not_lanunch_flag");
            not_root_flag = DwMain.GetItemDecimal(1, "not_root_flag");
            try
            {
                province_code = DwMain.GetItemString(1, "membnew_grp");
            }
            catch { province_code = ""; }
           
            try
            {
                slip_date = DwMain.GetItemDateTime(1, "slip_date");
            }
            catch
            {
                slip_date = state.SsWorkDate;
            }

            try
            {
                not_dine_flag = DwMain.GetItemDecimal(1, "not_dine_flag");
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
            thaifull_name = DwMain.GetItemString(1, "thaifull_name");
            thaisure_name = DwMain.GetItemString(1, "thaisure_name");
            try
            {
                mem_tel = DwMain.GetItemString(1, "mem_tel");
            }
            catch { mem_tel = ""; }

            GetCheckMaster(project_id, course_id);

            if (enrollment_docno == "")
            {
                try
                {
                    //หาเลขที่ slip ล่าสุด
                    sqlStr = "SELECT max(slip_no)as slip_no FROM projectslip";
                    dt2 = ta2.Query(sqlStr);
                    dt2.Next();
                    slip_no = dt2.GetString("slip_no");
                    if (slip_no == "")
                    {
                        slip_no = "000001";
                    }
                    else
                    {
                        slip_no = WebUtil.Right("000000" + Convert.ToString(Convert.ToInt32(slip_no) + 1), 6);
                    }

                    //หาเลขที่ enroll ล่าสุด
                    sqlStr = "SELECT max(enrollment_docno)as enrollment_docno FROM projectenrollment";
                    dt2 = ta2.Query(sqlStr);
                    dt2.Next();
                    enrollment_docno = dt2.GetString("enrollment_docno");
                    if (enrollment_docno == "")
                    {
                        enrollment_docno = "00000001";
                    }
                    else
                    {
                        enrollment_docno = Convert.ToString(Convert.ToInt32(enrollment_docno) + 1);
                        enrollment_docno = "00000000" + enrollment_docno;
                        enrollment_docno = WebUtil.Right(enrollment_docno, 8);
                    }

                    pro_type = DwMain.GetItemDecimal(1, "pro_type");
                    if (pro_type == 0)
                    {
                        //หาลำดับที่ในแต่ละจังหวัด
                        sqlStr = @"select count(*) as rowcount
                                from projectenrollment
                                where project_id = '" + project_id + @"' and
			                            course_id = " + course_id + @" and
		                                membnew_grp = '" + province_code + "'";
                        dt2.Clear();
                        dt2 = ta2.Query(sqlStr);
                        dt2.Next();
                        Decimal row = dt2.GetDecimal("rowcount");
                        seat_no = row + 1;
                    }

                    //อัพเดต เบอร์โทรของสมาชิก
                    //                    sqlStr = @"UPDATE mbmembmaster
                    //                               SET mem_tel = '" + mem_tel + @"'
                    //                               WHERE member_no = '" + member_no + "'";
                    //                    ta.Exe(sqlStr);

                    // insert ลง  projectenrollment
                    sqlStr = @"INSERT INTO projectenrollment(enrollment_docno  ,  member_no  ,  enroll_date  ,  membgroup_code  ,  project_id  ,  seat_no  ,  course_id  ,
                                                                 entry_date  ,  entry_id  ,  root_flag  ,  lanunch_flag  ,  dine_flag  ,  traveling_amt  ,  traveling_km  ,
                                                                 room_amt  ,  roomoutciti_amt  ,  magnitude1_flag  ,  magnitude2_flag  ,  magnitude3_flag  ,  magnitude4_flag  ,
                                                                 not_root_flag  ,  not_lanunch_flag  ,  not_dine_flag  ,  roomoutciti_flag  ,  traveling_flag  ,  lunch_amt  ,  other_amt  ,  membnew_grp)
                                            VALUES('" + enrollment_docno + "','" + member_no + "',to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy", WebUtil.EN) + "', 'dd/mm/yyyy'),'" + membgroup_code + "','" + project_id + "','" + seat_no + @"',
                                                   '" + course_id + "',to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy", WebUtil.EN) + "', 'dd/mm/yyyy'),'" + state.SsUsername + "','" + root_flag + "','" + lanunch_flag + "','" + dine_flag + @"',
                                                   '" + traveling_amt + "','" + traveling_km + "','" + room_amt + "','" + roomoutciti_amt + @"',
                                                   '" + magnitude1_flag + "','" + magnitude2_flag + "','" + magnitude3_flag + "','" + magnitude4_flag + @"',
                                                   '" + not_root_flag + "','" + not_lanunch_flag + "','" + not_dine_flag + "','" + roomoutciti_flag + "','" + traveling_flag + "','" + lunch_amt + "','" + other_amt + "','" + province_code + "')";
                    ta2.Exe(sqlStr);

                    // insert ลง  projectslip
                    sqlStr = @"INSERT INTO projectslip(slip_no  ,  branch_id  ,  enrollment_docno  ,  course_id  ,  project_id  ,  document_no  ,
                                                           member_no  ,  operate_date  ,  slip_date  ,  seat_no  ,  entry_date  ,  entry_id  ,
                                                            follower_flag  ,  thaifull_name  ,  thaisure_name  ,  employee_flag  ,  mem_tel  )
                                            VALUES('" + slip_no + "','" + state.SsCoopId + "','" + enrollment_docno + "','" + course_id + "','" + project_id + @"',
                                                   '" + enrollment_docno + "','" + member_no + "',to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy", WebUtil.EN) + @"', 'dd/mm/yyyy'),
                                                   to_date('" + slip_date.ToString("dd/MM/yyyy", WebUtil.EN) + "', 'dd/mm/yyyy'),'" + seat_no + @"',
                                                   to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy", WebUtil.EN) + "', 'dd/mm/yyyy'),'" + state.SsUsername + @"',
                                                   '" + follower_flag + "','" + thaifull_name + "','" + thaisure_name + "','" + employee_flag + "','" + mem_tel + "')";
                    ta2.Exe(sqlStr);

                    //อัพเดต จำนวนที่นั่ง
                    sqlStr = @"update projectcourse
                               set remain_seat = ((select remain_seat 
                                                  from   projectcourse 
                                                  where  project_id = '" + project_id + @"' and 
                                                          course_id = " + course_id + @") - 1)
                               where project_id = '" + project_id + @"' and
                                     course_id = " + course_id + "";
                    ta2.Exe(sqlStr);

                    //วนสร้างลำดับที่ใหม่ตามเลขสมาชิก
                    if (pro_type == 1)
                    {
                        Ds.LibraryList = WebUtil.PhysicalPath + @"Saving\DataWindow\app_assist\as_seminar.pbl";
                        Ds.DataWindowObject = "d_exec_project_enrollment_list_test";

                        Ds.SetTransaction(sqlca);
                        Ds.Retrieve(project_id, course_id, province_code);

                        //Ds.InsertRow(0);
                        //Ds.SetItemString(Ds.RowCount, "member_no", member_no);
                        Ds.SetSort("member_no");
                        Ds.Sort();
                        //int r = Ds.FindRow("member_no='" + member_no + "'", 1, Ds.RowCount);
                        //seat_no = r;
                        for (int i = 1; i <= Ds.RowCount; i++)
                        {
                            Ds.SetItemDecimal(i, "seat_no", i);
                        }
                        Ds.UpdateData();
                    }
                    ta2.Commit();
                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกเรียบร้อย");
                }
                catch (Exception ex)
                {
                    ta2.RollBack();
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                }
                //ta2.Close();
            }
            else if (enrollment_docno != "")
            {
                try
                {                  
                    String xml = DwMain.Describe("DataWindow.Data.XML");
                    DwMain.SetTransaction(sqlca);
                    DwMain.UpdateData();

                   


                    sqlStr = @"UPDATE projectslip
                               SET    slip_date = to_date('" + slip_date.ToString("dd/MM/yyyy", WebUtil.EN) + @"', 'dd/mm/yyyy'),
                                      follower_flag = '" + follower_flag + @"',
                                      employee_flag = '" + employee_flag + "'";
                    ta2.Exe(sqlStr); //ของเดิมไมมไม่ได้comment

                    sqlStr = @"UPDATE projectenrollment
                               SET    seat_no = '" + seat_no + @"'
                               where  project_id = '" + project_id + @"' and 
                                      course_id = '" + course_id + @"' and
                                      member_no = '" + member_no + "'";
                    ta2.Exe(sqlStr);  //ของเดิมไมมไม่ได้comment

                    try
                    {
                        DwUtil.UpdateDataWindow(DwMain, "as_seminar.pbl", "projectenrollment");
                    }
                    catch { }

                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกเรียบร้อย");
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                }
            }

            ta2.Close();
            dt2.Clear();
        }

        public void WebSheetLoadEnd()
        {
            ta.Close();
            dt.Clear();
            DwMain.SaveDataCache();
            tDwMain.Eng2ThaiAllRow();
            if (HfMemberNo.Value != "")
            {
                DwMain.SetRow(Convert.ToInt16(HfMemberNo.Value));
                DwMain.Focus();
            }
        }



        private void Hotel()
        {
            Decimal root_flag;

            root_flag = DwMain.GetItemDecimal(1, "root_flag");

            if (root_flag == 1)
            {
                DwMain.SetItemDecimal(1, "roomoutciti_amt", 0);
            }
            else if (root_flag == 0)
            {
                RoomRate();
            }
        }

        private void Protect()
        {
            Decimal follower_flag, employee_flag;
            Decimal lanunch_flag_1, root_flag_1, dine_flag_1;
            Decimal not_lanunch_flag, not_root_flag, not_dine_flag;

            follower_flag = DwMain.GetItemDecimal(1, "follower_flag");
            employee_flag = DwMain.GetItemDecimal(1, "employee_flag");

            lanunch_flag_1 = DwMain.GetItemDecimal(1, "lanunch_flag_1");
            root_flag_1 = DwMain.GetItemDecimal(1, "root_flag_1");
            dine_flag_1 = DwMain.GetItemDecimal(1, "dine_flag_1");
            not_lanunch_flag = DwMain.GetItemDecimal(1, "not_lanunch_flag");
            not_root_flag = DwMain.GetItemDecimal(1, "not_root_flag");
            not_dine_flag = DwMain.GetItemDecimal(1, "not_dine_flag");

            if (follower_flag == 1)
            {
                DwMain.SetItemDecimal(1, "employee_flag", 0);
                DwMain.SetItemDecimal(1, "follower_flag", 1);
            }
            if (employee_flag == 1)
            {
                DwMain.SetItemDecimal(1, "follower_flag", 0);
                DwMain.SetItemDecimal(1, "employee_flag", 1);
            }
            if (lanunch_flag_1 == 1)
            {
                DwMain.SetItemDecimal(1, "not_lanunch_flag", 0);
                DwMain.SetItemDecimal(1, "lanunch_flag_1", 1);
            }
            if (not_lanunch_flag == 1)
            {
                DwMain.SetItemDecimal(1, "not_lanunch_flag", 1);
                DwMain.SetItemDecimal(1, "lanunch_flag_1", 0);
            }
            if (root_flag_1 == 1)
            {
                DwMain.SetItemDecimal(1, "not_root_flag", 0);
                DwMain.SetItemDecimal(1, "root_flag_1", 1);
            }
            if (not_root_flag == 1)
            {
                DwMain.SetItemDecimal(1, "not_root_flag", 1);
                DwMain.SetItemDecimal(1, "root_flag_1", 0);
            }
            if (dine_flag_1 == 1)
            {
                DwMain.SetItemDecimal(1, "not_dine_flag", 0);
                DwMain.SetItemDecimal(1, "dine_flag_1", 1);
            }
            if (not_dine_flag == 1)
            {
                DwMain.SetItemDecimal(1, "not_dine_flag", 1);
                DwMain.SetItemDecimal(1, "dine_flag_1", 0);
            }
        }

        private void CourseId()
        {
            String sqlStr, envvalue, project_id;
            Decimal course_id;
            DateTime startcourse_date, endcourse_date;

            project_id = DwMain.GetItemString(1, "project_id");

            object[] ArgProjectId = new object[1];
            ArgProjectId[0] = project_id;
            DwUtil.RetrieveDDDW(DwMain, "course_id", "as_seminar.pbl", ArgProjectId);

            try
            {
                course_id = DwMain.GetItemDecimal(1, "course_id");
            }
            catch { course_id = 0; }

            if (project_id != "" && course_id != 0)
            {
                GetCheckMaster(project_id, course_id);

                if (remain_seat == 0)
                {
                    LtServerMessage.Text = WebUtil.WarningMessage("จำนวนที่นั่งเต็มแล้ว");
                    return;
                }
            }

            dt.Clear();
            //หาว่าเป็นส่วนกลางหรือภูมิภาค
            sqlStr = @"SELECT pro_type
                       FROM PROJECTMASTER
                       WHERE project_id = '" + project_id + "'";
            dt = ta.Query(sqlStr);
            dt.Next();
            pro_type = dt.GetDecimal("pro_type");
            DwMain.SetItemDecimal(1, "pro_type", pro_type);

            if (pro_type == 1)
            {
                DwMain.SetItemDecimal(1, "root_flag", 0);
                DwMain.SetItemDecimal(1, "traveling_flag", 1);
                DwMain.SetItemDecimal(1, "lunch_amt", 0);
                Traveling();
            }
            else if (pro_type == 0)
            {
                DwMain.SetItemDecimal(1, "root_flag", 1);
                DwMain.SetItemDecimal(1, "traveling_flag", 2);

                dt.Clear();
                //ค่าอาหารกลางวัน
                sqlStr = @"SELECT envvalue
                       FROM PROJECTSENVIRONMENTVAR
                       WHERE envcode = '02'";
                dt = ta.Query(sqlStr);
                dt.Next();
                envvalue = dt.GetString("envvalue");
                DwMain.SetItemDecimal(1, "lunch_amt", Convert.ToDecimal(envvalue));
            }

            //ที่พักสหกรณ์จัดให้
            sqlStr = @"SELECT envvalue
                       FROM PROJECTSENVIRONMENTVAR
                       WHERE envcode = '05'";
            dt = ta.Query(sqlStr);
            dt.Next();
            envvalue = dt.GetString("envvalue");
            //DwMain.SetItemDecimal(1, "room_amt", Convert.ToDecimal(envvalue));


            //ค่าที่พักจัดหาเองในเขต
            sqlStr = @"SELECT envvalue
                       FROM PROJECTSENVIRONMENTVAR
                       WHERE envcode = '06'";
            dt.Clear();
            dt = ta.Query(sqlStr);
            dt.Next();
            envvalue = dt.GetString("envvalue");
            //DwMain.SetItemDecimal(1, "roomoutciti_amt", Convert.ToDecimal(envvalue));

            dt.Clear();
            //ค่าเดินทางส่วนกลาง
            sqlStr = @"SELECT envvalue
                       FROM PROJECTSENVIRONMENTVAR
                       WHERE envcode = '07'";
            dt = ta.Query(sqlStr);
            dt.Next();
            envvalue = dt.GetString("envvalue");
            //DwMain.SetItemDecimal(1, "traveling_amt", Convert.ToDecimal(envvalue));

            dt.Clear();
            sqlStr = @"SELECT startcourse_date,endcourse_date,total_seat,remain_seat
                       FROM PROJECTCOURSE
                       WHERE project_id = '" + project_id + @"' and
                             course_id  = '" + course_id + "'";
            dt = ta.Query(sqlStr);
            dt.Next();
            startcourse_date = dt.GetDate("startcourse_date");
            endcourse_date = dt.GetDate("endcourse_date");
            total_seat = Convert.ToDecimal(dt.GetDouble("total_seat"));
            remain_seat = Convert.ToDecimal(dt.GetDouble("remain_seat"));

            DwMain.SetItemDateTime(1, "startcourse_date", startcourse_date);
            DwMain.SetItemDateTime(1, "endcourse_date", endcourse_date);
            DwMain.SetItemDecimal(1, "total_seat", total_seat);
            DwMain.SetItemDecimal(1, "remain_seat", remain_seat);
        }

        private void Traveling()
        {
            String sqlStr, envvalue;
            Decimal traveling_flag;

            traveling_flag = DwMain.GetItemDecimal(1, "traveling_flag");

            if (traveling_flag == 1)
            {
                sqlStr = @"SELECT envvalue
                       FROM PROJECTSENVIRONMENTVAR
                       WHERE envcode = '07'";
                dt = ta.Query(sqlStr);
                dt.Next();
                envvalue = dt.GetString("envvalue");
                DwMain.SetItemDecimal(1, "traveling_amt", Convert.ToDecimal(envvalue));

                DwMain.SetItemDecimal(1, "lunch_amt", 0);
            }
            else if (traveling_flag == 2)
            {
                GetDistance();
            }
        }

        private void GetDistance()
        {
            String sqlStr, member_no, postcode = "";
            String project_id, proj_post_code, membgroup_code;
            Decimal course_id;
            Int32 prize;

            member_no = DwMain.GetItemString(1, "member_no");
            project_id = DwMain.GetItemString(1, "project_id");
            course_id = DwMain.GetItemDecimal(1, "course_id");
            membgroup_code = DwMain.GetItemString(1, "membgroup_code").Trim();

            sqlStr = @"SELECT postcode,membgroup_code
                       FROM mbmembmaster
                       WHERE member_no = '" + member_no + "'";
            dt = ta.Query(sqlStr);
            dt.Next();
            if (retry_status != 1)
            {
                postcode = dt.GetString("membgroup_code");
            }
            else if (retry_status == 1)
            {
                postcode = dt.GetString("postcode");
            }

            //หาสถานที่จัดสัมมนา
            sqlStr = @"SELECT proj_post_code
                       FROM projectcourse
                       WHERE project_id = '" + project_id + @"' and 
                             course_id  = '" + course_id + "'";
            dt.Clear();
            dt = ta.Query(sqlStr);
            dt.Next();
            proj_post_code = dt.GetString("proj_post_code");

            //หาระยะทาง
            sqlStr = @"SELECT post_distance
                       FROM project_post_distance
                       WHERE post_satart_code = '" + postcode + @"' and
                             post_end_code = '" + proj_post_code + "'";
            dt.Clear();
            dt = ta.Query(sqlStr);
            dt.Next();
            post_distance = dt.GetDouble("post_distance");
            if (post_distance > 0)
            {
                RoomRate();
            }

            //หาค่าเดินทางต่อกิโล
            sqlStr = @"SELECT envvalue
                       FROM PROJECTSENVIRONMENTVAR
                       WHERE envcode = '01'";
            dt.Clear();
            dt = ta.Query(sqlStr);
            dt.Next();
            prize = Convert.ToInt32(dt.GetString("envvalue"));
            DwMain.SetItemDouble(1, "traveling_km", post_distance);

            post_distance = post_distance * prize * 2;
            if (post_distance < 200)
            {
                post_distance = 200;
            }
            else
            {
                post_distance = Math.Round(post_distance);
            }
            DwMain.SetItemDouble(1, "traveling_amt_1", post_distance);
        }

        private void RetrieveDwMain()
        {
            String project_id, member_no, course_id, document_no;

            project_id = DwMain.GetItemString(1, "project_id");
            member_no = DwMain.GetItemString(1, "member_no");
            course_id = Convert.ToString(DwMain.GetItemDecimal(1, "course_id"));


            DwMain.SetTransaction(sqlca);
            DwMain.Reset();
            DwMain.Retrieve(member_no, project_id, course_id);

            if (DwMain.RowCount < 1)
            {
                DwMain.Reset();
                DwMain.InsertRow(0);
            }
        }

        private void GetMemberDetail()
        {
            Decimal member_year, course_id, between_year;
            String member_no, prename_desc, memb_name, memb_surname, membgroup_code, membgroup_desc, mem_tel, project_id;
            DateTime member_date, birth_date, enroll_date;
            member_no = HfMemberNo.Value;

            Ds.LibraryList = WebUtil.PhysicalPath + @"Saving\DataWindow\app_assist\as_seminar.pbl";
            Ds.DataWindowObject = "d_member_detail";

            Ds.SetTransaction(sqlca);
            Ds.Retrieve(member_no);

            project_id = DwMain.GetItemString(1, "project_id");
            course_id = DwMain.GetItemDecimal(1, "course_id");

            prename_desc = Ds.GetItemString(1, "prename_desc");
            memb_name = Ds.GetItemString(1, "memb_name");
            memb_surname = Ds.GetItemString(1, "memb_surname");
            membgroup_code = Ds.GetItemString(1, "membgroup_code");
            membgroup_desc = Ds.GetItemString(1, "membgroup_desc");
            member_date = Ds.GetItemDateTime(1, "member_date");
            retry_status = Ds.GetItemDecimal(1, "retry_status");
            try
            {
                birth_date = Ds.GetItemDateTime(1, "birth_date");
            }
            catch
            {
                birth_date = DateTime.Now;
            }
            try
            {
                mem_tel = Ds.GetItemString(1, "mem_tel");
            }
            catch { mem_tel = ""; }

            //เช็คอายุสมาชิก
            GetCheckMaster(project_id, course_id);
            if (birth_date != DateTime.Now)
            {
                member_year = DateTime.Now.Year - birth_date.Year;
                if (member_year < member_time)
                {
                    LtServerMessage.Text = WebUtil.WarningMessage("สมาชิกรายนี้มีอายุไม่ถึงที่กำหนดไว้");
                }
            }
            //เช็คโครงการไม่ตรง
            GetCheckMaster(project_id, course_id);
            member_no = HfMemberNo.Value;
            if (project_id != member_no)
            {
                DataStore DsEnroll = new DataStore();
                DsEnroll.LibraryList = WebUtil.PhysicalPath + @"Saving\DataWindow\app_assist\as_seminar.pbl";
                DsEnroll.DataWindowObject = "d_check_enroll";

                DsEnroll.SetTransaction(sqlca);
                DsEnroll.Retrieve(member_no, project_id, course_id);

                try
                {
                    member_no = DsEnroll.GetItemString(1, "member_no");
                    if (member_no == project_id)
                    {
                        LtServerMessage.Text = WebUtil.WarningMessage("เช็คโครงการไม่ตรง");
                    }
                }
                catch { }
            }

            //เช็คว่าเคยไปสัมมนาแล้วหรือยัง
            if (check_came == 1)
            {
                DataStore DsEnroll = new DataStore();
                DsEnroll.LibraryList = WebUtil.PhysicalPath + @"Saving\DataWindow\app_assist\as_seminar.pbl";
                DsEnroll.DataWindowObject = "d_check_enroll";

                DsEnroll.SetTransaction(sqlca);
                DsEnroll.Retrieve(member_no, project_id, course_id);

                try
                {
                    enroll_date = DsEnroll.GetItemDateTime(1, "enroll_date");
                    between_year = DateTime.Now.Year - enroll_date.Year;
                    if (between_year < dif_year)
                    {
                        LtServerMessage.Text = WebUtil.WarningMessage("สมาชิกรายนี้ได้เข้าร่วมโครงการแล้วในปี" + " " + (Convert.ToInt32(enroll_date.Year) + 543));
                    }
                }
                catch { }
            }


            DwMain.SetItemString(1, "member_no", member_no);
            DwMain.SetItemString(1, "thaifull_name", memb_name);
            DwMain.SetItemString(1, "thaisure_name", memb_surname);
            DwMain.SetItemString(1, "membgroup_desc", membgroup_desc);
            DwMain.SetItemString(1, "membgroup_code", membgroup_code);
            DwMain.SetItemDateTime(1, "member_date", member_date);
            if (birth_date != DateTime.Now)
            {
                DwMain.SetItemDateTime(1, "birth_date", birth_date);
            }
            DwMain.SetItemString(1, "mem_tel", mem_tel);
            tDwMain.Eng2ThaiAllRow();

            Decimal traveling_flag = DwMain.GetItemDecimal(1, "traveling_flag");

            if (traveling_flag == 2)
            {
                GetDistance();
            }
            Ds.Reset();
        }

        private void RoomRate()
        {
            Decimal roomoutciti_flag, root_flag;
            String sqlStr, envvalue;

            roomoutciti_flag = DwMain.GetItemDecimal(1, "roomoutciti_flag");
            root_flag = DwMain.GetItemDecimal(1, "root_flag");

            if (roomoutciti_flag == 1 && root_flag == 0)
            {
                DwMain.SetItemDecimal(1, "roomoutciti_amt", 0);
            }
            else if (roomoutciti_flag == 2 && root_flag == 0)
            {
                sqlStr = @"SELECT envvalue
                       FROM PROJECTSENVIRONMENTVAR
                       WHERE envcode = '06'";
                dt = ta.Query(sqlStr);
                dt.Next();
                envvalue = dt.GetString("envvalue");
                DwMain.SetItemDecimal(1, "roomoutciti_amt", Convert.ToDecimal(envvalue));
            }
        }

        private void GetCheckMaster(String project_id, Decimal course_id)
        {
            DStore = new DataStore();
            DStore.LibraryList = WebUtil.PhysicalPath + @"Saving\DataWindow\app_assist\as_seminar.pbl";
            DStore.DataWindowObject = "d_check_master";

            DStore.SetTransaction(sqlca);
            DStore.Retrieve(project_id, course_id); 

            try
            {
                member_time = DStore.GetItemDecimal(1, "member_time");
            }
            catch { member_time = 0; }
            try
            {
                check_came = DStore.GetItemDecimal(1, "check_came");
            }
            catch { check_came = 0; }
            try
            {
                dif_year = DStore.GetItemDecimal(1, "dif_year");
            }
            catch { dif_year = 0; }
            try
            {
                remain_seat = DStore.GetItemDecimal(1, "remain_seat");
            }
            catch { remain_seat = 0; }
        }

        private void FilterProjectId()
        {
            Decimal project_year;

            project_year = DwMain.GetItemDecimal(1, "project_year");

            object[] args = new object[1];
            args[0] = project_year;

            DwUtil.RetrieveDDDW(DwMain, "project_id", "as_seminar.pbl", null);
        }
    }
}
