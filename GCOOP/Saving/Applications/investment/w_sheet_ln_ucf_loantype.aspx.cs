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
    public partial class w_sheet_ln_ucf_loantype : PageWebSheet, WebSheet
    {
        protected String jsPostDel;
        protected String jsPostInsert;
        protected String jsPostLoanType;
        protected String jsPostContintType;
        public String pbl = "ln_ucf_all.pbl";
        public void InitJsPostBack()
        {
            jsPostDel = WebUtil.JsPostBack(this, "jsPostDel");
            jsPostInsert = WebUtil.JsPostBack(this, "jsPostInsert");
            jsPostLoanType = WebUtil.JsPostBack(this, "jsPostLoanType");
            jsPostContintType = WebUtil.JsPostBack(this, "jsPostContintType");
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
                this.RestoreContextDw(DwDetail);
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
                case "jsPostLoanType":
                    JsPostLoanType();
                    break;
                case "jsPostContintType":
                    break;
            }
        }

        public void SaveWebSheet()
        {
            try
            {
                SaveInfo();
                DwUtil.RetrieveDataWindow(DwMain, pbl, null, null);
                if (DwDetail.RowCount > 0)
                {
                    SaveDetail();
                }
                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");

            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage("บันทึกไม่สำเร็จเนื่องจาก : " + ex); }
        }

        public void WebSheetLoadEnd()
        {
            try
            {
                DwUtil.RetrieveDDDW(DwMain, "loanpermgrp_code", pbl, null);
            }
            catch
            {
            }
            try
            {
                DwUtil.RetrieveDDDW(DwDetail, "loanpermgrp_code", pbl, null);
            }
            catch
            {
            }
            try
            {
                DwUtil.RetrieveDDDW(DwDetail, "defaultobj_code", pbl, null);
            }
            catch
            {
            }
            try
            {
                DwUtil.RetrieveDDDW(DwDetail, "inttabfix_code", pbl, null);
            }
            catch
            {
            }
            try
            {
                DwUtil.RetrieveDDDW(DwDetail, "inttabrate_code", pbl, null);
            }
            catch
            {
            }
            DwMain.SaveDataCache();
            DwDetail.SaveDataCache();
        }
        public void JsPostDel()
        {
            int r = Convert.ToInt32(HdRow.Value);
            string sqldel = "", loantype_code = "";
            try
            {
                loantype_code = DwMain.GetItemString(r, "loantype_code");
                sqldel = "delete from LCCFLOANTYPE where coop_id ='" + state.SsCoopId + "' and loantype_code='" + loantype_code + "'";
                Sdt del = WebUtil.QuerySdt(sqldel);
                DwMain.DeleteRow(r);
                DwDetail.Reset();
                LtServerMessage.Text = WebUtil.CompleteMessage("ลบแถวสำเร็จ");
            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }

        }
        public void JsPostLoanType()
        {
            int r = Convert.ToInt32(HdRow.Value);
            string loantype_code = "";
            try
            {
                loantype_code = DwMain.GetItemString(r, "loantype_code");
                DwUtil.RetrieveDataWindow(DwDetail, pbl, null,state.SsCoopControl, loantype_code);
                loantype_code = DwDetail.GetItemString(1, "loantype_code");
            }
            catch { LtServerMessage.Text = WebUtil.ErrorMessage("กณุณาบันทึกข้อมูล รหัสประเภทเงินกู้"); }
        }
        public void SaveInfo()
        {
            string sqlupdate = "", sqlinsert = "";
            string sqlcount = "", loantype_desc = "", loanpermgrp_code = "", loantype_code = "";
            int count = 0;
            int r = DwMain.RowCount;
            sqlcount = "select count(loantype_code) as countrow from LCCFLOANTYPE where coop_id ='" + state.SsCoopId + "'";
            Sdt c = WebUtil.QuerySdt(sqlcount);
            if (c.Next())
            {
                count = Convert.ToInt32(c.GetDecimal("countrow"));
            }
            //update
            for (int i = 1; i < count + 1; i++)
            {
                loantype_code = DwMain.GetItemString(i, "loantype_code");
                loantype_desc = DwMain.GetItemString(i, "loantype_desc");
                loanpermgrp_code = DwMain.GetItemString(i, "loanpermgrp_code");
                sqlupdate = "update LCCFLOANTYPE set loantype_desc='" + loantype_desc + "',loanpermgrp_code='" + loanpermgrp_code + "' where loantype_code='" + loantype_code + "' and coop_id ='" + state.SsCoopId + "'";
                Sdt up = WebUtil.QuerySdt(sqlupdate);
            }
            //insert
            for (int i = count + 1; i < r + 1; i++)
            {
                loantype_code = DwMain.GetItemString(i, "loantype_code");
                loantype_desc = DwMain.GetItemString(i, "loantype_desc");
                loanpermgrp_code = DwMain.GetItemString(i, "loanpermgrp_code");
                sqlinsert = @"  INSERT INTO LCCFLOANTYPE  
                ( coop_id,loantype_code,loantype_desc,loanpermgrp_code )  
                VALUES ( '" + state.SsCoopId + "','" + loantype_code + "','" + loantype_desc + "','" + loanpermgrp_code + "')";
                Sdt up = WebUtil.QuerySdt(sqlinsert);
            }
        }
        public void SaveDetail()
        {
            DwUtil.UpdateDataWindow(DwDetail, pbl, "LCCFLOANTYPE");
        }
    }
}