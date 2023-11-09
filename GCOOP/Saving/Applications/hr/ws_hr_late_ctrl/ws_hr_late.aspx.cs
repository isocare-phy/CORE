using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CoreSavingLibrary;

namespace Saving.Applications.hr.ws_hr_late_ctrl
{
    public partial class ws_hr_late : PageWebSheet,WebSheet
    {
    [JsPostBack]
    public string PostEmpNo { get; set; }
        public void InitJsPostBack()
        {
            dsLate.InitDs(this);
            dsMain.InitDs(this);
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack) 
            {
                
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == PostEmpNo)
            {
                string EmpNo = dsMain.DATA[0].EMP_NO;
                dsMain.RetrieveEmp(EmpNo);
                dsLate.RetrieveHrLogLate(EmpNo);
                dsLate.DATA[0].EMP_NO = EmpNo;//นำข้อมูล emp_no จากที่ RetrieveEmp
            }
        }

        public void SaveWebSheet()
        {
            try {
                string sql = "update hrloglate set late_cause={0} where coop_id={1} and emp_no={2} and seq_no = {3}";
                string emp_no = dsMain.DATA[0].EMP_NO;
                string late_cause = "";
                decimal seq_no = 0;
                ExecuteDataSource exed = new ExecuteDataSource(this);
                for (int row = 0; row < dsLate.RowCount; row++) {
                    sql = "update hrloglate set late_cause={0} where coop_id={1} and emp_no={2} and seq_no = {3}";
                    late_cause = dsLate.DATA[row].LATE_CAUSE;
                    seq_no = dsLate.DATA[row].SEQ_NO;
                    sql = WebUtil.SQLFormat(sql, late_cause, state.SsCoopId, emp_no, seq_no);
                    exed.SQL.Add(sql);
                    
                }

                exed.Execute();
                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกสำเร็จ");
            }catch(Exception ex ){
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
            }
        }

        public void WebSheetLoadEnd()
        {
            try {
                for (int i = 0; i < dsLate.RowCount; i++) {
                    DateTime late_time = dsLate.DATA[i].LATE_TIME;
                    string late_time_txt = late_time.ToString("HH:mm:ss");
                    dsLate.DATA[i].LATE_TTIME = late_time_txt;
                }
                
            }catch{
            
            }

        }
    }
}