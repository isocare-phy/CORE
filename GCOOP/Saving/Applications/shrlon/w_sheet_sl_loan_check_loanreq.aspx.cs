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
using CoreSavingLibrary.WcfNShrlon;
using CoreSavingLibrary.WcfNCommon;
using CoreSavingLibrary.WcfNBusscom;
using Sybase.DataWindow;
using DataLibrary;

namespace Saving.Applications.shrlon
{
    public partial class w_sheet_sl_loan_check_loanreq : PageWebSheet, WebSheet
    {
        public String lc_loangroup = "02";
        public String lc_loantype = null;
        private DwThDate tDwMain;
        //*******ประกาศตัวเกี่ยวกับ  Javascript********//
        protected String jsExpenseBank;
        protected String jsExpenseCode;
        protected String jsGetMemberInfo;
        protected String jsGetMemberCollno;
        protected String jsReNewPage;
        protected String jsOpenOldDocNo;
        protected String jsPostSetZero;
        protected String jsSetpriod;
        protected String jsCancelRequest;
        protected String jsRefresh;
        protected String jsCollInitP;
        protected String jsCollCondition;
        protected String jsSetDataList;
        protected String jsPostColl;
        protected String jsSumOthClr;
        protected String jsContPeriod;
        protected String jsChangeStartkeep;
        protected String jsResumLoanClear;
        protected String jsPermissSalary;
        protected String jsRevert;
        protected String jsPaycoopid;
        protected String jsObjective;
        protected String jsSetloantype;
        protected String jsPostreturn;
        protected String jsmaxcreditperiod;
        protected String jsLoanpaymenttype;
        protected string jssetcollrefcontno;
        protected string jsSetloantypechg;
        protected string JsReOtherclr;
        protected string jsSetFixdate;
        protected string jsExpensebankbrRetrieve;
        //*******ประกาศตัวเกี่ยวกับ  Reprot********//
        Sta ta;
        string reqdoc_no = "";
        String member_no = "";
        string fromset = "";
        int x = 1;
        protected String app;
        protected String gid;
        protected String rid;
        protected String pdf;
        protected String runProcess;
        protected String popupReport;

        protected string jsrecalloanpermiss;
        protected string jsCalpemisssalarybal;
        protected string jsGetexpensememno;
        protected string jsGetitemdescetc;
        protected string jsSsetAcci_dept;
        protected string jsReGenClear;
        //*******end Reprot********//

        private n_shrlonClient shrlonService; 
        private n_commonClient commonService;
        private n_busscomClient busscomService;
        String ls_membtype = "";
        String loantype = "";
        int li_cramationstatus = 0;//สถานะฌาปนกิจ
        String pbl = "sl_loan_requestment_cen.pbl";

        public void InitJsPostBack()
        {
            jsExpenseCode = WebUtil.JsPostBack(this, "jsExpenseCode");
            jsExpenseBank = WebUtil.JsPostBack(this, "jsExpenseBank");
            jsGetMemberInfo = WebUtil.JsPostBack(this, "jsGetMemberInfo");
            jsGetMemberCollno = WebUtil.JsPostBack(this, "jsGetMemberCollno");
            jsReNewPage = WebUtil.JsPostBack(this, "jsReNewPage");
            jsOpenOldDocNo = WebUtil.JsPostBack(this, "jsOpenOldDocNo");
            jsPostSetZero = WebUtil.JsPostBack(this, "jsPostSetZero");
            jsSetpriod = WebUtil.JsPostBack(this, "jsSetpriod");
            jsCancelRequest = WebUtil.JsPostBack(this, "jsCancelRequest");
            jsRefresh = WebUtil.JsPostBack(this, "jsRefresh");
            jsCollInitP = WebUtil.JsPostBack(this, "jsCollInitP");
            jsCollCondition = WebUtil.JsPostBack(this, "jsCollCondition");
            jsSetDataList = WebUtil.JsPostBack(this, "jsSetDataList");
            jsPostColl = WebUtil.JsPostBack(this, "jsPostColl");
            jsSumOthClr = WebUtil.JsPostBack(this, "jsSumOthClr");
            jsChangeStartkeep = WebUtil.JsPostBack(this, "jsChangeStartkeep");
            jsContPeriod = WebUtil.JsPostBack(this, "jsContPeriod");
            jsResumLoanClear = WebUtil.JsPostBack(this, "jsResumLoanClear");
            jsPermissSalary = WebUtil.JsPostBack(this, "jsPermissSalary");
            jsRevert = WebUtil.JsPostBack(this, "jsRevert");
            jsPaycoopid = WebUtil.JsPostBack(this, "jsPaycoopid");
            jsObjective = WebUtil.JsPostBack(this, "jsObjective");
            jsSetloantype = WebUtil.JsPostBack(this, "jsSetloantype");
            jsPostreturn = WebUtil.JsPostBack(this, "jsPostreturn");
            jsmaxcreditperiod = WebUtil.JsPostBack(this, "jsmaxcreditperiod");
            jsmaxcreditperiod = WebUtil.JsPostBack(this, "jsSetFixdate");

            jsrecalloanpermiss = WebUtil.JsPostBack(this, "jsrecalloanpermiss");
            JsReOtherclr = WebUtil.JsPostBack(this, "JsReOtherclr");
            runProcess = WebUtil.JsPostBack(this, "runProcess");
            jssetcollrefcontno = WebUtil.JsPostBack(this, "jssetcollrefcontno");
            jsSetloantypechg = WebUtil.JsPostBack(this, "jsSetloantypechg");
            jsExpensebankbrRetrieve = WebUtil.JsPostBack(this, "jsExpensebankbrRetrieve");
            jsCalpemisssalarybal = WebUtil.JsPostBack(this, "jsCalpemisssalarybal");
            jsGetexpensememno = WebUtil.JsPostBack(this, "jsGetexpensememno");
            jsGetitemdescetc = WebUtil.JsPostBack(this, "jsGetitemdescetc");
            jsSsetAcci_dept = WebUtil.JsPostBack(this, "jsSsetAcci_dept");
            jsReGenClear = WebUtil.JsPostBack(this, "jsReGenClear");

            tDwMain = new DwThDate(dw_main, this);
            tDwMain.Add("loanrequest_date", "loanrequest_tdate");
            tDwMain.Add("loanrcvfix_date", "loanrcvfix_tdate");
            tDwMain.Add("startkeep_date", "startkeep_tdate");

        }

        public void WebSheetLoadBegin()
        {

            try
            {
                shrlonService = wcf.NShrlon;
                commonService = wcf.NCommon; 

            }
            catch
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ติดต่อ Web Service ไม่ได้");
                return;
            }

            if (IsPostBack)
            {

                this.RestoreContextDw(dw_main);
                this.RestoreContextDw(dw_coll);
                this.RestoreContextDw(dw_clear);
                this.RestoreContextDw(dw_otherclr);
                String setting = dw_coll.Describe("coop_id.Protect");
                bool isChecked = CbCheckcoop.Checked;
                if (isChecked)
                {
                    dw_coll.Modify("coop_id.Protect=0");
                    RetreiveDDDW();
                    JsExpenseCode();
                }
                else
                {

                    dw_coll.SetItemString(1, "coop_id", state.SsCoopControl);
                    dw_coll.Modify("coop_id.Protect=1");
                }
            }
            else
            {
                //HdShowRemark.Value = "false";
            }
            if (dw_main.RowCount < 1)
            {
                dw_main.DisplayOnly = false;
                dw_clear.DisplayOnly = false;
                dw_coll.DisplayOnly = false;
                dw_otherclr.DisplayOnly = false;
                Session["strItemchange"] = "";
                dw_main.InsertRow(0);
                dw_main.SetItemString(1, "memcoop_id", state.SsCoopControl);
                RetreiveDDDW();
                JsChangeStartkeep();
                tDwMain.Eng2ThaiAllRow();

                CbCheckcoop.Checked = false;
                HdIsPostBack.Value = "false";
                HdCheckRemark.Value = "false";
            }
            if (dw_main.RowCount > 1)
            {
                dw_main.DeleteRow(dw_main.RowCount);
            }
            //Session["strItemchange"] = "";
            dw_message.Reset();
            dw_message.InsertRow(0);
            dw_message.DisplayOnly = false;
            dw_message.Visible = false;
            //HdCheckRemark.Value = "false";
        }

        /// <summary>
        /// Check  PostBack Javascript
        /// </summary>
        /// <param name="eventArg"></param>
        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "jsGetMemberInfo")
            {
                JsGetMemberInfo();
            }
            else if (eventArg == "jsExpenseBank")
            {
                JsExpenseBank();
            }
            else if (eventArg == "jsExpenseCode")
            {
                JsExpenseCode();
            }
            else if (eventArg == "jsGetMemberCollno")
            {
                JsGetMemberCollno();
            }
            else if (eventArg == "jsReNewPage")
            {
                JsReNewPage();
            }
            else if (eventArg == "jsOpenOldDocNo")
            {
                JsOpenOldDocNo();
            }
            else if (eventArg == "jsPostSetZero")
            {
                //  LtServerMessage.Text = "ยอดเงินขอกู้มากกว่ายอดเงินสิทธิ์ให้กู้ ";
                //hardcode 
                //dw_main.SetItemDecimal(1, "loanrequest_amt", 0);
            }
            else if (eventArg == "jsSetpriod")
            {
                JsSetpriod();

            }
            else if (eventArg == "jsCancelRequest")
            {

                JsCancelRequest();
            }
            else if (eventArg == "jsRefresh")
            {

            }
            else if (eventArg == "jsCollInitP")
            {
                JsCollInitP();
            }
            else if (eventArg == "jsCollCondition")
            {
                JsCollCondition();
            }
            else if (eventArg == "jsSetDataList")
            {
                JsSetDataList();
            }
            else if (eventArg == "jsPostColl")
            {
                JsPostColl();
            }
            else if (eventArg == "jsChangeStartkeep")
            {
                JsChangeStartkeep();
            }
            else if (eventArg == "jsSumOthClr")
            {
                JsSumOthClr();
            }
            else if (eventArg == "jsContPeriod")
            {
                JsContPeriod();

            }
            else if (eventArg == "jsResumLoanClear")
            {

               // JsResumLoanClear();

            }
            else if (eventArg == "jsPermissSalary")
            {
                JsPermissSalary();
            }
            else if (eventArg == "jsRevert")
            {
                JsRevert();
            }
            else if (eventArg == "jsObjective")
            {
                JsObjective();
            }
            else if (eventArg == "jsPaycoopid")
            {
                JsPaycoopid();
            }
            else if (eventArg == "jsExpensebankbrRetrieve")
            {
                JsExpensebankbrRetrieve();
            }

                 
            else if (eventArg == "jsSetloantype")
            {
                JsSetloantype();
            }
            else if (eventArg == "jsmaxcreditperiod")
            {
                Jsmaxcreditperiod();
            }
            else if (eventArg == "jssetcollrefcontno")
            {
                JsSetCollContno();
            }
            else if (eventArg == "jsSetloantypechg")
            {
                JsSetloantypechg();

            }
            else if (eventArg == "jsLoanpaymenttype") { JsLoanpaymenttype(); }

            else if (eventArg == "JsReOtherclr")
            {
                ReOtherMain();
            }
            else if (eventArg == "jsSetFixdate")
            {
                JsSetFixdate();
            }
            else if (eventArg == "jsCalpemisssalarybal")
            {
                JsCalpemisssalarybal();
            }
            else if (eventArg == "jsGetexpensememno") {
                JsGetexpensememno();
            }
            else if (eventArg == "jsGetitemdescetc")
            {
                JsGetitemdescetc();
            }
            else if (eventArg == "jsSsetAcci_dept") {
                SetAcci_dept();
            }
            else if (eventArg == "jsReGenClear") {
                JsRegenClear();
            }


        }

         
       


        private string of_getrerydate(DateTime birthDate)
        {
            string retry_date = "";
            string coop_id = state.SsCoopControl;
            try
            {
                Sdt dt = WebUtil.QuerySdt("select	retry_age,retry_month,retry_day from	cmcoopmaster where coop_id ='" + coop_id + "' ");
                // dt.next คือเลื่อนเคอร์เซอร์เพื่อไปหาค่าแถวถัดไป
                if (dt.Next())
                {   //เอาค่า +ปีที่เกษียณ  + วันเกิด
                    int retry_age = dt.GetInt32("retry_age");
                    int retry_month = dt.GetInt32("retry_month");
                    int retry_day = dt.GetInt32("retry_day");
                    //int retry_age = Convert.ToInt16(dt.Rows[0]["retry_age"]);
                    //int retry_month = Convert.ToInt16(dt.Rows[0]["retry_month"]);
                    //int retry_day = Convert.ToInt16(dt.Rows[0]["retry_day"]);
                    int year = birthDate.Year + retry_age;
                    int month = birthDate.Month;
                    int day = birthDate.Day;
                    int loop_day = 0;
                    //ตั้งค่าวันที่สิ้นสุดของแต่ล่ะเดือน

                    if (retry_day == 0)
                    {
                        int[] daysinmonth = new int[12];
                        for (int i = 0; i < 12; i++)
                        {
                            if (i == 1)
                            {
                                daysinmonth[i] = 28;
                            }
                            else
                            {

                                if (i == 0 || i == 2 || i == 4 || i == 6 || i == 7 || i == 9 || i == 11)
                                {
                                    daysinmonth[i] = 31;
                                }
                                else
                                { daysinmonth[i] = 30; }
                            }

                        }
                        for (int i = 0; i < 12; i++)
                        {
                            if (day > daysinmonth[i])
                            {   //เช็ควันที่สิ้นสุดของเดือน กุมภาพันธ์
                                if (i == 1)
                                {

                                    if ((year % 4 == 0 && year % 100 != 0) || year % 400 == 0)
                                    {
                                        day = 29;
                                    }
                                }
                            }
                            else
                            {
                                if (month == i + 1)
                                {
                                    day = daysinmonth[i];
                                }
                            }

                        }
                        if (retry_month != 0)
                        {
                            loop_day = daysinmonth[retry_month - 1];
                            day = loop_day;
                        }
                    }
                    else
                    {
                        day = retry_day;
                    }

                    if (retry_month != 0)
                    {
                        //เช็คเกษียณครบรอบ
                        if (month > retry_month)
                        {
                            year = year + 1;

                        }

                        month = retry_month;
                    }

                    retry_date = day.ToString("00") + '-' + month.ToString("00") + '-' + year.ToString("0000");
                    return retry_date;
                }
                else
                {
                    return retry_date;
                }
            }
            catch
            {
                return retry_date;
            }
        }

       
        private void JsCalpemisssalarybal()
        {
            try
            {
                
                JsCalMaxLoanpermiss();
                JsContPeriod();
               // JsLoanpaymenttype();
                JsSumOthClr();
                
                //    }
                //}
            }
            catch (Exception ex) { ex.ToString(); }


        }

        /// <summary>
        ///บันทึกใบคำขอ
        /// </summary>
        public void SaveWebSheet()
        {

           

        }

        private void Jscheckloancontarrer()
        {

            string membno = dw_main.GetItemString(1, "member_no");

            string ls_sql = "select sum( principal_arrear) as prn_arrear, sum(interest_arrear) as int_arrear from lncontmaster where member_no = '" + membno + "'";
            Sdt dtcont = WebUtil.QuerySdt(ls_sql);
            if (dtcont.Next())
            {
                decimal prnc_arrear = dtcont.GetDecimal("prn_arrear");
                decimal int_arrear = dtcont.GetDecimal("int_arrear");
                if (prnc_arrear > 0 || int_arrear > 0)
                {
                    MessageBox("ใบคำขอกู้"," สมาชิกท่านนี้มียอดค้างชำระกับสหกรณ์ เป็นต้น " + prnc_arrear.ToString() + " และมียอดค้างชำระ ดบ. " + int_arrear.ToString());
                    LtServerMessage.Text = WebUtil.ErrorMessage(" สมาชิกท่านนี้มียอดค้างชำระกับสหกรณ์ เป็นต้น " + prnc_arrear.ToString() + " และมียอดค้างชำระ ดบ. " + int_arrear.ToString());

                }
            }


        }
        public void WebSheetLoadEnd()
        {
           // JsExpenseCode();
            try
            {
                //DataWindowChild dwloantype_code = dw_main.GetChild("loantype_code_1");
                //dwloantype_code.SetFilter("LNLOANTYPE.LOANGROUP_CODE   ='" + lc_loangroup + "'");
                //dwloantype_code.Filter();

                JsSumOthClr();
                //DateTime ReqDate = dw_main.GetItemDateTime(1, "loanrequest_date");
                //dw_main.SetItemTime(1, "loanrcvfix_date", ReqDate);
                //tDwMain.Eng2ThaiAllRow();
                //string ReqDate = dw_main.GetItemString(1, "loanrequest_date");
                //dw_main.SetItemString(1, "loanrcvfix_tdate", ReqDate);
                str_itemchange strList = new str_itemchange();
                strList.xml_main = dw_main.Describe("DataWindow.Data.XML");

                if (dw_clear.RowCount == 0)
                { strList.xml_clear = null; }
                else { strList.xml_clear = dw_clear.Describe("DataWindow.Data.XML"); }
                if (dw_coll.RowCount == 0) { strList.xml_guarantee = null; }
                else { strList.xml_guarantee = dw_coll.Describe("DataWindow.Data.XML"); }
                Session["strItemchange"] = strList;

                //หาจำนวนงวดที่ชำระ
                string member_no = dw_main.GetItemString(1, "member_no");
                if (member_no != null || member_no != "")
                {
                    DateTime startkeep_date = dw_main.GetItemDate(1, "startkeep_date");
                    DateTime retry_date = dw_main.GetItemDateTime(1, "retry_date");
                    Int32 month_ = 12 - (startkeep_date.Month - 1);
                    Int32 retryage = (retry_date.Year - startkeep_date.Year - 1) * 12 + 9 + month_;
                    dw_main.SetItemDecimal(1, "retry_age", retryage);
                }
            }
            catch { }

            DwUtil.RetrieveDDDW(dw_coll, "loancolltype_code", pbl, null);
            dw_main.SaveDataCache();//main
            dw_coll.SaveDataCache();//หลักประกัน
            dw_clear.SaveDataCache();//หักกลบ     
            dw_otherclr.SaveDataCache();//หักอื่น
        }

        /// <summary>
        ///  reset หน้าใหม่
        /// </summary>
        private void JsReNewPage()
        {
            try
            {
                string loantype_code = dw_main.GetItemString(1, "loantype_code");
                dw_main.Reset();
                dw_main.InsertRow(0);
                dw_coll.Reset();
                dw_clear.Reset();
                dw_otherclr.Reset();
                dw_main.SetItemString(1, "memcoop_id", state.SsCoopControl);
                dw_main.SetItemString(1, "coop_id", state.SsCoopId);
                dw_main.SetItemString(1, "loantype_code", loantype_code);
                dw_main.SetItemString(1, "loantype_code_1", loantype_code);
                Session["loantypeCode"] = loantype_code;
                RetreiveDDDW();
                //JsChangeStartkeep();
                dw_main.SetItemDateTime(1, "loanrequest_date", state.SsWorkDate);
                dw_main.SetItemDate(1, "loanrcvfix_date", state.SsWorkDate); // edit by bank
                tDwMain.Eng2ThaiAllRow();

                JsSetloantype();
                HdIsPostBack.Value = "false";
                HdCheckRemark.Value = "false";
                HdShowRemark.Value = "false";
                Hdcollconfirm.Value = "0";
                //HdShowRemark.Value = "false";
                Ltjspopup.Text = " ";
                Ltjspopupclr.Text = "";
                //DataWindowChild dwloantype_code = dw_main.GetChild("loantype_code_1");
                //dwloantype_code.SetFilter("LNLOANTYPE.LOANTYPE_CODE in ('20','21','22') ");
                //dwloantype_code.Filter();
            }
            catch { 
            
            }
        }

        /// <summary>
        ///  retreive datawindows dropdown
        /// </summary>
        public void RetreiveDDDW()
        {

            try
            {

                loantype = Session["loantypeCode"].ToString();

                Session.Remove("loantype");

            }
            catch
            {
                String sql = "";
                //if (state.SsCoopControl == "001001")
                //{
                //    sql = "select min(loantype_code) from lnloantype where loangroup_code='" + lc_loangroup + "' and coop_id = '" + state.SsCoopControl + "'";
                //}
                //else 
                //{

                sql = "select min(loantype_code) from lnloantype where  coop_id = '" + state.SsCoopControl + "'";
                //}
                DataTable dt = WebUtil.Query(sql);
                if (dt.Rows.Count > 0)
                {
                    lc_loantype = dt.Rows[0][0].ToString().Trim();
                    loantype = lc_loantype;
                    Session.Remove("loantype");
                }
                else
                {
                    throw new Exception("ไม่พบประเภทเงินกู้");
                }
            }
            try
            {
                DwUtil.RetrieveDDDW(dw_main, "loantype_code_1", pbl, null);
                if (loantype == "")
                {
                    ///<กำหนดค่าเริ่มต้น เป็นสามัญ>

                    dw_main.SetItemString(1, "loantype_code", lc_loantype);

                }
                else
                {
                    //mai แก้ไข retrieve Dropdown
                    DataWindowChild dwloantype_code = dw_main.GetChild("loantype_code_1");
                    //dwloantype_code.SetFilter("LNLOANTYPE.LOANGROUP_CODE   ='" + lc_loangroup + "'");
                    //dwloantype_code.Filter();
                    dw_main.SetItemString(1, "loantype_code", loantype);
                }
                DwUtil.RetrieveDDDW(dw_main, "expense_code", pbl, null);
                DwUtil.RetrieveDDDW(dw_main, "loanobjective_code_1", pbl, loantype);
                DwUtil.RetrieveDDDW(dw_main, "membtype_code", pbl, null);
                //DwUtil.RetrieveDDDW(dw_main, "coop_id_1", pbl, null);
                //DwUtil.RetrieveDDDW(dw_main, "memcoop_id", pbl, null);
                //DwUtil.RetrieveDDDW(dw_main, "paytoorder_desc_1", pbl, null);
                //wa DwUtil.RetrieveDDDW(dw_main, "expense_branch_1", pbl, null);
                //mai แก้ไขเพิ่ม coop_id
                //DataWindowChild paytoorder = dw_main.GetChild("paytoorder_desc_1");
                //if (state.SsUsername == "Loan01")
                //{

                //    dw_main.SetItemString(1, "paytoorder_desc", "001001");
                //}
                //else if (state.SsUsername == "Loan02")
                //{

                //    dw_main.SetItemString(1, "paytoorder_desc", "001002");
                //}
                //else if (state.SsUsername == "Loan03")
                //{

                //    dw_main.SetItemString(1, "paytoorder_desc", "001003");
                //}
                //else if (state.SsUsername == "Loan04")
                //{

                //    dw_main.SetItemString(1, "paytoorder_desc", "001004");
                //}

                // else {
                dw_main.SetItemString(1, "paytoorder_desc", state.SsCoopId);
                //}
                //mai เพิ่ม coop_id
                dw_main.SetItemString(1, "coop_id", state.SsCoopId);

                DwUtil.RetrieveDDDW(dw_coll, "loancolltype_code", pbl, null);
                DwUtil.RetrieveDDDW(dw_coll, "coop_id", pbl, null);
                DwUtil.RetrieveDDDW(dw_otherclr, "clrothertype_code", pbl, null);


            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
        }

        /// <summary>
        /// กำหนดวันที่จ่ายเงินกู้และวันที่เรียกเก็บ
        /// </summary>
        private void JsChangeStartkeep()
        {
            try
            {
                JsSetFixdate();
                DateTime postingdate = new DateTime();
                DateTime processdate = new DateTime();
                DateTime ldtm_loanreceive = new DateTime();
                //dw_main.SetItemDateTime(1, "loanrequest_date", state.SsWorkDate);
                DateTime ldtm_loanrequest = dw_main.GetItemDateTime(1, "loanrequest_date");// state.SsWorkDate;
                // DateTime ldtm_loanrequest = dw_main.GetItemDateTime(1, "loanrequest_date");
                String loantype = dw_main.GetItemString(1, "loantype_code");

                ldtm_loanreceive = ldtm_loanrequest;

                int year = Convert.ToInt16(ldtm_loanreceive.Year + 543);
                short month = Convert.ToInt16(ldtm_loanreceive.Month);
                //a hardcode

                string ls_memno = "0000000";
                try
                {

                    ls_memno = dw_main.GetItemString(1, "member_no");
                }
                catch { ls_memno = "0000000"; }
                String sqlpro = " SELECT MAX(receipt_date)as LASTPROCESS_DATE FROM  kptempreceive where member_no = '" + ls_memno + "'";
                Sdt dtpro = WebUtil.QuerySdt(sqlpro);
                if (dtpro.Next())
                {
                    try
                    {
                        processdate = dtpro.GetDate("LASTPROCESS_DATE");
                        // if (processdate < ldtm_loanrequest) { processdate = ldtm_loanrequest; }
                    }
                    catch { processdate = ldtm_loanrequest; }

                    //กรณี วันที่จ่ายเงิน=วันที่ขอกู้

                }
                else { processdate = ldtm_loanrequest; }// wcf.NBusscom.of_getpostingdate(state.SsWsPass, ldtm_loanreceive); 

                if (dtpro.GetRowCount() <= 0)
                {
                    decimal day = ldtm_loanrequest.Day;
                    if (day >= 15)
                    {
                        month = Convert.ToInt16(ldtm_loanreceive.Month + 1);
                        if (month > 12)
                        {
                            month = 1;
                            year = year + 1;

                        }

                    }

                }
                //จ่ายเงินกู้หลังเรียกเก็บหรือไม่
                if (ldtm_loanreceive < processdate)
                {
                    month = Convert.ToInt16(ldtm_loanreceive.Month + 1);
                    if (month > 12)
                    {
                        month = 1;
                        year = year + 1;
                        postingdate = JsGetPostingdate(year, month, ldtm_loanreceive);
                    }
                    short month_old = Convert.ToInt16(ldtm_loanreceive.Month);
                    DateTime postingdate_old = JsGetPostingdate(year, month, ldtm_loanreceive);// wcf.NBusscom.of_getpostingdate2(state.SsWsPass, Convert.ToInt16(year), month_old);

                    int day = ldtm_loanreceive.DayOfYear - postingdate_old.DayOfYear;
                    if (day == 1)
                    {
                        month = Convert.ToInt16(ldtm_loanreceive.Month + 1);
                        postingdate = JsGetPostingdate(year, month, postingdate_old); //wcf.NBusscom.of_getpostingdate2(state.SsWsPass, Convert.ToInt16(year), month);
                    }
                    else
                    {
                        month = Convert.ToInt16(processdate.Month + 1);
                        postingdate = JsGetPostingdate(year, month, postingdate_old); // wcf.NBusscom.of_getpostingdate2(state.SsWsPass, Convert.ToInt16(year), month);
                    }


                }
                else if (ldtm_loanreceive > processdate)
                {

                    // ldtm_loanreceive = ldtm_loanrequest;// wcf.NBusscom.of_relativeworkdate(state.SsWsPass, ldtm_loanreceive, ai_increase);
                    month = Convert.ToInt16(ldtm_loanreceive.Month);
                    postingdate = JsGetPostingdate(year, month, ldtm_loanrequest); //wcf.NBusscom.of_getpostingdate2(state.SsWsPass, Convert.ToInt16(year), month);

                }
                else
                {
                    month = Convert.ToInt16(ldtm_loanreceive.Month + 1);
                    postingdate = JsGetPostingdate(year, month, ldtm_loanrequest);  //wcf.NBusscom.of_getpostingdate2(state.SsWsPass, Convert.ToInt16(year), month);
                }

                //dw_main.SetItemDate(1, "loanrcvfix_date", ldtm_loanrequest);
                //postingdate = wcf.InterPreter.GetStartkeep(state.SsConnectionIndex, state.SsCoopControl, ldtm_loanrequest);
                dw_main.SetItemDate(1, "startkeep_date", postingdate);
                tDwMain.Eng2ThaiAllRow();

            }
            catch (Exception ex)
            {
                //LtServerMessage.Text = WebUtil.ErrorMessage("JsChangeStartkeep===>" + ex);
            }
        }
        private DateTime JsGetPostingdate(int year, int month, DateTime request_date)
        {
            string ls_sql = "select postingdate from amworkcalendar where  year = " + year.ToString() + "  and month  = " + month.ToString();
            DateTime postingdate = request_date;
            Sdt dtpro = WebUtil.QuerySdt(ls_sql);
            if (dtpro.Next())
            {
                try
                {
                    int daypost = dtpro.GetInt32("postingdate");
                    year = year - 543;
                    string postdate = daypost.ToString() + "/" + month.ToString() + "/" + year.ToString();
                    postingdate = Convert.ToDateTime(postdate);
                    // if (postingdate < request_date) { postingdate = request_date; }
                }
                catch
                { postingdate = request_date; }
                //กรณี วันที่จ่ายเงิน=วันที่ขอกู้

            }
            else
            {
                postingdate = request_date;
            }
            return postingdate;
        }
        private void JsSetCollContno()
        {
            try
            {
                //กรณีเลือกกู้แบบกู้เพิ่ม
                string ls_memno = dw_main.GetItemString(1, "member_no");
                string ls_loantype = dw_main.GetItemString(1, "loantype_code");
                string contno_pri = "";
                string ls_sqlcontnopri = @"select max( loancontract_no ) as cont_no, loancredit_amt from lncontmaster where principal_balance > 0 and contract_status <> -9 and member_no = '" + ls_memno + "' and loantype_code = '" + ls_loantype + "' group by loancredit_amt ";

                Sdt dt9 = WebUtil.QuerySdt(ls_sqlcontnopri);
                if (dt9.Next())
                {
                    contno_pri = dt9.GetString("cont_no");
                }
                if (dt9.GetRowCount() < 1)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(" ไม่พบเลขสัญญาเดิมของสมาชิกท่านนี้ ไม่สามารถกู้เพิ่มได้");

                }

                dw_main.SetItemDecimal(1, "loanrequest_status", 1);
                dw_main.SetItemString(1, "loancontract_no", contno_pri);

                string sql_memcoll = @" SELECT LNCONTMASTER.MEMBER_NO as member_no,   
             LNCONTMASTER.LOANCONTRACT_NO as loancontract_no,   
             LNCONTCOLL.REF_COLLNO as  ref_collno,   
             LNCONTCOLL.DESCRIPTION as DESCRIPTION ,   
             LNCONTCOLL.COLL_AMT as COLL_AMT ,   
             LNCONTCOLL.COLL_PERCENT as COLL_PERCENT ,   
             LNCONTCOLL.BASE_PERCENT as BASE_PERCENT ,'Contno',
             LNCONTMASTER.COOP_ID as coop_id,
             LNCONTMASTER.loancredit_amt  ,
             LNCONTMASTER.last_periodrcv             
            FROM LNCONTCOLL,   
                 LNCONTMASTER  
           WHERE ( LNCONTCOLL.COOP_ID = LNCONTMASTER.COOP_ID ) and  
                 ( LNCONTCOLL.LOANCONTRACT_NO = LNCONTMASTER.LOANCONTRACT_NO ) and  
                 ( ( LNCONTCOLL.LOANCOLLTYPE_CODE in ('01') ) AND  
                 ( LNCONTMASTER.PRINCIPAL_BALANCE > 0 ) AND LNCONTMASTER.LOANCONTRACT_NO = '" + contno_pri + "')";

                Sdt dtcoll = WebUtil.QuerySdt(sql_memcoll);
                string collref_no = "", colldesc = "", coop_id = "";
                int coll_row = 0;
                decimal loancredit_amt = 0;
                decimal last_periodrcv = 0;
                dw_coll.Reset();
                if (dtcoll.GetRowCount() < 1)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบรายการสมาชิกค้ำประกันสัญญา " + contno_pri);
                }
                while (dtcoll.Next())
                {
                    collref_no = dtcoll.GetString("ref_collno");
                    colldesc = dtcoll.GetString("DESCRIPTION");
                    coop_id = dtcoll.GetString("coop_id");


                    loancredit_amt = dtcoll.GetDecimal("loanapprove_amt");
                    last_periodrcv = dtcoll.GetDecimal("last_periodrcv");

                    coll_row = dw_coll.InsertRow(0);

                    dw_coll.SetItemString(coll_row, "coop_id", coop_id);
                    dw_coll.SetItemString(coll_row, "ref_collno", collref_no);
                    dw_coll.SetItemString(coll_row, "DESCRIPTION", colldesc);
                    dw_coll.SetItemDecimal(coll_row, "coll_balance", 0);
                    dw_coll.SetItemDecimal(coll_row, "base_percent", 1);


                }

                if (loancredit_amt > 0)
                {
                    dw_main.SetItemDecimal(1, "loancredit_amt", loancredit_amt);
                }
                if (last_periodrcv > 1)
                {
                   
                        dw_main.SetItemDecimal(1, "onlinefee_amt", last_periodrcv);
                    
                }
            }
            catch { 
            
            }
        }

        private void JsGenrowColllnreq(decimal loancredit_amt)
        {
            //ตรวจค้ำประกัน

            int coll_num = 0;
            string loantype_code = dw_main.GetItemString(1, "loantype_code");
            decimal loanrequest_amt = dw_main.GetItemDecimal(1, "loanrequest_amt");
            int collrow = dw_coll.RowCount;

            String sqlpro = @" select useman_amt, useshare_flag from lnloantypereqgrt  
                            where loantype_code = '" + loantype_code + "' and money_from <=  " + loancredit_amt.ToString() + @" 
                                    and  money_to >= " + loancredit_amt.ToString();
            Sdt dtgrt = WebUtil.QuerySdt(sqlpro);
            if (dtgrt.Next())
            {
                coll_num = Convert.ToInt32(dtgrt.GetDecimal("useman_amt"));
                int coll_share = Convert.ToInt32(dtgrt.GetDecimal("useshare_flag"));

            }
            else { coll_num = 0; }

            for (int i = 1; i <= coll_num; i++)
            {

                dw_coll.InsertRow(0);
                dw_coll.SetItemString(i + 1, "loancolltype_code", "01");
            }
        }
        private void JsSetloantypechg()
        {
            string membno = " ";
            try
            {
                membno = dw_main.GetItemString(1, "member_no");
            }
            catch
            {
                membno = " ";
            }
            dw_main.SetItemDecimal(1, "otherclr_flag", 0);
            dw_main.SetItemDecimal(1, "otherclr_amt", 0);
            dw_coll.Reset();
            dw_clear.Reset();
            dw_otherclr.Reset();
            JsSetloantype();
            string ls_messagewarning = "";
            if (JsCheckLoanrequestwait(ref ls_messagewarning) == 1)
            {
                if (membno.Length > 5)
                {
                    Jsmaxcreditperiod();
                }
            }
            //JsCalMaxLoanpermiss();
            string ls_loantype = dw_main.GetItemString(1, "loantype_code");
            Session["loantypeCode"] = ls_loantype;
        }
        private void JsSetloantype()
        {

            string membtypecode = Hdmembtype_code.Value;

            try
            {
                loantype = dw_main.GetItemString(1, "loantype_code");

            }
            catch
            {
                dw_main.SetItemString(1, "loantype_code", "");
                DwUtil.RetrieveDDDW(dw_main, "loantype_code_1", pbl, null);
                dw_main.SetItemString(1, "membtype_code", membtypecode);
                DataWindowChild dwloantype_code = dw_main.GetChild("loantype_code_1");
                dwloantype_code.SetFilter("LNLOANTYPE.LOANGROUP_CODE   ='02' and LNLOANMBTYPE.MEMBTYPE_CODE ='" + membtypecode + "' ");
                dwloantype_code.Filter();
                dw_main.SetItemString(1, "loantype_code", loantype);

            }
            //wa  เพิ่มเติม
            DwUtil.RetrieveDDDW(dw_main, "loanobjective_code_1", pbl, loantype);
            DataWindowChild dwloantypeobj_code = dw_main.GetChild("loanobjective_code_1");
            dwloantypeobj_code.SetFilter("loantype_code  = '" + loantype + "' ");
            dwloantypeobj_code.Filter();

            string sqllntype = @"select  mangrtpermgrp_code, notmoreshare_flag, loanrighttype_code, customtime_type, loanright_type,  maxloan_amt,loanpermgrp_code,  contint_type,  contractint_rate, inttabrate_code,salperct_balance,salamt_balance,loanpayment_type,loanpayment_status,reqround_factor,payround_factor,
                                   salarybal_status,  lngrpcutright_flag, inttabrate_code, resign_timeadd  from lnloantype  where loantype_code ='" + loantype + @"' ";
            Sdt dtlntype = WebUtil.QuerySdt(sqllntype);
            string intcontinttabcode = "INT01";
            double intcontintrate = 0.065;
            decimal intcontinttype = 2;
            decimal payment_stauts = 1;
            decimal ldc_minpercsal = 0;
            decimal ldc_maxloan = 0;
            decimal customtime_type = 4;
            decimal notmoreshare_flag = 0;
            decimal loanright_type = 2;
            decimal reqround_factor = 0;
            decimal payround_factor = 0;
            decimal lngrpcutright_flag = 0;
            decimal salarybal_status = 0;
            string inttabrate_code = "01";
            string loanpermissgroup = "01";
            string loanrighttype_code = "01";
            string mangrtpermgrp_code = "01";
            decimal ldc_minsalaamt = 0, ldc_paymenttype = 1, resign_timeadd = 0;
            if (dtlntype.Next())
            {

                intcontinttabcode = dtlntype.GetString("inttabrate_code");
                intcontinttype = dtlntype.GetDecimal("contint_type");
                ldc_minpercsal = dtlntype.GetDecimal("salperct_balance");
                ldc_minsalaamt = dtlntype.GetDecimal("salamt_balance");
                ldc_paymenttype = dtlntype.GetDecimal("loanpayment_type");
                ldc_maxloan = dtlntype.GetDecimal("maxloan_amt");
                loanpermissgroup = dtlntype.GetString("loanpermgrp_code");
                payment_stauts = dtlntype.GetDecimal("loanpayment_status");

                reqround_factor = dtlntype.GetDecimal("reqround_factor");
                payround_factor = dtlntype.GetDecimal("payround_factor");
                lngrpcutright_flag = dtlntype.GetDecimal("lngrpcutright_flag");
                inttabrate_code = dtlntype.GetString("inttabrate_code");
                customtime_type = dtlntype.GetDecimal("customtime_type");
                loanright_type = dtlntype.GetDecimal("loanright_type");
                notmoreshare_flag = dtlntype.GetDecimal("notmoreshare_flag");
                mangrtpermgrp_code = dtlntype.GetString("mangrtpermgrp_code");
                resign_timeadd = dtlntype.GetDecimal("resign_timeadd");
                salarybal_status = dtlntype.GetDecimal("salarybal_status");
            }
            string sqlint = @"select interest_rate from lncfloanintratedet where loanintrate_code =(select inttabrate_code   from lnloantype  where loantype_code = '" + loantype + @"') ";
            Sdt dtint = WebUtil.QuerySdt(sqlint);
            while (dtint.Next())
            {

                intcontintrate = dtint.GetDouble("interest_rate");
                // intcontintrate = intcontintrate / 100;
            }
            Hdreqround_factor.Value = reqround_factor.ToString();
            Hdpayround_factor.Value = payround_factor.ToString();
            Hdlngrpcutright_flag.Value = lngrpcutright_flag.ToString();
            Hdinttabrate_code.Value = inttabrate_code;
            Hdcustomtime_type.Value = customtime_type.ToString();
            Hdloanright_type.Value = loanright_type.ToString();
            Hdloanrighttype_code.Value = loanrighttype_code;
            Hdnotmoreshare_flag.Value = notmoreshare_flag.ToString();
            Hdmangrtpermgrp_code.Value = mangrtpermgrp_code;
            Hdresign_timeadd.Value = resign_timeadd.ToString();
            Hdsalarybal_status.Value = salarybal_status.ToString();
            Hdminsalary_amt.Value = ldc_minsalaamt.ToString();
            dw_main.SetItemDecimal(1, "int_continttype", intcontinttype);
            dw_main.SetItemDouble(1, "int_contintrate", intcontintrate);
            dw_main.SetItemString(1, "int_continttabcode", intcontinttabcode);
            dw_main.SetItemDecimal(1, "minsalary_perc", ldc_minpercsal);
            dw_main.SetItemDecimal(1, "minsalary_amt", ldc_minsalaamt);
            dw_main.SetItemDecimal(1, "loanpayment_type", ldc_paymenttype);
            dw_main.SetItemString(1, "instype_code", loanpermissgroup);
            dw_main.SetItemDecimal(1, "loanpayment_status", payment_stauts);
            dw_main.SetItemDecimal(1, "loanmaxreq_amt", ldc_maxloan);

            string sql_lncontsnt = "select rdintsatang_type from lnloanconstant ";
            Sdt dtconsnt = WebUtil.QuerySdt(sql_lncontsnt);
            int li_rountint = 1;
            if (dtconsnt.Next())
            {
                li_rountint = dtconsnt.GetInt32("rdintsatang_type");
            }
            Hdrouninttype.Value = li_rountint.ToString();
        }

        /// <summary>
        /// fillter ประเภทการจ่ายเงิน
        /// </summary>
        private void JsExpenseCode()
        {
            str_itemchange strList = new str_itemchange();
            strList = WebUtil.nstr_itemchange_session(this);
            string expendCode = "";
            try
            {
                expendCode = dw_main.GetItemString(1, "expense_code");
            }
            catch { }
            if (expendCode == null || expendCode == "") return;

            if ((expendCode == "CHQ") || (expendCode == "TRN") || (expendCode == "CBT") || (expendCode == "DRF") || (expendCode == "TBK"))
            {

                //ฝั่งธนาคาร
                dw_main.Modify("t_20.visible =1");
                dw_main.Modify("expense_bank.visible =1");
                dw_main.Modify("t_30.visible =1");
                dw_main.Modify("expense_bank_1.visible =1");

                dw_main.Modify("t_39.visible =1");
                dw_main.Modify("expense_branch.visible =1");
                dw_main.Modify("t_27.visible =1");
                dw_main.Modify("expense_branch_1.visible =1");


                try
                {

                    DwUtil.RetrieveDDDW(dw_main, "expense_bank_1", "sl_loan_requestment.pbl", null);
                    //DwUtil.RetrieveDDDW(dw_main, "expense_branch_1", "sl_loan_requestment_cen.pbl", "006");
                    //DataWindowChild dwExpenseBranch = dw_main.GetChild("expense_branch_1");
                    //DataWindowChild dwExpenseBank = dw_main.GetChild("expense_bank_1");
                    // DwUtil.RetrieveDDDW(dw_main, "loantype_code_1", pbl, null);

                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                }
            }
            else if ((expendCode == "CSH") || (expendCode == "BEX") || (expendCode == "MON") || (expendCode == "MOS") || (expendCode == "MOO"))
            {

                //ฝั่งธนาคาร
                dw_main.Modify("t_20.visible =0");
                dw_main.Modify("expense_bank.visible =0");
                dw_main.Modify("t_30.visible =0");
                dw_main.Modify("expense_bank_1.visible =0");

                dw_main.Modify("t_39.visible =0");
                dw_main.Modify("expense_branch.visible =0");
                dw_main.Modify("t_27.visible =0");
                dw_main.Modify("expense_branch_1.visible =0");


            }
            if ((expendCode == "MON") || (expendCode == "MOS") || (expendCode == "MOO"))
            {
                if ((strList.xml_main == null) || (strList.xml_main == ""))
                {
                    strList.xml_main = dw_main.Describe("DataWindow.Data.XML");
                    strList.xml_main = shrlonService.of_recalfee(state.SsWsPass, strList.xml_main);
                    //นำเข้าข้อมูลหลัก
                    dw_main.Reset();
                    dw_main.ImportString(strList.xml_main, FileSaveAsType.Xml);
                    //DwUtil.ImportData(strList.xml_main, dw_main, tDwMain, FileSaveAsType.Xml);
                    if (dw_main.RowCount > 1) dw_main.DeleteRow(dw_main.RowCount);
                }
            }
        }
        private void Jssavecollmastdata()
        {
            try
            {
                string ref_collno = "", ls_desc = "", ls_collmastno = "", ls_coltypecode = "";
                decimal coll_amt = 0, landestimate_amt = 0, houseestimate_amt = 0;
                Int16 seq_no = 0;

                string loanrequest_docno = dw_main.GetItemString(1, "loanrequest_docno");
                string coop_id = state.SsCoopControl;
                string member_no = dw_main.GetItemString(1, "member_no");
                string sql_del = "delete from lnreqloancoll where loanrequest_docno = '" + loanrequest_docno + "'";
                Sta dta = new Sta(state.SsConnectionString);
                dta.Exe(sql_del);

                string sql_sel = "select max( collmast_no) as collmast_no from lncollmaster ";
                Sdt dtm = WebUtil.QuerySdt(sql_sel);
                if (dtm.Next())
                {
                    ls_collmastno = dtm.GetString("collmast_no");
                }
                // int sql_q = WebUtil.ExeSQL(sql_del);
                for (int i = 1; i <= dw_coll.RowCount; i++)
                {
                    try
                    {
                        ref_collno = dw_coll.GetItemString(i, "ref_collno");
                    }
                    catch { ref_collno = "Auto"; }
                    ls_desc = dw_coll.GetItemString(i, "description");
                    ls_coltypecode = dw_coll.GetItemString(i, "loancolltype_code");
                    landestimate_amt = dw_coll.GetItemDecimal(i, "coll_balance");
                    houseestimate_amt = dw_coll.GetItemDecimal(i, "coll_amt");

                    if (ls_coltypecode == "04")
                    {

                        coll_amt = landestimate_amt + houseestimate_amt;
                        if (ref_collno == "Auto" || ref_collno == "")
                        {
                            ls_collmastno = Convert.ToString(Convert.ToDouble(ls_collmastno) + 1);
                            ls_collmastno = "00000000" + ls_collmastno;
                            ls_collmastno = ls_collmastno.Substring(ls_collmastno.Length - 8, 8);
                            string sql_insertcollm = "insert into lncollmaster ( coop_id, member_no,collmast_no, collmast_refno, collmasttype_code, collmast_desc, landestimate_amt, houseestimate_amt,redeem_flag,loanrequest_docno) values (";
                            sql_insertcollm += "'" + coop_id + "','" + member_no + "','" + ls_collmastno + "','" + ls_desc + "','02','" + ls_desc + "'," + landestimate_amt.ToString() + "," + houseestimate_amt + ",0,'" + loanrequest_docno + "')";
                            dta.Exe(sql_insertcollm);

                            string sql_insertcollmco = "insert into lncollmastmemco( coop_id,collmast_no,memco_no,collmastmain_flag,memcoop_id) values (";
                            sql_insertcollmco += "'" + coop_id + "','" + ls_collmastno + "','" + member_no + "',1,'" + coop_id + "')";
                            dta.Exe(sql_insertcollmco);

                            ref_collno = ls_collmastno;
                        }

                        dw_coll.SetItemString(i, "ref_collno", ref_collno);
                        // dw_coll.SetItemString(i, "loanrequest_docno", loanrequest_docno);
                        // dw_coll.SetItemString(i, "coop_id", coop_id);

                    }
                }
               



            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
            }


        }

        /// <summary>
        /// fillter สาขาธนาคาร
        /// </summary>
        private void JsExpenseBank()
        {
            try
            {

                //wa
                String bankCode;
                try { bankCode = dw_main.GetItemString(1, "expense_bank").Trim(); }
                catch { bankCode = "034"; }
                String bankbranch;
                try { bankbranch = dw_main.GetItemString(1, "expense_branch").Trim(); }
                catch { bankbranch = "0000"; }
 
                DataWindowChild dwExpenseBranch = dw_main.GetChild("expense_branch_1");
                DwUtil.RetrieveDDDW(dw_main, "expense_branch_1", "sl_loan_requestment_cen.pbl", bankCode);
                dwExpenseBranch.SetFilter("CMUCFBANKBRANCH.bank_code ='" + bankCode + "'");
                dwExpenseBranch.Filter();
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        private void JsPaycoopid()
        {
            if (Hdcoopid.Value.Length > 1)
            {
                dw_main.SetItemString(1, "paytoorder_desc", Hdcoopid.Value);
                dw_main.SetItemString(1, "paytoorder_desc_1", Hdcoopid.Value);
            }
            else
            {
                int paycoop_id = Convert.ToInt16(Hdcoopid.Value);
                dw_main.SetItemString(1, "paytoorder_desc", "001" + paycoop_id.ToString("000"));
                dw_main.SetItemString(1, "paytoorder_desc_1", "001" + paycoop_id.ToString("000"));
            }
        }

        private void JsObjective()

        {

            Ltjspopup.Text = " ";
            int li_rowcount = dw_otherclr.RowCount;
            int li_find = dw_otherclr.FindRow("clrothertype_code = 'SHR'", 1, li_rowcount);
            if (li_find < 1)
            {
                li_find = dw_otherclr.InsertRow(0);
            }
            dw_otherclr.SetItemString(li_find, "clrothertype_code", "SHR");
            dw_otherclr.SetItemString(li_find, "clrother_desc", "ซื้อหุ้นเพิ่ม");
            dw_otherclr.SetItemDecimal(li_find, "clear_status", 0);
            dw_otherclr.SetItemDecimal(li_find, "clrother_amt", 0);

            JsSumOthClr();
            //int objective_code = Convert.ToInt16(Hdobjective.Value);
            //dw_main.SetItemString(1, "loanobjective_code", objective_code.ToString("000"));
            //dw_main.SetItemString(1, "loanobjective_code_1", objective_code.ToString("000"));
        }

        /// <summary>
        ///  init ข้อมูลสมาชิก
        /// </summary>
        /// 
        protected int JsCheckLoanrequestwait(ref string as_message)
        {
            try
            {
                String member_no = WebUtil.MemberNoFormat(HdMemberNo.Value);
                dw_main.SetItemString(1, "member_no", member_no);

                string ls_CoopControl = state.SsCoopControl; //สาขาที่ทำรายการ
                String ls_loantype = "";


                ls_loantype = dw_main.GetItemString(1, "loantype_code");
                string lnrequest_date = dw_main.GetItemString(1, "loanrequest_tdate");
                string sql_chkloan = @"select loanrequest_docno, loanrequest_status, loanrequest_date, loanrequest_amt from lnreqloan where loanrequest_status in (11,8) and 
                                        member_no = '" + member_no + "' and  loantype_code = '" + ls_loantype + "'";
                Sdt dtchk = WebUtil.QuerySdt(sql_chkloan);
                if (dtchk.Next()) {
                    string ls_reqloandocno = dtchk.GetString("loanrequest_docno");
                    decimal ldc_loanreqamt = dtchk.GetDecimal("loanrequest_amt");
                    DateTime ldtm_lnreq = dtchk.GetDate("loanrequest_date");
                    ldtm_lnreq = ldtm_lnreq.AddYears(543);
                    String entry_date = ldtm_lnreq.Day.ToString("00") + "/" + ldtm_lnreq.Month.ToString("00") + "/" + ldtm_lnreq.Year.ToString();// +string.Format("0:{dd/mm/yyyy}", ldtm_lnreq);
                   
                    LtServerMessage.Text = WebUtil.ErrorMessage("มีใบคำขอกู้สำหรับวันที่" + entry_date + "แล้ว ระบบจะดึงข้อมูลใบคำขอให้อัตโนมัติ");
                    txt_reqNo.Text = ls_reqloandocno;
                    txt_member_no.Text = member_no;
                    Hdcoopid.Value = ls_CoopControl;
                    string[] arg = new string[2] { ls_reqloandocno, ls_CoopControl };
                    DwUtil.RetrieveDataWindow(dw_main, pbl, null, arg);
                    tDwMain.Eng2ThaiAllRow();
                    DwUtil.RetrieveDataWindow(dw_clear, pbl, null, arg);
                    DwUtil.RetrieveDataWindow(dw_coll, pbl, null, arg);
                    DwUtil.RetrieveDataWindow(dw_otherclr, pbl, null, arg);

                    return -1;
                }

               
            }
            catch { 
            
            }     
            return 1;
        }
        private void JsGetMemberInfo()
        {
            try
            {
                JsReNewPage();
                CbCheckcoop.Checked = false;

                //CheckRemark(WebUtil.MemberNoFormat(HdMemberNo.Value));
                //  Checkcollloop.Checked = false;
                Decimal ldc_sharestk, ldc_periodshramt, ldc_periodshrvalue, ldc_shrvalue, ldc_loanrequeststatus = 0;
                Decimal ldc_salary = 0, ldc_incomemth = 0, ldc_paymonth;
                Decimal ldc_shrstkvalue = 0, allowloan_flag = 0;
                int li_shrpaystatus, li_lastperiod = 0, li_membertype, sequest_status = 0;

                DateTime ldtm_birth = new DateTime(), ldtm_member = new DateTime(), ldtm_work = new DateTime(), ldtm_retry = new DateTime();
                string ls_position, ls_remark, ls_membname, ls_membgroup, ls_groupname;
                string ls_membtypedesc = "", ls_controlname = "", ls_membcontrol = "", ls_appltype, ls_memno, ls_CoopControl;
                Decimal lndroploanall_flag = 0,li_memberstatus = 1, li_resignstatus = 0;
                string ls_messagewarning = " ";
                String member_no = WebUtil.MemberNoFormat(HdMemberNo.Value);
                dw_main.SetItemString(1, "member_no", member_no);

                //

                 if (JsCheckLoanrequestwait(ref ls_messagewarning) == 1)
                {
                    ls_CoopControl = state.SsCoopControl; //สาขาที่ทำรายการ

                    string as_xmlmessage = "";
                    //wa
                    JsSetDeptnodefault(1);
                   // JsSetExpenseDefault(1, member_no);
                    String ls_loantype = "";
                    try
                    {
                        //เช็คสิทธิ์ ก่อนขอกู้ว่ามีสัญญาเก่าค้างหรือไม่
                        int checkoldloanpayment = wcf.NShrlon.of_checkoldloanpayment(state.SsWsPass, ref as_xmlmessage, state.SsCoopControl, member_no, ls_loantype);
                        //ตรวจสอบการเคยผิดนัดชำระรายเดือน
                        string sql_arr = "select a.slip_date, a.principal_payment + a.interest_payment as item_pay from lncontstatement a, lncontmaster b where a.loanitemtype_code = 'RPM' and  a.loancontract_no = b.loancontract_no and b.member_no = '" + member_no + "'";
                        Sdt dtarr = WebUtil.QuerySdt(sql_arr);
                        string slip_date = "";
                        decimal item_pay = 0;
                        if (dtarr.Next()) { 
                            slip_date = Convert.ToString( dtarr.GetDateEn("slip_date"));
                            item_pay = dtarr.GetDecimal("item_pay");
                        
                        }
                        dtarr.Dispose();
                        if (checkoldloanpayment != 1)   //มีสัญญาเก่าค้าง
                        {
                            if ((as_xmlmessage != "") && (as_xmlmessage != null))
                            {
                                LtServerMessage.Text = WebUtil.ErrorMessage(as_xmlmessage);
                            }

                        }
                        else    //ไม่มีสัญญาเก่าค้าง
                        {
                            String ls_memcoopid;
                            if (HdMemcoopId.Value == "")
                            {
                                ls_memcoopid = state.SsCoopControl;
                            }
                            else
                            {
                                ls_memcoopid = HdMemcoopId.Value;
                            }

                            String sqlstr = @"   SELECT a.membgroup_control,
                                                b.membgroup_desc as control_desc,
                                                a.membgroup_code,
                                                a.membgroup_desc , 
                                             MBMEMBMASTER.BIRTH_DATE,   
                                             MBMEMBMASTER.MEMBER_DATE,   
                                             MBMEMBMASTER.WORK_DATE,   
                                             MBMEMBMASTER.RETRY_DATE,   
                                             MBMEMBMASTER.SALARY_AMOUNT,   
                                             MBMEMBMASTER.INCOMEETC_AMT,    
                                             MBMEMBMASTER.MEMBTYPE_CODE,   
                                             MBUCFMEMBTYPE.MEMBTYPE_DESC,   
                                             SHSHAREMASTER.LAST_PERIOD,   
                                             SHSHAREMASTER.PERIODSHARE_AMT,   
                                             mbucfprename.prename_desc||mbmembmaster.memb_name||'   '||mbmembmaster.memb_surname as member_name,   
                                             SHSHAREMASTER.PAYMENT_STATUS,   
                                             MBMEMBMASTER.POSITION_DESC,   
                                             MBMEMBMASTER.POSITION_CODE,   
                                             MBMEMBMASTER.REMARK,   
                                             MBMEMBMASTER.MEMBER_STATUS,   
                                             MBMEMBMASTER.RESIGN_STATUS,   
                                             SHSHAREMASTER.SHARESTK_AMT,   
                                             SHSHARETYPE.UNITSHARE_VALUE,   
                                             MBMEMBMASTER.MEMBER_TYPE,   
                                             MBMEMBMASTER.APPLTYPE_CODE,   MBMEMBMASTER.dropgurantee_flag,
                                             MBMEMBMASTER.RETRY_STATUS,      MBMEMBMASTER.DROPLOANALL_FLAG, 
                                             MBMEMBMASTER.MEMBER_NO,   
                                             MBMEMBMASTER.PAUSEKEEP_FLAG,   
                                             MBMEMBMASTER.PAUSEKEEP_DATE,       SHSHAREMASTER.sequest_status,
                                             MBMEMBMASTER.COOP_ID  ,MBMEMBMASTER.CREMATION_STATUS, MBMEMBMASTER.allowloan_flag
                               FROM MBMEMBMASTER,   
                                     MBUCFMEMBGROUP a ,   MBUCFMEMBGROUP b,
                                     MBUCFPRENAME,
                                     MBUCFMEMBTYPE,
                                     SHSHAREMASTER,   
                                     SHSHARETYPE  
                               WHERE ( a.MEMBGROUP_CODE = MBMEMBMASTER.MEMBGROUP_CODE ) and  a.membgroup_control = b.membgroup_code(+) and
                                        a.coop_id = b.coop_id(+) and 
                                    ( SHSHAREMASTER.MEMBER_NO = MBMEMBMASTER.MEMBER_NO ) and    
                                     ( MBMEMBMASTER.PRENAME_CODE = MBUCFPRENAME.PRENAME_CODE ) and
                                     ( MBMEMBMASTER.MEMBTYPE_CODE = MBUCFMEMBTYPE.MEMBTYPE_CODE ) and  
                                     ( SHSHAREMASTER.SHARETYPE_CODE = SHSHARETYPE.SHARETYPE_CODE ) and  
                                     ( MBMEMBMASTER.COOP_ID = a.COOP_ID ) and     ( MBMEMBMASTER.MEMBER_STATUS=1) AND
                                     ( MBMEMBMASTER.COOP_ID = SHSHAREMASTER.COOP_ID ) and  
                                     ( SHSHAREMASTER.COOP_ID = SHSHARETYPE.COOP_ID ) and  
                                     ( ( mbmembmaster.member_no = '" + member_no + @"' ) AND  
                                     ( shsharemaster.sharetype_code = '01' ) )    and  
                                     MBMEMBMASTER.COOP_ID   ='" + ls_memcoopid + @"' ";
                            Sdt dt = WebUtil.QuerySdt(sqlstr);//เป็น service

                            if (dt.GetRowCount() < 1) {
                                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบข้อมูลสมาชก หรอ สมาชิกท่านได้ปิดบัญชีสมาชิกแล้ว");
                            }
                            while (dt.Next())
                            {
                                try
                                {
                                    li_cramationstatus = dt.GetInt32("cremation_status");
                                }
                                catch { li_cramationstatus = 0; }

                                try
                                {
                                    lndroploanall_flag = dt.GetDecimal("DROPLOANALL_FLAG");
                                }
                                catch { lndroploanall_flag = 0; }
                                try { sequest_status = dt.GetInt32("sequest_status"); }
                                catch { sequest_status = 0; }
                                ls_membname = dt.GetString("member_name");
                                ls_membcontrol = dt.GetString("membgroup_control");
                                ls_controlname = dt.GetString("control_desc");
                                ls_membgroup = dt.GetString("membgroup_code");
                                ls_groupname = dt.GetString("membgroup_desc");
                                ldc_salary = dt.GetDecimal("salary_amount");
                                li_lastperiod = dt.GetInt32("last_period");
                                li_memberstatus = dt.GetInt32("member_status");
                                li_resignstatus = dt.GetInt32("resign_status");
                                try
                                {
                                    ldtm_birth = dt.GetDate("birth_date");
                                }
                                catch { }
                                try
                                {
                                    ///<หาวันที่เกษียณ>
                                   // ldtm_retry = wcf.NShrlon.of_calretrydate(state.SsWsPass,ldtm_birth);
                                   // ldtm_retry = Convert.ToDateTime(of_getrerydate(ldtm_birth));
                                    string retry_date = of_getrerydate(ldtm_birth);
                                    ldtm_retry = DateTime.ParseExact(retry_date, "dd-MM-yyyy", null);
                                }
                                catch { }
                                try
                                {
                                    ldtm_member = dt.GetDate("member_date");
                                }
                                catch { }
                                try
                                {
                                    ldtm_work = dt.GetDate("work_date");
                                }
                                catch { }
                                try
                                {
                                    ldc_incomemth = dt.GetDecimal("incomeetc_amt");
                                }
                                catch
                                {
                                    ldc_incomemth = 0;
                                }

                                ldc_paymonth = 0;
                                ls_position = dt.GetString("position_desc");
                                ls_remark = dt.GetString("remark");
                                ldc_shrvalue = dt.GetDecimal("unitshare_value");
                                ldc_sharestk = dt.GetDecimal("sharestk_amt");
                                ldc_periodshramt = dt.GetDecimal("periodshare_amt");
                                li_shrpaystatus = dt.GetInt32("payment_status");
                                li_membertype = dt.GetInt32("member_type");
                                ls_appltype = dt.GetString("appltype_code");
                                ls_memno = dt.GetString("member_no");
                                //ls_memcoopid = dt.GetString("coop_id");

                                ls_membtype = dt.GetString("membtype_code");
                                ls_membtypedesc = dt.GetString("membtype_desc");
                                allowloan_flag = dt.GetDecimal("allowloan_flag");
                                ldc_shrstkvalue = Convert.ToDecimal((Convert.ToInt32(ldc_shrvalue) * Convert.ToInt32(ldc_sharestk)));
                                ldc_periodshrvalue = Convert.ToDecimal((Convert.ToInt32(ldc_periodshramt) * Convert.ToInt32(ldc_shrvalue)));

                                if (li_resignstatus == 1) {
                                    LtServerMessage.Text = WebUtil.ErrorMessage("สมาชิกท่านได้ลาออกจากสหกรณ์แล้ว กรุณาตรวจสอบ");
                                }
                                if (allowloan_flag == 1)
                                {
                                    LtServerMessage.Text += WebUtil.ErrorMessage("สมาชิกท่านนี้ได้ทำหนังสือยินยอมแล้ว");
                                }
                                else {
                                    LtServerMessage.Text += WebUtil.ErrorMessage("สมาชิกท่านนี้ ยังไม่ได้ทำหนังสือยินยอม  กรุณาตรวจสอบ");
                                }

                                //dw_main.SetItemString(1, "memcoop_id", ls_memcoopid);
                                dw_main.SetItemString(1, "coop_id", state.SsCoopId);
                                dw_main.SetItemString(1, "member_name", ls_membname);
                                dw_main.SetItemString(1, "mbucfmembgroup_membgroup_code", ls_membgroup);
                                dw_main.SetItemString(1, "mbucfmembgroup_membgroup_desc", ls_groupname);
                                dw_main.SetItemString(1, "membgroup_desc", ls_controlname);
                                dw_main.SetItemDecimal(1, "salary_amt", ldc_salary);
                                dw_main.SetItemDecimal(1, "share_lastperiod", li_lastperiod);
                                dw_main.SetItemDateTime(1, "birth_date", ldtm_birth);
                                dw_main.SetItemDateTime(1, "member_date", ldtm_member);
                                //dw_main.SetItemString(1, "membtype_code", ls_membtype);
                                //dw_main.SetItemString(1, "membtype_desc", ls_membtypedesc);
                                try
                                {
                                    dw_main.SetItemDateTime(1, "retry_date", ldtm_retry);
                                }
                                catch { dw_main.SetItemDateTime(1, "retry_date", DateTime.Now); }

                                dw_main.SetItemDecimal(1, "incomemonth_other", ldc_incomemth);
                                dw_main.SetItemDecimal(1, "incomemonth_fixed", 0);
                                dw_main.SetItemDecimal(1, "paymonth_other", ldc_paymonth);
                                dw_main.SetItemString(1, "position_desc", ls_position);
                                dw_main.SetItemString(1, "remark", "");
                                dw_main.SetItemString(1, "member_remark", ls_remark);

                                dw_main.SetItemDecimal(1, "sharestk_value", ldc_shrstkvalue);
                                dw_main.SetItemDecimal(1, "periodshare_value", ldc_periodshrvalue);
                                dw_main.SetItemDecimal(1, "sharepay_status", li_shrpaystatus);
                                dw_main.SetItemDecimal(1, "intestimate_amt", 0);
                                dw_main.SetItemDecimal(1, "member_type", li_membertype);
                                dw_main.SetItemString(1, "appltype_code", ls_appltype);

                                //คำนวณอายสมาชิก
                                JsCalageyearmonth();
                                string loantype_code = dw_main.GetItemString(1, "loantype_code");
                                string sqlpauseloan = "select  loantype_code , pauseloan_cause, expirefix_flag, expirefix_date  from  lnmembpauseloan  where member_no =  '" + member_no + "'   and  loantype_code =  '" + loantype_code + "'";
                                Sdt dtp = WebUtil.QuerySdt(sqlpauseloan);
                                string pauseloan_desc = "";
                                DateTime expirefix_date;
                                decimal expirefix_flag = 0;
                                if (dtp.Next()) {
                                    pauseloan_desc = dtp.GetString("pauseloan_cause");
                                    expirefix_flag = dtp.GetDecimal("expirefix_flag");
                                    expirefix_date = dtp.GetDate("expirefix_date");
                                    if (expirefix_flag == 1)
                                    {
                                        LtServerMessage.Text += WebUtil.ErrorMessage("สมาชิกท่านนี้ได้ถูกห้ามกู้ประเภทนี้อยู่  " + loantype_code + " เหตผล " + pauseloan_desc + "  ถึงวันที่ " + Convert.ToString(expirefix_date.ToLongDateString()));
                                    }
                                    else {
                                        LtServerMessage.Text += WebUtil.ErrorMessage("สมาชิกท่านนี้ได้ถูกห้ามกู้ประเภทนี้อยู่  " + loantype_code + " เหตผล " + pauseloan_desc);
                                    }
                                }
                                dtp.Dispose();
                            }

                            dt.Dispose();
                           
                           // SetAcci_dept();//เซ็ค เลขที่บัญชี
                            Jscheckloancontarrer();
                            if (sequest_status == 1) //&& loantype == "23"
                            {

                                LtServerMessage.Text += WebUtil.ErrorMessage("สมาชิกท่านนี้ ศาลอายัดหุ้นไว้ ไม่สามารถกู้ได้");
                            }
                            if (lndroploanall_flag != 0)
                            {

                                LtServerMessage.Text += WebUtil.ErrorMessage("สมาชิกท่านนี้ งดกู้");
                            }
                            else
                            {
                                ///<สิทธิ์กู้สูดสุง>
                                ///
                               // string re_lastperiod = wcf.InterPreter.GetlastperiodShare(li_lastperiod);
                                //if (re_lastperiod == "1")
                                //{
                                Jsmaxcreditperiod();
                                //}
                                //else { LtServerMessage.Text = WebUtil.ErrorMessage("งวดการส่งค่าหุ้นหรือ อายุการเป็นสมาชิก ของสมาชิกท่านนี้ยังไม่ถึงเกณฑ์ที่กำหนด"); }
                                ///<ตรวจหาสัญญาที่จะหักกลบ ถ้ามีก็เอามาแสดง ใน dw_clear>
                                JsSetExpenseDefault(1, member_no);
                                JsCheckpauseloan();
                            }


                        } //end เช็คสิทธิ์ ก่อนขอกู้ว่ามีสัญญาเก่าค้างหรือไม่


                    }
                    catch( Exception exxx)
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถทำรายการ" + exxx.ToString());
                    }


                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("กรุณาเลือกประเภทเงินกู้ที่จะทำรายการ" + ex);
            }
                //string sql = "select count(loancontract_no) as Cloancontract_no  from lncontmaster "+
                //             "where loantype_code = '" + dw_main.GetItemString(1, "loantype_code") + "' "+
                //             "and member_no = '" + dw_main.GetItemString(1, "member_no") + "' and contract_status <> -9";
                //Sdt dtw = WebUtil.QuerySdt(sql);
                //if (dtw.Next())
                //{
                //    decimal onlinefee_amt = Convert.ToDecimal(dtw.GetString("Cloancontract_no"));
                //    dw_main.SetItemDecimal(1, "onlinefee_amt", onlinefee_amt);
                //}
            
            HdShowRemark.Value = "true";
            CheckRemark(WebUtil.MemberNoFormat(HdMemberNo.Value));
           
        }

        /// <summary>
        /// ดึงข้อมูลสัญญาหักกลบ
        /// </summary>
        /// 
        private void of_setloanclearstatus()
        {
            //wa
            Ltjspopupclr.Text = "";
            for (int k = 1; k <= dw_clear.RowCount; k++)
            {

                dw_clear.SetItemDecimal(k, "clear_status", 0);

            }

            //ตั้งค่าการหักชำระหนี้เก่า
            string loantypeclr, loantype, contclr_no;
            string ls_memcoopid = dw_main.GetItemString(1, "memcoop_id");
            String member_no = dw_main.GetItemString(1, "member_no");
            decimal ldc_minpay, li_minperiod, li_checkcontclr, last_payperid, li_calintflag = 0;
            string loantypereq_code = dw_main.GetItemString(1, "loantype_code");
            String sqlStr1 = @"SELECT LNLOANTYPECLR.LOANTYPE_CODE,   
                                 LNLOANTYPECLR.LOANTYPE_CLEAR,   
                                 LNLOANTYPECLR.MINPERIOD_PAY,   
                                 LNLOANTYPECLR.MINPERCENT_PAY,   
                                 LNLOANTYPECLR.CHKCONTCREDIT_FLAG,   
                                 LNLOANTYPE.LOANRIGHT_TYPE,   
                                 LNLOANTYPECLR.CONTRACT_STATUS  ,calint_flag
                            FROM LNLOANTYPECLR,   
                                 LNLOANTYPE  
                           WHERE ( LNLOANTYPECLR.LOANTYPE_CLEAR = LNLOANTYPE.LOANTYPE_CODE ) and  
                                 ( LNLOANTYPECLR.COOP_ID = LNLOANTYPE.COOP_ID ) and  
                                 ( ( LNLOANTYPECLR.LOANTYPE_CODE = '" + loantypereq_code + "' ) )    ";

            Sdt dt1 = WebUtil.QuerySdt(sqlStr1);
            if (dt1.Next())
            {
                for (int i = 0; i < dt1.GetRowCount(); i++)
                {
                    loantypeclr = Convert.ToString(dt1.Rows[i]["LOANTYPE_CLEAR"]);
                    li_minperiod = Convert.ToDecimal(dt1.Rows[i]["minperiod_pay"]);
                    ldc_minpay = Convert.ToDecimal(dt1.Rows[i]["minpercent_pay"]);
                    li_checkcontclr = Convert.ToDecimal(dt1.Rows[i]["contract_status"]);
                    li_calintflag = Convert.ToDecimal(dt1.Rows[i]["calint_flag"]);
                    for (int k = 1; k <= dw_clear.RowCount; k++)
                    {
                        contclr_no = dw_clear.GetItemString(k, "loancontract_no");
                        loantype = dw_clear.GetItemString(k, "loantype_code");
                        last_payperid = dw_clear.GetItemDecimal(k, "last_periodpay");
                        if (loantype == loantypeclr) //&& li_minperiod <= last_payperid
                        {
                            dw_clear.SetItemDecimal(k, "clear_status", 1);
                            if (li_minperiod > last_payperid)
                            {
                                Ltjspopupclr.Text += WebUtil.ErrorMessage(contclr_no + " สัญญาชำระไม่ถึงตามเกณฑ์ที่กำหนด ต้องส่งมาแล้วขั้นต่ำ " + li_minperiod.ToString());
                            }
                            if (li_calintflag == 0)
                            {
                                dw_clear.SetItemDecimal(k, "intestimate_amt", 0);
                                dw_clear.SetItemDecimal(k, "intclear_amt", 0);
                            }
                        }
                        
                       
                    }
                }
            }



        }
        private double of_getinterestrate(string loantype_code)
        {
            double intrate = 0;
            string ls_intratetab =  wcf.NBusscom.of_getattribloantype(state.SsWsPass, loantype_code, "inttabrate_code").ToString();
            String sqlint = @"  SELECT 
                                         INTEREST_RATE        
                                    FROM LNCFLOANINTRATEDET  
                                   WHERE ( COOP_ID ='" + state.SsCoopControl + @"'  ) AND  
                                         ( LOANINTRATE_CODE ='" + ls_intratetab + "'  ) order by effective_date ";
            Sdt dtint = WebUtil.QuerySdt(sqlint);
           while (dtint.Next())
           {
                intrate = dtint.GetDouble("INTEREST_RATE");
               
            }
            return intrate;
        }
        private void JsrecallInt()
        {
            //tDwMain.Eng2ThaiAllRow();
            DateTime loanrcvfix_date = dw_main.GetItemDate(1, "loanrcvfix_date");
            Decimal loanrequest_amt = dw_main.GetItemDecimal(1, "loanrequest_amt");
            DateTime startkeep_date = dw_main.GetItemDate(1, "startkeep_date");
            Decimal intrate = 0;
            string ls_loantype = dw_main.GetItemString(1, "loantype_code");
            Genbaseloanclear();

            string ls_intratetab = wcf.NBusscom.of_getattribloantype(state.SsWsPass, ls_loantype, "inttabrate_code").ToString();
            String sqlint = @"  SELECT 
                                         INTEREST_RATE        
                                    FROM LNCFLOANINTRATEDET  
                                   WHERE ( COOP_ID ='" + state.SsCoopControl + @"'  ) AND  
                                         ( LOANINTRATE_CODE ='" + ls_intratetab + "'  ) ";
            Sdt dtint = WebUtil.QuerySdt(sqlint);
            if (dtint.Next())
            {
                intrate = dtint.GetDecimal("INTEREST_RATE");

            }
            Decimal day_int = startkeep_date.DayOfYear - loanrcvfix_date.DayOfYear;
            Decimal intestimate_amt = Math.Round(loanrequest_amt * intrate * day_int / 365);
            dw_main.SetItemDecimal(1, "intestimate_amt", intestimate_amt);
        }

        private void Genbaseloanclear()
        {

            Ltjspopupclr.Text = "";

            String member_no = dw_main.GetItemString(1, "member_no");
            DateTime loanrcvfix_date = dw_main.GetItemDate(1, "loanrcvfix_date");
            String ls_memcoopid;
            try { ls_memcoopid = dw_main.GetItemString(1, "memcoop_id"); }
            catch { ls_memcoopid = state.SsCoopControl; }

            string ls_contno, ls_conttype, ls_prefix, ls_permgrp, ls_loantype = "", ls_intcontintcode, ls_coopid = "";
            Decimal     li_minperiod = 0, li_period, li_continttype, li_transfersts = 0, ldc_intestim30 = 0;
            Decimal li_paytype,  li_contstatus, li_intcontinttype, li_intsteptype;
            Decimal  li_periodamt, li_contlaw, li_paystatus, li_clearinsure,  li_od_flag = 0;
            Decimal ldc_appvamt, ldc_balance = 0, ldc_withdrawable, ldc_rkeepprin, ldc_rkeepint, ldc_transbal;
            Decimal ldc_intarrear, ldc_payment, ldc_intestim;
            Decimal ldc_minpay = 0, ldc_intrate, ldc_intcontintrate, ldc_intincrease;
            DateTime ldtm_lastcalint, ldtm_lastproc,  ldtm_approve, ldtm_startcont ;
            int li_interestmethod = 0, li_roundtype = 1;
            ls_loantype = dw_main.GetItemString(1, "loantype_code");
            li_roundtype = Convert.ToInt16(Hdrouninttype.Value);
            String sqlStr = @"   SELECT LNCONTMASTER.LOANCONTRACT_NO,   
                                             LNCONTMASTER.MEMBER_NO,   
                                             LNCONTMASTER.LOANTYPE_CODE,   
                                             LNCONTMASTER.LOANAPPROVE_AMT,   
                                             LNCONTMASTER.WITHDRAWABLE_AMT,   
                                             LNCONTMASTER.PRINCIPAL_BALANCE,   
                                             LNCONTMASTER.LAST_PERIODPAY,   
                                             LNCONTMASTER.LASTCALINT_DATE,   
                                             LNCONTMASTER.LASTPROCESS_DATE,   
                                             LNCONTMASTER.INTEREST_ARREAR,   
                                             LNCONTMASTER.RKEEP_PRINCIPAL,   
                                             LNCONTMASTER.RKEEP_INTEREST,   
                                             LNLOANTYPE.PREFIX,   
                                             LNCONTMASTER.LOANPAYMENT_TYPE,   
                                             LNCONTMASTER.PERIOD_PAYMENT,   
                                             LNLOANTYPE.LOANPERMGRP_CODE,   
                                             LNCONTMASTER.CONTRACT_STATUS,   
                                            LNCONTMASTER.INT_CONTINTTYPE as CONTRACTINT_TYPE,   
                                             LNCONTMASTER.INT_CONTINTRATE as  CONTRACT_INTEREST,   
                                             LNCONTMASTER.LOANAPPROVE_DATE,   
                                             LNCONTMASTER.STARTCONT_DATE,   
                                             LNCONTMASTER.INT_CONTINTTYPE,   
                                             LNCONTMASTER.INT_CONTINTRATE,   
                                             LNCONTMASTER.INT_CONTINTTABCODE,   
                                             LNCONTMASTER.INT_CONTINTINCREASE,   
                                             LNCONTMASTER.INT_INTSTEPTYPE,   
                                             LNCONTMASTER.PERIOD_PAYAMT,   
                                             LNCONTMASTER.CONTLAW_STATUS,   
                                             LNCONTMASTER.PRINCIPAL_TRANSBAL,   
                                             LNCONTMASTER.PAYMENT_STATUS,   
                                             LNLOANTYPE.CLEARINSURE_FLAG,   
                                             LNCONTMASTER.INSURECOLL_FLAG   ,
                                             LNLOANTYPE.interest_method,
                                             LNLOANTYPE.od_flag, LNLOANTYPE.shrstk_buytype
                                        FROM LNCONTMASTER,   
                                             LNLOANTYPE  
                                       WHERE ( LNCONTMASTER.LOANTYPE_CODE = LNLOANTYPE.LOANTYPE_CODE ) and  
                                             ( LNCONTMASTER.COOP_ID = LNLOANTYPE.COOP_ID ) and  
                                             ( ( lncontmaster.member_no = '" + member_no + @"' ) AND  
                                             ( lncontmaster.principal_balance + lncontmaster.withdrawable_amt > 0 ) AND  
                                             ( lncontmaster.contract_status > 0 ) AND  
                                             ( LNCONTMASTER.MEMCOOP_ID = '" + ls_memcoopid + @"' ) )   
                                    ORDER BY LNCONTMASTER.LOANCONTRACT_NO ASC  ";
            Sdt dt = WebUtil.QuerySdt(sqlStr);
            if (dt.Next())
            {
                dw_clear.Reset();

                int rowCount = dt.GetRowCount();
                for (int i = 0; i < rowCount; i++)
                {
                    try { ls_contno = dt.Rows[i]["loancontract_no"].ToString(); }
                    catch { ls_contno = ""; }
                    try
                    {
                        ls_conttype = dt.Rows[i]["loantype_code"].ToString();
                    }
                    catch { ls_conttype = ""; }
                    try
                    {
                        ls_prefix = dt.Rows[i]["prefix"].ToString();
                    }
                    catch { ls_prefix = ""; }
                    try
                    {// คำนำหน้าประเภทสัญญา
                        ls_permgrp = dt.Rows[i]["loanpermgrp_code"].ToString();
                    }
                    catch { ls_permgrp = ""; }
                    try
                    {// กลุ่มวงเงินกู้
                        li_paytype = Convert.ToDecimal(dt.Rows[i]["loanpayment_type"]);
                    }
                    catch { li_paytype = 0; }
                    try
                    {
                        li_period = Convert.ToDecimal(dt.Rows[i]["last_periodpay"]);
                    }
                    catch { li_period = 0; }
                    try
                    {
                        li_contstatus = Convert.ToDecimal(dt.Rows[i]["contract_status"]);
                    }
                    catch { li_contstatus = 0; }
                    try
                    {
                        li_continttype = Convert.ToDecimal(dt.Rows[i]["contractint_type"]);

                    }
                    catch { li_continttype = 0; }
                    try
                    {
                        ldc_intrate = Convert.ToDecimal(dt.Rows[i]["contract_interest"]);
                    }
                    catch { ldc_intrate = 0; }
                    try
                    {
                        ldc_payment = Convert.ToDecimal(dt.Rows[i]["period_payment"]);
                    }
                    catch { ldc_payment = 0; }
                    try
                    {
                        ldc_appvamt = Convert.ToDecimal(dt.Rows[i]["loanapprove_amt"]);
                    }
                    catch { ldc_appvamt = 0; }
                    try
                    {
                        ldc_withdrawable = Convert.ToDecimal(dt.Rows[i]["withdrawable_amt"]);
                    }
                    catch { ldc_withdrawable = 0; }
                    try
                    {
                        ldc_balance = Convert.ToDecimal(dt.Rows[i]["principal_balance"]);
                    }
                    catch { ldc_balance = 0; }
                    try
                    {
                        ldc_intarrear = Convert.ToDecimal(dt.Rows[i]["interest_arrear"]);
                    }
                    catch { ldc_intarrear = 0; }
                    try
                    {
                        ldc_rkeepprin = Convert.ToDecimal(dt.Rows[i]["rkeep_principal"]);
                    }
                    catch { ldc_rkeepprin = 0; }
                    try
                    {
                        ldc_rkeepint = Convert.ToDecimal(dt.Rows[i]["rkeep_interest"]);
                    }
                    catch { ldc_rkeepint = 0; }
                    try
                    {
                        ldtm_lastcalint = Convert.ToDateTime(dt.Rows[i]["lastcalint_date"]);
                    }
                    catch { ldtm_lastcalint = DateTime.Now; }
                    try
                    {
                        ldtm_lastproc = Convert.ToDateTime(dt.Rows[i]["lastprocess_date"]);
                    }
                    catch { ldtm_lastproc = DateTime.Now; }
                    try
                    {
                        ldtm_approve = Convert.ToDateTime(dt.Rows[i]["loanapprove_date"]);
                    }
                    catch { ldtm_approve = DateTime.Now; }
                    try
                    {
                        ldtm_startcont = Convert.ToDateTime(dt.Rows[i]["startcont_date"]);
                    }
                    catch { ldtm_startcont = DateTime.Now; }
                    try
                    {
                        li_intcontinttype = Convert.ToDecimal(dt.Rows[i]["int_continttype"]);
                    }
                    catch { li_intcontinttype = 0; }
                    try
                    {
                        ldc_intcontintrate = Convert.ToDecimal(dt.Rows[i]["int_contintrate"]);
                    }
                    catch { ldc_intcontintrate = 0; }
                    try
                    {
                        ls_intcontintcode = dt.Rows[i]["int_continttabcode"].ToString();
                    }
                    catch { ls_intcontintcode = ""; }
                    try
                    {
                        ldc_intincrease = Convert.ToDecimal(dt.Rows[i]["int_contintincrease"]);
                    }
                    catch { ldc_intincrease = 0; }
                    try
                    {
                        li_intsteptype = Convert.ToDecimal(dt.Rows[i]["int_intsteptype"]);
                    }
                    catch { li_intsteptype = 0; }
                    try
                    {
                        li_periodamt = Convert.ToDecimal(dt.Rows[i]["period_payamt"]);
                    }
                    catch { li_periodamt = 0; }
                    try
                    {
                        li_transfersts = Convert.ToDecimal(dt.Rows[i]["transfer_status"]);
                    }
                    catch { li_transfersts = 0; }
                    try
                    {
                        ls_coopid = dt.Rows[i]["coop_id"].ToString();
                    }
                    catch { ls_coopid = ls_memcoopid; }
                    try
                    {

                        //li_contlaw = Convert.ToDecimal(dt.Rows[i]["contlaw_status"]);
                        li_contlaw = Convert.ToDecimal(dt.Rows[i]["shrstk_buytype"]); //wa เปลียนเป็น ธกส 1 2 ให้นับรวมหนี้เพือคิดหุ้น
                    }
                    catch { li_contlaw = 0; }
                    try
                    {
                        ldc_transbal = Convert.ToDecimal(dt.Rows[i]["principal_transbal"]);
                    }
                    catch { ldc_transbal = 0; }
                    try
                    {
                        li_paystatus = Convert.ToDecimal(dt.Rows[i]["payment_status"]);
                    }
                    catch { li_paystatus = 0; }
                    try
                    {
                        li_clearinsure = Convert.ToDecimal(dt.Rows[i]["insurecoll_flag"]);
                    }
                    catch { li_clearinsure = 0; }

                    try
                    {
                        li_interestmethod = Convert.ToInt16(dt.Rows[i]["interest_method"]);
                    }
                    catch { li_interestmethod = 1; }
                    try
                    {
                        li_od_flag = Convert.ToInt16(dt.Rows[i]["od_flag"]);
                    }
                    catch { li_od_flag = 0; }

                  //  ldc_intcontintrate = dw_main.GetItemDecimal(1, "int_contintrate");
                    
                    loanrcvfix_date = dw_main.GetItemDateTime(1, "loanrequest_date");
                    decimal ldc_day = loanrcvfix_date.Day;
                    decimal ldc_month = loanrcvfix_date.Month;
                    decimal ldc_year = loanrcvfix_date.Year;
                    decimal ldc_dayfix = 15;

                    // ธกส เปลี่ยนการใส่อัตราดอกเบี้ย จาก 0.05  เป็น 5 
                    ldc_intcontintrate = Convert.ToDecimal(of_getinterestrate(ls_loantype));
                    //ธกส เปลี่ยนการใส่อัตรราดอกเบี้ย 
                    ldc_intcontintrate = ldc_intcontintrate / 100;
                    if (li_interestmethod == 1)
                    {
                        //คิด ดบ. รายวัน
                        Decimal day_int = Convert.ToDecimal((loanrcvfix_date - ldtm_lastcalint).TotalDays);
                        if (day_int < 0) { day_int = 0; }
                        //if (ldc_intcontintrate == 0)
                        //{
                           // ldc_intcontintrate = Convert.ToDecimal(of_getinterestrate(ls_loantype));
                           ////ธกส เปลี่ยนการใส่อัตรราดอกเบี้ย 
                           // ldc_intcontintrate = ldc_intcontintrate / 100;
                        //}
                        ldc_intestim = ldc_balance * (ldc_intcontintrate) * day_int / 365;
                       // ldc_intestim = wcf.NShrlon.of_roundmoney(state.SsWsPass, ldc_intestim);
                        ldc_intestim = JsRoundMoney(ldc_intestim, li_roundtype);
                        ldc_intestim30 = ldc_balance * (ldc_intcontintrate) * 30 / 365;
                       // ldc_intestim30 = wcf.NShrlon.of_roundmoney(state.SsWsPass, ldc_intestim30);
                        ldc_intestim30 = JsRoundMoney(ldc_intestim30, li_roundtype);
                    }
                    else
                    {
                       
                        //surin  ประเภทเดียวกัน ก่อน วันที่ 12 ไม่คิด  13-15 คิด คึ่งเดือน 16-31 คิดครึ่งเดือน
                        //ต่างประเภทกัน ก่อน 15 คิด ครึ่งเดือน หลัง 15 คิด 1 เดือน
                        if (ldc_day <= 15)
                        {
                            if (ls_conttype != ls_loantype)
                            {
                                if ((ls_loantype == "41" && ls_conttype == "44") || (ls_loantype == "44" && ls_conttype == "41"))
                                {
                                    ldc_intestim = 0;
                                }
                                else
                                {
                                    //ldc_intestim = wcf.NShrlon.of_roundmoney(state.SsWsPass, ldc_balance * (ldc_intcontintrate) * (Convert.ToDecimal(0.5)) / 12);
                                    ldc_intestim = JsRoundMoney(ldc_balance * (ldc_intcontintrate) * (Convert.ToDecimal(0.5)) / 12, li_roundtype);
                                }
                            }
                            else
                            {
                                ldc_intestim = 0;
                            }

                        }
                        else
                        {
                            if (ls_conttype != ls_loantype)
                            {
                                if ((ls_loantype == "41" && ls_conttype == "44") || (ls_loantype == "44" && ls_conttype == "41"))
                                {

                                    //ldc_intestim = wcf.NShrlon.of_roundmoney(state.SsWsPass, (ldc_balance * (ldc_intcontintrate) / 12) / 2);
                                    ldc_intestim = JsRoundMoney((ldc_balance * (ldc_intcontintrate) / 12) / 2, li_roundtype);
                                }
                                else
                                {
                                   
                                    //ldc_intestim = wcf.NShrlon.of_roundmoney(state.SsWsPass, ldc_balance * (ldc_intcontintrate) / 12);
                                    ldc_intestim = JsRoundMoney( ldc_balance * (ldc_intcontintrate) / 12, li_roundtype );
                                }
                            }
                            else
                            {
                              //  ldc_intestim = wcf.NShrlon.of_roundmoney(state.SsWsPass, (ldc_balance * (ldc_intcontintrate) / 12) / 2);
                                ldc_intestim = JsRoundMoney((ldc_balance * (ldc_intcontintrate) / 12) / 2 , li_roundtype);
                            }
                        }
                    }
                    dw_clear.InsertRow(i + 1);
                    dw_clear.SetItemString(i + 1, "loancontract_no", ls_contno);
                    dw_clear.SetItemString(i + 1, "coop_id", state.SsCoopId);
                    dw_clear.SetItemString(i + 1, "concoop_id", state.SsCoopControl);
                    dw_clear.SetItemString(i + 1, "loantype_code", ls_conttype);
                    dw_clear.SetItemString(i + 1, "prefix", ls_prefix);
                    dw_clear.SetItemDecimal(i + 1, "loanpayment_type", li_paytype);
                    dw_clear.SetItemDecimal(i + 1, "period_payment", ldc_payment);
                    dw_clear.SetItemDecimal(i + 1, "loanapprove_amt", ldc_appvamt);
                    dw_clear.SetItemDecimal(i + 1, "withdrawable_amt", ldc_withdrawable);
                    dw_clear.SetItemDecimal(i + 1, "principal_balance", ldc_balance);
                    dw_clear.SetItemDecimal(i + 1, "last_periodpay", li_period);
                    dw_clear.SetItemDecimal(i + 1, "minperiod_pay", li_minperiod);
                    dw_clear.SetItemDecimal(i + 1, "minpercent_pay", ldc_minpay);
                    dw_clear.SetItemDateTime(i + 1, "lastcalint_date", ldtm_lastcalint);
                    dw_clear.SetItemDecimal(i + 1, "contract_status", li_contstatus);
                    dw_clear.SetItemString(i + 1, "permissgroup_code", ls_permgrp);
                    // dw_clear.SetItemDecimal(i + 1, "clear_status", li_status);
                    dw_clear.SetItemDateTime(i + 1, "lastprocess_date", ldtm_lastproc);
                    dw_clear.SetItemDecimal(i + 1, "contractint_type", li_continttype);
                    dw_clear.SetItemDecimal(i + 1, "contract_interest", ldc_intrate);
                    dw_clear.SetItemDecimal(i + 1, "rkeep_principal", ldc_rkeepprin);
                    dw_clear.SetItemDecimal(i + 1, "rkeep_interest", ldc_rkeepint);
                    dw_clear.SetItemDecimal(i + 1, "interest_arrear", ldc_intarrear);//ldc_intarrear
                    dw_clear.SetItemDateTime(i + 1, "loanapprove_date", ldtm_approve);
                    dw_clear.SetItemDateTime(i + 1, "startcont_date", ldtm_startcont);
                    dw_clear.SetItemDecimal(i + 1, "int_continttype", li_intcontinttype);
                    dw_clear.SetItemDecimal(i + 1, "int_contintrate", ldc_intcontintrate);
                    dw_clear.SetItemString(i + 1, "int_continttabcode", ls_intcontintcode);
                    dw_clear.SetItemDecimal(i + 1, "int_contintincrease", ldc_intincrease);
                    dw_clear.SetItemDecimal(i + 1, "int_intsteptype", li_intsteptype);
                    dw_clear.SetItemDecimal(i + 1, "period_payamt", li_periodamt);
                    dw_clear.SetItemDecimal(i + 1, "contlaw_status", li_contlaw);
                    dw_clear.SetItemDecimal(i + 1, "payment_status", li_paystatus);
                    dw_clear.SetItemDecimal(i + 1, "principal_transbal", ldc_transbal);
                    dw_clear.SetItemDecimal(i + 1, "insurecoll_flag", li_clearinsure);
                    dw_clear.SetItemDecimal(i + 1, "countpay_flag", li_od_flag);

                    dw_clear.SetItemDecimal(i + 1, "intestimate_amt", Convert.ToDecimal(ldc_intestim30));
                    dw_clear.SetItemDecimal(i + 1, "intclear_amt", Convert.ToDecimal(ldc_intestim));


                }
            }

        }

        private void JsGetloangrouppermissuesed()
        {
            string permissgroup_code;
            int clear_status = 0;
            decimal principal_balance = 0, ldc_sumloangroup = 0, ldc_rkeepprin = 0;
            string loanpermissgroup = dw_main.GetItemString(1, "instype_code");
            for (int i = 1; i <= dw_clear.RowCount; i++)
            {
                permissgroup_code = dw_clear.GetItemString(i, "permissgroup_code");
                principal_balance = dw_clear.GetItemDecimal(i, "principal_balance");
                try
                {
                    ldc_rkeepprin = dw_clear.GetItemDecimal(i, "rkeep_principal");
                }
                catch
                {
                    ldc_rkeepprin = 0;
                }

                clear_status = Convert.ToInt16(dw_clear.GetItemDecimal(i, "clear_status"));
                if (loanpermissgroup == permissgroup_code && clear_status == 0)
                {
                    ldc_sumloangroup = ldc_sumloangroup + principal_balance - ldc_rkeepprin;
                }
            }
            dw_main.SetItemDecimal(1, "loangrpuse_amt", ldc_sumloangroup);
        }

        private void JsCalMaxLoanpermiss()
        {
            //คำนวณสิทธิกู้จากยอดเงินเดือนคงเหลือขั้นต่ำ
            string loantype_code = dw_main.GetItemString(1, "loantype_code");
            decimal ldc_salary = dw_main.GetItemDecimal(1, "salary_amt");
            decimal ldc_mthcoop = dw_main.GetItemDecimal(1, "paymonth_coop");
            decimal ldc_minpercsal = dw_main.GetItemDecimal(1, "minsalary_perc");
            decimal ldc_minsalaamt = Convert.ToDecimal(Hdminsalary_amt.Value);// dw_main.GetItemDecimal(1, "minsalary_amt");
            decimal ldc_maxloan = dw_main.GetItemDecimal(1, "loancredit_amt");
            decimal ldc_periodsend = dw_main.GetItemDecimal(1, "period_payamt");
            decimal ldc_paymenttype = dw_main.GetItemDecimal(1, "loanpayment_type");
            decimal ldc_intrate = dw_main.GetItemDecimal(1, "int_contintrate");
            decimal ldc_sharevalue = dw_main.GetItemDecimal(1, "sharestk_value");
            decimal ldc_incomeetc = dw_main.GetItemDecimal(1, "incomemonth_fixed");
            decimal ldc_paymtmetc = dw_main.GetItemDecimal(1, "paymonth_other");
            decimal salarybal_status = Convert.ToDecimal(Hdsalarybal_status.Value);
            decimal loan_credit = dw_main.GetItemDecimal(1, "loancredit_amt");
            //คำนวณเงินเดือนคงเหลือขั้นต่ำ สุรินทร์ กำหนดเงินเดือนคงเหลือไม่เกิน 2000
            if (ldc_minsalaamt > (ldc_salary * ldc_minpercsal) && salarybal_status == 2)
            {
                ldc_minsalaamt = Math.Round(ldc_salary * ldc_minpercsal);
            }
            else if (ldc_minsalaamt < (ldc_salary * ldc_minpercsal) && salarybal_status == 1)
            {
                ldc_minsalaamt = Math.Round(ldc_salary * ldc_minpercsal);
            }
            else if (salarybal_status == 0) {
                ldc_minsalaamt = 0;
            }

            dw_main.SetItemDecimal(1, "minsalary_amt", ldc_minsalaamt);
            
            // ธกส เปลี่ยนการใส่อัตราดอกเบี้ย จาก 0.05  เป็น 5 
            ldc_intrate = ldc_intrate / 100;
            decimal salary_balance = ldc_salary - ldc_minsalaamt - ldc_mthcoop + ldc_incomeetc - ldc_paymtmetc; //
            if (ldc_minsalaamt <= 0) { salary_balance = ldc_salary; }
            
            decimal ldc_permamt;
            double li_maxperiod = Convert.ToDouble(ldc_periodsend);
            String ll_roundpay = Hdpayround_factor.Value;
            int roundpay = Convert.ToInt16(ll_roundpay);
            if (roundpay > 0)
            {
                salary_balance = salary_balance - roundpay + (ldc_intrate * ldc_periodsend * 10 / 12);// ((salary_balance % roundpay) ;
                //  salary_balance = salary_balance + (((salary_balance % roundpay)  * ldc_intrate / 12) + 12 ); 
            }
            if (ldc_paymenttype == 1)
            {
                
                //คงต้น
                double ldc_dayyear = Convert.ToDouble(1) / Convert.ToDouble(12);
                double ldc_ddd = 1.00;
                double ldc_temp = Convert.ToDouble(ldc_periodsend) * (Convert.ToDouble(ldc_intrate) * ldc_dayyear) + ldc_ddd;
                ldc_permamt = Convert.ToDecimal((Convert.ToDouble(salary_balance) * Convert.ToDouble(ldc_periodsend)) / ldc_temp);

            }
            else
            { //คงยอด
                int li_fixcaltype = 1;//fixpaycal_type


                double ldc_permamttmp = 1.00, ldc_fr = 1.00, ldc_temp = 1.00;


                if (li_fixcaltype == 1)
                {
                    // ด/บ ทั้งปี / 12
                    // ldc_permamt = salary_balance * ((((1 + (ldc_intrate / 12)) ^ li_maxperiod) - 1) / ((ldc_intrate / 12) * ((1 + (ldc_intrate / 12)) ^ li_maxperiod)));
                    ldc_temp = Math.Log(1 + (Convert.ToDouble(Convert.ToDouble(ldc_intrate) / 12.00)));
                    ldc_fr = Math.Exp(-li_maxperiod * ldc_temp);
                    ldc_permamttmp = (Convert.ToDouble(salary_balance) * (1.00 - ldc_fr)) / ((Convert.ToDouble(ldc_intrate) / 12));
                }
                else
                {
                    // ด/บ 30 วัน/เดือน
                    ldc_temp = Math.Log(1 + (Convert.ToDouble(Convert.ToDouble(ldc_intrate) / (30.00 / 365.00))));
                    ldc_fr = Math.Exp(-li_maxperiod * ldc_temp);
                    ldc_permamttmp = (Convert.ToDouble(salary_balance) * (1.00 - ldc_fr)) / ((Convert.ToDouble(ldc_intrate) / (30.00 / 365.00)));

                }
                ldc_permamt = Convert.ToDecimal(ldc_permamttmp);
                
                if (ldc_permamt > loan_credit)
                {
                    ldc_permamt = loan_credit;
                }


            }
            //lngrpcutright_flag  หักยอดกู้กลุ่ม
           // String lngrpcutrightflag = wcf.NBusscom.of_getattribloantype(state.SsWsPass, loantype_code, "lngrpcutright_flag");
            String lngrpcutrightflag = Hdlngrpcutright_flag.Value;
            int lngrpcutright_flag = Convert.ToInt16(lngrpcutrightflag);

            //ตรวจสอบยอดขอกู้กลุ๋ม
            decimal loan_groupuse = 0;
            if (lngrpcutright_flag == 1)
            {
                JsGetloangrouppermissuesed();
                loan_groupuse = dw_main.GetItemDecimal(1, "loangrpuse_amt");
            }
            // ldc_permamt = ldc_permamt;//- loan_groupuse;
            if (ldc_permamt > ldc_maxloan)
            {
                ldc_permamt = ldc_maxloan;
            }
            if (ldc_permamt < 0) { ldc_permamt = 0; }
            // ปัดยอดขอกู้
           // String ll_roundloan = wcf.NBusscom.of_getattribloantype(state.SsWsPass, loantype_code, "reqround_factor");
            String ll_roundloan = Hdreqround_factor.Value;
            int roundloan = Convert.ToInt16(ll_roundloan);
            if (roundloan > 0)
            {
                if (ldc_permamt > 0)
                {
                    // by mong loanrequest_amt = Math.Round((loanrequest_amt) / roundloan) * roundloan;
                    if (ldc_permamt % roundloan > 0)
                    {
                        ldc_permamt = ldc_permamt - (ldc_permamt % roundloan);
                    }
                }
            }

            int notmoreshare_flag = Convert.ToInt16(Hdnotmoreshare_flag.Value);
            if (notmoreshare_flag == 1 && ldc_permamt > ldc_sharevalue) { ldc_permamt = ldc_sharevalue; } 
            //  ldc_permamt = Math.Round(ldc_permamt / 1000) * 1000;
            dw_main.SetItemDecimal(1, "loanpermiss_amt", ldc_permamt);
            dw_main.SetItemDecimal(1, "loanreqregis_amt", loan_credit);
            dw_main.SetItemDecimal(1, "loanrequest_amt", loan_credit);
            decimal intest_amt = of_calintestimatemain();

            //mong 56-01-31
            //JsGenrowColllnreq(ldc_permamt);
            string memno = dw_main.GetItemString(1, "member_no");
            string sql_loanreg = "select  loanrequest_amt from lnreqloanregister where loantype_code   = '" + loantype_code + "'    and member_no  =  '" + memno + "'  and reqregister_status = 8 ";
            Sdt dtreg = WebUtil.QuerySdt(sql_loanreg);
            if (dtreg.Next())
            {
                decimal loanreg_amt = dtreg.GetDecimal("loanrequest_amt");
                dw_main.SetItemDecimal(1, "loanreqregis_amt", loanreg_amt);
                
            }
        }


        /// <summary>
        /// init ข้อมูลคนค้ำ
        /// </summary>
        private void JsGetMemberCollno()
        {
            LtServerMessagecoll.Text = "";
            try
            {
                int row = Convert.ToInt16(HdRefcollrow.Value);
                String ls_memcoopid;
                String ref_collno = WebUtil.MemberNoFormat(HdRefcoll.Value);
                Decimal period_payamt = dw_main.GetItemDecimal(1, "period_payamt");
                String description = "";
                String membtype_code = "";
                Decimal ldc_salary = 0, retry_age = 0;
                DateTime ldtm_member = DateTime.Now;
                DateTime ldtm_birth = DateTime.Now;
                DateTime ldtm_retry = DateTime.Now;
                Decimal lndropgrantee_flag = 0;
                String remark = "";
                String loancolltype_code = dw_coll.GetItemString(row, "loancolltype_code");
                decimal loanmembtype = dw_main.GetItemDecimal(1, "member_type");
                //String member_no = WebUtil.MemberNoFormat(HdMemberNo.Value);
                ////string reqmem_no=dw_main.GetItemString(1, "member_no");
                //if (ref_collno == member_no)
                //{
                //    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถ ค้ำประกัน ผู้กู้ได้");
                //}

                DateTime ldtm_reqloan = dw_main.GetItemDateTime(1, "loanrequest_date");

                //Edit By BankCM check ทะเทียนคนค้ำซ้ำ
                Int32 collcount = dw_coll.RowCount;
                //for (int i = 2; i <= collcount;i++ )
                //{
                //    string collmemberno = "";
                //    try
                //    {
                //        collmemberno = dw_coll.GetItemString(i, "ref_collno");
                //        collmemberno = WebUtil.MemberNoFormat(collmemberno);
                //    }
                //    catch
                //    {
                //        collmemberno = "";
                //    }
                //    if ((collmemberno != "") && (collmemberno == ref_collno))
                //    {
                //        MessageBox("ข้อผิดพลาดการค้ำประกัน", "คนค้ำประกันทะเบียน "+ref_collno+" ได้ค้ำประกันในใบคำขอนี้เเล้ว กรูณาตรวจสอบ");
                //        ref_collno = "";
                //        dw_coll.SetItemString(i, "ref_collno", "");
                //    }
                //    else if (ref_collno == (WebUtil.MemberNoFormat(dw_main.GetItemString(1, "member_no"))))
                //    {
                //        MessageBox("ข้อผิดพลาดการค้ำประกัน", "ไม่สามารถทำรายการได้ ค้นค้ำประกันและผู้ขอกู้เป็นเลขเดียวกัน");
                //        ref_collno = "";
                //        dw_coll.SetItemString(i, "ref_collno", "");
                //    }
                //}
              

               
                //JsSetDeptnodefault(1); wa
                if (loancolltype_code == "01")
                {
                    dw_coll.SetItemString(row, "ref_collno", ref_collno);


                    if (HdMemcoopId.Value == "")
                    {
                        ls_memcoopid = dw_main.GetItemString(1, "memcoop_id");
                    }
                    else
                    {
                        ls_memcoopid = HdMemcoopId.Value;
                    }
                    dw_coll.SetItemString(row, "coop_id", state.SsCoopControl);

                    //mong ตรวจสอบการค้ำประกัน จำนวนที่ค้ำประกันได้

                    if (JsCheckMangrtColl(row, ref_collno) == 1)
                    {

                        String sqlstr = @"  SELECT       MBMEMBMASTER.BIRTH_DATE, MBMEMBMASTER.MEMBER_TYPE,
                                             MBMEMBMASTER.dropgurantee_flag,
                                             MBMEMBMASTER.REMARK,
                                             MBMEMBMASTER.MEMBER_DATE,   
                                             MBMEMBMASTER.WORK_DATE,   
                                             MBMEMBMASTER.RETRY_DATE,   
                                             MBMEMBMASTER.SALARY_AMOUNT,   
                                             MBMEMBMASTER.MEMBTYPE_CODE,   
                                             MBUCFMEMBTYPE.MEMBTYPE_DESC,                                             
                                             mbucfprename.prename_desc||mbmembmaster.memb_name||'   '||mbmembmaster.memb_surname as member_name
                               FROM MBMEMBMASTER,   
                                     MBUCFMEMBGROUP,   
                                     MBUCFPRENAME,
                                     MBUCFMEMBTYPE
                                  
                               WHERE ( MBUCFMEMBGROUP.MEMBGROUP_CODE = MBMEMBMASTER.MEMBGROUP_CODE ) and                                     
                                     ( MBMEMBMASTER.PRENAME_CODE = MBUCFPRENAME.PRENAME_CODE ) and
                                     ( MBMEMBMASTER.MEMBTYPE_CODE = MBUCFMEMBTYPE.MEMBTYPE_CODE ) and 
                                     ( MBMEMBMASTER.COOP_ID = MBUCFMEMBGROUP.COOP_ID ) and  
                                      ( mbmembmaster.member_no = '" + ref_collno + @"' ) AND                                 
                                     MBMEMBMASTER.COOP_ID   ='" + state.SsCoopControl + @"'    ";
                        Sdt dt = WebUtil.QuerySdt(sqlstr);

                        decimal collmembertype = 1;
                        while (dt.Next())
                        {
                            lndropgrantee_flag = dt.GetDecimal("dropgurantee_flag");
                            collmembertype = dt.GetDecimal("member_type");
                            description = dt.GetString("member_name");
                            try
                            {
                                remark = dt.GetString("remark");
                            }
                            catch { remark = ""; }
                            membtype_code = dt.GetString("membtype_code");
                            ldc_salary = dt.GetDecimal("salary_amount");
                            try
                            {
                                ldtm_birth = dt.GetDate("BIRTH_DATE");
                            }
                            catch { }
                            //try
                            //{
                            //    ldtm_retry = dt.GetDate("RETRY_DATE");
                            //}
                            //catch
                            //{
                                ///<หาวันที่เกษียณ>
                               // ldtm_retry = wcf.NShrlon.of_calretrydate(state.SsWsPass,ldtm_birth);
                                string retry_date = of_getrerydate(ldtm_birth);
                                ldtm_retry = DateTime.ParseExact(retry_date, "dd-MM-yyyy", null);
                            //}
                            //try
                            //{
                            //    ldtm_retry = Convert.ToDateTime(of_getrerydate(ldtm_birth));
                            //}
                            //catch
                            //{ ldtm_retry = ldtm_birth.AddYears(60); }
                        }
                        if (lndropgrantee_flag == 1)
                        {
                            //Edit By BankCM เปลี่ยน LT message เป็น Message Alert Box
                            MessageBox("ข้อผิดพลาดการค้ำประกัน", ref_collno + "สมาชิกไม่สามารถคำประกันได้เนื่องจากเป็นสมาชิกงดค้ำประกันทุกประเภท");
                            LtServerMessagecoll.Text = WebUtil.ErrorMessage("สมาชิกเลขที่ #"+ref_collno + "เป็นสมาชิกงดค้ำประกันทุกประเภท");
                            dw_coll.SetItemString(row, "ref_collno", " ");
                            return;
                        }
                        if (loanmembtype == 1 && collmembertype == 2) {
                            //Edit By BankCM เปลี่ยน LT message เป็น Message Alert Box
                            MessageBox("ข้อผิดพลาดการค้ำประกัน","ใบคำขอกู้นี้สมาชิกสามัญกู้ ไม่สามารถนำสมาชิกสมทบ มาค้ำประกันได้");
                            //LtServerMessagecoll.Text = WebUtil.ErrorMessage("ใบคำขอกู้นี้สมาชิกสามัญกู้ ไม่สามารถนำสมาชิกสมทบ มาค้ำประกันได้");
                        }
                        DateTime loanrequest_date = dw_main.GetItemDateTime(1, "loanrequest_date");
                        string loantype_code = dw_main.GetItemString(1, "loantype_code");
                        string mangrtpermgrp_code = Hdmangrtpermgrp_code.Value;
                        String[] mem_coll = GetMembercollwa(state.SsWsPass, ls_memcoopid, ref_collno, loanrequest_date, mangrtpermgrp_code, loantype_code); //, 

                       // decimal retry_age = Math.Round(wcf.NBusscom.of_cal_yearmonth(state.SsWsPass, state.SsWorkDate, ldtm_retry) * 12);
                        // mong
                        retry_age = (ldtm_retry.Year - ldtm_reqloan.Year) * 12;
                        retry_age += (ldtm_retry.Month - ldtm_reqloan.Month);
                        //if (ldtm_reqloan.Day > ldtm_retry.Day) { retry_age--; }
                        //decimal age_year = Math.Truncate(retry_age / 12);
                        //decimal age_month = (retry_age % 12) / 100;

                        //retry_age = age_year + age_month;
                        //if (mem_coll[3].Length > 9)
                        //{
                        //    LtServerMessagecoll.Text = WebUtil.WarningMessage(mem_coll[3]);
                        
                        //}
                        //if (mem_coll[6].Length > 9)
                        //{
                        //    LtServerMessagecoll.Text += WebUtil.WarningMessage(mem_coll[6]);

                        //}

                        if (retry_age < 0) {
                            //Edit By BankCM เปลี่ยน LT message เป็น Message Alert Box
                            MessageBox("ข้อผิดพลาดการค้ำประกัน","ผู้ค้ำ  " + ref_collno + "  เป็นสมาชิกที่เกษียณแล้ว กรณาตรวจสอบด้วย !!!! ");
                            //LtServerMessagecoll.Text += WebUtil.ErrorMessage("ผู้ค้ำ  " + ref_collno + "  เป็นสมาชิกที่เกษียณแล้ว กรณาตรวจสอบด้วย !!!! ");
                        }
                        if (retry_age < period_payamt)
                        {

                            LtServerMessagecoll.Text += WebUtil.WarningMessage("งวดเกษียณของผู้ค้ำ ท. " + ref_collno + "  น้อยกว่างวดการส่งผ้งวดการส่งชำระ  " + retry_age.ToString() + "  <  " + period_payamt.ToString() + " !!!");
                            dw_coll.SetItemString(row, "ref_collno", ref_collno);
                            dw_coll.SetItemString(row, "description", description);
                            dw_coll.SetItemDecimal(row, "coll_balance", 0); //Convert.ToDecimal(mem_coll[2])
                        }
                        else
                        {
                            try
                            {
                                if (mem_coll[0] != "")
                                {
                                    dw_coll.SetItemString(row, "ref_collno", ref_collno);
                                    dw_coll.SetItemString(row, "description", description);
                                    dw_coll.SetItemDecimal(row, "coll_balance", 0); //Convert.ToDecimal(mem_coll[2])
                                    HUseamt.Value = mem_coll[2];
                                    //  JsPostreturn();
                                }
                                else
                                {
                                    dw_coll.SetItemString(row, "ref_collno", ref_collno);
                                    dw_coll.SetItemString(row, "description", description);
                                    dw_coll.SetItemDecimal(row, "coll_balance", 0); //Convert.ToDecimal(mem_coll[2])
                                    HUseamt.Value = mem_coll[2];
                                }
                            }
                            catch { 
                            
                            }
                        }
                    }
                }
                else if (loancolltype_code == "02")
                {

                    dw_coll.SetItemString(row, "ref_collno", dw_main.GetItemString(1, "member_no"));
                    dw_coll.SetItemString(row, "description", dw_main.GetItemString(1, "member_name"));
                    dw_coll.SetItemDecimal(row, "coll_amt", dw_main.GetItemDecimal(1, "sharestk_value"));
                    dw_coll.SetItemDecimal(row, "coll_balance", dw_main.GetItemDecimal(1, "sharestk_value"));
                    dw_coll.SetItemDecimal(row, "use_amt", dw_main.GetItemDecimal(1, "sharestk_value"));
                   // HUseamt.Value = dw_main.GetItemDecimal(1, "sharestk_value").ToString();
                    //  JsPostreturn();
                }
                JsCollCondition();
            }
            catch (Exception ex)
            {
                //LtServerMessage.Text = WebUtil.ErrorMessage("JsGetMemberCollno===>" + ex); 
            }
        }
        public String[] GetMembercollwa(string wsPass, String memcoop_id, String ref_collno, DateTime dltm_workdate, string collmbgrp_code, string loantype_code)
        {
            String description = "";
            String membtype_code = "";
            Decimal ldc_salary = 0, member_age = 0, ldc_incomeetc = 0;
            DateTime ldtm_member = DateTime.Now;
            DateTime ldtm_birth = DateTime.Now;
            DateTime ldtm_retry = DateTime.Now;
            Decimal lndropgrantee_flag = 0;
            String remark = "";
            String[] mem_coll = new String[7];
            String sqlstr = @"  SELECT       MBMEMBMASTER.BIRTH_DATE,
                                             MBMEMBMASTER.dropgurantee_flag,
                                             MBMEMBMASTER.REMARK,
                                             MBMEMBMASTER.MEMBER_DATE,   
                                             MBMEMBMASTER.WORK_DATE,   
                                             MBMEMBMASTER.RETRY_DATE,   
                                             MBMEMBMASTER.SALARY_AMOUNT,
                                             MBMEMBMASTER.INCOMEETC_AMT,
                                             SHSHAREMASTER.LAST_PERIOD,  
                                             MBMEMBMASTER.MEMBTYPE_CODE,   
                                             MBUCFMEMBTYPE.MEMBTYPE_DESC,
                                             MBMEMBMASTER.MEMBER_TYPE,                                             
                                             mbucfprename.prename_desc||mbmembmaster.memb_name||'   '||mbmembmaster.memb_surname as member_name
                               FROM MBMEMBMASTER,   
                                     MBUCFMEMBGROUP,  SHSHAREMASTER , 
                                     MBUCFPRENAME,
                                     MBUCFMEMBTYPE
                                  
                               WHERE ( MBUCFMEMBGROUP.MEMBGROUP_CODE = MBMEMBMASTER.MEMBGROUP_CODE ) and                                     
                                     ( MBMEMBMASTER.PRENAME_CODE = MBUCFPRENAME.PRENAME_CODE ) and
                                     ( MBMEMBMASTER.MEMBTYPE_CODE = MBUCFMEMBTYPE.MEMBTYPE_CODE ) and 
                                     (  mbmembmaster.member_no = SHSHAREMASTER.MEMBER_NO ) AND ( MBMEMBMASTER.COOP_ID = SHSHAREMASTER.COOP_ID ) and 
                                     ( MBMEMBMASTER.COOP_ID = MBUCFMEMBGROUP.COOP_ID ) and  
                                      ( mbmembmaster.member_no = '" + ref_collno + @"' ) AND                                 
                                     MBMEMBMASTER.COOP_ID   ='" + memcoop_id + @"'    ";
            Sdt dt = ta.Query(sqlstr);
            while (dt.Next())
            {
                lndropgrantee_flag = dt.GetDecimal("dropgurantee_flag");
                description = dt.GetString("member_name");
                try
                {
                    remark = dt.GetString("remark");
                }
                catch { remark = ""; }
                membtype_code = dt.GetString("membtype_code");
                ldc_salary = dt.GetDecimal("salary_amount");
                ldc_incomeetc = dt.GetDecimal("INCOMEETC_AMT");
                ldc_salary += ldc_incomeetc;
                try
                {
                    ldtm_member = dt.GetDate("MEMBER_DATE");
                }
                catch { }
                decimal lastshare_period = 0;
                try
                {
                    lastshare_period = dt.GetDecimal("last_period");
                }
                catch { lastshare_period = shrlonService.of_cal_yearmonth(wsPass, ldtm_member, dltm_workdate) * 12; }

                member_age = lastshare_period;// bus.of_cal_yearmonth(wsPass, ldtm_member, dltm_workdate) * 12;
            }

            String mangrtypemrp_code = "";

            mangrtypemrp_code = collmbgrp_code;
            Decimal coll_balance = CalloanpermissColl(wsPass, member_age, ldc_salary, mangrtypemrp_code);
            if (coll_balance > 0)
            {
                mem_coll[0] = ref_collno;
                mem_coll[1] = description;
                mem_coll[2] = coll_balance.ToString();
                mem_coll[3] = "";
                mem_coll[4] = member_age.ToString();
                mem_coll[5] = lndropgrantee_flag.ToString();
                mem_coll[6] = remark;

            }
            else
            {
                mem_coll[0] = "";
                mem_coll[1] = "";
                mem_coll[2] = "";
                mem_coll[3] = "สิทธิ์ค้ำไม่ถึงเกณฑ์ที่กำหนด";
                mem_coll[4] = member_age.ToString();
                mem_coll[5] = lndropgrantee_flag.ToString();
                mem_coll[6] = remark;

            }

            //มง เพิ่ม กรณีสมาชิกสมทบ ให้เปลี่ยนรหัสสิทธิค้ำประกัน
            //decimal member_type = dt.GetDecimal("member_type");
            //if (member_type == 2 && (loantype_code == "21" || loantype_code == "22" || loantype_code == "28" || loantype_code == "29" || loantype_code == "30" || loantype_code == "C1" || loantype_code == "C2" || loantype_code == "C3")) { mangrtypemrp_code = "22"; }

            ta.Close();
            return mem_coll;
        }
        /// <summary>
        ///  เปิดใบคำขอเก่าขึ้นมาแก้ไข
        /// </summary>
        /// 
        private void JsCalageyearmonth()
        {
            try
            {

                DateTime ldtm_birth = DateTime.Now;
                DateTime ldtm_retry = DateTime.Now;
                DateTime ldtm_reqloan = DateTime.Now;
                DateTime ldtm_member = DateTime.Now;
                try
                {
                    ldtm_birth = dw_main.GetItemDateTime(1, "birth_date");
                }
                catch { }

                try
                {
                    ldtm_retry = dw_main.GetItemDateTime(1, "retry_date");
                }
                catch  {   }
                try
                {
                    ldtm_reqloan = dw_main.GetItemDateTime(1, "loanrequest_date");
                }
                catch {    }
                try
                {
                    ldtm_member = dw_main.GetItemDateTime(1, "member_date");
                }
                catch { }
                try
                {
                    ///<หาเกษียณอายุ>
                    decimal retry_age = (ldtm_retry.Year - ldtm_reqloan.Year) * 12 ;
                    retry_age += (ldtm_retry.Month - ldtm_reqloan.Month);
                    if (ldtm_reqloan.Day > ldtm_retry.Day) { retry_age--; }
                 //   decimal age_year = Math.Truncate( retry_age / 12);
                   // decimal age_month = (retry_age % 12) / 100 ;

                   // retry_age = age_year + age_month;
                   
                    dw_main.SetItemDecimal(1, "retry_age", retry_age);
                }
                catch { }

                try
                {
                    ///<หาอายุ-ของสมาชิก>
                    ///
                    decimal mbage_age = (ldtm_reqloan.Year - ldtm_birth.Year) * 12;
                    mbage_age += (ldtm_reqloan.Month - ldtm_birth.Month);
                    if (ldtm_birth.Day > ldtm_reqloan.Day) { mbage_age--; }
                    decimal age_year = Math.Truncate(mbage_age / 12);
                    decimal age_month = (mbage_age % 12) / 100;

                    mbage_age = age_year + age_month;


                    dw_main.SetItemDecimal(1, "birth_age", mbage_age);
                }
                catch { }

                try
                {
                    ///<หาอายุการเป็นสมาชิก>
                    ///
                    decimal member_age = (ldtm_reqloan.Year - ldtm_member.Year) * 12;
                    member_age += (ldtm_reqloan.Month - ldtm_member.Month);
                    if (ldtm_member.Day > ldtm_reqloan.Day) { member_age--; }

                    Int16 customtime_type = Convert.ToInt16(Hdcustomtime_type.Value);

                    //กรณี กำหนด ระยะเวลาดจาก งวดส่งห้นล่าสด
                    if (customtime_type == 1)
                    {
                        member_age = dw_main.GetItemDecimal(1, "share_lastperiod");
                    }

                    decimal age_year = Math.Truncate(member_age / 12);
                    decimal age_month = (member_age % 12) / 100;

                   
                    member_age = age_year + age_month;
                    
                    dw_main.SetItemDecimal(1, "member_age", member_age);
                }
                catch { }


            }
            catch { 
            
            }
        
        }
        public void JsRegenClear()
        {
            try
            {
                Genbaseloanclear(); //หนี้เดิม
                of_setloanclearstatus(); //บังคับหักกลบอะไรบ้าง
                JsSetMonthpayCoop(); //ยอดหักสหกร
                JsGenBuyshare();
                JsSumOthClr(); //ยอดหักอืนรวม
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ตั้งสัญญาเก่าหักกลบใหม่ และคำนวณยอดซื้อหุ้นใหม่ ====>" + ex); 
            }
        }
        private void JsOpenOldDocNo()
        {
            try
            {
                string ls_reqloandocno = dw_main.GetItemString(1, "loanrequest_docno");
                string ls_CoopControl = dw_main.GetItemString(1, "coop_id");
                string[] arg = new string[2] { ls_reqloandocno, ls_CoopControl };
                DateTime ldtm_now = DateTime.Now;

                txt_reqNo.Text = ls_reqloandocno;
                txt_member_no.Text = member_no;
                Hdcoopid.Value = ls_CoopControl;
                HdLnreqdoc_no.Value = ls_reqloandocno;
                DwUtil.RetrieveDataWindow(dw_main, pbl, null, arg);
                tDwMain.Eng2ThaiAllRow();
                DwUtil.RetrieveDataWindow(dw_clear, pbl, null, arg);
                DwUtil.RetrieveDataWindow(dw_coll, pbl, null, arg);
                DwUtil.RetrieveDataWindow(dw_otherclr, pbl, null, arg);

                if (dw_main.RowCount > 0) {
                    Int16 li_reqstatus = Convert.ToInt16( dw_main.GetItemDecimal(1, "loanrequest_status"));

                    if (li_reqstatus == 8) {

                       
                      
                        JsSumOthClr(); //ยอดหักอืนรวม
                    
                    }
                
                }

                try
                {

                    loantype = dw_main.GetItemString(1, "loantype_code");
                    // RetreiveDDDW();
                    DwUtil.RetrieveDDDW(dw_main, "loantype_code_1", pbl, null);
                    DwUtil.RetrieveDDDW(dw_main, "expense_code", pbl, null);
                    DwUtil.RetrieveDDDW(dw_main, "loanobjective_code_1", pbl, loantype);
                    DwUtil.RetrieveDDDW(dw_main, "membtype_code", pbl, null);
                    DwUtil.RetrieveDDDW(dw_coll, "loancolltype_code", pbl, null);
                    DwUtil.RetrieveDDDW(dw_coll, "coop_id", pbl, null);
                    DwUtil.RetrieveDDDW(dw_otherclr, "clrothertype_code", pbl, null);
                    // DwUtil.RetrieveDDDW(dw_main, "loantype_code_1", pbl, null);
                    if (dw_main.RowCount > 1) dw_main.DeleteRow(dw_main.RowCount);
                    //คำนวณอาย
                    JsCalageyearmonth();
                    JsExpensebankbrRetrieve();

                    Decimal loanrequestStatus = dw_main.GetItemDecimal(1, "loanrequest_status");
                    //เปิดให้แก้ไขได้หลังจาก open 
                    if ((loanrequestStatus != 8) && (loanrequestStatus != 81) && (loanrequestStatus != 11))
                    {
                        dw_main.DisplayOnly = true;
                        dw_clear.DisplayOnly = true;
                        dw_coll.DisplayOnly = true;
                        dw_otherclr.DisplayOnly = true;
                    }
                    else
                    {
                        dw_main.DisplayOnly = false;
                        dw_clear.DisplayOnly = false;
                        dw_coll.DisplayOnly = false;
                        dw_otherclr.DisplayOnly = false;
                    }
                   
                }
                catch (Exception ex)
                {
                    //LtServerMessage.Text = WebUtil.ErrorMessage("JsOpenOldDocNo====>" + ex); 
                }
            }
            catch
            {

            }
        }


        /// <summary>
        /// ยกเลิก ใบคำขอกู้
        /// </summary>    
        private void JsCancelRequest()
        {
            String dwXmlMain = dw_main.Describe("DataWindow.Data.XML");
            String cancelID = state.SsUsername;
            String coop_id = state.SsCoopId;
            string cancle_date = state.SsWorkDate.ToShortDateString();
            try
            {
                Decimal loanrequestStatus = dw_main.GetItemDecimal(1, "loanrequest_status");
                string loanreqdocno = dw_main.GetItemString(1,"loanrequest_docno");

                //เปิดให้แก้ไขได้หลังจาก open 
                if ((loanrequestStatus == 8) || ( loanrequestStatus == 81 ))
                {
                    string sql_up = "update lnreqloan set loanrequest_status = -9, cancel_id = '" + cancelID + "' where loanrequest_docno = '" + loanreqdocno + "'";
                    WebUtil.ExeSQL(sql_up);
                    WebUtil.ExeSQL("commit");
                    LtServerMessage.Text = "ยกเลิกใบคำขอก้เรียบร้อยแล้ว";
                    dw_main.DisplayOnly = true;
                    dw_clear.DisplayOnly = true;
                    dw_coll.DisplayOnly = true;
                    dw_otherclr.DisplayOnly = true;
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("JsCancelRequest===>" + ex);
            }

        }

        /// <summary>
        /// set วันที่เริ่มเรียกเก็บ
        /// </summary>



        /// <summary>
        ///ยอดขอกู้==> หายอดชำระ
        /// </summary>
        private void JsSetpriod()
        {
            string ls_loantype = dw_main.GetItemString(1, "loantype_code");
            Decimal loanrequest_amt = dw_main.GetItemDecimal(1, "loanrequest_amt");
            Decimal period_payamt = dw_main.GetItemDecimal(1, "maxsend_payamt"); // wa period_payamt
          
            decimal loanpayment_type = dw_main.GetItemDecimal(1, "loanpayment_type");
            // ปัดยอดชำระ
            //String ll_roundpay = wcf.NBusscom.of_getattribloantype(state.SsWsPass, ls_loantype, "payround_factor");
            String ll_roundpay = Hdpayround_factor.Value;
            if (ls_loantype == "23")
            {
                dw_main.SetItemDecimal(1, "period_payment", 0);
            }
            else
            {
                JsLoanpaymenttype();
            }
            Decimal intestimate_amt = of_calintestimatemain();
            dw_main.SetItemDecimal(1, "intestimate_amt", intestimate_amt);
            Ltjspopupclr.Text = "";
            JsGenBuyshare();
            JsCalinsurancepay();
            //JsSetmutualcoll();
            //JsSetmutualStability();
            Jsfirstperiod();
            //JsSetCalFSV();
            //JsCollCondition();
            JsSumOthClr();
         //   JsInsertRowcoll();
            HdIsPostBack.Value = "false";
        }

        /// <summary>
        /// ยอดชำระ==> หาจน.งวด 
        /// </summary>
        private void JsRevert()
        {
            string ls_loantype = dw_main.GetItemString(1, "loantype_code");
            Decimal period_payamt = new Decimal(0.00);
            period_payamt = dw_main.GetItemDecimal(1, "period_payamt");
            Decimal loanrequest_amt = dw_main.GetItemDecimal(1, "loanrequest_amt");
            Decimal period_payment = dw_main.GetItemDecimal(1, "period_payment");
            Decimal maxsend_payamt = dw_main.GetItemDecimal(1, "maxsend_payamt");
            Decimal custompayment_flag = dw_main.GetItemDecimal(1, "custompayment_flag");
            Decimal loanpayment_type = dw_main.GetItemDecimal(1, "loanpayment_type");
            // ปัดยอดชำระ
            //String ll_roundpay = wcf.NBusscom.of_getattribloantype(state.SsWsPass, ls_loantype, "payround_factor");
            String ll_roundpay = Hdpayround_factor.Value;
             int roundpay = 100;
             try
             {
                 roundpay = Convert.ToInt16(ll_roundpay);
             }
             catch
             {
                 roundpay = 100;
             }
           

            if (loanrequest_amt == 0 && period_payamt > 0)
            {
                loanrequest_amt = period_payamt * period_payment;
                // ปัดยอดขอกู้
                //String ll_roundloan = wcf.NBusscom.of_getattribloantype(state.SsWsPass, ls_loantype, "reqround_factor");
                String ll_roundloan = Hdreqround_factor.Value;
                int roundloan = Convert.ToInt16(ll_roundloan);
                if (roundloan > 0)
                {
                    if (loanrequest_amt > 0)
                    {
                        loanrequest_amt = Math.Round((loanrequest_amt) / roundloan) * roundloan;
                    }
                    else
                    {
                        loanrequest_amt = 0;

                    }
                }
                dw_main.SetItemDecimal(1, "loanrequest_amt", loanrequest_amt);

            }
            else
            {
                period_payamt = loanrequest_amt / period_payment;
                period_payamt = Math.Ceiling(Convert.ToDecimal(period_payamt));
                if ((period_payamt * 100) % 100 > 0)
                {
                    period_payamt = Math.Truncate(period_payamt) + 1;
                    //เอ    period_payamt++;
                }

                if (period_payamt > maxsend_payamt)
                {
                    period_payamt = maxsend_payamt;
                    period_payment = loanrequest_amt / period_payamt;
                    //period_payment = Math.Round(period_payment / roundpay) * roundpay;
                }
                // mong period_payment = Math.Round(period_payment / roundpay) * roundpay;
                //period_payment = Math.Round(period_payment, roundpay, MidpointRounding.AwayFromZero);
                if (period_payment % roundpay > 0)
                {
                    period_payment = period_payment + roundpay - (period_payment % roundpay);
                }
                if( custompayment_flag == 0 || loanpayment_type == 1 )
                {
                    dw_main.SetItemDecimal(1, "period_payment", period_payment);

                    dw_main.SetItemDecimal(1, "period_payamt", period_payamt);
                }
            }
            HdIsPostBack.Value = "false";
        }

        /// <summary>
        /// จน.งวด==> หายอดชำระ
        ///</summary>         
        private void JsContPeriod()
        {

            string ls_loantype = dw_main.GetItemString(1, "loantype_code");
            Decimal maxsend_payamt = dw_main.GetItemDecimal(1, "maxsend_payamt"); //
            Decimal period_payamt = dw_main.GetItemDecimal(1, "period_payamt");
            Decimal loanrequest_amt = dw_main.GetItemDecimal(1, "loanrequest_amt");
            decimal loanpayment_type = dw_main.GetItemDecimal(1, "loanpayment_type");
            decimal custompayment_flag = dw_main.GetItemDecimal(1, "custompayment_flag");
            
            decimal ldc_intrate = dw_main.GetItemDecimal(1, "int_contintrate");
            Decimal period_payment = 0;
            if (period_payamt == 0) { period_payamt = maxsend_payamt; }
            // ปัดยอดชำระ
            //String ll_roundpay = wcf.NBusscom.of_getattribloantype(state.SsWsPass, ls_loantype, "payround_factor");
            String ll_roundpay = Hdpayround_factor.Value;
            int roundpay = Convert.ToInt16(ll_roundpay);
            double ldc_fr = 1, ldc_temp = 1, loapayment_amt = 0;
            if (loanpayment_type == 1)
            {
                //คงต้น
                period_payment = loanrequest_amt / period_payamt;
            }
            else
            {
                int li_fixcaltype = Convert.ToInt16( wcf.NBusscom.of_getattribconstant(state.SsWsPass, "fixpaycal_type")); 
                //คงยอด
                //ธกส เปลี่ยนการใส่อัตราดอกเบี้ย
                ldc_intrate = ldc_intrate / 100;

                if (li_fixcaltype == 1)
                {
                    // ด/บ ทั้งปี / 12
                    //ldc_fr			= exp( - ai_period * log( ( 1 + adc_intrate / 12 ) ) )
                    //ldc_payamt	= adc_principal * ( adc_intrate / 12 ) / ( 1 - ldc_fr )

                    ldc_temp = Math.Log(1 + (Convert.ToDouble(ldc_intrate / 12)));
                    ldc_fr = Math.Exp(-Convert.ToDouble(period_payamt) * ldc_temp);
                    loapayment_amt = (Convert.ToDouble(loanrequest_amt) * (Convert.ToDouble(ldc_intrate) / 12)) / ((1 - ldc_fr));


                }
                else
                {
                    // ด/บ 30 วัน/เดือน
                    //ldc_fr			= exp( - ai_period * log( ( 1 + adc_intrate * ( 30/365 ) ) ) )
                    //ldc_payamt	= adc_principal * ( adc_intrate * 30/365 ) / ( 1 - ldc_fr )

                    ldc_temp = Math.Log(1 + (Convert.ToDouble(ldc_intrate * (30 / 365))));
                    ldc_fr = Math.Exp(-Convert.ToDouble(period_payamt) * ldc_temp);
                    loapayment_amt = (Convert.ToDouble(loanrequest_amt) * (Convert.ToDouble(ldc_intrate) * (30 / 365))) / ((1 - ldc_fr));
                    
                    
                    //แบบสุโขทัย
                    //ldc_fr			= ( adc_intrate * ( 1+ adc_intrate) ^ ai_period )/ (((1 + adc_intrate ) ^ ai_period ) - 1  )
                    //ldc_payamt = adc_principal / ldc_fr
                }

                period_payment = Convert.ToDecimal(loapayment_amt);// Math.Ceiling(Convert.ToDecimal(loapayment_amt));
            }
            if (roundpay > 0)
            {
                //by mong
                //period_payamt = Math.Round(period_payamt / 10) * 10;
                period_payment = Math.Ceiling(Convert.ToDecimal(period_payment));
                //period_payment = Math.Round(period_payment, roundpay, MidpointRounding.AwayFromZero);// *roundpay;
                if (period_payment % roundpay > 0)
                {
                    period_payment = period_payment + roundpay - (period_payment % roundpay);
                }
                

            }

            //สำหรับครูสุรินทร์
            if (loanpayment_type == 1)
            {
                //คงต้น
                period_payamt = loanrequest_amt / period_payment;

                if ((period_payamt * 100) % 100 > 0)
                {
                    period_payamt = Math.Truncate(period_payamt);
                    period_payamt++;
                }
            }

            if (custompayment_flag == 0)
            {
                dw_main.SetItemDecimal(1, "period_payment", period_payment);
                dw_main.SetItemDecimal(1, "period_payamt", period_payamt);
            }
            JsSumOthClr();
            //JsChecksalarybal();
            HdIsPostBack.Value = "false";
        }

        private void JsLoanpaymenttype()
        {

            string ls_loantype = dw_main.GetItemString(1, "loantype_code");
            Decimal period_payamt = dw_main.GetItemDecimal(1, "maxsend_payamt");
            Decimal loanrequest_amt = dw_main.GetItemDecimal(1, "loanrequest_amt");
            decimal loanpayment_type = dw_main.GetItemDecimal(1, "loanpayment_type");
            decimal ldc_intrate = dw_main.GetItemDecimal(1, "int_contintrate");
            Decimal period_payment = 0;

            // ปัดยอดชำระ
            //String ll_roundpay = wcf.NBusscom.of_getattribloantype(state.SsWsPass, ls_loantype, "payround_factor");
            String ll_roundpay = Hdpayround_factor.Value;
            int roundpay = Convert.ToInt16(ll_roundpay);
            // if (roundpay <= 0) { roundpay = 100; }
            double ldc_fr = 1, ldc_temp = 1, loapayment_amt = 0;
            if (loanpayment_type == 1)
            {
                //คงต้น
                period_payment = loanrequest_amt / period_payamt;
                loapayment_amt = (Convert.ToDouble(period_payment));
            }
            else
            {
                int li_fixcaltype = 1;

                try
                {
                    li_fixcaltype = Convert.ToInt16(Hdfixpaycal_type.Value);// Convert.ToInt16(wcf.NBusscom.of_getattribconstant(state.SsWsPass, "fixpaycal_type"));
                }
                catch { li_fixcaltype = 1; }
                //คงยอด
                //ธกส เปลี่ยนการใส่อัตราดอกเบี้ย
                ldc_intrate = ldc_intrate / 100;
                if (li_fixcaltype == 1)
                {

                    // ด/บ ทั้งปี / 12
                    //ldc_fr			= exp( - ai_period * log( ( 1 + adc_intrate / 12 ) ) )
                    //ldc_payamt	= adc_principal * ( adc_intrate / 12 ) / ( 1 - ldc_fr )

                    ldc_temp = Math.Log(1 + (Convert.ToDouble(ldc_intrate / 12)));
                    ldc_fr = Math.Exp(-Convert.ToDouble(period_payamt) * ldc_temp);
                    loapayment_amt = (Convert.ToDouble(loanrequest_amt) * (Convert.ToDouble(ldc_intrate) / 12)) / ((1 - ldc_fr));


                }
                else
                {
                    // ด/บ 30 วัน/เดือน
                //ldc_fr			= exp( - ai_period * log( ( 1 + adc_intrate * ( 30/365 ) ) ) )
                //ldc_payamt	= adc_principal * ( adc_intrate * 30/365 ) / ( 1 - ldc_fr )

                    ldc_temp = Math.Log(1 + (Convert.ToDouble(ldc_intrate * (30/365))));
                    ldc_fr = Math.Exp(-Convert.ToDouble(period_payamt) * ldc_temp);
                    loapayment_amt = (Convert.ToDouble(loanrequest_amt) * (Convert.ToDouble(ldc_intrate) * (30 / 365))) / ((1 - ldc_fr));
                    //แบบสุโขทัย
                    //ldc_fr			= ( adc_intrate * ( 1+ adc_intrate) ^ ai_period )/ (((1 + adc_intrate ) ^ ai_period ) - 1  )
                    //ldc_payamt = adc_principal / ldc_fr
                }

                period_payment = Convert.ToDecimal(loapayment_amt);// Math.Ceiling(Convert.ToDecimal(loapayment_amt));
            }
            if (roundpay > 0)
            {

                //wa
                period_payment = Math.Ceiling(Convert.ToDecimal(loapayment_amt));
                //period_payamt = Math.Round(period_payamt / 10) * 10;
                // period_payment = Math.Round(period_payment / roundpay) * roundpay;
                //period_payment = Math.Round(period_payment, roundpay, MidpointRounding.AwayFromZero);
                if (period_payment % roundpay > 0)
                {
                    period_payment = period_payment + roundpay - (period_payment % roundpay);
                }
            }
            //สำหรับครูสุรินทร์
            if (loanpayment_type == 1)
            {
                //คงต้น
                period_payamt = loanrequest_amt / period_payment;

                if ((period_payamt * 100) % 100 > 0)
                {
                    period_payamt = Math.Truncate(period_payamt);
                    period_payamt++;
                }
            }

            dw_main.SetItemDecimal(1, "period_payment", period_payment);
            dw_main.SetItemDecimal(1, "period_payamt", period_payamt);
            HdIsPostBack.Value = "false";
        }

        private void JsLoanCreditDividend()
        {
            string loantype = dw_main.GetItemString(1, "loantype_code");
            string member_no = dw_main.GetItemString(1, "member_no");
            string coop_id = dw_main.GetItemString(1, "coop_id");
            //คิดจากยอดหุ้น ยกมา
            double ldc_shrstkbegin = 0, ldc_divrate = 0, ldc_shrbuy = 0;
            string strat_date = " ", end_date = " ";
            String sqlstr = @"   SELECT  SHSHAREMASTER.SHARESTK_AMT,   (SHSHAREMASTER.SHAREBEGIN_AMT * SHSHARETYPE.UNITSHARE_VALUE ) as SHAREBEGIN_AMT,  
                                             SHSHARETYPE.UNITSHARE_VALUE, SHSHARETYPE.dividend_rate,SHSHARETYPE.loandivstart_date,SHSHARETYPE.loandivend_date
                               FROM   SHSHAREMASTER,   
                                     SHSHARETYPE  
                               WHERE     ( SHSHAREMASTER.SHARETYPE_CODE = SHSHARETYPE.SHARETYPE_CODE ) and 
                                     ( SHSHAREMASTER.COOP_ID = SHSHARETYPE.COOP_ID ) and  
                                     ( ( SHSHAREMASTER.member_no = '" + member_no + @"' ) AND  
                                     ( shsharemaster.sharetype_code = '01' ) )    and  
                                     SHSHAREMASTER.COOP_ID   ='" + coop_id + @"' ";


            Sdt dtshare = WebUtil.QuerySdt(sqlstr);
            if (dtshare.Next())
            {
                ldc_shrstkbegin = dtshare.GetDouble("SHAREBEGIN_AMT");
                ldc_divrate = dtshare.GetDouble("dividend_rate");

            }
          
            double loancredit_amt = ldc_shrstkbegin * ldc_divrate;
            decimal ldc_maxcredit = Convert.ToDecimal(loancredit_amt);
            //mong
            //String ll_roundloan = wcf.NBusscom.of_getattribloantype(state.SsWsPass, loantype, "reqround_factor");
            String ll_roundloan = Hdreqround_factor.Value;
            int roundloan = Convert.ToInt16(ll_roundloan);
            if (ldc_maxcredit % roundloan > 0)
            {
                ldc_maxcredit = ldc_maxcredit - (ldc_maxcredit % roundloan);
            }

            dw_main.SetItemDecimal(1, "loancredit_amt", ldc_maxcredit);
            dw_main.SetItemDecimal(1, "loanrequest_amt", ldc_maxcredit);
            dw_main.SetItemDecimal(1, "loanpermiss_amt", ldc_maxcredit);
            dw_main.SetItemDecimal(1, "maxsend_payamt", 12);
            dw_main.SetItemDecimal(1, "period_payamt", 12);
            //คงต้น
            // ปัดยอดชำระ
            decimal period_payamt = 0, period_payment = 0;
           
            period_payment = 0;
            dw_main.SetItemDecimal(1, "period_payment", period_payment);
            ldc_divrate = ldc_divrate * 100;
            Ltdividen.Text = WebUtil.CompleteMessage("ยอดหุ้นยกมาต้นปี =  " + ldc_shrstkbegin.ToString("###,###,###,###.00") + " ||  อัตราปันผลปีที่แล้ว = " + ldc_divrate.ToString("##.00") + " % ");
        }
        /// <summary>
        /// ค้ำประกัน
        /// </summary>
        /// 
        private int JsCheckMangrtColl(int row, string ref_collno)
        {
            LtServerMessagecoll.Text = "";
            Hdcollconfirm.Value = "0";
            string mangrtpermgrp_code = "01";
            string loantype = dw_main.GetItemString(1, "loantype_code");
            string membno = dw_main.GetItemString(1, "member_no");
            string memcoop_id = dw_main.GetItemString(1, "coop_id");
            string sql_lngrtlntype = " select cntmangrtnum_flag,cntmangrtval_flag,grtman_type, grtman_amt, grtman_allmax, mangrtpermgrp_code,countcontgrt_code,cntmangrtnum_type from lnloantype where loantype_code = '" + loantype + "'";
            Sdt dtlnt = WebUtil.QuerySdt(sql_lngrtlntype);
            decimal lntgrtcountmemglag = 0, lntgrtvalmemglag = 0, grtman_type = 0, grtman_allmax = 0;
            if (dtlnt.Next())
            {
                lntgrtcountmemglag = dtlnt.GetDecimal("cntmangrtnum_flag");
                lntgrtvalmemglag = dtlnt.GetDecimal("cntmangrtval_flag");
                grtman_type = dtlnt.GetDecimal("grtman_type");
                grtman_allmax = dtlnt.GetDecimal("grtman_amt");
                mangrtpermgrp_code = dtlnt.GetString("mangrtpermgrp_code");
            }
            
            
            string sql_lnconst = "select grtright_contflag, grtright_memflag, grtright_contract, grtright_member from lnloanconstant ";
            Sdt dt = WebUtil.QuerySdt(sql_lnconst);
            decimal grtchkcont_flag = 0, grtchkmem_flag = 0, grtcountcont = 0, grtcountmem = 0;
            if (dt.Next())
            {
                grtchkcont_flag = dt.GetDecimal("grtright_contflag");
                grtchkmem_flag = dt.GetDecimal("grtright_memflag");
                grtcountcont = dt.GetDecimal("grtright_contract");
                grtcountmem = dt.GetDecimal("grtright_member");

            }
            string contno_clr = "", contclr_all = "";
            decimal clear_flag = 0;
            int k = 0;
            for (int i = 1; i <= dw_clear.RowCount; i++)
            {
                contno_clr = dw_clear.GetItemString(i, "loancontract_no");
                clear_flag = dw_clear.GetItemDecimal(i, "clear_status");
                if (clear_flag == 1)
                {
                    k++;
                    if (k > 1) { contclr_all += ","; }
                    contclr_all += "'" + contno_clr + "'";

                }
            }
            dt.Dispose();
            //            string sql_memcoll = @" select  b.member_no , a.ref_collno , a.coll_status, a.loancontract_no ,
            //                                   b.principal_balance from  lncontcoll a, lncontmaster b 
            //                                    where a.loancontract_no = b.loancontract_no and b.principal_balance > 0 
            //                                    and a.coll_status = 1  and a.ref_collno = '" + ref_collno + @"' and 
            //                                   a.loancontract_no not in ( " + contclr_all + @")  order by b.member_no ";

            if ((contclr_all.Length <= 7) || (contclr_all.Trim() == "")) { contclr_all = "'00000'"; }

            string sql_memcoll = @" SELECT LNCONTMASTER.MEMBER_NO as member_no,   
             LNCONTMASTER.LOANCONTRACT_NO as loancontract_no,   
             LNCONTMASTER.PRINCIPAL_BALANCE as principal_balance ,
             LNCONTCOLL.REF_COLLNO as  ref_collno,   
             LNCONTCOLL.DESCRIPTION as DESCRIPTION ,   
             LNCONTCOLL.COLL_AMT as COLL_AMT ,   
             LNCONTCOLL.COLL_PERCENT as COLL_PERCENT ,   
            MBMEMBMASTER.MEMB_NAME,
			MBMEMBMASTER.MEMB_SURNAME,
             LNCONTCOLL.BASE_PERCENT as BASE_PERCENT ,'Contno'
            FROM LNCONTCOLL,   
                 LNCONTMASTER  ,MBMEMBMASTER
           WHERE ( LNCONTCOLL.COOP_ID = LNCONTMASTER.COOP_ID ) and  
                    ( MBMEMBMASTER.MEMBER_NO = LNCONTMASTER.MEMBER_NO) AND   LNCONTCOLL.COLL_STATUS = 1 and
                 ( LNCONTCOLL.LOANCONTRACT_NO = LNCONTMASTER.LOANCONTRACT_NO ) and  
                 ( ( LNCONTCOLL.LOANCOLLTYPE_CODE in ( '01') ) AND  
                 ( LNCONTMASTER.PRINCIPAL_BALANCE > 0 ) AND LNCONTMASTER.LOANCONTRACT_NO not in (" + contclr_all + @") and 
                 ( LNCONTCOLL.REF_COLLNO = '" + ref_collno + @"'  ) )      
        Union
         SELECT LNREQLOAN.MEMBER_NO  as member_no ,   
                 LNREQLOAN.LOANREQUEST_DOCNO as loancontract_no ,   
                 LNREQLOAN.LOANREQUEST_AMT as  principal_balance,
                 LNREQLOANCOLL.REF_COLLNO as  ref_collno ,   
                 LNREQLOANCOLL.DESCRIPTION  as DESCRIPTION ,   
                 LNREQLOANCOLL.COLL_AMT as COLL_AMT ,   
                 LNREQLOANCOLL.COLL_PERCENT as COLL_PERCENT,   
                MBMEMBMASTER.MEMB_NAME,
			        MBMEMBMASTER.MEMB_SURNAME,
                 LNREQLOANCOLL.BASE_PERCENT as BASE_PERCENT  , 'Lnreqloan'
            FROM LNREQLOAN,   
                 LNREQLOANCOLL  ,MBMEMBMASTER
           WHERE ( LNREQLOAN.COOP_ID = LNREQLOANCOLL.COOP_ID ) and  
                    ( MBMEMBMASTER.MEMBER_NO = LNREQLOAN.MEMBER_NO) AND
                 ( LNREQLOAN.LOANREQUEST_DOCNO = LNREQLOANCOLL.LOANREQUEST_DOCNO ) and  
                 ( ( LNREQLOANCOLL.LOANCOLLTYPE_CODE = '01' ) AND  
                 ( LNREQLOAN.LOANREQUEST_STATUS = 8 ) AND  
                 ( LNREQLOANCOLL.REF_COLLNO =  '" + ref_collno + @"'  ) ) order by member_no ";
            Sdt dt_coll = WebUtil.QuerySdt(sql_memcoll);

            if (grtchkcont_flag == 1 && dt_coll.GetRowCount() >= grtcountcont)
            {
                //ตรวจสอบการค้ำประกันเกินสัญญาที่ระบุ
                string msgcoll = "ผู้ค้ำสมาชิกเลขที่ # " + ref_collno + "  ได้ค้ำประกันเกินกว่าที่กำหนดไว้ค้ำประกันได้สูงสุด  " + grtcountcont.ToString() + " สัญญา ค้ำประกันไปแล้ัว " + dt_coll.GetRowCount().ToString();
                MessageBox("ใบคำขอกู้", msgcoll);
                LtServerMessagecoll.Text = WebUtil.ErrorMessage(msgcoll);
                LtServerMessage.Text = WebUtil.ErrorMessage(msgcoll);
                dw_coll.SetItemString(row, "ref_collno", "");
                dw_coll.SetItemString(row, "description", "");
                dw_coll.SetItemDecimal(row, "coll_balance", 0);
                return 1;
            }
            string member_nochk, prememno = "";
            int collmem = 0;
            decimal principal_balance = 0, ldc_collbalance = 0 , ldc_colluse = 0, ldc_collpercent = 0, ldc_collamt = 0;
            string ls_msg = "", memb_name = "", memb_surname = "";
            while (dt_coll.Next())
            {
                member_nochk = dt_coll.GetString("member_no");
                memb_name = dt_coll.GetString("MEMB_NAME");
                memb_surname = dt_coll.GetString("MEMB_SURNAME");
                principal_balance = dt_coll.GetDecimal("principal_balande");
                ldc_collpercent = dt_coll.GetDecimal("COLL_PERCENT");
                ldc_colluse = principal_balance * ldc_collpercent;
                ldc_collamt += ldc_colluse;
                if (prememno != member_nochk)
                {
                    collmem++;
                    ls_msg += "(" + collmem.ToString() +") , ค้ำประกันสมาชิกเลขที่  " + member_nochk + " " + memb_name.Trim() + " " + memb_surname.Trim() + "/n";
                }
                if (membno == member_nochk) { collmem--; }
                prememno = member_nochk;

            }
            if (grtchkmem_flag == 1 && collmem >= grtcountmem && lntgrtcountmemglag  == 1)
            {

                //ตรวจสอบการค้ำประกันเกินจำนวนคนที่ระบุ
                string msgcollm = "ผู้ค้ำสมาชิกเลขที่ # " + ref_collno + "  ได้ค้ำประกันสมาชิกเกินกว่าที่กำหนดไว้ค้ำประกันได้สูงสุด  " + "/n" + grtcountmem.ToString() + " ค้ำประกันได้ไม่ไม่เกิน " + collmem.ToString() + "/n " + ls_msg;
               // MessageBox("ใบคำขอกู้", msgcollm);
                LtServerMessage.Text = WebUtil.ErrorMessage("ผู้ค้ำสมาชิกเลขที่ # " + ref_collno + "  ได้ค้ำประกันสมาชิกเกินกว่าที่กำหนดไว้ค้ำประกันได้สูงสุด  " + "\n" + grtcountmem.ToString() + " ค้ำประกันได้ไม่ไม่เกิน " + collmem.ToString() + "/n " + ls_msg);
                // Ltdividen.Text = WebUtil.ErrorMessage(ls_msg);
                
                //dw_coll.SetItemString(row, "ref_collno", "");
                //dw_coll.SetItemString(row, "description", "");
                dw_coll.SetItemDecimal(row, "coll_balance", 0);
                //return 1; //-1
                Hdcollconfirm.Value = "8";
            }

            //เช็คสิทธิค้ำยามยอดเงิน
            if (lntgrtvalmemglag == 1) 
            { 
                 String sqlstr = @"  SELECT       MBMEMBMASTER.BIRTH_DATE,
                                             MBMEMBMASTER.dropgurantee_flag,
                                             MBMEMBMASTER.REMARK,
                                             MBMEMBMASTER.MEMBER_DATE,   
                                             MBMEMBMASTER.WORK_DATE,   
                                             MBMEMBMASTER.RETRY_DATE,   
                                             MBMEMBMASTER.SALARY_AMOUNT,   
                                             MBMEMBMASTER.MEMBTYPE_CODE,   
                                             MBUCFMEMBTYPE.MEMBTYPE_DESC,                                             
                                             mbucfprename.prename_desc||mbmembmaster.memb_name||'   '||mbmembmaster.memb_surname as member_name
                               FROM MBMEMBMASTER,   
                                     MBUCFMEMBGROUP,   
                                     MBUCFPRENAME,
                                     MBUCFMEMBTYPE
                                  
                               WHERE ( MBUCFMEMBGROUP.MEMBGROUP_CODE = MBMEMBMASTER.MEMBGROUP_CODE ) and                                     
                                     ( MBMEMBMASTER.PRENAME_CODE = MBUCFPRENAME.PRENAME_CODE ) and
                                     ( MBMEMBMASTER.MEMBTYPE_CODE = MBUCFMEMBTYPE.MEMBTYPE_CODE ) and 
                                     ( MBMEMBMASTER.COOP_ID = MBUCFMEMBGROUP.COOP_ID ) and  
                                      ( mbmembmaster.member_no = '" + ref_collno + @"' ) AND                                 
                                     MBMEMBMASTER.COOP_ID   ='" + memcoop_id + @"'    ";
            Sdt dtgrt = WebUtil.QuerySdt(sqlstr);
            DateTime ldtm_member = DateTime.Now;
            while (dtgrt.Next()) 
            {
                string membtype_code = dtgrt.GetString("membtype_code");
               decimal ldc_salary = dtgrt.GetDecimal("salary_amount");
                try
                {
                    ldtm_member = dtgrt.GetDate("MEMBER_DATE");
                }
                catch { }
                DateTime ldtm_reqloan = dw_main.GetItemDateTime(1, "loanrequest_date");
                Decimal member_age = 0 ;// = BusscomService.of_cal_yearmonth(state.SsWsPass, ldtm_member, DateTime.Now);

                member_age = (ldtm_reqloan.Year - ldtm_member.Year) * 12;
                member_age += (ldtm_reqloan.Month - ldtm_member.Month);
                if (ldtm_member.Day > ldtm_reqloan.Day) { member_age--; }
                //decimal age_year = Math.Truncate(member_age / 12);
                //decimal age_month = (member_age % 12) / 100;

                //member_age = age_year + age_month;

                //คำนวณสิทธิค้ำประกัน
                Decimal coll_amt = CalloanpermissColl(state.SsWsPass,member_age, ldc_salary, mangrtpermgrp_code);
                ldc_collbalance = coll_amt - ldc_collamt;
                if (ldc_collbalance < 0) { ldc_collbalance = 0; }
                dw_coll.SetItemDecimal(row, "coll_amt", coll_amt);
                dw_coll.SetItemDecimal(row, "coll_balance", ldc_collbalance);
                dw_coll.SetItemDecimal(row, "coll_useamt", ldc_collamt);
                return 1; //-1

            }
            
         
            
            }
            dt_coll.Dispose();
            return 1;
        }
        public Decimal CalloanpermissColl(String wsPass, Decimal memtime, Decimal ldc_salary, String mangrtypemrp_code)
        {

            Decimal ldc_maxcredit = 1500000, ldc_maxcreditcoll = 0;
            String sqlStrcredit = @"  SELECT MANGRTPERMGRP_CODE,   
                                             SEQ_NO,   
                                             STARTSHARE_AMT,   
                                             ENDSHARE_AMT,   
                                             STARTMEMBER_TIME,   
                                             ENDMEMBER_TIME,   
                                             PERCENTSHARE,   
                                             PERCENTSALARY,   
                                             MAXGRT_AMT,   
                                             START_SALARY,   
                                             END_SALARY  
                                        FROM LNGRPMANGRTPERMDET   
                                        WHERE  MANGRTPERMGRP_CODE ='" + mangrtypemrp_code + @"'  
                                             and STARTMEMBER_TIME <=" + memtime + " and ENDMEMBER_TIME >" + memtime + @" 
                                    ORDER BY  MANGRTPERMGRP_CODE,   
                                             SEQ_NO ";
            Sdt dt = ta.Query(sqlStrcredit);
            while (dt.Next())
            {
                ldc_maxcredit = ldc_salary * dt.GetDecimal("PERCENTSALARY");
                ldc_maxcreditcoll = dt.GetDecimal("MAXGRT_AMT");
                if (ldc_maxcredit > ldc_maxcreditcoll) { ldc_maxcredit = ldc_maxcreditcoll; }
            }


            return ldc_maxcredit;

        }
        private void JsCollCondition()
        {
            string ls_loantype = dw_main.GetItemString(1, "loantype_code");

            Decimal coll_balance = 0;
            string loancolltype_code = "";
            Decimal  coll_use = 0;
            Decimal per90 = new Decimal(0.9);
            Decimal sharestk_value = dw_main.GetItemDecimal(1, "sharestk_value");
            Decimal loanrequest, loanrequestbal = 0, colluseest_amt = 0;
            loanrequest = dw_main.GetItemDecimal(1, "loanrequest_amt");
            // loanrequest_amt = loanrequest - sharestk_value;
            int row = dw_coll.RowCount;
            decimal coll_newbalance = 0;
            int i = 0;
            loanrequestbal = loanrequest;
            colluseest_amt = loanrequest / row;
            for (i = 0; i < row; i++)
            {

                loancolltype_code = dw_coll.GetItemString(i + 1, "loancolltype_code");
                try
                {
                    coll_balance = dw_coll.GetItemDecimal(i + 1, "coll_balance");
                }
                catch { coll_balance = 0; }
                if (colluseest_amt >= coll_balance )
                {
                    coll_use = coll_balance;
                    coll_newbalance += colluseest_amt - coll_balance;
                }
                else {
                    coll_use = colluseest_amt;
                   
                }

               
                dw_coll.SetItemDecimal(i + 1, "use_amt", coll_use);
                if (loancolltype_code == "01")
                {
                    dw_coll.SetItemDecimal(i + 1, "base_percent", 1);
                }
                else
                {
                    dw_coll.SetItemDouble(i + 1, "base_percent", 0.9);
                }
                dw_coll.SetItemDecimal(i + 1, "coll_percent", (coll_use / loanrequest));

                loanrequestbal = loanrequestbal - coll_use;
            }
            if (coll_newbalance > 0) {
                for (i = 0; i < row; i++)
                {

                    if( coll_newbalance <= 0 ) {continue;}
                    loancolltype_code = dw_coll.GetItemString(i + 1, "loancolltype_code");
                    try
                    {
                        coll_balance = dw_coll.GetItemDecimal(i + 1, "coll_balance");
                    }
                    catch { coll_balance = 0; }
                    try
                    {
                        coll_use = dw_coll.GetItemDecimal(i + 1, "use_amt");
                    }
                    catch { coll_use = 0; }

                    if (coll_balance - coll_use > 0) {
                        if (coll_newbalance > coll_balance - coll_use)
                        {
                            coll_use += coll_balance - coll_use;
                            coll_newbalance -= coll_balance - coll_use;
                        }
                        else {
                            coll_use += coll_newbalance;
                            if (coll_use > 500000) {
                                coll_use = 500000;
                                coll_newbalance -= coll_use;
                            }
                            coll_newbalance = 0;

                        }
                        dw_coll.SetItemDecimal(i + 1, "use_amt", coll_use);
                        dw_coll.SetItemDecimal(i + 1, "coll_percent", (coll_use / loanrequest));
                    
                    }
                }
            }

        }

        private void JsExpensebankbrRetrieve() 
        {
            try
            {

                String bankCode;
                try { bankCode = dw_main.GetItemString(1, "expense_bank").Trim(); }
                catch { bankCode = "006"; }
                DwUtil.RetrieveDDDW(dw_main, "expense_branch_1", "sl_loan_requestment_cen.pbl", bankCode);
                dw_main.SetItemDouble(1, "retrive_bk_branchflag", 1);
            }
            catch { }
        

        }
       private void JsCollInitP()
        {

            string ls_loantype = dw_main.GetItemString(1, "loantype_code");
          
                Decimal coll_balance = 0;
                string loancolltype_code = "";
                Decimal per90 = new Decimal(0.9);
                Decimal sharestk_value = dw_main.GetItemDecimal(1, "sharestk_value");
                Decimal loanrequest;
                double collpercent_use = 0;
                loanrequest = dw_main.GetItemDecimal(1, "loanrequest_amt");
              // loanrequest_amt = loanrequest - sharestk_value;
                int row = dw_coll.RowCount;
                int i = 0;
               
                for (i = 0; i < row; i++)
                {

                    loancolltype_code = dw_coll.GetItemString(i + 1, "loancolltype_code");

                     coll_balance = dw_coll.GetItemDecimal(i + 1, "use_amt");
                    // balance = loanrequest_amt * (collbalance / total);
                   //  dw_coll.SetItemDecimal(i + 1, "use_amt", balance);
                     if (loancolltype_code == "01")
                     {
                         dw_coll.SetItemDecimal(i + 1, "base_percent", 1);
                     }
                     else
                     {
                         dw_coll.SetItemDouble(i + 1, "base_percent", 0.9);
                     }
                     collpercent_use = Convert.ToDouble(coll_balance / loanrequest);
                     dw_coll.SetItemDouble(i + 1, "coll_percent", collpercent_use);


                }

         

        }

        private void JsSetDataList()
        {
            str_itemchange strList = new str_itemchange();
            strList = WebUtil.nstr_itemchange_session(this);
            if (dw_clear.RowCount == 0)
            { strList.xml_clear = null; }
            else { strList.xml_clear = dw_clear.Describe("DataWindow.Data.XML"); }
            if (dw_coll.RowCount == 0) { strList.xml_guarantee = null; }
            else { strList.xml_guarantee = dw_coll.Describe("DataWindow.Data.XML"); }

            Session["strItemchange"] = strList;
        }

        private void JsPostColl()
        {
            try
            {

                String columnName = "ref_collno";
                if (HdColumnName.Value == "" || HdColumnName.Value == "setcolldetail") { columnName = "setcolldetail"; }
                String dwMainXML = dw_main.Describe("DataWindow.Data.XML");
                String dwCollXML = dw_coll.Describe("DataWindow.Data.XML");
                String dwClearXML = dw_clear.Describe("DataWindow.Data.XML");
                String t = dw_main.GetItemString(1, "loantype_code");
                str_itemchange strList = new str_itemchange();

                // ตรวจสอบว่า HdRowNumber มีค่าหรือไม่ ถ้าไม่มี ไม่ต้องทำอะไร
                if (HdRowNumber.Value.ToString() != "")
                {
                    int rowNumber = Convert.ToInt32(HdRowNumber.Value.ToString());

                    HdColumnName.Value = "";
                    if ((columnName == "checkmancollperiod") || (columnName == "setcolldetail"))
                    {
                        dw_coll.SetItemString(rowNumber, "ref_collno", HdRefcollNO.Value);
                        dwCollXML = dw_coll.Describe("DataWindow.Data.XML");
                    }
                    String refCollNo = "";
                    try
                    {
                        refCollNo = dw_coll.GetItemString(rowNumber, "ref_collno");
                        HdRefcollNO.Value = refCollNo;
                    }
                    catch
                    {
                        refCollNo = "";
                    }

                    strList.column_name = columnName;
                    strList.xml_main = dwMainXML;
                    strList.xml_guarantee = dwCollXML;
                    strList.xml_clear = dwClearXML;
                    strList.import_flag = true;
                    strList.format_type = "CAT";

                    strList.column_data = dw_coll.GetItemString(rowNumber, "loancolltype_code") + refCollNo;
                    // เรียก service สำหรับการเปลี่ยนแปลงค่า ของ DW_coll
                    int result = shrlonService.of_itemchangecoll(state.SsWsPass, ref strList);
                    Session["strItemchange"] = strList;
                    //if ((strList.xml_message != null) && (strList.xml_message != ""))
                    //{
                    //    //dw_message.Reset();MO
                    //    //dw_message.ImportString(strList.xml_message, FileSaveAsType.Xml);
                    //    DwUtil.ImportData(strList.xml_message, dw_message, null, FileSaveAsType.Xml);
                    //    HdMsg.Value = dw_message.GetItemString(1, "msgtext");
                    //    HdMsgWarning.Value = dw_message.GetItemString(1, "msgicon");
                    //}
                    if (result == 8)
                    {
                        HdReturn.Value = result.ToString();
                        HdColumnName.Value = strList.column_name;
                    }
                    //try
                    //{
                    //    dw_otherclr.Reset();
                    //    dw_otherclr.ImportString(LtXmlOtherlr.Text, FileSaveAsType.Xml);
                    //}
                    //catch { dw_otherclr.Reset(); }
                    try
                    {
                        dw_coll.Reset();
                        dw_coll.ImportString(strList.xml_guarantee, FileSaveAsType.Xml);
                    }
                    catch { dw_coll.Reset(); }

                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }

        }

        /// <summary>
        /// sum ชำระหนี้อืนทั้งหมด
        /// </summary>       
        private void JsSumOthClr()
        {

            DateTime loanrcvfix_date = dw_main.GetItemDate(1, "loanrcvfix_date");
            String ls_memcoopid;
            Decimal ldc_contintrate = dw_main.GetItemDecimal(1, "int_contintrate");

            short year = Convert.ToInt16(loanrcvfix_date.Year + 543);
            short month = Convert.ToInt16(loanrcvfix_date.Month);
            int day_amount = 31;

            

            try { ls_memcoopid = dw_main.GetItemString(1, "memcoop_id"); }
            catch { ls_memcoopid = state.SsCoopControl; }
            Decimal clrother_amt = 0, ldc_rkeepprin = 0;
            Decimal principal_balance = 0, intestimate_amt = 0;
            Decimal sum_clear1 = 0;
            Decimal otherclr_amt = 0;

            Decimal clear_status;
            int i, j;
            int row_clr = dw_otherclr.RowCount;
            int row_clear = dw_clear.RowCount;
            if (row_clr > 0)
            {
                for (i = 0; i < row_clr; i++)
                {
                    try
                    {
                        clrother_amt = dw_otherclr.GetItemDecimal(i + 1, "clrother_amt");
                    }
                    catch { clrother_amt = 0; }
                    otherclr_amt = otherclr_amt + clrother_amt;
                    if (otherclr_amt > 0)
                    {
                        dw_main.SetItemDecimal(1, "otherclr_flag", 1);
                        dw_main.SetItemDecimal(1, "otherclr_amt", otherclr_amt);
                    }
                    else if (clrother_amt == 0)
                    {
                        dw_main.SetItemDecimal(1, "otherclr_flag", 0);
                        dw_main.SetItemDecimal(1, "otherclr_amt", 0);
                    }
                }

            }
            else
            {
                dw_main.SetItemDecimal(1, "otherclr_flag", 0);
                dw_main.SetItemDecimal(1, "otherclr_amt", 0);
            }
            if (row_clear > 0)
            {
                for (j = 0; j < row_clear; j++)
                {
                    Decimal loanpayment_type = dw_clear.GetItemDecimal(j + 1, "loanpayment_type");
                    string ls_contno = dw_clear.GetItemString(j + 1, "loancontract_no");
                    try
                    {
                        clear_status = dw_clear.GetItemDecimal(j + 1, "clear_status");
                    }
                    catch { clear_status = 0; }
                    if (clear_status == 1)
                    {

                        try
                        {
                            principal_balance = dw_clear.GetItemDecimal(j + 1, "principal_payamt");
                        }
                        catch { principal_balance = 0; }

                        try
                        {
                            if (loanpayment_type == 1)
                            {
                                intestimate_amt = dw_clear.GetItemDecimal(j + 1, "interest_payamt");
                            }
                            else { intestimate_amt = 0; }
                            // dw_clear.SetItemDecimal(j + 1, "intestimate_amt", intestimate_amt);
                        }
                        catch { intestimate_amt = 0; }

                        try
                        {
                            ldc_rkeepprin = dw_clear.GetItemDecimal(j + 1, "rkeep_principal");
                        }
                        catch
                        {
                            ldc_rkeepprin = 0;
                        }

                        sum_clear1 = sum_clear1 + principal_balance + intestimate_amt; // - ldc_rkeepprin




                        //if (Hdprincipal.Value == null || Hdprincipal.Value == "")
                        //{

                        //    try
                        //    {
                        //        principal_balance = dw_clear.GetItemDecimal(j + 1, "principal_balance");
                        //    }
                        //    catch { principal_balance = 0; }
                        //    try
                        //    {
                        //        if (loanpayment_type == 1)
                        //        {
                        //            intestimate_amt = dw_clear.GetItemDecimal(j + 1, "intclear_amt");
                        //        }
                        //        else { intestimate_amt = 0; }
                        //       // dw_clear.SetItemDecimal(j + 1, "intestimate_amt", intestimate_amt);
                        //    }
                        //    catch { intestimate_amt = 0; }
                        //}
                        //else
                        //{

                        //    try
                        //    {
                        //        principal_balance = dw_clear.GetItemDecimal(j + 1, "principal_balance");
                        //    }
                        //    catch { principal_balance = 0; }
                        //    try {
                        //        ldc_rkeepprin = dw_clear.GetItemDecimal(j + 1, "rkeep_principal");
                        //    }
                        //    catch {
                        //        ldc_rkeepprin = 0;
                        //    }
                        //    intestimate_amt = dw_clear.GetItemDecimal(j + 1, "intestimate_amt");
                        //    //try
                        //    //{
                        //    //    if (loanpayment_type == 1)
                        //    //    {
                        //    //        intestimate_amt = wcf.NShrlon.of_roundmoney(state.SsWsPass, (wcf.NShrlon.of_computeinterest(state.SsWsPass, ls_memcoopid, ls_contno, loanrcvfix_date)));
                        //    //    }
                        //    //    else { intestimate_amt = 0; }
                        //    //    dw_clear.SetItemDecimal(j + 1, "intestimate_amt", intestimate_amt);
                        //    //}
                        //    //catch { intestimate_amt = 0; }
                        //}


                        //sum_clear1 = sum_clear1 + principal_balance - ldc_rkeepprin + intestimate_amt;
                        
                    }


                    else if (clear_status == 0)
                    {

                        //day_amount = postingdate.DayOfYear - postingdate_bf.DayOfYear;
                        //principal_balance = dw_clear.GetItemDecimal(j + 1, "principal_balance");
                        //intestimate_amt = principal_balance * (ldc_contintrate) * day_amount / 365;
                        //intestimate_amt = wcf.NShrlon.of_roundmoney(state.SsWsPass, intestimate_amt);
                        ////  intestimate_amt = wcf.NShrlon.of_roundmoney(state.SsWsPass, (wcf.NShrlon.of_computeinterest(state.SsWsPass, ls_memcoopid, ls_contno, loanrcvfix_date)));

                        //dw_clear.SetItemDecimal(j + 1, "intestimate_amt", intestimate_amt);

                    }
                }

            }
            of_calintestimatemain();
            Decimal total = sum_clear1;
            dw_main.SetItemDecimal(1, "sum_clear", total);
            if (total > 0)
            {
                dw_main.SetItemDecimal(1, "clearloan_flag", 1);
            }
            else { dw_main.SetItemDecimal(1, "clearloan_flag", 0); }
            // of_recalloanpermiss();
            JsChecksalarybal();
        }

        /// <summary>
        /// sum ชำระหนี้เก่าทั้งหมด
        /// </summary>  
      


        /// <summary>
        /// หาสิทธิ์กู้สูงสุดจากอายุสมาชิก
        /// </summary>
        private void Jsmaxcreditperiod()
        {
            try
            {
                String ls_loantype = dw_main.GetItemString(1, "loantype_code");
                DateTime ldtm_member;
                try
                {
                    ldtm_member = dw_main.GetItemDateTime(1, "member_date");
                }
                catch { ldtm_member = state.SsWorkDate; }


                int ldc_share_lastperiod = Convert.ToInt16(dw_main.GetItemDecimal(1, "share_lastperiod"));
                int li_loanrequest_type = Convert.ToInt16(dw_main.GetItemDecimal(1, "loanrequest_type"));

                Decimal ldc_shrstkvalue = dw_main.GetItemDecimal(1, "sharestk_value");
                Decimal ldc_salary = dw_main.GetItemDecimal(1, "salary_amt");
                String ls_memcoopid = dw_main.GetItemString(1, "memcoop_id");
                //  String ls_membtypedesc = dw_main.GetItemString(1, "membtype_desc");
                String member_no = dw_main.GetItemString(1, "member_no");

                ///<หาอายุงานของาสมาชิก>

                ///<กำหนดวันที่จ่ายและเรียกเก็บแต่ปละเภทสัญญาวันจ่ายไม่เหมือนกัน>
                JsChangeStartkeep();

                Decimal[] max_creditperiod = new Decimal[4];
                string loanrighttype_code = "01";
                Decimal loancredit_amt = 0, loanright_type = 1;
                ls_loantype = dw_main.GetItemString(1, "loantype_code");
                decimal li_timeage = dw_main.GetItemDecimal(1, "birth_age");
                //decimal ldc_timembyear = Convert.ToInt16(dw_main.GetItemDecimal(1, "member_age"));
                decimal ldc_timembyear = dw_main.GetItemDecimal(1, "member_age");
                string ls_timembyear = ldc_timembyear.ToString("##00.00");
                int li_timemb = Convert.ToInt16(ls_timembyear.Substring(0, 2)) * 12 + Convert.ToInt16(ls_timembyear.Substring(3, 2));   //ert.ToInt16(Math.Truncate(ldc_timembyear) * 12 + ((ldc_timembyear * 100) % 100));

                decimal customtime_type = 0;
//                string sqllntype = @"select  loanrighttype_code,customtime_type, loanright_type
//                                        from lnloantype  where loantype_code ='" + ls_loantype + @"' ";
//                Sdt dtrigtht = WebUtil.QuerySdt(sqllntype);
//                if (dtrigtht.Next())
//                {
//                    loanrighttype_code = dtrigtht.GetString("loanrighttype_code");
//                    customtime_type = dtrigtht.GetDecimal("customtime_type");
//                    loanright_type = dtrigtht.GetDecimal("loanright_type");
//                }

               customtime_type = Convert.ToInt16( Hdcustomtime_type.Value) ;
               loanright_type = Convert.ToInt16( Hdloanright_type.Value );
               loanrighttype_code = Hdloanrighttype_code.Value;
                //งวดหุ้น
                if (customtime_type == 1)
                {
                    li_timemb = ldc_share_lastperiod;
                }
                else if (customtime_type == 3)
                {
                    //เงินวิทยะฐานะ
                    li_timemb = Convert.ToInt32(dw_main.GetItemDecimal(1, "incomemonth_other"));
                    ldc_salary = dw_main.GetItemDecimal(1, "incomemonth_other");
                } if (loanright_type == 3)
                {
                    //ดูจากสัญญาหลัก  คือ อ่านตารางกำหนดวงเงินกู้ OD
                    decimal ldc_maxloanamt = 0, maxperiod_payamt = 0;
                    DateTime ldtm_expirecont, ldtm_today;
                    string ls_contcreditno = "";
                    string ls_Sql = @"  select	contcredit_no, loancreditbal_amt, expirecont_date, contract_time,maxperiod_payamt
                                from	lncontcredit          where	(  member_no	 =  '" + member_no + @"'  ) and
		                        ( loantype_code	=  '" + ls_loantype + "') and      ( contcredit_status	= 1 )";



                    Sdt dtcreditc = WebUtil.QuerySdt(ls_Sql);
                    if (dtcreditc.GetRowCount() <= 0)
                    {
                        ldc_maxloanamt = 0;
                        ls_contcreditno = "";
                    }
                    if (dtcreditc.Next())
                    {
                        try
                        {
                            ldc_maxloanamt = dtcreditc.GetDecimal("loancreditbal_amt");
                            maxperiod_payamt = dtcreditc.GetDecimal("maxperiod_payamt");
                        }
                        catch { ldc_maxloanamt = 0; }
                        loancredit_amt = ldc_maxloanamt;
                        dw_main.SetItemDecimal(1, "loancredit_amt", ldc_maxloanamt);
                        dw_main.SetItemDecimal(1, "maxsend_payamt", maxperiod_payamt);
                        //DateTime ldtm_expirecont = dt .GetDate("expirecont_date");
                        // ls_contcreditno = dtcredit.GetString("contcredit_no");
                    }

                }
                else if (loanright_type == 5)
                {
                    //กู้ปันผล
                    JsLoanCreditDividend();
                   

                }
                else
                {
                    if ( li_timemb < ldc_share_lastperiod ) { li_timemb = ldc_share_lastperiod; }
                    ///< หาสิทธิ์กู้สูงสุด,งวดสูงสุด>
                    ///
                    try
                    {
                        max_creditperiod = Calloanpermisssurin(state.SsWsPass, ls_memcoopid, ls_loantype, ldc_salary, ldc_shrstkvalue, li_timemb, member_no, li_timeage);
                        //max_creditperiod = shrlonService.Calloanpermiss(state.SsWsPass, ls_memcoopid, ls_loantype, ldc_salary, ldc_shrstkvalue, li_timemb, member_no, li_timeage);
                    }
                    catch (Exception ex) {
                        LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                    
                    }
                        Decimal ldc_percenshare = max_creditperiod[2];
                    Decimal ldc_percensaraly = max_creditperiod[3];

                    loancredit_amt = max_creditperiod[0];

                    //mong
                    //String ll_roundloan = wcf.NBusscom.of_getattribloantype(state.SsWsPass, ls_loantype, "reqround_factor");
                    String ll_roundloan = Hdreqround_factor.Value;
                    int roundloan = Convert.ToInt16(ll_roundloan);
                    if (loancredit_amt % roundloan > 0)
                    {
                        loancredit_amt = loancredit_amt - (loancredit_amt % roundloan);
                    }

                    if (li_loanrequest_type == 2)
                    {
                        //กรณี กู้เพิ่ม


                    }

                    dw_main.SetItemDecimal(1, "loancredit_amt", loancredit_amt);
                    dw_main.SetItemDecimal(1, "maxsend_payamt", max_creditperiod[1]);

                }

                JsCalperiodSend();//เช็คงวดเกษียณอายุ
                Genbaseloanclear(); //หนี้เดิม
                of_setloanclearstatus(); //บังคับหักกลบอะไรบ้าง
                JsSetMonthpayCoop(); //ยอดหักสหกรณ์
               
                if (loanright_type != 5)
                {
                    JsContPeriod(); // ยอดชำระ/งวด
                } 
                //mong56-10-15
                if (loanright_type == 5) 
                {
                 //ตรวจว่าเคยกู้ปันผลไปแล้วหรอเปล่า
                    decimal loan_balance = 0;
                    int li_find = dw_clear.FindRow("loantype_code = '" + ls_loantype + "'", 1, dw_clear.RowCount);
                   
                    if (li_find > 0)
                    {
                        loan_balance = dw_clear.GetItemDecimal(li_find, "principal_balance");
                        if (loan_balance > 0)
                        {
                            dw_main.SetItemDecimal(1, "loancredit_amt", 0);
                            dw_main.SetItemDecimal(1, "loanreqregis_amt", 0);
                            dw_main.SetItemDecimal(1, "loanrequest_amt", 0);
                            LtServerMessage.Text = WebUtil.ErrorMessage("สมาชิกท่านนี้ เลขที่" + member_no + "ได้ทำการกู้ฉุกเฉินปันผล ไปแล้ว กรุณาตรวจสอบ...");
                        }
                    }

                
                }
                Jsfirstperiod(); // ดบ. ชำระงวดแรก
                JsSumOthClr(); //ยอดหักอืนรวม
                JsInsertRowcoll(); //ค้ำประกัน กี่แภว
            }
            catch (Exception ex)
            {
                // LtServerMessage.Text = WebUtil.ErrorMessage("Jsmaxcreditperiod===>" + ex);
            }

        }


        private void JsSetMonthpayCoop()
        {
            try
            {
                string ls_loantype;
                int li_index, li_count;
                int li_clrstatus, li_paytype, li_shrpaystatus, li_chkbalacestatus;

                Decimal ldc_shrperiod, ldc_payment, ldc_intestm;
                Decimal ldc_sumpay = 0, li_paymentstatus = 1;

                // ดึงรายการหุ้น
                ldc_shrperiod = dw_main.GetItemDecimal(1, "periodshare_value");
                li_shrpaystatus = Convert.ToInt32(dw_main.GetItemDecimal(1, "sharepay_status"));

                // ถ้างดเก็บค่าุหุ้นให้หุ้นต่อเดือนเป็นศูนย์
                if (li_shrpaystatus == -1) { ldc_shrperiod = 0; }

                // ดึงรายการหนี้
                li_count = dw_clear.RowCount;
                Decimal li_odflag = 0, ldc_loamappamt = 0, li_periodamt = 0;
                for (li_index = 1; li_index <= li_count; li_index++)
                {

                    li_clrstatus = Convert.ToInt32(dw_clear.GetItemDecimal(li_index, "clear_status"));
                    ls_loantype = dw_clear.GetItemString(li_index, "loantype_code");
                    //li_chkbalacestatus = Convert.ToInt32(dw_clear.GetItemDecimal(li_index, "clear_status"));

                    if (li_clrstatus == 0)
                    {
                        li_paytype = Convert.ToInt32(dw_clear.GetItemDecimal(li_index, "loanpayment_type"));
                        ldc_payment = dw_clear.GetItemDecimal(li_index, "period_payment");
                        ldc_loamappamt = dw_clear.GetItemDecimal(li_index, "loanapprove_amt");
                        li_periodamt = dw_clear.GetItemDecimal(li_index, "period_payamt");
                        ldc_intestm = dw_clear.GetItemDecimal(li_index, "intestimate_amt");
                        li_odflag = dw_clear.GetItemDecimal(li_index, "countpay_flag");
                        li_paymentstatus = dw_clear.GetItemDecimal(li_index, "payment_status");
                        if (li_paymentstatus == -13 || li_paymentstatus == -9)
                        {
                            ldc_payment = 0;
                            ldc_intestm = 0;
                        }
                        if (li_odflag == 1)
                        {

                            ldc_payment = ldc_loamappamt / li_periodamt;
                        }
                        if (li_paytype == 1)
                        {
                            ldc_sumpay += ldc_payment + ldc_intestm;
                        }
                        else
                        {
                            ldc_sumpay += ldc_payment;
                        }
                    }
                }

                ldc_sumpay = ldc_sumpay + ldc_shrperiod;
                dw_main.SetItemDecimal(1, "paymonth_coop", ldc_sumpay);
            }
            catch
            {

            }
        }


        private void of_recalloanpermiss()
        {

            JsSetMonthpayCoop();
            //สิทธิกู้ตามเงินเดือนคงเลหือ
            JsCalMaxLoanpermiss();
            JsContPeriod();
            JsGenBuyshare();           
            JsSumOthClr();

        }

        /// <summary>
        /// ตรวจสอบสิทธิ์ตามเงินเดือนคงเหลือ
        /// </summary>
        protected void JsPermissSalary()
        {


            //คำนวณสิทธิกู้จากยอดเงินเดือนคงเหลือขั้นต่ำ
            JsSetMonthpayCoop();
            Decimal ldc_shrstkvalue = dw_main.GetItemDecimal(1, "sharestk_value");
            String loantype_code = dw_main.GetItemString(1, "loantype_code");
            decimal ldc_salary = dw_main.GetItemDecimal(1, "salary_amt");
            decimal ldc_mthcoop = dw_main.GetItemDecimal(1, "paymonth_coop");
            decimal ldc_minpercsal = dw_main.GetItemDecimal(1, "minsalary_perc");
            decimal ldc_minsalaamt = dw_main.GetItemDecimal(1, "minsalary_amt");
            decimal ldc_maxloan = dw_main.GetItemDecimal(1, "loancredit_amt");
            decimal ldc_periodsend = dw_main.GetItemDecimal(1, "period_payamt");
            decimal ldc_paymenttype = dw_main.GetItemDecimal(1, "loanpayment_type");
            decimal ldc_intrate = dw_main.GetItemDecimal(1, "int_contintrate");
            decimal retry_age = dw_main.GetItemDecimal(1, "retry_age");

            decimal ldc_incomeetc = dw_main.GetItemDecimal(1, "incomemonth_fixed");
            decimal ldc_paymtmetc = dw_main.GetItemDecimal(1, "paymonth_other");

            String ll_roundloan = wcf.NBusscom.of_getattribloantype(state.SsWsPass, loantype_code, "reqround_factor");
            int roundloan = Convert.ToInt16(ll_roundloan);
            String ll_roundpay = wcf.NBusscom.of_getattribloantype(state.SsWsPass, loantype_code, "payround_factor");
            int roundpay = Convert.ToInt16(ll_roundpay);

            if (ldc_periodsend > retry_age)
            {
                ldc_periodsend = retry_age;
            }
            //คำนวณเงินเดือนคงเหลือขั้นต่ำ

            if (ldc_minsalaamt < (ldc_salary * ldc_minpercsal))
            {
                ldc_minsalaamt = Math.Round(ldc_salary * ldc_minpercsal);
            }

            decimal salary_balance = ldc_salary - ldc_minsalaamt - ldc_mthcoop + ldc_incomeetc - ldc_paymtmetc;
            decimal ldc_permamt;
            double li_maxperiod = Convert.ToDouble(ldc_periodsend);

            if (ldc_paymenttype == 1)
            {

                //คงต้น
                decimal ldc_temp = (ldc_periodsend * (ldc_intrate * (30 / 365)) + 1);
                ldc_permamt = (salary_balance * ldc_periodsend) / ldc_temp;

            }
            else
            { //คงยอด
                int li_fixcaltype = 1;//fixpaycal_type
                double ldc_permamttmp = 1, ldc_fr = 1, ldc_temp = 1;

                if (li_fixcaltype == 1)
                {
                    // ด/บ ทั้งปี / 12
                    // ldc_permamt = salary_balance * ((((1 + (ldc_intrate / 12)) ^ li_maxperiod) - 1) / ((ldc_intrate / 12) * ((1 + (ldc_intrate / 12)) ^ li_maxperiod)));
                    ldc_temp = Math.Log(1 + (Convert.ToDouble(ldc_intrate / 12)));
                    ldc_fr = Math.Exp(-li_maxperiod * ldc_temp);
                    ldc_permamttmp = (Convert.ToDouble(salary_balance) * (1 - ldc_fr)) / ((Convert.ToDouble(ldc_intrate) / 12));
                }
                else
                {
                    // ด/บ 30 วัน/เดือน
                    ldc_temp = Math.Log(1 + (Convert.ToDouble(ldc_intrate / (30 / 365))));
                    ldc_fr = Math.Exp(-li_maxperiod * ldc_temp);
                    ldc_permamttmp = (Convert.ToDouble(salary_balance) * (1 - ldc_fr)) / ((Convert.ToDouble(ldc_intrate) / (30 / 365)));

                }
                ldc_permamt = Convert.ToDecimal(ldc_permamttmp);
                decimal loan_credit = dw_main.GetItemDecimal(1, "loancredit_amt");
                if (ldc_permamt > loan_credit)
                {
                    ldc_permamt = loan_credit;
                }
            }

            //ตรวจสอบยอดขอกู้กลุ๋ม
            // JsGetloangrouppermissuesed();
            decimal loan_groupuse = 0;// dw_main.GetItemDecimal(1, "loangrpuse_amt");
            ldc_permamt = ldc_permamt - loan_groupuse;
            while (ldc_permamt > ldc_maxloan)
            {
                ldc_permamt = ldc_maxloan - 10;


            }
            if (ldc_permamt < 0) { ldc_permamt = 0; }
            // wa ldc_permamt = Math.Round(ldc_permamt / roundloan) * roundloan;

            if (ldc_permamt % roundloan > 0)
            {
                ldc_permamt = ldc_permamt - (ldc_permamt % roundloan);
            }

            if (ldc_permamt > ldc_maxloan) { ldc_permamt = ldc_permamt - roundloan; }
            decimal period_payment = Math.Round((ldc_permamt / ldc_periodsend) / roundpay) * roundpay;



            dw_main.SetItemDecimal(1, "loanpermiss_amt", ldc_permamt);
            dw_main.SetItemDecimal(1, "loanrequest_amt", ldc_permamt);
            dw_main.SetItemDecimal(1, "period_payment", period_payment);
            dw_main.SetItemDecimal(1, "period_payamt", ldc_periodsend);

            Decimal intestimate_amt = of_calintestimatemain();
            dw_main.SetItemDecimal(1, "intestimate_amt", intestimate_amt);
            JsGenBuyshare();

            JsInsertRowcoll();

        }

        private int JsCheckDataBeforesave()
        {
            //ตรวจค้ำประกัน  wa

            int coll_num = 0;
            string loantype_code = dw_main.GetItemString(1, "loantype_code");
            decimal loanrequest_amt = dw_main.GetItemDecimal(1, "loanrequest_amt");
            int collrow = dw_coll.RowCount;

            String sqlpro = @" select useman_amt, useshare_flag from lnloantypereqgrt  
                            where loantype_code = '" + loantype_code + "' and money_from <=  " + loanrequest_amt.ToString() + @" 
                                    and  money_to >= " + loanrequest_amt.ToString();
            Sdt dtgrt = WebUtil.QuerySdt(sqlpro);
            if (dtgrt.Next())
            {
                coll_num = Convert.ToInt32(dtgrt.GetDecimal("useman_amt"));
                int coll_share = Convert.ToInt32(dtgrt.GetDecimal("useshare_flag"));

            }
            else { coll_num = 0; }

            if (coll_num > collrow)
            {
                //แสดงข้อความ
                LtServerMessage.Text = WebUtil.ErrorMessage("ท่านป้อนสมาชิกค้ำประกันไม่ครบ กรุณาป้อนให้ครบด้วย ต้องระบุคนค้ำประกัน จำนวน " + coll_num.ToString());
                return 1;
            }

            return 1;
        }

        private void JsInsertRowcoll()
        {
            try
            {
                String loantype_code = dw_main.GetItemString(1, "loantype_code");
                Decimal ldc_shrstkvalue = dw_main.GetItemDecimal(1, "sharestk_value");
                Decimal ldc_permamt = dw_main.GetItemDecimal(1, "loanrequest_amt");
                Decimal ldc_useshare_flag = 0, ldc_useman_type = 0, ldc_useman_amt = 0;

                String sqlreqgrt = @"   SELECT COOP_ID,   
                                        LOANTYPE_CODE,   
                                        SEQ_NO,   
                                        MONEY_FROM,   
                                        MONEY_TO,   
                                        USESHARE_FLAG,   
                                        USEMAN_AMT,   
                                        USEMAN_TYPE,   
                                        USEMEMMAIN_AMT,   
                                        USEMEMCO_AMT,   
                                        USEMEM_OPERATION  
                                    FROM  LNLOANTYPEREQGRT    
                                    WHERE COOP_ID ='" + state.SsCoopControl + @"'    
                                  and LOANTYPE_CODE='" + loantype_code + @"'
                                and MONEY_FROM <=" + ldc_permamt + " and MONEY_TO >" + ldc_permamt + " ";
                Sdt dtreqgrt = WebUtil.QuerySdt(sqlreqgrt);
                if (dtreqgrt.Next())
                {
                    ldc_useshare_flag = dtreqgrt.GetDecimal("USESHARE_FLAG");
                    ldc_useman_type = dtreqgrt.GetDecimal("USEMAN_TYPE");
                    ldc_useman_amt = dtreqgrt.GetDecimal("USEMAN_AMT");
                }
                if (dtreqgrt.GetRowCount() <= 0)
                {
                    ldc_useshare_flag = 0;
                    ldc_useman_type = 0;
                }
                string membno = dw_main.GetItemString(1, "member_no");
                string memb_name = dw_main.GetItemString(1, "member_name");
                if (ldc_useshare_flag == 1) // หุ้น
                {
                    dw_coll.Reset();
                    int row = dw_coll.InsertRow(1);

                    dw_coll.SetItemString(row, "loancolltype_code", "02");
                    dw_coll.SetItemString(row, "ref_collno", membno);
                    dw_coll.SetItemString(row, "description", memb_name);
                    dw_coll.SetItemDecimal(row, "coll_amt", ldc_permamt);
                    dw_coll.SetItemDecimal(row, "coll_balance", ldc_permamt);
                    dw_coll.SetItemDecimal(row, "use_amt", ldc_permamt);
                    dw_coll.SetItemString(row, "coop_id", state.SsCoopControl);
                }
                if (ldc_useman_type >= 1)
                {
                    int row = dw_coll.RowCount;

                    for (int i = 1; i <= ldc_useman_amt; i++)
                    {
                        if (row < ldc_useman_amt)
                        {
                            dw_coll.InsertRow(0);
                        }
                        else { break; }
                    }
                }
                //if (dw_coll.RowCount > ldc_useman_type)
                //{
                //    for (int i = 1; i < dw_coll.RowCount; i++)
                //    {
                //        DwUtil.DeleteLastRow(dw_coll);
                //    }
                //}
                DwUtil.RetrieveDDDW(dw_coll, "coop_id", pbl, null);
            }
            catch { 
            
            }
        }

        /// <summary>
        /// ชื้อหุ้นเพิ่ม
        /// </summary>
        private void JsGenBuyshare()
        {

            try
            {
                Ltjspopup.Text = " ";
                decimal buyshare_amt = 0;
                string loantypereq_code = dw_main.GetItemString(1, "loantype_code");
                decimal loanrequest_amt = dw_main.GetItemDecimal(1, "loanrequest_amt");
                decimal sharestk_val = dw_main.GetItemDecimal(1, "sharestk_value");
                decimal shrstk_buytype = 0, shrbuyext_amt = 0, buyshrlnreq_percen = 0, buyshrrecnet_percen = 0;
                decimal sharestk_percent = 0, ldc_prnbalance = 0, buyshrmax_amt = 0;

                string sqllntype = @"select  shrstk_buytype
                                        from lnloantype  where loantype_code ='" + loantypereq_code + @"' ";
                Sdt dtstk = WebUtil.QuerySdt(sqllntype);
                if (dtstk.Next())
                {
                    shrstk_buytype = dtstk.GetDecimal("shrstk_buytype");

                }

                string sql = "select sharestk_percent, buyshrext_amt,buyshrlnreq_percen, buyshrrecnet_percen ,buyshrmax_amt  from lnloantypebuyshare where loantype_code ='" + loantypereq_code + "' and startloan_amt <= " + loanrequest_amt + " and endloan_amt >= " + loanrequest_amt;
                Sdt dttype = WebUtil.QuerySdt(sql);
                if (dttype.Next())
                {

                    sharestk_percent = dttype.GetDecimal("sharestk_percent");
                    shrbuyext_amt = dttype.GetDecimal("buyshrext_amt");
                    buyshrlnreq_percen = dttype.GetDecimal("buyshrlnreq_percen");
                    buyshrrecnet_percen = dttype.GetDecimal("buyshrrecnet_percen");
                    buyshrmax_amt = dttype.GetDecimal("buyshrmax_amt");
                }

                int li_count = dw_clear.RowCount;
                int li_index, li_clrstatus, li_paytype, li_countpayflag;
                Decimal ldc_principal_balance = 0, ldc_intestm, ldc_sumpay = 0;

                string ls_loantype = "";
                if (shrstk_buytype == 1)
                {
                    ldc_principal_balance += loanrequest_amt;
                    decimal shareperc_val = (sharestk_val / ldc_principal_balance) * 100;
                    if (shareperc_val <= sharestk_percent)
                    {

                        shareperc_val = sharestk_percent - shareperc_val;
                        buyshare_amt = (ldc_principal_balance * shareperc_val) / 100;
                        decimal ldc_mod = buyshare_amt % 10;
                        if (ldc_mod >= 1)
                        {
                            buyshare_amt = buyshare_amt + (10 - ldc_mod) - 10;

                        }

                        dw_otherclr.InsertRow(0);
                        dw_otherclr.SetItemString(1, "clrothertype_code", "SHR");
                        dw_otherclr.SetItemString(1, "clrother_desc", "ซื้อหุ้นเพิ่ม");
                        dw_otherclr.SetItemDecimal(1, "clear_status", 1);
                        dw_otherclr.SetItemDecimal(1, "clrother_amt", buyshare_amt);
                    }

                }
                else if (shrstk_buytype == 2)
                {
                    //ยอดหนี้ทั้งหมด

                    decimal total_balance = 0, ldc_mod = 0;
                    for (li_index = 1; li_index <= li_count; li_index++)
                    {

                        li_clrstatus = Convert.ToInt32(dw_clear.GetItemDecimal(li_index, "clear_status"));
                        ls_loantype = dw_clear.GetItemString(li_index, "loantype_code");
                        li_countpayflag = Convert.ToInt32(dw_clear.GetItemDecimal(li_index, "contlaw_status"));
                        if (li_clrstatus == 0 && li_countpayflag >= 1)
                        {
                            li_paytype = Convert.ToInt32(dw_clear.GetItemDecimal(li_index, "loanpayment_type"));
                            ldc_principal_balance = dw_clear.GetItemDecimal(li_index, "principal_balance");
                            total_balance += ldc_principal_balance;

                        }
                    }

                    total_balance += loanrequest_amt;
                    decimal shareminloan = total_balance * sharestk_percent/100;
                    ldc_mod = shareminloan % 10;
                    if (ldc_mod > 0)
                    {

                        shareminloan = shareminloan + (10 - ldc_mod);
                    }
                    if (shareminloan > sharestk_val) {

                        buyshare_amt = shareminloan - sharestk_val;
                       // Ltjspopup.Text = WebUtil.WarningMessage(" สมาชิกท่านนี้ต้องซื้อหุ้นเพิ่ม " + buyshare_amt.ToString("#,##0"));
                    }
                    decimal shareperc_val = (sharestk_val / total_balance) * 100;
                    if (buyshare_amt > 0 )
                    {
                              Ltjspopup.Text = WebUtil.ErrorMessage(" สมาชิกท่านนี้ต้องซื้อหุ้นเพิ่ม " + buyshare_amt.ToString("#,##0"));
                      
                    }
                    else
                    {
                        if (dw_otherclr.RowCount >= 1) { dw_otherclr.DeleteRow(dw_otherclr.RowCount); }

                    }
                    //ยอดรับสุทธิ
                    if (buyshrlnreq_percen > 0 || buyshrrecnet_percen > 0)
                    {
                        ldc_principal_balance = 0;
                        ldc_prnbalance = 0;
                        for (li_index = 1; li_index <= li_count; li_index++)
                        {

                            li_clrstatus = Convert.ToInt32(dw_clear.GetItemDecimal(li_index, "clear_status"));
                            ls_loantype = dw_clear.GetItemString(li_index, "loantype_code");

                            if (li_clrstatus == 1 && loantypereq_code == ls_loantype)
                            {
                                li_paytype = Convert.ToInt32(dw_clear.GetItemDecimal(li_index, "loanpayment_type"));
                                ldc_prnbalance = dw_clear.GetItemDecimal(li_index, "principal_balance");
                                ldc_principal_balance += ldc_prnbalance;

                            }
                        }
                    }

                    //wa
                    if (buyshrlnreq_percen > 0 && ldc_principal_balance == 0)
                    {
                        decimal shrbuyinc_amt = Convert.ToDecimal(loanrequest_amt * (buyshrlnreq_percen / 100));
                        ldc_mod = shrbuyinc_amt % 10;
                        if (ldc_mod >= 1)
                        {

                            shrbuyinc_amt = shrbuyinc_amt + (10 - ldc_mod);



                        }
                        Ltjspopup.Text += " ต้องซื้อเพิ่มจากยอดขอกู้ " + shrbuyinc_amt.ToString("#,##0");
                        buyshare_amt += shrbuyinc_amt;

                    }
                    //wa

                    if (buyshrrecnet_percen > 0 && ldc_principal_balance > 0)
                    {


                        decimal loannetreceive_amt = loanrequest_amt - ldc_principal_balance;

                        decimal shrbuyincnet_amt = Convert.ToDecimal(loannetreceive_amt * (buyshrrecnet_percen / 100));

                        if (shrbuyincnet_amt > 10000 && loantypereq_code == "13") { shrbuyincnet_amt = 10000; }
                        ldc_mod = shrbuyincnet_amt % 10;
                        if (ldc_mod >= 1)
                        {
                            shrbuyincnet_amt = shrbuyincnet_amt + (10 - ldc_mod);


                        }
                        Ltjspopup.Text += " ต้องซื้อเพิ่มจากยอดกู้เพิ่ม " + shrbuyincnet_amt.ToString("#,##0");
                        string ls_obj = dw_main.GetItemString(1, "loanobjective_code");
                        //กรณีกู้เพือประกอบอาชีพ  ให้คิด 10000 อย่างเดีย
                        if (shrbuyincnet_amt > 10000 && loantypereq_code == "13" && ls_obj == "009")
                        {

                            buyshare_amt = shrbuyincnet_amt;
                        }
                        else
                        {
                            buyshare_amt += shrbuyincnet_amt;
                        }
                    }
                    //wa
                    //if (shrbuyext_amt  > 0  && sharestk_percent > 0)
                    //{
                    buyshare_amt += shrbuyext_amt;
                    if (buyshrmax_amt > 0 && buyshare_amt > buyshrmax_amt) {
                        buyshare_amt = buyshrmax_amt;
                    }
                    //}
                    if (buyshare_amt >= 1)
                    {
                        int li_rowcount = dw_otherclr.RowCount;
                        if (li_rowcount < 1)
                        {
                            dw_otherclr.InsertRow(0);
                        }
                        dw_otherclr.SetItemString(1, "clrothertype_code", "SHR");
                        dw_otherclr.SetItemString(1, "clrother_desc", "ซื้อหุ้นเพิ่ม");
                        dw_otherclr.SetItemDecimal(1, "clear_status", 1);
                        dw_otherclr.SetItemDecimal(1, "clrother_amt", buyshare_amt);
                    }
                }
                else if (shrstk_buytype == 3)
                {
                    //ยอดกลุ่มเงินกู้


                }
                else if (shrstk_buytype == 4)
                {
                    //ยอดรับสุทธิ
                    for (li_index = 1; li_index <= li_count; li_index++)
                    {

                        li_clrstatus = Convert.ToInt32(dw_clear.GetItemDecimal(li_index, "clear_status"));
                        ls_loantype = dw_clear.GetItemString(li_index, "loantype_code");

                        if (li_clrstatus == 1)
                        {
                            li_paytype = Convert.ToInt32(dw_clear.GetItemDecimal(li_index, "loanpayment_type"));
                            ldc_principal_balance = dw_clear.GetItemDecimal(li_index, "principal_balance");
                            ldc_principal_balance += ldc_principal_balance;

                        }
                    }

                    decimal loannetreceive_amt = loanrequest_amt - ldc_principal_balance;
                    if (loannetreceive_amt > 0)
                    {
                        buyshare_amt = (loannetreceive_amt * sharestk_percent) / 100;
                        decimal ldc_mod = buyshare_amt % 10;
                        if (ldc_mod >= 1)
                        {
                            buyshare_amt = buyshare_amt + (10 - ldc_mod) - 10;

                        }
                    }
                    else
                    {
                        buyshare_amt = 0;
                    }
                    if (buyshare_amt > 0)
                    {
                        int li_row0 = dw_otherclr.RowCount;
                        if (li_row0 < 1)
                        {
                            dw_otherclr.InsertRow(0);
                        }
                        dw_otherclr.SetItemString(1, "clrothertype_code", "SHR");
                        dw_otherclr.SetItemString(1, "clrother_desc", "ซื้อหุ้นเพิ่ม");
                        dw_otherclr.SetItemDecimal(1, "clear_status", 1);
                        dw_otherclr.SetItemDecimal(1, "clrother_amt", buyshare_amt);
                    }
                }

                else
                {
                    if (dw_otherclr.RowCount >= 1) { dw_otherclr.DeleteRow(dw_otherclr.RowCount); }
                }

            }
            catch { 
            
            }
        }

        //หักงวดแรก
        private void Jsfirstperiod()
        {
            try
            {
                string ls_loantype = dw_main.GetItemString(1, "loantype_code");
                string ls_memno = dw_main.GetItemString(1, "member_no");
                decimal ldc_calintflag = 0;
                String sqlStrcalintfuture = @" SELECT calintfuture_flag
                                    FROM LNLOANTYPE
                                    WHERE LOANTYPE_CODE='" + ls_loantype + "'";

                Sdt dt1 = WebUtil.QuerySdt(sqlStrcalintfuture);
                if (dt1.GetRowCount() < 1) { ldc_calintflag = 0; }
                if (dt1.Next())
                {
                    try
                    {
                        ldc_calintflag = dt1.GetDecimal("calintfuture_flag");

                    }
                    catch
                    {
                        ldc_calintflag = 0;
                    }
                }
                Decimal intestimate_amt = 0;
                if (ldc_calintflag == 0)
                {

                }
                else if (ldc_calintflag == 1)
                {
                    intestimate_amt = this.of_calinterestfuture();

                    try
                    {

                        int li_find = dw_otherclr.FindRow("clrothertype_code = 'SFP'", 1, dw_otherclr.RowCount);
                        if (li_find < 1)
                        {
                            li_find = dw_otherclr.InsertRow(0);
                        }

                        dw_otherclr.SetItemString(li_find, "clrothertype_code", "SFP");
                        dw_otherclr.SetItemString(li_find, "clrother_desc", "หักชำระ ดบ. ล่วงหน้า");
                        dw_otherclr.SetItemDecimal(li_find, "clear_status", 1);
                        dw_otherclr.SetItemDecimal(li_find, "clrother_amt", intestimate_amt);

                        JsSumOthClr();
                    }
                    catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex.ToString()); }
                }
                else
                {
                    if (ldc_calintflag == 2)
                    {
                        intestimate_amt = of_calinterestfuture();
                    }
                    string sqldeptno = @" SELECT deptaccount_no
                                    FROM mbmembmaster
                                    WHERE member_no='" + ls_memno + "'";

                    Sdt dt2 = WebUtil.QuerySdt(sqldeptno);

                    if (dt2.Next())
                    {
                        try
                        {
                            string ls_deptno = dt2.GetString("deptaccount_no");
                            dw_otherclr.InsertRow(0);
                            int row = dw_otherclr.RowCount;
                            dw_otherclr.SetItemString(row, "clrothertype_code", "DEP");
                            dw_otherclr.SetItemString(row, "clrother_desc", ls_deptno);
                            dw_otherclr.SetItemDecimal(row, "clear_status", 1);
                            dw_otherclr.SetItemDecimal(row, "clrother_amt", intestimate_amt);
                        }
                        catch
                        {

                        }
                    }

                }
            }
            catch { 
            
            }
        }
        private void JsGetitemdescetc()
        {
            try
            {
                int row = Convert.ToInt16(HdRowNumber.Value);
                string itemetc_code = dw_otherclr.GetItemString(row, "clrothertype_code");
                string sql_prmgrp = "select    slipitemtype_desc    from slucfslipitemtype  where slipitemtype_code = '" + itemetc_code + "'";
                Sdt dt_itemetc = WebUtil.QuerySdt(sql_prmgrp);
                if (dt_itemetc.Next())
                {
                    string item_desc = dt_itemetc.GetString("slipitemtype_desc");
                    dw_otherclr.SetItemString(row, "clrother_desc", item_desc);
                }
            }
            catch
            {

            }

        }
        private void JsChecksalarybal()
        {
            try
            {
                decimal loanpayment_type = dw_main.GetItemDecimal(1, "loanpayment_type");
                decimal period_payment = dw_main.GetItemDecimal(1, "period_payment");//ยอดชำระสัญญาใหม่
                decimal loanpayment_status = dw_main.GetItemDecimal(1, "loanpayment_status");
                decimal intestimate_amt = dw_main.GetItemDecimal(1, "intestimate_amt");
                decimal paymonth_coop = dw_main.GetItemDecimal(1, "paymonth_coop");//ยอดหักจากสหกรณ์
                decimal minsalary_balance = dw_main.GetItemDecimal(1, "minsalary_amt");//เงินเดือนคงเหลือขั้นต่ำ(ตาราง lnloantype)
                decimal salary_amt = dw_main.GetItemDecimal(1, "salary_amt");
                decimal ldc_incomeetc = dw_main.GetItemDecimal(1, "incomemonth_fixed");
                decimal ldc_paymtmetc = dw_main.GetItemDecimal(1, "paymonth_other");
                decimal custompay_flag = dw_main.GetItemDecimal(1, "custompayment_flag");
                decimal retrysend_flag = dw_main.GetItemDecimal(1, "tax_amt"); //ส่งเกินงวดเกษียณ
                if (loanpayment_type == 1)
                {
                    period_payment += intestimate_amt;
                }
                decimal sumpaymth = paymonth_coop + period_payment + ldc_paymtmetc;//ยอดหักทั้งหมด
                decimal salary_diff = salary_amt + ldc_incomeetc - sumpaymth;//เงินเดือน - ยอดหักทั้งหมด
                decimal total = salary_diff - minsalary_balance;//เงินเดือนคงเหลือ - เงินเดือนคงเหลือขั้นต่ำ
                if ((total < -5))
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("สมาชิกคนนี้เงินเดือนไม่พอหัก กรุณาตรวจสอบ เงินเดือนคงเหลือขั้นต่ำ =" + minsalary_balance.ToString("#,##0.00"));
                    //if (retrysend_flag == 0 && custompay_flag == 0)
                    //{
                    //   // JsCalMaxLoanpermiss();//คำนวณสิทธิ์สูงสุด
                    //    JsLoanpaymenttype();
                    //    JsSumOthClr();
                    //}
                }
            }
            catch (Exception ex) { ex.ToString(); }
        }
        private void JsCheckpauseloan() 
        {
            string loantype_code = dw_main.GetItemString(1, "loantype_code");
            string loantype_pause = "", ls_loantypeclr = "";
            try
            {
                string ls_sql = "select loantype_pause from lnloantypepause where loantype_code = '" + loantype_code + "'";
                Sdt dt = WebUtil.QuerySdt(ls_sql);
                if (dt.Next()) {
                    loantype_pause = dt.GetString("loantype_pause");

                    for (int i = 1; i <= dw_clear.RowCount; i++)
                    {
                        ls_loantypeclr = dw_clear.GetItemString(i, "loantype_code");
                        if (loantype_pause == ls_loantypeclr) {
                            dw_main.SetItemDecimal(1, "loanrequest_amt", 0);
                            dw_main.SetItemDecimal(1, "loancredit_amt", 0);
                            LtServerMessage.Text += Message.ErrorMessage( "สมาชิกท่านนี้มีเงินกูประเภท " + ls_loantypeclr  + " ไม่สามารถกู้ประเภทนี้ได้แล้ว กรุณาตรวจสอบ");
                            return;
                        }

                    }
                }
            }
            catch { 
            
            }
        }

        private void JsCalperiodSend()
        {
            DateTime ldtm_retry = dw_main.GetItemDateTime(1, "retry_date");
            DateTime ldtm_lnreq = dw_main.GetItemDateTime(1, "loanrequest_date");
            DateTime ldtm_birthday = dw_main.GetItemDateTime(1, "birth_date");

            string ls_loantype = dw_main.GetItemString(1, "loantype_code");
            decimal ldc_maxperiod = dw_main.GetItemDecimal(1, "maxsend_payamt");
            decimal ldc_retryperiod = 0, li_retrychkflag = 0, retry_age = 500;
            String sqlStrperiod = @" SELECT retryloansend_time,retryloanchk_type
                                    FROM LNLOANTYPE
                                    WHERE LOANTYPE_CODE='" + ls_loantype + "'";

            Sdt dt1 = WebUtil.QuerySdt(sqlStrperiod);
            if (dt1.Next())
            {
                try
                {
                    ldc_retryperiod = dt1.GetDecimal("retryloansend_time");
                    li_retrychkflag = dt1.GetDecimal("retryloanchk_type");
                }
                catch
                {
                    ldc_retryperiod = 0;
                }
            }
            retry_age = dw_main.GetItemDecimal(1, "retry_age");
            if (li_retrychkflag == 1)
            {
                //ตรวจงวดเกษียณปกติ
               
                if (retry_age < 0) { retry_age = 0; }
                // retry_age = BusscomService.of_cal_yearmonth(state.SsWsPass, ldtm_lnreq, ldtm_retry) * 12;
                if (retry_age < ldc_maxperiod)
                {
                    ldc_maxperiod = retry_age;
                }
            }

            if (li_retrychkflag == 2)
            {
                //ตรวจงวดการส่งถึงแค่อายุ 60
                ldtm_retry = ldtm_birthday.AddYears(60);
               // retry_age = BusscomService.of_cal_yearmonth(state.SsWsPass, ldtm_lnreq, ldtm_retry);

                retry_age = (ldtm_retry.Year - ldtm_lnreq.Year) * 12;
                retry_age += (ldtm_retry.Month - ldtm_lnreq.Month);
                if (ldtm_lnreq.Day > ldtm_retry.Day) { retry_age--; }
              //  decimal age_year = Math.Truncate(retry_age / 12);
              //  decimal age_month = (retry_age % 12) / 100;

               // retry_age = age_year + age_month;

                if (retry_age < ldc_maxperiod)
                {
                    ldc_maxperiod = retry_age;
                }

            }
            if (ldc_retryperiod > 0)
            {

                if (retry_age < ldc_maxperiod)
                {
                    retry_age = retry_age + ldc_retryperiod;
                }
                if (retry_age < ldc_maxperiod)
                {
                    ldc_maxperiod = retry_age;
                }
            }

            dw_main.SetItemDecimal(1, "maxsend_payamt", ldc_maxperiod);
            dw_main.SetItemDecimal(1, "period_payamt", ldc_maxperiod);
        }
        private void JsGetexpensememno()
        {
            try
            {
                string memno = dw_main.GetItemString(1, "member_no");
                string strsql = @"select expense_code, expense_bank, expense_branch, expense_accid 
                                        from mbmembmaster where member_no = '" + memno + "'";
                try
                {
                    Sdt dtloanrcv = WebUtil.QuerySdt(strsql);

                    if (dtloanrcv.GetRowCount() <= 0)
                    {

                        LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบเลขที่บัญชีเงินธนาคารของสมาชิก " + memno);
                    }
                    if (dtloanrcv.Next())
                    {
                        string loanrcv_code = "", loanrcv_bank = "";

                        try { loanrcv_code = dw_main.GetItemString(1, "expense_code"); }
                        catch { loanrcv_code = ""; }
                        if (loanrcv_code == "" || loanrcv_code == null) { loanrcv_code = dtloanrcv.GetString("expense_code"); }

                        try { loanrcv_bank = dw_main.GetItemString(1, "expense_bank"); }
                        catch { loanrcv_bank = ""; }
                        if (loanrcv_bank == "" || loanrcv_bank == null) { loanrcv_bank = dtloanrcv.GetString("expense_bank"); }

                        string loanrcv_branch = dtloanrcv.GetString("expense_branch");
                        string loanrcv_accid = dtloanrcv.GetString("expense_accid");

                        if (loanrcv_code != null)
                        {
                            dw_main.SetItemString(1, "expense_code", loanrcv_code);
                            dw_main.SetItemString(1, "expense_bank", loanrcv_bank);
                            if (loanrcv_branch == "" || loanrcv_branch == null)
                            {
                                DwUtil.RetrieveDDDW(dw_main, "expense_branch_1", "sl_loan_requestment_cen.pbl", loanrcv_bank);
                            }
                            else
                            {
                                dw_main.SetItemString(1, "expense_branch", loanrcv_branch);
                            }
                            dw_main.SetItemString(1, "expense_accid", loanrcv_accid);

                            // DwUtil.RetrieveDDDW(dw_main, "expense_branch", "sl_loan_requestment_cen.pbl", null);
                        }
                        else
                        {
                            dw_main.SetItemString(1, "expense_code", "CBT");
                        }

                        if (loanrcv_code == "CBT" && loanrcv_bank.Length > 2)
                        {
                            string sql_bkk = "select branch_name from cmucfbankbranch where bank_code = '" + loanrcv_bank + "' and branch_id = '" + loanrcv_branch + "'";
                            Sdt dtk = WebUtil.QuerySdt(sql_bkk);
                            string bankbranch = "";
                            if (dtk.Next())
                            {
                                bankbranch = dtk.GetString("branch_name").Trim();
                                //dw_main.SetItemString(1, "bank_branch", bankbranch);
                                //dw_main.SetItemDouble(1, "retrive_bk_branchflag", 0);
                                // dw_main.SetItemString(1, "expense_branch_1", bankbranch);
                                JsExpensebankbrRetrieve();
                            }
                        }
                        if (loanrcv_code == "CBO" && loanrcv_bank.Length > 2)
                        {
                            string sql_bkk = "select branch_name from cmucfbankbranch where bank_code = '" + loanrcv_bank + "' and branch_id = '" + loanrcv_branch + "'";
                            Sdt dtk = WebUtil.QuerySdt(sql_bkk);
                            string bankbranch = "";
                            if (dtk.Next())
                            {
                                bankbranch = dtk.GetString("branch_name").Trim();
                                //dw_main.SetItemString(1, "bank_branch", bankbranch);
                                //dw_main.SetItemDouble(1, "retrive_bk_branchflag", 0);
                                // dw_main.SetItemString(1, "expense_branch_1", bankbranch);
                                JsExpensebankbrRetrieve();
                            }

                        }
                    }
                    //JsExpenseBank();
                }
                catch { }

            }
            catch
            {
            }

        }

        /// <summary>
        /// เช็คแลกกันค้ำ โดยใช้สิทธิ์กู้สูงสุดอกตาราง
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GenpermissCollLoop(object sender, EventArgs e)
        {
            //try
            //{
            //    string as_coopid = state.SsCoopId;
            //    String ls_memcoopid = dw_main.GetItemString(1, "memcoop_id");
            //    String ls_membtypedesc = dw_main.GetItemString(1, "membtype_desc");
            //    String member_no = dw_main.GetItemString(1, "member_no");
            //    DateTime ldtm_member;
            //    bool isChecked = Checkcollloop.Checked;
            //    if (isChecked)
            //    {
            //        try
            //        {
            //            ldtm_member = dw_main.GetItemDateTime(1, "member_date");
            //        }
            //        catch { ldtm_member = state.SsWorkDate; }
            //        ///<หาอายุงานของาสมาชิก>
            //        int memtime = 0;
            //        Decimal member_age = dw_main.GetItemDecimal(1, "member_age");
            //        if (member_age > 1)
            //        {
            //            memtime = Convert.ToInt32(BusscomService.of_cal_yearmonth(state.SsWsPass, ldtm_member, state.SsWorkDate) * 12);

            //        }
            //        else
            //        {

            //            memtime = Convert.ToInt32(member_age * 100);
            //        }
            //        String[] max_creditperiod = new String[5];
            //        Decimal per70 = new Decimal(0.7);
            //        Decimal per90 = new Decimal(0.9);
            //        Decimal loancredit_amt = 0;
            //        Decimal ldc_maxperiod = 0;

            //        string ls_loantype = dw_main.GetItemString(1, "loantype_code");

            //        ///< หาสิทธิ์กู้สูงสุด,งวดสูงสุด>                  
            //        max_creditperiod = shrlonService.CalloanpermissCollLoop(state.SsWsPass, ls_memcoopid, as_coopid, ls_loantype, member_no, memtime);

            //        loancredit_amt = Convert.ToDecimal(max_creditperiod[0]);
            //        ldc_maxperiod = Convert.ToDecimal(max_creditperiod[1]);

            //        dw_main.SetItemDecimal(1, "loancredit_amt", loancredit_amt);
            //        dw_main.SetItemDecimal(1, "maxsend_payamt", ldc_maxperiod);
            //        dw_main.SetItemDecimal(1, "period_payamt", ldc_maxperiod);

            //    }

            //    else { Jsmaxcreditperiod(); }
            //}
            //catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage("GenpermissCollLoop==>" + ex); }

        }

        /// <summary>
        /// สำหรับคำนวณดอกเบี้ยประมาณการสำหรับสัญญาใหม่
        /// </summary>
        /// <returns></returns>   

        private decimal of_calinterestfuture()
        { 
             string ls_continttabcode, ls_coopid;
            string ls_loantype;
            int li_continttype, li_intsteptype, li_introundtype;
            Decimal ldc_apvamt, ldc_inttotal = 0, ldc_prncalint;
            Decimal ldc_contintrate, ldc_intincrease;
            DateTime ldtm_calintfrom, ldtm_calintto, ldtm_rcvfix, ldtm_estimate, adc_fixrate;
            try
            {
                // ข้อกำหนดเรื่องดอกเบี้ย
                li_continttype = Convert.ToInt32(dw_main.GetItemDecimal(1, "int_continttype"));
                ls_continttabcode = dw_main.GetItemString(1, "int_continttabcode");
                ldc_contintrate = dw_main.GetItemDecimal(1, "int_contintrate");
                ldc_intincrease = dw_main.GetItemDecimal(1, "int_contintincrease");
                li_intsteptype = Convert.ToInt32(dw_main.GetItemDecimal(1, "int_intsteptype"));
                ls_coopid = dw_main.GetItemString(1, "coop_id");
                ls_loantype = dw_main.GetItemString(1, "loantype_code");

                ldc_apvamt = dw_main.GetItemDecimal(1, "loanrequest_amt");
                ldc_prncalint = dw_main.GetItemDecimal(1, "loanrequest_amt");
                ldtm_rcvfix = dw_main.GetItemDateTime(1, "loanrcvfix_date");
                li_introundtype = Convert.ToInt16(Hdrouninttype.Value);
                ldtm_estimate = dw_main.GetItemDateTime(1, "startkeep_date");
                
                // ถ้าไม่มียอดต้นที่จะคิด ด/บ เป็น 0
                if (ldc_prncalint <= 0) { return 0; }

                // ถ้าสถานะนี้เป็นไม่คิด ด/บ
                if (li_continttype == 0) { return 0; }
                
                // ถ้าวันที่คิดดอกเบี้ยถึงน้อยกว่าวันที่คิดดอกเบี้ยล่าสุด ด/บ 0
                if (ldtm_estimate <= ldtm_rcvfix) { return 0; }

                // ถ้าไม่ได้ set รูปแบบขั้นดอกเบี้ยไว้ set ให้เป็นจากยอดอนุมัติ
                if (li_intsteptype == null) { li_intsteptype = 1; }

                ldtm_calintfrom = ldtm_rcvfix;
                ldtm_calintto = ldtm_estimate;
                TimeSpan ts = ldtm_calintto - ldtm_calintfrom;
                decimal ldc_days = Convert.ToDecimal(ts.Days.ToString());
                decimal ldc_months = TruncateDecimal(ldc_days / 30, 0);
                decimal ldc_mod = (ldc_days % 30);
                if (ldc_mod > 0 && ldc_mod <= 15) { ldc_months = ldc_months + Convert.ToDecimal(0.5); }
                if (ldc_mod > 15 && ldc_mod <= 30) { ldc_months = ldc_months + Convert.ToDecimal(1.0); }
                decimal ldc_intestpay = 0;
                ldc_contintrate = ldc_contintrate / 100;
                switch (li_continttype.ToString())
                {
                    case "1":	// ตาม rate ที่ระบุ

                        //// อัตราด/บเพิ่มลดพิเศษ
                        //inv_intsrv.of_setintincrease(0);


                        ldc_inttotal = JsRoundMoney(ldc_prncalint * (ldc_contintrate) * (ldc_months) / 12, li_introundtype);
                        //  ldc_inttotal = wcf.NShrlon.of_computeinterest(state.SsWsPass, state.SsCoopControl, ls_continttabcode, ldtm_calintto);
                        //  ldc_inttotal = JsRoundMoney(ldc_inttotal, li_introundtype);
                        break;
                    case "2":	// ตามตารางด/บที่ระบุ

                        //ldc_inttotal = Math.Round(wcf.NShrlon.of_computeinterest2(state.SsWsPass, state.SsCoopControl, ls_continttabcode, ldtm_calintfrom, ldtm_calintto, ldc_prncalint, ldc_apvamt));
                        //ldc_inttotal = wcf.NShrlon.of_roundmoney(state.SsWsPass, ldc_prncalint * (ldc_contintrate) * (Convert.ToDecimal(0.5)) / 12);

                        ldc_intestpay = JsRoundMoney(ldc_prncalint * (ldc_contintrate) * (ldc_months) / 12, li_introundtype);
                        ldc_inttotal = JsRoundMoney(ldc_prncalint * (ldc_contintrate) * (ldc_months) / 12, li_introundtype);
                        break;
                    default:
                        // ldc_inttotal = Math.Round(wcf.NShrlon.of_computeintmonth(state.SsWsPass, state.SsCoopControl, ls_loantype, ldc_contintrate, ldtm_calintfrom, ldtm_calintto, ldc_prncalint));
                        ldc_inttotal = JsRoundMoney(ldc_prncalint * (ldc_contintrate) * (ldc_months) / 12, li_introundtype);
                        break;
                }
                //dw_main.SetItemDecimal(1, "intestimate_amt", ldc_inttotal);
            }
            catch (Exception ex)
            {
                //LtServerMessage.Text = WebUtil.ErrorMessage("of_calintestimatemain==+>" + ex); 
            }

            return ldc_inttotal;
            
        }
        public Decimal of_calintestimatemain()
        {
            string ls_continttabcode, ls_coopid;
            string ls_loantype;
            int li_continttype, li_intsteptype, li_introundtype;
            Decimal ldc_apvamt, ldc_inttotal = 0, ldc_prncalint;
            Decimal ldc_contintrate, ldc_intincrease;
            DateTime ldtm_calintfrom, ldtm_calintto, ldtm_rcvfix, ldtm_estimate, adc_fixrate;
            try
            {
                // ข้อกำหนดเรื่องดอกเบี้ย
                li_continttype = Convert.ToInt32(dw_main.GetItemDecimal(1, "int_continttype"));
                ls_continttabcode = dw_main.GetItemString(1, "int_continttabcode");
                ldc_contintrate = dw_main.GetItemDecimal(1, "int_contintrate");
                ldc_intincrease = dw_main.GetItemDecimal(1, "int_contintincrease");
                li_intsteptype = Convert.ToInt32(dw_main.GetItemDecimal(1, "int_intsteptype"));
                ls_coopid = dw_main.GetItemString(1, "coop_id");
                ls_loantype = dw_main.GetItemString(1, "loantype_code");

                ldc_apvamt = dw_main.GetItemDecimal(1, "loanrequest_amt");
                ldc_prncalint = dw_main.GetItemDecimal(1, "loanrequest_amt");
                ldtm_rcvfix = dw_main.GetItemDateTime(1, "loanrcvfix_date");

                li_introundtype = Convert.ToInt16(Hdrouninttype.Value);
                ldtm_estimate = ldtm_rcvfix.AddDays(30);


                // ถ้าไม่มียอดต้นที่จะคิด ด/บ เป็น 0
                if (ldc_prncalint <= 0) { return 0; }

                // ถ้าสถานะนี้เป็นไม่คิด ด/บ
                if (li_continttype == 0) { return 0; }


                // ถ้าวันที่คิดดอกเบี้ยถึงน้อยกว่าวันที่คิดดอกเบี้ยล่าสุด ด/บ 0
                if (ldtm_estimate <= ldtm_rcvfix) { return 0; }

                // ถ้าไม่ได้ set รูปแบบขั้นดอกเบี้ยไว้ set ให้เป็นจากยอดอนุมัติ
                if (li_intsteptype == null) { li_intsteptype = 1; }

                ldtm_calintfrom = ldtm_rcvfix;
                ldtm_calintto = ldtm_estimate;
                decimal ldc_intestpay = 0;
                ldc_contintrate = ldc_contintrate / 100;
                switch (li_continttype.ToString())
                {
                    case "1":	// ตาม rate ที่ระบุ

                        //// อัตราด/บเพิ่มลดพิเศษ
                        //inv_intsrv.of_setintincrease( 0 );

                        //ldc_inttotal = wcf.NShrlon.of_computeinterest(state.SsWsPass, state.SsCoopControl, ls_continttabcode, ldtm_calintto);
                        // ldc_inttotal = Math.Round(wcf.NShrlon.of_computeintmonth(state.SsWsPass, state.SsCoopControl, ls_loantype, ldc_contintrate, ldtm_calintfrom, ldtm_calintto, ldc_prncalint));
                        //ldc_inttotal = wcf.NShrlon.of_roundmoney(state.SsWsPass, ldc_prncalint * (ldc_contintrate) * (Convert.ToDecimal(0.5)) / 12);
                        ldc_inttotal = JsRoundMoney(ldc_prncalint * (ldc_contintrate) * (Convert.ToDecimal(0.5)) / 12, li_introundtype);
                        break;
                    case "2":	// ตามตารางด/บที่ระบุ

                        // อัตราด/บเพิ่มลดพิเศษ

                        //ldc_inttotal = Math.Round(wcf.NShrlon.of_computeinterest2(state.SsWsPass, state.SsCoopControl, ls_continttabcode, ldtm_calintfrom, ldtm_calintto, ldc_prncalint, ldc_apvamt));
                        //ldc_inttotal = wcf.NShrlon.of_roundmoney(state.SsWsPass, ldc_prncalint * (ldc_contintrate) * (Convert.ToDecimal(0.5)) / 12);
                        ldc_intestpay = JsRoundMoney(ldc_prncalint * (ldc_contintrate) * (Convert.ToDecimal(0.5)) / 12, li_introundtype);
                        ldc_inttotal = JsRoundMoney(ldc_prncalint * (ldc_contintrate) * (Convert.ToDecimal(1.0)) / 12, li_introundtype);
                        break;
                    default:
                       // ldc_inttotal = Math.Round(wcf.NShrlon.of_computeintmonth(state.SsWsPass, state.SsCoopControl, ls_loantype, ldc_contintrate, ldtm_calintfrom, ldtm_calintto, ldc_prncalint));
                        ldc_inttotal = JsRoundMoney(wcf.NShrlon.of_computeintmonth(state.SsWsPass, state.SsCoopControl, ls_loantype, ldc_contintrate, ldtm_calintfrom, ldtm_calintto, ldc_prncalint), li_introundtype);
                        break;
                }
                dw_main.SetItemDecimal(1, "intestimate_amt", ldc_inttotal);
            }
            catch (Exception ex)
            {
                //LtServerMessage.Text = WebUtil.ErrorMessage("of_calintestimatemain==+>" + ex); 
            }

            return ldc_inttotal;

        }


        //private void JsRunProcess()
        //{

        //    // --- Page Arguments
        //    try
        //    {
        //        app = Request["app"].ToString();
        //    }
        //    catch { }
        //    if (app == null || app == "")
        //    {
        //        app = state.SsApplication;
        //    }
        //    try
        //    {
        //        //gid = Request["gid"].ToString();
        //        gid = "LNNORM_DAILY";
        //    }
        //    catch { }
        //    try
        //    {
        //        //rid = Request["rid"].ToString();
        //        rid = "LNNORM_DAILY15";
        //    }
        //    catch { }

        //    String doc_no = dw_main.GetItemString(1, "loanrequest_docno");

        //    if (x == 2)
        //    {
        //        doc_no = reqdoc_no;
        //    }
        //    if (doc_no == null || doc_no == "")
        //    {
        //        return;
        //    }

        //    //String ls_xmlmain = dw_main.Describe("DataWindow.Data.XML");
        //    //String ls_format = "CAT";
        //    //short li_membtime = 0; Decimal ldc_right25 = 0; Decimal ldc_right33 = 0; Decimal ldc_right35 = 0; Decimal ldc_right26 = 0; Decimal ldc_right40 = 0;
        //    //int re = shrlonService.of_print_callpermiss(state.SsWsPass, ls_xmlmain, ls_format, ref li_membtime, ref ldc_right25, ref ldc_right33, ref ldc_right35, ref ldc_right26, ref ldc_right40);
        //    //Decimal li_membtime_ = li_membtime;
        //    //Decimal ldc_right25_ = ldc_right25;
        //    //Decimal ldc_right33_ = ldc_right33;
        //    //Decimal ldc_right35_ = ldc_right35;
        //    //Decimal ldc_right26_ = ldc_right26;
        //    //Decimal ldc_right40_ = ldc_right40;
        //    //string loan26 = dw_main.GetItemString(1, "loantype_code");
        //    //if (loan26 == "26")
        //    //{
        //    //    decimal right26 = dw_main.GetItemDecimal(1, "loancredit_amt");
        //    //    ldc_right26_ = right26;
        //    //    ldc_right33_ = 0;
        //    //    ldc_right35_ = 0;
        //    //    ldc_right25_ = 0;
        //    //}


        //    //แปลง Criteria ให้อยู่ในรูปแบบมาตรฐาน.
        //    ReportHelper lnv_helper = new ReportHelper();
        //    lnv_helper.AddArgument(doc_no, ArgumentType.String);
        //    //lnv_helper.AddArgument(li_membtime_.ToString(), ArgumentType.Number);
        //    //lnv_helper.AddArgument(ldc_right25_.ToString(), ArgumentType.Number);
        //    //lnv_helper.AddArgument(ldc_right33_.ToString(), ArgumentType.Number);
        //    //lnv_helper.AddArgument(ldc_right35_.ToString(), ArgumentType.Number);
        //    //lnv_helper.AddArgument(ldc_right26_.ToString(), ArgumentType.Number);
        //    //lnv_helper.AddArgument(ldc_right40_.ToString(), ArgumentType.Number);

        //    //ชื่อไฟล์ PDF = YYYYMMDDHHMMSS_<GID>_<RID>.PDF

        //    String pdfFileName = DateTime.Now.ToString("yyyyMMddHHmmss", WebUtil.EN);
        //    pdfFileName += "_" + gid + "_" + rid + ".pdf";
        //    pdfFileName = pdfFileName.Trim();

        //    //ส่งให้ ReportService สร้าง PDF ให้ {โดยปกติจะอยู่ใน C:\GCOOP\Saving\PDF\}.
        //    try
        //    {
        //        Saving.WsReport.Report lws_report = wcf.Report;
        //        //String criteriaXML = lnv_helper.PopArgumentsXML();
        //        //this.pdf = lws_report.GetPDFURL(state.SsWsPass) + pdfFileName;
        //        String li_return = lws_report.Run(state.SsWsPass, app, gid, rid, criteriaXML, pdfFileName);
        //        if (li_return == "true")
        //        {
        //            HdOpenIFrame.Value = "True";
        //            HdcheckPdf.Value = "True";

        //        }
        //        else if (li_return != "true")
        //        {
        //            HdcheckPdf.Value = "False";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LtServerMessage.Text = WebUtil.ErrorMessage(ex);
        //        return;
        //    }
        //    Session["pdf"] = pdf;
        //    //PopupReport();


        //}
        //private void JsRunProcessInvoice()
        //{

        //    // --- Page Arguments
        //    try
        //    {
        //        app = Request["app"].ToString();
        //    }
        //    catch { }
        //    if (app == null || app == "")
        //    {
        //        app = state.SsApplication;
        //    }
        //    try
        //    {
        //        //gid = Request["gid"].ToString();
        //        gid = "LNNORM_DAILY";
        //    }
        //    catch { }
        //    try
        //    {
        //        //rid = Request["rid"].ToString();
        //        rid = "LNNORM_DAILY13";
        //    }
        //    catch { }

        //    String doc_no = dw_main.GetItemString(1, "loanrequest_docno");

        //    if (x == 2)
        //    {
        //        doc_no = reqdoc_no;
        //    }

        //    if (doc_no == null || doc_no == "")
        //    {
        //        return;
        //    }




        //    //แปลง Criteria ให้อยู่ในรูปแบบมาตรฐาน.
        //    ReportHelper lnv_helper = new ReportHelper();
        //    lnv_helper.AddArgument(doc_no, ArgumentType.String);

        //    //ชื่อไฟล์ PDF = YYYYMMDDHHMMSS_<GID>_<RID>.PDF

        //    String pdfFileName = DateTime.Now.ToString("yyyyMMddHHmmss", WebUtil.EN);
        //    pdfFileName += "_" + gid + "_" + rid + ".pdf";
        //    pdfFileName = pdfFileName.Trim();

        //    //ส่งให้ ReportService สร้าง PDF ให้ {โดยปกติจะอยู่ใน C:\GCOOP\Saving\PDF\}.
        //    try
        //    {
        //        Saving.WsReport.Report lws_report = wcf.Report;
        //        //String criteriaXML = lnv_helper.PopArgumentsXML();
        //        //this.pdf = lws_report.GetPDFURL(state.SsWsPass) + pdfFileName;
        //        String li_return = lws_report.Run(state.SsWsPass, app, gid, rid, criteriaXML, pdfFileName);
        //        if (li_return == "true")
        //        {
        //            HdOpenIFrame.Value = "True";
        //            HdcheckPdf.Value = "True";

        //        }
        //        else if (li_return != "true")
        //        {
        //            HdcheckPdf.Value = "False";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LtServerMessage.Text = WebUtil.ErrorMessage(ex);
        //        return;
        //    }
        //    Session["pdfinvoice"] = pdf;
        //    //PopupReport();


        //}
        //public void JspPopupReport()
        //{
        //    //เด้ง Popup ออกรายงานเป็น PDF.
        //    //String pop = "Gcoop.OpenPopup('http://localhost/GCOOP/WebService/Report/PDF/20110223093839_shrlonchk_SHRLONCHK001.pdf')";

        //    String pop = "Gcoop.OpenPopup('" + Session["pdf"].ToString() + "')";
        //    ClientScript.RegisterClientScriptBlock(this.GetType(), "DsReport", pop, true);

        //}
        //public void JspPopupReportInvoice()
        //{
        //    //เด้ง Popup ออกรายงานเป็น PDF.
        //    //String pop = "Gcoop.OpenPopup('http://localhost/GCOOP/WebService/Report/PDF/20110223093839_shrlonchk_SHRLONCHK001.pdf')";

        //    String pop = "Gcoop.OpenPopup('" + Session["pdfinvoice"].ToString() + "')";
        //    ClientScript.RegisterClientScriptBlock(this.GetType(), "DsReport", pop, true);

        //}
       
        private void JsSetExpenseDefault(int row, string as_memno)
        {
            string memberNo = dw_main.GetItemString(1, "member_no");
            string ls_loantype = dw_main.GetItemString(1, "loantype_code");
            try
            {
                decimal defaultpaytype = 1;
                String sqlStrdefaultpay = @" SELECT defaultpay_type
                                    FROM LNLOANTYPE
                                    WHERE LOANTYPE_CODE='" + ls_loantype + "'";

                Sdt dt1 = WebUtil.QuerySdt(sqlStrdefaultpay);
                if (dt1.GetRowCount() < 1) { defaultpaytype = 1; }
                if (dt1.Next())
                {
                    try
                    {
                        defaultpaytype = dt1.GetDecimal("defaultpay_type");

                    }
                    catch {
                        defaultpaytype = 1; 
                    }
                }


                if (defaultpaytype == 2)
                {
                    //                    string strsql = @"select loanrcv_code , loanrcv_bank, loanrcv_branch, loanrcv_accid  
                    //                                    from mbmembexpense where member_no = '" + as_memno + "'";
                    string strsql = @"select expense_code, expense_bank, expense_branch, expense_accid 
                                        from mbmembmaster where member_no = '" + as_memno + "'";
                    try
                    {
                        Sdt dtloanrcv = WebUtil.QuerySdt(strsql);

                        if (dtloanrcv.GetRowCount() <= 0)
                        {

                            LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบเลขที่บัญชีเงินธนาคารของสมาชิก " + member_no);
                        }
                        if (dtloanrcv.Next())
                        {
                            try
                            {
                                string loanrcv_code = dtloanrcv.GetString("expense_code");
                                string loanrcv_bank = dtloanrcv.GetString("expense_bank");
                                string loanrcv_branch = dtloanrcv.GetString("expense_branch");
                                string loanrcv_accid = dtloanrcv.GetString("expense_accid");

                                dw_main.SetItemString(1, "expense_code", loanrcv_code);
                                dw_main.SetItemString(1, "expense_bank", loanrcv_bank);
                                dw_main.SetItemString(1, "expense_branch", loanrcv_branch);
                                dw_main.SetItemString(1, "expense_accid", loanrcv_accid);

                                string sql_bkk = "select branch_name from cmucfbankbranch where bank_code = '" + loanrcv_bank + "' and branch_id = '" + loanrcv_branch + "'";
                                Sdt dtk = WebUtil.QuerySdt(sql_bkk);
                                string bankbranch = "";
                                if (dtk.Next())
                                {
                                    bankbranch = dtk.GetString("branch_name").Trim();
                                    dw_main.SetItemString(1, "bank_branch", bankbranch);
                                    dw_main.SetItemDouble(1, "retrive_bk_branchflag", 0);
                                    //dw_main.SetItemString(1, "expense_branch_1", bankbranch);

                                }
                                if (loanrcv_code == null)
                                {
                                    strsql = @"select loanrcv_code , loanrcv_bank, loanrcv_branch, loanrcv_accid  
                                                        from mbmembexpense where member_no = '" + as_memno + "'";

                                    dtloanrcv = WebUtil.QuerySdt(strsql);


                                    if (dtloanrcv.Next())
                                    {

                                        loanrcv_code = dtloanrcv.GetString("loanrcv_code");
                                        loanrcv_bank = dtloanrcv.GetString("loanrcv_bank");
                                        loanrcv_branch = dtloanrcv.GetString("loanrcv_branch");
                                        loanrcv_accid = dtloanrcv.GetString("loanrcv_accid");

                                        dw_main.SetItemString(1, "expense_code", loanrcv_code);
                                        dw_main.SetItemString(1, "expense_bank", loanrcv_bank);
                                        dw_main.SetItemString(1, "expense_branch", loanrcv_branch);
                                        dw_main.SetItemString(1, "expense_accid", loanrcv_accid);

                                        sql_bkk = "select branch_name from cmucfbankbranch where bank_code = '" + loanrcv_bank + "' and branch_id = '" + loanrcv_branch + "'";
                                        dtk = WebUtil.QuerySdt(sql_bkk);
                                        bankbranch = "";
                                        if (dtk.Next())
                                        {
                                            bankbranch = dtk.GetString("branch_name").Trim();
                                            dw_main.SetItemString(1, "bank_branch", bankbranch);
                                            dw_main.SetItemDouble(1, "retrive_bk_branchflag", 0);
                                            //dw_main.SetItemString(1, "expense_branch_1", bankbranch);

                                        }

                                    }
                                }
                            }
                            catch {
                                strsql = @"select loanrcv_code , loanrcv_bank, loanrcv_branch, loanrcv_accid  
                                                        from mbmembexpense where member_no = '" + as_memno + "'";

                                dtloanrcv = WebUtil.QuerySdt(strsql);


                                if (dtloanrcv.Next())
                                {

                                    string loanrcv_code = dtloanrcv.GetString("loanrcv_code");
                                    string loanrcv_bank = dtloanrcv.GetString("loanrcv_bank");
                                    string loanrcv_branch = dtloanrcv.GetString("loanrcv_branch");
                                    string loanrcv_accid = dtloanrcv.GetString("loanrcv_accid");

                                    dw_main.SetItemString(1, "expense_code", loanrcv_code);
                                    dw_main.SetItemString(1, "expense_bank", loanrcv_bank);
                                    dw_main.SetItemString(1, "expense_branch", loanrcv_branch);
                                    dw_main.SetItemString(1, "expense_accid", loanrcv_accid);

                                    string sql_bkk = "select branch_name from cmucfbankbranch where bank_code = '" + loanrcv_bank + "' and branch_id = '" + loanrcv_branch + "'";
                                    Sdt dtk = WebUtil.QuerySdt(sql_bkk);
                                    string bankbranch = "";
                                    if (dtk.Next())
                                    {
                                        bankbranch = dtk.GetString("branch_name").Trim();
                                        dw_main.SetItemString(1, "bank_branch", bankbranch);
                                        dw_main.SetItemDouble(1, "retrive_bk_branchflag", 0);
                                        //dw_main.SetItemString(1, "expense_branch_1", bankbranch);

                                    }
                                }
                            }
                        }
                        else
                        {
                          
                        }
                            //JsExpenseBank();
                    }catch {  }
                      
                }
            }
            catch
            {


            }


            
        }
        private void JsSetDeptnodefault(Int16 row)
        {
            string dept_acc = "";
              try
            {
                string memberNo = dw_main.GetItemString(1, "member_no");
                String strSQL = "";
                strSQL = @"SELECT DPDEPTMASTER.DEPTACCOUNT_NO,   
             DPDEPTMASTER.DEPTCLOSE_STATUS,   
             DPDEPTMASTER.PRNCBAL  
            FROM DPDEPTMASTER,   
                 MBMEMBMASTER,   
                 DPDEPTTYPE  
           WHERE ( DPDEPTMASTER.MEMBER_NO = MBMEMBMASTER.MEMBER_NO ) and 
                 ( DPDEPTMASTER.DEPTTYPE_CODE = DPDEPTTYPE.DEPTTYPE_CODE ) and  
                 ( DPDEPTMASTER.MEMCOOP_ID = DPDEPTTYPE.COOP_ID )  and DPDEPTMASTER.DEPTCLOSE_STATUS=0  and
                 ( DPDEPTMASTER.MEMBER_NO = '" + memberNo + "')  ";
                Sdt dtdept = WebUtil.QuerySdt(strSQL);

                if (dtdept.GetRowCount() <= 0)
                {

                    LtServerMessage.Text = WebUtil.ErrorMessage("สมาชิกท่านนี้ยังไม่มีเลขที่บัญชีเงินฝาก กรูณาเปิดบัญชีเงินฝากด้วย");
                }
                //if (dtdept.Next())
                //{
                //    dept_acc = dtdept.GetString("DEPTACCOUNT_NO");
                //}
                //if (dw_otherclr.RowCount > 0)
                //{
                //    dw_otherclr.SetItemString(othrow, "clrother_desc", dept_acc);
                //}
            }
            catch
            {

            }

        }

        protected void SetAcci_dept()
        {
            string dept_acc = "";
            Int16 othrow = Convert.ToInt16(HdRowNumber.Value);
            string memberNo = dw_main.GetItemString(1, "member_no");
           string ls_sql = "select deptaccount_no from mbmembmaster where member_no = '" + member_no + "'";
            Sdt dt = WebUtil.QuerySdt(ls_sql);

            if (dt.Next())
            {
                try
                {
                    dept_acc = dt.GetString("deptaccount_no");
                }
                catch {
                    dept_acc = "";
                }
            }
            if( dept_acc.Length <= 2 )
            {

                try
                {

                    String strSQL = "";
                    strSQL = @"SELECT DPDEPTMASTER.DEPTACCOUNT_NO,   
                     DPDEPTMASTER.MEMBER_NO,   
                     DPDEPTMASTER.DEPTACCOUNT_NAME,   
                     DPDEPTMASTER.DEPTTYPE_CODE,   
                     MBMEMBMASTER.MEMB_NAME,   
                     MBMEMBMASTER.MEMB_SURNAME,   
                     MBMEMBMASTER.MEMBGROUP_CODE,   
                     DPDEPTMASTER.DEPT_OBJECTIVE,   
                     DPDEPTMASTER.DEPTCLOSE_STATUS,   
                     DPDEPTTYPE.DEPTTYPE_GROUP,   
                     DPDEPTMASTER.PRNCBAL  
                    FROM DPDEPTMASTER,   
                     MBMEMBMASTER,   
                     DPDEPTTYPE  
                    WHERE ( DPDEPTMASTER.MEMBER_NO = MBMEMBMASTER.MEMBER_NO ) and  
                     ( DPDEPTMASTER.DEPTTYPE_CODE = DPDEPTTYPE.DEPTTYPE_CODE ) and  
                     ( DPDEPTMASTER.MEMCOOP_ID = DPDEPTTYPE.COOP_ID )  and DPDEPTMASTER.DEPTCLOSE_STATUS=0  and
                     ( DPDEPTMASTER.MEMBER_NO = '" + memberNo + "')  ORDER BY DPDEPTMASTER.DEPTACCOUNT_NO ASC ";
                    Sdt dtdept = WebUtil.QuerySdt(strSQL);

                    if (dtdept.Next())
                    {
                        int rowCount = dtdept.GetRowCount();
                        
                        for (int x = 0; x < rowCount; x++)
                        {

                            dept_acc = dtdept.Rows[x]["DEPTACCOUNT_NO"].ToString();
                            if (x == 0)
                            {
                                // dw_main.SetItemString(1, "expense_accid", dept_acc);
                            }
                        }
                    }


                }
                catch { dept_acc = ""; }
            }
            dw_otherclr.SetItemString(othrow, "clrother_desc", dept_acc);
        }
       
        protected void ReOtherMain()
        {
            dw_main.SetItemDecimal(1, "otherclr_flag", 0);
            dw_main.SetItemDecimal(1, "otherclr_amt", 0);
        }
        protected void JsSetFixdate()
        {
            //string loanrcvfix_flag = dw_main.GetItemString(1, "loanrcvfix_flag").Trim();
            //if (loanrcvfix_flag == "1")
            //{
            //    string rcvfixdate = dw_main.GetItemString(1,"loanrcvfix_tdate");
            //           rcvfixdate = WebUtil.ConvertDateThaiToEng(dw_main,"loanrcvfix_tdate",rcvfixdate);
            //           dw_main.SetItemDateTime(1, "loanrcvfix_date", Convert.ToDateTime(rcvfixdate));
            //}
            //else
            //{
            //    dw_main.SetItemDateTime(1, "loanrcvfix_date", state.SsWorkDate);

            //}
        }
        protected void CheckRemark(String MemberNo)
        {
            //if (HdShowRemark.Value == "true")
            //{
                String strSQL = "SELECT MBMEMBREMARKSTAT.MEMBER_NO FROM MBMEMBREMARKSTAT WHERE MBMEMBREMARKSTAT.MEMBER_NO = '" + MemberNo + "'";
                Sdt dtdept = WebUtil.QuerySdt(strSQL);
                if (dtdept.Next())
                {
                    //Response.Redirect("\\CEN\\GCOOP\\Saving\\Applications\\shrlon\\dlg\\w_dlg_ln_remarkstatus.aspx?MemberNo =" + MemberNo + "");
                    //Response.Write("<script>window.open('../dlg/w_dlg_ln_remarkstatus.aspx?MemberNo=" + MemberNo + "','_blank')</script>"); 
                    HdCheckRemark.Value = "true";
                    HdShowRemark.Value = "true";
                }
                else
                {
                    HdCheckRemark.Value = "false";
                    HdShowRemark.Value = "false";
                }
            //}

        }
        private void JsCalinsurancepay()  //pom-1 
        {
            string ls_memno = dw_main.GetItemString(1, "member_no");
            string ls_loantype = dw_main.GetItemString(1, "loantype_code");
            string request_date = dw_main.GetItemString(1, "loanrequest_tdate");
            decimal loanrequest_amt = dw_main.GetItemDecimal(1, "loanrequest_amt");
            decimal inscost_amt = 0, suminscontbf_amt = 0, insratepay_amt = 0, loancutbf_amt = 1000000;
            string sql_chkloantype = " select instype_code, inscost_amt from insucfloanforcetype where loantype_code  = '" + ls_loantype + "' and  start_loan <= " + Convert.ToString(loanrequest_amt) + " and end_loan >= " + Convert.ToString(loanrequest_amt);
            Sdt dtinschkloan = WebUtil.QuerySdt(sql_chkloantype);
            if (dtinschkloan.Next())
            {
                string instype_code = dtinschkloan.GetString("instype_code");
                decimal maxinscost_amt = dtinschkloan.GetDecimal("inscost_amt");
                string sql_chkins = "select sum( inscost_blance ) as sumcost_amt from insgroupmaster where insmemb_status = 1 and member_no = " + ls_memno;
                Sdt dtins = WebUtil.QuerySdt(sql_chkins);
                if (dtins.Next())
                {
                    suminscontbf_amt = dtins.GetDecimal("sumcost_amt");

                }
                inscost_amt = loanrequest_amt - loancutbf_amt - suminscontbf_amt;
                if (inscost_amt < 500000) { inscost_amt = 500000; }
                if (inscost_amt >= 500001 && inscost_amt < 1000000) { inscost_amt = 1000000; }
                if (inscost_amt >= 1000001 && inscost_amt < 9900000) { inscost_amt = 1500000; }

                string request_mmdd = request_date.Substring(4, 4) + request_date.Substring(2, 2) + request_date.Substring(0, 2);
                // request_mmdd =    //request_date.Substring(5, 4) +request_mmdd; 

                string sql_insrate = "select inspayment_amt from insurancerate  where instype_code = '" + instype_code + "' and start_ddmm <= '" + request_mmdd + "' and  end_ddmm >= '" + request_mmdd + "'";
                Sdt dtrate = WebUtil.QuerySdt(sql_insrate);

                if (dtrate.Next())
                {
                    insratepay_amt = dtrate.GetDecimal("inspayment_amt");
                }
                else
                {
                    insratepay_amt = 0;
                }

                decimal rowcount = dw_otherclr.RowCount;
                string ls_itemtype = "";
                int krow = 0;
                for (int i = 1; i <= rowcount; i++)
                {
                    ls_itemtype = dw_otherclr.GetItemString(i, "clrothertype_code");
                    if (ls_itemtype.Trim() == instype_code.Trim())
                    {
                        krow = i;


                    }
                }
                if (krow > 0)
                {
                    dw_otherclr.SetItemDecimal(krow, "clrother_amt", insratepay_amt);
                }
                else
                {
                    krow = dw_otherclr.InsertRow(0);
                    dw_otherclr.SetItemDecimal(krow, "clrother_amt", insratepay_amt);
                }
                dw_otherclr.SetItemString(krow, "clrothertype_code", instype_code);
                dw_otherclr.SetItemDecimal(krow, "clrother_amt", insratepay_amt);
                dw_otherclr.SetItemString(krow, "clrother_desc", "ประกันชีวิต");
            }

        }
        private void JsSetmutualcoll()
        {
            //init กองทุนผู้สค้ำ   mumembtype_code  = '01' 
            try
            {
                string ls_memno = dw_main.GetItemString(1, "member_no");
                string ls_loantype = dw_main.GetItemString(1, "loantype_code");
                string request_date = dw_main.GetItemString(1, "loanrequest_tdate");
                double loanrequest_amt = Convert.ToDouble(dw_main.GetItemDecimal(1, "loanrequest_amt"));
                decimal muttotalpay_amt = 0;
                string sql_mutloantype = " select percent_amt, mut_amt, maxmut_amt from mutloantype where muttype_code = '01' and loantype_code = '" + ls_loantype + "'";
                Sdt dtm = WebUtil.QuerySdt(sql_mutloantype);

                if (dtm.Next())
                {
                    double mut_percent = dtm.GetDouble("percent_amt");
                    decimal mut_amt = dtm.GetDecimal("mut_amt");
                    decimal mutmax_amt = dtm.GetDecimal("maxmut_amt");
                    decimal mut_pay = 0;

                    if (mut_percent > 0)
                    {
                        mut_pay = Convert.ToDecimal(mut_percent * Convert.ToDouble(loanrequest_amt));
                    }
                    if (mut_amt > 0)
                    {
                        mut_pay = mut_amt;
                    }
                    if (mut_pay > mutmax_amt) { mut_pay = mutmax_amt; }

                    string sql_mut = " select totalpay_amt from mumembmaster where member_no = '" + ls_memno + "'  and  mumembtype_code = '01' ";
                    Sdt dt_mut = WebUtil.QuerySdt(sql_mut);
                    if (dt_mut.GetRowCount() > 0)
                    {
                        try
                        {
                            muttotalpay_amt = dt_mut.GetDecimal("totalpay_amt");
                        }
                        catch { muttotalpay_amt = 0; }
                    }

                    decimal rowcount = dw_otherclr.RowCount;
                    string ls_itemtype = "";
                    int krow = 0;

                    for (int i = 1; i <= rowcount; i++)
                    {
                        ls_itemtype = dw_otherclr.GetItemString(i, "clrother_amt");
                        if (ls_itemtype == "MUC")
                        {
                            krow = i;

                        }
                    }
                    decimal mutpayment_amtd = Convert.ToDecimal(mut_pay);
                    if (krow <= 0)
                    {
                        krow = dw_otherclr.InsertRow(0);

                    }
                    dw_otherclr.SetItemString(krow, "clrothertype_code", "MUC");
                    dw_otherclr.SetItemDecimal(krow, "clrother_amt", muttotalpay_amt);
                    dw_otherclr.SetItemString(krow, "clrother_desc", "คืนเงินกองทุน");


                    rowcount = dw_otherclr.RowCount;
                    for (int i = 1; i <= rowcount; i++)
                    {
                        ls_itemtype = dw_otherclr.GetItemString(i, "clrother_amt");
                        if (ls_itemtype == "MTC")
                        {
                            krow = i;

                        }
                    }

                    if (krow <= 0)
                    {
                        krow = dw_otherclr.InsertRow(0);

                    }
                    dw_otherclr.SetItemString(krow, "clrothertype_code", "MTC");
                    dw_otherclr.SetItemDecimal(krow, "clrother_amt", mutpayment_amtd);
                    dw_otherclr.SetItemString(krow, "clrother_desc", "กองทุนช่วยเหลือผู้ค้ำ");

                }
            }
            catch { 
            
            }
        }
        private void JsSetmutualStability()
        {
            //init กองทุนเพื่อความมั่นคง   mumembtype_code  = '02' 
            try
            {
                string ls_memno = dw_main.GetItemString(1, "member_no");
                string ls_loantype = dw_main.GetItemString(1, "loantype_code");
                string request_date = dw_main.GetItemString(1, "loanrequest_tdate");
                Decimal loanrequest_amt = dw_main.GetItemDecimal(1, "loanrequest_amt");
                String ls_coop_id = dw_main.GetItemString(1, "coop_id");
                Decimal mutpayment_amt = 0;
                string sql_mutloantype = " select percent_amt, mut_amt, maxmut_amt from mutloantype where muttype_code = '02' and loantype_code = '" + ls_loantype + "'";
                Sdt dtm = WebUtil.QuerySdt(sql_mutloantype);

                if (dtm.Next())
                {

                    double mut_percent = dtm.GetDouble("percent_amt");
                    decimal mut_amt = dtm.GetDecimal("mut_amt");
                    decimal mutmax_amt = dtm.GetDecimal("maxmut_amt");
                    decimal mut_pay = 0;

                    if (mut_percent > 0)
                    {
                        mut_pay = Convert.ToDecimal(mut_percent * Convert.ToDouble(loanrequest_amt));
                    }
                    if (mut_amt > 0)
                    {
                        mut_pay = mut_amt;
                    }

                   

                    string sql_mut = " select totalpay_amt from mumembmaster where member_no = '" + ls_memno + "' and  mumembtype_code = '02' ";
                    Sdt dt_mut = WebUtil.QuerySdt(sql_mut);
                    if (dt_mut.GetRowCount() <= 0)
                    {  //ยังไม่เคยมีการทำกองทุนเพื่อความมั่นคง เก็บ 10000
                        try
                        {
                            string sql_mutrate = " select   mupercen ,min_payamt ,max_payamt from   muucfpayrate where  coop_id = '" + ls_coop_id + "' and    min_loanrequestamt    <=  " + loanrequest_amt + "   and  " + loanrequest_amt + " <  max_loanrequestamt";
                            Sdt dt_mutrate = WebUtil.QuerySdt(sql_mutrate);
                            Decimal mu_percen = 0;
                            Decimal mu_min = 0;
                            Decimal mu_max = 0;
                            if (dt_mutrate.GetRowCount() > 0)
                            {
                                mu_percen = Convert.ToDecimal(dt_mutrate.Rows[0][0].ToString().Trim());  //dt_mutrate.GetDecimal("mupercen");
                                mu_min = Convert.ToDecimal(dt_mutrate.Rows[0][1].ToString().Trim());
                                mu_max = Convert.ToDecimal(dt_mutrate.Rows[0][2].ToString().Trim());
                            }

                            mutpayment_amt = (loanrequest_amt * mu_percen) / 100;

                            if (mutpayment_amt < mu_min)
                            {
                                mutpayment_amt = mu_min;
                            }

                            if (mutpayment_amt > mu_max)
                            {
                                mutpayment_amt = mu_max;
                            }

                        }
                        catch { mutpayment_amt = 0; }
                    }

                    decimal rowcount = dw_otherclr.RowCount;
                    string ls_itemtype = "";
                    int krow = 0;

                    for (int i = 1; i <= rowcount; i++)
                    {
                        ls_itemtype = dw_otherclr.GetItemString(i, "clrother_amt");
                        if (ls_itemtype == "MUT")
                        {
                            krow = i;
                        }
                    }
                    decimal mutpayment_amtd = Convert.ToDecimal(mutpayment_amt);

                    if (krow <= 0)
                    {
                        krow = dw_otherclr.InsertRow(0);
                    }
                    dw_otherclr.SetItemString(krow, "clrothertype_code", "MUT");
                    dw_otherclr.SetItemDecimal(krow, "clrother_amt", mutpayment_amtd);
                    dw_otherclr.SetItemString(krow, "clrother_desc", "กองทุนเพื่อความมั่นคง");
                }
            }
            catch
            { 
            
            }
        }

        public decimal[] Calloanpermisssurin(String wsPass, String as_coopid, String ls_loantype, Decimal ldc_salary, Decimal ldc_shrstkvalue, int li_time, String as_member_no, decimal li_timeage)
        {
            
            Decimal[] max_creditperiod = new Decimal[4];
            Decimal ldc_maxcredit = 0, ldc_percensaraly = 0, ldc_maxloanamt = 0, ldc_percenshare = 0, ldc_maxright = 0, ldc_maxdept = 0, ldc_maxcoll = 0, ldc_maxshk = 0; ;
            String loanright_type = shrlonService.of_getattribloantype(wsPass, ls_loantype, "loanright_type");
            Decimal ldc_share_flag = 0, ldc_deposit_flag = 0, ldc_collamst_flag = 0;
            String sqlright = @"  SELECT COOP_ID,   
                                                 LOANTYPE_CODE,   
                                                 SEQ_NO,   
                                                 SHARE_FLAG,   
                                                 DEPOSIT_FLAG,   
                                                 COLLMAST_FLAG, 
                                                 dividend_flag,   
                                                 MAXLOAN_AMT  
                                            FROM LNLOANTYPERIGHT  
                                            WHERE COOP_ID ='" + as_coopid + @"'    and LOANTYPE_CODE='" + ls_loantype + @"'";
            Sdt dtright = ta.Query(sqlright);
            if (dtright.Next())
            {
                ldc_share_flag = dtright.GetDecimal("SHARE_FLAG");
                ldc_deposit_flag = dtright.GetDecimal("DEPOSIT_FLAG");
                ldc_collamst_flag = dtright.GetDecimal("COLLMAST_FLAG");
            }


            // หา maxcredit
            String sqlStrcredit = @" SELECT COOP_ID,   
                                    LOANTYPE_CODE,   
                                    SEQ_NO,   
                                    STARTSHARE_AMT,   
                                    ENDSHARE_AMT,   
                                    STARTMEMBER_TIME,   
                                    ENDMEMBER_TIME, 
                                    startage_amt,
                                    endage_amt,  
                                    PERCENTSHARE,   
                                    PERCENTSALARY,   
                                    MAXLOAN_AMT,   
                                    STARTSALARY_AMT,   
                                    ENDSALARY_AMT  
                                FROM LNLOANTYPECUSTOM  
                                WHERE  COOP_ID ='" + as_coopid + @"'  
                                    and LOANTYPE_CODE='" + ls_loantype + @"' 
                                    and STARTMEMBER_TIME <=" + li_time + @"
                                    and ENDMEMBER_TIME >" + li_time + @"
                                    and STARTAGE_AMT <=" + li_timeage + @"
                                    and ENDAGE_AMT >" + li_timeage + @" ";
            Sdt dtcredit = ta.Query(sqlStrcredit);
            if (dtcredit.Next())
            {
                try
                {
                    ldc_percenshare = dtcredit.GetDecimal("PERCENTSHARE");
                    if (ldc_percenshare == 100) { ldc_percenshare = 1; }
                }
                catch { ldc_percenshare = 0; }
                ldc_percensaraly = dtcredit.GetDecimal("PERCENTSALARY");
                ldc_maxloanamt = dtcredit.GetDecimal("MAXLOAN_AMT");
            }


            if (loanright_type == "1")
            {

                if (ldc_share_flag == 1) // หุ้น
                {
                    ldc_maxshk += ldc_shrstkvalue;
                }

                else if (ldc_deposit_flag == 1) // เงินฝาก
                {
                    String sqldept = @"   SELECT 
                                   sum(  DPDEPTMASTER.PRNCBAL) as   PRNCBAL
                                FROM DPDEPTMASTER,   
                                     MBMEMBMASTER,   
                                     DPDEPTTYPE  
                               WHERE ( DPDEPTMASTER.MEMBER_NO = MBMEMBMASTER.MEMBER_NO ) and  
                                     ( DPDEPTMASTER.DEPTTYPE_CODE = DPDEPTTYPE.DEPTTYPE_CODE ) and  
                                     ( DPDEPTMASTER.MEMCOOP_ID = DPDEPTTYPE.COOP_ID )  and 
                                     DPDEPTMASTER.DEPTCLOSE_STATUS=0  and 
                                     DPDEPTMASTER.MEMBER_NO ='" + as_member_no + @"'
                                 and DPDEPTMASTER.COOP_ID='" + as_coopid + @"'";
                    Sdt dtdept = ta.Query(sqldept);
                    if (dtdept.Next())
                    {
                        ldc_maxdept += dtdept.GetDecimal("PRNCBAL");
                    }
                }
                else if (ldc_collamst_flag == 1)// หลักทรัพย์
                {
                    String sqlcoll = @"  SELECT DISTINCT LNCOLLMASTER.COLLMAST_NO,   
                                             LNCOLLMASTER.COLLMAST_REFNO,   
                                             LNCOLLMASTER.COLLMASTTYPE_CODE,   
                                             LNCOLLMASTER.COLLMAST_DESC,   
                                             LNCOLLMASTER.MORTGAGE_PRICE,   
                                             LNCOLLMASTER.REDEEM_FLAG,   
                                             LNCOLLMASTMEMCO.MEMCO_NO  
                                        FROM LNCOLLMASTER,   
                                             LNCOLLMASTMEMCO  
                                       WHERE ( LNCOLLMASTER.COLLMAST_NO = LNCOLLMASTMEMCO.COLLMAST_NO ) and  
                                             ( ( LNCOLLMASTMEMCO.MEMCO_NO = '" + as_member_no + @"' ) ) ";
                    Sdt dtcoll = ta.Query(sqlcoll);
                    if (dtcoll.Next())
                    {
                        ldc_maxcoll += dtcoll.GetDecimal("PRNCBAL");
                    }


                }


            }
            else if (loanright_type == "2")
            {

                if (ldc_percenshare > 0)
                {


                    ldc_maxcredit = ldc_shrstkvalue * ldc_percenshare;
                }

                else
                {
                    ldc_maxcredit = ldc_salary * ldc_percensaraly;
                }
                if (ldc_maxcredit > ldc_maxloanamt)
                {
                    ldc_maxcredit = ldc_maxloanamt;
                }
            }
            else if (loanright_type == "3")
            {

                //ดูจากสัญญาหลัก  คือ อ่านตารางกำหนดวงเงินกู้ OD

                DateTime ldtm_expirecont, ldtm_today;
                string ls_contcreditno = "";
                string ls_Sql = @"  select	contcredit_no, loancreditbal_amt, expirecont_date
                                from	lncontcredit          where	(  member_no	 =  '" + as_member_no + @"'  ) and
		                        ( loantype_code	=  '" + ls_loantype + "') and      ( contcredit_status	= 1 )";


                Sdt dtcreditc = ta.Query(ls_Sql);

                if (dtcreditc.GetRowCount() <= 0)
                {
                    ldc_maxloanamt = 0;
                    ls_contcreditno = "";
                }
                if (dtcreditc.Next())
                {
                    try
                    {
                        ldc_maxloanamt = dtcreditc.GetDecimal("loancreditbal_amt");

                    }
                    catch { ldc_maxloanamt = 0; }
                    ldc_maxcredit = ldc_maxloanamt;
                    ldtm_expirecont = dtcredit.GetDate("expirecont_date");
                    ls_contcreditno = dtcredit.GetString("contcredit_no");
                }


            }

            else if (loanright_type == "4")
            {
                //คิดจากเงินเดือนคงเหลือ
                ldc_maxcredit = (ldc_salary * ldc_percensaraly) + (ldc_percenshare * ldc_shrstkvalue);
                if (ldc_maxcredit > ldc_maxloanamt)
                {
                    ldc_maxcredit = ldc_maxloanamt;
                }
            }
            else if (loanright_type == "5")
            {
                //คิดจากยอดหุ้น ยกมา
                double ldc_shrstkbegin = 0, ldc_divrate = 0, ldc_shrbuy = 0;
                string strat_date = " ", end_date = " ";
                String sqlstr = @"   SELECT  SHSHAREMASTER.SHARESTK_AMT,   (SHSHAREMASTER.SHAREBEGIN_AMT * SHSHARETYPE.UNITSHARE_VALUE ) as SHAREBEGIN_AMT,  
                                             SHSHARETYPE.UNITSHARE_VALUE, SHSHARETYPE.dividend_rate,SHSHARETYPE.loandivstart_date,SHSHARETYPE.loandivend_date
                               FROM   SHSHAREMASTER,   
                                     SHSHARETYPE  
                               WHERE     ( SHSHAREMASTER.SHARETYPE_CODE = SHSHARETYPE.SHARETYPE_CODE ) and 
                                     ( SHSHAREMASTER.COOP_ID = SHSHARETYPE.COOP_ID ) and  
                                     ( ( SHSHAREMASTER.member_no = '" + as_member_no + @"' ) AND  
                                     ( shsharemaster.sharetype_code = '01' ) )    and  
                                     SHSHAREMASTER.COOP_ID   ='" + as_coopid + @"' ";

                Sdt dtshare = ta.Query(sqlstr);
                if (dtshare.Next())
                {
                    ldc_shrstkbegin = dtshare.GetDouble("SHAREBEGIN_AMT");
                    ldc_divrate = dtshare.GetDouble("dividend_rate");

                }
                double loancredit_amt = ldc_shrstkbegin * ldc_divrate;
                ldc_maxcredit = Convert.ToDecimal(loancredit_amt);
            }
            // หา maxperiod

            ldc_maxcredit = ldc_maxcredit + ldc_maxshk + ldc_maxcoll + ldc_maxdept;
            if (ldc_maxcredit > ldc_maxloanamt)
            {
                ldc_maxcredit = ldc_maxloanamt;
            }
            String sqlStrperiod = @"  SELECT COOP_ID,   
                                        LOANTYPE_CODE,   
                                        SEQ_NO,   
                                        MONEY_FROM,
                                        MONEY_TO,
                                        MAX_PERIOD
                                    FROM LNLOANTYPEPERIOD
                                    WHERE  COOP_ID ='" + as_coopid + @"'  
                                    and LOANTYPE_CODE='" + ls_loantype + @"' 
                                    and " + ldc_maxcredit + " between MONEY_FROM and  MONEY_TO "; //MONEY_FROM <=" + ldc_maxcredit + " and MONEY_TO >" + ldc_maxcredit + ";

            Sdt dtperiod = ta.Query(sqlStrperiod);
            Decimal ldc_maxperiod = 0;
            if (dtperiod.Next())
            {

                ldc_maxperiod = dtperiod.GetDecimal("MAX_PERIOD");
            }

            max_creditperiod[0] = ldc_maxcredit;
            max_creditperiod[1] = ldc_maxperiod;
            max_creditperiod[2] = ldc_percenshare;
            max_creditperiod[3] = ldc_percensaraly;
            ta.Close();
            ldc_maxcredit = 0;
            return max_creditperiod;
        }

        private decimal TruncateDecimal(decimal value, int precision)
        {
            decimal step = (decimal)Math.Pow(10, precision);
            int tmp = (int)Math.Truncate(step * value);
            return tmp / step;
        }
        private decimal JsRoundMoney( decimal adc_money , int ai_roundtype) 
        {
            try
            {
                decimal tmp = TruncateDecimal(adc_money,2);
                decimal tmp1 = TruncateDecimal(tmp, 0);
                decimal factorvalue = tmp -tmp1;
                double facvalue = Convert.ToDouble( factorvalue);
                decimal ipoint1 = TruncateDecimal(factorvalue, 1);
                decimal ipoint2 = factorvalue - ipoint1;
                double ipoint22 = Convert.ToDouble(ipoint2);
                double tmpround = 0;
                switch (ai_roundtype)
                {
                    case 1 :
                        //ปัดที่ละสลึง
                        tmpround = 0.00;
                        if (facvalue >= 0.01 && facvalue <= 0.25) { tmpround = 0.25; }
                        if (facvalue >= 0.26 && facvalue <= 0.50) { tmpround = 0.50; }
                        if (facvalue >= 0.51 && facvalue <= 0.75) { tmpround = 0.75; }
                        if (facvalue >= 0.76 && facvalue <= 0.99) { tmpround = 1.00; }
                         
                        break;
                    case 2 :
                        //ปัดที่ละ 5 สตางค์
                         tmpround = 0.00;
                         if (ipoint22 == 0.00 ) { return adc_money; }
                         if (ipoint22 == 0.05) { return adc_money; }
                         if (ipoint22 >= 0.01 && ipoint22 <= 0.04) { ipoint22 = 0.05; }
                         if (ipoint22 >= 0.06 && ipoint22 <= 0.09) { ipoint22 = 0.10; }
                         tmpround = Convert.ToDouble(ipoint1 ) +  ipoint22;
                       
                        break;
                    case 3 :
                        //ปัดที่ละ 10 สตางค์

                        if (ipoint22 == 0.00)
                        {
                            return adc_money;
                        }
                        else {
                            ipoint22 = 0.10;
                        }
                        tmpround = Convert.ToDouble(ipoint1) + ipoint22;

                        break;
                        
                    case 4 :
                        //ปัดเต็มบาท

                        if (facvalue > 0.49)
                        {
                            tmpround = 1.00;
                        }
                        else {
                            tmpround = 0.00;
                        }
                        
                        break;

                    case 99 :
                        //ปัดตามตาราง
                        //li_find	= ids_roundfactor.find( "factor_code = '"+is_rdsatangtab+"' and factor_step >= "+string( ldc_facvalue, "0.00" ), 1, ids_roundfactor.rowcount() )
                        //if li_find <= 0 then
                        //    return adc_money
                        //end if

                        //ldc_rdamt	= ids_roundfactor.getitemdecimal( li_find, "round_amt" )
                        tmpround = facvalue;
                        break;

                    default :
                        tmpround = facvalue;
                        break;

                }
                tmp = tmp1 + Convert.ToDecimal( tmpround );
                return tmp;

            }
            catch {
                return adc_money;
            }
            
        
        }
        private void MessageBox(string title, string msg)
        {
            //Page.RegisterClientScriptBlock(Page.GetType(),"mes", "<script language='javascript'>alert('" + msg + "')</script>",true);
            //Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), title, "<script language='javascript'>alert('" + msg + "')</script>", true);
            ClientScript.RegisterStartupScript(this.GetType(), title, "alert('" + msg + "');", true);

        }

    }
}


/// 
/// ประเภทเงินกู้  lnloantype
/// ประเภทสมาชิก lnloanmbtype
/// กู้ฉุกเฉิน           10   salary X 3   max_period = 12 ใช้หุ้นค้ำ
/// กู้สามัญหุ้น/เงินฝากค้ำ  20   กู้หุ้นได้ 100%  ,เงินฝาก 100%    max_period  = 96  <800,000 ||  max_period  = 120  >  800,000 <2,000,000 
/// กู้สามัญบุคคลค้ำ      21   ตามตาราง สิทธิ์กู้  lnloantypecustum  ตามตาราง สิทธิํค้ำ lngrpmanprtpermtdet
///                       max_period  = 96  <800,000 ||  max_period  = 120  >  800,000 <2,000,000 
/// 
/// 
/// 
/// 
