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
using Sybase.DataWindow;
using DataLibrary;
using CoreSavingLibrary.WcfNCommon;


namespace Saving.Applications.hr
{
    public partial class w_sheet_hr_transfertoofficedea : PageWebSheet, WebSheet
    {
        private String emplid;
        private DwThDate tDwMain;
        private DwThDate tDwDetail;
        private DwThDate tDwList;
        private String pbl = "hr_transfertoofficedea.pbl";

        protected String postNewClear;
        protected String postGetMember;
        protected String postAddRow;
        protected String postShowDetail;
        protected String postDeleteRow;
        protected String postSearchGetMember;
        protected DataStore DStore;


        //==================

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
            postGetMember = WebUtil.JsPostBack(this, "postGetMember");
            postNewClear = WebUtil.JsPostBack(this, "postNewClear");
            postShowDetail = WebUtil.JsPostBack(this, "postShowDetail");
            postAddRow = WebUtil.JsPostBack(this, "postAddRow");
            postDeleteRow = WebUtil.JsPostBack(this, "postDeleteRow");

            //====================================
            tDwMain = new DwThDate(DwMain, this);
            tDwMain.Add("entry_date", "entry_tdate");
            tDwMain.Eng2ThaiAllRow();

            tDwList = new DwThDate(DwList, this);
            tDwList.Add("move_date", "move_tdate");
            tDwList.Eng2ThaiAllRow();

            tDwDetail = new DwThDate(DwDetail, this);
            tDwDetail.Add("move_date", "move_tdate");
            tDwDetail.Add("start_date", "start_tdate");
            tDwDetail.Eng2ThaiAllRow();
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
            else if (eventArg == "postShowDetail")
            {
                JspostShowDetail();
            }
            else if (eventArg == "postAddRow")
            {
                JspostAddRow();
            }
            else if (eventArg == "postDeleteRow")
            {
                JspostDeleteRow();
            }
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

            int row = Convert.ToInt32(HfRow.Value);
            if (DwList.GetItemString(row, "seq_no") != "Auto")
            {
                seq_no = DwDetail.GetItemString(1, "seq_no");
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

        private void JspostAddRow()
        {
            String emplid = null;
            String seq_no = null;
            Decimal salary_new;
            String position_new, local_new,step_new;
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
                if (DwList.RowCount > 0)
                {
                    if (DwList.GetItemString(DwList.RowCount, "seq_no") != "Auto")
                    {
                        DwList.InsertRow(0);
                    }
                }
                else { DwList.InsertRow(0); }
                DwDetail.Reset();
                DwDetail.InsertRow(0);
                DwDetail.SetItemDate(1, "move_date", state.SsWorkDate);
                DwDetail.SetItemDate(1, "start_date", state.SsWorkDate);
                tDwDetail.Eng2ThaiAllRow();

                try
                {
                    seq_no = DwList.GetItemString(1, "seq_no");
                }
                catch { seq_no = ""; }
                DStore = new DataStore();
                DStore.LibraryList = WebUtil.PhysicalPath + "//Saving//DataWindow//hr//hr_transfertoofficedea.pbl";
                //DStore.LibraryList = "C:/GCOOP_ALL/CAT/GCOOP/Saving/DataWindow/hr/hr_transfertoofficedea.pbl";
                DStore.DataWindowObject = "dw_hr_get_detail";
                DStore.SetTransaction(sqlca);
                DStore.Retrieve(emplid);
                DStore.SetFilter("seq_no = '" + seq_no + "'");
                DStore.Filter();

                try
                {
                    position_new = DStore.GetItemString(1, "position_new");
                }
                catch { position_new = null; }

                try
                {

                    local_new = DStore.GetItemString(1, "local_new");
                }
                catch { local_new = null; }
                try
                {

                    step_new = DStore.GetItemString(1, "step_new");
                }
                catch { step_new = null; }

                try
                {
                    salary_new = DStore.GetItemDecimal(1, "salary_new");
                }
                catch
                {
                    salary_new = 0;
                }
                DwDetail.SetItemDecimal(1, "salary_old", salary_new);
                DwDetail.SetItemString(1, "position_old", position_new);
                DwDetail.SetItemString(1, "local_old", local_new);
                DwDetail.SetItemString(1, "step_old", step_new);
            }
        }
        public void SaveWebSheet()
        {
           try
            {
                String seq_new = null;
                String empl_tmp = DwMain.GetItemString(1, "emplid").Trim();
                String seq_no = DwDetail.GetItemString(1, "seq_no").Trim();

                if (empl_tmp == "Auto" || empl_tmp == null)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ยังไม่มีข้อมูลเจ้าหน้าที่ กรุณาเลือกเจ้าหน้าที่");
                }
                else
                {
                    if (DwList.RowCount > 0)
                    {
                        if (seq_no == "Auto")
                        {
                            String move_tdate, start_tdate, SEQ_NO, EMPLID, POSITION_OLD, POSITION_NEW, LOCAL_OLD, LOCAL_NEW, STEP_NEW, STEP_OLD, level_old, level_new, account_old, account_new, command_at = null;
                            Decimal SALARY_OLD, SALARY_NEW = 0;

                            DateTime MOVE_DATE = new DateTime();
                            DateTime START_DATE = new DateTime();
                            DateTime ENTRY_DATE = new DateTime();


                            //ตัวแปร วันที่
                            String movedate_dd = null;
                            String movedate_mm = null;
                            String movedate_yyyy = null;

                            String start_dd = null;
                            String start_mm = null;
                            String start_yyyy = null;

                            String entry_dd = null;
                            String entry_mm = null;
                            String entry_yyyy = null;

                            // Get ค่าขึ้นมา check ค่าว่าง
                            try
                            {
                                move_tdate = DwDetail.GetItemString(1, "move_tdate");
                            }
                            catch { move_tdate = null; }

                            try
                            {
                                start_tdate = DwDetail.GetItemString(1, "start_tdate");
                            }
                            catch { start_tdate = null; }

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
                                POSITION_OLD = DwDetail.GetItemString(1, "position_old");
                            }
                            catch { POSITION_OLD = null; }

                            try
                            {
                                POSITION_NEW = DwDetail.GetItemString(1, "position_new");
                            }
                            catch { POSITION_NEW = null; }

                            try
                            {
                                LOCAL_OLD = DwDetail.GetItemString(1, "local_old");
                            }
                            catch { LOCAL_OLD = null; }

                            try
                            {
                                LOCAL_NEW = DwDetail.GetItemString(1, "local_new");
                            }
                            catch { LOCAL_NEW = null; }

                            try
                            {
                                STEP_OLD = DwDetail.GetItemString(1, "step_old");
                            }
                            catch { STEP_OLD = null; }

                            try
                            {
                                STEP_NEW = DwDetail.GetItemString(1, "step_new");
                            }
                            catch { STEP_NEW = null; }

                            try
                            {
                                SALARY_OLD = DwDetail.GetItemDecimal(1, "salary_old");
                            }
                            catch { SALARY_OLD = 0; }

                            try
                            {
                                SALARY_NEW = DwDetail.GetItemDecimal(1, "salary_new");
                            }
                            catch { SALARY_NEW = 0; }


                            try
                            {
                                MOVE_DATE = DwDetail.GetItemDate(1, "move_date");
                                movedate_dd = MOVE_DATE.Day.ToString();
                                movedate_mm = MOVE_DATE.Month.ToString();
                                movedate_yyyy = MOVE_DATE.Year.ToString();

                            }
                            catch { }

                            try
                            {
                                START_DATE = DwDetail.GetItemDate(1, "start_date");
                                start_dd = START_DATE.Day.ToString();
                                start_mm = START_DATE.Month.ToString();
                                start_yyyy = START_DATE.Year.ToString();

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
                            try
                            {
                                command_at = DwDetail.GetItemString(1, "command_at");
                            }
                            catch { command_at = null; }

                            //================
                            if (move_tdate == null)
                            {
                                LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอกข้อมูลวันที่มีผล");
                            }
                            else if (start_tdate == null)
                            {
                                LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอกข้อมูลวันที่มาเริ่มงาน");
                            }
                            else if (LOCAL_OLD == null)
                            {
                                LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอกข้อมูลสายงานเดิม");
                            }
                            else if (LOCAL_NEW == null)
                            {
                                LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอกข้อมูลสายงานใหม่");
                            }
                           
                          
                            else if (POSITION_OLD == null)
                            {
                                LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอกข้อมูลตำแหน่งเดิม");
                            }
                            else if (POSITION_NEW == null)
                            {
                                LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอกข้อมูลตำแหน่งใหม่");
                            }

                            else
                            {

                                //new record
                                try
                                {
                                    Sta ta = new Sta(sqlca.ConnectionString);

                                    String seq_tmp = GetDocNo("HRTRANSFER");
                                    seq_tmp = WebUtil.Right(seq_tmp, 6);
                                    seq_new = "TF" + GetYear("HREMPLFILEMAS") + seq_tmp;
                                    DwDetail.SetItemString(1, "seq_no", seq_new);
                                    DwDetail.SetItemString(1, "emplid", EMPLID);

                                    String sql = @"INSERT INTO HRNMLTRANSFERTOOFFICEDEA (seq_no, emplid, move_date, start_date, position_old, position_new, local_old, local_new, salary_old, salary_new, entry_id, entry_date, branch_id, step_new, step_old,level_old, level_new, account_old, account_new, command_at,command_type)";
                                    sql += " VALUES ('" + seq_new + "', '" + EMPLID + "', to_date('" + movedate_dd + "/" + movedate_mm + "/" + movedate_yyyy + "' ,'dd/mm/yyyy') , to_date('" + start_dd + "/" + start_mm + "/" + start_yyyy + "' ,'dd/mm/yyyy'), '" + POSITION_OLD + "' , '" + POSITION_NEW + "', '"+LOCAL_OLD+"', '"+LOCAL_NEW+"' ,'" + SALARY_OLD + "' ,'" + SALARY_NEW + "' ,'" + state.SsUsername + "' ,  to_date('" + entry_dd + "/" + entry_mm + "/" + entry_yyyy + "' ,'dd/mm/yyyy') , '" + state.SsCoopId + "', '"+STEP_NEW+"', '"+STEP_OLD+"','','','', '', '" + command_at + "','1')";
                                    String sql1 = "UPDATE HRNMLEMPLFILEMAS SET postid = '" + POSITION_NEW + "', salary = '" + SALARY_NEW + "',sideid = '"+LOCAL_NEW+"',deptid = '"+STEP_NEW+"' WHERE emplid = '" + EMPLID + "' ";
                                    ta.Exe(sql);
                                    ta.Exe(sql1);
                                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทีกข้อมูลเสร็จเรียบร้อยแล้ว");
                                    JspostGetMember();
                                    ta.Close();
                                }
                                catch (Exception ex)
                                {
                                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถบันทึกข้อมูลได้" + ex.Message);
                                }
                            }
                        }
                        else
                        {
                            seq_new = seq_no;
                            DwDetail.SetTransaction(sqlca);
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

        protected String GetDocNo(String doc_code)
        {
            CommonClient comm = wcf.Common;
            String maxdoc = comm.GetNewDocNo(state.SsWsPass, doc_code);
            return maxdoc;
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

        public void WebSheetLoadEnd()
        {
            if (DwMain.RowCount > 1)
            {
                DwMain.DeleteRow(DwMain.RowCount);
            }

            DwMain.SaveDataCache();
            DwList.SaveDataCache();
            DwDetail.SaveDataCache();
        }

        #endregion

        private void JspostGetMember()
        {
            String seq_no = null;
            String emplid = null;
            Decimal salary;
            String postid, deptid, sideid;

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
                try
                {
                    DStore = new DataStore();
                    DStore.LibraryList = "C:/GCOOP_ALL/CEN/GCOOP/Saving/DataWindow/hr/hr_transfertoofficedea.pbl";
                    DStore.DataWindowObject = "dw_hr_get_master";
                    DStore.SetTransaction(sqlca);
                    DStore.Retrieve(emplid);

                    try
                    {
                        salary = DStore.GetItemDecimal(1, "salary");
                    }
                    catch { salary = 0; }

                    try
                    {
                        postid = DStore.GetItemString(1, "postid");
                    }
                    catch { postid = null; }

                    try
                    {

                        deptid = DStore.GetItemString(1, "deptid");
                    }
                    catch { deptid = null; }

                    try
                    {

                        sideid = DStore.GetItemString(1, "sideid");
                    }
                    catch { sideid = null; }

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
                        DwDetail.SetItemDecimal(1, "salary_old", salary);
                        DwDetail.SetItemString(1, "position_old", postid);
                        DwDetail.SetItemString(1, "step_old", deptid);
                        DwDetail.SetItemString(1, "local_old", sideid);
                    }
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
                }
            }
        }
    }
}
