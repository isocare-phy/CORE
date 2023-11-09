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
    public partial class w_sheet_cmducf_invtadjtype : PageWebSheet, WebSheet
    {
        String pbl = "cmd_ucfinvtadjtype.pbl";
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
            DwDetailout.SetTransaction(sqlca);

            if (!IsPostBack)
            {
                DwMain.InsertRow(0);
                DwDetail.Retrieve();
                DwDetailout.Retrieve();
            }
            else
            {
                this.RestoreContextDw(DwMain);
                this.RestoreContextDw(DwDetail);
                this.RestoreContextDw(DwDetailout);
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
                String adjtype_code = "", adjtype_desc;
                Decimal sign_flag = 0;
                adjtype_desc = DwMain.GetItemString(1, "adjtype_desc").Trim();
                sign_flag = DwMain.GetItemDecimal(1, "sign_flag");
                try
                {
                    String se = @"select max(adjtype_code)as adjtype_code from ptucfinvtadjtype";
                    ta = WebUtil.QuerySdt(se);
                    if (ta.Next())
                    {
                        adjtype_code = ta.GetString("adjtype_code");
                    }
                    if (adjtype_code == null || adjtype_code == "")
                    {
                        adjtype_code = "0";
                    }
                    adjtype_code = Convert.ToString(Convert.ToDecimal(adjtype_code) + 1);

                    while (adjtype_code.Length < 3)
                    {
                        adjtype_code = "0" + adjtype_code;
                    }
                }
                catch { }
                try
                {
                    String insert = @"insert into ptucfinvtadjtype
                                (adjtype_code,adjtype_desc,sign_flag)
                                values('" + adjtype_code + "','" + adjtype_desc + "'," + sign_flag + " )";
                    ta = WebUtil.QuerySdt(insert);
                    DwDetail.Retrieve();
                    DwDetailout.Retrieve();
                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
                    DwMain.SetItemString(1, "adjtype_desc",null);

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
            DwDetailout.SaveDataCache();
        }

        private void PostOnDelete()
        {
            String AdjtypeCode = "";
            Int32 row = Convert.ToInt32(HdR.Value);
            try
            {
                AdjtypeCode = DwDetail.GetItemString(row, "adjtype_code");
                String del = @"delete ptucfinvtadjtype where adjtype_code = '" + AdjtypeCode + "'";
                ta = WebUtil.QuerySdt(del);
                DwDetail.Retrieve();
                LtServerMessage.Text = WebUtil.CompleteMessage("ทำการลบรายการ " + AdjtypeCode + " สำเร็จ");
            }
            catch (Exception ex)
            { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }

            try
            {
                AdjtypeCode = DwDetailout.GetItemString(row, "adjtype_code");
                String del = @"delete ptucfinvtadjtype where adjtype_code = '" + AdjtypeCode + "'";
                ta = WebUtil.QuerySdt(del);
                DwDetailout.Retrieve();
                LtServerMessage.Text = WebUtil.CompleteMessage("ทำการลบรายการ " + AdjtypeCode + " สำเร็จ");
            }
            catch (Exception ex)
            { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }

        }
    }
}