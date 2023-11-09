using System;
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
using Saving.WsShrlon;
using CommonLibrary;
using Saving.WsCommon;
using System.Web.Services.Protocols;


namespace Saving.Applications.app_assist
{
    public partial class w_sheet_as_appinsresign : PageWebSheet, WebSheet
    {
        private DwThDate tDwsearch;
        private String pbl = "as_appinsresign.pbl";
        protected String MemberNo;
        protected String GetStatus;
        protected String newClear;

        
        public void InitJsPostBack()
        {
            MemberNo = WebUtil.JsPostBack(this, "MemberNo");
            GetStatus = WebUtil.JsPostBack(this, "GetStatus");
            newClear = WebUtil.JsPostBack(this, "newClear");

            tDwsearch = new DwThDate(dw_search, this);
            tDwsearch.Add("closeins_date", "closeins_tdate");
            tDwsearch.Add("compaccept_date", "compaccept_tdate");


        }

        public void WebSheetLoadBegin()
        {

            this.ConnectSQLCA();
            if (IsPostBack)
            {

                try
                {
                    dw_search.RestoreContext();
                    dw_list.RestoreContext();


                }
                catch { }
            }
            if (dw_search.RowCount < 1)
            {

                //dw_search.InsertRow(0);
                dw_list.InsertRow(0);
                DwUtil.RetrieveDataWindow(dw_search, pbl, tDwsearch, null);
                //dw_search.SetItemDateTime(1, "closeins_date", state.SsWorkDate);
                //dw_search.SetItemDateTime(1, "compaccept_date", state.SsWorkDate);
                tDwsearch.Eng2ThaiAllRow();


            }

        }


        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "MemberNo")
            {
                JsPostMemberNo();
            }
            else if (eventArg == "GetStatus")
            {
                JsPostGetStatus();
            }
            else if (eventArg == "newClear")
            {
                JsNewClear();
            }


        }

        public void SaveWebSheet()
        {
            try
            {

                DwUtil.UpdateDateWindow(dw_search, "as_appinsresign.pbl", "INSREQRESIGN");
                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลเสร็จเรียบร้อย");
                DwUtil.RetrieveDataWindow(dw_search, pbl, tDwsearch, null);

            }
            catch (Exception ex)
            {

                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        public void WebSheetLoadEnd()
        {
          


        }
        private void JsPostMemberNo()
        {
            try
            {
              
                //object ff = dw_detpay.GetItemDecimal(1, "period_payamt");
                String member_no = HMember_no.Value;
                object[] argsDwMain = new object[1] { member_no }; 
                dw_list.Reset();
                DwUtil.RetrieveDataWindow(dw_list, "as_appinsresign.pbl", null, argsDwMain);
                DwUtil.DeleteLastRow(dw_list);
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }
        private void JsPostGetStatus()
        {
            Decimal status = Convert.ToDecimal(Hstatus.Value);
            Int32 row = Convert.ToInt32(hrow.Value);
            dw_search.SetItemDecimal(row, "reqresign_status", status);
            dw_list.InsertRow(0);
            DwUtil.DeleteLastRow(dw_list);

           
        }
        private void JsNewClear() {
            dw_search.Reset();
            dw_list.Reset();
            //dw_search.InsertRow(0);
            dw_list.InsertRow(0);
            DwUtil.RetrieveDataWindow(dw_search, pbl, tDwsearch, null);
         
        }

    }
}