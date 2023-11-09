using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataLibrary;
//เพิ่ม
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using System.Xml;
using System.Globalization;
//using CProcessing;

namespace Saving.Applications.shrlon.ws_sl_follow_edit_ctrl
{
    public partial class ws_sl_follow_edit : PageWebSheet, WebSheet
    {
        [JsPostBack]
        public string PostMemberNo { get; set; }
        [JsPostBack]
        public string PostProcess { get; set; }
        [JsPostBack]
        public string PostDetail { get; set; }
        [JsPostBack]
        public string PostInsertRow { get; set; }
        [JsPostBack]
        public String PostDeleteRow { get; set; }

        protected String runProcess;
        private int chk_error = 0;

        public void InitJsPostBack()
        {
            wd_main.InitMain(this);
            wd_detail.InitDetail(this);
            wd_statement.InitStatement(this);
            runProcess = WebUtil.JsPostBack(this, "runProcess");
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {

            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == PostMemberNo)
            {

                string ls_member_no = WebUtil.MemberNoFormat(wd_main.DATA[0].MEMBER_NO);
                wd_main.RetrieveMain(ls_member_no);
                wd_main.DdLoancontract();

                string ck_mem = wd_main.DATA[0].MEMBER_NO.Trim();
                if (ck_mem == null || ck_mem == "")
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบเลขสมาชิกในระบบ");
                    wd_statement.ResetRow();
                    wd_detail.ResetRow();
                    wd_main.ResetRow();
                }
            }
            else if (eventArg == PostProcess)
            {
                try
                {
                    CalIntprocess();
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
                }
            }
            else if (eventArg == PostDetail)
            {
                string ls_acc_no = wd_main.DATA[0].loancontract_no;
                wd_detail.RetrieveDetail(ls_acc_no);
                wd_statement.RetrieveStatement(ls_acc_no);
                wd_statement.DdSlipitemtype();
            }

            else if (eventArg == PostInsertRow)
            {
                wd_statement.InsertLastRow();
                int ls_row = wd_statement.RowCount - 1;
                try
                {
                    wd_statement.DATA[ls_row].SEQ_NO = wd_statement.GetMaxValueDecimal("SEQ_NO") + 1;
                }
                catch
                {
                    if (wd_statement.RowCount < 1)
                    {
                        wd_statement.DATA[ls_row].SEQ_NO = 1;
                    }
                }
                wd_statement.DdSlipitemtype();

            }
            else if (eventArg == PostDeleteRow)
            {
                int ls_row = wd_statement.GetRowFocus();
                wd_statement.DeleteRow(ls_row);
            }
        }

        public void SaveWebSheet()
        {
            try
            {
                string coopcontrol = state.SsCoopControl;
                string coopid = state.SsCoopId;
                string loancontract_no = wd_main.DATA[0].loancontract_no;
                decimal prn_arrear = 0, int_arrear = 0;

                int ls_row = wd_statement.RowCount;
                for (int i = 0; i < ls_row; i++)
                {
                   
                    wd_statement.DATA[i].COOP_CONTROL = coopcontrol;
                    wd_statement.DATA[i].COOP_ID = coopid;
                    wd_statement.DATA[i].LOANCONTRACT_NO = loancontract_no;
                    wd_statement.DATA[i].SEQ_NO = i + 1;
                    if (wd_statement.DATA[i].SLIPTYPE_CODE == "LAR" || wd_statement.DATA[i].SLIPTYPE_CODE == "RPM")
                    {
                        prn_arrear += wd_statement.DATA[i].PRINCIPAL_PAYMENT;
                        int_arrear += wd_statement.DATA[i].INTEREST_PAYMENT;
                    }
                    else if (wd_statement.DATA[i].SLIPTYPE_CODE == "PMP")
                    {
                        prn_arrear -= wd_statement.DATA[i].PRINCIPAL_PAYMENT;
                        int_arrear -= wd_statement.DATA[i].INTEREST_PAYMENT;
                    }
                    else 
                    {
                        prn_arrear += 0;
                        int_arrear += 0;
                    }
                }
                ExecuteDataSource exe1 = new ExecuteDataSource(this);
                exe1.AddRepeater(wd_statement);
                exe1.Execute();

                wd_detail.DATA[0].LAST_STM_NO = ls_row;
                wd_detail.DATA[0].PRINCIPAL_ARREAR = prn_arrear;
                wd_detail.DATA[0].INTEREST_ARREAR = int_arrear;

                ExecuteDataSource exe2 = new ExecuteDataSource(this);
                exe2.AddFormView(wd_detail, ExecuteType.Update);
                exe2.Execute();

                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
                wd_statement.ResetRow();
                wd_detail.ResetRow();
                wd_main.ResetRow();
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        public void WebSheetLoadEnd()
        {

        }
        //run_process
        public String outputProcess = "";
        public String logsFilePath = "";

        public void CalIntprocess()
        {
            this.connectionString = state.SsConnectionString;
            string coop_id = state.SsCoopId;
            string coop_control = state.SsCoopControl;
            string entry_id = state.SsUsername;
            String log_file = "trans_log_" + coop_id.Replace(",", "-") + "_" + entry_id + ".log";
            try
            {
                String xml = "<?xml version=\"1.0\" encoding=\"UTF-16LE\" standalone=\"no\"?>" +
                            "<coop_id>" + coop_id + "</coop_id>" +
                            "<coop_control>" + coop_control + "</coop_control>" +
                            "<entry_id>" + entry_id + "</entry_id>" +
                            "<log_file>" + log_file + "</log_file>";
                outputProcess = WebUtil.runProcessingCappExtend(state, "runPostProcess", xml, "", "");
                logsFilePath = Environment.GetEnvironmentVariable("windir") + "\\Temp\\" + log_file;
                HdLogFile.Value = logsFilePath;
                Task.Factory.StartNew(() => this.runObjectProcessing(outputProcess, this.xmlconfig, "runPostProcess", xml, "", ""));
                File.WriteAllText(logsFilePath, "<<ประมวลเสร็จกดดูรายละเอียดที่ Log>>");
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
            finally
            {
            }
        }
        private XmlConfigService xml;

        //ส่วนเรียก Processing หลัก

        public void runObjectProcessing(String outputProcess, XmlConfigService xmlConfig, String object_name, String criteria_xml, String criteria_xml_1, String criteria_xml_2)
        {
            this.xml = xmlConfig;
            initProcessing(outputProcess, object_name, criteria_xml, criteria_xml_1, criteria_xml_2);
            if (object_name == "runPostProcess")
            {
                WriteLine("runPostProcess");
                bool status = false;
                HdERR.Value = "";
                int allStep = 3;  //Step ทั้งหมด
                Sta taSrc = new Sta(state.SsConnectionString);
                Sta taSrcQuery = new Sta(state.SsConnectionString);
                taSrc.Transection();
                taSrcQuery.Transection();
                String Mode = Hdmode.Value.ToString(); // เลือก mode ได้ ใช้ if เอา

                status = runPostProcess(allStep, taSrc, taSrcQuery, Mode);

                if (status)
                {
                    taSrc.Close();
                    taSrcQuery.Close();
                    finishProcessMsgLog("ทำรายการสำเร็จ", allStep);
                }
                else
                {
                    taSrc.RollBack();
                    taSrc.Close();
                    taSrcQuery.RollBack();
                    taSrcQuery.Close();
                    finishProcessMsgLog("!!! ผิดพลาด ไม่สำเร็จขั้นตอนที่  " + HdERR.Value.ToString(), allStep);
                    LtServerMessage.Text = WebUtil.ErrorMessage(HdERR.Value.ToString());
                }
            }
            closeProcessing("Main");
        }

        private Boolean runPostProcess(int allStep, Sta taSrc, Sta taSrcQuery, String Mode)
        {
            startProcessMsgLog("กำลังดำเนินการเริ่มประมวลผล", allStep);
            Sdt dt = null;
            try
            {
                try
                {
                    updateProcessMsgLog("STEP " + 1 + " กำลังเตรียมข้อมูล....", 1);
                }
                catch
                {
                    chk_error = 1;
                    HdERR.Value = chk_error.ToString();
                    return false;
                }
                //step 2
                try
                {
                    int die_cnt = 0;
                    string seq_main = @"select distinct lcm.loancontract_no,
                                        lcm.member_no,
                                        lcm.loantype_code,
                                        lcm.loanapprove_date,
                                        lcm.principal_balance,
                                        lcm.period_payment,
                                        lcm.period_payamt,
                                        lcm.lastcalint_date,
                                        lcm.contract_status
                                        from slslipadjust ajm, slslipadjustdet ajd, kpmastreceivedet kpd, lncontmaster lcm
                                        where ajm.adjslip_no = ajd.adjslip_no
                                        and ajm.ref_recvperiod = kpd.recv_period
                                        and ajm.ref_slipno = kpd.kpslip_no
                                        and ajd.loancontract_no = kpd.loancontract_no
                                        and ajd.loancontract_no = lcm.loancontract_no
                                        and ajd.slipitemtype_code = 'LON'
                                        and ajm.adjtype_code = 'MTH'
                                        and kpd.keepitemtype_code like 'L%'
                                        and ajm.slip_status  = 1
                                        and lcm.loancontract_no not in  (select loancontract_no from lfcontmaster )";
                    dt = taSrcQuery.Query(seq_main);
                    int row_count = dt.Rows.Count;
                    while (dt.Next())
                    {
                        die_cnt++;
                        if ((die_cnt % 1) == 0)
                        {
                            updateProcessMsgLog("STEP " + 2 + ": กำลัง ตั้งข้อมูลสัญญาใหม่  " + die_cnt + "/" + row_count + " ราย ", 2);
                            string sqlIns = @"insert into LFCONTMASTER (coop_control, coop_id, loancontract_no, member_no, loantype_code, loanapprove_date, 
                                                                        principal_balance, period_payment, period_payamt, lastcalint_date, contract_status )
                                                                        values ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10})";
                            sqlIns = WebUtil.SQLFormat(sqlIns, state.SsCoopControl, state.SsCoopId, dt.GetString("loancontract_no"), dt.GetString("member_no"), dt.GetString("loantype_code"), dt.GetDate("loanapprove_date"),
                                                               dt.GetDecimal("principal_balance"), dt.GetInt32("period_payment"), dt.GetDecimal("period_payamt"), dt.GetDate("lastcalint_date"), dt.GetInt32("contract_status"));
                            taSrc.Exe(sqlIns);
                        }
                    }
                }
                catch
                {
                    chk_error = 2;
                    HdERR.Value = chk_error.ToString();
                    return false;
                }

                //step 3
                try
                {
                    int die_cnt = 0;
                    string seq_main = @"select distinct lcm.loancontract_no,
                                        lcm.member_no,
                                        lcm.loantype_code,
                                        lcm.loanapprove_date,
                                        lcm.principal_balance,
                                        lcm.period_payment,
                                        lcm.period_payamt,
                                        lcm.lastcalint_date,
                                        lcm.contract_status
                                        from slslipadjust ajm, slslipadjustdet ajd, kpmastreceivedet kpd, lncontmaster lcm
                                        where ajm.adjslip_no = ajd.adjslip_no
                                        and ajm.ref_recvperiod = kpd.recv_period
                                        and ajm.ref_slipno = kpd.kpslip_no
                                        and ajd.loancontract_no = kpd.loancontract_no
                                        and ajd.loancontract_no = lcm.loancontract_no
                                        and ajd.slipitemtype_code = 'LON'
                                        and ajm.adjtype_code = 'MTH'
                                        and kpd.keepitemtype_code like 'L%'
                                        and ajm.slip_status  = 1
                                        and lcm.loancontract_no in  (select loancontract_no from lfcontmaster )";
                    dt = taSrcQuery.Query(seq_main);
                    int row_count = dt.Rows.Count;
                    while (dt.Next())
                    {
                        die_cnt++;
                        if ((die_cnt % 1) == 0)
                        {
                            updateProcessMsgLog("STEP " + 3 + ": กำลัง อัพเดกข้อมูลสัญญา  " + die_cnt + "/" + row_count + " ราย ", 3);
                            string sqlUp = @"update LFCONTMASTER set principal_balance = {0},period_payment = {1},period_payamt = {2},lastcalint_date = {3} where loancontract_no = {4} and coop_control = {5} and coop_id = {6}";
                            sqlUp = WebUtil.SQLFormat(sqlUp, dt.GetDecimal("principal_balance"), dt.GetInt32("period_payment"), dt.GetDecimal("period_payamt"), dt.GetDate("lastcalint_date"), dt.GetString("loancontract_no"), state.SsCoopControl, state.SsCoopId);
                            taSrc.Exe(sqlUp);
                        }
                    }
                }
                catch
                {
                    chk_error = 3;
                    HdERR.Value = chk_error.ToString();
                    return false;
                }
                taSrc.Commit();
                //step 4
                try
                {
                    int die_cnt = 0;
                    string seq_main = @"select loancontract_no from lfcontmaster where contract_status = 1 ";
                    dt = taSrcQuery.Query(seq_main);
                    int row_count = dt.Rows.Count;
                    while (dt.Next())
                    {
                        die_cnt++;
                        if ((die_cnt % 1) == 0)
                        {
                            updateProcessMsgLog("STEP " + 4 + ": กำลัง สร้างข้อมูลหนี้ค้าง  " + die_cnt + "/" + row_count + " สัญญา ", 4);
                            string loancontract_no = dt.GetString("loancontract_no");
                            string sqlSelSeq = "select last_stm_no,principal_arrear,interest_arrear from lfcontmaster where loancontract_no = '" + loancontract_no + "'";
                            Sdt dtSelSeq = taSrcQuery.Query(sqlSelSeq);
                            int seq = 0;
                            decimal principal_arrear = 0, interest_arrear = 0;
                            if (dtSelSeq.Next())
                            {
                                seq = dtSelSeq.GetInt32("last_stm_no");
                                principal_arrear = dtSelSeq.GetDecimal("principal_arrear");
                                interest_arrear = dtSelSeq.GetDecimal("interest_arrear");
                            }

                            string seq_SelLon = @"select kpd.loancontract_no,
                                               rank() over ( partition by kpd.loancontract_no order by kpd.loancontract_no, kpd.recv_period asc ) as seq_no,
                                               kpd.recv_period,
                                               ajm.adjslip_no,
                                               ajd.principal_adjamt,
                                               ajd.interest_adjamt,
                                               ajd.item_adjamt,
                                               kpd.calintfrom_date,
                                               kpd.calintto_date,
                                               kpd.interest_period,
                                               ajd.principal_adjamt,
                                               ajd.interest_adjamt,
                                               ajm.slip_status
                                               from slslipadjust ajm, slslipadjustdet ajd, kpmastreceivedet kpd
                                               where ajm.adjslip_no = ajd.adjslip_no
                                               and ajm.ref_recvperiod = kpd.recv_period
                                               and ajm.ref_slipno = kpd.kpslip_no
                                               and ajd.loancontract_no = kpd.loancontract_no
                                               and ajd.slipitemtype_code = 'LON'
                                               and ajm.adjtype_code = 'MTH'
                                               and kpd.keepitemtype_code like 'L%'
                                               and ajm.slip_status  = 1
                                               and kpd.loancontract_no = {0}
                                               and kpd.recv_period not in (select distinct recv_period from lfcontstatement where loancontract_no = kpd.loancontract_no)
                                               and ajm.adjslip_no not in (select distinct ref_slipno from lfcontstatement where loancontract_no = kpd.loancontract_no)";
                            seq_SelLon = WebUtil.SQLFormat(seq_SelLon, loancontract_no);
                            Sdt dtSelLon = taSrcQuery.Query(seq_SelLon);
                            while (dtSelLon.Next())
                            {
                                seq++;
                                string Ins_Lon = @"insert into LFCONTSTATEMENT (coop_control, coop_id, loancontract_no, seq_no,recv_period,sliptype_code,ref_slipno,
                                                                                principal_payment,interest_payment,item_payment,calint_from,calint_to,interest_period,
                                                                                principal_arrear,interest_arrear,entry_id,entry_date,remark,item_status)
                                                                                values ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18})";
                                Ins_Lon = WebUtil.SQLFormat(Ins_Lon, state.SsCoopControl, state.SsCoopId, dtSelLon.GetString("loancontract_no"), seq, dtSelLon.GetString("recv_period"), "LAR", dtSelLon.GetString("adjslip_no"),
                                                                     dtSelLon.GetDecimal("principal_adjamt"),  dtSelLon.GetDecimal("interest_adjamt"),  dtSelLon.GetDecimal("item_adjamt"),  dtSelLon.GetDate("calintfrom_date"),  dtSelLon.GetDate("calintto_date"), dtSelLon.GetDecimal("interest_period"),
                                                                     dtSelLon.GetDecimal("principal_adjamt"),  dtSelLon.GetDecimal("interest_adjamt"),state.SsUsername,state.SsWorkDate,"",1);
                                taSrcQuery.Exe(Ins_Lon);
                                principal_arrear = principal_arrear + dtSelLon.GetDecimal("principal_adjamt");
                                interest_arrear = interest_arrear + dtSelLon.GetDecimal("interest_adjamt");
                            }

                            string sqlUp = @"update LFCONTMASTER set last_stm_no = {0},principal_arrear = {1},interest_arrear = {2} where loancontract_no = {3} and coop_control = {4} and coop_id = {5}";
                            sqlUp = WebUtil.SQLFormat(sqlUp, seq, principal_arrear, interest_arrear, loancontract_no, state.SsCoopControl, state.SsCoopId);
                            taSrcQuery.Exe(sqlUp);
                        }
                    }
                    taSrcQuery.Commit();
                }
                catch
                {
                    chk_error = 4;
                    HdERR.Value = chk_error.ToString();
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                HdERR.Value = ex.Message;
                return false;
            }
        }

    }
}