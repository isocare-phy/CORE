using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary.WcfNInvestment;
using CoreSavingLibrary.WcfNCommon;
using Sybase.DataWindow;
using System.Web.Services.Protocols;
using DataLibrary;
using System.Data;

namespace Saving.Applications.investment
{
    public partial class w_sheet_reprint_slippayin : PageWebSheet, WebSheet
    {
        private DwThDate tdwhead;
        private n_commonClient commonService;
        private n_investmentClient inv;
        protected String jsFind;
        protected String jspostNewClear;
        protected String jsPrint;

        public String sql = "";
        string reqdoc_no = "";
        String member_no = "";
        string fromset = "";
        int x = 1;
        public void InitJsPostBack()
        {
            jspostNewClear = WebUtil.JsPostBack(this, "jspostNewClear");
            jsFind = WebUtil.JsPostBack(this, "jsFind");
            jsPrint = WebUtil.JsPostBack(this, "jsPrint");
            tdwhead = new DwThDate(dw_main, this);
            tdwhead.Add("start_date", "start_tdate");
            tdwhead.Add("end_date", "end_tdate");

        }

        public void WebSheetLoadBegin()
        {
            DwUtil.RetrieveDDDW(dw_main, "member_no_1", "loan_slippayin.pbl", null);
            try
            {
                this.ConnectSQLCA();
            }
            catch { LtServerMessage.Text = WebUtil.CompleteMessage("ติดต่อ Database ไม่ได้"); }

            try
            {
                inv = wcf.NInvestment;
            }
            catch { LtServerMessage.Text = WebUtil.CompleteMessage("ติดต่อ Service ไม่ได้"); }

            dw_main.SetTransaction(sqlca);
            dw_detail.SetTransaction(sqlca);

            if (!IsPostBack)
            {
                
                JspostNewClear();
            }
            else
            {
                this.RestoreContextDw(dw_main);
                this.RestoreContextDw(dw_detail);
            }

        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "jspostNewClear")
            {
                JspostNewClear();
            }

            else if (eventArg == "jsPrint")
            {
                JsPrint();
            }
         else if (eventArg == "jsFind")
         {
             JsFind();
         }
        }

        public void SaveWebSheet()
        {
            try
            {
                String dwdetail_XML = dw_detail.Describe("DataWindow.Data.XML");
            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
        }

        public void WebSheetLoadEnd()
        {
            dw_main.SaveDataCache();
            dw_detail.SaveDataCache();
        }



        private void JspostNewClear()
        {
            dw_main.Reset();
            dw_detail.Reset();
            dw_main.InsertRow(1);
            this.dw_main.SetItemDateTime(1, "start_date", state.SsWorkDate);
            this.dw_main.SetItemDateTime(1, "end_date", state.SsWorkDate);
            tdwhead.Eng2ThaiAllRow();
        }

        private void JsFind()
        {

            String ls_memberno, ls_sqlext = "", ls_sql = "", ls_temp= "";
            ls_sql = dw_detail.GetSqlSelect();
            string date_begin = "";
            string date_end = "";
            string str_date = "";
            string str_month = "";
            string str_year = "";
            string date_begin1 = "";
            string date_end1 = "";

            //DateTime startDate = dw_main.GetItemDateTime(1, "start_date");
            //DateTime endDate = dw_main.GetItemDateTime(1, "end_date");
            try
            {
                ls_memberno = WebUtil.StringFormat(dw_main.GetItemString(1, "member_no").Trim(), "000000");
            }
            catch { ls_memberno = ""; }
         

            //String ls_memberno, ls_sqlext = "";
            //ls_sqlext = dw_detail.GetSqlSelect();
            //string date_begin = "";
            //string date_end = "";
            try 
            {
                date_begin1 = dw_main.GetItemDateTime(1, "start_date").ToString("ddMMyyyy");
                str_date = date_begin1.Substring(0, 2);
                str_month = date_begin1.Substring(2, 2);
                str_year = Convert.ToString(Convert.ToInt32(date_begin1.Substring(4, 4)));
                date_begin = str_date + "/" + str_month + "/" + str_year;

                date_end1 = dw_main.GetItemDateTime(1, "end_date").ToString("ddMMyyyy");
                str_date = date_end1.Substring(0, 2);
                str_month = date_end1.Substring(2, 2);
                str_year = Convert.ToString(Convert.ToInt32(date_end1.Substring(4, 4)));
                date_end = str_date + "/" + str_month + "/" + str_year;
            }
            catch { }
            

           // ls_sql = dw_detail.GetSqlSelect();
            ls_sqlext += "and (  lcslippayin.coop_id  = '" + state.SsCoopControl + "')";

            if (ls_memberno.Length > 0)
            {
                ls_sqlext += " and (  lcslippayin.member_no = '" + ls_memberno + "')";
            }

            if (str_date.Length > 0)
            {
                ls_sqlext += "and lcslippayin.slip_date between to_date('" + date_begin + "','dd/mm/yyyy') and to_date('" + date_end + "','dd/mm/yyyy')";
            }

            ls_temp = ls_sql + ls_sqlext;
            if (ls_sql != ls_temp)
            {
               // hidden_search.Value = ls_temp;
                DwUtil.ImportData(ls_temp, dw_detail, null);
              //  JspostClearHidden();
            }
            else
            {
                dw_detail.Reset();
            }


        }
        private void JsPrint()
        {
            try
            {
                int row = Convert.ToInt32(HdRow.Value);
                HdRow.Value = "";
                string slip_no = dw_detail.GetItemString(row, "payinslip_no");
                Printing.LnCoopSlip(this, state.SsCoopId, slip_no, 1);
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }

        }

        private void JsCheckAll()
        {
            Boolean Select = false;// CheckAll.Checked;
            Decimal Set = 1;

            if (Select == true)
            {
                Set = 1;
            }
            else if (Select == false)
            {
                Set = 0;
            }
            for (int i = 1; i <= dw_detail.RowCount; i++)
            {
                dw_detail.SetItemDecimal(i, "operate_flag", Set);
            }
        }

        public string slipno { get; set; }
    }
}