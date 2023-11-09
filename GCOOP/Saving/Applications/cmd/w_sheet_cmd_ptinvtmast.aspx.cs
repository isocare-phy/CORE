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
using CoreSavingLibrary.WcfNCommon;

namespace Saving.Applications.cmd
{
    public partial class w_sheet_cmd_ptinvtmast : PageWebSheet, WebSheet
    {
        protected String postFindShow;
        protected String jsPostOnInsert;
        protected String jsPostInvtGrp;
        protected String jsPostDealer;

        Sdt dt = new Sdt();
        String pbl = "cmd_ptinvtmast.pbl";
    
        public void InitJsPostBack()
        {
            jsPostOnInsert = WebUtil.JsPostBack(this, "jsPostOnInsert");
            postFindShow = WebUtil.JsPostBack(this, "postFindShow");
            jsPostInvtGrp = WebUtil.JsPostBack(this, "jsPostInvtGrp");
            jsPostDealer = WebUtil.JsPostBack(this, "jsPostDealer");
        }

        public void WebSheetLoadBegin()
        {
            this.ConnectSQLCA();
            DwMain.SetTransaction(sqlca);
            DwDetail.SetTransaction(sqlca);

            if (!IsPostBack)
            {
                DwMain.InsertRow(0);
                DwMain.SetItemString(1, "invtcut_type", "FI"); //กำหนดค่าเริ่มต้นเป็นแบบ FIFO
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
                case "jsPostOnInsert":
                    PostOnInsert();
                    break;
                case "postFindShow":
                    jsPostFindShow();
                    break;
                case "jsPostInvtGrp":
                    PostInvtGrp();
                    break;
                case "jsPostDealer":
                    PostDealer();
                    break;
            }
        }

        public void SaveWebSheet()
        {
            string invt_id = "", invtgrp_code = "", invtsubgrp_code = "", invt_name = "", unit_code = "" ;
            string invtcut_type = "", dealer_no = "";
            decimal running = 0, qty_max = 0, qty_reorder = 0;
            try
            {
                invt_id = DwMain.GetItemString(1, "invt_id");
                invt_name = DwMain.GetItemString(1, "invt_name").Trim().Replace("'", "");
                invtgrp_code = DwMain.GetItemString(1, "invtgrp_code");
                try { invtsubgrp_code = DwMain.GetItemString(1, "invtsubgrp_code"); }
                catch { invtsubgrp_code = ""; }
                unit_code = DwMain.GetItemString(1, "unit_code");
                qty_max = DwMain.GetItemDecimal(1, "qty_max");
                qty_reorder = DwMain.GetItemDecimal(1, "qty_reorder");
                invtcut_type = DwMain.GetItemString(1, "invtcut_type");
                try { dealer_no = DwMain.GetItemString(1, "dealer_no"); }
                catch { dealer_no = ""; }
                if (invt_id == "AUTO")
                {
                    if (state.SsCoopControl == "000000" || state.SsCoopControl == "022001")
                    {
                        ///format วัสดุ สอ.มศว 0000-0000 -> 00{หมวดวัสดุ} 00{หมวดย่อยวัสดุ} - 0000{running number}
                        running = GetRuningNumber(invtgrp_code, invtsubgrp_code);
                        invt_id = invtgrp_code.Substring(1, 2) + invtsubgrp_code.Substring(1, 2) + running.ToString("0000");
                    }
                    else
                    {
                        n_commonClient com = wcf.NCommon;
                        invt_id = com.of_getnewdocno(state.SsWsPass, state.SsCoopId, "CMDINVT");
                    }
                    DwMain.SetItemString(1, "invt_id", invt_id);
                    String seqin = @"insert into PTINVTMAST 
                        (INVT_ID, INVT_NAME, QTY_MAX, QTY_REORDER, QTY_BAL, UNIT_CODE, INVTGRP_CODE, INVTSUBGRP_CODE, INVTCUT_TYPE, DEALER_NO) 
                        values ('" + invt_id + "','" + invt_name + "', " + qty_max + ", " + qty_reorder + ", 0, '" + unit_code + "','" + invtgrp_code + @"',
                        '" + invtsubgrp_code + "','"+ invtcut_type +"', '"+ dealer_no +"')";
                    dt = WebUtil.QuerySdt(seqin);
                }
                else
                {
                    String sequp = @"update PTINVTMAST set INVT_NAME ='" + invt_name + "', QTY_MAX =" + qty_max + ", QTY_REORDER =" + qty_reorder + @", 
                    UNIT_CODE ='" + unit_code + "', INVTGRP_CODE ='" + invtgrp_code + "', INVTSUBGRP_CODE ='" + invtsubgrp_code + @"', 
                    INVTCUT_TYPE = '" + invtcut_type + "', DEALER_NO = '"+ dealer_no +"' where INVT_ID = '" + invt_id + "'";
                    dt = WebUtil.QuerySdt(sequp);
                }
                //DwMain.UpdateData();
                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ " + invt_id);
                ResetPage();
            }
            catch (Exception ex)
            {

                LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
            }

        }

        public void WebSheetLoadEnd()
        {
            DwMain.SaveDataCache();
            DwDetail.SaveDataCache();
        }

        #region Extra Function
        private void PostOnInsert()
        {
            DwMain.InsertRow(0);
        }

        private void jsPostFindShow()
        {
            String invt_id = HinvtId.Value;
            DwMain.Retrieve(invt_id);
            string invtgrp_code = DwMain.GetItemString(1, "invtgrp_code");
            string invtsubgrp_code = DwMain.GetItemString(1, "invtsubgrp_code");
            DwMain.SetItemString(1, "invtgrp_code", invtgrp_code);
            DwUtil.RetrieveDDDW(DwMain, "invtsubgrp_code", pbl, invtgrp_code);
            DwMain.SetItemString(1, "invtsubgrp_code", invtsubgrp_code);
            DwDetail.Retrieve(invt_id);
        }

        private void PostDealer()
        {
            string dealer_no = "", dealer_name = "";
            try
            {
                dealer_no = DwMain.GetItemString(1, "dealer_no");
                string sqlsedealer = @"select dealer_name from ptucfdealer where dealer_no = {0}";
                sqlsedealer = WebUtil.SQLFormat(sqlsedealer, dealer_no);
                dt = WebUtil.QuerySdt(sqlsedealer);
                if (dt.Next())
                {
                    dealer_name = dt.GetString("dealer_name").Trim();
                }
                if (dealer_name == null || dealer_name == string.Empty)
                {
                    throw new Exception("หมายเลขคู่ค้าเลขที่ " + dealer_no + " ไม่มีข้อมูล");
                }
                else
                {
                    DwMain.SetItemString(1, "dealer_name", dealer_name);
                }
            }
            catch (Exception ex)
            { 
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
                DwMain.SetItemString(1, "dealer_no", "");
                DwMain.SetItemString(1, "dealer_name", "");
            }
        }

        private void PostInvtGrp()
        {
            string invtgrp = "";
            invtgrp = DwMain.GetItemString(1, "invtgrp_code");
            DwUtil.RetrieveDDDW(DwMain, "invtsubgrp_code", pbl, invtgrp);
        }

        public void ResetPage()
        {
            DwMain.Reset();
            DwDetail.Reset();
            DwMain.InsertRow(0);
            DwMain.SetItemString(1, "invtcut_type", "FI"); //กำหนดค่าเริ่มต้นเป็นแบบ FIFO
        }

        //get format from ptucfconfigitem
        private String CheckFormat(String invtid)
        {
            Decimal year_flag = 0, branch_flag = 0, dept_flag = 0, group_flag = 0, subgroup_flag = 0, number_flag = 0;
            String configitem_format = "";
            String invt_year = "";  

            String sqlformat = @"select year_flag, branch_flag, dept_flag, group_flag, subgroup_flag, number_flag, configitem_format 
                                from ptucfconfigitem where configitem_code = 'INVNO'";
            dt = WebUtil.QuerySdt(sqlformat);
            if (dt.Next())
            {
                year_flag = dt.GetDecimal("year_flag");
                branch_flag = dt.GetDecimal("branch_flag");
                dept_flag = dt.GetDecimal("dept_flag");
                group_flag = dt.GetDecimal("group_flag");
                subgroup_flag = dt.GetDecimal("subgroup_flag");
                number_flag = dt.GetDecimal("number_flag");
                configitem_format = dt.GetString("configitem_format");
            }
            if (year_flag == 1)
            {
                invt_year = invtid.Substring(0, 2);
            }
            return invtid;
        }

        private Decimal GetRuningNumber(string invtgrpCode, string invtsubgrpCode)
        {
            decimal running = 0;
            string sqlcount = @"select count(1) as running from ptinvtmast where invtgrp_code = '" + invtgrpCode + "' and invtsubgrp_code = '"+ invtsubgrpCode +"'";
            dt = WebUtil.QuerySdt(sqlcount);
            if (dt.Next())
            {
                running = dt.GetDecimal("running");
            }
            running++;
            return running;
        }
        #endregion

    }
}