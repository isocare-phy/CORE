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
using System.Data.OracleClient; //เพิ่มเข้ามา
using Sybase.DataWindow;//เพิ่มเข้ามา
using System.Globalization; //เพิ่มเข้ามา
using DataLibrary; //เพิ่มเข้ามา
using CoreSavingLibrary.WcfNAccount;
using System.Web.Services.Protocols; //เรียกใช้ service
using System.Threading;

namespace Saving.Applications.account
{
    public partial class w_sheet_vc_voucher_tks : PageWebSheet, WebSheet
    {
        private string is_sql;
        protected String postInsertDetail;
        protected String postDeleteDetail;
        private CultureInfo th;
        private DwThDate tdw_list;
        private DwThDate tdw_main;
        private DwThDate tdw_detail;
        private DwThDate tdw_date;
        private DwThDate tdw_search;
        private n_accountClient accService;//ประกาศตัวแปร WebService

        //ประกาศ JavaScript Postback
        protected String postSelectList;
        protected String postVoucherDate;
        protected String postW_dlg_Click;
        protected String postNewClear;
        protected String postSearchVoucher;
        //protected String postPrint;
        protected String postAccidDtail;
        protected String app;
        protected String gid;
        protected String rid;
        protected String pdf;

        string pbl = "vc_voucher_edit.pbl";


        // เพิ่มแถว รายละเอียด
        private void JspostInsertDetail()
        {
            int row2 = Dw_detail.InsertRow(0);
            //int row = Convert.ToInt16(Hd_row.Value);

            String account_id = Dw_main.GetItemString(1, "vcvoucherdet_account_id");
            decimal seq_no = Dw_main.GetItemDecimal(1, "seq_no");
            DateTime VcDate = Dw_date.GetItemDateTime(1, "voucher_date");
            String account_nature = Dw_date.GetItemString(1, "acc_side");
            String voucher_no = Dw_main.GetItemString(1, "voucher_no");
            String accledgroup_typ = "00";


            Dw_detail.SetItemString(row2, "account_id", account_id);
            Dw_detail.SetItemString(row2, "coop_id", state.SsCoopId);
            Dw_detail.SetItemDecimal(row2, "seq_no", seq_no);
            Dw_detail.SetItemDateTime(row2, "journal_date", VcDate);
            Dw_detail.SetItemString(row2, "account_nature", account_nature);
            Dw_detail.SetItemString(row2, "voucher_no", voucher_no);
            Dw_detail.SetItemString(row2, "accledgroup_typ", accledgroup_typ);
        }

        private void JspostDeleteDetail()
        {
            try
            {
                Int16 RowDetail = Convert.ToInt16(Hd_row.Value);
                //Dw_detail.DeleteRow(RowDetail);
                //Dw_detail.UpdateData();

                String account_id = Dw_main.GetItemString(1, "vcvoucherdet_account_id");
                decimal seq_no = Dw_main.GetItemDecimal(1, "seq_no");
                DateTime VcDate = Dw_date.GetItemDateTime(1, "voucher_date");
                String account_nature = Dw_date.GetItemString(1, "acc_side");
                String voucher_no = Dw_main.GetItemString(1, "voucher_no");
                decimal order_no = Dw_detail.GetItemDecimal(RowDetail, "order_no");
                string sql = "delete from accledger_detail where voucher_no = '" + voucher_no + "' and account_id = '" + account_id + "' and account_nature = '" + account_nature + "' and seq_no = '" + seq_no + "' and order_no = " + order_no + "";
                int delete = WebUtil.ExeSQL(sql);
                //DwUtil.UpdateDataWindow(Dw_detail, "vc_voucher_edit.pbl", "accledger_detail");
                //Dw_detail.UpdateData();

               LtServerMessage.Text = WebUtil.CompleteMessage("ลบข้อมูลเสร็จเรียบร้อยแล้ว");
               int row = int.Parse(Hd_row.Value);
               String voucherno = Dw_list.GetItemString(row, "voucher_no");
               String account_id2 = Dw_list.GetItemString(row, "account_id");
               String acc_side = Dw_date.GetItemString(1, "acc_side");
               DwUtil.RetrieveDataWindow(Dw_detail, pbl, null, voucherno, account_id2, acc_side);

            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
            }
        }


        //#region WebSheet Members


        //JS-Event
        private void JspostSearchVoucher()
        {
            //String vc_no = Dw_find.GetItemString(1, "voucher_no");

            //try
            //{
            //    int li_row = Dw_list.FindRow("voucher_no = '" + vc_no.Trim() + "'", 0, Dw_list.RowCount);
            //    if (li_row > 0)
            //    {
            //        //retrieve Master Detail
            //        try
            //        {
            //            String voucher_no = "";
            //            voucher_no = Dw_list.GetItemString(li_row, "voucher_no").Trim();
            //            String as_vcmas_xml = Dw_main.Describe("Datawindow.Data.Xml");
            //            String as_vcdet_xml = Dw_detail.Describe("Datawindow.Data.Xml");
            //            int result = accService.GetListVcMasDetail(state.SsWsPass, voucher_no, ref as_vcmas_xml, ref as_vcdet_xml);
            //            if (result == 1)
            //            {
            //                Dw_main.Reset();
            //                //Dw_detail.Reset();
            //                //Dw_footer.Reset();
            //                //Dw_main.ImportString(as_vcmas_xml, FileSaveAsType.Xml);
            //                Dw_detail.ImportString(as_vcdet_xml, FileSaveAsType.Xml);
            //                //Dw_footer.ImportString(as_vcdet_xml, FileSaveAsType.Xml);
            //            }

            //            Dw_list.SelectRow(0, false);
            //            Dw_list.SelectRow(li_row, true);
            //            Dw_list.SetRow(li_row);


            //        }
            //        catch (SoapException ex)
            //        {
            //            LtServerMessage.Text = WebUtil.ErrorMessage("เกิดข้อผิดพลาด " + WebUtil.SoapMessage(ex));
            //        }
            //        catch (Exception ex)
            //        {
            //            LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
            //        }


            //    }
            //    else
            //    {
            //        LtServerMessage.Text = WebUtil.CompleteMessage("ไม่พบข้อมูลรายการเลขที่ voucher ที่ค้นหา");
            //        Dw_find.Reset();
            //        Dw_find.InsertRow(0);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
            //}


        }



        private void JspostNewClear()
        {
            Dw_date.Reset();
            Dw_date.InsertRow(0);
            Dw_date.SetItemDate(1, "voucher_date", state.SsWorkDate);
            tdw_date.Eng2ThaiAllRow();


            Dw_list.Reset();


            Dw_main.Reset();
            Dw_main.InsertRow(0);


            Dw_detail.Reset();
            //Dw_footer.Reset();

            //Dw_find.Reset();
            //Dw_find.InsertRow(0);

            //lbl_moneybg.Text = "";
            //lbl_moneyfw.Text = "";
            //Panel1.Visible = false;

            JspostVoucherDate();

        }
        private void JspostSelectList()
        {
            DateTime VcDate = Dw_date.GetItemDateTime(1, "voucher_date");
            String acc_side = Dw_date.GetItemString(1, "acc_side");
            Dw_list.Retrieve(VcDate, acc_side, state.SsCoopControl);

        }


        private void JSpostAccidDtail()
        {
            int row = int.Parse(Hd_row.Value);
            String voucherno = Dw_list.GetItemString(row, "voucher_no");
            String account_id = Dw_list.GetItemString(row, "account_id");
            String acc_side = Dw_date.GetItemString(1, "acc_side");
            //Dw_main.Retrieve(voucherno, account_id);
            DwUtil.RetrieveDataWindow(Dw_main, pbl, null, voucherno, account_id);
            DwUtil.RetrieveDataWindow(Dw_detail, pbl, null, voucherno, account_id, acc_side);

        }

        private void JspostVoucherDate()
        {

            DateTime VcDate = Dw_date.GetItemDateTime(1, "voucher_date");
            String wsPass = state.SsWsPass;

            //ส่วนที่กำหนด จัดรูปแบบวันที่ให้กับ Dw_date
            Dw_main.SetItemDate(1, "voucher_date", VcDate);
            Dw_main.SetItemString(1, "voucher_tdate", VcDate.ToString("ddMMyyyy", new CultureInfo("th-TH")));
            tdw_date.Eng2ThaiAllRow();
            JspostSelectList();


        }



        #region WebSheet Members

        public void InitJsPostBack()
        {


            //ตั้งค่า JsPostBack
            postSelectList = WebUtil.JsPostBack(this, "postSelectList");
            postVoucherDate = WebUtil.JsPostBack(this, "postVoucherDate");
            postW_dlg_Click = WebUtil.JsPostBack(this, "postW_dlg_Click");
            postNewClear = WebUtil.JsPostBack(this, "postNewClear");
            postSearchVoucher = WebUtil.JsPostBack(this, "postSearchVoucher");
            //postPrint = WebUtil.JsPostBack(this, "postPrint");
            postAccidDtail = WebUtil.JsPostBack(this, "postAccidDtail");
            postInsertDetail = WebUtil.JsPostBack(this, "postInsertDetail");
            postDeleteDetail = WebUtil.JsPostBack(this, "postDeleteDetail");
            //การเรียกใช้ DwThDate
            tdw_date = new DwThDate(Dw_date, this);
            tdw_date.Add("voucher_date", "voucher_tdate");

            tdw_main = new DwThDate(Dw_main, this);
            tdw_main.Add("voucher_date", "voucher_tdate");


        }

        //  พิมพ์ใบเสร็จหน้าลงรายวัน
        public void PopupReportshr()
        {
            //Print();
            // Thread.Sleep(5000);
            Thread.Sleep(700);

            String pop = "Gcoop.OpenPopup('" + Session["pdf"].ToString() + "')";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "DsReport", pop, true);

        }

        //  พิมพ์ใบเสร็จหน้าลงรายวัน
        //private void Print()
        //{

        //    app = state.SsApplication;
        //    try
        //    {
        //        //gid = Request["gid"].ToString();
        //        gid = "acc_1_daily";
        //    }
        //    catch { }
        //    try
        //    {
        //        //rid = Request["rid"].ToString();
        //        rid = "[ACD090]";
        //    }
        //    catch { }



        //    String voucher_no = Dw_main.GetItemString(1, "voucher_no");
        //    String account_id = "";
        //    String sql_txt = "select cash_account_code from accconstant";
        //    DataTable dt = WebUtil.Query(sql_txt);

        //    if (dt.Rows.Count > 0)
        //    {
        //        account_id = dt.Rows[0][0].ToString().Trim();
        //    }
        //    //แปลง Criteria ให้อยู่ในรูปแบบมาตรฐาน.

        //    ReportHelper lnv_helper = new ReportHelper();

        //    lnv_helper.AddArgument(voucher_no, ArgumentType.String);
        //    lnv_helper.AddArgument(account_id, ArgumentType.String);


        //    //ชื่อไฟล์ PDF = YYYYMMDDHHMMSS_<GID>_<RID>.PDF
        //    String pdfFileName = DateTime.Now.ToString("yyyyMMddHHmmss", WebUtil.EN);
        //    pdfFileName += "_" + gid + "_" + rid + ".pdf";
        //    pdfFileName = pdfFileName.Trim();
        //    //ส่งให้ ReportService สร้าง PDF ให้ {โดยปกติจะอยู่ใน C:\GCOOP\Saving\PDF\}.
        //    try
        //    {
        //        //CoreSavingLibrary.WcfReport.ReportClient lws_report = wcf.Report;
        //        //String criteriaXML = lnv_helper.PopArgumentsXML();
        //        //this.pdf = lws_report.GetPDFURL(state.SsWsPass) + pdfFileName;
        //        String li_return = lws_report.Run(state.SsWsPass, app, gid, rid, criteriaXML, pdfFileName);
        //        if (li_return == "true")
        //        {
        //            HdOpenIFrame.Value = "True";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LtServerMessage.Text = WebUtil.ErrorMessage(ex);
        //        return;
        //    }
        //    Session["pdf"] = pdf;
        //    // PopupReport();
        //}



        public void WebSheetLoadBegin()
        {

            this.ConnectSQLCA();
            accService = wcf.NAccount;//ประกาศ new
            th = new CultureInfo("th-TH");

            Dw_date.SetTransaction(sqlca);
            Dw_list.SetTransaction(sqlca);
            Dw_main.SetTransaction(sqlca);
            Dw_detail.SetTransaction(sqlca);
            //Dw_find.SetTransaction(sqlca);
            //Dw_footer.SetTransaction(sqlca);

            ////ถ้าเป็น tks ให้มีปุ่ม Print หน้าลงรายวัน
            //if (state.SsCoopId == "010001")
            //{
            //    print2.Visible = true;

            //}

            //else
            //{
            //    print2.Visible = false;
            //}


            if (!IsPostBack)
            {
                JspostNewClear();
                //Dw_date.InsertRow(0);
                //Dw_main.InsertRow(0);
                //Dw_find.InsertRow(0);
                //HdBranchId.Value = state.SsCoopId;
                //Panel1.Visible = false;
            }
            else
            {
                this.RestoreContextDw(Dw_date);
                this.RestoreContextDw(Dw_list);
                this.RestoreContextDw(Dw_main);
                this.RestoreContextDw(Dw_detail);
                //this.RestoreContextDw(Dw_find);
                //this.RestoreContextDw(Dw_footer);

            }


        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postSelectList")
            {
                JspostSelectList();
            }
            else if (eventArg == "postVoucherDate")
            {
                JspostVoucherDate();
            }
            else if (eventArg == "postW_dlg_Click")
            {
                JspostSelectList();
                DateTime Vc_date = Dw_date.GetItemDateTime(1, "voucher_date");

                Dw_list.Retrieve(Vc_date, state.SsCoopControl);
            }
            else if (eventArg == "postNewClear")
            {
                JspostNewClear();
            }
            else if (eventArg == "postSearchVoucher")
            {
                JspostSearchVoucher();
            }
            //else if (eventArg == "postPrint")
            //{
            //    PopupReportshr();
            //}
            else if (eventArg == "postAccidDtail")
            {
                JSpostAccidDtail();
            }
            else if (eventArg == "postInsertDetail")
            {
                JspostInsertDetail();
            }
            else if (eventArg == "postDeleteDetail")
            {
                JspostDeleteDetail();
            }
        }

        public void SaveWebSheet()
        {


            try
            {
                for (int i = 1; i <= Dw_detail.RowCount; i++)
                {
                    Dw_detail.SetItemDecimal(i, "order_no", i);
                }

                int row = int.Parse(Hd_row.Value);
                String voucherno = Dw_list.GetItemString(row, "voucher_no");
                String account_id = Dw_list.GetItemString(row, "account_id");
                String acc_side = Dw_date.GetItemString(1, "acc_side");
                string sql = "delete from accledger_detail where voucher_no = '" + voucherno + "' and account_id = '" + account_id + "' and account_nature = '" + acc_side + "'";
                Sdt delete = WebUtil.QuerySdt(sql);
                DwUtil.InsertDataWindow(Dw_detail, pbl, "ACCLEDGER_DETAIL");
                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลเสร็จเรียบร้อยแล้ว");
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.ToString());
            }
        }

        public void WebSheetLoadEnd()
        {
            //ป้องกันการ load Datawindow เบิ้ล
            if (Dw_date.RowCount > 1)
            {
                Dw_date.DeleteRow(Dw_date.RowCount);
            }
            else if (Dw_main.RowCount > 1)
            {
                Dw_main.DeleteRow(Dw_main.RowCount);
            }
            //else if (Dw_find.RowCount > 1)
            //{
            //    Dw_find.DeleteRow(Dw_find.RowCount);
            //}



            Dw_main.SaveDataCache();
            Dw_list.SaveDataCache();
            Dw_detail.SaveDataCache();
            Dw_date.SaveDataCache();
            //Dw_find.SaveDataCache();
            //Dw_footer.SaveDataCache();
        }

        #endregion
    }
}