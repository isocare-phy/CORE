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
using Sybase.DataWindow;
using DataLibrary;
using System.Data.OracleClient;

namespace Saving.Applications.cmd.dlg
{

    public partial class w_dlg_cmd_ptdurtmaster : PageWebDialog, WebDialog
    {
        protected string postFindinvt;
        protected string jsPostRetrieveSubgrp;

        private string is_sql;
        string pbl = "cmd_ptdurtmaster.pbl";
        //========================
        
        #region WebDialog Members

        public void InitJsPostBack()
        {
            postFindinvt = WebUtil.JsPostBack(this, "postFindinvt");
            jsPostRetrieveSubgrp = WebUtil.JsPostBack(this, "jsPostRetrieveSubgrp");
        }

        public void WebDialogLoadBegin()
        {
            this.ConnectSQLCA();
            Dw_find.SetTransaction(sqlca);
            Dw_detail.SetTransaction(sqlca);
            is_sql = Dw_detail.GetSqlSelect();

            if (!IsPostBack)
            {
                Dw_find.InsertRow(0);
                DwUtil.RetrieveDDDW(Dw_find, "branch_code", pbl, null);
                jspostFindinvt();
            }
            else
            {
                this.RestoreContextDw(Dw_find);
                this.RestoreContextDw(Dw_detail);
            }

        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postFindinvt")
            {
                jspostFindinvt();
            }
            else if (eventArg == "jsPostRetrieveSubgrp")
            {
                PostRetrieveSubgrp();
            }
        }

        public void WebDialogLoadEnd()
        {
            Dw_find.SaveDataCache();
            Dw_detail.SaveDataCache();
        }
        #endregion

        private void PostRetrieveSubgrp()
        {
            String durtgrp_code = Dw_find.GetItemString(1, "durtgrp_code").Trim();
            DwUtil.RetrieveDDDW(Dw_find, "durtgrpsub_code", pbl, durtgrp_code);
        }

        private void jspostFindinvt()
        {

            string ls_durt_id, ls_durt_name, ls_durtgrp_code, ls_durtgrpsub_code, ls_branchcode, ls_durtregno;
            string ls_sqltext = "", ls_temp, ls_order = "";

            try { ls_durt_id = Dw_find.GetItemString(1, "durt_id"); }
            catch { ls_durt_id = ""; }
            try { ls_branchcode = Dw_find.GetItemString(1, "branch_code"); }
            catch { ls_branchcode = ""; }
            try { ls_durtregno = Dw_find.GetItemString(1, "durt_regno"); }
            catch { ls_durtregno = ""; }
            try { ls_durt_name = Dw_find.GetItemString(1, "durt_name"); }
            catch { ls_durt_name = ""; }
            try { ls_durtgrp_code = Dw_find.GetItemString(1, "durtgrp_code"); }
            catch { ls_durtgrp_code = string.Empty; }
            try { ls_durtgrpsub_code = Dw_find.GetItemString(1, "durtgrpsub_code"); }
            catch { ls_durtgrpsub_code = string.Empty; }

            if (ls_durt_id.Length > 0)
            {
                ls_sqltext += " and ( PTDURTMASTER.DURT_ID LIKE  '%" + ls_durt_id + "%') ";
            }
            if (ls_branchcode.Length > 0)
            {
                ls_sqltext += " and ( PTDURTMASTER.BRANCH_CODE LIKE  '%" + ls_branchcode + "%') ";
            }
            if (ls_durtregno.Length > 0)
            {
                ls_sqltext += " and ( PTDURTMASTER.DURT_REGNO LIKE  '%" + ls_durtregno + "%') ";
            }
            if (ls_durt_name.Length > 0)
            {
                ls_sqltext += " and ( PTDURTMASTER.DURT_NAME LIKE  '%" + ls_durt_name + "%') ";
            }
            if (ls_durtgrp_code.Length > 0)
            {
                ls_sqltext += " and ( PTDURTMASTER.DURTGRP_CODE LIKE  '%" + ls_durtgrp_code + "%') ";
            }
            if (ls_durtgrpsub_code.Length > 0)
            {
                ls_sqltext += " and ( PTDURTMASTER.DURTGRPSUB_CODE LIKE  '%" + ls_durtgrpsub_code + "%') ";
            }
            ls_order = " ORDER BY DURT_ID ASC";
            ls_temp = is_sql + ls_sqltext + ls_order;
            HSqlTemp.Value = ls_temp;
            Dw_detail.SetSqlSelect(HSqlTemp.Value);
            Dw_detail.Retrieve();

            if (Dw_detail.RowCount < 1)
            {
                LtServerMessage.Text = WebUtil.CompleteMessage("ไม่พบรายการรหัสที่ค้นหา");
                Dw_detail.Reset();
            }

        }
    }
}
