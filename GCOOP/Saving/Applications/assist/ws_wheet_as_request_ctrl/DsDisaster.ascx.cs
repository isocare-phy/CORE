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
    public partial class DsDisaster : DataSourceFormView
    {
        public DataSet1.ASSREQMASTERDataTable DATA { get; set; }
        public void InitDsDisaster(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.ASSREQMASTER;
            this.EventItemChanged = "OnDsDisasterItemChanged";
            this.EventClicked = "OnDsDisasterClicked";
            this.InitDataSource(pw, FormView1, this.DATA, "dsDisaster");
            this.Button.Add("b_linkaddress");
            this.Register();
        }

        public void Retrieve(String memno, String assisttype_code)
        {
            //string sql = @"select assistpay_code disaster_type, assreqmaster.* from assreqmaster where req_status = 8 and coop_control={0} and member_no={1} and assisttype_code={2}";
            string sql = @"
                    select 
	                    ass.assist_docno,
	                    ass.assistpay_code disaster_type,
	                    ass.assist_amt,
	                    ass.req_date,
	                    ass.disaster_date,
	                    mb.addr_no H_MEMB_ADDR,
	                    mb.addr_moo H_MOO,
	                    mb.addr_soi H_SOI,
	                    mb.addr_village H_village,
	                    mb.addr_road H_ROAD,
	                    tam.tambol_code H_TAMBOL_CODE,
	                    dis.district_code H_DISTRICT_CODE,
	                    pro.province_code H_PROVINCE_CODE,
	                    mb.addr_postcode H_POSTCODE,
	                    ass.memb_addr,
	                    ass.soi,
	                    ass.road,
	                    ass.tambol_code,
	                    ass.district_code,
	                    ass.province_code,
	                    ass.postcode,
                        ass.damages_amt
                    from (select * from mbmembmaster where coop_id = {0} and member_no = {1}) mb
                    left join (select * from assreqmaster where req_status = 8 and coop_control = {0} and member_no = {1} and assisttype_code = {2}) ass on mb.member_no = ass.member_no
                    left join mbucfprovince pro on mb.province_code = pro.province_code
                    left join mbucfdistrict dis on mb.amphur_code = dis.district_code and mb.province_code = pro.province_code
                    left join mbucftambol tam on mb.tambol_code = tam.tambol_code and mb.amphur_code = dis.district_code
                    ";
            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, memno, assisttype_code);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }

        public void Retrieve2(String memno, String assist_docno)
        {
            //string sql = @"select assistpay_code disaster_type, assreqmaster.* from assreqmaster where req_status = 8 and coop_control={0} and member_no={1} and assisttype_code={2}";
            string sql = @"
                    select 
	                    ass.assist_docno,
	                    ass.assistpay_code disaster_type,
	                    ass.assist_amt,
	                    ass.req_date,
	                    ass.disaster_date,
	                    mb.addr_no H_MEMB_ADDR,
	                    mb.addr_moo H_MOO,
	                    mb.addr_soi H_SOI,
	                    mb.addr_village H_village,
	                    mb.addr_road H_ROAD,
	                    tam.tambol_code H_TAMBOL_CODE,
	                    dis.district_code H_DISTRICT_CODE,
	                    pro.province_code H_PROVINCE_CODE,
	                    mb.addr_postcode H_POSTCODE,
	                    ass.memb_addr,
	                    ass.soi,
	                    ass.road,
	                    ass.tambol_code,
	                    ass.district_code,
	                    ass.province_code,
	                    ass.postcode,
                        ass.damages_amt
                    from (select * from mbmembmaster where coop_id = {0} and member_no = {1}) mb
                    left join (select * from assreqmaster where req_status = 8 and coop_control = {0} and member_no = {1} and assist_docno = {2}) ass on mb.member_no = ass.member_no
                    left join mbucfprovince pro on mb.province_code = pro.province_code
                    left join mbucfdistrict dis on mb.amphur_code = dis.district_code and mb.province_code = pro.province_code
                    left join mbucftambol tam on mb.tambol_code = tam.tambol_code and mb.amphur_code = dis.district_code
                    ";
            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, memno, assist_docno);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
        }

        public void DdDisaster_type(string ls_getassisttype)
        {
            string sql = @"select * from
                        (
	                        select
		                        assucfassistpaytype.assistpay_code,
		                        assucfassistpaytype.assistpay_desc
	                        from assucfassisttypedet
	                        inner join assucfassistpaytype on assucfassisttypedet.assisttype_pay = assucfassistpaytype.assistpay_code
	                        where assucfassisttypedet.coop_id = {0} and assucfassisttypedet.assisttype_code = {1}
	                        union
	                        select
		                        '00',
		                        case assucfassistpaytype.assisttype_group when '01' then 'เลือกระดับชั้น' when '03' then 'เลือกผู้เกียวข้อง' when '04' then 'เลือกภัยพิบัติ' else '' end 
	                        from assucfassisttypedet
	                        inner join assucfassistpaytype on assucfassisttypedet.assisttype_pay = assucfassistpaytype.assistpay_code
	                        where assucfassisttypedet.coop_id = {0} and assucfassisttypedet.assisttype_code = {1} and rownum = 1
                        )order by assistpay_code";
            sql = WebUtil.SQLFormat(sql, state.SsCoopId, ls_getassisttype);
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "disaster_type", "assistpay_desc", "assistpay_code");
        }
//        public void GetAddr(string memno)
//        {
//            string sql = @"select 
//	                        mb.addr_no H_MEMB_ADDR,
//	                        mb.addr_moo H_MOO,
//	                        mb.addr_soi H_SOI,
//	                        mb.addr_village H_village,
//	                        mb.addr_road H_ROAD,
//	                        tam.tambol_code H_TAMBOL_CODE,
//	                        dis.district_code H_DISTRICT_CODE,
//	                        pro.province_code H_PROVINCE_CODE,
//	                        mb.addr_postcode H_POSTCODE
//                        from mbmembmaster mb
//                        inner join mbucfprovince pro on mb.province_code = pro.province_code
//                        inner join mbucfdistrict dis on mb.amphur_code = dis.district_code and mb.province_code = pro.province_code
//                        inner join mbucftambol tam on mb.tambol_code = tam.tambol_code and mb.amphur_code = dis.district_code
//                        where member_no = {0}";
//            sql = WebUtil.SQLFormat(sql, memno);
//            DataTable dt = WebUtil.Query(sql);
//            this.ImportData(dt);
//        }

        public void DdCurrProvince()
        {
            string sql = @"
              SELECT PROVINCE_CODE,   
                     PROVINCE_DESC  ,1 as sorter
                FROM MBUCFPROVINCE  
            union 
            select '','',0 from dual order by sorter,PROVINCE_DESC ASC";
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "H_PROVINCE_CODE", "PROVINCE_DESC", "PROVINCE_CODE");
        }
        public void DdProvince()
        {
            string sql = @"
              SELECT PROVINCE_CODE,   
                     PROVINCE_DESC  ,1 as sorter
                FROM MBUCFPROVINCE  
            union 
            select '','',0 from dual order by sorter,PROVINCE_DESC ASC";
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "PROVINCE_CODE", "PROVINCE_DESC", "PROVINCE_CODE");
        }

        public void DdCurrDistrict(string currprovince_code)
        {
            string sql = @"
              SELECT DISTRICT_CODE,   
                     PROVINCE_CODE,   
                     DISTRICT_DESC,   
                     POSTCODE  ,1 as sorter
                FROM MBUCFDISTRICT 
              where (PROVINCE_CODE={0}) 
            union
            select '','','','',0 from dual ORDER BY sorter,DISTRICT_DESC ASC,   
                     DISTRICT_CODE ASC  ";
            sql = WebUtil.SQLFormat(sql, currprovince_code);
            DataTable dt = WebUtil.Query(sql);            
            this.DropDownDataBind(dt, "H_DISTRICT_CODE", "DISTRICT_DESC", "DISTRICT_CODE");
        }
        public void DdDistrict(string province_code)
        {
            string sql = @"
              SELECT DISTRICT_CODE,   
                     PROVINCE_CODE,   
                     DISTRICT_DESC,   
                     POSTCODE  ,1 as sorter
                FROM MBUCFDISTRICT 
              where (PROVINCE_CODE={0}) 
            union
            select '','','','',0 from dual ORDER BY sorter,DISTRICT_DESC ASC,   
                     DISTRICT_CODE ASC  ";
            sql = WebUtil.SQLFormat(sql, province_code);
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "DISTRICT_CODE", "DISTRICT_DESC", "DISTRICT_CODE");
        }
        public void DdCurrTambol(string currdistrict_code)
        {
            string sql = @"
              SELECT MBUCFTAMBOL.TAMBOL_CODE,   
                     MBUCFTAMBOL.TAMBOL_DESC,   
                     MBUCFTAMBOL.DISTRICT_CODE  ,1 as sorter
                FROM MBUCFTAMBOL,   
                     MBUCFDISTRICT  
               WHERE ( MBUCFTAMBOL.DISTRICT_CODE = MBUCFDISTRICT.DISTRICT_CODE )  and
                     (MBUCFTAMBOL.DISTRICT_CODE={0})
             union
            select '','','',0 from dual order by sorter,TAMBOL_DESC ASC ";
            sql = WebUtil.SQLFormat(sql, currdistrict_code);
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "H_TAMBOL_CODE", "TAMBOL_DESC", "TAMBOL_CODE");
        }
        public void DdTambol(string district_code)
        {
            string sql = @"
              SELECT MBUCFTAMBOL.TAMBOL_CODE,   
                     MBUCFTAMBOL.TAMBOL_DESC,   
                     MBUCFTAMBOL.DISTRICT_CODE  ,1 as sorter
                FROM MBUCFTAMBOL,   
                     MBUCFDISTRICT  
               WHERE ( MBUCFTAMBOL.DISTRICT_CODE = MBUCFDISTRICT.DISTRICT_CODE )  and
                     (MBUCFTAMBOL.DISTRICT_CODE={0})
             union
            select '','','',0 from dual order by sorter,TAMBOL_DESC ASC ";
            sql = WebUtil.SQLFormat(sql, district_code);
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "TAMBOL_CODE", "TAMBOL_DESC", "TAMBOL_CODE");
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}
