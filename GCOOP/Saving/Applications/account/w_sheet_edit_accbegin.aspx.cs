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

using DataLibrary;
using Sybase.DataWindow;
using System.Data.OracleClient;
using System.Globalization;
using CoreSavingLibrary.WcfNAccount;
using System.Web.Services.Protocols;

namespace Saving.Applications.account
{
    public partial class w_sheet_edit_accbegin : PageWebSheet, WebSheet 
    {
        protected String postSumledger;
        protected String postDrCr;
        private n_accountClient accService;
        
        //==================================
        private void JsPostDrCr()
        {
            int RowDetail = Convert.ToInt32(Hd_row.Value);
            //Decimal sum_dr = Dw_footer.GetItemDecimal(1,"compute_2");
            //Decimal sum_cr = Dw_footer.GetItemDecimal(1, "compute_1");
            Decimal begin_dr_amount = Dw_main.GetItemDecimal(RowDetail, "begin_dr_amount");
            Decimal begin_cr_amount = Dw_main.GetItemDecimal(RowDetail, "begin_cr_amount");

            Dw_footer.SetItemDecimal(RowDetail, "begin_dr_amount", begin_dr_amount);
            Dw_footer.SetItemDecimal(RowDetail, "begin_cr_amount", begin_cr_amount);
        
        }


        private void JspostSumledger()
        {
            try 
            {
                n_accountClient accService = wcf.NAccount;
                int acc_year = state.SsWorkDate.Year;
                Int16 account_year = Convert.ToInt16(acc_year);
                String wsPass = state.SsWsPass;
                String branchID = state.SsCoopId;

                //เรียกใช้ webservice
                //int result = accService.GenerateFirstSumleger(wsPass, account_year, state.SsCoopId);
                int result = wcf.NAccount.of_add_first_sumleger_period(wsPass, account_year, state.SsCoopId);
                Dw_main.Retrieve(account_year, state.SsCoopId);
                Dw_footer.Retrieve(account_year, state.SsCoopId);
            }
            catch (SoapException ex)
            {
                //webutl จัดตัวหนังสือไว้ทำสีแดงให้ ตรงกลางจอ                    //webutil.soapmessage จะเอาerror มาใส่แทน      
                LtServerMessage.Text = WebUtil.ErrorMessage(WebUtil.SoapMessage(ex));
            }
            catch (Exception ex) 
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message); 
            }
        }
      
        //==================================

        #region WebSheet Members

        public void InitJsPostBack()
        {
            postSumledger = WebUtil.JsPostBack(this, "postSumledger");
            postDrCr = WebUtil.JsPostBack(this, "postDrCr");
        }

        public void WebSheetLoadBegin()
        {
            this.ConnectSQLCA();
            accService = wcf.NAccount;//ประกาศ new
            Dw_main.SetTransaction(sqlca);
            Dw_footer.SetTransaction(sqlca);
            if (!IsPostBack) 
            {

                int account_year = state.SsWorkDate.Year;
                String coop_id = state.SsCoopId;
                Dw_main.Retrieve(account_year, coop_id);
                Dw_footer.Retrieve(account_year, coop_id);
            } 
            else
            {
                this.RestoreContextDw(Dw_main);
                this.RestoreContextDw(Dw_footer);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postSumledger")
            {
                JspostSumledger();
            }
            else if (eventArg == "postDrCr") { 
            // Refresh ยอดรวม
                JsPostDrCr();
            }

        }

        public void SaveWebSheet()
        {
            //try
            //{
            //    AccountClient accService = wcf.NAccount;

            //    // การโยน ไฟล์ xml ไปให้ service
            //    String xmlBegin = Dw_main.Describe("Datawindow.Data.Xml");
            //    String wsPass = state.SsWsPass;
            //    String Coopid = state.SsCoopId;

            //    //เรียกใช้ webservice
            //    int result = accService.SaveBeginAcc(wsPass, xmlBegin, Coopid);
            //    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลเรียบร้อยแล้ว");
            //}
            //catch (SoapException ex)
            //{
            //    //webutl จัดตัวหนังสือไว้ทำสีแดงให้ ตรงกลางจอ                    //webutil.soapmessage จะเอาerror มาใส่แทน      
            //    LtServerMessage.Text = WebUtil.ErrorMessage(WebUtil.SoapMessage(ex));
            //}
            //catch (Exception ex)
            //{
            //    LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
            //}
            int year = 0;
            Int16 accperiod = 0;
            Decimal sum_dr = 0;
            Decimal sum_cr = 0;

             Sta ta = new Sta(sqlca.ConnectionString);
            try
            {
                
                String sql = @"select present_account_ye  from accconstant where coop_id = '" + state.SsCoopId + "'";
                Sdt dt = ta.Query(sql);
                if (dt.Next())
                {
                    year = Convert.ToInt16(dt.GetString("present_account_ye"));
                    //Dw_main.SetItemDecimal(1, "period", accperiod);
                }
            }
            catch{}

            ta.Close();

            Sta ta1 = new Sta(sqlca.ConnectionString);
            String sql1 = @"select max(period), max(dr_amount) , max(cr_amount) from accsumledgerperiod where account_year = '" + year + "' and coop_id = '" + state.SsCoopId + "'";
                Sdt dt1 = ta1.Query(sql1);
                if (dt1.Next())
                {
                    accperiod = Convert.ToInt16(dt1.GetString("max(period)"));
                    sum_dr = Convert.ToDecimal(dt1.GetString("max(dr_amount)"));
                    sum_cr = Convert.ToDecimal(dt1.GetString("max(cr_amount)"));
                    //Dw_main.SetItemDecimal(1, "period", accperiod);
                }

            ta1.Close();

            if (accperiod == 1 && sum_dr == 0 && sum_cr == 0)
            {


                try
                {
                    Dw_main.UpdateData();
                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลเสร็จเรียบร้อยแล้ว");
                }
                catch (Exception ex)
                {
                    //sqlca.Rollback();
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
                }
            }
            else {

                LtServerMessage.Text = WebUtil.WarningMessage("มีการผ่านรายการแล้ว กรุณายกเลิกผ่านรายการจนถึงงวดบัญชีที่ 1");
            }
            
        }

        public void WebSheetLoadEnd()
        {
            Dw_main.SaveDataCache();
            Dw_footer.SaveDataCache();
        }

        #endregion
    }
}
