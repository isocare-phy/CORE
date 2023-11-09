using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommonLibrary;
//using CommonLibrary.WsShrlon;
//using CommonLibrary.WsCommon;
using Sybase.DataWindow;
using System.Web.Services.Protocols;

namespace Saving.Applications.app_assist.dlg
{
    public partial class w_dlg_kt_cancel_slip_65 : PageWebDialog, WebDialog
    {

        protected String refresh;
        protected String search;

        public void InitJsPostBack()
        {
            refresh = WebUtil.JsPostBack(this, "refresh");
            search = WebUtil.JsPostBack(this, "search");

        }

        public void WebDialogLoadBegin()
        {
            if (!IsPostBack)
            {
                dw_detail.InsertRow(0);
            }
            else
            {

                dw_detail.RestoreContext();
            }
            DwUtil.RetrieveDataWindow(dw_detail, "kt_65years.pbl", null);



        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "refresh")
            {
                Refresh();
            }
            if (eventArg == "search")
            {
                Search();
            }
        }

        public void WebDialogLoadEnd()
        {
            try
            {
                if (dw_detail.RowCount > 1)
                {
                    dw_detail.DeleteRow(2);
                }
            }
            catch { }

            dw_detail.SaveDataCache();

        }
        public void Refresh() { }
        protected void Search()
        {
            //DateTime startdate, enddate;
            try
            {

                //string as_startdate = dw_cri.GetItemString(1, "startdate");
                //string as_enddate = dw_cri.GetItemString(1, "enddate");
                //as_startdate = as_startdate.Replace("/", "");
                //as_enddate = as_enddate.Replace("/", "");
                //startdate = DateTime.ParseExact(as_startdate, "ddMMyyyy", WebUtil.TH);
                //enddate = DateTime.ParseExact(as_enddate, "ddMMyyyy", WebUtil.TH);
                //DwUtil.RetrieveDataWindow(dw_detail, "kt_pension.pbl", null, startdate, enddate);
            }
            catch
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอกวันที่ให้ถูกต้อง ตัวอย่าง(01122555 หรือ 01/12/2555)");
            }



        }



    }
}
