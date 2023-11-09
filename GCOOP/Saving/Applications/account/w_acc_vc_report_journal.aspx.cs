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
//using CoreSavingLibrary.WcfPrint;

namespace Saving.Applications.account
{
    public partial class w_acc_vc_report_journal : PageWebSheet, WebSheet
    {
        protected String postOpenReport;
        protected String postOpenPDF;
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
            postOpenPDF = WebUtil.JsPostBack(this, "postOpenPDF");

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
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postOpenReport")
            {
                this.JspostOpenReport();
            }
            else if (eventArg == "postOpenPDF")
            {
                this.PostOpenPDF();
            }
        }

        private void PostOpenPDF()
        {
            String xmlpdf = dw_rpt.Describe("Datawindow.data.XML");

            //ชื่อไฟล์ PDF = YYYYMMDDHHMMSS_<GID>_<RID>.PDF
            String pdfFileName = DateTime.Now.ToString("yyyyMMddHHmmss", WebUtil.EN);
            pdfFileName += "_Triblane.pdf";
            pdfFileName = pdfFileName.Trim();


            //PrintClient wsPrint = wcf.Print;
            //int inV = wsPrint.PrintPDF(state.SsWsPass, xmlpdf, pdfFileName);
            //if (inV < 0)
            //{
            //    LtServerMessage.Text = WebUtil.ErrorMessage("เกิดข้อผิดพลาด " + xmlpdf);
            //    return;
            //}

            //String pdfURL = wsPrint.GetPDFURL(state.SsWsPass) + pdfFileName;

            //String pop = "Gcoop.OpenPopup('" + pdfURL + "')";
            //ClientScript.RegisterClientScriptBlock(this.GetType(), "Triblane", pop, true);
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

            DateTime sdt = DateTime.ParseExact(HdSDateUS.Value, "dd/MM/yyyy", WebUtil.EN);
            DateTime edt = DateTime.ParseExact(HdEDateUS.Value, "dd/MM/yyyy", WebUtil.EN);
            Int16 int_type = Convert.ToInt16(dw_main.GetItemDecimal(1, "type_group"));

            if (int_type == Convert.ToInt16(4))
            {
                dw_rpt.DataWindowObject = "r_day10_journal_all";
            }
            else
            {
                dw_rpt.DataWindowObject = "r_day10_journal";
            }
            dw_rpt.Reset();

            try
            {
                str_tmp = Accsrv.of_gen_day_journalreport(state.SsWsPass, sdt, edt, int_type, state.SsCoopId);
                if (str_tmp != "")
                {

                    dw_rpt.ImportString(str_tmp, FileSaveAsType.Xml);//ทำการ import string xml
                    str_tmp = "t_head.text = '";
                    str_tmp += state.SsCoopName;
                    str_tmp += "'";
                    dw_rpt.Modify(str_tmp);

                    str_tmp = "t_name.text = '";
                    str_tmp += "รายงานสรุปประจำวัน ";
                    str_tmp += "'";
                    dw_rpt.Modify(str_tmp);

                    str_tmp = "t_date.text = '";
                    str_tmp += "ประจำวันที่ ";
                    str_tmp += accF.CnvStrDateToStrFDate(HdSDateTH.Value, "th");
                    if(!HdSDateUS.Value.Equals(HdEDateUS.Value))
                    {
                        str_tmp += "ถึงวันที่ ";
                        str_tmp += accF.CnvStrDateToStrFDate(HdEDateTH.Value, "th");
                    }
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
