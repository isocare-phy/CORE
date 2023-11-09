using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommonLibrary;
using CommonLibrary.WsInsurance;
using Sybase.DataWindow;

namespace Saving.Applications.app_assist
{
    public partial class w_sheet_kt_installloan : PageWebSheet, WebSheet
    {

        String pbl = "as_asn_installloan.pbl";
        protected String jsRetrieval;
        protected String jsFind;
        protected String jsCancel;
        protected String jsInstall;
        private DwThDate tdwMain;
        private Insurance InsService;
        protected String ClearProcess;
        protected String jsReadyKeep;



        //====================================================================//
        private void JsNewClear()
        {
            dw_main.InsertRow(0);
            dw_data.Reset();
            dw_main.SetItemDate(1, "start_date", state.SsWorkDate);
            dw_main.SetItemDate(1, "end_date", state.SsWorkDate);
            tdwMain.Eng2ThaiAllRow();

        }

        //====================================================================//
        public void InitJsPostBack()
        {
            jsRetrieval = WebUtil.JsPostBack(this, "jsRetrieval");
            jsFind = WebUtil.JsPostBack(this, "jsFind");
            jsInstall = WebUtil.JsPostBack(this, "jsInstall");
            jsCancel = WebUtil.JsPostBack(this, "jsCancel");
            ClearProcess = WebUtil.JsPostBack(this, "ClearProcess");
            jsReadyKeep = WebUtil.JsPostBack(this, "jsReadyKeep");

            tdwMain = new DwThDate(dw_main, this);
            tdwMain.Add("start_date", "start_tdate");
            tdwMain.Add("end_date", "end_tdate");
        }

        //====================================================================//]
        public void WebSheetLoadBegin()
        {
            HdRunProcess.Value = "false";
            InsService = WsCalling.Insurance;

            if (!IsPostBack)
            {
                JsNewClear();
            }
            else
            {
                this.RestoreContextDw(dw_main);
                this.RestoreContextDw(dw_data);
            }
        }

        //====================================================================//
        public void CheckJsPostBack(string eventArg)
        {

            if (eventArg == "jsRetrieval")
            {
                JsRetrieval();
            }else if (eventArg == "jsFind")
            {
                JsFind();
            }else if (eventArg == "jsInstall")
            {
                JsInstall();
            }else if (eventArg == "jsCancel")
            {
                JsCancel();
            }else if (eventArg == "ClearProcess")
            {
                clearProcess();
            }
            else if (eventArg == "jsReadyKeep")
            {
                JsReadyKeep();
            }
            
        }

        //====================================================================//
        private void clearProcess()
        {
            dw_main.Reset();
            dw_data.Reset();

            JsNewClear();


        }

        //====================================================================//
        public void SaveWebSheet()
        {
        }

        //====================================================================//
        public void WebSheetLoadEnd()
        {
            try
            {
                DwUtil.RetrieveDDDW(dw_main, "loantype_code", pbl, null);
               // DwUtil.RetrieveDDDW(dw_main, "instype_code", pbl, null);
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
            ///fix กู้สามัญ
            dw_main.SetItemString(1, "loantype_code", "02");

            dw_main.SaveDataCache();
            dw_data.SaveDataCache();
        }

        //==============================Button ดึงข้อมูล==============================//
        private void JsRetrieval()
        {
            try
            {
                String loantype_code = dw_main.GetItemString(1, "loantype_code");
                DateTime ldtm_start = dw_main.GetItemDateTime(1, "start_date");
                DateTime ldtm_end = dw_main.GetItemDateTime(1, "end_date");

                    object[] arg = new object[3] { loantype_code, ldtm_start, ldtm_end };
                    DwUtil.RetrieveDataWindow(dw_data, pbl, tdwMain, arg);
            

            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        //==============================Button ค้นหา==============================//
        private void JsFind()
        {
            try
            {
                String member_no = dw_main.GetItemString(1, "member_no");
                member_no = WebUtil.MemberNoFormat(member_no);
                dw_main.SetItemString(1, "member_no", member_no);

                dw_data.SetFilter("member_no='" + member_no + "'");
                dw_data.Filter();
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        //==============================Button ติดตั้งประกัน==============================//
        private void JsInstall()
        {
            try
            {
                String Xml_datacri = dw_main.Describe("DataWindow.Data.XML");
                String Xml_dataloan = dw_data.Describe("DataWindow.Data.XML");

                int result = InsService.KtInstall(state.SsWsPass, Xml_datacri, Xml_dataloan, state.SsApplication, state.CurrentPage);
          
                HdRunProcess.Value = "true";
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        //==============================Button ยกเลิกติดตั้งประกัน==============================//
        private void JsCancel()
        {
            try
            {
                String Xml_datacri = dw_main.Describe("DataWindow.Data.XML");
                String Xml_datadetail = dw_data.Describe("DataWindow.Data.XML");


                int result = InsService.InitCancleInstall(state.SsWsPass, Xml_datacri, Xml_datadetail, state.SsApplication, state.CurrentPage);
                HdRunProcess.Value = "true";
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.ToString());
            }
        }

        private void JsReadyKeep()
        {
            try
            {
                int result = InsService.of_process_keep_ast(state.SsWsPass);
                if (result == 1)
                {
                    LtServerMessage.Text = WebUtil.CompleteMessage("เตรียมข้อมูลการเรียกเก็บเรียบร้อย");
                }
                else
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("เกิดข้อมูลผิดพลาดไม่สามารถทำดำเนินการได้");
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }

        }
    }
}