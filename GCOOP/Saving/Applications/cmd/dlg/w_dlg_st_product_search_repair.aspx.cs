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
    public partial class w_dlg_st_product_search_repair : PageWebDialog, WebDialog
    {
        private DwThDate tDwMain;
        Sdt dt = new Sdt();
        private string is_sql;
        string pbl = "dlg_search.pbl";

        protected string jsPostSearch;
        #region WebDialog Member

        public void InitJsPostBack()
        {
            tDwMain = new DwThDate(DwMain, this);
            tDwMain.Add("start_date", "start_tdate");
            tDwMain.Add("end_date", "end_tdate");

            jsPostSearch = WebUtil.JsPostBack(this, "jsPostSearch");
        }

        public void WebDialogLoadBegin()
        {
            this.ConnectSQLCA();
            DwDetail.SetTransaction(sqlca);
            is_sql = DwDetail.GetSqlSelect();

            if (!IsPostBack)
            {
                DwMain.InsertRow(0);
                DwMain.SetItemDateTime(1, "start_date", state.SsWorkDate);
                DwMain.SetItemDateTime(1, "end_date", state.SsWorkDate);
                DwDetail.Retrieve();
            }
            else
            {
                this.RestoreContextDw(DwMain, tDwMain);
                this.RestoreContextDw(DwDetail);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "jsPostSearch")
            {
                PostFindReqrepair();
            }
        }

        public void WebDialogLoadEnd()
        {
            DwMain.SaveDataCache();
            DwDetail.SaveDataCache();
            tDwMain.Eng2ThaiAllRow();
        }

        #endregion

        #region Function

        private void PostFindReqrepair()
        {
            string reqrepair_no = "", reqrepair_name = "";
            string ls_sqltext = "", ls_order = "", ls_temp = "";
            DateTime start_date = new DateTime(), end_date = new DateTime();

            try { reqrepair_no = DwMain.GetItemString(1, "reqrepair_no"); }
            catch { reqrepair_no = string.Empty; }
            try { reqrepair_name = DwMain.GetItemString(1, "reqrepair_name"); }
            catch { reqrepair_name = string.Empty; }
            start_date = DwMain.GetItemDateTime(1, "start_date");
            end_date = DwMain.GetItemDateTime(1, "end_date");

            if (reqrepair_no.Length > 0)
            {
                ls_sqltext += " and ( ptreqdurtrepair.reqrepair_no like '%" + reqrepair_no + "%') ";
            }
            if (reqrepair_name.Length > 0)
            {
                ls_sqltext += " and ( ptreqdurtrepair.reqrepair_name like '%" + reqrepair_name + "%') ";
            }
            ls_sqltext += "and (ptreqdurtrepair.reqrepair_date between {0} and {1})";
            ls_sqltext = WebUtil.SQLFormat(ls_sqltext, start_date, end_date);
            ls_order = " order by reqrepair_no asc";
            ls_temp = is_sql + ls_sqltext + ls_order;
            HSqlTemp.Value = ls_temp;
            DwDetail.SetSqlSelect(HSqlTemp.Value);
            DwDetail.Retrieve();

            if (DwDetail.RowCount < 1)
            {
                LtServerMessage.Text = WebUtil.CompleteMessage("ไม่พบรายการรหัสที่ค้นหา");
                DwDetail.Reset();
                DwDetail.Retrieve();
            }
        }

        #endregion

    }
}
