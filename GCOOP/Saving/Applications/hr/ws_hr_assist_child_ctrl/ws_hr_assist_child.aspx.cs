using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataLibrary;
using System.Data;

namespace Saving.Applications.hr.ws_hr_assist_child_ctrl
{
    public partial class ws_hr_assist_child : PageWebSheet, WebSheet
    {
        [JsPostBack]
        public string PostEmpno { get; set; }
        [JsPostBack]
        public string PostEmpLlist { get; set; }
        [JsPostBack]
        public string PostDeleteRow { get; set; }
        [JsPostBack]
        public String PostInsertRow { get; set; }
        [JsPostBack]
        public String PostCalhalf { get; set; }
        [JsPostBack]
        public String PostDelete { get; set; }

        decimal assist_halfamt = 0;

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == PostEmpno)
            {
                dsDetail.Visible = true;
                dsList.Visible = true;
                dsMain.Visible = true;
                string emp_no = dsMain.DATA[0].emp_no;
                dsMain.Retrieve(emp_no);
                dsDetail.DdEducation();
                dsList.Retrieve(emp_no);
                dsMain.DATA[0].emp_no = emp_no;
                

                string sql = "select sum(assist_amt) as assist_amt from hremployeeassist where coop_id = {0} and emp_no = {1} and assist_code = '02'";
            //string assist_detail = "";
            //string row = Convert.ToString(dsDetail.DATA[0].SEQ_NO);
            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, dsMain.DATA[0].emp_no.Trim());
            Sdt dt = WebUtil.QuerySdt(sql);
            
                if (dt.Next())
                {
                    assist_halfamt = dt.GetDecimal("assist_amt"); ;
                }

                dsDetail.DATA[0].ASSIST_HALFAMT = assist_halfamt;
                
            }
            if (eventArg == PostEmpLlist)
            {
                string emp_no = dsMain.DATA[0].emp_no;
                dsMain.Retrieve(emp_no);
                dsList.Retrieve(emp_no);
                dsMain.DATA[0].emp_no = emp_no;
                int row = dsList.GetRowFocus();
                string Seqno = Convert.ToString(dsList.DATA[row].SEQ_NO);
                dsDetail.RetrieveDetail(Seqno, emp_no);
                dsDetail.DdEducation();

                string sql = "select sum(assist_amt) as assist_amt from hremployeeassist where coop_id = {0} and emp_no = {1} and assist_code = '02'";
                //string assist_detail = "";
                //string row = Convert.ToString(dsDetail.DATA[0].SEQ_NO);
                sql = WebUtil.SQLFormat(sql, state.SsCoopControl, dsMain.DATA[0].emp_no.Trim());
                Sdt dt = WebUtil.QuerySdt(sql);

                if (dt.Next())
                {
                    assist_halfamt = dt.GetDecimal("assist_amt"); ;
                }

                dsDetail.DATA[0].ASSIST_HALFAMT = assist_halfamt;

            }
            else if (eventArg == PostDelete)
            {

                int row = dsList.GetRowFocus();
                string Seqno = Convert.ToString(dsList.DATA[row].SEQ_NO);

                try
                {


                    ExecuteDataSource exe = new ExecuteDataSource(this);
                    string ls_sqldel = @"delete from hremployeeassist where coop_id = '" + state.SsCoopControl + "' and emp_no = '" + dsMain.DATA[0].emp_no.Trim() + "' and assist_code = '02' and seq_no = '" + Seqno + "'";
                    exe.SQL.Add(ls_sqldel);
                    exe.Execute();
                    exe.SQL.Clear();
                    LtServerMessage.Text = WebUtil.CompleteMessage("ลบข้อมูลสำเร็จ");
                    reset();
                    dsDetail.Visible = false;
                    dsList.Visible = false;

                }
                catch (Exception ex)
                {

                    LtServerMessage.Text = WebUtil.ErrorMessage(ex);

                }



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
            string sql = "select * from hremployeeassist where coop_id = {0} and emp_no = {1} and assist_code = '02'";
            //string assist_detail = "";
            string row = Convert.ToString(dsDetail.DATA[0].SEQ_NO);
            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, dsMain.DATA[0].emp_no.Trim());
            Sdt dt = WebUtil.QuerySdt(sql);
            try
            {
                if (dt.Rows.Count <= 0)
                {
                    decimal SeqNo = 1;//unique
                    string fullname = dsMain.DATA[0].fullname;
                    EmpNo = dsMain.DATA[0].emp_no;//unique
                    dsDetail.DATA[0].SEQ_NO = SeqNo;
                    dsDetail.DATA[0].ASSIST_CODE = "02";
                    dsDetail.DATA[0].EMP_NO = EmpNo;
                    dsDetail.DATA[0].COOP_ID = coop_id;
                   // dsDetail.DATA[0].ASSIST_DATE = state.SsWorkDate;
                    dsDetail.DATA[0].ASSIST_FCAREER = dsMain.DATA[0].occupation;
                    //dsDetail.DATA[0].ASSIST_FPOS = dsMain.DATA[0].position_f;
                    //dsDetail.DATA[0].ASSIST_FOFFICE = dsMain.DATA[0].empgroup;

                    string sql1 = "select f_relation from hremployeefamily where f_relation = 'บุตร' and emp_no = {0}";
                    sql1 = WebUtil.SQLFormat(sql1, dsMain.DATA[0].emp_no.Trim());
                    Sdt dt1 = WebUtil.QuerySdt(sql1);
                    if (dt1.Rows.Count <= 0 || Convert.ToDecimal(dsDetail.DATA[0].ASSIST_SON) > 25)
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถทำรายการได้เนื่องจากท่านไม่มีข้อมูลบุตรในทะเบียนประวัติพนักงาน หรือ บุตรของท่านอายุเกิน 25 ปี");
                        
                    }
                    else
                    {

                        ExecuteDataSource exe = new ExecuteDataSource(this);
                        exe.AddFormView(dsDetail, ExecuteType.Insert);
                        exe.SQL.Add(sql);
                        exe.Execute();
                        exe.SQL.Clear();
                        LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลสำเร็จ" + " " + EmpNo + " " + fullname);
                        reset();
                        dsDetail.Visible = false;
                        dsList.Visible = false;
                    }
                    
                   

                }
                else
                {

                    string fullname = dsMain.DATA[0].fullname;
                    //string seqno = Convert.ToString(dsList.DATA[0].SEQ_NO);
                    if (dsDetail.DATA[0].SEQ_NO.ToString() != "0")
                    {
                        string sql2 = "select * from hremployeeassist where coop_id = {0} and emp_no = {1} and assist_code = '02'";
                        sql2 = WebUtil.SQLFormat(sql2, state.SsCoopControl, dsDetail.DATA[0].EMP_NO.Trim());
                        Sdt dt2 = WebUtil.QuerySdt(sql);
                        dsDetail.DATA[0].COOP_ID = coop_id;
                        dsDetail.DATA[0].ASSIST_CODE = "02";
                        dsDetail.DATA[0].ASSIST_FCAREER = dsMain.DATA[0].occupation;
                        exed.AddFormView(dsDetail, ExecuteType.Update);
                        exed.Execute();
                        exed.SQL.Clear();
                        LtServerMessage.Text = WebUtil.CompleteMessage("แก้ไขข้อมูลสำเร็จ" + " " + EmpNo + " " + fullname);
                        reset();
                        dsDetail.Visible = false;
                        dsList.Visible = false;
                        
                    }
                    else
                    //if (dsList.DATA[0].SEQ_NO.ToString() == SeqNo.ToString())
                    {
                        string CoopId = state.SsCoopId;//unique
                        decimal SeqNo = 1;//unique
                        string sql2 = @"select max(seq_no) as seq_no from hremployeeassist where emp_no ={0} and coop_id={1} and assist_code = '02'";//นำค่าแมกของ seq_no บวก 1 เพื่อให้เป็นค่า seq_no ของลำดับต่อไป
                        sql2 = WebUtil.SQLFormat(sql2, EmpNo, CoopId);//format ในรูปของ sql
                        Sdt dt2 = WebUtil.QuerySdt(sql2);
                        if (dt2.Next())
                        {
                            SeqNo = dt2.GetDecimal("seq_no");
                            SeqNo = SeqNo + 1;
                        }
                        dsDetail.DATA[0].SEQ_NO = SeqNo;//กำหนดค่าให้ ds.Leave >> SEQ_NO ใหม่ ให้เป็น ค่าใหม่ที่กำหนด จาก string sql select max(seq_no)+1
                        dsDetail.DATA[0].ASSIST_CODE = "02";
                        dsDetail.DATA[0].EMP_NO = EmpNo;
                        dsDetail.DATA[0].COOP_ID = coop_id;
                       // dsDetail.DATA[0].ASSIST_DATE = state.SsWorkDate;
                        dsDetail.DATA[0].ASSIST_FCAREER = dsMain.DATA[0].occupation;
                        //dsDetail.DATA[0].ASSIST_FPOS = dsMain.DATA[0].position_f;
                        //dsDetail.DATA[0].ASSIST_FOFFICE = dsMain.DATA[0].empgroup;

                        string sql1 = "select f_relation from hremployeefamily where f_relation = 'บุตร' and emp_no = {0}";
                    sql1 = WebUtil.SQLFormat(sql1, dsMain.DATA[0].emp_no.Trim());
                    Sdt dt1 = WebUtil.QuerySdt(sql1);
                    if (dt1.Rows.Count <= 0 || Convert.ToDecimal(dsDetail.DATA[0].ASSIST_SON) > 25)
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถทำรายการได้เนื่องจากท่านไม่มีข้อมูลบุตรในทะเบียนประวัติพนักงาน หรือ บุตรของท่านอายุเกิน 25 ปี");

                    }
                    else
                    {

                        ExecuteDataSource exe = new ExecuteDataSource(this);

                        exe.AddFormView(dsDetail, ExecuteType.Insert);
                        exe.Execute();
                        LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลสำเร็จ " + " " + EmpNo + " " + fullname);
                        reset();
                        dsDetail.Visible = false;
                        dsList.Visible = false;
                    }
                       
                    }
                }
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