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
    public partial class w_dlg_td_search_value : PageWebDialog, WebDialog
    {
        protected String jsSearch;

        public void InitJsPostBack()
        {
            jsSearch = WebUtil.JsPostBack(this, "jsSearch");
        }

        public void WebDialogLoadBegin()
        {
            try
            {
                HdSlipType.Value = Request["sliptype_code"].ToString().Trim();                
            }
            catch
            {
                HdSlipType.Value = "";
                //LtServerMessage.Text = WebUtil.ErrorMessage("เกิดข้อผิดพลาดทางเทคนิค กรุณากดใหม่");
                //return;
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
                    //String SQL = SQLBegin + " and tdstockslip.branch_id = '" + state.SsCoopId + "'";
                    SQLBegin = SQLBegin + " and tdstockslip.coop_id = '" + state.SsCoopId + "'";
                    

                    if ((HdSlipType.Value).Trim() != "")
                    {
                        //SQL = SQLBegin + " and tdstockslip.sliptype_code = '" + HdSlipType.Value + "'";
                        SQLBegin = SQLBegin + " and tdstockslip.sliptype_code = '" + HdSlipType.Value + "'";
                    }


                    if ((HdDebtNo.Value).Trim() != "" && HdDebtNo.Value != "null")
                    {
                        if (HdSlipType.Value == "PO" || HdSlipType.Value == "RC")
                        {
                            //SQL = SQLBegin + " and tdstockslip.cred_no = '" + HdDebtNo.Value + "'";
                            SQLBegin = SQLBegin + " and tdstockslip.cred_no = '" + HdDebtNo.Value + "'";
                        }
                        else
                        {
                            //SQL = SQLBegin + " and tdstockslip.debt_no = '" + HdDebtNo.Value + "'";
                            SQLBegin = SQLBegin + " and tdstockslip.debt_no = '" + HdDebtNo.Value + "'";
                        }
                    }

                    DwDetail.SetSqlSelect(SQLBegin);
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
    }
}
