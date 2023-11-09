using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommonLibrary;
using CommonLibrary.WsMis;
using System.Web.Services.Protocols;
using Sybase.DataWindow;
using DBAccess;
using System.IO;
using Sybase.DataWindow.Web;

namespace Saving.Applications.app_assist
{
    public partial class w_sheet_mis_export_pension1 : PageWebSheet, WebSheet
    {
        DataStore DStore;
        private Mis MisService;
        protected String postNewClear;
        protected String jsButtonShowData;
        protected String expExcel;
        protected String postFlag;
        private DwThDate tDwMain;

        private void JspostNewClear()
        {
            Dw_main.Reset();
            Dw_main.InsertRow(0);
            Dw_main1.Reset();
            Dw_main1.InsertRow(0);

            Dw_main.SetItemDateTime(1, "start_date", state.SsWorkDate);
            tDwMain.Eng2ThaiAllRow();

        }

        public void InitJsPostBack()
        {
            postNewClear = WebUtil.JsPostBack(this, "postNewClear");
            jsButtonShowData = WebUtil.JsPostBack(this, "jsButtonShowData");
            expExcel = WebUtil.JsPostBack(this, "expExcel");
            postFlag = WebUtil.JsPostBack(this, "postFlag");
            //=========================
            tDwMain = new DwThDate(Dw_main, this);
            tDwMain.Add("start_date", "start_tdate");
            tDwMain.Add("end_date", "end_tdate");
        }

        public void WebSheetLoadBegin()
        {
            tDwMain.Eng2ThaiAllRow();
            try
            {
                this.ConnectSQLCA();
            }
            catch { LtServerMessage.Text = WebUtil.CompleteMessage("ติดต่อ Database ไม่ได้"); }

            try
            {
                MisService = WsCalling.Mis;
            }
            catch { LtServerMessage.Text = WebUtil.CompleteMessage("ติดต่อ Service ไม่ได้"); }

            Dw_main.SetTransaction(sqlca);
            Dw_main1.SetTransaction(sqlca);

            if (!IsPostBack)
            {
                JspostNewClear();
            }
            else
            {
                this.RestoreContextDw(Dw_main);
                this.RestoreContextDw(Dw_main1);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postNewClear")
            {
                JspostNewClear();
            }
            else if (eventArg == "jsButtonShowData")
            {
                JsButtonShowData();
            }
            else if (eventArg == "expExcel")
            {
                SaveFile();
            }
            else if (eventArg == "postFlag")
            {
                //JsButtonShowData();
            }
     
            
        }

        public void SaveWebSheet()
        {

        }

        public void WebSheetLoadEnd()
        {
            Dw_main.SaveDataCache();
            Dw_main1.SaveDataCache();
        }

        private void JsButtonShowData()
        {
            //Button1.Enabled = true;
            string start_tdate;
            try
            {
                start_tdate = Dw_main.GetItemString(1, "start_tdate");
                //end_tdate = Dw_main.GetItemString(1, "end_tdate");
                DateTime startDate = DateTime.ParseExact(start_tdate, "ddMMyyyy", WebUtil.TH);//Dw_main.GetItemDateTime(1, "start_date");
                //DateTime endDate = DateTime.ParseExact(start_tdate, "ddMMyyyy", WebUtil.TH);//Dw_main.GetItemDateTime(1, "end_date");
                Dw_main1.Retrieve(startDate);
            }
            catch (Exception ex)
            {

            }
        }

        private void SaveFile()
        {
            String report_format = Dw_main.GetItemString(1, "report_format");
            if (report_format == "TXT")
            {
                try
                {
                    str_rptexcel astr_rptexcel = new str_rptexcel();
                    astr_rptexcel.as_xmldw = Dw_main1.Describe("DataWindow.Data.XML");
                    //int result = MisService.of_dwexportexcel_etn(state.SsWsPass, astr_rptexcel);
                    if (astr_rptexcel.as_xmldw != "")
                    {
                        String xml_detail = astr_rptexcel.as_xmldw;
                        String filename = "Pension(1)" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                        String path = WebUtil.GetStoreFile(state.SsApplication, "sms_text\\" + filename);
                        
                        DStore = new DataStore();
                        DStore.LibraryList = WebUtil.PhysicalPath + @"Saving\DataWindow\mis\smsobjects.pbl";
                        DStore.DataWindowObject = "d_export_egat_sms_request_pension1";
                        DStore.ImportString(xml_detail, FileSaveAsType.Xml);
                        int row = DStore.RowCount;
                        int i = 1;
                        string a, b, c, d, e, f, g, h;
                        String total = "";
                        StreamWriter writer = new StreamWriter("C:\\TEMP\\" + filename,false,System.Text.Encoding.GetEncoding(874));
                        while (i < row + 1)
                        {
                            try
                            {
                                a = DStore.GetItemString(i, "mem_no");
                            }
                            catch
                            {
                                a = "";
                            }
                            try
                            {
                                b = DStore.GetItemString(i, "mem_name");
                            }
                            catch
                            {
                                b = "";
                            }
                            try
                            {
                                c = DStore.GetItemString(i, "mem_telmobile");
                            }
                            catch
                            {
                                c = "";
                            }
                            try
                            {
                                d = DStore.GetItemString(i, "text1");
                            }
                            catch
                            {
                                d = "";
                            }
                            try
                            {
                                e = DStore.GetItemString(i, "asnslippayout_payout_amt");
                            }
                            catch
                            {
                                e = "";
                            }
                            try
                            {
                                f = DStore.GetItemString(i, "text2");
                            }
                            catch
                            {
                                f = "";
                            }
                            try
                            {
                                g = DStore.GetItemString(i, "str_date");
                            }
                            catch
                            {
                                g = "";
                            }
                            try
                            {
                                h = DStore.GetItemString(i, "mbmembmaster_text3");
                            }
                            catch
                            {
                                h = "";
                            }
                            total = a + "," + b + "," + c + "," + d + "," + e + "," + f + "," + g + "," + h;
                            writer.WriteLine(total.Replace(Environment.NewLine, "<br/>"));
                            i++;

                        }
                        writer.Close();
                        JspostNewClear();

                        string path2 = WebUtil.CreateLinkDownload(state.SsApplication, "sms_text/" + filename);
                        //LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลสำเร็จ คุณสามารถดาวน์โหลดไฟล์ได้ที่นี่  <a href=\"" + "C:\\TEMP\\" + filename + "\" target='_blank'>" + filename + "</a>");
                        LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลสำเร็จ คุณสามารถดาวน์โหลดไฟล์ได้ที่C:\\TEMP\\" + filename);                  
                        //SaveTest(filename);
                    }
                    else
                    {
                        //   HdExportFile.Value = "";
                        LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบข้อมูล กรุณาตรวจสอบใหม่");
                        JspostNewClear();
                    }

                }
                catch (SoapException ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("เกิดข้อผิดพลาด " + WebUtil.SoapMessage(ex));
                }


                

            }
            else if (report_format == "XLS")
            {
                try
                {
                    str_rptexcel astr_rptexcel = new str_rptexcel();
                    astr_rptexcel.as_xmldw = Dw_main1.Describe("DataWindow.Data.XML");
                    int result = MisService.of_dwexportexcel_etn(state.SsWsPass, astr_rptexcel);
                    if (astr_rptexcel.as_xmldw != "")
                    {
                        String xml_detail = astr_rptexcel.as_xmldw;
                        String filename = "Pension(1)" + DateTime.Now.ToString("yyyyMMdd") + ".xls";
                        String path = WebUtil.GetStoreFile(state.SsApplication, "sms_excel\\" + filename);

                        DStore = new DataStore();
                        DStore.LibraryList = WebUtil.PhysicalPath + @"Saving\DataWindow\mis\smsobjects.pbl";
                        DStore.DataWindowObject = "d_export_egat_sms_request_pension1";
                        DStore.ImportString(xml_detail, FileSaveAsType.Xml);
                        int row = DStore.RowCount;
                        int i = 1;
                        string a, b, c, d, e, f, g, h;
                        StreamWriter writer = new StreamWriter(path);
                        while (i < row + 1)
                        {
                            try
                            {
                                a = DStore.GetItemString(i, "mem_no");
                            }
                            catch
                            {
                                a = "";
                            }
                            try
                            {
                                b = DStore.GetItemString(i, "mem_name");
                            }
                            catch
                            {
                                b = "";
                            }
                            try
                            {
                                c = DStore.GetItemString(i, "mem_telmobile");
                            }
                            catch
                            {
                                c = "";
                            }
                            try
                            {
                                d = DStore.GetItemString(i, "text1");
                            }
                            catch
                            {
                                d = "";
                            }
                            try
                            {
                                e = DStore.GetItemString(i, "asnslippayout_payout_amt");
                            }
                            catch
                            {
                                e = "";
                            }
                            try
                            {
                                f = DStore.GetItemString(i, "text2");
                            }
                            catch
                            {
                                f = "";
                            }
                            try
                            {
                                g = DStore.GetItemString(i, "str_date");
                            }
                            catch
                            {
                                g = "";
                            }
                            try
                            {
                                h = DStore.GetItemString(i, "mbmembmaster_text3");
                            }
                            catch
                            {
                                h = "";
                            }
                            writer.Write(a);
                            writer.Write('\t');
                            writer.Write(b);
                            writer.Write('\t');
                            writer.Write(c);
                            writer.Write('\t');
                            writer.Write(d);
                            writer.Write('\t');
                            writer.Write(e);
                            writer.Write('\t');
                            writer.Write(f);
                            writer.Write('\t');
                            writer.Write(g);
                            writer.Write('\t');
                            writer.WriteLine(h.Replace(Environment.NewLine, "<br/>"));
                            i++;

                        }
                        writer.Close();
                        JspostNewClear();
                        string path2 = WebUtil.CreateLinkDownload(state.SsApplication, "sms_excel/" + filename);
                        LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลสำเร็จ คุณสามารถดาวน์โหลดไฟล์ได้ที่นี่  <a href=\"" + path2 + "\" target='_blank'>" + filename + "</a>");
                    }
                    else
                    {
                        //   HdExportFile.Value = "";
                        LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบข้อมูล กรุณาตรวจสอบใหม่");
                        JspostNewClear();
                    }

                }
                catch (SoapException ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("เกิดข้อผิดพลาด " + WebUtil.SoapMessage(ex));
                }
            }
        }

        public static string MimeType(string Extension)
        {
            string mime = "application/octetstream";
            if (string.IsNullOrEmpty(Extension))
                return mime;
            string ext = Extension.ToLower();
            Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (rk != null && rk.GetValue("Content Type") != null)
                mime = rk.GetValue("Content Type").ToString();
            return mime;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            SaveFile();
            string FileName = "Pension(1)" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            string fName = @"C:\\TEMP\\" + FileName;
            FileInfo fi = new FileInfo(fName);
            long sz = fi.Length;

            Response.ClearContent();
            Response.ContentType = MimeType(Path.GetExtension(fName));
            Response.AddHeader("Content-Disposition", string.Format("attachment; filename = {0}", System.IO.Path.GetFileName(fName)));
            Response.AddHeader("Content-Length", sz.ToString("F0"));
            Response.TransmitFile(fName);
            Response.End();
        }

        protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBox1.Checked == true)
            {
                Button1.Enabled = true;
            }
            else
                Button1.Enabled = false;
        }

        //protected void PostPrint()
        //{
        //    string FileName = "Pension(1)" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
        //    string fName = @"C:\\TEMP\\" + FileName;
        //    FileInfo fi = new FileInfo(fName);
        //    long sz = fi.Length;

        //    Response.ClearContent();
        //    Response.ContentType = MimeType(Path.GetExtension(fName));
        //    Response.AddHeader("Content-Disposition", string.Format("attachment; filename = {0}", System.IO.Path.GetFileName(fName)));
        //    Response.AddHeader("Content-Length", sz.ToString("F0"));
        //    Response.TransmitFile(fName);
        //    Response.End();
        //    WebSheetLoadEnd();
        //}
    }
}