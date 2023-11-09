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
    public partial class w_sheet_bg_reprintreciept : PageWebSheet,WebSheet
    {
        private DwThDate tDwMain;
        private BudgetClient sevBudget;
        protected String InitList;
        protected String PrintReceipt;

      

        public void InitJsPostBack()
        {
            InitList = WebUtil.JsPostBack(this, "InitList");
            PrintReceipt = WebUtil.JsPostBack(this, "PrintReceipt");
            tDwMain = new DwThDate(DwMain, this);
            tDwMain.Add("operate_date", "operate_tdate");
        }

        public void WebSheetLoadBegin()
        {
             sevBudget = wcf.Budget;
             if (!IsPostBack)
             {
                 DwMain.InsertRow(0);
                 DwMain.SetItemDateTime(1, "operate_date", state.SsWorkDate);
                 tDwMain.Eng2ThaiAllRow();
             }
             else
             {
                 this.RestoreContextDw(DwMain);
                 this.RestoreContextDw(DwList);
             }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if(eventArg == "InitList"){
                String member_no = DwMain.GetItemString(1, "member_no");
                String payout_slipno = DwMain.GetItemString(1, "payout_slipno");
                DateTime operate_date = DwMain.GetItemDateTime(1, "operate_date");

                String xmlMain = "";
                try
                {
                    String xml = "";   //sevBudget.print(state.SsWsPass, operate_date, payout_slipno, member_no);
                 DwList.ImportString(xml, FileSaveAsType.Xml);
                  //  xmlMain = DwMain.Describe("DataWindow.Data.XML");
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                }
            }
            else if (eventArg == "PrintReceipt")
            {
                String xmlList = "";
                try
                {
                    xmlList = DwList.Describe("DataWindow.Data.XML");
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                }
            }
        }

        public void SaveWebSheet()
        {
            
        }

        public void WebSheetLoadEnd()
        {
            DwMain.SaveDataCache();
            DwList.SaveDataCache();
        }

      
    }
}
