using System;
using CoreSavingLibrary;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using DataLibrary;
using Sybase.DataWindow;
using CoreSavingLibrary.WcfNCommon;

namespace Saving.Applications
{
    public partial class w_sheet_hr_employee_workday : PageWebSheet, WebSheet
    {
        private String emplid;
        private DwThDate tDwMain;
        private DwThDate tDwDetail;
        private DwThDate tDwList;
        public String a = "";
        protected String postNewClear;
        protected String postGetMember;
        protected String postAddRow;
        protected String postShowDetail;
        protected String postDeleteRow;
        protected String postSearchGetMember;
        protected String postCheckTimeWork;
        protected String postDifferTime;
        //==================
        // ตรวจสอบว่า ใส่รูปแบบเวลาถูกต้อง หรือไม่




        /// <summary>
        /// เพิ่มรายการมาสายในตาราง hrnmlemplleavdea (Insert ค่า)
        /// </summary>
        /// <param name="timerent">เวลาที่สาย</param>

        private void JspostSearchGetMember()
        {
            String emplcode = null;
            String emplid = null;

            emplcode = DwMain.GetItemString(1, "emplcode");

            Sta ta = new Sta(sqlca.ConnectionString);
            try
            {
                String sql = @"select emplid from hrnmlemplfilemas where emplcode = '" + emplcode + "'";
                Sdt dt = ta.Query(sql);
                if (dt.Next())
                {
                    emplid = dt.GetString("emplid");
                    DwMain.SetItemString(1, "emplid", emplid);
                    JspostGetMember();
                }
                else
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบข้อมูลเจ้าหน้าที่รายนี้ กรุณากรอกข้อมูลใหม่");
                    JspostNewClear();
                }
            }
            catch (Exception ex)
            {
                String err = ex.ToString();
            }

            ta.Close();
        }

        public void SetDwMasterEnable(int protect)
        {
            try
            {
                if (protect == 1)
                {
                    DwMain.Enabled = false;
                }
                else
                {
                    DwMain.Enabled = true;
                }
                int RowAll = int.Parse(DwMain.Describe("Datawindow.Column.Count"));
                for (int li_index = 1; li_index <= RowAll; li_index++)
                {
                    DwMain.Modify("#" + li_index.ToString() + ".protect= " + protect.ToString());
                }
            }
            catch (Exception ex)
            {
                //  LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
            }
        }


        protected String GetYear(String doc_code)
        {
            String yy = "";
            Sta ta = new Sta(sqlca.ConnectionString);
            try
            {
                String sql = @"select document_year from cmshrlondoccontrol where Document_Code = '" + doc_code + "'";
                Sdt dt = ta.Query(sql);
                if (dt.Next())
                {
                    yy = dt.GetString("document_year");
                }
                yy = WebUtil.Mid(yy, 2, 2);
            }
            catch (Exception ex)
            {
                String err = ex.ToString();
            }

            ta.Close();
            return yy;
        }

        protected String GetDocNo(String doc_code)
        {
            n_commonClient comm = wcf.NCommon;
            String maxdoc = comm.of_getnewdocno(state.SsWsPass,state.SsCoopId, doc_code);
            return maxdoc;
        }

        private void JspostDeleteRow()
        {
            Sta ta = new Sta(sqlca.ConnectionString);
            try
            {
                emplid = DwMain.GetItemString(1, "emplid").Trim();
                String seq_no = DwDetail.GetItemString(1, "seq_no").Trim();

                String sql = @"Delete from hrnmltransfertoofficedea where seq_no = '" + seq_no + "'";
                try
                {
                    ta.Exe(sql);
                    LtServerMessage.Text = WebUtil.CompleteMessage("ลบข้อมูลเรียบร้อยแล้ว");
                    JspostGetMember();
                }
                catch
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถลบข้อมูลรายการ " + seq_no + " ได้");
                }
            }

            catch (Exception ex)
            {
                String err = ex.ToString();

            }
            ta.Close();

            //Retrieve ...
            JspostGetMember();
        }

        private void JspostShowDetail()
        {
            String seq_no = null;
            String emplid = null;
            try
            {
                emplid = DwMain.GetItemString(1, "emplid");
            }
            catch (Exception ex)
            {
                emplid = null;
            }

            seq_no = DwDetail.GetItemString(1, "seq_no");
            DwDetail.Reset();
            DwDetail.Retrieve(emplid);
            DwDetail.SetFilter("seq_no = '" + seq_no + "'");
            DwDetail.Filter();
            DwDetail.SetItemString(1, "seq_no", seq_no);
            tDwDetail.Eng2ThaiAllRow();
        }

        private void JspostAddRow()
        {
            String emplid = null;
            try
            {
                emplid = DwMain.GetItemString(1, "emplid");
            }
            catch { emplid = null; }

            if (emplid == null || emplid == "Auto")
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบรหัสสมาชิก กรุณากรอกรหัสมาชิกก่อน");
                JspostNewClear();
            }
            else
            {
                Decimal salary_old = getSalaryOld(emplid);
                DwList.InsertRow(0);
                DwDetail.Reset();
                DwDetail.InsertRow(0);
                DwDetail.SetItemDate(1, "move_date", state.SsWorkDate);
                DwDetail.SetItemString(1, "seq_no", "Auto");
                DwDetail.SetItemDecimal(1, "salary_old", salary_old);
                tDwDetail.Eng2ThaiAllRow();


            }
        }

        private void JspostGetMember()
        {
            String seq_no = null;
            //  String emplid = null;
            try
            {
                emplid = DwMain.GetItemString(1, "emplid");
            }
            catch (Exception ex)
            {
                emplid = null;
            }

            if (emplid == "" || emplid == null)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบข้อมูลรหัสสมาชิกรายนี้ กรุณาเลือกรายการใหม่");
                JspostNewClear();
            }
            else
            {
                DwMain.Reset();
                DwMain.Retrieve(emplid);
                DwMain.SetItemString(1, "entry_id", state.SsUsername);
                DwMain.SetItemDate(1, "entry_date", state.SsWorkDate);
                DwMain.SetItemString(1, "branch_id", state.SsCoopId);
                tDwMain.Eng2ThaiAllRow();

                DwList.Reset();
                DwList.Retrieve(emplid);
                tDwList.Eng2ThaiAllRow();

                if (DwList.RowCount > 0)
                {

                    seq_no = DwList.GetItemString(1, "seq_no");
                    DwDetail.Reset();
                    DwDetail.Retrieve(emplid);
                    DwDetail.SetFilter("seq_no = '" + seq_no + "'");
                    DwDetail.Filter();
                    DwDetail.SetItemString(1, "seq_no", seq_no);
                    tDwDetail.Eng2ThaiAllRow();

                }
                else
                {
                    JspostAddRow();
                }
            }
        }
        protected Decimal getSalaryOld(string emplid)
        {
            Sta ta = new Sta(sqlca.ConnectionString);
            decimal salary = 0;
            try
            {


                String sql = @"select salary_new from HRNMLTRANSFERTOOFFICEDEA where emplid = '" + emplid + "' and seq_no in (select max(seq_no) from HRNMLTRANSFERTOOFFICEDEA where command_type = '2' and emplid = '" + emplid + "')";
                Sdt dt = ta.Query(sql);
                if (dt.Next())
                {
                    salary = dt.GetDecimal("salary_new");
                }
                else
                {
                    String sql1 = @"select salary from hrnmlemplfilemas where emplid = '" + emplid + "' ";
                    Sdt dt1 = ta.Query(sql1);
                    if (dt1.Next())
                    {
                        salary = dt1.GetDecimal("salary");
                    }
                }
            }

            catch (Exception ex)
            {
                String err = ex.ToString();

            }
            ta.Close();
            return salary;

        }
        private void JspostNewClear()
        {

            DwMain.Reset();
            DwMain.InsertRow(0);
            DwMain.SetItemString(1, "entry_id", state.SsUsername);
            DwMain.SetItemString(1, "branch_id", state.SsCoopId);
            DwMain.SetItemDate(1, "entry_date", state.SsWorkDate);
            tDwMain.Eng2ThaiAllRow();

            DwList.Reset();
            DwDetail.Reset();
        }

        //==================
        #region WebSheet Members

        public void InitJsPostBack()
        {
            postNewClear = WebUtil.JsPostBack(this, "postNewClear");
            postGetMember = WebUtil.JsPostBack(this, "postGetMember");
            postAddRow = WebUtil.JsPostBack(this, "postAddRow");
            postShowDetail = WebUtil.JsPostBack(this, "postShowDetail");
            postDeleteRow = WebUtil.JsPostBack(this, "postDeleteRow");
            postSearchGetMember = WebUtil.JsPostBack(this, "postSearchGetMember");
            postDifferTime = WebUtil.JsPostBack(this, "postDifferTime");

            //====================================
            tDwMain = new DwThDate(DwMain, this);
            tDwMain.Add("entry_date", "entry_tdate");
            tDwMain.Eng2ThaiAllRow();

            tDwDetail = new DwThDate(DwDetail, this);
            tDwDetail.Add("move_date", "move_tdate");
            tDwDetail.Eng2ThaiAllRow();

            tDwList = new DwThDate(DwList, this);
            tDwList.Add("move_date", "move_tdate");
            tDwList.Eng2ThaiAllRow();
        }

        public void WebSheetLoadBegin()
        {
            this.ConnectSQLCA();
            DwMain.SetTransaction(sqlca);
            DwList.SetTransaction(sqlca);
            DwDetail.SetTransaction(sqlca);

            if (!IsPostBack)
            {
                JspostNewClear();
            }
            else
            {
                this.RestoreContextDw(DwMain);
                this.RestoreContextDw(DwList);
                this.RestoreContextDw(DwDetail);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postNewClear")
            {
                JspostNewClear();
            }
            else if (eventArg == "postGetMember")
            {
                JspostGetMember();
            }
            else if (eventArg == "postAddRow")
            {
                JspostAddRow();
            }
            else if (eventArg == "postShowDetail")
            {
                JspostShowDetail();
            }
            else if (eventArg == "postDeleteRow")
            {
                JspostDeleteRow();
            }
            else if (eventArg == "postSearchGetMember")
            {
                JspostSearchGetMember();
            }
            else if (eventArg == "postDifferTime")
            {
                // JspostDifferTime();
            }

        }

        public void SaveWebSheet()
        {
            //Check values before save record
            //check new record or edit row
            String workdate_tdate1 = null;
            Decimal ot_amt = 0;
            try
            {
                String seq_new = null;
                // String empl_new = null;

                String empl_tmp = DwMain.GetItemString(1, "emplid").Trim();
                String seq_no = DwDetail.GetItemString(1, "seq_no").Trim();
                workdate_tdate1 = DwDetail.GetItemString(1, "move_tdate");
                if (empl_tmp == "Auto" || empl_tmp == null)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ยังไม่มีข้อมูลเจ้าหน้าที่ กรุณาเลือกเจ้าหน้าที่");
                }
                else if (workdate_tdate1 == null)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอกข้อมูลวันที่เข้างาน");
                }
                else
                {

                    if (DwList.RowCount > 0)
                    {
                        if (seq_no == "Auto" || seq_no == null)
                        {
                            String move_tdate, SEQ_NO, EMPLID;
                            Decimal salary_old, salary_new;

                            DateTime move_date = new DateTime();
                            DateTime ENTRY_DATE = new DateTime();

                            //ตัวแปร วันที่
                            String movedate_dd = null;
                            String movedate_mm = null;
                            String movedate_yyyy = null;

                            String entry_dd = null;
                            String entry_mm = null;
                            String entry_yyyy = null;

                            try
                            {
                                move_tdate = DwDetail.GetItemString(1, "move_tdate");
                            }
                            catch { move_tdate = null; }

                            try
                            {
                                SEQ_NO = DwDetail.GetItemString(1, "seq_no");
                            }
                            catch { SEQ_NO = null; }

                            try
                            {
                                EMPLID = DwMain.GetItemString(1, "emplid");
                            }
                            catch { EMPLID = null; }
                            try
                            {
                                salary_old = DwDetail.GetItemDecimal(1, "salary_old");
                            }
                            catch { salary_old = 0; }

                            try
                            {
                                salary_new = DwDetail.GetItemDecimal(1, "salary_new");
                            }
                            catch { salary_new = 0; }


                            try
                            {
                                move_date = DwDetail.GetItemDate(1, "move_date");
                                movedate_dd = move_date.Day.ToString();
                                movedate_mm = move_date.Month.ToString();
                                movedate_yyyy = move_date.Year.ToString();

                            }
                            catch { }

                            try
                            {
                                ENTRY_DATE = state.SsWorkDate;
                                entry_dd = ENTRY_DATE.Day.ToString();
                                entry_mm = ENTRY_DATE.Month.ToString();
                                entry_yyyy = ENTRY_DATE.Year.ToString();
                            }
                            catch { }



                            //================
                            if (move_tdate == null)
                            {
                                LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอกข้อมูลให้ครบ");
                            }
                            else
                            {
                                //JspostCheckTimeWork();
                                //new record

                                try
                                {
                                    Sta ta = new Sta(sqlca.ConnectionString);

                                    String seq_tmp = GetDocNo("HRTRANSFER");
                                    seq_tmp = WebUtil.Right(seq_tmp, 6);
                                    seq_new = "SU" + GetYear("HREMPLFILEMAS") + seq_tmp;
                                    DwDetail.SetItemString(1, "seq_no", seq_new);
                                    DwDetail.SetItemString(1, "emplid", EMPLID);

                                    String sql = @"INSERT INTO HRNMLTRANSFERTOOFFICEDEA (seq_no, emplid, move_date,salary_old, salary_new, entry_id, entry_date, branch_id,command_type)";
                                    sql += " VALUES ('" + seq_new + "', '" + EMPLID + "', to_date('" + movedate_dd + "/" + movedate_mm + "/" + movedate_yyyy + "' ,'dd/mm/yyyy') ,'" + salary_old + "' ,'" + salary_new + "' ,'" + state.SsUsername + "' ,  to_date('" + entry_dd + "/" + entry_mm + "/" + entry_yyyy + "' ,'dd/mm/yyyy') , '" + state.SsCoopId + "','2')";
                                    // String sql1 = "UPDATE HRNMLEMPLFILEMAS SET salary = '" + salary_new + "' WHERE emplid = '" + EMPLID + "' ";
                                    ta.Exe(sql);
                                    // ta.Exe(sql1);
                                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทีกข้อมูลเสร็จเรียบร้อยแล้ว");
                                    JspostGetMember();
                                    ta.Close();

                                }
                                catch (Exception ex)
                                {
                                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถบันทึกข้อมูลได้");
                                }
                            }
                        }
                        else
                        {
                            seq_new = seq_no; ;
                            DwDetail.UpdateData();
                            LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกการแก้ไขข้อมูลเสร็จเรียบร้อยแล้ว");
                            JspostGetMember();
                        }
                    }
                    else
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากดปุ่ม เพิ่มแถว");
                    }
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
            }
        }

        public void WebSheetLoadEnd()
        {
            if (DwMain.RowCount > 1)
            {
                DwMain.DeleteRow(DwMain.RowCount);
            }

            if (DwList.RowCount > 0)
            {
                SetDwMasterEnable(1);
            }
            else
            {
                SetDwMasterEnable(0);
            }

            DwMain.SaveDataCache();
            DwList.SaveDataCache();
            DwDetail.SaveDataCache();
        }

        #endregion
    }
}
