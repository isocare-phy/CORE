using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommonLibrary;
using CommonLibrary.WsKeeping;
using CommonLibrary.WsFinance;
using Sybase.DataWindow;
using System.Web.Services.Protocols;

namespace Saving.Applications.app_assist
{
    public partial class w_sheet_kt_retire_rotation : PageWebSheet, WebSheet
    {
        private Keeping KpService;

        protected String jspostRetrieve;
        protected String jspostProcess;
        protected String ClearProcess;
        protected String jspostaccid_flag;
        protected String postmoneytype_code;
        private DwThDate tdwCri;
        private string pbl = "kt_50bath.pbl";

        private Finance Fin;

        public void InitJsPostBack()
        {
            jspostRetrieve = WebUtil.JsPostBack(this, "jspostRetrieve");
            jspostProcess = WebUtil.JsPostBack(this, "jspostProcess");
            ClearProcess = WebUtil.JsPostBack(this, "ClearProcess");
            jspostaccid_flag = WebUtil.JsPostBack(this, "jspostaccid_flag");
            postmoneytype_code = WebUtil.JsPostBack(this, "postmoneytype_code");

            tdwCri = new DwThDate(DwCri, this);
            tdwCri.Add("operate_date", "operate_tdate");
            tdwCri.Add("slip_date", "slip_tdate");
        }

        public void WebSheetLoadBegin()
        {
            HdRunProcess.Value = "false";
            KpService = WsCalling.Keeping;
            Fin = WsCalling.Finance;
            if (!IsPostBack)
            {
                DwCri.InsertRow(0);
                InitInterate();
                DwCri.SetItemString(1, "moneytype_code", "CSH");
            }
            else
            {
                this.RestoreContextDw(DwMain);
                this.RestoreContextDw(DwCri);
            }
            tdwCri.Eng2ThaiAllRow();
        }

        public void CheckJsPostBack(string eventArg)
        {
            switch (eventArg)
            {
                case "jspostRetrieve":
                    GetInfo();
                    break;
                case "jspostProcess":
                    KtRunProcess();
                    break;
                case "ClearProcess":
                    JsNewClear();
                    break;
                case "jspostaccid_flag":
                    initmoneyandtoaccid();
                    break;
                case "postmoneytype_code":
                    initmoneyandtoaccid();
                    break;
            }
        }

        public void SaveWebSheet()
        {
        }

        public void WebSheetLoadEnd()
        {
            try
            {
                DwUtil.RetrieveDDDW(DwCri, "moneytype_code", pbl, null);
                string moneytype_code = DwUtil.GetString(DwCri, 1, "moneytype_code", "");
                if (moneytype_code != "")
                {
                    decimal accid_flag = 0;
                    try {accid_flag = DwCri.GetItemDecimal(1, "accid_flag"); }
                    catch { }
                    if (accid_flag == 1)
                    {
                        DwUtil.RetrieveDDDW(DwCri, "tofrom_accid", pbl, moneytype_code);
                    }
                }
            }
            catch
            {
            }
            DwCri.SaveDataCache();
            DwMain.SaveDataCache();
        }

        private void JsNewClear()
        {
            DwCri.Reset();
            InitInterate();
            DwMain.Reset();
            DwCri.SetItemDateTime(1, "operate_date", state.SsWorkDate);
            DwCri.SetItemDateTime(1, "slip_date", state.SsWorkDate);
            DwCri.SetItemString(1, "entry_id", state.SsUsername);

        }

        private void InitInterate()
        {
            try
            {
                str_keep astr_keep = new str_keep();
                astr_keep.xml_option = "";// DwCri.Describe("DataWindow.Data.XML");


                int result = KpService.of_init_proc_cls_asn(state.SsWsPass, ref astr_keep);
                //String Sresult = astr_keep.xml_option;
                DwUtil.ImportData(astr_keep.xml_option, DwCri, tdwCri, FileSaveAsType.Xml);
                DwCri.SetItemDateTime(1, "operate_date", state.SsWorkDate);
                DwCri.SetItemDateTime(1, "slip_date", state.SsWorkDate);
                DwCri.SetItemString(1, "entry_id", state.SsUsername);
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถทำรายการได้" + ex);
            }
        }

        private void GetInfo()
        {
            string proc_option = "";
            DateTime operate_date = DateTime.MinValue;

            try
            {
                string toperate_date = DwCri.GetItemString(1, "operate_tdate");
                if (toperate_date.Length == 8)
                {
                    DateTime dt = DateTime.ParseExact(toperate_date, "ddMMyyyy", WebUtil.TH);
                    DwMain.SetItemDateTime(1, "operate_date", dt);
                }
            }
            catch { }
            try
            {
                proc_option = DwCri.GetItemString(1, "proc_option");
                operate_date = DwCri.GetItemDateTime(1, "operate_date");
            }
            catch
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถดึงข้อมูลจากเงื่อนไขได้");
                return;
            }

            DwUtil.RetrieveDataWindow(DwMain, "kt_50bath.pbl", null, proc_option, operate_date);
        }

        private void KtRunProcess()
        {
            try
            {
                str_keep astr_keep = new str_keep();
                astr_keep.xml_option = DwCri.Describe("DataWindow.Data.XML");

                int result = KpService.KtRetireProcess(state.SsWsPass, astr_keep, state.SsApplication, state.CurrentPage);

                HdRunProcess.Value = "true";
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        private void initmoneyandtoaccid()
        {
            string moneytype = "";
            try { moneytype = DwCri.GetItemString(1, "moneytype_code"); }
            catch { }
            decimal accid_flag = 0;
            try { accid_flag = DwCri.GetItemDecimal(1, "accid_flag"); }
            catch { }
            if (accid_flag == 1)
            {
                if (moneytype != "TRN")
                {
                    string tofrom_accid = Fin.DefaultAccId(state.SsWsPass, moneytype);
                    DwCri.SetItemString(1, "tofrom_accid", tofrom_accid);
                    DwCri.SetItemString(1, "to_system", "");
                }
                else
                {
                    DwCri.SetItemString(1, "tofrom_system", "DEP");
                    DwCri.SetItemString(1, "tofrom_accid", "");
                }
            }
            else
            {
                DwCri.SetItemString(1, "tofrom_accid", "");
            }
        }
    }
}