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
using CommonLibrary;
using DBAccess;
using Sybase.DataWindow;
using System.Globalization;

namespace Saving.Applications.app_assist
{
    public partial class w_sheet_as_appinsnew : PageWebSheet, WebSheet
    {
        private DwThDate tDwDetail;
        protected String IncDetail;
        protected String InstypeCode;
        protected String GetStatus;
        protected String newClear;




        #region WebSheet Members

        public void InitJsPostBack()
        {
            InstypeCode = WebUtil.JsPostBack(this, "InstypeCode");
            GetStatus = WebUtil.JsPostBack(this, "GetStatus");
            newClear = WebUtil.JsPostBack(this, "newClear");



            //IncDetail = WebUtil.JsPostBack(this, "IncDetail");

            tDwDetail = new DwThDate(dw_main, this);
            tDwDetail.Add("insaprove_date", "insaprove_tdate");

        }

        public void WebSheetLoadBegin()
        {
            this.ConnectSQLCA();
            if (IsPostBack)
            {

                try
                {
                    dw_main.RestoreContext();
                    dw_detail.RestoreContext();


                }
                catch { }
            }
            if (dw_main.RowCount < 1)
            {


                dw_main.InsertRow(0);
                dw_detail.InsertRow(0);
                DwUtil.RetrieveDataWindow(dw_main, "as_appinsnew.pbl", tDwDetail, null);
                SetApproveTDateAllRow();
                //dw_search.SetItemDateTime(1, "compaccept_date", state.SsWorkDate);
                tDwDetail.Eng2ThaiAllRow();


            }


        }

        public void CheckJsPostBack(string eventArg)
        {
            //if (eventArg == "IncDetail")
            //{
            //    JsIncDetail();
            //}
            if (eventArg == "InstypeCode")
            {
                JsInstypeCode();
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

                DwUtil.UpdateDateWindow(dw_main, "as_appinsnew.pbl", "INSREQNEW");
                String member_no;
                int insreq_status;
                int row = dw_main.RowCount;
                for (int i = 1; i <= row; i++)
                {
                    insreq_status = Convert.ToInt32(dw_main.GetItemDecimal(i, "insreq_status"));
                    member_no = dw_main.GetItemString(i, "member_no");
                    if (insreq_status == 1)
                    {
                        new WsInsurance.Insurance().SaveAppinsnew(state.SsWsPass, member_no);
                    }
                }
                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลเรียบร้อย");
                DwUtil.RetrieveDataWindow(dw_main, "as_appinsnew.pbl", tDwDetail, null);


            }
            catch (Exception ex)
            {

                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }


        }

        public void WebSheetLoadEnd()
        {
            //dw_main.SaveDataCache();
            //dw_detail.SaveDataCache();
            //dw_main.Retrieve();
            //tDwDetail.Eng2ThaiAllRow();
        }

        #endregion

        private void JsIncDetail()
        {
            //int rowNumber = Convert.ToInt16(HfRowNumber.Value);
            //string rateCode = dw_main.GetItemString(rowNumber, "insreqdoc_no");
            //dw_main.Reset();
            //dw_main.Retrieve();
            //dw_detail.Reset();
            //dw_detail.Retrieve(rateCode);
            //tDwDetail.Eng2ThaiAllRow();
        }
        private void JsInstypeCode()
        {
            try
            {
                //object ff = dw_detpay.GetItemDecimal(1, "period_payamt");
                String inscode = HDinstype_code.Value;
                object[] argsDwMain = new object[1] { inscode };
                //dw_detail.Reset();
                DwUtil.RetrieveDataWindow(dw_detail, "as_appinsnew.pbl", null, argsDwMain);
                //DwUtil.DeleteLastRow(dw_detail);
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }

        }

        private void SetApproveTDateAllRow()
        {
            int r = dw_main.RowCount;
            for (int j = 1; j <= r; j++)
            {
                dw_main.SetItemDateTime(j, "insaprove_date", state.SsWorkDate);
            }
        }
        private void JsPostGetStatus()
        {
            Decimal status = Convert.ToDecimal(Hstatus.Value);
            Int32 row = Convert.ToInt32(hrow.Value);
            dw_main.SetItemDecimal(row, "insreq_status", status);

        }
        private void JsNewClear()
        {
            dw_main.Reset();
            dw_detail.Reset();
            dw_main.InsertRow(0);
            dw_detail.InsertRow(0);
            DwUtil.RetrieveDataWindow(dw_main, "as_appinsnew.pbl", tDwDetail, null);

        }



    }
}
