using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using System.Data;

namespace Saving.Applications.assist.ws_as_ucfassisttypedet_ctrl
{
    public partial class DsList : DataSourceRepeater
    {
        public DataSet1.ASSUCFASSISTTYPEDETDataTable DATA { get; private set; }
        public void InitDsList(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.ASSUCFASSISTTYPEDET;
            this.EventItemChanged = "OnDsListItemChanged";
            this.EventClicked = "OnDsListClicked";
            this.InitDataSource(pw, Repeater1, this.DATA, "dsList");
            this.Button.Add("b_del");
            this.Register();
        }

        /// <summary>
        /// ดึงข้อมูลกลุ่มประเภทการจ่ายสวัสดิการ
        /// </summary>
        ///        
        public void AssistPayType(string ls_asscode ,  ref string ls_minpaytype , ref string ls_minpaydesc )
        {
            string sql = @"select 
	                        assistpay_code, 
	                        assistpay_code || ' - ' || assistpay_desc display,
                            assistpay_desc,
	                        1 as sorter from assucfassisttypepay 
                        where coop_id= {0} and assisttype_code = {1}
                        order by sorter, assistpay_code";
            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, ls_asscode);
            DataTable dt = WebUtil.Query(sql);
            ls_minpaytype = dt.Rows[0].Field<string>("assistpay_code");
            ls_minpaydesc = dt.Rows[0].Field<string>("assistpay_desc");
            this.DropDownDataBind(dt, "assistpay_code", "display", "assistpay_code");
        }

        public void AssistPayTypeRow(string ls_asscode, ref string ls_minpaytype, int row)
        {
            string sql = @"select 
	                        assistpay_code, 
	                        assistpay_code || ' - ' || assistpay_desc display, 
                            assistpay_desc,
	                        1 as sorter from assucfassisttypepay 
                        where coop_id= {0} and assisttype_code = {1}
                        union
                        select '','',99 from dual order by sorter, assistpay_code";
            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, ls_asscode);
            DataTable dt = WebUtil.Query(sql);
            ls_minpaytype = dt.Rows[0].Field<string>("assistpay_code");
            this.DropDownDataBind(dt, "assistpay_code", "display", "assistpay_code" , row);
        }
        public void Membertype(string ls_membcat)
        {
            string sql = @"select membtype_code,membtype_desc as display,1 as sorter from mbucfmembtype where coop_id = {0} and member_type = {1}
                            union
                            select 'AL' as membtype_code,'ทั้งหมด',99 as sorter from dual order by sorter, membtype_code";
            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, ls_membcat);
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "membtype_code", "display", "membtype_code");
        }

 
        public void MembertypeRow(string ls_membcat, int row)
        {
            string sql = @"select membtype_code,membtype_desc as display,1 as sorter from mbucfmembtype where coop_id = {0} and member_type = {1}
                            union
                            select 'AL' as membtype_code,'ทั้งหมด',99 as sorter from dual order by sorter, membtype_code";
            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, ls_membcat);
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "membtype_code", "display", "membtype_code", row);
        }

        public void MemberCattype()
        {
            string sql = @"select distinct  '0' || to_char( member_type ) as membcat_code, case when member_type = '1' then 'สามัญ' else 'สมทบ' end as display,1 as sorter from mbucfmembtype
                            union
                            select 'AL' as membcat_code,'ทั้งหมด',99 as sorter from dual order by sorter, membcat_code";
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "membcat_code", "display", "membcat_code");
        }

        public void MemberCattypeFilter()
        {
            string sql = @"select membcat_code,membcat_desc as display,1 as sorter from mbucfcategory
                            union
                            select 'AL' as membcat_code,'ทั้งหมด',99 as sorter from dual order by sorter, membcat_code";
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "membcat_code", "display", "membcat_code");
        }

        public void RetrieveData(string ls_asstypecode, int li_year)
        {
            string sql = @"select * from ASSUCFASSISTTYPEDET 
                           where assisttype_code='" + ls_asstypecode + @"' 
                           and coop_id ='" + state.SsCoopControl + "' and assist_year = '" + li_year + "' order by assistpay_code,membtype_code,min_check,seq_no";
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);
            //AssistPayType(ls_asstypecode, ref ls_minpaytype);
        }
    }
}