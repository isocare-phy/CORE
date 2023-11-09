using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataLibrary;
using System.Data;

namespace Saving.Applications.hr.ws_hr_give_child_ctrl
{
    public partial class ws_hr_assist_child : PageWebSheet, WebSheet
    {
        [JsPostBack]
        public string PostEmpno { get; set; }
        [JsPostBack]
        public string PostCalMoney { get; set; }
        [JsPostBack]
        public string PostDeleteRow { get; set; }
        [JsPostBack]
        public String PostInsertRow { get; set; }
        [JsPostBack]
        public String PostCalhalf { get; set; }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == PostEmpno)
            {
                string emp_no = dsMain.DATA[0].emp_no;
                dsMain.Retrieve(emp_no);
                dsDetail.Retrieve(emp_no);


                dsList.Retrieve(emp_no);
                


                dsMain.DATA[0].emp_no = emp_no;
            }

        }

        public void InitJsPostBack()
        {
            dsMain.InitDsMain(this);
            dsList.InitDsList(this);
            dsDetail.InitDsDetail(this);
        }

        public void SaveWebSheet()
        {
            decimal count;
            try
            {
                String coop_id = state.SsCoopId;
                String emp_no = dsMain.DATA[0].emp_no;
                String selectsql = "select max(seq_no) as seq_no from hremployeeassist where emp_no = '" + emp_no + "' ";
                DataTable dt = new DataTable();
                dt = WebUtil.Query(selectsql);
                //Sdt dt = WebUtil.QuerySdt(selectsql);
                try
                {
                    count = Convert.ToDecimal(dt.Rows[0]["seq_no"].ToString().Trim());
                }
                catch
                {
                    count = 0;
                }
               
                decimal seq_no = count + 1;
                String assist_code = "03";
                decimal money = dsDetail.DATA[0].approve_money;
                String assist_desc = "จำนวน " + dsDetail.DATA[0].desc + " คน";
                String assist_remark = dsDetail.DATA[0].remark;
                decimal assist_learn = 0;
                decimal assist_use = 0;
                decimal assist_draw = 0;
                String assist_school = " ";

                int row_selecttax = dt.Rows.Count;

                String sql = @"insert into hremployeeassist(coop_id,emp_no,seq_no,assist_code,assist_date
                                    ,assist_desc,assist_amt,assist_remark,assist_learn,assist_use,
                                     assist_draw,assist_school)
                                         values({0},{1},{2},{3},sysdate,{4},{5},{6},{7},{8},{9},{10})";
                sql = WebUtil.SQLFormat(sql, coop_id, emp_no, seq_no, assist_code, assist_desc, money, assist_remark, 
                    assist_learn, assist_use,assist_draw, assist_school);
                DataTable dt2 = new DataTable();
                dt2 = WebUtil.Query(sql);

                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลสำเร็จ");
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }

            string as_empno = dsMain.DATA[0].emp_no;
            Decimal as_page = dsDetail.DATA[0].page;
            string as_occupa = dsDetail.DATA[0].occupation;
            string as_emogrp = dsDetail.DATA[0].empgroup;
            string as_office = dsDetail.DATA[0].office;
            string as_location = dsDetail.DATA[0].location;
            string as_rephur = dsDetail.DATA[0].regis_amphur;
            string as_reno = dsDetail.DATA[0].regis_no;
            string as_redate = dsDetail.DATA[0].regis_date;
            string as_remark = dsDetail.DATA[0].remark;
            Decimal as_appmon = dsDetail.DATA[0].approve_money;
            string as_month = dsDetail.DATA[0].month;
            string as_desc = dsDetail.DATA[0].desc;

            iReportArgument args = new iReportArgument();
            args.Add("as_coopid", iReportArgumentType.String, state.SsCoopId);
            args.Add("as_empno", iReportArgumentType.String, as_empno);
            args.Add("as_page", iReportArgumentType.Integer, Convert.ToInt32(as_page));
            args.Add("as_occupa", iReportArgumentType.String, as_occupa);
            args.Add("as_emogrp", iReportArgumentType.String, as_emogrp);
            args.Add("as_office", iReportArgumentType.String, as_office);
            args.Add("as_location", iReportArgumentType.String, as_location);
            args.Add("as_rephur", iReportArgumentType.String, as_rephur);
            args.Add("as_reno", iReportArgumentType.String, as_reno);
            args.Add("as_redate", iReportArgumentType.String, as_redate);
            args.Add("as_remark", iReportArgumentType.String, as_remark);
            args.Add("as_appmon", iReportArgumentType.Integer, Convert.ToInt32(as_appmon));
            args.Add("as_month", iReportArgumentType.String, as_month);
            args.Add("as_desc", iReportArgumentType.String, as_desc);

            iReportBuider report = new iReportBuider(this, "กำลังออกรายงาน.....");
            report.AddCriteria("r_hr_give_child", "คำขอรับเงินช่วยเหลือบุตร", ReportType.pdf, args);
            report.AutoOpenPDF = true;
            report.Retrieve();

        }

        public void WebSheetLoadBegin()
        {

        }

        public void WebSheetLoadEnd()
        {

        }

    }
}