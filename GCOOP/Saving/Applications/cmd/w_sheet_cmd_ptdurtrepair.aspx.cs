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
using DataLibrary;
using System.Globalization;
using System.Data.OracleClient;
using Sybase.DataWindow;
using CoreSavingLibrary.WcfNCommon;

namespace Saving.Applications.cmd
{
    public partial class w_sheet_cmd_ptdurtrepair : PageWebSheet, WebSheet
    {
        private DwThDate tDwMain;
        Sdt dt = new Sdt();
        String pbl = "cmd_ptdurtrepair.pbl";

        protected string jsPostFindShow;
        protected string jsPostDealer;
        protected string jsPostCheckrepairstatus;
        protected string jsPostReqrepairno;
        protected string jsReqrepairToContract;
        protected string jsPostPrint;

        public void InitJsPostBack()
        {
            tDwMain = new DwThDate(DwMain, this);
            tDwMain.Add("reqrepair_date", "reqrepair_tdate");
            tDwMain.Add("apprepair_date", "apprepair_tdate");
            tDwMain.Add("startrepair_date", "startrepair_tdate");
            tDwMain.Add("endrepair_date", "endrepair_tdate");

            jsPostFindShow = WebUtil.JsPostBack(this, "jsPostFindShow");
            jsPostDealer = WebUtil.JsPostBack(this, "jsPostDealer");
            jsPostCheckrepairstatus = WebUtil.JsPostBack(this, "jsPostCheckrepairstatus");
            jsPostReqrepairno = WebUtil.JsPostBack(this, "jsPostReqrepairno");
            jsReqrepairToContract = WebUtil.JsPostBack(this, "jsReqrepairToContract");
            jsPostPrint = WebUtil.JsPostBack(this, "jsPostPrint");
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
                case "jsPostFindShow":
                    PostFindShow();
                    break;
                case "jsPostDealer":
                    PostDealer();
                    break;
                case "jsPostCheckrepairstatus":
                    decimal repair_status = DwMain.GetItemDecimal(1, "repair_status");
                    if (repair_status == 0)
                    {
                        DwMain.SetItemDateTime(1, "apprepair_date", state.SsWorkDate); 
                    }
                    break;
                case "jsPostReqrepairno":
                        PostInitReqrepairno(DwMain.GetItemString(1, "reqrepair_no").Trim());
                    break;
                case "jsReqrepairToContract":
                    PostReqrepairToContract();
                    break;
                case "jsPostPrint":
                    PrintReqRepair(HdReqrepairNo.Value);
                    break;
            }
        }

        public void SaveWebSheet()
        {
            string reqrepair_no ="", reqrepair_name ="", reqrepair_tel ="", reqrepair_email ="", durt_id ="", repair_detail ="";
            string contact_name = "", contact_tel = "", contact_email = "";
            string repairman_name = "", repairman_tel = "", repairman_email = "", repairman_addr = "", retails_defective = "";
            string branch_code = "", dealer_no = "";
            DateTime reqrepair_date = new DateTime(), apprepair_date = new DateTime();
            DateTime startrepair_date = new DateTime(), endrepair_date = new DateTime();
            decimal repair_status = 0, repair_amt = 0;
            Sdt ta1, ta2;
            try
            {
                reqrepair_no = DwMain.GetItemString(1, "reqrepair_no");
                reqrepair_date = DwMain.GetItemDateTime(1, "reqrepair_date");
                durt_id = DwMain.GetItemString(1, "durt_id");
                reqrepair_name = DwMain.GetItemString(1, "reqrepair_name");
                try { reqrepair_tel = DwMain.GetItemString(1, "reqrepair_tel"); }
                catch { reqrepair_tel = string.Empty; }
                try { reqrepair_email = DwMain.GetItemString(1, "reqrepair_email"); }
                catch { reqrepair_email = string.Empty; }
                try { contact_name = DwMain.GetItemString(1, "contact_name"); }
                catch { contact_name = string.Empty; }
                try { contact_tel = DwMain.GetItemString(1, "contact_tel"); }
                catch { contact_tel = string.Empty; }
                try { contact_email = DwMain.GetItemString(1, "contact_email"); }
                catch { contact_email = string.Empty; }
                try { repairman_name = DwMain.GetItemString(1, "repairman_name"); }
                catch { repairman_name = string.Empty; }
                try { repairman_tel = DwMain.GetItemString(1, "repairman_tel"); }
                catch { repairman_tel = string.Empty; }
                try { repairman_email = DwMain.GetItemString(1, "repairman_email"); }
                catch { repairman_email = string.Empty; }
                try { repairman_addr = DwMain.GetItemString(1, "repairman_addr"); }
                catch {repairman_addr = string.Empty; }
                repair_detail = DwMain.GetItemString(1, "repair_detail");
                try { retails_defective = DwMain.GetItemString(1, "retails_defective"); }
                catch { retails_defective = repair_detail; }
                try { apprepair_date = DwMain.GetItemDateTime(1, "apprepair_date"); }
                catch { }
                try { startrepair_date = DwMain.GetItemDateTime(1, "startrepair_date"); }
                catch { }
                try { endrepair_date = DwMain.GetItemDateTime(1, "endrepair_date"); }
                catch { }
                repair_status = DwMain.GetItemDecimal(1, "repair_status");
                try { branch_code = DwMain.GetItemString(1, "branch_code"); }
                catch { branch_code = "001"; }
                try { dealer_no = DwMain.GetItemString(1, "dealer_no"); }
                catch { dealer_no = string.Empty; }
                try { repair_amt = DwMain.GetItemDecimal(1, "repair_amt"); }
                catch { repair_amt = 0; }

                if (reqrepair_no == "AUTO")
                {
                    n_commonClient com = wcf.NCommon;
                    reqrepair_no = com.of_getnewdocno(state.SsWsPass, state.SsCoopId, "CMDREQREPAIR");
                    string insertreqRepair = @"insert into ptreqdurtrepair (reqrepair_no, reqrepair_date, durt_id,
                            reqrepair_name, reqrepair_tel, reqrepair_email, contact_name, contact_tel, contact_email,
                            repairman_name, repairman_tel, repairman_email, repairman_addr, repair_detail, retails_defective,
                            apprepair_date, startrepair_date, endrepair_date, repair_status, branch_code, dealer_no, repair_amt)
                            values ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, 
                            {12}, {13}, {14}, {15}, {16}, {17}, {18}, {19}, {20}, {21})";
                    insertreqRepair = WebUtil.SQLFormat(insertreqRepair, reqrepair_no, reqrepair_date, durt_id,
                        reqrepair_name, reqrepair_tel, reqrepair_email, contact_name, contact_tel, contact_email,
                        repairman_name, repairman_tel, repairman_email, repairman_addr, repair_detail, retails_defective,
                        apprepair_date, startrepair_date, endrepair_date, repair_status, branch_code, dealer_no, repair_amt);
                    ta1 = WebUtil.QuerySdt(insertreqRepair);
                    if (repair_status == 0)
                    {
                        PostUpDurtMast(durt_id, 5);
                    }
                    else if (repair_status == 9)
                    {
                        PostUpDurtMast(durt_id, 9);
                    }
                    else if (repair_status == 1)
                    {
                        PostUpDurtMast(durt_id, 1);
                    }
                    PrintReqRepair(reqrepair_no);
                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ เลขที่ " + reqrepair_no);
                }
                else
                {
                    string upreqRepair = @"update ptreqdurtrepair set contact_name = {1}, contact_tel = {2}, 
                            contact_email = {3}, repairman_name = {4}, repairman_tel = {5}, repairman_email = {6}, 
                            repairman_addr = {7}, repair_detail = {8}, retails_defective = {9}, apprepair_date = {10}, 
                            startrepair_date = {11}, endrepair_date = {12}, repair_status = {13}, branch_code = {14}, 
                            repair_amt = {15} where reqrepair_no = {0}";
                    upreqRepair = WebUtil.SQLFormat(upreqRepair, reqrepair_no, contact_name, contact_tel, contact_email,
                        repairman_name, repairman_tel, repairman_email, repairman_addr, repair_detail, retails_defective,
                        apprepair_date, startrepair_date, endrepair_date, repair_status, branch_code, repair_amt);
                    ta2 = WebUtil.QuerySdt(upreqRepair);
                    if (repair_status == 0)
                    {
                        PostUpDurtMast(durt_id, 5);
                    }
                    else if (repair_status == 9)
                    {
                        PostUpDurtMast(durt_id, 9);
                    }
                    else if (repair_status == 1)
                    {
                        PostUpDurtMast(durt_id, 1);
                    }
                    HdState.Value = "Update";
                    HdReqrepairNo.Value = reqrepair_no;
                    LtServerMessage.Text = WebUtil.CompleteMessage("ปรับปรุงสำเร็จ เลขที่ " + reqrepair_no);
                }
                PostReset();
            }
            catch (Exception ex)
            { LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message); }
        }

        public void WebSheetLoadEnd()
        {
            tDwMain.Eng2ThaiAllRow();
            DwMain.SaveDataCache();
        }

        public void PostReset()
        {
            DwMain.Reset();
            DwMain.InsertRow(0);
            DwMain.SetItemString(1, "reqrepair_no", "AUTO");
            DwMain.SetItemDateTime(1, "reqrepair_date", state.SsWorkDate);
            DwMain.SetItemDecimal(1, "repair_status", 8);
            DwMain.SetItemDecimal(1, "repair_amt", 0);
            DwUtil.RetrieveDDDW(DwMain, "branch_code", pbl, null); 
        }

        private void PostFindShow()
        {
            string durt_id = "", durt_regno = "", durt_name = "", brand_name = "", model_desc = "", durtserial_number = "";
            decimal durt_status = 0;
            durt_id = DwMain.GetItemString(1, "durt_id");
            try
            {
                string se = @"select durt_id, durt_regno, durt_name, brand_name, model_desc, durtserial_number, durt_status 
                from ptdurtmaster where durt_id = {0}";
                se = WebUtil.SQLFormat(se, durt_id);
                dt = WebUtil.QuerySdt(se);
                if (dt.Next())
                {
                    durt_regno = dt.GetString("durt_regno").Trim();
                    durt_name = dt.GetString("durt_name");
                    brand_name = dt.GetString("brand_name");
                    model_desc = dt.GetString("model_desc");
                    durtserial_number = dt.GetString("durtserial_number");
                    durt_status = dt.GetDecimal("durt_status");
                }
                if (durt_status == 1)
                {
                    DwMain.SetItemString(1, "durt_regno", durt_regno);
                    DwMain.SetItemString(1, "durt_name", durt_name);
                    DwMain.SetItemString(1, "brand_name", brand_name);
                    DwMain.SetItemString(1, "model_desc", model_desc);
                    DwMain.SetItemString(1, "durtserial_number", durtserial_number);
                }
                else if (durt_status == 5 || durt_status == 9)
                {
                    DwMain.SetItemString(1, "durt_regno", "");
                    DwMain.SetItemString(1, "durt_name", "");
                    DwMain.SetItemString(1, "brand_name", "");
                    DwMain.SetItemString(1, "model_desc", "");
                    DwMain.SetItemString(1, "durtserial_number", "");
                    this.SetOnLoadedScript("alert('เลขครุภัณฑ์เลขที่ " + durt_regno + " อยู่ในสถานะ\n ส่งซ่อม หรือ รอตัดจำหน่ายแล้ว')");
                }
            }
            catch (Exception ex)
            { LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message); }
        }

        private void PostDealer()
        {
            string dealer_no = "", dealer_name = "", repairman_tel = "", repairman_email = "", repairman_addr = "";
            string a = "";
            try
            {
                dealer_no = DwMain.GetItemString(1, "dealer_no");
                string sqlsedealer = @"select dealer_name, dealer_addr, dealer_phone, dealer_email 
                        from ptucfdealer where dealer_no = {0}";
                sqlsedealer = WebUtil.SQLFormat(sqlsedealer, dealer_no);
                dt = WebUtil.QuerySdt(sqlsedealer);
                if (dt.Next())
                {
                    dealer_name = dt.GetString("dealer_name").Trim();
                    repairman_addr = dt.GetString("dealer_addr").Trim();
                    repairman_tel = dt.GetString("dealer_phone").Trim();
                    repairman_email = dt.GetString("dealer_email").Trim();
                }
                if (dealer_name == null || dealer_name == string.Empty)
                {
                    throw new Exception("หมายเลขคู่ค้าเลขที่ " + dealer_no + " ไม่มีข้อมูล");
                }
                else
                {
                    decimal ckdeal = 0;
                    string sechkdeal = @"select count(1) as ckcount from ptucfdealer 
                                where dealer_contractno1 is not null and
                                dealer_contractno2 is not null
                                and dealer_no = {0}";
                    sechkdeal = WebUtil.SQLFormat(sechkdeal, dealer_no);
                    Sdt dt2 = WebUtil.QuerySdt(sechkdeal);
                    if (dt2.Next())
                    {
                        ckdeal = dt2.GetDecimal("ckcount");
                    }
                    if (ckdeal > 0)
                    {
                        HdDealerNo.Value = dealer_no;
                        HdCkdeal.Value = "true";
                    }
                    {
                        DwMain.SetItemString(1, "repairman_name", dealer_name);
                        DwMain.SetItemString(1, "repairman_addr", repairman_addr);
                        DwMain.SetItemString(1, "repairman_tel", repairman_tel);
                        DwMain.SetItemString(1, "repairman_email", repairman_email);
                    }
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
                DwMain.SetItemString(1, "dealer_no", "");
                DwMain.SetItemString(1, "repairman_name", "");
            }
        }

        private void PostUpDurtMast(string durtId, decimal durtState)
        {
            try
            {
                string upstatedurt = @"update ptdurtmaster set durt_status = {1} where durt_id = {0}";
                upstatedurt = WebUtil.SQLFormat(upstatedurt, durtId, durtState);
                dt = WebUtil.QuerySdt(upstatedurt);
            }
            catch (Exception ex)
            { LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message); }
        }

        private void PostInitReqrepairno(string reqrepair_no)
        {
            DwUtil.RetrieveDataWindow(DwMain, pbl, null, reqrepair_no);
            HdState.Value = "Update";
            HdReqrepairNo.Value = DwMain.GetItemString(1, "reqrepair_no");
        }

        private void PostReqrepairToContract()
        {
            string reqrepair_name = "", reqrepair_tel = "", reqrepair_email = "";
            try
            {

                try { reqrepair_name = DwMain.GetItemString(1, "reqrepair_name"); }
                catch { throw new Exception("กรุณาระบุชื่อผู้แจ้งซ่อม !!!"); }
                try { reqrepair_tel = DwMain.GetItemString(1, "reqrepair_tel"); }
                catch { throw new Exception("กรุณาระบุเบอร์ติดต่อผู้แจ้ง !!!"); }
                try { reqrepair_email = DwMain.GetItemString(1, "reqrepair_email"); }
                catch { reqrepair_email = ""; }
                DwMain.SetItemString(1, "contact_name", reqrepair_name);
                DwMain.SetItemString(1, "contact_tel", reqrepair_tel);
                DwMain.SetItemString(1, "contact_email", reqrepair_email);
            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.WarningMessage(ex); }
        }

        private void PrintReqRepair(string reqrepair_no)
        {
            try
            {
                iReportArgument args = new iReportArgument();
                args.Add("as_reqrepairno", iReportArgumentType.String, reqrepair_no);
                iReportBuider report = new iReportBuider(this, "ใบแจ้งซ่อมครุภัณฑ์");
                report.AddCriteria("r_slip_cmdreq_repair_msv", "ใบแจ้งซ่อมครุภัณฑ์", ReportType.pdf, args);
                report.AutoOpenPDF = true;
                report.Retrieve();
                LtServerMessage.Text = WebUtil.CompleteMessage("ใบแจ้งซ่อมเลขที่ " + reqrepair_no);
                HdState.Value = "";
                PostReset();
            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message); }
        }
    }
}