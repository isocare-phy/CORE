using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommonLibrary;
using DBAccess;
using System.Data;

namespace Saving.Applications.app_assist
{
    public partial class w_sheet_kt_contant_familydead : PageWebSheet, WebSheet
    {
        protected String jsBtDel;
        protected String jsInsertRow;
        protected String jsTypeBtDel;

        protected Sta ta;
        public void InitJsPostBack()
        {
            jsBtDel = WebUtil.JsPostBack(this, "jsBtDel");
            jsInsertRow = WebUtil.JsPostBack(this, "jsInsertRow");
            jsTypeBtDel = WebUtil.JsPostBack(this, "jsTypeBtDel");
        }

        public void WebSheetLoadBegin()
        {
            this.ConnectSQLCA();
            ta = new Sta(sqlca.ConnectionString);

            if (!IsPostBack)
            {
                DwUtil.RetrieveDataWindow(DwMain, "kt_paydead.pbl", null, null);
                HdMainRowCount.Value = Convert.ToString(DwMain.RowCount);
                try
                {
                    DwMain.GetItemDecimal(1, "seq_no");
                }
                catch
                {
                    DwMain.Reset();
                }

                DwUtil.RetrieveDataWindow(DwType, "kt_paydead.pbl", null, null);
                HdTypeRowCount.Value = Convert.ToString(DwType.RowCount);
                try
                {
                    DwType.GetItemDecimal(1, "familytype_code");
                }
                catch
                {
                    DwType.Reset();
                }
            }
            else
            {
                this.RestoreContextDw(DwMain);
                this.RestoreContextDw(DwType);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            switch (eventArg)
            {
                case "jsBtDel":
                    JSBtDel();
                    break;
                case "jsInsertRow":
                    JSInsertRow();
                    break;
                case "jsTypeBtDel":
                    JSTypeBtDel();
                    break;
            }
        }

        public void SaveWebSheet()
        {
            String sql = "select count(*) as ktmax from asnucffamilydead";
            DataTable dt = WebUtil.Query(sql);
            int rowdb = 0;
            try
            {
                rowdb = Convert.ToInt32(dt.Rows[0]["ktmax"]);
            }
            catch { }
            int rowCount = DwMain.RowCount;

            if (rowCount == rowdb)
            {
                try
                {
                    DwUtil.UpdateDateWindow(DwMain, "kt_paydead.pbl", "asnucffamilydead");
                    DwUtil.RetrieveDataWindow(DwMain, "kt_paydead.pbl", null, null);
                    HdMainRowCount.Value = Convert.ToString(DwMain.RowCount);
                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถทำรายการได้" + ex);
                }
            }
            else
            {
                String mql = "select max(seq_no) as sqmax from asnucffamilydead";
                DataTable dt2 = WebUtil.Query(mql);
                int seqno = 0;
                try
                {
                    seqno = Convert.ToInt32(dt2.Rows[0]["sqmax"]);
                }
                catch { }
                int j = 1;
                try
                {

                    int rowFirst = 0;
                    if (seqno != 0)
                    {
                        rowFirst = int.Parse(HdMainRowCount.Value);
                    }
                    int[] rows = new int[rowCount - rowFirst];
                    int ii = 0;
                    for (int i = rowFirst; i < rowCount; i++)
                    {
                        DwMain.SetItemDecimal(i + 1, "seq_no", seqno + j);
                        rows[ii] = i + 1;
                        ii++;
                        j++;
                    }
                    DwUtil.InsertDataWindow(DwMain, "kt_paydead.pbl", "asnucffamilydead", rows);
                    DwUtil.UpdateDateWindow(DwMain, "kt_paydead.pbl", "asnucffamilydead");
                    DwUtil.RetrieveDataWindow(DwMain, "kt_paydead.pbl", null, null);
                    HdMainRowCount.Value = Convert.ToString(DwMain.RowCount);
                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลเรียบร้อยแล้ว");
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                }
            }
            String sqlMrange = "select count(*) as ktmrmax from asnucffamilytype";
            DataTable dtMrange = WebUtil.Query(sqlMrange);
            int Mrangerowdb = 0;
            try
            {
                Mrangerowdb = Convert.ToInt32(dt.Rows[0]["ktmrmax"]);
            }
            catch { }
            int MrowCount = DwType.RowCount;
            if (MrowCount == Mrangerowdb)
            {
                try
                {
                    DwUtil.UpdateDateWindow(DwType, "kt_paydead.pbl", "asnucffamilytype");
                    DwUtil.RetrieveDataWindow(DwType, "kt_paydead.pbl", null, null);
                    HdTypeRowCount.Value = Convert.ToString(DwType.RowCount);
                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถทำรายการได้" + ex);
                }
            }
            else
            {
                //String Mrangemql = "select max(familytype_code) as ktsmrmax from asnucfktmoneyrange";
                //DataTable dt2Mrange = WebUtil.Query(Mrangemql);
                //int Mseqno = 0;
                //try
                //{
                //    Mseqno = Convert.ToInt32(dt2Mrange.Rows[0]["ktsmrmax"]);
                //}
                //catch { }
                int Mj = 1;
                try
                {
                    int MrowFirst = 0;
                    //if (Mseqno != 0)
                    //{
                        MrowFirst = int.Parse(HdTypeRowCount.Value);
                    //}
                    int[] Mrows = new int[MrowCount - MrowFirst];
                    int Mii = 0;
                    for (int Mi = MrowFirst; Mi < MrowCount; Mi++)
                    {
                        //DwMoneyRange.SetItemDecimal(Mi + 1, "seq_no", Mseqno + Mj);
                        Mrows[Mii] = Mi + 1;
                        Mii++;
                        Mj++;
                    }
                    DwUtil.UpdateDateWindow(DwType, "kt_paydead.pbl", "asnucffamilytype");
                    DwUtil.InsertDataWindow(DwType, "kt_paydead.pbl", "asnucffamilytype", Mrows);
                    DwUtil.RetrieveDataWindow(DwType, "kt_paydead.pbl", null, null);
                    HdTypeRowCount.Value = Convert.ToString(DwType.RowCount);
                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลเรียบร้อยแล้ว");
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                }
            }
        }

        public void WebSheetLoadEnd()
        {
            DwMain.SaveDataCache();
            DwType.SaveDataCache();
            ta.Close();
        }
        private void JSBtDel()
        {
            int rownum = Convert.ToInt32(HdMainRowDel.Value);
            if (HdMainRowDel.Value != HdMainRowAdd.Value)
            {

                decimal seq_no = DwMain.GetItemDecimal(rownum, "seq_no");
                try
                {
                    string sql = "DELETE asnucffamilydead where seq_no = " + seq_no;
                    ta.Exe(sql);
                    DwUtil.RetrieveDataWindow(DwMain, "kt_paydead.pbl", null, null);
                    HdMainRowCount.Value = Convert.ToString(DwMain.RowCount);
                    LtServerMessage.Text = WebUtil.CompleteMessage("ทำการลบแถวสำเร็จ");
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถทำรายการได้" + ex);
                }
            }
            else
            {
                DwMain.DeleteRow(Convert.ToInt32(HdMainRowAdd.Value));
            }
            HdMainRowAdd.Value = "";
            HdMainRowDel.Value = "";
        }

        private void JSTypeBtDel()
        {
            int rownum = Convert.ToInt32(HdTypeRowDel.Value);
            if (HdTypeRowDel.Value != HdTypeRowAdd.Value)
            {

                decimal familytype_code = DwType.GetItemDecimal(rownum, "familytype_code");
                try
                {
                    string sql = "DELETE asnucffamilytype where familytype_code = " + familytype_code;
                    ta.Exe(sql);
                    DwUtil.RetrieveDataWindow(DwType, "kt_paydead.pbl", null, null);
                    HdTypeRowCount.Value = Convert.ToString(DwType.RowCount);
                    LtServerMessage.Text = WebUtil.CompleteMessage("ทำการลบแถวสำเร็จ");
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถทำรายการได้" + ex);
                }
            }
            else
            {
                DwType.DeleteRow(Convert.ToInt32(HdTypeRowAdd.Value));
            }
            HdTypeRowAdd.Value = "";
            HdTypeRowDel.Value = "";
        }
        public void JSInsertRow()
        {
            DwMain.InsertRow(0);
            HdMainRowAdd.Value = Convert.ToString(DwMain.RowCount);
        }
    }
}