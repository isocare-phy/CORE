using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary.WcfNCommon;
using CoreSavingLibrary.WcfNShrlon;
using CoreSavingLibrary.WcfNBusscom;
using Sybase.DataWindow;
using System.Web.Services.Protocols;
using DataLibrary;

namespace Saving.Applications.shrlon
{
    public partial class w_sheet_sl_EstimateCredit_by_bank : PageWebSheet, WebSheet
    {
        protected String jsEstimate;
        private n_shrlonClient shrlonService;
        private n_commonClient commonService;
        public void InitJsPostBack()
        {
            jsEstimate = WebUtil.JsPostBack(this, "jsEstimate");

        }

        public void WebSheetLoadBegin()
        {
            str_itemchange strList = new str_itemchange();
            try
            {
                shrlonService = wcf.NShrlon;
                commonService = wcf.NCommon;
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                return;
            }

            this.ConnectSQLCA();
            if (IsPostBack)
            {
                //this.RestoreContextDw(dw_head);
                //this.RestoreContextDw(dw_detail);
                this.RestoreContextDw(dw_estimate);

            }
            else
            {
                dw_estimate.InsertRow(1);
                // dw_detail.InsertRow(0);
                string ls_intratetab = wcf.NBusscom.of_getattribloantype(state.SsWsPass, "30", "inttabrate_code").ToString();
                Decimal intrate = 0;

                String sqlint = @"  SELECT 
                                         INTEREST_RATE        
                                    FROM LNCFLOANINTRATEDET  
                                   WHERE ( COOP_ID ='" + state.SsCoopControl + @"'  ) AND  
                                         ( LOANINTRATE_CODE ='" + ls_intratetab + "'  ) ";
                Sdt dtint = WebUtil.QuerySdt(sqlint);
                if (dtint.Next())
                {
                    intrate = dtint.GetDecimal("INTEREST_RATE");

                }
                dw_estimate.SetItemDecimal(1, "interate_rate", new Decimal(0.055));
                dw_estimate.SetItemDecimal(1, "day_amount", 31);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "jsEstimate")
            {
                JsEstimate();
            }
        }

        private void JsEstimate()
        {

            Decimal period_payamt = dw_estimate.GetItemDecimal(1, "period");
            Decimal loanrequest_amt = dw_estimate.GetItemDecimal(1, "loanrequest_amt");
            decimal loanpayment_type = 2;// dw_main.GetItemDecimal(1, "loanpayment_type");
            decimal ldc_intrate = dw_estimate.GetItemDecimal(1, "interate_rate");
            Decimal period_payment = 0;

            // ปัดยอดชำระ
            String ll_roundpay = wcf.NBusscom.of_getattribloantype(state.SsWsPass, "30", "payround_factor");
            int roundpay = Convert.ToInt16(ll_roundpay);
            double ldc_fr = 1, ldc_temp = 1, loapayment_amt = 0;
            if (loanpayment_type == 1)
            {
                //คงต้น
                period_payment = loanrequest_amt / period_payamt;
            }
            else
            {
                int li_fixcaltype = 1;
                //คงยอด


                if (li_fixcaltype == 1)
                {
                    // ด/บ ทั้งปี / 12
                    //ldc_fr			= exp( - ai_period * log( ( 1 + adc_intrate / 12 ) ) )
                    //ldc_payamt	= adc_principal * ( adc_intrate / 12 ) / ( 1 - ldc_fr )

                    ldc_temp = Math.Log(1 + (Convert.ToDouble(ldc_intrate / 12)));
                    ldc_fr = Math.Exp(-Convert.ToDouble(period_payamt) * ldc_temp);
                    loapayment_amt = (Convert.ToDouble(loanrequest_amt) * (Convert.ToDouble(ldc_intrate) / 12)) / ((1 - ldc_fr));


                }
                else
                {
                    //แบบสุโขทัย
                    //ldc_fr			= ( adc_intrate * ( 1+ adc_intrate) ^ ai_period )/ (((1 + adc_intrate ) ^ ai_period ) - 1  )
                    //ldc_payamt = adc_principal / ldc_fr
                }

                period_payment = Math.Ceiling(Convert.ToDecimal(loapayment_amt));
            }
            if (roundpay > 0)
            {

                period_payamt = Math.Round(period_payamt / 10) * 10;

            }
            dw_estimate.SetItemDecimal(1, "payment_amt", period_payment);


        }

        public void SaveWebSheet()
        {
            throw new NotImplementedException();
        }

        public void WebSheetLoadEnd()
        {
            dw_estimate.SaveDataCache();
            //dw_head.SaveDataCache();
            //dw_detail.SaveDataCache();
        }
        protected void Text_Changed(object sender, EventArgs e)
        {
            //string percen = TextBox1.Text;
            ////Decimal income = Convert.ToDecimal(Request["income"].ToString());
            ////Decimal paymonth = Convert.ToDecimal(Request["paymonth"].ToString());
            //Decimal salary_amt = dw_head.GetItemDecimal(1, "salary_amt");
            //Decimal itemincome_amt = dw_head.GetItemDecimal(1, "itemincome_amt");
            //Decimal itemincome_other = dw_head.GetItemDecimal(1, "itemincome_other");
            //int i = 0;
            //Decimal period_payment = 0, intestimate_amt = 0;
            //Decimal itempayment_amt = 0;
            //for (i = 1; i <= dw_detail.RowCount; i++)
            //{
            //    period_payment = dw_detail.GetItemDecimal(i, "period_payment");
            //    intestimate_amt = dw_detail.GetItemDecimal(i, "intestimate_amt");
            //    itempayment_amt += period_payment + intestimate_amt;

            //}


            ////dw_head.GetItemDecimal(1, "itempayment_amt");
            //Decimal itempayment_oth = dw_head.GetItemDecimal(1, "itempayment_oth");
            //Decimal emer_loop = dw_head.GetItemDecimal(1, "emer_loop");
            //TextBox4.Text = salary_amt.ToString("#,###,#00.00");
            //salary_amt = salary_amt * Convert.ToDecimal(percen) / 100;
            //TextBox2.Text = salary_amt.ToString("#,###,#00.00");
            //Decimal total = salary_amt + itemincome_amt + itemincome_other - itempayment_amt - itempayment_oth + emer_loop;
            //TextBox3.Text = total.ToString("#,###,#00.00");
            //TextBox5.Text = (itempayment_amt - itempayment_oth + emer_loop).ToString("#,###,#00.00");



        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            //string percen = TextBox1.Text;
            ////Decimal income = Convert.ToDecimal(Request["income"].ToString());
            ////Decimal paymonth = Convert.ToDecimal(Request["paymonth"].ToString());
            //Decimal salary_amt = dw_head.GetItemDecimal(1, "salary_amt");
            //Decimal itemincome_amt = dw_head.GetItemDecimal(1, "itemincome_amt");
            //Decimal itemincome_other = dw_head.GetItemDecimal(1, "itemincome_other");
            //int i = 0;
            //Decimal period_payment = 0, intestimate_amt = 0;
            //Decimal itempayment_amt = 0;
            //for (i = 1; i <= dw_detail.RowCount; i++)
            //{
            //    period_payment = dw_detail.GetItemDecimal(i, "period_payment");
            //    intestimate_amt = dw_detail.GetItemDecimal(i, "intestimate_amt");
            //    itempayment_amt += period_payment + intestimate_amt;

            //}

            ////  Decimal itempayment_amt = dw_head.GetItemDecimal(1, "itempayment_amt");
            //Decimal itempayment_oth = dw_head.GetItemDecimal(1, "itempayment_oth");
            //Decimal emer_loop = dw_head.GetItemDecimal(1, "emer_loop");
            //TextBox4.Text = salary_amt.ToString("#,###,#00.00");
            //salary_amt = salary_amt * Convert.ToDecimal(percen) / 100;
            //TextBox2.Text = salary_amt.ToString("#,###,#00.00");
            //Decimal total = salary_amt + itemincome_amt + itemincome_other - itempayment_amt - itempayment_oth + emer_loop;
            //TextBox3.Text = total.ToString("#,###,#00.00");
            //TextBox5.Text = (itempayment_amt - itempayment_oth + emer_loop).ToString("#,###,#00.00");
        }

    }
}