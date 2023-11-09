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
    public partial class w_sheet_acc_contuse_profit : PageWebSheet,WebSheet
    {
        protected String postDetail;
        protected String postBalance;
        protected String insertRowDetail;
        protected String deleteRowDetail;
        private n_accountClient accService;
        protected String menubarnew;

        #region WebSheet Members

        public void InitJsPostBack()
        {
            postDetail = WebUtil.JsPostBack(this, "postDetail");
            postBalance = WebUtil.JsPostBack(this, "postBalance");
            insertRowDetail = WebUtil.JsPostBack(this, "insertRowDetail");
            deleteRowDetail = WebUtil.JsPostBack(this, "deleteRowDetail");
            menubarnew = WebUtil.JsPostBack(this, "menubarnew");
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
            else if (eventArg == "menubarnew")
            {
                DwMain.Reset();
                DwMain.InsertRow(0);
                DwDetail.Reset();
            }
        }

        public void SaveWebSheet()
        {
            String xmlDetail = "";
            try
            {
                for (int i = 1; i <= DwDetail.RowCount; i++)
                {
                    DwDetail.SetItemString(i, "coop_id", state.SsCoopId);
                }
                xmlDetail = DwDetail.Describe("DataWindow.Data.XML");
                accService.of_save_contuseprofit(state.SsWsPass, xmlDetail);
                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลเรียบร้อยแล้ว");
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        public void WebSheetLoadEnd()
        {
            WebUtil.RetrieveDDDW(DwDetail, "acc_code", "acc_contuse_profit.pbl", state.SsCoopId );
            DwMain.SaveDataCache();
            DwDetail.SaveDataCache();
        }

        #endregion

        private void JsPostDetail()
        {
            short year = short.Parse((DwMain.GetItemDecimal(1, "acc_year") -543 ).ToString());
            short period = short.Parse(DwMain.GetItemDecimal(1, "acc_period").ToString());
            try
            {
                string xmlDetail = accService.of_init_contuseprofit(state.SsWsPass, year, period);   

                DwDetail.Reset();
                try
                {
                    DwUtil.ImportData(xmlDetail, DwDetail, null, FileSaveAsType.Xml);
                    WebUtil.RetrieveDDDW(DwDetail, "acc_code", "acc_contuse_profit.pbl", state.SsCoopId);
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
            string coopid = state.SsCoopId;
            try
            {
                DwDetail.SetItemDecimal(row, "acc_year", year -543);
                DwDetail.SetItemDecimal(row, "acc_period", period);
                DwDetail.SetItemString(row, "coop_id", coopid); 
                
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        private void JsDeleteRowDetail()
        {
            int row = int.Parse(HdDetailRow.Value);
            DwDetail.DeleteRow(row);
        }
    }
}
