using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using DataLibrary;

namespace Saving.Applications.shrlon.ws_lon_prc_preparepay_ctrl
{
    public partial class ws_lon_prc_preparepay : PageWebSheet, WebSheet
    {
        public string outputProcess;

        [JsPostBack]
        public string PostSetAccDate { get; set; }
        [JsPostBack]
        public string PostNext { get; set; }
        [JsPostBack]
        public string PostPrevious { get; set; }
        
        [JsPostBack]
        public string PostCancel { get; set; }
        [JsPostBack]
        public string PostProcess { get; set; }
        [JsPostBack]
        public string PostSetAvgPercent { get; set; }
        public void InitJsPostBack()
        {
            dsMain.InitDsMain(this);
            dsLoan.InitDsLoan(this);
        }
        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                Reset();


            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == PostNext)
            {
                Next();
            }
            else if (eventArg == PostPrevious)
            {
                Panel1.Visible = true;
                Panel3.Visible = true;
                Panel2.Visible = false;
                Panel4.Visible = false;
            }
            else if (eventArg == PostCancel)
            {
                Reset();
            }
            

            else if (eventArg == PostProcess)
            {
                try
                {
                    string xml_option = dsMain.ExportXml();
                    string xml_option_lntype = dsLoan.ExportXml();
                    outputProcess = WebUtil.runProcessing(state, "LNPAYPREPARE", xml_option, xml_option_lntype, "");
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
                }
            }
        }

        private void getYear()
        {
            try
            {
                string sql = @"select rtrim(ltrim(current_year)) as current_year from yrcfconstant";

                sql = WebUtil.SQLFormat(sql);
                Sdt dt = WebUtil.QuerySdt(sql);
                if (dt.Next())
                {
                    dsMain.DATA[0].BIZZ_PERIOD = dt.GetString("current_year");
                }
                //getAccDate(dsMain.DATA[0].BIZZ_PERIOD);
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        private void Reset()
        {
            dsMain.ResetRow();

            dsMain.DATA[0].PREPARE_DATE = state.SsWorkDate;
            dsMain.DATA[0].CALINTTO_DATE = state.SsWorkDate;
            dsMain.DATA[0].PREPARE_TDATE = state.SsWorkDate.ToString("ddMMyyyy");
            dsMain.DATA[0].CALINTTO_TDATE = state.SsWorkDate.ToString("ddMMyyyy");
            dsMain.DATA[0].COOP_ID = state.SsCoopId;
            dsMain.DATA[0].ENTRY_ID = state.SsUsername;
            dsMain.DATA[0].PREPARECLR_FLAG = 1;
            dsMain.DATA[0].PREPARELON_FLAG = 1;
            dsMain.DATA[0].PROC_TYPE = "1";
            dsMain.DATA[0].CALTYPE_CODE = "ALL";
            dsMain.DATA[0].PREPARETYPE_CODE = "DIV";


            dsLoan.ResetRow();
            dsLoan.RetrieveLoan();

            getYear();

            Panel1.Visible = true;
            Panel2.Visible = false;
            Panel3.Visible = true;
            Panel4.Visible = false;

        }
        private void Next()
        {
            string sql = "";
            string avgtype_code = "";

            string preparetype_code = dsMain.DATA[0].PREPARETYPE_CODE; //ระบบที่รับชำระ
            string caltype_code = dsMain.DATA[0].CALTYPE_CODE; //วิธีการคำนวณ

            dsLoan.RetrieveLoan();

            if (caltype_code == "" || caltype_code == null) //เช็คว่าเลือกวิธีการคำนวณไหม
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("กรุณาเลือกวิธีคำนวณ");
            }
            else if (preparetype_code == "" || preparetype_code == null) //เช็คว่าเลือกระบบที่ชำระ
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("กรุณาเลือกระบบที่รับชำระ");
            }


            else
            {
                Panel1.Visible = false;
                Panel3.Visible = false;
                Panel2.Visible = true;
                Panel4.Visible = true;
            }


        }
        public void SaveWebSheet()
        {
        }
        public void WebSheetLoadEnd()
        {
        }
    }
}