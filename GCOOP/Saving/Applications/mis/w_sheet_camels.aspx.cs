using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary.WcfNCommon;
using CoreSavingLibrary.WcfNShrlon;
using Sybase.DataWindow;
using System.Web.Services.Protocols;
using CoreSavingLibrary.WcfNMis;

namespace Saving.Applications.mis
{
    public partial class w_sheet_camels : PageWebSheet, WebSheet
    {
        private DwTrans SQLCA;
        private n_misClient MisService;
        private n_commonClient commonService;

        protected String jsClickSubmit;

        public void InitJsPostBack()
        {
            jsClickSubmit = WebUtil.JsPostBack(this, "jsClickSubmit");
        }

        public void WebSheetLoadBegin()
        {
            
            try
            {
                MisService = wcf.NMis;
                commonService = wcf.NCommon;
            }
            catch
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ติดต่อ Web Service ไม่ได้");
                return;
            }
            if (IsPostBack)
            {
                this.RestoreContextDw(dw_main);
                this.RestoreContextDw(dw_cri);

            }
            else {
                dw_main.InsertRow(0);
                dw_cri.InsertRow(0);

                string toDayNow = DateTime.Now.ToString("M/yyyy");
                string[] DateNow = toDayNow.Split('/');

                try
                {
                    Decimal endMonth = Convert.ToDecimal(DateNow[0]);
                    Decimal endYear = Convert.ToDecimal(DateNow[1]) + 543;
                    Decimal startMonth = endMonth;
                    Decimal startYear = endYear - 1;

                    dw_cri.SetItemDecimal(1, "start_month", startMonth);
                    dw_cri.SetItemDecimal(1, "end_month", endMonth);
                    dw_cri.SetItemDecimal(1, "start_year", startYear);
                    dw_cri.SetItemDecimal(1, "end_year", endYear);
                }
                catch {}
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "jsClickSubmit")
            {
                ClickSubmit();
            }            
        }

        public void SaveWebSheet()
        {
           
        }

        public void WebSheetLoadEnd()
        {
            dw_cri.SaveDataCache();
            dw_main.SaveDataCache();
        }
       
        public void ClickSubmit()
        {
            Int16 startMonth = 0;
            String startYear = "";
            Int16 endMouth = 0;
            String endYear = "";

            try
            {
                try
                {startMonth = Convert.ToInt16(dw_cri.GetItemDecimal(1, "start_month"));}
                catch { startMonth = 0; }
                try { startYear = Convert.ToString(dw_cri.GetItemString(1, "start_year")); }
                catch { startYear = ""; }
                try { endMouth = Convert.ToInt16(dw_cri.GetItemDecimal(1, "end_month")); }
                catch { endMouth = 0; }
                try { endYear = Convert.ToString(dw_cri.GetItemString(1, "end_year")); }
                catch { endYear = ""; }

                if ((startMonth == 0) || (startYear == "") || (endMouth == 0) || (endYear == ""))
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("กรุณาเลือกข้อมูลให้ครบ");
                }
                else
                {
                    //WsMis.Mis Ms = wcf.NMis;
                    String xmlMain = MisService.RatioGetRatiosValues(state.SsWsPass, startMonth, startYear, endMouth, endYear);
                    dw_main.Reset();
                    dw_main.ImportString(xmlMain, FileSaveAsType.Xml);
                }
            }
            catch (Exception ex)
            { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
        }
    }
}