using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using System.Data;

namespace Saving.Applications.hr.ws_hr_worktime_ctrl
{
    public partial class DsList : DataSourceRepeater
    {
        public DataSet1.HRLOGWORKTIMEDataTable DATA { get; private set; }
        public void InitDs(PageWeb pw)
        {
            css1.Visible = false;
            DataSet1 ds = new DataSet1();
            this.DATA = ds.HRLOGWORKTIME;
            this.EventItemChanged = "OnDsListItemChanged";
            this.EventClicked = "OnDsListClicked";
            this.InitDataSource(pw, Repeater1, this.DATA, "dsList");

            //กรณีมีปุ่มให้ใช้คำสั่งนี้ add เพื่อให้เกิด event clicked ที่ปุ่ม
            this.Button.Add("b_del");//ถ้าหน้าจอไม่มีปุ่มให้ลบหรือคอมเมนท์บรรทัดนี้
            //this.Button.Add("b_search");
            this.Register();
        }

        public void RetrieveLogWorkDate(DateTime work_date)
        {
            string sql = @"
                select hw.emp_no,mp.prename_desc,he.emp_name,he.emp_surname,hw.start_time,hw.end_time,hwc.worktime_code
	            from hremployee he,mbucfprename mp,hrlogworktime hw,hrucfworktimecode hwc
                where hw.work_date = {0}
                and hw.coop_id = {1}
                and he.emp_no = hw.emp_no
                and he.prename_code=mp.prename_code
                and hw.worktime_code = hwc.worktime_code
			    and he.work_date <= {0}
                order by emp_no";
            sql = WebUtil.SQLFormat(sql, work_date, state.SsCoopId);
            DataTable dt = WebUtil.Query(sql);            
            this.ImportData(dt);
            this.SetTimeAfRetrieve();
            this.Ddhrucfworktimecode();
        }

        public void SetTimeAfRetrieve() {

            if (state.SsCoopControl == "031001")
            {

                for (int r = 0; r < this.RowCount; r++)
                {
                    this.DATA[r].START_HOUR = Convert.ToInt16("8");
                    this.DATA[r].START_MINUTE = Convert.ToInt16("30");
                    this.DATA[r].END_HOUR = Convert.ToInt16("16");
                    this.DATA[r].END_MINUTE = Convert.ToInt16("30");

                }

            }
            else
            {

                for (int r = 0; r < this.RowCount; r++)
                {
                    this.DATA[r].START_HOUR = Convert.ToInt16("8");
                    this.DATA[r].START_MINUTE = Convert.ToInt16("0");
                    this.DATA[r].END_HOUR = Convert.ToInt16("16");
                    this.DATA[r].END_MINUTE = Convert.ToInt16("0");

                }
            }
        
        }

        public void RetrieveShowEmp(DateTime WDate)
        {
            string sql = @"
                select he.emp_no,
                mp.prename_desc,
                he.emp_name,
                he.emp_surname,
                TO_NUMBER(TO_CHAR(ht.start_time , 'hh24')) as start_time1,
                TO_NUMBER(TO_CHAR(ht.start_time , 'mi')) as start_time2,
                TO_NUMBER(TO_CHAR(ht.end_time , 'hh24')) as end_time1,
                TO_NUMBER(TO_CHAR(ht.end_time , 'mi')) as end_time2,
                ht.worktime_code,
                ht.work_date
                from hremployee he, mbucfprename mp , hrlogworktime ht
                where he.prename_code = mp.prename_code							    
                and ht.emp_no(+) = he.emp_no
                and he.emp_status = 1							  
                and he.coop_id = {0}
                and ht.work_date(+) = {1}
                and he.work_date <= {1} 
                order by emp_no";

            sql = WebUtil.SQLFormat(sql, state.SsCoopId, WDate);
            DataTable dt = WebUtil.Query(sql);
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                this.InsertLastRow();
                this.DATA[i].COOP_ID = state.SsCoopId;
                this.DATA[i].WORK_DATE = WDate;  
                this.DATA[i].EMP_NO = dt.Rows[i]["emp_no"].ToString();
                this.DATA[i].PRENAME_DESC = dt.Rows[i]["prename_desc"].ToString();
                this.DATA[i].EMP_NAME = dt.Rows[i]["emp_name"].ToString();
                this.DATA[i].EMP_SURNAME = dt.Rows[i]["emp_surname"].ToString();
                this.DATA[i].START_HOUR = Convert.ToInt16(dt.Rows[i]["start_time1"]);
                this.DATA[i].START_MINUTE = Convert.ToInt16(dt.Rows[i]["start_time2"]);
                this.DATA[i].END_HOUR = Convert.ToInt16(dt.Rows[i]["end_time1"]);
                this.DATA[i].END_MINUTE = Convert.ToInt16(dt.Rows[i]["end_time2"]);
                this.DATA[i].WORKTIME_CODE = dt.Rows[i]["worktime_code"].ToString();
            }
            //this.ImportData(dt);
            //this.SetTimeAfRetrieve();
            this.Ddhrucfworktimecode();
        }

    

        public void RetrieveShowEmp2(DateTime WDate)
        {
            string sql = @"
                select he.emp_no,
                mp.prename_desc,
                he.emp_name,
                he.emp_surname,
                ht.start_time,
                ht.end_time,
                ht.worktime_code,
                ht.work_date
                from hremployee he, mbucfprename mp , hrlogworktime ht
                where he.prename_code = mp.prename_code							    
                and ht.emp_no(+) = he.emp_no
                and he.emp_status = 1							  
                and he.coop_id = {0}
                and ht.work_date(+) = {1}
                and he.work_date <= {1}
                order by emp_no";

            sql = WebUtil.SQLFormat(sql, state.SsCoopId, WDate);
            DataTable dt = WebUtil.Query(sql);
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                this.InsertLastRow();
                this.DATA[i].COOP_ID = state.SsCoopId;
                this.DATA[i].WORK_DATE = WDate;
                this.DATA[i].EMP_NO = dt.Rows[i]["emp_no"].ToString();
                this.DATA[i].PRENAME_DESC = dt.Rows[i]["prename_desc"].ToString();
                this.DATA[i].EMP_NAME = dt.Rows[i]["emp_name"].ToString();
                this.DATA[i].EMP_SURNAME = dt.Rows[i]["emp_surname"].ToString();
                this.DATA[i].WORKTIME_CODE = "NML";


            }
            //this.ImportData(dt);
            this.SetTimeAfRetrieve();
            this.Ddhrucfworktimecode();
        }

        public void Ddhrucfworktimecode()
        {
            string sql = @"
                select worktime_code,worktime_desc from hrucfworktimecode";
                //union
                //select '','',0 from dual order by sorter
            DataTable dt = WebUtil.Query(sql);
            this.DropDownDataBind(dt, "worktime_code", "worktime_desc", "worktime_code");
        }//คิวรี่ ประเภทการทำงาน
    }
}