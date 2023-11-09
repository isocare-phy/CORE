using System;
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
using CommonLibrary;
using CommonLibrary.WsDeposit;
using CommonLibrary.WsCommon;
using Sybase.DataWindow;
using DBAccess;
using System.Web.Services.Protocols;
using Saving.ConstantConfig;


namespace Saving.Applications.app_assist
{
    public partial class w_sheet_kt_viewlog65 : PageWebSheet, WebSheet
    {
        private DwThDate tDwMain;
        private string ass_code;
        private String pblFileName = "kt_pension.pbl";
        private DwThDate thDwSlip;
        TimeSpan tp;
        //POSTBACK
        protected String postMemberNo;
        protected String postChangeAssist;
        protected String postChangeAmt;
        protected Sta ta;
        protected Sdt dt;

        #region WebSheet Members

        public void InitJsPostBack()
        {
            postMemberNo = WebUtil.JsPostBack(this, "postMemberNo");
            tDwMain = new DwThDate(DwMain, this);
            thDwSlip = new DwThDate(DwSlip, this);
        }

        public void WebSheetLoadBegin()
        {
            this.ConnectSQLCA();
            ta = new Sta(sqlca.ConnectionString);
            dt = new Sdt();

            if (!IsPostBack)
            {
                tDwMain.Eng2ThaiAllRow();
                DwMain.Reset();
                DwMain.InsertRow(0);
            }
            else
            {
                this.RestoreContextDw(DwMain);
                this.RestoreContextDw(DwSlip);
            }
            tDwMain.Eng2ThaiAllRow();
        }

        public void CheckJsPostBack(string eventArg)
        {

            if (eventArg == "postMemberNo")
            {
                JsPostMemberNo();
            }

        }

        public void SaveWebSheet()
        {
            string status = "NO";
            string payoutslip_no = "";
            string cancel_id = "";
            decimal payout_amt = 0;
            string money_type = "";
            decimal post_fin = 0;
            string member_no = DwMain.GetItemString(1, "mbmembmaster_member_no");
            int row = DwSlip.RowCount;
            //เช็คสถานะสลิป
            for (int i = 1; i < row + 1; i++)
            {
                post_fin = DwSlip.GetItemDecimal(i, "post_fin");
                try
                {
                    cancel_id = DwSlip.GetItemString(i, "cancel_id");
                }
                catch { cancel_id = "0"; }
                if (post_fin == 8 && cancel_id == "1")
                { status = "YES"; }
            }
            if (status == "YES")
            {
                for (int i = 1; i < row + 1; i++)
                {
                    payoutslip_no = DwSlip.GetItemString(i, "payoutslip_no");
                    cancel_id = DwSlip.GetItemString(i, "cancel_id");
                    payout_amt = DwSlip.GetItemDecimal(i, "payout_amt");
                    money_type = DwSlip.GetItemString(i, "cmucfmoneytype_moneytype_desc");
                    post_fin = DwSlip.GetItemDecimal(i, "post_fin");

                    if (cancel_id == "1")
                    {
                        if (money_type == "เงินสด")
                        {
                            try
                            {
                                string sqlcancel = "update asnslippayout set cancel_id = '" + state.SsUsername + "',slip_status=-9, cancel_date = to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy')" +
                                " where payoutslip_no='" + payoutslip_no + "'";
                                Sdt can = WebUtil.QuerySdt(sqlcancel);
                            }
                            catch (Exception ex)
                            {
                                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                            }
                        }//end if moneytype
                        else
                        {  //ยกเลิก 2 ที่

                            try
                            {
                                string sqlcancel = "update asnslippayout set cancel_id = '" + state.SsUsername +
                                "',slip_status=-9, cancel_date = to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy')" +
                                " where payoutslip_no='" + payoutslip_no + "'";
                                Sdt can = WebUtil.QuerySdt(sqlcancel);
                            }
                            catch (Exception ex)
                            {
                                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                            }
                            //ยกเลิก dpdepttran
                            try
                            {
                                string sqldp = "update dpdepttran set tran_status = -9 where ref_slipno ='" + payoutslip_no + "'";
                                Sdt can = WebUtil.QuerySdt(sqldp);
                            }
                            catch (Exception ex)
                            {
                                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                            }
                            //ยกเลิก sltranpayin
                            try
                            {
                                string sqlsl = "update sltranspayin set trans_status=-9 where system_code='ASS' and loancontract_no=(select loancontract_no from asnslippayoutdet where payoutslip_no='" + payoutslip_no + "'";
                                Sdt can = WebUtil.QuerySdt(sqlsl);
                            }
                            catch (Exception ex)
                            {
                                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                            }
                        }
                        //---------------------------------
                        //คืนเงินเข้ากองทุน
                        decimal perpay = payout_amt;
                        decimal sumpays = 0;
                        decimal balance = 0;
                        try
                        {
                            string sqlQ = "select perpay,sumpays,balance from asnreqmaster where member_no='" + member_no + "' and assist_docno like 'SF%'";
                            Sdt selectQ = WebUtil.QuerySdt(sqlQ);
                            if (selectQ.Next())
                            {
                                //perpay = selectQ.GetDecimal("perpay");
                                sumpays = selectQ.GetDecimal("sumpays");
                                balance = selectQ.GetDecimal("balance");
                            }
                            sumpays = sumpays - perpay;
                            balance = balance + perpay;
                        }
                        catch (Exception ex)
                        {
                            LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                        }
                        try
                        {
                            string updatepay = "update asnreqmaster set sumpays=" + sumpays + ",balance=" + balance + ",pay_status=8" +
                            "   where member_no='" + member_no + "' and assist_docno like 'SF%'";
                            Sdt update = WebUtil.QuerySdt(updatepay);
                            //-------------------------------------------------
                            //decimal withdraw_count = 0;
                            //string sqlwithdraw = "select withdraw_count from asnpensionperpay where member_no='" + member_no + "' ";
                            //update = WebUtil.QuerySdt(sqlwithdraw);
                            //if (update.Next())
                            //{
                            //    withdraw_count = update.GetDecimal("withdraw_count");
                            //}
                            ////-------------------------------------------------
                            //withdraw_count = withdraw_count - 1;
                            //string updateperpay = "update asnpensionperpay set withdraw_count =" + withdraw_count + " where member_no='" + member_no + "'";
                            //update = WebUtil.QuerySdt(updateperpay);
                            //-------------------------------------------------
                            DwSlip.Reset();
                            object[] argsDwMain = new object[2] { member_no, "90" };
                            DwUtil.RetrieveDataWindow(DwSlip, "kt_65years.pbl", null, argsDwMain);
                            LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
                        }
                        catch (Exception ex)
                        {
                            LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                        }
                        //--------------------------------------------------------------------------------

                    }//end if cancel
                    else
                    {

                    }

                }//end for

            }
            else
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถยกเลิกรายการที่จ่ายไปแล้ว");
            }




        }

        public void WebSheetLoadEnd()
        {
            DwSlip.PageNavigationBarSettings.Visible = (DwSlip.RowCount > 10);
            DwMain.SaveDataCache();
            DwSlip.SaveDataCache();
            ta.Close();
        }

        #endregion

        private void NewClear()
        {
        }

        //JS-POSTBACK
        private void JsPostMemberNo()
        {
            string membNo = int.Parse(DwMain.GetItemString(1, "mbmembmaster_member_no")).ToString("00000000");
            ass_code = "90"; //กำหนดค่า assisttype_code = 90 ซึ่งเป็นประเภทกองทุนสวัสดิการเงินสะสม 65 ปีมีสุข
            DateTime birthdate, memberdate;
            string name, sname, personID, tel, mobileTel, member_no;
            object[] argsDwMain = new object[2] { membNo, ass_code };
            try
            {
                DwMain.SetItemString(1, "mbmembmaster_member_no", membNo);
                try // Query ข้อมูลส่วนตัวของสมาชิกเพื่อมาแสดง
                {
                    string sqlassdocno = @"select member_no, memb_name, memb_surname, card_person, birth_date, member_date, mem_tel, mem_telmobile  from mbmembmaster where member_no = '" + membNo + "'";
                    dt = ta.Query(sqlassdocno);
                    dt.Next();
                    member_no = dt.GetString("member_no");
                    if (member_no == "")
                    {
                        throw new Exception("ไม่พบข้อมูลของหมายเลขสมาชิกนี้");
                    }
                    name = dt.GetString("memb_name");
                    sname = dt.GetString("memb_surname");
                    personID = dt.GetString("card_person");
                    birthdate = dt.GetDate("birth_date");
                    memberdate = dt.GetDate("member_date");
                    tel = dt.GetString("mem_tel");
                    mobileTel = dt.GetString("mem_telmobile");
                    //DwMain.SetItemDateTime(1, "mbmembmaster_birth_date", birthdate);
                    //DwMain.SetItemDateTime(1, "mbmembmaster_member_date", memberdate);
                    //DwMain.SetItemString(1, "mbmembmaster_memb_name", name);
                    //DwMain.SetItemString(1, "mbmembmaster_memb_surname", sname);
                    //DwMain.SetItemString(1, "mbmembmaster_card_person", personID);
                    //DwMain.SetItemString(1, "mbmembmaster_mem_tel", tel);
                    //DwMain.SetItemString(1, "mbmembmaster_mem_telmobile", mobileTel);
                    //tp = state.SsWorkDate - birthdate;
                    //Double age_year = (tp.TotalDays / 365);
                    //string age = age_year.ToString();
                    //age = age.Substring(0, 2);
                    //DwMain.SetItemString(1, "age", age);

                    try
                    {
                        DwMain.SetItemDateTime(1, "mbmembmaster_birth_date", birthdate);
                        tp = state.SsWorkDate - birthdate;
                        Double age_year = (tp.TotalDays / 365);
                        string age = age_year.ToString();
                        age = age.Substring(0, 2);
                        DwMain.SetItemString(1, "age", age);
                    }
                    catch { DwMain.SetItemString(1, "age", "-"); }
                    try
                    {
                        DwMain.SetItemDateTime(1, "mbmembmaster_member_date", memberdate);
                    }
                    catch { }
                    try
                    {
                        DwMain.SetItemString(1, "mbmembmaster_memb_name", name);
                    }
                    catch { DwMain.SetItemString(1, "mbmembmaster_memb_name", "-"); }
                    try
                    {
                        DwMain.SetItemString(1, "mbmembmaster_memb_surname", sname);
                    }
                    catch { DwMain.SetItemString(1, "mbmembmaster_memb_surname", "-"); }
                    try
                    {
                        DwMain.SetItemString(1, "mbmembmaster_card_person", personID);
                    }
                    catch { DwMain.SetItemString(1, "mbmembmaster_card_person", "-"); }
                    try
                    {
                        DwMain.SetItemString(1, "mbmembmaster_mem_tel", tel);
                    }
                    catch { DwMain.SetItemString(1, "mbmembmaster_mem_tel", "-"); }
                    try
                    {
                        DwMain.SetItemString(1, "mbmembmaster_mem_telmobile", mobileTel);
                    }
                    catch { DwMain.SetItemString(1, "mbmembmaster_mem_telmobile", "-"); }

                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                }
            }
            catch (Exception)
            {
                LtServerMessage.Text = WebUtil.WarningMessage("ไม่พบเลขที่สมาชิก " + membNo);
            }

            try
            {
                DwSlip.Reset();
                DwUtil.RetrieveDataWindow(DwSlip, pblFileName, null, argsDwMain);
                int row = DwSlip.RowCount;
                string slip_no = "";
                decimal tran_status = 0;
                for (int i = 1; i < row + 1; i++)
                {
                    //slip_no = DwSlip.GetItemString(i, "payoutslip_no");
                    try
                    {
                        slip_no = DwSlip.GetItemString(i, "payoutslip_no");
                    }
                    catch { 
                        slip_no = "";
                            DwSlip.Reset();
                    }
                    if (slip_no != "")
                    {
                        try
                        {
                            string sqlck = "select tran_status from dpdepttran where ref_slipno ='" + slip_no + "' and tran_status <> -9 ";
                            Sdt ck = WebUtil.QuerySdt(sqlck);
                            if (ck.Next())
                            {
                                tran_status = ck.GetDecimal("tran_status");
                                if (tran_status == 0)
                                {
                                    DwSlip.SetItemDecimal(i, "post_fin", 8);
                                }
                                else
                                {
                                    DwSlip.SetItemDecimal(i, "post_fin", 1);
                                }

                            }
                        }
                        catch { }
                        try
                        {
                            string sqlck = "select trans_status from sltranspayin where system_code='ASS' and trans_status<>-9 and loancontract_no= " +
                                           "(select loancontract_no from asnslippayoutdet where payoutslip_no='" + slip_no + "')";
                            Sdt ck = WebUtil.QuerySdt(sqlck);
                            if (ck.Next())
                            {
                                tran_status = ck.GetDecimal("trans_status");
                                if (tran_status == 0)
                                {
                                    DwSlip.SetItemDecimal(i, "post_fin", 8);
                                }
                                else
                                {
                                    DwSlip.SetItemDecimal(i, "post_fin", 1);
                                }
                            }
                        }
                        catch { }
                    }
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }

        }

    }
}