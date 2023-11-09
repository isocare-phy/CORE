using System;
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
using Saving.CmConfig;
using Sybase.DataWindow;
using Saving.WsShrlon;
using Saving.WsCommon;

namespace Saving.Applications.loantracking.dlg
{
    public partial class w_dlg_divavg_memgroup : PageWebDialog,WebDialog 
    {

        private Shrlon shrlonService;
        private Common commonService;
        public String postSearchMemGroup;
        //=============================
        protected void JsNewClear()
        {
            Dw_main.Reset();
            Dw_main.InsertRow(0);
            Dw_detail.Reset();
        }

        private void JspostSearchMemGroup()
        {
            try
            {
                str_divavg astr_divavg = new str_divavg();
                String xml_head = Dw_main.Describe("DataWindow.Data.XML");
                int result = shrlonService.SearchListMemGroup(state.SsWsPass, ref astr_divavg, xml_head);
                if (result == 1)
                {
                    String xml_detail = astr_divavg.xml_detail;
                    Dw_detail.Reset();
                    Dw_detail.ImportString(xml_detail, FileSaveAsType.Xml);
                }
                else
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบข้อมูล กรุณาตรวจสอบใหม่");
                    JsNewClear();
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
                JsNewClear();
            }
        }

        #region WebDialog Members

        public void InitJsPostBack()
        {
            postSearchMemGroup = WebUtil.JsPostBack(this, "postSearchMemGroup");
        }

        public void WebDialogLoadBegin()
        {
            try
            {
                shrlonService = new Shrlon();
                commonService = new Common();

            }
            catch
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ติดต่อ Web Service ไม่ได้");
            }

            try
            {
                this.ConnectSQLCA();
            }
            catch { LtServerMessage.Text = WebUtil.CompleteMessage("ติดต่อ Database ไม่ได้"); }

            Dw_main.SetTransaction(sqlca);
            Dw_detail.SetTransaction(sqlca);

            if (!IsPostBack)
            {
                String B_name = "";
                try
                {
                    B_name = Request["B_name"].Trim();
                    HdB_name.Value = B_name;
                }
                catch { }

                JsNewClear();
            }
            else
            {
                this.RestoreContextDw(Dw_main);
                this.RestoreContextDw(Dw_detail);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postSearchMemGroup")
            {
                JspostSearchMemGroup();
            }
        }

        public void WebDialogLoadEnd()
        {
            if (Dw_main.RowCount > 1)
            {
                Dw_main.DeleteRow(Dw_main.RowCount);
            }
            Dw_main.SaveDataCache();
            Dw_detail.SaveDataCache();
        }

        #endregion
    }
}
