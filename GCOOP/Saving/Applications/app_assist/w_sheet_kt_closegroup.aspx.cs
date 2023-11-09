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
using CoreSavingLibrary.WcfNDeposit;
using System.Web.Services.Protocols;
using DataLibrary;
using Saving.ConstantConfig;
using Sybase.DataWindow;


namespace Saving.Applications.app_assist
{
    public partial class w_sheet_kt_closegroup : PageWebSheet, WebSheet
    {
        private DwThDate tDwMain;
        protected String newClear;
        private n_depositClient depService;
        private DepositConfig depConfig;
        private String deptAccountNo = null;
        private bool isException = false;
        DataStore dtMain = new DataStore(WebUtil.PhysicalPath + "Saving\\DataWindow\\app_assist\\kt_50bath.pbl", "d_dp_kt_slip_master_usebkbal"); //ใช้เก็บค่าเพื่อนำไปใช้่ปิดบัญชี
        protected Sta ta;
        //JS
        protected String jscloseGroup;
        protected String postDeptWith; //เพิ่มเข้ามา

        private void DefaultSendGov(String cashType)
        {
            Decimal deptSlipNetAmt = dtMain.GetItemDecimal(1, "deptslip_netamt");
            dtMain.SetItemDecimal(1, "send_gov", 0);
            if (cashType == "CSH" && deptSlipNetAmt >= 2000000)
            {
                dtMain.SetItemDecimal(1, "send_gov", 1);
            }
            else
            {
                dtMain.SetItemDecimal(1, "send_gov", 0);
            }
        } //เพิ่มเข้ามา

        private String TryDwMainGetString(String column)
        {
            try
            {
                return dtMain.GetItemString(1, column).Trim();
            }
            catch
            {
                return "";
            }
        } //เพิ่มเข้ามา

        public void InitJsPostBack()
        {
            postDeptWith = WebUtil.JsPostBack(this, "postDeptWith");
            newClear = WebUtil.JsPostBack(this, "newClear");
            jscloseGroup = WebUtil.JsPostBack(this, "jscloseGroup");
        }

        public void WebSheetLoadBegin()
        {
            this.ConnectSQLCA();
            ta = new Sta(sqlca.ConnectionString);
            try
            {
                depService = wcf.NDeposit;
            }
            catch
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ติดต่อ Web Service ไม่ได้");
                return;
            }

            if (IsPostBack)
            {
                this.RestoreContextDw(DwMain);
            }
            else
            {
                DwUtil.RetrieveDataWindow(DwMain, "kt_50bath.pbl", null, null);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "jscloseGroup")
            {
                CloseGroup();
            }
            else if (eventArg == "postDeptWith")
            {
                JsPostDeptWith();
            }
        }

        public void SaveWebSheet()
        {
        }

        public void WebSheetLoadEnd()
        {
            DwMain.SaveDataCache();
            ta.Close();
        }

        private void JsNewClear()
        {
            DwMain.Reset();
            dtMain.Reset();
            DwMain.InsertRow(0);
            DwMain.SaveDataCache();
        } //เพิ่มเข้ามา

        private void CloseGroup()
        {
            int n = DwMain.RowCount;
            string[] arrAccNo = new string[n];
            string[] arrMemberNo = new string[n];
            double tempIntAmt = 0;
            //double tempAccuint = 0;
            double tempPrncbal = 0;

            try
            {
                for (int i = 0; i < n; i++)
                {
                    arrAccNo[i] = DwMain.GetItemString(i + 1, "deptaccount_no"); // เก็บค่า member_no ลงใน array
                    arrMemberNo[i] = DwMain.GetItemString(i + 1, "member_no");
                }
            }
            catch
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบข้อมูลสมาชิก ที้มีสถานะรอปิดกองทุน");
            }

            for (int j = 0; j < n; j++) // Set ค่าใส่ datastore และประมวลผลปิดบัญชี
            {
                try
                {
//                    arrAccNo[j] = depService.BaseFormatAccountNo(state.SsWsPass, arrAccNo[j]);
//                    String ls_xml = depService.InitDepSlip(state.SsWsPass, arrAccNo[j], state.SsCoopId, state.SsUsername, state.SsWorkDate, state.SsClientIp);
//                    DwUtil.ImportData(ls_xml, dtMain, tDwMain, FileSaveAsType.Xml);

//                    dtMain.SetItemString(1, "deptwith_flag", "/");// เซ็ตค่า type ให้เป็นปิดบัญชี
//                    dtMain.SetItemString(1, "recppaytype_code", "CCA");//เซ็ดค่า type เป็นประเภทปิดบัญชีด้วยเงินสด
//                    dtMain.SetItemString(1, "deptitemtype_code", "CCA");//เซ็ดค่า type เป็นประเภทปิดบัญชีด้วยเงินสด
//                    dtMain.SetItemDecimal(1, "deptslip_amt", dtMain.GetItemDecimal(1, "prncbal")); //set ค่าจำนวนเงิน
//                    dtMain.SetItemDateTime(1, "operate_time", DateTime.Now);
//                    string deptslip_date_tdate = state.SsWorkDate.ToString("ddmm") + Convert.ToString(Convert.ToInt32(state.SsWorkDate.ToString("yyyy")) + 543);
//                    dtMain.SetItemString(1, "deptslip_date_tdate", deptslip_date_tdate);

//                    String lsxml = dtMain.Describe("DataWindow.Data.XML");
//                    dtMain.Reset();


//                    String[] resu = depService.InitDeptSlipCalInt(state.SsWsPass, arrAccNo[j], state.SsCoopId, state.SsUsername, state.SsWorkDate, state.SsClientIp, lsxml, "");
//                    DwUtil.ImportData(resu[0], dtMain, tDwMain, FileSaveAsType.Xml);

//                    //tempAccuint = dtMain.GetItemDouble(1, "accuint_amt");
//                    tempIntAmt = dtMain.GetItemDouble(1, "int_amt1");
//                    tempPrncbal = dtMain.GetItemDouble(1,"deptslip_amt");


//                    resu[0] = dtMain.Describe("DataWindow.Data.XML");

//                    string sqlStr = @"UPDATE  asnreqmaster
//                                      SET assistdept_amt = '"+(tempIntAmt + tempPrncbal)+"', pay_status ='0' WHERE deptaccount_no ='"+ arrAccNo[j] +"'";
//                    ta.Exe(sqlStr);


//                    String result = depService.WithdrawClose(state.SsWsPass, resu[0], ""); // เรียกฟังก์ชันปิดบัญชี

//                    string strsql_dp = "UPDATE dpdeptmaster SET accuint_amt = " + tempIntAmt + ", prncbal = " + tempPrncbal + " WHERE deptaccount_no = '" + arrAccNo[j] + "'";
//                    ta.Exe(strsql_dp);

//                    DwMain.Reset();
//                    String endMessage = "บันทึกทำรายการปิดบัญชีเรียบร้อยแล้ว";
//                    LtServerMessage.Text = WebUtil.CompleteMessage(endMessage);
                }
                catch (Exception ex)
                {
                    //LtServerMessage.Text = WebUtil.ErrorMessage("เกิดข้อผิดพลาดขณะทำรายการ");
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                }
            }

        }

        private void JsPostDeptWith()
        {
            String accNo = dtMain.GetItemString(1, "deptaccount_no");
            try
            {
                isException = false;
                try
                {
                    dtMain.SetItemString(1, "deptitemtype_code", dtMain.GetItemString(1, "recppaytype_code"));
                }
                catch { }
                dtMain.SetItemString(1, "tofrom_accid", "");

                String deptWithFlag = TryDwMainGetString("deptwith_flag");
                String recpPayTypeCode = TryDwMainGetString("recppaytype_code");
                String deptTypeCode = TryDwMainGetString("DEPTTYPE_CODE");
                String cashType = depService.of_get_cashtype(state.SsWsPass, recpPayTypeCode);// TryDwMainGetString("cash_type");
                Decimal adc_intsum=0;
                if (deptWithFlag == "/")
                {
                    dtMain.SetItemDecimal(1, "deptslip_amt", dtMain.GetItemDecimal(1, "prncbal"));
                }

                if (deptWithFlag == "/")
                {
                    String ls_xml = dtMain.Describe("DataWindow.Data.XML");
                    String ls_xml_det = "";
                    //if (DwItem.RowCount > 0)
                    //{
                    //    ls_xml_det = DwItem.Describe("DataWindow.Data.XML");
                    //}
                    //String[] result = depService.InitDeptSlipCalInt(state.SsWsPass, accNo, state.SsCoopId, state.SsUsername, state.SsWorkDate, state.SsClientIp, ls_xml, ls_xml_det, adc_intsum);
                    //DwUtil.ImportData(result[0], dtMain, tDwMain, FileSaveAsType.Xml);
                    //DwItem.Reset();
                    //if (WebUtil.IsXML(result[1]))
                    //{
                    //    DwUtil.ImportData(result[1], DwItem, tDwItem, FileSaveAsType.Xml);
                    //}
                }
                //else if (deptWithFlag == "+")
                //{
                //    decimal balance = DwMain.GetItemDecimal(1, "prncbal");
                //    Decimal isEqualDept = depService.IsEqualDept(state.SsWsPass, accNo, state.SsCoopId, balance, deptTypeCode, recpPayTypeCode);
                //    if (isEqualDept > 0)
                //    {
                //        DwMain.SetItemDecimal(1, "deptslip_amt", isEqualDept);
                //        DwMain.SetItemDecimal(1, "deptslip_netamt", isEqualDept);
                //        JsPostTotalWidthFixed();
                //        DwMain.Modify("deptslip_amt.Protect=1");
                //    }
                //}
                DefaultSendGov(cashType);
            }
            catch (Exception ex)
            {
                isException = true;
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
            finally
            {
                if (isException)
                {
                    accNo = null;
                    JsNewClear();
                }
            }
            this.deptAccountNo = accNo;
        } //Function ใช้ในการเลือกประเภทการทำรายการ

        //private void CloseAccount()
        //{
        //    try
        //    {
        //        String ls_xml = DwMain.Describe("DataWindow.Data.XML");

        //        String result = depService.WithdrawClose(state.SsWsPass, ls_xml, "");
        //        String endMessage = "บันทึกทำรายการปิดบัญชี " + depConfig.CnvDeptAccountFormat(deptAccountNo) + " เรียบร้อยแล้ว";
        //        JsNewClear();
        //        LtServerMessage.Text = WebUtil.CompleteMessage(endMessage);
        //        //CallPrintBook(result, endMessage);
        //    }
        //    catch (Exception ex)
        //    {
        //        LtServerMessage.Text = WebUtil.ErrorMessage(ex);
        //    }
        //}   
    }
}