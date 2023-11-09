using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;

namespace Saving.Applications.hr.ws_hr_constant_ucfworktimecode_ctrl
{
    public partial class ws_hr_constant_ucfworktimecode : PageWebSheet, WebSheet
    {
        [JsPostBack]
        public string PostProbitemtype { get; set; }
        [JsPostBack]
        public string PostNewRow { get; set; }
        [JsPostBack]
        public string PostDelRow { get; set; }

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
            if (eventArg == PostNewRow)
            {
                dsList.InsertLastRow();

            }
            else if (eventArg == PostDelRow)
            {
                int row = dsList.GetRowFocus();
                dsList.DeleteRow(row);
            }
        }

        public void SaveWebSheet()
        {
            try
            {
                ExecuteDataSource exe = new ExecuteDataSource(this);
                exe.AddRepeater(dsList);
                int result = exe.Execute();
                dsList.Retrieve();
                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลสำเร็จ (" + result + ")");
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        public void WebSheetLoadEnd()
        {

        }
    }
}