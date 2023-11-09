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
    public partial class w_sheet_hr_entry_emp_family : PageWebSheet, WebSheet
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

                    String sql = @"Delete FROM hrnmlemplfamidea where seq_no = '" + seq_no + "'";
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
                //DwDetail.SetItemDate(1, "birthdate_date", state.SsWorkDate);
                //tDwDetail.Eng2ThaiAllRow();
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
                    //tDwDetail.Eng2ThaiAllRow();
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
            DwDetail.SetItemDate(1, "birthdate", state.SsWorkDate);
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
            tDwDetail = new DwThDate(DwDetail, this);
            tDwDetail.Add("birthdate", "birthdate_tdate");
            tDwMain.Add("entry_date", "entry_tdate");
            tDwMain.Eng2ThaiAllRow();
           
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
            //check new record or edit ro
           
            tDwMain.Eng2ThaiAllRow();
          
            try
            {
                String seq_new, birthdate_tdate = null;
                DateTime birthdate_date2 = new DateTime();
                // String empl_new = null;
                String empl_tmp = DwMain.GetItemString(1, "emplid").Trim();
                String seq_no = DwDetail.GetItemString(1, "seq_no").Trim();
                String pbl = "hr_entry_emp_family.pbl";
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
                            String famititl = null;
                            String famifirsname = null;
                            String famirela = null;
                            String familastname = null;
                            String famioccu = null;
                            DateTime birthdate = new DateTime();
                            DateTime entry_date = new DateTime();
                            //DateTime birthdate_date = new DateTime();

                            //ตัวแปร วันที่
                            String birthdate_dd = null;
                            String birthdate_mm = null;
                            String birthdate_yyyy = null;

                            String entry_dd = null;
                            String entry_mm = null;
                            String entry_yyyy = null;

                            try
                            {
                                emplid = DwMain.GetItemString(1, "emplid");
                            }
                            catch { emplid = null; }
                            try
                            {
                                famititl = DwDetail.GetItemString(1, "famititl");
                            }
                            catch { famititl = null; }

                            try
                            {
                                famifirsname = DwDetail.GetItemString(1, "famifirsname");
                            }
                            catch { famifirsname = null; }

                            try
                            {
                                famirela = DwDetail.GetItemString(1, "famirela");
                            }
                            catch { famirela = null; }

                            try
                            {
                                familastname = DwDetail.GetItemString(1, "famelastname");
                            }
                            catch { familastname = null; }

                            try
                            {
                                famirela = DwDetail.GetItemString(1, "famirela");
                            }
                            catch { famirela = null; }

                            try
                            {
                                famioccu = DwDetail.GetItemString(1, "famioccu");
                            }
                            catch { famioccu = null; }

                            try
                            {
                                birthdate_tdate = DwDetail.GetItemString(1, "birthdate_tdate");
                            }
                            catch { birthdate_tdate = null; }

                            try
                            {
                                birthdate_date2 = DwDetail.GetItemDateTime(1, "birthdate");
                            }
                            catch {  }
                           
                            try
                            {
                                birthdate = DateTime.ParseExact(birthdate_tdate, "ddMMyyyy", WebUtil.EN);
                                birthdate_dd = birthdate.Day.ToString();
                                birthdate_mm = birthdate.Month.ToString();
                               int birthdate_yyyyth = birthdate.Year-543;
                               birthdate_yyyy = birthdate_yyyyth.ToString();
                            }
                            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }

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

                            if (famititl == null || famifirsname == null || famirela == null)
                            {
                                LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอกข้อมูลให้ครบ");
                            }
                            else
                            {

                                //new record
                                Sta ta = new Sta(sqlca.ConnectionString);
                                String seq_tmp = GetDocNo("HRFAMILY");
                                seq_tmp = WebUtil.Right(seq_tmp, 4);
                                seq_new = "FA" + GetYear("HREMPLFILEMAS") + seq_tmp;
                                DwDetail.SetItemString(1, "seq_no", seq_new);
                                DwDetail.SetItemString(1, "emplid", DwMain.GetItemString(1, "emplid"));
                                DwDetail.SetItemString(1, "entry_id", state.SsUsername);
                                DwDetail.SetItemDateTime(1, "entry_date", state.SsWorkDate);
                                DwDetail.SetItemString(1, "branch_id", state.SsCoopId);

                                //DwDetail.UpdateData();
                                String sql = @"INSERT INTO HRNMLEMPLFAMIDEA (seq_no, emplid, famititl, famifirsname, famelastname, famirela, famioccu, entry_id, entry_date, branch_id, birthdate)";
                                sql += " VALUES ('" + seq_new + "' , '" + emplid + "', '" + famititl + "', '" + famifirsname + "' ,'" + familastname + "' , '" + famirela + "', '" + famioccu + "', '" + state.SsUsername + "' , to_date('" + entry_dd + "/" + entry_mm + "/" + entry_yyyy + "' ,'dd/mm/yyyy') , '" + state.SsCoopId + "', to_date('" + birthdate_dd + "/" + birthdate_mm + "/" + birthdate_yyyy + "' ,'dd/mm/yyyy'))";
                                ta.Exe(sql);
                                LtServerMessage.Text = WebUtil.CompleteMessage("บันทีกข้อมูลเสร็จเรียบร้อยแล้ว");
                                JspostGetMember();
                            }
                        }
                        else
                        {
                            seq_new = seq_no; ;
                            DwUtil.UpdateDataWindow(DwDetail, pbl, "HRNMLEMPLFAMIDEA");
                            //DwDetail.UpdateData();
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
         tDwDetail.Thai2EngAllRow();
            DwMain.SaveDataCache();
            DwList.SaveDataCache();
            DwDetail.SaveDataCache();
        }
        #endregion
    }
}
