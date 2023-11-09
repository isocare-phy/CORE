using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using System.Data;

namespace Saving.Applications.assist.ws_wheet_as_request_ctrl
{
    public partial class DsGain : DataSourceRepeater
    {
        public DataSet1.ASSREQMASTERDataTable DATA { get; set; }
        public void InitDsGain(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.ASSREQMASTER;
            this.EventItemChanged = "OnDsGainItemChanged";
            this.EventClicked = "OnDsGainClicked";
            this.InitDataSource(pw, Repeater1, this.DATA, "dsGain");
            this.Button.Add("b_del");
            this.Register();
        }

        public void RetrieveGainMb(String memno)
        {
            string sql = @"select
        	                seq_no,
        	                gain_name,
        	                gain_surname,
        	                gain_relation assistpay_code
                        from mbgainmaster
                        where coop_id = {0} and member_no = {1}
                        order by seq_no";
            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, memno);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }

        public void RetrieveGainAss(String memno)
        {
            string sql = @"select 
	                        seq_no,
	                        gainprename_code,
	                        gain_name,
	                        gain_surname,
	                        gain_relation assistpay_code
                        from assreqmaster a
                        inner join assreqgain b on a.assist_docno = b.assist_docno
                        where b.coop_control = {0} and member_no = {1} and req_status = 8
                        order by seq_no";
            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, memno);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }

        public void RetrievePrename()
        {
            string sql = @"select prename_code gainprename_code, prename_desc from mbucfprename order by prename_code";
            sql = WebUtil.SQLFormat(sql);
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "gainprename_code", "prename_desc", "gainprename_code");
        }

        public void GainRealtion()
        {
            string sql = @"
            SELECT CONCERN_CODE,
                GAIN_CONCERN
            FROM MBUCFGAINCONCERN
            order by CONCERN_CODE";
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "assistpay_code", "GAIN_CONCERN", "CONCERN_CODE");
        }
    }
}