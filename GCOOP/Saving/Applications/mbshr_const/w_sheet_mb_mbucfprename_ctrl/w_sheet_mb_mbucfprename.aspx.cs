using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace Saving.Applications.mbshr_const.w_sheet_mb_mbucfprename_ctrl
{
    public partial class w_sheet_mb_mbucfprename : PageWebSheet, WebSheet
    {
        [JsPostBack]
        public string PostInsertRow { get; set; }
        [JsPostBack]
        public string PostDeleteRow { get; set; }

        public void InitJsPostBack()
        {
            dsMain.InitDsMain(this);
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                dsMain.Retrieve();
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == PostInsertRow)
            {
                dsMain.InsertLastRow();
                int ls_rowcount = dsMain.RowCount - 1;
                dsMain.FindTextBox(ls_rowcount, "prename_code").Focus();
            }
            else if (eventArg == PostDeleteRow)
            {
                int rowDel = dsMain.GetRowFocus();
                dsMain.DeleteRow(rowDel);
            }
        }

        public void SaveWebSheet()
        {
            try
            {
                ExecuteDataSource exed = new ExecuteDataSource(this);
                exed.AddRepeater(dsMain);

                int result = exed.Execute();
                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
                dsMain.ResetRow();
                dsMain.Retrieve();
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
            }
        }

        public void WebSheetLoadEnd()
        {
        }
    }
}