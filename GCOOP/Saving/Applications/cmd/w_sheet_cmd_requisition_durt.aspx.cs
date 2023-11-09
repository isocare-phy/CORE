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
    public partial class w_sheet_cmd_requisition_durt : PageWebSheet, WebSheet
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
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            switch (eventArg)
            {
                case "jsPostSlipinNo":
                    PostRetriveSlipinNo();
                    break;
                case "jsPostFormat":
                    //PostFormat();
                    break;
            }
        }

        public void SaveWebSheet()
        {
            String durt_id = "", dept_code = "", holder_name = "", durt_regno = "", up = "", branch_code = "";
            int row = 0;
            row = DwDetail.RowCount;

            try
            {
                for (int i = 1; i <= row; i++)
                {
                    try { durt_id = DwDetail.GetItemString(i, "durt_id"); }
                    catch { throw new Exception("ไม่สามารถดึงเลขครุภัณฑ์ได้"); }
                    try { dept_code = DwDetail.GetItemString(i, "dept_code"); }
                    catch { dept_code = ""; }
                    try { holder_name = DwDetail.GetItemString(i, "holder_name").Trim(); }
                    catch { holder_name = ""; }
                    try { durt_regno = DwDetail.GetItemString(i, "durt_regno").Trim(); }
                    catch { durt_regno = ""; }
                    try { branch_code = DwDetail.GetItemString(i, "branch_code"); }
                    catch { branch_code = "000"; }

                    if (dept_code != null || dept_code != "")
                    {
                        durt_regno = PostFormat(i);
                    }
                    //update ptdurtmaster
                    up = @"update ptdurtmaster set dept_code = '" + dept_code + "', holder_name = '" + holder_name + "', durt_regno = '" + durt_regno + @"'
                            branch_code = '"+branch_code+"' where durt_id = '" + durt_id + "'";
                    ta = WebUtil.QuerySdt(up);
                    //update ptdurtslipindet
                    up = @"update ptdurtslipindet set dept_code = '" + dept_code + "', holder_name = '" + holder_name + "', durt_regno = '" + durt_regno + @"'
                            where durt_id = '" + durt_id + "'";
                    ta = WebUtil.QuerySdt(up);
                }
                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
            }
            catch (Exception ex)
            { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }

        }

        public void WebSheetLoadEnd()
        {
            DwMain.SaveDataCache();
            tDwMain.Eng2ThaiAllRow();
            DwDetail.SaveDataCache();
        }

        #region function

        private void PostRetriveSlipinNo()
        {
            String slipin_no = "";
            slipin_no = DwMain.GetItemString(1, "slipin_no");
            DwUtil.RetrieveDataWindow(DwMain, pbl, null, slipin_no);
            DwUtil.RetrieveDataWindow(DwDetail, pbl, null, slipin_no);
            DwUtil.RetrieveDDDW(DwDetail, "dept_code", pbl, null);
        }
        //หา format เลขครุภัณฑ์
        String PostFormat(int row)
        {
            String se = "", durt_regno = "", maxdept = "";
            Decimal year_flag = 0, branch_flag = 0, dept_flag = 0, group_flag = 0, number_flag = 0;

            se = "select year_flag, branch_flag, dept_flag, group_flag, number_flag from ptucfconfigitem where configitem_code = 'REGNO'";
            ta = WebUtil.QuerySdt(se);
            if (ta.Next())
            {
                year_flag = ta.GetDecimal("year_flag");
                branch_flag = ta.GetDecimal("branch_flag");
                dept_flag = ta.GetDecimal("dept_flag");
                group_flag = ta.GetDecimal("group_flag");
                number_flag = ta.GetDecimal("number_flag");
            }

            if (year_flag == 1)
            { durt_regno += Convert.ToString(DwMain.GetItemDecimal(1, "devaluelastcal_year") + 543).Substring(0, 2); }
            if (branch_flag == 1)
            { durt_regno += "001"; }
            if (dept_flag == 1)
            { durt_regno += DwDetail.GetItemString(row, "dept_code"); }
            if (group_flag == 1)
            { durt_regno += DwMain.GetItemString(1, "durtgrp_code"); }
            if (number_flag == 1)
            {
                maxdept = MaxDept(DwDetail.GetItemString(row, "dept_code"), DwMain.GetItemString(1, "durtgrp_code"));
                durt_regno += maxdept;
            }

            return durt_regno;
        }
        String MaxDept(String DeptCode, String DurtGrpCode)
        {
            String maxdep = "", se = "";
            se = "select count(dept_code) as cdeptcode from ptdurtmaster where dept_code = '" + DeptCode + "' and durtgrp_code = '" + DurtGrpCode + "'";
            ta = WebUtil.QuerySdt(se);
            if (ta.Next())
            {
                maxdep = Convert.ToString(ta.GetDecimal("cdeptcode") + 1);
            }
            for (int i = maxdep.Length; i < 4; i++)
            {
                maxdep = "0" + maxdep;
            }
            return maxdep;
        }

        #endregion
    }
}