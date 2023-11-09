using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CoreSavingLibrary;
using DataLibrary;
using System.Data.OracleClient;

namespace Saving.Applications.hr.ws_hr_apv_leave_ctrl
{
    public partial class ws_hr_apv_leave : PageWebSheet, WebSheet
    {
        [JsPostBack]
        public string PostAPVLEAVE { get; set; }
        [JsPostBack]
        public string PostAPV { get; set; }
        [JsPostBack]
        public string PostCancel { get; set; }

        public void InitJsPostBack()
        {
            dsSearch.InitMain(this);
            dsMain.InitDsDetail(this);
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                //dsMain.Visible = false;
            }
            else
            {
                //dsMain.Visible = false;
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            DateTime date = new DateTime();
            date = dsSearch.DATA[0].LEAVE_DATE;
            if (eventArg == "PostAPVLEAVE")
            {
                dsMain.RetrieveLeaveDetail(date);
            }
            else if (eventArg == "PostAPV")
            {
                string detail = "";
                int apv = 1;
                int row = dsMain.GetRowFocus();
                string _emp_no = dsMain.DATA[row].EMP_NO;
                decimal _seq_no = dsMain.DATA[row].SEQ_NO;
                detail = getDetail(_emp_no);
                updatePostAPV(_emp_no, apv, _seq_no, detail);
                dsMain.RetrieveLeaveDetail(date);
            }
            else if (eventArg == "PostCancel")
            {
                string detail = "";
                int apv = -9;
                int row = dsMain.GetRowFocus();
                string _emp_no = dsMain.DATA[row].EMP_NO;
                decimal _seq_no = dsMain.DATA[row].SEQ_NO;
                detail = getDetail(_emp_no);
                updatePostCANCEL(_emp_no, apv, _seq_no, detail);
                dsMain.RetrieveLeaveDetail(date);
            }
        }

        #region getDetail
        private string getDetail(string _emp_no)
        {
            string detail = @"select  hl.emp_no,hl.seq_no,
                                   hl.leave_date,
		                           hlc.leave_desc,
		                           hl.leave_from,hl.leave_to,
		                           hl.totalday,hl.apv_date,
                                   he.EMP_NAME||' '||he.EMP_SURNAME as fullname         
                            from 
                                  hrlogleave hl,
		                          hrucfleavecode hlc, HREMPLOYEE he
                            where
                                  hl.coop_id={0} and he.emp_no ={1} and
                                  hl.leave_code = hlc.leave_code and
                                  hl.EMP_NO=he.EMP_NO and
                                  hl.apv_status=8
                            order by
                                  hl.seq_no asc,hl.leave_date";
            string full_name = "";
            try
            {
                detail = WebUtil.SQLFormat(detail, state.SsCoopId, _emp_no);
                Sdt dt = WebUtil.QuerySdt(detail);
                if (dt.Next())
                {
                    full_name = dt.GetString("fullname");
                }
            }
            catch (OracleException ora)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ora.ToString());
            }
            return full_name;
        }
        #endregion

        #region updatePostCANCEL
        private void updatePostCANCEL(string _emp_no, int apv, decimal _seq_no, string _detail)
        {
            ExecuteDataSource exes = new ExecuteDataSource(this);
            string update_apv = @"update hrlogleave set apv_status={0}
                                where emp_no={1} and seq_no={2}";
            try
            {
                object[] argUpdate = new object[] { apv, _emp_no, _seq_no };
                update_apv = WebUtil.SQLFormat(update_apv, argUpdate);
                exes.SQL.Add(update_apv);
                exes.Execute();
            }
            catch (OracleException or)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(or.ToString());
            }
            finally
            {
                exes.SQL.Clear();
                LtServerMessage.Text = WebUtil.CompleteMessage(_emp_no + " " + _detail + " " + "ไม่อนุมัติการลางาน");
            }
        }
        #endregion

        #region updatePostAPV
        private void updatePostAPV(string _emp_no, int apv, decimal _seq_no, string _detail)
        {
            ExecuteDataSource exes = new ExecuteDataSource(this);
            string update_apv = @"update hrlogleave set apv_status={0}
                                where emp_no={1} and seq_no= {2}";
            try
            {
                object[] argUpdate = new object[] { apv, _emp_no, _seq_no };
                update_apv = WebUtil.SQLFormat(update_apv, argUpdate);
                exes.SQL.Add(update_apv);
                exes.Execute();
            }
            catch (OracleException or)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(or.ToString());
            }
            finally
            {
                exes.SQL.Clear();
                LtServerMessage.Text = WebUtil.CompleteMessage(_emp_no + " " + _detail + " " + "อนุมัติการลางาน");
            }
        }
        #endregion

        public void SaveWebSheet()
        {

        }

        public void WebSheetLoadEnd()
        {
            dsSearch.ResetRow();
        }
    }
}