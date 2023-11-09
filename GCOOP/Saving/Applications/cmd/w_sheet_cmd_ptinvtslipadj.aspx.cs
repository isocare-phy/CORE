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
    public partial class w_sheet_cmd_ptinvtslipadj : PageWebSheet, WebSheet
    {
        String pbl = "cmd_ptinvtslipadj.pbl";
        String item_code = "003"; //รายการปรับปรุงวัสดุ : 003 รายการปรับปรุงเพิ่ม, 004 รายการปรับปรุงลด
        Sdt ta;
        private DwThDate tDwMain;
        protected String jsPostFindShow;
        protected String jspostDlgShow;
        protected String jsCheckBal;

        public void InitJsPostBack()
        {

            tDwMain = new DwThDate(DwMain, this);
            tDwMain.Add("slip_date", "slip_tdate");
            jsPostFindShow = WebUtil.JsPostBack(this, "jsPostFindShow");
            jspostDlgShow = WebUtil.JsPostBack(this, "jspostDlgShow");
            jsCheckBal = WebUtil.JsPostBack(this, "jsCheckBal");
            DwUtil.RetrieveDDDW(DwMain, "adjtype_code", pbl, null);
        }

        public void WebSheetLoadBegin()
        {
            this.ConnectSQLCA();
            DwMain.SetTransaction(sqlca);
            if (!IsPostBack)
            {
                DwMain.InsertRow(0);
                DwMain.SetItemDateTime(1, "slip_date", state.SsWorkDate);
            }
            else
            {
                this.RestoreContextDw(DwMain, tDwMain);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            switch (eventArg)
            {
                case "jsPostFindShow":
                    PostFindShow();
                    break;
                case "jspostDlgShow":
                    PostDlgShow();
                    break;
                case "jsCheckBal":
                    CheckBal();
                    break;
            }
        }

        public void SaveWebSheet()
        {

            String slipadj_no = "", invt_id = "", lot_id = "", reason_desc = "", adjtype_code = "";
            Decimal invt_qty = 0, sumqty_bal = 0, sign_flag = 0;
            DateTime slip_date;
            try
            {
                slipadj_no = DwMain.GetItemString(1, "slipadj_no");
                if (slipadj_no == "AUTO")
                {
                    n_commonClient com = wcf.NCommon;
                    slipadj_no = com.of_getnewdocno(state.SsWsPass,state.SsCoopId, "CMDSLIPADJINVT");
                    try { slip_date = DwMain.GetItemDateTime(1, "slip_date"); }
                    catch { slip_date = state.SsWorkDate; }
                    try { invt_id = DwMain.GetItemString(1, "invt_id").Trim(); }
                    catch { }
                    try { lot_id = DwMain.GetItemString(1, "lot_id").Trim(); }
                    catch { }
                    try
                    {
                        adjtype_code = DwMain.GetItemString(1, "adjtype_code");
                        String se = @"  SELECT SIGN_FLAG
                                                            FROM PTUCFINVTADJTYPE 
                                                            WHERE ADJTYPE_CODE = '" + adjtype_code + "'";
                        ta = WebUtil.QuerySdt(se);
                        if (ta.Next())
                        {
                            sign_flag = ta.GetDecimal("sign_flag");
                        }
                    }
                    catch { }
                    try { reason_desc = DwMain.GetItemString(1, "reason_desc"); }
                    catch { }
                    try { invt_qty = DwMain.GetItemDecimal(1, "invt_qty"); }
                    catch { }

                    ///insert into ptinvtslipadj
                    try
                    {
                        String insert = @"insert into ptinvtslipadj
                                (
                                    slipadj_no, slip_date, invt_id, 
                                    lot_id, adjtype_code, reason_desc, invt_qty
                                ) values
                                ( '" + slipadj_no + "', to_date('" + slip_date.ToString("dd/MM/yyyy") + @"','dd/MM/yyyy'),
                                  '" + invt_id + "','" + lot_id + "','" + adjtype_code + "','" + reason_desc + @"',
                                  " + invt_qty + ")";
                        ta = WebUtil.QuerySdt(insert);
                        UpdateMastLot(invt_id, lot_id, invt_qty, sign_flag);
                        sumqty_bal = UpQtyBal(invt_id, invt_qty, sign_flag);
                        InsertStatement(invt_qty, sumqty_bal, lot_id, slipadj_no, invt_id, slip_date);                     
                    }
                    catch (Exception ex)
                    { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }

                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ " + slipadj_no);
                    ResetPage();
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
            }
        }

        public void WebSheetLoadEnd()
        {
            DwMain.SaveDataCache();
            tDwMain.Eng2ThaiAllRow();
        }

        #region Extra Function
        ///update qty_bal in ptinvtmast 
        Decimal UpQtyBal(String InvtId, Decimal QtyAmt, Decimal SignFlag)
        {
            Decimal qty_bal = 0;
            try
            {
                String sesql = "select qty_bal from ptinvtmast where invt_id = '" + InvtId + "'";
                ta = WebUtil.QuerySdt(sesql);
                if (ta.Next())
                {
                    qty_bal = ta.GetDecimal("qty_bal");
                }
                if (SignFlag < 0)
                {
                    qty_bal = qty_bal - QtyAmt;
                }
                else
                {
                    qty_bal = qty_bal + QtyAmt;
                }

                String upsql = "update ptinvtmast set qty_bal = " + qty_bal + " where invt_id = '" + InvtId + "'";
                ta = WebUtil.QuerySdt(upsql);
            }
            catch (Exception ex)
            { LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถอัพเดทยอดคงเหลือได้ " + ex); }
            return qty_bal;
        }

        private void InsertStatement(Decimal QtyAmt, Decimal QtyBal, String LotId, String SlipadjNo, String InvtId, DateTime SlipDate)
        {
            Decimal SeqNo = MaxSeqInvtId(InvtId);
            try
            {
                String insert = @"insert into ptinvtstatement
                    (
	                    seq_no, item_code, qty_amt, qty_bal, operate_date,
	                    ref_lotid, invt_id, ref_slipno, slip_date 
                    ) values
                    (   " + SeqNo + ",'" + item_code + "', " + QtyAmt + ", " + QtyBal + @", 
                        to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy") + @"','dd/MM/yyyy'), 
                        '" + LotId + @"', '" + InvtId + @"', '" + SlipadjNo + @"',
                        to_date('" + SlipDate.ToString("dd/MM/yyyy") + @"','dd/MM/yyyy') 
                    )";
                ta = WebUtil.QuerySdt(insert);
            }
            catch (Exception ex)
            { LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถบันทึกการเคลื่อนไหวการลงรับวัสดุได้ " + ex); }
        }

        ///update into ptmetlmastlot
        private void UpdateMastLot(String InvtId, String LotId, Decimal InvQty, Decimal SignFlag)
        {
            Decimal qty_bal = 0;
            try
            {
                String sesql = "select qty_bal from ptmetlmastlot where invt_id = '" + InvtId + "' and lot_id = '" + LotId + "'";
                ta = WebUtil.QuerySdt(sesql);
                if (ta.Next())
                {
                    qty_bal = ta.GetDecimal("qty_bal");
                }
                if (SignFlag < 0)
                {
                    qty_bal = qty_bal - InvQty;
                }
                else
                {
                    qty_bal = qty_bal + InvQty;
                }


                String upsql = @"update ptmetlmastlot set qty_bal = " + qty_bal + @" 
                                where invt_id = '" + InvtId + "' and lot_id = '" + LotId + "'";
                ta = WebUtil.QuerySdt(upsql);
            }
            catch (Exception ex)
            { LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถอัพเดทรายการลงเคลื่อนไหว LOT วัสดุได้ " + ex); }

        }

        Decimal MaxSeqInvtId(String InvtId)
        {
            Decimal maxseq = 0;
            String sesql = @"select max(seq_no) as seq_no from ptinvtstatement where invt_id = '" + InvtId.Trim() + "'";
            ta = WebUtil.QuerySdt(sesql);
            if (ta.Next())
            {
                maxseq = ta.GetDecimal("seq_no");
            }
            maxseq++;

            return maxseq;
        }

        private void PostFindShow()
        {
            String invt_id = "", lot_id = "", invt_name = "";
            invt_id = HinvtId.Value;
            try
            {
                String se = @"select ptinvtmast.invt_name, ptmetlmastlot.lot_id 
                                from ptinvtmast,ptmetlmastlot 
                                where ptmetlmastlot.invt_id = ptinvtmast.invt_id and
                                ptmetlmastlot.invt_id = '" + invt_id + "'";
                ta = WebUtil.QuerySdt(se);
                if (ta.Next())
                {
                    lot_id = ta.GetString("lot_id").Trim();
                    invt_name = ta.GetString("invt_name").Trim();
                }
                DwMain.SetItemString(1, "invt_id", invt_id);
                DwMain.SetItemString(1, "invt_name", invt_name);
                DwMain.SetItemDecimal(1, "invt_qty", 0);
            }
            catch { }
            Lotid(1, invt_id, lot_id);
        }

        private void PostDlgShow()
        {
            String invt_id = "", lot_id = "";
            Decimal unit_price = 0;
            lot_id = HinvtId.Value;
            invt_id = DwMain.GetItemString(1, "invt_id".Trim());

            try
            {
                String se = "select unit_price from ptmetlmastlot where lot_id = '" + lot_id + "' order by lot_id asc ";
                ta = WebUtil.QuerySdt(se);
                if (ta.Next())
                {
                    unit_price = ta.GetDecimal("unit_price");
                }
                DwMain.SetItemString(1, "lot_id", lot_id);
            }
            catch { }

        }
        //เช็คจำนวนที่ปรับปรุง จาก จำนวนคงเหลือ
        private void CheckBal()
        {
            Decimal qty_bal = 0, sign_flag = 0, invt_qty = 0;
            String invt_id = "", lot_id = "", adjtype_code = "";
            try { invt_id = DwMain.GetItemString(1, "invt_id").Trim(); }
            catch { }
            try { lot_id = DwMain.GetItemString(1, "lot_id").Trim(); }
            catch { }
            try { invt_qty = DwMain.GetItemDecimal(1, "invt_qty"); }
            catch { }
            try
            {
                adjtype_code = DwMain.GetItemString(1, "adjtype_code");
                String se = @"  SELECT SIGN_FLAG
                                        FROM PTUCFINVTADJTYPE 
                                        WHERE ADJTYPE_CODE = '" + adjtype_code + "'";
                ta = WebUtil.QuerySdt(se);
                if (ta.Next())
                {
                    sign_flag = ta.GetDecimal("sign_flag");
                }
            }
            catch { }

            try
            {
                String sesql = "select qty_bal from ptmetlmastlot where invt_id = '" + invt_id + "' and lot_id = '" + lot_id + "'";
                ta = WebUtil.QuerySdt(sesql);
                if (ta.Next())
                {
                    qty_bal = ta.GetDecimal("qty_bal");
                }
                if (sign_flag < 0)
                {
                    if (qty_bal < invt_qty)
                    {
                        DwMain.SetItemDecimal(1, "invt_qty", 0);
                        HdState.Value = "-9";
                        LtServerMessage.Text = WebUtil.ErrorMessage("จำนวนที่ปรับปรุงเกินกว่าจำนวนที่คงเหลือ");
                    }
                    else { HdState.Value = "1";  }
                }
                else { HdState.Value = "1"; }
            }
            catch { }

        }
        ///เลือก lot วัสดุที่ต้องการตัดเบิก
        private void Lotid(Int32 row, String invtId, String lotId)
        {
            if (lotId == "" || lotId == null)
            {
                lotId = "%";
            }
            else
            {
                lotId = lotId + "%";
            }

            String se = "select lot_id from ptmetlmastlot where qty_bal <> 0 and invt_id ='" + invtId + "' and lot_id like '" + lotId + "' order by lot_id asc ";
            ta = WebUtil.QuerySdt(se);
            if (ta.Next())
            {
                lotId = ta.GetString("lot_id");
            }
           
            DwMain.SetItemString(row, "lot_id", lotId.Replace("%",""));
        }

        public void ResetPage()
        {
            DwMain.Reset();
            DwMain.InsertRow(0);
            DwMain.SetItemDateTime(1, "slip_date", state.SsWorkDate);
        }

        #endregion
    }
}