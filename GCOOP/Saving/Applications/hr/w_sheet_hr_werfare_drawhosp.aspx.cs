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
    public partial class w_sheet_hr_werfare_drawhosp : PageWebSheet, WebSheet
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
            String maxdoc = comm.of_getnewdocno(state.SsWsPass,state.SsCoopId, doc_code);
            return maxdoc;
        }

        private void JspostDeleteRow()
        {
            int RowAll = DwDetail.RowCount;
            String seq_no = null;
            seq_no = DwDetail.GetItemString(RowAll, "emp_seq");
            if (seq_no == "" || seq_no == "<Auto>")
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

                    String sql = @"Delete FROM HRNMLWELFAREEMPLOYEEDEA where emp_seq = '" + seq_no + "'";
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

            seq_no = DwDetail.GetItemString(1, "emp_seq");
            DwDetail.Reset();
            DwDetail.Retrieve(emplid);
            DwDetail.SetFilter("emp_seq = '" + seq_no + "'");
            DwDetail.Filter();
            DwDetail.SetItemString(1, "emp_seq", seq_no);
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
                DwDetail.SetItemDate(1, "commit_date", state.SsWorkDate);
                DwDetail.SetItemDate(1, "appv_date", state.SsWorkDate);
              
                DwDetail.SetItemDate(1, "paid_date", state.SsWorkDate);
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
                    seq_no = DwList.GetItemString(1, "emp_seq");
                    DwDetail.Reset();
                    DwDetail.Retrieve(emplid);
                    DwDetail.SetFilter("emp_seq = '" + seq_no + "'");
                    DwDetail.Filter();
                    DwDetail.SetItemString(1, "emp_seq", seq_no);
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
            tDwDetail.Add("appv_date", "appv_tdate");
            tDwDetail.Add("paid_date", "paid_tdate");
            tDwDetail.Add("commit_date", "commit_tdate");
          
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
                String wfempl_type, appv_tdate, commit_tdate,paid_tdate, commitremk, locate, cremation_type, seq_new = null, paid_status, commit_status, commit_by;
                String pbl = "hr_werfare.pbl";
                decimal pay_commit;
                String empl_tmp = DwMain.GetItemString(1, "emplid").Trim();
                String seq_no = DwDetail.GetItemString(1, "emp_seq").Trim();

                if (empl_tmp == "<Auto>" || empl_tmp == null)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ยังไม่มีข้อมูลเจ้าหน้าที่ กรุณาเลือกเจ้าหน้าที่");
                }
                else
                {
                    if (DwList.RowCount > 0)
                    {
                        if (seq_no == "<Auto>" || seq_no == null)
                        {
                            String EMPLID ;
                           
                            DateTime commit_date = new DateTime();
                            DateTime appv_date = new DateTime();
                           
                            DateTime paid_date = new DateTime();
                            DateTime entry_date = new DateTime();

                            //ตัวแปร วันที่
                            String commit_date_dd = null;
                            String commit_date_mm = null;
                            String commit_date_yyyy = null;

                            String appv_date_dd = null;
                            String appv_date_mm = null;
                            String appv_date_yyyy = null;

                          

                            String paid_date_dd = null;
                            String paid_date_mm = null;
                            String paid_date_yyyy = null;

                            String entry_dd = null;
                            String entry_mm = null;
                            String entry_yyyy = null;

                            try
                            {
                                wfempl_type = DwDetail.GetItemString(1, "wfempl_type");
                            }
                            catch { wfempl_type = null; }

                            try
                            {
                                appv_tdate = DwDetail.GetItemString(1, "appv_tdate");
                            }
                            catch { appv_tdate = null; }
                            try
                            {
                                commit_tdate = DwDetail.GetItemString(1, "commit_tdate");
                            }
                            catch { commit_tdate = null; }
                           

                            try
                            {
                                paid_tdate = DwDetail.GetItemString(1, "paid_tdate");
                            }
                            catch { paid_tdate = null; }

                            try
                            {
                                seq_no = DwDetail.GetItemString(1, "emp_seq");
                            }
                            catch { seq_no = null; }

                            try
                            {
                                commitremk = DwDetail.GetItemString(1, "commitremk");
                            }
                            catch { commitremk = null; }

                            try
                            {
                                locate = DwDetail.GetItemString(1, "locate");
                            }
                            catch { locate = null; }

                           

                            try
                            {
                                paid_status = DwDetail.GetItemString(1, "paid_status");
                            }
                            catch { paid_status = null; }
                            
                            try
                            {
                                commit_status = DwDetail.GetItemString(1, "commit_status");
                            }
                            catch { commit_status = null; }

                            try
                            {
                                commit_by = DwDetail.GetItemString(1, "commit_by");
                            }
                            catch { commit_by = null; }

                            try
                            {
                                pay_commit = DwDetail.GetItemDecimal(1, "pay_commit");
                            }
                            catch { pay_commit = 0; }
                            try
                            {
                                cremation_type = DwDetail.GetItemString(1, "cremation_type");
                            }
                            catch { cremation_type = null; }
                         
                             try
                             {
                                 EMPLID = DwMain.GetItemString(1, "emplid");
                             }
                             catch { EMPLID = null; }

                             try
                             {
                                 commit_date = DateTime.ParseExact(DwDetail.GetItemString(1, "commit_tdate"), "ddMMyyyy", WebUtil.TH);
                                 commit_date_dd = commit_date.Day.ToString();
                                 commit_date_mm = commit_date.Month.ToString();
                                 commit_date_yyyy = commit_date.Year.ToString();

                             }
                             catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }

                             try
                             {  
                                 appv_date = DateTime.ParseExact(DwDetail.GetItemString(1, "appv_tdate"), "ddMMyyyy", WebUtil.TH);
                                 appv_date_dd = appv_date.Day.ToString();
                                 appv_date_mm = appv_date.Month.ToString();
                                 appv_date_yyyy = appv_date.Year.ToString();
                             }
                             catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }

                            
                             try
                             {
                                 paid_date = DateTime.ParseExact(DwDetail.GetItemString(1, "paid_tdate"), "ddMMyyyy", WebUtil.TH);
                                 paid_date_dd = paid_date.Day.ToString();
                                 paid_date_mm = paid_date.Month.ToString();
                                 paid_date_yyyy = paid_date.Year.ToString();
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

                             if (commit_tdate == null)
                             {
                                 LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอกวันที่เอกสาร");
                             }
                             else if (appv_tdate == null)
                             {
                                 LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอกวันที่อนุมัติ");
                             }
                          
                             else if (paid_tdate == null)
                             {
                                 LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอกวันที่ส่งให้การเงิน");
                             }
                             else
                             {
                                 Sta ta = new Sta(sqlca.ConnectionString);
                                 String seq_tmp = GetDocNo("HRDRAWEMP");
                                 seq_tmp = WebUtil.Right(seq_tmp, 4);
                                 seq_new = "EMP" + GetYear("HREMPLFILEMAS") + seq_tmp;
                                 DwDetail.SetItemString(1, "emp_seq", seq_new);
                                 DwDetail.SetItemString(1, "emplid", EMPLID);
                                 String sql = @"INSERT INTO HRNMLWELFAREEMPLOYEEDEA (emp_seq, emplid, wfempl_type,appv_date,commit_date,paid_date,commitremk,locate,paid_status,commit_status,commit_by,pay_commit,entry_id,entry_date,branch_id,cremation_type)";
                                 sql += " VALUES ('" + seq_new + "' , '" + EMPLID + "', '" + wfempl_type + "', to_date('" + appv_date_dd + "/" + appv_date_mm + "/" + appv_date_yyyy + "' ,'dd/mm/yyyy'),to_date('" + commit_date_dd + "/" + commit_date_mm + "/" + commit_date_yyyy + "' ,'dd/mm/yyyy') , to_date('" + paid_date_dd + "/" + paid_date_mm + "/" + paid_date_yyyy + "' ,'dd/mm/yyyy'), '" + commitremk + "' ,'" + locate + "', '" + paid_status + "', '" + commit_status + "' ,'" + commit_by + "','" + pay_commit + "','" + state.SsUsername + "' ,to_date('" + entry_dd + "/" + entry_mm + "/" + entry_yyyy + "' ,'dd/mm/yyyy') , '" + state.SsCoopId + "','" + cremation_type + "')";
                                 ta.Exe(sql);
                                 LtServerMessage.Text = WebUtil.CompleteMessage("บันทีกข้อมูลเสร็จเรียบร้อยแล้ว");
                                 JspostGetMember();
                                 ta.Close();
                             }
                        }
                        else
                        {
                            
                            seq_new = seq_no;
                            DwUtil.UpdateDataWindow(DwDetail, pbl, "HRNMLWELFAREEMPLOYEEDEA");
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
