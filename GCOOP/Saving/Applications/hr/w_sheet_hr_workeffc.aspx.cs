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
    public partial class w_sheet_hr_workeffc : PageWebSheet,WebSheet
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

                String sql = @"Delete from hrnmlworkeffcdea where seq_no = '" + seq_no + "'";
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
                DwDetail.SetItemDate(1, "docudate", state.SsWorkDate);
                DwDetail.SetItemDate(1, "weappvdate", state.SsWorkDate);
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

            tDwList = new DwThDate(DwList, this);
            tDwList.Add("docudate", "docudate_tdate");
            tDwList.Eng2ThaiAllRow();

            tDwDetail = new DwThDate(DwDetail, this);
            tDwDetail.Add("docudate", "docudate_tdate");
            tDwDetail.Add("weappvdate", "weappvdate_tdate");
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
                            String SEQ_NO, EMPLID, DOCUNO, WETYPE, WEDETL, WEREMK,WERESULT ,WEAPPVBY = null;
                            String docudate_tdate, weappvdate_tdate = null;
                            DateTime DOCUDATE = new DateTime();
                            DateTime WEAPPVDATE = new DateTime();
                            DateTime ENTRY_DATE = new DateTime();


                            //ตัวแปร วันที่
                            String docudate_dd = null;
                            String docudate_mm = null;
                            String docudate_yyyy = null;

                            String weappbdate_dd = null;
                            String weappbdate_mm = null;
                            String weappbdate_yyyy = null;

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
                                DOCUNO = DwDetail.GetItemString(1, "docuno");
                            }
                            catch { DOCUNO = null; }

                            try
                            {
                                WETYPE = DwDetail.GetItemString(1, "wetype");
                            }
                            catch { WETYPE = null; }

                            try
                            {
                                WEDETL = DwDetail.GetItemString(1, "wedetl");
                            }
                            catch { WEDETL = null; }

                            try
                            {
                                WEREMK = DwDetail.GetItemString(1, "weremk");
                            }
                            catch { WEREMK = null; }

                            try
                            {
                                WEAPPVBY = DwDetail.GetItemString(1, "weappvby");
                            }
                            catch { WEAPPVBY = null; }


                            try
                            {
                                DOCUDATE = DwDetail.GetItemDate(1, "docudate");
                                docudate_dd = DOCUDATE.Day.ToString();
                                docudate_mm = DOCUDATE.Month.ToString();
                                docudate_yyyy = DOCUDATE.Year.ToString();

                            }
                            catch { }

                            try
                            {
                                WEAPPVDATE = DwDetail.GetItemDate(1, "weappvdate");
                                weappbdate_dd = WEAPPVDATE.Day.ToString();
                                weappbdate_mm = WEAPPVDATE.Month.ToString();
                                weappbdate_yyyy = WEAPPVDATE.Year.ToString();

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
                                docudate_tdate = DwDetail.GetItemString(1, "docudate_tdate");
                            }
                            catch { docudate_tdate = null; }

                            try
                            {
                                weappvdate_tdate = DwDetail.GetItemString(1, "weappvdate_tdate");
                            }
                            catch { weappvdate_tdate = null; }

                            try 
                            {
                                WERESULT = DwDetail.GetItemString(1, "weresult");
                            }
                            catch { WERESULT = null; }


                            //================
                            if (docudate_tdate == null)
                            {
                                LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอกข้อมูลวันที่ใบทำรายการ");
                            }
                            else if (weappvdate_tdate  == null)
                            {
                                LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอกข้อมูลวันที่อนุมัติ");
                            }
                            else if (WEDETL == null)
                            {
                                LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอกข้อมูลรายละเอียดความดี / ความผิด");
                            }
                            else if (WERESULT == null)
                            {
                                LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอกข้อมูลรางวัลที่ได้รับ / โทษที่ได้รับ");
                            }
                            else if (WEAPPVBY  == null)
                            {
                                LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอกข้อมูลผู้อนุมัติ");
                            }
                            else
                            {

                                //new record
                                try
                                {
                                    Sta ta = new Sta(sqlca.ConnectionString);

                                    String seq_tmp = GetDocNo("HRWORKEFF");
                                    //String seq_tmp = "SE53000010";
                                    seq_tmp = WebUtil.Right(seq_tmp, 6);
                                    seq_new = "EF" + GetYear("HREMPLFILEMAS") + seq_tmp;
                                    DwDetail.SetItemString(1, "seq_no", seq_new);
                                    DwDetail.SetItemString(1, "emplid", EMPLID);


                                    String sql = @"INSERT INTO HRNMLWORKEFFCDEA (seq_no, emplid, docuno, docudate, wetype, wedetl, weresult, weremk, weappvby, weappvdate, entry_id, entry_date, branch_id)";
                                    sql += " VALUES ('" + seq_new + "', '" + EMPLID + "', '" + DOCUNO + "' , to_date('" + docudate_dd + "/" + docudate_mm + "/" + docudate_yyyy + "' ,'dd/mm/yyyy'), '" + WETYPE + "' , '" + WEDETL + "', '" + WERESULT + "', '" + WEREMK + "' ,'" + WEAPPVBY + "',to_date('" + weappbdate_dd + "/" + weappbdate_mm + "/" + weappbdate_yyyy + "' ,'dd/mm/yyyy')  ,'"+state.SsUsername+"', to_date('" + entry_dd + "/" + entry_mm + "/" + entry_yyyy + "' ,'dd/mm/yyyy') , '" + state.SsCoopId + "')";
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
