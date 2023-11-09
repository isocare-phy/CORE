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
using System.Globalization;
using Sybase.DataWindow;
using DataLibrary;
using System.Data.OracleClient;
using System.Web.Services.Protocols;
using CoreSavingLibrary.WcfNAccount;

namespace Saving.Applications.account.dlg
{
    public partial class w_dlg_acc_bktrnpay_edit : PageWebDialog, WebDialog
    {
        private n_accountClient accService;
        private DwThDate tDwMain;
        protected string jsPostDetail;
        protected string jsPostDwShareInsertRow;
        protected string jsPostDwLoanInsertRow;
        protected string jsPostShareFlag;
        protected string jsPostLoanFlag;
        protected string jsPostDwOtherInsertRow;
        protected string jsSave;
        protected string jsDeleteShare;
        protected string jsPostOtherFlag;
        String mem_name, mem_surname, memb_group, prename_code, memb_desc, pre_desc, remark, phone, mobile, resign_code, resign_desc;

        private void JsDeleteShare()
        {
            Int16 Rowshare = Convert.ToInt16(HdRowDeleteShare.Value);
            String Pay_docno = DwShare.GetItemString(Rowshare, "paytrnbank_docno");
            String paytype = DwShare.GetItemString(Rowshare, "paytrnitemtype_code");
            try
            {
                String sql = @"DELETE FROM accpaytrnbankdet WHERE paytrnbank_docno ='" + Pay_docno + @"' 
                                and coop_id ='" + state.SsCoopId + @"' and paytrnitemtype_code = '" + paytype + @"'";
                WebUtil.ExeSQL(sql);
                LtServerMessage.Text = WebUtil.CompleteMessage("ลบข้อมูลเรียบร้อยแล้ว");
                DwShare.Retrieve(Pay_docno);

            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
            }    
            
        }

        private void JsPostDetail()
        {

            String memno = Dwmain.GetItemString(1, "member_no");
            memno = WebUtil.MemberNoFormat(memno);

                Sta ta = new Sta(state.SsConnectionString);
                String sql = "";
                sql = @"SELECT MEMB_NAME , MEMB_SURNAME, MEMBGROUP_CODE, PRENAME_CODE, ADDR_PHONE, ADDR_MOBILEPHONE, REMARK, RESIGNCAUSE_CODE  
                    FROM MBMEMBMASTER  
                    WHERE ( MEMBER_NO = '" + memno + "' ) AND ( COOP_ID = '" + state.SsCoopId + @"' )";
                Sdt dt = ta.Query(sql);
                try
                {
                    if (dt.Next())
                    {
                        mem_name = dt.Rows[0]["MEMB_NAME"].ToString();
                        mem_surname = dt.Rows[0]["MEMB_SURNAME"].ToString();
                        memb_group = dt.Rows[0]["MEMBGROUP_CODE"].ToString();
                        prename_code = dt.Rows[0]["PRENAME_CODE"].ToString();
                        phone = dt.Rows[0]["ADDR_PHONE"].ToString();
                        mobile = dt.Rows[0]["ADDR_MOBILEPHONE"].ToString();
                        remark = dt.Rows[0]["REMARK"].ToString();
                        resign_code = dt.Rows[0]["RESIGNCAUSE_CODE"].ToString();
                    }
                }
                catch
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบหมายเลขสมาชิกเลขที่ " + memno);
                }
                ta.Close();



                Sta ta2 = new Sta(state.SsConnectionString);
                String sql2 = "";
                sql2 = @"SELECT MEMBGROUP_DESC 
                    FROM MBUCFMEMBGROUP  
                    WHERE ( MEMBGROUP_CODE = '" + memb_group + "' ) AND ( COOP_ID = '" + state.SsCoopId + @"' )";
                Sdt dt2 = ta2.Query(sql2);
                try
                {
                    if (dt2.Next())
                    {
                        memb_desc = dt2.Rows[0]["MEMBGROUP_DESC"].ToString();
                    }
                }
                catch
                {

                }
                ta2.Close();

                Sta ta3 = new Sta(state.SsConnectionString);
                String sql3 = "";
                sql3 = @"SELECT PRENAME_DESC 
                    FROM MBUCFPRENAME  
                    WHERE ( PRENAME_CODE = '" + prename_code + "' )";
                Sdt dt3 = ta3.Query(sql3);
                try
                {
                    if (dt3.Next())
                    {
                        pre_desc = dt3.Rows[0]["PRENAME_DESC"].ToString();
                    }
                }
                catch
                {

                }
                ta3.Close();

                Sta ta4 = new Sta(state.SsConnectionString);
                String sql4 = "";
                sql4 = @"SELECT RESIGNCAUSE_DESC 
                    FROM  MBUCFRESIGNCAUSE  
                    WHERE ( RESIGNCAUSE_CODE = '" + resign_code + "' )";
                Sdt dt4 = ta4.Query(sql4);
                try
                {
                    if (dt4.Next())
                    {
                        resign_desc = dt4.Rows[0]["RESIGNCAUSE_DESC"].ToString();
                    }
                }
                catch
                {

                }
                ta4.Close();

                Dwmain.SetItemString(1, "mem_name", pre_desc + mem_name + " " + mem_surname);
                Dwmain.SetItemString(1, "mem_group", memb_group + " - " + memb_desc);
                Dwmain.SetItemString(1, "coop_id", state.SsCoopId);
                Dwmain.SetItemString(1, "membgroup_code", memb_group);
                Dwmain.SetItemString(1, "member_no", memno);
                Dwmain.SetItemString(1, "addr_phone", phone);
                Dwmain.SetItemString(1, "addr_mobilephone", mobile);
                Dwmain.SetItemString(1, "remark", remark);
                Dwmain.SetItemString(1, "resigncause_desc", resign_desc);
        }

        private void JsPostDwShareInsertRow()
        {
            DwShare.InsertRow(0);

        }

        private void JsPostDwLoanInsertRow()
        {
            DwLoan.InsertRow(0);

        }

        private void JsPostDwOtherInsertRow()
        {
            DwOther.InsertRow(0);
        }
        

        private void JsPostShareFlag()
        {
            Decimal flag =  DwShare.GetItemDecimal(1, "operate_flag");
            int sharerow = int.Parse(HdRowShare.Value);
            DwShare.SetItemDecimal(sharerow, "operate_flag", flag);
        
        }

        private void JsPostLoanFlag()
        {
            Decimal flag = DwLoan.GetItemDecimal(1, "operate_flag");
            int loanrow = int.Parse(HdRowLoan.Value);
            DwLoan.SetItemDecimal(loanrow, "operate_flag", flag);
            if (flag == 1)
            {
                try
                {
                    String mem_no = Dwmain.GetItemString(1, "member_no");
                    DwUtil.RetrieveDDDW(DwLoan, "loancontract_no", "acc_post_pay.pbl", mem_no);
                }
                catch {}
            }
            else
            {
                DwLoan.SetItemString(loanrow, "loancontract_no", "");
            }

        }

        private void JsPostOtherFlag()
        {
            Decimal flag = DwOther.GetItemDecimal(1, "operate_flag");
            int otherrow = int.Parse(HdRowOther.Value);
            DwShare.SetItemDecimal(otherrow, "operate_flag", flag);

        }

        private void jsPostMember()
        { 
        
        }


        private void JsSave()
        {
            try
            {
                n_accountClient accService = wcf.NAccount;
                // การโยน ไฟล์ xml ไปให้ service
                String xmlDwmain = Dwmain.Describe("Datawindow.Data.Xml");
                String xmlDwShare = DwShare.Describe("Datawindow.Data.Xml");
                String xmlDwLoan = DwLoan.Describe("Datawindow.Data.Xml");
                String xmlDwEtc = DwOther.Describe("Datawindow.Data.Xml");
                //int result = accService.of_gen_data_paytrnbank_detail(state.SsWsPass, xmlDwmain, xmlDwShare, xmlDwLoan, xmlDwEtc);
                int result = wcf.NAccount.of_gen_data_paytrnbank(state.SsWsPass, xmlDwmain, xmlDwShare, xmlDwLoan, xmlDwEtc);
                if (result == 1)
                {
                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลเรียบร้อยแล้ว");
                }
                else
                {
                    LtServerMessage.Text = WebUtil.CompleteMessage("ไม่สามารถบันทึกรายการได้");
                }
            }
            catch (SoapException ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(WebUtil.SoapMessage(ex));
            }
        }

        public void InitJsPostBack()
        {
            jsPostDetail = WebUtil.JsPostBack(this, "jsPostDetail");
            jsPostDwShareInsertRow = WebUtil.JsPostBack(this, "jsPostDwShareInsertRow");
            jsPostDwLoanInsertRow = WebUtil.JsPostBack(this, "jsPostDwLoanInsertRow");
            jsPostShareFlag = WebUtil.JsPostBack(this, "jsPostShareFlag");
            jsPostLoanFlag = WebUtil.JsPostBack(this, "jsPostLoanFlag");
            jsPostOtherFlag = WebUtil.JsPostBack(this, "jsPostOtherFlag");
            jsPostDwOtherInsertRow = WebUtil.JsPostBack(this, "jsPostDwOtherInsertRow");
            jsSave = WebUtil.JsPostBack(this, "jsSave");
            jsDeleteShare = WebUtil.JsPostBack(this, "jsDeleteShare");
            tDwMain = new DwThDate(Dwmain, this);
            tDwMain.Add("doc_date", "doc_tdate");
        }

        public void WebDialogLoadBegin()
        {
            this.ConnectSQLCA();
            Dwmain.SetTransaction(sqlca);
            DwShare.SetTransaction(sqlca);
            DwLoan.SetTransaction(sqlca);
            DwOther.SetTransaction(sqlca);
            //tDwMain = new DwThDate(Dwmain);
            //tDwMain.Add("doc_date", "doc_tdate");
            try
            {
                if (!IsPostBack)
                {
                    Dwmain.InsertRow(0);
                    DwShare.InsertRow(0);
                    DwLoan.InsertRow(0);
                    DwOther.InsertRow(0);
                    try
                    {
                        String queryStrTrnDate = "";
                        String payType = "";
                        
                        try { queryStrTrnDate = Request["trnDate"].Trim(); }
                        catch { }
                        try { payType = Request["payType"].Trim(); }
                        catch { }
                        if (payType == "b_new")
                        {

                            DateTime Trn_PayDate = DateTime.ParseExact(queryStrTrnDate, "ddMMyyyy", new CultureInfo("th-TH"));
                            Dwmain.SetItemDate(1, "trn_date", Trn_PayDate);
                            //Dwmain.SetItemString(1, "trn_tdate", Trn_PayDate.ToString("ddMMyyyy", new CultureInfo("th-TH")));
                            Dwmain.SetItemDate(1, "doc_date", state.SsWorkDate);
                            Dwmain.SetItemString(1, "doc_tdate", state.SsWorkDate.ToString("ddMMyyyy", new CultureInfo("th-TH")));
                            Dwmain.SetItemString(1, "mem_name", "");
                            Dwmain.SetItemString(1, "mem_group", "");
                        }
                        else
                        {
                            String Acno = "";
                            try { Acno = Request["Acno"].Trim(); }
                            catch { }
                            Dwmain.Retrieve(Acno);
                            JsPostDetail();
                            DateTime doc_date =  Dwmain.GetItemDateTime(1, "doc_date");
                            Dwmain.SetItemString(1, "doc_tdate", doc_date.ToString("ddMMyyyy", new CultureInfo("th-TH")));
                            DwShare.Retrieve(Acno);
                            DwLoan.Retrieve(Acno);
                            DwOther.Retrieve(Acno);
                        }

                    }
                    catch (Exception ex)
                    {
                        LtServerMessage.Text = ex.ToString();
                    }
                }
                else
                {
                    this.RestoreContextDw(Dwmain);
                    this.RestoreContextDw(DwShare);
                    this.RestoreContextDw(DwLoan);
                    this.RestoreContextDw(DwOther);
                }
            }

            catch (Exception ex)
            {
                LtServerMessage.Text = ex.ToString();
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "jsPostDetail")
            {
                JsPostDetail();
            }
            else if (eventArg == "jsPostDwShareInsertRow")
            {
                JsPostDwShareInsertRow();
            }
            else if (eventArg == "jsPostDwLoanInsertRow")
            {
                JsPostDwLoanInsertRow();
            }
            else if (eventArg == "jsPostShareFlag")
            {
                JsPostShareFlag();
            }
            else if (eventArg == "jsPostLoanFlag")
            {
                JsPostLoanFlag();
            }
            else if (eventArg == "JsPostOtherFlag")
            {
                JsPostOtherFlag();
            }
            else if (eventArg == "jsPostDwOtherInsertRow")
            {
                JsPostDwOtherInsertRow();
            }
            else if (eventArg == "jsSave")
            {
                JsSave();
            }
            else if (eventArg == "jsDeleteShare")
            {
                JsDeleteShare();
            }          
            
        }

        public void WebDialogLoadEnd()
        {
            Dwmain.SaveDataCache();
            DwShare.SaveDataCache();
            DwLoan.SaveDataCache();
            DwOther.SaveDataCache();
        }


    }
}