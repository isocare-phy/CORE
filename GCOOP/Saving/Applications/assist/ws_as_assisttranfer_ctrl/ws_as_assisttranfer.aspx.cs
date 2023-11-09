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
using DataLibrary;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Saving.Applications.assist.ws_as_assisttranfer_ctrl
{
    public partial class ws_as_assisttranfer : PageWebSheet, WebSheet
    {
        Sdt ta = new Sdt();
        [JsPostBack]
        public string Postbudget_to { get; set; }


        public void InitJsPostBack()
        {
            dsList.InitDsList(this);
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {

                RetrieveYear();
                dsList.DdBudgetTo(state.SsCoopId, dsList.DATA[0].ASSIST_YEAR - 543);
                dsList.DATA[0].ENTRY_DATE = state.SsWorkDate;
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "Postbudget_to")
            {

                string account_year = Convert.ToString(Convert.ToDecimal(dsList.DATA[0].ASSIST_YEAR) - 543);
                string accbudgetgroup_typ_to = dsList.DATA[0].ASSISTTYPE_TO;
                string accbudgetgroup_typ_from = dsList.DATA[0].ASSISTTYPE_FROM;
                
            }
        }

        public void Postbudget_af()
        {
            decimal transfer_amt = dsList.DATA[0].TRANSFER_AMT;
        }

        public void SaveWebSheet()
        {
            string assisttype_to = dsList.DATA[0].ASSISTTYPE_TO;
            string assisttype_from = dsList.DATA[0].ASSISTTYPE_FROM;
            string REMARK = dsList.DATA[0].REMARK;
            decimal TRANSFER_AMT = dsList.DATA[0].TRANSFER_AMT;
            DateTime ENTRY_DATE = dsList.DATA[0].ENTRY_DATE;
            Int32 seq_no = 0;
            decimal assist_year = Convert.ToDecimal(dsList.DATA[0].ASSIST_YEAR) - 543;

            if (assisttype_to == assisttype_from)
            {
                LtServerMessage.Text = WebUtil.WarningMessage("รหัสประเภทซ้ำกัน กรุณาตรวจสอบ");
                return;
            }
            else if (assisttype_to == null || assisttype_to == "")
            {
                LtServerMessage.Text = WebUtil.WarningMessage("กรุณากรอกข้อสวัสดิการที่รับโอน");
                return;
            }
            else if (assisttype_from == null || assisttype_from == "")
            {
                LtServerMessage.Text = WebUtil.WarningMessage("กรุณากรอกข้อสวัสดิการที่โอนย้าย");
                return;
            }
            string sqlseq_no = "select max(seq_no) as seq_no from ASSASSISTTRANFER where ASSIST_YEAR = '" + assist_year + "'  order by  seq_no";
            Sdt dt = WebUtil.QuerySdt(sqlseq_no);
            if (dt.Next())
            {
                seq_no = dt.GetInt32(0);
            }
            seq_no++;

            try
            {

                String sqlInsert = @"INSERT INTO ASSASSISTTRANFER  " +
                                       "(COOP_ID,						    ASSIST_YEAR,					SEQ_NO," +                       
                                       "ASSISTTYPE_FROM, 					TRANSFER_AMT,                   ASSISTTYPE_TO," +                        						
                                       "ENTRY_DATE,			                ENTRY_ID,						REMARK ) " +                                                           
                                  "VALUES" +
                                    "('" + state.SsCoopId + "',             '" + assist_year + "',		    '" + seq_no + "'," +
                                    "'" + assisttype_from + "',	            '" + TRANSFER_AMT + "',	        '" + assisttype_to + "'," +
                                    " to_date('" + ENTRY_DATE.ToString("dd/MM/yyyy") + @"','dd/MM/yyyy'),	'" + state.SsUsername + "',  '" + REMARK + "') ";                                                                                 //6
                Sdt Insert = WebUtil.QuerySdt(sqlInsert);


                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลสำเร็จ");
                dsList.ResetRow();
            }
            catch
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("บันทึกไม่สำเสร็จ");
            }
        }


   
        public void RetrieveYear()
        {

            string sqlSelect = "select max(ass_year) as assist_year from assucfyear order by  assist_year";
            Sdt dt = WebUtil.QuerySdt(sqlSelect);
            if (dt.Next())
            {
                decimal assist_year = dt.GetDecimal(0);
                assist_year = assist_year + 543;
                dsList.DATA[0].ASSIST_YEAR = assist_year;


            }

        }


        public void WebSheetLoadEnd()
        {

        }
    }
}