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
using CoreSavingLibrary.WcfNCommon;

namespace Saving.Applications.cmd
{
    public partial class w_sheet_cmducf_invtsubgroup : PageWebSheet, WebSheet
    {
        String pbl = "cmd_ucfinvtsubgroupcode.pbl";
        Sdt ta;

        protected string jsPostRetriveInvtgrpsub;
        protected string jsPostSetEdit;

        public void InitJsPostBack()
        {
            jsPostRetriveInvtgrpsub = WebUtil.JsPostBack(this, "jsPostRetriveInvtgrpsub");
            jsPostSetEdit = WebUtil.JsPostBack(this, "jsPostSetEdit");
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                Session["invgrpCode"] = "";
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
                case "jsPostRetriveInvtgrpsub":
                    PostRetriveInvtgrpsub();
                    break;
                case "jsPostSetEdit":
                    Int32 rowse = Convert.ToInt32(HdR.Value);
                    HdSaveMode.Value = "U";
                    DwMain.SetItemString(1, "invtsubgrp_code", DwDetail.GetItemString(rowse, "invtsubgrp_code"));
                    DwMain.SetItemString(1, "invtsubgrp_desc", DwDetail.GetItemString(rowse, "invtsubgrp_desc"));
                    break;
            }
        }

        public void SaveWebSheet()
        {
            try
            {
                string invgrp_code = "", invtsubgrp_code = "", invtsubgrp_desc = "";
                invgrp_code = DwMain.GetItemString(1, "invtgrp_code");
                invtsubgrp_code = DwMain.GetItemString(1, "invtsubgrp_code");
                invtsubgrp_desc = DwMain.GetItemString(1, "invtsubgrp_desc").Trim();
                if (HdSaveMode.Value == "I" && invtsubgrp_code == "AUTO")
                {
                    invtsubgrp_code = GetDiffInvtGrpSubCode(invgrp_code);
                    if (invtsubgrp_code == null || invtsubgrp_code == "")
                    { invtsubgrp_code = GetMaxInvtGrpSubCode(invgrp_code); }

                    string insql = @"insert into ptucfinvtsubgroupcode values ({0}, {2}, {1})";
                    insql = WebUtil.SQLFormat(insql, invgrp_code, invtsubgrp_code, invtsubgrp_desc);
                    ta = WebUtil.QuerySdt(insql);
                }
                else
                {
                    string upsql = @"update ptucfinvtsubgroupcode set invtsubgrp_desc = {2} where invtgrp_code = {0} and invtsubgrp_code = {1}";
                    upsql = WebUtil.SQLFormat(upsql, invgrp_code, invtsubgrp_code, invtsubgrp_desc);
                    ta = WebUtil.QuerySdt(upsql);
                }
                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกหน่วยนับสำเร็จ รหัส " + invtsubgrp_code);
                Session["invgrpCode"] = invgrp_code;
                ResetPage();
            }
            catch (Exception ex)
            { LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message); }
        }

        public void WebSheetLoadEnd()
        {
            DwMain.SaveDataCache();
            DwDetail.SaveDataCache();
        }

        #region function
        public void ResetPage()
        {
            DwMain.Reset();
            DwDetail.Reset();
            DwMain.InsertRow(0);
            string invtgrp_code = Session["invgrpCode"].ToString();
            DwUtil.RetrieveDDDW(DwMain, "invtgrp_code", pbl, null);
            DwMain.SetItemString(1, "invtgrp_code", Session["invgrpCode"].ToString());
            DwMain.SetItemString(1, "invtsubgrp_code", "AUTO");
            HdSaveMode.Value = "I";
            PostRetriveInvtgrpsub();
        }

        private void PostRetriveInvtgrpsub()
        {
            string invtgrp_code = DwMain.GetItemString(1, "invtgrp_code");
            DwUtil.RetrieveDataWindow(DwDetail, pbl, null, invtgrp_code);
        }

        private string GetDiffInvtGrpSubCode(string invtgrp_code)
        {
            string invtsubgrp_code = "";
            try
            {
                string sql = @"select runcont_no from (select rownum as runcont_no from dual c 
                connect by level <= ( select ( to_number( max( invtsubgrp_code ) ) - to_number( min( invtsubgrp_code ) ) ) as diffcont 
                from ptucfinvtsubgroupcode where invtgrp_code = {0}) 
                minus select to_number( invtsubgrp_code ) from ptucfinvtsubgroupcode where invtgrp_code = {0}) order by runcont_no asc";
                sql = WebUtil.SQLFormat(sql, invtgrp_code);
                ta = WebUtil.QuerySdt(sql);
                if (ta.Next())
                {
                    invtsubgrp_code = ta.GetDecimal("runcont_no").ToString("000");
                }
            }
            catch (Exception ex)
            { LtServerMessage.Text = WebUtil.WarningMessage(ex.Message); }
            return invtsubgrp_code;
        }

        private string GetMaxInvtGrpSubCode(string invtgrp_code)
        {
            string invtsubgrp_code = "";
            try
            {
                String se = @"select max(invtsubgrp_code)as invtsubgrp_code from ptucfinvtsubgroupcode where invtgrp_code = {0} ";
                se = WebUtil.SQLFormat(se, invtgrp_code);
                ta = WebUtil.QuerySdt(se);
                if (ta.Next())
                {
                    invtsubgrp_code = ta.GetString("invtsubgrp_code");
                }
                if (invtsubgrp_code == null || invtsubgrp_code == "")
                {
                    invtsubgrp_code = "0";
                }
                invtsubgrp_code = (Convert.ToDecimal(invtsubgrp_code) + 1).ToString("000");
            }
            catch { }
            return invtsubgrp_code;
        }
        #endregion
    }
}