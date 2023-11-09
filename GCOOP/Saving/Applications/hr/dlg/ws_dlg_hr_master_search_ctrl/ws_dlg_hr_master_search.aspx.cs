using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CoreSavingLibrary;

namespace Saving.Applications.hr.dlg.ws_dlg_hr_master_search_ctrl
{
    public partial class ws_dlg_hr_master_search : PageWebDialog, WebDialog
    {

        public void InitJsPostBack()
        {
            dsCriteria.InitDsCriteria(this);
            dsList.InitDsList(this);
        }

        public void WebDialogLoadBegin()
        {
            if (!IsPostBack)
            {
                dsCriteria.DdDeptgrp();
                dsCriteria.DdPosition();
            }
        }

        public void CheckJsPostBack(string eventArg)
        {

        }

        public void WebDialogLoadEnd()
        {

        }

        protected void BtSearch_Click(object sender, EventArgs e)
        {
            try
            {
                String ls_empno = "", ls_salaryid = "", ls_pos = "";
                String ls_empname = "", ls_empsurname = "", ls_deptgrp = "";                

                string ls_sqlext = "";

                string coop_id = state.SsCoopControl;

                string ls_sql1 = "and mbmembmaster.coop_id ='" + coop_id + "'";

                ls_empno = dsCriteria.DATA[0].EMP_NO.Trim();                
                ls_salaryid = dsCriteria.DATA[0].SALARY_ID.Trim();
                ls_pos = dsCriteria.DATA[0].POS_CODE.Trim();
                ls_empname = dsCriteria.DATA[0].EMP_NAME.Trim();
                ls_empsurname = dsCriteria.DATA[0].EMP_SURNAME.Trim();
                ls_deptgrp = dsCriteria.DATA[0].DEPTGRP_CODE.Trim();

                if (ls_empno.Length > 0)
                {
                    ls_sqlext = " and ( hr.emp_no = '" + ls_empno + "') ";
                }
                if (ls_salaryid.Length > 0)
                {
                    ls_sqlext = " and (  hr.salary_id like '%" + ls_salaryid + "%') ";
                }
                if (ls_empname.Length > 0)
                {
                    ls_sqlext = " and (  hr.emp_name like '%" + ls_empname + "%') ";
                }
                if (ls_empsurname.Length > 0)
                {
                    ls_sqlext += " and ( hr.emp_surname like '%" + ls_empsurname + "%') ";
                }
                if (ls_pos.Length > 0)
                {
                    ls_sqlext += " and ( hr.pos_code like '%" + ls_pos + "%') ";
                }

                if (ls_deptgrp.Length > 0)
                {
                    ls_sqlext += " and ( hr.deptgrp_code = '" + ls_deptgrp + "' )";
                }                

                string sql = sql = @"
                    select hr.emp_no,
                    hr.salary_id,
                    mup.prename_desc || hr.emp_name || '  ' || hr.emp_surname as emp_fullname,
                    hup.pos_desc,
                    hug.deptgrp_desc,
                    hul.deptline_desc
                    from hremployee hr,
                    mbucfprename mup,
                    hrucfposition hup,
                    hrucfdeptgrp hug,
                    hrucfdeptline hul
                    where hr.prename_code = mup.prename_code
                    and hr.pos_code = hup.pos_code
                    and hr.deptgrp_code = hug.deptgrp_code(+)
                    and hug.deptline_code = hul.deptline_code(+)
                    and hr.emp_status = '1'
                    and hr.coop_id = '" + coop_id + "' " + ls_sqlext + " order by emp_no";
                DataTable dt = WebUtil.Query(sql);
                dsList.ImportData(dt);
                LbCount.Text = "ดึงข้อมูลได้ " + dt.Rows.Count + " รายการ";

            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }
    }
}