using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using System.Data;
using DataLibrary;

namespace Saving.Applications.hr.ws_hr_master_ctrl
{
    public partial class DsMain : DataSourceFormView
    {
        public DataSet1.DT_MainDataTable DATA { get; set; }

        public void InitDsMain(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.DT_Main;
            this.EventItemChanged = "OnDsMainItemChanged";
            this.EventClicked = "OnDsMainClicked";
            this.InitDataSource(pw, FormView1, this.DATA, "dsMain");
            this.TableName = "HREMPLOYEE";
            this.Register();
        }

        public void Retrieve(string as_empno)
        {
            string sql = @"select hremployee.coop_id,   
                 hremployee.emp_no,   
                 hremployee.prename_code,   
                 hremployee.emp_name,   
                 hremployee.emp_surname,     
                 hremployee.emptype_code,   
                 hremployee.deptgrp_code,   
                 hremployee.pos_code,   
                 hremployee.sex,
                 hremployee.salary_id
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
            (case when deptgrp_code = '09' then hdg.deptgrp_code || ' - ' || hdg.deptgrp_desc else hdg.deptgrp_code || ' - ' || hdg.deptgrp_desc || ' สายงาน: ' || hdl.deptline_desc end) as deptgrp_desc,
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
    }
}