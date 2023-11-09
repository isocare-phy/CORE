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
using CoreSavingLibrary.WcfAgency;
using CoreSavingLibrary.WcfCommon;
using System.Web.Services.Protocols;
using DataLibrary;
using System.Globalization;
using System.IO;
using DataLibrary;

namespace Saving.Applications.agency
{
    public partial class w_sheet_ag_reqagmemb : PageWebSheet, WebSheet
    {

        private AgencyClient AgencyService;
        private DwThDate tDw_main;
        protected String postNewClear;
        protected String postInitReqAgMemb;
        protected String postChgReqAgMemb;
        protected String postSearchChgReqAgMemb;
        protected String postResign;
        protected String postReturn;
        protected String postSetAddress;
        //============================================//
        private void JspostSetAddress()
        {
            String memb_addr = null;
            String mooban = null;
            String soi = null;
            String road = null;
            String tambol = null;
            String district_code = null;
            String province_code = null;
            String postcode = null;

            try { 
                memb_addr = Dw_main.GetItemString(1, "memb_addr").Trim();
            }
            catch { memb_addr = null; }

            try {
                mooban = Dw_main.GetItemString(1, "mooban").Trim();
            }
            catch { mooban = null; }

            try { soi = Dw_main.GetItemString(1, "soi").Trim();
                soi = " ซอย :" + soi;
            }
            catch { soi = null; }

            try { road = Dw_main.GetItemString(1, "road").Trim();
                  road = " ถนน :" + road;
            }
            catch { road = null; }

            try { tambol = Dw_main.GetItemString(1, "tambol").Trim();
            }
            catch { tambol = null; }

            try { district_code = Dw_main.GetItemString(1, "district_code").Trim();
            }
            catch { district_code = null; }

            try { province_code = Dw_main.GetItemString(1, "province_code").Trim();
            }
            catch { province_code = null; }

            try { postcode = Dw_main.GetItemString(1, "postcode").Trim(); }
            catch { postcode = null; }

            String province_desc = null;
            String distrinct_desc = null;
            String tambol_desc = null;
            //String district = Dw_main.GetItemString(1, "district_code").Trim();

            // หาชื่อจังหวัด
            if (province_code == null || province_code == "")
            {
            
            }
            else
            {
                Sta ta = new Sta(sqlca.ConnectionString);
                try
                {
                    String sql = @"select province_desc from mbucfprovince where province_code = '" + province_code + "'";
                    Sdt dt = ta.Query(sql);
                    if (dt.Next())
                    {
                        province_desc = dt.GetString("province_desc").Trim();
                    }
                    else
                    {
                        sqlca.Rollback();
                    }
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
                }
            }

            if (province_code == null || district_code == null )
            {
            
            }
            else
            {
                // หาชื่ออำเภอ
                Sta ta1 = new Sta(sqlca.ConnectionString);
                try
                {
                    String district_full = province_code + district_code;
                    String sql1 = @"select district_desc from mbucfdistrict where province_code = '" + province_code + "' and district_code = '" + district_full + "'";
                    Sdt dt1 = ta1.Query(sql1);
                    if (dt1.Next())
                    {
                        distrinct_desc = dt1.GetString("district_desc").Trim();
                    }
                    else
                    {
                        sqlca.Rollback();
                    }
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
                }
            }

            if (tambol == null || tambol == "")
            {

            }
            else
            {
                if (tambol.Length == 2)
                {
                    // หาชื่อตำบล
                    Sta ta2 = new Sta(sqlca.ConnectionString);
                    try
                    {
                        String district_full = province_code + district_code;
                        String tambol_full = district_full + tambol;
                        String sql2 = @"select tambol_desc from mbucftambol where district_code = '" + district_full + "' and tambol_code = '" + tambol_full + "'";
                        Sdt dt2 = ta2.Query(sql2);
                        if (dt2.Next())
                        {
                            tambol_desc = dt2.GetString("tambol_desc").Trim();
                            tambol = tambol_desc;
                        }
                        else
                        {
                            sqlca.Rollback();
                        }
                    }
                    catch (Exception ex)
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
                    }
                }
            }
            String  address = memb_addr + ' ' + mooban + ' ' + soi + ' ' + road + ' ' + tambol + ' ' + distrinct_desc + ' ' + province_desc + ' ' + postcode;
            Dw_main.SetItemString(1, "address", address);
        }

        private void JspostResign()
        {
            try
            {
                str_agent astr_agent = new str_agent();
                astr_agent.xml_head = Dw_main.Describe("DataWindow.Data.XML");
                astr_agent.column = "b_resign";
                astr_agent.row = 1;
                int result = AgencyService.ChgReqAgMemb(state.SsWsPass, ref astr_agent);
                if (result == 1)
                {
                    String xml_detail = astr_agent.xml_head;
                    Dw_main.Reset();
                    Dw_main.ImportString(xml_detail, FileSaveAsType.Xml);
                }
                else
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบข้อมูล กรุณาตรวจสอบใหม่");
                    JspostNewClear();
                }
            }
            catch (SoapException ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("เกิดข้อผิดพลาด " + WebUtil.SoapMessage(ex));
            }
            catch (Exception ex)
            {
                // LtServerMessage.Text = WebUtil.ErrorMessage("เกิดข้อผิดพลาด " + ex);
                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบข้อมูล กรุณาทำรายการค้นหาใหม่");
                JspostNewClear();
            }
        }

        private void JspostReturn()
        {
            try
            {
                str_agent astr_agent = new str_agent();
                astr_agent.xml_head = Dw_main.Describe("DataWindow.Data.XML");
                astr_agent.column = "b_ccresign";
                astr_agent.row = 1;
                int result = AgencyService.ChgReqAgMemb(state.SsWsPass, ref astr_agent);
                if (result == 1)
                {
                    String xml_detail = astr_agent.xml_head;
                    Dw_main.Reset();
                    Dw_main.ImportString(xml_detail, FileSaveAsType.Xml);
                }
                else
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบข้อมูล กรุณาตรวจสอบใหม่");
                    JspostNewClear();
                }
            }
            catch (SoapException ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("เกิดข้อผิดพลาด " + WebUtil.SoapMessage(ex));
            }
            catch (Exception ex)
            {
                // LtServerMessage.Text = WebUtil.ErrorMessage("เกิดข้อผิดพลาด " + ex);
                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบข้อมูล กรุณาทำรายการค้นหาใหม่");
                JspostNewClear();
            }
        }
        private void JspostPicture()
        {
            String picture_name = null;
            String signature_name = null;
            try
            {
                picture_name = Dw_main.GetItemString(1, "picture_name");
            }
            catch
            {
                picture_name = null;
            }

            try
            {
                signature_name = Dw_main.GetItemString(1, "signature_name");
            }
            catch
            {
                signature_name = null;
            }

            if (picture_name != null)
            {
                Img_picture.Width = 120;
                Img_picture.ImageUrl = picture_name;
            }

            if (signature_name != null)
            {
                Img_signature.Width = 120;
                Img_signature.ImageUrl = signature_name;
            }
        }

        private void JspostUpLoadPicture()
        {
            //Browse new Member picture
            String agentrequest_no = Dw_main.GetItemString(1, "agentrequest_no");
            if ((FileUpload1.PostedFile != null) && (FileUpload1.PostedFile.ContentLength > 0))
            {
                try
                {
                    String filename1 = Path.GetFileName(FileUpload1.PostedFile.FileName);
                    String path_img1 = Server.MapPath("") + "\\image\\picture\\" + filename1;
                    String url_img1 = state.SsUrl + "Applications\\agency\\image\\picture\\" + filename1.Replace("\\", "/");
                    FileUpload1.SaveAs(path_img1);
                    Img_picture.Width = 120;
                    Img_picture.ImageUrl = url_img1;
                    Dw_main.SetItemString(1, "picture_name", url_img1);
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถอัพโหลดรูปภาพได้");
                }
            }

            if ((FileUpload2.PostedFile != null) && (FileUpload2.PostedFile.ContentLength > 0))
            {
                try
                {
                    String filename2 = Path.GetFileName(FileUpload2.PostedFile.FileName);
                    String path_img2 = Server.MapPath("") + "\\image\\signature\\" + filename2;
                    String url_img2 = state.SsUrl + "Applications\\agency\\image\\signature\\" + filename2.Replace("\\", "/");
                    FileUpload2.SaveAs(path_img2);
                    Img_picture.Width = 120;
                    Img_picture.ImageUrl = url_img2;
                    Dw_main.SetItemString(1, "signature_name", url_img2);
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถอัพโหลดรูปภาพได้");
                }
            }
        }
        //Init จาก Mbmembmaster
        private void JspostChgReqAgMemb()
        {
            Hd_resign.Value = "";
            try
            {
                str_agent astr_agent = new str_agent();
                astr_agent.xml_head = Dw_main.Describe("DataWindow.Data.XML");
                astr_agent.column = "member_no";
                astr_agent.row = 1;
                int result = AgencyService.ChgReqAgMemb(state.SsWsPass, ref astr_agent);
                if (result == 1)
                {
                    String xml_detail = astr_agent.xml_head;
                    Dw_main.Reset();
                    Dw_main.ImportString(xml_detail, FileSaveAsType.Xml);
                    JspostSetAddress();
                    tDw_main.Eng2ThaiAllRow();
                }
                else
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบข้อมูล กรุณาตรวจสอบใหม่");
                    JspostNewClear();
                }
            }
            catch (SoapException ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("เกิดข้อผิดพลาด " + WebUtil.SoapMessage(ex));
            }
            catch (Exception ex)
            {
                // LtServerMessage.Text = WebUtil.ErrorMessage("เกิดข้อผิดพลาด " + ex);
                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบข้อมูล กรุณาทำรายการค้นหาใหม่");
                JspostNewClear();
            }
        }

        //SearchInit จาก Mbmembmaster
        private void JspostSearchChgReqAgMemb()
        {
            Hd_resign.Value = "";
            try
            {
                str_agent astr_agent = new str_agent();
                astr_agent.xml_head = Dw_main.Describe("DataWindow.Data.XML");
                astr_agent.column = "member_no";
                astr_agent.row = 1;
                int result = AgencyService.ChgReqAgMemb(state.SsWsPass, ref astr_agent);
                if (result == 1)
                {
                    String xml_detail = astr_agent.xml_head;
                    Dw_main.Reset();
                    Dw_main.ImportString(xml_detail, FileSaveAsType.Xml);
                    tDw_main.Eng2ThaiAllRow();
                }
                else
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบข้อมูล กรุณาตรวจสอบใหม่");
                    JspostNewClear();
                }
            }
            catch (SoapException ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("เกิดข้อผิดพลาด " + WebUtil.SoapMessage(ex));
            }
            catch (Exception ex)
            {
                // LtServerMessage.Text = WebUtil.ErrorMessage("เกิดข้อผิดพลาด " + ex);
                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบข้อมูล กรุณาทำรายการค้นหาใหม่");
                JspostNewClear();
            }
        }

        //Init จากตารางตัวแทน
        private void JspostInitReqAgMemb()
        {
           // Hd_resign.Value = "resign";
            try
            {
                str_agent astr_agent = new str_agent();

                astr_agent.xml_head = Dw_main.Describe("DataWindow.Data.XML");
                int result = AgencyService.InitReqAgMemb(state.SsWsPass, ref astr_agent);
                if (result == 1)
                {
                    String xml_detail = astr_agent.xml_head;
                    Dw_main.Reset();
                    Dw_main.ImportString(xml_detail, FileSaveAsType.Xml);
                    tDw_main.Eng2ThaiAllRow();
                    JspostSetAddress();
                    JspostPicture();
                }
                else
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบข้อมูล กรุณาตรวจสอบใหม่");
                    JspostNewClear();
                }
            }
            catch (SoapException ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("เกิดข้อผิดพลาด " + WebUtil.SoapMessage(ex));
            }
            catch (Exception ex)
            {
                // LtServerMessage.Text = WebUtil.ErrorMessage("เกิดข้อผิดพลาด " + ex);
                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบข้อมูล กรุณาทำรายการค้นหาใหม่");
                JspostNewClear();
            }
        }

        private void JspostNewClear()
        {
            Dw_main.Reset();
            Dw_main.InsertRow(0);

            Dw_main.SetItemDate(1, "agent_date", DateTime.Now);
            tDw_main.Eng2ThaiAllRow();
            Img_picture.ImageUrl = Img_picture.ImageUrl = state.SsUrl + "Applications\\agency\\image\\picture\\" + "icon_guest.jpg";
            Img_signature.ImageUrl = Img_signature.ImageUrl = state.SsUrl + "Applications\\agency\\image\\signature\\" + "icon_guest.jpg";
        }

       
        //============================================//

        #region WebSheet Members

        public void InitJsPostBack()
        {

            postNewClear = WebUtil.JsPostBack(this, "postNewClear");
            postInitReqAgMemb = WebUtil.JsPostBack(this, "postInitReqAgMemb");
            postChgReqAgMemb = WebUtil.JsPostBack(this, "postChgReqAgMemb");
            postSearchChgReqAgMemb = WebUtil.JsPostBack(this, "postSearchChgReqAgMemb");
            postResign = WebUtil.JsPostBack(this, "postResign");
            postReturn = WebUtil.JsPostBack(this, "postReturn");
            postSetAddress = WebUtil.JsPostBack(this, "postSetAddress");
            //=====================
            tDw_main = new DwThDate(Dw_main, this);
            tDw_main.Add("agent_date", "agent_tdate");

            DwUtil.RetrieveDDDW(Dw_main, "membgroup_code", "egat_ag_regagmemb.pbl", null);
            //DwUtil.RetrieveDDDW(Dw_main, "agentgrp_code", "egat_ag_regagmemb.pbl", null);
            DwUtil.RetrieveDDDW(Dw_main, "prename_code", "egat_ag_regagmemb.pbl", null);
        }

        public void WebSheetLoadBegin()
        {
            Hd_resign.Value = "";
            this.ConnectSQLCA();

            try
            {
                AgencyService = wcf.Agency;
            }
            catch { LtServerMessage.Text = WebUtil.CompleteMessage("ติดต่อ Service ไม่ได้"); }

            if (!IsPostBack)
            {
                Dw_main.Reset();
                Dw_main.InsertRow(0);

                Dw_main.SetItemDate(1, "agent_date", DateTime.Now);
                tDw_main.Eng2ThaiAllRow();
           
                
            }
            else
            {
                this.RestoreContextDw(Dw_main);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postNewClear")
            {
                JspostNewClear();
            }
            else if (eventArg == "postInitReqAgMemb")
            {
                JspostInitReqAgMemb();
            }
            else if (eventArg == "postChgReqAgMemb")
            {
                JspostChgReqAgMemb();
            }

            else if (eventArg == "postSearchChgReqAgMemb")
            {
                JspostSearchChgReqAgMemb();
            }
            else if (eventArg == "postResign")
            {
                JspostResign();
            }
            else if (eventArg == "postReturn")
            {
                JspostReturn();
            }
            else if (eventArg == "postSetAddress")
            {
                JspostSetAddress();
            }
        }

        public void SaveWebSheet()
        {

            String member_no = null;
            try 
            {
                member_no = Dw_main.GetItemString(1, "member_no");
            }
            catch { member_no = null; }

            if (FileUpload1.PostedFile.ContentLength == 0 && FileUpload2.PostedFile.ContentLength == 0)
            {
                try
                {
                    str_agent astr_agent = new str_agent();
                    astr_agent.xml_head = Dw_main.Describe("DataWindow.Data.XML");
                    int result = AgencyService.SaveReqAgMemb(state.SsWsPass, astr_agent);
                    if (result == 1)
                    {
                        LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลเสร็จเรียบร้อยแล้ว");
                        JspostNewClear();
                    }
                    else
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถบันทึกข้อมูลได้");
                    }
                }
                catch (SoapException ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("เกิดข้อผิดพลาด " + WebUtil.SoapMessage(ex));
                }
                catch (Exception ex)
                {
                    JspostNewClear();
                }
            }
            else
            {
                JspostUpLoadPicture();

                try
                {
                    str_agent astr_agent = new str_agent();
                    astr_agent.xml_head = Dw_main.Describe("DataWindow.Data.XML");
                    int result = AgencyService.SaveReqAgMemb(state.SsWsPass, astr_agent);
                    if (result == 1)
                    {
                        LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลเสร็จเรียบร้อยแล้ว");
                        JspostNewClear();
                    }
                    else
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถบันทึกข้อมูลได้");
                    }
                }
                catch (SoapException ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("เกิดข้อผิดพลาด " + WebUtil.SoapMessage(ex));
                }
                catch (Exception ex)
                {
                    JspostNewClear();
                }
            }
            

            
        }

        public void WebSheetLoadEnd()
        {
            Dw_main.SaveDataCache();
            if (Dw_main.RowCount > 1)
            {
                Dw_main.DeleteRow(Dw_main.RowCount);
            }

            

        }

        #endregion

        protected void B_resign_Click(object sender, EventArgs e)
        {
           
            Hd_resign.Value = "";
           
        }
    }
}
