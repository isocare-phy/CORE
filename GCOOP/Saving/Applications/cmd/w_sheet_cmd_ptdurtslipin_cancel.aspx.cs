using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary.WcfNCommon;
using Sybase.DataWindow;
using System.Web.Services.Protocols;
using DataLibrary;

namespace Saving.Applications.cmd
{
    public partial class w_sheet_cmd_ptdurtslipin_cancel : PageWebSheet, WebSheet
    {
        string pbl = "cmd_ptdurtslipincancel.pbl";
        Sdt dt = new Sdt();
        private DwThDate tDwMain;

        protected string jsPostSearch;
        protected string jsPostSetProtect;

        public void InitJsPostBack()
        {
            tDwMain = new DwThDate(DwMain, this);
            tDwMain.Add("slip_dates", "tslip_dates");
            tDwMain.Add("slip_datee", "tslip_datee");

            jsPostSearch = WebUtil.JsPostBack(this, "jsPostSearch");
            jsPostSetProtect = WebUtil.JsPostBack(this, "jsPostSetProtect");
        }

        public void WebSheetLoadBegin()
        {
            this.ConnectSQLCA();
            DwDetail.SetTransaction(sqlca);
            if (!IsPostBack)
            {
                Reset();
            }
            else
            {
                this.RestoreContextDw(DwMain, tDwMain);
                this.RestoreContextDw(DwDetail);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            switch (eventArg)
            {
                case "jsPostSearch":
                    PostSearch();
                    break;
                case "jsPostSetProtect":
                    PostSetProtect();
                    break;
            }
        }

        public void SaveWebSheet()
        {
            string slipin_no = "", remark = "";
            string sqlupslipin = "", sqlupdurtmast = "";
            decimal choose_flag = 0;
            Sdt dt1, dt2;
            try
            {
                for (int i = 1; i <= DwDetail.RowCount; i++)
                {
                    choose_flag = DwDetail.GetItemDecimal(i, "choose_flag");
                    slipin_no = DwDetail.GetItemString(i, "slipin_no");
                    try { remark = DwDetail.GetItemString(i, "remark"); }
                    catch { remark = string.Empty; }
                    if (choose_flag == 1)
                    {
                        ///update ptdurtslipin
                        sqlupslipin = @"update ptdurtslipin 
                                    set slip_status = -9, cancel_id = {1}, 
                                        cancel_date = {2}, remark = {3}
                                    where slipin_no = {0}";
                        sqlupslipin = WebUtil.SQLFormat(sqlupslipin, slipin_no, state.SsUsername, state.SsWorkDate, remark);
                        dt1 = WebUtil.QuerySdt(sqlupslipin);
                        ///update ptdurtmaster
                        sqlupdurtmast = @"update ptdurtmaster
                                    set durt_status = -9, cancel_id = {1}, 
                                        cancel_date = {2}, remark = {3}
                                    where lot_id = {0}";
                        sqlupdurtmast = WebUtil.SQLFormat(sqlupdurtmast, slipin_no, state.SsUsername, state.SsWorkDate, remark);
                        dt2 = WebUtil.QuerySdt(sqlupdurtmast);
                    }
                }
                LtServerMessage.Text = WebUtil.CompleteMessage("ระบบได้ทำการยกเลิกรายการลงรับเลขที่ " + slipin_no + " เรียบร้อยแล้ว");
                Reset();
            }
            catch (Exception ex)
            { LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message); }
        }

        public void WebSheetLoadEnd()
        {
            DwMain.SaveDataCache();
            DwDetail.SaveDataCache();
            tDwMain.Eng2ThaiAllRow();
        }

        #region Function
        private void Reset()
        {
            DwMain.Reset();
            DwDetail.Reset();
            DwMain.InsertRow(0);
            DwMain.SetItemDateTime(1, "slip_dates", state.SsWorkDate);
            DwMain.SetItemDateTime(1, "slip_datee", state.SsWorkDate);
            HdRow.Value = "0";
            PostSearch();
            PostSetProtect();
        }

        private void PostSetProtect()
        {
            Int32 rowse = Convert.ToInt32(HdRow.Value);
            decimal choose_flag = 0;
            for (Int32 i = 1; i <= DwDetail.RowCount; i++)
            {
                if (i == rowse)
                {
                    choose_flag = DwDetail.GetItemDecimal(rowse, "choose_flag");
                    if (choose_flag == 0)
                    { DwDetail.SetItemString(rowse, "remark", string.Empty); }
                }
                else
                {
                    DwDetail.SetItemDecimal(i, "choose_flag", 0);
                    DwDetail.SetItemString(i, "remark", string.Empty);
                }
            }
        }

        private void PostSearch()
        {
            string slipin_no = "", ref_docno = "", ls_sql = "", ls_sqlmodify = "", ls_sqlgrpby = "", ls_sqlorder = "";
            DateTime slip_dates, slip_datee;
            try { slipin_no = DwMain.GetItemString(1, "slipin_no"); }
            catch { slipin_no = string.Empty; }
            try { ref_docno = DwMain.GetItemString(1, "ref_docno"); }
            catch { ref_docno = string.Empty; }
            try { slip_dates = DwMain.GetItemDateTime(1, "slip_dates"); }
            catch { slip_dates = state.SsWorkDate; }
            try { slip_datee = DwMain.GetItemDateTime(1, "slip_datee"); }
            catch { slip_datee = state.SsWorkDate; }
            ls_sql = DwDetail.GetSqlSelect();
            if (slipin_no.Length > 0)
            {
                ls_sqlmodify += " and PTDURTSLIPIN.SLIPIN_NO like '%"+ slipin_no +"%'";
            }
            if (ref_docno.Length > 0)
            {
                ls_sqlmodify += " and PTDURTSLIPIN.REF_DOCNO like '%" + ref_docno + "%'";
            }
            ls_sqlmodify += " and PTDURTSLIPIN.SLIP_DATE between {0} and {1} ";
            ls_sqlgrpby = @" GROUP BY PTDURTSLIPIN.SLIPIN_NO,PTDURTSLIPIN.SLIP_DATE,   
                             PTDURTSLIPIN.DURT_NAME,PTDURTSLIPIN.DURT_QTY,PTDURTSLIPIN.REMARK";
            ls_sqlorder = " ORDER BY PTDURTSLIPIN.SLIPIN_NO";
            ls_sql = ls_sql + ls_sqlmodify + ls_sqlgrpby + ls_sqlorder;
            ls_sql = WebUtil.SQLFormat(ls_sql, slip_dates, slip_datee);
            DwDetail.SetSqlSelect(ls_sql);
            DwDetail.Retrieve();
            this.DisConnectSQLCA();
        }
        #endregion
    }
}