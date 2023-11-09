using System;
using CoreSavingLibrary;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using DataLibrary;
using Sybase.DataWindow;
using CoreSavingLibrary.WcfNPm;
using System.Web.Services.Protocols;

namespace Saving.Applications.pm
{
    public partial class w_sheet_pm_receivable : PageWebSheet, WebSheet
    {
        private DwThDate tDwMain;
        protected String JsPostCalintDate;
        protected String RunProcess;
        public void InitJsPostBack()
        {
            RunProcess = WebUtil.JsPostBack(this, "RunProcess");
            JsPostCalintDate = WebUtil.JsPostBack(this, "JsPostCalintDate");

            tDwMain = new DwThDate(DwMain, this);
            tDwMain.Add("calintto_date", "calintto_tdate");
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                DateTime last_month_date = state.SsWorkDate;
                
                int day = DateTime.DaysInMonth(last_month_date.Year, last_month_date.Month);
                last_month_date = DateTime.ParseExact(day.ToString("00") + last_month_date.ToString("MMyyyy"), "ddMMyyyy", null);

                
                DwMain.InsertRow(0);
                DwMain.SetItemDateTime(1, "calintto_date",last_month_date);
            }
            else
            {
                this.RestoreContextDw(DwMain);
            }
            //tDwMain.Eng2ThaiAllRow();
        }

        public void CheckJsPostBack(string eventArg)
        {

            if (eventArg == "JsPostCalintDate")
            {
                //ChangeCalintDate();
            }
            if (eventArg == "RunProcess")
            {
                jsRunProcess();
            }
        }

        public void SaveWebSheet()
        {
            
        }

        public void WebSheetLoadEnd()
        {
            tDwMain.Eng2ThaiAllRow();
            DwMain.SaveDataCache();
            //DwDetail.SaveDataCache();
        }        //public void ChangeCalintDate()
        //{
        //    try
        //    {
        //        DateTime entry_date = DateTime.ParseExact(hdate.Value, "ddMMyyyy", WebUtil.TH);
        //        DwMain.SetItemDateTime(1, "calintto_date", entry_date);
        //    }
        //    catch
        //    { }
        //}

        public void jsRunProcess()
        {
            n_pmClient svPm = wcf.NPm;
            DateTime calintto_date = DateTime.Now;
            
            try
            {
                calintto_date = DwMain.GetItemDateTime(1, "calintto_date");
                int result = svPm.of_process_intrecvable(state.SsWsPass,calintto_date);//runprocess
                if (result == 1)
                {
                    LtServerMessage.Text = WebUtil.CompleteMessage("จัดทำข้อมูลสำเร็จ");
                }
                else
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("จัดทำข้อมูลไม่สำเร็จ");
                }
            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
        }
    }
}