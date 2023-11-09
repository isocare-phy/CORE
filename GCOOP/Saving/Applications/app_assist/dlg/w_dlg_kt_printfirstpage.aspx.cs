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
using DataLibrary;


namespace Saving.Applications.app_assist.dlg
{
    public partial class w_dlg_kt_printfirstpage : PageWebDialog, WebDialog
    {
        protected String printFirstPage;
        #region WebDialog Members

        public void InitJsPostBack()
        {
            printFirstPage = WebUtil.JsPostBack(this, "printFirstPage");
        }

        public void WebDialogLoadBegin()
        {
            HdCloseIFrame.Value = "false";
            if (!IsPostBack)
            {
                HdDeptAccountNo.Value = Request["deptAccountNo"].Trim();
                HdPassBookNo.Value = Request["deptPassBookNo"].Trim();
            }
            else { }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "printFirstPage")
            {
                DepositClient dep = wcf.Deposit;
                String apvId = state.SsUsername;
                int normFlag = 1;
                try
                {
                    //dep.PrintBookFirstPage(state.SsWsPass, state.SsApplication, HdDeptAccountNo.Value, state.SsCoopId, state.SsUsername, HdPassBookNo.Value, "00", apvId, state.SsPrinterSet, normFlag, state.SsWorkDate, 1);
                    //XmlConfigService xml = new XmlConfigService();
                    int printStatus = 1;// xml.DepositPrintMode;
                    string xml_return = "", xml_return_bf = "";
                    dep.PrintBookFirstPage(state.SsWsPass, state.SsApplication, HdDeptAccountNo.Value, state.SsCoopId, state.SsUsername, HdPassBookNo.Value, "00", apvId, state.SsPrinterSet, normFlag, state.SsWorkDate, 1, printStatus, ref xml_return, ref xml_return_bf);
                    if (xml_return != "" && printStatus == 1)
                    {
                        //if (printStatus == 1)
                        //{
                        //    Printing.Print(this, "Slip/ap_deposit/PrintBookFirstPage.aspx", xml_return, 1);
                        //}
                        //else
                        //{
                        Printing.DeptPrintBookFirstPage(this, xml_return);
                        //}
                    }
                    HdCloseIFrame.Value = "true";
                }
                catch (Exception ex)
                {
                    Label1.Text = WebUtil.ErrorMessage(ex);
                }
            }
        }

        public void WebDialogLoadEnd()
        {
        }

        #endregion

    }
}