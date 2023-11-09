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
using DataLibrary;
using System.Globalization;
using Sybase.DataWindow;
using CoreSavingLibrary.WcfNCommon;
using System.Data.SqlClient;

namespace Saving.Applications.cmd
{
    public partial class w_sheet_cmd_ptdurtslipout : PageWebSheet, WebSheet
    {
        protected String jsPostDel;
        protected String jsPostFindShow;
        protected String jsPostInsertRow;
        String pbl = "cmd_ptdurtslipout.pbl";
        Sdt dt = new Sdt();
        private DwThDate tDwMain;

        public void InitJsPostBack()
        {
            tDwMain = new DwThDate(DwMain, this);
            tDwMain.Add("slip_date", "slip_tdate");

            jsPostDel = WebUtil.JsPostBack(this, "jsPostDel");
            jsPostFindShow = WebUtil.JsPostBack(this, "jsPostFindShow");
            jsPostInsertRow = WebUtil.JsPostBack(this, "jsPostInsertRow");
        }

        public void WebSheetLoadBegin()
        {

            if (!IsPostBack)
            {
                PostReset();
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
                case "jsPostDel":
                    PostDel();
                    break;
                case "jsPostFindShow":
                    PostFindShow();
                    break;
                case "jsPostInsertRow":
                    PostInsertRow();
                    break;
            }
        }

        public void SaveWebSheet()
        {
            Sdt ta, ta1, ta2;
            ///ptdurtslipout
            String insert = "", upsql = "";
            String slipout_no = "", committee_no = "", cutreason_code = "", ref_docno = "";
            Decimal slip_status = 1;
            DateTime slip_date, sale_date;
            ///ptdurtslipoutdet
            String durt_id = "", remark = "";
            Decimal price_bal = 0, sale_amt = 0, durt_status = 1;
            try
            {
                slipout_no = DwMain.GetItemString(1, "slipout_no").Trim();
                if (slipout_no == "AUTO")
                {
                    n_commonClient com = wcf.NCommon;
                    slipout_no = com.of_getnewdocno(state.SsWsPass, state.SsCoopControl, "CMDSLIPOUTDURT");
                    try { slip_date = DwMain.GetItemDateTime(1, "slip_date"); }
                    catch { slip_date = state.SsWorkDate; }
                    try { sale_date = DwMain.GetItemDateTime(1, "sale_date"); }
                    catch { sale_date = state.SsWorkDate; }
                    try { ref_docno = DwMain.GetItemString(1, "ref_docno").Trim(); }
                    catch { ref_docno = ""; }
                    try { cutreason_code = DwMain.GetItemString(1, "cutreason_code"); }
                    catch { }
                    //insert ptdurtslipout
                    insert = @"insert into ptdurtslipout (slipout_no, slip_date, sale_date, committee_no,
	                cutreason_code, ref_docno, slip_status) values
                    ('" + slipout_no + "',to_date('" + slip_date.ToString("dd/MM/yyyy") + @"','dd/MM/yyyy'), 
                    to_date('" + sale_date.ToString("dd/MM/yyyy") + @"','dd/MM/yyyy'),'" + committee_no + @"',
                    '" + cutreason_code + "','" + ref_docno + "'," + slip_status + ")";
                    ta = WebUtil.QuerySdt(insert);

                    //insert ptdurtslipoutdet
                    for (Int32 i = 1; i <= DwDetail.RowCount; i++)
                    {
                        try { durt_id = DwDetail.GetItemString(i, "durt_id").Trim(); }
                        catch { throw new Exception("กรุณาระบุ เลขวัสดุที่ต้องการตัดจำหน่าย !!!"); }
                        try { price_bal = DwDetail.GetItemDecimal(i, "devaluebal_amt"); }
                        catch { price_bal = 0; }
                        try { sale_amt = DwDetail.GetItemDecimal(i, "sale_amt"); }
                        catch { sale_amt = 0; }
                        durt_status = -1;
                        try { remark = DwDetail.GetItemString(i, "remark").Trim(); }
                        catch { remark = ""; }

                        //insert detail
                        insert = @"insert into ptdurtslipoutdet (durt_id, price_bal, price_sale , slipout_no) 
                                values('" + durt_id + "'," + price_bal + "," + sale_amt + ", '" + slipout_no + "')";
                        ta1 = WebUtil.QuerySdt(insert);

                        //update ptdurtmaster
                        upsql = @"update ptdurtmaster set sale_date = to_date('" + sale_date.ToString("dd/MM/yyyy") + @"','dd/mm/yyyy'),
                        durt_status = " + durt_status + ", sale_amt = " + sale_amt + ", remark = '" + remark + @"'
                        where durt_id = '" + durt_id + "'";
                        ta2 = WebUtil.QuerySdt(upsql);
                    }
                }
                LtServerMessage.Text = WebUtil.CompleteMessage("ลงรับรายการ " + slipout_no + " สำเร็จ");
                PostReset();
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        public void WebSheetLoadEnd()
        {
            DwMain.SaveDataCache();
            tDwMain.Eng2ThaiAllRow();
            DwDetail.SaveDataCache();
        }

        private void PostReset()
        {
            DwMain.Reset();
            DwDetail.Reset();
            DwMain.InsertRow(0);
            DwDetail.InsertRow(0);
            DwMain.SetItemDateTime(1, "slip_date", state.SsWorkDate);
            DwMain.SetItemDateTime(1, "sale_date", state.SsWorkDate);
            DwMain.SetItemDecimal(1, "slip_status", 1);
            DwDetail.SetItemDecimal(1, "seq_no", 1);
            DwDetail.SetItemDecimal(1, "price_bal", 0);
            DwDetail.SetItemDecimal(1, "price_sale", 0);
            DwUtil.RetrieveDDDW(DwMain, "cutreason_code", pbl);
            DwUtil.RetrieveDDDW(DwDetail, "durt_status", pbl);
        }

        private void PostDel()
        {
            Int32 row = Convert.ToInt32(HdR.Value);
            DwDetail.DeleteRow(row);
        }

        private void PostFindShow()
        {
            string durt_id = "", durt_name = "";
            decimal devaluebal_amt = 0;
            int row = Convert.ToInt32(HdR.Value);
            try
            {
                try { durt_id = DwDetail.GetItemString(row, "durt_id").Trim(); }
                catch { durt_id = HdDurtId.Value.Trim(); }
                string sqlse = @"select durt_name, devaluebal_amt from ptdurtmaster where durt_id = {0}";
                sqlse = WebUtil.SQLFormat(sqlse, durt_id);
                dt = WebUtil.QuerySdt(sqlse);
                if (dt.Next())
                {
                    durt_name = dt.GetString("durt_name");
                    devaluebal_amt = dt.GetDecimal("devaluebal_amt");
                    DwDetail.SetItemString(row, "durt_name", durt_name);
                    DwDetail.SetItemDecimal(row, "price_bal", devaluebal_amt);
                }
            }
            catch (Exception ex)
            { LtServerMessage.Text = WebUtil.WarningMessage2("ไม่พบข้อมูล กรุณาตรวจสอบ !!!" + ex.Message); }
        }

        private void PostInsertRow()
        {
            int row = DwDetail.InsertRow(0);
            DwDetail.SetItemDecimal(2, "seq_no", 2);
            DwDetail.SetItemDecimal(2, "price_bal", 0);
            DwDetail.SetItemDecimal(2, "price_sale", 0);
        }
    }
}