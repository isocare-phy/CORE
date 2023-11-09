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
    public partial class w_sheet_pm_financial_investment : PageWebSheet, WebSheet
    {
        protected String RunProcess;
        protected String GetValue;

        public void InitJsPostBack()
        {
            RunProcess = WebUtil.JsPostBack(this, "RunProcess");
            GetValue = WebUtil.JsPostBack(this, "GetValue");
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                DwMain.InsertRow(0);
                DwMain.SetItemDecimal(1,"setting_type",1);

                //try
                //{
                //    DwUtil.RetrieveDataWindow(DwDetail, "pm_investment.pbl",null,null);
                //    string test = DwDetail.GetItemString(1,"account_no");
                //}
                //catch
                //{
                //    DwDetail.Reset();
                //}
            }
            else
            {
                this.RestoreContextDw(DwDetail);
                this.RestoreContextDw(DwMain);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "RunProcess")
            {
                jsRunProcess();
            }
            if (eventArg == "GetValue")
            {
                JsGetValue();
            }

        }

        public void SaveWebSheet()
        {
            
        }

        public void WebSheetLoadEnd()
        {
            DwDetail.SaveDataCache();
            DwMain.SaveDataCache();
        }
        public void ChangeSettingType()
        {
            //try
            //{
            //    decimal setting_type = DwMain.GetItemDecimal(1, "setting_type");
            //    DwDetail.Reset();
            //    try
            //    {
            //        DwUtil.RetrieveDataWindow(DwDetail, "pm_investment.pbl", null, null);
            //        string test = DwDetail.GetItemString(1,"account_no");
            //    }
            //    catch { DwDetail.Reset(); }
            //    if (setting_type > 0)
            //    {
            //        DwDetail.SetFilter("setting_type =" + setting_type + "");
            //        DwDetail.Filter();
            //    }
                
            //}
            //catch 
            //{ 
            //    //LtServerMessage.Text = WebUtil.ErrorMessage("กรุณาเลือกระยะอีกครั้ง"); 
            //}

        }
        public void JsGetValue()
        {
            decimal year = 0;
            decimal setting_type = 0;
            int status = 1;
            try
            {
                year = DwMain.GetItemDecimal(1,"year_setting");
                try
                {
                    setting_type = DwMain.GetItemDecimal(1, "setting_type");
                }
                catch
                {
                    setting_type = 1;
                    status = 0;   
                }
            }
            catch 
            {
                year = 0;
                status = 0;
            }
            if (status == 1)
            {
                try
                {
                    DwUtil.RetrieveDataWindow(DwDetail, "pm_investment.pbl", null, year,setting_type);
                    string test = DwDetail.GetItemString(1, "account_no");
                    //LtServerMessage.Text = WebUtil.CompleteMessage("ดึงข้อมูลสำเร็จ");
                }
                catch
                {
                    //LtServerMessage.Text = WebUtil.CompleteMessage("ดึงข้อมูลสำเร็จ แต่ไม่พบข้อมูล");
                    DwDetail.Reset();
                }
            }
            else
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("กรุณาเลือกข้อมูลให้ครบ");
            }

        }
        public void jsRunProcess()
        {
            n_pmClient svPm = wcf.NPm;
            short year = 0;
            try
            {
                 year = Convert.ToInt16(DwMain.GetItemDecimal(1, "year_setting"));
            }
            catch 
            {
                year = 0;
                LtServerMessage.Text = WebUtil.ErrorMessage("กรุณาระบุปีที่ต้องการประมวล");
            }
            if (year != 0)
            {
                try
                {
                    int result = svPm.of_process_shortlong(state.SsWsPass, year);
                    if (result == 1)
                    {
                        LtServerMessage.Text = WebUtil.CompleteMessage("จัดทำข้อมูลสำเร็จ");

                    }
                    else
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage("จัดทำข้อมูลไม่สำเร็จ");
                    }
                    JsGetValue();
                }
                catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
            }
        }
    }
}