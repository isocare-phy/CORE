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
using System.Web.Services.Protocols;
using System.Globalization;

namespace Saving.Applications.account
{
    public partial class w_acc_report_ledger : PageWebSheet, WebSheet
    {
        protected String postOpenReport;
        protected String changeValue;
        protected String saveData;
        protected String onSetDate;
        private n_accountClient Accsrv;
        private accFunction accF;
        private DwThDate tDwMain;

        #region WebSheet Members

        public void InitJsPostBack()
        {
            changeValue = WebUtil.JsPostBack(this, "changeValue");
            postOpenReport = WebUtil.JsPostBack(this, "postOpenReport");
            //onSetDate = WebUtil.JsPostBack(this, "onSetDate");

            tDwMain = new DwThDate(dw_main, this);
            tDwMain.Add("start_date", "start_tdate");

            tDwMain = new DwThDate(dw_main, this);
            tDwMain.Add("end_date", "end_tdate");

            tDwMain.Eng2ThaiAllRow();
        }

        public void WebSheetLoadBegin()
        {
            try
            {
                Accsrv = wcf.NAccount;
            }
            catch
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ติดต่อ Web Service ไม่ได้");
                return;
            }
            try
            {
                accF = new accFunction();
            }
            catch
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ติดต่อ Function ไม่ได้");
                return;
            }

            if (IsPostBack)
            {
                this.RestoreContextDw(dw_main);
                this.RestoreContextDw(dw_rpt);
            }
            if (dw_main.RowCount < 1)
            {
                dw_main.Reset();
                dw_main.InsertRow(0);
                dw_rpt.Reset();
                SetChildDW();
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postOpenReport")
            {
                this.JspostOpenReport();
            }
        }

        public void SaveWebSheet()
        {

        }

        public void WebSheetLoadEnd()
        {
            if (dw_main.RowCount > 1)
            {
                dw_main.DeleteRow(dw_main.RowCount);
            }
            dw_main.SaveDataCache();
            dw_rpt.SaveDataCache();
        }

        #endregion

        private void JspostOpenReport()
        {
            string str_tmp = "";
            //ChangeRpt();
            dw_rpt.Reset();
            DateTime sdt = DateTime.ParseExact(HdSDateUS.Value, "dd/MM/yyyy", WebUtil.EN);
            DateTime edt = DateTime.ParseExact(HdEDateUS.Value, "dd/MM/yyyy", WebUtil.EN);
            String acc_id_bg = dw_main.GetItemString(1, "start_accid").Trim();
            String acc_id_ed = dw_main.GetItemString(1, "end_accid").Trim();
            try
            {
                str_tmp = Accsrv.of_gen_ledger_report(state.SsWsPass, sdt, edt, acc_id_bg, acc_id_ed, state.SsCoopId);
                if (str_tmp != "")
                {
                    dw_rpt.ImportString(str_tmp, FileSaveAsType.Xml);//ทำการ import string xml
                    str_tmp = "t_coopname.text = '";
                    str_tmp += state.SsCoopName;
                    str_tmp += "'";
                    dw_rpt.Modify(str_tmp);
                    
                    /*str_tmp = "t_name.text = '";
                    str_tmp += "รายงานแยกประเภทบัญชี ";
                    str_tmp += "'";
                    dw_rpt.Modify(str_tmp);
                    */
                    str_tmp = "t_date.text = '";
                    str_tmp += " ณ วันที่ ";
                    str_tmp += accF.CnvStrDateToStrFDate(HdSDateTH.Value, "th");
                    str_tmp += "ถึงวันที่ ";
                    str_tmp += accF.CnvStrDateToStrFDate(HdEDateTH.Value, "th");
                    str_tmp += "'";
                    dw_rpt.Modify(str_tmp);
                }
            }
            catch (SoapException ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("เกิดข้อผิดพลาด " + WebUtil.SoapMessage(ex));
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
            }
        }
        protected void SetChildDW()
        {
            // accstart
            DwUtil.RetrieveDDDW(dw_main, "start_accid", "acc_report_design.pbl", null);

            // accend
            DwUtil.RetrieveDDDW(dw_main, "end_accid", "acc_report_design.pbl", null);
        }
    }
}
