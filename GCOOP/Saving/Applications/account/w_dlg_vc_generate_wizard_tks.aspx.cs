using System;
using CoreSavingLibrary;
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
using System.Data.OracleClient;
using Sybase.DataWindow;
using System.Globalization;
using CoreSavingLibrary.WcfNAccount;
using System.Threading;

namespace Saving.Applications.account
{
    public partial class w_dlg_vc_generate_wizard_tks : PageWebSheet, WebSheet
    {
        private CultureInfo th;
        private DwThDate tdw_wizard;
        private n_accountClient accService;
        protected String postNext;
        protected String JspostChangeDw;
        protected String JsSearchSlip;
        protected String jsPostGetAccid;

        protected String app;
        protected String gid;
        protected String rid;
        protected String pdf;
        protected String Vc_no;

        //public String Sys_App;
        //public DateTime Voucher_date;
        //===============================
        private void JspostBack()
        {
            Dw_wizard.DataWindowObject = "d_vc_vcgen_wizard_vcdate";
            Dw_wizard.Reset();
            Dw_wizard.InsertRow(0);

            Dw_wizard.InsertRow(0);
            //Dw_wizard.SetItemString(1, "coop_id", state.SsCoopControl);
            //Dw_wizard.SetItemString(1, "branch_id", state.SsCoopId);
            Dw_wizard.SetItemString(1, "systemgen_code", "ALL");
            Dw_wizard.SetItemDate(1, "voucher_date", state.SsWorkDate);
            Dw_wizard.SetItemString(1, "voucher_tdate", state.SsWorkDate.ToString("ddMMyyyy", new CultureInfo("th-TH")));

            //Dw_wizard.SetItemString(1, "systemgen_code", Sys_App);
            //Dw_wizard.SetItemDate(1, "voucher_date", Voucher_date);
            //Dw_wizard.SetItemString(1, "voucher_tdate", Voucher_date.ToString("ddMMyyyy", new CultureInfo("th-TH")));

            tdw_wizard.Eng2ThaiAllRow();


        }

        private void SearchSlip()
        {

        }

        private void ChangeDw()
        {
            //Sys_App = Dw_wizard.GetItemString(1, "systemgen_code").Trim();
            //Voucher_date = Dw_wizard.GetItemDate(1, "voucher_date");
            B_step.Visible = false;
            B_next.Visible = true;
            String systemgen_code = Dw_wizard.GetItemString(1, "systemgen_code");
            DateTime voucher_date = Dw_wizard.GetItemDateTime(1, "voucher_date");
            switch (Dw_wizard.GetItemString(1, "systemgen_code").Trim())
            {
                case "CSH":
                    Dw_wizard.DataWindowObject = "d_vc_vcgen_wizard_vctype_cash";
                    break;
                case "LON":
                    Dw_wizard.DataWindowObject = "d_vc_vcgen_wizard_vctype_loan";
                    break;
                case "SHR":
                    Dw_wizard.DataWindowObject = "d_vc_vcgen_wizard_vctype_share";
                    break;
                case "DEP":
                    Dw_wizard.DataWindowObject = "d_vc_vcgen_wizard_vctype_dept";
                    break;
                case "FIN":
                    Dw_wizard.DataWindowObject = "d_vc_vcgen_wizard_vctype_fin";
                    break;
                case "LAN":
                    Dw_wizard.DataWindowObject = "d_vc_vcgen_wizard_vctype_loan_iv";
                    break;
                case "AST":
                    Dw_wizard.DataWindowObject = "d_vc_vcgen_wizard_vctype_ast";
                    break;
                case "HRM":
                    Dw_wizard.DataWindowObject = "d_vc_vcgen_wizard_vctype_hrm";
                    break;
                case "KEP":
                    Dw_wizard.DataWindowObject = "d_vc_vcgen_wizard_vctype_deptkep";
                    break;
                case "TRL":
                    Dw_wizard.DataWindowObject = "d_vc_vcgen_wizard_vctype_loan_trn";
                    break;
                case "TRS":
                    Dw_wizard.DataWindowObject = "d_vc_vcgen_wizard_vctype_shr_trn";
                    break;
                case ("ALL"):
                    Dw_wizard.DataWindowObject = "d_vc_vcgen_wizard_finish";
                    break;
                case ("ANC"):
                    Dw_wizard.DataWindowObject = "d_vc_vcgen_wizard_finish";
                    break;
                case ("DIV"):
                    Dw_wizard.DataWindowObject = "d_vc_vcgen_wizard_vctype_div_avg";
                    break;
            }
            Dw_wizard.Reset();
            Dw_wizard.InsertRow(0);
            Dw_wizard.SetItemDateTime(1, "voucher_date", voucher_date);
            Dw_wizard.SetItemString(1, "voucher_tdate", voucher_date.ToString("ddMMyyyy", new CultureInfo("th-TH")));
            Dw_wizard.SetItemString(1, "systemgen_code", systemgen_code);
            //B_back.Enabled = true;
        }

        //===============================

        #region WebSheet Members


        public void InitJsPostBack()
        {
            JspostChangeDw = WebUtil.JsPostBack(this, "JspostChangeDw");
            JsSearchSlip = WebUtil.JsPostBack(this, "JsSearchSlip");
            jsPostGetAccid = WebUtil.JsPostBack(this, "jsPostGetAccid");

            tdw_wizard = new DwThDate(Dw_wizard, this);
            tdw_wizard.Add("voucher_date", "voucher_tdate");
        }

        public void WebSheetLoadBegin()
        {

            try
            {
                accService = wcf.NAccount;
            }
            catch
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ติดต่อ Webservice ไม่ได้");
                return;
            }

            this.ConnectSQLCA();
            th = new CultureInfo("th-TH");

            Dw_wizard.SetTransaction(sqlca);
            if (!IsPostBack)
            {
                B_next.Visible = false;
                B_Print.Visible = false;
                Dw_wizard.InsertRow(0);
                Dw_wizard.SetItemString(1, "systemgen_code", "CSH");
                Dw_wizard.SetItemDate(1, "voucher_date", state.SsWorkDate);
                Dw_wizard.SetItemString(1, "voucher_tdate", state.SsWorkDate.ToString("ddMMyyyy", new CultureInfo("th-TH")));

                tdw_wizard.Eng2ThaiAllRow();

            }
            else
            {
                this.RestoreContextDw(Dw_wizard);
            }

        }

        public void CheckJsPostBack(string eventArg)
        {
            switch (eventArg)
            {
                case "JspostChangeDw":
                    //ChangeDw();
                    break;
                case "JsSearchSlip":
                    SearchSlip();
                    break;
                case "jsPostGetAccid":
                    GetAccid();
                    break;
            }

        }

        public void SaveWebSheet()
        {
        }

        public void WebSheetLoadEnd()
        {
            Dw_wizard.SaveDataCache();
        }

        #endregion

        public void B_next_Click(object sender, EventArgs e)
        {
            try
            {
                B_next.Visible = false;
                DateTime voucher_date;
                int result;
                String as_type;
                //String Vc_no;
                String systemgen_code = Dw_wizard.GetItemString(1, "systemgen_code");
                switch (Dw_wizard.GetItemString(1, "systemgen_code").Trim())
                {
                    case "CSH":
                        voucher_date = Dw_wizard.GetItemDateTime(1, "voucher_date");
                        as_type = Dw_wizard.Describe("Datawindow.Data.Xml");
                        //result = accService.of_vcproc_tks(state.SsWsPass, voucher_date, systemgen_code, state.SsCoopControl, state.SsUsername, as_type, ref Vc_no);
                        result = wcf.NAccount.of_vcproc_tks(state.SsWsPass, voucher_date, systemgen_code, state.SsCoopControl, state.SsUsername, as_type, ref Vc_no);
                        if (result == 1) { Dw_wizard.DataWindowObject = "d_vc_vcgen_wizard_finish"; }
                        break;
                    case "LON":
                        voucher_date = Dw_wizard.GetItemDateTime(1, "voucher_date");
                        as_type = Dw_wizard.Describe("Datawindow.Data.Xml");
                        result = wcf.NAccount.of_vcproc_tks(state.SsWsPass, voucher_date, systemgen_code, state.SsCoopControl, state.SsUsername, as_type, ref Vc_no);
                        if (result == 1) { Dw_wizard.DataWindowObject = "d_vc_vcgen_wizard_finish"; }
                        break;
                    case "SHR":
                        voucher_date = Dw_wizard.GetItemDateTime(1, "voucher_date");
                        as_type = Dw_wizard.Describe("Datawindow.Data.Xml");
                        result = wcf.NAccount.of_vcproc_tks(state.SsWsPass, voucher_date, systemgen_code, state.SsCoopControl, state.SsUsername, as_type, ref Vc_no);
                        if (result == 1) { Dw_wizard.DataWindowObject = "d_vc_vcgen_wizard_finish"; }
                        break;
                    case "DEP":
                        voucher_date = Dw_wizard.GetItemDateTime(1, "voucher_date");
                        as_type = Dw_wizard.Describe("Datawindow.Data.Xml");
                        result = accService.of_vcproc_tks(state.SsWsPass, voucher_date, systemgen_code, state.SsCoopControl, state.SsUsername, as_type, ref Vc_no);
                        //result = wcf.NAccount.of_vcproc_tks(state.SsWsPass, voucher_date, systemgen_code, state.SsCoopControl, state.SsUsername, as_type, ref Vc_no);
                        if (result == 1) { Dw_wizard.DataWindowObject = "d_vc_vcgen_wizard_finish"; }
                        break;
                    case "FIN":
                        voucher_date = Dw_wizard.GetItemDateTime(1, "voucher_date");
                        as_type = Dw_wizard.Describe("Datawindow.Data.Xml");
                        result = accService.of_vcproc_tks(state.SsWsPass, voucher_date, systemgen_code, state.SsCoopControl, state.SsUsername, as_type, ref Vc_no);
                        //result = wcf.NAccount.of_vcproc_tks(state.SsWsPass, voucher_date, systemgen_code, state.SsCoopControl, state.SsUsername, as_type, ref Vc_no);
                        if (result == 1) { Dw_wizard.DataWindowObject = "d_vc_vcgen_wizard_finish"; }
                        break;
                    case "LAN":
                        voucher_date = Dw_wizard.GetItemDateTime(1, "voucher_date");
                        as_type = Dw_wizard.Describe("Datawindow.Data.Xml");
                        result = wcf.NAccount.of_vcproc_tks(state.SsWsPass, voucher_date, systemgen_code, state.SsCoopControl, state.SsUsername, as_type, ref Vc_no);
                        if (result == 1) { Dw_wizard.DataWindowObject = "d_vc_vcgen_wizard_finish"; }
                        break;
                    case "AST":
                        voucher_date = Dw_wizard.GetItemDateTime(1, "voucher_date");
                        as_type = Dw_wizard.Describe("Datawindow.Data.Xml");
                        result = accService.of_vcproc_tks(state.SsWsPass, voucher_date, systemgen_code, state.SsCoopControl, state.SsUsername, as_type, ref Vc_no);
                        //result = wcf.NAccount.of_vcproc_tks(state.SsWsPass, voucher_date, systemgen_code, state.SsCoopControl, state.SsUsername, as_type, ref Vc_no);
                        if (result == 1) { Dw_wizard.DataWindowObject = "d_vc_vcgen_wizard_finish"; }
                        break;
                    case "HRM":
                        voucher_date = Dw_wizard.GetItemDateTime(1, "voucher_date");
                        as_type = Dw_wizard.Describe("Datawindow.Data.Xml");
                        result = accService.of_vcproc_tks(state.SsWsPass, voucher_date, systemgen_code, state.SsCoopControl, state.SsUsername, as_type, ref Vc_no);
                        //result = wcf.NAccount.of_vcproc_tks(state.SsWsPass, voucher_date, systemgen_code, state.SsCoopControl, state.SsUsername, as_type, ref Vc_no);
                        if (result == 1) { Dw_wizard.DataWindowObject = "d_vc_vcgen_wizard_finish"; }
                        break;
                    case "KEP":
                        voucher_date = Dw_wizard.GetItemDateTime(1, "voucher_date");
                        as_type = Dw_wizard.Describe("Datawindow.Data.Xml");
                        result = wcf.NAccount.of_vcproc_tks(state.SsWsPass, voucher_date, systemgen_code, state.SsCoopControl, state.SsUsername, as_type, ref Vc_no);
                        if (result == 1) { Dw_wizard.DataWindowObject = "d_vc_vcgen_wizard_finish"; }
                        break;
                    case "TRL":
                        voucher_date = Dw_wizard.GetItemDateTime(1, "voucher_date");
                        as_type = Dw_wizard.Describe("Datawindow.Data.Xml");
                        result = wcf.NAccount.of_vcproc_tks(state.SsWsPass, voucher_date, systemgen_code, state.SsCoopControl, state.SsUsername, as_type, ref Vc_no);
                        if (result == 1) { Dw_wizard.DataWindowObject = "d_vc_vcgen_wizard_finish"; }
                        break;
                    case "TRS":
                        voucher_date = Dw_wizard.GetItemDateTime(1, "voucher_date");
                        as_type = Dw_wizard.Describe("Datawindow.Data.Xml");
                        result = wcf.NAccount.of_vcproc_tks(state.SsWsPass, voucher_date, systemgen_code, state.SsCoopControl, state.SsUsername, as_type, ref Vc_no);
                        if (result == 1) { Dw_wizard.DataWindowObject = "d_vc_vcgen_wizard_finish"; }
                        break;
                    case ("ALL"):

                        break;
                    case ("ANC"):

                        break;
                    case "DIV":
                        voucher_date = Dw_wizard.GetItemDateTime(1, "voucher_date");
                        as_type = Dw_wizard.Describe("Datawindow.Data.Xml");
                        result = accService.of_vcproc_tks(state.SsWsPass, voucher_date, systemgen_code, state.SsCoopControl, state.SsUsername, as_type, ref Vc_no);
                        //result = wcf.NAccount.of_vcproc_tks(state.SsWsPass, voucher_date, systemgen_code, state.SsCoopControl, state.SsUsername, as_type, ref Vc_no);
                        if (result == 1) { Dw_wizard.DataWindowObject = "d_vc_vcgen_wizard_finish"; }
                        break;

                }
                HdVc_No.Value = Vc_no;
                B_Print.Visible = true;
            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }
        }

        protected void B_next_Step(object sender, EventArgs e)
        {
            ChangeDw();
        }

        protected void B_back_Click(object sender, EventArgs e)
        {
            JspostBack();
        }
        //ออกรายงาน
        protected void B_Print_Click(object sender, EventArgs e)
        {
            Print();

        }


        //  พิมพ์ใบเสร็จหน้าลงรายวัน
        protected void Print()
        {
            try
            {
				String[] voucher_no = HdVc_No.Value.Split(',');
				//voucher_no=voucher_no.Replace(",","','");
				String szFieldList="(";
				for(int i=0;i<voucher_no.Length;i++)
				{
					szFieldList+="VCVOUCHER.VOUCHER_NO='"+voucher_no[i]+"' OR ";
				}
				szFieldList+=")";
				szFieldList=szFieldList.Replace("OR )",")");
				String szSqlstatm=@"SELECT VCVOUCHER.VOUCHER_NO,
									 VCVOUCHER.VOUCHER_DATE,
									 VCVOUCHERDET.ACCOUNT_ID,
									 VCVOUCHER.VOUCHER_DESC,
									 VCVOUCHERDET.DR_AMT,
									 VCVOUCHERDET.CR_AMT,
									 VCVOUCHER.VOUCHER_AMT,
									 ACCMASTER.ACCOUNT_NAME,
									 VCVOUCHERDET.SEQ_NO,
									 VCVOUCHER.ENTRY_ID,
									 VCVOUCHER.VOUCHER_TYPE,
									 VCVOUCHERDET.ITEM_DESC,
									 VCVOUCHER.BRANCH_ID,
									 VCVOUCHERDET.ACCOUNT_SIDE,
									 VCVOUCHER.CASH_TYPE,
									 ACCCNTCOOP.COOP_DESC
								FROM VCVOUCHER,
									 VCVOUCHERDET,
									 ACCMASTER,
									 ACCCNTCOOP
							   WHERE ( VCVOUCHERDET.VOUCHER_NO = VCVOUCHER.VOUCHER_NO ) and
									 ( VCVOUCHERDET.ACCOUNT_ID = ACCMASTER.ACCOUNT_ID ) and
									 ( VCVOUCHER.COOP_ID = VCVOUCHERDET.COOP_ID ) and
									 ( VCVOUCHERDET.COOP_ID = ACCMASTER.COOP_ID ) and
									 ( ACCMASTER.COOP_ID = ACCCNTCOOP.COOP_ID ) and ";
							szSqlstatm+=szFieldList+" AND ";
							szSqlstatm+=@"( VCVOUCHER.VOUCHER_STATUS = 1 ) AND
									 ( VCVOUCHER.COOP_ID = '" + state.SsCoopControl + @"' )
										order by VCVOUCHER.CASH_TYPE,
										VCVOUCHER.VOUCHER_DATE,
										VCVOUCHER.VOUCHER_TYPE,
										VCVOUCHER.VOUCHER_NO,
										VCVOUCHERDET.ACCOUNT_SIDE DESC,
										VCVOUCHERDET.ACCOUNT_ID,
										VCVOUCHERDET.SEQ_NO";
                iReportArgument args = new iReportArgument(szSqlstatm);                
                //args.Add("as_voucher_no", iReportArgumentType.String, voucher_no);
                //args.Add("as_coopid", iReportArgumentType.String, state.SsCoopId);
                iReportBuider report = new iReportBuider(this, "รายงาน Voucher");
                report.AddCriteria("r_day30_voucherdaily_tks_by_vc_print", "PDF รายงาน Voucher", ReportType.pdf, args);
                report.AutoOpenPDF = true;
                report.Retrieve();
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }
        private void GetAccid()
        {
            string acc_list = Hdacclist.Value;
            String cash_type = HdCashType.Value;
            //int row = Convert.ToInt32(Hdrow.Value);
            switch (Dw_wizard.GetItemString(1, "systemgen_code").Trim())
            {
                case "CSH":
                    if (cash_type == "b_lonfilter")
                    {
                        Dw_wizard.SetItemString(1, "gencash_lonfilter", acc_list);
                    }
                    else { Dw_wizard.SetItemString(1, "gencash_shrfilter", acc_list); }
                    break;
                case "LON":
                    Dw_wizard.SetItemString(1, "genloan_rcvfilter", acc_list);
                    break;
                case "FIN":
                    Dw_wizard.SetItemString(1, "genloan_rcvfilter", acc_list);
                    break;
                case "SHR":
                    Dw_wizard.SetItemString(1, "genshr_rcvfilter", acc_list);
                    break;
            }

        }
    }
}
