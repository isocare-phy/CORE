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
using Sybase.DataWindow;
using CoreSavingLibrary.WcfNCommon;
using System.Data.SqlClient;

namespace Saving.Applications.cmd
{
    public partial class w_sheet_requisition_durt : PageWebSheet, WebSheet
    {
        String pbl = "cmd_requisition_durt.pbl";
        Sdt ta;

        private DwThDate tDwMain;
        protected String jsPostSlipinNo;
        protected String jsPostFormat;

        public void InitJsPostBack()
        {
            tDwMain = new DwThDate(DwMain, this);
            tDwMain.Add("buy_date", "buy_tdate");

            jsPostSlipinNo = WebUtil.JsPostBack(this, "jsPostSlipinNo");
            jsPostFormat = WebUtil.JsPostBack(this, "jsPostFormat");
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                DwMain.InsertRow(0);
            }
            else
            {
                this.RestoreContextDw(DwMain, tDwMain);
                this.RestoreContextDw(DwDetail);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            switch (eventArg)
            {
                case "jsPostSlipinNo":
                    PostRetriveSlipinNo();
                    break;
                case "jsPostSetFormat":
                    PostSetFormat();
                    break;
            }
        }

        public void SaveWebSheet()
        {
            String durt_id = "", dept_code = "", holder_name = "", durt_regno = "", durtserial_number = "";
            string branch_code = "", up = "";
            int row = 0;
            row = DwDetail.RowCount;

            try
            {
                for (int i = 1; i <= row; i++)
                {
                    try { branch_code = DwDetail.GetItemString(i, "branch_code"); }
                    catch { throw new Exception("กรุณาระบบสาขา"); }
                    try { durt_id = DwDetail.GetItemString(i, "durt_id"); }
                    catch { throw new Exception("ไม่สามารถดึงเลขครุภัณฑ์ได้"); }
                    try { dept_code = DwDetail.GetItemString(i, "dept_code"); }
                    catch { dept_code = ""; }
                    try { holder_name = DwDetail.GetItemString(i, "holder_name").Trim(); }
                    catch { throw new Exception("กรุณากรอกชื่อผู้ครอบครองครุภัณฑ์"); }
                    try { durt_regno = DwDetail.GetItemString(i, "durt_regno").Trim(); }
                    catch { durt_regno = ""; }
                    try { durtserial_number = DwDetail.GetItemString(i, "durtserial_number"); }
                    catch { durtserial_number = string.Empty; }

                    if (durt_regno == null || durt_regno == "")
                    {
                        durt_regno = PostFormat(i);
                        DwDetail.SetItemString(i, "durt_regno", durt_regno);
                        HdDurtreg.Value += Convert.ToString(i) + "." + durt_regno + " ";
                    }
                    //update ptdurtmaster
                    up = @"update ptdurtmaster set dept_code = '"+dept_code+"', holder_name = '"+holder_name+"', durt_regno = '"+durt_regno+@"',
                           durtserial_number = '" + durtserial_number + "', branch_code = '" + branch_code + "' where durt_id = '" + durt_id + "'";
                    Sdt ta1 = WebUtil.QuerySdt(up);
                    //update ptdurtslipindet
                    string upslip = @"update ptdurtslipindet set dept_code = '" + dept_code + "', holder_name = '" + holder_name + "', durt_regno = '" + durt_regno + @"'
                            where durt_id = '" + durt_id + "'";
                    Sdt ta2 = WebUtil.QuerySdt(upslip);

                }
                //this.SetOnLoadedScript("alert('"+ HdDurtreg.Value +"')");
                HdCksave.Value = "true";
                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
            }
            catch (Exception ex)
            { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }


        }

        public void WebSheetLoadEnd()
        {
            DwMain.SaveDataCache();
            DwDetail.SaveDataCache();
            tDwMain.Eng2ThaiAllRow();
        }

        private void PostRetriveSlipinNo()
        {
            String slipin_no = "";
            slipin_no = DwMain.GetItemString(1, "slipin_no");
            DwUtil.RetrieveDataWindow(DwMain, pbl, null, slipin_no);
            DwUtil.RetrieveDataWindow(DwDetail, pbl, null, slipin_no);  
        }

        private void PostSetFormat()
        {
            String durt_regno = "", dept_code = "";
            int r = Convert.ToInt32(HdRow.Value);
            try { dept_code = DwDetail.GetItemString(r, "dept_code"); }
            catch { dept_code = string.Empty; }
            durt_regno = PostFormat(r);
            DwDetail.SetItemString(r, "durt_regno", durt_regno);
            HdRow.Value = null;
        }

        String PostFormat(int row)
        {
            String se = "", durt_regno = "", maxdept = "", running_durt = "", doc_code = "";
            Decimal year_flag = 0, branch_flag = 0, dept_flag = 0, group_flag = 0, number_flag = 0 ;
            string branch_code = "", durtgrp_code = "", durtgrpsub_code = "";
            try
            {
                doc_code = "CMDDURTRUN";
                //Format 1 >> XX-XXXX-YYNNNN = รหัสสาขา-รหัสกลุ่ม(XX)รหัสกลุ่มย่อย(XX)-ปีruning number
                if (state.SsCoopControl == "000000" || state.SsCoopControl == "022001")
                {
                    try { branch_code = DwDetail.GetItemString(row, "branch_code"); }
                    catch { throw new Exception("กรุณาระบุ สาขา"); }
                    try { durtgrp_code = DwMain.GetItemString(1, "durtgrp_code"); }
                    catch { }
                    try { durtgrpsub_code = DwMain.GetItemString(1, "durtgrpsub_code"); }
                    catch { }
                    ///running ให้ใช้ count max จาก durtgrp_code แทน //edit by cherry @06072560
                    //running_durt = GetMaxRunningDurt(durtgrp_code, durtgrpsub_code);
                    doc_code = doc_code + branch_code + durtgrp_code + durtgrpsub_code;
                    running_durt = wcf.NCommon.of_getnewdocno(state.SsWsPass, state.SsCoopControl, doc_code);
                    durt_regno = branch_code.Substring(1, 2) + durtgrp_code.Substring(1, 2) + durtgrpsub_code.Substring(1, 2) + running_durt;
                }
                else if (state.SsCoopControl == "031001")
                {
                    string prefix = "สอ.ชป.";
                    try { durtgrp_code = DwMain.GetItemString(1, "durtgrp_code"); }
                    catch { }
                    try { durtgrpsub_code = DwMain.GetItemString(1, "durtgrpsub_code"); }
                    catch { }
                    string durtgrpsub_abb = DurtSubgrpAbb(durtgrp_code, durtgrpsub_code);
                    string year = WebUtil.GetAccyear(state.SsCoopControl, state.SsWorkDate).ToString().Substring(2);
                    running_durt = wcf.NCommon.of_getnewdocno(state.SsWsPass, state.SsCoopControl, doc_code);
                    durt_regno = prefix + durtgrpsub_abb + year + "/" + running_durt;
                }
                else
                {
                    try { branch_code = DwDetail.GetItemString(row, "branch_code"); }
                    catch { throw new Exception("กรุณาระบุ สาขา"); }
                    try { durtgrp_code = DwMain.GetItemString(1, "durtgrp_code"); }
                    catch { }
                    try { durtgrpsub_code = DwMain.GetItemString(1, "durtgrpsub_code"); }
                    catch { }
                    
                    running_durt = wcf.NCommon.of_getnewdocno(state.SsWsPass, state.SsCoopControl, doc_code);
                    durt_regno = branch_code.Substring(1, 2) + durtgrp_code.Substring(1, 2) + durtgrpsub_code.Substring(1, 2) + running_durt;

                    //se = "select year_flag, branch_flag, dept_flag, group_flag, number_flag from ptucfconfigitem where configitem_code = 'REGNO'";
                    //ta = WebUtil.QuerySdt(se);
                    //if (ta.Next())
                    //{
                    //    year_flag = ta.GetDecimal("year_flag");
                    //    branch_flag = ta.GetDecimal("branch_flag");
                    //    dept_flag = ta.GetDecimal("dept_flag");
                    //    group_flag = ta.GetDecimal("group_flag");
                    //    number_flag = ta.GetDecimal("number_flag");
                    //}

                    //if (year_flag == 1)
                    //{ durt_regno += Convert.ToString(DwMain.GetItemDecimal(1, "devaluelastcal_year") + 543).Substring(2, 2) + "-"; }
                    //if (branch_flag == 1)
                    //{ durt_regno += "001" + "-"; }
                    //if (dept_flag == 1)
                    //{ durt_regno += DeptAbb(DwDetail.GetItemString(row, "dept_code")) + "-"; }
                    //if (group_flag == 1)
                    //{ durt_regno += DurtgrpAbb(DwMain.GetItemString(1, "durtgrp_code")) + "-"; }
                    //if (number_flag == 1)
                    //{
                    //    maxdept = MaxDept(DwDetail.GetItemString(row, "dept_code"), DwMain.GetItemString(1, "durtgrp_code"));
                    //    durt_regno += maxdept;
                    //}
                }
            }
            catch (Exception ex)
            { LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message); }
            return durt_regno;
        }

        private String MaxDept(String DeptCode, String DurtGrpCode)
        {
            String maxdep = "", se = "";
            Int32 rowse = 0 , maxrow = 0;
            se = "select count(dept_code) as cdeptcode from ptdurtmaster where dept_code = '" + DeptCode + "' and durtgrp_code = '" + DurtGrpCode + "'";
            ta = WebUtil.QuerySdt(se);
            rowse = CheckselectDept(DeptCode);
            if (ta.Next())
            {
                maxrow = rowse + ta.GetInt32("cdeptcode") + 1;
                maxdep = Convert.ToString(maxrow);
            }
            for (int i = maxdep.Length; i < 4; i++)
            {
                maxdep = "0" + maxdep;
            }
            return maxdep;
        }

        private String DurtgrpAbb(String DurtgrpCode)
        {
            String DurtgrpAbb = "";
            String se = "select durtgrp_abb from ptucfdurtgrpcode where durtgrp_code = '"+ DurtgrpCode +"'";
            ta = WebUtil.QuerySdt(se);
            if (ta.Next())
            {
                DurtgrpAbb = ta.GetString("durtgrp_abb").Trim();
            }
            return DurtgrpAbb;
        }

        private String DurtSubgrpAbb(string DurtgrpCode, string DurtgrpsubCode)
        {
            String durtgrpsub_abb = "";
            String se = "select durtgrpsub_abb from ptucfdurtgrpsubcode where durtgrp_code = {0} and durtgrpsub_code = {1}";
            se = WebUtil.SQLFormat(se, DurtgrpCode, DurtgrpsubCode);
            ta = WebUtil.QuerySdt(se);
            if (ta.Next())
            {
                durtgrpsub_abb = ta.GetString("durtgrpsub_abb").Trim();
            }
            return durtgrpsub_abb;
        }

        private String DeptAbb(String DeptCode)
        {
            String dept_abb = "", se = "";
            se = "select dept_abb from ptucfdeptcode where dept_code = '" + DeptCode + "'";
            ta = WebUtil.QuerySdt(se);
            if (ta.Next())
            {
                dept_abb = ta.GetString("dept_abb").Trim();
            }
            return dept_abb;
        }

        private Int32 CheckselectDept(String DeptCode)
        {
            Int32 serow = 0;
            String d = "";
            for (Int32 i = 0; i <= DwDetail.RowCount; i++)
            {
                try { d = DwDetail.GetItemString(i, "dept_code").Trim(); }
                catch { d = ""; }
                if (d == DeptCode)
                {
                    serow = serow + 1;
                }
            }
            serow = serow - 1;
            return serow;
        }

        private string GetMaxRunningDurt(string DurtgrpCode, string DurtsubgrpCode)
        {
            string running = "", se = "";
            int rowse = 0, cdurt = 0;
            se = "select count(1) as cdurt from ptdurtmaster where durt_status = 1 and durtgrp_code = {0} and durtgrpsub_code = {1}";
            se = WebUtil.SQLFormat(se, DurtgrpCode, DurtsubgrpCode);
            ta = WebUtil.QuerySdt(se);
            rowse = CheckselectDurtDwDetail(DurtgrpCode, DurtsubgrpCode);
            if (ta.Next())
            {
                cdurt = ta.GetInt32("cdurt");
            }
            running = (rowse + cdurt + 1).ToString("0000");
            return running;
        }

        private int CheckselectDurtDwDetail(string DurtgrpCode, string DurtsubgrpCode)
        {
            int rowdwdetail = 0;
            String durt_regno = "";
            for (Int32 i = 1; i <= DwDetail.RowCount; i++)
            {
                try { durt_regno = DwDetail.GetItemString(i, "durt_regno").Trim(); }
                catch { durt_regno = string.Empty; }
                if (durt_regno != "" && durt_regno != null)
                {
                    rowdwdetail ++;
                }
            }
            return rowdwdetail;
        }

        private string GetMaxRunningDurtBranch(string BranchId, string DurtgrpCode, string DurtsubgrpCode)
        {
            string running = "", se = "";
            int rowse = 0, cdurt = 0;
            se = "select count(1) as cdurt from ptdurtmaster where durt_status = 1 and branch_code = {0} and durtgrp_code = {1} and durtgrpsub_code = {2}";
            se = WebUtil.SQLFormat(se, BranchId, DurtgrpCode, DurtsubgrpCode);
            ta = WebUtil.QuerySdt(se);
            rowse = CheckselectDurtDwDetail(DurtgrpCode, DurtsubgrpCode);
            if (ta.Next())
            {
                cdurt = ta.GetInt32("cdurt");
            }
            running = (rowse + cdurt + 1).ToString("0000");
            return running;
        }
    }
}