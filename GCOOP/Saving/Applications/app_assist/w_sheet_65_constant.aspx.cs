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
    public partial class w_sheet_65_constant : PageWebSheet, WebSheet
    {
        protected String jsBtDel;
        protected String jsInsertRow;
        protected DwThDate tDwPercent;

        protected Sta ta;
        public void InitJsPostBack()
        {
            jsBtDel = WebUtil.JsPostBack(this, "jsBtDel");
            jsInsertRow = WebUtil.JsPostBack(this, "jsInsertRow");
            tDwPercent = new DwThDate(DwPercent, this);
            tDwPercent.Add("edit_time", "edit_ttime");
        }

        public void WebSheetLoadBegin()
        {
            this.ConnectSQLCA();
            ta = new Sta(sqlca.ConnectionString);

            if (!IsPostBack)
            {
                DwUtil.RetrieveDataWindow(DwMain, "kt_65years.pbl", null, null);
                HdRowCount.Value = Convert.ToString(DwMain.RowCount);
                DwUtil.RetrieveDataWindow(DwPercent, "kt_65years.pbl", null, null);
                
            }
            else
            {
                this.RestoreContextDw(DwMain);
                this.RestoreContextDw(DwPercent);
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
            }
        }

        public void SaveWebSheet()
        {
            try // บันทึกการวันของการแก้ไขข้อมูล
            {
                string edit_ttime = DwPercent.GetItemString(1, "edit_ttime");
                DateTime eDate = DateTime.ParseExact(edit_ttime, "ddMMyyyy", WebUtil.TH);
                //edit_ttime = edit_ttime.Substring(2, 2) + edit_ttime.Substring(0, 2) + (Convert.ToInt32(edit_ttime.Substring(4, 4)) - 543).ToString(); // แปลงวันที่ จาก ddmmyyyy ให้เป็น mmddyyyy
                //edit_ttime = edit_ttime.Insert(2, "/");
                //edit_ttime = edit_ttime.Insert(5, "/");
                DwPercent.SetItemDateTime(1, "edit_time", Convert.ToDateTime(eDate)); 
            }
            catch { }
            String sql = "select count(*) as ktmax from asnucfpay65";
            DataTable dt = WebUtil.Query(sql);
            int rowdb = Convert.ToInt32(dt.Rows[0]["ktmax"]);
            int rowCount = DwMain.RowCount;
            if (rowCount == rowdb)
            {
                try
                {
                    DwUtil.UpdateDateWindow(DwMain, "kt_65years.pbl", "asnucfpay65");
                    DwUtil.RetrieveDataWindow(DwMain, "kt_65years.pbl", null, null);
                    DwUtil.UpdateDateWindow(DwPercent, "kt_65years.pbl", "asnucfperpay");
                    DwUtil.RetrieveDataWindow(DwPercent, "kt_65years.pbl", null, null);
                    HdRowCount.Value = Convert.ToString(DwMain.RowCount);
                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถทำรายการได้" + ex);
                }
            }
            else
            {
                String mql = "select max(seq_no) as sqmax from asnucfpay65";
                DataTable dt2 = WebUtil.Query(mql);
                int seqno = Convert.ToInt32(dt2.Rows[0]["sqmax"]);
                int j = 1;
                try
                {
                    int rowFirst = int.Parse(HdRowCount.Value);
                    int[] rows = new int[rowCount - rowFirst];
                    int ii = 0;
                    for (int i = rowFirst; i < rowCount; i++)
                    {
                        DwMain.SetItemDecimal(i + 1, "seq_no", seqno + j);
                        rows[ii] = i + 1;
                        ii++;
                        j++;
                    }
                    DwUtil.InsertDataWindow(DwMain, "kt_65years.pbl", "asnucfpay65", rows);
                    DwUtil.UpdateDateWindow(DwMain, "kt_65years.pbl", "asnucfpay65");
                    DwUtil.RetrieveDataWindow(DwMain, "kt_65years.pbl", null, null);
                    DwUtil.UpdateDateWindow(DwPercent, "kt_65years.pbl", "asnucfperpay");
                    DwUtil.RetrieveDataWindow(DwPercent, "kt_65years.pbl", null, null);
                    HdRowCount.Value = Convert.ToString(DwMain.RowCount);
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
            DwPercent.SaveDataCache();
            tDwPercent.Eng2ThaiAllRow();
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
                    string sql = "DELETE asnucfpay65 where seq_no = " + seq_no;
                    ta.Exe(sql);
                    DwUtil.RetrieveDataWindow(DwMain, "kt_65years.pbl", null, null);
                    HdRowCount.Value = Convert.ToString(DwMain.RowCount);
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

        public void JSInsertRow()
        {
            DwMain.InsertRow(0);
            HdMainRowAdd.Value = Convert.ToString(DwMain.RowCount);
        }
    }
}