using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CoreSavingLibrary;
using DataLibrary;

namespace Saving.Applications.hr.ws_hr_master_ctrl
{
    public partial class DsDetail : DataSourceFormView
    {
        public DataSet1.HREMPLOYEEDataTable DATA { get; set; }

        public void InitDsDetail(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.HREMPLOYEE;
            this.EventItemChanged = "OnDsDetailItemChanged";
            this.EventClicked = "OnDsDetailClicked";
            this.InitDataSource(pw, FormView1, this.DATA, "dsDetail");
            this.Button.Add("show");
            this.Register();
        }

        public void Retrieve(string as_empno)
        {
            string sql = @"select hremployee.coop_id,   
                 hremployee.emp_no,   
                 hremployee.prename_code,   
                 hremployee.emp_name,   
                 hremployee.emp_surname,   
                 hremployee.emp_ename,   
                 hremployee.emp_esurname,   
                 hremployee.emp_nickname,   
                 hremployee.emptype_code,   
                 hremployee.deptgrp_code,   
                 hremployee.pos_code,   
                 hremployee.sex,   
                 hremployee.bloodtype_code,   
                 hremployee.weight,   
                 hremployee.height,   
                 hremployee.nation,   
                 hremployee.religion,   
                 hremployee.adn_no,   
                 hremployee.adn_tambol,   
                 hremployee.adn_amphur,   
                 hremployee.adn_province,   
                 hremployee.adn_postcode,   
                 hremployee.adn_tel,   
                 hremployee.adn_email,   
                 hremployee.adr_no,   
                 hremployee.adr_tambol,   
                 hremployee.adr_amphur,   
                 hremployee.adr_province,   
                 hremployee.adr_postcode,   
                 hremployee.id_card,   
                 hremployee.birth_date,   
                 hremployee.work_date,   
                 hremployee.resign_date,   
                 hremployee.contain_date,   
                 hremployee.term_date,                       
                 hremployee.ref_membno,
                 hremployee.emp_status
            from hremployee
            where hremployee.coop_id = {0}
            and hremployee.emp_no = {1}";

            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, as_empno);
            Sdt dt = WebUtil.QuerySdt(sql);
            this.ImportData(dt);
        }

        public void DdDeptgrp()
        {
            string sql = @"
            select hdg.deptgrp_code,
            hdg.deptgrp_code || ' - ' || hdg.deptgrp_desc || ' สายงาน: ' || hdl.deptline_desc as deptgrp_desc,
            1 as sorter
            from hrucfdeptgrp hdg, hrucfdeptline hdl
            where hdg.deptline_code = hdl.deptline_code
            union 
            select '','', 0 from dual
            order by sorter, deptgrp_code";
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "deptgrp_code", "deptgrp_desc", "deptgrp_code");
        }

        public void DdPosition()
        {
            string sql = @"select pos_code,
            pos_desc,
            pos_money,
            1 as sorter
            from hrucfposition
            union 
            select '','', 0, 0 from dual
            order by sorter, pos_code";
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "pos_code", "pos_desc", "pos_code");
        }

        public void DdEmptype()
        {
            string sql = @"select emptype_code,
            emptype_desc,
            1 as sorter
            from hrucfemptype
            union 
            select '','', 0 from dual
            order by sorter, emptype_code";
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "emptype_code", "emptype_desc", "emptype_code");
        }

        public void DdPrename()
        {
            string sql = @"
            select prename_code,   
                prename_desc
            from mbucfprename
            order by prename_code";
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "prename_code", "prename_desc", "prename_code");
        }

        public void DdAdnTambol(string district_code)
        {
            string sql = @"
            select mbucftambol.tambol_code,   
                mbucftambol.tambol_desc,   
                mbucftambol.district_code  ,1 as sorter
                from mbucftambol,   
                mbucfdistrict  
            where ( mbucftambol.district_code = mbucfdistrict.district_code )
                and ( mbucftambol.district_code = {0} )
            union
            select '','','',0 from dual order by sorter,tambol_desc asc ";
            sql = WebUtil.SQLFormat(sql, district_code);
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "adn_tambol", "tambol_desc", "tambol_code");
        }

        public void DdAdrTambol(string district_code)
        {
            string sql = @"
            select mbucftambol.tambol_code,   
                mbucftambol.tambol_desc,   
                mbucftambol.district_code,
                1 as sorter
                from mbucftambol,   
                mbucfdistrict  
            where ( mbucftambol.district_code = mbucfdistrict.district_code )
                and ( mbucftambol.district_code = {0} )
            union
            select '','','',0 from dual order by sorter,tambol_desc asc ";
            sql = WebUtil.SQLFormat(sql, district_code);
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "adr_tambol", "tambol_desc", "tambol_code");
        }

        public void DdAdnAmphur(string province_code)
        {
            string sql = @"
            select district_code,   
                province_code,   
                district_desc,   
                postcode  ,1 as sorter
            from mbucfdistrict 
            where ( province_code = {0} ) 
            union
            select '','','','',0 from dual order by sorter,district_desc asc,   
                district_code asc";
            sql = WebUtil.SQLFormat(sql, province_code);
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "adn_amphur", "district_desc", "district_code");
        }

        public void DdAdrAmphur(string province_code)
        {
            string sql = @"
            select district_code,   
                province_code,   
                district_desc,   
                postcode  ,1 as sorter
            from mbucfdistrict 
            where ( province_code = {0} ) 
            union
            select '','','','',0 from dual order by sorter,district_desc asc,   
                district_code asc";
            sql = WebUtil.SQLFormat(sql, province_code);
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "adr_amphur", "district_desc", "district_code");
        }

        public void DdProvince()
        {
            string sql = @"
            select province_code,   
                province_desc  ,1 as sorter
            from mbucfprovince  
            union 
            select '','',0 from dual order by sorter, province_desc asc";
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "adn_province", "province_desc", "province_code");
            this.DropDownDataBind(dt, "adr_province", "province_desc", "province_code");
        }

        public void DdBloodtype()
        {
            string sql = @"select bloodtype_code,
            bloodtype_desc,
            1 as sorter
            from hrucfbloodtype
            union 
            select '','', 0 from dual
            order by sorter, bloodtype_code";
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "bloodtype_code", "bloodtype_desc", "bloodtype_code");
        }
    }
}