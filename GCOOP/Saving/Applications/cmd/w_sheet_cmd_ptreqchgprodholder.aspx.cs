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
    public partial class w_sheet_cmd_ptreqchgprodholder : PageWebSheet, WebSheet
    {
        protected String postFindShow;
        protected String jsPostOnInsert;

        private DwThDate tDwMain; 
        Sdt dt = new Sdt();
        String pbl = "cmd_reqchgprodholder.pbl";

        public void InitJsPostBack()
        {
            jsPostOnInsert = WebUtil.JsPostBack(this, "jsPostOnInsert");
            postFindShow = WebUtil.JsPostBack(this, "postFindShow");
            tDwMain = new DwThDate(DwMain, this);
            tDwMain.Add("reqchg_date", "reqchg_tdate");
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                PostReset();
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
                case "jsPostOnInsert":
                    PostOnInsert();
                    break;
                case "postFindShow":
                    jsPostFindShow();
                    break;
            }
        }

        public void SaveWebSheet()
        {
            String durt_id = "", durt_regno = "", durt_name = "", dept_code = "", holder_name = "", branch_code = "";
            String reqchg_no = "", old_deptcode = "", old_holdername = "", old_branchcode = "";
            DateTime reqchg_date = new DateTime();
            Sdt ta1, ta2;
            try
            {
                reqchg_no = DwMain.GetItemString(1, "reqchg_no");
                durt_id = DwMain.GetItemString(1, "durt_id");
                durt_regno = DwMain.GetItemString(1,"durt_regno").Trim();
                durt_name = DwMain.GetItemString(1,"durt_name").Trim();
                dept_code = DwMain.GetItemString(1, "dept_code").Trim();
                holder_name = DwMain.GetItemString(1, "holder_name").Trim();
                branch_code = DwMain.GetItemString(1, "branch_code");
                old_deptcode = DwMain.GetItemString(1, "old_deptcode").Trim();
                old_holdername = DwMain.GetItemString(1, "old_holdername").Trim();
                old_branchcode = DwMain.GetItemString(1, "old_branchcode");

                if (reqchg_no == "AUTO")
                {
                    n_commonClient com = wcf.NCommon;
                    reqchg_no = com.of_getnewdocno(state.SsWsPass,state.SsCoopId, "CMDREQCHG");
                    try { reqchg_date = DwMain.GetItemDateTime(1, "slip_date"); }
                    catch { reqchg_date = state.SsWorkDate; }

                    String insert = @"insert into ptreqchgprodholder 
                                    (reqchg_no, reqchg_date, durt_id, old_deptcode, old_holdername, old_branchcode, 
	                                dept_code, holder_name, branch_code, reqchg_status) values
                                    ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9})";
                    insert = WebUtil.SQLFormat(insert, reqchg_no, reqchg_date, durt_id, old_deptcode, old_holdername, old_branchcode,
                        dept_code, holder_name, branch_code, 1);
                    ta1 = WebUtil.QuerySdt(insert);

                    //update ข้อมูลลง ptdurtmaster
                    String upsql = @"update ptdurtmaster set dept_code = {1}, holder_name = {2}, branch_code = {3}
                                    where durt_id = {0}";
                    upsql = WebUtil.SQLFormat(upsql, durt_id, dept_code, holder_name, branch_code);
                    ta2 = WebUtil.QuerySdt(upsql);

                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ " + reqchg_no);
                    PostReset();
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
            }

        }

        public void WebSheetLoadEnd()
        {
            if (DwMain.RowCount > 1)
            {
                DwMain.DeleteRow(DwMain.RowCount);
            }
            tDwMain.Eng2ThaiAllRow();
            DwMain.SaveDataCache();
        }

        private void PostOnInsert()
        {
            DwMain.InsertRow(0);
        }

        private void jsPostFindShow()
        {
            string durt_id = "", durt_regno = "", durt_name = "", dept_code = "";
            string holder_name = "", branch_code = "";            
            Sdt ta;
            Int32 row = 1;
            durt_id = DwMain.GetItemString(row, "durt_id").Trim();
            try
            {
                String se = @"select durt_id, durt_regno, durt_name, dept_code, holder_name, branch_code 
                          from ptdurtmaster where  durt_id = '" + durt_id + "'";
                ta = WebUtil.QuerySdt(se);
                if (ta.Next())
                {
                    durt_regno = ta.GetString("durt_regno").Trim();
                    durt_name = ta.GetString("durt_name").Trim();
                    dept_code = ta.GetString("dept_code").Trim();
                    holder_name = ta.GetString("holder_name").Trim();
                    branch_code = ta.GetString("branch_code");
                }
                DwMain.SetItemString(row, "durt_regno", durt_regno);
                DwMain.SetItemString(row, "durt_name", durt_name);
                DwMain.SetItemString(row, "dept_code", string.Empty);
                DwMain.SetItemString(row, "holder_name", string.Empty);
                DwMain.SetItemString(row, "branch_code", string.Empty);
                DwMain.SetItemString(row, "old_deptcode", dept_code );
                DwMain.SetItemString(row, "old_holdername", holder_name);
                DwMain.SetItemString(row, "old_branchcode", branch_code);
            }
            catch(Exception ex)
            { LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message); }
        }

        private void PostReset()
        {
            DwMain.Reset();
            DwMain.InsertRow(0);
            tDwMain.Eng2ThaiAllRow();
            DwMain.SetItemDateTime(1, "reqchg_date", state.SsWorkDate);
            DwUtil.RetrieveDDDW(DwMain, "dept_code", pbl, null);
            DwUtil.RetrieveDDDW(DwMain, "old_deptcode", pbl, null);
            DwUtil.RetrieveDDDW(DwMain, "branch_code", pbl, null);
            DwUtil.RetrieveDDDW(DwMain, "old_branchcode", pbl, null);
        }
    }
}