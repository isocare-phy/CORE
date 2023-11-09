using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using DataLibrary;
using CoreSavingLibrary.WcfNShrlon;
using System.Data;

namespace Saving.Applications.assist.ws_sheet_as_genrequest_ctrl
{
    public partial class ws_sheet_as_genrequest : PageWebSheet, WebSheet
    {

        [JsPostBack]
        public string PostProcess { get; set; }
        [JsPostBack]
        public string PostSave { get; set; } 
        [JsPostBack]
        public string PostDefaultAcc { get; set; } 
        [JsPostBack]
        public string PostBank { get; set; } 
        [JsPostBack]
        public string PostBranch { get; set; } 
       
        public void InitJsPostBack()
        {
            dsMain.InitDsMain(this);
            dsList.InitDsList(this);
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)//show data first  
            {
                dsMain.GetAssYear();

                // แก้ปัญหาถ้าไม่ active dropdown ปี มัน get ค่า 0 มา
                string sqlStr = @"select max(ass_year + 543) ass_year from assucfyear where coop_id = {0}";
                sqlStr = WebUtil.SQLFormat(sqlStr, state.SsCoopId);
                Sdt dt1 = WebUtil.QuerySdt(sqlStr);
                dt1.Next();
                dsMain.DATA[0].process_year = dt1.GetString("ass_year");
                dsMain.DATA[0].process_month = state.SsWorkDate.Month.ToString();

                dsMain.AssistType();
                dsMain.DdMoneyType();
                dsMain.DdFromAccId();
                dsMain.DATA[0].work_date = state.SsWorkDate.ToString("dd/MM/") + (state.SsWorkDate.Year + 543).ToString();
                GetDefaultAcc(); //get เงินสด accid 
                this.SetOnLoadedScript("dsMain.GetElement(0, 'expense_bank').style.background = '#CCCCCC'; dsMain.GetElement(0, 'expense_bank').readOnly = true; dsMain.GetElement(0, 'expense_branch').style.background = '#CCCCCC'; dsMain.GetElement(0, 'expense_branch').readOnly = true;");
            }
            

        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == PostProcess)
            {
                dsList.ResetRow();
                GetShowList();
            }
            else if (eventArg == PostDefaultAcc)
            {
                GetDefaultAcc();
            }
            else if (eventArg == PostBank)
            {
                dsMain.DdBankDesc();
                InitTofromaccid();
            }
            else if (eventArg == PostBranch)
            {
                String bank = dsMain.DATA[0].expense_bank;
                dsMain.DdBranch(bank);
            }
            else if (eventArg == PostSave)
            {
                SaveData();
            }
        }

        private void GetDefaultAcc()
        {
            string sql = @"select cash_account_code from accconstant";
            sql = WebUtil.SQLFormat(sql);
            Sdt dt = WebUtil.QuerySdt(sql);
            if (dt.Next())
            {
                dsMain.DATA[0].tofrom_accid = dt.GetString("cash_account_code");
            }
        }

        private void InitTofromaccid()
        {
            string sql = @" 
                       SELECT MONEYTYPE_CODE,  
                              MONEYTYPE_GROUP, 
                              MONEYTYPE_DESC,   
                              SORT_ORDER  ,
                              MONEYTYPE_CODE || ' - '|| MONEYTYPE_DESC as MONEYTYPE_DISPLAY,
                              DEFAULTPAY_ACCID
                         FROM CMUCFMONEYTYPE WHERE  MONEYTYPE_GROUP IN('BNK','CHQ','CSH')  AND MONEYTYPE_CODE={0}  order by sort_order
                ";
            sql = WebUtil.SQLFormat(sql, dsMain.DATA[0].moneytype_code);
            Sdt dt = WebUtil.QuerySdt(sql);
            if (dt.Next())
            {
                dsMain.DATA[0].tofrom_accid = dt.GetString("DEFAULTPAY_ACCID").Trim();
            }
            else
            {
                dsMain.DATA[0].tofrom_accid = "0";
            }
                dsMain.DdBranch(dsMain.DATA[0].expense_bank);
        }
        private void GetShowList()
        {
            string ls_asscode = dsMain.DATA[0].assisttype_code;
            string ls_memtypecode, ls_assisgroup,  ls_yearmonth, sqlStr;
            int  li_agemem = 0, li_memtypeflag = 0, li_calflag = 0, li_proflag = 0;
            decimal dec_amount = 0, dec_prncbal = 0, dec_maxpayamt = 0;
           
            //เช็คว่าเป็นการคำนวณแบบเดือน หรือ ปี
            string sql_chk1 = @"select * from assucfassisttype where coop_id = {0} and assisttype_code = {1}";
            sql_chk1 = WebUtil.SQLFormat(sql_chk1, state.SsCoopId, ls_asscode);
            Sdt dt_chk1 = WebUtil.QuerySdt(sql_chk1);
            if (dt_chk1.Next())
            {
                li_calflag = dt_chk1.GetInt32("calculate_flag");
                li_memtypeflag = dt_chk1.GetInt32("membtype_flag");
                ls_assisgroup = dt_chk1.GetString("assisttype_group");
                li_proflag = dt_chk1.GetInt32("process_flag");// 1 = เดือน 2 = ปี
            }

            if (li_proflag == 2)
            {
                string ls_year = dsMain.DATA[0].process_year;            
                ls_yearmonth = Convert.ToString(Convert.ToDecimal(ls_year) - 1) + dsMain.DATA[0].process_month;
            }
            else { 
                ls_yearmonth = dsMain.DATA[0].process_year + dsMain.DATA[0].process_month; 
            }

            //ท่อนดึงข้อมูลผู้ที่มีสิทธิ์ได้รับสวัสดิการ
            sqlStr = @"
            select asm.member_no , ft_getmemname(asm.coop_id , asm.member_no) full_name , mb.birth_date ,ft_calagemth( mb.birth_date ,sysdate ) mem_age
            ,ft_calagemth( mb.member_date ,sysdate ) work_age, ast.slip_date
            ,asm.withdrawable_amt, asm.last_periodpay ,mb.membtype_code,asm.assisttype_code,ast.moneytype_code
            from  asscontmaster asm ,
              asscontstatement ast,
              mbmembmaster mb
            where   asm.asscontract_no = ast.asscontract_no
            and     asm.member_no = mb.member_no
            and     asm.payment_status = 1
            and     asm.max_periodpay > asm.last_periodpay
            and     asm.assisttype_code = {1}
            and     ast.seq_no = (select max( asscontstatement.seq_no ) from asscontstatement where asscontstatement.asscontract_no =  asm.asscontract_no)
            and     to_char(slip_date, 'yyyy') + 543 || to_char(slip_date, 'mm')  <= {2}
            and     asm.coop_id = {0} 
            and    mb.resign_status = 0
            and   asm.member_no not in
		    (
			select assreqmaster.member_no from  assreqmaster where
		    assreqmaster.assisttype_code=asm.assisttype_code  and assreqmaster.assistpay_code=asm.assistpaytype_code
		    and req_date={3} and req_status<>-9 ) order by to_char(slip_date, 'yyyy') + 543 || to_char(slip_date, 'mmdd') desc ,mb.member_no ";
            sqlStr = WebUtil.SQLFormat(sqlStr, state.SsCoopControl, ls_asscode, ls_yearmonth, state.SsWorkDate);
            Sdt dt = WebUtil.QuerySdt(sqlStr);
            dt = WebUtil.QuerySdt(sqlStr);
            if (dt.Next())
            {
                for (int ii = 0; ii < dt.GetRowCount(); ii++)
                {
                    dsList.InsertAtRow(ii);
                    dsList.DATA[ii].choose_flag = 1;
                    dsList.DATA[ii].member_no = dt.Rows[ii]["member_no"].ToString();
                    dsList.DATA[ii].full_name = dt.Rows[ii]["full_name"].ToString();
                    dsList.DATA[ii].birth_date = Convert.ToDateTime(dt.Rows[ii]["birth_date"]);
                    dsList.DATA[ii].moneytype_code = dt.Rows[ii]["moneytype_code"].ToString();
                    dsList.DATA[ii].work_age = dt.Rows[ii]["work_age"].ToString();
                    dsList.DATA[ii].mem_age = dt.Rows[ii]["mem_age"].ToString();
                    dsList.DATA[ii].slip_date = Convert.ToDateTime(dt.Rows[ii]["slip_date"]);
                    dec_prncbal = Convert.ToDecimal(dt.Rows[ii]["withdrawable_amt"]);
                    dsList.DATA[ii].last_periodpay = dt.Rows[ii]["last_periodpay"].ToString();
                    if (li_memtypeflag == 0)
                    {
                        ls_memtypecode = "%"; // ไม่เช็ด membtype_code
                    }
                    else
                    {
                        ls_memtypecode = dt.GetString("membtype_code");
                    }
                    string[] ls_arr_mem = dsList.DATA[ii].mem_age.Split('.');
                    string[] ls_arr_work_age = dsList.DATA[ii].work_age.Split('.');

                    if (li_calflag == 1) //เกรด
                    {
                    }
                    else if (li_calflag == 2) //อายุ
                    {
                        li_agemem = (Convert.ToInt32(ls_arr_mem[0]) * 12) + Convert.ToInt32(ls_arr_mem[1]); //อายุสมาชิก
                    }
                    else if (li_calflag == 3) //อายุการเป็นสมาชิก
                    {
                        li_agemem = (Convert.ToInt32(ls_arr_work_age[0]) * 12) + Convert.ToInt32(ls_arr_work_age[1]); //อายุการเป็นสมาชิก
                    }
                    else if (li_calflag == 4) //เงินเดือน
                    {
                    }
                    else if (li_calflag == 5) //ค่าเสียหาย
                    {
                    }
                    else if (li_calflag == 6) //ตามประเภทการจ่าย
                    {
                    }

                    sql_chk1 = "select " +
                        "   max_seqno," +
                        "   assucfassisttypedet.* " +
                        " from assucfassisttype " +
                        " inner join assucfassisttypedet on assucfassisttype.assisttype_code = assucfassisttypedet.assisttype_code " +
                        " inner join (" +
                        " select assisttype_code, max(seq_no) max_seqno " +
                        " from assucfassisttypedet " +
                        " where coop_id = '" + state.SsCoopControl + "' and assisttype_code = '" + ls_asscode + "' and membtype_code like '" + ls_memtypecode + "' group by assisttype_code " +
                        " )chk_seq on assucfassisttypedet.assisttype_code = chk_seq.assisttype_code " +
                        " where assucfassisttype.coop_id = '" + state.SsCoopControl + "'" +
                        " and assucfassisttype.assisttype_code = '" + ls_asscode + "'" +
                        " and assucfassisttypedet.membtype_code like '" + ls_memtypecode + "'";
                    dt_chk1 = WebUtil.QuerySdt(sql_chk1);
                    if (dt_chk1.Next())
                    {
                        if (dt_chk1.GetInt32("max_seqno") > 1)
                        {
                            if (li_calflag == 2 || li_calflag == 3) // ใช้อายุหรือ อายุสมาชิกคำนวณ
                            {
                                for (int ii_chk1 = 0; ii_chk1 < dt_chk1.Rows.Count; ii_chk1++)
                                {
                                    int ln_minchk = Convert.ToInt32(dt_chk1.Rows[ii_chk1]["min_check"]);
                                    int ln_maxchk = Convert.ToInt32(dt_chk1.Rows[ii_chk1]["max_check"]);
                                    int ln_nexttypepay = Convert.ToInt32(dt_chk1.Rows[ii_chk1]["next_typepay"]);
                                    if (ln_minchk <= li_agemem && li_agemem < ln_maxchk)
                                    {
                                        if (ln_nexttypepay == 1) //บาท
                                        {
                                            dec_maxpayamt = Convert.ToInt32(dt_chk1.Rows[ii_chk1]["max_payamt"]);
                                            dec_amount = Convert.ToInt32(dt_chk1.Rows[ii_chk1]["next_payamt"]);
                                        }
                                        else // เปอร์เซ็น
                                        {
                                            dec_maxpayamt = Convert.ToInt32(dt_chk1.Rows[ii_chk1]["max_payamt"]) ;
                                            dec_amount = (Convert.ToInt32(dt_chk1.Rows[ii_chk1]["max_payamt"]) * Convert.ToInt32(dt_chk1.Rows[ii_chk1]["next_payamt"]))  / 100;
                                        }
                                        if (dec_amount > Convert.ToInt32(dt_chk1.Rows[ii_chk1]["max_nextpayamt"]))
                                        {
                                            dec_amount = Convert.ToInt32(dt_chk1.Rows[ii_chk1]["max_nextpayamt"]);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (dt_chk1.GetInt32("next_typepay") == 1) //บาท
                            {
                                dec_amount = dt_chk1.GetInt32("next_payamt");
                            }
                            else // เปอร์เซ็น
                            {
                                dec_maxpayamt = dt_chk1.GetInt32("max_payamt");
                                dec_amount = (dt_chk1.GetInt32("max_payamt") * dt_chk1.GetInt32("next_payamt")) / 100;
                            }
                            if (dec_amount > dt_chk1.GetDecimal("max_nextpayamt"))
                            {
                                dec_amount = dt_chk1.GetDecimal("max_nextpayamt");
                            }
                        }
                    }
                    dsList.DATA[ii].max_payamt = dec_maxpayamt;
                    dsList.DATA[ii].approve_amt = dec_amount;
                    dsList.DATA[ii].prncbal = dec_prncbal;//- dec_amount
                }
            }
            if (dsList.RowCount == 0)
            {
                string ls_monthdesc = "";
                switch (dsMain.DATA[0].process_month)
                {
                    case "01":
                        ls_monthdesc = "มกราคม"; break;
                    case "02":
                        ls_monthdesc = "กุมภาพันธ์"; break;
                    case "03":
                        ls_monthdesc = "มีนาคม"; break;
                    case "04":
                        ls_monthdesc = "เมษายน"; break;
                    case "05":
                        ls_monthdesc = "พฤษภาคม"; break;
                    case "06":
                        ls_monthdesc = "มิถุนายน"; break;
                    case "07":
                        ls_monthdesc = "กรกฎาคม"; break;
                    case "08":
                        ls_monthdesc = "สิงหาคม"; break;
                    case "09":
                        ls_monthdesc = "กันยายน"; break;
                    case "10":
                        ls_monthdesc = "ตุลาคม"; break;
                    case "11":
                        ls_monthdesc = "พฤศจิกายน"; break;
                    case "12":
                        ls_monthdesc = "ธันวาคม"; break;
                }

                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบสิทธิ์ของสมาชิกในการขอสวัสดิการประเภทนี้ ในปี " + dsMain.DATA[0].process_year + " เดือน" + ls_monthdesc);
                return;
            }
            else
            {
                dsMain.DATA[0].all_check = 1;
                LtServerMessage.Text = WebUtil.CompleteMessage("มีรายการใบคำขอทั้งหมด " + dsList.RowCount + " รายการ");
            }
        }

        #region WebSheetGetShow
        //        //ของ พี่เก่ง
//        public void GetShowList2()
//        {
//            string ls_asscode = dsMain.DATA[0].assisttype_code;
//            string ls_saveassno = "", ls_keepassdocno = "", ls_memtypecode, ls_assisgroup, ls_chkmem = "", ls_yearmonth;
//            int li_savelastno = 0, li_agemem = 0, li_memtypeflag = 0, li_calflag = 0, li_proflag = 0, li_row = 0;
//            string ls_year = dsMain.DATA[0].process_year;

//            if (dsMain.DATA[0].process_month.Length == 1)
//            {
//                ls_yearmonth = dsMain.DATA[0].process_year + '0' + dsMain.DATA[0].process_month;
//            }
//            else
//            {
//                ls_yearmonth = dsMain.DATA[0].process_year + dsMain.DATA[0].process_month;
//            }

//            decimal dec_amount = 0;
//            try
//            {
//                String sqlStr = @"select 
//	                        last_documentno + 1 last_doc,
//                            document_year,
//	                        document_prefix || substr(document_year, 3, 2) || REPLACE(substr(document_format, 5, length(substr(document_format, 5, 6)) - length(last_documentno + 1)) || cast(last_documentno + 1 as varchar(6)), 'R', '0') assist_no
//                        from cmdocumentcontrol where document_code = 'ASSISTDOCNO'";
//                sqlStr = WebUtil.SQLFormat(sqlStr);
//                Sdt dt = WebUtil.QuerySdt(sqlStr);
//                if (dt.Next())
//                {
//                    ls_saveassno = dt.GetString("assist_no");
//                    ls_keepassdocno = dt.GetString("assist_no");
//                    li_savelastno = dt.GetInt32("last_doc");
//                    //li_year = dt.GetInt32("document_year");
//                }

//                sqlStr = @"
//select * from 
//(
//    select 
//	    mas.member_no,
//	    to_char(mb.birth_date, 'dd/mm/') || cast(to_char(mb.birth_date, 'yyyy') + 543 as varchar(4)) birth_date, 
//        birth_date order_birthdate,
//	    FT_CALAGE(mb.birth_date, sysdate, 4) age,
//	    FT_CALAGE(mb.member_date, sysdate, 4) work_age,
//	    pre.prename_desc || mb.memb_name || ' ' || mb.memb_surname full_name,
//	    stm.last_period,
//        stm.last_year,
//        mb.membtype_code
//    from (select * from asscontmaster where max_periodpay > last_periodpay) mas
//    inner join (select * from mbmembmaster where resign_status = 0) mb on mas.member_no = mb.member_no
//    inner join mbucfprename pre on mb.prename_code = pre.prename_code
//    inner join 
//    (
//	    select max(prov.last_period) last_period, max(prov.last_year) last_year, prov.asscontract_no
//	    from(
//		    select to_char(slip_date, 'yyyy') + 543 || to_char(slip_date, 'mm') last_period, to_char(slip_date, 'yyyy') + 543 last_year, asscontstatement.* from asscontstatement where item_status = 1 and coop_control = {0}
//	    )prov
//	    left join(
//		    select to_char(slip_date, 'yyyy') + 543 || to_char(slip_date, 'mm') last_period, to_char(slip_date, 'yyyy') + 543 last_year, asscontstatement.* from asscontstatement where item_status = -9 and coop_control = {0}
//	    )cel on prov.asscontract_no = cel.asscontract_no and prov.period = cel.period
//	    where cel.asscontract_no is null
//	    group by prov.asscontract_no
//    )stm on mas.asscontract_no = stm.asscontract_no
//    where mas.coop_control = {0} and mas.assisttype_code = {1} and mas.max_periodpay > mas.last_periodpay
//)where last_period < {2} 
//and member_no not in (select member_no from assreqmaster where coop_control = {0} and assisttype_code = {1} and req_status in ('8', '1') and to_char(req_date, 'yyyy') + 543 || to_char(req_date, 'mm') = {2})
//order by order_birthdate, member_no";
//                sqlStr = WebUtil.SQLFormat(sqlStr, state.SsCoopControl, ls_asscode, ls_yearmonth);
//                dt = WebUtil.QuerySdt(sqlStr);

//                string sql_chk1 = @"select * from assucfassisttype where coop_id = {0} and assisttype_code = {1}";
//                sql_chk1 = WebUtil.SQLFormat(sql_chk1, state.SsCoopId, ls_asscode);
//                Sdt dt_chk1 = WebUtil.QuerySdt(sql_chk1);
//                if (dt_chk1.Next())
//                {
//                    li_calflag = dt_chk1.GetInt32("calculate_flag");
//                    li_memtypeflag = dt_chk1.GetInt32("membtype_flag");
//                    ls_assisgroup = dt_chk1.GetString("assisttype_group");
//                    li_proflag = dt_chk1.GetInt32("process_flag");// 1 = เดือน 2 = ปี
//                }



//                for (int ii = 0; ii < dt.GetRowCount(); ii++)
//                {
//                    dt.Next();//main

//                    if (li_proflag == 2)
//                    {
//                        if (dsMain.DATA[0].process_year == dt.GetString("last_year"))
//                        {
//                            li_row = li_row + 1;
//                            goto skip;
//                        }
//                    }
//                    int li_pulsassno;
//                    dsList.InsertAtRow(ii - li_row);

//                    if (dsList.DATA[0].assist_docno == "") // เฉพาะรอบแรก
//                    {
//                        li_pulsassno = Convert.ToInt32(ls_keepassdocno.Substring(2, ls_keepassdocno.Length - 2)) + ii;
//                        ls_saveassno = ls_saveassno.Substring(0, 2) + li_pulsassno;
//                        //li_savelastno = li_savelastno + 1;
//                    }
//                    else
//                    {
//                        li_pulsassno = Convert.ToInt32((dsList.DATA[ii - li_row - 1].assist_docno).Substring(2, (dsList.DATA[ii - li_row - 1].assist_docno).Length - 2)) + 1;
//                        ls_saveassno = (dsList.DATA[ii - li_row - 1].assist_docno).Substring(0, 2) + li_pulsassno;
//                        li_savelastno = li_savelastno + 1;
//                    }

//                    dsList.DATA[ii - li_row].choose_flag = 1;
//                    dsList.DATA[ii - li_row].assist_docno = ls_saveassno;
//                    dsList.DATA[ii - li_row].birth_date = dt.GetString("birth_date");
//                    dsList.DATA[ii - li_row].age = dt.GetString("age");
//                    dsList.DATA[ii - li_row].work_age = dt.GetString("work_age");
//                    dsList.DATA[ii - li_row].member_no = dt.GetString("member_no");
//                    dsList.DATA[ii - li_row].full_name = dt.GetString("full_name");

//                    if (li_memtypeflag == 0)
//                    {
//                        ls_memtypecode = "%"; // ไม่เช็ด membtype_code
//                    }
//                    else
//                    {
//                        ls_memtypecode = dt.GetString("membtype_code");
//                    }
//                    string[] ls_arr_mem = dsList.DATA[ii - li_row].age.Split('.');
//                    string[] ls_arr_work_age = dsList.DATA[ii - li_row].work_age.Split('.');

//                    if (li_calflag == 1) //เกรด
//                    {
//                    }
//                    else if (li_calflag == 2) //อายุ
//                    {
//                        li_agemem = (Convert.ToInt32(ls_arr_mem[0]) * 12) + Convert.ToInt32(ls_arr_mem[1]); //อายุสมาชิก
//                    }
//                    else if (li_calflag == 3) //อายุการเป็นสมาชิก
//                    {
//                        li_agemem = (Convert.ToInt32(ls_arr_work_age[0]) * 12) + Convert.ToInt32(ls_arr_work_age[1]); //อายุการเป็นสมาชิก
//                    }
//                    else if (li_calflag == 4) //เงินเดือน
//                    {
//                    }
//                    else if (li_calflag == 5) //ค่าเสียหาย
//                    {
//                    }
//                    else if (li_calflag == 6) //ตามประเภทการจ่าย
//                    {
//                    }

//                    sql_chk1 = "select " +
//                        "   max_seqno," +
//                        "   assucfassisttypedet.* " +
//                        " from assucfassisttype " +
//                        " inner join assucfassisttypedet on assucfassisttype.assisttype_code = assucfassisttypedet.assisttype_code " +
//                        " inner join (" +
//                        " select assisttype_code, max(seq_no) max_seqno " +
//                        " from assucfassisttypedet " +
//                        " where coop_id = '" + state.SsCoopControl + "' and assisttype_code = '" + ls_asscode + "' and membtype_code like '" + ls_memtypecode + "' group by assisttype_code " +
//                        " )chk_seq on assucfassisttypedet.assisttype_code = chk_seq.assisttype_code " +
//                        " where assucfassisttype.coop_id = '" + state.SsCoopControl + "'" +
//                        " and assucfassisttype.assisttype_code = '" + ls_asscode + "'" +
//                        " and assucfassisttypedet.membtype_code like '" + ls_memtypecode + "'";
//                    dt_chk1 = WebUtil.QuerySdt(sql_chk1);
//                    if (dt_chk1.Next())
//                    {
//                        if (dt_chk1.GetInt32("max_seqno") > 1)
//                        {
//                            if (li_calflag == 2 || li_calflag == 3) // ใช้อายุหรือ อายุสมาชิกคำนวณ
//                            {
//                                for (int ii_chk1 = 0; ii_chk1 < dt_chk1.GetInt32("max_seqno"); ii_chk1++)
//                                {
//                                    if (dt_chk1.GetInt32("min_check") <= li_agemem && li_agemem < dt_chk1.GetInt32("max_check"))
//                                    {
//                                        if (dt_chk1.GetInt32("next_typepay") == 1) //บาท
//                                        {
//                                            dec_amount = dt_chk1.GetInt32("next_payamt");
//                                        }
//                                        else // เปอร์เซ็น
//                                        {
//                                            dec_amount = (dt_chk1.GetInt32("max_payamt") * dt_chk1.GetInt32("next_payamt")) / 100;
//                                        }
//                                        if (dec_amount > dt_chk1.GetDecimal("max_nextpayamt"))
//                                        {
//                                            dec_amount = dt_chk1.GetDecimal("max_nextpayamt");
//                                        }
//                                        goto next_step;
//                                    }
//                                    dt_chk1.Next();
//                                }
//                            }
//                        }
//                        else
//                        {
//                            if (dt_chk1.GetInt32("next_typepay") == 1) //บาท
//                            {
//                                dec_amount = dt_chk1.GetInt32("next_payamt");
//                            }
//                            else // เปอร์เซ็น
//                            {
//                                dec_amount = (dt_chk1.GetInt32("max_payamt") * dt_chk1.GetInt32("next_payamt")) / 100;
//                            }
//                            if (dec_amount > dt_chk1.GetDecimal("max_nextpayamt"))
//                            {
//                                dec_amount = dt_chk1.GetDecimal("max_nextpayamt");
//                            }
//                        }
//                    }
//                next_step:
//                    dsList.DATA[ii - li_row].approve_amt = dec_amount;
//                skip:
//                    li_row = li_row;
//                }
//                if (dsList.RowCount == 0)
//                {
//                    string ls_monthdesc = "";
//                    switch (dsMain.DATA[0].process_month)
//                    {
//                        case "01":
//                            ls_monthdesc = "มกราคม"; break;
//                        case "02":
//                            ls_monthdesc = "กุมภาพันธ์"; break;
//                        case "03":
//                            ls_monthdesc = "มีนาคม"; break;
//                        case "04":
//                            ls_monthdesc = "เมษายน"; break;
//                        case "05":
//                            ls_monthdesc = "พฤษภาคม"; break;
//                        case "06":
//                            ls_monthdesc = "มิถุนายน"; break;
//                        case "07":
//                            ls_monthdesc = "กรกฎาคม"; break;
//                        case "08":
//                            ls_monthdesc = "สิงหาคม"; break;
//                        case "09":
//                            ls_monthdesc = "กันยายน"; break;
//                        case "10":
//                            ls_monthdesc = "ตุลาคม"; break;
//                        case "11":
//                            ls_monthdesc = "พฤศจิกายน"; break;
//                        case "12":
//                            ls_monthdesc = "ธันวาคม"; break;
//                    }

//                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบสิทธิ์ของสมาชิกในการขอสวัสดิการประเภทนี้ ในปี " + dsMain.DATA[0].process_year + " เดือน" + ls_monthdesc);
//                    return;
//                }
//                else
//                {
//                    dsMain.DATA[0].all_check = 1;
//                    LtServerMessage.Text = WebUtil.CompleteMessage("มีรายการใบคำขอทั้งหมด " + dsList.RowCount + " รายการ");
//                }
//            }
//            catch
//            {
//                LtServerMessage.Text = WebUtil.ErrorMessage("Error");
//                return;
//            }
//        }
        #endregion
        public void SaveData()
        {
            try
            {
                string ls_asstypepay = "00";
                string sql = @"select
	                        assucfassistpaytype.assistpay_code
                        from assucfassisttype
                        inner join assucfassistpaytype on assucfassisttype.assisttype_group = assucfassistpaytype.assisttype_group
                        where assucfassisttype.coop_id = {0} and assucfassisttype.assisttype_code = {1}
                        and assucfassistpaytype.assistpay_desc like '%ตนเอง%'";
                sql = WebUtil.SQLFormat(sql, state.SsCoopId, dsMain.DATA[0].assisttype_code);
                Sdt dt = WebUtil.QuerySdt(sql);
                if (dt.Next()) { ls_asstypepay = dt.GetString("assistpay_code"); }

                string ls_reqdate = dsMain.DATA[0].work_date.Substring(0, 6) + (Convert.ToInt32(dsMain.DATA[0].work_date.Substring(6, 4)) - 543);
                for (int ii = 0; ii < dsList.RowCount; ii++)
                {
                    if (dsList.DATA[ii].choose_flag == 1)
                    {
                        string ls_assno = wcf.NCommon.of_getnewdocno(state.SsWsPass, state.SsCoopId, "ASSISTDOCNO");                        
                        string ls_memno = dsList.DATA[ii].member_no;
                        decimal dec_appamt = dsList.DATA[ii].approve_amt;

                        try
                        {
                            string sqlStr = @"insert into assreqmaster
                            (coop_id                    , coop_control              , assist_docno          , assist_year           , member_no 
                            , assisttype_code           , assistpay_code            , approve_amt           , assist_amt            , req_status
                            , req_date                  , approve_date              , entry_id              , moneytype_code        , expense_bank
                            , expense_branch            , expense_accid             , withdrawable_amt)
                            values
                            ({0}                        , {1}                       , {2}                   , {3}                   , {4}
                            , {5}                       , {6}                       , {7}                   , {8}                   , {9}
                            , to_date({10},'dd/mm/yyyy'), to_date({11},'dd/mm/yyyy'), {12}                  , {13}                  , {14}
                            , {15}                      , {16}                      , {17})";
                            sqlStr = WebUtil.SQLFormat(sqlStr,
                                state.SsCoopId, state.SsCoopControl, ls_assno, dsMain.DATA[0].process_year, ls_memno
                                , dsMain.DATA[0].assisttype_code, ls_asstypepay, dec_appamt, dec_appamt, "1"
                                , ls_reqdate, ls_reqdate, state.SsUsername, dsMain.DATA[0].moneytype_code, dsMain.DATA[0].expense_bank
                                , dsMain.DATA[0].expense_branch, dsMain.DATA[0].tofrom_accid, dec_appamt);
                            WebUtil.ExeSQL(sqlStr);
                        }
                        catch
                        {
                            LtServerMessage.Text = WebUtil.ErrorMessage("บันทึกผิดพลาด เลขสมาชิก " + ls_memno); return;
                        }
                    }
                }
                dsList.ResetRow();
                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
            }
            catch
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("Error"); return;
            }
        }

        public void SaveWebSheet()
        {
            
        }

        public void WebSheetLoadEnd()
        {
            if (dsMain.DATA[0].moneytype_code == "CSH")
            {
                this.SetOnLoadedScript("dsMain.GetElement(0, 'expense_bank').style.background = '#CCCCCC'; dsMain.GetElement(0, 'expense_bank').readOnly = true; dsMain.GetElement(0, 'expense_branch').style.background = '#CCCCCC'; dsMain.GetElement(0, 'expense_branch').readOnly = true;");
            }
        }
    }

}