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
using CoreSavingLibrary.WcfNAccount;  //เพิ่มเข้ามา
using Sybase.DataWindow;  //เพิ่มเข้ามา
using System.Globalization;  //เพิ่มเข้ามา
using System.Data.OracleClient;  //เพิ่มเข้ามา
using DataLibrary; //เพิ่มเข้ามา
using System.Web.Services.Protocols; //เพิ่มเข้ามา

namespace Saving.Applications.account
{
    public partial class w_acc_bftrnpay_detail : PageWebSheet, WebSheet
    {
        private CultureInfo th;
        protected String postNewClear;
        protected String jsPostDetail;
        protected String jsPostDeleteDetail;
        protected String jsRetrieve;
        private DwThDate tDwHead;

        private void JspostNewClear()
        {
            Dw_Head.Reset();
            Dw_Head.InsertRow(0);
            Dw_main.Reset();
            Dw_main.InsertRow(0);
            Dw_Detail.Reset();
            Dw_Detail.InsertRow(0);
        }

        private void JsRetrieve()
        {
            DateTime pay_date = Dw_Head.GetItemDateTime(1, "trnpay_date");
            Dw_main.Retrieve(pay_date);
        }

        private void JsPostDetail()
        {
            String AcNo = HdAcNo.Value;
            Dw_Detail.Retrieve(AcNo);
        }

        private void JsPostDeleteDetail()
        {
            Int16 RowDetail = Convert.ToInt16(Hdrow.Value);
            String Pay_docno = Dw_main.GetItemString(RowDetail, "paytrnbank_docno");
            DateTime pay_date = Dw_Head.GetItemDateTime(1, "trnpay_date");
            try
            {
                String sql = @"Update accpaytrnbank set paytrnbank_status = '"+ -9 +"' WHERE paytrnbank_docno ='" + Pay_docno + @"' and coop_id ='" + state.SsCoopId + @"'";
                WebUtil.ExeSQL(sql);
                String sql2 = @"DELETE FROM accpaytrnbankdet WHERE paytrnbank_docno ='" + Pay_docno + @"' and coop_id ='" + state.SsCoopId + @"'";
                WebUtil.ExeSQL(sql2);
                LtServerMessage.Text = WebUtil.CompleteMessage("ลบข้อมูลเรียบร้อยแล้ว");
                Dw_main.Retrieve(pay_date);
                
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
            }    
        }



        public void InitJsPostBack()
        {
            postNewClear = WebUtil.JsPostBack(this, "postNewClear");
            jsPostDetail = WebUtil.JsPostBack(this, "jsPostDetail");
            jsPostDeleteDetail = WebUtil.JsPostBack(this, "jsPostDeleteDetail");
            jsRetrieve = WebUtil.JsPostBack(this, "jsRetrieve");
            tDwHead = new DwThDate(Dw_Head, this);
            tDwHead.Add("trnpay_date", "trnpay_tdate");
        }

        public void WebSheetLoadBegin()
        {
            this.ConnectSQLCA();

            th = new CultureInfo("th-TH");
            Dw_Head.SetTransaction(sqlca);
            Dw_main.SetTransaction(sqlca);
            Dw_Detail.SetTransaction(sqlca);

            if (!IsPostBack)
            {
                JspostNewClear();
                Dw_Head.SetItemDateTime(1, "trnpay_date", state.SsWorkDate);
                Dw_Head.SetItemString(1, "trnpay_tdate", state.SsWorkDate.ToString("ddMMyyyy", new CultureInfo("th-TH")));
                Dw_main.Retrieve(state.SsWorkDate);
                
            }
            else
            {
                this.RestoreContextDw(Dw_Head);
                this.RestoreContextDw(Dw_main);
                this.RestoreContextDw(Dw_Detail);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postNewClear")
            {
                JspostNewClear();
            }
            if (eventArg == "jsPostDetail")
            {
                JsPostDetail();
            }
            if (eventArg == "jsPostDeleteDetail")
            {
                JsPostDeleteDetail();
            }
            if (eventArg == "jsRetrieve")
            {
                JsRetrieve();
            }
            
        }

        public void SaveWebSheet()
        {
            
        }

        public void WebSheetLoadEnd()
        {
            Dw_Head.SaveDataCache();
            Dw_main.SaveDataCache();
            Dw_Detail.SaveDataCache();
        }
    }
}