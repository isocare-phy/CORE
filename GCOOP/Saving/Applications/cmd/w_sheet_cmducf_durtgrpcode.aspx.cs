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
    public partial class w_sheet_cmducf_durtgrpcode : PageWebSheet, WebSheet
    {
        string pbl = "cmd_ucfdurtgrpcode.pbl";
        Sdt ta;
        protected String jsPostDelete;
        protected String jsPostSetData;

        public void InitJsPostBack()
        {
            jsPostDelete = WebUtil.JsPostBack(this, "jsPostDelete");
            jsPostSetData = WebUtil.JsPostBack(this, "jsPostSetData");
        }

        public void WebSheetLoadBegin()
        {
            this.ConnectSQLCA();
            DwDetail.SetTransaction(sqlca);

            if (!IsPostBack)
            {
                DwMain.InsertRow(0);
                DwMain.SetItemDecimal(1, "devalue_percent", 0);
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
                case "jsPostSetData":
                    PostSetData();
                    break;
            }
        }

        public void SaveWebSheet()
        {
            try
            {
                String durtgrp_code = "", durtgrp_desc = "", durtgrp_abb = "";
                Decimal devalue_percent = 0 ;
                if (HdStatus.Value == "Update")
                {
                    durtgrp_code = DwMain.GetItemString(1, "durtgrp_code").Trim();
                    durtgrp_desc = DwMain.GetItemString(1, "durtgrp_desc").Trim();
                    try { durtgrp_abb = DwMain.GetItemString(1, "durtgrp_abb").Trim(); }
                    catch { durtgrp_abb = ""; }
                    devalue_percent = DwMain.GetItemDecimal(1, "devalue_percent");
                    String up = @"update ptucfdurtgrpcode set durtgrp_desc = {0}, durtgrp_abb = {1}, 
                    devalue_percent = {2} where durtgrp_code = {3}";
                    up = WebUtil.SQLFormat(up, durtgrp_desc, durtgrp_abb, devalue_percent, durtgrp_code);
                    ta = WebUtil.QuerySdt(up);
                    LtServerMessage.Text = WebUtil.CompleteMessage("อัดเดทข้อมูลกลุ่มครุภัณฑ์ " + durtgrp_code + " สำเร็จ");
                    HdStatus.Value = null;
                    DwMain.Reset();
                    DwMain.InsertRow(0);
                    DwDetail.Retrieve();
                }
                else
                {
                    durtgrp_desc = DwMain.GetItemString(1, "durtgrp_desc").Trim();
                    try { durtgrp_abb = DwMain.GetItemString(1, "durtgrp_abb").Trim(); }
                    catch { durtgrp_abb = ""; }
                    try { devalue_percent = DwMain.GetItemDecimal(1, "devalue_percent"); }
                    catch { devalue_percent = 0; }
                    try
                    {
                        String se = @"select max(durtgrp_code)as durtgrp_code from ptucfdurtgrpcode";
                        ta = WebUtil.QuerySdt(se);
                        if (ta.Next())
                        {
                            durtgrp_code = ta.GetString("durtgrp_code");
                        }
                        if (durtgrp_code == null || durtgrp_code == "")
                        {
                            durtgrp_code = "0";
                        }
                        durtgrp_code = Convert.ToString(Convert.ToDecimal(durtgrp_code) + 1);

                        while (durtgrp_code.Length < 3)
                        {
                            durtgrp_code = "0" + durtgrp_code;
                        }
                    }
                    catch { }
                    try
                    {
                        String insert = @"insert into ptucfdurtgrpcode (durtgrp_code,durtgrp_desc, devalue_percent, durtgrp_abb)
                                values({0}, {1}, {2}, {3})";
                        insert = WebUtil.SQLFormat(insert, durtgrp_code, durtgrp_desc, devalue_percent, durtgrp_abb);
                        ta = WebUtil.QuerySdt(insert);
                        
                        LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
                        DwMain.Reset();
                        DwMain.InsertRow(0);
                        HdStatus.Value = null;
                        DwMain.SetItemString(1, "durtgrp_abb", null);
                        DwMain.SetItemString(1, "durtgrp_desc", null);
                        DwMain.SetItemDecimal(1, "devalue_percent", 0);
                        DwDetail.Retrieve();

                    }
                    catch { }
                }
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
            String durtgrpCode = "";
            Int32 row = Convert.ToInt32(HdR.Value);
            try
            {
                durtgrpCode = DwDetail.GetItemString(row, "durtgrp_code");
                String del = @"delete ptucfdurtgrpcode where durtgrp_code = {0}";
                del = WebUtil.SQLFormat(del, durtgrpCode);
                ta = WebUtil.QuerySdt(del);
                DwDetail.Retrieve();
                LtServerMessage.Text = WebUtil.CompleteMessage("ทำการลบรายการ " + durtgrpCode + " สำเร็จ");
            }
            catch (Exception ex)
            { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }

        }

        private void PostSetData()
        {
            Int32 rowse = Convert.ToInt32(HdR.Value);
            Decimal devalue_percent = 0;
            String durtgrp_code = "", durtgrp_desc = "", durtgrp_abb = "";
            durtgrp_code = DwDetail.GetItemString(rowse, "durtgrp_code").Trim();
            durtgrp_desc = DwDetail.GetItemString(rowse, "durtgrp_desc").Trim();
            try { durtgrp_abb = DwDetail.GetItemString(rowse, "durtgrp_abb").Trim(); }
            catch { durtgrp_abb = ""; }
            try { devalue_percent = DwDetail.GetItemDecimal(rowse, "devalue_percent"); }
            catch { devalue_percent = 0; }
            DwMain.SetItemString(1, "durtgrp_code", durtgrp_code);
            DwMain.SetItemString(1, "durtgrp_desc", durtgrp_desc);
            DwMain.SetItemString(1, "durtgrp_abb", durtgrp_abb);
            DwMain.SetItemDecimal(1, "devalue_percent", devalue_percent);
            HdStatus.Value = "Update";
        }
    }
}