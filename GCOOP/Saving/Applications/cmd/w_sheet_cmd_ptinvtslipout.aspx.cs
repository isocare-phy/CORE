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
    public partial class w_sheet_cmd_ptinvtslipout : PageWebSheet, WebSheet
    {
        String pbl = "cmd_ptinvtslipout.pbl";
        String item_code = "002"; //รายการเบิกวัสดุ
        Sdt ta;
        private DwThDate tDwMain;
        protected String jsPostDel;
        protected String jsPostFindShow;
        protected String jspostDlgShow;
        protected String jsPostInvtId;
        protected String jsCheckBal;
        protected String postTakereasonCode;


        public void InitJsPostBack()
        {
            tDwMain = new DwThDate(DwMain, this);
            tDwMain.Add("slip_date", "slip_tdate");
            jsPostFindShow = WebUtil.JsPostBack(this, "jsPostFindShow");
            jsPostDel = WebUtil.JsPostBack(this, "jsPostDel");
            jspostDlgShow = WebUtil.JsPostBack(this, "jspostDlgShow");
            jsPostInvtId = WebUtil.JsPostBack(this, "jsPostInvtId");
            jsCheckBal = WebUtil.JsPostBack(this, "jsCheckBal");
            postTakereasonCode = WebUtil.JsPostBack(this, "postTakereasonCode");
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                DwMain.InsertRow(0);
                DwDetail.InsertRow(0);
                DwMain.SetItemDateTime(1, "slip_date", state.SsWorkDate);
                DwUtil.RetrieveDDDW(DwMain, "dept_code", pbl, null);
                DwUtil.RetrieveDDDW(DwMain, "takereason_code", pbl, null);
                DwUtil.RetrieveDDDW(DwMain, "branch_code", pbl, null);
               
                DwDetail.SetItemDecimal(1, "invt_qty", 0);
                DwDetail.SetItemDecimal(1, "unit_price", 0);
            }
            else
            {
                DwMain.SetItemString(1, "branch_code", "001");
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
                case "jspostDlgShow":
                    PostDlgShow();
                    break;
                case "jsPostInvtId":
                    PostInvtId();
                    break;
                case "jsCheckBal":
                    CheckBal();
                    break;
                case "postTakereasonCode":
                    JsTakereasonCode();
                    break;
            }
        }

        public void SaveWebSheet()
        {
            String slipout_no = "", taker_name = "", apv_name = "", takereason_oth = "", takereason_code = "", dept_code = "";
            String invt_id = "", lot_id = "", branch_code = "", invtcut_type = "";
            Decimal slip_status = 1, invt_qty = 0, unit_price = 0 ;
            DateTime slip_date, operate_date;
            String insert = "";
            try
            {
                slipout_no = DwMain.GetItemString(1, "slipout_no");
                operate_date = state.SsWorkDate;
                if (slipout_no == "AUTO")
                {
                    n_commonClient com = wcf.NCommon;
                    slipout_no = com.of_getnewdocno(state.SsWsPass,state.SsCoopId, "CMDSLIPOUTINVT");
                    try { slip_date = DwMain.GetItemDateTime(1, "slip_date"); }
                    catch { slip_date = state.SsWorkDate; }
                    try { taker_name = DwMain.GetItemString(1, "taker_name").Trim(); }
                    catch { taker_name = string.Empty; }
                    try { apv_name = DwMain.GetItemString(1, "apv_name").Trim(); }
                    catch { apv_name = string.Empty; }
                    try { takereason_oth = DwMain.GetItemString(1, "takereason_oth").Trim(); }
                    catch { takereason_oth = string.Empty; }
                    try { takereason_code = DwMain.GetItemString(1, "takereason_code"); }
                    catch { takereason_code = string.Empty; }
                    try { dept_code = DwMain.GetItemString(1, "dept_code"); }
                    catch { dept_code = string.Empty; }
                    try { branch_code = DwMain.GetItemString(1, "branch_code"); }
                    catch { branch_code = string.Empty; }

                    ///insert into ptinvtslipout
                    try
                    {
                        insert = @"insert into ptinvtslipout
                                (
                                    slipout_no, slip_date, operate_date, 
                                    taker_name, slip_status, apv_name, takereason_oth,
                                    takereason_code, dept_code, branch_code
                                ) values
                                ( '" + slipout_no +"', to_date('"+ slip_date.ToString("dd/MM/yyyy") + @"','dd/MM/yyyy'), to_date('" + operate_date.ToString("dd/MM/yyyy") + @"','dd/MM/yyyy'),
                                  '"+ taker_name +"',"+ slip_status +",'"+ apv_name +"','"+ takereason_oth +@"',
                                  '" + takereason_code + "', '" + dept_code + "','" + branch_code + "')";
                        ta = WebUtil.QuerySdt(insert);
                    }
                    catch (Exception ex)
                    { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }

                    ///insert into ptinvtslipoutdet
                    try
                    {
                        Int32 rowde = DwDetail.RowCount ;
                        Decimal sumqty_bal = 0;

                        for (Int32 i = 1; i <= rowde; i++)
                        {
                            try { invt_id = DwDetail.GetItemString(i, "invt_id"); }
                            catch { throw new Exception("กรุณาระบุรหัสวัสดุ"); }
                            try { lot_id = DwDetail.GetItemString(i, "lot_id"); }
                            catch { throw new Exception("กรุณาระบุ lot วัสดุ"); }
                            try { invt_qty = DwDetail.GetItemDecimal(i, "invt_qty"); }
                            catch { throw new Exception("กรุณาระบุจำนวนวัสดุที่ต้องการเบิก"); }
                            try { unit_price = DwDetail.GetItemDecimal(i, "unit_price"); }
                            catch { }
                            try { invtcut_type = DwDetail.GetItemString(i, "invtcut_type"); }
                            catch { invtcut_type = "FI"; }

                            insert = @"insert into ptinvtslipoutdet 
                                     (
	                                    slipout_no, invt_id, lot_id,
	                                    invt_qty, unit_price
                                     ) values
                                     (
                                        '" + slipout_no + "','" + invt_id + "', '" + lot_id + @"',
                                        " + invt_qty + ", " + unit_price + ")";
                            ta = WebUtil.QuerySdt(insert);

                            //ให้เช็ครูปแบบการตัดวัสดุจาก field invtcut_type แทน
                            if (invtcut_type == "FI")
                            {
                                UpdateMastLotAuto(invt_id, invt_qty, slipout_no, slip_date);
                                sumqty_bal = UpQtyBal(invt_id, invt_qty);
                            }
                            else if (invtcut_type == "LI")
                            {
                                UpdateMastLotLIFO(invt_id, invt_qty, slipout_no, slip_date);
                                sumqty_bal = UpQtyBal(invt_id, invt_qty);
                            }
                            else if (invtcut_type == "CU")
                            {
                                sumqty_bal = UpQtyBal(invt_id, invt_qty);
                                InsertStatement(invt_qty, sumqty_bal, lot_id, slipout_no, invt_id, slip_date);
                                UpdateMastLot(invt_id, lot_id, invt_qty);
                            }
                        }
                    }
                    catch (Exception ex)
                    { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }

                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ เลขที่ทำรายการเบิกเลขที่ " + slipout_no);
                    DwMain.Reset();
                    DwDetail.Reset();
                    DwMain.InsertRow(0);
                    DwDetail.InsertRow(0);
                }
            }
            catch(Exception ex)
            { LtServerMessage.Text = WebUtil.ErrorMessage(ex) ; }
        }

        public void WebSheetLoadEnd()
        {
            DwMain.SaveDataCache();
            DwDetail.SaveDataCache();
            tDwMain.Eng2ThaiAllRow();
        }

        #region Extra Function
        private void PostDel()
        {
            Int32 row = Convert.ToInt32(HdR.Value);
            DwDetail.DeleteRow(row);
            CalPriceMain();
        }

        private void CalPriceMain()
        {
            Decimal sumprict = 0, price = 0;
            Int32 row = DwDetail.RowCount;
            for (int i = 1; i <= row; i++)
            {
                price = DwDetail.GetItemDecimal(i, "sumprice");
                sumprict = sumprict + price;
            }
            DwMain.SetItemDecimal(1, "sumprice", sumprict);
        }

        private void PostFindShow()
        {
            String invt_id = "", invt_name = "", lot_id = "";
            Decimal qty_bal = 0;
            Int32 row = Convert.ToInt32(HdR.Value);
            invt_id = HinvtId.Value;
                try
                {
                    String se = @"select invt_name ,qty_bal
                                from ptinvtmast where 
                                invt_id = '" + invt_id + "'";
                    ta = WebUtil.QuerySdt(se);
                    if (ta.Next())
                    {
                        invt_name = ta.GetString("invt_name").Trim();
                        qty_bal = ta.GetDecimal("qty_bal");
                    }
                    DwDetail.SetItemString(row, "invt_id", invt_id);
                    DwDetail.SetItemString(row, "invt_name", invt_name);
                    DwDetail.SetItemDecimal(row, "qty_bal", qty_bal);
                }
                catch { }
           Lotid(row, invt_id, lot_id);
           DwDetail.SetItemString(row, "lot_id", "AUTO");
                
        }

        private void PostDlgShow()
        {
            String invt_id = "", lot_id = "";
            Decimal unit_price = 0;
            Int32 row = Convert.ToInt32(HdR.Value);
            lot_id = HinvtId.Value;
            invt_id = DwDetail.GetItemString(row, "invt_id".Trim());
            
            try
            {
                String se = "select unit_price from ptmetlmastlot where lot_id = '" + lot_id + "' order by lot_id asc ";
                ta = WebUtil.QuerySdt(se);
                if (ta.Next())
                {
                    unit_price = ta.GetDecimal("unit_price");
                }
                DwDetail.SetItemString(row, "lot_id", lot_id);
                DwDetail.SetItemDecimal(row, "unit_price", unit_price);
            }
            catch { }

        }

        private void PostInvtId()
        {
            String invt_id = "", invt_name = "", lot_id = "", invtcut_type = "";
            Decimal qty_bal = 0;
            Int32 row = Convert.ToInt32(HdR.Value);
            invt_id = DwDetail.GetItemString(row, "invt_id").Trim();
            try
            {
                String se = @"select ptinvtmast.invt_name, ptinvtmast.qty_bal, ptinvtmast.invtcut_type
                                from ptinvtmast where 
                                ptinvtmast.invt_id = '" + invt_id + "'";
                ta = WebUtil.QuerySdt(se);
                if (ta.Next())
                {
                    invt_name = ta.GetString("invt_name").Trim();
                    qty_bal = ta.GetDecimal("qty_bal");
                    invtcut_type = ta.GetString("invtcut_type");
                }
                DwDetail.SetItemString(row, "invt_id", invt_id);
                DwDetail.SetItemString(row, "invt_name", invt_name);
                DwDetail.SetItemDecimal(row, "qty_bal", qty_bal);
                DwDetail.SetItemString(row, "invtcut_type", invtcut_type);
            }
            catch { }
            Lotid(row, invt_id, lot_id);
            DwDetail.SetItemString(row, "lot_id", "AUTO");
        }
        ///เลือก lot วัสดุที่ต้องการตัดเบิก
        private void Lotid(Int32 row, String invtId, String lotId) 
        {
            Decimal unit_price = 0;
            try
            {
                if (lotId == "" || lotId == null)
                {
                    lotId = "%";
                }
                else
                {
                    lotId = lotId + "%";
                }

                String se = "select lot_id, unit_price from ptmetlmastlot where qty_bal <> 0 and invt_id ='" + invtId + "' and lot_id like '" + lotId + "' order by lot_id asc ";
                ta = WebUtil.QuerySdt(se);
                if (ta.Next())
                {
                    lotId = ta.GetString("lot_id");
                    unit_price = ta.GetDecimal("unit_price");
                }
                if (lotId == "%")
                {
                    throw new Exception("รายการวัสดุที่ต้องการเบิกไม่มีจำนวนคงเหลือ");
                }
                else
                {
                    DwDetail.SetItemString(row, "lot_id", lotId);
                    DwDetail.SetItemDecimal(row, "unit_price", unit_price);
                }
            }
            catch(Exception ex)
            { 
                LtServerMessage.Text = WebUtil.WarningMessage(ex);
                DwDetail.SetItemString(row, "invt_id", "");
                DwDetail.SetItemString(row, "lot_id", "");
                DwDetail.SetItemString(row, "invt_name", "");
                DwDetail.SetItemDecimal(row, "invt_qty", 0);
                DwDetail.SetItemDecimal(row, "unit_price", 0);
            }
        }            
        ///update qty_bal in ptinvtmast 
        Decimal UpQtyBal(String InvtId, Decimal QtyAmt)
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
                qty_bal = qty_bal - QtyAmt;

                String upsql = "update ptinvtmast set qty_bal = " + qty_bal + " where invt_id = '" + InvtId + "'";
                ta = WebUtil.QuerySdt(upsql);
            }
            catch (Exception ex)
            { LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถอัพเดทยอดคงเหลือได้ " + ex); }
            return qty_bal;
        }
        ///insert into ptinvtstatement เพิ่มการเคลื่อนไหว
        private void InsertStatement(Decimal QtyAmt, Decimal QtyBal, String LotId, String SlipoutNo, String InvtId, DateTime SlipDate)
        {
            Decimal SeqNo = MaxSeqInvtId(InvtId);
            try
            {
                String insert = @"insert into ptinvtstatement
                    (
	                    seq_no,
	                    item_code,
	                    qty_amt,
	                    qty_bal,
	                    operate_date,
	                    ref_lotid,
	                    invt_id,
	                    ref_slipno,
	                    slip_date 
                    ) values
                    (   " + SeqNo + @", 
                        '" + item_code + @"', 
                        " + QtyAmt + @", 
                        " + QtyBal + @", 
                        to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy") + @"','dd/MM/yyyy'), 
                        '" + LotId + @"',
                        '" + InvtId + @"',
                        '" + SlipoutNo + @"',
                        to_date('" + SlipDate.ToString("dd/MM/yyyy") + @"','dd/MM/yyyy') 
                    )";
                ta = WebUtil.QuerySdt(insert);
            }
            catch (Exception ex)
            { LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถบันทึกการเคลื่อนไหวการลงรับวัสดุได้ " + ex); }
        }
        ///update into ptmetlmastlot อัพเดท lot
        private void UpdateMastLot(String InvtId,String LotId, Decimal InvQty)
        {
            Decimal qty_bal = 0;
            try
            {
                String sesql = "select qty_bal from ptmetlmastlot where invt_id = '" + InvtId + "' and lot_id = '"+ LotId +"'";
                ta = WebUtil.QuerySdt(sesql);
                if (ta.Next())
                {
                    qty_bal = ta.GetDecimal("qty_bal");
                }
                qty_bal = qty_bal - InvQty;
                String upsql = @"update ptmetlmastlot set qty_bal = "+ qty_bal +@" 
                                where invt_id = '" + InvtId + "' and lot_id = '"+ LotId +"'";
                ta = WebUtil.QuerySdt(upsql);
            }
            catch (Exception ex)
            { LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถอัพเดทรายการลงเคลื่อนไหว LOT วัสดุได้ " + ex); }

        }
        ///update ptmetlmastlot แบบอัตโนมัติ + insert เคลื่อนไหว FIFO
        private void UpdateMastLotAuto(String InvtId, Decimal InvQty, String SlipoutNo, DateTime SlipDate)
        {
            Decimal qty_bal = 0, ChkQtyBal = 0;
            String lot_id = "", upsql = "";
            try
            {
                String sesql = "select lot_id, qty_bal from ptmetlmastlot where invt_id = '" + InvtId + "' and qty_bal > 0 order by lot_id asc";
                ta = WebUtil.QuerySdt(sesql);
                if (ta.Next())
                {
                    lot_id = ta.GetString("lot_id").Trim();
                    qty_bal = ta.GetDecimal("qty_bal");
                }
                if (qty_bal >= InvQty)
                {
                    qty_bal = qty_bal - InvQty;
                    upsql = @"update ptmetlmastlot set qty_bal = " + qty_bal + @" 
                                where invt_id = '" + InvtId + "' and lot_id = '" + lot_id + "'";
                    ta = WebUtil.QuerySdt(upsql);
                    InsertStatement(InvQty, qty_bal, lot_id, SlipoutNo, InvtId, SlipDate);
                }
                else
                {
                    ChkQtyBal = InvQty - qty_bal;
                    upsql = @"update ptmetlmastlot set qty_bal = 0  
                            where invt_id = '" + InvtId + "' and lot_id = '" + lot_id + "'";
                    ta = WebUtil.QuerySdt(upsql);
                    InsertStatement(qty_bal, 0, lot_id, SlipoutNo, InvtId, SlipDate);
                }
                if (ChkQtyBal > 0)
                { UpdateMastLotAuto(InvtId, ChkQtyBal, SlipoutNo, SlipDate); }

            }
            catch (Exception ex)
            { LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถอัพเดทรายการเคลื่อนไหว LOT วัสดุแบบอัตโนมัติได้ " + ex.Message); }
        }
        
        ///update ptmetlmastlot แบบอัตโนมัติ + insert เคลื่อนไหว LIFO
        private void UpdateMastLotLIFO(String InvtId, Decimal InvQty, String SlipoutNo, DateTime SlipDate)
        {
            Decimal qty_bal = 0, ChkQtyBal = 0;
            String lot_id = "", upsql = "";
            try
            {
                String sesql = "select lot_id, qty_bal from ptmetlmastlot where invt_id = '" + InvtId + "' and qty_bal > 0 order by lot_id desc";
                ta = WebUtil.QuerySdt(sesql);
                if (ta.Next())
                {
                    lot_id = ta.GetString("lot_id").Trim();
                    qty_bal = ta.GetDecimal("qty_bal");
                }
                if (qty_bal >= InvQty)
                {
                    qty_bal = qty_bal - InvQty;
                    upsql = @"update ptmetlmastlot set qty_bal = " + qty_bal + @" 
                                where invt_id = '" + InvtId + "' and lot_id = '" + lot_id + "'";
                    ta = WebUtil.QuerySdt(upsql);
                    InsertStatement(InvQty, qty_bal, lot_id, SlipoutNo, InvtId, SlipDate);
                }
                else
                {
                    ChkQtyBal = InvQty - qty_bal;
                    upsql = @"update ptmetlmastlot set qty_bal = 0  
                            where invt_id = '" + InvtId + "' and lot_id = '" + lot_id + "'";
                    ta = WebUtil.QuerySdt(upsql);
                    InsertStatement(qty_bal, 0, lot_id, SlipoutNo, InvtId, SlipDate);
                }
                if (ChkQtyBal > 0)
                { UpdateMastLotLIFO(InvtId, ChkQtyBal, SlipoutNo, SlipDate); }

            }
            catch (Exception ex)
            { LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถอัพเดทรายการเคลื่อนไหว LOT วัสดุแบบอัตโนมัติได้ " + ex.Message); }
        }

        //เช็คจำนวนที่ปรับปรุง จาก จำนวนคงเหลือ
        private void CheckBal()
        {
            Int32 row = Convert.ToInt32(HdR.Value);
            Decimal qty_bal = 0, invt_qty = 0;
            String invt_id = "";//, lot_id = "";
            try { invt_id = DwDetail.GetItemString(row, "invt_id").Trim(); }
            catch { }
            //try { lot_id = DwDetail.GetItemString(row, "lot_id").Trim(); }
            //catch { }
            try { invt_qty = DwDetail.GetItemDecimal(row, "invt_qty"); }
            catch { }
            try
            {
                String sesql = "select qty_bal from ptinvtmast where invt_id = '" + invt_id + "' ";
                ta = WebUtil.QuerySdt(sesql);
                if (ta.Next())
                {
                    qty_bal = ta.GetDecimal("qty_bal");
                }
                    if (qty_bal < invt_qty)
                    {
                        invt_qty = 0;
                        LtServerMessage.Text = WebUtil.ErrorMessage("จำนวนที่เบิกเกินกว่าจำนวนที่คงเหลือ");
                    }
                    else { }
            }
            catch { }

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

        private void JsTakereasonCode()
        {
            string takereason_code = "";
            try
            {
                takereason_code = DwMain.GetItemString(1, "takereason_code");
            }
            catch
            {

            }
            if (takereason_code == "99" || takereason_code == "อื่นๆ")
            {
                DwMain.SetItemString(1, "takereason_oth", "");
            }
            else
            {
                DwUtil.RetrieveDDDW(DwMain, "takereason_oth", pbl, null);
            }

        }
    #endregion

    }
}

