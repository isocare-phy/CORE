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
    public partial class w_sheet_as_send_invite_jobs : PageWebSheet, WebSheet
    {
        protected String postSearchList;
        protected String postRefresh;
        protected String postInvite;
        protected String postcheckAll;
        protected String postChangeType;



        public void InitJsPostBack()
        {
            postRefresh = WebUtil.JsPostBack(this, "postRefresh");
            postInvite = WebUtil.JsPostBack(this, "postInvite");
            postSearchList = WebUtil.JsPostBack(this, "postSearchList");
            postcheckAll = WebUtil.JsPostBack(this, "postcheckAll");
            postChangeType = WebUtil.JsPostBack(this, "postChangeType");
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

//            sqlStr = @"SELECT MAX(projectinvite.invite_seq )AS invite_seq
//                               FROM   projectinvite
//                               WHERE  projectinvite.project_id  = '" + project_id + @"' and
//                                      projectinvite.course_id   = '" + course_id + @"' and
//                                      projectinvite.invite_type = '" + mem_type + "'";
            sqlStr = @"SELECT MAX(projectinvite.invite_seq )AS invite_seq
                               FROM   projectinvite
                               WHERE  projectinvite.course_id   = '" + course_id + @"' and
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
            Decimal mem_type, count_invite_num;
            String membgroup_no, member_no, project_id, membgroup_no_begin, membgroup_no_end;
            String memb_name, memb_surname, loancontract_no, card_person, salary_id;

            membgroup_no_begin = DwMain.GetItemString(1, "membgroup_no_begin");
            membgroup_no_end = DwMain.GetItemString(1, "membgroup_no_end");

            if (DwList.Visible == true)
            {
                DwList.SetTransaction(sqlca);
                DwList.Retrieve(membgroup_no_begin, membgroup_no_end);
            }
            else if (DwListMem.Visible == true)
            {
                DwListMem.SetTransaction(sqlca);
                DwListMem.Retrieve(membgroup_no_begin, membgroup_no_end);
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
                salary_id = DwMain.GetItemString(1, "salary_id");
            }
            catch { salary_id = ""; }

            try
            {
                card_person = DwMain.GetItemString(1, "card_person");
            }
            catch { card_person = ""; }

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
                loancontract_no = DwMain.GetItemString(1, "loancontract_no");
            }
            catch { loancontract_no = ""; }

            try
            {
                membgroup_no_begin = DwMain.GetItemString(1, "membgroup_no_begin");
            }
            catch { membgroup_no_begin = ""; }

            try
            {
                membgroup_no_end = DwMain.GetItemString(1, "membgroup_no_end");
            }
            catch { membgroup_no_end = ""; }

            mem_type = DwMain.GetItemDecimal(1, "mem_type");
            project_id = HfProjectId.Value;

            if (member_no != "")
            {
                strFilter1 = "member_no= '" + member_no + "'";
            }
            else
            {
                strFilter1 = "1=1";
            }

            if (salary_id != "")
            {
                strFilter2 = "salary_id= '" + salary_id + "'";
            }
            else
            {
                strFilter2 = "1=1";
            }

            if (card_person != "")
            {
                strFilter3 = "card_person= '" + card_person + "'";
            }
            else
            {
                strFilter3 = "1=1";
            }

            if (memb_name != "")
            {
                strFilter4 = "memb_name= '" + memb_name + "'";
            }
            else
            {
                strFilter4 = "1=1";
            }

            if (memb_surname != "")
            {
                strFilter5 = "memb_surname= '" + memb_surname + "'";
            }
            else
            {
                strFilter5 = "1=1";
            }

            if (loancontract_no != "")
            {
                strFilter6 = "loancontract_no= '" + loancontract_no + "'";
            }
            else
            {
                strFilter6 = "1=1";
            }

            //if (membgroup_no != "")
            //{
            //    strFilter7 = "membgroup_code= '" + membgroup_no + "'";
            //}
            //else
            //{
            //    strFilter7 = "1=1";
            //}

            strFilterAll = strFilter1 + " and " + strFilter2 + " and " + strFilter3 + " and " + strFilter4 + " and " + strFilter5 + " and " + strFilter6 + " and " + strFilter7;
            strFilterAll = strFilterAll.Trim();

            if (mem_type == 1)
            {
                //DwList.SetFilter("membgroup_code = '" + membgroup_no + "'");
                DwList.SetFilter(strFilterAll);
                DwList.Filter();

                //                sqlStr = @"SELECT count_invite_num
                //                           FROM   projectinvite
                //                           WHERE  membgroup_code = '" + membgroup_no + @"' and
                //                                  project_id     = '" + project_id + @"' and
                //                                  invite_type    = 1";
                //                dt = ta.Query(sqlStr);
                //                dt.Next();
                //                count_invite_num = Convert.ToDecimal(dt.GetDouble("count_invite_num"));
                //                DwMain.SetItemDecimal(1, "invite_accept", count_invite_num);
            }
            else if (mem_type == 2)
            {
                DwList.SetFilter(strFilterAll);
                DwList.Filter();
            }
            else if (mem_type == 3)
            {
                DwList.SetFilter(strFilterAll);
                DwList.Filter();
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
            for (int i = 1; i <= DwList.RowCount; i++)
            {
                DwList.SetItemDecimal(i, "select_flag", Set);
            }
        }
    }
}
