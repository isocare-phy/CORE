using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using DataLibrary;

namespace Saving.Applications.assist.ws_wheet_as_request_ctrl
{
    public partial class ws_wheet_as_request : PageWebSheet, WebSheet
    {
        [JsPostBack]
        public string PostRetriveMain { get; set; }
        [JsPostBack]
        public string PostRetriveBranch { get; set; }
        [JsPostBack]
        public string PostLinkAddress { get; set; }
        [JsPostBack]
        public string PostGetDistrict { get; set; }
        [JsPostBack]
        public string PostGetCurrDistrict { get; set; }
        [JsPostBack]
        public string PostGetCurrPostcode { get; set; }
        [JsPostBack]
        public string PostGetPostcode { get; set; }
        [JsPostBack]
        public string PostTap { get; set; }
        [JsPostBack]
        public string PostGenBaht { get; set; }
        [JsPostBack]
        public string PostGenBaht2 { get; set; }
        [JsPostBack]
        public string PostChageFamily { get; set; }
        [JsPostBack]
        public string PostChageLevel { get; set; }
        [JsPostBack]
        public string PostChkGpa { get; set; }
        [JsPostBack]
        public string PostGetAgeChild { get; set; }
        [JsPostBack]
        public string PostCardPersonChild { get; set; }
        [JsPostBack]
        public string PostCardPerson { get; set; }
        [JsPostBack]
        public string PostChkMoney { get; set; }
        [JsPostBack]
        public string PostInsertRow { get; set; }
        [JsPostBack]
        public string PostDelRow { get; set; }
        [JsPostBack]
        public string PostCalDateTreat { get; set; }
        [JsPostBack]
        public string PostLastYear { get; set; }
        [JsPostBack]
        public string PostCalChildNo { get; set; }
        [JsPostBack]
        public string PostEdit { get; set;}
        
        public string sqlStr;
        public string ls_asstype;

        public void InitJsPostBack()
        {
            dsMain.InitDsMain(this);
            dsAssisttype.InitDsAssisttype(this);
            dsDecease.InitDsDecease(this);
            dsFamilydecease.InitDsFamilydecease(this);
            dsEducation.InitDsEducation(this);
            dsDisaster.InitDsDisaster(this);
            dsPatronize.InitDsPatronize(this);
            dsAmount.InitDsAmount(this);
            dsList.InitDsList(this);
            dsGain.InitDsGain(this);
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)//show data first  
            {
                dsAssisttype.Visible = false;
                dsEducation.Visible = false;
                dsDecease.Visible = false;
                dsDisaster.Visible = false;
                dsPatronize.Visible = false;
                dsFamilydecease.Visible = false;
                dsGain.Visible = false;
                dsPatronize.DATA[0].cal_date = 1;
                dsAssisttype.GetAssYear();

                // แก้ปัญหาถ้าไม่ active dropdown ปี มัน get ค่า 0 มา
                sqlStr = @"select max(ass_year) ass_year from assucfyear where coop_id = {0}";
                sqlStr = WebUtil.SQLFormat(sqlStr, state.SsCoopId);
                Sdt dt1 = WebUtil.QuerySdt(sqlStr);
                dt1.Next();
                dsAssisttype.DATA[0].ass_year = dt1.GetInt32("ass_year");
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == PostRetriveMain)
            {
                RetriveData();
            }
            else if (eventArg == PostRetriveBranch)
            {
                dsAmount.RetrieveBranch(dsAmount.DATA[0].EXPENSE_BANK);
            }
            else if (eventArg == PostLinkAddress)
            {
                string ls_soi = dsDisaster.DATA[0].H_SOI;
                string ls_road = dsDisaster.DATA[0].H_ROAD;
                string ls_province = dsDisaster.DATA[0].H_PROVINCE_CODE;
                string ls_amphur = dsDisaster.DATA[0].H_DISTRICT_CODE;
                string ls_tambol = dsDisaster.DATA[0].H_TAMBOL_CODE;
                string ls_postcode = dsDisaster.DATA[0].H_POSTCODE;

                string ls_addr;
                ls_addr = dsDisaster.DATA[0].H_MEMB_ADDR;
                if (dsDisaster.DATA[0].H_MOO != "" && dsDisaster.DATA[0].H_MOO.Trim() != "-")
                {
                    ls_addr = ls_addr + " หมู่." + dsDisaster.DATA[0].H_MOO.Trim();
                }
                if (dsDisaster.DATA[0].H_village != "" && dsDisaster.DATA[0].H_village.Trim() != "-")
                {
                    ls_addr = ls_addr + "  หมู่บ้าน " + dsDisaster.DATA[0].H_village.Trim();
                }

                dsDisaster.DATA[0].MEMB_ADDR = ls_addr;
                dsDisaster.DATA[0].SOI = ls_soi;
                dsDisaster.DATA[0].ROAD = ls_road;
                dsDisaster.DATA[0].TAMBOL_CODE = ls_tambol;
                dsDisaster.DATA[0].DISTRICT_CODE = ls_amphur;
                dsDisaster.DATA[0].PROVINCE_CODE = ls_province;
                dsDisaster.DATA[0].POSTCODE = ls_postcode;

                dsDisaster.DdProvince();
                dsDisaster.DdDistrict(ls_province);
                dsDisaster.DdTambol(ls_amphur);
                dsDisaster.DATA[0].PROVINCE_CODE = ls_province;
            }
            else if (eventArg == PostGetCurrDistrict)
            {
                dsDisaster.DATA[0].H_DISTRICT_CODE = "";
                dsDisaster.DATA[0].H_TAMBOL_CODE = "";
                dsDisaster.DATA[0].H_POSTCODE = "";
                dsDisaster.DdCurrDistrict(dsDisaster.DATA[0].H_PROVINCE_CODE);
            }
            else if (eventArg == PostGetDistrict)
            {
                dsDisaster.DATA[0].DISTRICT_CODE = "";
                dsDisaster.DATA[0].TAMBOL_CODE = "";
                dsDisaster.DATA[0].POSTCODE = "";
                dsDisaster.DdDistrict(dsDisaster.DATA[0].PROVINCE_CODE);
            }
            else if (eventArg == PostGetCurrPostcode)
            {
                CurrPostcode();
            }
            else if (eventArg == PostGetPostcode)
            {
                Postcode();
            }
            else if (eventArg == PostTap)
            {
                dsEducation.ResetRow();
                dsDecease.ResetRow();
                dsDisaster.ResetRow();
                dsFamilydecease.ResetRow();
                dsPatronize.ResetRow();
                
                GetTap();
            }
            else if (eventArg == PostGenBaht)
            {
                //dsPatronize.DATA[0].cal_date //ถ้าเป็นประเภทอื่น จะส่งเป็น 0
                //GetBaht(dsPatronize.DATA[0].cal_date); // 0 คือ ปกติระบบคำนวณยอดเงินให้
                if (dsPatronize.DATA[0].cal_date != 0)
                {
                    GetBaht(Convert.ToInt32(hdCalDay.Value));
                }
                else {
                    GetBaht(dsPatronize.DATA[0].cal_date);
                }
            }
            else if (eventArg == PostGenBaht2)
            {
                //dsPatronize.DATA[0].cal_date //ถ้าเป็นประเภทอื่น จะส่งเป็น 0
                GetBaht(0); // 0 คือ ปกติระบบคำนวณยอดเงินให้
                //GetBaht(Convert.ToInt32(hdCalDay.Value));
            }
            else if (eventArg == PostChageFamily)
            {
                chkFamiyReq();
                GetBaht(0); //0 คือ ปกติระบบคำนวณยอดเงินให้
            }
            else if (eventArg == PostChageLevel)
            {
                ls_asstype = dsAssisttype.DATA[0].assisttype_code;
                string ls_level = dsEducation.DATA[0].child_level;
                
                if(state.SsCoopControl == "031001"){

                    sqlStr = @"select case  when sum(ss) is null then 0 
				                            else sum(ss) end as check1 from (
                                     select  case 
                                        when assistpay_code={3} then 1 
	                                    else 0 
                                       end as ss
                                from assreqmaster where coop_id = {0} and member_no = {1} and assisttype_code = {2})";
                    sqlStr = WebUtil.SQLFormat(sqlStr, state.SsCoopId, dsMain.DATA[0].member_no, dsAssisttype.DATA[0].assisttype_code, ls_level);
                    Sdt dt1 = WebUtil.QuerySdt(sqlStr);
                    if (dt1.Next())
                    {
                        Int32 ls_checker1 = Int32.Parse( dt1.GetString("check1"));
                        if (ls_checker1 >= 1) { 
                        LtServerMessage.Text = WebUtil.ErrorMessage("สมาชิกท่านนี้ " + dsMain.DATA[0].member_no + " ได้ขอทุนการศึกษาบุตรในระดับชั้นนี้ไปแล้ว");
                        dsAmount.DATA[0].APPROVE_AMT = 0;
                        dsAmount.DATA[0].system_amt = 0;
                            return;
                        }
                    }
                }
                GetBaht(0); //0 คือ ปกติระบบคำนวณยอดเงินให้
                if (dsEducation.DATA[0].CHILD_GPA > 0)
                {
                    chkGpa(ls_asstype, ls_level, dsEducation.DATA[0].CHILD_GPA);
                }
            }
            else if (eventArg == PostChkGpa)
            {
                if (dsEducation.DATA[0].CHILD_GPA > 0)// ถ้าเป็น 0 หรือเข้ารอบแรก ไม่ต้อง check *** เป็น 0 check ตอน save แล้ว
                {
                    ls_asstype = dsAssisttype.DATA[0].assisttype_code;
                    chkGpa(ls_asstype, dsEducation.DATA[0].child_level, dsEducation.DATA[0].CHILD_GPA);
                }
            }
            else if (eventArg == PostGetAgeChild)
            {
                GetAgeChild();
            }
            else if (eventArg == PostCardPersonChild)
            {
                int li_year = dsAssisttype.DATA[0].ass_year;
                //string sql = @"select document_year from cmdocumentcontrol where document_code = 'ASSISTDOCNO'";
                //sql = WebUtil.SQLFormat(sql);
                //Sdt dt = WebUtil.QuerySdt(sql);
                //if (dt.Next())
                //{
                //    li_year = dt.GetInt32("document_year");
                //}

                sqlStr = @"select to_char(req_date, 'dd/mm/') || cast(to_char(req_date, 'yyyy') + 543 as varchar(4)) req_date, assreqmaster.* 
                        from assreqmaster 
                        where req_status > 0 and coop_id = {0} and assist_year = {1} + 543 and child_card_person = {2} and assisttype_code = {3}";
                sqlStr = WebUtil.SQLFormat(sqlStr, state.SsCoopControl, li_year, dsEducation.DATA[0].CHILD_CARD_PERSON, dsAssisttype.DATA[0].assisttype_code);
                Sdt dt1 = WebUtil.QuerySdt(sqlStr);

                if (dt1.Next())
                {
                    string ls_pername = "";
                    if (dsEducation.DATA[0].CHILD_PRENAME_CODE == "01")
                    {
                        ls_pername = "นาย";
                    }
                    else if (dsEducation.DATA[0].CHILD_PRENAME_CODE == "03")
                    {
                        ls_pername = "นางสาว";
                    }
                    else if (dsEducation.DATA[0].CHILD_PRENAME_CODE == "08")
                    {
                        ls_pername = "ว่าที่ร้อยตรี";
                    }
                    else if (dsEducation.DATA[0].CHILD_PRENAME_CODE == "49")
                    {
                        ls_pername = "ด.ช.";
                    }
                    else if (dsEducation.DATA[0].CHILD_PRENAME_CODE == "50")
                    {
                        ls_pername = "ด.ญ.";
                    }

                    LtServerMessage.Text = WebUtil.ErrorMessage(ls_pername + dsEducation.DATA[0].CHILD_NAME + "  " + dsEducation.DATA[0].CHILD_SURNAME + " ได้ขอสวัสดิการประเภทนี้ไปแล้วเมื่อวันที่ " + dt1.GetString("req_date") + " ของเลขสมาชิก " + dt1.GetString("member_no"));
                    return;
                }
            }
            else if (eventArg == PostCardPerson)
            {
                int li_year = dsAssisttype.DATA[0].ass_year;
                //string sql = @"select document_year from cmdocumentcontrol where document_code = 'ASSISTDOCNO'";
                //sql = WebUtil.SQLFormat(sql);
                //Sdt dt = WebUtil.QuerySdt(sql);
                //if (dt.Next())
                //{
                //    li_year = dt.GetInt32("document_year");
                //}
                ////// ตรวจสอบว่าเป็นสมาชิกของสหกรณ์หรือไม่ //////////////
                sqlStr = @"select * from mbmembmaster where coop_id = {0} and card_person = {1}";
                sqlStr = WebUtil.SQLFormat(sqlStr, state.SsCoopId, dsFamilydecease.DATA[0].family_card_person);
                Sdt dt1 = WebUtil.QuerySdt(sqlStr);
                if (dt1.Next())
                {
                    dsFamilydecease.DATA[0].family_card_person = "";
                    LtServerMessage.Text = WebUtil.ErrorMessage("เลขบัตรบัตรประชาชน " + dsFamilydecease.DATA[0].family_card_person + " เป็นของเลขสมาชิก " + dt1.GetString("member_no") + " ซึ่งสมาชิกจะไม่สามารถขอสวัสดิการประเภทที่ได้");
                    return;
                }
                //////////////////////////////////////////////////

                sqlStr = @"select to_char(req_date, 'dd/mm/') || cast(to_char(req_date, 'yyyy') + 543 as varchar(4)) req_date, assreqmaster.* 
                        from assreqmaster 
                        where req_status > 0 and coop_id = {0} and assist_year = {1} + 543 and family_card_person = {2} and assisttype_code = {3}";
                sqlStr = WebUtil.SQLFormat(sqlStr, state.SsCoopControl, li_year, dsFamilydecease.DATA[0].family_card_person, dsAssisttype.DATA[0].assisttype_code);
                dt1 = WebUtil.QuerySdt(sqlStr);
                if (dt1.Next())
                {
                    dsFamilydecease.DATA[0].family_card_person = "";
                    LtServerMessage.Text = WebUtil.ErrorMessage(dsFamilydecease.DATA[0].familydead_name + " ได้ขอสวัสดิการประเภทนี้ไปแล้วเมื่อวันที่ " + dt1.GetString("req_date") + " ของเลขสมาชิก " + dt1.GetString("member_no"));
                    return;
                }
            }
            else if (eventArg == PostChkMoney)
            {
                GetBaht(99); // 99 user คีย์หน้าจอ
            }
            else if (eventArg == PostInsertRow)
            {
                dsGain.InsertLastRow();
                dsGain.RetrievePrename();
                dsGain.GainRealtion();
                for (int ii = 0; ii < dsGain.RowCount; ii++)
                {
                    if (ii + 1 == dsGain.RowCount)
                    {
                        dsGain.DATA[ii].seq_no = (ii + 1).ToString();
                        dsGain.DATA[ii].gainprename_code = "01"; // มันไม่ใส่ค่าเพราะไม่ได้ active
                        dsGain.DATA[ii].ASSISTPAY_CODE = "00";// มันไม่ใส่ค่าเพราะไม่ได้ active
                    }
                }
            }
            else if (eventArg == PostDelRow)
            {
                int row = dsGain.GetRowFocus();
                dsGain.DeleteRow(row);
                dsGain.RetrievePrename();
                dsGain.GainRealtion();
            }
            else if (eventArg == PostCalDateTreat)
            {
                CelTreatDate();
                GetBaht(Convert.ToInt32(hdCalDay.Value));
            }
            else if (eventArg == PostLastYear)
            {
                sqlStr = @"select * from assucfyear where coop_id = {0} and ass_year = {1}";
                sqlStr = WebUtil.SQLFormat(sqlStr, state.SsCoopId, dsAssisttype.DATA[0].ass_year);
                Sdt dt1 = WebUtil.QuerySdt(sqlStr);
                dt1.Next();
                dsPatronize.DATA[0].start_treat = dt1.GetDate("end_year");
                dsPatronize.DATA[0].end_treat = dt1.GetDate("end_year");
                CelTreatDate();
                GetBaht(Convert.ToInt32(hdCalDay.Value));
            }
            else if (eventArg == PostCalChildNo) {
                dsEducation.DATA[0].child_no = dsEducation.DATA[0].child_no_work + dsEducation.DATA[0].child_no_study;
            }
            else if (eventArg == PostEdit){
                string ls_assistdocno = dsMain.DATA[0].assist_docno;
                sqlStr = @"select * from assreqmaster where coop_id = {0} and assist_docno = {1}";
                sqlStr = WebUtil.SQLFormat(sqlStr, state.SsCoopControl, ls_assistdocno);
                Sdt dt1 = WebUtil.QuerySdt(sqlStr);
                if (dt1.Next())
                {
                    dsMain.DATA[0].member_no = dt1.GetString("member_no");
                    //dsAssisttype.DATA[0].assisttype_code = dt1.GetString("assisttype_code");
                    
                }
                //
                RetriveData();
                dsAssisttype.DATA[0].assisttype_code = dt1.GetString("assisttype_code");
                dsDecease.Retrieve2(dsMain.DATA[0].member_no, ls_assistdocno);
                dsFamilydecease.Retrieve2(dsMain.DATA[0].member_no, ls_assistdocno);
                dsEducation.Retrieve2(dsMain.DATA[0].member_no, ls_assistdocno);
                dsDisaster.Retrieve2(dsMain.DATA[0].member_no, ls_assistdocno);
                dsPatronize.Retrieve2(dsMain.DATA[0].member_no, ls_assistdocno);
                //dsAmount.Retrieve2(dsMain.DATA[0].member_no, ls_assistdocno);
                GetTap();
                GetBaht(0);
                dsAmount.Retrieve3(dsMain.DATA[0].member_no, ls_assistdocno, dsAmount.DATA[0].system_amt);
            }
        }

        

        private void GetBaht(int li_chkButton)
        {
            //li_chkButton = 0      คือ ปกติระบบคำนวณยอดเงินให้
            //li_chkButton = 99      คือ มีการเปลี่ยนแปลงจาก user โดยการคีย์จากหน้าจอ
            //li_chkButton = อื่นๆ    คือ เอาวันเป็นตำคำนวณ

            string ls_assisgroup = "00", ls_use = "00";
            string ls_assiscode = dsAssisttype.DATA[0].assisttype_code; ;
            string ls_memtypecode = dsMain.DATA[0].membtype_desc.Substring(0, 2);
            string[] ls_arr_agemem = dsMain.DATA[0].age_member.Split('.');
            string[] ls_arr_mem = dsMain.DATA[0].age.Split('.');
            int li_calflag = 0, li_agemem = 0, li_memtypeflag = 1;
            decimal dec_amount = 0;

            sqlStr = @"select * from assucfassisttype where coop_id = {0} and assisttype_code = {1}";
            sqlStr = WebUtil.SQLFormat(sqlStr, state.SsCoopControl, ls_assiscode);
            Sdt dt1 = WebUtil.QuerySdt(sqlStr);

            if (dt1.Next())
            {
                ls_assisgroup = dt1.GetString("assisttype_group");
                li_calflag = dt1.GetInt32("calculate_flag");
                li_memtypeflag = dt1.GetInt32("membtype_flag");
            }

            if (li_memtypeflag == 0) { ls_memtypecode = "%"; } // ไม่เช็ด membtype_code

            if (dsEducation.DATA[0].child_level != "") //tab 1
            {
                ls_use = dsEducation.DATA[0].child_level;
            }
            else if (dsFamilydecease.DATA[0].ASSISTPAY_CODE != "") //tab 3
            {
                ls_use = dsFamilydecease.DATA[0].ASSISTPAY_CODE;
            }
            else if (dsDisaster.DATA[0].disaster_type != "") // tab 4
            {
                ls_use = dsDisaster.DATA[0].disaster_type;
            }
            else if (dsPatronize.DATA[0].senile_type != "") // tab 5
            {
                ls_use = dsPatronize.DATA[0].senile_type;
            }
            if (ls_assisgroup == "02") // tab 2 fix สมาชิกเสียชีวิต
            {
                string sql_fix = @" select * from assucfassistpaytype where assistpay_desc like '%ตนเอง%' and assisttype_group = {0}";
                sql_fix = WebUtil.SQLFormat(sql_fix, ls_assisgroup);
                Sdt dt_fix = WebUtil.QuerySdt(sql_fix);
                dt_fix.Next();
                ls_use = dt_fix.GetString("ASSISTPAY_CODE"); ;
            }


            if (li_calflag == 1) //เกรด
            {
            }
            else if (li_calflag == 2) //อายุ
            {
                li_agemem = (Convert.ToInt32(ls_arr_mem[0]) * 12) + Convert.ToInt32(ls_arr_mem[1]); //อายุสมาชิก
            }
            else if (li_calflag == 3) //อายุการเป็นสมาชิก
            {
                if (ls_assiscode != "00")
                {
                    if (ls_assisgroup == "02") // tab 2 fix สมาชิกเสียชีวิต
                    {
                        string ls_dead = (dsDecease.DATA[0].DEAD_DATE).ToString("dd/MM/yyyy");
                        ls_dead = ls_dead.Substring(0, 6) + ls_dead.Substring(6, 4);
                        string sql = @" select member_date, FT_CALAGE(member_date ,to_date({0},'dd/mm/yyyy'), 4) as age_member from mbmembmaster where member_no = {1}";
                        sql = WebUtil.SQLFormat(sql, ls_dead, dsMain.DATA[0].member_no);
                        Sdt dt_age = WebUtil.QuerySdt(sql);
                        dt_age.Next();
                        ls_arr_agemem = dt_age.GetString("age_member").Split('.');
                    }
                    li_agemem = (Convert.ToInt32(ls_arr_agemem[0]) * 12) + Convert.ToInt32(ls_arr_agemem[1]); //อายุการเป็นสมาชิก
                }
                else
                {
                    li_agemem = 0;// รอบแรกเท่านั้น
                }
            }
            else if (li_calflag == 4) //เงินเดือน
            {
                if (ls_use != "00")
                {
                    sqlStr = @"select 
	                        assucfassisttypedet.min_check ,
	                        assucfassisttypedet.max_check ,
                            to_char(assucfassisttypedet.max_check, '999,999.00') show
                        from assucfassisttype
                        inner join assucfassisttypedet on assucfassisttype.assisttype_code = assucfassisttypedet.assisttype_code
                        where assucfassisttype.coop_id = {0} and
                        assucfassisttype.assisttype_code = {1} and
                        assucfassisttypedet.assisttype_pay = {2}";
                    sqlStr = WebUtil.SQLFormat(sqlStr, state.SsCoopControl, ls_assiscode, ls_use);
                    dt1 = WebUtil.QuerySdt(sqlStr);
                    dt1.Next();
                    if (dsMain.DATA[0].salary_amount >= dt1.GetDecimal("max_check"))
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage("สวัสดิการประเภทนี้มีเงื่อนไขเงินเดือนห้ามสูงกว่า " + dt1.GetDecimal("show") + " บาท");
                        dsAmount.DATA[0].APPROVE_AMT = 0;
                        return;
                    }
                }
            }
            else if (li_calflag == 5) //ค่าเสียหาย
            {
            }
            else if (li_calflag == 6) //ตามประเภทการจ่าย
            {
            }

            sqlStr = "select " +
                "   max_seqno," +
                "   assucfassisttypedet.* " +
                " from assucfassisttype " +
                " inner join assucfassisttypedet on assucfassisttype.assisttype_code = assucfassisttypedet.assisttype_code " +
                " inner join (" +
                " select assisttype_code, max(seq_no) max_seqno " +
                " from assucfassisttypedet " +
                " where coop_id = '" + state.SsCoopControl + "' and assisttype_code = '" + ls_assiscode + "' and membtype_code like '" + ls_memtypecode + "' group by assisttype_code " +
                " )chk_seq on assucfassisttypedet.assisttype_code = chk_seq.assisttype_code " +
                " where assucfassisttype.coop_id = '" + state.SsCoopControl + "'" +
                " and assucfassisttype.assisttype_code = '" + ls_assiscode + "'" +
                " and assucfassisttypedet.membtype_code like '" + ls_memtypecode + "'" +
                " and assucfassisttypedet.assisttype_pay = '" + ls_use + "'";
            Sdt dt2 = WebUtil.QuerySdt(sqlStr);
            if (dt2.Next())
            {
                if (dt2.GetInt32("max_seqno") > 1)
                {
                    if (li_calflag == 2 || li_calflag == 3) // ใช้อายุหรือ อายุสมาชิกคำนวณ
                    {
                        for (int ii = 0; ii < dt2.GetInt32("max_seqno"); ii++)
                        {
                            if (dt2.GetInt32("min_check") <= li_agemem && li_agemem < dt2.GetInt32("max_check"))
                            {
                                if (dt2.GetInt32("first_typepay") == 1) //บาท
                                {
                                    dec_amount = dt2.GetInt32("max_payamt");
                                }
                                else // เปอร์เซ็น
                                {
                                    dec_amount = (dt2.GetInt32("max_payamt") * dt2.GetInt32("first_payamt")) / 100;
                                }
                                dsAmount.DATA[0].system_amt = dt2.GetInt32("max_payamt");
                                goto next_step;
                            }
                            dt2.Next();
                        }
                    }
                }
                else
                {
                    if (dt2.GetInt32("first_typepay") == 1) //บาท
                    {
                        if (ls_assisgroup == "06")//fix รักษาพยาบาล tap 5
                        {
                            dec_amount = dt2.GetInt32("first_payamt") * li_chkButton;
                        }
                        else
                        {
                            dec_amount = dt2.GetInt32("max_payamt");
                        }
                    }
                    else // เปอร์เซ็น
                    {
                        dec_amount = (dt2.GetInt32("max_payamt") * dt2.GetInt32("first_payamt")) / 100;
                    }
                    dsAmount.DATA[0].system_amt = dt2.GetInt32("max_payamt");
                }
            }
            else
            {
                dsAmount.DATA[0].system_amt = 0;
                dsAmount.DATA[0].ever_amt = 0;
            }
        next_step:
            if (li_chkButton == 99) //user คีย์จากหน้าจอ
            {
                if (dsAmount.DATA[0].APPROVE_AMT > dec_amount)
                {
                    if ((ls_assiscode == "21" || ls_assiscode == "22") && (state.SsCoopId == "031001" || state.SsCoopId == "031002"))
                    {
                        ;
                    }
                    else
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage("ยอดเงินที่กรอกมีจำนวนมากกว่าค่าที่กำหนด กรุณาตรวจสอบ");
                        dsAmount.DATA[0].APPROVE_AMT = dec_amount;
                        return;
                    }
                }
            }
            else // คำนวณจากระบบ
            {
                if (dec_amount > dt2.GetDecimal("max_firstpayamt")) //ตรวจสอบยอดจ่ายสูงสุดในครั้งแรก
                {
                

                        dsAmount.DATA[0].APPROVE_AMT = dt2.GetDecimal("max_firstpayamt");

                }
                else
                {
                    dsAmount.DATA[0].APPROVE_AMT = dec_amount;
                }
                if (ls_assisgroup == "02") //fix สมาชิกเสียชีวิต tap 2
                {
                    if (dsAmount.DATA[0].APPROVE_AMT > 0)
                    {
                        dsAmount.DATA[0].APPROVE_AMT = dsAmount.DATA[0].APPROVE_AMT - dsAmount.DATA[0].ever_amt;
                    }
                }
            }
        }

        private void GetTap()
        {
            
            if (hdActMemno.Value != "chk2") { RetriveData(); }

            if (state.SsCoopControl == "031001")
            {
                if (dsAssisttype.DATA[0].assisttype_code == "21" || dsAssisttype.DATA[0].assisttype_code == "22")
                {
                    
                    //dsAmount.DATA[0].MONEYTYPE_CODE = "TRN";
                    //dsAmount.DATA[0].send_system = "DEP";
                    sqlStr = @"select bank_accid,moneytype_code,bank_code,bank_branch from mbmembmoneytr where coop_id ={0} and member_no ={1} and trtype_code = 'DVAV1' ";
                    sqlStr = WebUtil.SQLFormat(sqlStr, state.SsCoopId, dsMain.DATA[0].member_no);
                    Sdt dt5 = WebUtil.QuerySdt(sqlStr);
                    if (dt5.Next())
                    {
                        dsAmount.DATA[0].MONEYTYPE_CODE = dt5.GetString("moneytype_code");
                        if (dsAmount.DATA[0].MONEYTYPE_CODE == "TRN")
                        {
                            dsAmount.DATA[0].DEPTACCOUNT_NO = dt5.GetString("bank_accid");
                            dsAmount.DATA[0].send_system = "DEP";
                            dsAmount.DATA[0].EXPENSE_BANK = "";
                            dsAmount.DATA[0].EXPENSE_ACCID = "";
                            dsAmount.DATA[0].EXPENSE_BRANCH = "";
                            dsAmount.FindDropDownList(0, "deptaccount_no").Enabled = true;
                        }
                        else {
                            dsAmount.DATA[0].EXPENSE_BANK = dt5.GetString("bank_code");
                            dsAmount.DATA[0].EXPENSE_ACCID = dt5.GetString("bank_accid");
                            dsAmount.DATA[0].EXPENSE_BRANCH = dt5.GetString("bank_branch");
                        }
                    }
                    if (dsAmount.DATA[0].EXPENSE_BANK == "0") {
                        dsAmount.DATA[0].EXPENSE_BANK = "";
                    }
                    if (dsAmount.DATA[0].EXPENSE_BRANCH == "0") {
                        dsAmount.DATA[0].EXPENSE_BRANCH = "";
                    }
                }
            }

            string ls_chktap = "00", ls_asstype, ls_acttap = "0", ls_memno;
            int li_baht = 0;
            ls_memno = dsMain.DATA[0].member_no;
            ls_asstype = dsAssisttype.DATA[0].assisttype_code;
            
            sqlStr = @"select * from ASSUCFASSISTTYPE where assisttype_code = {0}";
            sqlStr = WebUtil.SQLFormat(sqlStr, ls_asstype);
            Sdt dt = WebUtil.QuerySdt(sqlStr);

            if (dt.Next())
            {
                ls_chktap = dt.GetString("assisttype_group");
            }
            if (ls_chktap == "01" || ls_chktap == "02" || ls_chktap == "03" || ls_chktap == "04" || ls_chktap == "05" || ls_chktap == "06")
            {
                dsList.RetrieveReqAssist(ls_memno, ls_asstype);
            }
            // ตรวจสอบการขอไปแล้วในปีนี้ หรือเดือนนี้
            sqlStr = @"select
	                    to_char(req.req_date, 'yyyymm') req_month,
	                    to_char({4}, 'yyyymm') req_now,
	                    req.assist_year,
	                    req.assisttype_code,
	                    case when assucfassisttype.process_flag = 2 then 2 else assucfassisttype.process_flag end process_flag,
	                    typedet.num_pay
                    from (select * from assreqmaster where req_status = 1) req
                    inner join (select distinct assisttype_code,num_pay from assucfassisttypedet) typedet on req.assisttype_code = typedet.assisttype_code
                    inner join assucfassisttype on assucfassisttype.assisttype_code = typedet.assisttype_code
                    where req.coop_control = {0}
                    and req.assist_year = {1} + 543
                    and req.member_no = {2}
                    and req.assisttype_code = {3}";
            sqlStr = WebUtil.SQLFormat(sqlStr, state.SsCoopControl, dsAssisttype.DATA[0].ass_year, ls_memno, ls_asstype, state.SsWorkDate);
            Sdt dt1 = WebUtil.QuerySdt(sqlStr);
            if (dt1.Next())
            {
               
                if (dt1.GetInt32("process_flag") == 2)//ปี
                {
                    //if (ls_chktap != "03")
                    //{
                        //if ((state.SsCoopId == "031001" || state.SsCoopId == "031002") && ls_chktap == "06") { }
                        //if (dt1.GetRowCount()<dt1.GetInt32("num_pay")){}
                        //else
                        //{
                            LtServerMessage.Text = WebUtil.ErrorMessage("สมาชิกท่านนี้ " + dsMain.DATA[0].member_no + " ได้ขอสวัสดิการประเภทนี้ ในปีนี้ไปแล้ว");
                            dsEducation.Visible = false;
                            dsDecease.Visible = false;
                            dsFamilydecease.Visible = false;
                            dsDisaster.Visible = false;
                            dsPatronize.Visible = false;
                            dsGain.Visible = false;
                            return;
                        //}
                    //}
                }
                else if (dt1.GetInt32("process_flag") == 1) // เดือน
                {
                    for (int ii = 0; ii < dt1.GetRowCount(); ii++)
                    {
                        if (dt1.GetInt32("req_month") >= dt1.GetInt32("req_now"))
                        {
                            LtServerMessage.Text = WebUtil.ErrorMessage("สมาชิกท่านนี้ " + dsMain.DATA[0].member_no + " ได้ขอสวัสดิการประเภทนี้ ในเดือนนี้ไปแล้ว");
                            dsEducation.Visible = false;
                            dsDecease.Visible = false;
                            dsFamilydecease.Visible = false;
                            dsDisaster.Visible = false;
                            dsPatronize.Visible = false;
                            dsGain.Visible = false;
                            return;
                        }
                        dt1.Next();
                    }
                }
                else {
                    if (dt1.GetRowCount() >= dt1.GetInt32("num_pay"))
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage("สมาชิกท่านนี้ " + dsMain.DATA[0].member_no + " ได้ขอสวัสดิการประเภทนี้ ครบแล้ว");
                        dsEducation.Visible = false;
                        dsDecease.Visible = false;
                        dsFamilydecease.Visible = false;
                        dsDisaster.Visible = false;
                        dsPatronize.Visible = false;
                        dsGain.Visible = false;
                        return;
                    }
                
                
                }

            }
            /////////////////////////////////

            

            if (ls_chktap == "01")
            {
                string[] ls_arr_agemem = dsMain.DATA[0].age_member.Split('.');
                if (Int32.Parse(ls_arr_agemem[0]) < 1 && state.SsCoopControl == "031001")
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("สมาชิกท่านนี้ " + dsMain.DATA[0].member_no + " อายุสมาชิกไม่ถึง 1 ปี");
                    dsEducation.Visible = false;
                    return;
                }
                dsEducation.Visible = true;
                if (dsEducation.DATA[0].ASSIST_DOCNO == "")
                {
                    dsEducation.DATA[0].REQ_DATE = state.SsWorkDate;
                }
                string[] name=dsMain.DATA[0].namesurname.Split(' ');

                dsEducation.DATA[0].CHILD_SURNAME = name[3];
                dsDecease.Visible = false;
                dsGain.Visible = false;
                dsFamilydecease.Visible = false;
                dsDisaster.Visible = false;
                dsPatronize.Visible = false;
                ls_acttap = "0";
            }
            else if (ls_chktap == "02")
            {
                dsEducation.Visible = false;
                dsDecease.Visible = true;
                dsGain.Visible = true;
                ////////ตรวจสอบว่าเคยขอสวัสดิการนี้ไปละยัง
                sqlStr = @"select * from assreqmaster 
                    where req_status = 1 and coop_control = {0} and member_no = {1} and assisttype_code = {2}";
                sqlStr = WebUtil.SQLFormat(sqlStr, state.SsCoopControl, ls_memno, ls_asstype);
                dt = WebUtil.QuerySdt(sqlStr);
                if (dt1.Next())
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ls_memno + " ได้ขอสวัสดิการประเภทนี้ไปแล้ว");
                    return;
                }
                //////////////////////////////
                if (dsDecease.DATA[0].ASSIST_DOCNO == "")
                {
                    dsDecease.DATA[0].REQ_DATE = state.SsWorkDate;
                    dsGain.RetrieveGainMb(ls_memno); // get จาก mb
                    dsGain.RetrievePrename();
                    dsGain.GainRealtion();

                    // แก้ปัญหา init ข้อมูลผู้รับผลประโยชน์แล้วเป็นค่า null เพราะต้องทำการ active มันก่อนถึงมีค่า
                    string sql = @"select
        	                seq_no,
        	                gain_relation 
                        from mbgainmaster
                        where coop_id = {0} and member_no = {1}
                        order by seq_no";
                    sql = WebUtil.SQLFormat(sql, state.SsCoopControl, ls_memno);
                    dt = WebUtil.QuerySdt(sql);
                    for (int ii = 0; ii < dsGain.RowCount; ii++)
                    {
                        dt.Next();
                        dsGain.DATA[ii].gainprename_code = "01"; // หน้าจอผู้รับผลประโยชน์ในระบบสมาชิกและหุ้นไม่มีคำนำหน้า
                        dsGain.DATA[ii].ASSISTPAY_CODE = dt.GetString("gain_relation");
                    }
                    /////////////////////////////////
                }
                else// เคยมีการขอแล้วแต่ยังไม่ได้ อนุมัติ
                {
                    dsGain.ResetRow();
                    dsGain.RetrieveGainAss(ls_memno); // get จาก ass
                    dsGain.RetrievePrename();
                    dsGain.GainRealtion();
                }
                dsFamilydecease.Visible = false;
                dsDisaster.Visible = false;
                dsPatronize.Visible = false;
                ls_acttap = "1";
            }
            else if (ls_chktap == "03")
            {
                dsEducation.Visible = false;
                dsDecease.Visible = false;
                dsGain.Visible = false;
                dsFamilydecease.Visible = true;
                if (dsFamilydecease.DATA[0].ASSIST_DOCNO == "")
                {
                    dsFamilydecease.DATA[0].REQ_DATE = state.SsWorkDate;
                }
                dsDisaster.Visible = false;
                dsPatronize.Visible = false;
                ls_acttap = "2";
            }
            else if (ls_chktap == "04")
            {
                dsEducation.Visible = false;
                dsDecease.Visible = false;
                dsGain.Visible = false;
                dsFamilydecease.Visible = false;
                dsDisaster.Visible = true;
                if (dsDisaster.DATA[0].ASSIST_DOCNO == "")
                {
                    dsDisaster.DATA[0].REQ_DATE = state.SsWorkDate;
                }
                dsPatronize.Visible = false;
                ls_acttap = "3";
            }
            else if (ls_chktap == "05")
            {
                dsEducation.Visible = false;
                dsDecease.Visible = false;
                dsGain.Visible = false;
                dsFamilydecease.Visible = false;
                dsDisaster.Visible = false;
                dsPatronize.Visible = true;
                if (dsPatronize.DATA[0].ASSIST_DOCNO == "")
                {
                    dsPatronize.DATA[0].REQ_DATE = state.SsWorkDate;
                }
                dsPatronize.FindTextBox(0, "start_treat").Enabled = false; //ใช้ Visible ไม่ได้ โปรแกรมไม่ get ค่า dropdown มาใช้ เลยใช้ Enabled ไปก่อน
                dsPatronize.FindTextBox(0, "end_treat").Enabled = false;
                dsPatronize.FindTextBox(0, "cal_date").Enabled = false;
                ls_acttap = "4";
            }
            else if (ls_chktap == "06") //เฉพาะรักษาพายาบาล
            {
                dsEducation.Visible = false;
                dsDecease.Visible = false;
                dsGain.Visible = false;
                dsFamilydecease.Visible = false;
                dsDisaster.Visible = false;
                dsPatronize.Visible = true;
                if (dsPatronize.DATA[0].ASSIST_DOCNO == "")
                {
                    dsPatronize.DATA[0].REQ_DATE = state.SsWorkDate;
                    dsPatronize.DATA[0].cal_date = 1; //เฉพาะรอบแรก
                    dsPatronize.DATA[0].start_treat = state.SsWorkDate;
                    dsPatronize.DATA[0].end_treat = state.SsWorkDate;
                }
                CelTreatDate();
                if (hdCalDay.Value != "")
                {
                    li_baht = Convert.ToInt32(hdCalDay.Value);
                }
                else
                {
                    li_baht = 1;
                }
                dsPatronize.FindTextBox(0, "start_treat").Enabled = true; //ใช้ Visible ไม่ได้ โปรแกรมไม่ get ค่า dropdown มาใช้ เลยใช้ Enabled ไปก่อน
                dsPatronize.FindTextBox(0, "end_treat").Enabled = true;
                dsPatronize.FindTextBox(0, "cal_date").Enabled = true;
                ls_acttap = "4";
            }

            hdTabIndex.Value = ls_acttap;

            //-------------- เช็ดประเภทสมาชิกว่ามีสิทธิ์ในสวัสดิการนี้มั้ย -----------------
            string ls_memtypecode = dsMain.DATA[0].membtype_desc.Substring(0, 2);

            sqlStr = @"select 
	                        distinct(membtype_code) member_type ,
	                        membtype_flag
                        from assucfassisttype 
                        inner join assucfassisttypedet on assucfassisttype.assisttype_code = assucfassisttypedet.assisttype_code 
                        where assucfassisttype.coop_id = {0} and assucfassisttype.assisttype_code = {1}";
            sqlStr = WebUtil.SQLFormat(sqlStr, state.SsCoopControl, ls_asstype);
            dt1 = WebUtil.QuerySdt(sqlStr);

            if (dt1.Next())
            {
                if (dt.GetInt32("membtype_flag") == 0) { goto next_step; } //ไม่ต้องเช็ด membtype_code
                for (int ii = 0; ii < dt1.GetRowCount(); ii++)
                {
                    if (ls_memtypecode == dt1.GetString("member_type"))
                    {
                        goto next_step;
                    }
                    dt1.Next();
                }
                LtServerMessage.Text = WebUtil.ErrorMessage("สมาชิกท่านนี้ " + dsMain.DATA[0].member_no + " ไม่มีสิทธิ์ในสวัสดิการประเภทนี้");
                dsAssisttype.DATA[0].assisttype_code = "00";
                dsEducation.Visible = false;
                dsDecease.Visible = false;
                dsFamilydecease.Visible = false;
                dsDisaster.Visible = false;
                dsPatronize.Visible = false;
                dsGain.Visible = false;
                return;
            }
        next_step: ;
            //GetBaht(li_baht);//0 คือ ปกติระบบคำนวณยอดเงินให้
            //-------------------------------------------------------------
        }

        private void RetriveData()
        {
            string ls_memno = WebUtil.MemberNoFormat(dsMain.DATA[0].member_no);
            dsAssisttype.AssistType();

            string sql = @"select  * from
                (
	                select 
		                count(*) chk_mem, resign_status, resigncause_code as dead_status
	                from mbmembmaster where coop_id = {0} and member_no = {1} group by resign_status, resigncause_code
	                union
	                select 
		                0, 0, '0' 
	                from mbmembmaster where rownum = 1
                )
                order by chk_mem desc";
            sql = WebUtil.SQLFormat(sql, state.SsCoopId, ls_memno);
            Sdt dt = WebUtil.QuerySdt(sql);
            dt.Next();
            if (dt.GetInt32("chk_mem") == 0)
            {
                dsMain.DATA[0].member_no = "";
                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบเลขสมาชิก " + ls_memno);
                return;
            }

            ls_asstype = dsAssisttype.DATA[0].assisttype_code;
            dsMain.Retrieve(ls_memno);
            dsMain.DATA[0].work_date = state.SsWorkDate;

            if (dt.GetString("dead_status") == "03")
            {
                LtServerMessage.Text = WebUtil.WarningMessage("สมาชิกทะเบียน " + ls_memno + " เสียชีวิต");
                dsAssisttype.Visible = true;
                //dsAssisttype.AssistType();
                dsAssisttype.FindDropDownList(0, "assisttype_code").Enabled = false;
                dsAssisttype.DATA[0].assisttype_code = "20";
                hdActMemno.Value = "chk2";
                dsDecease.Retrieve(ls_memno, dsAssisttype.DATA[0].assisttype_code);
                GetTap();
                hdTabIndex.Value = "1";

            }
            else if(dt.GetInt32("resign_status") == 1){
                dsMain.DATA[0].member_no = "";
                LtServerMessage.Text = WebUtil.ErrorMessage("สมาชิกทะเบียน " + ls_memno + " ลาออกไปแล้ว");
                return;
            }
            /*if (dt.GetInt32("dead_status") == 1)
            {
                LtServerMessage.Text = WebUtil.WarningMessage("สมาชิกทะเบียน " + ls_memno + " เสียชีวิต");
                dsAssisttype.Visible = true;
                //dsAssisttype.AssistType();
                dsAssisttype.FindDropDownList(0, "assisttype_code").Enabled = false;
                dsAssisttype.DATA[0].assisttype_code = "20";
                hdActMemno.Value = "chk2";
                dsDecease.Retrieve(ls_memno, ls_asstype);
                GetTap();
                hdTabIndex.Value = "1";
                return;
            }*/
            else
            {
                dsAssisttype.FindDropDownList(0, "assisttype_code").Enabled = true;
            }

            

            if (ls_memno != "" && ls_asstype != "")
            {
               
                dsAssisttype.Visible = true;
                //dsAssisttype.AssistType();
                chkFamily_flag(ls_memno, ls_asstype);
                //dsDecease.Retrieve(ls_memno, ls_asstype);
                //dsFamilydecease.Retrieve(ls_memno, ls_asstype);
                //dsEducation.Retrieve(ls_memno, ls_asstype);
                //dsDisaster.Retrieve(ls_memno, ls_asstype);
                dsDisaster.DdDisaster_type(ls_asstype);
                dsDisaster.DdCurrProvince();
                dsDisaster.DdCurrDistrict(dsDisaster.DATA[0].H_PROVINCE_CODE);
                dsDisaster.DdCurrTambol(dsDisaster.DATA[0].H_DISTRICT_CODE);
                dsDisaster.DdProvince();
                dsDisaster.DdDistrict(dsDisaster.DATA[0].PROVINCE_CODE);
                dsDisaster.DdTambol(dsDisaster.DATA[0].DISTRICT_CODE);
                //dsPatronize.Retrieve(ls_memno, ls_asstype);
                dsPatronize.PostPayType(ls_asstype);
                if (hdCheckDsAmount.Value == "chk1")
                {
                    dsAmount.Retrieve(ls_memno, ls_asstype);
                }
                //dsAmount.Retrieve2(ls_memno);
                dsAmount.RetrieveBank();
                dsAmount.RetrieveBranch(dsAmount.DATA[0].EXPENSE_BANK);
                dsAmount.RetrieveMoneyType();
                dsFamilydecease.PostPayType(ls_asstype);
                dsEducation.DdEducatLevel(ls_asstype);
                dsEducation.RetrievePrename();
                //dsGain.RetrievePrename();
                //dsGain.RetrieveGainMb(ls_memno);
                dsList.RetrieveReq(ls_memno);
                dsAmount.RetrieveDeptaccount(ls_memno);
                GetAgeChild();
                GetMonryEver(ls_memno, ls_asstype);
                if (hdActMemno.Value == "chk1") //แก้ปัญหาตอนเปลี่ยน member_no แล้วไม่มีใบคำขอที่ยังไม่ได้อนุมัติ
                {
                    hdActMemno.Value = "chk2";
                    GetTap();
                    hdActMemno.Value = "";
                }
                
            }
        }

        private void chkFamily_flag(string ls_memno, string ls_asstype)
        {
            sqlStr = @"select family_flag from assucfassisttype where coop_id = {0} and assisttype_code = {1}";
            sqlStr = WebUtil.SQLFormat(sqlStr, state.SsCoopId, ls_asstype);
            Sdt dt1 = WebUtil.QuerySdt(sqlStr);
            dt1.Next();

            if (dt1.GetInt32("family_flag") == 1)
            {
                sqlStr = @"select * from assreqmaster where card_person in (
	                        select mate_cardperson from mbmembmaster where coop_id = {0} and member_no = {1}
                        )";
                sqlStr = WebUtil.SQLFormat(sqlStr, state.SsCoopId, ls_memno);
                Sdt dt2 = WebUtil.QuerySdt(sqlStr);
                if (dt2.Next())
                {
                    LtServerMessage.Text = WebUtil.WarningMessage("สวัสดิการประเภทนี้คู่สมรส(" + dt2.GetString("member_no") + ") คู่สมรสของท่านได้ขอไปแล้ว");
                }
            }
        }

        
        public void CurrPostcode()
        {
            dsDisaster.DATA[0].H_TAMBOL_CODE = "";
            dsDisaster.DdCurrTambol(dsDisaster.DATA[0].H_DISTRICT_CODE);

            String sql = @"SELECT MBUCFDISTRICT.POSTCODE   
                FROM MBUCFDISTRICT,  
	                MBUCFPROVINCE         	                        
                WHERE ( MBUCFPROVINCE.PROVINCE_CODE = MBUCFDISTRICT.PROVINCE_CODE )   
	                and ( MBUCFDISTRICT.PROVINCE_CODE = {0} ) 
	                and ( MBUCFDISTRICT.DISTRICT_CODE = {1} )";
            sql = WebUtil.SQLFormat(sql, dsDisaster.DATA[0].H_PROVINCE_CODE, dsDisaster.DATA[0].H_DISTRICT_CODE);
            Sdt dt = WebUtil.QuerySdt(sql);
            if (dt.Next())
            {
                dsDisaster.DATA[0].H_POSTCODE = dt.Rows[0]["postcode"].ToString();
            }
        }
        public void Postcode()
        {
            dsDisaster.DATA[0].TAMBOL_CODE = "";
            dsDisaster.DdTambol(dsDisaster.DATA[0].DISTRICT_CODE);

            string sql = @"SELECT MBUCFDISTRICT.POSTCODE   
                FROM MBUCFDISTRICT,  
	                MBUCFPROVINCE         	                        
                WHERE ( MBUCFPROVINCE.PROVINCE_CODE = MBUCFDISTRICT.PROVINCE_CODE )   
	                and ( MBUCFDISTRICT.PROVINCE_CODE = {0} ) 
	                and ( MBUCFDISTRICT.DISTRICT_CODE = {1} )";
            sql = WebUtil.SQLFormat(sql, dsDisaster.DATA[0].PROVINCE_CODE, dsDisaster.DATA[0].DISTRICT_CODE);
            Sdt dt = WebUtil.QuerySdt(sql);
            if (dt.Next())
            {
                dsDisaster.DATA[0].POSTCODE = dt.Rows[0]["postcode"].ToString();
            }
        }

        private void chkFamiyReq()//kkkk
        {

            string sql = @"
                select a.*, b.num_pay from assreqmaster a
                inner join assucfassisttypedet b on a.assisttype_code = b.assisttype_code where a.req_status = 1 and 
                a.coop_control = {0} and a.assisttype_code = {1} and b.assisttype_pay = {2} and a.member_no = {3}";
            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, dsAssisttype.DATA[0].assisttype_code, dsFamilydecease.DATA[0].ASSISTPAY_CODE,dsMain.DATA[0].member_no);
            Sdt dt = WebUtil.QuerySdt(sql);

            if (dt.Next())
            {
                if (dt.GetRowCount() == dt.GetInt32("num_pay"))
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("สมาชิกท่านนี้ " + dt.GetString("member_no") + " ได้ขอสวัสดิการประเภทนี้ ในปีนี้ครบไปแล้ว");
                    dsEducation.Visible = false;
                    dsDecease.Visible = false;
                    dsFamilydecease.Visible = false;
                    dsDisaster.Visible = false;
                    dsPatronize.Visible = false;
                    dsGain.Visible = false;
                    return;
                }
                
            }
        }

        public void SaveWebSheet()
        {
            if (dsAssisttype.DATA[0].assisttype_code == "00")
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("กรุณาเลือก ประเภทสวัสดิการ");
                return;
            }
            ls_asstype = dsAssisttype.DATA[0].assisttype_code;
            string ls_typegroup = "00";
            int ls_calculate_flag = 0;
            String sql = @"select * from assucfassisttype where assisttype_code = {0}";
            sql = WebUtil.SQLFormat(sql, ls_asstype);
            Sdt dt = WebUtil.QuerySdt(sql);
            if (dt.Next())
            {
                ls_typegroup = dt.GetString("assisttype_group");
                ls_calculate_flag = dt.GetInt32("calculate_flag");
            }
            try
            {
                string ls_assdoc = "", ls_childpercode = "", ls_childname = "", ls_childsur = "", ls_childcard = "", ls_chilshool = "", ls_family_card = "", ls_asspaycode = "00";
                int li_reqstatus = 8;
                string ls_moneytype = "", ls_bank = "", ls_deptaccno = "", ls_accid = "", ls_branch = "", ls_remark = "", ls_sendsys = "";
                decimal dec_amount = 0, dec_childgpa = 0, dec_damages = 0;
                //DateTime date_childbirth = Convert.ToDateTime("01/01/1990"), dete_die = Convert.ToDateTime("01/01/1990"), dete_disaster = Convert.ToDateTime("01/01/1990");
                string ls_receive = "", ls_diename = "", ls_sqlupdate = "", ls_mem_card = "";
                string ls_disaster = "", ls_childbirth = "", ls_die = "", ls_approve = "";
                string ls_addr = "", ls_soi = "", ls_road = "", ls_province = "", ls_district = "", ls_tambol = "", ls_postcode = "";

                string ls_type_preple_add = "";
                decimal ls_principal_balance = 0;
               

                string ls_reqdate = state.SsWorkDate.ToString("dd/MM/yyyy");
                string ls_treatstart_date = "", ls_treatend_date = "";
                string ls_child_no = "", ls_child_no_work = "", ls_child_no_study = "";
                //////////////เลขบัตร ปชช.
                ls_mem_card = dsMain.DATA[0].card_person;
                ///////////////////////

                if (ls_typegroup == "01")//ส่งเสริมบุตร
                {
                    try { ls_assdoc = dsEducation.DATA[0].ASSIST_DOCNO; }
                    catch { ls_assdoc = ""; }
                    try
                    {
                        li_reqstatus = dsEducation.DATA[0].REQ_STATUS;
                        ls_reqdate = dsEducation.DATA[0].REQ_DATE.ToString("dd/MM/yyyy");
                        if (li_reqstatus == 1)
                        {
                            ls_approve = ls_reqdate;
                            ls_sqlupdate = ls_sqlupdate + ", APPROVE_DATE = to_date('" + ls_reqdate + "', 'dd/mm/yyyy')";
                        }
                        
                    }
                    catch { li_reqstatus = 8; }

                    try { 
                        ls_child_no = dsEducation.DATA[0].child_no.ToString();
                        ls_child_no_work = dsEducation.DATA[0].child_no_work.ToString();
                        ls_child_no_study = dsEducation.DATA[0].child_no_study.ToString();
                        ls_sqlupdate = ls_sqlupdate + ", CHILD_NO = '" + ls_child_no + "'";
                        ls_sqlupdate = ls_sqlupdate + ", CHILD_NO_WORK = '" + ls_child_no_work + "'";
                        ls_sqlupdate = ls_sqlupdate + ", CHILD_NO_STUDY = '" + ls_child_no_study + "'";
                    }
                    catch{
                        ls_child_no = "";
                        ls_child_no_work = "";
                        ls_child_no_study = "";
                    }

                    
                    try { ls_childpercode = dsEducation.DATA[0].CHILD_PRENAME_CODE; }
                    catch { ls_childpercode = ""; }
                    try
                    {
                        if (dsEducation.DATA[0].CHILD_NAME == "")
                        {
                            LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอก ชื่อบุตร");
                            return;
                        }
                        ls_childname = dsEducation.DATA[0].CHILD_NAME;
                    }
                    catch
                    {
                        ls_childname = "";
                    }
                    try
                    {
                        if (dsEducation.DATA[0].CHILD_SURNAME == "")
                        {
                            LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอก นามสกุลบุตร");
                            return;
                        }
                        ls_childsur = dsEducation.DATA[0].CHILD_SURNAME;
                    }
                    catch
                    {
                        ls_childsur = "";
                    }
                    try
                    {
                        if (dsEducation.DATA[0].CHILD_BIRTH_DATE.ToString("dd/MM/yyyy") == "01/01/1500")
                        {
                            LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอก วันเกิดบุตร");
                            return;
                        }
                        ls_childbirth = dsEducation.DATA[0].CHILD_BIRTH_DATE.ToString("dd/MM/yyyy");
                        ls_sqlupdate = ls_sqlupdate + ", CHILD_BIRTH_DATE = to_date('" + ls_childbirth + "', 'dd/mm/yyyy')";
                    }
                    catch { ls_childbirth = "01/01/1990"; }
                    try
                    {
                        if (dsEducation.DATA[0].CHILD_CARD_PERSON == "")
                        {
                            LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอก เลขบัตรประชาชน");
                            return;
                        }
                        ls_childcard = dsEducation.DATA[0].CHILD_CARD_PERSON;
                    }
                    catch { ls_childcard = ""; }
                    try
                    {
                        if (dsEducation.DATA[0].CHILD_SCHOOL == "")
                        {
                            LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอก ชื่อสถานศึกษา");
                            return;
                        }
                        ls_chilshool = dsEducation.DATA[0].CHILD_SCHOOL;
                    }
                    catch { ls_chilshool = ""; }
                    try
                    {
                        if (dsEducation.DATA[0].child_level == "" || dsEducation.DATA[0].child_level == "00")
                        {
                            LtServerMessage.Text = WebUtil.ErrorMessage("กรุณาเลือก ระดับชั้น");
                            return;
                        }
                        ls_asspaycode = dsEducation.DATA[0].child_level;
                    }
                    catch { ls_asspaycode = ""; }
                    try
                    {
                        if (ls_calculate_flag == 1)
                        {
                            if (dsEducation.DATA[0].CHILD_GPA == 0)
                            {
                                LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอก เกรดเฉลี่ย");
                                return;
                            }
                            dec_childgpa = dsEducation.DATA[0].CHILD_GPA;
                        }
                    }
                    catch { dec_childgpa = 0; }

                    //chkGpa(ls_asstype, ls_asspaycode, dec_childgpa);// ตรวจสอบ เกรด //ไม่ต้องเช็คแล้วส่งค่าเข้า hdSaveChk_GPA เช็ดหน้า ui แล้ว

                }
                else if (ls_typegroup == "02")//สมาชิกเสียชีวิต
                {
                    try { ls_assdoc = dsDecease.DATA[0].ASSIST_DOCNO; }
                    catch { ls_assdoc = ""; }
                    try
                    {
                        li_reqstatus = dsDecease.DATA[0].REQ_STATUS;
                        ls_reqdate = dsDecease.DATA[0].REQ_DATE.ToString("dd/MM/yyyy");
                        if (li_reqstatus == 1)
                        {
                            ls_approve = ls_reqdate;
                            ls_sqlupdate = ls_sqlupdate + ", APPROVE_DATE = to_date('" + ls_reqdate + "', 'dd/mm/yyyy')";
                        }
                    }
                    catch { li_reqstatus = 8; }
                    try
                    {
                        if (dsDecease.DATA[0].DEAD_DATE.ToString("dd/MM/yyyy") == "01/01/1500")
                        {
                            LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอก วันที่ถึงแก่กรรม");
                            return;
                        }
                        ls_die = dsDecease.DATA[0].DEAD_DATE.ToString("dd/MM/yyyy");
                        ls_sqlupdate = ls_sqlupdate + ", DEAD_DATE = to_date('" + ls_die + "', 'dd/mm/yyyy')";
                    }
                    catch { ls_die = "01/01/1990"; }
                    //insert ลง table assreqgain แทน
                    //try
                    //{
                    //    if (dsDecease.DATA[0].MEMBER_RECEIVE == "")
                    //    {
                    //        LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอก ชื่อผู้รับผลประโยชน์");
                    //        return;
                    //    }
                    //    ls_receive = dsDecease.DATA[0].MEMBER_RECEIVE;
                    //}
                    //catch { ls_receive = ""; }
                    try
                    {
                        String sql_fix = @" select * from assucfassistpaytype where assisttype_group = {0} ";
                        sql_fix = WebUtil.SQLFormat(sql_fix, ls_typegroup);
                        Sdt dt_fix = WebUtil.QuerySdt(sql_fix);
                        dt_fix.Next();
                        ls_asspaycode = dt_fix.GetString("ASSISTPAY_CODE");
                    }
                    catch
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage("ไม่มีประเภทการจ่ายของสวัสดิการประเภทนี้");
                        return;
                    }
                }
                else if (ls_typegroup == "03")//ครอบครัวเสียชีวิต
                {
                    try { ls_assdoc = dsFamilydecease.DATA[0].ASSIST_DOCNO; }
                    catch { ls_assdoc = ""; }
                    try
                    {
                        li_reqstatus = dsFamilydecease.DATA[0].REQ_STATUS;
                        ls_reqdate = dsFamilydecease.DATA[0].REQ_DATE.ToString("dd/MM/yyyy");
                        if (li_reqstatus == 1)
                        {
                            ls_approve = ls_reqdate;
                            ls_sqlupdate = ls_sqlupdate + ", APPROVE_DATE = to_date('" + ls_reqdate + "', 'dd/mm/yyyy')";
                        }
                    }
                    catch { li_reqstatus = 8; }
                    try
                    {
                        if (dsFamilydecease.DATA[0].DEAD_DATE.ToString("dd/MM/yyyy") == "01/01/1500")
                        {
                            LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอก วันที่ถึงแก่กรรม");
                            return;
                        }
                        ls_die = dsFamilydecease.DATA[0].DEAD_DATE.ToString("dd/MM/yyyy");
                        ls_sqlupdate = ls_sqlupdate + ", DEAD_DATE = to_date('" + ls_die + "', 'dd/mm/yyyy')";
                    }
                    catch { ls_die = "01/01/1990"; }
                    try
                    {
                        if (dsFamilydecease.DATA[0].familydead_name == "")
                        {
                            LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอก ชื่อผู้เสียชีวิต");
                            return;
                        }
                        ls_diename = dsFamilydecease.DATA[0].familydead_name;
                    }
                    catch { ls_diename = ""; }
                    try
                    {
                        if (dsFamilydecease.DATA[0].ASSISTPAY_CODE == "" || dsFamilydecease.DATA[0].ASSISTPAY_CODE == "00")
                        {
                            LtServerMessage.Text = WebUtil.ErrorMessage("กรุณาเลือก ความสัมพันธ์กับผู้ถึงแก่กรรม");
                            return;
                        }
                        ls_asspaycode = dsFamilydecease.DATA[0].ASSISTPAY_CODE;
                    }
                    catch { ls_asspaycode = ""; }
                    try
                    {
                        if (dsFamilydecease.DATA[0].family_card_person == "")
                        {
                            LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอก เลขบัตรประชาชน");
                            return;
                        }
                        ls_family_card = dsFamilydecease.DATA[0].family_card_person;
                    }
                    catch { ls_family_card = ""; }
                }
                else if (ls_typegroup == "04")//ประสบภัยพิบัติ
                {
                    try { ls_assdoc = dsDisaster.DATA[0].ASSIST_DOCNO; }
                    catch { ls_assdoc = ""; }
                    try
                    {
                        li_reqstatus = dsDisaster.DATA[0].REQ_STATUS;
                        ls_reqdate = dsDisaster.DATA[0].REQ_DATE.ToString("dd/MM/yyyy");
                        if (li_reqstatus == 1)
                        {
                            ls_approve = ls_reqdate;
                            ls_sqlupdate = ls_sqlupdate + ", APPROVE_DATE = to_date('" + ls_reqdate + "', 'dd/mm/yyyy')";
                        }
                    }
                    catch { li_reqstatus = 8; }
                    try
                    {
                        if (dsDisaster.DATA[0].DISASTER_DATE.ToString("dd/MM/yyyy") == "01/01/1500")
                        {
                            LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอก วันที่ประสบภัย");
                            return;
                        }
                        ls_disaster = dsDisaster.DATA[0].DISASTER_DATE.ToString("dd/MM/yyyy");
                        ls_sqlupdate = ls_sqlupdate + ", DISASTER_DATE = to_date('" + ls_disaster + "', 'dd/mm/yyyy')";
                    }
                    catch { ls_disaster = "01/01/1990"; }
                    try
                    {
                        if (dsDisaster.DATA[0].disaster_type == "" || dsDisaster.DATA[0].disaster_type == "00")
                        {
                            LtServerMessage.Text = WebUtil.ErrorMessage("กรุณาเลือก ประเภทประสบภัย");
                            return;
                        }
                        ls_asspaycode = dsDisaster.DATA[0].disaster_type;
                    }
                    catch { ls_asspaycode = ""; }
                    try
                    {
                        if (dsDisaster.DATA[0].POSTCODE == "")
                        {
                            LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอก ที่อยู่ที่ประสบภัย");
                            return;
                        }
                        ls_postcode = dsDisaster.DATA[0].POSTCODE;
                    }
                    catch { ls_postcode = ""; }
                    try
                    {
                        if (dsDisaster.DATA[0].MEMB_ADDR == "")
                        {
                            LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอก ที่อยู่ที่ประสบภัย");
                            return;
                        }
                        ls_addr = dsDisaster.DATA[0].MEMB_ADDR;
                    }
                    catch { ls_addr = ""; }
                    try { ls_soi = dsDisaster.DATA[0].SOI; }
                    catch { ls_soi = ""; }
                    try { ls_road = dsDisaster.DATA[0].ROAD; }
                    catch { ls_road = ""; }
                    try { ls_province = dsDisaster.DATA[0].PROVINCE_CODE; }
                    catch { ls_province = ""; }
                    try { ls_district = dsDisaster.DATA[0].DISTRICT_CODE; }
                    catch { ls_district = ""; }
                    try { ls_tambol = dsDisaster.DATA[0].TAMBOL_CODE; }
                    catch { ls_tambol = ""; }
                    try { dec_damages = dsDisaster.DATA[0].damages_amt; }
                    catch { dec_damages = 0; }
                  
                    try
                    {
                        ls_type_preple_add = dsDisaster.DATA[0].type_preple_add.ToString();
                        ls_sqlupdate = ls_sqlupdate + ", TYPE_PREPLE_ADD = '" + ls_type_preple_add + "'";
                    }
                    catch { ls_type_preple_add = ""; }
                  

                }
                else if (ls_typegroup == "05" || ls_typegroup == "06")//เกื้อกุลสมาชิก
                {
                    try { ls_assdoc = dsPatronize.DATA[0].ASSIST_DOCNO; }
                    catch { ls_assdoc = ""; }
                    try
                    {
                        li_reqstatus = dsPatronize.DATA[0].REQ_STATUS;
                        ls_reqdate = dsPatronize.DATA[0].REQ_DATE.ToString("dd/MM/yyyy");
                        if (li_reqstatus == 1)
                        {
                            ls_approve = ls_reqdate;
                            ls_sqlupdate = ls_sqlupdate + ", APPROVE_DATE = to_date('" + ls_reqdate + "', 'dd/mm/yyyy')";
                        }
                    }
                    catch { li_reqstatus = 8; }
                    try
                    {
                        if (dsPatronize.DATA[0].senile_type == "00" || dsPatronize.DATA[0].senile_type == "")
                        {
                            LtServerMessage.Text = WebUtil.ErrorMessage("กรุณาเลือก ประเภทการจ่าย");
                            return;
                        }
                        ls_asspaycode = dsPatronize.DATA[0].senile_type;
                    }
                    catch
                    {
                        ls_asspaycode = "";
                    }
                    if (dsPatronize.DATA[0].cal_date > 0) //เฉพาะรักษาพยาบาล
                    {
                        try
                        {
                            ls_treatstart_date = dsPatronize.DATA[0].start_treat.ToString("dd/MM/yyyy");
                            ls_sqlupdate = ls_sqlupdate + ", t reatstart_date = to_date('" + ls_treatstart_date + "', 'dd/mm/yyyy')";
                        }
                        catch { ls_treatstart_date = "01/01/1990"; }
                        try
                        {
                            ls_treatend_date = dsPatronize.DATA[0].end_treat.ToString("dd/MM/yyyy");
                            ls_sqlupdate = ls_sqlupdate + ", treatend_date = to_date('" + ls_treatend_date + "', 'dd/mm/yyyy')";
                        }
                        catch { ls_treatend_date = "01/01/1990"; }
                    }
                }


                //สวนของเรื่องเงิน ////////////////////
                try { ls_moneytype = dsAmount.DATA[0].MONEYTYPE_CODE; }
                catch { ls_moneytype = "CSH"; }

                try
                {
                    if (ls_moneytype == "CSH" || ls_moneytype == "")
                    {
                        ls_moneytype = "CSH";
                        ls_deptaccno = "";
                        ls_branch = "";
                        ls_bank = "";
                        ls_accid = "";
                    }
                    else if (ls_moneytype == "TRN")
                    {
                        if (dsAmount.DATA[0].send_system == "")
                        {
                            LtServerMessage.Text = WebUtil.ErrorMessage("กรุณาเลือก ระบบที่จะโอนไป");
                            return;
                        }
                        ls_sendsys = dsAmount.DATA[0].send_system;
                        if (ls_sendsys == "DEP")
                        {
                            if (dsAmount.DATA[0].DEPTACCOUNT_NO == "00")
                            {
                                LtServerMessage.Text = WebUtil.ErrorMessage("กรูณาเลือก เลขบัญชี");
                                return;
                            }
                            else
                            {
                                ls_deptaccno = dsAmount.DATA[0].DEPTACCOUNT_NO;
                                ls_branch = "";
                                ls_bank = "";
                                ls_accid = "";
                            }
                        }
                        else// LON
                        {
                            ls_branch = "";
                            ls_bank = "";
                            ls_accid = "";
                            ls_deptaccno = "";
                        }
                    }
                    else // CHQ, CBT
                    {
                        if (dsAmount.DATA[0].EXPENSE_BANK == "")
                        {
                            LtServerMessage.Text = WebUtil.ErrorMessage("กรูณาเลือก ธนาคาร");
                            return;
                        }
                        if (dsAmount.DATA[0].EXPENSE_ACCID == "")
                        {
                            LtServerMessage.Text = WebUtil.ErrorMessage("กรูณากรอก เลขธนาคาร");
                            return;
                        }
                        ls_branch = dsAmount.DATA[0].EXPENSE_BRANCH;
                        ls_bank = dsAmount.DATA[0].EXPENSE_BANK;
                        ls_accid = dsAmount.DATA[0].EXPENSE_ACCID;
                        ls_deptaccno = "";
                    }
                }
                catch
                {
                    ls_branch = "";
                    ls_bank = "";
                    ls_accid = "";
                    ls_deptaccno = "";
                }
                try { ls_remark = dsAmount.DATA[0].remark; }
                catch { ls_remark = ""; }
                try
                {
                    if (dsAmount.DATA[0].APPROVE_AMT <= 0)
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage("ตรวจสอบจำนวนเงิน");
                        return;
                    }
                    dec_amount = dsAmount.DATA[0].APPROVE_AMT;
                }
                catch { dec_amount = 0; }
                try
                {

                    ls_principal_balance = dsAmount.DATA[0].principal_balance;
                    ls_sqlupdate = ls_sqlupdate + ", PRINCIPAL_BALANCE = '" + ls_principal_balance + "'";
                }
                catch { ls_principal_balance = 0; }

                //---------------------- save --------------------------
                int li_year = state.SsWorkDate.Year;
                if (ls_assdoc == "")
                {
                    string ls_saveassno = "";
                    int li_savelastno = 0;
                    sql = @"select 
	                        last_documentno + 1 last_doc,
                            document_year,
	                        document_prefix || substr(document_year, 3, 2) || REPLACE(substr(document_format, 5, length(substr(document_format, 5, 6)) - length(last_documentno + 1)) || cast(last_documentno + 1 as varchar(6)), 'R', '0') assist_no
                        from cmdocumentcontrol where document_code = 'ASSISTDOCNO'";
                    sql = WebUtil.SQLFormat(sql);
                    dt = WebUtil.QuerySdt(sql);
                    if (dt.Next())
                    {
                        ls_saveassno = dt.GetString("assist_no");
                        li_savelastno = dt.GetInt32("last_doc");
                        //li_year = dt.GetInt32("document_year");
                        li_year = dsAssisttype.DATA[0].ass_year + 543;
                    }
                    string sqlStr = @"insert into assreqmaster
                    (COOP_ID 			            , COOP_CONTROL 		            , ASSIST_DOCNO                  , ASSIST_YEAR                   , MEMBER_NO 
                    , ASSISTTYPE_CODE               , ASSISTPAY_CODE 	            , APPROVE_AMT                   , ASSIST_AMT                    , REQ_STATUS 
                    , REQ_DATE 			            , CANCEL_DATE 		            , APPROVE_DATE                  , PAY_DATE                      , REMARK 
                    , ENTRY_ID 			            , CANCEL_ID 		            , MONEYTYPE_CODE                , EXPENSE_BANK                  , EXPENSE_BRANCH 
                    , EXPENSE_ACCID 	            , DEPTACCOUNT_NO 	            , MEMB_ADDR                     , ADDR_GROUP                    , SOI 
                    , ROAD 				            , TAMBOL_CODE		            , DISTRICT_CODE                 , PROVINCE_CODE                 , POSTCODE 
                    , DEAD_DATE 		            , WITHDRAWABLE_AMT 	            , CLEARLOAN_FLAG                , MEMBER_RECEIVE                , CHILD_PRENAME_CODE 
                    , CHILD_NAME 		            , CHILD_SURNAME 	            , CHILD_SCHOOL                  , CHILD_BIRTH_DATE              , CHILD_GPA 
                    , CHILD_CARD_PERSON	            , DISASTER_DATE 	            , REF_SLIPNO                    , FAMILYDEAD_NAME               , SEND_SYSTEM
                    , treatstart_date               , treatend_date                 , family_card_person            , damages_amt                   , card_person
                    , child_no                      , child_no_work                 , child_no_study                , type_preple_add               , principal_balance )
                    values
                    ({0}                            , {1}                           , {2}                           , {3}                           , {4}
                    , {5}                           , {6}                           , {7}                           , {8}                           , {9}
                    , to_date({10}, 'dd/mm/yyyy')   , {11}                          , to_date({12}, 'dd/mm/yyyy')   , {13}                          , {14}
                    , {15}                          , {16}                          , {17}                          , {18}                          , {19}
                    , {20}                          , {21}                          , {22}                          , {23}                          , {24}
                    , {25}                          , {26}                          , {27}                          , {28}                          , {29}
                    , to_date({30}, 'dd/mm/yyyy')   , {31}                          , {32}                          , {33}                          , {34}
                    , {35}                          , {36}                          , {37}                          , to_date({38}, 'dd/mm/yyyy')   , {39}
                    , {40}                          , to_date({41}, 'dd/mm/yyyy')   , {42}                          , {43}                          , {44}
                    , to_date({45}, 'dd/mm/yyyy')   , to_date({46}, 'dd/mm/yyyy')   , {47}                          , {48}                          , {49}
                    , {50}                          , {51}                          , {52}                          , {53}                          , {54})";

                    //fix กรมชล
   

                        sqlStr = WebUtil.SQLFormat(sqlStr
                        , state.SsCoopId, state.SsCoopControl, ls_saveassno, li_year, dsMain.DATA[0].member_no
                        , ls_asstype, ls_asspaycode, dec_amount, dec_amount, li_reqstatus
                        , ls_reqdate, "", ls_approve, "", ls_remark
                        , state.SsUsername, "", ls_moneytype, ls_bank, ls_branch
                        , ls_accid, ls_deptaccno, ls_addr, "", ls_soi
                        , ls_road, ls_tambol, ls_district, ls_province, ls_postcode
                        , ls_die, dec_amount, "0", ls_receive, ls_childpercode
                        , ls_childname, ls_childsur, ls_chilshool, ls_childbirth, dec_childgpa
                        , ls_childcard, ls_disaster, "", ls_diename, ls_sendsys
                        , ls_treatstart_date, ls_treatend_date, ls_family_card, dec_damages, ls_mem_card,ls_child_no,ls_child_no_work,ls_child_no_study, ls_type_preple_add,ls_principal_balance);

                    
                    WebUtil.ExeSQL(sqlStr);

                    if (ls_asstype == "20")//สมาชิกถึงแกกรรม
                    {
                        for (int ii = 0; ii < dsGain.RowCount; ii++)
                        {
                            sqlStr = @"insert into assreqgain 
                                    (coop_id, coop_control, assist_docno, seq_no
                                    , gainprename_code, gain_name, gain_surname
                                    , gain_relation, assist_amt)
                                    values
                                     ({0}, {1}, {2}, {3}
                                    , {4}, {5}, {6}
                                    , {7}, {8})";
                            sqlStr = WebUtil.SQLFormat(sqlStr
                                    , state.SsCoopId, state.SsCoopControl, ls_saveassno, dsGain.DATA[ii].seq_no
                                    , dsGain.DATA[ii].gainprename_code, dsGain.DATA[ii].gain_name, dsGain.DATA[ii].gain_surname
                                    , dsGain.DATA[ii].ASSISTPAY_CODE, 0);
                            WebUtil.ExeSQL(sqlStr);
                        }
                    }

                    sqlStr = @"update cmdocumentcontrol 
                                                set last_documentno = {0} 
                                                where document_code = 'ASSISTDOCNO'";
                    sqlStr = WebUtil.SQLFormat(sqlStr, li_savelastno);
                    WebUtil.ExeSQL(sqlStr);
                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกใบคำขอสำเร็จ");
                }
                else
                {
                    string sqlStr_update = "UPDATE ASSREQMASTER SET" +
                        " APPROVE_AMT 		    = " + dec_amount + "	, ASSIST_AMT 		    = " + dec_amount + "		    , REQ_STATUS 	        = " + li_reqstatus + "		, REQ_DATE              = to_date('" + ls_reqdate + "','dd/mm/yyyy')        , ASSISTPAY_CODE        = '" + ls_asspaycode + "'" +
                        ", REMARK               = '" + ls_remark + "'   , ENTRY_ID              = '" + state.SsUsername + "'	, MONEYTYPE_CODE 	    = '" + ls_moneytype + "'	, EXPENSE_BANK	        ='" + ls_bank + "'                                  , EXPENSE_BRANCH        = '" + ls_branch + "'" +
                        ", EXPENSE_ACCID        = '" + ls_accid + "'    , DEPTACCOUNT_NO        = '" + ls_deptaccno + "'        , MEMB_ADDR             = '" + ls_addr + "'         , SOI                   = '" + ls_soi + "'                                  , ROAD                  = '" + ls_road + "'" +
                        ", TAMBOL_CODE          = '" + ls_tambol + "'   , DISTRICT_CODE         = '" + ls_district + "'         , PROVINCE_CODE         = '" + ls_province + "'     , POSTCODE              = '" + ls_postcode + "'                             , WITHDRAWABLE_AMT      = " + dec_amount +
                        ", CLEARLOAN_FLAG       = '0'                   , MEMBER_RECEIVE        = '" + ls_receive + "'          , CHILD_PRENAME_CODE    = '" + ls_childpercode + "' , CHILD_NAME            = '" + ls_childname + "'                            , CHILD_SURNAME         = '" + ls_childsur + "'" +
                        ", CHILD_SCHOOL         = '" + ls_chilshool + "', CHILD_GPA             = " + dec_childgpa + "          , CHILD_CARD_PERSON     = '" + ls_childcard + "'    , FAMILYDEAD_NAME       = '" + ls_diename + "'                              , SEND_SYSTEM           = '" + ls_sendsys + "'" +
                        ", damages_amt          = " + dec_damages + "   , family_card_person    = '" + ls_family_card + "'" +
                        ls_sqlupdate +
                        " where ASSIST_DOCNO = '" + ls_assdoc + "' and coop_id = '" + state.SsCoopId + "'";
                    Sdt sql_update = WebUtil.QuerySdt(sqlStr_update);

                    if (ls_asstype == "20")//สมาชิกถึงแกกรรม
                    {
                        sqlStr = @"delete assreqgain where coop_control = {0} and ASSIST_DOCNO = {1}";
                        sqlStr = WebUtil.SQLFormat(sqlStr, state.SsCoopControl, ls_assdoc);
                        WebUtil.ExeSQL(sqlStr);
                        for (int ii = 0; ii < dsGain.RowCount; ii++)
                        {
                            sqlStr = @"insert into assreqgain 
                                    (coop_id, coop_control, assist_docno, seq_no
                                    , gainprename_code, gain_name, gain_surname
                                    , gain_relation, assist_amt)
                                    values
                                     ({0}, {1}, {2}, {3}
                                    , {4}, {5}, {6}
                                    , {7}, {8})";
                            sqlStr = WebUtil.SQLFormat(sqlStr
                                    , state.SsCoopId, state.SsCoopControl, ls_assdoc, dsGain.DATA[ii].seq_no
                                    , dsGain.DATA[ii].gainprename_code, dsGain.DATA[ii].gain_name, dsGain.DATA[ii].gain_surname
                                    , dsGain.DATA[ii].ASSISTPAY_CODE, 0);
                            WebUtil.ExeSQL(sqlStr);
                        }
                    }

                    LtServerMessage.Text = WebUtil.CompleteMessage("แก้ไขข้อมูลใบคำขอสำเร็จ");
                }
                dsMain.ResetRow();
                dsAssisttype.DATA[0].assisttype_code = "00";
                dsEducation.Visible = false;
                dsDecease.Visible = false;
                dsFamilydecease.Visible = false;
                dsDisaster.Visible = false;
                dsPatronize.Visible = false;
                dsGain.ResetRow();
                dsGain.Visible = false;
                dsAmount.ResetRow();
                dsList.RetrieveReq(dsMain.DATA[0].member_no);
                hdSaveChk_GPA.Value = "";
            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage("บันทึกไม่สำเร็จ : " + ex); }
        }
        public void chkGpa(string ls_assiscode, string ls_asslevel, decimal dec_gpa) //ตรวจสอบเกรด ตามค่าค่าคงที่
        {
            //li_chkstep = 0 คือส่งมาจากที่อื่น ที่ไม่ใช่ save
            //li_chkstep = 1 คือส่งมาจากตอน save
            String sql = @"select * from assucfassisttypedet where coop_id = {0} and assisttype_code = {1} and assisttype_pay = {2}";
            sql = WebUtil.SQLFormat(sql, state.SsCoopId, ls_assiscode, ls_asslevel);
            Sdt dt = WebUtil.QuerySdt(sql);
            if (dt.Next())
            {
                if (dt.GetDecimal("min_check") > dec_gpa)
                {
                    hdSaveChk_GPA.Value = dt.GetDecimal("min_check").ToString();
                    LtServerMessage.Text = WebUtil.WarningMessage("เกรดเฉลี่ย ต่ำกว่าค่าที่กำหนด");
                }
            }
        }
        public void GetAgeChild()// ตรวจสอบ อายุบุตร
        {
            string ls_brithdate = "";
            if (dsEducation.DATA[0].CHILD_BIRTH_DATE.ToString("dd/MM/yyyy") != "01/01/1500" && dsEducation.DATA[0].CHILD_BIRTH_DATE.ToString("dd/MM/yyyy") != "")
            {
                ls_brithdate = dsEducation.DATA[0].CHILD_BIRTH_DATE.ToString("dd/MM/yyyy");

                sqlStr = "select FT_CALAGE(to_date('" + ls_brithdate + "','dd/mm/yyyy'), sysdate, 4) child_age from assreqmaster where rownum = 1 ";
                sqlStr = WebUtil.SQLFormat(sqlStr);
                Sdt dt = WebUtil.QuerySdt(sqlStr);

                if (dt.Next())
                {
                    dsEducation.DATA[0].child_age = dt.GetDecimal("child_age");
                }
            }
            else
            {
                dsEducation.DATA[0].child_age = 0;
            }
        }

        public void CelTreatDate()
        {
            DateTime date_start_treat = dsPatronize.DATA[0].start_treat;
            DateTime date_end_treat = dsPatronize.DATA[0].end_treat;

            string sqlStr = @"select * from assucfyear where coop_id = {0} and ass_year = {1} and start_year <= {2} and end_year >= {3}";
            sqlStr = WebUtil.SQLFormat(sqlStr, state.SsCoopId, dsAssisttype.DATA[0].ass_year, date_start_treat, date_end_treat);
            Sdt dt = WebUtil.QuerySdt(sqlStr);
            if (dt.Next())// เช็ดวันที่รักษาอยู่ในปีสวัสดิการนี้หรือไม่
            {
                string sqlStr1 = @"select (to_date({1}, 'dd/mm/yyyy') - to_date({2}, 'dd/mm/yyyy'))  cal_treat, num_pay from assucfassisttypedet where assisttype_code = {0}";
                sqlStr1 = WebUtil.SQLFormat(sqlStr1, dsAssisttype.DATA[0].assisttype_code, date_end_treat.ToString("dd/MM/yyyy"), date_start_treat.ToString("dd/MM/yyyy"));
                Sdt dt1 = WebUtil.QuerySdt(sqlStr1);

                string sqlStr2 = @"select sum(treatend_date - treatstart_date) use_day,count(treatend_date - treatstart_date) count_use from assreqmaster where req_status = 1 and member_no = {0} and assisttype_code = {1}";
                sqlStr2 = WebUtil.SQLFormat(sqlStr2, dsMain.DATA[0].member_no, dsAssisttype.DATA[0].assisttype_code);
                Sdt dt2 = WebUtil.QuerySdt(sqlStr2);

                dt1.Next();
                dt2.Next();
                int li_caltreat = 1;
                int li_caltreat_true = 1;
                int li_count_use = (dt2.GetInt32("count_use"));
               
                if (state.SsCoopId == "031001" || state.SsCoopId == "031002") //fix สวัสดิการค่ารักษาพยาบาล กรมชล
                {
                    decimal use_allday = (dt2.GetInt32("count_use"));
                    if (Math.Ceiling(use_allday) >= (dt1.GetInt32("num_pay") / 5))
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage("ท่านขอสิทธิ์ สวัสดิการประเภทนี้ครบแล้ว");
                        li_caltreat = 0;
                        dsPatronize.FindTextBox(0, "start_treat").Enabled = false;
                        dsPatronize.FindTextBox(0, "end_treat").Enabled = false;
                        dsPatronize.FindTextBox(0, "cal_date").Enabled = false;
                    }
                    else if (li_count_use<3)
                    {

                        li_caltreat = (dt1.GetInt32("cal_treat"))+1;
                    }
                    
                    else
                    {
                        li_count_use = 3 - li_count_use;
                        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('ท่านมีสิทธิ์ ในสวัสดิการประเภทนี้เหลือ " + li_count_use + "ครั้ง(" + li_count_use*5+") วัน');", true);
                        dsPatronize.DATA[0].start_treat = state.SsWorkDate;
                        dsPatronize.DATA[0].end_treat = state.SsWorkDate;
                        li_caltreat = 1;
                    }
                    if (li_caltreat != 0)
                    {
                        if (li_caltreat < 3)
                        {
                           
                            if (li_caltreat != 1){
                                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('ท่านนอนโรงพยาบบาลไม่ถึง 3 วัน');", true);
                            }
                            li_caltreat = 0;
                            dsPatronize.DATA[0].start_treat = state.SsWorkDate;
                            dsPatronize.DATA[0].end_treat = state.SsWorkDate;
                        }
                        else if (li_caltreat > 5)
                        {
                            li_caltreat_true = li_caltreat;
                            li_caltreat = 5;
                            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('ท่านสามารถรับสวัสดิการนี้ได้สูงสุด 5 วัน');", true);
                            //DateTime date_end_treat2 = dsPatronize.DATA[0].end_treat;
                            //dsPatronize.DATA[0].start_treat = date_end_treat2.AddDays(-(li_caltreat));

                        }
                        else 
                        { 
                            li_caltreat_true = li_caltreat; 
                        }
                    }
                }
                else { 
                 if (dt2.GetInt32("use_day") >= dt1.GetInt32("num_pay"))
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ท่านขอสิทธิ์ สวัสดิการประเภทนี้ครบแล้ว");
                    li_caltreat = 0;
                    dsPatronize.FindTextBox(0, "start_treat").Enabled = false;
                    dsPatronize.FindTextBox(0, "end_treat").Enabled = false;
                    dsPatronize.FindTextBox(0, "cal_date").Enabled = false;
                }
                else if (dt1.GetInt32("cal_treat") + dt2.GetInt32("use_day") <= dt1.GetInt32("num_pay"))
                {
                    li_caltreat = (dt1.GetInt32("cal_treat"));
                }
                else if (dt1.GetInt32("num_pay") - dt2.GetInt32("use_day") >= dt1.GetInt32("cal_treat"))
                {
                    li_caltreat = (dt1.GetInt32("cal_treat"));
                }
                else
                {
                    li_caltreat = dt1.GetInt32("cal_treat") - (dt1.GetInt32("cal_treat") - (dt1.GetInt32("num_pay") - (dt2.GetInt32("use_day"))));
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('ท่านมีสิทธิ์ ในสวัสดิการประเภทนี้เหลือ " + li_caltreat + " วัน');", true);
                    dsPatronize.DATA[0].start_treat = state.SsWorkDate;
                    dsPatronize.DATA[0].end_treat = state.SsWorkDate;
                    li_caltreat = 1;
                }
                }
                dsPatronize.DATA[0].cal_date = li_caltreat_true;
                //this.SetOnLoadedScript("document.getElementById('txt_date').style.display = 'block'; document.getElementById('txt_to').style.display = 'block'; document.getElementById('txt_day').style.display = 'block';");
                //GetBaht(li_caltreat);
                hdCalDay.Value = li_caltreat.ToString();
            }
            else
            {
                string sqlStr3 = @"select * from assucfyear where coop_id = {0} and ass_year = {1}";
                sqlStr3 = WebUtil.SQLFormat(sqlStr3, state.SsCoopId, dsAssisttype.DATA[0].ass_year);
                Sdt dt3 = WebUtil.QuerySdt(sqlStr3);
                dt3.Next();

                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('วันที่รักษาไม่ได้อยู่ในช่วงปีสวัสดิการ กรุณาตรวจสอบ');", true);
                dsPatronize.DATA[0].start_treat = dt3.GetDate("end_year");
                dsPatronize.DATA[0].end_treat = dt3.GetDate("end_year");
                hdCalDay.Value = "0";
            } 
        }
        public void GetMonryEver(string ls_memno, string ls_asstype)
        {
            sqlStr = @"select * from assucfassisttype where coop_id = {0} and assisttype_code = {1}";
            sqlStr = WebUtil.SQLFormat(sqlStr, state.SsCoopId, ls_asstype);
            Sdt dt1 = WebUtil.QuerySdt(sqlStr);
            dt1.Next();
            string ls_assgroup = dt1.GetString("assisttype_group");

            if (ls_assgroup == "02") { ls_asstype = "21"; } //เฉพาะสมาชิกเสียชีวิต เอาเงินที่เคยได้อาวุโสมาแสดง

            sqlStr = @"select 
                        sum(payout_amt) ever_amt 
                    from assslippayout 
                    where slip_status = 1 and coop_control = {0} and member_no = {1} and assisttype_code = {2}";
            sqlStr = WebUtil.SQLFormat(sqlStr, state.SsCoopControl, ls_memno, ls_asstype);
            dt1 = WebUtil.QuerySdt(sqlStr);
            dt1.Next();
            dsAmount.DATA[0].ever_amt = dt1.GetDecimal("ever_amt");
        }

        public void WebSheetLoadEnd()
        {
            if (dsAmount.DATA[0].MONEYTYPE_CODE == "CSH" || dsAmount.DATA[0].MONEYTYPE_CODE == "")
            {
                dsAmount.FindDropDownList(0, "expense_bank").Enabled = false;
                dsAmount.FindTextBox(0, "expense_accid").Enabled = false;
                dsAmount.FindDropDownList(0, "expense_branch").Enabled = false;
                dsAmount.FindDropDownList(0, "send_system").Enabled = false;
                dsAmount.FindDropDownList(0, "deptaccount_no").Enabled = false;
            }
            else if (dsAmount.DATA[0].MONEYTYPE_CODE == "TRN")
            {
                dsAmount.FindDropDownList(0, "expense_bank").Enabled = false;
                dsAmount.FindTextBox(0, "expense_accid").Enabled = false;
                dsAmount.FindDropDownList(0, "expense_branch").Enabled = false;
                dsAmount.FindDropDownList(0, "send_system").Enabled = true;
                dsAmount.FindDropDownList(0, "deptaccount_no").Enabled = true;
            }
            else
            {
                dsAmount.FindDropDownList(0, "expense_bank").Enabled = true;
                dsAmount.FindTextBox(0, "expense_accid").Enabled = true;
                dsAmount.FindDropDownList(0, "expense_branch").Enabled = true;
                dsAmount.FindDropDownList(0, "send_system").Enabled = false;
                dsAmount.FindDropDownList(0, "deptaccount_no").Enabled = false;
            }

            //bee
            string ls_chktap = "00";
            ls_asstype = dsAssisttype.DATA[0].assisttype_code;
            sqlStr = @"select * from ASSUCFASSISTTYPE where assisttype_code = {0}";
            sqlStr = WebUtil.SQLFormat(sqlStr, ls_asstype);
            Sdt dt = WebUtil.QuerySdt(sqlStr);
            if (dt.Next())
            {
                ls_chktap = dt.GetString("assisttype_group");
            }
            if (ls_chktap == "02")
            {
                this.SetOnLoadedScript("document.getElementById('insertRow').style.display = 'block';");
            }
        }
    }
}