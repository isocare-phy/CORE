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
    public partial class w_sheet_pm_add_intreceivedate : PageWebSheet, WebSheet
    {
        protected String postAccountNO;
        protected String RunProcess;
        private DwThDate tDwMain;
        private DwThDate tDwDetail;
        
        public void InitJsPostBack()
        {
            tDwMain = new DwThDate(DwMain, this);
            tDwMain.Add("start_intdate", "start_inttdate");

            tDwDetail = new DwThDate(DwDetail, this);
            tDwDetail.Add("start_calint_date", "start_calint_tdate");
            tDwDetail.Add("last_cal_to_date", "last_cal_to_tdate");
            tDwDetail.Add("due_date", "due_tdate");

            postAccountNO = WebUtil.JsPostBack(this, "postAccountNO");
            RunProcess = WebUtil.JsPostBack(this, "RunProcess");
        }

        public void WebSheetLoadBegin()
        {
          if(!IsPostBack){

                  DwMain.InsertRow(0);
                  DwMain.SetItemDecimal(1, "int_timedue",0);
          }
          else
          {
              this.RestoreContextDw(DwMain,tDwMain);
              this.RestoreContextDw(DwDetail,tDwDetail);
          }
        }

        public void CheckJsPostBack(string eventArg)
        {
            switch (eventArg)
            {
                case "postAccountNO":
                    JsPostAccountNo();
                    break;
                case "RunProcess":
                    JsRunProcess();
                    break;
            }
        }

        public void SaveWebSheet()
        {
            n_pmClient svPm = wcf.NPm;
            string as_due_date_xml = "";
            string account_no = "";
            int result=0;
            
            try
            {
                account_no = DwMain.GetItemString(1,"account_no");
                account_no = int.Parse(account_no).ToString("0000000000");
                as_due_date_xml = DwDetail.Describe("Datawindow.data.XML");
                result = svPm.of_updateduedate(state.SsWsPass,as_due_date_xml,account_no);
                if (result == 1)
                {
                    int res;
                    string sqlup = "update pminvestmaster set cleanprice_amt = (select min(account_balance) from pminvestduedate where recint_flag = 1 and account_no = '" + account_no + "' ),lastcalint_date = (select min(start_calint_date) from pminvestduedate where recint_flag = 0 and account_no = '" + account_no + "')where account_no = '" + account_no + "'";
                    Sta pm_up = new Sta(state.SsConnectionString);
                    res = pm_up.Exe(sqlup);
                    pm_up.Close();
                    if (res == 1)
                    {
                        LtServerMessage.Text = WebUtil.CompleteMessage("ปรับปรุงข้อมูลสำเร็จ");
                    }
                    else { LtServerMessage.Text = WebUtil.ErrorMessage("ปรับปรุงข้อมูลไม่สำเร็จ"); }
                }
                else { LtServerMessage.Text = WebUtil.ErrorMessage("ปรับปรุงข้อมูลไม่สำเร็จ"); }
            }
            catch(Exception ex)
            { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }

        }

        public void WebSheetLoadEnd()
        {
            tDwMain.Eng2ThaiAllRow();
            tDwDetail.Eng2ThaiAllRow();
            DwMain.SaveDataCache();
            DwDetail.SaveDataCache();
        }
        public void JsRunProcess()
        {
            n_pmClient svPm = wcf.NPm;
            string result_xml = "";
            string as_condition_xml = DwMain.Describe("Datawindow.data.XML");
            try
            {

                 result_xml = svPm.of_genduedate(state.SsWsPass, as_condition_xml);
                 try
                 {
                     //string coop_id = "001001";
                     DwDetail.Reset();
                     DwDetail.ImportString(result_xml, FileSaveAsType.Xml);
                     LtServerMessage.Text = WebUtil.CompleteMessage("จัดทำข้อมูลสำเร็จ");
                 }
                 catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }

            }
            catch(Exception ex)
            { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
        }
        public void JsPostAccountNo()
        {
            string acc_no = "";
            string coop_id=state.SsCoopId;
            try
            {
                acc_no = DwMain.GetItemString(1, "account_no");
                acc_no = int.Parse(acc_no).ToString("0000000000");
                DwUtil.RetrieveDataWindow(DwMain, "pm_investment.pbl", null, coop_id,acc_no = int.Parse(acc_no).ToString("0000000000"));
            }
            catch 
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบข้อมูล account_no");
            }
            try
            {
                //coop_id = DwMain.GetItemString(1,"coop_id");
                coop_id = state.SsCoopId;
                
                try
                {
                    DwUtil.RetrieveDataWindow(DwDetail, "pm_investment.pbl", null, coop_id,acc_no = int.Parse(acc_no).ToString("0000000000"));
                    string test = DwDetail.GetItemString(1,"account_no");
                    test = int.Parse(test).ToString("0000000000");
                    LtServerMessage.Text = WebUtil.WarningMessage("ระบบจะทำการล้างข้อมูลวันที่กำหนดรับดอกเบี้ยเดิมทิ้ง หากมีการจัดทำข้อมูลใหม่");
                }
                catch 
                { 
                    DwDetail.Reset();
                }
            }
            catch
            { 
            
            }
        }

    }
}