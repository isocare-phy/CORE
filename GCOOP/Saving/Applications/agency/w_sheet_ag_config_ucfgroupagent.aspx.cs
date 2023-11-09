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
using Sybase.DataWindow;


namespace Saving.Applications.agency
{
    public partial class w_sheet_ag_config_ucfgroupagent : PageWebSheet,WebSheet 
    {

        protected String postInsertRow;
        protected String postDeleteRow;


        private void JspostInsertRow()
        {
            Dw_main.InsertRow(0);
        }

        private void JspostDeleteRow()
        {
            Int16 RowDetail = Convert.ToInt16(Hd_row.Value);
            Dw_main.DeleteRow(RowDetail);
            Dw_main.UpdateData();
            LtServerMessage.Text = WebUtil.CompleteMessage("ลบข้อมูลเสร็จเรียบร้อยแล้ว");
        }


        #region WebSheet Members

        public void InitJsPostBack()
        {
            postInsertRow = WebUtil.JsPostBack(this, "postInsertRow");
            postDeleteRow = WebUtil.JsPostBack(this, "postDeleteRow");
        }

        public void WebSheetLoadBegin()
        {
            try
            {
                this.ConnectSQLCA();
            }
            catch { LtServerMessage.Text = WebUtil.CompleteMessage("ติดต่อ Database ไม่ได้"); }

            Dw_main.SetTransaction(sqlca);

            if (!IsPostBack)
            {
                Dw_main.Retrieve();
            }
            else
            {
                this.RestoreContextDw(Dw_main);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postInsertRow")
            {
                JspostInsertRow();
            }
            else if (eventArg == "postDeleteRow")
            {
                JspostDeleteRow();
            }
        }

        public void SaveWebSheet()
        {
            try
            {
                int li_row = Dw_main.FindRow("isnull( agentgrp_code )or agentgrp_code = '' or isnull( agentgrp_desc )or agentgrp_desc = '' ", 0, Dw_main.RowCount);
                if (li_row > 0)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอกข้อมูลให้ครบ");
                }
                else
                {

                    Dw_main.UpdateData();
                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลเสร็จเรียบร้อยแล้ว");
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
            }
        }

        public void WebSheetLoadEnd()
        {
            Dw_main.SaveDataCache();
        }

        #endregion
    }
}
