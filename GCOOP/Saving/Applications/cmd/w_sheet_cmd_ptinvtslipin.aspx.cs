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
    public partial class w_sheet_cmd_ptinvtslipin : PageWebSheet, WebSheet
    {
        String pbl = "cmd_ptinvtslipin.pbl";
        String item_code = "001"; //รายการลงรับวัสดุ
        Sdt ta;
        private DwThDate tDwMain;
        protected String postFindShow;
        protected String jsPostInvtId;
        protected String jsPostOnInsert;
        protected String jsPostDel;
        protected string jsPostDealer;

        public void InitJsPostBack()
        {
            tDwMain = new DwThDate(DwMain, this);
            tDwMain.Add("slip_date", "slip_tdate");
            tDwMain.Add("buy_date", "buy_tdate");
            postFindShow = WebUtil.JsPostBack(this, "postFindShow");
            jsPostInvtId = WebUtil.JsPostBack(this, "jsPostInvtId");
            jsPostOnInsert = WebUtil.JsPostBack(this, "jsPostOnInsert");
            jsPostDel = WebUtil.JsPostBack(this, "jsPostDel");
            jsPostDealer = WebUtil.JsPostBack(this, "jsPostDealer");
        }

        public void WebSheetLoadBegin()
        {

            if (!IsPostBack)
            {
                DwMain.InsertRow(0);
                DwDetail.InsertRow(0);
                DwMain.SetItemDateTime(1, "slip_date", state.SsWorkDate);
                DwMain.SetItemDateTime(1, "buy_date", state.SsWorkDate);
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
                case "jsPostInvtId":
                    PostInvtId();
                break;
                case "jsPostOnInsert":
                    PostOnInsert();
                break;
                case "jsPostDel":
                    PostDel();
                break;
                case "postFindShow":
                    jsPostFindShow();
                break;
                case "jsPostDealer":
                    PostDealer();
                break;
            }
        }

        public void SaveWebSheet()
        {
            String slipin_no = "", ref_docno = "", invt_id = "";
            String brand_name = "", model_desc = "", dealer_no = "", dealer_name = "";
            Decimal unit_price = 0, invt_qty = 0, price_net = 0;
            DateTime slip_date, buy_date ;
            String insert = "";
            try
            {
                slipin_no = DwMain.GetItemString(1, "slipin_no");

                if (slipin_no == "AUTO")
                {
                    n_commonClient com = wcf.NCommon;
                    slipin_no = com.of_getnewdocno(state.SsWsPass,state.SsCoopId, "CMDSLIPININVT");
                    try { slip_date = DwMain.GetItemDateTime(1, "slip_date"); }
                    catch { slip_date = state.SsWorkDate; }
                    try { buy_date = DwMain.GetItemDateTime(1, "buy_date"); }
                    catch { buy_date = state.SsWorkDate; }
                    try { ref_docno = DwMain.GetItemString(1, "ref_docno").Trim(); }
                    catch { ref_docno = ""; }
                    try { dealer_no = DwMain.GetItemString(1, "dealer_no").Trim();}
                    catch { dealer_no = "";}
                    try { dealer_name = DwMain.GetItemString(1, "dealer_name").Trim(); }
                    catch{ dealer_name = "";}

                    ///insert into ptinvtslipin 
                    try
                    {
                        insert = @"insert into ptinvtslipin
                                (
	                                slipin_no, slip_date,
                                    buy_date, slip_status,
                                    entry_id, entry_date, 
                                    ref_docno, dealer_no, dealer_name         
                                ) values
                                ('" + slipin_no + @"',to_date('" + slip_date.ToString("dd/MM/yyyy") + @"','dd/MM/yyyy'), 
                                 to_date('" + buy_date.ToString("dd/MM/yyyy") + @"','dd/MM/yyyy'), 1,
                                 '" + state.SsUsername + "', to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy") + @"','dd/MM/yyyy'),
                                 '" + ref_docno + "', '"+ dealer_no +"','"+ dealer_name +"')";
                        ta = WebUtil.QuerySdt(insert);
                        //Sdt dt = state.SsOracleTA.RollBack(ta);
                    }
                    catch (Exception ex)
                    { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
                    ///insert into ptinvtslipindet
                    try
                    {
                        Int32 rowde = 0;
                        Decimal sumqty_bal = 0;
                        rowde = DwDetail.RowCount;

                        for (Int32 i = 1; i <= rowde; i++)
                        {
                            try { invt_id = DwDetail.GetItemString(i, "invt_id"); }
                            catch { throw new Exception("กรุณาระบุรหัสวัสดุให้ถูกต้อง"); }
                            try { brand_name = DwDetail.GetItemString(i, "brand_name").Trim(); }
                            catch { brand_name = ""; }
                            try { model_desc = DwDetail.GetItemString(i, "model_desc").Trim(); }
                            catch { model_desc = ""; }
                            try { unit_price = DwDetail.GetItemDecimal(i, "unit_price"); }
                            catch { unit_price = 0; }
                            try { invt_qty = DwDetail.GetItemDecimal(i, "invt_qty"); }
                            catch { invt_qty = 0; }
                            try { price_net = DwDetail.GetItemDecimal(i, "sumprice"); }
                            catch { price_net = 0; }

                            insert = @"insert into ptinvtslipindet 
                                     (
	                                    slipin_no, invt_id, brand_name,
	                                    model_desc, unit_price, invt_qty
                                     ) values
                                     (
                                        '"+ slipin_no +"','"+ invt_id +"', '"+ brand_name +@"',
                                        '"+ model_desc +"', "+ unit_price +", "+ invt_qty +@"
                                     )";
                            ta = WebUtil.QuerySdt(insert);
                            sumqty_bal = UpQtyBal(invt_id, invt_qty);
                            InsertStatement(invt_qty, sumqty_bal, slipin_no, invt_id, slip_date);
                            InsertMastLot(invt_id, brand_name, model_desc, slipin_no, unit_price, buy_date, invt_qty, invt_qty, price_net);
                        }

                    }
                    catch (Exception ex)
                    { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }

                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ " + slipin_no);
                    PostReset();
                }
                else
                {
                    throw new Exception("ไม่สามารถบันทึกรายการนี้ได้");
                }
            }
            catch (Exception ex)
            { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
        }

        public void WebSheetLoadEnd()
        {
            DwMain.SaveDataCache();
            DwDetail.SaveDataCache();
            tDwMain.Eng2ThaiAllRow();
        }

    #region ListFunction

        private void PostInvtId()
        {
            String invt_id = "", invt_name = "";
            Int32 row = Convert.ToInt32(HdR.Value);
            invt_id = DwDetail.GetItemString(row, "invt_id").Trim();
            try
            {
                String sesql = "select invt_name from ptinvtmast where invt_id = '" + invt_id + "'";
                ta = WebUtil.QuerySdt(sesql);
                if (ta.Next())
                {
                    invt_name = ta.GetString("invt_name").Trim();
                }
                DwDetail.SetItemString(row, "invt_name", invt_name);
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถดึงข้อมูลรายการวัสดุได้ " + ex);
            }
        }
        private void PostOnInsert()
        {
            DwDetail.InsertRow(0);
        }
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

            DwMain.SetItemDecimal(1, "sumamt", sumprict);
        }
        private void jsPostFindShow()
        {
            String invt_id = "", invt_name = "";
            Int32 row = Convert.ToInt32(HdR.Value);
            invt_id = HinvtId.Value;
            try
            {
                String se = @"select ptinvtmast.invt_name
                                from ptinvtmast where 
                                ptinvtmast.invt_id = '" + invt_id + "'";
                ta = WebUtil.QuerySdt(se);
                if (ta.Next())
                {
                    invt_name = ta.GetString("invt_name").Trim();
                }
                DwDetail.SetItemString(row, "invt_id", invt_id);
                DwDetail.SetItemString(row, "invt_name", invt_name);
            }
            catch { }
        }
        private void PostDealer()
        {
            string dealer_no = "", dealer_name = "";
            try
            {
                dealer_no = DwMain.GetItemString(1, "dealer_no");
                string sqlsedealer = @"select dealer_name from ptucfdealer where dealer_no = {0}";
                sqlsedealer = WebUtil.SQLFormat(sqlsedealer, dealer_no);
                ta = WebUtil.QuerySdt(sqlsedealer);
                if (ta.Next())
                {
                    dealer_name = ta.GetString("dealer_name").Trim();
                }
                if (dealer_name == null || dealer_name == string.Empty)
                {
                    throw new Exception("หมายเลขคู่ค้าเลขที่ " + dealer_no + " ไม่มีข้อมูล");
                }
                else
                {
                    DwMain.SetItemString(1, "dealer_name", dealer_name);
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
                DwMain.SetItemString(1, "dealer_no", "");
                DwMain.SetItemString(1, "dealer_name", "");
            }
        }
        private void PostReset()
        {
            DwMain.Reset();
            DwDetail.Reset();
            DwMain.InsertRow(0);
            DwDetail.InsertRow(0);
            DwMain.SetItemDateTime(1, "slip_date", state.SsWorkDate);
            DwMain.SetItemDateTime(1, "buy_date", state.SsWorkDate);
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
                qty_bal = qty_bal + QtyAmt;

                String upsql = "update ptinvtmast set qty_bal = " + qty_bal + " where invt_id = '" + InvtId + "'";
                ta = WebUtil.QuerySdt(upsql);
            }
            catch (Exception ex)
            { LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถอัพเดทยอดคงเหลือได้ " + ex); }
            return qty_bal;
        }
        ///insert into ptinvtstatement
        private void InsertStatement(Decimal QtyAmt, Decimal QtyBal, String SlipinNo, String InvtId, DateTime SlipDate)
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
                        to_date('" + SlipDate.ToString("dd/MM/yyyy") + @"','dd/MM/yyyy'), 
                        '" + SlipinNo + @"',
                        '" + InvtId + @"',
                        '" + SlipinNo + @"',
                        to_date('" + SlipDate.ToString("dd/MM/yyyy") + @"','dd/MM/yyyy') 
                    )";
                ta = WebUtil.QuerySdt(insert);
            }
            catch (Exception ex)
            { LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถบันทึกการเคลื่อนไหวการลงรับวัสดุได้ " + ex); }
        }
        ///insert into ptmetlmastlot
        private void InsertMastLot(String InvtId, String BrandName, String ModelDesc, String SlipinNo, Decimal UnitPrice, DateTime BuyDate, Decimal QtyAmt, Decimal QtyBal, Decimal PriceNet)
        {
            try
            {
                String insert = @"insert into ptmetlmastlot
                                (
	                                invt_id,
	                                brand_name,
	                                model_desc,
	                                lot_id,
	                                unit_price,
	                                buy_date,
	                                qty_amt,
	                                qty_bal,
                                    price_net

                                ) values
                                (
                                    '" + InvtId + @"',
                                    '" + BrandName + @"',
                                    '" + ModelDesc + @"',
                                    '" + SlipinNo + @"',
                                    " + UnitPrice + @",
                                    to_date('" + BuyDate.ToString("dd/MM/yyyy") + @"','dd/MM/yyyy'),
                                    " + QtyAmt + @",
                                    " + QtyBal + @",
                                    " + PriceNet + @"
                                )";
                ta = WebUtil.QuerySdt(insert);
            }
            catch (Exception ex)
            { LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถบันทึกรายการลงเคลื่อนไหว LOT วัสดุได้ " + ex); }

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

    #endregion

    }
}