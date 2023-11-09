using System;
using CoreSavingLibrary;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using DataLibrary;
using Sybase.DataWindow;
using System.Web.Services.Protocols;
using System.Threading;

namespace Saving.Applications.app_assist
{
    public partial class w_sheet_as_public_funds_edit : PageWebSheet, WebSheet
    {
        protected String app;
        protected String gid;
        protected String rid;
        protected String pdf;
        protected String postPopupReport;

        private String pbl = "as_public_funds.pbl";
        Sdt dt;

        protected DwThDate tDwMain;
        //protected DwThDate tDwDetail;

        protected String postToFromAccid;
        protected String postDepptacount;
        protected String postRefresh;
        protected String postChangeHeight;
        protected String postRetreiveDwMem;
        protected String postRetrieveDwMain;
        protected String postGetMemberDetail;
        protected String postChangeHost;
        protected String postChangeAge;
        protected String postRetrieveBankBranch;
        protected String postCopyAddress;
        protected String postMemProvince;
        protected String postMemDistrict;
        protected String postMemTambol;
        protected String postMainProvince;
        protected String postMainDistrict;
        protected String postMainTambol;
        protected String postGetMoney;

        private string member_no, assisttype_code, moneytype_code, sqlStr, assist_docno, remark, voucher_no, deptaccount_no, req_tdate, expense_accid, tofrom_accid, card_person;
        private string docNo, add, addr_group, soi, mooban, road, addProv, addDist, tumbolCode, postcode;
        private DateTime dtm;
        private decimal capital_year, req_status, posttovc_flag;
        private decimal ahome_flag = 0, bhome_flag = 0, chome_flag = 0, dhome_flag = 0;
        protected Sta ta;
        private DateTime birth_date, member_date, approve_date, req_date;
        private decimal assist_amt = 0;
        //private String sqlStr, envvalue;
        //private String member_no, assist_docno, membgroup_code;
        //private String married_member, to_system;
        //private String expense_bank, expense_branch, expense_accid, paytype_code, remark_cancel;
        //private String memb_addr, addr_group, soi, mooban, road, tambol, district_code, province_code, postcode, deptaccount_no, province_desc;
        //private Decimal capital_year, salary_amt, assist_amt, req_status, seq_pay, pay_status, rowcount, rowcount2, perpay;
        //private DateTime pay_date, cancel_date = DateTime.Now, center_date;
        //private Nullable<Decimal> cancel_id;

        //private String assisttype_code, cancal_id, expense_type, voucher_no, sumpay, moneytype_code;
        //private Decimal approve_amt, approve_date, paytype_date, posttovc_flag, assistdept_amt;
        //private DateTime req_date;

        //TimeSpan tp;

        #region WebSheet Member
        public void InitJsPostBack()
        {
            postRefresh = WebUtil.JsPostBack(this, "postRefresh");
            postChangeHost = WebUtil.JsPostBack(this, "postChangeHost");
            postChangeHeight = WebUtil.JsPostBack(this, "postChangeHeight");
            postRetreiveDwMem = WebUtil.JsPostBack(this, "postRetreiveDwMem");
            postRetrieveDwMain = WebUtil.JsPostBack(this, "postRetrieveDwMain");
            postChangeAge = WebUtil.JsPostBack(this, "postChangeAge");
            postGetMemberDetail = WebUtil.JsPostBack(this, "postGetMemberDetail");
            postRetrieveBankBranch = WebUtil.JsPostBack(this, "postRetrieveBankBranch");
            postCopyAddress = WebUtil.JsPostBack(this, "postCopyAddress");
            postMemProvince = WebUtil.JsPostBack(this, "postMemProvince");
            postMemDistrict = WebUtil.JsPostBack(this, "postMemDistrict");
            postMemTambol = WebUtil.JsPostBack(this, "postMemTambol");
            postMainProvince = WebUtil.JsPostBack(this, "postMainProvince");
            postMainDistrict = WebUtil.JsPostBack(this, "postMainDistrict");
            postMainTambol = WebUtil.JsPostBack(this, "postMainTambol");
            postGetMoney = WebUtil.JsPostBack(this, "postGetMoney");
            postPopupReport = WebUtil.JsPostBack(this, "postPopupReport");
            postDepptacount = WebUtil.JsPostBack(this, "postDepptacount");
            postToFromAccid = WebUtil.JsPostBack(this, "postToFromAccid");

            tDwMain = new DwThDate(DwMain, this);
            tDwMain.Add("req_date", "req_tdate");
            tDwMain.Add("approve_date", "approve_tdate");
        }

        public void WebSheetLoadBegin()
        {
            //Decimal birth_age = wcf.Busscom.of_cal_yearmonth(state.SsWsPass, DateTime.Now, DateTime.Now.AddMonths(3).AddYears(1));
            this.ConnectSQLCA();
            ta = new Sta(sqlca.ConnectionString);
            //tDwMain.Eng2ThaiAllRow();
            //tDwDetail.Eng2ThaiAllRow();
            //this.ConnectSQLCA();
            //ta = new Sta(sqlca.ConnectionString);
            //dt = new Sdt();

            HdDuplicate.Value = "";
            HdNameDuplicate.Value = "";
            LtAlert.Text = "";
            if (!IsPostBack)
            {
                HdActionStatus.Value = "Insert";
                //NewClear();
                DwMem.InsertRow(0);
                DwMain.InsertRow(0);
                DwMain.SetItemDateTime(1, "req_date", state.SsWorkDate);
                DwMain.SetItemDateTime(1, "approve_date", state.SsWorkDate);
                SetDefaultAccid();
            }
            else
            {
                this.RestoreContextDw(DwMem);
                this.RestoreContextDw(DwMain, tDwMain);
            }



            DwMain.SetItemString(1, "assisttype_code", "20");
            DwUtil.RetrieveDDDW(DwMain, "assisttype_code", "as_capital.pbl", state.SsCoopId);
            DwUtil.RetrieveDDDW(DwMain, "capital_year", "as_capital.pbl", null);
            try { capital_year = DwMain.GetItemDecimal(1, "capital_year"); }
            catch { capital_year = state.SsWorkDate.Year + 543; }
            DwMain.SetItemDecimal(1, "capital_year", capital_year);
            //GetLastDocNo(capital_year);
            //DwMain.SetItemString(1, "assist_docno", assist_docno);
            DwUtil.RetrieveDDDW(DwMain, "subdamagetype_code", "as_public_funds.pbl", null);
            DwUtil.RetrieveDDDW(DwMain, "moneytype_code", "as_public_funds.pbl", null);
            //DwUtil.RetrieveDDDW(DwMain, "expense_bank", "as_capital.pbl", null);
            //DwUtil.RetrieveDDDW(DwMain, "cancel_id", "as_capital.pbl", null);
            //DwUtil.RetrieveDDDW(DwMain, "req_status", "as_capital.pbl", null);
            //DwUtil.RetrieveDDDW(DwMain, "pay_status", "as_capital.pbl", null);
            //DwUtil.RetrieveDDDW(DwMain, "paytype_code", "as_capital.pbl", null);
            tDwMain.Eng2ThaiAllRow();

        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postRetreiveDwMem")
            {
                RetreiveDwMem();
            }
            else if (eventArg == "postRefresh")
            {
                //Refresh();
            }
            else if (eventArg == "postChangeAge")
            {
                ChangeAge();
            }
            else if (eventArg == "postRetrieveDwMain")
            {
                RetrieveDwMain();
            }
            else if (eventArg == "postChangeHost")
            {
                //ChangeHost();
            }
            else if (eventArg == "postRetrieveBankBranch")
            {
                RetrieveBankBranch();
            }
            else if (eventArg == "postChangeHeight")
            {
                //ChangeHeight();
            }
            else if (eventArg == "postGetMemberDetail")
            {
                GetMemberDetail();
            }
            else if (eventArg == "postCopyAddress")
            {
                JsPostCopyAddress();
            }
            else if (eventArg == "postGetMoney")
            {
                Getmoney();
            }
            else if (eventArg == "postMemProvince")
            {
                DwMem.SetItemNull(1, "postcode");
                DwMem.SetItemNull(1, "amphur_code");
                DwMem.SetItemNull(1, "tambol_code");
            }
            else if (eventArg == "postMemDistrict")
            {
                DwMem.SetItemNull(1, "tambol_code");
                try
                {
                    String sqlPostCode = "select * from mbucfdistrict where district_code = '" + DwMem.GetItemString(1, "amphur_code") + "'";
                    Sdt dtPostCode = WebUtil.QuerySdt(sqlPostCode);
                    if (dtPostCode.Next())
                    {
                        DwMem.SetItemString(1, "postcode", dtPostCode.GetString("postcode"));
                    }
                }
                catch { }
            }
            else if (eventArg == "postMemTambol")
            {
                //xxxxx
            }
            else if (eventArg == "postMainProvince")
            {
                DwMain.SetItemNull(1, "postcode");
                DwMain.SetItemNull(1, "district_code");
                DwMain.SetItemNull(1, "tambol_code");
            }
            else if (eventArg == "postMainDistrict")
            {
                DwMain.SetItemNull(1, "tambol_code");
                try
                {
                    String sqlPostCode = "select * from mbucfdistrict where district_code = '" + DwMain.GetItemString(1, "district_code") + "'";
                    Sdt dtPostCode = WebUtil.QuerySdt(sqlPostCode);
                    if (dtPostCode.Next())
                    {
                        DwMain.SetItemString(1, "postcode", dtPostCode.GetString("postcode"));
                    }
                }
                catch { }
            }
            else if (eventArg == "postMainTambol")
            {
                //xxxxx
            }
            else if (eventArg == "postPopupReport")
            {
                PopupReport();
            }
            else if (eventArg == "postDepptacount")
            {
                PostDepptacount();
            }
            else if (eventArg == "postToFromAccid")
            {
                PostToFromAccid();
            }
        }

        public void SaveWebSheet()
        {
            ChangeAge();
            Sta ta2 = new Sta(state.SsConnectionString);
            ta2.Transection();

            Sta ta = new Sta(sqlca.ConnectionString);
            DwMain.SetItemString(1, "coop_id", state.SsCoopId);

            try { deptaccount_no = DwMain.GetItemString(1, "deptaccount_no"); }
            catch { deptaccount_no = ""; }
            deptaccount_no = unformatDeptaccid(deptaccount_no);
            DwMain.SetItemString(1, "deptaccount_no", deptaccount_no);

            //---
            try { addr_group = DwMain.GetItemString(1, "addr_group"); }
            catch { addr_group = ""; }
            try { soi = DwMain.GetItemString(1, "soi"); }
            catch { soi = ""; }
            try { mooban = DwMain.GetItemString(1, "mooban"); }
            catch { mooban = ""; }
            try { road = DwMain.GetItemString(1, "road"); }
            catch { road = ""; }
            try { postcode = DwMain.GetItemString(1, "postcode"); }
            catch { postcode = ""; }
            try { req_date = DwMain.GetItemDateTime(1, "req_date"); }
            catch { req_date = state.SsWorkDate; }
            //---
            try
            {
                decimal reqStatus = 0;
                //docNo = DwUtil.GetString(DwMain, 1, "assist_docno", "");
                try { docNo = DwMain.GetItemString(1, "assist_docno"); }
                catch { docNo = ""; }
                try { reqStatus = DwMain.GetItemDecimal(1, "req_status"); }
                catch { reqStatus = 0; }


                //เพิ่มเช็คการ save โดยไม่ต้องมีการตรวจสอบ
                decimal chksave = 0;
                try { chksave = DwMem.GetItemDecimal(1, "chksave"); }
                catch { chksave = 0; }
                if (chksave == 1)
                {

                    #region ตรวจสอบว่า กรอก ว/ด/ป ประสบภัย?
                    try { dtm = DwMain.GetItemDateTime(1, "approve_date"); }
                    catch
                    {
                        //throw new Exception("กรุณากรอกค่า \"วันทีประสบภัย\"");
                        Response.Write("<script language='javascript'>alert('กรุณากรอกค่า วันทีประสบภัย'\n );</script>");
                        return;
                    }

                    #endregion

                    #region ตรวจสอบว่าคีย์บ้านเลขที่?
                    try { add = DwMain.GetItemString(1, "memb_addr"); }
                    catch
                    {
                        //throw new Exception("กรุณากรอกค่า \"บ้านเลขที่\"");
                        Response.Write("<script language='javascript'>alert('กรุณากรอกค่า บ้านเลขที่'\n );</script>");
                        return;
                    }
                    #endregion

                    #region ตรวจสอบว่าคีย์จังหวัด?
                    try { addProv = DwMain.GetItemString(1, "province_code"); }
                    catch
                    {
                        //throw new Exception("กรุณาเลือก \"จังหวัด\"");
                        Response.Write("<script language='javascript'>alert('กรุณากรอกค่า จังหวัด'\n );</script>");
                        return;
                    }
                    #endregion

                    #region ตรวจสอบว่าคีย์อำเภอ?
                    try { addDist = DwMain.GetItemString(1, "district_code"); }
                    catch
                    {
                        //throw new Exception("กรุณาเลือก \"อำเภอ\"");
                        Response.Write("<script language='javascript'>alert('กรุณากรอกค่า อำเภอ'\n );</script>");
                        return;
                    }
                    #endregion

                    #region ตรวจสอบบ้านเลขที่ซ้ำ?
                    //เช็คบ้านเลขที่ซ้ำ
                    tumbolCode = DwUtil.GetString(DwMain, 1, "tambol_code", "");
                    if (tumbolCode == "")
                    {
                        tumbolCode = " is null ";
                    }
                    else
                    {
                        tumbolCode = " = '" + tumbolCode + "' ";
                    }

                    bool isPlus = false;
                    String sqlAddPlus = @"SELECT * FROM asnreqmaster WHERE memb_addr = '" + DwUtil.GetString(DwMain, 1, "addr_no", "") + "' and tambol_code " + tumbolCode + " and assist_docno like 'AD%' and capital_year = '" + DwMain.GetItemDecimal(1, "capital_year") + "'";
                    Sdt dtAddPlus = WebUtil.QuerySdt(sqlAddPlus);
                    String membPlus = "";
                    while (dtAddPlus.Next())
                    {
                        membPlus += dtAddPlus.GetString("member_no") + " ";
                        isPlus = true;
                    }

                    if (isPlus)
                    {
                        //throw new Exception("พบที่อยู่ซ้ำกันในทะเบียนสมาชิกเลยที่ " + membPlus);
                        Response.Write("<script language='javascript'>alert('พบที่อยู่ซ้ำกันในทะเบียนสมาชิกเลยที่ " + membPlus + "'\n );</script>");
                        return;
                    }
                    #endregion

                }

                try { assist_amt = DwMain.GetItemDecimal(1, "assist_amt"); }
                catch { assist_amt = 0; }

                #region ตรวจสอบ ว่ามี assist_docno อยู่ใน asnreqmaster?
                sqlStr = @"SELECT * FROM asnreqmaster WHERE assist_docno = '" + HfAssistDocNo.Value + "'";
                Sdt dt = ta.Query(sqlStr);
                #endregion

                #region ตรวจสอบ ที่อยู่อาศัย กรณี เพิ่มข้อมูล
                //if (docNo == "" && reqStatus == 8)
                //{
                if (!dt.Next())
                {
                    decimal status_was = 0;// DwMain.GetItemDecimal(1, "");
                    try
                    {
                        status_was = DwMain.GetItemDecimal(1, "status_was");
                        if (status_was == 1)
                        {
                            DwMain.SetItemDecimal(1, "ahome_flag", 1);
                            DwMain.SetItemDecimal(1, "bhome_flag", 0);
                            DwMain.SetItemDecimal(1, "chome_flag", 0);
                            //DwMain.SetItemDecimal(1, "dhome_flag", 0);
                        }
                        else if (status_was == 2)
                        {
                            DwMain.SetItemDecimal(1, "ahome_flag", 0);
                            DwMain.SetItemDecimal(1, "bhome_flag", 1);
                            DwMain.SetItemDecimal(1, "chome_flag", 0);
                            //DwMain.SetItemDecimal(1, "dhome_flag", 0);
                        }
                        else if (status_was == 3)
                        {
                            DwMain.SetItemDecimal(1, "ahome_flag", 0);
                            DwMain.SetItemDecimal(1, "bhome_flag", 0);
                            DwMain.SetItemDecimal(1, "chome_flag", 1);
                            //DwMain.SetItemDecimal(1, "dhome_flag", 0);
                        }
                        else if (status_was == 4)
                        {
                            DwMain.SetItemDecimal(1, "ahome_flag", 0);
                            DwMain.SetItemDecimal(1, "bhome_flag", 0);
                            DwMain.SetItemDecimal(1, "chome_flag", 0);
                            //DwMain.SetItemDecimal(1, "dhome_flag", 1);
                        }
                    }
                    catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }


                    //String lastDocNo = GetLastDocNo(DwMain.GetItemDecimal(1, "capital_year"));
                    //DwMain.SetItemString(1, "assist_docno", lastDocNo);
                }
                #endregion

                #region ตรวจสอบ ที่อยู่อาศัย กรณี แก้ไขข้อมูล
                else
                {
                    //ถ่ายังไม่ได้ลงทะเบียน
                    //if (docNo != "" && reqStatus == 8)
                    //{
                    decimal status_was = 0;// DwMain.GetItemDecimal(1, "");
                    try
                    {
                        status_was = DwMain.GetItemDecimal(1, "status_was");
                        if (status_was == 1)
                        {
                            DwMain.SetItemDecimal(1, "ahome_flag", 1);
                            DwMain.SetItemDecimal(1, "bhome_flag", 0);
                            DwMain.SetItemDecimal(1, "chome_flag", 0);
                            DwMain.SetItemDecimal(1, "dhome_flag", 0);
                        }
                        else if (status_was == 2)
                        {
                            DwMain.SetItemDecimal(1, "ahome_flag", 0);
                            DwMain.SetItemDecimal(1, "bhome_flag", 1);
                            DwMain.SetItemDecimal(1, "chome_flag", 0);
                            DwMain.SetItemDecimal(1, "dhome_flag", 0);
                        }
                        else if (status_was == 3)
                        {
                            DwMain.SetItemDecimal(1, "ahome_flag", 0);
                            DwMain.SetItemDecimal(1, "bhome_flag", 0);
                            DwMain.SetItemDecimal(1, "chome_flag", 1);
                            DwMain.SetItemDecimal(1, "dhome_flag", 0);
                        }
                        else if (status_was == 4)
                        {
                            DwMain.SetItemDecimal(1, "ahome_flag", 0);
                            DwMain.SetItemDecimal(1, "bhome_flag", 0);
                            DwMain.SetItemDecimal(1, "chome_flag", 0);
                            DwMain.SetItemDecimal(1, "dhome_flag", 1);
                        }
                    }
                    catch { }
                }
                #endregion

                try { assisttype_code = DwMain.GetItemString(1, "assisttype_code"); }
                catch (Exception ex) { ex.ToString(); }
                DwMain.SetItemString(1, "assisttype_code", assisttype_code);
                try { member_no = DwMem.GetItemString(1, "member_no"); }
                catch (Exception ex) { ex.ToString(); }
                DwMem.SetItemString(1, "member_no", member_no);
                try { req_status = DwMain.GetItemDecimal(1, "req_status"); }
                catch (Exception ex) { ex.ToString(); }
                DwMain.SetItemDecimal(1, "req_status", req_status);
                try { moneytype_code = DwMain.GetItemString(1, "moneytype_code"); }
                catch { moneytype_code = ""; }
                DwMain.SetItemString(1, "moneytype_code", moneytype_code);

                //#region ตรวจสอบ ว่ามี assist_docno อยู่ใน asnreqmaster?
                //sqlStr = @"SELECT * FROM asnreqmaster WHERE assist_docno = '" + HfAssistDocNo.Value + "' AND approve_status = '1'";
                //dt = ta.Query(sqlStr);
                //#endregion
                //if (!dt.Next())
                //{

                //หลุดจาก if , else
                if ((docNo == "" || docNo == null) && reqStatus == 8)
                {
                    #region เพิ่มข้อมูล Insert ตาราง  asnreqmaster, asnreqcapitaldet, asnslippayout, asnmemsalary && Update ตาราง mbmembmaster, asndoccontrol
                    String lastDocNo = GetLastDocNo(DwMain.GetItemDecimal(1, "capital_year"));
                    DwMain.SetItemString(1, "assist_docno", lastDocNo);
                    try
                    {
                        //DwUtil.UpdateDataWindow(DwMem, pbl, "mbmembmaster");
                        UpdateMbmembmaster();
                    }
                    catch (Exception ex)
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
                        HdActionStatus.Value = "Insert";
                    }
                    try
                    {
                        //DwUtil.InsertDataWindow(DwMain, pbl, "ASNREQMASTER");//error "import string"!
                        InsertDwMain();
                    }
                    catch (Exception ex)
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
                        HdActionStatus.Value = "Insert";
                    }
                    try
                    {
                        Decimal capital_year = 0;
                        string assist_docno = "", subdmgtype_code = "", remark = "";
                        try { capital_year = DwMain.GetItemDecimal(1, "capital_year"); }
                        catch { capital_year = 0; }
                        try { assist_docno = DwMain.GetItemString(1, "assist_docno"); }
                        catch { assist_docno = ""; }
                        try { subdmgtype_code = DwMain.GetItemString(1, "subdamagetype_code"); }
                        catch { subdmgtype_code = ""; }
                        try { remark = DwMain.GetItemString(1, "remark"); }
                        catch { remark = ""; }

                        string damage_type = "";

                        if (subdmgtype_code == "disaster_28")
                        {
                            damage_type = "1";
                        }
                        else if (subdmgtype_code == "disaster_27")
                        {
                            damage_type = "2";
                        }
                        else if (subdmgtype_code == "disaster_2_13")
                        {
                            damage_type = "3";
                        }
                        else if (subdmgtype_code == "disaster_29")
                        {
                            damage_type = "4";
                        }
                        else if (subdmgtype_code == "disaster_30")
                        {
                            damage_type = "5";
                        }

                        string insert_asnreqcapitaldet = @"insert into asnreqcapitaldet (
                                                                            capital_year,
                                                                            assist_docno,
                                                                            DAMAGETYPE_CODE,
                                                                            SUBDAMAGETYPE_CODE,
                                                                            remark
                                                                ) values ('" + capital_year + @"',
                                                                          '" + assist_docno + @"',
                                                                              '" + damage_type + @"',
                                                                '" + subdmgtype_code.Trim() + @"',
                                                                 '" + remark + "')";
                        ta.Exe(insert_asnreqcapitaldet);

                        sqlStr = @"UPDATE  asndoccontrol
                            SET     last_docno   = '" + WebUtil.Right(assist_docno, 6) + @"'
                            WHERE   doc_prefix   = 'AD' and
                                    doc_year = '" + capital_year + "'";
                        ta.Exe(sqlStr);

                    }
                    catch (Exception ex) { ex.ToString(); }
                    try
                    {
                        GetItemDwMain();
                        try { assist_docno = DwMain.GetItemString(1, "assist_docno"); }
                        catch { assist_docno = ""; }
                        decimal status_home = 0;
                        try { status_home = DwMain.GetItemDecimal(1, "status_was"); }
                        catch { status_home = 0; }
                        //asnslippayout
                        sqlStr = "select * from asnucfpaytype where paytype_code = '" + moneytype_code + "'";
                        //moneytype_code = "";
                        //string deptaccount_no;
                        try { deptaccount_no = DwMain.GetItemString(1, "deptaccount_no"); }
                        catch { deptaccount_no = ""; }
                        Sdt dtMoneytypecode = WebUtil.QuerySdt(sqlStr);
                        Sdt dtAmt = null;
                        string work_date = state.SsWorkDate.ToString("MM/dd/yyyy");
                        string subdmgtype_code = "";
                        try { subdmgtype_code = DwMain.GetItemString(1, "subdamagetype_code"); }
                        catch { subdmgtype_code = ""; }
                        if (dtMoneytypecode.Next()) { moneytype_code = dtMoneytypecode.Rows[0]["moneytype_code"].ToString().Trim(); }
                        //
                        decimal age_range = 0;
                        try { age_range = Decimal.Parse(DwMain.GetItemString(1, "age_range")); }
                        catch { age_range = 0; }

                        age_range *= 12;//แปลงปีเป็นเดือน
                        //if (status_home == 1 || status_home == 3)
                        //{
                        sqlStr = @"SELECT * 
                                           FROM asnsenvironmentvar 
                                           WHERE envgroup = 'disaster' 
                                           AND '" + age_range + @"' BETWEEN start_age AND end_age 
                                           AND envcode LIKE '" + subdmgtype_code + @"'";
                        dtAmt = WebUtil.QuerySdt(sqlStr);
                        //                            }
                        //                            else if (status_home == 2)
                        //                            {
                        //                                sqlStr = @"SELECT * 
                        //                                           FROM asnsenvironmentvar 
                        //                                           WHERE envgroup = 'disaster' 
                        //                                           AND '" + age_range + @"' BETWEEN start_age AND end_age 
                        //                                           AND envcode LIKE '" + subdmgtype_code + @"'";
                        //                                dtAmt = WebUtil.QuerySdt(sqlStr);
                        //                            }
                        // insert into asnslippayout
                        for (int i = 0; i < dtAmt.Rows.Count; i++)
                        {
                            try { req_tdate = DwMain.GetItemDateTime(1, "req_date").ToString("dd/MM/yyyy", WebUtil.EN); }
                            catch { req_tdate = (state.SsWorkDate).ToString("dd/MM/yyyy", WebUtil.EN); }
                            int seq_no = i + 1;
                            sqlStr = @"INSERT INTO asnslippayout (
                                                            payoutslip_no,                              member_no,                          slip_date,                              operate_date,
                                                               payout_amt,                            slip_status,                      depaccount_no,                           assisttype_code,
                                                           moneytype_code,                               entry_id,                         entry_date,                                    seq_no,
                                                             capital_year,                          sliptype_code,                       tofrom_accid
                                                         )
                                                  VALUES (
                                                  '" + assist_docno + @"',                   '" + member_no + @"',       to_date('" + req_tdate + @"', 'dd/mm/yyyy'),to_date('" + req_tdate + "', 'dd/mm/yyyy'), '" +
                           dtAmt.Rows[i]["envvalue"].ToString() + @"',                                    '0',          '" + deptaccount_no + @"',                '" + assisttype_code + @"',
                                                '" + moneytype_code + @"',            '" + state.SsUsername + @"',       to_date('" + req_tdate + @"', 'dd/mm/yyyy'),            " + seq_no + @",
                                                  '" + capital_year + @"','" + dtAmt.Rows[i]["system_code"] + @"','" + dtAmt.Rows[i]["tofrom_accid"] + @"'
                                                   )";
                            ta2.Exe(sqlStr);
                        }
                        // insert into asnmemsalary
                        decimal salary_amt;
                        string membgroup_code;
                        DataTable dtMem;
                        sqlStr = "select * from mbmembmaster where member_No = '" + member_no + "'";
                        dtMem = WebUtil.QuerySdt(sqlStr);
                        salary_amt = decimal.Parse(dtMem.Rows[0]["salary_amount"].ToString());
                        membgroup_code = dtMem.Rows[0]["membgroup_code"].ToString();

                        sqlStr = @"INSERT INTO asnmemsalary(capital_year  ,  member_no  ,  assist_docno  ,
                                                    salary_amt     ,  membgroup_code)
                                         VALUES('" + capital_year + "','" + member_no + "','" + assist_docno + @"',
                                                '" + salary_amt + "','" + membgroup_code + @"')";
                        ta2.Exe(sqlStr);
                        ta2.Commit();
                    }
                    catch
                    {
                        ta2.RollBack();
                    }
                    NewClear();
                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลสำเร็จ");
                    #endregion
                }
                //}
                else
                {
                    #region แก้ไขข้อมูล update ตาราง mbmembmaster, asnreqmaster, asnreqcapitaldet, asnslippayout, asnmemsalary
                    DwMain.SetItemDecimal(1, "assist_amt", assist_amt);
                    try
                    {
                        //DwUtil.UpdateDataWindow(DwMem, pbl, "mbmembmaster");
                        UpdateMbmembmaster();
                    }
                    catch (Exception ex)
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
                        HdActionStatus.Value = "Insert";
                    }

                    int apv_status = 8;
               //     int req_status = (int)DwMain.GetItemDecimal(1, "req_status");
                    if (req_status == -1)
                    {
                        apv_status = -1;

                    }
                    else if (req_status == 0)
                    {
                        apv_status = 0;
                    }
                    else if (req_status == 1)
                    {
                        apv_status = 1;
                    }
                    String sql = @"UPDATE  asnreqmaster
                               SET     req_status    = '" + req_status + @"',
                                       approve_status  = '" + apv_status + @"'
                               WHERE   assist_docno  = '" + docNo + "'";
                    ta.Exe(sql);
                    if (req_status == -1 || req_status == -1)
                    {
                        try
                        {
                            sqlStr = @"UPDATE  asnreqmaster
                               SET     req_status    = '" + req_status + @"',
                                       approve_status  = '" + apv_status + @"'
                               WHERE   assist_docno  = '" + docNo + "'";
                            ta.Exe(sqlStr);
                        }
                        catch (Exception ex) { ex.ToString(); }
                    }
                    try
                    {
                        //DwUtil.UpdateDataWindow(DwMain, pbl, "ASNREQMASTER");
                        try { DwUtil.UpdateDataWindow(DwMain, pbl, "asnreqmaster"); }
                        catch (Exception ex) { ex.ToString(); }
                        try
                        {
                            Decimal capital_year = DwMain.GetItemDecimal(1, "capital_year");
                            string assist_docno = DwMain.GetItemString(1, "assist_docno");
                            string subdmgtype_code = DwMain.GetItemString(1, "subdamagetype_code");
                            string update_asnreqcapitaldet = "update asnreqcapitaldet set subdamagetype_code = '" + subdmgtype_code + "' where assist_docno = '" + assist_docno + "' and capital_year = '" + capital_year + "'";
                            ta.Exe(update_asnreqcapitaldet);
                        }
                        catch { }
                    }

                    catch (Exception ex)
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
                        HdActionStatus.Value = "Insert";
                    }
                    //
                    try
                    {
                        GetItemDwMain();
                        assist_docno = DwMain.GetItemString(1, "assist_docno");
                        decimal status_home = DwMain.GetItemDecimal(1, "status_was");
                        //asnslippayout
                        sqlStr = "select * from asnucfpaytype where paytype_code = '" + moneytype_code + "'";
                        moneytype_code = "";
                        //string deptaccount_no;
                        try
                        {
                            deptaccount_no = DwMain.GetItemString(1, "deptaccount_no");
                        }
                        catch
                        {
                            deptaccount_no = "";
                        }
                        Sdt dtMoneytypecode = WebUtil.QuerySdt(sqlStr);
                        Sdt dtAmt = null;
                        string work_date = state.SsWorkDate.ToString("MM/dd/yyyy");
                        if (dtMoneytypecode.Next())
                        {
                            moneytype_code = dtMoneytypecode.Rows[0]["moneytype_code"].ToString().Trim();
                        }

                        decimal age_range = Decimal.Parse(DwMain.GetItemString(1, "age_range"));
                        if (status_home == 1 || status_home == 3)
                        {
                            sqlStr = "select * from asnsenvironmentvar where envgroup = 'disaster' and '" + age_range + "' between start_age and end_age and envcode like 'disaster_1%'";
                            dtAmt = WebUtil.QuerySdt(sqlStr);
                        }
                        else if (status_home == 2)
                        {
                            sqlStr = "select * from asnsenvironmentvar where envgroup = 'disaster' and '" + age_range + "' between start_age and end_age and envcode like 'disaster_2%'";
                            dtAmt = WebUtil.QuerySdt(sqlStr);
                        }
                        // insert into asnslippayout
                        for (int i = 0; i < dtAmt.Rows.Count; i++)
                        {
                            int seq_no = i + 1;
                            sqlStr = @"UPDATE asnslippayout SET slip_date = to_date('" + DwMain.GetItemDateTime(1, "req_date").ToString("dd/MM/yyyy", WebUtil.EN) + "', 'dd/mm/yyyy'),operate_date = to_date('" + DwMain.GetItemDateTime(1, "req_date").ToString("dd/MM/yyyy", WebUtil.EN) + "', 'dd/mm/yyyy'),payout_amt = '" + dtAmt.Rows[i]["envvalue"].ToString() + "',depaccount_no = '" + deptaccount_no + "',moneytype_code = '" + moneytype_code + "' ,entry_id = '" + state.SsUsername + "',entry_date = to_date('" + DwMain.GetItemDateTime(1, "req_date").ToString("dd/MM/yyyy", WebUtil.EN) + "', 'dd/mm/yyyy') where payoutslip_no = '" + assist_docno + "' and capital_year = '" + capital_year + "' and seq_no = " + seq_no + "";
                            ta2.Exe(sqlStr);
                        }
                        decimal salary_amt;
                        string membgroup_code;
                        DataTable dtMem;
                        sqlStr = "select * from mbmembmaster where member_No = '" + member_no + "'";
                        dtMem = WebUtil.QuerySdt(sqlStr);
                        salary_amt = decimal.Parse(dtMem.Rows[0]["salary_amount"].ToString());
                        membgroup_code = dtMem.Rows[0]["membgroup_code"].ToString();

                        sqlStr = @"UPDATE  asnmemsalary
                               SET     salary_amt    = '" + salary_amt + @"'
                               WHERE   assist_docno  = '" + assist_docno + "' and capital_year = '" + capital_year + "'";
                        ta2.Exe(sqlStr);
                        ta2.Commit();
                    }
                    catch
                    {
                        ta2.RollBack();
                    }
                    NewClear();
                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกการแก้ไขข้อมูลสำเร็จ");
                    try
                    {
                        string assistdocno = DwMain.GetItemString(1, "assist_docno");
                        string coop_id = state.SsCoopId;
                        string updatecoopid = "update asnreqmaster set coop_id = '" + coop_id + "' where assist_docno ='" + assistdocno + "'";
                        ta.Exe(updatecoopid);
                    }
                    catch { }

                    #endregion
                }
            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
        }

        public void WebSheetLoadEnd()
        {
            //DwMain.SetItemDateTime(1, "pay_date", state.SsWorkDate);
            //DwMain.SetItemDateTime(1, "req_date", state.SsWorkDate);
            //DwMain.SetItemDateTime(1, "entry_date", state.SsWorkDate);
            //DwMain.SetItemDateTime(1, "approve_date", state.SsWorkDate);
            tDwMain.Eng2ThaiAllRow();
            ////deptaccount_no
            //try
            //{
            //    String membNo = DwUtil.GetString(DwMem, 1, "member_no", "");
            //    if (membNo != "")
            //    {
            //        DwUtil.RetrieveDDDW(DwMain, "deptaccount_no", pbl, membNo);
            //        DataWindowChild dc = DwMain.GetChild("deptaccount_no");
            //        String accNo = DwUtil.GetString(DwMain, 1, "deptaccount_no", "").Trim();
            //        if (dc.RowCount > 0 && accNo == "")
            //        {
            //            DwMain.SetItemString(1, "deptaccount_no", dc.GetItemString(1, "deptaccount_no"));
            //        }
            //    }
            //}
            //catch { }
            // DDDW - Tambol
            try
            {
                String pvCode = DwUtil.GetString(DwMem, 1, "amphur_code", "");
                if (pvCode != "")
                {
                    DwUtil.RetrieveDDDW(DwMem, "tambol_code", pbl, pvCode.Trim());
                }
            }
            catch { }
            // DDDW - District
            try
            {
                String pvCode = DwUtil.GetString(DwMem, 1, "province_code", "");
                if (pvCode != "")
                {
                    DwUtil.RetrieveDDDW(DwMem, "amphur_code", pbl, pvCode.Trim());
                }
            }
            catch { }
            // DDDW - Province
            try
            {
                DwUtil.RetrieveDDDW(DwMem, "province_code", pbl, null);
            }
            catch { }

            try
            {
                DwUtil.RetrieveDDDW(DwMain, "assisttype_code", pbl, state.SsCoopId);
            }
            catch { }
            try
            {
                DwUtil.RetrieveDDDW(DwMain, "capital_year", pbl, null);
            }
            catch { }
            try
            {
                DwUtil.RetrieveDDDW(DwMain, "req_status", pbl, null);
            }
            catch { }
            try
            {
                String pvCode = DwUtil.GetString(DwMain, 1, "district_code", "");
                if (pvCode != "")
                {
                    DwUtil.RetrieveDDDW(DwMain, "tambol_code", pbl, pvCode.Trim());
                }
            }
            catch { }
            try
            {
                String pvCode = DwUtil.GetString(DwMain, 1, "province_code", "");
                if (pvCode != "")
                {
                    DwUtil.RetrieveDDDW(DwMain, "district_code", pbl, pvCode.Trim());
                }
            }
            catch { }
            try
            {
                DwUtil.RetrieveDDDW(DwMain, "province_code", pbl, null);
            }
            catch { }
            try
            {
                DwMem.SaveDataCache();
                DwMain.SaveDataCache();
            }
            catch { }
        }
        #endregion

        #region Functions
        private void GetItemDwMain()
        {
            member_no = DwMem.GetItemString(1, "member_no");
            capital_year = DwMain.GetItemDecimal(1, "capital_year");
            assisttype_code = DwMain.GetItemString(1, "assisttype_code");
            try
            {
                moneytype_code = DwMain.GetItemString(1, "moneytype_code");
            }
            catch { moneytype_code = ""; }

            int[] member_year = new int[3];
            DateTime Req_date;
            Req_date = DwMain.GetItemDateTime(1, "req_date");

            DateTime member_date = DwMem.GetItemDate(1, "member_date");

            member_year = clsCalAge(member_date, Req_date);
            string member_age_range = member_year[2].ToString() + "." + member_year[1].ToString("00") + member_year[0].ToString("00");
            DwMain.SetItemString(1, "age_range", member_age_range);

        }

        private void RetreiveDwMem()
        {
            String member_no;
            //String coop_id;

            member_no = HfMemberNo.Value;
            //coop_id = state.SsCoopId;
            //object[] args = new object[2];
            //args[0] = member_no;
            //args[1] = coop_id;

            //DwMem.Reset();
            //DwUtil.RetrieveDataWindow(DwMem, "as_capital.pbl", null, args);
            DwUtil.RetrieveDataWindow(DwMem, "as_capital.pbl", null, member_no);
            if (DwMem.RowCount < 1)
            {
                LtAlert.Text = "<script>Alert()</script>";
                return;
            }

            GetMemberDetail();
            //GetSCHDetail();
        }

        private void GetMemberDetail()
        {
            String member_no = DwMem.GetItemString(1, "member_no");
            member_no = WebUtil.MemberNoFormat(member_no);

            DwUtil.RetrieveDataWindow(DwMem, pbl, null, member_no);
            capital_year = DwMain.GetItemDecimal(1, "capital_year");
            try
            {
                String reMember_no = DwMem.GetItemString(1, "member_no").Trim();
                if (reMember_no == "") DwMem.Reset();
            }
            catch { DwMem.Reset(); }

            if (DwMem.RowCount > 0)
            {
                String sqlMain = @"select * from ASNREQMASTER where member_no = '" + member_no + "' and assist_docno like 'AD%' and capital_year = '" + DwMain.GetItemDecimal(1, "capital_year") + "'";
                Sdt dtMain = WebUtil.QuerySdt(sqlMain);
                if (dtMain.Next())
                {
                    DwUtil.RetrieveDataWindow(DwMain, pbl, tDwMain, dtMain.GetString("assist_docno"), dtMain.GetInt32("capital_year"));
                    if (DwMain.GetItemDecimal(1, "ahome_flag") == 1)
                    {
                        DwMain.SetItemDecimal(1, "status_was", 1);
                    }
                    else if (DwMain.GetItemDecimal(1, "bhome_flag") == 1)
                    {
                        DwMain.SetItemDecimal(1, "status_was", 2);
                    }
                    else if (DwMain.GetItemDecimal(1, "chome_flag") == 1)
                    {
                        DwMain.SetItemDecimal(1, "status_was", 3);
                    }
                    //LtServerMessage.Text = WebUtil.WarningMessage("เลขทะเบียนสมาชิก " + member_no + " มีใบคำขออยู่ในระบบแล้ว");
                    Response.Write("<script language='javascript'>alert('เลขทะเบียนสมาชิก " + member_no + " มีใบคำขออยู่ในระบบแล้ว'\n );</script>");
                }
                else
                {
                    //DwMain.Reset();
                    //DwMain.InsertRow(0);

                    DwMain.SetItemDateTime(1, "req_date", state.SsWorkDate);//req_tdate
                    DwMain.SetItemString(1, "member_no", member_no);
                    String sql;
                    Sdt dt;

                    //damage_series
                    //sql = "select envvalue from asnsenvironmentvar where envcode = 'damage_series'";
                    //                    sql = @"SELECT *
                    //                            FROM asndoccontrol
                    //                            WHERE year = '" + capital_year + @"'
                    //                            AND doc_prefix = 'AD'
                    //                           ";
                    //                    dt = WebUtil.QuerySdt(sql);
                    //                    if (dt.Next())
                    //                    {
                    //                        decimal posttovc = Convert.ToDecimal(dt.GetString("last_docno"));
                    //                        DwMain.SetItemDecimal(1, "posttovc_flag", posttovc);
                    //                    }

                    //damage_home
                    sql = "select envvalue from asnsenvironmentvar where envcode = 'damage_home'";
                    dt = WebUtil.QuerySdt(sql);
                    if (dt.Next())
                    {
                        DwMain.SetItemDecimal(1, "status_was", dt.GetDecimal("envvalue"));
                    }
                    //age range
                    int[] member_year = new int[3];
                    DateTime member_date = DwMem.GetItemDate(1, "member_date");
                    member_year = clsCalAge(member_date, state.SsWorkDate);
                    string member_age_range = member_year[2].ToString() + "." + member_year[1].ToString("00") + member_year[0].ToString("00");
                    string s_member_year = member_year[2].ToString() + " ปี " + member_year[1].ToString() + " เดือน " + member_year[0].ToString() + " วัน";
                    DwMain.SetItemString(1, "age_range", member_age_range);
                    DwMem.SetItemString(1, "member_years_disp", s_member_year);
                    //LEK เซตค่าให้กับตัวแปร birth_age (นับอายุสมาชิกถึง วันที่ยื่นขอทุน)
                    sqlStr = @" select  ft_calagemth(birth_date , {1}) as birth_age
                                from    mbmembmaster 
                                where   member_no = {0} ";
                    DateTime approve_date = DwMain.GetItemDateTime(1, "approve_date");
                    sqlStr = WebUtil.SQLFormat(sqlStr, member_no, approve_date);
                    dt = WebUtil.QuerySdt(sqlStr);
                    if (dt.Next())
                    {
                        String birth_age = dt.GetString("birth_age") + " ปี";
                        DwMain.SetItemString(1, "birth_age", birth_age);
                    }
                    Getmoney();

                    tDwMain.Eng2ThaiAllRow();
                }
            }
            else
            {
                LtServerMessage.Text = WebUtil.WarningMessage("ไม่พบทะเบียนสมาชิกเลขที่ " + member_no);
                DwMem.Reset();
                DwMem.InsertRow(0);
                DwMain.Reset();
                DwMain.InsertRow(0);
            }
            //try { capital_year = DwMain.GetItemDecimal(1, "capital_year"); }
            //catch { capital_year = state.SsWorkDate.Year + 543; }
            //GetLastDocNo(capital_year);
            //DwMain.SetItemString(1, "assist_docno", assist_docno);
            SetDefaultAccid();

            deptaccount_no = SetdefaultDeptacc(member_no, state.SsCoopId);
            deptaccount_no = formatDeptaccid(deptaccount_no);
            DwMain.SetItemString(1, "deptaccount_no", deptaccount_no);
        }

        private void RetrieveDwMain()
        {
            try
            {
                DwUtil.RetrieveDataWindow(DwMem, "as_public_funds.pbl", null, HfMemNo.Value);
                DwUtil.RetrieveDataWindow(DwMain, "as_public_funds.pbl", tDwMain, HfAssistDocNo.Value.ToString(), Convert.ToDecimal(HfCapitalYear.Value));
                try { deptaccount_no = DwMain.GetItemString(1, "deptaccount_no"); }
                catch { deptaccount_no = ""; }
                deptaccount_no = formatDeptaccid(deptaccount_no);
                DwMain.SetItemString(1, "deptaccount_no", deptaccount_no);

                HdActionStatus.Value = "Update";

                string sql = "select * from asnreqmaster where assist_docno = '" + HfAssistDocNo.Value + "'";
                Sdt dt = WebUtil.QuerySdt(sql);
                if (dt.Rows.Count > 0)
                {
                    //Session["ss_dropdown_scholarship_branch"] = dt.Rows[0]["branch_id"];
                    //ss_dropdown_scholarship_branch = Session["ss_dropdown_scholarship_branch"].ToString();
                    if (dt.Rows[0]["ahome_flag"].ToString() == "1")
                    {
                        ahome_flag = 1;
                        DwMain.SetItemDecimal(1, "status_was", 1);
                    }
                    if (dt.Rows[0]["bhome_flag"].ToString() == "1")
                    {
                        bhome_flag = 1;
                        DwMain.SetItemDecimal(1, "status_was", 2);
                    }
                    if (dt.Rows[0]["chome_flag"].ToString() == "1")
                    {
                        chome_flag = 1;
                        DwMain.SetItemDecimal(1, "status_was", 3);
                    }
                    //if (dt.Rows[0]["dhome_flag"].ToString() == "1")
                    //{
                    //    DwMain.SetItemDecimal(1, "status_was", 4);
                    //}
                    //if (dt.Rows[0]["ehome_flag"].ToString() == "1")
                    //{
                    //    DwMain.SetItemDecimal(1, "status_was", 5);
                    //}
                }
                //age range
                int[] member_year = new int[3];
                DateTime member_date = DwMem.GetItemDate(1, "member_date");
                member_year = clsCalAge(member_date, state.SsWorkDate);
                string member_age_range = member_year[2].ToString() + "." + member_year[1].ToString("00") + member_year[0].ToString("00");
                DwMain.SetItemString(1, "age_range", member_age_range);
            }

            catch { }
        }

        private void RetrieveBankBranch()
        {
            String bank;

            try
            {
                bank = DwMain.GetItemString(1, "expense_bank");
            }
            catch { bank = ""; }

            DataWindowChild DcBankBranch = DwMain.GetChild("expense_branch");
            DcBankBranch.SetTransaction(sqlca);
            DcBankBranch.Retrieve();
            DcBankBranch.SetFilter("bank_code = '" + bank + "'");
            DcBankBranch.Filter();
        }

        private String GetLastDocNo(Decimal capital_year)
        {
            Sdt dt;
            assist_docno = "";
            decimal posttovc = 0;
            //try { capital_year = DwMain.GetItemDecimal(1, "capital_year"); }
            //catch { capital_year = state.SsWorkDate.Year; }
            try
            {
                string sqlStr = @"SELECT last_docno 
                               FROM   asndoccontrol
                               WHERE  doc_prefix = 'AD' and
                                      doc_year = '" + capital_year + "'";
                dt = ta.Query(sqlStr);
                dt.Next();

                try
                {
                    assist_docno = dt.GetString("last_docno");
                    if (assist_docno == "")
                    {
                        assist_docno = "000001";
                    }
                    else if (assist_docno == "000000")
                    {
                        assist_docno = "000001";
                    }
                    else
                    {
                        assist_docno = "000000" + Convert.ToString(Convert.ToInt32(assist_docno) + 1);
                        assist_docno = WebUtil.Right(assist_docno, 6);
                    }
                    posttovc = Convert.ToDecimal(assist_docno);
                    assist_docno = "AD" + assist_docno;
                    assist_docno = assist_docno.Trim();
                    DwMain.SetItemDecimal(1, "posttovc_flag", posttovc);
                    Hdposttovc_flag.Value = Convert.ToString(posttovc);
                }
                catch
                {
                    assist_docno = "AD000001";
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }

            return assist_docno;
        }

        private void JsPostCopyAddress()
        {
            //memb_addr, //addr_group, //soi, //mooban, //road, //tambol, //district_code, //province_code, //postcode
            //1 memb_addr
            try
            {
                DwMain.SetItemString(1, "memb_addr", DwMem.GetItemString(1, "addr_no").Trim());
            }
            catch
            {
                try { DwMain.SetItemNull(1, "memb_addr"); }
                catch { }
            }
            //-----------------------------------------------------------------------------------
            //2 addr_group
            try
            {
                DwMain.SetItemString(1, "addr_group", DwMem.GetItemString(1, "addr_moo").Trim());
            }
            catch
            {
                try { DwMain.SetItemNull(1, "addr_group"); }
                catch { }
            }
            //-----------------------------------------------------------------------------------
            //3 soi
            try
            {
                DwMain.SetItemString(1, "soi", DwMem.GetItemString(1, "addr_soi").Trim());
            }
            catch
            {
                try { DwMain.SetItemNull(1, "soi"); }
                catch { }
            }
            //-----------------------------------------------------------------------------------
            //4 mooban
            try
            {
                DwMain.SetItemString(1, "mooban", DwMem.GetItemString(1, "addr_village").Trim());
            }
            catch
            {
                try { DwMain.SetItemNull(1, "mooban"); }
                catch { }
            }
            //-----------------------------------------------------------------------------------
            //5 road
            try
            {
                DwMain.SetItemString(1, "road", DwMem.GetItemString(1, "addr_road").Trim());
            }
            catch
            {
                try { DwMain.SetItemNull(1, "road"); }
                catch { }
            }
            //-----------------------------------------------------------------------------------
            //6 tambol
            try
            {
                DwMain.SetItemString(1, "tambol_code", DwMem.GetItemString(1, "tambol_code").Trim());
            }
            catch
            {
                try { DwMain.SetItemNull(1, "tambol"); }
                catch { }
            }
            //-----------------------------------------------------------------------------------
            //7 district_code
            try
            {
                DwMain.SetItemString(1, "district_code", DwMem.GetItemString(1, "amphur_code").Trim());
            }
            catch
            {
                try { DwMain.SetItemNull(1, "district_code"); }
                catch { }
            }
            //-----------------------------------------------------------------------------------
            //8 province_code
            try
            {
                DwMain.SetItemString(1, "province_code", DwMem.GetItemString(1, "province_code").Trim());
            }
            catch
            {
                try { DwMain.SetItemNull(1, "province_code"); }
                catch { }
            }
            //-----------------------------------------------------------------------------------
            //9 postcode
            try
            {
                DwMain.SetItemString(1, "postcode", DwMem.GetItemString(1, "addr_postcode").Trim());
            }
            catch
            {
                try { DwMain.SetItemNull(1, "postcode"); }
                catch { }
            }
            //-----------------------------------------------------------------------------------
        }

        private void Getmoney()
        {
            Sdt dt;
            string sqlStr;
            decimal status_home = 0, age_range = 0;
            age_range = decimal.Parse(DwMain.GetItemString(1, "age_range"));
            status_home = DwMain.GetItemDecimal(1, "status_was");
            string subdamagetype_code = "";
            subdamagetype_code = DwMain.GetItemString(1, "subdamagetype_code");
            #region Old Code 20/09/2556
            //if (status_home == 1 || status_home == 3)
            //{

            //    sqlStr = "select * from asnsenvironmentvar where envgroup = 'disaster' and '" + age_range + "' between start_age and end_age and envcode like 'disaster_1%'";
            //    dt = ta.Query(sqlStr);
            //    if (dt.Next())
            //    {
            //        for (int i = 0; i < dt.Rows.Count; i++)
            //        {
            //            assist_amt = assist_amt + Decimal.Parse(dt.Rows[i]["envvalue"].ToString());
            //        }
            //        DwMain.SetItemDecimal(1, "assist_amt", assist_amt);
            //    }
            //}
            //else if (status_home == 2)
            //{
            //    sqlStr = "select * from asnsenvironmentvar where envgroup = 'disaster' and '" + age_range + "' between start_age and end_age and envcode like 'disaster_2%'";
            //    dt = ta.Query(sqlStr);
            //    if (dt.Next())
            //    {
            //        for (int i = 0; i < dt.Rows.Count; i++)
            //        {
            //            assist_amt = assist_amt + Decimal.Parse(dt.Rows[i]["envvalue"].ToString());
            //        }
            //        DwMain.SetItemDecimal(1, "assist_amt", assist_amt);
            //    }
            //}
            #endregion
            age_range = age_range * 12;//แปลงปีเป็นเดือน

            sqlStr = @" select * 
                        from asnsenvironmentvar 
                        where envgroup = 'disaster' 
                        and '" + age_range + @"' between start_age and end_age 
                        and envcode = '" + subdamagetype_code + @"'";
            dt = ta.Query(sqlStr);
            if (dt.Next())
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    assist_amt = assist_amt + Decimal.Parse(dt.Rows[i]["envvalue"].ToString());
                }
                DwMain.SetItemDecimal(1, "assist_amt", assist_amt);
            }
        }

        private void ChangeAge()
        {
            int[] member_year = new int[3];
            int[] checkage_date = new int[3];
            DateTime Req_date;
            try { Req_date = DwMain.GetItemDateTime(1, "req_date"); }
            catch { Req_date = state.SsWorkDate; }
            //Req_date = DateTime.ParseExact(hdate1.Value, "ddMMyyyy", WebUtil.TH);
            DateTime req_tdate = state.SsWorkDate;

            DwMain.SetItemDateTime(1, "req_date", Req_date);

            //DateTime member_date = DwMem.GetItemDate(1, "member_date");

            DateTime approve_date;
            approve_date = DwMain.GetItemDateTime(1, "approve_date");

            member_year = clsCalAge(Req_date, req_tdate);

            string member_age_range = member_year[2].ToString() + "." + member_year[1].ToString("00") + member_year[0].ToString("00");
            DwMain.SetItemString(1, "age_range", member_age_range);

            checkage_date = clsCalAge(Req_date, approve_date);
            string s_child_year = checkage_date[1].ToString() + " เดือน " + checkage_date[0].ToString() + " วัน";
            s_child_year = checkage_date[1].ToString() + "." + checkage_date[0].ToString();
            checkage.Value = s_child_year;
            Getmoney();
        }

        public int[] clsCalAge(DateTime d1, DateTime d2)
        {
            DateTime fromDate;
            DateTime toDate;

            int intyear;
            int intmonth;
            int intday;
            int intCheckedDay = 0;
            int[] CalAge = new int[3];

            if (d1 > d2)
            {
                fromDate = d2;
                toDate = d1;
            }
            else
            {
                fromDate = d1;
                toDate = d2;
            }
            intCheckedDay = 0;
            if (fromDate.Day > toDate.Day)
            {
                intCheckedDay = DateTime.DaysInMonth(fromDate.Year, fromDate.Month);
            }
            if (intCheckedDay != 0)
            {
                intday = (toDate.Day + intCheckedDay) - fromDate.Day;
                intCheckedDay = 1;
            }
            else
            {
                intday = toDate.Day - fromDate.Day;
            }
            if ((fromDate.Month + intCheckedDay) > toDate.Month)
            {
                intmonth = (toDate.Month + 12) - (fromDate.Month + intCheckedDay);
                intCheckedDay = 1;
            }
            else
            {
                intmonth = (toDate.Month) - (fromDate.Month + intCheckedDay);
                intCheckedDay = 0;
            }
            intyear = toDate.Year - (fromDate.Year + intCheckedDay);

            CalAge[0] = intday;
            CalAge[1] = intmonth;
            CalAge[2] = intyear;
            return CalAge;

        }

        private void RunProcess()
        {
            app = state.SsApplication;
            try
            {
                //gid = Request["gid"].ToString();
                gid = "assist_slip";
            }
            catch { }
            try
            {
                //rid = Request["rid"].ToString();
                rid = "ass_slip_master";
            }
            catch { }



            String assist_docno = DwMain.GetItemString(1, "assist_docno");
            decimal capital_year = DwMain.GetItemDecimal(1, "capital_year");
            //แปลง Criteria ให้อยู่ในรูปแบบมาตรฐาน.
            ReportHelper lnv_helper = new ReportHelper();
            lnv_helper.AddArgument(assist_docno, ArgumentType.String);
            lnv_helper.AddArgument(capital_year.ToString(), ArgumentType.Number);

            //ชื่อไฟล์ PDF = YYYYMMDDHHMMSS_<GID>_<RID>.PDF

            String pdfFileName = DateTime.Now.ToString("yyyyMMddHHmmss", WebUtil.EN);
            pdfFileName += "_" + gid + "_" + rid + "_" + state.SsClientIp + ".pdf";
            pdfFileName = pdfFileName.Trim();

            //ส่งให้ ReportService สร้าง PDF ให้ {โดยปกติจะอยู่ใน C:\GCOOP\Saving\PDF\}.
            try
            {
                CoreSavingLibrary.WcfReport.ReportClient lws_report = wcf.Report;
                String criteriaXML = lnv_helper.PopArgumentsXML();
                this.pdf = lws_report.GetPDFURL(state.SsWsPass) + pdfFileName;
                String li_return = lws_report.RunWithID(state.SsWsPass, app, gid, rid, state.SsUsername, criteriaXML, pdfFileName);
                if (li_return == "true")
                {
                    HdOpenIFrame.Value = "True";
                }
                //else if (li_return != "true")
                //{
                //    HdcheckPdf.Value = "False";
                //}
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                return;
            }
            Session["pdf"] = pdf;
            //PopupReport();
        }

        public void PopupReport()
        {
            RunProcess();
            // Thread.Sleep(5000);
            Thread.Sleep(4500);

            //เด้ง Popup ออกรายงานเป็น PDF.
            String pop = "Gcoop.OpenPopup('" + pdf + "')";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "DsReport", pop, true);
        }

        public void InsertDwMain()
        {
            string tumbol_code = "";

            try { tumbol_code = DwMain.GetItemString(1, "tambol_code"); }
            catch { tumbol_code = ""; }
            try { approve_date = DwMain.GetItemDateTime(1, "approve_date"); }
            catch { approve_date = state.SsWorkDate; }
            try { deptaccount_no = DwMain.GetItemString(1, "deptaccount_no"); }
            catch { deptaccount_no = ""; }
            try { ahome_flag = DwMain.GetItemDecimal(1, "ahome_flag"); }
            catch { ahome_flag = 0; }
            try { bhome_flag = DwMain.GetItemDecimal(1, "bhome_flag"); }
            catch { bhome_flag = 0; }
            try { chome_flag = DwMain.GetItemDecimal(1, "chome_flag"); }
            catch { chome_flag = 0; }
            try { tofrom_accid = DwMain.GetItemString(1, "tofrom_accid"); }
            catch { tofrom_accid = ""; }
            try { remark = DwMain.GetItemString(1, "remark"); }
            catch { remark = ""; }

            posttovc_flag = 0; // Convert.ToDecimal(Hdposttovc_flag.Value);
            try
            {
                sqlStr = @"INSERT INTO asnreqmaster(
                           assist_docno,                           capital_year,                            member_no,
                        assisttype_code,                             assist_amt,                           req_status,
                                 remark,                         moneytype_code,                         approve_date,
                          posttovc_flag,                             voucher_no,                             req_date,
                              memb_addr,                             addr_group,                                  soi,
                                 mooban,                                   road,                        district_code,
                          province_code,                               postcode,                          tambol_code,
                                coop_id,                             ahome_flag,                           bhome_flag,
                             chome_flag,                         deptaccount_no,                         tofrom_accid,
                             pay_status,                         approve_status
                        ) 
                        VALUES(
                '" + assist_docno + @"',                '" + capital_year + @"',                 '" + member_no + @"',
             '" + assisttype_code + @"',                  '" + assist_amt + @"',                '" + req_status + @"',
                      '" + remark + @"',              '" + moneytype_code + @"',      to_date('" + approve_date.ToString("dd/MM/yyyy", WebUtil.EN) + @"','dd/mm/yyyy'),
               '" + posttovc_flag + @"',                  '" + voucher_no + @"',          to_date('" + req_date.ToString("dd/MM/yyyy", WebUtil.EN) + @"','dd/mm/yyyy'),
                         '" + add + @"',                  '" + addr_group + @"',                       '" + soi + @"',                      
                      '" + mooban + @"',                        '" + road + @"',                   '" + addDist + @"',
                     '" + addProv + @"',                    '" + postcode + @"',               '" + tumbol_code + @"',
              '" + state.SsCoopId + @"',                    " + ahome_flag + @",                  " + bhome_flag + @",
                   " + chome_flag + @" ,               '" + deptaccount_no + @"',             '" + tofrom_accid + @"',
                   '" + 8 + @"' , " + 8 + @"
                        )";
                ta.Exe(sqlStr);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private void UpdateMbmembmaster()
        {
            String addr_no, addr_moo, addr_soi, addr_village, addr_road
                   , tambol_code, amphur_code, province_code, addr_postcode;

            try { addr_no = DwMem.GetItemString(1, "addr_no"); }
            catch { addr_no = ""; }
            try { addr_moo = DwMem.GetItemString(1, "addr_addr_moo"); }
            catch { addr_moo = ""; }
            try { addr_soi = DwMem.GetItemString(1, "addr_soi"); }
            catch { addr_soi = ""; }
            try { addr_village = DwMem.GetItemString(1, "addr_village"); }
            catch { addr_village = ""; }
            try { addr_road = DwMem.GetItemString(1, "addr_road"); }
            catch { addr_road = ""; }
            try { tambol_code = DwMem.GetItemString(1, "tambol_code"); }
            catch { tambol_code = ""; }
            try { amphur_code = DwMem.GetItemString(1, "amphur_code"); }
            catch { amphur_code = ""; }
            try { province_code = DwMem.GetItemString(1, "province_code"); }
            catch { province_code = ""; }
            try { addr_postcode = DwMem.GetItemString(1, "addr_postcode"); }
            catch { addr_postcode = ""; }

            sqlStr = @" UPDATE mbmembmaster SET addr_no = '" + addr_no + @"'
                                                ,addr_moo = '" + addr_moo + @"'
                                                ,addr_soi = '" + addr_soi + @"'
                                                ,addr_village = '" + addr_village + @"'
                                                ,addr_road = '" + addr_road + @"'
                                                ,tambol_code = '" + tambol_code + @"'
                                                ,amphur_code = '" + amphur_code + @"'
                                                ,province_code = '" + province_code + @"'
                                                ,addr_postcode = '" + addr_postcode + @"'
                        WHERE member_no = '" + member_no + @"'
                                            ";
            ta.Exe(sqlStr);
        }

        private void PostDepptacount()
        {
            deptaccount_no = Hfdeptaccount_no.Value;
            DwMain.SetItemString(1, "deptaccount_no", deptaccount_no);

            //try { member_no = DwMem.GetItemString(1, "member_no"); }
            //catch { member_no = ""; }
            //sqlStr = @" SELECT expense_accid 
            //            FROM mbmembmaster
            //            WHERE member_no = '" + member_no + @"'";
            //dt = ta.Query(sqlStr);

            //if (dt.Next())
            //{
            //   expense_accid = dt.GetString("expense_accid");
            //}
            //DwMain.SetItemString(1, "deptaccount_no", expense_accid);

        }

        private void PostToFromAccid()
        {
            try { moneytype_code = DwMain.GetItemString(1, "moneytype_code"); }
            catch { moneytype_code = ""; }
            DwUtil.RetrieveDDDW(DwMain, "tofrom_accid", "as_public_funds.pbl", moneytype_code);

            if (moneytype_code == "CSH")
            {
                DwMain.SetItemString(1, "tofrom_accid", "11010100");
                DwMain.Describe("t_16.Visible");
                DwMain.Modify("t_16.Visible=false");
                DwMain.Describe("tofrom_accid.Visible");
                DwMain.Modify("tofrom_accid.Visible=false");
                DwMain.Describe("t_10.Visible");
                DwMain.Modify("t_10.Visible=false");
                DwMain.Describe("deptaccount_no.Visible");
                DwMain.Modify("deptaccount_no.Visible=false");
            }
        }

        private void SetDefaultAccid()
        {
            DwMain.SetItemString(1, "moneytype_code", "CBT");
            PostToFromAccid();
            DwMain.SetItemString(1, "tofrom_accid", "11035200");
        }

        private void NewClear()
        {
            DwMem.Reset();
            DwMain.Reset();
            DwMem.InsertRow(0);
            DwMain.InsertRow(0);
            DwMain.SetItemDateTime(1, "req_date", state.SsWorkDate);
            DwMain.SetItemDateTime(1, "approve_date", state.SsWorkDate);
            SetDefaultAccid();

            DwMain.SetItemString(1, "assisttype_code", "20");
            DwUtil.RetrieveDDDW(DwMain, "assisttype_code", "as_capital.pbl", state.SsCoopId);
            DwUtil.RetrieveDDDW(DwMain, "capital_year", "as_capital.pbl", null);
            try { capital_year = DwMain.GetItemDecimal(1, "capital_year"); }
            catch { capital_year = state.SsWorkDate.Year + 543; }
            DwMain.SetItemDecimal(1, "capital_year", capital_year);
            //GetLastDocNo(capital_year);
            //DwMain.SetItemString(1, "assist_docno", assist_docno);
            DwUtil.RetrieveDDDW(DwMain, "subdamagetype_code", "as_public_funds.pbl", null);
            DwUtil.RetrieveDDDW(DwMain, "moneytype_code", "as_public_funds.pbl", null);
            tDwMain.Eng2ThaiAllRow();
        }

        private string formatIDCard(string idcard)
        {
            string[] id = new string[5];
            idcard = idcard.Replace("-", "");
            if (idcard.Length == 13)
            {
                id[0] = idcard.Substring(0, 1);
                id[1] = idcard.Substring(1, 4);
                id[2] = idcard.Substring(5, 5);
                id[3] = idcard.Substring(10, 2);
                id[4] = idcard.Substring(12, 1);

                idcard = id[0] + "-" + id[1] + "-" + id[2] + "-" + id[3] + "-" + id[4];
            }
            return idcard;
        }
        private string unfomatIDCard(string idcard)
        {
            idcard = idcard.Replace("-", "");
            return idcard;
        }

        private string formatDeptaccid(string deptaccid)
        {
            string[] deptid = new string[5];
            deptaccid = deptaccid.Replace("-", "");
            deptaccid = deptaccid.Trim();
            if (deptaccid.Length == 12)
            {
                deptid[0] = deptaccid.Substring(0, 2);
                deptid[1] = deptaccid.Substring(2, 3);
                deptid[2] = deptaccid.Substring(5, 1);
                deptid[3] = deptaccid.Substring(6, 5);
                deptid[4] = deptaccid.Substring(11, 1);

                deptaccid = deptid[0] + "-" + deptid[1] + "-" + deptid[2] + "-" + deptid[3] + "-" + deptid[4];
            }
            return deptaccid;
        }
        private string unformatDeptaccid(string deptaccid)
        {
            deptaccid = deptaccid.Replace("-", "");
            return deptaccid;
        }

        private string SetdefaultDeptacc(string member_no, string coop_id)
        {
            sqlStr = @"SELECT expense_accid FROM mbmembmaster WHERE coop_id = '" + coop_id + @"' AND member_no = '" + member_no + @"'";
            dt = ta.Query(sqlStr);
            if (dt.Next())
            {
                expense_accid = dt.GetString("expense_accid");
            }
            else
            {
                expense_accid = "";
            }

            return expense_accid;
        }
        #endregion
    }
}

