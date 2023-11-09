using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary.WcfNShrlon;
using CoreSavingLibrary.WcfNCommon;
using Sybase.DataWindow;

namespace Saving.Applications.shrlon
{
    public partial class w_sheet_regloan_year : PageWebSheet, WebSheet
    {
        private DwThDate tDwMain;
        private n_shrlonClient shrlonService;
        private n_commonClient commonService;
        protected String openNew;
        protected String jsPostMember = "";
        protected string jsLoanreq_dif;
        protected String jsLoanreq_amt;
        protected String jsSetloanreqAmt;
        protected String jsRefresh;
        protected String jsPostMemberColl;
        protected String setloanreq_year;
        protected String getLoanrequest;
        protected String setcollname;
        String as_collmemname = "";
        public void InitJsPostBack()
        {
            jsPostMember = WebUtil.JsPostBack(this, "jsPostMember");
            jsLoanreq_dif = WebUtil.JsPostBack(this, "jsLoanreq_dif");
            jsLoanreq_amt = WebUtil.JsPostBack(this, "jsLoanreq_amt");
            jsRefresh = WebUtil.JsPostBack(this, "jsRefresh");
            jsSetloanreqAmt = WebUtil.JsPostBack(this, "jsSetloanreqAmt");
            jsPostMemberColl = WebUtil.JsPostBack(this, "jsPostMemberColl");
            openNew = WebUtil.JsPostBack(this, "openNew");
            setloanreq_year = WebUtil.JsPostBack(this, "setloanreq_year");
            getLoanrequest = WebUtil.JsPostBack(this, "getLoanrequest");
            setcollname = WebUtil.JsPostBack(this, "setcollname");
            //----------------------------------------------------------
            tDwMain = new DwThDate(dw_main, this);
            tDwMain.Add("loanreq_date", "loanreq_tdate");

        }

        public void WebSheetLoadBegin()
        {
            try
            {
                shrlonService = wcf.NShrlon;
                commonService = wcf.NCommon;
            }
            catch
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ติดต่อ Web Service ไม่ได้");
                return;
            }

            this.ConnectSQLCA();

            if (IsPostBack)
            {
                this.RestoreContextDw(dw_main, tDwMain);
                this.RestoreContextDw(dw_oldreq);
               // Has_errtext.Value = "";
                HdIsPostBack.Value = "true";
            }
            if (dw_main.RowCount < 1)
            {
                InitDataWindow();
                dw_message.DisplayOnly = false;
           //     Has_errtext.Value = "";
                HdIsPostBack.Value = "false";
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "jsPostMember")
            {
                this.JsPostMember();

            }
            else if (eventArg == "jsLoanreq_dif")
            {
                JsLoanreq_dif();
                //  HdIsPostBack.Value = "false";
            }
            else if (eventArg == "jsLoanreq_amt")
            {
                JsLoanreq_amt();
                HdIsPostBack.Value = "false";
            }
            else if (eventArg == "jsRefresh")
            {
                JsRefresh();
                HdIsPostBack.Value = "false";
            }
            else if (eventArg == "jsSetloanreqAmt")
            {
                JsSetloanreqAmt();
                HdIsPostBack.Value = "false";
            }
            else if (eventArg == "jsPostMemberColl")
            {
                JsPostMemberColl();
                //HdIsPostBack.Value = "false";

            }
            else if (eventArg == "openNew")
            {
                JsOpenNew();
                HdIsPostBack.Value = "false";

            }
            else if (eventArg == "setloanreq_year")
            {
                HdIsPostBack.Value = "false";
            }
            else if (eventArg == "getLoanrequest")
            {

                GetLoanrequest();
            }
            else if (eventArg == "setcollname") {
                dw_main.SetItemString(1, "coll_name", as_collmemname);
            }
        }

        private void GetLoanrequest()
        {

            try
            {
                String member_no = HdMemberNo.Value;// dw_main.GetItemString(1, "member_no");
                String xml_newlnyear = dw_main.Describe("DataWindow.Data.XML");
                String xml_oldlnyear = dw_oldreq.Describe("DataWindow.Data.XML");
                Int16 lnreq_year = Convert.ToInt16(dw_main.GetItemDecimal(1, "loanreq_year"));
                Int16 month_start = Convert.ToInt16(dw_main.GetItemDecimal(1, "loanreq_mthstart"));
                DateTime lnreq_date = state.SsWorkDate;
                String entry_id = state.SsUsername;
                String branch_id = state.SsCoopId;

                str_lnreqyear astr_lnreqyear = new str_lnreqyear();
                astr_lnreqyear.lnreq_docno = Hloanrequest_docno.Value;
                astr_lnreqyear.member_no = member_no;
                astr_lnreqyear.xml_newlnyear = xml_newlnyear;
                astr_lnreqyear.xml_oldlnyear = xml_oldlnyear;
                astr_lnreqyear.lnreq_date = lnreq_date;
                astr_lnreqyear.lnreq_year = lnreq_year;
                astr_lnreqyear.month_start = month_start;
                astr_lnreqyear.entry_id = entry_id;
                astr_lnreqyear.branch_id = branch_id;
                int result = shrlonService.of_openreq_loanyear(state.SsWsPass, ref astr_lnreqyear);
                if (result == 1)
                {
                    try
                    {
                        dw_main.Reset();
                        DwUtil.ImportData(astr_lnreqyear.xml_newlnyear, dw_main, tDwMain, FileSaveAsType.Xml);
                        //dw_main.ImportString(astr_lnreqyear.xml_newlnyear, FileSaveAsType.Xml);
                        dw_main.SetItemDateTime(1, "loanreq_date", state.SsWorkDate);
                        tDwMain.Eng2ThaiAllRow();
                        HdMemberNo.Value = dw_main.GetItemString(1, "member_no");
                    }
                    catch (Exception ex)
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                        dw_main.Reset(); dw_main.InsertRow(0);
                    }
                    try
                    {
                        dw_oldreq.Reset();
                        DwUtil.ImportData(astr_lnreqyear.xml_oldlnyear, dw_oldreq, null, FileSaveAsType.Xml);
                        //dw_oldreq.ImportString(astr_lnreqyear.xml_oldlnyear, FileSaveAsType.Xml);
                    }
                    catch (Exception ex)
                    {

                        if (astr_lnreqyear.xml_oldlnyear == "")
                        {

                        }
                        else
                        {
                            LtServerMessage.Text = WebUtil.ErrorMessage(ex);

                        }

                    }
                }
            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }





        }

        private void JsOpenNew()
        {
            dw_main.Reset();
            dw_oldreq.Reset();
            dw_main.InsertRow(0);
            Decimal loanreq_year = Convert.ToDecimal(Session["lnreq_year"]);
            Decimal loanreq_mthstart = Convert.ToDecimal(Session["month_start"]);
            Decimal loancontract_running = Convert.ToDecimal(Session["loancontract_running"]) + 1;
            dw_main.SetItemDecimal(1, "loanreq_year", loanreq_year);
            dw_main.SetItemDecimal(1, "loanreq_mthstart", loanreq_mthstart);
            dw_main.SetItemDecimal(1, "loancontract_running", loancontract_running);
            dw_main.SetItemDateTime(1, "loanreq_date", state.SsWorkDate);
            tDwMain.Eng2ThaiAllRow();
            DwUtil.RetrieveDDDW(dw_main, "loantype_code", "sl_loan_requestment.pbl", null);
            dw_oldreq.InsertRow(0);
            Has_errtext.Value = "";
            HdIsPostBack.Value = "true";
           HdMemberNo.Value= "";
        }

        private void JsPostMemberColl()
        {
            String member_no = HdMemberNo.Value;
            String reqgrt_memno = Hdreqgrt_memno.Value;// dw_main.GetItemString(1, "member_no");
            // String coll_name = "";
           // Has_errtext.Value = "";
            String as_errtext = "";
            short ai_year = Convert.ToInt16(dw_main.GetItemDecimal(1, "loanreq_year"));
            try
            {
                int result = shrlonService.of_checkcollmemno(state.SsWsPass, ai_year, member_no, reqgrt_memno, ref as_collmemname, ref as_errtext);
                dw_main.SetItemString(1, "reqgrt_memno", reqgrt_memno);

                if (result == -1)
                {
                    dw_message.ImportString(as_errtext, FileSaveAsType.Xml);
                    Hmsgtext.Value = dw_message.GetItemString(1, "msgtext");
                    Has_errtext.Value = "-1";
                    HdMsg.Value = as_collmemname;

                }
                else { dw_main.SetItemString(1, "coll_name", as_collmemname); }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                // dw_main.SetItemString(1, "reqgrt_memno", reqgrt_memno);
                //dw_main.SetItemString(1, "coll_name", "");
                //dw_main.SetItemString(1, "reqgrt_memno", "");

            }
         //   Has_errtext.Value = "";
            HdIsPostBack.Value = "false";
        }

        private void InitDataWindow()
        {
            try
            {

                dw_main.InsertRow(0);
                dw_main.SetItemDecimal(1, "loanreq_year", Convert.ToInt16(DateTime.Now.Year) + 543);
                dw_main.SetItemDecimal(1, "loanreq_mthstart", Convert.ToInt16(DateTime.Now.Month));
                dw_main.SetItemDateTime(1, "loanreq_date", state.SsWorkDate);
                tDwMain.Eng2ThaiAllRow();
                DwUtil.RetrieveDDDW(dw_main, "loantype_code", "sl_loan_requestment.pbl", null);
                dw_oldreq.InsertRow(0);

            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
        }


        private void JsSetloanreqAmt()
        {

            Int16 loanreq_mthstart = 1;
            loanreq_mthstart = Convert.ToInt16(dw_main.GetItemDecimal(1, "loanreq_mthstart"));
            Decimal loanreq_amt = 0;
            Decimal loanreq_type = dw_main.GetItemDecimal(1, "loanreq_type");
            loanreq_amt = dw_main.GetItemDecimal(1, "loanreq_amt");
            if (loanreq_type == 1)
            {
                SetmonthAmt_2();

                dw_main.Modify("loanreq_dif.protect =1");
                switch (loanreq_mthstart)
                {
                    case 1:
                        dw_main.SetItemDecimal(1, "month1_amt", loanreq_amt);
                        dw_main.SetItemDecimal(1, "month2_amt", loanreq_amt);
                        dw_main.SetItemDecimal(1, "month3_amt", loanreq_amt);
                        dw_main.SetItemDecimal(1, "month4_amt", loanreq_amt);
                        dw_main.SetItemDecimal(1, "month5_amt", loanreq_amt);
                        dw_main.SetItemDecimal(1, "month6_amt", loanreq_amt);
                        dw_main.SetItemDecimal(1, "month7_amt", loanreq_amt);
                        dw_main.SetItemDecimal(1, "month8_amt", loanreq_amt);
                        dw_main.SetItemDecimal(1, "month9_amt", loanreq_amt);
                        dw_main.SetItemDecimal(1, "month10_amt", loanreq_amt);
                        dw_main.SetItemDecimal(1, "month11_amt", loanreq_amt);
                        dw_main.SetItemDecimal(1, "month12_amt", loanreq_amt);
                        break;
                    case 2:
                        dw_main.Modify("month1_amt.protect =1");

                        dw_main.SetItemDecimal(1, "month2_amt", loanreq_amt);
                        dw_main.SetItemDecimal(1, "month3_amt", loanreq_amt);
                        dw_main.SetItemDecimal(1, "month4_amt", loanreq_amt);
                        dw_main.SetItemDecimal(1, "month5_amt", loanreq_amt);
                        dw_main.SetItemDecimal(1, "month6_amt", loanreq_amt);
                        dw_main.SetItemDecimal(1, "month7_amt", loanreq_amt);
                        dw_main.SetItemDecimal(1, "month8_amt", loanreq_amt);
                        dw_main.SetItemDecimal(1, "month9_amt", loanreq_amt);
                        dw_main.SetItemDecimal(1, "month10_amt", loanreq_amt);
                        dw_main.SetItemDecimal(1, "month11_amt", loanreq_amt);
                        dw_main.SetItemDecimal(1, "month12_amt", loanreq_amt);
                        break;
                    case 3:
                        dw_main.Modify("month1_amt.protect =1");
                        dw_main.Modify("month2_amt.protect =1");

                        dw_main.SetItemDecimal(1, "month3_amt", loanreq_amt);
                        dw_main.SetItemDecimal(1, "month4_amt", loanreq_amt);
                        dw_main.SetItemDecimal(1, "month5_amt", loanreq_amt);
                        dw_main.SetItemDecimal(1, "month6_amt", loanreq_amt);
                        dw_main.SetItemDecimal(1, "month7_amt", loanreq_amt);
                        dw_main.SetItemDecimal(1, "month8_amt", loanreq_amt);
                        dw_main.SetItemDecimal(1, "month9_amt", loanreq_amt);
                        dw_main.SetItemDecimal(1, "month10_amt", loanreq_amt);
                        dw_main.SetItemDecimal(1, "month11_amt", loanreq_amt);
                        dw_main.SetItemDecimal(1, "month12_amt", loanreq_amt);
                        break;
                    case 4:
                        dw_main.Modify("month1_amt.protect =1");
                        dw_main.Modify("month2_amt.protect =1");
                        dw_main.Modify("month3_amt.protect =1");

                        dw_main.SetItemDecimal(1, "month4_amt", loanreq_amt);
                        dw_main.SetItemDecimal(1, "month5_amt", loanreq_amt);
                        dw_main.SetItemDecimal(1, "month6_amt", loanreq_amt);
                        dw_main.SetItemDecimal(1, "month7_amt", loanreq_amt);
                        dw_main.SetItemDecimal(1, "month8_amt", loanreq_amt);
                        dw_main.SetItemDecimal(1, "month9_amt", loanreq_amt);
                        dw_main.SetItemDecimal(1, "month10_amt", loanreq_amt);
                        dw_main.SetItemDecimal(1, "month11_amt", loanreq_amt);
                        dw_main.SetItemDecimal(1, "month12_amt", loanreq_amt);
                        break;
                    case 5:
                        dw_main.Modify("month1_amt.protect =1");
                        dw_main.Modify("month2_amt.protect =1");
                        dw_main.Modify("month3_amt.protect =1");
                        dw_main.Modify("month4_amt.protect =1");

                        dw_main.SetItemDecimal(1, "month5_amt", loanreq_amt);
                        dw_main.SetItemDecimal(1, "month6_amt", loanreq_amt);
                        dw_main.SetItemDecimal(1, "month7_amt", loanreq_amt);
                        dw_main.SetItemDecimal(1, "month8_amt", loanreq_amt);
                        dw_main.SetItemDecimal(1, "month9_amt", loanreq_amt);
                        dw_main.SetItemDecimal(1, "month10_amt", loanreq_amt);
                        dw_main.SetItemDecimal(1, "month11_amt", loanreq_amt);
                        dw_main.SetItemDecimal(1, "month12_amt", loanreq_amt);
                        break;
                    case 6:
                        dw_main.Modify("month1_amt.protect =1");
                        dw_main.Modify("month2_amt.protect =1");
                        dw_main.Modify("month3_amt.protect =1");
                        dw_main.Modify("month4_amt.protect =1");
                        dw_main.Modify("month5_amt.protect =1");

                        dw_main.SetItemDecimal(1, "month6_amt", loanreq_amt);
                        dw_main.SetItemDecimal(1, "month7_amt", loanreq_amt);
                        dw_main.SetItemDecimal(1, "month8_amt", loanreq_amt);
                        dw_main.SetItemDecimal(1, "month9_amt", loanreq_amt);
                        dw_main.SetItemDecimal(1, "month10_amt", loanreq_amt);
                        dw_main.SetItemDecimal(1, "month11_amt", loanreq_amt);
                        dw_main.SetItemDecimal(1, "month12_amt", loanreq_amt);
                        break;
                    case 7:
                        dw_main.Modify("month1_amt.protect =1");
                        dw_main.Modify("month2_amt.protect =1");
                        dw_main.Modify("month3_amt.protect =1");
                        dw_main.Modify("month4_amt.protect =1");
                        dw_main.Modify("month5_amt.protect =1");
                        dw_main.Modify("month6_amt.protect =1");

                        dw_main.SetItemDecimal(1, "month7_amt", loanreq_amt);
                        dw_main.SetItemDecimal(1, "month8_amt", loanreq_amt);
                        dw_main.SetItemDecimal(1, "month9_amt", loanreq_amt);
                        dw_main.SetItemDecimal(1, "month10_amt", loanreq_amt);
                        dw_main.SetItemDecimal(1, "month11_amt", loanreq_amt);
                        dw_main.SetItemDecimal(1, "month12_amt", loanreq_amt);
                        break;
                    case 8:
                        dw_main.Modify("month1_amt.protect =1");
                        dw_main.Modify("month2_amt.protect =1");
                        dw_main.Modify("month3_amt.protect =1");
                        dw_main.Modify("month4_amt.protect =1");
                        dw_main.Modify("month5_amt.protect =1");
                        dw_main.Modify("month6_amt.protect =1");
                        dw_main.Modify("month7_amt.protect =1");

                        dw_main.SetItemDecimal(1, "month8_amt", loanreq_amt);
                        dw_main.SetItemDecimal(1, "month9_amt", loanreq_amt);
                        dw_main.SetItemDecimal(1, "month10_amt", loanreq_amt);
                        dw_main.SetItemDecimal(1, "month11_amt", loanreq_amt);
                        dw_main.SetItemDecimal(1, "month12_amt", loanreq_amt);
                        break;
                    case 9:
                        dw_main.Modify("month1_amt.protect =1");
                        dw_main.Modify("month2_amt.protect =1");
                        dw_main.Modify("month3_amt.protect =1");
                        dw_main.Modify("month4_amt.protect =1");
                        dw_main.Modify("month5_amt.protect =1");
                        dw_main.Modify("month6_amt.protect =1");
                        dw_main.Modify("month7_amt.protect =1");
                        dw_main.Modify("month8_amt.protect =1");

                        dw_main.SetItemDecimal(1, "month9_amt", loanreq_amt);
                        dw_main.SetItemDecimal(1, "month10_amt", loanreq_amt);
                        dw_main.SetItemDecimal(1, "month11_amt", loanreq_amt);
                        dw_main.SetItemDecimal(1, "month12_amt", loanreq_amt);
                        break;
                    case 10:
                        dw_main.Modify("month1_amt.protect =1");
                        dw_main.Modify("month2_amt.protect =1");
                        dw_main.Modify("month3_amt.protect =1");
                        dw_main.Modify("month4_amt.protect =1");
                        dw_main.Modify("month5_amt.protect =1");
                        dw_main.Modify("month6_amt.protect =1");
                        dw_main.Modify("month7_amt.protect =1");
                        dw_main.Modify("month8_amt.protect =1");
                        dw_main.Modify("month9_amt.protect =1");

                        dw_main.SetItemDecimal(1, "month10_amt", loanreq_amt);
                        dw_main.SetItemDecimal(1, "month11_amt", loanreq_amt);
                        dw_main.SetItemDecimal(1, "month12_amt", loanreq_amt);
                        break;
                    case 11:
                        dw_main.Modify("month1_amt.protect =1");
                        dw_main.Modify("month2_amt.protect =1");
                        dw_main.Modify("month3_amt.protect =1");
                        dw_main.Modify("month4_amt.protect =1");
                        dw_main.Modify("month5_amt.protect =1");
                        dw_main.Modify("month6_amt.protect =1");
                        dw_main.Modify("month7_amt.protect =1");
                        dw_main.Modify("month8_amt.protect =1");
                        dw_main.Modify("month9_amt.protect =1");
                        dw_main.Modify("month10_amt.protect =1");

                        dw_main.SetItemDecimal(1, "month11_amt", loanreq_amt);
                        dw_main.SetItemDecimal(1, "month12_amt", loanreq_amt);
                        break;
                    case 12:
                        dw_main.Modify("month1_amt.protect =1");
                        dw_main.Modify("month2_amt.protect =1");
                        dw_main.Modify("month3_amt.protect =1");
                        dw_main.Modify("month4_amt.protect =1");
                        dw_main.Modify("month5_amt.protect =1");
                        dw_main.Modify("month6_amt.protect =1");
                        dw_main.Modify("month7_amt.protect =1");
                        dw_main.Modify("month8_amt.protect =1");
                        dw_main.Modify("month9_amt.protect =1");
                        dw_main.Modify("month10_amt.protect =1");
                        dw_main.Modify("month11_amt.protect =1");

                        dw_main.SetItemDecimal(1, "month12_amt", loanreq_amt);
                        break;
                }
            }


            else if (loanreq_type == 2)
            {

                Decimal loanreq_dif = dw_main.GetItemDecimal(1, "loanreq_dif");

                //if (loanreq_dif == 0) 
                //{ 



                //}//end if กรณีลดไม่เท่ากัน
                //else
                //{

                SetmonthAmt_2();
                switch (loanreq_mthstart)
                {
                    case 1:
                        dw_main.SetItemDecimal(1, "month1_amt", loanreq_amt);
                        loanreq_amt = loanreq_amt - loanreq_dif; if (loanreq_amt < 0) { loanreq_amt = 0; }
                        dw_main.SetItemDecimal(1, "month2_amt", loanreq_amt);
                        loanreq_amt = loanreq_amt - loanreq_dif; if (loanreq_amt < 0) { loanreq_amt = 0; }
                        dw_main.SetItemDecimal(1, "month3_amt", loanreq_amt);
                        loanreq_amt = loanreq_amt - loanreq_dif; if (loanreq_amt < 0) { loanreq_amt = 0; }
                        dw_main.SetItemDecimal(1, "month4_amt", loanreq_amt);
                        loanreq_amt = loanreq_amt - loanreq_dif; if (loanreq_amt < 0) { loanreq_amt = 0; }
                        dw_main.SetItemDecimal(1, "month5_amt", loanreq_amt);
                        loanreq_amt = loanreq_amt - loanreq_dif; if (loanreq_amt < 0) { loanreq_amt = 0; }
                        dw_main.SetItemDecimal(1, "month6_amt", loanreq_amt);
                        loanreq_amt = loanreq_amt - loanreq_dif; if (loanreq_amt < 0) { loanreq_amt = 0; }
                        dw_main.SetItemDecimal(1, "month7_amt", loanreq_amt);
                        loanreq_amt = loanreq_amt - loanreq_dif; if (loanreq_amt < 0) { loanreq_amt = 0; }
                        dw_main.SetItemDecimal(1, "month8_amt", loanreq_amt);
                        loanreq_amt = loanreq_amt - loanreq_dif; if (loanreq_amt < 0) { loanreq_amt = 0; }
                        dw_main.SetItemDecimal(1, "month9_amt", loanreq_amt);
                        loanreq_amt = loanreq_amt - loanreq_dif; if (loanreq_amt < 0) { loanreq_amt = 0; }
                        dw_main.SetItemDecimal(1, "month10_amt", loanreq_amt);
                        loanreq_amt = loanreq_amt - loanreq_dif; if (loanreq_amt < 0) { loanreq_amt = 0; }
                        dw_main.SetItemDecimal(1, "month11_amt", loanreq_amt);
                        loanreq_amt = loanreq_amt - loanreq_dif; if (loanreq_amt < 0) { loanreq_amt = 0; }
                        dw_main.SetItemDecimal(1, "month12_amt", loanreq_amt);
                        break;
                    case 2:
                        dw_main.Modify("month1_amt.protect =1");

                        dw_main.SetItemDecimal(1, "month2_amt", loanreq_amt);
                        loanreq_amt = loanreq_amt - loanreq_dif; if (loanreq_amt < 0) { loanreq_amt = 0; }
                        dw_main.SetItemDecimal(1, "month3_amt", loanreq_amt);
                        loanreq_amt = loanreq_amt - loanreq_dif; if (loanreq_amt < 0) { loanreq_amt = 0; }
                        dw_main.SetItemDecimal(1, "month4_amt", loanreq_amt);
                        loanreq_amt = loanreq_amt - loanreq_dif; if (loanreq_amt < 0) { loanreq_amt = 0; }
                        dw_main.SetItemDecimal(1, "month5_amt", loanreq_amt);
                        loanreq_amt = loanreq_amt - loanreq_dif; if (loanreq_amt < 0) { loanreq_amt = 0; }
                        dw_main.SetItemDecimal(1, "month6_amt", loanreq_amt);
                        loanreq_amt = loanreq_amt - loanreq_dif; if (loanreq_amt < 0) { loanreq_amt = 0; }
                        dw_main.SetItemDecimal(1, "month7_amt", loanreq_amt);
                        loanreq_amt = loanreq_amt - loanreq_dif; if (loanreq_amt < 0) { loanreq_amt = 0; }
                        dw_main.SetItemDecimal(1, "month8_amt", loanreq_amt);
                        loanreq_amt = loanreq_amt - loanreq_dif; if (loanreq_amt < 0) { loanreq_amt = 0; }
                        dw_main.SetItemDecimal(1, "month9_amt", loanreq_amt);
                        loanreq_amt = loanreq_amt - loanreq_dif; if (loanreq_amt < 0) { loanreq_amt = 0; }
                        dw_main.SetItemDecimal(1, "month10_amt", loanreq_amt);
                        loanreq_amt = loanreq_amt - loanreq_dif; if (loanreq_amt < 0) { loanreq_amt = 0; }
                        dw_main.SetItemDecimal(1, "month11_amt", loanreq_amt);
                        loanreq_amt = loanreq_amt - loanreq_dif; if (loanreq_amt < 0) { loanreq_amt = 0; }
                        dw_main.SetItemDecimal(1, "month12_amt", loanreq_amt);


                        break;
                    case 3:
                        dw_main.Modify("month1_amt.protect =1");
                        dw_main.Modify("month2_amt.protect =1");

                        dw_main.SetItemDecimal(1, "month3_amt", loanreq_amt);
                        loanreq_amt = loanreq_amt - loanreq_dif; if (loanreq_amt < 0) { loanreq_amt = 0; }
                        dw_main.SetItemDecimal(1, "month4_amt", loanreq_amt);
                        loanreq_amt = loanreq_amt - loanreq_dif; if (loanreq_amt < 0) { loanreq_amt = 0; }
                        dw_main.SetItemDecimal(1, "month5_amt", loanreq_amt);
                        loanreq_amt = loanreq_amt - loanreq_dif; if (loanreq_amt < 0) { loanreq_amt = 0; }
                        dw_main.SetItemDecimal(1, "month6_amt", loanreq_amt);
                        loanreq_amt = loanreq_amt - loanreq_dif; if (loanreq_amt < 0) { loanreq_amt = 0; }
                        dw_main.SetItemDecimal(1, "month7_amt", loanreq_amt);
                        loanreq_amt = loanreq_amt - loanreq_dif; if (loanreq_amt < 0) { loanreq_amt = 0; }
                        dw_main.SetItemDecimal(1, "month8_amt", loanreq_amt);
                        loanreq_amt = loanreq_amt - loanreq_dif; if (loanreq_amt < 0) { loanreq_amt = 0; }
                        dw_main.SetItemDecimal(1, "month9_amt", loanreq_amt);
                        loanreq_amt = loanreq_amt - loanreq_dif; if (loanreq_amt < 0) { loanreq_amt = 0; }
                        dw_main.SetItemDecimal(1, "month10_amt", loanreq_amt);
                        loanreq_amt = loanreq_amt - loanreq_dif; if (loanreq_amt < 0) { loanreq_amt = 0; }
                        dw_main.SetItemDecimal(1, "month11_amt", loanreq_amt);
                        loanreq_amt = loanreq_amt - loanreq_dif; if (loanreq_amt < 0) { loanreq_amt = 0; }
                        dw_main.SetItemDecimal(1, "month12_amt", loanreq_amt);


                        break;
                    case 4:
                        dw_main.Modify("month1_amt.protect =1");
                        dw_main.Modify("month2_amt.protect =1");
                        dw_main.Modify("month3_amt.protect =1");

                        dw_main.SetItemDecimal(1, "month4_amt", loanreq_amt);
                        loanreq_amt = loanreq_amt - loanreq_dif; if (loanreq_amt < 0) { loanreq_amt = 0; }
                        dw_main.SetItemDecimal(1, "month5_amt", loanreq_amt);
                        loanreq_amt = loanreq_amt - loanreq_dif; if (loanreq_amt < 0) { loanreq_amt = 0; }
                        dw_main.SetItemDecimal(1, "month6_amt", loanreq_amt);
                        loanreq_amt = loanreq_amt - loanreq_dif; if (loanreq_amt < 0) { loanreq_amt = 0; }
                        dw_main.SetItemDecimal(1, "month7_amt", loanreq_amt);
                        loanreq_amt = loanreq_amt - loanreq_dif; if (loanreq_amt < 0) { loanreq_amt = 0; }
                        dw_main.SetItemDecimal(1, "month8_amt", loanreq_amt);
                        loanreq_amt = loanreq_amt - loanreq_dif; if (loanreq_amt < 0) { loanreq_amt = 0; }
                        dw_main.SetItemDecimal(1, "month9_amt", loanreq_amt);
                        loanreq_amt = loanreq_amt - loanreq_dif; if (loanreq_amt < 0) { loanreq_amt = 0; }
                        dw_main.SetItemDecimal(1, "month10_amt", loanreq_amt);
                        loanreq_amt = loanreq_amt - loanreq_dif; if (loanreq_amt < 0) { loanreq_amt = 0; }
                        dw_main.SetItemDecimal(1, "month11_amt", loanreq_amt);
                        loanreq_amt = loanreq_amt - loanreq_dif; if (loanreq_amt < 0) { loanreq_amt = 0; }
                        dw_main.SetItemDecimal(1, "month12_amt", loanreq_amt);


                        break;
                    case 5:
                        dw_main.Modify("month1_amt.protect =1");
                        dw_main.Modify("month2_amt.protect =1");
                        dw_main.Modify("month3_amt.protect =1");
                        dw_main.Modify("month4_amt.protect =1");

                        dw_main.SetItemDecimal(1, "month5_amt", loanreq_amt);
                        loanreq_amt = loanreq_amt - loanreq_dif; if (loanreq_amt < 0) { loanreq_amt = 0; }
                        dw_main.SetItemDecimal(1, "month6_amt", loanreq_amt);
                        loanreq_amt = loanreq_amt - loanreq_dif; if (loanreq_amt < 0) { loanreq_amt = 0; }
                        dw_main.SetItemDecimal(1, "month7_amt", loanreq_amt);
                        loanreq_amt = loanreq_amt - loanreq_dif; if (loanreq_amt < 0) { loanreq_amt = 0; }
                        dw_main.SetItemDecimal(1, "month8_amt", loanreq_amt);
                        loanreq_amt = loanreq_amt - loanreq_dif; if (loanreq_amt < 0) { loanreq_amt = 0; }
                        dw_main.SetItemDecimal(1, "month9_amt", loanreq_amt);
                        loanreq_amt = loanreq_amt - loanreq_dif; if (loanreq_amt < 0) { loanreq_amt = 0; }
                        dw_main.SetItemDecimal(1, "month10_amt", loanreq_amt);
                        loanreq_amt = loanreq_amt - loanreq_dif; if (loanreq_amt < 0) { loanreq_amt = 0; }
                        dw_main.SetItemDecimal(1, "month11_amt", loanreq_amt);
                        loanreq_amt = loanreq_amt - loanreq_dif; if (loanreq_amt < 0) { loanreq_amt = 0; }
                        dw_main.SetItemDecimal(1, "month12_amt", loanreq_amt);


                        break;
                    case 6:
                        dw_main.Modify("month1_amt.protect =1");
                        dw_main.Modify("month2_amt.protect =1");
                        dw_main.Modify("month3_amt.protect =1");
                        dw_main.Modify("month4_amt.protect =1");
                        dw_main.Modify("month5_amt.protect =1");

                        dw_main.SetItemDecimal(1, "month6_amt", loanreq_amt);
                        loanreq_amt = loanreq_amt - loanreq_dif; if (loanreq_amt < 0) { loanreq_amt = 0; }
                        dw_main.SetItemDecimal(1, "month7_amt", loanreq_amt);
                        loanreq_amt = loanreq_amt - loanreq_dif; if (loanreq_amt < 0) { loanreq_amt = 0; }
                        dw_main.SetItemDecimal(1, "month8_amt", loanreq_amt);
                        loanreq_amt = loanreq_amt - loanreq_dif; if (loanreq_amt < 0) { loanreq_amt = 0; }
                        dw_main.SetItemDecimal(1, "month9_amt", loanreq_amt);
                        loanreq_amt = loanreq_amt - loanreq_dif; if (loanreq_amt < 0) { loanreq_amt = 0; }
                        dw_main.SetItemDecimal(1, "month10_amt", loanreq_amt);
                        loanreq_amt = loanreq_amt - loanreq_dif; if (loanreq_amt < 0) { loanreq_amt = 0; }
                        dw_main.SetItemDecimal(1, "month11_amt", loanreq_amt);
                        loanreq_amt = loanreq_amt - loanreq_dif; if (loanreq_amt < 0) { loanreq_amt = 0; }
                        dw_main.SetItemDecimal(1, "month12_amt", loanreq_amt);

                        break;
                    case 7:
                        dw_main.Modify("month1_amt.protect =1");
                        dw_main.Modify("month2_amt.protect =1");
                        dw_main.Modify("month3_amt.protect =1");
                        dw_main.Modify("month4_amt.protect =1");
                        dw_main.Modify("month5_amt.protect =1");
                        dw_main.Modify("month6_amt.protect =1");

                        dw_main.SetItemDecimal(1, "month7_amt", loanreq_amt);
                        loanreq_amt = loanreq_amt - loanreq_dif; if (loanreq_amt < 0) { loanreq_amt = 0; }
                        dw_main.SetItemDecimal(1, "month8_amt", loanreq_amt);
                        loanreq_amt = loanreq_amt - loanreq_dif; if (loanreq_amt < 0) { loanreq_amt = 0; }
                        dw_main.SetItemDecimal(1, "month9_amt", loanreq_amt);
                        loanreq_amt = loanreq_amt - loanreq_dif; if (loanreq_amt < 0) { loanreq_amt = 0; }
                        dw_main.SetItemDecimal(1, "month10_amt", loanreq_amt);
                        loanreq_amt = loanreq_amt - loanreq_dif; if (loanreq_amt < 0) { loanreq_amt = 0; }
                        dw_main.SetItemDecimal(1, "month11_amt", loanreq_amt);
                        loanreq_amt = loanreq_amt - loanreq_dif; if (loanreq_amt < 0) { loanreq_amt = 0; }
                        dw_main.SetItemDecimal(1, "month12_amt", loanreq_amt);

                        break;
                    case 8:
                        dw_main.Modify("month1_amt.protect =1");
                        dw_main.Modify("month2_amt.protect =1");
                        dw_main.Modify("month3_amt.protect =1");
                        dw_main.Modify("month4_amt.protect =1");
                        dw_main.Modify("month5_amt.protect =1");
                        dw_main.Modify("month6_amt.protect =1");
                        dw_main.Modify("month7_amt.protect =1");

                        dw_main.SetItemDecimal(1, "month8_amt", loanreq_amt);
                        loanreq_amt = loanreq_amt - loanreq_dif; if (loanreq_amt < 0) { loanreq_amt = 0; }
                        dw_main.SetItemDecimal(1, "month9_amt", loanreq_amt);
                        loanreq_amt = loanreq_amt - loanreq_dif; if (loanreq_amt < 0) { loanreq_amt = 0; }
                        dw_main.SetItemDecimal(1, "month10_amt", loanreq_amt);
                        loanreq_amt = loanreq_amt - loanreq_dif; if (loanreq_amt < 0) { loanreq_amt = 0; }
                        dw_main.SetItemDecimal(1, "month11_amt", loanreq_amt);
                        loanreq_amt = loanreq_amt - loanreq_dif; if (loanreq_amt < 0) { loanreq_amt = 0; }
                        dw_main.SetItemDecimal(1, "month12_amt", loanreq_amt);


                        break;
                    case 9:
                        dw_main.Modify("month1_amt.protect =1");
                        dw_main.Modify("month2_amt.protect =1");
                        dw_main.Modify("month3_amt.protect =1");
                        dw_main.Modify("month4_amt.protect =1");
                        dw_main.Modify("month5_amt.protect =1");
                        dw_main.Modify("month6_amt.protect =1");
                        dw_main.Modify("month7_amt.protect =1");
                        dw_main.Modify("month8_amt.protect =1");

                        dw_main.SetItemDecimal(1, "month9_amt", loanreq_amt);
                        loanreq_amt = loanreq_amt - loanreq_dif; if (loanreq_amt < 0) { loanreq_amt = 0; }
                        dw_main.SetItemDecimal(1, "month10_amt", loanreq_amt);
                        loanreq_amt = loanreq_amt - loanreq_dif; if (loanreq_amt < 0) { loanreq_amt = 0; }
                        dw_main.SetItemDecimal(1, "month11_amt", loanreq_amt);
                        loanreq_amt = loanreq_amt - loanreq_dif; if (loanreq_amt < 0) { loanreq_amt = 0; }
                        dw_main.SetItemDecimal(1, "month12_amt", loanreq_amt);


                        break;
                    case 10:
                        dw_main.Modify("month1_amt.protect =1");
                        dw_main.Modify("month2_amt.protect =1");
                        dw_main.Modify("month3_amt.protect =1");
                        dw_main.Modify("month4_amt.protect =1");
                        dw_main.Modify("month5_amt.protect =1");
                        dw_main.Modify("month6_amt.protect =1");
                        dw_main.Modify("month7_amt.protect =1");
                        dw_main.Modify("month8_amt.protect =1");
                        dw_main.Modify("month9_amt.protect =1");

                        dw_main.SetItemDecimal(1, "month10_amt", loanreq_amt);
                        loanreq_amt = loanreq_amt - loanreq_dif; if (loanreq_amt < 0) { loanreq_amt = 0; }
                        dw_main.SetItemDecimal(1, "month11_amt", loanreq_amt);
                        loanreq_amt = loanreq_amt - loanreq_dif; if (loanreq_amt < 0) { loanreq_amt = 0; }
                        dw_main.SetItemDecimal(1, "month12_amt", loanreq_amt);


                        break;
                    case 11:
                        dw_main.Modify("month1_amt.protect =1");
                        dw_main.Modify("month2_amt.protect =1");
                        dw_main.Modify("month3_amt.protect =1");
                        dw_main.Modify("month4_amt.protect =1");
                        dw_main.Modify("month5_amt.protect =1");
                        dw_main.Modify("month6_amt.protect =1");
                        dw_main.Modify("month7_amt.protect =1");
                        dw_main.Modify("month8_amt.protect =1");
                        dw_main.Modify("month9_amt.protect =1");
                        dw_main.Modify("month10_amt.protect =1");

                        dw_main.SetItemDecimal(1, "month11_amt", loanreq_amt);
                        loanreq_amt = loanreq_amt - loanreq_dif; if (loanreq_amt < 0) { loanreq_amt = 0; }
                        dw_main.SetItemDecimal(1, "month12_amt", loanreq_amt);

                        break;
                    case 12:
                        dw_main.Modify("month1_amt.protect =1");
                        dw_main.Modify("month2_amt.protect =1");
                        dw_main.Modify("month3_amt.protect =1");
                        dw_main.Modify("month4_amt.protect =1");
                        dw_main.Modify("month5_amt.protect =1");
                        dw_main.Modify("month6_amt.protect =1");
                        dw_main.Modify("month7_amt.protect =1");
                        dw_main.Modify("month8_amt.protect =1");
                        dw_main.Modify("month9_amt.protect =1");
                        dw_main.Modify("month10_amt.protect =1");
                        dw_main.Modify("month11_amt.protect =1");

                        dw_main.SetItemDecimal(1, "month12_amt", loanreq_amt);
                        break;
                    //  }//end else  กรณีลดเท่ากันหมดทุเดือน 
                }
            }
            else if (loanreq_type == 4)
            {

                SetmonthAmt_2();
                switch (loanreq_mthstart)
                {
                    case 1:
                        dw_main.SetItemDecimal(1, "month1_amt", loanreq_amt);

                        break;
                    case 2:
                        dw_main.SetItemDecimal(1, "month2_amt", loanreq_amt);

                        break;
                    case 3:
                        dw_main.SetItemDecimal(1, "month3_amt", loanreq_amt);

                        break;
                    case 4:
                        dw_main.SetItemDecimal(1, "month4_amt", loanreq_amt);

                        break;
                    case 5:
                        dw_main.SetItemDecimal(1, "month5_amt", loanreq_amt);

                        break;
                    case 6:
                        dw_main.SetItemDecimal(1, "month6_amt", loanreq_amt);

                        break;
                    case 7:
                        dw_main.SetItemDecimal(1, "month7_amt", loanreq_amt);

                        break;
                    case 8:
                        dw_main.SetItemDecimal(1, "month8_amt", loanreq_amt);

                        break;
                    case 9:
                        dw_main.SetItemDecimal(1, "month9_amt", loanreq_amt);

                        break;
                    case 10:
                        dw_main.SetItemDecimal(1, "month10_amt", loanreq_amt);

                        break;
                    case 11:

                        dw_main.SetItemDecimal(1, "month11_amt", loanreq_amt);

                        break;
                    case 12:
                        dw_main.SetItemDecimal(1, "month12_amt", loanreq_amt);

                        break;
                }

                //SetmonthAmt();
                //dw_main.Modify("loanreq_dif.protect =1");
                //switch (loanreq_mthstart)
                //{
                //    case 1:
                //        dw_main.SetItemDecimal(1, "month1_amt", loanreq_amt);

                //        dw_main.Modify("month2_amt.protect =1");
                //        dw_main.Modify("month3_amt.protect =1");
                //        dw_main.Modify("month4_amt.protect =1");
                //        dw_main.Modify("month5_amt.protect =1");
                //        dw_main.Modify("month6_amt.protect =1");
                //        dw_main.Modify("month7_amt.protect =1");
                //        dw_main.Modify("month8_amt.protect =1");
                //        dw_main.Modify("month9_amt.protect =1");
                //        dw_main.Modify("month10_amt.protect =1");
                //        dw_main.Modify("month11_amt.protect =1");
                //        dw_main.Modify("month12_amt.protect =1");
                //        break;
                //    case 2:
                //        dw_main.SetItemDecimal(1, "month2_amt", loanreq_amt);
                //        dw_main.Modify("month1_amt.protect =1");
                //        dw_main.Modify("month3_amt.protect =1");
                //        dw_main.Modify("month4_amt.protect =1");
                //        dw_main.Modify("month5_amt.protect =1");
                //        dw_main.Modify("month6_amt.protect =1");
                //        dw_main.Modify("month7_amt.protect =1");
                //        dw_main.Modify("month8_amt.protect =1");
                //        dw_main.Modify("month9_amt.protect =1");
                //        dw_main.Modify("month10_amt.protect =1");
                //        dw_main.Modify("month11_amt.protect =1");
                //        dw_main.Modify("month12_amt.protect =1");
                //        break;
                //    case 3:
                //        dw_main.SetItemDecimal(1, "month3_amt", loanreq_amt);
                //        dw_main.Modify("month2_amt.protect =1");
                //        dw_main.Modify("month1_amt.protect =1");
                //        dw_main.Modify("month4_amt.protect =1");
                //        dw_main.Modify("month5_amt.protect =1");
                //        dw_main.Modify("month6_amt.protect =1");
                //        dw_main.Modify("month7_amt.protect =1");
                //        dw_main.Modify("month8_amt.protect =1");
                //        dw_main.Modify("month9_amt.protect =1");
                //        dw_main.Modify("month10_amt.protect =1");
                //        dw_main.Modify("month11_amt.protect =1");
                //        dw_main.Modify("month12_amt.protect =1");
                //        break;
                //    case 4:
                //        dw_main.SetItemDecimal(1, "month4_amt", loanreq_amt);
                //        dw_main.Modify("month2_amt.protect =1");
                //        dw_main.Modify("month1_amt.protect =1");
                //        dw_main.Modify("month3_amt.protect =1");
                //        dw_main.Modify("month5_amt.protect =1");
                //        dw_main.Modify("month6_amt.protect =1");
                //        dw_main.Modify("month7_amt.protect =1");
                //        dw_main.Modify("month8_amt.protect =1");
                //        dw_main.Modify("month9_amt.protect =1");
                //        dw_main.Modify("month10_amt.protect =1");
                //        dw_main.Modify("month11_amt.protect =1");
                //        dw_main.Modify("month12_amt.protect =1");
                //        break;
                //    case 5:
                //        dw_main.SetItemDecimal(1, "month5_amt", loanreq_amt);
                //        dw_main.Modify("month2_amt.protect =1");
                //        dw_main.Modify("month1_amt.protect =1");
                //        dw_main.Modify("month4_amt.protect =1");
                //        dw_main.Modify("month3_amt.protect =1");
                //        dw_main.Modify("month6_amt.protect =1");
                //        dw_main.Modify("month7_amt.protect =1");
                //        dw_main.Modify("month8_amt.protect =1");
                //        dw_main.Modify("month9_amt.protect =1");
                //        dw_main.Modify("month10_amt.protect =1");
                //        dw_main.Modify("month11_amt.protect =1");
                //        dw_main.Modify("month12_amt.protect =1");
                //        break;
                //    case 6:
                //        dw_main.SetItemDecimal(1, "month6_amt", loanreq_amt);
                //        dw_main.Modify("month2_amt.protect =1");
                //        dw_main.Modify("month1_amt.protect =1");
                //        dw_main.Modify("month4_amt.protect =1");
                //        dw_main.Modify("month5_amt.protect =1");
                //        dw_main.Modify("month3_amt.protect =1");
                //        dw_main.Modify("month7_amt.protect =1");
                //        dw_main.Modify("month8_amt.protect =1");
                //        dw_main.Modify("month9_amt.protect =1");
                //        dw_main.Modify("month10_amt.protect =1");
                //        dw_main.Modify("month11_amt.protect =1");
                //        dw_main.Modify("month12_amt.protect =1");
                //        break;
                //    case 7:
                //        dw_main.SetItemDecimal(1, "month7_amt", loanreq_amt);
                //        dw_main.Modify("month2_amt.protect =1");
                //        dw_main.Modify("month1_amt.protect =1");
                //        dw_main.Modify("month4_amt.protect =1");
                //        dw_main.Modify("month5_amt.protect =1");
                //        dw_main.Modify("month6_amt.protect =1");
                //        dw_main.Modify("month3_amt.protect =1");
                //        dw_main.Modify("month8_amt.protect =1");
                //        dw_main.Modify("month9_amt.protect =1");
                //        dw_main.Modify("month10_amt.protect =1");
                //        dw_main.Modify("month11_amt.protect =1");
                //        dw_main.Modify("month12_amt.protect =1");
                //        break;
                //    case 8:
                //        dw_main.SetItemDecimal(1, "month8_amt", loanreq_amt);
                //        dw_main.Modify("month2_amt.protect =1");
                //        dw_main.Modify("month1_amt.protect =1");
                //        dw_main.Modify("month4_amt.protect =1");
                //        dw_main.Modify("month5_amt.protect =1");
                //        dw_main.Modify("month6_amt.protect =1");
                //        dw_main.Modify("month7_amt.protect =1");
                //        dw_main.Modify("month3_amt.protect =1");
                //        dw_main.Modify("month9_amt.protect =1");
                //        dw_main.Modify("month10_amt.protect =1");
                //        dw_main.Modify("month11_amt.protect =1");
                //        dw_main.Modify("month12_amt.protect =1");
                //        break;
                //    case 9:
                //        dw_main.SetItemDecimal(1, "month9_amt", loanreq_amt);
                //        dw_main.Modify("month2_amt.protect =1");
                //        dw_main.Modify("month1_amt.protect =1");
                //        dw_main.Modify("month4_amt.protect =1");
                //        dw_main.Modify("month5_amt.protect =1");
                //        dw_main.Modify("month6_amt.protect =1");
                //        dw_main.Modify("month7_amt.protect =1");
                //        dw_main.Modify("month8_amt.protect =1");
                //        dw_main.Modify("month3_amt.protect =1");
                //        dw_main.Modify("month10_amt.protect =1");
                //        dw_main.Modify("month11_amt.protect =1");
                //        dw_main.Modify("month12_amt.protect =1");
                //        break;
                //    case 10:
                //        dw_main.SetItemDecimal(1, "month10_amt", loanreq_amt);
                //        dw_main.Modify("month2_amt.protect =1");
                //        dw_main.Modify("month1_amt.protect =1");
                //        dw_main.Modify("month4_amt.protect =1");
                //        dw_main.Modify("month5_amt.protect =1");
                //        dw_main.Modify("month6_amt.protect =1");
                //        dw_main.Modify("month7_amt.protect =1");
                //        dw_main.Modify("month8_amt.protect =1");
                //        dw_main.Modify("month9_amt.protect =1");
                //        dw_main.Modify("month3_amt.protect =1");
                //        dw_main.Modify("month11_amt.protect =1");
                //        dw_main.Modify("month12_amt.protect =1");
                //        break;
                //    case 11:

                //        dw_main.SetItemDecimal(1, "month11_amt", loanreq_amt);
                //        dw_main.Modify("month2_amt.protect =1");
                //        dw_main.Modify("month1_amt.protect =1");
                //        dw_main.Modify("month4_amt.protect =1");
                //        dw_main.Modify("month5_amt.protect =1");
                //        dw_main.Modify("month6_amt.protect =1");
                //        dw_main.Modify("month7_amt.protect =1");
                //        dw_main.Modify("month8_amt.protect =1");
                //        dw_main.Modify("month9_amt.protect =1");
                //        dw_main.Modify("month10_amt.protect =1");
                //        dw_main.Modify("month3_amt.protect =1");
                //        dw_main.Modify("month12_amt.protect =1");
                //        break;
                //    case 12:
                //        dw_main.SetItemDecimal(1, "month12_amt", loanreq_amt);
                //        dw_main.Modify("month2_amt.protect =1");
                //        dw_main.Modify("month1_amt.protect =1");
                //        dw_main.Modify("month4_amt.protect =1");
                //        dw_main.Modify("month5_amt.protect =1");
                //        dw_main.Modify("month6_amt.protect =1");
                //        dw_main.Modify("month7_amt.protect =1");
                //        dw_main.Modify("month8_amt.protect =1");
                //        dw_main.Modify("month9_amt.protect =1");
                //        dw_main.Modify("month10_amt.protect =1");
                //        dw_main.Modify("month11_amt.protect =1");
                //        dw_main.Modify("month3_amt.protect =1");
                //        break;
                //}
            }

        }

        private void SetmonthAmt()
        {
            dw_main.SetItemDecimal(1, "month1_amt", 0);

            dw_main.SetItemDecimal(1, "month2_amt", 0);

            dw_main.SetItemDecimal(1, "month3_amt", 0);

            dw_main.SetItemDecimal(1, "month4_amt", 0);

            dw_main.SetItemDecimal(1, "month5_amt", 0);

            dw_main.SetItemDecimal(1, "month6_amt", 0);

            dw_main.SetItemDecimal(1, "month7_amt", 0);

            dw_main.SetItemDecimal(1, "month8_amt", 0);

            dw_main.SetItemDecimal(1, "month9_amt", 0);

            dw_main.SetItemDecimal(1, "month10_amt", 0);

            dw_main.SetItemDecimal(1, "month11_amt", 0);

            dw_main.SetItemDecimal(1, "month12_amt", 0);


            dw_main.Modify("month1_amt.protect =1");
            dw_main.Modify("month2_amt.protect =1");
            dw_main.Modify("month3_amt.protect =1");
            dw_main.Modify("month4_amt.protect =1");
            dw_main.Modify("month5_amt.protect =1");
            dw_main.Modify("month6_amt.protect =1");
            dw_main.Modify("month7_amt.protect =1");
            dw_main.Modify("month8_amt.protect =1");
            dw_main.Modify("month9_amt.protect =1");
            dw_main.Modify("month10_amt.protect =1");
            dw_main.Modify("month11_amt.protect =1");
            dw_main.Modify("month12_amt.protect =1");
        }
        private void SetmonthAmt_2()
        {
            dw_main.SetItemDecimal(1, "month1_amt", 0);

            dw_main.SetItemDecimal(1, "month2_amt", 0);

            dw_main.SetItemDecimal(1, "month3_amt", 0);

            dw_main.SetItemDecimal(1, "month4_amt", 0);

            dw_main.SetItemDecimal(1, "month5_amt", 0);

            dw_main.SetItemDecimal(1, "month6_amt", 0);

            dw_main.SetItemDecimal(1, "month7_amt", 0);

            dw_main.SetItemDecimal(1, "month8_amt", 0);

            dw_main.SetItemDecimal(1, "month9_amt", 0);

            dw_main.SetItemDecimal(1, "month10_amt", 0);

            dw_main.SetItemDecimal(1, "month11_amt", 0);

            dw_main.SetItemDecimal(1, "month12_amt", 0);
            dw_main.Modify("month1_amt.protect =0");
            dw_main.Modify("month2_amt.protect =0");
            dw_main.Modify("month3_amt.protect =0");
            dw_main.Modify("month4_amt.protect =0");
            dw_main.Modify("month5_amt.protect =0");
            dw_main.Modify("month6_amt.protect =0");
            dw_main.Modify("month7_amt.protect =0");
            dw_main.Modify("month8_amt.protect =0");
            dw_main.Modify("month9_amt.protect =0");
            dw_main.Modify("month10_amt.protect =0");
            dw_main.Modify("month11_amt.protect =0");
            dw_main.Modify("month12_amt.protect =0");


        }
        private void JsRefresh()
        {


        }

        private void JsLoanreq_dif()
        {

        }

        private void JsLoanreq_amt()
        {
            Decimal loancredit_amt = 0;
            Decimal loanreq_amt = 0;
            loancredit_amt = dw_main.GetItemDecimal(1, "loancredit_amt");
            loanreq_amt = Convert.ToDecimal(HFloanreq_amt.Value);// dw_main.GetItemDecimal(1, "loanreq_amt");
            if (loanreq_amt <= loancredit_amt)
            {
                //  alert("ยอดขอกู้ เกินสิทธิการกู้");
                dw_main.SetItemDecimal(1, "loanreq_amt", loanreq_amt);

            }
            else
            {
                dw_main.SetItemDecimal(1, "loanreq_amt", loancredit_amt);
            }
            JsSetloanreqAmt();
            // HdIsPostBack.Value = "false";
        }

        private void JsPostMember()
        {
            try
            {
                String member_no = HdMemberNo.Value;// dw_main.GetItemString(1, "member_no");
                String xml_newlnyear = dw_main.Describe("DataWindow.Data.XML");
                String xml_oldlnyear = dw_oldreq.Describe("DataWindow.Data.XML");
                Int16 lnreq_year = Convert.ToInt16(dw_main.GetItemDecimal(1, "loanreq_year"));
                Int16 month_start = Convert.ToInt16(dw_main.GetItemDecimal(1, "loanreq_mthstart"));
                DateTime lnreq_date = state.SsWorkDate;
                String entry_id = state.SsUsername;
                String branch_id = state.SsCoopId;

                str_lnreqyear astr_lnreqyear = new str_lnreqyear();
                astr_lnreqyear.member_no = member_no;
                astr_lnreqyear.xml_newlnyear = xml_newlnyear;
                astr_lnreqyear.xml_oldlnyear = xml_oldlnyear;
                astr_lnreqyear.lnreq_date = lnreq_date;
                astr_lnreqyear.lnreq_year = lnreq_year;
                astr_lnreqyear.month_start = month_start;
                astr_lnreqyear.entry_id = entry_id;
                astr_lnreqyear.branch_id = branch_id;
                int result = shrlonService.of_initreq_loanyear(state.SsWsPass, ref astr_lnreqyear);
                if (result == 1)
                {
                    try
                    {
                        dw_main.Reset();
                        DwUtil.ImportData(astr_lnreqyear.xml_newlnyear, dw_main, tDwMain, FileSaveAsType.Xml);
                        //dw_main.ImportString(astr_lnreqyear.xml_newlnyear, FileSaveAsType.Xml);
                        dw_main.SetItemDateTime(1, "loanreq_date", state.SsWorkDate);
                        tDwMain.Eng2ThaiAllRow();
                        try
                        {
                            String loanrunning = Session["loancontract_running"].ToString();
                            if (loanrunning != "")
                            {
                                Decimal loancontract_running = Convert.ToDecimal(Session["loancontract_running"]) + 1;
                                dw_main.SetItemDecimal(1, "loancontract_running", loancontract_running);
                            }
                        }
                        catch { }
                    }
                    catch (Exception ex)
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                        dw_main.Reset(); dw_main.InsertRow(0);
                    }
                    try
                    {
                        dw_oldreq.Reset();
                        DwUtil.ImportData(astr_lnreqyear.xml_oldlnyear, dw_oldreq, null, FileSaveAsType.Xml);
                        //dw_oldreq.ImportString(astr_lnreqyear.xml_oldlnyear, FileSaveAsType.Xml);
                    }
                    catch (Exception ex)
                    {

                        if (astr_lnreqyear.xml_oldlnyear == "")
                        {

                        }
                        else
                        {
                            LtServerMessage.Text = WebUtil.ErrorMessage(ex);

                        }

                    }
                }
            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
        }

        public void SaveWebSheet()
        {
            try
            {



                Decimal sharestk_value = dw_main.GetItemDecimal(1, "sharestk_value");
                Decimal loanreq_amt = dw_main.GetItemDecimal(1, "loanreq_amt");
                String reqgrt_memno;
                try
                {
                    reqgrt_memno = dw_main.GetItemString(1, "reqgrt_memno");
                }
                catch { reqgrt_memno = ""; }

                if (sharestk_value < loanreq_amt && reqgrt_memno == "")
                {


                    LtServerMessage.Text = WebUtil.ErrorMessage("ยอดขอกู้ มากกว่าทุนเรือนหุ้น  กรุณาใส่คนค้ำ");

                }
                else
                {

                    String member_no = HdMemberNo.Value;// dw_main.GetItemString(1, "member_no");
                    String xml_newlnyear = dw_main.Describe("DataWindow.Data.XML");
                    String xml_oldlnyear = dw_oldreq.Describe("DataWindow.Data.XML");
                    Int16 lnreq_year = Convert.ToInt16(dw_main.GetItemDecimal(1, "loanreq_year"));
                    Int16 month_start = Convert.ToInt16(dw_main.GetItemDecimal(1, "loanreq_mthstart"));
                    DateTime lnreq_date = dw_main.GetItemDateTime(1, "loanreq_date");
                    String entry_id = state.SsUsername;
                    String branch_id = state.SsCoopId;
                    try
                    {
                        Session["loancontract_running"] = dw_main.GetItemDecimal(1, "loancontract_running");
                    }
                    catch { }
                    Session["lnreq_year"] = dw_main.GetItemDecimal(1, "loanreq_year");
                    Session["month_start"] = dw_main.GetItemDecimal(1, "loanreq_mthstart");
                    str_lnreqyear astr_lnreqyear = new str_lnreqyear();
                    astr_lnreqyear.member_no = member_no;
                    astr_lnreqyear.xml_newlnyear = xml_newlnyear;
                    astr_lnreqyear.xml_oldlnyear = xml_oldlnyear;
                    astr_lnreqyear.lnreq_date = lnreq_date;
                    astr_lnreqyear.lnreq_year = lnreq_year;
                    astr_lnreqyear.month_start = month_start;
                    astr_lnreqyear.entry_id = entry_id;
                    astr_lnreqyear.branch_id = branch_id;
                    int result = shrlonService.of_savereq_loanyear(state.SsWsPass, ref astr_lnreqyear);
                    if (result == 1)
                    {
                        LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลเรียบร้อย");

                        //try
                        //{
                        //    dw_main.Reset();
                        //    dw_main.ImportString(astr_lnreqyear.xml_newlnyear, FileSaveAsType.Xml);
                        //}
                        //catch (Exception ex)
                        //{
                        //    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                        //    dw_main.Reset(); dw_main.InsertRow(0);
                        //}
                        //try
                        //{
                        //    dw_oldreq.Reset();
                        //    dw_oldreq.ImportString(astr_lnreqyear.xml_oldlnyear, FileSaveAsType.Xml);
                        //}
                        //catch (Exception ex)
                        //{

                        //    if (astr_lnreqyear.xml_oldlnyear == "")
                        //    {
                        //        //LtServerMessage.Text = WebUtil.WarningMessage("ไม่พบข้อมูลเก่า");
                        //        //dw_oldreq.Reset();
                        //        //dw_oldreq.InsertRow(0);
                        //    }
                        //    else
                        //    {
                        //        LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                        //        //dw_oldreq.Reset();
                        //        //dw_oldreq.InsertRow(0);
                        //    }

                        //}
                    }
                    JsOpenNew();
                }

            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }


        }

        public void WebSheetLoadEnd()
        {

           
            dw_main.SaveDataCache();
            dw_oldreq.SaveDataCache();
        }


    }
}
