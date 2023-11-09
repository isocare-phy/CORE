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
using CoreSavingLibrary.WcfNBudget;
using DataLibrary;
using Sybase.DataWindow;

namespace Saving.Applications.account
{
    public partial class w_sheet_bg_budgettype : PageWebSheet, WebSheet
    {
        protected String InsertRow;
        protected String DeleteRow;
        protected String SetFormat;
        Sta ta;
        protected String RetrieveList;

        private n_budgetClient serBudget;
        #region WebSheet Members

        public void InitJsPostBack()
        {
            InsertRow = WebUtil.JsPostBack(this, "InsertRow");
            DeleteRow = WebUtil.JsPostBack(this, "DeleteRow");
            SetFormat = WebUtil.JsPostBack(this, "SetFormat");
            RetrieveList = WebUtil.JsPostBack(this, "RetrieveList");
        }

        public void WebSheetLoadBegin()
        {
            ta = new Sta(state.SsConnectionString);
            serBudget = wcf.NBudget;
            if (!IsPostBack)
            {
                DwMain.InsertRow(0);
                DwUtil.RetrieveDDDW(DwMain, "bggrp_code", "budget.pbl", null);
            }
            else
            {
                this.RestoreContextDw(DwMain);
                this.RestoreContextDw(DwList);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            String bgGrpCode = DwMain.GetItemString(1, "bggrp_code");

            if (eventArg == "InsertRow")
            {
                DwList.InsertRow(0);
                DwList.SetItemString(DwList.RowCount, "budgetgroup_code", bgGrpCode);
                DwUtil.RetrieveDDDW(DwList, "account_id", "budget.pbl", null);
            }
            else if (eventArg == "DeleteRow")
            {
                //int row = int.Parse(HdListRow.Value);
                //DwList.DeleteRow(row);
                Delete_Row();
            }
            else if (eventArg == "RetrieveList")
            {
                object[] argMain = new object[1] { bgGrpCode};
                try
                {
                    DwList.Reset();
                    DwUtil.RetrieveDataWindow(DwList, "budget.pbl", null, argMain);
                    DwUtil.RetrieveDDDW(DwList, "account_id", "budget.pbl", null);
                }
                catch (Exception)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่มีข้อมูลประเภทงบประมาณ หมวด : " + bgGrpCode);
                }
            }
            else if (eventArg == "SetFormat")
            {
                int row = int.Parse(HdListRow.Value);
                String bgGrpType = "000" + DwList.GetItemString(row, "budgettype_code");
                bgGrpType = WebUtil.Right(bgGrpType, 3);
                DwList.SetItemString(row, "budgettype_code", bgGrpType);
                DwList.SelectRow(row, true);
            }
        }

        public void SaveWebSheet()
        {
            String xmlList = "";
            try
            {
                xmlList = DwList.Describe("DataWindow.Data.XML");
                serBudget.of_save_budget_type(state.SsWsPass, xmlList);
                DwList.Reset();
                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลเรียบร้อยแล้ว...");
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

        private void Delete_Row()
        {
            //int row = int.Parse(HdMainRow.Value);
            string group_code = Hdgroup.Value;
            string type_code = Hdtype.Value;
            try
            {
                String sql = @"DELETE FROM budgettype WHERE budgetgroup_code ='" + group_code + @"' and budgettype_code ='" + type_code + @"'";
                ta.Exe(sql);
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
            }
            //object[] arg = new object[1];
            //arg[0] = group_code;
            String bgGrpCode = DwMain.GetItemString(1, "bggrp_code");
            object[] argMain = new object[1] { bgGrpCode };
            try
            {
                DwList.Reset();
                DwUtil.RetrieveDataWindow(DwList, "budget.pbl", null, argMain);
                DwUtil.RetrieveDDDW(DwList, "account_id", "budget.pbl", null);
            }
            catch (Exception)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่มีข้อมูลประเภทงบประมาณ หมวด : " + bgGrpCode);
            }
            //DwList.Reset();
            //DwUtil.RetrieveDataWindow(DwList, "budget.pbl", null, arg);

        }
    }
}
