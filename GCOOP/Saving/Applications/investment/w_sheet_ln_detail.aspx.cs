using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary.WcfNCommon;
using DataLibrary;
using Sybase.DataWindow;

namespace Saving.Applications.investment
{
    public partial class w_sheet_ln_detail : PageWebSheet, WebSheet
    {
        #region Value
        protected String jsPostMemberNo;
        protected String jsPostListClick;
        string pbl = "loan_statement.pbl";
        #endregion

        #region Websheet member
        public void InitJsPostBack()
        {
            jsPostMemberNo = WebUtil.JsPostBack(this, "jsPostMemberNo");
            jsPostListClick = WebUtil.JsPostBack(this, "jsPostListClick");
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
                this.RestoreContextDw(Dwlist);
                this.RestoreContextDw(DwDetail);
                this.RestoreContextDw(Dwstatement);
                this.RestoreContextDw(Dwcoll);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            switch (eventArg)
            {
                case "jsPostMemberNo":
                    InitList();
                    break;
                case "jsPostListClick":
                    ListClick();
                    break;
            }
        }

        public void SaveWebSheet()
        {

        }

        public void WebSheetLoadEnd()
        {
            DwMain.SaveDataCache();
            Dwlist.SaveDataCache();
            DwDetail.SaveDataCache();
            Dwstatement.SaveDataCache();
            Dwcoll.SaveDataCache();
        }
        #endregion

        #region Function
        private void InitList()
        {
            try
            {
                String MemberNo = DwMain.GetItemString(1, "member_no");
                int MemberNo_Length = MemberNo.Length;
                switch (MemberNo_Length)
                {
                    case 1:
                        MemberNo = "00000" + MemberNo;
                        break;
                    case 2:
                        MemberNo = "0000" + MemberNo;
                        break;
                    case 3:
                        MemberNo = "000" + MemberNo;
                        break;
                    case 4:
                        MemberNo = "00" + MemberNo;
                        break;
                    case 5:
                        MemberNo = "0" + MemberNo;
                        break;
                }
                DwMain.SetItemString(1, "member_no", MemberNo);

                String sql = "select member_status from lcmembmaster where member_no = '" + MemberNo + "'";
                Sta ta = new Sta(new DwTrans().ConnectionString);
                Sdt dt = ta.Query(sql);

                if (dt.Next())
                {
                    if (dt.Rows[0][0].ToString() != "-1" && dt.Rows[0][0].ToString() != "-9")
                    {
                        DwUtil.RetrieveDataWindow(DwMain, pbl, null, state.SsCoopId, MemberNo);
                        DwUtil.RetrieveDataWindow(Dwlist, pbl, null, state.SsCoopId, MemberNo);
                    }
                    else
                    {
                        DwMain.Reset();
                        DwMain.InsertRow(0);
                        LtServerMessage.Text = WebUtil.ErrorMessage("สมาชิกเลขที่ " + MemberNo + " ได้ลาออกไปแล้ว");
                    }
                }
                else
                {
                    DwMain.Reset();
                    DwMain.InsertRow(0);
                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่มีเลขสมาชิก " + MemberNo);
                }
            }
            catch
            {

            }
        }

        private void ListClick()
        {
            int row = Convert.ToInt16(Hd_row.Value);
            string loancontract_no = Dwlist.GetItemString(row, "loancontract_no");

            DwUtil.RetrieveDataWindow(DwDetail, pbl, null, state.SsCoopControl, loancontract_no);
            DwUtil.RetrieveDataWindow(Dwstatement, pbl, null, state.SsCoopControl, loancontract_no);
            DwUtil.RetrieveDataWindow(Dwcoll, pbl, null, loancontract_no);

            if (DwDetail.RowCount == 0)
            {

                DwDetail.InsertRow(0);
            }
        }
        #endregion
    }
}