using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace Saving.Applications.account.dlg.w_dlg_vc_trn_fin_ctrl
{
    public partial class DsList : DataSourceRepeater
    {
        public DataSet1.FINSLIPDataTable DATA { get; set; }
        public void InitDsList(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.FINSLIP;
            this.EventItemChanged = "OnDsListItemChanged";
            this.EventClicked = "OnDsListClicked";
            this.InitDataSource(pw, Repeater1, this.DATA, "dsList");
            this.Register();
        }

        public void Retrieve(DateTime vdate)
        {
            string sql = @"
    SELECT DISTINCT  FINSLIP.SLIP_NO,   
                     FINSLIP.PAY_TOWHOM,   
                     FINSLIP.ENTRY_ID,   
                     FINSLIP.NONMEMBER_DETAIL,   
                     FINSLIP.MEMBER_NO,   
                     FINSLIP.ITEM_AMTNET,   
                     0 as operate_flag  
                FROM FINSLIP,   
                     FINSLIPDET  
               WHERE ( FINSLIP.SLIP_NO = FINSLIPDET.SLIP_NO ) and  
                     ( ( FINSLIP.OPERATE_DATE = {1} ) AND  
                     ( FINSLIPDET.POSTTOVC_FLAG = 0 ) AND  
                     ( FINSLIP.FROM_SYSTEM = 'FIN' ) AND  
                     ( FINSLIP.CASH_TYPE <> 'CSH' ) AND  
                     ( FINSLIP.RECEIVE_STATUS = 1 ) AND  
                     ( FINSLIP.COOP_ID = {0} ) AND  
                     ( FINSLIP.PAYMENT_STATUS = 1 ) )     ";
            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, vdate);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }


        
    }
}