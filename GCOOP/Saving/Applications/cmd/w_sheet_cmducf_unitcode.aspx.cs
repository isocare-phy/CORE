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
    public partial class w_sheet_cmducf_unitcode : PageWebSheet, WebSheet
    {
        String pbl = "cmd_ucfunitcode.pbl";
        Sdt ta;
        protected string jsPostDelete;
        protected string jsPostSetEdit;
        protected string jsPostInsertDwDetail; 

        public void InitJsPostBack()
        {
            jsPostDelete = WebUtil.JsPostBack(this, "jsPostDelete");
            jsPostSetEdit = WebUtil.JsPostBack(this, "jsPostSetEdit");
            jsPostInsertDwDetail = WebUtil.JsPostBack(this, "jsPostInsertDwDetail");
        }

        public void WebSheetLoadBegin()
        {
            this.ConnectSQLCA();
            DwDetail.SetTransaction(sqlca);

            if (!IsPostBack)
            {
                ResetPage();
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
                case "jsPostInsertDwDetail":
                    DwDetail.InsertRow(0);
                    string unit_code = GetMaxUnitCode();
                    DwDetail.SetItemString(DwDetail.RowCount, "unit_code", unit_code);
                    HdSaveMode.Value = "I";
                    break;
                case "jsPostSetEdit":
                    Int32 rowse = Convert.ToInt32(HdR.Value);
                    HdSaveMode.Value = "U";
                    DwMain.SetItemString(1, "unit_code", DwDetail.GetItemString(rowse, "unit_code"));
                    DwMain.SetItemString(1, "unit_desc", DwDetail.GetItemString(rowse, "unit_desc"));
                    break;
            }
        }

        public void SaveWebSheet()
        {
            try
            {
                string unit_code = "", unit_desc = "";
                unit_code = DwMain.GetItemString(1, "unit_code");
                unit_desc = DwMain.GetItemString(1, "unit_desc").Trim();
                if (HdSaveMode.Value == "I" && unit_code == "AUTO")
                {
                    unit_code = GetDiffUnitCode();
                    if (unit_code == null || unit_code == "")
                    { unit_code = GetMaxUnitCode(); }

                    string insql = @"insert into ptucfunitcode values ({0}, {1})";
                    insql = WebUtil.SQLFormat(insql, unit_code, unit_desc);
                    ta = WebUtil.QuerySdt(insql);
                }
                else
                {
                    string upsql = @"update ptucfunitcode set unit_desc = {1} where unit_code = {0}";
                    upsql = WebUtil.SQLFormat(upsql, unit_code, unit_desc);
                    ta = WebUtil.QuerySdt(upsql);
                }
                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกหน่วยนับสำเร็จ รหัส " + unit_code );
                ResetPage();
            }
            catch (Exception ex)
            { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
        }

        public void WebSheetLoadEnd()
        {
            DwMain.SaveDataCache();
            DwDetail.SaveDataCache();
        }

        private void PostOnDelete()
        {
            String unitCode = "";
            Int32 row = Convert.ToInt32(HdR.Value);
            try
            {
                unitCode = DwDetail.GetItemString(row, "unit_code");
                String del = @"delete ptucfunitcode where unit_code = '" + unitCode + "'";
                ta = WebUtil.QuerySdt(del);
                DwDetail.Retrieve();
                LtServerMessage.Text = WebUtil.CompleteMessage("ทำการลบรายการ " + unitCode + " สำเร็จ");
            }
            catch (Exception ex)
            { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }

        }

        private string GetMaxUnitCode()
        {
            string unit_code = "";
            try
            {
                String se = @"select max(unit_code)as unit_code from ptucfunitcode";
                ta = WebUtil.QuerySdt(se);
                if (ta.Next())
                {
                    unit_code = ta.GetString("unit_code");
                }
                if (unit_code == null || unit_code == "")
                {
                    unit_code = "0";
                }
                unit_code = (Convert.ToDecimal(unit_code) + 1).ToString("000");
            }
            catch { }
            return unit_code;
        }

        private string GetDiffUnitCode()
        {
            string unit_code = "";
            try
            {
                string sql = @"select runcont_no from (select rownum as runcont_no from dual c 
                connect by level <= ( select ( to_number( max( unit_code ) ) - to_number( min( unit_code ) ) ) as diffcont 
                from ptucfunitcode) minus select to_number( unit_code ) from ptucfunitcode) order by runcont_no asc";
                ta = WebUtil.QuerySdt(sql);
                if (ta.Next())
                {
                    unit_code = ta.GetDecimal("runcont_no").ToString("000");
                }
            }
            catch (Exception ex)
            { LtServerMessage.Text = WebUtil.WarningMessage(ex.Message); }
            return unit_code;
        }

        public void ResetPage()
        {
            DwMain.Reset();
            DwDetail.Reset();
            DwMain.InsertRow(0);
            DwMain.SetItemString(1, "unit_code", "AUTO");
            HdSaveMode.Value = "I";
            DwUtil.RetrieveDataWindow(DwDetail, pbl, null);
        }
    }
}       