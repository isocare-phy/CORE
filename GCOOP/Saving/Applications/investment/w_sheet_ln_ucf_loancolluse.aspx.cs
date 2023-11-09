using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary.WcfNCommon;
using DataLibrary;
using Sybase.DataWindow;

namespace Saving.Applications.investment
{
    public partial class w_sheet_ln_ucf_loancolluse : PageWebSheet, WebSheet
    {
        protected String jsPostTypeChange;
        protected String jsPostDelete;
        string pbl = "loan_ucf.pbl";

        #region WebSheet Members
        public void InitJsPostBack()
        {
            jsPostTypeChange = WebUtil.JsPostBack(this, "jsPostTypeChange");
            jsPostDelete = WebUtil.JsPostBack(this, "jsPostDelete");
        }

        public void WebSheetLoadBegin()
        {
            this.ConnectSQLCA();
            DwMain.SetTransaction(sqlca);
            if (!IsPostBack)
            {
                DwHead.InsertRow(0);
                DwMain.InsertRow(0);
            }
            else
            {
                this.RestoreContextDw(DwHead);
                this.RestoreContextDw(DwMain);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "jsPostTypeChange")
            {
                TypeChange();
            }
            else if (eventArg == "jsPostDelete")
            {
                DeleteData();
            }
        }

        public void SaveWebSheet()
        {
            try
            {
                string loantype_code = DwHead.GetItemString(1, "loantype_code");
                string loancolltype_code = DwHead.GetItemString(1, "loancolltype_code");
                string collmasttype_code = DwHead.GetItemString(1, "collmasttype_code");
                string coll_percent = DwMain.GetItemDecimal(1, "coll_percent").ToString();
                if (Hd_status.Value == "true")
                {
                    string sqlupdate = @"UPDATE lccfloantypecolluse SET coll_percent = '" + coll_percent + "' WHERE loantype_code = '" + loantype_code +
                        "' and loancolltype_code = '" + loancolltype_code + "' and collmasttype_code = '" + collmasttype_code + "'";
                    Sdt update = WebUtil.QuerySdt(sqlupdate);
                }
                else
                {
                    string sqlinsert = @"INSERT INTO lccfloantypecolluse VALUES('001001', '" + loantype_code + "', '" + loancolltype_code + "', '" +
                        collmasttype_code + "', '" + coll_percent + "')";
                    Sdt insert = WebUtil.QuerySdt(sqlinsert);
                }

                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลสำเร็จ");
            }
            catch
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("บันทึกข้อมูลไม่สำเร็จ");
            }
        }

        public void WebSheetLoadEnd()
        {
            try
            {
                DwUtil.RetrieveDDDW(DwHead, "loantype_code", pbl, null);
            }
            catch { }
            try
            {
                DwUtil.RetrieveDDDW(DwHead, "loancolltype_code", pbl, null);
            }
            catch { }
            try
            {
                DwUtil.RetrieveDDDW(DwHead, "collmasttype_code", pbl, null);
            }
            catch { }
            DwHead.SaveDataCache();
            DwMain.SaveDataCache();
        }
        #endregion

        private void TypeChange()
        {
            string loantype_code = DwHead.GetItemString(1, "loantype_code");
            string loancolltype_code = DwHead.GetItemString(1, "loancolltype_code");
            string collmasttype_code = DwHead.GetItemString(1, "collmasttype_code");
            string sqlselect = @"SELECT * FROM lccfloantypecolluse WHERE loantype_code = '" + loantype_code + "' and loancolltype_code = '" + loancolltype_code + 
                "' and collmasttype_code = '" + collmasttype_code + "'";
            Sdt select = WebUtil.QuerySdt(sqlselect);
            if (select.Next())
            {
                DwUtil.RetrieveDataWindow(DwMain, pbl, null, loantype_code, loancolltype_code, collmasttype_code);
                Hd_status.Value = "true";
            }
            else
            {
                DwMain.Reset();
                DwMain.InsertRow(0);
                LtServerMessage.Text = WebUtil.WarningMessage("ยังไม่มีข้อมูล");
                Hd_status.Value = "false";
            }

        }

        private void DeleteData()
        {
            try
            {
                string loantype_code = DwHead.GetItemString(1, "loantype_code");
                string loancolltype_code = DwHead.GetItemString(1, "loancolltype_code");
                string collmasttype_code = DwHead.GetItemString(1, "collmasttype_code");
                string coll_percent = DwMain.GetItemDecimal(1, "coll_percent").ToString();
                if (Hd_status.Value == "true")
                {
                    string sqldelete = @"DELETE FROM lccfloantypecolluse WHERE loantype_code = '" + loantype_code + "' and loancolltype_code = '" + loancolltype_code +
                        "' and collmasttype_code = '" + collmasttype_code + "'";
                    Sdt delete = WebUtil.QuerySdt(sqldelete);
                }
                DwMain.SetItemNull(1, "coll_percent");

                LtServerMessage.Text = WebUtil.CompleteMessage("ลบข้อมูลสำเร็จ");
            }
            catch
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ลบข้อมูลไม่สำเร็จ");
            }
        }
    }
}