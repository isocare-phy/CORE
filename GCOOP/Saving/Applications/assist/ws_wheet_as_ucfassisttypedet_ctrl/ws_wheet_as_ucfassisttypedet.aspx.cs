using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using DataLibrary;
using System.Data;

namespace Saving.Applications.assist.ws_wheet_as_ucfassisttypedet_ctrl
{
    public partial class ws_wheet_as_ucfassisttypedet : PageWebSheet, WebSheet
    {
        [JsPostBack]
        public string PostNewRow { get; set; }
        [JsPostBack]
        public string PostDelRow { get; set; }
        [JsPostBack]
        public string PostSeleteData { get; set; }
        [JsPostBack]
        public string PostMaxseqno { get; set; }
        
        public void InitJsPostBack()
        {
            dsMain.InitDsMain(this);
            dsList.InitDsList(this);
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                dsMain.AssistType();//show data first
                dsList.Membertype();
            }            
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == PostNewRow)
            {
                dsList.InsertLastRow();
                decimal seqno = dsList.RowCount;
                int     row   = dsList.RowCount-1;
                dsList.AssistPayType(dsMain.DATA[0].ASSISTTYPE_CODE);
                dsList.DATA[row].SEQ_NO = seqno;
                dsList.Membertype();
                dsList.FindTextBox(row, "next_payamt").Enabled = false;
                dsList.FindDropDownList(row, "next_typepay").Enabled = false;
                dsList.FindTextBox(row, "max_nextpayamt").Enabled = false;               
            }
            else if (eventArg == PostDelRow)
            {
                int row           = dsList.GetRowFocus();
                String ls_typepay = dsList.DATA[row].ASSISTTYPE_PAY;
                String ls_type    = dsMain.DATA[0].ASSISTTYPE_CODE;
                String ls_memtype = dsList.DATA[row].MEMBTYPE_CODE.ToString();
                String ls_seq_no     = dsList.DATA[row].SEQ_NO.ToString();
                try
                {
                    string sql = @"delete from assucfassisttypedet where coop_id = {0} and assisttype_pay={1} and assisttype_code={2} and membtype_code={3} and seq_no={4}";
                    sql = WebUtil.SQLFormat(sql, state.SsCoopId, ls_typepay, ls_type, ls_memtype, ls_seq_no);
                    WebUtil.ExeSQL(sql);
                    dsList.RetrieveData(dsMain.DATA[0].ASSISTTYPE_CODE);
                    dsList.Membertype();
                    LtServerMessage.Text = WebUtil.CompleteMessage("ลบข้อมูลสำเร็จ");
                }
                catch
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ลบข้อมูลไม่สำเสร็จ");
                    return;
                }

            }
            else if (eventArg == PostSeleteData)
            {
                string ls_asscode = dsMain.DATA[0].ASSISTTYPE_CODE;
                GetCalculate(ls_asscode);
                dsList.RetrieveData(ls_asscode);
                dsList.Membertype();                
            }           
        }

        private void GetCalculate(string ls_asscode)
        {
            string sqlStr = @"select 
                            	case calculate_flag when 1 then 'เกรดเฉลี่ย' when 2 then 'อายุ' when 3 then 'อายุการเป็นสมาชิก' when 4 then 'เงินเดือน' when 5 then 'ค่าเสียหาย' when 6 then 'ตามประเภทการจ่าย' else '' end calculate_flag
                        from ASSUCFASSISTTYPE where coop_id = {0} and assisttype_code = {1}";
            sqlStr = WebUtil.SQLFormat(sqlStr, state.SsCoopId, ls_asscode);
            Sdt dt1 = WebUtil.QuerySdt(sqlStr);
            dt1.Next();
            dsMain.DATA[0].calculate_flag = dt1.GetString("calculate_flag");

        }

        public void SaveWebSheet()
        {
            try
            {
                string sqlStr, ls_asstype, ls_asspay, ls_chkassispaycode = "", Is_membertype, ls_seqno;
                int li_row;

                ls_asstype = dsMain.DATA[0].ASSISTTYPE_CODE.ToString();                 
                for (li_row = 0; li_row < dsList.RowCount; li_row++)
                {
                    Is_membertype = dsList.DATA[li_row].MEMBTYPE_CODE.ToString().Trim();
                    ls_seqno = dsList.DATA[li_row].SEQ_NO.ToString();
                    if (Is_membertype == "")
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('กรุณาเลือกประเภทสมาชิก!!');", true); return;
                    }
                    else if (dsList.DATA[li_row].ASSISTTYPE_PAY.ToString()=="")
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('กรุณาเลือกประเภทการจ่ายสวัสดิการ!!');", true); return;
                    }
                    else if (ls_chkassispaycode.IndexOf(Is_membertype + dsList.DATA[li_row].ASSISTTYPE_PAY + ls_seqno) > 0)
                    {
                        this.SetOnLoadedScript("alert('เงื่อนไขประเภทการจ่ายซ้ำกัน กรุณาตรวจสอบ!!'); "); return;
                    }
                    else if (dsList.DATA[li_row].MAX_PAYAMT < 1)
                    {
                        this.SetOnLoadedScript("alert('กรุณากรอกยอดเงินที่จ่าย!!'); "); return;
                    }
                    //else if (dsList.DATA[li_row].FIRST_PAYAMT < 100)
                    //{
                    //    if (dsList.DATA[li_row].NEXT_PAYAMT < 1)
                    //    {
                    //        this.SetOnLoadedScript("alert('กรุณากรอกการจ่ายครั้งต่อไป!!'); "); return;
                    //    }
                    //}

                    ls_chkassispaycode = ls_chkassispaycode + ", " + Is_membertype + dsList.DATA[li_row].ASSISTTYPE_PAY.ToString() + ls_seqno;

                }
                for (li_row = 0; li_row < dsList.RowCount; li_row++)
                {
                    if (li_row == 0)
                    {
                        sqlStr = @"delete from ASSUCFASSISTTYPEDET where coop_id = '" + state.SsCoopId + "' and assisttype_code = '" + ls_asstype + "'";
                        WebUtil.ExeSQL(sqlStr);
                    }
                    
                    ls_asspay     = dsList.DATA[li_row].ASSISTTYPE_PAY.ToString();
                    Is_membertype = dsList.DATA[li_row].MEMBTYPE_CODE.ToString();
                    ls_seqno      = "1";
                    if (li_row >= 1) {
                        string sqlStr2 = @"select 
                            	max(seq_no) as max_seq
                        from ASSUCFASSISTTYPEDET where coop_id = {0} and assisttype_code = {1} and assisttype_pay= {2} and membtype_code = {3} ";
                        sqlStr2 = WebUtil.SQLFormat(sqlStr2, state.SsCoopId, ls_asstype, ls_asspay, Is_membertype);
                        Sdt dt9 = WebUtil.QuerySdt(sqlStr2);
                           dt9.Next();
                           string check1 = dt9.GetString("max_seq");
                           if (check1 != "")
                        {
                            ls_seqno = Convert.ToString(Convert.ToInt32(check1) + 1);
                        }
                        else {
                            ls_seqno = "1";
                        }
                    }
                    
                    decimal ls_unitbalance     = dsList.DATA[li_row].FIRST_TYPEPAY;
                    decimal ls_unitnextbalance = 0;
                    ls_unitnextbalance = dsList.DATA[li_row].NEXT_TYPEPAY; 
                    sqlStr = @"insert into ASSUCFASSISTTYPEDET
                            (
                            coop_id, assisttype_code, assisttype_pay, min_check, max_check
                            , max_payamt, num_pay, first_payamt, next_payamt,membtype_code,seq_no,first_typepay,next_typepay,max_firstpayamt,max_nextpayamt
                            )
                            values
                            (
                            {0}, {1}, {2}, {3}, {4}
                            ,{5}, {6}, {7}, {8},{9},{10},{11},{12},{13},{14}
                            )
                            ";
                    sqlStr = WebUtil.SQLFormat(sqlStr, state.SsCoopControl, ls_asstype, dsList.DATA[li_row].ASSISTTYPE_PAY, dsList.DATA[li_row].MIN_CHECK, dsList.DATA[li_row].MAX_CHECK
                        , dsList.DATA[li_row].MAX_PAYAMT, dsList.DATA[li_row].NUM_PAY, dsList.DATA[li_row].FIRST_PAYAMT, dsList.DATA[li_row].NEXT_PAYAMT, Is_membertype, ls_seqno, ls_unitbalance, ls_unitnextbalance, dsList.DATA[li_row].MAX_FIRSTPAYAMT, dsList.DATA[li_row].MAX_NEXTPAYAMT);
                    WebUtil.ExeSQL(sqlStr);

                }
                dsList.RetrieveData(dsMain.DATA[0].ASSISTTYPE_CODE);
                dsList.Membertype();
                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลสำเร็จ");
            }
            catch { LtServerMessage.Text = WebUtil.ErrorMessage("บันทึกไม่สำเสร็จ"); }
        }

        public void WebSheetLoadEnd()
        {
            for (int i = 0; i < dsList.RowCount; i++)
            {
                if (dsList.DATA[i].NUM_PAY == 1)
                {
                    dsList.FindTextBox(i, "next_payamt").Enabled = false;
                    dsList.FindDropDownList(i, "next_typepay").Enabled = false;
                    dsList.FindTextBox(i, "max_nextpayamt").Enabled = false;
                }                
            }
        }
    }
}