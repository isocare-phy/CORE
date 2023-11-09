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
    public partial class w_sheet_cmducf_deptcode : PageWebSheet, WebSheet
    {
        String pbl = "cmd_ucfdeptcode.pbl";
        Sdt ta;
        protected String jsPostDelete;
        protected String jsPostSetData;

        public void InitJsPostBack()
        {
            jsPostDelete = WebUtil.JsPostBack(this, "jsPostDelete");
            jsPostSetData = WebUtil.JsPostBack(this, "jsPostSetData");
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
                case "jsPostSetData":
                    PostSetData();
                    break;
            }
        }

        public void SaveWebSheet()
        {
            try
            {
                String dept_code = "", dept_desc = "", dept_abb = "";
                if (HdStatus.Value == "Update")
                {
                    dept_code = DwMain.GetItemString(1, "dept_code").Trim();
                    dept_desc = DwMain.GetItemString(1, "dept_desc").Trim();
                    dept_abb = DwMain.GetItemString(1, "dept_abb").Trim();
                    String up = "update ptucfdeptcode set dept_desc = {0}, dept_abb = {1} where dept_code = {2}";
                    up = WebUtil.SQLFormat(up, dept_desc, dept_abb, dept_code);
                    ta = WebUtil.QuerySdt(up);
                    LtServerMessage.Text = WebUtil.CompleteMessage("อัดเดทข้อมูลแผนก " + dept_code + " สำเร็จ");
                    HdStatus.Value = null;
                    DwMain.Reset();
                    DwMain.InsertRow(0);
                    DwDetail.Retrieve();
                }
                else
                {
                    dept_desc = DwMain.GetItemString(1, "dept_desc").Trim();
                    dept_abb = DwMain.GetItemString(1, "dept_abb").Trim();
                    try
                    {
                        String se = @"select max(dept_code)as dept_code from ptucfdeptcode";
                        ta = WebUtil.QuerySdt(se);
                        if (ta.Next())
                        {
                            dept_code = ta.GetString("dept_code");
                        }
                        if (dept_code == null || dept_code == "")
                        {
                            dept_code = "0";
                        }
                        dept_code = Convert.ToString(Convert.ToDecimal(dept_code) + 1);

                        while (dept_code.Length < 3)
                        {
                            dept_code = "0" + dept_code;
                        }
                    }
                    catch { }
                    try
                    {
                        String insert = @"insert into ptucfdeptcode (dept_code,dept_desc, dept_abb)
                                values( {0}, {1}, {2})";
                        insert = WebUtil.SQLFormat(insert, dept_code, dept_desc, dept_abb);
                        ta = WebUtil.QuerySdt(insert);
                        LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
                        DwMain.Reset();
                        DwMain.InsertRow(0);
                        DwDetail.Retrieve();
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }

        }

        public void WebSheetLoadEnd()
        {
            DwMain.SaveDataCache();
            DwDetail.SaveDataCache();
        }

  #region ListFunction

        private void PostOnDelete()
        {
            String deptCode = "";
            Int32 row = Convert.ToInt32(HdR.Value);
            try
            {
                deptCode = DwDetail.GetItemString(row, "dept_code");
                String del = @"delete ptucfdeptcode where dept_code = {0}";
                del = WebUtil.SQLFormat(del, deptCode);
                ta = WebUtil.QuerySdt(del);
                DwDetail.Retrieve();
                LtServerMessage.Text = WebUtil.CompleteMessage("ทำการลบรายการ " + deptCode + " สำเร็จ");
            }
            catch (Exception ex)
            { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }

        }
        private void PostSetData()
        {
            Int32 rowse = Convert.ToInt32(HdR.Value);
            String dept_desc = "", dept_abb = "", dept_code = "";
            dept_code = DwDetail.GetItemString(rowse, "dept_code").Trim();
            dept_desc = DwDetail.GetItemString(rowse, "dept_desc").Trim();
            dept_abb = DwDetail.GetItemString(rowse, "dept_abb").Trim();
            DwMain.SetItemString(1, "dept_code", dept_code);
            DwMain.SetItemString(1, "dept_desc", dept_desc);
            DwMain.SetItemString(1, "dept_abb", dept_abb);
            HdStatus.Value = "Update";
        }

  #endregion
    }
}