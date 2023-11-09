using System;
using CoreSavingLibrary;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using DataLibrary;
using System.Globalization;
using System.Data.OracleClient;
using Sybase.DataWindow;

namespace Saving.Applications.cmd
{
    public partial class w_sheet_cmducf_branchcode : PageWebSheet, WebSheet
    {
        String pbl = "cmd_ucfbranchcode.pbl";
        Sdt ta;

        protected String jsPostDelete;

        public void InitJsPostBack()
        {
            jsPostDelete = WebUtil.JsPostBack(this, "jsPostDelete");
        }

        public void WebSheetLoadBegin()
        {
            this.ConnectSQLCA();
            DwDetail.SetTransaction(sqlca);

            if (!IsPostBack)
            {
                DwMain.InsertRow(0);
                DwDetail.Retrieve();
            }
            else
            {
                this.RestoreContextDw(DwMain);
                this.RestoreContextDw(DwDetail);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            switch (eventArg)
            {
                case "jsPostDelete":
                    PostOnDelete();
                    break;

            }
        }

        public void SaveWebSheet()
        {
            try
            {
                String branch_code = "", branch_desc;

                branch_desc = DwMain.GetItemString(1, "branch_desc").Trim();
                try
                {
                    String se = @"select max(branch_code)as branch_code from ptucfbranchcode";
                    ta = WebUtil.QuerySdt(se);
                    if (ta.Next())
                    {
                        branch_code = ta.GetString("branch_code");
                    }
                    if (branch_code == null || branch_code == "")
                    {
                        branch_code = "0";
                    }
                    branch_code = Convert.ToString(Convert.ToDecimal(branch_code) + 1);

                    while (branch_code.Length < 3)
                    {
                        branch_code = "0" + branch_code;
                    }
                }
                catch { }
                try
                {
                    String insert = @"insert into ptucfbranchcode (branch_code,branch_desc)
                                values({0}, {1})";
                    insert = WebUtil.SQLFormat(insert, branch_code, branch_desc);
                    ta = WebUtil.QuerySdt(insert);
                    DwDetail.Retrieve();
                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
                    DwMain.SetItemString(1, "branch_desc", null);
                }
                catch { }
            }
            catch (Exception ex)
            { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
        }

        public void WebSheetLoadEnd()
        {
            if (DwMain.RowCount > 1)
            {
                DwMain.DeleteRow(DwMain.RowCount);
            }
            DwMain.SaveDataCache();
            DwDetail.SaveDataCache();
        }

        private void PostOnDelete()
        {
            String branchCode = "";
            Int32 row = Convert.ToInt32(HdR.Value);
            try
            {
                branchCode = DwDetail.GetItemString(row, "branch_code");
                String del = @"delete ptucfbranchcode where branch_code = {0}";
                del = WebUtil.SQLFormat(del, branchCode);
                ta = WebUtil.QuerySdt(del);
                DwDetail.Retrieve();
                LtServerMessage.Text = WebUtil.CompleteMessage("ทำการลบรายการ " + branchCode + " สำเร็จ");
            }
            catch (Exception ex)
            { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }

        }
    }
}