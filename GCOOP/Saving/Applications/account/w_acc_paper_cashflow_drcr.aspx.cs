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
    public partial class w_acc_paper_cashflow_drcr : PageWebSheet, WebSheet
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
            //changeValue = WebUtil.JsPostBack(this, "changeValue");
            postOpenReport = WebUtil.JsPostBack(this, "postOpenReport");

            //tDwMain = new DwThDate(dw_main, this);
            //tDwMain.Add("start_date", "start_tdate");

            //tDwMain = new DwThDate(dw_main, this);
            //tDwMain.Add("end_date", "end_tdate");

            //tDwMain.Eng2ThaiAllRow();

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
            
            String str_period = dw_main.GetItemString(1, "period");
            String str_year = dw_main.GetItemString(1, "year_period");
            Int16 int_type = Convert.ToInt16(dw_main.GetItemDecimal(1, "type_rpt"));
            String bra_id = dw_main.GetItemString(1, "branch_id").Trim();

            try
            {
                str_tmp = accF.GetFindToLDayOfM(str_period, str_year, "TH").Trim();
                HdSDateTH.Value = "01/" + str_period + "/" + str_year;
                HdSDateUS.Value = "01/" + str_period + "/" + Convert.ToString(Convert.ToInt16(str_year) - 543);
                HdEDateTH.Value = str_tmp + "/" + str_period + "/" + str_year;
                HdEDateUS.Value = str_tmp + "/" + str_period + "/" + Convert.ToString(Convert.ToInt16(str_year) - 543);
                str_tmp = Accsrv.of_gen_cashpaper_drcr(state.SsWsPass, DateTime.ParseExact(HdSDateUS.Value, "dd/MM/yyyy", WebUtil.EN), DateTime.ParseExact(HdEDateUS.Value, "dd/MM/yyyy", WebUtil.EN), int_type, bra_id);
                if (str_tmp != "")
                {
                    dw_rpt.ImportString(str_tmp, FileSaveAsType.Xml);//ทำการ import string xml
                    str_tmp = "t_head.text = '";
                    str_tmp += state.SsCoopName;
                    str_tmp += "'";
                    dw_rpt.Modify(str_tmp);

                    str_tmp = "t_name.text = '";
                    str_tmp += "กระดาษทำการงบกระแสเงินสด (เดบิต,เครดิต)";
                    str_tmp += "'";
                    dw_rpt.Modify(str_tmp);

                    str_tmp = "t_date.text = '";
                    str_tmp += "ประจำวันที่ ";
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
    }
}
