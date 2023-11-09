using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CoreSavingLibrary;
namespace Saving.Applications.hr.ws_hr_constant_ucfbloodtype_ctrl
{
    public partial class ws_hr_constant_ucfbloodtype : PageWebSheet, WebSheet
    {
        [JsPostBack]
        public string PostInsertRow { get; set; }

        [JsPostBack]
        public string PostDeleteRow { get; set; }

        public void InitJsPostBack()
        {
            dsList.InitDsList(this);
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                dsList.Retrieve();
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == PostInsertRow)
            {
                dsList.InsertLastRow();
                int ls_rowcount = dsList.RowCount - 1;
                dsList.FindTextBox(ls_rowcount, "bloodtype_code").Focus();
            }

            else if (eventArg == PostDeleteRow)
            {
                int Rowdelete = dsList.GetRowFocus();
                dsList.DeleteRow(Rowdelete);
            }
        }

        public void SaveWebSheet()
        {
            try
            {
                ExecuteDataSource exed = new ExecuteDataSource(this);
                exed.AddRepeater(dsList);

                int result = exed.Execute();

                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ"); //หากบันทึกให้ขึ้น บันทึกแล้ว
                dsList.Retrieve();
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message); // กรณีไม่สำเร็จ
            }
        }

        public void WebSheetLoadEnd()
        {

        }
    }
}