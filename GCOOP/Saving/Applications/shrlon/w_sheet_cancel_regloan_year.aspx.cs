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
    public partial class w_sheet_cancel_regloan_year : PageWebSheet, WebSheet
    {
        private DwThDate tDwMain;
        private n_shrlonClient shrlonService;
        private n_commonClient commonService;
        protected String openNew;
        protected String jsPostMember = "";

        protected String getLoanrequest;
        protected String initlist;
        public void InitJsPostBack()
        {
            jsPostMember = WebUtil.JsPostBack(this, "jsPostMember");
            initlist = WebUtil.JsPostBack(this, "initlist");
            openNew = WebUtil.JsPostBack(this, "openNew");
            getLoanrequest = WebUtil.JsPostBack(this, "getLoanrequest");
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


                HdIsPostBack.Value = "true";
            }
            if (dw_main.RowCount < 1)
            {
                InitDataWindow();
                HdIsPostBack.Value = "false";
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "jsPostMember")
            {
                this.JsPostMember();

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
            else if (eventArg == "initlist") { Initlist(); }
        }

        private void Initlist()
        {
            try
            {

                str_lnreqyear astr_lnreqyear = new str_lnreqyear();
                HfisCalInt.Value = "false";

                astr_lnreqyear.lnreq_docno = Hfloancontract_no.Value;
                int resultXML = shrlonService.of_openreq_loanyear(state.SsWsPass, ref astr_lnreqyear);
                try
                {

                    dw_main.Reset();
                    DwUtil.ImportData(astr_lnreqyear.xml_newlnyear, dw_main, null, FileSaveAsType.Xml);
                    //   DwUtil.DeleteLastRow(dw_main);
                    tDwMain.Eng2ThaiAllRow();

                }
                catch
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่มีเลขสมาชิก " + dw_main.GetItemString(1, "member_no"));
                    InitDataWindow();
                }

                // }
                //else if (result == 2)
                //{
                //    dw_list.Visible = true;
                //    dw_list.Reset();
                //    TextBox2.Text = as_xmllnrcv;
                //    dw_list.ImportString(as_xmllnrcv, Sybase.DataWindow.FileSaveAsType.Xml);
                //}
            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
        }
        private void GetLoanrequest()
        {

            try
            {
                String member_no = HdMemberNo.Value;// dw_main.GetItemString(1, "member_no");
                String xml_newlnyear = dw_main.Describe("DataWindow.Data.XML");

                Int16 lnreq_year = Convert.ToInt16(dw_main.GetItemDecimal(1, "loanreq_year"));
                Int16 month_start = Convert.ToInt16(dw_main.GetItemDecimal(1, "loanreq_mthstart"));
                DateTime lnreq_date = state.SsWorkDate;
                String entry_id = state.SsUsername;
                String branch_id = state.SsCoopId;

                str_lnreqyear astr_lnreqyear = new str_lnreqyear();
                astr_lnreqyear.lnreq_docno = Hloanrequest_docno.Value;
                astr_lnreqyear.member_no = member_no;
                astr_lnreqyear.xml_newlnyear = xml_newlnyear;

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

                    }
                    catch (Exception ex)
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                        dw_main.Reset(); dw_main.InsertRow(0);
                    }
                }
            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }





        }

        private void JsOpenNew()
        {
            dw_main.Reset();

            dw_main.InsertRow(0);

            dw_main.SetItemDateTime(1, "loanreq_date", state.SsWorkDate);
            tDwMain.Eng2ThaiAllRow();
            DwUtil.RetrieveDDDW(dw_main, "loantype_code", "sl_loan_requestment.pbl", null);

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


            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
        }

        private void JsPostMember()
        {

            try
            {
                str_lnreqyear astr_lnreqyear = new str_lnreqyear();
                String member_no = HdMemberNo.Value;
                String as_reqno = "";
                String as_xmlreqlist = "";

                int getresult = shrlonService.of_getmemblnreqyear(state.SsWsPass, member_no, ref as_reqno, ref as_xmlreqlist);
                if (getresult == 1)
                {

                    astr_lnreqyear.lnreq_docno = as_reqno;
                    Hfloancontract_no.Value = as_reqno;
                    int resultXML = shrlonService.of_openreq_loanyear(state.SsWsPass, ref astr_lnreqyear);
                    try
                    {
                        dw_main.Reset();
                        DwUtil.ImportData(astr_lnreqyear.xml_newlnyear, dw_main, tDwMain, FileSaveAsType.Xml);

                        tDwMain.Eng2ThaiAllRow();
                    }
                    catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }

                }
                else if (getresult > 1)
                {
                    dw_list.Visible = true;
                    dw_list.Reset();
                    TextBox2.Text = as_xmlreqlist;
                    DwUtil.ImportData(as_xmlreqlist, dw_list, null, FileSaveAsType.Xml);
                }
            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }



        }

        public void SaveWebSheet()
        {
            try
            {

                String member_no = HdMemberNo.Value;// dw_main.GetItemString(1, "member_no");
                String xml_newlnyear = dw_main.Describe("DataWindow.Data.XML");

                Int16 lnreq_year = Convert.ToInt16(dw_main.GetItemDecimal(1, "loanreq_year"));
                Int16 month_start = Convert.ToInt16(dw_main.GetItemDecimal(1, "loanreq_mthstart"));
                DateTime lnreq_date = dw_main.GetItemDateTime(1, "loanreq_date");
                String entry_id = state.SsUsername;
                String branch_id = state.SsCoopId;

                str_lnreqyear astr_lnreqyear = new str_lnreqyear();
                astr_lnreqyear.member_no = member_no;
                astr_lnreqyear.xml_newlnyear = xml_newlnyear;
                astr_lnreqyear.lnreq_docno = Hfloancontract_no.Value;
                astr_lnreqyear.lnreq_date = lnreq_date;
                astr_lnreqyear.lnreq_year = lnreq_year;
                astr_lnreqyear.month_start = month_start;
                astr_lnreqyear.entry_id = entry_id;
                astr_lnreqyear.branch_id = branch_id;
                int result = shrlonService.of_saveccl_loanyear(state.SsWsPass, ref astr_lnreqyear);
                if (result == 1)
                {
                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลเรียบร้อย");


                }
                JsOpenNew();


            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }


        }

        public void WebSheetLoadEnd()
        {

            dw_main.SaveDataCache();
            dw_list.SaveDataCache();
        }


    }
}
