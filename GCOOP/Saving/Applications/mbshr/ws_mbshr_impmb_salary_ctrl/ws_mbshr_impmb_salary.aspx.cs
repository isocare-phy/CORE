using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using System.IO;
using System.Data.OleDb;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using CoreSavingLibrary.WcfNCommon;
using DataLibrary;

namespace Saving.Applications.mbshr.ws_mbshr_impmb_salary_ctrl
{
    public partial class ws_mbshr_impmb_salary : PageWebSheet, WebSheet
    {
        [JsPostBack]
        public string PostImport { get; set; }
        [JsPostBack]
        public string PostUpdateSal { get; set; }
        [JsPostBack]
        public string PostUpdateSalParttime { get; set; }
        [JsPostBack]
        public string PostUpdateShare { get; set; }
        [JsPostBack]
        public string PostUpdateMembGroup { get; set; }
        [JsPostBack]
        public string PostUpdateMembGroupParttime { get; set; }
        [JsPostBack]
        public string PostMembName { get; set; }
        [JsPostBack]
        public string PostMembSurname { get; set; }
        [JsPostBack]
        public string PostUpdateMinShare { get; set; }

        ExecuteDataSource exc;
        Sdt dt = new Sdt();
        Sdt dt2 = new Sdt();
        public string ckmbgroupcode = "";
        public void InitJsPostBack()
        {
            
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {

            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "PostImport")
            {
                string[] segments= txtInput.FileName.Split('.');
                string fileExt = segments[segments.Length - 1].ToLower();
                if (fileExt == "txt")
                {
                    PostImpText();
                }
                else
                {

                }
            }
            else if (eventArg == "PostUpdateSal")
            {
                string membtype = "10";
                PostUpdateSals(membtype);
            }
            else if (eventArg == "PostUpdateSalParttime")
            {
                string membtype = "14";
                PostUpdateSals(membtype);
            }
            else if (eventArg == "PostUpdateShare")
            {
                PostUpdateShares();
            }
            else if (eventArg == "PostUpdateMembGroup")
            {
                string membtype = "10";
                PostCheckMbgroup();
                if (ckmbgroupcode == "" || ckmbgroupcode == null)
                {
                    PostUpdateMembGroups(membtype);
                }
            }
            else if (eventArg == "PostUpdateMembGroupParttime")
            {
                string membtype = "14";
                PostUpdateMembGroups(membtype);
            }
            else if (eventArg == "PostMembName")
            {
                PostMembNames();
            }
            else if (eventArg == "PostMembSurname")
            {
                PostMembSurnames();
            }
            else if (eventArg == "PostUpdateMinShare")
            {
                PostUpdateMinShares();
            }
        }

        public void SaveWebSheet()
        {
            
        }

        public void WebSheetLoadEnd()
        {
            
        }

        private void PostImpText()
        {
            exc = new ExecuteDataSource(this);
            string ls_sql = "", ls_salaryid = "", ls_prename = "", ls_membname = "";
            string ls_membsurname = "", ls_positiondesc = "", ls_positioncode = "", ls_membgroupcode = "";
            decimal ldm_salaryamt = 0;
            try
            {
                FileUpload fu = txtInput;
                string filname = txtInput.FileName.ToString().Trim();
                Stream stream = fu.PostedFile.InputStream;
                byte[] bt = new byte[stream.Length];
                stream.Read(bt, 0, (int)stream.Length);
                string txt = Encoding.Unicode.GetString(bt);
                txt = Regex.Replace(txt, "\r", "");
                string[] lines = Regex.Split(txt, "\n");
                int txtLength;
                int n = 1;
                string[] txtindex;

                // ลบข้อมูลเดิมทิ้งก่อน
                exc.SQL.Add("delete from mbmembmasterimp");

                foreach (string line in lines)
                {
                    txtLength = line.Length;
                    txtindex = line.Split('\t');
                    try { ls_salaryid = Convert.ToString(txtindex[0]); }
                    catch { ls_salaryid = ""; }
                    ls_salaryid = Regex.Replace(ls_salaryid, @"[^\w\d]", "");
                    try { ls_prename = Convert.ToString(txtindex[1]).Trim(); }
                    catch { ls_prename = ""; }
                    try { ls_membname = Convert.ToString(txtindex[2]).Trim(); }
                    catch { ls_membname = ""; }
                    try { ls_membsurname = Convert.ToString(txtindex[3]).Trim(); }
                    catch { ls_membsurname = ""; }
                    try { ls_positiondesc = Convert.ToString(txtindex[4].Trim()); }
                    catch { ls_positiondesc = ""; }
                    try { ldm_salaryamt = System.Math.Abs(Convert.ToDecimal(txtindex[5])); }
                    catch { ldm_salaryamt = 0; }
                    try { ls_positioncode = Convert.ToString(txtindex[6].Trim()); }
                    catch { ls_positioncode = ""; }                    
                    try { ls_membgroupcode = Convert.ToString(txtindex[7].Trim()); }
                    catch { ls_membgroupcode = ""; }

                    //intsert ข้อมูลไปพักไว้ก่อน
                    ls_sql = @"insert into mbmembmasterimp values({0},{1},{2},{3},{4},{5},{6},{7},{8},{9})";
                    ls_sql = WebUtil.SQLFormat(ls_sql, state.SsCoopControl, state.SsWorkDate, ls_salaryid, ls_prename, ls_membname, ls_membsurname, ls_positiondesc, ldm_salaryamt, ls_positioncode, ls_membgroupcode);
                    exc.SQL.Add(ls_sql);
                    
                }
                exc.Execute();
                exc.SQL.Clear();
                LtServerMessage.Text = WebUtil.CompleteMessage("Import Complete");
            }
            catch (Exception ex)
            { LtServerMessage.Text = WebUtil.ErrorMessage("เกิดข้อผิดพลาดในการ IMPORT ข้อมูล" + ex); }
        }
        /// <summary>
        /// ปรับปรุงเงินเดือน
        /// </summary>
        private void PostUpdateSals(string membtype)
        {
            string se, se_where, se_order, sql, up;
            string adjslip_no, adjsal_type, sharetype_code, member_no;
            string entry_id;
            decimal old_salary, old_sharebase, old_shareperiod;
            decimal new_salary, new_sharebase, new_shareperiod, posting_flag;
            Int16 rowcount = 0;
            DateTime operate_date, entry_date;
            try
            {
                if(membtype == "10")
                {
                    se_where = "and mi.salary_amount != mb.salary_amount ";
                }
                else
                {
                    se_where = "and mi.salary_amount * 22 != mb.salary_amount ";
                }
                se_order = "order by mb.member_no";
                se = @"select count(1) as crow from mbmembmasterimp mi, mbmembmaster mb, shsharemaster sm
                where mi.salary_id = mb.salary_id
                and mb.member_no = sm.member_no
                and mb.resign_status = 0
                and mb.membtype_code = {0} ";
                se = se + se_where + se_order;
                se = WebUtil.SQLFormat(se, membtype);
                dt = WebUtil.QuerySdt(se);
                if (dt.Next())
                {
                    rowcount = Convert.ToInt16(dt.GetDecimal("crow"));
                }
                for (Int16 i = 1; i <= rowcount; i++)
                {
                    adjslip_no = wcf.NCommon.of_getnewdocno(state.SsWsPass, state.SsCoopId, "MBADJSAL");
                    se = @"select 'MEM' as adjsal_type, sm.sharetype_code as sharetype_code, mb.member_no as member_no, mb.salary_amount as old_salary, 
                    sm.periodbase_amt as old_sharebase, sm.periodshare_amt as old_shareperiod, mi.salary_amount as new_salary, 1 as posting_flag
                    from mbmembmasterimp mi, mbmembmaster mb, shsharemaster sm
                    where mi.salary_id = mb.salary_id
                    and mb.member_no = sm.member_no
                    and mb.resign_status = 0
                    and mb.membtype_code = {0} ";
                    se = se + se_where + se_order;
                    se = WebUtil.SQLFormat(se, membtype);
                    dt2 = WebUtil.QuerySdt(se);
                    if (dt2.Next())
                    {
                        adjsal_type = dt2.GetString("adjsal_type");
                        sharetype_code = dt2.GetString("sharetype_code");
                        member_no = dt2.GetString("member_no");
                        old_salary = dt2.GetDecimal("old_salary");
                        old_sharebase = dt2.GetDecimal("old_sharebase");
                        old_shareperiod = dt2.GetDecimal("old_shareperiod");
                        new_salary = dt2.GetDecimal("new_salary");
                        new_sharebase = dt2.GetDecimal("old_sharebase");
                        new_shareperiod = dt2.GetDecimal("old_shareperiod");
                        posting_flag = dt2.GetDecimal("posting_flag");
                        operate_date = state.SsWorkDate;
                        entry_id = state.SsUsername;
                        entry_date = DateTime.Now;
                        if (membtype == "14")
                        { new_salary = new_salary * 22; }

                        sql = @"insert into mbadjsalary values({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15})";
                        sql = WebUtil.SQLFormat(sql, state.SsCoopControl, adjslip_no, adjsal_type, operate_date, sharetype_code, member_no, old_salary, old_sharebase, old_shareperiod,
                            new_salary, new_sharebase, new_shareperiod, posting_flag, entry_id, entry_date, state.SsCoopId);
                        dt = WebUtil.QuerySdt(sql);

                        up = @"update mbmembmaster set salary_amount = " + new_salary + " where member_no = '" + member_no + "'";
                        dt = WebUtil.QuerySdt(up);
                    }
                }
                LtServerMessage.Text = WebUtil.CompleteMessage("ปรับปรับข้อมูลเงินเดือนสำเร็จ");
            }
            catch (Exception ex)
            { LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถปรับปรับข้อมูลเงินเดือนได้ " + ex.Message); }
        }

        /// <summary>
        /// ปรับปรุงหุ้นฐานประจำปี
        /// </summary>
        private void PostUpdateShares()
        {
            //shpaymentadjustyear
            string member_no = "", payadjust_docno = "";
            decimal sharebegin_amt = 0, sharestk_amt = 0, last_period = 0, periodshare_amt = 0, salary_amount = 0;
            decimal minshare_amt = 0, unitshare_value = 0, new_periodvalue = 0, old_periodvalue = 0;
            try
            {
                string sqlim = @"select mb.member_no, sm.sharebegin_amt, sm.sharestk_amt, sm.last_period, sm.periodshare_amt, mb.salary_amount, st.minshare_amt
                    from mbmembmasterimp mi, mbmembmaster mb, shsharemaster sm, shsharetypemthrate st
                    where mi.salary_id = mb.salary_id
                    and mb.member_status = 1
                    and mb.resign_status = 0
                    and mb.member_no = sm.member_no
                    and mb.salary_amount between st.start_salary and st.end_salary
                    and sm.periodshare_amt < st.minshare_amt
				    and sm.sharetype_code = st.sharetype_code
                    order by mb.member_no";
                dt = WebUtil.QuerySdt(sqlim);
                while (dt.Next())
                {
                    member_no = dt.GetString("member_no");
                    last_period = dt.GetDecimal("last_period");
                    sharebegin_amt = dt.GetDecimal("sharebegin_amt");
                    sharestk_amt = dt.GetDecimal("sharestk_amt");
                    periodshare_amt = dt.GetDecimal("periodshare_amt");
                    salary_amount = dt.GetDecimal("salary_amount");

                    string getsh = @"select sr.minshare_amt, st.unitshare_value, (sr.minshare_amt * st.unitshare_value) as new_periodvalue from shsharetypemthrate sr, shsharetype st
                        where sr.coop_id = st.coop_id
                        and sr.sharetype_code = st.sharetype_code
                        and sr.member_type = 1
                        and {0} >= sr.start_salary
                        and {0} <= sr.end_salary";
                    getsh = WebUtil.SQLFormat(getsh, salary_amount);
                    Sdt sd = WebUtil.QuerySdt(getsh);
                    if (sd.Next())
                    {
                        minshare_amt = sd.GetDecimal("minshare_amt");
                        unitshare_value = sd.GetDecimal("unitshare_value");
                        new_periodvalue = sd.GetDecimal("new_periodvalue");
                    }
                    old_periodvalue = periodshare_amt * unitshare_value;

                    if (old_periodvalue < new_periodvalue)
                    {
                        payadjust_docno = wcf.NCommon.of_getnewdocno(state.SsWsPass, state.SsCoopControl, "MBADJSHRYEARDOCNO");
                        //insert ตารางเปลี่ยนแปลงประจำปี
                        string insertshyear = @"INSERT INTO SHPAYMENTADJUSTYEAR (COOP_ID, PAYADJUST_DOCNO , MEMBER_NO , PAYADJUST_DATE , SHAREBEGIN_VALUE , SHARESTK_VALUE , 
                        SHRLAST_PERIOD , PERIODBASE_VALUE , OLD_PERIODVALUE , OLD_PAYSTATUS , NEW_PERIODVALUE , NEW_PAYSTATUS , SHRPAYADJ_STATUS , APVIMMEDIATE_FLAG , 
                        REMARK , CHGSTOP_FLAG , CHGCONT_FLAG , CHGADD_FLAG , CHGLOW_FLAG , ENTRY_ID , ENTRY_DATE , APPROVE_ID , APPROVE_DATE ,
                        ENTRY_BYCOOPID , MEMCOOP_ID ) values 
                        ({0}, {1}, {2}, {3}, {4}, {5}, 
                         {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13},
                         {14}, {15}, {16}, {17}, {18}, {19}, {20}, {21}, {22},
                         {23}, {24})";
                        insertshyear = WebUtil.SQLFormat(insertshyear, state.SsCoopControl, payadjust_docno, member_no, state.SsWorkDate, sharebegin_amt, sharestk_amt,
                            periodshare_amt, new_periodvalue, old_periodvalue, 1, new_periodvalue, 1, 1, 0,
                            "ปรับเงินเดือนประจำปี/ ปรับฐานหุ้นประจำปี", 0, 0, 1, 0, state.SsUsername, state.SsWorkDate, state.SsUsername, state.SsWorkDate,
                            state.SsCoopId, state.SsCoopControl);
                        Sdt dt1 = WebUtil.QuerySdt(insertshyear);
                        //update ตาราง shsharemaster
                        string upshr = @"update shsharemaster set periodshare_amt = {0}, periodbase_amt = {1} where member_no = {2}";
                        upshr = WebUtil.SQLFormat(upshr, minshare_amt, minshare_amt, member_no);
                        Sdt dt2 = WebUtil.QuerySdt(upshr);
                    }
                }

                LtServerMessage.Text = WebUtil.CompleteMessage("ปรับข้อมูลฐานหุ้นประจำปีสำเร็จ");
            }
            catch (Exception ex)
            { LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message); }
        }

        /// <summary>
        /// ต้องการปรับปรุงฐานหุ้นขั้นต่ำตามเงินเดือนสมาชิก
        /// </summary>
        private void PostUpdateMinShares()
        {
            try
            {
                string sqlupbaseshr = @"update shsharemaster sm 
                                        set sm.periodbase_amt = (select st.minshare_amt from mbmembmaster mb, shsharetypemthrate st
										                                        where sm.member_no = mb.member_no
										                                        and sm.sharetype_code = st.sharetype_code
										                                        and mb.salary_amount between st.start_salary and st.end_salary
										                                        and sm.periodbase_amt <> st.minshare_amt
										                                        and mb.resign_status = 0
										                                        and trim( mb.membgroup_code ) between 'A' and 'F90000' )
                                        where sm.member_no in (select mb.member_no from mbmembmaster mb, shsharetypemthrate st
										                                        where sm.member_no = mb.member_no
										                                        and sm.sharetype_code = st.sharetype_code
										                                        and mb.salary_amount between st.start_salary and st.end_salary
										                                        and sm.periodbase_amt <> st.minshare_amt
										                                        and mb.resign_status = 0
										                                        and trim( mb.membgroup_code ) between 'A' and 'F90000' )";
                dt = WebUtil.QuerySdt(sqlupbaseshr);
                LtServerMessage.Text = WebUtil.CompleteMessage("ปรับปรุงฐานหุ้นขั้นต่ำตามเงินเดือนสมาชิก สำเร็จ");
            }
            catch(Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        /// <summary>
        /// ปรับปรุงสังกัดสมาชิก
        /// </summary>
        private void PostUpdateMembGroups(string membtype)
        {
            //mbreqchangegroup
            string se;
            string chggroup_docno, member_no, old_group, new_group, expchg_flag;
            string oldexp_code, oldexp_bank, oldexp_branch, oldexp_accid, expense_code;
            string expense_bank, expense_branch, expense_accid;
            decimal request_status ;
            Int16 rowcount = 0;
            DateTime req_date;
            try
            {
                se = @"select count(1) as crow from mbmembmasterimp mi, mbmembmaster mb
                where mi.salary_id = mb.salary_id
                and mb.resign_status = 0
                and mb.membtype_code = {0}
                and mi.membgroup_code <> trim(mb.membgroup_code)
                order by mb.member_no";
                se = WebUtil.SQLFormat(se, membtype);
                dt = WebUtil.QuerySdt(se);
                if (dt.Next())
                {
                    rowcount = Convert.ToInt16(dt.GetDecimal("crow"));
                }
                for(Int16 i = 1; i <= rowcount; i++)
                {
                    chggroup_docno = wcf.NCommon.of_getnewdocno(state.SsWsPass, state.SsCoopId, "MBCHGGROUP");
                    se = @"select mb.coop_id as coop_id, '' as chggroup_docno, mb.coop_id as membcoop_id, mb.member_no as member_no,mb.membgroup_code as oldmembgroup_code, 
                    mi.membgroup_code as newmembgroup_code, 'N' as expchg_flag, mb.expense_code as expense_code, mb.expense_bank as expense_bank, mb.expense_branch as expense_branch, 
                    mb.expense_accid as expense_accid,1 as request_status
                    from mbmembmasterimp mi, mbmembmaster mb
                    where mi.salary_id = mb.salary_id
                    and mb.resign_status = 0
                    and mb.membtype_code = {0}
                    and mi.membgroup_code <> trim(mb.membgroup_code)
                    order by mb.member_no";
                    se = WebUtil.SQLFormat(se, membtype);
                    dt2 = WebUtil.QuerySdt(se);
                    if (dt2.Next())
                    {
                        member_no = dt2.GetString("member_no");
                        old_group = dt2.GetString("oldmembgroup_code").Trim();
                        new_group = dt2.GetString("newmembgroup_code").Trim();
                        expchg_flag = dt2.GetString("expchg_flag");
                        oldexp_code = dt2.GetString("expense_code").Trim();
                        oldexp_bank = dt2.GetString("expense_bank").Trim();
                        oldexp_branch = dt2.GetString("expense_branch").Trim();
                        oldexp_accid = dt2.GetString("expense_accid").Trim();
                        expense_code = dt2.GetString("expense_code").Trim();
                        expense_bank = dt2.GetString("expense_bank").Trim();
                        expense_branch = dt2.GetString("expense_branch").Trim();
                        expense_accid = dt2.GetString("expense_accid").Trim();
                        request_status = dt2.GetDecimal("request_status");
                        req_date = state.SsWorkDate;

                        int resmbreq = of_insertmbreq(chggroup_docno, member_no, req_date, old_group, new_group, expchg_flag, oldexp_code, oldexp_bank, oldexp_branch, oldexp_accid, expense_code, expense_bank, expense_branch, expense_accid, request_status);
                        if (resmbreq != 1)
                        {
                            throw new Exception("ไม่สามารถทำการบันทึกใบคำขอเปลี่ยงแปลงสังกัดได้ !!!");
                        }

                        int resupmemb = of_updatemembgroup(member_no, new_group);
                        if (resupmemb != 1)
                        {
                            throw new Exception("ไม่สามารถทำการปรับปรุงสังกัดใหม่ของสมาชิกได้ !!!");
                        }
                    }
                }
                LtServerMessage.Text = WebUtil.CompleteMessage("ปรับปรุงข้อมูลสังกัดสมาชิก สำเร็จ");
            }
            catch (Exception ex)
            { LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถทำรายการปรับปรุงข้อมูลสังกัดสมาชิก" + ex.Message); }
           
        }

        private int of_insertmbreq(string chggroup_docno, string member_no, DateTime req_date, string old_group, string new_group, string expchg_flag, string oldexp_code, string oldexp_bank, string oldexp_branch, string oldexp_accid, string expense_code, string expense_bank, string expense_branch, string expense_accid, decimal request_status)
        {
            int res = 0;
            string sql, entry_id, entry_bycoopid, apv_id;
            DateTime entry_date, apv_date;
            entry_id = state.SsUsername;
            entry_bycoopid = state.SsCoopId;
            apv_id = state.SsUsername;
            entry_date = DateTime.Now;
            apv_date = state.SsWorkDate;
            try
            {
                sql = @"insert into mbreqchangegroup values({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21})";
                sql = WebUtil.SQLFormat(sql, state.SsCoopControl, chggroup_docno, state.SsCoopId, member_no, req_date, old_group, new_group, expchg_flag, oldexp_code, oldexp_bank,
                    oldexp_branch, oldexp_accid, expense_code, expense_bank, expense_branch, expense_accid, request_status, entry_id, entry_date, entry_bycoopid, apv_id, apv_date);
                dt = WebUtil.QuerySdt(sql);
                res = 1;
            }
            catch { res = -9; }
            return res;         
        }

        private int of_updatemembgroup(string Membno, string Membgrp)
        {
            int res = 0;
            string up;
            try
            {
                up = @"update mbmembmaster set membgroup_code = '" + Membgrp + "' where member_no = '" + Membno + "'";
                dt = WebUtil.QuerySdt(up);
                res = 1;
            }
            catch { res = -9; }
            return res;
        }

        /// <summary>
        /// ปรับปรุงข้อมูลชื่อสมาชิก
        /// </summary>
        private void PostMembNames()
        {
            string se, se2, sedoc, sqllog, sqllogdet, up;
            string modtb_code, modtb_tbname, clmkey_name, clmkey_desc;
            string clm_name, clmold_desc, clmnew_desc, clmtype_desc;
            string entry_id, entry_ip;
            decimal seq_no = 1 ;
            DateTime entry_date;
            string modtbdoc_no = "";
            Int16 rowcount = 0;
            try
            {
                se = @"select count(1) as crow from mbmembmasterimp mi, mbmembmaster mb
                        where mi.salary_id = mb.salary_id
                        and mb.resign_status = 0
                        and trim(mi.memb_name) != trim(mb.memb_name)
                        and mi.salary_id != '590000019'
                        order by mb.member_no";
                se = WebUtil.SQLFormat(se);
                dt = WebUtil.QuerySdt(se);
                if (dt.Next())
                {
                    rowcount = Convert.ToInt16(dt.GetDecimal("crow"));
                }
                if (rowcount == 0)
                { this.SetOnLoadedScript("alert('ไม่มีข้อมูลเปลี่ยนแปลงชื่อสมาชิก สำหรับปรับปรุง !!!')"); }

                for(Int16 i = 1; i <= rowcount; i++)
                {
                    sedoc = @"select to_char(max(modtbdoc_no) + 1,'000000000000000') as modtbdoc_no from sys_logmodtb order by modtbdoc_no"; //gen docno syslogmodtb
                    Sdt dt3 = WebUtil.QuerySdt(sedoc);
                    if (dt3.Next())
                    {
                        modtbdoc_no = dt3.GetString("modtbdoc_no");
                    }
                    se2 = @"select 'U' as modtb_code, 'MBMEMBMASTER' as modtb_tbname, 'MEMBER_NO' as clmkey_name, mb.member_no as member_no,
                            mi.memb_name as clmnew_desc, mb.memb_name as clmold_desc, dd.data_type as data_type, dd.column_name as column_name
                            from mbmembmasterimp mi, mbmembmaster mb, ( select data_type, column_name
                            FROM user_tab_columns where table_name = 'MBMEMBMASTER'
                            and column_name = 'MEMB_NAME') dd
                            where mi.salary_id = mb.salary_id
                            and mb.resign_status = 0
                            and mi.salary_id != '590000019'
                            and trim(mi.memb_name) != trim(mb.memb_name)
                            order by mb.member_no";
                    dt2 = WebUtil.QuerySdt(se2);
                    if (dt2.Next())
                    {
                        modtb_code = dt2.GetString("modtb_code");
                        modtb_tbname = dt2.GetString("modtb_tbname");
                        clmkey_name = dt2.GetString("clmkey_name");
                        clmkey_desc = dt2.GetString("member_no");
                        entry_id = state.SsUsername;
                        entry_ip = state.SsClientIp;
                        clm_name = dt2.GetString("column_name");
                        clmold_desc = dt2.GetString("clmold_desc");
                        clmnew_desc = dt2.GetString("clmnew_desc");
                        clmtype_desc = dt2.GetString("data_type");
                        entry_date = DateTime.Now;
                        sqllog = @"insert into sys_logmodtb values({0},{1},{2},{3},{4},{5},{6},{7},{8})";
                        sqllog = WebUtil.SQLFormat(sqllog, state.SsCoopControl, modtbdoc_no, modtb_code, modtb_tbname, clmkey_name, clmkey_desc, entry_id, entry_date, entry_ip);
                        dt = WebUtil.QuerySdt(sqllog);

                        sqllogdet = @"insert into sys_logmodtbdet values({0},{1},{2},{3},{4},{5},{6})";
                        sqllogdet = WebUtil.SQLFormat(sqllogdet, state.SsCoopControl, modtbdoc_no, seq_no, clm_name, clmold_desc, clmnew_desc, clmtype_desc);
                        dt = WebUtil.QuerySdt(sqllogdet);

                        up = @"update mbmembmaster set memb_name = '" + clmnew_desc + "' where member_no = '" + clmkey_desc + "'";
                        dt = WebUtil.QuerySdt(up);

                    }
                }
                if (rowcount == 0)
                { }
                else { LtServerMessage.Text = WebUtil.CompleteMessage("ปรับปรุงข้อมูลชื่อสมาชิก สำเร็จ"); }
            }
            catch (Exception ex)
            { LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถทำรายการปรับปรุงข้อมูลชื่อสมาชิก " + ex.Message); }
        }

        /// <summary>
        /// ปรับปรุงข้อมูลชื่อสกุลสมาชิก
        /// </summary>
        private void PostMembSurnames()
        {
            string se, se2, sedoc, sqllog, sqllogdet, up;
            string modtb_code, modtb_tbname, clmkey_name, clmkey_desc;
            string clm_name, clmold_desc, clmnew_desc, clmtype_desc;
            string entry_id, entry_ip;
            decimal seq_no = 1;
            DateTime entry_date;
            string modtbdoc_no = "";
            Int16 rowcount = 0;
            try
            {
                se = @"select count(1) as crow from mbmembmasterimp mi, mbmembmaster mb
                        where mi.salary_id = mb.salary_id
                        and mb.resign_status = 0
                        and trim(mi.memb_surname) != trim(mb.memb_surname)
                        and mi.salary_id != '590000019'
                        order by mb.member_no";
                se = WebUtil.SQLFormat(se);
                dt = WebUtil.QuerySdt(se);
                if (dt.Next())
                {
                    rowcount = Convert.ToInt16(dt.GetDecimal("crow"));
                }
                if (rowcount == 0)
                { this.SetOnLoadedScript("alert('ไม่มีข้อมูลเปลี่ยนแปลงชื่อสมาชิก สำหรับปรับปรุง !!!')"); }

                for (Int16 i = 1; i <= rowcount; i++)
                {
                    sedoc = @"select to_char(max(modtbdoc_no) + 1,'000000000000000') as modtbdoc_no from sys_logmodtb order by modtbdoc_no"; //gen docno syslogmodtb
                    Sdt dt3 = WebUtil.QuerySdt(sedoc);
                    if (dt3.Next())
                    {
                        modtbdoc_no = dt3.GetString("modtbdoc_no");
                    }
                    se2 = @"select 'U' as modtb_code, 'MBMEMBMASTER' as modtb_tbname, 'MEMBER_NO' as clmkey_name, mb.member_no as member_no,
                            mi.memb_surname as clmnew_desc, mb.memb_surname as clmold_desc, dd.data_type as data_type, dd.column_name as column_name
                            from mbmembmasterimp mi, mbmembmaster mb, ( select data_type, column_name
                            FROM user_tab_columns where table_name = 'MBMEMBMASTER'
                            and column_name = 'MEMB_SURNAME') dd
                            where mi.salary_id = mb.salary_id
                            and mb.resign_status = 0
                            and mi.salary_id != '590000019'
                            and trim(mi.memb_surname) != trim(mb.memb_surname)
                            order by mb.member_no";
                    dt2 = WebUtil.QuerySdt(se2);
                    if (dt2.Next())
                    {
                        modtb_code = dt2.GetString("modtb_code");
                        modtb_tbname = dt2.GetString("modtb_tbname");
                        clmkey_name = dt2.GetString("clmkey_name");
                        clmkey_desc = dt2.GetString("member_no");
                        entry_id = state.SsUsername;
                        entry_ip = state.SsClientIp;
                        clm_name = dt2.GetString("column_name");
                        clmold_desc = dt2.GetString("clmold_desc");
                        clmnew_desc = dt2.GetString("clmnew_desc");
                        clmtype_desc = dt2.GetString("data_type");
                        entry_date = DateTime.Now;
                        sqllog = @"insert into sys_logmodtb values({0},{1},{2},{3},{4},{5},{6},{7},{8})";
                        sqllog = WebUtil.SQLFormat(sqllog, state.SsCoopControl, modtbdoc_no, modtb_code, modtb_tbname, clmkey_name, clmkey_desc, entry_id, entry_date, entry_ip);
                        dt = WebUtil.QuerySdt(sqllog);

                        sqllogdet = @"insert into sys_logmodtbdet values({0},{1},{2},{3},{4},{5},{6})";
                        sqllogdet = WebUtil.SQLFormat(sqllogdet, state.SsCoopControl, modtbdoc_no, seq_no, clm_name, clmold_desc, clmnew_desc, clmtype_desc);
                        dt = WebUtil.QuerySdt(sqllogdet);

                        up = @"update mbmembmaster set memb_surname = '" + clmnew_desc + "' where member_no = '" + clmkey_desc + "'";
                        dt = WebUtil.QuerySdt(up);

                    }
                }
                if (rowcount == 0)
                { }
                else { LtServerMessage.Text = WebUtil.CompleteMessage("ปรับปรุงข้อมูลชื่อสกุลสมาชิก สำเร็จ"); }
            }
            catch (Exception ex)
            { LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถทำรายการปรับปรุงข้อมูลชื่อสกุลสมาชิก " + ex.Message); }
        }

        /// <summary>
        /// เช็คสังกัดสมาชิกก่อนว่า มีในค่าคงที่ไหม ถ้าไม่มียังไม่ให้ import
        /// </summary>
        private void PostCheckMbgroup()
        {
            string seckmbgroup = @"select distinct trim(membgroup_code) as membgroup_code from mbmembmasterimp
                                    minus 
                                    select trim(membgroup_code) as membgroup_code from mbucfmembgroup";
            Sdt dt3 = WebUtil.QuerySdt(seckmbgroup);
            while (dt3.Next())
            {
                ckmbgroupcode += dt3.GetString("membgroup_code") + ",";
            }
            if (ckmbgroupcode != null && ckmbgroupcode != "")
            {
                this.SetOnLoadedScript("alert('" + ckmbgroupcode + " กรุณเพิ่มรหัสสังกัดในระบบก่อนคะ \\nเนื่องจากสังกัดที่แสดงอาจจะเป็นสังกัดใหม่')");
            }
        }
    }
}