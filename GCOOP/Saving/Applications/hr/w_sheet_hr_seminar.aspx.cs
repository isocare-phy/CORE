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
using CoreSavingLibrary.WcfNCommon;

namespace Saving.Applications.hr
{
    public partial class w_sheet_hr_seminar : PageWebSheet, WebSheet
    {
        private String emplid;
        private DwThDate tDwMain;
        private DwThDate tDwDetail;
        

        protected String postNewClear;
        protected String postGetMember;
        protected String postAddRow;
        protected String postShowDetail;
        protected String postDeleteRow;
        protected String postSearchGetMember;

        //==================

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
            String maxdoc = comm.GetNewDocNo(state.SsWsPass, doc_code);
            return maxdoc;
        }

        private void JspostDeleteRow()
        {
            Sta ta = new Sta(sqlca.ConnectionString);
            try
            {
                emplid = DwMain.GetItemString(1, "emplid").Trim();
                String seq_no = DwDetail.GetItemString(1, "seq_no").Trim();

                String sql = @"Delete from hrnmlemplserminar where seq_no = '" + seq_no + "'";
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
                DwList.InsertRow(0);
                DwDetail.Reset();
                DwDetail.InsertRow(0);
                DwDetail.SetItemDate(1, "date_seminars", state.SsWorkDate);
                DwDetail.SetItemDate(1, "date_seminerf", state.SsWorkDate);
                DwDetail.SetItemDate(1, "commit_date", state.SsWorkDate);
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

            //====================================
            tDwMain = new DwThDate(DwMain, this);
            tDwMain.Add("entry_date", "entry_tdate");
            tDwMain.Eng2ThaiAllRow();

            tDwDetail = new DwThDate(DwDetail, this);
            tDwDetail.Add("date_seminars", "seminars_tdate");
            tDwDetail.Add("date_seminerf", "seminarf_tdate");
            tDwDetail.Add("commit_date", "commit_tdate");
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
            tDwDetail.Eng2ThaiAllRow();
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
                        if (seq_no == "Auto" || seq_no == null)
                        { 
                     
                            String seminars_tdate,seminarf_tdate,SEQ_NO, EMPLID, TYPESEMINAR, SUBJECTSEM, INSTITUTE, LOCATESEM, EXPERTISE_ASSENT, APPVSTATUS,COMMIT_ID,EVALUTIONREMARK,EVALUATE,REMARK,SUMMARY,SEMNAME   = null;
                            Decimal TIMEVENT, SEMIPAY = 0;

                            DateTime DATE_SEMINARS = new DateTime();
                            DateTime DATE_SEMINERF = new DateTime();
                            DateTime COMMIT_DATE = new DateTime ();
                            DateTime ENTRY_DATE = new DateTime();


                            //ตัวแปร วันที่
                            String seminarSdate_dd = null;
                            String seminarSdate_mm = null;
                            String seminarSdate_yyyy = null;

                            String seminarF_dd = null;
                            String seminarF_mm = null;
                            String seminarF_yyyy = null;

                            String commit_dd = null;
                            String commit_mm = null;
                            String commit_yyyy = null;

                            String entry_dd = null;
                            String entry_mm = null;
                            String entry_yyyy = null;

                            
                            try
                            {
                                seminars_tdate = DwDetail.GetItemString(1, "seminars_tdate");
                            }
                            catch { seminars_tdate = null; }

                            try
                            {
                                seminarf_tdate = DwDetail.GetItemString(1, "seminarf_tdate");
                            }
                            catch { seminarf_tdate = null; }

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
                                TYPESEMINAR = DwDetail.GetItemString(1, "typeseminar");
                            }
                            catch { TYPESEMINAR = null; }

                            try
                            {
                                SUBJECTSEM = DwDetail.GetItemString(1, "subjectsem");
                            }
                            catch { SUBJECTSEM = null; }

                            try
                            {
                                INSTITUTE = DwDetail.GetItemString(1, "institute");
                            }
                            catch { INSTITUTE = null; }

                            try
                            {
                                LOCATESEM  = DwDetail.GetItemString(1, "locatesem");
                            }
                            catch { LOCATESEM = null; }

                            try
                            {
                                EXPERTISE_ASSENT  = DwDetail.GetItemString(1, "expertise_assent");
                            }
                            catch { EXPERTISE_ASSENT = "0"; }

                            try 
                            {
                                TIMEVENT = DwDetail.GetItemDecimal(1, "TIMEVENT");
                            }
                            catch { TIMEVENT = 0; }
                            try
                            {
                                APPVSTATUS = DwDetail.GetItemString(1, "appvstatus");
                            }
                            catch { APPVSTATUS = "0"; }

                            try
                            {
                                COMMIT_ID  = DwDetail.GetItemString(1, "commit_id");
                            }
                            catch { COMMIT_ID = null; }

                            try
                            {
                                SEMIPAY  = DwDetail.GetItemDecimal(1, "semipay");
                            }
                            catch { SEMIPAY = 0; }

                            try
                            {
                                EVALUTIONREMARK = DwDetail.GetItemString(1, "evalutionremark");
                            }
                            catch { EVALUTIONREMARK = null; }


                            try
                            {
                                EVALUATE   = DwDetail.GetItemString(1, "evaluate");
                            }
                            catch { EVALUATE = null; }

                            try
                            {
                                REMARK  = DwDetail.GetItemString(1, "remark");
                            }
                            catch { REMARK = null; }


                            try
                            {
                                SUMMARY = DwDetail.GetItemString(1, "summary");
                            }
                            catch { SUMMARY = null; }

                            try
                            {
                                SEMNAME  = DwDetail.GetItemString(1, "semname");
                            }
                            catch { SEMNAME = null; }

                            try
                            {
                                DATE_SEMINARS = DateTime.ParseExact(DwDetail.GetItemString(1, "seminars_tdate"), "ddMMyyyy", WebUtil.TH);
                                seminarSdate_dd   = DATE_SEMINARS.Day.ToString();
                                seminarSdate_mm = DATE_SEMINARS.Month.ToString();
                                seminarSdate_yyyy = DATE_SEMINARS.Year.ToString();

                            }
                            catch { }

                            try
                            {
                                DATE_SEMINERF = DateTime.ParseExact(DwDetail.GetItemString(1, "seminarf_tdate"), "ddMMyyyy", WebUtil.TH);
                                seminarF_dd = DATE_SEMINERF.Day.ToString();
                                seminarF_mm  = DATE_SEMINERF.Month.ToString();
                                seminarF_yyyy = DATE_SEMINERF.Year.ToString();
                            }
                            catch { }

                            try
                            {
                                COMMIT_DATE = DateTime.ParseExact(DwDetail.GetItemString(1, "commit_tdate"), "ddMMyyyy", WebUtil.TH);
                                commit_dd  = COMMIT_DATE.Day.ToString();
                                commit_mm = COMMIT_DATE.Month.ToString();
                                commit_yyyy = COMMIT_DATE.Year.ToString();
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
                            if (TYPESEMINAR == null)
                            {
                                LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอกข้อมูลประเภทกิจกรรม");
                            }
                            else if(SUBJECTSEM == null)
                            {
                                 LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอกข้อมูลชื่อโครงการ");
                            }
                            else if(SEMNAME == null)
                            {
                                 LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอกข้อมูลหลักสูตร");
                            }
                            else if(LOCATESEM == null)
                            {
                                 LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอกข้อมูลสถานที่");
                            }
                            else if(seminars_tdate == null)
                            {
                                 LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอกข้อมูลวันที่เริ่ม");
                            }
                            else if(seminarf_tdate == null)
                            {
                                 LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอกข้อมูลวันที่สิ้นสุด");
                            }
                            else
                            {

                                //new record
                                try
                                {
                                    Sta ta = new Sta(sqlca.ConnectionString);

                                    String seq_tmp = GetDocNo("HRSERMINAR");
                                    seq_tmp = WebUtil.Right(seq_tmp, 6);
                                    seq_new = "SE" + GetYear("HREMPLFILEMAS") + seq_tmp;
                                    DwDetail.SetItemString(1, "seq_no", seq_new);
                                    DwDetail.SetItemString(1, "emplid", EMPLID);
                                    String sql = @"INSERT INTO HRNMLEMPLSERMINAR (seq_no, emplid, typeseminar, subjectsem, date_seminars, date_seminerf, institute, locatesem, expertise_assent, timevent, appvstatus, commit_id, commit_date, semipay, evalutionremark, evaluate, remark, entry_id, entry_date, branch_id, summary, semname  )";
                                    sql += " VALUES ('" + seq_new + "', '" + EMPLID + "','" + TYPESEMINAR + "' ,'" + SUBJECTSEM + "',to_date('" + seminarSdate_dd + "/" + seminarSdate_mm + "/" + seminarSdate_yyyy + "' ,'dd/mm/yyyy') , to_date('" + seminarF_dd + "/" + seminarF_mm + "/" + seminarF_yyyy + "' ,'dd/mm/yyyy'), '" + INSTITUTE + "' , '" + LOCATESEM + "', '" + EXPERTISE_ASSENT + "', '" + TIMEVENT + "' ,'" + APPVSTATUS + "' ,'" + COMMIT_ID + "' ,to_date('" + commit_dd + "/" + commit_mm + "/" + commit_yyyy + "' ,'dd/mm/yyyy') ,'" + SEMIPAY + "' ,'" + EVALUTIONREMARK  +"' ,'" + EVALUATE +"' ,'" + REMARK +"' ,'" + state.SsUsername +"' , to_date('" + entry_dd + "/" + entry_mm + "/" + entry_yyyy + "' ,'dd/mm/yyyy') , '" + state.SsCoopId + "', '" + SUMMARY + "', '" + SEMNAME + "')";
                                    ta.Exe(sql);
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
                            DwUtil.UpdateDataWindow(DwDetail, "hr_seminar.pbl", "HRNMLEMPLSERMINAR");
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
