using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using System.Data;
using DataLibrary;

namespace Saving.Applications.hr.ws_hr_changework_ctrl
{
    public partial class ws_hr_changework : PageWebSheet, WebSheet
    {
        [JsPostBack]
        public string PostEmpNo { get; set; }

        [JsPostBack]
        public string PostList { get; set; }

        public void InitJsPostBack()
        {
            dsMain.InitDs(this);
            dsDetail.InitDs(this);
            dsList.InitDsList(this);
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                //dsDetail.DdHrucfdeptline();
                //dsDetail.DATA[0].COOP_ID = state.SsCoopId;
                //dsDetail.DATA[0].START_DATE = state.SsWorkDate;
                dsDetail.Visible = false;
                dsList.Visible = false;
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == PostEmpNo)
            {
                DateTime START_DATE;

                dsDetail.Visible = true;
                dsList.Visible = true;
                try//try หาว่า ติดปัญหาอะไร
                {
                    string EmpNo = dsMain.DATA[0].EMP_NO;
                    dsMain.RetrieveEmp(EmpNo);
                    dsDetail.RetrieveHrEmployee(EmpNo);
                    dsDetail.DATA[0].EMP_NO = EmpNo;

                    String sql = @"SELECT HR.WORK_DATE AS WORK_DATE
                    FROM HREMPLOYEE HR           	                        
                    WHERE HR.COOP_ID = {0}
                    AND HR.EMP_NO = {1}";
                    sql = WebUtil.SQLFormat(sql, state.SsCoopControl, EmpNo);
                Sdt dt = WebUtil.QuerySdt(sql);
                if (dt.Next())
                {

                    START_DATE = dt.GetDate("WORK_DATE");
                    dsDetail.DATA[0].START_DATE = START_DATE;
                }

                    dsDetail.DATA[0].ORDER_DATE = state.SsWorkDate;
                    dsDetail.DATA[0].COOP_ID = state.SsCoopId;//ต้องเรียก จาก state และส่งไปยัง DataSet เพราะค่า coop_id ห้าม null 
                    dsDetail.DdHrucfdeptgrp();
                    dsDetail.DdHrucfposition();
                    dsList.RetrieveHrEmployee(EmpNo);// add detail logwork
                }
                catch (Exception ex)
                { //ถ้าติดปัญหาให้โชว์ว่าเกิดจากอะไร
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
                }
            }
        }

        public void SaveWebSheet()
        {
            try
            {
                decimal SeqNo = 1;//unique
                decimal SeqNo_max = 0;
                string EmpNo = dsMain.DATA[0].EMP_NO;//unique
                string CoopId = state.SsCoopId;//unique
                string sql = @"select max(seq_no)+1 as seq_no from hrlogchangework where emp_no ={0} and coop_id={1}";
                sql = WebUtil.SQLFormat(sql, EmpNo, CoopId);//unique
                Sdt dt = WebUtil.QuerySdt(sql);
                if (dt.Next())
                {
                    SeqNo = dt.GetDecimal("seq_no");
                    SeqNo_max = SeqNo + 1;
                }
                dsDetail.DATA[0].SEQ_NO = SeqNo_max;

                ExecuteDataSource exe = new ExecuteDataSource(this);
                //exe.AddFormView(dsDetail, ExecuteType.Insert);
                //exe.Execute();

             

                string ls_sql = @"insert into HRLOGCHANGEWORK(COOP_ID, EMP_NO, SEQ_NO, DEPTGRP_CODE, POS_CODE, SALARY_AMT, OLD_DEPTGRP_CODE, OLD_POS_CODE, OLD_SALARY_AMT, 
                                                              ORDER_DOCNO, START_DATE, ORDER_DATE)
                                                              values({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11})";
                ls_sql = WebUtil.SQLFormat(ls_sql, state.SsCoopControl, EmpNo, SeqNo_max , dsDetail.DATA[0].DEPTGRP_CODE, dsDetail.DATA[0].POS_CODE, dsDetail.DATA[0].SALARY_AMT, dsDetail.DATA[0].OLD_DEPTGRP_CODE, dsDetail.DATA[0].OLD_POS_CODE, dsDetail.DATA[0].OLD_SALARY_AMT, dsDetail.DATA[0].ORDER_DOCNO, dsDetail.DATA[0].START_DATE, dsDetail.DATA[0].ORDER_DATE);
                exe.SQL.Add(ls_sql);
                exe.Execute();
                exe.SQL.Clear();

                

               



                string OldDeptGrpCode = dsDetail.DATA[0].DEPTGRP_CODE;
                string OldPostCode = dsDetail.DATA[0].POS_CODE;
                decimal OldSalaryAmt = dsDetail.DATA[0].SALARY_AMT;//ตัวที่ต้องการเปลี่ยน


              /*  string sql_update = @"update hremployee set deptgrp_code = {0},pos_code = {1},salary_amt = {2} where emp_no = {3} ";
                object[] argslist_sql_update = new object[] { OldDeptGrpCode, OldPostCode, OldSalaryAmt, EmpNo };

                sql_update = WebUtil.SQLFormat(sql_update, argslist_sql_update);
                exe.SQL.Add(sql_update);
                exe.Execute();
                exe.SQL.Clear();*/

                String sql_update = @"update hremployee set deptgrp_code = {0},pos_code = {1},salary_amt = {2} where emp_no = {3}";
                sql_update = WebUtil.SQLFormat(sql_update, OldDeptGrpCode, OldPostCode, OldSalaryAmt, EmpNo);
                Sdt dt_update = WebUtil.QuerySdt(sql_update);

                ///// update วันที่คำสั่ง
               /* String sql_update_order = @"update hrlogchangework set order_date = {0}  where emp_no ={1} and coop_id={2} and seq_no = {3}";
                sql_update_order = WebUtil.SQLFormat(sql_update_order, dsDetail.DATA[0].ORDER_DATE, EmpNo, state.SsCoopId, SeqNo_max);
                Sdt dt_update_order = WebUtil.QuerySdt(sql_update_order);*/

                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
                dsList.RetrieveHrEmployee(EmpNo);
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถบันทึกได้" + ex.Message);
            }
        }

        public void WebSheetLoadEnd()
        {

        }
    }
}