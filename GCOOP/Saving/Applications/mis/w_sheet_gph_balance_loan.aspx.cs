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
using System.Web.Services.Protocols;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using CoreSavingLibrary.WcfNCommon;
using DataLibrary;
using CoreSavingLibrary.WcfNMis;
using System.Globalization;

namespace Saving.Applications.mis
{
    public partial class w_sheet_gph_loan_month : PageWebSheet, WebSheet
    {
        private n_misClient MisService;
        protected String postMonthRetrieve;
        protected String postQuarterRetrieve;
        protected String postHalfRetrieve;
        protected String postYearRetrieve;


        private void YearRetrieve()
        {
            Int16 li_startyear, li_endyear; string grpXml="", tableXml="";

            li_startyear = Convert.ToInt16(dw_cri_quar.GetItemDecimal(1, "start_year"));
            li_endyear = Convert.ToInt16(dw_cri_quar.GetItemDecimal(1, "end_year"));

            Int32 gph = MisService.of_gen_gph_balance_loan_year(state.SsWsPass, li_startyear, li_endyear,grpXml,tableXml);

            //.ดึงข้อมูลใส่ตาราง. 
            dw_graph_year.Reset();
            dw_graph_year.ImportString(grpXml, Sybase.DataWindow.FileSaveAsType.Xml);

            //.ดึงข้อมูลใส่ตาราง. 
            dw_data_year.Reset();
            dw_data_year.ImportString(tableXml, Sybase.DataWindow.FileSaveAsType.Xml);

            LtChangeTap.Text = "<script>showTabPage('" + HdCurrentTap.Value + "')</script>";
        }

        private void HalfRetrieve()
        {
            Int16 li_startyear, li_endyear; string grpXml="", tableXml="";
            Int16 li_starthalf, li_endhalf;

            li_startyear = Convert.ToInt16(dw_cri_quar.GetItemDecimal(1, "start_year"));
            li_starthalf = Convert.ToInt16(dw_cri_quar.GetItemDecimal(1, "start_quar"));
            li_endyear = Convert.ToInt16(dw_cri_quar.GetItemDecimal(1, "end_year"));
            li_endhalf = Convert.ToInt16(dw_cri_quar.GetItemDecimal(1, "end_quar"));

            Int32 gph = MisService.of_gen_gph_balance_loan_half(state.SsWsPass, li_starthalf, li_startyear, li_endhalf, li_endyear,grpXml,tableXml);

            //.ดึงข้อมูลใส่ตาราง. 
            dw_graph_half.Reset();
            dw_graph_half.ImportString(grpXml, Sybase.DataWindow.FileSaveAsType.Xml);

            //.ดึงข้อมูลใส่ตาราง. 
            dw_data_half.Reset();
            dw_data_half.ImportString(tableXml, Sybase.DataWindow.FileSaveAsType.Xml);

            LtChangeTap.Text = "<script>showTabPage('" + HdCurrentTap.Value + "')</script>";
        }

        private void QuarterRetrieve()
        {
            Int16 li_startyear, li_endyear; string grpXml="", tableXml="";
            Int16 li_startquar, li_endquar;

            li_startyear = Convert.ToInt16(dw_cri_quar.GetItemDecimal(1, "start_year"));
            li_startquar = Convert.ToInt16(dw_cri_quar.GetItemDecimal(1, "start_quar"));
            li_endyear = Convert.ToInt16(dw_cri_quar.GetItemDecimal(1, "end_year"));
            li_endquar = Convert.ToInt16(dw_cri_quar.GetItemDecimal(1, "end_quar"));

            Int32 gph = MisService.of_gen_gph_balance_loan_quarter(state.SsWsPass, li_startquar, li_startyear, li_endquar, li_endyear,grpXml,tableXml);

            //.ดึงข้อมูลใส่กราฟ.   
            dw_graph_quar.Reset();
            dw_graph_quar.ImportString(grpXml, Sybase.DataWindow.FileSaveAsType.Xml);

            //.ดึงข้อมูลใส่ตาราง. 
            dw_data_quar.Reset();
            dw_data_quar.ImportString(tableXml, Sybase.DataWindow.FileSaveAsType.Xml);

            LtChangeTap.Text = "<script>showTabPage('" + HdCurrentTap.Value + "')</script>";
        }

        private void MonthRetrieve()
        {
            Int16 li_startyear, li_endyear; string grpXml="", tableXml="";
            Int16 li_startmonth, li_endmonth;

            li_startyear = Convert.ToInt16(dw_cri_month.GetItemDecimal(1, "start_year"));
            li_startmonth = Convert.ToInt16(dw_cri_month.GetItemDecimal(1, "start_month"));
            li_endyear = Convert.ToInt16(dw_cri_month.GetItemDecimal(1, "end_year"));
            li_endmonth = Convert.ToInt16(dw_cri_month.GetItemDecimal(1, "end_month"));

            Int32 gph = MisService.of_gen_gph_balance_loan_month(state.SsWsPass, li_startmonth, li_startyear, li_endmonth, li_endyear,grpXml,tableXml);

            //.ดึงข้อมูลใส่กราฟ.           
            dw_graph_month.Reset();
            dw_graph_month.ImportString(grpXml, Sybase.DataWindow.FileSaveAsType.Xml);

            //.ดึงข้อมูลใส่ตาราง.           
            dw_data_month.Reset();
            dw_data_month.ImportString(tableXml, Sybase.DataWindow.FileSaveAsType.Xml);
        }



        #region WebSheet Members

        public void InitJsPostBack()
        {
            postMonthRetrieve = WebUtil.JsPostBack(this, "postMonthRetrieve");
            postQuarterRetrieve = WebUtil.JsPostBack(this, "postQuarterRetrieve");
            postHalfRetrieve = WebUtil.JsPostBack(this, "postHalfRetrieve");
            postYearRetrieve = WebUtil.JsPostBack(this, "postYearRetrieve");
        }

        public void WebSheetLoadBegin()
        {
            MisService = wcf.NMis;

            if (!IsPostBack)
            {
                //ตั้งค่า criteria
                dw_cri_month.InsertRow(0);
                dw_cri_month.SetItemDecimal(1, "start_year", 2552);
                dw_cri_month.SetItemDecimal(1, "end_year", 2553);
                dw_cri_month.SetItemDecimal(1, "start_month", 1);
                dw_cri_month.SetItemDecimal(1, "end_month", 12);

                dw_cri_quar.InsertRow(0);
                dw_cri_quar.SetItemDecimal(1, "start_year", 2552);
                dw_cri_quar.SetItemDecimal(1, "end_year", 2553);
                dw_cri_quar.SetItemDecimal(1, "start_quar", 1);
                dw_cri_quar.SetItemDecimal(1, "end_quar", 4);

                dw_cri_half.InsertRow(0);
                dw_cri_half.SetItemDecimal(1, "start_half", 1);
                dw_cri_half.SetItemDecimal(1, "start_year", 2552);
                dw_cri_half.SetItemDecimal(1, "end_half", 2);
                dw_cri_half.SetItemDecimal(1, "end_year", 2553);

                dw_cri_year.InsertRow(0);
                dw_cri_year.SetItemDecimal(1, "start_year", 2552);
                dw_cri_year.SetItemDecimal(1, "end_year", 2553);
            }
            else
            {
                this.RestoreContextDw(dw_cri_month);
                this.RestoreContextDw(dw_cri_quar);
                this.RestoreContextDw(dw_cri_half);
                this.RestoreContextDw(dw_cri_year);

                this.RestoreContextDw(dw_data_month);
                this.RestoreContextDw(dw_data_quar);
                this.RestoreContextDw(dw_data_half);
                this.RestoreContextDw(dw_data_year);

                this.RestoreContextDw(dw_graph_month);
                this.RestoreContextDw(dw_graph_quar);
                this.RestoreContextDw(dw_graph_half);
                this.RestoreContextDw(dw_graph_year);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postMonthRetrieve")
            {
                MonthRetrieve();
            }
            else if (eventArg == "postQuarterRetrieve")
            {
                QuarterRetrieve();
            }
            else if (eventArg == "postHalfRetrieve")
            {
                HalfRetrieve();
            }
            else if (eventArg == "postYearRetrieve")
            {
                YearRetrieve();
            }
        }

        public void SaveWebSheet()
        {

        }

        public void WebSheetLoadEnd()
        {
            dw_cri_month.SaveDataCache();
            dw_cri_quar.SaveDataCache();
            dw_cri_half.SaveDataCache();
            dw_cri_year.SaveDataCache();

            dw_data_month.SaveDataCache();
            dw_data_quar.SaveDataCache();
            dw_data_half.SaveDataCache();
            dw_data_year.SaveDataCache();

            dw_graph_month.SaveDataCache();
            dw_graph_quar.SaveDataCache();
            dw_graph_half.SaveDataCache();
            dw_graph_year.SaveDataCache();
        }

        #endregion

    }
}
