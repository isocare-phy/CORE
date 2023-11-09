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
using System.IO;
using System.Globalization;
using CoreSavingLibrary.WcfNCommon;

namespace Saving.Applications.hr
{
    public partial class w_sheet_hr_master : PageWebSheet, WebSheet
    {
        private String emplid;
        private DwThDate tDwMain;
        private DwThDate tDwFamily;
        private DwThDate tDwEdu;
        private DwThDate tDwExpr;
        private DwThDate tDwSeminar;
        private DwThDate tDwWelfare;
        protected String postNewClear;
        protected String postGetMember;
        protected String postDeleteRow;
        protected String postSearchGetMember;
        protected String postUploadpic;
        protected String postFilterAmpher;
        protected String postRefresh;
        protected String postFilterStep;
        protected String postStepSalary;
        protected String postFilterDeptid;
        protected String postSetPostcode;
        protected String postShowDetailFami;
        protected String postShowDetailEdu;
        protected String postShowDetailExp;
        protected String postShowDetailSemi;
        protected String postShowDetailWel;
        //==================
        //Set Postcode รหัสไปรษณีย์
        protected void JspostSetPostcode()
        {
            String emplprvn = null;
            String emplamph = null;
            String postcode = null;

            try
            {
                emplprvn = DwMain.GetItemString(1, "emplprvn");
            }
            catch { emplprvn = null; }

            try
            {
                emplamph = DwMain.GetItemString(1, "emplamph");
            }
            catch { emplamph = null; }

            if (emplprvn == null)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("กรุณาเลือกข้อมูลจังหวัด");
            }
            //else if (emplamph == null)
            //{
            //    LtServerMessage.Text = WebUtil.ErrorMessage("กรุณาเลือกข้อมูลอำเภอ");
            //}

            else
            {

                Sta ta = new Sta(sqlca.ConnectionString);
                try
                {
                    String sql = @"select postcode from mbucfdistrict where PROVINCE_CODE = '" + emplprvn + "' and district_code = '" + emplamph + "'";
                    Sdt dt = ta.Query(sql);
                    if (dt.Next())
                    {
                        postcode = dt.GetString("postcode");
                        DwMain.SetItemString(1, "empladdrpostcode", postcode);
                    }
                    else
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบข้อมูล กรุณาเลือกรายการใหม่");
                        //JspostNewClear();
                    }
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
                }
            }
        }

        protected void JspostFilterDeptid()
        {
            if (DwMain.GetItemString(1, "sideid") != null)
            {
                DataWindowChild dc = DwMain.GetChild("deptid");
                dc.SetTransaction(sqlca);
                dc.Reset();
                dc.Retrieve();
                dc.SetFilter("sideid ='" + DwMain.GetItemString(1, "sideid") + "'");
                dc.Filter();
            }
        }

        protected void JspostFilterStep()
        {
            if (DwMain.GetItemString(1, "postid") != null)
            {
                DataWindowChild dc = DwMain.GetChild("step_position");
                dc.SetTransaction(sqlca);
                dc.Reset();
                dc.Retrieve();
                dc.SetFilter("seq_posi ='" + DwMain.GetItemString(1, "postid") + "'");
                dc.Filter();
            }
        }

        protected void JspostStepSalary()
        {
            Decimal salary = 0;
            String hrlevel = null;
            String account = null;
            String step_position = null;
            String step_salary = null;

            try
            {
                step_position = DwMain.GetItemString(1, "step_position");
            }
            catch { step_position = null; }

            try
            {
                account = DwMain.GetItemString(1, "account");
            }
            catch { account = null; }

            try
            {
                hrlevel = DwMain.GetItemString(1, "postid");
            }
            catch { hrlevel = null; }

            Sta ta = new Sta(sqlca.ConnectionString);
            try
            {
                String sql = @"select salary from hrucfstep where hrlevel = '" + hrlevel + "' and account = '" + account + "' and step = '1' ";
                Sdt dt = ta.Query(sql);
                if (dt.Next())
                {
                    step_salary = dt.GetString("salary");
                    salary = Convert.ToDecimal(step_salary);
                    //salary = Convert.ToDecimal(dt.GetString("salary"));
                    DwMain.SetItemDecimal(1, "salary", salary);
                }
                else
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบข้อมูล กรุณาเลือกรายการใหม่");
                    //JspostNewClear();
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
            }
        }

        private void JspostPicture()
        {
            String emplid = DwMain.GetItemString(1, "emplid");
            String prename = DwMain.GetItemString(1, "prename_code").Trim();
            String pic_path = null;
            Sta ta = new Sta(sqlca.ConnectionString);
            try
            {
                String sql = @"select picpath from hrnmlemplfilemas where emplid = '" + emplid + "'";
                Sdt dt = ta.Query(sql);
                if (dt.Next())
                {
                    pic_path = dt.GetString("picpath");
                    DwMain.SetItemString(1, "emplpicpath", emplid);
                    try
                    {

                        if (pic_path != "" && pic_path != null)
                        {
                            Image1.Width = 120;
                            Image1.ImageUrl = pic_path;
                        }
                        else
                        {
                           
                                Image1.ImageUrl = "./image/icon_guest.jpg";
                            
                        }

                    }
                    catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message); }
                }
                else
                {
                    //LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบข้อมูลเจ้าหน้าที่รายนี้ กรุณากรอกข้อมูลใหม่");
                    //JspostNewClear();
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
                //String err = ex.ToString();
            }

            ta.Close();


        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            //Browse new empl picture
            String empl_tmp = DwMain.GetItemString(1, "emplid");
            if ((FileUpload.PostedFile != null) && (FileUpload.PostedFile.ContentLength > 0))
            {
                try
                {
                    String time = DateTime.Now.ToString("HH-mm-ss");
                    String filename = Path.GetFileName(FileUpload.PostedFile.FileName);

                    String path_img = Server.MapPath("") + "\\image\\" + time + "_" + filename;

                    String url_img = state.SsUrl + "Applications\\hr\\image\\" + filename.Replace("\\", "/");

                    FileUpload.SaveAs(path_img);

                    path_img = "./image/" + time + "_" + filename;


                    Image1.Width = 120;
                    Image1.ImageUrl = path_img;
                    //Label1.Text ="path = " +  path_img + "  , url = " + url_img;
                    DwMain.SetItemString(1, "emplpicpath", url_img);
                    DwMain.SetItemString(1, "picpath", path_img);
                   Hdpath.Value = path_img;
                    
                }
                catch (Exception ex)
                {
                    ex.ToString();
                    Label1.Visible = true;
                    Label1.Text = "Can not UpLoad picture <br />";
                }
            }
            else
            {
                if (FileUpload.PostedFile == null)
                {
                    Label1.Visible = true;
                    Label1.Text = "No picture source<br />";
                }
                else
                {
                    //Label1.Visible = true;
                    //Label1.Text = "Please login<br />";
                }
            }
        }


        protected void JspostDateEngToThai()
        {
            tDwMain = new DwThDate(DwMain, this);
            tDwFamily = new DwThDate(dw_family, this);
            tDwSeminar = new DwThDate(dw_siminardetail, this);
            tDwWelfare = new DwThDate(dw_weldetail, this);
            //tDwMain.Add("entry_date", "entry_tdate");
            tDwMain.Add("emplbirtdate", "birth_tdate");
           
            tDwMain.Add("emplciticardexpidate", "cityexp_tdate");
            tDwMain.Add("emplciticarddate", "citycard_tdate");
            
            tDwMain.Add("emplbegndate", "begin_tdate");
            tDwMain.Add("emplprobdate", "prob_tdate");
            tDwMain.Add("getout_date", "getout_tdate");
            tDwMain.Add("last_date", "last_tdate");
            tDwMain.Add("empend_date", "empend_tdate");
            tDwFamily.Add("birthdate", "birthdate_tdate");
            tDwSeminar.Add("date_seminars", "seminars_tdate");
            tDwSeminar.Add("date_seminarf", "seminarf_tdate");
            tDwSeminar.Add("commit_date", "commit_tdate");
            tDwWelfare.Add("appv_date", "appv_tdate");
            tDwWelfare.Add("paid_date", "paid_tdate");
            tDwWelfare.Add("commit_date", "commit_tdate");
            tDwMain.Eng2ThaiAllRow();
            tDwFamily.Eng2ThaiAllRow();
            tDwSeminar.Eng2ThaiAllRow();
            tDwWelfare.Eng2ThaiAllRow();
        }

        private void JspostFilterAmpher()
        {
            try
            {
                //String emplprvn = DwMain.GetItemString(1, "emplprvn");
                //object[] arg = new object[1];
                //arg[0] = emplprvn;
                //DwUtil.RetrieveDDDW(DwMain, "emplamph", "hr_master.pbl", arg);

                //if (DwMain.GetItemString(1, "emplprvn") != null)
                //{
                DataWindowChild childdis = DwMain.GetChild("emplamph");
                childdis.SetTransaction(sqlca);
                childdis.Retrieve();
                String emplprvn = DwMain.GetItemString(1, "emplprvn");
                String a = emplprvn;
                childdis.SetFilter("province_code='" + emplprvn + "'");
                childdis.Filter();
                //}
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }

        }

        protected void JspostUploadPic()
        {
            //Browse new empl picture
            //String empl_tmp = DwMain.GetItemString(1, "emplid").Trim();
            //if ((FileUpload.PostedFile != null) && (FileUpload.PostedFile.ContentLength > 0) && empl_tmp.StartsWith("EM"))
            //{
            //    try
            //    {
            //        String filename = Path.GetFileName(FileUpload.PostedFile.FileName);
            //        String path_img = Server.MapPath("") + "\\image\\" + filename;
            //        String url_img = state.SsUrl + "Applications\\hr\\image\\" + filename.Replace("\\", "/");
            //        FileUpload.SaveAs(path_img);
            //        Image1.Width = 120;
            //        Image1.ImageUrl = url_img;
            //        //Label1.Text ="path = " +  path_img + "  , url = " + url_img;
            //        DwMain.SetItemString(1, "emplpicpath", url_img);
            //    }
            //    catch (Exception ex)
            //    {
            //        LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            //    }
            //}
            //else
            //{
            //    if (FileUpload.PostedFile == null)
            //    {
            //        Label1.Visible = true;
            //        Label1.Text = "No picture source<br />";
            //    }
            //    else
            //    {
            //        Label1.Visible = true;
            //        Label1.Text = "Please login<br />";
            //    }
            //}
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
                //String emplcode = DwMain.GetItemString(1, "emplcode").Trim();
                //String emplfirsname = DwMain.GetItemString(1, "emplfirsname").Trim();
                //String empllastname = DwMain.GetItemString(1, "empllastname").Trim();
                //String empltypeid = DwMain.GetItemString(1, "empltypeid").Trim();
                emplid = HdfEmplid.Value.Trim();//DwMain.GetItemString(1, "emplid").Trim();
                //String emplcode = DwMain.GetItemString(1, "emplcode").Trim();
                String sql = @"Delete FROM hrnmlemplfilemas where emplid = '" + emplid + "'";
                try
                {
                    ta.Exe(sql);
                    LtServerMessage.Text = WebUtil.CompleteMessage("ลบข้อมูลเรียบร้อยแล้ว");
                    JspostNewClear();
                    //JspostGetMember();
                    HdfEmplid.Value = "";
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("เกิดข้อผิดพลาด " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
            }
            //}

            //catch (Exception ex)
            //{
            //    String err = ex.ToString();

            //}
            ta.Close();

            //Retrieve ...
            // JspostGetMember();
        }


        private void JspostGetMember()
        {
            try
            {
                emplid = DwMain.GetItemString(1, "emplid").Trim();
            }
            catch
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
                dw_edudetail.Retrieve(emplid);
                dw_edulist.Retrieve(emplid);
                dw_expdetail.Retrieve(emplid);
                dw_explist.Retrieve(emplid);
                dw_family.Retrieve(emplid);
                dw_famlist.Retrieve(emplid);
                dw_siminardetail.Retrieve(emplid);
                dw_siminarlist.Retrieve(emplid);
                dw_weldetail.Retrieve(emplid);
                dw_wellist.Retrieve(emplid);
                DataWindowChild childdis = DwMain.GetChild("emplamph");
                childdis.SetTransaction(sqlca);
                childdis.Reset();
                childdis.Retrieve();
                String emplprvn = DwMain.GetItemString(1, "emplprvn");
                childdis.SetFilter(" province_code='" + emplprvn + "'");
                childdis.Filter();

                JspostPicture();

                String emplcode = null;

                try
                {
                    emplcode = DwMain.GetItemString(1, "emplcode");
                }
                catch { emplcode = null; }

                if (emplcode != null)
                {
                    DwMain.SetItemString(1, "entry_id", state.SsUsername);
                    DwMain.SetItemDate(1, "entry_date", state.SsWorkDate);
                    DwMain.SetItemString(1, "branch_id", state.SsCoopId);
                    tDwMain.Eng2ThaiAllRow();
                }
                else
                {

                    DwMain.SetItemString(1, "entry_id", state.SsUsername);
                    DwMain.SetItemDate(1, "entry_date", state.SsWorkDate);
                    DwMain.SetItemString(1, "branch_id", state.SsCoopId);
                    tDwMain.Eng2ThaiAllRow();
                }
                if (dw_edudetail.RowCount<1)
                {
                    dw_edudetail.Reset();
                    dw_edudetail.InsertRow(0);
                }
                if(dw_expdetail.RowCount<1){
                    dw_expdetail.Reset();
                    dw_expdetail.InsertRow(0);
                }
                if(dw_family.RowCount<1){
                    dw_family.Reset();
                    dw_family.InsertRow(0);
                }
                if(dw_siminardetail.RowCount<1){
                    dw_siminardetail.Reset();
                    dw_siminardetail.InsertRow(0);
                    tDwSeminar.Eng2ThaiAllRow();
                }
                if(dw_weldetail.RowCount<1){
                    dw_weldetail.Reset();
                    dw_weldetail.InsertRow(0);
                    tDwWelfare.Eng2ThaiAllRow();
                }
            }
        }

        private void JspostNewClear()
        {
            DwMain.Reset();
            dw_family.Reset();
            dw_famlist.Reset();
            dw_edudetail.Reset();
            dw_edulist.Reset();
            dw_expdetail.Reset();
            dw_explist.Reset();
            dw_siminardetail.Reset();
            dw_siminarlist.Reset();
            dw_weldetail.Reset();
            dw_wellist.Reset();
            DwMain.InsertRow(0);
            dw_family.InsertRow(0);
            dw_famlist.InsertRow(0);
            dw_edudetail.InsertRow(0);
            dw_edulist.InsertRow(0);
            dw_expdetail.InsertRow(0);
            dw_explist.InsertRow(0);
            dw_siminardetail.InsertRow(0);
            dw_siminarlist.InsertRow(0);
            dw_weldetail.InsertRow(0);
            dw_wellist.InsertRow(0);
            DwMain.SetItemString(1, "entry_id", state.SsUsername);
            DwMain.SetItemString(1, "branch_id", state.SsCoopId);
            DwMain.SetItemDate(1, "entry_date", state.SsWorkDate);
            JspostDateEngToThai();
            tDwMain.Eng2ThaiAllRow();
            //image set

            try
            {
                String imageUrl = DwMain.GetItemString(1, "picpath");
                Image1.ImageUrl = imageUrl;
            }
            catch
            {
                Image1.ImageUrl =  "./image/icon_guest.jpg";
                DwMain.Reset();
                dw_family.Reset();
                dw_famlist.Reset();
                dw_edudetail.Reset();
                dw_edulist.Reset();
                dw_expdetail.Reset();
                dw_explist.Reset();
                dw_siminardetail.Reset();
                dw_siminarlist.Reset();
                dw_weldetail.Reset();
                dw_wellist.Reset();
                DwMain.InsertRow(0);
                dw_family.InsertRow(0);
                dw_famlist.InsertRow(0);
                dw_edudetail.InsertRow(0);
                dw_edulist.InsertRow(0);
                dw_expdetail.InsertRow(0);
                dw_explist.InsertRow(0);
                dw_siminardetail.InsertRow(0);
                dw_siminarlist.InsertRow(0);
                dw_weldetail.InsertRow(0);
                dw_wellist.InsertRow(0);
            }

        }
         private void JspostShowDetailFami()
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

            seq_no = dw_family.GetItemString(1, "seq_no");
            dw_family.Reset();
            dw_family.Retrieve(emplid);
            dw_family.SetFilter("seq_no = '" + seq_no + "'");
            dw_family.Filter();
            dw_family.SetItemString(1, "seq_no", seq_no);
            tDwFamily.Eng2ThaiAllRow();
        }
        //==================


        public void InitJsPostBack()
        {
            postNewClear = WebUtil.JsPostBack(this, "postNewClear");
            postGetMember = WebUtil.JsPostBack(this, "postGetMember");
            postDeleteRow = WebUtil.JsPostBack(this, "postDeleteRow");
            postSearchGetMember = WebUtil.JsPostBack(this, "postSearchGetMember");
            postUploadpic = WebUtil.JsPostBack(this, "postUploadpic");
            postFilterAmpher = WebUtil.JsPostBack(this, "postFilterAmpher");
            postRefresh = WebUtil.JsPostBack(this, "postRefresh");
            postFilterStep = WebUtil.JsPostBack(this, "postFilterStep");
            postStepSalary = WebUtil.JsPostBack(this, "postStepSalary");
            postFilterDeptid = WebUtil.JsPostBack(this, "postFilterDeptid");
            postSetPostcode = WebUtil.JsPostBack(this, "postSetPostcode");
            postShowDetailFami = WebUtil.JsPostBack(this, "postShowDetailFami");
            postShowDetailEdu = WebUtil.JsPostBack(this, "postShowDetailEdu");
            postShowDetailExp = WebUtil.JsPostBack(this, "postShowDetailExp");
            postShowDetailSemi = WebUtil.JsPostBack(this, "postShowDetailSemi");
            postShowDetailWel = WebUtil.JsPostBack(this, "postShowDetailWel");
            
            //====================================
            JspostDateEngToThai();
        }
        private void JspostShowDetailEdu()
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

            seq_no = dw_edudetail.GetItemString(1, "seq_no");
            dw_edudetail.Reset();
            dw_edudetail.Retrieve(emplid);
            dw_edudetail.SetFilter("seq_no = '" + seq_no + "'");
            dw_edudetail.Filter();
            dw_edudetail.SetItemString(1, "seq_no", seq_no);
        }
        private void JspostShowDetailExp()
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

            seq_no = dw_expdetail.GetItemString(1, "seq_no");
            dw_expdetail.Reset();
            dw_expdetail.Retrieve(emplid);
            dw_expdetail.SetFilter("seq_no = '" + seq_no + "'");
            dw_expdetail.Filter();
            dw_expdetail.SetItemString(1, "seq_no", seq_no);
        }
        private void JspostShowDetailSemi()
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

            seq_no = dw_siminardetail.GetItemString(1, "seq_no");
            dw_siminardetail.Reset();
            dw_siminardetail.Retrieve(emplid);
            dw_siminardetail.SetFilter("seq_no = '" + seq_no + "'");
            dw_siminardetail.Filter();
            dw_siminardetail.SetItemString(1, "seq_no", seq_no);
            tDwSeminar.Eng2ThaiAllRow();
        }
        private void JspostShowDetailWel()
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

            seq_no = dw_weldetail.GetItemString(1, "emp_seq");
            dw_weldetail.Reset();
            dw_weldetail.Retrieve(emplid);
            dw_weldetail.SetFilter("emp_seq = '" + seq_no + "'");
            dw_weldetail.Filter();
            dw_weldetail.SetItemString(1, "emp_seq", seq_no);
            tDwWelfare.Eng2ThaiAllRow();
        }
        public void WebSheetLoadBegin()
        {
            this.ConnectSQLCA();
            DwMain.SetTransaction(sqlca);
            dw_family.SetTransaction(sqlca);
            dw_famlist.SetTransaction(sqlca);
            dw_edudetail.SetTransaction(sqlca);
            dw_edulist.SetTransaction(sqlca);
            dw_expdetail.SetTransaction(sqlca);
            dw_explist.SetTransaction(sqlca);
            dw_siminardetail.SetTransaction(sqlca);
            dw_siminarlist.SetTransaction(sqlca);
            dw_wellist.SetTransaction(sqlca);
            dw_weldetail.SetTransaction(sqlca);
            if (!IsPostBack)
            {
                JspostNewClear();
            }
            else
            {
                this.RestoreContextDw(DwMain);
                this.RestoreContextDw(dw_family);
                this.RestoreContextDw(dw_famlist);
                this.RestoreContextDw(dw_edudetail);
                this.RestoreContextDw(dw_edulist);
                this.RestoreContextDw(dw_expdetail);
                this.RestoreContextDw(dw_explist);
                this.RestoreContextDw(dw_siminardetail);
                this.RestoreContextDw(dw_siminarlist);
                this.RestoreContextDw(dw_wellist);
                this.RestoreContextDw(dw_weldetail);
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
            else if (eventArg == "postDeleteRow")
            {
                JspostDeleteRow();
            }
            else if (eventArg == "postSearchGetMember")
            {
                JspostSearchGetMember();
            }
            else if (eventArg == "postUploadpic")
            {
                JspostUploadPic();
            }
            else if (eventArg == "postFilterAmpher")
            {
                JspostFilterAmpher();
            }
            else if (eventArg == "postRefresh")
            {
                String emplstat = null;
                emplstat = DwMain.GetItemString(1, "emplstat");
                if (emplstat != "2")
                {
                    //Refresh
                    DwMain.SetItemString(1, "getout_tdate", null);
                    DwMain.SetItemString(1, "last_tdate", null);
                    DwMain.SetItemString(1, "remkgetout", null);
                }
            }
            else if (eventArg == "postFilterStep")
            {
                JspostFilterStep();
            }
            else if (eventArg == "postStepSalary")
            {
                JspostStepSalary();
            }
            else if (eventArg == "postFilterDeptid")
            {
                JspostFilterDeptid();
            }
            else if (eventArg == "postSetPostcode")
            {
                JspostSetPostcode();
            }
            else if (eventArg == "postShowDetailFami")
            {
                JspostShowDetailFami();
            }
            else if (eventArg == "postShowDetailEdu")
            {
                JspostShowDetailEdu();
            }
            else if (eventArg == "postShowDetailExp")
            {
                JspostShowDetailExp();
            }

            else if (eventArg == "postShowDetailSemi")
            {
                JspostShowDetailSemi();
            }
            else if (eventArg == "postShowDetailWel")
            {
                JspostShowDetailWel();
            }
        }


        public void SaveWebSheet()
        {
            //Check values before save record
            //check new record or edit row

            try
            {
                String emplid = DwMain.GetItemString(1, "emplid").Trim();

                if (emplid == null)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ยังไม่มีข้อมูลเจ้าหน้าที่ กรุณาเลือกเจ้าหน้าที่");
                }
                else if (emplid == "Auto")
                {
                    String emplcode,prename_code, empltypeid, sideid, deptid, postid, empltitl, emplfirsname, empllastname, emplnickname, emplenghtitl, emplenghfirsname, emplenghlastname, post, emplenghnickname = null;
                    String emplsex, emplbloodgrup, emplweig, emplhigh, emplnation, emplorig, emplbirtprvn, empladdr1, emplprvn, emplamph, empldist, empladdrpostcode, empltele, emplciticardaddr1 = null;
                    String emplcitiid, email, emplciticardamph, emplstat, remkgetout, emplbirtcntry, step_position, account = null;

                    String birth_tdate, citycard_tdate, cityexp_tdate, begin_tdate, prob_tdate;
                    String picpath = Hdpath.Value.Trim();

           Decimal salary = 0;
                    Decimal hrlevel = 0;

                    DateTime emplbirtdate = new DateTime();
                    DateTime emplciticarddate = new DateTime();
                    DateTime emplciticardexpidate = new DateTime();
                    DateTime emplbegndate = new DateTime();
                    DateTime emplprobdate = new DateTime();
                    DateTime getout_date = new DateTime();
                    DateTime last_date = new DateTime();
                    DateTime entry_date = new DateTime();

                    //ตัวแปร วันที่
                    String birtdate_dd = null;
                    String birtdate_mm = null;
                    String birtdate_yyyy = null;

                    String citicarddate_dd = null;
                    String citicarddate_mm = null;
                    String citicarddate_yyyy = null;

                    String citicardexpdate_dd = null;
                    String citicardexpdate_mm = null;
                    String citicardexpdate_yyyy = null;

                    String begindate_dd = null;
                    String begindate_mm = null;
                    String begindate_yyyy = null;

                    String probdate_dd = null;
                    String probdate_mm = null;
                    String probdate_yyyy = null;

                    String getoutdate_dd = null;
                    String getoutdate_mm = null;
                    String getoutdate_yyyy = null;

                    String lastdate_dd = null;
                    String lastdate_mm = null;
                    String lastdate_yyyy = null;

                    String entry_dd = null;
                    String entry_mm = null;
                    String entry_yyyy = null;

                    // Get ค่าขึ้นมา check ค่าว่าง
                    // เช็คค่าว่างวันที่

                    try { account = DwMain.GetItemString(1, "account"); }
                    catch { account = null; }

                    try
                    {
                        post = DwMain.GetItemString(1, "postid");
                        hrlevel = Convert.ToDecimal(post);
                    }
                    catch { hrlevel = 0; }

                    try { step_position = DwMain.GetItemString(1, "step_position"); }
                    catch { step_position = null; }

                    try { salary = DwMain.GetItemDecimal(1, "salary"); }
                    catch { salary = 0; }

                    try { birth_tdate = DwMain.GetItemString(1, "birth_tdate"); }
                    catch { birth_tdate = null; }

                    try { citycard_tdate = DwMain.GetItemString(1, "citycard_tdate"); }
                    catch { citycard_tdate = null; }

                    try { cityexp_tdate = DwMain.GetItemString(1, "cityexp_tdate"); }
                    catch { cityexp_tdate = null; }

                    try { begin_tdate = DwMain.GetItemString(1, "begin_tdate"); }
                    catch { begin_tdate = null; }

                    try { prob_tdate = DwMain.GetItemString(1, "prob_tdate"); }
                    catch { prob_tdate = null; }
                    //==========================
                    try { emplid = DwMain.GetItemString(1, "emplid"); }
                    catch { emplid = null; }


                    try { prename_code = DwMain.GetItemString(1, "prename_code"); }
                    catch { prename_code = null; }

                    try { emplcode = DwMain.GetItemString(1, "emplcode"); }

                    catch { emplcode = null; }

                    try { empltypeid = DwMain.GetItemString(1, "empltypeid"); }
                    catch { empltypeid = null; }

                    try { sideid = DwMain.GetItemString(1, "sideid"); }
                    catch { sideid = null; }

                    try { deptid = DwMain.GetItemString(1, "deptid").Trim(); }
                    catch { deptid = null; }

                    try { postid = DwMain.GetItemString(1, "postid"); }
                    catch { postid = null; }

                    try { empltitl = DwMain.GetItemString(1, "empltitl"); }
                    catch { empltitl = null; }

                    try { emplfirsname = DwMain.GetItemString(1, "emplfirsname"); }
                    catch { emplfirsname = null; }

                    try { empllastname = DwMain.GetItemString(1, "empllastname"); }
                    catch { empllastname = null; }

                    try { emplnickname = DwMain.GetItemString(1, "emplnickname"); }
                    catch { emplnickname = null; }

                    try { emplenghlastname = DwMain.GetItemString(1, "emplenghlastname"); }
                    catch { emplenghlastname = null; }

                    try { emplenghnickname = DwMain.GetItemString(1, "emplenghnickname"); }
                    catch { emplenghnickname = null; }

                    try { emplsex = DwMain.GetItemString(1, "emplsex"); }
                    catch { emplsex = null; }

                    try { emplbloodgrup = DwMain.GetItemString(1, "emplbloodgrup"); }
                    catch { emplbloodgrup = null; }

                    try { emplweig = DwMain.GetItemString(1, "emplweig"); }
                    catch { emplweig = null; }

                    try { emplhigh = DwMain.GetItemString(1, "emplhigh"); }
                    catch { emplhigh = null; }
                    //==

                    try { emplnation = DwMain.GetItemString(1, "emplnation"); }
                    catch { emplnation = null; }

                    try { emplorig = DwMain.GetItemString(1, "emplorig"); }
                    catch { emplorig = null; }

                    try { emplbirtprvn = DwMain.GetItemString(1, "emplbirtprvn"); }
                    catch { emplbirtprvn = null; }

                    try { empladdr1 = DwMain.GetItemString(1, "empladdr1"); }
                    catch { empladdr1 = null; }

                    try { emplprvn = DwMain.GetItemString(1, "emplprvn"); }
                    catch { emplprvn = null; }

                    try { emplamph = DwMain.GetItemString(1, "emplamph"); }
                    catch { emplamph = null; }

                    //===
                    try { empldist = DwMain.GetItemString(1, "empldist"); }
                    catch { empldist = null; }

                    try { empladdrpostcode = DwMain.GetItemString(1, "empladdrpostcode"); }
                    catch { empladdrpostcode = null; }

                    try { empltele = DwMain.GetItemString(1, "empltele"); }
                    catch { empltele = null; }

                    try { emplciticardaddr1 = DwMain.GetItemString(1, "emplciticardaddr1"); }
                    catch { emplciticardaddr1 = null; }

                    try { emplcitiid = DwMain.GetItemString(1, "emplcitiid"); }
                    catch { emplcitiid = null; }

                    try { email = DwMain.GetItemString(1, "email"); }
                    catch { email = null; }

                    //===
                    try { emplciticardamph = DwMain.GetItemString(1, "emplciticardamph"); }
                    catch { emplciticardamph = null; }

                    try { emplstat = DwMain.GetItemString(1, "emplstat"); }
                    catch { emplstat = null; }

                    try { remkgetout = DwMain.GetItemString(1, "remkgetout"); }
                    catch { remkgetout = null; }

                    try { emplbirtcntry = DwMain.GetItemString(1, "emplbirtcntry"); }
                    catch { emplbirtcntry = null; }

                    try { emplenghtitl = DwMain.GetItemString(1, "emplenghtitl"); }
                    catch { emplenghtitl = null; }

                    try { emplenghfirsname = DwMain.GetItemString(1, "emplenghfirsname"); }
                    catch { emplenghfirsname = null; }

                    try
                    {
                        emplbirtdate = DwMain.GetItemDate(1, "emplbirtdate");
                        birtdate_dd = emplbirtdate.Day.ToString();
                        birtdate_mm = emplbirtdate.Month.ToString();
                        birtdate_yyyy = emplbirtdate.Year.ToString();

                    }
                    catch (Exception ex) { ex.ToString(); }

                    try
                    {
                        emplciticarddate = DwMain.GetItemDate(1, "emplciticarddate");
                        citicarddate_dd = emplciticarddate.Day.ToString();
                        citicarddate_mm = emplciticarddate.Month.ToString();
                        citicarddate_yyyy = emplciticarddate.Year.ToString();
                    }
                    catch (Exception ex) { ex.ToString(); }

                    try
                    {
                        emplciticardexpidate = DwMain.GetItemDate(1, "emplciticardexpidate");
                        citicardexpdate_dd = emplciticardexpidate.Day.ToString();
                        citicardexpdate_mm = emplciticardexpidate.Month.ToString();
                        citicardexpdate_yyyy = emplciticardexpidate.Year.ToString();
                    }
                    catch (Exception ex) { ex.ToString(); }

                    try
                    {
                        emplbegndate = DwMain.GetItemDate(1, "emplbegndate");
                        begindate_dd = emplbegndate.Day.ToString();
                        begindate_mm = emplbegndate.Month.ToString();
                        begindate_yyyy = emplbegndate.Year.ToString();
                    }
                    catch (Exception ex) { ex.ToString(); }

                    try
                    {
                        emplprobdate = DwMain.GetItemDate(1, "emplprobdate");
                        probdate_dd = emplprobdate.Day.ToString();
                        probdate_mm = emplprobdate.Month.ToString();
                        probdate_yyyy = emplprobdate.Year.ToString();
                    }
                    catch (Exception ex) { ex.ToString(); }

                    try
                    {
                        getout_date = DwMain.GetItemDate(1, "getout_date");
                        getoutdate_dd = getout_date.Day.ToString();
                        getoutdate_mm = getout_date.Month.ToString();
                        getoutdate_yyyy = getout_date.Year.ToString();
                    }
                    catch (Exception ex) { ex.ToString(); }


                    try
                    {
                        last_date = DwMain.GetItemDate(1, "last_date");
                        lastdate_dd = last_date.Day.ToString();
                        lastdate_mm = last_date.Month.ToString();
                        lastdate_yyyy = last_date.Year.ToString();
                    }
                    catch (Exception ex) { ex.ToString(); }

                    try
                    {
                        entry_date = state.SsWorkDate;
                        entry_dd = entry_date.Day.ToString();
                        entry_mm = entry_date.Month.ToString();
                        entry_yyyy = entry_date.Year.ToString();
                    }
                    catch (Exception ex) { ex.ToString(); }

                    tDwMain.Eng2ThaiAllRow();

                    //================
                    String empl_new = null;
                    String empltype = DwMain.GetItemString(1, "empltypeid");
                    if (emplcode == null)
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอกข้อมูลรหัสพนักงาน");
                    }
                    else if (birth_tdate == null)
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอกข้อมูลวันเกิด");
                    }
                    else if (citycard_tdate == null)
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอกข้อมูลวันที่ออกบัตร");
                    }
                    else if (cityexp_tdate == null)
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอกข้อมูลวันที่หมดอายุ");
                    }
                    else if (begin_tdate == null)
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอกข้อมูลวันที่เริ่มงาน");
                    }

                    //else if (empltype == "01" && prob_tdate == null)
                    else if (prob_tdate == null)
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอกข้อมูลวันที่บรรจุ");
                    }
                    //else if (empltype != "01")
                    //{
                    //    DwMain.SetItemString(1, "prob_tdate","01012554");
                    //}
                    // กรณีเลือกสถานะเป็นลาออกจะมีการบันทึกข้อมูลวันที่ลาออกด้วย
                    else if (emplstat == "2")
                    {
                        //new record  
                        emplid = DwMain.GetItemString(1, "emplid");
                        try
                        {
                            Sta ta = new Sta(sqlca.ConnectionString);
                            String emplid_last = GetDocNo("HREMPLFILEMAS");
                            emplid_last = WebUtil.Right(emplid_last, 4);
                            empl_new = "EM" + GetYear("HREMPLFILEMAS") + emplid_last;
                            DwMain.SetItemString(1, "emplid", empl_new);

                            String sql = @"INSERT INTO HRNMLEMPLFILEMAS (picpath,emplid, emplcode, empltypeid, sideid, deptid, postid, prename_code, emplfirsname, empllastname, emplnickname, emplenghtitl, emplenghfirsname, emplenghlastname, emplenghnickname,emplsex, emplbloodgrup, emplweig, emplhigh, emplnation, emplorig, emplbirtprvn, empladdr1,emplprvn,   emplamph, empldist, empladdrpostcode, empltele, emplciticardaddr1, emplcitiid, email, emplciticardamph, emplstat, remkgetout, emplbirtcntry, emplbirtdate, emplciticarddate, emplciticardexpidate, emplbegndate, emplprobdate, getout_date, last_date, step_position, account, hrlevel, salary,  entry_id, entry_date, branch_id)";
                            sql += " VALUES ('" + picpath + "','" + empl_new.Trim() + "', '" + emplcode + "', '" + empltypeid + "' , '" + sideid + "', '" + deptid + "' ,'" + postid + "','" + prename_code + "', '" + emplfirsname + "', '" + empllastname + "' ,'" + emplnickname + "','" + emplenghtitl + "','" + emplenghfirsname + "','" + emplenghlastname + "','" + emplenghnickname + "','" + emplsex + "','" + emplbloodgrup + "','" + emplweig + "','" + emplhigh + "','" + emplnation + "' ,'" + emplorig + "','" + emplbirtprvn + "','" + empladdr1 + "','" + emplprvn + "','" + emplamph + "','" + empldist + "','" + empladdrpostcode + "','" + empltele + "','" + emplciticardaddr1 + "','" + emplcitiid + "','" + email + "','" + emplciticardamph + "','" + emplstat + "','" + remkgetout + "','" + emplbirtcntry + "',  to_date('" + birtdate_dd + "/" + birtdate_mm + "/" + birtdate_yyyy + "' ,'dd/mm/yyyy'), to_date('" + citicarddate_dd + "/" + citicarddate_mm + "/" + citicarddate_yyyy + "' ,'dd/mm/yyyy') ,to_date('" + citicardexpdate_dd + "/" + citicardexpdate_mm + "/" + citicardexpdate_yyyy + "' ,'dd/mm/yyyy'), to_date('" + begindate_dd + "/" + begindate_mm + "/" + begindate_yyyy + "' ,'dd/mm/yyyy') , to_date('" + probdate_dd + "/" + probdate_mm + "/" + probdate_yyyy + "' ,'dd/mm/yyyy'),  to_date('" + getoutdate_dd + "/" + getoutdate_mm + "/" + getoutdate_yyyy + "' ,'dd/mm/yyyy'), to_date('" + lastdate_dd + "/" + lastdate_mm + "/" + lastdate_yyyy + "' ,'dd/mm/yyyy'),'" + step_position + "','" + account + "','" + hrlevel + "','" + salary + "','" + state.SsUsername + "' ,  to_date('" + entry_dd + "/" + entry_mm + "/" + entry_yyyy + "' ,'dd/mm/yyyy') , '" + state.SsCoopId + "')";
                            ta.Exe(sql);

                            LtServerMessage.Text = WebUtil.CompleteMessage("บันทีกข้อมูลเสร็จเรียบร้อยแล้ว");
                            JspostNewClear();
                            ta.Close();
                        }
                        catch (Exception ex)
                        {
                            LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถบันทึกข้อมูลได้" + ex.Message);
                        }
                    }
                    //กรณีที่ไม่มีการเลือกสถานะเป็นลาออก จะไม่มีการบันทึกข้อมูลวันที่ลาออก
                    else if (emplstat != "2")
                    {
                        //new record
                        emplid = DwMain.GetItemString(1, "emplid");
                        try
                        {

                            Sta ta = new Sta(sqlca.ConnectionString);
                            String emplid_last = GetDocNo("HREMPLFILEMAS");
                            emplid_last = WebUtil.Right(emplid_last, 6);
                            empl_new = "EM" + GetYear("HREMPLFILEMAS") + emplid_last;
                            DwMain.SetItemString(1, "emplid", empl_new);

                            String sql = @"INSERT INTO HRNMLEMPLFILEMAS (picpath,emplid, emplcode, empltypeid, sideid, deptid, postid, prename_code, emplfirsname, empllastname, emplnickname, emplenghtitl, emplenghfirsname, emplenghlastname, emplenghnickname,emplsex, emplbloodgrup, emplweig, emplhigh, emplnation, emplorig, emplbirtprvn, empladdr1,emplprvn,   emplamph, empldist, empladdrpostcode, empltele, emplciticardaddr1, emplcitiid, email, emplciticardamph, emplstat, remkgetout, emplbirtcntry, emplbirtdate, emplciticarddate, emplciticardexpidate, emplbegndate, emplprobdate, step_position, account, hrlevel, salary, entry_id, entry_date, branch_id)";
                            sql += " VALUES ('" + picpath + "','" + empl_new.Trim() + "', '" + emplcode + "', '" + empltypeid + "' , '" + sideid + "', '" + deptid + "' ,'" + postid + "','" + prename_code + "', '" + emplfirsname + "', '" + empllastname + "' ,'" + emplnickname + "','" + emplenghtitl + "','" + emplenghfirsname + "','" + emplenghlastname + "','" + emplenghnickname + "','" + emplsex + "','" + emplbloodgrup + "','" + emplweig + "','" + emplhigh + "','" + emplnation + "' ,'" + emplorig + "','" + emplbirtprvn + "','" + empladdr1 + "','" + emplprvn + "','" + emplamph + "','" + empldist + "','" + empladdrpostcode + "','" + empltele + "','" + emplciticardaddr1 + "','" + emplcitiid + "','" + email + "','" + emplciticardamph + "','" + emplstat + "','" + remkgetout + "','" + emplbirtcntry + "',  to_date('" + birtdate_dd + "/" + birtdate_mm + "/" + birtdate_yyyy + "' ,'dd/mm/yyyy'), to_date('" + citicarddate_dd + "/" + citicarddate_mm + "/" + citicarddate_yyyy + "' ,'dd/mm/yyyy') ,to_date('" + citicardexpdate_dd + "/" + citicardexpdate_mm + "/" + citicardexpdate_yyyy + "' ,'dd/mm/yyyy'), to_date('" + begindate_dd + "/" + begindate_mm + "/" + begindate_yyyy + "' ,'dd/mm/yyyy') , to_date('" + probdate_dd + "/" + probdate_mm + "/" + probdate_yyyy + "' ,'dd/mm/yyyy'),'" + step_position + "','" + account + "','" + hrlevel + "','" + salary + "','" + state.SsUsername + "' ,  to_date('" + entry_dd + "/" + entry_mm + "/" + entry_yyyy + "' ,'dd/mm/yyyy') , '" + state.SsCoopId + "')";
                            ta.Exe(sql);

                            LtServerMessage.Text = WebUtil.CompleteMessage("บันทีกข้อมูลเสร็จเรียบร้อยแล้ว");
                            JspostNewClear();
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
                    //= emplid.Trim();
                    DwMain.UpdateData();
                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกการแก้ไขข้อมูลเสร็จเรียบร้อยแล้ว");
                    JspostNewClear();
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
            }
        }

        public void WebSheetLoadEnd()
        {
            try
            {
                DwUtil.RetrieveDDDW(DwMain, "deptid", "hr_master.pbl", null);
                DwUtil.RetrieveDDDW(DwMain, "postid", "hr_master.pbl", null);
                DwUtil.RetrieveDDDW(DwMain, "prename_code", "hr_master.pbl", null);
                //DwUtil.RetrieveDDDW(DwMain, "emplenghtitl", "hr_master.pbl", null);
                DwUtil.RetrieveDDDW(DwMain, "emplbloodgrup", "hr_master.pbl", null);
                DwUtil.RetrieveDDDW(DwMain, "emplprvn", "hr_master.pbl", null);
                DwUtil.RetrieveDDDW(dw_edudetail, "educlevl", "hr_master.pbl", null);
                
                //DwUtil.RetrieveDDDW(DwMain, "emplamph", "hr_master.pbl", null);
            }
            catch { }
            if (DwMain.RowCount > 1)
            {
                DwMain.DeleteRow(DwMain.RowCount);
            }
            tDwFamily.Thai2EngAllRow();
            tDwSeminar.Thai2EngAllRow();
            tDwWelfare.Thai2EngAllRow(); 
            DwMain.SaveDataCache();
            dw_family.SaveDataCache();
            dw_edudetail.SaveDataCache();
            dw_expdetail.SaveDataCache();
            dw_siminardetail.SaveDataCache();
            dw_weldetail.SaveDataCache();
            
        }

    }
}