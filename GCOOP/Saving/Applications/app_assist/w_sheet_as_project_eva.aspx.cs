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
    public partial class w_sheet_as_project_eva : PageWebSheet, WebSheet
    {

        protected String postProjecteva;
        protected String postInsertRow;
        protected String postDeleteRow;
        protected String postSave;
        protected Sta ta;
        protected Sdt dt;

        public void InitJsPostBack()
        {
            postProjecteva = WebUtil.JsPostBack(this, "postProjecteva");
            postInsertRow = WebUtil.JsPostBack(this, "postInsertRow");
            postDeleteRow = WebUtil.JsPostBack(this, "postDeleteRow");
            postSave = WebUtil.JsPostBack(this, "postSave");
        }

        public void WebSheetLoadBegin()
        {
            this.ConnectSQLCA();

            ta = new Sta(sqlca.ConnectionString);
            dt = new Sdt();
            DwMain.SetTransaction(sqlca);
            if (!IsPostBack)
            {       
                DwMain.InsertRow(0);
                //DwUtil.RetrieveDDDW(DwMain, "project_id", "as_seminar.pbl", null);
                //DwMain.SetItemDateTime(1, "slip_date", state.SsWorkDate);
            }
            else
            {
                this.RestoreContextDw(DwMain);
            }
                //DwUtil.RetrieveDDDW(DwMain, "membnew_grp", "as_seminar.pbl", null);
        
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postProjecteva")
            {
                projecteva();
            }
            else if (eventArg == "postInsertRow")
            {
                insertRow();
            }
            else if (eventArg == "postDeleteRow")
            {
                DeleteRow();
            }
            else if (eventArg == "postSave")
            {
                Save();
            }
        }

      
        public void SaveWebSheet()
        {
            String sqlStr, projecteva, project_id, comment_desc;
            Decimal course_id , level1 , level2 , level3 , level4 , level5 , seq_no , seat_no ;

            try
            {
                projecteva = DwMain.GetItemString(1, "projecteva");
            }
            catch { projecteva = "" ;} 
            try
            {
                project_id = DwMain.GetItemString(1, "project_id");
            }
            catch { project_id = ""; }
            try
            {
                comment_desc = DwMain.GetItemString(1,"comment_desc");
            }
            catch { comment_desc = "" ; }

            try
            {
                course_id = DwMain.GetItemDecimal(1, "course_id");
            }
            catch { course_id = 0; }

            level1 = DwMain.GetItemDecimal(1, "level1");
            level2 = DwMain.GetItemDecimal(1, "level2");
            level3 = DwMain.GetItemDecimal(1, "level3");
            level4 = DwMain.GetItemDecimal(1, "level4");
            level5 = DwMain.GetItemDecimal(1, "level5");

            try
            {
                seq_no = DwMain.GetItemDecimal(1, "seq_no");
            }
            catch { seq_no = 0; }
            try
            {
                seat_no = DwMain.GetItemDecimal(1, "seat_no");
            }
            catch { seat_no = 0; }

            sqlStr = @"insert into projecteva(projecteva , project_id , course_id , comment_desc , level1 , level2 , level3 , level4 , level5 , seq_no , seat_no)
                                    values ('" + projecteva + "' , '" + project_id + "' , '" + course_id + "' , '" + comment_desc + "', '" + level1 + "', '" + level2 + "', '" + level3 + "' , '" + level4 + "', '" + level5 + "', '" + seq_no + "', '" + seat_no + "')";
            ta.Exe(sqlStr);
           
            LtServerMessage.Text = WebUtil.CompleteMessage("บันทีกข้อมูลเสร็จเรียบร้อยแล้ว");
        }


        public void WebSheetLoadEnd()
        {
            ta.Close();
            dt.Clear();
            DwMain.SaveDataCache();
        
        }


        private void projecteva()
        {

        }
        private void insertRow()
        {
            DwMain.InsertRow(0);
        }
        private void Save()
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

        private void DeleteRow()
        {
            int row = int.Parse(HdMainRow.Value);
            DwMain.DeleteRow(row);
        }

     
    }
}