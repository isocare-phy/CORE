using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary.WcfNShrlon;
using CoreSavingLibrary.WcfNCommon;
using DataLibrary;
//using CoreSavingLibrary.WcfReport;
using System.Globalization;
using System.Data;

namespace Saving.Applications.shrlon.ws_sl_slip_pay_ireport_ctrl
{
    public partial class ws_sl_slip_pay_ireport : PageWebSheet, WebSheet
    {
        [JsPostBack]
        public string PostMemberNo { get; set; }
        [JsPostBack]
        public string PostOperateFlag { get; set; }
        [JsPostBack]
        public string PostOperateFlagL { get; set; }
        [JsPostBack]
        public string PostOperateFlagE { get; set; }
        [JsPostBack]
        public string PostAccidFlag { get; set; }
        [JsPostBack]
        public string PostSlipItem { get; set; }
        [JsPostBack]
        public string PostInsertRow { get; set; }
        [JsPostBack]
        public string PostOperateDate { get; set; }
        [JsPostBack]
        public string PostMoneytype { get; set; }
        [JsPostBack]
        public string PostSearchRetrieve { get; set; }
        [JsPostBack]
        public string PostPrint { get; set; }
        [JsPostBack]
        public string PostDeleteRow { get; set; }

        string alerts = "";
        DateTime idtm_lastDate;
        DateTime idtm_activedate = new DateTime();
        CultureInfo th = System.Globalization.CultureInfo.GetCultureInfo("th-TH");

        public void InitJsPostBack()
        {
            dsMain.InitDsMain(this);
            dsDetailShare.InitDsDetailShare(this);
            dsDetailLoan.InitDsDetailLoan(this);
            dsDetailEtc.InitDsDetailEtc(this);
        }

        public void WebSheetLoadBegin()
        {

            if (!IsPostBack)
            {
                dsMain.DATA[0].SLIP_DATE = state.SsWorkDate;
                dsMain.DATA[0].OPERATE_DATE = state.SsWorkDate;
                of_activeworkdate();
                dsMain.DdSliptype();
                dsMain.DdTofromAccBlank();//ทดสอบ Dd ช่องว่าง
                dsMain.DdMoneyType();
                dsDetailEtc.DdLoanType();
                dsMain.DATA[0].SLIPTYPE_CODE = "PX";
                dsMain.DATA[0].ENTRY_ID = state.SsUsername;
                add_row.Visible = false;

            }
            dsMain.DATA[0].SLIPTYPE_CODE = "PX";
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == PostMemberNo)
            {
                add_row.Visible = true;

                this.InitLnRcv();

            }
            else if (eventArg == PostMoneytype)
            {
                string sliptype_code = dsMain.DATA[0].SLIPTYPE_CODE;
                string moneytype_code = dsMain.DATA[0].MONEYTYPE_CODE;
                dsMain.DATA[0].TOFROM_ACCID = "";
                dsMain.DdFromAccId(sliptype_code, moneytype_code);
                SetDefaultTofromaccid();
                if (state.SsCoopId == "008001" && moneytype_code != "TRN")
                {
                    dsMain.DATA[0].REF_SLIPAMT = 0;
                    dsMain.DATA[0].REF_SLIPNO = "";
                    dsMain.DATA[0].REF_SYSTEM = "";
                    dsMain.DATA[0].EXPENSE_ACCID = "";
                }
            }
            else if (eventArg == PostOperateFlag)
            {
                int row = dsDetailShare.GetRowFocus();
                decimal operate_flag = dsDetailShare.DATA[row].OPERATE_FLAG;
                decimal bfshrcont_balamt = dsDetailShare.DATA[row].BFSHRCONT_BALAMT;
                decimal item_payamt = dsDetailShare.DATA[row].ITEM_PAYAMT;
                decimal item_balance = dsDetailShare.DATA[row].ITEM_BALANCE;
                decimal periodcount_flag = dsDetailShare.DATA[row].PERIODCOUNT_FLAG;
                decimal period = dsDetailShare.DATA[row].PERIOD;

                if (operate_flag == 1)
                {
                    dsDetailShare.DATA[row].BFSHRCONT_BALAMT = bfshrcont_balamt;
                    dsDetailShare.DATA[row].ITEM_PAYAMT = item_payamt;
                    dsDetailShare.DATA[row].ITEM_BALANCE = item_balance;

                    calItemPay();
                }
                else if (operate_flag == 0)
                {
                    dsDetailShare.DATA[row].PRINCIPAL_PAYAMT = 0;
                    dsDetailShare.DATA[row].INTEREST_PAYAMT = 0;
                    dsDetailShare.DATA[row].ITEM_PAYAMT = 0;
                    calItemPay();
                }
            }
            else if (eventArg == PostOperateFlagL)
            {
                int rowl = dsDetailLoan.GetRowFocus();
                decimal operate_flag_l = dsDetailLoan.DATA[rowl].OPERATE_FLAG;
                decimal bfshrcont_balamt_l = dsDetailLoan.DATA[rowl].BFSHRCONT_BALAMT;
                decimal principal_payamt = dsDetailLoan.DATA[rowl].PRINCIPAL_PAYAMT;
                decimal interest_payamt = dsDetailLoan.DATA[rowl].INTEREST_PAYAMT;
                decimal item_payamt_l = dsDetailLoan.DATA[rowl].ITEM_PAYAMT;
                decimal item_balance_l = dsDetailLoan.DATA[rowl].ITEM_BALANCE;

                if (operate_flag_l == 1)
                {
                    if (dsDetailLoan.DATA[rowl].BFPXAFTERMTHKEEP_TYPE == 1)
                    {
                        decimal rkeep_principal = dsDetailLoan.DATA[rowl].RKEEP_PRINCIPAL;
                        if (principal_payamt > rkeep_principal)
                        {
                            bfshrcont_balamt_l = rkeep_principal;
                        }
                    }
                    dsDetailLoan.DATA[rowl].PRINCIPAL_PAYAMT = bfshrcont_balamt_l;
                    dsDetailLoan.DATA[rowl].INTEREST_PAYAMT = dsDetailLoan.DATA[rowl].CP_INTERESTPAY;
                    dsDetailLoan.DATA[rowl].ITEM_PAYAMT = dsDetailLoan.DATA[rowl].PRINCIPAL_PAYAMT + dsDetailLoan.DATA[rowl].INTEREST_PAYAMT;
                    dsDetailLoan.DATA[rowl].ITEM_BALANCE = bfshrcont_balamt_l - dsDetailLoan.DATA[rowl].PRINCIPAL_PAYAMT;
                    calItemPay();
                }
                else if (operate_flag_l == 0)
                {

                    dsDetailLoan.DATA[rowl].PRINCIPAL_PAYAMT = 0;
                    dsDetailLoan.DATA[rowl].INTEREST_PAYAMT = 0;
                    dsDetailLoan.DATA[rowl].ITEM_PAYAMT = 0;
                    dsDetailLoan.DATA[rowl].ITEM_BALANCE = bfshrcont_balamt_l - dsDetailLoan.DATA[rowl].PRINCIPAL_PAYAMT;
                    calItemPay();
                }
            }
            else if (eventArg == PostOperateFlagE)
            {
                int rowe = dsDetailEtc.GetRowFocus();
                decimal operate_flag_e = dsDetailEtc.DATA[rowe].OPERATE_FLAG;
                decimal item_payamt_e = dsDetailEtc.DATA[rowe].ITEM_PAYAMT;

                if (operate_flag_e == 1)
                {

                    dsDetailEtc.DATA[rowe].ITEM_PAYAMT = item_payamt_e;

                    calItemPay();
                }
                else if (operate_flag_e == 0)
                {

                    dsDetailEtc.DATA[rowe].SLIPITEMTYPE_CODE = "";
                    dsDetailEtc.DATA[rowe].SLIPITEM_DESC = "";
                    dsDetailEtc.DATA[rowe].ITEM_PAYAMT = 0;
                    calItemPay();
                }
            }
            else if (eventArg == PostSlipItem)
            {
                int row = dsDetailEtc.GetRowFocus();
                string slipitemtype_code = dsDetailEtc.DATA[row].SLIPITEMTYPE_CODE;
                //dsAdd.ItemType(slipitemtype_code);
                string sql = @" 
                 SELECT SLUCFSLIPITEMTYPE.SLIPITEMTYPE_CODE,   SLUCFSLIPITEMTYPE.SLIPITEMTYPE_DESC
                 FROM SLUCFSLIPITEMTYPE  
                 WHERE ( slucfslipitemtype.manual_flag = 1 ) and  SLUCFSLIPITEMTYPE.SLIPITEMTYPE_CODE={0}";
                sql = WebUtil.SQLFormat(sql, slipitemtype_code);
                Sdt dt = WebUtil.QuerySdt(sql);
                if (dt.Next())
                {
                    dsDetailEtc.DATA[row].SLIPITEM_DESC = dt.GetString("SLIPITEMTYPE_DESC");
                }
            }
            else if (eventArg == PostInsertRow)
            {
                dsDetailEtc.InsertLastRow();
                int currow = dsDetailEtc.RowCount - 1;
                try
                {
                    dsDetailEtc.DATA[currow].SEQ_NO = dsDetailEtc.GetMaxValueDecimal("SEQ_NO") + 1;
                }
                catch
                {
                    if (dsDetailEtc.RowCount < 1)
                    {
                        dsDetailEtc.DATA[currow].SEQ_NO = 1;
                    }
                }
                dsDetailEtc.DdLoanType();
            }
            else if (eventArg == PostDeleteRow)
            {
                int row = dsDetailEtc.GetRowFocus();
                dsDetailEtc.DeleteRow(row);
            }
            else if (eventArg == PostOperateDate)
            {
                DateTime operate_date = dsMain.DATA[0].OPERATE_DATE;

                //str.xml_sliphead = dsMain.ExportXml();
                //str.xml_sliplon = dsDetail.ExportXml();
                string xml_sliplon = dsDetailLoan.ExportXml();

                try
                {
                    Int32 xml_re = wcf.NShrlon.of_initslippayin_calint(state.SsWsPass,ref xml_sliplon, "PX", operate_date);
                    // dsMain.ResetRow();
                    dsDetailLoan.ResetRow();
                    // dsMain.ImportData(str.xml_sliphead);
                    dsDetailLoan.ImportData(xml_sliplon);
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                }
            }
            else if (eventArg == PostSearchRetrieve)
            {
                string payinslip_no = HdPayNo.Value;

                HdPayNo.Value = "";

                dsMain.RetrieveMain(payinslip_no);
                dsDetailLoan.RetrieveDetailLoan(payinslip_no);
                dsDetailShare.RetrieveDetailLoan(payinslip_no);
                dsDetailEtc.RetrieveDetailEtc(payinslip_no);

                string moneytype_code = dsMain.DATA[0].MONEYTYPE_CODE;
                string sliptype_code = dsMain.DATA[0].SLIPTYPE_CODE;

                dsMain.DdFromAccId(sliptype_code, moneytype_code);
                dsMain.DdMoneyType();
                SetDefaultTofromaccid();
            }
            else if (eventArg == PostPrint)
            {
                string slip_no = "IN57000006";

                //slip_no = dsMain.DATA[0].PAYINSLIP_NO;
                
                if (slip_no != "")
                {
                    PrintSlipSlpayin(slip_no);
                }
            }
        }

        public void PrintSlipSlpayin(string slip_no)
        {
            try
            {
                iReportArgument args = new iReportArgument();
                
                args.Add("as_payinslip_no", iReportArgumentType.String, slip_no);
                args.Add("as_coop_id", iReportArgumentType.String, state.SsCoopControl);
                iReportBuider report = new iReportBuider(this, "ใบเสร็จรับเงิน");
                report.AddCriteria("sl_slip_payin", "ดาวน์โหลด ใบเสร็จรับเงิน", ReportType.pdf, args);
                report.AutoOpenPDF = true;
                report.Retrieve();


                //iReportArgument args = new iReportArgument();

                //string coop_id = state.SsCoopControl;
                //string payinslip_no = slip_no;

                //args.Add("as_coop_id", iReportArgumentType.String, coop_id);
                //args.Add("as_payinslip_no", iReportArgumentType.String, payinslip_no);
                //iReportBuider report1 = new iReportBuider(this, args);
                //report1.AddCriteria("sl_slip_payin", "ดาวน์โหลด ใบเสร็จรับเงิน", ReportType.pdf, args);

                //report1.Retrieve();
                //report.Retrieve();

                //String sqlQuery = "select CRITERIA_XML from cmreportprocessing where process_id = '" + progressId + "'";
                //Sdt Query;

                //Query = WebUtil.QuerySdt(sqlQuery);
                //if (Query.Next())
                //{
                //    String CRITERIA_XML = Query.GetString("CRITERIA_XML");
                //    URL = XmlReadVar(CRITERIA_XML, "output_url");
                //    LtServerMessage.Text = WebUtil.CompleteMessage("<a href='" + URL + "' target='_black'>" + "ดาวน์โหลด EXCEL" + "</a><script>window.open('" + URL + "');</script>");
                //}
                //if (URL == "" || URL == null) 
                //    LtServerMessage.Text = WebUtil.ErrorMessage("สร้างรายงานไม่สำเร็จ");
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        public void SaveWebSheet()
        {
            try
            {
                str_slippayin strslip = new str_slippayin();
                strslip.coop_id = state.SsCoopId;
                strslip.entry_id = state.SsUsername;
                strslip.xml_sliphead = dsMain.ExportXml();
                strslip.xml_slipshr = dsDetailShare.ExportXml();
                strslip.xml_sliplon = dsDetailLoan.ExportXml();
                strslip.xml_slipetc = dsDetailEtc.ExportXml();

                idtm_lastDate = dsMain.DATA[0].SLIP_DATE;
                // strslip.xml_sliphead = dsMain.ExportXmlPBFormat("d_sl_payinslip");

                wcf.NShrlon.of_saveslip_payin(state.SsWsPass, ref strslip);

                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลสำเร็จ ");

                decimal print = dsMain.DATA[0].PRINT;

                if (print == 1)
                {
                    string payinslip_no = strslip.payinslip_no; ;
                    PrintSlipSlpayin(payinslip_no);
                }
                dsMain.ResetRow();
                dsDetailShare.ResetRow();
                dsDetailLoan.ResetRow();
                dsDetailEtc.ResetRow();

                if (idtm_lastDate != state.SsWorkDate)
                {
                    dsMain.DATA[0].SLIP_DATE = idtm_lastDate;
                    dsMain.DATA[0].OPERATE_DATE = idtm_lastDate;
                }
                else
                {
                    dsMain.DATA[0].SLIP_DATE = state.SsWorkDate;
                    dsMain.DATA[0].OPERATE_DATE = state.SsWorkDate;
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        public string GetSql(string rslip)
        {
            String sql = @"select
                    a.payinslip_no,
                    a.member_no,
                    a.sliptype_code,
                    a.moneytype_code,
                    a.document_no,
                    a.slip_date,
                    a.operate_date,
                    a.sharestk_value,
                    a.intaccum_amt,
                    a.sharestkbf_value,
                    a.slip_amt,
                    a.slip_status,
                    a.entry_id,
                    a.entry_bycoopid,
                    b.slipitemtype_code,
                    b.shrlontype_code,
                    b.loancontract_no,
                    b.slipitem_desc,
                    b.period,
                    b.principal_payamt,
                    b.interest_payamt,
                    b.item_payamt,
                    b.item_balance,
                    b.calint_to,
                    d.prename_desc||c.memb_name||'  '||c.memb_surname as member_name,
                    a.membgroup_code,
                    e.membgroup_desc,
                    c.membtype_code,
				    c.addr_no,
                    c.addr_moo,
				    c.addr_soi,
				    c.addr_village,
                    c.addr_road,
				    h.tambol_desc,
				    i.district_desc,
				    j.province_desc,
				    c.addr_postcode,
                    f.membtype_desc,
                    g.receipt_remark1 as remark_line1,
                    g.receipt_remark2 as remark_line2,
                    ftreadtbaht( a.slip_amt ) AS  money_thaibaht 
                    from slslippayin a, slslippayindet b, mbmembmaster c, mbucfprename d, mbucfmembgroup e, mbucfmembtype f, cmcoopmaster g, mbucftambol h, mbucfdistrict i, mbucfprovince j                    
                    where	a.coop_id = '" + state.SsCoopControl + @"'
                    and		a.payinslip_no in ('" + rslip + @"')
                    and     a.coop_id		    = b.coop_id
                    and		a.payinslip_no	    = b.payinslip_no
                    and		a.memcoop_id	    = c.coop_id
                    and		a.member_no			= c.member_no
                    and		c.prename_code		= d.prename_code
                    and		a.memcoop_id	    = e.coop_id
                    and		a.membgroup_code	= e.membgroup_code
                    and		c.coop_id		    = f.coop_id
                    and		c.membtype_code		= f.membtype_code
                    and		a.coop_id		    = g.coop_id
                    and		c.tambol_code	    = h.tambol_code (+)
                    and		c.amphur_code	    = i.district_code (+)
                    and		c.province_code		= j.province_code (+)";
            return sql;
        }

        public void WebSheetLoadEnd()
        {
            for (int i = 0; i < dsDetailShare.RowCount; i++)
            {
                if (dsDetailShare.DATA[i].OPERATE_FLAG == 1)
                {
                    dsDetailShare.FindTextBox(i, dsDetailShare.DATA.SLIPITEMColumn).ReadOnly = true;
                    dsDetailShare.FindTextBox(i, dsDetailShare.DATA.PERIODColumn).ReadOnly = true;
                    dsDetailShare.FindCheckBox(i, dsDetailShare.DATA.PERIODCOUNT_FLAGColumn).Enabled = true;
                    dsDetailShare.FindTextBox(i, dsDetailShare.DATA.BFSHRCONT_BALAMTColumn).ReadOnly = true;
                    dsDetailShare.FindTextBox(i, dsDetailShare.DATA.ITEM_PAYAMTColumn).ReadOnly = false;
                    dsDetailShare.FindTextBox(i, dsDetailShare.DATA.ITEM_BALANCEColumn).ReadOnly = true;
                }
                else
                {
                    dsDetailShare.FindTextBox(i, dsDetailShare.DATA.SLIPITEMColumn).ReadOnly = true;
                    dsDetailShare.FindTextBox(i, dsDetailShare.DATA.PERIODColumn).ReadOnly = true;
                    dsDetailShare.FindTextBox(i, dsDetailShare.DATA.BFSHRCONT_BALAMTColumn).ReadOnly = true;
                    dsDetailShare.FindTextBox(i, dsDetailShare.DATA.ITEM_PAYAMTColumn).ReadOnly = true;
                    dsDetailShare.FindTextBox(i, dsDetailShare.DATA.ITEM_BALANCEColumn).ReadOnly = true;
                    dsDetailShare.FindCheckBox(i, dsDetailShare.DATA.PERIODCOUNT_FLAGColumn).Enabled = false;
                }
            }
            for (int k = 0; k < dsDetailLoan.RowCount; k++)
            {
                if (dsDetailLoan.DATA[k].OPERATE_FLAG == 1)
                {
                    dsDetailLoan.FindTextBox(k, dsDetailLoan.DATA.LOANCONTRACT_NOColumn).ReadOnly = true;
                    dsDetailLoan.FindCheckBox(k, dsDetailLoan.DATA.PERIODCOUNT_FLAGColumn).Enabled = true;
                    dsDetailLoan.FindTextBox(k, dsDetailLoan.DATA.PERIODColumn).ReadOnly = true;
                    dsDetailLoan.FindTextBox(k, dsDetailLoan.DATA.BFSHRCONT_BALAMTColumn).ReadOnly = true;
                    dsDetailLoan.FindTextBox(k, dsDetailLoan.DATA.BFLASTCALINT_DATEColumn).ReadOnly = true;
                    dsDetailLoan.FindTextBox(k, dsDetailLoan.DATA.CP_INTERESTPAYColumn).ReadOnly = true;
                    dsDetailLoan.FindTextBox(k, dsDetailLoan.DATA.PRINCIPAL_PAYAMTColumn).ReadOnly = false;
                    dsDetailLoan.FindTextBox(k, dsDetailLoan.DATA.INTEREST_PAYAMTColumn).ReadOnly = false;
                    dsDetailLoan.FindTextBox(k, dsDetailLoan.DATA.ITEM_PAYAMTColumn).ReadOnly = false;
                    dsDetailLoan.FindTextBox(k, dsDetailLoan.DATA.ITEM_BALANCEColumn).ReadOnly = true;
                }
                else
                {
                    dsDetailLoan.FindTextBox(k, dsDetailLoan.DATA.LOANCONTRACT_NOColumn).ReadOnly = true;
                    dsDetailLoan.FindCheckBox(k, dsDetailLoan.DATA.PERIODCOUNT_FLAGColumn).Enabled = false;
                    dsDetailLoan.FindTextBox(k, dsDetailLoan.DATA.PERIODColumn).ReadOnly = true;
                    dsDetailLoan.FindTextBox(k, dsDetailLoan.DATA.BFSHRCONT_BALAMTColumn).ReadOnly = true;
                    dsDetailLoan.FindTextBox(k, dsDetailLoan.DATA.BFLASTCALINT_DATEColumn).ReadOnly = true;
                    dsDetailLoan.FindTextBox(k, dsDetailLoan.DATA.CP_INTERESTPAYColumn).ReadOnly = true;
                    dsDetailLoan.FindTextBox(k, dsDetailLoan.DATA.PRINCIPAL_PAYAMTColumn).ReadOnly = true;
                    dsDetailLoan.FindTextBox(k, dsDetailLoan.DATA.INTEREST_PAYAMTColumn).ReadOnly = true;
                    dsDetailLoan.FindTextBox(k, dsDetailLoan.DATA.ITEM_PAYAMTColumn).ReadOnly = true;
                    dsDetailLoan.FindTextBox(k, dsDetailLoan.DATA.ITEM_BALANCEColumn).ReadOnly = true;
                }
            }
            for (int k = 0; k < dsDetailEtc.RowCount; k++)
            {
                if (dsDetailEtc.DATA[k].OPERATE_FLAG == 1)
                {
                    dsDetailEtc.FindDropDownList(k, dsDetailEtc.DATA.SLIPITEMTYPE_CODEColumn).Enabled = true;
                    dsDetailEtc.FindTextBox(k, dsDetailLoan.DATA.SLIPITEM_DESCColumn).ReadOnly = true;
                    dsDetailEtc.FindTextBox(k, dsDetailLoan.DATA.ITEM_PAYAMTColumn).ReadOnly = false;
                    //dsDetailEtc.FindTextBox(k, dsDetailLoan.DATA.PRNCALINT_AMTColumn).ReadOnly = true;
                }
                else
                {
                    dsDetailEtc.FindDropDownList(k, dsDetailEtc.DATA.SLIPITEMTYPE_CODEColumn).Enabled = false;
                    dsDetailEtc.FindTextBox(k, dsDetailLoan.DATA.SLIPITEM_DESCColumn).ReadOnly = true;
                    dsDetailEtc.FindTextBox(k, dsDetailLoan.DATA.ITEM_PAYAMTColumn).ReadOnly = true;
                    //dsDetailEtc.FindTextBox(k, dsDetailLoan.DATA.PRNCALINT_AMTColumn).ReadOnly = true;
                }
            }
            if (dsMain.DATA[0].ACCID_FLAG == 1)
            {
                dsMain.FindDropDownList(0, dsMain.DATA.TOFROM_ACCIDColumn).Enabled = true;
            }
            else
            {
                dsMain.FindDropDownList(0, dsMain.DATA.TOFROM_ACCIDColumn).Enabled = false;
            }

            if (HdSlipStatus.Value == "1")
            {
                //main 
                dsMain.FindTextBox(0, dsMain.DATA.OPERATE_DATEColumn).ReadOnly = true;
                dsMain.FindDropDownList(0, "SLIPTYPE_CODE").Enabled = false;
                dsMain.FindDropDownList(0, "moneytype_code").Enabled = false;
                dsMain.FindCheckBox(0, "accid_flag").Enabled = false;
                dsMain.FindDropDownList(0, "tofrom_accid").Enabled = false;
                dsMain.FindTextBox(0, dsMain.DATA.COMPUTE1Column).ReadOnly = true;
                dsMain.FindTextBox(0, dsMain.DATA.ENTRY_IDColumn).ReadOnly = true;
                dsMain.FindTextBox(0, dsMain.DATA.SLIP_AMTColumn).ReadOnly = true;
                dsMain.FindTextBox(0, dsMain.DATA.EXPENSE_ACCIDColumn).ReadOnly = true;
                dsMain.FindTextBox(0, dsMain.DATA.REF_SLIPAMTColumn).ReadOnly = true;

                //loan
                for (int i = 0; i < dsDetailLoan.RowCount; i++)
                {
                    dsDetailLoan.FindCheckBox(i, "operate_flag").Enabled = false;
                    dsDetailLoan.FindTextBox(i, "loancontract_no").ReadOnly = true;
                    dsDetailLoan.FindCheckBox(i, "periodcount_flag").Enabled = false;
                    dsDetailLoan.FindTextBox(i, "period").ReadOnly = true;
                    dsDetailLoan.FindTextBox(i, "bfshrcont_balamt").ReadOnly = true;
                    dsDetailLoan.FindTextBox(i, "bflastcalint_date").ReadOnly = true;
                    dsDetailLoan.FindTextBox(i, "COMPUTE1").ReadOnly = true;
                    dsDetailLoan.FindTextBox(i, "principal_payamt").ReadOnly = true;
                    dsDetailLoan.FindTextBox(i, "interest_payamt").ReadOnly = true;
                    dsDetailLoan.FindTextBox(i, "item_payamt").ReadOnly = true;
                    dsDetailLoan.FindTextBox(i, "item_balance").ReadOnly = true;
                }
                //share
                for (int i = 0; i < dsDetailShare.RowCount; i++)
                {
                    dsDetailShare.FindCheckBox(i, "operate_flag").Enabled = false;
                    dsDetailShare.FindTextBox(i, "slipitem").ReadOnly = true;
                    dsDetailShare.FindCheckBox(i, "periodcount_flag").Enabled = false;
                    dsDetailShare.FindTextBox(i, "period").ReadOnly = true;
                    dsDetailShare.FindTextBox(i, "bfshrcont_balamt").ReadOnly = true;
                    dsDetailShare.FindTextBox(i, "item_payamt").ReadOnly = true;
                    dsDetailShare.FindTextBox(i, "item_balance").ReadOnly = true;
                }
                // etc
                for (int i = 0; i < dsDetailEtc.RowCount; i++)
                {
                    dsDetailEtc.FindCheckBox(i, "operate_flag").Enabled = false;
                    dsDetailEtc.FindDropDownList(i, "slipitemtype_code").Enabled = false;
                    dsDetailEtc.FindTextBox(i, "slipitem_desc").ReadOnly = true;
                    dsDetailEtc.FindTextBox(i, "item_payamt").ReadOnly = true;
                    dsDetailEtc.FindTextBox(i, "prncalint_amt").ReadOnly = true;
                }
            }

            if (state.SsCoopId != "008001")
            {
                try
                {
                    dsMain.FindButton(0, "b_ref").Style.Add(HtmlTextWriterStyle.Visibility, "hidden");
                    dsMain.FindTextBox(0, "expense_accid").Style.Add(HtmlTextWriterStyle.Visibility, "hidden");
                    dsMain.FindTextBox(0, "ref_slipamt").Style.Add(HtmlTextWriterStyle.Visibility, "hidden");
                    dsMain.IsShow = "hidden";

                    dsMain.FindTextBox(0, "wrtfund").Style.Add(HtmlTextWriterStyle.Visibility, "hidden");
                }
                catch
                {

                }
            }
        }

        private void InitLnRcv()
        {
            try
            {
                //HfisCalInt.Value = "false";

                string member_no = dsMain.DATA[0].MEMBER_NO;
                string mem_no = WebUtil.MemberNoFormat(member_no);

                str_slippayin slip = new str_slippayin();
                slip.member_no = mem_no;
                slip.slip_date = dsMain.DATA[0].SLIP_DATE;
                slip.operate_date = dsMain.DATA[0].OPERATE_DATE;
                slip.sliptype_code = dsMain.DATA[0].SLIPTYPE_CODE;
                slip.memcoop_id = state.SsCoopControl;
                wcf.NShrlon.of_initslippayin(state.SsWsPass, ref slip);
                dsMain.ImportData(slip.xml_sliphead);
                dsMain.DATA[0].ENTRY_ID = state.SsUsername;
                String mType = dsMain.DATA[0].MONEYTYPE_CODE;
                if (mType == "")
                {
                    dsMain.DATA[0].MONEYTYPE_CODE = "CSH";//dsMain.SetItemString(1, "moneytype_code", "CSH");
                }
                dsMain.DATA[0].SLIPTYPE_CODE = dsMain.DATA[0].SLIPTYPE_CODE.Trim();
                string sliptype_code = dsMain.DATA[0].SLIPTYPE_CODE;
                string moneytype_code = dsMain.DATA[0].MONEYTYPE_CODE;

                dsMain.DdSliptype();
                dsMain.DdFromAccId(sliptype_code, moneytype_code);
                dsMain.DdMoneyType();
                dsDetailEtc.DdLoanType();

                try
                {
                    slip.member_no = mem_no;
                    slip.slip_date = dsMain.DATA[0].SLIP_DATE;
                    slip.operate_date = dsMain.DATA[0].OPERATE_DATE;
                    slip.sliptype_code = dsMain.DATA[0].SLIPTYPE_CODE;
                    slip.memcoop_id = state.SsCoopControl;
                    wcf.NShrlon.of_initslippayin(state.SsWsPass, ref slip);

                    dsDetailShare.ImportData(slip.xml_slipshr);

                }
                catch { dsDetailShare.ResetRow(); }
                try
                {

                    slip.member_no = mem_no;
                    slip.slip_date = dsMain.DATA[0].SLIP_DATE;
                    slip.operate_date = dsMain.DATA[0].OPERATE_DATE;
                    slip.sliptype_code = dsMain.DATA[0].SLIPTYPE_CODE;
                    slip.memcoop_id = state.SsCoopControl;
                    wcf.NShrlon.of_initslippayin(state.SsWsPass, ref slip);
                    dsDetailLoan.ImportData(slip.xml_sliplon);


                }//dsDetailLoan.ResetRow();
                catch { dsDetailLoan.ResetRow(); }
                try
                {
                    //String reqDetailEtcXML = shrlonService.Initlist_lnreqapv(state.SsWsPass, state.SsCoopId, state.SsCoopId);
                    slip.member_no = mem_no;
                    slip.slip_date = dsMain.DATA[0].SLIP_DATE;
                    slip.operate_date = dsMain.DATA[0].OPERATE_DATE;
                    slip.sliptype_code = dsMain.DATA[0].SLIPTYPE_CODE;
                    slip.memcoop_id = state.SsCoopControl;
                    wcf.NShrlon.of_initslippayin(state.SsWsPass, ref slip);
                    dsDetailEtc.ImportData(slip.xml_slipetc);
                }
                catch { dsDetailEtc.ResetRow(); }

                //เช็คการเรียกเก็บ


                //init กองทุน
                try
                {
                    string sql = "select wrtfund_balance from mbmembmaster where coop_id={0} and member_no={1}";
                    sql = WebUtil.SQLFormat(sql, state.SsCoopId, mem_no);
                    Sdt dt = WebUtil.QuerySdt(sql);
                    if (dt.Next())
                    {
                        decimal wrtfund = dt.GetDecimal("wrtfund_balance");
                        dsMain.DATA[0].WRTFUND = wrtfund;


                    }
                }
                catch { }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                //ResetAllDw();
            }

            SetDefaultTofromaccid();
        }

        public void calItemPay()
        {
            int row = dsDetailShare.RowCount;
            int rowl = dsDetailLoan.RowCount;
            int rowe = dsDetailEtc.RowCount;

            decimal slip_amt = 0;
            for (int i = 0; i < row; i++)
            {
                decimal item_payamt = dsDetailShare.DATA[i].ITEM_PAYAMT;
                slip_amt += item_payamt;
            }
            for (int k = 0; k < rowl; k++)
            {
                decimal item_payamt = dsDetailLoan.DATA[k].ITEM_PAYAMT;
                slip_amt += item_payamt;

            }
            for (int j = 0; j < rowe; j++)
            {
                decimal item_payamt = dsDetailEtc.DATA[j].ITEM_PAYAMT;
                slip_amt += item_payamt;

            }

            dsMain.DATA[0].SLIP_AMT = slip_amt;

        }

        //#region Print Report
        //private void Report1st()
        //{

        //    String print_id = "LOAN_BOOK026"; //r_ln_booking_money.srd พิมพ์ใบแจ้งจ่ายเงินกู้
        //    app = state.SsApplication;
        //    gid = "LOAN_BOOK";
        //    rid = print_id;

        //    string branch_id = state.SsCoopId;
        //    string lnpostsend_regno = dsMain.DATA[0].NOTICE_DOCNO;
        //    ReportHelper lnv_helper = new ReportHelper();
        //    //lnv_helper.AddArgument(branch_id, ArgumentType.String);
        //    lnv_helper.AddArgument(lnpostsend_regno, ArgumentType.String);

        //    //ชื่อไฟล์ PDF = YYYYMMDDHHMMSS_<GID>_<RID>.PDF
        //    String pdfFileName = DateTime.Now.ToString("yyyyMMddHHmmss", WebUtil.EN);
        //    pdfFileName += "_" + gid + "_" + rid + ".pdf";
        //    pdfFileName = pdfFileName.Trim();
        //    //ส่งให้ ReportService สร้าง PDF ให้ {โดยปกติจะอยู่ใน C:\GCOOP\Saving\PDF\}.
        //    try
        //    {
        //        ReportClient lws_report = wcf.Report;
        //        //String criteriaXML = lnv_helper.PopArgumentsXML();
        //        //this.pdf = lws_report.GetPDFURL(state.SsWsPass) + pdfFileName;
        //        String li_return = lws_report.RunWithID(state.SsWsPass, app, gid, rid, state.SsUsername, criteriaXML, pdfFileName);
        //        if (li_return == "true")
        //        {
        //            HdOpenReport.Value = "True";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LtServerMessage.Text = WebUtil.ErrorMessage(ex);
        //        return;
        //    }
        //}
        ////public void PopupReport()
        ////{
        ////    //เด้ง Popup ออกรายงานเป็น PDF.
        ////    String pop = "Gcoop.OpenPopup('" + pdf + "')";
        ////    ClientScript.RegisterClientScriptBlock(this.GetType(), "DsReport", pop, true);
        ////}
        //#endregion

        /// <summary>
        /// set คู่บัญชี
        /// </summary>
        private void SetDefaultTofromaccid()
        {
            string sql = @"select 
	            account_id
            from 
	            cmucftofromaccid where
	            coop_id={0} and 
	            moneytype_code={1} and
	            sliptype_code={2} and
	            default_flag=1";
            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, dsMain.DATA[0].MONEYTYPE_CODE, dsMain.DATA[0].SLIPTYPE_CODE);
            Sdt dt = WebUtil.QuerySdt(sql);
            if (dt.Next())
            {
                string accid = dt.GetString("account_id");
                dsMain.DATA[0].TOFROM_ACCID = accid;
            }
        }

        /// <summary>
        /// get วันทำการ
        /// </summary>       
        public void of_activeworkdate()
        {
            try
            {
                string sqlStr;
                int li_clsdaystatus = 0;
                DateTime ldtm_workdate;
                Sdt dt;
                sqlStr = @" select workdate, closeday_status
                    from amappstatus 
                    where coop_id = '" + state.SsCoopId + @"'
                    and application = 'shrlon'";
                dt = WebUtil.QuerySdt(sqlStr);
                if (dt.Next())
                {
                    ldtm_workdate = dt.GetDate("workdate");
                    li_clsdaystatus = dt.GetInt32("closeday_status");
                    if (li_clsdaystatus == 1)
                    {
                        int result = wcf.NCommon.of_getnextworkday(state.SsWsPass, state.SsWorkDate, ref idtm_lastDate);
                    }
                    else
                    {
                        idtm_lastDate = state.SsWorkDate;
                    }
                }
                if (state.SsWorkDate != idtm_lastDate)
                {
                    dsMain.DATA[0].OPERATE_DATE = idtm_lastDate;
                    dsMain.DATA[0].SLIP_DATE = idtm_lastDate;
                    LtServerMessage.Text = WebUtil.WarningMessage("ระบบได้ทำการปิดวันไปแล้ว เปลี่ยนวันทำการเป็น " + idtm_lastDate.ToString("dd/MM/yyyy", th));
                    this.SetOnLoadedScript("alert('ระบบได้ทำการปิดวันไปแล้ว เปลี่ยนวันทำการเป็น " + idtm_lastDate.ToString("dd/MM/yyyy", th) + " ')");
                }
            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
        }

        public void CheckReturnWrt()
        {
            try
            {
                string cont_no = "";
                decimal loantype_code = 0;
                string member_no = "";
                string sqlCont = "";
                string sqlWrt = "";

                string coop_id, slip_no, entry_id, from_system, cash_type, payment_desc, bank_code, bank_branch, pay_towhom;
                string itempaytype_code, nonmember_detail, machine_id, tofrom_accid, account_no, chequebook_no;
                string ref_slipno, remark, from_bankcode, from_branchcode, recvpay_id, ref_system;
                DateTime entry_date, operate_date, dateon_chq, receive_date, recvpay_time;
                decimal payment_status, itempay_amt, pay_recv_status, member_flag, bankfee_amt, banksrv_amt, cheque_status;
                decimal receive_status, item_amtnet;

                member_no = dsMain.DATA[0].MEMBER_NO;
                coop_id = state.SsCoopId;
                entry_id = state.SsUsername;
                from_system = "LON";
                cash_type = dsMain.DATA[0].MONEYTYPE_CODE;
                payment_desc = "คืนกองทุนกสส สมาชิก " + member_no;
                bank_code = dsMain.DATA[0].EXPENSE_BANK;
                bank_branch = dsMain.DATA[0].EXPENSE_BRANCH;
                pay_towhom = dsMain.DATA[0].NAMEB;
                nonmember_detail = dsMain.DATA[0].NAMEB;
                itempaytype_code = "RWT";
                machine_id = "";
                tofrom_accid = dsMain.DATA[0].TOFROM_ACCID;
                account_no = "";
                chequebook_no = "";
                ref_slipno = "";
                payment_status = 1;
                //remark = payment_desc;
                from_bankcode = "";
                from_branchcode = "";
                recvpay_id = entry_id;
                ref_system = "LON";
                entry_date = state.SsWorkDate;
                operate_date = dsMain.DATA[0].OPERATE_DATE;
                dateon_chq = new DateTime();
                receive_date = new DateTime();
                recvpay_time = new DateTime();
                pay_recv_status = 0;
                member_flag = 1;
                bankfee_amt = 0;
                banksrv_amt = 0;
                cheque_status = 0;
                receive_status = 8;

                for (int i = 0; i < dsDetailLoan.RowCount; i++)
                {
                    cont_no = dsDetailLoan.DATA[i].LOANCONTRACT_NO;
                    if (dsDetailLoan.DATA[i].OPERATE_FLAG == 1)
                    {
                        sqlCont = "select * from lncontmaster where loancontract_no = {0} and coop_id={1} and principal_balance=0 and contract_status=-1";
                        sqlCont = WebUtil.SQLFormat(sqlCont, cont_no, state.SsCoopId);
                        Sdt dtCont = WebUtil.QuerySdt(sqlCont);
                        if (dtCont.Next())
                        {
                            sqlWrt = @"select * from wrtfundstatement 
where member_no = {0} and ref_contno ={1} and coop_id={2} and wrtitemtype_code='PWT'
and  ref_contno not in (select ref_contno from wrtfundstatement 
where member_no = {0} and ref_contno ={1} and coop_id={2} and wrtitemtype_code='RWT')";
                            sqlWrt = WebUtil.SQLFormat(sqlWrt, member_no, cont_no, state.SsCoopId);
                            Sdt dtWrt = WebUtil.QuerySdt(sqlWrt);
                            if (dtWrt.Next())
                            {
                                remark = cont_no;
                                itempay_amt = dtWrt.GetDecimal("wrtfund_balance");
                                item_amtnet = itempay_amt;
                                slip_no = Get_NumberDOC("FNRECEIVENO");
                                string insFinWrt = @"insert into finslip
                                                    (
	                                                    coop_id,            slip_no,        entry_id,           entry_date,						
	                                                    operate_date,       from_system,    payment_status,     cash_type,				
	                                                    payment_desc,       bank_code,      bank_branch,        itempay_amt,
	                                                    pay_towhom,         dateon_chq,     member_no	,       itempaytype_code,
	                                                    pay_recv_status,    member_flag,    nonmember_detail,   machine_id,
	                                                    bankfee_amt,        banksrv_amt,    tofrom_accid,       account_no,
	                                                    chequebook_no,      ref_slipno,     remark,             from_bankcode,
	                                                    from_branchcode,    cheque_status,  receive_date,       receive_status,
	                                                    item_amtnet,        recvpay_id,     recvpay_time,       ref_system
                                                    )  
                                                    values
                                                    (
                                                        {0}, {1}, {2}, {3},	
                                                        {4}, {5}, {6}, {7},	
                                                        {8}, {9}, {10}, {11},	
                                                        {12}, {13}, {14}, {15},	
                                                        {16}, {17}, {18}, {19},	
                                                        {20}, {21}, {22}, {23},	
                                                        {24}, {25}, {26}, {27},	
                                                        {28}, {29}, {30}, {31},	
                                                        {32}, {33}, {34}, {35}
                                                    )";
                                try
                                {
                                    insFinWrt = WebUtil.SQLFormat(insFinWrt,
                                        coop_id, slip_no, entry_id, entry_date,
                                                            operate_date, from_system, payment_status, cash_type,
                                                            payment_desc, bank_code, bank_branch, itempay_amt,
                                                            pay_towhom, dateon_chq, member_no, itempaytype_code,
                                                            pay_recv_status, member_flag, nonmember_detail, machine_id,
                                                            bankfee_amt, banksrv_amt, tofrom_accid, account_no,
                                                            chequebook_no, ref_slipno, remark, from_bankcode,
                                                            from_branchcode, cheque_status, receive_date, receive_status,
                                                            item_amtnet, recvpay_id, recvpay_time, ref_system);
                                    Sdt dtIns = WebUtil.QuerySdt(insFinWrt);

                                }
                                catch
                                {

                                }
                            }
                        }
                    }
                }
            }
            catch
            {

            }
        }

        public string Get_NumberDOC(string typecode)
        {
            string coop_id = state.SsCoopControl;
            Sta ta = new Sta(state.SsConnectionString);
            string postNumber = "";
            try
            {
                ta.AddInParameter("AVC_COOPID", coop_id, System.Data.OracleClient.OracleType.VarChar);
                ta.AddInParameter("AVC_DOCCODE", typecode, System.Data.OracleClient.OracleType.VarChar);
                ta.AddReturnParameter("return_value", System.Data.OracleClient.OracleType.VarChar);
                ta.ExePlSql("N_PK_DOCCONTROL.OF_GETNEWDOCNO");
                postNumber = ta.OutParameter("return_value").ToString();
                ta.Close();
            }
            catch
            {
                ta.Close();
            }
            return postNumber.ToString();
        }
    }
}