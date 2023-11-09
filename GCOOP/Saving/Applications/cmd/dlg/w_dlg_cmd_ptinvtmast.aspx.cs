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

    public partial class w_dlg_cmd_ptinvtmast : PageWebDialog, WebDialog
    {
        String pbl = "cmd_ptinvtmast.pbl";
        protected String postFindinvt;
        protected string jsPostInvtsubgrp;
        private string is_sql;

        #region WebDialog Main

        public void InitJsPostBack()
        {
            postFindinvt = WebUtil.JsPostBack(this, "postFindinvt");
            jsPostInvtsubgrp = WebUtil.JsPostBack(this, "jsPostInvtsubgrp");
        }

        public void WebDialogLoadBegin()
        {
            this.ConnectSQLCA();
            Dw_find.SetTransaction(sqlca);
            Dw_detail.SetTransaction(sqlca);
            is_sql = Dw_detail.GetSqlSelect();
            WebUtil.RetrieveDDDW(Dw_find, "invtgrp_code", pbl, null);

            if (!IsPostBack)
            {
                Dw_find.InsertRow(0);
                Dw_detail.Retrieve();
                HSqlTemp.Value = is_sql;

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
            else if (eventArg == "jsPostInvtsubgrp")
            {
                PostInvtsubgrp();
            }
        }

        public void WebDialogLoadEnd()
        {
            Dw_find.SaveDataCache();
            Dw_detail.SaveDataCache();
        }

        #endregion

        #region Fuction

        private void jspostFindinvt()
        {

            String ls_invt_id = "", ls_invt_name = "", ls_invt_grp = "", ls_invtsub_grp = "", ls_sqltext = "";
            String ls_temp = "", ls_order = "";

            try { ls_invt_id = Dw_find.GetItemString(1, "invt_id").Trim(); }
            catch { ls_invt_id = ""; }
            try { ls_invt_name = Dw_find.GetItemString(1, "invt_name").Trim(); }
            catch { ls_invt_name = ""; }
            try { ls_invt_grp = Dw_find.GetItemString(1, "invtgrp_code").Trim(); }
            catch { ls_invt_grp = ""; }
            try { ls_invtsub_grp = Dw_find.GetItemString(1, "invtsubgrp_code").Trim(); }
            catch { ls_invtsub_grp = ""; }

            if (ls_invt_id.Length > 0)
            {
                ls_sqltext += "AND ( PTINVTMAST.INVT_ID LIKE  '%" + ls_invt_id + "%') ";
            }
            if (ls_invt_name.Length > 0)
            {
                ls_sqltext += "AND ( PTINVTMAST.INVT_NAME LIKE  '%" + ls_invt_name + "%') ";
            }
            if (ls_invt_grp.Length > 0)
            {
                ls_sqltext += "AND ( PTUCFINVTGROUPCODE.INVTGRP_CODE like '%" + ls_invt_grp + "%') ";
            }
            if (ls_invtsub_grp.Length > 0)
            { ls_sqltext += "and (PTINVTMAST.INVTSUBGRP_CODE like '%" + ls_invtsub_grp +"%')"; }
            ls_order = "ORDER BY INVT_ID ASC";

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

        private void PostInvtsubgrp()
        {
            string invtgrp = "";
            invtgrp = Dw_find.GetItemString(1, "invtgrp_code");
            DwUtil.RetrieveDDDW(Dw_find, "invtsubgrp_code", pbl, invtgrp);
        }

        #endregion
    }
}
