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
using Sybase.DataWindow;
using System.Data.OracleClient;
using System.Globalization;
//using CoreSavingLibrary.WcfNDeposit;
//using CoreSavingLibrary.WcfNCommon;
using Saving;
using DataLibrary;
using System.Web.Services.Protocols;
using CoreSavingLibrary.WcfNDeposit; // new deposit
using CoreSavingLibrary.WcfNCommon; //new common

namespace Saving.Applications.ap_deposit
{
    public partial class w_sheet_procdeptuptran : PageWebSheet, WebSheet
    {
        // JavaSctipt PostBack
        protected String PostCutProcess;
        protected String PostRetriveDepttrans;
        protected String jsSetMemberFormat;
        protected String jsResetMemberno;
        private DwThDate tdw_processday;

        public string outputProcess;
        #region WebSheet Members

        public void InitJsPostBack()
        {
            PostCutProcess = WebUtil.JsPostBack(this, "PostCutProcess");
            PostRetriveDepttrans = WebUtil.JsPostBack(this, "PostRetriveDepttrans");
            jsSetMemberFormat = WebUtil.JsPostBack(this, "jsSetMemberFormat");
            jsResetMemberno = WebUtil.JsPostBack(this, "jsResetMemberno");
            tdw_processday = new DwThDate(Dw_Main, this);
            tdw_processday.Add("process_date", "process_tdate");
        }

        public void WebSheetLoadBegin()
        {
            HdMountCut.Value = "false";
            if (!IsPostBack)
            {
                Dw_Main.InsertRow(0);
                Dw_Main.SetItemDate(1, "process_date", state.SsWorkDate);
                tdw_processday.Eng2ThaiAllRow();
            }
            else
            {
                this.RestoreContextDw(Dw_Main);
                this.RestoreContextDw(Dw_Detail);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "PostCutProcess")
            {
                JsPostCutProcess();
            }
            if (eventArg == "PostRetriveDepttrans")
            {
                JsPostRetriveDepttrans();
            }
            if (eventArg == "jsSetMemberFormat")
            {
                String member_no = "";
                try { member_no = Dw_Main.GetItemString(1, "member_no"); }
                catch { member_no = ""; }
                if (member_no != null && member_no != "")
                {
                    member_no = WebUtil.MemberNoFormat(member_no);
                    Dw_Main.SetItemString(1, "member_no", member_no);
                }
            }
            if (eventArg == "jsResetMemberno")
            {
                Dw_Main.SetItemString(1, "member_no", "");
            }

        }

        public void SaveWebSheet()
        {
        }

        public void WebSheetLoadEnd()
        {
            Dw_Main.SaveDataCache();
            Dw_Detail.SaveDataCache();
        }

        #endregion

        private void JsPostRetriveDepttrans()
        {
            Dw_Detail.InsertRow(0);
            String system_code = Dw_Main.GetItemString(1, "system_code");
            DateTime ProcessDate = new DateTime(1370, 1, 1);
            try
            {
                ProcessDate = Dw_Main.GetItemDateTime(1, "process_date");
            }
            catch { }
            object[] args = new object[] { state.SsCoopId, ProcessDate,system_code };
            DwUtil.RetrieveDataWindow(Dw_Detail, "dp_depttrans.pbl", null, args);
            //Dw_Detail.Retrieve(state.SsCoopControl, "KEP", ProcessDate);
            String member_no = "";
            try { member_no = Dw_Main.GetItemString(1, "member_no"); }
            catch { member_no = ""; }
            if (member_no != null && member_no != "")
            {
                Dw_Detail.SetFilter("member_no = '" + member_no + "'");
                Dw_Detail.Filter();
            }
            Label1.Text = "จำนวนรายการทั้งหมด " + Dw_Detail.RowCount.ToString() + " รายการ";
        }

        private void JsPostCutProcess()
        {
            //bee
            int check_grouprunning = 0;
            string sql = @"select nvl(check_grouprunning,0) as check_grouprunning from dpdeptconstant";
            sql = WebUtil.SQLFormat(sql);
            Sdt dt = WebUtil.QuerySdt(sql);
            if (dt.Next())
            {
                check_grouprunning = dt.GetInt32("check_grouprunning");
            }
            if (check_grouprunning == 1)
            {
                sql = @"select count(*) countitem,
                    process_id,
                    (case when object_name ='DPUPMONTH' then 'ประมวลผลตัดยอดฝากรายเดือน - DPUPMONTH' 
                    when object_name ='DPTRANDEPT' then 'ประมวลฝากจากระบบอื่น - DPTRANDEPT' 
                    else object_name
                    end  )as object_name
                     from cmprocessing where object_name in('DPUPMONTH','DPTRANDEPT') and runtime_status=1 and show_flag <>-9 
                    group by process_id,object_name ";
                sql = WebUtil.SQLFormat(sql, state.SsWorkDate);
                dt = WebUtil.QuerySdt(sql);
                string errorStr = "";
                decimal ld_rec = 0, ld_pay = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Next();
                    if (dt.GetInt32("countitem") > 0)
                    {
                        errorStr += dt.GetString("object_name").Trim() + " " + dt.GetString("process_id").Trim();
                    }
                }
                if (errorStr.Trim().Length > 0)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ขณะนี้มีการทำรายการแบบกลุ่ม กรุณาทำรายการภายหลัง " + errorStr);
                    return;
                }
            }
            //bee end
            String system_code = Dw_Main.GetItemString(1, "system_code");

                try
                {
                    DateTime ProcessDate = new DateTime(1370, 1, 1);
                    try
                    {
                        ProcessDate = Dw_Main.GetItemDateTime(1, "process_date");
                    }
                    catch { }
                    
                    string option_xml = Dw_Main.Describe("DataWindow.Data.XML");
                    outputProcess = WebUtil.runProcessing(state, "DPTRANDEPT", option_xml, state.SsClientIp, "");

                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex);

                }

        }
    }
}