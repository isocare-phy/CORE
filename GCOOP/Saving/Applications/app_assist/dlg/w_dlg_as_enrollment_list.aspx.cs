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

namespace Saving.Applications.app_assist.dlg
{
    public partial class w_dlg_as_enrollment_list : PageWebDialog, WebDialog
    {
        protected String postGetCourseId;
        protected String postGetList;
        protected String postSaveList;
        protected String postDelete;

        protected String postFilterProjectId;//เพิ่มมา
        protected DataStore DStore; //เพิ่มมา

        private Sta ta;
        private Sdt dt;

        DataStore Ds;  //เพิ่มมา


        public void InitJsPostBack()
        {
            postGetCourseId = WebUtil.JsPostBack(this, "postGetCourseId");
            postGetList = WebUtil.JsPostBack(this, "postGetList");
            postSaveList = WebUtil.JsPostBack(this, "postSaveList");
            postDelete = WebUtil.JsPostBack(this, "postDelete");
            postFilterProjectId = WebUtil.JsPostBack(this, "postFilterProjectId");
        }

        public void WebDialogLoadBegin()
        {
            this.ConnectSQLCA();

            ta = new Sta(sqlca.ConnectionString);
            dt = new Sdt();

            Ds = new DataStore();
           
            DwMain.SetTransaction(sqlca);
            DwHead.SetTransaction(sqlca);//เพิ่มมา
            DwMainHead.SetTransaction(sqlca);//เพิ่มมา

            if (!IsPostBack)
            {
               
                DwMainHead.SetTransaction(sqlca);//เพิ่มมา

                DwHead.InsertRow(0);
                DwMainHead.InsertRow(0);
                
              
               DwUtil.RetrieveDDDW(DwHead, "project_id", "as_seminar.pbl", null);
               DwUtil.RetrieveDDDW(DwHead, "course_id", "as_seminar.pbl", null);               
               //DwMain.SetTransaction(sqlca);
               //DwMain.Retrieve();
            }
            else
            {
                this.RestoreContextDw(DwHead);
                this.RestoreContextDw(DwMainHead);
                this.RestoreContextDw(DwMain);
            }
            DwUtil.RetrieveDDDW(DwHead, "project_id", "as_seminar.pbl", null);//เพิ่มมา
            DwUtil.RetrieveDDDW(DwHead, "course_id", "as_seminar.pbl", null);//เพิ่มมา

        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postGetCourseId")
            {
                GetCourseId();
            }
            else if (eventArg == "postGetList")
            {
                GetListRetrieve();
            }
            else if (eventArg == "postSaveList")
            {
                SaveList();
            }
            else if (eventArg == "postDelete")
            {
                DeleteRow();
            }
            else if (eventArg == "postFilterProjectId")
            {
                FilterProjectId();
            }
        }

        private void DeleteRow()
        {
            String sqlStr, member_no;
            int row = Convert.ToInt32(HdMainRow.Value);

            member_no = DwMain.GetItemString(row, "member_no");

            sqlStr = "DELETE FROM projectenrollment WHERE member_no = '" + member_no + "'";

            ta.Exe(sqlStr);
        }

        public void WebDialogLoadEnd()
        {
            ta.Close();
            dt.Clear();
            DwHead.SaveDataCache();
            DwMain.SaveDataCache();
            DwMainHead.SaveDataCache();

            if (HdMainRow.Value != "")
            {
                DwMain.SetRow(Convert.ToInt16(HdMainRow.Value));
                DwMain.Focus();
            }
        }



        private void SaveList()
        {
            String sqlStr, member_no, mem_tel;
            try
            {
                DwMain.UpdateData();

                for (int i = 1; i <= DwMain.RowCount; i++)
                {
                    member_no = DwMain.GetItemString(i, "member_no");
                    try
                    {
                        mem_tel = DwMain.GetItemString(i, "mem_tel");
                    }
                    catch { mem_tel = ""; }

                    //อัพเดต เบอร์โทรของสมาชิก
                    sqlStr = @"UPDATE projectslip
                               SET mem_tel = '" + mem_tel + @"'
                               WHERE member_no = '" + member_no + "'";
                    ta.Exe(sqlStr);
                }

                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกเรียบร้อย");
            }
            catch { }
        }

        private void GetList()
        {
            String project_id, strAll, str1, str2, str3, member_no;
            Decimal course_id;

            try
            {
                project_id = DwHead.GetItemString(1, "project_id");
            }
            catch { project_id = ""; }
            try
            {
                course_id = DwHead.GetItemDecimal(1, "course_id");
            }
            catch { course_id = 9999; }
            try
            {
                member_no = DwHead.GetItemString(1, "member_no");
            }
            catch { member_no = ""; }

            if (project_id != "")
            {
                str1 = "project_id='" + project_id + "'";
            }
            else { str1 = "1=1"; }
            if (course_id != 9999)
            {
                str2 = "course_id =" + course_id + "";
            }
            else { str2 = "1=1"; }
            if (member_no != "")
            {
                str3 = "member_no ='" + member_no + "'";
            }
            else { str3 = "1=1"; }

            DwMain.SetTransaction(sqlca);
            DwMain.Retrieve();

            strAll = str1 + " and " + str2 + " and " + str3;
            DwMain.SetFilter(strAll);
            DwMain.Filter();
        }

        private void GetCourseId()
        {
            String project_id = DwHead.GetItemString(1, "project_id");

            object[] ArgProjectId = new object[1];
            ArgProjectId[0] = project_id;
            DwUtil.RetrieveDDDW(DwHead, "course_id", "as_seminar.pbl", ArgProjectId);
        }

        private void GetListRetrieve()
        {
            String project_id, strAll, str1, str2, str3, member_no;
            Decimal course_id;

            try
            {
                project_id = DwHead.GetItemString(1, "project_id");
            }
            catch { project_id = ""; }
            try
            {
                course_id = DwHead.GetItemDecimal(1, "course_id");
            }
            catch { course_id = 9999; }
            try
            {
                member_no = DwHead.GetItemString(1, "member_no");
            }
            catch { member_no = ""; }

            if (member_no != "")
            {
                str3 = "member_no ='" + member_no + "'";
            }
            else { str3 = "1=1"; }

            DwMain.Retrieve(project_id, course_id); 


            DwMain.SetFilter(str3);
            DwMain.Filter();
        }

        private void FilterProjectId()
        {
            String project_id;
            Decimal project_year;

          
            project_year = DwMain.GetItemDecimal(1, "project_year");
            project_id   = DwHead.GetItemString(1, "project_id");

            object[] args = new object[1];
            args[0] = project_year;
            args[1] = project_id;

         

            DwUtil.RetrieveDDDW(DwMain, "project_id", "as_seminar.pbl", null);
            DwUtil.RetrieveDDDW(DwHead, "project_id", "as_seminar.pbl", null);
        }
    }
}
