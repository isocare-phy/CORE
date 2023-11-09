using System;
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
using Sybase.DataWindow;
using DBAccess;
using System.Web.Services.Protocols;
using CommonLibrary;

namespace Saving.Applications.app_assist
{
    public partial class w_sheet_kt_modify50bath : PageWebSheet, WebSheet
    {
        private string pbl = "kt_50bath.pbl";
        protected String jsPostMembNo;
        protected String jsButtonPayClick;
        protected String jsButtonDelClick;
        protected DwThDate tDwSlip;

        public void InitJsPostBack()
        {
            jsPostMembNo = WebUtil.JsPostBack(this, "jsPostMembNo");
            jsButtonPayClick = WebUtil.JsPostBack(this, "jsButtonPayClick");
            jsButtonDelClick = WebUtil.JsPostBack(this, "jsButtonDelClick");

            tDwSlip = new DwThDate(DwSlip, this);
            tDwSlip.Add("entry_date", "entry_tdate");
        }

        public void WebSheetLoadBegin()
        {
            if (IsPostBack)
            {
                this.RestoreContextDw(DwMain);
                this.RestoreContextDw(DwSlip);
            }
            else
            {
                DwMain.Reset();
                DwMain.InsertRow(0);
                DwSlip.Reset();
                DwSlip.InsertRow(0);
                DwSlip.SetItemDate(1, "entry_date", state.SsWorkDate);
                tDwSlip.Eng2ThaiAllRow();

            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            switch (eventArg)
            {
                case "jsPostMembNo":
                    PostMembNo();
                    break;
                case "jsButtonDelClick":
                    ButtonDelClick();
                    break;
                case "jsButtonPayClick":
                    ButtonPayClick();
                    break;
            }
        }

        public void SaveWebSheet()
        {
            
        }

        public void WebSheetLoadEnd()
        {
            DwMain.SaveDataCache();
            DwSlip.SaveDataCache();
        }

        private void PostMembNo()
        {
            try
            {
                string member_no = DwMain.GetItemString(1, "member_no");
                member_no = WebUtil.MemberNoFormat(member_no);
                DwMain.SetItemString(1, "member_no", member_no);
                DwUtil.RetrieveDataWindow(DwMain, pbl, null, member_no);
                try 
                { 
                    DwMain.GetItemString(1, "member_no");
                    //DwSlip.SetItemDate(1, "entry_date", state.SsWorkDate);
                    DwSlip.SetItemDecimal(1, "dept_amt", 50);
                }
                catch
                {
                    //DwSlip.Reset();
                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบเลขสมาชิกนี้");
                    return;
                }

                try
                {
                    string memb_date = DwMain.GetItemDateTime(1, "birth_date").ToString("yyyy");
                    int memb_age = DateTime.Today.Year - (Convert.ToInt32(memb_date));
                    //--------------------------------------------------------------------------------------
                    int Dmemb_date = Convert.ToInt32(DwMain.GetItemDateTime(1, "birth_date").ToString("dd"));
                    int Mmemb_date = Convert.ToInt32(DwMain.GetItemDateTime(1, "birth_date").ToString("MM"));
                    int Ymemb_date = Convert.ToInt32(DwMain.GetItemDateTime(1, "birth_date").ToString("yyyy"));
                    if (DateTime.Today.Month < Mmemb_date)
                    {
                        memb_age = memb_age - 1;
                    }
                    else if (DateTime.Today.Month == Mmemb_date)
                    {
                        if (DateTime.Today.Day < Dmemb_date)
                        {
                            memb_age = memb_age - 1;
                        }
                    }
                    DwMain.SetItemString(1, "age", (memb_age + " ปี"));
                }
                catch { }
            }
            catch
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบเลขสมาชิกนี้");
                return;
            }
        }

        private void ButtonPayClick()
        {
            String member_no = "", deptaccount_no = "";
            decimal bfprncbal = 0 ,prncbal = 0, assistdept_amt = 0, deptmonth_status = 0, checkpend_amt = 0, laststmseq_no = 0, dept_amt = 0;
            DateTime operate_date = DwSlip.GetItemDate(1, "entry_date");
            DateTime entry_date = DateTime.Now;
            try
            {
                member_no = DwMain.GetItemString(1, "member_no");
                String SqlSelectData = "select dp.prncbal as prncbal , asn.assistdept_amt as assistdept_amt, dp.deptmonth_status as deptmonth_status, dp.checkpend_amt as checkpend_amt , dp.laststmseq_no as laststmseq_no , asn.deptaccount_no as deptaccount_no from dpdeptmaster dp , asnreqmaster asn where dp.deptaccount_no = asn.deptaccount_no and asn.member_no = '" + member_no + "' and asn.assisttype_code = '70' ";
                Sdt dtAsn = WebUtil.QuerySdt(SqlSelectData);
                if (dtAsn.Next())
                {
                    bfprncbal = dtAsn.GetDecimal("prncbal");
                    assistdept_amt = dtAsn.GetDecimal("assistdept_amt");
                    deptmonth_status = dtAsn.GetDecimal("deptmonth_status");
                    checkpend_amt = dtAsn.GetDecimal("checkpend_amt");
                    laststmseq_no = dtAsn.GetDecimal("laststmseq_no");
                    deptaccount_no = dtAsn.GetString("deptaccount_no").Trim();

                    dept_amt = DwSlip.GetItemDecimal(1,"dept_amt");

                    prncbal = bfprncbal + dept_amt;
                    assistdept_amt += dept_amt;
                    laststmseq_no++;

                    if (prncbal >= checkpend_amt)
                        deptmonth_status = 0;
                    
                    String SqlIns = "insert into dpdeptstatement(deptaccount_no,branch_id ,seq_no,deptitemtype_code,operate_date,ref_docno ,deptitem_amt,balance_forward,prncbal,check_status,item_status,entry_id,entry_date,cash_type)" 
                                                      + "values ('"+deptaccount_no+"','000',"+laststmseq_no+",'AJI',to_date('"+operate_date.ToString("dd/MM/yyyy")+"','dd/mm/yyyy'),'MODADMIN',"+dept_amt+","+bfprncbal+","+prncbal+",1,1,'"+state.SsUsername+"',sysdate,'')";
                    WebUtil.Query(SqlIns);

                    String SqlUpDeptMaster = "update dpdeptmaster set prncbal	= " + prncbal + " , deptmonth_status = " + deptmonth_status + " ,laststmseq_no = " + laststmseq_no + " where deptaccount_no	= '" + deptaccount_no + "' ";
                    WebUtil.Query(SqlUpDeptMaster);

                    String SqlUpAsnMaster = "update asnreqmaster set assistdept_amt = "+prncbal+" where deptaccount_no	= "+deptaccount_no+" and assisttype_code = '70'";
                    WebUtil.Query(SqlUpAsnMaster);

                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกเรียบร้อย " + bfprncbal + " : " + assistdept_amt + " : " + checkpend_amt + " : " + deptaccount_no + " : " + dept_amt);
                }

            }
            catch(Exception ex) 
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ทำรายการไม่สำเร็จ : " + ex);  
            }

        }

        private void ButtonDelClick()
        {
            String member_no = "", deptaccount_no = "";
            decimal bfprncbal = 0, prncbal = 0, assistdept_amt = 0, deptmonth_status = 0, checkpend_amt = 0, laststmseq_no = 0, dept_amt = 0;
            DateTime operate_date = DwSlip.GetItemDate(1, "entry_date");
            DateTime entry_date = DateTime.Now;
            try
            {
                member_no = DwMain.GetItemString(1, "member_no");
                String SqlSelectData = "select dp.prncbal as prncbal , asn.assistdept_amt as assistdept_amt, dp.deptmonth_status as deptmonth_status, dp.checkpend_amt as checkpend_amt , dp.laststmseq_no as laststmseq_no , asn.deptaccount_no as deptaccount_no from dpdeptmaster dp , asnreqmaster asn where dp.deptaccount_no = asn.deptaccount_no and asn.member_no = '" + member_no + "' and asn.assisttype_code = '70' ";
                Sdt dtAsn = WebUtil.QuerySdt(SqlSelectData);
                if (dtAsn.Next())
                {
                    bfprncbal = dtAsn.GetDecimal("prncbal");
                    assistdept_amt = dtAsn.GetDecimal("assistdept_amt");
                    deptmonth_status = dtAsn.GetDecimal("deptmonth_status");
                    checkpend_amt = dtAsn.GetDecimal("checkpend_amt");
                    laststmseq_no = dtAsn.GetDecimal("laststmseq_no");
                    deptaccount_no = dtAsn.GetString("deptaccount_no").Trim();

                    dept_amt = DwSlip.GetItemDecimal(1, "dept_amt");

                    prncbal = bfprncbal - dept_amt;
                    assistdept_amt -= dept_amt;
                    laststmseq_no++;

                    if (prncbal >= checkpend_amt)
                        deptmonth_status = 0;

                    String SqlIns = "insert into dpdeptstatement(deptaccount_no,branch_id ,seq_no,deptitemtype_code,operate_date,ref_docno ,deptitem_amt,balance_forward,prncbal,check_status,item_status,entry_id,entry_date,cash_type)"
                                                      + "values ('" + deptaccount_no + "','000'," + laststmseq_no + ",'AJO',to_date('" + operate_date.ToString("dd/MM/yyyy") + "','dd/mm/yyyy'),'MODADMIN'," + dept_amt + "," + bfprncbal + "," + prncbal + ",1,1,'" + state.SsUsername + "',sysdate,'')";
                    WebUtil.Query(SqlIns);

                    String SqlUpDeptMaster = "update dpdeptmaster set prncbal	= " + prncbal + " , deptmonth_status = " + deptmonth_status + " ,laststmseq_no = " + laststmseq_no + " where deptaccount_no	= '" + deptaccount_no + "' ";
                    WebUtil.Query(SqlUpDeptMaster);

                    String SqlUpAsnMaster = "update asnreqmaster set assistdept_amt = " + prncbal + " where deptaccount_no	= " + deptaccount_no + " and assisttype_code = '70'";
                    WebUtil.Query(SqlUpAsnMaster);

                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกเรียบร้อย " + bfprncbal + " : " + assistdept_amt + " : " + checkpend_amt + " : " + deptaccount_no + " : " + dept_amt);
                }

            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ทำรายการไม่สำเร็จ : " + ex);
            }
        }
    }
}