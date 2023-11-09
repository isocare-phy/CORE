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
using Sybase.DataWindow;
using CoreSavingLibrary.WcfAgency;
using CoreSavingLibrary.WcfCommon;
using System.Web.Services.Protocols;
using DataLibrary;
using System.Globalization;

namespace Saving.Applications.agency
{
    public partial class w_dlg_ag_agentprocess : PageWebSheet,WebSheet 
    {
        private AgencyClient agencyService;
        private CommonClient commonService;

        protected String postRefresh;
        protected String postAgentProcess;
        protected String postNewClear;

        //============================

        private void JspostAgentProcess()
        {
            try
            {
                try
                {
                    String xml_agentoption = Dw_main.Describe("Datawindow.Data.Xml");
                    agencyService.RunAgentProcess(state.SsWsPass, xml_agentoption, state.SsApplication, state.CurrentPage);
                    HdAgentProcess.Value = "true";
                }
                catch (SoapException ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(WebUtil.SoapMessage(ex));
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex.ToString());
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
            }
        }
        
        private void JspostYearMonth()
        {
            int rec_year = DateTime.Now.Year;
            rec_year = rec_year + 543;
            Dw_main.SetItemDecimal(1, "receive_year", rec_year);

            int rec_month = DateTime.Now.Month;
            Dw_main.SetItemDecimal(1, "receive_month", rec_month);

        }
        private void JspostNewClear()
        {
            Dw_main.Reset();
            Dw_main.InsertRow(0);
            JspostYearMonth();
        }

        #region WebSheet Members

        public void InitJsPostBack()
        {
            postRefresh = WebUtil.JsPostBack(this, "postRefresh");
            postAgentProcess = WebUtil.JsPostBack(this, "postAgentProcess");
            postNewClear = WebUtil.JsPostBack(this, "postNewClear");
        }

        public void WebSheetLoadBegin()
        {
            HdAgentProcess.Value = "false";
            try
            {
                this.ConnectSQLCA();
            }
            catch { LtServerMessage.Text = WebUtil.CompleteMessage("ติดต่อ Database ไม่ได้"); }

            try
            {
                agencyService = wcf.Agency;
                commonService = wcf.Common;
            }
            catch { LtServerMessage.Text = WebUtil.CompleteMessage("ติดต่อ Service ไม่ได้"); }

            Dw_main.SetTransaction(sqlca);
        

            if (!IsPostBack)
            {
                JspostNewClear();
            }
            else
            {
                this.RestoreContextDw(Dw_main);
            }
            
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postRefresh")
            {

            }
            else if (eventArg == "postAgentProcess")
            {
                JspostAgentProcess();
            }
            else if (eventArg == "postNewClear")
            {
                JspostNewClear();
            }
        }

        public void SaveWebSheet()
        {
        }

        public void WebSheetLoadEnd()
        {
            if (Dw_main.RowCount > 1)
            {
                Dw_main.DeleteRow(Dw_main.RowCount);
            }
            Dw_main.SaveDataCache();
        }

        #endregion
    }
}
