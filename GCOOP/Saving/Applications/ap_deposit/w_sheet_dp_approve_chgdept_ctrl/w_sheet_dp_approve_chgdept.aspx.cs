using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;

namespace Saving.Applications.ap_deposit.w_sheet_dp_approve_chgdept_ctrl
{
    public partial class w_sheet_dp_approve_chgdept : PageWebSheet, WebSheet
    {
        [JsPostBack]
        public string PostSearch { get; set; }
        public void InitJsPostBack()
        {
            dsMain.InitDsMain(this);
            dsList.InitDsList(this);
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                Begin();
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == PostSearch)
            {
                dsMain.DATA[0].all_check = 0;
                dsList.RetrieveData(state.SsCoopControl, dsMain.DATA[0].select_date, dsMain.DATA[0].end_date);
            }   
        }

        public void SaveWebSheet()
        {
            ExecuteDataSource exedinsert = new ExecuteDataSource(this);
            try {
                string ls_coopid = state.SsCoopId;
                for (int i = 0; i < dsList.RowCount ; i++ ) {
                    if (dsList.DATA[i].choose_flag == 1) {

                        string sqlStr = @"UPDATE DPREQCHG_DEPT SET APPROVE_FLAG=1,deptmontchg_date={2} WHERE APPROVE_FLAG=0
                        AND COOP_ID={0} AND DPREQCHG_DOC={1}";
                        sqlStr = WebUtil.SQLFormat(sqlStr, ls_coopid, dsList.DATA[i].DPREQCHG_DOC,state.SsWorkDate);
                        exedinsert.SQL.Add(sqlStr);
                        
                        sqlStr = @"update 	dpdeptmaster
	                    set			deptmonth_amt			= {4},
				                    deptmonth_amt_old		= {3},
				                    deptmonth_status		= {2}
	                    where	deptaccount_no 			    = {1}
	                    and		coop_id						= {0}";
                        sqlStr = WebUtil.SQLFormat(sqlStr, ls_coopid, dsList.DATA[i].DEPTACCOUNT_NO
                           , dsList.DATA[i].CHANGE_STATUS, dsList.DATA[i].DEPTMONTH_OLDAMT, dsList.DATA[i].DEPTMONTH_NEWAMT);
                        exedinsert.SQL.Add(sqlStr); 
                    }                
                }
                exedinsert.Execute();
                exedinsert.SQL.Clear();
                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลเรียบร้อย");
                Begin();
            }
            catch {
                exedinsert.SQL.Clear();
                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถบันทึกข้อมูลได้");
            }
        }
        public void Begin() {
            dsMain.DATA[0].all_check = 0;
            dsMain.DATA[0].select_date = state.SsWorkDate;
            dsMain.DATA[0].end_date = state.SsWorkDate;
            dsList.RetrieveData(state.SsCoopControl, state.SsWorkDate, state.SsWorkDate);
        }
        public void WebSheetLoadEnd()
        {
            
        }
    }
}