using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Saving.Applications.trading.dlg
{
    public partial class w_dlg_td_search_slip_apdebt : PageWebDialog, WebDialog
    {
        private DwThDate tDwCri;
        private DwThDate tDwMain;
        protected String jsSearchData;
        public void InitJsPostBack()
        {
            tDwCri = new DwThDate(DwCri);
            tDwCri.Add("creditdoc_date", "creditdoc_tdate");

            tDwMain = new DwThDate(DwMain);
            tDwMain.Add("creditdoc_date", "creditdoc_tdate");

            jsSearchData = WebUtil.JsPostBack(this, "jsSearchData");
        }

        public void WebDialogLoadBegin()
        {
            try
            {
                HdCredittype_code.Value = Request["credittype_code"].ToString().Trim();
            }
            catch
            {
                HdCredittype_code.Value = "";
            }
            try
            {
                HdDebtNo.Value = Request["debt_no"].ToString().Trim();
            }
            catch
            {
                HdDebtNo.Value = "";
            }

            if (!IsPostBack)
            {
                DwCri.InsertRow(0);
            }
            else
            {
                this.RestoreContextDw(DwCri, tDwCri);
                this.RestoreContextDw(DwMain, tDwMain);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            switch (eventArg)
            {
                case "jsSearchData":
                    InitData();
                    break;
            }
        }

        public void WebDialogLoadEnd()
        {
            tDwCri.Eng2ThaiAllRow();
            tDwMain.Eng2ThaiAllRow();

            DwCri.SaveDataCache();
            DwMain.SaveDataCache();
        }

        private void InitData()
        {
            try
            {
                DwUtil.RetrieveDataWindow(DwMain, "dlg_td_search_slip_apdebt.pbl", null, null);

                DwTrans SQLCA = new DwTrans();
                SQLCA.Connect();
                DwMain.SetTransaction(SQLCA);
                String SQLBegin = DwMain.GetSqlSelect();
                string creditdoc_no, debt_no, debt_name, creditdoc_tdate;
                try
                {
                    creditdoc_no = DwCri.GetItemString(1, "creditdoc_no");
                }
                catch { creditdoc_no = ""; }
                try
                {
                    debt_no = DwCri.GetItemString(1, "debt_no");
                }
                catch { debt_no = ""; }
                try
                {
                    debt_name = DwCri.GetItemString(1, "debt_name");
                }
                catch { debt_name = ""; }
                try
                {
                    creditdoc_tdate = (DwCri.GetItemDateTime(1, "creditdoc_date")).ToString("dd/MM/yyyy");
                }
                catch { creditdoc_tdate = ""; }


                if (creditdoc_no != "")
                {
                    SQLBegin = SQLBegin + " and tddebtcredit.creditdoc_no like '" + creditdoc_no + "%'";
                }
                if (debt_no != "")
                {
                    SQLBegin = SQLBegin + " and tddebtcredit.debt_no like '" + debt_no + "%'";
                }
                if (debt_name != "")
                {
                    SQLBegin = SQLBegin + " and tddebtmaster.debt_name like '" + debt_name + "%'";
                }
                if (creditdoc_tdate != "")
                {
                    SQLBegin = SQLBegin + " and trunc(tddebtcredit.creditdoc_date) >= trunc(to_date('" + creditdoc_tdate + "', 'dd/mm/yyyy'))";
                }
                try
                {
                    SQLBegin += " and tddebtcredit.coop_id = '" + state.SsCoopId + "'";
                    SQLBegin += " and tddebtcredit.debt_status = 8";
                    if ((HdCredittype_code.Value).Trim() != "")
                    {
                        SQLBegin += " and tddebtcredit.credittype_code = '" + HdCredittype_code.Value + "'";
                    }
                    if ((HdDebtNo.Value).Trim() != "")
                    {
                        SQLBegin += " and tddebtcredit.debt_no = '" + HdDebtNo.Value + "'";
                    }

                    SQLBegin += " order by tddebtcredit.creditdoc_no";

                    DwMain.SetSqlSelect(SQLBegin);
                    DwMain.Retrieve();
                    SQLCA.Disconnect();
                }
                catch
                {
                    SQLCA.Disconnect();
                    LtServerMessage.Text = WebUtil.ErrorMessage("แก้ข้อผิดพลาด ไม่สามารถค้นหาข้อมูลได้");
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }
    }
}