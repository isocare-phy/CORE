using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataLibrary;
using CoreSavingLibrary;

namespace Saving.Applications.pm
{
    public partial class w_sheet_invlinkaccount : PageWebSheet, WebSheet
    {
        protected String jsPostInsertRow;
        protected String jsPostDelRow;

        string pbl = "pm_investment.pbl";

        #region websheet member
        public void InitJsPostBack()
        {
            jsPostInsertRow = WebUtil.JsPostBack(this, "jsPostInsertRow");
            jsPostDelRow = WebUtil.JsPostBack(this, "jsPostDelRow");
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                DwUtil.RetrieveDataWindow(DwMain, pbl, null, null);
            }
            else
            {
                this.RestoreContextDw(DwMain);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            switch (eventArg)
            {
                case "jsPostInsertRow":
                    InsertRow();
                    break;

                case "jsPostDelRow":
                    DelRow();
                    break;
            }
        }

        public void SaveWebSheet()
        {
            if (CheckRepetitive())
            {
                string sqldel = "delete from pminvlinkaccount";
                Sdt del = WebUtil.QuerySdt(sqldel);
                try
                {
                    DwUtil.InsertDataWindow(DwMain, pbl, "PMINVLINKACCOUNT");
                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
                }
                catch
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("บันทึกไม่สำเร็จ");
                }
                //for (int i = 1; i <= DwMain.RowCount; i++)
                //{
                //    int[] row = { i };
                //    try
                //    {
                //        DwUtil.InsertDataWindow(DwMain, pbl, "PMINVLINKACCOUNT", row);
                //    }
                //    catch
                //    {
                //        DwUtil.UpdateDateWindow(DwMain, pbl, "PMINVLINKACCOUNT", row);
                //    }
                   
                //    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
                //}
            }
            else
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("มีรายการซ้ำ กรุณาตรวจสอบ");
            }
        }

        public void WebSheetLoadEnd()
        {
            try
            {
                DwUtil.RetrieveDDDW(DwMain, "invsource_code", pbl, null);
            }
            catch { }
            try
            {
                DwUtil.RetrieveDDDW(DwMain, "investtype_code", pbl, null);
            }
            catch { }
            try
            {
                DwUtil.RetrieveDDDW(DwMain, "prnc_account_id", pbl, null);
            }
            catch { }
            try
            {
                DwUtil.RetrieveDDDW(DwMain, "int_account_id", pbl, null);
            }
            catch { }
            try
            {
                DwUtil.RetrieveDDDW(DwMain, "tax_account_id", pbl, null);
            }
            catch { }

            DwMain.SaveDataCache();
        }
        #endregion

        #region function
        private void InsertRow()
        {
            int row = DwMain.InsertRow(0);
            DwMain.SetItemString(row, "coop_id", state.SsCoopId);
            DwMain.ScrollLastPage();
        }

        private void DelRow()
        {

            int row = Convert.ToInt32(Hdrow.Value);
            try
            {
                String invsource_code = DwMain.GetItemString(row, "invsource_code");
                String investtype_code = DwMain.GetItemString(row, "investtype_code");
                String ACCOUNT_NO = DwMain.GetItemString(row, "account_no");

                String sql = "DELETE PMINVLINKACCOUNT WHERE invsource_code ='" + invsource_code
                    + "'AND investtype_code ='" + investtype_code + "' AND account_no = '" + ACCOUNT_NO + "'";

                try
                {
                    Sdt delete = WebUtil.QuerySdt(sql);
                    DwMain.DeleteRow(row);
                    LtServerMessage.Text = WebUtil.CompleteMessage("ลบแถวสำเร็จ");
                }
                catch
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ลบแถวไม่สำเร็จ");
                }

            }
            catch
            {
                DwMain.DeleteRow(row);
                LtServerMessage.Text = WebUtil.CompleteMessage("ลบแถวสำเร็จ");
            }
        }

        private bool CheckRepetitive()
        {
            int find_row = 0;
            string invsource_code = "";
            string investtype_code = "";
            string account_no = "";


            for (int i = 1; i < DwMain.RowCount; i++)
            {
                find_row = 0;
                invsource_code = DwMain.GetItemString(i, "invsource_code");
                investtype_code = DwMain.GetItemString(i, "investtype_code");
                account_no = DwMain.GetItemString(i, "account_no");
                find_row = DwMain.FindRow("invsource_code='" + invsource_code + "' and investtype_code= '" + investtype_code + "' and account_no = '" + account_no + "'", i + 1, DwMain.RowCount);
                if (find_row > 0)
                {
                    return false;
                }
            }
            return true;
        }

        #endregion

    }
}