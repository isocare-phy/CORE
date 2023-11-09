using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace Saving.Applications.shrlon.ws_sl_slip_pay_mth_ctrl
{
    public partial class DsDetailLoan : DataSourceRepeater
    {
        public DataSet1.SLSLIPPAYINDETDataTable DATA { get; set; }
        public void InitDsDetailLoan(PageWeb pw)
        {
            css1.Visible = false;
            css2.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.SLSLIPPAYINDET;
            this.EventItemChanged = "OnDsDetailLoanItemChanged";
            this.EventClicked = "OnDsDetailLoanClicked";
            this.InitDataSource(pw, Repeater2, this.DATA, "dsDetailLoan");
             this.Button.Add("bloan_detail");
            //this.Button.Add("b_contsearch");
            this.Register();
        }
        public void RetrieveDetailLoan(string payinslip_no)
        {
            String sql = @"  SELECT SLSLIPPAYIN.COOP_ID,   
                             SLSLIPPAYIN.PAYINSLIP_NO,   
                             SLSLIPPAYINDET.OPERATE_FLAG,   
                             SLSLIPPAYINDET.LOANCONTRACT_NO,   
                             SLSLIPPAYINDET.PERIODCOUNT_FLAG,   
                             SLSLIPPAYINDET.PERIOD,   
                             SLSLIPPAYINDET.BFSHRCONT_BALAMT,   
                             SLSLIPPAYINDET.BFLASTCALINT_DATE,   
                             SLSLIPPAYINDET.INTEREST_PERIOD,   
                             SLSLIPPAYINDET.BFINTARR_AMT,   
                             SLSLIPPAYINDET.PRINCIPAL_PAYAMT,   
                             SLSLIPPAYINDET.INTEREST_PAYAMT,   
                             SLSLIPPAYINDET.ITEM_PAYAMT,   
                             SLSLIPPAYINDET.ITEM_BALANCE,   
                             SLSLIPPAYIN.MEMBER_NO,
                             SLSLIPPAYINDET.BFPERIOD_PAYMENT  
                        FROM SLSLIPPAYIN,   
                             SLSLIPPAYINDET  
                       WHERE ( SLSLIPPAYINDET.COOP_ID = SLSLIPPAYIN.COOP_ID ) and  
                             ( SLSLIPPAYINDET.PAYINSLIP_NO = SLSLIPPAYIN.PAYINSLIP_NO ) and  
                             (SLSLIPPAYINDET.SLIPITEMTYPE_CODE = 'LON') AND
                             ( ( SLSLIPPAYIN.COOP_ID = {0} ) AND  
                             ( SLSLIPPAYIN.PAYINSLIP_NO = {1} ) )    
                    ";

            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, payinslip_no);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }

      public void RetrievePrinInt(string member_no, string recv_period, string receipt_no)
        {
            String sql = @"  SELECT KPMASTRECEIVE.COOP_ID,   
                             KPMASTRECEIVE.RECEIPT_NO,   
                             KPMASTRECEIVEDET.KEEPITEM_STATUS,   
                             KPMASTRECEIVEDET.RECV_PERIOD,   
                             SUM(KPMASTRECEIVEDET.ADJUST_PRNAMT) AS ADJUST_PRNAMT,   
                             SUM(KPMASTRECEIVEDET.ADJUST_INTAMT) AS ADJUST_INTAMT,   
                             SUM(KPMASTRECEIVEDET.PRINCIPAL_PAYMENT) AS PRINCIPAL_PAYAMT,   
                             0 AS INTEREST_PAYAMT,   
                             0 AS ITEM_PAYMENT,   
                             0 AS ITEM_BALANCE, 
                             0 AS SUM_INTAMT,
                             0 AS SUM_PRNAMT,
                             0 AS SUMITEM_PAYMENT,
                             0 AS SUMINT_PAYMENT,
                             0 AS SUMPRN_PAYMENT,
                             KPMASTRECEIVE.MEMBER_NO
                        FROM KPMASTRECEIVE,   
                             KPMASTRECEIVEDET  
                       WHERE ( KPMASTRECEIVE.COOP_ID = KPMASTRECEIVEDET.COOP_ID ) and  
                             ( trim(KPMASTRECEIVE.KPSLIP_NO) = trim(KPMASTRECEIVEDET.KPSLIP_NO) ) and  
                             (KPMASTRECEIVEDET.KEEPITEMTYPE_CODE like 'L%') 
                        and KPMASTRECEIVEDET.KEEPITEM_STATUS <> 1
                        and KPMASTRECEIVE.MEMBER_NO = {1}
                        and KPMASTRECEIVE.COOP_ID = {0}
                       GROUP BY  KPMASTRECEIVE.COOP_ID,   
                                        KPMASTRECEIVE.RECEIPT_NO,   
                                        KPMASTRECEIVEDET.KEEPITEM_STATUS,      
                                        KPMASTRECEIVEDET.RECV_PERIOD,         
                                        KPMASTRECEIVE.MEMBER_NO
                        ORDER BY RECV_PERIOD, MEMBER_NO   
                    ";

            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, member_no);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }
    }
}