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

namespace Saving.Applications.app_assist
{
    public partial class w_sheet_as_register_jobs : PageWebSheet, WebSheet
    {
        protected String postCourseId;
        protected String postRetrieveDwMain;
        protected String postProtect;
        protected String postGetDistance;
        protected String postTraveling;
        protected String postGetMemberDetail;
        protected DwThDate tDwMain;

        public void InitJsPostBack()
        {
            tDwMain = new DwThDate(DwMain, this);
            tDwMain.Add("birth_date", "birth_tdate");
            tDwMain.Add("member_date", "member_tdate");
            tDwMain.Add("slip_date", "slip_tdate");
            postProtect = WebUtil.JsPostBack(this, "postProtect");
            postCourseId = WebUtil.JsPostBack(this, "postCourseId");
            postGetDistance = WebUtil.JsPostBack(this, "postGetDistance");
            postTraveling = WebUtil.JsPostBack(this, "postTraveling");
            postRetrieveDwMain = WebUtil.JsPostBack(this, "postRetrieveDwMain");
            postGetMemberDetail = WebUtil.JsPostBack(this, "postGetMemberDetail");
        }

        public void WebSheetLoadBegin()
        {
            this.ConnectSQLCA();

            if (!IsPostBack)
            {
                DwMain.InsertRow(0);
                DwUtil.RetrieveDDDW(DwMain, "project_id", "as_seminar.pbl", null);
            }
            else
            {
                this.RestoreContextDw(DwMain);
            }
            DwMain.SetItemDateTime(1, "slip_date", state.SsWorkDate);
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
            else if (eventArg == "postProtect")
            {
                Protect();
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

        private void GetMemberDetail()
        {
            String sqlStr;
            String member_no, prename_desc, memb_name, memb_surname, membgroup_code, membgroup_desc;
            DateTime member_date, birth_date;
            member_no = HfMemberNo.Value;

            DataStore Ds = new DataStore();
            Ds.LibraryList = "C:/GCOOP/Saving/DataWindow/app_assist/as_seminar.pbl";
            Ds.DataWindowObject = "d_member_detail";

            Ds.SetTransaction(sqlca);
            Ds.Retrieve(member_no);

            prename_desc = Ds.GetItemString(1, "prename_desc");
            memb_name = Ds.GetItemString(1, "memb_name");
            memb_surname = Ds.GetItemString(1, "memb_surname");
            membgroup_code = Ds.GetItemString(1, "membgroup_code");
            membgroup_desc = Ds.GetItemString(1, "membgroup_desc");
            member_date = Ds.GetItemDateTime(1, "member_date");
            birth_date = Ds.GetItemDateTime(1, "birth_date");

            DwMain.SetItemString(1, "member_no", member_no);
            DwMain.SetItemString(1, "thaifull_name", memb_name);
            DwMain.SetItemString(1, "thaisure_name", memb_surname);
            DwMain.SetItemString(1, "membgroup_desc", membgroup_desc);
            DwMain.SetItemString(1, "membgroup_code", membgroup_code);
            DwMain.SetItemDateTime(1, "member_date", member_date);
            DwMain.SetItemDateTime(1, "birth_date", birth_date);
            tDwMain.Eng2ThaiAllRow();
        }

        public void SaveWebSheet()
        {
            String sqlStr;
            String thaifull_name, thaisure_name, mem_tel, document_no;
            String member_no, slip_no, enrollment_docno, membgroup_code, project_id, seat_no, course_id;
            Decimal root_flag, lanunch_flag, dine_flag, not_root_flag, not_lanunch_flag, not_dine_flag = 0, magnitude1_flag, magnitude2_flag, magnitude3_flag, magnitude4_flag;
            Decimal traveling_amt, traveling_km, room_amt, roomoutciti_amt, follower_flag, employee_flag, lunch_amt, other_amt , roomoutciti_flag, traveling_flag;

            Sta ta = new Sta(sqlca.ConnectionString);
            Sdt dt = new Sdt();

            try
            {
                enrollment_docno = DwMain.GetItemString(1, "enrollment_docno");
            }
            catch { enrollment_docno = ""; }

            member_no = DwMain.GetItemString(1, "member_no");
            membgroup_code = DwMain.GetItemString(1, "membgroup_code");
            project_id = DwMain.GetItemString(1, "project_id");
            seat_no = DwMain.GetItemString(1, "seat_no");
            course_id = DwMain.GetItemString(1, "course_id");
            root_flag = DwMain.GetItemDecimal(1, "root_flag_1");
            lanunch_flag = DwMain.GetItemDecimal(1, "lanunch_flag_1");
            dine_flag = DwMain.GetItemDecimal(1, "dine_flag_1");
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
                not_dine_flag = DwMain.GetItemDecimal(1, "not_dine_flag");
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
            thaifull_name = DwMain.GetItemString(1, "thaifull_name");
            thaisure_name = DwMain.GetItemString(1, "thaisure_name");
            mem_tel = DwMain.GetItemString(1, "mem_tel");

            if (enrollment_docno == "")
            {
                try
                {
                    sqlStr = "SELECT max(slip_no)as slip_no FROM projectslip";
                    dt = ta.Query(sqlStr);
                    dt.Next();
                    slip_no = dt.GetString("slip_no");
                    slip_no = Convert.ToString(Convert.ToInt32(slip_no) + 1);

                    sqlStr = "SELECT max(enrollment_docno)as enrollment_docno FROM projectenrollment";
                    dt = ta.Query(sqlStr);
                    dt.Next();
                    enrollment_docno = dt.GetString("enrollment_docno");
                    enrollment_docno = Convert.ToString(Convert.ToInt32(enrollment_docno) + 1);

                    sqlStr = @"INSERT INTO projectenrollment(enrollment_docno  ,  member_no  ,  enroll_date  ,  membgroup_code  ,  project_id  ,  seat_no  ,  course_id  ,
                                                     entry_date  ,  entry_id  ,  root_flag  ,  lanunch_flag  ,  dine_flag  ,  traveling_amt  ,  traveling_km  ,
                                                     room_amt  ,  roomoutciti_amt  ,  magnitude1_flag  ,  magnitude2_flag  ,  magnitude3_flag  ,  magnitude4_flag  ,
                                                     not_root_flag  ,  not_lanunch_flag  ,  not_dine_flag  ,  roomoutciti_flag  ,  traveling_flag  ,  lunch_amt , other_amt)
                                VALUES('" + enrollment_docno + "','" + member_no + "',to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy", WebUtil.EN) + "', 'dd/mm/yyyy'),'" + membgroup_code + "','" + project_id + "','" + seat_no + @"',
                                       '" + course_id + "',to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy", WebUtil.EN) + "', 'dd/mm/yyyy'),'" + state.SsUsername + "','" + root_flag + "','" + lanunch_flag + "','" + dine_flag + @"',
                                       '" + traveling_amt + "','" + traveling_km + "','" + room_amt + "','" + roomoutciti_amt + @"',
                                       '" + magnitude1_flag + "','" + magnitude2_flag + "','" + magnitude3_flag + "','" + magnitude4_flag + @"',
                                       '" + not_root_flag + "','" + not_lanunch_flag + "','" + not_dine_flag + "','" + roomoutciti_flag + "','" + traveling_flag + "','" + lunch_amt + "','" + other_amt + "')";
                    ta.Exe(sqlStr);

                    sqlStr = @"INSERT INTO projectslip(slip_no  ,  branch_id  ,  enrollment_docno  ,  course_id  ,  project_id  ,  document_no  ,
                                               member_no  ,  operate_date  ,  slip_date  ,  seat_no  ,  entry_date  ,  entry_id  ,
                                                follower_flag  ,  thaifull_name  ,  thaisure_name  ,  employee_flag  ,  mem_tel  )
                                VALUES('" + slip_no + "','" + state.SsCoopId + "','" + enrollment_docno + "','" + course_id + "','" + project_id + @"',
                                       '" + enrollment_docno + "','" + member_no + "',to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy", WebUtil.EN) + @"', 'dd/mm/yyyy'),
                                       to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy", WebUtil.EN) + "', 'dd/mm/yyyy'),'" + seat_no + @"',
                                       to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy", WebUtil.EN) + "', 'dd/mm/yyyy'),'" + state.SsUsername + @"',
                                       '" + follower_flag + "','" + thaifull_name + "','" + thaisure_name + "','" + employee_flag + "','" + mem_tel + "')";
                    ta.Exe(sqlStr);

                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกเรียบร้อย");
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                }
            }
            else if (enrollment_docno != "")
            {
                try
                {
                    DwMain.SetTransaction(sqlca);
                    DwMain.UpdateData();

                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกเรียบร้อย");
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                }
            }
        }

        public void WebSheetLoadEnd()
        {
            DwMain.SaveDataCache();
        }



        private void Traveling()
        {
            String sqlStr, envvalue;
            Decimal traveling_flag;

            Sta ta = new Sta(sqlca.ConnectionString);
            Sdt dt = new Sdt();

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
            }
            else if (traveling_flag == 2)
            {
                GetDistance();
            }
        }

        private void GetDistance()
        {
            String sqlStr, member_no, postcode;
            String project_id, proj_post_code, membgroup_code;
            Decimal course_id;
            Double post_distance;

            Sta ta = new Sta(sqlca.ConnectionString);
            Sdt dt = new Sdt();
            Sdt dt2 = new Sdt();
            Sdt dt3 = new Sdt();

            member_no = DwMain.GetItemString(1, "member_no");
            project_id = DwMain.GetItemString(1, "project_id");
            course_id = DwMain.GetItemDecimal(1, "course_id");
            membgroup_code = DwMain.GetItemString(1, "membgroup_code");

            sqlStr = @"SELECT postcode,membgroup_code
                       FROM mbmembmaster
                       WHERE member_no = '" + member_no + "'";
            dt = ta.Query(sqlStr);
            dt.Next();
            postcode = dt.GetString("postcode");
            if (postcode == "")
            {
                postcode = membgroup_code.Trim();
            }

            sqlStr = @"SELECT proj_post_code
                       FROM projectcourse
                       WHERE project_id = '" + project_id + @"' and 
                             course_id  = '" + course_id + "'";
            dt2 = ta.Query(sqlStr);
            dt2.Next();
            proj_post_code = dt2.GetString("proj_post_code");

            sqlStr = @"SELECT post_distance
                       FROM project_post_distance
                       WHERE post_satart_code = '" + postcode + @"' and
                             post_end_code = '" + proj_post_code + "'";
            dt3 = ta.Query(sqlStr);
            dt3.Next();
            post_distance = dt3.GetDouble("post_distance");

            DwMain.SetItemDouble(1, "traveling_km", post_distance);
            post_distance = post_distance * 3;
            if (post_distance < 200)
            {
                post_distance = 200;
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
            DwMain.Retrieve(member_no, project_id, course_id);

            if (DwMain.RowCount < 1)
            {
                DwMain.Reset();
                DwMain.InsertRow(0);
            }
        }

        private void CourseId()
        {
            String sqlStr, envvalue;

            Sta ta = new Sta(sqlca.ConnectionString);
            Sdt dt = new Sdt();
            Sdt dt2 = new Sdt();
            Sdt dt3 = new Sdt();

            String project_id = DwMain.GetItemString(1, "project_id");
            object[] ArgProjectId = new object[1] { project_id };
            DwUtil.RetrieveDDDW(DwMain, "course_id", "as_seminar.pbl", ArgProjectId);

            sqlStr = @"SELECT envvalue
                       FROM PROJECTSENVIRONMENTVAR
                       WHERE envcode = '05'";
            dt = ta.Query(sqlStr);
            dt.Next();

            envvalue = dt.GetString("envvalue");
            DwMain.SetItemDecimal(1, "room_amt", Convert.ToDecimal(envvalue));

            sqlStr = @"SELECT envvalue
                       FROM PROJECTSENVIRONMENTVAR
                       WHERE envcode = '03'";
            dt2 = ta.Query(sqlStr);
            dt2.Next();
            envvalue = dt2.GetString("envvalue");
            DwMain.SetItemDecimal(1, "roomoutciti_amt", Convert.ToDecimal(envvalue));

            sqlStr = @"SELECT envvalue
                       FROM PROJECTSENVIRONMENTVAR
                       WHERE envcode = '07'";
            dt3 = ta.Query(sqlStr);
            dt3.Next();
            envvalue = dt3.GetString("envvalue");
            DwMain.SetItemDecimal(1, "traveling_amt", Convert.ToDecimal(envvalue));

            sqlStr = @"SELECT envvalue
                       FROM PROJECTSENVIRONMENTVAR
                       WHERE envcode = '02'";
            dt3 = ta.Query(sqlStr);
            dt3.Next();
            envvalue = dt3.GetString("envvalue");
            DwMain.SetItemDecimal(1, "lunch_amt", Convert.ToDecimal(envvalue));
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
    }
}
