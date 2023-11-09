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
    public partial class w_sheet_cmducf_suppliesgroup : PageWebSheet, WebSheet
    {
        String pbl = "cmd_ucfsuppliesgroup.pbl";
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
                String invtgrp_code = "", invtgrp_desc;

                invtgrp_desc = DwMain.GetItemString(1, "invtgrp_desc").Trim();
                try
                {
                    String se = @"select max(invtgrp_code)as invtgrp_code from ptucfinvtgroupcode";
                    ta = WebUtil.QuerySdt(se);
                    if (ta.Next())
                    {
                        invtgrp_code = ta.GetString("invtgrp_code");
                    }
                    if (invtgrp_code == null || invtgrp_code == "")
                    {
                        invtgrp_code = "0";
                    }
                    invtgrp_code = Convert.ToString(Convert.ToDecimal(invtgrp_code) + 1);

                    while (invtgrp_code.Length < 3)
                    {
                        invtgrp_code = "0" + invtgrp_code;
                    }
                }
                catch { }
                try
                {
                    String insert = @"insert into ptucfinvtgroupcode
                                (invtgrp_code,invtgrp_desc)
                                values('" + invtgrp_code + "','" + invtgrp_desc + "' )";
                    ta = WebUtil.QuerySdt(insert);
                    DwDetail.Retrieve();
                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
                    DwMain.SetItemString(1, "invtgrp_desc", null);

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
            String InvtgrpCode = "";
            Int32 row = Convert.ToInt32(HdR.Value);
            try
            {
                InvtgrpCode = DwDetail.GetItemString(row, "invtgrp_code");
                String del = @"delete ptucfinvtgroupcode where invtgrp_code = '"+InvtgrpCode+"'";
                ta = WebUtil.QuerySdt(del);
                DwDetail.Retrieve();
                LtServerMessage.Text = WebUtil.CompleteMessage("ทำการลบรายการ " + InvtgrpCode + " สำเร็จ");
            }
            catch (Exception ex)
            { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }

        }

    }
}