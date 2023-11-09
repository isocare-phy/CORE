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
    public partial class w_sheet_cmducf_durtgrpsubcode : PageWebSheet, WebSheet
    {
        String pbl = "cmd_ucfdurtgrpsubcode.pbl";
        Sdt ta;
        protected String jsPostDelete;
        protected String jsPostSetData;
        protected String jsPostRetriveDurtgrpsub;

        public void InitJsPostBack()
        {
            jsPostDelete = WebUtil.JsPostBack(this, "jsPostDelete");
            jsPostSetData = WebUtil.JsPostBack(this, "jsPostSetData");
            jsPostRetriveDurtgrpsub = WebUtil.JsPostBack(this, "jsPostRetriveDurtgrpsub");
        }

        public void WebSheetLoadBegin()
        {
            this.ConnectSQLCA();
            DwDetail.SetTransaction(sqlca);

            if (!IsPostBack)
            {
                DwMain.InsertRow(0);
                DwMain.SetItemDecimal(1, "devalue_percent", 0);
                DwUtil.RetrieveDDDW(DwMain, "durtgrp_code", pbl, null);
                //DwDetail.Retrieve();
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
                case "jsPostRetriveDurtgrpsub":
                    PostRetriveDurtgrpsub();
                    break;
            }
        }

        public void SaveWebSheet()
        {
            try
            {
                String durtgrp_code = "", durtgrpsub_code = "", durtgrpsub_desc = "", durtgrpsub_abb = "";
                Decimal devalue_percent = 0 ;
                if (HdStatus.Value == "Update")
                {
                    durtgrp_code = DwMain.GetItemString(1, "durtgrp_code").Trim();
                    durtgrpsub_code = DwMain.GetItemString(1, "durtgrpsub_code").Trim();
                    durtgrpsub_desc = DwMain.GetItemString(1, "durtgrpsub_desc").Trim();                    
                    try { durtgrpsub_abb = DwMain.GetItemString(1, "durtgrpsub_abb").Trim(); }
                    catch { durtgrpsub_abb = ""; }
                    devalue_percent = DwMain.GetItemDecimal(1, "devalue_percent");
                    String up = @"update ptucfdurtgrpsubcode set durtgrpsub_desc = {0}, durtgrpsub_abb = {1}, 
                    devalue_percent = {2} where durtgrp_code = {3} and durtgrpsub_code = {4}";
                    up = WebUtil.SQLFormat(up, durtgrpsub_desc, durtgrpsub_abb, devalue_percent, durtgrp_code, durtgrpsub_code);
                    ta = WebUtil.QuerySdt(up);
                    LtServerMessage.Text = WebUtil.CompleteMessage("อัดเดทข้อมูลกลุ่มครุภัณฑ์ " + durtgrp_code + ", " + durtgrpsub_code + " สำเร็จ");
                    HdStatus.Value = null;
                    DwMain.SetItemString(1, "durtgrpsub_code", null);
                    DwMain.SetItemString(1, "durtgrpsub_abb", null);
                    DwMain.SetItemString(1, "durtgrpsub_desc", null);
                    DwMain.SetItemDecimal(1, "devalue_percent", 0);
                    DwUtil.RetrieveDataWindow(DwDetail, pbl, null, durtgrp_code);
                    DwMain.SetItemString(1, "durtgrp_code", durtgrp_code);
                    PostRetriveDurtgrpsub();
                }
                else
                {
                    durtgrp_code = DwMain.GetItemString(1, "durtgrp_code").Trim();
                    durtgrpsub_desc = DwMain.GetItemString(1, "durtgrpsub_desc").Trim();
                    try { durtgrpsub_abb = DwMain.GetItemString(1, "durtgrpsub_abb").Trim(); }
                    catch { durtgrpsub_abb = ""; }
                    try { devalue_percent = DwMain.GetItemDecimal(1, "devalue_percent"); }
                    catch { devalue_percent = 0; }
                    durtgrpsub_code = GetMaxDurtGrpSubCode(durtgrp_code);

                    String insert = @"insert into ptucfdurtgrpsubcode
                            (durtgrp_code, durtgrpsub_code, durtgrpsub_desc, devalue_percent, durtgrpsub_abb)
                            values( {0}, {1}, {2}, {3}, {4})";
                    insert = WebUtil.SQLFormat(insert, durtgrp_code, durtgrpsub_code, durtgrpsub_desc, devalue_percent, durtgrpsub_abb);
                    ta = WebUtil.QuerySdt(insert);
                    ///เพิ่ม doccontrol ตาม สาขาก กลุ่มครุภัณฑ์ กลุ่มย่อยครุภัณฑ์
                    if (state.SsCoopControl == "000000" || state.SsCoopControl == "022001")
                    {
                        decimal year = WebUtil.GetAccyear(state.SsCoopControl, state.SsWorkDate) + 1;
                        string insertdoc = @"insert into cmdocumentcontrol (coop_id, document_code, 
                                document_name, last_documentno, document_length, document_format, document_year,
                                clear_type, system_code )
                                select {0}, 'CMDDURTRUN' || pb.branch_code||pd.durtgrp_code||pd.durtgrpsub_code,
                                'เลขป้ายทะเบียนครุภัณฑ์', 0, 6, 'YYRRRR', {3}, 1, 'CMD'
                                from ptucfbranchcode pb, ptucfdurtgrpsubcode pd
                                where pd.durtgrp_code = {1}
                                and pd.durtgrpsub_code = {2}";
                        insertdoc = WebUtil.SQLFormat(insertdoc, state.SsCoopControl, durtgrp_code, durtgrpsub_code, year);
                        Sdt ta2 = WebUtil.QuerySdt(insertdoc);
                    }
                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
                    HdStatus.Value = null;
                    DwMain.SetItemString(1, "durtgrpsub_code", null);
                    DwMain.SetItemString(1, "durtgrpsub_abb", null);
                    DwMain.SetItemString(1, "durtgrpsub_desc", null);
                    DwMain.SetItemDecimal(1, "devalue_percent", 0);
                    DwUtil.RetrieveDataWindow(DwDetail, pbl, null, durtgrp_code);
                    DwMain.SetItemString(1, "durtgrp_code", durtgrp_code);
                    PostRetriveDurtgrpsub();
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
            String durtgrpCode = "", durtgrpsubCode = "";
            Int32 row = Convert.ToInt32(HdR.Value);
            try
            {
                durtgrpCode = DwDetail.GetItemString(row, "durtgrp_code").Trim();
                durtgrpsubCode = DwDetail.GetItemString(row, "durtgrpsub_code").Trim();
                String del = @"delete ptucfdurtgrpsubcode where durtgrp_code = {0} and durtgrpsub_code = {1}";
                del = WebUtil.SQLFormat(del, durtgrpCode, durtgrpsubCode);
                ta = WebUtil.QuerySdt(del);
                DwDetail.Retrieve();
                LtServerMessage.Text = WebUtil.CompleteMessage("ทำการลบรายการ " + durtgrpCode +", " + durtgrpsubCode + " สำเร็จ");
            }
            catch (Exception ex)
            { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }

        }

        private void PostSetData()
        {
            Int32 rowse = Convert.ToInt32(HdR.Value);
            Decimal devalue_percent = 0;
            String durtgrpsub_code = "", durtgrpsub_desc = "", durtgrpsub_abb = "";
            durtgrpsub_code = DwDetail.GetItemString(rowse, "durtgrpsub_code").Trim();
            durtgrpsub_desc = DwDetail.GetItemString(rowse, "durtgrpsub_desc").Trim();
            try { durtgrpsub_abb = DwDetail.GetItemString(rowse, "durtgrpsub_abb").Trim(); }
            catch { durtgrpsub_abb = ""; }
            try { devalue_percent = DwDetail.GetItemDecimal(rowse, "devalue_percent"); }
            catch { devalue_percent = 0; }
            DwMain.SetItemString(1, "durtgrpsub_code", durtgrpsub_code);
            DwMain.SetItemString(1, "durtgrpsub_desc", durtgrpsub_desc);
            DwMain.SetItemString(1, "durtgrpsub_abb", durtgrpsub_abb);
            DwMain.SetItemDecimal(1, "devalue_percent", devalue_percent);
            HdStatus.Value = "Update";
        }

        private void PostRetriveDurtgrpsub()
        {
            Decimal devalue_percent = 0;
            String durtgrp_code = DwMain.GetItemString(1, "durtgrp_code").Trim();
            String se = "select devalue_percent from ptucfdurtgrpcode where durtgrp_code = {0}";
            se = WebUtil.SQLFormat(se, durtgrp_code);
            ta = WebUtil.QuerySdt(se);
            if (ta.Next())
            {
                devalue_percent = ta.GetDecimal("devalue_percent");
            }
            DwMain.SetItemDecimal(1, "devalue_percent", devalue_percent);
            DwUtil.RetrieveDataWindow(DwDetail, pbl, null, durtgrp_code);
        }

        private string GetMaxDurtGrpSubCode(string durtgrp_code)
        {
            string durtgrpsub_code = "";
            try
            {
                String se = @"select max(durtgrpsub_code)as durtgrpsub_code from ptucfdurtgrpsubcode where durtgrp_code = {0}";
                se = WebUtil.SQLFormat(se, durtgrp_code);
                ta = WebUtil.QuerySdt(se);
                if (ta.Next())
                {
                    durtgrpsub_code = ta.GetString("durtgrpsub_code");
                }
                if (durtgrpsub_code == null || durtgrpsub_code == "")
                {
                    durtgrpsub_code = "0";
                }
                durtgrpsub_code = (Convert.ToDecimal(durtgrpsub_code) + 1).ToString("000");
            }
            catch { }
            return durtgrpsub_code;
        }
    }
}