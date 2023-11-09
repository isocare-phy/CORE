using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace Saving.Applications.account.dlg.w_dlg_vc_trn_loan_ctrl
{
    public partial class DsList : DataSourceRepeater
    {
        public DataSet1.DataTable1DataTable DATA { get; set; }
        public void InitDsList(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.DataTable1;
            this.EventItemChanged = "OnDsListItemChanged";
            this.EventClicked = "OnDsListClicked";
            this.InitDataSource(pw, Repeater1, this.DATA, "dsList");
            this.Register();
        }

        public void Retrieve(DateTime vdate)
        {
            string sql = @"
              SELECT 
	                 SLSLIPPAYOUT.PAYOUTSLIP_NO,   
                     SLSLIPPAYOUT.SLIP_DATE,   
                     SLSLIPPAYOUT.SLIPTYPE_CODE,   
                     SLSLIPPAYOUT.LOANCONTRACT_NO,   
                     SLSLIPPAYOUT.MEMBER_NO || ' - ' ||  MBUCFPRENAME.PRENAME_DESC || 
                     MBMEMBMASTER.MEMB_NAME   || ' ' ||  MBMEMBMASTER.MEMB_SURNAME as memb_name,   
                     SLSLIPPAYOUT.SHRLONTYPE_CODE,   
                     SLSLIPPAYOUT.PAYOUT_AMT,   
                     SLSLIPPAYOUT.ENTRY_ID,   
                     SLSLIPPAYOUT.SLIP_STATUS
                FROM SLSLIPPAYOUT,   
                     MBMEMBMASTER,   
                     MBUCFPRENAME  
               WHERE ( SLSLIPPAYOUT.COOP_ID = MBMEMBMASTER.COOP_ID ) and  
                     ( SLSLIPPAYOUT.MEMBER_NO = MBMEMBMASTER.MEMBER_NO ) and  
                     ( MBUCFPRENAME.PRENAME_CODE = MBMEMBMASTER.PRENAME_CODE ) and  
                     ( ( SLSLIPPAYOUT.SLIP_DATE = {1} ) AND  
                     ( SLSLIPPAYOUT.COOP_ID = {0} ) AND  
                     ( SLSLIPPAYOUT.SLIP_STATUS = 1 ) AND  
                     (SLSLIPPAYOUT.POSTTOVC_FLAG = 0 OR  
                     SLSLIPPAYOUT.POSTTOVC_FLAG is null) AND  
                     SLSLIPPAYOUT.MONEYTYPE_CODE <> 'CSH' and
                     SLSLIPPAYOUT.SLIPTYPE_CODE <> 'SWD')    
            order by SLSLIPPAYOUT.SHRLONTYPE_CODE,SLSLIPPAYOUT.LOANCONTRACT_NO, SLSLIPPAYOUT.SLIP_DATE ";
            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, vdate);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }


        public void RetrieveCash(DateTime vdate)
        {
            string sql = @"
              SELECT 
	                 SLSLIPPAYOUT.PAYOUTSLIP_NO,   
                     SLSLIPPAYOUT.SLIP_DATE,   
                     SLSLIPPAYOUT.SLIPTYPE_CODE,   
                     SLSLIPPAYOUT.LOANCONTRACT_NO,   
                     SLSLIPPAYOUT.MEMBER_NO || ' - ' ||  MBUCFPRENAME.PRENAME_DESC || 
                     MBMEMBMASTER.MEMB_NAME   || ' ' ||  MBMEMBMASTER.MEMB_SURNAME as memb_name,   
                     SLSLIPPAYOUT.SHRLONTYPE_CODE,   
                     SLSLIPPAYOUT.PAYOUT_AMT,   
                     SLSLIPPAYOUT.ENTRY_ID,   
                     SLSLIPPAYOUT.SLIP_STATUS
                FROM SLSLIPPAYOUT,   
                     MBMEMBMASTER,   
                     MBUCFPRENAME  
               WHERE ( SLSLIPPAYOUT.COOP_ID = MBMEMBMASTER.COOP_ID ) and  
                     ( SLSLIPPAYOUT.MEMBER_NO = MBMEMBMASTER.MEMBER_NO ) and  
                     ( MBUCFPRENAME.PRENAME_CODE = MBMEMBMASTER.PRENAME_CODE ) and  
                     ( ( SLSLIPPAYOUT.SLIP_DATE = {1} ) AND  
                     ( SLSLIPPAYOUT.COOP_ID = {0} ) AND  
                     ( SLSLIPPAYOUT.SLIP_STATUS = 1 ) AND
                     (SLSLIPPAYOUT.POSTTOVC_FLAG = 0 OR  
                     SLSLIPPAYOUT.POSTTOVC_FLAG is null) AND  
                     SLSLIPPAYOUT.MONEYTYPE_CODE = 'CSH'  and
                     SLSLIPPAYOUT.SLIPTYPE_CODE <> 'SWD')   
            order by SLSLIPPAYOUT.SHRLONTYPE_CODE,SLSLIPPAYOUT.LOANCONTRACT_NO, SLSLIPPAYOUT.SLIP_DATE ";
            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, vdate);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }

        public void RetrieveCancel(DateTime vdate)
        {
            string sql = @"
              SELECT  DISTINCT SLSLIPPAYOUT.PAYOUTSLIP_NO,   
                     SLSLIPPAYOUT.SLIP_DATE,   
                     SLSLIPPAYOUT.SLIPTYPE_CODE,   
                     SLSLIPPAYOUT.LOANCONTRACT_NO,   
                     SLSLIPPAYOUT.MEMBER_NO || ' - ' ||  MBUCFPRENAME.PRENAME_DESC || 
                     MBMEMBMASTER.MEMB_NAME   || ' ' ||  MBMEMBMASTER.MEMB_SURNAME as memb_name,   
                     SLSLIPPAYOUT.SHRLONTYPE_CODE,   
                     SLSLIPPAYOUT.PAYOUT_AMT,   
                     SLSLIPPAYOUT.ENTRY_ID,   
                     SLSLIPPAYOUT.SLIP_STATUS
                FROM SLSLIPPAYOUT,   
                     MBMEMBMASTER,   
                     MBUCFPRENAME  
               WHERE ( SLSLIPPAYOUT.COOP_ID = MBMEMBMASTER.COOP_ID ) and  
                     ( SLSLIPPAYOUT.MEMBER_NO = MBMEMBMASTER.MEMBER_NO ) and  
                     ( MBUCFPRENAME.PRENAME_CODE = MBMEMBMASTER.PRENAME_CODE ) and  
                     ( ( SLSLIPPAYOUT.CANCEL_DATE = {1}) AND  
                     ( SLSLIPPAYOUT.COOP_ID = {0}) AND  
                     ( SLSLIPPAYOUT.SLIP_STATUS < 0 ) AND  
                     (SLSLIPPAYOUT.POSTTOVC_FLAG = 1 ) AND  
                        SLSLIPPAYOUT.canceltovc_flag = 0 AND
                     SLSLIPPAYOUT.MONEYTYPE_CODE <> 'CSH' and
                     SLSLIPPAYOUT.SLIPTYPE_CODE <> 'SWD')    
           
            UNION 
            SELECT   DISTINCT SLSLIPPAYIN.PAYINSLIP_NO,   
 			            SLSLIPPAYIN.SLIP_DATE, 
 			            SLSLIPPAYIN.SLIPTYPE_CODE,   
			             '' as LOANCONTRACT_NO,  
			            SLSLIPPAYIN.MEMBER_NO  || ' - ' ||  MBUCFPRENAME.PRENAME_DESC || 
                                 MBMEMBMASTER.MEMB_NAME   || ' ' ||  MBMEMBMASTER.MEMB_SURNAME as memb_name,  
			            '' as SHRLONTYPE_CODE, 
			            SLSLIPPAYIN.SLIP_AMT ,
 			            SLSLIPPAYIN.ENTRY_ID,   
      		            SLSLIPPAYIN.SLIP_STATUS 
                FROM SLUCFSLIPTYPE,   
                     CMUCFMONEYTYPE,   
                     SLSLIPPAYIN,   
   		            MBMEMBMASTER,   
  		            MBUCFPRENAME         
               WHERE SLSLIPPAYIN.COOP_ID = MBMEMBMASTER.COOP_ID AND 
                     ( SLSLIPPAYIN.MEMBER_NO = MBMEMBMASTER.MEMBER_NO ) and  
 		             ( MBUCFPRENAME.PRENAME_CODE = MBMEMBMASTER.PRENAME_CODE ) and
                     ( SLSLIPPAYIN.SLIPTYPE_CODE = SLUCFSLIPTYPE.SLIPTYPE_CODE ) and  
                     ( SLSLIPPAYIN.MONEYTYPE_CODE = CMUCFMONEYTYPE.MONEYTYPE_CODE ) and  
                     ( SLSLIPPAYIN.MONEYTYPE_CODE <> 'CSH' ) AND  
                     ( ( SLSLIPPAYIN.CANCEL_DATE = {1} ) AND  
                     ( SLSLIPPAYIN.COOP_ID = {0} ) AND  
                     ( SLSLIPPAYIN.SLIP_STATUS < 0 ) AND  
                     ( SLUCFSLIPTYPE.SLIPTYPESIGN_FLAG = 1 ) ) AND  
                     ( SLSLIPPAYIN.POSTTOVC_FLAG = 1 ) AND
		             ( slslippayin.canceltovc_flag = 0
                        OR  
                     ( SLSLIPPAYIN.CANCELTOVC_FLAG is null) ) AND  
                     ( SLSLIPPAYIN.SLIPTYPE_CODE = 'PX' )  
            order by SHRLONTYPE_CODE,LOANCONTRACT_NO, SLIP_DATE ";
            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, vdate);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }
    }
}
