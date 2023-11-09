using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Saving.Applications.mis.w_sheet_pm_estsumday_ctrl
{
    public partial class w_sheet_pm_estsumday : PageWebSheet, WebSheet
    {
        [JsPostBack]
        public string PostInsertRow { get; set; } //ไว้สำหลับเติมแถวเข้าไป

        [JsPostBack]
        public string PostdeleteRow { get; set; } //ไว้สำหลับลบแถวออก

        [JsPostBack]
        public string Postretrivegropdown { get; set; } 

        public void InitJsPostBack()
        {
            dsMain.InitDsMain(this);
            dsList.InitDsList(this);
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                dsMain.retrieveddp();
                dsList.retrieve("");
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == PostInsertRow)
            {
                dsList.InsertAtRow(0);
                dsList.DATA[0].COOP_ID = state.SsCoopControl;
            }

            else if (eventArg == PostdeleteRow)
            {
                int Rowdelete = dsList.GetRowFocus();
                dsList.DeleteRow(Rowdelete);
            }
            else if (eventArg == Postretrivegropdown)
            {
                string investype_code = dsMain.DATA[0].dropdown;
                dsList.retrieve(investype_code);
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