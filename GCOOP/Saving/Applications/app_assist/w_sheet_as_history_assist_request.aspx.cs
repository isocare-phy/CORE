using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Saving.Applications.app_assist
{
    public partial class w_sheet_as_history_assist_request : PageWebSheet, WebSheet
    {
        protected String postGetMemberDetail;
        public void InitJsPostBack()
        {
            postGetMemberDetail = WebUtil.JsPostBack(this, "postGetMemberDetail");
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                DwMain.InsertRow(0);
                DwDetail.InsertRow(0);
            }
            else
            {
                this.RestoreContextDw(DwMain);
                this.RestoreContextDw(DwDetail);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postGetMemberDetail")
            {
                GetMemberDetail();
            }
        }

        public void SaveWebSheet()
        {

        }

        public void WebSheetLoadEnd()
        {
            DwMain.SaveDataCache();
            DwDetail.SaveDataCache();
        }
        private void GetMemberDetail()
        {
            String member_no;
            DateTime member_date, birth_date;
            Decimal member_age;

            member_no = HdMemNo.Value;
            object[] args = new object[1];
            args[0] = member_no;
            int[] member_year = new int[3];

            DwMain.Reset();
            DwDetail.Reset();

            DwUtil.RetrieveDataWindow(DwMain, "as_capital.pbl", null, args);
            DwUtil.RetrieveDataWindow(DwDetail, "as_capital.pbl", null, args);

            try { member_date = DwMain.GetItemDateTime(1, "member_date"); }
            catch { member_date = state.SsWorkDate; }
            try { birth_date = DwMain.GetItemDateTime(1, "birth_date"); }
            catch { birth_date = state.SsWorkDate; }

            member_year = clsCalAge(member_date, state.SsWorkDate);
            string s_member_year = member_year[2].ToString() + " ปี " + member_year[1].ToString() + " เดือน " + member_year[0].ToString() + " วัน";
            DwMain.SetItemString(1, "member_years_disp", s_member_year);

            //member_age = wcf.Busscom.of_cal_yearmonth(state.SsWsPass, birth_date, state.SsWorkDate);
            //string s_member_age = member_age.ToString() + " ปี";
            //DwMain.SetItemString(1, "member_age", s_member_age);
            member_year = clsCalAge(birth_date, state.SsWorkDate);
            string member_age_range = member_year[2].ToString() + "." + member_year[1].ToString("00") + " ปี";
            DwMain.SetItemString(1, "member_age", member_age_range);

            if (DwMain.RowCount < 1)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบเลขสมาชิก " + member_no);
                DwMain.Reset();
                DwMain.InsertRow(0);
            }
            else if (DwDetail.RowCount < 1)
            {
                DwDetail.Reset();
                DwDetail.InsertRow(0);
            }
        }
        public int[] clsCalAge(DateTime d1, DateTime d2)
        {
            DateTime fromDate;
            DateTime toDate;

            int intyear;
            int intmonth;
            int intday;
            int intCheckedDay = 0;
            int[] CalAge = new int[3];

            if (d1 > d2)
            {
                fromDate = d2;
                toDate = d1;
            }
            else
            {
                fromDate = d1;
                toDate = d2;
            }
            intCheckedDay = 0;
            if (fromDate.Day > toDate.Day)
            {
                intCheckedDay = DateTime.DaysInMonth(fromDate.Year, fromDate.Month);
            }
            if (intCheckedDay != 0)
            {
                intday = (toDate.Day + intCheckedDay) - fromDate.Day;
                intCheckedDay = 1;
            }
            else
            {
                intday = toDate.Day - fromDate.Day;
            }
            if ((fromDate.Month + intCheckedDay) > toDate.Month)
            {
                intmonth = (toDate.Month + 12) - (fromDate.Month + intCheckedDay);
                intCheckedDay = 1;
            }
            else
            {
                intmonth = (toDate.Month) - (fromDate.Month + intCheckedDay);
                intCheckedDay = 0;
            }
            intyear = toDate.Year - (fromDate.Year + intCheckedDay);

            CalAge[0] = intday;
            CalAge[1] = intmonth;
            CalAge[2] = intyear;
            return CalAge;

        }
    }
}