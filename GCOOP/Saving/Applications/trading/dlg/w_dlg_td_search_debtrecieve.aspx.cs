using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Saving.Applications.trading.dlg
{
    public partial class w_dlg_td_search_debtrecieve : PageWebDialog, WebDialog
    {
        private DwThDate tDwCri;
        private DwThDate tDwMain;
        protected String jsSearchData;
        public void InitJsPostBack()
        {
            tDwCri = new DwThDate(DwCri);
            tDwCri.Add("debtintdoc_date", "debtintdoc_tdate");

            tDwMain = new DwThDate(DwMain);
            tDwMain.Add("slip_date", "slip_tdate");
            tDwMain.Add("debtintdoc_date", "debtintdoc_tdate");

            jsSearchData = WebUtil.JsPostBack(this, "jsSearchData");
        }

        public void WebDialogLoadBegin()
        {
            try
            {
                HdSlipType.Value = Request["debtdectype_code"].ToString().Trim();
            }
            catch
            {
                HdSlipType.Value = "";
                //LtServerMessage.Text = WebUtil.ErrorMessage("เกิดข้อผิดพลาดทางเทคนิค กรุณากดใหม่");
                //return;
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
                DwUtil.RetrieveDataWindow(DwMain, "dlg_td_search_recieve.pbl", null, null);

                DwTrans SQLCA = new DwTrans();
                SQLCA.Connect();
                DwMain.SetTransaction(SQLCA);
                String SQLBegin = DwMain.GetSqlSelect();

                string debtdecdoc_no, refdoc_no, debt_no, debtintdoc_tdate;//, cred_name;
                try
                {
                    debtdecdoc_no = DwCri.GetItemString(1, "debtdecdoc_no");
                }
                catch { debtdecdoc_no = ""; }
                try
                {
                    refdoc_no = DwCri.GetItemString(1, "refdoc_no");
                }
                catch { refdoc_no = ""; }
                try
                {
                    debt_no = DwCri.GetItemString(1, "debt_no");
                }
                catch { debt_no = ""; }
                try
                {
                    debtintdoc_tdate = (DwCri.GetItemDateTime(1, "debtintdoc_tdate")).ToString("dd/MM/yyyy");
                }
                catch { debtintdoc_tdate = ""; }
                //try
                //{
                //    cred_name = DwCri.GetItemString(1, "cred_name");
                //}
                //catch { cred_name = ""; }

                if (debtdecdoc_no != "")
                {
                    SQLBegin = SQLBegin + " and tddebtdec.debtdecdoc_no like '" + debtdecdoc_no + "%'";
                }
                if (refdoc_no != "")
                {
                    SQLBegin = SQLBegin + " and tddebtdec.refdoc_no like '" + refdoc_no + "%'";
                }
                if (debt_no != "")
                {
                    SQLBegin = SQLBegin + " and tddebtdec.debt_no like '" + debt_no + "%'";
                }
                if (debt_no != "")
                {
                    SQLBegin = SQLBegin + " and tddebtdec.debt_no like '" + debt_no + "%'";
                }
                if (debtintdoc_tdate != "")
                {
                    SQLBegin = SQLBegin + " and trunc(tddebtdec.debtintdoc_date) >= trunc(to_date('" + debtintdoc_tdate + "', 'dd/mm/yyyy'))";
                }
                //if (cred_name != "")
                //{
                //    SQLBegin = SQLBegin + " and tdcredmaster.cred_name like '" + cred_name + "%'";
                //}

                try
                {
                    String SQL = SQLBegin + " and tddebtdec.coop_id = '" + state.SsCoopId + "'";

                    if ((HdSlipType.Value).Trim() != "")
                    {
                        SQL = SQLBegin + " and tddebtdec.debtdectype_code = '" + HdSlipType.Value + "'";
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