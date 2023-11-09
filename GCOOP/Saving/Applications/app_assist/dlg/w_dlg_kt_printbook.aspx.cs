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
using CoreSavingLibrary.WcfNDeposit;
using System.Web.Services.Protocols;
namespace Saving.Applications.app_assist.dlg
{
    public partial class w_dlg_kt_printbook : PageWebDialog, WebDialog
    {
        private n_depositClient dep;
        private String deptAccountNo;
        protected String postPrintBook;

        #region WebDialog Members

        public void InitJsPostBack()
        {
            postPrintBook = WebUtil.JsPostBack(this, "postPrintBook");
        }

        public void WebDialogLoadBegin()
        {
            dep = wcf.NDeposit;
            this.ConnectSQLCA();
            DwNewBook.SetTransaction(sqlca);
            if (!IsPostBack)
            {
                HdIsPostBack.Value = "false";
                try
                {
                    HdAccountNo.Value = Request["deptAccountNo"];
                    deptAccountNo = HdAccountNo.Value;
                    String xmlInitDwNewBook = dep.of_init_printbook(state.SsWsPass, deptAccountNo, state.SsCoopId);
                    DwPrintPrompt.ImportString(xmlInitDwNewBook, Sybase.DataWindow.FileSaveAsType.Xml);
                }
                catch (SoapException ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(WebUtil.SoapMessage(ex));
                    DwPrintPrompt.InsertRow(0);
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
                    DwPrintPrompt.InsertRow(0);
                }
            }
            else
            {
                HdIsPostBack.Value = "true";
                this.RestoreContextDw(DwPrintPrompt);
            }
            this.deptAccountNo = HdAccountNo.Value;
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postPrintBook")
            {
                try
                {
                    Int16 seq = Convert.ToInt16(DwPrintPrompt.GetItemDecimal(1, "lastrec_no_pb"));
                    Int16 page = Convert.ToInt16(DwPrintPrompt.GetItemDecimal(1, "lastpage_no_pb"));
                    Int16 line = Convert.ToInt16(DwPrintPrompt.GetItemDecimal(1, "lastline_no_pb"));
                    Int16 ai_status = 0;
                    string as_xml_return = "";
                    String returnValue = "";//dep.PrintBook(state.SsWsPass, deptAccountNo, state.SsCoopId, seq, page, line, false, state.SsPrinterSet, ai_status,ref as_xml_return);
                    String[] re = returnValue.Split('@');
                    int rePage = int.Parse(re[0]);
                    int reReq = int.Parse(re[1]);

                    HdIsZeroPage.Value = rePage == 0 ? "true" : "false";

                    DwPrintPrompt.SetItemDecimal(1, "lastpage_no_pb", rePage);
                    DwPrintPrompt.SetItemDecimal(1, "lastrec_no_pb", reReq);
                    DwPrintPrompt.SetItemDecimal(1, "lastline_no_pb", 1);
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                }
            }
        }

        public void WebDialogLoadEnd()
        {
            DwPrintPrompt.SaveDataCache();
        }

        #endregion
    }
}