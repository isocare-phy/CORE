using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataLibrary;
using CoreSavingLibrary.WcfNCommon;
using System.Data;

namespace Saving.Applications.hr.w_sheet_hr_employee_workday_ctrl
{
    public partial class w_sheet_hr_employee_workday : PageWebSheet, WebSheet
    {
        [JsPostBack]
        public string PostInsertRow { get; set; } //สำสั่ง JS postback
        [JsPostBack]
        public string PostDeleteRow { get; set; } //สำสั่ง JS postback
        [JsPostBack]
        public string PostList { get; set; } //สำสั่ง JS postback


        public void InitJsPostBack()
        {
            dsList.InitDsList(this);
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                // dsList.retrieve();
                checkin_date.Text = string.Format("{0:dd/MM/yyyy}", state.SsWorkDate);
            }
           

        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == PostInsertRow)
            {
                dsList.InsertAtRow(0);

            }
            else if (eventArg == PostDeleteRow)
            {
                int Rowdelete = dsList.GetRowFocus();
                dsList.DeleteRow(Rowdelete);

            }
            else if (eventArg == PostList)
            {
                PostListDetail();
            }
        }

        public void SaveWebSheet()
        {
            try
            {
                ExecuteDataSource exed = new ExecuteDataSource(this);

                for (int i = 0; i < dsList.RowCount; i++)
                {

                    string emplid = dsList.DATA[i].EMPLID.Trim();
                    decimal checkin = dsList.DATA[i].CHECK_IN;
                    string seq_no = dsList.DATA[i].SEQ_NO;
                    string update = "update HRNMLEMPLWORKDAYDEA set check_in = '" + checkin + "' where emplid = '" + emplid + "' and seq_no = '" + seq_no + "'";
                    exed.SQL.Add(update);
                }
                int result = exed.Execute();

                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ"); //หากบันทึกให้ขึ้น บันทึกแล้ว


            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message); // กรณีไม่สำเร็จ
            }

        }

        public void WebSheetLoadEnd()
        {
        }
        protected void CheckAllChanged(object sender, EventArgs e)
        {
            string[] aa;
            int rowcount = dsList.RowCount;
            int i;
            if (check_in.Checked == true)
            {
                for (i = 0; i < rowcount; i++)
                {
                    dsList.DATA[i].CHECK_IN = 1;
                }
            }
            else
            {
                for (i = 0; i < rowcount; i++)
                {
                    dsList.DATA[i].CHECK_IN = 0;
                }
            }

        }
        protected void PostListDetail()
        {
            string[] s = new string[] { };
            DateTime workdatet = new DateTime();
            string workdate = checkin_date.Text.Trim().Replace("/", "");
            try
            {
                workdatet = DateTime.ParseExact(workdate, "ddMMyyyy", WebUtil.TH);
            }
            catch { LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอกรูปแบบวันที่ให้ถูกต้อง"); }
            string work_dd = workdatet.Day.ToString();
            string work_mm = workdatet.Month.ToString();
            int worki_yyyy = workdatet.Year;
            string work_yyyy = worki_yyyy.ToString();
            checkin_date.Text = string.Format("{0:dd/MM/yyyy}", workdatet);
            dsList.retrieve(work_dd, work_mm, work_yyyy);
            if (dsList.RowCount < 1)
            {
                ExecuteDataSource exed = new ExecuteDataSource(this);
                String seq_tmp = GetDocNo("HRWORKDATE");
                seq_tmp = WebUtil.Right(seq_tmp, 6);
                string seq_new = "WD" + GetYear("HREMPLFILEMAS") + seq_tmp;
                string selemplid = "select emplid from hrnmlemplfilemas where emplstat = '1'";
                DataTable dtemplid = WebUtil.Query(selemplid);
                for (int i = 0; i < dtemplid.Rows.Count; i++)
                {
                    string sql = @"INSERT INTO HRNMLEMPLWORKDAYDEA (seq_no, emplid, workdate, t_in, t_out, ot_in, ot_out, entry_id, entry_date,branch_id, remark, ot_amt, type_ot,check_in )";
                    sql += " VALUES ('" + seq_new + "', '" + dtemplid.Rows[i]["emplid"].ToString() + "', to_date('" + work_dd + "/" + work_mm + "/" + work_yyyy + "' ,'dd/mm/yyyy') , '', '' , '', '', '" + state.SsUsername + "' ,  to_date('" + work_dd + "/" + work_mm + "/" + work_yyyy + "' ,'dd/mm/yyyy') , '" + state.SsCoopId + "', '', '','','0')";
                    exed.SQL.Add(sql);
                }
                try
                {
                    int result = exed.Execute();
                }
                catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
                dsList.retrieve(work_dd, work_mm, work_yyyy);
            }
            check_in.Checked = false;
        }
        protected String GetDocNo(String doc_code)
        {
            n_commonClient comm = wcf.NCommon;
            String maxdoc = comm.of_getnewdocno(state.SsWsPass,state.SsCoopId, doc_code);
            return maxdoc;
        }
        protected String GetYear(String doc_code)
        {
            String yy = "";

            try
            {
                String sql = @"select document_year from cmshrlondoccontrol where Document_Code = '" + doc_code + "'";


                DataTable dtdoc = WebUtil.Query(sql);
                yy = dtdoc.Rows[0]["document_year"].ToString().Trim();
                yy = WebUtil.Mid(yy, 2, 2);
            }
            catch (Exception ex)
            {
                String err = ex.ToString();
            }


            return yy;
        }

    }
}