using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using DataLibrary;
using System.Data;
using System.Drawing;

namespace Saving.Applications.mbshr.ws_mbshr_adt_mbmoney_ctrl
{
    public partial class ws_mbshr_adt_mbmoney : PageWebSheet, WebSheet
    {
        [JsPostBack]
        public string PostMember { get; set; }
        [JsPostBack]
        public string PostInsertRow { get; set; }
        [JsPostBack]
        public string PostExpenseBank { get; set; }
        [JsPostBack]
        public string PostExpenseBankKeep { get; set; }
        [JsPostBack]
        public string PostExpenseBankDiv { get; set; }
        [JsPostBack]
        public string PostBank { get; set; }
        [JsPostBack]
        public string PostBankKeep { get; set; }
        [JsPostBack]
        public string PostBankDiv { get; set; }
        [JsPostBack]
        public string PostExpenseBranch { get; set; }
        [JsPostBack]
        public string PostExpenseBranchKeep { get; set; }
        [JsPostBack]
        public string PostExpenseBranchDiv { get; set; }
        [JsPostBack]
        public string PostSetAccDiv { get; set; }
        [JsPostBack]
        public string PostDeleteRow { get; set; }

        public void InitJsPostBack()
        {
            dsMain.InitDsMain(this);
            dsMaster.InitDsMaster(this);
            dsKeep.InitDsKeep(this);
            dsDiv.InitDsDiv(this);
            dsList.InitDsList(this);
        }

        public void WebSheetLoadBegin()
        {
            throw new NotImplementedException();
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == PostMember)
            {
                string member_no = WebUtil.MemberNoFormat(dsMain.DATA[0].member_no);
                dsMain.RetrieveMain(member_no);
                dsList.RetrieveList(member_no);
                //บัญชีหลัก
                dsMaster.RetrieveMaster(member_no);
                dsMaster.DdBank();
                string expense_bank = dsMaster.DATA[0].EXPENSE_BANK;
                dsMaster.DATA[0].bank_name = expense_bank;
                string bank_code = dsMaster.DATA[0].bank_name;
                dsMaster.DdBranch(bank_code);
                string expense_branch = dsMaster.DATA[0].EXPENSE_BRANCH;
                dsMaster.DATA[0].branch_name = expense_branch;
                ////บัญชีเรียกเก็บ
                dsKeep.RetrieveKeep(member_no);
                dsKeep.DdBank();
                string bank_code_keep = dsKeep.DATA[0].BANK_CODE;
                dsKeep.DATA[0].bank_name = bank_code_keep;
                string bank_name_keep = dsKeep.DATA[0].bank_name;
                dsKeep.DdBranch(bank_name_keep);
                string bank_branch = dsKeep.DATA[0].BANK_BRANCH;
                dsKeep.DATA[0].branch_name = bank_branch;                
                //บัญชีปันผล
                dsDiv.RetrieveDiv(member_no);
                string sqlStr = @"select moneytype_code,
		                        bank_code,
		                        bank_branch,
		                        bank_accid
                         from mbmembmoneytr 
                        where coop_id={0}   and
                        member_no={1} and
                        trtype_code ='DVAV1'";
                sqlStr = WebUtil.SQLFormat(sqlStr, state.SsCoopControl, member_no);
                Sdt dt = WebUtil.QuerySdt(sqlStr);
                if (dt.Next())
                {
                    //เช็คปันผล
                }
                else {
                    dsDiv.DATA[0].acc_flag = 1;
                    //SetAccDiv();
                }
                dsDiv.DdBank();
                string bank_code_div = dsDiv.DATA[0].BANK_CODE;
                dsDiv.DATA[0].bank_name = bank_code_div;
                string bank_name_div = dsDiv.DATA[0].bank_name;
                dsDiv.DdBranch(bank_name_div);
                string bank_branch_div = dsDiv.DATA[0].BANK_BRANCH;
                dsDiv.DATA[0].branch_name = bank_branch_div;
                dsDiv.RetrieveDDWType();
                string ls_type = dsDiv.DATA[0].MONEYTYPE_CODE;
                dsDiv.DATA[0].moneytype_desc = ls_type;

            }
            else if (eventArg == PostInsertRow)
            {
                dsList.InsertLastRow();
                dsList.DdMoneytrtype();
                dsList.DdMoneytype();
                dsList.DdBank();
            }
            else if (eventArg == PostExpenseBank)
            {
                string expense_bank = dsMaster.DATA[0].EXPENSE_BANK;
                dsMaster.DATA[0].bank_name = expense_bank;
                dsMaster.DdBank();
                dsMaster.DdBranch(expense_bank);
                dsMaster.DATA[0].EXPENSE_BRANCH = "";
                dsMaster.DATA[0].branch_name = "";
            }
            else if (eventArg == PostExpenseBankKeep)
            {
                string expense_bank = dsKeep.DATA[0].BANK_CODE;
                dsKeep.DATA[0].bank_name = expense_bank;
                dsKeep.DdBank();
                dsKeep.DdBranch(expense_bank);
                dsKeep.DATA[0].BANK_BRANCH = "";
                dsKeep.DATA[0].branch_name = "";
            }
            else if (eventArg == PostExpenseBankDiv)
            {
                string expense_bank = dsDiv.DATA[0].BANK_CODE;
                dsDiv.DATA[0].bank_name = expense_bank;
                dsDiv.DdBank();
                dsDiv.DdBranch(expense_bank);
                dsDiv.DATA[0].BANK_BRANCH = "";
                dsDiv.DATA[0].branch_name = "";
            }
            else if (eventArg == PostBank)
            {
                string bank_code = dsMaster.DATA[0].bank_name;
                dsMaster.DATA[0].EXPENSE_BANK = bank_code;
                dsMaster.DATA[0].EXPENSE_BRANCH = "";
                dsMaster.DATA[0].branch_name = "";
                dsMaster.DdBranch(bank_code);
            }
            else if (eventArg == PostBankKeep)
            {
                string bank_code = dsKeep.DATA[0].bank_name;
                dsKeep.DATA[0].BANK_CODE = bank_code;
                dsKeep.DATA[0].BANK_BRANCH = "";
                dsKeep.DATA[0].branch_name = "";
                dsKeep.DdBranch(bank_code);
            }
            else if (eventArg == PostBankDiv)
            {
                string bank_code = dsDiv.DATA[0].bank_name;
                dsDiv.DATA[0].BANK_CODE = bank_code;
                dsDiv.DATA[0].BANK_BRANCH = "";
                dsDiv.DATA[0].branch_name = "";
                dsDiv.DdBranch(bank_code);
            }
            else if (eventArg == PostExpenseBranch)
            {
                string expense_branch = dsMaster.DATA[0].EXPENSE_BRANCH;
                dsMaster.DATA[0].branch_name = expense_branch;
                string bank_code = dsMaster.DATA[0].bank_name;
                dsMaster.DATA[0].branch_name = expense_branch;
                dsMaster.DdBranch(bank_code);
            }
            else if (eventArg == PostExpenseBranchKeep)
            {
                string expense_branch = dsKeep.DATA[0].BANK_BRANCH;
                dsKeep.DATA[0].branch_name = expense_branch;
                string bank_code = dsKeep.DATA[0].bank_name;
                dsKeep.DATA[0].branch_name = expense_branch;
                dsKeep.DdBranch(bank_code);
            }
            else if (eventArg == PostExpenseBranchDiv)
            {
                string expense_branch = dsDiv.DATA[0].BANK_BRANCH;
                dsDiv.DATA[0].branch_name = expense_branch;
                string bank_code = dsDiv.DATA[0].bank_name;
                dsDiv.DATA[0].branch_name = expense_branch;
                dsDiv.DdBranch(bank_code);
            }
            else if (eventArg == PostSetAccDiv) {
                SetAccDiv();
            }
            else if (eventArg == PostDeleteRow)
            {
                int row = dsList.GetRowFocus();
                dsList.DeleteRow(row);
                dsList.DdMoneytrtype();
                dsList.DdMoneytype();
                dsList.DdBank();          
            }
        }
        private void SetAccDiv() {
            if (dsDiv.DATA[0].acc_flag == 1)
            {
                dsDiv.DATA[0].MONEYTYPE_CODE = dsMaster.DATA[0].EXPENSE_CODE;
                dsDiv.DdBank();
                string expense_bank = dsMaster.DATA[0].EXPENSE_BANK;
                dsDiv.DATA[0].bank_name = expense_bank;
                dsDiv.DATA[0].BANK_CODE = expense_bank;
                string bank_code = dsDiv.DATA[0].bank_name;
                dsDiv.DdBranch(bank_code);
                string expense_branch = dsMaster.DATA[0].EXPENSE_BRANCH;
                dsDiv.DATA[0].BANK_BRANCH = expense_branch;
                dsDiv.DATA[0].branch_name = expense_branch;
                dsDiv.DATA[0].BANK_ACCID = dsMaster.DATA[0].EXPENSE_ACCID; 
            }
            else {
                dsDiv.DATA[0].MONEYTYPE_CODE = "";
                dsDiv.DATA[0].BANK_CODE = "";
                dsDiv.DATA[0].bank_name = "";
                dsDiv.DATA[0].BANK_BRANCH = "";
                dsDiv.DATA[0].branch_name = "";
                dsDiv.DATA[0].BANK_ACCID = "";            
            }
        
        }
        public void SaveWebSheet()
        {
            if (dsDiv.DATA[0].acc_flag == 0) {
                if (dsDiv.DATA[0].MONEYTYPE_CODE == "") {
                   
                    this.SetOnLoadedScript("alert('กรุณาเลือกประเภทรายการ ในส่วนบัญชีปันผล!!')");
                    LtServerMessage.Text = WebUtil.ErrorMessage("กรุณาเลือกประเภทรายการ ในส่วนบัญชีปันผล!!");
                    return;
                }

                else if (dsDiv.DATA[0].MONEYTYPE_CODE == "CBT" || dsDiv.DATA[0].MONEYTYPE_CODE == "NXT")
                {
                    if (dsDiv.DATA[0].BANK_CODE == "")
                    {
                        this.SetOnLoadedScript("alert('กรุณาเลือกธนาคาร!!')");
                        LtServerMessage.Text = WebUtil.ErrorMessage("กรุณาเลือกธนาคาร!!");
                        return;
                    }
                    else if (dsDiv.DATA[0].BANK_BRANCH == "")
                    {
                        this.SetOnLoadedScript("alert('กรุณาเลือกสาขา!!')");
                        LtServerMessage.Text = WebUtil.ErrorMessage("กรุณาเลือกสาขา!!");
                        return;
                    }
                    else if (dsDiv.DATA[0].BANK_ACCID == "")
                    {
                        this.SetOnLoadedScript("alert('กรุณาใส่เลขบัญชี!!')");
                        LtServerMessage.Text = WebUtil.ErrorMessage("กรุณาใส่เลขบัญชี!!");
                        return;
                    }

                }
                else if (dsDiv.DATA[0].MONEYTYPE_CODE == "DEP")
                {
                    if (dsDiv.DATA[0].BANK_ACCID == "")
                    {
                        this.SetOnLoadedScript("alert('กรุณาใส่เลขบัญชี!!')");
                        LtServerMessage.Text = WebUtil.ErrorMessage("กรุณาใส่เลขบัญชี!!");
                        return;
                    }
                }
   

            ///////////////////////////////////////////////////////////////////////////////////////
                
            }
                if (dsKeep.DATA[0].MONEYTYPE_CODE == "")
                {
                    this.SetOnLoadedScript("alert('กรุณาเลือกประเภทรายการ ในส่วนบัญชีเรียกเก็บ!!')");
                    LtServerMessage.Text = WebUtil.ErrorMessage("กรุณาเลือกประเภทรายการ ในส่วนบัญชีเรียกเก็บ!!");
                    return;
                }

                else if (dsKeep.DATA[0].MONEYTYPE_CODE == "CBT")
                {
                    if (dsKeep.DATA[0].BANK_CODE == "")
                    {
                        this.SetOnLoadedScript("alert('กรุณาเลือกธนาคาร!!')");
                        LtServerMessage.Text = WebUtil.ErrorMessage("กรุณาเลือกธนาคาร!!");
                        return;
                    }
                    else if (dsKeep.DATA[0].BANK_BRANCH == "")
                    {
                        this.SetOnLoadedScript("alert('กรุณาเลือกสาขา!!')");
                        LtServerMessage.Text = WebUtil.ErrorMessage("กรุณาเลือกสาขา!!");
                        return;
                    }
                    else if (dsKeep.DATA[0].BANK_ACCID == "")
                    {
                        this.SetOnLoadedScript("alert('กรุณาใส่เลขบัญชี!!')");
                        LtServerMessage.Text = WebUtil.ErrorMessage("กรุณาใส่เลขบัญชี!!");
                        return;
                    }

                }
                else if (dsKeep.DATA[0].MONEYTYPE_CODE == "TRN")
                {
                    if (dsKeep.DATA[0].BANK_ACCID == "")
                    {
                        this.SetOnLoadedScript("alert('กรุณาใส่เลขบัญชี!!')");
                        LtServerMessage.Text = WebUtil.ErrorMessage("กรุณาใส่เลขบัญชี!!");
                        return;
                    }
                }
            ///////////////////////////////////////////////////////////////////////////////////////
                if (dsMaster.DATA[0].EXPENSE_CODE == "")
                {
                    this.SetOnLoadedScript("alert('กรุณาเลือกประเภทรายการ ในส่วนบัญชีหลัก!!')");
                    LtServerMessage.Text = WebUtil.ErrorMessage("กรุณาเลือกประเภทรายการ ในส่วนบัญชีหลัก!!");
                    return;
                }

                else if (dsMaster.DATA[0].EXPENSE_CODE == "CBT")
                {
                    if (dsMaster.DATA[0].EXPENSE_BANK == "")
                    {
                        this.SetOnLoadedScript("alert('กรุณาเลือกธนาคาร!!')");
                        LtServerMessage.Text = WebUtil.ErrorMessage("กรุณาเลือกธนาคาร!!");
                        return;
                    }
                    else if (dsMaster.DATA[0].EXPENSE_BRANCH == "")
                    {
                        this.SetOnLoadedScript("alert('กรุณาเลือกสาขา!!')");
                        LtServerMessage.Text = WebUtil.ErrorMessage("กรุณาเลือกสาขา!!");
                        return;
                    }
                    else if (dsMaster.DATA[0].EXPENSE_ACCID == "")
                    {
                        this.SetOnLoadedScript("alert('กรุณาใส่เลขบัญชี!!')");
                        LtServerMessage.Text = WebUtil.ErrorMessage("กรุณาใส่เลขบัญชี!!");
                        return;
                    }

                }
                else if (dsMaster.DATA[0].EXPENSE_CODE == "TRN")
                {
                    if (dsMaster.DATA[0].EXPENSE_ACCID == "")
                    {
                        this.SetOnLoadedScript("alert('กรุณาใส่เลขบัญชี!!')");
                        LtServerMessage.Text = WebUtil.ErrorMessage("กรุณาใส่เลขบัญชี!!");
                        return;
                    }
                }          

        
            try
            {
                string MbExpenseCode = "", MbExpenseBank = "", MbExpenseBranch = "", MbExpenseAccid = "";
                string KpExpenseCode = "", KpExpenseBank = "", KpExpenseBranch = "", KpExpenseAccid = "";
                string DivExpenseCode = "", DivExpenseBank = "", DivExpenseBranch = "", DivExpenseAccid = "";
                string EtcExpenseCode = "", EtcExpenseBank = "", EtcExpenseBranch = "", EtcExpenseAccid = "";
                Decimal li_seqno = 0;

                string ls_memno = dsMain.DATA[0].member_no;
                //init ข้อมูลก่อนแก้ไข
                string sqlseq_old = @"select trim(expense_code) as expense_code, trim(expense_bank) as expense_bank , trim(expense_branch) as expense_branch, trim(expense_accid) as expense_accid from MBMEMBMASTER where member_no = {0} and coop_id = {1} ";
                sqlseq_old = WebUtil.SQLFormat(sqlseq_old, ls_memno, state.SsCoopControl);
                Sdt seqold = WebUtil.QuerySdt(sqlseq_old) ;

                if (seqold.Next())
                {
                    MbExpenseCode = seqold.GetString("expense_code"); 
                    MbExpenseBank = seqold.GetString("expense_bank");
                    MbExpenseBranch = seqold.GetString("expense_branch");
                    MbExpenseAccid = seqold.GetString("expense_accid");
                }
                //init ข้อมูลก่อนแก้ไข
                sqlseq_old = @"select trim(moneytype_code) as moneytype_code, trim( bank_code ) as bank_code, trim( bank_branch ) as bank_branch, trim( bank_accid ) as bank_accid from mbmembmoneytr  where member_no = {0} and coop_id = {1} and trtype_code ='KEEP1'";
                sqlseq_old = WebUtil.SQLFormat(sqlseq_old, ls_memno, state.SsCoopControl);
                seqold = WebUtil.QuerySdt(sqlseq_old);
                
                if (seqold.Next()){
                    KpExpenseCode = seqold.GetString("moneytype_code");
                    KpExpenseBank = seqold.GetString("bank_code");
                    KpExpenseBranch = seqold.GetString("bank_branch");
                    KpExpenseAccid = seqold.GetString("bank_accid");
                 }

                //init ข้อมูลก่อนแก้ไข
                sqlseq_old = @"select trim(moneytype_code) as moneytype_code, trim( bank_code ) as bank_code, trim( bank_branch ) as bank_branch, trim( bank_accid ) as bank_accid from mbmembmoneytr  where member_no = {0} and coop_id = {1} and trtype_code ='DVAV1'";
                sqlseq_old = WebUtil.SQLFormat(sqlseq_old, ls_memno, state.SsCoopControl);
                seqold = WebUtil.QuerySdt(sqlseq_old);

                if (seqold.Next())
                {
                    DivExpenseCode = seqold.GetString("moneytype_code");
                    DivExpenseBank = seqold.GetString("bank_code");
                    DivExpenseBranch = seqold.GetString("bank_branch");
                    DivExpenseAccid = seqold.GetString("bank_accid");
                }
   

                string sqlStr = @"UPDATE MBMEMBMASTER SET EXPENSE_CODE={1}, EXPENSE_BANK={2},EXPENSE_BRANCH={3},EXPENSE_ACCID={4}
                                WHERE MEMBER_NO={0}";
                sqlStr = WebUtil.SQLFormat(sqlStr, ls_memno,dsMaster.DATA[0].EXPENSE_CODE,dsMaster.DATA[0].EXPENSE_BANK,dsMaster.DATA[0].EXPENSE_BRANCH,dsMaster.DATA[0].EXPENSE_ACCID);
                WebUtil.ExeSQL(sqlStr);

                //บันทึกประวัติการเปลี่ยนแปลงบัญชีหลัก
                if( state.SsCoopId == "032001") {
                    //ทดสอบเก็บประวัติการเปลี่ยนแปลงข้อมูลสำหรับกรมวิชาการเกษตรก่อน Edit By Mike  14/01/2019
                    //ชุด create ตารางเก็บประวัติการเปลี่ยนแปลงข้อมูล
                    //CREATE TABLE "MBMEMBMONEYTRHISTORY" ("COOP_ID" CHAR(6) NOT NULL, "MEMBER_NO" CHAR(8) NOT NULL, "SEQ_NO" NUMBER(5,0) NOT NULL, "ENTRY_DATE" DATE, "OLDTRTYPE_CODE" CHAR(5), "OLDMONEYTYPE_CODE" CHAR(3), "OLDBANK_CODE" CHAR(3), "OLDBANK_BRANCH" CHAR(4), "OLDBANK_ACCID" CHAR(15), "NEWTRTYPE_CODE" CHAR(5), "NEWMONEYTYPE_CODE" CHAR(3), "NEWBANK_CODE" CHAR(3), "NEWBANK_BRANCH" CHAR(4), "NEWBANK_ACCID" CHAR(15), "ENTRY_ID" VARCHAR2(30)) ;
                    //ALTER TABLE "MBMEMBMONEYTRHISTORY" ADD ( CONSTRAINT PK_MBTRHISTORY PRIMARY KEY ( "COOP_ID", "MEMBER_NO", "SEQ_NO" )) ;

                    if (MbExpenseCode != dsMaster.DATA[0].EXPENSE_CODE || MbExpenseBank != dsMaster.DATA[0].EXPENSE_BANK || MbExpenseBranch != dsMaster.DATA[0].EXPENSE_BRANCH || MbExpenseAccid != dsMaster.DATA[0].EXPENSE_ACCID )
                    {
                        string sqlseq = @"select nvl( max(seq_no) , 0 )  as seq_no from MBMEMBMONEYTRHISTORY where member_no = '" + ls_memno + "' and coop_id = '"+ state.SsCoopControl +"' ";
                        Sdt seq = WebUtil.QuerySdt(sqlseq);

                        if (seq.Next())
                        {
                            li_seqno =  seq.GetDecimal("seq_no") + 1;
                        }

                        String sqlHis1 = @"  INSERT INTO MBMEMBMONEYTRHISTORY  
                         ( COOP_ID,                          MEMBER_NO,                          SEQ_NO,   
                           ENTRY_DATE,                       OLDTRTYPE_CODE,                     OLDMONEYTYPE_CODE,   
                           OLDBANK_CODE,                     OLDBANK_BRANCH,                     OLDBANK_ACCID,   
                           NEWTRTYPE_CODE,                   NEWMONEYTYPE_CODE,                  NEWBANK_CODE,   
                           NEWBANK_BRANCH,                   NEWBANK_ACCID,                      ENTRY_ID )    
                        values
                         ( {0},                             {1},                                 {2},
                           {3},                             'MAIN1',                              {4},
                           {5},                             {6},                                 {7},
                           'MAIN1',                          {8},                                 {9},
                           {10},                            {11},                                {12} )";
                        sqlHis1 = WebUtil.SQLFormat(sqlHis1, state.SsCoopControl, ls_memno, li_seqno , state.SsWorkDate, MbExpenseCode, MbExpenseBank, MbExpenseBranch, MbExpenseAccid, dsMaster.DATA[0].EXPENSE_CODE
                            , dsMaster.DATA[0].EXPENSE_BANK, dsMaster.DATA[0].EXPENSE_BRANCH, dsMaster.DATA[0].EXPENSE_ACCID, state.SsUsername);
                        WebUtil.ExeSQL(sqlHis1);

                    }
                }

                //
                
                sqlStr = @"DELETE FROM MBMEMBMONEYTR WHERE MEMBER_NO={0}";
                sqlStr = WebUtil.SQLFormat(sqlStr, ls_memno);
                WebUtil.ExeSQL(sqlStr);

                sqlStr = @"INSERT INTO MBMEMBMONEYTR
                    (COOP_ID 			            , MEMBER_NO 		            , TRTYPE_CODE      , MONEYTYPE_CODE                   
                    , OVERRIDE_FLAG                 , BANK_CODE                     , BANK_BRANCH 	   , BANK_ACCID   )
                    values
                    ({0}                            , {1}                           , 'KEEP1'               , {2}                 
                    , {3}                           , {4}                           , {5}               , {6}  )";
                sqlStr = WebUtil.SQLFormat(sqlStr
                    ,  state.SsCoopControl, ls_memno, dsKeep.DATA[0].MONEYTYPE_CODE
                    , '0', dsKeep.DATA[0].BANK_CODE, dsKeep.DATA[0].BANK_BRANCH, dsKeep.DATA[0].BANK_ACCID);
                WebUtil.ExeSQL(sqlStr);

                //บันทึกประวัติการเปลี่ยนแปลงบัญชีเรียกเก็บ
                if (state.SsCoopId == "032001")
                {
                    if (KpExpenseCode != dsKeep.DATA[0].MONEYTYPE_CODE || KpExpenseBank != dsKeep.DATA[0].BANK_CODE || KpExpenseBranch != dsKeep.DATA[0].BANK_BRANCH || KpExpenseAccid != dsKeep.DATA[0].BANK_ACCID)
                    {
                        string sqlseq = @"select nvl( max(seq_no) , 0 ) as seq_no from MBMEMBMONEYTRHISTORY where member_no = '" + ls_memno + "' and coop_id = '" + state.SsCoopControl + "' ";
                        Sdt seq = WebUtil.QuerySdt(sqlseq);
                        if (seq.Next())
                        {
                            li_seqno = seq.GetDecimal("seq_no") + 1;
                        }

                        String sqlHis2 = @"  INSERT INTO MBMEMBMONEYTRHISTORY  
                     ( COOP_ID,                          MEMBER_NO,                          SEQ_NO,   
                       ENTRY_DATE,                       OLDTRTYPE_CODE,                     OLDMONEYTYPE_CODE,   
                       OLDBANK_CODE,                     OLDBANK_BRANCH,                     OLDBANK_ACCID,   
                       NEWTRTYPE_CODE,                   NEWMONEYTYPE_CODE,                  NEWBANK_CODE,   
                       NEWBANK_BRANCH,                   NEWBANK_ACCID,                      ENTRY_ID )    
                    values
                     ( {0},                             {1},                                 {2},
                       {3},                             'KEEP1',                              {4},
                       {5},                             {6},                                 {7},
                       'KEEP1',                          {8},                                 {9},
                       {10},                            {11},                                {12} )";
                        sqlHis2 = WebUtil.SQLFormat(sqlHis2, state.SsCoopControl, ls_memno, li_seqno, state.SsWorkDate, KpExpenseCode, KpExpenseBank, KpExpenseBranch, KpExpenseAccid, dsKeep.DATA[0].MONEYTYPE_CODE
                            , dsKeep.DATA[0].BANK_CODE, dsKeep.DATA[0].BANK_BRANCH, dsKeep.DATA[0].BANK_ACCID, state.SsUsername);
                        WebUtil.ExeSQL(sqlHis2);

                    }
                }

                //


                if (dsDiv.DATA[0].acc_flag == 0)
                {
                    sqlStr = @"INSERT INTO MBMEMBMONEYTR
                    (COOP_ID 			            , MEMBER_NO 		            , TRTYPE_CODE      , MONEYTYPE_CODE                   
                    , OVERRIDE_FLAG                 , BANK_CODE                     , BANK_BRANCH 	   , BANK_ACCID   )
                    values
                    ({0}                            , {1}                           , 'DVAV1'               , {2}                 
                    , {3}                           , {4}                           , {5}               , {6}  )";
                    sqlStr = WebUtil.SQLFormat(sqlStr
                        , state.SsCoopControl, ls_memno, dsDiv.DATA[0].MONEYTYPE_CODE
                        , '0', dsDiv.DATA[0].BANK_CODE, dsDiv.DATA[0].BANK_BRANCH, dsDiv.DATA[0].BANK_ACCID);
                    WebUtil.ExeSQL(sqlStr);


                   //บันทึกประวัติการเปลี่ยนแปลงบัญชีปันผล
                    if (state.SsCoopId == "032001")
                    {
                        if (DivExpenseCode != dsDiv.DATA[0].MONEYTYPE_CODE || DivExpenseBank != dsDiv.DATA[0].BANK_CODE || DivExpenseBranch != dsDiv.DATA[0].BANK_BRANCH || DivExpenseAccid != dsDiv.DATA[0].BANK_ACCID)
                        {
                            string sqlseq = @"select nvl( max(seq_no) , 0 ) as seq_no from MBMEMBMONEYTRHISTORY where member_no = '" + ls_memno + "' and coop_id = '" + state.SsCoopControl + "' ";
                            Sdt seq = WebUtil.QuerySdt(sqlseq);
                            if (seq.Next())
                            {
                                li_seqno = seq.GetDecimal("seq_no") + 1;
                            }

                            String sqlHis3 = @"  INSERT INTO MBMEMBMONEYTRHISTORY  
                     ( COOP_ID,                          MEMBER_NO,                          SEQ_NO,   
                       ENTRY_DATE,                       OLDTRTYPE_CODE,                     OLDMONEYTYPE_CODE,   
                       OLDBANK_CODE,                     OLDBANK_BRANCH,                     OLDBANK_ACCID,   
                       NEWTRTYPE_CODE,                   NEWMONEYTYPE_CODE,                  NEWBANK_CODE,   
                       NEWBANK_BRANCH,                   NEWBANK_ACCID,                      ENTRY_ID )    
                    values
                     ( {0},                             {1},                                 {2},
                       {3},                             'DVAV1',                              {4},
                       {5},                             {6},                                 {7},
                       'DVAV1',                          {8},                                 {9},
                       {10},                            {11},                                {12} )";
                            sqlHis3 = WebUtil.SQLFormat(sqlHis3, state.SsCoopControl, ls_memno, li_seqno, state.SsWorkDate, DivExpenseCode, DivExpenseBank, DivExpenseBranch, DivExpenseAccid, dsDiv.DATA[0].MONEYTYPE_CODE
                                , dsDiv.DATA[0].BANK_CODE, dsDiv.DATA[0].BANK_BRANCH, dsDiv.DATA[0].BANK_ACCID, state.SsUsername);
                            WebUtil.ExeSQL(sqlHis3);

                        }
                    }
                }

                for (int i = 0; i < dsList.RowCount; i++)
                {
                    sqlStr = @"INSERT INTO MBMEMBMONEYTR
                    (COOP_ID 			            , MEMBER_NO 		            , TRTYPE_CODE      , MONEYTYPE_CODE                   
                    , OVERRIDE_FLAG                 , BANK_CODE                     , BANK_BRANCH 	   , BANK_ACCID   )
                    values
                    ({0}                            , {1}                           , {2}               , {3}                 
                    , {4}                           , {5}                           , {6}               , {7}  )";
                    sqlStr = WebUtil.SQLFormat(sqlStr
                        , state.SsCoopControl, ls_memno, dsList.DATA[i].TRTYPE_CODE.Trim(), dsList.DATA[i].MONEYTYPE_CODE
                        , '0', dsList.DATA[i].BANK_CODE, dsList.DATA[i].BANK_BRANCH, dsList.DATA[i].BANK_ACCID);
                    WebUtil.ExeSQL(sqlStr);

                    //init ข้อมูลก่อนแก้ไข
                    sqlseq_old = @"select trim(moneytype_code) as moneytype_code, trim( bank_code ) as bank_code, trim( bank_branch ) as bank_branch, trim( bank_accid ) as bank_accid from mbmembmoneytr  where member_no = {0} and coop_id = {1} and trtype_code ='" + dsList.DATA[i].TRTYPE_CODE.Trim() + "'";
                    sqlseq_old = WebUtil.SQLFormat(sqlseq_old, ls_memno, state.SsCoopControl);
                    seqold = WebUtil.QuerySdt(sqlseq_old);

                    if (seqold.Next())
                    {
                        EtcExpenseCode = seqold.GetString("moneytype_code");
                        EtcExpenseBank = seqold.GetString("bank_code");
                        EtcExpenseBranch = seqold.GetString("bank_branch");
                        EtcExpenseAccid = seqold.GetString("bank_accid");
                    }

                    //บันทึกประวัติการเปลี่ยนแปลงบัญชีอื่นๆ
                    if (state.SsCoopId == "032001")
                    {
                        if (EtcExpenseCode != dsList.DATA[0].MONEYTYPE_CODE || EtcExpenseBank != dsList.DATA[0].BANK_CODE || EtcExpenseBranch != dsList.DATA[0].BANK_BRANCH || EtcExpenseAccid != dsList.DATA[0].BANK_ACCID)
                        {
                            string sqlseq = @"select nvl( max(seq_no) , 0 ) as seq_no from MBMEMBMONEYTRHISTORY where member_no = '" + ls_memno + "' and coop_id = '" + state.SsCoopControl + "' ";
                            Sdt seq = WebUtil.QuerySdt(sqlseq);
                            if (seq.Next())
                            {
                                li_seqno = seq.GetDecimal("seq_no") + 1;
                            }

                            String sqlHis4 = @"  INSERT INTO MBMEMBMONEYTRHISTORY  
                     ( COOP_ID,                          MEMBER_NO,                          SEQ_NO,   
                       ENTRY_DATE,                       OLDTRTYPE_CODE,                     OLDMONEYTYPE_CODE,   
                       OLDBANK_CODE,                     OLDBANK_BRANCH,                     OLDBANK_ACCID,   
                       NEWTRTYPE_CODE,                   NEWMONEYTYPE_CODE,                  NEWBANK_CODE,   
                       NEWBANK_BRANCH,                   NEWBANK_ACCID,                      ENTRY_ID )    
                    values
                     ( {0},                             {1},                                 {2},
                       {3},                             'DVAV1',                              {4},
                       {5},                             {6},                                 {7},
                       'DVAV1',                          {8},                                 {9},
                       {10},                            {11},                                {12} )";
                            sqlHis4 = WebUtil.SQLFormat(sqlHis4, state.SsCoopControl, ls_memno, li_seqno, state.SsWorkDate, EtcExpenseCode, EtcExpenseBank, EtcExpenseBranch, EtcExpenseAccid, dsList.DATA[0].MONEYTYPE_CODE
                                , dsList.DATA[0].BANK_CODE, dsList.DATA[0].BANK_BRANCH, dsList.DATA[0].BANK_ACCID, state.SsUsername);
                            WebUtil.ExeSQL(sqlHis4);

                        }

                    }
                }
                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
            }
        }

        public void WebSheetLoadEnd()
        {
            for(int i= 0; i < dsList.RowCount; i++) {
                if (dsList.DATA[i].MONEYTYPE_CODE == "TRN" || dsList.DATA[i].MONEYTYPE_CODE == "TBK" || dsList.DATA[i].MONEYTYPE_CODE == "CSH")
                {
                    //dsList.DATA[i].BANK_CODE= "00";
                    //dsList.DATA[i].BANK_BRANCH = "";
                    //dsList.DATA[i].branch_name = "";
                    dsList.FindDropDownList(i, "bank_code").Enabled = false;
                    dsList.FindTextBox(i, "bank_branch").Enabled = false;
                    dsList.FindTextBox(i, "branch_name").Enabled = false;
                    dsList.FindDropDownList(i, "bank_code").BackColor = Color.Gray;
                    dsList.FindTextBox(i, "bank_branch").BackColor = Color.Gray;
                    dsList.FindTextBox(i, "branch_name").BackColor = Color.Gray;
                    if (dsList.DATA[i].MONEYTYPE_CODE == "CSH")
                    {
                        //dsList.DATA[i].BANK_ACCID = "";
                        dsList.FindTextBox(i, "bank_accid").Enabled = false;
                        dsList.FindTextBox(i, "bank_accid").BackColor = Color.Gray; 
                    }
                }else {
                    dsList.FindDropDownList(i, "bank_code").Enabled = true;
                    dsList.FindTextBox(i, "bank_branch").Enabled = true;
                    dsList.FindTextBox(i, "branch_name").Enabled = true;
                    dsList.FindDropDownList(i, "bank_code").BackColor = Color.White;
                    dsList.FindTextBox(i, "bank_branch").BackColor = Color.White;
                    dsList.FindTextBox(i, "branch_name").BackColor = Color.White;
                    dsList.FindTextBox(i, "bank_accid").Enabled = true;
                    dsList.FindTextBox(i, "bank_accid").BackColor = Color.White;
                
                }
            } 
        }

    }
}