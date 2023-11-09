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
    public partial class w_sheet_as_edit_send_invite : PageWebSheet, WebSheet
    {
        protected String postChangeType;
        protected String postInsertRow;
        protected String postDeleteRow;
        protected String postSearchList;
        protected String postRefresh;
        protected String postInvite;
        protected String postcheckAll;


        public void InitJsPostBack()
        {
            postInsertRow = WebUtil.JsPostBack(this, "postInsertRow");
            postDeleteRow = WebUtil.JsPostBack(this, "postDeleteRow");
            postChangeType = WebUtil.JsPostBack(this, "postChangeType");

            postRefresh = WebUtil.JsPostBack(this, "postRefresh");
            postInvite = WebUtil.JsPostBack(this, "postInvite");
            postSearchList = WebUtil.JsPostBack(this, "postSearchList");
            postcheckAll = WebUtil.JsPostBack(this, "postcheckAll");

        }

        public void WebSheetLoadBegin()
        {
            this.ConnectSQLCA();

            if (!IsPostBack)
            {
                DwProList.SetTransaction(sqlca);
                DwProList.Retrieve();

                DwMain.InsertRow(0);
                DwList.SetTransaction(sqlca);
                //DwList.Retrieve();
                DwUtil.RetrieveDDDW(DwMain, "membgroup_no_begin", "as_seminar.pbl", null);
                DwUtil.RetrieveDDDW(DwMain, "membgroup_no_end", "as_seminar.pbl", null);
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
            else if (eventArg == "postChangeType")
            {
                ChangeType();
            }
            else if (eventArg == "postInsertRow")
            {
                JsPostInsertRow();
            }
            else if (eventArg == "postDeleteRow")
            {
                JsPostDeleteRow();
            }
            else if (eventArg == "postRefresh")
            {
                Refresh();
            }
        }

        private void Refresh()
        {

        }

        public void SaveWebSheet()
        {
            try
            {
                if (DwList.Visible == true)
                {
                    DwList.SetTransaction(sqlca);
                    DwList.UpdateData();
                }
                else if (DwListMem.Visible == true)
                {
                    DwListMem.SetTransaction(sqlca);
                    DwListMem.UpdateData();
                }
                LtServerMessage.Text = WebUtil.CompleteMessage("แก้ไขเรียบร้อย");
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
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
            String project_id;
            Int32 course_id;

            mem_type = DwMain.GetItemDecimal(1, "mem_type");
            project_id = HfProjectId.Value;
            course_id = Convert.ToInt32(HfCourseId.Value);

            if (mem_type == 1)
            {
                DwList.Reset();
                //DwList.SetTransaction(sqlca);
                //DwList.Retrieve(project_id, course_id);
                DwListMem.Reset();
                DwList.Visible = true;
                DwListMem.Visible = false;
            }
            else if (mem_type == 2)
            {
                DwListMem.Reset();
                //DwListMem.SetTransaction(sqlca);
                //DwListMem.Retrieve(project_id, course_id);
                DwList.Reset();
                DwList.Visible = false;
                DwListMem.Visible = true;
            }
        }

        private void JsPostDeleteRow()
        {
            int row = int.Parse(HdListRow.Value);

            if (DwList.Visible == true)
            {
                DwList.DeleteRow(row);
            }
            else if (DwListMem.Visible == true)
            {
                DwListMem.DeleteRow(row);
            }
        }

        private void JsPostInsertRow()
        {
            if (DwList.Visible == true)
            {
                DwList.InsertRow(0);
            }
            else if (DwListMem.Visible == true)
            {
                DwListMem.InsertRow(0);
            }
        }

        private void SearchList()
        {
            Int32 course_id;
            String sqlStr, strFilterAll = "", strFilter1 = "", strFilter2 = "", strFilter3 = "", strFilter4 = "", strFilter5 = "", strFilter6 = "", strFilter7 = "", strFilter8 = "";
            Decimal mem_type, count_invite_num;
            String membgroup_no, member_no, project_id, membgroup_no_begin, membgroup_no_end;
            String memb_name, memb_surname, loancontract_no, card_person, salary_id, membgroup_code, membgroup_desc;

            membgroup_no_begin = DwMain.GetItemString(1, "membgroup_no_begin");
            membgroup_no_end = DwMain.GetItemString(1, "membgroup_no_end");

            project_id = HfProjectId.Value;
            course_id = Convert.ToInt32(HfCourseId.Value);

            if (DwList.Visible == true)
            {
                DwList.SetTransaction(sqlca);
                DwList.Retrieve(membgroup_no_begin, membgroup_no_end, project_id, course_id);
            }
            else if (DwListMem.Visible == true)
            {
                DwListMem.SetTransaction(sqlca);
                DwListMem.Retrieve(membgroup_no_begin, membgroup_no_end, project_id, course_id);
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

            mem_type = DwMain.GetItemDecimal(1, "mem_type");
            project_id = HfProjectId.Value;

            if (mem_type == 1)
            {
                if (membgroup_code != "")
                {
                    strFilter1 = "membgroup_code= '" + membgroup_code + "'";
                }
                else
                {
                    strFilter1 = "1=1";
                }

                if (membgroup_desc != "")
                {
                    strFilter2 = "(membgroup_desc like '%" + membgroup_desc + "%')";
                }
                else
                {
                    strFilter2 = "1=1";
                }

                strFilterAll = strFilter1 + " and " + strFilter2;
                strFilterAll = strFilterAll.Trim();

                DwList.SetFilter(strFilterAll);
                DwList.Filter();
            }
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
                    strFilter2 = "(thaifull_name like '%" + memb_name + "%')";
                }
                else
                {
                    strFilter2 = "1=1";
                }

                if (memb_surname != "")
                {
                    strFilter3 = "(thaisure_name like '%" + memb_surname + "%')";
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
                    strFilter5 = "(membgroup_desc like '%" + membgroup_desc + "%')";
                }
                else
                {
                    strFilter5 = "1=1";
                }

                strFilterAll = strFilter1 + " and " + strFilter2 + " and " + strFilter3 + " and " + strFilter4 + " and " + strFilter5;
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
        }
    }
}
