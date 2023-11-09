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
using CoreSavingLibrary.WcfNCommon;
using Sybase.DataWindow;
using DataLibrary;
using System.Web.Services.Protocols;
using Saving.ConstantConfig;
//using CoreSavingLibrary.WcfCommon;

namespace Saving.Applications.app_assist
{
    public partial class w_sheet_as_reqdeposit : PageWebSheet, WebSheet
    {
        private DwThDate tDwMain;
        private DwThDate tDwCheque;
        private n_depositClient depService;
        private n_commonClient cmdService;
        private bool completeCheque;
       
        //string yDate;
        //POSTBACK
        protected String postChangeDeptType;
        protected String postMemberNo;
        protected String postTotalAmt;
        protected String postDeptAccountNo;
        protected String postPost;
        protected String postDwGainAddRow;
        protected String postGainMemberNo;
        protected String postDwGainDelRow;
        protected String postInsertRowCheque;
        protected String postDeleteRowCheque;
        protected String postBankCode;
        protected String postBankBranchCode;
        protected String postSaveNoCheckApv;
        protected String postPostOffice;

        #region WebSheet Members

        public void InitJsPostBack()
        {

            postChangeDeptType = WebUtil.JsPostBack(this, "postChangeDeptType");
            postMemberNo = WebUtil.JsPostBack(this, "postMemberNo");
            postTotalAmt = WebUtil.JsPostBack(this, "postTotalAmt");
            postDeptAccountNo = WebUtil.JsPostBack(this, "postDeptAccountNo");
            postPost = WebUtil.JsPostBack(this, "postPost");
            postDwGainAddRow = WebUtil.JsPostBack(this, "postDwGainAddRow");
            postDwGainDelRow = WebUtil.JsPostBack(this, "postDwGainDelRow");
            postGainMemberNo = WebUtil.JsPostBack(this, "postGainMemberNo");
            postBankCode = WebUtil.JsPostBack(this, "postBankCode");
            postBankBranchCode = WebUtil.JsPostBack(this, "postBankBranchCode");
            postInsertRowCheque = WebUtil.JsPostBack(this, "postInsertRowCheque");
            postDeleteRowCheque = WebUtil.JsPostBack(this, "postDeleteRowCheque");
            postSaveNoCheckApv = WebUtil.JsPostBack(this, "postSaveNoCheckApv");
            postPostOffice = WebUtil.JsPostBack(this, "postPostOffice");
            //------------------------------------------------------------------
            //------------------------------------------------------------------
            tDwMain = new DwThDate(DwMain, this);
            tDwMain.Add("deptopen_date", "deptopen_tdate");
            //------------------------------------------------------------------
            //tDwCheque = new DwThDate(DwCheque, this);
            //tDwCheque.Add("cheque_date", "cheque_tdate");
        }

        public void WebSheetLoadBegin()
        {

            try
            {
                cmdService = wcf.Common;
                depService = wcf.Deposit;
            }
            catch
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถติดต่อ WebService ได้");
            }
            HdIsInsertCheque.Value = "";
            HdSaveAccept.Value = "false";
            completeCheque = true;

            try
            {
                DwUtil.RetrieveDDDW(DwMain, "depttype_select", "kt_50bath.pbl", null);
                DwUtil.RetrieveDDDW(DwMain, "depttype_code_1", "kt_50bath.pbl", null);
                this.JsPostChangeDeptType();
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }

            if (!IsPostBack)
            {
                //DwMain.InsertRow(0);
                //DwDeptMonth.InsertRow(0);
                DwMain.SetItemDateTime(1, "deptopen_date", state.SsWorkDate);
                tDwMain.Eng2ThaiAllRow();
                try
                {
                    HdDayPassCheq.Value = depService.GetDpDeptConstant(state.SsWsPass, "daypasschq");
                }
                catch
                {
                    HdDayPassCheq.Value = "1";
                }
            }
            else
            {

                this.RestoreContextDw(DwMain);
                //this.JsPostChangeDeptType();
                //this.RestoreContextDw(DwGain);
                //this.RestoreContextDw(DwDeptMonth);
                //this.RestoreContextDw(DwCheque);
            }
            int monthIntPayMeth = DwUtil.GetInt(DwMain, 1, "monthintpay_meth");
            if (monthIntPayMeth == 2)
            {
                try
                {
                    DwUtil.RetrieveDDDW(DwMain, "bank_code_1", "kt_50bath.pbl", null);
                    String bankCode = DwUtil.GetString(DwMain, 1, "bank_code");
                    if (!string.IsNullOrEmpty(bankCode))
                    {
                        object[] argBB = new object[1] { bankCode };
                        DwUtil.RetrieveDDDW(DwMain, "bank_branch_1", "kt_50bath.pbl", argBB);
                        DataWindowChild dcBB = DwMain.GetChild("bank_branch_1");
                    }
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                }
            }
            else
            {
                DwMain.SetItemString(1, "bank_code", "");
                DwMain.SetItemString(1, "bank_branch", "");
            }
            LoopCheque();
        }

        public void CheckJsPostBack(string eventArg)
        {

            if (eventArg == "postChangeDeptType")
            {
                JsPostChangeDeptType();
            }
            else if (eventArg == "postMemberNo")
            {
                JsPostMemberNo();
            }
            else if (eventArg == "postTotalAmt")
            {
                JsPostTotalAmt();
            }
            else if (eventArg == "postDeptAccountNo")
            {
                JsPostDeptAccountNo();
            }
            else if (eventArg == "postDwGainAddRow")
            {
                JsPostDwGainAddRow();
            }
            else if (eventArg == "postDwGainDelRow")
            {
                JsPostDwGainDelRow();
            }
            else if (eventArg == "postGainMemberNo")
            {
                JsPostGainMemberNo();
            }
            else if (eventArg == "postInsertRowCheque")
            {
                JsPostInsertRowCheque();
            }
            else if (eventArg == "postDeleteRowCheque")
            {
                JsPostDeleteRowCheque();
            }
            else if (eventArg == "postBankCode")
            {
                JsPostBankCode();
            }
            else if (eventArg == "postBankBranchCode")
            {
                JsPostBankBranchCode();
            }
            else if (eventArg == "postSaveNoCheckApv")
            {
                SaveSheet();
            }
            else if (eventArg == "postPostOffice")
            {
                JsPostPostOffice();
            }
        }

        public void SaveWebSheet()
        {

            if (!completeCheque)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอกเลขที่เช็คให้ครบถ้วน!");
                return;
            }

            String slipXml = DwMain.Describe("datawindow.data.xml");
            String as_apvdoc = "";
           // String[] apv = depService.CheckRightPermissionDep(state.SsWsPass, state.SsUsername, state.SsCoopId, slipXml, 1);
            String[] apv = depService.CheckRightPermissionDep(state.SsWsPass, state.SsUsername, state.SsCoopId, slipXml, slipXml, 1,ref as_apvdoc);
            if (apv[0] == "true")
            {
                SaveKT();
                SaveSheet();
                
            }
            else
            {
                String deptAccountNo = "RequestOPN";
                deptAccountNo = depService.BaseFormatAccountNo(state.SsWsPass, deptAccountNo);
                String memberNo = DwUtil.GetString(DwMain, 1, "member_no", "");
                Decimal netAmt = DwUtil.GetDec(DwMain, 1, "deptreq_sumamt", 0);
                String accName = DwUtil.GetString(DwMain, 1, "deptaccount_name", "");
                String itemType = DwUtil.GetString(DwMain, 1, "recppaytype_code", "");
                String itemTypeDesc = apv[1].Trim();
                itemTypeDesc = itemTypeDesc.Length > 59 ? itemTypeDesc.Substring(0, 59) : itemTypeDesc;
                try
                {
                    //String reportNo = depService.AddApvTask(state.SsWsPass, state.SsUsername, state.SsApplication, state.SsClientIp, itemType, itemTypeDesc, deptAccountNo, memberNo, state.SsWorkDate, netAmt, deptAccountNo, accName, apv[0], "DEP", 1, state.SsCoopId);
                   // HdProcessId.Value = reportNo;
                    HdAvpCode.Value = apv[0];
                    HdItemType.Value = itemType;
                    HdAvpAmt.Value = netAmt.ToString();
                    HdCheckApvAlert.Value = "true";
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
                }
            }
        }

        public void WebSheetLoadEnd()
        {
            String typeCode = DwUtil.GetString(DwMain, 1, "depttype_code");
            if (!string.IsNullOrEmpty(typeCode))
            {

                DwUtil.RetrieveDDDW(DwMain, "deptpassbook_no", "kt_50bath.pbl", null);
                String sql =
                @"select	book_group, book_stmbase
                from		dpdepttype
                where		depttype_code	= '" + typeCode + "'";
                try
                {
                    DataTable dt = WebUtil.Query(sql);
                    if (dt.Rows.Count > 0)
                    {
                        String bookGroup = dt.Rows[0][0].ToString();//.GetString(0);
                        String bookStmBase = dt.Rows[0][1].ToString();//dt.GetString(1);
                        DataWindowChild dcBookNo = DwMain.GetChild("deptpassbook_no");
                        dcBookNo.SetFilter("( branch_id = '" + state.SsCoopId + "' ) and ( book_type = '" + bookStmBase + "' ) and ( book_grp = '" + bookGroup + "' )");
                        dcBookNo.Filter();
                        if (dcBookNo.RowCount > 0)
                        {
                            DwMain.SetItemString(1, "deptpassbook_no", dcBookNo.GetItemString(1, "book_no"));
                        }
                    }
                }
                catch { }
            }
            String memberNo = DwUtil.GetString(DwMain, 1, "member_no");
            try
            {
                DataWindowChild dcRecp = DwMain.GetChild("recppaytype_code");
                dcRecp.SetFilter("GROUP_ITEMTPE='OPN' AND ACTIVE_FLAG=1");
                dcRecp.Filter();
            }
            catch { }
            //if (DwGain.RowCount > 0)
            //{
            //    DwUtil.RetrieveDDDW(DwGain, "prename_code", "kt_50bath.pbl", null);
            //}
            if (!string.IsNullOrEmpty(memberNo) && memberNo != "CIF")
            {
                DwUtil.RetrieveDDDW(DwMain, "acccont_type", "kt_50bath.pbl");
                DwUtil.RetrieveDDDW(DwMain, "recppaytype_code", "kt_50bath.pbl");
                
                try
                {
                    DataWindowChild dcRPT = DwMain.GetChild("recppaytype_code");
                    dcRPT.SetFilter("group_itemtpe='OPN' and active_flag=1");
                    dcRPT.Filter();
                }
                catch { }
                try
                {
                    String membCard = depService.GetCardPerson(state.SsWsPass, memberNo);
                    membCard = depService.ViewCardMemberFormat(state.SsWsPass, membCard);
                    DwMain.Modify("t_member_card.text = '" + membCard + "'");
                }
                catch { }
            }
            else
            {
                DwMain.SetItemString(1, "recppaytype_code", "");
            }
            String recpPayTypeCode = DwUtil.GetString(DwMain, 1, "recppaytype_code");
            String cashType = "";
            if (!string.IsNullOrEmpty(recpPayTypeCode))
            {
                try
                {
                    String tfAccId = DwUtil.GetString(DwMain, 1, "tofrom_accid");
                    cashType = depService.GetCashType(state.SsWsPass, recpPayTypeCode);
                    DwUtil.RetrieveDDDW(DwMain, "tofrom_accid", "kt_50bath.pbl", null);
                    DataWindowChild dcRP = DwMain.GetChild("tofrom_accid");
                    dcRP.SetFilter("cash_type='" + cashType + "'");
                    dcRP.Filter();
                    if (cashType == "CSH" && string.IsNullOrEmpty(tfAccId))
                    {
                        String tfAccNo = dcRP.GetItemString(1, "account_id");
                        DwMain.SetItemString(1, "tofrom_accid", tfAccNo);
                    }
                }
                catch (Exception) { }
            }
            else
            {
                DwMain.SetItemString(1, "tofrom_accid", "");
            }
            if (cashType == "CHQ")
            {
                DwMain.Modify("deptrequest_amt.Protect=1");
                DwMain.Modify("request_tranamt.Protect=1");
                //if (DwCheque.RowCount <= 0)
                //{
                //    JsPostInsertRowCheque();
                //}
            }
            else
            {
                //int i = 0;
                //while (DwCheque.RowCount > 0)
                //{
                //    DwCheque.DeleteRow(0);
                //    if (i++ > 500) break;
                //}
            }
            String dueDate = HdDueDate.Value;
            if (dueDate != "" && dueDate != null && dueDate != "01/01/2443")
            {
                DwMain.Modify("t_duedate.text= '" + dueDate + "'");
            }
            else
            {
                DwMain.Modify("t_duedate.text= 'ไม่มีกำหนด'");
            }

            DwMain.SaveDataCache();
            //DwGain.SaveDataCache();
            //DwDeptMonth.SaveDataCache();
            //DwCheque.SaveDataCache();
        }

        #endregion

        private void NewClear()
        {
            DwMain.Reset();
            DwMain.InsertRow(0);
            //DwDeptMonth.Reset();
            //DwDeptMonth.InsertRow(0);
            DwMain.SetItemDateTime(1, "deptopen_date", state.SsWorkDate);
            tDwMain.Eng2ThaiAllRow();
            //DwGain.Reset();
            //DwCheque.Reset();
        }

        private void SaveSheet()
        {
            try
            {
                //String reqChequeXml = DwCheque.RowCount > 0 ? DwCheque.Describe("DataWindow.Data.XML") : "";
                //String reqCoDeptXml = DwGain.RowCount > 0 ? DwGain.Describe("DataWindow.Data.XML") : "";
                
                short period = 0;
                String reqMainXml = DwMain.Describe("datawindow.data.xml");
                //DwMain.SaveAs("C:\\dwMain.xml", Sybase.DataWindow.FileSaveAsType.Xml);
                //String accountNo = depService.OpenAccount(state.SsWsPass, reqMainXml, reqChequeXml, reqCoDeptXml, period);
                String accountNo = "";
                String slip_no = "";
                String as_apvdoc = "";
                int result = depService.of_openaccount(state.SsWsPass, reqMainXml, "", "", period, ref accountNo, ref slip_no,ref as_apvdoc);
                HdAccoutNo.Value = accountNo;
                try
                {
                    HdPassBookNo.Value = DwUtil.GetString(DwMain, 1, "deptpassbook_no");
                }
                catch (Exception)
                {
                    HdPassBookNo.Value = "";
                }
                NewClear();
                LtServerMessage.Text = WebUtil.CompleteMessage("ทำการเปิดบัญชีเรียบร้อยแล้ว ได้เลขที่บัญชีใหม่คือ " + depService.ViewAccountNoFormat(state.SsWsPass, accountNo));
                HdSaveAccept.Value = "true";
            }
            catch (SoapException ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(WebUtil.SoapMessage(ex));
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
            }
            this.JsPostChangeDeptType();
        }

        private String TryDwMainGetString(String column)
        {
            try
            {
                return DwMain.GetItemString(1, column).Trim();
            }
            catch
            {
                return "";
            }
        }

        //JS-POSTBACK
        private void JsPostChangeDeptType()
        {
            try
            {
                //String deptType = DwUtil.GetString(DwMain, 1, "depttype_select");
                String deptType = "10";
                String xml = depService.of_init_openslip(state.SsWsPass, deptType, "01", state.SsCoopId, state.SsWorkDate, state.SsUsername, state.SsClientIp);
                DwMain.Reset();
                DwMain.ImportString(xml, Sybase.DataWindow.FileSaveAsType.Xml);
                DwMain.SetItemDateTime(1, "deptopen_date", state.SsWorkDate);
                tDwMain.Eng2ThaiAllRow();
                DateTime dueDate = depService.of_getduedate(state.SsWsPass, deptType, state.SsWorkDate);
                String tDueDate = dueDate.ToString("dd/MM/yyyy", WebUtil.TH);
                //yDate = dueDate.Year.ToString("yyyy",WebUtil.TH);
                HdDueDate.Value = tDueDate;
                //String sql = " select deptgroup_code as deptGroup from dpdepttype where depttype_code = '" + deptType + "'";
                //DataTable dtDept = WebUtil.Query(sql);
                //if (dtDept.Rows.Count > 0)
                //{
                //    deptGrp = dtDept.Rows[0]["deptGroup"] != null ? dtDept.Rows[0]["deptGroup"].ToString().Trim() : "";
                //    if (deptGrp == "01" || deptGrp == "03")
                //    {
                //        DwMain.Modify("upint_time.Protect = 0");
                //        DwMain.Modify("upint_time.Background.Color = '16777215'");
                //        //DwMain.SetProperty("upint_time.Background.Color", "RGB(255, 255, 255)");
                //    }
                //    else
                //    {
                //        DwMain.Modify("upint_time.Protect = 1");
                //        DwMain.Modify("upint_time.Background.Color = '15132390'");
                //    }
                //}

            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        //JS-POSTBACK
        private void JsPostMemberNo()
        {
            String deptType = DwUtil.GetString(DwMain, 1, "depttype_select");
            DwMain.SetItemString(1, "entry_id", state.SsUsername);
            DwMain.SetItemString(1, "recppaytype_code", "OCA");
            DwMain.SetItemDateTime(1, "entry_date", state.SsWorkDate);
            DwMain.SetItemString(1, "member_branchid", state.SsCoopId);
            DwMain.SetItemString(1, "machine_id", state.SsClientIp);
            short memberFlag = Convert.ToInt16(DwMain.GetItemDecimal(1, "member_flag"));
            //String membNo = cmdService.BaseFormatMemberNo(state.SsWsPass, TryDwMainGetString("member_no"));
            string membNo = int.Parse(DwMain.GetItemString(1, "member_no")).ToString("00000000");
            try
            {
                String membName = depService.of_get_namemember(state.SsWsPass, membNo, state.SsCoopId, memberFlag);
                DwMain.SetItemString(1, "member_no", membNo);
                DwMain.SetItemString(1, "deptaccount_name", membName);
                DwMain.SetItemString(1, "condforwithdraw", membName + " แต่เพียงผู้เดียว");
                String sql = " select deptgroup_code as deptGroup from dpdepttype where depttype_code = '" + deptType + "'";
                DataTable dtDept = WebUtil.Query(sql);
                if (dtDept.Rows.Count > 0)
                {
                    String deptGrp = dtDept.Rows[0]["deptGroup"] != null ? dtDept.Rows[0]["deptGroup"].ToString().Trim() : "";
                    if (membNo != "" && membNo != "CIF" && (deptGrp == "01" || deptGrp == "03"))
                    {
                        DwMain.Modify("upint_time.Protect = 0");
                        DwMain.Modify("upint_time.Background.Color = '16777215'");
                    }
                    else
                    {
                        DwMain.Modify("upint_time.Protect = 1");
                        DwMain.Modify("upint_time.Background.Color = '15132390'");
                    }
                }
            }
            catch (Exception)
            {
                DwMain.SetItemString(1, "member_no", "CIF");
                LtServerMessage.Text = WebUtil.WarningMessage("ไม่พบเลขที่สมาชิก " + membNo);
            }
            //String sql = "select member_no from mbmembmaster where member_no='" + membNo + "'";
            //DataTable dt = WebUtil.Query(sql);
            //if (dt.Rows.Count > 0)
            //{
            //String membNameFull = depService.GetNewAccountNames(state.SsWsPass, membNo);
            //String membCardPerson = depService.GetCardPerson(state.SsWsPass, membNo);
            //    DwMain.SetItemString(1, "member_no", membNo);
            //    DwMain.SetItemString(1, "deptaccount_name", membNameFull);
            //    DwMain.SetItemString(1, "condforwithdraw", membNameFull + " แต่เพียงผู้เดียว");
            //}
            //else
            //{
            //    DwMain.SetItemString(1, "member_no", "CIF");
            //    LtServerMessage.Text = WebUtil.WarningMessage("ไม่พบเลขที่สมาชิก " + membNo);
            //}
        }

        //JS-POSTBACK
        private void JsPostDwGainAddRow()
        {
            //DwGain.InsertRow(0);
            //SetDwGainSeq();
        }

        //JS-POSTBACK
        private void JsPostDwGainDelRow()
        {
            try
            {
                int row = int.Parse(HdDwGainCurrentRow.Value);
                //DwGain.DeleteRow(row);
                //SetDwGainSeq();
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        //JS-POSTBACK
        private void JsPostGainMemberNo()
        {
            try
            {
//                int row = int.Parse(HdDwGainCurrentRow.Value);
//                //String memNo = DwGain.GetItemString(row, "ref_id");
//                memNo = cmdService.BaseFormatMemberNo(state.SsWsPass, memNo);
//                String sql = @" 
//                SELECT
//                    memb_name as NAME,
//                    member_no as REF_ID,
//                    memb_surname as SURNAME,
//                    prename_code as PRENAME_CODE,
//                    memb_addr as HOUSE_NO,   
//                    addr_group as GROUP_NO,
//                    soi as SOI,
//                    tambol_code as TUMBOL,
//                    district_code as DISTRICT,
//                    province_code as PROVINCE,
//                    mem_tel as PHONE_NO,
//                    postcode as POST_CODE,
//                    road as ROAD,
//                    '{0}' as COOPBRANCH_ID
//                FROM MBMEMBMASTER
//                WHERE MEMBER_NO = '{1}'";
//                sql = String.Format(sql, state.SsCoopId, memNo);
//                DataTable dt = WebUtil.Query(sql);
//                if (dt.Rows.Count > 0)
//                {
//                    String NAME = dt.Rows[0]["NAME"] != null ? dt.Rows[0]["NAME"].ToString().Trim() : "";
//                    String REF_ID = dt.Rows[0]["REF_ID"] != null ? dt.Rows[0]["REF_ID"].ToString().Trim() : "";
//                    String SURNAME = dt.Rows[0]["SURNAME"] != null ? dt.Rows[0]["SURNAME"].ToString().Trim() : "";
//                    String PRENAME_CODE = dt.Rows[0]["PRENAME_CODE"] != null ? dt.Rows[0]["PRENAME_CODE"].ToString().Trim() : "";
//                    String HOUSE_NO = dt.Rows[0]["HOUSE_NO"] != null ? dt.Rows[0]["HOUSE_NO"].ToString().Trim() : "";
//                    String GROUP_NO = dt.Rows[0]["GROUP_NO"] != null ? dt.Rows[0]["GROUP_NO"].ToString().Trim() : "";
//                    String SOI = dt.Rows[0]["SOI"] != null ? dt.Rows[0]["SOI"].ToString().Trim() : "";
//                    String TUMBOL = dt.Rows[0]["TUMBOL"] != null ? dt.Rows[0]["TUMBOL"].ToString().Trim() : "";
//                    String DISTRICT = dt.Rows[0]["DISTRICT"] != null ? dt.Rows[0]["DISTRICT"].ToString().Trim() : "";
//                    String PROVINCE = dt.Rows[0]["PROVINCE"] != null ? dt.Rows[0]["PROVINCE"].ToString().Trim() : "";
//                    String PHONE_NO = dt.Rows[0]["PHONE_NO"] != null ? dt.Rows[0]["PHONE_NO"].ToString().Trim() : "";
//                    String POST_CODE = dt.Rows[0]["POST_CODE"] != null ? dt.Rows[0]["POST_CODE"].ToString().Trim() : "";
//                    String ROAD = dt.Rows[0]["ROAD"] != null ? dt.Rows[0]["ROAD"].ToString().Trim() : "";
//                    String COOPBRANCH_ID = dt.Rows[0]["COOPBRANCH_ID"] != null ? dt.Rows[0]["COOPBRANCH_ID"].ToString().Trim() : "";
//                    //DwGain.SetItemString(row, "NAME", NAME);
//                    //DwGain.SetItemString(row, "REF_ID", REF_ID);
//                    //DwGain.SetItemString(row, "SURNAME", SURNAME);
//                    //DwGain.SetItemString(row, "PRENAME_CODE", PRENAME_CODE);
//                    //DwGain.SetItemString(row, "HOUSE_NO", HOUSE_NO);
//                    //DwGain.SetItemString(row, "GROUP_NO", GROUP_NO);
//                    //DwGain.SetItemString(row, "SOI", SOI);
//                    //DwGain.SetItemString(row, "TUMBOL", TUMBOL);
//                    //DwGain.SetItemString(row, "DISTRICT", DISTRICT);
//                    //DwGain.SetItemString(row, "PROVINCE", PROVINCE);
//                    //DwGain.SetItemString(row, "PHONE_NO", PHONE_NO);
//                    //DwGain.SetItemString(row, "POST_CODE", POST_CODE);
//                    //DwGain.SetItemString(row, "ROAD", ROAD);
//                    //DwGain.SetItemString(row, "COOPBRANCH_ID", COOPBRANCH_ID);
//                    //SetDwGainSeq();
                //}
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        //JS-EVENT
        private void JsPostInsertRowCheque()
        {
            //DwCheque.InsertRow(0);
            //DwCheque.SetItemDateTime(DwCheque.RowCount, "cheque_date", state.SsWorkDate);
            //DwCheque.SetItemDecimal(DwCheque.RowCount, "day_float", int.Parse(HdDayPassCheq.Value));
            //tDwCheque.Eng2ThaiAllRow();
            //HdIsInsertCheque.Value = "true";
        }

        //JS-EVENT
        private void JsPostBankCode()
        {
            //try
            //{
            //    Int32 row = Convert.ToInt32(HdDwChequeRow.Value);
            //    String bankCode = DwCheque.GetItemString(row, "bank_code");
            //    String sql = "select bank_desc from cmucfbank where bank_code='" + bankCode + "'";
            //    DataTable dt = WebUtil.Query(sql);
            //    if (dt.Rows.Count > 0)
            //    {
            //        String bankName = dt.Rows[0][0].ToString().Trim();
            //        DwCheque.SetItemString(row, "bank_name", bankName);
            //    }
            //    else
            //    {
            //        throw new Exception("ไม่พบรหัสธนาคาร " + bankCode);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            //}
        }

        //JS-EVENT
        private void JsPostBankBranchCode()
        {
            //try
            //{
            //    Int32 row = Convert.ToInt32(HdDwChequeRow.Value);
            //    String bankCode = DwCheque.GetItemString(row, "bank_code");
            //    String branchCode = DwCheque.GetItemString(row, "branch_code");
            //    String sql = "select branch_name from cmucfbankbranch where bank_code='" + bankCode + "' and branch_id='" + branchCode + "'";
            //    DataTable dt = WebUtil.Query(sql);
            //    if (dt.Rows.Count > 0)
            //    {
            //        String branchName = dt.Rows[0][0].ToString().Trim();
            //        DwCheque.SetItemString(row, "branch_name", branchName);
            //    }
            //    else
            //    {
            //        throw new Exception("ไม่พบรหัสสาขาธนาคารเลขที่ " + branchCode);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            //}
        }

        //JS-EVENT
        private void JsPostDeleteRowCheque()
        {
            //try
            //{
            //    DwCheque.DeleteRow(int.Parse(HdDwChequeRow.Value));
            //}
            //catch (Exception ex)
            //{
            //    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            //}
        }

        //JS-EVENT
        private void JsPostTotalAmt()
        {
            //throw new NotImplementedException();
            //DwMain.setitem
        }

        //JS-EVENT
        private void JsPostDeptAccountNo()
        {
            try
            {
                String accNo = DwMain.GetItemString(1, "tran_deptacc_no");
                String newAccNo = depService.BaseFormatAccountNo(state.SsWsPass, accNo);
                String viewAccNo = depService.ViewAccountNoFormat(state.SsWsPass, newAccNo);
                String sql = "select deptaccount_name from dpdeptmaster where deptaccount_no= '" + newAccNo + "'";
                DataTable dt = WebUtil.Query(sql);
                if (dt.Rows.Count > 0)
                {
                    DwMain.SetItemString(1, "dept_tranacc_name", dt.Rows[0][0].ToString().Trim());
                    DwMain.SetItemString(1, "tran_deptacc_no", viewAccNo);
                }
                else
                {
                    DwMain.SetItemString(1, "dept_tranacc_name", "");
                    throw new Exception("ไม่พบเลขที่บัญชี " + viewAccNo);
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.WarningMessage(ex);
            }
        }

        //JS-EVENT
        private void JsPostPostOffice()
        {
            //membgroup_code
            //membgroup_desc
            try
            {
                String postCode = DwMain.GetItemString(1, "tran_deptacc_no");
                String sql = "select membgroup_desc from mbucfmembgroup where membgroup_code = '" + postCode + "'";
                DataTable dt = WebUtil.Query(sql);
                if (dt.Rows.Count > 0)
                {
                    DwMain.SetItemString(1, "dept_tranacc_name", dt.Rows[0][0].ToString().Trim());
                }
                else
                {
                    DwMain.SetItemString(1, "dept_tranacc_name", "");
                    throw new Exception("ไม่พบรหัสปณ. : " + postCode);
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.WarningMessage(ex);
            }
        }


        //INNER FUNCTION
        private void SetDwGainSeq()
        {
            //for (int i = 1; i <= DwGain.RowCount; i++)
            //{
            //    DwGain.SetItemDecimal(i, "seq_no", i);
            //}
        }

        //INNER FUNCTION
        private void LoopCheque()
        {
            //try
            //{
            //    for (int i = 1; i <= DwCheque.RowCount; i++)
            //    {
            //        try
            //        {
            //            String chequeNo = DwUtil.GetString(DwCheque, i, "cheque_no", "");
            //            completeCheque = chequeNo == "" ? false : completeCheque;
            //            int ii = chequeNo == "" ? 0 : int.Parse(chequeNo);

            //            if (ii > 0)
            //            {
            //                DwCheque.SetItemString(i, "cheque_no", ii.ToString("0000000"));
            //            }
            //            else
            //            {
            //                completeCheque = false;
            //            }
            //        }
            //        catch { completeCheque = false; }
            //    }
            //}
            //catch { }
        }
        private void SaveKT()
        {
            string ass_docno = "";
            string ck = "";
            string memNo = DwUtil.GetString(DwMain, 1, "member_no");
            try
            {
                string qurryass = "select max(assist_docno) from asnreqmaster where assisttype_code = '70'and assist_docno like 'KT%'";
                DataTable qass = WebUtil.Query(qurryass);
                if (qass.Rows.Count > 0)
                {
                    ass_docno = qass.Rows[0][0].ToString();
                    if (ass_docno != "")
                    {
                        ass_docno = ass_docno.Substring(2, 7);
                    }
                    else { ass_docno = "000000"; }
                }
            }
            catch {
                ass_docno = "000000"; 
            }
            //string dDate = yDate;
            Decimal dDate = 2554;
            Decimal ass = Convert.ToDecimal(ass_docno);
            ass++;
            string AssAmt = "KT"+ass.ToString("000000");
            try
            {
                string savesql = "insert into asnreqmaster (assist_docno,capital_year,member_no,assisttype_code) values ('"+ AssAmt+"',"+dDate+",'"+memNo+"','70')";
                DataTable savekt = WebUtil.Query(savesql);
                
            }
                
            catch
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถบันทึกข้อมูล ในกองทุนได้");
            }
            
            
        }
    }
}