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
using CoreSavingLibrary.WcfNAccount;
using Sybase.DataWindow;

namespace Saving.Applications.account
{
    public partial class w_sheet_moreaccount_activity : PageWebSheet,WebSheet
    {
        protected String postDetail;
        protected String postCRDR;
        protected String postAccountId;
        protected String insertRowDetail;
        protected String deleteRowDetail;
        private n_accountClient accService;

        #region WebSheet Members

        public void InitJsPostBack()
        {
            postDetail = WebUtil.JsPostBack(this, "postDetail");
            postCRDR = WebUtil.JsPostBack(this, "postCRDR");
            postAccountId = WebUtil.JsPostBack(this, "postAccountId");
            insertRowDetail = WebUtil.JsPostBack(this, "insertRowDetail");
            deleteRowDetail = WebUtil.JsPostBack(this, "deleteRowDetail");
        }

        public void WebSheetLoadBegin()
        {
            try
            {
                accService = wcf.NAccount;
            }
            catch
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ติดต่อ Webservice ไม่ได้");
                return;
            }

            if (!IsPostBack)
            {
                DwMain.InsertRow(0);
            }
            else
            {
                this.RestoreContextDw(DwMain);
                this.RestoreContextDw(DwDetail);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postDetail")
            {
                JsPostDetail();
            }
            else if (eventArg == "insertRowDetail")
            {
                JsInsertRowDetail();
            }
            else if (eventArg == "deleteRowDetail")
            {
                JsDeleteRowDetail();
            }
            else if (eventArg == "postAccountId")
            {
                JsPostAccountId();
            }
        }

        public void SaveWebSheet()
        {
            String xmlDetail = "";
            try
            {
                xmlDetail = DwDetail.Describe("DataWindow.Data.XML");
                int result = accService.of_save_account_splitactivity(state.SsWsPass, xmlDetail);   
                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลเรียบร้อยแล้ว");  

            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        public void WebSheetLoadEnd()
        {
            WebUtil.RetrieveDDDW(DwDetail, "account_id", "acc_moreaccount_activity.pbl", state.SsCoopId);
            WebUtil.RetrieveDDDW(DwDetail, "account_id_1", "acc_moreaccount_activity.pbl",state.SsCoopId);
            DwMain.SaveDataCache();
            DwDetail.SaveDataCache();
        }

        #endregion

        private void JsPostDetail()
        {

            short year = short.Parse((Convert.ToInt32(DwMain.GetItemDecimal(1, "acc_year")) - 543).ToString());
            short period = short.Parse(DwMain.GetItemDecimal(1, "acc_period").ToString());
            try
            {
                String xmlDetail = "";
                xmlDetail = accService.of_init_account_splitactivity(state.SsWsPass, year, period);
                //xmlDetail = accService.GetAccountSplitActivity(state.SsWsPass, year, period,state.SsCoopId);
                                 
                DwDetail.Reset();
                try
                {
                    DwUtil.ImportData(xmlDetail, DwDetail, null, FileSaveAsType.Xml); 
                }
                catch
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่มีรายการปีที่ : " + year + " งวดที่ : " + period);
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        private void JsInsertRowDetail()
        {
            Decimal year = DwMain.GetItemDecimal(1, "acc_year");
            Decimal period = DwMain.GetItemDecimal(1, "acc_period");
            DwDetail.InsertRow(0);
            int row = DwDetail.RowCount;
            DwDetail.SetItemDecimal(row, "acc_year", year - 543);
            DwDetail.SetItemDecimal(row, "acc_period", period);
            DwDetail.SetItemString(row, "branch_id", state.SsCoopId);
        }

        private void JsDeleteRowDetail()
        {
            int row = int.Parse(HdDetailRow.Value);
            DwDetail.DeleteRow(row);
        }

        private void JsPostAccountId()
        {
            int row = int.Parse(HdDetailRow.Value);
            String accountId = DwDetail.GetItemString(row, "account_id");  
            DataWindowChild dc = DwDetail.GetChild("account_id_1");
            dc.SetFilter("account_id = '" + accountId + "'");
            dc.Filter();
        }
    }
}
