using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBAccess;
using Sybase.DataWindow;
using CommonLibrary;

namespace Saving.Applications.app_assist
{
    public partial class w_sheet_pension_sharewithdraw : PageWebSheet, WebSheet
    {
        protected DwThDate tDwCri;
        protected String jsPostSerch;

        public void InitJsPostBack()
        {
            jsPostSerch = WebUtil.JsPostBack(this, "jsPostSerch");

            tDwCri = new DwThDate(DwCri, this);
            tDwCri.Add("start_date", "start_tdate");
            tDwCri.Add("end_date", "end_tdate");
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                DwCri.InsertRow(0);
                DwCri.SetItemDate(1, "start_date", state.SsWorkDate);
                DwCri.SetItemDate(1, "end_date", state.SsWorkDate);
            }
            else
            {
                this.RestoreContextDw(DwCri);
                this.RestoreContextDw(DwMain);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "jsPostSerch")
            {
                RetrieveData();
            }
        }

        public void SaveWebSheet()
        {
            decimal assist_amt = 0, approve_amt = 0, sumpays = 0, perpay = 0, balance = 0, check_flag = 0 , seq_no = 0;
            String member_no = "", assist_docno="";
            DateTime calculate_date = state.SsWorkDate;
            try
            {
                for (int i = 1; i <= DwMain.RowCount; i++)
                {
                    check_flag = DwMain.GetItemDecimal(i, "check_flag");
                    if (check_flag == 1)
                    {
                        assist_docno = DwMain.GetItemString(i, "assist_docno");
                        seq_no = DwMain.GetItemDecimal(i, "seq_no");
                        member_no = DwMain.GetItemString(i, "member_no");
                        assist_amt = DwMain.GetItemDecimal(i, "assist_amt");
                        approve_amt = DwMain.GetItemDecimal(i, "approve_amt");
                        sumpays = DwMain.GetItemDecimal(i, "sumpay");
                        perpay = DwMain.GetItemDecimal(i, "perpay");
                        balance = DwMain.GetItemDecimal(i, "balance");
                        calculate_date = DwMain.GetItemDate(i, "calculate_date");

                        string SqlUpdate80 = "update asnreqmaster set paytype_code = 'SW' , approve_amt = " + approve_amt + " , assist_amt = " + assist_amt + " , sumpays = " + sumpays + " , balance = " + balance + " , perpay = " + perpay + @"
                                      , calculate_date = to_date('" + calculate_date.ToString("dd/MM/yyyy") + "','dd/mm/yyyy')  where member_no = '" + member_no + "' and assisttype_code = '80'";
                        WebUtil.Query(SqlUpdate80);

                        string updateStatus = "update asnsharewithdraw set post_status = 1 where assist_docno = '" + assist_docno + "' and assisttype_code = '80' and seq_no =" + seq_no + " and member_no = '"+member_no+"'";
                        WebUtil.Query(updateStatus);
                    }
                }
                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
            }
            catch { }
        }

        public void WebSheetLoadEnd()
        {
            tDwCri.Eng2ThaiAllRow();
            DwCri.SaveDataCache();
            DwMain.SaveDataCache();
        }

        public void RetrieveData()
        {
            DateTime start_date = state.SsWorkDate, end_date = state.SsWorkDate;
            start_date = DwCri.GetItemDate(1, "start_date");
            end_date = DwCri.GetItemDate(1, "end_date");

            DwUtil.RetrieveDataWindow(DwMain, "kt_pension.pbl", null, start_date, end_date);
        }
    }
}