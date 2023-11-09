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
using System.Timers;
using DataLibrary;
using Sybase.DataWindow;
using System.Web.Services.Protocols;
namespace Saving.Applications.app_assist.dlg
{
    public partial class w_dlg_as_child_lists : PageWebDialog, WebDialog
    {
        protected String postInsertRow;
        protected String postDeleteRow;
        protected String postSaveList;
        protected String postFilterScholarship;

        protected String member_no;

        String[] ArStr;

        public void InitJsPostBack()
        {
            postInsertRow = WebUtil.JsPostBack(this, "postInsertRow");
            postDeleteRow = WebUtil.JsPostBack(this, "postDeleteRow");
            postSaveList = WebUtil.JsPostBack(this, "postSaveList");
            postFilterScholarship = WebUtil.JsPostBack(this, "postFilterScholarship");
        }

        public void WebDialogLoadBegin()
        {
            this.ConnectSQLCA();

            ArStr = new String[2];
            try
            {
                member_no = Request["member_no"];
                ArStr = member_no.Split(',');
            }
            catch { member_no = ""; }

            int row = DwMain.RowCount;
            if (row >= 1)
            {
                for (int i = 1; i <= DwMain.RowCount; i++)
                {
                    DwMain.SetItemString(i, "child_surname", ArStr[1]);
                }
            }

            if (!IsPostBack)
            {
                DwMain.SetTransaction(sqlca);
                DwMain.Retrieve(ArStr[0]);
            }
            else
            {
                this.RestoreContextDw(DwMain);
            }
            DwUtil.RetrieveDDDW(DwMain, "scholarship_level", "as_capital.pbl", null);
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postInsertRow")
            {
                JsPostInsertRow();
            }
            else if (eventArg == "postDeleteRow")
            {
                JsPostDeleteRow();
            }
            else if (eventArg == "postSaveList")
            {
                SaveList();
            }
            else if (eventArg == "postFilterScholarship")
            {
                //FilterScholarship();
            }
        }

        public void WebDialogLoadEnd()
        {
            DwMain.SaveDataCache();
        }



        private void FilterScholarship()
        {
            Decimal level_School = DwMain.GetItemDecimal(1, "level_School");

            DwUtil.RetrieveDDDW(DwMain, "scholarship_level", "as_capital.pbl", null);
            DataWindowChild Dc = DwMain.GetChild("scholarship_level");
            Dc.SetFilter("school_group =" + level_School + "");
            Dc.Filter();
        }

        private void SaveList()
        {
            for (int i = 1; i <= DwMain.RowCount; i++)
            {
                DwMain.SetItemString(i, "member_no", ArStr[0]);
            }

            try
            {
                DwMain.SetTransaction(sqlca);
                DwMain.UpdateData();
                //DwUtil.UpdateDataWindow(DwMain, "as_capital.pbl", "asnchildlists");
                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกเรียบร้อยแล้ว");
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        private void JsPostDeleteRow()
        {
            Decimal select_flag;

            for (int i = 1; i <= DwMain.RowCount; i++)
            {
                select_flag = DwMain.GetItemDecimal(i, "select_flag");

                if (select_flag == 1)
                {
                    DwMain.DeleteRow(i);
                }
            }
        }

        private void JsPostInsertRow()
        {
            DwMain.InsertRow(0);
            int row = DwMain.RowCount;
            if (row >= 1)
            {
                for (int i = 1; i <= DwMain.RowCount; i++)
                {
                    DwMain.SetItemString(i, "child_surname", ArStr[1]);
                }
            }
        }
    }
}
