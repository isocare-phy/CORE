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
using System.Data.OracleClient;
using Sybase.DataWindow;

namespace Saving.Applications.cmd
{
    public partial class w_sheet_cmducf_takereasoncode : PageWebSheet, WebSheet
    {
        String pbl = "cmd_ucftakereasoncode.pbl";
        Sdt ta;
        protected String jsPostDelete;

        public void InitJsPostBack()
        {
            jsPostDelete = WebUtil.JsPostBack(this, "jsPostDelete");
        }

        public void WebSheetLoadBegin()
        {
            this.ConnectSQLCA();
            DwDetail.SetTransaction(sqlca);

            if (!IsPostBack)
            {
                DwMain.InsertRow(0);
                DwDetail.Retrieve();
            }
            else
            {
                this.RestoreContextDw(DwMain);
                this.RestoreContextDw(DwDetail);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            switch (eventArg)
            {
                case "jsPostDelete":
                    PostOnDelete();
                    break;

            }
        }

        public void SaveWebSheet()
        {
            try
            {
                String takereason_code = "", takereason_desc;

                takereason_desc = DwMain.GetItemString(1, "takereason_desc").Trim();
                try
                {
                    String se = @"select max(takereason_code)as takereason_code from ptucftakereasoncode";
                    ta = WebUtil.QuerySdt(se);
                    if (ta.Next())
                    {
                        takereason_code = ta.GetString("takereason_code");
                    }
                    if (takereason_code == null || takereason_code == "")
                    {
                        takereason_code = "0";
                    }
                    takereason_code = Convert.ToString(Convert.ToDecimal(takereason_code) + 1);

                    while (takereason_code.Length < 2)
                    {
                        takereason_code = "0" + takereason_code;
                    }
                }
                catch { }
                try
                {
                    String insert = @"insert into ptucftakereasoncode
                                (takereason_code,takereason_desc)
                                values('" + takereason_code + "','" + takereason_desc + "' )";
                    ta = WebUtil.QuerySdt(insert);
                    DwDetail.Retrieve();
                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
                    DwMain.SetItemString(1, "takereason_desc",null);

                }
                catch { }
            }
            catch (Exception ex)
            { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }

        }

        public void WebSheetLoadEnd()
        {
            DwMain.SaveDataCache();
            DwDetail.SaveDataCache();
        }


        private void PostOnDelete()
        {
            String TakereasonCode = "";
            Int32 row = Convert.ToInt32(HdR.Value);
            try
            {
                TakereasonCode = DwDetail.GetItemString(row, "takereason_code");
                String del = @"delete ptucftakereasoncode where takereason_code = '" + TakereasonCode + "'";
                ta = WebUtil.QuerySdt(del);
                DwDetail.Retrieve();
                LtServerMessage.Text = WebUtil.CompleteMessage("ทำการลบรายการ " + TakereasonCode + " สำเร็จ");
            }
            catch (Exception ex)
            { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }

        }

    }
}