using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataLibrary;
using System.Data;

namespace Saving.Applications.hr.ws_hr_cure_family_ctrl
{
    public partial class ws_hr_cure_family : PageWebSheet, WebSheet
    {
        [JsPostBack]
        public string PostEmpno { get; set; }
        [JsPostBack]
        public string PostCalMoney { get; set; }
        [JsPostBack]
        public string PostDeleteRow { get; set; }
        [JsPostBack]
        public String PostInsertRow { get; set; }
        [JsPostBack]
        public String PostCalhalf { get; set; }
        [JsPostBack]
        public String PostEmpLlist { get; set; }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == PostEmpno)
            {
                dsDetail.Visible = true;
                dsList.Visible = true;
                dsMain.Visible = true;
                string emp_no = dsMain.DATA[0].emp_no;
                dsMain.Retrieve(emp_no);
                dsMain.DATA[0].emp_no = emp_no;
                dsDetail.DATA[0].ASSIST_SDATE = state.SsWorkDate;
                dsDetail.DATA[0].ASSIST_EDATE = state.SsWorkDate;
                dsList.RetreiveList(emp_no);
            }
            else if (eventArg == PostEmpLlist)
            {

                string emp_no = dsMain.DATA[0].emp_no;
                int row = dsList.GetRowFocus();
                string Seqno = Convert.ToString(dsList.DATA[row].SEQ_NO);
                dsMain.Retrieve(emp_no);
                dsMain.DATA[0].emp_no = emp_no;
                dsDetail.DATA[0].ASSIST_SDATE = state.SsWorkDate;
                dsDetail.DATA[0].ASSIST_EDATE = state.SsWorkDate;
                dsList.RetreiveList(emp_no);
                dsDetail.RetrieveList(Seqno, emp_no);
                dsMain.DATA[0].f_salary = dsDetail.DATA[0].ASSIST_FSALARY;

            }

        }

        public void InitJsPostBack()
        {
            dsMain.InitDsMain(this);
            dsList.InitDsList(this);
            dsDetail.InitDsDetail(this);
        }

        public void SaveWebSheet()
        {
            ExecuteDataSource exed = new ExecuteDataSource(this);
            string coop_id = state.SsCoopControl;
            string EmpNo = dsMain.DATA[0].emp_no;
            string sql = "select * from hremployeeassist where coop_id = {0} and emp_no = {1}";
            string row = Convert.ToString(dsDetail.DATA[0].SEQ_NO);
            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, dsMain.DATA[0].emp_no.Trim());
            Sdt dt = WebUtil.QuerySdt(sql);
            decimal moneycure_money = 0;
            decimal assist_minamt = 0;
            string cheack = "";

            if (dsDetail.DATA[0].ASSIST_STATE == "ตนเอง")
            {
                cheack = "01";
            }
            else
            {
                cheack = "02";
            }

            string sql5 = "select moneycure_money from hrucfmoneycure where moneycure_code = {0}";
            sql5 = WebUtil.SQLFormat(sql5, cheack);
            Sdt dt5 = WebUtil.QuerySdt(sql5);

            if (dt5.Next())
            {
                moneycure_money = dt5.GetDecimal("moneycure_money");
            }

            if (cheack == "01")
            {

                string sql1 = "select sum(assist_minamt) as assist_amt from hremployeeassist where coop_id = {0} and emp_no = {1} and assist_state = {2}";
                sql1 = WebUtil.SQLFormat(sql1, state.SsCoopControl, dsMain.DATA[0].emp_no.Trim(), "ตนเอง");
                Sdt dt1 = WebUtil.QuerySdt(sql1);

                if (dt1.Next())
                {
                    assist_minamt = dt1.GetDecimal("assist_minamt");
                }
                   
            }
            else
            {
                string sql4 = "select sum(assist_minamt) as assist_amt from hremployeeassist where coop_id = {0} and emp_no = {1} and assist_state != {2}";
                sql4 = WebUtil.SQLFormat(sql4, state.SsCoopControl, dsMain.DATA[0].emp_no.Trim(), "ตนเอง");
                Sdt dt4 = WebUtil.QuerySdt(sql4);

                if (dt4.Next())
                {
                    assist_minamt = dt4.GetDecimal("assist_minamt");
                }

                
            }

            try
            {
                if (dt.Rows.Count <= 0)
                {
                    decimal SeqNo = 1;//unique
                    string fullname = dsMain.DATA[0].fullname;
                    EmpNo = dsMain.DATA[0].emp_no;//unique
                    decimal salary = Convert.ToDecimal(dsMain.DATA[0].f_salary);
                    dsDetail.DATA[0].SEQ_NO = SeqNo;
                    dsDetail.DATA[0].ASSIST_CODE = "01";
                    dsDetail.DATA[0].ASSIST_FSALARY = salary;
                    dsDetail.DATA[0].EMP_NO = EmpNo;
                    dsDetail.DATA[0].COOP_ID = coop_id;
                    dsDetail.DATA[0].ASSIST_DATE = state.SsWorkDate;


                    if (assist_minamt <= moneycure_money)
                    {

                        ExecuteDataSource exe = new ExecuteDataSource(this);
                        exe.AddFormView(dsDetail, ExecuteType.Insert);
                        exe.SQL.Add(sql);
                        exe.Execute();
                        exe.SQL.Clear();
                        LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลสำเร็จ" + " " + EmpNo + " " + fullname);
                    }
                    else
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถทำรายการได้เนื่องจากเกินวงเงินเบิกเเล้ว");
                    }

                    //ออกรายงาน
                    string as_empno = dsMain.DATA[0].emp_no;
                    decimal as_seqno = dsDetail.DATA[0].SEQ_NO;

                    iReportArgument args = new iReportArgument();
                    args.Add("as_coopid", iReportArgumentType.String, state.SsCoopId);
                    args.Add("as_empno", iReportArgumentType.String, as_empno);
                    args.Add("as_seqno", iReportArgumentType.String, Convert.ToString(as_seqno));

                    iReportBuider ireport = new iReportBuider(this, "กำลังออกรายงาน.....");
                    ireport.AddCriteria("r_hr_cure_family", "ใบเบิกเงินค่ารักษาพยาบาล", ReportType.pdf, args);
                    ireport.AutoOpenPDF = true;
                    ireport.Retrieve();

                    reset();
                    dsDetail.Visible = false;
                    dsList.Visible = false;
                }
                else
                {

                    string fullname = dsMain.DATA[0].fullname;
                    decimal salary = Convert.ToDecimal(dsMain.DATA[0].f_salary);
                    //string seqno = Convert.ToString(dsList.DATA[0].SEQ_NO);
                    if (dsDetail.DATA[0].SEQ_NO.ToString() != "0")
                    {
                        string sql2 = "select * from hremployeeassist where coop_id = {0} and emp_no = {1}";
                        sql2 = WebUtil.SQLFormat(sql2, state.SsCoopControl, dsDetail.DATA[0].EMP_NO.Trim());
                        Sdt dt2 = WebUtil.QuerySdt(sql);
                        dsDetail.DATA[0].COOP_ID = coop_id;
                        dsDetail.DATA[0].ASSIST_FSALARY = salary;
                        dsDetail.DATA[0].SEQ_NO = dsDetail.DATA[0].SEQ_NO;
                        exed.AddFormView(dsDetail, ExecuteType.Update);
                        exed.Execute();
                        exed.SQL.Clear();
                        LtServerMessage.Text = WebUtil.CompleteMessage("แก้ไขข้อมูลสำเร็จ" + " " + EmpNo + " " + fullname);
                        //ออกรายงาน
                        string as_empno = dsMain.DATA[0].emp_no;
                        decimal as_seqno = dsDetail.DATA[0].SEQ_NO;

                        iReportArgument args = new iReportArgument();
                        args.Add("as_coopid", iReportArgumentType.String, state.SsCoopId);
                        args.Add("as_empno", iReportArgumentType.String, as_empno);
                        args.Add("as_seqno", iReportArgumentType.String, Convert.ToString(as_seqno));

                        iReportBuider ireport = new iReportBuider(this, "กำลังออกรายงาน.....");
                        ireport.AddCriteria("r_hr_cure_family", "ใบเบิกเงินค่ารักษาพยาบาล", ReportType.pdf, args);
                        ireport.AutoOpenPDF = true;
                        ireport.Retrieve();

                        reset();
                        dsDetail.Visible = false;
                        dsList.Visible = false;
                    }
                    else
                    //if (dsList.DATA[0].SEQ_NO.ToString() == SeqNo.ToString())
                    {
                        string CoopId = state.SsCoopId;//unique
                        decimal SeqNo = 1;//unique
                        string sql2 = @"select max(seq_no)+1 as seq_no from hremployeeassist where emp_no ={0} and coop_id={1}";//นำค่าแมกของ seq_no บวก 1 เพื่อให้เป็นค่า seq_no ของลำดับต่อไป
                        sql2 = WebUtil.SQLFormat(sql2, EmpNo, CoopId);//format ในรูปของ sql
                        Sdt dt2 = WebUtil.QuerySdt(sql2);
                        if (dt2.Next())
                        {
                            SeqNo = dt2.GetDecimal("seq_no");
                        }
                        dsDetail.DATA[0].SEQ_NO = SeqNo;//กำหนดค่าให้ ds.Leave >> SEQ_NO ใหม่ ให้เป็น ค่าใหม่ที่กำหนด จาก string sql select max(seq_no)+1
                        dsDetail.DATA[0].ASSIST_CODE = "01";
                        dsDetail.DATA[0].ASSIST_FSALARY = salary;
                        dsDetail.DATA[0].EMP_NO = EmpNo;
                        dsDetail.DATA[0].COOP_ID = coop_id;
                        dsDetail.DATA[0].ASSIST_DATE = state.SsWorkDate;

                        if (assist_minamt <= moneycure_money)
                        {

                            ExecuteDataSource exe = new ExecuteDataSource(this);
                            exe.AddFormView(dsDetail, ExecuteType.Insert);
                            exe.Execute();
                            LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลสำเร็จ " + " " + EmpNo + " " + fullname);
                        }
                        else
                        {
                            LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถทำรายการได้เนื่องจากเกินวงเงินเบิกเเล้ว");
                        }
                        //ออกรายงาน
                        string as_empno = dsMain.DATA[0].emp_no;
                        decimal as_seqno = dsDetail.DATA[0].SEQ_NO;

                        iReportArgument args = new iReportArgument();
                        args.Add("as_coopid", iReportArgumentType.String, state.SsCoopId);
                        args.Add("as_empno", iReportArgumentType.String, as_empno);
                        args.Add("as_seqno", iReportArgumentType.String, Convert.ToString(as_seqno));

                        iReportBuider ireport = new iReportBuider(this, "กำลังออกรายงาน.....");
                        ireport.AddCriteria("r_hr_cure_family", "ใบเบิกเงินค่ารักษาพยาบาล", ReportType.pdf, args);
                        ireport.AutoOpenPDF = true;
                        ireport.Retrieve();

                        reset();
                        dsDetail.Visible = false;
                        dsList.Visible = false;
                    }
                }

                dsList.RetreiveList(EmpNo);
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        private void reset()
        {
            dsMain.ResetRow();
            dsDetail.ResetRow();
            dsList.ResetRow();
        }


        public void WebSheetLoadBegin()
        {

        }

        public void WebSheetLoadEnd()
        {

        }

    }
}