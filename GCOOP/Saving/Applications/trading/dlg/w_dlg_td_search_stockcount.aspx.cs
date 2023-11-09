using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Saving.Applications.trading.dlg
{
    public partial class w_dlg_td_search_stockcount : PageWebDialog, WebDialog
    {
        string pbl = "dlg_td_search_stockcout.pbl";
        private DwThDate tDwCri;
        private DwThDate tDwMain;
        protected String jsSearch;
        public void InitJsPostBack()
        {            
            tDwCri = new DwThDate(DwCri);
            tDwCri.Add("s_slip_date", "s_slip_tdate");
            tDwCri.Add("e_slip_date", "e_slip_tdate");

            tDwMain = new DwThDate(DwMain);
            tDwMain.Add("slip_date", "slip_tdate");
            jsSearch = WebUtil.JsPostBack(this, "jsSearch");
        }

        public void WebDialogLoadBegin()
        {
            if (!IsPostBack)
            {
                DwCri.InsertRow(0);
                try
                {
                    HdSlipType.Value = Request["sliptype_code"].ToString().Trim();
                    DwCri.SetItemString(1, "sliptype_code", HdSlipType.Value);
                }
                catch
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("เกิดข้อผิดพลาดทางเทคนิค กรุณากดใหม่");
                    return;
                }
            }
            else
            {
                this.RestoreContextDw(DwCri, tDwCri);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            switch (eventArg)
            {
                case "jsSearch":
                    InitData();
                    break;
            }
        }

        public void WebDialogLoadEnd()
        {
            try
            {
                DwUtil.RetrieveDDDW(DwCri, "store_id", pbl, null);
            }
            catch { } 
            tDwCri.Eng2ThaiAllRow();
            tDwMain.Eng2ThaiAllRow();
            DwCri.SaveDataCache();
            DwMain.SaveDataCache();
        }

        private void InitData()
        {
            try
            {
                DwUtil.RetrieveDataWindow(DwMain, pbl, null, null);

                DwTrans SQLCA = new DwTrans();
                SQLCA.Connect();
                DwMain.SetTransaction(SQLCA);
                String SQLBegin = DwMain.GetSqlSelect();
                string slip_no,store_id,s_slip_tdate, e_slip_tdate;
                try
                {
                    slip_no = DwCri.GetItemString(1, "slip_no");
                }
                catch { slip_no = ""; }
                try
                {
                    store_id = DwCri.GetItemString(1, "store_id");
                }
                catch { store_id = ""; }
                try
                {
                    s_slip_tdate = (DwCri.GetItemDateTime(1, "s_slip_date")).ToString("dd/MM/yyyy");
                }
                catch { s_slip_tdate = ""; }
                try
                {
                    e_slip_tdate = (DwCri.GetItemDateTime(1, "e_slip_date")).ToString("dd/MM/yyyy");
                }
                catch { e_slip_tdate = ""; }

                if (slip_no != "")
                {
                    SQLBegin = SQLBegin + " and tdstockcount.slip_no like '" + slip_no + "%'";
                }
                if (store_id != "")
                {
                    SQLBegin = SQLBegin + " and tdstockcount.store_id like '" + store_id + "%'";
                }
                if (s_slip_tdate != "" )
                {
                    SQLBegin = SQLBegin + " and trunc(tdstockcount.slip_date) >= trunc(to_date('" + s_slip_tdate + "', 'dd/mm/yyyy'))";
                }

                if (e_slip_tdate != "")
                {
                    SQLBegin = SQLBegin + " and trunc(tdstockcount.slip_date) <= trunc(to_date('" + e_slip_tdate + "', 'dd/mm/yyyy'))";
                }
                try
                {
                    String SQL = SQLBegin + " and tdstockcount.coop_id = '" + state.SsCoopId +
                            "' and tdstockcount.sliptype_code = '" + HdSlipType.Value + "'";

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


    //    public void JsSearch()
    //    {
    //        string as_slip_no, s_slip_tdate, e_slip_tdate;       

    //        try
    //        {
    //            as_slip_no = "%" + DwCri.GetItemString(1, "slip_no") + "%";
    //        }
    //        catch
    //        {
    //            as_slip_no = "%";
    //        }          

    //        try
    //        {
    //            s_slip_tdate = (DwCri.GetItemDateTime(1, "s_slip_date")).ToString("dd/MM/yyyy");
    //        }
    //        catch { s_slip_tdate = ""; }
    //        try
    //        {
    //            e_slip_tdate = (DwCri.GetItemDateTime(1, "e_slip_date")).ToString("dd/MM/yyyy");
    //        }
    //        catch { e_slip_tdate = ""; }

    //        object[] args = new object[3];
    //        args[0] = as_slip_no;
    //        args[1] = s_slip_tdate;
    //        args[2] = e_slip_tdate;

    //        DwUtil.RetrieveDataWindow(DwDetail, "dlg_td_search_stockcount.pbl", null, args);
    //    }
    //}
}
