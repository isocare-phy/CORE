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
    public partial class w_sheet_ln_ucf_loanperiod : PageWebSheet, WebSheet
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
                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage("บันทึกไม่สำเร็จเนื่องจาก : "+ex); }

        }

        public void WebSheetLoadEnd()
        {
            DwMain.SaveDataCache();
        }
        public void JsPostDel()
        {
            int r = Convert.ToInt32(HdRow.Value);
            string sqldel = "", loantype_code = "";
            
            try
            {
                loantype_code = DwMain.GetItemString(r, "loantype_code");
                if (loantype_code != "" && loantype_code != null)
                {
                    sqldel = "delete from lccfloantypeperiod where coop_id='" + state.SsCoopId + "' and loantype_code='" + loantype_code + "'";
                    Sdt del = WebUtil.QuerySdt(sqldel);
                    DwMain.DeleteRow(r);
                    LtServerMessage.Text = WebUtil.CompleteMessage("ลบแถวสำเร็จ");
                }
                else {
                    DwMain.DeleteRow(r);
                    LtServerMessage.Text = WebUtil.CompleteMessage("ลบแถวสำเร็จ");
                }
            }
            catch
            {
                DwMain.DeleteRow(r);
                LtServerMessage.Text = WebUtil.CompleteMessage("ลบแถวสำเร็จ");
            }
        }
        public void SaveInfo()
        {
            string sqlupdate = "", sqlinsert = "";
            string sqlcount = "",loantype_code = "";
            Decimal seq_no = 0;
            Decimal money_from = 0;
            Decimal money_to = 0;
            Decimal max_period = 0;
            int count = 0;
            int r = DwMain.RowCount;
            loantype_code = DwMain.GetItemString(r, "loantype_code");
            sqlcount = "select count(loantype_code) as countrow from lccfloantypeperiod where coop_id='" + state.SsCoopId + "' ";
            Sdt c = WebUtil.QuerySdt(sqlcount);
            while (c.Next())
            {
                count = Convert.ToInt32(c.GetDecimal("countrow"));
            }
            //update
            for (int i = 1; i < count + 1; i++)
            {
                seq_no = DwMain.GetItemDecimal(i, "seq_no");
                seq_no = DwMain.GetItemDecimal(i, "money_from");
                seq_no = DwMain.GetItemDecimal(i, "money_to");
                seq_no = DwMain.GetItemDecimal(i, "max_period");
                sqlupdate = "update LCCFLOANTYPEPERIOD set seq_no='" + seq_no + "',money_from='" + money_from + "',money_to='" + money_to + "',max_period='" + max_period + "' where coop_id ='" + state.SsCoopId + "' and loantype_code='" + loantype_code + "'";
                Sdt up = WebUtil.QuerySdt(sqlupdate);
            }
            //insert
            for (int i = count+1; i < r + 1; i++)
            {
                loantype_code = DwMain.GetItemString(i, "loantype_code");
                seq_no = DwMain.GetItemDecimal(i, "seq_no");
                money_from = DwMain.GetItemDecimal(i, "money_from");
                money_to = DwMain.GetItemDecimal(i, "money_to");
                max_period = DwMain.GetItemDecimal(i, "max_period");
                sqlinsert = @"  INSERT INTO LCCFLOANTYPEPERIOD  
                ( coop_id,loantype_code,seq_no,money_from,money_to,max_period)  
                VALUES ( '" + state.SsCoopId + "','" + loantype_code + "','" + seq_no + "','" + money_from + "','" + money_to + "','" + max_period + "')";
                Sdt up = WebUtil.QuerySdt(sqlinsert);
            }
        }
    }
}