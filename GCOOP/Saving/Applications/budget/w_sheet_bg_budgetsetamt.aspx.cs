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
using CoreSavingLibrary.WcfBudget;
using Sybase.DataWindow;

namespace Saving.Applications.budget
{
    public partial class w_sheet_bg_budgetsetamt : PageWebSheet,WebSheet
    {
        protected String RetrieveList;
        protected String Refresh;

        private BudgetClient serBudget;

        #region WebSheet Members

        public void InitJsPostBack()
        {
            RetrieveList = WebUtil.JsPostBack(this, "RetrieveList");
            Refresh = WebUtil.JsPostBack(this, "Refresh");
        }

        public void WebSheetLoadBegin()
        {
            //this.ConnectSQLCA();
            //DwList.SetTransaction(sqlca);
            serBudget = wcf.Budget;
            if (!IsPostBack)
            {
                DwMain.InsertRow(0);
            }
            else
            {
                this.RestoreContextDw(DwMain);
                this.RestoreContextDw(DwList);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "RetrieveList")
            {
                retrieve();
            }
            if (eventArg == "Refresh")
            {
                refresh();
            }
        }

        private void refresh()
        {
            
        }

        public void SaveWebSheet()
        {
            String xmlList = "";
            Decimal bgYear = DwMain.GetItemDecimal(1, "budgetyear") - 543;
            try
            {
                short year = short.Parse(bgYear.ToString());
                xmlList = DwList.Describe("DataWindow.Data.XML");
                string x = xmlList.Replace("<d_bg_buggetsetamt_list_new_group1>", "");
                xmlList = x;
                x = xmlList.Replace("</d_bg_buggetsetamt_list_new_group1>", "");
                xmlList = x;
                serBudget.SaveBudgetAmount(state.SsWsPass, year, xmlList);
                DwList.Reset();
                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลเรียบร้อยแล้ว...");

                retrieve();
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        public void WebSheetLoadEnd()
        {
            DwMain.SaveDataCache();
            DwList.SaveDataCache();
        }

        #endregion

        public void retrieve()
        {
            Decimal bgYear = DwMain.GetItemDecimal(1, "budgetyear") - 543;
            String xmlDwList = "";
            try
            {
                short year = short.Parse(bgYear.ToString());
                xmlDwList = serBudget.GetSetBudgetAmount(state.SsWsPass, year);
                DwUtil.ImportData(xmlDwList, DwList, null, FileSaveAsType.Xml);
                DwList.Sort();
                DwList.CalculateGroups();
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }
    }
}
