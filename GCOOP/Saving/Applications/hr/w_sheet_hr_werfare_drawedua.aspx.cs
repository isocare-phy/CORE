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
    public partial class w_sheet_hr_werfare_drawedua : PageWebSheet, WebSheet
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
                    LtServerMessage.Text = WebUtil.ErrorMessage("กรุณาเพิ่มข้อมูลเจ้าหน้าที่รหัส :" + emplcode + " ที่หน้าจอข้อมูลเจ้าหน้าที่ก่อน");
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
            int RowAll = DwDetail.RowCount;
            String seq_no = null;
            seq_no = DwDetail.GetItemString(RowAll, "seq_no");
            if (seq_no == "" || seq_no == "Auto")
            {
                DwDetail.DeleteRow(RowAll);
                DwList.DeleteRow(RowAll);
            }
            else
            {
                Sta ta = new Sta(sqlca.ConnectionString);
                try
                {
                    emplid = DwMain.GetItemString(1, "emplid").Trim();
                    //String seq_no = DwDetail.GetItemString(1, "seq_no").Trim();

                    String sql = @"Delete FROM HRNMLDRAWEDUADEA where seq_no = '" + seq_no + "'";
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
            }
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
                DwDetail.SetItemDate(1, "indocudate", state.SsWorkDate);
                DwDetail.SetItemDate(1, "appvdate", state.SsWorkDate);
                DwDetail.SetItemDate(1, "bookdate", state.SsWorkDate);
                DwDetail.SetItemDate(1, "paiddate", state.SsWorkDate);
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
            //tDwMain.Eng2ThaiAllRow();

            tDwDetail = new DwThDate(DwDetail, this);
            tDwDetail.Add("indocudate", "indocu_tdate");
            tDwDetail.Add("appvdate", "appv_tdate");
            tDwDetail.Add("bookdate", "book_tdate");
            tDwDetail.Add("paiddate", "paid_tdate");
            //tDwDetail.Eng2ThaiAllRow();
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
                tDwDetail.Eng2ThaiAllRow();
                // JspostRetrieveDDW();
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
            //Check values before save record
            //check new record or edit row
            tDwMain.Eng2ThaiAllRow();
            try
            {
                String appv_tdate, indocu_tdate, book_tdate, paid_tdate, seq_new = null;
                String pbl = "hr_werfare.pbl";
                // String empl_new = null;

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
                            String EMPLID, appvby, indocuno, levledua, institute, termedua, yearedua, learner, docuno, bookno, remk1, remk2, periid, inexid, paidstatus, pay_no, payer, paidform = null;
                            Decimal totalindrawedu_amt, totaldrawedua_amt = 0;
                            DateTime indocudate = new DateTime();
                            DateTime appvdate = new DateTime();
                            DateTime bookdate = new DateTime();
                            DateTime paiddate = new DateTime();
                            DateTime entry_date = new DateTime();

                            //ตัวแปร วันที่
                            String indocudate_dd = null;
                            String indocudate_mm = null;
                            String indocudate_yyyy = null;

                            String appvdate_dd = null;
                            String appvdate_mm = null;
                            String appvdate_yyyy = null;

                            String bookdate_dd = null;
                            String bookdate_mm = null;
                            String bookdate_yyyy = null;

                            String paiddate_dd = null;
                            String paiddate_mm = null;
                            String paiddate_yyyy = null;

                            String entry_dd = null;
                            String entry_mm = null;
                            String entry_yyyy = null;

                            try
                            {
                                indocu_tdate = DwDetail.GetItemString(1, "indocu_tdate");
                            }
                            catch { indocu_tdate = null; }

                            try
                            {
                                appv_tdate = DwDetail.GetItemString(1, "appv_tdate");
                            }
                            catch { appv_tdate = null; }

                            try
                            {
                                book_tdate = DwDetail.GetItemString(1, "book_tdate");
                            }
                            catch { book_tdate = null; }

                            try
                            {
                                paid_tdate = DwDetail.GetItemString(1, "paid_tdate");
                            }
                            catch { paid_tdate = null; }

                            try
                            {
                                seq_no = DwDetail.GetItemString(1, "seq_no");
                            }
                            catch { seq_no = null; }

                            try
                            {
                                indocuno = DwDetail.GetItemString(1, "indocuno");
                            }
                            catch { indocuno = null; }

                            try
                            {
                                appvby = DwDetail.GetItemString(1, "appvby");
                            }
                            catch { appvby = null; }

                            try
                            {
                                levledua = DwDetail.GetItemString(1, "levledua");
                            }
                            catch { levledua = null; }
                            
                            try
                            {
                                institute = DwDetail.GetItemString(1, "institute");
                            }
                            catch { institute = null; }

                            try
                            {
                                termedua = DwDetail.GetItemString(1, "termedua");
                            }
                            catch { termedua = null; }

                            try
                            {
                                yearedua = DwDetail.GetItemString(1, "yearedua");
                            }
                            catch { yearedua = null; }

                            try
                            {
                                docuno = DwDetail.GetItemString(1, "docuno");
                            }
                            catch { docuno = null; }

                            try
                            {
                                bookno = DwDetail.GetItemString(1, "bookno");
                            }
                            catch { bookno = null; }
                         
                            try
                            {
                                remk1 = DwDetail.GetItemString(1, "remk1");
                            }
                            catch { remk1 = null; }

                            try
                            {
                                remk2 = DwDetail.GetItemString(1, "remk2");
                            }
                            catch { remk2 = null; }

                            try
                            {
                                periid = DwDetail.GetItemString(1, "periid");
                            }
                            catch { periid = null; }

                            try
                            {
                                inexid = DwDetail.GetItemString(1, "inexid");
                            }
                            catch { inexid = null; }

                            try
                            {
                                learner = DwDetail.GetItemString(1, "learner");
                            }
                            catch { learner = null; }
                            
                            try
                            {
                                paidstatus = DwDetail.GetItemString(1, "paidstatus");
                            }
                            catch { paidstatus = null; }

                            try
                            {
                                pay_no = DwDetail.GetItemString(1, "pay_no");
                            }
                            catch { pay_no = null; }

                            try
                            {
                                payer = DwDetail.GetItemString(1, "payer");
                            }
                            catch { payer = null; }

                            try
                            {
                                paidform = DwDetail.GetItemString(1, "paidform");
                            }
                            catch { paidform = null; }

                            try 
                            {
                                totalindrawedu_amt = DwDetail.GetItemDecimal(1, "totalindrawedu_amt");
                            }
                            catch { totalindrawedu_amt = 0; }

                             try 
                            {
                                totaldrawedua_amt = DwDetail.GetItemDecimal(1, "totaldrawedua_amt");
                            }
                             catch { totaldrawedua_amt = 0; }

                             try
                             {
                                 EMPLID = DwMain.GetItemString(1, "emplid");
                             }
                             catch { EMPLID = null; }

                             try
                             {
                                 indocudate = DateTime.ParseExact(DwDetail.GetItemString(1, "indocu_tdate"), "ddMMyyyy", WebUtil.TH);
                                 //indocudate = Convert.ToDateTime(DwDetail.GetItemString(1, "indocu_tdate"));
                                 indocudate_dd = indocudate.Day.ToString();
                                 indocudate_mm = indocudate.Month.ToString();
                                 indocudate_yyyy = indocudate.Year.ToString();

                             }
                             catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }

                             try
                             {
                                 //appvdate = Convert.ToDateTime(DwDetail.GetItemString(1, "appv_tdate"));
                                 appvdate = DateTime.ParseExact(DwDetail.GetItemString(1, "appv_tdate"), "ddMMyyyy", WebUtil.TH);
                                 appvdate_dd = appvdate.Day.ToString();
                                 appvdate_mm = appvdate.Month.ToString();
                                 appvdate_yyyy = appvdate.Year.ToString();
                             }
                             catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
                             
                             try
                             {
                                 //bookdate = Convert.ToDateTime(DwDetail.GetItemString(1, "book_tdate"));
                                 bookdate = DateTime.ParseExact(DwDetail.GetItemString(1, "book_tdate"), "ddMMyyyy", WebUtil.TH);
                                 bookdate_dd = bookdate.Day.ToString();
                                 bookdate_mm = bookdate.Month.ToString();
                                 bookdate_yyyy = bookdate.Year.ToString();
                             }
                             catch (Exception ex) 
                             { 
                                 LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                             }

                             try
                             {
                                 //paiddate = Convert.ToDateTime(DwDetail.GetItemString(1, "paid_tdate"));
                                 paiddate = DateTime.ParseExact(DwDetail.GetItemString(1, "paid_tdate"), "ddMMyyyy", WebUtil.TH);
                                 paiddate_dd = paiddate.Day.ToString();
                                 paiddate_mm = paiddate.Month.ToString();
                                 paiddate_yyyy = paiddate.Year.ToString();
                             }
                             catch (Exception ex) 
                             { 
                                 LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                             }

                             try
                             {
                                 entry_date = state.SsWorkDate;
                                 entry_dd = entry_date.Day.ToString();
                                 entry_mm = entry_date.Month.ToString();
                                 entry_yyyy = entry_date.Year.ToString();
                             }
                             catch (Exception ex) 
                             { 
                                 LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                             }

                            //if (famititl == null || famifirsname == null || famirela == null)
                            //{
                            //    LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอกข้อมูลให้ครบ");
                            //}
                            //else
                            //{

                                //new record
                                Sta ta = new Sta(sqlca.ConnectionString);
                                String seq_tmp = GetDocNo("HRDRAWEDUA");
                                seq_tmp = WebUtil.Right(seq_tmp, 4);
                                seq_new = "EDU" + GetYear("HREMPLFILEMAS") + seq_tmp;
                                DwDetail.SetItemString(1, "seq_no", seq_new);
                                DwDetail.SetItemString(1, "emplid", EMPLID);


                                String sql = @"INSERT INTO HRNMLDRAWEDUADEA (seq_no, emplid, indocuno, indocudate, institute, levledua, yearedua, termedua, learner, docuno, bookno, bookdate, remk1, remk2 ,totalindrawedu_amt, totaldrawedua_amt, periid, inexid, pay_no, paidstatus, payer, paiddate, paidform, appvby, appvdate, entry_id, entry_date, branch_id)";
                                sql += " VALUES ('" + seq_new + "' , '" + EMPLID + "', '" + indocuno + "',to_date('" + indocudate_dd + "/" + indocudate_mm + "/" + indocudate_yyyy + "' ,'dd/mm/yyyy'),'" + institute + "' , '" + levledua + "', '" + yearedua + "' ,'" + termedua + "','" + learner + "', '" + docuno + "', '" + bookno + "' , to_date('" + bookdate_dd + "/" + bookdate_mm + "/" + bookdate_yyyy + "' ,'dd/mm/yyyy'),'" + remk1 + "','" + remk2 + "','" + totalindrawedu_amt + "','" + totaldrawedua_amt + "', '" + periid + "','" + inexid + "','" + pay_no + "','" + paidstatus + "','" + payer + "',to_date('" + paiddate_dd + "/" + paiddate_mm + "/" + paiddate_yyyy + "' ,'dd/mm/yyyy'),'" + paidform + "' ,'" + appvby + "',to_date('" + appvdate_dd + "/" + appvdate_mm + "/" + appvdate_yyyy + "' ,'dd/mm/yyyy'),'" + state.SsUsername + "' ,to_date('" + entry_dd + "/" + entry_mm + "/" + entry_yyyy + "' ,'dd/mm/yyyy') , '" + state.SsCoopId + "')";
                                ta.Exe(sql);

                                LtServerMessage.Text = WebUtil.CompleteMessage("บันทีกข้อมูลเสร็จเรียบร้อยแล้ว");
                                JspostGetMember();
                                ta.Close();
                            //}
                        }
                        else
                        {

                            seq_new = seq_no;
                            DwUtil.UpdateDataWindow(DwDetail, pbl, "HRNMLDRAWEDUADEA");
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
            tDwDetail.Eng2ThaiAllRow();
            tDwMain.Eng2ThaiAllRow();
            DwMain.SaveDataCache();
            DwList.SaveDataCache();
            DwDetail.SaveDataCache();

        }
        #endregion

        public string EMPLID { get; set; }
    }
}
