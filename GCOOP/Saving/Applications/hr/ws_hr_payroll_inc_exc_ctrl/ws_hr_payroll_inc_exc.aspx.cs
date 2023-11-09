using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using DataLibrary;

namespace Saving.Applications.hr.ws_hr_payroll_inc_exc_ctrl
{
    public partial class ws_hr_payroll_inc_exc : PageWebSheet, WebSheet
    {
        [JsPostBack]
        public String PostEmpNo { get; set; }
        [JsPostBack]
        public String PostRetrieve { get; set; }
        [JsPostBack]
        public String PostInsertRowFixed { get; set; }
        [JsPostBack]
        public String PostInsertRowOther { get; set; }
        [JsPostBack]
        public String PostDeleteFixedRow { get; set; }
        [JsPostBack]
        public String PostDeleteOtherRow { get; set; }

        public void InitJsPostBack()
        {
            dsMain.InitDsMain(this);
            dsListFixed.InitDsListFixed(this);
            //dsListOther.InitDsListOther(this);
            dsList.InitDsList(this);
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                NewClear();
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == PostEmpNo)
            {
                string ls_empno = dsMain.DATA[0].EMP_NO;
                dsMain.Retrieve(ls_empno);
                dsListFixed.Retrieve(ls_empno);
                //dsListOther.Retrieve(ls_empno, "");
                //dsListOther.DdSalaryItem();
                dsListFixed.DdSalaryItem();
                dsList.Retrieve(ls_empno);
            }
            else if (eventArg == PostRetrieve)
            {
                //string ls_period = year.Text + Convert.ToDecimal(month.Text).ToString("00");
                string ls_empno = dsMain.DATA[0].EMP_NO;
                //dsListOther.Retrieve(ls_empno, ls_period);
                //dsListOther.DdSalaryItem();
            }
            else if (eventArg == PostInsertRowFixed)
            {
                dsListFixed.InsertLastRow();
                dsListFixed.DdSalaryItem();
            }
            else if (eventArg == PostInsertRowOther)
            {
                //dsListOther.InsertLastRow();
                //dsListOther.DdSalaryItem();
            }
            else if (eventArg == PostDeleteFixedRow)
            {
                int r = dsListFixed.GetRowFocus();
                dsListFixed.DeleteRow(r);
                dsListFixed.DdSalaryItem();
            }
            else if (eventArg == PostDeleteOtherRow)
            {
                //int r = dsListOther.GetRowFocus();
                //dsListOther.DeleteRow(r);
                //dsListOther.DdSalaryItem();
            }
        }

        public void NewClear()
        {
            dsMain.ResetRow();
            //dsListOther.ResetRow();
            dsListFixed.ResetRow();
            dsList.ResetRow();

            //year.Text = Convert.ToString(WebUtil.GetAccyear(state.SsCoopControl, state.SsWorkDate));
            //month.Text = DateTime.Now.Month.ToString();
        }

        public void SaveWebSheet()
        {
            ExecuteDataSource exe = new ExecuteDataSource(this);
            string ls_coopid = state.SsCoopControl;
            string ls_empno = dsMain.DATA[0].EMP_NO;
            string fullname = dsMain.DATA[0].cp_name;
            //string ls_period = year.Text + Convert.ToDecimal(month.Text).ToString("00");

            //set ค่า
            for (int i = 0; i < dsListFixed.RowCount; i++)
            {
                dsListFixed.DATA[i].COOP_ID = ls_coopid;
                dsListFixed.DATA[i].EMP_NO = ls_empno;
                dsListFixed.DATA[i].SEQ_NO = i + 1;
            }

            //for (int i = 0; i < dsListOther.RowCount; i++)
            //{
            //    dsListOther.DATA[i].COOP_ID = ls_coopid;
            //    dsListOther.DATA[i].EMP_NO = ls_empno;                
            //    dsListOther.DATA[i].SEQ_NO = i + 1;
            //}

            //hrbasepayrollfixed
            String sqldelfixed = ("delete hrbasepayrollfixed where coop_id ='" + ls_coopid + "' and emp_no = '" + ls_empno + "'");
            exe.SQL.Add(sqldelfixed);

            for (int i = 0; i < dsListFixed.RowCount; i++)
            {
                string sqlInsertFixed = @"insert into hrbasepayrollfixed(coop_id,
                    emp_no,   
                    seq_no,   
                    salitem_code,   
                    item_amt )values({0},{1},{2},{3},{4})";
                object[] argslistInsert = new object[] { ls_coopid,
                        dsListFixed.DATA[0].EMP_NO,
                        dsListFixed.DATA[i].SEQ_NO,
                        dsListFixed.DATA[i].SALITEM_CODE,
                        dsListFixed.DATA[i].ITEM_AMT };
                sqlInsertFixed = WebUtil.SQLFormat(sqlInsertFixed, argslistInsert);
                exe.SQL.Add(sqlInsertFixed);
            }

            //hrbasepayrollother
//            String sqldelother = ("delete hrbasepayrollother where coop_id ='" + ls_coopid + "' and emp_no = '" + ls_empno + "'");
//            exe.SQL.Add(sqldelother);

//            for (int i = 0; i < dsListOther.RowCount; i++)
//            {
//                string sqlInsertOther = @"insert into hrbasepayrollother(coop_id,
//                    emp_no,   
//                    payroll_period,   
//                    seq_no,   
//                    salitem_code,   
//                    payother_desc,   
//                    item_amt )values({0},{1},{2},{3},{4},{5},{6})";
//                object[] argslistInsert = new object[] { ls_coopid,
//                        dsListOther.DATA[0].EMP_NO,
//                        dsListOther.DATA[i].PAYROLL_PERIOD,
//                        dsListOther.DATA[i].SEQ_NO,
//                        dsListOther.DATA[i].SALITEM_CODE,
//                        dsListOther.DATA[i].PAYOTHER_DESC,
//                        dsListOther.DATA[i].ITEM_AMT };
//                sqlInsertOther = WebUtil.SQLFormat(sqlInsertOther, argslistInsert);
//                exe.SQL.Add(sqlInsertOther);
//            }

            try
            {
                exe.Execute();
                LtServerMessage.Text = WebUtil.CompleteMessage(ls_empno + " " + fullname + "บันทึกสำเร็จ");
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }

            NewClear();
        }

        public void WebSheetLoadEnd()
        {

        }
    }
}