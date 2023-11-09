using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace Saving.Applications.account.dlg.w_dlg_vc_trn_share_ctrl
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
            SELECT  DISTINCT SLSLIPPAYOUT.PAYOUTSLIP_NO,
                    MBUCFPRENAME.PRENAME_DESC || MBMEMBMASTER.MEMB_NAME || ' ' ||
                    MBMEMBMASTER.MEMB_SURNAME as memb_name,
                    MBMEMBMASTER.MEMBER_NO,
                    SLSLIPPAYOUT.PAYOUT_AMT,
                    MBMEMBMASTER.MEMBGROUP_CODE,
                    SLSLIPPAYOUT.SLIP_DATE,
                    SLSLIPPAYOUT.MONEYTYPE_CODE
            FROM    MBMEMBMASTER,
                    SLSLIPPAYIN,
                    SLSLIPPAYINDET,
                    SLSLIPPAYOUT,
                    MBUCFPRENAME
            WHERE   ( slslippayin.payinslip_no = slslippayindet.payinslip_no (+)) and
                    ( slslippayout.slipclear_no = slslippayin.payinslip_no (+)) and
                    ( MBMEMBMASTER.COOP_ID = SLSLIPPAYOUT.COOP_ID) and
                    ( SLSLIPPAYIN.COOP_ID = SLSLIPPAYINDET.COOP_ID(+)) and
                    ( SLSLIPPAYOUT.COOP_ID = SLSLIPPAYIN.COOP_ID(+)) and
                    ( MBMEMBMASTER.PRENAME_CODE = MBUCFPRENAME.PRENAME_CODE ) and
                    ( SLSLIPPAYOUT.MEMBER_NO = MBMEMBMASTER.MEMBER_NO ) and
                    ( ( SLSLIPPAYOUT.SLIP_DATE = {1} ) AND
                    ( SLSLIPPAYOUT.COOP_ID = {0} ) AND
                    ( SLSLIPPAYOUT.SLIPTYPE_CODE = 'SWD' ) ) AND
                    SLSLIPPAYOUT.SLIP_STATUS = 1 AND
                    (SLSLIPPAYOUT.POSTTOVC_FLAG = 0 OR
                    SLSLIPPAYOUT.POSTTOVC_FLAG is null) and
                    SLSLIPPAYOUT.MONEYTYPE_CODE <> 'CSH' ";
            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, vdate);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }

        public void RetrieveCash(DateTime vdate)
        {
            string sql = @"
            SELECT  DISTINCT SLSLIPPAYOUT.PAYOUTSLIP_NO,
                    MBUCFPRENAME.PRENAME_DESC || MBMEMBMASTER.MEMB_NAME || ' ' ||
                    MBMEMBMASTER.MEMB_SURNAME as memb_name,
                    MBMEMBMASTER.MEMBER_NO,
                    SLSLIPPAYOUT.PAYOUT_AMT,
                    MBMEMBMASTER.MEMBGROUP_CODE,
                    SLSLIPPAYOUT.SLIP_DATE,
                    SLSLIPPAYOUT.MONEYTYPE_CODE
            FROM    MBMEMBMASTER,
                    SLSLIPPAYIN,
                    SLSLIPPAYINDET,
                    SLSLIPPAYOUT,
                    MBUCFPRENAME
            WHERE   ( slslippayin.payinslip_no = slslippayindet.payinslip_no (+)) and
                    ( slslippayout.slipclear_no = slslippayin.payinslip_no (+)) and
                    ( MBMEMBMASTER.COOP_ID = SLSLIPPAYOUT.COOP_ID) and
                    ( SLSLIPPAYIN.COOP_ID = SLSLIPPAYINDET.COOP_ID(+)) and
                    ( SLSLIPPAYOUT.COOP_ID = SLSLIPPAYIN.COOP_ID(+)) and
                    ( MBMEMBMASTER.PRENAME_CODE = MBUCFPRENAME.PRENAME_CODE ) and
                    ( SLSLIPPAYOUT.MEMBER_NO = MBMEMBMASTER.MEMBER_NO ) and
                    ( ( SLSLIPPAYOUT.SLIP_DATE = {1} ) AND
                    ( SLSLIPPAYOUT.COOP_ID = {0} ) AND
                    ( SLSLIPPAYOUT.SLIPTYPE_CODE = 'SWD' ) ) AND
                    SLSLIPPAYOUT.SLIP_STATUS = 1 AND
                    (SLSLIPPAYOUT.POSTTOVC_FLAG = 0 OR
                    SLSLIPPAYOUT.POSTTOVC_FLAG is null) and
                    SLSLIPPAYOUT.MONEYTYPE_CODE = 'CSH'  ";
            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, vdate);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }



    }
}