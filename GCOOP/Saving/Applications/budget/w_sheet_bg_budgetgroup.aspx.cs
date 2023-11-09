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
using DataLibrary;
using Sybase.DataWindow;

namespace Saving.Applications.budget
{
    public partial class w_sheet_bg_budgetgroup : PageWebSheet,WebSheet
    {
        protected String InsertRow;
        protected String DeleteRow;
        Sta ta;
        protected String SetFormat;

        private BudgetClient serBudget;


        #region WebSheet Members

        public void InitJsPostBack()
        {
            InsertRow = WebUtil.JsPostBack(this, "InsertRow");
            DeleteRow = WebUtil.JsPostBack(this, "DeleteRow");
            SetFormat = WebUtil.JsPostBack(this, "SetFormat");
        }

        public void WebSheetLoadBegin()
        {
            ta = new Sta(state.SsConnectionString);
            serBudget = wcf.Budget;
            if (!IsPostBack)
            {
                DwUtil.RetrieveDataWindow(DwMain, "budget.pbl", null, null);
            }
            else
            {
                this.RestoreContextDw(DwMain);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "InsertRow")
            {
                DwMain.InsertRow(0);
            }
            else if (eventArg == "DeleteRow")
            {
                //int row = int.Parse(HdMainRow.Value);
                //DwMain.DeleteRow(row);
                Delete_Row();
            }
            else if (eventArg == "SetFormat")
            {
                int row = int.Parse(HdMainRow.Value);
                String bgGrpCode = "00" + DwMain.GetItemString(row, "budgetgroup_code");
                bgGrpCode = WebUtil.Right(bgGrpCode, 2);
                DwMain.SetItemString(row, "budgetgroup_code", bgGrpCode);
                DwMain.SelectRow(row, true);
            }
        }

        public void SaveWebSheet()
        {
            String xmlMain = "";
            try
            {
                xmlMain = DwMain.Describe("DataWindow.Data.XML");
                serBudget.SaveBudgetGroup(state.SsWsPass, xmlMain);
                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลเรียบร้อยแล้ว...");
                DwMain.Reset();
                DwUtil.RetrieveDataWindow(DwMain, "budget.pbl", null, null);
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        public void WebSheetLoadEnd()
        {
            DwMain.SaveDataCache();
        }

        #endregion

        private void Delete_Row()
        {
            //int row = int.Parse(HdMainRow.Value);
            string code = Hdcode.Value;
            try
            {
                String sql = @"DELETE FROM budgetgroup WHERE budgetgroup_code ='" + code + @"'";
                ta.Exe(sql);
                DwMain.Reset();
                DwUtil.RetrieveDataWindow(DwMain, "budget.pbl", null, null);
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
            }
        }
    }
}
