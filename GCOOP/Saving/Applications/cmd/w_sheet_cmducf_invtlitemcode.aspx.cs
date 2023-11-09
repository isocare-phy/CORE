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
    public partial class w_sheet_cmducf_invtlitemcode : PageWebSheet, WebSheet
    {
        String pbl = "cmd_ucfinvtlitemcode.pbl";
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
                String item_code = "", item_desc="";
                Decimal sing_flag = 0 ;

                item_desc = DwMain.GetItemString(1, "item_des").Trim();
                sing_flag = DwMain.GetItemDecimal(1, "sign_flag");
                try
                {
                    String se = @"select max(item_code)as item_code from ptucfinvtlitemcode";
                    ta = WebUtil.QuerySdt(se);
                    if (ta.Next())
                    {
                        item_code = ta.GetString("item_code");
                    }
                    if (item_code == null || item_code == "")
                    {
                        item_code = "0";
                    }
                    item_code = Convert.ToString(Convert.ToDecimal(item_code) + 1);

                    while (item_code.Length < 3)
                    {
                        item_code = "0" + item_code;
                    }
                }
                catch { }
                try
                {
                    String insert = @"insert into ptucfinvtlitemcode
                                (item_code,item_des,sign_flag)
                                values('" + item_code + "','" + item_desc + "'," + sing_flag + " )";
                    ta = WebUtil.QuerySdt(insert);
                    DwDetail.Retrieve();
                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
                    DwMain.SetItemString(1, "item_des", null);

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
            String ItemCode = "";
            Int32 row = Convert.ToInt32(HdR.Value);
            try
            {
                ItemCode = DwDetail.GetItemString(row, "item_code");
                String del = @"delete ptucfinvtlitemcode where item_code = '" + ItemCode + "'";
                ta = WebUtil.QuerySdt(del);
                DwDetail.Retrieve();
                LtServerMessage.Text = WebUtil.CompleteMessage("ทำการลบรายการ " + ItemCode + " สำเร็จ");
            }
            catch (Exception ex)
            { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }

        }

    }
}