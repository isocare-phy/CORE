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
    public partial class w_sheet_as_new_project : PageWebSheet, WebSheet
    {
        protected String postInsertRow;
        protected String postDeleteRow;
        protected String postProjectId;
        protected String postFilterProjectId;
        protected DwThDate tDwMain;
        protected DwThDate tDwDetail;
        private String pbl = "as_seminar.pbl";

        Sta ta;
        Sdt dt;


        public void InitJsPostBack()
        {
            tDwMain = new DwThDate(DwMain, this);
            tDwDetail = new DwThDate(DwDetail, this);
            tDwMain.Add("startproject_date", "startproject_tdate");
            tDwMain.Add("endproject_date", "endproject_tdate");
            tDwMain.Add("startapply_date", "startapply_tdate");
            tDwMain.Add("endapply_date", "endapply_tdate");
            tDwDetail.Add("startcourse_date", "startcourse_tdate");
            tDwDetail.Add("endcourse_date", "endcourse_tdate");

            postInsertRow = WebUtil.JsPostBack(this, "postInsertRow");
            postFilterProjectId = WebUtil.JsPostBack(this, "postFilterProjectId");
            postDeleteRow = WebUtil.JsPostBack(this, "postDeleteRow");
            postProjectId = WebUtil.JsPostBack(this, "postProjectId");
        }

        public void WebSheetLoadBegin()
        {
            this.ConnectSQLCA();

            ta = new Sta(sqlca.ConnectionString);
            dt = new Sdt();

            if (!IsPostBack)
            {
                try
                {
                    try
                    {
                        DwList.InsertRow(0);

                        //DwUtil.RetrieveDataWindow(DwList, pbl, null, null);
                        //DwUtil.RetrieveDDDW(DwList, "project_id", pbl, null);
                        //DwUtil.RetrieveDDDW(DwList, "project_year", pbl, null);     
                
                    }
                    catch (Exception ex)
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                    }

                    DwMain.InsertRow(0);
                    DwDetail.InsertRow(0);

                    DwUtil.RetrieveDDDW(DwMain, "projecttype_code_1", "as_seminar.pbl", null);
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                }
            }

            else
            {
                this.RestoreContextDw(DwList);
                this.RestoreContextDw(DwMain);
                this.RestoreContextDw(DwDetail);
            }
            //DwUtil.RetrieveDDDW(DwDetail, "proj_post_code", pbl, null);
            //DwMain.SetItemString(1, "projecttype_code", "01");
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postProjectId")
            {
                ProjectId();
            }
            else if (eventArg == "postInsertRow")
            {
                JsPostInsertRow();
            }
            else if (eventArg == "postDeleteRow")
            {
                JsPostDeleteRow();
            }
            else if (eventArg == "postFilterProjectId")
            {
                FilterProjectId();
            }
        }

        public void SaveWebSheet()
        {
            String project_id, projectIdMain;
            int rowCount;

            project_id = DwMain.GetItemString(1, "project_id");

            if (project_id == "AUTO")
            {
                project_id = LastProjectId();
                //project_id = project_id.Trim();

                DwMain.SetItemString(1, "project_id", project_id);

                try
                {
                    DwUtil.InsertDataWindow(DwMain, "as_seminar.pbl", "projectmaster");

                    project_id = DwMain.GetItemString(1, "project_id");
                    for (int i = 1; i <= DwDetail.RowCount; i++)
                    {
                        DwDetail.SetItemString(i, "project_id", project_id);
                    }

                    try
                    {
                        DwUtil.InsertDataWindow(DwDetail, "as_seminar.pbl", "projectcourse");
                    }
                    catch { }

                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกเรียบร้อย");

                    DwUtil.RetrieveDataWindow(DwList, pbl, null, null);
                    DwMain.Reset();
                    DwMain.InsertRow(0);
                    DwDetail.Reset();
                    DwDetail.InsertRow(0);
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                }
            }
            else if (project_id != "" || project_id != "AUTO")
            {
                try
                {
                    try
                    {
                        DwUtil.UpdateDataWindow(DwMain, "as_seminar.pbl", "projectmaster");
                    }
                    catch { }

                    try
                    {
                        for (int i = 1; i <= DwDetail.RowCount; i++)
                        {
                            DwDetail.SetItemString(i, "project_id", project_id);
                        }
                        DwUtil.UpdateDataWindow(DwDetail, "as_seminar.pbl", "projectcourse");
                        //DwUtil.InsertDataWindow(DwDetail, "as_seminar.pbl", "projectcourse");
                    }
                    catch { }

                    DwDetail.SetTransaction(sqlca);
                    DwDetail.UpdateData();

                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกเรียบร้อย");

                    //DwUtil.RetrieveDataWindow(DwList, pbl, null, null);
                    DwMain.Reset();
                    DwMain.InsertRow(0);
                    DwDetail.Reset();
                    DwDetail.InsertRow(0);
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                }
            }
        }

        public void WebSheetLoadEnd()
        {
            dt.Clear();
            tDwMain.Eng2ThaiAllRow();
            tDwDetail.Eng2ThaiAllRow();
            this.DisConnectSQLCA();
            DwList.SaveDataCache();
            DwMain.SaveDataCache();
            DwDetail.SaveDataCache();
        }



        private void FilterProjectId()
        {
            Decimal project_year;

            project_year = DwList.GetItemDecimal(1, "project_year");

            object[] args = new object[1];
            args[0] = project_year;

            DwUtil.RetrieveDDDW(DwList, "project_id", pbl, args);
        }

        private void ProjectId()
        {
            String project_id;

            try
            {
                project_id = HfProjectId.Value;
            }
            catch { project_id = ""; }

            object[] args = new object[1];
            args[0] = project_id;

            //DwMain.SetTransaction(sqlca);
            //DwMain.Retrieve(project_id);
            DwMain.Reset();
            DwUtil.RetrieveDataWindow(DwMain, pbl, tDwMain, args);
            //DwDetail.SetTransaction(sqlca);
            //DwDetail.Retrieve(project_id);
            DwDetail.Reset();
            DwUtil.RetrieveDataWindow(DwDetail, pbl, tDwDetail, args);

            tDwMain.Eng2ThaiAllRow();
        }

        private void JsPostDeleteRow()
        {
            int row = int.Parse(HdMainRow.Value);
            DwDetail.DeleteRow(row);
        }

        private void JsPostInsertRow()
        {
            DwDetail.InsertRow(0);
        }

        private String FindProjectId()
        {
            String strSQL, projectID;
            int rowCount;

            projectID = DwMain.GetItemString(1, "project_id");


            strSQL = @"SELECT  project_id 
                               FROM   projectmaster
                               WHERE  project_id = '" + projectID + "'";
            dt.Clear();
            dt = ta.Query(strSQL);
            dt.Next();

            projectID = dt.GetString("project_id");
            return projectID;
        }

        private String LastProjectId()
        {
            String strSQL, last_projectId;

            strSQL = @"SELECT max(project_id) as project_id
                              FROM   projectmaster";
            dt.Clear();
            dt = ta.Query(strSQL);
            dt.Next();

            last_projectId = dt.GetString("project_id6");
            last_projectId = "000000" + Convert.ToString(Convert.ToInt32(last_projectId) + 1);
            last_projectId = WebUtil.Right(last_projectId, 6);

            return last_projectId;
        }
    }
}
