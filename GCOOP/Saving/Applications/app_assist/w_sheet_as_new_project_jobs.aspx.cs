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
    public partial class w_sheet_as_new_project_jobs : PageWebSheet, WebSheet
    {
        protected String postProjectId;
        protected DwThDate tDwMain;
        protected DwThDate tDwDetail;
        private String pbl = "as_seminar.pbl";




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

            postProjectId = WebUtil.JsPostBack(this, "postProjectId");
        }

        public void WebSheetLoadBegin()
        {
            this.ConnectSQLCA();

            if (!IsPostBack)
            {
                try
                {
                    try
                    {
                        DwUtil.RetrieveDataWindow(DwList, pbl, null, null);
                    }
                    catch { }

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
            DwMain.SetItemString(1, "projecttype_code", "02");
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postProjectId")
            {
                ProjectId();
            }
        }

        public void SaveWebSheet()
        {
            String project_id, projectIdMain;
            int rowCount;

            projectIdMain = DwMain.GetItemString(1, "project_id").Trim();
            project_id = FindProjectId();
            project_id = project_id.Trim();

            if (project_id == projectIdMain)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("เลขที่โครงการซ้ำ");
                return;
            }

            if (project_id == "")
            {
                try
                {
                    DwUtil.InsertDataWindow(DwMain, "as_seminar.pbl", "projectmaster");

                    try
                    {
                        project_id = DwDetail.GetItemString(1, "project_id");
                    }
                    catch
                    {
                        project_id = "";
                    }

                    if (project_id == "")
                    {
                        project_id = DwMain.GetItemString(1, "project_id");
                        DwDetail.SetItemString(1, "project_id", project_id);
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
            else if (project_id != "")
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
                        DwUtil.UpdateDataWindow(DwDetail, "as_seminar.pbl", "projectcourse");
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
        }

        public void WebSheetLoadEnd()
        {
            tDwMain.Eng2ThaiAllRow();
            tDwDetail.Eng2ThaiAllRow();
            this.DisConnectSQLCA();
            DwList.SaveDataCache();
            DwMain.SaveDataCache();
            DwDetail.SaveDataCache();
        }



        private String FindProjectId()
        {
            String strSQL, projectID;
            int rowCount;

            Sta ta = new Sta(sqlca.ConnectionString);
            Sdt dt = new Sdt();

            projectID = DwMain.GetItemString(1, "project_id");


            strSQL = @"SELECT project_id 
                               FROM   projectmaster
                               WHERE  project_id = '" + projectID + "'";
            dt = ta.Query(strSQL);
            dt.Next();

            projectID = dt.GetString("project_id");
            return projectID;
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
    }
}
