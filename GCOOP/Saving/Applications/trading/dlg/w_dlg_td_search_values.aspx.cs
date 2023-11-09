using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Saving;
using DataLibrary;
using Sybase.DataWindow;


namespace Saving.Applications.trading.dlg
{
    public partial class w_dlg_td_search_values : PageWebDialog, WebDialog
    {
        protected String jsSearch;
        protected String jsSend;

        public void InitJsPostBack()
        {
            jsSearch = WebUtil.JsPostBack(this, "jsSearch");
            jsSend = WebUtil.JsPostBack(this, "jsSend");
        }

        public void WebDialogLoadBegin()
        {
            HdEnd.Value = "false";
            try
            {
                HdSlipType.Value = Request["sliptype_code"].ToString().Trim();
                HdDebtNo.Value = Request["debt_no"].ToString().Trim();
            }
            catch
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("เกิดข้อผิดพลาดทางเทคนิค กรุณากดใหม่");
                return;
            }  
            if (!IsPostBack)
            {
                DwCri.InsertRow(0);
            }
            else
            {
                this.RestoreContextDw(DwCri);
                this.RestoreContextDw(DwDetail);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "jsSearch")
            {
                JsSearch();
            }
            else if (eventArg == "jsSend")
            {
                JsSend();
            }
        }

        public void WebDialogLoadEnd()
        {
            DwCri.SaveDataCache();
            DwDetail.SaveDataCache();
        }

        public void JsSearch()
        {
            try
            {
                DwUtil.RetrieveDataWindow(DwDetail, "dlg_td_search_slip.pbl", null, null);

                DwTrans SQLCA = new DwTrans();
                SQLCA.Connect();
                DwDetail.SetTransaction(SQLCA);
                String SQLBegin = DwDetail.GetSqlSelect();
                string slip_no, startslip_date, endslip_date;
                try
                {
                    slip_no = DwCri.GetItemString(1, "slip_no");
                }
                catch { slip_no = ""; }
                try
                {
                    startslip_date = (DwCri.GetItemDateTime(1, "startslip_date")).ToString("dd/MM/yyyy");
                }
                catch { startslip_date = ""; }
                try
                {
                    endslip_date = (DwCri.GetItemDateTime(1, "endslip_date")).ToString("dd/MM/yyyy");
                }
                catch { endslip_date = ""; }

                if (slip_no != "")
                {
                    SQLBegin = SQLBegin + " and tdstockslip.slip_no like '" + slip_no + "%'";
                }
                if (startslip_date != "")
                {
                    SQLBegin = SQLBegin + " and trunc(tdstockslip.slip_date) >= trunc(to_date('" + startslip_date + "', 'dd/mm/yyyy'))";
                }

                if (endslip_date != "")
                {
                    SQLBegin = SQLBegin + " and trunc(tdstockslip.slip_date) <= trunc(to_date('" + endslip_date + "', 'dd/mm/yyyy'))";
                }
                try
                {
                    String SQL = SQLBegin + " and tdstockslip.coop_id = '" + state.SsCoopId + "' and tdstockslip.sliptype_code = '" + HdSlipType.Value + "'" +
                        " and tdstockslip.debt_no = '" + HdDebtNo.Value + "'" ;

                    DwDetail.SetSqlSelect(SQL);
                    DwDetail.Retrieve();
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

        private void JsSend()
        {
            try
            {
                string select = "", product_no = "";
                for (int i = 1; i <= DwDetail.RowCount; i++)
                {
                    if (DwDetail.GetItemString(i, "select_flag") == "1")
                    {
                        select = select + DwDetail.GetItemString(i, "slip_no") + ",";
                        product_no = product_no + DwDetail.GetItemString(i, "product_no") + ",";
                    }
                }
                HdResult.Value = select;
                HdProductCode.Value = product_no;
                HdEnd.Value = "true";
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }
    }
}
