using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sybase.DataWindow;
using DataLibrary;

namespace Saving.Applications.app_assist
{
    public partial class w_sheet_as_scholarship : PageWebSheet, WebSheet
    {
        protected DwThDate tDwMain,
                           tDwDetail
            ;
        protected String pbl = "as_public_funds.pbl",
                         moneytype_code,
                         assist_docno,
                         sqlStr,
                         member_no,
                         surname_child
            ;
        protected DataStore Ds
            ;
        protected Decimal capital_year
            ;
        protected int period
            ;
        protected Sta ta;
        protected Sdt dt;

        protected String postGetMemberDetail;

        #region WebSheetMaster
        public void InitJsPostBack()
        {
            tDwMain = new DwThDate(DwMain, this);
            tDwMain.Add("req_date", "req_tdate");
            tDwMain.Add("approve_date", "approve_tdate");

            tDwDetail = new DwThDate(DwDetail, this);
            tDwDetail.Add("childbirth_date", "childbirth_tdate");

            postGetMemberDetail = WebUtil.JsPostBack(this, "postGetMemberDetail");
        }

        public void WebSheetLoadBegin()
        {
            this.ConnectSQLCA();
            ta = new Sta(sqlca.ConnectionString);

            if (!IsPostBack)
            {
                JsNewClear();
                RetrieveDDDW();
            }
            else
            {
                this.RestoreContextDw(DwMem);
                this.RestoreContextDw(DwMain);
                this.RestoreContextDw(DwDetail);
            }

            tDwMain.Eng2ThaiAllRow();
            tDwDetail.Eng2ThaiAllRow();
        }

        public void CheckJsPostBack(string eventArg)
        {
            switch (eventArg)
            {
                case "postGetMemberDetail":
                    GetMemberDetail();
                    break;
            }
        }

        public void SaveWebSheet()
        {
        }

        public void WebSheetLoadEnd()
        {
            try
            {
                DwMem.SaveDataCache();
                DwMain.SaveDataCache();
                DwDetail.SaveDataCache();
            }
            catch (Exception ex) { ex.ToString(); }
        }
        #endregion

        #region Function(s)
        /// <summary>
        /// Clear หน้าจอใหม่
        /// </summary>
        protected void JsNewClear()
        {
            DwMem.InsertRow(0);
            DwMain.InsertRow(0);
            DwDetail.InsertRow(0);
            DwMain.SetItemDateTime(1, "req_date", state.SsWorkDate);
            DwMain.SetItemDateTime(1, "approve_date", state.SsWorkDate);
            DwDetail.SetItemDateTime(1, "childbirth_date", state.SsWorkDate);
            SetDefaultAccid();
        }

        /// <summary>
        /// Set Default เลขคู่บัญชี
        /// </summary>
        protected void SetDefaultAccid()
        {
            DwMain.SetItemString(1, "moneytype_code", "CBT");
            PostToFromAccid();
            DwMain.SetItemString(1, "tofrom_accid", "11035200");
        }

        /// <summary>
        /// Set Default วิธีการจ่าย
        /// </summary>
        protected void PostToFromAccid()
        {
            try { moneytype_code = DwMain.GetItemString(1, "moneytype_code"); }
            catch { moneytype_code = ""; }
            DwUtil.RetrieveDDDW(DwMain, "tofrom_accid", "as_public_funds.pbl", moneytype_code);
        }

        /// <summary>
        /// 
        /// </summary>
        protected void SetDefaultDistrict()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        protected void SetDefaultProvince()
        {

        }

        /// <summary>
        /// RetrieveDDDW.(Default)
        /// Set Default ปีทุนสวัสดิการ
        /// </summary>
        protected void RetrieveDDDW()
        {
            DwUtil.RetrieveDDDW(DwMain, "moneytype_code", "as_public_funds.pbl", null);
            DwUtil.RetrieveDDDW(DwMain, "assisttype_code", "as_public_funds.pbl", state.SsCoopId);
            DwUtil.RetrieveDDDW(DwMain, "capital_year", "as_capital.pbl", null);
            try { capital_year = DwMain.GetItemDecimal(1, "capital_year"); }
            catch { capital_year = state.SsWorkDate.Year + 543; }
            DwMain.SetItemDecimal(1, "capital_year", capital_year);
            GetLastDocNo(capital_year);
            DwMain.SetItemString(1, "assist_docno", assist_docno);

            DwUtil.RetrieveDDDW(DwMain, "req_status", "as_capital.pbl", null);
            DwUtil.RetrieveDDDW(DwDetail, "scholarship_type", "as_capital.pbl", state.SsCoopId);
            DwUtil.RetrieveDDDW(DwDetail, "scholarship_level", "as_capital.pbl", null);
            DwUtil.RetrieveDDDW(DwDetail, "childprename_code", "as_public_funds.pbl", null);
        }

        /// <summary>
        /// get เลขที่ใบคำขอล่าสุด
        /// </summary>
        /// <param name="capital_year">ปีทุนสวัสดิการ</param>
        /// <returns>เลขที่ใบคำขอ</returns>
        protected String GetLastDocNo(Decimal capital_year)
        {
            try
            {
                sqlStr = @" SELECT MAX(assist_docno) AS last_no 
                            FROM asnreqmaster 
                            WHERE capital_year='" + capital_year + @"' 
                            AND assist_docno LIKE 'AS%'";
                dt = ta.Query(sqlStr);
                if (dt.Next())
                {
                    string lastDocNo = dt.GetString(0);
                    if (lastDocNo == "")
                    {
                        lastDocNo = "000000";
                    }
                    else
                    {
                        lastDocNo = lastDocNo.Replace("AS", "");
                    }
                    int iii = int.Parse(lastDocNo);
                    assist_docno = "AS" + (iii + 1).ToString("000000");
                    return assist_docno;
                }
                else
                {
                    assist_docno = "AS000000";
                    return assist_docno;
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
            return "";
        }

        /// <summary>
        /// Get Detail สมาชิก
        /// </summary>
        protected void GetMemberDetail()
        {
            try { member_no = DwMem.GetItemString(1, "member_no"); }
            catch { member_no = "0"; }
            member_no = WebUtil.MemberNoFormat(member_no);
            try { capital_year = DwMain.GetItemDecimal(1, "capital_year"); }
            catch { capital_year = Convert.ToDecimal(state.SsWorkDate.Year) + 543; }

            DwMain.Reset();
            DwMain.InsertRow(0);
            DwDetail.Reset();
            DwDetail.InsertRow(0);

            #region Chk งวดหุ้น(ไม่ใช้)
            //            try
            //            {
            //                sqlStr = @" SELECT last_period 
            //                            FROM shsharemaster 
            //                            WHERE member_no ='" + member_no + @"'
            //                          ";
            //                dt = ta.Query(sqlStr);
            //                if (dt.Rows.Count > 0)
            //                {
            //                    int period = Convert.ToInt32(dt.Rows[0]["last_period"].ToString());
            //                    if (period < 13)
            //                    {
            //                        LtServerMessage.Text = WebUtil.WarningMessage("ทะเบียน " + member_no + " ยังไม่สามารถทำการขอทุนได้");
            //                        JsNewClear();
            //                        return;
            //                    }
            //                }
            //            }
            //            catch { WebUtil.ErrorMessage("GetMemberDetail ERROR!(Chk งวดหุ้น)"); }
            #endregion

            #region Retrieve DwMem
            DwUtil.RetrieveDataWindow(DwMem, pbl, null, member_no);
            try { surname_child = DwMem.GetItemString(1, "memb_surname"); }
            catch { surname_child = ""; }
            try
            {
                String reMember_no = DwMem.GetItemString(1, "member_no").Trim();
                if (reMember_no == "")
                {
                    DwMem.Reset();
                    DwDetail.Reset();
                }
            }
            catch { DwMem.Reset(); DwDetail.Reset(); }
            #endregion

            #region กรณีใบคำขอเก่า(ไม่ใช้)
//            if (DwMem.RowCount > 0)
//            {
//                sqlStr = @" SELECT * 
//                                        FROM asnreqmaster 
//                                        WHERE member_no = '" + member_no + @"' 
//                                        AND assist_docno LIKE 'AS%'";
//                dt = ta.Query(sqlStr);
//                if (dt.Next())
//                {

//                }
//            }
            #endregion

        }


        #endregion
    }
}