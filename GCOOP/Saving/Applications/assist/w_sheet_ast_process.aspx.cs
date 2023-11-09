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
using DataLibrary;
using CoreSavingLibrary.WcfNCommon;
using CoreSavingLibrary.WcfNShrlon;
using Sybase.DataWindow;
using DataLibrary;
using System.Web.Services.Protocols;
using System.Globalization;

namespace Saving.Applications.assist
{
    public partial class w_sheet_ast_process : PageWebSheet, WebSheet
    {
        #region WebSheet Members

        protected String jsProcess;
        protected String clearProcess;
        protected String postSetDate;
        private n_commonClient commonService;
        private DwThDate tdwcriteria;
        public string outputProcess;

        String pbl = "kt_65years.pbl";

        public void InitJsPostBack()
        {
            jsProcess = WebUtil.JsPostBack(this, "jsProcess");
            clearProcess = WebUtil.JsPostBack(this, "clearProcess");
            postSetDate = WebUtil.JsPostBack(this, "postSetDate");
            tdwcriteria = new DwThDate(dw_criteria, this);
            tdwcriteria.Add("start_date", "start_tdate");
            tdwcriteria.Add("end_date", "end_tdate");
        }

        public void WebSheetLoadBegin()
        {
            HdRunProcess.Value = "false";
            try
            {
                
                commonService = wcf.NCommon;
            }
            catch
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ติดต่อ Web Service ไม่ได้");
                return;
            }
            this.ConnectSQLCA();

            if (IsPostBack)
            {
                try
                {
                    dw_criteria.RestoreContext();
                }
                catch { }
            }
            else
            {
                int li_year = 0;
                if (dw_criteria.RowCount < 1)
                {
                    dw_criteria.InsertRow(0);
                   // dw_criteria.SetItemDecimal(1, "acc_year", Convert.ToInt16(state.SsWorkDate.Year) + 543);
                    dw_criteria.SetItemDateTime(1, "start_date", state.SsWorkDate);
                    dw_criteria.SetItemDateTime(1, "end_date", state.SsWorkDate);
                }
              //  li_year = Convert.ToInt32(dw_criteria.GetItemDecimal(1, "acc_year"));
                string ls_sql = @"select current_accyear 
                from amappstatus 
                where coop_id = {0}
                and application = {1}";
                ls_sql = WebUtil.SQLFormat(ls_sql, state.SsCoopControl, state.SsApplication);
                Sdt dt = WebUtil.QuerySdt(ls_sql);
                if (dt.Next())
                {
                     li_year = dt.GetInt32("current_accyear");
                    
                    //    dw_criteria.SetItemDecimal(1, "acc_year", li_year);

                        ls_sql = @"select accstart_date, accend_date
                        from cmaccountyear
                        where coop_id = {0}
                        and account_year = {1}";
                        ls_sql = WebUtil.SQLFormat(ls_sql, state.SsCoopControl, li_year);
                        Sdt dt1 = WebUtil.QuerySdt(ls_sql);
                        if (dt1.Next())
                        {
                            dw_criteria.SetItemDateTime(1, "start_date", dt1.GetDate("accstart_date"));
                            dw_criteria.SetItemDateTime(1, "end_date", dt1.GetDate("accend_date"));
                        }
                   
                }
                tdwcriteria.Eng2ThaiAllRow();
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "jsProcess")
            {
                JsProcess();
            }
            else if (eventArg == "clearProcess")
            {
                JsClearProcess();
            }
           
        }

        public void SaveWebSheet()
        {
        }

        public void WebSheetLoadEnd()
        {
            if (dw_criteria.RowCount > 1)
            {
                dw_criteria.DeleteRow(dw_criteria.RowCount);
            }
            tdwcriteria.Eng2ThaiAllRow();
           // DwUtil.RetrieveDDDW(dw_criteria, "assistype_code", pbl, null);
        }

        private void JsProcess()
        {
            //try
            //{
            //    String entry_id = state.SsUsername;
            //    String ls_xmldwcriteria = dw_criteria.Describe("DataWindow.Data.XML");
            //    shrlonService.RunLnShotLongProcess(state.SsWsPass, ls_xmldwcriteria, entry_id, state.SsApplication, state.CurrentPage);//RunLnShotLongProcess
            //    HdRunProcess.Value = "true";
            //}
            //catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
            try
            {
                string ls_assistype = dw_criteria.GetItemString(1, "assistype_code");
                string option_xml = dw_criteria.Describe("DataWindow.Data.XML");
                if (ls_assistype == "22")
                {
                    outputProcess = WebUtil.runProcessing(state, "AST_PROCESS22", option_xml, "", "");
                }
                else if (ls_assistype == "60") {

                    outputProcess = WebUtil.runProcessing(state, "AST_GNS60", option_xml, "", "");
                }
                else if (ls_assistype == "65")
                {
                    outputProcess = WebUtil.runProcessing(state, "AST_GNS", option_xml, "", "");

                }
            }
            catch (SoapException ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(WebUtil.SoapMessage(ex));
            }
        }

        private void JsClearProcess()
        {
            dw_criteria.Reset();
            dw_criteria.InsertRow(0);
            string ls_sql = @"select current_accyear 
                from amappstatus 
                where coop_id = {0}
                and application = {1}";
            ls_sql = WebUtil.SQLFormat(ls_sql, state.SsCoopControl, state.SsApplication);
            Sdt dt = WebUtil.QuerySdt(ls_sql);
            if (dt.Next())
            {
               int li_year = dt.GetInt32("current_accyear");

                //    dw_criteria.SetItemDecimal(1, "acc_year", li_year);

                ls_sql = @"select accstart_date, accend_date
                        from cmaccountyear
                        where coop_id = {0}
                        and account_year = {1}";
                ls_sql = WebUtil.SQLFormat(ls_sql, state.SsCoopControl, li_year);
                Sdt dt1 = WebUtil.QuerySdt(ls_sql);
                if (dt1.Next())
                {
                    dw_criteria.SetItemDateTime(1, "start_date", dt1.GetDate("accstart_date"));
                    dw_criteria.SetItemDateTime(1, "end_date", dt1.GetDate("accend_date"));
                }

            }
            tdwcriteria.Eng2ThaiAllRow();
        }

        #endregion
    }
}
