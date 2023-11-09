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
    public partial class w_sheet_cmducf_durtrcvtype : PageWebSheet, WebSheet
    {
        String pbl = "cmd_ucfdurtrcvtype.pbl";
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
                String durtrcv_code = "", durtrcv_desc;

                durtrcv_desc = DwMain.GetItemString(1, "durtrcv_desc").Trim();
                try
                {
                    String se = @"select max(durtrcv_code)as durtrcv_code from ptucfdurtrcvtype";
                    ta = WebUtil.QuerySdt(se);
                    if (ta.Next())
                    {
                        durtrcv_code = ta.GetString("durtrcv_code");
                    }
                    if (durtrcv_code == null || durtrcv_code == "")
                    {
                        durtrcv_code = "0";
                    }
                    durtrcv_code = Convert.ToString(Convert.ToDecimal(durtrcv_code) + 1);

                    while (durtrcv_code.Length < 2)
                    {
                        durtrcv_code = "0" + durtrcv_code;
                    }
                }
                catch { }
                try
                {
                    String insert = @"insert into ptucfdurtrcvtype (durtrcv_code,durtrcv_desc)
                                values( {0}, {1})";
                    insert = WebUtil.SQLFormat(insert, durtrcv_code, durtrcv_desc);
                    ta = WebUtil.QuerySdt(insert);
                    DwDetail.Retrieve();
                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
                    DwMain.SetItemString(1, "durtrcv_desc",null);

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
            String durtrcvCode = "";
            Int32 row = Convert.ToInt32(HdR.Value);
            try
            {
                durtrcvCode = DwDetail.GetItemString(row, "durtrcv_code");
                String del = @"delete ptucfdurtrcvtype where durtrcv_code = {0}";
                del = WebUtil.SQLFormat(del, durtrcvCode);
                ta = WebUtil.QuerySdt(del);
                DwDetail.Retrieve();
                LtServerMessage.Text = WebUtil.CompleteMessage("ทำการลบรายการ " + durtrcvCode + " สำเร็จ");
            }
            catch (Exception ex)
            { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }

        }
        
    }
}