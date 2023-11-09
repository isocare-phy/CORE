using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using DataLibrary;

namespace Saving.Applications.assist.ws_sheet_as_ucfassist_ctrl
{
    public partial class ws_sheet_as_ucfassist : PageWebSheet, WebSheet
    {
        [JsPostBack]
        public string PostNewRowTap2 { get; set; }
        [JsPostBack]
        public string PostNewRowTap3 { get; set; }
        [JsPostBack]
        public string PostDelRowTap1 { get; set; }
        [JsPostBack]
        public string PostDelRowTap2 { get; set; }
        [JsPostBack]
        public string PostDelRowTap3 { get; set; }
        [JsPostBack]
        public string PostSendAssCode { get; set; }
        
        
        public string sqlStr;

        public void InitJsPostBack()
        {
            dsAssisttype.InitDsAssisttype(this);
            dsUcfasstype.InitDsUcfasstype(this);
            dsUcfasspaytype.InitDsUcfasspaytype(this);
            //dsUcfasstypedet.InitDsUcfasstypedet(this);
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)//show data first  
            {
                dsAssisttype.AssistType();
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == PostNewRowTap2)
            {
                string ls_asscode = dsAssisttype.DATA[0].ASSISTTYPE_CODE;
                dsUcfasspaytype.InsertAtRow(0);
                dsUcfasspaytype.SetItem(0, dsUcfasspaytype.DATA.COOP_IDColumn, state.SsCoopControl);//set value to primary key

                string ls_paycode;
                //for (int li_row = 1; li_row < dsUcfasspaytype.RowCount; li_row++)
                //{
                //    if (li_chkasscode < Convert.ToInt32(dsUcfasspaytype.DATA[li_row].ASSISTPAY_CODE))
                //    {
                //        li_chkasscode = Convert.ToInt32(dsUcfasspaytype.DATA[li_row].ASSISTPAY_CODE);
                //    }
                //}

                string sql = @"select * from
                    (
	                    select max(assistpay_code) assistpay_code from assucfassistpaytype where coop_id = {0}
	                    union
	                    select '0' from assucfassistpaytype where rownum = 1
                    )order by assistpay_code desc";
                sql = WebUtil.SQLFormat(sql,state.SsCoopId);
                Sdt dt = WebUtil.QuerySdt(sql);
                dt.Next();

                ls_paycode = (dt.GetInt32("assistpay_code") + 1).ToString();
                if (ls_paycode.Length == 1) { ls_paycode = '0' + ls_paycode; }

                sql = @"select assisttype_group from assucfassisttype where assisttype_code={0} and coop_id={1}";
                sql = WebUtil.SQLFormat(sql, ls_asscode, state.SsCoopId);
                dt = WebUtil.QuerySdt(sql);
                dt.Next();
                dsUcfasspaytype.DATA[0].ASSISTTYPE_GROUP = dt.GetString("assisttype_group");
                dsUcfasspaytype.DATA[0].ASSISTPAY_CODE = ls_paycode;
                dsUcfasspaytype.RetriveGroup();
            }
            //else if(eventArg == PostNewRowTap3)
            //{
            //    dsUcfasstypedet.InsertLastRow();
            //    int li_seqno = dsUcfasstypedet.RowCount;
            //    int row = dsUcfasstypedet.RowCount - 1;
            //    dsUcfasstypedet.AssistPayType(dsAssisttype.DATA[0].ASSISTTYPE_CODE);
            //    dsUcfasstypedet.DATA[row].SEQ_NO = li_seqno;
            //    dsUcfasstypedet.Membertype();
            //    dsUcfasstypedet.FindTextBox(row, "next_payamt").Enabled = false;
            //    dsUcfasstypedet.FindDropDownList(row, "next_typepay").Enabled = false;
            //    dsUcfasstypedet.FindTextBox(row, "max_nextpayamt").Enabled = false;
            //}
            else if (eventArg == PostDelRowTap1)
            {
                string ls_chktype = "";
                //int row = dsUcfasstype.GetRowFocus();
                string ls_type = dsUcfasstype.DATA[0].ASSISTTYPE_CODE;

                //chk ประเภทคำขอ
                string sql = @"select assisttype_code from asnreqmaster where assisttype_code={0} and coop_id={1} and req_status=1 and rownum=1";
                sql = WebUtil.SQLFormat(sql, ls_type, state.SsCoopControl);
                Sdt dt = WebUtil.QuerySdt(sql);
                //chk เงื่อนไขการจ่าย
                string sql_pay = @"select assisttype_pay from ASSUCFASSISTTYPEDET where assisttype_code={0} and coop_id={1} ";
                sql_pay = WebUtil.SQLFormat(sql_pay, ls_type, state.SsCoopControl);
                Sdt dt_pay = WebUtil.QuerySdt(sql_pay);

                if (dt.Next())
                {
                    ls_chktype = dt.GetString("assisttype_code");
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('สวัสดิการประเภทนี้มีการขอใช้แล้วไม่สามารถลบได้');", true); return;
                }
                else if (dt_pay.Next())
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('สวัสดิการประเภทนี้มีการกำหนดเงื่อนไขการจ่ายแล้วไม่สามารถลบได้');", true); return;
                }
                else
                {
                    //dsList.DeleteRow(row);
                    try
                    {
                        ls_chktype = @"delete from ASSUCFASSISTTYPE where coop_id = {0} and assisttype_code={1} ";
                        ls_chktype = WebUtil.SQLFormat(ls_chktype, state.SsCoopId, ls_type);
                        WebUtil.ExeSQL(ls_chktype);
                        dsUcfasstype.ResetRow();
                        dsAssisttype.AssistType();
                        LtServerMessage.Text = WebUtil.CompleteMessage("ลบข้อมูลสำเร็จ");
                    }
                    catch
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage("ลบข้อมูลไม่สำเสร็จ");
                    }
                }
            }
            else if (eventArg == PostDelRowTap2)
            {
                string ls_chktype = "";
                int row = dsUcfasspaytype.GetRowFocus();
                string ls_type = dsUcfasspaytype.DATA[row].ASSISTPAY_CODE;
                string ls_asscode = dsAssisttype.DATA[0].ASSISTTYPE_CODE;
                //chk เงื่อนไขการจ่าย
                string sql = @"select assisttype_pay from ASSUCFASSISTTYPEDET where assisttype_pay={0} and coop_id={1} ";
                sql = WebUtil.SQLFormat(sql, ls_type, state.SsCoopControl);
                Sdt dt = WebUtil.QuerySdt(sql);
                if (dt.Next())
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('การจ่ายสวัสดิการประเภทนี้มีการกำหนดเงื่อนไขการจ่ายแล้วไม่สามารถลบได้');", true); return;
                }
                else
                {
                    try
                    {
                        ls_chktype = @"delete from ASSUCFASSISTPAYTYPE where coop_id = {0} and assistpay_code={1} ";
                        ls_chktype = WebUtil.SQLFormat(ls_chktype, state.SsCoopId, ls_type);
                        WebUtil.ExeSQL(ls_chktype);
                        dsAssisttype.AssistType();
                        dsUcfasspaytype.Retrieve(ls_asscode);
                        dsUcfasspaytype.RetriveGroup();
                        LtServerMessage.Text = WebUtil.CompleteMessage("ลบข้อมูลสำเร็จ");
                    }
                    catch
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage("ลบข้อมูลไม่สำเสร็จ");
                    }
                } 
            }
            //else if (eventArg == PostDelRowTap3)
            //{
            //    int row = dsUcfasstypedet.GetRowFocus();
            //    string ls_typepay = dsUcfasstypedet.DATA[row].ASSISTTYPE_PAY;
            //    string ls_asscode = dsAssisttype.DATA[0].ASSISTTYPE_CODE;
            //    string ls_memtype = dsUcfasstypedet.DATA[row].MEMBTYPE_CODE.ToString();
            //    string ls_seq_no = dsUcfasstypedet.DATA[row].SEQ_NO.ToString();
            //    try
            //    {
            //        string sql = @"delete from assucfassisttypedet where coop_id = {0} and assisttype_pay={1} and assisttype_code={2} and membtype_code={3} and seq_no={4}";
            //        sql = WebUtil.SQLFormat(sql, state.SsCoopId, ls_typepay, ls_asscode, ls_memtype, ls_seq_no);
            //        WebUtil.ExeSQL(sql);
            //        dsUcfasstypedet.RetrieveData(ls_asscode);
            //        dsUcfasstypedet.Membertype();
            //        LtServerMessage.Text = WebUtil.CompleteMessage("ลบข้อมูลสำเร็จ");
            //    }
            //    catch
            //    {
            //        LtServerMessage.Text = WebUtil.ErrorMessage("ลบข้อมูลไม่สำเสร็จ");
            //        return;
            //    }
            //}
            else if (eventArg == PostSendAssCode)
            {
                string ls_asscode = dsAssisttype.DATA[0].ASSISTTYPE_CODE;
                dsUcfasstype.Retrieve(ls_asscode);
                dsUcfasstype.RetriveGroup();
                dsUcfasspaytype.Retrieve(ls_asscode);
                dsUcfasspaytype.RetriveGroup();
                //dsUcfasstypedet.RetrieveData(ls_asscode);
                //dsUcfasstypedet.Membertype();
            }
        }


        public void SaveWebSheet()
        {
            if (dsAssisttype.DATA[0].ASSISTTYPE_CODE == "00")
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("กรุณาเลือกประเภท สวัสดิการ");
                return;
            }

            ////////////// tap 1 ////////////////
            string sqlStr, ls_assiscode, ls_assisdesc, ls_chkassiscode = "", ls_assisgroup = "";
            decimal dec_membflag, dec_calflag, dec_proflag;
            int li_row, li_family;
            decimal li_stmflag = 0;
            ls_assiscode = dsUcfasstype.DATA[0].ASSISTTYPE_CODE.ToString();
            try
            {

                if (dsUcfasstype.DATA[0].ASSISTTYPE_CODE.ToString() == "")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('กรุณากรอกรหัสประเภทสวัสดิการ');", true); return;
                }
                else if (dsUcfasstype.DATA[0].ASSISTTYPE_DESC.ToString() == "")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('กรุณากรอกคำอธิบายสวัสดิการ');", true); return;
                }
                
                ls_assisdesc = dsUcfasstype.DATA[0].ASSISTTYPE_DESC.ToString();
                li_stmflag = dsUcfasstype.DATA[0].STM_FLAG;
                ls_assisgroup = dsUcfasstype.DATA[0].ASSISTTYPE_GROUP;
                dec_membflag = dsUcfasstype.DATA[0].membtype_flag;
                dec_calflag = dsUcfasstype.DATA[0].calculate_flag;
                li_family = dsUcfasstype.DATA[0].family_flag;

                if (li_stmflag == 0)
                {
                    dec_proflag = 0;
                }
                else
                {
                    dec_proflag = dsUcfasstype.DATA[0].process_flag;
                }

                //chk ประเภทสวัสดิการ
                string sql = @"select assisttype_code from ASSUCFASSISTTYPE where assisttype_code={0} and coop_id={1}";
                sql = WebUtil.SQLFormat(sql, ls_assiscode, state.SsCoopControl);
                Sdt dt = WebUtil.QuerySdt(sql);
                if (dt.Next())
                {
                    sqlStr = @"update ASSUCFASSISTTYPE set 
                            assisttype_desc={0},stm_flag={1},ASSISTTYPE_GROUP={4}, membtype_flag = {5}, calculate_flag = {6}, process_flag = {7}, family_flag = {8}
                            where assisttype_code={2} and coop_id={3}
                            ";
                    sqlStr = WebUtil.SQLFormat(sqlStr, ls_assisdesc, li_stmflag, ls_assiscode, state.SsCoopId, ls_assisgroup, dec_membflag, dec_calflag, dec_proflag, li_family);
                    WebUtil.ExeSQL(sqlStr);
                }
                dsUcfasstype.Retrieve(ls_assiscode);
                dsUcfasstype.RetriveGroup();
            }
            catch
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ประเภทสวัสดิการ บันทึกไม่สำเสร็จ");
                return;
            }
            ////////////// tap 2 ////////////////
            try
            {
                for (li_row = 0; li_row < dsUcfasspaytype.RowCount; li_row++)
                {
                    if (dsUcfasspaytype.DATA[li_row].ASSISTPAY_CODE.ToString() == "")
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('กรุณากรอกรหัสประเภทการจ่ายสวัสดิการ');", true); return;
                    }
                    else if (dsUcfasspaytype.DATA[li_row].ASSISTPAY_DESC.ToString() == "")
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('กรุณากรอกคำอธิบายารจ่ายสวัสดิการ');", true); return;
                    }
                    //else if (ls_chkassiscode.IndexOf(dsUcfasspaytype.DATA[li_row].ASSISTPAY_CODE.ToString()) > 0)
                    //{
                    //    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('เลขประเภทการจ่ายสวัสดิการซ้ำกัน กรุณาตรวจสอบ');", true); return;
                    //}
                    //ls_chkassiscode = ls_chkassiscode + ", " + dsUcfasspaytype.DATA[li_row].ASSISTPAY_CODE.ToString();

                }
                for (li_row = 1; li_row < dsUcfasspaytype.RowCount; li_row++)
                {
                    string ls_asspaycode = dsUcfasspaytype.DATA[li_row -1].ASSISTPAY_CODE;
                    ls_assisdesc = dsUcfasspaytype.DATA[li_row - 1].ASSISTPAY_DESC.ToString();
                    ls_assisgroup = dsUcfasspaytype.DATA[li_row- 1].ASSISTTYPE_GROUP;
                    //chk ประเภทสวัสดิการ
                    string sql = @"select assistpay_code from ASSUCFASSISTPAYTYPE where assistpay_code={0} and coop_id={1} and ASSISTTYPE_GROUP = {2}";
                    sql = WebUtil.SQLFormat(sql, ls_asspaycode, state.SsCoopControl, ls_assisgroup);
                    Sdt dt = WebUtil.QuerySdt(sql);
                    if (dt.Next())
                    {
                        sqlStr = @"update ASSUCFASSISTPAYTYPE set 
                                assistpay_desc={0}, ASSISTTYPE_GROUP={3} where assistpay_code={1} and coop_id={2}
                                ";
                        sqlStr = WebUtil.SQLFormat(sqlStr, ls_assisdesc, ls_asspaycode, state.SsCoopId, ls_assisgroup);
                        WebUtil.ExeSQL(sqlStr);
                    }
                    else
                    {
                        sqlStr = @"insert into ASSUCFASSISTPAYTYPE 
                            (assistpay_code, assistpay_desc, coop_id,ASSISTTYPE_GROUP)
                            values
                            ({0}, {1}, {2}, {3})";
                        sqlStr = WebUtil.SQLFormat(sqlStr, ls_asspaycode, ls_assisdesc, state.SsCoopId, ls_assisgroup);
                        WebUtil.ExeSQL(sqlStr);
                    }
                }
                dsUcfasspaytype.Retrieve(ls_assiscode);
                dsUcfasspaytype.RetriveGroup();
            }
            catch
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ประเภทการจ่ายสวัสดิการ บันทึกไม่สำเสร็จ");
                return;
            }
            ////////////// tap 3 ////////////////
//            try
//            {
//                string ls_asspay, ls_chkassispaycode = "", Is_membertype, ls_seqno;

//                for (li_row = 1; li_row <= dsUcfasstypedet.RowCount; li_row++)
//                {
//                    Is_membertype = dsUcfasstypedet.DATA[li_row - 1].MEMBTYPE_CODE.ToString().Trim();
//                    ls_seqno = dsUcfasstypedet.DATA[li_row - 1].SEQ_NO.ToString();
//                    if (Is_membertype == "")
//                    {
//                        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('กรุณาเลือกประเภทสมาชิก!!');", true); return;
//                    }
//                    else if (dsUcfasstypedet.DATA[li_row - 1].ASSISTTYPE_PAY.ToString() == "")
//                    {
//                        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('กรุณาเลือกประเภทการจ่ายสวัสดิการ!!');", true); return;
//                    }
//                    else if (ls_chkassispaycode.IndexOf(Is_membertype + dsUcfasstypedet.DATA[li_row - 1].ASSISTTYPE_PAY + ls_seqno) > 0)
//                    {
//                        this.SetOnLoadedScript("alert('เงื่อนไขประเภทการจ่ายซ้ำกัน กรุณาตรวจสอบ!!'); "); return;
//                    }
//                    else if (dsUcfasstypedet.DATA[li_row - 1].MAX_PAYAMT < 1)
//                    {
//                        this.SetOnLoadedScript("alert('กรุณากรอกยอดเงินที่จ่าย!!'); "); return;
//                    }
//                    //else if (dsUcfasstypedet.DATA[li_row].FIRST_PAYAMT < 100)
//                    //{
//                    //    if (dsUcfasstypedet.DATA[li_row].NEXT_PAYAMT < 1)
//                    //    {
//                    //        this.SetOnLoadedScript("alert('กรุณากรอกการจ่ายครั้งต่อไป!!'); "); return;
//                    //    }
//                    //}

//                    ls_chkassispaycode = ls_chkassispaycode + ", " + Is_membertype + dsUcfasstypedet.DATA[li_row - 1].ASSISTTYPE_PAY.ToString() + ls_seqno;

//                }
//                for (li_row = 1; li_row <= dsUcfasstypedet.RowCount; li_row++)
//                {
//                    if (li_row == 1)
//                    {
//                        sqlStr = @"delete from ASSUCFASSISTTYPEDET where coop_id = '" + state.SsCoopId + "' and assisttype_code = '" + ls_assiscode + "'";
//                        WebUtil.ExeSQL(sqlStr);
//                    }
//                    ls_asspay = dsUcfasstypedet.DATA[li_row - 1].ASSISTTYPE_PAY.ToString();
//                    Is_membertype = dsUcfasstypedet.DATA[li_row - 1].MEMBTYPE_CODE.ToString();
//                    ls_seqno = dsUcfasstypedet.DATA[li_row - 1].SEQ_NO.ToString();
//                    decimal ls_unitbalance = dsUcfasstypedet.DATA[li_row - 1].FIRST_TYPEPAY;
//                    decimal ls_unitnextbalance = 0;
//                    ls_unitnextbalance = dsUcfasstypedet.DATA[li_row - 1].NEXT_TYPEPAY;
//                    sqlStr = @"insert into ASSUCFASSISTTYPEDET
//                            (
//                            coop_id, assisttype_code, assisttype_pay, min_check, max_check
//                            , max_payamt, num_pay, first_payamt, next_payamt,membtype_code,seq_no,first_typepay,next_typepay,max_firstpayamt,max_nextpayamt
//                            )
//                            values
//                            (
//                            {0}, {1}, {2}, {3}, {4}
//                            ,{5}, {6}, {7}, {8},{9},{10},{11},{12},{13},{14}
//                            )
//                            ";
//                    sqlStr = WebUtil.SQLFormat(sqlStr, state.SsCoopControl, ls_assiscode, dsUcfasstypedet.DATA[li_row - 1].ASSISTTYPE_PAY, dsUcfasstypedet.DATA[li_row - 1].MIN_CHECK, dsUcfasstypedet.DATA[li_row - 1].MAX_CHECK
//                        , dsUcfasstypedet.DATA[li_row - 1].MAX_PAYAMT, dsUcfasstypedet.DATA[li_row - 1].NUM_PAY, dsUcfasstypedet.DATA[li_row - 1].FIRST_PAYAMT, dsUcfasstypedet.DATA[li_row - 1].NEXT_PAYAMT, Is_membertype, ls_seqno, ls_unitbalance, ls_unitnextbalance, dsUcfasstypedet.DATA[li_row - 1].MAX_FIRSTPAYAMT, dsUcfasstypedet.DATA[li_row - 1].MAX_NEXTPAYAMT);
//                    WebUtil.ExeSQL(sqlStr);

//                }
//                dsUcfasstypedet.RetrieveData(ls_assiscode);
//                dsUcfasstypedet.Membertype();
//            }
//            catch { LtServerMessage.Text = WebUtil.ErrorMessage("เงื่อนไขการจ่ายสวัสดิการ บันทึกไม่สำเสร็จ"); return; }
            /////////////////////////////////////
            LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลสำเร็จ");
        }
        

        public void WebSheetLoadEnd()
        {
            for (int ii = 0; ii < dsUcfasspaytype.RowCount; ii++)
            {
                dsUcfasspaytype.FindDropDownList(ii, "ASSISTTYPE_GROUP").Enabled = false;
            }
        }
    }
}