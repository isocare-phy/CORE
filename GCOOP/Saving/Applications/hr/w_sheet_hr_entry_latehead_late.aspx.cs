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
    public partial class w_sheet_hr_entry_latehead_late : PageWebSheet, WebSheet
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
        protected String postDifferTime;
        //==================
        private void JspostDifferTime()
        {
            int HH1, MM1, SS1, HH2, MM2, SS2, tr_hh, tr_mm, tr_ss = 0;
            //differ Time
            String t_start = DwDetail.GetItemString(1, "timestart");
            String t_end = DwDetail.GetItemString(1, "timeend");
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
            Hd_timerent.Value = time_rent;


            if (tr_hh == 0 && tr_mm == 0 && tr_ss == 0)
            {
                DwDetail.SetItemDecimal(1, "leavtotaldate", 0);
                Hd_totaldate.Value = "0";
            }
            else
            {
                DwDetail.SetItemDecimal(1, "leavtotaldate", 1);
                Hd_totaldate.Value = "1";
            }
        }

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

                String sql = @"Delete from hrnmlemplleavdea where seq_no = '" + seq_no + "'";
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

            if (emplid == null || emplid == "<Auto>")
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
                tDwDetail.Eng2ThaiAllRow();

                String sql = @"select w_ntime_late from hrucftimework";
                sql = WebUtil.SQLFormat(sql);
                Sdt result = WebUtil.QuerySdt(sql);

                while (result.Next())
                {
                    DwDetail.SetItemString(1, "timestart", result.GetString("w_ntime_late"));
                }

                //DwDetail.SetItemString(1, "timestart", "083100");
                Hd_timerent.Value = "";
                Hd_totaldate.Value = "";

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

            Hd_timerent.Value = "";
            Hd_totaldate.Value = "";
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

            tDwList = new DwThDate(DwList, this);
            tDwList.Add("leavfromdate", "leavfrom_tdate");
            tDwList.Eng2ThaiAllRow();

            tDwDetail = new DwThDate(DwDetail, this);
            tDwDetail.Add("leavfromdate", "leavfrom_tdate");
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
            else if (eventArg == "postDifferTime")
            {
                String t_end = DwDetail.GetItemString(1, "timeend");
                t_end = t_end.Replace(":", "");
                //|| t_end.Length < 6
                if (t_end.Length > 6)
                {
                    t_end = t_end.Substring(0, 6);
                }
                else
                {
                    int lth = t_end.Length;
                    lth = 6 - lth;
                    for (int i = 0; i < lth; i++) {
                        t_end = t_end + "0";
                    }
                }

                DwDetail.SetItemString(1, "timeend", t_end);

                JspostDifferTime();

                t_end = t_end.Substring(0, 2) + ":" + t_end.Substring(2, 2) + ":" + t_end.Substring(4, 2);
                DwDetail.SetItemString(1, "timeend", t_end);
            }

        }

        public void SaveWebSheet()
        {
            try
            {
                String seq_new = null;
                String empl_tmp = DwMain.GetItemString(1, "emplid").Trim();
                String seq_no = DwDetail.GetItemString(1, "seq_no").Trim();

                if (empl_tmp == "<Auto>" || empl_tmp == null)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ยังไม่มีข้อมูลเจ้าหน้าที่ กรุณาเลือกเจ้าหน้าที่");
                }
                else
                {
                    if (DwList.RowCount > 0)
                    {
                        if (seq_no == "Auto" || seq_no == null)
                        {
                            String SEQ_NO, EMPLID, DOCUTYPE, TIMESTART, TIMEEND, LEAVREMK1, LEAVREMK2, TIMERENT, leavfrom_tdate = null;

                            Decimal LEAVTOTALDATE = 0;
                            DateTime LEAVFROMDATE = new DateTime();
                            DateTime ENTRY_DATE = new DateTime();

                            //ตัวแปร วันที่
                            String leavfromdate_dd = null;
                            String leavfromdate_mm = null;
                            String leavfromdate_yyyy = null;

                            String entry_dd = null;
                            String entry_mm = null;
                            String entry_yyyy = null;

                            // Get ค่าขึ้นมา check ค่าว่าง
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
                                leavfrom_tdate = DwDetail.GetItemString(1, "leavfrom_tdate");
                            }
                            catch { leavfrom_tdate = null; }

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
                                LEAVTOTALDATE = DwDetail.GetItemDecimal(1, "leavtotaldate");
                            }
                            catch { LEAVTOTALDATE = 1; }

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
                                LEAVFROMDATE = DwDetail.GetItemDate(1, "leavfromdate");
                                leavfromdate_dd = LEAVFROMDATE.Day.ToString();
                                leavfromdate_mm = LEAVFROMDATE.Month.ToString();
                                leavfromdate_yyyy = LEAVFROMDATE.Year.ToString();

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
                            if (TIMESTART == null)
                            {
                                LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอกข้อมูลเวลาที่สาย");
                            }
                            else if (TIMEEND == null)
                            {
                                LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอกข้อมูลเวลาเข้างาน");
                            }
                            else if (leavfrom_tdate == null)
                            {
                                LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอกข้อมูลวันที่มาสาย");
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

                                    String sql = @"INSERT INTO HRNMLEMPLLEAVDEA (seq_no, emplid, docutype, leavfromdate, timestart, timeend, timerent, leavtotaldate,leavremk1, leavremk2, entry_id, entry_date, branch_id,leavtypeid)";
                                    sql += " VALUES ('" + seq_new + "', '" + EMPLID + "', '" + "2" + "' , to_date('" + leavfromdate_dd + "/" + leavfromdate_mm + "/" + leavfromdate_yyyy + "' ,'dd/mm/yyyy'), '" + TIMESTART + "' , '" + TIMEEND + "', '" + TIMERENT + "','" + LEAVTOTALDATE + "' ,'" + LEAVREMK1 + "' ,'" + LEAVREMK2 + "' ,'" + state.SsUsername + "' ,  to_date('" + entry_dd + "/" + entry_mm + "/" + entry_yyyy + "' ,'dd/mm/yyyy') , '" + state.SsCoopId + "','008')";
                                    ta.Exe(sql);
                                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทีกข้อมูลเสร็จเรียบร้อยแล้ว");
                                    JspostGetMember();
                                    DwDetail.SetItemString(1, "timerent", Hd_timerent.Value);
                                    DwDetail.SetItemDecimal(1, "leavtotaldate", Convert.ToDecimal(Hd_totaldate.Value));
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
