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
    public partial class w_sheet_ln_ucf_loangroup : PageWebSheet, WebSheet
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
            string sqldel = "", loanpermgrp_code = "";
            
            try
            {
                loanpermgrp_code = DwMain.GetItemString(r, "loanpermgrp_code");
                if (loanpermgrp_code != "" && loanpermgrp_code != null)
                {
                    sqldel = "delete from lccfgrploanpermiss where loanpermgrp_code='" + loanpermgrp_code + "'";
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
            string sqlcount = "", loanpermgrp_desc = "", loanpermgrp_code = "";
            Decimal maxpermiss_amt = 0;
            int count = 0;
            int r = DwMain.RowCount;
            loanpermgrp_code = DwMain.GetItemString(r, "loanpermgrp_code");
            sqlcount = "select count(loanpermgrp_code) as countrow from lccfgrploanpermiss where coop_id ='" + state.SsCoopId + "'";
            Sdt c = WebUtil.QuerySdt(sqlcount);
            while (c.Next())
            {
                count = Convert.ToInt32(c.GetDecimal("countrow"));
            }
            //update
            for (int i = 1; i < count + 1; i++)
            {
                loanpermgrp_desc = DwMain.GetItemString(i, "loanpermgrp_desc");
                maxpermiss_amt = DwMain.GetItemDecimal(i, "maxpermiss_amt");
                sqlupdate = "update lccfgrploanpermiss set loanpermgrp_desc='" + loanpermgrp_desc + "',maxpermiss_amt='" + maxpermiss_amt + "' where loanpermgrp_code ='" + loanpermgrp_code + "'";
                Sdt up = WebUtil.QuerySdt(sqlupdate);
            }
            //insert
            for (int i = count+1; i < r + 1; i++)
            {
                loanpermgrp_code = DwMain.GetItemString(i, "loanpermgrp_code");
                loanpermgrp_desc = DwMain.GetItemString(i, "loanpermgrp_desc");
                maxpermiss_amt = DwMain.GetItemDecimal(i, "maxpermiss_amt");
                sqlinsert = @"  INSERT INTO lccfgrploanpermiss  
                ( coop_id,loanpermgrp_code,loanpermgrp_desc,maxpermiss_amt)  
                VALUES ( '" + state.SsCoopId + "','" + loanpermgrp_code + "','" + loanpermgrp_desc + "','" + maxpermiss_amt + "')";
                Sdt up = WebUtil.QuerySdt(sqlinsert);
            }
        }
    }
}