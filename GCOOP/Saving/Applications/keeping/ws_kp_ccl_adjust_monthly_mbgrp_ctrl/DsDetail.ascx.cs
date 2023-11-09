using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using System.Data;

namespace Saving.Applications.keeping.ws_kp_ccl_adjust_monthly_mbgrp_ctrl
{
    public partial class DsDetail : DataSourceRepeater
    {
        public DataSet1.SLSLIPADJUSTDETDataTable DATA { get; set; }

        public void InitDsDetail(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.SLSLIPADJUSTDET;
            this.EventItemChanged = "OnDsDetailItemChanged";
            this.EventClicked = "OnDsDetailClicked";
            this.InitDataSource(pw, Repeater1, this.DATA, "dsDetail");
            this.Register();
        }

        public void RetrieveKpdet(string recv_period, string kpslip_no, string coop_id)
        {
            string sql = @"SELECT 1 as operate_flag,    
                             KPTEMPRECEIVEDET.KEEPITEMTYPE_CODE AS SLIPITEMTYPE_CODE,   
                             KPTEMPRECEIVEDET.SEQ_NO,   
                             KPTEMPRECEIVEDET.LOANCONTRACT_NO,   
                             KPTEMPRECEIVEDET.DESCRIPTION AS SLIPITEM_DESC,   
                             KPTEMPRECEIVEDET.PRINCIPAL_PAYMENT AS PRINCIPAL_ADJAMT,   
                             KPTEMPRECEIVEDET.INTEREST_PAYMENT AS INTEREST_ADJAMT,   
                             KPTEMPRECEIVEDET.INTARREAR_PAYMENT AS INTARREAR_ADJAMT,   
                             KPTEMPRECEIVEDET.ITEM_PAYMENT AS ITEM_ADJAMT,   
                             KPTEMPRECEIVEDET.ITEM_BALANCE,    
                             KPTEMPRECEIVEDET.PERIOD AS PERIODADJ_AMT,   
                             KPTEMPRECEIVEDET.BFINTEREST_ARREAR AS BFINTARR_AMT,   
                             KPTEMPRECEIVEDET.BFINTEREST_ARREAR AS BFINTARRSET_AMT,   
                             KPTEMPRECEIVEDET.CALINTTO_DATE AS BFLASTCALINT_DATE,   
                             KPTEMPRECEIVEDET.CALINTTO_DATE AS BFLASTPROC_DATE,   
                             KPTEMPRECEIVEDET.CALINTTO_DATE AS BFLASTPAY_DATE,   
                             KPTEMPRECEIVEDET.ITEM_BALANCE AS BFSHRCONT_BALAMT,   
                             KPTEMPRECEIVEDET.BFCONTRACT_STATUS AS BFSHRCONT_STATUS,   
                             KPTEMPRECEIVEDET.BFCONTLAW_STATUS,    
                             KPTEMPRECEIVEDET.PRINCIPAL_PAYMENT AS BFMTHPAY_PRNAMT,   
                             KPTEMPRECEIVEDET.INTEREST_PAYMENT AS BFMTHPAY_INTAMT,   
                             KPTEMPRECEIVEDET.ITEM_PAYMENT AS BFMTHPAY_ITEMAMT,   
                             KPTEMPRECEIVEDET.INTARREAR_PAYMENT AS BFMTHPAY_INTARRAMT,   
                             KPTEMPRECEIVEDET.MEMBER_NO AS BFMTHPAY_REFMEMBNO,   
                             KPTEMPRECEIVEDET.SEQ_NO AS BFMTHPAY_SEQNO,   
                             KPUCFKEEPITEMTYPE.SIGN_FLAG,   
                             KPTEMPRECEIVEDET.COOP_ID,   
                             KPTEMPRECEIVEDET.SHRLONTYPE_CODE  
                        FROM KPTEMPRECEIVEDET, KPUCFKEEPITEMTYPE   
	                    WHERE ( KPTEMPRECEIVEDET.KEEPITEMTYPE_CODE = KPUCFKEEPITEMTYPE.KEEPITEMTYPE_CODE )
                        and   ( KPTEMPRECEIVEDET.KPSLIP_NO = {0} ) AND  
                              ( KPTEMPRECEIVEDET.COOP_ID = {1} ) and
                              ( KPTEMPRECEIVEDET.RECV_PERIOD = {2} )";
            sql = WebUtil.SQLFormat(sql, kpslip_no, coop_id, recv_period);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }

    }
}