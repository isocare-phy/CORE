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
    public partial class w_sheet_as_send_invite : PageWebSheet, WebSheet
    {
        protected String postSearchList;
        protected String postRefresh;
        protected String postInvite;
        protected String postcheckAll;
        protected String postcheckAllpostcode;
        protected String postChangeType;
        protected String postFraction;



        public void InitJsPostBack()
        {
            postRefresh = WebUtil.JsPostBack(this, "postRefresh");
            postInvite = WebUtil.JsPostBack(this, "postInvite");
            postSearchList = WebUtil.JsPostBack(this, "postSearchList");
            postcheckAll = WebUtil.JsPostBack(this, "postcheckAll");
            postcheckAllpostcode = WebUtil.JsPostBack(this, "postcheckAllpostcode");
            postChangeType = WebUtil.JsPostBack(this, "postChangeType");
            postFraction = WebUtil.JsPostBack(this, "postFraction");
        }

        public void WebSheetLoadBegin()
        {
            this.ConnectSQLCA();

            if (!IsPostBack)
            {
                try
                {
                    DwProList.SetTransaction(sqlca);
                    DwProList.Retrieve();
                    DwMain.InsertRow(0);
                    DwList.SetTransaction(sqlca);
                    //DwList.Retrieve();
                    DwUtil.RetrieveDDDW(DwMain, "membgroup_no_begin", "as_seminar.pbl", null);
                    DwUtil.RetrieveDDDW(DwMain, "membgroup_no_end", "as_seminar.pbl", null);
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                }
            }
            else
            {
                this.RestoreContextDw(DwMain);
                this.RestoreContextDw(DwList);
                this.RestoreContextDw(DwListMem);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postSearchList")
            {
                SearchList();
            }
            else if (eventArg == "postRefresh")
            {
                Refresh();
            }
            else if (eventArg == "postFraction")
            {
                Fraction();
            }
            else if (eventArg == "postcheckAllpostcode")
            {
                checkAllpostcode();
            }
            else if (eventArg == "postInvite")
            {
                Invite();
            }
            else if (eventArg == "postcheckAll")
            {
                JsCheckAll();
            }
            else if (eventArg == "postChangeType")
            {
                ChangeType();
            }
        }

        public void SaveWebSheet()
        {

        }

        public void WebSheetLoadEnd()
        {
            this.DisConnectSQLCA();
            DwList.SaveDataCache();
            DwMain.SaveDataCache();
            DwListMem.SaveDataCache();
        }





        private void Fraction()
        {
            String memb_ctrl, memb_ctrl2;
            Decimal f1, f2, countmem, result, max;

            int row = Convert.ToInt32(HfRow.Value);

            f1 = DwList.GetItemDecimal(row, "fraction");
            f2 = DwList.GetItemDecimal(row, "fraction2");

            memb_ctrl = DwList.GetItemString(row, "sapgrp_control");
            max = DwList.GetItemDecimal(row, "max");

            for (int i = 1; i <= DwList.RowCount; i++)
            {
                memb_ctrl2 = DwList.GetItemString(i, "sapgrp_control");
                countmem = DwList.GetItemDecimal(i, "countmem");

                if (memb_ctrl2 == memb_ctrl)
                {
                    result = countmem / f1;
                    if (result > max && max != 0)
                    {
                        result = max;
                    }
                    result = Math.Round(result, 0, MidpointRounding.AwayFromZero);
                    if (result == 0)
                    {
                        result = 1;
                    }
                    DwList.SetItemDecimal(i, "inv_num", result);
                }
            }
        }

        private void ChangeType()
        {
            Decimal mem_type;

            mem_type = DwMain.GetItemDecimal(1, "mem_type");

            if (mem_type == 1)
            {
                DwList.Reset();
                DwList.InsertRow(0);
                DwListMem.Reset();
                DwList.Visible = true;
                DwListMem.Visible = false;
            }
            else if (mem_type == 2)
            {
                DwListMem.Reset();
                DwListMem.InsertRow(0);
                DwList.Reset();
                DwList.Visible = false;
                DwListMem.Visible = true;
            }
            else if (mem_type == 3)
            {
                DwListMem.Reset();
                DwListMem.InsertRow(0);
                DwList.Reset();
                DwList.Visible = false;
                DwListMem.Visible = true;
            }
        }

        private void Invite()
        {
            Decimal select_flag, countmem;
            Decimal mem_type, course_id, count_invite_num;
            String sqlStr, project_id, invite_seq, membgroup_code, membgroup_desc, memb_name, memb_surname, member_no, prename_desc;

            Sta ta = new Sta(sqlca.ConnectionString);
            Sdt dt = new Sdt();
            Sdt dt2 = new Sdt();

            project_id = HfProjectId.Value;
            course_id = Convert.ToDecimal(HfCourseId.Value);

            mem_type = DwMain.GetItemDecimal(1, "mem_type");

            sqlStr = @"SELECT MAX(projectinvite.invite_seq )AS invite_seq
                               FROM   projectinvite
                               WHERE  projectinvite.project_id  = '" + project_id + @"' and
                                      projectinvite.course_id   = '" + course_id + @"' and
                                      projectinvite.invite_type = '" + mem_type + "'";
            dt = ta.Query(sqlStr);
            dt.Next();
            invite_seq = dt.GetString("invite_seq");

            if (invite_seq == "")
            {
                invite_seq = "000001";
            }
            else
            {
                invite_seq = Convert.ToString(Convert.ToInt32(invite_seq) + 1);
                invite_seq = "000000" + invite_seq;
                invite_seq = WebUtil.Right(invite_seq, 6);
            }

            //try
            //{
            //    count_invite_num = DwMain.GetItemDecimal(1, "invite_accept");
            //}
            //catch { count_invite_num = 0; }

            if (mem_type == 1)
            {
                try
                {
                    //if (count_invite_num == 0)
                    //{
                    for (int i = 1; i <= DwList.RowCount; i++)
                    {
                        select_flag = DwList.GetItemDecimal(i, "select_flag");
                        if (select_flag == 1)
                        {
                            membgroup_code = DwList.GetItemString(i, "membgroup_code");
                            membgroup_desc = DwList.GetItemString(i, "membgroup_desc");
                            count_invite_num = DwList.GetItemDecimal(i, "inv_num");
                            countmem = DwList.GetItemDecimal(i, "countmem");

                            sqlStr = @"INSERT INTO projectinvite(invite_type  ,  invite_seq  ,  project_id  ,  course_id  ,
                                                     membgroup_code  ,  membgroup_desc  ,  count_invite_num  ,  count_invite_accept)
                                    VALUES ('" + mem_type + "','" + invite_seq + "','" + project_id + "','" + course_id + @"',
                                        '" + membgroup_code + "','" + membgroup_desc + "','" + countmem + "','" + count_invite_num + "')";

                            invite_seq = Convert.ToString(Convert.ToDecimal(invite_seq) + 1);
                            invite_seq = "000000" + invite_seq;
                            invite_seq = WebUtil.Right(invite_seq, 6);
                            ta.Exe(sqlStr);
                        }
                    }
                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกเรียบร้อย");
                    //                    }
                    //                    else if (count_invite_num != 0)
                    //                    {
                    //                        membgroup_code = DwList.GetItemString(1, "membgroup_code");
                    //                        membgroup_desc = DwList.GetItemString(1, "membgroup_desc");

                    //                        sqlStr = @"UPDATE projectinvite
                    //                                   SET    count_invite_num = '" + count_invite_num + @"'
                    //                                   WHERE  membgroup_code   = '" + membgroup_code + @"' and
                    //                                          project_id       = '" + project_id + @"' and
                    //                                          invite_type      = 1";
                    //                        ta.Exe(sqlStr);
                    //                        LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกเรียบร้อย");
                    //                    }
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                }
            }
            else if (mem_type == 2)
            {
                for (int i = 1; i <= DwListMem.RowCount; i++)
                {
                    select_flag = DwListMem.GetItemDecimal(i, "select_flag");

                    if (select_flag == 1)
                    {
                        try
                        {
                            membgroup_code = DwListMem.GetItemString(i, "membgroup_code");
                            membgroup_desc = DwListMem.GetItemString(i, "membgroup_desc");
                            member_no = DwListMem.GetItemString(i, "member_no");
                            prename_desc = DwListMem.GetItemString(i, "prename_desc");
                            memb_name = DwListMem.GetItemString(i, "memb_name");
                            memb_surname = DwListMem.GetItemString(i, "memb_surname");

                            sqlStr = @"INSERT INTO projectinvite(invite_type  ,  invite_seq  ,  project_id  ,  course_id  ,
                                                                 membgroup_code  ,  membgroup_desc  ,  member_no  , thaifull_name  ,  thaisure_name  ,  count_invite_num  ,  count_invite_accept)
                                                    VALUES ('" + mem_type + "','" + invite_seq + "','" + project_id + "','" + course_id + @"',
                                                            '" + membgroup_code + "','" + membgroup_desc + "','" + member_no + "','" + (prename_desc + memb_name) + "','" + memb_surname + "','1','1')";
                            ta.Exe(sqlStr);

                            invite_seq = Convert.ToString(Convert.ToDecimal(invite_seq) + 1);
                            invite_seq = "000000" + invite_seq;
                            invite_seq = WebUtil.Right(invite_seq, 6);
                        }
                        catch (Exception ex)
                        {
                            LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                        }
                    }
                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกเรียบร้อย");
                }
            }
            else if (mem_type == 3)
            {
                for (int i = 1; i <= DwListMem.RowCount; i++)
                {
                    select_flag = DwListMem.GetItemDecimal(i, "select_flag");

                    if (select_flag == 1)
                    {
                        try
                        {
                            membgroup_code = DwListMem.GetItemString(i, "membgroup_code");
                            membgroup_desc = DwListMem.GetItemString(i, "membgroup_desc");
                            member_no = DwListMem.GetItemString(i, "member_no");
                            prename_desc = DwListMem.GetItemString(i, "prename_desc");
                            memb_name = DwListMem.GetItemString(i, "memb_name");
                            memb_surname = DwListMem.GetItemString(i, "memb_surname");

                            sqlStr = @"INSERT INTO projectinvite(invite_type  ,  invite_seq  ,  project_id  ,  course_id  ,
                                                                 membgroup_code  ,  membgroup_desc  ,  member_no  , thaifull_name  ,  thaisure_name  ,  count_invite_num  ,  count_invite_accept)
                                                    VALUES ('" + mem_type + "','" + invite_seq + "','" + project_id + "','" + course_id + @"',
                                                            '" + membgroup_code + "','" + membgroup_desc + "','" + member_no + "','" + (prename_desc + memb_name) + "','" + memb_surname + "','1','1')";
                            ta.Exe(sqlStr);

                            invite_seq = Convert.ToString(Convert.ToDecimal(invite_seq) + 1);
                            invite_seq = "000000" + invite_seq;
                            invite_seq = WebUtil.Right(invite_seq, 6);
                        }
                        catch (Exception ex)
                        {
                            LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                        }
                    }
                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกเรียบร้อย");
                }
            }
        }

        private void Refresh()
        {

        }

        private void SearchList()
        {
            String sqlStr, strFilterAll = "", strFilter1 = "", strFilter2 = "", strFilter3 = "", strFilter4 = "", strFilter5 = "", strFilter6 = "", strFilter7 = "", strFilter8 = "";
            Decimal mem_type, count_invite_num, coure_id;
            String member_no, project_id, membgroup_no_begin, membgroup_no_end, postcode_begin, postcode_end;
            String memb_name, memb_surname, loancontract_no, card_person, salary_id, membgroup_code, membgroup_desc;

            membgroup_no_begin = DwMain.GetItemString(1, "membgroup_no_begin");
            membgroup_no_end = DwMain.GetItemString(1, "membgroup_no_end");
            project_id = HfProjectId.Value;
            coure_id = Convert.ToInt32(HfCourseId.Value);

            if (DwList.Visible == true)
            {
                DwList.SetTransaction(sqlca);
                DwList.Retrieve(membgroup_no_begin, membgroup_no_end, project_id, coure_id);
            }
            else if (DwListMem.Visible == true)
            {
                DwListMem.SetTransaction(sqlca);
                DwListMem.Retrieve(membgroup_no_begin, membgroup_no_end, project_id, coure_id);
            }

            Sta ta = new Sta(sqlca.ConnectionString);
            Sdt dt = new Sdt();

            try
            {
                member_no = DwMain.GetItemString(1, "member_no");
            }
            catch { member_no = ""; }

            try
            {
                memb_name = DwMain.GetItemString(1, "memb_name");
            }
            catch { memb_name = ""; }

            try
            {
                memb_surname = DwMain.GetItemString(1, "memb_surname");
            }
            catch { memb_surname = ""; }

            try
            {
                membgroup_code = DwMain.GetItemString(1, "membgroup_code");
            }
            catch { membgroup_code = ""; }

            try
            {
                membgroup_desc = DwMain.GetItemString(1, "membgroup_desc");
            }
            catch { membgroup_desc = ""; }

            try
            {
                postcode_begin = DwMain.GetItemString(1, "postcode_begin");
            }
            catch { postcode_begin = ""; }

            try
            {
                postcode_end = DwMain.GetItemString(1, "postcode_end");
            }
            catch { postcode_end = ""; }

            mem_type = DwMain.GetItemDecimal(1, "mem_type");
            project_id = HfProjectId.Value;

            //เช็คว่าเข้าร่วนโครงการนี้ไปแล้วหรือยัง 
            try
            {
                member_no = DwMain.GetItemString(1, "member_no");
                project_id = DwMain.GetItemString(1, "project_id");
                //course_id = DsEnroll.GetItemDecimal(1, "course_id");

                if (member_no == project_id)
                {
                    LtServerMessage.Text = WebUtil.WarningMessage("เลขที่ทะเบียนสมาชิกรายนี้ได้เข้าร่วมโครงการนี้ไปแล้ว");
                }
            }
            catch { }
            //จบการเช็คว่าเข้าร่วนโครงการนี้ไปแล้วหรือยัง 

            if (mem_type == 1)
            {
                if (member_no != "")
                {
                    strFilter1 = "membgroup_code= '" + membgroup_code + "'";
                }
                else
                {
                    strFilter1 = "1=1";
                }

                if (memb_name != "")
                {
                    strFilter2 = "(membgroup_desc like '" + membgroup_desc + "%')";
                }
                else
                {
                    strFilter2 = "1=1";
                }

                if (postcode_begin != "" && postcode_end != "")
                {
                    strFilter3 = "(mbmembmaster_postcode between '" + postcode_begin + "' and '" + postcode_end + "')";
                }

                strFilterAll = strFilter1 + " and " + strFilter2;
                strFilterAll = strFilterAll.Trim();

                DwList.SetFilter(strFilterAll);
                DwList.Filter();
            }

                  //end chk mem_type==1


            else if (mem_type == 2)
            {
                if (member_no != "")
                {
                    strFilter1 = "member_no= '" + member_no + "'";
                }
                else
                {
                    strFilter1 = "1=1";
                }

                if (memb_name != "")
                {
                    strFilter2 = "(memb_name like '" + memb_name + "%')";
                }
                else
                {
                    strFilter2 = "1=1";
                }

                if (memb_surname != "")
                {
                    strFilter3 = "(memb_surname like '" + memb_surname + "%')";
                }
                else
                {
                    strFilter3 = "1=1";
                }

                if (membgroup_code != "")
                {
                    strFilter4 = "(membgroup_code like '" + membgroup_code + "%')";
                }
                else
                {
                    strFilter4 = "1=1";
                }

                if (membgroup_desc != "")
                {
                    strFilter5 = "(membgroup_desc like '" + membgroup_desc + "%')";
                }
                else
                {
                    strFilter5 = "1=1";
                }

                if (postcode_begin != "" && postcode_end != "")
                {
                    strFilter6 = "(mbmembmaster_postcode between '" + postcode_begin + "' and '" + postcode_end + "')";
                }
                else
                {
                    strFilter6 = "1=1";
                }

                strFilterAll = strFilter1 + " and " + strFilter2 + " and " + strFilter3 + " and " + strFilter4 + " and " + strFilter5 + " and " + strFilter6;
                strFilterAll = strFilterAll.Trim();

                try
                {
                    DwListMem.SetFilter(strFilterAll);
                    DwListMem.Filter();
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                }
            }

                //end chk mem_type==2


            else if (mem_type == 3)
            {
                if (member_no != "")
                {
                    strFilter1 = "member_no= '" + member_no + "'";
                }
                else
                {
                    strFilter1 = "1=1";
                }

                if (memb_name != "")
                {
                    strFilter2 = "(memb_name like '" + memb_name + "%')";
                }
                else
                {
                    strFilter2 = "1=1";
                }

                if (memb_surname != "")
                {
                    strFilter3 = "(memb_surname like '" + memb_surname + "%')";
                }
                else
                {
                    strFilter3 = "1=1";
                }

                if (membgroup_code != "")
                {
                    strFilter4 = "(membgroup_code like '" + membgroup_code + "%')";
                }
                else
                {
                    strFilter4 = "1=1";
                }

                if (membgroup_desc != "")
                {
                    strFilter5 = "(membgroup_desc like '" + membgroup_desc + "%')";
                }
                else
                {
                    strFilter5 = "1=1";
                }

                if (postcode_begin != "" && postcode_end != "")
                {
                    strFilter6 = "(mbmembmaster_postcode between '" + postcode_begin + "' and '" + postcode_end + "')";
                }
                else
                {
                    strFilter6 = "1=1";
                }

                strFilterAll = strFilter1 + " and " + strFilter2 + " and " + strFilter3 + " and " + strFilter4 + " and " + strFilter5 + " and " + strFilter6;
                strFilterAll = strFilterAll.Trim();


                try
                {
                    DwListMem.SetFilter(strFilterAll);
                    DwListMem.Filter();
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                }

                // end chk==3


            }
        }

        private void JsCheckAll()
        {
            Decimal Set = 1;
            Boolean Select = CheckAll.Checked;

            if (Select == true)
            {
                Set = 1;
            }
            else if (Select == false)
            {
                Set = 0;
            }

            Decimal mem_type = DwMain.GetItemDecimal(1, "mem_type");

            if (mem_type == 1)
            {
                for (int i = 1; i <= DwList.RowCount; i++)
                {
                    DwList.SetItemDecimal(i, "select_flag", Set);
                }
            }
            else if (mem_type == 2)
            {
                for (int i = 1; i <= DwList.RowCount; i++)
                {
                    DwListMem.SetItemDecimal(i, "select_flag", Set);
                }
            }
        }

        private void checkAllpostcode()
        {
            String memb_ctrl, memb_ctrl2;
            Decimal select_allflag;

            int row = Convert.ToInt32(HfRow.Value);

            memb_ctrl = DwList.GetItemString(row, "sapgrp_control").Trim();
            select_allflag = DwList.GetItemDecimal(row, "select_all");

            for (int i = 1; i <= DwList.RowCount; i++)
            {
                memb_ctrl2 = DwList.GetItemString(i, "sapgrp_control").Trim();

                if (memb_ctrl2 == memb_ctrl)
                {
                    DwList.SetItemDecimal(i, "select_flag", select_allflag);
                }
            }
        }

    }
}
