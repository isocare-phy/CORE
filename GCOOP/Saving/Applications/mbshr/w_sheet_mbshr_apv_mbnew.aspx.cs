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
//using CoreSavingLibrary.WcfShrlon;

using CoreSavingLibrary.WcfNShrlon;
using Sybase.DataWindow;
using DataLibrary;

namespace Saving.Applications.mbshr
{
    public partial class w_sheet_mbshr_apv_mbnew : PageWebSheet, WebSheet
    {
        //private ShrlonClient shrlonService;
        private n_shrlonClient shrlonService;
        private DwThDate tDwOption;
        private DwThDate tDwList;
        public String pbl = "mb_apvmbnew.pbl";
        protected String postInit;
        protected String postSetStatus;
        protected String postRequestStatus;
        protected String postSetMemberNo;
        //===========================
        public void InitJsPostBack()
        {
            postInit = WebUtil.JsPostBack(this, "postInit");
            postSetStatus = WebUtil.JsPostBack(this, "postSetStatus");
            postRequestStatus = WebUtil.JsPostBack(this, "postRequestStatus");
            postSetMemberNo = WebUtil.JsPostBack(this, "postSetMemberNo");
            ///===========================
            tDwOption = new DwThDate(Dw_option, this);
            tDwOption.Add("apply_sdate", "apply_stdate");
            tDwOption.Add("apply_edate", "apply_etdate");
            tDwOption.Add("membdatefix_sdate", "membdatefix_stdate");
            tDwOption.Add("membdatefix_edate", "membdatefix_etdate");

            tDwList = new DwThDate(Dw_list, this);
            tDwList.Add("approve_date", "approve_tdate");
        }

        public void WebSheetLoadBegin()
        {
            try
            {
                //shrlonService = wcf.NShrlon;
                shrlonService = wcf.NShrlon;
            }
            catch
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ติดต่อ Web Service ไม่ได้");
            }

            if (!IsPostBack)
            {
                JspostNewClear();
            }
            else
            {
                this.RestoreContextDw(Dw_option);
                this.RestoreContextDw(Dw_list);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postInit")
            {
                JspostInit();
            }
            else if (eventArg == "postSetStatus")
            {
                JspostSetStatus();
            }
            else if (eventArg == "postRequestStatus")
            {
                JspostRequestStatus();
            }
            else if (eventArg == "postSetMemberNo")
            {
                JspostSetMemberNo();
            }
        }

        public void SaveWebSheet()
        {
            try
            {
                str_mbreqnew astr_mbreqnew = new str_mbreqnew();
                JspostSetMemberNo();
                astr_mbreqnew.xml_reqlist = Dw_list.Describe("DataWindow.Data.XML");
                int result = shrlonService.of_saveapv_mbnew(state.SsWsPass, ref astr_mbreqnew);
                if (result == 1)
                {
                    string sql_update = "update shsharemaster set payment_status = -1 where member_No in  (select member_no from mbmembmaster where memref_flag = 0 and member_No like 'S%')";
                    WebUtil.Query(sql_update); //สมทบ งวดเก็บ หุ้น

                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลเรียบร้อยแล้ว");
                    JspostNewClear();
                }
            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message); }
        }

        public void WebSheetLoadEnd()
        {
            Dw_option.SaveDataCache();
            Dw_list.SaveDataCache();
        }

        //=====================
        private void JspostNewClear()
        {
            try
            {
                Dw_option.Reset();
                Dw_option.InsertRow(0);
                //Dw_option.SetItemDate(1, "apply_sdate", state.SsWorkDate);
                //Dw_option.SetItemDate(1, "apply_edate", state.SsWorkDate);

                //Dw_option.SetItemDate(1, "membdatefix_sdate", state.SsWorkDate);
                //Dw_option.SetItemDate(1, "membdatefix_edate", state.SsWorkDate);
                tDwOption.Eng2ThaiAllRow();


                DwUtil.RetrieveDDDW(Dw_option, "membgroup_scode_1", pbl, state.SsCoopId);
                DwUtil.RetrieveDDDW(Dw_option, "membgroup_ecode_1", pbl, state.SsCoopId);
                string[] minmaxmemgroup = ReportUtil.GetMinMaxMembgroup();
                Dw_option.SetItemString(1, "membgroup_scode_1", "");
                Dw_option.SetItemString(1, "membgroup_ecode_1", "");



                Dw_list.Reset();
            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message); }
        }

        private void JspostInit()
        {
            decimal ldc_mbnofix = 0;
            try
            {
                str_mbreqnew astr_mbreqnew = new str_mbreqnew();
                astr_mbreqnew.xml_reqopt = Dw_option.Describe("DataWindow.Data.XML");
                //int result = shrlonService.InitApvNewMemberList(state.SsWsPass, ref astr_mbreqnew);
                int result = shrlonService.of_initlist_apvmbnew(state.SsWsPass, ref astr_mbreqnew);
                if (result == 1)
                {
                    Dw_list.Reset();
                    DwUtil.ImportData(astr_mbreqnew.xml_reqlist, Dw_list, null, FileSaveAsType.Xml);

                    for (int i = 1; i <= Dw_list.RowCount; i++)
                    {
                        try { ldc_mbnofix = Dw_list.GetItemDecimal(i, "memnofix_flag"); }
                        catch { ldc_mbnofix = 0; }

                        if (ldc_mbnofix == 0)
                        {
                            Dw_list.SetItemString(i, "member_no", "AUTO");
                        }
                    }
                }
            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message); }
        }

        private void JspostSetStatus()
        {
            try
            {
                String btn_name = Hdbutton.Value.Trim();
                //รออนุมัติ
                if (btn_name == "b_wait")
                {
                    for (int i = 1; i <= Dw_list.RowCount; i++)
                    {
                        Dw_list.SetItemString(i, "member_no", "AUTO");
                        Dw_list.SetItemString(i, "approve_id", state.SsUsername);
                        Dw_list.SetItemString(i, "coop_id", state.SsCoopId);
                        Dw_list.SetItemDecimal(i, "appl_status", 8);
                        Dw_list.SetItemString(i, "approve_tdate", "00/00/0000");
                        tDwList.Thai2EngAllRow();
                    }
                }
                //อนุมติ
                else if (btn_name == "b_apv")
                {
                    for (int i = 1; i <= Dw_list.RowCount; i++)
                    {
                        Dw_list.SetItemString(i, "approve_id", state.SsUsername);
                        Dw_list.SetItemString(i, "coop_id", state.SsCoopId);
                        Dw_list.SetItemDecimal(i, "appl_status", 1);
                        Dw_list.SetItemDate(i, "approve_date", state.SsWorkDate);
                        tDwList.Eng2ThaiAllRow();
                    }
                }
                //ไม่อนุมัติ
                else if (btn_name == "b_noapv")
                {
                    for (int i = 1; i <= Dw_list.RowCount; i++)
                    {
                        Dw_list.SetItemString(i, "member_no", "AUTO");
                        Dw_list.SetItemString(i, "approve_id", state.SsUsername);
                        Dw_list.SetItemString(i, "coop_id", state.SsCoopId);
                        Dw_list.SetItemDecimal(i, "appl_status", 0);
                        Dw_list.SetItemDate(i, "approve_date", state.SsWorkDate);
                        tDwList.Eng2ThaiAllRow();
                    }
                }
            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message); }
        }

        private void JspostRequestStatus()
        {
            try
            {
                int rowcurrent = int.Parse(HdRow.Value);
                Decimal request_status = Dw_list.GetItemDecimal(rowcurrent, "appl_status");
                if (request_status == 1)
                {
                    Dw_list.SetItemString(rowcurrent, "approve_id", state.SsUsername);
                    Dw_list.SetItemString(rowcurrent, "coop_id", state.SsCoopId);
                    Dw_list.SetItemDecimal(rowcurrent, "appl_status", 1);
                    Dw_list.SetItemDate(rowcurrent, "approve_date", state.SsWorkDate);
                    tDwList.Eng2ThaiAllRow();
                }
                else if (request_status == 0)
                {
                    Dw_list.SetItemString(rowcurrent, "member_no", "AUTO");
                    Dw_list.SetItemString(rowcurrent, "approve_id", state.SsUsername);
                    Dw_list.SetItemString(rowcurrent, "coop_id", state.SsCoopId);
                    Dw_list.SetItemDecimal(rowcurrent, "appl_status", 0);
                    Dw_list.SetItemDate(rowcurrent, "approve_date", state.SsWorkDate);
                    tDwList.Eng2ThaiAllRow();
                }
                else
                {
                    Dw_list.SetItemString(rowcurrent, "member_no", "AUTO");
                    Dw_list.SetItemString(rowcurrent, "approve_id", state.SsUsername);
                    Dw_list.SetItemString(rowcurrent, "coop_id", state.SsCoopId);
                    Dw_list.SetItemDecimal(rowcurrent, "appl_status", 8);
                    Dw_list.SetItemString(rowcurrent, "approve_tdate", "00/00/0000");
                    tDwList.Thai2EngAllRow();
                }
            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message); }
        }

        private void JspostSetMemberNo()
        {
            try
            {
                for (int i = 1; i <= Dw_list.RowCount; i++)
                {
                    decimal appl_status = Dw_list.GetItemDecimal(i, "appl_status");
                    decimal member_type = Dw_list.GetItemDecimal(i, "member_type");
                    string document_code = "", ls_memberno, member_no = "";
                    decimal last_documentno = 0, last_member = 0;

                    //สถานะอนุมติ
                    if (appl_status == 1)
                    {
                        //สมาชิกปกติ
                        if (member_type == 1)
                        {
                            document_code = "MBMEMBERNO";
                        }
                        //สมาชิกสมทบ
                        else if (member_type >= 2)
                        {
                            document_code = "MBMEMBERCONO";
                            int checkdocno = JsCheckDocNo(document_code, state.SsCoopControl);
                            if (checkdocno == 1)
                            {
                                document_code = "MBMEMBERNO";
                            }
                        }

                        //ตรวจสอบว่ามีการกำหนดเลขไปหรือยัง
                        member_no = Dw_list.GetItemString(i, "member_no").Trim();
                        if (member_no == "AUTO")
                        {
                            //หาเลขลำดับล่าสุด
                            try
                            {
                                String sql = "";
                                sql = @"select last_documentno from cmdocumentcontrol where coop_id = '" + state.SsCoopId + @"' 
                                        and document_code = '" + document_code + "'";
                                Sdt dt = WebUtil.QuerySdt(sql);
                                if (dt.Next())
                                {
                                    last_documentno = Convert.ToDecimal(dt.GetString("last_documentno"));
                                }
                            }
                            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message); }

                            last_member = last_documentno + 1;
                            ls_memberno = Convert.ToString(last_member);

                            //สมาชิก สมทบ
                            if (member_type == 2)
                            {
                                    ls_memberno = WebUtil.MemberNoFormat(ls_memberno);
                                    ls_memberno = "S" + ls_memberno.Substring(1, ls_memberno.Length - 1);
                            }
                            ls_memberno = WebUtil.MemberNoFormat(ls_memberno);
                            Dw_list.SetItemString(i, "member_no", ls_memberno);

                            String sqlupdate = "update cmdocumentcontrol set last_documentno = '" + last_member + "' where document_code = '" + document_code + "'and coop_id = '" + state.SsCoopControl + "'";
                            WebUtil.Query(sqlupdate);
                        }
                    }
                }
            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message); }
        }

        private int JsCheckDocNo(string docNo, string coopId)
        {
            int checkdocno_status = 0;
            decimal ccheckdocno = 0;
            string sqlse = @"select count(1) as ccheckdocno from cmdocumentcontrol where document_code = {0} and coop_id = {1}";
            sqlse = WebUtil.SQLFormat(sqlse, docNo, coopId);
            Sdt ta = WebUtil.QuerySdt(sqlse);
            if (ta.Next())
            {
                ccheckdocno = ta.GetDecimal("ccheckdocno");
            }
            if (ccheckdocno > 0)
            { checkdocno_status = -9; }
            else
            { checkdocno_status = 1; }
            return checkdocno_status;
        }
    }
}