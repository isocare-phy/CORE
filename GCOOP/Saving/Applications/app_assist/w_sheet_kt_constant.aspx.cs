using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataLibrary;
using System.Data;

namespace Saving.Applications.app_assist
{
    public partial class w_sheet_kt_constant : PageWebSheet, WebSheet
    {
        protected String jsBtDel;
        protected String jsInsertRow;

        protected Sta ta;
        public void InitJsPostBack()
        {
            jsBtDel = WebUtil.JsPostBack(this, "jsBtDel");
            jsInsertRow = WebUtil.JsPostBack(this, "jsInsertRow");
        }

        public void WebSheetLoadBegin()
        {
            this.ConnectSQLCA();
            ta = new Sta(sqlca.ConnectionString);

            if (!IsPostBack)
            {
                DwUtil.RetrieveDataWindow(DwMain, "kt_50bath.pbl", null, null);
                HdRowCount.Value = Convert.ToString(DwMain.RowCount);
            }
            else
            {                
                this.RestoreContextDw(DwMain);
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
            String sql = "select count(*) as ktmax from asnucfpayktdead";
            DataTable dt = WebUtil.Query(sql);
            int rowdb = Convert.ToInt32(dt.Rows[0]["ktmax"]);
            int rowCount = DwMain.RowCount;
            if (rowCount == rowdb)
            {
                try
                {
                    DwUtil.UpdateDataWindow(DwMain, "kt_50bath.pbl", "asnucfpayktdead");
                    DwUtil.RetrieveDataWindow(DwMain, "kt_50bath.pbl", null, null);
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
                String mql = "select max(seq_no) as sqmax from asnucfpayktdead";
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
                    DwUtil.InsertDataWindow(DwMain, "kt_50bath.pbl", "asnucfpayktdead", rows);
                    DwUtil.UpdateDataWindow(DwMain, "kt_50bath.pbl", "asnucfpayktdead");
                    DwUtil.RetrieveDataWindow(DwMain, "kt_50bath.pbl", null, null);
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
                    string sql = "DELETE asnucfpayktdead where seq_no = " + seq_no;
                    ta.Exe(sql);
                    DwUtil.RetrieveDataWindow(DwMain, "kt_50bath.pbl", null, null);
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