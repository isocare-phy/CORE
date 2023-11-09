using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary.WcfNInvestment;

namespace Saving.Applications.investment
{
    public partial class w_sheet_ln_process_short_long : PageWebSheet, WebSheet
    {
        [JsPostBack]
        public string PostMonth { get; set; }

        private DwThDate tDwMain;

        public void InitJsPostBack()
        {
            tDwMain = new DwThDate(DwMain, this);
            tDwMain.Add("accstart_date", "accstart_tdate");
            tDwMain.Add("accend_date", "accend_tdate");
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                int y1 = state.SsWorkDate.Year;
                int m1 = state.SsWorkDate.Month;
                DwMain.InsertRow(0);
                DwMain.SetItemDecimal(1, "est_year", y1 + 543);
                DwMain.SetItemDecimal(1, "est_month", m1);
                if (m1 == 12)
                {
                    m1 = 1;
                    y1++;
                }
                else
                {
                    m1++;
                }
                DateTime dt1 = new DateTime(y1, m1, 1);
                DateTime dt2 = dt1.AddYears(1).AddDays(-1);
                DwMain.SetItemDate(1, "accstart_date", dt1);
                DwMain.SetItemDate(1, "accend_date", dt2);
            }
            else
            {
                DwMain.RestoreContext();
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == PostMonth)
            {
                int year = Convert.ToInt32(DwMain.GetItemDecimal(1, "est_year"));
                int month = Convert.ToInt32(DwMain.GetItemDecimal(1, "est_month"));
                if (month == 12)
                {
                    month = 1;
                    year++;
                }
                else
                {
                    month++;
                }
                DateTime dt1 = new DateTime(year - 543, month, 1);

                DateTime dt2 = dt1.AddYears(1).AddDays(-1);

                DwMain.SetItemDate(1, "accstart_date", dt1);
                DwMain.SetItemDate(1, "accend_date", dt2);
            }
        }

        public void SaveWebSheet()
        {
            try
            {
                if (DwMain.RowCount > 1)
                {
                    DwMain.DeleteRow(2);
                }

                string xml = DwMain.Describe("Datawindow.Data.XML");
                str_lcprocloan lcp = new str_lcprocloan();
                lcp.xml_option = xml;
                wcf.NInvestment.of_saveproc_shortlong(state.SsWsPass, lcp);
                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลสำเร็จ");
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        public void WebSheetLoadEnd()
        {
            try
            {
                tDwMain.Eng2ThaiAllRow();
            }
            catch { }
            DwMain.SaveDataCache();
        }
    }
}