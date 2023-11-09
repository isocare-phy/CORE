using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using System.Data;

namespace Saving.Applications.shrlon.ws_lon_prc_preparepay_ctrl
{
    public partial class DsLoan : DataSourceRepeater
    {
        public DataSet1.loanDataTable DATA { get; set; }

        public void InitDsLoan(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.loan;
            this.EventItemChanged = "OnDsLoanItemChanged";
            this.EventClicked = "OnDsLoanClicked";
            this.InitDataSource(pw, Repeater1, this.DATA, "dsLoan");
            this.Register();
        }
        public void RetrieveLoan()
        {
            String sql = @" SELECT  
                            LNLOANTYPE.COOP_ID ,          
                            LNLOANTYPE.LOANTYPE_CODE ,           
                            LNLOANTYPE.LOANTYPE_DESC ,           
                            0 as OPERATE_FLAG    
                            FROM LNLOANTYPE
                            LEFT JOIN CMCOOPMASTER on LNLOANTYPE.COOP_ID = CMCOOPMASTER.COOP_CONTROL
                            WHERE CMCOOPMASTER.COOP_ID = {0} 
                            ORDER BY LNLOANTYPE.LOANTYPE_CODE";
            sql = WebUtil.SQLFormat(sql, state.SsCoopControl);
                DataTable dt = WebUtil.Query(sql);
                this.ImportData(dt);

        }
    }
}