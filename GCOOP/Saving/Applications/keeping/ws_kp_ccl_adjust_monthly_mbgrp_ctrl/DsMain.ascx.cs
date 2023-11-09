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
    public partial class DsMain : DataSourceFormView
    {
        public DataSet1.SLSLIPADJUSTDataTable DATA { get; private set; }

        public void InitDsMain(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.SLSLIPADJUST;
            this.EventItemChanged = "OnDsMainItemChanged";
            this.EventClicked = "OnDsMainClicked";
            this.InitDataSource(pw, FormView1, this.DATA, "dsMain");
            this.Register();
        }

        public void DdSlslipadjtype()
        {
            string sql = @" SELECT COOP_ID, ADJTYPE_CODE, ADJTYPE_DESC, ADJTYPE_SORT  
                            FROM SLUCFSLIPADJTYPE  
                            WHERE COOP_ID = {0} ORDER BY ADJTYPE_SORT";
            sql = WebUtil.SQLFormat(sql, state.SsCoopControl);
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "ADJTYPE_CODE", "ADJTYPE_DESC", "ADJTYPE_CODE");
        }

        public void RetrieveKpmast(string recv_period, string kpslip_no, string coop_id)
        {
            string sql = @"SELECT MBMEMBMASTER.MEMB_NAME,   
                                 MBMEMBMASTER.MEMB_SURNAME,   
                                 MBMEMBMASTER.MEMBGROUP_CODE,   
                                 MBUCFPRENAME.PRENAME_DESC,   
                                 KPTEMPRECEIVE.MEMBER_NO,
                                 'MTH' as ADJTYPE_CODE,   
                                 KPTEMPRECEIVE.KPSLIP_NO AS REF_SLIPNO,   
                                 KPTEMPRECEIVE.RECEIPT_DATE AS REF_SLIPDATE,   
                                 KPTEMPRECEIVE.RECEIVE_AMT AS REF_SLIPAMT,   
                                 KPTEMPRECEIVE.RECEIVE_AMT AS SLIP_AMT,   
                                 1 AS SLIP_STATUS,   
                                 KPTEMPRECEIVE.COOP_ID,   
                                 KPTEMPRECEIVE.MEMCOOP_ID,   
                                 KPTEMPRECEIVE.RECV_PERIOD AS REF_RECVPERIOD,   
                                 1 AS SLIPRETALL_FLAG, 
                                 MBUCFMEMBGROUP.MEMBGROUP_DESC  
                           FROM KPTEMPRECEIVE,   
                                 MBMEMBMASTER,   
                                 MBUCFPRENAME,   
                                 MBUCFMEMBGROUP  
                           WHERE ( MBUCFPRENAME.PRENAME_CODE (+) = MBMEMBMASTER.PRENAME_CODE) and  
                                 ( KPTEMPRECEIVE.MEMBER_NO = MBMEMBMASTER.MEMBER_NO ) and  
                                 ( KPTEMPRECEIVE.MEMCOOP_ID = MBMEMBMASTER.COOP_ID ) and  
                                 ( MBMEMBMASTER.COOP_ID = MBUCFMEMBGROUP.COOP_ID ) and  
                                 ( MBMEMBMASTER.MEMBGROUP_CODE = MBUCFMEMBGROUP.MEMBGROUP_CODE ) and  
                                 ( KPTEMPRECEIVE.KPSLIP_NO = {0}) AND  
                                 ( KPTEMPRECEIVE.COOP_ID = {1} ) and
                                 ( KPTEMPRECEIVE.RECV_PERIOD = {2} )";
            sql = WebUtil.SQLFormat(sql, kpslip_no, coop_id, recv_period);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }
    }
}