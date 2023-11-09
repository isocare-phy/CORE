using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Saving;
using DataLibrary;
using Sybase.DataWindow;


namespace Saving.Applications.trading.dlg
{
    public partial class w_dlg_td_search_memb : PageWebDialog, WebDialog
    {
        protected String jsSearch;

        public void InitJsPostBack()
        {
            jsSearch = WebUtil.JsPostBack(this, "jsSearch");
        }

        public void WebDialogLoadBegin()
        {
            if (!IsPostBack)
            {
                try
                {
                    HdMembStauts.Value = Request["member_status"].ToString().Trim();
                }
                catch
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("เกิดข้อผิดพลาดทางเทคนิค กรุณากดใหม่");
                    return;
                }
                DwCri.InsertRow(0);
            }
            else
            {
                this.RestoreContextDw(DwCri);
                this.RestoreContextDw(DwDetail);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "jsSearch")
            {
                JsSearch();
            }
        }

        public void WebDialogLoadEnd()
        {
            DwCri.SaveDataCache();
            DwDetail.SaveDataCache();
        }

        public void JsSearch()
        {
            string as_member_no = "";
            string as_memb_name = "";

            try
            {
                as_member_no = "%" + DwCri.GetItemString(1, "member_no") + "%";
            }
            catch
            {
                as_member_no = "%";
            }
            try
            {
                as_memb_name = "%" + DwCri.GetItemString(1, "memb_name") + "%";
            }
            catch
            {
                as_memb_name = "%";
            }

            object[] args = new object[3];
            args[0] = as_member_no;
            args[1] = as_memb_name;
            args[2] = HdMembStauts.Value;


            DwUtil.RetrieveDataWindow(DwDetail, "dlg_dp_search_memb.pbl", null, args);
        }

    }
}