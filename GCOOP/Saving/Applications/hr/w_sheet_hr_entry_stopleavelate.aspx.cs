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
    public partial class w_sheet_hr_entry_stopleavelate : PageWebSheet, WebSheet
    {
        private String emplid;
        private DwThDate tDwMain;
        private DwThDate tDwDetail;
        private DwThDate tDwList;

        protected String postNewClear;
        protected String postGetMember;
        protected String postAddRow;
        protected String postShowDetail;
        protected String postDeleteRow;
        protected String postSearchGetMember;
        protected String postDifferDate;
        protected String postDifferTime;
        //==================
        // ตรวจสอบจำนวนวันที่และเวลา
        private void JspostDifferTime()
        {
            String t_end = DwDetail.GetItemString(1, "timeend");
            String t_start = DwDetail.GetItemString(1, "timestart");
            t_end = t_end.Replace(":", "");
            t_start = t_start.Replace(":", "");
            //|| t_end.Length < 6
            if (t_end.Length > 6)
            {
                t_end = t_end.Substring(0, 6);
            }
            else
            {
                int lth = t_end.Length;
                lth = 6 - lth;
                for (int i = 0; i < lth; i++)
                {
                    t_end = t_end + "0";
                }
            }

            if (t_start.Length > 6)
            {
                t_start = t_start.Substring(0, 6);
            }
            else
            {
                int lth = t_start.Length;
                lth = 6 - lth;
                for (int i = 0; i < lth; i++)
                {
                    t_start = t_start + "0";
                }
            }

            DwDetail.SetItemString(1, "timeend", t_end);
            DwDetail.SetItemString(1, "timestart", t_start);
            int HH1, MM1, SS1, HH2, MM2, SS2, tr_hh, tr_mm, tr_ss = 0;
            //differ Time
            //String t_start = DwDetail.GetItemString(1, "timestart");
          // String t_end = DwDetail.GetItemString(1, "timeend");
            String time_rent = null;
            String t_hh, t_mm, t_ss = null;
            HH1 = int.Parse(WebUtil.Left(t_start, 2));
            MM1 = int.Parse(WebUtil.Mid(t_start, 2, 2));
            SS1 = int.Parse(WebUtil.Mid(t_start, 4, 2));

            HH2 = int.Parse(WebUtil.Left(t_end, 2));
            MM2 = int.Parse(WebUtil.Mid(t_end, 2, 2));
            SS2 = int.Parse(WebUtil.Mid(t_end, 4, 2));


            // หาจำนวน HH MM SS คงเหลือ
            if (HH2 > HH1)
            {
                tr_hh = HH2 - HH1;
            }
            else
            {
                tr_hh = 0;
            }

            if (MM2 > MM1 || MM2 == MM1)
            {
                tr_mm = MM2 - MM1;

            }
            else
            {
                if ((MM2 + MM1) > 60)
                {
                    tr_hh = tr_hh - 1;
                    tr_mm = (MM2 + MM1) - 60;
                    tr_mm = 60 - tr_mm;
                }
                else
                {
                    if (MM2 > MM1 || MM2 == MM1)
                    {
                        tr_mm = MM2 - MM1;
                    }
                    else
                    {
                        tr_hh = tr_hh - 1;
                        tr_mm = MM1 - MM2;
                        tr_mm = 60 - tr_mm;
                    }
                }
            }


            if (SS2 > SS1)
            {
                tr_ss = SS2 - SS1;
            }
            else
            {
                tr_ss = 0;
            }

            if (Convert.ToString(tr_hh).Length < 2)
            {
                t_hh = "0" + Convert.ToString(tr_hh);
            }
            else
            {
                t_hh = Convert.ToString(tr_hh);
            }

            if (Convert.ToString(tr_mm).Length < 2)
            {
                t_mm = "0" + Convert.ToString(tr_mm);
            }
            else
            {
                t_mm = Convert.ToString(tr_mm);
            }

            if (Convert.ToString(tr_ss).Length < 2)
            {
                t_ss = "0" + Convert.ToString(tr_ss);
            }
            else
            {
                t_ss = Convert.ToString(tr_ss);
            }

            time_rent = Convert.ToString(t_hh) + Convert.ToString(t_mm) + Convert.ToString(t_ss);
            DwDetail.SetItemString(1, "timerent", time_rent);
            Hd_leavtotalhour.Value = time_rent;

            t_end = t_end.Substring(0, 2) + ":" + t_end.Substring(2, 2) + ":" + t_end.Substring(4, 2);
            DwDetail.SetItemString(1, "timeend", t_end);

            t_start = t_start.Substring(0, 2) + ":" + t_start.Substring(2, 2) + ":" + t_start.Substring(4, 2);
            DwDetail.SetItemString(1, "timestart", t_start);
        }

        private void JspostGetTimeWork()
        {
            String Wtime_Id = "01";
            String Time_start = null;
            String Time_end = null;
            Sta ta = new Sta(sqlca.ConnectionString);
            try
            {
                String sql = @"select  w_Ntime_In, w_Ntime_Out from hrucftimework where Wtime_Id = '" + Wtime_Id + "'";
                Sdt dt = ta.Query(sql);
                if (dt.Next())
                {
                    Time_start = dt.GetString("w_Ntime_In");
                    Time_end = dt.GetString("w_Ntime_Out");

                    DwDetail.SetItemString(1, "timestart", Time_start);
                    DwDetail.SetItemString(1, "timeend", Time_end);
                }
                else
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบข้อมูลเจ้าหน้าที่รายนี้ กรุณากรอกข้อมูลใหม่");
                    JspostNewClear();
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
            }
            ta.Close();
        }

        private void JspostDifferDate()
        {
            //Differ date
            DateTime startDate = DwDetail.GetItemDate(1, "leavfromdate");
            DateTime endDate = DwDetail.GetItemDate(1, "leavtodate");
            TimeSpan sp = endDate.Subtract(startDate);
            int totalDate = sp.Days + 1;
            DwDetail.SetItemDouble(1, "leavtotaldate", totalDate);
            Hd_totaldate.Value = Convert.ToString(totalDate);
        }


        private void JspostSearchGetMember()
        {
            String emplcode = null;
            String emplid = null;

            emplcode = DwMain.GetItemString(1, "emplcode");

            Sta ta = new Sta(sqlca.ConnectionString);
            try
            {
                String sql = @"select emplid from hrnmlemplfilemas where emplcode = '" + emplcode.Trim() + "'";
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
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
                //String err = ex.ToString();
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

        protected int getMaxstopleave(String leavtypeid, String emplid)
        {

            int max = 0;
            Sta ta = new Sta(sqlca.ConnectionString);
            try
            {
                String sql = @"  SELECT HRUCFPRIVILEGE.PRIVILEGE   FROM HRUCFPRIVILEGE where HRUCFPRIVILEGE.LEAVTYPEID = '" + leavtypeid + "' and  HRUCFPRIVILEGE.POSIWID = '" + emplid + "'   ";
                Sdt dt = ta.Query(sql);
                if (dt.Next())
                {
                    max = dt.GetInt32("PRIVILEGE");
                }

            }
            catch (Exception ex)
            {
                String err = ex.ToString();
            }

            ta.Close();
            return max;
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

                String sql = @"Delete FROM HRNMLEMPLLEAVDEA where seq_no = '" + seq_no + "'";
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
            DwUtil.RetrieveDataWindow(DwDetail, "hr_entry_stopleavelate.pbl", null, emplid);
            //DwDetail.Retrieve(emplid);
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
                DwDetail.SetItemDate(1, "leavfromdate", state.SsWorkDate);
                DwDetail.SetItemDate(1, "leavtodate", state.SsWorkDate);
                DwDetail.SetItemDate(1, "commit_date", state.SsWorkDate);
                tDwDetail.Eng2ThaiAllRow();

                DwDetail.SetItemString(1, "docutype", "1");
                JspostGetTimeWork();
                JspostDifferDate();
                JspostDifferTime();
            }
        }

        private void JspostGetMember()
        {
            String seq_no = null;
            //  String emplid = null;
            try
            {
                emplid = DwMain.GetItemString(1, "emplid").Trim();
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
                    // DwDetail.SetItemString(1, "seq_no", seq_no);
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
            Hd_totaldate.Value = null;
            Hd_leavtotalhour.Value = null;
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
            postDifferDate = WebUtil.JsPostBack(this, "postDifferDate");
            postDifferTime = WebUtil.JsPostBack(this, "postDifferTime");

            //====================================
            tDwMain = new DwThDate(DwMain, this);
            tDwMain.Add("entry_date", "entry_tdate");
            tDwMain.Eng2ThaiAllRow();

            tDwList = new DwThDate(DwList, this);
            tDwList.Add("leavtodate", "leavto_tdate");
            tDwList.Eng2ThaiAllRow();

            tDwDetail = new DwThDate(DwDetail, this);
            tDwDetail.Add("leavfromdate", "leavfrom_tdate");
            tDwDetail.Add("leavtodate", "leavto_tdate");
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
            else if (eventArg == "postDifferDate")
            {
                JspostDifferDate();
            }
            else if (eventArg == "postDifferTime")
            {
                JspostDifferTime();
            }
        }


        public void SaveWebSheet()
        {
            //Check values before save record
            //check new record or edit row

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
                            String SEQ_NO, TIMESTATUS, EMPLID, DOCUTYPE, APPVBY, LEAVTYPEID, TIMESTART, TIMEEND, TIMERENT, LEAVTELE, LEAVREMK1, LEAVREMK2, APPVSTATUS, COMMIT_ID, APPVREMK1, APPVREMK2 = null;
                            String leavfrom_tdate, leavto_tdate, commit_tdate = null;
                            Decimal LEAVTOTALDATE = 0;

                            DateTime LEAVFROMDATE = new DateTime();
                            DateTime LEAVTODATE = new DateTime();
                            DateTime COMMIT_DATE = new DateTime();
                            DateTime ENTRY_DATE = new DateTime();

                            //ตัวแปร วันที่
                            String leavfromdate_dd = null;
                            String leavfromdate_mm = null;
                            String leavfromdate_yyyy = null;

                            String leavtodate_dd = null;
                            String leavtodate_mm = null;
                            String leavtodate_yyyy = null;

                            String commitdate_dd = null;
                            String commitdate_mm = null;
                            String commitdate_yyyy = null;

                            String entry_dd = null;
                            String entry_mm = null;
                            String entry_yyyy = null;

                            // Get ค่าขึ้นมา check ค่าว่าง
                            try
                            {
                                TIMESTATUS = DwDetail.GetItemString(1, "timestatus");
                            }
                            catch { TIMESTATUS = null; }

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
                                DOCUTYPE = DwMain.GetItemString(1, "docutype");
                            }
                            catch { DOCUTYPE = null; }

                            try
                            {
                                APPVBY = DwDetail.GetItemString(1, "appvby");
                            }
                            catch { APPVBY = null; }

                            try
                            {
                                LEAVTYPEID = DwDetail.GetItemString(1, "leavtypeid");
                            }
                            catch { LEAVTYPEID = null; }

                            try
                            {
                                TIMESTART = DwDetail.GetItemString(1, "timestart");
                            }
                            catch { TIMESTART = null; }

                            try
                            {
                                TIMEEND = DwDetail.GetItemString(1, "timeend");
                            }
                            catch { TIMEEND = null; }

                            try
                            {
                                TIMERENT = DwDetail.GetItemString(1, "timerent");
                            }
                            catch { TIMERENT = null; }

                            try
                            {
                                LEAVTELE = DwDetail.GetItemString(1, "leavtele");
                            }
                            catch { LEAVTELE = null; }

                            try
                            {
                                LEAVREMK1 = DwDetail.GetItemString(1, "leavremk1");
                            }
                            catch { LEAVREMK1 = null; }

                            try
                            {
                                LEAVREMK2 = DwDetail.GetItemString(1, "leavremk2");
                            }
                            catch { LEAVREMK2 = null; }

                            try
                            {
                                APPVSTATUS = DwDetail.GetItemString(1, "appvstatus");
                            }
                            catch { APPVSTATUS = null; }

                            try
                            {
                                COMMIT_ID = DwDetail.GetItemString(1, "commit_id");
                            }
                            catch { COMMIT_ID = null; }

                            try
                            {
                                APPVREMK1 = DwDetail.GetItemString(1, "appvremk1");
                            }
                            catch { APPVREMK1 = null; }

                            try
                            {
                                APPVREMK2 = DwDetail.GetItemString(1, "appvremk2");
                            }
                            catch { APPVREMK2 = null; }

                            try
                            {
                                LEAVTOTALDATE = DwDetail.GetItemDecimal(1, "leavtotaldate");
                            }
                            catch { LEAVTOTALDATE = 0; }

                            try
                            {
                                leavfrom_tdate = DwDetail.GetItemString(1, "leavfrom_tdate");
                            }
                            catch { leavfrom_tdate = null; }

                            try
                            {
                                leavto_tdate = DwDetail.GetItemString(1, "leavto_tdate");
                            }
                            catch { leavto_tdate = null; }

                            try
                            {
                                commit_tdate = DwDetail.GetItemString(1, "commit_tdate");
                            }
                            catch { commit_tdate = null; }


                            try
                            {
                                LEAVFROMDATE = DwDetail.GetItemDate(1, "leavfromdate");
                                leavfromdate_dd = LEAVFROMDATE.Day.ToString();
                                leavfromdate_mm = LEAVFROMDATE.Month.ToString();
                                leavfromdate_yyyy = LEAVFROMDATE.Year.ToString();

                            }
                            catch { }

                            try
                            {
                                LEAVTODATE = DwDetail.GetItemDate(1, "leavtodate");
                                leavtodate_dd = LEAVTODATE.Day.ToString();
                                leavtodate_mm = LEAVTODATE.Month.ToString();
                                leavtodate_yyyy = LEAVTODATE.Year.ToString();

                            }
                            catch { }

                            try
                            {
                                COMMIT_DATE = DwDetail.GetItemDate(1, "commit_date");
                                commitdate_dd = COMMIT_DATE.Day.ToString();
                                commitdate_mm = COMMIT_DATE.Month.ToString();
                                commitdate_yyyy = COMMIT_DATE.Year.ToString();

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

                            int maxstop = getMaxstopleave(LEAVTYPEID, EMPLID);
                            //================
                            if (leavfrom_tdate == null)
                            {
                                LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอกข้อมูลวันที่เริ่มต้น");
                            }
                            else if (leavto_tdate == null)
                            {
                                LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอกข้อมูลวันที่สิ้นสุด");
                            }
                            else if (commit_tdate == null)
                            {
                                LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอกข้อมูลวันที่อนุมัติ");
                            }
                            else if (LEAVTOTALDATE > maxstop)
                            {
                                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถลาเกินกำหนด ในแต่ละปี");
                            }
                            else
                            {
                                //new record
                                try
                                {
                                    Sta ta = new Sta(sqlca.ConnectionString);

                                    String seq_tmp = GetDocNo("HRLEAV");
                                    seq_tmp = WebUtil.Right(seq_tmp, 6);
                                    seq_new = "LV" + GetYear("HREMPLFILEMAS") + seq_tmp;
                                    DwDetail.SetItemString(1, "seq_no", seq_new);
                                    DwDetail.SetItemString(1, "emplid", EMPLID);

                                    //  DifferTime();

                                    String sql = @"INSERT INTO HRNMLEMPLLEAVDEA (seq_no, emplid, docutype, appvby, leavtypeid, leavfromdate, leavtodate, timestart, timeend, timerent, leavtotaldate, leavtele, leavremk1, leavremk2, appvstatus, commit_id, appvremk1, appvremk2,commit_date, entry_id, entry_date, branch_id,timestatus)";
                                    sql += " VALUES ('" + seq_new + "', '" + EMPLID + "', '" + "1" + "' , '" + APPVBY + "', '" + LEAVTYPEID + "' , to_date('" + leavfromdate_dd + "/" + leavfromdate_mm + "/" + leavfromdate_yyyy + "' ,'dd/mm/yyyy'), to_date('" + leavtodate_dd + "/" + leavtodate_mm + "/" + leavtodate_yyyy + "' ,'dd/mm/yyyy') , '" + TIMESTART + "' , '" + TIMEEND + "', '" + TIMERENT + "','" + LEAVTOTALDATE + "' ,'" + LEAVTELE + "','" + LEAVREMK1 + "' ,'" + LEAVREMK2 + "' ,'" + APPVSTATUS + "', '" + COMMIT_ID + "','" + APPVREMK1 + "','" + APPVREMK2 + "', to_date('" + commitdate_dd + "/" + commitdate_mm + "/" + commitdate_yyyy + "' ,'dd/mm/yyyy'),'" + state.SsUsername + "' ,  to_date('" + entry_dd + "/" + entry_mm + "/" + entry_yyyy + "' ,'dd/mm/yyyy') , '" + state.SsCoopId + "','" + TIMESTATUS + "')";
                                    ta.Exe(sql);
                                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทีกข้อมูลเสร็จเรียบร้อยแล้ว");
                                    JspostGetMember();
                                    DwDetail.SetItemString(1, "timerent", Hd_leavtotalhour.Value);
                                    DwDetail.SetItemDecimal(1, "leavtotaldate", Convert.ToDecimal(Hd_totaldate.Value));
                                    int totalstop = maxstop - Convert.ToInt16(LEAVTOTALDATE);
                                    String sqlupdate = "update HRUCFPRIVILEGE set PRIVILEGE = '" + totalstop + "' where LEAVTYPEID = '" + LEAVTYPEID + "' and POSIWID = '"+EMPLID+"'";
                                    ta.Exe(sqlupdate);
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
