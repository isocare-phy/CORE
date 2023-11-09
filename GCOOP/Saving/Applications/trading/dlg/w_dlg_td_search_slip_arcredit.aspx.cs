using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Saving.Applications.trading.dlg
{
    public partial class w_dlg_td_search_slip_arcredit : PageWebDialog, WebDialog
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
                HdCredNo.Value = Request["debt_no"].ToString().Trim();
            }
            catch
            {
                HdCredNo.Value = "";
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
                DwUtil.RetrieveDataWindow(DwMain, "dlg_td_search_slip_apcredit.pbl", null, null);

                DwTrans SQLCA = new DwTrans();
                SQLCA.Connect();
                DwMain.SetTransaction(SQLCA);
                String SQLBegin = DwMain.GetSqlSelect();
                string creditdoc_no, debt_no, cred_name, creditdoc_tdate;
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
                    cred_name = DwCri.GetItemString(1, "cred_name");
                }
                catch { cred_name = ""; }
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
                if (cred_name != "")
                {
                    SQLBegin = SQLBegin + " and tddebtmaster.debt_name like '" + cred_name + "%'";   
                }
                if (creditdoc_tdate != "")
                {
                    SQLBegin = SQLBegin + " and trunc(tddebtcredit.creditdoc_date) >= trunc(to_date('" + creditdoc_tdate + "', 'dd/mm/yyyy'))";
                }
                try
                {
                    String SQL = SQLBegin + " and tddebtcredit.coop_id = '" + state.SsCoopId + "'";
                    if ((HdCredNo.Value).Trim() != "")
                    {
                        SQL = SQL + " and tddebtmaster.debt_no = '" + HdCredNo.Value + "'";
                    }

                    DwMain.SetSqlSelect(SQL);
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