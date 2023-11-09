using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataLibrary;
using Sybase.DataWindow;

namespace Saving.Applications.investment
{
    public partial class w_sheet_ln_ucf_loanapprove : PageWebSheet, WebSheet
    {
        protected String jsPostDel;
        protected String jsPostInsert;
        public String pbl = "ln_ucf_all.pbl";
        public void InitJsPostBack()
        {
            jsPostDel = WebUtil.JsPostBack(this, "jsPostDel");
            jsPostInsert = WebUtil.JsPostBack(this, "jsPostInsert");
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                DwUtil.RetrieveDataWindow(DwMain, pbl, null, null);
            }
            else
            {
                this.RestoreContextDw(DwMain);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            switch (eventArg)
            {
                case "jsPostDel":
                    JsPostDel();
                    break;
                case "jsPostInsert":
                    DwMain.InsertRow(0);
                    DwMain.SetItemString(DwMain.RowCount, "coop_id", state.SsCoopId);
                    break;

            }
        }

        public void SaveWebSheet()
        {
            try
            {
                SaveInfo();
                DwUtil.RetrieveDataWindow(DwMain, pbl, null, null);
                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage("บันทึกไม่สำเร็จเนื่องจาก : " + ex); }

        }

        public void WebSheetLoadEnd()
        {
            DwMain.SaveDataCache();
        }
        public void JsPostDel()
        {
            int r = Convert.ToInt32(HdRow.Value);
            string sqldel = "", loanapproveby_code = "";
            try
            {
                loanapproveby_code = DwMain.GetItemString(r, "loanapproveby_code");
                sqldel = "delete from LCUCFLOANAPPROVEBY where coop_id ='" + state.SsCoopId + "' and loanapproveby_code='" + loanapproveby_code + "'";
                Sdt del = WebUtil.QuerySdt(sqldel);
                DwMain.DeleteRow(r);
                LtServerMessage.Text = WebUtil.CompleteMessage("ลบแถวสำเร็จ");
            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }

        }
        public void SaveInfo()
        {
            string sqlupdate = "", sqlinsert = "";
            string sqlcount = "", loanapproveby_desc = "", loanapproveby_code = "";
            int count = 0;
            int r = DwMain.RowCount;
            sqlcount = "select count(loanapproveby_code) as countrow from LCUCFLOANAPPROVEBY where coop_id ='" + state.SsCoopId + "'";
            Sdt c = WebUtil.QuerySdt(sqlcount);
            if (c.Next())
            {
                count = Convert.ToInt32(c.GetDecimal("countrow"));
            }
            //update
            for (int i = 1; i < count + 1; i++)
            {
                loanapproveby_code = DwMain.GetItemString(i, "loanapproveby_code");
                loanapproveby_desc = DwMain.GetItemString(i, "loanapproveby_desc");
                sqlupdate = "update LCUCFLOANAPPROVEBY set loanapproveby_desc='" + loanapproveby_desc + "' where loanapproveby_code='" + loanapproveby_code + "' and coop_id ='" + state.SsCoopId + "'";
                Sdt up = WebUtil.QuerySdt(sqlupdate);
            }
            //insert
            for (int i = count + 1; i < r + 1; i++)
            {
                loanapproveby_code = DwMain.GetItemString(i, "loanapproveby_code");
                loanapproveby_desc = DwMain.GetItemString(i, "loanapproveby_desc");
                sqlinsert = @"  INSERT INTO LCUCFLOANAPPROVEBY  
                ( coop_id,loanapproveby_code,loanapproveby_desc )  
                VALUES ( '" + state.SsCoopId + "','" + loanapproveby_code + "','" + loanapproveby_desc + "')";
                Sdt up = WebUtil.QuerySdt(sqlinsert);
            }
        }
    }
}