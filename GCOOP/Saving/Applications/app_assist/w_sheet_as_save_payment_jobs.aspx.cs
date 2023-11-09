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
using Sybase.DataWindow;
using DataLibrary;
using System.Web.Services.Protocols;

namespace Saving.Applications.app_assist
{
    public partial class w_sheet_as_save_payment_jobs : PageWebSheet, WebSheet
    {
        protected String postRetrieveCost;
        protected String postRefresh;
        protected String postInsertRow;
        protected String postDeleteRow;



        #region WebSheet Members

        public void InitJsPostBack()
        {
            postRefresh = WebUtil.JsPostBack(this, "postRefresh");
            postInsertRow = WebUtil.JsPostBack(this, "postInsertRow");
            postDeleteRow = WebUtil.JsPostBack(this, "postDeleteRow");
            postRetrieveCost = WebUtil.JsPostBack(this, "postRetrieveCost");
        }

        public void WebSheetLoadBegin()
        {
            this.ConnectSQLCA();

            if (!IsPostBack)
            {
                DwList.SetTransaction(sqlca);
                DwList.Retrieve();
                DwMain.InsertRow(0);
            }
            else
            {
                this.RestoreContextDw(DwList);
                this.RestoreContextDw(DwMain);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postRetrieveCost")
            {
                RetrieveCost();
            }
            else if (eventArg == "postRefresh")
            {
                Refresh();
            }
            else if (eventArg == "postInsertRow")
            {
                JsPostInsertRow();
            }
            else if (eventArg == "postDeleteRow")
            {
                JsPostDeleteRow();
            }
        }

        public void SaveWebSheet()
        {
            String cost_seq, project_id, sqlStr;
            Decimal course_id;
            int row;

            Sta ta = new Sta(sqlca.ConnectionString);
            Sdt dt = new Sdt();
            Sdt dt2 = new Sdt();

            row = Convert.ToInt32(HdDetailRow.Value);


            try
            {
                try
                {
                    cost_seq = HfCostSeq.Value;
                }
                catch
                {
                    cost_seq = "1";
                }

                if (cost_seq == "")
                {
                    sqlStr = @"SELECT MAX(projectcosts.costs_seq )AS costs_seq
                               FROM   projectcosts";
                    dt = ta.Query(sqlStr);
                    dt.Next();
                    cost_seq = dt.GetString("costs_seq");
                    if (cost_seq == "")
                    {
                        cost_seq = "0";
                    }
                    cost_seq = Convert.ToString(Convert.ToInt32(cost_seq) + 1);

                    project_id = DwList.GetItemString(row, "project_id");
                    course_id = DwList.GetItemDecimal(row, "course_id");

                    sqlStr = "INSERT INTO projectcosts(costs_seq , project_id,course_id)VALUES('" + cost_seq + "','" + project_id + "','" + course_id + "')";
                    ta.Query(sqlStr);
                }

                for (int i = 1; i <= DwMain.RowCount; i++)
                {
                    DwMain.SetItemString(i, "costs_seq", cost_seq);
                }

                DwMain.SetTransaction(sqlca);
                DwMain.UpdateData();

                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกเรียบร้อย");
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
        }

        #endregion



        private void Refresh()
        {

        }

        private void RetrieveCost()
        {
            String project_id;
            Decimal course_id;

            try
            {
                project_id = HfProjectId.Value;
            }
            catch { project_id = ""; }

            try
            {
                course_id = Convert.ToDecimal(HfCourseId.Value);
            }
            catch { course_id = 1; }

            DwMain.SetTransaction(sqlca);
            DwMain.Retrieve(project_id, course_id);
        }

        private void JsPostDeleteRow()
        {
            int row = int.Parse(HdMainRow.Value);
            DwMain.DeleteRow(row);
        }

        private void JsPostInsertRow()
        {
            DwMain.InsertRow(0);
        }
    }
}
